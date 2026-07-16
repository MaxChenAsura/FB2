using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;

/// <summary>
/// CFB2SI0200BO 的摘要描述
/// </summary>
public class CFB2SI0200BO : BaseService
{
    public CFB2SI0200BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public string Update(CFB2SI0200DAO fb2si, string BONUS_ITEM_RP, string BONUS_ITEM_AL, string BONUS_ITEM_D, string yearDays)
    {
        try
        {
            
            DataTable tmp = fb2si.getEmpData();
            DateTime now = DateTime.Parse(DateTime.Now.ToString());
            BeginTransaction();
            for (int i = 0; i < tmp.Rows.Count; i++)
            {
                fb2si.EMP_ID = Convert.ToString(tmp.Rows[i]["EMP_ID"]);
                //紅利紅利明細維護檔
                fb2si.Update("TB_S_M_BONUS_D", BONUS_ITEM_RP, BONUS_ITEM_AL, BONUS_ITEM_D, yearDays, now);
                //紅利明細原始檔
                fb2si.UpdateDefault(now);

                if (i % 500 == 0)
                {
                    Commit();
                    BeginTransaction();
                }
            }
            //更新 紅利明細主檔
            fb2si.UpdateMain(BONUS_ITEM_RP, BONUS_ITEM_AL, BONUS_ITEM_D, yearDays, now);
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