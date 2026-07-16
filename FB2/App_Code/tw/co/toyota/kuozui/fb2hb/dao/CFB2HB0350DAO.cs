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
/// CFB2HB0350DAO 的摘要描述
/// </summary>
public class CFB2HB0350DAO : BaseDAO
{
    public string HR_CHG_NO { get; set; }
    public string EMP_ID { get; set; }
    public string ORI_DEPT_NO { get; set; }
    public string END_DT { get; set; }
    public string PLAN_END_DT { get; set; }    
    public string CHK_END_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string FUNC_ID { get; set; }
    
        
    public CFB2HB0350DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    //Gridview 查詢資料
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string emp_id, 
                            string start_dt_s, string start_dt_e, string is_valid)
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
            sb.Append(@" a.HR_CHG_NO,a.EMP_ID
                        ,CONVERT(VARCHAR(10),START_DT,111)  START_DT
                        ,CONVERT(VARCHAR(10),PLAN_END_DT,111)  PLAN_END_DT
                        ,CONVERT(VARCHAR(10),END_DT,111)  END_DT
                        ,B.EMP_NAME
                        ,B.DIV_DEPT_FULL_NAME as DEPT_NAME
                        from TB_H_R_EMP_RETENTION A
                        INNER JOIN VW_H_EMP_DATA B ON A.EMP_ID = B.EMP_ID ");
            sb.Append(" where 1=1 ");
            if (emp_id != "")
            {
                sb.Append(" and a.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
            }
  
            if (start_dt_s != "")
            {
                sb.Append(" and  iif(a.END_DT is null,a.PLAN_END_DT,a.END_DT)  >= CONVERT(datetime,@start_dt_s)");
                ht.Add("@start_dt_s", start_dt_s);

            }
            if (start_dt_e != "")
            {
                sb.Append(" and a.START_DT <= CONVERT(datetime,@start_dt_e) ");
                ht.Add("@start_dt_e", start_dt_e);
            }
            if (is_valid == "1")
            {
                sb.Append(" and a.END_DT is NULL ");
            }
            if (is_valid == "2")
            {
                sb.Append(" and a.END_DT is not NULL ");
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
    public int getCount(int startRowIndex, int maximumRows, string emp_id, 
                            string start_dt_s, string start_dt_e, string is_valid)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select COUNT(*) total_record ");
            sb.Append(" from TB_H_R_EMP_RETENTION a");
            sb.Append(" where 1=1");
            if (emp_id != "")
            {
                sb.Append(" and a.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
            }           

            if (start_dt_s != "")
            {

                sb.Append(" and iif(a.END_DT is null,a.PLAN_END_DT,a.END_DT) >= CONVERT(datetime,@start_dt_s)");
                ht.Add("@start_dt_s", start_dt_s);

            }
            if (start_dt_e != "")
            {
                sb.Append(" and a.START_DT <= CONVERT(datetime,@start_dt_e) ");
                ht.Add("@start_dt_e", start_dt_e);
            }
            if (is_valid == "1")
            {
                sb.Append(" and a.END_DT is NULL ");
            }
            if (is_valid == "2")
            {
                sb.Append(" and a.END_DT is not NULL ");
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

    //取得修改頁面的顯示資料
    public DataTable getiniData(string emp_id, string hr_chg_no)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@"SELECT 
                        A.HR_CHG_NO,A.EMP_ID
                        ,iif(END_DT is null,'',CONVERT(VARCHAR(10),A.END_DT,111) ) AS END_DT
                        ,iif(PLAN_END_DT is null,'',CONVERT(VARCHAR(10),A.PLAN_END_DT,111) ) AS PLAN_END_DT
                        ,B.EMP_NAME
                        FROM TB_H_R_EMP_RETENTION A
                        LEFT JOIN TB_H_M_EMP B ON A.EMP_ID=B.EMP_ID
                        WHERE A.EMP_ID=@EMP_ID
                        AND A.HR_CHG_NO=@HR_CHG_NO
                        ");

            ht.Add("@EMP_ID", emp_id);
            ht.Add("@HR_CHG_NO", hr_chg_no);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

  

    public void update()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(@" UPDATE A
                SET END_DT= @END_DT
                ,PLAN_END_DT = @PLAN_END_DT
                ,UPDATED_BY = @UPDATED_BY
                ,UPDATED_DT = getdate()
                ,FUNC_ID = @FUNC_ID
                FROM TB_H_R_EMP_RETENTION A 
                WHERE A.EMP_ID=@EMP_ID
                AND A.HR_CHG_NO=@HR_CHG_NO
                 ");

            //PK值
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@HR_CHG_NO", HR_CHG_NO);

            //修改資料
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            if(END_DT!="")
                ht.Add("@END_DT", END_DT);
            else
                ht.Add("@END_DT", DBNull.Value);

            if (PLAN_END_DT != "")
                ht.Add("@PLAN_END_DT", PLAN_END_DT);
            else
                ht.Add("@PLAN_END_DT", DBNull.Value);

            
           
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }


}