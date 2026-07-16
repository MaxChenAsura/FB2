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
/// CFB2DA0300BO 的摘要描述
/// </summary>
public class WFB2DA0300BO : BaseService
{
	public WFB2DA0300BO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    public string deleteData(List<Tuple<string, string, string>> deleteList)
    {
        try
        {
            WFB2DA0300DAO dao = new WFB2DA0300DAO();
            BeginTransaction();

            foreach (var deleteitem in deleteList)
            {
                //刪除主檔資料
                dao.deleteData(deleteitem.Item1, deleteitem.Item2, deleteitem.Item3);
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

    public DateTime getFN_S_DUTY_EDT()
    {
        try
        {
            WFB2DA0300DAO dao = new WFB2DA0300DAO();
            return dao.getFN_D_DUTY_CLOSE_D();
        }
        catch (Exception)
        {

            throw;
        }
    }

    public string SP_DA030_01(WFB2DA0300DAO dao)
    {
        try
        {
            string result = "0";
            //call sp
            int err = dao.SP_DA030_01(dao);

            //確認SP有無成功
            DataTable dtSPresult = dao.checkSP("SP_DA030_01");
            if (dtSPresult.Rows.Count > 0)
            {
                //PROC_STATUS：Y = 成功,N = 失敗,E = Exception  ,PROC_LOG：處理結果中文訊息
                if (Convert.ToString(dtSPresult.Rows[0]["PROC_STATUS"]) != "Y")
                    return Convert.ToString(dtSPresult.Rows[0]["PROC_LOG"]) + "\\n";
            }

            return result;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public string getCALENDAR_DT(string calendar_cd, string calendar_dt)
    {
        try
        {
            string result = "";
            WFB2DA0300DAO dao = new WFB2DA0300DAO();
            DataTable dt = dao.getCALENDAR_DT(calendar_cd, calendar_dt);
            if (dt.Rows.Count > 0)
            {
                result = dt.Rows[0]["DT_TYPE"].ToString();
            }

            return result;
        }
        catch (Exception)
        {
            
            throw;
        }
    }

    public string addData(WFB2DA0300DAO dao)
    {
        try
        {
            //取得現有資料
            if (dao.CALENDAR_CD !="All")
            {
                if (dao.getExistData())
                {
                    return "行事曆+日期+日期類型(原) 已存在!";
                }

                BeginTransaction();
                dao.addData();
                Commit();
            }
            else
            {
                DataTable dt = new DataTable();
                dt = dao.get_CALENDAR_DT_TYPE();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dao.CALENDAR_CD = dt.Rows[i]["CALENDAR_CD"].ToString();
                    dao.DT_TYPE_O = dt.Rows[i]["DT_TYPE"].ToString();
                    if (dao.getExistData())
                    {
                        return "行事曆+日期+日期類型(原) 已存在!";
                    }
                }

                BeginTransaction();
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dao.CALENDAR_CD = dt.Rows[i]["CALENDAR_CD"].ToString();
                    dao.DT_TYPE_O = dt.Rows[i]["DT_TYPE"].ToString();
                    dao.addData();
                }
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

    public string getTB_D_M_CALENDAR_ADJ(WFB2DA0300DAO dao)
    {
        try
        {
            if (dao.getTB_D_M_CALENDAR_ADJ())
            {
                return "已存在";
            } 

            return "0";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
}