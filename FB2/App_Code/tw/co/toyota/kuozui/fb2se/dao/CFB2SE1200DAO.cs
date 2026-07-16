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
/// CFB2SE1200DAO 的摘要描述
/// </summary>
public class CFB2SE1200DAO : BaseDAO
{
    public string EMP_ID { get; set; }
    public string DEPT_NO { get; set; }
    public string data_key { get; set; }
    public string EFFECT_YM { get; set; }

	public CFB2SE1200DAO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    public DataTable emp()
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.Append(" Select EMP_ID,EMP_NAME,LICENSE_ID From TB_H_M_EMP");
        sb.Append(" where EMP_ID=@EMP_ID");
        ht.Add("@EMP_ID", EMP_ID);
        return dbConn.Query(sb, ht);

    }
    public DataTable dept()
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.Append(" Select DEPT_NO,DEPT_NAME From VW_H_DEPT_DATA");
        sb.Append(" where DEPT_NO=@DEPT_NO");
        ht.Add("@DEPT_NO", DEPT_NO);
        return dbConn.Query(sb, ht);

    }

    public DataTable GetData(int startRowIndex, int maximumRows, string sortExpression, string effect_ym, string emp_id, string dept_no)
    {
        try
        {
            if (sortExpression.Contains("EMP_ID"))
                sortExpression = sortExpression.Replace("EMP_ID", "a.EMP_ID");
            if (sortExpression.Contains("LEVEL_CD"))
                sortExpression = sortExpression.Replace("LEVEL_CD", "a.LEVEL_CD");
            if (sortExpression.Contains("GRADE_CD"))
                sortExpression = sortExpression.Replace("GRADE_CD", "a.GRADE_CD");
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" Select * From");
            sb.AppendLine(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.AppendLine(" a.EFFECT_YM,a.EMP_ID,b.EMP_NAME,a.LEVEL_CD,a.GRADE_CD,a.DEPT_NO,a.DEPT_NAME_20,a.DEPT_NAME_30,a.DEPT_NAME_40,a.APPROVE_MARK,a.THIS_YEAR_GRADE,a.LEVEL_PAY_OLD");
            sb.AppendLine(" ,a.EXAMINE_ADJ,a.LEVEL_ADJ,a.LEVEL_PAY_NEW,a.ABILITY_PAY_OLD,a.ABILITY_PAY_NEW,a.ABILITY_ADJ,a.LEVEL_PAY_DIFF,a.NOPAYDIFF_YN,");
            sb.AppendLine(" a.EFFECT_YM+a.EMP_ID as qdatakey");
            sb.AppendLine(" from TB_S_M_SALARY_ADJ_D a");
            sb.AppendLine(" left join TB_H_M_EMP b on a.EMP_ID=b.EMP_ID");
            sb.AppendLine(" where 1=1 and a.CHG_STATUS<>'D'");
            if (effect_ym != "")
            {
                sb.AppendLine(" and a.EFFECT_YM=@effect_ym  ");
                ht.Add("@effect_ym", effect_ym.Replace("/", ""));
            }
            if (emp_id != "")
            {
                sb.AppendLine(" and a.EMP_ID = @emp_id  ");
                ht.Add("@emp_id", emp_id);
            }
            if (dept_no != "")
            {
                sb.AppendLine(" and a.DEPT_NO = @dept_no  ");
                ht.Add("@dept_no", dept_no);
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
    public int GetCount(int startRowIndex, int maximumRows, string effect_ym, string emp_id, string dept_no)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("Select COUNT(*) total_record");
            sb.AppendLine(" from TB_S_M_SALARY_ADJ_D a");
            sb.AppendLine(" left join TB_H_M_EMP b on a.EMP_ID=b.EMP_ID");
            sb.AppendLine(" where 1=1 and a.CHG_STATUS<>'D'");
            if (effect_ym != "")
            {
                sb.AppendLine(" and a.EFFECT_YM=@effect_ym  ");
                ht.Add("@effect_ym", effect_ym.Replace("/", ""));
            }
            if (emp_id != "")
            {
                sb.AppendLine(" and a.EMP_ID = @emp_id  ");
                ht.Add("@emp_id", emp_id);
            }
            if (dept_no != "")
            {
                sb.AppendLine(" and a.DEPT_NO = @dept_no  ");
                ht.Add("@dept_no", dept_no);
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
    //刪除
    internal string getExistData()
    {
        try
        {
            string RELEASE_BY="";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("select isnull(RELEASE_BY,'') AS RELEASE_BY from TB_S_M_SALARY_ADJ_H ");
            sb.AppendLine(" where EFFECT_YM=@EFFECT_YM");
            ht.Add("@EFFECT_YM", EFFECT_YM);

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                RELEASE_BY = (string)dt.Rows[0]["RELEASE_BY"];
            }
            return RELEASE_BY;
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
            sb.AppendLine("Update TB_S_M_SALARY_ADJ_D ");
            sb.AppendLine(" Set CHG_STATUS='D'");
            sb.AppendLine(" ,UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID");
            sb.AppendLine(" where EFFECT_YM+EMP_ID = @delitem");
            ht.Add("@delitem", delitem);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SE120");
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //EXCEL
    public DataTable getExcelData()
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" Select ROW_NUMBER() OVER(ORDER BY a.EFFECT_YM,a.EMP_ID,a.DEPT_NAME_20,a.DEPT_NAME_30 ) As RowNumber,");
            sb.AppendLine(" a.EFFECT_YM,a.EMP_ID,b.EMP_NAME,a.LEVEL_CD,a.GRADE_CD,a.DEPT_NO,a.DEPT_NAME_20,a.DEPT_NAME_30,a.DEPT_NAME_40,a.THIS_YEAR_GRADE,a.LEVEL_PAY_OLD");
            sb.AppendLine(" ,a.EXAMINE_ADJ,a.LEVEL_ADJ,a.LEVEL_PAY_NEW,a.ABILITY_PAY_OLD,a.ABILITY_PAY_NEW,a.ABILITY_ADJ,a.LEVEL_PAY_DIFF,a.NOPAYDIFF_YN,");
            sb.AppendLine(" a.EFFECT_YM+a.EMP_ID as qdatakey");
            sb.AppendLine(" from TB_S_M_SALARY_ADJ_D a");
            sb.AppendLine(" left join TB_H_M_EMP b on a.EMP_ID=b.EMP_ID");
            sb.AppendLine(" where 1=1 and a.CHG_STATUS<>'D'");
            if (EFFECT_YM != "")
            {
                sb.AppendLine(" and a.EFFECT_YM=@effect_ym  ");
                ht.Add("@effect_ym", EFFECT_YM);
            }
            if (EMP_ID != "")
            {
                sb.AppendLine(" and a.EMP_ID = @emp_id  ");
                ht.Add("@emp_id", EMP_ID);
            }
            if (DEPT_NO != "")
            {
                sb.AppendLine(" and a.DEPT_NO = @dept_no  ");
                ht.Add("@dept_no", DEPT_NO);
            }
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
}