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
/// CFB2HA010DAO 的摘要描述
/// </summary>
public class CFB2HA0100DAO : BaseDAO
{
    //基本欄位
    public string DEPT_LEVEL { get; set; }
    public string DEPT_LEVEL_DESC { get; set; }
    public string LEVEL_TYPE { get; set; }
    public string IS_VALID { get; set; }
    public string REMARK { get; set; }
    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }

    public CFB2HA0100DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    internal DataTable getDeptLevel()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select distinct convert(varchar,DEPT_LEVEL) + '-' + DEPT_LEVEL_DESC DEPT_LEVEL_DESC,DEPT_LEVEL");
            sb.Append(" from TB_H_M_DEPT_LEVEL");
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //Gridview 查詢資料
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string dept_level)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,* ");
            sb.Append(" from ( ");
            sb.Append("     select a.DEPT_LEVEL,a.DEPT_LEVEL_DESC,b.SUB_CD + '-' + b.SUB_DESC LEVEL_TYPE_DESC , a.LEVEL_TYPE,a.IS_VALID,a.REMARK");
            sb.Append("     from TB_H_M_DEPT_LEVEL a,TB_9_M_COMM_D b ");
            sb.Append("     where a.LEVEL_TYPE = b.SUB_CD");
            sb.Append("     and b.MAIN_CD = 'LEVEL_TYPE'");
            if (dept_level != "-1")
            {
                sb.Append(" and DEPT_LEVEL = @DEPT_LEVEL ");
                ht.Add("@DEPT_LEVEL", dept_level);
            }

            sb.Append(" )alltb ");
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
    public int getCount(int startRowIndex, int maximumRows, string dept_level)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select COUNT(*) total_record ");
            sb.Append(" from TB_H_M_DEPT_LEVEL a,TB_9_M_COMM_D b ");
            sb.Append(" where a.LEVEL_TYPE = b.SUB_CD");
            sb.Append(" and b.MAIN_CD = 'LEVEL_TYPE'");
            if (dept_level != "-1")
            {
                sb.Append(" and DEPT_LEVEL = @DEPT_LEVEL ");
                ht.Add("@DEPT_LEVEL", dept_level);
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

    internal void addDept_Level(CFB2HA0100DAO fb2ha010)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" insert into TB_H_M_DEPT_LEVEL (DEPT_LEVEL,DEPT_LEVEL_DESC,LEVEL_TYPE,IS_VALID,REMARK,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" values (@DEPT_LEVEL,@DEPT_LEVEL_DESC,@LEVEL_TYPE,@IS_VALID,@REMARK,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");
            ht.Add("@DEPT_LEVEL", fb2ha010.DEPT_LEVEL);
            ht.Add("@DEPT_LEVEL_DESC", fb2ha010.DEPT_LEVEL_DESC);
            ht.Add("@LEVEL_TYPE", fb2ha010.LEVEL_TYPE);
            ht.Add("@IS_VALID", fb2ha010.IS_VALID);
            ht.Add("@REMARK", fb2ha010.REMARK);
            ht.Add("@CREATED_BY", fb2ha010.CREATED_BY);
            ht.Add("@UPDATED_BY", fb2ha010.UPDATED_BY);
            ht.Add("@FUNC_ID", fb2ha010.FUNC_ID);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void updateDept_Level(CFB2HA0100DAO fb2ha010)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_H_M_DEPT_LEVEL set DEPT_LEVEL_DESC = @DEPT_LEVEL_DESC,LEVEL_TYPE = @LEVEL_TYPE,");
            sb.Append("IS_VALID = @IS_VALID,REMARK = @REMARK,UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE()");
            sb.Append(" where DEPT_LEVEL = @DEPT_LEVEL");
            ht.Add("@DEPT_LEVEL", fb2ha010.DEPT_LEVEL);
            ht.Add("@DEPT_LEVEL_DESC", fb2ha010.DEPT_LEVEL_DESC);
            ht.Add("@LEVEL_TYPE", fb2ha010.LEVEL_TYPE);
            ht.Add("@IS_VALID", fb2ha010.IS_VALID);
            ht.Add("@REMARK", fb2ha010.REMARK);
            ht.Add("@UPDATED_BY", fb2ha010.UPDATED_BY);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getExistDept(string DEPT_LEVEL)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select DEPT_NO From");
            sb.Append(" TB_H_M_DEPT");

            sb.Append(" where DEPT_LEVEL = @DEPT_LEVEL and END_DT >= GETDATE()");
            ht.Add("@DEPT_LEVEL", DEPT_LEVEL);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getExistDeptLevel(string dept_level)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(0) deptcount From");
            sb.Append(" TB_H_M_DEPT");

            sb.Append(" where DEPT_LEVEL = @DEPT_LEVEL");
            ht.Add("@DEPT_LEVEL", dept_level);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void deleteDeptLevel(string item)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //寫log
            sb.Append(" update TB_H_M_DEPT_LEVEL set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2HA010' ");
            sb.Append(" where DEPT_LEVEL = @DEPT_LEVEL;");
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);

            sb.Append(" Delete From TB_H_M_DEPT_LEVEL");
            sb.Append(" where DEPT_LEVEL = @DEPT_LEVEL;");
            ht.Add("@DEPT_LEVEL", item);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getExistLevel(string DEPT_LEVEL)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select DEPT_LEVEL From");
            sb.Append(" TB_H_M_DEPT_LEVEL");

            sb.Append(" where DEPT_LEVEL = @DEPT_LEVEL ");
            ht.Add("@DEPT_LEVEL", DEPT_LEVEL);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
}