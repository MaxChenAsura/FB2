using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;

/// <summary>
/// CFB2SA1510DAO 的摘要描述
/// </summary>
public class CFB2SM1400DAO : BaseDAO
{
    public CFB2SM1400DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public string DATA_YEAR { get; set; }
    public string SEND_DT { get; set; }
    public string EMP_ID { get; set; }
    public string MAIL_TITLE { get; set; }
    public string MAIL_DESC { get; set; }
    public string CONTACT_TO { get; set; }
    public string CC_EMAIL { get; set; }
    public string CC_EMAIL_TO { get; set; }
    public string DATA_SEQ { get; set; }
    

    internal DataTable checkEmpEmail()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select a.SALARY_EMAIL from TB_H_M_EMP a where a.EMP_ID=@EMP_ID ");
            ht.Add("@EMP_ID", SessionHandle.Current.emp_id);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
    //Gridview 查詢資料
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string DATA_YEAR, string EMP_ID, string SEND_DT)
    {
        try
        {
            if (sortExpression.Contains("EMP_ID"))
            {
                sortExpression = sortExpression.Replace("EMP_ID", "d.EMP_ID");
            }
            if (sortExpression.Contains("SEND_DT"))
            {
                sortExpression = sortExpression.Replace("SEND_DT", "t.SEND_DT");
            }
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" d.DATA_YEAR,d.EMP_ID,e.EMP_NAME,case when t.MAIL_YN = 'Y' then CONVERT(varchar,t.SEND_DT,111) else null end SEND_DT");
            sb.Append(" from TB_S_M_PROMOTION d");
            sb.Append(" left join TB_H_M_EMP e on d.EMP_ID = e.EMP_ID");
            sb.Append(" left join TB_S_M_PROMOTION_MAIL_BAT_H h");
            sb.Append(" on d.DATA_YEAR = h.DATA_YEAR  ");
            if (EMP_ID != "")
            {
                sb.Append(" and h.QRY_EMP_ID = d.emp_id");                
            }
            sb.Append(" left join (");
            sb.Append(" SELECT EMP_ID,MAX(SEND_DT)as SEND_DT,MAIL_YN FROM TB_S_M_PROMOTION_MAIL_BAT_D group by EMP_ID,SEND_DT,MAIL_YN");
            sb.Append(" )t on h.SEND_DT = t.SEND_DT and d.EMP_ID = t.EMP_ID");
            sb.Append(" where d.DATA_YEAR = @DATA_YEAR and d.PROMOTION_TYPE ='0'");
            if (EMP_ID != "")
            {
                sb.Append(" and d.EMP_ID like @EMP_ID ");
                ht.Add("@EMP_ID", EMP_ID + "%");
            }
            if (SEND_DT != "")
            {
                sb.Append(" and t.SEND_DT = @SEND_DT ");
                ht.Add("@SEND_DT", SEND_DT);
            }
            sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar)");

            ht.Add("@DATA_YEAR", DATA_YEAR);
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
    public int getCount(int startRowIndex, int maximumRows, string DATA_YEAR, string EMP_ID, string SEND_DT)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select COUNT(*) total_record");
            sb.Append(" from TB_S_M_PROMOTION d");
            sb.Append(" left join TB_H_M_EMP e on d.EMP_ID = e.EMP_ID");
            sb.Append(" left join TB_S_M_PROMOTION_MAIL_BAT_H h");
            sb.Append(" on d.DATA_YEAR = h.DATA_YEAR  ");
            if (EMP_ID != "")
            {
                sb.Append(" and h.QRY_EMP_ID = d.emp_id");
            }
            sb.Append(" left join (");
            sb.Append(" SELECT EMP_ID,MAX(SEND_DT)as SEND_DT,MAIL_YN FROM TB_S_M_PROMOTION_MAIL_BAT_D group by EMP_ID,SEND_DT,MAIL_YN");
            sb.Append(" )t on h.SEND_DT = t.SEND_DT and d.EMP_ID = t.EMP_ID");
            sb.Append(" where d.DATA_YEAR = @DATA_YEAR and d.PROMOTION_TYPE ='0'");
            if (EMP_ID != "")
            {
                sb.Append(" and d.EMP_ID like @EMP_ID ");
                ht.Add("@EMP_ID", EMP_ID + "%");
            }
            if (SEND_DT != "")
            {
                sb.Append(" and t.SEND_DT = @SEND_DT ");
                ht.Add("@SEND_DT", SEND_DT);
            }

            ht.Add("@DATA_YEAR", DATA_YEAR);            

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
    internal DataTable checkCCEmail()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select SALARY_EMAIL from TB_H_M_EMP  where EMP_ID=@EMP_ID ");
            ht.Add("@EMP_ID", CC_EMAIL);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
    internal DataTable checkSign(string DATA_YEAR, string DATA_SEQ)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select PROCESS_STATUS,RELEASE_DT from TB_S_M_PROMOTION_H a");
            sb.Append(" where a.DATA_YEAR=@DATA_YEAR and a.DATA_SEQ = @DATA_SEQ and a.PROMOTION_TYPE ='0'");

            ht.Add("@DATA_SEQ", DATA_SEQ);
            ht.Add("@DATA_YEAR", DATA_YEAR);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable checkRecvEmail(string DATA_YEAR, string EMP_ID)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("  select t.EMP_ID,t.EMP_NAME from TB_S_M_PROMOTION t left join TB_H_M_EMP t2 on t.EMP_ID=t2.EMP_ID");
            sb.Append(" where t.DATA_YEAR=@DATA_YEAR  and t.PROMOTION_TYPE ='0'  and isnull(t2.SALARY_EMAIL,'')=''");
            if (EMP_ID != "")
            {
                sb.Append(" and t.EMP_ID = @EMP_ID");
                ht.Add("@EMP_ID", EMP_ID);

            }
            ht.Add("@DATA_YEAR", DATA_YEAR);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getMailList()
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select t.EMP_ID,t2.SALARY_EMAIL from TB_S_M_PROMOTION t   ");
            sb.Append(" left join TB_H_M_EMP t2 on t.EMP_ID=t2.EMP_ID");
            sb.Append(" where t.DATA_YEAR = @DATA_YEAR  and t.PROMOTION_TYPE ='0'  and isnull(t2.SALARY_EMAIL,'')<>''");
            if (EMP_ID != "")
            {
                sb.Append(" and t.EMP_ID = @EMP_ID");
                ht.Add("@EMP_ID", EMP_ID);

            }
            ht.Add("@DATA_YEAR", DATA_YEAR);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable get_PDF_Data()
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            if (SEND_DT == "")
            {
                sb.Append(" select b.EMP_ID,b.DATA_YEAR,CONVERT(varchar,b.EXECUTIVE_DATE,112) EXECUTIVE_DATE,p.PJOB_DESC");
                sb.Append(" ,dbo.FN_S_GET_SALAYRCD_AMT(b.EMP_ID,b.EXECUTIVE_DATE, '1001') as ABILITY_PAY_A");
                sb.Append(" ,dbo.FN_S_GET_SALAYRCD_AMT(b.EMP_ID,b.EXECUTIVE_DATE, '1002') as LEVEL_PAY_A");
                sb.Append(" ,dbo.FN_S_GET_SALAYRCD_AMT(b.EMP_ID,b.EXECUTIVE_DATE, '1003') as PJOB_PAY_A");
                sb.Append(" ,dbo.FN_S_GET_SALAYRCD_AMT(b.EMP_ID,b.EXECUTIVE_DATE, '1004') as PROFESSION_PAY_A");
                sb.Append(" ,dbo.FN_S_GET_PARAMETER('SM', 'PROMOTION_MAIL_USER_DATA') as MAIL_USER");
                sb.Append(" ,b.EMP_NAME,b.DATA_YEAR,b.DEPT_NO,c.DEPT_NAME_20+c.DEPT_NAME_30+c.DEPT_NAME_40 as DEPT_NAME,m.IS_UNION_MEMBER");
                sb.Append(" from TB_S_M_PROMOTION b");
                sb.Append(" left join VW_H_EMP_DATA c on b.EMP_ID=c.EMP_ID");
                sb.Append(" left join TB_H_M_LEVEL m on c.LEVEL_CD = m.LEVEL_CD  and m.END_DT = '9999/12/31'");
                sb.Append(" left join VW_TB_H_M_PJOB p on c.PJOB_CD = p.PJOB_CD");
                sb.Append(" where 1=1 ");
                if (DATA_YEAR != "")
                {
                    sb.Append(" and b.DATA_YEAR = @DATA_YEAR");
                    ht.Add("@DATA_YEAR", DATA_YEAR);
                }
                if (EMP_ID != "")
                {
                    sb.Append(" and b.EMP_ID = @EMP_ID");
                    ht.Add("@EMP_ID", EMP_ID);
                } 
               
            }
            else
            {
                sb.Append(" select t.EMP_ID,t.SENDTO_MAIL,t.MAIL_TITLE,t.MAIL_DESC,t.EMAIL,b.DATA_YEAR,CONVERT(varchar,b.EXECUTIVE_DATE,112) EXECUTIVE_DATE,p.PJOB_DESC");
                sb.Append(" ,dbo.FN_S_GET_SALAYRCD_AMT(t.EMP_ID,b.EXECUTIVE_DATE, '1001') as ABILITY_PAY_A");
                sb.Append(" ,dbo.FN_S_GET_SALAYRCD_AMT(t.EMP_ID,b.EXECUTIVE_DATE, '1002') as LEVEL_PAY_A");
                sb.Append(" ,dbo.FN_S_GET_SALAYRCD_AMT(t.EMP_ID,b.EXECUTIVE_DATE, '1003') as PJOB_PAY_A");
                sb.Append(" ,dbo.FN_S_GET_SALAYRCD_AMT(t.EMP_ID,b.EXECUTIVE_DATE, '1004') as PROFESSION_PAY_A");
                sb.Append(" ,dbo.FN_S_GET_PARAMETER('SM', 'PROMOTION_MAIL_USER_DATA') as MAIL_USER");
                sb.Append(" ,b.EMP_NAME,t.SEND_DT,c.LICENSE_ID,t.DATA_YEAR,b.DEPT_NO,c.DEPT_NAME_20+c.DEPT_NAME_30+c.DEPT_NAME_40 as DEPT_NAME,m.IS_UNION_MEMBER,t.CC_EMAIL,t.CONTACT_TO");
                sb.Append(" from (");
                sb.Append("  select a.EMP_ID ,a1.DATA_YEAR,a.EMAIL,a1.MAIL_TITLE,a1.MAIL_DESC,a1.SENDTO_MAIL,a1.SEND_DT,a.CC_EMAIL,a1.CONTACT_TO");
                sb.Append("  from TB_S_M_PROMOTION_MAIL_BAT_D a");
                sb.Append("  left join TB_S_M_PROMOTION_MAIL_BAT_H a1 on a.SEND_DT=a1.SEND_DT");                
                sb.Append(" ) t");
                sb.Append(" left join TB_S_M_PROMOTION b on t.EMP_ID=b.EMP_ID and t.DATA_YEAR =b.DATA_YEAR and b.PROMOTION_TYPE ='0'");
                sb.Append(" left join VW_H_EMP_DATA c on t.EMP_ID=c.EMP_ID");
                sb.Append(" left join VW_TB_H_M_PJOB p on c.PJOB_CD = p.PJOB_CD");
                sb.Append(" left join TB_H_M_LEVEL m on c.LEVEL_CD = m.LEVEL_CD  and m.END_DT = '9999/12/31'");
                sb.Append("  where 1=1 ");
                if (DATA_YEAR != "")
                {
                    sb.Append(" and b.DATA_YEAR = @DATA_YEAR");
                    ht.Add("@DATA_YEAR", DATA_YEAR);
                }
                if (EMP_ID != "")
                {
                    sb.Append(" and b.EMP_ID = @EMP_ID");
                    ht.Add("@EMP_ID", EMP_ID);
                }
                if (SEND_DT != "")
                {
                    sb.Append(" and t.SEND_DT = @SEND_DT");
                    ht.Add("@SEND_DT", SEND_DT);
                }               
            }

            return dbConn.Query(sb, ht);
            
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getSalaryYM()
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" select (select left(CONVERT(varchar,DATEADD(MM,1,CONVERT(date,dbo.FN_S_SALARY_YM()+'01')),112),6)) salaryYM");    

            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }
    internal DataTable getvaaData()
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" select CODE_VAL1 from TB_9_M_PARAMETER where SYS_CD='SA' and MAIN_CD='HIRING_SALARY_MEM_SDT'  ");

            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }
    internal DataTable getvbbData()
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" select CODE_VAL1 from TB_9_M_PARAMETER where SYS_CD='SA' and MAIN_CD='HIRING_SALARY_CHG_SDT'  ");

            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void deleteMAIL_BAT_H()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" delete from TB_S_M_PROMOTION_MAIL_BAT_H where DATA_YEAR=@DATA_YEAR AND REPLACE(CONVERT(char(10),SEND_DT,120),'-','/') =@SEND_DT");
            sb.Append(" and QRY_EMP_ID = @EMP_ID");
            
            //if (EMP_ID != "")
            //{
            //    sb.Append(" and QRY_EMP_ID = @EMP_ID");
            //    ht.Add("@EMP_ID", EMP_ID);

            //}
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@DATA_YEAR", DATA_YEAR);
            ht.Add("@SEND_DT", SEND_DT);

            dbConn.ExecuteT(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void deleteMAIL_BAT_D()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" delete from TB_S_M_PROMOTION_MAIL_BAT_D where REPLACE(CONVERT(char(10),SEND_DT,120),'-','/') =@SEND_DT");
            sb.Append(" and QRY_EMP_ID = @EMP_ID");
            
            //if (EMP_ID != "")
            //{
            //    sb.Append(" and QRY_EMP_ID = @EMP_ID");
            //    ht.Add("@EMP_ID", EMP_ID);

            //}
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SEND_DT", SEND_DT);

            dbConn.ExecuteT(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }

    }

    internal void addMAIL_BAT_H()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" insert into TB_S_M_PROMOTION_MAIL_BAT_H (SEND_DT,MAIL_TITLE,MAIL_DESC,DATA_YEAR,DATA_SEQ,QRY_EMP_ID,SENDTO_MAIL,CONTACT_TO,CREATED_BY,CREATED_DT,FUNC_ID)");
            sb.Append(" values (@SEND_DT,@MAIL_TITLE,@MAIL_DESC,@DATA_YEAR,@DATA_SEQ,@QRY_EMP_ID,@SENDTO_MAIL,@CONTACT_TO,@CREATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@SEND_DT", SEND_DT);
            ht.Add("@MAIL_TITLE", MAIL_TITLE);
            ht.Add("@MAIL_DESC", MAIL_DESC);
            ht.Add("@DATA_YEAR", DATA_YEAR);
            ht.Add("@DATA_SEQ", DATA_SEQ);
            ht.Add("@QRY_EMP_ID", EMP_ID);
            ht.Add("@SENDTO_MAIL", SENDTO_MAIL);
            ht.Add("@CONTACT_TO", CONTACT_TO);
            ht.Add("@CREATED_BY", CREATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void addMAIL_BAT_D(string empid, string salary_email)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" insert into TB_S_M_PROMOTION_MAIL_BAT_D (SEND_DT,EMP_ID,EMAIL,MAIL_YN,CC_EMAIL,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID,QRY_EMP_ID)");
            sb.Append(" values (@SEND_DT,@EMP_ID,@EMAIL,'N',@CC_EMAIL_TO,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID,@QRY_EMP_ID)");

            ht.Add("@SEND_DT", SEND_DT);
            ht.Add("@EMP_ID", empid);
            ht.Add("@EMAIL", salary_email);
            ht.Add("@CC_EMAIL_TO", CC_EMAIL_TO);
            ht.Add("@CREATED_BY", CREATED_BY);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            ht.Add("@QRY_EMP_ID", EMP_ID);

            dbConn.ExecuteT(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public string CREATED_BY { get; set; }

    public string FUNC_ID { get; set; }

    public string SENDTO_MAIL { get; set; }

    public object UPDATED_BY { get; set; }
}