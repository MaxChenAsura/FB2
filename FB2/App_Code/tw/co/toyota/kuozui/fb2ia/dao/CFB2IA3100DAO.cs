using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;
using System.Text;
using System.Collections;

/// <summary>
/// CFB2IA3100DAO 的摘要描述
/// </summary>
public class CFB2IA3100DAO : BaseDAO
{
    public int count { get; set; }
    public Int64 RowNumber { get; set; }

    public string sys_cd { get; set; }
    public string main_cd { get; set; }
    public string is_valid { get; set; }

    public CFB2IA3100DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public DataTable getDDL()
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select SUB_CD ,SUB_CD+'-'+SUB_DESC SUB_DESC from TB_9_M_COMM_D ");
            sb.Append(" where SYS_CD=@sys_cd and MAIN_CD = @main_cd");
            ht.Add("@sys_cd", sys_cd);
            ht.Add("@main_cd", main_cd);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public DataTable company(string COMPANY_CD)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.Append(" Select COMPANY_CD,COMPANY_SNAME,HEALTH_ORG_ID,LABOR_ORG_ID From TB_H_M_COMPANY");
        sb.Append(" where COMPANY_CD=@COMPANY_CD");
        ht.Add("@COMPANY_CD", COMPANY_CD);
        return dbConn.Query(sb, ht);

    }
    public DataTable GetData(int startRowIndex, int maximumRows, string sortExpression, string company_cd, string bills_kind, string fees_ym,
                                string licence_id, string ins_name, string family_name)
    {
        try
        {
            if (sortExpression.Contains("COMPANY_CD"))
                sortExpression = sortExpression.Replace("COMPANY_CD", "a.COMPANY_CD");
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" a.COMPANY_CD,a.COMPANY_SNAME,b.LICENSE_ID,b.INS_NAME,b.FAMILY_NAME,b.INS_AMT,b.CHANG_TYPE,b.FEES_REMARK,b.FEES_SELF,b.FEES_CMP,b.FEES,b.TRACED_FEES_SELF,");
            sb.Append(" b.TRACED_FEES_CMP,b.TRACED_FEES,b.FEES_TOTAL,b.LAST_UPDATE_DT,b.RATE");
            sb.Append(" from TB_H_M_COMPANY a,TB_I_S_BILLS b");
            sb.Append(" where a.COMPANY_CD = @company_cd and b.COMPANY_CD = @company_cd and b.BILLS_KIND=@bills_kind");
            sb.Append(" and b.FEES_YM = @fees_ym ");
            if (licence_id != "")
            {
                sb.Append(" and b.LICENSE_ID like  @licence_id + '%'");
                ht.Add("@licence_id", licence_id);
            }
            if (ins_name != "")
            {
                sb.Append(" and b.INS_NAME like  @ins_name + '%'");
                ht.Add("@ins_name", ins_name);
            }
            if (family_name != "")
            {
                sb.Append(" and b.FAMILY_NAME like @family_name + '%'");
                ht.Add("@family_name", family_name);
            }
            sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar) ");

            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            ht.Add("@company_cd", company_cd);
            ht.Add("@fees_ym", fees_ym.Replace("/", ""));
            ht.Add("@bills_kind", bills_kind);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public int GetCount(int startRowIndex, int maximumRows, string company_cd, string bills_kind, string fees_ym,
                                string licence_id, string ins_name, string family_name)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select COUNT(*) total_record");
            sb.Append(" from TB_I_S_BILLS ");
            sb.Append(" where COMPANY_CD = @company_cd and BILLS_KIND=@bills_kind");
            sb.Append(" and FEES_YM = @fees_ym ");
            if (licence_id != "")
            {
                sb.Append(" and LICENSE_ID like  @licence_id + '%'");
                ht.Add("@licence_id", licence_id);
            }
            if (ins_name != "")
            {
                sb.Append(" and INS_NAME like  @ins_name + '%'");
                ht.Add("@ins_name", ins_name);
            }
            if (family_name != "")
            {
                sb.Append(" and FAMILY_NAME like  @family_name + '%'");
                ht.Add("@family_name", family_name);
            }
            ht.Add("@company_cd", company_cd);
            ht.Add("@fees_ym", fees_ym.Replace("/", ""));
            ht.Add("@bills_kind", bills_kind);
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
    public void Delete(string BILLS_KIND, string COMPANY_CD, string FEES_YM)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Delete From TB_I_S_BILLS ");
            sb.Append(" where BILLS_KIND = @BILLS_KIND and COMPANY_CD=@COMPANY_CD and FEES_YM=@FEES_YM");
            ht.Add("@BILLS_KIND", BILLS_KIND);
            ht.Add("@COMPANY_CD", COMPANY_CD);
            ht.Add("@FEES_YM", FEES_YM.Replace("/", ""));
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }
    public void Add(string COMPANY_CD, string cell1, string cell2, string cell3, string cell4, string cell5, string cell6, string cell7, string cell8, string cell9, string cell10,
                    string cell11, string cell12, string cell13, string cell14, string cell15, string cell16, string cell17, string cell18, string cell19, string cell20,
                    string cell21, string cell22, string cell23)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("INSERT INTO TB_I_S_BILLS (BILLS_KIND,COMPANY_CD,FEES_YM,ITEM,");
            sb.Append(" INS_AMT,LICENSE_ID,BIRTH_DT,BTYPE,CHANG_TYPE,FEES_REMARK,RATE,FEES_SELF,");
            sb.Append(" FEES_CMP,FEES,TRACED_MEMO,TRACED_YMS,COMPFEES_YM,TRACED_FEES_SELF,TRACED_FEES_CMP,");
            sb.Append(" TRACED_FEES,FEES_TOTAL,INS_REMARK,INS_NAME,FAMILY_REMARK,FAMILY_NAME,");
            sb.Append(" CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" Values ('A',@COMPANY_CD,@FEES_YM,@ITEM,");
            sb.Append(" @INS_AMT,@LICENSE_ID,@BIRTH_DT,@BTYPE,@CHANG_TYPE,@FEES_REMARK,'0',@FEES_SELF,");
            sb.Append(" @FEES_CMP,@FEES_INS,@TRACED_MEMO,@TRACED_YMS,@COMPFEES_YM,@TRACED_FEES_SELF,@TRACED_FEES_CMP,");
            sb.Append(" @TRACED_FEES,@FEES_TOTAL,@INS_REMARK,@INS_NAME,@FAMILY_REMARK,@FAMILY_NAME,");
            sb.Append(" @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@COMPANY_CD", COMPANY_CD);
            ht.Add("@FEES_YM", Convert.ToString(Convert.ToInt64(cell1.Trim().Replace("/", "")) + 191100));
            if (cell3.Trim() == "")
                ht.Add("@ITEM", DBNull.Value);
            else
                ht.Add("@ITEM", cell3.Trim());
            if (cell4.Trim() == "")
                ht.Add("@INS_AMT", DBNull.Value);
            else
                ht.Add("@INS_AMT", cell4.Trim());
            if (cell5.Trim() == "")
                ht.Add("@LICENSE_ID", DBNull.Value);
            else
                ht.Add("@LICENSE_ID", cell5.Trim());
            ht.Add("@BIRTH_DT", Convert.ToString(Convert.ToInt64(cell6.Trim()) + 19110000));
            if (cell7.Trim() == "")
                ht.Add("@BTYPE", DBNull.Value);
            else
                ht.Add("@BTYPE", cell7.Trim());
            if (cell8.Trim() == "")
                ht.Add("@CHANG_TYPE", DBNull.Value);
            else
                ht.Add("@CHANG_TYPE", cell8.Trim());
            if (cell9.Trim() == "")
                ht.Add("@FEES_REMARK", DBNull.Value);
            else
                ht.Add("@FEES_REMARK", cell9.Trim());
            if (cell10.Trim() == "")
                ht.Add("@FEES_SELF", DBNull.Value);
            else
                ht.Add("@FEES_SELF", cell10.Trim());
            if (cell11.Trim() == "")
                ht.Add("@FEES_CMP", DBNull.Value);
            else
                ht.Add("@FEES_CMP", cell11.Trim());
            if (cell12.Trim() == "")
                ht.Add("@FEES_INS", DBNull.Value);
            else
                ht.Add("@FEES_INS", cell12.Trim());
            if (cell13.Trim() == "")
                ht.Add("@TRACED_MEMO", DBNull.Value);
            else
                ht.Add("@TRACED_MEMO", cell13.Trim());
            if (cell14.Trim() == "")
                ht.Add("@TRACED_YMS", DBNull.Value);
            else
                ht.Add("@TRACED_YMS", cell14.Trim());
            if (cell15.Trim() == "")
                ht.Add("@COMPFEES_YM", DBNull.Value);
            else
                ht.Add("@COMPFEES_YM", cell15.Trim());
            if (cell16.Trim() == "")
                ht.Add("@TRACED_FEES_SELF", DBNull.Value);
            else
                ht.Add("@TRACED_FEES_SELF", cell16.Trim());
            if (cell17.Trim() == "")
                ht.Add("@TRACED_FEES_CMP", DBNull.Value);
            else
                ht.Add("@TRACED_FEES_CMP", cell17.Trim());
            if (cell18.Trim() == "")
                ht.Add("@TRACED_FEES", DBNull.Value);
            else
                ht.Add("@TRACED_FEES", cell18.Trim());
            if (cell19.Trim() == "")
                ht.Add("@FEES_TOTAL", DBNull.Value);
            else
                ht.Add("@FEES_TOTAL", cell19.Trim());
            if (cell20.Trim() == "")
                ht.Add("@INS_REMARK", DBNull.Value);
            else
                ht.Add("@INS_REMARK", cell20.Trim());
            if (cell21.Trim() == "")
                ht.Add("@INS_NAME", DBNull.Value);
            else
                ht.Add("@INS_NAME", cell21.Trim());
            if (cell22.Trim() == "")
                ht.Add("@FAMILY_REMARK", DBNull.Value);
            else
                ht.Add("@FAMILY_REMARK", cell22.Trim());
            if (cell23.Trim() == "")
                ht.Add("@FAMILY_NAME", DBNull.Value);
            else
                ht.Add("@FAMILY_NAME", cell23.Trim());
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2IA3100");

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void Add_B(string COMPANY_CD
        , string cell1, string cell2, string cell3, string cell4, string cell5
        , string cell6, string cell7, string cell8, string cell9, string cell10
        , string cell11, string cell12)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("INSERT INTO TB_I_S_BILLS (BILLS_KIND,COMPANY_CD,FEES_YM,ITEM,");
            sb.Append(" INS_AMT,LICENSE_ID,BIRTH_DT,BTYPE,CHANG_TYPE,FEES_REMARK,RATE,FEES_SELF,");
            sb.Append(" FEES_CMP,FEES,TRACED_MEMO,TRACED_YMS,COMPFEES_YM,TRACED_FEES_SELF,TRACED_FEES_CMP,");
            sb.Append(" TRACED_FEES,FEES_TOTAL,INS_REMARK,INS_NAME,FAMILY_REMARK,FAMILY_NAME,LAST_UPDATE_DT,");
            sb.Append(" CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" Values ('B',@COMPANY_CD,@FEES_YM,@ITEM,");
            sb.Append(" @INS_AMT,@LICENSE_ID,@BIRTH_DT,@BTYPE,@CHANG_TYPE,@FEES_REMARK,'0',@FEES_SELF,");
            sb.Append(" @FEES_CMP,@FEES,@TRACED_MEMO,@TRACED_YMS,@COMPFEES_YM,@TRACED_FEES_SELF,@TRACED_FEES_CMP,");
            sb.Append(" @TRACED_FEES,@FEES_TOTAL,@INS_REMARK,@INS_NAME,@FAMILY_REMARK,@FAMILY_NAME,@LAST_UPDATE_DT,");
            sb.Append(" @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@COMPANY_CD", COMPANY_CD);
            ht.Add("@FEES_YM", Convert.ToString(Convert.ToInt64(cell7.Trim()) + 191100));
            ht.Add("@ITEM", count);
            if (cell6.Trim() == "")
                ht.Add("@INS_AMT", DBNull.Value);
            else
                ht.Add("@INS_AMT", cell6.Trim());
            if (cell4.Trim() == "")
                ht.Add("@LICENSE_ID", DBNull.Value);
            else
                ht.Add("@LICENSE_ID", cell4.Trim());
            ht.Add("@BIRTH_DT", Convert.ToString(Convert.ToInt64(cell5.Trim()) + 19110000));
            ht.Add("@BTYPE", DBNull.Value);
            if (cell8.Trim() == "")
                ht.Add("@CHANG_TYPE", DBNull.Value);
            else
                ht.Add("@CHANG_TYPE", cell8.Trim());
            ht.Add("@FEES_REMARK", DBNull.Value);
            if (cell11.Trim() == "")
                ht.Add("@FEES_SELF", DBNull.Value);
            else
                ht.Add("@FEES_SELF", cell11.Trim());
            if (cell12.Trim() == "")
                ht.Add("@FEES_CMP", DBNull.Value);
            else
                ht.Add("@FEES_CMP", cell12.Trim());
            ht.Add("@FEES", Convert.ToString(Convert.ToInt64(cell11.Trim()) + Convert.ToInt64(cell12.Trim())));
            ht.Add("@TRACED_MEMO", DBNull.Value);
            ht.Add("@TRACED_YMS", DBNull.Value);
            ht.Add("@COMPFEES_YM", DBNull.Value);
            ht.Add("@TRACED_FEES_SELF", DBNull.Value);
            ht.Add("@TRACED_FEES_CMP", DBNull.Value);
            ht.Add("@TRACED_FEES", DBNull.Value);
            if (cell11.Trim() == "")
                ht.Add("@FEES_TOTAL", DBNull.Value);
            else
                ht.Add("@FEES_TOTAL", cell11.Trim());
            ht.Add("@INS_REMARK", DBNull.Value);
            if (cell10.Trim() == "")
                ht.Add("@INS_NAME", DBNull.Value);
            else
                ht.Add("@INS_NAME", cell10.Trim());
            ht.Add("@FAMILY_REMARK", DBNull.Value);
            ht.Add("@FAMILY_NAME", DBNull.Value);
            if (cell9.Trim() == "")
                ht.Add("@LAST_UPDATE_DT", DBNull.Value);
            else
                ht.Add("@LAST_UPDATE_DT", Convert.ToString(Convert.ToInt64(cell9.Trim()) + 19110000));
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2IA3100");

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void Add_C(string COMPANY_CD, string cell1, string cell2, string cell3, string cell4, string cell5, string cell6, string cell7, string cell8, string cell9, string cell10,
                    string cell11, string cell12, string cell13, string cell14, string cell15, string cell16)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("INSERT INTO TB_I_S_BILLS (BILLS_KIND,COMPANY_CD,FEES_YM,ITEM,");
            sb.Append(" INS_AMT,LICENSE_ID,BIRTH_DT,BTYPE,CHANG_TYPE,FEES_REMARK,RATE,FEES_SELF,");
            sb.Append(" FEES_CMP,FEES,TRACED_MEMO,TRACED_YMS,COMPFEES_YM,TRACED_FEES_SELF,TRACED_FEES_CMP,");
            sb.Append(" TRACED_FEES,FEES_TOTAL,INS_REMARK,INS_NAME,FAMILY_REMARK,FAMILY_NAME,LAST_UPDATE_DT,");
            sb.Append(" CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" Values ('C',@COMPANY_CD,@FEES_YM,@ITEM,");
            sb.Append(" @INS_AMT,@LICENSE_ID,@BIRTH_DT,@BTYPE,@CHANG_TYPE,@FEES_REMARK,@RATE,@FEES_SELF,");
            sb.Append(" @FEES_CMP,@FEES,@TRACED_MEMO,@TRACED_YMS,@COMPFEES_YM,@TRACED_FEES_SELF,@TRACED_FEES_CMP,");
            sb.Append(" @TRACED_FEES,@FEES_TOTAL,@INS_REMARK,@INS_NAME,@FAMILY_REMARK,@FAMILY_NAME,@LAST_UPDATE_DT,");
            sb.Append(" @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@COMPANY_CD", COMPANY_CD);
            ht.Add("@FEES_YM", Convert.ToString(Convert.ToInt64(cell3.Trim()) + 191100));
            ht.Add("@ITEM", count);
            if (cell8.Trim() == "")
                ht.Add("@INS_AMT", DBNull.Value);
            else
                ht.Add("@INS_AMT", cell8.Trim());
            if (cell7.Trim() == "")
                ht.Add("@LICENSE_ID", DBNull.Value);
            else
                ht.Add("@LICENSE_ID", cell7.Trim());
            ht.Add("@BIRTH_DT", DBNull.Value);
            ht.Add("@BTYPE", DBNull.Value);
            ht.Add("@CHANG_TYPE", DBNull.Value);
            ht.Add("@FEES_REMARK", DBNull.Value);
            if (cell9.Trim() == "")
                ht.Add("@RATE", DBNull.Value);
            else
                ht.Add("@RATE", cell9.Trim());
            if (cell10.Trim() == "")
                ht.Add("@FEES_SELF", DBNull.Value);
            else
                ht.Add("@FEES_SELF", cell10.Trim());
            ht.Add("@FEES_CMP", "0");
            if (cell10.Trim() == "")
                ht.Add("@FEES", DBNull.Value);
            else
                ht.Add("@FEES", cell10.Trim());
            ht.Add("@TRACED_MEMO", DBNull.Value);
            ht.Add("@TRACED_YMS", DBNull.Value);
            ht.Add("@COMPFEES_YM", DBNull.Value);
            ht.Add("@TRACED_FEES_SELF", DBNull.Value);
            ht.Add("@TRACED_FEES_CMP", DBNull.Value);
            ht.Add("@TRACED_FEES", DBNull.Value);
            if (cell10.Trim() == "")
                ht.Add("@FEES_TOTAL", DBNull.Value);
            else
                ht.Add("@FEES_TOTAL", cell10.Trim());
            ht.Add("@INS_REMARK", DBNull.Value);
            if (cell5.Trim() == "")
                ht.Add("@INS_NAME", DBNull.Value);
            else
                ht.Add("@INS_NAME", cell5.Trim());
            ht.Add("@FAMILY_REMARK", DBNull.Value);
            ht.Add("@FAMILY_NAME", DBNull.Value);
            if (cell13.Trim() == "")
                ht.Add("@LAST_UPDATE_DT", DBNull.Value);
            else
                ht.Add("@LAST_UPDATE_DT", Convert.ToString(Convert.ToInt64(cell13.Trim()) + 19110000));
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2IA3100");

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void Add_D(string COMPANY_CD, string cell1, string cell2, string cell3, string cell4, string cell5, string cell6, string cell7, string cell8, string cell9, string cell10,
                    string cell11, string cell12, string cell13, string cell14, string cell15, string cell16)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("INSERT INTO TB_I_S_BILLS (BILLS_KIND,COMPANY_CD,FEES_YM,ITEM,");
            sb.Append(" INS_AMT,LICENSE_ID,BIRTH_DT,BTYPE,CHANG_TYPE,FEES_REMARK,RATE,FEES_SELF,");
            sb.Append(" FEES_CMP,FEES,TRACED_MEMO,TRACED_YMS,COMPFEES_YM,TRACED_FEES_SELF,TRACED_FEES_CMP,");
            sb.Append(" TRACED_FEES,FEES_TOTAL,INS_REMARK,INS_NAME,FAMILY_REMARK,FAMILY_NAME,LAST_UPDATE_DT,");
            sb.Append(" CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" Values ('D',@COMPANY_CD,@FEES_YM,@ITEM,");
            sb.Append(" @INS_AMT,@LICENSE_ID,@BIRTH_DT,@BTYPE,@CHANG_TYPE,@FEES_REMARK,@RATE,@FEES_SELF,");
            sb.Append(" @FEES_CMP,@FEES,@TRACED_MEMO,@TRACED_YMS,@COMPFEES_YM,@TRACED_FEES_SELF,@TRACED_FEES_CMP,");
            sb.Append(" @TRACED_FEES,@FEES_TOTAL,@INS_REMARK,@INS_NAME,@FAMILY_REMARK,@FAMILY_NAME,@LAST_UPDATE_DT,");
            sb.Append(" @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@COMPANY_CD", COMPANY_CD);
            ht.Add("@FEES_YM", Convert.ToString(Convert.ToInt64(cell3.Trim()) + 191100));
            ht.Add("@ITEM", count);
            if (cell8.Trim() == "")
                ht.Add("@INS_AMT", DBNull.Value);
            else
                ht.Add("@INS_AMT", cell8.Trim());
            if (cell7.Trim() == "")
                ht.Add("@LICENSE_ID", DBNull.Value);
            else
                ht.Add("@LICENSE_ID", cell7.Trim());
            ht.Add("@BIRTH_DT", DBNull.Value);
            ht.Add("@BTYPE", DBNull.Value);
            if (cell4.Trim() == "")
                ht.Add("@CHANG_TYPE", DBNull.Value);
            else
                ht.Add("@CHANG_TYPE", cell4.Trim());
            ht.Add("@FEES_REMARK", DBNull.Value);
            if (cell9.Trim() == "")
                ht.Add("@RATE", DBNull.Value);
            else
                ht.Add("@RATE", cell9.Trim());
            if (cell10.Trim() == "")
                ht.Add("@FEES_SELF", DBNull.Value);
            else
                ht.Add("@FEES_SELF", cell10.Trim());
            ht.Add("@FEES_CMP", "0");
            if (cell10.Trim() == "")
                ht.Add("@FEES", DBNull.Value);
            else
                ht.Add("@FEES", cell10.Trim());
            ht.Add("@TRACED_MEMO", DBNull.Value);
            ht.Add("@TRACED_YMS", DBNull.Value);
            ht.Add("@COMPFEES_YM", DBNull.Value);
            ht.Add("@TRACED_FEES_SELF", DBNull.Value);
            ht.Add("@TRACED_FEES_CMP", DBNull.Value);
            ht.Add("@TRACED_FEES", DBNull.Value);
            if (cell10.Trim() == "")
                ht.Add("@FEES_TOTAL", DBNull.Value);
            else
                ht.Add("@FEES_TOTAL", cell10.Trim());
            ht.Add("@INS_REMARK", DBNull.Value);
            if (cell5.Trim() == "")
                ht.Add("@INS_NAME", DBNull.Value);
            else
                ht.Add("@INS_NAME", cell5.Trim());
            ht.Add("@FAMILY_REMARK", DBNull.Value);
            ht.Add("@FAMILY_NAME", DBNull.Value);
            if (cell13.Trim() == "")
                ht.Add("@LAST_UPDATE_DT", DBNull.Value);
            else
                ht.Add("@LAST_UPDATE_DT", Convert.ToString(Convert.ToInt64(cell13.Trim()) + 19110000));
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2IA3100");

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

}