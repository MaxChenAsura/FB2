using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;
using System.Data;
using System.Data.SqlClient;
using System.Text;
using System.Collections;

/// <summary>
/// WFB2DB0200DAO 的摘要描述
/// </summary>

public class WFB2DB0200DAO
{
    public string PLANT_CD { get; set; }
    public string PLANT { get; set; }
    public string DEPT_NO { get; set; }
    public string DEPT_NAME { get; set; }
    public string EMP_ID { get; set; }
    public string EMP_NAME { get; set; }
    public DateTime CALENDAR_DT { get; set; }
    public string WORK_DAY_CD { get; set; }
    public string WORK_DAY { get; set; }
    public string SHIFT_CD { get; set; }
    public string SHIFT { get; set; }
    public string DUTY_STIME { get; set; }
    public string DUTY_ETIME { get; set; }
    public string WORK_SHIFT_DESC { get; set; }
    public string WORK_SHIFT_CD { get; set; }
    public string SHIFT_TIME_CD { get; set; }
    public string WORK_HOUR { get; set; }
    public string WORK_PERIOD_HOUR { get; set; }
    public string DUTY_BEFORE_REST_STIME_1 { get; set; }
    public string DUTY_BEFORE_REST_ETIME_1 { get; set; }
    public string DINING_STIME_1 { get; set; }
    public string DINING_ETIME_1 { get; set; }
    public string DINING_STIME_2 { get; set; }
    public string DINING_ETIME_2 { get; set; }
    public string REST_STIME_1 { get; set; }
    public string REST_ETIME_1 { get; set; }
    public string REST_STIME_2 { get; set; }
    public string REST_ETIME_2 { get; set; }
    public string REST_STIME_3 { get; set; }
    public string REST_ETIME_3 { get; set; }
    public string DINING_STIME_3 { get; set; }
    public string DINING_ETIME_3 { get; set; }
    public string DUTY_AFTER_REST_STIME_1 { get; set; }
    public string DUTY_AFTER_REST_ETIME_1 { get; set; }
    public string DUTY_AFTER_REST_STIME_2 { get; set; }
    public string DUTY_AFTER_REST_ETIME_2 { get; set; }
    public string WORK_SHIFT_ALLOWANCE_TYPE { get; set; }
    public string CREATED_BY { get; set; }
    public DateTime CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public DateTime UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }

}


public class WFB2DB0200DL : BaseDAO
{
    public WFB2DB0200DL()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public int GetGridDataCount(int startRowIndex, int maximumRows, string CALENDAR_DT_Start,
                                string CALENDAR_DT_End, string PLANT_CD, string DEPT_NO,
                                string EMP_ID, string JOIN_DT_Start, string JOIN_DT_End,
                                string WORK_SHIFT_CD, string DEPTAuth, string IsDEPT,
                                string sp_dept, string work_day_cd, string shift_cd)
    {
        

        StringBuilder sb_EMP_DAY_DUTY = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb_EMP_DAY_DUTY.Append(@" select * from  TB_D_M_EMP_DAY_DUTY  TDMEDD with (nolock) where 1=1");
        //若有輸入勤務日期起，A.勤務日期 >='畫面.勤務日期起'
        if (!string.IsNullOrEmpty(CALENDAR_DT_Start))
        {
            sb_EMP_DAY_DUTY.AppendLine(" and TDMEDD.CALENDAR_DT>=@CALENDAR_DT_Start ");
            ht.Add("@CALENDAR_DT_Start", CALENDAR_DT_Start);
        }
        //若有輸入勤務日期迄，A.勤務日期 <='畫面.勤務日期迄'
        if (!string.IsNullOrEmpty(CALENDAR_DT_End))
        {
            sb_EMP_DAY_DUTY.AppendLine(" and TDMEDD.CALENDAR_DT<=@CALENDAR_DT_End ");
            ht.Add("@CALENDAR_DT_End", CALENDAR_DT_End);
        }
        //若有輸入工號，A.工號 like '畫面.工號%'	
        if (!string.IsNullOrEmpty(EMP_ID))
        {
            sb_EMP_DAY_DUTY.AppendLine(" and TDMEDD.EMP_ID like @EMP_ID+'%' ");
            ht.Add("@EMP_ID", EMP_ID);
        }
        //若有輸入輪值表代碼，A.輪值表代碼 like '畫面.輪值表代碼%'		
        if (!string.IsNullOrEmpty(WORK_SHIFT_CD))
        {
            sb_EMP_DAY_DUTY.AppendLine("  and TDMEDD.WORK_SHIFT_CD like @WORK_SHIFT_CD +'%' ");
            ht.Add("@WORK_SHIFT_CD", WORK_SHIFT_CD);
        }
        //顯示資料權限設定,若不為super user (以員工為主)
        if (SessionHandle.Current.is_super != "Y")
        {
            sb_EMP_DAY_DUTY.Append(@" AND TDMEDD.EMP_ID IN(	 select EMP_ID from dbo.FN_H_GET_AUTH_EMP(@loginID,@departments)  )");
            ht.Add("@loginID", SessionHandle.Current.emp_id);
            ht.Add("@departments", SessionHandle.Current.departments);
        }
        //出勤別
        if (work_day_cd != "-1" && work_day_cd != null)
        {
            sb_EMP_DAY_DUTY.AppendLine(" and TDMEDD.WORK_DAY_CD = @WORK_DAY_CD  ");
            ht.Add("@WORK_DAY_CD", work_day_cd);
        }
        //班別
        if(shift_cd !=""){
            sb_EMP_DAY_DUTY.AppendLine(" and TDMEDD.SHIFT_CD = @SHIFT_CD  ");
            ht.Add("@SHIFT_CD", shift_cd);
        }


        StringBuilder sb = new StringBuilder();
        sb.AppendLine(" select sum(total_record) total_record ");
        sb.AppendLine(" from (");
        sb.AppendLine("       select COUNT(1) total_record ");
        //sb.AppendLine("       from TB_D_M_EMP_DAY_DUTY TDMEDD ");
        sb.Append(" from  ( " + sb_EMP_DAY_DUTY + " ) TDMEDD");
        sb.AppendLine("       left join VW_H_EMP_DATA VHED on VHED.EMP_ID=TDMEDD.EMP_ID ");
        sb.AppendLine("	        where 1=1 ");

        //若有輸入工廠區分，A.工號 in(select 工號 from VW_H_EMP_DATA(員工人事資料VIEW) where 在職狀態 in('01','02') and 工廠區分 = 畫面.工廠區分)
        if (PLANT_CD != Resources.Resource.wfb2db_dll_PlaceChoice)
        {
            sb.AppendLine("       and VHED.EMP_ID in (select EMP_ID  from VW_H_EMP_DATA where  PLANT_CD =@PLANT_CD) ");
            ht.Add("@PLANT_CD", PLANT_CD);
        }

        //若有輸入部門，A.工號 in(select 工號 from VW_H_EMP_DATA(員工人事資料VIEW) where 在職狀態 in('01','02') and 部門代號 = 畫面.部門代號)	
        if (!string.IsNullOrEmpty(DEPT_NO))
        {
            sb.AppendLine("       and VHED.EMP_ID in (select EMP_ID  from VW_H_EMP_DATA where  DEPT_NO =@DEPT_NO) ");
            ht.Add("@DEPT_NO", DEPT_NO);
        }

        //若有輸入入社日期起，A.工號 in(select 工號 from VW_H_EMP_DATA(員工人事資料VIEW) where 在職狀態 in('01','02') and 入社日期 >= 畫面.入社日期起)																																																																					
        if (!string.IsNullOrEmpty(JOIN_DT_Start))
        {
            sb.AppendLine("       and VHED.EMP_ID in (select EMP_ID  from VW_H_EMP_DATA where  JOIN_DT >=@JOIN_DT_Start) ");
            ht.Add("@JOIN_DT_Start", JOIN_DT_Start);
        }
        //若有輸入入社日期迄，A.工號 in(select 工號 from VW_H_EMP_DATA(員工人事資料VIEW) where 在職狀態 in('01','02') and 入社日期 <= 畫面.入社日期迄)																																																																					
        if (!string.IsNullOrEmpty(JOIN_DT_End))
        {
            sb.AppendLine("       and VHED.EMP_ID in (select EMP_ID  from VW_H_EMP_DATA where  JOIN_DT <=@JOIN_DT_End) ");
            ht.Add("@JOIN_DT_End", JOIN_DT_End);
        }
       
        sb.AppendLine(" 			                                ) a ");

        Int32 ReturnValue = Convert.ToInt32(dbConn.Query(sb, ht).Rows[0]["total_record"]);
        return ReturnValue;
    }

    public DataTable GetGridData(int startRowIndex, int maximumRows, string CALENDAR_DT_Start,
                                 string CALENDAR_DT_End, string PLANT_CD, string DEPT_NO,
                                 string EMP_ID, string JOIN_DT_Start, string JOIN_DT_End,
                                 string WORK_SHIFT_CD, string DEPTAuth, string IsDEPT,
                                 string sp_dept, string work_day_cd, string shift_cd, string sortExpression)
    {



        StringBuilder sb_EMP_DAY_DUTY = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb_EMP_DAY_DUTY.Append(@" select * from  TB_D_M_EMP_DAY_DUTY  TDMEDD with (nolock) where 1=1 ");
        //若有輸入勤務日期起，A.勤務日期 >='畫面.勤務日期起'
        if (!string.IsNullOrEmpty(CALENDAR_DT_Start))
        {
            sb_EMP_DAY_DUTY.AppendLine(" and TDMEDD.CALENDAR_DT>=@CALENDAR_DT_Start ");
            ht.Add("@CALENDAR_DT_Start", CALENDAR_DT_Start);
        }
        //若有輸入勤務日期迄，A.勤務日期 <='畫面.勤務日期迄'
        if (!string.IsNullOrEmpty(CALENDAR_DT_End))
        {
            sb_EMP_DAY_DUTY.AppendLine(" and TDMEDD.CALENDAR_DT<=@CALENDAR_DT_End ");
            ht.Add("@CALENDAR_DT_End", CALENDAR_DT_End);
        }
        //若有輸入工號，A.工號 like '畫面.工號%'	
        if (!string.IsNullOrEmpty(EMP_ID))
        {
            sb_EMP_DAY_DUTY.AppendLine(" and TDMEDD.EMP_ID like @EMP_ID+'%' ");
            ht.Add("@EMP_ID", EMP_ID);
        }
        //若有輸入輪值表代碼，A.輪值表代碼 like '畫面.輪值表代碼%'		
        if (!string.IsNullOrEmpty(WORK_SHIFT_CD))
        {
            sb_EMP_DAY_DUTY.AppendLine("  and TDMEDD.WORK_SHIFT_CD like @WORK_SHIFT_CD +'%' ");
            ht.Add("@WORK_SHIFT_CD", WORK_SHIFT_CD);
        }
        //顯示資料權限設定,若不為super user (以員工為主)
        if (SessionHandle.Current.is_super != "Y")
        {
            sb_EMP_DAY_DUTY.Append(@" AND TDMEDD.EMP_ID IN(	 select EMP_ID from dbo.FN_H_GET_AUTH_EMP(@loginID,@departments)  )");
            ht.Add("@loginID", SessionHandle.Current.emp_id);
            ht.Add("@departments", SessionHandle.Current.departments);
        }
        //出勤別
        if (work_day_cd != "-1" && work_day_cd != null)
        {
            sb_EMP_DAY_DUTY.AppendLine(" and TDMEDD.WORK_DAY_CD = @WORK_DAY_CD  ");
            ht.Add("@WORK_DAY_CD", work_day_cd);
        }
        //班別
        if (shift_cd != "")
        {
            sb_EMP_DAY_DUTY.AppendLine(" and TDMEDD.SHIFT_CD = @SHIFT_CD  ");
            ht.Add("@SHIFT_CD", shift_cd);
        }


        StringBuilder sb = new StringBuilder();
        sb.AppendLine(" select * ");
        sb.AppendLine(" from (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber ,* ");
        sb.AppendLine("       from (select distinct VHED.PLANT_CD, ");
        sb.AppendLine(" 	               T9MCD_PLANT_CD.SUB_DESC PLANT, ");
        sb.AppendLine(" 	               VHED.DEPT_NO, ");
        sb.AppendLine(" 	               VHED.DEPT_FULL_NAME DEPT_NAME, ");
        sb.AppendLine(" 	               VHED.EMP_ID, ");
        sb.AppendLine(" 	               VHED.EMP_NAME, ");
        sb.AppendLine(" 	               convert(varchar, TDMEDD.CALENDAR_DT, 111) CALENDAR_DT, ");
        sb.AppendLine(" 	               TDMEDD.WORK_DAY_CD, ");
        sb.AppendLine(" 	               T9MCD_WORK_DAY_CD.SUB_DESC WORK_DAY, ");
        sb.AppendLine(" 	               TDMEDD.SHIFT_CD, ");
        sb.AppendLine(" 	               TDMEDD.SHIFT_CD +'-'+SHIFTH.SHIFT_DESC SHIFT, ");
        sb.AppendLine(" 	               VHED.WORK_CD, ");
        sb.AppendLine(" 	               VHED.WORK_CD +'-'+T9MCD_WORK_CD_CD.SUB_DESC WORK_CD_DESC, ");
        sb.AppendLine(" 	               SHIFTH.DUTY_STIME, ");
        sb.AppendLine(" 	               SHIFTH.DUTY_ETIME, ");
        sb.AppendLine(" 	               TDMEDD.CALENDAR_CD, ");
        sb.AppendLine(" 	               TDMEDD.WORK_SHIFT_CD, ");
        sb.AppendLine(" 	               TDMEDD.DT_TYPE, TDMEDD.DT_TYPE +'-'+T9MCD_DT_TYPE.SUB_DESC DT_TYPE_DESC ");
        sb.Append(" from  ( " + sb_EMP_DAY_DUTY + " ) TDMEDD");
        //sb.AppendLine("             from (select * from  TB_D_M_EMP_DAY_DUTY  TDMEDD with (nolock)  )  TDMEDD ");
        sb.AppendLine("             left join VW_H_EMP_DATA VHED on VHED.EMP_ID=TDMEDD.EMP_ID ");
        sb.AppendLine("             left join TB_9_M_COMM_D T9MCD_PLANT_CD on T9MCD_PLANT_CD.SUB_CD=VHED.PLANT_CD and T9MCD_PLANT_CD.MAIN_CD='PLANT_CD' and T9MCD_PLANT_CD.SYS_CD='HB' ");
        sb.AppendLine("             left join TB_9_M_COMM_D T9MCD_WORK_DAY_CD on T9MCD_WORK_DAY_CD.SUB_CD=TDMEDD.WORK_DAY_CD and T9MCD_WORK_DAY_CD.MAIN_CD='WORK_DAY_CD' and T9MCD_WORK_DAY_CD.SYS_CD='DA' ");
        sb.AppendLine("             left join TB_9_M_COMM_D T9MCD_WORK_CD_CD on T9MCD_WORK_CD_CD.SUB_CD=VHED.WORK_CD and T9MCD_WORK_CD_CD.MAIN_CD='WORK_CD' and T9MCD_WORK_CD_CD.SYS_CD='HB' ");
        sb.AppendLine("             left join TB_9_M_COMM_D T9MCD_DT_TYPE on T9MCD_DT_TYPE.SUB_CD=TDMEDD.DT_TYPE and T9MCD_DT_TYPE.MAIN_CD='DT_TYPE' and T9MCD_DT_TYPE.SYS_CD='DA' ");
        sb.AppendLine("             left join TB_D_M_SHIFT_H SHIFTH on SHIFTH.SHIFT_CD=TDMEDD.SHIFT_CD and TDMEDD.CALENDAR_DT >= SHIFTH.START_DT and TDMEDD.CALENDAR_DT <= SHIFTH.END_DT ");
        sb.AppendLine("	            where 1=1 ");
        //若有輸入工廠區分，A.工號 in(select 工號 from VW_H_EMP_DATA(員工人事資料VIEW) where 在職狀態 in('01','02') and 工廠區分 = 畫面.工廠區分)
        if (PLANT_CD != Resources.Resource.wfb2db_dll_PlaceChoice)
        {
            sb.AppendLine(" and VHED.EMP_ID in (select EMP_ID  from VW_H_EMP_DATA where  PLANT_CD =@PLANT_CD) ");
            ht.Add("@PLANT_CD", PLANT_CD);
        }
        //若有輸入部門，A.工號 in(select 工號 from VW_H_EMP_DATA(員工人事資料VIEW) where 在職狀態 in('01','02') and 部門代號 = 畫面.部門代號)	
        if (!string.IsNullOrEmpty(DEPT_NO))
        {
            sb.AppendLine(" and  VHED.EMP_ID in (select EMP_ID  from VW_H_EMP_DATA where  DEPT_NO =@DEPT_NO) ");
            ht.Add("@DEPT_NO", DEPT_NO);
        }
        //若有輸入入社日期起，A.工號 in(select 工號 from VW_H_EMP_DATA(員工人事資料VIEW) where 在職狀態 in('01','02') and 入社日期 >= 畫面.入社日期起)																																																																					
        if (!string.IsNullOrEmpty(JOIN_DT_Start))
        {
            sb.AppendLine("  and VHED.EMP_ID in (select EMP_ID  from VW_H_EMP_DATA where  JOIN_DT >=@JOIN_DT_Start) ");
            ht.Add("@JOIN_DT_Start", JOIN_DT_Start);
        }
        //若有輸入入社日期迄，A.工號 in(select 工號 from VW_H_EMP_DATA(員工人事資料VIEW) where 在職狀態 in('01','02') and 入社日期 <= 畫面.入社日期迄)																																																																					
        if (!string.IsNullOrEmpty(JOIN_DT_End))
        {
            sb.AppendLine(" and VHED.EMP_ID in (select EMP_ID  from VW_H_EMP_DATA where  JOIN_DT <=@JOIN_DT_End) ");
            ht.Add("@JOIN_DT_End", JOIN_DT_End);
        }

        sb.AppendLine(" )a) GRID_DATA where RowNumber between CAST(@startRowIndex+1 as varchar) ");
        sb.AppendLine("                     AND CAST(@startRowIndex+@maximumRows as varchar)");
        ht.Add("@startRowIndex", startRowIndex);
        ht.Add("@maximumRows", maximumRows);

        DataTable returnDt = dbConn.Query(sb, ht);

        return returnDt;
    }

    public WFB2DB0200DAO GetSingleData(WFB2DB0200DAO dao)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.AppendLine(" select VHED.PLANT_CD, ");
        sb.AppendLine(" 	   T9MCD_PLANT_CD.SUB_DESC PLANT, ");
        sb.AppendLine(" 	   VHED.DEPT_NO, ");
        sb.AppendLine(" 	   VHED.DEPT_FULL_NAME DEPT_NAME, ");
        sb.AppendLine(" 	   VHED.EMP_ID, ");
        sb.AppendLine(" 	   VHED.EMP_NAME, ");
        sb.AppendLine(" 	   convert(varchar, TDMEDD.CALENDAR_DT, 111) CALENDAR_DT, ");
        sb.AppendLine(" 	   TDMEDD.CALENDAR_DT relCALENDAR_DT, ");
        sb.AppendLine(" 	   TDMEDD.WORK_DAY_CD, ");
        sb.AppendLine(" 	   T9MCD_WORK_DAY_CD.SUB_DESC WORK_DAY, ");
        sb.AppendLine(" 	   TDMEDD.SHIFT_CD, ");
        sb.AppendLine(" 	   TDMS.SHIFT_DESC SHIFT, ");
        sb.AppendLine(" 	   TDMEDD.CALENDAR_CD, ");
        sb.AppendLine(" 	   TDMWS.WORK_SHIFT_DESC, ");
        sb.AppendLine(" 	   TDMEDD.WORK_SHIFT_CD, ");
        sb.AppendLine(" 	   TDMEDD.SHIFT_TIME_CD, ");
        sb.AppendLine(" 	   TDMEDD.WORK_HOUR, ");
        sb.AppendLine(" 	   TDMEDD.WORK_PERIOD_HOUR, ");
        sb.AppendLine(" 	   SHIFTH.DUTY_STIME, ");
        sb.AppendLine(" 	   SHIFTH.DUTY_ETIME, ");
        sb.AppendLine("        SHIFTD_B.DUTY_BEFORE_REST_STIME_1 DINING_STIME_1, ");
        sb.AppendLine("        SHIFTD_B.DUTY_BEFORE_REST_ETIME_1 DINING_ETIME_1, ");
        sb.AppendLine("        SHIFTD_BR.DUTY_BEFORE_REST_STIME_1 , ");
        sb.AppendLine("        SHIFTD_BR.DUTY_BEFORE_REST_ETIME_1 , ");
        sb.AppendLine("        SHIFTD_L.DUTY_BEFORE_REST_STIME_1 DINING_STIME_2, ");
        sb.AppendLine("        SHIFTD_L.DUTY_BEFORE_REST_ETIME_1 DINING_ETIME_2, ");
        sb.AppendLine("        SHIFTD_DR1.DUTY_BEFORE_REST_STIME_1 REST_STIME_1, ");
        sb.AppendLine("        SHIFTD_DR1.DUTY_BEFORE_REST_ETIME_1 REST_ETIME_1, ");
        sb.AppendLine("        SHIFTD_DR2.DUTY_BEFORE_REST_STIME_1 REST_STIME_2, ");
        sb.AppendLine("        SHIFTD_DR2.DUTY_BEFORE_REST_ETIME_1 REST_ETIME_2, ");
        sb.AppendLine("        SHIFTD_DR3.DUTY_BEFORE_REST_STIME_1 REST_STIME_3, ");
        sb.AppendLine("        SHIFTD_DR3.DUTY_BEFORE_REST_ETIME_1 REST_ETIME_3, ");
        sb.AppendLine("        SHIFTD_D.DUTY_BEFORE_REST_STIME_1 DINING_STIME_3, ");
        sb.AppendLine("        SHIFTD_D.DUTY_BEFORE_REST_ETIME_1 DINING_ETIME_3,  ");
        sb.AppendLine("        SHIFTD_AR1.DUTY_BEFORE_REST_STIME_1 DUTY_AFTER_REST_STIME_1, ");
        sb.AppendLine("        SHIFTD_AR1.DUTY_BEFORE_REST_ETIME_1 DUTY_AFTER_REST_ETIME_1, ");
        sb.AppendLine("        SHIFTD_AR2.DUTY_BEFORE_REST_STIME_1 DUTY_AFTER_REST_STIME_2,  ");
        sb.AppendLine("        SHIFTD_AR2.DUTY_BEFORE_REST_ETIME_1 DUTY_AFTER_REST_ETIME_2, ");
        sb.AppendLine(" 	   TDMEDD.WORK_SHIFT_ALLOWANCE_TYPE ");
        sb.AppendLine(" from ( select * from  TB_D_M_EMP_DAY_DUTY   where EMP_ID=@EMP_ID and CALENDAR_DT=@CALENDAR_DT ) TDMEDD ");
        sb.AppendLine(" left join VW_H_EMP_DATA VHED on VHED.EMP_ID=TDMEDD.EMP_ID ");
        sb.AppendLine(" left join TB_9_M_COMM_D T9MCD_PLANT_CD on T9MCD_PLANT_CD.SUB_CD=VHED.PLANT_CD and T9MCD_PLANT_CD.MAIN_CD='PLANT_CD' ");
        sb.AppendLine(" left join TB_9_M_COMM_D T9MCD_WORK_DAY_CD on T9MCD_WORK_DAY_CD.SUB_CD=TDMEDD.WORK_DAY_CD and T9MCD_WORK_DAY_CD.MAIN_CD='WORK_DAY_CD' ");
        sb.AppendLine(" left join TB_D_M_SHIFT_H TDMS on TDMS.SHIFT_CD=TDMEDD.SHIFT_CD and TDMEDD.CALENDAR_DT >= TDMS.START_DT and TDMEDD.CALENDAR_DT <= TDMS.END_DT ");
        sb.AppendLine(" left join TB_D_M_WORK_SHIFT_H TDMWS ON TDMWS.WORK_SHIFT_CD=TDMEDD.WORK_SHIFT_CD ");
        sb.AppendLine(" left join TB_D_M_SHIFT_H SHIFTH on SHIFTH.SHIFT_CD=TDMEDD.SHIFT_CD and TDMEDD.CALENDAR_DT >= SHIFTH.START_DT and TDMEDD.CALENDAR_DT <= SHIFTH.END_DT ");
        sb.AppendLine(" left join TB_D_M_SHIFT_D SHIFTD_B on SHIFTD_B.SHIFT_CD=TDMEDD.SHIFT_CD and TDMS.START_DT=SHIFTD_B.START_DT and SUBSTRING(SHIFTD_B.TIME_CD,2,1)='B' ");
        sb.AppendLine(" left join TB_D_M_SHIFT_D SHIFTD_BR on SHIFTD_BR.SHIFT_CD=TDMEDD.SHIFT_CD and TDMS.START_DT=SHIFTD_BR.START_DT and SUBSTRING(SHIFTD_BR.TIME_CD,1,2)='BR' ");
        sb.AppendLine(" left join TB_D_M_SHIFT_D SHIFTD_L on SHIFTD_L.SHIFT_CD=TDMEDD.SHIFT_CD and TDMS.START_DT=SHIFTD_L.START_DT and SUBSTRING(SHIFTD_L.TIME_CD,2,1)='L' ");
        sb.AppendLine(" left join TB_D_M_SHIFT_D SHIFTD_D on SHIFTD_D.SHIFT_CD=TDMEDD.SHIFT_CD and TDMS.START_DT=SHIFTD_D.START_DT and SUBSTRING(SHIFTD_D.TIME_CD,2,1)='D' ");
        sb.AppendLine(" left join TB_D_M_SHIFT_D SHIFTD_DR1 on SHIFTD_DR1.SHIFT_CD=TDMEDD.SHIFT_CD and TDMS.START_DT=SHIFTD_DR1.START_DT and SHIFTD_DR1.TIME_CD='DR1' ");
        sb.AppendLine(" left join TB_D_M_SHIFT_D SHIFTD_DR2 on SHIFTD_DR2.SHIFT_CD=TDMEDD.SHIFT_CD and TDMS.START_DT=SHIFTD_DR2.START_DT and SHIFTD_DR2.TIME_CD='DR2' ");
        sb.AppendLine(" left join TB_D_M_SHIFT_D SHIFTD_DR3 on SHIFTD_DR3.SHIFT_CD=TDMEDD.SHIFT_CD and TDMS.START_DT=SHIFTD_DR3.START_DT and SHIFTD_DR3.TIME_CD='DR3' ");
        sb.AppendLine(" left join TB_D_M_SHIFT_D SHIFTD_AR1 on SHIFTD_AR1.SHIFT_CD=TDMEDD.SHIFT_CD and TDMS.START_DT=SHIFTD_AR1.START_DT and SHIFTD_AR1.TIME_CD='AR1' ");
        sb.AppendLine(" left join TB_D_M_SHIFT_D SHIFTD_AR2 on SHIFTD_AR2.SHIFT_CD=TDMEDD.SHIFT_CD and TDMS.START_DT=SHIFTD_AR2.START_DT and SHIFTD_AR2.TIME_CD='AR2' ");
        sb.AppendLine(" where VHED.EMP_ID=@EMP_ID and TDMEDD.CALENDAR_DT=@CALENDAR_DT ");
        ht.Add("@EMP_ID", dao.EMP_ID);
        ht.Add("@CALENDAR_DT", dao.CALENDAR_DT);
        return (from item in dbConn.Query(sb, ht).AsEnumerable()
                select new WFB2DB0200DAO
                {
                    EMP_ID = (item.Table.Columns.Contains("EMP_ID") ? item.Field<string>("EMP_ID") : null),
                    EMP_NAME = (item.Table.Columns.Contains("EMP_NAME") ? item.Field<string>("EMP_NAME") : null),
                    PLANT_CD = (item.Table.Columns.Contains("PLANT_CD") ? item.Field<string>("PLANT_CD") : null),
                    PLANT = (item.Table.Columns.Contains("PLANT") ? item.Field<string>("PLANT") : null),
                    DEPT_NAME = (item.Table.Columns.Contains("DEPT_NAME") ? item.Field<string>("DEPT_NAME") : null),
                    CALENDAR_DT = item.Field<DateTime>("relCALENDAR_DT"),
                    WORK_DAY_CD = (item.Table.Columns.Contains("WORK_DAY_CD") ? item.Field<string>("WORK_DAY_CD") : null),
                    WORK_DAY = (item.Table.Columns.Contains("WORK_DAY") ? item.Field<string>("WORK_DAY") : null),
                    WORK_SHIFT_DESC = (item.Table.Columns.Contains("WORK_SHIFT_DESC") ? item.Field<string>("WORK_SHIFT_DESC") : null),
                    WORK_SHIFT_CD = (item.Table.Columns.Contains("WORK_SHIFT_CD") ? item.Field<string>("WORK_SHIFT_CD") : null),
                    SHIFT_CD = (item.Table.Columns.Contains("SHIFT_CD") ? item.Field<string>("SHIFT_CD") : null),
                    SHIFT = (item.Table.Columns.Contains("SHIFT") ? item.Field<string>("SHIFT") : null),
                    SHIFT_TIME_CD = (item.Table.Columns.Contains("SHIFT_TIME_CD") ? item.Field<string>("SHIFT_TIME_CD") : null),
                    WORK_HOUR = (item.Table.Columns.Contains("WORK_HOUR") ? item.Field<string>("WORK_HOUR") : null),
                    WORK_PERIOD_HOUR = (item.Table.Columns.Contains("WORK_PERIOD_HOUR") ? item.Field<string>("WORK_PERIOD_HOUR") : null),
                    DUTY_STIME = (item.Table.Columns.Contains("DUTY_STIME") ? item.Field<string>("DUTY_STIME") : null),
                    DUTY_ETIME = (item.Table.Columns.Contains("DUTY_ETIME") ? item.Field<string>("DUTY_ETIME") : null),
                    DUTY_BEFORE_REST_STIME_1 = (item.Table.Columns.Contains("DUTY_BEFORE_REST_STIME_1") ? item.Field<string>("DUTY_BEFORE_REST_STIME_1") : null),
                    DUTY_BEFORE_REST_ETIME_1 = (item.Table.Columns.Contains("DUTY_BEFORE_REST_ETIME_1") ? item.Field<string>("DUTY_BEFORE_REST_ETIME_1") : null),
                    DINING_STIME_1 = (item.Table.Columns.Contains("DINING_STIME_1") ? item.Field<string>("DINING_STIME_1") : null),
                    DINING_ETIME_1 = (item.Table.Columns.Contains("DINING_ETIME_1") ? item.Field<string>("DINING_ETIME_1") : null),
                    DINING_STIME_2 = (item.Table.Columns.Contains("DINING_STIME_2") ? item.Field<string>("DINING_STIME_2") : null),
                    DINING_ETIME_2 = (item.Table.Columns.Contains("DINING_ETIME_2") ? item.Field<string>("DINING_ETIME_2") : null),
                    REST_STIME_1 = (item.Table.Columns.Contains("REST_STIME_1") ? item.Field<string>("REST_STIME_1") : null),
                    REST_ETIME_1 = (item.Table.Columns.Contains("REST_ETIME_1") ? item.Field<string>("REST_ETIME_1") : null),
                    REST_STIME_2 = (item.Table.Columns.Contains("REST_STIME_2") ? item.Field<string>("REST_STIME_2") : null),
                    REST_ETIME_2 = (item.Table.Columns.Contains("REST_ETIME_2") ? item.Field<string>("REST_ETIME_2") : null),
                    REST_STIME_3 = (item.Table.Columns.Contains("REST_STIME_3") ? item.Field<string>("REST_STIME_3") : null),
                    REST_ETIME_3 = (item.Table.Columns.Contains("REST_ETIME_3") ? item.Field<string>("REST_ETIME_3") : null),
                    DINING_STIME_3 = (item.Table.Columns.Contains("DINING_STIME_3") ? item.Field<string>("DINING_STIME_3") : null),
                    DINING_ETIME_3 = (item.Table.Columns.Contains("DINING_ETIME_3") ? item.Field<string>("DINING_ETIME_3") : null),
                    DUTY_AFTER_REST_STIME_1 = (item.Table.Columns.Contains("DUTY_AFTER_REST_STIME_1") ? item.Field<string>("DUTY_AFTER_REST_STIME_1") : null),
                    DUTY_AFTER_REST_ETIME_1 = (item.Table.Columns.Contains("DUTY_AFTER_REST_ETIME_1") ? item.Field<string>("DUTY_AFTER_REST_ETIME_1") : null),
                    DUTY_AFTER_REST_STIME_2 = (item.Table.Columns.Contains("DUTY_AFTER_REST_STIME_2") ? item.Field<string>("DUTY_AFTER_REST_STIME_2") : null),
                    DUTY_AFTER_REST_ETIME_2 = (item.Table.Columns.Contains("DUTY_AFTER_REST_ETIME_2") ? item.Field<string>("DUTY_AFTER_REST_ETIME_2") : null),
                    WORK_SHIFT_ALLOWANCE_TYPE = (item.Table.Columns.Contains("WORK_SHIFT_ALLOWANCE_TYPE") ? item.Field<string>("WORK_SHIFT_ALLOWANCE_TYPE") : null)
                }).ToList().First();
    }

    //改寫成直接執行SQL前
    //public bool UpdateData(WFB2DB0200DAO dao, out string Message)
    //{
    //    try
    //    {
    //        Message = string.Empty;
    //        StringBuilder sb = new StringBuilder();
    //        Hashtable ht = new Hashtable();
    //        sb.Append("SP_D_UPD_EMP_DAY_DUTY3");
    //        ht.Add("@pEmpId", dao.EMP_ID);
    //        ht.Add("@pWorkShiftCd", dao.WORK_DAY_CD);
    //        ht.Add("@pCalendarDtS", dao.CALENDAR_DT);
    //        ht.Add("@pCalendarDtE", dao.CALENDAR_DT);
    //        ht.Add("@pShiftCd", dao.SHIFT_CD);
    //        ht.Add("@pUserID", dao.UPDATED_BY);
    //        ht.Add("@pFuncID", dao.FUNC_ID);
    //        dbConn.ExecuteSP(sb, ht, true);
    //        return true;
    //    }
    //    catch (Exception ex)
    //    {
    //        Message = ex.Message;
    //        return false;
    //    }
    //}

    public DataTable GetTB_D_M_SHIFT_H(string SHIFT_CD, DateTime CALENDAR_DT)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();

        sb.AppendLine("select SHIFTH.SHIFT_DESC,                                                                                                                                 ");
        sb.AppendLine("       SHIFTH.SHIFT_TIME_CD,                                                                                                                              ");
        sb.AppendLine("       SHIFTH.WORK_HOUR,                                                                                                                                  ");
        sb.AppendLine("       SHIFTH.WORK_PERIOD_HOUR,                                                                                                                           ");
        sb.AppendLine("       SHIFTH.DUTY_STIME,                                                                                                                                 ");
        sb.AppendLine("       SHIFTH.DUTY_ETIME,                                                                                                                                 ");
        sb.AppendLine("       SHIFTD_B.DUTY_BEFORE_REST_STIME_1 DINING_STIME_1,                                                                                                  ");
        sb.AppendLine("       SHIFTD_B.DUTY_BEFORE_REST_ETIME_1 DINING_ETIME_1,                                                                                                  ");
        sb.AppendLine("       SHIFTD_BR.DUTY_BEFORE_REST_STIME_1 ,                                                                                                               ");
        sb.AppendLine("       SHIFTD_BR.DUTY_BEFORE_REST_ETIME_1 ,                                                                                                               ");
        sb.AppendLine("       SHIFTD_L.DUTY_BEFORE_REST_STIME_1 DINING_STIME_2,                                                                                                  ");
        sb.AppendLine("       SHIFTD_L.DUTY_BEFORE_REST_ETIME_1 DINING_ETIME_2,                                                                                                  ");
        sb.AppendLine("       SHIFTD_DR1.DUTY_BEFORE_REST_STIME_1 REST_STIME_1,                                                                                                  ");
        sb.AppendLine("       SHIFTD_DR1.DUTY_BEFORE_REST_ETIME_1 REST_ETIME_1,                                                                                                  ");
        sb.AppendLine("       SHIFTD_DR2.DUTY_BEFORE_REST_STIME_1 REST_STIME_2,                                                                                                  ");
        sb.AppendLine("       SHIFTD_DR2.DUTY_BEFORE_REST_ETIME_1 REST_ETIME_2,                                                                                                  ");
        sb.AppendLine("       SHIFTD_DR3.DUTY_BEFORE_REST_STIME_1 REST_STIME_3,                                                                                                  ");
        sb.AppendLine("       SHIFTD_DR3.DUTY_BEFORE_REST_ETIME_1 REST_ETIME_3,                                                                                                  ");
        sb.AppendLine("       SHIFTD_D.DUTY_BEFORE_REST_STIME_1 DINING_STIME_3,                                                                                                  ");
        sb.AppendLine("       SHIFTD_D.DUTY_BEFORE_REST_ETIME_1 DINING_ETIME_3,                                                                                                  ");
        sb.AppendLine("       SHIFTD_AR1.DUTY_BEFORE_REST_STIME_1 DUTY_AFTER_REST_STIME_1,                                                                                       ");
        sb.AppendLine("       SHIFTD_AR1.DUTY_BEFORE_REST_ETIME_1 DUTY_AFTER_REST_ETIME_1,                                                                                       ");
        sb.AppendLine("       SHIFTD_AR2.DUTY_BEFORE_REST_STIME_1 DUTY_AFTER_REST_STIME_2,                                                                                       ");
        sb.AppendLine("       SHIFTD_AR2.DUTY_BEFORE_REST_ETIME_1 DUTY_AFTER_REST_ETIME_2, 	                                                                                     ");
        //TODO
        sb.AppendLine("       SHIFTD_ALLOWANCE.SUB_CD+'-'+SHIFTD_ALLOWANCE.SUB_DESC as ALLOWANCE_desc, 	                                                                                     ");
        sb.AppendLine("       SHIFTD_TIME.SUB_CD+'-'+SHIFTD_TIME.SUB_DESC as TIME_desc	                                                                                     ");
        sb.AppendLine(" from TB_D_M_SHIFT_H SHIFTH                                                                                                                               ");
        sb.AppendLine(" left join TB_D_M_SHIFT_D SHIFTD_B on SHIFTD_B.SHIFT_CD=SHIFTH.SHIFT_CD and SHIFTH.START_DT=SHIFTD_B.START_DT and SUBSTRING(SHIFTD_B.TIME_CD,2,1)='B'     ");
        sb.AppendLine(" left join TB_D_M_SHIFT_D SHIFTD_BR on SHIFTD_BR.SHIFT_CD=SHIFTH.SHIFT_CD and SHIFTH.START_DT=SHIFTD_BR.START_DT and SUBSTRING(SHIFTD_BR.TIME_CD,1,2)='BR'");
        sb.AppendLine(" left join TB_D_M_SHIFT_D SHIFTD_L on SHIFTD_L.SHIFT_CD=SHIFTH.SHIFT_CD and SHIFTH.START_DT=SHIFTD_L.START_DT and SUBSTRING(SHIFTD_L.TIME_CD,2,1)='L'     ");
        sb.AppendLine(" left join TB_D_M_SHIFT_D SHIFTD_D on SHIFTD_D.SHIFT_CD=SHIFTH.SHIFT_CD and SHIFTH.START_DT=SHIFTD_D.START_DT and SUBSTRING(SHIFTD_D.TIME_CD,2,1)='D'     ");
        sb.AppendLine(" left join TB_D_M_SHIFT_D SHIFTD_DR1 on SHIFTD_DR1.SHIFT_CD=SHIFTH.SHIFT_CD and SHIFTH.START_DT=SHIFTD_DR1.START_DT and SHIFTD_DR1.TIME_CD='DR1'          ");
        sb.AppendLine(" left join TB_D_M_SHIFT_D SHIFTD_DR2 on SHIFTD_DR2.SHIFT_CD=SHIFTH.SHIFT_CD and SHIFTH.START_DT=SHIFTD_DR2.START_DT and SHIFTD_DR2.TIME_CD='DR2'          ");
        sb.AppendLine(" left join TB_D_M_SHIFT_D SHIFTD_DR3 on SHIFTD_DR3.SHIFT_CD=SHIFTH.SHIFT_CD and SHIFTH.START_DT=SHIFTD_DR3.START_DT and SHIFTD_DR3.TIME_CD='DR3'          ");
        sb.AppendLine(" left join TB_D_M_SHIFT_D SHIFTD_AR1 on SHIFTD_AR1.SHIFT_CD=SHIFTH.SHIFT_CD and SHIFTH.START_DT=SHIFTD_AR1.START_DT and SHIFTD_AR1.TIME_CD='AR1'          ");
        sb.AppendLine(" left join TB_D_M_SHIFT_D SHIFTD_AR2 on SHIFTD_AR2.SHIFT_CD=SHIFTH.SHIFT_CD and SHIFTH.START_DT=SHIFTD_AR2.START_DT and SHIFTD_AR2.TIME_CD='AR2'          ");
        //todo
        sb.AppendLine(" left join TB_9_M_COMM_D SHIFTD_ALLOWANCE on SHIFTH.WORK_SHIFT_ALLOWANCE_TYPE=SHIFTD_ALLOWANCE.SUB_CD and SHIFTD_ALLOWANCE.SYS_CD='SC' and SHIFTD_ALLOWANCE.MAIN_CD='WORK_SHIFT_ALLOWANCE_TYPE' and SHIFTD_ALLOWANCE.IS_VALID='Y'");
        sb.AppendLine(" left join TB_9_M_COMM_D SHIFTD_TIME on SHIFTH.SHIFT_TIME_CD=SHIFTD_TIME.SUB_CD and SHIFTD_TIME.SYS_CD='DA' and SHIFTD_TIME.MAIN_CD='SHIFT_TIME_CD' and SHIFTD_TIME.IS_VALID='Y'");
        sb.AppendLine(" where SHIFTH.SHIFT_CD=@SHIFT_CD AND SHIFTH.START_DT<=@CALENDAR_DT  AND SHIFTH.END_DT>=@CALENDAR_DT                                                            ");

        ht.Add("@SHIFT_CD", SHIFT_CD);
        ht.Add("@CALENDAR_DT", CALENDAR_DT);
        return dbConn.Query(sb, ht);

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
    public DataTable getDEPT_NAME(string dept_no)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select DEPT_NAME ");
            sb.AppendLine(" from VW_H_DEPT_DATA ");
            sb.AppendLine(" where DEPT_NO = @DEPT_NO ");
            ht.Add("@DEPT_NO", dept_no);

            return dbConn.Query(sb, ht, true);
        }
        catch
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
            ht.Add("@WORK_SHIFT_CD", work_shift_cd);

            return dbConn.Query(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }

    public DataTable getSHIFT_CD(string emp_id, string calendar_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" SELECT SHIFT_CD,SHIFT_CD+'-'+SHIFT_DESC SHIFT_DESC FROM [dbo].[FN_DB020_01] ");
            sb.Append(" ( ");
            sb.Append(" @p_EMP_ID,@p_CALENDAR_DT ");
            sb.Append(" ) ");

            ht.Add("@p_EMP_ID", emp_id);
            ht.Add("@p_CALENDAR_DT", calendar_dt);
            return dbConn.Query(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }

    public DataTable getSHIFT_CD_Batch(string emp_id, string calendar_dt, string updateShiftCD)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" SELECT SHIFT_CD,SHIFT_CD+'-'+SHIFT_DESC SHIFT_DESC FROM [dbo].[FN_DB020_01] ");
            sb.Append(" ( ");
            sb.Append(" @p_EMP_ID,@p_CALENDAR_DT ");
            sb.Append(" ) where SHIFT_CD = @SHIFT_CD ");

            ht.Add("@p_EMP_ID", emp_id);
            ht.Add("@p_CALENDAR_DT", calendar_dt);
            ht.Add("@SHIFT_CD", updateShiftCD);
            return dbConn.Query(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }

    public DataTable getSHIFT_CD_ALL(string emp_id, string calendar_dt)
    {
        try
        {
            string resultCode = InitialView();

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            if (resultCode.IndexOf('A') != -1)
            {
                sb.Append(" select SHIFT_CD,SHIFT_CD+'-'+SHIFT_DESC as SHIFT_DESC from TB_D_M_SHIFT_H H");
                sb.Append(" WHERE  @calendar_dt between H.START_DT and H.END_DT");
            }
            else
            {
                sb.Append(" select DISTINCT D.SHIFT_CD,D.SHIFT_CD +'-'+H.SHIFT_DESC AS SHIFT_DESC ");
                sb.Append(" from TB_D_M_EMP_DAY_DUTY A ");
                sb.Append(" LEFT JOIN TB_D_M_WORK_SHIFT_D D ON A.WORK_SHIFT_CD = D.WORK_SHIFT_CD ");
                sb.Append(" LEFT JOIN TB_D_M_SHIFT_H H ON D.SHIFT_CD = H.SHIFT_CD ");
                sb.Append(" WHERE  A.EMP_ID = @EMP_ID AND @calendar_dt between H.START_DT and H.END_DT");

                //sb.Append(" WHERE  A.EMP_ID = @EMP_ID AND CONVERT(VARCHAR(4),A.CALENDAR_DT,112)= @YEAR ");

                if (resultCode.IndexOf('B') != -1)
                {
                    sb.Append(" UNION ");
                    sb.Append(" SELECT C.SUB_CD AS SHIFT_CD,C.SUB_CD +'-'+ H.SHIFT_DESC AS SHIFT_DESC ");
                    sb.Append(" FROM TB_9_M_COMM_D C ");
                    sb.Append(" LEFT JOIN TB_D_M_SHIFT_H H ON C.SUB_CD = H.SHIFT_CD AND @calendar_dt between H.START_DT and H.END_DT");
                    sb.Append(" WHERE C.SYS_CD ='DB' and MAIN_CD ='SHIFT_CD' ");
                }
            }
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@calendar_dt", calendar_dt);
            //ht.Add("@YEAR", year);
            return dbConn.Query(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }
    

    private string InitialView()
    {

        ACESLib.ACES aces = new ACESLib.ACES();
        string SysCode = "";    //大分類代碼
        string syscodeatt = "";
        string resultCode = "";

        foreach (string DB_ROLE_CD in aces.GetRoles().Split(',')) //取得「資料角色代碼」
        {
            try
            {

                SysCode = ((ACESLib.DEPTBean)aces.GetDEPTAuth(DB_ROLE_CD.Trim())).SysCode;        //取得此資料角色「大分類代碼」

                foreach (string big_sysCode in SysCode.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
                {
                    if (big_sysCode.Trim().Equals("SHIFT_CD"))
                    {
                        syscodeatt = aces.GetCodeAtt(DB_ROLE_CD.Trim(), big_sysCode.Trim());
                        syscodeatt = syscodeatt.Trim();
                        if (resultCode == "")                       //一個人不只一個資料角色 第一個角色撈出的小分類
                            resultCode = "," + syscodeatt + ",";
                        else
                        {
                            foreach (string code in syscodeatt.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
                            {
                                if (resultCode.IndexOf(code.Trim()) == -1)  //resultCode 沒有就加進去
                                    resultCode += code.Trim() + ",";
                            }
                        }
                    }
                }
            }
            catch
            {

            }
        }

        return resultCode;
    }


    public void UpdateData(WFB2DB0200DAO dao)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            //2.工號+勤務日期 => 指定特定的班別
            //2.1.更新日勤務班表資料檔
            sb.AppendLine(" UPDATE  a                                                                             ");
            sb.AppendLine(" Set [SHIFT_CD] = b.SHIFT_CD                                                                               ");
            sb.AppendLine("    ,[SHIFT_TIME_CD] = b.SHIFT_TIME_CD                                                                     ");
            sb.AppendLine("    ,[WORK_HOUR] = b.[WORK_HOUR]                                                                           ");
            sb.AppendLine("    ,[WORK_PERIOD_HOUR] = b.[WORK_PERIOD_HOUR]                                                             ");
            sb.AppendLine("    ,[DUTY_STIME] = b.[DUTY_STIME]                                                                         ");
            sb.AppendLine("    ,[DUTY_ETIME] = b.[DUTY_ETIME]                                                                         ");
            sb.AppendLine("    ,[WORK_SHIFT_ALLOWANCE_TYPE] = b.[WORK_SHIFT_ALLOWANCE_TYPE]				                              ");
            sb.AppendLine("    ,WORK_DAY_CD = @WORK_DAY_CD		                                                                      ");
            sb.AppendLine("    ,[UPDATED_BY] = @UPDATED_BY                                                                            ");
            sb.AppendLine("    ,[UPDATED_DT] = getDate()                                                                              ");
            sb.AppendLine("    ,[FUNC_ID] = @FuncID                                                                                   ");
            sb.AppendLine(" From (select * from  TB_D_M_EMP_DAY_DUTY where CALENDAR_DT = @CALENDAR_DT and EMP_ID = @EMP_ID  )a                                                                                ");
            sb.AppendLine(" Inner Join (                                                                                              ");
            sb.AppendLine(" 	  select D.SHIFT_TIME_CD                                                                              ");
            sb.AppendLine(" 			,D.WORK_HOUR,D.WORK_PERIOD_HOUR,dbo.FN_D_GEN_DATETIME(@CALENDAR_DT_S,D.DUTY_STIME) DUTY_STIME ");
            sb.AppendLine(" 			,dbo.FN_D_GEN_DATETIME(@CALENDAR_DT_S,D.DUTY_ETIME) DUTY_ETIME                                ");
            sb.AppendLine(" 			,D.WORK_SHIFT_ALLOWANCE_TYPE,D.SHIFT_CD                                                       ");
            sb.AppendLine(" 		from TB_D_M_SHIFT_H D                                                                             ");
            sb.AppendLine("        where @CALENDAR_DT_S >= D.START_DT                                                                 ");
            sb.AppendLine(" 	     and @CALENDAR_DT_E <= D.END_DT                                                                   ");
            sb.AppendLine(" 		 and SHIFT_CD = @SHIFT_CD                                                                          ");
            sb.AppendLine(" ) b                                                                                                       ");
            sb.AppendLine("  On a.CALENDAR_DT = @CALENDAR_DT_S                                                                        ");
            sb.AppendLine("  and a.EMP_ID = @EMP_ID                                                                                    ");
            ht.Add("@UPDATED_BY", dao.UPDATED_BY);
            ht.Add("@SHIFT_CD", dao.SHIFT_CD);
            ht.Add("@CALENDAR_DT_S", Convert.ToDateTime(dao.CALENDAR_DT).ToString("yyyy/MM/dd"));
            ht.Add("@CALENDAR_DT_E", Convert.ToDateTime(dao.CALENDAR_DT).ToString("yyyy/MM/dd"));
            ht.Add("@CALENDAR_DT", dao.CALENDAR_DT);
            ht.Add("@WORK_DAY_CD", dao.WORK_DAY_CD);
            ht.Add("@EMP_ID", dao.EMP_ID);
            ht.Add("@FuncID", SessionHandle.Current.FUNC_ID);
            dbConn.ExecuteT(sb, ht, true);
            ht.Clear();
            sb.Clear();

            sb.AppendLine(" --2.2.更新日勤務狀態資料檔	                                                                             ");
            sb.AppendLine(" update TB_D_M_EMP_DUTY_CHECK_STATUS                                                                      ");
            sb.AppendLine("   set [DUTY_CHECK_RESULT] = 'N',                                                                         ");
            sb.AppendLine("  LATE_HOUR= 0,LEAVE_EARLY_HOUR= 0,LACK_HOUR= 0,DUTY_HOUR= 0,LEAVE_HOUR= 0                                 ");
            sb.AppendLine("  ,LEAVE_INFO= '',OVERTIME_HOUR_APPLY= 0,OVERTIME_HOUR_APPROVE= 0,VIOLATE_BEFORE_HOUR= 0                    ");
            sb.AppendLine("  ,VIOLATE_AFTER_HOUR= 0,OVERTIME_INFO= '',SHIFT_CD= '',WORK_SHIFT_ALLOWANCE_TYPE= '',                       ");
            sb.AppendLine(" 	   UPDATED_BY = @UPDATED_BY,                                                                         ");
            sb.AppendLine(" 	   UPDATED_DT = getDate()                                                                            ");
            sb.AppendLine(" where EMP_ID = @EMP_ID                                                                                   ");
            sb.AppendLine("   and CALENDAR_DT = @CALENDAR_DT_S                                                                       ");
            sb.AppendLine("   and CALENDAR_DT>dbo.FN_S_DUTY_EDT('LM')                                                                      ");
            ht.Add("@UPDATED_BY", dao.UPDATED_BY);
            ht.Add("@CALENDAR_DT_S", Convert.ToDateTime(dao.CALENDAR_DT).ToString("yyyy/MM/dd"));
            ht.Add("@EMP_ID", dao.EMP_ID);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //班表間隔11小時檢核
    public string exec_SP_DH_SHIFT_DUTY_CHK(WFB2DB0200DAO dao)
    {
        try
        {

            string rtnMessage = "";
            string rtnFlag = "";
            using (SqlConnection conn = new SqlConnection(utilities.connstr))
            using (SqlCommand comm = new SqlCommand())
            {
                conn.Open();
                comm.Connection = conn;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = "SP_DH_SHIFT_DUTY_CHK";
                comm.Parameters.AddWithValue("@p_EMP_ID", dao.EMP_ID);
                comm.Parameters.AddWithValue("@p_SHIFT_CD", dao.SHIFT_CD);
                comm.Parameters.AddWithValue("@p_START_DT", dao.CALENDAR_DT);
                comm.Parameters.AddWithValue("@p_END_DT", dao.CALENDAR_DT);
                comm.Parameters.AddWithValue("@p_CHECK_CD", "A");   //前後都檢查

                comm.Parameters.Add("@p_RTN_FLAG", SqlDbType.NVarChar, 600).Direction = ParameterDirection.Output;
                comm.Parameters.Add("@p_RTN_MSG", SqlDbType.NVarChar, 600).Direction = ParameterDirection.Output;

                comm.ExecuteNonQuery();
                rtnFlag = (string)comm.Parameters["@p_RTN_FLAG"].Value;
                rtnMessage = (string)comm.Parameters["@p_RTN_MSG"].Value;
                conn.Close();
            }
            return rtnFlag + "|" + rtnMessage;
        }
        catch (Exception)
        {

            throw;
        }
    }


}