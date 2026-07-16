using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;

/// <summary>
/// CFB2SF0150BO 的摘要描述
/// </summary>
public class CFB2SF0150BO : BaseService
{
    public CFB2SF0150BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public string Approve(List<string> appitem_list, List<string> emp_id_list, List<string> APP_REMARK_list)
    {
        CFB2SF0150DAO fb2sf = new CFB2SF0150DAO();
        try
        {
            //emp_id_list去除重複的值
            for (int i = emp_id_list.Count - 1; i >= 0; --i)
            {
                if (emp_id_list.IndexOf(emp_id_list[i]) != i)

                    emp_id_list.RemoveAt(i);
            }

            
            BeginTransaction();
            for (int i = 0; i < appitem_list.Count; i++)
            {
                string APP_REMARK = APP_REMARK_list[i];
                string appitem = appitem_list[i];
                fb2sf.Update_TB_S_M_ARREARS_COURT_H(appitem, APP_REMARK);
                fb2sf.Del_TB_S_M_ARREARS_TARGET(appitem);
                fb2sf.Update_TB_S_M_ARREARS_TARGET(appitem);
            }
            Commit();
            BeginTransaction();
            for (int k = 0; k < emp_id_list.Count; k++)
            {
                string emp_id = emp_id_list[k];

                //取出總金額
                int TOTAMOUNT = fb2sf.GET_TOTAMOUNT(emp_id);
                //找出須須異動債權比例的金額
                DataTable dt = fb2sf.GET_EACHAMOUNT(emp_id);
                decimal currentrate = 100;      //v目前可用比例
                decimal ratio = 0;              //v債權比例
                decimal count = dt.Rows.Count;  //v總比數
                decimal vi = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string amountkey = Convert.ToString(dt.Rows[i]["AMOUNTKEY"]);
                    vi = vi + 1;
                    if (vi == count)
                        ratio = currentrate;
                    else
                    {
                        ratio = Math.Floor(((decimal)dt.Rows[i]["AMOUNT"] / TOTAMOUNT) * 100);   //無條件捨去至整數位
                        //20150908 增加
                        if (ratio == 0) {
                            ratio = 1;
                        }
                        currentrate = currentrate - ratio;
                    }
                    fb2sf.Update_RATIO(amountkey, ratio);
                }
            }
            Commit();
            return "0";
        }
        catch (Exception)
        {
            RollBack();
            throw;
        }
    }

    public string Reject(List<string> rejitem_list, List<string> APP_REMARK_list)
    {
        CFB2SF0150DAO fb2sf = new CFB2SF0150DAO();
        try
        {
            BeginTransaction();
            for (int i = 0; i < rejitem_list.Count; i++)
            {
                string APP_REMARK = APP_REMARK_list[i];
                string rejitem = rejitem_list[i];
                fb2sf.Reject(rejitem, APP_REMARK);
            }
            Commit();
            return "0";
        }
        catch (Exception)
        {
            RollBack();
            throw;
        }
    }
}