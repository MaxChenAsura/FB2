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
/// CFB2990100DAO 的摘要描述
/// </summary>
public class CFB2SM2300DAO : BaseDAO
{
    public CFB2SM2300DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public string RowNumber { get; set; }
    public string QDATAKEY { get; set; }
    public string DATA_YEAR { get; set; }
    public string DATA_SEQ { get; set; }
    public string PROCESS_STATUS { get; set; }
    public string GENERATE_DT { get; set; }
    public string NOTICE_BY { get; set; }
    public string NOTICE_DT { get; set; }
    public string APPROVE_BY { get; set; }
    public string APPROVE_DT { get; set; }
    public string EXECUTIVE_DT { get; set; }

    public string REMARK_DESC { get; set; }
    public string CHG_STATUS { get; set; }
    public string DEPT_NO { get; set; }
    public string EMP_ID { get; set; }
    public string EMP_NAME { get; set; }
    public string WS_CD { get; set; }
    public string WORK_YEARS { get; set; }
    public string LEVEL_CD { get; set; }
    public string GRADE_CD { get; set; }
    public string PJOB_CD { get; set; }
    public string LEVEL_WORK_YEARS { get; set; }
    public string LEVEL_CD_NEW { get; set; }
    public string GRADE_CD_NEW { get; set; }
    public string PJOB_CD_NEW { get; set; }
    public string EMP_CHG_CD { get; set; }
    public string ASSESS_SCORE_1 { get; set; }
    public string ASSESS_SCORE_2 { get; set; }
    public string ASSESS_SCORE_3 { get; set; }
    public string ASSESS_SCORE_4 { get; set; }
    public string ASSESS_SCORE_5 { get; set; }

    //for查詢欄位
    public string txt_DATA_YEAR_search { get; set; }

    #region Qry


    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string data_year, string firstLoad)
    {
        try
        {
            DateTime CurrTime = DateTime.Now;
            if (sortExpression == "")
            {
                sortExpression = "DATA_YEAR DESC,DATA_SEQ ";
            }

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" select * from");
            sb.AppendLine(" (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.AppendLine(" P.DATA_YEAR+CONVERT(varchar(2),P.DATA_SEQ) as qdatakey, ");
            sb.AppendLine(" P.DATA_YEAR,P.DATA_SEQ, P.PROCESS_STATUS, P.GENERATE_DT, P.NOTICE_DT,P.APPROVE_DT,P.EXECUTIVE_DT,P.RELEASE_DT,P.PROMOTION_TYPE ");
            sb.AppendLine(" from TB_S_M_PROMOTION_H P ");
            sb.AppendLine(" left join VW_H_EMP_DATA NOTICE on NOTICE.EMP_ID = P.NOTICE_BY");
            sb.AppendLine(" left join VW_H_EMP_DATA APPROVE on APPROVE.EMP_ID = P.APPROVE_BY");
            sb.AppendLine(" where 1=1");
            sb.AppendLine(" and P.PROMOTION_TYPE='5' ");
            if (firstLoad == "Y") //如果是第一次進到頁面，列出近五年資料
            {
            	//Stanley Chen fixed...增加查詢「系統年度+1年」的資料
                //sb.AppendLine(" and (P.DATA_YEAR >= @DATA_YEAR-4 and P.DATA_YEAR <= @DATA_YEAR) ");
                sb.AppendLine(" and (P.DATA_YEAR >= @DATA_YEAR-4) ");
                ht.Add("@DATA_YEAR", CurrTime.Year.ToString());
            }
            else
            {
                if (data_year != "" && data_year != null)
                {
                    sb.AppendLine(" and P.DATA_YEAR = @DATA_YEAR  ");
                    ht.Add("@DATA_YEAR", data_year);
                }
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
    public int getCount(int startRowIndex, int maximumRows, string data_year, string firstLoad)
    {
        try
        {
            DateTime CurrTime = DateTime.Now;
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" Select COUNT(*) total_record ");
            sb.AppendLine(" from TB_S_M_PROMOTION_H");
            sb.AppendLine(" where 1=1");
            sb.AppendLine(" and PROMOTION_TYPE='5' ");
            if (firstLoad == "Y") //如果是第一次進到頁面，列出近五年資料
            {
            	//Stanley Chen fixed...增加查詢「系統年度+1年」的資料
                //sb.AppendLine(" and (DATA_YEAR >= @DATA_YEAR-4 and DATA_YEAR <= @DATA_YEAR) ");
                sb.AppendLine(" and (DATA_YEAR >= @DATA_YEAR-4) ");
                ht.Add("@DATA_YEAR", CurrTime.Year.ToString());
            }
            else
            {
                if (data_year != "" && data_year != null)
                {
                    sb.AppendLine(" and DATA_YEAR = @DATA_YEAR  ");
                    ht.Add("@DATA_YEAR", data_year);
                }
            }
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
    //Release主檔更新
    internal void updateReleaseData(string qdatakey)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" Update TB_S_M_PROMOTION_H ");
            sb.AppendLine(" Set RELEASE_DT = GETDATE(),UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID");
            sb.AppendLine(" where  DATA_YEAR+CONVERT(varchar(2),DATA_SEQ) = @QDATAKEY");
            sb.AppendLine(" and PROMOTION_TYPE='5' ");
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SM230");

            ht.Add("@QDATAKEY", qdatakey);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }
    #endregion

    #region Dtl

    //查詢明細表頭部分
    public DataTable getDtlHeader(string qdatakey)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" select P.DATA_YEAR, P.DATA_SEQ, P.REMARK_DESC ");
            sb.AppendLine(" ,CONVERT(varchar(100), P.NOTICE_DT, 111) as NOTICE_DT ");
            sb.AppendLine(" ,CONVERT(varchar(100), P.APPROVE_DT, 111) as APPROVE_DT ");
            sb.AppendLine(" ,CONVERT(varchar(100), P.EXECUTIVE_DT, 111) as EXECUTIVE_DT ");
            sb.AppendLine(" ,NOTICE.EMP_NAME as NOTICE_BY_NAME ");   //提出核可人員(姓名)
            sb.AppendLine(" ,APPROVE.EMP_NAME as APPROVE_BY_NAME ");//核可人員(姓名)
            sb.AppendLine(" from TB_S_M_PROMOTION_H P");
            sb.AppendLine(" left join VW_H_EMP_DATA NOTICE on NOTICE.EMP_ID = P.NOTICE_BY");
            sb.AppendLine(" left join VW_H_EMP_DATA APPROVE on APPROVE.EMP_ID = P.APPROVE_BY");
            sb.AppendLine(" where P.DATA_YEAR+CONVERT(varchar(2),P.DATA_SEQ) = @QDATAKEY");
            sb.AppendLine(" and P.PROMOTION_TYPE='5' ");
            ht.Add("@QDATAKEY", qdatakey);
            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch
        {
            throw;
        }
    }
    //查詢明細
    public DataTable getDtlData(int startRowIndex, int maximumRows, string sortExpression, string hid_qdatakey)
    {
        try
        {
            if (sortExpression.Contains("UPDATED_DT"))
                sortExpression = sortExpression.Replace("UPDATED_DT", "P.UPDATED_DT");

            if (sortExpression == "")
            {
                sortExpression = "EXCEPTION_STATUS DESC,P.UPDATED_DT DESC,PJOB_CD ASC,EMP_ID";
            }
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //sb.AppendLine(" select * from");
            sb.AppendLine(" select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.AppendLine(" P.DATA_YEAR + P.EMP_ID as dtldatakey, ");
            sb.AppendLine(" P.DEPT_NO +'-'+ P.DIV_FULL_DEPT_NAME as DEP, ");
            sb.AppendLine(" P.PJOB_CD +'-'+ P.PJOB_DESC as PJOB, ");
            sb.AppendLine(" P.PJOB_CD_NEW +'-'+ P.PJOB_DESC_NEW as PJOB_NEW, ");
            sb.AppendLine(" D.SUB_DESC as EMP_CHG_CD_SUB, ");
            sb.AppendLine(" P.UPDATED_DT, ");
            sb.AppendLine(" D.UPDATED_DT as COMM_UPDATED_DT, ");
            sb.AppendLine(" P.EXCEPTION_STATUS,P.CHG_STATUS, P.PROCESS_STATUS, P.PJOB_CD, P.EMP_ID, P.EMP_NAME, P.WS_CD, P.WORK_YEARS, P.LEVEL_CD, P.GRADE_CD, P.LEVEL_WORK_YEARS, ");
            sb.AppendLine(" P.PJOB_CD_NEW +'-'+ P.PJOB_DESC_NEW as PJOB_NEW, ");
            sb.AppendLine(" P.LEVEL_CD_NEW, P.GRADE_CD_NEW, P.EMP_CHG_CD, P.ASSESS_SCORE_1, P.ASSESS_SCORE_2 ");
            //sb.AppendLine(" , P.ASSESS_SCORE_3, P.ASSESS_SCORE_4,P.ASSESS_SCORE_5 ");
            sb.AppendLine(" from TB_S_M_PROMOTION_TXN P");
            sb.AppendLine(" left join TB_9_M_COMM_D D on D.SYS_CD = 'HB' and D.MAIN_CD='EMP_CHG_CD' and D.SUB_CD = P.EMP_CHG_CD");
            sb.AppendLine(" where P.DATA_YEAR+CONVERT(varchar(2),P.DATA_SEQ) = @QDATAKEY");
            sb.AppendLine(" and P.PROMOTION_TYPE='5' ");
            ht.Add("@QDATAKEY", hid_qdatakey);

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }

    }
    public int getDtlCount(int startRowIndex, int maximumRows, string hid_qdatakey)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" Select COUNT(*) total_record ");
            sb.AppendLine(" from TB_S_M_PROMOTION_TXN");
            sb.AppendLine(" where DATA_YEAR+CONVERT(varchar(2),DATA_SEQ) = @QDATAKEY");
            sb.AppendLine(" and PROMOTION_TYPE='5' ");
            ht.Add("@QDATAKEY", hid_qdatakey);

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

    #endregion
}