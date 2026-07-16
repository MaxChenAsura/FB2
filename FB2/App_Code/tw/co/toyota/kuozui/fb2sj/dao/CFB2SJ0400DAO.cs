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
/// CFB2SJ040DAO 的摘要描述
/// </summary>
public class CFB2SJ0400DAO : BaseDAO
{
    //SJ040基本欄位
    public string ASSESS_YEAR { get; set; }
    public string ASSESS_TYPE { get; set; }
    public string EMP_ID { get; set; }
    public string REMARK { get; set; }

    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }


    public CFB2SJ0400DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }


    #region 資料取得
    //取得 考核類別
    public int checkAsessType(string sub_cd)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" select count(*) resultCount from TB_9_M_COMM_D B  
                        where  MAIN_CD = 'ASSESS_TYPE'  and IS_VALID='Y'  and SYS_CD='SJ'
                        and SUB_CD=@SUB_CD
                        ");
            ht.Add("@SUB_CD", sub_cd);
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

    //確認工號存在
    public int checkEMPID(string year,string type,string empId)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" select count(*) resultCount from TB_S_M_ASSESS_TARGET
                        where 1=1
                        and ASSESS_YEAR = @ASSESS_YEAR
                        and ASSESS_TYPE = @ASSESS_TYPE
                        and EMP_ID =@EMP_ID
                        ");
            ht.Add("@ASSESS_YEAR", year);
            ht.Add("@ASSESS_TYPE", type);
            ht.Add("@EMP_ID", empId);
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

    
    #endregion

    #region Gridview 資料
    //Gridview 查詢資料
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression
                            , string assess_year_s, string assess_year_e, string assess_type, string emp_id, string emp_name, string dept_no
                           )
    {
        try
        {
            if (sortExpression.Contains("EMP_ID"))
            {
                sortExpression = sortExpression.Replace("EMP_ID", "a.EMP_ID");
            }
            if (sortExpression.Contains("ASSESS_YEAR"))
            {
                sortExpression = sortExpression.Replace("ASSESS_YEAR", "a.ASSESS_YEAR");
            }
            if (sortExpression.Contains("ASSESS_TYPE"))
            {
                sortExpression = sortExpression.Replace("ASSESS_TYPE", "a.ASSESS_TYPE");
            }
            if (sortExpression.Contains("REMARK"))
            {
                sortExpression = sortExpression.Replace("REMARK", "a.REMARK");
            }
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + ") As RowNumber, ");
            sb.Append(@"  A.ASSESS_YEAR,A.ASSESS_TYPE  
                        , a.ASSESS_TYPE + '-' + b.SUB_DESC ASSESS_TYPE_DESC
                        , A.EMP_ID
                        , C.EMP_NAME
                        , C.DEPT_NO
                        , C.DEPT_NO +' '
                        + C.DEPT_NAME_20+' ' 
                        + C.DEPT_NAME_30+' '  
                        + C.DEPT_NAME_40+' '
                        + C.DEPT_NAME_50+' '
                        + C.DEPT_NAME_60+' '
                        + C.DEPT_NAME_70  as DEPT_NO_DESC
                        , A.REMARK
                        from TB_S_M_ASSESS_REMARK A
                        left join TB_9_M_COMM_D B on  A.ASSESS_TYPE = B.SUB_CD and B.MAIN_CD = 'ASSESS_TYPE'  and B.IS_VALID='Y'  and B.SYS_CD='SJ'
                        left join TB_S_M_ASSESS_TARGET C on A.ASSESS_YEAR = C.ASSESS_YEAR and A.ASSESS_TYPE = C.ASSESS_TYPE and A.EMP_ID = C.EMP_ID ");
            sb.Append(" where 1=1 ");
            //查詢條件
            if (assess_year_s != "")
            {
                sb.Append(" and A.ASSESS_YEAR >= @ASSESS_YEAR_S ");
                ht.Add("@ASSESS_YEAR_S", assess_year_s);
            }
            if (assess_year_e != "")
            {
                sb.Append(" and A.ASSESS_YEAR <= @ASSESS_YEAR_E ");
                ht.Add("@ASSESS_YEAR_E", assess_year_e);
            }
            if (assess_type != "-1")
            {
                sb.Append(" and A.ASSESS_TYPE = @ASSESS_TYPE ");
                ht.Add("@ASSESS_TYPE", assess_type);
            }
            if (emp_id != "")
            {
                sb.Append(" and A.EMP_ID like @EMP_ID ");
                ht.Add("@EMP_ID", emp_id + "%");
            }
            if (emp_name != "")
            {
                sb.Append(" and EMP_NAME like @EMP_NAME ");
                ht.Add("@EMP_NAME", emp_name + "%");
            }
            if (dept_no != "")
            {
                sb.Append(" and DEPT_NO like @DEPT_NO ");
                ht.Add("@DEPT_NO", dept_no + "%");
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
    public int getCount(int startRowIndex, int maximumRows
                        , string assess_year_s, string assess_year_e, string assess_type, string emp_id, string emp_name, string dept_no)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) resultCount ");
            sb.Append(@" from TB_S_M_ASSESS_REMARK A
                        left join TB_9_M_COMM_D B on  A.ASSESS_TYPE = B.SUB_CD and B.MAIN_CD = 'ASSESS_TYPE'  and B.IS_VALID='Y'  and B.SYS_CD='SJ'
                        left join TB_S_M_ASSESS_TARGET C on A.ASSESS_YEAR = C.ASSESS_YEAR and A.ASSESS_TYPE = C.ASSESS_TYPE and A.EMP_ID = C.EMP_ID ");
            sb.Append(" where 1=1 ");
            //查詢條件
            if (assess_year_s != "")
            {
                sb.Append(" and A.ASSESS_YEAR >= @ASSESS_YEAR_S ");
                ht.Add("@ASSESS_YEAR_S", assess_year_s);
            }
            if (assess_year_e != "")
            {
                sb.Append(" and A.ASSESS_YEAR <= @ASSESS_YEAR_E ");
                ht.Add("@ASSESS_YEAR_E", assess_year_e);
            }
            if (assess_type != "-1")
            {
                sb.Append(" and A.ASSESS_TYPE = @ASSESS_TYPE ");
                ht.Add("@ASSESS_TYPE", assess_type);
            }
            if (emp_id != "")
            {
                sb.Append(" and A.EMP_ID like @EMP_ID ");
                ht.Add("@EMP_ID", emp_id + "%");
            }
            if (emp_name != "")
            {
                sb.Append(" and EMP_NAME like @EMP_NAME ");
                ht.Add("@EMP_NAME", emp_name + "%");
            }
            if (dept_no != "")
            {
                sb.Append(" and DEPT_NO like @DEPT_NO ");
                ht.Add("@DEPT_NO", dept_no + "%");
            }
       

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

    #endregion


    #region DB存取
    //刪除 
    public void deleteData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" delete from TB_S_M_ASSESS_REMARK 
                        where ASSESS_YEAR = @ASSESS_YEAR 
                        and ASSESS_TYPE = @ASSESS_TYPE
                        and EMP_ID = @EMP_ID
                        ");
            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@EMP_ID", EMP_ID);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //刪除 全部
    public void deleteAllData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" delete from TB_S_M_ASSESS_REMARK 
                        where ASSESS_YEAR = @ASSESS_YEAR 
                        and ASSESS_TYPE = @ASSESS_TYPE
                        ");
            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //修改
    public void updateData() {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            DateTime now = DateTime.Parse(DateTime.Now.ToString());
            sb.Append(@" update TB_S_M_ASSESS_REMARK
                        set REMARK =@REMARK
                        where ASSESS_YEAR = @ASSESS_YEAR 
                        and ASSESS_TYPE = @ASSESS_TYPE
                        and EMP_ID = @EMP_ID
                    ");
            
            //修改值
            ht.Add("@REMARK", REMARK);

            //PK值
            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@EMP_ID", EMP_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }

    
    }

    //新增
    internal void insertData(DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" INSERT INTO TB_S_M_ASSESS_REMARK
                         (ASSESS_YEAR,ASSESS_TYPE,EMP_ID,REMARK
                          ,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID
                         )
                        VALUES
                        (@ASSESS_YEAR,@ASSESS_TYPE,@EMP_ID,@REMARK
                          ,@CREATED_BY,@CREATED_DT,@UPDATED_BY,@UPDATED_DT,@FUNC_ID
                         )
                    ");

            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@REMARK", REMARK);

            ht.Add("@CREATED_BY", UPDATED_BY);
            ht.Add("@CREATED_DT", now);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@UPDATED_DT", now);
            ht.Add("@FUNC_ID", FUNC_ID);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion



}