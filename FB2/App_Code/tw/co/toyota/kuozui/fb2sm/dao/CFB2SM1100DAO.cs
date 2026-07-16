using FB2.tw.co.toyota.kuozui.dao;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

/// <summary>
/// CFB2SM1100DAO 的摘要描述
/// </summary>
public class CFB2SM1100DAO : BaseDAO
{
    public string DATA_YEAR_gv { get; set; }
    public string DATA_SEQ { get; set; }
    public string GENERATE_DT { get; set; }
    public string EXECUTIVE_DT { get; set; }
    public string CREATED_BY { get; set; }
    public string UPDATED_BY { get; set; }
    public string FUNC_ID { get; set; }
    public string EMP_ID { get; set; }
    public string LEVEL_CD_NEW { get; set; }
    public string GRADE_CD_NEW { get; set; }
    public string PJOB_CD_NEW { get; set; }
    public string DATA_YEAR { get; set; }
    public string DEPT_NO { get; set; }
    public string EMP_NAME { get; set; }
    public string WS_CD { get; set; }
    public string WORK_YEARS { get; set; }
    public string LEVEL_CD { get; set; }
    public string GRADE_CD { get; set; }
    public string PJOB_CD { get; set; }
    public string LEVEL_WORK_YEARS { get; set; }
    public string EMP_CHG_CD { get; set; }
    public string ASSESS_SCORE_1 { get; set; }
    public string ASSESS_SCORE_2 { get; set; }
    public string ASSESS_SCORE_3 { get; set; }
    public string ASSESS_SCORE_4 { get; set; }
    public string ASSESS_SCORE_5 { get; set; }
    public string DEPT_NAME { get; set; }
    public string PJOB_DESC { get; set; }
    public string EMP_CHG_DESC { get; set; }
    public string PJOB_DESC_NEW { get; set; }
    public string PROCESS_STATUS { get; set; }
    public CFB2SM1100DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    #region "Initial"
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
            sb.AppendLine(" where START_DT < = GETDATE() and END_DT> = GETDATE()  ");
            sb.AppendLine(" and LEVEL_CD not in ('5A','RB') ORDER BY ORDER_SEQ ");
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
            sb.Append(" Select PJOB_CD, PJOB_CD + '-' + PJOB_DESC as PJOB_DESC From VW_TB_H_M_PJOB ");
            sb.Append(" where LEVEL_CD<>'5A' and LEVEL_CD<>'RB'  ");
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
            sb.AppendLine(" where P.PROMOTION_TYPE='0' ");
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
            sb.AppendLine(" from TB_S_M_PROMOTION_H where PROMOTION_TYPE='0' ");

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
    //取得現在晉昇回數
    public DataTable getdata_seq(int current_year)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" select * from(select  MAX(DATA_SEQ)+1 as NEW_DATA_SEQ from TB_S_M_PROMOTION_H where DATA_YEAR= @DATA_YEAR and PROMOTION_TYPE = '0')a ");
            sb.AppendLine(" where NEW_DATA_SEQ is not null ");
            ht.Add("@DATA_YEAR", current_year);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void addPromotion()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("insert into TB_S_M_PROMOTION_H (DATA_YEAR,DATA_SEQ,PROMOTION_TYPE,PROCESS_STATUS,PROMOTION_TOTAL,EXECUTIVE_DT,");
            sb.AppendLine(" CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.AppendLine(" values (@DATA_YEAR,@DATA_SEQ,0,'N',0,@EXECUTIVE_DT,");
            sb.AppendLine(" @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@DATA_YEAR", DATA_YEAR_gv);
            ht.Add("@DATA_SEQ", DATA_SEQ);
            ht.Add("@EXECUTIVE_DT", EXECUTIVE_DT);
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
    public void updatePromotion()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" update TB_S_M_PROMOTION_H set EXECUTIVE_DT = @EXECUTIVE_DT, ");
            sb.AppendLine(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID ");
            sb.AppendLine(" where DATA_YEAR=@DATA_YEAR and DATA_SEQ = @DATA_SEQ and PROMOTION_TYPE='0'");
            ht.Add("@DATA_YEAR", DATA_YEAR_gv);
            ht.Add("@DATA_SEQ", DATA_SEQ);

            ht.Add("@EXECUTIVE_DT", EXECUTIVE_DT);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }
    public DataTable checkRepead()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("select DATA_YEAR from TB_S_M_PROMOTION_H ");
            sb.AppendLine(" where DATA_YEAR = @DATA_YEAR ");
            sb.AppendLine(" and DATA_SEQ = @DATA_SEQ");
            sb.AppendLine(" and PROMOTION_TYPE = '0' ");
            ht.Add("@DATA_YEAR", DATA_YEAR_gv);
            ht.Add("@DATA_SEQ", DATA_SEQ);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
    //刪除 晉昇作業主檔
    public void deletePromotion_H(string data_year, string data_seq)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("delete from  TB_S_M_PROMOTION_H");
            sb.AppendLine(" where DATA_YEAR=@DATA_YEAR and DATA_SEQ = @DATA_SEQ and PROMOTION_TYPE = '0' ");
            ht.Add("@DATA_YEAR", data_year);
            ht.Add("@DATA_SEQ", data_seq);
            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {

            throw;
        }
    }
    //刪除 晉昇人員生成檔
    public void deletePromotion_TXN(string data_year, string data_seq)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("delete from  TB_S_M_PROMOTION_TXN");
            sb.AppendLine(" where DATA_YEAR = @DATA_YEAR and DATA_SEQ = @DATA_SEQ and PROMOTION_TYPE = '0' ");
            ht.Add("@DATA_YEAR", data_year);
            ht.Add("@DATA_SEQ", data_seq);
            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {

            throw;
        }
    }
    //刪除 晉昇人員主檔
    public void deletePromotion(string data_year)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" delete from  TB_S_M_PROMOTION ");
            sb.AppendLine(" where DATA_YEAR = @DATA_YEAR ");
            sb.AppendLine(" and PROMOTION_TYPE='0' ");
            ht.Add("@DATA_YEAR", data_year);
            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {

            throw;
        }
    }
    public DataTable getMainEmp_id(string p)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select EMP_ID ,DEPT_NO , DIRECT_HEAD_EMP_ID from VW_H_EMP_DATA where EMP_ID = @EMP_ID");
            ht.Add("@EMP_ID", p);
            return dbConn.Query(sb, ht);
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
            sb.AppendLine(" where DATA_YEAR = @DATA_YEAR and DATA_SEQ = @DATA_SEQ and b.EMP_ID = @NOTICE_BY and PROMOTION_TYPE = '0' ");

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
    public DataTable getGenerateDT_Gp(string data_year, string data_seq)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" Select COUNT(*) total_record_Gp  from ( ");
            sb.AppendLine(" select PROCESS_STATUS from TB_S_M_PROMOTION_H where PROCESS_STATUS = 'Y' ");
            sb.AppendLine(" and DATA_YEAR = @DATA_YEAR and DATA_SEQ = @DATA_SEQ and PROMOTION_TYPE = '0' ");
            ht.Add("@DATA_YEAR", data_year);
            ht.Add("@DATA_SEQ", data_seq);
            sb.AppendLine(" )a ");
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable getdata_seq(string b)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("select  MAX(DATA_SEQ)+1 as NEW_DATA_SEQ from TB_S_M_PROMOTION_H where DATA_YEAR=@DATA_YEAR ");
            ht.Add("@DATA_YEAR", b);
            return dbConn.Query(sb, ht);
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

        sb.AppendLine(" select DATA_YEAR, DATA_SEQ, REPLACE(CONVERT(CHAR(10), NOTICE_DT, 120), '-', '/') NOTICE_DT, ");
        sb.AppendLine(" REPLACE(CONVERT(CHAR(10), EXECUTIVE_DT, 120), '-', '/') EXECUTIVE_DT, REMARK_DESC, ");
        sb.AppendLine(" REPLACE(CONVERT(CHAR(10), EXECUTIVE_DT, 120), '-', '/') EXECUTIVE_DT ");
        sb.AppendLine(" from TB_S_M_PROMOTION_H ");
        sb.AppendLine(" where DATA_YEAR = @DATA_YEAR and DATA_SEQ = @DATA_SEQ and PROMOTION_TYPE='0' ");

        ht.Add("@DATA_YEAR", data_year);
        ht.Add("@DATA_SEQ", data_seq);
        return dbConn.Query(sb, ht);
    }
    public DataTable getDtlData(int startRowIndex, int maximumRows, string sortExpression, string dept_no,
                           string emp_id, string level_cd, string emp_name, string level_cd_new,
                            string emp_chg_cd, string ws_cd, string DATA_YEAR, string DATA_SEQ)
    {
        try
        {

            //if (sortExpression.Contains("REMARK"))
            //{
            //    sortExpression = sortExpression.Replace("REMARK", "a.REMARK");
            //}

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" SELECT NO,EXCEPTION_STATUS,DEPT_NO,EMP_ID,EMP_NAME,WS_CD,WORK_YEARS,LEVEL_CD, ");
            sb.AppendLine("        GRADE_CD,PJOB_CD,PJOB_CD_DESC,LEVEL_WORK_YEARS,LEVEL_CD_NEW,PROCESS_STATUS,FINIAL_CHG_DT,CHG_STATUS,CHG_STATUS_DESC,");
            sb.AppendLine("        GRADE_CD_NEW,PJOB_CD_NEW,PJOB_CD_NEW_DESC,EMP_CHG_CD,ASSESS_SCORE_1,ASSESS_SCORE_2,ASSESS_SCORE_3,");
            sb.AppendLine("        ASSESS_SCORE_4,ASSESS_SCORE_5 ,DATA_YEAR,EXECUTIVE_STATUS");
            sb.AppendLine("	from (	select ");
            sb.AppendLine("	             ROW_NUMBER() OVER ( order by " + sortExpression + " ) AS NO ,	 ");
            sb.AppendLine("              CHG_STATUS, CHG_STATUS +'-'+ d.SUB_DESC as CHG_STATUS_DESC, ");
            sb.AppendLine("	              case EXCEPTION_STATUS when 'Y' then 'V' else '' end as EXCEPTION_STATUS ,DEPT_NO + '-' +DIV_FULL_DEPT_NAME as DEPT_NO , ");
            sb.AppendLine("	              EMP_ID,EMP_NAME,WS_CD,WORK_YEARS,LEVEL_CD ,	 ");
            sb.AppendLine("	              GRADE_CD,PJOB_CD,PJOB_CD+'-'+PJOB_DESC as PJOB_CD_DESC,LEVEL_WORK_YEARS,LEVEL_CD_NEW, ");
            sb.AppendLine("	              GRADE_CD_NEW,PJOB_CD_NEW,PJOB_CD_NEW+'-'+PJOB_DESC_NEW as PJOB_CD_NEW_DESC,b.SUB_DESC as EMP_CHG_CD,ASSESS_SCORE_1,	 ");
            sb.AppendLine("	              ASSESS_SCORE_2,ASSESS_SCORE_3,ASSESS_SCORE_4,ASSESS_SCORE_5, EXECUTIVE_STATUS, PROCESS_STATUS, FINIAL_CHG_DT, DATA_YEAR ");
            sb.AppendLine("	         from TB_S_M_PROMOTION_TXN a ");
            sb.AppendLine("          left join TB_9_M_COMM_D b on  b.SYS_CD='HB' and b.MAIN_CD='EMP_CHG_CD' and b.SUB_CD = a.EMP_CHG_CD ");
            sb.AppendLine("          left join TB_9_M_COMM_D D on  d.SYS_CD='SM' and d.MAIN_CD='CHG_STATUS' and d.SUB_CD = a.CHG_STATUS ");
            sb.AppendLine("	        where DATA_YEAR = @DATA_YEAR and DATA_SEQ = @DATA_SEQ and PROMOTION_TYPE='0'  ");

            ht.Add("@DATA_YEAR", DATA_YEAR);
            ht.Add("@DATA_SEQ", DATA_SEQ);
            //部門:
            if (dept_no != "")
            {
                sb.AppendLine(" and a.DEPT_NO like @DEPT_NO ");
                ht.Add("@DEPT_NO", "%" + dept_no + "%");
            }

            //工號:
            if (emp_id != "")
            {
                sb.AppendLine(" and a.EMP_ID like @EMP_ID ");
                ht.Add("@EMP_ID", "%" + emp_id + "%");
            }

            //原資格:
            if (level_cd != "" && level_cd != "-1")
            {
                sb.AppendLine(" and a.LEVEL_CD = @LEVEL_CD ");
                ht.Add("@LEVEL_CD", level_cd);
            }

            //姓名:
            if (emp_name != "")
            {
                sb.AppendLine(" and a.EMP_NAME LIKE @EMP_NAME ");
                ht.Add("@EMP_NAME", "%" + emp_name.Trim() + "%");
            }
            //晉昇資格:
            if (level_cd_new != "" && level_cd_new != "-1")
            {
                sb.AppendLine(" and a.LEVEL_CD_NEW = @LEVEL_CD_NEW ");
                ht.Add("@LEVEL_CD_NEW", level_cd_new);
            }

            //在職區分:
            if (emp_chg_cd != "" && emp_chg_cd != "-1")
            {
                sb.AppendLine(" and a.EMP_CHG_CD = @EMP_CHG_CD ");
                ht.Add("@EMP_CHG_CD", emp_chg_cd);
            }

            //職種:
            if (ws_cd != "" && ws_cd != "-1")
            {
                sb.AppendLine(" and a.WS_CD = @WS_CD ");
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
    public int getDtlCount(int startRowIndex, int maximumRows, string dept_no,
                           string emp_id, string level_cd, string emp_name, string level_cd_new,
                            string emp_chg_cd, string ws_cd, string DATA_YEAR, string DATA_SEQ)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("Select COUNT(*) total_record ");
            sb.AppendLine("	         from TB_S_M_PROMOTION_TXN a ");
            sb.AppendLine("          left join TB_9_M_COMM_D b on  b.SYS_CD='HB' and b.MAIN_CD='EMP_CHG_CD' and b.SUB_CD = a.EMP_CHG_CD ");
            sb.AppendLine("          left join TB_9_M_COMM_D D on  d.SYS_CD='SM' and d.MAIN_CD='CHG_STATUS' and d.SUB_CD = a.CHG_STATUS ");
            sb.AppendLine("	        where DATA_YEAR = @DATA_YEAR and DATA_SEQ = @DATA_SEQ and PROMOTION_TYPE='0'  ");
            ht.Add("@DATA_YEAR", DATA_YEAR);
            ht.Add("@DATA_SEQ", DATA_SEQ);
            //部門:
            if (dept_no != "")
            {
                sb.AppendLine(" and a.DEPT_NO like @DEPT_NO ");
                ht.Add("@DEPT_NO", "%" + dept_no + "%");
            }

            //工號:
            if (emp_id != "")
            {
                sb.AppendLine(" and a.EMP_ID like @EMP_ID ");
                ht.Add("@EMP_ID", "%" + emp_id + "%");
            }

            //原資格:
            if (level_cd != "" && level_cd != "-1")
            {
                sb.AppendLine(" and a.LEVEL_CD = @LEVEL_CD ");
                ht.Add("@LEVEL_CD", level_cd);
            }

            //姓名:
            if (emp_name != "")
            {
                sb.AppendLine(" and a.EMP_NAME LIKE @EMP_NAME ");
                ht.Add("@EMP_NAME", "%" + emp_name.Trim() + "%");
            }
            //晉昇資格:
            if (level_cd_new != "" && level_cd_new != "-1")
            {
                sb.AppendLine(" and a.LEVEL_CD_NEW = @LEVEL_CD_NEW ");
                ht.Add("@LEVEL_CD_NEW", level_cd_new);
            }

            //在職區分:
            if (emp_chg_cd != "" && emp_chg_cd != "-1")
            {
                sb.AppendLine(" and a.EMP_CHG_CD = @EMP_CHG_CD ");
                ht.Add("@EMP_CHG_CD", emp_chg_cd);
            }

            //職種:
            if (ws_cd != "" && ws_cd != "-1")
            {
                sb.AppendLine(" and a.WS_CD = @WS_CD ");
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
        catch
        {
            throw;
        }
    }
    public DataTable getEmp(string EMP_ID)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("select EMP_ID,EMP_NAME,DEPT_NO+'-'+DEPT_NAME DEPT_NO,WS_CD,WORK_YEARS,LEVEL_CD,GRADE_CD,");
            sb.AppendLine(" PJOB_CD+'-'+PJOB_DESC PJOB_CD,ROUND(RECENT_LEVEL_WORK_DAYS/365,1) RECENT_LEVEL_WORK_DAYS,EMP_CHG_CD");
            sb.AppendLine(" from VW_H_EMP_DATA");
            sb.AppendLine(" where  EMP_ID = @EMP_ID");
            ht.Add("@EMP_ID", EMP_ID);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
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
            sb.AppendLine("         select TOP 5 ASSESS_YEAR from TB_S_M_ASSESS where EMP_ID = @EMP_ID and len(SCORE_1H) > 0 and ASSESS_YEAR < @YEAR ORDER BY ASSESS_YEAR desc ");
            sb.AppendLine("      ) ");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@YEAR", DateTime.Now.Year.ToString());
            return dbConn.Query(sb, ht);
        }
        catch
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
            sb.AppendLine(" select EMP_ID, DEPT_NO,DEPT_NAME_20+ ' '+ DEPT_NAME_40 as DEPT_NAME, DEPT_NO + '-' + DEPT_NAME_20+ ' '+DEPT_NAME_40 as DEPT_NO1,GRADE_CD, ");
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
    public void updataPH(string PROMOTION_TOTAL)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //BeginTransaction();

            //基本資料
            sb.AppendLine(" Update TB_S_M_PROMOTION_H Set ");
            sb.AppendLine(" PROMOTION_TOTAL=@PROMOTION_TOTAL, NOTICE_DT = null ,NOTICE_BY = '' , APPROVE_DT = null , APPROVE_BY = '' , PROCESS_STATUS = 'N' ,GENERATE_DT = GETDATE(), ");
            sb.AppendLine(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID ");
            sb.AppendLine(" Where DATA_SEQ=@DATA_SEQ and DATA_YEAR=@DATA_YEAR and PROMOTION_TYPE = '0' ");
            //sb.AppendLine(" Where DATA_SEQ=@DATA_SEQ and DATA_YEAR=@DATA_YEAR");
            ht.Add("@PROMOTION_TOTAL", PROMOTION_TOTAL);
            ht.Add("@DATA_SEQ", DATA_SEQ);
            ht.Add("@DATA_YEAR", DATA_YEAR);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }
    public DataTable getPROMOTION_TOTAL()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select count(*) cnt from TB_S_M_PROMOTION_TXN where DATA_SEQ=@DATA_SEQ and DATA_YEAR=@DATA_YEAR and PROMOTION_TYPE = '0' and CHG_STATUS <> 'D'");
            ht.Add("@DATA_SEQ", DATA_SEQ);
            ht.Add("@DATA_YEAR", DATA_YEAR);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //刪除 檢查是否已核可
    public DataTable getPromotionHDT(string data_year, string data_seq)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" Select COUNT(*) total_record from ( ");
            sb.AppendLine(" select PROCESS_STATUS from TB_S_M_PROMOTION_H where PROCESS_STATUS = 'Y' ");
            sb.AppendLine(" and DATA_YEAR = @DATA_YEAR and DATA_SEQ = @DATA_SEQ and PROMOTION_TYPE = '0' ");
            ht.Add("@DATA_YEAR", data_year);
            ht.Add("@DATA_SEQ", data_seq);
            //DataTable dt = dbConn.Query(sb, ht);
            sb.AppendLine(" )a ");
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //刪除明細
    public void deletePromotionDtlYtoN(string data_year, string emp_id, string data_seq)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" DELETE TB_S_M_PROMOTION_TXN ");
            sb.AppendLine(" where DATA_YEAR = @DATA_YEAR and EMP_ID= @EMP_ID and DATA_SEQ = @DATA_SEQ and PROMOTION_TYPE = '0' ");
            ht.Add("@DATA_YEAR", data_year);
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@DATA_SEQ", data_seq);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //update明細
    public void updatePromotiondtl(string chg_status)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" update TB_S_M_PROMOTION_TXN set CHG_STATUS = @CHG_STATUS,LEVEL_CD_NEW = @LEVEL_CD_NEW,GRADE_CD_NEW = @GRADE_CD_NEW, PJOB_CD_NEW = @PJOB_CD_NEW");
            sb.AppendLine(" ,PJOB_DESC_NEW = @PJOB_DESC_NEW,PROCESS_STATUS = @PROCESS_STATUS,FINIAL_CHG_DT = GETDATE(),UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID ");
            sb.AppendLine(" where DATA_YEAR=@DATA_YEAR and DATA_SEQ = @DATA_SEQ and PROMOTION_TYPE='0' and EMP_ID=@EMP_ID ");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@DATA_YEAR", DATA_YEAR);
            ht.Add("@DATA_SEQ", DATA_SEQ);

            ht.Add("@CHG_STATUS", chg_status);
            ht.Add("@LEVEL_CD_NEW", LEVEL_CD_NEW);
            ht.Add("@GRADE_CD_NEW", GRADE_CD_NEW);
            ht.Add("@PJOB_CD_NEW", PJOB_CD_NEW);
            ht.Add("@PJOB_DESC_NEW", PJOB_DESC_NEW);
            ht.Add("@PROCESS_STATUS", PROCESS_STATUS);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }
    //add 明細
    public void addPromotiondtl(string chg_status)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" insert into TB_S_M_PROMOTION_TXN ");
            sb.AppendLine(" (DATA_YEAR,EMP_ID,EMP_NAME,EMP_CHG_CD,LEVEL_CD,GRADE_CD,PJOB_CD,PJOB_DESC,WS_CD,DEPT_NO ");
            sb.AppendLine(" ,DIV_FULL_DEPT_NAME,FINIAL_CHG_DT,WORK_YEARS,LEVEL_WORK_YEARS,EXCEPTION_STATUS,CHG_STATUS,PROCESS_STATUS ");
            sb.AppendLine("  ,EXECUTIVE_STATUS,APPROVE_BY,APPROVE_DT,LEVEL_CD_NEW,GRADE_CD_NEW,PJOB_CD_NEW,PJOB_DESC_NEW ");
            sb.AppendLine("  ,DATA_SEQ,PROMOTION_TYPE,ASSESS_SCORE_1,ASSESS_SCORE_2,ASSESS_SCORE_3,ASSESS_SCORE_4,ASSESS_SCORE_5 ");
            sb.AppendLine(" ,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)　");
            sb.AppendLine(" values (@DATA_YEAR,@EMP_ID,@EMP_NAME,@EMP_CHG_CD,@LEVEL_CD,@GRADE_CD,@PJOB_CD,@PJOB_DESC,@WS_CD,@DEPT_NO ");
            sb.AppendLine(" ,@DIV_FULL_DEPT_NAME,GETDATE(),@WORK_YEARS,@LEVEL_WORK_YEARS,'N',@CHG_STATUS,'N' ");
            sb.AppendLine("  ,'N',null,null,@LEVEL_CD_NEW,@GRADE_CD_NEW,@PJOB_CD1_NEW,@PJOB_DESC1_NEW ");
            sb.AppendLine("  ,@DATA_SEQ,'0',@ASSESS_SCORE_1,@ASSESS_SCORE_2,@ASSESS_SCORE_3,@ASSESS_SCORE_4,@ASSESS_SCORE_5 ");
            sb.AppendLine(" ,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");
            ht.Add("@DATA_YEAR", DATA_YEAR);
            ht.Add("@DATA_SEQ", DATA_SEQ);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@DEPT_NO", DEPT_NO);
            ht.Add("@DIV_FULL_DEPT_NAME", DEPT_NAME);
            ht.Add("@EMP_NAME", EMP_NAME);
            ht.Add("@WS_CD", WS_CD);
            if (WORK_YEARS != "")
            {
                ht.Add("@WORK_YEARS", WORK_YEARS);
            }
            else
            {
                ht.Add("@WORK_YEARS", "0");
            }
            ht.Add("@LEVEL_CD", LEVEL_CD);
            ht.Add("@GRADE_CD", GRADE_CD);
            ht.Add("@PJOB_CD", PJOB_CD);
            ht.Add("@PJOB_DESC", PJOB_DESC);
            if (LEVEL_WORK_YEARS != "")
            {
                ht.Add("@LEVEL_WORK_YEARS", LEVEL_WORK_YEARS);
            }
            else { ht.Add("@LEVEL_WORK_YEARS", "0"); }
            ht.Add("@CHG_STATUS", chg_status);
            ht.Add("@EMP_CHG_CD", EMP_CHG_CD);
            ht.Add("@ASSESS_SCORE_1", ASSESS_SCORE_1);
            ht.Add("@ASSESS_SCORE_2", ASSESS_SCORE_2);
            ht.Add("@ASSESS_SCORE_3", ASSESS_SCORE_3);
            ht.Add("@ASSESS_SCORE_4", ASSESS_SCORE_4);
            ht.Add("@ASSESS_SCORE_5", ASSESS_SCORE_5);
            ht.Add("@LEVEL_CD_NEW", LEVEL_CD_NEW);
            ht.Add("@GRADE_CD_NEW", GRADE_CD_NEW);
            ht.Add("@PJOB_CD1_NEW", PJOB_CD_NEW);
            ht.Add("@PJOB_DESC1_NEW", PJOB_DESC_NEW);

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
    //手動修改前 檢查核可狀態
    public DataTable getPROCESS_STATUS()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" Select DATA_YEAR, DATA_SEQ , PROCESS_STATUS from TB_S_M_PROMOTION_H  ");
            sb.AppendLine(" where DATA_YEAR=@DATA_YEAR and DATA_SEQ=@DATA_SEQ and PROMOTION_TYPE = '0'  ");
            ht.Add("@DATA_YEAR", DATA_YEAR);
            ht.Add("@DATA_SEQ", DATA_SEQ);
            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }
    #endregion

    #region "檢核"
    public DataTable check_LevelIsRA()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select a.EMP_ID, a.SCORE_1H, b.AGE ");
            sb.AppendLine("   from TB_S_M_ASSESS a  ");
            sb.AppendLine("   left join VW_H_EMP_DATA b on a.EMP_ID=b.EMP_ID ");
            sb.AppendLine("  where a.ASSESS_YEAR = (select max(ASSESS_YEAR) from TB_S_M_ASSESS where EMP_ID = @EMP_ID) ");
            sb.AppendLine("    and a.EMP_ID = @EMP_ID and  b.AGE >= '46' and(a.SCORE_1H='A' or a.SCORE_1H='B' or a.SCORE_1H='C' ) ");

            ht.Add("@EMP_ID", EMP_ID);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable check_LevelIsRB()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select a.EMP_ID, a.SCORE_1H, b.AGE ");
            sb.AppendLine("   from TB_S_M_ASSESS a  ");
            sb.AppendLine("   left join VW_H_EMP_DATA b on a.EMP_ID=b.EMP_ID ");
            sb.AppendLine("  where a.ASSESS_YEAR = (select max(ASSESS_YEAR) from TB_S_M_ASSESS where EMP_ID = @EMP_ID) ");
            sb.AppendLine("    and a.EMP_ID = @EMP_ID and (a.SCORE_1H='B' or a.SCORE_1H='C' or a.SCORE_1H='D' ) ");

            ht.Add("@EMP_ID", EMP_ID);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable checkGRADE_LEVEL()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" Select count(*) cnt from TB_H_M_LEVEL_GRADE where IS_VALID='Y' and  LEVEL_CD=@LEVEL_CD_NEW ");
            sb.AppendLine(" and GRADE_CD=@GRADE_CD_NEW");
            ht.Add("@LEVEL_CD_NEW", LEVEL_CD_NEW);
            ht.Add("@GRADE_CD_NEW", GRADE_CD_NEW);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable checkPJOB_LEVEL()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" Select count(*) cnt from VW_TB_H_M_PJOB ");
            sb.AppendLine("  where PJOB_CD=@PJOB_CD_NEW and LEVEL_CD=@LEVEL_CD_NEW ");
            ht.Add("@LEVEL_CD_NEW", LEVEL_CD_NEW);
            ht.Add("@PJOB_CD_NEW", PJOB_CD_NEW);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable getemp_id()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" Select EMP_ID  From TB_S_M_PROMOTION_TXN where EMP_ID = @EMP_ID and DATA_YEAR = @DATA_YEAR");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@DATA_YEAR", DATA_YEAR);
            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable getemp_idImport()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" Select EMP_ID  From TB_S_M_PROMOTION_TXN where EMP_ID = @EMP_ID and DATA_YEAR = @DATA_YEAR");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@DATA_YEAR", DATA_YEAR);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable checkgrade()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select EMP_ID,GRADE_CD from VW_H_EMP_DATA where EMP_ID = @EMP_ID and GRADE_CD<=@GRADE_CD_NEW ");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@GRADE_CD_NEW", GRADE_CD_NEW);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public string getOriLevelData()
    {
        string st = "";
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("select ORDER_SEQ from TB_H_M_LEVEL h");
            sb.AppendLine(" left join VW_H_EMP_DATA v");
            sb.AppendLine(" on h.LEVEL_CD = v.LEVEL_CD");
            sb.AppendLine(" where v.EMP_ID = @EMP_ID and getdate() between h.START_DT and h.END_DT");
            ht.Add("@EMP_ID", EMP_ID);

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                st = dt.Rows[0]["ORDER_SEQ"].ToString();
            }
            return st;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public string getNewiLevelData()
    {
        string st = "";
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("select ORDER_SEQ from TB_H_M_LEVEL");
            sb.AppendLine(" where LEVEL_CD = @LEVEL_CD");
            sb.AppendLine(" and getdate() between START_DT and END_DT");

            ht.Add("@LEVEL_CD", LEVEL_CD_NEW);

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                st = dt.Rows[0]["ORDER_SEQ"].ToString();
            }
            return st;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public string getOriLevelCD()
    {
        string st = "";
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select LEVEL_CD from VW_H_EMP_DATA where EMP_ID = @EMP_ID");
            ht.Add("@EMP_ID", EMP_ID);

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                st = dt.Rows[0]["LEVEL_CD"].ToString();
            }
            return st;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public string getOriGrade()
    {
        string st = "";
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select GRADE_CD from VW_H_EMP_DATA where EMP_ID = @EMP_ID");
            ht.Add("@EMP_ID", EMP_ID);

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                st = dt.Rows[0]["GRADE_CD"].ToString();
            }
            return st;
        }
        catch (Exception)
        {
            throw;
        }
    }
    /*
    public string getGradeSeq(string levelCD, string graedCD)
    {
        string st = "";
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select ORDER_SEQ from TB_H_M_LEVEL_GRADE  ");
            sb.Append(" where IS_VALID='Y' ");
            sb.Append(" and LEVEL_CD=@LEVEL_CD ");
            sb.Append(" and GRADE_CD=@GRADE_CD ");

            ht.Add("@LEVEL_CD", levelCD);
            ht.Add("@GRADE_CD", graedCD);

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                st = dt.Rows[0]["ORDER_SEQ"].ToString();
            }
            return st;
        }
        catch (Exception)
        {
            throw;
        }
    }
    */

    public DataTable checkLevelCd()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select LEVEL_CD from VW_H_EMP_DATA where EMP_ID = @EMP_ID  ");
            ht.Add("@EMP_ID", EMP_ID);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion

    #region "Import"
    public DataTable getEmpData(string cell1)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.AppendLine(" Select EMP_ID from VW_H_EMP_DATA  ");
        sb.AppendLine(" where EMP_ID=@EMP_ID ");
        ht.Add("@EMP_ID", cell1);
        return dbConn.Query(sb, ht);
    }
    public DataTable getLeaveCd(string cell3)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.AppendLine(" Select LEVEL_CD from TB_H_M_LEVEL_GRADE  ");
        sb.AppendLine(" where LEVEL_CD = @LEVEL_CD ");
        ht.Add("@LEVEL_CD", cell3);
        return dbConn.Query(sb, ht);

    }
    public DataTable getPjobCd(string cell5)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.AppendLine(" Select PJOB_CD from TB_H_M_PJOB  ");
        sb.AppendLine(" where PJOB_CD = @PJOB_CD ");
        ht.Add("@PJOB_CD", cell5);
        return dbConn.Query(sb, ht);
    }
    public DataTable getPJOB_CD_DESC()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" Select PJOB_CD, PJOB_DESC From VW_TB_H_M_PJOB where PJOB_CD = @PJOB_CD ");
            ht.Add("@PJOB_CD", PJOB_CD_NEW);
            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }
    public DataTable getEmpName(string cell1, string cell2)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.AppendLine(" Select EMP_ID, EMP_NAME from VW_H_EMP_DATA  ");
        sb.AppendLine(" where EMP_ID=@EMP_ID and EMP_NAME = @EMP_NAME ");
        ht.Add("@EMP_ID", cell1);
        ht.Add("@EMP_NAME", cell2);
        return dbConn.Query(sb, ht);
    }
    #endregion

}