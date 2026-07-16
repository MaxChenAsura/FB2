using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;

/// <summary>
/// CFB2SH010BO 的摘要描述
/// </summary>
public class CFB2990500BO : BaseService
{
	public CFB2990500BO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}


    //新增
    public string insertData(CFB2990500DAO fb299050DAO)
    {
        string rtnmessage = "";
        try
        {

            //若需要則要進行邏輯檢查(與DB相關的)
            //00.檢查PK值有無重覆
            DataTable dupdata = fb299050DAO.getPKData();
            if ((int)dupdata.Rows[0]["resultCount"] > 0)
            {
                rtnmessage += "新主假別及子假別代號重覆\\n";
            }
            //01檢查新子假別是否與新主假別對應
            dupdata = fb299050DAO.getSubLevel();
            if ((int)dupdata.Rows[0]["resultCount"] == 0)
            {
                rtnmessage += "新主假別無該子假別或不存在 \\n";
            }

            //02檢查新主假別,新子假別,舊主假別代號是否存在
            dupdata = fb299050DAO.getMainLevel();
            if ((int)dupdata.Rows[0]["resultCount"] == 0)
            {
                rtnmessage += "新主假別不存在\\n";
            }
            dupdata = fb299050DAO.getOldMainLevel();
            if ((int)dupdata.Rows[0]["resultCount"] == 0)
            {
                rtnmessage += "舊主假別代號不存在\\n";
            }

            //檢查OK更新
            if (rtnmessage == "")
            {
                try
                {
                    BeginTransaction();

                    fb299050DAO.insertData();

                    Commit();

                    return "0";
                }
                catch (Exception ex)
                {
                    RollBack();
                    return ex.Message;
                }

            }
            return rtnmessage;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    //修改
    public string updateData(CFB2990500DAO dao)
    {
        string rtnmessage = "";
        try
        {

            //若需要則要進行邏輯檢查

            //檢查OK更新
            if (rtnmessage == "")
            {
                try
                {
                    BeginTransaction();

                    dao.updateData();

                    Commit();

                    return "0";
                }
                catch (Exception ex)
                {
                    RollBack();
                    return ex.Message;
                }

            }
            return rtnmessage;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    //刪除
    public string deleteData(List<Tuple<string, string>> keysList)
    {
        CFB2990500DAO fb299050DAO = new CFB2990500DAO();
        string rtnmessage = "";//存在檢查後的訊息
        try
        {

            //檢查完成後，逐筆進行刪除
            if (rtnmessage == "")
            {
                try
                {
                    BeginTransaction();
                    foreach (var item in keysList)
                    {
                        fb299050DAO.TMLCD = item.Item1;
                        fb299050DAO.TSLCD = item.Item2;
                        fb299050DAO.deleteData();
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
            else
            {
                return rtnmessage;
            }

        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    //取得主假別說明
    public DataTable getMAIN_LEAVE_DESC(string main_leave_cd)
    {
        try
        {
            CFB2990500DAO wfb2DAO = new CFB2990500DAO();
            return wfb2DAO.getMAIN_LEAVE_DESC(main_leave_cd);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //取得子假別說明
    public DataTable getSUB_LEAVE_DESC(string sub_leave_cd)
    {
        try
        {
            CFB2990500DAO wfb2DAO = new CFB2990500DAO();
            return wfb2DAO.getSUB_LEAVE_DESC(sub_leave_cd);
        }
        catch (Exception)
        {
            throw;
        }
    }
   
}