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
public class CFB2HA0300DAO : BaseDAO
{
    public CFB2HA0300DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public string RowNumber { get; set; }
    public string QDATAKEY { get; set; }
    public string ACC_DEPT_NO { get; set; }
    public string ACC_DEPT_NAME { get; set; }
    public string ddl_CAR_TYPE { get; set; }
    public string COST_DEPT_NO { get; set; }
    public string BUDGET_DEPT_NO { get; set; }
    public string IS_VALID { get; set; }
    public string REMARK { get; set; }
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
            sb.Append(" where SYS_CD='HA' and MAIN_CD='CAR_TYPE' and IS_VALID='Y'  ");
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
    //internal System.Data.DataTable getSYS_ID()
    //{
    //    try
    //    {
    //        StringBuilder sb = new StringBuilder();
    //        Hashtable ht = new Hashtable();
    //        sb.Append(" select SUB_CD ,SUB_DESC from TB_9_M_COMM_D where 1=1 and sys_cd = '99' and main_cd = 'sys_id' and is_valid ='Y' order by SUB_CD");
    //        return dbConn.Query(sb);

    //    }
    //    catch (Exception)
    //    {

    //        throw;
    //    }
    //}

    public DataTable get_SYS_ID_Data()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select SYS_CD , MAIN_CD , SUB_CD ,SUB_DESC from TB_9_M_COMM_D");
            sb.Append(" where SYS_CD='HA' and MAIN_CD='CAR_TYPE'  ");

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string sys_id, string ddl_CAR_TYPE, string txt_ACC_DEPT_NAME, string txt_COST_DEPT_NO, string txt_BUDGET_DEPT_NO, string ddl_IS_VALID)
    {
        try
        {
            if (sortExpression == "")
            {
                sortExpression = "CAR_TYPE";
            }
            if (sortExpression.Contains("IS_VALID") || sortExpression.Contains("REMARK"))
            {
                sortExpression = string.Format("A.{0}", sortExpression);
            }
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * From");
            sb.Append(" (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" A.ACC_DEPT_NO+A.ACC_DEPT_NAME as qdatakey,A.ACC_DEPT_NO , A.ACC_DEPT_NAME , A.CAR_TYPE ,A.IS_VALID,A.COST_DEPT_NO,A.BUDGET_DEPT_NO,A.REMARK,B.SUB_DESC");
            sb.Append(" from TB_H_M_DEPT_ACC A");
            sb.Append(" left join TB_9_M_COMM_D B on B.MAIN_CD='CAR_TYPE' and b.SYS_CD='HA'");
            sb.Append("  where 1=1 and A.CAR_TYPE=B.SUB_CD and b.SYS_CD='HA'");

            if (sys_id != "-1" && sys_id != "")
            {
                sb.Append(" and ACC_DEPT_NO Like '" + sys_id + "%'");
                //ht.Add("@ACC_DEPT_NO", sys_id);
            }
            if (txt_ACC_DEPT_NAME != "")
            {
                sb.Append(" and ACC_DEPT_NAME  Like '" + txt_ACC_DEPT_NAME + "%'");
                //ht.Add("@ACC_DEPT_NAME", txt_ACC_DEPT_NAME);
            }
            if (ddl_CAR_TYPE != " " && ddl_CAR_TYPE != "-1")
            {
               
                sb.Append(" and CAR_TYPE = @CAR_TYPE ");
                ht.Add("@CAR_TYPE", ddl_CAR_TYPE);
            }
            if (txt_COST_DEPT_NO != "")
            {
                sb.Append(" and COST_DEPT_NO Like '" + txt_COST_DEPT_NO + "%'");
                //ht.Add("@COST_DEPT_NO", txt_COST_DEPT_NO);
            }
            if (txt_BUDGET_DEPT_NO != "")
            {
                sb.Append(" and BUDGET_DEPT_NO Like '" + txt_BUDGET_DEPT_NO + "%'");
                //ht.Add("@BUDGET_DEPT_NO", txt_BUDGET_DEPT_NO);
            }
            if (ddl_IS_VALID != "")
            {
                sb.Append(" and A.IS_VALID = @IS_VALID  ");
                ht.Add("@IS_VALID", ddl_IS_VALID);
            }
            //else
            //{
            //sb.Append(" and A.IS_VALID != @IS_VALID  ");
            //    ht.Add("@IS_VALID", "N");
            //}
            
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
    public int getCount(int startRowIndex, int maximumRows, string sys_id, string ddl_CAR_TYPE, string txt_ACC_DEPT_NAME, string txt_COST_DEPT_NO, string txt_BUDGET_DEPT_NO, string ddl_IS_VALID)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) total_record ");
            sb.Append(" from TB_H_M_DEPT_ACC A");
            sb.Append(" left join TB_9_M_COMM_D B on B.MAIN_CD='CAR_TYPE' and b.SYS_CD='HA'");
            sb.Append(" where 1=1 and A.CAR_TYPE=B.SUB_CD and b.SYS_CD='HA'");
            if (sys_id != "-1" && sys_id != "")
            {
                sb.Append(" and ACC_DEPT_NO Like '" + sys_id + "%'");
                //ht.Add("@ACC_DEPT_NO", sys_id);
            }
            if (txt_ACC_DEPT_NAME != "")
            {
                sb.Append(" and ACC_DEPT_NAME  Like '" + txt_ACC_DEPT_NAME + "%'");
                //ht.Add("@ACC_DEPT_NAME", txt_ACC_DEPT_NAME);
            }
            if (ddl_CAR_TYPE != " " && ddl_CAR_TYPE != "-1")
            {
               
                sb.Append(" and CAR_TYPE = @CAR_TYPE ");
                ht.Add("@CAR_TYPE", ddl_CAR_TYPE);
            }
            if (txt_COST_DEPT_NO != "")
            {
                sb.Append(" and COST_DEPT_NO Like '" + txt_COST_DEPT_NO + "%'");
                //ht.Add("@COST_DEPT_NO", txt_COST_DEPT_NO);
            }
            if (txt_BUDGET_DEPT_NO != "")
            {
                sb.Append(" and BUDGET_DEPT_NO Like '" + txt_BUDGET_DEPT_NO + "%'");
                //ht.Add("@BUDGET_DEPT_NO", txt_BUDGET_DEPT_NO);
            }
            if (ddl_IS_VALID != "")
            {
                sb.Append(" and A.IS_VALID = @IS_VALID  ");
                ht.Add("@IS_VALID", ddl_IS_VALID);
            }
            //else
            //{
            //    sb.Append(" and A.IS_VALID != @IS_VALID  ");
            //    ht.Add("@IS_VALID", "N");
            //}
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
    public string deleteData(string deleteitem)
    {
        //刪除 薪資部門區分設定檔
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        //寫log
        sb.Append(" update TB_H_M_DEPT_ACC set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2HA030' ");
        sb.Append(" where ACC_DEPT_NO+ACC_DEPT_NAME = @qdatakey; ");
        ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);

        sb.Append(" Delete from TB_H_M_DEPT_ACC ");
        sb.Append(" where ACC_DEPT_NO+ACC_DEPT_NAME = @qdatakey; ");
        ht.Add("@qdatakey", deleteitem);
        dbConn.ExecuteT(sb, ht, true);

        return "0";
    }
    internal DataTable getExistData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select * from TB_H_M_DEPT_ACC where ACC_DEPT_NO = @ACC_DEPT_NO");
            ht.Add("@ACC_DEPT_NO", ACC_DEPT_NO);
           

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
            sb.Append("INSERT INTO TB_H_M_DEPT_ACC (ACC_DEPT_NO,ACC_DEPT_NAME,CAR_TYPE,COST_DEPT_NO,BUDGET_DEPT_NO,IS_VALID,FUNC_ID,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,REMARK)");
            sb.Append(" Values (@ACC_DEPT_NO,@ACC_DEPT_NAME,@CAR_TYPE,@COST_DEPT_NO,@BUDGET_DEPT_NO,@IS_VALID,@FUNC_ID,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@REMARK)");
            ht.Add("@ACC_DEPT_NO", ACC_DEPT_NO);
            ht.Add("@ACC_DEPT_NAME", ACC_DEPT_NAME);
            string str = ddl_CAR_TYPE.Substring(0, 1);
            ht.Add("@CAR_TYPE", str);
            ht.Add("@COST_DEPT_NO", COST_DEPT_NO);
            ht.Add("@BUDGET_DEPT_NO", BUDGET_DEPT_NO);
            ht.Add("@IS_VALID", IS_VALID);
            ht.Add("@FUNC_ID", "FB2HA0300");
            ht.Add("@CREATED_BY", CREATED_BY);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@REMARK", REMARK);
            
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
            sb.Append("Update TB_H_M_DEPT_ACC ");
            sb.Append(" Set ACC_DEPT_NO = @ACC_DEPT_NO,ACC_DEPT_NAME=@ACC_DEPT_NAME,CAR_TYPE=@CAR_TYPE,COST_DEPT_NO=@COST_DEPT_NO,BUDGET_DEPT_NO=@BUDGET_DEPT_NO,IS_VALID=@IS_VALID,REMARK=@REMARK,UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE()");
            sb.Append(" where ACC_DEPT_NO = @ACC_DEPT_NO");
            ht.Add("@ACC_DEPT_NO", ACC_DEPT_NO);
            ht.Add("@ACC_DEPT_NAME", ACC_DEPT_NAME);
            string str = ddl_CAR_TYPE.Substring(0, 1);
            ht.Add("@CAR_TYPE", str);
            ht.Add("@COST_DEPT_NO", COST_DEPT_NO);
            ht.Add("@BUDGET_DEPT_NO", BUDGET_DEPT_NO);
            ht.Add("@IS_VALID", IS_VALID);
            
            
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@REMARK", REMARK);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
}