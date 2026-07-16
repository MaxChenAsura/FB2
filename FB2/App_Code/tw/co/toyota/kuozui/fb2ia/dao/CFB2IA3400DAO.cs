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
/// CFB2IA3400DAO 的摘要描述
/// </summary>
public class CFB2IA3400DAO : BaseDAO
{
    public string sys_cd { get; set; }
    public string main_cd { get; set; }
    public string is_valid { get; set; }

    public string APP_REMARK { get; set; }
    public string SALARY_YM { get; set; }
    public string EMP_ID { get; set; }
    public string EMP_NAME { get; set; }
    public string TRACE_AMT { get; set; }
    public string REMARK { get; set; }
    public string data_key { get; set; }
    public CFB2IA3400DAO()
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
            sb.Append(" where SYS_CD=@sys_cd and MAIN_CD = @main_cd and IS_VALID=@is_valid");
            ht.Add("@sys_cd", sys_cd);
            ht.Add("@main_cd", main_cd);
            ht.Add("@is_valid", is_valid);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public DataTable emp(string EMP_ID)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.Append(" Select EMP_ID,EMP_NAME,LICENSE_ID From TB_H_M_EMP");
        sb.Append(" where EMP_ID=@EMP_ID");
        ht.Add("@EMP_ID", EMP_ID);
        return dbConn.Query(sb, ht);

    }
    public DataTable GetData(int startRowIndex, int maximumRows, string sortExpression, string salary_ym, string emp_id, string approve_status)
    {
        try
        {
            if (sortExpression.Contains("INS_TYPE_DESC"))
                sortExpression = sortExpression.Replace("INS_TYPE_DESC", "d.sub_desc");
            if (sortExpression.Contains("IDENTITY_KIND_DESC"))
                sortExpression = sortExpression.Replace("IDENTITY_KIND_DESC", "e.sub_desc");
            if (sortExpression.Contains("TRACE_TYPE_DESC"))
                sortExpression = sortExpression.Replace("TRACE_TYPE_DESC", "g.sub_desc");
            if (sortExpression.Contains("TRACE_KIND_DESC"))
                sortExpression = sortExpression.Replace("TRACE_TYPE_DESC", "h.sub_desc");
            if (sortExpression.Contains("EMP_ID"))
                sortExpression = sortExpression.Replace("EMP_ID", "a.EMP_ID");
            if (sortExpression.Contains("LICENSE_ID"))
                sortExpression = sortExpression.Replace("LICENSE_ID", "a.LICENSE_ID");
            if (sortExpression.Contains("EMP_NAME"))
                sortExpression = sortExpression.Replace("EMP_NAME", "f.emp_name");
            if (sortExpression.Contains("REMARK") && !sortExpression.Contains("APP_REMARK"))
                sortExpression = sortExpression.Replace("REMARK", "a.REMARK");
            if (sortExpression.Contains("APP_REMARK"))
                sortExpression = sortExpression.Replace("APP_REMARK", "a.APP_REMARK");
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" a.* ,d.sub_desc as INS_TYPE_DESC,e.sub_desc as IDENTITY_KIND_DESC,f.emp_name,g.sub_desc as TRACE_TYPE_DESC,h.sub_desc as TRACE_KIND_DESC,");
            sb.Append(" a.SALARY_YM+a.EMP_ID+a.INS_TYPE+a.IDENTITY_KIND+a.LICENSE_ID+a.TRACE_KIND as qdatakey,");
            sb.Append(" a.SALARY_YM+a.EMP_ID+a.INS_TYPE as qdatakey2");
            sb.Append(" from TB_I_M_FEES_TRACEBACK a");
            sb.Append(" left join TB_H_M_EMP b on b.EMP_ID = a.UPDATED_BY");
            sb.Append(" left join TB_H_R_HEAD_DEPT c on b.DEPT_NO=c.MNG_DEPT_NO");
            sb.Append(" left join TB_9_M_COMM_D d on d.SYS_CD='IA' AND d.MAIN_CD='INS_TYPE' and a.INS_TYPE= d.sub_cd");
            sb.Append(" left join TB_9_M_COMM_D e on e.SYS_CD='IA' and e.MAIN_CD='IDENTITY_KIND' and a.IDENTITY_KIND=e.sub_cd");
            sb.Append(" left join VW_H_EMP_FAMILY_NAME f on a.IDENTITY_KIND=f.IDENTITY_KIND and a.LICENSE_ID=f.LICENSE_ID and f.EMP_ID=a.EMP_ID");
            sb.Append(" left join TB_9_M_COMM_D g on g.SYS_CD='IA' AND g.MAIN_CD='TRACE_TYPE' and a.TRACE_TYPE= g.sub_cd");
            sb.Append(" left join TB_9_M_COMM_D h on h.SYS_CD='IA' AND h.MAIN_CD='TRACE_KIND' and a.TRACE_KIND= h.sub_cd and h.IS_VALID = 'Y'");
            sb.Append(" where c.EMP_ID=@enter_emp_id and a.APPROVE_STATUS=@approve_status");
            if (salary_ym != "")
            {
                sb.Append(" and a.SALARY_YM=@salary_ym  ");
                ht.Add("@salary_ym", salary_ym.Replace("/", ""));
            }
            if (emp_id != "")
            {
                sb.Append(" and a.EMP_ID = @emp_id  ");
                ht.Add("@emp_id", emp_id);
            }
            sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar) ");

            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            ht.Add("@enter_emp_id", SessionHandle.Current.emp_id);
            ht.Add("@approve_status", approve_status);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public int GetCount(int startRowIndex, int maximumRows, string salary_ym, string emp_id, string approve_status)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select COUNT(*) total_record from TB_I_M_FEES_TRACEBACK a");
            sb.Append(" left join TB_H_M_EMP b on b.EMP_ID = a.UPDATED_BY");
            sb.Append(" left join TB_H_R_HEAD_DEPT c on b.DEPT_NO=c.MNG_DEPT_NO");
            sb.Append(" left join TB_9_M_COMM_D d on d.SYS_CD='IA' AND d.MAIN_CD='INS_TYPE' and a.INS_TYPE= d.sub_cd");
            sb.Append(" left join TB_9_M_COMM_D e on e.SYS_CD='IA' and e.MAIN_CD='IDENTITY_KIND' and a.IDENTITY_KIND=e.sub_cd");
            sb.Append(" left join VW_H_EMP_FAMILY_NAME f on a.IDENTITY_KIND=f.IDENTITY_KIND and a.LICENSE_ID=f.LICENSE_ID and f.EMP_ID=a.EMP_ID");
            sb.Append(" left join TB_9_M_COMM_D g on g.SYS_CD='IA' AND g.MAIN_CD='TRACE_TYPE' and a.TRACE_TYPE= g.sub_cd");
            sb.Append(" left join TB_9_M_COMM_D h on h.SYS_CD='IA' AND h.MAIN_CD='TRACE_KIND' and a.TRACE_KIND= h.sub_cd and h.IS_VALID = 'Y'");
            sb.Append(" where c.EMP_ID=@enter_emp_id and a.APPROVE_STATUS=@approve_status");
            if (salary_ym != "")
            {
                sb.Append(" and a.SALARY_YM=@salary_ym  ");
                ht.Add("@salary_ym", salary_ym.Replace("/", ""));
            }
            if (emp_id != "")
            {
                sb.Append(" and a.EMP_ID = @emp_id  ");
                ht.Add("@emp_id", emp_id);
            }
            ht.Add("@enter_emp_id", SessionHandle.Current.emp_id);
            ht.Add("@approve_status", approve_status);

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
    public void Approve(string appitem, string APP_REMARK)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Update TB_I_M_FEES_TRACEBACK ");
            sb.Append(" Set APPROVE_DT=GETDATE(),APPROVE_STATUS=@APPROVE_STATUS,APP_REMARK=@APP_REMARK,APPROVE_BY=@APPROVE_BY,");
            sb.Append(" UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID");
            sb.Append(" where SALARY_YM+EMP_ID+INS_TYPE+IDENTITY_KIND+LICENSE_ID+TRACE_KIND = @data_key");

            ht.Add("@data_key", appitem);
            ht.Add("@APPROVE_STATUS", "Y");
            ht.Add("@APP_REMARK", APP_REMARK);
            ht.Add("@APPROVE_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2IA340");
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public decimal Calculate(string appitem, string qdata2)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            decimal sum = 0;
            string appitem_value = "";
            string[] appitem_array = appitem.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (string single_appitem in appitem_array)
            {
                appitem_value += "@appitem" + Array.IndexOf(appitem_array, single_appitem).ToString() + ",";
                ht.Add("@appitem" + Array.IndexOf(appitem_array, single_appitem).ToString(), single_appitem);

            }
            appitem_value = appitem_value.Trim(',');
            sb.AppendLine("select isnull(sum(t.TRACE_AMT),0) as TRACE_AMT_SUM");
            sb.AppendLine("from(select SALARY_YM,EMP_ID,INS_TYPE,IDENTITY_KIND,LICENSE_ID,TRACE_KIND,");
            sb.AppendLine("            case");
            sb.AppendLine("              when TRACE_TYPE= 'A' then TRACE_AMT*-1");
            sb.AppendLine("              when TRACE_TYPE= 'B' then TRACE_AMT*1");
            sb.AppendLine("            END 'TRACE_AMT'");
            sb.AppendLine("     from TB_I_M_FEES_TRACEBACK) t");
            sb.AppendLine(" where t.SALARY_YM+t.EMP_ID+t.INS_TYPE = @qdata2");
            sb.AppendLine("   and t.SALARY_YM+t.EMP_ID+t.INS_TYPE+t.IDENTITY_KIND+t.LICENSE_ID+t.TRACE_KIND in (" + appitem_value + ")");

            //ht.Add("@appitem", appitem);

            ht.Add("@qdata2", qdata2);
            DataTable dt = dbConn.QueryT(sb, ht, true);
            if (dt.Rows.Count > 0)
            {
                sum = (decimal)dt.Rows[0]["TRACE_AMT_SUM"];
            }
            return sum;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void TB_S_M_SUBSIDY_DEDUCTIONS_1(string appitem, string qdata2,decimal sum,decimal amount)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            string appitem_value = "";
            string[] appitem_array = appitem.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (string single_appitem in appitem_array)
            {
                appitem_value += "@appitem" + Array.IndexOf(appitem_array, single_appitem).ToString() + ",";
                ht.Add("@appitem" + Array.IndexOf(appitem_array, single_appitem).ToString(), single_appitem);

            }
            appitem_value = appitem_value.Trim(',');
            sb.AppendLine("insert into TB_S_M_SUBSIDY_DEDUCTIONS_1 (DATA_YM,EMP_ID,EMP_NAME,SALARY_ID,SEQ_NO,AMOUNT,IS_PLUS,IS_TAX,REMARK,SALARY_STATUS,");
            sb.AppendLine("                                         SALARY_PROC_DT,SALARY_DT,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.AppendLine("select distinct (select convert(char(6),dateadd(month,1,dbo.FN_S_SALARY_YM()+'01'),112)) as DATA_YM");
            sb.AppendLine("       ,a.EMP_ID,f.EMP_NAME,z.SALARY_ID_NEW");
            sb.AppendLine("       ,(select isnull(max(b.SEQ_NO)+1,1) ");
            sb.AppendLine("         from TB_S_M_SUBSIDY_DEDUCTIONS_1 b ");
            sb.AppendLine("         where b.DATA_YM=z.SALARY_YM and b.EMP_ID=z.EMP_ID and b.SALARY_ID=z.SALARY_ID_NEW ) as SEQ_NO");
            sb.AppendLine("       ,@amount as AMOUNT,c.IS_PLUS,c.IS_TAX");
            sb.AppendLine("       ,(select REPLACE(m.EMP_NAME,' ','')+':'+REMARK + ','"); 
            sb.AppendLine("         from TB_I_M_FEES_TRACEBACK n");
            sb.AppendLine("         left join VW_H_EMP_FAMILY_NAME m on n.IDENTITY_KIND=m.IDENTITY_KIND and n.LICENSE_ID=m.LICENSE_ID and m.EMP_ID=n.EMP_ID"); 
            sb.AppendLine("         where n.EMP_ID = a.EMP_ID");
            sb.AppendLine("               and n.SALARY_YM+n.EMP_ID+n.INS_TYPE+n.IDENTITY_KIND+n.LICENSE_ID+n.TRACE_KIND in (" + appitem_value + ")");
            sb.AppendLine("               and n.SALARY_YM+n.EMP_ID+n.INS_TYPE=@qdata2");
            sb.AppendLine("         FOR XML PATH('')) as REMARKS");
            sb.AppendLine("       ,'N' as SALARY_STATUS,@SALARY_PROC_DT as SALARY_PROC_DT");
            sb.AppendLine("       ,@SALARY_DT as SALARY_DT,@CREATED_BY as CREATED_BY,GETDATE() as CREATED_DT,@UPDATED_BY as UPDATED_BY");
            sb.AppendLine("       ,GETDATE() as UPDATED_DT,@FUNC_ID as FUNC_ID");
            sb.AppendLine("from TB_I_M_FEES_TRACEBACK a ");
            sb.AppendLine("left join TB_H_M_EMP f on a.EMP_ID=f.EMP_ID");

            if (sum > 0)
            {
                sb.AppendLine("     ,(select SALARY_YM,EMP_ID,");
                sb.AppendLine("         case");
                sb.AppendLine("           when TRACE_KIND ='A' and INS_TYPE='A' then '2025'");//個人
                sb.AppendLine("           when TRACE_KIND ='A' and INS_TYPE='B' then '2027'");
                sb.AppendLine("           when TRACE_KIND ='A' and INS_TYPE='C' then '2024'");
                sb.AppendLine("           when TRACE_KIND ='A' and INS_TYPE='D' then '2026'");//團保
                sb.AppendLine("           when TRACE_KIND ='B' and INS_TYPE='A' then '2103'");//雇主
                sb.AppendLine("           when TRACE_KIND ='B' and INS_TYPE='B' then '2104'");
                sb.AppendLine("           when TRACE_KIND ='B' and INS_TYPE='C' then '2101'");
                sb.AppendLine("         end 'SALARY_ID_NEW'");
                sb.AppendLine("       from TB_I_M_FEES_TRACEBACK");
                sb.AppendLine("       where SALARY_YM+EMP_ID+INS_TYPE+IDENTITY_KIND+LICENSE_ID+TRACE_KIND in (" + appitem_value + ")");
                sb.AppendLine("             and SALARY_YM+EMP_ID+INS_TYPE=@qdata2)z");
            }
            if (sum < 0)
            {
                sb.AppendLine("     ,(select SALARY_YM,EMP_ID,");
                sb.AppendLine("         case");
                sb.AppendLine("           when TRACE_KIND ='A' and INS_TYPE='A' then '3025'");
                sb.AppendLine("           when TRACE_KIND ='A' and INS_TYPE='B' then '3027'");
                sb.AppendLine("           when TRACE_KIND ='A' and INS_TYPE='C' then '3024'");
                sb.AppendLine("           when TRACE_KIND ='A' and INS_TYPE='D' then '3026'");//團保
                sb.AppendLine("           when TRACE_KIND ='B' and INS_TYPE='A' then '3103'");
                sb.AppendLine("           when TRACE_KIND ='B' and INS_TYPE='B' then '3104'");
                sb.AppendLine("           when TRACE_KIND ='B' and INS_TYPE='C' then '3101'");
                sb.AppendLine("         end 'SALARY_ID_NEW'");
                sb.AppendLine("       from TB_I_M_FEES_TRACEBACK");
                sb.AppendLine("       where SALARY_YM+EMP_ID+INS_TYPE+IDENTITY_KIND+LICENSE_ID+TRACE_KIND in (" + appitem_value + ")");
                sb.AppendLine("             and SALARY_YM+EMP_ID+INS_TYPE=@qdata2)z"); 
            }
            
            sb.AppendLine("left join TB_S_M_SALARY_ITEM c on c.SALARY_ID=z.SALARY_ID_NEW ");
            sb.AppendLine("where a.SALARY_YM+a.EMP_ID+a.INS_TYPE+a.IDENTITY_KIND+a.LICENSE_ID+a.TRACE_KIND in (" + appitem_value + ")");
            sb.AppendLine("      and a.SALARY_YM+a.EMP_ID+a.INS_TYPE=@qdata2");

            //sum必須為絕對值
            if (sum < 0)
                amount = sum * -1;
            else
                amount = sum;
            ht.Add("@amount", amount);
            ht.Add("@SALARY_PROC_DT", DBNull.Value);
            ht.Add("@SALARY_DT", DBNull.Value);
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2IA340");
            ht.Add("@qdata2", qdata2);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void Reject(string rejitem, string APP_REMARK)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Update TB_I_M_FEES_TRACEBACK ");
            sb.Append(" Set APPROVE_DT=GETDATE(),APPROVE_STATUS=@APPROVE_STATUS,APP_REMARK=@APP_REMARK,APPROVE_BY=@APPROVE_BY,");
            sb.Append(" UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID");
            sb.Append(" where SALARY_YM+EMP_ID+INS_TYPE+IDENTITY_KIND+LICENSE_ID+TRACE_KIND = @data_key");

            ht.Add("@data_key", rejitem);
            ht.Add("@APPROVE_STATUS", "B");
            ht.Add("@APP_REMARK", APP_REMARK);
            ht.Add("@APPROVE_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2IA340");
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

}