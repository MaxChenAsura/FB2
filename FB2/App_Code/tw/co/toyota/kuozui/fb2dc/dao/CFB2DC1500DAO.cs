using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;

/// <summary>
/// CFB2DC1500DAO 的摘要描述
/// </summary>
public class CFB2DC1500DAO : BaseDAO
{
    public CFB2DC1500DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public string DUTY_YM { get; set; }

    public string PLANT_CD { get; set; }

    public string DEPT_NO { get; set; }

    public string DEPT_NAME { get; set; }

    public string WS_CD { get; set; }

    public string WORK_CD { get; set; }


    //欠勤率
    public System.Data.DataTable searchResult1()
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //sb.AppendLine(" select A1.PLANT_CD,A1.DEPT_NO,A1.DEPT_NO_40,A1.DEPT_NAME_40,A1.EMP_ID,");
            sb.Append("select * from ( ");
            sb.Append(@"select A1.PLANT_CD,A1.EMP_ID
                        ,A1.DEPT_NO,A1.PJOB_CD,A1.PJOB_DESC,A1.DEPT_NO_40,A1.DEPT_NAME_40
                        ,A1.EMP_NAME,A1.WS_CD,A1.WORK_CD,A1.LEVEL_CD,A1.COMPANY_CD,A1.SEX_CD,A1.EMP_CHG_DESC,
                    ");
            sb.AppendLine(" CAST(ROUND(((convert(decimal,A2.WORK_HOUR)) / 60), 2) AS DECIMAL(12, 2)) WORK_HOUR,");
            sb.AppendLine(" CAST(ROUND(((isnull(convert(decimal,A3.TOTAL_TIME_APPROVE),0)) / 60), 2) AS DECIMAL(12, 2)) TOTAL_TIME_APPROVE,");
            sb.AppendLine(" CAST(ROUND(((convert(decimal,A2.WORK_HOUR) - isnull(convert(decimal,A3.TOTAL_TIME_APPROVE),0)) / 60), 2) AS DECIMAL(12, 2)) ACTUAL_TIME_APPROVE  from");
            sb.AppendLine(" (");
            sb.AppendLine(" select A.EMP_ID, SUM(dbo.FN_D_GET_WORK_HOUR(WORK_HOUR,'M')) WORK_HOUR 	FROM TB_D_M_EMP_DAY_DUTY A ");
            sb.AppendLine(" LEFT JOIN  VW_H_EMP_DATA  B  ON A.EMP_ID = B.EMP_ID ");
            //sb.AppendLine(" LEFT JOIN  TB_S_M_EMP_RESULT  C  ON B.emp_id = C.emp_id  and  C.SALARY_YM = @YM ");
            sb.AppendLine(" LEFT JOIN  TB_H_R_EMP_DATA_MONTH C  ON B.emp_id = C.emp_id  and  C.YM = @YM  ");
            sb.AppendLine(" WHERE  substring(convert(char(10),A.CALENDAR_DT,120),0,8) = @YM2 and A.WORK_DAY_CD ='1'");
            sb.AppendLine(" GROUP BY A.EMP_ID ");
            sb.AppendLine(" )A2 ");
            sb.AppendLine(" left join");
            sb.AppendLine(" (");
            sb.AppendLine(" select A.EMP_ID , 	SUM(TOTAL_TIME_APPROVE) TOTAL_TIME_APPROVE");
            sb.AppendLine(" FROM     TB_D_M_LEAVE_APPLY_DAY A");
            sb.AppendLine(" LEFT JOIN  VW_H_EMP_DATA  B  ON A.EMP_ID = B.EMP_ID ");
            //sb.AppendLine(" LEFT JOIN  TB_S_M_EMP_RESULT  C  ON B.emp_id = C.emp_id  and  C.SALARY_YM = @YM ");
            sb.AppendLine(" LEFT JOIN  TB_H_R_EMP_DATA_MONTH C  ON B.emp_id = C.emp_id  and  C.YM = @YM  ");

            sb.AppendLine(" WHERE  substring(convert(char(10),APPLY_LEAVE_SDT,120),0,8) = @YM2");
            sb.AppendLine(" AND  A.FORM_STATUS  not in ('N','D') ");
            sb.AppendLine(" AND  SALARY_SETTLE_STATUS<>'N' ");
            sb.AppendLine(" AND MAIN_LEAVE_CD not in('L','R','4','R') ");
            

            sb.AppendLine(" GROUP BY A.EMP_ID  ");
            sb.AppendLine(" )A3 on A2.EMP_ID = A3.EMP_ID ");
            sb.AppendLine(@" left join ( select PLANT_CD,DEPT_NO,DEPT_NO_40,DEPT_NAME_40,EMP_ID,EMP_NAME,PJOB_CD,PJOB_DESC,LEVEL_CD,WS_CD,WORK_CD,COMPANY_CD,SEX_CD,EMP_CHG_DESC 
                            from TB_H_R_EMP_DATA_MONTH where YM=@YM
                           ");
            sb.AppendLine(" )A1 on A1.EMP_ID = A2.EMP_ID");
            sb.AppendLine(" Where A1.EMP_ID is not null ");
            sb.AppendLine(" ) as final where 1=1 ");

            if (DEPT_NO != "")
            {
                sb.AppendLine(" AND DEPT_NO = @DEPT_NO ");
                ht.Add("@DEPT_NO", DEPT_NO);
            }
            if (PLANT_CD != "-1" && PLANT_CD != "")
            {
                sb.AppendLine(" AND PLANT_CD = @PLANT_CD ");
                ht.Add("@PLANT_CD", PLANT_CD);
            }
            if (WS_CD != "-1" && WS_CD != "")
            {
                sb.AppendLine(" AND WS_CD = @WS_CD ");
                ht.Add("@WS_CD", WS_CD);
            }
            if (WORK_CD != "-1" && WORK_CD != "")
            {
                sb.AppendLine(" AND WORK_CD = @WORK_CD ");
                ht.Add("@WORK_CD", WORK_CD);
            }

            ht.Add("@YM", DUTY_YM.Replace("/", ""));
            ht.Add("@YM2", DUTY_YM.Replace("/", "-"));

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    //請假統計
    internal DataTable searchResult2()
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select * from ( ");
            sb.AppendLine(" Select A3.PLANT_CD,A3.DEPT_NO,A3.DEPT_NO_40,A3.DEPT_NAME_40,");
            sb.AppendLine(" A3.EMP_ID,A3.EMP_NAME,A3.PJOB_CD,A3.WS_CD,A3.WORK_CD,A3.PJOB_DESC,A3.LEVEL_CD,A3.COMPANY_CD,A3.SEX_CD,A3.EMP_CHG_DESC,");
            sb.AppendLine(" A1.MAIN_LEAVE_CD,");
            sb.AppendLine(" CAST(ROUND(((SUM(A1.TOTAL_TIME_APPROVE)) / 60), 2) AS DECIMAL(12, 2)) TOTAL_TIME_APPROVE,");
            sb.AppendLine(" (select CAST(ROUND(((SUM(LACK_HOUR)) / 60), 2) AS DECIMAL(12, 2)) LACK_HOUR from TB_D_M_EMP_DUTY_CHECK_STATUS  ");
            sb.AppendLine(" where substring(convert(CHAR(10), CALENDAR_DT, 120), 0, 8) = @YM2 ");
            sb.AppendLine(" and DUTY_CHECK_RESULT = 'E3' ");
            sb.AppendLine(" and TB_D_M_EMP_DUTY_CHECK_STATUS.EMP_ID = A3.EMP_ID");
            sb.AppendLine(" group by EMP_ID");
            sb.AppendLine(" ) LACK_HOUR");
            sb.AppendLine(" from (");
            sb.AppendLine(" SELECT  A.EMP_ID ,  MAIN_LEAVE_CD, SUB_LEAVE_CD, SUM(TOTAL_TIME_APPROVE)  TOTAL_TIME_APPROVE ");
            sb.AppendLine(" FROM     TB_D_M_LEAVE_APPLY_DAY A");
            sb.AppendLine(" LEFT JOIN  VW_H_EMP_DATA  B  ON A.emp_id = B.emp_id ");
            //sb.AppendLine(" LEFT JOIN  TB_S_M_EMP_RESULT  C  ON B.emp_id = C.emp_id  and  C.SALARY_YM = @YM ");
            sb.AppendLine(" LEFT JOIN  TB_H_R_EMP_DATA_MONTH C  ON B.emp_id = C.emp_id  and  C.YM = @YM  ");

            sb.AppendLine(" WHERE substring(convert(CHAR(10), APPLY_LEAVE_SDT, 120), 0, 8) = @YM2");
            sb.AppendLine(" AND  FORM_STATUS  not in ('N','D')");
            //sb.AppendLine(" AND  SALARY_SETTLE_STATUS<>'N'");
            sb.AppendLine(" AND A.MAIN_LEAVE_CD NOT IN ('X','Y')");
            /*
            if (PLANT_CD != "-1" && PLANT_CD != "")
            {
                sb.AppendLine(" AND B.PLANT_CD = @PLANT_CD ");
                ht.Add("@PLANT_CD", PLANT_CD);
            }
            if (DEPT_NO != "")
            {
                sb.AppendLine(" AND B.DEPT_NO = @DEPT_NO ");
                ht.Add("@DEPT_NO", DEPT_NO);
            }
            if (WS_CD != "-1" && WS_CD != "")
            {
                sb.AppendLine(" AND B.WS_CD = @WS_CD ");
                ht.Add("@WS_CD", WS_CD);
            }
            */
            sb.AppendLine(" GROUP BY A.EMP_ID ,  MAIN_LEAVE_CD, SUB_LEAVE_CD");
            sb.AppendLine(" )A1");
            sb.AppendLine(" left join  ");
            sb.AppendLine(" ( SELECT  PLANT_CD,DEPT_NO,DEPT_NO_40,DEPT_NAME_40,EMP_ID,EMP_NAME,PJOB_CD,PJOB_DESC,LEVEL_CD,WS_CD,WORK_CD,COMPANY_CD,SEX_CD,EMP_CHG_DESC");
            sb.AppendLine(" from TB_H_R_EMP_DATA_MONTH where YM=@YM ");
            sb.AppendLine(" )A3");
            sb.AppendLine(" on A1.EMP_ID = A3.EMP_ID");
            sb.AppendLine(" Where A1.EMP_ID is not null ");
            sb.AppendLine(" group by ");
            sb.AppendLine(" A3.PLANT_CD,A3.DEPT_NO,A3.DEPT_NO_40,A3.DEPT_NAME_40,");
            sb.AppendLine(" A3.EMP_ID,A3.EMP_NAME,A3.PJOB_CD,A3.WS_CD,A3.WORK_CD,");
            sb.AppendLine(" A1.MAIN_LEAVE_CD,A3.PJOB_DESC,A3.LEVEL_CD,A3.COMPANY_CD,A3.SEX_CD,A3.EMP_CHG_DESC ");

            sb.AppendLine(" ) as final where 1=1 ");

            if (DEPT_NO != "")
            {
                sb.AppendLine(" AND DEPT_NO = @DEPT_NO ");
                ht.Add("@DEPT_NO", DEPT_NO);
            }
            if (PLANT_CD != "-1" && PLANT_CD != "")
            {
                sb.AppendLine(" AND PLANT_CD = @PLANT_CD ");
                ht.Add("@PLANT_CD", PLANT_CD);
            }
            if (WS_CD != "-1" && WS_CD != "")
            {
                sb.AppendLine(" AND WS_CD = @WS_CD ");
                ht.Add("@WS_CD", WS_CD);
            }
            if (WORK_CD != "-1" && WORK_CD != "")
            {
                sb.AppendLine(" AND WORK_CD = @WORK_CD ");
                ht.Add("@WORK_CD", WORK_CD);
            }
            sb.AppendLine("order by EMP_ID");
            ht.Add("@YM", DUTY_YM.Replace("/", ""));
            ht.Add("@YM2", DUTY_YM.Replace("/", "-"));



            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    //加班明細
    internal DataTable searchResult3()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();


            sb.AppendLine(" select A2.PLANT_CD,A2.DEPT_NO,A2.DEPT_NO_40,A2.DEPT_NAME_40,A2.PJOB_DESC,A2.LEVEL_CD,A2.COMPANY_CD,A2.SEX_CD,A2.EMP_CHG_DESC,");
            sb.AppendLine(" A2.EMP_ID,A2.EMP_NAME,A2.PJOB_CD,A2.WS_CD,A2.WORK_CD,A1.OVERTIME_CD,A1.APPROVE_OVERTIME_HOUR");
            sb.AppendLine(" from");
            sb.AppendLine(" (");
            sb.AppendLine(" SELECT  A.EMP_ID,  A.OVERTIME_CD,  A.OVERTIME_DT_TYPE, CAST(ROUND(((SUM(APPROVE_OVERTIME_HOUR)) / 60), 2) AS DECIMAL(12, 2))  APPROVE_OVERTIME_HOUR");
            sb.AppendLine(" FROM     TB_D_M_OVERTIME_APPLY A ");
            //sb.AppendLine(" LEFT JOIN  VW_H_EMP_DATA  B  ON A.emp_id = B.emp_id ");
            //sb.AppendLine(" LEFT JOIN  TB_S_M_EMP_RESULT  C  ON B.emp_id = C.emp_id  and  C.SALARY_YM = @YM ");
            //sb.AppendLine(" LEFT JOIN  TB_H_R_EMP_DATA_MONTH C  ON B.emp_id = C.emp_id  and  C.YM = @YM  ");
            sb.AppendLine(" WHERE ");
            sb.AppendLine(" substring(convert(CHAR(10), APPLY_OVERTIME_DT, 120), 0, 8) =@YM2");
            sb.AppendLine(" AND  FORM_STATUS  not in ('N','D') ");
            //sb.AppendLine(" AND  SALARY_SETTLE_STATUS<>'N' ");
            sb.AppendLine(" GROUP BY A. EMP_ID , A.OVERTIME_CD, A.OVERTIME_DT_TYPE");
            sb.AppendLine(" )A1 ");
            sb.AppendLine(" join ");

            sb.AppendLine(" (");
            sb.AppendLine(" SELECT PLANT_CD,DEPT_NO,DEPT_NO_40,DEPT_NAME_40,");
            sb.AppendLine(" EMP_ID,EMP_NAME,PJOB_CD,PJOB_DESC,LEVEL_CD,WS_CD,WORK_CD,COMPANY_CD,SEX_CD,EMP_CHG_DESC");
            sb.AppendLine(" from TB_H_R_EMP_DATA_MONTH where YM=@YM ");
            if (PLANT_CD != "-1" && PLANT_CD != "")
            {
                sb.AppendLine(" AND PLANT_CD = @PLANT_CD ");
                ht.Add("@PLANT_CD", PLANT_CD);
            }
            if (DEPT_NO != "")
            {
                sb.AppendLine(" AND DEPT_NO = @DEPT_NO ");
                ht.Add("@DEPT_NO", DEPT_NO);
            }
            if (WS_CD != "-1" && WS_CD != "")
            {
                sb.AppendLine(" AND WS_CD = @WS_CD ");
                ht.Add("@WS_CD", WS_CD);
            }
            if (WORK_CD != "-1" && WORK_CD != "")
            {
                sb.AppendLine(" AND WORK_CD = @WORK_CD ");
                ht.Add("@WORK_CD", WORK_CD);
            }



            sb.AppendLine(" )A2");
            sb.AppendLine(" on A1.EMP_ID = A2.EMP_ID");
            sb.AppendLine(" Where A1.EMP_ID is not null ");
            
            sb.AppendLine(" order by EMP_ID");

            ht.Add("@YM", DUTY_YM.Replace("/", ""));
            ht.Add("@YM2", DUTY_YM.Replace("/", "-"));



            return dbConn.Query(sb, ht);
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
}