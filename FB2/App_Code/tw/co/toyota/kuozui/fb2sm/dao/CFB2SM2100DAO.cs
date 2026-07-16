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
/// CFB2SM2100DAO 的摘要描述
/// </summary>
public class CFB2SM2100DAO : BaseDAO
{
    public string DATA_YEAR { get; set; }
    public string EXCEPTION_STATUS { get; set; }
    public string EXECUTIVE_STATUS { get; set; }
    public string PROMOTION_TYPE { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_DT { get; set; }
    public string DIV_FULL_DEPT_NAME { get; set; }
    public string LEVEL_PAY_OLD { get; set; }
    public string ABILITY_PAY_OLD { get; set; }
    public string PJOB_PAY_OLD { get; set; }
    public string PROFESSION_PAY_OLD { get; set; }
    public string APPROVE_DT { get; set; }
    public string APPROVE_BY { get; set; }
    public string LEVEL_PAY_NEW { get; set; }
    public string ABILITY_PAY_NEW { get; set; }
    public string PJOB_PAY_NEW { get; set; }
    public string PROFESSION_PAY_NEW { get; set; }
    public string ASSESS_SCORE_3 { get; set; }
    public string ASSESS_SCORE_5 { get; set; }
    public string ASSESS_SCORE_4 { get; set; }
    public string DATA_SEQ { get; set; }
    public string GENERATE_DT { get; set; }
    public string EXECUTIVE_DT { get; set; }
    public string CREATED_BY { get; set; }
    public string UPDATED_BY { get; set; }
    public string FUNC_ID { get; set; }
    public string ws_cd { get; set; }
    public string emp_chg_cd { get; set; }
    public string level_cd_new { get; set; }
    public string level_cd { get; set; }
    public string emp_name { get; set; }
    public string emp_id { get; set; }
    public string LEVEL_CD_NEW { get; set; }
    public string GRADE_CD_NEW { get; set; }
    public string PJOB_CD_NEW { get; set; }
    public string CHG_STATUS { get; set; }
    public string PROCESS_STATUS { get; set; }
    public string FINIAL_CHG_DT { get; set; }
    public string EMP_ID { get; set; }
    public string PJOB_DESC_NEW { get; set; }
    public string ASSESS_SCORE_2 { get; set; }
    public string ASSESS_SCORE_1 { get; set; }
    public string EMP_CHG_CD { get; set; }
    public string LEVEL_WORK_YEARS { get; set; }
    public string PJOB_DESC { get; set; }
    public string PJOB_CD { get; set; }
    public string GRADE_CD { get; set; }
    public string LEVEL_CD { get; set; }
    public string WORK_YEARS { get; set; }
    public string WS_CD { get; set; }
    public string EMP_NAME { get; set; }
    public string DEPT_NAME { get; set; }
    public string DEPT_NO { get; set; }
    public string NOTICE_DT { get; set; }
    public string NOTICE_BY { get; set; }
    public string RELEASE_DT { get; set; }
    public string PROMOTION_TOTAL { get; set; }
    public string REMARK_DESC { get; set; }

    #region "Initail"
    public DataTable getCommCode(string sys_cd, string main_cd, string is_valid)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select sub_cd ,sub_cd+'-'+sub_desc as sub_desc ");
            sb.AppendLine(" from TB_9_M_COMM_D ");
            sb.AppendLine(" where SYS_CD = @SYS_CD ");
            sb.AppendLine(" and MAIN_CD = @MAIN_CD ");
            ht.Add("@SYS_CD", sys_cd);
            ht.Add("@MAIN_CD", main_cd);
            if (is_valid != "")
            {
                sb.AppendLine(" and IS_VALID = @IS_VALID ");
                ht.Add("@IS_VALID", is_valid);
            }

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getLEVEL_CD()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" Select LEVEL_CD From TB_H_M_LEVEL ");
            sb.AppendLine(" where START_DT < = GETDATE() and END_DT> = GETDATE() ");

            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getGRADE_CD(string level_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select GRADE_CD from TB_H_M_LEVEL_GRADE ");
            sb.AppendLine(" where IS_VALID='Y' ");
            sb.AppendLine(" and  LEVEL_CD=@LEVEL_CD ");
            ht.Add("@LEVEL_CD", level_cd);
            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }
    public DataTable getLEVEL_CD_NEW()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" Select LEVEL_CD From TB_H_M_LEVEL ");
            sb.AppendLine(" where START_DT < = GETDATE() and END_DT> = GETDATE() ");

            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }
    public DataTable getPJOB_CD_NEW()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" Select PJOB_CD, PJOB_CD + '-' + PJOB_DESC as PJOB_DESC From VW_TB_H_M_PJOB ");

            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getPJOB_CD_NEW(string level_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select PJOB_CD, PJOB_CD + '-' + PJOB_DESC as PJOB_DESC From VW_TB_H_M_PJOB ");
            sb.Append(" where LEVEL_CD=@LEVEL_CD  ");
            ht.Add("@LEVEL_CD", level_cd);
            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }

    #endregion

    #region "Qry"
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string data_year, string first_load)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" SELECT * FROM ( SELECT ROW_NUMBER() OVER ( order by " + sortExpression + " ) AS RowNumber ,  ");
            sb.AppendLine(" P.DATA_YEAR, P.DATA_SEQ, convert(char(10),P.GENERATE_DT,111) GENERATE_DT ");
            sb.AppendLine(" ,P.PROCESS_STATUS, P.PROCESS_STATUS +'-'+ D.SUB_DESC as PROCESS_STATUS_DESC ");
            sb.AppendLine(" ,convert(char(10),P.NOTICE_DT,111) NOTICE_DT ");
            sb.AppendLine(" ,convert(char(10),P.APPROVE_DT,111) APPROVE_DT ");
            sb.AppendLine(" ,convert(char(10),P.RELEASE_DT,111) RELEASE_DT ");
            sb.AppendLine(" ,convert(char(10),P.EXECUTIVE_DT,111) EXECUTIVE_DT ");
            sb.AppendLine(" from TB_S_M_PROMOTION_H P ");
            sb.AppendLine(" left join TB_9_M_COMM_D D on D.SYS_CD='SA' and D.MAIN_CD='PROCESS_STATUS' and P.PROCESS_STATUS = D.SUB_CD ");
            sb.AppendLine(" where P.PROMOTION_TYPE='5' ");
            if (data_year == "")
            {
                if (first_load == "Y")
                {
                    DateTime CurrTime = DateTime.Now;
                    sb.AppendLine(" and (DATA_YEAR >= @DATA_YEAR-5) ");
                    ht.Add("@DATA_YEAR", CurrTime.Year);
                }

            }
            //年度:
            if (data_year != "")
            {
                sb.AppendLine(" and DATA_YEAR = @DATA_YEAR ");
                ht.Add("@DATA_YEAR", data_year);
            }
            sb.AppendLine(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.AppendLine(" AND CAST(@startRowIndex+@maximumRows as varchar)");
            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public int getCount(int startRowIndex, int maximumRows, string data_year, string first_load)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" Select COUNT(*) total_record ");
            sb.AppendLine(" from TB_S_M_PROMOTION_H where PROMOTION_TYPE='5' ");

            if (data_year == "")
            {
                if (first_load == "Y")
                {
                    DateTime CurrTime = DateTime.Now;
                    sb.AppendLine(" and (DATA_YEAR >= @DATA_YEAR-5) ");
                    ht.Add("@DATA_YEAR", CurrTime.Year);
                }
            }

            if (data_year != "")
            {
                sb.AppendLine(" and DATA_YEAR = @DATA_YEAR ");
                ht.Add("@DATA_YEAR", data_year);
            }

            int t = 0;
            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                t = (int)dt.Rows[0]["total_record"];
            }
            return t;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void deletePromotion_H(string p1, string p2)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("delete from  TB_S_M_PROMOTION_H");
            sb.AppendLine(" where DATA_YEAR=@DATA_YEAR and DATA_SEQ = @DATA_SEQ");
            sb.AppendLine(" and PROMOTION_TYPE='5' ");
            ht.Add("@DATA_YEAR", p1);
            ht.Add("@DATA_SEQ", p2);
            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {

            throw;
        }
    }
    public void deletePromotion_TXN(string p1, string p2)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("delete from  TB_S_M_PROMOTION_TXN");
            sb.AppendLine(" where DATA_YEAR=@DATA_YEAR and DATA_SEQ = @DATA_SEQ");
            sb.AppendLine(" and PROMOTION_TYPE='5' ");
            ht.Add("@DATA_YEAR", p1);
            ht.Add("@DATA_SEQ", p2);
            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {

            throw;
        }
    }
    public void deletePromotion(string p1, string p2)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" delete from  TB_S_M_PROMOTION ");
            sb.AppendLine(" where DATA_YEAR=@DATA_YEAR ");
            sb.AppendLine(" and PROMOTION_TYPE='5' ");
            ht.Add("@DATA_YEAR", p1);
            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {

            throw;
        }
    }

    public void updateData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" update TB_S_M_PROMOTION_H set EXECUTIVE_DT = @EXECUTIVE_DT, ");
            sb.AppendLine(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID ");
            sb.AppendLine(" where DATA_YEAR=@DATA_YEAR and DATA_SEQ = @DATA_SEQ and PROMOTION_TYPE='5' ");
            ht.Add("@DATA_YEAR", DATA_YEAR);
            ht.Add("@DATA_SEQ", DATA_SEQ);

            ht.Add("@EXECUTIVE_DT", EXECUTIVE_DT);

            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SM210");

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }
    public void addData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" insert into TB_S_M_PROMOTION_H (DATA_YEAR,DATA_SEQ, ");
            sb.AppendLine(" PROMOTION_TYPE,PROCESS_STATUS,PROMOTION_TOTAL,EXECUTIVE_DT, ");
            sb.AppendLine(" CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID) ");
            sb.AppendLine(" values (@DATA_YEAR,@DATA_SEQ,'5','N','0',@EXECUTIVE_DT, ");
            sb.AppendLine(" @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID) ");

            ht.Add("@DATA_YEAR", DATA_YEAR);
            ht.Add("@DATA_SEQ", DATA_SEQ);
            ht.Add("@EXECUTIVE_DT", EXECUTIVE_DT);
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SM210");

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable dtexit()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("	select DATA_YEAR, DATA_SEQ from TB_S_M_PROMOTION_H where DATA_YEAR = @DATA_YEAR and DATA_SEQ = @DATA_SEQ and PROMOTION_TYPE = '5' ");
            ht.Add("@DATA_YEAR", DATA_YEAR);
            ht.Add("@DATA_SEQ", DATA_SEQ);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public bool checkYearNoSeq()
    {
        try
        {
            bool isYearNoSeq = true;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("	select count(1) total from TB_S_M_PROMOTION_H where DATA_YEAR = @DATA_YEAR and PROMOTION_TYPE='5' ");
            ht.Add("@DATA_YEAR", DATA_YEAR);
            DataTable dt = dbConn.Query(sb, ht);
            if (Convert.ToInt32(dt.Rows[0]["total"]) > 0)
                isYearNoSeq = false;
            return isYearNoSeq;
        }
        catch
        {
            throw;
        }
    }
    //取得現在晉昇回數
    public DataTable getdata_seq(int current_year)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" select * from(select  MAX(DATA_SEQ)+1 as NEW_DATA_SEQ from TB_S_M_PROMOTION_H where DATA_YEAR= @DATA_YEAR and PROMOTION_TYPE = '5')a ");
            sb.AppendLine(" where NEW_DATA_SEQ is not null ");
            ht.Add("@DATA_YEAR", current_year);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //檢核「是否已核可」(核可狀態==Y)，顯示訊息提示視窗「已核可無法刪除」
    public DataTable getGenerateDT_Gp(string p1, string p2)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" Select COUNT(*) total_record_Gp  from ( ");
            sb.AppendLine(" select PROCESS_STATUS from TB_S_M_PROMOTION_H where PROCESS_STATUS = 'Y' ");
            sb.AppendLine(" and DATA_YEAR = @DATA_YEAR and DATA_SEQ = @DATA_SEQ and PROMOTION_TYPE='5' ");
            ht.Add("@DATA_YEAR", p1);
            ht.Add("@DATA_SEQ", p2);
            sb.AppendLine(" )a ");


            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //public DataTable getEMP_CHG_CD()
    //{
    //    try
    //    {
    //        StringBuilder sb = new StringBuilder();
    //        Hashtable ht = new Hashtable();
    //        sb.AppendLine("Select SUB_LEAVE_CD, SUB_LEAVE_CD + '-' + SUB_LEAVE_DESC  as SUB_LEAVE_DESC from TB_D_M_LEAVE_TYPE_D");

    //        return dbConn.Query(sb, ht);

    //    }
    //    catch (Exception)
    //    {

    //        throw;
    //    }
    //}

    //public DataTable getWS_CD()
    //{
    //    try
    //    {
    //        StringBuilder sb = new StringBuilder();
    //        Hashtable ht = new Hashtable();
    //        sb.AppendLine("Select SUB_LEAVE_CD, SUB_LEAVE_CD + '-' + SUB_LEAVE_DESC  as SUB_LEAVE_DESC from TB_D_M_LEAVE_TYPE_D");

    //        return dbConn.Query(sb, ht);

    //    }
    //    catch (Exception)
    //    {

    //        throw;
    //    }
    //}




    //public DataTable getPromotionHDT(string data_year, string data_seq)
    //{
    //    try
    //    {

    //        StringBuilder sb = new StringBuilder();
    //        Hashtable ht = new Hashtable();

    //        sb.AppendLine(" Select COUNT(*) total_record from ( ");
    //        sb.AppendLine(" select PROCESS_STATUS from TB_S_M_PROMOTION_H where PROCESS_STATUS = 'Y' ");
    //        sb.AppendLine(" and DATA_YEAR = @DATA_YEAR and DATA_SEQ = @DATA_SEQ");
    //        ht.Add("@DATA_YEAR", data_year);
    //        ht.Add("@DATA_SEQ", data_seq);
    //        sb.AppendLine(" )a ");


    //        return dbConn.Query(sb, ht);
    //    }
    //    catch (Exception)
    //    {
    //        throw;
    //    }
    //}
    #endregion

    #region "Generate & Release"
    public DataTable getGenerateData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select EMP_ID, EMP_NAME, EMP_CHG_CD, LEVEL_CD, GRADE_CD, PJOB_CD, PJOB_DESC,  ");
            sb.AppendLine("	WS_CD, DEPT_NO, DEPT_NAME_20+' '+DEPT_NAME_30+' '+DEPT_NAME_40 as DEPT_NAME, WORK_YEARS, GRADE_CD as GRADE_CD_NEW, WORK_DAYS, ");
            sb.AppendLine(" RECENT_LEVEL_WORK_DAYS + DATEDIFF(DAY,getdate(),  CONVERT(varchar, YEAR(GETDATE()))  +'/12/31') as LEVEL_WORK_DAYS_toEnd, ");
            sb.AppendLine("	LEVEL_CD as LEVEL_CD_NEW, PJOB_CD as PJOB_CD_NEW, PJOB_DESC as PJOB_DESC_NEW ");
            sb.AppendLine("	from VW_H_EMP_DATA where LEVEL_CD ='5A' and ( GRADE_CD='2' or GRADE_CD='3' or GRADE_CD='4' or GRADE_CD='5' or GRADE_CD='6') ");
            sb.AppendLine("	 and RECENT_LEVEL_WORK_DAYS + DATEDIFF(DAY,getdate(),  CONVERT(varchar, YEAR(GETDATE()))  +'/12/31') >= 365  ");
            sb.AppendLine("  and EMP_STATUS = '01' ");
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public void deleteAllDtl(string data_year, string data_seq)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" delete from TB_S_M_PROMOTION_TXN where DATA_YEAR = @DATA_YEAR and DATA_SEQ = @DATA_SEQ and PROMOTION_TYPE='5'");
            ht.Add("@DATA_YEAR", data_year);
            ht.Add("@DATA_SEQ", data_seq);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void addGenerateData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" insert into TB_S_M_PROMOTION_TXN  ");
            sb.AppendLine(" (DATA_YEAR, EMP_ID, EMP_NAME, EMP_CHG_CD, LEVEL_CD, GRADE_CD, PJOB_CD, PJOB_DESC, WS_CD, DEPT_NO, DIV_FULL_DEPT_NAME ");
            sb.AppendLine(" ,FINIAL_CHG_DT, WORK_YEARS, LEVEL_WORK_YEARS ");
            sb.AppendLine(" ,EXCEPTION_STATUS, CHG_STATUS, PROCESS_STATUS, EXECUTIVE_STATUS, APPROVE_BY, APPROVE_DT, LEVEL_CD_NEW               ");
            sb.AppendLine(" ,GRADE_CD_NEW, PJOB_CD_NEW, PJOB_DESC_NEW ");
            sb.AppendLine(" ,DATA_SEQ, PROMOTION_TYPE, ASSESS_SCORE_1, ASSESS_SCORE_2, ASSESS_SCORE_3, ASSESS_SCORE_4, ASSESS_SCORE_5            ");
            sb.AppendLine(" ,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID )                                                               ");
            sb.AppendLine(" values (@DATA_YEAR, @EMP_ID, @EMP_NAME, @EMP_CHG_CD, @LEVEL_CD, @GRADE_CD, @PJOB_CD, @PJOB_DESC, @WS_CD, @DEPT_NO, @DIV_FULL_DEPT_NAME ");
            sb.AppendLine("         ,GETDATE(), @WORK_YEARS, @LEVEL_WORK_YEARS ");
            sb.AppendLine("         ,'N', 'I', 'N', 'N', null, null, @LEVEL_CD_NEW ");
            sb.AppendLine("         ,@GRADE_CD_NEW, @PJOB_CD1_NEW, @PJOB_DESC1_NEW ");
            sb.AppendLine("         ,@DATA_SEQ, '5', @ASSESS_SCORE_1,@ASSESS_SCORE_2, null, null, null ");
            sb.AppendLine("         ,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID) ");
            ht.Add("@DATA_YEAR", DATA_YEAR);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@EMP_NAME", EMP_NAME);
            ht.Add("@EMP_CHG_CD", EMP_CHG_CD);
            ht.Add("@LEVEL_CD", LEVEL_CD);
            ht.Add("@GRADE_CD", GRADE_CD);
            ht.Add("@PJOB_CD", PJOB_CD);
            ht.Add("@PJOB_DESC", PJOB_DESC);
            ht.Add("@WS_CD", WS_CD);
            ht.Add("@DEPT_NO", DEPT_NO);
            ht.Add("@DIV_FULL_DEPT_NAME", DIV_FULL_DEPT_NAME);
            ht.Add("@WORK_YEARS", WORK_YEARS);
            ht.Add("@LEVEL_WORK_YEARS", LEVEL_WORK_YEARS);
            ht.Add("@LEVEL_CD_NEW", LEVEL_CD_NEW);
            ht.Add("@GRADE_CD_NEW", GRADE_CD_NEW);
            ht.Add("@PJOB_CD1_NEW", PJOB_CD_NEW);
            ht.Add("@PJOB_DESC1_NEW", PJOB_DESC_NEW);
            ht.Add("@DATA_SEQ", DATA_SEQ);
            ht.Add("@ASSESS_SCORE_1", ASSESS_SCORE_1);
            ht.Add("@ASSESS_SCORE_2", ASSESS_SCORE_2);
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SM210");
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public object updateGenerate_H(string data_year, string data_seq)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" update TB_S_M_PROMOTION_H set  ");
            sb.AppendLine(" PROMOTION_TOTAL = @PROMOTION_TOTAL,GENERATE_DT = GETDATE(), NOTICE_DT = NULL, NOTICE_BY = '', APPROVE_DT = NULL, ");
            sb.AppendLine(" APPROVE_BY = '', PROCESS_STATUS = 'N', UPDATED_BY = @UPDATED_BY, ");
            sb.AppendLine(" UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID ");
            sb.AppendLine(" where DATA_YEAR = @DATA_YEAR and DATA_SEQ = @DATA_SEQ and PROMOTION_TYPE='5' ");
            ht.Add("@DATA_YEAR", data_year);
            ht.Add("@DATA_SEQ", data_seq);
            ht.Add("@PROMOTION_TOTAL", PROMOTION_TOTAL);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SM210");

            return dbConn.ExecuteT(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public object updateRelease_H(string data_year, string data_seq)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" update TB_S_M_PROMOTION_H set  ");
            sb.AppendLine(" NOTICE_DT = GETDATE(), NOTICE_BY = @NOTICE_BY, APPROVE_DT = NULL,APPROVE_BY = b.DIRECT_HEAD_EMP_ID, ");
            sb.AppendLine(" PROCESS_STATUS = 'N', UPDATED_BY = @UPDATED_BY, ");
            sb.AppendLine(" UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID  ");
            sb.AppendLine(" from TB_H_M_EMP b ");
            //sb.AppendLine(" left join TB_H_R_HEAD_DEPT c on b.DEPT_NO = c.MNG_DEPT_NO and b.DIRECT_HEAD_EMP_ID = c.EMP_ID ");
            sb.AppendLine(" where DATA_YEAR = @DATA_YEAR and DATA_SEQ = @DATA_SEQ and b.EMP_ID = @NOTICE_BY and PROMOTION_TYPE = '5' ");

            ht.Add("@DATA_YEAR", data_year);
            ht.Add("@DATA_SEQ", data_seq);
            ht.Add("@NOTICE_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SM210");

            return dbConn.ExecuteT(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public object updatePROMOTION_H(string data_year, string data_seq)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" update TB_S_M_PROMOTION_H set  ");
            sb.AppendLine(" PROMOTION_TOTAL = @PROMOTION_TOTAL, NOTICE_DT = NULL, NOTICE_BY = '', APPROVE_DT = NULL, ");
            sb.AppendLine(" APPROVE_BY = '', PROCESS_STATUS = 'N', UPDATED_BY = @UPDATED_BY, ");
            sb.AppendLine(" UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID ");
            sb.AppendLine(" where DATA_YEAR = @DATA_YEAR and DATA_SEQ = @DATA_SEQ and PROMOTION_TYPE='5' ");
            ht.Add("@DATA_YEAR", data_year);
            ht.Add("@DATA_SEQ", data_seq);
            ht.Add("@PROMOTION_TOTAL", PROMOTION_TOTAL);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SM210");

            return dbConn.ExecuteT(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion

    #region "Dtl"
    public DataTable getHeader(string data_year, string data_seq)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();

        sb.AppendLine(" select DATA_YEAR, DATA_SEQ, CONVERT(CHAR(19), NOTICE_DT, 111) as NOTICE_DT, PROCESS_STATUS,");
        sb.AppendLine(" case PROCESS_STATUS when 'N' then '未核可' when 'Y' then '已核可' else '' end as PROCESS_STATUS_DESC, REMARK_DESC, ");
        sb.AppendLine(" CONVERT(CHAR(19), EXECUTIVE_DT, 111) as EXECUTIVE_DT ");
        sb.AppendLine(" from TB_S_M_PROMOTION_H ");
        sb.AppendLine(" where DATA_YEAR = @DATA_YEAR and DATA_SEQ = @DATA_SEQ and PROMOTION_TYPE='5' ");

        ht.Add("@DATA_YEAR", data_year);
        ht.Add("@DATA_SEQ", data_seq);
        return dbConn.Query(sb, ht);
    }
    public DataTable getDtlData(int startRowIndex, int maximumRows, string sortExpression, string dept_no, string emp_id, string level_cd, string emp_name, string level_cd_new, string emp_chg_cd, string ws_cd, string data_year, string data_seq)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("	select NO,CHG_STATUS,CHG_STATUS_DESC,EXCEPTION_STATUS,DEPT_NO,EMP_ID,EMP_NAME,WS_CD,WORK_YEARS,LEVEL_CD, 	 ");
            sb.AppendLine("	       GRADE_CD,PJOB_CD,LEVEL_WORK_YEARS,LEVEL_CD_NEW,	 ");
            sb.AppendLine("	       GRADE_CD_NEW,PJOB_CD_NEW,PJOB_CD_NEW_DESC,EMP_CHG_CD,ASSESS_SCORE_1,ASSESS_SCORE_2, ");
            sb.AppendLine("	       EXECUTIVE_STATUS, PROCESS_STATUS, FINIAL_CHG_DT, DATA_YEAR	 ");
            sb.AppendLine("	from (	select ");
            sb.AppendLine("	             ROW_NUMBER() OVER ( order by " + sortExpression + " ) AS NO ,	 ");
            sb.AppendLine("              CHG_STATUS, CHG_STATUS +'-'+ d.SUB_DESC as CHG_STATUS_DESC, ");
            sb.AppendLine("	              case EXCEPTION_STATUS when 'Y' then 'V' else '' end as EXCEPTION_STATUS ,DEPT_NO + '-' +DIV_FULL_DEPT_NAME as DEPT_NO , ");
            sb.AppendLine("	              EMP_ID,EMP_NAME,WS_CD,WORK_YEARS,LEVEL_CD ,	 ");
            sb.AppendLine("	              GRADE_CD,PJOB_CD+'-'+PJOB_DESC as PJOB_CD,LEVEL_WORK_YEARS,LEVEL_CD_NEW, ");
            sb.AppendLine("	              GRADE_CD_NEW,PJOB_CD_NEW,PJOB_CD_NEW+'-'+PJOB_DESC_NEW as PJOB_CD_NEW_DESC,b.SUB_DESC as EMP_CHG_CD,ASSESS_SCORE_1,	 ");
            sb.AppendLine("	              ASSESS_SCORE_2, EXECUTIVE_STATUS, PROCESS_STATUS, FINIAL_CHG_DT, DATA_YEAR ");
            sb.AppendLine("	         from TB_S_M_PROMOTION_TXN a ");
            sb.AppendLine("          left join TB_9_M_COMM_D b on  b.SYS_CD='HB' and b.MAIN_CD='EMP_CHG_CD' and b.SUB_CD = a.EMP_CHG_CD ");
            sb.AppendLine("          left join TB_9_M_COMM_D D on  d.SYS_CD='SM' and d.MAIN_CD='CHG_STATUS' and d.SUB_CD = a.CHG_STATUS ");
            sb.AppendLine("	        where DATA_YEAR = @DATA_YEAR and DATA_SEQ = @DATA_SEQ and PROMOTION_TYPE='5'  ");
            ht.Add("@DATA_YEAR", data_year);
            ht.Add("@DATA_SEQ", data_seq);
            //部門:
            if (dept_no != "")
            {
                sb.AppendLine(" and DEPT_NO like @DEPT_NO ");
                ht.Add("@DEPT_NO", "%" + dept_no + "%");
            }

            //工號:
            if (emp_id != "")
            {
                sb.AppendLine(" and EMP_ID like @EMP_ID ");
                ht.Add("@EMP_ID", "%" + emp_id + "%");
            }

            //原資格:
            if (level_cd != "")
            {
                sb.AppendLine(" and LEVEL_CD = @LEVEL_CD ");
                ht.Add("@LEVEL_CD", level_cd);
            }

            //姓名:
            if (emp_name != "")
            {
                sb.AppendLine(" and EMP_NAME LIKE @EMP_NAME ");
                ht.Add("@EMP_NAME", "%" + emp_name.Trim() + "%");
            }
            //晉昇資格:
            if (level_cd_new != "")
            {
                sb.AppendLine(" and LEVEL_CD_NEW = @LEVEL_CD_NEW ");
                ht.Add("@LEVEL_CD_NEW", level_cd_new);
            }

            //在職區分:
            if (emp_chg_cd != "")
            {
                sb.AppendLine(" and EMP_CHG_CD = @EMP_CHG_CD ");
                ht.Add("@EMP_CHG_CD", emp_chg_cd);
            }

            //職種:
            if (ws_cd != "")
            {
                sb.AppendLine(" and WS_CD = @WS_CD ");
                ht.Add("@WS_CD", ws_cd);
            }


            sb.AppendLine(" )god_data where NO between CAST(@startRowIndex+1 as varchar) ");
            sb.AppendLine(" AND CAST(@startRowIndex+@maximumRows as varchar)");




            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public int getDtlCount(int startRowIndex, int maximumRows, string dept_no, string emp_id, string level_cd, string emp_name, string level_cd_new, string emp_chg_cd, string ws_cd, string data_year, string data_seq)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" 	Select COUNT(*) total_record ");
            sb.AppendLine("	      from TB_S_M_PROMOTION_TXN a ");
            sb.AppendLine("          left join TB_9_M_COMM_D b on  b.SYS_CD='HB' and b.MAIN_CD='EMP_CHG_CD' and b.SUB_CD = a.EMP_CHG_CD ");
            sb.AppendLine("          left join TB_9_M_COMM_D D on  d.SYS_CD='SM' and d.MAIN_CD='CHG_STATUS' and d.SUB_CD = a.CHG_STATUS ");
            sb.AppendLine("	     where DATA_YEAR = @DATA_YEAR and DATA_SEQ = @DATA_SEQ and PROMOTION_TYPE='5'  ");
            ht.Add("@DATA_YEAR", data_year);
            ht.Add("@DATA_SEQ", data_seq);
            //部門:
            if (dept_no != "")
            {
                sb.AppendLine(" and a.DEPT_NO like @DEPT_NO ");
                ht.Add("@DEPT_NO", "%" + dept_no + "%");
            }

            //工號:
            if (emp_id != "")
            {
                sb.AppendLine(" and EMP_ID like @EMP_ID ");
                ht.Add("@EMP_ID", "%" + emp_id + "%");
            }

            //原資格:
            if (level_cd != "" )
            {
                sb.AppendLine(" and LEVEL_CD = @LEVEL_CD ");
                ht.Add("@LEVEL_CD", level_cd);
            }

            //姓名:
            if (emp_name != "")
            {
                sb.AppendLine(" and EMP_NAME LIKE @EMP_NAME ");
                ht.Add("@EMP_NAME", "%" + emp_name.Trim() + "%");
            }
            //晉昇資格:
            if (level_cd_new != "")
            {
                sb.AppendLine(" and LEVEL_CD_NEW = @LEVEL_CD_NEW ");
                ht.Add("@LEVEL_CD_NEW", level_cd_new);
            }

            //在職區分:
            if (emp_chg_cd != "")
            {
                sb.AppendLine(" and EMP_CHG_CD = @EMP_CHG_CD ");
                ht.Add("@EMP_CHG_CD", emp_chg_cd);
            }

            //職種:
            if (ws_cd != "" )
            {
                sb.AppendLine(" and WS_CD = @WS_CD ");
                ht.Add("@WS_CD", ws_cd);
            }

            int t = 0;
            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                t = (int)dt.Rows[0]["total_record"];
            }
            return t;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable checkCD_NEW()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" Select count(*) total from TB_H_M_LEVEL_GRADE where IS_VALID='Y' and  LEVEL_CD=@LEVEL_CD_NEW and GRADE_CD=@GRADE_CD_NEW");
            ht.Add("@LEVEL_CD_NEW", LEVEL_CD_NEW);
            ht.Add("@GRADE_CD_NEW", GRADE_CD_NEW);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable checkJOB_NEW()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" Select count(*) total from TB_H_M_PJOB where ");
            sb.AppendLine(" START_DT < = GETDATE() and END_DT> = GETDATE() and PJOB_CD=@PJOB_CD_NEW and LEVEL_CD=@LEVEL_CD_NEW ");
            ht.Add("@LEVEL_CD_NEW", LEVEL_CD_NEW);
            ht.Add("@PJOB_CD_NEW", PJOB_CD_NEW);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable checkEMP_IDexist()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" Select EMP_ID  From TB_S_M_PROMOTION_TXN where EMP_ID = @EMP_ID and DATA_YEAR = @DATA_YEAR ");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@DATA_YEAR", DATA_YEAR);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable getLEVEL_CD_SEQ(string level_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select ORDER_SEQ from VW_TB_H_M_LEVEL Where LEVEL_CD=@LEVEL_CD ");
            ht.Add("@LEVEL_CD", level_cd);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable getEMP_ID_data(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select EMP_ID, DEPT_NO,DEPT_NAME_20+ ''+DEPT_NAME_30+' '+ DEPT_NAME_40 as DEPT_NAME, DEPT_NO + '-' + DEPT_NAME_20+ ' '+DEPT_NAME_30+' '+DEPT_NAME_40 as DEPT_NO1,GRADE_CD, ");
            sb.AppendLine("        EMP_NAME, WS_CD, WORK_YEARS, WORK_DAYS, LEVEL_CD, GRADE_CD, PJOB_CD, PJOB_DESC, PJOB_CD+'-'+ PJOB_DESC as PJOB_CD1, ");
            sb.AppendLine("        EMP_CHG_CD ,EMP_CHG_DESC,EMP_CHG_CD+'-'+ EMP_CHG_DESC  as EMP_CHG_CD1,  ");
            sb.AppendLine("        CONVERT(Decimal(6,1),Round(RECENT_LEVEL_WORK_DAYS/365.0,1)) as LEVEL_WORK_YEARS, EMP_ID, ");
            sb.AppendLine("        DATEDIFF(DAY,RECENT_LEVEL_DT,  CONVERT(varchar, YEAR(GETDATE()))  +'/12/31') +1 as LEVEL_WORK_DAYS_toEnd ");    
            sb.AppendLine("   from VW_H_EMP_DATA where EMP_ID= @EMP_ID ");
            ht.Add("@EMP_ID", emp_id);
            return dbConn.Query(sb, ht);   //convert(Decimal(5,3),Round( 150.03,1))
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void addPromotiondtl()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" insert into TB_S_M_PROMOTION_TXN values ( @DATA_YEAR, @EMP_ID, @EMP_NAME, @EMP_CHG_CD, ");
            sb.AppendLine(" @LEVEL_CD, @GRADE_CD, @PJOB_CD, @PJOB_DESC, @WS_CD, @DEPT_NO, @DIV_FULL_DEPT_NAME, GETDATE(), ");
            sb.AppendLine(" @WORK_YEARS, @LEVEL_WORK_YEARS, 'N', 'I', 'N', 'N', null, null, @LEVEL_CD_NEW, ");
            sb.AppendLine(" @GRADE_CD_NEW, @PJOB1_CD_NEW, @PJOB_DESC1_NEW, @DATA_SEQ,'5', @ASSESS_SCORE_1, ");
            sb.AppendLine(" @ASSESS_SCORE_2, null, null, null, @CREATED_BY, GETDATE(), @UPDATED_BY, GETDATE(), @FUNC_ID )");
            ht.Add("@DATA_YEAR", DATA_YEAR);
            ht.Add("@DATA_SEQ", DATA_SEQ);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@DEPT_NO", DEPT_NO);
            ht.Add("@DIV_FULL_DEPT_NAME", DIV_FULL_DEPT_NAME);
            ht.Add("@EMP_NAME", EMP_NAME);
            ht.Add("@WS_CD", WS_CD);
            ht.Add("@WORK_YEARS", WORK_YEARS);
            ht.Add("@LEVEL_CD", LEVEL_CD);
            ht.Add("@GRADE_CD", GRADE_CD);
            ht.Add("@PJOB_CD", PJOB_CD);
            ht.Add("@PJOB_DESC", PJOB_DESC);
            ht.Add("@LEVEL_WORK_YEARS", LEVEL_WORK_YEARS);
            ht.Add("@EMP_CHG_CD", EMP_CHG_CD);
            ht.Add("@ASSESS_SCORE_1", ASSESS_SCORE_1);
            ht.Add("@ASSESS_SCORE_2", ASSESS_SCORE_2);
            ht.Add("@LEVEL_CD_NEW", LEVEL_CD_NEW);
            ht.Add("@GRADE_CD_NEW", GRADE_CD_NEW);
            ht.Add("@PJOB1_CD_NEW", PJOB_CD_NEW);
            ht.Add("@PJOB_DESC1_NEW", PJOB_DESC_NEW);
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SM210");
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void deletePromotionDtlYtoN(string data_year, string emp_id, string process_status, string data_seq)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" update TB_S_M_PROMOTION_TXN set ");
            sb.AppendLine(" CHG_STATUS = 'D', ");
            sb.AppendLine(" UPDATED_BY = @UPDATED_BY, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID, ");
            sb.AppendLine(" PROCESS_STATUS=@PROCESS_STATUS, FINIAL_CHG_DT = GETDATE()");
            sb.AppendLine(" where DATA_YEAR = @DATA_YEAR and EMP_ID= @EMP_ID and DATA_SEQ = @DATA_SEQ and PROMOTION_TYPE='5' ");

            ht.Add("@DATA_YEAR", data_year);
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@DATA_SEQ", data_seq);
            if (process_status == "Y")
                ht.Add("@PROCESS_STATUS", "N");
            else
                ht.Add("@PROCESS_STATUS", process_status);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SM210");

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void updatePromoyionDtlTXN()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" update TB_S_M_PROMOTION_TXN set ");
            sb.AppendLine(" LEVEL_CD_NEW = @LEVEL_CD_NEW, GRADE_CD_NEW = @GRADE_CD_NEW, ");
            sb.AppendLine(" PJOB_CD_NEW = @PJOB_CD_NEW, PJOB_DESC_NEW= @PJOB_DESC_NEW,");
            sb.AppendLine(" CHG_STATUS = 'U', ");
            sb.AppendLine(" FINIAL_CHG_DT = GETDATE(), UPDATED_BY = @UPDATED_BY,  ");
            sb.AppendLine(" UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID ");
            sb.AppendLine(" where DATA_YEAR = @DATA_YEAR and DATA_SEQ = @DATA_SEQ and EMP_ID= @EMP_ID and PROMOTION_TYPE='5' ");

            ht.Add("@DATA_YEAR", DATA_YEAR);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@DATA_SEQ", DATA_SEQ);
            ht.Add("@LEVEL_CD_NEW", LEVEL_CD_NEW);
            ht.Add("@GRADE_CD_NEW", GRADE_CD_NEW);
            ht.Add("@PJOB_CD_NEW", PJOB_CD_NEW);
            ht.Add("@PJOB_DESC_NEW", PJOB_DESC_NEW);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }
    public void deletePromotionDtl(string data_year, string emp_id, string data_seq)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" update TB_S_M_PROMOTION_TXN set ");
            sb.AppendLine(" CHG_STATUS = 'D', ");
            sb.AppendLine(" UPDATED_BY = @UPDATED_BY, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID,");
            sb.AppendLine(" FINIAL_CHG_DT = GETDATE()");
            sb.AppendLine(" where DATA_YEAR = @DATA_YEAR and DATA_SEQ = @DATA_SEQ and EMP_ID= @EMP_ID and PROMOTION_TYPE='5' ");

            ht.Add("@DATA_YEAR", data_year);
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@DATA_SEQ", data_seq);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable getPROMOTION_TOTAL(string data_year, string data_seq)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("	select EMP_ID from TB_S_M_PROMOTION_TXN ");
            sb.AppendLine("	where DATA_SEQ = @DATA_SEQ and DATA_YEAR = @DATA_YEAR ");
            sb.AppendLine("	and PROMOTION_TYPE = '5' and CHG_STATUS <> 'D' ");

            ht.Add("@DATA_YEAR", data_year);
            ht.Add("@DATA_SEQ", data_seq);


            return dbConn.QueryT(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    //取得 前一回能力考核成績、前二回能力考核成績	
    public DataTable get2score(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("	select EMP_ID, SCORE_1H ");
            sb.AppendLine("	  from TB_S_M_ASSESS ");
            sb.AppendLine("	 where EMP_ID=@EMP_ID ");
            sb.AppendLine("	   and ASSESS_YEAR in ( ");
            sb.AppendLine("         select TOP 2 ASSESS_YEAR from TB_S_M_ASSESS where EMP_ID = @EMP_ID and len(SCORE_1H) > 0 ORDER BY ASSESS_YEAR desc ");
            sb.AppendLine("      )  order by ASSESS_YEAR  desc ");
            ht.Add("@EMP_ID", emp_id);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    #endregion
}



