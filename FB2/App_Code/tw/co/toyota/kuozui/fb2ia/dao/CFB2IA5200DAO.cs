using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;

/// <summary>
/// CFB2IA5200DAO 的摘要描述
/// </summary>
public class CFB2IA5200DAO : BaseDAO
{
	public CFB2IA5200DAO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
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
    public DataTable GetData(int startRowIndex, int maximumRows, string sortExpression, string emp_id, string salary_year, string salary_ym)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + ") As RowNumber,");
            sb.Append(" ta.* from (");
            sb.Append(" select '當月代扣' as CTYPE,a.SALARY_YM,c.sub_desc as INS_TYPE_NAME,a.INS_AMT,a.INS_TOTAL,0 as SLEF_RATE");
            sb.Append(" ,a.SALARY_DT,'' as CMEMO");
            sb.Append(" from TB_I_R_FEES_MONTH a");
            sb.Append(" LEFT JOIN TB_9_M_COMM_D c on c.sys_cd='IA' and c.main_cd='INS_TYPE' and c.sub_cd=a.INS_TYPE");
            sb.Append(" where a.IDENTITY_KIND ='1'  AND a.emp_id=@emp_id and a.INS_TYPE in ('A','B')");
            sb.Append(" union all");
            sb.Append(" select '當月代扣' as CTYPE,a.SALARY_YM,'勞退自提' as INS_TYPE_NAME,a.INS_AMT,a.INS_TOTAL,a.SLEF_RATE");
            sb.Append(" ,a.SALARY_DT,'' as CMEMO");
            sb.Append(" from TB_I_R_FEES_MONTH a");
            sb.Append(" where a.IDENTITY_KIND ='1' and a.emp_id=@emp_id and a.INS_TYPE ='C' and a.SLEF_RATE>0");
            sb.Append(" union all");
            sb.Append(" select '雇主提撥' as CTYPE,a.SALARY_YM,'勞退提撥' as INS_TYPE_NAME,a.INS_AMT,a.SELF_D_AMT as INS_TOTAL,0 as SLEF_RATE");
            sb.Append(" ,a.SALARY_DT,'' as CMEMO");
            sb.Append(" from TB_I_R_FEES_MONTH a");
            sb.Append(" where a.IDENTITY_KIND ='1' AND a.emp_id=@emp_id and a.INS_TYPE ='C' and a.SELF_D_AMT>0");
            sb.Append(" union all");//20150930 terry modify
            sb.Append(" select '追溯保費' as CTYPE ,a.SALARY_YM1 as SALARY_YM,c.sub_desc+'-'+d.sub_desc as INS_TYPE_NAME,0 as INS_AMT");
            sb.Append(" ,case when TRACE_TYPE='A' then a.TRACE_AMT else a.TRACE_AMT*-1 end INS_TOTAL,0 as SLEF_RATE");
            sb.Append(" ,a.SALARY_DT,('追溯年月:'+a.SALARY_YM+'_'+a.REMARK) as CMEMO");
            sb.Append(" from  TB_I_M_FEES_TRACEBACK a");
            sb.Append(" LEFT JOIN TB_9_M_COMM_D c on c.sys_cd='IA' and c.main_cd='INS_TYPE' and c.sub_cd=a.INS_TYPE");
            sb.Append(" LEFT JOIN TB_9_M_COMM_D d on d.sys_cd='IA' and d.main_cd='TRACE_KIND' and d.sub_cd=a.TRACE_KIND");//追溯區分
            sb.Append(" where a.IDENTITY_KIND ='1' AND a.emp_id=@emp_id  and (a.TRACE_KIND='A' or (a.TRACE_KIND='B' and a.INS_TYPE='C'))");
            sb.Append(" ) ta");
            sb.Append(" where  CONVERT(varchar(4), ta.SALARY_YM)=@salary_year");
            

            if (salary_ym != "")
            {

                sb.Append(" and ta.SALARY_YM= @salary_ym");
                ht.Add("@salary_ym", salary_ym.Replace("/", ""));
           
            }
            //sb.Append(" order by ta.ins_type_name,ta.SALARY_YM,ta.CTYPE");
                sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar) ");
            
            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            ht.Add("@emp_id", emp_id);
            ht.Add("@salary_year", salary_year);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public int GetCount(int startRowIndex, int maximumRows, string emp_id, string salary_year, string salary_ym)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select COUNT(*) total_record From (");
            sb.Append(" select '當月代扣' as CTYPE,a.SALARY_YM,c.sub_desc as INS_TYPE_NAME,a.INS_AMT,a.INS_TOTAL,0 as SLEF_RATE");
            sb.Append(" ,a.SALARY_DT,'' as CMEMO");
            sb.Append(" from TB_I_R_FEES_MONTH a");
            sb.Append(" LEFT JOIN TB_9_M_COMM_D c on c.sys_cd='IA' and c.main_cd='INS_TYPE' and c.sub_cd=a.INS_TYPE");
            sb.Append(" where a.IDENTITY_KIND ='1'  AND a.emp_id=@emp_id and a.INS_TYPE in ('A','B')");
            sb.Append(" union all");
            sb.Append(" select '當月代扣' as CTYPE,a.SALARY_YM,'勞退自提' as INS_TYPE_NAME,a.INS_AMT,a.INS_TOTAL,a.SLEF_RATE");
            sb.Append(" ,a.SALARY_DT,'' as CMEMO");
            sb.Append(" from TB_I_R_FEES_MONTH a");
            sb.Append(" where a.IDENTITY_KIND ='1' and a.emp_id=@emp_id and a.INS_TYPE ='C' and a.SLEF_RATE>0");
            sb.Append(" union all");
            sb.Append(" select '雇主提撥' as CTYPE,a.SALARY_YM,'勞退提撥' as INS_TYPE_NAME,a.INS_AMT,a.SELF_D_AMT as INS_TOTAL,0 as SLEF_RATE");
            sb.Append(" ,a.SALARY_DT,'' as CMEMO");
            sb.Append(" from TB_I_R_FEES_MONTH a");
            sb.Append(" where a.IDENTITY_KIND ='1' AND a.emp_id=@emp_id and a.INS_TYPE ='C' and a.SELF_D_AMT>0");
            sb.Append(" union all");
            sb.Append(" select '追溯保費' as CTYPE ,a.SALARY_YM1 as SALARY_YM,c.sub_desc+'-'+d.sub_desc  as INS_TYPE_NAME,0 as INS_AMT");
            sb.Append(" ,case when TRACE_TYPE='A' then a.TRACE_AMT else a.TRACE_AMT*-1 end INS_TOTAL,0 as SLEF_RATE");
            sb.Append(" ,a.SALARY_DT,('追溯年月:'+a.SALARY_YM+'_'+a.REMARK) as CMEMO");
            sb.Append(" from  TB_I_M_FEES_TRACEBACK a");
            sb.Append(" LEFT JOIN TB_9_M_COMM_D c on c.sys_cd='IA' and c.main_cd='INS_TYPE' and c.sub_cd=a.INS_TYPE");
            sb.Append(" LEFT JOIN TB_9_M_COMM_D d on d.sys_cd='IA' and d.main_cd='TRACE_KIND' and d.sub_cd=a.TRACE_KIND");//追溯區分
            sb.Append(" where a.IDENTITY_KIND ='1' AND a.emp_id=@emp_id  and (a.TRACE_KIND='A' or (a.TRACE_KIND='B' and a.INS_TYPE='C'))");
            sb.Append(" ) ta");
            sb.Append(" where  CONVERT(varchar(4), ta.SALARY_YM)=@salary_year");

            if (salary_ym != "")
            {

                sb.Append(" and ta.SALARY_YM= @salary_ym");
                ht.Add("@salary_ym", salary_ym.Replace("/", ""));

            }
            ht.Add("@emp_id", emp_id);
            ht.Add("@salary_year", salary_year);
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
    public DataTable GetData2(int startRowIndex, int maximumRows, string sortExpression, string emp_id, string salary_year, string salary_ym)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + ") As RowNumber,");
            sb.Append(" ta.* from (");
            sb.Append(" select '當月代扣' as CTYPE,a.SALARY_YM,b.REATION_NAME,b.EMP_NAME,a.INS_AMT,a.INS_TOTAL,a.SALARY_DT,'' as CMEMO");
            sb.Append(" from TB_I_R_FEES_MONTH a");
            sb.Append(" left join VW_H_EMP_FAMILY_NAME b on a.EMP_ID=b.EMP_ID and a.LICENSE_ID=b.LICENSE_ID and a.IDENTITY_KIND=b.IDENTITY_KIND");
            sb.Append(" where a.IDENTITY_KIND ='2'  and a.emp_id=@emp_id and a.INS_TYPE='B'");
            sb.Append(" union all");
            sb.Append(" select '追溯保費' as CTYPE ,a.SALARY_YM1 as SALARY_YM,b.REATION_NAME,b.EMP_NAME,0 as INS_AMT,case when TRACE_TYPE='A' then a.TRACE_AMT else a.TRACE_AMT*-1 end INS_TOTAL");
            sb.Append(" ,a.SALARY_DT,('追溯年月:'+a.SALARY_YM+'_'+a.REMARK) as CMEMO");
            sb.Append(" from  TB_I_M_FEES_TRACEBACK a");
            sb.Append(" left join VW_H_EMP_FAMILY_NAME b on a.EMP_ID=b.EMP_ID and a.LICENSE_ID=b.LICENSE_ID and a.IDENTITY_KIND=b.IDENTITY_KIND");
            sb.Append(" where a.IDENTITY_KIND ='2' and a.emp_id=@emp_id and a.INS_TYPE='B'");
            sb.Append(" ) ta");
            sb.Append(" where CONVERT(varchar(4), ta.SALARY_YM)=@salary_year");

            if (salary_ym != "")
            {
                sb.Append(" and ta.SALARY_YM= @salary_ym");
                ht.Add("@salary_ym", salary_ym.Replace("/", ""));
            }
            sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar) ");
            
            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            ht.Add("@emp_id", emp_id);
            ht.Add("@salary_year", salary_year);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public int GetCount2(int startRowIndex, int maximumRows, string emp_id, string salary_year, string salary_ym)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select COUNT(*) total_record From (");
            sb.Append(" select '當月代扣' as CTYPE,a.SALARY_YM,b.REATION_NAME,b.EMP_NAME,a.INS_AMT,a.INS_TOTAL,a.SALARY_DT,'' as CMEMO");
            sb.Append(" from TB_I_R_FEES_MONTH a");
            sb.Append(" left join VW_H_EMP_FAMILY_NAME b on a.EMP_ID=b.EMP_ID and a.LICENSE_ID=b.LICENSE_ID and a.IDENTITY_KIND=b.IDENTITY_KIND");
            sb.Append(" where a.IDENTITY_KIND ='2'  and a.emp_id=@emp_id and a.INS_TYPE='B'");
            sb.Append(" union all");
            sb.Append(" select '追溯保費' as CTYPE ,a.SALARY_YM1 as SALARY_YM,b.REATION_NAME,b.EMP_NAME,0 as INS_AMT,case when TRACE_TYPE='A' then a.TRACE_AMT else a.TRACE_AMT*-1 end INS_TOTAL");
            sb.Append(" ,a.SALARY_DT,('追溯年月:'+a.SALARY_YM+'_'+a.REMARK) as CMEMO");
            sb.Append(" from  TB_I_M_FEES_TRACEBACK a");
            sb.Append(" left join VW_H_EMP_FAMILY_NAME b on a.EMP_ID=b.EMP_ID and a.LICENSE_ID=b.LICENSE_ID and a.IDENTITY_KIND=b.IDENTITY_KIND");
            sb.Append(" where a.IDENTITY_KIND ='2' and a.emp_id=@emp_id and a.INS_TYPE='B'");
            sb.Append(" ) ta");
            sb.Append(" where CONVERT(varchar(4), ta.SALARY_YM)=@salary_year");

            if (salary_ym != "")
            {
                sb.Append(" and ta.SALARY_YM= @salary_ym");
                ht.Add("@salary_ym", salary_ym.Replace("/", ""));
            }
            ht.Add("@emp_id", emp_id);
            ht.Add("@salary_year", salary_year);
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
    public DataTable GetData3(int startRowIndex, int maximumRows, string sortExpression, string emp_id, string salary_year, string salary_ym)
    {
        try
        {
            if (sortExpression.Contains("TRAGE_TYPE_NAME"))
                sortExpression = sortExpression.Replace("TRAGE_TYPE_NAME", "d.SUB_DESC");
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select * From");
            sb.Append(" (Select  ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" a.SALARY_YM,b.REATION_NAME,b.EMP_NAME,a.GINS_KIND,a.GFEES_SELF,a.SALARY_DT,d.SUB_DESC as TRAGE_TYPE_NAME");
            sb.Append(" from TB_I_R_GROUP_MONTH a");
            sb.Append(" left join VW_H_EMP_FAMILY_NAME b on a.EMP_ID=b.EMP_ID and a.LICENSE_ID=b.LICENSE_ID and a.IDENTITY_KIND=b.IDENTITY_KIND");
            sb.Append(" left join (select distinct EMP_ID,IDENTITY_KIND,LICENSE_ID,TARGET_TYPE from TB_I_M_GROUP_TXN ) c ");
            sb.Append(" on a.EMP_ID=c.EMP_ID and a.IDENTITY_KIND=c.IDENTITY_KIND and a.LICENSE_ID=c.LICENSE_ID");
            sb.Append(" left join TB_9_M_COMM_D d on d.SYS_CD='IA' and d.MAIN_CD='TARGET_TYPE' and d.SUB_CD=c.TARGET_TYPE");
            sb.Append(" where a.emp_id=@emp_id and CONVERT(varchar(4), a.SALARY_YM)=@salary_year");

            if (salary_ym != "")
            {

                sb.Append(" and a.SALARY_YM= @salary_ym");
                ht.Add("@salary_ym", salary_ym.Replace("/", ""));

            }
            sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar) ");
            

            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            ht.Add("@emp_id", emp_id);
            ht.Add("@salary_year", salary_year);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public int GetCount3(int startRowIndex, int maximumRows, string emp_id, string salary_year, string salary_ym)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select COUNT(*) total_record");
            sb.Append(" from TB_I_R_GROUP_MONTH a");
            sb.Append(" left join VW_H_EMP_FAMILY_NAME b on a.EMP_ID=b.EMP_ID and a.LICENSE_ID=b.LICENSE_ID and a.IDENTITY_KIND=b.IDENTITY_KIND");
            sb.Append(" left join (select distinct EMP_ID,IDENTITY_KIND,LICENSE_ID,TARGET_TYPE from TB_I_M_GROUP_TXN ) c ");
            sb.Append(" on a.EMP_ID=c.EMP_ID and a.IDENTITY_KIND=c.IDENTITY_KIND and a.LICENSE_ID=c.LICENSE_ID");
            sb.Append(" left join TB_9_M_COMM_D d on d.SYS_CD='IA' and d.MAIN_CD='TARGET_TYPE' and d.SUB_CD=c.TARGET_TYPE");
            sb.Append(" where a.emp_id=@emp_id and CONVERT(varchar(4), a.SALARY_YM)=@salary_year");

            if (salary_ym != "")
            {

                sb.Append(" and a.SALARY_YM= @salary_ym");
                ht.Add("@salary_ym", salary_ym.Replace("/", ""));

            }
            ht.Add("@salary_year", salary_year);
            ht.Add("@emp_id", emp_id);
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
}