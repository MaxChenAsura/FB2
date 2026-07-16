using FB2.tw.co.toyota.kuozui.dao;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;

/// <summary>
/// Class1 的摘要描述
/// </summary>
[Serializable]
public class WFB2DA0200DtlDAO
{
    public string SHIFT_CD { get; set; }
    public DateTime START_DT { get; set; }
    public string TIME_CD { get; set; }
    public string IS_IFLOW_SHOW { get; set; }
    public string DUTY_BEFORE_REST_STIME_1 { get; set; }
    public string DUTY_BEFORE_REST_ETIME_1 { get; set; }
    public string CREATED_BY { get; set; }
    public DateTime CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public DateTime UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }
}

[Serializable]
public class WFB2DA0200DAO
{
    public string SHIFT_CD { get; set; }
    public DateTime START_DT { get; set; }
    public DateTime? END_DT { get; set; }
    public string IS_IFLOW_SHOW { get; set; }
    public string SHIFT_DESC { get; set; }
    public string SHIFT_TIME_CD { get; set; }
    public string WORK_HOUR { get; set; }
    public string WORK_PERIOD_HOUR { get; set; }
    public string DUTY_STIME { get; set; }
    public string DUTY_ETIME { get; set; }
    public string WORK_SHIFT_ALLOWANCE_TYPE { get; set; }
    public string R_SHIFT_CD { get; set; }
    public string REMARK { get; set; }
    public string CREATED_BY { get; set; }
    public DateTime CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public DateTime UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }
    public List<WFB2DA0200DtlDAO> Dtl { get; set; }
}

[Serializable]
public class WFB2DA0200EMP_DAY_DUTY_DAO
{
    public string CALENDAR_DT { get; set; }
    public string EMP_ID { get; set; }
}

public class WFB2DA0200DL : BaseDAO
{
    public int GetGridDataCount(int startRowIndex, int maximumRows, string SHIFT_CD,
                                string SHIFT_TIME_CD, string SHIFT_DESC, string VALID,
                                string START_DT, string END_DT, string DUTY_TIME,
                                string EAT_TIME, string REST_TIME, string WORK_SHIFT_ALLOWANCE_TYPE, string IS_IFLOW_SHOW)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" select COUNT(1) total_record   ");
            sb.AppendLine(" from TB_D_M_SHIFT_H SHIFTH ");
            sb.AppendLine(" where 1=1 ");
            //若有輸入時段別區別，A.時段別區分 = '畫面.時段別區分'	
            if (string.IsNullOrEmpty(SHIFT_TIME_CD.ToUpper()) == false)
            {
                sb.AppendLine("            and SHIFTH.SHIFT_TIME_CD=@SHIFT_TIME_CD ");
                ht.Add("@SHIFT_TIME_CD", SHIFT_TIME_CD.ToUpper());
            }

            //若有輸入班別代碼，A.班別代碼 LIKE '畫面.班別代碼%'																																																							
            if (string.IsNullOrEmpty(SHIFT_CD.ToUpper()) == false)
            {
                sb.AppendLine("            and SHIFTH.SHIFT_CD like @SHIFT_CD+'%' ");
                ht.Add("@SHIFT_CD", SHIFT_CD.ToUpper());
            }

            //若有輸入班別說明，A.班別說明 LIKE '%畫面.班別說明%'	
            if (string.IsNullOrEmpty(SHIFT_DESC) == false)
            {
                sb.AppendLine("            and SHIFTH.SHIFT_DESC like '%'+@SHIFT_DESC+'%' ");
                ht.Add("@SHIFT_DESC", SHIFT_DESC);
            }

            //若有輸入生效起日，A.結束日期>=畫面.生效起日
            if (string.IsNullOrEmpty(START_DT) == false)
            {
                sb.AppendLine("            and SHIFTH.END_DT>=@START_DT ");
                ht.Add("@START_DT", START_DT);
            }

            //若有輸入生效迄日，A.生效日期<=畫面.生效迄日
            //若有輸入生效迄日，且選取"有效"，A.結束日期>=畫面.生效迄日
            //若有輸入生效迄日，且選取"無效"，A.結束日期<畫面.生效迄日
            if (string.IsNullOrEmpty(END_DT) == false)
            {
                if (VALID == "N")
                    sb.AppendLine("            and SHIFTH.END_DT<@END_DT ");
                else if (VALID == "Y")
                    sb.AppendLine("            and SHIFTH.END_DT>=@END_DT ");
                else
                    sb.AppendLine("            and SHIFTH.START_DT<=@END_DT ");

                ht.Add("@END_DT", END_DT);
            }

            //若有輸入上班時間，畫面.上班時間 >= A.勤務上班時間 且 畫面.上班時間 <= A.勤務下班時間 	
            if (string.IsNullOrEmpty(DUTY_TIME) == false)
            {
                sb.AppendLine("            and SHIFTH.DUTY_STIME<=@DUTY_TIME ");
                sb.AppendLine("            and SHIFTH.DUTY_ETIME>=@DUTY_TIME ");
                ht.Add("@DUTY_TIME", DUTY_TIME);
            }

            //"若有輸入用餐時間，班別代碼 in(
            //"																														
            //                                        select 班別代碼 from 班別明細檔																				
            //                                           where substring(時段區分,2,1) in('B','L','D')																				
            //                                             and 結束時間 >=  畫面上.用餐時間																				
            //                                             and 開始時間 <=  畫面上.用餐時間)		
            if (string.IsNullOrEmpty(EAT_TIME) == false)
            {
                sb.AppendLine("            and SHIFTH.SHIFT_CD in (select SHIFT_CD ");
                sb.AppendLine("                                    from TB_D_M_SHIFT_D TDMSD");
                sb.AppendLine("                                    where TDMSD.FUNC_ID='FB2DA020' ");
                sb.AppendLine("                                      and substring(TDMSD.TIME_CD,2,1) in ('B','L','D') ");
                sb.AppendLine("                                      and TDMSD.DUTY_BEFORE_REST_STIME_1<=@EAT_TIME ");
                sb.AppendLine("                                      and TDMSD.DUTY_BEFORE_REST_ETIME_1>=@EAT_TIME) ");
                ht.Add("@EAT_TIME", EAT_TIME);
            }

            //"若有輸入休息時間，班別代碼 in(
            //"																														
            //                                        select 班別代碼 from 班別明細檔																				
            //                                           where substring(時段區分,2,1) in('R')																				
            //                                             and 結束時間 >=  畫面上.用餐時間起																				
            //                                             and 開始時間 <=  畫面上.用餐時間迄)		
            if (string.IsNullOrEmpty(REST_TIME) == false)
            {
                sb.AppendLine("            and SHIFTH.SHIFT_CD in (select SHIFT_CD ");
                sb.AppendLine("                                    from TB_D_M_SHIFT_D TDMSD");
                sb.AppendLine("                                    where TDMSD.FUNC_ID='FB2DA020' ");
                sb.AppendLine("                                      and substring(TDMSD.TIME_CD,2,1) in ('R') ");
                sb.AppendLine("                                      and TDMSD.DUTY_BEFORE_REST_STIME_1<=@REST_TIME ");
                sb.AppendLine("                                      and TDMSD.DUTY_BEFORE_REST_ETIME_1>=@REST_TIME) ");
                ht.Add("@REST_TIME", REST_TIME);
            }

            //若有輸入津貼，A.津貼=畫面.津貼
            if (string.IsNullOrEmpty(WORK_SHIFT_ALLOWANCE_TYPE) == false)
            {
                sb.AppendLine("            and SHIFTH.WORK_SHIFT_ALLOWANCE_TYPE=@WORK_SHIFT_ALLOWANCE_TYPE ");
                ht.Add("@WORK_SHIFT_ALLOWANCE_TYPE", WORK_SHIFT_ALLOWANCE_TYPE);
            }

            //IFLOW顯示否
            if (string.IsNullOrEmpty(IS_IFLOW_SHOW) == false)
            {
                sb.AppendLine("            and SHIFTH.IS_IFLOW_SHOW=@IS_IFLOW_SHOW ");
                ht.Add("@IS_IFLOW_SHOW", IS_IFLOW_SHOW);
            }

            Int32 ReturnValue = Convert.ToInt32(dbConn.Query(sb, ht).Rows[0]["total_record"]);
            return ReturnValue;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable GetGridData(int startRowIndex, int maximumRows, string SHIFT_CD,
                                string SHIFT_TIME_CD, string SHIFT_DESC, string VALID,
                                string START_DT, string END_DT, string DUTY_TIME,
                                string EAT_TIME, string REST_TIME, string WORK_SHIFT_ALLOWANCE_TYPE, string IS_IFLOW_SHOW,
                                string sortExpression)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            if (sortExpression.Contains("SHIFT_CD"))
                sortExpression = sortExpression.Replace("SHIFT_CD", "SHIFTH.SHIFT_CD");
            if (sortExpression.Contains("SHIFT_TIME_DESC"))
                sortExpression = sortExpression.Replace("SHIFT_TIME_DESC", "STCD.SUB_DESC");
            if (sortExpression.Contains("START_DT"))
                sortExpression = sortExpression.Replace("START_DT", "SHIFTH.START_DT");
            if (sortExpression.Contains("END_DT"))
                sortExpression = sortExpression.Replace("END_DT", "SHIFTH.END_DT");
            if (sortExpression.Contains("DUTY_STIME"))
                sortExpression = sortExpression.Replace("DUTY_STIME", "SHIFTH.DUTY_STIME");
            if (sortExpression.Contains("MealTime1S"))
                sortExpression = sortExpression.Replace("MealTime1S", "SHIFTD_B.DUTY_BEFORE_REST_STIME_1");
            if (sortExpression.Contains("MealTime1ResetS"))
                sortExpression = sortExpression.Replace("MealTime1ResetS", "SHIFTD_BR.DUTY_BEFORE_REST_STIME_1");
            if (sortExpression.Contains("MealTime2S"))
                sortExpression = sortExpression.Replace("MealTime2S", "SHIFTD_L.DUTY_BEFORE_REST_STIME_1");
            if (sortExpression.Contains("MealTime2ResetS1"))
                sortExpression = sortExpression.Replace("MealTime2ResetS1", "SHIFTD_DR1.DUTY_BEFORE_REST_STIME_1");
            if (sortExpression.Contains("MealTime2ResetS2"))
                sortExpression = sortExpression.Replace("MealTime2ResetS2", "SHIFTD_DR2.DUTY_BEFORE_REST_STIME_1");
            if (sortExpression.Contains("MealTime2ResetS3"))
                sortExpression = sortExpression.Replace("MealTime2ResetS3", "SHIFTD_DR3.DUTY_BEFORE_REST_STIME_1");
            if (sortExpression.Contains("MealTime3S"))
                sortExpression = sortExpression.Replace("MealTime3S", "SHIFTD_D.DUTY_BEFORE_REST_STIME_1");
            if (sortExpression.Contains("MealTime3ResetS1"))
                sortExpression = sortExpression.Replace("MealTime3ResetS1", "SHIFTD_AR1.DUTY_BEFORE_REST_STIME_1");
            if (sortExpression.Contains("MealTime3ResetS2"))
                sortExpression = sortExpression.Replace("MealTime3ResetS2", "SHIFTD_AR2.DUTY_BEFORE_REST_STIME_1");
            if (sortExpression.Contains("WORK_SHIFT_ALLOWANCE_TYPE_CD"))
                sortExpression = sortExpression.Replace("WORK_SHIFT_ALLOWANCE_TYPE_CD", "WSATD.SUB_CD");

            sb.AppendLine(" select * ");
            sb.AppendLine(" from (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,   ");    //SHIFTH.SHIFT_CD ASC
            sb.AppendLine("		         SHIFTH.SHIFT_CD, ");
            sb.AppendLine("		         SHIFTH.SHIFT_DESC, ");
            sb.AppendLine("		         STCD.SUB_CD SHIFT_TIME_CD, ");
            sb.AppendLine("		         STCD.SUB_DESC  SHIFT_TIME_DESC, ");
            sb.AppendLine("		         SHIFTH.DUTY_STIME, ");
            sb.AppendLine("		         SHIFTH.DUTY_ETIME, ");
            sb.AppendLine("		         IIF(SHIFTH.IS_IFLOW_SHOW='Y','是','否') as IS_IFLOW_SHOW, ");
            sb.AppendLine("		         IIF(SHIFTD_B.DUTY_BEFORE_REST_STIME_1 IS NULL,NULL,RIGHT(REPLICATE('0',4)+CAST(CAST(ISNULL(SHIFTD_B.DUTY_BEFORE_REST_STIME_1,'0')AS INT)%2400 AS VARCHAR),4)) MealTime1S, ");
            sb.AppendLine("		         IIF(SHIFTD_B.DUTY_BEFORE_REST_ETIME_1 IS NULL,NULL,RIGHT(REPLICATE('0',4)+CAST(CAST(ISNULL(SHIFTD_B.DUTY_BEFORE_REST_ETIME_1,'0')AS INT)%2400 AS VARCHAR),4))  MealTime1E, ");
            sb.AppendLine("		         IIF(SHIFTD_BR.DUTY_BEFORE_REST_STIME_1 IS NULL,NULL,RIGHT(REPLICATE('0',4)+CAST(CAST(ISNULL(SHIFTD_BR.DUTY_BEFORE_REST_STIME_1,'0')AS INT)%2400 AS VARCHAR),4)) MealTime1ResetS, ");
            sb.AppendLine("		         IIF(SHIFTD_BR.DUTY_BEFORE_REST_ETIME_1 IS NULL,NULL,RIGHT(REPLICATE('0',4)+CAST(CAST(ISNULL(SHIFTD_BR.DUTY_BEFORE_REST_ETIME_1,'0')AS INT)%2400 AS VARCHAR),4)) MealTime1ResetE,	 ");
            sb.AppendLine("		         IIF(SHIFTD_L.DUTY_BEFORE_REST_STIME_1 IS NULL,NULL,RIGHT(REPLICATE('0',4)+CAST(CAST(ISNULL(SHIFTD_L.DUTY_BEFORE_REST_STIME_1,'0')AS INT)%2400 AS VARCHAR),4)) MealTime2S, ");
            sb.AppendLine("		         IIF(SHIFTD_L.DUTY_BEFORE_REST_ETIME_1 IS NULL,NULL,RIGHT(REPLICATE('0',4)+CAST(CAST(ISNULL(SHIFTD_L.DUTY_BEFORE_REST_ETIME_1,'0')AS INT)%2400 AS VARCHAR),4)) MealTime2E, ");
            sb.AppendLine("		         IIF(SHIFTD_DR1.DUTY_BEFORE_REST_STIME_1 IS NULL,NULL,RIGHT(REPLICATE('0',4)+CAST(CAST(ISNULL(SHIFTD_DR1.DUTY_BEFORE_REST_STIME_1,'0')AS INT)%2400 AS VARCHAR),4)) MealTime2ResetS1, ");
            sb.AppendLine("		         IIF(SHIFTD_DR1.DUTY_BEFORE_REST_ETIME_1 IS NULL,NULL,RIGHT(REPLICATE('0',4)+CAST(CAST(ISNULL(SHIFTD_DR1.DUTY_BEFORE_REST_ETIME_1,'0')AS INT)%2400 AS VARCHAR),4)) MealTime2ResetE1, ");
            sb.AppendLine("		         IIF(SHIFTD_DR2.DUTY_BEFORE_REST_STIME_1 IS NULL,NULL,RIGHT(REPLICATE('0',4)+CAST(CAST(ISNULL(SHIFTD_DR2.DUTY_BEFORE_REST_STIME_1,'0')AS INT)%2400 AS VARCHAR),4)) MealTime2ResetS2, ");
            sb.AppendLine("		         IIF(SHIFTD_DR2.DUTY_BEFORE_REST_ETIME_1 IS NULL,NULL,RIGHT(REPLICATE('0',4)+CAST(CAST(ISNULL(SHIFTD_DR2.DUTY_BEFORE_REST_ETIME_1,'0')AS INT)%2400 AS VARCHAR),4)) MealTime2ResetE2, ");
            sb.AppendLine("		         IIF(SHIFTD_DR3.DUTY_BEFORE_REST_STIME_1 IS NULL,NULL,RIGHT(REPLICATE('0',4)+CAST(CAST(ISNULL(SHIFTD_DR3.DUTY_BEFORE_REST_STIME_1,'0')AS INT)%2400 AS VARCHAR),4)) MealTime2ResetS3, ");
            sb.AppendLine("		         IIF(SHIFTD_DR3.DUTY_BEFORE_REST_ETIME_1 IS NULL,NULL,RIGHT(REPLICATE('0',4)+CAST(CAST(ISNULL(SHIFTD_DR3.DUTY_BEFORE_REST_ETIME_1,'0')AS INT)%2400 AS VARCHAR),4)) MealTime2ResetE3, ");
            sb.AppendLine("		         IIF(SHIFTD_D.DUTY_BEFORE_REST_STIME_1 IS NULL,NULL,RIGHT(REPLICATE('0',4)+CAST(CAST(ISNULL(SHIFTD_D.DUTY_BEFORE_REST_STIME_1,'0')AS INT)%2400 AS VARCHAR),4)) MealTime3S, ");
            sb.AppendLine("		         IIF(SHIFTD_D.DUTY_BEFORE_REST_ETIME_1 IS NULL,NULL,RIGHT(REPLICATE('0',4)+CAST(CAST(ISNULL(SHIFTD_D.DUTY_BEFORE_REST_ETIME_1,'0')AS INT)%2400 AS VARCHAR),4)) MealTime3E, ");
            sb.AppendLine("		         IIF(SHIFTD_AR1.DUTY_BEFORE_REST_STIME_1 IS NULL,NULL,RIGHT(REPLICATE('0',4)+CAST(CAST(ISNULL(SHIFTD_AR1.DUTY_BEFORE_REST_STIME_1,'0')AS INT)%2400 AS VARCHAR),4)) MealTime3ResetS1, ");
            sb.AppendLine("		         IIF(SHIFTD_AR1.DUTY_BEFORE_REST_ETIME_1 IS NULL,NULL,RIGHT(REPLICATE('0',4)+CAST(CAST(ISNULL(SHIFTD_AR1.DUTY_BEFORE_REST_ETIME_1,'0')AS INT)%2400 AS VARCHAR),4)) MealTime3ResetE1, ");
            sb.AppendLine("		         IIF(SHIFTD_AR2.DUTY_BEFORE_REST_STIME_1 IS NULL,NULL,RIGHT(REPLICATE('0',4)+CAST(CAST(ISNULL(SHIFTD_AR2.DUTY_BEFORE_REST_STIME_1,'0')AS INT)%2400 AS VARCHAR),4)) MealTime3ResetS2, ");
            sb.AppendLine("		         IIF(SHIFTD_AR2.DUTY_BEFORE_REST_ETIME_1 IS NULL,NULL,RIGHT(REPLICATE('0',4)+CAST(CAST(ISNULL(SHIFTD_AR2.DUTY_BEFORE_REST_ETIME_1,'0')AS INT)%2400 AS VARCHAR),4)) MealTime3ResetE2, ");
            sb.AppendLine("		         WSATD.SUB_CD WORK_SHIFT_ALLOWANCE_TYPE_CD, ");
            sb.AppendLine("		         WSATD.SUB_DESC WORK_SHIFT_ALLOWANCE_TYPE_DESC, ");
            sb.AppendLine("		         SHIFTH.START_DT, ");
            sb.AppendLine("		         SHIFTH.END_DT	  ");
            sb.AppendLine("	      from TB_D_M_SHIFT_H SHIFTH ");
            sb.AppendLine("	      left join TB_9_M_COMM_H WSATH on WSATH.SYS_CD='SC' and WSATH.MAIN_CD='WORK_SHIFT_ALLOWANCE_TYPE' ");
            sb.AppendLine("	      left join TB_9_M_COMM_D WSATD on WSATD.MAIN_CD=WSATH.MAIN_CD and WSATD.SUB_CD=SHIFTH.WORK_SHIFT_ALLOWANCE_TYPE ");
            sb.AppendLine("	      left join TB_9_M_COMM_H STCH on STCH.SYS_CD='DA' and STCH.MAIN_CD='SHIFT_TIME_CD' ");
            sb.AppendLine("       left join TB_9_M_COMM_D STCD on STCD.MAIN_CD=STCH.MAIN_CD and STCD.SUB_CD=SHIFTH.SHIFT_TIME_CD ");
            sb.AppendLine("	      left join TB_D_M_SHIFT_D SHIFTD_B on SHIFTD_B.SHIFT_CD=SHIFTH.SHIFT_CD  and SHIFTD_B.START_DT=SHIFTH.START_DT and SHIFTD_B.TIME_CD='BB1'  ");
            sb.AppendLine("	      left join TB_D_M_SHIFT_D SHIFTD_BR on SHIFTD_BR.SHIFT_CD=SHIFTH.SHIFT_CD  and SHIFTD_BR.START_DT=SHIFTH.START_DT and SHIFTD_BR.TIME_CD='BR1' ");
            sb.AppendLine("	      left join TB_D_M_SHIFT_D SHIFTD_L on SHIFTD_L.SHIFT_CD=SHIFTH.SHIFT_CD and SHIFTD_L.START_DT=SHIFTH.START_DT  and SHIFTD_L.TIME_CD='DL1'  ");
            sb.AppendLine("	      left join TB_D_M_SHIFT_D SHIFTD_D on SHIFTD_D.SHIFT_CD=SHIFTH.SHIFT_CD and SHIFTD_D.START_DT=SHIFTH.START_DT  and SHIFTD_D.TIME_CD='AD1'  ");
            sb.AppendLine("	      left join TB_D_M_SHIFT_D SHIFTD_DR1 on SHIFTD_DR1.SHIFT_CD=SHIFTH.SHIFT_CD and SHIFTD_DR1.START_DT=SHIFTH.START_DT and SHIFTD_DR1.TIME_CD='DR1'  ");
            sb.AppendLine("	      left join TB_D_M_SHIFT_D SHIFTD_DR2 on SHIFTD_DR2.SHIFT_CD=SHIFTH.SHIFT_CD and SHIFTD_DR2.START_DT=SHIFTH.START_DT  and SHIFTD_DR2.TIME_CD='DR2'  ");
            sb.AppendLine("	      left join TB_D_M_SHIFT_D SHIFTD_DR3 on SHIFTD_DR3.SHIFT_CD=SHIFTH.SHIFT_CD and SHIFTD_DR3.START_DT=SHIFTH.START_DT and SHIFTD_DR3.TIME_CD='DR3'  ");
            sb.AppendLine("	      left join TB_D_M_SHIFT_D SHIFTD_AR1 on SHIFTD_AR1.SHIFT_CD=SHIFTH.SHIFT_CD and SHIFTD_AR1.START_DT=SHIFTH.START_DT and SHIFTD_AR1.TIME_CD='AR1'  ");
            sb.AppendLine("	      left join TB_D_M_SHIFT_D SHIFTD_AR2 on SHIFTD_AR2.SHIFT_CD=SHIFTH.SHIFT_CD and SHIFTD_AR2.START_DT=SHIFTH.START_DT and SHIFTD_AR2.TIME_CD='AR2'  ");
            sb.AppendLine("       where 1=1 ");

            //若有輸入時段別區別，A.時段別區分 = '畫面.時段別區分'	
            if (string.IsNullOrEmpty(SHIFT_TIME_CD.ToUpper()) == false)
            {
                sb.AppendLine("            and SHIFTH.SHIFT_TIME_CD=@SHIFT_TIME_CD ");
                ht.Add("@SHIFT_TIME_CD", SHIFT_TIME_CD.ToUpper());
            }

            //若有輸入班別代碼，A.班別代碼 LIKE '畫面.班別代碼%'																																																							
            if (string.IsNullOrEmpty(SHIFT_CD.ToUpper()) == false)
            {

                sb.AppendLine("            and SHIFTH.SHIFT_CD  like @SHIFT_CD+'%' ");
                ht.Add("@SHIFT_CD", SHIFT_CD.ToUpper());
            }

            //若有輸入班別說明，A.班別說明 LIKE '%畫面.班別說明%'	
            if (string.IsNullOrEmpty(SHIFT_DESC) == false)
            {
                sb.AppendLine("            and SHIFTH.SHIFT_DESC like '%'+@SHIFT_DESC+'%' ");
                ht.Add("@SHIFT_DESC", SHIFT_DESC);
            }

            //若有輸入生效起日，A.結束日期>=畫面.生效起日
            if (string.IsNullOrEmpty(START_DT) == false)
            {
                sb.AppendLine("            and SHIFTH.END_DT>=@START_DT  ");
                ht.Add("@START_DT", START_DT);
            }

            //若有輸入生效迄日，A.生效日期<=畫面.生效迄日
            //若有輸入生效迄日，且選取"有效"，A.結束日期>=畫面.生效迄日
            //若有輸入生效迄日，且選取"無效"，A.結束日期<畫面.生效迄日
            if (string.IsNullOrEmpty(END_DT) == false)
            {
                if (VALID == "N")
                    sb.AppendLine("            and SHIFTH.END_DT<@END_DT ");
                else if (VALID == "Y")
                    sb.AppendLine("            and SHIFTH.END_DT>=@END_DT ");
                else
                    sb.AppendLine("            and SHIFTH.START_DT<=@END_DT ");

                ht.Add("@END_DT", END_DT);
            }

            //若有輸入上班時間，畫面.上班時間 >= A.勤務上班時間 且 畫面.上班時間 <= A.勤務下班時間 	
            if (string.IsNullOrEmpty(DUTY_TIME) == false)
            {
                sb.AppendLine("            and SHIFTH.DUTY_STIME<=@DUTY_TIME ");
                sb.AppendLine("            and SHIFTH.DUTY_ETIME>=@DUTY_TIME ");
                ht.Add("@DUTY_TIME", DUTY_TIME);
            }

            //"若有輸入用餐時間，班別代碼 in(
            //"																														
            //                                        select 班別代碼 from 班別明細檔																				
            //                                           where substring(時段區分,2,1) in('B','L','D')																				
            //                                             and 結束時間 >=  畫面上.用餐時間																				
            //                                             and 開始時間 <=  畫面上.用餐時間)		
            if (string.IsNullOrEmpty(EAT_TIME) == false)
            {
                sb.AppendLine("            and SHIFTH.SHIFT_CD in (select SHIFT_CD ");
                sb.AppendLine("                                    from TB_D_M_SHIFT_D TDMSD");
                sb.AppendLine("                                    where TDMSD.FUNC_ID='FB2DA020' ");
                sb.AppendLine("                                      and substring(TDMSD.TIME_CD,2,1) in ('B','L','D') ");
                sb.AppendLine("                                      and TDMSD.DUTY_BEFORE_REST_STIME_1<=@EAT_TIME ");
                sb.AppendLine("                                      and TDMSD.DUTY_BEFORE_REST_ETIME_1>=@EAT_TIME) ");
                ht.Add("@EAT_TIME", EAT_TIME);
            }

            //"若有輸入休息時間，班別代碼 in(
            //"																														
            //                                        select 班別代碼 from 班別明細檔																				
            //                                           where substring(時段區分,2,1) in('R')																				
            //                                             and 結束時間 >=  畫面上.用餐時間起																				
            //                                             and 開始時間 <=  畫面上.用餐時間迄)		
            if (string.IsNullOrEmpty(REST_TIME) == false)
            {
                sb.AppendLine("            and SHIFTH.SHIFT_CD in (select SHIFT_CD ");
                sb.AppendLine("                                    from TB_D_M_SHIFT_D TDMSD ");
                sb.AppendLine("                                    where TDMSD.FUNC_ID='FB2DA020' ");
                sb.AppendLine("                                      and substring(TDMSD.TIME_CD,2,1) in ('R') ");
                sb.AppendLine("                                      and TDMSD.DUTY_BEFORE_REST_STIME_1<=@REST_TIME ");
                sb.AppendLine("                                      and TDMSD.DUTY_BEFORE_REST_ETIME_1>=@REST_TIME) ");
                ht.Add("@REST_TIME", REST_TIME);
            }

            //若有輸入津貼，A.津貼=畫面.津貼
            if (string.IsNullOrEmpty(WORK_SHIFT_ALLOWANCE_TYPE) == false)
            {
                sb.AppendLine("            and SHIFTH.WORK_SHIFT_ALLOWANCE_TYPE=@WORK_SHIFT_ALLOWANCE_TYPE ");
                ht.Add("@WORK_SHIFT_ALLOWANCE_TYPE", WORK_SHIFT_ALLOWANCE_TYPE);
            }

            //IFLOW顯示否
            if (string.IsNullOrEmpty(IS_IFLOW_SHOW) == false)
            {
                sb.AppendLine("            and SHIFTH.IS_IFLOW_SHOW=@IS_IFLOW_SHOW ");
                ht.Add("@IS_IFLOW_SHOW", IS_IFLOW_SHOW);
            }

            sb.AppendLine(" ) TDMCH where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.AppendLine("           AND CAST(@startRowIndex+@maximumRows as varchar)");
            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            DataTable returnDt = dbConn.Query(sb, ht);

            return returnDt;
        }
        catch (Exception)
        {

            throw;
        }
    }


    public int CheckTB_D_M_SHIFT_H_DataByKey(WFB2DA0200DAO dao, bool isOnTran)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select COUNT(1) total_record   ");
            sb.AppendLine(" from TB_D_M_SHIFT_H SHIFTH ");
            sb.AppendLine(" where SHIFTH.FUNC_ID=@FUNC_ID ");
            sb.AppendLine("  and SHIFTH.SHIFT_CD = @SHIFT_CD ");
            sb.AppendLine("  and SHIFTH.END_DT >= @END_DT ");
            sb.AppendLine("  and SHIFTH.START_DT <= @START_DT ");

            ht.Add("@FUNC_ID", dao.FUNC_ID);
            ht.Add("@SHIFT_CD", dao.SHIFT_CD.ToUpper());
            ht.Add("@END_DT", dao.END_DT == null ? null : Convert.ToDateTime(dao.END_DT).ToString("yyyy-MM-dd"));
            ht.Add("@START_DT", dao.START_DT.ToString("yyyy-MM-dd"));

            if (isOnTran)
                return Convert.ToInt32(dbConn.QueryT(sb, ht).Rows[0]["total_record"]);
            else
                return Convert.ToInt32(dbConn.Query(sb, ht).Rows[0]["total_record"]);
        }
        catch (Exception)
        {

            throw;
        }

    }

    public int CheckTB_D_M_EMP_DAY_DUTY(WFB2DA0200DAO dao, bool isOnTran)
    {
        try
        {
            int CheckDtl = 0;
            if (dao.Dtl != null)
            {
                foreach (WFB2DA0200DtlDAO dtl in dao.Dtl)
                {
                    StringBuilder sb = new StringBuilder();
                    Hashtable ht = new Hashtable();

                    sb.AppendLine(" select count(1) total_record from TB_D_M_EMP_DAY_DUTY A ");
                    sb.AppendLine(" where A.SHIFT_CD = @SHIFT_CD ");
                    sb.AppendLine("   and A.CALENDAR_DT >= @DUTY_BEFORE_REST_STIME_1 ");
                    sb.AppendLine("   and A.CALENDAR_DT <= @DUTY_BEFORE_REST_ETIME_1 ");

                    ht.Add("@SHIFT_CD", dtl.SHIFT_CD.ToUpper());
                    ht.Add("@DUTY_BEFORE_REST_STIME_1", dao.START_DT.ToString("yyyy-MM-dd"));
                    ht.Add("@DUTY_BEFORE_REST_ETIME_1", dao.END_DT == null ? null : Convert.ToDateTime(dao.END_DT).ToString("yyyy-MM-dd"));

                    if (isOnTran)
                        CheckDtl += Convert.ToInt32(dbConn.QueryT(sb, ht).Rows[0]["total_record"]);
                    else
                        CheckDtl += Convert.ToInt32(dbConn.Query(sb, ht).Rows[0]["total_record"]);
                    if (CheckDtl > 0)
                        return CheckDtl;
                }
            }
            return CheckDtl;
        }
        catch (Exception)
        {

            throw;
        }
    }
    //新增班別主檔
    public void InsertTB_D_M_SHIFT_H(WFB2DA0200DAO dao, bool isOnTran)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" INSERT INTO TB_D_M_SHIFT_H ");
            sb.AppendLine("            ([SHIFT_CD] ");
            sb.AppendLine("            ,[START_DT] ");
            sb.AppendLine("            ,[END_DT] ");
            sb.AppendLine("            ,[SHIFT_DESC] ");
            sb.AppendLine("            ,[SHIFT_TIME_CD] ");
            sb.AppendLine("            ,[WORK_HOUR] ");
            sb.AppendLine("            ,[WORK_PERIOD_HOUR] ");
            sb.AppendLine("            ,[DUTY_STIME] ");
            sb.AppendLine("            ,[DUTY_ETIME] ");
            sb.AppendLine("            ,[WORK_SHIFT_ALLOWANCE_TYPE] ");
            sb.AppendLine("            ,[R_SHIFT_CD]");
            sb.AppendLine("            ,[IS_IFLOW_SHOW] ");
            sb.AppendLine("            ,[REMARK] ");
            sb.AppendLine("            ,[CREATED_BY] ");
            sb.AppendLine("            ,[CREATED_DT] ");
            sb.AppendLine("            ,[UPDATED_BY] ");
            sb.AppendLine("            ,[UPDATED_DT] ");
            sb.AppendLine("            ,[FUNC_ID]) ");
            sb.AppendLine("      VALUES ");
            sb.AppendLine("            (@SHIFT_CD ");
            sb.AppendLine("            ,@START_DT ");
            sb.AppendLine("            ,@END_DT ");
            sb.AppendLine("            ,@SHIFT_DESC ");
            sb.AppendLine("            ,@SHIFT_TIME_CD ");
            sb.AppendLine("            ,@WORK_HOUR ");
            sb.AppendLine("            ,@WORK_PERIOD_HOUR ");
            sb.AppendLine("            ,@DUTY_STIME ");
            sb.AppendLine("            ,@DUTY_ETIME ");
            sb.AppendLine("            ,@WORK_SHIFT_ALLOWANCE_TYPE ");
            sb.AppendLine("            ,@R_SHIFT_CD ");
            sb.AppendLine("            ,@IS_IFLOW_SHOW ");
            sb.AppendLine("            ,@REMARK ");
            sb.AppendLine("            ,@CREATED_BY ");
            sb.AppendLine("            ,@CREATED_DT ");
            sb.AppendLine("            ,@UPDATED_BY ");
            sb.AppendLine("            ,@UPDATED_DT ");
            sb.AppendLine("            ,@FUNC_ID) ");

            ht.Add("@SHIFT_CD", dao.SHIFT_CD.ToUpper());
            ht.Add("@START_DT", dao.START_DT.ToString("yyyy-MM-dd"));
            ht.Add("@END_DT", dao.END_DT == null ? null : Convert.ToDateTime(dao.END_DT).ToString("yyyy-MM-dd"));
            ht.Add("@SHIFT_DESC", dao.SHIFT_DESC);
            ht.Add("@SHIFT_TIME_CD", dao.SHIFT_TIME_CD.ToUpper());
            ht.Add("@WORK_HOUR", dao.WORK_HOUR);
            ht.Add("@WORK_PERIOD_HOUR", dao.WORK_PERIOD_HOUR);
            ht.Add("@DUTY_STIME", dao.DUTY_STIME);
            ht.Add("@DUTY_ETIME", dao.DUTY_ETIME);
            ht.Add("@WORK_SHIFT_ALLOWANCE_TYPE", dao.WORK_SHIFT_ALLOWANCE_TYPE);
            ht.Add("@R_SHIFT_CD", dao.R_SHIFT_CD);
            ht.Add("@IS_IFLOW_SHOW", dao.IS_IFLOW_SHOW);
            ht.Add("@REMARK", dao.REMARK);
            ht.Add("@CREATED_BY", dao.CREATED_BY);
            ht.Add("@CREATED_DT", dao.CREATED_DT.ToString("yyyy-MM-dd"));
            ht.Add("@UPDATED_BY", dao.UPDATED_BY);
            ht.Add("@UPDATED_DT", dao.UPDATED_DT);
            ht.Add("@FUNC_ID", dao.FUNC_ID);


            if (isOnTran)
                dbConn.ExecuteT(sb, ht);
            else
                dbConn.Execute(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public void UpdateTB_D_M_SHIFT_H_END_DT(WFB2DA0200DAO dao, bool isOnTran)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" UPDATE TB_D_M_SHIFT_H ");
            sb.AppendLine("    SET [END_DT] = @END_DT ");
            sb.AppendLine("       ,[IS_IFLOW_SHOW] = @IS_IFLOW_SHOW ");
            sb.AppendLine("       ,[UPDATED_BY] = @UPDATED_BY ");
            sb.AppendLine("       ,[UPDATED_DT] = @UPDATED_DT");
            sb.AppendLine("  WHERE 1=1 ");
            sb.AppendLine("    and [SHIFT_CD] = @SHIFT_CD ");
            sb.AppendLine("    and START_DT in (select MAX(A.START_DT) START_DT  ");
            sb.AppendLine("                      from TB_D_M_SHIFT_H A ");
            sb.AppendLine("                      where A.SHIFT_CD = @SHIFT_CD ");
            sb.AppendLine("                      group by A.SHIFT_CD ) ");

            ht.Add("@SHIFT_CD", dao.SHIFT_CD.ToUpper());
            ht.Add("@END_DT", dao.START_DT.AddDays(-1).ToString("yyyy-MM-dd"));
            ht.Add("@IS_IFLOW_SHOW", dao.IS_IFLOW_SHOW);
            ht.Add("@UPDATED_BY", dao.UPDATED_BY);
            ht.Add("@UPDATED_DT", dao.UPDATED_DT);
            ht.Add("@FUNC_ID", dao.FUNC_ID);


            if (isOnTran)
                dbConn.ExecuteT(sb, ht);
            else
                dbConn.Execute(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public void UpdateTB_D_M_SHIFT_H_END_DT_BY_RSHIFT(WFB2DA0200DAO dao, bool isOnTran)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" UPDATE TB_D_M_SHIFT_H ");
            sb.AppendLine("    SET [END_DT] = @END_DT ");
            sb.AppendLine("       ,[UPDATED_BY] = @UPDATED_BY ");
            sb.AppendLine("       ,[UPDATED_DT] = @UPDATED_DT");
            sb.AppendLine("  WHERE 1=1 ");
            sb.AppendLine("    and [SHIFT_CD] = @R_SHIFT_CD ");
            sb.AppendLine("    and START_DT in (select MAX(A.START_DT) START_DT  ");
            sb.AppendLine("                      from TB_D_M_SHIFT_H A ");
            sb.AppendLine("                      where A.SHIFT_CD = @R_SHIFT_CD ");
            sb.AppendLine("                      group by A.SHIFT_CD ) ");

            ht.Add("@R_SHIFT_CD", dao.R_SHIFT_CD.ToUpper());
            ht.Add("@END_DT", dao.START_DT.AddDays(-1).ToString("yyyy-MM-dd"));
            ht.Add("@UPDATED_BY", dao.UPDATED_BY);
            ht.Add("@UPDATED_DT", dao.UPDATED_DT);
            ht.Add("@FUNC_ID", dao.FUNC_ID);


            if (isOnTran)
                dbConn.ExecuteT(sb, ht);
            else
                dbConn.Execute(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
    //修改 輪值表的班別代碼
    public void Update_TB_D_M_WORK_SHIFT_D(WFB2DA0200DAO dao)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" UPDATE TB_D_M_WORK_SHIFT_D ");
            sb.AppendLine("    SET [SHIFT_CD] = @SHIFT_CD ");            
            sb.AppendLine("       ,[UPDATED_BY] = @UPDATED_BY ");
            sb.AppendLine("       ,[UPDATED_DT] = @UPDATED_DT");
            sb.AppendLine("       ,[FUNC_ID] = @FUNC_ID");
            sb.AppendLine("  WHERE CALENDAR_DT >= @START_DT ");
            sb.AppendLine("    and SHIFT_CD = @R_SHIFT_CD  ");

            ht.Add("@SHIFT_CD", dao.SHIFT_CD.ToUpper());
            ht.Add("@R_SHIFT_CD", dao.R_SHIFT_CD.ToUpper());
            ht.Add("@START_DT", dao.START_DT);
            ht.Add("@IS_IFLOW_SHOW", dao.IS_IFLOW_SHOW);
            ht.Add("@UPDATED_BY", dao.UPDATED_BY);
            ht.Add("@UPDATED_DT", dao.UPDATED_DT);
            ht.Add("@FUNC_ID", dao.FUNC_ID);

            dbConn.ExecuteT(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //修改 TB_D_M_EMP_DAY_DUTY 日勤務班表資料檔
    public void Update_TB_D_M_EMP_DAY_DUTY(WFB2DA0200DAO dao)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" UPDATE TB_D_M_EMP_DAY_DUTY ");
            sb.AppendLine("    SET [SHIFT_TIME_CD] = @SHIFT_TIME_CD ");
            sb.AppendLine("       ,[WORK_HOUR] = @WORK_HOUR ");
            sb.AppendLine("       ,[DUTY_STIME] = dbo.FN_D_GEN_DATETIME(CALENDAR_DT,@DUTY_STIME ) ");
            sb.AppendLine("       ,[DUTY_ETIME] =  iif(    @DUTY_ETIME > @DUTY_STIME , dbo.FN_D_GEN_DATETIME(CALENDAR_DT,@DUTY_ETIME), dbo.FN_D_GEN_DATETIME(CALENDAR_DT +1  ,@DUTY_ETIME)  ) ");
            sb.AppendLine("       ,WORK_SHIFT_ALLOWANCE_TYPE = @WORK_SHIFT_ALLOWANCE_TYPE ");
            sb.AppendLine("       ,[SHIFT_CD] = @SHIFT_CD ");
            sb.AppendLine("       ,[UPDATED_BY] = @UPDATED_BY ");
            sb.AppendLine("       ,[UPDATED_DT] = @UPDATED_DT");
            sb.AppendLine("       ,[FUNC_ID] = @FUNC_ID");
            sb.AppendLine("  WHERE CALENDAR_DT >= @START_DT ");
            sb.AppendLine("    and CALENDAR_DT <= @END_DT  ");
            sb.AppendLine("    and SHIFT_CD = @R_SHIFT_CD  ");

            ht.Add("@SHIFT_TIME_CD", dao.SHIFT_TIME_CD.ToUpper());
            ht.Add("@WORK_HOUR", dao.WORK_HOUR);
            ht.Add("@DUTY_STIME", dao.DUTY_STIME);
            ht.Add("@DUTY_ETIME", dao.DUTY_ETIME);
            ht.Add("@WORK_SHIFT_ALLOWANCE_TYPE", dao.WORK_SHIFT_ALLOWANCE_TYPE);
            ht.Add("@SHIFT_CD", dao.SHIFT_CD.ToUpper());
            ht.Add("@R_SHIFT_CD", dao.R_SHIFT_CD.ToUpper());
            ht.Add("@START_DT", dao.START_DT);
            ht.Add("@END_DT", dao.END_DT);
            ht.Add("@UPDATED_BY", dao.UPDATED_BY);
            ht.Add("@UPDATED_DT", dao.UPDATED_DT);
            ht.Add("@FUNC_ID", dao.FUNC_ID);

            dbConn.ExecuteT(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //日勤務狀態檔reopen
    public void Update_TB_D_M_EMP_DUTY_CHECK_STATUS(WFB2DA0200DAO dao)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" UPDATE TB_D_M_EMP_DUTY_CHECK_STATUS ");
            sb.AppendLine("    SET  DUTY_CHECK_RESULT = 'N' ");            
            sb.AppendLine("       ,[UPDATED_BY] = @UPDATED_BY ");
            sb.AppendLine("       ,[UPDATED_DT] = @UPDATED_DT");
            sb.AppendLine("       ,[FUNC_ID] = @FUNC_ID");
            sb.AppendLine("  WHERE CALENDAR_DT >= @START_DT ");
            sb.AppendLine("    and CALENDAR_DT < @END_DT  ");
            sb.AppendLine("    and SHIFT_CD = @R_SHIFT_CD  ");
            
            ht.Add("@R_SHIFT_CD", dao.R_SHIFT_CD.ToUpper());
            ht.Add("@START_DT", dao.START_DT);
            ht.Add("@END_DT", dao.END_DT);
            ht.Add("@UPDATED_BY", dao.UPDATED_BY);
            ht.Add("@UPDATED_DT", dao.UPDATED_DT);
            ht.Add("@FUNC_ID", dao.FUNC_ID);

            dbConn.ExecuteT(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //修改班別主檔
    public void UpdateTB_D_M_SHIFT_H(WFB2DA0200DAO dao, bool isOnTran)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" UPDATE TB_D_M_SHIFT_H ");
            sb.AppendLine("    SET [END_DT] = @END_DT ");
            sb.AppendLine("       ,[SHIFT_DESC] = @SHIFT_DESC ");
            sb.AppendLine("       ,[SHIFT_TIME_CD] = @SHIFT_TIME_CD ");
            sb.AppendLine("       ,[WORK_HOUR] = @WORK_HOUR ");
            sb.AppendLine("       ,[WORK_PERIOD_HOUR] = @WORK_PERIOD_HOUR ");
            sb.AppendLine("       ,[DUTY_STIME] = @DUTY_STIME ");
            sb.AppendLine("       ,[DUTY_ETIME] = @DUTY_ETIME ");
            sb.AppendLine("       ,[WORK_SHIFT_ALLOWANCE_TYPE] = @WORK_SHIFT_ALLOWANCE_TYPE ");
            sb.AppendLine("       ,[IS_IFLOW_SHOW] = @IS_IFLOW_SHOW ");
            sb.AppendLine("       ,[REMARK] = @REMARK ");
            sb.AppendLine("       ,[UPDATED_BY] = @UPDATED_BY ");
            sb.AppendLine("       ,[UPDATED_DT] = @UPDATED_DT");
            sb.AppendLine("       ,[FUNC_ID] = @FUNC_ID ");
            sb.AppendLine("    WHERE [SHIFT_CD] = @SHIFT_CD ");
            sb.AppendLine("    and START_DT = @START_DT");

            ht.Add("@SHIFT_CD", dao.SHIFT_CD.ToUpper());
            ht.Add("@START_DT", dao.START_DT.ToString("yyyy-MM-dd"));
            ht.Add("@END_DT", dao.END_DT == null ? null : Convert.ToDateTime(dao.END_DT).ToString("yyyy-MM-dd"));
            ht.Add("@SHIFT_DESC", dao.SHIFT_DESC);
            ht.Add("@SHIFT_TIME_CD", dao.SHIFT_TIME_CD.ToUpper());
            ht.Add("@WORK_HOUR", dao.WORK_HOUR);
            ht.Add("@WORK_PERIOD_HOUR", dao.WORK_PERIOD_HOUR);
            ht.Add("@DUTY_STIME", dao.DUTY_STIME);
            ht.Add("@DUTY_ETIME", dao.DUTY_ETIME);
            ht.Add("@WORK_SHIFT_ALLOWANCE_TYPE", dao.WORK_SHIFT_ALLOWANCE_TYPE);
            ht.Add("@IS_IFLOW_SHOW", dao.IS_IFLOW_SHOW);
            ht.Add("@REMARK", dao.REMARK);
            ht.Add("@UPDATED_BY", dao.UPDATED_BY);
            ht.Add("@UPDATED_DT", dao.UPDATED_DT);
            ht.Add("@FUNC_ID", dao.FUNC_ID);


            if (isOnTran)
            {
                dbConn.ExecuteT(sb, ht);
            }
            else
            {
                dbConn.Execute(sb, ht);
            }
        }
        catch (Exception)
        {

            throw;
        }
    }

    public bool UpdateTB_D_M_SHIFT_HByUnValid(WFB2DA0200DAO dao, bool isOnTran)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" UPDATE TB_D_M_SHIFT_H ");
            sb.AppendLine("    SET [END_DT] = @END_DT ");
            sb.AppendLine("       ,[UPDATED_BY] = @UPDATED_BY ");
            sb.AppendLine("       ,[UPDATED_DT] = @UPDATED_DT");
            sb.AppendLine("       ,[FUNC_ID] = @FUNC_ID");
            sb.AppendLine("  WHERE [SHIFT_CD] = @SHIFT_CD ");
            sb.AppendLine("    and START_DT =@START_DT");

            ht.Add("@SHIFT_CD", dao.SHIFT_CD.ToUpper());
            ht.Add("@START_DT", dao.START_DT.ToString("yyyy-MM-dd"));
            ht.Add("@END_DT", dao.END_DT == null ? null : Convert.ToDateTime(dao.END_DT).ToString("yyyy-MM-dd"));
            ht.Add("@UPDATED_BY", dao.UPDATED_BY);
            ht.Add("@UPDATED_DT", dao.UPDATED_DT);
            ht.Add("@FUNC_ID", dao.FUNC_ID);


            if (isOnTran)
            {
                dbConn.ExecuteT(sb, ht);
            }
            else
            {
                dbConn.Execute(sb, ht);
            }
            return true;
        }
        catch (Exception)
        {

            throw;
        }
    }

    //新增班別明細檔
    public void InsertTB_D_M_SHIFT_D(WFB2DA0200DtlDAO dao, bool isOnTran)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" INSERT INTO [TB_D_M_SHIFT_D] ");
            sb.AppendLine("            ([SHIFT_CD] ");
            sb.AppendLine("            ,[START_DT] ");
            sb.AppendLine("            ,[TIME_CD] ");
            sb.AppendLine("            ,[DUTY_BEFORE_REST_STIME_1] ");
            sb.AppendLine("            ,[DUTY_BEFORE_REST_ETIME_1] ");
            sb.AppendLine("            ,[CREATED_BY] ");
            sb.AppendLine("            ,[CREATED_DT] ");
            sb.AppendLine("            ,[UPDATED_BY] ");
            sb.AppendLine("            ,[UPDATED_DT] ");
            sb.AppendLine("            ,[FUNC_ID]) ");
            sb.AppendLine("      VALUES ");
            sb.AppendLine("            (@SHIFT_CD ");
            sb.AppendLine("            ,@START_DT ");
            sb.AppendLine("            ,@TIME_CD ");
            sb.AppendLine("            ,@DUTY_BEFORE_REST_STIME_1 ");
            sb.AppendLine("            ,@DUTY_BEFORE_REST_ETIME_1 ");
            sb.AppendLine("            ,@CREATED_BY ");
            sb.AppendLine("            ,@CREATED_DT ");
            sb.AppendLine("            ,@UPDATED_BY ");
            sb.AppendLine("            ,@UPDATED_DT ");
            sb.AppendLine("            ,@FUNC_ID) ");

            ht.Add("@SHIFT_CD", dao.SHIFT_CD.ToUpper());
            ht.Add("@START_DT", dao.START_DT.ToString("yyyy-MM-dd"));
            ht.Add("@TIME_CD", dao.TIME_CD.ToUpper());
            ht.Add("@DUTY_BEFORE_REST_STIME_1", dao.DUTY_BEFORE_REST_STIME_1);
            ht.Add("@DUTY_BEFORE_REST_ETIME_1", dao.DUTY_BEFORE_REST_ETIME_1);
            ht.Add("@CREATED_BY", dao.CREATED_BY);
            ht.Add("@CREATED_DT", dao.CREATED_DT);
            ht.Add("@UPDATED_BY", dao.UPDATED_BY);
            ht.Add("@UPDATED_DT", dao.UPDATED_DT);
            ht.Add("@FUNC_ID", dao.FUNC_ID);

            if (isOnTran)
                dbConn.ExecuteT(sb, ht);
            else
                dbConn.Execute(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public void UpdateTB_D_M_SHIFT_D(WFB2DA0200DAO dao, bool isOnTran)
    {
        try
        {

            WFB2DA0200DAO oldData = GetSinglSHIFT_Data(dao, true);

            foreach (WFB2DA0200DtlDAO dtl in dao.Dtl)
            {
                if (oldData.Dtl.Where(p => p.SHIFT_CD.ToUpper() == dtl.SHIFT_CD.ToUpper() && p.START_DT == dtl.START_DT && p.TIME_CD.ToUpper() == dtl.TIME_CD.ToUpper()).ToList().Count() == 0)
                    InsertTB_D_M_SHIFT_D(dtl, true);
                else
                {
                    StringBuilder sb = new StringBuilder();
                    Hashtable ht = new Hashtable();

                    sb.AppendLine(" UPDATE [TB_D_M_SHIFT_D] ");
                    sb.AppendLine("    SET [DUTY_BEFORE_REST_STIME_1] = @DUTY_BEFORE_REST_STIME_1 ");
                    sb.AppendLine("       ,[DUTY_BEFORE_REST_ETIME_1] = @DUTY_BEFORE_REST_ETIME_1 ");
                    sb.AppendLine("       ,[UPDATED_BY] = @UPDATED_BY ");
                    sb.AppendLine("       ,[UPDATED_DT] = @UPDATED_DT ");
                    sb.AppendLine("       ,[FUNC_ID] = @FUNC_ID ");
                    sb.AppendLine("  WHERE [SHIFT_CD] = @SHIFT_CD  ");
                    sb.AppendLine("    and [START_DT] = @START_DT  ");
                    sb.AppendLine("    and [TIME_CD] = @TIME_CD  ");

                    ht.Add("@SHIFT_CD", dao.SHIFT_CD.ToUpper());
                    ht.Add("@START_DT", dao.START_DT.ToString("yyyy-MM-dd"));
                    ht.Add("@TIME_CD", dtl.TIME_CD.ToUpper());
                    ht.Add("@DUTY_BEFORE_REST_STIME_1", dtl.DUTY_BEFORE_REST_STIME_1);
                    ht.Add("@DUTY_BEFORE_REST_ETIME_1", dtl.DUTY_BEFORE_REST_ETIME_1);
                    ht.Add("@UPDATED_BY", dtl.UPDATED_BY);
                    ht.Add("@UPDATED_DT", dtl.UPDATED_DT);
                    ht.Add("@FUNC_ID", dtl.FUNC_ID);

                    if (isOnTran)
                        dbConn.ExecuteT(sb, ht);
                    else
                        dbConn.Execute(sb, ht);
                }
            }
        }
        catch (Exception)
        {

            throw;
        }
    }

    //更新  日勤務狀態資料檔-日勤務刷卡比對狀態為N
    public void UpdateDUTY_CHECK_RESULT(WFB2DA0200DAO dao, bool isOnTran)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(@" UPDATE TB_D_M_EMP_DUTY_CHECK_STATUS 
                            SET 
                             DUTY_CHECK_RESULT = @DUTY_CHECK_RESULT 
                            ,UPDATED_BY = @UPDATED_BY 
                            ,UPDATED_DT = @UPDATED_DT
                            ,FUNC_ID = @FUNC_ID
                           where   1=1 
                            and CALENDAR_DT>= @START_DT 
                            and CALENDAR_DT > dbo.FN_D_DUTY_CLOSE_DT(-1)
                            and CALENDAR_DT<= @END_DT 
                            and CALENDAR_DT<=  ( select  CODE_VAL1 from dbo.TB_9_M_PARAMETER where SYS_CD = 'DC' and MAIN_CD = 'COMPARE_DT' )  
                            and SHIFT_CD= @SHIFT_CD 
                          ");
            ht.Add("@DUTY_CHECK_RESULT", 'N');
            ht.Add("@UPDATED_BY", dao.UPDATED_BY);
            ht.Add("@UPDATED_DT", dao.UPDATED_DT);
            ht.Add("@FUNC_ID", dao.FUNC_ID);

            ht.Add("@START_DT", dao.START_DT);
            ht.Add("@END_DT", dao.END_DT);
            ht.Add("@SHIFT_CD", dao.SHIFT_CD.ToUpper());
            if (isOnTran)
                dbConn.ExecuteT(sb, ht);
            else
                dbConn.Execute(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }



    public List<WFB2DA0200EMP_DAY_DUTY_DAO> GetTB_D_M_EMP_DAY_DUTY(WFB2DA0200DAO dao, bool isOnTran)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" select EMP_ID, ");
            sb.AppendLine("        CALENDAR_DT ");
            sb.AppendLine(" from TB_D_M_EMP_DAY_DUTY ");
            sb.AppendLine(" where SHIFT_CD = @SHIFT_CD ");
            sb.AppendLine("   and CALENDAR_DT >= @CALENDAR_DT ");

            ht.Add("@SHIFT_CD", dao.SHIFT_CD.ToUpper());
            ht.Add("@CALENDAR_DT", dao.START_DT.ToString("yyyy-MM-dd"));

            if (isOnTran)
                return (from item in dbConn.QueryT(sb, ht).AsEnumerable()
                        select new WFB2DA0200EMP_DAY_DUTY_DAO
                    {
                        EMP_ID = (item.Table.Columns.Contains("EMP_ID") ? item.Field<string>("EMP_ID") : null),
                        CALENDAR_DT = (item.Table.Columns.Contains("CALENDAR_DT") ? item.Field<DateTime>("CALENDAR_DT").ToString("yyyy/MM/dd") : null)
                    }).ToList();
            else
                return (from item in dbConn.Query(sb, ht).AsEnumerable()
                        select new WFB2DA0200EMP_DAY_DUTY_DAO
                        {
                            EMP_ID = (item.Table.Columns.Contains("EMP_ID") ? item.Field<string>("EMP_ID") : null),
                            CALENDAR_DT = (item.Table.Columns.Contains("CALENDAR_DT") ? item.Field<DateTime>("CALENDAR_DT").ToString("yyyy/MM/dd") : null)
                        }).ToList();
        }
        catch (Exception)
        {

            throw;
        }
    }

    public void ReOpenEMP_DAY_DUTY(List<WFB2DA0200EMP_DAY_DUTY_DAO> daos, bool isOnTran)
    {
        try
        {
            foreach (WFB2DA0200EMP_DAY_DUTY_DAO dao in daos)
            {
                StringBuilder sb = new StringBuilder();
                Hashtable ht = new Hashtable();

                sb.Append("SP_D_EMP_DUTY_CHECK_STATUS_RE_OPEN");
                ht.Add("@pEmpId", dao.EMP_ID);
                ht.Add("@pCalendarDt", dao.CALENDAR_DT);
                ht.Add("@pUserID", SessionHandle.Current.emp_id);
                ht.Add("@pFuncID", SessionHandle.Current.FUNC_ID);
                if (isOnTran)
                    dbConn.ExecuteSPT(sb, ht, false);
                else
                    dbConn.ExecuteSP(sb, ht, false);
            }
        }
        catch (Exception)
        {

            throw;
        }
    }

    public WFB2DA0200DAO GetSinglSHIFT_Data(WFB2DA0200DAO dao, bool isOnTran)
    {
        try
        {
            WFB2DA0200DAO ReturnValue = new WFB2DA0200DAO();
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select [SHIFT_CD] ");
            sb.AppendLine("       ,[START_DT] ");
            sb.AppendLine("       ,[END_DT] ");
            sb.AppendLine("       ,[SHIFT_DESC] ");
            sb.AppendLine("       ,[SHIFT_TIME_CD] ");
            sb.AppendLine("       ,[WORK_HOUR] ");
            sb.AppendLine("       ,[WORK_PERIOD_HOUR] ");
            sb.AppendLine("       ,[DUTY_STIME] ");
            sb.AppendLine("       ,[DUTY_ETIME] ");
            sb.AppendLine("       ,[WORK_SHIFT_ALLOWANCE_TYPE] ");
            sb.AppendLine("       ,[IS_IFLOW_SHOW] ");
            sb.AppendLine("       ,[REMARK] ");
            sb.AppendLine("       ,[CREATED_BY] ");
            sb.AppendLine("       ,[CREATED_DT] ");
            sb.AppendLine("       ,[UPDATED_BY] ");
            sb.AppendLine("       ,[UPDATED_DT] ");
            sb.AppendLine("       ,[FUNC_ID] ");
            sb.AppendLine(" from TB_D_M_SHIFT_H ");
            sb.AppendLine(" where SHIFT_CD=@SHIFT_CD and START_DT=@START_DT ");

            ht.Add("@SHIFT_CD", dao.SHIFT_CD.ToUpper());
            ht.Add("@START_DT", dao.START_DT.ToString("yyyy-MM-dd"));

            if (isOnTran)
                ReturnValue = (from item in dbConn.QueryT(sb, ht).AsEnumerable()
                               select new WFB2DA0200DAO
                               {
                                   SHIFT_CD = (item.Table.Columns.Contains("SHIFT_CD") ? item.Field<string>("SHIFT_CD").ToUpper() : null),
                                   START_DT = item.Field<DateTime>("START_DT"),
                                   END_DT = (item.Table.Columns.Contains("END_DT") ? item.Field<DateTime?>("END_DT") : null),
                                   SHIFT_DESC = (item.Table.Columns.Contains("SHIFT_DESC") ? item.Field<string>("SHIFT_DESC") : null),
                                   SHIFT_TIME_CD = (item.Table.Columns.Contains("SHIFT_TIME_CD") ? item.Field<string>("SHIFT_TIME_CD").ToUpper() : null),
                                   WORK_HOUR = (item.Table.Columns.Contains("WORK_HOUR") ? item.Field<string>("WORK_HOUR") : null),
                                   WORK_PERIOD_HOUR = (item.Table.Columns.Contains("WORK_PERIOD_HOUR") ? item.Field<string>("WORK_PERIOD_HOUR") : null),
                                   DUTY_STIME = (item.Table.Columns.Contains("DUTY_STIME") ? item.Field<string>("DUTY_STIME") : null),
                                   DUTY_ETIME = (item.Table.Columns.Contains("DUTY_ETIME") ? item.Field<string>("DUTY_ETIME") : null),
                                   WORK_SHIFT_ALLOWANCE_TYPE = (item.Table.Columns.Contains("WORK_SHIFT_ALLOWANCE_TYPE") ? item.Field<string>("WORK_SHIFT_ALLOWANCE_TYPE") : null),
                                   IS_IFLOW_SHOW = (item.Table.Columns.Contains("IS_IFLOW_SHOW") ? item.Field<string>("IS_IFLOW_SHOW") : null),
                                   REMARK = (item.Table.Columns.Contains("REMARK") ? item.Field<string>("REMARK") : null),
                                   CREATED_BY = (item.Table.Columns.Contains("CREATED_BY") ? item.Field<string>("CREATED_BY") : null),
                                   CREATED_DT = item.Field<DateTime>("CREATED_DT"),
                                   UPDATED_BY = (item.Table.Columns.Contains("UPDATED_BY") ? item.Field<string>("UPDATED_BY") : null),
                                   UPDATED_DT = item.Field<DateTime>("UPDATED_DT"),
                                   FUNC_ID = (item.Table.Columns.Contains("FUNC_ID") ? item.Field<string>("FUNC_ID") : null)
                               }).ToList().First();
            else
                ReturnValue = (from item in dbConn.Query(sb, ht).AsEnumerable()
                               select new WFB2DA0200DAO
                               {
                                   SHIFT_CD = (item.Table.Columns.Contains("SHIFT_CD") ? item.Field<string>("SHIFT_CD").ToUpper() : null),
                                   START_DT = item.Field<DateTime>("START_DT"),
                                   END_DT = (item.Table.Columns.Contains("END_DT") ? item.Field<DateTime?>("END_DT") : null),
                                   SHIFT_DESC = (item.Table.Columns.Contains("SHIFT_DESC") ? item.Field<string>("SHIFT_DESC") : null),
                                   SHIFT_TIME_CD = (item.Table.Columns.Contains("SHIFT_TIME_CD") ? item.Field<string>("SHIFT_TIME_CD").ToUpper() : null),
                                   WORK_HOUR = (item.Table.Columns.Contains("WORK_HOUR") ? item.Field<string>("WORK_HOUR") : null),
                                   WORK_PERIOD_HOUR = (item.Table.Columns.Contains("WORK_PERIOD_HOUR") ? item.Field<string>("WORK_PERIOD_HOUR") : null),
                                   DUTY_STIME = (item.Table.Columns.Contains("DUTY_STIME") ? item.Field<string>("DUTY_STIME") : null),
                                   DUTY_ETIME = (item.Table.Columns.Contains("DUTY_ETIME") ? item.Field<string>("DUTY_ETIME") : null),
                                   WORK_SHIFT_ALLOWANCE_TYPE = (item.Table.Columns.Contains("WORK_SHIFT_ALLOWANCE_TYPE") ? item.Field<string>("WORK_SHIFT_ALLOWANCE_TYPE") : null),
                                   IS_IFLOW_SHOW = (item.Table.Columns.Contains("IS_IFLOW_SHOW") ? item.Field<string>("IS_IFLOW_SHOW") : null),
                                   REMARK = (item.Table.Columns.Contains("REMARK") ? item.Field<string>("REMARK") : null),
                                   CREATED_BY = (item.Table.Columns.Contains("CREATED_BY") ? item.Field<string>("CREATED_BY") : null),
                                   CREATED_DT = item.Field<DateTime>("CREATED_DT"),
                                   UPDATED_BY = (item.Table.Columns.Contains("UPDATED_BY") ? item.Field<string>("UPDATED_BY") : null),
                                   UPDATED_DT = item.Field<DateTime>("UPDATED_DT"),
                                   FUNC_ID = (item.Table.Columns.Contains("FUNC_ID") ? item.Field<string>("FUNC_ID") : null)
                               }).ToList().First();
            sb.Clear();
            ht.Clear();
            sb.AppendLine(" SELECT [SHIFT_CD] ");
            sb.AppendLine("       ,[START_DT] ");
            sb.AppendLine("       ,[TIME_CD] ");
            sb.AppendLine("       ,[DUTY_BEFORE_REST_STIME_1] ");
            sb.AppendLine("       ,[DUTY_BEFORE_REST_ETIME_1] ");
            sb.AppendLine("       ,[CREATED_BY] ");
            sb.AppendLine("       ,[CREATED_DT] ");
            sb.AppendLine("       ,[UPDATED_BY] ");
            sb.AppendLine("       ,[UPDATED_DT] ");
            sb.AppendLine("       ,[FUNC_ID] ");
            sb.AppendLine(" FROM [TB_D_M_SHIFT_D] ");
            sb.AppendLine(" where SHIFT_CD=@SHIFT_CD ");
            sb.AppendLine("   and START_DT=@START_DT ");
            ht.Add("@SHIFT_CD", dao.SHIFT_CD.ToUpper());
            ht.Add("@START_DT", dao.START_DT.ToString("yyyy-MM-dd"));

            if (isOnTran)
                ReturnValue.Dtl = (from item in dbConn.QueryT(sb, ht).AsEnumerable()
                                   select new WFB2DA0200DtlDAO
                                   {
                                       SHIFT_CD = (item.Table.Columns.Contains("SHIFT_CD") ? item.Field<string>("SHIFT_CD").ToUpper() : null),
                                       START_DT = item.Field<DateTime>("START_DT"),
                                       TIME_CD = (item.Table.Columns.Contains("TIME_CD") ? item.Field<string>("TIME_CD").ToUpper() : null),
                                       DUTY_BEFORE_REST_STIME_1 = (item.Table.Columns.Contains("DUTY_BEFORE_REST_STIME_1") ? item.Field<string>("DUTY_BEFORE_REST_STIME_1") : null),
                                       DUTY_BEFORE_REST_ETIME_1 = (item.Table.Columns.Contains("DUTY_BEFORE_REST_ETIME_1") ? item.Field<string>("DUTY_BEFORE_REST_ETIME_1") : null),
                                       CREATED_BY = (item.Table.Columns.Contains("CREATED_BY") ? item.Field<string>("CREATED_BY") : null),
                                       CREATED_DT = item.Field<DateTime>("CREATED_DT"),
                                       UPDATED_BY = (item.Table.Columns.Contains("UPDATED_BY") ? item.Field<string>("UPDATED_BY") : null),
                                       UPDATED_DT = item.Field<DateTime>("UPDATED_DT"),
                                       FUNC_ID = (item.Table.Columns.Contains("FUNC_ID") ? item.Field<string>("FUNC_ID") : null)
                                   }).ToList();
            else
                ReturnValue.Dtl = (from item in dbConn.Query(sb, ht).AsEnumerable()
                                   select new WFB2DA0200DtlDAO
                                   {
                                       SHIFT_CD = (item.Table.Columns.Contains("SHIFT_CD") ? item.Field<string>("SHIFT_CD").ToUpper() : null),
                                       START_DT = item.Field<DateTime>("START_DT"),
                                       TIME_CD = (item.Table.Columns.Contains("TIME_CD") ? item.Field<string>("TIME_CD").ToUpper() : null),
                                       DUTY_BEFORE_REST_STIME_1 = (item.Table.Columns.Contains("DUTY_BEFORE_REST_STIME_1") ? item.Field<string>("DUTY_BEFORE_REST_STIME_1") : null),
                                       DUTY_BEFORE_REST_ETIME_1 = (item.Table.Columns.Contains("DUTY_BEFORE_REST_ETIME_1") ? item.Field<string>("DUTY_BEFORE_REST_ETIME_1") : null),
                                       CREATED_BY = (item.Table.Columns.Contains("CREATED_BY") ? item.Field<string>("CREATED_BY") : null),
                                       CREATED_DT = item.Field<DateTime>("CREATED_DT"),
                                       UPDATED_BY = (item.Table.Columns.Contains("UPDATED_BY") ? item.Field<string>("UPDATED_BY") : null),
                                       UPDATED_DT = item.Field<DateTime>("UPDATED_DT"),
                                       FUNC_ID = (item.Table.Columns.Contains("FUNC_ID") ? item.Field<string>("FUNC_ID") : null)
                                   }).ToList();
            return ReturnValue;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public int CheckTB_D_M_WORK_SHIFT_DUnValid(WFB2DA0200DAO dao, bool isOnTran)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select count(1) total_record from TB_D_M_WORK_SHIFT_D A ");
            sb.AppendLine(" where A.SHIFT_CD = @SHIFT_CD ");
            sb.AppendLine("   and A.CALENDAR_DT > @START_DT ");
            sb.AppendLine("   and A.CALENDAR_DT <=@END_DT ");
            ht.Add("@SHIFT_CD", dao.SHIFT_CD.ToUpper());
            ht.Add("@START_DT", dao.START_DT.ToString("yyyy-MM-dd"));
            ht.Add("@END_DT", dao.END_DT == null ? null : Convert.ToDateTime(dao.END_DT).ToString("yyyy-MM-dd"));
            if (isOnTran)
                return Convert.ToInt32(dbConn.QueryT(sb, ht).Rows[0]["total_record"]);
            else
                return Convert.ToInt32(dbConn.Query(sb, ht).Rows[0]["total_record"]);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public int CheckTB_D_M_WORK_SHIFT_DByDel(WFB2DA0200DAO dao, bool isOnTran)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select count(1) total_record from TB_D_M_WORK_SHIFT_D A ");
            sb.AppendLine(" where A.SHIFT_CD = @SHIFT_CD ");
            ht.Add("@SHIFT_CD", dao.SHIFT_CD.ToUpper());
            if (isOnTran)
                return Convert.ToInt32(dbConn.QueryT(sb, ht).Rows[0]["total_record"]);
            else
                return Convert.ToInt32(dbConn.Query(sb, ht).Rows[0]["total_record"]);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public int CheckTB_D_M_EMP_DAY_DUTY_ByDel(WFB2DA0200DAO dao, bool isOnTran)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" select count(1) total_record ");
            sb.AppendLine(" from (select WORK_SHIFT_CD,SHIFT_CD ");
            sb.AppendLine(" 	  from TB_D_M_WORK_SHIFT_D A1 ");
            sb.AppendLine(" 	  where A1.SHIFT_CD = @SHIFT_CD) A  ");
            sb.AppendLine(" inner join TB_D_M_EMP_DAY_DUTY B on A.SHIFT_CD = B.SHIFT_CD ");

            ht.Add("@SHIFT_CD", dao.SHIFT_CD.ToUpper());

            if (isOnTran)
                return Convert.ToInt32(dbConn.QueryT(sb, ht).Rows[0]["total_record"]);
            else
                return Convert.ToInt32(dbConn.Query(sb, ht).Rows[0]["total_record"]);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public void DeleteTB_D_M_SHIFT_H(WFB2DA0200DAO dao, bool isOnTran)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //寫log
            sb.Append(" update TB_D_M_SHIFT_H set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2DA020' ");
            sb.Append(" where SHIFT_CD=@SHIFT_CD ");
            sb.Append("   and START_DT=@START_DT; ");
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);

            sb.AppendLine(" delete from [TB_D_M_SHIFT_H] ");
            sb.AppendLine(" where SHIFT_CD=@SHIFT_CD ");
            sb.AppendLine("   and START_DT=@START_DT; ");
            ht.Add("@SHIFT_CD", dao.SHIFT_CD.ToUpper());
            ht.Add("@START_DT", dao.START_DT.ToString("yyyy-MM-dd"));

            if (isOnTran)
                dbConn.ExecuteT(sb, ht);
            else
                dbConn.Execute(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public void DeleteTB_D_M_SHIFT_D(WFB2DA0200DtlDAO Dtl, bool isOnTran)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //寫log
            sb.Append(" update TB_D_M_SHIFT_D set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2DA020' ");
            sb.Append(" where SHIFT_CD=@SHIFT_CD ");
            sb.Append("   and START_DT=@START_DT ");
            sb.Append("   and TIME_CD=@TIME_CD; ");
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);

            sb.AppendLine(" delete from [TB_D_M_SHIFT_D] ");
            sb.AppendLine(" where SHIFT_CD=@SHIFT_CD ");
            sb.AppendLine("   and START_DT=@START_DT ");
            sb.AppendLine("   and TIME_CD=@TIME_CD; ");
            ht.Add("@SHIFT_CD", Dtl.SHIFT_CD.ToUpper());
            ht.Add("@START_DT", Dtl.START_DT.ToString("yyyy-MM-dd"));
            ht.Add("@TIME_CD", Dtl.TIME_CD.ToUpper());

            if (isOnTran)
                dbConn.ExecuteT(sb, ht);
            else
                dbConn.Execute(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public void DeleteTB_D_M_SHIFT_DByH(WFB2DA0200DAO Dao, bool isOnTran)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //寫log
            sb.Append(" update TB_D_M_SHIFT_D set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2DA020' ");
            sb.Append(" where SHIFT_CD=@SHIFT_CD ");
            sb.Append("   and START_DT=@START_DT; ");
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);

            sb.AppendLine(" delete from [TB_D_M_SHIFT_D] ");
            sb.AppendLine(" where SHIFT_CD=@SHIFT_CD ");
            sb.AppendLine("   and START_DT=@START_DT; ");
            ht.Add("@SHIFT_CD", Dao.SHIFT_CD.ToUpper());
            ht.Add("@START_DT", Dao.START_DT.ToString("yyyy-MM-dd"));

            if (isOnTran)
                dbConn.ExecuteT(sb, ht);
            else
                dbConn.Execute(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public int CheckAddStartDT(string SHIFT_CD, string START_DT)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(@" select count(*) resultCount from TB_D_M_SHIFT_H
                            where SHIFT_CD=@SHIFT_CD
                            and @START_DT < START_DT  ");
            ht.Add("@SHIFT_CD", SHIFT_CD);
            ht.Add("@START_DT", START_DT);
            return Convert.ToInt32(dbConn.Query(sb, ht).Rows[0]["resultCount"]);
        }
        catch (Exception)
        {
            throw;
        }
    }

    internal DataTable get_R_SHIFT_CD_Data()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("SELECT SHIFT_CD,SHIFT_CD+'-'+SHIFT_DESC SHIFT_DESC FROM VW_D_M_SHIFT_H order by SHIFT_CD  ");
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DateTime FN_S_DUTY_EDT(string p)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select  dbo.FN_S_DUTY_EDT(@p) S_DUTY_EDT ");

            ht.Add("@p", p);
            return Convert.ToDateTime(dbConn.Query(sb, ht).Rows[0]["S_DUTY_EDT"]);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DateTime FN_D_DUTY_CLOSE_DT(string p)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select  dbo.FN_D_DUTY_CLOSE_DT(@p) dt ");

            ht.Add("@p", p);
            return Convert.ToDateTime(dbConn.Query(sb, ht).Rows[0]["dt"]);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public WFB2DA0200DAO GetAddSHIFT_D_Data(string shift_cd)
    {
        try
        {
            WFB2DA0200DAO ReturnValue = new WFB2DA0200DAO();
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select [SHIFT_CD] ");
            sb.AppendLine("       ,GETDATE() as START_DT ");
            sb.AppendLine("       ,convert(datetime,'9999/12/31') as  END_DT ");
            sb.AppendLine("       ,[SHIFT_DESC] ");
            sb.AppendLine("       ,[SHIFT_TIME_CD] ");
            sb.AppendLine("       ,[WORK_HOUR] ");
            sb.AppendLine("       ,[WORK_PERIOD_HOUR] ");
            sb.AppendLine("       ,[DUTY_STIME] ");
            sb.AppendLine("       ,[DUTY_ETIME] ");
            sb.AppendLine("       ,[WORK_SHIFT_ALLOWANCE_TYPE] ");
            sb.AppendLine("       ,[REMARK] ");
            sb.AppendLine(" from VW_D_R_SHIFT_H ");
            sb.AppendLine(" where SHIFT_CD=@SHIFT_CD   ");

            ht.Add("@SHIFT_CD", shift_cd);

            ReturnValue = (from item in dbConn.Query(sb, ht).AsEnumerable()
                            select new WFB2DA0200DAO
                            {
                                SHIFT_CD = (item.Table.Columns.Contains("SHIFT_CD") ? item.Field<string>("SHIFT_CD").ToUpper() : null),
                                START_DT = item.Field<DateTime>("START_DT"),
                                END_DT = (item.Table.Columns.Contains("END_DT") ? item.Field<DateTime?>("END_DT") : null),
                                SHIFT_DESC = (item.Table.Columns.Contains("SHIFT_DESC") ? item.Field<string>("SHIFT_DESC") : null),
                                SHIFT_TIME_CD = (item.Table.Columns.Contains("SHIFT_TIME_CD") ? item.Field<string>("SHIFT_TIME_CD").ToUpper() : null),
                                WORK_HOUR = (item.Table.Columns.Contains("WORK_HOUR") ? item.Field<string>("WORK_HOUR") : null),
                                WORK_PERIOD_HOUR = (item.Table.Columns.Contains("WORK_PERIOD_HOUR") ? item.Field<string>("WORK_PERIOD_HOUR") : null),
                                DUTY_STIME = (item.Table.Columns.Contains("DUTY_STIME") ? item.Field<string>("DUTY_STIME") : null),
                                DUTY_ETIME = (item.Table.Columns.Contains("DUTY_ETIME") ? item.Field<string>("DUTY_ETIME") : null),
                                WORK_SHIFT_ALLOWANCE_TYPE = (item.Table.Columns.Contains("WORK_SHIFT_ALLOWANCE_TYPE") ? item.Field<string>("WORK_SHIFT_ALLOWANCE_TYPE") : null),
                                REMARK = (item.Table.Columns.Contains("REMARK") ? item.Field<string>("REMARK") : null),
                            }).ToList().First();
            sb.Clear();
            ht.Clear();

            sb.AppendLine(" SELECT [SHIFT_CD] ");
            sb.AppendLine("       ,[TIME_CD] ");
            sb.AppendLine("       ,[DUTY_BEFORE_REST_STIME_1] ");
            sb.AppendLine("       ,[DUTY_BEFORE_REST_ETIME_1] ");

            sb.AppendLine(" FROM VW_D_R_SHIFT_D ");
            sb.AppendLine(" where SHIFT_CD=@SHIFT_CD ");
            ht.Add("@SHIFT_CD", shift_cd);

            ReturnValue.Dtl = (from item in dbConn.Query(sb, ht).AsEnumerable()
                               select new WFB2DA0200DtlDAO
                               {
                                   SHIFT_CD = (item.Table.Columns.Contains("SHIFT_CD") ? item.Field<string>("SHIFT_CD").ToUpper() : null),
                                   TIME_CD = (item.Table.Columns.Contains("TIME_CD") ? item.Field<string>("TIME_CD").ToUpper() : null),
                                   DUTY_BEFORE_REST_STIME_1 = (item.Table.Columns.Contains("DUTY_BEFORE_REST_STIME_1") ? item.Field<string>("DUTY_BEFORE_REST_STIME_1") : null),
                                   DUTY_BEFORE_REST_ETIME_1 = (item.Table.Columns.Contains("DUTY_BEFORE_REST_ETIME_1") ? item.Field<string>("DUTY_BEFORE_REST_ETIME_1") : null),
                               }).ToList();
            return ReturnValue;
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable get_SHIFT_CD_Data(string SHIFT_CD)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("SELECT COUNT(*) ct FROM TB_D_M_SHIFT_H where SHIFT_CD = @SHIFT_CD  ");
            ht.Add("@SHIFT_CD", SHIFT_CD);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }










}