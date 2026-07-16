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
/// CFB2990400DAO 的摘要描述
/// </summary>
public class CFB2990400DAO : BaseDAO
{
    public CFB2990400DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public string RowNumber { get; set; }
    public string QDATAKEY { get; set; }
    public string SYS_ID { get; set; }
    public string SYS_NAME { get; set; }
    public string MODE_ID { get; set; }
    public string MODE_NAME { get; set; }
    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }
    public string FUNCTION_ID { get; set; }
    public string FUNCTION_NAME { get; set; }

    
    //for查詢欄位
    public string ddl_SYS_ID { get; set; }



    public DataTable getSYS_ID()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select SUB_CD ,SUB_DESC from TB_9_M_COMM_D where 1=1 and sys_cd = '99' and main_cd = 'SYS_LOG' and is_valid ='Y' order by SUB_CD");
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
            sb.Append(" select SUB_CD ,SUB_DESC from TB_9_M_COMM_D where 1=1 and sys_cd = '99' and main_cd = 'SYS_LOG' and is_valid ='Y' and SUB_CD = @SUB_CD order by SUB_CD");
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
            sb.Append(" select SYS_ID , SYS_NAME , MODE_ID ,MODE_NAME from TB_9_M_SYS_M");
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string sys_id)
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
            sb.Append(" (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" SYS_ID+MODE_ID as qdatakey,SYS_ID , SYS_NAME , MODE_ID ,MODE_NAME");
            sb.Append(" from TB_9_M_SYS_M");
            sb.Append(" where 1=1");

            if (sys_id != "-1" && sys_id != "A")
            {
                sb.Append(" and SYS_ID = @SYS_ID  ");
                ht.Add("@SYS_ID", sys_id);
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
    public int getCount(int startRowIndex, int maximumRows, string sys_id)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) total_record ");
            sb.Append(" from TB_9_M_SYS_M");
            sb.Append(" where 1=1");
            if (sys_id != "-1" && sys_id != "A")
            {
                sb.Append(" and SYS_ID = @SYS_ID ");
                ht.Add("@SYS_ID", sys_id);
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

    public DataTable getFuncData(DataTable dt)
    {
        try
        {
            dbConn.OtherCommStr = utilities.ACESconnstr;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //sb.Append(" select count(*) as dtt ");
            sb.Append(" SELECT  SUBSTRING(SYS_FUNC_NAME,1,8) as FUNC_ID,SUBSTRING(SYS_FUNC_NAME,1,8)+':'+ SUBSTRING(SYS_FUNC_NAME,CHARINDEX(' ',SYS_FUNC_NAME,1)+1,DATALENGTH(SYS_FUNC_NAME) - CHARINDEX(' ',SYS_FUNC_NAME,1)) as FUNC_NAME ");
            sb.Append(" FROM TB_M_SYS_FUNC ");
            sb.Append(" where SYS_ITEM_CD = 'FB2' and SYS_TYPE = 'F' ");
            sb.Append("  and LOG_FLAG='Y'  ");
            string aaa="";
            for(int i=0;i<dt.Rows.Count;i++){
                aaa += ",@aaa" + i.ToString();
                ht.Add("@aaa" + i.ToString(), dt.Rows[i]["FUNCTION_ID"].ToString());
            }
            if (dt.Rows.Count > 0)
            {
                sb.Append(" and SUBSTRING(SYS_FUNC_NAME,1,8) not in ( " + aaa.Trim(',') + " )");
            }
            
            sb.Append(" order by FUNC_ID ");

            

            DataTable dt1 = dbConn.Query(sb, ht);            
            
            dbConn.OtherCommStr = "";
            return dt1;
        }
        catch
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
            sb.Append("    select  ROW_NUMBER() OVER(ORDER BY FUNC_ID) As RowNumber,td.* from (");
            sb.Append("     select  d.*, FUNCTION_ID +':'+ FUNCTION_NAME as FUNC_NAME,m.SYS_ID+m.MODE_ID as S_M_ID");
            sb.Append("     from TB_9_M_SYS_M as m					");
            sb.Append("     inner join TB_9_M_SYS_D as d on m.FUNC_ID = d.FUNC_ID  and d.MODE_ID = m.MODE_ID");
            sb.Append("     where 1=1 )td where S_M_ID=@ID");

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
            sb.Append("     select ROW_NUMBER() OVER(ORDER BY FUNC_ID) As RowNumber,* from (	select");
            sb.Append("     d.*, FUNCTION_ID +':'+ FUNCTION_NAME as FUNC_NAME,m.SYS_ID+m.MODE_ID as S_M_ID ");
            sb.Append("     from TB_9_M_SYS_M as m					");
            sb.Append("     inner join TB_9_M_SYS_D as d on m.FUNC_ID = d.FUNC_ID  and d.MODE_ID = m.MODE_ID");
            sb.Append("     where 1=1 )td where S_M_ID=@ID");
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
            sb.Append(" 	select * from (select d.MODE_ID, d.FUNCTION_ID, d.FUNCTION_NAME,m.SYS_ID+m.MODE_ID as S_M_ID");
            sb.Append("     from TB_9_M_SYS_M as m					");
            sb.Append("     inner join TB_9_M_SYS_D as d on m.FUNC_ID = d.FUNC_ID  and d.MODE_ID = m.MODE_ID");
            sb.Append("     where 1=1 )td where S_M_ID=@ID");
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
    public void deleteData(string sys_id,string mode_id)
    {
        try
        {
            //刪除共用代碼主檔
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //寫log
            sb.AppendLine(" update TB_9_M_SYS_M set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB299040' ");
            sb.AppendLine(" where SYS_ID = @SYS_ID and MODE_ID = @MODE_ID; ");

            sb.Append("Delete from TB_9_M_SYS_M ");
            sb.Append(" where SYS_ID = @SYS_ID and MODE_ID = @MODE_ID;");
            ht.Add("@SYS_ID", sys_id);
            ht.Add("@MODE_ID", mode_id);
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);

            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            
            throw;
        } 
    }

    public void deleteDtlData(string MODE_ID)
    {
        try
        {
            //刪除明細檔
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //寫log
            sb.AppendLine(" update TB_9_M_SYS_D set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB299040' ");
            sb.AppendLine(" where MODE_ID = @MODE_ID; ");

            sb.Append("Delete from TB_9_M_SYS_D ");
            sb.Append(" where  MODE_ID = @MODE_ID;");

            ht.Add("@MODE_ID", MODE_ID);
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {            
            throw;
        }       
        
    }


    public string deleteFUNC(string MODE_ID)
    {
        //刪除共用代碼主檔
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        //寫log
        sb.AppendLine(" update TB_9_M_SYS_D set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB299040' ");
        sb.AppendLine(" where MODE_ID = @MODE_ID; ");

        sb.Append(" Delete from TB_9_M_SYS_D where  ");
        sb.Append(" MODE_ID = @MODE_ID;");
        ht.Add("@MODE_ID", MODE_ID);
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
            sb.Append("Select * from TB_9_M_SYS_M where SYS_ID+MODE_ID = @SYS_ID+@MODE_ID");
            ht.Add("@SYS_ID", SYS_ID);
            ht.Add("@MODE_ID", MODE_ID);

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
            sb.Append("INSERT INTO TB_9_M_SYS_M (SYS_ID,SYS_NAME,MODE_ID,MODE_NAME,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" Values (@SYS_ID,@SYS_NAME,@MODE_ID,@MODE_NAME,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");
            ht.Add("@SYS_ID", SYS_ID);
            ht.Add("@SYS_NAME", SYS_NAME);
            ht.Add("@MODE_ID", MODE_ID);
            ht.Add("@MODE_NAME", MODE_NAME);
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
    internal void updateData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Update TB_9_M_SYS_M ");
            sb.Append(" Set MODE_NAME = @MODE_NAME,UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID");
            sb.Append(" where SYS_ID+MODE_ID = @SYS_ID+@MODE_ID");
            ht.Add("@SYS_ID", SYS_ID);
            ht.Add("@MODE_ID", MODE_ID);
            ht.Add("@MODE_NAME", MODE_NAME);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    internal void addData(string add)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("INSERT INTO TB_9_M_SYS_M (SYS_ID,SYS_NAME,MODE_ID,MODE_NAME,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" Values (@SYS_ID,@SYS_NAME,@MODE_ID,@MODE_NAME,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE()),@FUNC_ID");
            ht.Add("@SYS_ID", SYS_ID);
            ht.Add("@SYS_NAME", SYS_NAME);
            ht.Add("@MODE_ID", MODE_ID);
            ht.Add("@MODE_NAME", MODE_NAME);
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

    internal void add_SYS_D_Data()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("INSERT INTO TB_9_M_SYS_D (MODE_ID, FUNCTION_ID, FUNCTION_NAME, CREATED_BY, CREATED_DT, UPDATED_BY, UPDATED_DT, FUNC_ID)");
            sb.Append(" Values (@MODE_ID, @FUNCTION_ID, @FUNCTION_NAME, @CREATED_BY, GETDATE(), @UPDATED_BY, GETDATE(), @FUNC_ID)");
            ht.Add("@MODE_ID", MODE_ID);
            ht.Add("@FUNCTION_ID", FUNCTION_ID);
            ht.Add("@FUNCTION_NAME", FUNCTION_NAME);
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
    internal DataTable getExist_SYS_D_Data()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select * from TB_9_M_SYS_D where FUNCTION_ID+MODE_ID = @FUNCTION_ID+@MODE_ID");
            ht.Add("@FUNCTION_ID", FUNCTION_ID);
            ht.Add("@MODE_ID", MODE_ID);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

}