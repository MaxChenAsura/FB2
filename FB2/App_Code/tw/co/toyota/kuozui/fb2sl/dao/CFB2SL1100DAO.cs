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
/// CFB2SL1100DAO 的摘要描述
/// </summary>
public class CFB2SL1100DAO : BaseDAO
{
    public CFB2SL1100DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    #region Qry
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
    public DataTable getCompany_cd(string company_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select COMPANY_SNAME ");
            sb.AppendLine(" from TB_H_M_COMPANY ");
            sb.AppendLine(" where COMPANY_CD = @COMPANY_CD ");
            ht.Add("@COMPANY_CD", company_cd);

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string company_cd, string data_ym, string emp_id
                            , string emp_name, string tax_format, string data_format)
    {
        try
        {
            if (sortExpression.Contains("EMP_ID"))
                sortExpression = sortExpression.Replace("EMP_ID", "I.EMP_ID");

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" select * From");
            sb.AppendLine("         (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,                    ");
            sb.AppendLine("                 I.DATA_FORMAT, C.COMPANY_SNAME, I.EMP_ID, I.LICENSE_ID, I.EMP_NAME, I.TAX_FORMAT     ");
            sb.AppendLine("                , I.AMOUNT, I.TAX, I.CONTACT_ZIP_CD, I.CONTACT_ADDR,I.TAX_FORMAT +'-'+ e.SUB_DESC as TAX_FORMAT_DESC ");
            sb.AppendLine("                , I.DATA_FORMAT+I.COMPANY_CD+I.EMP_ID+I.TAX_FORMAT+I.TAX_FORMAT_DTL as qdatakey        ");
            sb.AppendLine("            from TB_S_M_IMPORT_OTHER I                                                                ");
            sb.AppendLine("            left join TB_H_M_COMPANY C on I.COMPANY_CD = C.COMPANY_CD                                 ");
            sb.AppendLine("            left join TB_9_M_COMM_D e on e.SYS_CD = 'SC' and e.MAIN_CD = 'TAX_FORMAT' and I.TAX_FORMAT = e.SUB_CD ");
            sb.AppendLine("           Where I.DATA_YM = @DATA_YM                                                                 ");
            sb.AppendLine("             and I.COMPANY_CD = @COMPANY_CD  ");
            ht.Add("@DATA_YM", data_ym);
            ht.Add("@COMPANY_CD", company_cd);

            if (emp_id != "")
            {
                sb.AppendLine(" and I.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
            }
            if (emp_name != "")
            {
                sb.AppendLine(" and I.EMP_NAME like '%'+ @EMP_NAME +'%' ");
                ht.Add("@EMP_NAME", emp_name.Trim());
            }
            if (tax_format != "")
            {
                sb.AppendLine(" and I.TAX_FORMAT = @TAX_FORMAT ");
                ht.Add("@TAX_FORMAT", tax_format);
            }
            if (data_format != "")
            {
                sb.AppendLine(" and I.DATA_FORMAT = @DATA_FORMAT ");
                ht.Add("@DATA_FORMAT", data_format);
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
    public int getCount(int startRowIndex, int maximumRows, string company_cd, string data_ym, string emp_id
                            , string emp_name, string tax_format, string data_format)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" select count(*) total_record ");
            sb.AppendLine("            from TB_S_M_IMPORT_OTHER I                                                                ");
            sb.AppendLine("            left join TB_H_M_COMPANY C on I.COMPANY_CD = C.COMPANY_CD                                 ");
            sb.AppendLine("           Where I.DATA_YM = @DATA_YM                                                                 ");
            sb.AppendLine("             and I.COMPANY_CD = @COMPANY_CD  ");
            ht.Add("@DATA_YM", data_ym);
            ht.Add("@COMPANY_CD", company_cd);

            if (emp_id != "")
            {
                sb.AppendLine(" and I.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
            }
            if (emp_name != "")
            {
                sb.AppendLine(" and I.EMP_NAME like '%'+ @EMP_NAME +'%' ");
                ht.Add("@EMP_NAME", emp_name.Trim());
            }
            if (tax_format != "")
            {
                sb.AppendLine(" and I.TAX_FORMAT = @TAX_FORMAT ");
                ht.Add("@TAX_FORMAT", tax_format);
            }
            if (data_format != "")
            {
                sb.AppendLine(" and I.DATA_FORMAT = @DATA_FORMAT ");
                ht.Add("@DATA_FORMAT", data_format);
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

    #region "Import Excel"
    public bool checkExistIsRepeat(string company_cd,string data_ym, string data_format)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select count(1) as totalcount ");
            sb.AppendLine(" from TB_S_M_IMPORT_OTHER ");
            sb.AppendLine(" where COMPANY_CD = @COMPANY_CD ");
            sb.AppendLine(" and DATA_YM = @DATA_YM ");
            sb.AppendLine(" and DATA_FORMAT = @DATA_FORMAT ");
            ht.Add("@COMPANY_CD", company_cd);
            ht.Add("@DATA_YM", data_ym);
            ht.Add("@DATA_FORMAT", data_format);
            DataTable dt = dbConn.Query(sb, ht);
            int total = 0;
            if (dt.Rows.Count > 0)
                total = Convert.ToInt32(dt.Rows[0]["totalcount"]);
            if (total == 0)
                return false;
            else
                return true;
        }
        catch
        {
            throw;
        }
    }
    public void deleteData(string company_cd, string data_ym, string data_format)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //寫log
            sb.AppendLine(" update TB_S_M_IMPORT_OTHER set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2SL110' ");
            sb.AppendLine(" where COMPANY_CD = @COMPANY_CD ");
            sb.AppendLine(" and DATA_YM = @DATA_YM ");
            sb.AppendLine(" and DATA_FORMAT = @DATA_FORMAT; ");
            
            sb.AppendLine(" delete from TB_S_M_IMPORT_OTHER ");
            sb.AppendLine(" where COMPANY_CD = @COMPANY_CD ");
            sb.AppendLine(" and DATA_YM = @DATA_YM ");
            sb.AppendLine(" and DATA_FORMAT = @DATA_FORMAT; ");
            ht.Add("@COMPANY_CD", company_cd);
            ht.Add("@DATA_YM", data_ym);
            ht.Add("@DATA_FORMAT", data_format);
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getEMP_Data(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select EMP_NAME, LICENSE_ID, CONTACT_ZIP_CD, CONTACT_ADDR ");
            sb.AppendLine(" from VW_H_EMP_DATA ");
            sb.AppendLine(" where EMP_ID = @EMP_ID ");
            ht.Add("@EMP_ID", emp_id);
            return dbConn.QueryT(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public bool checkTAX_FORMATIsExist(string tax_format)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select COUNT(1) total         ");
            sb.AppendLine("   from TB_9_M_COMM_D         ");
            sb.AppendLine("  where SYS_CD = 'SC'         ");
            sb.AppendLine("    and MAIN_CD ='TAX_FORMAT' ");
            sb.AppendLine("    and SUB_CD = @TAX_FORMAT  ");
            ht.Add("@TAX_FORMAT", tax_format);
            DataTable dt = dbConn.QueryT(sb, ht);
            int total = 0;
            if (dt.Rows.Count > 0)
                total = Convert.ToInt32(dt.Rows[0]["total"]);
            if (total == 0)
                return true;
            else
                return false;
        }
        catch
        {
            throw;
        }
    }
    public void addImportData_type_IsAorD(string company_cd, string data_format, string data_ym, string cell0, string cell1, string cell2, string cell3, string cell5
                                           , string emp_name, string license_id, string contact_zip_cd, string contact_addr)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" insert into TB_S_M_IMPORT_OTHER ");
            sb.Append(" ( DATA_YM, COMPANY_CD, EMP_ID, AMOUNT, TAX, DATA_FORMAT, TAX_FORMAT ");
            sb.Append(" , TAX_FORMAT_DTL , LICENSE_ID, EMP_NAME, CONTACT_ZIP_CD, CONTACT_ADDR ");
            sb.Append(" , CREATED_BY, CREATED_DT, UPDATED_BY, UPDATED_DT, FUNC_ID) ");
            sb.Append(" values (@DATA_YM, @COMPANY_CD, @EMP_ID, @AMOUNT, @TAX, @DATA_FORMAT, @TAX_FORMAT ");
            sb.Append(" , @TAX_FORMAT_DTL, @LICENSE_ID, @EMP_NAME, @CONTACT_ZIP_CD, @CONTACT_ADDR ");
            sb.Append(" , @CREATED_BY, GETDATE(), @UPDATED_BY, GETDATE(), @FUNC_ID)");

            ht.Add("@DATA_YM", data_ym);
            ht.Add("@COMPANY_CD", company_cd);
            ht.Add("@EMP_ID", cell1);
            ht.Add("@AMOUNT", cell2);
            ht.Add("@TAX", cell3);
            ht.Add("@DATA_FORMAT", cell0);
            ht.Add("@TAX_FORMAT", cell5);
            ht.Add("@TAX_FORMAT_DTL", "");
            ht.Add("@LICENSE_ID", license_id);
            ht.Add("@EMP_NAME", emp_name);
            ht.Add("@CONTACT_ZIP_CD", contact_zip_cd);
            ht.Add("@CONTACT_ADDR", contact_addr);
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SL110");

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void addImportData_type_IsV(string company_cd,string data_format, string data_ym, string cell0,string  cell1, string cell2,string cell3
                                     , string cell4, string cell5, string cell6, string cell7, string cell8, string cell9)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" insert into TB_S_M_IMPORT_OTHER ");
            sb.Append(" ( DATA_YM, COMPANY_CD, EMP_ID, AMOUNT, TAX, DATA_FORMAT, TAX_FORMAT ");
            sb.Append(" , TAX_FORMAT_DTL , LICENSE_ID, EMP_NAME, CONTACT_ZIP_CD, CONTACT_ADDR ");
            sb.Append(" , CREATED_BY, CREATED_DT, UPDATED_BY, UPDATED_DT, FUNC_ID) ");
            sb.Append(" values (@DATA_YM, @COMPANY_CD, @EMP_ID, @AMOUNT, @TAX_1, @DATA_FORMAT, @TAX_FORMAT ");
            sb.Append(" , @TAX_FORMAT_DTL, @LICENSE_ID, @EMP_NAME, @CONTACT_ZIP_CD, @CONTACT_ADDR ");
            sb.Append(" , @CREATED_BY, GETDATE(), @UPDATED_BY, GETDATE(), @FUNC_ID)");

            ht.Add("@DATA_YM", data_ym);
            ht.Add("@COMPANY_CD", company_cd);
            ht.Add("@EMP_ID", cell1);
            ht.Add("@AMOUNT", cell8);
            ht.Add("@TAX_1", cell9);
            ht.Add("@DATA_FORMAT", cell0);
            ht.Add("@TAX_FORMAT", cell6);
            ht.Add("@TAX_FORMAT_DTL", cell7);
            ht.Add("@LICENSE_ID", cell2);
            ht.Add("@EMP_NAME", cell3);
            ht.Add("@CONTACT_ZIP_CD", cell4);
            ht.Add("@CONTACT_ADDR", cell5);
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SL110");

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion
}