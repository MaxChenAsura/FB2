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
/// CFB2SL4100DAO 的摘要描述
/// </summary>
public class CFB2SL4100DAO : BaseDAO
{
    public bool IsSuper;
    public CFB2SL4100DAO()
    {
        IsSuper = isSuperUser();
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public bool isSuperUser()
    {
        bool result = false;
        ACESLib.ACES aces = new ACESLib.ACES();
        String dbRole = aces.GetRoles();
        IList<string> role = dbRole.Split(',');
        try
        {
            foreach (string DB_ROLE_CD in role)
            {
                //string DB_ROLE_CD = "FB2DBMANAGER";
                string SysCode = ((ACESLib.DEPTBean)aces.GetDEPTAuth(DB_ROLE_CD)).SysCode;         //取得「大分類代碼」
                foreach (string big_sysCode in SysCode.Split(','))
                {
                    if (big_sysCode.Trim().Equals("SUPER"))
                    {
                        result = true;
                    }
                }
            }
        }
        catch
        {
        }
        return result;
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
    public DataTable getCommCode2(string sys_cd, string main_cd, string is_valid)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select sub_cd ,sub_desc as sub_desc ");
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

    public DataTable getVENDOR_MEMBER_NAME(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select a.VENDOR_MEMBER_NO,a.VENDOR_MEMBER_NAME EMP_NAME,b.VENDOR_NAME DEPT_NAME ");
            sb.Append(" from TB_D_M_VENDOR_D a ");
            sb.Append(" left join TB_D_M_VENDOR_H b on a.VENDOR_NO=b.VENDOR_NO ");
            sb.Append(" where a.VENDOR_NO is not null and a.VENDOR_MEMBER_NO=@EMP_ID ");
            ht.Add("@EMP_ID", emp_id);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string data_ym, string dept_no, string ws_cd
                             , string emp_id, string license_id, string emp_status)
    {
        try
        {
            if (sortExpression.Contains("EMP_ID"))
                sortExpression = sortExpression.Replace("EMP_ID", "I.EMP_ID");
            if (sortExpression.Contains("EMP_NAME"))
                sortExpression = sortExpression.Replace("EMP_NAME", "I.EMP_NAME");
            if (sortExpression == "")
            {
                sortExpression = "V.PJOB_CD,I.EMP_ID";
            }
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" select * From");
            sb.AppendLine("         (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,                    ");
            sb.AppendLine("                V.DEPT_NO, V.DEPT_NO+'-'+ V.DIV_DEPT_FULL_NAME as DEPT, I.EMP_ID as EMP_ID, I.EMP_NAME          ");
            sb.AppendLine("                , V.EMP_STATUS, V.WS_CD +'-'+ D.SUB_DESC as WS_CD, V.LEVEL_CD, V.GRADE_CD, V.PJOB_CD+'-'+ V.PJOB_DESC as PJOB, I.LICENSE_ID ");
            sb.AppendLine("                , I.DATA_YM + I.LICENSE_ID as qdatakey, V.PJOB_CD                                     ");
            sb.AppendLine("            from TB_S_R_IMX I                                                                         ");
            sb.AppendLine("            left Join VW_H_EMP_DATA V on I.EMP_ID = V.EMP_ID                                          ");
            sb.AppendLine("            left Join TB_9_M_COMM_D D on D.SYS_CD = 'HB' and D.MAIN_CD = 'WS_CD' and D.SUB_CD = V.WS_CD ");
            sb.AppendLine("           Where I.DATA_YM = @DATA_YM  and I.COMPANY_CD = 'K'                                         ");

            ht.Add("@DATA_YM", data_ym);

            if (dept_no != "")
            {
                sb.AppendLine(" and V.DEPT_NO = @DEPT_NO  ");
                ht.Add("@DEPT_NO", dept_no);
            }
            if (ws_cd != "")
            {
                sb.AppendLine(" and V.WS_CD = @WS_CD  ");
                ht.Add("@WS_CD", ws_cd);
            }
            if (IsSuper)
            {
                if (emp_id != "")
                {
                    sb.AppendLine(" and I.EMP_ID like '%'+ @EMP_ID+'%' ");
                    ht.Add("@EMP_ID", emp_id);
                }
            }
            else
            {
                if (emp_id != "")
                {
                    sb.AppendLine(" and I.EMP_ID like '%'+ @EMP_ID+'%' ");
                    ht.Add("@EMP_ID", emp_id);
                }
                sb.AppendLine(" and I.EMP_ID = @EMP_ID2 ");
                ht.Add("@EMP_ID2", SessionHandle.Current.emp_id);
            }
            if (license_id != "")
            {
                sb.AppendLine(" and I.LICENSE_ID = @LICENSE_ID ");
                ht.Add("@LICENSE_ID", license_id);
            }
            if (emp_status != "" && emp_status != "dead")
            {
                sb.AppendLine(" and V.EMP_STATUS = @EMP_STATUS ");
                ht.Add("@EMP_STATUS", emp_status);
            }
            else if (emp_status == "dead")
            {
                sb.AppendLine(" and V.LEAVE_REASON = 'C11' ");
            }

            //sb.AppendLine("  group by I.DATA_YM,I.COMPANY_CD,V.COMPANY_NAME,V.DEPT_NO,V.DIV_DEPT_FULL_NAME,I.EMP_ID,I.EMP_NAME,I.LICENSE_ID ");
            sb.AppendLine("  group by I.DATA_YM,V.DEPT_NO,V.DIV_DEPT_FULL_NAME,I.EMP_ID,I.EMP_NAME,I.LICENSE_ID ");
            sb.AppendLine("           ,V.WS_CD,V.LEVEL_CD,V.GRADE_CD, V.EMP_STATUS,V.PJOB_CD,V.PJOB_DESC,D.SUB_DESC ");

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
    public int getCount(int startRowIndex, int maximumRows, string data_ym, string dept_no, string ws_cd
                             , string emp_id, string license_id, string emp_status)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" Select COUNT(*) total_record ");
            sb.AppendLine(" from (  ");
            sb.AppendLine("          select V.DEPT_NO, V.DEPT_NO+'-'+ V.DIV_DEPT_FULL_NAME as DEPT, I.EMP_ID as EMP_ID, I.EMP_NAME          ");
            sb.AppendLine("                , V.EMP_STATUS, V.WS_CD +'-'+ D.SUB_DESC as WS_CD, V.LEVEL_CD, V.GRADE_CD, V.PJOB_CD+'-'+ V.PJOB_DESC as PJOB, I.LICENSE_ID ");
            sb.AppendLine("                , I.DATA_YM + I.LICENSE_ID as qdatakey, V.PJOB_CD                                     ");
            sb.AppendLine("            from TB_S_R_IMX I                                                                         ");
            sb.AppendLine("            left Join VW_H_EMP_DATA V on I.EMP_ID = V.EMP_ID                                          ");
            sb.AppendLine("            left Join TB_9_M_COMM_D D on D.SYS_CD = 'HB' and D.MAIN_CD = 'WS_CD' and D.SUB_CD = V.WS_CD ");
            sb.AppendLine("           Where I.DATA_YM = @DATA_YM  and I.COMPANY_CD = 'K'                                         ");

            ht.Add("@DATA_YM", data_ym);

            if (dept_no != "")
            {
                sb.AppendLine(" and V.DEPT_NO = @DEPT_NO  ");
                ht.Add("@DEPT_NO", dept_no);
            }
            if (ws_cd != "")
            {
                sb.AppendLine(" and V.WS_CD = @WS_CD  ");
                ht.Add("@WS_CD", ws_cd);
            }
            if (IsSuper)
            {
                if (emp_id != "")
                {
                    sb.AppendLine(" and I.EMP_ID like '%'+ @EMP_ID+'%' ");
                    ht.Add("@EMP_ID", emp_id);
                }
            }
            else
            {
                if (emp_id != "")
                {
                    sb.AppendLine(" and I.EMP_ID like '%'+ @EMP_ID+'%' ");
                    ht.Add("@EMP_ID", emp_id);
                }
                sb.AppendLine(" and I.EMP_ID = @EMP_ID2 ");
                ht.Add("@EMP_ID2", SessionHandle.Current.emp_id);
            }
            if (license_id != "")
            {
                sb.AppendLine(" and I.LICENSE_ID = @LICENSE_ID ");
                ht.Add("@LICENSE_ID", license_id);
            }
            if (emp_status != "" && emp_status != "dead")
            {
                sb.AppendLine(" and V.EMP_STATUS = @EMP_STATUS ");
                ht.Add("@EMP_STATUS", emp_status);
            }
            else if (emp_status == "dead")
            {
                sb.AppendLine(" and V.LEAVE_REASON = 'C11' ");
            }

            sb.AppendLine("  group by I.DATA_YM,V.DEPT_NO,V.DIV_DEPT_FULL_NAME,I.EMP_ID,I.EMP_NAME,I.LICENSE_ID ");
            sb.AppendLine("           ,V.WS_CD,V.LEVEL_CD,V.GRADE_CD, V.EMP_STATUS,V.PJOB_CD,V.PJOB_DESC,D.SUB_DESC ");

            sb.AppendLine(" ) as z");
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

    #region Dtl

    //查詢明細表頭部分
    public DataTable getDtlHeader(string qdatakey)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();

        sb.AppendLine(" select R.DATA_YM, R.COMPANY_CD + '-' + C.COMPANY_SNAME as COMPANY, V.DEPT_NO+ '-' +V.DIV_DEPT_FULL_NAME as DEPT ");
        sb.AppendLine("      ,R.EMP_ID, R.EMP_NAME, R.LICENSE_ID, Sum(R.AMOUNT) As AMOUNT, Sum(R.TAX) As Tax                           ");
        sb.AppendLine("  from TB_S_R_IMX R                                                                                             ");
        sb.AppendLine("  left Join VW_H_EMP_DATA V On R.EMP_ID = V.EMP_ID                                                              ");
        sb.AppendLine("  left Join TB_H_M_COMPANY C On R.COMPANY_CD = C.COMPANY_CD                                                     ");
        sb.AppendLine(" where R.DATA_YM + R.LICENSE_ID = @QDATAKEY and R.COMPANY_CD = 'K'                                              ");
        sb.AppendLine(" group By R.DATA_YM, R.COMPANY_CD, C.COMPANY_SNAME, V.DEPT_NO, V.DIV_DEPT_FULL_NAME                             ");
        sb.AppendLine("		 ,R.EMP_ID, R.EMP_NAME, R.LICENSE_ID                                                                       ");
        sb.AppendLine("	order by  R.EMP_ID                                                                                             ");

        ht.Add("@QDATAKEY", qdatakey);
        return dbConn.Query(sb, ht);
    }
    //查詢明細
    public DataTable getDtlData(int startRowIndex, int maximumRows, string sortExpression, string hid_qdatakey)
    {
        try
        {

            if (sortExpression == "")
            {
                sortExpression = "R.SALARY_DT, R.PAY_KIND, R.PAY_NAME";
            }
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select * from");
            sb.AppendLine(" (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.AppendLine("          R.PAY_KIND,R.PAY_KIND+'-'+R.PAY_NAME as PAY, R.SALARY_DT, Sum(R.AMOUNT) As AMOUNT, Sum(R.TAX) As Tax");
            sb.AppendLine("    from TB_S_R_IMX R                                                                             ");
            sb.AppendLine("    left Join VW_H_EMP_DATA V On R.EMP_ID = V.EMP_ID                                              ");
            sb.AppendLine("   where R.DATA_YM + R.LICENSE_ID = @QDATAKEY  and   R.COMPANY_CD = 'K'                           ");
            sb.AppendLine("   group By R.PAY_KIND, R.PAY_NAME, R.SALARY_DT                                                   ");

            ht.Add("@QDATAKEY", hid_qdatakey);

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
    public int getDtlCount(int startRowIndex, int maximumRows, string hid_qdatakey)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select COUNT(*) total_record ");
            sb.AppendLine("    from TB_S_R_IMX R                                                                             ");
            sb.AppendLine("    left Join VW_H_EMP_DATA V On R.EMP_ID = V.EMP_ID                                              ");
            sb.AppendLine("   where R.DATA_YM + R.LICENSE_ID = @QDATAKEY    and   R.COMPANY_CD = 'K'                         ");
            sb.AppendLine("   group By R.PAY_KIND, R.PAY_NAME, R.SALARY_DT                                                   ");
            ht.Add("@QDATAKEY", hid_qdatakey);
            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                t = dt.Rows.Count;
            }
            return t;
        }
        catch (Exception)
        {
            throw;
        }

    }
    #endregion
}