using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

/// <summary>
/// WFB2IA0100Service 的摘要描述
/// </summary>
public class CFB2IA0100BO : BaseService
{
    public CFB2IA0100BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    //新增 TB_I_M_REDUCE
    public string addREDUCE(CFB2IA0100DAO wfb2ia)
    {
        try
        {
            //取得現有資料
            DataTable tmp = wfb2ia.getExistData();
            DataTable tmp2 = wfb2ia.getMaxEndDTByType();
            if (tmp.Rows.Count > 0)
                return "減免代碼+生效日期 資料重覆!";
            if (tmp2.Rows.Count > 0 && tmp2.Rows[0]["maxEndDT"].ToString()!="")
            {
                if (Convert.ToDateTime(wfb2ia.EFFECT_DT) < Convert.ToDateTime(tmp2.Rows[0]["maxEndDT"]))
                    return "生效日期重疊!";
                else
                {
                    BeginTransaction();
                    wfb2ia.addREDUCE();
                    Commit();
                }
            }

            else
            {
                BeginTransaction();
                wfb2ia.addREDUCE();
                Commit();
            }

            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    //更新 TB_I_M_REDUCE
    public string updateREDUCE(CFB2IA0100DAO wfb2ia)
    {
        try
        {
            BeginTransaction();

            wfb2ia.updateREDUCE();

            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    //刪除 TB_I_M_REDUCE
    public string deleteREDUCE(List<Tuple<string, string>> reduce_cd)
    {
        try
        {
            CFB2IA0100DAO wfb2ia = new CFB2IA0100DAO();
            BeginTransaction();
            foreach (var item in reduce_cd)
            {
                wfb2ia.deleteREDUCE(item.Item1, item.Item2);
            }
            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
}