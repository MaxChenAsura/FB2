using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;

/// <summary>
/// CFB2SA1510BO 的摘要描述
/// </summary>
public class CFB2SM1400BO : BaseService
{
    public CFB2SM1400BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public string excute(CFB2SM1400DAO dao)
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
                //依查詢條件刪除[晉昇作業MAIL主檔/晉昇作業MAIL明細檔]
                dao.deleteMAIL_BAT_H();
                dao.deleteMAIL_BAT_D();

                //新增[晉昇作業MAIL主檔]
                dao.addMAIL_BAT_H();

                //依查詢資料寫入 [晉昇作業MAIL明細檔] 
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

    public string checkSign(string DATA_YEAR, string DATA_SEQ)
    {
        try
        {
            //檢查指定年度是否已完成簽核動作
            string errMessage = "";
            CFB2SM1400DAO dao = new CFB2SM1400DAO();
            DataTable dt = dao.checkSign(DATA_YEAR, DATA_SEQ);
            if (dt.Rows[0]["PROCESS_STATUS"].ToString() !="Y" && dt.Rows[0]["RELEASE_DT"].ToString() != null)
            {
                errMessage = "年度:" + DATA_YEAR + "回數:" + DATA_SEQ + "未完成簽核及發佈作業,不允執行此功能。";
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
            CFB2SM1400DAO dao = new CFB2SM1400DAO();
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
            //檢查是否有晉昇名單中,發薪MAIL 為空白的人員
            string errMessage = "是否確定執行發放MAIL通知功能:";
            string vStr_desc = "";
            string vStr_desc1 = "晉昇名單中MAIL 空白的名單如下: \n";

            CFB2SM1400DAO dao = new CFB2SM1400DAO();
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
    public DataTable get_PDF_Data(CFB2SM1400DAO dao)
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
}