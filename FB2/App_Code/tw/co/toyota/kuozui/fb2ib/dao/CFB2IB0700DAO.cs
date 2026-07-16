using FB2.tw.co.toyota.kuozui.dao;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

/// <summary>
/// CFB2IB0700DAO 的摘要描述
/// </summary>
public class CFB2IB0700DAO : BaseDAO
{

    //screen PARA
    public string PAYMENT_DATE_YM { get; set; }
    public string EMP_ID { get; set; }

    //update PARA
    public string ONE_TIME_AMOUNT { get; set; }
    public string ACCU_AMOUNT { get; set; }
    public string ACCU_OVER_AMOUNT { get; set; }
    public string INS_COST_BASE { get; set; }
    public string INS_COST { get; set; }
    public string UPDATED_BY { get; set; }
    public string FUNC_ID { get; set; }


	public CFB2IB0700DAO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    //Gridview 查詢資料
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string PAYMENT_DATE, string EMP_ID)
    {
        try
        {
            //    if (sortExpression.Contains("EMP_ID"))
            //    {
            //        sortExpression = sortExpression.Replace("EMP_ID", "a.EMP_ID");
            //    }
            //    if (sortExpression.Contains("EMP_NAME"))
            //    {
            //        sortExpression = sortExpression.Replace("EMP_NAME", "a.EMP_NAME");
            //    }

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" CONVERT(char(10),PAYMENT_DATE, 111)PAYMENT_DATE ,EMP_ID,INS_MONTH_AMOUNT,FOUR_AMOUNT,ONE_TIME_AMOUNT,ACCU_AMOUNT,");
            sb.Append(" ACCU_OVER_AMOUNT,INS_COST_BASE,INS_COST,DATA_SOURCE,SALARY_TYPE,SALARY_ID,PAY_KIND");
            sb.Append(" from TB_S_M_INS2_DETAIL");
            sb.Append(" where left(Convert(varchar, PAYMENT_DATE,112),4) =@PAYMENT_DATE");
            sb.Append(" and IS_NOT_CAL = 'N' and EMP_ID = @EMP_ID");

            ht.Add("@PAYMENT_DATE", PAYMENT_DATE);
            ht.Add("@EMP_ID", EMP_ID);

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
    public int getCount(int startRowIndex, int maximumRows, string PAYMENT_DATE, string EMP_ID)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select COUNT(*) total_record ");
            sb.Append(" from TB_S_M_INS2_DETAIL");
            sb.Append(" where left(Convert(varchar, PAYMENT_DATE,112),4) =@PAYMENT_DATE");
            sb.Append(" and IS_NOT_CAL = 'N' and EMP_ID = @EMP_ID");

            ht.Add("@PAYMENT_DATE", PAYMENT_DATE);
            ht.Add("@EMP_ID", EMP_ID);


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

    public string selectPara(string PAYMENT_DATE)
    {
        string st = "";
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" select INS_RATE_PERSON from TB_S_M_INS2_BASIC_SET a");
            sb.Append(" right join( select MAX(YEAR_MONTH) YEAR_MONTH from TB_S_M_INS2_BASIC_SET");
            sb.Append(" where YEAR_MONTH <= left(Convert(varchar,@PAYMENT_DATE ,112),6) )b");
            sb.Append(" on a.YEAR_MONTH = b.YEAR_MONTH");

            ht.Add("@PAYMENT_DATE", PAYMENT_DATE);


            DataTable dt = dbConn.Query(sb, ht);
            if(dt.Rows.Count > 0){
                st = dt.Rows[0]["INS_RATE_PERSON"].ToString();
            }
            return st;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void updateINS2_DETAIL(string PAYMENT_DATE, string DATA_SOURCE, string SALARY_TYPE, string SALARY_ID, string EMP_ID, string PAY_KIND)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" update TB_S_M_INS2_DETAIL");
            sb.Append(" set ONE_TIME_AMOUNT = @ONE_TIME_AMOUNT , ACCU_AMOUNT = @ACCU_AMOUNT,");
            sb.Append(" ACCU_OVER_AMOUNT = @ACCU_OVER_AMOUNT , INS_COST_BASE = @INS_COST_BASE,");
            sb.Append(" INS_COST = @INS_COST , UPDATED_BY = @UPDATED_BY , UPDATED_DT =getdate()");
            sb.Append(" where PAYMENT_DATE = @PAYMENT_DATE and DATA_SOURCE = @DATA_SOURCE and SALARY_TYPE = @SALARY_TYPE");
            sb.Append(" and SALARY_ID = @SALARY_ID and EMP_ID = @EMP_ID and PAY_KIND = @PAY_KIND");
           

            ht.Add("@ONE_TIME_AMOUNT", ONE_TIME_AMOUNT);
            ht.Add("@ACCU_AMOUNT", ACCU_AMOUNT);
            ht.Add("@ACCU_OVER_AMOUNT", ACCU_OVER_AMOUNT);
            ht.Add("@INS_COST_BASE", INS_COST_BASE);
            ht.Add("@INS_COST", INS_COST);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@PAYMENT_DATE", PAYMENT_DATE);
            ht.Add("@DATA_SOURCE", DATA_SOURCE);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            ht.Add("@SALARY_ID", SALARY_ID);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@PAY_KIND", PAY_KIND);    


            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }


}