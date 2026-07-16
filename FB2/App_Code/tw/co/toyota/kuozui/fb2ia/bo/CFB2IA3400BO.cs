using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;

/// <summary>
/// CFB2IA3400BO 的摘要描述
/// </summary>
public class CFB2IA3400BO : BaseService
{
    public CFB2IA3400BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public string Approve(List<string> appitem_list, List<string> APP_REMARK_list, List<string> qdata2_list)
    {
        CFB2IA3400DAO fb2ia = new CFB2IA3400DAO();
        try
        {
            //qdata2_list去除重複的值
            for (int i = qdata2_list.Count - 1; i >= 0; --i)
            {
                if (qdata2_list.IndexOf(qdata2_list[i]) != i)

                    qdata2_list.RemoveAt(i);
            }
            //appitem_list→string
            string combindedString = string.Join(",", appitem_list);  //SALARY_YM+EMP_ID+INS_TYPE+IDENTITY_KIND+LICENSE_ID(key)
            BeginTransaction();
            for (int k = 0; k < qdata2_list.Count; k++)
            {
                string qdata2 = qdata2_list[k];    //SALARY_YM+EMP_ID+INS_TYPE(加總依據)
                decimal sum = fb2ia.Calculate(combindedString, qdata2);
                //sum必須為絕對值
                //if (sum < 0)
                //    sum = sum * -1;
                if (sum != 0)
                    fb2ia.TB_S_M_SUBSIDY_DEDUCTIONS_1(combindedString, qdata2, sum,0);
            }
            for (int i = 0; i < appitem_list.Count; i++)
            {
                string APP_REMARK = APP_REMARK_list[i];
                string delitem = appitem_list[i];  //SALARY_YM+EMP_ID+INS_TYPE+IDENTITY_KIND+LICENSE_ID+TRACE_KIND(key)
                fb2ia.Approve(delitem, APP_REMARK);
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
        CFB2IA3400DAO fb2ia = new CFB2IA3400DAO();
        try
        {
            BeginTransaction();
            for (int i = 0; i < rejitem_list.Count; i++)
            {
                string APP_REMARK = APP_REMARK_list[i];
                string rejitem = rejitem_list[i];
                fb2ia.Reject(rejitem, APP_REMARK);
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