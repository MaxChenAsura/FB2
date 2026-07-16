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
/// CFB2HA0300BO 的摘要描述
/// </summary>
public class CFB2SB2300DAO : BaseDAO
{
    public CFB2SB2300DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public string RowNumber { get; set; }
    public string QDATAKEY { get; set; }
    public string YEAR_MONTH { get; set; }
    public string INS_RATE_PERSON { get; set; }
    public string INS_RATE_COMP { get; set; }
    public string INS_MAX_MONTH { get; set; }
    public string INS_MIN_AMOUNT { get; set; }
    public string INS_MAX_AMOUNT { get; set; }
    public string CHG_AMT_A { get; set; }
    public string CHG_AMT_B { get; set; }
    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }
    //for查詢欄位
    public string ddl_SYS_ID { get; set; }

    public string SALARY_NAME { get; set; }
    public string SALARY_ID { get; set; }
    public string EMP_NAME { get; set; }
    public string EMP_ID { get; set; }
    public string EMP_CD { get; set; }
    public string SEQ_NO { get; set; }
    public string SEQ_NO_B { get; set; }

    public string DEPT_NO { get; set; }
    public string CHG_STATUS { get; set; }
    public string PROCESS_STATUS { get; set; }
    public string AMOUNT { get; set; }
    public string DATA_YM { get; set; }
    public string SALARY_STATUS { get; set; }
    public string REMARK { get; set; }
    public string APP_REMARK { get; set; }
    public string OP_MSG { get; set; }

    public DataTable getSYS_ID(string SUB_CD)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select SUB_CD ,SUB_DESC from TB_9_M_COMM_D where 1=1 and sys_cd = '99' and main_cd = 'sys_id' and is_valid ='Y' and SUB_CD = @SUB_CD order by SUB_CD");
            ht.Add("@SUB_CD", SUB_CD);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getFUNC_ID(string ID)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" SELECT        *");
            sb.Append(" FROM            TB_9_M_SYS_M");
            sb.Append(" WHERE SYS_ID+MODE_ID = @ID");
            ht.Add("@ID", ID);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public System.Data.DataTable getSYS_ID()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select SUB_CD ,SUB_DESC from TB_9_M_COMM_D where 1=1 and sys_cd = '99' and main_cd = 'sys_id' and is_valid ='Y' order by SUB_CD");
            return dbConn.Query(sb);

        }
        catch (Exception)
        {

            throw;
        }
    }
    public DataTable getDefaultData1(string EMP_ID, string SALARY_ID, string DATA_YM, string SEQ_NO)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select t1.DATA_YM as DATA_YM,t1.EMP_ID as EMP_ID ,t.EMP_NAME as EMP_NAME ,t.EMP_CD as EMP_CD,t.EMP_CD +'-'+ d.SUB_DESC as DESC1,t1.SALARY_ID  as SALARY_ID");
            sb.Append(" ,t1.SALARY_ID +'-'+ s.SALARY_NAME as SALARY_NAME ,t1.AMOUNT as  CHG_AMT_B,t1.SALARY_STATUS as  SALARY_STATUS,t1.SALARY_STATUS +'-'+ p.SUB_DESC as DESC4");
            sb.Append(" ,t1.SALARY_DT as SALARY_DT,'' as CHG_AMT_B ,'Y' as PROCESS_STATUS ,'Y-已生效'as DESC2,'' as CHG_STATUS , ''as DESC3");
            sb.Append(" ,'' as APPROVE_DT ,'' as APPROVE_BY ,t1.REMARK as REMARK ,'' as APP_REMARK,t1.SEQ_NO as SEQ_NO , ct.EMP_ID + '-' + ct.EMP_NAME as CREATED_NAME	");
            sb.Append(" ,t1.CREATED_BY, t1.CREATED_BY + CASE WHEN ISNULL(ct.EMP_NAME,'') = '' THEN '' ELSE '-' + ct.EMP_NAME END AS CREATE_NAME");
            sb.Append(" ,'' as APPROVE_NAME ");
            sb.Append(" ,dept.DEPT_NO , dept.DEPT_NAME ");
            sb.Append(" from TB_S_M_SUBSIDY_DEDUCTIONS_1 t1");
            sb.Append(" left join TB_H_M_EMP t  on t1.EMP_ID = t.EMP_ID");
            sb.Append(" left join TB_S_M_SALARY_ITEM s on   t1.SALARY_ID = s.SALARY_ID");
            sb.Append(" left join TB_9_M_COMM_D d on  d.SYS_CD ='HB' and  d.MAIN_CD='EMP_CD' and  t.EMP_CD = d.SUB_CD");
            sb.Append(" left join TB_9_M_COMM_D p on  p.SYS_CD ='SB' and  p.MAIN_CD='SALARY_STATUS' and  t1.SALARY_STATUS = p.SUB_CD");
            sb.Append(" left join TB_H_M_EMP ct  on t1.CREATED_BY = ct.EMP_ID");
            sb.Append(" left join TB_H_M_DEPT dept on t.DEPT_NO = dept.DEPT_NO");
            sb.Append(" where 1=1 AND t1.EMP_ID = @EMP_ID AND t1.SALARY_ID = @SALARY_ID AND t1.DATA_YM = @DATA_YM AND t1.SEQ_NO = @SEQ_NO");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SALARY_ID", SALARY_ID);
            ht.Add("@DATA_YM", DATA_YM);
            ht.Add("@SEQ_NO", SEQ_NO);
            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }
    public DataTable getDefaultData2(string EMP_ID, string SALARY_ID, string DATA_YM, string SEQ_NO)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select t2.DATA_YM as DATA_YM,t2.EMP_ID as EMP_ID  ,t.EMP_NAME as EMP_NAME ,t.EMP_CD as EMP_CD,t.EMP_CD +'-'+ d.SUB_DESC as DESC1,t2.SALARY_ID as SALARY_ID");
            sb.Append(" ,t2.SALARY_ID +'-'+ s.SALARY_NAME as SALARY_NAME,t2.CHG_AMT_A as CHG_AMT_A ,'N' as  SALARY_STATUS,'N-未處理' as DESC4");
            sb.Append(" ,'' as SALARY_DT,convert(varchar(10),t2.CHG_AMT_B) as CHG_AMT_B ,t2.PROCESS_STATUS as PROCESS_STATUS");
            sb.Append(" ,t2.PROCESS_STATUS+'-'+ h.SUB_DESC as DESC2,t2.CHG_STATUS as CHG_STATUS ,t2.CHG_STATUS+'-'+ p.SUB_DESC as DESC3,t2.APPROVE_DT as APPROVE_DT");
            sb.Append(" ,t2.APPROVE_BY as APPROVE_BY ,t2.REMARK as REMARK ,t2.APP_REMARK as APP_REMARK,t2.SEQ_NO as SEQ_NO , ct.EMP_ID + '-' + ct.EMP_NAME as CREATED_NAME	");
            sb.Append(" ,t2.CREATED_BY, t2.CREATED_BY + CASE WHEN ISNULL(ct.EMP_NAME,'') = '' THEN '' ELSE '-' + ct.EMP_NAME END AS CREATE_NAME");
            sb.Append(" , t2.APPROVE_BY + CASE WHEN ISNULL(at.EMP_NAME,'') = '' THEN '' ELSE '-' + at.EMP_NAME END AS APPROVE_NAME");
            sb.Append(" ,dept.DEPT_NO , dept.DEPT_NAME ");
            sb.Append(" from TB_S_M_SUBSIDY_DEDU_1_TMP t2");
            sb.Append(" left join TB_H_M_EMP t  on t2.EMP_ID = t.EMP_ID");
            sb.Append(" left join TB_S_M_SALARY_ITEM s on  t2.SALARY_ID = s.SALARY_ID");
            sb.Append(" left join TB_9_M_COMM_D d on  d.SYS_CD ='HB' and  d.MAIN_CD='EMP_CD' and  t.EMP_CD = d.SUB_CD");
            sb.Append(" left join TB_9_M_COMM_D h on  h.MAIN_CD='PROCESS_STATUS' and h.SYS_CD ='SA' and  t2.PROCESS_STATUS = h.SUB_CD");
            sb.Append(" left join TB_9_M_COMM_D p on  p.MAIN_CD='CHG_STATUS' and p.SYS_CD ='SA' and  t2.CHG_STATUS = p.SUB_CD");
            sb.Append(" left join TB_H_M_EMP ct  on t2.CREATED_BY = ct.EMP_ID");
            sb.Append(" left join TB_H_M_EMP at  on t2.APPROVE_BY = at.EMP_ID");
            sb.Append(" left join TB_H_M_DEPT dept on t.DEPT_NO = dept.DEPT_NO");
            sb.Append(" where 1=1 AND t2.EMP_ID = @EMP_ID AND t2.SALARY_ID = @SALARY_ID AND t2.DATA_YM = @DATA_YM AND t2.SEQ_NO = @SEQ_NO");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SALARY_ID", SALARY_ID);
            ht.Add("@DATA_YM", DATA_YM);
            ht.Add("@SEQ_NO", SEQ_NO);

            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable get_SYS_ID_Data()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select SYS_CD , MAIN_CD , SUB_CD ,SUB_DESC from TB_9_M_COMM_D");
            sb.Append(" where SYS_CD='HB' and MAIN_CD=CAR_TYPE  ");

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string txt_DATA_YM, string txt_EMP_ID, string txt_SALARY_ID, string ddl_PROCESS_STATUS, string ddl_SALARY_STATUS, string txt_EMP_NAME, string ddl_EMP_CD)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * From (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber, * from (");
            sb.Append(" select t1.DATA_YM as DATA_YM,t1.EMP_ID as EMP_ID ,t.EMP_NAME as EMP_NAME ,t.EMP_CD as EMP_CD,t.EMP_CD +'-'+ d.SUB_DESC as DESC1,t1.SALARY_ID  as SALARY_ID");
            sb.Append(" ,t1.SALARY_ID +'-'+ s.SALARY_NAME as SALARY_NAME ,t1.AMOUNT as CHG_AMT_A,t1.SALARY_STATUS as  SALARY_STATUS,t1.SALARY_STATUS +'-'+ p.SUB_DESC as DESC4");
            sb.Append(" ,t1.SALARY_DT as SALARY_DT,0 as CHG_AMT_B ,'Y' as PROCESS_STATUS ,'Y-已生效'as DESC2,'' as CHG_STATUS , ''as DESC3");
            sb.Append(" ,(Select Top 1 APPROVE_DT From TB_S_M_SUBSIDY_DEDU_1_TMP  Where EMP_ID = t1.EMP_ID And SALARY_ID=t1.SALARY_ID And DATA_YM = t1.DATA_YM AND APPROVE_DT is not null AND SALARY_ID=t1.SALARY_ID AND PROCESS_STATUS= 'Y' Order By APPROVE_DT DESC) as APPROVE_DT ");
            sb.Append(" ,t1.REMARK as REMARK ,'' as APP_REMARK,t1.SEQ_NO as SEQ_NO ,t2.CREATED_BY, t2.CREATED_BY + CASE WHEN ISNULL(ct.EMP_NAME,'') = '' THEN '' ELSE '-' + ct.EMP_NAME END AS CREATED_NAME");
            sb.Append(" ,(Select Top 1 IsNull(APPROVE_BY + '-' + b.EMP_NAME,'')  From TB_S_M_SUBSIDY_DEDU_1_TMP a Inner Join TB_H_M_EMP b On a.APPROVE_BY=b.EMP_ID Where a.EMP_ID = t1.EMP_ID And SALARY_ID=t1.SALARY_ID And DATA_YM = t1.DATA_YM AND APPROVE_DT is not null  AND PROCESS_STATUS= 'Y' Order By APPROVE_DT DESC) AS APPROVE_NAME ");
            sb.Append(" from TB_S_M_SUBSIDY_DEDUCTIONS_1 t1");
            sb.Append(" left join TB_H_M_EMP t  on t1.EMP_ID = t.EMP_ID");
            sb.Append(" left join TB_S_M_SALARY_ITEM s on   t1.SALARY_ID = s.SALARY_ID");
            sb.Append(" left join TB_9_M_COMM_D d on  d.SYS_CD ='HB' and  d.MAIN_CD='EMP_CD' and  t.EMP_CD = d.SUB_CD");
            sb.Append(" left join TB_9_M_COMM_D p on  p.SYS_CD ='SB' and  p.MAIN_CD='SALARY_STATUS' and  t1.SALARY_STATUS = p.SUB_CD");
            sb.Append(" left join TB_S_M_SUBSIDY_DEDU_1_TMP t2 on t1.DATA_YM = t2.DATA_YM and t1.EMP_ID = t2.EMP_ID and t1.SALARY_ID = t2.SALARY_ID and t1.SEQ_NO = t2.SEQ_NO");
            sb.Append(" left join TB_H_M_EMP ct  on t2.CREATED_BY = ct.EMP_ID");
            sb.Append(" where 1=1");
            sb.Append(" and  t1.SALARY_ID in (select SALARY_ID from TB_S_M_SUBSIDY_MEM_D where TYPE='1' and EMP_ID = @LOGIN_ID  ) and t1.DATA_YM = @DATA_YM");
            if (txt_SALARY_ID != "")
            {
                sb.Append(" and (t1.SALARY_ID = @SALARY_ID)");
            }
            if (ddl_PROCESS_STATUS != "-1" && ddl_PROCESS_STATUS != "Y")
            {
                sb.Append(" and  t1.DATA_YM = NULL");
            }
            if (ddl_SALARY_STATUS != "-1")
            {
                sb.Append(" and (t1.SALARY_STATUS = @SALARY_STATUS)");
            }
            if (ddl_EMP_CD != "-1")
            {
                sb.Append(" and (t.EMP_CD = @EMP_CD)");
            }
            if (txt_EMP_ID != "")
            {
                sb.Append(" and (t1.EMP_ID LIKE @EMP_ID)");
            }
            if (txt_EMP_NAME != "")
            {
                sb.Append(" and (t.EMP_NAME LIKE @EMP_NAME)");
            }

            sb.Append(" UNION");
            sb.Append(" select t2.DATA_YM as DATA_YM,t2.EMP_ID as EMP_ID  ,t.EMP_NAME as EMP_NAME ,t.EMP_CD as EMP_CD,t.EMP_CD +'-'+ d.SUB_DESC as DESC1,t2.SALARY_ID as SALARY_ID");
            sb.Append(" ,t2.SALARY_ID +'-'+ s.SALARY_NAME as SALARY_NAME,t2.CHG_AMT_A as CHG_AMT_A ,'N' as  SALARY_STATUS,'N-未處理' as DESC4");
            sb.Append(" ,'' as SALARY_DT,convert(varchar(10),t2.CHG_AMT_B) as CHG_AMT_B ,t2.PROCESS_STATUS as PROCESS_STATUS");
            sb.Append(" ,t2.PROCESS_STATUS+'-'+ h.SUB_DESC as DESC2,t2.CHG_STATUS as CHG_STATUS ,t2.CHG_STATUS+'-'+ p.SUB_DESC as DESC3,t2.APPROVE_DT as APPROVE_DT");
            sb.Append(" ,t2.REMARK as REMARK ,t2.APP_REMARK as APP_REMARK,t2.SEQ_NO as SEQ_NO ");
            sb.Append(" ,t2.CREATED_BY, t2.CREATED_BY + CASE WHEN ISNULL(ct.EMP_NAME,'') = '' THEN '' ELSE '-' + ct.EMP_NAME END AS CREATED_NAME");
            sb.Append(" , t2.APPROVE_BY + CASE WHEN ISNULL(at.EMP_NAME,'') = '' THEN '' ELSE '-' + at.EMP_NAME END AS APPROVE_NAME");
            sb.Append(" from TB_S_M_SUBSIDY_DEDU_1_TMP t2");
            sb.Append(" left join TB_H_M_EMP t  on t2.EMP_ID = t.EMP_ID");
            sb.Append(" left join TB_S_M_SALARY_ITEM s on  t2.SALARY_ID = s.SALARY_ID");
            sb.Append(" left join TB_9_M_COMM_D d on  d.SYS_CD ='HB' and  d.MAIN_CD='EMP_CD' and  t.EMP_CD = d.SUB_CD");
            sb.Append(" left join TB_9_M_COMM_D h on  h.MAIN_CD='PROCESS_STATUS' and h.SYS_CD ='SA' and  t2.PROCESS_STATUS = h.SUB_CD");
            sb.Append(" left join TB_9_M_COMM_D p on  p.MAIN_CD='CHG_STATUS' and p.SYS_CD ='SA' and  t2.CHG_STATUS = p.SUB_CD");
            sb.Append(" left join TB_H_M_EMP ct  on t2.CREATED_BY = ct.EMP_ID");
            sb.Append(" left join TB_H_M_EMP at  on t2.APPROVE_BY = at.EMP_ID");
            sb.Append(" where 1=1");
            sb.Append(" and  t2.SALARY_ID in (select SALARY_ID from TB_S_M_SUBSIDY_MEM_D where TYPE='1' and EMP_ID = @LOGIN_ID  )  and t2.PROCESS_STATUS <>'Y' and t2.DATA_YM = @DATA_YM");
            if (txt_SALARY_ID != "")
            {
                sb.Append(" and (t2.SALARY_ID = @SALARY_ID)");
            }
            if (ddl_PROCESS_STATUS != "-1")
            {
                sb.Append(" and  t2.PROCESS_STATUS = @PROCESS_STATUS");
            }
            if (ddl_SALARY_STATUS != "-1")
            {
                if (ddl_SALARY_STATUS == "Y")
                {
                    sb.Append(" and  t2.DATA_YM = NULL");
                }
            }
            if (ddl_EMP_CD != "-1")
            {
                sb.Append(" and (t.EMP_CD = @EMP_CD)");
            }
            if (txt_EMP_ID != "")
            {
                sb.Append(" and (t2.EMP_ID LIKE @EMP_ID)");
            }
            if (txt_EMP_NAME != "")
            {
                sb.Append(" and (t.EMP_NAME LIKE @EMP_NAME)");
            }
            sb.Append(" ) as tb");
            sb.Append(" ) god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar)");
            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);

            string DATA_YM = txt_DATA_YM.Replace("/", "");
            ht.Add("@DATA_YM", DATA_YM);
            ht.Add("@SALARY_ID", txt_SALARY_ID);
            ht.Add("@PROCESS_STATUS", ddl_PROCESS_STATUS);
            ht.Add("@SALARY_STATUS", ddl_SALARY_STATUS);
            ht.Add("@EMP_CD", ddl_EMP_CD);
            ht.Add("@EMP_ID", string.Format("{0}%", txt_EMP_ID));
            ht.Add("@EMP_NAME", string.Format("{0}%", txt_EMP_NAME));
            ht.Add("@LOGIN_ID", SessionHandle.Current.emp_id);

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public int getCount(int startRowIndex, int maximumRows, string txt_DATA_YM, string txt_EMP_ID, string txt_SALARY_ID, string ddl_PROCESS_STATUS, string ddl_SALARY_STATUS, string txt_EMP_NAME, string ddl_EMP_CD)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select count(*) as total_record from (");
            sb.Append(" select t1.DATA_YM as DATA_YM,t1.EMP_ID as EMP_ID ,t.EMP_NAME as EMP_NAME ,t.EMP_CD as EMP_CD,t.EMP_CD +'-'+ d.SUB_DESC as DESC1,t1.SALARY_ID  as SALARY_ID");
            sb.Append(" ,t1.SALARY_ID +'-'+ s.SALARY_NAME as SALARY_NAME ,t1.AMOUNT as CHG_AMT_A,t1.SALARY_STATUS as  SALARY_STATUS,t1.SALARY_STATUS +'-'+ p.SUB_DESC as DESC4");
            sb.Append(" ,t1.SALARY_DT as SALARY_DT,0 as CHG_AMT_B ,'Y' as PROCESS_STATUS ,'Y-已生效'as DESC2,'' as CHG_STATUS , ''as DESC3");
            sb.Append(" ,(Select Top 1 APPROVE_DT From TB_S_M_SUBSIDY_DEDU_1_TMP Where EMP_ID = t1.EMP_ID And SALARY_ID=t1.SALARY_ID And DATA_YM = t1.DATA_YM AND APPROVE_DT is not null AND SALARY_ID=t1.SALARY_ID AND PROCESS_STATUS= 'Y' Order By APPROVE_DT DESC) as APPROVE_DT ");
            sb.Append(" ,(Select Top 1 IsNull(APPROVE_BY + '-' + b.EMP_NAME,'')  From TB_S_M_SUBSIDY_DEDU_1_TMP a Inner Join TB_H_M_EMP b On a.APPROVE_BY=b.EMP_ID Where a.EMP_ID = t1.EMP_ID And a.SALARY_ID=t1.SALARY_ID And DATA_YM = t1.DATA_YM AND APPROVE_DT is not null AND PROCESS_STATUS= 'Y' Order By APPROVE_DT DESC) AS APPROVE_NAME ");
            sb.Append( ",t1.REMARK as REMARK ,'' as APP_REMARK,t1.SEQ_NO as SEQ_NO");
            sb.Append(" from TB_S_M_SUBSIDY_DEDUCTIONS_1 t1");
            sb.Append(" left join TB_H_M_EMP t  on t1.EMP_ID = t.EMP_ID");
            sb.Append(" left join TB_S_M_SALARY_ITEM s on   t1.SALARY_ID = s.SALARY_ID");
            sb.Append(" left join TB_9_M_COMM_D d on  d.SYS_CD ='HB' and  d.MAIN_CD='EMP_CD' and  t.EMP_CD = d.SUB_CD");
            sb.Append(" left join TB_9_M_COMM_D p on  p.SYS_CD ='SB' and  p.MAIN_CD='SALARY_STATUS' and  t1.SALARY_STATUS = p.SUB_CD");
            sb.Append(" left join TB_S_M_SUBSIDY_DEDU_1_TMP at  on t1.EMP_ID = at.EMP_ID  and t1.DATA_YM = at.DATA_YM and t1.SALARY_ID = at.SALARY_ID");
            sb.Append(" left join TB_H_M_EMP ct  on at.CREATED_BY = ct.EMP_ID");
            sb.Append(" where 1=1");
            sb.Append(" and  t1.SALARY_ID in (select SALARY_ID from TB_S_M_SUBSIDY_MEM_D where TYPE='1' and EMP_ID = @LOGIN_ID  ) and t1.DATA_YM = @DATA_YM");
            if (txt_SALARY_ID != "")
            {
                sb.Append(" and (t1.SALARY_ID = @SALARY_ID)");
            }
            if (ddl_PROCESS_STATUS != "-1" && ddl_PROCESS_STATUS != "Y")
            {
                sb.Append(" and  t1.DATA_YM = NULL");
            }
            if (ddl_SALARY_STATUS != "-1")
            {
                sb.Append(" and (t1.SALARY_STATUS = @SALARY_STATUS)");
            }
            if (ddl_EMP_CD != "-1")
            {
                sb.Append(" and (t.EMP_CD = @EMP_CD)");
            }
            if (txt_EMP_ID != "")
            {
                sb.Append(" and (t1.EMP_ID LIKE @EMP_ID)");
            }
            if (txt_EMP_NAME != "")
            {
                sb.Append(" and (t.EMP_NAME LIKE @EMP_NAME)");
            }

            sb.Append(" UNION");
            sb.Append(" select t2.DATA_YM as DATA_YM,t2.EMP_ID as EMP_ID  ,t.EMP_NAME as EMP_NAME ,t.EMP_CD as EMP_CD,t.EMP_CD +'-'+ d.SUB_DESC as DESC1,t2.SALARY_ID as SALARY_ID");
            sb.Append(" ,t2.SALARY_ID +'-'+ s.SALARY_NAME as SALARY_NAME,t2.CHG_AMT_A as CHG_AMT_A ,'N' as  SALARY_STATUS,'N-未處理' as DESC4");
            sb.Append(" ,'' as SALARY_DT,convert(varchar(10),t2.CHG_AMT_B) as CHG_AMT_B ,t2.PROCESS_STATUS as PROCESS_STATUS");
            sb.Append(" ,t2.PROCESS_STATUS+'-'+ h.SUB_DESC as DESC2,t2.CHG_STATUS as CHG_STATUS ,t2.CHG_STATUS+'-'+ p.SUB_DESC as DESC3,t2.APPROVE_DT as APPROVE_DT, '' AS APPROVE_NAME");
            sb.Append(" ,t2.REMARK as REMARK ,t2.APP_REMARK as APP_REMARK,t2.SEQ_NO as SEQ_NO");
            sb.Append(" from TB_S_M_SUBSIDY_DEDU_1_TMP t2");
            sb.Append(" left join TB_H_M_EMP t  on t2.EMP_ID = t.EMP_ID");
            sb.Append(" left join TB_S_M_SALARY_ITEM s on  t2.SALARY_ID = s.SALARY_ID");
            sb.Append(" left join TB_9_M_COMM_D d on  d.SYS_CD ='HB' and  d.MAIN_CD='EMP_CD' and  t.EMP_CD = d.SUB_CD");
            sb.Append(" left join TB_9_M_COMM_D h on  h.MAIN_CD='PROCESS_STATUS' and h.SYS_CD ='SA' and  t2.PROCESS_STATUS = h.SUB_CD");
            sb.Append(" left join TB_9_M_COMM_D p on  p.MAIN_CD='CHG_STATUS' and p.SYS_CD ='SA' and  t2.CHG_STATUS = p.SUB_CD");
            sb.Append(" where 1=1");
            sb.Append(" and  t2.SALARY_ID in (select SALARY_ID from TB_S_M_SUBSIDY_MEM_D where TYPE='1' and EMP_ID = @LOGIN_ID  )  and t2.PROCESS_STATUS <>'Y' and t2.DATA_YM = @DATA_YM");
            if (txt_SALARY_ID != "")
            {
                sb.Append(" and (t2.SALARY_ID = @SALARY_ID)");
            }
            if (ddl_PROCESS_STATUS != "-1")
            {
                sb.Append(" and  t2.PROCESS_STATUS = @PROCESS_STATUS");
            }
            if (ddl_SALARY_STATUS != "-1")
            {
                if (ddl_SALARY_STATUS == "Y")
                {
                    sb.Append(" and  t2.DATA_YM = NULL");
                }
            }
            if (ddl_EMP_CD != "-1")
            {
                sb.Append(" and (t.EMP_CD = @EMP_CD)");
            }
            if (txt_EMP_ID != "")
            {
                sb.Append(" and (t2.EMP_ID LIKE @EMP_ID)");
            }
            if (txt_EMP_NAME != "")
            {
                sb.Append(" and (t.EMP_NAME LIKE @EMP_NAME)");
            }
            sb.Append(" ) as tb");


            string DATA_YM = txt_DATA_YM.Replace("/", "");
            ht.Add("@DATA_YM", DATA_YM);
            ht.Add("@SALARY_ID", txt_SALARY_ID);
            ht.Add("@PROCESS_STATUS", ddl_PROCESS_STATUS);
            ht.Add("@SALARY_STATUS", ddl_SALARY_STATUS);
            ht.Add("@EMP_CD", ddl_EMP_CD);
            ht.Add("@EMP_ID", string.Format("{0}%", txt_EMP_ID));
            ht.Add("@EMP_NAME", string.Format("{0}%", txt_EMP_NAME));
            ht.Add("@LOGIN_ID", SessionHandle.Current.emp_id);

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

    public DataTable getModeData(string id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("     select ROW_NUMBER() OVER(ORDER BY m.FUNC_ID) As RowNumber,");
            sb.Append("     d.* ");
            sb.Append("     from TB_9_M_SYS_M as m					");
            sb.Append("     inner join TB_9_M_SYS_D as d on m.FUNC_ID = d.FUNC_ID and m.SYS_ID+m.MODE_ID = @ID");
            sb.Append("     where 1=1");

            ht.Add("@ID", id);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    public DataTable getModeData(int startRowIndex, int maximumRows, string sortExpression, string id)
    {
        try
        {
            if (sortExpression == "")
            {
                sortExpression = "MODE_ID";
            }
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * From");
            sb.Append(" (");
            sb.Append("     select ROW_NUMBER() OVER(ORDER BY m.FUNC_ID) As RowNumber,");
            sb.Append("     d.* ");
            sb.Append("     from TB_9_M_SYS_M as m					");
            sb.Append("     inner join TB_9_M_SYS_D as d on m.FUNC_ID = d.FUNC_ID and m.SYS_ID+m.MODE_ID = @ID");
            sb.Append("     where 1=1");
            sb.Append(" ) god_data where RowNumber between CAST(@startRowIndex as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar)");

            ht.Add("@ID", id);
            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public int getModeCount(int startRowIndex, int maximumRows, string id)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select COUNT(*) total_record from (");
            sb.Append(" 	select d.MODE_ID, d.FUNCTION_ID, d.FUNCTION_NAME");
            sb.Append("     from TB_9_M_SYS_M as m					");
            sb.Append("     inner join TB_9_M_SYS_D as d on m.FUNC_ID = d.FUNC_ID and m.SYS_ID+m.MODE_ID = @ID");
            sb.Append("     where 1=1");
            sb.Append(" ) as tb1");

            ht.Add("@ID", id);
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

    //取得基本資料
    public DataTable getEMPFile()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select E.EMP_NAME, E.EMP_CD, COMM.SUB_DESC, E.DEPT_NO, D.DEPT_NAME, E.PJOB_CD, E.PJOB_DESC,E.WORK_SHIFT_CD, E.WORK_SHIFT_DESC, E.LEAVE_DT");
            sb.Append(" , CONVERT(char(10), E.JOIN_DT, 120) JOIN_DT ,E.REGISTER_ADDR");
            sb.Append(" , (select top 1 ADDRESS from TB_D_M_TRANS_ALLOWANCE_D where E.EMP_ID = TB_D_M_TRANS_ALLOWANCE_D.EMP_ID) CONTACT_ADDR");
            sb.Append(" , E.MOBILE_TEL_1, E.CONTACT_TEL,AGE ");
            sb.Append(" , E.EMP_CD + '-' + COMM.SUB_DESC EMP_CD_DESC   ");
            sb.Append(" FROM VW_H_EMP_DATA AS E");
            sb.Append(" LEFT JOIN VW_H_DEPT_DATA AS D ON E.DEPT_NO = D.DEPT_NO");
            sb.Append(" LEFT JOIN TB_9_M_COMM_D AS COMM ON E.EMP_CD = COMM.SUB_CD and COMM.MAIN_CD = 'EMP_CD' AND COMM.SYS_CD = 'HB'");
            sb.Append(" WHERE EMP_ID = @EMP_ID");
            ht.Add("@EMP_ID", EMP_ID);

            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable getEMPFile(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select *");
            sb.Append(" FROM TB_H_M_EMP");
            sb.Append(" WHERE EMP_ID = @EMP_ID");
            ht.Add("@EMP_ID", emp_id);

            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得基本資料
    public DataTable getSALARYFile()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select SALARY_ID,SALARY_NAME from TB_S_M_SALARY_ITEM");
            sb.Append(" WHERE SALARY_ID = @SALARY_ID");
            sb.Append(" and SALARY_ID in ( select SALARY_ID from TB_S_M_SUBSIDY_MEM_D   ");
            sb.Append("                     where  TYPE =@TYPE ");
            sb.Append("                     and EMP_ID=@EMP_ID ");
            sb.Append("                     )");
            ht.Add("@SALARY_ID", SALARY_ID);
            ht.Add("@TYPE", "1");
            ht.Add("@EMP_ID", EMP_ID);
            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {
            throw;
        }
    }

    public string deleteData()
    {


        //刪除共用代碼主檔
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.Append("Update TB_S_M_SUBSIDY_DEDU_1_TMP ");
        sb.Append(" Set PROCESS_STATUS='N',CHG_STATUS='D',UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE()");
        sb.Append(" where EMP_ID = @EMP_ID");
        ht.Add("@EMP_ID", EMP_ID.Substring(0, 5));


        ht.Add("@UPDATED_BY", UPDATED_BY);
        dbConn.ExecuteT(sb, ht, true);



        return "0";
    }
    public string deleteTB_S_M_SUBSIDY_DEDU_1_TMP()
    {


        //刪除TB_S_M_SUBSIDY_DEDU_1_TMP
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.Append(" Delete TB_S_M_SUBSIDY_DEDU_1_TMP ");
        sb.Append(" where EMP_ID = @EMP_ID AND SALARY_ID = @SALARY_ID AND DATA_YM = @DATA_YM AND SEQ_NO = @SEQ_NO");
        ht.Add("@EMP_ID", EMP_ID);
        ht.Add("@SALARY_ID", SALARY_ID);
        ht.Add("@DATA_YM", DATA_YM);
        ht.Add("@SEQ_NO", SEQ_NO);
        dbConn.ExecuteT(sb, ht, true);



        return "0";
    }
    public string deleteTmp()
    {


        //刪除共用代碼主檔
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.Append("DELETE FROM TB_S_S_SUBSIDY_TMP");
        sb.Append(" where CREATED_BY = @EMP_ID");
        ht.Add("@EMP_ID", SessionHandle.Current.emp_id);


        dbConn.ExecuteT(sb, ht, true);



        return "0";
    }

    /// <summary>
    /// 取得 最近一次薪資計算年月
    /// </summary>
    /// <returns></returns>
    public DataTable getLatestSalaryYM()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select dbo.FN_S_SALARY_YM() as SALARY_YM ");
            return dbConn.Query(sb, ht);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    /// <summary>
    /// 取得 前一個月的薪資是否已鎖定
    /// </summary>
    /// <returns></returns>
    public DataTable getIsLoked()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select SALARY_LOCKED from TB_S_M_SALARY_MONTH_CTRL ");
            sb.Append(" where SALARY_TYPE=@SALARY_TYPE ");
            sb.Append(" and OPERATION_ID =@OPERATION_ID ");
            sb.Append(" and SALARY_YM =@SALARY_YM ");
            ht.Add("@SALARY_TYPE", "A");
            ht.Add("@OPERATION_ID", "G01");
            ht.Add("@SALARY_YM", DATA_YM);

            return dbConn.Query(sb, ht);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    //是否有權限新增
    public DataTable getSubsidyCount()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select count(0) resultCount from TB_S_M_SUBSIDY_MEM_D ");
            sb.Append(" where TYPE=@TYPE ");
            sb.Append(" and EMP_ID=@EMP_ID ");
            ht.Add("@TYPE", "1");
            ht.Add("@EMP_ID", SessionHandle.Current.emp_id);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }


    /// <summary>
    /// 取得序號
    /// </summary>
    /// <returns></returns>
    public DataTable getSeqNO()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select isnull(max(SEQ_NO),0) SEQ_NO from TB_S_M_SUBSIDY_DEDU_1_TMP ");
            sb.Append(" where DATA_YM =@DATA_YM and EMP_ID=@EMP_ID and SALARY_ID=@SALARY_ID ");
            ht.Add("@DATA_YM", DATA_YM);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SALARY_ID", SALARY_ID);

            return dbConn.QueryT(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public void addData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("INSERT INTO TB_S_M_SUBSIDY_DEDU_1_TMP ");
            sb.Append("( DATA_YM,EMP_ID, SALARY_ID, SEQ_NO, SEQ_NO_B, CHG_AMT_B, CHG_AMT_A  ");
            sb.Append(" ,CHG_STATUS, PROCESS_STATUS, APPROVE_BY, APPROVE_DT, REMARK, APP_REMARK  ");
            sb.Append(" ,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID )  ");
            sb.Append(" Values (");
            sb.Append(" @DATA_YM, @EMP_ID, @SALARY_ID, @SEQ_NO, @SEQ_NO_B, @CHG_AMT_B ");
            sb.Append(" ,@CHG_AMT_A, @CHG_STATUS, @PROCESS_STATUS, @APPROVE_BY, @APPROVE_DT, @REMARK, @APP_REMARK ");
            sb.Append(" ,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID ");
            sb.Append(" ) ");
            ht.Add("@DATA_YM", DATA_YM);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SALARY_ID", SALARY_ID);
            ht.Add("@SEQ_NO", SEQ_NO);
            ht.Add("@SEQ_NO_B", "0");
            ht.Add("@CHG_AMT_B", "0");
            ht.Add("@CHG_AMT_A", CHG_AMT_A);
            ht.Add("@CHG_STATUS", CHG_STATUS);
            ht.Add("@PROCESS_STATUS", "N");
            ht.Add("@APPROVE_BY", "");
            ht.Add("@APPROVE_DT", DBNull.Value);

            ht.Add("@REMARK", REMARK);
            ht.Add("@APP_REMARK", "");
            ht.Add("@CREATED_BY", CREATED_BY);
            ht.Add("@UPDATED_BY", CREATED_BY);
            ht.Add("@FUNC_ID", "FB2SB2300");


            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }
    public void addData1()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("INSERT INTO TB_S_M_SUBSIDY_DEDU_1_TMP ");
            sb.Append("( DATA_YM,EMP_ID, SALARY_ID, SEQ_NO, SEQ_NO_B, CHG_AMT_B, CHG_AMT_A  ");
            sb.Append(" ,CHG_STATUS, PROCESS_STATUS, APPROVE_BY, APPROVE_DT, REMARK, APP_REMARK  ");
            sb.Append(" ,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID )  ");
            sb.Append(" Values (");
            sb.Append(" @DATA_YM, @EMP_ID, @SALARY_ID, @SEQ_NO, @SEQ_NO_B, @CHG_AMT_B ");
            sb.Append(" ,@CHG_AMT_A, @CHG_STATUS, @PROCESS_STATUS, @APPROVE_BY, @APPROVE_DT, @REMARK, @APP_REMARK ");
            sb.Append(" ,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID ");
            sb.Append(" ) ");
            ht.Add("@DATA_YM", DATA_YM);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SALARY_ID", SALARY_ID);
            ht.Add("@SEQ_NO", SEQ_NO);
            ht.Add("@SEQ_NO_B", SEQ_NO_B);
            ht.Add("@CHG_AMT_B", CHG_AMT_B);
            ht.Add("@CHG_AMT_A", CHG_AMT_A);
            ht.Add("@CHG_STATUS", CHG_STATUS);
            ht.Add("@PROCESS_STATUS", "N");
            ht.Add("@APPROVE_BY", "");
            ht.Add("@APPROVE_DT", DBNull.Value);

            ht.Add("@REMARK", REMARK);
            ht.Add("@APP_REMARK", "");
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SB2300");


            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }

    public void addExcelData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" INSERT INTO TB_S_S_SUBSIDY_TMP");
            sb.Append("                          (CREATED_BY, SEQ_NO, EMP_ID, SALARY_ID, AMOUNT, REMARK, OP_MSG)");
            sb.Append(" VALUES        (@CREATED_BY, @SEQ_NO, @EMP_ID, @SALARY_ID, @AMOUNT, @REMARK, @OP_MSG)");
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@SEQ_NO", SEQ_NO);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SALARY_ID", SALARY_ID);
            ht.Add("@AMOUNT", AMOUNT);
            ht.Add("@REMARK", REMARK);
            ht.Add("@OP_MSG", OP_MSG);


            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void testData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" INSERT INTO TB_S_S_SUBSIDY_TMP");
            sb.Append("                          (CREATED_BY, SEQ_NO, EMP_ID, EMP_NAME, AMOUNT, REMARK, OP_MSG)");
            sb.Append(" VALUES        (@CREATED_BY, @SEQ_NO, @EMP_ID, @EMP_NAME, @AMOUNT, @REMARK, @OP_MSG)");
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@SEQ_NO", SEQ_NO);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@EMP_NAME", EMP_NAME);
            ht.Add("@AMOUNT", AMOUNT);
            ht.Add("@REMARK", REMARK);
            ht.Add("@OP_MSG", "");


            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void updateData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" UPDATE			TB_S_M_SUBSIDY_DEDU_1_TMP");
            sb.Append(" SET				CHG_AMT_B = @CHG_AMT_B, CHG_AMT_A = @CHG_AMT_A, CHG_STATUS = @CHG_STATUS, PROCESS_STATUS = @PROCESS_STATUS, APPROVE_BY = @APPROVE_BY, APPROVE_DT = @APPROVE_DT");
            sb.Append(" 				, REMARK = @REMARK, APP_REMARK = @APP_REMARK, CREATED_BY = @CREATED_BY, CREATED_DT = GETDATE(), UPDATED_BY = @UPDATED_BY, UPDATED_DT = GETDATE(),  FUNC_ID = @FUNC_ID");
            sb.Append(" WHERE			(DATA_YM = @DATA_YM) AND (EMP_ID = @EMP_ID) AND (SALARY_ID = @SALARY_ID) AND (SEQ_NO = @SEQ_NO)");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SALARY_ID", SALARY_ID);
            ht.Add("@DATA_YM", DATA_YM);
            ht.Add("@SEQ_NO", SEQ_NO);
            ht.Add("@CHG_AMT_B", CHG_AMT_B);
            ht.Add("@CHG_AMT_A", CHG_AMT_A);
            ht.Add("@CHG_STATUS", CHG_STATUS);
            ht.Add("@PROCESS_STATUS", "N");
            ht.Add("@APPROVE_BY", "");
            ht.Add("@APPROVE_DT", DBNull.Value);
            ht.Add("@REMARK", REMARK);
            ht.Add("@APP_REMARK", APP_REMARK);
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SB230");


            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
}