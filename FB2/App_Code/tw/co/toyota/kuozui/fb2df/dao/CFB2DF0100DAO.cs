using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;

/// <summary>
/// CFB2DF0100DAO 的摘要描述
/// </summary>
public class CFB2DF0100DAO : BaseDAO
{
    public CFB2DF0100DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public string BASE_NO { get; set; }

    public string BASE_NAME { get; set; }

    public string AMOUNT { get; set; }

    public string CREATED_BY { get; set; }

    public string UPDATED_BY { get; set; }

    public string FUNC_ID { get; set; }

    //Gridview 查詢資料
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" BASE_NO,BASE_NAME,AMOUNT");
            sb.Append(" from TB_D_M_ACCOM_BASE ");

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
    public int getCount(int startRowIndex, int maximumRows)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select COUNT(*) total_record ");
            sb.Append(" from TB_D_M_ACCOM_BASE ");

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

    internal DataTable getUsedBaseNo(string item)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select count(BASE_NO) basecount from TB_D_M_ACCOM_MAIN");
            sb.Append(" where BASE_NO = @BASE_NO and (END_DT is not null or END_DT > GETDATE())");
            ht.Add("@BASE_NO", item);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void deleteBaseNo(string item)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //寫log
            sb.Append(" update TB_D_M_ACCOM_BASE set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2DF010' ");
            sb.Append(" where BASE_NO = @BASE_NO; ");
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);

            sb.Append(" delete from TB_D_M_ACCOM_BASE");
            sb.Append(" where BASE_NO = @BASE_NO; ");
            ht.Add("@BASE_NO", item);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void addBaseNo(CFB2DF0100DAO dao)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("insert into TB_D_M_ACCOM_BASE (BASE_NO,BASE_NAME,AMOUNT,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" values (@BASE_NO,@BASE_NAME,@AMOUNT,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID) ");
            ht.Add("@BASE_NO", dao.BASE_NO);
            ht.Add("@BASE_NAME", dao.BASE_NAME);
            ht.Add("@AMOUNT", dao.AMOUNT);
            ht.Add("@CREATED_BY", dao.CREATED_BY);
            ht.Add("@UPDATED_BY", dao.UPDATED_BY);
            ht.Add("@FUNC_ID", dao.FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void updateBaseNo(CFB2DF0100DAO dao)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("update TB_D_M_ACCOM_BASE set BASE_NAME = @BASE_NAME,AMOUNT = @AMOUNT,UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE()");
            sb.Append(" from TB_D_M_ACCOM_BASE where BASE_NO = @BASE_NO");
            ht.Add("@BASE_NO", dao.BASE_NO);
            ht.Add("@BASE_NAME", dao.BASE_NAME);
            ht.Add("@AMOUNT", dao.AMOUNT);
            ht.Add("@UPDATED_BY", dao.UPDATED_BY);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void updateAccom(CFB2DF0100DAO dao)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("update TB_D_M_ACCOM_MAIN set AMOUNT = @AMOUNT,UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE()");
            sb.Append(" from TB_D_M_ACCOM_MAIN where BASE_NO = @BASE_NO and (END_DT = null or END_DT > GETDATE())");
            ht.Add("@BASE_NO", dao.BASE_NO);
            ht.Add("@AMOUNT", dao.AMOUNT);
            ht.Add("@UPDATED_BY", dao.UPDATED_BY);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getExistBaseNo(string base_no)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select base_no basecount from TB_D_M_ACCOM_BASE");
            sb.Append(" where BASE_NO = @BASE_NO ");
            ht.Add("@BASE_NO", base_no);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            
            throw;
        }
        
    }
}