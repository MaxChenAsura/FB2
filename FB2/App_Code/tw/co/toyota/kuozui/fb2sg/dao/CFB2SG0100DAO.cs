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
/// CFB2SG010DAO 的摘要描述
/// </summary>
public class CFB2SG0100DAO : BaseDAO
{
    //基本欄位
    public string FESTIVAL_TYPE { get; set; }
    public string FESTIVAL_PAY_COND { get; set; }
    public string FESTIVAL_AMT { get; set; }
    public string WORK_YEARS_SDT { get; set; }
    public string WORK_YEARS_EDT { get; set; }
    public string PRID_CD { get; set; }
  
    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }

    //hidden欄位
    public string PRID_CD_OLD { get; set; }



    public CFB2SG0100DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }


    #region 資料取得
    //依PK值取得資料，
    internal DataTable getCondLogData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select *, FESTIVAL_TYPE + '-' + b.SUB_DESC FESTIVAL_TYPE_DESC  ");
            sb.Append(" from TB_S_M_FESTIVAL_COND_LOG a");
            sb.Append("  left join TB_9_M_COMM_D b on  a.FESTIVAL_TYPE = b.SUB_CD and b.MAIN_CD = 'FESTIVAL_TYPE'  ");
            sb.Append(" order by FESTIVAL_YEAR DESC, FESTIVAL_TYPE ASC ");
        
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
            sb.Append("select count(0) resultCount from TB_S_M_FESTIVAL_COND");
            sb.Append(" where FESTIVAL_TYPE=@FESTIVAL_TYPE");
            sb.Append(" and WORK_YEARS_SDT = @WORK_YEARS_SDT");
            sb.Append(" and PRID_CD = @PRID_CD");
            ht.Add("@FESTIVAL_TYPE", FESTIVAL_TYPE);
            ht.Add("@WORK_YEARS_SDT", WORK_YEARS_SDT);
            ht.Add("@PRID_CD", PRID_CD);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //取得同節金類別 + 某員工區分的 在職年資起迄  
    internal DataTable getPridCDData(string pridCD)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select WORK_YEARS_SDT, WORK_YEARS_EDT from TB_S_M_FESTIVAL_COND");
            sb.Append(" where FESTIVAL_TYPE=@FESTIVAL_TYPE");
            sb.Append(" and PRID_CD like @PRID_CD");
            ht.Add("@FESTIVAL_TYPE", FESTIVAL_TYPE);
            ht.Add("@PRID_CD", "%"+ pridCD+"%" );
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //在職年資起,與同節金類別+同員工區分的起迄是否重疊
    internal DataTable getValidData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select count(0) resultCount from TB_S_M_FESTIVAL_COND");
            sb.Append(" where FESTIVAL_TYPE=@FESTIVAL_TYPE");
            sb.Append(" and WORK_YEARS_SDT <> @WORK_YEARS_SDT");
            sb.Append(" and WORK_YEARS_SDT <= @WORK_YEARS_EDT");
            sb.Append(" and WORK_YEARS_EDT   >= @WORK_YEARS_EDT");
            sb.Append(" and PRID_CD   = @PRID_CD");

            ht.Add("@FESTIVAL_TYPE", FESTIVAL_TYPE);
            ht.Add("@WORK_YEARS_SDT", WORK_YEARS_SDT);
            ht.Add("@WORK_YEARS_EDT", WORK_YEARS_EDT);
            ht.Add("@PRID_CD", PRID_CD);
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
                           , string festivalType)
    {
        try
        {

            //if (sortExpression.Contains("ENV_ALLOWANCE_TYPE"))
            //    sortExpression = sortExpression.Replace("ENV_ALLOWANCE_TYPE", "a.ENV_ALLOWANCE_TYPE");

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + ") As RowNumber,");
            sb.Append(" FESTIVAL_TYPE + '-' + b.SUB_DESC FESTIVAL_TYPE_DESC   ");
            sb.Append(" , FESTIVAL_TYPE, FESTIVAL_PAY_COND, FESTIVAL_AMT, WORK_YEARS_SDT, WORK_YEARS_EDT, PRID_CD   ");
            sb.Append(" from TB_S_M_FESTIVAL_COND a ");
            sb.Append("  left join TB_9_M_COMM_D b on  a.FESTIVAL_TYPE = b.SUB_CD and b.MAIN_CD = 'FESTIVAL_TYPE' and b.IS_VALID='Y'  and b.SYS_CD='SG'  ");
            sb.Append(" where 1=1 ");


            if (festivalType!="" && festivalType != "-1")
            {
                sb.Append(" and FESTIVAL_TYPE = @FESTIVAL_TYPE ");
                ht.Add("@FESTIVAL_TYPE", festivalType);
            }

            sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar)");


            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    
    //Gridview 查詢總筆數
    public int getCount(int startRowIndex, int maximumRows
                        , string festivalType)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) resultCount ");
            sb.Append(" from TB_S_M_FESTIVAL_COND ");
            sb.Append(" where 1=1 ");

            if (festivalType != "" && festivalType != "-1")
            {
                sb.Append(" and FESTIVAL_TYPE = @FESTIVAL_TYPE ");
                ht.Add("@FESTIVAL_TYPE", festivalType);
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
    public void deleteData(string type, string sdt,string pridCD)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("delete from TB_S_M_FESTIVAL_COND ");
            sb.Append(" where FESTIVAL_TYPE = @FESTIVAL_TYPE and WORK_YEARS_SDT = @WORK_YEARS_SDT and PRID_CD = @PRID_CD ");
            ht.Add("@FESTIVAL_TYPE", type);
            ht.Add("@WORK_YEARS_SDT", sdt);
            ht.Add("@PRID_CD", pridCD);
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
            sb.Append(" update TB_S_M_FESTIVAL_COND ");
            sb.Append(" set FESTIVAL_AMT = @FESTIVAL_AMT ");
            sb.Append(" ,FESTIVAL_PAY_COND = @FESTIVAL_PAY_COND ");
            sb.Append(" ,WORK_YEARS_EDT = @WORK_YEARS_EDT ");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT = @UPDATED_DT");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where FESTIVAL_TYPE = @FESTIVAL_TYPE and WORK_YEARS_SDT = @WORK_YEARS_SDT and PRID_CD = @PRID_CD");

            ht.Add("@FESTIVAL_AMT", FESTIVAL_AMT);
            ht.Add("@FESTIVAL_PAY_COND", FESTIVAL_PAY_COND);
            ht.Add("@WORK_YEARS_EDT", WORK_YEARS_EDT);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@UPDATED_DT", now);
            ht.Add("@FUNC_ID", FUNC_ID);

            //pk值
            ht.Add("@FESTIVAL_TYPE", FESTIVAL_TYPE);
            ht.Add("@WORK_YEARS_SDT", WORK_YEARS_SDT);
            ht.Add("@PRID_CD", PRID_CD);

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
            sb.Append(" INSERT INTO TB_S_M_FESTIVAL_COND ");
            sb.Append(" ( ");
            sb.Append(" FESTIVAL_TYPE,FESTIVAL_PAY_COND,FESTIVAL_AMT,WORK_YEARS_SDT,WORK_YEARS_EDT,PRID_CD ");
            sb.Append(" ,CREATED_BY, CREATED_DT, UPDATED_BY, UPDATED_DT, FUNC_ID  ");
            sb.Append(" ) ");
            sb.Append(" VALUES ");
            sb.Append(" ( @FESTIVAL_TYPE,  @FESTIVAL_PAY_COND,  @FESTIVAL_AMT,  @WORK_YEARS_SDT,  @WORK_YEARS_EDT,  @PRID_CD ");
            sb.Append("   ,@CREATED_BY,  @CREATED_DT,  @UPDATED_BY,  @UPDATED_DT,  @FUNC_ID )");

            ht.Add("@FESTIVAL_TYPE", FESTIVAL_TYPE);
            ht.Add("@FESTIVAL_PAY_COND", FESTIVAL_PAY_COND);
            ht.Add("@FESTIVAL_AMT", FESTIVAL_AMT);
            ht.Add("@WORK_YEARS_SDT", WORK_YEARS_SDT);
            ht.Add("@WORK_YEARS_EDT", WORK_YEARS_EDT);
            ht.Add("@PRID_CD", PRID_CD);
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