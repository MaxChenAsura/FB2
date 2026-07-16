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
/// CFB2DC0200DAO 的摘要描述
/// </summary>
public class CFB2DC0200DAO : BaseDAO
{

    public string CARD_TYPE { get; set; }
    public string CARD_TYPE_DESC { get; set; }
    public string CLOCK_TYPE_A { get; set; }
    public string CLOCK_TYPE_B { get; set; }
    public string CLOCK_TYPE_C { get; set; }
    public string CARD_USED_CD { get; set; }
    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }

    public CFB2DC0200DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string card_type)
    {
        try
        {
            if (sortExpression.Contains("CARD_TYPE"))
                sortExpression = sortExpression.Replace("CARD_TYPE", "a.CARD_TYPE");

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * From");
            sb.Append(" (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" a.CARD_TYPE,a.CARD_TYPE_DESC,a.CLOCK_TYPE_A,a.CLOCK_TYPE_B,a.CLOCK_TYPE_C,");
            sb.Append(" b.SUB_CD+'-'+b.SUB_DESC CARD_USED_CD");
            sb.Append(" from TB_D_M_CARD_TYPE a ");
            sb.Append(" left join TB_9_M_COMM_D b on b.SYS_CD='DC' and b.MAIN_CD='CARD_USED_CD' and b.SUB_CD=a.CARD_USED_CD ");
            sb.Append(" where 1=1 ");

            if (card_type != "-1" && card_type != null)
            {
                sb.Append(" and a.CARD_TYPE = @CARD_TYPE ");
                ht.Add("@CARD_TYPE", card_type);
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

    public int getCount(int startRowIndex, int maximumRows, string card_type)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select COUNT(*) total_record ");
            sb.Append(" from TB_D_M_CARD_TYPE a ");
            sb.Append(" left join TB_9_M_COMM_D b on b.SYS_CD='DC' and b.MAIN_CD='CARD_USED_CD' and b.SYS_CD=a.CARD_USED_CD ");
            sb.Append(" where 1=1 ");

            if (card_type != "-1" && card_type != null)
            {
                sb.Append(" and a.CARD_TYPE = @CARD_TYPE ");
                ht.Add("@CARD_TYPE", card_type);
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

    public void deleteCARD_TYPE(string item)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //寫log
            sb.Append(" update TB_D_M_CARD_TYPE set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2DC020' ");
            sb.Append(" where CARD_TYPE = @CARD_TYPE;");
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);

            sb.Append(" delete from TB_D_M_CARD_TYPE");
            sb.Append(" where CARD_TYPE = @CARD_TYPE;");
            ht.Add("@CARD_TYPE", item);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getExistData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select CARD_TYPE from TB_D_M_CARD_TYPE");
            sb.Append(" where CARD_TYPE = @CARD_TYPE");
            ht.Add("@CARD_TYPE", CARD_TYPE);

            return dbConn.Query(sb, ht);
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    public void addCLOCK_TYPE()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("insert into TB_D_M_CARD_TYPE ( ");
            sb.Append(" CARD_TYPE,CARD_TYPE_DESC,CLOCK_TYPE_A,CLOCK_TYPE_B,CLOCK_TYPE_C,CARD_USED_CD,");
            sb.Append(" CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" values ( ");
            sb.Append(" @CARD_TYPE,@CARD_TYPE_DESC,@CLOCK_TYPE_A,@CLOCK_TYPE_B,@CLOCK_TYPE_C,@CARD_USED_CD,");
            sb.Append(" @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@CARD_TYPE", CARD_TYPE);
            ht.Add("@CARD_TYPE_DESC", CARD_TYPE_DESC);
            ht.Add("@CLOCK_TYPE_A", CLOCK_TYPE_A);
            ht.Add("@CLOCK_TYPE_B", CLOCK_TYPE_B);
            ht.Add("@CLOCK_TYPE_C", CLOCK_TYPE_C);
            ht.Add("@CARD_USED_CD", CARD_USED_CD);
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

    public void updateCARD_TYPE()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_D_M_CARD_TYPE ");
            sb.Append(" set CARD_TYPE_DESC=@CARD_TYPE_DESC,CLOCK_TYPE_A=@CLOCK_TYPE_A,CLOCK_TYPE_B=@CLOCK_TYPE_B,");
            sb.Append(" CLOCK_TYPE_C=@CLOCK_TYPE_C,CARD_USED_CD=@CARD_USED_CD,");
            sb.Append(" UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID");
            sb.Append(" where CARD_TYPE=@CARD_TYPE ");

            ht.Add("@CARD_TYPE", CARD_TYPE);
            ht.Add("@CARD_TYPE_DESC", CARD_TYPE_DESC);
            ht.Add("@CLOCK_TYPE_A", CLOCK_TYPE_A);
            ht.Add("@CLOCK_TYPE_B", CLOCK_TYPE_B);
            ht.Add("@CLOCK_TYPE_C", CLOCK_TYPE_C);
            ht.Add("@CARD_USED_CD", CARD_USED_CD);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable getCARD_TYPE()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select distinct CARD_TYPE+'-'+CARD_TYPE_DESC CARD_TYPE_DESC,CARD_TYPE");
            sb.Append(" from TB_D_M_CARD_TYPE");
            sb.Append(" order by CARD_TYPE");
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
}