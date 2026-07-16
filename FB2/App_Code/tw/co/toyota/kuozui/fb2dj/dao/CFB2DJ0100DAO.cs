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
/// CFB2DJ010DAO 的摘要描述
/// </summary>
public class CFB2DJ0100DAO : BaseDAO
{
    //基本欄位
    public string ENV_ALLOWANCE_TYPE { get; set; }
    public string START_DT { get; set; }
    public string END_DT { get; set; }
    public string ENV_ALLOWANCE_DESC { get; set; }
    public string ENV_ALLOWANCE_VALUE { get; set; }
    public string ENV_MIN_UNIT { get; set; }
    public string REMARK { get; set; }
    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }


    public CFB2DJ0100DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }


    #region 資料取得
    //查詢條件的環境津貼等級(僅有生效的)
    public DataTable getEnvType()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select distinct ENV_ALLOWANCE_TYPE sub_cd ");
            sb.Append(" from TB_D_M_ENV_ALLOWANCE_TYPE ");
           // sb.Append(" where GETDATE() >= START_DT and GETDATE()  <= END_DT ");
           // sb.Append(" order by ENV_ALLOWANCE_VALUE desc ");
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //依PK值取得資料，
    internal DataTable getPKDataCount()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select count(0) typecount from TB_D_M_ENV_ALLOWANCE_TYPE");
            sb.Append(" where ENV_ALLOWANCE_TYPE=@ENV_ALLOWANCE_TYPE");
            sb.Append(" and START_DT = @START_DT");
            ht.Add("@ENV_ALLOWANCE_TYPE", ENV_ALLOWANCE_TYPE);
            ht.Add("@START_DT", Convert.ToDateTime(START_DT));
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }


    //取得該等級的最大的生效時間
    internal DataTable getMaxEndDTByType()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select MAX(END_DT) maxEndDT from TB_D_M_ENV_ALLOWANCE_TYPE ");
            sb.Append(" where ENV_ALLOWANCE_TYPE=@ENV_ALLOWANCE_TYPE ");
            ht.Add("@ENV_ALLOWANCE_TYPE", ENV_ALLOWANCE_TYPE);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }



    //取得 環境津貼申請資料檔 是否已使用
    internal DataTable getExistType(string type, String startDT, string endDT)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(0) typecount From");
            sb.Append(" TB_D_M_ENV_ALLOWANCE_APPLY");
            sb.Append(" where ENV_ALLOWANCE_TYPE = @ENV_ALLOWANCE_TYPE");
            sb.Append(" and APPLY_DT >= @startDT ");
            sb.Append(" and APPLY_DT <= @endDT ");
            ht.Add("@ENV_ALLOWANCE_TYPE", type);
            ht.Add("@startDT", startDT);
            ht.Add("@endDT", endDT);
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
                            , string env_type, string is_valid)
    {
        try
        {

            //if (sortExpression.Contains("ENV_ALLOWANCE_TYPE"))
            //    sortExpression = sortExpression.Replace("ENV_ALLOWANCE_TYPE", "a.ENV_ALLOWANCE_TYPE");

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //sb.Append(" Select * From  TB_D_M_ENV_ALLOWANCE_TYPE");
            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + ") As RowNumber,");
            sb.Append(" ENV_ALLOWANCE_TYPE, ENV_ALLOWANCE_DESC,ENV_ALLOWANCE_VALUE,ENV_MIN_UNIT,START_DT,END_DT,REMARK ");
            sb.Append(" from TB_D_M_ENV_ALLOWANCE_TYPE ");
            sb.Append(" where 1=1 ");

            if (env_type !="" &&  env_type != "-1")
            {
                sb.Append(" and ENV_ALLOWANCE_TYPE = @ENV_ALLOWANCE_TYPE ");
                ht.Add("@ENV_ALLOWANCE_TYPE", env_type);
            }
            if (is_valid == "Y")
            {
                sb.Append(" and GETDATE() >= START_DT and GETDATE()  <= END_DT   ");
            }
            if (is_valid == "N")
            {
                sb.Append(" and GETDATE()  >= END_DT    ");
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
    public int getCount(int startRowIndex, int maximumRows, string env_type, string is_valid)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) total_record ");
            sb.Append(" from TB_D_M_ENV_ALLOWANCE_TYPE ");
            sb.Append(" where 1=1 ");


            if (env_type != "" && env_type != "-1")
            {
                sb.Append(" and ENV_ALLOWANCE_TYPE = @ENV_ALLOWANCE_TYPE ");
                ht.Add("@ENV_ALLOWANCE_TYPE", env_type);
            }
            if (is_valid == "Y")
            {
                sb.Append(" and GETDATE() >= START_DT and GETDATE()  <= END_DT   ");
            }
            if (is_valid == "N")
            {
                sb.Append(" and GETDATE()  >= END_DT    ");
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

    #endregion


    #region DB存取
    //刪除 
    public void deleteData(string type, string start_dt)
    {
        try
        {
            // string dt = Convert.ToDateTime(start_dt).ToString("yyyy/MM/dd HH:mm:ss.fff");
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //寫log
            sb.Append(" update TB_D_M_ENV_ALLOWANCE_TYPE set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2DJ010' ");
            sb.Append(" where ENV_ALLOWANCE_TYPE = @ENV_ALLOWANCE_TYPE and START_DT = @START_DT; ");
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);

            sb.Append(" delete from TB_D_M_ENV_ALLOWANCE_TYPE ");
            //sb.Append(" where ENV_ALLOWANCE_TYPE = @ENV_ALLOWANCE_TYPE and Convert(char(10),START_DT,111) = @START_DT ");
            sb.Append(" where ENV_ALLOWANCE_TYPE = @ENV_ALLOWANCE_TYPE ");
            sb.Append(" and START_DT = @START_DT; ");

            ht.Add("@ENV_ALLOWANCE_TYPE", type);
            ht.Add("@START_DT", Convert.ToDateTime(start_dt).ToString("yyyy/MM/dd"));
            //ht.Add("@START_DT", start_dt);
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
            sb.Append(" update TB_D_M_ENV_ALLOWANCE_TYPE ");
            sb.Append(" set END_DT = @END_DT ");
            sb.Append(" ,ENV_ALLOWANCE_DESC = @ENV_ALLOWANCE_DESC ");
            sb.Append(" ,ENV_ALLOWANCE_VALUE = @ENV_ALLOWANCE_VALUE ");
            sb.Append(" ,ENV_MIN_UNIT = @ENV_MIN_UNIT ");
            sb.Append(" ,REMARK = @REMARK ");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT = @UPDATED_DT");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where ENV_ALLOWANCE_TYPE = @ENV_ALLOWANCE_TYPE and START_DT = @START_DT");

            ht.Add("@END_DT", Convert.ToDateTime(END_DT));
            ht.Add("@ENV_ALLOWANCE_DESC", ENV_ALLOWANCE_DESC);
            ht.Add("@ENV_ALLOWANCE_VALUE", ENV_ALLOWANCE_VALUE);
            ht.Add("@ENV_MIN_UNIT", ENV_MIN_UNIT);
            ht.Add("@REMARK", REMARK);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@UPDATED_DT", now);
            ht.Add("@FUNC_ID", FUNC_ID);
            ht.Add("@ENV_ALLOWANCE_TYPE", ENV_ALLOWANCE_TYPE);
            ht.Add("@START_DT", Convert.ToDateTime(START_DT));

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
            sb.Append(" INSERT INTO TB_D_M_ENV_ALLOWANCE_TYPE ");
            sb.Append(" ( ");
            sb.Append(" ENV_ALLOWANCE_TYPE, START_DT, END_DT, ENV_ALLOWANCE_DESC, ENV_ALLOWANCE_VALUE, ENV_MIN_UNIT,REMARK ");
            sb.Append(" ,CREATED_BY, CREATED_DT, UPDATED_BY, UPDATED_DT, FUNC_ID  ");
            sb.Append(" ) ");
            sb.Append(" VALUES ");
            sb.Append(" ( @ENV_ALLOWANCE_TYPE,  @START_DT,  @END_DT,  @ENV_ALLOWANCE_DESC,  @ENV_ALLOWANCE_VALUE,  @ENV_MIN_UNIT, @REMARK  ");

            sb.Append("   ,@CREATED_BY,  @CREATED_DT,  @UPDATED_BY,  @UPDATED_DT,  @FUNC_ID )");

            ht.Add("@ENV_ALLOWANCE_TYPE", ENV_ALLOWANCE_TYPE);
            ht.Add("@START_DT", Convert.ToDateTime(START_DT));
            ht.Add("@END_DT", Convert.ToDateTime(END_DT));
            ht.Add("@ENV_ALLOWANCE_DESC", ENV_ALLOWANCE_DESC);
            ht.Add("@ENV_ALLOWANCE_VALUE", ENV_ALLOWANCE_VALUE);
            ht.Add("@ENV_MIN_UNIT", ENV_MIN_UNIT);
            ht.Add("@REMARK", REMARK);
            ht.Add("@CREATED_BY", UPDATED_BY);
            ht.Add("@CREATED_DT", now);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@UPDATED_DT", now);
            ht.Add("@FUNC_ID", FUNC_ID);
            //ht.Add("@ENV_ALLOWANCE_TYPE", ENV_ALLOWANCE_TYPE);
            //ht.Add("@START_DT", Convert.ToDateTime(START_DT));

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion



}