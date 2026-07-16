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
/// CFB2DJ040DAO 的摘要描述
/// </summary>
public class CFB2DJ0400DAO : BaseDAO
{
    //dj030基本欄位
    public string APPLY_DT { get; set; }
    public string ENV_ALLOWANCE_TYPE { get; set; }
    public string EMP_ID { get; set; }
    public string EMP_NAME { get; set; }
    public string DEPT_NAME { get; set; }
    public string DEPT_NO { get; set; }
    public string LAYOUT_NO { get; set; }
    public string WORK_SHIFT_CD { get; set; }
    public string ENV_APPLY_CD { get; set; }
    public string APPLY_HOUR { get; set; }
    public string IFLOW_NO { get; set; }
    public string IFLOW_APPROVE_DT { get; set; }
    public string ENV_CHECK_STATUS { get; set; }
    public string ENV_CHECK_LOG { get; set; }
    public string ENV_ALLOWANCE_VALUE { get; set; }
    public string ENV_APPLY_AMT { get; set; }
    public string ENV_SALARY_STATUS { get; set; }
    public string SALARY_DT { get; set; }
    public string CLOSED_BY { get; set; }
    public string CLOSED_DT { get; set; }

    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }



    //dj010
    /*
    public string ENV_ALLOWANCE_TYPE { get; set; }
    public string START_DT { get; set; }
    public string END_DT { get; set; }
    public string ENV_ALLOWANCE_DESC { get; set; }
    public string ENV_ALLOWANCE_VALUE { get; set; }
    public string ENV_MIN_UNIT { get; set; }
    public string REMARK { get; set; }
   */


    public CFB2DJ0400DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }


    #region 資料取得
    //取得是否為主管
    internal DataTable getMNGData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select count(0) mngCount from TB_H_R_HEAD_DEPT ");
            sb.Append(" where EMP_ID=@EMP_ID");
            ht.Add("@EMP_ID", SessionHandle.Current.emp_id);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }


    #endregion

    #region Gridview 資料
    //Gridview 查詢資料
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string isSuper
                         , string appleDT_S, string appleDT_E, string empID, string empName, string deptNO
                         , string checkStatus, string salaryStatus, string iflowNO, string salaryDT, string layout_no)
    {
        try
        {

            if (sortExpression.Contains("EMP_ID"))
                sortExpression = sortExpression.Replace("EMP_ID", "a.EMP_ID");
            if (sortExpression.Contains("EMP_NAME"))
                sortExpression = sortExpression.Replace("EMP_NAME", "a.EMP_NAME");

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + ") As RowNumber,");
            sb.Append(" DEPT_NAME, LAYOUT_NO, a.EMP_ID, a.EMP_NAME, DEPT_NO   ");
            sb.Append(" , WORK_SHIFT_CD, ENV_ALLOWANCE_TYPE, APPLY_DT, IFLOW_NO, SALARY_DT   ");
            sb.Append(" , ENV_APPLY_CD, ENV_CHECK_STATUS, ENV_CHECK_LOG, ENV_SALARY_STATUS   ");//為排序而加的
            sb.Append(" , ENV_APPLY_CD + '-' + b.SUB_DESC ENV_APPLY_CD_DESC   ");
            sb.Append(" , ENV_CHECK_STATUS + '-' + c.SUB_DESC ENV_CHECK_STATUS_DESC   ");
            sb.Append(" , ENV_CHECK_LOG + '-' + d.SUB_DESC ENV_CHECK_LOG_DESC   ");
            sb.Append(" , ENV_SALARY_STATUS + '-' + e.SUB_DESC ENV_SALARY_STATUS_DESC   ");
            sb.Append(" , isnull(V.EMP_NAME,'') CREATED_NAME  ");
            sb.Append(" from TB_D_M_ENV_ALLOWANCE_APPLY a ");
            sb.Append("  left join TB_9_M_COMM_D b on  a.ENV_APPLY_CD = b.SUB_CD and b.MAIN_CD = 'ENV_APPLY_CD'  and b.IS_VALID='Y'  and b.SYS_CD='DJ'   ");
            sb.Append("  left join TB_9_M_COMM_D c on  a.ENV_CHECK_STATUS = c.SUB_CD and c.MAIN_CD = 'ENV_CHECK_STATUS'  and c.IS_VALID='Y'  and c.SYS_CD='DJ'  ");
            sb.Append("  left join TB_9_M_COMM_D d on  a.ENV_CHECK_LOG = d.SUB_CD  and d.MAIN_CD = 'ENV_CHECK_LOG'  and d.IS_VALID='Y'  and  d.SYS_CD='DJ'   ");
            sb.Append("  left join TB_9_M_COMM_D e on  a.ENV_SALARY_STATUS = e.SUB_CD  and e.MAIN_CD = 'ENV_SALARY_STATUS'  and e.IS_VALID='Y'  and e.SYS_CD='DJ'   ");
            sb.Append("  left join (select EMP_ID, EMP_NAME from   TB_H_M_EMP ) V on a.CREATED_BY=V.EMP_ID  ");
            sb.Append(" where 1=1 ");

            //顯示資料權限設定
            if (SessionHandle.Current.is_super != "Y")
            {
                sb.Append(@" AND a.EMP_ID IN(	 select EMP_ID from dbo.FN_H_GET_AUTH_EMP(@loginID,@departments)  )");
                ht.Add("@loginID", SessionHandle.Current.emp_id);
                ht.Add("@departments", SessionHandle.Current.departments);
            }

            //查詢條件-津貼期間
            if (appleDT_S != "")
            {
                sb.Append(" and APPLY_DT >=  @APPLY_DT_S ");
                ht.Add("@APPLY_DT_S", Convert.ToDateTime(appleDT_S).ToString("yyyy/MM/dd"));
            }
            if (appleDT_E != "")
            {
                sb.Append(" and APPLY_DT <=  @APPLY_DT_E ");
                ht.Add("@APPLY_DT_E", Convert.ToDateTime(appleDT_E).ToString("yyyy/MM/dd"));
            }
            //查詢條件-發薪日期
            if (salaryDT != "")
            {
                sb.Append(" and SALARY_DT =  @SALARY_DT ");
                ht.Add("@SALARY_DT", Convert.ToDateTime(salaryDT).ToString("yyyy/MM/dd"));
            }

            //查詢條件-工號
            if (empID != "")
            {
                sb.Append(" and a.EMP_ID like @EMP_ID ");
                ht.Add("@EMP_ID", empID + "%");
            }

            //查詢條件-姓名
            if (empName != "")
            {
                sb.Append(" and a.EMP_NAME like @EMP_NAME ");
                ht.Add("@EMP_NAME", empName + "%");
            }
            //查詢條件-部門代號
            if (deptNO != "")
            {
                sb.Append(" and DEPT_NO like @DEPT_NO ");
                ht.Add("@DEPT_NO", deptNO + "%");
            }
            //查詢條件-表單編號
            if (iflowNO != "")
            {
                sb.Append(" and IFLOW_NO like @IFLOW_NO ");
                ht.Add("@IFLOW_NO", iflowNO + "%");
            }
            //查詢條件-比對狀態
            if (checkStatus!="" && checkStatus != "-1")
            {
                sb.Append(" and ENV_CHECK_STATUS = @ENV_CHECK_STATUS ");
                ht.Add("@ENV_CHECK_STATUS", checkStatus);
            }

            //查詢條件-計薪狀態
            if (salaryStatus!="" && salaryStatus != "-1")
            {
                sb.Append(" and ENV_SALARY_STATUS = @ENV_SALARY_STATUS ");
                ht.Add("@ENV_SALARY_STATUS", salaryStatus);
            }
            //查詢條件-Laytout_No
            if (layout_no != "")
            {
                sb.Append(" and LAYOUT_NO like @LAYOUT_NO ");
                ht.Add("@LAYOUT_NO", layout_no + "%");
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
    public int getCount(int startRowIndex, int maximumRows, string isSuper
                          , string appleDT_S, string appleDT_E, string empID, string empName, string deptNO
                          , string checkStatus, string salaryStatus, string iflowNO, string salaryDT, string layout_no)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) total_record ");
            sb.Append(" from TB_D_M_ENV_ALLOWANCE_APPLY  ");
            sb.Append(" where 1=1 ");

            //顯示資料權限設定
            if (SessionHandle.Current.is_super != "Y")
            {
                sb.Append(@" AND EMP_ID IN(	 select EMP_ID from dbo.FN_H_GET_AUTH_EMP(@loginID,@departments)  )");
                ht.Add("@loginID", SessionHandle.Current.emp_id);
                ht.Add("@departments", SessionHandle.Current.departments);
            }

            //查詢條件-津貼期間
            if (appleDT_S != "")
            {
                sb.Append(" and APPLY_DT >=  @APPLY_DT_S ");
                ht.Add("@APPLY_DT_S", Convert.ToDateTime(appleDT_S).ToString("yyyy/MM/dd"));
            }
            if (appleDT_E != "")
            {
                sb.Append(" and APPLY_DT <=  @APPLY_DT_E ");
                ht.Add("@APPLY_DT_E", Convert.ToDateTime(appleDT_E).ToString("yyyy/MM/dd"));
            }
            //查詢條件-發薪日期
            if (salaryDT != "")
            {
                sb.Append(" and SALARY_DT =  @SALARY_DT ");
                ht.Add("@SALARY_DT", Convert.ToDateTime(salaryDT).ToString("yyyy/MM/dd"));
            }

            //查詢條件-工號
            if (empID != "")
            {
                sb.Append(" and EMP_ID like @EMP_ID ");
                ht.Add("@EMP_ID", empID + "%");
            }

            //查詢條件-姓名
            if (empName != "")
            {
                sb.Append(" and EMP_NAME like @EMP_NAME ");
                ht.Add("@EMP_NAME", empName + "%");
            }
            //查詢條件-部門代號
            if (deptNO != "")
            {
                sb.Append(" and DEPT_NO like @DEPT_NO ");
                ht.Add("@DEPT_NO", deptNO + "%");
            }
            //查詢條件-表單編號
            if (iflowNO != "")
            {
                sb.Append(" and IFLOW_NO like @IFLOW_NO ");
                ht.Add("@IFLOW_NO", iflowNO + "%");
            }
            //查詢條件-比對狀態
            if (checkStatus != "" && checkStatus != "-1")
            {
                sb.Append(" and ENV_CHECK_STATUS = @ENV_CHECK_STATUS ");
                ht.Add("@ENV_CHECK_STATUS", checkStatus);
            }

            //查詢條件-計薪狀態
            if (salaryStatus != "" && salaryStatus != "-1")
            {
                sb.Append(" and ENV_SALARY_STATUS = @ENV_SALARY_STATUS ");
                ht.Add("@ENV_SALARY_STATUS", salaryStatus);
            }

            //查詢條件-Laytout_No
            if (layout_no != "")
            {
                sb.Append(" and LAYOUT_NO like @LAYOUT_NO ");
                ht.Add("@LAYOUT_NO", layout_no + "%");
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

    #endregion


    #region DB存取
    //刪除 
    public void deleteData(string type, string start_dt)
    {
        try
        {
            // string dt = Convert.ToDateTime(start_dt).ToString("yyyy/MM/dd HH:mm:ss.fff");
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("delete from TB_D_M_ENV_ALLOWANCE_TYPE ");
            //sb.Append(" where ENV_ALLOWANCE_TYPE = @ENV_ALLOWANCE_TYPE and Convert(char(10),START_DT,111) = @START_DT ");
            sb.Append(" where ENV_ALLOWANCE_TYPE = @ENV_ALLOWANCE_TYPE and START_DT = @START_DT ");
            ht.Add("@ENV_ALLOWANCE_TYPE", type);
            ht.Add("@START_DT", Convert.ToDateTime(start_dt).ToString("yyyy/MM/dd"));
            //ht.Add("@START_DT", start_dt);
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
            sb.Append(" update TB_D_M_ENV_ALLOWANCE_TYPE ");
            sb.Append(" set END_DT = @END_DT ");
            sb.Append(" ,ENV_ALLOWANCE_DESC = @ENV_ALLOWANCE_DESC ");
            sb.Append(" ,ENV_ALLOWANCE_VALUE = @ENV_ALLOWANCE_VALUE ");
            sb.Append(" ,ENV_MIN_UNIT = @ENV_MIN_UNIT ");
            sb.Append(" ,REMARK = @REMARK ");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT = @UPDATED_DT");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where ENV_ALLOWANCE_TYPE = @ENV_ALLOWANCE_TYPE and START_DT = @START_DT");

            //ht.Add("@END_DT", END_DT + " 23:59:59");
            //ht.Add("@ENV_ALLOWANCE_DESC", ENV_ALLOWANCE_DESC);
            //ht.Add("@ENV_ALLOWANCE_VALUE", ENV_ALLOWANCE_VALUE);
            //ht.Add("@ENV_MIN_UNIT", ENV_MIN_UNIT);
            //ht.Add("@REMARK", REMARK);
            //ht.Add("@UPDATED_BY", UPDATED_BY);
            //ht.Add("@UPDATED_DT", now);
            //ht.Add("@FUNC_ID", FUNC_ID);
            //ht.Add("@ENV_ALLOWANCE_TYPE", ENV_ALLOWANCE_TYPE);
            //ht.Add("@START_DT", Convert.ToDateTime(START_DT));

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }

    
    }

    //新增
    internal void insertData()
    {
        try
        {
            DateTime now = DateTime.Parse(DateTime.Now.ToString());
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" INSERT INTO TB_D_M_ENV_ALLOWANCE_TYPE ");
            sb.Append(" ( ");
            sb.Append(" ENV_ALLOWANCE_TYPE, START_DT, END_DT, ENV_ALLOWANCE_DESC, ENV_ALLOWANCE_VALUE, ENV_MIN_UNIT,REMARK ");
            sb.Append(" ,CREATED_BY, CREATED_DT, UPDATED_BY, UPDATED_DT, FUNC_ID  ");
            sb.Append(" ) ");
            sb.Append(" VALUES ");
            sb.Append(" ( @ENV_ALLOWANCE_TYPE,  @START_DT,  @END_DT,  @ENV_ALLOWANCE_DESC,  @ENV_ALLOWANCE_VALUE,  @ENV_MIN_UNIT, @REMARK  ");

            sb.Append("   ,@CREATED_BY,  @CREATED_DT,  @UPDATED_BY,  @UPDATED_DT,  @FUNC_ID )");

            //ht.Add("@ENV_ALLOWANCE_TYPE", ENV_ALLOWANCE_TYPE);
            //ht.Add("@START_DT", Convert.ToDateTime(START_DT));
            //ht.Add("@END_DT", Convert.ToDateTime(END_DT));
            //ht.Add("@ENV_ALLOWANCE_DESC", ENV_ALLOWANCE_DESC);
            //ht.Add("@ENV_ALLOWANCE_VALUE", ENV_ALLOWANCE_VALUE);
            //ht.Add("@ENV_MIN_UNIT", ENV_MIN_UNIT);
            //ht.Add("@REMARK", REMARK);
            //ht.Add("@CREATED_BY", UPDATED_BY);
            //ht.Add("@CREATED_DT", now);
            //ht.Add("@UPDATED_BY", UPDATED_BY);
            //ht.Add("@UPDATED_DT", now);
            //ht.Add("@FUNC_ID", FUNC_ID);
            //ht.Add("@ENV_ALLOWANCE_TYPE", ENV_ALLOWANCE_TYPE);
            //ht.Add("@START_DT", Convert.ToDateTime(START_DT));

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion



}