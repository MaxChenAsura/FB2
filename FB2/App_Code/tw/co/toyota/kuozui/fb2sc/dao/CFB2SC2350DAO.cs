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
/// CFB2SC2350DAO 的摘要描述
/// </summary>
public class CFB2SC2350DAO : BaseDAO
{
    public CFB2SC2350DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public string RowNumber { get; set; }
    public string SALARY_DT { get; set; }
    public string SALARY_TYPE { get; set; }
    public string PAY_KIND { get; set; }
    //public string EMP_ID { get; set; }

    public int temp_row { get; set; }

    public string EMP_ID_AREA { get; set; }
    public string PAY_METHOD { get; set; }

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

    public DataTable getCommCode(string sys_cd, string main_cd, string is_valid, string sub_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select sub_cd ,sub_desc as sub_desc ");
            sb.Append(" from TB_9_M_COMM_D ");
            sb.Append(" where SYS_CD = @SYS_CD ");
            sb.Append(" and MAIN_CD = @MAIN_CD ");
            sb.Append(" and SUB_CD = @SUB_CD ");
            ht.Add("@SYS_CD", sys_cd);
            ht.Add("@MAIN_CD", main_cd);
            ht.Add("@SUB_CD", sub_cd);
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

    public DataTable paykind(string PAY_KIND)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.Append(" Select SALARY_ID,SALARY_NAME From VW_SALARYAND9999");
        sb.Append(" where SALARY_ID=@PAY_KIND");
        ht.Add("@PAY_KIND", PAY_KIND);
        return dbConn.Query(sb, ht);

    }

    public DataTable getSALARY_DT_By_Fn(string salary_type)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select dbo.FN_S_SALARY_DT(@p_salary_type) as SALARY_DT ");
            ht.Add("@p_salary_type", salary_type);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    #endregion

    #region Qry

    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string salary_dt, string salary_type, string pay_kind)
    {
        try
        {
            DataTable dt = new DataTable();
            if (salary_type == "A")
                dt = getDataA(startRowIndex, maximumRows, sortExpression, salary_dt, salary_type, pay_kind);
            else
                dt = getDataExceptA(startRowIndex, maximumRows, sortExpression, salary_dt, salary_type, pay_kind);
   
            return dt;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable getDataA(int startRowIndex, int maximumRows, string sortExpression, string salary_dt, string salary_type, string pay_kind)
    {
        try
        {
            if (sortExpression.Contains("EMP_ID"))
                sortExpression = sortExpression.Replace("EMP_ID", "t1.EMP_ID");
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            temp_row = 0;

            sb.AppendLine(" select * from");
            sb.AppendLine("   (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber, ");
            sb.AppendLine("           t1.EMP_ID,t1.EMP_NAME ");
            sb.AppendLine("      from TB_S_M_EMP_RESULT t1  ");
            sb.AppendLine("   where 1=1 and  CONVERT(varchar(100), t1.SALARY_DT , 111) = @SALARY_DT ");
            sb.AppendLine("     and t1.SALARY_PAY_METHOD='C' ");
            sb.AppendLine("     )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.AppendLine(" AND CAST(@startRowIndex+@maximumRows as varchar)");

            ht.Add("@SALARY_DT", salary_dt);
            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);

            DataTable dt = dbConn.Query(sb, ht);
            temp_row = dt.Rows.Count;
            return dt;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable getDataExceptA(int startRowIndex, int maximumRows, string sortExpression, string salary_dt, string salary_type, string pay_kind)
    {
        try
        {
            if (sortExpression.Contains("EMP_ID"))
                sortExpression = sortExpression.Replace("EMP_ID", "t1.EMP_ID");
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            temp_row = 0;

            sb.AppendLine(" select * from");
            sb.AppendLine("   (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber, ");
            sb.AppendLine("           t1.EMP_ID,t1.EMP_NAME ");
            sb.AppendLine("      from TB_S_M_EMP_RESULT_TMP t1  ");
            sb.AppendLine("   where 1=1 and  CONVERT(varchar(100), t1.SALARY_DT , 111) = @SALARY_DT ");
            sb.AppendLine("     and t1.SALARY_PAY_METHOD='C' ");
            sb.AppendLine("     and t1.SALARY_TYPE = @SALARY_TYPE and t1.PAY_KIND = @PAY_KIND  ");
            sb.AppendLine("     )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.AppendLine(" AND CAST(@startRowIndex+@maximumRows as varchar)");

            ht.Add("@SALARY_DT", salary_dt);
            ht.Add("@SALARY_TYPE", salary_type);
            ht.Add("@PAY_KIND", pay_kind);
            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);

            DataTable dt = dbConn.Query(sb, ht);
            temp_row = dt.Rows.Count;
            return dt;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public int getCount(int startRowIndex, int maximumRows, string salary_dt, string salary_type, string pay_kind)
    {
        try
        {
            return temp_row;
        }
        catch (Exception)
        {
            throw;
        }

    }

    public bool checkClose(string salary_dt, string salary_type, string pay_kind)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select count(1) total ");
            sb.AppendLine("   from TB_S_M_SALARY_PAY_H ");
            sb.AppendLine("  where 1=1 ");
            sb.AppendLine(@"    and CONVERT(varchar(100), SALARY_DT , 111) = @SALARY_DT
                                and SALARY_TYPE = @SALARY_TYPE 
                                and PAY_KIND = @PAY_KIND   
                                ");
            ht.Add("@SALARY_DT", salary_dt);
            ht.Add("@SALARY_TYPE", salary_type);
            ht.Add("@PAY_KIND", pay_kind);
            DataTable dt = dbConn.Query(sb, ht, true);
            if (Convert.ToInt32(dt.Rows[0]["total"]) > 0)
                return true;
            else
                return false;
        }
        catch
        {
            throw;
        }
    }


    #endregion

    #region 查詢總筆數

    internal DataTable getTotal(string salary_dt, string salary_type, string pay_kind)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            if (salary_type == "A"){
                sb.Append(@"                        
                            SELECT CASH_TOT, TRANS_TOT FROM 
                              ( SELECT '1' as KEY1, count(*) as CASH_TOT
                                  FROM TB_S_M_EMP_RESULT t1
                                 WHERE t1.SALARY_DT = @SALARY_DT1  and SALARY_PAY_METHOD='C' ) A
                            left join
                              ( SELECT '1' as KEY1, count(*) as TRANS_TOT
                                  FROM TB_S_M_EMP_RESULT t1
                                 WHERE t1.SALARY_DT = @SALARY_DT2  and SALARY_PAY_METHOD='T' ) B
                            on A.KEY1 = B.KEY1
                          ");
                ht.Add("@SALARY_DT1", salary_dt);
                ht.Add("@SALARY_DT2", salary_dt);

            }else{
                sb.Append(@"                        
                            SELECT CASH_TOT, TRANS_TOT FROM 
                              ( SELECT '1' as KEY1, count(*) as CASH_TOT
                                  FROM TB_S_M_EMP_RESULT_TMP t1
                                 WHERE t1.SALARY_DT = @SALARY_DT1  and SALARY_PAY_METHOD='C' 
                                   AND t1.SALARY_TYPE = @SALARY_TYPE1 AND t1.PAY_KIND = @PAY_KIND1) A
                            left join
                              ( SELECT '1' as KEY1, count(*) as TRANS_TOT
                                  FROM TB_S_M_EMP_RESULT_TMP t1
                                 WHERE t1.SALARY_DT = @SALARY_DT2  and SALARY_PAY_METHOD='T' 
                                   AND t1.SALARY_TYPE = @SALARY_TYPE2 AND t1.PAY_KIND = @PAY_KIND2) B
                            on A.KEY1 = B.KEY1
                          ");
                ht.Add("@SALARY_DT1", salary_dt);
                ht.Add("@SALARY_DT2", salary_dt);
                ht.Add("@SALARY_TYPE1", salary_type);
                ht.Add("@SALARY_TYPE2", salary_type);
                ht.Add("@PAY_KIND1", pay_kind);
                ht.Add("@PAY_KIND2", pay_kind);
            }


            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }
    #endregion

    #region save
    public void updateDataA(string emp_id)
    {
        try
        {
            
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" update TB_S_M_EMP_RESULT ");
            sb.AppendLine("    set SALARY_PAY_METHOD = @SALARY_PAY_METHOD, UPDATED_BY = @UPDATED_BY, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID ");
            sb.AppendLine("  where CONVERT(varchar(100), SALARY_DT , 111) = @SALARY_DT  ");
            sb.AppendLine("    and EMP_ID = @EMP_ID");
            ht.Add("@SALARY_PAY_METHOD", this.PAY_METHOD);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SC235");

            ht.Add("@SALARY_DT", this.SALARY_DT);
            ht.Add("@EMP_ID", emp_id);

            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }

    public void updateDataExceptA(string emp_id)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" update TB_S_M_EMP_RESULT_TMP ");
            sb.AppendLine("    set SALARY_PAY_METHOD = @SALARY_PAY_METHOD, UPDATED_BY = @UPDATED_BY, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID ");
            sb.AppendLine("  where CONVERT(varchar(100), SALARY_DT , 111) = @SALARY_DT  ");
            sb.AppendLine("    and SALARY_TYPE = @SALARY_TYPE");
            sb.AppendLine("    and PAY_KIND = @PAY_KIND");
            sb.AppendLine("    and EMP_ID = @EMP_ID");
            ht.Add("@SALARY_PAY_METHOD", this.PAY_METHOD);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SC235");

            ht.Add("@SALARY_DT", this.SALARY_DT);
            ht.Add("@SALARY_TYPE", this.SALARY_TYPE);
            ht.Add("@PAY_KIND", this.PAY_KIND);
            ht.Add("@EMP_ID", emp_id);


            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }

    #endregion



}