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
using System.Data.Odbc;


/// <summary>
/// CFB2SC2100DAO 的摘要描述
/// </summary>
public class CFB2SC2100DAO : BaseDAO
{
    public CFB2SC2100DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public string RowNumber { get; set; }
    public string SALARY_DT { get; set; }
    public string SALARY_YM { get; set; }
    public string SALARY_SDT { get; set; }
    public string SALARY_EDT { get; set; }
    public string DUTY_SDT { get; set; }
    public string DUTY_EDT { get; set; }
    public string SALARY_TYPE { get; set; }
    public string PROCESS_STATUS { get; set; }
    public string SALARY_ID { get; set; }
    public string OPERATION_ID { get; set; }
    public string IACYC { get; set; }
    //for查詢欄位
    public string ddl_SYS_CD { get; set; }

    #region "Intial Page"
    public DataTable getCommCode(string sys_cd, string main_cd, string is_valid)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select sub_cd ,sub_cd+'-'+sub_desc as sub_desc ");
            sb.Append(" from TB_9_M_COMM_D ");
            sb.Append(" where SYS_CD = @SYS_CD ");
            sb.Append(" and MAIN_CD = @MAIN_CD ");
            ht.Add("@SYS_CD", sys_cd);
            ht.Add("@MAIN_CD", main_cd);
            if (!string.IsNullOrEmpty(is_valid))
            {
                sb.Append(" and IS_VALID = @IS_VALID ");
                ht.Add("@IS_VALID", is_valid);
            }

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    #endregion

    #region Qry

    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string salary_dt_s, string salary_dt_e,
                             string salary_ym, string salary_sdt, string salary_edt, string duty_sdt, string duty_edt, string salary_type)
    {
        try
        {
            if (sortExpression.Contains("SALARY_YM"))
                sortExpression = sortExpression.Replace("SALARY_YM", "t.SALARY_YM");
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" select * from");
            sb.AppendLine("      (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber, ");
            sb.AppendLine("              t.SALARY_DT,t.SALARY_YM,t.SALARY_TYPE,t.SALARY_SDT ,t.SALARY_EDT ,t.DUTY_SDT,t.DUTY_EDT,t.PROCESS_STATUS ");
            sb.AppendLine("             ,t.PROCESS_STATUS +'-'+ d.SUB_DESC as PROCESS_STATUS_DESC                                                 ");
            sb.AppendLine("             ,t.SALARY_TYPE +'-'+ e.SUB_DESC as SALARY_TYPE_DESC                                                       ");
            sb.AppendLine("             ,t.PAY_KIND,t.PAY_KIND +'-'+ p.SALARY_NAME as PAY_KIND_DESC                                               ");
            sb.AppendLine(" 	        ,t.SALARY_TYPE + CONVERT(varchar(100), t.SALARY_DT , 111) as qdatakey                                     ");
            sb.AppendLine(" 	        ,t.IACYC                                     ");
            sb.AppendLine("        from TB_S_M_SALARY_CAL_H t                                                                                     ");
            sb.AppendLine("        left join TB_9_M_COMM_D d on  d.SYS_CD ='SC' and  d.MAIN_CD='PROCESS_STATUS' and  t.PROCESS_STATUS = d.SUB_CD  ");
            sb.AppendLine("        left join TB_9_M_COMM_D e on  e.SYS_CD ='SC' and  e.MAIN_CD='SALARY_TYPE' and  t.SALARY_TYPE = e.SUB_CD     ");
            sb.AppendLine("        left join VW_SALARYAND9999 p on  t.PAY_KIND = p.SALARY_ID                                              ");
            sb.AppendLine(" where 1=1                                                                                                      ");

            if (salary_dt_s != "")
            {
                sb.AppendLine(" and t.SALARY_DT >= @SALARY_DT_S  ");
                ht.Add("@SALARY_DT_S", salary_dt_s);
            }
            if (salary_dt_e != "")
            {
                sb.AppendLine(" and t.SALARY_DT <= @SALARY_DT_E  ");
                ht.Add("@SALARY_DT_E", salary_dt_e);
            }
            if (salary_ym != "")
            {
                sb.AppendLine(" and t.SALARY_YM = @SALARY_YM ");
                ht.Add("@SALARY_YM", salary_ym.Replace("/", ""));
            }
            if (salary_sdt != "")
            {
                sb.AppendLine(" and  t.SALARY_SDT >= @SALARY_SDT ");
                ht.Add("@SALARY_SDT", salary_sdt);
            }
            if (salary_edt != "")
            {
                sb.AppendLine(" and  t.SALARY_EDT <= @SALARY_EDT  ");
                ht.Add("@SALARY_EDT", salary_edt);
            }
            if (duty_sdt != "")
            {
                sb.AppendLine(" and t.DUTY_SDT >= @DUTY_SDT  ");
                ht.Add("@DUTY_SDT", duty_sdt);
            }
            if (duty_edt != "")
            {
                sb.AppendLine(" and t.DUTY_EDT <= @DUTY_EDT  ");
                ht.Add("@DUTY_EDT", duty_edt);
            }
            if (salary_type != "")
            {
                sb.AppendLine(" and t.SALARY_TYPE = @SALARY_TYPE  ");
                ht.Add("@SALARY_TYPE", salary_type);
            }

            sb.AppendLine("     )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.AppendLine(" AND CAST(@startRowIndex+@maximumRows as varchar)");

            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);

            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public int getCount(int startRowIndex, int maximumRows, string salary_dt_s, string salary_dt_e,
                             string salary_ym, string salary_sdt, string salary_edt, string duty_sdt, string duty_edt, string salary_type)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" Select COUNT(*) total_record ");
            sb.AppendLine(" from TB_S_M_SALARY_CAL_H t                                                                                     ");
            sb.AppendLine(" left join TB_9_M_COMM_D d on  d.SYS_CD ='SC' and  d.MAIN_CD='PROCESS_STATUS' and  t.PROCESS_STATUS = d.SUB_CD  ");
            sb.AppendLine(" left join TB_9_M_COMM_D e on  e.SYS_CD ='SC' and  e.MAIN_CD='SALARY_TYPE' and  t.SALARY_TYPE = e.SUB_CD     ");
            sb.AppendLine(" left join VW_SALARYAND9999 p on  t.PAY_KIND = p.SALARY_ID                                              ");
            sb.AppendLine(" where 1=1                                                                                                      ");

            if (salary_dt_s != "")
            {
                sb.AppendLine(" and t.SALARY_DT >= @SALARY_DT_S  ");
                ht.Add("@SALARY_DT_S", salary_dt_s);
            }
            if (salary_dt_e != "")
            {
                sb.AppendLine(" and t.SALARY_DT <= @SALARY_DT_E  ");
                ht.Add("@SALARY_DT_E", salary_dt_e);
            }
            if (salary_ym != "")
            {
                sb.AppendLine(" and t.SALARY_YM = @SALARY_YM ");
                ht.Add("@SALARY_YM", salary_ym.Replace("/", ""));
            }
            if (salary_sdt != "")
            {
                sb.AppendLine(" and  t.SALARY_SDT >= @SALARY_SDT ");
                ht.Add("@SALARY_SDT", salary_sdt);
            }
            if (salary_edt != "")
            {
                sb.AppendLine(" and  t.SALARY_EDT <= @SALARY_EDT  ");
                ht.Add("@SALARY_EDT", salary_edt);
            }
            if (duty_sdt != "")
            {
                sb.AppendLine(" and t.DUTY_SDT >= @DUTY_SDT  ");
                ht.Add("@DUTY_SDT", duty_sdt);
            }
            if (duty_edt != "")
            {
                sb.AppendLine(" and t.DUTY_EDT <= @DUTY_EDT  ");
                ht.Add("@DUTY_EDT", duty_edt);
            }
            if (salary_type != "")
            {
                sb.AppendLine(" and t.SALARY_TYPE = @SALARY_TYPE  ");
                ht.Add("@SALARY_TYPE", salary_type);
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
    public string deleteData(string deleteitem)
    {
        //刪除主檔
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.AppendLine(" delete from TB_S_M_SALARY_CAL_H ");
        sb.AppendLine(" where SALARY_TYPE + CONVERT(varchar(100), SALARY_DT , 111) +PAY_KIND = @qdatakey ");
        ht.Add("@qdatakey", deleteitem);
        dbConn.ExecuteT(sb, ht, true);

        //刪除明細
        StringBuilder sb2 = new StringBuilder();
        Hashtable ht2 = new Hashtable();
        sb2.AppendLine(" delete from TB_S_M_SALARY_CAL_D ");
        sb2.AppendLine(" where SALARY_TYPE + CONVERT(varchar(100), SALARY_DT , 111) +PAY_KIND = @qdatakey ");
        ht2.Add("@qdatakey", deleteitem);
        dbConn.ExecuteT(sb2, ht2, true);
        return "0";
    }
    public int addConfirm(string salary_type)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" select count(1) as total from TB_S_M_SALARY_CAL_H ");
            sb.AppendLine(" where PROCESS_STATUS <> '4' and SALARY_TYPE = @SALARY_TYPE ");
            ht.Add("@SALARY_TYPE", salary_type);
            DataTable dt = dbConn.Query(sb, ht);
            return Convert.ToInt32(dt.Rows[0]["total"]);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //public DataTable getExistData()
    //{
    //    try
    //    {
    //        StringBuilder sb = new StringBuilder();
    //        Hashtable ht = new Hashtable();

    //        sb.AppendLine("Select * from TB_S_M_SALARY_GROUP_H ");
    //        sb.AppendLine(" where KIND_CD+GROUP_TYPE+GROUP_ID = @KIND_CD+@GROUP_TYPE+@GROUP_ID ");
    //        ht.Add("@KIND_CD", KIND_CD);
    //        ht.Add("@GROUP_TYPE", GROUP_TYPE);
    //        ht.Add("@GROUP_ID", GROUP_ID);

    //        return dbConn.Query(sb, ht);
    //    }
    //    catch (Exception)
    //    {

    //        throw;
    //    }
    //}
    //public void addData()
    //{
    //    try
    //    {
    //        StringBuilder sb = new StringBuilder();
    //        Hashtable ht = new Hashtable();

    //        sb.AppendLine(" insert into TB_S_M_SALARY_GROUP_H (KIND_CD,GROUP_TYPE,GROUP_ID,GROUP_NAME,LEVEL,CLASSIFY,ORDER_SEQ ");
    //        sb.AppendLine("                                    ,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID) ");
    //        sb.AppendLine(" values (@KIND_CD,@GROUP_TYPE,@GROUP_ID,@GROUP_NAME,@LEVEL,@CLASSIFY,@ORDER_SEQ,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");
    //        ht.Add("@KIND_CD", KIND_CD);
    //        ht.Add("@GROUP_TYPE", GROUP_TYPE);
    //        ht.Add("@GROUP_ID", GROUP_ID);
    //        ht.Add("@GROUP_NAME", GROUP_NAME);
    //        ht.Add("@LEVEL", LEVEL);
    //        ht.Add("@CLASSIFY", CLASSIFY);
    //        ht.Add("@ORDER_SEQ", ORDER_SEQ);
    //        ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
    //        ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
    //        ht.Add("@FUNC_ID", "FB2SC120");

    //        dbConn.ExecuteT(sb, ht, true);
    //    }
    //    catch (Exception)
    //    {
    //        throw;
    //    }
    //}
    //public void updateData()
    //{
    //    try
    //    {
    //        StringBuilder sb = new StringBuilder();
    //        Hashtable ht = new Hashtable();

    //        sb.AppendLine("Update TB_S_M_SALARY_GROUP_H ");
    //        sb.AppendLine(" Set GROUP_NAME = @GROUP_NAME,LEVEL = @LEVEL,CLASSIFY = @CLASSIFY,ORDER_SEQ = @ORDER_SEQ ");
    //        sb.AppendLine("     ,UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID ");
    //        sb.AppendLine(" where KIND_CD+GROUP_TYPE+GROUP_ID = @KIND_CD+@GROUP_TYPE+@GROUP_ID ");
    //        ht.Add("@KIND_CD", KIND_CD);
    //        ht.Add("@GROUP_TYPE", GROUP_TYPE);
    //        ht.Add("@GROUP_ID", GROUP_ID);
    //        ht.Add("@GROUP_NAME", GROUP_NAME);
    //        ht.Add("@LEVEL", LEVEL);
    //        ht.Add("@CLASSIFY", CLASSIFY);
    //        ht.Add("@ORDER_SEQ", ORDER_SEQ);
    //        ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
    //        ht.Add("@FUNC_ID", "FB2SC120");

    //        dbConn.ExecuteT(sb, ht, true);
    //    }
    //    catch (Exception)
    //    {
    //        throw;
    //    }
    //}
    #endregion

    #region "Add"
    public DataTable getSalary_Cal_H(string salary_ym, string salary_type)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" select DUTY_EDT from TB_S_M_SALARY_CAL_H ");
            sb.AppendLine(" where SALARY_YM = @SALARY_YM and SALARY_TYPE = @SALARY_TYPE ");
            sb.AppendLine(" order by DUTY_EDT ASC ");
            ht.Add("@SALARY_TYPE", salary_type);
            ht.Add("@SALARY_YM", salary_ym.Replace("/", ""));

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
    public DataTable getAddDataA(int startRowIndex, int maximumRows, string sortExpression, string salary_type)
    {
        try
        {
            if (sortExpression.Contains("SALARY_CD"))
                sortExpression = sortExpression.Replace("SALARY_CD", "t1.SALARY_CD");
            if (sortExpression.Contains("SALARY_ID"))
                sortExpression = sortExpression.Replace("SALARY_ID", "t1.SALARY_ID");
            if (sortExpression == "")
            {
                sortExpression = "t1.SALARY_CD,t1.SALARY_ID";
            }
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" select * from");
            sb.AppendLine("      (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,                                  ");
            sb.AppendLine("             t1.SALARY_ID as SALARY_ID,t1.SALARY_NAME,t1.SALARY_CD,t1.SALARY_CD +'-'+ d.SUB_DESC as DESC1        ");
            sb.AppendLine("         from TB_S_M_SALARY_ITEM t1                                                                              ");
            sb.AppendLine("         left join TB_9_M_COMM_D d on  d.SYS_CD ='SC' and  d.MAIN_CD='SALARY_CD' and  t1.SALARY_CD = d.SUB_CD    ");
            sb.AppendLine("        where 1=1 and t1.SALARY_CD in('1','2','3') and t1.IS_DISABLE='N'                                        ");
            sb.AppendLine("     )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.AppendLine(" AND CAST(@startRowIndex+@maximumRows as varchar)");

            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);

            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public int getAddCountA(int startRowIndex, int maximumRows, string salary_type)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" select COUNT(*) total_record ");
            sb.AppendLine("   from TB_S_M_SALARY_ITEM t1                                                                              ");
            sb.AppendLine("   left join TB_9_M_COMM_D d on  d.SYS_CD ='SC' and  d.MAIN_CD='SALARY_CD' and  t1.SALARY_CD = d.SUB_CD    ");
            sb.AppendLine("  where 1=1 and t1.SALARY_CD in('1','2','3') and t1.IS_DISABLE='N'                                        ");
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
    public DataTable getAddDataB(int startRowIndex, int maximumRows, string sortExpression, string salary_type)
    {
        try
        {
            if (sortExpression.Contains("SALARY_CD"))
                sortExpression = sortExpression.Replace("SALARY_CD", "t1.SALARY_CD");
            if (sortExpression.Contains("SUB_GROUP_ID"))
                sortExpression = sortExpression.Replace("SUB_GROUP_ID", "g1.SUB_GROUP_ID");
            if (sortExpression == "")
            {
                sortExpression = "t1.SALARY_CD,g1.SUB_GROUP_ID";
            }
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" select * from");
            sb.AppendLine("      (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,                                  ");
            sb.AppendLine("          g1.SUB_GROUP_ID as SALARY_ID,t1.SALARY_NAME,t1.SALARY_CD,t1.SALARY_CD +'-'+ d.SUB_DESC as DESC1        ");
            sb.AppendLine("           from TB_S_M_SALARY_GROUP_D g1                                                                         ");
            sb.AppendLine("           left join TB_S_M_SALARY_ITEM t1 on  g1.SUB_GROUP_ID = t1.SALARY_ID and t1.IS_DISABLE='N'              ");
            sb.AppendLine("           left join TB_9_M_COMM_D d on  d.SYS_CD ='SC' and  d.MAIN_CD='SALARY_CD' and  t1.SALARY_CD = d.SUB_CD  ");
            sb.AppendLine("          where 1=1 and  g1.KIND_CD ='A' and g1.GROUP_TYPE ='B'                                                  ");
            sb.AppendLine("     )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.AppendLine(" AND CAST(@startRowIndex+@maximumRows as varchar)");

            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);

            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public int getAddCountB(int startRowIndex, int maximumRows, string salary_type)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" select COUNT(*) total_record ");
            sb.AppendLine("   from TB_S_M_SALARY_GROUP_D g1                                                                         ");
            sb.AppendLine("   left join TB_S_M_SALARY_ITEM t1 on  g1.SUB_GROUP_ID = t1.SALARY_ID and t1.IS_DISABLE='N'              ");
            sb.AppendLine("   left join TB_9_M_COMM_D d on  d.SYS_CD ='SC' and  d.MAIN_CD='SALARY_CD' and  t1.SALARY_CD = d.SUB_CD  ");
            sb.AppendLine("  where 1=1 and  g1.KIND_CD ='A' and g1.GROUP_TYPE ='B'                                                  ");

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
    public DataTable getAddDataC(int startRowIndex, int maximumRows, string sortExpression, string salary_type)
    {
        try
        {
            if (sortExpression.Contains("SALARY_CD"))
                sortExpression = sortExpression.Replace("SALARY_CD", "t1.SALARY_CD");
            if (sortExpression.Contains("SUB_GROUP_ID"))
                sortExpression = sortExpression.Replace("SUB_GROUP_ID", "g1.SUB_GROUP_ID");
            if (sortExpression == "")
            {
                sortExpression = "t1.SALARY_CD,g1.SUB_GROUP_ID";
            }
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" select * from");
            sb.AppendLine("      (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,                                  ");
            sb.AppendLine("               g1.SUB_GROUP_ID as SALARY_ID,t1.SALARY_NAME,t1.SALARY_CD,t1.SALARY_CD +'-'+ d.SUB_DESC as DESC1      ");
            sb.AppendLine("         from TB_S_M_SALARY_GROUP_D g1                                                                           ");
            sb.AppendLine("         left join TB_S_M_SALARY_ITEM t1 on  g1.SUB_GROUP_ID = t1.SALARY_ID and t1.IS_DISABLE='N'                   ");
            sb.AppendLine("         left join TB_9_M_COMM_D d on  d.SYS_CD ='SC' and  d.MAIN_CD='SALARY_CD' and  t1.SALARY_CD = d.SUB_CD    ");
            sb.AppendLine("        where 1=1 and  g1.KIND_CD ='A' and g1.GROUP_TYPE ='C'                                                  ");
            sb.AppendLine("     )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.AppendLine(" AND CAST(@startRowIndex+@maximumRows as varchar)");

            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);

            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public int getAddCountC(int startRowIndex, int maximumRows, string salary_type)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" select COUNT(*) total_record ");
            sb.AppendLine("   from TB_S_M_SALARY_GROUP_D g1                                                                           ");
            sb.AppendLine("   left join TB_S_M_SALARY_ITEM t1 on  g1.SUB_GROUP_ID = t1.SALARY_ID and t1.IS_DISABLE='N'                   ");
            sb.AppendLine("   left join TB_9_M_COMM_D d on  d.SYS_CD ='SC' and  d.MAIN_CD='SALARY_CD' and  t1.SALARY_CD = d.SUB_CD    ");
            sb.AppendLine("  where 1=1 and  g1.KIND_CD ='A' and g1.GROUP_TYPE ='C'                                                  ");

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
    public DataTable getAddDataD(int startRowIndex, int maximumRows, string sortExpression, string salary_type)
    {
        try
        {
            if (sortExpression.Contains("SALARY_CD"))
                sortExpression = sortExpression.Replace("SALARY_CD", "t1.SALARY_CD");
            if (sortExpression.Contains("SUB_GROUP_ID"))
                sortExpression = sortExpression.Replace("SUB_GROUP_ID", "g1.SUB_GROUP_ID");
            if (sortExpression == "")
            {
                sortExpression = "t1.SALARY_CD,g1.SUB_GROUP_ID";
            }
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" select * from");
            sb.AppendLine("      (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,                                  ");
            sb.AppendLine("               g1.SUB_GROUP_ID as SALARY_ID,t1.SALARY_NAME,t1.SALARY_CD,t1.SALARY_CD +'-'+ d.SUB_DESC as DESC1      ");
            sb.AppendLine("         from TB_S_M_SALARY_GROUP_D g1                                                                           ");
            sb.AppendLine("         left join TB_S_M_SALARY_ITEM t1 on  g1.SUB_GROUP_ID = t1.SALARY_ID and t1.IS_DISABLE='N'                   ");
            sb.AppendLine("         left join TB_9_M_COMM_D d on  d.SYS_CD ='SC' and  d.MAIN_CD='SALARY_CD' and  t1.SALARY_CD = d.SUB_CD    ");
            sb.AppendLine("        where 1=1 and  g1.KIND_CD ='A' and g1.GROUP_TYPE ='C'                                                  ");
            sb.AppendLine("     )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.AppendLine(" AND CAST(@startRowIndex+@maximumRows as varchar)");

            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);

            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public int getAddCountD(int startRowIndex, int maximumRows, string salary_type)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" select COUNT(*) total_record ");
            sb.AppendLine("   from TB_S_M_SALARY_GROUP_D g1                                                                           ");
            sb.AppendLine("   left join TB_S_M_SALARY_ITEM t1 on  g1.SUB_GROUP_ID = t1.SALARY_ID and t1.IS_DISABLE='N'                   ");
            sb.AppendLine("   left join TB_9_M_COMM_D d on  d.SYS_CD ='SC' and  d.MAIN_CD='SALARY_CD' and  t1.SALARY_CD = d.SUB_CD    ");
            sb.AppendLine("  where 1=1 and  g1.KIND_CD ='A' and g1.GROUP_TYPE ='C'                                                  ");
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
    public string getFUN_SALARY_YM()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select dbo.FN_S_SALARY_YM() as FUN_SALARY_YM ");
            DataTable dt = dbConn.Query(sb, ht, true);
            return Convert.ToString(dt.Rows[0]["FUN_SALARY_YM"]);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void saveAddData(List<string> salary_idList)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" insert into TB_S_M_SALARY_CAL_H (SALARY_DT, SALARY_YM,SALARY_TYPE, SALARY_SDT, SALARY_EDT, DUTY_SDT, DUTY_EDT, PROCESS_STATUS ");
            sb.AppendLine("        ,PAY_KIND,CREATED_BY, CREATED_DT, UPDATED_BY, UPDATED_DT, FUNC_ID,IACYC) ");
            sb.AppendLine(" values (@SALARY_DT, @SALARY_YM, @SALARY_TYPE, @SALARY_SDT, @SALARY_EDT, @DUTY_SDT, @DUTY_EDT, @PROCESS_STATUS ");
            sb.AppendLine("         ,@PAY_KIND,@CREATED_BY, GETDATE(), @UPDATED_BY, GETDATE(), @FUNC_ID,@IACYC) ");
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_YM", SALARY_YM);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            if (SALARY_SDT == "")
                ht.Add("@SALARY_SDT", DBNull.Value);
            else
                ht.Add("@SALARY_SDT", SALARY_SDT);
            if (SALARY_EDT == "")
                ht.Add("@SALARY_EDT", DBNull.Value);
            else
                ht.Add("@SALARY_EDT", SALARY_EDT);
            if (DUTY_SDT == "")
                ht.Add("@DUTY_SDT", DBNull.Value);
            else
                ht.Add("@DUTY_SDT", DUTY_SDT);
            if (DUTY_EDT == "")
                ht.Add("@DUTY_EDT", DBNull.Value);
            else
                ht.Add("@DUTY_EDT", DUTY_EDT);
            ht.Add("@PROCESS_STATUS", "1");
            if (SALARY_TYPE == "A")
                ht.Add("@PAY_KIND", "9999");
            else
                ht.Add("@PAY_KIND", salary_idList[0]);
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SC210");
            ht.Add("@IACYC", IACYC);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void saveAddDtl(string salary_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" insert into TB_S_M_SALARY_CAL_D (SALARY_DT, SALARY_TYPE, SALARY_ID, PAY_KIND ");
            sb.AppendLine("        ,CREATED_BY, CREATED_DT, UPDATED_BY, UPDATED_DT, FUNC_ID) ");
            sb.AppendLine(" values (@SALARY_DT, @SALARY_TYPE, @SALARY_ID, @PAY_KIND  ");
            sb.AppendLine("         ,@CREATED_BY, GETDATE(), @UPDATED_BY, GETDATE(), @FUNC_ID) ");
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            ht.Add("@SALARY_ID", salary_id);
            if (SALARY_TYPE == "A")
                ht.Add("@PAY_KIND", "9999");
            else
                ht.Add("@PAY_KIND", salary_id);
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SC210");

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable getExistData(List<string> salary_idList)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select SALARY_DT from TB_S_M_SALARY_CAL_H ");
            sb.AppendLine("  where CONVERT(varchar(100), SALARY_DT , 111) = @SALARY_DT ");
            sb.AppendLine("    and SALARY_TYPE = @SALARY_TYPE ");
            sb.AppendLine("    and PAY_KIND = @PAY_KIND ");
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            if (SALARY_TYPE == "A")
                ht.Add("@PAY_KIND", "9999");
            else
                ht.Add("@PAY_KIND", salary_idList[0]);
            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion

    #region "Detail"
    public DataTable getDtlHeader(string salary_dt, string salary_type, string pay_kind)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine("   select t.SALARY_DT,t.SALARY_YM,t.SALARY_TYPE,t.SALARY_SDT ,t.SALARY_EDT ,t.DUTY_SDT,t.DUTY_EDT,t.PROCESS_STATUS ");
            sb.AppendLine("         ,t.PROCESS_STATUS +'-'+ d.SUB_DESC as PROCESS_STATUS_DESC,t.SALARY_TYPE +'-'+  d.SUB_DESC as SALARY_TYPE_DESC  ");
            sb.AppendLine("         ,t.PAY_KIND,t.PAY_KIND +'-'+ p.SALARY_NAME as PAY_KIND_DESC,t.IACYC                                               ");
            sb.AppendLine("     from TB_S_M_SALARY_CAL_H t                                                                                    ");
            sb.AppendLine("    left join TB_9_M_COMM_D d on  d.SYS_CD ='SC' and  d.MAIN_CD='SALARY_TYPE' and  t.SALARY_TYPE = d.SUB_CD ");
            sb.AppendLine("        left join VW_SALARYAND9999 p on  t.PAY_KIND = p.SALARY_ID                                              ");
            sb.AppendLine("    where 1=1 and CONVERT(varchar(100), t.SALARY_DT , 111) = @SALARY_DT and t.SALARY_TYPE = @SALARY_TYPE  and t.PAY_KIND=@PAY_KIND  ");
            ht.Add("@SALARY_DT", salary_dt);
            ht.Add("@SALARY_TYPE", salary_type);
            ht.Add("@PAY_KIND", pay_kind);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
    //查詢明細
    public DataTable getDtlData(int startRowIndex, int maximumRows, string sortExpression, string salary_dt, string salary_type, string pay_kind)
    {
        try
        {
            if (sortExpression.Contains("SALARY_ID"))
                sortExpression = sortExpression.Replace("SALARY_ID", "t.SALARY_ID");
            if (sortExpression.Contains("SALARY_CD"))
                sortExpression = sortExpression.Replace("SALARY_CD", "t1.SALARY_CD");
            if (sortExpression == "")
            {
                sortExpression = "t.SALARY_ID";
            }
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select * From");
            sb.AppendLine(" (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,                               ");
            sb.AppendLine("         t.SALARY_ID,t1.SALARY_NAME,t1.SALARY_CD,t1.SALARY_CD +'-'+ d.SUB_DESC as DESC1                  ");
            sb.AppendLine("    from TB_S_M_SALARY_CAL_D t                                                                           ");
            sb.AppendLine("    left join TB_S_M_SALARY_ITEM t1 on  t.SALARY_ID = t1.SALARY_ID and t1.IS_DISABLE='N'                 ");
            sb.AppendLine("    left join TB_9_M_COMM_D d on  d.SYS_CD ='SC' and  d.MAIN_CD='SALARY_CD' and  t1.SALARY_CD = d.SUB_CD ");
            sb.AppendLine("    where 1=1 and CONVERT(varchar(100), t.SALARY_DT , 111) = @SALARY_DT and t.SALARY_TYPE = @SALARY_TYPE and t.PAY_KIND=@PAY_KIND  ");
            sb.AppendLine(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.AppendLine(" AND CAST(@startRowIndex+@maximumRows as varchar)");
            ht.Add("@SALARY_DT", salary_dt);
            ht.Add("@SALARY_TYPE", salary_type);
            ht.Add("@PAY_KIND", pay_kind);
            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            return dbConn.Query(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }
    public int getDtlCount(int startRowIndex, int maximumRows, string salary_dt, string salary_type, string pay_kind)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select COUNT(*) total_record ");
            sb.AppendLine("    from TB_S_M_SALARY_CAL_D t                                                                           ");
            sb.AppendLine("    left join TB_S_M_SALARY_ITEM t1 on  t.SALARY_ID = t1.SALARY_ID and t1.IS_DISABLE='N'                 ");
            sb.AppendLine("    left join TB_9_M_COMM_D d on  d.SYS_CD ='SC' and  d.MAIN_CD='SALARY_CD' and  t1.SALARY_CD = d.SUB_CD ");
            sb.AppendLine("    where 1=1 and CONVERT(varchar(100), t.SALARY_DT , 111) = @SALARY_DT and t.SALARY_TYPE = @SALARY_TYPE  and t.PAY_KIND=@PAY_KIND          ");
            ht.Add("@SALARY_DT", salary_dt);
            ht.Add("@SALARY_TYPE", salary_type);
            ht.Add("@PAY_KIND", pay_kind);
            DataTable dt = dbConn.Query(sb, ht, true);
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
    public DataTable chkMonthClose(string salary_dt, string salary_type, string pay_kind)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(@" select count(*) as CNT from TB_S_M_SALARY_CAL_H 
                                        where PROCESS_STATUS <> '4'
                                        and (SALARY_DT <> @SALARY_DT  or SALARY_TYPE <> @SALARY_TYPE  or PAY_KIND <> @PAY_KIND )");

            ht.Add("@SALARY_DT", salary_dt);
            ht.Add("@SALARY_TYPE", salary_type);
            ht.Add("@PAY_KIND", pay_kind);

            DataTable dt1 = dbConn.Query(sb, ht);
            
            return dt1;
        }
        catch
        {
            throw;
        }
    }
    public DataTable getOPERATION_NAME(string salary_type, string salary_dt, string salary_ym)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select t1.SALARY_TYPE,t1.OPERATION_ID,t1.OPERATION_NAME,t1.SALARY_REQ,t1.PROC_SOUCE ,t2.PROCESS_DT,t2.SALARY_LOCKED  ");
            sb.AppendLine("   from TB_S_M_SALARY_CTRL t1                                                                                         ");
            sb.AppendLine("   left join TB_S_M_SALARY_MONTH_CTRL t2 on t1.SALARY_TYPE = t2.SALARY_TYPE and t1.OPERATION_ID = t2.OPERATION_ID     ");
            sb.AppendLine("    and t2.SALARY_YM = @SALARY_YM                                                                                     ");
            sb.AppendLine("  where t1.SALARY_TYPE = @SALARY_TYPE and t1.SALARY_REQ ='Y' and ISNULL(t2.SALARY_LOCKED,'') <>'Y' and  t1.OPERATION_ID <> 'B01' and t1.PROC_SOUCE ='1' ");
            sb.AppendLine(" UNION                                                                                                                ");
            sb.AppendLine(" select t1.SALARY_TYPE,t1.OPERATION_ID,t1.OPERATION_NAME,t1.SALARY_REQ,t1.PROC_SOUCE ,t2.PROCESS_DT,t2.SALARY_LOCKED  ");
            sb.AppendLine("   from TB_S_M_SALARY_CTRL t1                                                                                         ");
            sb.AppendLine("   left join TB_S_M_SALARY_MONTH_CTRL t2 on t1.SALARY_TYPE = t2.SALARY_TYPE and t1.OPERATION_ID = t2.OPERATION_ID     ");
            sb.AppendLine("    and CONVERT(varchar(100), t2.SALARY_DT , 111) = @SALARY_DT                                                        ");
            sb.AppendLine("  where t1.SALARY_TYPE = @SALARY_TYPE and t1.SALARY_REQ ='Y' and ISNULL(t2.SALARY_LOCKED,'') <>'Y'  and t1.OPERATION_ID = 'B01'  ");
            sb.AppendLine("  order By t1.OPERATION_ID                                                                                            ");
            ht.Add("@SALARY_TYPE", salary_type);
            ht.Add("@SALARY_DT", salary_dt);
            ht.Add("@SALARY_YM", salary_ym);
            return dbConn.Query(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }
    public void RunSP_S_SALARY_CAL_EXEC(string salary_type, string pay_kind, string salary_dt, string salary_ym, string salary_sdt, string salary_edt, string duty_sdt, string duty_edt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_S_SALARY_CAL_EXEC");
            if (string.IsNullOrEmpty(salary_type))
                ht.Add("@pSalaryType", DBNull.Value);
            else
                ht.Add("@pSalaryType", salary_type);

            if (string.IsNullOrEmpty(pay_kind))
                ht.Add("@pPayKind", DBNull.Value);
            else
                ht.Add("@pPaykind", pay_kind);

            if (string.IsNullOrEmpty(salary_dt))
                ht.Add("@pSalaryDate", DBNull.Value);
            else
                ht.Add("@pSalaryDate", salary_dt);

            if (string.IsNullOrEmpty(salary_ym))
                ht.Add("@pSalaryYM", DBNull.Value);
            else
                ht.Add("@pSalaryYM", salary_ym);

            if (string.IsNullOrEmpty(salary_sdt))
                ht.Add("@pSalaryDT_STR", DBNull.Value);
            else
                ht.Add("@pSalaryDT_STR", salary_sdt);

            if (string.IsNullOrEmpty(salary_edt))
                ht.Add("@pSalaryDT_END", DBNull.Value);
            else
                ht.Add("@pSalaryDT_END", salary_edt);

            if (string.IsNullOrEmpty(duty_sdt))
                ht.Add("@pWorkDT_STR", DBNull.Value);
            else
                ht.Add("@pWorkDT_STR", duty_sdt);

            if (string.IsNullOrEmpty(duty_edt))
                ht.Add("@pWorkDT_END", DBNull.Value);
            else
                ht.Add("@pWorkDT_END", duty_edt);

            ht.Add("@pUserID", SessionHandle.Current.emp_id);
            ht.Add("@pFuncID", "FB2SC210");

            dbConn.ExecuteSP(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void RunSP_S_SALARY_ABNORMAL_EXEC(string salary_type, string pay_kind, string salary_dt, string salary_ym, string salary_sdt, string salary_edt, string duty_sdt, string duty_edt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_S_SALARY_ABNORMAL_EXEC");
            if (string.IsNullOrEmpty(salary_type))
                ht.Add("@pSalaryType", DBNull.Value);
            else
                ht.Add("@pSalaryType", salary_type);

            if (string.IsNullOrEmpty(pay_kind))
                ht.Add("@pPaykind", DBNull.Value);
            else
                ht.Add("@pPaykind", pay_kind);

            if (string.IsNullOrEmpty(salary_dt))
                ht.Add("@pSalaryDate", DBNull.Value);
            else
                ht.Add("@pSalaryDate", salary_dt);

            if (string.IsNullOrEmpty(salary_ym))
                ht.Add("@pSalaryYM", DBNull.Value);
            else
                ht.Add("@pSalaryYM", salary_ym);

            if (string.IsNullOrEmpty(salary_sdt))
                ht.Add("@pSalaryDT_STR", DBNull.Value);
            else
                ht.Add("@pSalaryDT_STR", salary_sdt);

            if (string.IsNullOrEmpty(salary_edt))
                ht.Add("@pSalaryDT_END", DBNull.Value);
            else
                ht.Add("@pSalaryDT_END", salary_edt);

            if (string.IsNullOrEmpty(duty_sdt))
                ht.Add("@pWorkDT_STR", DBNull.Value);
            else
                ht.Add("@pWorkDT_STR", duty_sdt);

            if (string.IsNullOrEmpty(duty_edt))
                ht.Add("@pWorkDT_END", DBNull.Value);
            else
                ht.Add("@pWorkDT_END", duty_edt);
            ht.Add("@pUserID", SessionHandle.Current.emp_id);
            ht.Add("@pFuncID", "FB2SC210");

            dbConn.ExecuteSP(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable checkSP(string proc_id)
    {

        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select top (1) PROC_STATUS, PROC_LOG  ");
            sb.AppendLine("   from TB_H_R_SP_LOG                  ");
            sb.AppendLine("  where  PROC_ID = @PROC_ID            ");
            sb.AppendLine("  order by PROC_DT desc                ");
            ht.Add("@PROC_ID", proc_id);
            return dbConn.QueryT(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }
    public DataTable checkSP2(string proc_id)
    {

        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select top (1) PROC_STATUS, PROC_LOG  ");
            sb.AppendLine("   from TB_H_R_SP_LOG                  ");
            sb.AppendLine("  where  PROC_ID = @PROC_ID            ");
            sb.AppendLine("  order by PROC_DT desc                ");
            ht.Add("@PROC_ID", proc_id);
            return dbConn.Query(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getExcelData(string salary_type, string salary_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select t1.EMP_ID, e.EMP_NAME, t1.SALARY_ID, t2.SALARY_NAME,t1.SEQ_NO,t1.MSG_TYPE ,t1.OP_MSG             ");
            sb.AppendLine("   from TB_S_M_SALARY_ERROR_RPT t1                                                                       ");
            sb.AppendLine("   left join TB_S_M_SALARY_ITEM t2 on t1.SALARY_ID = t2.SALARY_ID                                        ");
            sb.AppendLine("   left join VW_H_EMP_DATA e on t1.EMP_ID = e.EMP_ID                                                     ");
            sb.AppendLine("    where 1=1 and CONVERT(varchar(100), t1.SALARY_DT , 111) = @SALARY_DT and t1.SALARY_TYPE = @SALARY_TYPE ");
            sb.AppendLine("   order by t1.MSG_TYPE,t1.OP_MSG");
            ht.Add("@SALARY_TYPE", salary_type);
            ht.Add("@SALARY_DT", salary_dt);
            return dbConn.Query(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }

    public DataTable getTB_S_M_SALARY_CTRL(string salary_ym)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select t1.SALARY_TYPE,t1.OPERATION_ID,t1.OPERATION_NAME,t1.SALARY_REQ,t1.PROC_SOUCE,t2.PROCESS_DT,t2.SALARY_LOCKED ");
            sb.AppendLine(" from TB_S_M_SALARY_CTRL t1 ");
            sb.AppendLine(" left join TB_S_M_SALARY_MONTH_CTRL t2 on t1.SALARY_TYPE = t2.SALARY_TYPE and t1.OPERATION_ID = t2.OPERATION_ID and t2.SALARY_YM = @SALARY_YM ");
            sb.AppendLine(" where t1.SALARY_TYPE = 'A'  and t1.SALARY_REQ ='Y' and t1.PROC_SOUCE ='2' and t2.PROCESS_DT is null ");
            ht.Add("@SALARY_YM", salary_ym);
            return dbConn.Query(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }
    public int RunSP_S_EMP_DATA_MONTH_EXEC()
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.Append("SP_S_EMP_DATA_MONTH_EXEC");

        ht.Add("@pSalaryType", SALARY_TYPE);
        ht.Add("@pSalaryDate", SALARY_DT);
        ht.Add("@pSalaryYM", SALARY_YM);
        ht.Add("@pUserID", SessionHandle.Current.emp_id);
        ht.Add("@pFuncID", "FB2SC210");

        return dbConn.ExecuteSP(sb, ht, true);
    }
    public int InsertTB_S_M_SALARY_MONTH_CTRL_Dateial2(string operation_id)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.AppendLine(" insert into TB_S_M_SALARY_MONTH_CTRL (SALARY_TYPE,SALARY_YM,SALARY_DT,START_DT,END_DT,PROCESS_DT,OPERATION_ID,SALARY_LOCKED,LOCK_DT,FUNC_ID)  ");
        sb.AppendLine("   values( @SALARY_TYPE,@SALARY_YM,@SALARY_DT,@START_DT,@END_DT,GETDATE(),@OPERATION_ID,@SALARY_LOCKED,GETDATE(),@FUNC_ID) ");

        ht.Add("@SALARY_TYPE", SALARY_TYPE);
        ht.Add("@SALARY_YM", SALARY_YM);
        ht.Add("@SALARY_DT", SALARY_DT);
        ht.Add("@START_DT", SALARY_SDT);
        ht.Add("@END_DT", SALARY_EDT);
        ht.Add("@OPERATION_ID", operation_id);
        ht.Add("@SALARY_LOCKED", "Y");
        ht.Add("@FUNC_ID", "FB2SC210");
        return dbConn.ExecuteT(sb, ht);
    }
    public void update_DB1CMBC0_1()
    {
        DBConnector_ODBC odbc = new DBConnector_ODBC(utilities.ODBCconnstr);
        try
        {
            ////AS400
            OdbcCommand ocomm = new OdbcCommand();
            ocomm.CommandText += "update B1CLIB.DB1CMBC0 set CSALST = 'N' , CSALPD = 0 , CSALDT = 0";
            ocomm.CommandText += "  where CSALDT = ?";

            ocomm.Parameters.AddWithValue("", Convert.ToInt32(SALARY_DT.Replace("/", "")));

            odbc.getDataTable(ocomm);
        }
        catch (Exception)
        {

            throw;
        }
        finally {
            odbc.connectionClose();
        }
    }
    public void update_DB1CMBC0_2()
    {
        DBConnector_ODBC odbc = new DBConnector_ODBC(utilities.ODBCconnstr);
        try
        {
            ////AS400
            OdbcCommand ocomm = new OdbcCommand();
            ocomm.CommandText += "update B1CLIB.DB1CMBC0 set CSALST = 'Y' , CSALPD = ? , CSALDT = ?";
            ocomm.CommandText += "  where CSALDT = 0";

            ocomm.Parameters.AddWithValue("", Convert.ToInt32(DateTime.Now.ToString("yyyyMMdd")));
            ocomm.Parameters.AddWithValue("", Convert.ToInt32(SALARY_DT.Replace("/", "")));   

            odbc.getDataTable(ocomm);
        }
        catch (Exception)
        {

            throw;
        }
        finally
        {
            odbc.connectionClose();
        }
    }
    public int UpdateTB_S_M_SUBSIDY_DEDUCTIONS_1_Dateial2(bool Lock)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();

        sb.AppendLine(" update TB_S_M_SUBSIDY_DEDUCTIONS_1 ");
        if (!Lock)
        {
            sb.AppendLine("   set SALARY_STATUS='N' ");
            sb.AppendLine("      ,SALARY_PROC_DT=null");
            sb.AppendLine("      ,SALARY_DT=null ");

        }
        else
        {
            sb.AppendLine("   set SALARY_STATUS='Y' ");
            sb.AppendLine("      ,SALARY_PROC_DT=getdate()");
            sb.AppendLine("      ,SALARY_DT=@SALARY_DT ");
        }
        sb.AppendLine("      ,UPDATED_BY=@UPDATED_BY ");
        sb.AppendLine("      ,UPDATED_DT=getdate() ");
        sb.AppendLine("      ,FUNC_ID=@FUNC_ID ");
        sb.AppendLine(" where 1=1 ");  //DATA_YM=@DATA_YM
        if (!Lock)
            sb.AppendLine("   and SALARY_STATUS='Y'  and SALARY_DT = @SALARY_DT");
        else
            sb.AppendLine("   and SALARY_STATUS<>'Y' ");

        ht.Add("@SALARY_DT", SALARY_SDT);
        ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
        ht.Add("@FUNC_ID", "FB2SC210");
        ht.Add("@DATA_YM", SALARY_YM);

        return dbConn.ExecuteT(sb, ht);
    }
    public int UpdateTB_S_M_SUBSIDY_DEDUCTIONS_D_Dateial2(bool Lock)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();

        sb.AppendLine(" update TB_S_M_SUBSIDY_DEDUCTIONS_D ");
        if (!Lock)
        {
            sb.AppendLine("   set SALARY_STATUS='N' ");
            sb.AppendLine("      ,SALARY_PROC_DT=null");
            sb.AppendLine("      ,SALARY_DT=null ");

        }
        else
        {
            sb.AppendLine("   set SALARY_STATUS='Y' ");
            sb.AppendLine("      ,SALARY_PROC_DT=getdate()");
            sb.AppendLine("      ,SALARY_DT=@SALARY_DT ");
        }
        sb.AppendLine("      ,UPDATED_BY=@UPDATED_BY ");
        sb.AppendLine("      ,UPDATED_DT=getdate() ");
        sb.AppendLine("      ,FUNC_ID=@FUNC_ID ");
        sb.AppendLine(" where DATA_YM=@DATA_YM ");
        if (!Lock)
            sb.AppendLine("   and SALARY_STATUS='Y'  and SALARY_DT = @SALARY_DT");
        else
            sb.AppendLine("   and SALARY_STATUS<>'Y' ");

        ht.Add("@SALARY_DT", SALARY_SDT);
        ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
        ht.Add("@FUNC_ID", "FB2SC210");
        ht.Add("@DATA_YM", SALARY_YM);

        return dbConn.ExecuteT(sb, ht);
    }
    //I01 (預付薪)
    public int CheckTB_S_M_SALARY_CAL_H()
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.AppendLine(" select count('1') CheckData  ");
        sb.AppendLine(" from TB_S_M_SALARY_CAL_H  ");
        sb.AppendLine(" where SALARY_TYPE=@SALARY_TYPE ");
        sb.AppendLine("   and SALARY_YM=@SALARY_YM ");

        ht.Add("@SALARY_TYPE", SALARY_TYPE);
        ht.Add("@SALARY_YM", SALARY_YM);

        return Convert.ToInt16(dbConn.QueryT(sb, ht).Rows[0]["CheckData"]);

    }
    //J01(其他類獎金月結)
    public int UpdateTB_S_OTHER_BOUNS_D_Dateial2(bool Lock)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.AppendLine(" update TB_S_M_OTHER_BOUNS_D ");
        if (!Lock)
        {
            sb.AppendLine("   set SALARY_STATUS='N' ");
            sb.AppendLine("      ,SALARY_PROC_DT=null");
            sb.AppendLine("      ,SALARY_DT=null ");
        }
        else
        {
            sb.AppendLine("   set SALARY_STATUS='Y' ");
            sb.AppendLine("      ,SALARY_PROC_DT=getdate() ");
            sb.AppendLine("      ,SALARY_DT=@SALARY_DT ");
        }
        sb.AppendLine("      ,UPDATED_BY=@UPDATED_BY ");
        sb.AppendLine("      ,UPDATED_DT=getdate() ");
        sb.AppendLine("      ,FUNC_ID=@FUNC_ID ");
        if (!Lock)
            sb.AppendLine(" where SALARY_STATUS='Y' and SALARY_DT = @SALARY_DT");
        else
            sb.AppendLine(" where SALARY_STATUS<>'Y' ");

        ht.Add("@SALARY_DT", SALARY_DT);
        ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
        ht.Add("@FUNC_ID", "FB2SC210");

        return dbConn.ExecuteT(sb, ht);

    }
    #endregion
}