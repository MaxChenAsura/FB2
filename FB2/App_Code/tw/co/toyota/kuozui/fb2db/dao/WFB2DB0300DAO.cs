using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;
using System.Data;
using System.Text;
using System.Collections;

/// <summary>
/// WFB2DB0300DAO 的摘要描述
/// </summary>

public class WFB2DB0300DAO
{
}
public class WFB2DB0300ErrorDAO
{
    public string WORK_SHIFT_CD { get; set; }
    public string WORK_SHIFT_DESC { get; set; }
    public string CALENDAR_DT_START { get; set; }
    public string CALENDAR_DT_END { get; set; }
    public string MEMO { get; set; }
}

public class WFB2DB0300DL : BaseDAO
{
    public WFB2DB0300DL()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public int GetGridDataCount(string PLANT_CD, string DEPT_NO, string EMP_ID, string WORK_SHIFT_CD, string JOIN_DT_Start, string JOIN_DT_End)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.AppendLine(" select count(1) as TotlCount");
        sb.AppendLine(" from VW_H_EMP_DATA VHED ");
        sb.AppendLine(" left join TB_9_M_COMM_D T9MCD on T9MCD.SUB_CD=VHED.PLANT_CD and MAIN_CD='PLANT_CD' ");
        sb.AppendLine(" where 1=1 and VHED.EMP_STATUS='01' ");

        if (PLANT_CD != Resources.Resource.wfb2db_dll_PlaceChoice)
        {
            sb.AppendLine("         and PLANT_CD like @PLANT_CD+'%' ");
            ht.Add("@PLANT_CD", PLANT_CD);
        }

        if (!String.IsNullOrEmpty(DEPT_NO))
        {
            sb.AppendLine("         and DEPT_NO like @DEPT_NO+'%'");
            ht.Add("@DEPT_NO", DEPT_NO);
        }

        if (!String.IsNullOrEmpty(EMP_ID))
        {
            sb.AppendLine("         and EMP_ID like @EMP_ID +'%' ");
            ht.Add("@EMP_ID", EMP_ID);
        }
        if (!String.IsNullOrEmpty(WORK_SHIFT_CD))
        {
            sb.AppendLine("         and WORK_SHIFT_CD like @WORK_SHIFT_CD +'%'");
            ht.Add("@WORK_SHIFT_CD", WORK_SHIFT_CD);
        }

        if (!String.IsNullOrEmpty(JOIN_DT_Start))
        {
            sb.AppendLine("         and JOIN_DT>=@JOIN_DT_Start ");
            ht.Add("@JOIN_DT_Start", JOIN_DT_Start);
        }
        if (!String.IsNullOrEmpty(JOIN_DT_End))
        {
            sb.AppendLine("         and JOIN_DT<=@JOIN_DT_End ");
            ht.Add("@JOIN_DT_End", JOIN_DT_End);
        }
        return Convert.ToInt32(dbConn.Query(sb, ht).Rows[0][0]);
    }

    public DataTable GetGridData(int startRowIndex, int maximumRows, string PLANT_CD, string DEPT_NO, string EMP_ID, string WORK_SHIFT_CD, string JOIN_DT_Start, string JOIN_DT_End, string sortExpression)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.AppendLine(" select * ");
        sb.AppendLine(" from (select ROW_NUMBER() OVER(ORDER BY VHED.EMP_ID,VHED.PLANT_CD ) As RowNumber , ");
        sb.AppendLine("              EMP_ID, ");
        sb.AppendLine("   	         EMP_NAME, ");
        sb.AppendLine("              PLANT_CD, ");
        sb.AppendLine("              T9MCD.SUB_DESC PLANT, ");
        sb.AppendLine("              DEPT_NO, ");
        sb.AppendLine("              DEPT_NAME, ");
        sb.AppendLine("              WORK_SHIFT_CD, ");
        sb.AppendLine("              WORK_SHIFT_DESC ");
        sb.AppendLine("       from VW_H_EMP_DATA VHED ");
        sb.AppendLine("       left join TB_9_M_COMM_D T9MCD on T9MCD.SUB_CD=VHED.PLANT_CD and MAIN_CD='PLANT_CD' ");
        sb.AppendLine("       where 1=1 and VHED.EMP_STATUS='01' ");


        if (PLANT_CD != Resources.Resource.wfb2db_dll_PlaceChoice)
        {
            sb.AppendLine("         and PLANT_CD like @PLANT_CD+'%' ");
            ht.Add("@PLANT_CD", PLANT_CD);
        }

        if (!String.IsNullOrEmpty(DEPT_NO))
        {
            sb.AppendLine("         and DEPT_NO like @DEPT_NO+'%'");
            ht.Add("@DEPT_NO", DEPT_NO);
        }

        if (!String.IsNullOrEmpty(EMP_ID))
        {
            sb.AppendLine("         and EMP_ID like @EMP_ID +'%' ");
            ht.Add("@EMP_ID", EMP_ID);
        }
        if (!String.IsNullOrEmpty(WORK_SHIFT_CD))
        {
            sb.AppendLine("         and WORK_SHIFT_CD like @WORK_SHIFT_CD +'%'");
            ht.Add("@WORK_SHIFT_CD", WORK_SHIFT_CD);
        }

        if (!String.IsNullOrEmpty(JOIN_DT_Start))
        {
            sb.AppendLine("         and JOIN_DT>=@JOIN_DT_Start ");
            ht.Add("@JOIN_DT_Start", JOIN_DT_Start);
        }
        if (!String.IsNullOrEmpty(JOIN_DT_End))
        {
            sb.AppendLine("         and JOIN_DT<=@JOIN_DT_End ");
            ht.Add("@JOIN_DT_End", JOIN_DT_End);
        }



        sb.AppendLine(" ) GRID_DATA where RowNumber between CAST(@startRowIndex+1 as varchar) ");
        sb.AppendLine("                     AND CAST(@startRowIndex+@maximumRows as varchar)");
        ht.Add("@startRowIndex", startRowIndex);
        ht.Add("@maximumRows", maximumRows);

        DataTable returnDt = dbConn.Query(sb, ht);

        return returnDt;

    }

    public DataTable GetDataByEMP_ID(string EMP_ID)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.AppendLine(" select EMP_ID, ");
        sb.AppendLine("   	   EMP_NAME, ");
        sb.AppendLine("        PLANT_CD, ");
        sb.AppendLine("        T9MCD.SUB_DESC PLANT, ");
        sb.AppendLine("        DEPT_NO, ");
        sb.AppendLine("        DEPT_NAME, ");
        sb.AppendLine("        WORK_SHIFT_CD, ");
        sb.AppendLine("        WORK_SHIFT_DESC ");
        sb.AppendLine(" from VW_H_EMP_DATA VHED ");
        sb.AppendLine(" left join TB_9_M_COMM_D T9MCD on T9MCD.SUB_CD=VHED.PLANT_CD and MAIN_CD='PLANT_CD' ");
        sb.AppendLine(" where EMP_ID=@EMP_ID ");
        ht.Add("@EMP_ID", EMP_ID);

        DataTable returnDt = dbConn.Query(sb, ht);

        return returnDt;
    }

    public DataTable GetAllGrantData()
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.AppendLine(" select EMP_ID, ");
        sb.AppendLine("   	   EMP_NAME, ");
        sb.AppendLine("        PLANT_CD, ");
        sb.AppendLine("        T9MCD.SUB_DESC PLANT, ");
        sb.AppendLine("        DEPT_NO, ");
        sb.AppendLine("        DEPT_NAME, ");
        sb.AppendLine("        WORK_SHIFT_CD, ");
        sb.AppendLine("        WORK_SHIFT_DESC ");
        sb.AppendLine(" from VW_H_EMP_DATA VHED ");
        sb.AppendLine(" left join TB_9_M_COMM_D T9MCD on T9MCD.SUB_CD=VHED.PLANT_CD and MAIN_CD='PLANT_CD' ");
        sb.AppendLine(" where EMP_STATUS='01' ");
        DataTable returnDt = dbConn.Query(sb, ht);
        return returnDt;
    }

    public DateTime? checkS_DUTY_EDT(DateTime UiDateTime)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.AppendLine("  select dbo.FN_S_DUTY_EDT('A') S_DUTY_EDT ");

        DataTable returnData = dbConn.Query(sb, ht);
        if (returnData.Rows.Count == 0)
            return null;
        else
        {
            if (returnData.Rows[0][0] == DBNull.Value)
                return null;
            else
                return Convert.ToDateTime(returnData.Rows[0][0]);
        }
    }

    public DateTime? CheckWorkSheetDate(string EMP_ID)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();

        sb.AppendLine(" select MAX(TDMWSD.CALENDAR_DT) ");
        sb.AppendLine(" from TB_D_M_WORK_SHIFT_D TDMWSD ");
        sb.AppendLine(" join VW_H_EMP_DATA VHED on VHED.WORK_SHIFT_CD=TDMWSD.WORK_SHIFT_CD and VHED.EMP_ID=@EMP_ID ");

        ht.Add("@EMP_ID", EMP_ID);
        DataTable returnData = dbConn.Query(sb, ht);
        if (returnData.Rows.Count == 0)
            return null;
        else
        {
            if (returnData.Rows[0][0] == DBNull.Value)
                return null;
            else
                return Convert.ToDateTime(returnData.Rows[0][0]);
        }
    }

    public int checkEMP_DAY_DUTYCount(string CALENDAR_DATE_Start, string CALENDAR_DATE_End, DataTable EMP_IDs)
    {
        StringBuilder sb = new StringBuilder();
        StringBuilder sb2 = new StringBuilder();
        Hashtable ht = new Hashtable();
        //sb.AppendLine("Drop Table #TempTable");
        //dbConn.ExecuteT(sb, ht);
        //sb.Clear();
        sb.AppendLine("Create Table #TempTable(");
        sb.AppendLine("EMP_ID varchar(5)       ");
        sb.AppendLine(")                       ");
        dbConn.ExecuteT(sb, ht);
        sb.Clear();
        //string[] arrEmpIds = EMP_IDs.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        
        foreach (DataRow empid in EMP_IDs.Rows)
        {
            sb.AppendLine("insert into #TempTable values(@empid)");
            ht.Add("@empid", empid["EMP_ID"]);
            dbConn.ExecuteT(sb, ht);
            sb.Clear();
            ht.Clear();
            
        }
        sb.AppendLine(" select count(*) ");
        sb.AppendLine(" from TB_D_M_EMP_DAY_DUTY A ");
        sb.AppendLine(" where A.CALENDAR_DT >= @CALENDAR_DATE_Start ");
        sb.AppendLine("   and A.CALENDAR_DT <= @CALENDAR_DATE_End ");

        ht.Add("@CALENDAR_DATE_Start", CALENDAR_DATE_Start);
        ht.Add("@CALENDAR_DATE_End", CALENDAR_DATE_End);



        if (EMP_IDs.Rows.Count>0)
            sb.AppendLine("   and A.EMP_ID in (select EMP_ID from #TempTable) ");
        
        int t = Convert.ToInt32(dbConn.QueryT(sb, ht).Rows[0][0]);

        sb2.AppendLine("Drop Table #TempTable");
        dbConn.ExecuteT(sb2, ht);
        sb2.Clear();
        return t;

    }

    public bool callSP_D_UPD_EMP_DAY_DUTY(string CALENDAR_DATE_Start, string CALENDAR_DATE_End, string USER_ID, string FUNC_ID, out string Message)
    {
        //執行【維護員工日勤務班表(一)】(I.置換, A.工號, A.輪值表代碼, 畫面上.勤務日期區間起, 畫面上.勤務日期區間迄, 登入者帳號, 更新作業FunctionID)																																																													
        try
        {
            Message = string.Empty;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            
            sb.Append("SP_D_UPD_EMP_DAY_DUTY");
            ht.Add("@pStartDt", CALENDAR_DATE_Start);
            ht.Add("@pEndDt", CALENDAR_DATE_End);
            ht.Add("@pUserID", USER_ID);
            ht.Add("@pFuncID", FUNC_ID);
            dbConn.ExecuteSPT(sb, ht, true);
            return true;
        }
        catch (Exception ex)
        {
            Message = ex.Message;
            return false;
        }

    }


    public bool callSP_D_UPD_EMP_DAY_DUTY1(string EMP_ID, string WORK_SHIFT_CD, string CALENDAR_DATE_Start, string CALENDAR_DATE_End, string USER_ID, string FUNC_ID, out string Message)
    {
        //執行【維護員工日勤務班表(一)】(I.置換, A.工號, A.輪值表代碼, 畫面上.勤務日期區間起, 畫面上.勤務日期區間迄, 登入者帳號, 更新作業FunctionID)																																																													
        try
        {
            Message = string.Empty;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("SP_D_UPD_EMP_DAY_DUTY1");
            ht.Add("@pHandleCd", "I");
            ht.Add("@pEmpId", EMP_ID);
            ht.Add("@pWorkShiftCd", WORK_SHIFT_CD);
            ht.Add("@pStartDt", CALENDAR_DATE_Start);
            ht.Add("@pEndDt", CALENDAR_DATE_End);
            ht.Add("@pUserID", USER_ID);
            ht.Add("@pFuncID", FUNC_ID);
            dbConn.ExecuteSPT(sb, ht, true);
            return true;
        }
        catch (Exception ex)
        {
            Message = ex.Message;
            return false;
        }

    }


    public DataTable getDEPT_NAME(string dept_no)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select DEPT_NAME ");
            sb.AppendLine(" from VW_H_DEPT_DATA ");
            sb.AppendLine(" where DEPT_NO = @DEPT_NO ");
            ht.Add("@DEPT_NO", dept_no.ToUpper());

            return dbConn.Query(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getEmp_Name(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select EMP_NAME ");
            sb.AppendLine(" from VW_H_EMP_DATA ");
            sb.AppendLine(" where EMP_ID =@EMP_ID ");
            ht.Add("@EMP_ID", emp_id);
            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable getWORK_SHIFT_DESC(string work_shift_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select WORK_SHIFT_DESC ");
            sb.AppendLine(" from TB_D_M_WORK_SHIFT_H ");
            sb.AppendLine(" where WORK_SHIFT_CD = @WORK_SHIFT_CD ");
            ht.Add("@WORK_SHIFT_CD", work_shift_cd.ToUpper());

            return dbConn.Query(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getEmp_Name_add(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("Select e.EMP_ID,e.EMP_NAME,d.DEPT_NAME,e.DEPT_NO,PLANT_NAME           ");
            sb.AppendLine("from VW_H_EMP_DATA e,VW_H_DEPT_DATA d                                 ");
            sb.AppendLine("where e.DEPT_NO = d.DEPT_NO and EMP_STATUS = '01' and e.EMP_ID=@EMP_ID");
            ht.Add("@EMP_ID", emp_id);
            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
}