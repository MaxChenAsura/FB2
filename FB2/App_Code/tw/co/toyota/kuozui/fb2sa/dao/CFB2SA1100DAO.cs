using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using FB2.tw.co.toyota.kuozui.dao;
using System.Text;
using System.Collections;

/// <summary>
/// CF2SA1100DAO 的摘要描述
/// </summary>
public class CFB2SA1100DAO : BaseDAO
{

    public CFB2SA1100DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public DataTable GetData(int startRowIndex, int maximumRows, string sortExpression, string data_year, string education_cd, string level_cd
                           , string grade_cd, string ws_cd, string grade_year)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" a.WS_CD+'-'+b.SUB_DESC as WS_CD,a.LEVEL_CD,a.GRADE_CD,a.EDUCATION_CD+'-'+c.SUB_DESC as EDUCATION_CD");
            sb.Append(" ,a.GRADE_YEAR,a.LEVEL_PAY1,a.LEVEL_PAY2,a.LEVEL_PAY3");
            sb.Append(" from TB_S_M_HIRING_SALARY_TMP_D a");
            sb.Append(" left join TB_9_M_COMM_D b on a.WS_CD=b.SUB_CD and b.SYS_CD='HB' and b.MAIN_CD='WS_CD'");
            sb.Append(" left join TB_9_M_COMM_D c on a.EDUCATION_CD=c.SUB_CD and c.SYS_CD='HB' and c.MAIN_CD='EDUCATION_CD'");
            sb.Append(" where a.DATA_YEAR=@DATA_YEAR");
            ht.Add("@DATA_YEAR", data_year);
            if (education_cd != "" && education_cd != "-1")
            {
                sb.Append(" and a.EDUCATION_CD=@EDUCATION_CD  ");
                ht.Add("@EDUCATION_CD", education_cd);
            }
            if (level_cd != "")
            {
                sb.Append(" and a.LEVEL_CD like '%' + @LEVEL_CD + '%'");
                ht.Add("@LEVEL_CD", level_cd);
            }
            if (grade_cd != "")
            {
                sb.Append(" and a.GRADE_CD like '%' + @GRADE_CD + '%'");
                ht.Add("@GRADE_CD", grade_cd);
            }
            if (ws_cd != "" && ws_cd != "-1")
            {
                sb.Append(" and a.WS_CD = @WS_CD  ");
                ht.Add("@WS_CD", ws_cd);
            }
            if (grade_year != "")
            {
                sb.Append(" and a.GRADE_YEAR = @GRADE_YEAR  ");
                ht.Add("@GRADE_YEAR", grade_year);
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
    public int GetCount(int startRowIndex, int maximumRows, string data_year, string education_cd, string level_cd , string grade_cd, string ws_cd, string grade_year)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select COUNT(*) total_record from TB_S_M_HIRING_SALARY_TMP_D a");
            sb.Append(" left join TB_9_M_COMM_D b on a.WS_CD=b.SUB_CD and b.SYS_CD='HB' and b.MAIN_CD='WS_CD'");
            sb.Append(" left join TB_9_M_COMM_D c on a.EDUCATION_CD=c.SUB_CD and c.SYS_CD='HB' and c.MAIN_CD='EDUCATION_CD'");
            sb.Append(" where a.DATA_YEAR=@DATA_YEAR");
            ht.Add("@DATA_YEAR", data_year);
            if (education_cd != "" && education_cd != "-1")
            {
                sb.Append(" and a.EDUCATION_CD=@EDUCATION_CD  ");
                ht.Add("@EDUCATION_CD", education_cd);
            }
            if (level_cd != "")
            {
                sb.Append(" and a.LEVEL_CD like '%' + @LEVEL_CD + '%'");
                ht.Add("@LEVEL_CD", level_cd);
            }
            if (grade_cd != "")
            {
                sb.Append(" and a.GRADE_CD like '%' + @GRADE_CD + '%'");
                ht.Add("@GRADE_CD", grade_cd);
            }
            if (ws_cd != "" && ws_cd != "-1")
            {
                sb.Append(" and a.WS_CD = @WS_CD  ");
                ht.Add("@WS_CD", ws_cd);
            }
            if (grade_year != "")
            {
                sb.Append(" and a.GRADE_YEAR = @GRADE_YEAR  ");
                ht.Add("@GRADE_YEAR", grade_year);
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
    //初任薪生成
    public DataTable GetData1(int startRowIndex, int maximumRows, string sortExpression, string data_year)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY a.WS_CD,a.LEVEL_CD,a.GRADE_CD,a.EDUCATION_CD ) As RowNumber,");
            sb.Append(" a.WS_CD,a.WS_CD+'-'+b.SUB_DESC as WS_CD_NAME,a.LEVEL_CD,a.GRADE_CD,a.EDUCATION_CD+'-'+c.SUB_DESC as EDUCATION_CD_NAME,a.EDUCATION_CD");
            sb.Append(" ,a.START_SALARY,a.BASE_YEAR,a.START_YEAR,a.END_YEAR,a.BASE_RANGE,a.FEMALE_RANGE,a.ARMY_RANGE");
            sb.Append(" from TB_S_M_HIRING_SALARY_SET a");
            sb.Append(" left join TB_9_M_COMM_D b on a.WS_CD=b.SUB_CD and b.SYS_CD='HB' and b.MAIN_CD='WS_CD'");
            sb.Append(" left join TB_9_M_COMM_D c on a.EDUCATION_CD=c.SUB_CD and c.SYS_CD='HB' and c.MAIN_CD='EDUCATION_CD'");
            sb.Append(" where a.DATA_YEAR=@DATA_YEAR");
            ht.Add("@DATA_YEAR", data_year);
        
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
    public int GetCount1(int startRowIndex, int maximumRows, string data_year)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select COUNT(*) total_record from TB_S_M_HIRING_SALARY_SET a");
            sb.Append(" where a.DATA_YEAR=@DATA_YEAR");
            ht.Add("@DATA_YEAR", data_year);
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
   //初任薪試算資料生成 (產生初任薪試算資料)
    public void Data_mark(string pa_data_year)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_S_FB2SA110");
            ht.Add("@qry_date_year", pa_data_year);
            ht.Add("@qry_user_id", SessionHandle.Current.emp_id);
            dbConn.ExecuteSP(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
   
    public DataTable getExcelData(string paDATA_YEAR, string paEDUCATION_CD, string paLEVEL_CD, string paGRADE_CD, string paWS_CD, string paGRADE_YEAR)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select a.DATA_YEAR,a.WS_CD, ");
            sb.Append(" a.LEVEL_CD,a.GRADE_CD,c.SUB_DESC as EDUCATION_CD_NAME");
            sb.Append(" ,a.GRADE_YEAR,a.LEVEL_PAY1,a.LEVEL_PAY2,a.LEVEL_PAY3");
            sb.Append(" from TB_S_M_HIRING_SALARY_TMP_D a");
            sb.Append(" left join TB_9_M_COMM_D b on a.WS_CD=b.SUB_CD and b.SYS_CD='HB' and b.MAIN_CD='WS_CD'");
            sb.Append(" left join TB_9_M_COMM_D c on a.EDUCATION_CD=c.SUB_CD and c.SYS_CD='HB' and c.MAIN_CD='EDUCATION_CD'");
            sb.Append(" where a.DATA_YEAR=@DATA_YEAR");
            ht.Add("@DATA_YEAR", paDATA_YEAR);
            if (paEDUCATION_CD != "" && paEDUCATION_CD != "-1")
            {
                sb.Append(" and a.EDUCATION_CD=@EDUCATION_CD  ");
                ht.Add("@EDUCATION_CD", paEDUCATION_CD);
            }
            if (paLEVEL_CD != "")
            {
                sb.Append(" and a.LEVEL_CD like '%' + @LEVEL_CD + '%'");
                ht.Add("@LEVEL_CD", paLEVEL_CD);
            }
            if (paGRADE_CD != "")
            {
                sb.Append(" and a.GRADE_CD like '%' + @GRADE_CD + '%'");
                ht.Add("@GRADE_CD", paGRADE_CD);
            }
            if (paWS_CD != "" && paWS_CD != "-1")
            {
                sb.Append(" and a.WS_CD = @WS_CD  ");
                ht.Add("@WS_CD", paWS_CD);
            }
            if (paGRADE_YEAR != "")
            {
                sb.Append(" and a.GRADE_YEAR = @GRADE_YEAR  ");
                ht.Add("@GRADE_YEAR", paGRADE_YEAR);
            }
            //sb.Append(" ) as z ");
            return dbConn.Query(sb, ht);

        }
        catch
        {
            throw;
        }
    }

    //回傳[TB_S_M_HIRING_SALARY_SET] 指定查尋年度的資料筆數
    public void getTB_S_HIRING_SALARY_SET_CONUT(string paDATA_YEAR)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select count(0) as resultCount from TB_S_M_HIRING_SALARY_SET");
            sb.Append(" where DATA_YEAR=@pa_data_year ");
            ht.Add("@pa_data_year", paDATA_YEAR);

            DataTable dtt = dbConn.Query(sb, ht);

            if (dtt.Rows.Count >0)
            {
                if ((int)dtt.Rows[0]["resultCount"] == 0)
                {
                    sb.Append("INSERT INTO TB_S_M_HIRING_SALARY_SET( DATA_YEAR,WS_CD,LEVEL_CD,GRADE_CD,EDUCATION_CD,CREATED_BY,CREATED_DT,FUNC_ID ) ");
                    sb.Append(" SELECT @data_tear ,WS_CD,LEVEL_CD,GRADE_CD,EDUCATION_CD,@CREATED_BY,GETDATE(),'FB2SA110' from TB_S_M_SALARY_LEVEL ");
                    ht.Add("@data_tear", paDATA_YEAR);
                    ht.Add("@created_by", SessionHandle.Current.emp_id);
                    dbConn.ExecuteT(sb, ht, true);
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    //檢查初任薪生成是否已生效,若已生效不允重新計算
    internal DataTable getS_M_HRING_TEMP_H(string pa_data_year)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select count(0) resultCount from TB_S_M_HIRING_SALARY_TMP_H");
            sb.Append(" where DATA_YEAR=@pa_data_year and PROCESS_STATUS='Y'");
            ht.Add("@pa_data_year", pa_data_year);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    
    //刪除[TB_S_M_HIRING_SALARY_SET] 指定查尋年度的資料
    public void DeleteData(string pa_data_year)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("DELETE from TB_S_M_HIRING_SALARY_SET");
            sb.Append(" where DATA_YEAR=@pa_data_year ");
            ht.Add("@pa_data_year", pa_data_year);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //新增 TB_S_M_HIRING_SALARY_SET (資料生成鈕使用)
    internal void insertData(string pa_DATA_YEAR, string pa_WS_CD, string pa_LEVEL_CD, string pa_GRADE_CD, string pa_EDUCATION_CD, string pa_START_SALARY, string pa_BASE_YEAR, string pa_START_YEAR, string pa_END_YEAR, string pa_BASE_RANGE, string pa_FEMALE_RANGE, string pa_ARMY_RANGE)
    {
        try
        {
            DateTime now = DateTime.Parse(DateTime.Now.ToString());
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" INSERT INTO TB_S_M_HIRING_SALARY_SET ");
            sb.Append(" ( ");
            sb.Append(" DATA_YEAR,WS_CD,LEVEL_CD,GRADE_CD,EDUCATION_CD,START_SALARY,BASE_YEAR,START_YEAR,END_YEAR ");
            sb.Append(" ,BASE_RANGE,FEMALE_RANGE,ARMY_RANGE,APPROVE_MARK ");
            sb.Append(" ,CREATED_BY,CREATED_DT,FUNC_ID  ");
            sb.Append(" ) ");
            sb.Append(" VALUES ");
            sb.Append(" (@pa_DATA_YEAR,@pa_WS_CD,@pa_LEVEL_CD,@pa_GRADE_CD,@pa_EDUCATION_CD,@pa_START_SALARY,@pa_BASE_YEAR,@pa_START_YEAR,@pa_END_YEAR ");
            sb.Append(" ,@pa_BASE_RANGE,@pa_FEMALE_RANGE,@pa_ARMY_RANGE,'' ");
            sb.Append("  ,@CREATED_BY, GETDATE(),  @FUNC_ID )");

            ht.Add("@pa_DATA_YEAR", pa_DATA_YEAR);
            ht.Add("@pa_WS_CD", pa_WS_CD);
            ht.Add("@pa_LEVEL_CD", pa_LEVEL_CD);
            ht.Add("@pa_GRADE_CD", pa_GRADE_CD);
            ht.Add("@pa_EDUCATION_CD", pa_EDUCATION_CD);
            ht.Add("@pa_START_SALARY", pa_START_SALARY);
            ht.Add("@pa_BASE_YEAR", pa_BASE_YEAR);
            ht.Add("@pa_START_YEAR", pa_START_YEAR);
            ht.Add("@pa_END_YEAR", pa_END_YEAR);
            ht.Add("@pa_BASE_RANGE", pa_BASE_RANGE);
            ht.Add("@pa_FEMALE_RANGE", pa_FEMALE_RANGE);
            ht.Add("@pa_ARMY_RANGE", pa_ARMY_RANGE);
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SA110");
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
}