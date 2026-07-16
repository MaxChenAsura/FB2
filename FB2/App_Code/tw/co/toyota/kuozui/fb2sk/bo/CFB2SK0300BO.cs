using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;

/// <summary>
/// CFB2SK0300BO 的摘要描述
/// </summary>
public class CFB2SK0300BO : BaseService
{
	public CFB2SK0300BO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    public string Add_S_K_MUTUAL(CFB2SK0300DAO fb2sk)
    {
        try
        {
            //取得現有資料(檢查重複)
            DataTable tmp = fb2sk.getExistData();
            BeginTransaction();
            if (tmp.Rows.Count > 0)
            {
                return "年月重複!";
            }
            else
            {
                fb2sk.Add_S_K_MUTUAL();
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
    public string Update_S_K_MUTUAL(CFB2SK0300DAO fb2sk)
    {
        try
        {
            BeginTransaction();
            fb2sk.Update_S_K_MUTUAL();
            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    public string Delete_S_K_MUTUAL(List<string> DATA_YMs)
    {
        CFB2SK0300DAO fb2sk = new CFB2SK0300DAO();
        try
        {
            BeginTransaction();
            foreach (string DATA_YM in DATA_YMs)
            {
                fb2sk.Delete_S_K_MUTUAL(DATA_YM);
            }
            Commit();

            return "0";
        }
        catch (Exception)
        {
            throw;
        }
    }
    public string Release_S_K_MUTUAL(CFB2SK0300DAO sk030DAO)
    {
        try
        {

            //檢核能否薪資轉出
            DataTable dt = new DataTable();
            dt = sk030DAO.checkDataExist();
            if (dt.Rows.Count == 0)
            {
                return "薪資類別尚未建立最新月薪";
            }
            if (dt.Rows.Count > 0)
            {
                string PROCESS_STATUS = dt.Rows[0]["PROCESS_STATUS"].ToString();
                if (PROCESS_STATUS != "1")
                {
                    return "該年月薪資已經鎖定，請將本月的互助金合併於次月。";
                }
                sk030DAO.SALARY_DT = dt.Rows[0]["SALARY_DT"].ToString() != "" ? Convert.ToDateTime(dt.Rows[0]["SALARY_DT"].ToString()).ToString("yyyy/MM/dd") : "";
                sk030DAO.START_DT = dt.Rows[0]["SALARY_SDT"].ToString() != "" ? Convert.ToDateTime(dt.Rows[0]["SALARY_SDT"].ToString()).ToString("yyyy/MM/dd") : "";
                sk030DAO.END_DT = dt.Rows[0]["SALARY_EDT"].ToString() != "" ? Convert.ToDateTime(dt.Rows[0]["SALARY_EDT"].ToString()).ToString("yyyy/MM/dd") : "";
            }


          

            BeginTransaction();

            //新增 薪資月結控制檔(TB_S_M_SALARY_MONTH_CTRL)
            sk030DAO.insert_SALARY_MONTH_CTRL();
            //更新 互助金資料設定檔
            sk030DAO.Release_S_K_MUTUAL();

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