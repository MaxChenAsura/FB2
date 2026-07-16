using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;

/// <summary>
/// CFB2SA1510BO 的摘要描述
/// </summary>
public class CFB2SA1510BO : BaseService
{
    public CFB2SA1510BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public string excute(CFB2SA1510DAO dao)
    {
        try
        {
            
            DataTable dt = dao.getMailList();
            DataTable EmpEmail = dao.checkEmpEmail();
            if (EmpEmail.Rows.Count > 0)
            {
                dao.SENDTO_MAIL = EmpEmail.Rows[0]["SALARY_EMAIL"].ToString();
            }
            if (dao.CC_EMAIL != "")
            {
                DataTable CCEmail = dao.checkCCEmail();
                if (CCEmail.Rows.Count > 0)
                {
                    dao.CC_EMAIL_TO = CCEmail.Rows[0]["SALARY_EMAIL"].ToString();
                }
            }
            
            if (dt.Rows.Count > 0)
            {
                BeginTransaction();
                //依查詢條件刪除[初任薪作業MAIL主檔/初任薪作業MAIL明細檔]
                dao.deleteMAIL_BAT_H();
                dao.deleteMAIL_BAT_D();

                //新增[初任薪作業MAIL主檔]
                dao.addMAIL_BAT_H();

                //依查詢資料寫入 [初任薪作業MAIL明細檔] 
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dao.addMAIL_BAT_D(dt.Rows[i]["EMP_ID"].ToString(), dt.Rows[i]["SALARY_EMAIL"].ToString());
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

    public string checkSign(string DATA_YEAR)
    {
        try
        {
            //檢查指定年度是否已完成簽核動作
            string errMessage = "";
            CFB2SA1510DAO dao = new CFB2SA1510DAO();
            DataTable dt = dao.checkSign(DATA_YEAR);
            if ((int)dt.Rows[0]["cnt"] == 0)
            {
                errMessage = "年度:" + DATA_YEAR + "未完成簽核作業,不允執行此功能。";
            }

            return errMessage;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public string checkEmpEmail()
    {
        try
        {
            //檢查執行者是否有mail 帳號
            string errMessage = "";
            CFB2SA1510DAO dao = new CFB2SA1510DAO();
            DataTable dt = dao.checkEmpEmail();
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["SALARY_EMAIL"].ToString() == "")
                    errMessage = "你本人尚未設定MAIL帳號,無法執行此功能";
            }
            return errMessage;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public string checkRecvEmail(string DATA_YEAR, string EMP_ID)
    {
        try
        {
            //檢查是否有初任薪名單中,發薪MAIL 為空白的人員且不為日籍會社
            string errMessage = "是否確定執行發放MAIL通知功能:";
            string vStr_desc = "";
            string vStr_desc1 = "初任薪調薪名單中MAIL 空白的名單如下: \n";

            CFB2SA1510DAO dao = new CFB2SA1510DAO();
            DataTable dt = dao.checkRecvEmail(DATA_YEAR, EMP_ID);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    vStr_desc += "工號:" + dt.Rows[i]["EMP_ID"].ToString() + "--" + dt.Rows[i]["EMP_NAME"].ToString() + " \n";
                }
            }
            return errMessage + vStr_desc1 + vStr_desc;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public DataTable get_PDF_Data(CFB2SA1510DAO dao)
    {
        try
        {
            return dao.get_PDF_Data();
        }
        catch (Exception)
        {

            throw;
        }
    }
    public DataTable getSalaryYM()
    {
        try
        {
            CFB2SA1510DAO dao = new CFB2SA1510DAO();
            return dao.getSalaryYM();
        }
        catch (Exception)
        {

            throw;
        }
    }
    public DataTable getvaaData()
    {
        try
        {
            CFB2SA1510DAO dao = new CFB2SA1510DAO();
            return dao.getvaaData();
        }
        catch (Exception)
        {

            throw;
        }
    }
    public DataTable getvbbData()
    {
        try
        {
            CFB2SA1510DAO dao = new CFB2SA1510DAO();
            return dao.getvbbData();
        }
        catch (Exception)
        {

            throw;
        }
    }
    public DataTable before_Pay(CFB2SA1510DAO dao)
    {
        try
        {
            
            return dao.before_Pay();
        }
        catch (Exception)
        {

            throw;
        }
    }
    public DataTable after_Pay(CFB2SA1510DAO dao)
    {
        try
        {

            return dao.after_Pay();
        }
        catch (Exception)
        {

            throw;
        }
    }
}