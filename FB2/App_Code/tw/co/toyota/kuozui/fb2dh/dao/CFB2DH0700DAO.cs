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
public class CFB2DH0700DAO : BaseDAO
{
    public CFB2DH0700DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }


    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string apply_leave_sdt, string apply_leave_edt, string emp_id,
        string dept_no, string main_leave_cd, string sub_leave_cd, string level_cd_s, string level_cd_e, string is_super, string is_dept, string departments)
    {
        try
        {

            StringBuilder sb_tb1 = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb_tb1.Append(@" select * from TB_D_M_LEAVE_APPLY_DAY a  with (nolock)  
                             where  1=1     ");

             if (apply_leave_sdt != "")
            {
                if (apply_leave_edt != "")
                {
                    sb_tb1.Append(" and a.APPLY_LEAVE_SDT >= CONVERT(datetime,@apply_leave_sdt) and a.APPLY_LEAVE_EDT <= CONVERT(datetime,@apply_leave_edt)");
                    ht.Add("@apply_leave_sdt", apply_leave_sdt);
                    ht.Add("@apply_leave_edt", apply_leave_edt);
                }
                else
                {
                    sb_tb1.Append(" and a.APPLY_LEAVE_SDT >= CONVERT(datetime,@apply_leave_sdt) ");
                    ht.Add("@apply_leave_sdt", apply_leave_sdt);
                }
            }
            else if (apply_leave_edt != "")
            {
                sb_tb1.Append(" and a.APPLY_LEAVE_EDT <= CONVERT(datetime,@apply_leave_edt) ");
                ht.Add("@apply_leave_edt", apply_leave_edt);
            }

            if (emp_id != "")
            {
                sb_tb1.Append(" and a.EMP_ID = @emp_id ");
                ht.Add("@emp_id", emp_id);
            }

            if (dept_no != "")
            {
                sb_tb1.Append(" and a.DEPT_NO like @dept_no ");
                ht.Add("@dept_no", dept_no+"%");
            }

            if (main_leave_cd != "")
            {
                sb_tb1.Append(" and a.MAIN_LEAVE_CD = @main_leave_cd ");
                ht.Add("@main_leave_cd", main_leave_cd);
            }

            if (sub_leave_cd != "-1" && sub_leave_cd != null)
            {
                sb_tb1.Append(" and  a.SUB_LEAVE_CD = @sub_leave_cd	 ");
                ht.Add("@sub_leave_cd", sub_leave_cd);
            }

            if (level_cd_s != "")
            {
                if (level_cd_e != "")
                {
                    sb_tb1.Append(" and a.LEVEL_CD >= @level_cd_s and a.LEVEL_CD <= @level_cd_e ");
                    ht.Add("@level_cd_s", level_cd_s);
                    ht.Add("@level_cd_e", level_cd_e);
                }
                else
                {
                    sb_tb1.Append(" and a.LEVEL_CD >= @level_cd_s ");
                    ht.Add("@level_cd_s", level_cd_s);
                }
            }
            else if (level_cd_e != "")
            {
                sb_tb1.Append(" and a.LEVEL_CD <= @level_cd_e ");
                ht.Add("@level_cd_e", level_cd_e);
            }
            sb_tb1.Append(" and FORM_STATUS in ('Y','C','X','P') ");





            if (sortExpression.Contains("EMP_ID"))
                sortExpression = sortExpression.Replace("EMP_ID", "a.EMP_ID");

            if (sortExpression.Contains("APPLY_LEAVE_SDT"))
                sortExpression = sortExpression.Replace("APPLY_LEAVE_SDT", "a.APPLY_LEAVE_SDT");

            if (sortExpression.Contains("APPLY_LEAVE_STIME"))
                sortExpression = sortExpression.Replace("APPLY_LEAVE_STIME", "a.APPLY_LEAVE_STIME");

            if (sortExpression.Contains("MAIN_LEAVE_CD"))
                sortExpression = sortExpression.Replace("MAIN_LEAVE_CD", "a.MAIN_LEAVE_CD");

            if (sortExpression.Contains("SUB_LEAVE_CD"))
                sortExpression = sortExpression.Replace("SUB_LEAVE_CD", "a.SUB_LEAVE_CD");

            if (sortExpression.Contains("DEPT_NAME"))
                sortExpression = sortExpression.Replace("DEPT_NAME", "b.DEPT_NAME");

            if (sortExpression.Contains("APPLY_LEAVE_EDT"))
                sortExpression = sortExpression.Replace("APPLY_LEAVE_EDT", "a.APPLY_LEAVE_EDT");

            if (sortExpression.Contains("IFLOW_NO"))
                sortExpression = sortExpression.Replace("IFLOW_NO", "a.IFLOW_NO");


            StringBuilder sb = new StringBuilder();
            sb.Append(" select * From");
            sb.Append(" (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" b.DEPT_NAME DEPT_NAME,a.EMP_ID,c.EMP_NAME EMP_NAME, ");
            sb.Append(" d.MAIN_LEAVE_CD+'-'+d.MAIN_LEAVE_DESC MAIN_LEAVE_CD, ");
            sb.Append(" e.SUB_LEAVE_CD+'-'+e.SUB_LEAVE_DESC SUB_LEAVE_CD, ");
            sb.Append(" CONVERT(char(10), a.APPLY_LEAVE_SDT, 111)  APPLY_LEAVE_SDT,left(REPLACE(CONVERT(char(10), a.APPLY_LEAVE_STIME, 108),'-','/'),5)  APPLY_LEAVE_STIME,  ");
            sb.Append("	CONVERT(char(10), a.APPLY_LEAVE_EDT, 111)  APPLY_LEAVE_EDT,left(REPLACE(CONVERT(char(10), a.APPLY_LEAVE_ETIME, 108),'-','/'),5)  APPLY_LEAVE_ETIME,  ");
            sb.Append(" a.TOTAL_TIME_APPROVE,a.IFLOW_APPROVE_DT, ");
            sb.Append(" f.SUB_CD+'-'+f.SUB_DESC CHECK_STATUS, ");
            sb.Append(" g.SUB_CD+'-'+g.SUB_DESC SALARY_SETTLE_STATUS,a.PAY_DT, ");
            sb.Append(" h.SUB_CD+'-'+h.SUB_DESC FORM_STATUS,a.IFLOW_NO,a.FACT_HAPPEN_DT ");
            //sb.Append(" from TB_D_M_LEAVE_APPLY_DAY a  with (nolock) ");
            sb.Append(" FROM ( " + sb_tb1 + " ) a ");
            //顯示資料權限設定
            if (is_super != "Y")
            {
                sb.Append(@" inner join  (	 select EMP_ID from dbo.FN_H_GET_AUTH_EMP(@loginID,@departments) )T    on A.EMP_ID=T.EMP_ID ");
                ht.Add("@loginID", SessionHandle.Current.emp_id);
                ht.Add("@departments", departments);
            }
            sb.Append(" left join VW_H_DEPT_DATA b  with (nolock) on b.DEPT_NO=a.DEPT_NO ");
            sb.Append(" left join VW_H_EMP_DATA c  with (nolock) on c.EMP_ID=a.EMP_ID ");
            sb.Append(" left join TB_D_M_LEAVE_TYPE_H d  with (nolock) on d.MAIN_LEAVE_CD=a.MAIN_LEAVE_CD ");
            sb.Append(" left join TB_D_M_LEAVE_TYPE_D e  with (nolock) on e.MAIN_LEAVE_CD=a.MAIN_LEAVE_CD and e.SUB_LEAVE_CD=a.SUB_LEAVE_CD ");
            sb.Append(" left join TB_9_M_COMM_D f  with (nolock) on f.MAIN_CD = 'CHECK_STATUS' and f.SYS_CD = 'DI' and f.IS_VALID='Y' and f.SUB_CD=a.CHECK_STATUS ");
            sb.Append(" left join TB_9_M_COMM_D g  with (nolock) on g.MAIN_CD = 'SALARY_SETTLE_STATUS' and g.SYS_CD = 'DI' and g.IS_VALID='Y' and g.SUB_CD=a.SALARY_SETTLE_STATUS ");
            sb.Append(" left join TB_9_M_COMM_D h  with (nolock) on h.MAIN_CD = 'FORM_STATUS' and h.SYS_CD = 'DH' and h.IS_VALID='Y' and h.SUB_CD=a.FORM_STATUS ");
            sb.Append(" where 1=1 ");

           


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

    public int getCount(int startRowIndex, int maximumRows, string apply_leave_sdt, string apply_leave_edt, string emp_id,
        string dept_no, string main_leave_cd, string sub_leave_cd, string level_cd_s, string level_cd_e, string is_super, string is_dept, string departments)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select COUNT(*) total_record ");
            sb.Append(" from TB_D_M_LEAVE_APPLY_DAY a  with (nolock) ");
            //顯示資料權限設定
            if (is_super != "Y")
            {
                sb.Append(@" inner join  (	 select EMP_ID from dbo.FN_H_GET_AUTH_EMP(@loginID,@departments) )T    on A.EMP_ID=T.EMP_ID ");
                ht.Add("@loginID", SessionHandle.Current.emp_id);
                ht.Add("@departments", departments);
            }

            sb.Append(" where 1=1 ");
           
            if (apply_leave_sdt != "")
            {
                if (apply_leave_edt != "")
                {
                    sb.Append(" and a.APPLY_LEAVE_SDT >= CONVERT(datetime,@apply_leave_sdt) and a.APPLY_LEAVE_EDT <= CONVERT(datetime,@apply_leave_edt)");
                    ht.Add("@apply_leave_sdt", apply_leave_sdt);
                    ht.Add("@apply_leave_edt", apply_leave_edt);
                }
                else
                {
                    sb.Append(" and a.APPLY_LEAVE_SDT >= CONVERT(datetime,@apply_leave_sdt) ");
                    ht.Add("@apply_leave_sdt", apply_leave_sdt);
                }
            }
            else if (apply_leave_edt != "")
            {
                sb.Append(" and a.APPLY_LEAVE_EDT <= CONVERT(datetime,@apply_leave_edt) ");
                ht.Add("@apply_leave_edt", apply_leave_edt);
            }

            if (emp_id != "")
            {
                sb.Append(" and a.EMP_ID = @emp_id ");
                ht.Add("@emp_id", emp_id);
            }

            if (dept_no != "")
            {
                sb.Append(" and a.DEPT_NO like @dept_no ");
                ht.Add("@dept_no", dept_no+"%");
            }

            if (main_leave_cd != "")
            {
                sb.Append(" and a.MAIN_LEAVE_CD = @main_leave_cd ");
                ht.Add("@main_leave_cd", main_leave_cd);
            }

            if (sub_leave_cd != "-1" && sub_leave_cd != null)
            {
                sb.Append(" and  a.SUB_LEAVE_CD = @sub_leave_cd	 ");
                ht.Add("@sub_leave_cd", sub_leave_cd);
            }

            if (level_cd_s != "")
            {
                if (level_cd_e != "")
                {
                    sb.Append(" and a.LEVEL_CD >= @level_cd_s and a.LEVEL_CD <= @level_cd_e ");
                    ht.Add("@level_cd_s", level_cd_s);
                    ht.Add("@level_cd_e", level_cd_e);
                }
                else
                {
                    sb.Append(" and a.LEVEL_CD >= @level_cd_s ");
                    ht.Add("@level_cd_s", level_cd_s);
                }
            }
            else if (level_cd_e != "")
            {
                sb.Append(" and a.LEVEL_CD <= @level_cd_e ");
                ht.Add("@level_cd_e", level_cd_e);
            }

            sb.Append(" and a.FORM_STATUS not in ('N','D') ");

            

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