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
/// CFB2SL4200DAO 的摘要描述
/// </summary>
public class CFB2SL4200DAO : BaseDAO
{
    public bool IsSuper;
    public CFB2SL4200DAO()
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
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression,string year, string dept_no, string ws_cd
                             , string emp_id, string license_id, string emp_status)
    {
        try
        {
            if (sortExpression.Contains("EMP_ID"))
                sortExpression = sortExpression.Replace("EMP_ID", "V.EMP_ID");
            if (sortExpression.Contains("EMP_NAME"))
                sortExpression = sortExpression.Replace("EMP_NAME", "V.EMP_NAME");
            if (sortExpression == "")
            {
                sortExpression = "V.PJOB_CD,V.EMP_ID";
            }
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" select * From");
            sb.AppendLine("         (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,               ");
            sb.AppendLine("                 V.DEPT_NO, V.DEPT_NO+'-'+ V.DIV_DEPT_FULL_NAME as DEPT, I.EMP_ID, V.EMP_NAME, V.EMP_STATUS ");
            sb.AppendLine("               , V.WS_CD +'-'+ D.SUB_DESC as WS_CD, V.LEVEL_CD, V.GRADE_CD, V.PJOB_CD+'-'+V.PJOB_DESC as PJOB, V.LICENSE_ID, V.PJOB_CD ");
            sb.AppendLine("  from TB_S_R_IMX_DTL I ");
            //sb.AppendLine("  join (select EMP_ID,LICENSE_ID, Count(*) Cnt                                                   ");
            //sb.AppendLine("          from TB_I_R_FEES_MONTH                                                                 ");
            //sb.AppendLine("         where IS_YN = 'Y'                                                                       ");
            //sb.AppendLine("           and SALARY_DT between @SALARY_DT_S and @SALARY_DT_E                                   ");
            //sb.AppendLine("         group by EMP_ID,LICENSE_ID) I                                                           ");
            //sb.AppendLine("    on I.EMP_ID = V.EMP_ID                                                                       ");
            sb.AppendLine("  left Join VW_H_EMP_DATA V on I.EMP_ID = V.EMP_ID ");
            sb.AppendLine("  left Join TB_9_M_COMM_D D on D.SYS_CD = 'HB' and D.MAIN_CD = 'WS_CD' and D.SUB_CD = V.WS_CD ");
            sb.AppendLine(" where I.DATA_YM = @DATA_YM and I.TAX_FORMAT = '50'  ");

            ht.Add("@DATA_YM", year);

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
                    sb.AppendLine(" and I.EMP_ID = @EMP_ID ");
                    ht.Add("@EMP_ID", emp_id);
                }
            }
            else
            {
                if (emp_id != "")
                {
                    sb.AppendLine(" and I.EMP_ID = @EMP_ID ");
                    ht.Add("@EMP_ID", emp_id);
                }
                sb.AppendLine(" and I.EMP_ID = @EMP_ID2 ");
                ht.Add("@EMP_ID2", SessionHandle.Current.emp_id);
            }
            if (license_id != "")
            {
                sb.AppendLine(" and V.LICENSE_ID = @LICENSE_ID ");
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
    public int getCount(int startRowIndex, int maximumRows, string year, string dept_no, string ws_cd
                             , string emp_id, string license_id, string emp_status)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" select count(*) total_record ");
            sb.AppendLine("  from TB_S_R_IMX_DTL I ");
            //sb.AppendLine("  join (select EMP_ID,LICENSE_ID, Count(*) Cnt                                                   ");
            //sb.AppendLine("          from TB_I_R_FEES_MONTH                                                                 ");
            //sb.AppendLine("         where IS_YN = 'Y'                                                                       ");
            //sb.AppendLine("           and SALARY_DT between @SALARY_DT_S and @SALARY_DT_E                                   ");
            //sb.AppendLine("         group by EMP_ID,LICENSE_ID) I                                                           ");
            //sb.AppendLine("    on I.EMP_ID = V.EMP_ID                                                                       ");
            sb.AppendLine("  left Join VW_H_EMP_DATA V on I.EMP_ID = V.EMP_ID ");
            sb.AppendLine("  left Join TB_9_M_COMM_D D on D.SYS_CD = 'HB' and D.MAIN_CD = 'WS_CD' and D.SUB_CD = V.WS_CD ");
            sb.AppendLine(" where I.DATA_YM = @DATA_YM and I.TAX_FORMAT = '50' ");

            ht.Add("@DATA_YM", year);

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
                    sb.AppendLine(" and I.EMP_ID = @EMP_ID ");
                    ht.Add("@EMP_ID", emp_id);
                }
            }
            else
            {
                if (emp_id != "")
                {
                    sb.AppendLine(" and I.EMP_ID = @EMP_ID ");
                    ht.Add("@EMP_ID", emp_id);
                }
                sb.AppendLine(" and I.EMP_ID = @EMP_ID2 ");
                ht.Add("@EMP_ID2", SessionHandle.Current.emp_id);
            }
            if (license_id != "")
            {
                sb.AppendLine(" and V.LICENSE_ID = @LICENSE_ID ");
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
    public DataTable getDtlHeader(string emp_id, string salary_dt_s, string salary_dt_e)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();

        sb.AppendLine(" select V.DEPT_NO,V.DIV_DEPT_FULL_NAME,V.EMP_ID,V.EMP_NAME,Sum(R.INS_TOTAL) As AMOUNT       ");
        sb.AppendLine("   from (select INS_TYPE, EMP_ID, SALARY_DT, Sum(INS_TOTAL) INS_TOTAL                       ");
        sb.AppendLine("           From TB_I_R_FEES_MONTH                                                           ");
        sb.AppendLine("          Where IS_YN = 'Y'                                                                 ");
        sb.AppendLine(" 		   And INS_TYPE In ('A', 'B')                                                      ");
        sb.AppendLine(" 		   And SALARY_YM Between left(REPLACE(@SALARY_DT_S,'/',''),6) And left(REPLACE(@SALARY_DT_E,'/',''),6)                             ");
        sb.AppendLine(" 		   And EMP_ID = @EMP_ID                                                            ");
        sb.AppendLine(" 		 Group By INS_TYPE, EMP_ID, SALARY_DT                                              ");
        sb.AppendLine("   Union all                                                                                   ");
        sb.AppendLine(" 		Select 'G' as INS_TYPE, EMP_ID, SALARY_DT, Sum(GFEES_SELF) INS_TOTAL               ");
        sb.AppendLine(" 		  From TB_I_R_GROUP_MONTH                                                          ");
        sb.AppendLine(" 		 Where IS_YN = 'Y'                                                                 ");
        sb.AppendLine(" 		   And SALARY_YM Between left(REPLACE(@SALARY_DT_S,'/',''),6) And left(REPLACE(@SALARY_DT_E,'/',''),6)                             ");
        sb.AppendLine(" 		   And EMP_ID = @EMP_ID                                                            ");
        sb.AppendLine(" 		 Group By EMP_ID, SALARY_DT                                                        ");
        sb.AppendLine("   Union all                                                                                   ");
        sb.AppendLine(" 		Select 'INS2' INS_TYPE, EMP_ID, PAYMENT_DATE as SALARY_DT, Sum(INS_COST) INS_TOTAL ");
        sb.AppendLine(" 		  From TB_S_M_INS2_DETAIL                                                          ");
        sb.AppendLine(" 		 Where PAYMENT_DATE Between @SALARY_DT_S And @SALARY_DT_E                          ");
        sb.AppendLine(" 		   And EMP_ID = @EMP_ID                                                            ");
        sb.AppendLine(" 		 Group By EMP_ID, PAYMENT_DATE                                                     ");
        //TERRY ADD 加 追溯保費
        sb.AppendLine(@" Union all
		                        Select 'TRACE' INS_TYPE, EMP_ID, SALARY_DT, SUM(isnull(case when TRACE_TYPE='A' then TRACE_AMT else TRACE_AMT*-1 end,0)) As INS_TOTAL 
 		                          From TB_I_M_FEES_TRACEBACK                                                          
 		                         Where SALARY_YM Between left(REPLACE(@SALARY_DT_S,'/',''),6) And left(REPLACE(@SALARY_DT_E,'/',''),6)                            
 		                           And EMP_ID = @EMP_ID  and trace_kind = 'A'    --20170426 JEAN 追加 TRACE_KIND='A'的條件 (只限個人的追溯才計入)                                                            
 		                         Group By EMP_ID, SALARY_DT  ");
        
        sb.AppendLine("    ) R                                                                                     ");
        sb.AppendLine("    Join VW_H_EMP_DATA V                                                                    ");
        sb.AppendLine("      On R.EMP_ID = V.EMP_ID                                                                ");
        sb.AppendLine("   Where R.EMP_ID = @EMP_ID                                  ");        
        sb.AppendLine("   Group By V.DEPT_NO,V.DIV_DEPT_FULL_NAME,V.EMP_ID,V.EMP_NAME                              ");

        ht.Add("@EMP_ID", emp_id);
        ht.Add("@SALARY_DT_S", salary_dt_s);
        ht.Add("@SALARY_DT_E", salary_dt_e);

        return dbConn.Query(sb, ht);
    }
    //查詢明細
    public DataTable getDtlData(int startRowIndex, int maximumRows, string sortExpression, string emp_id, string salary_dt_s, string salary_dt_e)
    {
        try
        {
            //20160428 TERRY add 加入追溯保費明細
            if (sortExpression.Contains("WK_SALARY_YM"))
                sortExpression = sortExpression.Replace("WK_SALARY_YM", "A.WK_SALARY_YM");
            if (sortExpression.Contains("WK_INS_TYPE"))
                sortExpression = sortExpression.Replace("WK_INS_TYPE", "A.WK_INS_TYPE");
            if (sortExpression.Contains("WK_IDENTITY_KIND"))
                sortExpression = sortExpression.Replace("WK_IDENTITY_KIND", "A.WK_IDENTITY_KIND");
            if (sortExpression.Contains("WK_LICENSE_ID"))
                sortExpression = sortExpression.Replace("WK_LICENSE_ID", "A.WK_LICENSE_ID");
            if (sortExpression.Contains("WK_INS_NAME"))
                sortExpression = sortExpression.Replace("WK_INS_NAME", "A.WK_INS_NAME");
            if (sortExpression == "")
            {
                sortExpression = "A.WK_SALARY_YM";
            }
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" select * From                                                                                                                      ");
            sb.AppendLine("         (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,                                                  ");
            sb.AppendLine("            A.WK_SALARY_YM,                                                                                                         ");
            sb.AppendLine(" CASE WHEN A.WK_INS_TYPE ='INS2' THEN 'INS2-補充保費'                                                                               ");
            sb.AppendLine("      WHEN A.WK_INS_TYPE ='G' THEN 'G-團保'                                                                                         ");
            sb.AppendLine("      ELSE A.WK_INS_TYPE +'-'+ D1.SUB_DESC END as WK_INS_TYPE,                                                                      ");
            sb.AppendLine(" A.WK_IDENTITY_KIND +'-'+ D2.SUB_DESC as WK_IDENTITY_KIND,                                                                          ");
            sb.AppendLine(" A.WK_LICENSE_ID,                                                                                                                   ");
            sb.AppendLine(" A.WK_INS_NAME,                                                                                                                     ");
            sb.AppendLine(" A.WK_SALARY_DT,                                                                                                                    ");
            sb.AppendLine(" A.WK_INS_TOTAL,                                                                                                                    ");
            sb.AppendLine(" A.WK_SALARY_YM + A.WK_INS_TYPE + A.WK_IDENTITY_KIND as dtldatakey                                                                  ");
            sb.AppendLine(" From (                                                                                                                             ");
            sb.AppendLine(" 	select                                                                                                                         ");
            sb.AppendLine(" 		M1.SALARY_YM As WK_SALARY_YM,                                                                                              ");
            sb.AppendLine(" 		M1.INS_TYPE As WK_INS_TYPE,                                                                                                ");
            sb.AppendLine(" 		M1.IDENTITY_KIND As WK_IDENTITY_KIND,                                                                                      ");
            sb.AppendLine(" 		M1.LICENSE_ID As WK_LICENSE_ID,                                                                                            ");
            sb.AppendLine(" 		V1.EMP_NAME As WK_INS_NAME,                                                                                                ");
            sb.AppendLine(" 		M1.SALARY_DT As WK_SALARY_DT,                                                                                              ");
            sb.AppendLine(" 	Sum(M1.INS_TOTAL) As WK_INS_TOTAL                                                                                              ");
            sb.AppendLine(" 	From TB_I_R_FEES_MONTH M1                                                                                                      ");
            sb.AppendLine(" 	Join VW_H_EMP_DATA V1 On M1.EMP_ID = V1.EMP_ID                                                                                 ");
            sb.AppendLine(" 	Where M1.IDENTITY_KIND = '1'                                                                                                   ");
            sb.AppendLine(" 	And M1.IS_YN = 'Y'                                                                                                             ");
            sb.AppendLine(" 	And M1.INS_TYPE In ('A', 'B')                                                                                                  ");
            //sb.AppendLine(" 	And M1.SALARY_DT Between @SALARY_DT_S And @SALARY_DT_E                                                                         ");
			sb.AppendLine(" 	And M1.SALARY_YM Between left(REPLACE(@SALARY_DT_S,'/',''),6) And left(REPLACE(@SALARY_DT_E,'/',''),6)                         ");
            sb.AppendLine(" 	And V1.EMP_ID = @EMP_ID                                                                                                        ");
            sb.AppendLine(" 	Group By M1.SALARY_YM, M1.INS_TYPE, M1.IDENTITY_KIND, M1.LICENSE_ID, V1.EMP_NAME, M1.SALARY_DT                                 ");
            //TERRY ADD
            sb.AppendLine(@" Union all 
	                            select a.SALARY_YM As WK_SALARY_YM,
	                            a.INS_TYPE As WK_INS_TYPE,
	                            a.IDENTITY_KIND As WK_IDENTITY_KIND, 
	                            a.LICENSE_ID As WK_LICENSE_ID,
	                            V1.EMP_NAME As WK_INS_NAME, 
	                            a.SALARY_DT As WK_SALARY_DT, 
	                            SUM(isnull(case when TRACE_TYPE='A' then a.TRACE_AMT else a.TRACE_AMT*-1 end,0)) As WK_INS_TOTAL
	                            from  TB_I_M_FEES_TRACEBACK a 
	                            Join VW_H_EMP_DATA V1 On a.EMP_ID = V1.EMP_ID    
	                            where a.IDENTITY_KIND ='1' AND a.emp_id=@EMP_ID  and (a.TRACE_KIND='A' /*or (a.TRACE_KIND='B' and a.INS_TYPE='C')*/) 
	                            And a.SALARY_YM Between left(REPLACE(@SALARY_DT_S,'/',''),6) And left(REPLACE(@SALARY_DT_E,'/',''),6)     
	                            group by a.SALARY_YM, a.INS_TYPE, a.IDENTITY_KIND, a.LICENSE_ID, V1.EMP_NAME, a.SALARY_DT ");
            //END
            // 20170427 JEAN 追加眷屬的追溯 START
            sb.AppendLine(@" 
                            Union all 
	                            select a.SALARY_YM As WK_SALARY_YM,
	                            a.INS_TYPE As WK_INS_TYPE,
	                            a.IDENTITY_KIND As WK_IDENTITY_KIND, 
	                            a.LICENSE_ID As WK_LICENSE_ID,
	                            /* 20170427 JEAN 眷屬以身份證字號看眷屬檔取姓名 刪除舊邏輯 START
	                            V1.EMP_NAME As WK_INS_NAME, 
						      20170427 JEAN 眷屬以身份證字號看眷屬檔取姓名 刪除舊邏輯 END*/
						   F1.FAMILY_NAME As WK_INS_NAME,
	                            a.SALARY_DT As WK_SALARY_DT, 
	                            SUM(isnull(case when TRACE_TYPE='A' then a.TRACE_AMT else a.TRACE_AMT*-1 end,0)) As WK_INS_TOTAL
	                            from  TB_I_M_FEES_TRACEBACK a 
	                            /* 20170427 JEAN 眷屬以身份證字號看眷屬檔取姓名 刪除舊邏輯 START
						   Join VW_H_EMP_DATA V1 On a.EMP_ID = V1.EMP_ID    
						      20170427 JEAN 眷屬以身份證字號看眷屬檔取姓名 刪除舊邏輯 END*/
	                            -- 20170427 JEAN 眷屬以身份證字號看眷屬檔取姓名 新邏輯 START
						   Join [dbo].[VW_H_M_EMP_FAMILY] F1 On a.LICENSE_ID = F1.FAMILY_LICENSE_ID  
	                            -- 20170427 JEAN 眷屬以身份證字號看眷屬檔取姓名 新邏輯 END 

	                            where a.IDENTITY_KIND ='2' AND a.emp_id=@EMP_ID 
	                            And a.SALARY_YM Between left(REPLACE(@SALARY_DT_S,'/',''),6) And left(REPLACE(@SALARY_DT_E,'/',''),6)     
	                            group by a.SALARY_YM, a.INS_TYPE, a.IDENTITY_KIND, a.LICENSE_ID, F1.FAMILY_NAME, a.SALARY_DT
                            ");

            //-- 20170427 JEAN 追加眷屬的追溯 END
            sb.AppendLine(" Union  all                                                                                                                            ");
            sb.AppendLine(" 	select                                                                                                                         ");
            sb.AppendLine(" 	    M2.SALARY_YM As WK_SALARY_YM,                                                                                              ");
            sb.AppendLine(" 	    M2.INS_TYPE As WK_INS_TYPE,                                                                                                ");
            sb.AppendLine(" 	    M2.IDENTITY_KIND As WK_IDENTITY_KIND,                                                                                      ");
            sb.AppendLine(" 	    F.FAMILY_LICENSE_ID As WK_LICENSE_ID,                                                                                      ");
            sb.AppendLine(" 	    F.FAMILY_NAME As WK_INS_NAME,                                                                                              ");
            sb.AppendLine(" 	    M2.SALARY_DT As WK_SALARY_DT,                                                                                              ");
            sb.AppendLine(" 	    Sum(M2.INS_TOTAL) As WK_INS_TOTAL                                                                                          ");
            sb.AppendLine(" 	From TB_I_R_FEES_MONTH M2                                                                                                      ");
            sb.AppendLine(" 	left Join TB_H_M_EMP_FAMILY F On M2.EMP_ID = F.EMP_ID and M2.LICENSE_ID=F.FAMILY_LICENSE_ID                                   ");
            sb.AppendLine(" 	Where M2.IDENTITY_KIND = '2'  And M2.IS_YN = 'Y'                                                                             ");
            sb.AppendLine(" 	And M2.INS_TYPE ='B'                                                                                                ");
            //sb.AppendLine(" 	And M2.SALARY_DT Between @SALARY_DT_S And @SALARY_DT_E                                                                         ");
			sb.AppendLine(" 	And M2.SALARY_YM Between left(REPLACE(@SALARY_DT_S,'/',''),6) And left(REPLACE(@SALARY_DT_E,'/',''),6)                         ");
            sb.AppendLine(" 	And M2.EMP_ID = @EMP_ID                                                                                                        ");
            sb.AppendLine(" 	Group By M2.SALARY_YM, M2.INS_TYPE, M2.IDENTITY_KIND, F.FAMILY_LICENSE_ID, F.FAMILY_NAME, M2.SALARY_DT                         ");
            sb.AppendLine(" Union  all                                                                                                                         ");
            sb.AppendLine(" 	select                                                                                                                         ");
            sb.AppendLine(" 	    M3.SALARY_YM As WK_SALARY_YM,                                                                                              ");
            sb.AppendLine(" 	    'G' As WK_INS_TYPE,                                                                                                        ");
            sb.AppendLine(" 	    M3.IDENTITY_KIND As WK_IDENTITY_KIND,                                                                                      ");
            sb.AppendLine(" 	    M3.LICENSE_ID As WK_LICENSE_ID,                                                                                            ");
            sb.AppendLine(" 	    V.EMP_NAME As WK_INS_NAME,                                                                                                 ");
            sb.AppendLine(" 	    M3.SALARY_DT As WK_SALARY_DT,                                                                                              ");
            sb.AppendLine(" 	    Sum(M3.GFEES_SELF) As WK_INS_TOTAL                                                                                         ");
            sb.AppendLine(" 	From TB_I_R_GROUP_MONTH M3                                                                                                     ");
            sb.AppendLine(" 	Join VW_H_EMP_DATA V On M3.EMP_ID = V.EMP_ID                                                                                   ");
            sb.AppendLine(" 	Where M3.IDENTITY_KIND = '1'                                                                                                   ");
            sb.AppendLine(" 	And M3.IS_YN = 'Y'                                                                                                             ");
            //sb.AppendLine(" 	And M3.SALARY_DT Between @SALARY_DT_S And @SALARY_DT_E                                                                         ");
			sb.AppendLine(" 	And M3.SALARY_YM Between left(REPLACE(@SALARY_DT_S,'/',''),6) And left(REPLACE(@SALARY_DT_E,'/',''),6)                         ");
            sb.AppendLine(" 	And V.EMP_ID = @EMP_ID                                                                                                         ");
            sb.AppendLine(" 	Group By M3.SALARY_YM, M3.IDENTITY_KIND, M3.LICENSE_ID, V.EMP_NAME, M3.SALARY_DT                                               ");
            sb.AppendLine(" Union  all                                                                                                                            ");
            sb.AppendLine(" 	select                                                                                                                         ");
            sb.AppendLine(" 	    M4.SALARY_YM As WK_SALARY_YM,                                                                                              ");
            sb.AppendLine(" 	    'G' As WK_INS_TYPE,                                                                                                        ");
            sb.AppendLine(" 	    M4.IDENTITY_KIND As WK_IDENTITY_KIND,                                                                                      ");
            sb.AppendLine(" 	    F.FAMILY_LICENSE_ID As WK_LICENSE_ID,                                                                                      ");
            sb.AppendLine(" 	    F.FAMILY_NAME As WK_INS_NAME,                                                                                              ");
            sb.AppendLine(" 	    M4.SALARY_DT As WK_SALARY_DT,                                                                                              ");
            sb.AppendLine(" 	    Sum(M4.GFEES_SELF) As WK_INS_TOTAL                                                                                         ");
            sb.AppendLine(" 	From TB_I_R_GROUP_MONTH M4                                                                                                     ");
            sb.AppendLine(" 	Join TB_H_M_EMP_FAMILY F On M4.EMP_ID = F.EMP_ID  and M4.LICENSE_ID=F.FAMILY_LICENSE_ID                                                                             ");
            sb.AppendLine(" 	Where M4.IDENTITY_KIND = '2'                                                                                                   ");
            sb.AppendLine(" 	And M4.IS_YN = 'Y'                                                                                                             ");
            //sb.AppendLine(" 	And M4.SALARY_DT Between @SALARY_DT_S And @SALARY_DT_E                                                                         ");
			sb.AppendLine(" 	And M4.SALARY_YM Between left(REPLACE(@SALARY_DT_S,'/',''),6) And left(REPLACE(@SALARY_DT_E,'/',''),6)                         ");
            sb.AppendLine(" 	And M4.EMP_ID = @EMP_ID                                                                                                        ");
            sb.AppendLine(" 	Group By M4.SALARY_YM, M4.IDENTITY_KIND, F.FAMILY_LICENSE_ID, F.FAMILY_NAME, M4.SALARY_DT                                      ");
            sb.AppendLine(" Union  all                                                                                                                    ");
            sb.AppendLine(" 	select                                                                                                                         ");
            sb.AppendLine(" 	    M5.INS_COST_YM As WK_SALARY_YM,                                                                                            ");
            sb.AppendLine(" 	    'INS2' As WK_INS_TYPE,                                                                                                     ");
            sb.AppendLine(" 	    '1' As WK_IDENTITY_KIND,                                                                                                   ");
            sb.AppendLine(" 	    M5.LICENSE_ID As WK_LICENSE_ID,                                                                                            ");
            sb.AppendLine(" 	    V.EMP_NAME As WK_INS_NAME,                                                                                                 ");
            sb.AppendLine(" 	    M5.PAYMENT_DATE As WK_SALARY_DT,                                                                                           ");
            sb.AppendLine(" 	    Sum(M5.INS_COST) As WK_INS_TOTAL                                                                                           ");
            sb.AppendLine(" 	From TB_S_M_INS2_DETAIL M5                                                                                                     ");
            sb.AppendLine(" 	Join VW_H_EMP_DATA V On M5.EMP_ID = V.EMP_ID                                                                                   ");
            sb.AppendLine(" 	Where M5.PAYMENT_DATE Between @SALARY_DT_S And @SALARY_DT_E                                                                    ");			
            sb.AppendLine(" 	And M5.EMP_ID = @EMP_ID                                                                                                        ");
            sb.AppendLine(" 	Group By M5.INS_COST_YM, M5.LICENSE_ID, V.EMP_NAME, M5.PAYMENT_DATE                                                            ");
            sb.AppendLine(" ) A                                                                                                                                ");
            sb.AppendLine(" left join TB_9_M_COMM_D D1 on D1.SYS_CD ='IA' and D1.MAIN_CD ='INS_TYPE' and D1.SUB_CD = A.WK_INS_TYPE                             ");
            sb.AppendLine(" left join TB_9_M_COMM_D D2 on D2.SYS_CD ='IA' and D2.MAIN_CD ='IDENTITY_KIND' and D2.SUB_CD = A.WK_IDENTITY_KIND                   ");                                    
            
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@SALARY_DT_S", salary_dt_s);
            ht.Add("@SALARY_DT_E", salary_dt_e);

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
    public int getDtlCount(int startRowIndex, int maximumRows, string emp_id, string salary_dt_s, string salary_dt_e)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select COUNT(*) total_record ");
            sb.AppendLine("     From (                                                                                                                         ");
            sb.AppendLine(" 	select                                                                                                                         ");
            sb.AppendLine(" 		M1.SALARY_YM As WK_SALARY_YM,                                                                                              ");
            sb.AppendLine(" 		M1.INS_TYPE As WK_INS_TYPE,                                                                                                ");
            sb.AppendLine(" 		M1.IDENTITY_KIND As WK_IDENTITY_KIND,                                                                                      ");
            sb.AppendLine(" 		M1.LICENSE_ID As WK_LICENSE_ID,                                                                                            ");
            sb.AppendLine(" 		V1.EMP_NAME As WK_INS_NAME,                                                                                                ");
            sb.AppendLine(" 		M1.SALARY_DT As WK_SALARY_DT,                                                                                              ");
            sb.AppendLine(" 	Sum(M1.INS_TOTAL) As WK_INS_TOTAL                                                                                              ");
            sb.AppendLine(" 	From TB_I_R_FEES_MONTH M1                                                                                                      ");
            sb.AppendLine(" 	Join VW_H_EMP_DATA V1 On M1.EMP_ID = V1.EMP_ID                                                                                 ");
            sb.AppendLine(" 	Where M1.IDENTITY_KIND = '1'                                                                                                   ");
            sb.AppendLine(" 	And M1.IS_YN = 'Y'                                                                                                             ");
            sb.AppendLine(" 	And M1.INS_TYPE In ('A', 'B')                                                                                                  ");
            //sb.AppendLine(" 	And M1.SALARY_DT Between @SALARY_DT_S And @SALARY_DT_E                                                                         ");
            sb.AppendLine(" 	And M1.SALARY_YM Between left(REPLACE(@SALARY_DT_S,'/',''),6) And left(REPLACE(@SALARY_DT_E,'/',''),6)                         ");
            sb.AppendLine(" 	And V1.EMP_ID = @EMP_ID                                                                                                        ");
            sb.AppendLine(" 	Group By M1.SALARY_YM, M1.INS_TYPE, M1.IDENTITY_KIND, M1.LICENSE_ID, V1.EMP_NAME, M1.SALARY_DT                                 ");
            
            //TERRY ADD
            sb.AppendLine(@" Union all 
	                            select a.SALARY_YM1 As WK_SALARY_YM,
	                            a.INS_TYPE As WK_INS_TYPE,
	                            a.IDENTITY_KIND As WK_IDENTITY_KIND, 
	                            a.LICENSE_ID As WK_LICENSE_ID,
	                            V1.EMP_NAME As WK_INS_NAME, 
	                            a.SALARY_DT As WK_SALARY_DT, 
	                            SUM(isnull(case when TRACE_TYPE='A' then a.TRACE_AMT else a.TRACE_AMT*-1 end,0)) As WK_INS_TOTAL
	                            from  TB_I_M_FEES_TRACEBACK a 
	                            Join VW_H_EMP_DATA V1 On a.EMP_ID = V1.EMP_ID    
	                            where a.IDENTITY_KIND ='1' AND a.emp_id=@EMP_ID  and (a.TRACE_KIND='A' or (a.TRACE_KIND='B' and a.INS_TYPE='C')) 
	                            And a.SALARY_YM1 Between left(REPLACE(@SALARY_DT_S,'/',''),6) And left(REPLACE(@SALARY_DT_E,'/',''),6)     
	                            group by a.SALARY_YM1, a.INS_TYPE, a.IDENTITY_KIND, a.LICENSE_ID, V1.EMP_NAME, a.SALARY_DT ");
            //END

            sb.AppendLine(" Union  all                                                                                                                            ");
            sb.AppendLine(" 	select                                                                                                                         ");
            sb.AppendLine(" 	    M2.SALARY_YM As WK_SALARY_YM,                                                                                              ");
            sb.AppendLine(" 	    M2.INS_TYPE As WK_INS_TYPE,                                                                                                ");
            sb.AppendLine(" 	    M2.IDENTITY_KIND As WK_IDENTITY_KIND,                                                                                      ");
            sb.AppendLine(" 	    F.FAMILY_LICENSE_ID As WK_LICENSE_ID,                                                                                      ");
            sb.AppendLine(" 	    F.FAMILY_NAME As WK_INS_NAME,                                                                                              ");
            sb.AppendLine(" 	    M2.SALARY_DT As WK_SALARY_DT,                                                                                              ");
            sb.AppendLine(" 	    Sum(M2.INS_TOTAL) As WK_INS_TOTAL                                                                                          ");
            sb.AppendLine(" 	From TB_I_R_FEES_MONTH M2                                                                                                      ");
            sb.AppendLine(" 	left Join TB_H_M_EMP_FAMILY F On M2.EMP_ID = F.EMP_ID and M2.LICENSE_ID=F.FAMILY_LICENSE_ID                                   ");
            sb.AppendLine(" 	Where M2.IDENTITY_KIND = '2'  And M2.IS_YN = 'Y'                                                                             ");
            sb.AppendLine(" 	And M2.INS_TYPE ='B'                                                                                                ");
            //sb.AppendLine(" 	And M2.SALARY_DT Between @SALARY_DT_S And @SALARY_DT_E                                                                         ");
            sb.AppendLine(" 	And M2.SALARY_YM Between left(REPLACE(@SALARY_DT_S,'/',''),6) And left(REPLACE(@SALARY_DT_E,'/',''),6)                         ");
            sb.AppendLine(" 	And M2.EMP_ID = @EMP_ID                                                                                                        ");
            sb.AppendLine(" 	Group By M2.SALARY_YM, M2.INS_TYPE, M2.IDENTITY_KIND, F.FAMILY_LICENSE_ID, F.FAMILY_NAME, M2.SALARY_DT                         ");
            sb.AppendLine(" Union  all                                                                                                                         ");
            sb.AppendLine(" 	select                                                                                                                         ");
            sb.AppendLine(" 	    M3.SALARY_YM As WK_SALARY_YM,                                                                                              ");
            sb.AppendLine(" 	    'G' As WK_INS_TYPE,                                                                                                        ");
            sb.AppendLine(" 	    M3.IDENTITY_KIND As WK_IDENTITY_KIND,                                                                                      ");
            sb.AppendLine(" 	    M3.LICENSE_ID As WK_LICENSE_ID,                                                                                            ");
            sb.AppendLine(" 	    V.EMP_NAME As WK_INS_NAME,                                                                                                 ");
            sb.AppendLine(" 	    M3.SALARY_DT As WK_SALARY_DT,                                                                                              ");
            sb.AppendLine(" 	    Sum(M3.GFEES_SELF) As WK_INS_TOTAL                                                                                         ");
            sb.AppendLine(" 	From TB_I_R_GROUP_MONTH M3                                                                                                     ");
            sb.AppendLine(" 	Join VW_H_EMP_DATA V On M3.EMP_ID = V.EMP_ID                                                                                   ");
            sb.AppendLine(" 	Where M3.IDENTITY_KIND = '1'                                                                                                   ");
            sb.AppendLine(" 	And M3.IS_YN = 'Y'                                                                                                             ");
            //sb.AppendLine(" 	And M3.SALARY_DT Between @SALARY_DT_S And @SALARY_DT_E                                                                         ");
            sb.AppendLine(" 	And M3.SALARY_YM Between left(REPLACE(@SALARY_DT_S,'/',''),6) And left(REPLACE(@SALARY_DT_E,'/',''),6)                         ");
            sb.AppendLine(" 	And V.EMP_ID = @EMP_ID                                                                                                         ");
            sb.AppendLine(" 	Group By M3.SALARY_YM, M3.IDENTITY_KIND, M3.LICENSE_ID, V.EMP_NAME, M3.SALARY_DT                                               ");
            sb.AppendLine(" Union  all                                                                                                                            ");
            sb.AppendLine(" 	select                                                                                                                         ");
            sb.AppendLine(" 	    M4.SALARY_YM As WK_SALARY_YM,                                                                                              ");
            sb.AppendLine(" 	    'G' As WK_INS_TYPE,                                                                                                        ");
            sb.AppendLine(" 	    M4.IDENTITY_KIND As WK_IDENTITY_KIND,                                                                                      ");
            sb.AppendLine(" 	    F.FAMILY_LICENSE_ID As WK_LICENSE_ID,                                                                                      ");
            sb.AppendLine(" 	    F.FAMILY_NAME As WK_INS_NAME,                                                                                              ");
            sb.AppendLine(" 	    M4.SALARY_DT As WK_SALARY_DT,                                                                                              ");
            sb.AppendLine(" 	    Sum(M4.GFEES_SELF) As WK_INS_TOTAL                                                                                         ");
            sb.AppendLine(" 	From TB_I_R_GROUP_MONTH M4                                                                                                     ");
            sb.AppendLine(" 	Join TB_H_M_EMP_FAMILY F On M4.EMP_ID = F.EMP_ID  and M4.LICENSE_ID=F.FAMILY_LICENSE_ID                                                                             ");
            sb.AppendLine(" 	Where M4.IDENTITY_KIND = '2'                                                                                                   ");
            sb.AppendLine(" 	And M4.IS_YN = 'Y'                                                                                                             ");
            //sb.AppendLine(" 	And M4.SALARY_DT Between @SALARY_DT_S And @SALARY_DT_E                                                                         ");
            sb.AppendLine(" 	And M4.SALARY_YM Between left(REPLACE(@SALARY_DT_S,'/',''),6) And left(REPLACE(@SALARY_DT_E,'/',''),6)                         ");
            sb.AppendLine(" 	And M4.EMP_ID = @EMP_ID                                                                                                        ");
            sb.AppendLine(" 	Group By M4.SALARY_YM, M4.IDENTITY_KIND, F.FAMILY_LICENSE_ID, F.FAMILY_NAME, M4.SALARY_DT                                      ");
            sb.AppendLine(" Union  all                                                                                                                    ");
            sb.AppendLine(" 	select                                                                                                                         ");
            sb.AppendLine(" 	    M5.INS_COST_YM As WK_SALARY_YM,                                                                                            ");
            sb.AppendLine(" 	    'INS2' As WK_INS_TYPE,                                                                                                     ");
            sb.AppendLine(" 	    '1' As WK_IDENTITY_KIND,                                                                                                   ");
            sb.AppendLine(" 	    M5.LICENSE_ID As WK_LICENSE_ID,                                                                                            ");
            sb.AppendLine(" 	    V.EMP_NAME As WK_INS_NAME,                                                                                                 ");
            sb.AppendLine(" 	    M5.PAYMENT_DATE As WK_SALARY_DT,                                                                                           ");
            sb.AppendLine(" 	    Sum(M5.INS_COST) As WK_INS_TOTAL                                                                                           ");
            sb.AppendLine(" 	From TB_S_M_INS2_DETAIL M5                                                                                                     ");
            sb.AppendLine(" 	Join VW_H_EMP_DATA V On M5.EMP_ID = V.EMP_ID                                                                                   ");
            sb.AppendLine(" 	Where M5.PAYMENT_DATE Between @SALARY_DT_S And @SALARY_DT_E                                                                    ");
            sb.AppendLine(" 	And M5.EMP_ID = @EMP_ID                                                                                                        ");
            sb.AppendLine(" 	Group By M5.INS_COST_YM, M5.LICENSE_ID, V.EMP_NAME, M5.PAYMENT_DATE                                                            ");
            sb.AppendLine(" ) A                                                                                                                                ");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@SALARY_DT_S", salary_dt_s);
            ht.Add("@SALARY_DT_E", salary_dt_e);
            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                t = Convert.ToInt32(dt.Rows[0]["total_record"]);
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