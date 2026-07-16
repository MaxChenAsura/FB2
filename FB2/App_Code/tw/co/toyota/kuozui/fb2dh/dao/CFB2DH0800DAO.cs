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
/// CFB2DH0700DAO 的摘要描述
/// </summary>
public class CFB2DH0800DAO : BaseDAO
{
    public CFB2DH0800DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }


    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string baseyear, string emp_id)
    {
        try
        {
            if (sortExpression.Contains("BASE_YEAR"))
                sortExpression = sortExpression.Replace("BASE_YEAR", "a.BASE_YEAR"); 

            if (sortExpression.Contains("EMP_ID"))
                sortExpression = sortExpression.Replace("EMP_ID", "a.EMP_ID");

            if (sortExpression.Contains("MAIN_LEAVE_CD"))
                sortExpression = sortExpression.Replace("MAIN_LEAVE_CD", "a.MAIN_LEAVE_CD");

            if (sortExpression.Contains("SUB_LEAVE_CD"))
                sortExpression = sortExpression.Replace("SUB_LEAVE_CD", "a.SUB_LEAVE_CD"); 

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * From");
            sb.Append(" (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" a.EMP_ID,BASE_YEAR,a.MAIN_LEAVE_CD+'-'+d.MAIN_LEAVE_DESC MAIN_LEAVE_CD,a.SUB_LEAVE_CD+'-'+e.SUB_LEAVE_DESC SUB_LEAVE_CD, ");
            sb.Append(" Convert(varchar(10),a.START_DT,111)START_DT,Convert(varchar(10),a.END_DT,111)END_DT, convert(decimal(8,2),AVAILABLE_VALUE/60.0 ) AVAILABLE_VALUE ");
            sb.Append(" ,convert(Decimal(8,2),Round( A.DEFFER_VALUE/60.0,2))  as DEFFER_VALUE  ");
            sb.Append(" ,convert(Decimal(8,2),Round( A.APPROVE_VALUE/60.0,2)) as APPROVE_VALUE  ");
            sb.Append(" ,convert(Decimal(8,2),Round( A.USED_PAY_LEAVE_VALUE/60,2)) as USED_PAY_LEAVE_VALUE ");
            sb.Append(" ,A.SALARY_SETTLE_CD +'-'+ isnull(G.SUB_DESC,'') as SALARY_SETTLE_CD_DESC   ");
            sb.Append(" from TB_D_M_EMP_AVAILABLE_LEAVE a  with (nolock) ");            
            sb.Append(" left join TB_D_M_LEAVE_TYPE_H d  with (nolock) on d.MAIN_LEAVE_CD=a.MAIN_LEAVE_CD ");
            sb.Append(" left join TB_D_M_LEAVE_TYPE_D e  with (nolock) on e.MAIN_LEAVE_CD=a.MAIN_LEAVE_CD and e.SUB_LEAVE_CD=a.SUB_LEAVE_CD ");
            sb.AppendLine("         left join TB_9_M_COMM_D G on G.MAIN_CD='SALARY_SETTLE_CD'  and G.SYS_CD='DH'  and G.IS_VALID='Y'  and A.SALARY_SETTLE_CD = G.SUB_CD ");
            sb.Append(" where 1=1 ");

            if (baseyear != "")
            {
                sb.Append(" and a.BASE_YEAR = @BASE_YEAR ");
                ht.Add("@BASE_YEAR", baseyear);                
            }

            if (emp_id != "")
            {
                sb.Append(" and a.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
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

    public int getCount(int startRowIndex, int maximumRows, string baseyear, string emp_id)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select COUNT(*) total_record ");
            sb.Append(" from TB_D_M_EMP_AVAILABLE_LEAVE a  with (nolock) ");
            sb.Append(" left join TB_D_M_LEAVE_TYPE_H d  with (nolock) on d.MAIN_LEAVE_CD=a.MAIN_LEAVE_CD ");
            sb.Append(" left join TB_D_M_LEAVE_TYPE_D e  with (nolock) on e.MAIN_LEAVE_CD=a.MAIN_LEAVE_CD and e.SUB_LEAVE_CD=a.SUB_LEAVE_CD "); 
            sb.Append(" where 1=1 ");

            if (baseyear != "")
            {
                sb.Append(" and a.BASE_YEAR = @BASE_YEAR ");
                ht.Add("@BASE_YEAR", baseyear);
            }

            if (emp_id != "")
            {
                sb.Append(" and a.EMP_ID = @EMP_ID ");
                ht.Add("@emp_id", emp_id);
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

    public DataTable getSUB_LEAVE_CD(string main_leave_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select SUB_LEAVE_CD,SUB_LEAVE_CD+'-'+SUB_LEAVE_DESC SUB_LEAVE_DESC ");
            sb.Append(" from TB_D_M_LEAVE_TYPE_D ");
            sb.Append(" where MAIN_LEAVE_CD=@MAIN_LEAVE_CD ");
            ht.Add("@MAIN_LEAVE_CD", main_leave_cd);

            return dbConn.Query(sb, ht);
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    public DataTable getEmpName(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select EMP_ID,EMP_NAME,DEPT_NO,DEPT_NAME ");
            sb.Append(" from VW_H_EMP_DATA ");
            sb.Append(" where EMP_ID=@EMP_ID ");
            ht.Add("@EMP_ID", emp_id);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getMAIN_LEAVE_DESC(string main_leave_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select MAIN_LEAVE_CD,MAIN_LEAVE_DESC ");
            sb.Append(" from TB_D_M_LEAVE_TYPE_H ");
            sb.Append(" where MAIN_LEAVE_CD=@MAIN_LEAVE_CD ");
            ht.Add("@MAIN_LEAVE_CD", main_leave_cd);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
}