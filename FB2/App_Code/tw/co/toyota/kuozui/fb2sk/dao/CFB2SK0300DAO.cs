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
/// CFB2SK0300DAO 的摘要描述
/// </summary>
public class CFB2SK0300DAO : BaseDAO
{
    public Int64 RowNumber { get; set; }
    public string DATA_YM { get; set; }
    public string SALARY_AMT { get; set; }
    public string REMARK { get; set; }
    public string SALARY_TRANS_DT { get; set; }
    public string SALARY_TRANS_BY { get; set; }
    public string PROCESS_STATUS { get; set; }
    public string SALARY_DT { get; set; }
    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }

    //薪資月結控制檔(TB_S_M_SALARY_MONTH_CTRL)
    public string START_DT { get; set; }
    public string END_DT { get; set; }

    //for查詢欄位
    public string data_ym_s { get; set; }
    public string data_ym_e { get; set; }

    //public CFB2SK0300DAO()
    //{


    //}

    public DataTable GetData(int startRowIndex, int maximumRows, string sortExpression, string data_ym_s, string data_ym_e)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" DATA_YM,SALARY_AMT,REMARK,SALARY_TRANS_DT,SALARY_DT");
            sb.Append(" from TB_S_M_MUTUAL");

            if (data_ym_s != "")
            {
                if (data_ym_e != "")
                {
                    sb.Append(" where DATA_YM >= @data_ym_s and DATA_YM <= @data_ym_e ");
                    ht.Add("@data_ym_s", data_ym_s.Replace("/", ""));
                    ht.Add("@data_ym_e", data_ym_e.Replace("/", ""));
                }
                else
                {
                    sb.Append(" where DATA_YM >= @data_ym_s  ");
                    ht.Add("@data_ym_s", data_ym_s.Replace("/", ""));
                }

            }
            else if (data_ym_e != "")
            {
                sb.Append(" where DATA_YM <= @data_ym_e  ");
                ht.Add("@data_ym_e", data_ym_e.Replace("/", ""));
            }
            sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar) ");


            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public int GetCount(int startRowIndex, int maximumRows, string data_ym_s, string data_ym_e)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select COUNT(*) total_record From TB_S_M_MUTUAL");
            if (data_ym_s != "")
            {
                if (data_ym_e != "")
                {
                    sb.Append(" where DATA_YM >= @data_ym_s and DATA_YM <= @data_ym_e ");
                    ht.Add("@data_ym_s", data_ym_s.Replace("/", ""));
                    ht.Add("@data_ym_e", data_ym_e.Replace("/", ""));
                }
                else
                {
                    sb.Append(" where DATA_YM >= @data_ym_s  ");
                    ht.Add("@data_ym_s", data_ym_s.Replace("/", ""));
                }

            }
            else if (data_ym_e != "")
            {
                sb.Append(" where DATA_YM <= @data_ym_e  ");
                ht.Add("@data_ym_e", data_ym_e.Replace("/", ""));
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
    //查詢現有資料
    internal DataTable getExistData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select * from TB_S_M_MUTUAL");
            sb.Append(" where DATA_YM = @DATA_YM");
            ht.Add("@DATA_YM", DATA_YM);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
    public void Add_S_K_MUTUAL()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("INSERT INTO TB_S_M_MUTUAL (DATA_YM,SALARY_AMT,REMARK,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" Values (@DATA_YM,@SALARY_AMT,@REMARK,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@DATA_YM", DATA_YM);
            ht.Add("@SALARY_AMT", SALARY_AMT);
            ht.Add("@REMARK", REMARK);
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
    public void Update_S_K_MUTUAL()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Update TB_S_M_MUTUAL ");
            sb.Append(" Set SALARY_AMT = @SALARY_AMT,REMARK = @REMARK,UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID");
            sb.Append(" where DATA_YM = @DATA_YM");

            ht.Add("@DATA_YM", DATA_YM);
            ht.Add("@SALARY_AMT", SALARY_AMT);
            ht.Add("@REMARK", REMARK);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }
    public void Delete_S_K_MUTUAL(string DATA_YM)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //寫log
            sb.AppendLine(" update TB_S_M_MUTUAL set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2SK030' ");
            sb.AppendLine(" where DATA_YM = @DATA_YM; ");

            sb.Append(" Delete From TB_S_M_MUTUAL ");
            sb.Append(" where DATA_YM = @DATA_YM; ");
            ht.Add("@DATA_YM", DATA_YM);
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }
    public void Release_S_K_MUTUAL()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Update TB_S_M_MUTUAL ");
            sb.Append(" Set SALARY_TRANS_DT = GETDATE() ");
            sb.Append(" ,SALARY_DT = @SALARY_DT ");
            sb.Append(" ,PROCESS_STATUS = @PROCESS_STATUS ");
            sb.Append(" ,SALARY_TRANS_BY = @SALARY_TRANS_BY ");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY  ");
            sb.Append(" ,UPDATED_DT = GETDATE() ");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where DATA_YM = @DATA_YM");

            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@PROCESS_STATUS", "Y");
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@SALARY_TRANS_BY", SALARY_TRANS_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            ht.Add("@DATA_YM", DATA_YM);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }
    //新增 薪資月結控制檔(TB_S_M_SALARY_MONTH_CTRL)
    public void insert_SALARY_MONTH_CTRL()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            DateTime now = DateTime.Parse(DateTime.Now.ToString());
            sb.Append("INSERT INTO TB_S_M_SALARY_MONTH_CTRL (SALARY_TYPE,SALARY_YM,SALARY_DT,OPERATION_ID,PROCESS_DT,START_DT,END_DT,SALARY_LOCKED,LOCK_DT,FUNC_ID)");
            sb.Append(" Values (@SALARY_TYPE,@SALARY_YM,@SALARY_DT,@OPERATION_ID,@PROCESS_DT,@START_DT,@END_DT,@SALARY_LOCKED,@LOCK_DT,@FUNC_ID)");

            ht.Add("@SALARY_TYPE", "A");
            ht.Add("@SALARY_YM", DATA_YM);
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@OPERATION_ID", "H01");
            ht.Add("@PROCESS_DT", now);
            ht.Add("@START_DT", START_DT);
            ht.Add("@END_DT", END_DT);
            ht.Add("@SALARY_LOCKED", "N");
            ht.Add("@LOCK_DT", DBNull.Value);

            ht.Add("@FUNC_ID", "FB2SK030");
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //檢核能否薪資轉出
    public DataTable checkDataExist()
    {
        try
        {
            DataTable dt = new DataTable();

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" select * from TB_S_M_SALARY_CAL_H ");
            sb.Append(" where SALARY_TYPE='A' and SALARY_YM=@SALARY_YM ");
            ht.Add("@SALARY_YM", DATA_YM);
            dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch (Exception)
        {
            throw;
        }

    }

}