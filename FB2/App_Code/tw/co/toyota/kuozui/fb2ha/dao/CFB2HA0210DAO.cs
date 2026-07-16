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
/// CFB2HA0210DAO 的摘要描述
/// </summary>
public class CFB2HA0210DAO : BaseDAO
{

    public string DEPT_NO { get; set; }

    public string START_DT { get; set; }

    public string END_DT { get; set; }

    public string REMARK { get; set; }
    public object CREATED_BY { get; set; }
    public string UPDATED_BY { get; set; }

    public string FUNC_ID { get; set; }


    public CFB2HA0210DAO()
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
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string dept_no, string start_dt_s,
                            string start_dt_e, string end_dt_s, string end_dt_e, string is_valid, string dept_level)
    {
        try
        {
            if (sortExpression.Contains("DEPT_LEVEL"))
            {
                sortExpression = sortExpression.Replace("DEPT_LEVEL", "a.DEPT_LEVEL");
            }
            if (sortExpression.Contains("DEPT_NO"))
            {
                sortExpression = sortExpression.Replace("DEPT_NO", "a.DEPT_NO");
            }
            if (sortExpression.Contains("UP_DEPT_NAME"))
            {
                sortExpression = sortExpression.Replace("UP_DEPT_NAME", "a.UP_DEPT_NO");
            }

            if (sortExpression.Contains("DEPT_NAME"))
            {
                sortExpression = sortExpression.Replace("DEPT_NAME", "b.DEPT_NAME");
            }

            if (sortExpression.Contains("START_DT"))
            {
                sortExpression = sortExpression.Replace("START_DT", "a.START_DT");
            }
            if (sortExpression.Contains("END_DT"))
            {
                sortExpression = sortExpression.Replace("END_DT", "a.END_DT");
            }
            if (sortExpression.Contains("REMARK"))
            {
                sortExpression = sortExpression.Replace("REMARK", "a.REMARK");
            }

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" a.DEPT_LEVEL,c.DEPT_LEVEL_DESC,a.DEPT_NO,b.DEPT_NAME,REPLACE(CONVERT(char(10), a.START_DT, 120),'-','/') START_DT,");
            sb.Append(" REPLACE(CONVERT(char(10), a.END_DT, 120),'-','/') END_DT,a.REMARK,");
            sb.Append(" a.UP_DEPT_NO + '-' + u.DEPT_NAME UP_DEPT_NAME ");
            sb.Append(" from TB_H_M_DEPT_ORG a ");
            sb.Append(" left join TB_H_M_DEPT b ");
            sb.Append("     on a.DEPT_NO = b.DEPT_NO  and b.START_DT <=a.START_DT and b.END_DT >=a.START_DT ");
            sb.Append(" left join TB_H_M_DEPT_LEVEL c");
            sb.Append("     on a.DEPT_LEVEL = c.DEPT_LEVEL ");
            sb.Append(" left join TB_H_M_DEPT u ");
            sb.Append("     on a.UP_DEPT_NO = u.DEPT_NO and u.START_DT <= GETDATE() and u.END_DT >=GETDATE() ");
            sb.Append(" Where  1 = 1 ");
            if (dept_level != "-1")
            {
                sb.Append(" and a.DEPT_LEVEL = @DEPT_LEVEL ");
                ht.Add("@DEPT_LEVEL", dept_level);
            }
            if (dept_no != "")
            {
                sb.Append(" and a.UP_DEPT_NO LIKE @DEPT_NO ");
                ht.Add("@DEPT_NO", dept_no + "%");
            }

            if (start_dt_s != "")
            {
                sb.AppendLine(" and a.START_DT >= @START_DT_S ");
                ht.Add("@START_DT_S", start_dt_s);
            }
            if (start_dt_e != "")
            {
                sb.AppendLine(" and a.START_DT <= @START_DT_E ");
                ht.Add("@START_DT_E", start_dt_e);
            }

            if (end_dt_s != "")
            {
                sb.AppendLine(" and a.END_DT >= @END_DT_S ");
                ht.Add("@END_DT_S", end_dt_s);
            }
            if (end_dt_e != "")
            {
                sb.AppendLine(" and a.END_DT <= @END_DT_E ");
                ht.Add("@END_DT_E", end_dt_e);
            }

            if (is_valid == "Y")
            {
                sb.AppendLine(" and (a.START_DT <= @CURRENT_DT and a.END_DT >= @CURRENT_DT) ");
                ht.Add("@CURRENT_DT", DateTime.Now.Date.ToString("yyyy/MM/dd"));
            }
            if (is_valid == "N")
            {
                sb.AppendLine(" and (a.START_DT >= @CURRENT_DT  or a.END_DT <= @CURRENT_DT ) ");
                ht.Add("@CURRENT_DT", DateTime.Now.Date.ToString("yyyy/MM/dd"));
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
    public int getCount(int startRowIndex, int maximumRows, string dept_no, string start_dt_s,
                            string start_dt_e, string end_dt_s, string end_dt_e, string is_valid, string dept_level)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select COUNT(*) total_record ");
            sb.Append(" from TB_H_M_DEPT_ORG a ");
            sb.Append(" left join TB_H_M_DEPT b ");
            sb.Append("     on a.DEPT_NO = b.DEPT_NO and b.START_DT <=a.START_DT and b.END_DT >=a.START_DT ");
            sb.Append(" left join TB_H_M_DEPT_LEVEL c");
            sb.Append("     on a.DEPT_LEVEL = c.DEPT_LEVEL ");
            sb.Append(" left join TB_H_M_DEPT u ");
            sb.Append("     on a.UP_DEPT_NO = u.DEPT_NO and u.START_DT <= GETDATE() and u.END_DT >=GETDATE() ");
            sb.Append(" Where  1 = 1 ");
            if (dept_level != "-1")
            {
                sb.Append(" and a.DEPT_LEVEL = @DEPT_LEVEL ");
                ht.Add("@DEPT_LEVEL", dept_level);
            }
            if (dept_no != "")
            {
                sb.Append(" and a.UP_DEPT_NO LIKE @DEPT_NO ");
                ht.Add("@DEPT_NO", dept_no + "%");
            }
            if (start_dt_s != "")
            {
                sb.AppendLine(" and a.START_DT >= @START_DT_S ");
                ht.Add("@START_DT_S", start_dt_s);
            }
            if (start_dt_e != "")
            {
                sb.AppendLine(" and a.START_DT <= @START_DT_E ");
                ht.Add("@START_DT_E", start_dt_e);
            }

            if (end_dt_s != "")
            {
                sb.AppendLine(" and a.END_DT >= @END_DT_S ");
                ht.Add("@END_DT_S", end_dt_s);
            }
            if (end_dt_e != "")
            {
                sb.AppendLine(" and a.END_DT <= @END_DT_E ");
                ht.Add("@END_DT_E", end_dt_e);
            }

            if (is_valid == "Y")
            {
                sb.AppendLine(" and (a.START_DT <= @CURRENT_DT and a.END_DT >= @CURRENT_DT) ");
                ht.Add("@CURRENT_DT", DateTime.Now.Date.ToString("yyyy/MM/dd"));
            }
            if (is_valid == "N")
            {
                sb.AppendLine(" and (a.START_DT >= @CURRENT_DT  or a.END_DT <= @CURRENT_DT ) ");
                ht.Add("@CURRENT_DT", DateTime.Now.Date.ToString("yyyy/MM/dd"));
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


    internal void updateDept_Org()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_H_M_DEPT_ORG set END_DT = @END_DT,REMARK = @REMARK,");
            sb.Append(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID");
            sb.Append(" where DEPT_NO = @DEPT_NO and START_DT = @START_DT");
            ht.Add("@END_DT", END_DT);
            ht.Add("@REMARK", REMARK);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@DEPT_NO", DEPT_NO);
            ht.Add("@START_DT", START_DT);
            ht.Add("@FUNC_ID", FUNC_ID);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }


    //Gridview 查詢資料
    public DataTable getAddData(int startRowIndex, int maximumRows, string sortExpression, string dept_no, string dept_level,
                            string start_dt)
    {
        try
        {



            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" a.DEPT_NO,a.DEPT_NAME,a.START_DT");
            sb.Append(" from TB_H_M_DEPT a ");
            sb.Append(" where not exists(select DEPT_NO from TB_H_M_DEPT_ORG WHERE END_DT >= @START_DT and TB_H_M_DEPT_ORG.DEPT_NO = a.DEPT_NO) ");
            sb.Append(" and a.END_DT >= @START_DT ");
            ht.Add("@START_DT", start_dt);
            if (dept_no != "")
            {
                sb.Append(" and a.DEPT_NO LIKE @DEPT_NO ");
                ht.Add("@DEPT_NO", dept_no + "%");
            }
            if (dept_level != "-1")
            {
                sb.Append(" and a.DEPT_LEVEL = @DEPT_LEVEL ");
                ht.Add("@DEPT_LEVEL", dept_level);
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
    public int getAddCount(int startRowIndex, int maximumRows, string dept_no, string dept_level,
                            string start_dt)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select COUNT(*) total_record ");
            sb.Append(" from TB_H_M_DEPT a ");
            sb.Append(" where not exists(select DEPT_NO from TB_H_M_DEPT_ORG WHERE END_DT >= @START_DT and TB_H_M_DEPT_ORG.DEPT_NO = a.DEPT_NO) ");
            sb.Append(" and a.END_DT >= @START_DT ");
            ht.Add("@START_DT", start_dt);
            if (dept_no != "")
            {
                sb.Append(" and a.DEPT_NO LIKE @DEPT_NO ");
                ht.Add("@DEPT_NO", dept_no + "%");
            }
            if (dept_level != "-1")
            {
                sb.Append(" and a.DEPT_LEVEL = @DEPT_LEVEL ");
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

    internal DataTable getExistDeptOrg(string dept_no, string start_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select DEPT_NO From");
            sb.Append(" TB_H_M_DEPT_ORG");

            sb.Append(" where DEPT_NO = @DEPT_NO and START_DT = @START_DT");
            ht.Add("@DEPT_NO", dept_no);
            ht.Add("@START_DT", start_dt);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void addDeptLevel(string dept_no, string up_dept_no, string up_dept_level, string start_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" insert into TB_H_M_DEPT_ORG (DEPT_NO,START_DT,END_DT,DEPT_LEVEL,UP_DEPT_NO,UP_DEPT_LEVEL,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" Select @DEPT_NO,@START_DT,'9999/12/31',a.DEPT_LEVEL,@UP_DEPT_NO,@UP_DEPT_LEVEL,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID");
            sb.Append(" from TB_H_M_DEPT a");
            sb.Append(" where DEPT_NO = @DEPT_NO  ");
            //sb.Append(" and GETDATE() between START_DT and END_DT ");
            sb.Append(" and END_DT >= @START_DT ");
            ht.Add("@DEPT_NO", dept_no);
            ht.Add("@START_DT", start_dt);
            ht.Add("@UP_DEPT_NO", up_dept_no);
            ht.Add("@UP_DEPT_LEVEL", up_dept_level);
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



    internal DataTable getUpDeptData(string up_dept_no)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select  top 1  DEPT_NAME,DEPT_LEVEL From");
            sb.Append(" TB_H_M_DEPT");

            sb.Append(" where DEPT_NO = @DEPT_NO");
            ht.Add("@DEPT_NO", up_dept_no);
            sb.Append(" order by END_DT desc ");
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public bool checkDeptNoIsExist(string dept_no)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select DEPT_NO ");
            sb.Append(" from TB_H_M_DEPT");
            sb.Append(" where DEPT_NO = @DEPT_NO and START_DT <= DATEADD(DAY,7, GETDATE()) and END_DT >= GETDATE()");
            ht.Add("@DEPT_NO", dept_no);
            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void deleteDept_Org(string DEPT_NO, string START_DT)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //寫log
            sb.Append(" update TB_H_M_DEPT_ORG set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2HA021' ");
            sb.Append(" where DEPT_NO = @DEPT_NO and CONVERT(VARCHAR(10),START_DT,111) = @START_DT; ");
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);

            sb.Append(" delete from TB_H_M_DEPT_ORG ");
            sb.Append(" where DEPT_NO = @DEPT_NO and CONVERT(VARCHAR(10),START_DT,111) = @START_DT; ");
            ht.Add("@DEPT_NO", DEPT_NO);
            ht.Add("@START_DT", START_DT);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable getSalaryYm()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select dbo.FN_S_SALARY_YM() as SALARY_YM ");
            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
}