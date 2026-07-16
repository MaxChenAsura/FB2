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
public class CFB2990700DAO : BaseDAO
{
    //HA070基本欄位
    public string TPARTO { get; set; }
    public string TPARTN { get; set; }

    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }


    public CFB2990700DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }


    #region 資料取得


    //依PK值取得資料，
    internal DataTable getPKData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select count(0) resultCount from zAS400_TB_S_DB3K9T0 ");
            sb.Append(" where TPARTO=@TPARTO");
            sb.Append(" and TPARTN = @TPARTN");
            ht.Add("@TPARTO", TPARTO);
            ht.Add("@TPARTN", TPARTN);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getDeptDate()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select count(0) resultCount from VW_H_DEPT_DATA ");
            sb.Append(" where DEPT_NO=@TPARTN ");
            ht.Add("@TPARTN", TPARTN);
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
                            , string tparto, string tpartn
                           )
    {
        try
        {

            //if (sortExpression.Contains("AWARD_BASE"))
            //    sortExpression = sortExpression.Replace("AWARD_BASE", "a.AWARD_BASE");

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + ") As RowNumber,");
            sb.Append(" a.*   ");
            sb.Append(" from zAS400_TB_S_DB3K9T0 a ");
            sb.Append(" where 1=1 ");

            if (tparto != "")
            {
                sb.Append(" and  TPARTO like @TPARTO ");
                ht.Add("@TPARTO", tparto + "%");
            }
            if (tpartn != "")
            {
                sb.Append(" and   TPARTN like @TPARTN ");
                ht.Add("@TPARTN", tpartn + "%");
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
                         , string tparto, string tpartn )
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) resultCount ");
            sb.Append(" from zAS400_TB_S_DB3K9T0 a ");
            sb.Append(" where 1=1 ");


            if (tparto != "")
            {
                sb.Append(" and  TPARTO like @TPARTO ");
                ht.Add("@TPARTO", tparto + "%");
            }
            if (tpartn != "")
            {
                sb.Append(" and   TPARTN like @TPARTN ");
                ht.Add("@TPARTN", tpartn + "%");
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
            sb.Append("delete from zAS400_TB_S_DB3K9T0 ");
            sb.Append(" where TPARTO = @TPARTO  ");
            sb.Append(" and TPARTN = @TPARTN ");
            ht.Add("@TPARTO", TPARTO);
            ht.Add("@TPARTN", TPARTN);
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
            sb.Append(@" update TB_S_M_AWARD_COND 



                    ");
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@UPDATED_DT", now);
            ht.Add("@FUNC_ID", FUNC_ID);

            //PK值
            //ht.Add("@LEVEL_CD", LEVEL_CD);
            //ht.Add("@WS_CD", WS_CD);
            //ht.Add("@AWARD", AWARD);

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
            sb.Append(@" INSERT INTO zAS400_TB_S_DB3K9T0
                         (TPARTO,TPARTN,TPARTN1,TPARTN2,TPARTN3,TPARTN4,TPARTN5,TPARTN6,THWKNO)
                        SELECT　@TPARTO,@TPARTN
                        ,isnull(DEPT_NAME_20,'')
                        ,isnull(DEPT_NAME_30,'')
                        ,isnull(DEPT_NAME_40,'')
                        ,isnull(DEPT_NAME_50,'')
                        ,isnull(DEPT_NAME_60,'')
                        ,isnull(DEPT_NAME_70,'')
                        ,isnull(HEAD_EMP_ID,'')
                        from VW_H_DEPT_DATA
                        where DEPT_NO=@TPARTN
                    ");

            ht.Add("@TPARTO", TPARTO);
            ht.Add("@TPARTN", TPARTN);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion



}