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
/// CFB2SC1200DAO 的摘要描述
/// </summary>
public class CFB2SC1200DAO : BaseDAO
{
    public CFB2SC1200DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public string RowNumber { get; set; }
    public string KIND_CD { get; set; }
    public string GROUP_TYPE { get; set; }
    public string GROUP_ID { get; set; }
    public string GROUP_NAME { get; set; }
    public string LEVEL { get; set; }
    public string CLASSIFY { get; set; }
    public string ORDER_SEQ { get; set; }
    public string FUNC_ID { get; set; }
    public string SUB_GROUP_ID { get; set; }
    public string DATA_SCOPE { get; set; }
    public string CD_TYPE { get; set; }
    public string ACCOUNTING_NO1 { get; set; }
    public string ACCOUNTING_NO2 { get; set; }
    public string PAY_CD { get; set; }
    public string VOUCHER_FORMAT { get; set; }
    public string INV_TYPE { get; set; }
    public string VOU_PAY_TARGET { get; set; }
    public string VOU_PAY_TYPE { get; set; }
    public string VOU_PAY_DT_SRC { get; set; }
    public string BUDGET_DEPT { get; set; }
    public string MEMO { get; set; }
    public string VOU_VENDOR_CD { get; set; }
    public string ACCOUNTING_NO3 { get; set; }
    public string ACCOUNTING_NO4 { get; set; }
    public string ACCOUNTING_NO5 { get; set; }
    public string IS_SHARE { get; set; }
    public string ORI_ACCOUNTING_NO1 { get; set; }

    //for查詢欄位
    public string ddl_SYS_CD { get; set; }

    public DataTable getCommCode(string sys_cd, string main_cd, string is_valid)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select sub_cd ,sub_cd+'-'+sub_desc as sub_desc ");
            sb.Append(" from TB_9_M_COMM_D ");
            sb.Append(" where SYS_CD = @SYS_CD ");
            sb.Append(" and MAIN_CD = @MAIN_CD ");
            ht.Add("@SYS_CD", sys_cd);
            ht.Add("@MAIN_CD", main_cd);
            if (!string.IsNullOrEmpty(is_valid))
            {
                sb.Append(" and IS_VALID = @IS_VALID ");
                ht.Add("@IS_VALID", is_valid);
            }

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getSALARY_NAME(string salary_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select SALARY_NAME ");
            sb.Append(" from TB_S_M_SALARY_ITEM ");
            sb.Append(" where SALARY_ID = @SALARY_ID ");
            ht.Add("@SALARY_ID", salary_id);

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    #region Qry

    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string kind_cd, string group_type,
                             string group_name, string classify, string group_id, string level, string sub_group_id, string sub_group_name)
    {
        try
        {
            if (sortExpression == "")
            {
                sortExpression = "alltb.KIND_CD ASC,alltb.ORDER_SEQ";
            }
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" select * from");
            sb.Append("      (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,* ");
            sb.Append("       from(select distinct t1.KIND_CD as KIND_CD,t1.GROUP_TYPE as GROUP_TYPE,t1.GROUP_ID as GROUP_ID ,t1.LEVEL");
            sb.Append("          ,t1.GROUP_NAME,t1.ORDER_SEQ,t1.CLASSIFY ");
            sb.Append("          ,t1.KIND_CD +'-'+ d.SUB_DESC as DESC1 ");
            sb.Append("          ,t1.GROUP_TYPE +'-'+ e.SUB_DESC as DESC2 ");
            sb.Append("          ,t1.CLASSIFY +'-'+(CASE t1.KIND_CD WHEN 'D' THEN fd0.SUB_DESC ELSE fd1.SUB_DESC END) as DESC3 ");
            sb.Append("          ,t1.KIND_CD + t1.GROUP_TYPE + t1.GROUP_ID as qdatakey");
            sb.Append("          from TB_S_M_SALARY_GROUP_H t1 ");
            sb.Append("          left join TB_S_M_SALARY_GROUP_D t2  on t1.KIND_CD = t2.KIND_CD and t1.GROUP_TYPE = t2.GROUP_TYPE and t1.GROUP_ID = t2.GROUP_ID ");
            sb.Append("          left join TB_9_M_COMM_D d on  d.SYS_CD ='SC' and  d.MAIN_CD='KIND_CD' and  t1.KIND_CD = d.SUB_CD ");
            sb.Append("          left join TB_9_M_COMM_D e on  e.SYS_CD ='SC' and  e.MAIN_CD='SALARY_TYPE' and  t1.GROUP_TYPE = e.SUB_CD ");
            sb.Append("         left join TB_9_M_COMM_D fd1 on  fd1.SYS_CD ='SC' and  fd1.MAIN_CD='CLASSIFY' and  t1.CLASSIFY = fd1.SUB_CD AND t1.KIND_CD <>'D' ");
            sb.Append("          left join TB_9_M_COMM_D fd0 on  fd0.SYS_CD ='SC' and  fd0.MAIN_CD='VOUCHER_FORMAT' and  t1.CLASSIFY = fd0.SUB_CD AND t1.KIND_CD = 'D' ");
            sb.Append("          left join TB_S_M_SALARY_ITEM s on t1.KIND_CD <>'D' and t2.SUB_GROUP_ID = s.SALARY_ID ");
            sb.Append("          left join TB_S_M_SALARY_GROUP_H g on g.GROUP_ID = t2.SUB_GROUP_ID ");
            sb.Append("          where 1=1");

            if (kind_cd != "")
            {
                sb.Append(" and t1.KIND_CD = @KIND_CD  ");
                ht.Add("@KIND_CD", kind_cd);
            }
            if (group_type != "")
            {
                sb.Append(" and t1.GROUP_TYPE = @GROUP_TYPE  ");
                ht.Add("@GROUP_TYPE", group_type);
            }
            if (group_name != "")
            {
                sb.Append(" and t1.GROUP_NAME like '%'+ @GROUP_NAME +'%'  ");
                ht.Add("@GROUP_NAME", group_name);
            }
            if (classify != "")
            {
                sb.Append(" and t1.CLASSIFY = @CLASSIFY ");
                ht.Add("@CLASSIFY", classify);
            }
            if (group_id != "")
            {
                sb.Append(" and t1.GROUP_ID = @GROUP_ID  ");
                ht.Add("@GROUP_ID", group_id);
            }
            if (level != "")
            {
                sb.Append(" and t1.LEVEL = @LEVEL  ");
                ht.Add("@LEVEL", level);
            }
            if (sub_group_id != "")
            {
                sb.Append(" and t2.SUB_GROUP_ID = @SUB_GROUP_ID  ");
                ht.Add("@SUB_GROUP_ID", sub_group_id);
            }
            if (sub_group_name != "")
            {
                sb.Append(" and ( s.SALARY_NAME like '%'+ @SUB_GROUP_NAME +'%' or g.GROUP_NAME like '%'+ @SUB_GROUP_NAME +'%' )  ");
                ht.Add("@SUB_GROUP_NAME", sub_group_name);
            }

            sb.Append("         )alltb ");
            sb.Append("     )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar)");

            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);

            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch
        {
            throw;
        }
    }
    public int getCount(int startRowIndex, int maximumRows, string kind_cd, string group_type, string group_name
                        , string classify, string group_id, string level, string sub_group_id, string sub_group_name)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select COUNT(*) total_record ");
            sb.Append("   from(select distinct t1.KIND_CD as KIND_CD,t1.GROUP_TYPE as GROUP_TYPE,t1.GROUP_ID as GROUP_ID ,t1.LEVEL");
            sb.Append("      ,t1.GROUP_NAME,t1.ORDER_SEQ,t1.CLASSIFY ");
            sb.Append("      ,t1.KIND_CD +'-'+ d.SUB_DESC as DESC1 ");
            sb.Append("      ,t1.GROUP_TYPE +'-'+ e.SUB_DESC as DESC2 ");
            sb.Append("      ,t1.CLASSIFY +'-'+ f.SUB_DESC as DESC3 ");
            sb.Append("      ,t1.KIND_CD + t1.GROUP_TYPE + t1.GROUP_ID as qdatakey");
            sb.Append("      from TB_S_M_SALARY_GROUP_H t1 ");
            sb.Append("      left join TB_S_M_SALARY_GROUP_D t2  on t1.KIND_CD = t2.KIND_CD and t1.GROUP_TYPE = t2.GROUP_TYPE and t1.GROUP_ID = t2.GROUP_ID ");
            sb.Append("      left join TB_9_M_COMM_D d on  d.SYS_CD ='SC' and  d.MAIN_CD='KIND_CD' and  t1.KIND_CD = d.SUB_CD ");
            sb.Append("      left join TB_9_M_COMM_D e on  e.SYS_CD ='SC' and  e.MAIN_CD='GROUP_TYPE' and  t1.GROUP_TYPE = e.SUB_CD ");
            sb.Append("      left join TB_9_M_COMM_D f on  f.SYS_CD ='SC' and  f.MAIN_CD='CLASSIFY' and  t1.CLASSIFY = f.SUB_CD ");
            sb.Append("      left join TB_S_M_SALARY_ITEM s on t1.KIND_CD <>'D' and t2.SUB_GROUP_ID = s.SALARY_ID ");
            sb.Append("      left join TB_S_M_SALARY_GROUP_H g on g.GROUP_ID = t2.SUB_GROUP_ID ");
            sb.Append("      where 1=1");

            if (kind_cd != "")
            {
                sb.Append(" and t1.KIND_CD = @KIND_CD  ");
                ht.Add("@KIND_CD", kind_cd);
            }
            if (group_type != "")
            {
                sb.Append(" and t1.GROUP_TYPE = @GROUP_TYPE  ");
                ht.Add("@GROUP_TYPE", group_type);
            }
            if (group_name != "")
            {
                sb.Append(" and t1.GROUP_NAME like '%'+ @GROUP_NAME +'%'  ");
                ht.Add("@GROUP_NAME", group_name);
            }
            if (classify != "")
            {
                sb.Append(" and t1.CLASSIFY = @CLASSIFY ");
                ht.Add("@CLASSIFY", classify);
            }
            if (group_id != "")
            {
                sb.Append(" and t1.GROUP_ID = @GROUP_ID  ");
                ht.Add("@GROUP_ID", group_id);
            }
            if (level != "")
            {
                sb.Append(" and t1.LEVEL = @LEVEL  ");
                ht.Add("@LEVEL", level);
            }
            if (sub_group_id != "")
            {
                sb.Append(" and t2.SUB_GROUP_ID = @SUB_GROUP_ID  ");
                ht.Add("@SUB_GROUP_ID", sub_group_id);
            }
            if (sub_group_name != "")
            {
                sb.Append(" and ( s.SALARY_NAME like '%'+ @SUB_GROUP_NAME +'%' or g.GROUP_NAME like '%'+ @SUB_GROUP_NAME +'%' )  ");
                ht.Add("@SUB_GROUP_NAME", sub_group_name);
            }
            sb.Append("  )alltb ");

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
    public string deleteData(string kind_cd, string group_type, string group_id)
    {
        //刪除主檔
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        //寫log
        sb.Append(" update TB_S_M_SALARY_GROUP_H set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2SC120' ");
        sb.Append(" where KIND_CD = @KIND_CD and GROUP_TYPE = @GROUP_TYPE and GROUP_ID = @GROUP_ID; ");

        sb.Append(" delete from TB_S_M_SALARY_GROUP_H ");
        sb.Append(" where KIND_CD = @KIND_CD and GROUP_TYPE = @GROUP_TYPE and GROUP_ID = @GROUP_ID; ");
        ht.Add("@KIND_CD", kind_cd);
        ht.Add("@GROUP_TYPE", group_type);
        ht.Add("@GROUP_ID", group_id);
        ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);
        dbConn.ExecuteT(sb, ht, true);
        
        //刪除明細
        StringBuilder sb2 = new StringBuilder();
        Hashtable ht2 = new Hashtable();
        //寫log
        sb.Append(" update TB_S_M_SALARY_GROUP_D set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2SC120' ");
        sb.Append(" where KIND_CD = @KIND_CD and GROUP_TYPE = @GROUP_TYPE and GROUP_ID = @GROUP_ID; ");

        sb2.Append(" delete from TB_S_M_SALARY_GROUP_D ");
        sb2.Append(" where KIND_CD = @KIND_CD and GROUP_TYPE = @GROUP_TYPE and GROUP_ID = @GROUP_ID; ");
        ht2.Add("@KIND_CD", kind_cd);
        ht2.Add("@GROUP_TYPE", group_type);
        ht2.Add("@GROUP_ID", group_id);
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

            sb.Append("Select * from TB_S_M_SALARY_GROUP_H ");
            sb.Append(" where KIND_CD = @KIND_CD and GROUP_TYPE = @GROUP_TYPE and GROUP_ID = @GROUP_ID ");
            ht.Add("@KIND_CD", KIND_CD);
            ht.Add("@GROUP_TYPE", GROUP_TYPE);
            ht.Add("@GROUP_ID", GROUP_ID);

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

            sb.Append(" insert into TB_S_M_SALARY_GROUP_H (KIND_CD,GROUP_TYPE,GROUP_ID,GROUP_NAME,LEVEL,CLASSIFY,ORDER_SEQ ");
            sb.Append("                                    ,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID) ");
            sb.Append(" values (@KIND_CD,@GROUP_TYPE,@GROUP_ID,@GROUP_NAME,@LEVEL,@CLASSIFY,@ORDER_SEQ,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");
            ht.Add("@KIND_CD", KIND_CD);
            ht.Add("@GROUP_TYPE", GROUP_TYPE);
            ht.Add("@GROUP_ID", GROUP_ID);
            ht.Add("@GROUP_NAME", GROUP_NAME);
            ht.Add("@LEVEL", LEVEL);
            ht.Add("@CLASSIFY", CLASSIFY);
            ht.Add("@ORDER_SEQ", ORDER_SEQ);
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SC120");

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

            sb.Append("Update TB_S_M_SALARY_GROUP_H ");
            sb.Append(" Set GROUP_NAME = @GROUP_NAME,LEVEL = @LEVEL,CLASSIFY = @CLASSIFY,ORDER_SEQ = @ORDER_SEQ ");
            sb.Append("     ,UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID ");
            sb.Append(" where KIND_CD = @KIND_CD and GROUP_TYPE = @GROUP_TYPE and GROUP_ID = @GROUP_ID ");
            ht.Add("@KIND_CD", KIND_CD);
            ht.Add("@GROUP_TYPE", GROUP_TYPE);
            ht.Add("@GROUP_ID", GROUP_ID);
            ht.Add("@GROUP_NAME", GROUP_NAME);
            ht.Add("@LEVEL", LEVEL);
            ht.Add("@CLASSIFY", CLASSIFY);
            ht.Add("@ORDER_SEQ", ORDER_SEQ);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SC120");

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable getLastTwoNum(string key_kind_cd, string key_group_type, string convert_level)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select ISNULL(MAX(substring(GROUP_ID,4,2)),'00') as GROUP_ID_LAST2 ");
            sb.Append("  from TB_S_M_SALARY_GROUP_H ");
            sb.Append(" where substring(GROUP_ID,1, 3)= @GROUP_ID_Before3 ");
            ht.Add("@GROUP_ID_Before3", key_kind_cd + key_group_type + convert_level);
            return dbConn.Query(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }
    #endregion

    #region "Detail 1"
    //查詢明細表頭部分
    public DataTable getDtlHeader(string qdatakey)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();

        sb.Append(" SELECT t1.KIND_CD, t1.GROUP_TYPE as GROUP_TYPE, t1.GROUP_ID as GROUP_ID, t1.GROUP_NAME, t1.CLASSIFY ");
        sb.Append(" ,t1.KIND_CD +'-'+ d.SUB_DESC as KIND_CD_name ");
        sb.Append(" ,t1.GROUP_TYPE +'-'+ e.SUB_DESC as GROUP_TYPE_name ");
        sb.Append(" from TB_S_M_SALARY_GROUP_H t1");
        sb.Append(" left join TB_9_M_COMM_D d on  d.SYS_CD ='SC' and  d.MAIN_CD='KIND_CD' and  t1.KIND_CD = d.SUB_CD ");
        sb.Append(" left join TB_9_M_COMM_D e on  e.SYS_CD ='SC' and  e.MAIN_CD='SALARY_TYPE' and  t1.GROUP_TYPE = e.SUB_CD ");
        sb.Append(" where t1.KIND_CD+t1.GROUP_TYPE+t1.GROUP_ID = @QDATAKEY");
        ht.Add("@QDATAKEY", qdatakey);
        DataTable dt = dbConn.Query(sb, ht);

        return dt;
    }
    public string deleteDtlData(string kind_cd, string group_type, string group_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //寫log
            sb.Append(" update TB_S_M_SALARY_GROUP_D set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2SC120' ");
            sb.Append(" where KIND_CD =@KIND_CD and GROUP_TYPE = @GROUP_TYPE and GROUP_ID = @GROUP_ID; ");

            sb.Append(" delete from TB_S_M_SALARY_GROUP_D ");
            sb.Append(" where KIND_CD = @KIND_CD and GROUP_TYPE = @GROUP_TYPE and GROUP_ID = @GROUP_ID; ");
            ht.Add("@KIND_CD", kind_cd);
            ht.Add("@GROUP_TYPE", group_type);
            ht.Add("@GROUP_ID", group_id);
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);
            dbConn.ExecuteT(sb, ht, true);
            return "0";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    internal void addDtlData(string kind_cd, string group_type, string group_id, string selectedSub_group_ID)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" insert into TB_S_M_SALARY_GROUP_D (KIND_CD, GROUP_TYPE, GROUP_ID, SUB_GROUP_ID, DATA_SCOPE, CD_TYPE, ACCOUNTING_NO1 ");
            sb.Append("        ,ACCOUNTING_NO2, PAY_CD, VOUCHER_FORMAT, INV_TYPE, VOU_PAY_TARGET, VOU_PAY_TYPE, VOU_PAY_DT_SRC, BUDGET_DEPT, MEMO ");
            sb.Append("        ,CREATED_BY, CREATED_DT, UPDATED_BY, UPDATED_DT, FUNC_ID) ");
            sb.Append(" values (@KIND_CD, @GROUP_TYPE, @GROUP_ID, @SUB_GROUP_ID, @DATA_SCOPE, @CD_TYPE, @ACCOUNTING_NO1 ");
            sb.Append("         ,@ACCOUNTING_NO2, @PAY_CD, @VOUCHER_FORMAT, @INV_TYPE, @VOU_PAY_TARGET, @VOU_PAY_TYPE, @VOU_PAY_DT_SRC, @BUDGET_DEPT, @MEMO ");
            sb.Append("         ,@CREATED_BY, GETDATE(), @UPDATED_BY, GETDATE(), @FUNC_ID) ");
            ht.Add("@KIND_CD", kind_cd);
            ht.Add("@GROUP_TYPE", group_type);
            ht.Add("@GROUP_ID", group_id);
            ht.Add("@SUB_GROUP_ID", selectedSub_group_ID);
            ht.Add("@DATA_SCOPE", "");
            ht.Add("@CD_TYPE", "");
            ht.Add("@ACCOUNTING_NO1", "");
            ht.Add("@ACCOUNTING_NO2", "");
            ht.Add("@PAY_CD", "");
            ht.Add("@VOUCHER_FORMAT", "0");
            ht.Add("@INV_TYPE", "");
            ht.Add("@VOU_PAY_TARGET", "");
            ht.Add("@VOU_PAY_TYPE", "");
            ht.Add("@VOU_PAY_DT_SRC", "");
            ht.Add("@BUDGET_DEPT", "");
            ht.Add("@MEMO", "");
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SC120");
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    #region "level = 0"
    //取得選擇項目 明細檔 level = 0
    public DataTable getSelectedData_Is0(string qdatakey)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();

        sb.Append(" select d1.SUB_GROUP_ID as SUB_GROUP_ID,d1.SUB_GROUP_ID +'-'+ isnull(s.SALARY_NAME,'') as GROUP_NAME ");
        sb.Append("   from TB_S_M_SALARY_GROUP_D d1 ");
        sb.Append("   left join TB_S_M_SALARY_ITEM s on d1.SUB_GROUP_ID = s.SALARY_ID ");
        sb.Append("  where d1.KIND_CD+d1.GROUP_TYPE+d1.GROUP_ID = @QDATAKEY ");
        sb.Append("  order by s.ORDER_SEQ,d1.GROUP_ID ");
        ht.Add("@QDATAKEY", qdatakey);
        DataTable dt = dbConn.Query(sb, ht);

        return dt;
    }
    public DataTable getNonSelectedData_Is0(string group_id)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.Append(" select s.SALARY_ID as GROUP_ID,s.SALARY_ID +'-'+ s.SALARY_NAME as GROUP_NAME ");
        sb.Append("   from TB_S_M_SALARY_ITEM s ");
        sb.Append("  where 1=1 ");
        if (group_id != "" && group_id != null)
        {
            sb.Append(" and  s.SALARY_ID not in ( " + group_id + "  ) ");
        }
        sb.Append(" order by s.ORDER_SEQ,s.SALARY_ID ");
        DataTable dt = dbConn.Query(sb, ht);
        return dt;
    }
    #endregion

    #region "level != 0"
    //取得選擇項目 明細檔 level != 0
    public DataTable getSelectedData_IsNot0(string qdatakey)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();

        sb.Append(" select d1.SUB_GROUP_ID,d1.SUB_GROUP_ID +'-'+ h1.GROUP_NAME as GROUP_NAME  ");
        sb.Append("   from TB_S_M_SALARY_GROUP_D d1 ");
        sb.Append("   left join TB_S_M_SALARY_GROUP_H h1 on d1.KIND_CD = h1.KIND_CD and  d1.GROUP_TYPE = h1.GROUP_TYPE and d1.SUB_GROUP_ID =h1.GROUP_ID ");
        sb.Append("  where d1.KIND_CD+d1.GROUP_TYPE+d1.GROUP_ID = @QDATAKEY ");
        sb.Append("   order by h1.ORDER_SEQ,d1.GROUP_ID  ");
        ht.Add("@QDATAKEY", qdatakey);
        DataTable dt = dbConn.Query(sb, ht);

        return dt;
    }
    public DataTable getNonSelectedData_IsNot0(string qdatakey, string kind_cd, string group_type)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.Append("  select h.GROUP_ID,h.GROUP_ID +'-'+ h.GROUP_NAME as GROUP_NAME  ");
        sb.Append("    from TB_S_M_SALARY_GROUP_H h  ");
        sb.Append("  where 1=1  ");
        sb.Append(" and h.GROUP_ID not in  ");
        sb.Append("       ( select d1.SUB_GROUP_ID ");
        sb.Append("           from TB_S_M_SALARY_GROUP_D d1 ");
        sb.Append("          where d1.KIND_CD+d1.GROUP_TYPE+d1.GROUP_ID = @QDATAKEY ");
        sb.Append("       ) ");
        sb.Append(" and  h.KIND_CD = @KIND_CD and h.GROUP_TYPE = @GROUP_TYPE ");
        sb.Append("  order by h.ORDER_SEQ,h.GROUP_ID ");
        ht.Add("@QDATAKEY", qdatakey);
        ht.Add("@KIND_CD", kind_cd);
        ht.Add("@GROUP_TYPE", group_type);
        DataTable dt = dbConn.Query(sb, ht);
        return dt;
    }
    #endregion



    #endregion

    #region "Detail 2"
    //查詢明細
    public DataTable getDtlData2(int startRowIndex, int maximumRows, string sortExpression, string hid_qdatakey)
    {
        try
        {
           
            if (sortExpression == "")
            {
                sortExpression = "h1.ORDER_SEQ,d1.SUB_GROUP_ID";
            }
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * From");
            sb.Append(" (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append("         d1.SUB_GROUP_ID,d1.SUB_GROUP_ID +'-'+ h1.GROUP_NAME as GROUP_NAME,d1.DATA_SCOPE ");
            sb.Append("        ,d1.CD_TYPE,d1.ACCOUNTING_NO1,d1.ACCOUNTING_NO2,d1.PAY_CD ");
            sb.Append("        ,d1.ACCOUNTING_NO3,d1.ACCOUNTING_NO4,d1.ACCOUNTING_NO5,d1.IS_SHARE ");
            sb.Append("        ,d1.VOUCHER_FORMAT,d1.INV_TYPE,d1.VOU_PAY_TARGET,d1.VOU_PAY_TYPE,d1.VOU_PAY_DT_SRC,d1.BUDGET_DEPT,d1.MEMO ");
            sb.Append("        ,d1.KIND_CD + d1.GROUP_TYPE + d1.GROUP_ID + d1.SUB_GROUP_ID + d1.ACCOUNTING_NO1 as Dtldatakey");
            sb.Append("        ,d1.DATA_SCOPE +'-'+ d.SUB_DESC as DATA_SCOPE_DESC");
            sb.Append("        ,d1.PAY_CD +'-'+ e.SUB_DESC as PAY_CD_DESC");
            sb.Append("        ,d1.INV_TYPE +'-'+ f.SUB_DESC as INV_TYPE_DESC");
            sb.Append("        ,d1.VOU_PAY_TARGET +'-'+ g.SUB_DESC as VOU_PAY_TARGET_DESC");
            sb.Append("        ,d1.VOU_PAY_TYPE +'-'+ i.SUB_DESC as VOU_PAY_TYPE_DESC");
            sb.Append("        ,d1.VOU_PAY_DT_SRC +'-'+ j.SUB_DESC as VOU_PAY_DT_SRC_DESC");
            sb.Append("        ,d1.VOU_VENDOR_CD + '-' + k.SUB_DESC as VOU_VENDOR_CD");//
            sb.Append("        ,d1.VOU_VENDOR_CD as VV_CD");//
            sb.Append("   from TB_S_M_SALARY_GROUP_D d1 ");
            sb.Append("          left join TB_9_M_COMM_D d on  d.SYS_CD ='SC' and  d.MAIN_CD='DATA_SCOPE' and  d1.DATA_SCOPE = d.SUB_CD ");
            sb.Append("          left join TB_9_M_COMM_D e on  e.SYS_CD ='SC' and  e.MAIN_CD='VOU_PAY_CD' and  d1.PAY_CD = e.SUB_CD ");
            sb.Append("          left join TB_9_M_COMM_D f on  f.SYS_CD ='SC' and  f.MAIN_CD='INV_TYPE' and  d1.INV_TYPE = f.SUB_CD ");
            sb.Append("          left join TB_9_M_COMM_D g on  g.SYS_CD ='SC' and  g.MAIN_CD='VOU_PAY_TARGET' and  d1.VOU_PAY_TARGET = g.SUB_CD ");
            sb.Append("          left join TB_9_M_COMM_D i on  i.SYS_CD ='SC' and  i.MAIN_CD='VOU_PAY_TYPE' and  d1.VOU_PAY_TYPE = i.SUB_CD ");
            sb.Append("          left join TB_9_M_COMM_D j on  j.SYS_CD ='SC' and  j.MAIN_CD='VOU_PAY_DT_SRC' and  d1.VOU_PAY_DT_SRC = j.SUB_CD ");
            sb.Append("          left join TB_9_M_COMM_D k on  k.SYS_CD ='SC' and  k.MAIN_CD='VOU_VENDOR_CD' and d1.VOU_VENDOR_CD = k.SUB_CD    ");//財務 + 廠商別
            sb.Append("   left join TB_S_M_SALARY_GROUP_H h1 on  h1.KIND_CD='B' and  d1.GROUP_TYPE = h1.GROUP_TYPE and d1.SUB_GROUP_ID = h1.GROUP_ID ");
            sb.Append("  where 1=1 and d1.KIND_CD + d1.GROUP_TYPE + d1.GROUP_ID = @QDATAKEY ");

            sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar)");
            ht.Add("@QDATAKEY", hid_qdatakey);
            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            return dbConn.Query(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }
    public int getDtlCount2(int startRowIndex, int maximumRows, string hid_qdatakey)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select COUNT(*) total_record ");
            sb.Append("   from TB_S_M_SALARY_GROUP_D d1 ");
            sb.Append("          left join TB_9_M_COMM_D d on  d.SYS_CD ='SC' and  d.MAIN_CD='DATA_SCOPE' and  d1.DATA_SCOPE = d.SUB_CD ");
            sb.Append("          left join TB_9_M_COMM_D e on  e.SYS_CD ='SC' and  e.MAIN_CD='VOU_PAY_CD' and  d1.PAY_CD = e.SUB_CD ");
            sb.Append("          left join TB_9_M_COMM_D f on  f.SYS_CD ='SC' and  f.MAIN_CD='INV_TYPE' and  d1.INV_TYPE = f.SUB_CD ");
            sb.Append("          left join TB_9_M_COMM_D g on  g.SYS_CD ='SC' and  g.MAIN_CD='VOU_PAY_TARGET' and  d1.VOU_PAY_TARGET = g.SUB_CD ");
            sb.Append("          left join TB_9_M_COMM_D i on  i.SYS_CD ='SC' and  i.MAIN_CD='VOU_PAY_TYPE' and  d1.VOU_PAY_TYPE = i.SUB_CD ");
            sb.Append("          left join TB_9_M_COMM_D j on  j.SYS_CD ='SC' and  j.MAIN_CD='VOU_PAY_DT_SRC' and  d1.VOU_PAY_DT_SRC = j.SUB_CD ");
            sb.Append("          left join TB_9_M_COMM_D k on  k.SYS_CD ='SC' and  k.MAIN_CD='VOU_VENDOR_CD' and d1.VOU_VENDOR_CD = k.SUB_CD    ");//財務 + 廠商別
            sb.Append("   left join TB_S_M_SALARY_GROUP_H h1 on  h1.KIND_CD='B' and  d1.GROUP_TYPE = h1.GROUP_TYPE and d1.SUB_GROUP_ID = h1.GROUP_ID ");
            sb.Append("  where 1=1 and d1.KIND_CD + d1.GROUP_TYPE + d1.GROUP_ID = @QDATAKEY ");
            ht.Add("@QDATAKEY", hid_qdatakey);
            DataTable dt = dbConn.Query(sb, ht, true);
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
    public DataTable getSalary_Name(string group_id, string group_type)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();

        sb.Append(" select GROUP_ID,GROUP_NAME ");
        sb.Append("   from TB_S_M_SALARY_GROUP_H ");
        sb.Append("  where KIND_CD ='B' and GROUP_TYPE = @GROUP_TYPE ");
        sb.Append(" and GROUP_ID = @GROUP_ID ");
        ht.Add("@GROUP_ID", group_id);
        ht.Add("@GROUP_TYPE", group_type);
        return dbConn.Query(sb, ht);
    }
    public string deleteDtlData2(string kind_cd, string group_type, string group_id, string sub_group_id, string accounting_no1)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //寫log
            sb.Append(" update TB_S_M_SALARY_GROUP_D set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2SC120' ");
            sb.Append(" where KIND_CD =@KIND_CD and GROUP_TYPE =@GROUP_TYPE and GROUP_ID =@GROUP_ID and SUB_GROUP_ID = @SUB_GROUP_ID and ACCOUNTING_NO1 = @ACCOUNTING_NO1; ");

            sb.Append(" delete from TB_S_M_SALARY_GROUP_D ");
            sb.Append(" where KIND_CD =@KIND_CD and GROUP_TYPE =@GROUP_TYPE and GROUP_ID =@GROUP_ID and SUB_GROUP_ID = @SUB_GROUP_ID and ACCOUNTING_NO1 = @ACCOUNTING_NO1; ");
            ht.Add("@KIND_CD", kind_cd);
            ht.Add("@GROUP_TYPE", group_type);
            ht.Add("@GROUP_ID", group_id);
            ht.Add("@SUB_GROUP_ID", sub_group_id);
            ht.Add("@ACCOUNTING_NO1", accounting_no1);
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);
            dbConn.ExecuteT(sb, ht, true);
            return "0";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    internal DataTable getExistDtlData2()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * from TB_S_M_SALARY_GROUP_D ");
            sb.Append(" where KIND_CD = @KIND_CD");
            sb.Append("   and GROUP_TYPE = @GROUP_TYPE");
            sb.Append("   and GROUP_ID = @GROUP_ID");
            sb.Append("   and SUB_GROUP_ID = @SUB_GROUP_ID");
            sb.Append("   and ACCOUNTING_NO1 = @ACCOUNTING_NO1");
            ht.Add("@KIND_CD", KIND_CD);
            ht.Add("@GROUP_TYPE", GROUP_TYPE);
            ht.Add("@GROUP_ID", GROUP_ID);
            ht.Add("@SUB_GROUP_ID", SUB_GROUP_ID);
            ht.Add("@ACCOUNTING_NO1", ACCOUNTING_NO1);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    internal void addDtlData2()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" insert into TB_S_M_SALARY_GROUP_D (KIND_CD, GROUP_TYPE, GROUP_ID, SUB_GROUP_ID, DATA_SCOPE, CD_TYPE, ACCOUNTING_NO1 ");
            sb.Append("        ,ACCOUNTING_NO2, PAY_CD, VOUCHER_FORMAT, INV_TYPE, VOU_PAY_TARGET, VOU_PAY_TYPE, VOU_PAY_DT_SRC, BUDGET_DEPT, MEMO ");
            sb.Append("        ,ACCOUNTING_NO3, ACCOUNTING_NO4, ACCOUNTING_NO5, IS_SHARE");
            sb.Append("        ,CREATED_BY, CREATED_DT, UPDATED_BY, UPDATED_DT, FUNC_ID,VOU_VENDOR_CD) ");
            sb.Append(" values (@KIND_CD, @GROUP_TYPE, @GROUP_ID, @SUB_GROUP_ID, @DATA_SCOPE, @CD_TYPE, @ACCOUNTING_NO1 ");
            sb.Append("         ,@ACCOUNTING_NO2, @PAY_CD, @VOUCHER_FORMAT, @INV_TYPE, @VOU_PAY_TARGET, @VOU_PAY_TYPE, @VOU_PAY_DT_SRC, @BUDGET_DEPT, @MEMO ");
            sb.Append("         ,@ACCOUNTING_NO3, @ACCOUNTING_NO4, @ACCOUNTING_NO5, @IS_SHARE");
            sb.Append("         ,@CREATED_BY, GETDATE(), @UPDATED_BY, GETDATE(), @FUNC_ID,@VOU_VENDOR_CD) ");
            ht.Add("@KIND_CD", KIND_CD);
            ht.Add("@GROUP_TYPE", GROUP_TYPE);
            ht.Add("@GROUP_ID", GROUP_ID);
            ht.Add("@SUB_GROUP_ID", SUB_GROUP_ID);
            ht.Add("@DATA_SCOPE", DATA_SCOPE);
            ht.Add("@CD_TYPE", CD_TYPE);
            ht.Add("@ACCOUNTING_NO1", ACCOUNTING_NO1);
            ht.Add("@ACCOUNTING_NO2", ACCOUNTING_NO2);
            ht.Add("@ACCOUNTING_NO3", ACCOUNTING_NO3);
            ht.Add("@ACCOUNTING_NO4", ACCOUNTING_NO4);
            ht.Add("@ACCOUNTING_NO5", ACCOUNTING_NO5);
            ht.Add("@IS_SHARE", IS_SHARE);
            ht.Add("@PAY_CD", PAY_CD);
            ht.Add("@VOUCHER_FORMAT", VOUCHER_FORMAT);
            ht.Add("@INV_TYPE", INV_TYPE);
            ht.Add("@VOU_PAY_TARGET", VOU_PAY_TARGET);
            ht.Add("@VOU_PAY_TYPE", VOU_PAY_TYPE);
            ht.Add("@VOU_PAY_DT_SRC", VOU_PAY_DT_SRC);
            ht.Add("@BUDGET_DEPT", BUDGET_DEPT);
            ht.Add("@MEMO", MEMO);
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SC120");
            ht.Add("@VOU_VENDOR_CD", VOU_VENDOR_CD);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    internal void updateDtlData2()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_S_M_SALARY_GROUP_D ");
            sb.Append(" set DATA_SCOPE = @DATA_SCOPE, CD_TYPE = @CD_TYPE, ACCOUNTING_NO2 = @ACCOUNTING_NO2 ");
            sb.Append(" ,PAY_CD = @PAY_CD, VOUCHER_FORMAT = @VOUCHER_FORMAT, INV_TYPE = @INV_TYPE, VOU_PAY_TARGET = @VOU_PAY_TARGET ");
            sb.Append(" , VOU_PAY_TYPE = @VOU_PAY_TYPE, VOU_PAY_DT_SRC = @VOU_PAY_DT_SRC, BUDGET_DEPT = @BUDGET_DEPT, MEMO = @MEMO ");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID ,VOU_VENDOR_CD = @VOU_VENDOR_CD");
            sb.Append(" ,ACCOUNTING_NO1 = @ACCOUNTING_NO1,ACCOUNTING_NO3 = @ACCOUNTING_NO3,ACCOUNTING_NO4 = @ACCOUNTING_NO4");
            sb.Append(" ,ACCOUNTING_NO5 = @ACCOUNTING_NO5,IS_SHARE = @IS_SHARE");
            sb.Append(" where KIND_CD = @KIND_CD");
            sb.Append("   and GROUP_TYPE = @GROUP_TYPE");
            sb.Append("   and GROUP_ID = @GROUP_ID");
            sb.Append("   and SUB_GROUP_ID = @SUB_GROUP_ID");
            sb.Append("   and ACCOUNTING_NO1 = @ORI_ACCOUNTING_NO1");
            ht.Add("@KIND_CD", KIND_CD);
            ht.Add("@GROUP_TYPE", GROUP_TYPE);
            ht.Add("@GROUP_ID", GROUP_ID);
            ht.Add("@SUB_GROUP_ID", SUB_GROUP_ID);
            ht.Add("@ACCOUNTING_NO1", ACCOUNTING_NO1);
            ht.Add("@DATA_SCOPE", DATA_SCOPE);
            ht.Add("@CD_TYPE", CD_TYPE);
            ht.Add("@ACCOUNTING_NO2", ACCOUNTING_NO2);
            ht.Add("@PAY_CD", PAY_CD);
            ht.Add("@VOUCHER_FORMAT", VOUCHER_FORMAT);
            ht.Add("@INV_TYPE", INV_TYPE);
            ht.Add("@VOU_PAY_TARGET", VOU_PAY_TARGET);
            ht.Add("@VOU_PAY_TYPE", VOU_PAY_TYPE);
            ht.Add("@VOU_PAY_DT_SRC", VOU_PAY_DT_SRC);
            ht.Add("@BUDGET_DEPT", BUDGET_DEPT);
            ht.Add("@MEMO", MEMO);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SC120");
            ht.Add("@VOU_VENDOR_CD", VOU_VENDOR_CD);
            ht.Add("@ACCOUNTING_NO3", ACCOUNTING_NO3);
            ht.Add("@ACCOUNTING_NO4", ACCOUNTING_NO4);
            ht.Add("@ACCOUNTING_NO5", ACCOUNTING_NO5);
            ht.Add("@IS_SHARE", IS_SHARE);
            ht.Add("@ORI_ACCOUNTING_NO1", ORI_ACCOUNTING_NO1);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion
}