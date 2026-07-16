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
public class CFB2SA1510DAO : BaseDAO
{
    public CFB2SA1510DAO()
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

            sb.Append(@" d.DATA_YEAR,d.EMP_ID,e.EMP_NAME,case when t2.MAIL_YN = 'Y' then CONVERT(varchar,t2.SEND_DT,111) else null end SEND_DT 
	                     from TB_S_M_HIRING_SALARY_MEM_D d 
	                     inner join (	
		                     select EMP_ID,MAX(SEND_DT)SEND_DT,MAIL_YN from (
			                     select distinct t.* from TB_S_M_HIRING_MAIL_BAT_H h
			                     left join (
				                     SELECT EMP_ID,MAX(SEND_DT)as SEND_DT,MAIL_YN FROM TB_S_M_HIRING_MAIL_BAT_D group by EMP_ID,SEND_DT,MAIL_YN 
			                     )t on h.SEND_DT = t.SEND_DT --and d.EMP_ID = t.EMP_ID	
			                     )t1
			                     GROUP BY EMP_ID,MAIL_YN
	                     )t2 on d.EMP_ID = t2.EMP_ID
	                     left join TB_H_M_EMP e on d.EMP_ID = e.EMP_ID 
            ");

            //sb.Append(" d.DATA_YEAR,d.EMP_ID,e.EMP_NAME,case when t.MAIL_YN = 'Y' then CONVERT(varchar,t.SEND_DT,111) else null end SEND_DT");
            //sb.Append(" from TB_S_M_HIRING_SALARY_MEM_D d");
            //sb.Append(" left join TB_H_M_EMP e on d.EMP_ID = e.EMP_ID");
            //sb.Append(" left join TB_S_M_HIRING_MAIL_BAT_H h");
            //sb.Append(" on d.DATA_YEAR = h.EFFECT_YM");
            //sb.Append(" left join (");
            //sb.Append(" SELECT EMP_ID,MAX(SEND_DT)as SEND_DT,MAIL_YN FROM TB_S_M_HIRING_MAIL_BAT_D group by EMP_ID,SEND_DT,MAIL_YN");
            //sb.Append(" )t on h.SEND_DT = t.SEND_DT and d.EMP_ID = t.EMP_ID");

            sb.Append(" where d.DATA_YEAR = @DATA_YEAR");
            if (EMP_ID != "")
            {
                sb.Append(" and d.EMP_ID like @EMP_ID ");
                ht.Add("@EMP_ID", EMP_ID + "%");
            }
            if (SEND_DT != "")
            {
                sb.Append(" and t2.SEND_DT = @SEND_DT ");
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
            sb.Append(" from TB_S_M_HIRING_SALARY_MEM_D d");
            sb.Append(@" inner join (	
		                     select EMP_ID,MAX(SEND_DT)SEND_DT,MAIL_YN from (
			                     select distinct t.* from TB_S_M_HIRING_MAIL_BAT_H h
			                     left join (
				                     SELECT EMP_ID,MAX(SEND_DT)as SEND_DT,MAIL_YN FROM TB_S_M_HIRING_MAIL_BAT_D group by EMP_ID,SEND_DT,MAIL_YN 
			                     )t on h.SEND_DT = t.SEND_DT --and d.EMP_ID = t.EMP_ID	
			                     )t1
			                     GROUP BY EMP_ID,MAIL_YN
	                     )t2 on d.EMP_ID = t2.EMP_ID
	                     left join TB_H_M_EMP e on d.EMP_ID = e.EMP_ID  
            ");

            //sb.Append(" left join TB_H_M_EMP e on d.EMP_ID = e.EMP_ID");
            //sb.Append(" left join TB_S_M_HIRING_MAIL_BAT_H h");
            //sb.Append(" on d.DATA_YEAR = h.EFFECT_YM");
            //sb.Append(" left join (");
            //sb.Append(" SELECT EMP_ID,MAX(SEND_DT)as SEND_DT,MAIL_YN FROM TB_S_M_HIRING_MAIL_BAT_D group by EMP_ID,SEND_DT,MAIL_YN");
            //sb.Append(" )t on h.SEND_DT = t.SEND_DT and d.EMP_ID = t.EMP_ID");

            sb.Append(" where d.DATA_YEAR = @DATA_YEAR");
            if (EMP_ID != "")
            {
                sb.Append(" and d.EMP_ID like @EMP_ID ");
                ht.Add("@EMP_ID", EMP_ID + "%");
            }
            if (SEND_DT != "")
            {
                sb.Append(" and t2.SEND_DT = @SEND_DT ");
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
    internal DataTable checkSign(string DATA_YEAR)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select count(*) cnt from TB_S_M_HIRING_SALARY_MEM_D a where a.DATA_YEAR=@DATA_YEAR");
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
            sb.Append(" select t.EMP_ID,t.EMP_NAME from TB_S_M_HIRING_SALARY_MEM_D t left join TB_H_M_EMP t2 on t.EMP_ID=t2.EMP_ID ");
            sb.Append(" where t.DATA_YEAR=@DATA_YEAR  and isnull(t2.SALARY_EMAIL,'')='' and isnull(t2.JPN_CD,'')=''");
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
            sb.Append(" select t.EMP_ID,t2.SALARY_EMAIL from TB_S_M_HIRING_SALARY_MEM_D t  ");
            sb.Append(" left join TB_H_M_EMP t2 on t.EMP_ID=t2.EMP_ID where t.DATA_YEAR=@DATA_YEAR  and isnull(t2.SALARY_EMAIL,'')<>'' and isnull(JPN_CD,'') = '' ");
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
                sb.Append(" select b.EMP_ID,b.ABILITY_PAY_A,b.ABILITY_PAY_B,");
                sb.Append(" ISNULL(SUM(d.ABILITY_REPAY),0)ABILITY_REPAY,ISNULL(SUM(d.NO_TAX_OVERTIME_REPAY),0)NO_TAX_OVERTIME_REPAY,");
                sb.Append(" ISNULL(SUM(d.TAX_OVERTIME_REPAY),0)TAX_OVERTIME_REPAY, ISNULL(SUM(d.LEAVE_REPAY),0)LEAVE_REPAY,ISNULL(SUM(d.WORK_SHIFT_REPAY),0)WORK_SHIFT_REPAY,");
                sb.Append(" b.EMP_NAME,c.LICENSE_ID,b.DATA_YEAR,c.DEPT_NO,c.DEPT_NAME_20+c.DEPT_NAME_30+c.DEPT_NAME_40 as DEPT_NAME,b.LEVEL_CD,b.GRADE_CD");
                sb.Append(" from TB_S_M_HIRING_SALARY_MEM_D b");
                sb.Append(" left join TB_S_M_HIRING_SALARY_REPAY_MEM d on b.EMP_ID = d.EMP_ID and b.DATA_YEAR = d.DATA_YEAR");
                sb.Append(" left join VW_H_EMP_DATA c on b.EMP_ID=c.EMP_ID");
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
                sb.Append(" group by b.EMP_ID,b.ABILITY_PAY_A,b.ABILITY_PAY_B,b.EMP_NAME,c.LICENSE_ID,b.DATA_YEAR, c.DEPT_NO,c.DEPT_NAME_20+c.DEPT_NAME_30+c.DEPT_NAME_40,b.LEVEL_CD,b.GRADE_CD");
               
            }
            else
            {
                sb.Append(" select t.EMP_ID,t.SENDTO_MAIL,t.CONTACT_TO,");
                sb.Append(" SUM(d.ABILITY_REPAY)ABILITY_REPAY,SUM(d.NO_TAX_OVERTIME_REPAY)NO_TAX_OVERTIME_REPAY,SUM(d.TAX_OVERTIME_REPAY)TAX_OVERTIME_REPAY,");
                sb.Append(" SUM(d.LEAVE_REPAY)LEAVE_REPAY,SUM(d.WORK_SHIFT_REPAY)WORK_SHIFT_REPAY,b.ABILITY_PAY_A,b.ABILITY_PAY_B,");
                sb.Append(" b.EMP_NAME,replace(convert(char(10),t.SEND_DT,120),'-','/') SEND_DT,c.LICENSE_ID,b.DATA_YEAR,c.DEPT_NO,c.DEPT_NAME_20+c.DEPT_NAME_30+c.DEPT_NAME_40 as DEPT_NAME,b.LEVEL_CD,b.GRADE_CD");
                sb.Append(" from ");
                sb.Append("  (select a.EMP_ID ,a.EMAIL,a.CC_EMAIL,a1.MAIL_TITLE,a1.MAIL_DESC,a1.SENDTO_MAIL,a1.SEND_DT,a1.EFFECT_YM,a1.CONTACT_TO");
                sb.Append("   from TB_S_M_HIRING_MAIL_BAT_D a");
                sb.Append("  left join TB_S_M_HIRING_MAIL_BAT_H a1 on a.SEND_DT=a1.SEND_DT ) t");
                sb.Append(" ");
                sb.Append(" left join TB_S_M_HIRING_SALARY_MEM_D b on t.EMP_ID=b.EMP_ID and t.EFFECT_YM =b.DATA_YEAR");
                sb.Append(" left join TB_S_M_HIRING_SALARY_REPAY_MEM d on t.EMP_ID=d.EMP_ID and t.EFFECT_YM =d.DATA_YEAR");
                sb.Append(" left join VW_H_EMP_DATA c on t.EMP_ID=c.EMP_ID");
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
                sb.Append(" group by t.EMP_ID,t.SENDTO_MAIL,t.MAIL_TITLE,t.MAIL_DESC,t.EMAIL,b.ABILITY_PAY_B,b.ABILITY_PAY_A,t.CC_EMAIL,t.CONTACT_TO");
                sb.Append(" ,b.EMP_NAME,replace(convert(char(10),t.SEND_DT,120),'-','/'),c.LICENSE_ID,b.DATA_YEAR,");
                sb.Append(" c.DEPT_NO,c.DEPT_NAME_20+c.DEPT_NAME_30+c.DEPT_NAME_40,b.LEVEL_CD,b.GRADE_CD ");
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

    internal DataTable before_Pay()
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(@" select LEVEL_PAY as AMOUNT from TB_H_M_LEVEL
                         where START_DT = 
                         (
                            select MAX(START_DT) from TB_H_M_LEVEL
                            where LEVEL_CD =
                            (              
	                            select LEVEL_CD from TB_S_M_HIRING_SALARY_MEM_D
	                            where EMP_ID = @EMP_ID
                            )
                            and END_DT < '9999/12/31'
                         )
                         and LEVEL_CD =
                         (              
	                         select LEVEL_CD from TB_S_M_HIRING_SALARY_MEM_D
	                         where EMP_ID = @EMP_ID
                         )
            ");


//            sb.Append(@" select AMOUNT from TB_S_M_SALARY_TXN
//                         where EFFECT_SDT = (
//	                         select MAX(EFFECT_SDT)EFFECT_SDT from TB_S_M_SALARY_TXN
//	                         where EFFECT_SDT < (
//	                         select MAX(EFFECT_SDT)EFFECT_SDT from TB_S_M_SALARY_TXN
//	                         where EMP_ID = @EMP_ID and SALARY_ID = '1002' --資格俸
//	                         )and EMP_ID = @EMP_ID and SALARY_ID = '1002' --資格俸
//                         )
//                         and EMP_ID = @EMP_ID and SALARY_ID = '1002' --資格俸
//            ");
            ht.Add("@EMP_ID", EMP_ID);

            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }
    internal DataTable after_Pay()
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(@" select LEVEL_PAY as AMOUNT from TB_H_M_LEVEL
                         where LEVEL_CD =
                         (              
	                         select LEVEL_CD from TB_S_M_HIRING_SALARY_MEM_D
	                         where EMP_ID = @EMP_ID
                         )
                         and END_DT = '9999/12/31'
            ");


//            sb.Append(@" select EMP_ID,AMOUNT from TB_S_M_SALARY_TXN
//                         where EFFECT_SDT = (
//	                         select MAX(EFFECT_SDT)EFFECT_SDT from TB_S_M_SALARY_TXN
//	                         where EMP_ID = @EMP_ID and SALARY_ID = '1002' --資格俸
//                         )
//                         and EMP_ID = @EMP_ID and SALARY_ID = '1002' --資格俸
//            ");
            ht.Add("@EMP_ID", EMP_ID);
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
            sb.Append(" delete from TB_S_M_HIRING_MAIL_BAT_H where EFFECT_YM=@DATA_YEAR AND REPLACE(CONVERT(char(10),SEND_DT,120),'-','/') =@SEND_DT");
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
            sb.Append(" delete from TB_S_M_HIRING_MAIL_BAT_D where REPLACE(CONVERT(char(10),SEND_DT,120),'-','/') =@SEND_DT");
            sb.Append(" and QRY_EMP_ID = @EMP_ID");
            
            //if (EMP_ID != "")
            //{
            //    //sb.Append(" and EMP_ID = @EMP_ID");
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

    internal void addMAIL_BAT_H()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" insert into TB_S_M_HIRING_MAIL_BAT_H (SEND_DT,MAIL_TITLE,MAIL_DESC,EFFECT_YM,QRY_EMP_ID,SENDTO_MAIL,CONTACT_TO,CREATED_BY,CREATED_DT,FUNC_ID)");
            sb.Append(" values (@SEND_DT,@MAIL_TITLE,@MAIL_DESC,@EFFECT_YM,@QRY_EMP_ID,@SENDTO_MAIL,@CONTACT_TO,@CREATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@SEND_DT", SEND_DT);
            ht.Add("@MAIL_TITLE", MAIL_TITLE);
            ht.Add("@MAIL_DESC", MAIL_DESC);
            ht.Add("@EFFECT_YM", DATA_YEAR);
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

    internal void addMAIL_BAT_D(string emp_id, string salary_email)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" insert into TB_S_M_HIRING_MAIL_BAT_D (SEND_DT,EMP_ID,EMAIL,MAIL_YN,CC_EMAIL,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID,QRY_EMP_ID)");
            sb.Append(" values (@SEND_DT,@EMP_ID,@EMAIL,'N',@CC_EMAIL_TO,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID,@QRY_EMP_ID)");

            ht.Add("@SEND_DT", SEND_DT);
            ht.Add("@EMP_ID", emp_id);
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