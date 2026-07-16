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
/// CFB2SS050DAO 的摘要描述
/// </summary>
public class CFB2SS0500DAO : BaseDAO
{
    //期間工資遺激勵金主檔
    public string SALARY_DT { get; set; }
    public string INCENTIVE_TYPE { get; set; }
    public string INCENTIVE_DESC { get; set; }
    public string INCENTIVE_NUM { get; set; }
    public string INCENTIVE_TOTAL { get; set; }
    public string PRE_STATUS { get; set; }
    public string PRE_DT { get; set; }

    //明細檔
    public string EMP_ID { get; set; }

    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }


    public CFB2SS0500DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }


    #region Gridview 資料
    //Gridview 查詢資料
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression
                         , string sdt, string edt, string status, string type 
                           )
    {
        try
        {
            /*
            if (sortExpression.Contains("PJOB_CD"))
                sortExpression = sortExpression.Replace("PJOB_CD", "A.PJOB_CD");
            if (sortExpression.Contains("SALARY_ID"))
                sortExpression = sortExpression.Replace("SALARY_ID", "A.SALARY_ID");
            */
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + ") As RowNumber, ");
            sb.Append(@" CONVERT(VARCHAR(10),A.SALARY_DT,111)  as SALARY_DT  
                        ,A.INCENTIVE_TYPE
                        ,A.INCENTIVE_TYPE+ iif(isnull(B.SUB_DESC,'')='','','-'+B.SUB_DESC) as TYPE_DESC
                        ,A.INCENTIVE_NUM
                        ,A.INCENTIVE_TOTAL
                        ,A.PRE_STATUS
                        ,A.PRE_DT
                        ,A.PRE_STATUS + iif(isnull(C.SUB_DESC,'')='','','-'+C.SUB_DESC)  as STATUS_DESC
                        from TB_S_M_INCENTIVE_PAY_H A  with (nolock) 
                        left join TB_9_M_COMM_D B  with (nolock)  on B.SYS_CD='SS' and B.main_cd='INCENTIVE_TYPE' and B.is_valid='Y' and A.INCENTIVE_TYPE=B.sub_cd
                        left join TB_9_M_COMM_D C   with (nolock)  on C.SYS_CD='99' and C.main_cd='IS_YN' and C.is_valid='Y'  and A.PRE_STATUS=C.sub_cd 
                         ");
            sb.Append(" where 1=1 ");

            if (sdt != "")
            {
                sb.Append(" and SALARY_DT >=@SALARY_SDT ");
                ht.Add("@SALARY_SDT", sdt);
            }
            if (edt != "")
            {
                sb.Append(" and SALARY_DT <=@SALARY_EDT ");
                ht.Add("@SALARY_EDT", edt);
            }
            if (type != "-1")
            {
                sb.Append(" and A.INCENTIVE_TYPE = @INCENTIVE_TYPE ");
                ht.Add("@INCENTIVE_TYPE", type);
            }

            if (status != "-1")
            {
                sb.Append(" and PRE_STATUS= @PRE_STATUS ");
                ht.Add("@PRE_STATUS", status);
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
                       , string sdt, string edt, string status, string type)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) resultCount ");
            sb.Append(@" from TB_S_M_INCENTIVE_PAY_H A with (nolock) ");
            sb.Append(" where 1=1 ");

            if (sdt != "")
            {
                sb.Append(" and A.SALARY_DT >=@SALARY_SDT ");
                ht.Add("@SALARY_SDT", sdt);
            }
            if (edt != "")
            {
                sb.Append(" and A.SALARY_DT <=@SALARY_EDT ");
                ht.Add("@SALARY_EDT", edt);
            }
            if (type != "-1")
            {
                sb.Append(" and A.INCENTIVE_TYPE = @INCENTIVE_TYPE ");
                ht.Add("@INCENTIVE_TYPE", type);
            }

            if (status != "-1")
            {
                sb.Append(" and A.PRE_STATUS= @PRE_STATUS ");
                ht.Add("@PRE_STATUS", status);
            }

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                t = (int)dt.Rows[0]["resultCount"];
            }
            return t;
        }
        catch (Exception)
        {

            throw;
        }

    }

    #endregion

    #region  DTL Gridview 資料
    //Gridview 查詢資料
    public DataTable getDtlData(int startRowIndex, int maximumRows, string sortExpression
                         , string salary_dt, string type, string emp_id )
    {
        try
        {
            if (sortExpression.Contains("EMP_ID"))
                sortExpression = sortExpression.Replace("EMP_ID", "A.EMP_ID");


            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + ") As RowNumber, ");
            sb.Append(@" A.EMP_ID,B.EMP_NAME
                        ,A.SALARY_DT,A.INCENTIVE_TYPE
                        ,A.START_DT,A.END_DT
                        ,B.JOIN_DT,B.BE_EMP_DT,B.LEAVE_DT
                        ,A.INCENTIVE_AMT
                        from TB_S_M_INCENTIVE_PAY_D A
                        inner join (
                         select EMP_ID,EMP_NAME,JOIN_DT,BE_EMP_DT,LEAVE_DT from TB_H_M_EMP 
                        ) B on A.EMP_ID = B.EMP_ID
                        where 1=1
                        ");
            if (emp_id != "")
            {
                sb.Append(" and A.EMP_ID =@EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
            }
            if (salary_dt != "")
            {
                sb.Append(" and A.SALARY_DT= @SALARY_DT ");
                ht.Add("@SALARY_DT", salary_dt); ;
            }
            if (type != "")
            {
                sb.Append(" and A.INCENTIVE_TYPE= @INCENTIVE_TYPE  ");
                ht.Add("@INCENTIVE_TYPE", type);
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
    public int getDtlCount(int startRowIndex, int maximumRows
                         , string salary_dt, string type, string emp_id)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) resultCount ");
            sb.Append(@"   from TB_S_M_INCENTIVE_PAY_D A 
                    where 1=1 ");

            if (emp_id != "")
            {
                sb.Append(" and A.EMP_ID =@EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
            }
            if (salary_dt != "")
            {
                sb.Append(" and A.SALARY_DT= @SALARY_DT ");
                ht.Add("@SALARY_DT", salary_dt); ;
            }
            if (type != "")
            {
                sb.Append(" and A.INCENTIVE_TYPE= @INCENTIVE_TYPE  ");
                ht.Add("@INCENTIVE_TYPE", type);
            }

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                t = (int)dt.Rows[0]["resultCount"];
            }
            return t;
        }
        catch (Exception)
        {

            throw;
        }

    }

    #endregion


    //刪除儲存
    public void delTableSave(string tableName)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("delete from   ");
            sb.Append(tableName );
            sb.Append(@" 
                where 1=1
                and SALARY_DT =@SALARY_DT 
                and INCENTIVE_TYPE=@INCENTIVE_TYPE 
            ");
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@INCENTIVE_TYPE", INCENTIVE_TYPE);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    /*是否已轉傳薪資, 0:未轉,>0已轉*/
    public int chkIS_SEND()
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@"select count(*) cnt 
                        from TB_S_M_INCENTIVE_PAY_H
                        where 1=1
                        and SALARY_DT = @SALARY_DT         
                        and INCENTIVE_TYPE = @INCENTIVE_TYPE         
                        and PRE_STATUS = 'Y' 

            ");
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@INCENTIVE_TYPE", INCENTIVE_TYPE);

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                t = (int)dt.Rows[0]["cnt"];
            }
            return t;
        }
        catch (Exception)
        {

            throw;
        }
    }

    //檢核 節金檔 P-節金檔是否已發薪,A-節金是否已存在
    public string checkFN_SS_CHK_FESTIVAL(string check_cd)
    {
        try
        {
            string result = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select dbo.FN_SS_CHK_FESTIVAL(@SALARY_DT,@INCENTIVE_TYPE,@CHECK_CD) as result;   ");
          
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@INCENTIVE_TYPE", INCENTIVE_TYPE);
            ht.Add("@CHECK_CD", check_cd);      //P-節金檔是否已發薪,A-節金是否已存在
            DataTable dt = dbConn.Query(sb, ht, true);

            if (dt.Rows.Count > 0)
            {
                result = dt.Rows[0]["result"].ToString();
            }
            return result;
        }
        catch
        {
            throw;
        }
    }
    //EXCEL匯出
    public DataTable getExcelData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(@"
                        select 
                        CONVERT(VARCHAR(10),A.SALARY_DT,111)  as SALARY_DT  
                        ,A.INCENTIVE_TYPE+ iif(isnull(B.SUB_DESC,'')='','','-'+B.SUB_DESC) as INCENTIVE_TYPE
                        ,A.EMP_ID
                        ,C.EMP_NAME
                        ,iif(A.START_DT is null,'', CONVERT(VARCHAR(10),A.START_DT,111) ) as START_DT
                        ,iif(A.END_DT is null,'', CONVERT(VARCHAR(10),A.END_DT,111) ) as END_DT  
                        ,convert(varchar,A.WORK_DAYS) 		 as WORK_DAYS
                        ,A.BASE_MONTH 
                        ,convert(varchar,A.INCENTIVE_MONTH)	 as INCENTIVE_MONTH
                        ,A.ATTENDANCE_AMT
                        ,A.REWARD_AMT
                        ,A.DISCIPLINE_AMT
                        ,A.INCENTIVE_AMT
                        ,A.BASE_DAY
                        ,convert(varchar,A.LEAVE_A_DAY)		 as LEAVE_A_DAY
                        ,convert(varchar,A.LEAVE_B_DAY)		 as LEAVE_B_DAY
                        ,convert(varchar,A.LEAVE_AB_DAYS)	 as LEAVE_AB_DAYS
                        ,convert(varchar,A.LEAVE_Q_DAY)		 as LEAVE_Q_DAY
                        ,convert(varchar,A.LEAVE_Q_DAYS)	 as LEAVE_Q_DAYS
                        ,convert(varchar,A.THIRD_CNT_REWARD) as THIRD_CNT_REWARD
                        ,convert(varchar,A.SECOND_CNT_REWARD)as SECOND_CNT_REWARD
                        ,convert(varchar,A.FIRST_CNT_REWARD) as FIRST_CNT_REWARD
                        ,convert(varchar,A.THIRD_CNT_PUNISH) as THIRD_CNT_PUNISH
                        ,convert(varchar,A.SECOND_CNT_PUNISH)as SECOND_CNT_PUNISH
                        ,convert(varchar,A.FIRST_CNT_PUNISH) as FIRST_CNT_PUNISH
                        ,convert(varchar,A.JUDGEMENT_DAYS)   as JUDGEMENT_DAYS
                        from TB_S_M_INCENTIVE_PAY_D A with (nolock)
                        left join TB_9_M_COMM_D B with (nolock) on B.SYS_CD='SS' and B.main_cd='INCENTIVE_TYPE' and B.is_valid='Y' and A.INCENTIVE_TYPE=B.sub_cd
                        left join (select EMP_ID,EMP_NAME from TB_H_M_EMP  with (nolock) ) C on A.EMP_ID = C.EMP_ID
                        ");
            sb.Append("  where 1=1 ");
            sb.Append(" and SALARY_DT =@SALARY_DT ");
            sb.Append(" and A.INCENTIVE_TYPE = @INCENTIVE_TYPE ");

            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@INCENTIVE_TYPE", INCENTIVE_TYPE);
    
            sb.Append(" order by EMP_ID ");

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }


    #region  SP執行 轉薪資

    //獎金轉至薪資(節金檔)
    internal void exec_SP_SS_SEND_FESTIVAL()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_SS_SEND_FESTIVAL");
            ht.Add("@p_SALARY_DT", SALARY_DT);
            ht.Add("@p_INCENTIVE_TYPE", INCENTIVE_TYPE);
            ht.Add("@p_UserID", CREATED_BY);
            ht.Add("@p_FuncID", "FB2SS050");
            dbConn.ExecuteSPT(sb, ht, true);

        }
        catch (Exception)
        {

            throw;
        }
    }

    //獎金取消轉至薪資(節金檔)
    internal void exec_SP_SS_CANCEL_FESTIVAL()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_SS_CANCEL_FESTIVAL");
            ht.Add("@p_SALARY_DT", SALARY_DT);
            ht.Add("@p_INCENTIVE_TYPE", INCENTIVE_TYPE);
            ht.Add("@p_UserID", CREATED_BY);
            ht.Add("@p_FuncID", FUNC_ID);
            dbConn.ExecuteSPT(sb, ht, true);

        }
        catch (Exception)
        {

            throw;
        }
    }

   
    #endregion


}