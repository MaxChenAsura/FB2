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
/// CFB2DH0100DAO 的摘要描述
/// </summary>
public class CFB2DH0100DAO : BaseDAO
{
    public string MAIN_LEAVE_CD { get; set; }
    public string MAIN_LEAVE_DESC { get; set; }
    public string IS_IFLOW_SHOW { get; set; }
    public string ORDER_SEQ { get; set; }
    public string IS_USED { get; set; }
    public string CREATED_BY { get; set; }
    public string UPDATED_BY { get; set; }
    public string FUNC_ID { get; set; }

    public string SUB_LEAVE_CD { get; set; }
    public string SUB_LEAVE_DESC { get; set; }
    public string LEAVE_PAY_RATE { get; set; }
    public string LEAVE_TIME_UNIT { get; set; }
    public string LEAVE_COUNT_HOUR { get; set; }
    public string LEAVE_MIN_VALUE { get; set; }
    public string LEAVE_COUNT_CD { get; set; }
    public string LEAVE_MAX_DAY_CD { get; set; }
    public string IS_INCLUDE_HOLIDAY { get; set; }
    public string LEAVE_TIME_LIMIT_CD { get; set; }
    public string LEAVE_ALLOW_CD { get; set; }
    public string LEAVE_SPECIAL_CD { get; set; }
    public string SALARY_SETTLE_CD { get; set; }
    public string IS_QRY_SHOW { get; set; }

    public string MERGE_SUB_LEAVE_CD { get; set; }
    public string LEAVE_MAX_DAY { get; set; }
    public string START_DT_CD { get; set; }
    public string START_SIGN { get; set; }
    public string START_DAY { get; set; }
    public string END_DT_CD { get; set; }
    public string END_SIGN { get; set; }
    public string END_DAY { get; set; }

    public string START_TIME_CD { get; set; }
    public string START_SIGN_AD { get; set; }
    public string START_HOURS { get; set; }
    public string END_TIME_CD { get; set; }
    public string END_SIGN_AD { get; set; }
    public string END_HOURS { get; set; }

    public string EMP_CD { get; set; }
    public string PJOB_CD { get; set; }

    public string AWARD_DAY { get; set; }
    public string BONUS_DAY { get; set; }
    public string PLAN_DAY { get; set; }
    public string IS_ASSESS { get; set; }
    public string IS_ACC_HOUR { get; set; }
    public string DH_WORK_DAY_CD { get; set; }
    public string DH_SEX_CD { get; set; }

    public CFB2DH0100DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    //主假別查詢
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string main_leave_cd, string is_used)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * From");
            sb.Append(" (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" MAIN_LEAVE_CD,MAIN_LEAVE_DESC,IS_IFLOW_SHOW,ORDER_SEQ,IS_USED");
            sb.Append(" from TB_D_M_LEAVE_TYPE_H ");
            sb.Append(" where 1=1 ");

            if (main_leave_cd != "")
            {
                sb.Append(" and MAIN_LEAVE_CD = @main_leave_cd ");
                ht.Add("@main_leave_cd", main_leave_cd);
            }
            /*
            if (is_iflow_show != "-1" && is_iflow_show != null)
            {
                sb.Append(" and IS_IFLOW_SHOW = @is_iflow_show ");
                ht.Add("@is_iflow_show", is_iflow_show);
            }
            */
            if (is_used != "-1" && is_used != null)
            {
                sb.Append(" and IS_USED = @is_used ");
                ht.Add("@is_used", is_used);
            }

            sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar) ");

            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    public int getCount(int startRowIndex, int maximumRows, string main_leave_cd,  string is_used)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select COUNT(*) total_record ");
            sb.Append(" from TB_D_M_LEAVE_TYPE_H ");
            sb.Append(" where 1=1 ");

            if (main_leave_cd != "")
            {
                sb.Append(" and MAIN_LEAVE_CD = @main_leave_cd ");
                ht.Add("@main_leave_cd", main_leave_cd);
            }
            /*
            if (is_iflow_show != "-1" && is_iflow_show != null)
            {
                sb.Append(" and IS_IFLOW_SHOW = @is_iflow_show ");
                ht.Add("@is_iflow_show", is_iflow_show);
            }
            */
            if (is_used != "-1" && is_used != null)
            {
                sb.Append(" and IS_USED = @is_used ");
                ht.Add("@is_used", is_used);
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

    //子假別查詢
    public DataTable getData2(int startRowIndex, int maximumRows, string sortExpression, string main_leave_cd, string is_iflow_show, string is_used)
    {
        try
        {
            if (sortExpression.Contains("MAIN_LEAVE_CD"))
                sortExpression = sortExpression.Replace("MAIN_LEAVE_CD", "a.MAIN_LEAVE_CD");

            if (sortExpression.Contains("SUB_LEAVE_CD"))
                sortExpression = sortExpression.Replace("SUB_LEAVE_CD", "a.SUB_LEAVE_CD");

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * From");
            sb.Append(" (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" a.MAIN_LEAVE_CD,a.SUB_LEAVE_CD,a.SUB_LEAVE_DESC,a.LEAVE_PAY_RATE, ");
            sb.Append(" a.AWARD_DAY,a.BONUS_DAY,a.PLAN_DAY,IIF(a.IS_ASSESS='Y','Y-是','N-否') IS_ASSESS, IIF(a.IS_ACC_HOUR='Y','Y-是','N-否') IS_ACC_HOUR, ");
            sb.Append(" b.SUB_CD+'-'+b.SUB_DESC LEAVE_TIME_UNIT,a.LEAVE_COUNT_HOUR,a.LEAVE_MIN_VALUE, ");
            sb.Append(" c.SUB_CD+'-'+c.SUB_DESC LEAVE_COUNT_CD, ");
            sb.Append(" d.SUB_CD+'-'+d.SUB_DESC LEAVE_MAX_DAY_CD,a.IS_INCLUDE_HOLIDAY, ");
            sb.Append(" e.SUB_CD+'-'+e.SUB_DESC LEAVE_TIME_LIMIT_CD, ");
            sb.Append(" f.SUB_CD+'-'+f.SUB_DESC LEAVE_ALLOW_CD, ");
            sb.Append(" g.SUB_CD+'-'+g.SUB_DESC LEAVE_SPECIAL_CD,");
            sb.Append(" h.SUB_CD+'-'+h.SUB_DESC SALARY_SETTLE_CD,a.IS_IFLOW_SHOW,a.IS_USED,a.IS_QRY_SHOW, ");
            sb.Append(" i.SUB_CD+'-'+i.SUB_DESC DH_WORK_DAY_CD,j.SUB_CD+'-'+j.SUB_DESC DH_SEX_CD ");
            sb.Append(" from TB_D_M_LEAVE_TYPE_D a");
            sb.Append(" left join TB_9_M_COMM_D b on b.SYS_CD='DH' and b.MAIN_CD='LEAVE_TIME_UNIT' and b.IS_VALID='Y' and b.SUB_CD=a.LEAVE_TIME_UNIT ");
            sb.Append(" left join TB_9_M_COMM_D c on c.SYS_CD='DH' and c.MAIN_CD='LEAVE_COUNT_CD' and c.IS_VALID='Y' and c.SUB_CD=a.LEAVE_COUNT_CD ");
            sb.Append(" left join TB_9_M_COMM_D d on d.SYS_CD='DH' and d.MAIN_CD='LEAVE_MAX_DAY_CD' and d.IS_VALID='Y' and d.SUB_CD=a.LEAVE_MAX_DAY_CD ");
            sb.Append(" left join TB_9_M_COMM_D e on e.SYS_CD='DH' and e.MAIN_CD='LIMIT_CD' and e.IS_VALID='Y' and e.SUB_CD=a.LEAVE_TIME_LIMIT_CD ");
            sb.Append(" left join TB_9_M_COMM_D f on f.SYS_CD='DH' and f.MAIN_CD='LIMIT_CD' and f.IS_VALID='Y' and f.SUB_CD=a.LEAVE_ALLOW_CD ");
            sb.Append(" left join TB_9_M_COMM_D g on g.SYS_CD='DH' and g.MAIN_CD='LEAVE_SPECIAL_CD' and g.IS_VALID='Y' and g.SUB_CD=a.LEAVE_SPECIAL_CD ");
            sb.Append(" left join TB_9_M_COMM_D h on h.SYS_CD='DH' and h.MAIN_CD='SALARY_SETTLE_CD' and h.IS_VALID='Y' and h.SUB_CD=a.SALARY_SETTLE_CD ");
            sb.Append(" left join TB_9_M_COMM_D i on i.SYS_CD='DH' and i.MAIN_CD='DH_WORK_DAY_CD' and i.SUB_CD=a.DH_WORK_DAY_CD ");
            sb.Append(" left join TB_9_M_COMM_D j on j.SYS_CD='DH' and j.MAIN_CD='DH_SEX_CD' and j.SUB_CD=a.DH_SEX_CD ");
            sb.Append(" where 1=1 ");

            if (main_leave_cd != "")
            {
                sb.Append(" and a.MAIN_LEAVE_CD = @main_leave_cd ");
                ht.Add("@main_leave_cd", main_leave_cd.Split('-')[0]);
            }

            if (is_iflow_show != "-1" && is_iflow_show != null)
            {
                sb.Append(" and a.IS_IFLOW_SHOW = @is_iflow_show ");
                ht.Add("@is_iflow_show", is_iflow_show);
            }

            if (is_used != "-1" && is_used != null)
            {
                sb.Append(" and a.IS_USED = @is_used ");
                ht.Add("@is_used", is_used);
            }

            sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar) ");

            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    public int getCount2(int startRowIndex, int maximumRows, string main_leave_cd, string is_iflow_show, string is_used)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select COUNT(*) total_record ");
            sb.Append(" from TB_D_M_LEAVE_TYPE_D a");
            sb.Append(" left join TB_9_M_COMM_D b on b.SYS_CD='DH' and b.MAIN_CD='LEAVE_TIME_UNIT' and b.IS_VALID='Y' and b.SUB_CD=a.LEAVE_TIME_UNIT ");
            sb.Append(" left join TB_9_M_COMM_D c on c.SYS_CD='DH' and c.MAIN_CD='LEAVE_COUNT_CD' and c.IS_VALID='Y' and c.SUB_CD=a.LEAVE_COUNT_CD ");
            sb.Append(" left join TB_9_M_COMM_D d on d.SYS_CD='DH' and d.MAIN_CD='LEAVE_MAX_DAY_CD' and d.IS_VALID='Y' and d.SUB_CD=a.LEAVE_MAX_DAY_CD ");
            sb.Append(" left join TB_9_M_COMM_D e on e.SYS_CD='DH' and e.MAIN_CD='LIMIT_CD' and e.IS_VALID='Y' and e.SUB_CD=a.LEAVE_TIME_LIMIT_CD ");
            sb.Append(" left join TB_9_M_COMM_D f on f.SYS_CD='DH' and f.MAIN_CD='LIMIT_CD' and f.IS_VALID='Y' and f.SUB_CD=a.LEAVE_ALLOW_CD ");
            sb.Append(" left join TB_9_M_COMM_D g on g.SYS_CD='DH' and g.MAIN_CD='LEAVE_SPECIAL_CD' and g.IS_VALID='Y' and g.SUB_CD=a.LEAVE_SPECIAL_CD ");
            sb.Append(" left join TB_9_M_COMM_D h on h.SYS_CD='DH' and h.MAIN_CD='SALARY_SETTLE_CD' and h.IS_VALID='Y' and h.SUB_CD=a.SALARY_SETTLE_CD ");
            sb.Append(" left join TB_9_M_COMM_D i on i.SYS_CD='DH' and i.MAIN_CD='DH_WORK_DAY_CD' and i.SUB_CD=a.DH_WORK_DAY_CD ");
            sb.Append(" left join TB_9_M_COMM_D j on j.SYS_CD='DH' and j.MAIN_CD='DH_SEX_CD' and j.SUB_CD=a.DH_SEX_CD ");
            sb.Append(" where 1=1 ");

            if (main_leave_cd != "")
            {
                sb.Append(" and MAIN_LEAVE_CD = @main_leave_cd ");
                ht.Add("@main_leave_cd", main_leave_cd.Split('-')[0]);
            }

            if (is_iflow_show != "-1" && is_iflow_show != null)
            {
                sb.Append(" and IS_IFLOW_SHOW = @is_iflow_show ");
                ht.Add("@is_iflow_show", is_iflow_show);
            }

            if (is_used != "-1" && is_used != null)
            {
                sb.Append(" and IS_USED = @is_used ");
                ht.Add("@is_used", is_used);
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

    //子假別使用上限控管條件查詢
    public DataTable getData3(int startRowIndex, int maximumRows, string sortExpression, string main_leave_cd, string sub_leave_cd)
    {
        try
        {
            if (sortExpression.Contains("MAIN_LEAVE_CD"))
                sortExpression = sortExpression.Replace("MAIN_LEAVE_CD", "a.MAIN_LEAVE_CD");

            if (sortExpression.Contains("MERGE_SUB_LEAVE_CD"))
                sortExpression = sortExpression.Replace("MERGE_SUB_LEAVE_CD", "a.MERGE_SUB_LEAVE_CD");

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * From");
            sb.Append(" (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" a.MAIN_LEAVE_CD,a.MERGE_SUB_LEAVE_CD,a.LEAVE_MAX_DAY,b.SUB_CD+'-'+b.SUB_DESC START_DT_CD, ");
            sb.Append(" a.START_SIGN,a.START_DAY,c.SUB_CD+'-'+c.SUB_DESC END_DT_CD,a.END_SIGN,a.END_DAY ");
            sb.Append(" from TB_D_M_LEAVE_MAX_DAY a ");
            sb.Append(" left join TB_9_M_COMM_D b on b.SYS_CD='DH' and b.MAIN_CD='START_DT_CD' ");
            sb.Append(" and b.IS_VALID='Y' and b.SUB_CD=a.START_DT_CD ");
            sb.Append(" left join TB_9_M_COMM_D c on c.SYS_CD='DH' and c.MAIN_CD='START_DT_CD' ");
            sb.Append(" and c.IS_VALID='Y' and c.SUB_CD=a.END_DT_CD ");
            sb.Append(" where 1=1 ");

            if (main_leave_cd != "")
            {
                sb.Append(" and a.MAIN_LEAVE_CD = @main_leave_cd ");
                ht.Add("@main_leave_cd", main_leave_cd.Split('-')[0]);
            }

            if (sub_leave_cd != "")
            {
                sb.Append(" and a.MERGE_SUB_LEAVE_CD like @sub_leave_cd ");
                ht.Add("@sub_leave_cd", "%" + sub_leave_cd.Split('-')[0] + "%");
            }

            sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar) ");

            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    public int getCount3(int startRowIndex, int maximumRows, string main_leave_cd, string sub_leave_cd)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select COUNT(*) total_record ");
            sb.Append(" from TB_D_M_LEAVE_MAX_DAY a ");
            sb.Append(" left join TB_9_M_COMM_D b on b.SYS_CD='DH' and b.MAIN_CD='START_DT_CD' ");
            sb.Append(" and b.IS_VALID='Y' and b.SUB_CD=a.START_DT_CD ");
            sb.Append(" left join TB_9_M_COMM_D c on c.SYS_CD='DH' and c.MAIN_CD='START_DT_CD' ");
            sb.Append(" and c.IS_VALID='Y' and c.SUB_CD=a.END_DT_CD ");
            sb.Append(" where 1=1 ");

            if (main_leave_cd != "")
            {
                sb.Append(" and a.MAIN_LEAVE_CD = @main_leave_cd ");
                ht.Add("@main_leave_cd", main_leave_cd.Split('-')[0]);
            }

            if (sub_leave_cd != "")
            {
                sb.Append(" and a.MERGE_SUB_LEAVE_CD like @sub_leave_cd ");
                ht.Add("@sub_leave_cd", "%" + sub_leave_cd.Split('-')[0] + "%");
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

    //子假別請假時段限制條件查詢
    public DataTable getData4(int startRowIndex, int maximumRows, string sortExpression, string main_leave_cd, string sub_leave_cd)
    {
        try
        {
            if (sortExpression.Contains("MAIN_LEAVE_CD"))
                sortExpression = sortExpression.Replace("MAIN_LEAVE_CD", "a.MAIN_LEAVE_CD");

            if (sortExpression.Contains("SUB_LEAVE_CD"))
                sortExpression = sortExpression.Replace("SUB_LEAVE_CD", "a.SUB_LEAVE_CD");

            if (sortExpression.Contains("START_TIME_CD"))
                sortExpression = sortExpression.Replace("START_TIME_CD", "a.START_TIME_CD");

            if (sortExpression.Contains("END_TIME_CD"))
                sortExpression = sortExpression.Replace("END_TIME_CD", "a.END_TIME_CD");

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * From");
            sb.Append(" (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" a.MAIN_LEAVE_CD,a.SUB_LEAVE_CD,a.START_SIGN,b.SUB_CD+'-'+b.SUB_DESC START_TIME_CD,a.START_SIGN_AD, ");
            sb.Append(" a.START_HOURS,a.END_SIGN,c.SUB_CD+'-'+c.SUB_DESC END_TIME_CD,a.END_SIGN_AD,a.END_HOURS ");
            sb.Append(" from TB_D_M_LEAVE_TIME_LIMIT a ");
            sb.Append(" left join TB_9_M_COMM_D b on b.SYS_CD='DH' and b.MAIN_CD='START_TIME_CD' ");
            sb.Append(" and b.IS_VALID='Y' and b.SUB_CD=a.START_TIME_CD ");
            sb.Append(" left join TB_9_M_COMM_D c on c.SYS_CD='DH' and c.MAIN_CD='START_TIME_CD' ");
            sb.Append(" and c.IS_VALID='Y' and c.SUB_CD=a.END_TIME_CD ");
            sb.Append(" where 1=1 ");

            if (main_leave_cd != "")
            {
                sb.Append(" and a.MAIN_LEAVE_CD = @main_leave_cd ");
                ht.Add("@main_leave_cd", main_leave_cd.Split('-')[0]);
            }

            if (sub_leave_cd != "")
            {
                sb.Append(" and a.SUB_LEAVE_CD = @sub_leave_cd ");
                ht.Add("@sub_leave_cd", sub_leave_cd.Split('-')[0]);
            }

            sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar) ");

            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    public int getCount4(int startRowIndex, int maximumRows, string main_leave_cd, string sub_leave_cd)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select COUNT(*) total_record ");
            sb.Append(" from TB_D_M_LEAVE_TIME_LIMIT a ");
            sb.Append(" left join TB_9_M_COMM_D b on b.SYS_CD='DH' and b.MAIN_CD='START_TIME_CD' ");
            sb.Append(" and b.IS_VALID='Y' and b.SUB_CD=a.START_TIME_CD ");
            sb.Append(" left join TB_9_M_COMM_D c on c.SYS_CD='DH' and c.MAIN_CD='START_TIME_CD' ");
            sb.Append(" and c.IS_VALID='Y' and c.SUB_CD=a.END_TIME_CD ");
            sb.Append(" where 1=1 ");

            if (main_leave_cd != "")
            {
                sb.Append(" and a.MAIN_LEAVE_CD = @main_leave_cd ");
                ht.Add("@main_leave_cd", main_leave_cd.Split('-')[0]);
            }

            if (sub_leave_cd != "")
            {
                sb.Append(" and a.SUB_LEAVE_CD = @sub_leave_cd ");
                ht.Add("@sub_leave_cd", sub_leave_cd.Split('-')[0]);
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

    //子假別適用人員查詢
    public DataTable getData5(int startRowIndex, int maximumRows, string sortExpression, string main_leave_cd, string sub_leave_cd)
    {
        try
        {
            if (sortExpression.Contains("MAIN_LEAVE_CD"))
                sortExpression = sortExpression.Replace("MAIN_LEAVE_CD", "a.MAIN_LEAVE_CD");

            if (sortExpression.Contains("SUB_LEAVE_CD"))
                sortExpression = sortExpression.Replace("SUB_LEAVE_CD", "a.SUB_LEAVE_CD");

            if (sortExpression.Contains("EMP_CD"))
                sortExpression = sortExpression.Replace("EMP_CD", "a.EMP_CD");

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * From");
            sb.Append(" (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" a.MAIN_LEAVE_CD,a.SUB_LEAVE_CD,b.SUB_CD+'-'+b.SUB_DESC EMP_CD, c.PJOB_CD+'-'+c.PJOB_DESC PJOB_CD ");
            sb.Append(" from TB_D_M_LEAVE_ALLOW a ");
            sb.Append(" left join TB_9_M_COMM_D b on b.SYS_CD='HB' and b.MAIN_CD='EMP_CD' ");
            sb.Append(" and b.IS_VALID='Y' and b.SUB_CD=a.EMP_CD ");
            sb.Append(" left join TB_H_M_PJOB c on a.PJOB_CD = c.PJOB_CD and getdate() between start_dt and end_dt ");
            sb.Append(" where 1=1 ");

            if (main_leave_cd != "")
            {
                sb.Append(" and a.MAIN_LEAVE_CD = @main_leave_cd ");
                ht.Add("@main_leave_cd", main_leave_cd.Split('-')[0]);
            }

            if (sub_leave_cd != "")
            {
                sb.Append(" and a.SUB_LEAVE_CD = @sub_leave_cd ");
                ht.Add("@sub_leave_cd", sub_leave_cd.Split('-')[0]);
            }

            sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar) ");

            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    public int getCount5(int startRowIndex, int maximumRows, string main_leave_cd, string sub_leave_cd)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select COUNT(*) total_record ");
            sb.Append(" from TB_D_M_LEAVE_ALLOW a ");
            sb.Append(" left join TB_9_M_COMM_D b on b.SYS_CD='HB' and b.MAIN_CD='EMP_CD' ");
            sb.Append(" and b.IS_VALID='Y' and b.SUB_CD=a.EMP_CD ");
            sb.Append(" left join TB_H_M_PJOB c on a.PJOB_CD = c.PJOB_CD and getdate() between start_dt and end_dt ");
            sb.Append(" where 1=1 ");

            if (main_leave_cd != "")
            {
                sb.Append(" and a.MAIN_LEAVE_CD = @main_leave_cd ");
                ht.Add("@main_leave_cd", main_leave_cd.Split('-')[0]);
            }

            if (sub_leave_cd != "")
            {
                sb.Append(" and a.SUB_LEAVE_CD = @sub_leave_cd ");
                ht.Add("@sub_leave_cd", sub_leave_cd.Split('-')[0]);
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

    public void deleteLEAVE_TYPE_H(string item)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //寫log
            sb.Append(" update TB_D_M_LEAVE_TYPE_H set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2DH010' ");
            sb.Append(" where MAIN_LEAVE_CD = @MAIN_LEAVE_CD;");
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);

            sb.Append(" delete from TB_D_M_LEAVE_TYPE_H");
            sb.Append(" where MAIN_LEAVE_CD = @MAIN_LEAVE_CD;");
            ht.Add("@MAIN_LEAVE_CD", item);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //請假申請資料檔
    public DataTable getLEAVE_APPLY(string item1, string item2)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select MAIN_LEAVE_CD from TB_D_M_LEAVE_APPLY");
            sb.Append(" where MAIN_LEAVE_CD = @MAIN_LEAVE_CD");
            sb.Append(" and FORM_STATUS <> 'N' and FORM_STATUS <> 'D' ");
            if (item2 != "")
            {
                sb.Append(" and SUB_LEAVE_CD = @SUB_LEAVE_CD");
                ht.Add("@SUB_LEAVE_CD", item2);
            }
            ht.Add("@MAIN_LEAVE_CD", item1);

            return dbConn.Query(sb, ht);
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    //日請假資料檔
    public DataTable getLEAVE_APPLY_DAY(string item1, string item2)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select MAIN_LEAVE_CD from TB_D_M_LEAVE_APPLY_DAY");
            sb.Append(" where MAIN_LEAVE_CD = @MAIN_LEAVE_CD");
            sb.Append(" and FORM_STATUS <> 'N' and FORM_STATUS <> 'D' ");
            if (item2 != "")
            {
                sb.Append(" and SUB_LEAVE_CD = @SUB_LEAVE_CD");
                ht.Add("@SUB_LEAVE_CD", item2);
            }
            ht.Add("@MAIN_LEAVE_CD", item1);

            return dbConn.Query(sb, ht);
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    //子假別資料檔
    public DataTable getLEAVE_TYPE_D(string item1, string item2)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select MAIN_LEAVE_CD from TB_D_M_LEAVE_TYPE_D");
            sb.Append(" where MAIN_LEAVE_CD = @MAIN_LEAVE_CD");
            if (item2 != "")
            {
                sb.Append(" and SUB_LEAVE_CD = @SUB_LEAVE_CD");
                ht.Add("@SUB_LEAVE_CD", item2);
            }
            ht.Add("@MAIN_LEAVE_CD", item1);

            return dbConn.Query(sb, ht);
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    //子假別使用上限控管條件檔
    public DataTable getLEAVE_MAX_DAY(string item1, string item2)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select MAIN_LEAVE_CD from TB_D_M_LEAVE_MAX_DAY");
            sb.Append(" where MAIN_LEAVE_CD = @MAIN_LEAVE_CD");
            if (item2 != "")
            {
                sb.Append(" and MERGE_SUB_LEAVE_CD in (@MERGE_SUB_LEAVE_CD)");
                ht.Add("@MERGE_SUB_LEAVE_CD", item2.Split(','));
            }
            ht.Add("@MAIN_LEAVE_CD", item1);

            return dbConn.Query(sb, ht);
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    //子假別請假時段限制條件檔
    public DataTable getLEAVE_TIME_LIMIT(string item1, string item2)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select MAIN_LEAVE_CD from TB_D_M_LEAVE_TIME_LIMIT");
            sb.Append(" where MAIN_LEAVE_CD = @MAIN_LEAVE_CD");
            if (item2 != "")
            {
                sb.Append(" and SUB_LEAVE_CD = @SUB_LEAVE_CD");
                ht.Add("@SUB_LEAVE_CD", item2);
            }
            ht.Add("@MAIN_LEAVE_CD", item1);

            return dbConn.Query(sb, ht);
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    //子假別適用人員設定檔
    public DataTable getLEAVE_ALLOW(string item1, string item2)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select MAIN_LEAVE_CD from TB_D_M_LEAVE_ALLOW");
            sb.Append(" where MAIN_LEAVE_CD = @MAIN_LEAVE_CD");
            if (item2 != "")
            {
                sb.Append(" and SUB_LEAVE_CD = @SUB_LEAVE_CD");
                ht.Add("@SUB_LEAVE_CD", item2);
            }
            ht.Add("@MAIN_LEAVE_CD", item1);

            return dbConn.Query(sb, ht);
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    public DataTable getExistData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select MAIN_LEAVE_CD from TB_D_M_LEAVE_TYPE_H");
            sb.Append(" where MAIN_LEAVE_CD = @MAIN_LEAVE_CD");
            ht.Add("@MAIN_LEAVE_CD", MAIN_LEAVE_CD);

            return dbConn.Query(sb, ht);
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    //新增 TB_D_M_LEAVE_TYPE_H	主假別資料檔
    public void addLEAVE_TYPE_H()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("insert into TB_D_M_LEAVE_TYPE_H ( ");
            sb.Append(" MAIN_LEAVE_CD,MAIN_LEAVE_DESC,IS_IFLOW_SHOW,ORDER_SEQ,IS_USED,");
            sb.Append(" CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" values ( ");
            sb.Append(" @MAIN_LEAVE_CD,@MAIN_LEAVE_DESC,@IS_IFLOW_SHOW,@ORDER_SEQ,@IS_USED,");
            sb.Append(" @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@MAIN_LEAVE_CD", MAIN_LEAVE_CD);
            ht.Add("@MAIN_LEAVE_DESC", MAIN_LEAVE_DESC);
            ht.Add("@IS_IFLOW_SHOW", IS_IFLOW_SHOW);
            ht.Add("@ORDER_SEQ", ORDER_SEQ);
            ht.Add("@IS_USED", IS_USED);
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

    public void updateLEAVE_TYPE_H()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_D_M_LEAVE_TYPE_H ");
            sb.Append(" set MAIN_LEAVE_DESC=@MAIN_LEAVE_DESC,IS_IFLOW_SHOW=@IS_IFLOW_SHOW,ORDER_SEQ=@ORDER_SEQ,IS_USED=@IS_USED,");
            sb.Append(" UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID");
            sb.Append(" where MAIN_LEAVE_CD=@MAIN_LEAVE_CD ");

            ht.Add("@MAIN_LEAVE_CD", MAIN_LEAVE_CD);
            ht.Add("@MAIN_LEAVE_DESC", MAIN_LEAVE_DESC);
            ht.Add("@IS_IFLOW_SHOW", IS_IFLOW_SHOW);
            ht.Add("@ORDER_SEQ", ORDER_SEQ);
            ht.Add("@IS_USED", IS_USED);

            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void deleteLEAVE_TYPE_D(Tuple<string, string> item)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //寫log
            sb.Append(" update TB_D_M_LEAVE_TYPE_D set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2DH010' ");
            sb.Append(" where MAIN_LEAVE_CD = @MAIN_LEAVE_CD");
            sb.Append(" and SUB_LEAVE_CD = @SUB_LEAVE_CD;");
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);

            sb.Append(" delete from TB_D_M_LEAVE_TYPE_D");
            sb.Append(" where MAIN_LEAVE_CD = @MAIN_LEAVE_CD");
            sb.Append(" and SUB_LEAVE_CD = @SUB_LEAVE_CD;");
            ht.Add("@MAIN_LEAVE_CD", item.Item1);
            ht.Add("@SUB_LEAVE_CD", item.Item2);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getDefaultData(string main_leave_cd, string sub_leave_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select MAIN_LEAVE_CD,SUB_LEAVE_CD,SUB_LEAVE_DESC,LEAVE_PAY_RATE,LEAVE_TIME_UNIT, ");
            sb.Append(" LEAVE_COUNT_HOUR,LEAVE_MIN_VALUE,LEAVE_COUNT_CD,LEAVE_MAX_DAY_CD,IS_INCLUDE_HOLIDAY, ");
            sb.Append(" LEAVE_TIME_LIMIT_CD,LEAVE_ALLOW_CD,LEAVE_SPECIAL_CD,SALARY_SETTLE_CD,IS_IFLOW_SHOW, ");
            sb.Append(" IS_USED,IS_QRY_SHOW,AWARD_DAY,BONUS_DAY,PLAN_DAY,IS_ASSESS,IS_ACC_HOUR,DH_WORK_DAY_CD,DH_SEX_CD ");
            sb.Append(" from TB_D_M_LEAVE_TYPE_D ");
            sb.Append(" where MAIN_LEAVE_CD=@MAIN_LEAVE_CD and SUB_LEAVE_CD=@SUB_LEAVE_CD ");
            ht.Add("@MAIN_LEAVE_CD", main_leave_cd);
            ht.Add("@SUB_LEAVE_CD", sub_leave_cd);

            return dbConn.Query(sb, ht);
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    public void updateLEAVE_TYPE_D()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_D_M_LEAVE_TYPE_D ");
            sb.Append(" set SUB_LEAVE_DESC=@SUB_LEAVE_DESC,LEAVE_PAY_RATE=@LEAVE_PAY_RATE,LEAVE_TIME_UNIT=@LEAVE_TIME_UNIT, ");
            sb.Append(" LEAVE_COUNT_HOUR=@LEAVE_COUNT_HOUR,LEAVE_MIN_VALUE=@LEAVE_MIN_VALUE,LEAVE_COUNT_CD=@LEAVE_COUNT_CD, ");
            sb.Append(" LEAVE_MAX_DAY_CD=@LEAVE_MAX_DAY_CD,IS_INCLUDE_HOLIDAY=@IS_INCLUDE_HOLIDAY,LEAVE_TIME_LIMIT_CD=@LEAVE_TIME_LIMIT_CD, ");
            sb.Append(" LEAVE_ALLOW_CD=@LEAVE_ALLOW_CD,LEAVE_SPECIAL_CD=@LEAVE_SPECIAL_CD,SALARY_SETTLE_CD=@SALARY_SETTLE_CD, ");
            sb.Append(" IS_IFLOW_SHOW=@IS_IFLOW_SHOW,IS_USED=@IS_USED,IS_QRY_SHOW=@IS_QRY_SHOW, ");
            sb.Append(" UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID");
            sb.Append(" ,AWARD_DAY=@AWARD_DAY,BONUS_DAY=@BONUS_DAY,PLAN_DAY=@PLAN_DAY");
            sb.Append(" ,IS_ASSESS=@IS_ASSESS,IS_ACC_HOUR=@IS_ACC_HOUR,DH_WORK_DAY_CD=@DH_WORK_DAY_CD,DH_SEX_CD=@DH_SEX_CD");
            sb.Append(" where MAIN_LEAVE_CD=@MAIN_LEAVE_CD ");
            sb.Append(" and SUB_LEAVE_CD=@SUB_LEAVE_CD ");

            ht.Add("@MAIN_LEAVE_CD", MAIN_LEAVE_CD);
            ht.Add("@SUB_LEAVE_CD", SUB_LEAVE_CD);
            ht.Add("@SUB_LEAVE_DESC", SUB_LEAVE_DESC);
            ht.Add("@LEAVE_PAY_RATE", LEAVE_PAY_RATE);
            ht.Add("@LEAVE_TIME_UNIT", LEAVE_TIME_UNIT);
            ht.Add("@LEAVE_COUNT_HOUR", LEAVE_COUNT_HOUR);
            ht.Add("@LEAVE_MIN_VALUE", LEAVE_MIN_VALUE);
            ht.Add("@LEAVE_COUNT_CD", LEAVE_COUNT_CD);
            ht.Add("@LEAVE_MAX_DAY_CD", LEAVE_MAX_DAY_CD);
            ht.Add("@IS_INCLUDE_HOLIDAY", IS_INCLUDE_HOLIDAY);
            ht.Add("@LEAVE_TIME_LIMIT_CD", LEAVE_TIME_LIMIT_CD);
            ht.Add("@LEAVE_ALLOW_CD", LEAVE_ALLOW_CD);
            ht.Add("@LEAVE_SPECIAL_CD", LEAVE_SPECIAL_CD);
            ht.Add("@SALARY_SETTLE_CD", SALARY_SETTLE_CD);
            ht.Add("@IS_IFLOW_SHOW", IS_IFLOW_SHOW);
            ht.Add("@IS_USED", IS_USED);
            ht.Add("@IS_QRY_SHOW", IS_QRY_SHOW);

            ht.Add("@AWARD_DAY", AWARD_DAY);
            ht.Add("@BONUS_DAY", BONUS_DAY);
            ht.Add("@PLAN_DAY", PLAN_DAY);
            ht.Add("@IS_ASSESS", IS_ASSESS);
            ht.Add("@IS_ACC_HOUR", IS_ACC_HOUR);
            ht.Add("@DH_WORK_DAY_CD", DH_WORK_DAY_CD);
            ht.Add("@DH_SEX_CD", DH_SEX_CD);

            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void addLEAVE_TYPE_D()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("insert into TB_D_M_LEAVE_TYPE_D ( ");
            sb.Append(" MAIN_LEAVE_CD,SUB_LEAVE_CD,SUB_LEAVE_DESC,LEAVE_PAY_RATE,LEAVE_TIME_UNIT, ");
            sb.Append(" LEAVE_COUNT_HOUR,LEAVE_MIN_VALUE,LEAVE_COUNT_CD,LEAVE_MAX_DAY_CD,IS_INCLUDE_HOLIDAY, ");
            sb.Append(" LEAVE_TIME_LIMIT_CD,LEAVE_ALLOW_CD,LEAVE_SPECIAL_CD,SALARY_SETTLE_CD,IS_IFLOW_SHOW, ");
            sb.Append(" IS_USED,IS_QRY_SHOW, ");
            sb.Append(" AWARD_DAY,BONUS_DAY,PLAN_DAY,IS_ASSESS,IS_ACC_HOUR,DH_WORK_DAY_CD,DH_SEX_CD, ");
            sb.Append(" CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" values ( ");
            sb.Append(" @MAIN_LEAVE_CD,@SUB_LEAVE_CD,@SUB_LEAVE_DESC,@LEAVE_PAY_RATE,@LEAVE_TIME_UNIT, ");
            sb.Append(" @LEAVE_COUNT_HOUR,@LEAVE_MIN_VALUE,@LEAVE_COUNT_CD,@LEAVE_MAX_DAY_CD,@IS_INCLUDE_HOLIDAY, ");
            sb.Append(" @LEAVE_TIME_LIMIT_CD,@LEAVE_ALLOW_CD,@LEAVE_SPECIAL_CD,@SALARY_SETTLE_CD,@IS_IFLOW_SHOW, ");
            sb.Append(" @IS_USED,@IS_QRY_SHOW, ");
            sb.Append(" @AWARD_DAY,@BONUS_DAY,@PLAN_DAY,@IS_ASSESS,@IS_ACC_HOUR,@DH_WORK_DAY_CD,@DH_SEX_CD, ");
            sb.Append(" @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@MAIN_LEAVE_CD", MAIN_LEAVE_CD);
            ht.Add("@SUB_LEAVE_CD", SUB_LEAVE_CD);
            ht.Add("@SUB_LEAVE_DESC", SUB_LEAVE_DESC);
            ht.Add("@LEAVE_PAY_RATE", LEAVE_PAY_RATE);
            ht.Add("@LEAVE_TIME_UNIT", LEAVE_TIME_UNIT);
            ht.Add("@LEAVE_COUNT_HOUR", LEAVE_COUNT_HOUR);
            ht.Add("@LEAVE_MIN_VALUE", LEAVE_MIN_VALUE);
            ht.Add("@LEAVE_COUNT_CD", LEAVE_COUNT_CD);
            ht.Add("@LEAVE_MAX_DAY_CD", LEAVE_MAX_DAY_CD);
            ht.Add("@IS_INCLUDE_HOLIDAY", IS_INCLUDE_HOLIDAY);
            ht.Add("@LEAVE_TIME_LIMIT_CD", LEAVE_TIME_LIMIT_CD);
            ht.Add("@LEAVE_ALLOW_CD", LEAVE_ALLOW_CD);
            ht.Add("@LEAVE_SPECIAL_CD", LEAVE_SPECIAL_CD);
            ht.Add("@SALARY_SETTLE_CD", SALARY_SETTLE_CD);
            ht.Add("@IS_IFLOW_SHOW", IS_IFLOW_SHOW);
            ht.Add("@IS_USED", IS_USED);
            ht.Add("@IS_QRY_SHOW", IS_QRY_SHOW);

            ht.Add("@AWARD_DAY", AWARD_DAY);
            ht.Add("@BONUS_DAY", BONUS_DAY);
            ht.Add("@PLAN_DAY", PLAN_DAY);
            ht.Add("@IS_ASSESS", IS_ASSESS);
            ht.Add("@IS_ACC_HOUR", IS_ACC_HOUR);
            ht.Add("@DH_WORK_DAY_CD", DH_WORK_DAY_CD);
            ht.Add("@DH_SEX_CD", DH_SEX_CD);

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

    public void deleteLEAVE_MAX_DAY(Tuple<string, string, string, string> item)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //寫log
            sb.Append(" update TB_D_M_LEAVE_MAX_DAY set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2DH010' ");
            sb.Append(" where MAIN_LEAVE_CD = @MAIN_LEAVE_CD");
            sb.Append(" and MERGE_SUB_LEAVE_CD = @MERGE_SUB_LEAVE_CD");
            sb.Append(" and START_DT_CD = @START_DT_CD");
            sb.Append(" and END_DT_CD = @END_DT_CD;");
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);

            sb.Append(" delete from TB_D_M_LEAVE_MAX_DAY");
            sb.Append(" where MAIN_LEAVE_CD = @MAIN_LEAVE_CD");
            sb.Append(" and MERGE_SUB_LEAVE_CD = @MERGE_SUB_LEAVE_CD");
            sb.Append(" and START_DT_CD = @START_DT_CD");
            sb.Append(" and END_DT_CD = @END_DT_CD;");

            ht.Add("@MAIN_LEAVE_CD", item.Item1);
            ht.Add("@MERGE_SUB_LEAVE_CD", item.Item2);
            ht.Add("@START_DT_CD", item.Item3.Split('-')[0]);
            ht.Add("@END_DT_CD", item.Item4.Split('-')[0]);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getLEAVE_MAX_DAY2()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select MAIN_LEAVE_CD from TB_D_M_LEAVE_MAX_DAY");
            sb.Append(" where MAIN_LEAVE_CD = @MAIN_LEAVE_CD");
            sb.Append(" and MERGE_SUB_LEAVE_CD = @MERGE_SUB_LEAVE_CD");

            ht.Add("@MAIN_LEAVE_CD", MAIN_LEAVE_CD);
            ht.Add("@MERGE_SUB_LEAVE_CD", MERGE_SUB_LEAVE_CD);

            return dbConn.Query(sb, ht);
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    public void addLEAVE_MAX_DAY()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("insert into TB_D_M_LEAVE_MAX_DAY( ");
            sb.Append(" MAIN_LEAVE_CD,MERGE_SUB_LEAVE_CD,LEAVE_MAX_DAY,START_DT_CD, ");
            sb.Append(" START_SIGN,START_DAY,END_DT_CD,END_SIGN,END_DAY, ");
            sb.Append(" CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" values ( ");
            sb.Append(" @MAIN_LEAVE_CD,@MERGE_SUB_LEAVE_CD,@LEAVE_MAX_DAY,@START_DT_CD, ");
            sb.Append(" @START_SIGN,@START_DAY,@END_DT_CD,@END_SIGN,@END_DAY, ");
            sb.Append(" @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@MAIN_LEAVE_CD", MAIN_LEAVE_CD);
            ht.Add("@MERGE_SUB_LEAVE_CD", MERGE_SUB_LEAVE_CD);
            ht.Add("@LEAVE_MAX_DAY", LEAVE_MAX_DAY);
            ht.Add("@START_DT_CD", START_DT_CD);
            ht.Add("@START_SIGN", START_SIGN);
            ht.Add("@START_DAY", START_DAY);
            ht.Add("@END_DT_CD", END_DT_CD);
            ht.Add("@END_SIGN", END_SIGN);
            ht.Add("@END_DAY", END_DAY);
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

    public void updateLEAVE_MAX_DAY()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_D_M_LEAVE_MAX_DAY ");
            sb.Append(" set LEAVE_MAX_DAY=@LEAVE_MAX_DAY,START_SIGN=@START_SIGN, ");
            sb.Append(" START_DAY=@START_DAY,END_SIGN=@END_SIGN,END_DAY=@END_DAY, ");
            sb.Append(" UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID");
            sb.Append(" where MAIN_LEAVE_CD=@MAIN_LEAVE_CD ");
            sb.Append(" and MERGE_SUB_LEAVE_CD=@MERGE_SUB_LEAVE_CD ");

            ht.Add("@MAIN_LEAVE_CD", MAIN_LEAVE_CD);
            ht.Add("@MERGE_SUB_LEAVE_CD", MERGE_SUB_LEAVE_CD);
            ht.Add("@LEAVE_MAX_DAY", LEAVE_MAX_DAY);
            ht.Add("@START_SIGN", START_SIGN);
            ht.Add("@START_DAY", START_DAY);
            ht.Add("@END_SIGN", END_SIGN);
            ht.Add("@END_DAY", END_DAY);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void deleteLEAVE_TIME_LIMIT(Tuple<string, string, string, string> item)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //寫log
            sb.Append(" update TB_D_M_LEAVE_TIME_LIMIT set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2DH010' ");
            sb.Append(" where MAIN_LEAVE_CD = @MAIN_LEAVE_CD ");
            sb.Append(" and SUB_LEAVE_CD = @SUB_LEAVE_CD ");
            sb.Append(" and START_TIME_CD = @START_TIME_CD ");
            sb.Append(" and END_TIME_CD = @END_TIME_CD; ");
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);

            sb.Append(" delete from TB_D_M_LEAVE_TIME_LIMIT ");
            sb.Append(" where MAIN_LEAVE_CD = @MAIN_LEAVE_CD ");
            sb.Append(" and SUB_LEAVE_CD = @SUB_LEAVE_CD ");
            sb.Append(" and START_TIME_CD = @START_TIME_CD ");
            sb.Append(" and END_TIME_CD = @END_TIME_CD; ");

            ht.Add("@MAIN_LEAVE_CD", item.Item1);
            ht.Add("@SUB_LEAVE_CD", item.Item2);
            ht.Add("@START_TIME_CD", item.Item3.Split('-')[0]);
            ht.Add("@END_TIME_CD", item.Item4.Split('-')[0]);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getLEAVE_TIME_LIMIT2()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select MAIN_LEAVE_CD from TB_D_M_LEAVE_TIME_LIMIT");
            sb.Append(" where MAIN_LEAVE_CD = @MAIN_LEAVE_CD");
            sb.Append(" and SUB_LEAVE_CD = @SUB_LEAVE_CD");
            sb.Append(" and START_TIME_CD = @START_TIME_CD");
            sb.Append(" and END_TIME_CD = @END_TIME_CD");

            ht.Add("@MAIN_LEAVE_CD", MAIN_LEAVE_CD);
            ht.Add("@SUB_LEAVE_CD", SUB_LEAVE_CD);
            ht.Add("@START_TIME_CD", START_TIME_CD);
            ht.Add("@END_TIME_CD", END_TIME_CD);

            return dbConn.Query(sb, ht);
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    public void addLEAVE_TIME_LIMIT()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("insert into TB_D_M_LEAVE_TIME_LIMIT( ");
            sb.Append(" MAIN_LEAVE_CD,SUB_LEAVE_CD,START_SIGN,START_TIME_CD,START_SIGN_AD,START_HOURS, ");
            sb.Append(" END_SIGN,END_TIME_CD,END_SIGN_AD,END_HOURS, ");
            sb.Append(" CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" values ( ");
            sb.Append(" @MAIN_LEAVE_CD,@SUB_LEAVE_CD,@START_SIGN,@START_TIME_CD,@START_SIGN_AD,@START_HOURS, ");
            sb.Append(" @END_SIGN,@END_TIME_CD,@END_SIGN_AD,@END_HOURS, ");
            sb.Append(" @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@MAIN_LEAVE_CD", MAIN_LEAVE_CD);
            ht.Add("@SUB_LEAVE_CD", SUB_LEAVE_CD);
            ht.Add("@START_SIGN", START_SIGN);
            ht.Add("@START_TIME_CD", START_TIME_CD);
            ht.Add("@START_SIGN_AD", START_SIGN_AD);
            ht.Add("@START_HOURS", START_HOURS);
            ht.Add("@END_SIGN", END_SIGN);
            ht.Add("@END_TIME_CD", END_TIME_CD);
            ht.Add("@END_SIGN_AD", END_SIGN_AD);
            ht.Add("@END_HOURS", END_HOURS);
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

    public void updateLEAVE_TIME_LIMIT()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_D_M_LEAVE_TIME_LIMIT ");
            sb.Append(" set START_SIGN=@START_SIGN,START_SIGN_AD=@START_SIGN_AD,START_HOURS=@START_HOURS, ");
            sb.Append(" END_SIGN=@END_SIGN,END_SIGN_AD=@END_SIGN_AD,END_HOURS=@END_HOURS, ");
            sb.Append(" UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID");
            sb.Append(" where MAIN_LEAVE_CD=@MAIN_LEAVE_CD and SUB_LEAVE_CD=@SUB_LEAVE_CD ");
            sb.Append(" and START_TIME_CD=@START_TIME_CD and END_TIME_CD=@END_TIME_CD ");

            ht.Add("@MAIN_LEAVE_CD", MAIN_LEAVE_CD);
            ht.Add("@SUB_LEAVE_CD", SUB_LEAVE_CD);
            ht.Add("@START_TIME_CD", START_TIME_CD.Split('-')[0]);
            ht.Add("@END_TIME_CD", END_TIME_CD.Split('-')[0]);
            ht.Add("@START_SIGN", START_SIGN);
            ht.Add("@START_SIGN_AD", START_SIGN_AD);
            ht.Add("@START_HOURS", START_HOURS);
            ht.Add("@END_SIGN", END_SIGN);
            ht.Add("@END_SIGN_AD", END_SIGN_AD);
            ht.Add("@END_HOURS", END_HOURS);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void deleteLEAVE_ALLOW(Tuple<string, string, string, string> item)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //寫log
            sb.Append(" update TB_D_M_LEAVE_ALLOW set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2DH010' ");
            sb.Append(" where MAIN_LEAVE_CD = @MAIN_LEAVE_CD");
            sb.Append(" and SUB_LEAVE_CD = @SUB_LEAVE_CD");
            sb.Append(" and EMP_CD = @EMP_CD   ");
            sb.Append(" and PJOB_CD = @PJOB_CD ");
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);

            sb.Append(" delete from TB_D_M_LEAVE_ALLOW");
            sb.Append(" where MAIN_LEAVE_CD = @MAIN_LEAVE_CD");
            sb.Append(" and SUB_LEAVE_CD = @SUB_LEAVE_CD");
            sb.Append(" and EMP_CD = @EMP_CD ");
            sb.Append(" and PJOB_CD = @PJOB_CD ");

            ht.Add("@MAIN_LEAVE_CD", item.Item1);
            ht.Add("@SUB_LEAVE_CD", item.Item2);
            ht.Add("@EMP_CD", item.Item3.Split('-')[0]);
            ht.Add("@PJOB_CD", item.Item4);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getLEAVE_APPLY2(Tuple<string, string, string, string> item)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select MAIN_LEAVE_CD from TB_D_M_LEAVE_APPLY");
            sb.Append(" where MAIN_LEAVE_CD = @MAIN_LEAVE_CD");
            sb.Append(" and SUB_LEAVE_CD = @SUB_LEAVE_CD");
            sb.Append(" and EMP_ID in ( select EMP_ID from TB_H_M_EMP where EMP_CD=@EMP_CD)");

            ht.Add("@MAIN_LEAVE_CD", item.Item1);
            ht.Add("@SUB_LEAVE_CD", item.Item2);
            ht.Add("@EMP_CD", item.Item3.Split('-')[0]);
            return dbConn.Query(sb, ht);
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    public DataTable getLEAVE_APPLY_DAY2(Tuple<string, string, string, string> item)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select MAIN_LEAVE_CD from TB_D_M_LEAVE_APPLY_DAY");
            sb.Append(" where MAIN_LEAVE_CD = @MAIN_LEAVE_CD");
            sb.Append(" and SUB_LEAVE_CD = @SUB_LEAVE_CD");
            sb.Append(" and EMP_ID in ( select EMP_ID from TB_H_M_EMP where EMP_CD=@EMP_CD)");

            ht.Add("@MAIN_LEAVE_CD", item.Item1);
            ht.Add("@SUB_LEAVE_CD", item.Item2);
            ht.Add("@EMP_CD", item.Item3.Split('-')[0]);
            return dbConn.Query(sb, ht);
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    public DataTable getLEAVE_ALLOW2()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select MAIN_LEAVE_CD from TB_D_M_LEAVE_ALLOW");
            sb.Append(" where MAIN_LEAVE_CD = @MAIN_LEAVE_CD");
            sb.Append(" and SUB_LEAVE_CD = @SUB_LEAVE_CD");
            sb.Append(" and EMP_CD = @EMP_CD");
            sb.Append(" and PJOB_CD = @PJOB_CD");

            ht.Add("@MAIN_LEAVE_CD", MAIN_LEAVE_CD);
            ht.Add("@SUB_LEAVE_CD", SUB_LEAVE_CD);
            ht.Add("@EMP_CD", EMP_CD);
            ht.Add("@PJOB_CD", PJOB_CD);
            

            return dbConn.Query(sb, ht);
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    public DataTable getPJOB()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * from VW_TB_H_M_PJOB");
            sb.Append(" where 1=1");
            sb.Append(" and PJOB_CD = @PJOB_CD");
            ht.Add("@PJOB_CD", PJOB_CD);

            return dbConn.Query(sb, ht);
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    public void addLEAVE_ALLOW()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("insert into TB_D_M_LEAVE_ALLOW( ");
            sb.Append(" MAIN_LEAVE_CD,SUB_LEAVE_CD,EMP_CD,PJOB_CD, ");
            sb.Append(" CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" values ( ");
            sb.Append(" @MAIN_LEAVE_CD,@SUB_LEAVE_CD,@EMP_CD,@PJOB_CD, ");
            sb.Append(" @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@MAIN_LEAVE_CD", MAIN_LEAVE_CD);
            ht.Add("@SUB_LEAVE_CD", SUB_LEAVE_CD);
            ht.Add("@EMP_CD", EMP_CD);
            ht.Add("@PJOB_CD", PJOB_CD);
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


    public DataTable getMAIN_LEAVE_DESC(string main_leave_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select MAIN_LEAVE_CD,MAIN_LEAVE_DESC ");
            sb.Append(" from TB_D_M_LEAVE_TYPE_H ");
            sb.Append(" where MAIN_LEAVE_CD=@MAIN_LEAVE_CD ");
            ht.Add("@MAIN_LEAVE_CD", main_leave_cd);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
}