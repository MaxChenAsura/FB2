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
/// wfb2SA2200 的摘要描述
/// </summary>
public class CFB2SA2200DAO : BaseDAO
{

    public string EMP_ID { get; set; }
    public string SALARY_ID { get; set; }
    public string EFFECT_SDT { get; set; }
    public string EFFECT_EDT { get; set; }
    public string SEQ_NO { get; set; }
    public string SEQ_NB { get; set; }
    public string AMOUNT { get; set; }
    public string REMARK { get; set; }
    public string APP_REMARK { get; set; }
    public string APPROVE_BY { get; set; }
    public string FUNC_ID { get; set; }

    public string CHG_STATUS { get; set; }
    public string PROCESS_STATUS { get; set; }
    public string EFFECT_SDT_B { get; set; }
    public string CHG_AMT_B { get; set; } //異動前金額

    public CFB2SA2200DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string company_cd,
                            string emp_cd, string emp_id, string emp_name, string user_id)
    {
        try
        {
            if (sortExpression.Contains("EMP_ID"))
                sortExpression = sortExpression.Replace("EMP_ID", "t2.EMP_ID");
            if (sortExpression.Contains("EMP_NAME"))
                sortExpression = sortExpression.Replace("EMP_NAME", "t.EMP_NAME");
            if (sortExpression.Contains("EMP_CD"))
                sortExpression = sortExpression.Replace("EMP_CD", "t.EMP_CD");
            if (sortExpression.Contains("JOIN_DT"))
                sortExpression = sortExpression.Replace("JOIN_DT", "t.JOIN_DT");
            if (sortExpression.Contains("COMPANY_CD"))
                sortExpression = sortExpression.Replace("COMPANY_CD", "t.COMPANY_CD");
            if (sortExpression.Contains("SALARY_ID"))
                sortExpression = sortExpression.Replace("SALARY_ID", "t2.SALARY_ID");
            if (sortExpression.Contains("REMARK"))
                sortExpression = sortExpression.Replace("REMARK", "t2.REMARK");
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * from");
            sb.Append(" (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,t2.EMP_ID,t.EMP_NAME ");
            sb.Append(" ,t.LEVEL_CD,t.GRADE_CD, t.EMP_CD, t.EMP_CD + '-' + e.SUB_DESC as EMP_CD_DESC,t.JOIN_DT,t.COMPANY_CD");
            sb.Append(" ,t.COMPANY_CD+ '-'+  c.COMPANY_SNAME as COMPANY_SNAME,t.PJOB_CD,t.PJOB_CD+ '-'+  f.PJOB_DESC  as PJOB_DESC ");
            sb.Append(" ,t2.SALARY_ID as SALARY_ID ,t2.SALARY_ID + '-'+  IsNull(s.SALARY_NAME,'') as SALARY_NAME,t2.CHG_AMT_A as CHG_AMT_A ");
            sb.Append(" ,t2.EFFECT_SDT_B,t2.EFFECT_SDT_A,t2.EFFECT_EDT_B,t2.EFFECT_EDT_A,t2.SEQ_NO as SEQ_NO,t2.SEQ_NO_B as SEQ_NO_B ,t2.CHG_AMT_B as CHG_AMT_B ");
            sb.Append(" ,t2.PROCESS_STATUS as PROCESS_STATUS,t2.PROCESS_STATUS+ '-'+  d.SUB_DESC as PROCESS_STATUS_DESC ");
            sb.Append(" ,t2.CHG_STATUS as CHG_STATUS,t2.CHG_STATUS+ '-'+  p.SUB_DESC as CHG_STATUS_DESC,t2.APPROVE_DT as APPROVE_DT  ");
            sb.Append(" ,t2.APPROVE_BY as APPROVE_BY ,t2.REMARK as REMARK ,t2.APP_REMARK as APP_REMARK, f.EMP_STATUS, f.EMP_STATUS + '-' + m.SUB_DESC as EMP_STATUS_DESC ");
            sb.Append(" from TB_S_M_SALARY_TXN_TMP t2 ");
            sb.Append(" left join TB_H_M_EMP t on t2.EMP_ID = t.EMP_ID ");
            sb.Append(" left join TB_H_M_COMPANY c on t.COMPANY_CD = c.COMPANY_CD ");
            sb.Append(" left join TB_9_M_COMM_D e on e.SYS_CD='HB' and e.MAIN_CD='EMP_CD' and  t.EMP_CD = e.SUB_CD ");
            sb.Append(" left join VW_H_EMP_DATA f on t2.UPDATED_BY = f.EMP_ID ");
            sb.Append(" left join TB_S_M_SALARY_ITEM s on  t2.SALARY_ID = s.SALARY_ID ");
            sb.Append(" left join TB_9_M_COMM_D d on  d.SYS_CD='SA' and d.MAIN_CD='PROCESS_STATUS' and t2.PROCESS_STATUS = d.SUB_CD ");
            sb.Append(" left join TB_9_M_COMM_D p on  p.SYS_CD='SA' and p.MAIN_CD='CHG_STATUS' and  t2.CHG_STATUS = p.SUB_CD ");
            sb.Append(" left join TB_H_R_HEAD_DEPT b on f.DEPT_NO=b.MNG_DEPT_NO ");
            sb.Append(" left join TB_9_M_COMM_D m on m.SYS_CD='HB' and m.MAIN_CD='EMP_STATUS' and f.EMP_STATUS = m.SUB_CD ");
            sb.Append(" where 1=1 and t2.PROCESS_STATUS ='N' and b.EMP_ID=@USER_ID ");
            ht.Add("@USER_ID", user_id);
            //A.若聘用單位<>'' ==>  and t.COMPANY_CD =畫面.聘用單位. 
            if (company_cd != "-1" && company_cd != null)
            {
                sb.Append(" and t.COMPANY_CD = @COMPANY_CD ");
                ht.Add("@COMPANY_CD", company_cd);
            }
            //B.員工區分<>'' ==>  and t.EMP_CD =畫面.員工區分. 
            if (emp_cd != "-1" && emp_cd != null)
            {
                sb.Append(" and t.EMP_CD = @EMP_CD ");
                ht.Add("@EMP_CD", emp_cd);
            }
            //C.若工號<>'' ==>  and t2.EMP_ID like '畫面.工號%'. 
            if (emp_id != "")
            {
                sb.Append(" and t2.EMP_ID like @EMP_ID ");
                ht.Add("@EMP_ID", emp_id + "%");
            }
            //D.若員工姓名<>'' ==>  and t.EMP_NAME like '畫面.員工姓名%'. 
            if (emp_name != "")
            {
                sb.Append(" and t.EMP_NAME like @EMP_NAME ");
                ht.Add("@EMP_NAME", emp_name + "%");
            }
            sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar)");


            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch
        {
            throw;
        }
    }

    public int getCount(int startRowIndex, int maximumRows, string company_cd,
                            string emp_cd, string emp_id, string emp_name,string user_id)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select count(*) total_record from TB_S_M_SALARY_TXN_TMP t2 ");
            sb.Append(" left join TB_H_M_EMP t on t2.EMP_ID = t.EMP_ID ");
            sb.Append(" left join TB_H_M_COMPANY c on  t.COMPANY_CD = c.COMPANY_CD ");
            sb.Append(" left join TB_9_M_COMM_D e on  e.SYS_CD='HB' and  e.MAIN_CD='WS_CD' and  t.WS_CD = e.SUB_CD ");
            sb.Append(" left join VW_H_EMP_DATA f on t2.UPDATED_BY = f.EMP_ID ");
            sb.Append(" left join TB_S_M_SALARY_ITEM s on  t2.SALARY_ID = s.SALARY_ID ");
            sb.Append(" left join TB_9_M_COMM_D d on  d.SYS_CD='SA' and d.MAIN_CD='PROCESS_STATUS' and t2.PROCESS_STATUS = d.SUB_CD ");
            sb.Append(" left join TB_9_M_COMM_D p on  p.SYS_CD='SA' and p.MAIN_CD='CHG_STATUS' and t2.CHG_STATUS = p.SUB_CD ");
            sb.Append(" left join TB_H_R_HEAD_DEPT b on f.DEPT_NO=b.MNG_DEPT_NO ");
            sb.Append(" where 1=1 and t2.PROCESS_STATUS ='N' and b.EMP_ID=@USER_ID ");
            ht.Add("@USER_ID", user_id);
            //A.若聘用單位<>'' ==>  and t.COMPANY_CD =畫面.聘用單位. 
            if (company_cd != "-1" && company_cd != null)
            {
                sb.Append(" and t.COMPANY_CD = @COMPANY_CD ");
                ht.Add("@COMPANY_CD", company_cd);
            }
            //B.員工區分<>'' ==>  and t.EMP_CD =畫面.員工區分. 
            if (emp_cd != "-1" && emp_cd != null)
            {
                sb.Append(" and t.EMP_CD = @EMP_CD ");
                ht.Add("@EMP_CD", emp_cd);
            }
            //C.若工號<>'' ==>  and t2.EMP_ID like '畫面.工號%'. 
            if (emp_id != "")
            {
                sb.Append(" and t2.EMP_ID like @EMP_ID ");
                ht.Add("@EMP_ID", emp_id + "%");
            }
            //D.若員工姓名<>'' ==>  and t.EMP_NAME like '畫面.員工姓名%'. 
            if (emp_name != "")
            {
                sb.Append(" and t.EMP_NAME like @EMP_NAME ");
                ht.Add("@EMP_NAME", emp_name + "%");
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

    public void insertSALARY_TXN()
    {
        try
        {
            //新增 敘薪資料檔
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Insert into TB_S_M_SALARY_TXN(EMP_ID,SALARY_ID,AMOUNT,EFFECT_SDT,EFFECT_EDT,SEQ_NO, ");
            sb.Append(" APPROVE_BY,APPROVE_DT,REMARK,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" Values (@EMP_ID,@SALARY_ID,@AMOUNT,@EFFECT_SDT,@EFFECT_EDT, ");
            sb.Append(" (select isnull(max(SEQ_NO),0) + 1 from TB_S_M_SALARY_TXN where EMP_ID = @EMP_ID and SALARY_ID = @SALARY_ID and EFFECT_SDT = @EFFECT_SDT),");
            sb.Append(" @APPROVE_BY,GETDATE(),@REMARK,@APPROVE_BY,GETDATE(),@APPROVE_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SALARY_ID", SALARY_ID);
            ht.Add("@AMOUNT", AMOUNT);
            ht.Add("@EFFECT_SDT", EFFECT_SDT);
            ht.Add("@EFFECT_EDT", EFFECT_EDT);
            ht.Add("@REMARK", REMARK);
            ht.Add("@APPROVE_BY", APPROVE_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void updateSALARY_TXN_EFFECT_EDT()
    {
        try
        {
            //修改 敘薪資料檔
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_S_M_SALARY_TXN ");
            sb.Append(" set EFFECT_EDT=@EFFECT_EDT , UPDATED_BY = @UPDATED_BY, UPDATED_DT = GETDATE(),FUNC_ID ='FB2SA220'");
            sb.Append(" where EMP_ID=@EMP_ID and SALARY_ID=@SALARY_ID and EFFECT_EDT='9999/12/31' /*and AMOUNT = @AMOUNT*/");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SALARY_ID", SALARY_ID);
            ht.Add("@EFFECT_EDT", (Convert.ToDateTime(EFFECT_SDT).AddDays(-1)).ToShortDateString());
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            //ht.Add("@AMOUNT", CHG_AMT_B);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public void updateSALARY_TXN()
    {
        try
        {
            //修改 敘薪資料檔
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_S_M_SALARY_TXN ");
            sb.Append(" set EFFECT_SDT=@EFFECT_SDT,EFFECT_EDT=@EFFECT_EDT,AMOUNT=@AMOUNT,APPROVE_BY = @APPROVE_BY,APPROVE_DT=GETDATE() ");
            sb.Append(" , REMARK = @REMARK, UPDATED_BY = @UPDATED_BY, UPDATED_DT = GETDATE(),FUNC_ID ='FB2SA220' ");
            sb.Append(" where EMP_ID=@EMP_ID and SALARY_ID=@SALARY_ID and EFFECT_SDT=@EFFECT_SDT2 and SEQ_NO=@SEQ_NB /* and AMOUNT = @tmpAMOUNT */ ");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SALARY_ID", SALARY_ID);
            ht.Add("@EFFECT_SDT2", EFFECT_SDT_B);
            ht.Add("@SEQ_NB", SEQ_NB);
            ht.Add("@EFFECT_SDT", EFFECT_SDT);
            ht.Add("@EFFECT_EDT", EFFECT_EDT);
            ht.Add("@AMOUNT", AMOUNT);
            ht.Add("@APPROVE_BY", APPROVE_BY);
            ht.Add("@REMARK", REMARK);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            //ht.Add("@tmpAMOUNT", CHG_AMT_B);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public void deleteSALARY_TXN()
    {
        try
        {
            //刪除 敘薪資料檔
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" delete from  TB_S_M_SALARY_TXN ");
            sb.Append(" where EMP_ID=@EMP_ID and SALARY_ID=@SALARY_ID and EFFECT_SDT=@EFFECT_SDT_B and SEQ_NO=@SEQ_NB /*and AMOUNT = @AMOUNT */ ");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SALARY_ID", SALARY_ID);
            ht.Add("@EFFECT_SDT_B", EFFECT_SDT_B);
            ht.Add("@SEQ_NB", SEQ_NB);
            ht.Add("@AMOUNT", CHG_AMT_B);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public void updateSALARY_TXN_TMP()
    {
        try
        {
            //修改 敘薪資料暫存檔
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_S_M_SALARY_TXN_TMP ");
            sb.Append(" set PROCESS_STATUS=@PROCESS_STATUS,APPROVE_BY = @APPROVE_BY,APPROVE_DT=GETDATE(),APP_REMARK=@APP_REMARK ");
            sb.Append(" , UPDATED_BY = @UPDATED_BY, UPDATED_DT = GETDATE(),FUNC_ID ='FB2SA220' ");
            sb.Append(" where EMP_ID=@EMP_ID and SALARY_ID=@SALARY_ID and EFFECT_SDT_B=@EFFECT_SDT_B and SEQ_NO=@SEQ_NO");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SALARY_ID", SALARY_ID);
            ht.Add("@EFFECT_SDT_B", EFFECT_SDT_B);
            ht.Add("@SEQ_NO", SEQ_NO);
            ht.Add("@PROCESS_STATUS", PROCESS_STATUS);
            ht.Add("@APP_REMARK", APP_REMARK);
            ht.Add("@APPROVE_BY", APPROVE_BY);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
}