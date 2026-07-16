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
public class CFB2HA0800DAO : BaseDAO
{
    //HA070基本欄位
    public string TPJOBN { get; set; }
    public string TPJOBO { get; set; }
    public string T1LVLO { get; set; }

    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }


    public CFB2HA0800DAO()
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
            sb.Append(" select count(0) resultCount from zAS400_TB_S_DB3K9T1 ");
            sb.Append(" where TPJOBN=@TPJOBN");
            sb.Append(" and TPJOBO = @TPJOBO");
            ht.Add("@TPJOBN", TPJOBN);
            ht.Add("@TPJOBO", TPJOBO);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getPjobDate()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select count(0) resultCount from VW_TB_H_M_PJOB ");
            sb.Append(" where PJOB_CD=@TPJOBN ");
            ht.Add("@TPJOBN", TPJOBN);
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
                            , string tpjobo, string t1lvlo, string tpjobn, string t1levl
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
            sb.Append(" from zAS400_TB_S_DB3K9T1 a ");
            sb.Append(" where 1=1 ");

            if (tpjobo != "")
            {
                sb.Append(" and  TPJOBO like @TPJOBO ");
                ht.Add("@TPJOBO", tpjobo + "%");
            }
            if (t1lvlo != "")
            {
                sb.Append(" and   T1LVLO like @T1LVLO ");
                ht.Add("@T1LVLO", t1lvlo + "%");
            }
            if (tpjobn != "")
            {
                sb.Append(" and   TPJOBN like @TPJOBN ");
                ht.Add("@TPJOBN", tpjobn + "%");
            }
            if (t1levl != "")
            {
                sb.Append(" and   T1LEVL like @T1LEVL ");
                ht.Add("@T1LEVL", t1levl + "%");
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
                         , string tpjobo, string t1lvlo, string tpjobn, string t1levl)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) resultCount ");
            sb.Append(" from zAS400_TB_S_DB3K9T1 a ");
            sb.Append(" where 1=1 ");


            if (tpjobo != "")
            {
                sb.Append(" and  TPJOBO like @TPJOBO ");
                ht.Add("@TPJOBO", tpjobo + "%");
            }
            if (t1lvlo != "")
            {
                sb.Append(" and   T1LVLO like @T1LVLO ");
                ht.Add("@T1LVLO", t1lvlo + "%");
            }
            if (tpjobn != "")
            {
                sb.Append(" and   TPJOBN like @TPJOBN ");
                ht.Add("@TPJOBN", tpjobn + "%");
            }
            if (t1levl != "")
            {
                sb.Append(" and   T1LEVL like @T1LEVL ");
                ht.Add("@T1LEVL", t1levl + "%");
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
            sb.Append("delete from zAS400_TB_S_DB3K9T1 ");
            sb.Append(" where TPJOBN = @TPJOBN  ");
            sb.Append(" and TPJOBO = @TPJOBO ");
            ht.Add("@TPJOBN", TPJOBN);
            ht.Add("@TPJOBO", TPJOBO);
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
            sb.Append(@" INSERT INTO zAS400_TB_S_DB3K9T1
                         (TPJOBN,T1LEVL,T1WSID,TPJNMN,TPJOBO,T1LVLO,T1USID,T1MTDT,T1MTTM)
                        SELECT　@TPJOBN
                        ,isnull(LEVEL_CD,'')
                        ,isnull(WS_CD,'')
                        ,isnull(PJOB_DESC,'')
                        ,@TPJOBO
                        ,@T1LVLO
                        ,@T1USID
                        ,CONVERT(decimal(7,0),CONVERT(varchar(4),YEAR(getdate())-1911)
                         +right('0'+CONVERT(varchar(2),MONTH(getdate())),2)
                         +right('0'+CONVERT(varchar(2),DAY(getdate())),2))           
                        ,CONVERT(decimal(6,0),REPLACE(CONVERT(VARCHAR(8),GETDATE(),108),':','') )
                        from VW_TB_H_M_PJOB
                        where PJOB_CD=@TPJOBN
                    ");
            
            ht.Add("@TPJOBN", TPJOBN);
            ht.Add("@TPJOBO", TPJOBO);
            ht.Add("@T1LVLO", T1LVLO);
            ht.Add("@T1USID", CREATED_BY);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion



}