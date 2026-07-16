using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using FB2.tw.co.toyota.kuozui.dao;
using System.Text;
using System.Collections;

/// <summary>
/// WFB2IA0500 的摘要描述
/// </summary>
public class CFB2IA0500DAO : BaseDAO
{
    public string TARGET_TYPE { get; set; }
    public string GINS_KIND { get; set; }
    public string GINS_ITEM { get; set; }
    public string GINS_ITEM_NAME { get; set; }
    public string AMT { get; set; }
    public string PERSON_QTY_S { get; set; }
    public string PERSON_QTY_E { get; set; }
    public string HOUSE_YN { get; set; }
    public string EMP_RATE { get; set; }
    public string CMP_RATE { get; set; }
    public string UNION_RATE { get; set; }
    public string CREATED_BY { get; set; }
    public string UPDATED_BY { get; set; }
    public string FUNC_ID { get; set; }

    public string FEES_YN { get; set; }

    public CFB2IA0500DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string target_type, string gins_kind)
    {
        try
        {
            if (sortExpression.Contains("TARGET_TYPE"))
                sortExpression = sortExpression.Replace("TARGET_TYPE", "a.TARGET_TYPE");

            if (sortExpression.Contains("GINS_KIND"))
                sortExpression = sortExpression.Replace("GINS_KIND", "a.GINS_KIND");

            if (sortExpression.Contains("GINS_ITEM"))
                sortExpression = sortExpression.Replace("GINS_ITEM", "a.GINS_ITEM");

            if (sortExpression.Contains("PERSON_QTY_S"))
                sortExpression = sortExpression.Replace("PERSON_QTY_S", "a.PERSON_QTY_S");


            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * from");
            sb.Append(" (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" b.SUB_CD+'-'+b.SUB_DESC TARGET_TYPE,b.SUB_CD,a.GINS_KIND,a.GINS_ITEM,a.GINS_ITEM_NAME,a.AMT,a.PERSON_QTY_S,a.PERSON_QTY_E,");
            sb.Append(" a.HOUSE_YN,a.EMP_RATE,a.CMP_RATE,a.UNION_RATE");
            sb.Append(" from TB_I_M_GROUP_KIND a");
            sb.Append(" left join TB_9_M_COMM_D b");
            sb.Append(" on a.TARGET_TYPE=b.SUB_CD and b.SYS_CD='IA' and b.MAIN_CD='TARGET_TYPE' ");
            sb.Append(" where 1=1");

            if (target_type != "-1" && target_type != null)
            {
                sb.Append(" and a.TARGET_TYPE = @target_type ");
                ht.Add("@target_type", target_type);
            }

            if (gins_kind != "-1" && gins_kind != null)
            {
                sb.Append(" and a.GINS_KIND = @gins_kind ");
                ht.Add("@gins_kind", gins_kind);
            }
            sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar) ");

            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    public int getCount(int startRowIndex, int maximumRows, string target_type, string gins_kind)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select COUNT(*) total_record ");
            sb.Append(" from TB_I_M_GROUP_KIND a");
            sb.Append(" left join TB_9_M_COMM_D b");
            sb.Append(" on a.TARGET_TYPE=b.SUB_CD and b.SYS_CD='IA' and b.MAIN_CD='TARGET_TYPE' ");
            sb.Append(" where 1=1");

            if (target_type != "-1" && target_type != null)
            {
                sb.Append(" and TARGET_TYPE = @target_type ");
                ht.Add("@target_type", target_type);
            }

            if (gins_kind != "-1" && gins_kind != null)
            {
                sb.Append(" and GINS_KIND = @gins_kind ");
                ht.Add("@gins_kind", gins_kind);
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

    //取得現有資料
    public DataTable getExistData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select TARGET_TYPE from TB_I_M_GROUP_KIND");
            sb.Append(" where TARGET_TYPE = @TARGET_TYPE and GINS_KIND = @GINS_KIND ");
            sb.Append(" and GINS_ITEM = @GINS_ITEM and PERSON_QTY_S = @PERSON_QTY_S");
            ht.Add("@TARGET_TYPE", TARGET_TYPE);
            ht.Add("@GINS_KIND", GINS_KIND);
            ht.Add("@GINS_ITEM", GINS_ITEM);
            ht.Add("@PERSON_QTY_S", PERSON_QTY_S);

            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }

    //新增 TB_I_M_GROUP_KIND
    public void addGROUP_KIND()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("insert into TB_I_M_GROUP_KIND ( ");
            sb.Append(" TARGET_TYPE,GINS_KIND,GINS_ITEM,GINS_ITEM_NAME,AMT,PERSON_QTY_S,");
            sb.Append(" PERSON_QTY_E,HOUSE_YN,EMP_RATE,CMP_RATE,UNION_RATE,FEES_YN,");
            sb.Append(" CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" values ( ");
            sb.Append(" @TARGET_TYPE,@GINS_KIND,@GINS_ITEM,@GINS_ITEM_NAME,@AMT,@PERSON_QTY_S,");
            sb.Append(" @PERSON_QTY_E,@HOUSE_YN,@EMP_RATE,@CMP_RATE,@UNION_RATE,@FEES_YN,");
            sb.Append(" @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@TARGET_TYPE", TARGET_TYPE);
            ht.Add("@GINS_KIND", GINS_KIND);
            ht.Add("@GINS_ITEM", GINS_ITEM);
            ht.Add("@GINS_ITEM_NAME", GINS_ITEM_NAME);

            if (AMT == "")
                ht.Add("@AMT", 0);
            else
                ht.Add("@AMT", AMT);

            ht.Add("@PERSON_QTY_S", PERSON_QTY_S);
            ht.Add("@PERSON_QTY_E", PERSON_QTY_E);
            ht.Add("@HOUSE_YN", HOUSE_YN);
            ht.Add("@EMP_RATE", EMP_RATE);
            ht.Add("@CMP_RATE", CMP_RATE);
            ht.Add("@UNION_RATE", UNION_RATE);
            ht.Add("@FEES_YN", FEES_YN);

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

    //更新 TB_I_M_GROUP_KIND
    public void updateGROUP_KIND()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_I_M_GROUP_KIND ");
            sb.Append(" set GINS_ITEM_NAME=@GINS_ITEM_NAME,AMT=@AMT,PERSON_QTY_S=@PERSON_QTY_S,");
            sb.Append(" PERSON_QTY_E=@PERSON_QTY_E,HOUSE_YN=@HOUSE_YN,EMP_RATE=@EMP_RATE,CMP_RATE=@CMP_RATE,");
            sb.Append(" UNION_RATE=@UNION_RATE,UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE()");
            sb.Append(" where TARGET_TYPE = @TARGET_TYPE and GINS_KIND = @GINS_KIND ");
            sb.Append(" and GINS_ITEM = @GINS_ITEM and PERSON_QTY_S = @PERSON_QTY_S");

            ht.Add("@TARGET_TYPE", TARGET_TYPE);
            ht.Add("@GINS_KIND", GINS_KIND);
            ht.Add("@GINS_ITEM", GINS_ITEM);
            ht.Add("@GINS_ITEM_NAME", GINS_ITEM_NAME);
            if (AMT == "")
                ht.Add("@AMT", 0);
            else
                ht.Add("@AMT", AMT);

            ht.Add("@PERSON_QTY_S", PERSON_QTY_S);
            ht.Add("@PERSON_QTY_E", PERSON_QTY_E);
            ht.Add("@HOUSE_YN", HOUSE_YN);
            ht.Add("@EMP_RATE", EMP_RATE);
            ht.Add("@CMP_RATE", CMP_RATE);
            ht.Add("@UNION_RATE", UNION_RATE);

            ht.Add("@UPDATED_BY", UPDATED_BY);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得 TB_I_M_GROUP_TXN資料
    public DataTable getExistGINS_KIND(string target_type, string gins_kind)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select COUNT(0) txncount from TB_I_M_GROUP_TXN");
            sb.Append(" where TARGET_TYPE = @TARGET_TYPE and GINS_KIND = @GINS_KIND");
            ht.Add("@TARGET_TYPE", target_type);
            ht.Add("@GINS_KIND", gins_kind);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //刪除 TB_I_M_GROUP_KIND
    public void deleteGROUP_KIND(string target_type, string gins_kind, string gins_item, string person_qty_s)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //寫log
            sb.Append(" update TB_I_M_GROUP_KIND set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2IA050' ");
            sb.Append(" where TARGET_TYPE = @TARGET_TYPE and GINS_KIND = @GINS_KIND ");
            sb.Append(" and GINS_ITEM = @GINS_ITEM and PERSON_QTY_S = @PERSON_QTY_S; ");
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);

            sb.Append(" delete from TB_I_M_GROUP_KIND ");
            sb.Append(" where TARGET_TYPE = @TARGET_TYPE and GINS_KIND = @GINS_KIND ");
            sb.Append(" and GINS_ITEM = @GINS_ITEM and PERSON_QTY_S = @PERSON_QTY_S;");
            ht.Add("@TARGET_TYPE", target_type);
            ht.Add("@GINS_KIND", gins_kind);
            ht.Add("@GINS_ITEM", gins_item);
            ht.Add("@PERSON_QTY_S", person_qty_s);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable getExistGINS_KIND2(string target_type,string gins_kind,string gins_item,string person_qty_s)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select COUNT(0) txncount from TB_I_M_GROUP_KIND");
            sb.Append(" where TARGET_TYPE = @TARGET_TYPE and GINS_KIND = @GINS_KIND");
            sb.Append(" and GINS_ITEM = @GINS_ITEM and PERSON_QTY_S = @PERSON_QTY_S");
            sb.Append(" and FEES_YN = 'Y'");
            ht.Add("@TARGET_TYPE", target_type);
            ht.Add("@GINS_KIND", gins_kind);
            ht.Add("@GINS_ITEM", gins_item);
            ht.Add("@PERSON_QTY_S", person_qty_s);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
}