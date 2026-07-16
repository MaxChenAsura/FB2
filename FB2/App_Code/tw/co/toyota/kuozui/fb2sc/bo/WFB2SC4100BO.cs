using FB2.tw.co.toyota.kuozui.bo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// WFB2SC4100BO 的摘要描述
/// </summary>
public class WFB2SC4100BO : BaseService
{
    WFB2SC4100DL dl = null;
    public WFB2SC4100BO()
    {
        dl = new WFB2SC4100DL();
    }

    public int GetGridDataCount(int startRowIndex, int maximumRows, string strIsSuper,
                                string SALARY_YM, string SALARY_DT_S, string SALARY_DT_E,
                                string QryEMP_ID, string EMP_NAME, string DEPT_NO,
                                string EMP_CHG_CD)
    {
        return dl.GetGridDataCount(startRowIndex, maximumRows, strIsSuper, SALARY_YM, SALARY_DT_S, SALARY_DT_E, QryEMP_ID, EMP_NAME, DEPT_NO, EMP_CHG_CD);
    }

    public DataTable GetGridData(int startRowIndex, int maximumRows, string strIsSuper,
                                 string SALARY_YM, string SALARY_DT_S, string SALARY_DT_E,
                                 string QryEMP_ID, string EMP_NAME, string DEPT_NO,
                                 string EMP_CHG_CD, string sortExpression)
    {
        return dl.GetGridData(startRowIndex, maximumRows, strIsSuper, SALARY_YM, SALARY_DT_S, SALARY_DT_E, QryEMP_ID, EMP_NAME, DEPT_NO, EMP_CHG_CD, sortExpression);
    }

    public WFB2SC4100DtlDAO GetDetailHeaderByTypeA(DateTime SALARY_DT, string SALARY_TYPE, string EMP_ID, string PAY_KIND)
    {
        return dl.GetDetailHeaderByTypeA(SALARY_DT, SALARY_TYPE, EMP_ID, PAY_KIND);
    }

    public WFB2SC4100DtlDAO GetDetailHeaderByTypeNotA(DateTime SALARY_DT, string SALARY_TYPE, string EMP_ID, string PAY_KIND)
    {
        return dl.GetDetailHeaderByTypeNotA(SALARY_DT, SALARY_TYPE, EMP_ID, PAY_KIND);
    }

    public DataTable GetShouldBeAddedData(DateTime SALARY_DT, string SALARY_TYPE, string EMP_ID, string PAY_KIND, string IS_TAX, int IS_PLUS)
    {
        return dl.GetShouldBeAddedData(SALARY_DT, SALARY_TYPE, EMP_ID, PAY_KIND, IS_TAX, IS_PLUS);
    }
    public DataTable Get2B(string EMP_ID, DateTime SALARY_DT, string SALARY_TYPE, string PAY_KIND)
    {
        return dl.Get2B(EMP_ID, SALARY_DT, SALARY_TYPE, PAY_KIND);
    }

    public DataTable GetOverTime(string EMP_ID, DateTime SALARY_DT)
    {
        return dl.GetOverTime(EMP_ID, SALARY_DT);
    }

    public DataTable GetLeave(string EMP_ID, DateTime SALARY_DT)
    {
        return dl.GetLeave(EMP_ID, SALARY_DT);
    }
    public DataTable GetWorkShift(string EMP_ID, DateTime SALARY_DT)
    {
        return dl.GetWorkShift(EMP_ID, SALARY_DT);
    }

    public DataTable GetAvailableLeave(string EMP_ID, DateTime SALARY_DT)
    {
        return dl.GetAvailableLeave(EMP_ID, SALARY_DT);
    }

    public DataTable GetPension(string EMP_ID, DateTime SALARY_DT)
    {
        return dl.GetPension(EMP_ID, SALARY_DT);
    }

    public DataTable GetINS2(string EMP_ID, DateTime SALARY_DT)
    {
        return dl.GetINS2(EMP_ID, SALARY_DT);
    }
    public DataTable GetItemByTypeC_1031(string EMP_ID, DateTime SALARY_DT)
    {
        return dl.GetItemByTypeC_1031(EMP_ID, SALARY_DT);
    }
    public DataTable GetItemByTypeC_1033(string EMP_ID, DateTime SALARY_DT)
    {
        return dl.GetItemByTypeC_1033(EMP_ID, SALARY_DT);
    }
    public DataTable GetTB_S_M_DUTY_RESULT_H_SDT_EDT(DateTime SALARY_DT)
    {
        return dl.GetTB_S_M_DUTY_RESULT_H_SDT_EDT(SALARY_DT);
    }
    //年奬
    public DataTable GetReSendDataBy1031(string SALARY_TYPE, DateTime SALARY_DT)
    {
        return dl.GetReSendDataBy1031(SALARY_TYPE, SALARY_DT);

    }
    //紅利
    public DataTable GetReSendDataBy1033(string SALARY_TYPE, DateTime SALARY_DT)
    {
        return dl.GetReSendDataBy1033(SALARY_TYPE, SALARY_DT);

    }

    public DataTable GetReSendDataBy1035_1032_1062_1056_1070(string SALARY_TYPE, DateTime SALARY_DT)
    {
        return dl.GetReSendDataBy1035_1032_1062_1056_1070(SALARY_TYPE, SALARY_DT);

    }

    public DataTable GetReSendDataBy9999_1061_1038_1039(string SALARY_TYPE, DateTime SALARY_DT)
    {
        return dl.GetReSendDataBy9999_1061_1038_1039(SALARY_TYPE, SALARY_DT);

    }

    public DataTable GetReSendDataBy1035(string SALARY_TYPE, DateTime SALARY_DT)
    {
        return dl.GetReSendDataBy1035(SALARY_TYPE, SALARY_DT);

    }

    public void ReSent(string mail_title, string mail_desc, DateTime salary_dt, string salary_type, string pay_kind, string emp_id, string email)
    {
        try
        {
            this.BeginTransaction();
            string DescLet = "註記：若您對薪資明細資料有任何疑問，請逕洽管理部人事勞務G薪資擔當查詢。";
            if (dl.checkMail_H_IsExist(DateTime.Now, salary_dt, salary_type, pay_kind))
            {
                dl.InsertTB_S_M_MAIL_BAT_H(DateTime.Now, mail_title, mail_desc + DescLet, salary_dt, salary_type, pay_kind);
            }
            if (dl.checkMail_D_IsExist(DateTime.Now, salary_dt, salary_type, pay_kind, emp_id))
            {
                dl.InsertTB_S_M_MAIL_BAT_D(DateTime.Now, salary_dt, salary_type, pay_kind, emp_id, email);
            }
            this.Commit();
        }
        catch (Exception ex)
        {
            this.RollBack();
            throw ex;
        }
    }

    public string getLICENSE_ID(String EMP_ID)
    {
        return dl.getLICENSE_ID(EMP_ID);
    }
}