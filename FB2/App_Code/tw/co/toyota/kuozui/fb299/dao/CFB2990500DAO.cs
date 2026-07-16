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
public class CFB2990500DAO : BaseDAO
{
    //HA070基本欄位
    public string TMLCD { get; set; }
    public string TSLCD { get; set; }
    public string T4LCC { get; set; }

    
    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }


    public CFB2990500DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }


    #region 資料取得
    //取得主假別說明
    public DataTable getMAIN_LEAVE_DESC(string main_leave_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select MAIN_LEAVE_CD,MAIN_LEAVE_DESC ");
            sb.Append(" from TB_D_M_LEAVE_TYPE_H ");
            sb.Append(" where MAIN_LEAVE_CD=@MAIN_LEAVE_CD ");
            ht.Add("@MAIN_LEAVE_CD", main_leave_cd);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //取得子假別說明
    public DataTable getSUB_LEAVE_DESC(string sub_leave_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select SUB_LEAVE_CD,SUB_LEAVE_DESC,LEAVE_TIME_UNIT ");
            sb.Append(" from TB_D_M_LEAVE_TYPE_D ");
            sb.Append(" where SUB_LEAVE_CD=@SUB_LEAVE_CD ");
            ht.Add("@SUB_LEAVE_CD", sub_leave_cd);

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
            sb.Append(" select count(0) resultCount from zAS400_TB_S_DB3K9T4 ");
            sb.Append(" where TMLCD=@TMLCD");
            sb.Append(" and TSLCD = @TSLCD");
            ht.Add("@TMLCD", TMLCD);
            ht.Add("@TSLCD", TSLCD);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getMainLevel()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select count(0) resultCount from TB_D_M_LEAVE_TYPE_H ");
            sb.Append(" where MAIN_LEAVE_CD=@TMLCD ");
            ht.Add("@TMLCD", TMLCD);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getSubLevel()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select count(0) resultCount from TB_D_M_LEAVE_TYPE_D ");
            sb.Append(" where MAIN_LEAVE_CD=@MAIN_LEAVE_CD ");
            sb.Append(" and SUB_LEAVE_CD=@SUB_LEAVE_CD ");
            ht.Add("@MAIN_LEAVE_CD", TMLCD);
            ht.Add("@SUB_LEAVE_CD", TSLCD);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }


    internal DataTable getOldMainLevel()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select count(0) resultCount from TB_9_M_COMM_D ");
            sb.Append(" where sys_CD='DH' and MAIN_CD='T4LCC' ");
            sb.Append(" and SUB_CD=@T4LCC ");
            ht.Add("@T4LCC", T4LCC);
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
                            , string tmlcd, string tslcd, string t4lcc
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
            sb.Append(" from zAS400_TB_S_DB3K9T4 a ");
            sb.Append(" where 1=1 ");

            if (tmlcd != "")
            {
                sb.Append(" and  TMLCD = @TMLCD ");
                ht.Add("@TMLCD", tmlcd );
            }
            if (tslcd != "")
            {
                sb.Append(" and   TSLCD = @TSLCD ");
                ht.Add("@TSLCD", tslcd );
            }
            if (t4lcc != "")
            {
                sb.Append(" and   T4LCC = @T4LCC ");
                ht.Add("@T4LCC", t4lcc );
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
                         , string tmlcd, string tslcd, string t4lcc)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) resultCount ");
            sb.Append(" from zAS400_TB_S_DB3K9T4 a ");
            sb.Append(" where 1=1 ");


            if (tmlcd != "")
            {
                sb.Append(" and  TMLCD = @TMLCD ");
                ht.Add("@TMLCD", tmlcd);
            }
            if (tslcd != "")
            {
                sb.Append(" and   TSLCD = @TSLCD ");
                ht.Add("@TSLCD", tslcd );
            }
            if (t4lcc != "")
            {
                sb.Append(" and   T4LCC = @T4LCC ");
                ht.Add("@T4LCC", t4lcc);
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
            sb.Append("delete from zAS400_TB_S_DB3K9T4 ");
            sb.Append(" where TMLCD = @TMLCD  ");
            sb.Append(" and TSLCD = @TSLCD ");
            ht.Add("@TMLCD", TMLCD);
            ht.Add("@TSLCD", TSLCD);
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
            sb.Append(@" update zAS400_TB_S_DB3K9T4
                        set T4LCC =@T4LCC
                        where TMLCD=@TMLCD 
                        and   TSLCD=@TSLCD
                    ");
            ht.Add("@T4LCC", T4LCC);
            

            //PK值
            ht.Add("@TMLCD", TMLCD);
            ht.Add("@TSLCD", TSLCD);

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
            sb.Append(@" INSERT INTO zAS400_TB_S_DB3K9T4
                         (TMLCD,TMDESC,TSLCD,TSDESC,T4UNIT,T4LCC)
                        select 
                         H.MAIN_LEAVE_CD
                        ,H.MAIN_LEAVE_DESC
                        ,D.SUB_LEAVE_CD
                        ,D.SUB_LEAVE_DESC
                        ,D.LEAVE_TIME_UNIT
                        ,@T4LCC
                        from TB_D_M_LEAVE_TYPE_H H
                        left join  TB_D_M_LEAVE_TYPE_D D on H.MAIN_LEAVE_CD = D.MAIN_LEAVE_CD
                        where H.MAIN_LEAVE_CD=@TMLCD
                        and D.SUB_LEAVE_CD=@TSLCD
                    ");

            ht.Add("@T4LCC", T4LCC);
            ht.Add("@TMLCD", TMLCD);
            ht.Add("@TSLCD", TSLCD);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion



}