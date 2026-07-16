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
/// CFB2IA5100DAO 的摘要描述
/// </summary>
public class CFB2IA5100DAO : BaseDAO
{
    public string EMP_ID { get; set; }
    public string LICENSE_ID_FIRST { get; set; }
    
	public CFB2IA5100DAO()
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
    public DataTable GetData(int startRowIndex, int maximumRows, string sortExpression, string emp_id, string updated_sdt, string updated_edt, string license_id)
    {
        try
        {
            if (sortExpression.Contains("EMP_ID"))
                sortExpression = sortExpression.Replace("EMP_ID", "a.EMP_ID");
            if (sortExpression.Contains("LICENSE_ID"))
                sortExpression = sortExpression.Replace("LICENSE_ID", "a.LICENSE_ID");
            if (sortExpression.Contains("EMP_NAME"))
                sortExpression = sortExpression.Replace("EMP_NAME", "a.EMP_NAME");
            if (sortExpression.Contains("BIRTH_DT"))
                sortExpression = sortExpression.Replace("BIRTH_DT", "a.BIRTH_DT");
            if (sortExpression == "")
                sortExpression = "a.EMP_ID";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" a.EMP_ID,a.UPDATED_DT,b.REATION_NAME,a.LICENSE_ID,a.EMP_NAME,a.BIRTH_DT,a.LICENSE_ID_FIRST ");
            sb.Append(" from TB_I_M_PERSONDATA a");
            sb.Append(" left join VW_H_EMP_FAMILY_NAME b on a.EMP_ID=b.EMP_ID and a.LICENSE_ID=b.LICENSE_ID");
            sb.Append(" where 1=1");

            if (emp_id != "")
            {
                sb.Append(" and a.EMP_ID= @emp_id");
                ht.Add("@emp_id", emp_id);
            }
            if (license_id != "")
            {
                sb.Append(" and a.LICENSE_ID like '" + @license_id + "%'");
                ht.Add("@license_id", license_id);
            }
            if (updated_sdt != "")
            {
                sb.Append(" and CONVERT(varchar(8), a.UPDATED_DT, 112) >= @updated_sdt  ");
                ht.Add("@updated_sdt", updated_sdt.Replace("/", ""));
            }
            //if (updated_sdt != "")
            //{
            //    if (updated_edt != "")
            //    {
            //        sb.Append(" and CONVERT(varchar(8), a.UPDATED_DT, 112) >= @updated_sdt and CONVERT(varchar(8), a.UPDATED_DT, 112) <= @updated_edt ");
            //        ht.Add("@updated_sdt", updated_sdt.Replace("/", ""));
            //        ht.Add("@updated_edt", updated_edt.Replace("/", ""));
            //    }
            //    else
            //    {
            //        sb.Append(" and a.UPDATED_DT >= @updated_sdt  ");
            //        ht.Add("@updated_sdt", updated_sdt.Replace("/", ""));
            //    }

            //}
            if (updated_edt != "")
            {
                sb.Append(" and CONVERT(varchar(8), a.UPDATED_DT, 112) <= @updated_edt  ");
                ht.Add("@updated_edt", updated_edt.Replace("/", ""));
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
    public int GetCount(int startRowIndex, int maximumRows, string emp_id, string updated_sdt, string updated_edt, string license_id)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select COUNT(*) total_record ");
            sb.Append(" from TB_I_M_PERSONDATA a");
            sb.Append(" left join VW_H_EMP_FAMILY_NAME b on a.EMP_ID=b.EMP_ID and a.LICENSE_ID=b.LICENSE_ID");
            sb.Append(" where 1=1");

            if (emp_id != "")
            {
                sb.Append(" and a.EMP_ID= @emp_id");
                ht.Add("@emp_id", emp_id);
            }
            if (license_id != "")
            {
                sb.Append(" and a.LICENSE_ID like '" + @license_id + "%'");
                ht.Add("@license_id", license_id);
            }
            if (updated_sdt != "")
            {
                sb.Append(" and CONVERT(varchar(8), a.UPDATED_DT, 112) >= @updated_sdt  ");
                ht.Add("@updated_sdt", updated_sdt.Replace("/", ""));
            }
            //if (updated_sdt != "")
            //{
            //    if (updated_edt != "")
            //    {
            //        sb.Append(" and a.UPDATED_DT >= @updated_sdt and a.UPDATED_DT <= @updated_edt ");
            //        ht.Add("@updated_sdt", updated_sdt.Replace("/", ""));
            //        ht.Add("@updated_edt", updated_edt.Replace("/", ""));
            //    }
            //    else
            //    {
            //        sb.Append(" and a.UPDATED_DT >= @updated_sdt  ");
            //        ht.Add("@updated_sdt", updated_sdt.Replace("/", ""));
            //    }

            //}
            if (updated_edt != "")
            {
                sb.Append(" and CONVERT(varchar(8), a.UPDATED_DT, 112) <= @updated_edt  ");
                ht.Add("@updated_edt", updated_edt.Replace("/", ""));
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
    public DataTable GetData2(int startRowIndex, int maximumRows, string sortExpression, string EMP_ID, string LICENSE_ID_FIRST)
    {
        try
        {
            if (sortExpression.Contains("EMP_ID"))
                sortExpression = sortExpression.Replace("EMP_ID", "a.EMP_ID");
            if (sortExpression.Contains("LICENSE_ID"))
                sortExpression = sortExpression.Replace("LICENSE_ID", "a.LICENSE_ID");
            if (sortExpression.Contains("EMP_NAME"))
                sortExpression = sortExpression.Replace("EMP_NAME", "a.EMP_NAME");
            if (sortExpression.Contains("BIRTH_DT"))
                sortExpression = sortExpression.Replace("BIRTH_DT", "a.BIRTH_DT");
            if (sortExpression=="")
                sortExpression ="a.EMP_ID";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" a.EMP_ID,a.CREATED_DT,b.REATION_NAME,a.LICENSE_ID,a.EMP_NAME,a.BIRTH_DT ");
            sb.Append(" from TB_I_R_DATAUPDAE_HIS a");
            sb.Append(" left join VW_H_EMP_FAMILY_NAME b on a.EMP_ID=b.EMP_ID and a.LICENSE_ID=b.LICENSE_ID");
            sb.Append(" where a.EMP_ID=@EMP_ID and a.LICENSE_ID_FIRST=@LICENSE_ID_FIRST");

            sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar) ");

            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@LICENSE_ID_FIRST", LICENSE_ID_FIRST);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public int GetCount2(int startRowIndex, int maximumRows, string EMP_ID, string LICENSE_ID_FIRST)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select COUNT(*) total_record");
            sb.Append(" from TB_I_R_DATAUPDAE_HIS a");
            sb.Append(" left join VW_H_EMP_FAMILY_NAME b on a.EMP_ID=b.EMP_ID and a.LICENSE_ID=b.LICENSE_ID");
            sb.Append(" where a.EMP_ID=@EMP_ID and a.LICENSE_ID_FIRST=@LICENSE_ID_FIRST");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@LICENSE_ID_FIRST", LICENSE_ID_FIRST);
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
}