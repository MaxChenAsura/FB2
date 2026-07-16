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
/// CFB2SL3100DAO 的摘要描述
/// </summary>
public class CFB2SK1200DAO : BaseDAO
{
    public bool IsSuper;

    public CFB2SK1200DAO()
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
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string data_ym,
                         string dept_no, string emp_id, string license_id)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY PJOB_CD,I.EMP_ID ASC ) As RowNumber,");
            sb.Append(" I.YEAR,concat(V.DEPT_NO,'-'+V.DIV_DEPT_FULL_NAME)as DEPT_NO,I.EMP_ID,I.EMP_NAME,V.EMP_STATUS,V.WS_CD,");
            sb.Append(" V.WS_CD+'-'+d.SUB_DESC as WS_CD_DESC,");
            sb.Append(" V.LEVEL_CD,V.GRADE_CD,concat(V.PJOB_CD,'-'+V.PJOB_DESC)as PJOB_CD,I.LICENSE_ID,V.SALARY_EMAIL");
            sb.Append(" from TB_S_R_MUTUAL_YEAR_DTL I ");
            sb.Append(" left join VW_H_EMP_DATA V On I.EMP_ID = V.EMP_ID");
            sb.Append(" left join TB_9_M_COMM_D d on d.SYS_CD = 'HB' and d.MAIN_CD = 'WS_CD' and V.WS_CD = d.SUB_CD ");           
            sb.Append(" where I.YEAR = @DATA_YM ");
            ht.Add("@DATA_YM", data_ym);

            if (dept_no != "")
            {
                sb.Append("and V.DEPT_NO = @DEPT_NO ");
                ht.Add("@DEPT_NO", dept_no);
            }
            if (IsSuper)
            {
                if (emp_id != "")
                {
                    sb.Append("and I.EMP_ID like '%'+ @EMP_ID+'%' ");
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
                sb.Append("and I.EMP_ID = @EMP_ID2 ");
                ht.Add("@EMP_ID2", SessionHandle.Current.emp_id);
            }
            if (license_id != "")
            {
                sb.Append(" and I.LICENSE_ID = @LICENSE_ID ");
                ht.Add("@LICENSE_ID", license_id);
            }
           
            sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar)");

            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    //Gridview 查詢總筆數
    public int getCount(int startRowIndex, int maximumRows, string data_ym,
                         string dept_no, string emp_id, string license_id)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select COUNT(*) total_record ");
            sb.Append(" from TB_S_R_MUTUAL_YEAR_DTL I ");
            sb.Append(" left join VW_H_EMP_DATA V On I.EMP_ID = V.EMP_ID");
            sb.Append(" left join TB_9_M_COMM_D d on d.SYS_CD = 'HB' and d.MAIN_CD = 'WS_CD' and V.WS_CD = d.SUB_CD ");
            sb.Append(" where I.YEAR = @DATA_YM ");
            ht.Add("@DATA_YM", data_ym);

            if (dept_no != "")
            {
                sb.Append("and V.DEPT_NO = @DEPT_NO ");
                ht.Add("@DEPT_NO", dept_no);
            }
            if (IsSuper)
            {
                if (emp_id != "")
                {
                    sb.Append("and I.EMP_ID like '%'+ @EMP_ID+'%' ");
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
                sb.Append("and I.EMP_ID = @EMP_ID2 ");
                ht.Add("@EMP_ID2", SessionHandle.Current.emp_id);
            }
            if (license_id != "")
            {
                sb.Append(" and I.LICENSE_ID = @LICENSE_ID ");
                ht.Add("@LICENSE_ID", license_id);
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
    internal DataTable get_PDF_Data(string data_ym, string license_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select YEAR,EMP_ID,LICENSE_ID,EMP_NAME,isnull(JPN_CD,'')JPN_CD,DBO.FN_H_sConvert(REGISTER_ADDR,0) REGISTER_ADDR,PAYMENT_AMT,EXCEED_183,MUTUAL_SEQ");
            sb.Append(" from TB_S_R_MUTUAL_YEAR_DTL D ");
            sb.Append(" where YEAR = @DATA_YM ");
            sb.Append(" and D.LICENSE_ID = @LICENSE_ID");           
            
            ht.Add("@DATA_YM", data_ym);
            ht.Add("@LICENSE_ID", license_id);

            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    internal DataTable get_MUTUAL_Data()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select COMPANY_ID,TAX_ORG_ID,CATEGORY_INCOME ,OTHER_ITEM,UNIT_NAME,DBO.FN_H_sConvert(UNIT_ADDR,0) UNIT_ADDR,UNIT_MEN");
            sb.Append(" from TB_S_M_MUTUAL_TAX_STATEMENT D ");
          
            
            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    internal DataTable get_PDF_Data2(string data_ym,string salary_dt1, string salary_dt2, string license_id ,string sdt,string edt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select A.WK_COMPANY_CD, A.WK_IDENTITY_KIND, A.WK_EMP_ID, A.WK_LICENSE_ID, RTRIM(LTRIM(A.WK_INS_NAME)) WK_INS_NAME, A.WK_INS_TYPE, A.WK_INS_TOTAL,");
            sb.Append(" M.COMPANY_NAME, M.COMPANY_ADDR, M.CHAIRMAN_NAME ");
            sb.Append(" From ( /* 本人勞保健保*/");
            sb.Append(" Select M.COMPANY_CD As WK_COMPANY_CD,'本人' As WK_IDENTITY_KIND,V.EMP_ID As WK_EMP_ID, ");
            sb.Append(" V.LICENSE_ID As WK_LICENSE_ID,V.EMP_NAME As WK_INS_NAME,D.SUB_DESC As WK_INS_TYPE, ");
            sb.Append(" Sum(M.INS_TOTAL) As WK_INS_TOTAL ");
            sb.Append(" From TB_I_R_FEES_MONTH M ");
            sb.Append(" Join VW_H_EMP_DATA V On M.EMP_ID = V.EMP_ID ");
            sb.Append("  left join TB_9_M_COMM_D D on D.SYS_CD='IA' AND D.MAIN_CD ='INS_TYPE' and D.SUB_CD = M.INS_TYPE  ");
            sb.Append(" Where M.IDENTITY_KIND = '1' and M.INS_TYPE <> 'C'");
            sb.Append(" And M.IS_YN = 'Y' ");
            sb.Append(" And M.SALARY_DT Between @SALARY_DT1 And @SALARY_DT2 ");
            sb.Append(" And V.LICENSE_ID = @LICENSE_ID AND M.INS_TOTAL >0 AND M.COMPANY_CD='K' ");
            sb.Append(" Group By M.COMPANY_CD,M.IDENTITY_KIND, V.EMP_ID, V.LICENSE_ID, V.EMP_NAME, M.INS_TYPE,D.SUB_DESC ");
            sb.Append(" Union all /* 眷屬健保*/");
            sb.Append(" Select M.COMPANY_CD As WK_COMPANY_CD,T.SUB_DESC As WK_IDENTITY_KIND,M.EMP_ID As WK_EMP_ID, ");
            sb.Append(" F.FAMILY_LICENSE_ID As WK_LICENSE_ID,F.FAMILY_NAME As WK_INS_NAME,D.SUB_DESC As WK_INS_TYPE, ");
            sb.Append(" Sum(M.INS_TOTAL) As WK_INS_TOTAL ");
            sb.Append(" From TB_I_R_FEES_MONTH M ");
            sb.Append(" left Join TB_H_M_EMP_FAMILY F On M.EMP_ID = F.EMP_ID and M.LICENSE_ID=F.FAMILY_LICENSE_ID  ");
            sb.Append(" left Join VW_H_EMP_DATA V On M.EMP_ID = V.EMP_ID   ");
            sb.Append("  left join TB_9_M_COMM_D D on D.SYS_CD='IA' AND D.MAIN_CD ='INS_TYPE' and D.SUB_CD = M.INS_TYPE  ");
            sb.Append("  left join TB_9_M_COMM_D T on T.SYS_CD='HB' AND T.MAIN_CD ='FAMILY_RELATION' and T.SUB_CD = F.FAMILY_RELATION ");
            sb.Append(" Where M.IDENTITY_KIND = '2' ");
            sb.Append(" And M.IS_YN = 'Y' ");
            sb.Append(" And M.SALARY_DT Between @SALARY_DT1 And @SALARY_DT2 AND V.LICENSE_ID = @LICENSE_ID AND M.INS_TOTAL >0 AND M.COMPANY_CD='K' ");
            sb.Append(" Group By M.COMPANY_CD, T.SUB_DESC, M.EMP_ID, F.FAMILY_LICENSE_ID, F.FAMILY_NAME, M.INS_TYPE,D.SUB_DESC ");
            //sb.Append(" --NEW 團保 本人-- ");
            sb.Append(" Union all  /* 本人團保*/                                                                                                             ");
            sb.Append("  Select M.COMPANY_CD As WK_COMPANY_CD,'本人' As WK_IDENTITY_KIND,V.EMP_ID As WK_EMP_ID,  V.LICENSE_ID As WK_LICENSE_ID, ");
            sb.Append(" V.EMP_NAME As WK_INS_NAME,'團保' As WK_INS_TYPE,  Sum(M.GFEES_SELF) As WK_INS_TOTAL                                     ");
            sb.Append(" From TB_I_R_GROUP_MONTH M                                                                                               ");
            sb.Append(" Join VW_H_EMP_DATA V On M.EMP_ID = V.EMP_ID                                                                             ");
            sb.Append(" Where M.IDENTITY_KIND = '1'  And M.IS_YN = 'Y'  And M.SALARY_DT Between @SALARY_DT1 And @SALARY_DT2                   ");
            sb.Append(" And V.LICENSE_ID = @LICENSE_ID AND M.GFEES_SELF >0 AND M.COMPANY_CD='K'                                                ");
            sb.Append(" Group By M.COMPANY_CD,M.IDENTITY_KIND, V.EMP_ID, V.LICENSE_ID, V.EMP_NAME                                               ");
            //sb.Append("  --NEW 團保 眷屬--                                                                                                      ");
            sb.Append(" Union all    /* 團保 眷屬*/                                                                                                            ");
            sb.Append(" Select M.COMPANY_CD As WK_COMPANY_CD,T.SUB_DESC As WK_IDENTITY_KIND,M.EMP_ID As WK_EMP_ID,                              ");
            sb.Append(" F.FAMILY_LICENSE_ID As WK_LICENSE_ID,F.FAMILY_NAME As WK_INS_NAME,'團保' As WK_INS_TYPE,                                ");
            sb.Append(" Sum(M.GFEES_SELF) As WK_INS_TOTAL                                                                                       ");
            sb.Append(" From TB_I_R_GROUP_MONTH M                                                                                               ");
            sb.Append(" left Join TB_H_M_EMP_FAMILY F On M.EMP_ID = F.EMP_ID and M.LICENSE_ID=F.FAMILY_LICENSE_ID                               ");
            sb.Append(" left Join VW_H_EMP_DATA V On M.EMP_ID = V.EMP_ID                                                                        ");
            sb.Append(" left join TB_9_M_COMM_D T on T.SYS_CD='HB' AND T.MAIN_CD ='FAMILY_RELATION' and T.SUB_CD = F.FAMILY_RELATION            ");
            sb.Append(" Where M.IDENTITY_KIND = '2'  And M.IS_YN = 'Y'  And M.SALARY_DT Between  @SALARY_DT1 And @SALARY_DT2                   ");
            sb.Append(" AND V.LICENSE_ID = @LICENSE_ID AND M.GFEES_SELF >0 AND M.COMPANY_CD='K'                                                ");
            sb.Append(" Group By M.COMPANY_CD, T.SUB_DESC, M.EMP_ID, F.FAMILY_LICENSE_ID, F.FAMILY_NAME                                         ");
           // sb.Append(" -- 二代健保 本人 需有201401  201412 兩個參數                                                                            ");
            sb.Append("  Union all    /* 二代健保 本人*/                                                                                                           ");
            sb.Append("  Select V.COMPANY_CD As WK_COMPANY_CD,'本人' As WK_IDENTITY_KIND,V.EMP_ID As WK_EMP_ID,  V.LICENSE_ID As WK_LICENSE_ID, ");
            sb.Append(" V.EMP_NAME As WK_INS_NAME,'二代健保' As WK_INS_TYPE,  Sum(M.INS_COST) As WK_INS_TOTAL                                   ");
            sb.Append(" From TB_S_M_INS2_DETAIL M                                                                                               ");
            sb.Append(" Join VW_H_EMP_DATA V On M.EMP_ID = V.EMP_ID                                                                             ");
            sb.Append(" Where M.INS_COST_YM Between @sdt And @edt                                                                       ");
            sb.Append(" and M.INS_COST >0 AND V.LICENSE_ID = @LICENSE_ID                                                                       ");
            sb.Append(" Group By V.COMPANY_CD,V.EMP_ID, V.LICENSE_ID, V.EMP_NAME                                                                ");
            sb.Append(" ) A ");
            sb.Append(" Join TB_S_R_IMX_COMPANY M On A.WK_COMPANY_CD = M.COMPANY_CD And M.DATA_YM = @DATA_YM ");

            ht.Add("@SALARY_DT1",salary_dt1);
            ht.Add("@SALARY_DT2", salary_dt2);
            ht.Add("@DATA_YM", data_ym);
            ht.Add("@LICENSE_ID", license_id);
            ht.Add("@sdt", sdt);
            ht.Add("@edt", edt);

            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable getSendToEmail()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select CODE_VAL1 from TB_9_M_PARAMETER where SYS_CD ='SL' and MAIN_CD ='SEND_TO'  ");
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
    public DataTable getEmailContent()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select CODE_VAL1,REMARK from TB_9_M_PARAMETER where SYS_CD ='SK' and MAIN_CD ='MAIL_CONTENT'");
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
}