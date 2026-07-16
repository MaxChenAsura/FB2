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
/// CFB2SB2200BO 的摘要描述
/// </summary>
public class CFB2SB2200DAO : BaseDAO
{
    public CFB2SB2200DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public string RowNumber { get; set; }
    public string QDATAKEY { get; set; }
    public string SEQ_NO { get; set; }
    public string EMP_ID { get; set; }
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
    public string DATA_YM { get; set; }

    public string IS_PLUS { get; set; }
    public string IS_TAX { get; set; }

    public DataTable getPROCESS_STATUS()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select SYS_CD , MAIN_CD , SUB_CD ,SUB_DESC from TB_9_M_COMM_D");
            sb.Append(" where SYS_CD='SA' and MAIN_CD='PROCESS_STATUS'  ");
            return dbConn.Query(sb);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getEMP_CD()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select SYS_CD , MAIN_CD , SUB_CD ,SUB_DESC from TB_9_M_COMM_D");
            sb.Append(" where SYS_CD='HB' and MAIN_CD='EMP_CD'  ");
            return dbConn.Query(sb);
        }
        catch
        {
            throw;
        }
    }
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
    public DataTable getSALARY_ITEM(string tablename, string columnname, string qrystr)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendFormat(" select * from {0}", tablename);
            sb.AppendFormat(" where {0} = '{1}'", columnname, qrystr);
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
    //internal System.Data.DataTable getSYS_ID()
    //{
    //    try
    //    {
    //        StringBuilder sb = new StringBuilder();
    //        Hashtable ht = new Hashtable();
    //        sb.Append(" select SUB_CD ,SUB_DESC from TB_9_M_COMM_D where 1=1 and sys_cd = '99' and main_cd = 'sys_id' and is_valid ='Y' order by SUB_CD");
    //        return dbConn.Query(sb);

    //    }
    //    catch (Exception)
    //    {

    //        throw;
    //    }
    //}

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
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string txt_SALARY_ID, string txt_START_SDT_B, string txt_START_SDT_A, string ddl_EMP_CD, string txt_EMP_ID, string txt_EMP_NAME)
    {
        try
        {
            if (sortExpression.Contains("EMP_ID"))
            {
                sortExpression = string.Format("t2.{0}", sortExpression);
            }
            if (sortExpression.Contains("SALARY_ID"))
            {
                sortExpression = string.Format("t2.{0}", sortExpression);
            }
            if (sortExpression.Contains("EMP_CD"))
            {
                sortExpression = string.Format("t.{0}", sortExpression);
            }
            if (sortExpression.Contains("EMP_NAME"))
            {
                sortExpression = string.Format("t.{0}", sortExpression);
            }
            if (sortExpression.Contains("CREATED_BY"))
            {
                sortExpression = string.Format("t2.{0}", sortExpression);
            }
            if (sortExpression.Contains("REMARK"))
            {
                sortExpression = string.Format("t2.{0}", sortExpression);
            }
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * From");
            sb.Append(" (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber");
            sb.Append(" ,t2.EMP_ID as qdatakey");
            sb.Append(" , t2.EMP_ID,t.EMP_NAME,ed.SUB_DESC as EMP_CD_DESC,t.EMP_CD,t2.SALARY_ID as SALARY_ID ,t2.SALARY_ID +'-'+ s.SALARY_NAME as SALARY_NAME");
            sb.Append(" , t2.CHG_AMT_A as CHG_AMT_A ,t2.CHG_AMT_B as CHG_AMT_B,t2.PROCESS_STATUS as PROCESS_STATUS ,t2.PROCESS_STATUS+'-'+ d.SUB_DESC as DESC1");
            sb.Append(" , t2.START_DT_A, t2.START_DT_B,t2.END_DATE_B,t2.END_DATE_A,t2.CHG_STATUS as CHG_STATUS,t2.CHG_STATUS+'-'+ p.SUB_DESC as DESC2");
            sb.Append(" , t2.APPROVE_DT as APPROVE_DT ,t2.APPROVE_BY as APPROVE_BY,t2.REMARK as REMARK ,t2.APP_REMARK as APP_REMARK, t2.CREATED_BY, t3.EMP_NAME as CREATED_NAME,t2.CREATED_BY + CASE WHEN ISNULL(t3.EMP_NAME,'') = '' THEN '' ELSE '-' + t3.EMP_NAME END as CREATED_DESC");
            sb.Append(" ,t.EMP_CD + CASE WHEN ISNULL(ed.SUB_DESC,'') = '' THEN '' ELSE '-' + ed.SUB_DESC END as EMP_CD_DESC1 ,t2.SEQ_NO");
            sb.Append(" from TB_S_M_SUBSIDY_DEDU_2_TMP t2 ");
            sb.Append(" left join TB_H_M_EMP t on t2.EMP_ID = t.EMP_ID");
            sb.Append(" left join TB_S_M_SALARY_ITEM s on t2.SALARY_ID = s.SALARY_ID");
            sb.Append(" left join TB_9_M_COMM_D d on  d.SYS_CD='SA' and    d.MAIN_CD='PROCESS_STATUS' and d.SYS_CD ='SA' and  t2.PROCESS_STATUS = d.SUB_CD");
            sb.Append(" left join TB_9_M_COMM_D p on  p.SYS_CD='SA' and    p.MAIN_CD='CHG_STATUS' and p.SYS_CD ='SA' and  t2.CHG_STATUS = p.SUB_CD");
            sb.Append(" left join TB_H_M_EMP b on b.EMP_ID = t2.UPDATED_BY");
            sb.Append(" left join TB_H_R_HEAD_DEPT c on b.DEPT_NO=c.MNG_DEPT_NO");
            sb.Append(" left join TB_9_M_COMM_D ed on  t.EMP_CD = ed.SUB_CD  and ed.SYS_CD='HB' and    ed.MAIN_CD='EMP_CD' ");
            sb.Append(" left join TB_H_M_EMP t3 on t2.CREATED_BY = t3.EMP_ID");
            sb.Append(" where 1=1 and t2.PROCESS_STATUS ='N'");
            sb.Append(" and c.EMP_ID = @LOGIN_ID ");
            if (string.IsNullOrEmpty(APPROVE_BY))
            {
                APPROVE_BY = string.Empty;
            }
            if (APPROVE_BY != "")
            {
                sb.Append(" and t2.APPROVE_BY = @APPROVE_BY  ");
                ht.Add("@APPROVE_BY", APPROVE_BY);
            }

            if (txt_SALARY_ID != "")
            {
                sb.Append(" and t2.SALARY_ID = @SALARY_ID  ");
                ht.Add("@SALARY_ID", txt_SALARY_ID);
            }
            if (txt_START_SDT_B != "")
            {
                sb.Append(" and t2.START_DT_B >= @START_DT_B  ");
                ht.Add("@START_DT_B", txt_START_SDT_B);
            }
            if (txt_START_SDT_A != "")
            {
                sb.Append(" and t2.START_DT_B = @START_SDT_A  ");
                ht.Add("@START_SDT_A", txt_START_SDT_A);
            }
            if (ddl_EMP_CD != " " && ddl_EMP_CD != "-1")
            {
                sb.Append(" and t.EMP_CD = @EMP_CD ");
                ht.Add("@EMP_CD", ddl_EMP_CD);
            }
            if (txt_EMP_ID != "")
            {
                sb.Append(" and t2.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", txt_EMP_ID);
            }
            if (txt_EMP_NAME != "")
            {
                sb.Append(" and t.EMP_NAME LIKE @EMP_NAME ");
                ht.Add("@EMP_NAME", string.Format("%{0}%", txt_EMP_NAME));
            }
            ht.Add("@LOGIN_ID", SessionHandle.Current.emp_id);
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
    public int getCount(int startRowIndex, int maximumRows, string txt_SALARY_ID, string txt_START_SDT_B, string txt_START_SDT_A, string ddl_EMP_CD, string txt_EMP_ID, string txt_EMP_NAME)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) total_record ");
            sb.Append(" from TB_S_M_SUBSIDY_DEDU_2_TMP t2 ");
            sb.Append(" left join TB_H_M_EMP t on t2.EMP_ID = t.EMP_ID");
            sb.Append(" left join TB_S_M_SALARY_ITEM s on t2.SALARY_ID = s.SALARY_ID");
            sb.Append(" left join TB_9_M_COMM_D d on  d.SYS_CD='SA' and    d.MAIN_CD='PROCESS_STATUS' and d.SYS_CD ='SA' and  t2.PROCESS_STATUS = d.SUB_CD");
            sb.Append(" left join TB_9_M_COMM_D p on  p.SYS_CD='SA' and    p.MAIN_CD='CHG_STATUS' and p.SYS_CD ='SA' and  t2.CHG_STATUS = p.SUB_CD");
            sb.Append(" left join TB_H_M_EMP b on b.EMP_ID = t2.UPDATED_BY");
            sb.Append(" left join TB_H_R_HEAD_DEPT c on b.DEPT_NO=c.MNG_DEPT_NO");
            sb.Append(" left join TB_9_M_COMM_D ed on  t.EMP_CD = ed.SUB_CD  and ed.SYS_CD='HB' and    ed.MAIN_CD='EMP_CD' ");
            sb.Append(" left join TB_H_M_EMP t3 on t2.CREATED_BY = t3.EMP_ID");
            sb.Append(" where 1=1 and t2.PROCESS_STATUS ='N' ");
            sb.Append(" and c.EMP_ID = @LOGIN_ID ");
            if (APPROVE_BY != "")
            {
                sb.Append(" and t2.APPROVE_BY = @APPROVE_BY  ");
                ht.Add("@APPROVE_BY", APPROVE_BY);
            }

            if (txt_SALARY_ID != "")
            {
                sb.Append(" and t2.SALARY_ID = @SALARY_ID  ");
                ht.Add("@SALARY_ID", txt_SALARY_ID);
            }
            if (txt_START_SDT_B != "")
            {
                sb.Append(" and START_DT_B >= @START_DT_B  ");
                ht.Add("@START_DT_B", txt_START_SDT_B);
            }
            if (txt_START_SDT_A != "")
            {
                sb.Append(" and START_DT_B = @START_SDT_A  ");
                ht.Add("@START_SDT_A", txt_START_SDT_A);
            }
            if (ddl_EMP_CD != " " && ddl_EMP_CD != "-1")
            {
                sb.Append(" and t.EMP_CD = @EMP_CD ");
                ht.Add("@EMP_CD", ddl_EMP_CD);
            }
            if (txt_EMP_ID != "")
            {
                sb.Append(" and t2.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", txt_EMP_ID);
            }
            if (txt_EMP_NAME != "")
            {
                sb.Append(" and t.EMP_NAME LIKE @EMP_NAME ");
                ht.Add("@EMP_NAME", string.Format("%{0}%", txt_EMP_NAME));
            }
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

    //public DataTable getData()
    //{
    //    try
    //    {
    //        StringBuilder sb = new StringBuilder();
    //        sb.Append(" Select * From TB_9_M_COMM_H";
    //         sb.Append(" where 1=1";

    //        if (SYS_CD != "")
    //        {
    //             sb.Append(" and SYS_CD = @SYS_CD ";
    //            ht.Add("@SYS_CD", SYS_CD);
    //        }

    //        return dbConn.Query(sb, ht);
    //    }
    //    catch (Exception)
    //    {

    //        throw;
    //    }
    //}
    public string deleteData(string deleteitem)
    {
        //刪除共用代碼主檔
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.Append("Delete from TB_H_M_DEPT_ACC where  ");
        sb.Append(" ACC_DEPT_NO+ACC_DEPT_NAME = @qdatakey");
        ht.Add("@qdatakey", deleteitem);
        dbConn.ExecuteT(sb, ht, true);

        return "0";
    }
    internal DataTable getExistData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //sb.Append(" Select * from TB_9_M_SYS_M where SYS_ID+MODE_ID = @SYS_ID+@MODE_ID");
            //ht.Add("@SYS_ID", ACC_DEPT_NO);
            //ht.Add("@MODE_ID", ACC_DEPT_NAME);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
    internal void addData_11()
    {
        try
        {

            string Login_id = string.Empty;
            Login_id = SessionHandle.Current.emp_id;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" INSERT INTO TB_S_M_SUBSIDY_DEDUCTIONS_2");
            sb.Append("                          (EMP_ID, EMP_NAME, SALARY_ID, START_DT, END_DATE, AMOUNT, IS_PLUS, IS_TAX, APPROVE_BY, APPROVE_DT");
            sb.Append("                          , REMARK, CREATED_BY, CREATED_DT, UPDATED_BY, UPDATED_DT, FUNC_ID)");
            sb.Append(" VALUES        (@EMP_ID, @EMP_NAME, @SALARY_ID, @START_DT, @END_DATE, @AMOUNT, @IS_PLUS, @IS_TAX, @APPROVE_BY, GETDATE()");
            sb.Append(" , @REMARK, @CREATED_BY, GETDATE(), @UPDATED_BY, GETDATE(), @FUNC_ID)");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@EMP_NAME", EMP_NAME);
            ht.Add("@SALARY_ID", SALARY_ID);
            ht.Add("@START_DT", START_DT_B);
            ht.Add("@END_DATE", END_DATE_B);
            ht.Add("@AMOUNT", CHG_AMT_A);
            ht.Add("@IS_PLUS", IS_PLUS);
            ht.Add("@IS_TAX", IS_TAX);
            ht.Add("@APPROVE_BY", Login_id);
            ht.Add("@REMARK", REMARK);
            ht.Add("@CREATED_BY", Login_id);
            ht.Add("@UPDATED_BY", Login_id);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    internal void addData_12()
    {
        try
        {

            string Login_id = string.Empty;
            Login_id = SessionHandle.Current.emp_id;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" INSERT INTO TB_S_M_SUBSIDY_DEDUCTIONS_D");
            sb.Append("                          (DATA_YM, EMP_ID, EMP_NAME, SALARY_ID, START_DT, AMOUNT, IS_PLUS, IS_TAX");
            sb.Append("                          , REMARK, CREATED_BY, CREATED_DT, UPDATED_BY, UPDATED_DT, FUNC_ID)");
            sb.Append(" VALUES        (@DATA_YM, @EMP_ID, @EMP_NAME, @SALARY_ID, @START_DT, @AMOUNT, @IS_PLUS, @IS_TAX");
            sb.Append(" , @REMARK, @CREATED_BY, GETDATE(), @UPDATED_BY, GETDATE(), @FUNC_ID)");
            ht.Add("@DATA_YM", DATA_YM);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@EMP_NAME", EMP_NAME);
            ht.Add("@SALARY_ID", SALARY_ID);
            ht.Add("@START_DT", START_DT_B);
            ht.Add("@AMOUNT", CHG_AMT_A);
            ht.Add("@IS_PLUS", IS_PLUS);
            ht.Add("@IS_TAX", IS_TAX);
            //ht.Add("@APPROVE_BY", Login_id);
            ht.Add("@REMARK", REMARK);
            ht.Add("@CREATED_BY", Login_id);
            ht.Add("@UPDATED_BY", Login_id);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    internal void updateData_13()
    {
        try
        {
            string Login_id = string.Empty;
            Login_id = SessionHandle.Current.emp_id;

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" UPDATE       TB_S_M_SUBSIDY_DEDU_2_TMP");
            sb.Append(" SET          APPROVE_BY = @APPROVE_BY, APPROVE_DT = GETDATE()");
            sb.Append(" 			, PROCESS_STATUS = 'Y', APP_REMARK = @APP_REMARK, UPDATED_BY = @UPDATED_BY, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID");
            sb.Append(" WHERE		EMP_ID = @EMP_ID and SALARY_ID = @SALARY_ID and START_DT_A = @START_DT_A and SEQ_NO = @SEQ_NO ");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SALARY_ID", SALARY_ID);
            ht.Add("@START_DT_A", START_DT_B);
            ht.Add("@SEQ_NO", SEQ_NO);

            ht.Add("@APPROVE_BY", Login_id);
            ht.Add("@APP_REMARK", APP_REMARK);
            ht.Add("@UPDATED_BY", Login_id);
            ht.Add("@FUNC_ID", FUNC_ID);


            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    internal void updateData_21()
    {
        try
        {
            string Login_id = string.Empty;
            Login_id = SessionHandle.Current.emp_id;


            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" UPDATE       TB_S_M_SUBSIDY_DEDUCTIONS_2");
            sb.Append(" SET                END_DATE = @END_DATE, AMOUNT = @AMOUNT, APPROVE_BY = @APPROVE_BY ");
            sb.Append("                    , APPROVE_DT = GETDATE(), UPDATED_BY = @UPDATED_BY ,UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID, REMARK = @REMARK");
            sb.Append(" WHERE        EMP_ID = @EMP_ID and SALARY_ID = @SALARY_ID and START_DT = @START_DT ");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SALARY_ID", SALARY_ID);
            ht.Add("@START_DT", START_DT_A);
            ht.Add("@END_DATE", END_DATE_A);
            ht.Add("@AMOUNT", CHG_AMT_A);
            ht.Add("@APPROVE_BY", Login_id);
            ht.Add("@REMARK", APP_REMARK);
            ht.Add("@UPDATED_BY", Login_id);
            ht.Add("@FUNC_ID", FUNC_ID);


            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    internal void updateData_22()
    {
        try
        {

            string Login_id = string.Empty;
            Login_id = SessionHandle.Current.emp_id;

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" UPDATE       TB_S_M_SUBSIDY_DEDUCTIONS_D");
            sb.Append(" SET                AMOUNT = @AMOUNT, UPDATED_BY = @UPDATED_BY ,UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID");
            sb.Append(" WHERE        EMP_ID = @EMP_ID and SALARY_ID = @SALARY_ID and START_DT = @START_DT ");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SALARY_ID", SALARY_ID);
            ht.Add("@START_DT", START_DT_A);
            ht.Add("@AMOUNT", CHG_AMT_A);
            ht.Add("@UPDATED_BY", Login_id);
            ht.Add("@FUNC_ID", FUNC_ID);


            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public string deleteData_23_2()
    {
        //刪除共用代碼主檔
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.Append(" Delete from TB_S_M_SUBSIDY_DEDUCTIONS_D   ");
        sb.Append(" where (EMP_ID = @EMP_ID) and (SALARY_ID = @SALARY_ID) and (DATA_YM > @DATA_YM)");
        ht.Add("@EMP_ID", EMP_ID);
        ht.Add("@SALARY_ID", SALARY_ID);
        ht.Add("@DATA_YM", Convert.ToDateTime(END_DATE_A).ToString("yyyyMM"));
        dbConn.ExecuteT(sb, ht, true);

        return "0";
    }
    internal void addData_23_1()
    {
        try
        {

            string Login_id = string.Empty;
            Login_id = SessionHandle.Current.emp_id;

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" INSERT INTO TB_S_M_SUBSIDY_DEDUCTIONS_D");
            sb.Append("               (DATA_YM, EMP_ID, EMP_NAME, SALARY_ID, AMOUNT, START_DT, IS_PLUS, IS_TAX, REMARK, CREATED_BY, CREATED_DT, UPDATED_BY, UPDATED_DT, FUNC_ID)");
            sb.Append(" VALUES        (@DATA_YM, @EMP_ID, @EMP_NAME, @SALARY_ID, @AMOUNT, @START_DT, @IS_PLUS, @IS_TAX, @REMARK, @CREATED_BY, GETDATE(), @UPDATED_BY, GETDATE(),@FUNC_ID)");
            ht.Add("@DATA_YM", DATA_YM);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@EMP_NAME", EMP_NAME);
            ht.Add("@SALARY_ID", SALARY_ID);
            ht.Add("@START_DT", START_DT_B);
            ht.Add("@AMOUNT", CHG_AMT_A);
            ht.Add("@IS_PLUS", IS_PLUS);
            ht.Add("@IS_TAX", IS_TAX);
            ht.Add("@REMARK", REMARK);
            ht.Add("@CREATED_BY", Login_id);
            ht.Add("@UPDATED_BY", Login_id);
            ht.Add("@FUNC_ID", FUNC_ID);


            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    internal void updateData_23_2()
    {
        try
        {

            string Login_id = string.Empty;
            Login_id = SessionHandle.Current.emp_id;

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Update TB_S_M_SUBSIDY_DEDUCTIONS_D ");
            sb.Append(" Set AMOUNT=@AMOUNT, REMARK=@REMARK,UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID");
            sb.Append(" Where START_DT=@START_DT And EMP_ID=@EMP_ID And SALARY_ID=@SALARY_ID And SALARY_STATUS ='N' ");
            
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SALARY_ID", SALARY_ID);
            ht.Add("@START_DT", START_DT_B);
            ht.Add("@AMOUNT", CHG_AMT_A);
            ht.Add("@REMARK", REMARK);
            ht.Add("@UPDATED_BY", Login_id);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    internal void updateData_24()
    {
        try
        {


            string Login_id = string.Empty;
            Login_id = SessionHandle.Current.emp_id;


            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" UPDATE       TB_S_M_SUBSIDY_DEDU_2_TMP");
            sb.Append(" SET          APPROVE_BY = @APPROVE_BY, APPROVE_DT = GETDATE()");
            sb.Append(" 			, PROCESS_STATUS = 'Y', APP_REMARK = @APP_REMARK, UPDATED_BY = @UPDATED_BY, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID");
            sb.Append(" WHERE		EMP_ID = @EMP_ID and SALARY_ID = @SALARY_ID and START_DT_A = @START_DT_A and SEQ_NO = @SEQ_NO ");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SALARY_ID", SALARY_ID);
            ht.Add("@START_DT_A", START_DT_B);
            ht.Add("@SEQ_NO", SEQ_NO);
            ht.Add("@APPROVE_BY", Login_id);
            ht.Add("@APP_REMARK", APP_REMARK);
            ht.Add("@UPDATED_BY", Login_id);
            ht.Add("@FUNC_ID", FUNC_ID);



            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public string deleteData_31()
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.Append(" Delete from TB_S_M_SUBSIDY_DEDUCTIONS_2   ");
        sb.Append(" where (EMP_ID = @EMP_ID) and (SALARY_ID = @SALARY_ID) and (START_DT = @START_DT)");
        sb.Append(" Delete from TB_S_M_SUBSIDY_DEDUCTIONS_D   ");
        sb.Append(" where (EMP_ID = @EMP_ID) and (SALARY_ID = @SALARY_ID) and (START_DT = @START_DT)");
        ht.Add("@EMP_ID", EMP_ID);
        ht.Add("@SALARY_ID", SALARY_ID);
        ht.Add("@START_DT", START_DT_B);
        dbConn.ExecuteT(sb, ht, true);

        return "0";
    }
    internal void updateData_32()
    {
        try
        {

            string Login_id = string.Empty;
            Login_id = SessionHandle.Current.emp_id;

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" UPDATE       TB_S_M_SUBSIDY_DEDU_2_TMP ");
            sb.Append(" SET          APPROVE_BY = @APPROVE_BY, APPROVE_DT = GETDATE()");
            sb.Append(" 			, PROCESS_STATUS = 'Y', APP_REMARK = @APP_REMARK, UPDATED_BY = @UPDATED_BY, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID");
            sb.Append(" WHERE		EMP_ID = @EMP_ID and SALARY_ID = @SALARY_ID and START_DT_A = @START_DT_A and SEQ_NO = @SEQ_NO ");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SALARY_ID", SALARY_ID);
            ht.Add("@START_DT_A", START_DT_B);
            ht.Add("@SEQ_NO", SEQ_NO);

            ht.Add("@APPROVE_BY", Login_id);
            ht.Add("@APP_REMARK", APP_REMARK);
            ht.Add("@UPDATED_BY", Login_id);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    internal void updateData_reject()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" UPDATE       TB_S_M_SUBSIDY_DEDU_2_TMP");
            sb.Append(" SET          APPROVE_BY = @APPROVE_BY, APPROVE_DT = GETDATE()");
            sb.Append(" 			, PROCESS_STATUS = 'B', APP_REMARK = @APP_REMARK, UPDATED_BY = @UPDATED_BY, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID");
            sb.Append(" WHERE		EMP_ID = @EMP_ID and SALARY_ID = @SALARY_ID and START_DT_A = @START_DT_A and SEQ_NO = @SEQ_NO ");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SALARY_ID", SALARY_ID);
            ht.Add("@START_DT_A", START_DT_B);
            ht.Add("@SEQ_NO", SEQ_NO);

            ht.Add("@APPROVE_BY", SessionHandle.Current.emp_id);
            ht.Add("@APP_REMARK", APP_REMARK);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
}