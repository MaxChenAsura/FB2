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
public class CFB2SL3100DAO : BaseDAO
{
    public bool IsSuper;

	public CFB2SL3100DAO()
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
                         string dept_no, string ws_cd, string emp_id, string license_id,string emp_status)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY PJOB_CD,I.EMP_ID ASC ) As RowNumber,");
            sb.Append(" concat(V.DEPT_NO,'-'+V.DIV_DEPT_FULL_NAME)as DEPT_NO,I.TAX_FORMAT,I.EMP_ID,I.EMP_NAME,V.EMP_STATUS,V.WS_CD,");
            sb.Append(" V.WS_CD+'-'+d.SUB_DESC as WS_CD_DESC,I.TAX_FORMAT +'-'+  e.SUB_DESC as TAX_FORMAT_DESC, ");
            sb.Append(" V.LEVEL_CD,V.GRADE_CD,concat(V.PJOB_CD,'-'+V.PJOB_DESC)as PJOB_CD,I.LICENSE_ID,V.SALARY_EMAIL");
            sb.Append(" from TB_S_R_IMX_DTL I ");
            sb.Append(" left join VW_H_EMP_DATA V On I.EMP_ID = V.EMP_ID");
            sb.Append(" left join TB_9_M_COMM_D d on d.SYS_CD = 'HB' and d.MAIN_CD = 'WS_CD' and V.WS_CD = d.SUB_CD ");
            sb.Append(" left join TB_9_M_COMM_D e on e.SYS_CD = 'SC' and e.MAIN_CD = 'TAX_FORMAT' and I.TAX_FORMAT = e.SUB_CD ");
            sb.Append(" where LEFT(I.DATA_YM,4) = @DATA_YM and I.COMPANY_CD = 'K'");
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
            if (ws_cd != "")
            {
                sb.Append(" and V.WS_CD = LEFT(@WS_CD,1) ");
                ht.Add("@WS_CD", ws_cd);
            }
            if (emp_status != "")
            {
                sb.Append(" and V.EMP_STATUS = @EMP_STATUS ");
                ht.Add("@EMP_STATUS", emp_status);
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
                         string dept_no, string ws_cd, string emp_id, string license_id, string emp_status)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select COUNT(*) total_record ");
            sb.Append(" from TB_S_R_IMX_DTL I ");
            sb.Append(" left join VW_H_EMP_DATA V On I.EMP_ID = V.EMP_ID ");
            sb.Append(" where LEFT(I.DATA_YM,4) = @DATA_YM and I.COMPANY_CD = 'K'");
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
            if (ws_cd != "")
            {
                sb.Append(" and V.WS_CD = @WS_CD ");
                ht.Add("@WS_CD", ws_cd);
            }
            if (emp_status != "")
            {
                sb.Append(" and V.EMP_STATUS = @EMP_STATUS ");
                ht.Add("@EMP_STATUS", emp_status);
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
    internal DataTable get_PDF_Data(string data_ym, string license_id,string tax_format)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select D.LICENSE_ID,D.TAX_FORMAT,D.TAX_SEQ,D.AMOUNT,D.TAX,(D.AMOUNT-D.TAX) INCOME,D.RETIRE_AMT,D.EMP_ID,D.EMP_NAME,");
            sb.Append(" DBO.FN_H_sConvert(D.REGISTER_ADDR,0)AS REGISTER_ADDR,D.PAY_YR,D.PAY_YM_START,D.PAY_YM_END,M.COMPANY_ID,M.TAX_ORG_ID,");
            sb.Append(" case when isnull(H.JPN_CD,'') = '' then '' else D.DUE_183 end DUE_183,");
            sb.Append(" isnull(D.COUNTRY_CD,'') COUNTRY_CD,isnull(D.TAX_DEAL_CD,'') TAX_DEAL_CD,M.COMPANY_NAME,M.COMPANY_ADDR,M.CHAIRMAN_NAME ");
            sb.Append(" from TB_S_R_IMX_DTL D ");
            sb.Append(" join TB_S_R_IMX_COMPANY M On LEFT(D.DATA_YM,4) = LEFT(M.DATA_YM,4) And D.COMPANY_CD = M.COMPANY_CD ");
            sb.Append(" left join TB_H_M_EMP H on D.EMP_ID = H.EMP_ID");
            sb.Append(" where LEFT(D.DATA_YM,4) = @DATA_YM ");
            sb.Append(" and D.LICENSE_ID = @LICENSE_ID");
            sb.Append(" and D.TAX_FORMAT = @TAX_FORMAT");
            sb.Append(" and D.COMPANY_CD = 'K'");
            
            ht.Add("@DATA_YM", data_ym);
            ht.Add("@LICENSE_ID", license_id);
            ht.Add("@TAX_FORMAT", tax_format);

            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    internal DataTable get_PDF_Data2(string data_ym,string salary_dt1, string salary_dt2, string license_id ,string sdt,string edt,string emp_id)
    {
        try
        {
            //20160427 TERRY更新 保險未計入 追溯部分
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select A.WK_COMPANY_CD, A.WK_IDENTITY_KIND, A.WK_EMP_ID, A.WK_LICENSE_ID, RTRIM(LTRIM(A.WK_INS_NAME)) WK_INS_NAME, A.WK_INS_TYPE, A.WK_INS_TOTAL,");
            sb.Append(" M.COMPANY_NAME, M.COMPANY_ADDR, M.CHAIRMAN_NAME ");
/* 201710  本人勞保健保 改由SL311匯入檔資料取得 */
            sb.Append(@"From (
                                       /* 本人勞保*/																
		                                select 'K' As WK_COMPANY_CD,'本人' As WK_IDENTITY_KIND,V.EMP_ID As WK_EMP_ID, a.LICENSE_ID As WK_LICENSE_ID,V.EMP_NAME As WK_INS_NAME,'勞保' As WK_INS_TYPE,a.AMOUNT	 as WK_INS_TOTAL														
				                                from TB_S_R_IMX_LABOR_UPLOAD a												
				                                --join VW_H_EMP_DATA V On a.LICENSE_ID = V.LICENSE_ID
                                                join (select EMP_NAME,EMP_ID,LICENSE_ID from VW_H_EMP_DATA where emp_id=@emp_id) V On a.LICENSE_ID = V.LICENSE_ID													
				                                where a.DATA_YEAR = @DATA_YM	AND a.LICENSE_ID = @LICENSE_ID  	 and a.AMOUNT > 0											
		                                Union all  														
                                /* 本人眷屬健保*/																
		                                select 'K' As WK_COMPANY_CD,														
				                                IIF(a.IDENTITY_KIND='1','本人',D.SUB_DESC) As WK_IDENTITY_KIND,V.EMP_ID As WK_EMP_ID, 												
				                                IIF(a.IDENTITY_KIND='1',a.LICENSE_ID,a.LICENSE_ID_B) As WK_LICENSE_ID,												
				                                IIF(a.IDENTITY_KIND='1',V.EMP_NAME,b.FAMILY_NAME )  As WK_INS_NAME,'健保' As WK_INS_TYPE,a.AMOUNT as WK_INS_TOTAL											
				                                from TB_S_R_IMX_HEALTH_UPLOAD a												
				                                --join VW_H_EMP_DATA V On a.LICENSE_ID = V.LICENSE_ID	
                                                join (select EMP_NAME,EMP_ID,LICENSE_ID from VW_H_EMP_DATA where emp_id=@emp_id ) V On a.LICENSE_ID = V.LICENSE_ID												
				                                left join TB_H_M_EMP_FAMILY b on a.LICENSE_ID_B = b.FAMILY_LICENSE_ID and V.EMP_ID = b.EMP_ID												
				                                left join TB_9_M_COMM_D D on D.SYS_CD='HB' AND D.MAIN_CD ='FAMILY_RELATION' and D.SUB_CD = b.FAMILY_RELATION 												
				                                where a.DATA_YEAR = @DATA_YM and a.LICENSE_ID = @LICENSE_ID and a.AMOUNT >0												
                                  ");

/* 201710以前舊抓法
            sb.Append(" From ( -- 本人勞保健保");
            sb.Append(" Select M.COMPANY_CD As WK_COMPANY_CD,'本人' As WK_IDENTITY_KIND,V.EMP_ID As WK_EMP_ID, ");
            sb.Append(" V.LICENSE_ID As WK_LICENSE_ID,V.EMP_NAME As WK_INS_NAME,D.SUB_DESC As WK_INS_TYPE, ");
            //new
            sb.Append(" Sum(M.INS_TOTAL + isnull((case when K.TRACE_TYPE='A' then K.TRACE_AMT else K.TRACE_AMT*-1 end) ,0) ) As WK_INS_TOTAL ");
            sb.Append(" From TB_I_R_FEES_MONTH M ");
            sb.Append(" Join VW_H_EMP_DATA V On M.EMP_ID = V.EMP_ID ");
            sb.Append("  left join TB_9_M_COMM_D D on D.SYS_CD='IA' AND D.MAIN_CD ='INS_TYPE' and D.SUB_CD = M.INS_TYPE  ");
            //new
            sb.Append(" left join TB_I_M_FEES_TRACEBACK K on M.EMP_ID = K.EMP_ID and M.INS_TYPE = K.INS_TYPE and M.SALARY_YM = K.SALARY_YM and M.IDENTITY_KIND = K.IDENTITY_KIND and M.LICENSE_ID =K.LICENSE_ID ");
            -- 20170427 JEAN 追溯不看單位(TRACE_KIND='B')的部份 只看個人(TRACE_KIND='A')
            sb.Append(" and K.IDENTITY_KIND ='1' and (K.TRACE_KIND='A'--or (K.TRACE_KIND='B' and K.INS_TYPE='C')  ) ");
            sb.Append(" Where M.IDENTITY_KIND = '1' and M.INS_TYPE <> 'C'");
            sb.Append(" And M.IS_YN = 'Y' ");
            //sb.Append(" And M.SALARY_DT Between @SALARY_DT1 And @SALARY_DT2 ");
			sb.Append(" And M.SALARY_YM Between left(REPLACE(@SALARY_DT1,'/',''),6) And left(REPLACE(@SALARY_DT2,'/',''),6)");
            --20170427 惠菁在月檔補上有追溯保費的金額為0  這樣才能JOIN到追溯檔
            sb.Append(" And V.LICENSE_ID = @LICENSE_ID  AND M.COMPANY_CD='K'  --AND M.INS_TOTAL >0");
            sb.Append(" Group By M.COMPANY_CD,M.IDENTITY_KIND, V.EMP_ID, V.LICENSE_ID, V.EMP_NAME, M.INS_TYPE,D.SUB_DESC ");
            sb.Append(" Union all   --眷屬健保");
            sb.Append(" Select M.COMPANY_CD As WK_COMPANY_CD,T.SUB_DESC As WK_IDENTITY_KIND,M.EMP_ID As WK_EMP_ID, ");
            sb.Append(" F.FAMILY_LICENSE_ID As WK_LICENSE_ID,F.FAMILY_NAME As WK_INS_NAME,D.SUB_DESC As WK_INS_TYPE, ");
            sb.Append(" Sum(M.INS_TOTAL + isnull((case when K.TRACE_TYPE='A' then K.TRACE_AMT else K.TRACE_AMT*-1 end) ,0) ) As WK_INS_TOTAL  ");
            sb.Append(" From TB_I_R_FEES_MONTH M ");
            sb.Append(" left Join TB_H_M_EMP_FAMILY F On M.EMP_ID = F.EMP_ID and M.LICENSE_ID=F.FAMILY_LICENSE_ID  ");
            sb.Append(" left Join VW_H_EMP_DATA V On M.EMP_ID = V.EMP_ID   ");
            sb.Append("  left join TB_9_M_COMM_D D on D.SYS_CD='IA' AND D.MAIN_CD ='INS_TYPE' and D.SUB_CD = M.INS_TYPE  ");
            sb.Append("  left join TB_9_M_COMM_D T on T.SYS_CD='HB' AND T.MAIN_CD ='FAMILY_RELATION' and T.SUB_CD = F.FAMILY_RELATION ");
            sb.Append(" left join TB_I_M_FEES_TRACEBACK K on M.EMP_ID = K.EMP_ID and M.INS_TYPE = K.INS_TYPE and M.SALARY_YM = K.SALARY_YM and M.IDENTITY_KIND = K.IDENTITY_KIND and F.FAMILY_LICENSE_ID =K.LICENSE_ID ");
            --20170427 JEAN 追溯不看單位(TRACE_KIND='B')的部份 只看個人(TRACE_KIND='A')
            sb.Append(" and K.IDENTITY_KIND ='2' and (K.TRACE_KIND='A' --or (K.TRACE_KIND='B' and K.INS_TYPE='B') ) ");
            sb.Append(" Where M.IDENTITY_KIND = '2' ");
            sb.Append(" And M.IS_YN = 'Y' ");
			sb.Append(" And M.SALARY_YM Between left(REPLACE(@SALARY_DT1,'/',''),6) And left(REPLACE(@SALARY_DT2,'/',''),6)");
			//sb.Append(" And M.SALARY_DT Between @SALARY_DT1 And @SALARY_DT2 ");
            --20170427 惠菁在月檔補上有追溯保費的金額為0  這樣才能JOIN到追溯檔
            sb.Append(" AND V.LICENSE_ID = @LICENSE_ID AND M.COMPANY_CD='K'  --AND M.INS_TOTAL >0 ");
            sb.Append(" Group By M.COMPANY_CD, T.SUB_DESC, M.EMP_ID, F.FAMILY_LICENSE_ID, F.FAMILY_NAME, M.INS_TYPE,D.SUB_DESC ");
*/
            //sb.Append(" --NEW 團保 本人-- ");
            sb.Append(" Union all  /* 本人團保*/                                                                                                             ");
            /* 20170427 JEAN 團保要含追溯金額 刪除原程式碼 */
            /*
            sb.Append("  Select M.COMPANY_CD As WK_COMPANY_CD,'本人' As WK_IDENTITY_KIND,V.EMP_ID As WK_EMP_ID,  V.LICENSE_ID As WK_LICENSE_ID, ");
            sb.Append(" V.EMP_NAME As WK_INS_NAME,'團保' As WK_INS_TYPE,  Sum(M.GFEES_SELF) As WK_INS_TOTAL                                     ");
            sb.Append(" From TB_I_R_GROUP_MONTH M                                                                                               ");
            sb.Append(" Join VW_H_EMP_DATA V On M.EMP_ID = V.EMP_ID                                                                             ");
            sb.Append(" Where M.IDENTITY_KIND = '1'  And M.IS_YN = 'Y' ");
			sb.Append(" And M.SALARY_YM Between left(REPLACE(@SALARY_DT1,'/',''),6) And left(REPLACE(@SALARY_DT2,'/',''),6)");
			//sb.Append(" And M.SALARY_DT Between @SALARY_DT1 And @SALARY_DT2 ");
            sb.Append(" And V.LICENSE_ID = @LICENSE_ID AND M.GFEES_SELF >0 AND M.COMPANY_CD='K'                                                ");
            sb.Append(" Group By M.COMPANY_CD,M.IDENTITY_KIND, V.EMP_ID, V.LICENSE_ID, V.EMP_NAME                                               ");
             */
            sb.Append(@"
                        SELECT M.COMPANY_CD AS WK_COMPANY_CD,
                               M.SUB_DESC AS WK_IDENTITY_KIND,
                               M.EMP_ID AS WK_EMP_ID,
                               M.LICENSE_ID AS WK_LICENSE_ID,
                               M.EMP_NAME AS WK_INS_NAME,
                               '團保' AS WK_INS_TYPE,
                               SUM(M.GFEES_SELF
		                     +ISNULL((CASE
                                                           WHEN K.TRACE_TYPE = 'A'
                                                           THEN K.TRACE_AMT
                                                           ELSE K.TRACE_AMT * -1
                                                       END), 0)
		                     ) AS WK_INS_TOTAL
		                      FROM 
                    (
                        SELECT M.COMPANY_CD,
                               '本人'  AS SUB_DESC,
                               V.EMP_ID,
                               V.LICENSE_ID,
                               V.EMP_NAME,
                               '團保' AS INS_TYPE,
                               SUM(M.GFEES_SELF) AS GFEES_SELF,
		                     M.SALARY_YM 
                        FROM TB_I_R_GROUP_MONTH M
                             JOIN VW_H_EMP_DATA V ON M.EMP_ID = V.EMP_ID
                        WHERE M.IDENTITY_KIND = '1'
                              AND M.IS_YN = 'Y'
                              AND M.SALARY_YM BETWEEN LEFT(REPLACE(@SALARY_DT1, '/', ''), 6) AND LEFT(REPLACE(@SALARY_DT2, '/', ''), 6)
                              AND V.LICENSE_ID = @LICENSE_ID
                              --AND M.GFEES_SELF > 0
                              AND M.COMPANY_CD = 'K'
                        GROUP BY M.COMPANY_CD,
                                 M.IDENTITY_KIND,
                                 V.EMP_ID,
                                 V.LICENSE_ID,
                                 V.EMP_NAME,
		                       M.SALARY_YM
                    ) M
                    LEFT JOIN TB_I_M_FEES_TRACEBACK K ON M.EMP_ID = K.EMP_ID
                                                                  AND K.INS_TYPE = 'D'
                                                                  AND M.SALARY_YM = K.SALARY_YM   --20170426 JEAN K.SALARY_YM1(薪資月份) → K.SALARY_YM(追補月份)
                                                                  --AND M.IDENTITY_KIND = K.IDENTITY_KIND
                                                                  AND M.LICENSE_ID = K.LICENSE_ID
                                                                  AND K.IDENTITY_KIND = '1'

                    GROUP BY M.COMPANY_CD,
                                 M.SUB_DESC,
                                 M.EMP_ID,
                                 M.LICENSE_ID,
                                 M.EMP_NAME
            ");
            
            //sb.Append("  --NEW 團保 眷屬--                                                                                                      ");
            sb.Append(" Union all    /* 團保 眷屬*/                                                                                                            ");
            //20170427 JEAN 眷屬團保要含追溯金額 新程式碼 START
            /*
            sb.Append(" Select M.COMPANY_CD As WK_COMPANY_CD,T.SUB_DESC As WK_IDENTITY_KIND,M.EMP_ID As WK_EMP_ID,                              ");
            sb.Append(" F.FAMILY_LICENSE_ID As WK_LICENSE_ID,F.FAMILY_NAME As WK_INS_NAME,'團保' As WK_INS_TYPE,                                ");
            sb.Append(" Sum(M.GFEES_SELF) As WK_INS_TOTAL                                                                                       ");
            sb.Append(" From TB_I_R_GROUP_MONTH M                                                                                               ");
            sb.Append(" left Join TB_H_M_EMP_FAMILY F On M.EMP_ID = F.EMP_ID and M.LICENSE_ID=F.FAMILY_LICENSE_ID                               ");
            sb.Append(" left Join VW_H_EMP_DATA V On M.EMP_ID = V.EMP_ID                                                                        ");
            sb.Append(" left join TB_9_M_COMM_D T on T.SYS_CD='HB' AND T.MAIN_CD ='FAMILY_RELATION' and T.SUB_CD = F.FAMILY_RELATION            ");
            sb.Append(" Where M.IDENTITY_KIND = '2'  And M.IS_YN = 'Y' ");
			sb.Append(" And M.SALARY_YM Between left(REPLACE(@SALARY_DT1,'/',''),6) And left(REPLACE(@SALARY_DT2,'/',''),6)");
			//sb.Append(" And M.SALARY_DT Between @SALARY_DT1 And @SALARY_DT2 ");
            sb.Append(" AND V.LICENSE_ID = @LICENSE_ID AND M.GFEES_SELF >0 AND M.COMPANY_CD='K'                                                ");
            sb.Append(" Group By M.COMPANY_CD, T.SUB_DESC, M.EMP_ID, F.FAMILY_LICENSE_ID, F.FAMILY_NAME                                         ");
            */
            sb.Append(@" 
                        select M.COMPANY_CD AS WK_COMPANY_CD,
                               M.SUB_DESC AS WK_IDENTITY_KIND,
                               M.EMP_ID AS WK_EMP_ID,
                               M.FAMILY_LICENSE_ID AS WK_LICENSE_ID,
                               M.FAMILY_NAME AS WK_INS_NAME,
                               '團保' AS WK_INS_TYPE,
                               SUM(M.GFEES_SELF
		                     +ISNULL((CASE
                                                           WHEN K.TRACE_TYPE = 'A'
                                                           THEN K.TRACE_AMT
                                                           ELSE K.TRACE_AMT * -1
                                                       END), 0)
		                     ) AS WK_INS_TOTAL
		                      from 
                    (
                        SELECT M.COMPANY_CD,
                               T.SUB_DESC,
                               M.EMP_ID,
                               F.FAMILY_LICENSE_ID,
                               F.FAMILY_NAME,
                               '團保' AS INS_TYPE,
                               SUM(M.GFEES_SELF) AS GFEES_SELF, 
		                     M.SALARY_YM 
                        FROM TB_I_R_GROUP_MONTH M
                             LEFT JOIN TB_H_M_EMP_FAMILY F ON M.EMP_ID = F.EMP_ID
                                                              AND M.LICENSE_ID = F.FAMILY_LICENSE_ID
                             LEFT JOIN VW_H_EMP_DATA V ON M.EMP_ID = V.EMP_ID
                             LEFT JOIN TB_9_M_COMM_D T ON T.SYS_CD = 'HB'
                                                          AND T.MAIN_CD = 'FAMILY_RELATION'
                                                          AND T.SUB_CD = F.FAMILY_RELATION
                        WHERE M.IDENTITY_KIND = '2'
                              AND M.IS_YN = 'Y'
                              AND M.SALARY_YM BETWEEN LEFT(REPLACE(@SALARY_DT1, '/', ''), 6) AND LEFT(REPLACE(@SALARY_DT2, '/', ''), 6)
                              AND V.LICENSE_ID = @LICENSE_ID
                              AND M.GFEES_SELF > 0
                              AND M.COMPANY_CD = 'K'
                        GROUP BY M.COMPANY_CD,
                                 T.SUB_DESC,
                                 M.EMP_ID,
                                 F.FAMILY_LICENSE_ID,
                                 F.FAMILY_NAME,
		                       M.SALARY_YM
		                       )
		                       M
		   
	                       LEFT JOIN TB_I_M_FEES_TRACEBACK K ON M.EMP_ID = K.EMP_ID
                                                                  AND K.INS_TYPE = 'D'
                                                                  AND M.SALARY_YM = K.SALARY_YM   --20170426 JEAN K.SALARY_YM1(薪資月份) → K.SALARY_YM(追補月份)
                                                                  --AND M.IDENTITY_KIND = K.IDENTITY_KIND
                                                                  AND M.FAMILY_LICENSE_ID = K.LICENSE_ID
                                                                  AND K.IDENTITY_KIND = '2'

                    GROUP BY M.COMPANY_CD,
                                 M.SUB_DESC,
                                 M.EMP_ID,
                                 M.FAMILY_LICENSE_ID,
                                 M.FAMILY_NAME
            ");


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
            sb.Append(" Join TB_S_R_IMX_COMPANY M On A.WK_COMPANY_CD = M.COMPANY_CD And M.DATA_YM = @DATA_YM   WHERE WK_INS_TOTAL > 0;  --20170427 JEAN 總額大於零才印出");

            ht.Add("@SALARY_DT1",salary_dt1);
            ht.Add("@SALARY_DT2", salary_dt2);
            ht.Add("@DATA_YM", data_ym);
            ht.Add("@LICENSE_ID", license_id);
            ht.Add("@sdt", sdt);
            ht.Add("@edt", edt);
            ht.Add("@emp_id", emp_id);
            
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
            sb.Append(" select CODE_VAL1,REMARK from TB_9_M_PARAMETER where SYS_CD ='SL' and MAIN_CD ='MAIL_CONTENT'");
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
}