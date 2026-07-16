using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text;

/// <summary>
/// CFB2SA2300BO 的摘要描述
/// </summary>
public class CFB2SA2300BO : BaseService
{
	public CFB2SA2300BO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    //敘薪資料產生
    public string execSP_S_SALARY_DATA_EXEC(List<CFB2SA2300DAO> dao)
    {
        string t = "", desc = "", msg = "";
        DataTable dt= new DataTable();
        try
        {
            for (int i = 0; i < dao.Count; i++)
            {
                dao[i].execSP_S_SALARY_DATA_EXEC() ;
                dt = dao[i].check_SP_Status();

                if (dt.Rows.Count > 0)
                {
                    t = dt.Rows[0]["PROC_STATUS"].ToString();
                    desc = dt.Rows[0]["PROC_LOG"].ToString();
                    if (t != "Y")
                        msg += "\n工號 " + dao[i].EMP_ID.ToString() + " 執行失敗! " + desc;
                }
            }

            return msg;
        }
        catch (Exception)
        {
            throw;
        }
    }
    
}