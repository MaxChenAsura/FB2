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
/// CFB2DJ010DAO 的摘要描述
/// </summary>
public class CFB2DJ0300DAO : BaseDAO
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


    public CFB2DJ0300DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }


    #region 資料取得


    //依PK值取得資料，
    internal DataTable getPKData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select ENV_CHECK_STATUS  from TB_D_M_ENV_ALLOWANCE_APPLY ");
            sb.Append(" where APPLY_DT=@APPLY_DT");
            sb.Append(" and ENV_ALLOWANCE_TYPE = @ENV_ALLOWANCE_TYPE");
            sb.Append(" and EMP_ID = @EMP_ID");
            ht.Add("@ENV_ALLOWANCE_TYPE", ENV_ALLOWANCE_TYPE);
            ht.Add("@APPLY_DT", Convert.ToDateTime(APPLY_DT));
            ht.Add("@EMP_ID", EMP_ID);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //取得在某日期(生效日期)是否已有效的資料，
    internal DataTable getValidEnvData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select count(0) typecount from TB_D_M_ENV_ALLOWANCE_TYPE");
            sb.Append(" where ENV_ALLOWANCE_TYPE=@ENV_ALLOWANCE_TYPE");
            sb.Append(" and START_DT <= @APPLY_DT");
            sb.Append(" and END_DT   >= @APPLY_DT");
            //ht.Add("@ENV_ALLOWANCE_TYPE", ENV_ALLOWANCE_TYPE);
            //ht.Add("@APPLY_DT", Convert.ToDateTime(START_DT));
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
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression
                          , string appleDT_S, string appleDT_E, string empID, string empName, string deptNO, string checkStatus, string salaryStatus, string iflowNO)
    {
        try
        {

            //if (sortExpression.Contains("ENV_ALLOWANCE_TYPE"))
            //    sortExpression = sortExpression.Replace("ENV_ALLOWANCE_TYPE", "a.ENV_ALLOWANCE_TYPE");

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + ") As RowNumber,");
            sb.Append(" DEPT_NAME, LAYOUT_NO, EMP_ID, EMP_NAME, DEPT_NO   ");
            sb.Append(" , WORK_SHIFT_CD, ENV_ALLOWANCE_TYPE, APPLY_DT, IFLOW_NO, SALARY_DT   ");
            sb.Append(" , ENV_APPLY_CD, ENV_CHECK_STATUS, ENV_CHECK_LOG, ENV_SALARY_STATUS   ");//為排序而加的
            sb.Append(" , ENV_APPLY_CD + '-' + b.SUB_DESC ENV_APPLY_CD_DESC   ");
            sb.Append(" , ENV_CHECK_STATUS + '-' + c.SUB_DESC ENV_CHECK_STATUS_DESC   ");
            sb.Append(" , ENV_CHECK_LOG + '-' + d.SUB_DESC ENV_CHECK_LOG_DESC   ");
            sb.Append(" , ENV_SALARY_STATUS + '-' + e.SUB_DESC ENV_SALARY_STATUS_DESC   ");
            sb.Append(" from TB_D_M_ENV_ALLOWANCE_APPLY a ");
            sb.Append("  left join TB_9_M_COMM_D b on  a.ENV_APPLY_CD = b.SUB_CD and b.MAIN_CD = 'ENV_APPLY_CD'  and b.IS_VALID='Y'  and b.SYS_CD='DJ'  ");
            sb.Append("  left join TB_9_M_COMM_D c on  a.ENV_CHECK_STATUS = c.SUB_CD and c.MAIN_CD = 'ENV_CHECK_STATUS'  and c.IS_VALID='Y'  and  c.SYS_CD='DJ'  ");
            sb.Append("  left join TB_9_M_COMM_D d on  a.ENV_CHECK_LOG = d.SUB_CD  and d.MAIN_CD = 'ENV_CHECK_LOG'  and d.IS_VALID='Y'  and d.SYS_CD='DJ'   ");
            sb.Append("  left join TB_9_M_COMM_D e on  a.ENV_SALARY_STATUS = e.SUB_CD  and e.MAIN_CD = 'ENV_SALARY_STATUS'  and e.IS_VALID='Y'  and e.SYS_CD='DJ'   ");
            sb.Append(" where 1=1 ");

            //查詢條件-津貼期間
            if (appleDT_S != "")
            {
                sb.Append(" and APPLY_DT >=  @APPLY_DT_S ");
                ht.Add("@APPLY_DT_S", Convert.ToDateTime(appleDT_S).ToString("yyyy/MM/dd"));
            }
            if (appleDT_S != "")
            {
                sb.Append(" and APPLY_DT <=  @APPLY_DT_E ");
                ht.Add("@APPLY_DT_E", Convert.ToDateTime(appleDT_E).ToString("yyyy/MM/dd"));
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
            if (checkStatus != "-1")
            {
                sb.Append(" and ENV_CHECK_STATUS = @ENV_CHECK_STATUS ");
                ht.Add("@ENV_CHECK_STATUS", checkStatus);
            }

            //查詢條件-計薪狀態
            if (salaryStatus != "-1")
            {
                sb.Append(" and ENV_SALARY_STATUS = @ENV_SALARY_STATUS ");
                ht.Add("@ENV_SALARY_STATUS", salaryStatus);
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
                       , string appleDT_S, string appleDT_E, string empID, string empName, string deptNO, string checkStatus, string salaryStatus, string iflowNO)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) total_record ");
            sb.Append(" from TB_D_M_ENV_ALLOWANCE_APPLY ");
            sb.Append(" where 1=1 ");


            //查詢條件-津貼期間
            if (appleDT_S != "")
            {
                sb.Append(" and APPLY_DT >=  @APPLY_DT_S ");
                ht.Add("@APPLY_DT_S", Convert.ToDateTime(appleDT_S).ToString("yyyy/MM/dd"));
            }
            if (appleDT_S != "")
            {
                sb.Append(" and APPLY_DT <=  @APPLY_DT_E ");
                ht.Add("@APPLY_DT_E", Convert.ToDateTime(appleDT_E).ToString("yyyy/MM/dd"));
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
            if (checkStatus != "-1")
            {
                sb.Append(" and ENV_CHECK_STATUS = @ENV_CHECK_STATUS ");
                ht.Add("@ENV_CHECK_STATUS", checkStatus);
            }

            //查詢條件-計薪狀態
            if (salaryStatus != "-1")
            {
                sb.Append(" and ENV_SALARY_STATUS = @ENV_SALARY_STATUS ");
                ht.Add("@ENV_SALARY_STATUS", salaryStatus);
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
    public void deleteData(string apply_dt, string evnType, string enpID)
    {
        try
        {
            // string dt = Convert.ToDateTime(start_dt).ToString("yyyy/MM/dd HH:mm:ss.fff");
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("delete from TB_D_M_ENV_ALLOWANCE_APPLY ");
            //sb.Append(" where ENV_ALLOWANCE_TYPE = @ENV_ALLOWANCE_TYPE and Convert(char(10),START_DT,111) = @START_DT ");
            sb.Append(" where APPLY_DT = @APPLY_DT ");
            sb.Append(" and ENV_ALLOWANCE_TYPE = @ENV_ALLOWANCE_TYPE ");
            sb.Append(" and EMP_ID = @EMP_ID "); 
            ht.Add("@APPLY_DT", Convert.ToDateTime(apply_dt).ToString("yyyy/MM/dd"));
            ht.Add("@ENV_ALLOWANCE_TYPE", evnType);
            ht.Add("@EMP_ID", enpID);
            //ht.Add("@START_DT", start_dt);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //更新比對狀態為加扣項(I)
    public void updateData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            DateTime now = DateTime.Parse(DateTime.Now.ToString());
            sb.Append(" update TB_D_M_ENV_ALLOWANCE_APPLY ");
            sb.Append(" set ENV_SALARY_STATUS = @ENV_SALARY_STATUS ");
            sb.Append(" where APPLY_DT = @APPLY_DT ");
            sb.Append(" and ENV_ALLOWANCE_TYPE = @ENV_ALLOWANCE_TYPE ");
            sb.Append(" and EMP_ID = @EMP_ID ");
            ht.Add("@ENV_SALARY_STATUS", "I");
            ht.Add("@APPLY_DT", Convert.ToDateTime(APPLY_DT).ToString("yyyy/MM/dd"));
            ht.Add("@ENV_ALLOWANCE_TYPE", ENV_ALLOWANCE_TYPE);
            ht.Add("@EMP_ID", EMP_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }


    }

    #endregion



}