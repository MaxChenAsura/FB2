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
/// CFB2DC0100BO 的摘要描述
/// </summary>
public class CFB2DC0100BO : BaseService
{
	public CFB2DC0100BO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    //刪除 TB_D_M_CLOCK 卡鐘情報檔
    public string deleteCLOCK(List<string> clock_no)
    {
        try
        {
            CFB2DC0100DAO wfb2dc = new CFB2DC0100DAO();
            BeginTransaction();
            foreach (string item in clock_no)
            {
                wfb2dc.deleteCLOCK(item);
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

    //新增 TB_D_M_CLOCK 卡鐘情報檔
    public string addCLOCK(CFB2DC0100DAO wfb2dc)
    {
        try
        {

            //錯誤訊息
            string rtnmessage = "";
            //若需要則要進行邏輯檢查
            DataTable dt = new DataTable();
            //1.檢查 卡鐘IP
            dt = wfb2dc.check_CLOCK_IP();
            if ((int)dt.Rows[0]["resultCount"] > 0)
            {
                rtnmessage += "卡鐘IP 重覆 \\n";
                return rtnmessage;
            }

            //取得現有資料
            DataTable tmp = wfb2dc.getExistData();

            if (tmp.Rows.Count > 0)
                return "卡鐘編號重覆";
            else
            {
                BeginTransaction();
                wfb2dc.addCLOCK();
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

    //更新 TB_D_M_CLOCK 卡鐘情報檔
    public string updateCLOCK(CFB2DC0100DAO wfb2dc)
    {
       
        try
        {
            //錯誤訊息
            string rtnmessage = "";
            //若需要則要進行邏輯檢查
            DataTable dt = new DataTable();
            //1.檢查 卡鐘IP
            dt = wfb2dc.check_CLOCK_IP();
            if ((int)dt.Rows[0]["resultCount"] > 0)
            {
                rtnmessage += "卡鐘IP 重覆 \\n";
                return rtnmessage;
            }

            BeginTransaction();

            wfb2dc.updateCLOCK();

            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }


    public DataTable getCLOCK_DESC(string clock_no)
    {
        try
        {
            CFB2DC0100DAO wfb2dc = new CFB2DC0100DAO();
            return wfb2dc.getCLOCK_DESC(clock_no);
        }
        catch (Exception)
        {

            throw;
        }
    }
}