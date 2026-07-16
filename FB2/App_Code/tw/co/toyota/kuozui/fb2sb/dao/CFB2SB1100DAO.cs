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
public class CFB2SB1100DAO : BaseDAO
{
    public CFB2SB1100DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public string RowNumber { get; set; }
    public string QDATAKEY { get; set; }
    public string EMP_ID { get; set; }
    public string EMP_NAME { get; set; }
    public string EMP_CHG_CD_DESC { get; set; }
    public string ddl_SUB_CD { get; set; }
    
  
    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }
    //for查詢欄位

    public string FUNCTION_ID { get; set; }
    public string FUNCTION_NAME { get; set; }
    public string MODE_ID { get; set; }
    public string TYPE { get; set; }
    
    public string SALARY_ID { get; set; }
    public string ITEM_SEQ { get; set; }

    public DataTable getSYS_ID()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select SYS_CD , MAIN_CD , SUB_CD ,SUB_DESC from TB_9_M_COMM_D");
            sb.Append(" where SYS_CD='SB' and MAIN_CD='DATA_TYPE'  ");
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
    public DataTable getEMP_ID(string ID)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" SELECT A.*,B.EMP_NAME,C.SUB_CD+'-'+C.SUB_DESC As TYPE");
            sb.Append(" FROM TB_S_M_SUBSIDY_MEM_H A,VW_H_EMP_DATA B,TB_9_M_COMM_D C");
            sb.Append(" WHERE (A.EMP_ID=B.EMP_ID)and(A.EMP_ID = @ID)and(C.SYS_CD='SB')and(A.DATA_TYPE=C.SUB_CD)");
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
            sb.Append(" where SYS_CD='HB' and MAIN_CD=CAR_TYPE  ");

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    //20140922 Terry modify
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string data_type, string emp_id, string emp_name)
    {
        try
        {
            if (sortExpression.Contains("EMP_ID"))
            {
                sortExpression = sortExpression.Replace("EMP_ID", "A.EMP_ID");
            }
            if (sortExpression == "")
            {
                sortExpression = "A.EMP_ID";
            }
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            //sb.Append(" (select ROW_NUMBER() OVER(ORDER BY A.DATA_TYPE ) As RowNumber,");
            sb.Append(" A.EMP_ID,V.EMP_NAME,V.EMP_STATUS + '-' + V.EMP_STATUS_DESC as EMP_STATUS,C.SUB_CD,C.SUB_DESC ");
            sb.Append(" from TB_S_M_SUBSIDY_MEM_H A ");
            sb.Append(" left join VW_H_EMP_DATA V on A.EMP_ID = V.EMP_ID ");
            sb.Append(" left join TB_9_M_COMM_D C on A.DATA_TYPE = C.SUB_CD and C.SYS_CD = 'SB' and C.MAIN_CD='DATA_TYPE' ");
            sb.Append(" where 1=1 ");
            if (data_type != "-1")
            {
                sb.Append(" and A.DATA_TYPE = @data_type  ");
                ht.Add("@data_type", data_type);
            }
            if (emp_id != "")
            {
                sb.Append(" and A.EMP_ID like '%'+@emp_id+'%' ");
                ht.Add("@emp_id", emp_id);
            }
            if (emp_name != "")
            {
                sb.Append(" and V.EMP_NAME like '%'+@emp_name+'%'   ");
                ht.Add("@emp_name", emp_name);
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
    //20140922 Terry modify
    public int getCount(int startRowIndex, int maximumRows, string data_type, string emp_id, string emp_name)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) total_record ");
            sb.Append(" from TB_S_M_SUBSIDY_MEM_H A ");
            sb.Append(" left join VW_H_EMP_DATA V on A.EMP_ID = V.EMP_ID ");
            sb.Append(" left join TB_9_M_COMM_D C on A.DATA_TYPE = C.SUB_CD and C.SYS_CD = 'SB' and C.MAIN_CD='DATA_TYPE' ");
            sb.Append(" where 1=1 ");
            if (data_type != "-1")
            {
                sb.Append(" and A.DATA_TYPE = @data_type  ");
                ht.Add("@data_type", data_type);
            }
            if (emp_id != "")
            {
                sb.Append(" and A.EMP_ID like '%'+@emp_id+'%' ");
                ht.Add("@emp_id", emp_id);
            }
            if (emp_name != "")
            {
                sb.Append(" and V.EMP_NAME like '%'+@emp_name+'%'   ");
                ht.Add("@emp_name", emp_name);
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

    public DataTable getUnSelectedData1(string id, string type)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select s.SALARY_ID,s.SALARY_ID +'-'+ s.SALARY_NAME as SALARY_NAME");
            sb.Append(" from TB_S_M_SALARY_ITEM s");
            sb.Append(" where 1=1 and  s.SALARY_ID not in (");
            sb.Append(" select  d1.SALARY_ID");
            sb.Append(" from TB_S_M_SUBSIDY_MEM_D d1");
            sb.Append(" where d1.EMP_ID = @EMP_ID  and d1.TYPE =  @TYPE ");
            sb.Append(" ) and s.SALARY_CD ='2' ");
            sb.Append(" order by s.SALARY_ID");

            ht.Add("@EMP_ID", id);
            ht.Add("@TYPE", type);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
  
    public DataTable getSelectedData2(string id, string type)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select d1.SALARY_ID,d1.SALARY_ID + '-' + s.SALARY_NAME as SALARY_NAME");
            sb.Append(" from TB_S_M_SUBSIDY_MEM_D d1");
            sb.Append(" left join TB_S_M_SALARY_ITEM s on d1.SALARY_ID = s.SALARY_ID");
            sb.Append(" where 1=1 and  d1.EMP_ID = @EMP_ID  and d1.TYPE = @TYPE");
            sb.Append(" order by d1.ITEM_SEQ");

            ht.Add("@EMP_ID", id);
            ht.Add("@TYPE", type);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    public DataTable getUnselectedData2(string type)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select s.SALARY_ID,s.SALARY_ID + '-' + s.SALARY_NAME as SALARY_NAME");
            sb.Append(" from TB_S_M_SALARY_ITEM s");
            sb.Append(" where 1=1 and  s.SALARY_ID not in (");
            sb.Append(" select  d1.SALARY_ID");
            sb.Append(" from TB_S_M_SUBSIDY_MEM_D d1");
            sb.Append(" where d1.type = @TYPE");
            sb.Append(" )");
            sb.Append("  order by s.SALARY_ID");
           

            
            ht.Add("@TYPE", type);
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
            sb.Append("     select ROW_NUMBER() OVER(ORDER BY m.EMP_ID) As RowNumber,");
            sb.Append("     d.* ,m.SALARY_ID+'-'+d.SALARY_NAME as 'SALARY',EMP_ID,TYPE ");
            sb.Append("     from TB_S_M_SUBSIDY_MEM_D as m						");
            sb.Append("     inner join TB_S_M_SALARY_ITEM as d on m.SALARY_ID = d.SALARY_ID and m.EMP_ID = @ID");
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
            sb.Append(" 	select m.*");
            sb.Append("     from TB_S_M_SUBSIDY_MEM_D as m						");
            sb.Append("     inner join TB_S_M_SALARY_ITEM as d on m.SALARY_ID = d.SALARY_ID and m.EMP_ID = @ID");
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
    public void deleteData(string deleteitem)
    {
        //刪除主檔
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.Append("Delete from TB_S_M_SUBSIDY_MEM_H where  ");
        sb.Append(" EMP_ID = @EMP_ID");

        ht.Add("@EMP_ID", deleteitem);

        dbConn.ExecuteT(sb, ht, true);
       
    }

    public void deleteDetail(string deleteitem)
    {
        //刪除明細檔
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.Append("Delete from TB_S_M_SUBSIDY_MEM_D where  ");
        sb.Append(" EMP_ID = @EMP_ID");

        ht.Add("@EMP_ID", deleteitem);

        dbConn.ExecuteT(sb, ht, true);

    }

    internal DataTable getExistData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select * from TB_S_M_SUBSIDY_MEM_H where EMP_ID = @EMP_ID and DATA_TYPE=@DATA_TYPE");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@DATA_TYPE", ddl_SUB_CD);
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
            sb.Append("INSERT INTO TB_S_M_SUBSIDY_MEM_H (DATA_TYPE,EMP_ID,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" Values (@DATA_TYPE,@EMP_ID,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");
            ht.Add("@DATA_TYPE", ddl_SUB_CD);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@CREATED_BY", CREATED_BY);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", "FB2SB1100");
            
           
            
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //internal void updateData()
    //{
    //    try
    //    {
    //        StringBuilder sb = new StringBuilder();
    //        Hashtable ht = new Hashtable();
    //        sb.Append("Update TB_S_M_SUBSIDY_MEM_H ");
    //        sb.Append(" Set DATA_TYPE=@DATA_TYPE,EMP_ID=@EMP_ID,UPDATED_BY=@UPDATED_BY");
    //        sb.Append(" where DATA_TYPE = @DATA_TYPE");

    //        string str = ddl_SUB_CD.Substring(0, 1);
    //        ht.Add("@DATA_TYPE", str);
    //        ht.Add("@EMP_ID", EMP_ID);

    //        ht.Add("@UPDATED_BY", UPDATED_BY);


    //        dbConn.ExecuteT(sb, ht, true);
    //    }
    //    catch (Exception)
    //    {
    //        throw;
    //    }
    //}
    internal void add_SYS_D_Data()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            

            if (ITEM_SEQ == "1")
            {
                sb.Append("Delete from TB_S_M_SUBSIDY_MEM_D where  ");
                sb.Append(" EMP_ID = @EMP_ID");

                ht.Add("@EMP_ID", EMP_ID);

                dbConn.ExecuteT(sb, ht, true);
            }
            

            
            
            sb.Append("INSERT INTO TB_S_M_SUBSIDY_MEM_D (TYPE, EMP_ID, SALARY_ID,ITEM_SEQ, CREATED_BY, CREATED_DT, UPDATED_BY, UPDATED_DT, FUNC_ID)");
            sb.Append(" Values (@TYPE, @EMP_ID, @SALARY_ID,@ITEM_SEQ, @CREATED_BY, GETDATE(), @UPDATED_BY, GETDATE(), @FUNC_ID)");
            ht.Add("@TYPE", TYPE);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SALARY_ID", SALARY_ID);
            ht.Add("@ITEM_SEQ", ITEM_SEQ);
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

    internal DataTable getEmp_Status(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select EMP_ID,EMP_NAME,EMP_STATUS + '-' + EMP_STATUS_DESC as EMP_STATUS from VW_H_EMP_DATA");
            sb.Append(" where EMP_ID = @EMP_ID");
            ht.Add("@EMP_ID", emp_id);
           
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public string deleteAllData()
    {        
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.Append("Delete from TB_S_M_SUBSIDY_MEM_D where EMP_ID = @EMP_ID");
        sb.Append(" and TYPE = @TYPE");

        ht.Add("@EMP_ID", EMP_ID);
        ht.Add("@TYPE", TYPE);

        dbConn.ExecuteT(sb, ht, true);

        return "0";
    }

    public void insertDetail(string SALARY_ID, int ITEM_SEQ)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();            

            sb.Append(" insert into TB_S_M_SUBSIDY_MEM_D ");
            sb.Append("(TYPE,EMP_ID,SALARY_ID,ITEM_SEQ,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" values(@TYPE,@EMP_ID,@SALARY_ID,@ITEM_SEQ,@CREATED_BY,getdate(),@UPDATED_BY,getdate(),@FUNC_ID)");

            ht.Add("@TYPE", TYPE);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SALARY_ID", SALARY_ID);
            ht.Add("@ITEM_SEQ", ITEM_SEQ);
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



}