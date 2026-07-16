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
public class CFB2SM1200DAO : BaseDAO
{
    public CFB2SM1200DAO()
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
            if (sortExpression.Contains("NOTICE_BY_NAME"))
                sortExpression = sortExpression.Replace("NOTICE_BY_NAME", "NOTICE.EMP_NAME");

            if (sortExpression.Contains("APPROVE_BY_NAME"))
                sortExpression = sortExpression.Replace("APPROVE_BY_NAME", "APPROVE.EMP_NAME");

            if (sortExpression == "")
            {
                sortExpression = "DATA_YEAR DESC,DATA_SEQ ";
            }
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" select * from");
            sb.AppendLine(" (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.AppendLine(" P.DATA_YEAR+CONVERT(varchar(2),P.DATA_SEQ) as qdatakey, ");
            sb.AppendLine(" NOTICE.EMP_NAME as NOTICE_BY_NAME, ");   //提出核可人員(姓名)
            sb.AppendLine(" APPROVE.EMP_NAME as APPROVE_BY_NAME, ");//核可人員(姓名)
            sb.AppendLine(" P.DATA_YEAR,P.DATA_SEQ, P.PROCESS_STATUS, P.GENERATE_DT, P.NOTICE_DT,P.APPROVE_DT,P.EXECUTIVE_DT,P.RELEASE_DT,P.PROMOTION_TYPE, ");
            sb.AppendLine(" P.PROCESS_STATUS +'-'+ D.SUB_DESC as PROCESS_STATUS_DESC ");
            sb.AppendLine(" from TB_S_M_PROMOTION_H P ");
            sb.AppendLine(" left join VW_H_EMP_DATA NOTICE on NOTICE.EMP_ID = P.NOTICE_BY");
            sb.AppendLine(" left join VW_H_EMP_DATA APPROVE on APPROVE.EMP_ID = P.APPROVE_BY");
            sb.AppendLine(" left join TB_9_M_COMM_D D on D.SYS_CD='SA' and D.MAIN_CD='PROCESS_STATUS' and P.PROCESS_STATUS = D.SUB_CD ");
            sb.AppendLine(" left join TB_H_M_EMP b on b.EMP_ID = P.NOTICE_BY                                                         ");
            sb.AppendLine(" left join TB_H_R_HEAD_DEPT c on b.DEPT_NO=c.MNG_DEPT_NO                                                    ");

            sb.AppendLine(" where 1=1 and P.PROMOTION_TYPE='0' and c.EMP_ID= @CURRENT_EMP_ID ");
            //and c.EMP_ID= @CURRENT_EMP_ID
            ht.Add("@CURRENT_EMP_ID", SessionHandle.Current.emp_id);

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
            sb.AppendLine(" from TB_S_M_PROMOTION_H P");
            sb.AppendLine(" left join VW_H_EMP_DATA NOTICE on NOTICE.EMP_ID = P.NOTICE_BY");
            sb.AppendLine(" left join VW_H_EMP_DATA APPROVE on APPROVE.EMP_ID = P.APPROVE_BY");
            sb.AppendLine(" left join TB_H_M_EMP b on b.EMP_ID = P.NOTICE_BY                                                         ");
            sb.AppendLine(" left join TB_H_R_HEAD_DEPT c on b.DEPT_NO=c.MNG_DEPT_NO                                                    ");

            sb.AppendLine(" where 1=1 and P.PROMOTION_TYPE='0' and c.EMP_ID= @CURRENT_EMP_ID ");
            //and c.EMP_ID= @CURRENT_EMP_ID
            ht.Add("@CURRENT_EMP_ID", SessionHandle.Current.emp_id);
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

    #endregion

    #region Dtl

    //查詢明細表頭部分
    public DataTable getDtlHeader(string qdatakey)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" select P.DATA_YEAR, P.DATA_SEQ, P.REMARK_DESC, P.PROCESS_STATUS ");
            sb.AppendLine(" ,CONVERT(varchar(100), P.NOTICE_DT, 111) as NOTICE_DT ");
            sb.AppendLine(" ,CONVERT(varchar(100), P.APPROVE_DT, 111) as APPROVE_DT ");
            sb.AppendLine(" ,CONVERT(varchar(100), P.EXECUTIVE_DT, 111) as EXECUTIVE_DT ");
            sb.AppendLine(" ,NOTICE.EMP_NAME as NOTICE_BY_NAME ");   //提出核可人員(姓名)
            sb.AppendLine(" ,APPROVE.EMP_NAME as APPROVE_BY_NAME ");//核可人員(姓名)
            sb.AppendLine(" from TB_S_M_PROMOTION_H P");
            sb.AppendLine(" left join VW_H_EMP_DATA NOTICE on NOTICE.EMP_ID = P.NOTICE_BY");
            sb.AppendLine(" left join VW_H_EMP_DATA APPROVE on APPROVE.EMP_ID = P.APPROVE_BY");
            sb.AppendLine(" where P.DATA_YEAR+CONVERT(varchar(2),P.DATA_SEQ) = @QDATAKEY");
            sb.AppendLine(" and P.PROMOTION_TYPE='0' ");
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
            if (sortExpression == "")
            {
                sortExpression = "EXCEPTION_STATUS DESC,P.UPDATED_DT DESC,PJOB_CD ASC,EMP_ID";
            }
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //sb.AppendLine(" select * from");
            sb.AppendLine(" select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.AppendLine(" P.DATA_YEAR + P.EMP_ID as dtldatakey, ");
            sb.AppendLine(" P.DEPT_NO +'-'+ P.DIV_FULL_DEPT_NAME as DEPT, ");
            sb.AppendLine(" P.PJOB_CD +'-'+ P.PJOB_DESC as PJOB, ");
            sb.AppendLine(" P.PJOB_CD_NEW +'-'+ P.PJOB_DESC_NEW as PJOB_NEW, ");
            sb.AppendLine(" D.SUB_DESC as EMP_CHG_CD_SUB, ");
            sb.AppendLine(" P.UPDATED_DT as PROMOTION_UPDATED_DT, ");
            sb.AppendLine(" D.UPDATED_DT as COMM_UPDATED_DT, ");
            sb.AppendLine(" P.EXCEPTION_STATUS,P.CHG_STATUS, P.PROCESS_STATUS, P.PJOB_CD, P.EMP_ID, P.EMP_NAME, P.WS_CD, P.WORK_YEARS, P.LEVEL_CD, P.GRADE_CD, P.LEVEL_WORK_YEARS, ");
            sb.AppendLine(" P.LEVEL_CD_NEW, P.GRADE_CD_NEW, P.EMP_CHG_CD, P.ASSESS_SCORE_1, P.ASSESS_SCORE_2, P.ASSESS_SCORE_3, P.ASSESS_SCORE_4,");
            sb.AppendLine(" P.ASSESS_SCORE_5 ");
            sb.AppendLine(" from TB_S_M_PROMOTION_TXN P");
            sb.AppendLine(" left join TB_9_M_COMM_D D on D.SYS_CD = 'HB' and D.MAIN_CD='EMP_CHG_CD' and D.SUB_CD = P.EMP_CHG_CD");
            sb.AppendLine(" where P.DATA_YEAR+CONVERT(varchar(2),P.DATA_SEQ) = @QDATAKEY");
            sb.AppendLine(" and P.PROMOTION_TYPE='0' ");
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
            sb.AppendLine(" and PROMOTION_TYPE='0' ");
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
    //核可的主檔更新
    internal void updateMasterConfirmData(string qdatakey, string remark)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" Update TB_S_M_PROMOTION_H ");
            sb.AppendLine(" Set PROCESS_STATUS = @PROCESS_STATUS,APPROVE_BY = @APPROVE_BY,APPROVE_DT = GETDATE(),REMARK_DESC = @REMARK_DESC");
            sb.AppendLine(" ,UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID");
            sb.AppendLine(" where  DATA_YEAR+CONVERT(varchar(2),DATA_SEQ) = @QDATAKEY");
            sb.AppendLine(" and PROMOTION_TYPE='0' ");

            ht.Add("@PROCESS_STATUS", "Y");
            ht.Add("@APPROVE_BY", SessionHandle.Current.emp_id);
            ht.Add("@REMARK_DESC", remark);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SM120");

            ht.Add("@QDATAKEY", qdatakey);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }
    //核可的明細檔更新
    internal void updateConfirmData(string confirmListItem, string qdatakey, string remark)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" Update TB_S_M_PROMOTION_TXN ");
            sb.AppendLine(" Set PROCESS_STATUS = @PROCESS_STATUS,EXCEPTION_STATUS = @EXCEPTION_STATUS,APPROVE_BY = @APPROVE_BY,APPROVE_DT = GETDATE()");
            sb.AppendLine(" ,UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID");
            sb.AppendLine(" where  DATA_YEAR+CONVERT(varchar(2),DATA_SEQ) = @QDATAKEY");
            sb.AppendLine(" and PROMOTION_TYPE='0' ");
            sb.AppendLine(" and EMP_ID = @REMAINLISTITEM");

            ht.Add("@PROCESS_STATUS", "Y");    //核可狀態
            ht.Add("@EXCEPTION_STATUS", "N"); //異常狀態
            ht.Add("@APPROVE_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SM120");

            ht.Add("@QDATAKEY", qdatakey);
            ht.Add("@REMAINLISTITEM", confirmListItem);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }
    //駁回的主檔更新
    internal void updateMasterRejectData(string qdatakey, string remark)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" Update TB_S_M_PROMOTION_H ");
            sb.AppendLine(" Set PROCESS_STATUS = @PROCESS_STATUS,NOTICE_BY = @NOTICE_BY,NOTICE_DT = @NOTICE_DT,APPROVE_BY = @APPROVE_BY,APPROVE_DT = @APPROVE_DT");
            sb.AppendLine(" ,REMARK_DESC = @REMARK_DESC,UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID");
            sb.AppendLine(" where  DATA_YEAR+CONVERT(varchar(2),DATA_SEQ) = @QDATAKEY");
            sb.AppendLine(" and PROMOTION_TYPE='0' ");

            ht.Add("@PROCESS_STATUS", "B");
            ht.Add("NOTICE_BY", DBNull.Value);
            ht.Add("NOTICE_DT", DBNull.Value);
            ht.Add("APPROVE_BY", DBNull.Value);
            ht.Add("APPROVE_DT", DBNull.Value);
            ht.Add("REMARK_DESC", remark);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SM120");

            ht.Add("@QDATAKEY", qdatakey);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }
    //駁回未打勾的明細檔更新
    internal void updateRemainData(string remainListItem, string qdatakey, string remark)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" Update TB_S_M_PROMOTION_TXN ");
            sb.AppendLine(" Set PROCESS_STATUS = @PROCESS_STATUS,EXCEPTION_STATUS = @EXCEPTION_STATUS,APPROVE_BY = @APPROVE_BY");
            sb.AppendLine(" ,APPROVE_DT = @APPROVE_DT,UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID");
            sb.AppendLine(" where  DATA_YEAR+CONVERT(varchar(2),DATA_SEQ) = @QDATAKEY");
            sb.AppendLine(" and PROMOTION_TYPE='0' ");
            sb.AppendLine(" and EMP_ID = @REMAINLISTITEM");

            ht.Add("@PROCESS_STATUS", "B");    //核可狀態
            ht.Add("@EXCEPTION_STATUS", "N"); //異常狀態
            ht.Add("@APPROVE_BY", DBNull.Value);
            ht.Add("@APPROVE_DT", DBNull.Value);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SM120");

            ht.Add("@QDATAKEY", qdatakey);
            ht.Add("@REMAINLISTITEM", remainListItem);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }
    //駁回有打勾的明細檔更新
    internal void updateRejectData(string rejectListItem, string qdatakey, string remark)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" Update TB_S_M_PROMOTION_TXN ");
            sb.AppendLine(" Set PROCESS_STATUS = @PROCESS_STATUS,EXCEPTION_STATUS = @EXCEPTION_STATUS,APPROVE_BY = @APPROVE_BY");
            sb.AppendLine(" ,APPROVE_DT = @APPROVE_DT,UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID");
            sb.AppendLine(" where  DATA_YEAR+CONVERT(varchar(2),DATA_SEQ) = @QDATAKEY");
            sb.AppendLine(" and PROMOTION_TYPE='0' ");
            sb.AppendLine(" and EMP_ID = @REMAINLISTITEM");

            ht.Add("@PROCESS_STATUS", "B");    //核可狀態
            ht.Add("@EXCEPTION_STATUS", "Y"); //異常狀態
            ht.Add("@APPROVE_BY", DBNull.Value);
            ht.Add("@APPROVE_DT", DBNull.Value);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SM120");

            ht.Add("@QDATAKEY", qdatakey);
            ht.Add("@REMAINLISTITEM", rejectListItem);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }

    #endregion
}