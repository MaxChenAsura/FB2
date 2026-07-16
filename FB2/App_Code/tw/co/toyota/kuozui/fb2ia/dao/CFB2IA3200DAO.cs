using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using FB2.tw.co.toyota.kuozui.dao;
using System.Text;
using System.Collections;

/// <summary>
/// CFB2IA1300DAO 的摘要描述
/// </summary>
public class CFB2IA3200DAO : BaseDAO
{
    public string EMP_ID { get; set; }
    public string SALARY_SYM { get; set; }
    public string SALARY_EYM { get; set; }
    public string COMPANY_CD { get; set; }
    public string EFFECT_DT { get; set; }
    public string AVG_SALARY { get; set; }
    public string A_OLD_INSAMT { get; set; }
    public string A_NEW_INSAMT { get; set; }
    public string B_OLD_INSAMT { get; set; }
    public string B_NEW_INSAMT { get; set; }
    public string C_OLD_INSAMT { get; set; }
    public string C_NEW_INSAMT { get; set; }
    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }

    public string BILLS_KIND { get; set; }
    public string FEES_YM { get; set; }
    public string TRACE_OR_CHANGE { get; set; }
    public string YNB { get; set; }
    public string IDENTITY_KIND { get; set; }
    public string LICENSE_ID { get; set; }

    public CFB2IA3200DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression,
                            string company_cd, string fees_ym, string emp_id, string license_id, string bills_kind, string type, string ynb)
    {
        try
        {
            if (sortExpression.Contains("COMPANY_CD"))
                sortExpression = sortExpression.Replace("COMPANY_CD", "a.COMPANY_CD");
            if (sortExpression.Contains("LICENSE_ID"))
                sortExpression = sortExpression.Replace("LICENSE_ID", "a.LICENSE_ID");
            if (sortExpression.Contains("EMP_ID"))
                sortExpression = sortExpression.Replace("EMP_ID", "a.EMP_ID");
            if (sortExpression.Contains("EMP_NAME"))
                sortExpression = sortExpression.Replace("EMP_NAME", "a.EMP_NAME");
            if (sortExpression.Contains("EMP_CD_NAME"))
                sortExpression = sortExpression.Replace("EMP_CD_NAME", "e.sub_desc");
            if (sortExpression.Contains("EMP_CHG_CD_NAME"))
                sortExpression = sortExpression.Replace("EMP_CHG_CD_NAME", "d.sub_desc");
            if (sortExpression.Contains("IDENTITY_KIND_NAME"))
                sortExpression = sortExpression.Replace("IDENTITY_KIND_NAME", "f.sub_desc");
            if (sortExpression.Contains("FAMILY_NAME"))
                sortExpression = sortExpression.Replace("FAMILY_NAME", "a.FAMILY_NAME");
            if (sortExpression.Contains("BILLS_INS_AMT"))
                sortExpression = sortExpression.Replace("BILLS_INS_AMT", "a.BILLS_INS_AMT");
            if (sortExpression.Contains("CHANG_TYPE"))
                sortExpression = sortExpression.Replace("CHANG_TYPE", "a.CHANG_TYPE");
            if (sortExpression.Contains("FEES_REMARK"))
                sortExpression = sortExpression.Replace("FEES_REMARK", "a.FEES_REMARK");
            if (sortExpression.Contains("FEES_SELF"))
                sortExpression = sortExpression.Replace("FEES_SELF", "a.FEES_SELF");
            if (sortExpression.Contains("FEES_CMP"))
                sortExpression = sortExpression.Replace("FEES_CMP", "a.FEES_CMP");
            if (sortExpression.Contains("FEES"))
                sortExpression = sortExpression.Replace("FEES", "a.FEES");
            if (sortExpression.Contains("TRACED_FEES_SELF"))
                sortExpression = sortExpression.Replace("TRACED_FEES_SELF", "a.TRACED_FEES_SELF");
            if (sortExpression.Contains("TRACED_FEES_CMP"))
                sortExpression = sortExpression.Replace("TRACED_FEES_CMP", "a.TRACED_FEES_CMP");
            if (sortExpression.Contains("TRACED_FEES"))
                sortExpression = sortExpression.Replace("TRACED_FEES", "a.TRACED_FEES");
            if (sortExpression.Contains("BILLS_TOT"))
                sortExpression = sortExpression.Replace("BILLS_TOT", "a.TRACED_FEES+a.FEES");
            if (sortExpression.Contains("INS_FEES"))
                sortExpression = sortExpression.Replace("INS_FEES", "a.INS_FEES");
            if (sortExpression.Contains("DIFF_AMT"))
                sortExpression = sortExpression.Replace("DIFF_AMT", "a.BILLS_FEES-a.INS_FEES");
            if (sortExpression.Contains("PROCESS_MEMO"))
                sortExpression = sortExpression.Replace("PROCESS_MEMO", "a.PROCESS_MEMO");

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select * from ");
            sb.AppendLine(" (select  row_number() over( order by " + sortExpression + ") as RowNumber,b.COMPANY_SNAME,a.LICENSE_ID,a.EMP_ID,a.EMP_NAME,d.sub_desc as EMP_CHG_CD_NAME ,e.sub_desc as EMP_CD_NAME ");
            sb.AppendLine("        ,f.SUB_DESC as IDENTITY_KIND_NAME,a.FAMILY_NAME,a.BILLS_INS_AMT,a.CHANG_TYPE,a.FEES_REMARK,a.FEES_SELF,a.FEES_CMP");
            sb.AppendLine("        ,a.FEES,a.TRACED_FEES_SELF,a.TRACED_FEES_CMP,a.TRACED_FEES,a.TRACED_FEES_SELF+a.FEES_SELF as BILLS_TOT,a.INS_FEES,(a.FEES_SELF+a.TRACED_FEES_SELF-a.INS_FEES) as DIFF_AMT");
            sb.AppendLine("        ,a.PROCESS_MEMO,a.LAST_UPDATE_DT,a.BILLS_FEES,a.RATE,(a.BILLS_FEES-a.INS_FEES) as DIFF_AMT1	");
            sb.AppendLine(" from TB_I_M_BILLS_COMPARE a");
            sb.AppendLine(" left join TB_H_M_COMPANY b on a.COMPANY_CD= b.COMPANY_CD");
            sb.AppendLine(" left join VW_H_EMP_DATA c on a.emp_id= c.emp_id");
            sb.AppendLine(" left join TB_9_M_COMM_D d on d.SYS_CD='HB' and d.main_cd='EMP_CHG_CD' and d.sub_cd= c.EMP_CHG_CD /*在職區分*/ ");
            sb.AppendLine(" left join TB_9_M_COMM_D e on e.SYS_CD='HB' and e.main_cd='EMP_CD' and e.sub_cd= c.EMP_CD /*員工區分*/	");
            sb.AppendLine(" left join TB_9_M_COMM_D f on f.SYS_CD='IA' and f.main_cd='IDENTITY_KIND' and f.sub_cd= a.IDENTITY_KIND /*身分別*/");
            sb.AppendLine(" where a.company_cd='" + company_cd + "' and a.BILLS_KIND='" + bills_kind + "' ");
            //保費年月
            if (fees_ym != "")
            {
                sb.AppendLine(" and a.FEES_YM = @fees_ym ");
                ht.Add("@fees_ym", fees_ym.Replace("/", ""));
            }
            if (emp_id != "")
            {
                sb.AppendLine(" and a.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
            }
            if (license_id != "")
            {
                sb.AppendLine(" and a.LICENSE_ID like @LICENSE_ID ");
                ht.Add("@LICENSE_ID", license_id + '%');
            }
            if (type == "1")//追溯處理
            {
                sb.AppendLine(" and a.TRACED_YN = @TRACED_YN ");
                ht.Add("@TRACED_YN", ynb);
            }
            if (type == "2")//異動投保等級
            {
                sb.AppendLine(" and a.CHANG_LEVEL_YN = @CHANG_LEVEL_YN ");
                ht.Add("@CHANG_LEVEL_YN", ynb);
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

    public int getCount(int startRowIndex, int maximumRows, string company_cd, string fees_ym, string emp_id, string license_id, string bills_kind, string type, string ynb)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("select COUNT(a.EMP_ID) total_record ");
            sb.AppendLine(" from TB_I_M_BILLS_COMPARE a");
            sb.AppendLine(" where a.company_cd='" + company_cd + "' and a.BILLS_KIND='" + bills_kind + "' ");
            //保費年月
            if (fees_ym != "")
            {
                sb.AppendLine(" and a.FEES_YM = @fees_ym ");
                ht.Add("@fees_ym", fees_ym.Replace("/", ""));
            }
            if (emp_id != "")
            {
                sb.AppendLine(" and a.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
            }
            if (license_id != "")
            {
                sb.AppendLine(" and a.LICENSE_ID like @LICENSE_ID ");
                ht.Add("@LICENSE_ID", license_id + '%');
            }
            if (type == "1")//追溯處理
            {
                sb.AppendLine(" and a.TRACED_YN = @TRACED_YN ");
                ht.Add("@TRACED_YN", ynb);
            }
            if (type == "2")//異動投保等級
            {
                sb.AppendLine(" and a.CHANG_LEVEL_YN = @CHANG_LEVEL_YN ");
                ht.Add("@CHANG_LEVEL_YN", ynb);
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

    public void Check_FeeA(string def_ym,string company_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_I_COMPARE_3200_A");
            ht.Add("@qry_company_cd", company_cd);
            ht.Add("@qry_salary_ym", def_ym);
            ht.Add("@qry_user_id", SessionHandle.Current.emp_id);
            dbConn.ExecuteSP(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void Check_FeeB(string def_ym, string company_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_I_COMPARE_3200_B");
            ht.Add("@qry_company_cd", company_cd);
            ht.Add("@qry_salary_ym", def_ym);
            ht.Add("@qry_user_id", SessionHandle.Current.emp_id);
            dbConn.ExecuteSP(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void Check_FeeC(string def_ym, string company_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_I_COMPARE_3200_C");
            ht.Add("@qry_company_cd", company_cd);
            ht.Add("@qry_salary_ym", def_ym);
            ht.Add("@qry_user_id", SessionHandle.Current.emp_id);
            dbConn.ExecuteSP(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void Check_FeeD(string def_ym, string company_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_I_COMPARE_3200_D");
            ht.Add("@qry_company_cd", company_cd);
            ht.Add("@qry_salary_ym", def_ym);
            ht.Add("@qry_user_id", SessionHandle.Current.emp_id);
            dbConn.ExecuteSP(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable countRow(string BILLS_KIND, string FEES_YM, string COMPANY_CD)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.AppendLine(" Select count(*) row From TB_I_M_BILLS_COMPARE");
        sb.AppendLine(" where BILLS_KIND = @BILLS_KIND and COMPANY_CD=@COMPANY_CD and FEES_YM = @FEES_YM");

        ht.Add("@BILLS_KIND", BILLS_KIND);
        ht.Add("@COMPANY_CD", COMPANY_CD);
        ht.Add("@FEES_YM", FEES_YM);

        return dbConn.QueryT(sb, ht);

    }
    public DataTable checkStatus(string BILLS_KIND, string FEES_YM, string COMPANY_CD)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.AppendLine(" Select TRACED_YN,CHANG_LEVEL_YN  From TB_I_M_BILLS_COMPARE");
        sb.AppendLine(" where BILLS_KIND = @BILLS_KIND and COMPANY_CD=@COMPANY_CD and FEES_YM = @FEES_YM");
        sb.AppendLine(" group by TRACED_YN,CHANG_LEVEL_YN");

        ht.Add("@BILLS_KIND", BILLS_KIND);
        ht.Add("@COMPANY_CD", COMPANY_CD);
        ht.Add("@FEES_YM", FEES_YM);

        return dbConn.Query(sb, ht);

    }
    public void callEMP(string yyyymmdd)
    {
        try
        {
            
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_I_EMP_INCOMPANY");
            ht.Add("@pDesc", "FB2IA320");
            String aa = Convert.ToDateTime(yyyymmdd).AddMonths(1).AddDays(-1).ToShortDateString();
            ht.Add("@pDate",  Convert.ToDateTime(yyyymmdd).AddMonths(1).AddDays(-1).ToShortDateString());
            ht.Add("@pUserID", SessionHandle.Current.emp_id);
            ht.Add("@pFuncID", "FB2IA320");
            dbConn.ExecuteSP(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable company(string COMPANY_CD)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.AppendLine(" Select COMPANY_CD,COMPANY_SNAME From TB_H_M_COMPANY");
        sb.AppendLine(" where COMPANY_CD=@COMPANY_CD");
        ht.Add("@COMPANY_CD", COMPANY_CD);
        return dbConn.Query(sb, ht);

    }
    public DataTable getExcelData(string company_cd, string fees_ym, string emp_id, string license_id, string bills_kind)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select * from ");
            sb.AppendLine(" (select  row_number() over( order by a.emp_id) as RowNumber,b.COMPANY_SNAME,a.LICENSE_ID,a.EMP_ID,a.EMP_NAME");
            sb.AppendLine("        ,IIF(ISNULL(d.sub_desc,'')='',h.sub_desc,d.sub_desc) as EMP_CHG_CD_NAME ,IIF(ISNULL(e.sub_desc,'')='',i.sub_desc,e.sub_desc) as EMP_CD_NAME ");
            sb.AppendLine("        ,f.SUB_DESC as IDENTITY_KIND_NAME,a.FAMILY_NAME,a.BILLS_INS_AMT,a.CHANG_TYPE,a.FEES_REMARK,a.FEES_SELF,a.FEES_CMP");
            sb.AppendLine("        ,a.FEES,a.TRACED_FEES_SELF,a.TRACED_FEES_CMP,a.TRACED_FEES,(a.TRACED_FEES_SELF+a.FEES_SELF) as BILLS_TOT,a.INS_FEES,(a.FEES_SELF+a.TRACED_FEES_SELF-a.INS_FEES) as DIFF_AMT");
            sb.AppendLine("        ,a.TRACED_MEMO,a.TRACED_YMS,a.COMPFEES_YM ");
            sb.AppendLine("        ,a.PROCESS_MEMO,a.LAST_UPDATE_DT,a.BILLS_FEES,a.RATE,(a.BILLS_FEES-a.INS_FEES) as DIFF_AMT1	");
            sb.AppendLine(" from TB_I_M_BILLS_COMPARE a");
            sb.AppendLine(" left join TB_H_M_COMPANY b on a.COMPANY_CD= b.COMPANY_CD");
            sb.AppendLine(" left join TB_H_R_EMP_DATA_MONTH c on a.emp_id= c.emp_id and c.YM = @fees_ym ");
            sb.AppendLine(" left join TB_9_M_COMM_D d on d.SYS_CD='HB' and d.main_cd='EMP_CHG_CD' and d.sub_cd= c.EMP_CHG_CD /*在職區分*/ ");
            sb.AppendLine(" left join TB_9_M_COMM_D e on e.SYS_CD='HB' and e.main_cd='EMP_CD' and e.sub_cd= c.EMP_CD /*員工區分*/	");
            sb.AppendLine(" left join TB_9_M_COMM_D f on f.SYS_CD='IA' and f.main_cd='IDENTITY_KIND' and f.sub_cd= a.IDENTITY_KIND /*身分別*/");
            sb.AppendLine(" left join VW_H_EMP_DATA g on a.emp_id = g.emp_id");
            sb.AppendLine(" left join TB_9_M_COMM_D h on h.SYS_CD='HB' and h.main_cd='EMP_CHG_CD' and h.sub_cd= g.EMP_CHG_CD");
            sb.AppendLine(" left join TB_9_M_COMM_D i on i.SYS_CD='HB' and i.main_cd='EMP_CD' and i.sub_cd= g.EMP_CD /*員工區分*/");
            sb.AppendLine(" where a.company_cd='" + company_cd + "' and a.BILLS_KIND='" + bills_kind + "' ");
            //保費年月
            if (fees_ym != "")
            {
                sb.AppendLine(" and a.FEES_YM = @fees_ym ");
                ht.Add("@fees_ym", fees_ym.Replace("/", ""));
            }
            if (emp_id != "")
            {
                sb.AppendLine(" and a.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
            }
            if (license_id != "")
            {
                sb.AppendLine(" and a.LICENSE_ID like @LICENSE_ID ");
                ht.Add("@LICENSE_ID", license_id + '%');
            }

            sb.AppendLine(" ) z");
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getEmp_Name(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select EMP_NAME ");
            sb.AppendLine(" from VW_H_EMP_DATA ");
            sb.AppendLine(" where EMP_ID =@EMP_ID ");
            ht.Add("@EMP_ID", emp_id);
            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void update_BILLS_COMPARE()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" update TB_I_M_BILLS_COMPARE");
            //週一改 還要區分追溯處理否 或 異動投保等級否
            if (TRACE_OR_CHANGE == "1") //追溯處理
            {
                sb.AppendLine(" set TRACED_YN = @YNB ");
            }
            if (TRACE_OR_CHANGE == "2") //異動投保等級
            {
                sb.AppendLine(" set CHANG_LEVEL_YN = @YNB ");
            }
            sb.AppendLine(" , UPDATED_BY = @UPDATED_BY , UPDATED_DT = getdate() , FUNC_ID = @FUNC_ID ");
            sb.AppendLine(" where BILLS_KIND =@BILLS_KIND  and FEES_YM= @FEES_YM and COMPANY_CD = @COMPANY_CD and EMP_ID = @EMP_ID");
            sb.AppendLine(" and LICENSE_ID= @LICENSE_ID");
            if (IDENTITY_KIND !="")
            {
                sb.AppendLine(" and IDENTITY_KIND =@IDENTITY_KIND");
                ht.Add("@IDENTITY_KIND", IDENTITY_KIND);     
            }

            ht.Add("@YNB", YNB);
            ht.Add("@BILLS_KIND", BILLS_KIND);
            ht.Add("@FEES_YM", FEES_YM);
            ht.Add("@COMPANY_CD", COMPANY_CD);
            ht.Add("@EMP_ID", EMP_ID);                   
            ht.Add("@LICENSE_ID", LICENSE_ID);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.QueryT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
}