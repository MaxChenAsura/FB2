using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;

/// <summary>
/// CFB2HB0300BO 的摘要描述
/// </summary>
public class CFB2HB0300BO : BaseService
{
	public CFB2HB0300BO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    public string getEmpName(string emp_id)
    {
        try
        {
            CFB2HB0400DAO dao = new CFB2HB0400DAO();
            DataTable dt = dao.getEmpName(emp_id);
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["EMP_NAME"].ToString();
            }
            else
                return "";
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getiniData(string emp_id, string hr_chg_no)
    {
        try
        {
            CFB2HB0300DAO hb030DAO = new CFB2HB0300DAO();
            return hb030DAO.getiniData(emp_id, hr_chg_no);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //修改
    public string update(CFB2HB0300DAO hb030DAO)
    {
        try
        {
            //其部門代號需存在
            string rtnmessage = "";
            if (hb030DAO.CHK_END_DT == "")
            {
                hb030DAO.CHK_END_DT = DateTime.Now.ToString("yyyy/MM/dd");               
            }
            if (hb030DAO.chkDEPT() == 0)
                rtnmessage += "該部門代號未生效!! \\n";

            if (rtnmessage != "")
            {
                return rtnmessage;
            }

            BeginTransaction();
            hb030DAO.update();
            hb030DAO.update_CHG_H();
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