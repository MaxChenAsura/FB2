using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;

/// <summary>
/// CFB2SC2700BO 的摘要描述
/// </summary>
public class CFB2SC2700BO : BaseService
{
    public CFB2SC2700BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public string Execute(string SALARY_TYPE, string SALARY_DT, string PAY_KIND, string PAY_ID, string EMAIL_DT, string TITLE, string CONTENT)
    {
        try
        {
            CFB2SC2700DAO fb2sc = new CFB2SC2700DAO();
            BeginTransaction();
            fb2sc.Del_TB_S_C_MAIL_BAT_H(SALARY_TYPE, SALARY_DT, PAY_KIND, EMAIL_DT);
            fb2sc.Del_TB_S_C_MAIL_BAT_D(SALARY_TYPE, SALARY_DT, PAY_KIND, EMAIL_DT);
            DataTable dt = fb2sc.getEmpData(SALARY_TYPE, PAY_ID);
            if (dt.Rows.Count > 0)
            {
                fb2sc.Add_TB_S_C_MAIL_BAT_H(EMAIL_DT, TITLE, CONTENT, SALARY_DT, SALARY_TYPE, PAY_KIND);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    string EMP_ID = dt.Rows[i]["EMP_ID"].ToString();
                    string SALARY_EMAIL = dt.Rows[i]["SALARY_EMAIL"].ToString();
                    fb2sc.Add_TB_S_C_MAIL_BAT_D(EMAIL_DT, SALARY_DT, SALARY_TYPE, PAY_KIND, EMP_ID, SALARY_EMAIL);
                }
            }
            fb2sc.Update_TB_S_M_SALARY_PAY_H(EMAIL_DT, PAY_ID);
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