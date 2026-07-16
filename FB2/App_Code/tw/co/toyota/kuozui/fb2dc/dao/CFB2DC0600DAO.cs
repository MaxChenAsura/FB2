using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;

/// <summary>
/// CFB2DC0600DAO 的摘要描述
/// </summary>
public class CFB2DC0600DAO : BaseDAO
{
	public CFB2DC0600DAO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}
    public string FUNC_ID { get; set; }

    public string CREATED_BY { get; set; }

    public string EMP_ID { get; set; }

    public string ABNORMAL_TYPE { get; set; }

    public string ABNORMAL_REASON_CD { get; set; }

    public string CALENDAR_DT { get; set; }

    public string ABNORMAL_DT { get; set; }

    public string HOUR { get; set; }

    public string MINUTE { get; set; }

    public string ABNORMAL_SOURCE_CD { get; set; }

    public string IS_RE_MAKE { get; set; }

    public string IS_CONFIRM { get; set; }

    public string REMARK { get; set; }

    public string UPDATED_BY { get; set; }
    //Gridview 查詢資料
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string abnormal_dt_s, string abnormal_dt_e, string emp_id,
                string dept_no, string abnormal_type, string abnormal_reason_cd, string abnormal_source_cd, string iflow_no, string iflow_approve_dt)
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
            sb.Append(" DEPT_NO,DEPT_NO+'-'+isnull(DEPT_NAME,'') as DEPT_NAME,a.EMP_ID,EMP_NAME,ABNORMAL_TYPE,ABNORMAL_TYPE + '-' + c.SUB_DESC ABNORMAL_TYPE_DESC,");
            sb.Append(" ABNORMAL_REASON_CD,ABNORMAL_REASON_CD + '-' + d.SUB_DESC ABNORMAL_REASON_DESC,");
            
            sb.Append(" REPLACE(CONVERT(char(10), a.CALENDAR_DT, 120),'-','/') CALENDAR_DT,");
            sb.Append(" REPLACE(CONVERT(char(16), a.ABNORMAL_DT, 120),'-','/') ABNORMAL_DT,");
            sb.Append(" ABNORMAL_SOURCE_CD,ABNORMAL_SOURCE_CD + '-' + e.SUB_DESC ABNORMAL_SOURCE_DESC,");
            sb.Append(" IFLOW_NO,REPLACE(CONVERT(char(10), a.IFLOW_APPROVE_DT, 120),'-','/') IFLOW_APPROVE_DT,");
            sb.Append(" IS_CONFIRM,case IS_RE_MAKE when 'Y' then '是' when 'N' then '否' else IS_RE_MAKE end as IS_RE_MAKE");
            sb.Append(" from TB_D_M_ABNORMAL_APPLY a left join VW_H_EMP_DATA b on a.EMP_ID = b.EMP_ID");
            sb.Append(" left join TB_9_M_COMM_D c on a.ABNORMAL_TYPE = c.SUB_CD and c.SYS_CD = 'DC' and c.MAIN_CD = 'ABNORMAL_TYPE' ");
            sb.Append(" left join TB_9_M_COMM_D d on a.ABNORMAL_REASON_CD = d.SUB_CD and d.SYS_CD = 'DC' and d.MAIN_CD = 'ABNORMAL_REASON_CD'");
            sb.Append(" left join TB_9_M_COMM_D e on a.ABNORMAL_SOURCE_CD = e.SUB_CD and e.SYS_CD = 'DC' and e.MAIN_CD = 'ABNORMAL_SOURCE_CD' ");
            
            sb.Append(" where a.EMP_ID is not null");
            if (abnormal_dt_s != "")
            {
                sb.Append(" and REPLACE(CONVERT(char(10), a.ABNORMAL_DT, 120),'-','/') >= @abnormal_dt_s ");
                ht.Add("@abnormal_dt_s", abnormal_dt_s);
            }
            if (abnormal_dt_e != "")
            {
                sb.Append(" and REPLACE(CONVERT(char(10), a.ABNORMAL_DT, 120),'-','/') <= @abnormal_dt_e ");
                ht.Add("@abnormal_dt_e", abnormal_dt_e);
            }
            if (emp_id != "")
            {
                sb.Append(" and a.EMP_ID like @EMP_ID ");
                ht.Add("@EMP_ID", emp_id + "%");
            }
            if (dept_no != "")
            {
                sb.Append(" and b.DEPT_NO like @DEPT_NO ");
                ht.Add("@DEPT_NO", dept_no + "%");
            }
            if (abnormal_type != "-1")
            {
                sb.Append(" and a.ABNORMAL_TYPE = @ABNORMAL_TYPE ");
                ht.Add("@ABNORMAL_TYPE", abnormal_type);
            }
            if (abnormal_reason_cd != "-1")
            {
                sb.Append(" and a.ABNORMAL_REASON_CD = @ABNORMAL_REASON_CD ");
                ht.Add("@ABNORMAL_REASON_CD", abnormal_reason_cd);
            }

            if (abnormal_source_cd != "-1")
            {
                sb.Append(" and a.ABNORMAL_SOURCE_CD = @ABNORMAL_SOURCE_CD ");
                ht.Add("@ABNORMAL_SOURCE_CD", abnormal_source_cd);
            }
            if (iflow_no != "")
            {

                sb.Append(" and a.IFLOW_NO like @IFLOW_NO");
                ht.Add("@IFLOW_NO", iflow_no + "%");

            }
            if (iflow_approve_dt != "")
            {

                sb.Append(" and REPLACE(CONVERT(char(7), a.IFLOW_APPROVE_DT, 120),'-','/') = @IFLOW_APPROVE_DT");
                ht.Add("@IFLOW_APPROVE_DT", iflow_approve_dt);

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
    public int getCount(int startRowIndex, int maximumRows, string abnormal_dt_s, string abnormal_dt_e, string emp_id,
                string dept_no, string abnormal_type, string abnormal_reason_cd, string abnormal_source_cd, string iflow_no, string iflow_approve_dt)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select COUNT(*) total_record ");
            sb.Append(" from TB_D_M_ABNORMAL_APPLY a left join VW_H_EMP_DATA b on a.EMP_ID = b.EMP_ID");
            sb.Append(" left join TB_9_M_COMM_D c on a.ABNORMAL_TYPE = c.SUB_CD and c.SYS_CD = 'DC' and c.MAIN_CD = 'ABNORMAL_TYPE' ");
            sb.Append(" left join TB_9_M_COMM_D d on a.ABNORMAL_REASON_CD = d.SUB_CD and d.SYS_CD = 'DC' and d.MAIN_CD = 'ABNORMAL_REASON_CD'");
            sb.Append(" left join TB_9_M_COMM_D e on a.ABNORMAL_SOURCE_CD = e.SUB_CD and e.SYS_CD = 'DC' and e.MAIN_CD = 'ABNORMAL_SOURCE_CD' ");

            sb.Append(" where a.EMP_ID is not null");
            if (abnormal_dt_s != "")
            {
                sb.Append(" and REPLACE(CONVERT(char(10), a.ABNORMAL_DT, 120),'-','/') >= @abnormal_dt_s ");
                ht.Add("@abnormal_dt_s", abnormal_dt_s);
            }
            if (abnormal_dt_e != "")
            {
                sb.Append(" and REPLACE(CONVERT(char(10), a.ABNORMAL_DT, 120),'-','/') <= @abnormal_dt_e ");
                ht.Add("@abnormal_dt_e", abnormal_dt_e);
            }
            if (emp_id != "")
            {
                sb.Append(" and a.EMP_ID like @EMP_ID ");
                ht.Add("@EMP_ID", emp_id + "%");
            }
            if (dept_no != "")
            {
                sb.Append(" and b.DEPT_NO like @DEPT_NO ");
                ht.Add("@DEPT_NO", dept_no + "%");
            }
            if (abnormal_type != "-1")
            {
                sb.Append(" and a.ABNORMAL_TYPE = @ABNORMAL_TYPE ");
                ht.Add("@ABNORMAL_TYPE", abnormal_type);
            }
            if (abnormal_reason_cd != "-1")
            {
                sb.Append(" and a.ABNORMAL_REASON_CD = @ABNORMAL_REASON_CD ");
                ht.Add("@ABNORMAL_REASON_CD", abnormal_reason_cd);
            }

            if (abnormal_source_cd != "-1")
            {
                sb.Append(" and a.ABNORMAL_SOURCE_CD = @ABNORMAL_SOURCE_CD ");
                ht.Add("@ABNORMAL_SOURCE_CD", abnormal_source_cd);
            }
            if (iflow_no != "")
            {

                sb.Append(" and a.IFLOW_NO like @IFLOW_NO");
                ht.Add("@IFLOW_NO", iflow_no + "%");

            }
            if (iflow_approve_dt != "")
            {

                sb.Append(" and REPLACE(CONVERT(char(7), a.IFLOW_APPROVE_DT, 120),'-','/') = @IFLOW_APPROVE_DT");
                ht.Add("@IFLOW_APPROVE_DT", iflow_approve_dt);

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

    

    internal DataTable getData(string emp_id, string abnormal_type, string calendar_dt, string abnormal_source_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" select");
            sb.Append(" DEPT_NO + DEPT_NAME DEPT_NAME,a.EMP_ID,EMP_NAME,ABNORMAL_TYPE,");
            sb.Append(" ABNORMAL_REASON_CD,");
            
            sb.Append(" REPLACE(CONVERT(char(10), a.CALENDAR_DT, 120),'-','/') CALENDAR_DT,");
            sb.Append(" REPLACE(CONVERT(char(10), a.ABNORMAL_DT, 120),'-','/') ABNORMAL_DT,");
            sb.Append(" RIGHT('0' + CONVERT(VARCHAR(2), DATEPART(HH,ABNORMAL_DT)), 2)  ABNORMAL_DT_HOUR,");
            sb.Append(" RIGHT('0' + CONVERT(VARCHAR(2), DATEPART(MI,ABNORMAL_DT)), 2)    ABNORMAL_DT_MINUTE, ");

            sb.Append(" ABNORMAL_SOURCE_CD,");
            sb.Append(" IFLOW_NO,REPLACE(CONVERT(char(10), a.IFLOW_APPROVE_DT, 120),'-','/') IFLOW_APPROVE_DT,");
            sb.Append(" IS_CONFIRM, IS_RE_MAKE ,a.REMARK");
            sb.Append(" from TB_D_M_ABNORMAL_APPLY a inner join VW_H_EMP_DATA b on a.EMP_ID = b.EMP_ID");
           
            sb.Append(" where a.EMP_ID = @EMP_ID and ABNORMAL_TYPE = @ABNORMAL_TYPE  ");
            sb.Append(" and REPLACE(CONVERT(char(10), CALENDAR_DT, 120),'-','/') = @CALENDAR_DT and ABNORMAL_SOURCE_CD = @ABNORMAL_SOURCE_CD");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@ABNORMAL_TYPE", abnormal_type);
            ht.Add("@CALENDAR_DT", calendar_dt);
            ht.Add("@ABNORMAL_SOURCE_CD", abnormal_source_cd);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            
            throw;
        }
    }

    internal DataTable getDupData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select EMP_ID  ");
            sb.Append(" from TB_D_M_ABNORMAL_APPLY where EMP_ID = @EMP_ID and ABNORMAL_TYPE = @ABNORMAL_TYPE ");
            sb.Append(" and REPLACE(CONVERT(char(10), CALENDAR_DT, 120),'-','/') = @CALENDAR_DT and ABNORMAL_SOURCE_CD = @ABNORMAL_SOURCE_CD");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@ABNORMAL_TYPE", ABNORMAL_TYPE);
            ht.Add("@CALENDAR_DT", CALENDAR_DT);
            ht.Add("@ABNORMAL_SOURCE_CD", ABNORMAL_SOURCE_CD);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            
            throw;
        }
    }

    internal void updateABNORMAL_APPLY()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("update  TB_D_M_ABNORMAL_APPLY set ABNORMAL_REASON_CD = @ABNORMAL_REASON_CD,ABNORMAL_DT = @ABNORMAL_DT,");
            sb.Append(" IS_CONFIRM = @IS_CONFIRM,IS_RE_MAKE = @IS_RE_MAKE,REMARK = @REMARK,");
            sb.Append(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID");
            sb.Append(" from TB_D_M_ABNORMAL_APPLY where EMP_ID = @EMP_ID and ABNORMAL_TYPE = @ABNORMAL_TYPE ");
            sb.Append(" and REPLACE(CONVERT(char(10), CALENDAR_DT, 120),'-','/') = @CALENDAR_DT and ABNORMAL_SOURCE_CD = @ABNORMAL_SOURCE_CD");

            ht.Add("@ABNORMAL_REASON_CD", ABNORMAL_REASON_CD);
            ht.Add("@ABNORMAL_DT", ABNORMAL_DT + " " + HOUR + ":" + MINUTE + ":00");
            ht.Add("@IS_CONFIRM", IS_CONFIRM);
            ht.Add("@IS_RE_MAKE", IS_RE_MAKE);
            ht.Add("@REMARK", REMARK);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@ABNORMAL_TYPE", ABNORMAL_TYPE);
            ht.Add("@CALENDAR_DT", CALENDAR_DT);
            ht.Add("@ABNORMAL_SOURCE_CD", ABNORMAL_SOURCE_CD);


            dbConn.ExecuteT(sb,ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void addABNORMAL_APPLY()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("insert into  TB_D_M_ABNORMAL_APPLY (EMP_ID,ABNORMAL_TYPE,CALENDAR_DT,ABNORMAL_DT,ABNORMAL_REASON_CD,ABNORMAL_SOURCE_CD,");
            sb.Append(" IS_CONFIRM,REMARK,IS_RE_MAKE,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" values (@EMP_ID,@ABNORMAL_TYPE,@CALENDAR_DT,@ABNORMAL_DT,@ABNORMAL_REASON_CD,@ABNORMAL_SOURCE_CD,");
            sb.Append(" @IS_CONFIRM,@REMARK,@IS_RE_MAKE,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@ABNORMAL_TYPE", ABNORMAL_TYPE);
            ht.Add("@CALENDAR_DT", CALENDAR_DT);
            ht.Add("@ABNORMAL_SOURCE_CD", ABNORMAL_SOURCE_CD);
            ht.Add("@ABNORMAL_REASON_CD", ABNORMAL_REASON_CD);
            ht.Add("@ABNORMAL_DT", ABNORMAL_DT + " " + HOUR + ":" + MINUTE + ":00");
            ht.Add("@IS_CONFIRM", IS_CONFIRM);
            ht.Add("@IS_RE_MAKE", IS_RE_MAKE);
            ht.Add("@REMARK", REMARK);
            ht.Add("@CREATED_BY", CREATED_BY);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //將 日勤務狀態檔 比對結果 改為 N
    internal void SP_D_EMP_DUTY_CHECK_STATUS_RE_OPEN()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_D_EMP_DUTY_CHECK_STATUS_RE_OPEN");
            ht.Add("@pEmpId", EMP_ID);
            ht.Add("@pCalendarDt", CALENDAR_DT);
            ht.Add("@pUserID", SessionHandle.Current.emp_id);
            ht.Add("@pFuncID", "FB2DC060");

            dbConn.ExecuteSP(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }


    internal void deleteABNORMAL_APPLY(string emp_id, string abnormal_type, string calendar_dt, string abnormal_source_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //寫log
            sb.Append(" update TB_D_M_ABNORMAL_APPLY set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2DC060' ");
            sb.Append(" where EMP_ID = @EMP_ID and ABNORMAL_TYPE = @ABNORMAL_TYPE and ");
            sb.Append(" REPLACE(CONVERT(char(10), CALENDAR_DT, 120),'-','/') = @CALENDAR_DT and ABNORMAL_SOURCE_CD = @ABNORMAL_SOURCE_CD;");
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);

            sb.Append(" delete from  TB_D_M_ABNORMAL_APPLY ");
            sb.Append(" where EMP_ID = @EMP_ID and ABNORMAL_TYPE = @ABNORMAL_TYPE and ");
            sb.Append(" REPLACE(CONVERT(char(10), CALENDAR_DT, 120),'-','/') = @CALENDAR_DT and ABNORMAL_SOURCE_CD = @ABNORMAL_SOURCE_CD;");

            ht.Add("@EMP_ID", emp_id);
            ht.Add("@ABNORMAL_TYPE", abnormal_type);
            ht.Add("@CALENDAR_DT", calendar_dt);
            ht.Add("@ABNORMAL_SOURCE_CD", abnormal_source_cd);
            
            dbConn.ExecuteT(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }


    //Gridview 查詢資料
    public DataTable getBatchData(int startRowIndex, int maximumRows, string sortExpression, string join_dt, string dept_no, string plant_cd,
                                string line_cd, string work_shift_cd, string AddEmp, string DeleteEmp, string SearchFlag)
    {
        try
        {
            
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" DEPT_NO,EMP_ID,EMP_NAME,PLANT_CD,PLANT_NAME,DEPT_FULL_NAME,WORK_SHIFT_CD,WORK_SHIFT_DESC");

            sb.Append(" from VW_H_EMP_DATA ");
           
            if (SearchFlag == "Y")
            {
                sb.Append(" where EMP_STATUS in ('01','02') ");
                if (join_dt != "")
                {
                    sb.Append(" and REPLACE(CONVERT(char(10), JOIN_DT, 120),'-','/') >= @join_dt ");
                    ht.Add("@join_dt", join_dt);
                }
                if (dept_no != "")
                {
                    sb.Append(" and DEPT_NO= @dept_no ");
                    ht.Add("@dept_no", dept_no);
                }
                if (plant_cd != "-1")
                {
                    sb.Append(" and PLANT_CD = @plant_cd ");
                    ht.Add("@plant_cd", plant_cd);
                }
                if (line_cd != "-1")
                {
                    sb.Append(" and EMP_ID in (select EMP_ID from TB_D_M_TRANS_ALLOWANCE_D where LINE_CD = @LINE_CD and GETDATE() >= START_DT and GETDATE() <= END_DT) ");
                    ht.Add("@LINE_CD", line_cd);
                }
                if (work_shift_cd != "")
                {
                    sb.Append(" and WORK_SHIFT_CD = @work_shift_cd ");
                    ht.Add("@work_shift_cd", work_shift_cd);
                }
            }
            else
            {
                sb.Append(" where EMP_ID is null ");
            }
            if (DeleteEmp != "")
            {
                string[] arrDeleteEmp = DeleteEmp.Split(',');

                sb.Append(" and ( EMP_ID not in (@deleteEMP_IDS) )");
                ht.Add("@deleteEMP_IDS", arrDeleteEmp);
            }
            if (AddEmp != "")
            {
                string[] arrAddEmp = AddEmp.Split(',');

                sb.Append(" or ( EMP_ID in (@addEMP_IDS) )");
                ht.Add("@addEMP_IDS", arrAddEmp);
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
    public int getBatchCount(int startRowIndex, int maximumRows, string join_dt, string dept_no, string plant_cd,
                            string line_cd, string work_shift_cd, string AddEmp, string DeleteEmp, string SearchFlag)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select COUNT(*) total_record ");
            sb.Append(" from VW_H_EMP_DATA ");
            if (SearchFlag == "Y")
            {
                sb.Append(" where EMP_STATUS in ('01','02') ");
                if (join_dt != "")
                {
                    sb.Append(" and REPLACE(CONVERT(char(10), JOIN_DT, 120),'-','/') >= @join_dt ");
                    ht.Add("@join_dt", join_dt);
                }
                if (dept_no != "")
                {
                    sb.Append(" and DEPT_NO= @dept_no ");
                    ht.Add("@dept_no", dept_no);
                }
                if (plant_cd != "-1")
                {
                    sb.Append(" and PLANT_CD = @plant_cd ");
                    ht.Add("@plant_cd", plant_cd);
                }
                if (line_cd != "-1")
                {
                    sb.Append(" and EMP_ID in (select EMP_ID from TB_D_M_TRANS_ALLOWANCE_D where LINE_CD = @LINE_CD and GETDATE() >= START_DT and GETDATE() <= END_DT) ");
                    ht.Add("@LINE_CD", line_cd);
                }
                if (work_shift_cd != "")
                {
                    sb.Append(" and WORK_SHIFT_CD = @work_shift_cd ");
                    ht.Add("@work_shift_cd", work_shift_cd);
                }
            }
            else
            {
                sb.Append(" where EMP_ID is null ");
            }
            if (DeleteEmp != "")
            {
                string[] arrDeleteEmp = DeleteEmp.Split(',');

                sb.Append(" and ( EMP_ID not in (@deleteEMP_IDS) )");
                ht.Add("@deleteEMP_IDS", arrDeleteEmp);
            }
            if (AddEmp != "")
            {
                string[] arrAddEmp = AddEmp.Split(',');

                sb.Append(" or ( EMP_ID in (@EMP_IDS) )");
                ht.Add("@EMP_IDS", arrAddEmp);
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

    internal DataTable getExistData(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select EMP_NAME  ");
            sb.Append(" from TB_D_M_ABNORMAL_APPLY a,VW_H_EMP_DATA b where a.EMP_ID = b.EMP_ID and a.EMP_ID = @EMP_ID and ABNORMAL_TYPE = @ABNORMAL_TYPE ");
            sb.Append(" and REPLACE(CONVERT(char(10), CALENDAR_DT, 120),'-','/') = @CALENDAR_DT and ABNORMAL_SOURCE_CD = @ABNORMAL_SOURCE_CD");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@ABNORMAL_TYPE", ABNORMAL_TYPE);
            ht.Add("@CALENDAR_DT", CALENDAR_DT);
            ht.Add("@ABNORMAL_SOURCE_CD", ABNORMAL_SOURCE_CD);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            
            throw;
        }
    }

    internal void callSP_D_UPD_CARD_DATA(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_D_UPD_CARD_DATA");
            ht.Add("@pHandleCd", "I");
            ht.Add("@pEmpId", emp_id);
            ht.Add("@pCardUsedCd", "A");
            ht.Add("@pStartDt", DateTime.Now.ToString("yyyy/MM/dd"));
            ht.Add("@pEndDt", "9999/12/31");
            ht.Add("@pUserID", CREATED_BY);
            ht.Add("@pFuncID", FUNC_ID);

            dbConn.ExecuteSP(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }
    public DataTable getEmp_Name(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select EMP_NAME,DEPT_NAME ");
            sb.AppendLine(" from VW_H_EMP_DATA ");
            sb.AppendLine(" where EMP_ID =@EMP_ID ");
            ht.Add("@EMP_ID", emp_id);
            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable getDEPT_NAME(string dept_no)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select DEPT_NAME ");
            sb.AppendLine(" from VW_H_DEPT_DATA ");
            sb.AppendLine(" where DEPT_NO = @DEPT_NO ");
            ht.Add("@DEPT_NO", dept_no);

            return dbConn.Query(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getWORK_SHIFT_DESC(string work_shift_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select WORK_SHIFT_DESC ");
            sb.AppendLine(" from TB_D_M_WORK_SHIFT_H ");
            sb.AppendLine(" where WORK_SHIFT_CD = @WORK_SHIFT_CD ");
            ht.Add("@WORK_SHIFT_CD", work_shift_cd);

            return dbConn.Query(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getEmp_Name_add(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("Select e.EMP_ID,e.EMP_NAME,d.DEPT_NAME,e.DEPT_NO,PLANT_NAME,WORK_SHIFT_DESC ");
            sb.AppendLine("from VW_H_EMP_DATA e,VW_H_DEPT_DATA d                                 ");
            sb.AppendLine("where e.DEPT_NO = d.DEPT_NO and EMP_STATUS = '01' and e.EMP_ID=@EMP_ID");
            ht.Add("@EMP_ID", emp_id);
            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
}