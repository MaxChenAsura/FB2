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
/// CFB2HB0300DAO 的摘要描述
/// </summary>
public class CFB2HB0300DAO : BaseDAO
{
    public string HR_CHG_NO { get; set; }
    public string EMP_ID { get; set; }
    public string ORI_DEPT_NO { get; set; }
    public string END_DT { get; set; }
    public string PLAN_END_DT { get; set; }    
    public string CHK_END_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string FUNC_ID { get; set; }
    
        
    public CFB2HB0300DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    //Gridview 查詢資料
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string emp_id, string start_dept_no, string end_dept_no,
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
            sb.Append(" a.EMP_ID,b.EMP_NAME,a.ORI_DEPT_NO,a.ORI_DEPT_NO + ' ' + a.ORI_DEPT_NAME ORI_DEPT_NAME,a.START_DEPT_NO,");
            sb.Append("a.START_DEPT_NO + ' ' + a.START_DEPT_NAME START_DEPT_NAME,a.END_DEPT_NO,a.END_DEPT_NO + ' ' + a.END_DEPT_NAME END_DEPT_NAME,");
            sb.Append(" REPLACE(CONVERT(char(10), a.START_DT, 120),'-','/') START_DT,");
            sb.Append(" REPLACE(CONVERT(char(10), a.PLAN_END_DT, 120),'-','/') PLAN_END_DT,");
            sb.Append(" REPLACE(CONVERT(char(10), a.END_DT, 120),'-','/') END_DT,a.HR_CHG_NO");
            sb.Append(" from TB_H_R_EMP_ASSIST a,TB_H_M_EMP b");
            sb.Append(" where a.EMP_ID = b.EMP_ID");
            if (emp_id != "")
            {
                sb.Append(" and a.EMP_ID like @EMP_ID ");
                ht.Add("@EMP_ID", emp_id + "%");
            }
            if (start_dept_no != "")
            {
                sb.Append(" and a.START_DEPT_NO = @START_DEPT_NO ");
                ht.Add("@START_DEPT_NO", start_dept_no);
            }
            if (end_dept_no != "")
            {
                sb.Append(" and a.END_DEPT_NO = @END_DEPT_NO ");
                ht.Add("@END_DEPT_NO", end_dept_no);
            }

            if (start_dt_s != "")
            {

                sb.Append(" and a.END_DT >= CONVERT(datetime,@start_dt_s)");
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
    public int getCount(int startRowIndex, int maximumRows, string emp_id, string start_dept_no, string end_dept_no,
                            string start_dt_s, string start_dt_e, string is_valid)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select COUNT(*) total_record ");
            sb.Append(" from TB_H_R_EMP_ASSIST a,TB_H_M_EMP b");
            sb.Append(" where a.EMP_ID = b.EMP_ID");
            if (emp_id != "")
            {
                sb.Append(" and a.EMP_ID like @EMP_ID ");
                ht.Add("@EMP_ID", emp_id + "%");
            }
            if (start_dept_no != "")
            {
                sb.Append(" and a.START_DEPT_NO = @START_DEPT_NO ");
                ht.Add("@START_DEPT_NO", start_dept_no);
            }
            if (end_dept_no != "")
            {
                sb.Append(" and a.END_DEPT_NO = @END_DEPT_NO ");
                ht.Add("@END_DEPT_NO", end_dept_no);
            }

            if (start_dt_s != "")
            {

                sb.Append(" and a.END_DT >= CONVERT(datetime,@start_dt_s)");
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
                        A.HR_CHG_NO,A.EMP_ID,A.ORI_DEPT_NO,A.ORI_DEPT_NAME
                        ,iif(END_DT is null,'',CONVERT(VARCHAR(10),A.END_DT,111) ) AS END_DT
                        ,iif(PLAN_END_DT is null,'',CONVERT(VARCHAR(10),A.PLAN_END_DT,111) ) AS PLAN_END_DT
                        ,B.EMP_NAME
                        FROM TB_H_R_EMP_ASSIST A
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

    //檢查部門是否存在
    public int chkDEPT()
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@"SELECT count(*) AS resultCount
                        FROM TB_H_M_DEPT
                        WHERE 1=1
                        and @END_DT BETWEEN START_DT AND END_DT
                        and DEPT_NO=@ORI_DEPT_NO
                         ");
            ht.Add("@ORI_DEPT_NO", ORI_DEPT_NO);
            ht.Add("@END_DT", CHK_END_DT);

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                t = (int)dt.Rows[0]["resultCount"];
            }

            return t;
        }
        catch (Exception)
        {

            throw;
        }
    }

    //修改履歷檔
    public void update()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(@" UPDATE A
                SET END_DT= @END_DT
                ,PLAN_END_DT= @PLAN_END_DT
                ,ORI_DEPT_NO=@ORI_DEPT_NO
                ,ORI_DEPT_NAME=B.DEPT_NAME
                ,ORI_DEPT_NAME_20=B.DEPT_NAME_20
                ,ORI_DEPT_NAME_30=B.DEPT_NAME_30
                ,ORI_DEPT_NAME_40=B.DEPT_NAME_40
                ,ORI_DEPT_NAME_50=B.DEPT_NAME_50
                ,ORI_DEPT_NAME_60=B.DEPT_NAME_60
                ,ORI_DEPT_NAME_70=B.DEPT_NAME_70
                ,UPDATED_BY = @UPDATED_BY
                ,UPDATED_DT = getdate()
                ,FUNC_ID = @FUNC_ID
                FROM TB_H_R_EMP_ASSIST A 
                LEFT JOIN (select * from TB_H_R_DEPT_DATA B where DEPT_NO=@ORI_DEPT_NO) B ON 1=1
                WHERE A.EMP_ID=@EMP_ID
                AND A.HR_CHG_NO=@HR_CHG_NO
                 ");

            //PK值
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@HR_CHG_NO", HR_CHG_NO);

            //修改資料
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            ht.Add("@ORI_DEPT_NO", ORI_DEPT_NO);
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

    //修改人事異動單(TB_H_M_EMP_HR_CHANGE_H	人事異動主檔)
    public void update_CHG_H()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(@" 
                UPDATE TB_H_M_EMP_HR_CHANGE_H
                SET PLAN_END_DT= @PLAN_END_DT
                ,END_HR_CHG_NO = @END_HR_CHG_NO
                ,UPDATED_BY = @UPDATED_BY
                ,UPDATED_DT = getdate()
                ,FUNC_ID = @FUNC_ID               
                WHERE EMP_ID=@EMP_ID
                AND  HR_CHG_NO=@HR_CHG_NO
            ");

            //PK值
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@HR_CHG_NO", HR_CHG_NO);

            //修改資料
            if (END_DT == "") {
                ht.Add("@END_HR_CHG_NO", "");
            }else{
                ht.Add("@END_HR_CHG_NO", FUNC_ID);
            }

            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

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