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
/// CFB2SC5300BO 的摘要描述
/// </summary>
public class CFB2SC5300DAO : BaseDAO
{
    public CFB2SC5300DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public string RowNumber { get; set; }
    public string QDATAKEY { get; set; }
    public string SALARY_DT { get; set; }
    public string SALARY_TYPE { get; set; }
    public string DATA_YM { get; set; }
    public string EMP_ID { get; set; }
    public string DEPT_NO { get; set; }
    public string JPN_CD { get; set; }
    public string EMP_NAME { get; set; }
    public string EMP_CD { get; set; }
    public string SALARY_ID { get; set; }
    public string SALARY_NAME { get; set; }
    public string CHG_AMT_A { get; set; }
    public string CHG_AMT_B { get; set; }
    public string PROCESS_STATUS { get; set; }
    public string START_DT_B { get; set; }
    public string START_DT_A { get; set; }
    public string END_DATE_B { get; set; }
    public string END_DATE_A { get; set; }
    public string CHG_STATUS { get; set; }
    public string APPROVE_DT { get; set; }
    public string APPROVE_BY { get; set; }
    public string REMARK { get; set; }
    public string APP_REMARK { get; set; }
    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }
    public string day { get; set; }
    public string IS_PLUS { get; set; }
    public string IS_TAX { get; set; }

    public DataTable getJPN_CD()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select SYS_CD , MAIN_CD , SUB_CD ,SUB_DESC from TB_9_M_COMM_D");
            sb.Append(" where SYS_CD='HB' and MAIN_CD='JPN_CD' and IS_VALID = 'Y' ");
            return dbConn.Query(sb);

        }
        catch
        {
            throw;
        }
    }

    public DataTable ChkJP(string JPN_CD)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select DISTINCT p.DATA_YM,V.DEPT_NO_20,V.DEPT_NAME_20,v.DEPT_NO_40,V.DEPT_NAME_40,d.SUB_DESC");
            sb.Append(" From  TB_S_R_SALARY_REPORT_D P	");
            sb.Append(" Join 	VW_H_EMP_DATA V On V.EMP_ID = P.EMP_ID	");
            sb.Append(" Join 	TB_9_M_COMM_D d on SYS_CD = 'HB' And MAIN_CD = 'JPN_CD' And IS_VALID = 'Y' and d.SUB_CD=v.JPN_CD	");
            sb.Append(" where 1=1");
            if (JPN_CD != "-1" && JPN_CD != "99")
            {
                sb.Append(" and V.JPN_CD = @JPN_CD	");
                ht.Add("@JPN_CD", JPN_CD);
            }
            if (JPN_CD == "99")
            {
                sb.Append(" AND V.JPN_CD NOT IN (SELECT SUB_CD FROM TB_9_M_COMM_D WHERE SYS_CD = 'HB' AND MAIN_CD = 'JPN_CD' AND IS_VALID = 'Y')	");

            }
            return dbConn.Query(sb, ht);

        }
        catch
        {
            throw;
        }
    }

    public bool getProcess_Status()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select count(1) total from TB_S_M_SALARY_CAL_H ");
            sb.Append(" where SALARY_DT = @SALARY_DT and SALARY_TYPE = @SALARY_TYPE and PROCESS_STATUS <> '4' ");
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);

            DataTable dt = dbConn.Query(sb, ht);
            if (Convert.ToInt32(dt.Rows[0]["total"]) > 0)
                return true;
            else
                return false;
        }
        catch
        {
            throw;
        }
    }
    public DataTable searchResult_DEPT(string SALARY_DT, string SALARY_TYPE, string DEPT_NO, string EMP_ID, string JPN_CD, string CREATED_BY)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select DISTINCT P.DATA_YM,ISNULL(V.DEPT_NO_20,'')DEPT_NO_20,V.DEPT_NAME_20,ISNULL(V.DEPT_NO_40,'')DEPT_NO_40 ");
            sb.Append("  ,V.DEPT_NAME_40,ISNULL(V.JPN_CD,'')JPN_CD,isnull(d.SUB_DESC,'')as JPN_DESC ");
            sb.Append("  ,case when V.ACC_CD ='4' then '直接人員' else '間接人員' end as ACC_DESC");
            sb.Append("  ,case when V.ACC_CD ='4' then '4' else '1' end as ACC_CD");
            sb.Append(" from TB_S_R_SALARY_REPORT_D P	");
            sb.Append(" left Join VW_H_EMP_DATA V On V.EMP_ID = P.EMP_ID	");
            sb.Append(" left join TB_S_M_SALARY_CAL_H H on P.SALARY_DT = H.SALARY_DT and P.SALARY_TYPE = H.SALARY_TYPE   ");
            sb.Append(" left join TB_9_M_COMM_D d on d.MAIN_CD ='JPN_CD' and d.SYS_CD ='HB' and d.SUB_CD = V.JPN_CD  ");
            sb.Append(" where 1=1 and P.SALARY_DT = @SALARY_DT and P.SALARY_TYPE = @SALARY_TYPE and P.CREATED_BY='" + CREATED_BY + "'  ");
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            if (DEPT_NO != "")
            {
                sb.Append(" and V.DEPT_NO = @DEPT_NO	");
                ht.Add("@DEPT_NO", DEPT_NO);
            }
            if (EMP_ID != "")
            {
                sb.Append(" and V.EMP_ID = @EMP_ID	");
                ht.Add("@EMP_ID", EMP_ID);
            }
            if (JPN_CD != "-1" && JPN_CD != "99")
            {
                sb.Append(" and V.JPN_CD = @JPN_CD	");
                ht.Add("@JPN_CD", JPN_CD);
            }
            if (JPN_CD == "99")
            {
                sb.Append(" and V.JPN_CD not in (select SUB_CD from TB_9_M_COMM_D where SYS_CD = 'HB' AND MAIN_CD = 'JPN_CD' and IS_VALID = 'Y')	");
            }
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
    public DataTable searchResult_DEPTRow(string SALARY_DT, string SALARY_TYPE, string DEPT_NO, string EMP_ID, string JPN_CD, string CREATED_BY)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select DISTINCT P.DATA_YM,ISNULL(V.DEPT_NO_20,'')DEPT_NO_20,V.DEPT_NAME_20,ISNULL(V.DEPT_NO_40,'')DEPT_NO_40 ");
            sb.Append("  ,V.DEPT_NAME_40,ISNULL(V.JPN_CD,'')JPN_CD,isnull(d.SUB_DESC,'')as JPN_DESC ");
            sb.Append("  ,case when V.ACC_CD ='4' then '直接人員' else '間接人員' end as ACC_DESC");
            sb.Append("  ,case when V.ACC_CD ='4' then '4' else '1' end as ACC_CD");
            sb.Append(" from TB_S_R_SALARY_REPORT_D P	");
            sb.Append(" left Join VW_H_EMP_DATA V On V.EMP_ID = P.EMP_ID	");
            sb.Append(" left join TB_S_M_SALARY_CAL_H H on P.SALARY_DT = H.SALARY_DT and P.SALARY_TYPE = H.SALARY_TYPE   ");
            sb.Append(" left join TB_9_M_COMM_D d on d.MAIN_CD ='JPN_CD' and d.SYS_CD ='HB' and d.SUB_CD = V.JPN_CD  ");
            sb.Append(" where 1=1 and P.SALARY_DT = @SALARY_DT and P.SALARY_TYPE = @SALARY_TYPE and P.CREATED_BY='" + CREATED_BY + "'  ");
            sb.Append(" and ISNULL( V.DEPT_NO_20,'') = ''  ");
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            if (DEPT_NO != "")
            {
                sb.Append(" and V.DEPT_NO = @DEPT_NO	");
                ht.Add("@DEPT_NO", DEPT_NO);
            }
            if (EMP_ID != "")
            {
                sb.Append(" and V.EMP_ID = @EMP_ID	");
                ht.Add("@EMP_ID", EMP_ID);
            }
            if (JPN_CD != "-1" && JPN_CD != "99")
            {
                sb.Append(" and V.JPN_CD = @JPN_CD	");
                ht.Add("@JPN_CD", JPN_CD);
            }
            if (JPN_CD == "99")
            {
                sb.Append(" and V.JPN_CD not in (select SUB_CD from TB_9_M_COMM_D where SYS_CD = 'HB' AND MAIN_CD = 'JPN_CD' and IS_VALID = 'Y')	");
            }
            return dbConn.Query(sb, ht);
           
        }
        catch (Exception)
        {

            throw;
        }
    }
    public DataTable searchSubject(string SALARY_DT, string SALARY_TYPE,string CREATED_BY)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select '1' as a11");
            string index = "";
            for (int i = 1; i <= 48; i++)
            {
                if (i < 10)
                    index = "0" + i;
                else
                    index = i.ToString();
                sb.Append(" ,(SELECT SALARY_NAME FROM TB_S_R_SALARY_REPORT_H WHERE convert(varchar(10),SALARY_DT,111) = @SALARY_DT and SALARY_TYPE = @SALARY_TYPE and IS_PLUS = 1 AND SALARY_ID_SEQ = " + i + " and CREATED_BY='" + CREATED_BY + "') AS add" + index + "");
                sb.Append(" ,(SELECT SALARY_NAME FROM TB_S_R_SALARY_REPORT_H WHERE convert(varchar(10),SALARY_DT,111) = @SALARY_DT and SALARY_TYPE = @SALARY_TYPE and IS_PLUS = -1 AND SALARY_ID_SEQ = " + i + " and CREATED_BY='" + CREATED_BY + "') AS sub" + index + "");
            }
            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            ht.Add("@SALARY_DT", SALARY_DT);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
    public DataTable searchBy_EMP_ID(string SALARY_DT, string SALARY_TYPE, string DEPT_NO, string EMP_ID, string JPN_CD, string CREATED_BY, string tableName)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select P.EMP_ID,V.DEPT_NO_20,V.DEPT_NO_40,V.EMP_NAME,isnull(V.JPN_CD,'')JPN_CD ");
            sb.Append("  ,case when V.ACC_CD ='4' then '4' else '1' end as ACC_CD");

            string index = "";
            for (int i = 1; i <= 48; i++)
            {
                if (i < 10)
                    index = "0" + i;
                else
                    index = i.ToString();
                sb.Append(" ,ISNULL(sum(P.AMOUNT_A_" + index + "),0) as AMOUNT_A_" + index + " ");
                sb.Append(" ,ISNULL(sum(P.AMOUNT_D_" + index + "),0) as AMOUNT_D_" + index + "");
            }
            //加項合計
            sb.Append("   ,ISNULL((select sum(P2.AMOUNT) as AMOUNT From VW_H_EMP_DATA V ");
            sb.Append("     Join " + tableName + " P2 On V.EMP_ID = P2.EMP_ID	and P.SALARY_DT=P2.SALARY_DT and P.SALARY_TYPE = P2.SALARY_TYPE and IS_PLUS=1 ");
            sb.Append("     Join TB_S_M_SALARY_ITEM b on P2.SALARY_ID = b.SALARY_ID ");
            sb.Append("     where V.EMP_ID = P.EMP_ID and b.PAY_OBJECT = 'E' GROUP BY P2.EMP_ID),0)as ADD_SUM ");
            //扣項合計
            sb.Append("     ,ISNULL((select sum(P2.AMOUNT) as AMOUNT From VW_H_EMP_DATA V  ");
            sb.Append("   Join  " + tableName + "  P2 On V.EMP_ID = P2.EMP_ID and P.SALARY_DT=P2.SALARY_DT and P.SALARY_TYPE = P2.SALARY_TYPE and IS_PLUS=-1 ");
            sb.Append("   Join TB_S_M_SALARY_ITEM b on P2.SALARY_ID = b.SALARY_ID ");
            sb.Append("  where V.EMP_ID = P.EMP_ID and b.PAY_OBJECT = 'E' GROUP BY P2.EMP_ID),0) as SUB_SUM ");
            //實扣金額
            sb.Append("  ,ISNULL((select sum(P2.AMOUNT) as AMOUNT From VW_H_EMP_DATA V  ");
            sb.Append("   Join  " + tableName + "  P2 on V.EMP_ID = P2.EMP_ID and P.SALARY_DT=P2.SALARY_DT and P.SALARY_TYPE = P2.SALARY_TYPE and IS_PLUS=-1 ");
            sb.Append("   Join TB_S_M_SALARY_ITEM b on P2.SALARY_ID = b.SALARY_ID ");
            sb.Append("  where V.EMP_ID = P.EMP_ID and b.PAY_OBJECT = 'E' GROUP BY P2.EMP_ID),0) as REAL_SUB_SUM ");
            //積欠總額
            sb.Append("   ,ISNULL((select sum(P2.AMOUNT) as AMOUNT From VW_H_EMP_DATA V  ");
            sb.Append("    Join TB_S_M_STAFF_ARREARS_H P2 On V.EMP_ID = P2.EMP_ID	and IS_VAILD='Y' And AMOUNT <> TOTAL_AMT ");
            sb.Append("    where V.EMP_ID = P.EMP_ID GROUP BY P2.EMP_ID),0) as ARREARS_SUM ");
            //課稅合計
            sb.Append("   ,ISNULL((select sum(P2.AMOUNT*P2.IS_PLUS) as AMOUNT From VW_H_EMP_DATA V ");
            sb.Append("     Join  " + tableName + "  P2 On V.EMP_ID = P2.EMP_ID	and  P.SALARY_DT=P2.SALARY_DT and P.SALARY_TYPE = P2.SALARY_TYPE and  IS_TAX='Y' ");
            sb.Append("     Join TB_S_M_SALARY_ITEM b on P2.SALARY_ID = b.SALARY_ID ");
            sb.Append("    where V.EMP_ID = P.EMP_ID and b.PAY_OBJECT = 'E'  GROUP BY P2.EMP_ID),0) as TAXATION_SUM ");
            //所得總額
            sb.Append("   ,ISNULL((select sum(P2.AMOUNT*P2.IS_PLUS) as AMOUNT From VW_H_EMP_DATA V ");
            sb.Append("     Join  " + tableName + "  P2 On V.EMP_ID = P2.EMP_ID and  P.SALARY_DT=P2.SALARY_DT and P.SALARY_TYPE = P2.SALARY_TYPE ");
            sb.Append("     Join TB_S_M_SALARY_ITEM b on P2.SALARY_ID = b.SALARY_ID ");
            sb.Append("    where V.EMP_ID = P.EMP_ID and b.PAY_OBJECT = 'E' GROUP BY P2.EMP_ID),0) as TOTAL_SUM ");
            //所得代扣
            sb.Append("    ,ISNULL((select Sum(P2.AMOUNT) From VW_H_EMP_DATA V ");
            sb.Append("     Join  " + tableName + "   P2 On V.EMP_ID = P2.EMP_ID and  P.SALARY_DT=P2.SALARY_DT and P.SALARY_TYPE = P2.SALARY_TYPE ");
            sb.Append("     Join TB_S_M_SALARY_ITEM b on P2.SALARY_ID = b.SALARY_ID and b.SALARY_CD='4' and b.IS_TAX='Y' ");
            sb.Append("    where V.EMP_ID = P.EMP_ID and b.PAY_OBJECT = 'E' GROUP BY P2.EMP_ID),0) as TOTAL_TAX ");
            //實領總額
            sb.Append(" ,ISNULL((select sum(P2.AMOUNT*P2.IS_PLUS) as AMOUNT From VW_H_EMP_DATA V ");
            sb.Append("   Join  " + tableName + "  P2 On V.EMP_ID = P2.EMP_ID and  P.SALARY_DT=p2.SALARY_DT and P.SALARY_TYPE = P2.SALARY_TYPE ");
            sb.Append("   Join TB_S_M_SALARY_ITEM b on P2.SALARY_ID = b.SALARY_ID ");
            sb.Append("  where V.EMP_ID = P.EMP_ID and b.PAY_OBJECT = 'E' GROUP BY P2.EMP_ID),0) as REAL_SUM ");


            sb.Append(" From  TB_S_R_SALARY_REPORT_D P	");
            sb.Append(" left join VW_H_EMP_DATA V On V.EMP_ID = P.EMP_ID	");
            sb.Append(" where 1=1  and P.CREATED_BY='" + CREATED_BY + "' and P.SALARY_DT = @SALARY_DT and P.SALARY_TYPE = @SALARY_TYPE	");
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);

            if (DEPT_NO != "")
            {
                sb.Append(" and V.DEPT_NO = @DEPT_NO	");
                ht.Add("@DEPT_NO", DEPT_NO);
            }
            if (EMP_ID != "")
            {
                sb.Append(" and V.EMP_ID = @EMP_ID	");
                ht.Add("@EMP_ID", EMP_ID);
            }
            if (JPN_CD != "-1" && JPN_CD != "99")
            {
                sb.Append(" and V.JPN_CD = @JPN_CD	");
                ht.Add("@JPN_CD", JPN_CD);
            }
            if (JPN_CD == "99")
            {
                sb.Append(" AND V.JPN_CD NOT IN (SELECT SUB_CD FROM TB_9_M_COMM_D WHERE SYS_CD = 'HB' AND MAIN_CD = 'JPN_CD' AND IS_VALID = 'Y')	");
            }
            sb.Append(" GROUP BY P.EMP_ID,V.DEPT_NO_20,V.DEPT_NO_40,V.EMP_NAME,ACC_CD,JPN_CD,P.SALARY_DT,P.SALARY_TYPE ");
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
    public DataTable searchBy_Dept40(string SALARY_DT, string SALARY_TYPE, string DEPT_NO, string EMP_ID, string JPN_CD, string CREATED_BY, string tableName)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" select COUNT(*) EMP_COUNT,V.DEPT_NO_40 ");

            string index = "";
            for (int i = 1; i <= 48; i++)
            {
                if (i < 10)
                    index = "0" + i;
                else
                    index = i.ToString();
                sb.Append(" ,ISNULL(sum(P.AMOUNT_A_" + index + "),0) as AMOUNT_A_" + index + " ");
                sb.Append(" ,ISNULL(sum(P.AMOUNT_D_" + index + "),0) as AMOUNT_D_" + index + "");
            }
            //加項合計
            sb.Append("   ,ISNULL((select sum(P2.AMOUNT) as AMOUNT From VW_H_EMP_DATA V2 ");
            sb.Append("     Join " + tableName + " P2 On V2.EMP_ID = P2.EMP_ID	and P.SALARY_DT=P2.SALARY_DT and P.SALARY_TYPE = P2.SALARY_TYPE and IS_PLUS=1 ");
            sb.Append("     Join TB_S_M_SALARY_ITEM b on P2.SALARY_ID = b.SALARY_ID ");
            sb.Append("     where V.DEPT_NO_40 = V2.DEPT_NO_40 and b.PAY_OBJECT = 'E' GROUP BY V2.DEPT_NO_40),0)as ADD_SUM ");
            //扣項合計
            sb.Append("     ,ISNULL((select sum(P2.AMOUNT) as AMOUNT From VW_H_EMP_DATA V2  ");
            sb.Append("   Join  " + tableName + "  P2 On V2.EMP_ID = P2.EMP_ID and P.SALARY_DT=P2.SALARY_DT and P.SALARY_TYPE = P2.SALARY_TYPE and IS_PLUS=-1 ");
            sb.Append("   Join TB_S_M_SALARY_ITEM b on P2.SALARY_ID = b.SALARY_ID ");
            sb.Append("  where V.DEPT_NO_40 = V2.DEPT_NO_40 and b.PAY_OBJECT = 'E' GROUP BY V2.DEPT_NO_40),0) as SUB_SUM ");
            //實扣金額
            sb.Append("  ,ISNULL((select sum(P2.AMOUNT) as AMOUNT From VW_H_EMP_DATA V2  ");
            sb.Append("   Join  " + tableName + "  P2 on V2.EMP_ID = P2.EMP_ID and P.SALARY_DT=P2.SALARY_DT and P.SALARY_TYPE = P2.SALARY_TYPE and IS_PLUS=-1 ");
            sb.Append("   Join TB_S_M_SALARY_ITEM b on P2.SALARY_ID = b.SALARY_ID ");
            sb.Append("  where V.DEPT_NO_40 = V2.DEPT_NO_40 and b.PAY_OBJECT = 'E' GROUP BY V2.DEPT_NO_40),0) as REAL_SUB_SUM ");
            //積欠總額
            sb.Append("   ,ISNULL((select sum(P2.AMOUNT) as AMOUNT From VW_H_EMP_DATA V2  ");
            sb.Append("    Join TB_S_M_STAFF_ARREARS_H P2 On V2.EMP_ID = P2.EMP_ID	and IS_VAILD='Y' And AMOUNT <> TOTAL_AMT ");
            sb.Append("    where V.DEPT_NO_40 = V2.DEPT_NO_40 GROUP BY V2.DEPT_NO_40),0) as ARREARS_SUM ");
            //課稅合計
            sb.Append("   ,ISNULL((select sum(P2.AMOUNT*P2.IS_PLUS) as AMOUNT From VW_H_EMP_DATA V2 ");
            sb.Append("     Join  " + tableName + "  P2 On V2.EMP_ID = P2.EMP_ID	and  P.SALARY_DT=P2.SALARY_DT and P.SALARY_TYPE = P2.SALARY_TYPE and  IS_TAX='Y' ");
            sb.Append("     Join TB_S_M_SALARY_ITEM b on P2.SALARY_ID = b.SALARY_ID ");
            sb.Append("    where V.DEPT_NO_40 = V2.DEPT_NO_40 and b.PAY_OBJECT = 'E'  GROUP BY V2.DEPT_NO_40),0) as TAXATION_SUM ");
            //所得總額
            sb.Append("   ,ISNULL((select sum(P2.AMOUNT*P2.IS_PLUS) as AMOUNT From VW_H_EMP_DATA V2 ");
            sb.Append("     Join  " + tableName + "  P2 On V2.EMP_ID = P2.EMP_ID and  P.SALARY_DT=P2.SALARY_DT and P.SALARY_TYPE = P2.SALARY_TYPE ");
            sb.Append("     Join TB_S_M_SALARY_ITEM b on P2.SALARY_ID = b.SALARY_ID ");
            sb.Append("    where V.DEPT_NO_40 = V2.DEPT_NO_40 and b.PAY_OBJECT = 'E' GROUP BY V2.DEPT_NO_40),0) as TOTAL_SUM ");
            //所得代扣
            sb.Append("    ,ISNULL((select Sum(P2.AMOUNT) From VW_H_EMP_DATA V2 ");
            sb.Append("     Join  " + tableName + "   P2 On V2.EMP_ID = P2.EMP_ID and  P.SALARY_DT=P2.SALARY_DT and P.SALARY_TYPE = P2.SALARY_TYPE ");
            sb.Append("     Join TB_S_M_SALARY_ITEM b on P2.SALARY_ID = b.SALARY_ID and b.SALARY_CD='4' and b.IS_TAX='Y' ");
            sb.Append("    where V.DEPT_NO_40 = V2.DEPT_NO_40 and b.PAY_OBJECT = 'E' GROUP BY V2.DEPT_NO_40),0) as TOTAL_TAX ");
            //實領總額
            sb.Append(" ,ISNULL((select sum(P2.AMOUNT*P2.IS_PLUS) as AMOUNT From VW_H_EMP_DATA V2 ");
            sb.Append("   Join  " + tableName + "  P2 On V2.EMP_ID = P2.EMP_ID and  P.SALARY_DT=p2.SALARY_DT and P.SALARY_TYPE = P2.SALARY_TYPE ");
            sb.Append("   Join TB_S_M_SALARY_ITEM b on P2.SALARY_ID = b.SALARY_ID ");
            sb.Append("  where V.DEPT_NO_40 = V2.DEPT_NO_40 and b.PAY_OBJECT = 'E' GROUP BY V2.DEPT_NO_40),0) as REAL_SUM ");


            sb.Append(" From  TB_S_R_SALARY_REPORT_D P	");
            sb.Append(" left join VW_H_EMP_DATA V On V.EMP_ID = P.EMP_ID	");
            sb.Append(" where 1=1  and P.CREATED_BY='" + CREATED_BY + "' and P.SALARY_DT = @SALARY_DT and P.SALARY_TYPE = @SALARY_TYPE	");
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);

            if (DEPT_NO != "")
            {
                sb.Append(" and V.DEPT_NO = @DEPT_NO	");
                ht.Add("@DEPT_NO", DEPT_NO);
            }
            if (EMP_ID != "")
            {
                sb.Append(" and V.EMP_ID = @EMP_ID	");
                ht.Add("@EMP_ID", EMP_ID);
            }
            if (JPN_CD != "-1" && JPN_CD != "99")
            {
                sb.Append(" and V.JPN_CD = @JPN_CD	");
                ht.Add("@JPN_CD", JPN_CD);
            }
            if (JPN_CD == "99")
            {
                sb.Append(" AND V.JPN_CD NOT IN (SELECT SUB_CD FROM TB_9_M_COMM_D WHERE SYS_CD = 'HB' AND MAIN_CD = 'JPN_CD' AND IS_VALID = 'Y')	");
            }
            sb.Append(" GROUP BY V.DEPT_NO_40,P.SALARY_DT,P.SALARY_TYPE ");

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
    public DataTable searchBy_Dept20(string SALARY_DT, string SALARY_TYPE, string DEPT_NO, string EMP_ID, string JPN_CD, string CREATED_BY, string tableName)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" select COUNT(*) EMP_COUNT,V.DEPT_NO_20,V.DEPT_NAME_20 ");

            string index = "";
            for (int i = 1; i <= 48; i++)
            {
                if (i < 10)
                    index = "0" + i;
                else
                    index = i.ToString();
                sb.Append(" ,ISNULL(sum(P.AMOUNT_A_" + index + "),0) as AMOUNT_A_" + index + " ");
                sb.Append(" ,ISNULL(sum(P.AMOUNT_D_" + index + "),0) as AMOUNT_D_" + index + "");
            }
            //加項合計
            sb.Append("   ,ISNULL((select sum(P2.AMOUNT) as AMOUNT From VW_H_EMP_DATA V2 ");
            sb.Append("     Join " + tableName + " P2 On V2.EMP_ID = P2.EMP_ID	and P.SALARY_DT=P2.SALARY_DT and P.SALARY_TYPE = P2.SALARY_TYPE and IS_PLUS=1 ");
            sb.Append("     Join TB_S_M_SALARY_ITEM b on P2.SALARY_ID = b.SALARY_ID ");
            sb.Append("     where V.DEPT_NO_20 = V2.DEPT_NO_20 and b.PAY_OBJECT = 'E' GROUP BY V2.DEPT_NO_20),0)as ADD_SUM ");
            //扣項合計
            sb.Append("     ,ISNULL((select sum(P2.AMOUNT) as AMOUNT From VW_H_EMP_DATA V2  ");
            sb.Append("   Join  " + tableName + "  P2 On V2.EMP_ID = P2.EMP_ID and P.SALARY_DT=P2.SALARY_DT and P.SALARY_TYPE = P2.SALARY_TYPE and IS_PLUS=-1 ");
            sb.Append("   Join TB_S_M_SALARY_ITEM b on P2.SALARY_ID = b.SALARY_ID ");
            sb.Append("  where V.DEPT_NO_20 = V2.DEPT_NO_20 and b.PAY_OBJECT = 'E' GROUP BY V2.DEPT_NO_20),0) as SUB_SUM ");
            //實扣金額
            sb.Append("  ,ISNULL((select sum(P2.AMOUNT) as AMOUNT From VW_H_EMP_DATA V2  ");
            sb.Append("   Join  " + tableName + "  P2 on V2.EMP_ID = P2.EMP_ID and P.SALARY_DT=P2.SALARY_DT and P.SALARY_TYPE = P2.SALARY_TYPE and IS_PLUS=-1 ");
            sb.Append("   Join TB_S_M_SALARY_ITEM b on P2.SALARY_ID = b.SALARY_ID ");
            sb.Append("  where V.DEPT_NO_20 = V2.DEPT_NO_20 and b.PAY_OBJECT = 'E' GROUP BY V2.DEPT_NO_20),0) as REAL_SUB_SUM ");
            //積欠總額
            sb.Append("   ,ISNULL((select sum(P2.AMOUNT) as AMOUNT From VW_H_EMP_DATA V2  ");
            sb.Append("    Join TB_S_M_STAFF_ARREARS_H P2 On V2.EMP_ID = P2.EMP_ID	and IS_VAILD='Y' And AMOUNT <> TOTAL_AMT ");
            sb.Append("    where V.DEPT_NO_20 = V2.DEPT_NO_20 GROUP BY V2.DEPT_NO_20),0) as ARREARS_SUM ");
            //課稅合計
            sb.Append("   ,ISNULL((select sum(P2.AMOUNT*P2.IS_PLUS) as AMOUNT From VW_H_EMP_DATA V2 ");
            sb.Append("     Join  " + tableName + "  P2 On V2.EMP_ID = P2.EMP_ID	and  P.SALARY_DT=P2.SALARY_DT and P.SALARY_TYPE = P2.SALARY_TYPE and  IS_TAX='Y' ");
            sb.Append("     Join TB_S_M_SALARY_ITEM b on P2.SALARY_ID = b.SALARY_ID ");
            sb.Append("    where V.DEPT_NO_20 = V2.DEPT_NO_20 and b.PAY_OBJECT = 'E'  GROUP BY V2.DEPT_NO_20),0) as TAXATION_SUM ");
            //所得總額
            sb.Append("   ,ISNULL((select sum(P2.AMOUNT*P2.IS_PLUS) as AMOUNT From VW_H_EMP_DATA V2 ");
            sb.Append("     Join  " + tableName + "  P2 On V2.EMP_ID = P2.EMP_ID and  P.SALARY_DT=P2.SALARY_DT and P.SALARY_TYPE = P2.SALARY_TYPE ");
            sb.Append("     Join TB_S_M_SALARY_ITEM b on P2.SALARY_ID = b.SALARY_ID ");
            sb.Append("    where V.DEPT_NO_20 = V2.DEPT_NO_20 and b.PAY_OBJECT = 'E' GROUP BY V2.DEPT_NO_20),0) as TOTAL_SUM ");
            //所得代扣
            sb.Append("    ,ISNULL((select Sum(P2.AMOUNT) From VW_H_EMP_DATA V2 ");
            sb.Append("     Join  " + tableName + "   P2 On V2.EMP_ID = P2.EMP_ID and  P.SALARY_DT=P2.SALARY_DT and P.SALARY_TYPE = P2.SALARY_TYPE ");
            sb.Append("     Join TB_S_M_SALARY_ITEM b on P2.SALARY_ID = b.SALARY_ID and b.SALARY_CD='4' and b.IS_TAX='Y' ");
            sb.Append("    where V.DEPT_NO_20 = V2.DEPT_NO_20 and b.PAY_OBJECT = 'E' GROUP BY V2.DEPT_NO_20),0) as TOTAL_TAX ");
            //實領總額
            sb.Append(" ,ISNULL((select sum(P2.AMOUNT*P2.IS_PLUS) as AMOUNT From VW_H_EMP_DATA V2 ");
            sb.Append("   Join  " + tableName + "  P2 On V2.EMP_ID = P2.EMP_ID and  P.SALARY_DT=p2.SALARY_DT and P.SALARY_TYPE = P2.SALARY_TYPE ");
            sb.Append("   Join TB_S_M_SALARY_ITEM b on P2.SALARY_ID = b.SALARY_ID ");
            sb.Append("  where V.DEPT_NO_20 = V2.DEPT_NO_20 and b.PAY_OBJECT = 'E' GROUP BY V2.DEPT_NO_20),0) as REAL_SUM ");


            sb.Append(" From  TB_S_R_SALARY_REPORT_D P	");
            sb.Append(" left join VW_H_EMP_DATA V On V.EMP_ID = P.EMP_ID	");
            sb.Append(" where 1=1  and P.CREATED_BY='" + CREATED_BY + "' and P.SALARY_DT = @SALARY_DT and P.SALARY_TYPE = @SALARY_TYPE	");
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);

            if (DEPT_NO != "")
            {
                sb.Append(" and V.DEPT_NO = @DEPT_NO	");
                ht.Add("@DEPT_NO", DEPT_NO);
            }
            if (EMP_ID != "")
            {
                sb.Append(" and V.EMP_ID = @EMP_ID	");
                ht.Add("@EMP_ID", EMP_ID);
            }
            if (JPN_CD != "-1" && JPN_CD != "99")
            {
                sb.Append(" and V.JPN_CD = @JPN_CD	");
                ht.Add("@JPN_CD", JPN_CD);
            }
            if (JPN_CD == "99")
            {
                sb.Append(" AND V.JPN_CD NOT IN (SELECT SUB_CD FROM TB_9_M_COMM_D WHERE SYS_CD = 'HB' AND MAIN_CD = 'JPN_CD' AND IS_VALID = 'Y')	");
            }
            sb.Append(" GROUP BY V.DEPT_NO_20,P.SALARY_DT,P.SALARY_TYPE,V.DEPT_NAME_20 ");

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
    public DataTable searchBy_Company(string SALARY_DT, string SALARY_TYPE, string DEPT_NO, string EMP_ID, string JPN_CD, string CREATED_BY, string tableName)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();


            sb.Append("     select COUNT(*) EMP_COUNT ");
            string index = "";
            for (int i = 1; i <= 48; i++)
            {
                if (i < 10)
                    index = "0" + i;
                else
                    index = i.ToString();
                sb.Append(" ,ISNULL(sum(P.AMOUNT_A_" + index + "),0) as AMOUNT_A_" + index + " ");
                sb.Append(" ,ISNULL(sum(P.AMOUNT_D_" + index + "),0) as AMOUNT_D_" + index + "");
            }

            //加項合計
            sb.Append("   ,ISNULL((select sum(P2.AMOUNT) as AMOUNT From VW_H_EMP_DATA V2 ");
            sb.Append("     Join " + tableName + " P2 On V2.EMP_ID = P2.EMP_ID and P.SALARY_DT=P2.SALARY_DT and P.SALARY_TYPE = P2.SALARY_TYPE and IS_PLUS=1 ");
            sb.Append("     Join TB_S_M_SALARY_ITEM b on P2.SALARY_ID = b.SALARY_ID ");
            sb.Append("     where  b.PAY_OBJECT = 'E' ),0)as ADD_SUM ");
            //扣項合計
            sb.Append("   ,ISNULL((select sum(P2.AMOUNT) as AMOUNT From VW_H_EMP_DATA V2  ");
            sb.Append("   Join  " + tableName + "  P2 On V2.EMP_ID = P2.EMP_ID and P.SALARY_DT=P2.SALARY_DT and P.SALARY_TYPE = P2.SALARY_TYPE and IS_PLUS=-1 ");
            sb.Append("   Join TB_S_M_SALARY_ITEM b on P2.SALARY_ID = b.SALARY_ID ");
            sb.Append("  where  b.PAY_OBJECT = 'E' ),0) as SUB_SUM ");
            //實扣金額
            sb.Append("  ,ISNULL((select sum(P2.AMOUNT) as AMOUNT From VW_H_EMP_DATA V2  ");
            sb.Append("   Join  " + tableName + "  P2 on V2.EMP_ID = P2.EMP_ID and P.SALARY_DT=P2.SALARY_DT and P.SALARY_TYPE = P2.SALARY_TYPE and IS_PLUS=-1 ");
            sb.Append("   Join TB_S_M_SALARY_ITEM b on P2.SALARY_ID = b.SALARY_ID ");
            sb.Append("  where  b.PAY_OBJECT = 'E' ),0) as REAL_SUB_SUM ");
            //積欠總額
            sb.Append("   ,ISNULL((select sum(P2.AMOUNT) as AMOUNT From VW_H_EMP_DATA V2  ");
            sb.Append("    Join TB_S_M_STAFF_ARREARS_H P2 On V2.EMP_ID = P2.EMP_ID and IS_VAILD='Y' And AMOUNT <> TOTAL_AMT ");
            sb.Append("    ),0) as ARREARS_SUM ");
            //課稅合計
            sb.Append("   ,ISNULL((select sum(P2.AMOUNT*P2.IS_PLUS) as AMOUNT From VW_H_EMP_DATA V2 ");
            sb.Append("     Join  " + tableName + "  P2 On V2.EMP_ID = P2.EMP_ID	and  P.SALARY_DT=P2.SALARY_DT and P.SALARY_TYPE = P2.SALARY_TYPE and  IS_TAX='Y' ");
            sb.Append("     Join TB_S_M_SALARY_ITEM b on P2.SALARY_ID = b.SALARY_ID ");
            sb.Append("    where  b.PAY_OBJECT = 'E'  ),0) as TAXATION_SUM ");
            //所得總額
            sb.Append("   ,ISNULL((select sum(P2.AMOUNT*P2.IS_PLUS) as AMOUNT From VW_H_EMP_DATA V2 ");
            sb.Append("     Join  " + tableName + "  P2 On V2.EMP_ID = P2.EMP_ID and  P.SALARY_DT=P2.SALARY_DT and P.SALARY_TYPE = P2.SALARY_TYPE ");
            sb.Append("     Join TB_S_M_SALARY_ITEM b on P2.SALARY_ID = b.SALARY_ID ");
            sb.Append("    where  b.PAY_OBJECT = 'E' ),0) as TOTAL_SUM ");
            //所得代扣
            sb.Append("    ,ISNULL((select Sum(P2.AMOUNT) From VW_H_EMP_DATA V2 ");
            sb.Append("     Join  " + tableName + "  P2 On V2.EMP_ID = P2.EMP_ID and  P.SALARY_DT=P2.SALARY_DT and P.SALARY_TYPE = P2.SALARY_TYPE ");
            sb.Append("     Join TB_S_M_SALARY_ITEM b on P2.SALARY_ID = b.SALARY_ID and b.SALARY_CD='4' and b.IS_TAX='Y' ");
            sb.Append("    where  b.PAY_OBJECT = 'E' ),0) as TOTAL_TAX ");
            //實領總額
            sb.Append(" ,ISNULL((select sum(P2.AMOUNT*P2.IS_PLUS) as AMOUNT From VW_H_EMP_DATA V2 ");
            sb.Append("   Join  " + tableName + "  P2 On V2.EMP_ID = P2.EMP_ID and  P.SALARY_DT=p2.SALARY_DT and P.SALARY_TYPE = P2.SALARY_TYPE ");
            sb.Append("   Join TB_S_M_SALARY_ITEM b on P2.SALARY_ID = b.SALARY_ID ");
            sb.Append("  where  b.PAY_OBJECT = 'E' ),0) as REAL_SUM ");


            sb.Append(" From  TB_S_R_SALARY_REPORT_D P	");
            sb.Append(" left join VW_H_EMP_DATA V On V.EMP_ID = P.EMP_ID	");
            sb.Append(" where 1=1  and P.CREATED_BY='" + CREATED_BY + "' and P.SALARY_DT = @SALARY_DT and P.SALARY_TYPE = @SALARY_TYPE	");
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);

            sb.Append(" GROUP BY P.SALARY_DT,P.SALARY_TYPE ");

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
    internal void addData_H()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" INSERT INTO TB_S_R_SALARY_REPORT_H (");
            sb.Append(" P.SALARY_DT, P.SALARY_TYPE, DATA_YM, SALARY_ID_SEQ,");
            sb.Append(" SALARY_ID, SALARY_NAME, IS_PLUS, IS_TAX,");
            sb.Append(" CREATED_BY, CREATED_DT, UPDATED_BY, UPDATED_DT, FUNC_ID)");
            sb.Append(" SELECT	");
            sb.Append(" S.SALARY_DT, S.SALARY_TYPE, S.DATA_YM,	");
            sb.Append(" ROW_NUMBER() over(PARTITION BY S.IS_PLUS ORDER BY S.SALARY_DT, S.SALARY_TYPE, S.DATA_YM, S.IS_PLUS, S.SALARY_ID) SALARY_ID_SEQ,");
            sb.Append(" S.SALARY_ID, S.SALARY_NAME, S.IS_PLUS, S.IS_TAX,	");
            sb.Append(" " + CREATED_BY + " as CREATED_BY, GETDATE() as CREATED_DT, " + UPDATED_BY + " as UPDATED_BY, GETDATE() as UPDATED_DT, 'FB2SC530' as FUNC_ID	");
            sb.Append(" FROM(	");
            sb.Append(" SELECT  P.SALARY_DT, P.SALARY_TYPE, DATA_YM, SALARY_ID, SALARY_NAME, IS_PLUS, IS_TAX, COUNT(*) CNT	");
            sb.Append(" FROM TB_S_S_SALARY_PAY P 	");
            sb.Append(" LEFT JOIN VW_H_EMP_DATA v on  p.EMP_ID=v.EMP_ID	");
            sb.Append(" LEFT JOIN TB_S_M_SALARY_CAL_H H on P.SALARY_DT = H.SALARY_DT and P.SALARY_TYPE = H.SALARY_TYPE  and P.PAY_KIND =  H.PAY_KIND ");
            //sb.Append(" where SALARY_DT between '" + DATA_YM + "/1'  And '" + DATA_YM + "/" + day + "' And PAY_ID Is Not Null ");

            sb.Append(" where P.SALARY_TYPE = @SALARY_TYPE and convert(varchar(10),P.SALARY_DT,111) = @SALARY_DT ");
            if (DEPT_NO != "")
            {
                sb.Append(" and V.DEPT_NO = @DEPT_NO	");
                ht.Add("@DEPT_NO", DEPT_NO);
            }
            if (JPN_CD != "-1" && JPN_CD != "99")
            {
                sb.Append(" and v.JPN_CD = @JPN_CD	");
                ht.Add("@JPN_CD", JPN_CD);
            }
            if (JPN_CD == "99")
            {
                sb.Append(" AND v.JPN_CD NOT IN (SELECT SUB_CD FROM TB_9_M_COMM_D WHERE SYS_CD = 'HB' AND MAIN_CD = 'JPN_CD' AND IS_VALID = 'Y')	");

            }
            if (EMP_ID != "")
            {
                sb.Append(" and v.EMP_ID = @EMP_ID	");
                ht.Add("@EMP_ID", EMP_ID);
            }
            sb.Append(" GROUP BY P.SALARY_DT, P.SALARY_TYPE, DATA_YM, SALARY_ID, SALARY_NAME, IS_PLUS, IS_TAX) S");
            sb.Append(" ORDER BY S.SALARY_DT, S.SALARY_TYPE, S.DATA_YM, S.IS_PLUS desc,S.SALARY_ID, SALARY_ID_SEQ");
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);


            dbConn.ExecuteT(sb, ht, true);



        }
        catch (Exception)
        {
            throw;
        }
    }
    internal void addData_D()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" INSERT INTO TB_S_R_SALARY_REPORT_D (");
            sb.Append("         SALARY_DT, SALARY_TYPE, DATA_YM, EMP_ID");
            string index = "";
            for (int i = 1; i <= 48; i++)
            {
                if (i < 10)
                    index = "0" + i;
                else
                    index = i.ToString();
                sb.Append(" 			,AMOUNT_A_" + index + ",AMOUNT_D_" + index + "");
            }
            sb.Append("         ,CREATED_BY, CREATED_DT, UPDATED_BY, UPDATED_DT, FUNC_ID)");
            sb.Append("        SELECT SALARY_DT,SALARY_TYPE, DATA_YM, EMP_ID");
            for (int i = 1; i <= 48; i++)
            {
                if (i < 10)
                    index = "0" + i;
                else
                    index = i.ToString();
                sb.Append(" 			,isnull(sum(S.AMOUNT_A_" + index + "),0) as AMOUNT_A_" + index + "");
                sb.Append(" 			,isnull(sum(S.AMOUNT_D_" + index + "),0) as AMOUNT_D_" + index + "");
            }

            sb.Append(" 			 ,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID										 ");
            sb.Append(" 			from (");
            sb.Append("         SELECT P.SALARY_DT, P.SALARY_TYPE, P.DATA_YM, P.EMP_ID");
            for (int i = 1; i <= 48; i++)
            {
                if (i < 10)
                    index = "0" + i;
                else
                    index = i.ToString();
                sb.Append(" 	,(select SUM(P.AMOUNT) where H.IS_PLUS='1' AND H.SALARY_ID_SEQ = " + i + " ) as AMOUNT_A_" + index + "");
                sb.Append(" 	,(select SUM(P.AMOUNT) where H.IS_PLUS='-1' AND H.SALARY_ID_SEQ =" + i + " ) as AMOUNT_D_" + index + "");
            }
            sb.Append("           ,'" + CREATED_BY + "' as CREATED_BY, GETDATE() as CREATED_DT, '" + UPDATED_BY + "' as UPDATED_BY, GETDATE() as UPDATED_DT, 'FB2SC530' as FUNC_ID");
            sb.Append("         FROM TB_S_S_SALARY_PAY P");
            sb.Append(" left join VW_H_EMP_DATA V On V.EMP_ID = P.EMP_ID	");
            sb.Append(" left join TB_S_R_SALARY_REPORT_H H");
            sb.Append("         ON P.SALARY_DT = H.SALARY_DT AND P.SALARY_TYPE = H.SALARY_TYPE AND P.DATA_YM = H.DATA_YM AND P.SALARY_ID = H.SALARY_ID");
            sb.Append("         WHERE H.CREATED_BY = '" + CREATED_BY + "' and P.SALARY_DT = '" + SALARY_DT + "' and P.SALARY_TYPE = '" + SALARY_TYPE + "' ");
            sb.Append("         GROUP BY P.SALARY_DT, P.SALARY_TYPE, P.DATA_YM, P.EMP_ID, H.IS_PLUS, H.SALARY_ID_SEQ) S");
            sb.Append(" 		  GROUP BY SALARY_DT, SALARY_TYPE, DATA_YM, EMP_ID,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID");

            dbConn.ExecuteT(sb, ht, true);



        }
        catch (Exception)
        {
            throw;
        }
    }
    public string deleteData()
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();

        sb.Append(" Delete From TB_S_R_SALARY_REPORT_H  ");
        sb.Append(" Where SALARY_DT = @SALARY_DT and SALARY_TYPE = @SALARY_TYPE and CREATED_BY = @CREATED_BY");
        sb.Append(" Delete From TB_S_R_SALARY_REPORT_D  ");
        sb.Append(" Where SALARY_DT = @SALARY_DT and SALARY_TYPE = @SALARY_TYPE and CREATED_BY = @CREATED_BY");
        ht.Add("@SALARY_DT", SALARY_DT);
        ht.Add("@SALARY_TYPE", SALARY_TYPE);
        ht.Add("@CREATED_BY", CREATED_BY);
        dbConn.ExecuteT(sb, ht, true);

        return "0";
    }

}