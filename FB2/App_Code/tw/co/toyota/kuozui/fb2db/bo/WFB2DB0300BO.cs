using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;
using System.Data;

/// <summary>
/// WFB2DB0300BO 的摘要描述
/// </summary>
public class WFB2DB0300BO : BaseService
{
    private WFB2DB0300DL dl = null;

    public WFB2DB0300BO()
    {
        dl = new WFB2DB0300DL();
    }

    public DataTable GetGridData(int startRowIndex, int maximumRows, string PLANT_CD, string DEPT_NO, string EMP_ID, string WORK_SHIFT_CD, string JOIN_DT_Start, string JOIN_DT_End, string sortExpression)
    {
        return dl.GetGridData(startRowIndex, maximumRows, PLANT_CD, DEPT_NO, EMP_ID, WORK_SHIFT_CD, JOIN_DT_Start, JOIN_DT_End, sortExpression);
    }

    public int GetGridDataCount(string PLANT_CD, string DEPT_NO, string EMP_ID, string WORK_SHIFT_CD, string JOIN_DT_Start, string JOIN_DT_End)
    {
        return dl.GetGridDataCount(PLANT_CD, DEPT_NO, EMP_ID, WORK_SHIFT_CD, JOIN_DT_Start, JOIN_DT_End);
    }

    public DataTable GetDataByEMP_ID(string EMP_ID)
    {
        return dl.GetDataByEMP_ID(EMP_ID);
    }

    public bool checkS_DUTY_EDT(DateTime UiDateTime)
    {
        DateTime? checkdate = dl.checkS_DUTY_EDT(UiDateTime);
        if (checkdate == null)
            return true;
        else
        {
            if (UiDateTime <= checkdate)
                return false;
            else
                return true;
        }
    }
    public DateTime? CheckWorkSheetDate(string EMP_ID)
    {
        return dl.CheckWorkSheetDate(EMP_ID);
    }

    public DataTable GetAllGrantData()
    {
        return dl.GetAllGrantData();
    }

    public int checkEMP_DAY_DUTYCount(string CALENDAR_DATE_Start, string CALENDAR_DATE_End, DataTable EMP_IDs)
    {
        BeginTransaction();

        int DAY_DUTYCount= dl.checkEMP_DAY_DUTYCount(CALENDAR_DATE_Start, CALENDAR_DATE_End, EMP_IDs);
        RollBack();
        return DAY_DUTYCount;
    }

    public bool callSP_D_UPD_EMP_DAY_DUTY(string CALENDAR_DATE_Start, string CALENDAR_DATE_End, string USER_ID, string FUNC_ID, out string Message)
    {
        
        Message = string.Empty;

        this.BeginTransaction();
        //foreach (DataRow row in UiData.Rows)
        //{
            if (!dl.callSP_D_UPD_EMP_DAY_DUTY(CALENDAR_DATE_Start, CALENDAR_DATE_End, USER_ID, FUNC_ID, out Message))
            {
                this.RollBack();
                return false;
            }
            
        //}
            this.Commit();
        return true;
    }

    public bool callSP_D_UPD_EMP_DAY_DUTY1(DataTable UiData, string CALENDAR_DATE_Start, string CALENDAR_DATE_End, string USER_ID, string FUNC_ID, out string Message)
    {

        Message = string.Empty;

        this.BeginTransaction();
        foreach (DataRow row in UiData.Rows)
        {
            if (!dl.callSP_D_UPD_EMP_DAY_DUTY1(Convert.ToString(row["EMP_ID"]), Convert.ToString(row["WORK_SHIFT_CD"]), CALENDAR_DATE_Start, CALENDAR_DATE_End, USER_ID, FUNC_ID, out Message))
        {
            this.RollBack();
            return false;
        }

        }
        this.Commit();
        return true;
    }
}