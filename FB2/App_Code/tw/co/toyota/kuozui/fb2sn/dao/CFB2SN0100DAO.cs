using FB2.tw.co.toyota.kuozui.dao;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

/// <summary>
/// CFB2SN0100DAO 的摘要描述
/// </summary>
/// 
public class CFB2SN0100DAO : BaseDAO
{
    //screen PARA
    public string YEAR { get; set; }
    public string AFA_FOR { get; set; }
    public string SEARCH_YEAR { get; set; }
    public string SEARCH_AFA_FOR { get; set; }
    public string TYPE { get; set; }
    public string KEY1 { get; set; }
    public string KEY2 { get; set; }

    //insert PARA
    public string EMP_ID { get; set; }
    public string EMP_NAME { get; set; }
    public string AFA_AMOUNT { get; set; }
    public string AFA_TOTAL_AMOUNT { get; set; }
    public string AFA_TOTAL_PEOPLE { get; set; }  
    public string CREATED_BY { get; set; }
    public string UPDATED_BY { get; set; }
    public string FUNC_ID { get; set; }

    public CFB2SN0100DAO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    //Gridview 查詢資料
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string SEARCH_YEAR, string SEARCH_AFA_FOR)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            string[] lines = Regex.Split(SEARCH_AFA_FOR, ":");
            TYPE = lines[0];//哪一種獎金
            KEY1 = lines[1];
            KEY2 = lines[2];
            if (TYPE == "a")
            {
                sb.Append(" Select * From");
                sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
                sb.Append(" case when b.AFA_APPROVE_STATUS='N' then 'N-未核'   ");
                sb.Append(" when b.AFA_APPROVE_STATUS='B' then 'B-駁回'");
                sb.Append(" when b.AFA_APPROVE_STATUS='Y' then 'B-已核' end AFA_APPROVE_STATUS,");
                sb.Append(" isnull(AFA_APPROVE_MARK,'') AFA_APPROVE_MARK,EMP_ID,EMP_NAME,c.PJOB_CD+'-'+c.PJOB_DESC PJOB_CD,a.LEVEL_CD,AFA_AMT ,b.AFA_REMARK");
                sb.Append(" from TB_S_M_AWARD_D a");
                sb.Append(" left join  TB_S_M_AWARD_H b on a.AWARD_YEAR = b.AWARD_YEAR and a.AWARD_ROUND = b.AWARD_ROUND");
                sb.Append(" left join TB_H_M_PJOB c on a.PJOB_CD = c.PJOB_CD and START_DT <= GETDATE() and END_DT > GETDATE()");
                sb.Append(" where b.AWARD_YEAR  = @KEY1 and b.AWARD_ROUND = @KEY2  and a.AFA_AMT <> 0");
               
            }
            if (TYPE == "b")
            {
                sb.Append(" Select * From");
                sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
                sb.Append(" case when b.AFA_APPROVE_STATUS='N' then 'N-未核'   ");
                sb.Append(" when b.AFA_APPROVE_STATUS='B' then 'B-駁回'");
                sb.Append(" when b.AFA_APPROVE_STATUS='Y' then 'B-已核' end AFA_APPROVE_STATUS,");
                sb.Append(" isnull(AFA_APPROVE_MARK,'') AFA_APPROVE_MARK,EMP_ID,EMP_NAME,c.PJOB_CD+'-'+c.PJOB_DESC PJOB_CD,a.LEVEL_CD,AFA_AMT ,b.AFA_REMARK");
                sb.Append(" from TB_S_R_BONUS_D a");
                sb.Append(" left join  TB_S_M_BONUS_H b on a.BONUS_YEAR = b.BONUS_YEAR and a.BONUS_ROUND = b.BONUS_ROUND");
                sb.Append(" left join TB_H_M_PJOB c on a.PJOB_CD = c.PJOB_CD and START_DT <= GETDATE() and END_DT > GETDATE()");
                sb.Append(" where b.BONUS_YEAR  = @KEY1 and b.BONUS_ROUND = @KEY2  and a.AFA_AMT <> 0");
            }
            if (TYPE == "c")
            {
                sb.Append(" Select * From");
                sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
                sb.Append(" case when b.AFA_APPROVE_STATUS='N' then 'N-未核'   ");
                sb.Append(" when b.AFA_APPROVE_STATUS='B' then 'B-駁回'");
                sb.Append(" when b.AFA_APPROVE_STATUS='Y' then 'B-已核' end AFA_APPROVE_STATUS,");
                sb.Append(" isnull(AFA_APPROVE_MARK,'') AFA_APPROVE_MARK,EMP_ID,EMP_NAME,c.PJOB_CD+'-'+c.PJOB_DESC PJOB_CD,a.LEVEL_CD,AFA_AMT,");
                sb.Append(" case when AFA_APPROVE_MARK = 'V' then b.AFA_REMARK else '' end AFA_REMARK");
                sb.Append(" from TB_S_R_FESTIVAL_D a");
                sb.Append(" left join  TB_S_M_FESTIVAL_H b on a.FESTIVAL_TYPE = b.FESTIVAL_TYPE and a.FESTIVAL_DT = b.FESTIVAL_DT and a.EMP_CD = b.EMP_CD");
                sb.Append(" left join TB_H_M_PJOB c on a.PJOB_CD = c.PJOB_CD and START_DT <= GETDATE() and END_DT > GETDATE()");
                sb.Append(" where b.FESTIVAL_TYPE  = @KEY1 and b.FESTIVAL_DT = @KEY2  and a.AFA_AMT <> 0");
            }

            ht.Add("@KEY1", KEY1);
            ht.Add("@KEY2", KEY2);


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
    public int getCount(int startRowIndex, int maximumRows, string SEARCH_YEAR, string SEARCH_AFA_FOR)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();


            string[] lines = Regex.Split(SEARCH_AFA_FOR, ":");
            TYPE = lines[0];//哪一種獎金
            KEY1 = lines[1];
            KEY2 = lines[2];
            if (TYPE == "a")
            {
                sb.Append(" Select COUNT(*) total_record");      
                sb.Append(" from TB_S_M_AWARD_D a");
                sb.Append(" left join  TB_S_M_AWARD_H b on a.AWARD_YEAR = b.AWARD_YEAR and a.AWARD_ROUND = b.AWARD_ROUND");
                sb.Append(" left join TB_H_M_PJOB c on a.PJOB_CD = c.PJOB_CD and START_DT <= GETDATE() and END_DT > GETDATE()");
                sb.Append(" where b.AWARD_YEAR  = @KEY1 and b.AWARD_ROUND = @KEY2  and a.AFA_AMT <> 0");
            }
            if (TYPE == "b")
            {
                sb.Append(" Select COUNT(*) total_record");      
                sb.Append(" from TB_S_R_BONUS_D a");
                sb.Append(" left join  TB_S_M_BONUS_H b on a.BONUS_YEAR = b.BONUS_YEAR and a.BONUS_ROUND = b.BONUS_ROUND");
                sb.Append(" left join TB_H_M_PJOB c on a.PJOB_CD = c.PJOB_CD and START_DT <= GETDATE() and END_DT > GETDATE()");
                sb.Append(" where b.BONUS_YEAR  = @KEY1 and b.BONUS_ROUND = @KEY2  and a.AFA_AMT <> 0");
            }
            if (TYPE == "c")
            {
                sb.Append(" Select COUNT(*) total_record");              
                sb.Append(" from TB_S_R_FESTIVAL_D a");
                sb.Append(" left join  TB_S_M_FESTIVAL_H b on a.FESTIVAL_TYPE = b.FESTIVAL_TYPE and a.FESTIVAL_DT = b.FESTIVAL_DT and a.EMP_CD = b.EMP_CD");
                sb.Append(" left join TB_H_M_PJOB c on a.PJOB_CD = c.PJOB_CD and START_DT <= GETDATE() and END_DT > GETDATE()");
                sb.Append(" where b.FESTIVAL_TYPE  = @KEY1 and b.FESTIVAL_DT = @KEY2  and a.AFA_AMT <> 0");
            }

            ht.Add("@KEY1", KEY1);
            ht.Add("@KEY2", KEY2);

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

    public DataTable afa_for_Data()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select CONVERT(varchar,AWARD_YEAR) + ' 年獎' + AWARD_ROUND + '回' as showWord ,'a:' + CONVERT(varchar,AWARD_YEAR)+ ':'  + AWARD_ROUND as id from TB_S_M_AWARD_H    ");
            sb.Append(" where AWARD_YEAR = @year ");
            sb.Append(" union all ");
            sb.Append("select CONVERT(varchar,BONUS_YEAR)+ ' 紅利' + BONUS_ROUND + '回'  as showWord,'b:' + CONVERT(varchar,BONUS_YEAR)+ ':'  + BONUS_ROUND as id from TB_S_M_BONUS_H     ");
            sb.Append("where BONUS_YEAR = @year ");
            sb.Append("union all ");
            sb.Append("select distinct(convert(varchar,FESTIVAL_DT,111))+ ' 一時金' as showWord,'c:' + FESTIVAL_TYPE + ':' + convert(varchar,FESTIVAL_DT,111) as id from TB_S_M_FESTIVAL_H ");
            sb.Append("where left(convert(varchar,FESTIVAL_DT,112),4)  = @year ");
            sb.Append("and FESTIVAL_TYPE = '3'");

            ht.Add("@year", YEAR);

            DataTable dt = dbConn.Query(sb, ht);


            return dt;
        }
        catch
        {
            throw;
        }
    }

    public DataTable search_afa_for_Data()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select CONVERT(varchar,AWARD_YEAR) + ' 年獎' + AWARD_ROUND + '回' as showWord ,'a:' + CONVERT(varchar,AWARD_YEAR)+ ':'  + AWARD_ROUND as id from TB_S_M_AWARD_H    ");
            sb.Append(" where AWARD_YEAR = @year  and IS_AFA = 'Y'");
            sb.Append(" union all ");
            sb.Append("select CONVERT(varchar,BONUS_YEAR)+ ' 紅利' + BONUS_ROUND + '回'  as showWord,'b:' + CONVERT(varchar,BONUS_YEAR)+ ':'  + BONUS_ROUND as id from TB_S_M_BONUS_H     ");
            sb.Append("where BONUS_YEAR = @year  and IS_AFA = 'Y'");
            sb.Append("union all ");
            sb.Append("select distinct(convert(varchar,FESTIVAL_DT,111))+ ' 一時金' as showWord,'c:' + FESTIVAL_TYPE + ':' + convert(varchar,FESTIVAL_DT,111) as id from TB_S_M_FESTIVAL_H ");
            sb.Append("where left(convert(varchar,FESTIVAL_DT,112),4)  = @year ");
            sb.Append("and FESTIVAL_TYPE = '3'  and IS_AFA = 'Y'");

            ht.Add("@year", YEAR);

            DataTable dt = dbConn.Query(sb, ht);


            return dt;
        }
        catch
        {
            throw;
        }
    }

    public DataTable is_AWARD_approve()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select isnull(APPROVE_BY,'') APPROVE_BY,isnull(SALARY_TRANS_BY,'') SALARY_TRANS_BY from TB_S_M_AWARD_H ");
            sb.Append(" where AWARD_YEAR = @KEY1  and AWARD_ROUND = @KEY2");

            ht.Add("@KEY1", KEY1);
            ht.Add("@KEY2", KEY2);

            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch
        {
            throw;
        }
    }

    public DataTable is_BONUS_approve()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select isnull(APPROVE_BY,'') APPROVE_BY,isnull(SALARY_TRANS_BY,'') SALARY_TRANS_BY from TB_S_M_BONUS_H ");
            sb.Append(" where BONUS_YEAR = @KEY1  and BONUS_ROUND = @KEY2");

            ht.Add("@KEY1", KEY1);
            ht.Add("@KEY2", KEY2);

            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch
        {
            throw;
        }
    }
    public DataTable is_FESTIVAL_approve()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select isnull(APPROVE_BY,'') APPROVE_BY,isnull(SALARY_TRANS_BY,'') SALARY_TRANS_BY from TB_S_M_FESTIVAL_H ");
            sb.Append(" where FESTIVAL_TYPE = @KEY1  and FESTIVAL_DT = @KEY2");

            ht.Add("@KEY1", KEY1);
            ht.Add("@KEY2", KEY2);

            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch
        {
            throw;
        }
    }

    public DataTable update_TB_S_M_AWARD_D() 
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("update TB_S_M_AWARD_D ");
            sb.Append(" set AFA_AMT = @AFA_AMT , TOTAL_AFA_AMT = @AFA_AMT + AWARD_AMT, AFA_APPROVE_MARK = '', ");
            sb.Append(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = getdate(),FUNC_ID = @FUNC_ID");
            sb.Append(" where AWARD_YEAR = @KEY1  and AWARD_ROUND = @KEY2 and EMP_ID = @EMP_ID");

            ht.Add("@AFA_AMT", AFA_AMOUNT);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            ht.Add("@KEY1", KEY1);
            ht.Add("@KEY2", KEY2);
            ht.Add("@EMP_ID", EMP_ID);

            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch
        {
            throw;
        }
    }
    public DataTable update_TB_S_M_AWARD_H()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("update TB_S_M_AWARD_H ");
            sb.Append(" set AFA_RELEASE_DT=getdate(),AFA_RELEASE_BY = @AFA_RELEASE_BY,AFA_APPROVE_DT = @AFA_APPROVE_DT,AFA_APPROVE_BY = @AFA_APPROVE_BY,");
            sb.Append(" AFA_APPROVE_STATUS=@AFA_APPROVE_STATUS,AFA_REMARK = @AFA_REMARK,IS_AFA = @IS_AFA,AFA_TOTAL_MONEY=@AFA_TOTAL_MONEY,AFA_TOTAL_PEOPLE = @AFA_TOTAL_PEOPLE,");
            sb.Append(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = getdate(),FUNC_ID = @FUNC_ID");
            sb.Append(" where AWARD_YEAR = @KEY1  and AWARD_ROUND = @KEY2 ");

            ht.Add("@AFA_RELEASE_BY", UPDATED_BY);
            ht.Add("@AFA_APPROVE_DT", DBNull.Value);
            ht.Add("@AFA_APPROVE_BY", "");
            ht.Add("@AFA_APPROVE_STATUS", "N");
            ht.Add("@AFA_REMARK", "");
            ht.Add("@IS_AFA", "Y");
            ht.Add("@AFA_TOTAL_MONEY", AFA_TOTAL_AMOUNT);
            ht.Add("@AFA_TOTAL_PEOPLE", AFA_TOTAL_PEOPLE);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            ht.Add("@KEY1", KEY1);
            ht.Add("@KEY2", KEY2);
            ht.Add("@EMP_ID", EMP_ID);

            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch
        {
            throw;
        }
    }
    public DataTable update_TB_S_R_BONUS_D()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("update TB_S_R_BONUS_D ");
            sb.Append(" set AFA_AMT = @AFA_AMT , TOTAL_AFA_AMT = @AFA_AMT + BONUS_AMT, AFA_APPROVE_MARK = '', ");
            sb.Append(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = getdate(),FUNC_ID = @FUNC_ID");
            sb.Append(" where BONUS_YEAR = @KEY1  and BONUS_COUNT = @KEY2 and EMP_ID = @EMP_ID");

            ht.Add("@AFA_AMT", AFA_AMOUNT);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            ht.Add("@KEY1", KEY1);
            ht.Add("@KEY2", KEY2);
            ht.Add("@EMP_ID", EMP_ID);

            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch
        {
            throw;
        }
    }
    public DataTable update_TB_S_M_BONUS_H() 
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("update TB_S_M_BONUS_H ");
            sb.Append(" set AFA_RELEASE_DT=getdate(),AFA_RELEASE_BY = @AFA_RELEASE_BY,AFA_APPROVE_DT = @AFA_APPROVE_DT,AFA_APPROVE_BY = @AFA_APPROVE_BY,");
            sb.Append(" AFA_APPROVE_STATUS=@AFA_APPROVE_STATUS,AFA_REMARK = @AFA_REMARK,IS_AFA = @IS_AFA,AFA_TOTAL_MONEY=@AFA_TOTAL_MONEY,AFA_TOTAL_PEOPLE = @AFA_TOTAL_PEOPLE,");
            sb.Append(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = getdate(),FUNC_ID = @FUNC_ID");
            sb.Append(" where BONUS_YEAR = @KEY1  and BONUS_ROUND = @KEY2 ");

            ht.Add("@AFA_RELEASE_BY", UPDATED_BY);
            ht.Add("@AFA_APPROVE_DT", DBNull.Value);
            ht.Add("@AFA_APPROVE_BY", "");
            ht.Add("@AFA_APPROVE_STATUS", "N");
            ht.Add("@AFA_REMARK", "");
            ht.Add("@IS_AFA", "Y");
            ht.Add("@AFA_TOTAL_MONEY", AFA_TOTAL_AMOUNT);
            ht.Add("@AFA_TOTAL_PEOPLE", AFA_TOTAL_PEOPLE);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            ht.Add("@KEY1", KEY1);
            ht.Add("@KEY2", KEY2);
            ht.Add("@EMP_ID", EMP_ID);

            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch
        {
            throw;
        }
    }
    public DataTable update_TB_S_R_FESTIVAL_D()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("update TB_S_R_FESTIVAL_D ");
            sb.Append(" set AFA_AMT = @AFA_AMT , TOTAL_AFA_AMT = @AFA_AMT + FESTIVAL_AMT, AFA_APPROVE_MARK = '',");
            sb.Append(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = getdate(),FUNC_ID = @FUNC_ID");
            sb.Append(" where FESTIVAL_TYPE = @KEY1  and FESTIVAL_DT = @KEY2 and EMP_ID = @EMP_ID");

            ht.Add("@AFA_AMT", AFA_AMOUNT);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            ht.Add("@KEY1", KEY1);
            ht.Add("@KEY2", KEY2);
            ht.Add("@EMP_ID", EMP_ID);

            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch
        {
            throw;
        }
    }
    public DataTable update_TB_S_M_FESTIVAL_H()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("update TB_S_M_FESTIVAL_H ");
            sb.Append(" set AFA_RELEASE_DT=getdate(),AFA_RELEASE_BY = @AFA_RELEASE_BY,AFA_APPROVE_DT = @AFA_APPROVE_DT,AFA_APPROVE_BY = @AFA_APPROVE_BY,");
            sb.Append(" AFA_APPROVE_STATUS=@AFA_APPROVE_STATUS,AFA_REMARK = @AFA_REMARK,IS_AFA = @IS_AFA,AFA_TOTAL_MONEY=@AFA_TOTAL_MONEY,AFA_TOTAL_PEOPLE = @AFA_TOTAL_PEOPLE,");
            sb.Append(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = getdate(),FUNC_ID = @FUNC_ID");
            sb.Append(" where FESTIVAL_TYPE = @KEY1  and FESTIVAL_DT = @KEY2 ");

            ht.Add("@AFA_RELEASE_BY", UPDATED_BY);
            ht.Add("@AFA_APPROVE_DT", DBNull.Value);
            ht.Add("@AFA_APPROVE_BY", "");
            ht.Add("@AFA_APPROVE_STATUS", "N");
            ht.Add("@AFA_REMARK", "");
            ht.Add("@IS_AFA", "Y");
            ht.Add("@AFA_TOTAL_MONEY", AFA_TOTAL_AMOUNT);
            ht.Add("@AFA_TOTAL_PEOPLE", AFA_TOTAL_PEOPLE);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            ht.Add("@KEY1", KEY1);
            ht.Add("@KEY2", KEY2);
            ht.Add("@EMP_ID", EMP_ID);

            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch
        {
            throw;
        }
    }
    public DataTable is_Exist_Emp_Id_AWARD()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select count(*)rows from TB_S_M_AWARD_D ");
            sb.Append(" where AWARD_YEAR = @KEY1  and AWARD_ROUND = @KEY2 and EMP_ID = @EMP_ID");

            ht.Add("@KEY1", KEY1);
            ht.Add("@KEY2", KEY2);
            ht.Add("@EMP_ID", EMP_ID);

            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch
        {
            throw;
        }
    }
    public DataTable is_Exist_Emp_Id_BONUS()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select count(*)rows from TB_S_R_BONUS_D ");
            sb.Append(" where BONUS_YEAR = @KEY1  and BONUS_COUNT = @KEY2 and EMP_ID = @EMP_ID");

            ht.Add("@KEY1", KEY1);
            ht.Add("@KEY2", KEY2);
            ht.Add("@EMP_ID", EMP_ID);

            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch
        {
            throw;
        }
    }
    public DataTable is_Exist_Emp_Id_FESTIVAL()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select count(*)rows from TB_S_R_FESTIVAL_D ");
            sb.Append(" where FESTIVAL_TYPE = @KEY1  and FESTIVAL_DT = @KEY2 and EMP_ID = @EMP_ID");

            ht.Add("@KEY1", KEY1);
            ht.Add("@KEY2", KEY2);
            ht.Add("@EMP_ID", EMP_ID);

            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch
        {
            throw;
        }
    }
    public DataTable id_match_name()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select count(*)rows from TB_H_M_EMP ");
            sb.Append(" where EMP_ID = @EMP_ID and EMP_NAME = @EMP_NAME");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@EMP_NAME", EMP_NAME);

            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch
        {
            throw;
        }
    }

    public DataTable clean_TB_S_M_AWARD_D()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("update TB_S_M_AWARD_D ");
            sb.Append(" set AFA_AMT = 0 , TOTAL_AFA_AMT = 0, AFA_APPROVE_MARK = '', ");
            sb.Append(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = getdate(),FUNC_ID = @FUNC_ID");
            sb.Append(" where AWARD_YEAR = @KEY1  and AWARD_ROUND = @KEY2 ");
                       
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            ht.Add("@KEY1", KEY1);
            ht.Add("@KEY2", KEY2);            

            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch
        {
            throw;
        }
    }
    public DataTable clean_TB_S_R_BONUS_D()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("update TB_S_R_BONUS_D ");
            sb.Append(" set AFA_AMT = 0 , TOTAL_AFA_AMT = 0, AFA_APPROVE_MARK = '', ");
            sb.Append(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = getdate(),FUNC_ID = @FUNC_ID");
            sb.Append(" where BONUS_YEAR = @KEY1  and BONUS_COUNT = @KEY2 ");
                        
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            ht.Add("@KEY1", KEY1);
            ht.Add("@KEY2", KEY2);
            

            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch
        {
            throw;
        }
    }
    public DataTable clean_TB_S_R_FESTIVAL_D()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("update TB_S_R_FESTIVAL_D ");
            sb.Append(" set AFA_AMT = 0 , TOTAL_AFA_AMT = 0, AFA_APPROVE_MARK = '',");
            sb.Append(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = getdate(),FUNC_ID = @FUNC_ID");
            sb.Append(" where FESTIVAL_TYPE = @KEY1  and FESTIVAL_DT = @KEY2 ");
                        
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            ht.Add("@KEY1", KEY1);
            ht.Add("@KEY2", KEY2);
            ht.Add("@EMP_ID", EMP_ID);

            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch
        {
            throw;
        }
    }
    public DataTable clean_TB_S_M_AWARD_H()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("update TB_S_M_AWARD_H ");
            sb.Append(" set AFA_RELEASE_DT=@AFA_RELEASE_DT,AFA_RELEASE_BY = @AFA_RELEASE_BY,AFA_APPROVE_DT = @AFA_APPROVE_DT,AFA_APPROVE_BY = @AFA_APPROVE_BY,");
            sb.Append(" AFA_APPROVE_STATUS=@AFA_APPROVE_STATUS,AFA_REMARK = @AFA_REMARK,IS_AFA = @IS_AFA,AFA_TOTAL_MONEY=@AFA_TOTAL_MONEY,AFA_TOTAL_PEOPLE = @AFA_TOTAL_PEOPLE,");
            sb.Append(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = getdate(),FUNC_ID = @FUNC_ID");
            sb.Append(" where AWARD_YEAR = @KEY1  and AWARD_ROUND = @KEY2 ");

            ht.Add("@AFA_RELEASE_DT", DBNull.Value);
            ht.Add("@AFA_RELEASE_BY", "");
            ht.Add("@AFA_APPROVE_DT", DBNull.Value);
            ht.Add("@AFA_APPROVE_BY", "");
            ht.Add("@AFA_APPROVE_STATUS", "N");
            ht.Add("@AFA_REMARK", "");
            ht.Add("@IS_AFA", "N");
            ht.Add("@AFA_TOTAL_MONEY", 0);
            ht.Add("@AFA_TOTAL_PEOPLE", 0);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            ht.Add("@KEY1", KEY1);
            ht.Add("@KEY2", KEY2);            

            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch
        {
            throw;
        }
    }
    public DataTable clean_TB_S_M_BONUS_H()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("update TB_S_M_BONUS_H ");
            sb.Append(" set AFA_RELEASE_DT=@AFA_RELEASE_DT,AFA_RELEASE_BY = @AFA_RELEASE_BY,AFA_APPROVE_DT = @AFA_APPROVE_DT,AFA_APPROVE_BY = @AFA_APPROVE_BY,");
            sb.Append(" AFA_APPROVE_STATUS=@AFA_APPROVE_STATUS,AFA_REMARK = @AFA_REMARK,IS_AFA = @IS_AFA,AFA_TOTAL_MONEY=@AFA_TOTAL_MONEY,AFA_TOTAL_PEOPLE = @AFA_TOTAL_PEOPLE,");
            sb.Append(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = getdate(),FUNC_ID = @FUNC_ID");
            sb.Append(" where BONUS_YEAR = @KEY1  and BONUS_ROUND = @KEY2 ");

            ht.Add("@AFA_RELEASE_DT", DBNull.Value);
            ht.Add("@AFA_RELEASE_BY", "");
            ht.Add("@AFA_APPROVE_DT", DBNull.Value);
            ht.Add("@AFA_APPROVE_BY", "");
            ht.Add("@AFA_APPROVE_STATUS", "N");
            ht.Add("@AFA_REMARK", "");
            ht.Add("@IS_AFA", "N");
            ht.Add("@AFA_TOTAL_MONEY", 0);
            ht.Add("@AFA_TOTAL_PEOPLE", 0);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            ht.Add("@KEY1", KEY1);
            ht.Add("@KEY2", KEY2);            

            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch
        {
            throw;
        }
    }
    public DataTable clean_TB_S_M_FESTIVAL_H()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("update TB_S_M_FESTIVAL_H ");
            sb.Append(" set AFA_RELEASE_DT=@AFA_RELEASE_DT,AFA_RELEASE_BY = @AFA_RELEASE_BY,AFA_APPROVE_DT = @AFA_APPROVE_DT,AFA_APPROVE_BY = @AFA_APPROVE_BY,");
            sb.Append(" AFA_APPROVE_STATUS=@AFA_APPROVE_STATUS,AFA_REMARK = @AFA_REMARK,IS_AFA = @IS_AFA,AFA_TOTAL_MONEY=@AFA_TOTAL_MONEY,AFA_TOTAL_PEOPLE = @AFA_TOTAL_PEOPLE,");
            sb.Append(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = getdate(),FUNC_ID = @FUNC_ID");
            sb.Append(" where FESTIVAL_TYPE = @KEY1  and FESTIVAL_DT = @KEY2 ");

            ht.Add("@AFA_RELEASE_DT", DBNull.Value);
            ht.Add("@AFA_RELEASE_BY", "");
            ht.Add("@AFA_APPROVE_DT", DBNull.Value);
            ht.Add("@AFA_APPROVE_BY", "");
            ht.Add("@AFA_APPROVE_STATUS", "N");
            ht.Add("@AFA_REMARK", "");
            ht.Add("@IS_AFA", "N");
            ht.Add("@AFA_TOTAL_MONEY", 0);
            ht.Add("@AFA_TOTAL_PEOPLE", 0);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            ht.Add("@KEY1", KEY1);
            ht.Add("@KEY2", KEY2);     

            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch
        {
            throw;
        }
    }


}