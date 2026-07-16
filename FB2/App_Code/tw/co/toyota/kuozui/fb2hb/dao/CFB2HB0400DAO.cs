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
/// CFB2HB0400DAO 的摘要描述
/// </summary>
public class CFB2HB0400DAO : BaseDAO
{


    public string EMP_ID { get; set; }

    public string START_DT { get; set; }

    public string END_DT { get; set; }

    public string TRAINING_COMPANY { get; set; }

    public string TRAINING_GOAL { get; set; }

    public string CREATED_BY { get; set; }

    public string UPDATED_BY { get; set; }

    public string FUNC_ID { get; set; }

    public CFB2HB0400DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    //Gridview 查詢資料
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string emp_id, string start_dt_s, string start_dt_e,
                            string training_company, string training_goal)
    {
        try
        {
            if (sortExpression.Contains("EMP_ID"))
            {
                sortExpression = sortExpression.Replace("EMP_ID", "a.EMP_ID");
            }

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" a.EMP_ID,b.EMP_NAME,a.ORI_DEPT_NO,a.ORI_DEPT_NO + ' ' + a.ORI_DEPT_NAME_20 ORI_DEPT_FULL_NAME,");
            sb.Append(" a.TRAINING_COMPANY,a.TRAINING_GOAL,");
            sb.Append(" REPLACE(CONVERT(char(10), a.START_DT, 120),'-','/') START_DT,");
            sb.Append(" REPLACE(CONVERT(char(10), a.END_DT, 120),'-','/') END_DT");
            sb.Append(" from TB_H_M_EMP_TRAINING a,TB_H_M_EMP b");
            sb.Append(" where a.EMP_ID = b.EMP_ID");
            if (emp_id != "")
            {
                sb.Append(" and a.EMP_ID like @EMP_ID ");
                ht.Add("@EMP_ID", emp_id + "%");
            }
            if (start_dt_s != "")
            {
                sb.Append(" and a.END_DT >= @start_dt_s ");
                ht.Add("@start_dt_s", start_dt_s);
            }
            if (start_dt_e != "")
            {
                sb.Append(" and a.START_DT <= @start_dt_e ");
                ht.Add("@start_dt_e", start_dt_e);
            }

            if (training_company != "")
            {

                sb.Append(" and a.TRAINING_COMPANY like @TRAINING_COMPANY");
                ht.Add("@TRAINING_COMPANY", "%" + training_company + "%");

            }
            if (training_goal != "")
            {
                sb.Append(" and a.TRAINING_GOAL like @TRAINING_GOAL ");
                ht.Add("@TRAINING_GOAL", "%" + training_goal + "%");
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
    //Gridview 查詢總筆數
    public int getCount(int startRowIndex, int maximumRows, string emp_id, string start_dt_s, string start_dt_e,
                            string training_company, string training_goal)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select COUNT(*) total_record ");
            sb.Append(" from TB_H_M_EMP_TRAINING a,TB_H_M_EMP b");
            sb.Append(" where a.EMP_ID = b.EMP_ID");
            if (emp_id != "")
            {
                sb.Append(" and a.EMP_ID like @EMP_ID ");
                ht.Add("@EMP_ID", emp_id + "%");
            }
            if (start_dt_s != "")
            {
                sb.Append(" and a.END_DT >= @start_dt_s ");
                ht.Add("@start_dt_s", start_dt_s);
            }
            if (start_dt_e != "")
            {
                sb.Append(" and a.START_DT <= @start_dt_e ");
                ht.Add("@start_dt_e", start_dt_e);
            }

            if (training_company != "")
            {
                sb.Append(" and a.TRAINING_COMPANY like @TRAINING_COMPANY");
                ht.Add("@TRAINING_COMPANY", "%" + training_company + "%");

            }
            if (training_goal != "")
            {
                sb.Append(" and a.TRAINING_GOAL like @TRAINING_GOAL");
                ht.Add("@TRAINING_GOAL", "%" + training_goal + "%");
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



    internal void delete_Training(string emp_id, string start_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //寫log
            sb.Append(" update TB_H_M_EMP_TRAINING set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2HB040' ");
            sb.Append(" where EMP_ID = @EMP_ID and START_DT = @START_DT;");
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);

            sb.Append(" delete from TB_H_M_EMP_TRAINING where EMP_ID = @EMP_ID and START_DT = @START_DT;");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@START_DT", start_dt);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void updateTraining()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_H_M_EMP_TRAINING ");
            sb.Append(" set END_DT = @END_DT,TRAINING_COMPANY = @TRAINING_COMPANY,TRAINING_GOAL = @TRAINING_GOAL,UPDATED_BY = @UPDATED_BY, UPDATED_DT = GETDATE()");
            sb.Append(" where EMP_ID = @EMP_ID and START_DT = @START_DT");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@END_DT", END_DT);
            ht.Add("@TRAINING_COMPANY", TRAINING_COMPANY);
            ht.Add("@TRAINING_GOAL", TRAINING_GOAL);
            ht.Add("@START_DT", START_DT);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void addTraining()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("insert into TB_H_M_EMP_TRAINING ");
            sb.Append("(EMP_ID,START_DT,END_DT,TRAINING_COMPANY,TRAINING_GOAL,ORI_DEPT_NO,ORI_DEPT_FULL_NAME,ORI_DIV_DEPT_FULL_NAME,ORI_DEPT_NAME_20,ORI_DEPT_NAME_30,ORI_DEPT_NAME_40,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID) ");
            sb.Append(" select @EMP_ID,@START_DT,@END_DT,@TRAINING_COMPANY,@TRAINING_GOAL,DEPT_NO,DEPT_FULL_NAME,DIV_DEPT_FULL_NAME,DEPT_NAME_20,DEPT_NAME_30,DEPT_NAME_40,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID");
            sb.Append(" from VW_H_EMP_DATA where EMP_ID = @EMP_ID");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@START_DT", START_DT);
            ht.Add("@END_DT", END_DT);
            ht.Add("@TRAINING_COMPANY", TRAINING_COMPANY);
            ht.Add("@TRAINING_GOAL", TRAINING_GOAL);
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

    internal DataTable getEMPData(string EMP_ID)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select EMP_NAME,DEPT_NO + ' ' + DEPT_NAME_20 DEPT_NAME from VW_H_EMP_DATA Where VW_H_EMP_DATA.EMP_ID = @EMP_ID");
            ht.Add("@EMP_ID", EMP_ID);
            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {
            
            throw;
        }
    }

    internal DataTable getExistData(string emp_id, string start_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select EMP_ID from TB_H_M_EMP_TRAINING Where EMP_ID = @EMP_ID and START_DT = @START_DT");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@START_DT", start_dt);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            
            throw;
        }
    }

    internal DataTable getExistDataT(string emp_id, string start_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select EMP_ID from TB_H_M_EMP_TRAINING Where EMP_ID = @EMP_ID and START_DT = @START_DT");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@START_DT", start_dt);
            return dbConn.QueryT(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getDeptData(string dept_no)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select DEPT_NO from VW_H_DEPT_DATA Where  DEPT_NO= @DEPT_NO ");
            ht.Add("@DEPT_NO", dept_no);
            return dbConn.QueryT(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getDupData(string emp_id, string start_dt, string end_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select EMP_ID from TB_H_M_EMP_TRAINING Where EMP_ID = @EMP_ID and (START_DT between @START_DT and @END_DT or END_DT between @START_DT and @END_DT)");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@START_DT", start_dt);
            ht.Add("@END_DT", end_dt);
            return dbConn.QueryT(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getExitEmp(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select EMP_ID from VW_H_EMP_DATA Where EMP_ID = @EMP_ID");
            ht.Add("@EMP_ID", emp_id);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getEmpName(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select EMP_NAME from VW_H_EMP_DATA Where EMP_ID = @EMP_ID");
            ht.Add("@EMP_ID", emp_id);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
}