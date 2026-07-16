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
public class CFB2DJ0200DAO : BaseDAO
{
    //基本欄位
    public string DEPT_NAME { get; set; }
    public string DEPT_NO { get; set; }
    public string LAYOUT_NO { get; set; }
    public string ENV_ALLOWANCE_TYPE { get; set; }
    public string START_DT { get; set; }
    public string END_DT { get; set; }
    public string WORK_SHIFT_NAME { get; set; }
    public string ENV_MAX_HOUR { get; set; }
    public string REMARK { get; set; }
    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }

    


    public CFB2DJ0200DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }


    #region 資料取得
    //查詢條件的環境津貼等級(僅有生效的)
    public DataTable getEnvType()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select distinct ENV_ALLOWANCE_TYPE sub_cd ");
            sb.Append(" from TB_D_M_ENV_ALLOWANCE_TYPE ");
           // sb.Append(" where GETDATE() >= START_DT and GETDATE()  <= END_DT ");
            //sb.Append(" order by ENV_ALLOWANCE_VALUE desc ");
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getPKData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select count(0) resultCount ");
            sb.Append(" from TB_D_M_ENV_ALLOWANCE_LIMIT ");
            sb.Append(" where 1=1 ");
            sb.Append(" and DEPT_NO = @DEPT_NO  ");
            sb.Append(" and LAYOUT_NO = @LAYOUT_NO  ");
            sb.Append(" and ENV_ALLOWANCE_TYPE = @ENV_ALLOWANCE_TYPE  ");
            sb.Append(" and START_DT = @START_DT  ");

            ht.Add("@DEPT_NO", DEPT_NO);
            ht.Add("@LAYOUT_NO", LAYOUT_NO);
            ht.Add("@ENV_ALLOWANCE_TYPE", ENV_ALLOWANCE_TYPE);
            ht.Add("@START_DT", Convert.ToDateTime(START_DT));

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //取得同部課+layout的等級只能同一種
    public DataTable getDeptLayoutData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select count(0) resultCount,ENV_ALLOWANCE_TYPE ");
            sb.Append(" from TB_D_M_ENV_ALLOWANCE_LIMIT ");
            sb.Append(" where 1=1 ");
            sb.Append(" and DEPT_NO like @DEPT_NO  ");
            sb.Append(" and LAYOUT_NO = @LAYOUT_NO  ");
            //sb.Append(" and ENV_ALLOWANCE_TYPE = @ENV_ALLOWANCE_TYPE  ");
            sb.Append(" and @START_DT between START_DT and END_DT  ");
            sb.Append(" group by ENV_ALLOWANCE_TYPE ");

            ht.Add("@DEPT_NO", DEPT_NO.Substring(0,4)+"%"  );
            ht.Add("@LAYOUT_NO", LAYOUT_NO);
            //ht.Add("@ENV_ALLOWANCE_TYPE", ENV_ALLOWANCE_TYPE);
            ht.Add("@START_DT", Convert.ToDateTime(START_DT));
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }



    //取得在某日期(生效日期)是否已有效的資料，
    internal DataTable getMaxEndDTByType()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select  MAX(END_DT) maxEndDT from TB_D_M_ENV_ALLOWANCE_LIMIT");
            sb.Append(" where 1=1 ");
            sb.Append(" and DEPT_NO = @DEPT_NO  ");
            sb.Append(" and LAYOUT_NO = @LAYOUT_NO  ");
            sb.Append(" and ENV_ALLOWANCE_TYPE = @ENV_ALLOWANCE_TYPE  ");

            ht.Add("@DEPT_NO", DEPT_NO);
            ht.Add("@LAYOUT_NO", LAYOUT_NO);
            ht.Add("@ENV_ALLOWANCE_TYPE", ENV_ALLOWANCE_TYPE);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //Grid的部門資料
    public DataTable getDeptData(string dept_no)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select DEPT_NO,DEPT_NAME,DEPT_LEVEL,UP_DEPT_NO,UP_DEPT_NAME,HEAD_EMP_ID ,isnull(HEAD_EMP_NAME,'') HEAD_EMP_NAME  ");
            sb.Append(" ,DEPT_NO_20,DEPT_NAME_20,DEPT_NO_30,DEPT_NAME_30,DEPT_NO_40,DEPT_NAME_40,DEPT_NO_50,DEPT_NAME_50,DEPT_NO_60,DEPT_NAME_60,DEPT_NO_70,DEPT_NAME_70  ");
            sb.Append(" ,DEPT_FULL_NAME, DIV_DEPT_FULL_NAME  ");
            sb.Append(" From VW_H_DEPT_DATA where DEPT_NO is not null");
            //sb.Append("Select DEPT_NO,DEPT_NAME from TB_H_M_DEPT where DEPT_NO is not null ");

            if (dept_no != "")
            {
                sb.Append(" and DEPT_NO = @DEPT_NO");
                ht.Add("@DEPT_NO", dept_no);
            }

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }


    //取得 環境津貼申請資料檔 是否已使用
    internal DataTable getExistType(string dept_no, string layout_no, string type, string startDT, string endDT)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(0) typecount ");
            sb.Append(" From TB_D_M_ENV_ALLOWANCE_APPLY");
            sb.Append(" where ENV_ALLOWANCE_TYPE = @ENV_ALLOWANCE_TYPE");
            sb.Append(" and LAYOUT_NO = @LAYOUT_NO");
            sb.Append(" and DEPT_NO = @DEPT_NO");
            sb.Append(" and APPLY_DT >= @startDT ");
            sb.Append(" and APPLY_DT <= @endDT ");
            ht.Add("@ENV_ALLOWANCE_TYPE", type);
             ht.Add("@LAYOUT_NO", layout_no);
             ht.Add("@DEPT_NO", dept_no);
            ht.Add("@startDT", startDT);
            ht.Add("@endDT", endDT);
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
                             , string dept_name, string layout_no, string dept_no, string env_type, string is_valid, string work_shift_name)
    {
        try
        {

            //if (sortExpression.Contains("ENV_ALLOWANCE_TYPE"))
            //    sortExpression = sortExpression.Replace("ENV_ALLOWANCE_TYPE", "a.ENV_ALLOWANCE_TYPE");


            //if (sortExpression.Contains("LAYOUT_NO"))
            //    sortExpression = sortExpression.Replace("LAYOUT_NO", " convert(int, LAYOUT_NO ) ");


            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select * From   ");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + ") As RowNumber,");
            sb.Append(" DEPT_NAME, DEPT_NO,WORK_SHIFT_NAME,LAYOUT_NO,ENV_ALLOWANCE_TYPE, ENV_MAX_HOUR ,START_DT,END_DT,REMARK ");
            sb.Append(" from TB_D_M_ENV_ALLOWANCE_LIMIT ");
            sb.Append(" where 1=1 ");



            //查詢條件
            if (dept_name != "")
            {
                sb.Append(" and DEPT_NAME like @DEPT_NAME ");
                ht.Add("@DEPT_NAME", "%"+dept_name + "%");
            }
            if (dept_no != "")
            {
                sb.Append(" and DEPT_NO like @DEPT_NO ");
                ht.Add("@DEPT_NO", dept_no + "%");
            }


            if (layout_no != "")
            {
                sb.Append(" and LAYOUT_NO = @LAYOUT_NO ");
                ht.Add("@LAYOUT_NO", layout_no );
            }

            if (env_type !="" && env_type != "-1")
            {
                sb.Append(" and ENV_ALLOWANCE_TYPE = @ENV_ALLOWANCE_TYPE ");
                ht.Add("@ENV_ALLOWANCE_TYPE", env_type);
            }
            if (is_valid == "Y")
            {
                sb.Append(" and GETDATE() >= START_DT and GETDATE()  <= END_DT   ");
            }
            if (is_valid == "N")
            {
                sb.Append(" and GETDATE()  >= END_DT    ");
            }
            if (work_shift_name != "")
            {
                sb.Append(" and WORK_SHIFT_NAME like @WORK_SHIFT_NAME ");
                ht.Add("@WORK_SHIFT_NAME", "%" + work_shift_name+"%");
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
                         , string dept_name, string layout_no, string dept_no, string env_type, string is_valid, string work_shift_name)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) total_record ");
            sb.Append(" from TB_D_M_ENV_ALLOWANCE_LIMIT ");
            sb.Append(" where 1=1 ");

            //查詢條件
            if (dept_name != "")
            {
                sb.Append(" and DEPT_NAME like @DEPT_NAME ");
                ht.Add("@DEPT_NAME", dept_name + "%");
            }
            if (dept_no != "")
            {
                sb.Append(" and DEPT_NO like @DEPT_NO ");
                ht.Add("@DEPT_NO", dept_no + "%");
            }


            if (layout_no != "")
            {
                sb.Append(" and LAYOUT_NO = @LAYOUT_NO ");
                ht.Add("@LAYOUT_NO", layout_no);
            }

            if (env_type != "" && env_type != "-1")
            {
                sb.Append(" and ENV_ALLOWANCE_TYPE = @ENV_ALLOWANCE_TYPE ");
                ht.Add("@ENV_ALLOWANCE_TYPE", env_type);
            }
            if (is_valid == "Y")
            {
                sb.Append(" and GETDATE() >= START_DT and GETDATE()  <= END_DT   ");
            }
            if (is_valid == "N")
            {
                sb.Append(" and GETDATE()  >= END_DT    ");
            }
            if (work_shift_name != "")
            {
                sb.Append(" and WORK_SHIFT_NAME like @WORK_SHIFT_NAME ");
                ht.Add("@WORK_SHIFT_NAME", "%" + work_shift_name + "%");
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
    public void deleteData(string dept_no, string layout_no, string type, string start_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //寫log
            sb.Append(" update TB_D_M_ENV_ALLOWANCE_LIMIT set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2DJ020' ");
            sb.Append(" where DEPT_NO = @DEPT_NO ");
            sb.Append(" and LAYOUT_NO = @LAYOUT_NO ");
            sb.Append(" and ENV_ALLOWANCE_TYPE = @ENV_ALLOWANCE_TYPE ");
            sb.Append(" and START_DT = @START_DT;  ");
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);

            sb.Append(" delete from TB_D_M_ENV_ALLOWANCE_LIMIT ");
            sb.Append(" where DEPT_NO = @DEPT_NO ");
            sb.Append(" and LAYOUT_NO = @LAYOUT_NO ");
            sb.Append(" and ENV_ALLOWANCE_TYPE = @ENV_ALLOWANCE_TYPE ");
            sb.Append(" and START_DT = @START_DT;  ");
            ht.Add("@DEPT_NO", dept_no);
            ht.Add("@LAYOUT_NO", layout_no);
            ht.Add("@ENV_ALLOWANCE_TYPE", type);
            ht.Add("@START_DT", Convert.ToDateTime(start_dt).ToString("yyyy/MM/dd"));
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
            sb.Append(" update TB_D_M_ENV_ALLOWANCE_LIMIT ");
            sb.Append(" set END_DT = @END_DT ");
            sb.Append(" ,WORK_SHIFT_NAME = @WORK_SHIFT_NAME ");
            sb.Append(" ,ENV_MAX_HOUR = @ENV_MAX_HOUR ");
            sb.Append(" ,REMARK = @REMARK ");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT = @UPDATED_DT");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where 1=1");
            sb.Append(" and DEPT_NO = @DEPT_NO ");
            sb.Append(" and LAYOUT_NO = @LAYOUT_NO ");
            sb.Append(" and ENV_ALLOWANCE_TYPE = @ENV_ALLOWANCE_TYPE ");
            sb.Append(" and START_DT = @START_DT ");

            ht.Add("@END_DT", END_DT + " 23:59:59");
            ht.Add("@WORK_SHIFT_NAME", WORK_SHIFT_NAME);
            ht.Add("@ENV_MAX_HOUR", ENV_MAX_HOUR);
            ht.Add("@REMARK", REMARK);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@UPDATED_DT", now);
            ht.Add("@FUNC_ID", FUNC_ID);
            //PK值
            ht.Add("@DEPT_NO", DEPT_NO);
            ht.Add("@LAYOUT_NO", LAYOUT_NO);
            ht.Add("@ENV_ALLOWANCE_TYPE", ENV_ALLOWANCE_TYPE);
            ht.Add("@START_DT", Convert.ToDateTime(START_DT));

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
            sb.Append(" INSERT INTO TB_D_M_ENV_ALLOWANCE_LIMIT ");
            sb.Append(" ( ");
            sb.Append(" DEPT_NAME, DEPT_NO, LAYOUT_NO, ENV_ALLOWANCE_TYPE, START_DT ");
            sb.Append(" , END_DT, WORK_SHIFT_NAME, ENV_MAX_HOUR, REMARK  ");
            sb.Append(" , CREATED_BY, CREATED_DT, UPDATED_BY, UPDATED_DT, FUNC_ID  ");
            sb.Append(" ) ");
            sb.Append(" VALUES ");
            sb.Append(" ( ");
            sb.Append(" @DEPT_NAME, @DEPT_NO, @LAYOUT_NO, @ENV_ALLOWANCE_TYPE, @START_DT  ");
            sb.Append(" , @END_DT, @WORK_SHIFT_NAME, @ENV_MAX_HOUR, @REMARK  ");
            sb.Append(" ,@CREATED_BY,  @CREATED_DT,  @UPDATED_BY,  @UPDATED_DT,  @FUNC_ID ");
             sb.Append(" ) ");


             ht.Add("@DEPT_NAME", DEPT_NAME);
             ht.Add("@DEPT_NO", DEPT_NO);
             ht.Add("@LAYOUT_NO", LAYOUT_NO);
             ht.Add("@ENV_ALLOWANCE_TYPE", ENV_ALLOWANCE_TYPE);
             ht.Add("@START_DT", Convert.ToDateTime(START_DT));
             ht.Add("@END_DT", Convert.ToDateTime(END_DT));
             ht.Add("@WORK_SHIFT_NAME", WORK_SHIFT_NAME);
             ht.Add("@ENV_MAX_HOUR", ENV_MAX_HOUR);
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