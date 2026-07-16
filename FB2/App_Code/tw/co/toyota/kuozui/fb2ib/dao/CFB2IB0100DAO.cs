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
public class CFB2IB0100DAO : BaseDAO
{
    public CFB2IB0100DAO()
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
    public string INS_CON_MIN_AMOUNT { get; set; }
    public string INS_CON_MAX_AMOUNT { get; set; }
    //Terry add
    public string INS_SALE_MIN_AMOUNT { get; set; }
    public string INS_SALE_MAX_AMOUNT { get; set; }
    public string INS_STOCK_MIN_AMOUNT { get; set; }
    public string INS_STOCK_MAX_AMOUNT { get; set; }
    public string INS_INTEREST_MIN_AMOUNT { get; set; }
    public string INS_INTEREST_MAX_AMOUNT { get; set; }
  
    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }
    //for查詢欄位
    public string ddl_SYS_ID { get; set; }



    public DataTable getSYS_ID()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select SYS_CD , MAIN_CD , SUB_CD ,SUB_DESC from TB_9_M_COMM_D");
            sb.Append(" where SYS_CD='HB' and MAIN_CD='CAR_TYPE'  ");
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
   
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression)
    {
        try
        {
            if (sortExpression == "")
            {
                sortExpression = "YEAR_MONTH";
            }
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + ") As RowNumber,");//Terry add
            //sb.Append(" (select ROW_NUMBER() OVER(ORDER BY YEAR_MONTH ) As RowNumber,");
            sb.Append("*");
            sb.Append(" from TB_S_M_INS2_BASIC_SET");
            sb.Append(" where 1=1");

            
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
    public int getCount(int startRowIndex, int maximumRows)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) total_record ");
            sb.Append(" from TB_S_M_INS2_BASIC_SET");
            sb.Append(" where 1=1");
            
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
        //刪除二代健保參數檔
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        //寫log
        sb.AppendLine(" update TB_S_M_INS2_BASIC_SET set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2IB010' ");
        sb.AppendLine(" where YEAR_MONTH = @YEAR_MONTH; ");
       
        sb.Append(" Delete from TB_S_M_INS2_BASIC_SET ");
        sb.Append(" where YEAR_MONTH = @YEAR_MONTH;");
        ht.Add("@YEAR_MONTH", deleteitem);
        ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);

        dbConn.ExecuteT(sb, ht, true);

        return "0";
    }
    internal DataTable getExistData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select * from TB_S_M_INS2_BASIC_SET where YEAR_MONTH = @YEAR_MONTH");
            ht.Add("@YEAR_MONTH", YEAR_MONTH);
           
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
    internal void addData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("INSERT INTO TB_S_M_INS2_BASIC_SET (YEAR_MONTH,INS_RATE_PERSON,INS_RATE_COMP,INS_MAX_MONTH,INS_RENT_MIN_AMOUNT,INS_RENT_MAX_AMOUNT,INS_CON_MAX_AMOUNT,INS_CON_MIN_AMOUNT,");
            sb.Append(" INS_SALE_MIN_AMOUNT,INS_SALE_MAX_AMOUNT,INS_STOCK_MIN_AMOUNT,INS_STOCK_MAX_AMOUNT,INS_INTEREST_MIN_AMOUNT,INS_INTEREST_MAX_AMOUNT,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" Values (@YEAR_MONTH,@INS_RATE_PERSON,@INS_RATE_COMP,@INS_MAX_MONTH,@INS_RENT_MIN_AMOUNT,@INS_RENT_MAX_AMOUNT,@INS_CON_MAX_AMOUNT,@INS_CON_MIN_AMOUNT,@INS_SALE_MIN_AMOUNT,");
            sb.Append(" @INS_SALE_MAX_AMOUNT,@INS_STOCK_MIN_AMOUNT,@INS_STOCK_MAX_AMOUNT,@INS_INTEREST_MIN_AMOUNT,@INS_INTEREST_MAX_AMOUNT,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");
            ht.Add("@YEAR_MONTH", YEAR_MONTH);
            ht.Add("@INS_RATE_PERSON", INS_RATE_PERSON);
            ht.Add("@INS_RATE_COMP", INS_RATE_COMP);
            ht.Add("@INS_MAX_MONTH", INS_MAX_MONTH);
            ht.Add("@INS_RENT_MIN_AMOUNT", INS_MIN_AMOUNT);
            ht.Add("@INS_RENT_MAX_AMOUNT", INS_MAX_AMOUNT);
            ht.Add("@INS_CON_MAX_AMOUNT", INS_CON_MAX_AMOUNT);
            ht.Add("@INS_CON_MIN_AMOUNT", INS_CON_MIN_AMOUNT);
            //terry add
            ht.Add("@INS_SALE_MIN_AMOUNT", INS_SALE_MIN_AMOUNT);
            ht.Add("@INS_SALE_MAX_AMOUNT", INS_SALE_MAX_AMOUNT);
            ht.Add("@INS_STOCK_MIN_AMOUNT", INS_STOCK_MIN_AMOUNT);
            ht.Add("@INS_STOCK_MAX_AMOUNT", INS_STOCK_MAX_AMOUNT);
            ht.Add("@INS_INTEREST_MIN_AMOUNT", INS_INTEREST_MIN_AMOUNT);
            ht.Add("@INS_INTEREST_MAX_AMOUNT", INS_INTEREST_MAX_AMOUNT);

            ht.Add("@CREATED_BY", CREATED_BY);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", "FB2IB040");
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    internal void updateData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Update TB_S_M_INS2_BASIC_SET ");
            sb.Append(" Set INS_RATE_PERSON=@INS_RATE_PERSON,INS_RATE_COMP=@INS_RATE_COMP,INS_MAX_MONTH=@INS_MAX_MONTH,INS_RENT_MIN_AMOUNT=@INS_RENT_MIN_AMOUNT,INS_RENT_MAX_AMOUNT=@INS_RENT_MAX_AMOUNT,");
            sb.Append(" INS_SALE_MIN_AMOUNT = @INS_SALE_MIN_AMOUNT,INS_SALE_MAX_AMOUNT = @INS_SALE_MAX_AMOUNT,INS_STOCK_MIN_AMOUNT = @INS_STOCK_MIN_AMOUNT,INS_STOCK_MAX_AMOUNT = @INS_STOCK_MAX_AMOUNT,");
            sb.Append(" INS_INTEREST_MIN_AMOUNT = @INS_INTEREST_MIN_AMOUNT,INS_INTEREST_MAX_AMOUNT = @INS_INTEREST_MAX_AMOUNT, UPDATED_BY=@UPDATED_BY,UPDATED_DT = GETDATE()");
            sb.Append(" where YEAR_MONTH = @YEAR_MONTH");
            ht.Add("@YEAR_MONTH", YEAR_MONTH);
            ht.Add("@INS_RATE_PERSON", INS_RATE_PERSON);
            ht.Add("@INS_RATE_COMP", INS_RATE_COMP);
            ht.Add("@INS_MAX_MONTH", INS_MAX_MONTH);
            ht.Add("@INS_RENT_MIN_AMOUNT", INS_MIN_AMOUNT);
            ht.Add("@INS_RENT_MAX_AMOUNT", INS_MAX_AMOUNT);
            //terry add
            ht.Add("@INS_SALE_MIN_AMOUNT", INS_SALE_MIN_AMOUNT);
            ht.Add("@INS_SALE_MAX_AMOUNT", INS_SALE_MAX_AMOUNT);
            ht.Add("@INS_STOCK_MIN_AMOUNT", INS_STOCK_MIN_AMOUNT);
            ht.Add("@INS_STOCK_MAX_AMOUNT", INS_STOCK_MAX_AMOUNT);
            ht.Add("@INS_INTEREST_MIN_AMOUNT", INS_INTEREST_MIN_AMOUNT);
            ht.Add("@INS_INTEREST_MAX_AMOUNT", INS_INTEREST_MAX_AMOUNT);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //internal System.Data.DataTable getSYS_ID()
    //{
        //try
        //{
        //    StringBuilder sb = new StringBuilder();
        //    Hashtable ht = new Hashtable();
        //    sb.Append(" select SUB_CD ,SUB_DESC from TB_9_M_COMM_D where 1=1 and sys_cd = '99' and main_cd = 'sys_id' and is_valid ='Y' order by SUB_CD");
        //    return dbConn.Query(sb);

        //}
        //catch (Exception)
        //{

        //    throw;
        //}
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
}