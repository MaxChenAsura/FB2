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
/// CFB2990100DAO 的摘要描述
/// </summary>
public class CFB2990100DAO : BaseDAO
{
    public CFB2990100DAO()
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
    public string USER_UPD { get; set; }
    public string SUB_CD { get; set; }
    public string SUB_DESC { get; set; }
    public string CODE_VAL1 { get; set; }
    public string CODE_VAL2 { get; set; }
    public string REMARK { get; set; }
    public string IS_VALID { get; set; }
    public string ORDER_SEQ { get; set; }
    public string FUNC_ID { get; set; }
    //for查詢欄位
    public string ddl_SYS_CD { get; set; }

    #region Qry

    public DataTable get_SYS_CD_Data()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine("select distinct SYS_CD from TB_9_M_COMM_H;");
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string sys_cd,string main_cd,string main_desc)
    {
        try
        {
            if (sortExpression == "")
            {
                sortExpression = "SYS_CD";
            }
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" select * From");
            sb.AppendLine("         (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.AppendLine("             SYS_CD+MAIN_CD as qdatakey,SYS_CD,MAIN_CD,MAIN_DESC,USER_UPD");
            sb.AppendLine("            from TB_9_M_COMM_H");
            sb.AppendLine("           where 1=1");

            if (sys_cd != "" )
            {
                sb.AppendLine(" and SYS_CD = @SYS_CD  ");
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
            sb.AppendLine(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.AppendLine(" AND CAST(@startRowIndex+@maximumRows as varchar)");

            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public int getCount(int startRowIndex, int maximumRows, string sys_cd,string main_cd,string main_desc)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" select count(*) total_record ");
            sb.AppendLine(" from TB_9_M_COMM_H ");
            sb.AppendLine(" where 1=1");
            if (sys_cd != "" )
            {
                sb.AppendLine(" and SYS_CD = @SYS_CD ");
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
    //        StringBuilder sb = new StringBuilder();Hashtable ht = new Hashtable();
    //        sb.AppendLine(" Select * From TB_9_M_COMM_H";
    //         sb.AppendLine(" where 1=1";

    //        if (SYS_CD != "")
    //        {
    //             sb.AppendLine(" and SYS_CD = @SYS_CD ";
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
        sb.AppendLine(" update TB_9_M_COMM_H set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB299010' ");
        sb.AppendLine(" where SYS_CD = @SYS_CD and MAIN_CD = @MAIN_CD; ");

        sb.AppendLine(" delete from TB_9_M_COMM_H ");
        sb.AppendLine(" where SYS_CD = @SYS_CD and MAIN_CD = @MAIN_CD; ");
        ht.Add("@SYS_CD", sys_cd);
        ht.Add("@MAIN_CD", main_cd);
        ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);
        dbConn.ExecuteT(sb, ht, true);

        //刪除共用代碼主檔後 連帶共用代碼明細檔刪除
        StringBuilder sb2 = new StringBuilder();
        Hashtable ht2 = new Hashtable();
        //寫log
        sb2.AppendLine(" update TB_9_M_COMM_D set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB299010' ");
        sb2.AppendLine(" where SYS_CD = @SYS_CD and MAIN_CD = @MAIN_CD; ");

        sb2.AppendLine(" delete from TB_9_M_COMM_D ");
        sb2.AppendLine(" where SYS_CD = @SYS_CD and MAIN_CD = @MAIN_CD; ");
        ht2.Add("@SYS_CD", sys_cd);
        ht2.Add("@MAIN_CD", main_cd);
        ht2.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);
        dbConn.ExecuteT(sb2, ht2, true);
        return "0";
    }
    internal DataTable getExistData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine("Select * from TB_9_M_COMM_H where SYS_CD+MAIN_CD = @SYS_CD+@MAIN_CD");
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

            sb.AppendLine("INSERT INTO TB_9_M_COMM_H (SYS_CD,MAIN_CD,MAIN_DESC,USER_UPD,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.AppendLine(" Values (@SYS_CD,@MAIN_CD,@MAIN_DESC,@USER_UPD,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");
            ht.Add("@SYS_CD", SYS_CD);
            ht.Add("@MAIN_CD", MAIN_CD);
            ht.Add("@MAIN_DESC", MAIN_DESC);
            ht.Add("@USER_UPD", USER_UPD);
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB299010");

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

            sb.AppendLine(" update TB_9_M_COMM_H ");
            sb.AppendLine(" set MAIN_DESC = @MAIN_DESC,USER_UPD = @USER_UPD,UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID");
            sb.AppendLine(" where SYS_CD+MAIN_CD = @SYS_CD+@MAIN_CD");
            ht.Add("@SYS_CD", SYS_CD);
            ht.Add("@MAIN_CD", MAIN_CD);
            ht.Add("@MAIN_DESC", MAIN_DESC);
            ht.Add("@USER_UPD", USER_UPD);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB299010");

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    #endregion

    #region Dtl

    //查詢明細表頭部分
    public DataTable getDtlHeader()
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();

        sb.AppendLine(" select * ");
        sb.AppendLine(" from TB_9_M_COMM_H");
        sb.AppendLine(" where SYS_CD = @SYS_CD and MAIN_CD = @MAIN_CD");
        ht.Add("@SYS_CD", SYS_CD);
        ht.Add("@MAIN_CD", MAIN_CD);
        return dbConn.Query(sb, ht);
    }
    //查詢明細
    public DataTable getDtlData(int startRowIndex, int maximumRows, string sortExpression, string sys_cd,string main_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select * From");
            sb.AppendLine(" (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.AppendLine(" SYS_CD, SUB_CD, SUB_DESC, CODE_VAL1, CODE_VAL2, REMARK, IS_VALID, ORDER_SEQ ");
            sb.AppendLine(" from TB_9_M_COMM_D");
            sb.AppendLine(" where SYS_CD = @SYS_CD and MAIN_CD = @MAIN_CD");
            ht.Add("@SYS_CD", sys_cd);
            ht.Add("@MAIN_CD", main_cd);
            sb.AppendLine(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.AppendLine(" AND CAST(@startRowIndex+@maximumRows as varchar)");
            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }

    }
    public int getDtlCount(int startRowIndex, int maximumRows, string sys_cd, string main_cd)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select COUNT(*) total_record ");
            sb.AppendLine(" from TB_9_M_COMM_D");
            sb.AppendLine(" where SYS_CD = @SYS_CD and MAIN_CD = @MAIN_CD");
            ht.Add("@SYS_CD", sys_cd);
            ht.Add("@MAIN_CD", main_cd);

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
    public string deleteDtlData(string deleteDtlItem, string sys_cd,string main_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //寫log
            sb.AppendLine(" update TB_9_M_COMM_D set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB299010' ");
            sb.AppendLine(" where SYS_CD = @SYS_CD and MAIN_CD = @MAIN_CD and SUB_CD = @SUB_CD; ");

            sb.AppendLine(" delete from TB_9_M_COMM_D  ");
            sb.AppendLine(" where SYS_CD = @SYS_CD and MAIN_CD = @MAIN_CD");
            sb.AppendLine(" and SUB_CD = @SUB_CD;");
            ht.Add("@SYS_CD", sys_cd);
            ht.Add("@MAIN_CD", main_cd);
            ht.Add("@SUB_CD", deleteDtlItem);
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);
            dbConn.ExecuteT(sb, ht, true);
            return "0";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    internal DataTable getExistDtlData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select * from TB_9_M_COMM_D ");
            sb.AppendLine(" where SYS_CD = @SYS_CD and MAIN_CD = @MAIN_CD and ");
            sb.AppendLine(" SUB_CD = @SUB_CD ");
            ht.Add("@SYS_CD", SYS_CD);
            ht.Add("@MAIN_CD", MAIN_CD);
            ht.Add("@SUB_CD", SUB_CD);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
    internal void addDtlData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" insert into TB_9_M_COMM_D (SYS_CD,MAIN_CD,IS_VALID,SUB_CD,SUB_DESC,CODE_VAL1,CODE_VAL2,REMARK,ORDER_SEQ,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID) ");
            sb.AppendLine(" values (@SYS_CD,@MAIN_CD,@IS_VALID,@SUB_CD,@SUB_DESC,@CODE_VAL1,@CODE_VAL2,@REMARK,@ORDER_SEQ,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID) ");
            ht.Add("@SYS_CD", SYS_CD);
            ht.Add("@MAIN_CD", MAIN_CD);
            ht.Add("@IS_VALID", IS_VALID);
            ht.Add("@SUB_CD", SUB_CD);
            ht.Add("@SUB_DESC", SUB_DESC);
            ht.Add("@CODE_VAL1", CODE_VAL1);
            ht.Add("@CODE_VAL2", CODE_VAL2);
            ht.Add("@REMARK", REMARK);
            if (string.IsNullOrEmpty(ORDER_SEQ))
            {
                ht.Add("@ORDER_SEQ", "0");
            }
            else
            {
                ht.Add("@ORDER_SEQ", ORDER_SEQ);
            }
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB299010");

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    internal void updateDtlData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" update TB_9_M_COMM_D ");
            sb.AppendLine(" set SUB_DESC = @SUB_DESC,CODE_VAL1 = @CODE_VAL1,CODE_VAL2 = @CODE_VAL2,REMARK = @REMARK,ORDER_SEQ = @ORDER_SEQ,IS_VALID = @IS_VALID ");
            sb.AppendLine(" ,UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID ");
            sb.AppendLine(" where SYS_CD = @SYS_CD and MAIN_CD = @MAIN_CD and SUB_CD = @SUB_CD ");
            ht.Add("@SYS_CD", SYS_CD);
            ht.Add("@MAIN_CD", MAIN_CD);
            ht.Add("@SUB_CD", SUB_CD);
            ht.Add("@SUB_DESC", SUB_DESC);
            ht.Add("@IS_VALID", IS_VALID);
            ht.Add("@CODE_VAL1", CODE_VAL1);
            ht.Add("@CODE_VAL2", CODE_VAL2);
            ht.Add("@REMARK", REMARK);
            if (string.IsNullOrEmpty(ORDER_SEQ))
            {
                ht.Add("@ORDER_SEQ", "0");
            }
            else
            {
                ht.Add("@ORDER_SEQ", ORDER_SEQ);
            }
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB299010");

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    #endregion
}