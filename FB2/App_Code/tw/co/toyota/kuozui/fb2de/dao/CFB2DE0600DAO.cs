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
public class CFB2DE0600DAO : BaseDAO
{
    public string CREATED_BY { get; set; }
    public string UPDATED_BY { get; set; }
    public string FUNC_ID { get; set; }

    public string MONEY { get; set; }
    public string CLOCK_NO { get; set; }   

	public CFB2DE0600DAO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}
    //Gridview 查詢資料
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string CLOCK_NO)
    {
        try
        {
            if (sortExpression.Contains("CLOCK_NO"))
            {
                sortExpression = sortExpression.Replace("CLOCK_NO", "a.CLOCK_NO");
            }

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" a.CLOCK_NO,a.PRICE,b.clock_desc,b.CLOCK_IP from TB_D_M_CLOCK_MONEY a");
            sb.Append(" left join TB_D_M_CLOCK b on a.CLOCK_NO = b.CLOCK_NO");           
            sb.Append(" where 1=1");
            if (CLOCK_NO != "-1")
            {
                sb.Append(" and a.CLOCK_NO = @CLOCK_NO ");
                ht.Add("@CLOCK_NO", CLOCK_NO);
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
    //Gridview 查詢總筆數
    public int getCount(int startRowIndex, int maximumRows, string CLOCK_NO)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select COUNT(*) total_record ");
            sb.Append(" from TB_D_M_CLOCK_MONEY a");
            sb.Append(" left join TB_D_M_CLOCK b on a.CLOCK_NO = b.CLOCK_NO");
            sb.Append(" where 1=1");
            if (CLOCK_NO != "-1")
            {
                sb.Append(" and a.CLOCK_NO = @CLOCK_NO ");
                ht.Add("@CLOCK_NO", CLOCK_NO);
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

    internal DataTable getCLOCKData(string CLOCK_NO)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(@" 
                        select a.CLOCK_NO,a.PRICE,b.clock_desc,b.CLOCK_IP from TB_D_M_CLOCK_MONEY a
                        left join TB_D_M_CLOCK b on a.CLOCK_NO = b.CLOCK_NO 
                        where 1=1
                      ");
            if (CLOCK_NO != "-1")
            {
                sb.Append(" and a.CLOCK_NO = @CLOCK_NO ");
                ht.Add("@CLOCK_NO", CLOCK_NO);
            }           
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
            sb.Append(" set PRICE = @MONEY,UPDATED_BY = @UPDATED_BY, UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID");
            sb.Append(" where CLOCK_NO = @CLOCK_NO");
            ht.Add("@MONEY", MONEY);
            ht.Add("@CLOCK_NO", CLOCK_NO);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", "FB2DE060");  
                       
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
            //寫log
            sb.Append(" update TB_D_M_CLOCK_MONEY  SET UPDATED_BY = @UPDATED_BY ,UPDATED_DT = getdate() , FUNC_ID = @FUNC_ID ");
            sb.Append(" where CLOCK_NO = @CLOCK_NO ");

            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2DE060");  

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