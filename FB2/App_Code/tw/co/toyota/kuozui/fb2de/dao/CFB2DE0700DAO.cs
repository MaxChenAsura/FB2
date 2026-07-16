using FB2.tw.co.toyota.kuozui.dao;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

/// <summary>
/// CFB2DE0600DAO 的摘要描述
/// </summary>
public class CFB2DE0700DAO : BaseDAO
{
    public string CREATED_BY { get; set; }
    public string UPDATED_BY { get; set; }
    public string FUNC_ID { get; set; }

    public string MONEY { get; set; }
    public string CLOCK_NO { get; set; }   

	public CFB2DE0700DAO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}
    //Gridview 查詢資料
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string EMP_ID, 
        Boolean rb_dt1, Boolean rb_dt2, string WORK_DT, string MANAGER_DT_S, string MANAGER_DT_E)
    {
        try
        {           
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            //sb.Append(" CONVERT(varchar,MANAGER_DT,111)MANAGER_DT,b.CLOCK_DESC,ROUND(isnull(c.PRICE,0),0)PRICE from TB_D_R_RES_ACTURL a");
            sb.Append(" CONVERT(varchar,MANAGER_DT,111)MANAGER_DT,b.CLOCK_DESC,ROUND(isnull(MEAL_AMOUNT,0),0)PRICE from TB_D_R_RES_ACTURL a");
            sb.Append(" left join TB_D_M_CLOCK b on a.CLOCK_NO = b.CLOCK_NO");
            //sb.Append(" left join TB_D_M_CLOCK_MONEY c on a.CLOCK_NO = c.CLOCK_NO");
            sb.Append(" where MEALSHIFT = 'D' and a.EMP_ID = @eid and EXPENSE_CD = '2'");
            if (EMP_ID != "")
            {
                sb.Append(" and a.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", EMP_ID);
            }
            if (rb_dt1 == true && WORK_DT != "")
            {
                sb.Append(" and left(CONVERT(varchar,MANAGER_DT,111),7)= @WORK_DT ");
                ht.Add("@WORK_DT", WORK_DT);
            }
            if (rb_dt2 == true && MANAGER_DT_S != "" && MANAGER_DT_E != "")
            {
                sb.Append(" and CONVERT(varchar,MANAGER_DT,111) between @MANAGER_DT_S and @MANAGER_DT_E ");
                ht.Add("@MANAGER_DT_S", MANAGER_DT_S);
                ht.Add("@MANAGER_DT_E", MANAGER_DT_E);
            }            

            sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar)");

            ht.Add("@eid", SessionHandle.Current.emp_id);
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
    public int getCount(int startRowIndex, int maximumRows, string EMP_ID,
        Boolean rb_dt1, Boolean rb_dt2, string WORK_DT, string MANAGER_DT_S, string MANAGER_DT_E)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select COUNT(*) total_record ");
            sb.Append(" from TB_D_R_RES_ACTURL a");
            sb.Append(" left join TB_D_M_CLOCK b on a.CLOCK_NO = b.CLOCK_NO");
            //sb.Append(" left join TB_D_M_CLOCK_MONEY c on a.CLOCK_NO = c.CLOCK_NO");
            sb.Append(" where MEALSHIFT = 'D' and a.EMP_ID = @eid and EXPENSE_CD = '2'");
            if (EMP_ID != "")
            {
                sb.Append(" and a.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", EMP_ID);
            }
            if (rb_dt1 == true && WORK_DT != "")
            {
                sb.Append(" and left(CONVERT(varchar,MANAGER_DT,111),7)= @WORK_DT ");
                ht.Add("@WORK_DT", WORK_DT);
            }
            if (rb_dt2 == true && MANAGER_DT_S != "" && MANAGER_DT_E != "")
            {
                sb.Append(" and CONVERT(varchar,MANAGER_DT,111) between @MANAGER_DT_S and @MANAGER_DT_E ");
                ht.Add("@MANAGER_DT_S", MANAGER_DT_S);
                ht.Add("@MANAGER_DT_E", MANAGER_DT_E);
            }
            ht.Add("@eid", SessionHandle.Current.emp_id);

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

    internal DataTable getMaxDT()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(@" 
                        --select CONVERT(varchar,MANAGER_DT,111)MANAGER_DT,b.CLOCK_DESC,c.PRICE from TB_D_R_RES_ACTURL a
                        --left join TB_D_M_CLOCK b on a.CLOCK_NO = b.CLOCK_NO 
                        --left join TB_D_M_CLOCK_MONEY c on a.CLOCK_NO = c.CLOCK_NO
                        --where 1=1
                        select left(MAX(CONVERT(varchar,MANAGER_DT,111)),7) MANAGER_DT from TB_D_R_RES_ACTURL 
                      ");
                 
            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getEMPData(string EMP_ID)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" select EMP_NAME, DEPT_NAME");
            sb.Append(" from VW_H_EMP_DATA ");
            sb.Append(" where EMP_ID = @EMP_ID");            

            ht.Add("@EMP_ID", EMP_ID);

            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getTotalAmount(string EMP_ID, Boolean b1, Boolean b2, string WORK_DT, string MANAGER_DT_S, string MANAGER_DT_E)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(@"                        
                        select isnull(SUM(MEAL_AMOUNT),0)MONEY from TB_D_R_RES_ACTURL a
                        --left join TB_D_M_CLOCK_MONEY c on a.CLOCK_NO = c.CLOCK_NO
                        where 1 = 1 and MEALSHIFT = 'D' and EXPENSE_CD = '2'
                      ");
            if (EMP_ID != "")
            {
                sb.Append(" and a.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", EMP_ID);
            }
            if (b1 == true && WORK_DT != "")
            {
                sb.Append(" and left(CONVERT(varchar,MANAGER_DT,111),7)= @WORK_DT ");
                ht.Add("@WORK_DT", WORK_DT);
            }
            if (b2 == true && MANAGER_DT_S != "" && MANAGER_DT_E != "")
            {
                sb.Append(" and CONVERT(varchar,MANAGER_DT,111) between @MANAGER_DT_S and @MANAGER_DT_E ");
                ht.Add("@MANAGER_DT_S", MANAGER_DT_S);
                ht.Add("@MANAGER_DT_E", MANAGER_DT_E);
            }
            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getEveryCount(string EMP_ID, Boolean b1, Boolean b2, string WORK_DT, string MANAGER_DT_S, string MANAGER_DT_E)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(@"                        
                        select count(MEAL_AMOUNT) mcount,isnull(MEAL_AMOUNT,0)PRICE from TB_D_R_RES_ACTURL a
                        --left join TB_D_M_CLOCK_MONEY c on a.CLOCK_NO = c.CLOCK_NO
                        where 1 = 1 and MEALSHIFT = 'D' and EXPENSE_CD = '2'
                      ");
            if (EMP_ID != "")
            {
                sb.Append(" and a.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", EMP_ID);
            }
            if (b1 == true && WORK_DT != "")
            {
                sb.Append(" and left(CONVERT(varchar,MANAGER_DT,111),7)= @WORK_DT ");
                ht.Add("@WORK_DT", WORK_DT);
            }
            if (b2 == true && MANAGER_DT_S != "" && MANAGER_DT_E != "")
            {
                sb.Append(" and CONVERT(varchar,MANAGER_DT,111) between @MANAGER_DT_S and @MANAGER_DT_E ");
                ht.Add("@MANAGER_DT_S", MANAGER_DT_S);
                ht.Add("@MANAGER_DT_E", MANAGER_DT_E);
            }
            sb.Append(" group by MEAL_AMOUNT ");
            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getCLOCK(string CLOCK_NO)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(@" 
                        select clock_desc,CLOCK_IP from TB_D_M_CLOCK                        
                        where 1=1
                      ");
            if (CLOCK_NO != "-1")
            {
                sb.Append(" and CLOCK_NO = @CLOCK_NO ");
                ht.Add("@CLOCK_NO", CLOCK_NO);
            }
            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getALLCLOCKData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(@" 
                        select CLOCK_NO,clock_desc from TB_D_M_CLOCK
                        where CLOCK_TYPE = 'B' and IS_VALID = 'Y' 
                        and CLOCK_NO not in ( select CLOCK_NO from TB_D_M_CLOCK_MONEY )
                      ");
           
            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }

    public void insertCLOCKMONEY()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(@" insert into TB_D_M_CLOCK_MONEY (CLOCK_NO,PRICE,CREATED_BY,CREATED_DT,UPDATED_BY,
                         UPDATED_DT,FUNC_ID) 
                         values (@CLOCK_NO,@MONEY,@CREATED_BY,getdate(),@UPDATED_BY,
                         getdate(),@FUNC_ID)");


            ht.Add("@CLOCK_NO", CLOCK_NO);
            ht.Add("@MONEY", MONEY);
            ht.Add("@CREATED_BY", CREATED_BY);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            
            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }
     
    internal void updateCLOCK_MONEY()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_D_M_CLOCK_MONEY ");
            sb.Append(" set PRICE = @MONEY,UPDATED_BY = @UPDATED_BY, UPDATED_DT = GETDATE()");
            sb.Append(" where CLOCK_NO = @CLOCK_NO");
            ht.Add("@MONEY", MONEY);
            ht.Add("@CLOCK_NO", CLOCK_NO);
            ht.Add("@UPDATED_BY", UPDATED_BY);
                       
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //刪除資料
    public void deleteData(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Delete from TB_D_M_CLOCK_MONEY   ");
            sb.Append(" where CLOCK_NO = @CLOCK_NO;");
            ht.Add("@CLOCK_NO", CLOCK_NO);
            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }



}