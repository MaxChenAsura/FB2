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
/// CFB2DJ040DAO 的摘要描述
/// </summary>
public class CFB2990600DAO : BaseDAO
{
    //HA070基本欄位
    public string TOVRCD { get; set; }
    public string T5AWC { get; set; }

    
    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }


    public CFB2990600DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }


    #region 資料取得
    //取得主假別說明
    public DataTable getOVERTIME_CD()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@"select OVERTIME_CD, OVERTIME_CD+'-'+OVERTIME_DESC OVERTIME_DESC
                        from TB_D_M_OVERTIME_TYPE
                        ");

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }


    //依PK值取得資料，
    internal DataTable getPKData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select count(0) resultCount from zAS400_TB_S_DB3K9T5 ");
            sb.Append(" where TOVRCD=@TOVRCD");
            ht.Add("@TOVRCD", TOVRCD);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    
    #endregion

    #region Gridview 資料
    //Gridview 查詢資料
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression
                            , string tovrcd, string t5awc
                           )
    {
        try
        {
            //if (sortExpression.Contains("AWARD_BASE"))
            //    sortExpression = sortExpression.Replace("AWARD_BASE", "a.AWARD_BASE");

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + ") As RowNumber, ");
            sb.Append(@" TOVRCD
                        ,TOVRCD+'-'+isnull(B.OVERTIME_DESC,'')	TOVRCD_DESC
                        ,T5AWC   
                        from zAS400_TB_S_DB3K9T5 A
                        left join TB_D_M_OVERTIME_TYPE  B on A.TOVRCD=B.OVERTIME_CD ");
            sb.Append(" where 1=1 ");

            if (tovrcd != "-1")
            {
                sb.Append(" and  TOVRCD = @TOVRCD ");
                ht.Add("@TOVRCD", tovrcd);
            }
            if (t5awc != "")
            {
                sb.Append(" and   T5AWC = @T5AWC ");
                ht.Add("@T5AWC", t5awc);
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
    public int getCount(int startRowIndex, int maximumRows
                        , string tovrcd, string t5awc)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) resultCount ");
            sb.Append(" from zAS400_TB_S_DB3K9T5 a ");
            sb.Append(" where 1=1 ");


            if (tovrcd != "-1")
            {
                sb.Append(" and  TOVRCD = @TOVRCD ");
                ht.Add("@TOVRCD", tovrcd);
            }
            if (t5awc != "")
            {
                sb.Append(" and   T5AWC = @T5AWC ");
                ht.Add("@T5AWC", t5awc);
            }



            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                t = (int)dt.Rows[0]["resultCount"];
            }
            return t;
        }
        catch (Exception)
        {

            throw;
        }

    }

    #endregion


    #region DB存取
    //刪除 
    public void deleteData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("delete from zAS400_TB_S_DB3K9T5 ");
            sb.Append(" where TOVRCD = @TOVRCD  ");
            ht.Add("@TOVRCD", TOVRCD);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //修改
    public void updateData() {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            DateTime now = DateTime.Parse(DateTime.Now.ToString());
            sb.Append(@" update zAS400_TB_S_DB3K9T5
                        set T5AWC =@T5AWC
                        where TOVRCD=@TOVRCD 
                    ");
            ht.Add("@T5AWC", T5AWC);

            //PK值
            ht.Add("@TOVRCD", TOVRCD);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }

    
    }

    //新增
    internal void insertData()
    {
        try
        {
            DateTime now = DateTime.Parse(DateTime.Now.ToString());
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" INSERT INTO zAS400_TB_S_DB3K9T5
                         (TOVRCD,T5AWC)
                         select 
                         @TOVRCD
                        ,@T5AWC
                    ");

            ht.Add("@TOVRCD", TOVRCD);
            ht.Add("@T5AWC", T5AWC);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion



}