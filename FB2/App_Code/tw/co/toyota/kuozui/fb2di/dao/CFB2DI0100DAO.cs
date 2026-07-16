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
/// CFB2DI0100DAO 的摘要描述
/// </summary>
public class CFB2DI0100DAO : BaseDAO
{
    public string OVERTIME_CD { get; set; }
    public string OVERTIME_DESC { get; set; }
    public string OVERTIME_DT_TYPE { get; set; }
    public string SHOUR_10 { get; set; }
    public string EHOUR_10 { get; set; }
    public string SHOUR_15 { get; set; }
    public string EHOUR_15 { get; set; }
    public string SHOUR_20 { get; set; }
    public string EHOUR_20 { get; set; }
    public string NONTAX_SHOUR { get; set; }
    public string NONTAX_EHOUR { get; set; }
    public string OTHER_SHOUR { get; set; }
    public string OTHER_EHOUR { get; set; }
    public string TAX_SHOUR { get; set; }
    public string TAX_EHOUR { get; set; }
    public string OVERTIME_EXCHANGE_CD { get; set; }
    public string SALARY_SETTLE_CD { get; set; }
    public string OVERTIME_ALLOW_CD { get; set; }
    public string IS_DUTY_CHECK { get; set; }
    public string IS_USED { get; set; }
    public string IS_IFLOW_SHOW { get; set; }
    public string REMARK { get; set; }
    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }

    public string WS_CD { get; set; }
    public string PJOB_CD { get; set; }
    public string WORK_CD { get; set; }

    public string WORK_DAY_CD { get; set; }
    public string O_HOUR_CD { get; set; }
    public string O_MUL_CD { get; set; }
    public string HYPER_SHOUR { get; set; }
    public string HYPER_EHOUR { get; set; }
    public string NORMAL_SHOUR { get; set; }
    public string NORMAL_EHOUR { get; set; }
    public string BASE_SHOUR { get; set; }
    public string BASE_EHOUR { get; set; }
    public string CHG_WORK_CD { get; set; }

    public CFB2DI0100DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    //加班類型維護查詢
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, 
        string overtime_cd, string overtime_dt_type, string is_used, string is_iflow_show)
    {
        try
        {
            if (sortExpression.Contains("OVERTIME_CD"))
                sortExpression = sortExpression.Replace("OVERTIME_CD", "a.OVERTIME_CD");
            if (sortExpression.Contains("OVERTIME_DT_TYPE"))
                sortExpression = sortExpression.Replace("OVERTIME_DT_TYPE", "a.OVERTIME_DT_TYPE");

            if (sortExpression.Contains("O_HOUR_CD"))
                sortExpression = sortExpression.Replace("O_HOUR_CD", "a.O_HOUR_CD");
            if (sortExpression.Contains("O_MUL_CD"))
                sortExpression = sortExpression.Replace("O_MUL_CD", "a.O_MUL_CD");
            if (sortExpression.Contains("HYPER_HOUR"))
                sortExpression = sortExpression.Replace("HYPER_HOUR", "a.HYPER_SHOUR");
            if (sortExpression.Contains("NORMAL_HOUR"))
                sortExpression = sortExpression.Replace("NORMAL_HOUR", "a.NORMAL_SHOUR");
            if (sortExpression.Contains("NONTAX_HOUR"))
                sortExpression = sortExpression.Replace("NONTAX_HOUR", "a.NONTAX_SHOUR");
            if (sortExpression.Contains("OTHER_HOUR"))
                sortExpression = sortExpression.Replace("OTHER_HOUR", "a.OTHER_SHOUR");
            if (sortExpression.Contains("TAX_HOUR"))
                sortExpression = sortExpression.Replace("TAX_HOUR", "a.TAX_SHOUR");

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * From");
            sb.Append(" (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" a.OVERTIME_CD,a.OVERTIME_DESC,a.WORK_DAY_CD+'-'+e.SUB_DESC WORK_DAY_CD, b.SUB_CD+'-'+b.SUB_DESC OVERTIME_DT_TYPE, ");
            sb.Append(" a.O_HOUR_CD+'-'+f.O_HOUR_DESC O_HOUR_CD,a.O_MUL_CD+'-'+g.O_MUL_DESC O_MUL_CD, ");
            sb.Append(" CONVERT(varchar,a.HYPER_SHOUR)+'~'+CONVERT(varchar,a.HYPER_EHOUR) HYPER_HOUR, ");
            sb.Append(" CONVERT(varchar,a.NORMAL_SHOUR)+'~'+CONVERT(varchar,a.NORMAL_EHOUR) NORMAL_HOUR, ");
            sb.Append(" CONVERT(varchar,a.NONTAX_SHOUR)+'~'+CONVERT(varchar,a.NONTAX_EHOUR) NONTAX_HOUR, ");
            sb.Append(" CONVERT(varchar,a.OTHER_SHOUR)+'~'+CONVERT(varchar,a.OTHER_EHOUR) OTHER_HOUR, ");
            sb.Append(" CONVERT(varchar,a.TAX_SHOUR)+'~'+CONVERT(varchar,a.TAX_EHOUR) TAX_HOUR, ");
            sb.Append(" c.SUB_CD+'-'+c.SUB_DESC OVERTIME_EXCHANGE_CD,d.SUB_CD+'-'+d.SUB_DESC SALARY_SETTLE_CD, ");
            sb.Append(" a.CHG_WORK_CD,a.IS_IFLOW_SHOW ");
            sb.Append(" from TB_D_M_OVERTIME_TYPE a ");
            sb.Append(" left join TB_9_M_COMM_D b on b.SYS_CD='DI' and b.MAIN_CD='OVERTIME_DT_TYPE' and b.IS_VALID='Y' ");
            sb.Append(" and a.OVERTIME_DT_TYPE=b.SUB_CD ");
            sb.Append(" left join TB_9_M_COMM_D c on c.SYS_CD='DI' and c.MAIN_CD='OVERTIME_EXCHANGE_CD' and c.IS_VALID='Y' ");
            sb.Append(" and a.OVERTIME_EXCHANGE_CD=c.SUB_CD ");
            sb.Append(" left join TB_9_M_COMM_D d on d.SYS_CD='DH' and d.MAIN_CD='SALARY_SETTLE_CD' and d.IS_VALID='Y' ");
            sb.Append(" and a.SALARY_SETTLE_CD=d.SUB_CD ");
            sb.Append(" left join TB_9_M_COMM_D e on e.SYS_CD='DA' and e.MAIN_CD='WORK_DAY_CD' and e.IS_VALID='Y' ");
            sb.Append(" and a.WORK_DAY_CD=e.SUB_CD ");
            sb.Append(" left join TB_D_M_HOUR_CD_H f on a.O_HOUR_CD=f.O_HOUR_CD ");
            sb.Append(" left join TB_D_M_MUL_CD_H g on a.O_MUL_CD=g.O_MUL_CD ");
            sb.Append(" where 1=1 ");

            if (overtime_cd != "-1" && overtime_cd != null)
            {
                sb.Append(" and a.OVERTIME_CD = @overtime_cd ");
                ht.Add("@overtime_cd", overtime_cd);
            }

            if (overtime_dt_type != "-1" && overtime_dt_type != null)
            {
                sb.Append(" and a.OVERTIME_DT_TYPE = @overtime_dt_type ");
                ht.Add("@overtime_dt_type", overtime_dt_type);
            }

            if (is_used != "-1" && is_used != null)
            {
                sb.Append(" and a.IS_USED = @is_used ");
                ht.Add("@is_used", is_used);
            }
            if (is_iflow_show != "-1" && is_iflow_show != null)
            {
                sb.Append(" and a.IS_IFLOW_SHOW = @is_iflow_show ");
                ht.Add("@is_iflow_show", is_iflow_show);
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

    public int getCount(int startRowIndex, int maximumRows, 
        string overtime_cd, string overtime_dt_type, string is_used, string is_iflow_show)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select COUNT(*) total_record ");
            sb.Append(" from TB_D_M_OVERTIME_TYPE a ");
            sb.Append(" where 1=1 ");

            if (overtime_cd != "-1" && overtime_cd != null)
            {
                sb.Append(" and a.OVERTIME_CD = @overtime_cd ");
                ht.Add("@overtime_cd", overtime_cd);
            }

            if (overtime_dt_type != "-1" && overtime_dt_type != null)
            {
                sb.Append(" and a.OVERTIME_DT_TYPE = @overtime_dt_type ");
                ht.Add("@overtime_dt_type", overtime_dt_type);
            }

            if (is_used != "-1" && is_used != null)
            {
                sb.Append(" and a.IS_USED = @is_used ");
                ht.Add("@is_used", is_used);
            }

            if (is_iflow_show != "-1" && is_iflow_show != null)
            {
                sb.Append(" and a.IS_IFLOW_SHOW = @is_iflow_show ");
                ht.Add("@is_iflow_show", is_iflow_show);
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

    //換休適用人員查詢
    public DataTable getData2(int startRowIndex, int maximumRows, string sortExpression, string overtime_cd, string overtime_dt_type)
    {
        try
        {
            if (sortExpression.Contains("OVERTIME_CD"))
                sortExpression = sortExpression.Replace("OVERTIME_CD", "a.OVERTIME_CD");

            if (sortExpression.Contains("OVERTIME_DT_TYPE"))
                sortExpression = sortExpression.Replace("OVERTIME_DT_TYPE", "a.OVERTIME_DT_TYPE");

            if (sortExpression.Contains("WS_CD"))
                sortExpression = sortExpression.Replace("WS_CD", "a.WS_CD");

            if (sortExpression.Contains("PJOB_CD"))
                sortExpression = sortExpression.Replace("PJOB_CD", "a.PJOB_CD");

            if (sortExpression.Contains("WORK_CD"))
                sortExpression = sortExpression.Replace("WORK_CD", "a.WORK_CD");

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * From");
            sb.Append(" (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" a.OVERTIME_CD,a.OVERTIME_DT_TYPE,b.SUB_CD+'-'+b.SUB_DESC WS_CD,c.SUB_CD+'-'+c.SUB_DESC WORK_CD, ");
            sb.Append(" d.PJOB_CD+'-'+d.PJOB_DESC PJOB_CD ");
            sb.Append(" from TB_D_M_OVERTIME_ALLOW a ");
            sb.Append(" left join TB_9_M_COMM_D b on b.SYS_CD='HB' and b.MAIN_CD='WS_CD' and b.IS_VALID='Y' and b.SUB_CD=a.WS_CD ");
            sb.Append(" left join TB_9_M_COMM_D c on c.SYS_CD='HB' and c.MAIN_CD='WORK_CD' and c.IS_VALID='Y' and c.SUB_CD=a.WORK_CD ");
            sb.Append(" left join TB_H_M_PJOB d on d.PJOB_CD=a.PJOB_CD and d.END_DT >= GETDATE() ");
            sb.Append(" where 1=1 ");

            if (overtime_cd != "")
            {
                sb.Append(" and a.OVERTIME_CD = @overtime_cd ");
                ht.Add("@overtime_cd", overtime_cd.Split('-')[0]);
            }

            if (overtime_dt_type != "")
            {
                sb.Append(" and a.OVERTIME_DT_TYPE = @overtime_dt_type ");
                ht.Add("@overtime_dt_type", overtime_dt_type.Split('-')[0]);
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

    public int getCount2(int startRowIndex, int maximumRows, string overtime_cd, string overtime_dt_type)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select COUNT(*) total_record ");
            sb.Append(" from TB_D_M_OVERTIME_ALLOW a ");
            sb.Append(" left join TB_9_M_COMM_D b on b.SYS_CD='HB' and b.MAIN_CD='WS_CD' and b.IS_VALID='Y' and b.SUB_CD=a.WS_CD ");
            sb.Append(" left join TB_9_M_COMM_D c on c.SYS_CD='HB' and c.MAIN_CD='WORK_CD' and c.IS_VALID='Y' and c.SUB_CD=a.WORK_CD ");
            sb.Append(" left join TB_H_M_PJOB d on d.PJOB_CD=a.PJOB_CD and d.END_DT >= GETDATE() ");
            sb.Append(" where 1=1 ");

            if (overtime_cd != "")
            {
                sb.Append(" and a.OVERTIME_CD = @overtime_cd ");
                ht.Add("@overtime_cd", overtime_cd.Split('-')[0]);
            }

            if (overtime_dt_type != "")
            {
                sb.Append(" and a.OVERTIME_DT_TYPE = @overtime_dt_type ");
                ht.Add("@overtime_dt_type", overtime_dt_type.Split('-')[0]);
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

    public DataTable getOVERTIME_CD()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select OVERTIME_CD+'-'+OVERTIME_DESC OVERTIME_DESC,OVERTIME_CD ");
            sb.Append(" from TB_D_M_OVERTIME_TYPE");
            sb.Append(" order by IS_IFLOW_SHOW desc, OVERTIME_CD");
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getOVERTIME_APPLY(string overtime_cd, string overtime_dt_type)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select OVERTIME_CD ");
            sb.Append(" from TB_D_M_OVERTIME_APPLY");
            sb.Append(" where OVERTIME_CD=@OVERTIME_CD and OVERTIME_DT_TYPE=@OVERTIME_DT_TYPE");
            sb.Append(" and FORM_STATUS not in ('N','D')");

            ht.Add("@OVERTIME_CD", overtime_cd);
            ht.Add("@OVERTIME_DT_TYPE", overtime_dt_type);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public void deleteOVERTIME_TYPE(Tuple<string, string> item)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //寫log
            sb.Append(" update TB_D_M_OVERTIME_TYPE set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2DI010' ");
            sb.Append(" where OVERTIME_CD = @OVERTIME_CD");
            sb.Append(" and OVERTIME_DT_TYPE = @OVERTIME_DT_TYPE;");
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);

            sb.Append(" delete from TB_D_M_OVERTIME_TYPE");
            sb.Append(" where OVERTIME_CD = @OVERTIME_CD");
            sb.Append(" and OVERTIME_DT_TYPE = @OVERTIME_DT_TYPE;");
            ht.Add("@OVERTIME_CD", item.Item1);
            ht.Add("@OVERTIME_DT_TYPE", item.Item2);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public void deleteOVERTIME_ALLOW(Tuple<string, string> item)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //寫log
            sb.Append(" update TB_D_M_OVERTIME_ALLOW set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2DI010' ");
            sb.Append(" where OVERTIME_CD = @OVERTIME_CD");
            sb.Append(" and OVERTIME_DT_TYPE = @OVERTIME_DT_TYPE;");
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);

            sb.Append(" delete from TB_D_M_OVERTIME_ALLOW");
            sb.Append(" where OVERTIME_CD = @OVERTIME_CD");
            sb.Append(" and OVERTIME_DT_TYPE = @OVERTIME_DT_TYPE;");
            ht.Add("@OVERTIME_CD", item.Item1);
            ht.Add("@OVERTIME_DT_TYPE", item.Item2);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getOVERTIME_IFLOW(string overtime_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select OVERTIME_CD ");
            sb.Append(" from VW_D_M_OVERTIME_FLOW");
            sb.Append(" where OVERTIME_CD = @OVERTIME_CD");
            ht.Add("@OVERTIME_CD", overtime_cd);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getDefaultData(string overtime_cd, string overtime_dt_type)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select OVERTIME_CD,OVERTIME_DESC,WORK_DAY_CD,OVERTIME_DT_TYPE,O_HOUR_CD,O_MUL_CD ");
            sb.Append(" ,HYPER_SHOUR,HYPER_EHOUR, NONTAX_SHOUR,NONTAX_EHOUR ");
            sb.Append(" ,NORMAL_SHOUR,NORMAL_EHOUR, OTHER_SHOUR,OTHER_EHOUR ");
            sb.Append(" ,BASE_SHOUR,BASE_EHOUR, TAX_SHOUR,TAX_EHOUR ");
            sb.Append(" ,OVERTIME_EXCHANGE_CD,SALARY_SETTLE_CD,CHG_WORK_CD ");
            sb.Append(" ,IS_USED,IS_IFLOW_SHOW,REMARK ");
            sb.Append(" from TB_D_M_OVERTIME_TYPE ");
            sb.Append(" where OVERTIME_CD = @OVERTIME_CD and OVERTIME_DT_TYPE=@OVERTIME_DT_TYPE ");
            ht.Add("@OVERTIME_CD", overtime_cd);
            ht.Add("@OVERTIME_DT_TYPE", overtime_dt_type);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getOVERTIME_TYPE(string overtime_cd, string overtime_dt_type)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select OVERTIME_CD ");
            sb.Append(" from TB_D_M_OVERTIME_TYPE");
            sb.Append(" where OVERTIME_CD = @OVERTIME_CD and OVERTIME_DT_TYPE=@OVERTIME_DT_TYPE ");
            ht.Add("@OVERTIME_CD", overtime_cd);
            ht.Add("@OVERTIME_DT_TYPE", overtime_dt_type);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public void updateOVERTIME_TYPE()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_D_M_OVERTIME_TYPE set ");
            sb.Append(" OVERTIME_DESC=@OVERTIME_DESC, WORK_DAY_CD=@WORK_DAY_CD, OVERTIME_DT_TYPE=@OVERTIME_DT_TYPE, O_HOUR_CD=@O_HOUR_CD, O_MUL_CD=@O_MUL_CD ");
            sb.Append(" , HYPER_SHOUR=@HYPER_SHOUR, HYPER_EHOUR=@HYPER_EHOUR, NORMAL_SHOUR=@NORMAL_SHOUR, NORMAL_EHOUR=@NORMAL_EHOUR, BASE_SHOUR=@BASE_SHOUR ");
            sb.Append(" , BASE_EHOUR=@BASE_EHOUR, NONTAX_SHOUR=@NONTAX_SHOUR, NONTAX_EHOUR=@NONTAX_EHOUR, TAX_SHOUR=@TAX_SHOUR, TAX_EHOUR=@TAX_EHOUR ");
            sb.Append(" , OTHER_SHOUR=@OTHER_SHOUR, OTHER_EHOUR=@OTHER_EHOUR, OVERTIME_EXCHANGE_CD=@OVERTIME_EXCHANGE_CD, SALARY_SETTLE_CD=@SALARY_SETTLE_CD, CHG_WORK_CD=@CHG_WORK_CD ");
            sb.Append(" , IS_USED=@IS_USED, IS_IFLOW_SHOW=@IS_IFLOW_SHOW, REMARK=@REMARK ");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID ");
            sb.Append(" where OVERTIME_CD=@OVERTIME_CD and OVERTIME_DT_TYPE=@OVERTIME_DT_TYPE ");

            ht.Add("@OVERTIME_CD", OVERTIME_CD);
            ht.Add("@OVERTIME_DT_TYPE", OVERTIME_DT_TYPE);
            ht.Add("@OVERTIME_DESC", OVERTIME_DESC);
            ht.Add("@WORK_DAY_CD", WORK_DAY_CD);
            ht.Add("@O_HOUR_CD", O_HOUR_CD);
            ht.Add("@O_MUL_CD", O_MUL_CD);
            ht.Add("@HYPER_SHOUR", HYPER_SHOUR);
            ht.Add("@HYPER_EHOUR", HYPER_EHOUR);
            ht.Add("@NORMAL_SHOUR", NORMAL_SHOUR);
            ht.Add("@NORMAL_EHOUR", NORMAL_EHOUR);

            if (BASE_SHOUR == "")
                ht.Add("@BASE_SHOUR", 0);
            else
                ht.Add("@BASE_SHOUR", BASE_SHOUR);
            if (BASE_EHOUR == "")
                ht.Add("@BASE_EHOUR", 0);
            else
                ht.Add("@BASE_EHOUR", BASE_EHOUR);

            if (NONTAX_SHOUR == "")
                ht.Add("@NONTAX_SHOUR", 0);
            else
                ht.Add("@NONTAX_SHOUR", NONTAX_SHOUR);
            if (NONTAX_EHOUR == "")
                ht.Add("@NONTAX_EHOUR", 0);
            else
                ht.Add("@NONTAX_EHOUR", NONTAX_EHOUR);

            if (TAX_SHOUR == "")
                ht.Add("@TAX_SHOUR", 0);
            else
                ht.Add("@TAX_SHOUR", TAX_SHOUR);
            if (TAX_EHOUR == "")
                ht.Add("@TAX_EHOUR", 0);
            else
                ht.Add("@TAX_EHOUR", TAX_EHOUR);

            if (OTHER_SHOUR == "")
                ht.Add("@OTHER_SHOUR", 0);
            else
                ht.Add("@OTHER_SHOUR", OTHER_SHOUR);
            if (OTHER_EHOUR == "")
                ht.Add("@OTHER_EHOUR", 0);
            else
                ht.Add("@OTHER_EHOUR", OTHER_EHOUR);

            ht.Add("@OVERTIME_EXCHANGE_CD", OVERTIME_EXCHANGE_CD);
            ht.Add("@SALARY_SETTLE_CD", SALARY_SETTLE_CD);
            ht.Add("@CHG_WORK_CD", CHG_WORK_CD);
            ht.Add("@IS_USED", IS_USED);
            ht.Add("@IS_IFLOW_SHOW", IS_IFLOW_SHOW);
            ht.Add("@REMARK", REMARK);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void addOVERTIME_TYPE()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" insert into TB_D_M_OVERTIME_TYPE (  ");
            sb.Append(" OVERTIME_CD, OVERTIME_DESC, WORK_DAY_CD, OVERTIME_DT_TYPE, O_HOUR_CD ");
            sb.Append(" , O_MUL_CD, HYPER_SHOUR, HYPER_EHOUR, NORMAL_SHOUR, NORMAL_EHOUR ");
            sb.Append(" , BASE_SHOUR, BASE_EHOUR, NONTAX_SHOUR, NONTAX_EHOUR, TAX_SHOUR ");
            sb.Append(" , TAX_EHOUR, OTHER_SHOUR, OTHER_EHOUR, OVERTIME_EXCHANGE_CD, SALARY_SETTLE_CD ");
            sb.Append(" , CHG_WORK_CD, IS_USED, IS_IFLOW_SHOW, REMARK, START_DT ");
            sb.Append(" , END_DT, SHOUR_10, EHOUR_10, SHOUR_15, EHOUR_15 ");
            sb.Append(" , SHOUR_20, EHOUR_20, OVERTIME_ALLOW_CD, IS_DUTY_CHECK ");
            sb.Append(" ,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID ");
            sb.Append(" ) values ( ");
            sb.Append(" @OVERTIME_CD, @OVERTIME_DESC, @WORK_DAY_CD, @OVERTIME_DT_TYPE, @O_HOUR_CD ");
            sb.Append(" , @O_MUL_CD, @HYPER_SHOUR, @HYPER_EHOUR, @NORMAL_SHOUR, @NORMAL_EHOUR ");
            sb.Append(" , @BASE_SHOUR, @BASE_EHOUR, @NONTAX_SHOUR, @NONTAX_EHOUR, @TAX_SHOUR ");
            sb.Append(" , @TAX_EHOUR, @OTHER_SHOUR, @OTHER_EHOUR, @OVERTIME_EXCHANGE_CD, @SALARY_SETTLE_CD ");
            sb.Append(" , @CHG_WORK_CD, @IS_USED, @IS_IFLOW_SHOW, @REMARK, @START_DT ");
            sb.Append(" , @END_DT, @SHOUR_10, @EHOUR_10, @SHOUR_15, @EHOUR_15 ");
            sb.Append(" , @SHOUR_20, @EHOUR_20, @OVERTIME_ALLOW_CD, @IS_DUTY_CHECK,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID ) ");


            ht.Add("@OVERTIME_CD", OVERTIME_CD);
            ht.Add("@OVERTIME_DT_TYPE", OVERTIME_DT_TYPE);
            ht.Add("@OVERTIME_DESC", OVERTIME_DESC);
            ht.Add("@WORK_DAY_CD", WORK_DAY_CD);
            ht.Add("@O_HOUR_CD", O_HOUR_CD);
            ht.Add("@O_MUL_CD", O_MUL_CD);
            ht.Add("@HYPER_SHOUR", HYPER_SHOUR);
            ht.Add("@HYPER_EHOUR", HYPER_EHOUR);
            ht.Add("@NORMAL_SHOUR", NORMAL_SHOUR);
            ht.Add("@NORMAL_EHOUR", NORMAL_EHOUR);

            if (BASE_SHOUR == "")
                ht.Add("@BASE_SHOUR", 0);
            else
                ht.Add("@BASE_SHOUR", BASE_SHOUR);
            if (BASE_EHOUR == "")
                ht.Add("@BASE_EHOUR", 0);
            else
                ht.Add("@BASE_EHOUR", BASE_EHOUR);


            if (NONTAX_SHOUR == "")
                ht.Add("@NONTAX_SHOUR", 0);
            else
                ht.Add("@NONTAX_SHOUR", NONTAX_SHOUR);
            if (NONTAX_EHOUR == "")
                ht.Add("@NONTAX_EHOUR", 0);
            else
                ht.Add("@NONTAX_EHOUR", NONTAX_EHOUR);
            if (OTHER_SHOUR == "")
                ht.Add("@OTHER_SHOUR", 0);
            else
                ht.Add("@OTHER_SHOUR", OTHER_SHOUR);
            if (OTHER_EHOUR == "")
                ht.Add("@OTHER_EHOUR", 0);
            else
                ht.Add("@OTHER_EHOUR", OTHER_EHOUR);
            if (TAX_SHOUR == "")
                ht.Add("@TAX_SHOUR", 0);
            else
                ht.Add("@TAX_SHOUR", TAX_SHOUR);
            if (TAX_EHOUR == "")
                ht.Add("@TAX_EHOUR", 0);
            else
                ht.Add("@TAX_EHOUR", TAX_EHOUR);

            ht.Add("@OVERTIME_EXCHANGE_CD", OVERTIME_EXCHANGE_CD);
            ht.Add("@SALARY_SETTLE_CD", SALARY_SETTLE_CD);
            ht.Add("@CHG_WORK_CD", CHG_WORK_CD);
            ht.Add("@IS_USED", IS_USED);
            ht.Add("@IS_IFLOW_SHOW", IS_IFLOW_SHOW);
            ht.Add("@REMARK", REMARK);
            ht.Add("@CREATED_BY", CREATED_BY);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);


            ht.Add("@START_DT", DBNull.Value);
            ht.Add("@END_DT", DBNull.Value);

            if (SHOUR_10 == "" || SHOUR_10 == null)
                ht.Add("@SHOUR_10", 0);
            else
                ht.Add("@SHOUR_10", SHOUR_10);
            if (EHOUR_10 == "" || EHOUR_10 == null)
                ht.Add("@EHOUR_10", 0);
            else
                ht.Add("@EHOUR_10", EHOUR_10);
            if (SHOUR_15 == "" || SHOUR_15 == null)
                ht.Add("@SHOUR_15", 0);
            else
                ht.Add("@SHOUR_15", SHOUR_15);
            if (EHOUR_15 == "" || EHOUR_15 == null)
                ht.Add("@EHOUR_15", 0);
            else
                ht.Add("@EHOUR_15", EHOUR_15);
            if (SHOUR_20 == "" || SHOUR_20 == null)
                ht.Add("@SHOUR_20", 0);
            else
                ht.Add("@SHOUR_20", SHOUR_20);
            if (EHOUR_20 == "" || EHOUR_20 == null)
                ht.Add("@EHOUR_20", 0);
            else
                ht.Add("@EHOUR_20", EHOUR_20);

            ht.Add("@OVERTIME_ALLOW_CD", "");
            ht.Add("@IS_DUTY_CHECK", "");


            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable getOVERTIME_APPLY2(string overtime_cd, string overtime_dt_type, string ws_cd, string pjob_cd, string work_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select OVERTIME_CD ");
            sb.Append(" from TB_D_M_OVERTIME_APPLY");
            sb.Append(" where OVERTIME_CD=@OVERTIME_CD and OVERTIME_DT_TYPE=@OVERTIME_DT_TYPE");
            sb.Append(" and FORM_STATUS not in ('N','D')");
            sb.Append(" and EMP_ID in (select EMP_ID from TB_H_M_EMP ");
            sb.Append(" where WS_CD=@WS_CD and PJOB_CD=@PJOB_CD and WORK_CD=@WORK_CD)");

            ht.Add("@OVERTIME_CD", overtime_cd);
            ht.Add("@OVERTIME_DT_TYPE", overtime_dt_type);
            ht.Add("@WS_CD", ws_cd);
            ht.Add("@PJOB_CD", pjob_cd);
            ht.Add("@WORK_CD", work_cd);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getOVERTIME_IFLOW2(string overtime_cd, string ws_cd, string pjob_cd, string work_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select OVERTIME_CD ");
            sb.Append(" from VW_D_M_OVERTIME_FLOW");
            sb.Append(" where OVERTIME_CD = @OVERTIME_CD");
            sb.Append(" and EMP_ID in (select EMP_ID from TB_H_M_EMP ");
            sb.Append(" where WS_CD=@WS_CD and PJOB_CD=@PJOB_CD and WORK_CD=@WORK_CD)");

            ht.Add("@OVERTIME_CD", overtime_cd);
            ht.Add("@WS_CD", ws_cd);
            ht.Add("@PJOB_CD", pjob_cd);
            ht.Add("@WORK_CD", work_cd);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public void deleteOVERTIME_ALLOW2(Tuple<string, string, string, string, string> item)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //寫log
            sb.Append(" update TB_D_M_OVERTIME_ALLOW set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2DI010' ");
            sb.Append(" where OVERTIME_CD = @OVERTIME_CD");
            sb.Append(" and OVERTIME_DT_TYPE = @OVERTIME_DT_TYPE");
            sb.Append(" and WS_CD=@WS_CD and PJOB_CD=@PJOB_CD and WORK_CD=@WORK_CD;");
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);

            sb.Append(" delete from TB_D_M_OVERTIME_ALLOW");
            sb.Append(" where OVERTIME_CD = @OVERTIME_CD");
            sb.Append(" and OVERTIME_DT_TYPE = @OVERTIME_DT_TYPE");
            sb.Append(" and WS_CD=@WS_CD and PJOB_CD=@PJOB_CD and WORK_CD=@WORK_CD;");

            ht.Add("@OVERTIME_CD", item.Item1);
            ht.Add("@OVERTIME_DT_TYPE", item.Item2);
            ht.Add("@WS_CD", item.Item3);
            ht.Add("@PJOB_CD", item.Item4);
            ht.Add("@WORK_CD", item.Item5);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
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
            sb.Append(" select OVERTIME_CD from TB_D_M_OVERTIME_ALLOW");
            sb.Append(" where OVERTIME_CD = @OVERTIME_CD");
            sb.Append(" and OVERTIME_DT_TYPE = @OVERTIME_DT_TYPE");
            sb.Append(" and WS_CD=@WS_CD and PJOB_CD=@PJOB_CD and WORK_CD=@WORK_CD");

            ht.Add("@OVERTIME_CD", OVERTIME_CD);
            ht.Add("@OVERTIME_DT_TYPE", OVERTIME_DT_TYPE);
            ht.Add("@WS_CD", WS_CD);
            ht.Add("@PJOB_CD", PJOB_CD);
            ht.Add("@WORK_CD", WORK_CD);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public void addOVERTIME_ALLOW()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("insert into TB_D_M_OVERTIME_ALLOW( ");
            sb.Append(" OVERTIME_CD,OVERTIME_DT_TYPE,WS_CD,PJOB_CD,WORK_CD,");
            sb.Append(" CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" values ( ");
            sb.Append(" @OVERTIME_CD,@OVERTIME_DT_TYPE,@WS_CD,@PJOB_CD,@WORK_CD,");
            sb.Append(" @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@OVERTIME_CD", OVERTIME_CD);
            ht.Add("@OVERTIME_DT_TYPE", OVERTIME_DT_TYPE);
            ht.Add("@WS_CD", WS_CD);
            ht.Add("@PJOB_CD", PJOB_CD);
            ht.Add("@WORK_CD", WORK_CD);
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

    internal DataTable getO_HOUR_CD()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select O_HOUR_CD,O_HOUR_CD+'-'+O_HOUR_DESC O_HOUR_DESC ");
            sb.Append(" from TB_D_M_HOUR_CD_H ");
            sb.Append(" where IS_USED='Y' ");

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getO_MUL_CD()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select O_MUL_CD, O_MUL_CD+'-'+O_MUL_DESC O_MUL_DESC ");
            sb.Append(" from TB_D_M_MUL_CD_H ");
            sb.Append(" where IS_USED='Y' ");

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

}