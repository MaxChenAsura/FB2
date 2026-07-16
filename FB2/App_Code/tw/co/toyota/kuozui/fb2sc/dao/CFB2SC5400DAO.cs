using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;
using System.Text;
using System.Collections;


/// <summary>
/// CFB2SC5400BO 的摘要描述
/// </summary>
public class CFB2SC5400DAO : BaseDAO
{
    public CFB2SC5400DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public string RowNumber { get; set; }
    public string QDATAKEY { get; set; }
    public string DATA_YM { get; set; }
    public string EMP_ID { get; set; }
    public string DEPT_NO { get; set; }
    public string JPN_CD { get; set; }
    public string EMP_NAME { get; set; }
    public string EMP_CD { get; set; }
    public string SALARY_ID { get; set; }
    public string SALARY_YM { get; set; }
    public string SALARY_NAME { get; set; }
    public string CHG_AMT_A { get; set; }
    public string CHG_AMT_B { get; set; }
    public string PROCESS_STATUS { get; set; }
    public string START_DT_B { get; set; }
    public string START_DT_A { get; set; }
    public string END_DATE_B { get; set; }
    public string END_DATE_A { get; set; }
    public string CHG_STATUS { get; set; }
    public string APPROVE_DT { get; set; }
    public string APPROVE_BY { get; set; }
    public string REMARK { get; set; }
    public string APP_REMARK { get; set; }
    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }

    public string IS_PLUS { get; set; }
    public string IS_TAX { get; set; }

    public DataTable getJPN_CD()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select SYS_CD , MAIN_CD , SUB_CD ,SUB_DESC from TB_9_M_COMM_D");
            sb.Append(" where SYS_CD='HB' and MAIN_CD='JPN_CD' and IS_VALID = 'Y' ");
            return dbConn.Query(sb);

        }
        catch
        {
            throw;
        }
    }
    public DataTable get_SE2200_PDF_DATA()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(" execute dbo.SP_SE2200PDFDATA  ");
            return dbConn.Query(sb);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable get_PDF_Data(string SALARY_YM, string DEPT_NO, string EMP_ID, string SALARY_DT_S, string SALARY_DT_E)
    {
         try
        {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.Append(" Select	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　");
        sb.Append(" V.DEPT_NO,	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　");
        sb.Append(" V.DEPT_NAME_20,	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　");
        sb.Append(" V.DEPT_NAME_40,	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　");
        sb.Append(" P.EMP_ID,	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　");
        sb.Append(" V.EMP_NAME,	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　");
        sb.Append(" Replace(Convert(Varchar(12),CONVERT(money,Sum(P.AMOUNT)),1),'.00','') as AMOUNT,	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　");
        sb.Append(" (Select Sum(S.AMOUNT * S.IS_PLUS) TAX	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　");
        sb.Append(" 　	From TB_S_S_SALARY_PAY S Join TB_S_M_SALARY_ITEM I On S.SALARY_ID = I.SALARY_ID");
        sb.Append(" 	Where S.SALARY_DT Between CONVERT(DATETIME,@SALARY_DT_S)  And CONVERT(DATETIME,@SALARY_DT_E) 　	　");
        sb.Append(" 　	And S.EMP_ID = P.EMP_ID	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　");
        sb.Append(" 　	And S.PAY_KIND = '9999'	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　");
        sb.Append(" 　	And I.SALARY_CD = '4'	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　");
        sb.Append(" 　	And S.PAY_ID Is Not Null	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　");
        sb.Append(" ) TAX	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　");
        sb.Append(" From TB_S_M_STAFF_ARREARS_D P	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　");
        sb.Append(" Join VW_H_EMP_DATA V	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　");
        sb.Append(" On P.EMP_ID = V.EMP_ID	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　");
        sb.Append(" Where P.SALARY_YM = @SALARY_YM	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　	　");
        ht.Add("@SALARY_YM", SALARY_YM);
        ht.Add("@SALARY_DT_S", SALARY_DT_S);
        ht.Add("@SALARY_DT_E", SALARY_DT_E);


        if (EMP_ID != "")
        {
            sb.Append(" and V.EMP_ID = @EMP_ID ");
            ht.Add("@EMP_ID", EMP_ID);
        }
        if (DEPT_NO != "")
        {
            sb.Append(" and V.DEPT_NO = @DEPT_NO ");
            ht.Add("@DEPT_NO", DEPT_NO);
        }

        sb.Append(" Group By V.DEPT_NO, V.DEPT_NAME_20, V.DEPT_NAME_40, P.EMP_ID, V.EMP_NAME");
        sb.Append(" Order By V.DEPT_NO, P.EMP_ID	");
        return dbConn.Query(sb, ht);
             }
        catch
        {
            throw;
        }
    }

    //public DataTable searchResult()
    //{
    //    try
    //    {

    //        StringBuilder sb = new StringBuilder();
    //        Hashtable ht = new Hashtable();
    //        sb.Append(" Select");
    //        sb.Append(" V.DEPT_NO,");
    //        sb.Append(" V.DEPT_NAME_20,");
    //        sb.Append(" V.DEPT_NAME_40,");
    //        sb.Append(" P.EMP_ID,");
    //        sb.Append(" V.EMP_NAME,");
    //        sb.Append(" Sum(P.AMOUNT) AMOUNT,");
    //        sb.Append(" (Select Sum(S.AMOUNT * S.IS_PLUS) TAX");
    //        sb.Append(" From TB_S_M_SALARY_PAY S");
    //        sb.Append(" Join TB_S_M_SALARY_ITEM I");
    //        sb.Append(" On S.SALARY_ID = I.SALARY_ID");
    //        sb.Append(" Where S.SALARY_DT BETWEEN @DATA_YM+'/01/01' And @DATA_YM+'/12/31'");
    //        ht.Add("@DATA_YM", DATA_YM);
    //        sb.Append(" And S.EMP_ID = P.EMP_ID");
    //        sb.Append(" And S.PAY_KIND = '9999'");
    //        sb.Append(" And I.SALARY_CD = '4'");
    //        sb.Append(" ) TAX");
    //        sb.Append(" From TB_S_M_STAFF_ARREARS_D P");
    //        sb.Append(" Join VW_H_EMP_DATA V");
    //        sb.Append(" On P.EMP_ID = V.EMP_ID");
    //        sb.Append(" Where P.SALARY_YM = @SALARY_YM");
    //        ht.Add("@SALARY_YM", SALARY_YM);
            

    //        if (EMP_ID != "")
    //        {
    //            sb.Append(" and P.EMP_ID = @EMP_ID ");
    //            ht.Add("@EMP_ID", EMP_ID);
    //        }
    //        if (DEPT_NO != "")
    //        {
    //            sb.Append(" and V.DEPT_NO = @DEPT_NO ");
    //            ht.Add("@DEPT_NO", DEPT_NO);
    //        }

    //        sb.Append(" Group By V.DEPT_NO, V.DEPT_NAME_20, V.DEPT_NAME_40, P.EMP_ID, V.EMP_NAME");
    //        sb.Append(" Order By V.DEPT_NO, P.EMP_ID");

            



            
            //if (DEPT_NO != "")
            //{
            //    sb.Append(" and d.DEPT_NO = @DEPT_NO ");
            //    ht.Add("@DEPT_NO", DEPT_NO);
            //}
            //if (EMP_CHG_CD != "-1")
            //{
            //    sb.Append(" and d.EMP_CHG_CD = @EMP_CHG_CD ");
            //    ht.Add("@EMP_CHG_CD", EMP_CHG_CD);
            //}
            //if (LEAVE_DT_S != "")
            //{
            //    if (LEAVE_DT_E != "")
            //    {
            //        sb.Append(" and LEAVE_DT >= CONVERT(datetime,@LEAVE_DT_S) and LEAVE_DT <= CONVERT(datetime,@LEAVE_DT_E)");
            //        ht.Add("@LEAVE_DT_S", LEAVE_DT_S);
            //        ht.Add("@LEAVE_DT_E", LEAVE_DT_E);
            //    }
            //    else
            //    {
            //        sb.Append(" and LEAVE_DT >= CONVERT(datetime,@LEAVE_DT_S) ");
            //        ht.Add("@LEAVE_DT_S", LEAVE_DT_S);
            //    }
            //}
            //else if (LEAVE_DT_E != "")
            //{
            //    sb.Append(" and LEAVE_DT <= CONVERT(datetime,@LEAVE_DT_E) ");
            //    ht.Add("@leave_dt_e", LEAVE_DT_E);
            //}
            //if (ACCOM_CD != "-1" && ACCOM_CD != null)
            //{
            //    sb.Append(" and b.SUB_CD = @accom ");
            //    ht.Add("@accom", ACCOM_CD);
            //}
            //if (ACCOM_BUILD_CD != "-1" && ACCOM_BUILD_CD != null)
            //{
            //    sb.Append(" and b2.SUB_CD = @accom_build ");
            //    ht.Add("@accom_build", ACCOM_BUILD_CD);
            //}
            //if (ROOM_NO != "")
            //{
            //    sb.Append(" and a.ROOM_NO = @room_no ");
            //    ht.Add("@room_no", ROOM_NO);
            //}
            //if (AGE != "")
            //{
            //    switch (age_where)
            //    {
            //        case "greater":
            //            sb.Append(" and AGE > @AGE ");
            //            break;
            //        case "less":
            //            sb.Append(" and AGE < @AGE ");
            //            break;
            //        case "equal":
            //            sb.Append(" and AGE = @AGE ");
            //            break;
            //        default:
            //            break;
            //    }

            //    ht.Add("@AGE", AGE);
            //}
            //if (START_DT != "")
            //{
            //    switch (start_dt_where)
            //    {
            //        case "greater":
            //            sb.Append(" and a.START_DT > CONVERT(datetime,@START_DT) ");
            //            break;
            //        case "less":
            //            sb.Append(" and a.START_DT < CONVERT(datetime,@START_DT) ");
            //            break;
            //        case "equal":
            //            sb.Append(" and a.START_DT = CONVERT(datetime,@START_DT) ");
            //            break;
            //        default:
            //            break;
            //    }

            //    ht.Add("@START_DT", START_DT);
            //}
            //if (work_year != "")
            //{
            //    switch (work_year_where)
            //    {
            //        case "greater":
            //            sb.Append(" and WORK_YEARS > @work_year ");
            //            break;
            //        case "less":
            //            sb.Append(" and WORK_YEARS < @work_year ");
            //            break;
            //        case "equal":
            //            sb.Append(" and WORK_YEARS = @work_year  ");
            //            break;
            //        default:
            //            break;
            //    }
            //    ht.Add("@work_year", work_year);
            //}

    //        return dbConn.Query(sb, ht);
    //    }
    //    catch (Exception)
    //    {

    //        throw;
    //    }
    //}
    internal void addData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" INSERT INTO TB_S_M_SALARY_REPORT_H (");
            sb.Append(" SALARY_DT, SALARY_TYPE, DATA_YM, SALARY_ID_SEQ,");
            sb.Append(" SALARY_ID, SALARY_NAME, IS_PLUS, IS_TAX,");
            sb.Append(" CREATED_BY, CREATED_DT, UPDATED_BY, UPDATED_DT, FUNC_ID)");
            sb.Append(" SELECT");
            sb.Append(" S.SALARY_DT, S.SALARY_TYPE, S.DATA_YM,");
            sb.Append(" ROW_NUMBER() over(PARTITION BY S.IS_PLUS ORDER BY S.SALARY_DT, S.SALARY_TYPE, S.DATA_YM, S.IS_PLUS, S.SALARY_ID) SALARY_ID_SEQ,");
            sb.Append(" S.SALARY_ID, S.SALARY_NAME, S.IS_PLUS, S.IS_TAX,");
            sb.Append(" @login_emp_id, GETDATE(), @login_emp_idID, GETDATE(), 'FB2SC530' FUNC_ID");
            sb.Append(" FROM(");
            sb.Append(" SELECT  SALARY_DT, SALARY_TYPE, DATA_YM, SALARY_ID, SALARY_NAME, IS_PLUS, IS_TAX, COUNT(*) CNT");
            sb.Append(" FROM TB_S_M_SALARY_PAY");
            sb.Append(" WHERE P.SALARY_DT BETWEEN @DATA_YMS AND @DATA_YME");
            sb.Append(" ");
            sb.Append(" ");
            sb.Append(" GROUP BY SALARY_DT, SALARY_TYPE, DATA_YM, SALARY_ID, SALARY_NAME, IS_PLUS, IS_TAX) S");
            sb.Append(" ORDER BY S.SALARY_DT, S.SALARY_TYPE, S.DATA_YM, S.IS_PLUS, SALARY_ID_SEQ");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@EMP_NAME", EMP_NAME);
            ht.Add("@DEPT_NO", DEPT_NO);
            //ht.Add("@DEPT_NAME", DEPT_NAME);
            //ht.Add("@LEVEL_CD", LEVEL_CD);
            //ht.Add("@PJOB_DESC", PJOB_DESC);
            //ht.Add("@DOC_NO", DOC_NO);
            //ht.Add("@START_DT", START_DT);
            //ht.Add("@JUDGEMENT_TYPE", JUDGEMENT_TYPE);
            //ht.Add("@REASON_CD", REASON_CD);
            //ht.Add("@FIRST_CNT", FIRST_CNT);
            //ht.Add("@SECOND_CNT", SECOND_CNT);
            //ht.Add("@THIRD_CNT", THIRD_CNT);
            //ht.Add("@IS_FIRE", IS_FIRE);
            ht.Add("@REMARK", REMARK);
            ht.Add("@CREATED_BY", CREATED_BY);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public string deleteData(string login_emp_id)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();

        sb.Append(" Delete From TB_S_M_SALARY_REPORT_H  ");
        sb.Append(" Where CREATED_BY = @login_emp_id");
        sb.Append(" Delete From TB_S_M_SALARY_REPORT_D  ");
        sb.Append(" Where CREATED_BY = @login_emp_id");
        ht.Add("@login_emp_id", login_emp_id);
        dbConn.ExecuteT(sb, ht, true);

        return "0";
    }

}