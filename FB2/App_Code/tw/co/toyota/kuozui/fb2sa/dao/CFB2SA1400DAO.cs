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
public class CFB2SA1400DAO : BaseDAO
{
    public string DATA_YEAR { get; set; }
    public string EMP_ID { get; set; }
    public string REMARK { get; set; }
    public string PROCESS_STATUS { get; set; }
    public string APPROVE_STATUS { get; set; }
    public string APPROVE_BY { get; set; }
    public string UPDATED_BY { get; set; }
    public string FUNC_ID { get; set; }

    public string APPROVE_MARK { get; set; }
    public string WS_CD { get; set; }
    public string LEVEL_CD { get; set; }
    public string GRADE_CD { get; set; }
    public string EDUCATION_CD { get; set; }
    public string GRADE_YEAR { get; set; }

    public CFB2SA1400DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression,
                            string data_year, string user_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * from");
            sb.Append(" (select  ROW_NUMBER() OVER(ORDER BY " + sortExpression + "  ) As RowNumber,t.DATA_YEAR,t.PROCESS_STATUS ");
            sb.Append(" ,t.PROCESS_STATUS+ '-'+ d.SUB_DESC as PROCESS_STATUS_DESC,t.START_DT,t.END_DT,t.RELEASE_DT ");
            sb.Append(" ,t.RELEASE_BY,b.EMP_NAME as RELEASE_BY_NAME,t.APPROVE_DT,t.APPROVE_BY,b2.EMP_NAME as APPROVE_BY_NAME ");
            sb.Append(" ,t.APPROVE_STATUS,t.APPROVE_STATUS+ '-'+ p.SUB_DESC as APPROVE_STATUS_DESC ");
            sb.Append(" from TB_S_M_HIRING_SALARY_TMP_H t ");
            sb.Append(" left join TB_H_M_EMP b on b.EMP_ID = t.RELEASE_BY ");
            sb.Append(" left join TB_H_R_HEAD_DEPT c on b.DEPT_NO=c.MNG_DEPT_NO ");
            sb.Append(" left join TB_9_M_COMM_D d on d.SYS_CD='SA' and d.MAIN_CD='PROCESS_STATUS' and  t.PROCESS_STATUS = d.SUB_CD ");
            sb.Append(" left join TB_9_M_COMM_D p on p.SYS_CD='SA' and p.MAIN_CD='APPROVE_STATUS' and  t.APPROVE_STATUS = p.SUB_CD ");
            sb.Append(" left join TB_H_M_EMP b2 on b2.EMP_ID = t.APPROVE_BY ");
            sb.Append(" where  c.EMP_ID=@USER_ID and t.RELEASE_BY<>'' ");

            ht.Add("@USER_ID", user_id);
            //A.若初任薪年度<>'' ==>  and  t.DATA_YEAR = 畫面.初任薪年度. 
            if (data_year != "" && data_year != null)
            {
                sb.Append(" and t.DATA_YEAR = @DATA_YEAR ");
                ht.Add("@DATA_YEAR", data_year);
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

    public int getCount(int startRowIndex, int maximumRows,
                            string data_year, string user_id)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select count(*) total_record ");
            sb.Append(" from TB_S_M_HIRING_SALARY_TMP_H t ");
            sb.Append(" left join TB_H_M_EMP b on b.EMP_ID = t.RELEASE_BY ");
            sb.Append(" left join TB_H_R_HEAD_DEPT c on b.DEPT_NO=c.MNG_DEPT_NO ");
            sb.Append(" left join TB_9_M_COMM_D d on d.SYS_CD='SA' and d.MAIN_CD='PROCESS_STATUS' and  t.PROCESS_STATUS = d.SUB_CD ");
            sb.Append(" left join TB_9_M_COMM_D p on p.SYS_CD='SA' and p.MAIN_CD='APPROVE_STATUS' and  t.APPROVE_STATUS = p.SUB_CD ");
            sb.Append(" left join TB_H_M_EMP b2 on b2.EMP_ID = t.APPROVE_BY ");
            sb.Append(" where  c.EMP_ID=@USER_ID and t.RELEASE_BY<>'' ");

            ht.Add("@USER_ID", user_id);
            //A.若初任薪年度<>'' ==>  and  t.DATA_YEAR = 畫面.初任薪年度. 
            if (data_year != "" && data_year != null)
            {
                sb.Append(" and t.DATA_YEAR = @DATA_YEAR ");
                ht.Add("@DATA_YEAR", data_year);
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

    public DataTable getHIRING_SALARY_TMP_HData(string data_year)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select  t.DATA_YEAR,t.PROCESS_STATUS,t.PROCESS_STATUS+ '-'+ d.SUB_DESC as PROCESS_STATUS_DESC,t.START_DT,t.END_DT ");
            sb.Append(" ,t.RELEASE_DT,t.RELEASE_BY,b.EMP_NAME as RELEASE_BY_NAME,t.APPROVE_DT,t.APPROVE_BY,b2.EMP_ID + '-' + b2.EMP_NAME as APPROVE_BY_NAME ");
            sb.Append(" ,t.APPROVE_STATUS,t.APPROVE_STATUS+ '-'+ p.SUB_DESC as APPROVE_STATUS_DESC,t.REMARK ");
            sb.Append(" from TB_S_M_HIRING_SALARY_TMP_H t ");
            sb.Append(" left join TB_H_M_EMP b on b.EMP_ID = t.RELEASE_BY ");
            sb.Append(" left join TB_9_M_COMM_D d on d.SYS_CD='SA' and d.MAIN_CD='PROCESS_STATUS' and  t.PROCESS_STATUS = d.SUB_CD ");
            sb.Append(" left join TB_9_M_COMM_D p on p.SYS_CD='SA' and p.MAIN_CD='APPROVE_STATUS' and  t.APPROVE_STATUS = p.SUB_CD ");
            sb.Append(" left join TB_H_M_EMP b2 on b2.EMP_ID = t.APPROVE_BY ");
            sb.Append(" where  t.DATA_YEAR = @DATA_YEAR ");

            ht.Add("@DATA_YEAR", data_year);
            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getDetailData1(int startRowIndex, int maximumRows, string sortExpression,
                        string data_year)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * from");
            sb.Append(" (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber ");
            sb.Append(" ,t.APPROVE_MARK ,t.WS_CD,t.WS_CD + '-' + d.SUB_DESC as WS_CD_DESC ,t.LEVEL_CD,t.GRADE_CD,t.EDUCATION_CD ");
            sb.Append(" ,t.EDUCATION_CD + '-' + p.SUB_DESC as EDUCATION_CD_DESC ,t.GRADE_YEAR,t.LEVEL_PAY1,t.LEVEL_PAY2,t.LEVEL_PAY3 ");
            sb.Append(" from TB_S_M_HIRING_SALARY_TMP_D t ");
            sb.Append(" left join TB_9_M_COMM_D d on  d.SYS_CD='HB' and   d.MAIN_CD='WS_CD' and  t.WS_CD = d.SUB_CD ");
            sb.Append(" left join TB_9_M_COMM_D p on   p.SYS_CD='HB' and  p.MAIN_CD='EDUCATION_CD' and  t.EDUCATION_CD = p.SUB_CD ");
            sb.Append(" where 1=1 and t.DATA_YEAR = @DATA_YEAR ");

            ht.Add("@DATA_YEAR", data_year);

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

    public int getDetailCount1(int startRowIndex, int maximumRows, string data_year)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select count(*) total_record ");
            sb.Append(" from TB_S_M_HIRING_SALARY_TMP_D t ");
            sb.Append(" left join TB_9_M_COMM_D d on  d.SYS_CD='HB' and   d.MAIN_CD='WS_CD' and  t.WS_CD = d.SUB_CD ");
            sb.Append(" left join TB_9_M_COMM_D p on   p.SYS_CD='HB' and  p.MAIN_CD='EDUCATION_CD' and  t.EDUCATION_CD = p.SUB_CD ");
            sb.Append(" where 1=1 and t.DATA_YEAR = @DATA_YEAR ");

            ht.Add("@DATA_YEAR", data_year);

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

    public DataTable getDetailData2(int startRowIndex, int maximumRows, string sortExpression,
                        string data_year)
    {
        try
        {
            if (sortExpression.Contains("START_SALARY"))
                sortExpression = sortExpression.Replace("START_SALARY", "t.START_SALARY");
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * from (");
            sb.Append(" select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber ");
            sb.Append(" ,t.APPROVE_MARK ,t.DATA_YEAR,t.WS_CD,t.WS_CD + '-' + d.SUB_DESC as WS_CD_DESC,t.LEVEL_CD,t.GRADE_CD,t.EDUCATION_CD ");
            sb.Append(" ,t.EDUCATION_CD + '-' + p.SUB_DESC as EDUCATION_CD_DESC,t.START_SALARY,t.BASE_YEAR,t.START_YEAR,t.END_YEAR ");
            sb.Append(" ,t.BASE_RANGE,t.FEMALE_RANGE,t.ARMY_RANGE ");
            sb.Append(" from TB_S_M_HIRING_SALARY_SET t ");
            sb.Append(" left join TB_9_M_COMM_D d on  d.SYS_CD='HB' and   d.MAIN_CD='WS_CD' and  t.WS_CD = d.SUB_CD ");
            sb.Append(" left join TB_9_M_COMM_D p on   p.SYS_CD='HB' and  p.MAIN_CD='EDUCATION_CD' and  t.EDUCATION_CD = p.SUB_CD ");
            sb.Append(" where 1=1 and t.DATA_YEAR=@DATA_YEAR ");

            ht.Add("@DATA_YEAR", data_year);

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

    public int getDetailCount2(int startRowIndex, int maximumRows, string data_year)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select count(*) total_record ");
            sb.Append(" from TB_S_M_HIRING_SALARY_SET t ");
            sb.Append(" left join TB_9_M_COMM_D d on  d.SYS_CD='HB' and   d.MAIN_CD='WS_CD' and  t.WS_CD = d.SUB_CD ");
            sb.Append(" left join TB_9_M_COMM_D p on   p.SYS_CD='HB' and  p.MAIN_CD='EDUCATION_CD' and  t.EDUCATION_CD = p.SUB_CD ");
            sb.Append(" where 1=1 and t.DATA_YEAR=@DATA_YEAR ");

            ht.Add("@DATA_YEAR", data_year);

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

    public void updateHIRING_SALARY_TMP_H()
    {
        try
        {
            //修改 敘薪資料檔
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_S_M_HIRING_SALARY_TMP_H ");
            sb.Append(" set PROCESS_STATUS=@PROCESS_STATUS,APPROVE_BY = @APPROVE_BY,APPROVE_STATUS=@APPROVE_STATUS ");
            if (APPROVE_BY != "")
                sb.Append(" ,APPROVE_DT=GETDATE() ");
            else
            {
                sb.Append(" ,APPROVE_DT=null,RELEASE_BY='',RELEASE_DT=null  ");
            }

            sb.Append(" ,REMARK=@REMARK,UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID");
            sb.Append(" where DATA_YEAR=@DATA_YEAR ");

            ht.Add("@DATA_YEAR", DATA_YEAR);
            ht.Add("@PROCESS_STATUS", PROCESS_STATUS);
            ht.Add("@APPROVE_BY", APPROVE_BY);
            ht.Add("@APPROVE_STATUS", APPROVE_STATUS);
            ht.Add("@REMARK", REMARK);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public void insertHIRING_SALARY()
    {
        try
        {
            //新增 敘薪資料檔
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" insert into TB_S_M_HIRING_SALARY ");
            sb.Append(" select a.DATA_YEAR,a.WS_CD,a.LEVEL_CD,a.GRADE_CD,a.EDUCATION_CD,a.GRADE_YEAR ");
            sb.Append(" ,a.LEVEL_PAY1,a.LEVEL_PAY2,a.LEVEL_PAY3,b.START_DT,'9999/12/31',b.APPROVE_DT ");
            sb.Append(" ,@APPROVE_BY,GETDATE(),@FUNC_ID ");
            sb.Append(" from TB_S_M_HIRING_SALARY_TMP_D a ");
            sb.Append(" left join TB_S_M_HIRING_SALARY_TMP_H b on a.DATA_YEAR = b.DATA_YEAR ");
            sb.Append(" where a.DATA_YEAR = @DATA_YEAR");

            ht.Add("@DATA_YEAR", DATA_YEAR);
            ht.Add("@APPROVE_BY", APPROVE_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void updateHIRING_SALARY_EFFECT_EDT()
    {
        try
        {
            //修改 敘薪資料暫存檔
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_S_M_HIRING_SALARY ");
            sb.Append(" set EFFECT_EDT = DATEADD(Day,-1,b.START_DT) ");
            sb.Append(" from TB_S_M_HIRING_SALARY a ");
            sb.Append(" left join TB_S_M_HIRING_SALARY_TMP_H b on @DATA_YEAR = b.DATA_YEAR ");
            sb.Append(" where a.DATA_YEAR = Convert(int,@DATA_YEAR)-1");

            ht.Add("@DATA_YEAR", DATA_YEAR);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void updateHIRING_SALARY_TMP_D()
    {
        try
        {
            //註記 更新 初任薪試算明細檔
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_S_M_HIRING_SALARY_TMP_D ");
            sb.Append(" set APPROVE_MARK=@APPROVE_MARK");
            sb.Append(" where DATA_YEAR=@DATA_YEAR and WS_CD=@WS_CD and LEVEL_CD=@LEVEL_CD and GRADE_CD=@GRADE_CD ");
            sb.Append(" and EDUCATION_CD=@EDUCATION_CD and GRADE_YEAR=@GRADE_YEAR ");

            ht.Add("@DATA_YEAR", DATA_YEAR);
            ht.Add("@WS_CD", WS_CD);
            ht.Add("@LEVEL_CD", LEVEL_CD);
            ht.Add("@GRADE_CD", GRADE_CD);
            ht.Add("@EDUCATION_CD", EDUCATION_CD);
            ht.Add("@GRADE_YEAR", GRADE_YEAR);
            ht.Add("@APPROVE_MARK", APPROVE_MARK);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void updateHIRING_SALARY_SET()
    {
        try
        {
            //註記 更新 初任薪試算設定檔
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_S_M_HIRING_SALARY_SET ");
            sb.Append(" set APPROVE_MARK=@APPROVE_MARK");
            sb.Append(" where DATA_YEAR=@DATA_YEAR and WS_CD=@WS_CD and LEVEL_CD=@LEVEL_CD and GRADE_CD=@GRADE_CD ");
            sb.Append(" and EDUCATION_CD=@EDUCATION_CD ");

            ht.Add("@DATA_YEAR", DATA_YEAR);
            ht.Add("@WS_CD", WS_CD);
            ht.Add("@LEVEL_CD", LEVEL_CD);
            ht.Add("@GRADE_CD", GRADE_CD);
            ht.Add("@EDUCATION_CD", EDUCATION_CD);
            ht.Add("@APPROVE_MARK", APPROVE_MARK);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
}