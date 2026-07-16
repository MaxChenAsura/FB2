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
public class CFB2HA0600DAO : BaseDAO
{
    public CFB2HA0600DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public string RowNumber { get; set; }
    public string QDATAKEY { get; set; }
    public string HR_CHG_CD { get; set; }
    public string HR_CHG_DESC { get; set; }
    public string IS_VALID { get; set; }
    public string IS_FOR_BATCH { get; set; }
    public string IS_FOR_TRANSFER_IN { get; set; }
    public string IS_SHOW { get; set; }
    public string IS_PROFESSION_PJOB { get; set; }
    public string UPD_RIGHT_CD { get; set; }
    public string IS_INS_EARLIER { get; set; }
    public string IS_TEMP { get; set; }
    public string IS_LEAVE { get; set; }
    public string IS_UPD_HR { get; set; }
    public string IS_UPD_DEPT_HEAD { get; set; }
    public string SALARY_PROC_CD { get; set; }
    public string CONTRACT_PROC_CD { get; set; }
    public string EMP_CHG_STATUS { get; set; }
    public string REMARK { get; set; }
    public string INSURANCE_PROC_CD { get; set; }
    public string DUTY_PROC_CD { get; set; }
  
    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }
    //for查詢欄位
    public string ddl_SYS_ID { get; set; }



    public DataTable getEMP_CHG_STATUS()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select SYS_CD , MAIN_CD , SUB_CD ,SUB_DESC from TB_9_M_COMM_D");
            sb.Append(" where SYS_CD='HC' and MAIN_CD='EMP_CHG_STATUS'  ");
            sb.Append(" ORDER BY  ORDER_SEQ ASC ");
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
    internal DataTable getDefaultData(string HR_CHG_CD_ID)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
          
            sb.Append(" select ROW_NUMBER() OVER(ORDER BY HR_CHG_CD ) As RowNumber,");
            sb.Append(" a.*,b.SUB_CD+'-'+b.SUB_DESC as EMP_CHG_STATUS_all");
            sb.Append(" from TB_H_M_HR_CHANGE_CODE a");
            sb.Append(" left join TB_9_M_COMM_D b on b.SYS_CD='HC' and b.MAIN_CD='EMP_CHG_STATUS' and a.EMP_CHG_STATUS=b.SUB_CD");
            sb.Append(" where a.HR_CHG_CD = @HR_CHG_CD");
            ht.Add("@HR_CHG_CD", HR_CHG_CD_ID);
            

            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable get_EMP_CHG_STATUS_Data()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select SYS_CD , MAIN_CD , SUB_CD ,SUB_DESC from TB_9_M_COMM_D");
            sb.Append(" where SYS_CD='HC' and MAIN_CD=EMP_CHG_STATUS  ");

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string txt_HR_CHG_CD, string ddl_IS_VALID)
    {
        try
        {
            if (sortExpression == "")
            {
                sortExpression = "CAR_TYPE";
            }
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * From");
            sb.Append(" (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append("*,'' as 'IS_ADD_WORK_YEARS'");
            sb.Append(" from TB_H_M_HR_CHANGE_CODE");
            sb.Append(" where 1=1");
            if (txt_HR_CHG_CD != "")
            {
                sb.Append(" and HR_CHG_CD LIKE @HR_CHG_CD ");
                ht.Add("@HR_CHG_CD", string.Format("%{0}%", txt_HR_CHG_CD));
            }
            if (ddl_IS_VALID != "")
            {
                sb.Append(" and IS_VALID = @IS_VALID ");
                ht.Add("@IS_VALID", ddl_IS_VALID);
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
    public int getCount(int startRowIndex, int maximumRows, string ddl_IS_VALID, string txt_HR_CHG_CD)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) total_record ");
            sb.Append(" from TB_H_M_HR_CHANGE_CODE");
            sb.Append(" where 1=1");
            if (txt_HR_CHG_CD != "")
            {
                sb.Append(" and HR_CHG_CD LIKE @HR_CHG_CD ");
                ht.Add("@HR_CHG_CD", string.Format("%{0}%", txt_HR_CHG_CD));
            }
            if (ddl_IS_VALID != "")
            {
                sb.Append(" and IS_VALID = @IS_VALID ");
                ht.Add("@IS_VALID", ddl_IS_VALID);
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

    public string deleteData_1(string deleteitem)
    {
        //刪除人事異動代碼檔
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        //寫log
        sb.Append(" update TB_H_M_HR_CHANGE_CODE set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2HA060' ");
        sb.Append(" where HR_CHG_CD = @HR_CHG_CD; ");
        ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);

        sb.Append(" Delete from TB_H_M_HR_CHANGE_CODE   ");
        sb.Append(" where HR_CHG_CD = @HR_CHG_CD;");
        ht.Add("@HR_CHG_CD", deleteitem);
        dbConn.ExecuteT(sb, ht, true);

        return "0";
    }
    public string deleteData(string deleteitem)
    {
        //刪除人事異動代碼檔
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        //寫log
        sb.Append(" update TB_H_M_HR_CHANGE_CODE set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2HA060' ");
        sb.Append(" where HR_CHG_CD = @HR_CHG_CD; ");
        ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);

        sb.Append(" delete from TB_H_M_HR_CHANGE_CODE ");
        sb.Append(" where HR_CHG_CD = @HR_CHG_CD;");
        ht.Add("@HR_CHG_CD", deleteitem);
        dbConn.ExecuteT(sb, ht, true);

        return "0";
    }
    internal DataTable getExistData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select * from TB_H_M_HR_CHANGE_CODE where HR_CHG_CD = @HR_CHG_CD");
            ht.Add("@HR_CHG_CD", HR_CHG_CD);
           
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
            sb.Append("INSERT INTO TB_H_M_HR_CHANGE_CODE (HR_CHG_CD,HR_CHG_DESC,IS_VALID,IS_FOR_BATCH,IS_FOR_TRANSFER_IN,IS_SHOW,IS_PROFESSION_PJOB,UPD_RIGHT_CD,IS_INS_EARLIER,IS_TEMP,IS_LEAVE,IS_UPD_HR,IS_UPD_DEPT_HEAD,SALARY_PROC_CD,CONTRACT_PROC_CD,EMP_CHG_STATUS,REMARK,INSURANCE_PROC_CD,DUTY_PROC_CD,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" Values (@HR_CHG_CD,@HR_CHG_DESC,@IS_VALID,@IS_FOR_BATCH,@IS_FOR_TRANSFER_IN,@IS_SHOW,@IS_PROFESSION_PJOB,@UPD_RIGHT_CD,@IS_INS_EARLIER,@IS_TEMP,@IS_LEAVE,@IS_UPD_HR,@IS_UPD_DEPT_HEAD,@SALARY_PROC_CD,@CONTRACT_PROC_CD,@EMP_CHG_STATUS,@REMARK,@INSURANCE_PROC_CD,@DUTY_PROC_CD,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");
            ht.Add("@HR_CHG_CD", HR_CHG_CD);
            ht.Add("@HR_CHG_DESC", HR_CHG_DESC);
            ht.Add("@IS_VALID", IS_VALID);
            ht.Add("@IS_FOR_BATCH", IS_FOR_BATCH);
            ht.Add("@IS_FOR_TRANSFER_IN", IS_FOR_TRANSFER_IN);
            ht.Add("@IS_SHOW", IS_SHOW);
            ht.Add("@IS_PROFESSION_PJOB", IS_PROFESSION_PJOB);
            ht.Add("@UPD_RIGHT_CD", UPD_RIGHT_CD);
            ht.Add("@IS_INS_EARLIER", IS_INS_EARLIER);
            ht.Add("@IS_TEMP", IS_TEMP);
            ht.Add("@IS_LEAVE", IS_LEAVE);
            ht.Add("@IS_UPD_HR", IS_UPD_HR);
            ht.Add("@IS_UPD_DEPT_HEAD", IS_UPD_DEPT_HEAD);
            ht.Add("@SALARY_PROC_CD", SALARY_PROC_CD);
            ht.Add("@CONTRACT_PROC_CD", CONTRACT_PROC_CD);
            ht.Add("@EMP_CHG_STATUS", EMP_CHG_STATUS);
            ht.Add("@REMARK", REMARK);
            ht.Add("@INSURANCE_PROC_CD", INSURANCE_PROC_CD);
            ht.Add("@DUTY_PROC_CD", DUTY_PROC_CD);
            ht.Add("@CREATED_BY", CREATED_BY);
            ht.Add("@UPDATED_BY", UPDATED_BY);
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
            sb.Append("Update TB_H_M_HR_CHANGE_CODE ");
            sb.Append(" Set HR_CHG_DESC=@HR_CHG_DESC,IS_VALID=@IS_VALID,IS_FOR_BATCH=@IS_FOR_BATCH,IS_FOR_TRANSFER_IN=@IS_FOR_TRANSFER_IN,IS_SHOW=@IS_SHOW,IS_PROFESSION_PJOB=@IS_PROFESSION_PJOB,UPD_RIGHT_CD=@UPD_RIGHT_CD,IS_INS_EARLIER=@IS_INS_EARLIER,IS_TEMP=@IS_TEMP,IS_LEAVE=@IS_LEAVE,IS_UPD_HR=@IS_UPD_HR,IS_UPD_DEPT_HEAD=@IS_UPD_DEPT_HEAD,SALARY_PROC_CD=@SALARY_PROC_CD,CONTRACT_PROC_CD=@CONTRACT_PROC_CD,EMP_CHG_STATUS=@EMP_CHG_STATUS,REMARK=@REMARK,INSURANCE_PROC_CD=@INSURANCE_PROC_CD,DUTY_PROC_CD=@DUTY_PROC_CD,UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE()");
            sb.Append(" where HR_CHG_CD = @HR_CHG_CD");
            ht.Add("@HR_CHG_CD", HR_CHG_CD);
            ht.Add("@HR_CHG_DESC", HR_CHG_DESC);
            ht.Add("@IS_VALID", IS_VALID);
            ht.Add("@IS_FOR_BATCH", IS_FOR_BATCH);
            ht.Add("@IS_FOR_TRANSFER_IN", IS_FOR_TRANSFER_IN);
            ht.Add("@IS_SHOW", IS_SHOW);
            ht.Add("@IS_PROFESSION_PJOB", IS_PROFESSION_PJOB);
            ht.Add("@UPD_RIGHT_CD", UPD_RIGHT_CD);
            ht.Add("@IS_INS_EARLIER", IS_INS_EARLIER);
            ht.Add("@IS_TEMP", IS_TEMP);
            ht.Add("@IS_LEAVE", IS_LEAVE);
            ht.Add("@IS_UPD_HR", IS_UPD_HR);
            ht.Add("@IS_UPD_DEPT_HEAD", IS_UPD_DEPT_HEAD);
            ht.Add("@SALARY_PROC_CD", SALARY_PROC_CD);
            ht.Add("@CONTRACT_PROC_CD", CONTRACT_PROC_CD);
            ht.Add("@EMP_CHG_STATUS", EMP_CHG_STATUS);
            ht.Add("@REMARK", REMARK);
            ht.Add("@INSURANCE_PROC_CD", INSURANCE_PROC_CD);
            ht.Add("@DUTY_PROC_CD", DUTY_PROC_CD);            
            ht.Add("@UPDATED_BY", UPDATED_BY);
           
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
}