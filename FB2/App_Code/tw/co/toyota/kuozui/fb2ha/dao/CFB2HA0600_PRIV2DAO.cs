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
public class CFB2HA0600_PRIV2DAO : BaseDAO
{
    public CFB2HA0600_PRIV2DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public string RowNumber { get; set; }
    public string QDATAKEY { get; set; }
    public string HR_CHG_CD { get; set; }
    public string HR_CHG_DESC { get; set; }
    public string HR_CHG_ITEM { get; set; }
    public string HR_CHG_ITEM_DESC { get; set; }
    public string IS_VALID { get; set; }
    
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
            sb.Append(" where SYS_CD='HC' and MAIN_CD='HR_CHG_ITEM'  ");
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
   
    public DataTable get_SYS_ID_Data()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select SYS_CD , MAIN_CD , SUB_CD ,SUB_DESC from TB_9_M_COMM_D");
            sb.Append(" where SYS_CD='HC' and MAIN_CD=HR_CHG_ITEM  ");

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string HR_CHG_CD, string HR_CHG_ITEM)
    {
        try
        {
            if (sortExpression == "")
            {
                sortExpression = "HR_CHG_CD";
            }
            else {
                if (sortExpression.Contains("HR_CHG_DESC"))
                {
                    sortExpression = string.Format("b.{0}", sortExpression);
                }
                else {
                    sortExpression = string.Format("a.{0}", sortExpression);
                }
            }
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * From");
            sb.Append(" (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append("a.*,b.HR_CHG_DESC as HR_CHG_B ");
            sb.Append(" from TB_H_M_HR_CHANGE_CODE_ITEM a");
            sb.Append(" left join TB_H_M_HR_CHANGE_CODE b on a.HR_CHG_CD=b.HR_CHG_CD");
            sb.Append(" left join TB_9_M_COMM_D c on c.SYS_CD='HC' and c.MAIN_CD='HR_CHG_ITEM' and c.IS_VALID='Y' and a.HR_CHG_ITEM=c.SUB_CD");
            sb.Append(" where 1=1");
            if (HR_CHG_CD != "")
            {
                sb.Append(" and a.HR_CHG_CD LIKE @HR_CHG_CD ");
                ht.Add("@HR_CHG_CD", string.Format("%{0}%", HR_CHG_CD));
            }
            if (HR_CHG_ITEM != "" && HR_CHG_ITEM != "-1")
            {
                sb.Append(" and HR_CHG_ITEM = @HR_CHG_ITEM ");
                ht.Add("@HR_CHG_ITEM", HR_CHG_ITEM.Substring(0, 2));
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
    public int getCount(int startRowIndex, int maximumRows, string HR_CHG_CD, string HR_CHG_ITEM)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) total_record ");
            sb.Append(" from TB_H_M_HR_CHANGE_CODE_ITEM");
            sb.Append(" where 1=1");
            if (HR_CHG_CD != "")
            {
                sb.Append(" and HR_CHG_CD LIKE @HR_CHG_CD ");
                ht.Add("@HR_CHG_CD", string.Format("%{0}%", HR_CHG_CD));
            }
            if (HR_CHG_ITEM != "" && HR_CHG_ITEM != "-1")
            {
                sb.Append(" and HR_CHG_ITEM = @HR_CHG_ITEM ");
                ht.Add("@HR_CHG_ITEM", HR_CHG_ITEM.Substring(0, 2));
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


    public string deleteData(string deleteitem)
    {
        string[] arrstr = deleteitem.Split('|');

        //刪除共用代碼主檔
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        //寫log
        sb.Append(" update TB_H_M_HR_CHANGE_CODE_ITEM set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2HA060' ");
        sb.Append(" where HR_CHG_CD = @HR_CHG_CD AND HR_CHG_ITEM = @HR_CHG_ITEM; ");
        ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);

        sb.Append(" Delete from TB_H_M_HR_CHANGE_CODE_ITEM ");
        sb.Append(" where HR_CHG_CD = @HR_CHG_CD AND HR_CHG_ITEM = @HR_CHG_ITEM; ");
        ht.Add("@HR_CHG_CD", arrstr[0]);
        ht.Add("@HR_CHG_ITEM", arrstr[1]);
        dbConn.ExecuteT(sb, ht, true);

        return "0";
    }
    internal DataTable getExistData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select * from TB_H_M_HR_CHANGE_CODE_ITEM where HR_CHG_CD = @HR_CHG_CD and HR_CHG_ITEM = @HR_CHG_ITEM");
            ht.Add("@HR_CHG_CD", HR_CHG_CD);
            ht.Add("@HR_CHG_ITEM", HR_CHG_ITEM);

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
            sb.Append("INSERT INTO TB_H_M_HR_CHANGE_CODE_ITEM (IS_VALID,HR_CHG_CD,HR_CHG_DESC,HR_CHG_ITEM,HR_CHG_ITEM_DESC,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" Values (@IS_VALID,@HR_CHG_CD,@HR_CHG_DESC,@HR_CHG_ITEM,@HR_CHG_ITEM_DESC,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");
            ht.Add("@IS_VALID", IS_VALID);
            ht.Add("@HR_CHG_CD", HR_CHG_CD);
            ht.Add("@HR_CHG_DESC", HR_CHG_DESC);
            ht.Add("@HR_CHG_ITEM", HR_CHG_ITEM);
            ht.Add("@HR_CHG_ITEM_DESC", HR_CHG_ITEM_DESC);
            ht.Add("@CREATED_BY", CREATED_BY);
            ht.Add("@UPDATED_BY", CREATED_BY);
            ht.Add("@FUNC_ID", "FB2HA060");
            
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
            sb.Append("Update TB_H_M_HR_CHANGE_CODE_ITEM ");
            sb.Append(" Set HR_CHG_CD=@HR_CHG_CD,HR_CHG_DESC=@HR_CHG_DESC,HR_CHG_ITEM=@HR_CHG_ITEM,HR_CHG_ITEM_DESC=@HR_CHG_ITEM_DESC,UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID");
            //sb.Append(" Set HR_CHG_CD=@HR_CHG_CD,HR_CHG_ITEM=@HR_CHG_ITEM,UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE()");
            sb.Append(" where HR_CHG_CD = @HR_CHG_CD");
            ht.Add("@HR_CHG_CD", HR_CHG_CD);
            ht.Add("@HR_CHG_DESC", HR_CHG_DESC);
            ht.Add("@HR_CHG_ITEM", HR_CHG_ITEM);
            ht.Add("@HR_CHG_ITEM_DESC", HR_CHG_ITEM_DESC);
            ht.Add("@UPDATED_BY", CREATED_BY);
            ht.Add("@FUNC_ID", "FB2HA060");

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
}