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
/// CFB2990200DAO 的摘要描述
/// </summary>
public class CFB2990200DAO : BaseDAO
{
    public CFB2990200DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public string RowNumber { get; set; }
    public string QDATAKEY { get; set; }
    public string SYS_CD { get; set; }
    public string MAIN_CD { get; set; }
    public string MAIN_DESC { get; set; }
    public string CODE_VAL1 { get; set; }
    public string REMARK { get; set; }
    public string FUNC_ID { get; set; }
    //for查詢欄位
    public string ddl_SYS_CD { get; set; }



    public DataTable get_SYS_CD_Data()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select distinct SYS_CD from TB_9_M_PARAMETER ");
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string sys_cd, string main_cd,string main_desc)
    {
        try
        {
            if (sortExpression == "")
            {
                sortExpression = "SYS_CD";
            }
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * From");
            sb.Append(" (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" SYS_CD+MAIN_CD as qdatakey,SYS_CD,MAIN_CD,MAIN_DESC,CODE_VAL1,REMARK");
            sb.Append(" from TB_9_M_PARAMETER");
            sb.Append(" where 1=1");

            if (sys_cd != "")
            {
                sb.Append(" and SYS_CD = @SYS_CD  ");
                ht.Add("@SYS_CD", sys_cd);
            }
            if (main_cd != "")
            {
                sb.Append(" and MAIN_CD like '%'+ @MAIN_CD +'%' ");
                ht.Add("@MAIN_CD", main_cd);
            }
            if (main_desc != "")
            {
                sb.Append(" and MAIN_DESC like '%'+ @MAIN_DESC +'%'  ");
                ht.Add("@MAIN_DESC", main_desc);
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
    public int getCount(int startRowIndex, int maximumRows, string sys_cd,string main_cd ,string main_desc)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) total_record ");
            sb.Append(" from TB_9_M_PARAMETER");
            sb.Append(" where 1=1");
            if (sys_cd != "")
            {
                sb.Append(" and SYS_CD = @SYS_CD  ");
                ht.Add("@SYS_CD", sys_cd);
            }
            if (main_cd != "")
            {
                sb.Append(" and MAIN_CD like '%'+ @MAIN_CD +'%' ");
                ht.Add("@MAIN_CD", main_cd);
            }
            if (main_desc != "")
            {
                sb.Append(" and MAIN_DESC like '%'+ @MAIN_DESC +'%' ");
                ht.Add("@MAIN_DESC", main_desc);
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
    //public DataTable getData()
    //{
    //    try
    //    {
    //        StringBuilder sb = new StringBuilder();
    //        sb.Append(" Select * From TB_9_M_COMM_H";
    //         sb.Append(" where 1=1";

    //        if (SYS_CD != "")
    //        {
    //             sb.Append(" and SYS_CD = @SYS_CD ";
    //            ht.Add("@SYS_CD", SYS_CD);
    //        }

    //        return dbConn.Query(sb, ht);
    //    }
    //    catch (Exception)
    //    {

    //        throw;
    //    }
    //}
    public string deleteData(string sys_cd,string main_cd)
    {
        //刪除共用代碼主檔
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        //寫log
        sb.AppendLine(" update TB_9_M_PARAMETER set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB299020' ");
        sb.AppendLine(" where SYS_CD = @SYS_CD and MAIN_CD = @MAIN_CD; ");

        sb.Append("Delete from TB_9_M_PARAMETER ");
        sb.Append(" where SYS_CD = @SYS_CD and MAIN_CD = @MAIN_CD;");
        ht.Add("@SYS_CD", sys_cd);
        ht.Add("@MAIN_CD", main_cd);
        ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);
        dbConn.ExecuteT(sb, ht, true);

        return "0";
    }
    internal DataTable getExistData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select * from TB_9_M_PARAMETER where SYS_CD+MAIN_CD = @SYS_CD+@MAIN_CD");
            ht.Add("@SYS_CD", SYS_CD);
            ht.Add("@MAIN_CD", MAIN_CD);

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
            sb.Append("INSERT INTO TB_9_M_PARAMETER (SYS_CD,MAIN_CD,MAIN_DESC,CODE_VAL1,REMARK,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" Values (@SYS_CD,@MAIN_CD,@MAIN_DESC,@CODE_VAL1,@REMARK,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");
            ht.Add("@SYS_CD", SYS_CD);
            ht.Add("@MAIN_CD", MAIN_CD);
            ht.Add("@MAIN_DESC", MAIN_DESC);
            ht.Add("@CODE_VAL1", CODE_VAL1);
            ht.Add("@REMARK", REMARK);
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB299020");

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
            sb.Append("Update TB_9_M_PARAMETER ");
            sb.Append(" Set MAIN_DESC = @MAIN_DESC,CODE_VAL1 = @CODE_VAL1,REMARK = @REMARK,UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID");
            sb.Append(" where SYS_CD = @SYS_CD and MAIN_CD = @MAIN_CD");
            ht.Add("@SYS_CD", SYS_CD);
            ht.Add("@MAIN_CD", MAIN_CD);
            ht.Add("@MAIN_DESC", MAIN_DESC);
            ht.Add("@CODE_VAL1", CODE_VAL1);
            ht.Add("@REMARK", REMARK);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB299020");
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
}