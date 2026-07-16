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
public class CFB2SH0100DAO : BaseDAO
{
    //sh010基本欄位
    public string LEVEL_CD { get; set; }
    public string WS_CD { get; set; }
    public string AWARD { get; set; }
    public string AWARD_BASE { get; set; }
    public string AWARD_DESC { get; set; }

    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }



    //dj010
    /*
    public string ENV_ALLOWANCE_TYPE { get; set; }
    public string START_DT { get; set; }
    public string END_DT { get; set; }
    public string ENV_ALLOWANCE_DESC { get; set; }
    public string ENV_ALLOWANCE_VALUE { get; set; }
    public string ENV_MIN_UNIT { get; set; }
    public string REMARK { get; set; }
   */


    public CFB2SH0100DAO()
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
            sb.Append(" select count(0) resultCount from TB_S_M_AWARD_COND ");
            sb.Append(" where LEVEL_CD=@LEVEL_CD");
            sb.Append(" and WS_CD = @WS_CD");
            sb.Append(" and AWARD = @AWARD");
            ht.Add("@LEVEL_CD", LEVEL_CD);
            ht.Add("@WS_CD", WS_CD);
            ht.Add("@AWARD", AWARD);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //取得生效的資格檔，
    internal DataTable getEMPLevelData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select LEVEL_CD level, ORDER_SEQ orderSeq from  TB_H_M_LEVEL ");
            sb.Append(" where GETDATE()>=START_DT and GETDATE()<=END_DT ");
            sb.Append(" order by ORDER_SEQ ");
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
                            , string award, string level_cd, string ws_cd
                           )
    {
        try
        {

            if (sortExpression.Contains("AWARD_BASE"))
                sortExpression = sortExpression.Replace("AWARD_BASE", "a.AWARD_BASE");
            if (sortExpression.Contains("LEVEL_CD"))
                sortExpression = sortExpression.Replace("LEVEL_CD", "a.LEVEL_CD");

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + ") As RowNumber,");
            sb.Append("a.LEVEL_CD, WS_CD, AWARD, AWARD_BASE, AWARD_DESC   ");
            sb.Append(" , a.WS_CD + '-' + e.SUB_DESC WS_CD_DESC   ");
            sb.Append(" from TB_S_M_AWARD_COND a ");
           // sb.Append(" inner join VW_TB_H_M_LEVEL b on  a.LEVEL_CD = b.LEVEL_CD  ");
             sb.Append(" left join TB_9_M_COMM_D e on  a.WS_CD = e.SUB_CD  and e.MAIN_CD = 'WS_CD'  and IS_VALID='Y' and SYS_CD='HB'  ");
            sb.Append(" where 1=1 ");
            //sb.Append(" and GETDATE()>= b.START_DT and GETDATE()<= b.END_DT ");

            if (award != "")
            {
                sb.Append(" and (  AWARD like @AWARD ");
                ht.Add("@AWARD", award + "%");
            }


            if (award != "" && ws_cd == "-1" && level_cd == "")
            {
                sb.Append(" and   WS_CD = @WS_CD  and a.LEVEL_CD = @LEVEL_CD  ) or (   AWARD like @AWARD  ");
                ht.Add("@WS_CD", "");
                ht.Add("@LEVEL_CD", "");
            }
           
            if (award != "")
            {
                sb.Append(" ) ");
            }

            if (level_cd != "")
            {
                sb.Append(" and a.LEVEL_CD like @LEVEL_CD ");
                ht.Add("@LEVEL_CD", level_cd + "%");
            }

            if (ws_cd != "-1")
            {
                sb.Append(" and WS_CD = @WS_CD ");
                ht.Add("@WS_CD", ws_cd);
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
                         , string award, string level_cd, string ws_cd)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) resultCount ");
            sb.Append(" from TB_S_M_AWARD_COND a ");
            sb.Append(" where 1=1 ");


            if (award != "")
            {
                sb.Append(" and (  AWARD like @AWARD ");
                ht.Add("@AWARD", award + "%");
            }


            if (award != "" && ws_cd == "-1" && level_cd == "")
            {
                sb.Append(" and   WS_CD = @WS_CD  and a.LEVEL_CD = @LEVEL_CD  ) or (   AWARD like @AWARD  ");
                ht.Add("@WS_CD", "");
                ht.Add("@LEVEL_CD", "");
            }

            if (award != "")
            {
                sb.Append(" ) ");
            }

            if (level_cd != "")
            {
                sb.Append(" and a.LEVEL_CD like @LEVEL_CD ");
                ht.Add("@LEVEL_CD", level_cd + "%");
            }

            if (ws_cd != "-1")
            {
                sb.Append(" and WS_CD = @WS_CD ");
                ht.Add("@WS_CD", ws_cd);
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
    public void deleteData(string level_Cd, string ws_cd , string award)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("delete from TB_S_M_AWARD_COND ");
            sb.Append(" where LEVEL_CD = @LEVEL_CD  ");
            sb.Append(" and WS_CD = @WS_CD ");
            sb.Append(" and AWARD = @AWARD ");
            ht.Add("@LEVEL_CD", level_Cd);
            ht.Add("@WS_CD", ws_cd);
            ht.Add("@AWARD", award);
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
            sb.Append(" update TB_S_M_AWARD_COND ");
            sb.Append(" set AWARD_BASE = @AWARD_BASE ");
            sb.Append(" ,AWARD_DESC = @AWARD_DESC ");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT = @UPDATED_DT");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where LEVEL_CD = @LEVEL_CD ");
            sb.Append(" and  WS_CD = @WS_CD ");
            sb.Append(" and  AWARD = @AWARD ");

            ht.Add("@AWARD_BASE", AWARD_BASE);
            ht.Add("@AWARD_DESC", AWARD_DESC);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@UPDATED_DT", now);
            ht.Add("@FUNC_ID", FUNC_ID);

            //PK值
            ht.Add("@LEVEL_CD", LEVEL_CD);
            ht.Add("@WS_CD", WS_CD);
            ht.Add("@AWARD", AWARD);

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
            sb.Append(" INSERT INTO TB_S_M_AWARD_COND ");
            sb.Append(" ( ");
            sb.Append(" LEVEL_CD,WS_CD,AWARD,AWARD_BASE,AWARD_DESC ");
            sb.Append(" ,CREATED_BY, CREATED_DT, UPDATED_BY, UPDATED_DT, FUNC_ID  ");
            sb.Append(" ) ");
            sb.Append(" VALUES ");
            sb.Append(" ( @LEVEL_CD,  @WS_CD,  @AWARD,  @AWARD_BASE,  @AWARD_DESC  ");

            sb.Append("   ,@CREATED_BY,  @CREATED_DT,  @UPDATED_BY,  @UPDATED_DT,  @FUNC_ID )");

            ht.Add("@LEVEL_CD", LEVEL_CD);
            ht.Add("@WS_CD", WS_CD);
            ht.Add("@AWARD", AWARD);
            ht.Add("@AWARD_BASE", AWARD_BASE);
            ht.Add("@AWARD_DESC", AWARD_DESC);
            ht.Add("@CREATED_BY", UPDATED_BY);
            ht.Add("@CREATED_DT", now);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@UPDATED_DT", now);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion



}