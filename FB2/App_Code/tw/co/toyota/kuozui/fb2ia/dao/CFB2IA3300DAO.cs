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
/// CFB2IA3300DAO 的摘要描述
/// </summary>
public class CFB2IA3300DAO : BaseDAO
{
    public string sys_cd { get; set; }
    public string main_cd { get; set; }
    public string is_valid { get; set; }
    public string sub_cd { get; set; }

    public string SALARY_YM { get; set; }
    public string EMP_ID { get; set; }
    public string SUB_DESC_INS { get; set; }
    public string SUB_DESC_IDENTITY { get; set; }
    public string LICENSE_ID { get; set; }
    public string SUB_DESC_TRACE_TYPE { get; set; }
    public string TRACE_AMT { get; set; }
    public string REMARK { get; set; }
    public string data_key { get; set; }
    public string TRACE_KIND { get; set; }

	public CFB2IA3300DAO()
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
            if (sub_cd != null && sub_cd != "")
            {
                sb.Append(" and SUB_CD=@sub_cd");
                ht.Add("@sub_cd", sub_cd);
            }
                
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
    public DataTable id(string LICENSE_ID, string EMP_ID)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.Append(" Select FAMILY_NAME From TB_H_M_EMP_FAMILY  ");
        sb.Append(" where FAMILY_LICENSE_ID=@LICENSE_ID");
        if (EMP_ID != "")
        {
            sb.Append(" and EMP_ID=@EMP_ID");
            ht.Add("@EMP_ID", EMP_ID);
        }
        ht.Add("@LICENSE_ID", LICENSE_ID);
        
        return dbConn.Query(sb, ht);
    }
    public DataTable id2(string LICENSE_ID, string EMP_ID)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.Append(" Select EMP_NAME From TB_H_M_EMP ");
        sb.Append(" where LICENSE_ID=@LICENSE_ID");
        if (EMP_ID != "")
        {
            sb.Append(" and EMP_ID=@EMP_ID");
            ht.Add("@EMP_ID", EMP_ID);
        }
        ht.Add("@LICENSE_ID", LICENSE_ID);
        return dbConn.Query(sb, ht);
    }
    public DataTable GetData(int startRowIndex, int maximumRows, string sortExpression, string salary_ym, string emp_id, string approve_status, string trace_kind)
    {
        try
        {
            if (sortExpression.Contains("EMP_ID"))
                sortExpression = sortExpression.Replace("EMP_ID", "a.EMP_ID");
            if (sortExpression.Contains("EMP_NAME"))
                sortExpression = sortExpression.Replace("EMP_NAME", "b.EMP_NAME");
            if (sortExpression.Contains("IDENTITY_KIND"))
                sortExpression = sortExpression.Replace("IDENTITY_KIND", "a.IDENTITY_KIND");
            if (sortExpression.Contains("LICENSE_ID"))
                sortExpression = sortExpression.Replace("LICENSE_ID", "a.LICENSE_ID");
            if (sortExpression.Contains("FAMILY_NAME"))
                sortExpression = sortExpression.Replace("FAMILY_NAME", "c.EMP_NAME");
            if (sortExpression.Contains("REMARK") && !sortExpression.Contains("APP_REMARK"))
                sortExpression = sortExpression.Replace("REMARK", "a.REMARK");
            if (sortExpression.Contains("APP_REMARK"))
                sortExpression = sortExpression.Replace("APP_REMARK", "a.APP_REMARK");
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" Select * From");
            sb.AppendLine(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.AppendLine(" a.SALARY_YM,a.EMP_ID,b.EMP_NAME,a.INS_TYPE+'-'+d.SUB_DESC as INS_TYPE");
            sb.AppendLine(" ,a.IDENTITY_KIND+'-'+e.SUB_DESC as IDENTITY_KIND,a.LICENSE_ID,c.EMP_NAME as FAMILY_NAME,a.TRACE_TYPE+'-'+f.SUB_DESC as TRACE_TYPE,");
            sb.AppendLine(" a.TRACE_AMT,a.REMARK,a.APPROVE_STATUS+'-'+g.SUB_DESC as APPROVE_STATUS,a.APP_REMARK,");
            sb.AppendLine(" case a.IS_YN when 'N' then '未轉薪資'");
            sb.AppendLine("              when 'Y' then '轉薪資'  ");
            sb.AppendLine(" end 'IS_YN' , a.TRACE_KIND+'-'+h.SUB_DESC as  TRACE_KIND                      ");
            sb.AppendLine(" ,a.SALARY_YM+a.EMP_ID+a.INS_TYPE+a.IDENTITY_KIND+a.LICENSE_ID+a.TRACE_KIND as qdatakey");
            sb.AppendLine(" from TB_I_M_FEES_TRACEBACK a");
            sb.AppendLine(" left join TB_H_M_EMP b on a.EMP_ID=b.EMP_ID");
            sb.AppendLine(" left join VW_H_EMP_FAMILY_NAME c on c.IDENTITY_KIND='2' and a.LICENSE_ID=c.LICENSE_ID and a.EMP_ID=c.EMP_ID");
            sb.AppendLine(" left join TB_9_M_COMM_D d on d.SYS_CD='IA' and d.MAIN_CD='INS_TYPE' and d.SUB_CD=a.INS_TYPE");
            sb.AppendLine(" left join TB_9_M_COMM_D e on e.SYS_CD='IA' and e.MAIN_CD='IDENTITY_KIND' and e.SUB_CD=a.IDENTITY_KIND");
            sb.AppendLine(" left join TB_9_M_COMM_D f on f.SYS_CD='IA' and f.MAIN_CD='TRACE_TYPE' and f.SUB_CD=a.TRACE_TYPE");
            sb.AppendLine(" left join TB_9_M_COMM_D g on g.SYS_CD='SA' and g.MAIN_CD='APPROVE_STATUS' and g.SUB_CD=a.APPROVE_STATUS");
            sb.AppendLine(" left join TB_9_M_COMM_D h on h.SYS_CD='IA' and h.MAIN_CD='TRACE_KIND' and h.SUB_CD=a.TRACE_KIND");
            sb.AppendLine(" where 1=1");
            if (salary_ym != "")
            {
                sb.AppendLine(" and a.SALARY_YM=@salary_ym  ");
                ht.Add("@salary_ym", salary_ym.Replace("/", ""));
            }
            if (emp_id != "")
            {
                sb.AppendLine(" and a.EMP_ID = @emp_id  ");
                ht.Add("@emp_id", emp_id);
            }
            if (approve_status != "" && approve_status != "-1")
            {
                sb.AppendLine(" and a.APPROVE_STATUS = @approve_status  ");
                ht.Add("@approve_status", approve_status);
            }
            if (trace_kind != "-1")
            {
                sb.AppendLine(" and a.TRACE_KIND=@trace_kind  ");
                ht.Add("@trace_kind", trace_kind);
            }
            sb.AppendLine(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.AppendLine(" AND CAST(@startRowIndex+@maximumRows as varchar) ");
            
            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public int GetCount(int startRowIndex, int maximumRows, string salary_ym, string emp_id, string approve_status, string trace_kind)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select COUNT(*) total_record from TB_I_M_FEES_TRACEBACK a");
            sb.Append(" left join TB_H_M_EMP b on a.EMP_ID=b.EMP_ID");
            sb.Append(" left join VW_H_EMP_FAMILY_NAME c on c.IDENTITY_KIND='2' and a.LICENSE_ID=c.LICENSE_ID and a.EMP_ID=c.EMP_ID");
            sb.Append(" where 1=1");
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
            if (approve_status != ""&&approve_status != "-1")
            {
                sb.Append(" and a.APPROVE_STATUS = @approve_status  ");
                ht.Add("@approve_status", approve_status);
            }
            if (trace_kind != "-1")
            {
                sb.AppendLine(" and a.TRACE_KIND=@trace_kind  ");
                ht.Add("@trace_kind", trace_kind);
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
    //查詢現有資料
    internal DataTable getExistData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select * from TB_I_M_FEES_TRACEBACK");
            sb.Append(" where SALARY_YM+EMP_ID+INS_TYPE+IDENTITY_KIND+LICENSE_ID+TRACE_KIND = @data_key");
            ht.Add("@data_key", data_key);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
    public void Add()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("INSERT INTO TB_I_M_FEES_TRACEBACK (SALARY_YM,EMP_ID,INS_TYPE,IDENTITY_KIND,LICENSE_ID,TRACE_TYPE,TRACE_AMT,REMARK,TRACE_KIND,");
            sb.Append(" APPROVE_BY,APPROVE_STATUS,APP_REMARK,IS_YN,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" Values (@SALARY_YM,@EMP_ID,@SUB_DESC_INS,@SUB_DESC_IDENTITY,@LICENSE_ID,@SUB_DESC_TRACE_TYPE,@TRACE_AMT,@REMARK,@TRACE_KIND,");
            sb.Append(" @APPROVE_BY,@APPROVE_STATUS,@APP_REMARK,@IS_YN,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@SALARY_YM", SALARY_YM);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SUB_DESC_INS", SUB_DESC_INS);
            ht.Add("@SUB_DESC_IDENTITY", SUB_DESC_IDENTITY);
            ht.Add("@LICENSE_ID", LICENSE_ID.ToUpper());
            ht.Add("@SUB_DESC_TRACE_TYPE", SUB_DESC_TRACE_TYPE);
            ht.Add("@TRACE_AMT", TRACE_AMT);
            ht.Add("@REMARK", REMARK);
            ht.Add("@APPROVE_BY", "");
            ht.Add("@APPROVE_STATUS", "N");
            ht.Add("@APP_REMARK", "");
            ht.Add("@IS_YN", "N");
            ht.Add("@TRACE_KIND", TRACE_KIND);
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2IA330");
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void Update()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Update TB_I_M_FEES_TRACEBACK ");
            sb.Append(" Set TRACE_TYPE=@SUB_DESC_TRACE_TYPE,TRACE_AMT=@TRACE_AMT,REMARK=@REMARK,");
            sb.Append(" APPROVE_DT=@APPROVE_DT,APPROVE_BY=@APPROVE_BY,APPROVE_STATUS=@APPROVE_STATUS,APP_REMARK=@APP_REMARK,TRACE_KIND = @TRACE_KIND,");
            sb.Append(" UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID");
            sb.Append(" where SALARY_YM+EMP_ID+INS_TYPE+IDENTITY_KIND+LICENSE_ID+TRACE_KIND = @data_key");

            ht.Add("@SUB_DESC_TRACE_TYPE", SUB_DESC_TRACE_TYPE);
            ht.Add("@TRACE_AMT", TRACE_AMT);
            ht.Add("@REMARK", REMARK);
            ht.Add("@APPROVE_DT", DBNull.Value);
            ht.Add("@APPROVE_BY", "");
            ht.Add("@TRACE_KIND", TRACE_KIND);
            ht.Add("@APPROVE_STATUS", "N");
            ht.Add("@APP_REMARK", "");
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2IA330");
            ht.Add("@data_key", data_key);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }
    public void Delete(string delitem)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Delete From TB_I_M_FEES_TRACEBACK ");
            sb.Append(" where SALARY_YM+EMP_ID+INS_TYPE+IDENTITY_KIND+LICENSE_ID = @delitem");
            ht.Add("@delitem", delitem);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }
}