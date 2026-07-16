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
public class CFB2SG0200DAO : BaseDAO
{
    //節金維護檔 基本欄位
    public string FESTIVAL_TYPE { get; set; }
    public string FESTIVAL_DT { get; set; }
    public string EMP_CD_PK { get; set; } 
    public string EMP_CD { get; set; }
    public string FESTIVAL_PAY_DT { get; set; }

    public string FESTIVAL_DESC { get; set; }
    public string FESTIVAL_TOTAL_AMT { get; set; }
    public string FESTIVAL_TOTAL_NUM { get; set; }
    public string APPROVE_STATUS { get; set; }
    public string REMARK { get; set; }

    public string TARGET_GEN_DT { get; set; }
    public string RELEASE_DT { get; set; }
    public string RELEASE_BY { get; set; }
    public string APPROVE_DT { get; set; }
    public string APPROVE_BY { get; set; }
    public string SALARY_TRANS_DT { get; set; }
    public string SALARY_TRANS_BY { get; set; }
    public string PROCESS_STATUS { get; set; }
    public string SALARY_DT { get; set; }
    public string FREEZE_FLAG { get; set; }


    //節金維護檔 基本欄位
    public string FESTIVAL_LOGIC { get; set; }
    public string CALCULATE_ITEM { get; set; }
    public string CALCULATE_COND { get; set; }
    public string CALCULATE_CONTENT1 { get; set; }
    public string CALCULATE_CONTENT2 { get; set; }
    public string FESTIVAL_SQL_COMMAND { get; set; }


    //節金明細維護檔
     public string EMP_ID { get; set; }
     public string EMP_NAME { get; set; }
     public string DEPT_NO { get; set; }
     public string PLANT_CD { get; set; }
     public string JPN_CD { get; set; }
     public string COMPANY_CD { get; set; }
     public string LEVEL_CD { get; set; }
     public string GRADE_CD { get; set; }

    public string PJOB_CD { get; set; }
    public string JOIN_DT { get; set; }
    public string WORK_YEARS { get; set; }
    public string WORK_DAYS { get; set; }
    public string EMP_CHG_CD { get; set; }
    public string WS_CD { get; set; }

    public string SEX_CD { get; set; }
    public string LEVEL_PAY { get; set; }
    public string ABILITY_PAY { get; set; }
    public string PJOB_PAY{ get; set; }
    public string PROFESSION_PAY { get; set; }
    public string FOOD_SUBSIDY { get; set; }
    
    public string FESTIVAL_AMT { get; set; }
    public string FESTIVAL_AMT_OLD { get; set; }
    public string FESTIVAL_TAX { get; set; }
    public string FESTIVAL_AMT_R { get; set; }
    public string PAY_TYPE { get; set; }
    public string PAY_TYPE_OLD { get; set; }

    public string APPROVE_FLAG { get; set; }
    public string CHG_STATUS { get; set; }
    public string APPROVE_MARK { get; set; }

    //共用欄位
    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }


    public CFB2SG0200DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }


    #region 資料取得


    //依PK值取得資料(節金維護檔)
    internal DataTable getPKData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select count(0) resultCount from TB_S_M_FESTIVAL_H");
            sb.Append(" where FESTIVAL_TYPE=@FESTIVAL_TYPE");
            sb.Append(" and FESTIVAL_DT = @FESTIVAL_DT");
            sb.Append(" and EMP_CD = @EMP_CD");
            sb.Append(" and FESTIVAL_PAY_DT = @FESTIVAL_PAY_DT");
            ht.Add("@FESTIVAL_TYPE", FESTIVAL_TYPE);
            ht.Add("@FESTIVAL_DT", Convert.ToDateTime(FESTIVAL_DT));
            ht.Add("@EMP_CD", EMP_CD);
            ht.Add("@FESTIVAL_PAY_DT", Convert.ToDateTime(FESTIVAL_PAY_DT));
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //依PK值取得資料(節金條件檔)，
    internal DataTable getPKCAL()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select count(0) resultCount from TB_S_M_FESTIVAL_CAL");
            sb.Append(" where FESTIVAL_TYPE=@FESTIVAL_TYPE");
            sb.Append(" and FESTIVAL_DT = @FESTIVAL_DT");
            sb.Append(" and EMP_CD = @EMP_CD");
            sb.Append(" and FESTIVAL_PAY_DT = @FESTIVAL_PAY_DT");
            sb.Append(" and FESTIVAL_SQL_COMMAND = @FESTIVAL_SQL_COMMAND");
            ht.Add("@FESTIVAL_TYPE", FESTIVAL_TYPE);
            ht.Add("@FESTIVAL_DT", Convert.ToDateTime(FESTIVAL_DT));
            ht.Add("@EMP_CD", EMP_CD);
            ht.Add("@FESTIVAL_PAY_DT", Convert.ToDateTime(FESTIVAL_PAY_DT));
            ht.Add("@FESTIVAL_SQL_COMMAND", FESTIVAL_SQL_COMMAND);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //取得生效的資格檔，
    internal DataTable getEMPLevelData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select LEVEL_CD level, ORDER_SEQ orderSeq from  TB_H_M_LEVEL ");
            sb.Append(" where GETDATE()>=START_DT and GETDATE()<=END_DT ");
            sb.Append(" order by ORDER_SEQ ");
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
    //取得尚生效的職務檔，
    internal DataTable getPjobData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select PJOB_CD pjob  from  TB_H_M_PJOB ");
            sb.Append(" where GETDATE()>=START_DT and GETDATE()<=END_DT ");
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }


    #endregion

    #region Gridview 資料
    //Gridview 查詢資料(Qry)
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression
                            , string festival_type, string emp_cd, string festival_dt, string festivalPayDT
                           )
    {
        try
        {

            //if (sortExpression.Contains("ENV_ALLOWANCE_TYPE"))
            //    sortExpression = sortExpression.Replace("ENV_ALLOWANCE_TYPE", "a.ENV_ALLOWANCE_TYPE");

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + ") As RowNumber,");
            sb.Append(" FESTIVAL_TYPE, EMP_CD, FESTIVAL_DT, FESTIVAL_PAY_DT, FESTIVAL_DESC   ");
            sb.Append(" , TARGET_GEN_DT, FREEZE_FLAG  ");
            sb.Append(" , FESTIVAL_TYPE + '-' + b.SUB_DESC FESTIVAL_TYPE_DESC   ");
            sb.Append(" , EMP_CD + '-' + c.SUB_DESC EMP_CD_DESC   ");
            sb.Append(" from TB_S_M_FESTIVAL_H a ");
            sb.Append("  left join TB_9_M_COMM_D b on  a.FESTIVAL_TYPE = b.SUB_CD and b.MAIN_CD = 'FESTIVAL_TYPE' and b.IS_VALID='Y' and b.SYS_CD='SG' ");
            sb.Append("  left join TB_9_M_COMM_D c on  a.EMP_CD = c.SUB_CD and c.MAIN_CD = 'EMP_CD'and c.IS_VALID='Y' and c.SYS_CD='HB' ");
            sb.Append(" where 1=1 ");

            //查詢條件-dropDownList
            if (festival_type !="" && festival_type != "-1")
            {
                sb.Append(" and FESTIVAL_TYPE = @FESTIVAL_TYPE ");
                ht.Add("@FESTIVAL_TYPE", festival_type);
            }
            if (emp_cd != "" && emp_cd != "-1")
            {
                sb.Append(" and EMP_CD = @EMP_CD ");
                ht.Add("@EMP_CD", emp_cd);
            }
            //查詢條件-日期
            if (festival_dt != "")
            {
                sb.Append(" and FESTIVAL_DT = @FESTIVAL_DT ");
                ht.Add("@FESTIVAL_DT", Convert.ToDateTime(festival_dt).ToString("yyyy/MM/dd"));
            }
            if (festivalPayDT != "")
            {
                sb.Append(" and FESTIVAL_PAY_DT = @FESTIVAL_PAY_DT ");
                ht.Add("@FESTIVAL_PAY_DT", Convert.ToDateTime(festivalPayDT).ToString("yyyy/MM/dd"));
            }



            //查詢條件-text
            //if (dept_name != "")
            //{
            //    sb.Append(" and DEPT_NAME like @DEPT_NAME ");
            //    ht.Add("@DEPT_NAME", dept_name + "%");
            //}



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

    //Gridview 查詢總筆數(Qry)
    public int getCount(int startRowIndex, int maximumRows
                         , string festival_type, string emp_cd, string festival_dt, string festivalPayDT
                        )
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) resultCount ");
            sb.Append(" from TB_S_M_FESTIVAL_H ");
            sb.Append(" where 1=1 ");



            //查詢條件-dropDownList
            if (festival_type != "" && festival_type != "-1")
            {
                sb.Append(" and FESTIVAL_TYPE = @FESTIVAL_TYPE ");
                ht.Add("@FESTIVAL_TYPE", festival_type);
            }
            if (emp_cd != "" && emp_cd != "-1")
            {
                sb.Append(" and EMP_CD = @EMP_CD ");
                ht.Add("@EMP_CD", emp_cd);
            }
            //查詢條件-日期
            if (festival_dt != "")
            {
                sb.Append(" and FESTIVAL_DT = @FESTIVAL_DT ");
                ht.Add("@FESTIVAL_DT", Convert.ToDateTime(festival_dt).ToString("yyyy/MM/dd"));
            }
            if (festivalPayDT != "")
            {
                sb.Append(" and FESTIVAL_PAY_DT = @FESTIVAL_PAY_DT ");
                ht.Add("@FESTIVAL_PAY_DT", Convert.ToDateTime(festivalPayDT).ToString("yyyy/MM/dd"));
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


    //Gridview 查詢資料(Dtl)
    public DataTable getDataDtl(int startRowIndex, int maximumRows, string sortExpression
                                , string festival_type, string emp_cd, string festival_dt, string festivalPayDT
                           )
    {
        try
        {

            //if (sortExpression.Contains("ENV_ALLOWANCE_TYPE"))
            //    sortExpression = sortExpression.Replace("ENV_ALLOWANCE_TYPE", "a.ENV_ALLOWANCE_TYPE");

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + ") As RowNumber,");
            sb.Append(" FESTIVAL_LOGIC, CALCULATE_ITEM, CALCULATE_COND, CALCULATE_CONTENT1, CALCULATE_CONTENT2, FESTIVAL_SQL_COMMAND   ");
            sb.Append(" , CALCULATE_ITEM + '-' + b.SUB_DESC CALCULATE_ITEM_DESC   ");
            sb.Append(" , FESTIVAL_TYPE + '-' + c.SUB_DESC FESTIVAL_TYPE_DESC   ");
            sb.Append(" from TB_S_M_FESTIVAL_CAL a ");
            sb.Append("  left join TB_9_M_COMM_D b on  a.CALCULATE_ITEM = b.SUB_CD and b.MAIN_CD = 'CALCULATE_ITEM' and b.IS_VALID='Y' and b.SYS_CD='SG' ");
            sb.Append("  left join TB_9_M_COMM_D c on  a.FESTIVAL_TYPE = c.SUB_CD and c.MAIN_CD = 'FESTIVAL_TYPE' and c.IS_VALID='Y' and c.SYS_CD='SG' ");
            sb.Append(" where 1=1 ");

            //若直接輸入網址不應該有查詢資料
            if (festival_type == "" || emp_cd == "" || festival_dt == "" || festivalPayDT == "")
            {
                sb.Append(" and 1=2 ");
            }
            //查詢條件
            if (festival_type != "")
            {
                sb.Append(" and FESTIVAL_TYPE = @FESTIVAL_TYPE ");
                ht.Add("@FESTIVAL_TYPE", festival_type);
            }
            if (emp_cd != "")
            {
                sb.Append(" and EMP_CD = @EMP_CD ");
                ht.Add("@EMP_CD", emp_cd);
            }
            //查詢條件-日期
            if (festival_dt != "")
            {
                sb.Append(" and FESTIVAL_DT = @FESTIVAL_DT ");
                ht.Add("@FESTIVAL_DT", Convert.ToDateTime(festival_dt).ToString("yyyy/MM/dd"));
            }
            if (festivalPayDT != "")
            {
                sb.Append(" and FESTIVAL_PAY_DT = @FESTIVAL_PAY_DT ");
                ht.Add("@FESTIVAL_PAY_DT", Convert.ToDateTime(festivalPayDT).ToString("yyyy/MM/dd"));
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

    //Gridview 查詢總筆數(Dtl)
    public int getCountDtl(int startRowIndex, int maximumRows
                            , string festival_type, string emp_cd, string festival_dt, string festivalPayDT
                        )
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) resultCount ");
            sb.Append(" from TB_S_M_FESTIVAL_CAL ");
            sb.Append(" where 1=1 ");

            //查詢條件-
            if (festival_type != "")
            {
                sb.Append(" and FESTIVAL_TYPE = @FESTIVAL_TYPE ");
                ht.Add("@FESTIVAL_TYPE", festival_type);
            }
            if (emp_cd != "")
            {
                sb.Append(" and EMP_CD = @EMP_CD ");
                ht.Add("@EMP_CD", emp_cd);
            }
            //查詢條件-日期
            if (festival_dt != "")
            {
                sb.Append(" and FESTIVAL_DT = @FESTIVAL_DT ");
                ht.Add("@FESTIVAL_DT", Convert.ToDateTime(festival_dt).ToString("yyyy/MM/dd"));
            }
            if (festivalPayDT != "")
            {
                sb.Append(" and FESTIVAL_PAY_DT = @FESTIVAL_PAY_DT ");
                ht.Add("@FESTIVAL_PAY_DT", Convert.ToDateTime(festivalPayDT).ToString("yyyy/MM/dd"));
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
    //刪除 節金維護檔
    public void deleteDataMH(string festival_type, string festival_dt, string emp_cd, string festivalPayDT)
    {
        try
        {
            // string dt = Convert.ToDateTime(start_dt).ToString("yyyy/MM/dd HH:mm:ss.fff");
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("delete from TB_S_M_FESTIVAL_H ");
            //sb.Append(" where ENV_ALLOWANCE_TYPE = @ENV_ALLOWANCE_TYPE and Convert(char(10),START_DT,111) = @START_DT ");
            sb.Append(" where FESTIVAL_TYPE = @FESTIVAL_TYPE ");
            sb.Append(" and FESTIVAL_DT = @FESTIVAL_DT ");
            sb.Append(" and EMP_CD = @EMP_CD ");
            sb.Append(" and FESTIVAL_PAY_DT = @FESTIVAL_PAY_DT ");
            ht.Add("@FESTIVAL_TYPE", festival_type);
            ht.Add("@FESTIVAL_DT", Convert.ToDateTime(festival_dt).ToString("yyyy/MM/dd"));
            ht.Add("@EMP_CD", emp_cd);
            ht.Add("@FESTIVAL_PAY_DT", Convert.ToDateTime(festivalPayDT).ToString("yyyy/MM/dd"));
            //ht.Add("@START_DT", start_dt);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //刪除 節金明細主檔
    public void deleteDataRD(string festival_type, string festival_dt, string emp_cd, string festivalPayDT)
    {
        try
        {
            // string dt = Convert.ToDateTime(start_dt).ToString("yyyy/MM/dd HH:mm:ss.fff");
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("delete from TB_S_R_FESTIVAL_D ");
            //sb.Append(" where ENV_ALLOWANCE_TYPE = @ENV_ALLOWANCE_TYPE and Convert(char(10),START_DT,111) = @START_DT ");
            sb.Append(" where FESTIVAL_TYPE = @FESTIVAL_TYPE ");
            sb.Append(" and FESTIVAL_DT = @FESTIVAL_DT ");
            sb.Append(" and EMP_CD = @EMP_CD ");
            sb.Append(" and FESTIVAL_PAY_DT = @FESTIVAL_PAY_DT ");
            ht.Add("@FESTIVAL_TYPE", festival_type);
            ht.Add("@FESTIVAL_DT", Convert.ToDateTime(festival_dt).ToString("yyyy/MM/dd"));
            ht.Add("@EMP_CD", emp_cd);
            ht.Add("@FESTIVAL_PAY_DT", Convert.ToDateTime(festivalPayDT).ToString("yyyy/MM/dd"));
            //ht.Add("@START_DT", start_dt);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //刪除 節金明細維護檔
    public void deleteDataMD(string festival_type, string festival_dt, string emp_cd, string festivalPayDT)
    {
        try
        {
            // string dt = Convert.ToDateTime(start_dt).ToString("yyyy/MM/dd HH:mm:ss.fff");
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("delete from TB_S_M_FESTIVAL_D ");
            //sb.Append(" where ENV_ALLOWANCE_TYPE = @ENV_ALLOWANCE_TYPE and Convert(char(10),START_DT,111) = @START_DT ");
            sb.Append(" where FESTIVAL_TYPE = @FESTIVAL_TYPE ");
            sb.Append(" and FESTIVAL_DT = @FESTIVAL_DT ");
            sb.Append(" and EMP_CD = @EMP_CD ");
            sb.Append(" and FESTIVAL_PAY_DT = @FESTIVAL_PAY_DT ");
            ht.Add("@FESTIVAL_TYPE", festival_type);
            ht.Add("@FESTIVAL_DT", Convert.ToDateTime(festival_dt).ToString("yyyy/MM/dd"));
            ht.Add("@EMP_CD", emp_cd);
            ht.Add("@FESTIVAL_PAY_DT", Convert.ToDateTime(festivalPayDT).ToString("yyyy/MM/dd"));
            //ht.Add("@START_DT", start_dt);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //刪除 節金條件檔 deleteDataMCAL
    public void deleteDataMCAL(string festival_type, string festival_dt, string emp_cd, string festivalPayDT)
    {
        try
        {
            // string dt = Convert.ToDateTime(start_dt).ToString("yyyy/MM/dd HH:mm:ss.fff");
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("delete from TB_S_M_FESTIVAL_CAL ");
            //sb.Append(" where ENV_ALLOWANCE_TYPE = @ENV_ALLOWANCE_TYPE and Convert(char(10),START_DT,111) = @START_DT ");
            sb.Append(" where FESTIVAL_TYPE = @FESTIVAL_TYPE ");
            sb.Append(" and FESTIVAL_DT = @FESTIVAL_DT ");
            sb.Append(" and EMP_CD = @EMP_CD ");
            sb.Append(" and FESTIVAL_PAY_DT = @FESTIVAL_PAY_DT ");
            ht.Add("@FESTIVAL_TYPE", festival_type);
            ht.Add("@FESTIVAL_DT", Convert.ToDateTime(festival_dt).ToString("yyyy/MM/dd"));
            ht.Add("@EMP_CD", emp_cd);
            ht.Add("@FESTIVAL_PAY_DT", Convert.ToDateTime(festivalPayDT).ToString("yyyy/MM/dd"));
            //ht.Add("@START_DT", start_dt);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //刪除 節金條件檔 deleteDataMCAL
    public void deleteDataDtl(string festival_type, string festival_dt, string emp_cd, string festivalPayDT, string festival_sql)
    {
        try
        {
            // string dt = Convert.ToDateTime(start_dt).ToString("yyyy/MM/dd HH:mm:ss.fff");
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("delete from TB_S_M_FESTIVAL_CAL ");
            sb.Append(" where FESTIVAL_TYPE = @FESTIVAL_TYPE ");
            sb.Append(" and FESTIVAL_DT = @FESTIVAL_DT ");
            sb.Append(" and EMP_CD = @EMP_CD ");
            sb.Append(" and FESTIVAL_PAY_DT = @FESTIVAL_PAY_DT ");
            sb.Append(" and FESTIVAL_SQL_COMMAND = @FESTIVAL_SQL_COMMAND ");
            ht.Add("@FESTIVAL_TYPE", festival_type);
            ht.Add("@FESTIVAL_DT", Convert.ToDateTime(festival_dt).ToString("yyyy/MM/dd"));
            ht.Add("@EMP_CD", emp_cd);
            ht.Add("@FESTIVAL_PAY_DT", Convert.ToDateTime(festivalPayDT).ToString("yyyy/MM/dd"));
            ht.Add("@FESTIVAL_SQL_COMMAND", festival_sql);
            //ht.Add("@START_DT", start_dt);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }



    //修改
    public void updateData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            DateTime now = DateTime.Parse(DateTime.Now.ToString());

            sb.Append(" update TB_S_M_FESTIVAL_H ");
            sb.Append(" set FESTIVAL_DESC = @FESTIVAL_DESC ");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT = @UPDATED_DT");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where FESTIVAL_TYPE = @FESTIVAL_TYPE ");
            sb.Append(" and FESTIVAL_DT = @FESTIVAL_DT ");
            sb.Append(" and EMP_CD = @EMP_CD ");
            sb.Append(" and FESTIVAL_PAY_DT = @FESTIVAL_PAY_DT ");

            ht.Add("@FESTIVAL_DESC", FESTIVAL_DESC);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@UPDATED_DT", now);
            ht.Add("@FUNC_ID", FUNC_ID);


            //PK值
            ht.Add("@FESTIVAL_TYPE", FESTIVAL_TYPE);
            ht.Add("@FESTIVAL_DT", Convert.ToDateTime(FESTIVAL_DT).ToString("yyyy/MM/dd"));
            ht.Add("@EMP_CD", EMP_CD);
            ht.Add("@FESTIVAL_PAY_DT", Convert.ToDateTime(FESTIVAL_PAY_DT).ToString("yyyy/MM/dd"));


            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }


    }

    //新增(節金維護檔)
    internal void insertData()
    {
        try
        {
            DateTime now = DateTime.Parse(DateTime.Now.ToString());
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" INSERT INTO TB_S_M_FESTIVAL_H ");
            sb.Append(" ( ");
            sb.Append(" FESTIVAL_TYPE,FESTIVAL_DT,EMP_CD,FESTIVAL_PAY_DT,FESTIVAL_DESC ");
            sb.Append(" ,CREATED_BY, CREATED_DT, UPDATED_BY, UPDATED_DT, FUNC_ID  ");
            sb.Append(" ) ");
            sb.Append(" VALUES ");
            sb.Append(" ( @FESTIVAL_TYPE,  @FESTIVAL_DT,  @EMP_CD,  @FESTIVAL_PAY_DT,  @FESTIVAL_DESC  ");

            sb.Append("   ,@CREATED_BY,  @CREATED_DT,  @UPDATED_BY,  @UPDATED_DT,  @FUNC_ID )");

            ht.Add("@FESTIVAL_TYPE", FESTIVAL_TYPE);
            ht.Add("@FESTIVAL_DT", Convert.ToDateTime(FESTIVAL_DT));
            ht.Add("@FESTIVAL_PAY_DT", Convert.ToDateTime(FESTIVAL_PAY_DT));
            ht.Add("@EMP_CD", EMP_CD);
            ht.Add("@FESTIVAL_DESC", FESTIVAL_DESC);

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

    //新增(節金條件檔)
    internal void insertDataCAL()
    {
        try
        {
            DateTime now = DateTime.Parse(DateTime.Now.ToString());
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" INSERT INTO TB_S_M_FESTIVAL_CAL ");
            sb.Append(" ( ");
            sb.Append(" FESTIVAL_TYPE, FESTIVAL_DT, EMP_CD, FESTIVAL_PAY_DT, FESTIVAL_LOGIC ");
            sb.Append(" ,CALCULATE_ITEM, CALCULATE_COND, CALCULATE_CONTENT1, CALCULATE_CONTENT2, FESTIVAL_SQL_COMMAND ");
            sb.Append(" ,CREATED_BY, CREATED_DT, UPDATED_BY, UPDATED_DT, FUNC_ID  ");
            sb.Append(" ) ");
            sb.Append(" VALUES ");
            sb.Append(" ( @FESTIVAL_TYPE,  @FESTIVAL_DT,  @EMP_CD,  @FESTIVAL_PAY_DT,  @FESTIVAL_LOGIC  ");
            sb.Append(" , @CALCULATE_ITEM,  @CALCULATE_COND,  @CALCULATE_CONTENT1,  @CALCULATE_CONTENT2,  @FESTIVAL_SQL_COMMAND  ");
            sb.Append("   ,@CREATED_BY,  @CREATED_DT,  @UPDATED_BY,  @UPDATED_DT,  @FUNC_ID )");

            ht.Add("@FESTIVAL_TYPE", FESTIVAL_TYPE);
            ht.Add("@FESTIVAL_DT", Convert.ToDateTime(FESTIVAL_DT));
            ht.Add("@FESTIVAL_PAY_DT", Convert.ToDateTime(FESTIVAL_PAY_DT));
            ht.Add("@EMP_CD", EMP_CD);
            ht.Add("@FESTIVAL_LOGIC", FESTIVAL_LOGIC);

            ht.Add("@CALCULATE_ITEM", CALCULATE_ITEM);
            ht.Add("@CALCULATE_COND", CALCULATE_COND);
            ht.Add("@CALCULATE_CONTENT1", CALCULATE_CONTENT1);
            ht.Add("@CALCULATE_CONTENT2", CALCULATE_CONTENT2);
            ht.Add("@FESTIVAL_SQL_COMMAND", FESTIVAL_SQL_COMMAND);

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

    //呼叫節金對象生成SP
    internal void execSP_S_FESTIVAL_DATA()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_S_FESTIVAL_DATA");
            ht.Add("@FESTIVAL_TYPE", FESTIVAL_TYPE);
            ht.Add("@FESTIVAL_DT", Convert.ToDateTime(FESTIVAL_DT).ToString("yyyy/MM/dd"));
            ht.Add("@FESTIVAL_PAY_DT", Convert.ToDateTime(FESTIVAL_PAY_DT).ToString("yyyy/MM/dd"));
            ht.Add("@EMP_CD", EMP_CD);
            ht.Add("@USERID", CREATED_BY);//CREATED_BY
            ht.Add("@FUNCID", "FB2SG020");
            dbConn.ExecuteSPT(sb, ht, true);

        }
        catch (Exception)
        {

            throw;
        }

    }

    #endregion


    #region 檔案上傳

    //取得該員工相關資料
    internal void getEmpData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select * from VW_H_EMP_DATA ");
            sb.Append(" where EMP_ID = @EMP_ID ");
            ht.Add("@EMP_ID", EMP_ID);
            DataTable dt = dbConn.Query(sb, ht);

            foreach (DataRow dr in dt.Rows)
            {
                this.EMP_NAME = Convert.ToString(dr["EMP_NAME"]);
                this.DEPT_NO = Convert.ToString(dr["DEPT_NO"]);
                this.PLANT_CD = Convert.ToString(dr["PLANT_CD"]);
                this.JPN_CD = Convert.ToString(dr["JPN_CD"]);
                this.COMPANY_CD = Convert.ToString(dr["COMPANY_CD"]);
                this.LEVEL_CD = Convert.ToString(dr["LEVEL_CD"]);
                this.GRADE_CD = Convert.ToString(dr["GRADE_CD"]);
                this.PJOB_CD = Convert.ToString(dr["PJOB_CD"]);
                this.WORK_YEARS = Convert.ToString(dr["WORK_YEARS"]);
                this.JOIN_DT = Convert.ToString(dr["JOIN_DT"]);
               // this.WORK_DAYS = Convert.ToString(dr["WORK_DAYS"]);
                this.EMP_CD = Convert.ToString(dr["EMP_CD"]);
                this.EMP_CHG_CD = Convert.ToString(dr["EMP_CHG_CD"]);
                this.WS_CD = Convert.ToString(dr["WS_CD"]);
                this.SEX_CD = Convert.ToString(dr["SEX_CD"]);
            }



        }
        catch (Exception)
        {

            throw;
        }
    }

    //判斷工號是否存在
    internal DataTable getEmpCount(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select count(0) resultCount from VW_H_EMP_DATA ");
            sb.Append(" where EMP_ID = @EMP_ID ");
            ht.Add("@EMP_ID", emp_id);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //判斷工號是否存在 節金明細維護檔
    internal DataTable getFestivalEmpCountFromTemp(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select count(0) resultCount from TB_S_M_FESTIVAL_D_TEMP ");
            sb.Append(" where  ");
            sb.Append("   EMP_ID = @EMP_ID ");
            sb.Append(" and FESTIVAL_TYPE = @FESTIVAL_TYPE ");
            sb.Append(" and  FESTIVAL_DT = @FESTIVAL_DT ");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@FESTIVAL_TYPE", FESTIVAL_TYPE);
            ht.Add("@FESTIVAL_DT", FESTIVAL_DT);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //刪除工號- 節金明細維護檔
    internal void deleteFestivalEmp(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" delete from TB_S_M_FESTIVAL_D ");
            sb.Append(" where  ");
            sb.Append("   EMP_ID = @EMP_ID ");
            sb.Append(" and FESTIVAL_TYPE = @FESTIVAL_TYPE ");
            sb.Append(" and  FESTIVAL_DT = @FESTIVAL_DT ");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@FESTIVAL_TYPE", FESTIVAL_TYPE);
            ht.Add("@FESTIVAL_DT", FESTIVAL_DT);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }


    //建立temp 節金明細維護檔
    internal void createFestivaltemp()
    {

        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        try
        {
          
            sb.Append(" select * into TB_S_M_FESTIVAL_D_TEMP 	from TB_S_M_FESTIVAL_D 	 ");
            sb.Append(" where  1=1 ");
            sb.Append(" and FESTIVAL_TYPE = @FESTIVAL_TYPE ");
            sb.Append(" and  FESTIVAL_DT = @FESTIVAL_DT ");
            ht.Add("@FESTIVAL_TYPE", FESTIVAL_TYPE);
            ht.Add("@FESTIVAL_DT", FESTIVAL_DT);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            sb.Clear();
            ht.Clear();
            sb.Append(" delete  from TB_S_M_FESTIVAL_D_TEMP 	 ");
            sb.Append(" where  1=1 ");
            dbConn.ExecuteT(sb, ht, true);
        }
    }

    //刪除temp 節金明細維護檔
    internal void dropFestivaltemp()
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        try
        {
            sb.Append("drop table TB_S_M_FESTIVAL_D_TEMP 	 ");

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            //createFestivaltemp();
        }
    }
    //判斷支付狀態是否存在
    internal DataTable getPayTypeCount(string subCD)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select count(0) resultCount from  TB_9_M_COMM_D ");
            sb.Append(" where  MAIN_CD = 'PAY_TYPE'   ");
            sb.Append(" and SYS_CD='SC'  ");
            sb.Append(" and IS_VALID='Y'   ");
            sb.Append(" and SUB_CD=@SUB_CD   ");
            ht.Add("@SUB_CD", subCD);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }


    //取得參數檔-獎金類所得稅率
    internal DataTable getTaxRate()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select CONVERT(decimal(3,2),CODE_VAL1)  as taxRate  from TB_9_M_PARAMETER ");
            sb.Append(" where SYS_CD='SL' ");
            sb.Append(" and MAIN_CD='BOUNS_TAX_RATE' ");
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //取得參數檔-所得稅代扣金額下限
    internal DataTable getLimitLow()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select CONVERT(decimal(7),CODE_VAL1) as limitLow  from TB_9_M_PARAMETER ");
            sb.Append(" where SYS_CD='SL' ");
            sb.Append(" and MAIN_CD='INCOME_LIMIT_LOW' ");
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //新增節金明細維護檔(檔案上傳)
    internal void insertTarget(DateTime now)
    {
        try
        {
            
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("insert into TB_S_M_FESTIVAL_D");
            sb.Append(" ( ");
            sb.Append("   FESTIVAL_TYPE, FESTIVAL_DT, FESTIVAL_PAY_DT, EMP_ID, EMP_NAME ");
            sb.Append(" , DEPT_NO, PLANT_CD, JPN_CD, COMPANY_CD, LEVEL_CD ");
            sb.Append(" , GRADE_CD, PJOB_CD, JOIN_DT, WORK_DAYS, EMP_CD ");
            sb.Append(" , EMP_CHG_CD, WS_CD, SEX_CD, FESTIVAL_AMT, FESTIVAL_TAX ");
            sb.Append(" , FESTIVAL_AMT_R, PAY_TYPE, APPROVE_FLAG, CHG_STATUS, WORK_YEARS ");
            sb.Append(" , LEVEL_PAY, ABILITY_PAY, PJOB_PAY, PROFESSION_PAY, FOOD_SUBSIDY ");
            sb.Append(" , CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID ");
            sb.Append(" ) ");
            sb.Append(" VALUES ");
            sb.Append(" (@FESTIVAL_TYPE, @FESTIVAL_DT, @FESTIVAL_PAY_DT, @EMP_ID,@EMP_NAME  ");
            sb.Append(" ,@DEPT_NO,@PLANT_CD, @JPN_CD, @COMPANY_CD, @LEVEL_CD ");
            sb.Append(" ,@GRADE_CD, @PJOB_CD, @JOIN_DT, @WORK_DAYS,@EMP_CD  ");
            sb.Append(" ,@EMP_CHG_CD,@WS_CD,@SEX_CD,@FESTIVAL_AMT,@FESTIVAL_TAX  ");
            sb.Append(" ,@FESTIVAL_AMT_R,@PAY_TYPE,@APPROVE_FLAG,@CHG_STATUS, @WORK_YEARS  ");
            sb.Append(" ,@LEVEL_PAY,@ABILITY_PAY, @PJOB_PAY, @PROFESSION_PAY, @FOOD_SUBSIDY ");
            sb.Append(" ,@CREATED_BY,  @CREATED_DT,  @UPDATED_BY,  @UPDATED_DT,  @FUNC_ID )");

            ht.Add("@FESTIVAL_TYPE", FESTIVAL_TYPE);
            ht.Add("@FESTIVAL_DT", Convert.ToDateTime(FESTIVAL_DT));
            ht.Add("@FESTIVAL_PAY_DT", Convert.ToDateTime(FESTIVAL_PAY_DT));
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@EMP_NAME", EMP_NAME);

            ht.Add("@DEPT_NO", DEPT_NO);
            ht.Add("@PLANT_CD", PLANT_CD);
            ht.Add("@JPN_CD", JPN_CD);
            ht.Add("@COMPANY_CD", COMPANY_CD);
            ht.Add("@LEVEL_CD", LEVEL_CD);

            ht.Add("@GRADE_CD", GRADE_CD);
            ht.Add("@PJOB_CD", PJOB_CD);
            ht.Add("@JOIN_DT", Convert.ToDateTime(JOIN_DT));
            ht.Add("@WORK_DAYS", WORK_DAYS);
            ht.Add("@EMP_CD", EMP_CD);

            ht.Add("@EMP_CHG_CD", EMP_CHG_CD);
            ht.Add("@WS_CD", WS_CD);
            ht.Add("@SEX_CD", SEX_CD);
            ht.Add("@FESTIVAL_AMT", FESTIVAL_AMT);
            ht.Add("@FESTIVAL_TAX", FESTIVAL_TAX);

            ht.Add("@FESTIVAL_AMT_R", FESTIVAL_AMT_R);
            ht.Add("@PAY_TYPE", PAY_TYPE);
            ht.Add("@APPROVE_FLAG", "N");
            ht.Add("@CHG_STATUS", "G");
            ht.Add("@WORK_YEARS", WORK_YEARS);

            ht.Add("@LEVEL_PAY", LEVEL_PAY);
            ht.Add("@ABILITY_PAY", ABILITY_PAY);
            ht.Add("@PJOB_PAY", PJOB_PAY);
            ht.Add("@PROFESSION_PAY", PROFESSION_PAY);
            ht.Add("@FOOD_SUBSIDY", FOOD_SUBSIDY);

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


    //刪除 節金明細維護檔(檔案上傳)
    public void deleteTarget()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("delete from TB_S_M_FESTIVAL_D ");
            sb.Append(" where FESTIVAL_TYPE = @FESTIVAL_TYPE  ");
            sb.Append(" and FESTIVAL_DT = @FESTIVAL_DT  ");
            sb.Append(" and FESTIVAL_PAY_DT = @FESTIVAL_PAY_DT ");
            sb.Append(" and EMP_CD = @EMP_CD ");
            ht.Add("@FESTIVAL_TYPE", FESTIVAL_TYPE);
            ht.Add("@FESTIVAL_DT", FESTIVAL_DT);
            ht.Add("@FESTIVAL_PAY_DT", FESTIVAL_PAY_DT);
            ht.Add("@EMP_CD", EMP_CD_PK);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }


    //更新節金維護檔(檔案上傳)
    public void updateTarget_H(int amt_total,int num_total,DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_S_M_FESTIVAL_H ");
            sb.Append(" set ");
            sb.Append(" FESTIVAL_TOTAL_AMT = @FESTIVAL_TOTAL_AMT ");
            sb.Append(" ,FESTIVAL_TOTAL_NUM = @FESTIVAL_TOTAL_NUM ");
            sb.Append(" ,TARGET_GEN_DT = @TARGET_GEN_DT ");
            sb.Append(" ,APPROVE_STATUS = @APPROVE_STATUS ");
            sb.Append(" ,REMARK = @REMARK ");
            sb.Append(" ,RELEASE_DT = @RELEASE_DT ");
            sb.Append(" ,RELEASE_BY = @RELEASE_BY ");
            sb.Append(" ,APPROVE_DT = @APPROVE_DT ");
            sb.Append(" ,APPROVE_BY = @APPROVE_BY ");
            sb.Append(" ,PROCESS_STATUS = @PROCESS_STATUS ");
            sb.Append(" ,SALARY_DT = @SALARY_DT ");
            sb.Append(" ,FREEZE_FLAG = @FREEZE_FLAG ");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT = @UPDATED_DT");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where FESTIVAL_TYPE = @FESTIVAL_TYPE ");
            sb.Append("  and FESTIVAL_DT = @FESTIVAL_DT");
            sb.Append("  and FESTIVAL_PAY_DT = @FESTIVAL_PAY_DT");
            sb.Append("  and EMP_CD = @EMP_CD");

            //set值
            ht.Add("@FESTIVAL_TOTAL_AMT", amt_total);
            ht.Add("@FESTIVAL_TOTAL_NUM", num_total);
            ht.Add("@TARGET_GEN_DT", DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd") ));
            ht.Add("@APPROVE_STATUS", "N");
            ht.Add("@REMARK", "");
            ht.Add("@RELEASE_DT", DBNull.Value);
            ht.Add("@RELEASE_BY", "");
            ht.Add("@APPROVE_DT", DBNull.Value);
            ht.Add("@APPROVE_BY", "");
            ht.Add("@PROCESS_STATUS", "N");
            ht.Add("@SALARY_DT", DBNull.Value);
            ht.Add("@FREEZE_FLAG", "N");

            //PK值
            ht.Add("@FESTIVAL_TYPE", FESTIVAL_TYPE);
            ht.Add("@FESTIVAL_DT", FESTIVAL_DT);
            ht.Add("@FESTIVAL_PAY_DT", FESTIVAL_PAY_DT);
            ht.Add("@EMP_CD", EMP_CD_PK);
            //新修日期
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