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
/// CFB2SC5100DAO 的摘要描述
/// </summary>
public class CFB2SC5100DAO : BaseDAO
{
    public string EMP_ID { get; set; }
    public string LICENSE_ID_FIRST { get; set; }

    public CFB2SC5100DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
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
    public string getSALARY_TYPE(string salary_type)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select SUB_DESC ");
            sb.AppendLine(" from TB_9_M_COMM_D ");
            sb.AppendLine(" where SYS_CD = 'SC' ");
            sb.AppendLine(" and MAIN_CD = 'SALARY_TYPE' ");
            sb.AppendLine(" and SUB_CD = @SUB_CD ");
            ht.Add("@SUB_CD", salary_type);
            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count == 1)
                return Convert.ToString(dt.Rows[0]["SUB_DESC"]);
            else
                return "";
        }
        catch
        {
            throw;
        }
    }
    public DataTable getSuperData_TypeIsA(string pay_id, string salary_type, string dept_no, string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select tty.* from (   ");
            sb.AppendLine(" select ISNULL(V.SALARY_ACCOUNT_NO,0) AS SALARY_ACCOUNT_NO, V.EMP_NAME, V.EMP_ID, ISNULL(V.JPN_CD,'') JPN_CD                 ");
            sb.AppendLine("        , V.SALARY_YM, CONVERT(varchar(100), P.PAY_DT , 111) as PAY_DT, V.COMPANY_CD, C.COMPANY_NAME                         ");
            sb.AppendLine("        , Sum(P.AMOUNT * P.IS_PLUS) AMOUNT, D1.SUB_DESC as JPN_NAME                                                          ");
            sb.AppendLine("        , V.SALARY_YM + ISNULL(CONVERT(varchar(8), P.PAY_DT , 112),'')                                                       ");
            sb.AppendLine("          + ISNULL(substring(V.SALARY_ACCOUNT_NO,1,3),'')                                                                    ");
            //sb.AppendLine("          + ISNULL(V.JPN_CD,0)+ISNULL(V.COMPANY_CD,0) as GROUP_ID                                                                ");
            sb.AppendLine("          + IIF(V.JPN_CD=1 and V.SALARY_ACCOUNT_BANK=050,'', ISNULL(V.JPN_CD,0))+ISNULL(V.COMPANY_CD,0) as GROUP_ID                                                                ");
            sb.AppendLine("            ,ISNULL(V.SALARY_ACCOUNT_BANK,0) AS SALARY_ACCOUNT_BANK          ");
            sb.AppendLine(@"     ,CASE WHEN JPN_CD = 2 THEN '1'
		                                                        WHEN JPN_CD = 1 and SALARY_ACCOUNT_BANK = '007' and SALARY_PAY_METHOD = 'T' THEN '2'
				                                                WHEN SALARY_ACCOUNT_BANK = '050' and SALARY_PAY_METHOD = 'T' THEN '3'
				                                                WHEN SALARY_ACCOUNT_BANK = '822' and SALARY_PAY_METHOD = 'T' THEN '4'
																WHEN SALARY_PAY_METHOD = 'C' THEN '5'
				                                                END as Flag                                                                                                 ");
            sb.AppendLine(" ,V.SALARY_PAY_METHOD                                                                                                ");
            sb.AppendLine(" from TB_S_M_SALARY_PAY_H H                                                                                                  ");
            sb.AppendLine(" left Join TB_S_M_EMP_RESULT V on  H.SALARY_DT = V.SALARY_DT and H.SALARY_YM = V.SALARY_YM                                   ");
            sb.AppendLine(" left join TB_S_S_SALARY_PAY P on  H.SALARY_DT = P.SALARY_DT  and H.SALARY_TYPE = P.SALARY_TYPE and H.PAY_KIND = P.PAY_KIND and V.EMP_ID=P.EMP_ID  ");
            sb.AppendLine(" left join TB_S_M_SALARY_ITEM I on P.SALARY_ID = I.SALARY_ID ");
            sb.AppendLine(" left join TB_9_M_COMM_D D1 on D1.SYS_CD ='HB' and D1.MAIN_CD ='JPN_CD' and D1.SUB_CD = V.JPN_CD                             ");
            sb.AppendLine(" left join TB_H_M_COMPANY C on C.COMPANY_CD = V.COMPANY_CD                                                                   ");
            sb.AppendLine(" where V.COMPANY_CD = 'K' and I.PAY_OBJECT = 'E' ");

            sb.AppendLine(" and H.PAY_ID = @PAY_ID ");
            sb.AppendLine(" and H.SALARY_TYPE = @SALARY_TYPE ");
            ht.Add("@PAY_ID", pay_id);
            ht.Add("@SALARY_TYPE", salary_type);
            if (dept_no != "")
            {
                sb.AppendLine(" and V.DEPT_NO = @DEPT_NO ");
                ht.Add("@DEPT_NO", dept_no);
            }
            if (emp_id != "")
            {
                sb.AppendLine(" and V.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
            }
            sb.AppendLine("    group By V.SALARY_ACCOUNT_NO, V.EMP_NAME, V.EMP_ID, V.JPN_CD,V.SALARY_ACCOUNT_BANK, V.SALARY_YM, P.PAY_DT, V.COMPANY_CD ,C.COMPANY_NAME, D1.SUB_DESC,V.SALARY_PAY_METHOD ");
          //  sb.AppendLine("    order By V.SALARY_YM,P.PAY_DT,V.COMPANY_CD, V.JPN_CD DESC, substring(V.SALARY_ACCOUNT_NO,1,3) ");
            sb.AppendLine(" ) tty where tty.AMOUNT >0 ");
            //sb.AppendLine(" order By tty.SALARY_YM,tty.PAY_DT,tty.COMPANY_CD,tty.JPN_CD DESC, substring(tty.SALARY_ACCOUNT_NO,1,3) ");//ori
           // sb.AppendLine("    order By  tty.SALARY_YM,tty.PAY_DT, tty.COMPANY_CD ,substring(tty.SALARY_ACCOUNT_NO,1,3), tty.JPN_CD DESC");//初版
            sb.AppendLine("    order By Flag,JPN_CD desc ");
            DataTable dt = dbConn.Query(sb, ht, true);
            return dt;
        }
        catch
        {
            throw;
        }
    }
    public DataTable getSuperData_TypeIsB(string pay_id, string salary_type, string dept_no, string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select ISNULL(V.SALARY_ACCOUNT_NO,0) AS SALARY_ACCOUNT_NO, V.EMP_NAME, V.EMP_ID, ISNULL(V.JPN_CD,'') JPN_CD                       ");
            sb.AppendLine("        , V.SALARY_YM, CONVERT(varchar(100), P.PAY_DT , 111) as PAY_DT, V.COMPANY_CD, C.COMPANY_NAME                              ");
            sb.AppendLine("        , Sum(P.AMOUNT * P.IS_PLUS) AMOUNT, D1.SUB_DESC as JPN_NAME                                                               ");
            sb.AppendLine("        , V.SALARY_YM + ISNULL(CONVERT(varchar(8), P.PAY_DT , 112),0)                                                             ");
            sb.AppendLine("          + ISNULL(substring(V.SALARY_ACCOUNT_NO,1,3),0)                                                                          ");
            //sb.AppendLine("          + ISNULL(V.JPN_CD,0)+ISNULL(V.COMPANY_CD,0) as GROUP_ID                                                                 ");
            //sb.AppendLine("          + ISNULL(V.JPN_CD,0)+ISNULL(V.COMPANY_CD,0) as GROUP_ID                                                                ");
            sb.AppendLine("          + IIF(V.JPN_CD=1 and V.SALARY_ACCOUNT_BANK=050,'', ISNULL(V.JPN_CD,0))+ISNULL(V.COMPANY_CD,0) as GROUP_ID                                                                ");
            sb.AppendLine("            ,ISNULL(V.SALARY_ACCOUNT_BANK,0) AS SALARY_ACCOUNT_BANK          ");
            sb.AppendLine(@"     ,CASE WHEN JPN_CD = 2 THEN '1'
		                                                        WHEN JPN_CD = 1 and SALARY_ACCOUNT_BANK = '007' and SALARY_PAY_METHOD = 'T' THEN '2'
				                                                WHEN SALARY_ACCOUNT_BANK = '050' and SALARY_PAY_METHOD = 'T' THEN '3'
				                                                WHEN SALARY_ACCOUNT_BANK = '822' and SALARY_PAY_METHOD = 'T' THEN '4'
																WHEN SALARY_PAY_METHOD = 'C' THEN '5'
				                                                END as Flag                                                                                                 ");
            sb.AppendLine("     ,V.SALARY_PAY_METHOD                                                                                                  ");
            sb.AppendLine("     from TB_S_M_SALARY_PAY_H H                                                                                                   ");
            sb.AppendLine("     left Join TB_S_M_EMP_RESULT_TMP V on H.SALARY_DT = V.SALARY_DT and H.SALARY_TYPE = V.SALARY_TYPE and H.PAY_KIND = V.PAY_KIND ");
            sb.AppendLine("     left join TB_S_S_SALARY_PAY P on  H.SALARY_DT = P.SALARY_DT and H.SALARY_TYPE = P.SALARY_TYPE and H.PAY_KIND = P.PAY_KIND and V.EMP_ID=P.EMP_ID    ");
            sb.AppendLine("     left join TB_9_M_COMM_D D1 on D1.SYS_CD ='HB' and D1.MAIN_CD ='JPN_CD' and D1.SUB_CD = V.JPN_CD                              ");
            sb.AppendLine("     left join TB_H_M_COMPANY C on C.COMPANY_CD = V.COMPANY_CD                                                                    ");
            sb.AppendLine("    where V.COMPANY_CD = 'K'                                                                                                      ");

            sb.AppendLine(" and H.PAY_ID = @PAY_ID ");
            sb.AppendLine(" and H.SALARY_TYPE = @SALARY_TYPE ");
            ht.Add("@PAY_ID", pay_id);
            ht.Add("@SALARY_TYPE", salary_type);
            if (dept_no != "")
            {
                sb.AppendLine(" and V.DEPT_NO = @DEPT_NO ");
                ht.Add("@DEPT_NO", dept_no);
            }
            if (emp_id != "")
            {
                sb.AppendLine(" and V.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
            }
            sb.AppendLine("    group By V.SALARY_ACCOUNT_NO, V.EMP_NAME, V.EMP_ID, V.JPN_CD,V.SALARY_ACCOUNT_BANK, V.SALARY_YM, P.PAY_DT, V.COMPANY_CD ,C.COMPANY_NAME, D1.SUB_DESC,V.SALARY_PAY_METHOD ");
            sb.AppendLine("    having Sum(P.AMOUNT * P.IS_PLUS) > 0 ");
            //sb.AppendLine("    order By V.SALARY_YM,P.PAY_DT,V.COMPANY_CD, V.JPN_CD DESC, substring(V.SALARY_ACCOUNT_NO,1,3) ");
            sb.AppendLine("    order By Flag,JPN_CD desc ");
            DataTable dt = dbConn.Query(sb, ht, true);
            return dt;
        }
        catch
        {
            throw;
        }
    }
    public DataTable getSuperData_TypeIsC(string pay_id, string salary_type, string dept_no, string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select  ISNULL(V.SALARY_ACCOUNT_NO,0) AS SALARY_ACCOUNT_NO, V.EMP_NAME, V.EMP_ID, ISNULL(V.JPN_CD,'') JPN_CD                     ");
            sb.AppendLine("         , V.SALARY_YM,CONVERT(varchar(100), P.PAY_DT , 111) as PAY_DT, V.COMPANY_CD, C.COMPANY_NAME, P.PAY_KIND                 ");
            sb.AppendLine("         , Sum(P.AMOUNT * P.IS_PLUS) AMOUNT, D1.SUB_DESC as JPN_NAME                                                             ");
            sb.AppendLine("        , ISNULL(P.PAY_KIND,0) + V.SALARY_YM + ISNULL(CONVERT(varchar(8), P.PAY_DT , 112),0)                                     ");
            sb.AppendLine("          + ISNULL(substring(V.SALARY_ACCOUNT_NO,1,3),0)                                                                         ");
            //sb.AppendLine("          + ISNULL(V.JPN_CD,0)+ISNULL(V.COMPANY_CD,0) as GROUP_ID                                                                ");
            sb.AppendLine("          + IIF(V.JPN_CD=1 and V.SALARY_ACCOUNT_BANK=050,'', ISNULL(V.JPN_CD,0))+ISNULL(V.COMPANY_CD,0) as GROUP_ID                                                                ");
            sb.AppendLine("            ,ISNULL(V.SALARY_ACCOUNT_BANK,0) AS SALARY_ACCOUNT_BANK          ");
            sb.AppendLine(@"     ,CASE WHEN JPN_CD = 2 THEN '1'
		                                                        WHEN JPN_CD = 1 and SALARY_ACCOUNT_BANK = '007' and SALARY_PAY_METHOD = 'T' THEN '2'
				                                                WHEN SALARY_ACCOUNT_BANK = '050' and SALARY_PAY_METHOD = 'T' THEN '3'
				                                                WHEN SALARY_ACCOUNT_BANK = '822' and SALARY_PAY_METHOD = 'T' THEN '4'
																WHEN SALARY_PAY_METHOD = 'C' THEN '5'
				                                                END as Flag                                                                                                  ");
            sb.AppendLine("     ,V.SALARY_PAY_METHOD                                                                                                  ");
            sb.AppendLine("     from TB_S_M_SALARY_PAY_H H                                                                                                  ");
            sb.AppendLine("     left Join TB_S_M_EMP_RESULT_TMP V on H.SALARY_DT = V.SALARY_DT and H.SALARY_TYPE = V.SALARY_TYPE and H.PAY_KIND = V.PAY_KIND ");
            sb.AppendLine("     left join TB_S_S_SALARY_PAY P on  H.SALARY_DT = P.SALARY_DT  and H.SALARY_TYPE = P.SALARY_TYPE and H.PAY_KIND = P.PAY_KIND and V.EMP_ID=P.EMP_ID  ");
            sb.AppendLine("     left join TB_9_M_COMM_D D1 on D1.SYS_CD ='HB' and D1.MAIN_CD ='JPN_CD' and D1.SUB_CD = V.JPN_CD                             ");
            sb.AppendLine("     left join TB_H_M_COMPANY C on C.COMPANY_CD = V.COMPANY_CD                                                                   ");
            sb.AppendLine("    where V.COMPANY_CD = 'K'                                                                                                     ");
            sb.AppendLine(" and H.PAY_ID = @PAY_ID ");
            sb.AppendLine(" and H.SALARY_TYPE = @SALARY_TYPE ");
            ht.Add("@PAY_ID", pay_id);
            ht.Add("@SALARY_TYPE", salary_type);
            if (dept_no != "")
            {
                sb.AppendLine(" and V.DEPT_NO = @DEPT_NO ");
                ht.Add("@DEPT_NO", dept_no);
            }
            if (emp_id != "")
            {
                sb.AppendLine(" and V.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
            }
            sb.AppendLine("    group By V.SALARY_ACCOUNT_NO, V.EMP_NAME, V.EMP_ID, V.JPN_CD,V.SALARY_ACCOUNT_BANK, V.SALARY_YM, P.PAY_DT, V.COMPANY_CD ,C.COMPANY_NAME, P.PAY_KIND, D1.SUB_DESC,V.SALARY_PAY_METHOD   ");
            sb.AppendLine("    having Sum(P.AMOUNT * P.IS_PLUS) > 0 ");
            //sb.AppendLine("    order By P.PAY_KIND, V.SALARY_YM, P.PAY_DT, V.COMPANY_CD, V.JPN_CD DESC, substring(V.SALARY_ACCOUNT_NO,1,3) ");//ori
            //sb.AppendLine("    order By P.PAY_KIND, V.SALARY_YM, P.PAY_DT, V.COMPANY_CD ,substring(V.SALARY_ACCOUNT_NO,1,3), V.JPN_CD DESC");
            sb.AppendLine("    order By Flag,JPN_CD desc ");
            DataTable dt = dbConn.Query(sb, ht, true);
            return dt;
        }
        catch
        {
            throw;
        }
    }
    public DataTable getSuperData_TypeIsD(string pay_id, string salary_type, string dept_no, string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select ISNULL(V.SALARY_ACCOUNT_NO,0) AS SALARY_ACCOUNT_NO, V.EMP_NAME, V.EMP_ID, ISNULL(V.JPN_CD,'') JPN_CD                      ");
            sb.AppendLine("        , V.SALARY_YM, CONVERT(varchar(100), P.PAY_DT , 111) as PAY_DT, V.COMPANY_CD, C.COMPANY_NAME, P.PAY_KIND                 ");
            sb.AppendLine("        , D.FESTIVAL_TYPE, sum(P.AMOUNT * P.IS_PLUS) AMOUNT, D1.SUB_DESC as JPN_NAME                                             ");
            sb.AppendLine("        , D.FESTIVAL_TYPE+V.SALARY_YM + ISNULL(CONVERT(varchar(8), P.PAY_DT , 112),0)                                            ");
            sb.AppendLine("          + ISNULL(substring(V.SALARY_ACCOUNT_NO,1,3),0)                                                                         ");
            sb.AppendLine("          + ISNULL(V.JPN_CD,0)+ISNULL(V.COMPANY_CD,0) as GROUP_ID                                                                ");
            sb.AppendLine("     from TB_S_M_SALARY_PAY_H H                                                                                                  ");
            sb.AppendLine("     left Join TB_S_M_EMP_RESULT_TMP V on H.SALARY_DT = V.SALARY_DT and H.SALARY_TYPE = V.SALARY_TYPE and H.PAY_KIND = V.PAY_KIND ");
            sb.AppendLine("     left join TB_S_S_SALARY_PAY P on  H.SALARY_DT = P.SALARY_DT  and H.SALARY_TYPE = P.SALARY_TYPE and H.PAY_KIND = P.PAY_KIND and V.EMP_ID=P.EMP_ID  ");
            sb.AppendLine("     left join TB_9_M_COMM_D D1 on D1.SYS_CD ='HB' and D1.MAIN_CD ='JPN_CD' and D1.SUB_CD = V.JPN_CD                             ");
            sb.AppendLine("     left join TB_H_M_COMPANY C on C.COMPANY_CD = V.COMPANY_CD                                                                       ");
            sb.AppendLine("    left join TB_S_R_FESTIVAL_D D                                                                                                ");
            sb.AppendLine("    on V.EMP_ID = D.EMP_ID and D.FESTIVAL_PAY_DT = P.SALARY_DT and P.PAY_KIND = '1035'                                           ");
            sb.AppendLine("    where V.COMPANY_CD = 'K'                                                                                                     ");

            sb.AppendLine(" and H.PAY_ID = @PAY_ID ");
            sb.AppendLine(" and H.SALARY_TYPE = @SALARY_TYPE ");
            ht.Add("@PAY_ID", pay_id);
            ht.Add("@SALARY_TYPE", salary_type);
            if (dept_no != "")
            {
                sb.AppendLine(" and V.DEPT_NO = @DEPT_NO ");
                ht.Add("@DEPT_NO", dept_no);
            }
            if (emp_id != "")
            {
                sb.AppendLine(" and V.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
            }
            sb.AppendLine(" group By V.SALARY_ACCOUNT_NO, V.EMP_NAME, V.EMP_ID, V.JPN_CD, V.SALARY_YM, P.PAY_DT, V.COMPANY_CD ,C.COMPANY_NAME, P.PAY_KIND,D.FESTIVAL_TYPE, D1.SUB_DESC ");
            sb.AppendLine("    having Sum(P.AMOUNT * P.IS_PLUS) > 0 ");
            sb.AppendLine(" order By V.SALARY_YM, P.PAY_DT, D.FESTIVAL_TYPE, V.COMPANY_CD, V.JPN_CD DESC,substring(V.SALARY_ACCOUNT_NO,1,3)                     ");
            DataTable dt = dbConn.Query(sb, ht, true);
            return dt;
        }
        catch
        {
            throw;
        }
    }
}
