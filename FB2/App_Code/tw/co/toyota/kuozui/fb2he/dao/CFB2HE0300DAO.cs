using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;

/// <summary>
/// CFB2HE0300DAO 的摘要描述
/// </summary>
public class CFB2HE0300DAO : BaseDAO
{
    //he030基本欄位

    public string LICENSE_ID { get; set; }
    public string PJOB_CD { get; set; }
    public string APPLY_DT { get; set; }

    public string APPROVE_BY { get; set; }
    public string APPROVE_DT { get; set; }
    public string APPROVE_STATUS { get; set; }
    public string APPROVE_REMARK { get; set; }

    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }

    //ACES
    bool isSuper { get; set; }  //是否為supervisor(擔當)

    ACESLib.ACES aces;
    public CFB2HE0300DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
   

    //駁回
    public void reject(DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_H_M_APPLICANT_JOB ");
            sb.Append(" set APPROVE_BY = @APPROVE_BY ");
            sb.Append(" ,APPROVE_DT = @APPROVE_DT");
            sb.Append(" ,APPROVE_STATUS = @APPROVE_STATUS");
            sb.Append(" ,APPROVE_REMARK = @APPROVE_REMARK");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT = @UPDATED_DT");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where LICENSE_ID = @LICENSE_ID ");
            sb.Append(" and PJOB_CD = @PJOB_CD ");
            sb.Append(" and APPLY_DT = @APPLY_DT ");
            //set值
            ht.Add("@APPROVE_BY", APPROVE_BY);
            ht.Add("@APPROVE_DT", now);
            ht.Add("@APPROVE_STATUS", "B"); //B:駁回
            ht.Add("@APPROVE_REMARK", APPROVE_REMARK);

            //新修日期
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@UPDATED_DT", now);
            ht.Add("@FUNC_ID", FUNC_ID);

            //PK值
            ht.Add("@LICENSE_ID", LICENSE_ID);
            ht.Add("@PJOB_CD", PJOB_CD);
            ht.Add("@APPLY_DT", Convert.ToDateTime(APPLY_DT).ToString("yyyy/MM/dd"));

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //核可
    public void approve(DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_H_M_APPLICANT_JOB ");
            sb.Append(" set APPROVE_BY = @APPROVE_BY ");
            sb.Append(" ,APPROVE_DT = @APPROVE_DT");
            sb.Append(" ,APPROVE_STATUS = @APPROVE_STATUS");
            sb.Append(" ,APPROVE_REMARK = @APPROVE_REMARK");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT = @UPDATED_DT");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where LICENSE_ID = @LICENSE_ID ");
            sb.Append(" and PJOB_CD = @PJOB_CD ");
            sb.Append(" and APPLY_DT = @APPLY_DT ");
            //set值
            ht.Add("@APPROVE_BY", APPROVE_BY);
            ht.Add("@APPROVE_DT", now);
            ht.Add("@APPROVE_STATUS", "Y"); //Y:核可
            ht.Add("@APPROVE_REMARK", APPROVE_REMARK);

            //新修日期
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@UPDATED_DT", now);
            ht.Add("@FUNC_ID", FUNC_ID);

            //PK值
            ht.Add("@LICENSE_ID", LICENSE_ID);
            ht.Add("@PJOB_CD", PJOB_CD);
            ht.Add("@APPLY_DT", Convert.ToDateTime(APPLY_DT).ToString("yyyy/MM/dd"));

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    #region Qry Gridview 資料
    //Gridview 查詢資料
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression
            , string pjob_cd, string interview_process_status, string adopt_DT_S, string adopt_DT_E, string adopt_by
            , string adopt_result, string approve_DT_S, string approve_DT_E, string approve_by, string approve_status
    )
    {
        try
        {
            if (sortExpression.Contains("PJOB_CD"))
                sortExpression = sortExpression.Replace("PJOB_CD", "D.PJOB_CD");
            if (sortExpression.Contains("EMP_CD"))
                sortExpression = sortExpression.Replace("EMP_CD", "D.EMP_CD");
            if (sortExpression.Contains("WS_CD"))
                sortExpression = sortExpression.Replace("WS_CD", "D.WS_CD");
            if (sortExpression.Contains("COMPANY_CD"))
                sortExpression = sortExpression.Replace("COMPANY_CD", "D.COMPANY_CD");
            if (sortExpression.Contains("PLANT_CD"))
                sortExpression = sortExpression.Replace("PLANT_CD", "D.PLANT_CD");
            if (sortExpression.Contains("PLANT_CD"))
                sortExpression = sortExpression.Replace("DEPT_NO", "D.DEPT_NO");
            if (sortExpression.Contains("LEVEL_CD"))
                sortExpression = sortExpression.Replace("LEVEL_CD", "D.LEVEL_CD");
            if (sortExpression.Contains("GRADE_CD"))
                sortExpression = sortExpression.Replace("GRADE_CD", "D.GRADE_CD");
            if (sortExpression.Contains("WORK_CD"))
                sortExpression = sortExpression.Replace("WORK_CD", "D.WORK_CD");
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + ") As RowNumber,");
            sb.Append(@" H.EMP_NAME
                        ,D.APPLY_DT
                        ,D.PJOB_CD+' '+D.PJOB_DESC PJOB_CD_DESC
                        ,D.LEVEL_CD,D.GRADE_CD
                        ,D.EMP_CD+'-'+b.SUB_DESC EMP_CD_DESC
                        ,IIF(D.ADOPT_RESULT='','', D.ADOPT_RESULT+'-'+ c.SUB_DESC ) ADOPT_RESULT_DESC 
                        ,IIF(D.ADOPT_BY='','', E1.EMP_NAME)	ADOPT_NAME
                        ,D.ADOPT_DT
                        ,IIF(D.APPROVE_STATUS='','', D.APPROVE_STATUS+'-'+ e.SUB_DESC )	APPROVE_STATUS_DESC 
                        ,IIF(D.APPROVE_BY='','', E2.EMP_NAME)	APPROVE_NAME
                        ,APPROVE_DT ,D.LICENSE_ID,D.PJOB_CD
                        ,ADOPT_RESULT,APPROVE_STATUS
                        from TB_H_M_APPLICANT H
                        left join TB_H_M_APPLICANT_JOB  D on H.LICENSE_ID=D.LICENSE_ID
                        left join TB_9_M_COMM_D b on  D.EMP_CD = b.SUB_CD and b.MAIN_CD = 'EMP_CD'  and b.IS_VALID='Y'  and b.SYS_CD='HB'
                        left join TB_9_M_COMM_D c on  D.ADOPT_RESULT = c.SUB_CD and c.MAIN_CD = 'ADOPT_RESULT'  and c.IS_VALID='Y'  and c.SYS_CD='HE'
                        left join TB_9_M_COMM_D e on  D.APPROVE_STATUS = e.SUB_CD and e.MAIN_CD = 'APPROVE_STATUS'  and e.IS_VALID='Y'  and e.SYS_CD='SA'
                        left join TB_H_M_EMP E1 on E1.EMP_ID=D.ADOPT_BY
                        left join TB_H_M_EMP E2 on E2.EMP_ID=D.APPROVE_BY  
                      ");
            sb.Append(" where 1=1 ");

            //查詢條件
            if (pjob_cd != "")
            {
                sb.Append(" and D.PJOB_CD like @PJOB_CD ");
                ht.Add("@PJOB_CD", pjob_cd + "%");
            }
            if (interview_process_status != "-1")
            {
                sb.Append(" and D.INTERVIEW_PROCESS_STATUS = @INTERVIEW_PROCESS_STATUS ");
                ht.Add("@INTERVIEW_PROCESS_STATUS", interview_process_status);
            }
            if (adopt_DT_S != "")
            {
                sb.Append(" and D.ADOPT_DT >= @ADOPT_DT_S ");
                ht.Add("@ADOPT_DT_S", adopt_DT_S+" 00:00:00");
            }
            if (adopt_DT_E != "")
            {
                sb.Append(" and D.ADOPT_DT <= @ADOPT_DT_E ");
                ht.Add("@ADOPT_DT_E", adopt_DT_E+" 23:59:59");
            }
            if (adopt_by != "")
            {
                sb.Append(" and D.ADOPT_BY like @ADOPT_BY ");
                ht.Add("@ADOPT_BY", adopt_by + "%");
            }
            if (adopt_result != "-1")
            {
                sb.Append(" and D.ADOPT_RESULT = @ADOPT_RESULT ");
                ht.Add("@ADOPT_RESULT", adopt_result);
            }

            if (approve_DT_S != "")
            {
                sb.Append(" and D.APPROVE_DT >= @APPROVE_DT_S ");
                ht.Add("@APPROVE_DT_S", approve_DT_S + " 00:00:00");
            }
            if (approve_DT_E != "")
            {
                sb.Append(" and D.APPROVE_DT <= @APPROVE_DT_E ");
                ht.Add("@APPROVE_DT_E", approve_DT_E + " 23:59:59");
            }
            if (approve_by != "")
            {
                sb.Append(" and D.APPROVE_BY like @APPROVE_BY ");
                ht.Add("@APPROVE_BY", approve_by + "%");
            }
            if (approve_status != "-1")
            {
                sb.Append(" and D.APPROVE_STATUS = @APPROVE_STATUS ");
                ht.Add("@APPROVE_STATUS", approve_status);
            }





            sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar)");


            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    //Gridview 查詢總筆數
    public int getCount(int startRowIndex, int maximumRows
               , string pjob_cd, string interview_process_status, string adopt_DT_S, string adopt_DT_E, string adopt_by
            , string adopt_result, string approve_DT_S, string approve_DT_E, string approve_by, string approve_status)
    {
        try
        {
            int result = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) resultCount ");
            sb.Append(@" from TB_H_M_APPLICANT H
                        left join TB_H_M_APPLICANT_JOB  D on H.LICENSE_ID=D.LICENSE_ID ");
            sb.Append(" where 1=1 ");

            //查詢條件
            if (pjob_cd != "")
            {
                sb.Append(" and D.PJOB_CD like @PJOB_CD ");
                ht.Add("@PJOB_CD", pjob_cd + "%");
            }
            if (interview_process_status != "-1")
            {
                sb.Append(" and D.INTERVIEW_PROCESS_STATUS = @INTERVIEW_PROCESS_STATUS ");
                ht.Add("@INTERVIEW_PROCESS_STATUS", interview_process_status);
            }
            if (adopt_DT_S != "")
            {
                sb.Append(" and D.ADOPT_DT >= @ADOPT_DT_S ");
                ht.Add("@ADOPT_DT_S", adopt_DT_S + " 00:00:00");
            }
            if (adopt_DT_E != "")
            {
                sb.Append(" and D.ADOPT_DT <= @ADOPT_DT_E ");
                ht.Add("@ADOPT_DT_E", adopt_DT_E + " 23:59:59");
            }
            if (adopt_by != "")
            {
                sb.Append(" and D.ADOPT_BY like @ADOPT_BY ");
                ht.Add("@ADOPT_BY", adopt_by + "%");
            }
            if (adopt_result != "-1")
            {
                sb.Append(" and D.ADOPT_RESULT = @ADOPT_RESULT ");
                ht.Add("@ADOPT_RESULT", adopt_result);
            }

            if (approve_DT_S != "")
            {
                sb.Append(" and D.APPROVE_DT >= @APPROVE_DT_S ");
                ht.Add("@APPROVE_DT_S", approve_DT_S + " 00:00:00");
            }
            if (approve_DT_E != "")
            {
                sb.Append(" and D.APPROVE_DT <= @APPROVE_DT_E ");
                ht.Add("@APPROVE_DT_E", approve_DT_E + " 23:59:59");
            }
            if (approve_by != "")
            {
                sb.Append(" and D.APPROVE_BY like @APPROVE_BY ");
                ht.Add("@APPROVE_BY", approve_by + "%");
            }
            if (approve_status != "-1")
            {
                sb.Append(" and D.APPROVE_STATUS = @APPROVE_STATUS ");
                ht.Add("@APPROVE_STATUS", approve_status);
            }


            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                result = (int)dt.Rows[0]["resultCount"];
            }
            return result;
        }
        catch (Exception)
        {

            throw;
        }

    }

    #endregion





}