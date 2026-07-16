using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;
using System.Text.RegularExpressions;

/// <summary>
/// CFB2SN0200DAO 的摘要描述
/// </summary>
public class CFB2SN0200DAO : BaseDAO
{
    //畫面
    public string YEAR { get; set; }

    public string TYPE { get; set; }
    public string KEY1 { get; set; }
    public string KEY2 { get; set; }

    public string EMP_ID { get; set; }
    public string AFA_REMARK { get; set; }
    public string AFA_APPROVE_BY { get; set; }
    
    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }


    public CFB2SN0200DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }


    #region 資料取得
    //下拉選單
    public DataTable afa_for_Data()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select CONVERT(varchar,AWARD_YEAR) + ' 年獎' + AWARD_ROUND + '回' as showWord ,'a:' + CONVERT(varchar,AWARD_YEAR)+ ':'  + AWARD_ROUND as id from TB_S_M_AWARD_H    ");
            sb.Append(" where AWARD_YEAR = @year and IS_AFA = 'Y'");
            sb.Append(" union all ");
            sb.Append("select CONVERT(varchar,BONUS_YEAR)+ ' 紅利' + BONUS_ROUND + '回'  as showWord,'b:' + CONVERT(varchar,BONUS_YEAR)+ ':'  + BONUS_ROUND as id from TB_S_M_BONUS_H     ");
            sb.Append("where BONUS_YEAR = @year and IS_AFA = 'Y' ");
            sb.Append("union all ");
            sb.Append("select distinct(convert(varchar,FESTIVAL_DT,111))+ ' 一時金' as showWord,'c:' + FESTIVAL_TYPE + ':' + convert(varchar,FESTIVAL_DT,111) as id from TB_S_M_FESTIVAL_H ");
            sb.Append("where left(convert(varchar,FESTIVAL_DT,112),4)  = @year ");
            sb.Append("and FESTIVAL_TYPE = '3' and IS_AFA = 'Y'");

            ht.Add("@year", YEAR);

            DataTable dt = dbConn.Query(sb, ht);


            return dt;
        }
        catch
        {
            throw;
        }
    }

    public DataTable getTitle_Data()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            if (TYPE == "a")
            {
                sb.Append(" Select CONVERT(varchar(10),AWARD_DT, 111) PAY_DT,AFA_TOTAL_MONEY,AFA_TOTAL_PEOPLE,AFA_REMARK");
                sb.Append(" from TB_S_M_AWARD_H");
                sb.Append(" where AWARD_DT  = @KEY1 and AWARD_ROUND = @KEY2");
            }
            else if (TYPE == "b")
            {
                sb.Append(" Select CONVERT(varchar(10),BONUS_DT, 111) PAY_DT,AFA_TOTAL_MONEY,AFA_TOTAL_PEOPLE,AFA_REMARK");
                sb.Append(" from TB_S_M_BONUS_H");
                sb.Append(" where BONUS_YEAR  = @KEY1 and BONUS_ROUND = @KEY2");
            }
            else
            {
                sb.Append(" Select CONVERT(varchar(10),FESTIVAL_PAY_DT, 111) PAY_DT,AFA_TOTAL_MONEY,AFA_TOTAL_PEOPLE,AFA_REMARK");
                sb.Append(" from TB_S_M_FESTIVAL_H");
                sb.Append(" where FESTIVAL_TYPE  = @KEY1 and FESTIVAL_DT = @KEY2");
            }


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

    public DataTable check_Approve_Data()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            if (TYPE == "a")
            {
                sb.Append(" Select isnull(AFA_APPROVE_BY,'')AFA_APPROVE_BY");
                sb.Append(" from TB_S_M_AWARD_H");
                sb.Append(" where AWARD_DT  = @KEY1 and AWARD_ROUND = @KEY2");
            }
            else if (TYPE == "b")
            {
                sb.Append(" Select isnull(AFA_APPROVE_BY,'')AFA_APPROVE_BY");
                sb.Append(" from TB_S_M_BONUS_H");
                sb.Append(" where BONUS_YEAR  = @KEY1 and BONUS_ROUND = @KEY2");
            }
            else
            {
                sb.Append(" Select isnull(AFA_APPROVE_BY,'')AFA_APPROVE_BY");
                sb.Append(" from TB_S_M_FESTIVAL_H");
                sb.Append(" where FESTIVAL_TYPE  = @KEY1 and FESTIVAL_DT = @KEY2");
            }


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

    public DataTable check_Status()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            if (TYPE == "a")
            {
                sb.Append(" Select AFA_APPROVE_STATUS");
                sb.Append(" from TB_S_M_AWARD_H");
                sb.Append(" where AWARD_DT  = @KEY1 and AWARD_ROUND = @KEY2");
            }
            else if (TYPE == "b")
            {
                sb.Append(" Select AFA_APPROVE_STATUS");
                sb.Append(" from TB_S_M_BONUS_H");
                sb.Append(" where BONUS_YEAR  = @KEY1 and BONUS_ROUND = @KEY2");
            }
            else
            {
                sb.Append(" Select AFA_APPROVE_STATUS");
                sb.Append(" from TB_S_M_FESTIVAL_H");
                sb.Append(" where FESTIVAL_TYPE  = @KEY1 and FESTIVAL_DT = @KEY2");
            }


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

        #endregion

    #region Gridview 資料
    //Gridview 查詢資料
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string YEAR, string AFA_FOR)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            if (AFA_FOR != "-1") // 有阿法值對像
            {
                string[] lines = Regex.Split(AFA_FOR, ":");
                TYPE = lines[0];//哪一種獎金
                KEY1 = lines[1];
                KEY2 = lines[2];

                if (TYPE == "a")
                {
                    sb.Append(" Select * From");
                    sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY AWARD_YEAR) As RowNumber,");
                    sb.Append("  CONVERT(varchar,AWARD_YEAR) + ' 年獎' + AWARD_ROUND + '回' as AFA_FOR,'a:' + CONVERT(varchar,AWARD_YEAR)+ ':'  + AWARD_ROUND as keyWord,");
                    sb.Append(" case when AFA_APPROVE_STATUS='N' then 'N-未核'   ");
                    sb.Append(" when AFA_APPROVE_STATUS='B' then 'B-駁回'");
                    sb.Append(" when AFA_APPROVE_STATUS='Y' then 'B-已核' end AFA_APPROVE_STATUS,");
                    sb.Append(" AFA_RELEASE_BY,convert(varchar,AFA_RELEASE_DT,111)AFA_RELEASE_DT,AFA_APPROVE_BY,convert(varchar,AFA_APPROVE_DT,111) AFA_APPROVE_DT,convert(varchar,SALARY_TRANS_DT,111)SALARY_TRANS_DT");
                    sb.Append(" from TB_S_M_AWARD_H ");
                    sb.Append(" where AWARD_YEAR  = @KEY1 and AWARD_ROUND = @KEY2  and IS_AFA = 'Y'");
                }
                if (TYPE == "b")
                {
                    sb.Append(" Select * From");
                    sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY BONUS_YEAR) As RowNumber,");
                    sb.Append("  CONVERT(varchar,BONUS_YEAR)+ ' 紅利' + BONUS_ROUND + '回'  as AFA_FOR,'b:' + CONVERT(varchar,BONUS_YEAR)+ ':'  + BONUS_ROUND as keyWord,");
                    sb.Append(" case when AFA_APPROVE_STATUS='N' then 'N-未核'   ");
                    sb.Append(" when AFA_APPROVE_STATUS='B' then 'B-駁回'");
                    sb.Append(" when AFA_APPROVE_STATUS='Y' then 'B-已核' end AFA_APPROVE_STATUS,");
                    sb.Append(" AFA_RELEASE_BY,convert(varchar,AFA_RELEASE_DT,111)AFA_RELEASE_DT,AFA_APPROVE_BY,convert(varchar,AFA_APPROVE_DT,111) AFA_APPROVE_DT,convert(varchar,SALARY_TRANS_DT,111)SALARY_TRANS_DT");
                    sb.Append(" from TB_S_M_BONUS_H ");
                    sb.Append(" where BONUS_YEAR  = @KEY1 and BONUS_ROUND = @KEY2  and IS_AFA = 'Y'");
                }
                if (TYPE == "c")
                {
                    sb.Append(" Select * From");
                    sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY FESTIVAL_DT) As RowNumber,");
                    sb.Append("  convert(varchar,FESTIVAL_DT,111)+ ' 一時金' as AFA_FOR,'c:' + FESTIVAL_TYPE + ':' + convert(varchar,FESTIVAL_DT,111) as keyWord,");
                    sb.Append(" case when AFA_APPROVE_STATUS='N' then 'N-未核'   ");
                    sb.Append(" when AFA_APPROVE_STATUS='B' then 'B-駁回'");
                    sb.Append(" when AFA_APPROVE_STATUS='Y' then 'B-已核' end AFA_APPROVE_STATUS,");
                    sb.Append(" AFA_RELEASE_BY,convert(varchar,AFA_RELEASE_DT,111)AFA_RELEASE_DT,AFA_APPROVE_BY,convert(varchar,AFA_APPROVE_DT,111) AFA_APPROVE_DT,convert(varchar,SALARY_TRANS_DT,111)SALARY_TRANS_DT");
                    sb.Append(" from TB_S_M_FESTIVAL_H ");
                    sb.Append(" where FESTIVAL_TYPE  = @KEY1 and FESTIVAL_DT = @KEY2  and IS_AFA = 'Y'");
                    sb.Append(" group by FESTIVAL_DT,FESTIVAL_TYPE,AFA_APPROVE_STATUS,AFA_RELEASE_BY,AFA_RELEASE_DT,AFA_APPROVE_BY, AFA_APPROVE_DT,SALARY_TRANS_DT ");
                }

                ht.Add("@KEY1", KEY1);
                ht.Add("@KEY2", KEY2);


                sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
                sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar)");


               
            }
            else // 只有阿法值年度
            {
                sb.Append(" Select * from (");
                sb.Append(" Select ROW_NUMBER() OVER(ORDER BY AFA_FOR) As RowNumber,a.AFA_FOR,a.keyWord,a.AFA_APPROVE_STATUS,a.AFA_RELEASE_BY,a.AFA_RELEASE_DT,");
                sb.Append(" a.AFA_APPROVE_BY,a.AFA_APPROVE_DT,a.SALARY_TRANS_DT from (");
                sb.Append(" select CONVERT(varchar,AWARD_YEAR) + ' 年獎' + AWARD_ROUND + '回' as AFA_FOR,'a:' + CONVERT(varchar,AWARD_YEAR)+ ':'  + AWARD_ROUND as keyWord,");
                sb.Append(" case when AFA_APPROVE_STATUS='N' then 'N-未核'   ");
                sb.Append(" when AFA_APPROVE_STATUS='B' then 'B-駁回'");
                sb.Append(" when AFA_APPROVE_STATUS='Y' then 'B-已核' end AFA_APPROVE_STATUS,");
                sb.Append(" AFA_RELEASE_BY,convert(varchar,AFA_RELEASE_DT,111)AFA_RELEASE_DT,AFA_APPROVE_BY,convert(varchar,AFA_APPROVE_DT,111) AFA_APPROVE_DT,convert(varchar,SALARY_TRANS_DT,111)SALARY_TRANS_DT");
                sb.Append(" from TB_S_M_AWARD_H");
                sb.Append(" where AWARD_YEAR  = @YEAR");
                sb.Append(" and IS_AFA = 'Y'");
                sb.Append(" UNION ALL");
                sb.Append(" select  CONVERT(varchar,BONUS_YEAR)+ ' 紅利' + BONUS_ROUND + '回'  as AFA_FOR,'b:' + CONVERT(varchar,BONUS_YEAR)+ ':'  + BONUS_ROUND as keyWord,");
                sb.Append(" case when AFA_APPROVE_STATUS='N' then 'N-未核'   ");
                sb.Append(" when AFA_APPROVE_STATUS='B' then 'B-駁回'");
                sb.Append(" when AFA_APPROVE_STATUS='Y' then 'B-已核' end AFA_APPROVE_STATUS,");
                sb.Append(" AFA_RELEASE_BY,convert(varchar,AFA_RELEASE_DT,111)AFA_RELEASE_DT,AFA_APPROVE_BY,convert(varchar,AFA_APPROVE_DT,111) AFA_APPROVE_DT,convert(varchar,SALARY_TRANS_DT,111)SALARY_TRANS_DT");
                sb.Append(" from TB_S_M_BONUS_H");
                sb.Append(" where BONUS_YEAR  = @YEAR and IS_AFA = 'Y'");
                sb.Append(" UNION ALL");
                sb.Append(" Select convert(varchar,FESTIVAL_DT,111)+ ' 一時金' as AFA_FOR,'c:' + FESTIVAL_TYPE + ':' + convert(varchar,FESTIVAL_DT,111) as keyWord,");
                sb.Append(" case when AFA_APPROVE_STATUS='N' then 'N-未核'   ");
                sb.Append(" when AFA_APPROVE_STATUS='B' then 'B-駁回'");
                sb.Append(" when AFA_APPROVE_STATUS='Y' then 'B-已核' end AFA_APPROVE_STATUS,");
                sb.Append(" AFA_RELEASE_BY,convert(varchar,AFA_RELEASE_DT,111)AFA_RELEASE_DT,AFA_APPROVE_BY,convert(varchar,AFA_APPROVE_DT,111) AFA_APPROVE_DT,convert(varchar,SALARY_TRANS_DT,111)SALARY_TRANS_DT");
                sb.Append(" from TB_S_M_FESTIVAL_H");
                sb.Append(" where left(convert(varchar,FESTIVAL_DT,112),4)  = @YEAR and FESTIVAL_TYPE = '3' and IS_AFA = 'Y'");
                sb.Append(" group by FESTIVAL_DT,FESTIVAL_TYPE,AFA_APPROVE_STATUS,AFA_RELEASE_BY,AFA_RELEASE_DT,");
                sb.Append(" AFA_APPROVE_BY, AFA_APPROVE_DT,SALARY_TRANS_DT");
                sb.Append(" )a");
                sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
                sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar)");

                ht.Add("@YEAR", YEAR);
 
            }

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
    public int getCount(int startRowIndex, int maximumRows, string YEAR, string AFA_FOR)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();


            if (AFA_FOR != "-1") // 有阿法值對像
            {
                string[] lines = Regex.Split(AFA_FOR, ":");
                TYPE = lines[0];//哪一種獎金
                KEY1 = lines[1];
                KEY2 = lines[2];

                if (TYPE == "a")
                {
                    sb.Append(" Select COUNT(*) total_record");                    
                    sb.Append(" from TB_S_M_AWARD_H ");
                    sb.Append(" where AWARD_YEAR  = @KEY1 and AWARD_ROUND = @KEY2  and IS_AFA = 'Y'");
                }
                if (TYPE == "b")
                {
                    sb.Append(" Select COUNT(*) total_record"); 
                    sb.Append(" from TB_S_M_BONUS_H ");
                    sb.Append(" where BONUS_YEAR  = @KEY1 and BONUS_ROUND = @KEY2  and IS_AFA = 'Y'");
                }
                if (TYPE == "c")
                {
                    sb.Append(" Select COUNT(*) total_record from (");
                    sb.Append(" Select * From");
                    sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY FESTIVAL_DT) As RowNumber,");
                    sb.Append("  convert(varchar,FESTIVAL_DT,111)+ ' 一時金' as AFA_FOR,'c:' + FESTIVAL_TYPE + ':' + convert(varchar,FESTIVAL_DT,111) as keyWord,");
                    sb.Append(" AFA_APPROVE_STATUS,AFA_RELEASE_BY,convert(varchar,AFA_RELEASE_DT,111)AFA_RELEASE_DT,AFA_APPROVE_BY,convert(varchar,AFA_APPROVE_DT,111) AFA_APPROVE_DT,convert(varchar,SALARY_TRANS_DT,111)SALARY_TRANS_DT");
                    sb.Append(" from TB_S_M_FESTIVAL_H ");
                    sb.Append(" where FESTIVAL_TYPE  = @KEY1 and FESTIVAL_DT = @KEY2  and IS_AFA = 'Y'");
                    sb.Append(" group by FESTIVAL_DT,FESTIVAL_TYPE,AFA_APPROVE_STATUS,AFA_RELEASE_BY,AFA_RELEASE_DT,AFA_APPROVE_BY, AFA_APPROVE_DT,SALARY_TRANS_DT ");
                    sb.Append(" )b");
                    sb.Append(" )god_data");
                }

                ht.Add("@KEY1", KEY1);
                ht.Add("@KEY2", KEY2);
            
            }
            else // 只有阿法值年度
            {
                sb.Append(" Select  COUNT(*) total_record from (");
                sb.Append(" Select ROW_NUMBER() OVER(ORDER BY AFA_FOR) As RowNumber,a.AFA_FOR,a.keyWord,a.AFA_APPROVE_STATUS,a.AFA_RELEASE_BY,a.AFA_RELEASE_DT,");
                sb.Append(" a.AFA_APPROVE_BY,a.AFA_APPROVE_DT,a.SALARY_TRANS_DT from (");
                sb.Append(" select CONVERT(varchar,AWARD_YEAR) + ' 年獎' + AWARD_ROUND + '回' as AFA_FOR,'a:' + CONVERT(varchar,AWARD_YEAR)+ ':'  + AWARD_ROUND as keyWord,");
                sb.Append(" AFA_APPROVE_STATUS,AFA_RELEASE_BY,convert(varchar,AFA_RELEASE_DT,111)AFA_RELEASE_DT,AFA_APPROVE_BY,convert(varchar,AFA_APPROVE_DT,111) AFA_APPROVE_DT,convert(varchar,SALARY_TRANS_DT,111)SALARY_TRANS_DT");
                sb.Append(" from TB_S_M_AWARD_H");
                sb.Append(" where AWARD_YEAR  = @YEAR");
                sb.Append(" and IS_AFA = 'Y'");
                sb.Append(" UNION ALL");
                sb.Append(" select  CONVERT(varchar,BONUS_YEAR)+ ' 紅利' + BONUS_ROUND + '回'  as AFA_FOR,'b:' + CONVERT(varchar,BONUS_YEAR)+ ':'  + BONUS_ROUND as keyWord,");
                sb.Append(" AFA_APPROVE_STATUS,AFA_RELEASE_BY,convert(varchar,AFA_RELEASE_DT,111)AFA_RELEASE_DT,AFA_APPROVE_BY,convert(varchar,AFA_APPROVE_DT,111) AFA_APPROVE_DT,convert(varchar,SALARY_TRANS_DT,111)SALARY_TRANS_DT");
                sb.Append(" from TB_S_M_BONUS_H");
                sb.Append(" where BONUS_YEAR  = @YEAR and IS_AFA = 'Y'");
                sb.Append(" UNION ALL");
                sb.Append(" Select convert(varchar,FESTIVAL_DT,111)+ ' 一時金' as AFA_FOR,'c:' + FESTIVAL_TYPE + ':' + convert(varchar,FESTIVAL_DT,111) as keyWord,");
                sb.Append(" AFA_APPROVE_STATUS,AFA_RELEASE_BY,convert(varchar,AFA_RELEASE_DT,111)AFA_RELEASE_DT,AFA_APPROVE_BY,convert(varchar,AFA_APPROVE_DT,111) AFA_APPROVE_DT,convert(varchar,SALARY_TRANS_DT,111)SALARY_TRANS_DT");
                sb.Append(" from TB_S_M_FESTIVAL_H");
                sb.Append(" where left(convert(varchar,FESTIVAL_DT,112),4)  = @YEAR and FESTIVAL_TYPE = '3' and IS_AFA = 'Y'");
                sb.Append(" group by FESTIVAL_DT,FESTIVAL_TYPE,AFA_APPROVE_STATUS,AFA_RELEASE_BY,AFA_RELEASE_DT,");
                sb.Append(" AFA_APPROVE_BY, AFA_APPROVE_DT,SALARY_TRANS_DT");
                sb.Append(" )a");
                sb.Append(" )god_data ");
                
                ht.Add("@YEAR", YEAR);

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

    public DataTable getDataDtl(int startRowIndex, int maximumRows, string AFA_FOR, string sortExpression)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            if (AFA_FOR.IndexOf(":") != -1) // 有阿法值對像
            {
                string[] lines = Regex.Split(AFA_FOR, ":");
                TYPE = lines[0];//哪一種獎金
                KEY1 = lines[1];
                KEY2 = lines[2];

                if (TYPE == "a")
                {
                    sb.Append(" Select * From");
                    sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
                    sb.Append(" isnull(AFA_APPROVE_MARK,'') AFA_APPROVE_MARK,b.AFA_APPROVE_STATUS,EMP_ID,EMP_NAME,c.PJOB_CD+'-'+c.PJOB_DESC PJOB_CD,a.LEVEL_CD,AFA_AMT ,b.AFA_REMARK");
                    sb.Append(" from TB_S_M_AWARD_D a");
                    sb.Append(" left join  TB_S_M_AWARD_H b on a.AWARD_YEAR = b.AWARD_YEAR and a.AWARD_ROUND = b.AWARD_ROUND");
                    sb.Append(" left join TB_H_M_PJOB c on a.PJOB_CD = c.PJOB_CD and START_DT <= GETDATE() and END_DT > GETDATE()");
                    sb.Append(" where b.AWARD_YEAR  = @KEY1 and b.AWARD_ROUND = @KEY2  and a.AFA_AMT <> 0");

                }
                if (TYPE == "b")
                {
                    sb.Append(" Select * From");
                    sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
                    sb.Append(" isnull(AFA_APPROVE_MARK,'') AFA_APPROVE_MARK,b.AFA_APPROVE_STATUS,EMP_ID,EMP_NAME,c.PJOB_CD+'-'+c.PJOB_DESC PJOB_CD,a.LEVEL_CD,AFA_AMT ,b.AFA_REMARK");
                    sb.Append(" from TB_S_R_BONUS_D a");
                    sb.Append(" left join  TB_S_M_BONUS_H b on a.BONUS_YEAR = b.BONUS_YEAR and a.BONUS_ROUND = b.BONUS_ROUND");
                    sb.Append(" left join TB_H_M_PJOB c on a.PJOB_CD = c.PJOB_CD and START_DT <= GETDATE() and END_DT > GETDATE()");
                    sb.Append(" where b.BONUS_YEAR  = @KEY1 and b.BONUS_ROUND = @KEY2  and a.AFA_AMT <> 0");
                }
                if (TYPE == "c")
                {
                    sb.Append(" Select * From");
                    sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
                    sb.Append(" isnull(AFA_APPROVE_MARK,'') AFA_APPROVE_MARK,b.AFA_APPROVE_STATUS,EMP_ID,EMP_NAME,c.PJOB_CD+'-'+c.PJOB_DESC PJOB_CD,a.LEVEL_CD,AFA_AMT ,b.AFA_REMARK");
                    sb.Append(" from TB_S_R_FESTIVAL_D a");
                    sb.Append(" left join  TB_S_M_FESTIVAL_H b on a.FESTIVAL_TYPE = b.FESTIVAL_TYPE and a.FESTIVAL_DT = b.FESTIVAL_DT and a.EMP_CD = b.EMP_CD");
                    sb.Append(" left join TB_H_M_PJOB c on a.PJOB_CD = c.PJOB_CD and START_DT <= GETDATE() and END_DT > GETDATE()");
                    sb.Append(" where b.FESTIVAL_TYPE  = @KEY1 and b.FESTIVAL_DT = @KEY2  and a.AFA_AMT <> 0");
                }
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
    public int getCountDtl(int startRowIndex, int maximumRows, string AFA_FOR)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();


            if (AFA_FOR.IndexOf(":") != -1) // 有阿法值對像
            {
                string[] lines = Regex.Split(AFA_FOR, ":");
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



    #region DB存取
    //刪除 
    public void deleteData(string type, string start_dt)
    {
        try
        {
            // string dt = Convert.ToDateTime(start_dt).ToString("yyyy/MM/dd HH:mm:ss.fff");
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("delete from TB_D_M_ENV_ALLOWANCE_TYPE ");
            //sb.Append(" where ENV_ALLOWANCE_TYPE = @ENV_ALLOWANCE_TYPE and Convert(char(10),START_DT,111) = @START_DT ");
            sb.Append(" where ENV_ALLOWANCE_TYPE = @ENV_ALLOWANCE_TYPE and START_DT = @START_DT ");
            ht.Add("@ENV_ALLOWANCE_TYPE", type);
            ht.Add("@START_DT", Convert.ToDateTime(start_dt).ToString("yyyy/MM/dd"));
            //ht.Add("@START_DT", start_dt);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //修改
    public void updateData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            DateTime now = DateTime.Parse(DateTime.Now.ToString());
            sb.Append(" update TB_D_M_ENV_ALLOWANCE_TYPE ");
            sb.Append(" set END_DT = @END_DT ");
            sb.Append(" ,ENV_ALLOWANCE_DESC = @ENV_ALLOWANCE_DESC ");
            sb.Append(" ,ENV_ALLOWANCE_VALUE = @ENV_ALLOWANCE_VALUE ");
            sb.Append(" ,ENV_MIN_UNIT = @ENV_MIN_UNIT ");
            sb.Append(" ,REMARK = @REMARK ");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT = @UPDATED_DT");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where ENV_ALLOWANCE_TYPE = @ENV_ALLOWANCE_TYPE and START_DT = @START_DT");

            //ht.Add("@END_DT", END_DT + " 23:59:59");
            //ht.Add("@ENV_ALLOWANCE_DESC", ENV_ALLOWANCE_DESC);
            //ht.Add("@ENV_ALLOWANCE_VALUE", ENV_ALLOWANCE_VALUE);
            //ht.Add("@ENV_MIN_UNIT", ENV_MIN_UNIT);
            //ht.Add("@REMARK", REMARK);
            //ht.Add("@UPDATED_BY", UPDATED_BY);
            //ht.Add("@UPDATED_DT", now);
            //ht.Add("@FUNC_ID", FUNC_ID);
            //ht.Add("@ENV_ALLOWANCE_TYPE", ENV_ALLOWANCE_TYPE);
            //ht.Add("@START_DT", Convert.ToDateTime(START_DT));

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }


    }

    //修改主檔
    public void updateMainData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            //目前的備註 是insert空白
            if (TYPE == "a")
            {
                sb.Append("update TB_S_M_AWARD_H ");
                sb.Append(" set AFA_REMARK='',AFA_APPROVE_DT = getdate(),AFA_APPROVE_BY = @AFA_APPROVE_BY,AFA_APPROVE_STATUS = 'Y',");                
                sb.Append(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = getdate(),FUNC_ID = @FUNC_ID");
                sb.Append(" where AWARD_YEAR = @KEY1  and AWARD_ROUND = @KEY2 ");                     
            }
            else if (TYPE == "b")
            {
                sb.Append("update TB_S_M_BONUS_H ");
                sb.Append(" set AFA_REMARK='',AFA_APPROVE_DT = getdate(),AFA_APPROVE_BY = @AFA_APPROVE_BY,AFA_APPROVE_STATUS = 'Y',");
                sb.Append(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = getdate(),FUNC_ID = @FUNC_ID");
                sb.Append(" where BONUS_YEAR = @KEY1  and BONUS_ROUND = @KEY2 ");                       
            }
            else
            {
                sb.Append("update TB_S_M_FESTIVAL_H ");
                sb.Append(" set AFA_REMARK='',AFA_APPROVE_DT = getdate(),AFA_APPROVE_BY = @AFA_APPROVE_BY,AFA_APPROVE_STATUS = 'Y',");
                sb.Append(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = getdate(),FUNC_ID = @FUNC_ID");
                sb.Append(" where FESTIVAL_TYPE = @KEY1  and FESTIVAL_DT = @KEY2 ");                                   
            }

            //ht.Add("@REMARK", REMARK);
            ht.Add("@AFA_APPROVE_BY", UPDATED_BY);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            ht.Add("@KEY1", KEY1);
            ht.Add("@KEY2", KEY2); 

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }
    //修改明細檔
    public void updateDetailData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            if (TYPE == "a")
            {
                sb.Append("update TB_S_M_AWARD_D ");
                sb.Append(" set AFA_APPROVE_MARK='',");
                sb.Append(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = getdate(),FUNC_ID = @FUNC_ID");
                sb.Append(" where AWARD_YEAR = @KEY1  and AWARD_ROUND = @KEY2 ");
            }
            else if (TYPE == "b")
            {
                sb.Append("update TB_S_R_BONUS_D ");
                sb.Append(" set AFA_APPROVE_MARK='',");
                sb.Append(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = getdate(),FUNC_ID = @FUNC_ID");
                sb.Append(" where BONUS_YEAR = @KEY1  and BONUS_ROUND = @KEY2 ");
            }
            else
            {
                sb.Append("update TB_S_R_FESTIVAL_D ");
                sb.Append(" set AFA_APPROVE_MARK='',");
                sb.Append(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = getdate(),FUNC_ID = @FUNC_ID");
                sb.Append(" where FESTIVAL_TYPE = @KEY1  and FESTIVAL_DT = @KEY2 ");
            }
            
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            ht.Add("@KEY1", KEY1);
            ht.Add("@KEY2", KEY2);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }
    //駁回-異常註記更新
    public void updateMarkData_D(string mark)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            if (TYPE == "a")
            {
                sb.Append("update TB_S_M_AWARD_D ");
                sb.Append(" set AFA_APPROVE_MARK=@mark,");
                sb.Append(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = getdate(),FUNC_ID = @FUNC_ID");
                sb.Append(" where AWARD_YEAR = @KEY1  and AWARD_ROUND = @KEY2 and EMP_ID = @EMP_ID ");
            }
            else if (TYPE == "b")
            {
                sb.Append("update TB_S_R_BONUS_D ");
                sb.Append(" set AFA_APPROVE_MARK=@mark,");
                sb.Append(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = getdate(),FUNC_ID = @FUNC_ID");
                sb.Append(" where BONUS_YEAR = @KEY1  and BONUS_ROUND = @KEY2 and EMP_ID = @EMP_ID");
            }
            else
            {
                sb.Append("update TB_S_R_FESTIVAL_D ");
                sb.Append(" set AFA_APPROVE_MARK=@mark,");
                sb.Append(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = getdate(),FUNC_ID = @FUNC_ID");
                sb.Append(" where FESTIVAL_TYPE = @KEY1  and FESTIVAL_DT = @KEY2 and EMP_ID = @EMP_ID");
            }

            ht.Add("@mark", mark);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            ht.Add("@KEY1", KEY1);
            ht.Add("@KEY2", KEY2);
            ht.Add("@EMP_ID", EMP_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //駁回-修改主檔
    public void rejectMainData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
                        
            if (TYPE == "a")
            {
                sb.Append("update TB_S_M_AWARD_H ");
                sb.Append(" set AFA_REMARK = @AFA_REMARK,AFA_RELEASE_BY = '',AFA_RELEASE_DT = @AFA_RELEASE_DT,AFA_APPROVE_DT = @AFA_APPROVE_DT,AFA_APPROVE_BY = '',AFA_APPROVE_STATUS = 'B',");
                sb.Append(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = getdate(),FUNC_ID = @FUNC_ID");
                sb.Append(" where AWARD_YEAR = @KEY1  and AWARD_ROUND = @KEY2 ");
            }
            else if (TYPE == "b")
            {
                sb.Append("update TB_S_M_BONUS_H ");
                sb.Append(" set AFA_REMARK = @AFA_REMARK,AFA_RELEASE_BY = '',AFA_RELEASE_DT = @AFA_RELEASE_DT,AFA_APPROVE_DT = @AFA_APPROVE_DT,AFA_APPROVE_BY = '',AFA_APPROVE_STATUS = 'B',");
                sb.Append(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = getdate(),FUNC_ID = @FUNC_ID");
                sb.Append(" where BONUS_YEAR = @KEY1  and BONUS_ROUND = @KEY2 ");
            }
            else
            {
                sb.Append("update TB_S_M_FESTIVAL_H ");
                sb.Append(" set AFA_REMARK = @AFA_REMARK,AFA_RELEASE_BY = '',AFA_RELEASE_DT = @AFA_RELEASE_DT,AFA_APPROVE_DT = @AFA_APPROVE_DT,AFA_APPROVE_BY = '',AFA_APPROVE_STATUS = 'B',");
                sb.Append(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = getdate(),FUNC_ID = @FUNC_ID");
                sb.Append(" where FESTIVAL_TYPE = @KEY1  and FESTIVAL_DT = @KEY2 ");
            }

            //ht.Add("@REMARK", REMARK);
            ht.Add("@AFA_REMARK", AFA_REMARK);
            ht.Add("@AFA_RELEASE_DT", DBNull.Value);
            ht.Add("@AFA_APPROVE_DT", DBNull.Value);
            ht.Add("@AFA_APPROVE_BY", UPDATED_BY);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            ht.Add("@KEY1", KEY1);
            ht.Add("@KEY2", KEY2);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }
    //駁回-修改明細檔
    public void rejectDetailData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            if (TYPE == "a")
            {
                sb.Append("update TB_S_M_AWARD_D ");
                sb.Append(" set AFA_APPROVE_MARK='V',");
                sb.Append(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = getdate(),FUNC_ID = @FUNC_ID");
                sb.Append(" where AWARD_YEAR = @KEY1  and AWARD_ROUND = @KEY2 ");
            }
            else if (TYPE == "b")
            {
                sb.Append("update TB_S_R_BONUS_D ");
                sb.Append(" set AFA_APPROVE_MARK='V',");
                sb.Append(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = getdate(),FUNC_ID = @FUNC_ID");
                sb.Append(" where BONUS_YEAR = @KEY1  and BONUS_ROUND = @KEY2 ");
            }
            else
            {
                sb.Append("update TB_S_R_FESTIVAL_D ");
                sb.Append(" set AFA_APPROVE_MARK='V',");
                sb.Append(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = getdate(),FUNC_ID = @FUNC_ID");
                sb.Append(" where FESTIVAL_TYPE = @KEY1  and FESTIVAL_DT = @KEY2 ");
            }

            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            ht.Add("@KEY1", KEY1);
            ht.Add("@KEY2", KEY2);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }
    //新增
    internal void insertData()
    {
        try
        {
            DateTime now = DateTime.Parse(DateTime.Now.ToString());
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" INSERT INTO TB_D_M_ENV_ALLOWANCE_TYPE ");
            sb.Append(" ( ");
            sb.Append(" ENV_ALLOWANCE_TYPE, START_DT, END_DT, ENV_ALLOWANCE_DESC, ENV_ALLOWANCE_VALUE, ENV_MIN_UNIT,REMARK ");
            sb.Append(" ,CREATED_BY, CREATED_DT, UPDATED_BY, UPDATED_DT, FUNC_ID  ");
            sb.Append(" ) ");
            sb.Append(" VALUES ");
            sb.Append(" ( @ENV_ALLOWANCE_TYPE,  @START_DT,  @END_DT,  @ENV_ALLOWANCE_DESC,  @ENV_ALLOWANCE_VALUE,  @ENV_MIN_UNIT, @REMARK  ");

            sb.Append("   ,@CREATED_BY,  @CREATED_DT,  @UPDATED_BY,  @UPDATED_DT,  @FUNC_ID )");

            //ht.Add("@ENV_ALLOWANCE_TYPE", ENV_ALLOWANCE_TYPE);
            //ht.Add("@START_DT", Convert.ToDateTime(START_DT));
            //ht.Add("@END_DT", Convert.ToDateTime(END_DT));
            //ht.Add("@ENV_ALLOWANCE_DESC", ENV_ALLOWANCE_DESC);
            //ht.Add("@ENV_ALLOWANCE_VALUE", ENV_ALLOWANCE_VALUE);
            //ht.Add("@ENV_MIN_UNIT", ENV_MIN_UNIT);
            //ht.Add("@REMARK", REMARK);
            //ht.Add("@CREATED_BY", UPDATED_BY);
            //ht.Add("@CREATED_DT", now);
            //ht.Add("@UPDATED_BY", UPDATED_BY);
            //ht.Add("@UPDATED_DT", now);
            //ht.Add("@FUNC_ID", FUNC_ID);
            //ht.Add("@ENV_ALLOWANCE_TYPE", ENV_ALLOWANCE_TYPE);
            //ht.Add("@START_DT", Convert.ToDateTime(START_DT));

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion

    #region 一括異常註記-(Dtl)
/*
    //異常註記-update 備註說明  (考核資料維護檔 DTL)
    public void updateMarkData_H(DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_S_M_AWARD_H ");
            sb.Append(" set REMARK = @REMARK ");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT = @UPDATED_DT");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where AWARD_YEAR = @AWARD_YEAR ");
            sb.Append("  and AWARD_ROUND = @AWARD_ROUND");
            //set值
            ht.Add("@REMARK", REMARK);

            //新修日期
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@UPDATED_DT", now);
            ht.Add("@FUNC_ID", FUNC_ID);

            //PK值
            ht.Add("@AWARD_YEAR", AWARD_YEAR);
            ht.Add("@AWARD_ROUND", AWARD_ROUND);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }
    //異常註記-update 異常註記為空白 或V (考核資料維護檔 DTL)
    public void updateMarkData_D(DateTime now, string approve_mark)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_S_M_AWARD_DM ");
            sb.Append(" set APPROVE_MARK = @APPROVE_MARK ");
            if (approve_mark != "")
            {
                sb.Append(" ,APPROVE_FLAG = @APPROVE_FLAG");
            }
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT = @UPDATED_DT");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where AWARD_YEAR = @AWARD_YEAR ");
            sb.Append("  and AWARD_ROUND = @AWARD_ROUND");
            sb.Append("  and EMP_ID = @EMP_ID");
            ht.Add("@APPROVE_MARK", approve_mark);
            if (approve_mark != "")
            {
                ht.Add("@APPROVE_FLAG", "N");
            }
            //PK值
            ht.Add("@AWARD_YEAR", AWARD_YEAR);
            ht.Add("@AWARD_ROUND", AWARD_ROUND);
            ht.Add("@EMP_ID", EMP_ID);


            //新修日期
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@UPDATED_DT", now);
            ht.Add("@FUNC_ID", FUNC_ID);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
*/
    #endregion
/*
    //取得異常註記的筆數
    public int getMarkData()
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) resultCount ");
            sb.Append(" from TB_S_M_AWARD_DM  ");
            sb.Append(" where AWARD_YEAR = @AWARD_YEAR ");
            sb.Append("  and AWARD_ROUND = @AWARD_ROUND");
            sb.Append("  and APPROVE_MARK = @APPROVE_MARK");
            //PK值
            ht.Add("@AWARD_YEAR", AWARD_YEAR);
            ht.Add("@AWARD_ROUND", AWARD_ROUND);
            ht.Add("@APPROVE_MARK", "V");
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


    #region 核可DB存取(Dtl)
    //核可-回復成核可狀態  (年獎維護檔 DTL)
    public void updateApproveData_H(DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_S_M_AWARD_H ");
            sb.Append(" set REMARK = @REMARK ");
            sb.Append(" ,APPROVE_DT = @APPROVE_DT");
            sb.Append(" ,APPROVE_BY = @APPROVE_BY");
            sb.Append(" ,APPROVE_STATUS = @APPROVE_STATUS");
            sb.Append(" ,FREEZE_FLAG = @FREEZE_FLAG");
            sb.Append(" ,AWARD_TOTAL_DECIMAL=(select count(*) from TB_S_M_AWARD_D where AWARD_YEAR=@AWARD_YEAR and AWARD_ROUND = @AWARD_ROUND) ");
            sb.Append(" ,AWARD_TOTAL_AMOUNT=(select isnull(sum(AWARD_AMT),0) from TB_S_M_AWARD_D where AWARD_YEAR = @AWARD_YEAR and AWARD_ROUND = @AWARD_ROUND) ");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT = @UPDATED_DT");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where AWARD_YEAR = @AWARD_YEAR ");
            sb.Append("  and AWARD_ROUND = @AWARD_ROUND");

            //set值
            ht.Add("@REMARK", "");
            ht.Add("@APPROVE_DT", now);
            ht.Add("@APPROVE_BY", APPROVE_BY);
            ht.Add("@APPROVE_STATUS", "Y");
            ht.Add("@FREEZE_FLAG", "N");

            //新修日期
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@UPDATED_DT", now);
            ht.Add("@FUNC_ID", FUNC_ID);

            //PK值
            ht.Add("@AWARD_YEAR", AWARD_YEAR);
            ht.Add("@AWARD_ROUND", AWARD_ROUND);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }


    //核可 更新-年獎明細維護檔
    public void updateAllApproveData_D(DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_S_M_AWARD_DM ");
            sb.Append(" set APPROVE_MARK = @APPROVE_MARK ");
            sb.Append(" ,APPROVE_FLAG = @APPROVE_FLAG");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT = @UPDATED_DT");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where AWARD_YEAR = @AWARD_YEAR ");
            sb.Append("  and AWARD_ROUND = @AWARD_ROUND");
            //set值
            ht.Add("@APPROVE_MARK", "");
            ht.Add("@APPROVE_FLAG", "Y");

            //PK值
            ht.Add("@AWARD_YEAR", AWARD_YEAR);
            ht.Add("@AWARD_ROUND", AWARD_ROUND);

            //新修日期
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@UPDATED_DT", now);
            ht.Add("@FUNC_ID", FUNC_ID);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //核可 刪除-年獎明細維護檔異動狀態為D
    public void deleteStatusData_D()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("delete from TB_S_M_AWARD_DM ");
            sb.Append(" where AWARD_YEAR = @AWARD_YEAR ");
            sb.Append("   and AWARD_ROUND = @AWARD_ROUND");
            sb.Append("   and CHG_STATUS = @CHG_STATUS");
            ht.Add("@CHG_STATUS", "D");
            //PK值
            ht.Add("@AWARD_YEAR", AWARD_YEAR);
            ht.Add("@AWARD_ROUND", AWARD_ROUND);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //核可,刪除-年獎明細主檔
    public void deleteApproveData_D_H(DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" delete from TB_S_M_AWARD_D ");
            sb.Append(" where AWARD_YEAR = @AWARD_YEAR ");
            sb.Append("  and AWARD_ROUND = @AWARD_ROUND");

            //PK值
            ht.Add("@AWARD_YEAR", AWARD_YEAR);
            ht.Add("@AWARD_ROUND", AWARD_ROUND);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //核可, 新增-年獎明細主檔
    public void insertApproveData_D_H(DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" insert into TB_S_M_AWARD_D ");
            sb.Append(@" (AWARD_YEAR,AWARD_ROUND,AWARD_DAYS,EMP_ID,EMP_NAME
		                ,SEX_CD,EMP_CHG_CD,WS_CD,JPN_CD,COMPANY_CD
		                ,DEPT_NO,LEVEL_CD,PJOB_CD,JOIN_DT,LEAVE_DT
		                ,STAY_DT,BE_CONTRACT_DT,BE_EMP_DT,WORK_DAYS,EMP_CD
		                ,ID_DESC,LEVEL_PAY,ABILITY_PAY,PJOB_PAY,PROFESSION_PAY
		                ,FOOD_SUBSIDY,LEVELUP_FLAG,LEVEL_PAY_BEFORE,ABILITY_PAY_BEFORE,PJOB_PAY_BEFORE
		                ,PROFESSION_PAY_BEFORE,FOOD_SUBSIDY_BEFORE,SCORE_2H,AWARD_BASE,SCORE_2H_BEFORE
		                ,AWARD_BASE_BEFORE,LEAVE_A_HOUR,LEAVE_B_HOUR,LEAVE_C_HOUR,LEAVE_Q_HOUR
		                ,LEAVE_OP_HOUR,THIRD_CNT_P,SECOND_CNT_P,FIRST_CNT_P,THIRD_CNT_M
		                ,SECOND_CNT_M,FIRST_CNT_M,ATTEND_DAYS,REWARD_DAYS,DISCIPLINE_DAYS
		                ,AWARD_WORK_DAYS,AWARD_AMT,AWARD_TAX,AWARD_AMT_R,AWARD_AMT_TMEP
		                ,AWARD_AMT_LEVEL,PAY_TYPE,CHG_STATUS,PRIMEVAL_FLAG,APPROVE_FLAG,APPROVE_MARK
		                ,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID) 
                        ");
            sb.Append(@"select
		                AWARD_YEAR,AWARD_ROUND,AWARD_DAYS,EMP_ID,EMP_NAME
		                ,SEX_CD,EMP_CHG_CD,WS_CD,JPN_CD,COMPANY_CD
		                ,DEPT_NO,LEVEL_CD,PJOB_CD,JOIN_DT,LEAVE_DT
		                ,STAY_DT,BE_CONTRACT_DT,BE_EMP_DT,WORK_DAYS,EMP_CD
		                ,ID_DESC,LEVEL_PAY,ABILITY_PAY,PJOB_PAY,PROFESSION_PAY
		                ,FOOD_SUBSIDY,LEVELUP_FLAG,LEVEL_PAY_BEFORE,ABILITY_PAY_BEFORE,PJOB_PAY_BEFORE
		                ,PROFESSION_PAY_BEFORE,FOOD_SUBSIDY_BEFORE,SCORE_2H,AWARD_BASE,SCORE_2H_BEFORE
		                ,AWARD_BASE_BEFORE,LEAVE_A_HOUR,LEAVE_B_HOUR,LEAVE_C_HOUR,LEAVE_Q_HOUR
		                ,LEAVE_OP_HOUR,THIRD_CNT_P,SECOND_CNT_P,FIRST_CNT_P,THIRD_CNT_M
		                ,SECOND_CNT_M,FIRST_CNT_M,ATTEND_DAYS,REWARD_DAYS,DISCIPLINE_DAYS
		                ,AWARD_WORK_DAYS,AWARD_AMT,AWARD_TAX,AWARD_AMT_R,AWARD_AMT_TMEP
		                ,AWARD_AMT_LEVEL,PAY_TYPE,CHG_STATUS,PRIMEVAL_FLAG,APPROVE_FLAG,APPROVE_MARK
		                ,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID  
                    ");
            sb.Append(" from TB_S_M_AWARD_DM ");
            sb.Append(" where AWARD_YEAR = @AWARD_YEAR ");
            sb.Append("  and AWARD_ROUND = @AWARD_ROUND");

            //PK值
            ht.Add("@AWARD_YEAR", AWARD_YEAR);
            ht.Add("@AWARD_ROUND", AWARD_ROUND);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    #endregion

    #region 駁回DB存取(Dtl)

    //駁回 更新-年獎明細維護檔
    public void updateRejectData_D(DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_S_M_AWARD_DM ");
            sb.Append(" set APPROVE_MARK = @APPROVE_MARK ");
            sb.Append(" ,APPROVE_FLAG = @APPROVE_FLAG");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT = @UPDATED_DT");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where AWARD_YEAR = @AWARD_YEAR ");
            sb.Append("  and AWARD_ROUND = @AWARD_ROUND");
            sb.Append("  and EMP_ID = @EMP_ID");
            ht.Add("@APPROVE_MARK", "V");
            ht.Add("@APPROVE_FLAG", "N");

            //PK值
            ht.Add("@AWARD_YEAR", AWARD_YEAR);
            ht.Add("@AWARD_ROUND", AWARD_ROUND);
            ht.Add("@EMP_ID", EMP_ID);


            //新修日期
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@UPDATED_DT", now);
            ht.Add("@FUNC_ID", FUNC_ID);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //駁回 更新-(將全部的異常記常設為空白)
    public void updateAllRejectData_D(DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_S_M_AWARD_DM ");
            sb.Append(" set APPROVE_MARK = @APPROVE_MARK ");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT = @UPDATED_DT");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where AWARD_YEAR = @AWARD_YEAR ");
            sb.Append("  and AWARD_ROUND = @AWARD_ROUND");
            ht.Add("@APPROVE_MARK", ""); //異常註常為空白

            //PK值
            ht.Add("@AWARD_YEAR", AWARD_YEAR);
            ht.Add("@AWARD_ROUND", AWARD_ROUND);

            //新修日期
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@UPDATED_DT", now);
            ht.Add("@FUNC_ID", FUNC_ID);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //駁回-回復成未核可前狀態  (年獎維護檔 DTL)
    public void updateRejectData_H(DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_S_M_AWARD_H ");
            sb.Append(" set REMARK = @REMARK ");
            sb.Append(" ,RELEASE_DT = @RELEASE_DT");
            sb.Append(" ,RELEASE_BY = @RELEASE_BY");
            sb.Append(" ,APPROVE_DT = @APPROVE_DT");
            sb.Append(" ,APPROVE_BY = @APPROVE_BY");
            sb.Append(" ,APPROVE_STATUS = @APPROVE_STATUS");
            sb.Append(" ,FREEZE_FLAG = @FREEZE_FLAG");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT = @UPDATED_DT");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where AWARD_YEAR = @AWARD_YEAR ");
            sb.Append("  and AWARD_ROUND = @AWARD_ROUND");
            //set值
            ht.Add("@REMARK", REMARK);
            ht.Add("@RELEASE_DT", DBNull.Value);
            ht.Add("@RELEASE_BY", RELEASE_BY);
            ht.Add("@APPROVE_DT", DBNull.Value);
            ht.Add("@APPROVE_BY", APPROVE_BY);
            ht.Add("@APPROVE_STATUS", "B");//B:駁回
            ht.Add("@FREEZE_FLAG", "N");

            //新修日期
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@UPDATED_DT", now);
            ht.Add("@FUNC_ID", FUNC_ID);

            //PK值
            ht.Add("@AWARD_YEAR", AWARD_YEAR);
            ht.Add("@AWARD_ROUND", AWARD_ROUND);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    #endregion

    #region 資料比對及EXCEL下載
    //判斷是否已有核可的明細主檔資料
    public DataTable getPreDataCount()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select count(0) resultCount ");
            sb.Append(" from TB_S_M_AWARD_D a ");
            sb.Append(" where 1=1 ");
            sb.Append(" and a.AWARD_YEAR = @AWARD_YEAR  ");
            sb.Append(" and a.AWARD_ROUND = @AWARD_ROUND  ");
            ht.Add("@AWARD_YEAR", AWARD_YEAR);
            ht.Add("@AWARD_ROUND", AWARD_ROUND);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //取得新增的對象資料(原始資料[original]、前次核可[prev])
    public DataTable getAddExcelData(string data)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            if (data == "prev")
            {
                sb.Append(" select  a.*                       ");
                sb.Append(" , a.CHG_STATUS + '-' + b.SUB_DESC CHG_STATUS_DESC   ");
                sb.Append(" , a.EMP_CD + '-' + c.SUB_DESC EMP_CD_DESC   ");
                sb.Append(" , a.EMP_CHG_CD + '-' + d.SUB_DESC EMP_CHG_CD_DESC   ");
                sb.Append(" , a.PAY_TYPE + '-' + e.SUB_DESC PAY_TYPE_DESC       ");
                sb.Append(" from TB_S_M_AWARD_D a             ");
                sb.Append("  left join TB_9_M_COMM_D b on  a.CHG_STATUS = b.SUB_CD and b.MAIN_CD = 'CHG_STATUS' and b.IS_VALID='Y'  and b.SYS_CD='SA'   ");
                sb.Append("  left join TB_9_M_COMM_D c on  a.EMP_CD = c.SUB_CD and c.MAIN_CD = 'EMP_CD' and b.IS_VALID='Y' and c.SYS_CD='HB' ");
                sb.Append("  left join TB_9_M_COMM_D d on  a.EMP_CHG_CD = d.SUB_CD and d.MAIN_CD = 'EMP_CHG_CD' and d.IS_VALID='Y' and d.SYS_CD='HB'    ");
                sb.Append("  left join TB_9_M_COMM_D e on  a.PAY_TYPE = e.SUB_CD and e.MAIN_CD = 'PAY_TYPE' and e.IS_VALID='Y'  and e.SYS_CD='SC'       ");
                sb.Append(" WHERE not exists (                ");
                sb.Append(" select * from TB_S_M_AWARD_DM f   ");
                sb.Append(" where                             ");
                sb.Append(" AWARD_YEAR = @AWARD_YEAR          ");
                sb.Append(" and AWARD_ROUND = @AWARD_ROUND    ");
                sb.Append(" and a.EMP_ID = f.EMP_ID           ");
                sb.Append(" )and                              ");
                sb.Append(" a.AWARD_YEAR = @AWARD_YEAR AND    ");
                sb.Append(" a.AWARD_ROUND = @AWARD_ROUND      ");
            }
            else if (data == "original")
            {
                sb.Append(" select  a.*                       ");
                sb.Append(" , a.CHG_STATUS + '-' + b.SUB_DESC CHG_STATUS_DESC   ");
                sb.Append(" , a.EMP_CD + '-' + c.SUB_DESC EMP_CD_DESC   ");
                sb.Append(" , a.EMP_CHG_CD + '-' + d.SUB_DESC EMP_CHG_CD_DESC   ");
                sb.Append(" , a.PAY_TYPE + '-' + e.SUB_DESC PAY_TYPE_DESC       ");
                sb.Append(" from TB_S_M_AWARD_DM a             ");
                sb.Append("  left join TB_9_M_COMM_D b on  a.CHG_STATUS = b.SUB_CD and b.MAIN_CD = 'CHG_STATUS' and b.IS_VALID='Y'  and b.SYS_CD='SA'   ");
                sb.Append("  left join TB_9_M_COMM_D c on  a.EMP_CD = c.SUB_CD and c.MAIN_CD = 'EMP_CD' and b.IS_VALID='Y' and c.SYS_CD='HB' ");
                sb.Append("  left join TB_9_M_COMM_D d on  a.EMP_CHG_CD = d.SUB_CD and d.MAIN_CD = 'EMP_CHG_CD' and d.IS_VALID='Y' and d.SYS_CD='HB'    ");
                sb.Append("  left join TB_9_M_COMM_D e on  a.PAY_TYPE = e.SUB_CD and e.MAIN_CD = 'PAY_TYPE' and e.IS_VALID='Y'  and e.SYS_CD='SC'       ");
                sb.Append(" WHERE not exists (                ");
                sb.Append(" select * from TB_S_S_AWARD_D  f   ");
                sb.Append(" where                             ");
                sb.Append(" AWARD_YEAR = @AWARD_YEAR          ");
                sb.Append(" and AWARD_ROUND = @AWARD_ROUND    ");
                sb.Append(" and a.EMP_ID = f.EMP_ID           ");
                sb.Append(" )and                              ");
                sb.Append(" a.AWARD_YEAR = @AWARD_YEAR AND    ");
                sb.Append(" a.AWARD_ROUND = @AWARD_ROUND      ");

            }

            //PK值
            ht.Add("@AWARD_YEAR", AWARD_YEAR);
            ht.Add("@AWARD_ROUND", AWARD_ROUND);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得刪除的對象資料(原始資料[original]、前次核可[prev])
    public DataTable getDelExcelData(string data)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            if (data == "prev")
            {
                sb.Append(" select  a.*                       ");
                sb.Append(" , a.CHG_STATUS + '-' + b.SUB_DESC CHG_STATUS_DESC   ");
                sb.Append(" , a.EMP_CD + '-' + c.SUB_DESC EMP_CD_DESC   ");
                sb.Append(" , a.EMP_CHG_CD + '-' + d.SUB_DESC EMP_CHG_CD_DESC   ");
                sb.Append(" , a.PAY_TYPE + '-' + e.SUB_DESC PAY_TYPE_DESC       ");
                sb.Append(" from TB_S_M_AWARD_DM a             ");
                sb.Append("  left join TB_9_M_COMM_D b on  a.CHG_STATUS = b.SUB_CD and b.MAIN_CD = 'CHG_STATUS' and b.IS_VALID='Y'  and b.SYS_CD='SA'   ");
                sb.Append("  left join TB_9_M_COMM_D c on  a.EMP_CD = c.SUB_CD and c.MAIN_CD = 'EMP_CD' and b.IS_VALID='Y' and c.SYS_CD='HB' ");
                sb.Append("  left join TB_9_M_COMM_D d on  a.EMP_CHG_CD = d.SUB_CD and d.MAIN_CD = 'EMP_CHG_CD' and d.IS_VALID='Y' and d.SYS_CD='HB'    ");
                sb.Append("  left join TB_9_M_COMM_D e on  a.PAY_TYPE = e.SUB_CD and e.MAIN_CD = 'PAY_TYPE' and e.IS_VALID='Y'  and e.SYS_CD='SC'       ");
                sb.Append(" WHERE not exists (                ");
                sb.Append(" select * from TB_S_M_AWARD_D f    ");
                sb.Append(" where                             ");
                sb.Append(" AWARD_YEAR = @AWARD_YEAR          ");
                sb.Append(" and AWARD_ROUND = @AWARD_ROUND    ");
                sb.Append(" and a.EMP_ID = f.EMP_ID           ");
                sb.Append(" )and                              ");
                sb.Append(" a.AWARD_YEAR = @AWARD_YEAR AND    ");
                sb.Append(" a.AWARD_ROUND = @AWARD_ROUND      ");
            }
            else if (data == "original")
            {
                sb.Append(" select  a.*                       ");
                sb.Append(" , a.CHG_STATUS + '-' + b.SUB_DESC CHG_STATUS_DESC   ");
                sb.Append(" , a.EMP_CD + '-' + c.SUB_DESC EMP_CD_DESC   ");
                sb.Append(" , a.EMP_CHG_CD + '-' + d.SUB_DESC EMP_CHG_CD_DESC   ");
                sb.Append(" , a.PAY_TYPE + '-' + e.SUB_DESC PAY_TYPE_DESC       ");
                sb.Append(" from TB_S_S_AWARD_D a             ");
                sb.Append("  left join TB_9_M_COMM_D b on  a.CHG_STATUS = b.SUB_CD and b.MAIN_CD = 'CHG_STATUS' and b.IS_VALID='Y'  and b.SYS_CD='SA'   ");
                sb.Append("  left join TB_9_M_COMM_D c on  a.EMP_CD = c.SUB_CD and c.MAIN_CD = 'EMP_CD' and b.IS_VALID='Y' and c.SYS_CD='HB' ");
                sb.Append("  left join TB_9_M_COMM_D d on  a.EMP_CHG_CD = d.SUB_CD and d.MAIN_CD = 'EMP_CHG_CD' and d.IS_VALID='Y' and d.SYS_CD='HB'    ");
                sb.Append("  left join TB_9_M_COMM_D e on  a.PAY_TYPE = e.SUB_CD and e.MAIN_CD = 'PAY_TYPE' and e.IS_VALID='Y'  and e.SYS_CD='SC'       ");
                sb.Append(" WHERE not exists (                ");
                sb.Append(" select * from TB_S_M_AWARD_DM f     ");
                sb.Append(" where                             ");
                sb.Append(" AWARD_YEAR = @AWARD_YEAR          ");
                sb.Append(" and AWARD_ROUND = @AWARD_ROUND    ");
                sb.Append(" and a.EMP_ID = f.EMP_ID           ");
                sb.Append(" )and                              ");
                sb.Append(" a.AWARD_YEAR = @AWARD_YEAR AND    ");
                sb.Append(" a.AWARD_ROUND = @AWARD_ROUND      ");
            }

            //PK值
            ht.Add("@AWARD_YEAR", AWARD_YEAR);
            ht.Add("@AWARD_ROUND", AWARD_ROUND);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得與主檔比較的資料
    public DataTable getPreCompareData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select                                                                          ");
            sb.Append(" a.EMP_ID  N_EMP_ID  ,  f.EMP_ID  O_EMP_ID                                       ");
            sb.Append(" ,a.EMP_NAME  N_EMP_NAME  ,  f.EMP_NAME  O_EMP_NAME                              ");
            sb.Append(" ,a.EMP_CHG_CD  N_EMP_CHG_CD  ,  f.EMP_CHG_CD  O_EMP_CHG_CD                      ");
            sb.Append(" ,a.WS_CD  N_WS_CD  ,  f.WS_CD  O_WS_CD                                          ");
            sb.Append(" ,a.JPN_CD  N_JPN_CD  ,  f.JPN_CD  O_JPN_CD                                      ");
            sb.Append(" ,a.DEPT_NO  N_DEPT_NO  ,  f.DEPT_NO  O_DEPT_NO                                  ");
            sb.Append(" ,a.LEVEL_CD  N_LEVEL_CD  ,  f.LEVEL_CD  O_LEVEL_CD                              ");
            sb.Append(" ,a.PJOB_CD  N_PJOB_CD  ,  f.PJOB_CD  O_PJOB_CD                                  ");
            sb.Append(" ,a.JOIN_DT  N_JOIN_DT  ,  f.JOIN_DT  O_JOIN_DT                                  ");
            sb.Append(" ,a.LEAVE_DT  N_LEAVE_DT  ,  f.LEAVE_DT  O_LEAVE_DT                              ");
            sb.Append(" ,a.STAY_DT  N_STAY_DT  ,  f.STAY_DT  O_STAY_DT                                  ");
            sb.Append(" ,a.BE_CONTRACT_DT  N_BE_CONTRACT_DT  ,  f.BE_CONTRACT_DT  O_BE_CONTRACT_DT      ");
            sb.Append(" ,a.BE_EMP_DT  N_BE_EMP_DT  ,  f.BE_EMP_DT  O_BE_EMP_DT                          ");
            sb.Append(" ,a.WORK_DAYS  N_WORK_DAYS  ,  f.WORK_DAYS  O_WORK_DAYS                          ");
            sb.Append(" ,a.EMP_CD  N_EMP_CD  ,  f.EMP_CD  O_EMP_CD                                      ");
            sb.Append(" ,a.ID_DESC  N_ID_DESC  ,  f.ID_DESC  O_ID_DESC                                  ");
            sb.Append(" ,a.LEVEL_PAY  N_LEVEL_PAY  ,  f.LEVEL_PAY  O_LEVEL_PAY                          ");
            sb.Append(" ,a.ABILITY_PAY  N_ABILITY_PAY  ,  f.ABILITY_PAY  O_ABILITY_PAY                  ");
            sb.Append(" ,a.PJOB_PAY  N_PJOB_PAY  ,  f.PJOB_PAY  O_PJOB_PAY                              ");
            sb.Append(" ,a.PROFESSION_PAY  N_PROFESSION_PAY  ,  f.PROFESSION_PAY  O_PROFESSION_PAY      ");
            sb.Append(" ,a.FOOD_SUBSIDY  N_FOOD_SUBSIDY  ,  f.FOOD_SUBSIDY  O_FOOD_SUBSIDY              ");
            sb.Append(" ,a.SCORE_2H  N_SCORE_2H  ,  f.SCORE_2H  O_SCORE_2H                              ");
            sb.Append(" ,a.AWARD_BASE  N_AWARD_BASE  ,  f.AWARD_BASE  O_AWARD_BASE                      ");
            sb.Append(" ,a.LEAVE_A_HOUR  N_LEAVE_A_HOUR  ,  f.LEAVE_A_HOUR  O_LEAVE_A_HOUR              ");
            sb.Append(" ,a.LEAVE_B_HOUR  N_LEAVE_B_HOUR  ,  f.LEAVE_B_HOUR  O_LEAVE_B_HOUR              ");
            sb.Append(" ,a.LEAVE_C_HOUR  N_LEAVE_C_HOUR  ,  f.LEAVE_C_HOUR  O_LEAVE_C_HOUR              ");
            sb.Append(" ,a.LEAVE_Q_HOUR  N_LEAVE_Q_HOUR  ,  f.LEAVE_Q_HOUR  O_LEAVE_Q_HOUR              ");
            sb.Append(" ,a.LEAVE_OP_HOUR  N_LEAVE_OP_HOUR  ,  f.LEAVE_OP_HOUR  O_LEAVE_OP_HOUR          ");
            sb.Append(" ,a.THIRD_CNT_P  N_THIRD_CNT_P  ,  f.THIRD_CNT_P  O_THIRD_CNT_P                  ");
            sb.Append(" ,a.SECOND_CNT_P  N_SECOND_CNT_P  ,  f.SECOND_CNT_P  O_SECOND_CNT_P              ");
            sb.Append(" ,a.FIRST_CNT_P  N_FIRST_CNT_P  ,  f.FIRST_CNT_P  O_FIRST_CNT_P                  ");
            sb.Append(" ,a.THIRD_CNT_M  N_THIRD_CNT_M  ,  f.THIRD_CNT_M  O_THIRD_CNT_M                  ");
            sb.Append(" ,a.SECOND_CNT_M  N_SECOND_CNT_M  ,  f.SECOND_CNT_M  O_SECOND_CNT_M              ");
            sb.Append(" ,a.FIRST_CNT_M  N_FIRST_CNT_M  ,  f.FIRST_CNT_M  O_FIRST_CNT_M                  ");
            sb.Append(" ,a.ATTEND_DAYS  N_ATTEND_DAYS  ,  f.ATTEND_DAYS  O_ATTEND_DAYS                  ");
            sb.Append(" ,a.REWARD_DAYS  N_REWARD_DAYS  ,  f.REWARD_DAYS  O_REWARD_DAYS                  ");
            sb.Append(" ,a.DISCIPLINE_DAYS  N_DISCIPLINE_DAYS  ,  f.DISCIPLINE_DAYS  O_DISCIPLINE_DAYS  ");
            sb.Append(" ,a.AWARD_WORK_DAYS  N_AWARD_WORK_DAYS  ,  f.AWARD_WORK_DAYS  O_AWARD_WORK_DAYS  ");
            sb.Append(" ,a.AWARD_AMT  N_AWARD_AMT  ,  f.AWARD_AMT  O_AWARD_AMT                          ");
            sb.Append(" ,a.LEVELUP_FLAG  N_LEVELUP_FLAG  ,  f.LEVELUP_FLAG  O_LEVELUP_FLAG              ");
            sb.Append(" ,a.PAY_TYPE  N_PAY_TYPE  ,  f.PAY_TYPE  O_PAY_TYPE                              ");
            sb.Append(" ,a.CHG_STATUS  N_CHG_STATUS  ,  f.CHG_STATUS  O_CHG_STATUS                      ");
            sb.Append(" , a.CHG_STATUS + '-' + b.SUB_DESC N_CHG_STATUS_DESC   ");
            sb.Append(" , a.EMP_CD + '-' + c.SUB_DESC N_EMP_CD_DESC   ");
            sb.Append(" , a.EMP_CHG_CD + '-' + d.SUB_DESC N_EMP_CHG_CD_DESC   ");
            sb.Append(" , a.PAY_TYPE + '-' + e.SUB_DESC N_PAY_TYPE_DESC       ");
            sb.Append(" , f.CHG_STATUS + '-' + h.SUB_DESC O_CHG_STATUS_DESC   ");
            sb.Append(" , f.EMP_CD + '-' + i.SUB_DESC O_EMP_CD_DESC   ");
            sb.Append(" , f.EMP_CHG_CD + '-' + j.SUB_DESC O_EMP_CHG_CD_DESC   ");
            sb.Append(" , f.PAY_TYPE + '-' + g.SUB_DESC O_PAY_TYPE_DESC       ");
            sb.Append(" from TB_S_M_AWARD_DM a left join  TB_S_M_AWARD_D f  on a.EMP_ID = f.EMP_ID      ");
            sb.Append("  left join TB_9_M_COMM_D b on  a.CHG_STATUS = b.SUB_CD and b.MAIN_CD = 'CHG_STATUS' and b.IS_VALID='Y'  and b.SYS_CD='SA'   ");
            sb.Append("  left join TB_9_M_COMM_D c on  a.EMP_CD = c.SUB_CD and c.MAIN_CD = 'EMP_CD' and b.IS_VALID='Y' and c.SYS_CD='HB' ");
            sb.Append("  left join TB_9_M_COMM_D d on  a.EMP_CHG_CD = d.SUB_CD and d.MAIN_CD = 'EMP_CHG_CD' and d.IS_VALID='Y' and d.SYS_CD='HB'    ");
            sb.Append("  left join TB_9_M_COMM_D e on  a.PAY_TYPE = e.SUB_CD and e.MAIN_CD = 'PAY_TYPE' and e.IS_VALID='Y'  and e.SYS_CD='SC'       ");
            sb.Append("  left join TB_9_M_COMM_D g on  f.PAY_TYPE = g.SUB_CD and g.MAIN_CD = 'PAY_TYPE' and g.IS_VALID='Y'  and g.SYS_CD='SC'       ");
            sb.Append("  left join TB_9_M_COMM_D h on  f.CHG_STATUS = h.SUB_CD and h.MAIN_CD = 'CHG_STATUS' and h.IS_VALID='Y'  and h.SYS_CD='SA'   ");
            sb.Append("  left join TB_9_M_COMM_D i on  f.EMP_CD = i.SUB_CD and i.MAIN_CD = 'EMP_CD' and i.IS_VALID='Y' and i.SYS_CD='HB' ");
            sb.Append("  left join TB_9_M_COMM_D j on  f.EMP_CHG_CD = j.SUB_CD and j.MAIN_CD = 'EMP_CHG_CD' and j.IS_VALID='Y' and j.SYS_CD='HB'    ");
            sb.Append(" where 1=1                                                                       ");
            sb.Append(" and a.AWARD_YEAR=@AWARD_YEAR and a.AWARD_ROUND=@AWARD_ROUND                     ");
            sb.Append(" and f.AWARD_YEAR=@AWARD_YEAR  and f.AWARD_ROUND=@AWARD_ROUND                    ");
            sb.Append(" and a.APPROVE_FLAG ='N'                                                         ");

            //PK值
            ht.Add("@AWARD_YEAR", AWARD_YEAR);
            ht.Add("@AWARD_ROUND", AWARD_ROUND);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }


    //取得與原始檔比較的資料
    public DataTable getOriginalCompareData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select                                                                          ");
            sb.Append(" a.EMP_ID  N_EMP_ID  ,  f.EMP_ID  O_EMP_ID                                       ");
            sb.Append(" ,a.EMP_NAME  N_EMP_NAME  ,  f.EMP_NAME  O_EMP_NAME                              ");
            sb.Append(" ,a.EMP_CHG_CD  N_EMP_CHG_CD  ,  f.EMP_CHG_CD  O_EMP_CHG_CD                      ");
            sb.Append(" ,a.WS_CD  N_WS_CD  ,  f.WS_CD  O_WS_CD                                          ");
            sb.Append(" ,a.JPN_CD  N_JPN_CD  ,  f.JPN_CD  O_JPN_CD                                      ");
            sb.Append(" ,a.DEPT_NO  N_DEPT_NO  ,  f.DEPT_NO  O_DEPT_NO                                  ");
            sb.Append(" ,a.LEVEL_CD  N_LEVEL_CD  ,  f.LEVEL_CD  O_LEVEL_CD                              ");
            sb.Append(" ,a.PJOB_CD  N_PJOB_CD  ,  f.PJOB_CD  O_PJOB_CD                                  ");
            sb.Append(" ,a.JOIN_DT  N_JOIN_DT  ,  f.JOIN_DT  O_JOIN_DT                                  ");
            sb.Append(" ,a.LEAVE_DT  N_LEAVE_DT  ,  f.LEAVE_DT  O_LEAVE_DT                              ");
            sb.Append(" ,a.STAY_DT  N_STAY_DT  ,  f.STAY_DT  O_STAY_DT                                  ");
            sb.Append(" ,a.BE_CONTRACT_DT  N_BE_CONTRACT_DT  ,  f.BE_CONTRACT_DT  O_BE_CONTRACT_DT      ");
            sb.Append(" ,a.BE_EMP_DT  N_BE_EMP_DT  ,  f.BE_EMP_DT  O_BE_EMP_DT                          ");
            sb.Append(" ,a.WORK_DAYS  N_WORK_DAYS  ,  f.WORK_DAYS  O_WORK_DAYS                          ");
            sb.Append(" ,a.EMP_CD  N_EMP_CD  ,  f.EMP_CD  O_EMP_CD                                      ");
            sb.Append(" ,a.ID_DESC  N_ID_DESC  ,  f.ID_DESC  O_ID_DESC                                  ");
            sb.Append(" ,a.LEVEL_PAY  N_LEVEL_PAY  ,  f.LEVEL_PAY  O_LEVEL_PAY                          ");
            sb.Append(" ,a.ABILITY_PAY  N_ABILITY_PAY  ,  f.ABILITY_PAY  O_ABILITY_PAY                  ");
            sb.Append(" ,a.PJOB_PAY  N_PJOB_PAY  ,  f.PJOB_PAY  O_PJOB_PAY                              ");
            sb.Append(" ,a.PROFESSION_PAY  N_PROFESSION_PAY  ,  f.PROFESSION_PAY  O_PROFESSION_PAY      ");
            sb.Append(" ,a.FOOD_SUBSIDY  N_FOOD_SUBSIDY  ,  f.FOOD_SUBSIDY  O_FOOD_SUBSIDY              ");
            sb.Append(" ,a.SCORE_2H  N_SCORE_2H  ,  f.SCORE_2H  O_SCORE_2H                              ");
            sb.Append(" ,a.AWARD_BASE  N_AWARD_BASE  ,  f.AWARD_BASE  O_AWARD_BASE                      ");
            sb.Append(" ,a.LEAVE_A_HOUR  N_LEAVE_A_HOUR  ,  f.LEAVE_A_HOUR  O_LEAVE_A_HOUR              ");
            sb.Append(" ,a.LEAVE_B_HOUR  N_LEAVE_B_HOUR  ,  f.LEAVE_B_HOUR  O_LEAVE_B_HOUR              ");
            sb.Append(" ,a.LEAVE_C_HOUR  N_LEAVE_C_HOUR  ,  f.LEAVE_C_HOUR  O_LEAVE_C_HOUR              ");
            sb.Append(" ,a.LEAVE_Q_HOUR  N_LEAVE_Q_HOUR  ,  f.LEAVE_Q_HOUR  O_LEAVE_Q_HOUR              ");
            sb.Append(" ,a.LEAVE_OP_HOUR  N_LEAVE_OP_HOUR  ,  f.LEAVE_OP_HOUR  O_LEAVE_OP_HOUR          ");
            sb.Append(" ,a.THIRD_CNT_P  N_THIRD_CNT_P  ,  f.THIRD_CNT_P  O_THIRD_CNT_P                  ");
            sb.Append(" ,a.SECOND_CNT_P  N_SECOND_CNT_P  ,  f.SECOND_CNT_P  O_SECOND_CNT_P              ");
            sb.Append(" ,a.FIRST_CNT_P  N_FIRST_CNT_P  ,  f.FIRST_CNT_P  O_FIRST_CNT_P                  ");
            sb.Append(" ,a.THIRD_CNT_M  N_THIRD_CNT_M  ,  f.THIRD_CNT_M  O_THIRD_CNT_M                  ");
            sb.Append(" ,a.SECOND_CNT_M  N_SECOND_CNT_M  ,  f.SECOND_CNT_M  O_SECOND_CNT_M              ");
            sb.Append(" ,a.FIRST_CNT_M  N_FIRST_CNT_M  ,  f.FIRST_CNT_M  O_FIRST_CNT_M                  ");
            sb.Append(" ,a.ATTEND_DAYS  N_ATTEND_DAYS  ,  f.ATTEND_DAYS  O_ATTEND_DAYS                  ");
            sb.Append(" ,a.REWARD_DAYS  N_REWARD_DAYS  ,  f.REWARD_DAYS  O_REWARD_DAYS                  ");
            sb.Append(" ,a.DISCIPLINE_DAYS  N_DISCIPLINE_DAYS  ,  f.DISCIPLINE_DAYS  O_DISCIPLINE_DAYS  ");
            sb.Append(" ,a.AWARD_WORK_DAYS  N_AWARD_WORK_DAYS  ,  f.AWARD_WORK_DAYS  O_AWARD_WORK_DAYS  ");
            sb.Append(" ,a.AWARD_AMT  N_AWARD_AMT  ,  f.AWARD_AMT  O_AWARD_AMT                          ");
            sb.Append(" ,a.LEVELUP_FLAG  N_LEVELUP_FLAG  ,  f.LEVELUP_FLAG  O_LEVELUP_FLAG              ");
            sb.Append(" ,a.PAY_TYPE  N_PAY_TYPE  ,  f.PAY_TYPE  O_PAY_TYPE                              ");
            sb.Append(" ,a.CHG_STATUS  N_CHG_STATUS  ,  f.CHG_STATUS  O_CHG_STATUS                      ");
            sb.Append(" , a.CHG_STATUS + '-' + b.SUB_DESC N_CHG_STATUS_DESC   ");
            sb.Append(" , a.EMP_CD + '-' + c.SUB_DESC N_EMP_CD_DESC   ");
            sb.Append(" , a.EMP_CHG_CD + '-' + d.SUB_DESC N_EMP_CHG_CD_DESC   ");
            sb.Append(" , a.PAY_TYPE + '-' + e.SUB_DESC N_PAY_TYPE_DESC       ");
            sb.Append(" , f.CHG_STATUS + '-' + h.SUB_DESC O_CHG_STATUS_DESC   ");
            sb.Append(" , f.EMP_CD + '-' + i.SUB_DESC O_EMP_CD_DESC   ");
            sb.Append(" , f.EMP_CHG_CD + '-' + j.SUB_DESC O_EMP_CHG_CD_DESC   ");
            sb.Append(" , f.PAY_TYPE + '-' + g.SUB_DESC O_PAY_TYPE_DESC       ");
            sb.Append(" from TB_S_M_AWARD_DM a left join  TB_S_S_AWARD_D f  on a.EMP_ID = f.EMP_ID      ");
            sb.Append("  left join TB_9_M_COMM_D b on  a.CHG_STATUS = b.SUB_CD and b.MAIN_CD = 'CHG_STATUS' and b.IS_VALID='Y'  and b.SYS_CD='SA'   ");
            sb.Append("  left join TB_9_M_COMM_D c on  a.EMP_CD = c.SUB_CD and c.MAIN_CD = 'EMP_CD' and b.IS_VALID='Y' and c.SYS_CD='HB' ");
            sb.Append("  left join TB_9_M_COMM_D d on  a.EMP_CHG_CD = d.SUB_CD and d.MAIN_CD = 'EMP_CHG_CD' and d.IS_VALID='Y' and d.SYS_CD='HB'    ");
            sb.Append("  left join TB_9_M_COMM_D e on  a.PAY_TYPE = e.SUB_CD and e.MAIN_CD = 'PAY_TYPE' and e.IS_VALID='Y'  and e.SYS_CD='SC'       ");
            sb.Append("  left join TB_9_M_COMM_D g on  f.PAY_TYPE = g.SUB_CD and g.MAIN_CD = 'PAY_TYPE' and g.IS_VALID='Y'  and g.SYS_CD='SC'       ");
            sb.Append("  left join TB_9_M_COMM_D h on  f.CHG_STATUS = h.SUB_CD and h.MAIN_CD = 'CHG_STATUS' and h.IS_VALID='Y'  and h.SYS_CD='SA'   ");
            sb.Append("  left join TB_9_M_COMM_D i on  f.EMP_CD = i.SUB_CD and i.MAIN_CD = 'EMP_CD' and i.IS_VALID='Y' and i.SYS_CD='HB' ");
            sb.Append("  left join TB_9_M_COMM_D j on  f.EMP_CHG_CD = j.SUB_CD and j.MAIN_CD = 'EMP_CHG_CD' and j.IS_VALID='Y' and j.SYS_CD='HB'    ");
            sb.Append(" where 1=1                                                                       ");
            sb.Append(" and a.AWARD_YEAR=@AWARD_YEAR and a.AWARD_ROUND=@AWARD_ROUND                     ");
            sb.Append(" and f.AWARD_YEAR=@AWARD_YEAR  and f.AWARD_ROUND=@AWARD_ROUND                    ");
            sb.Append(" and a.PRIMEVAL_FLAG <> 'N'                                                      ");

            //PK值
            ht.Add("@AWARD_YEAR", AWARD_YEAR);
            ht.Add("@AWARD_ROUND", AWARD_ROUND);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion
*/
}