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
public class CFB2SH3400DAO : BaseDAO
{
    //年獎維護檔
    public string AWARD_YEAR { get; set; }
    public string AWARD_ROUND { get; set; }
    public string AWARD_ROUND_DESC { get; set; }
    public string AWARD_DAYS { get; set; }
    public string AWARD_DT { get; set; }
    public string AWARD_STIME { get; set; }
    public string AWARD_ETIME { get; set; }
    public string AWARD_ITEM_A { get; set; }
    public string AWARD_ITEM_RP { get; set; }
    public string AWARD_ITEM_AL { get; set; }
    public string AWARD_ITEM_D { get; set; }
    public string TARGET_GEN_DT { get; set; }
    public string AWARD_TOTAL_PEOPLE { get; set; }
    public string GEN_DT { get; set; }
    public string AWARD_TOTAL_AMOUNT { get; set; }
    public string RELEASE_DT { get; set; }
    public string RELEASE_BY { get; set; }
    public string APPROVE_DT { get; set; }
    public string APPROVE_BY { get; set; }
    public string APPROVE_STATUS { get; set; }
    public string REMARK { get; set; }
    public string SALARY_TRANS_DT { get; set; }
    public string SALARY_TRANS_BY { get; set; }
    public string PROCESS_STATUS { get; set; }
    public string SALARY_DT { get; set; }
    public string FREEZE_FLAG { get; set; }


    //年獎明細維護檔
    public string EMP_ID { get; set; }
    public string APPROVE_FLAG { get; set; }

    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }


    public CFB2SH3400DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }


    #region 資料取得
    //判斷登入者是否為提出核可者的直屬長官
    public DataTable isDirectHeadEmp()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select count(a.RELEASE_BY) resultCount ");
            sb.Append(" from TB_S_M_FR_AWARD_H a ");
            sb.Append(" left join VW_H_EMP_DATA b on a.RELEASE_BY = b.EMP_ID ");
            sb.Append(" left join TB_H_R_HEAD_DEPT c on b.DEPT_NO=c.MNG_DEPT_NO ");
            sb.Append(" where 1=1 ");
            sb.Append(" and a.AWARD_YEAR = @AWARD_YEAR  ");
            sb.Append(" and a.AWARD_ROUND = @AWARD_ROUND  ");
            sb.Append(" and c.EMP_ID = @EMP_ID  ");

            ht.Add("@AWARD_YEAR", AWARD_YEAR);
            ht.Add("@AWARD_ROUND", AWARD_ROUND);
            ht.Add("@EMP_ID", SessionHandle.Current.emp_id);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //判斷登入者是否為提出核可者的本身
    public DataTable isSelfLogin()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select count(a.RELEASE_BY) resultCount ");
            sb.Append(" from TB_S_M_FR_AWARD_H a ");
            sb.Append(" where 1=1 ");
            sb.Append(" and a.AWARD_YEAR = @AWARD_YEAR  ");
            sb.Append(" and a.AWARD_ROUND = @AWARD_ROUND  ");
            sb.Append(" and a.RELEASE_BY = @EMP_ID  ");

            ht.Add("@AWARD_YEAR", AWARD_YEAR);
            ht.Add("@AWARD_ROUND", AWARD_ROUND);
            ht.Add("@EMP_ID", SessionHandle.Current.emp_id);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }


    //查詢明細表頭部分
    public void getTitleData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" select  a.*   ");
            sb.Append(" , a.APPROVE_STATUS + '-' + c.SUB_DESC APPROVE_STATUS_DESC   ");
            sb.Append(" from TB_S_M_FR_AWARD_H a ");
            sb.Append("  left join TB_9_M_COMM_D c on  a.APPROVE_STATUS = c.SUB_CD and c.MAIN_CD = 'APPROVE_STATUS'  and c.SYS_CD='SA' and c.IS_VALID='Y' ");
            sb.Append(" where 1=1 ");
            if (AWARD_YEAR != "")
            {
                sb.Append(" and AWARD_YEAR = @AWARD_YEAR ");
                ht.Add("@AWARD_YEAR", AWARD_YEAR);
            }
            

            DataTable dt = dbConn.Query(sb, ht);
            foreach (DataRow dr in dt.Rows)
            {

               
                ///this.AWARD_DAYS = Convert.ToString(dr["AWARD_DAYS"]);
                this.AWARD_DT = dr["AWARD_DT"].ToString() != "" ? Convert.ToDateTime(dr["AWARD_DT"].ToString()).ToString("yyyy/MM/dd") : "";
                this.AWARD_TOTAL_AMOUNT = Convert.ToString(dr["AWARD_TOTAL_AMOUNT"]);
                this.AWARD_TOTAL_PEOPLE = Convert.ToString(dr["AWARD_TOTAL_PEOPLE"]);
                this.REMARK = Convert.ToString(dr["REMARK"]);

                this.RELEASE_DT = dr["RELEASE_DT"].ToString() != "" ? Convert.ToDateTime(dr["RELEASE_DT"].ToString()).ToString("yyyy/MM/dd") : "";
                //this.SALARY_TRANS_DT = dr["SALARY_TRANS_DT"].ToString() != "" ? Convert.ToDateTime(dr["SALARY_TRANS_DT"].ToString()).ToString("yyyy/MM/dd") : "";
                this.APPROVE_STATUS = Convert.ToString(dr["APPROVE_STATUS"]);
                this.FREEZE_FLAG = Convert.ToString(dr["FREEZE_FLAG"]);
            }

        }
        catch
        {
            throw;
        }

    }



    #endregion

    #region Gridview 資料
    //Gridview 查詢資料
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression
                            , string awardYear_S, string awardYear_E, string awardRound
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
            sb.Append(" AWARD_YEAR,AWARD_DT, RELEASE_DT, APPROVE_DT, TARGET_GEN_DT  ");
            sb.Append(" ,  APPROVE_STATUS, FREEZE_FLAG, RELEASE_BY, APPROVE_BY   ");
            sb.Append(" , APPROVE_STATUS + '-' + c.SUB_DESC APPROVE_STATUS_DESC   ");
            sb.Append(" ,  isnull(d.EMP_NAME,'')   RELEASE_BY_NAME   ");
            sb.Append(" ,  isnull(e.EMP_NAME,'')   APPROVE_BY_NAME   ");
            sb.Append(" from TB_S_M_FR_AWARD_H a ");
            sb.Append("  left join TB_9_M_COMM_D c on  a.APPROVE_STATUS = c.SUB_CD and c.MAIN_CD = 'APPROVE_STATUS' and c.IS_VALID='Y' and c.SYS_CD='SA' ");
            sb.Append("  left join  VW_H_EMP_DATA d on a.RELEASE_BY = d.EMP_ID ");
            sb.Append("  left join  VW_H_EMP_DATA e on a.APPROVE_BY = e.EMP_ID ");
            sb.Append(" where 1=1 ");

            //查詢條件-text
            if (awardYear_S != "")
            {
                sb.Append(" and AWARD_YEAR >= @AWARD_YEAR_S ");
                ht.Add("@AWARD_YEAR_S", awardYear_S);
            }
            if (awardYear_E != "")
            {
                sb.Append(" and AWARD_YEAR <= @AWARD_YEAR_E ");
                ht.Add("@AWARD_YEAR_E", awardYear_E);
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
                          , string awardYear_S, string awardYear_E, string awardRound
                        )
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) resultCount ");
            sb.Append(" from TB_S_M_FR_AWARD_H ");
            sb.Append(" where 1=1 ");

            //查詢條件-text
            if (awardYear_S != "")
            {
                sb.Append(" and AWARD_YEAR >= @AWARD_YEAR_S ");
                ht.Add("@AWARD_YEAR_S", awardYear_S);
            }
            if (awardYear_E != "")
            {
                sb.Append(" and AWARD_YEAR <= @AWARD_YEAR_E ");
                ht.Add("@AWARD_YEAR_E", awardYear_E);
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


    #region Gridview (查詢明細) 資料
    //Gridview 查詢資料
    public DataTable getDataDtl(int startRowIndex, int maximumRows, string sortExpression
                            , string award_year, string award_round, string emp_id, string emp_name
                           )
    {
        try
        {
            if (sortExpression.Contains("UPDATED_DT"))
                sortExpression = sortExpression.Replace("UPDATED_DT", "a.UPDATED_DT");
          

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + ") As RowNumber,");
            sb.Append(" a.*   ");
            sb.Append(" , a.CHG_STATUS + '-' + b.SUB_DESC CHG_STATUS_DESC   ");
            sb.Append(" , a.EMP_CHG_CD + '-' + d.SUB_DESC EMP_CHG_CD_DESC   ");
            sb.Append(" from TB_S_M_FR_AWARD_D a ");
            sb.Append("  left join TB_9_M_COMM_D b on  a.CHG_STATUS = b.SUB_CD and b.MAIN_CD = 'CHG_STATUS' and b.IS_VALID='Y'  and b.SYS_CD='SA' ");
            sb.Append("  left join TB_9_M_COMM_D d on  a.EMP_CHG_CD = d.SUB_CD and d.MAIN_CD = 'EMP_CHG_CD' and d.IS_VALID='Y' and d.SYS_CD='HB' ");
            sb.Append(" where 1=1 ");

            //若直接輸入網址不應該有查詢資料
            if (award_round == "" || award_year == "")
            {
                sb.Append(" and 1=2 ");
            }

            //查詢條件
            if (award_year != "")
            {
                sb.Append(" and a.AWARD_YEAR = @AWARD_YEAR ");
                ht.Add("@AWARD_YEAR", award_year);
            }
       
            if (emp_id.Trim() != "")
            {
                sb.Append(" and a.EMP_ID = @emp_id ");
                ht.Add("emp_id", emp_id);

            }
            if (emp_name.Trim() != "")
            {
                sb.Append(" and a.EMP_NAME like @emp_name ");
                ht.Add("emp_name", emp_name + "%");

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
    public int getCountDtl(int startRowIndex, int maximumRows
                          , string award_year, string award_round, string emp_id, string emp_name
                        )
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) resultCount ");
            sb.Append(" from TB_S_M_FR_AWARD_D ");
            sb.Append(" where 1=1 ");

            //若直接輸入網址不應該有查詢資料
            if (award_round == "" || award_year == "")
            {
                sb.Append(" and 1=2 ");
            }
            //查詢條件
            if (award_year != "")
            {
                sb.Append(" and AWARD_YEAR = @AWARD_YEAR ");
                ht.Add("@AWARD_YEAR", award_year);
            }
       
            if (emp_id.Trim() != "")
            {
                sb.Append(" and EMP_ID = @emp_id ");
                ht.Add("emp_id", emp_id);

            }
            if (emp_name.Trim() != "")
            {
                sb.Append(" and EMP_NAME like @emp_name ");
                ht.Add("emp_name", emp_name + "%");

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
    public void updateData()
    {
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

    #region 一括異常註記-(Dtl)

    //異常註記-update 備註說明  (考核資料維護檔 DTL)
    public void updateMarkData_H(DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_S_M_FR_AWARD_H ");
            sb.Append(" set REMARK = @REMARK ");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT = @UPDATED_DT");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where AWARD_YEAR = @AWARD_YEAR ");
            //set值
            ht.Add("@REMARK", REMARK);

            //新修日期
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@UPDATED_DT", now);
            ht.Add("@FUNC_ID", FUNC_ID);

            //PK值
            ht.Add("@AWARD_YEAR", AWARD_YEAR);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }
    //異常註記-update 異常註記為空白 或V (考核資料維護檔 DTL)
    public void updateMarkData_D(DateTime now, string approve_mark)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_S_M_FR_AWARD_D ");
            sb.Append(" set APPROVE_MARK = @APPROVE_MARK ");
            if (approve_mark != "")
            {
                sb.Append(" ,APPROVE_FLAG = @APPROVE_FLAG");
            }
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT = @UPDATED_DT");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where AWARD_YEAR = @AWARD_YEAR ");
            sb.Append("  and EMP_ID = @EMP_ID");
            ht.Add("@APPROVE_MARK", approve_mark);
            if (approve_mark != "")
            {
                ht.Add("@APPROVE_FLAG", "N");
            }
            //PK值
            ht.Add("@AWARD_YEAR", AWARD_YEAR);
            ht.Add("@EMP_ID", EMP_ID);


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

    //取得異常註記的筆數
    public int getMarkData()
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) resultCount ");
            sb.Append(" from TB_S_M_FR_AWARD_D  ");
            sb.Append(" where AWARD_YEAR = @AWARD_YEAR ");
            sb.Append("  and APPROVE_MARK = @APPROVE_MARK");
            //PK值
            ht.Add("@AWARD_YEAR", AWARD_YEAR);
            ht.Add("@APPROVE_MARK", "V");
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


    #region 核可DB存取(Dtl)
    //核可-回復成核可狀態  (年獎維護檔 DTL)
    public void updateApproveData_H(DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_S_M_FR_AWARD_H ");
            sb.Append(" set REMARK = @REMARK ");
            sb.Append(" ,APPROVE_DT = @APPROVE_DT");
            sb.Append(" ,APPROVE_BY = @APPROVE_BY");
            sb.Append(" ,APPROVE_STATUS = @APPROVE_STATUS");
            sb.Append(" ,FREEZE_FLAG = @FREEZE_FLAG");
            sb.Append(" ,AWARD_TOTAL_PPEOPLE=(select count(*) from TB_S_M_FR_AWARD_D where AWARD_YEAR=@AWARD_YEAR ) ");
            sb.Append(" ,AWARD_TOTAL_AMOUNT=(select isnull(sum(AWARD_AMT),0) from TB_S_M_FR_AWARD_D where AWARD_YEAR = @AWARD_YEAR) ");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT = @UPDATED_DT");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where AWARD_YEAR = @AWARD_YEAR ");

            //set值
            ht.Add("@REMARK", "");
            ht.Add("@APPROVE_DT", now);
            ht.Add("@APPROVE_BY", APPROVE_BY);
            ht.Add("@APPROVE_STATUS", "Y");
            ht.Add("@FREEZE_FLAG", "N");

            //新修日期
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@UPDATED_DT", now);
            ht.Add("@FUNC_ID", FUNC_ID);

            //PK值
            ht.Add("@AWARD_YEAR", AWARD_YEAR);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }


    //核可 更新-年獎明細維護檔
    public void updateAllApproveData_D(DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_S_M_FR_AWARD_D ");
            sb.Append(" set APPROVE_MARK = @APPROVE_MARK ");
            sb.Append(" ,APPROVE_FLAG = @APPROVE_FLAG");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT = @UPDATED_DT");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where AWARD_YEAR = @AWARD_YEAR ");
            //set值
            ht.Add("@APPROVE_MARK", "");
            ht.Add("@APPROVE_FLAG", "Y");

            //PK值
            ht.Add("@AWARD_YEAR", AWARD_YEAR);

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
    //核可 刪除-年獎明細維護檔異動狀態為D
    public void deleteStatusData_D()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("delete from TB_S_M_FR_AWARD_D ");
            sb.Append(" where AWARD_YEAR = @AWARD_YEAR ");
            sb.Append("   and CHG_STATUS = @CHG_STATUS");
            ht.Add("@CHG_STATUS", "D");
            //PK值
            ht.Add("@AWARD_YEAR", AWARD_YEAR);
            ht.Add("@AWARD_ROUND", AWARD_ROUND);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //核可,刪除-年獎明細主檔
    public void deleteApproveData_D_H(DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" delete from TB_S_M_FR_AWARD_D ");
            sb.Append(" where AWARD_YEAR = @AWARD_YEAR ");

            //PK值
            ht.Add("@AWARD_YEAR", AWARD_YEAR);
            ht.Add("@AWARD_ROUND", AWARD_ROUND);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    

    #endregion

    #region 駁回DB存取(Dtl)

    //駁回 更新-年獎明細維護檔
    public void updateRejectData_D(DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_S_M_FR_AWARD_D ");
            sb.Append(" set APPROVE_MARK = @APPROVE_MARK ");
            sb.Append(" ,APPROVE_FLAG = @APPROVE_FLAG");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT = @UPDATED_DT");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where AWARD_YEAR = @AWARD_YEAR ");
            sb.Append("  and EMP_ID = @EMP_ID");
            ht.Add("@APPROVE_MARK", "V");
            ht.Add("@APPROVE_FLAG", "N");

            //PK值
            ht.Add("@AWARD_YEAR", AWARD_YEAR);
            ht.Add("@EMP_ID", EMP_ID);


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

    //駁回 更新-(將全部的異常記常設為空白)
    public void updateAllRejectData_D(DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_S_M_FR_AWARD_D ");
            sb.Append(" set APPROVE_MARK = @APPROVE_MARK ");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT = @UPDATED_DT");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where AWARD_YEAR = @AWARD_YEAR ");
            ht.Add("@APPROVE_MARK", ""); //異常註常為空白

            //PK值
            ht.Add("@AWARD_YEAR", AWARD_YEAR);

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

    //駁回-回復成未核可前狀態  (年獎維護檔 DTL)
    public void updateRejectData_H(DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_S_M_FR_AWARD_H ");
            sb.Append(" set REMARK = @REMARK ");
            sb.Append(" ,RELEASE_DT = @RELEASE_DT");
            sb.Append(" ,RELEASE_BY = @RELEASE_BY");
            sb.Append(" ,APPROVE_DT = @APPROVE_DT");
            sb.Append(" ,APPROVE_BY = @APPROVE_BY");
            sb.Append(" ,APPROVE_STATUS = @APPROVE_STATUS");
            sb.Append(" ,FREEZE_FLAG = @FREEZE_FLAG");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT = @UPDATED_DT");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where AWARD_YEAR = @AWARD_YEAR ");
            //set值
            ht.Add("@REMARK", REMARK);
            ht.Add("@RELEASE_DT", DBNull.Value);
            ht.Add("@RELEASE_BY", RELEASE_BY);
            ht.Add("@APPROVE_DT", DBNull.Value);
            ht.Add("@APPROVE_BY", APPROVE_BY);
            ht.Add("@APPROVE_STATUS", "B");//B:駁回
            ht.Add("@FREEZE_FLAG", "N");

            //新修日期
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@UPDATED_DT", now);
            ht.Add("@FUNC_ID", FUNC_ID);

            //PK值
            ht.Add("@AWARD_YEAR", AWARD_YEAR);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    #endregion

    #region 資料比對及EXCEL下載
    //判斷是否已有核可的明細主檔資料
    public DataTable getPreDataCount()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select count(0) resultCount ");
            sb.Append(" from TB_S_M_FR_AWARD_D a ");
            sb.Append(" where 1=1 ");
            sb.Append(" and a.AWARD_YEAR = @AWARD_YEAR  ");
            sb.Append(" and a.AWARD_ROUND = @AWARD_ROUND  ");
            ht.Add("@AWARD_YEAR", AWARD_YEAR);
            ht.Add("@AWARD_ROUND", AWARD_ROUND);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //取得新增的對象資料(原始資料[original]、前次核可[prev])
    public DataTable getAddExcelData(string data)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            if (data == "prev")
            {
                sb.Append(" select  a.*                       ");
                sb.Append(" , a.CHG_STATUS + '-' + b.SUB_DESC CHG_STATUS_DESC   ");
                sb.Append(" , a.EMP_CD + '-' + c.SUB_DESC EMP_CD_DESC   ");
                sb.Append(" , a.EMP_CHG_CD + '-' + d.SUB_DESC EMP_CHG_CD_DESC   ");
                sb.Append(" , a.PAY_TYPE + '-' + e.SUB_DESC PAY_TYPE_DESC       ");
                sb.Append(" from TB_S_M_FR_AWARD_D a             ");
                sb.Append("  left join TB_9_M_COMM_D b on  a.CHG_STATUS = b.SUB_CD and b.MAIN_CD = 'CHG_STATUS' and b.IS_VALID='Y'  and b.SYS_CD='SA'   ");
                sb.Append("  left join TB_9_M_COMM_D c on  a.EMP_CD = c.SUB_CD and c.MAIN_CD = 'EMP_CD' and b.IS_VALID='Y' and c.SYS_CD='HB' ");
                sb.Append("  left join TB_9_M_COMM_D d on  a.EMP_CHG_CD = d.SUB_CD and d.MAIN_CD = 'EMP_CHG_CD' and d.IS_VALID='Y' and d.SYS_CD='HB'    ");
                sb.Append("  left join TB_9_M_COMM_D e on  a.PAY_TYPE = e.SUB_CD and e.MAIN_CD = 'PAY_TYPE' and e.IS_VALID='Y'  and e.SYS_CD='SC'       ");
                sb.Append(" WHERE not exists (                ");
                sb.Append(" select * from TB_S_M_FR_AWARD_DM f   ");
                sb.Append(" where                             ");
                sb.Append(" AWARD_YEAR = @AWARD_YEAR          ");
                sb.Append(" and AWARD_ROUND = @AWARD_ROUND    ");
                sb.Append(" and a.EMP_ID = f.EMP_ID           ");
                sb.Append(" )and                              ");
                sb.Append(" a.AWARD_YEAR = @AWARD_YEAR AND    ");
                sb.Append(" a.AWARD_ROUND = @AWARD_ROUND      ");
            }
            else if (data == "original")
            {
                sb.Append(" select  a.*                       ");
                sb.Append(" , a.CHG_STATUS + '-' + b.SUB_DESC CHG_STATUS_DESC   ");
                sb.Append(" , a.EMP_CD + '-' + c.SUB_DESC EMP_CD_DESC   ");
                sb.Append(" , a.EMP_CHG_CD + '-' + d.SUB_DESC EMP_CHG_CD_DESC   ");
                sb.Append(" , a.PAY_TYPE + '-' + e.SUB_DESC PAY_TYPE_DESC       ");
                sb.Append(" from TB_S_M_FR_AWARD_DM a             ");
                sb.Append("  left join TB_9_M_COMM_D b on  a.CHG_STATUS = b.SUB_CD and b.MAIN_CD = 'CHG_STATUS' and b.IS_VALID='Y'  and b.SYS_CD='SA'   ");
                sb.Append("  left join TB_9_M_COMM_D c on  a.EMP_CD = c.SUB_CD and c.MAIN_CD = 'EMP_CD' and b.IS_VALID='Y' and c.SYS_CD='HB' ");
                sb.Append("  left join TB_9_M_COMM_D d on  a.EMP_CHG_CD = d.SUB_CD and d.MAIN_CD = 'EMP_CHG_CD' and d.IS_VALID='Y' and d.SYS_CD='HB'    ");
                sb.Append("  left join TB_9_M_COMM_D e on  a.PAY_TYPE = e.SUB_CD and e.MAIN_CD = 'PAY_TYPE' and e.IS_VALID='Y'  and e.SYS_CD='SC'       ");
                sb.Append(" WHERE not exists (                ");
                sb.Append(" select * from TB_S_S_AWARD_D  f   ");
                sb.Append(" where                             ");
                sb.Append(" AWARD_YEAR = @AWARD_YEAR          ");
                sb.Append(" and AWARD_ROUND = @AWARD_ROUND    ");
                sb.Append(" and a.EMP_ID = f.EMP_ID           ");
                sb.Append(" )and                              ");
                sb.Append(" a.AWARD_YEAR = @AWARD_YEAR AND    ");
                sb.Append(" a.AWARD_ROUND = @AWARD_ROUND      ");

            }

            //PK值
            ht.Add("@AWARD_YEAR", AWARD_YEAR);
            ht.Add("@AWARD_ROUND", AWARD_ROUND);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得刪除的對象資料(原始資料[original]、前次核可[prev])
    public DataTable getDelExcelData(string data)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            if (data == "prev")
            {
                sb.Append(" select  a.*                       ");
                sb.Append(" , a.CHG_STATUS + '-' + b.SUB_DESC CHG_STATUS_DESC   ");
                sb.Append(" , a.EMP_CD + '-' + c.SUB_DESC EMP_CD_DESC   ");
                sb.Append(" , a.EMP_CHG_CD + '-' + d.SUB_DESC EMP_CHG_CD_DESC   ");
                sb.Append(" , a.PAY_TYPE + '-' + e.SUB_DESC PAY_TYPE_DESC       ");
                sb.Append(" from TB_S_M_FR_AWARD_DM a             ");
                sb.Append("  left join TB_9_M_COMM_D b on  a.CHG_STATUS = b.SUB_CD and b.MAIN_CD = 'CHG_STATUS' and b.IS_VALID='Y'  and b.SYS_CD='SA'   ");
                sb.Append("  left join TB_9_M_COMM_D c on  a.EMP_CD = c.SUB_CD and c.MAIN_CD = 'EMP_CD' and b.IS_VALID='Y' and c.SYS_CD='HB' ");
                sb.Append("  left join TB_9_M_COMM_D d on  a.EMP_CHG_CD = d.SUB_CD and d.MAIN_CD = 'EMP_CHG_CD' and d.IS_VALID='Y' and d.SYS_CD='HB'    ");
                sb.Append("  left join TB_9_M_COMM_D e on  a.PAY_TYPE = e.SUB_CD and e.MAIN_CD = 'PAY_TYPE' and e.IS_VALID='Y'  and e.SYS_CD='SC'       ");
                sb.Append(" WHERE not exists (                ");
                sb.Append(" select * from TB_S_M_FR_AWARD_D f    ");
                sb.Append(" where                             ");
                sb.Append(" AWARD_YEAR = @AWARD_YEAR          ");
                sb.Append(" and AWARD_ROUND = @AWARD_ROUND    ");
                sb.Append(" and a.EMP_ID = f.EMP_ID           ");
                sb.Append(" )and                              ");
                sb.Append(" a.AWARD_YEAR = @AWARD_YEAR AND    ");
                sb.Append(" a.AWARD_ROUND = @AWARD_ROUND      ");
            }
            else if (data == "original")
            {
                sb.Append(" select  a.*                       ");
                sb.Append(" , a.CHG_STATUS + '-' + b.SUB_DESC CHG_STATUS_DESC   ");
                sb.Append(" , a.EMP_CD + '-' + c.SUB_DESC EMP_CD_DESC   ");
                sb.Append(" , a.EMP_CHG_CD + '-' + d.SUB_DESC EMP_CHG_CD_DESC   ");
                sb.Append(" , a.PAY_TYPE + '-' + e.SUB_DESC PAY_TYPE_DESC       ");
                sb.Append(" from TB_S_S_AWARD_D a             ");
                sb.Append("  left join TB_9_M_COMM_D b on  a.CHG_STATUS = b.SUB_CD and b.MAIN_CD = 'CHG_STATUS' and b.IS_VALID='Y'  and b.SYS_CD='SA'   ");
                sb.Append("  left join TB_9_M_COMM_D c on  a.EMP_CD = c.SUB_CD and c.MAIN_CD = 'EMP_CD' and b.IS_VALID='Y' and c.SYS_CD='HB' ");
                sb.Append("  left join TB_9_M_COMM_D d on  a.EMP_CHG_CD = d.SUB_CD and d.MAIN_CD = 'EMP_CHG_CD' and d.IS_VALID='Y' and d.SYS_CD='HB'    ");
                sb.Append("  left join TB_9_M_COMM_D e on  a.PAY_TYPE = e.SUB_CD and e.MAIN_CD = 'PAY_TYPE' and e.IS_VALID='Y'  and e.SYS_CD='SC'       ");
                sb.Append(" WHERE not exists (                ");
                sb.Append(" select * from TB_S_M_FR_AWARD_DM f     ");
                sb.Append(" where                             ");
                sb.Append(" AWARD_YEAR = @AWARD_YEAR          ");
                sb.Append(" and AWARD_ROUND = @AWARD_ROUND    ");
                sb.Append(" and a.EMP_ID = f.EMP_ID           ");
                sb.Append(" )and                              ");
                sb.Append(" a.AWARD_YEAR = @AWARD_YEAR AND    ");
                sb.Append(" a.AWARD_ROUND = @AWARD_ROUND      ");
            }

            //PK值
            ht.Add("@AWARD_YEAR", AWARD_YEAR);
            ht.Add("@AWARD_ROUND", AWARD_ROUND);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得與主檔比較的資料
    public DataTable getPreCompareData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select                                                                          ");
            sb.Append(" a.EMP_ID  N_EMP_ID  ,  f.EMP_ID  O_EMP_ID                                       ");
            sb.Append(" ,a.EMP_NAME  N_EMP_NAME  ,  f.EMP_NAME  O_EMP_NAME                              ");
            sb.Append(" ,a.EMP_CHG_CD  N_EMP_CHG_CD  ,  f.EMP_CHG_CD  O_EMP_CHG_CD                      ");
            sb.Append(" ,a.WS_CD  N_WS_CD  ,  f.WS_CD  O_WS_CD                                          ");
            sb.Append(" ,a.JPN_CD  N_JPN_CD  ,  f.JPN_CD  O_JPN_CD                                      ");
            sb.Append(" ,a.DEPT_NO  N_DEPT_NO  ,  f.DEPT_NO  O_DEPT_NO                                  ");
            sb.Append(" ,a.LEVEL_CD  N_LEVEL_CD  ,  f.LEVEL_CD  O_LEVEL_CD                              ");
            sb.Append(" ,a.PJOB_CD  N_PJOB_CD  ,  f.PJOB_CD  O_PJOB_CD                                  ");
            sb.Append(" ,a.JOIN_DT  N_JOIN_DT  ,  f.JOIN_DT  O_JOIN_DT                                  ");
            sb.Append(" ,a.LEAVE_DT  N_LEAVE_DT  ,  f.LEAVE_DT  O_LEAVE_DT                              ");
            sb.Append(" ,a.STAY_DT  N_STAY_DT  ,  f.STAY_DT  O_STAY_DT                                  ");
            sb.Append(" ,a.BE_CONTRACT_DT  N_BE_CONTRACT_DT  ,  f.BE_CONTRACT_DT  O_BE_CONTRACT_DT      ");
            sb.Append(" ,a.BE_EMP_DT  N_BE_EMP_DT  ,  f.BE_EMP_DT  O_BE_EMP_DT                          ");
            sb.Append(" ,a.WORK_DAYS  N_WORK_DAYS  ,  f.WORK_DAYS  O_WORK_DAYS                          ");
            sb.Append(" ,a.EMP_CD  N_EMP_CD  ,  f.EMP_CD  O_EMP_CD                                      ");
            sb.Append(" ,a.ID_DESC  N_ID_DESC  ,  f.ID_DESC  O_ID_DESC                                  ");
            sb.Append(" ,a.LEVEL_PAY  N_LEVEL_PAY  ,  f.LEVEL_PAY  O_LEVEL_PAY                          ");
            sb.Append(" ,a.ABILITY_PAY  N_ABILITY_PAY  ,  f.ABILITY_PAY  O_ABILITY_PAY                  ");
            sb.Append(" ,a.PJOB_PAY  N_PJOB_PAY  ,  f.PJOB_PAY  O_PJOB_PAY                              ");
            sb.Append(" ,a.PROFESSION_PAY  N_PROFESSION_PAY  ,  f.PROFESSION_PAY  O_PROFESSION_PAY      ");
            sb.Append(" ,a.FOOD_SUBSIDY  N_FOOD_SUBSIDY  ,  f.FOOD_SUBSIDY  O_FOOD_SUBSIDY              ");
            sb.Append(" ,a.SCORE_2H  N_SCORE_2H  ,  f.SCORE_2H  O_SCORE_2H                              ");
            sb.Append(" ,a.AWARD_BASE  N_AWARD_BASE  ,  f.AWARD_BASE  O_AWARD_BASE                      ");
            sb.Append(" ,a.LEAVE_A_HOUR  N_LEAVE_A_HOUR  ,  f.LEAVE_A_HOUR  O_LEAVE_A_HOUR              ");
            sb.Append(" ,a.LEAVE_B_HOUR  N_LEAVE_B_HOUR  ,  f.LEAVE_B_HOUR  O_LEAVE_B_HOUR              ");
            sb.Append(" ,a.LEAVE_C_HOUR  N_LEAVE_C_HOUR  ,  f.LEAVE_C_HOUR  O_LEAVE_C_HOUR              ");
            sb.Append(" ,a.LEAVE_Q_HOUR  N_LEAVE_Q_HOUR  ,  f.LEAVE_Q_HOUR  O_LEAVE_Q_HOUR              ");
            sb.Append(" ,a.LEAVE_OP_HOUR  N_LEAVE_OP_HOUR  ,  f.LEAVE_OP_HOUR  O_LEAVE_OP_HOUR          ");
            sb.Append(" ,a.THIRD_CNT_P  N_THIRD_CNT_P  ,  f.THIRD_CNT_P  O_THIRD_CNT_P                  ");
            sb.Append(" ,a.SECOND_CNT_P  N_SECOND_CNT_P  ,  f.SECOND_CNT_P  O_SECOND_CNT_P              ");
            sb.Append(" ,a.FIRST_CNT_P  N_FIRST_CNT_P  ,  f.FIRST_CNT_P  O_FIRST_CNT_P                  ");
            sb.Append(" ,a.THIRD_CNT_M  N_THIRD_CNT_M  ,  f.THIRD_CNT_M  O_THIRD_CNT_M                  ");
            sb.Append(" ,a.SECOND_CNT_M  N_SECOND_CNT_M  ,  f.SECOND_CNT_M  O_SECOND_CNT_M              ");
            sb.Append(" ,a.FIRST_CNT_M  N_FIRST_CNT_M  ,  f.FIRST_CNT_M  O_FIRST_CNT_M                  ");
            sb.Append(" ,a.ATTEND_DAYS  N_ATTEND_DAYS  ,  f.ATTEND_DAYS  O_ATTEND_DAYS                  ");
            sb.Append(" ,a.REWARD_DAYS  N_REWARD_DAYS  ,  f.REWARD_DAYS  O_REWARD_DAYS                  ");
            sb.Append(" ,a.DISCIPLINE_DAYS  N_DISCIPLINE_DAYS  ,  f.DISCIPLINE_DAYS  O_DISCIPLINE_DAYS  ");
            sb.Append(" ,a.AWARD_WORK_DAYS  N_AWARD_WORK_DAYS  ,  f.AWARD_WORK_DAYS  O_AWARD_WORK_DAYS  ");
            sb.Append(" ,a.AWARD_AMT  N_AWARD_AMT  ,  f.AWARD_AMT  O_AWARD_AMT                          ");
            sb.Append(" ,a.LEVELUP_FLAG  N_LEVELUP_FLAG  ,  f.LEVELUP_FLAG  O_LEVELUP_FLAG              ");
            sb.Append(" ,a.PAY_TYPE  N_PAY_TYPE  ,  f.PAY_TYPE  O_PAY_TYPE                              ");
            sb.Append(" ,a.CHG_STATUS  N_CHG_STATUS  ,  f.CHG_STATUS  O_CHG_STATUS                      ");
            sb.Append(" , a.CHG_STATUS + '-' + b.SUB_DESC N_CHG_STATUS_DESC   ");
            sb.Append(" , a.EMP_CD + '-' + c.SUB_DESC N_EMP_CD_DESC   ");
            sb.Append(" , a.EMP_CHG_CD + '-' + d.SUB_DESC N_EMP_CHG_CD_DESC   ");
            sb.Append(" , a.PAY_TYPE + '-' + e.SUB_DESC N_PAY_TYPE_DESC       ");
            sb.Append(" , f.CHG_STATUS + '-' + h.SUB_DESC O_CHG_STATUS_DESC   ");
            sb.Append(" , f.EMP_CD + '-' + i.SUB_DESC O_EMP_CD_DESC   ");
            sb.Append(" , f.EMP_CHG_CD + '-' + j.SUB_DESC O_EMP_CHG_CD_DESC   ");
            sb.Append(" , f.PAY_TYPE + '-' + g.SUB_DESC O_PAY_TYPE_DESC       ");
            sb.Append(" from TB_S_M_FR_AWARD_DM a left join  TB_S_M_FR_AWARD_D f  on a.EMP_ID = f.EMP_ID      ");
            sb.Append("  left join TB_9_M_COMM_D b on  a.CHG_STATUS = b.SUB_CD and b.MAIN_CD = 'CHG_STATUS' and b.IS_VALID='Y'  and b.SYS_CD='SA'   ");
            sb.Append("  left join TB_9_M_COMM_D c on  a.EMP_CD = c.SUB_CD and c.MAIN_CD = 'EMP_CD' and b.IS_VALID='Y' and c.SYS_CD='HB' ");
            sb.Append("  left join TB_9_M_COMM_D d on  a.EMP_CHG_CD = d.SUB_CD and d.MAIN_CD = 'EMP_CHG_CD' and d.IS_VALID='Y' and d.SYS_CD='HB'    ");
            sb.Append("  left join TB_9_M_COMM_D e on  a.PAY_TYPE = e.SUB_CD and e.MAIN_CD = 'PAY_TYPE' and e.IS_VALID='Y'  and e.SYS_CD='SC'       ");
            sb.Append("  left join TB_9_M_COMM_D g on  f.PAY_TYPE = g.SUB_CD and g.MAIN_CD = 'PAY_TYPE' and g.IS_VALID='Y'  and g.SYS_CD='SC'       ");
            sb.Append("  left join TB_9_M_COMM_D h on  f.CHG_STATUS = h.SUB_CD and h.MAIN_CD = 'CHG_STATUS' and h.IS_VALID='Y'  and h.SYS_CD='SA'   ");
            sb.Append("  left join TB_9_M_COMM_D i on  f.EMP_CD = i.SUB_CD and i.MAIN_CD = 'EMP_CD' and i.IS_VALID='Y' and i.SYS_CD='HB' ");
            sb.Append("  left join TB_9_M_COMM_D j on  f.EMP_CHG_CD = j.SUB_CD and j.MAIN_CD = 'EMP_CHG_CD' and j.IS_VALID='Y' and j.SYS_CD='HB'    ");
            sb.Append(" where 1=1                                                                       ");
            sb.Append(" and a.AWARD_YEAR=@AWARD_YEAR and a.AWARD_ROUND=@AWARD_ROUND                     ");
            sb.Append(" and f.AWARD_YEAR=@AWARD_YEAR  and f.AWARD_ROUND=@AWARD_ROUND                    ");
            sb.Append(" and a.APPROVE_FLAG ='N'                                                         ");

            //PK值
            ht.Add("@AWARD_YEAR", AWARD_YEAR);
            ht.Add("@AWARD_ROUND", AWARD_ROUND);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }


    //取得與原始檔比較的資料
    public DataTable getOriginalCompareData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select                                                                          ");
            sb.Append(" a.EMP_ID  N_EMP_ID  ,  f.EMP_ID  O_EMP_ID                                       ");
            sb.Append(" ,a.EMP_NAME  N_EMP_NAME  ,  f.EMP_NAME  O_EMP_NAME                              ");
            sb.Append(" ,a.EMP_CHG_CD  N_EMP_CHG_CD  ,  f.EMP_CHG_CD  O_EMP_CHG_CD                      ");
            sb.Append(" ,a.WS_CD  N_WS_CD  ,  f.WS_CD  O_WS_CD                                          ");
            sb.Append(" ,a.JPN_CD  N_JPN_CD  ,  f.JPN_CD  O_JPN_CD                                      ");
            sb.Append(" ,a.DEPT_NO  N_DEPT_NO  ,  f.DEPT_NO  O_DEPT_NO                                  ");
            sb.Append(" ,a.LEVEL_CD  N_LEVEL_CD  ,  f.LEVEL_CD  O_LEVEL_CD                              ");
            sb.Append(" ,a.PJOB_CD  N_PJOB_CD  ,  f.PJOB_CD  O_PJOB_CD                                  ");
            sb.Append(" ,a.JOIN_DT  N_JOIN_DT  ,  f.JOIN_DT  O_JOIN_DT                                  ");
            sb.Append(" ,a.LEAVE_DT  N_LEAVE_DT  ,  f.LEAVE_DT  O_LEAVE_DT                              ");
            sb.Append(" ,a.STAY_DT  N_STAY_DT  ,  f.STAY_DT  O_STAY_DT                                  ");
            sb.Append(" ,a.BE_CONTRACT_DT  N_BE_CONTRACT_DT  ,  f.BE_CONTRACT_DT  O_BE_CONTRACT_DT      ");
            sb.Append(" ,a.BE_EMP_DT  N_BE_EMP_DT  ,  f.BE_EMP_DT  O_BE_EMP_DT                          ");
            sb.Append(" ,a.WORK_DAYS  N_WORK_DAYS  ,  f.WORK_DAYS  O_WORK_DAYS                          ");
            sb.Append(" ,a.EMP_CD  N_EMP_CD  ,  f.EMP_CD  O_EMP_CD                                      ");
            sb.Append(" ,a.ID_DESC  N_ID_DESC  ,  f.ID_DESC  O_ID_DESC                                  ");
            sb.Append(" ,a.LEVEL_PAY  N_LEVEL_PAY  ,  f.LEVEL_PAY  O_LEVEL_PAY                          ");
            sb.Append(" ,a.ABILITY_PAY  N_ABILITY_PAY  ,  f.ABILITY_PAY  O_ABILITY_PAY                  ");
            sb.Append(" ,a.PJOB_PAY  N_PJOB_PAY  ,  f.PJOB_PAY  O_PJOB_PAY                              ");
            sb.Append(" ,a.PROFESSION_PAY  N_PROFESSION_PAY  ,  f.PROFESSION_PAY  O_PROFESSION_PAY      ");
            sb.Append(" ,a.FOOD_SUBSIDY  N_FOOD_SUBSIDY  ,  f.FOOD_SUBSIDY  O_FOOD_SUBSIDY              ");
            sb.Append(" ,a.SCORE_2H  N_SCORE_2H  ,  f.SCORE_2H  O_SCORE_2H                              ");
            sb.Append(" ,a.AWARD_BASE  N_AWARD_BASE  ,  f.AWARD_BASE  O_AWARD_BASE                      ");
            sb.Append(" ,a.LEAVE_A_HOUR  N_LEAVE_A_HOUR  ,  f.LEAVE_A_HOUR  O_LEAVE_A_HOUR              ");
            sb.Append(" ,a.LEAVE_B_HOUR  N_LEAVE_B_HOUR  ,  f.LEAVE_B_HOUR  O_LEAVE_B_HOUR              ");
            sb.Append(" ,a.LEAVE_C_HOUR  N_LEAVE_C_HOUR  ,  f.LEAVE_C_HOUR  O_LEAVE_C_HOUR              ");
            sb.Append(" ,a.LEAVE_Q_HOUR  N_LEAVE_Q_HOUR  ,  f.LEAVE_Q_HOUR  O_LEAVE_Q_HOUR              ");
            sb.Append(" ,a.LEAVE_OP_HOUR  N_LEAVE_OP_HOUR  ,  f.LEAVE_OP_HOUR  O_LEAVE_OP_HOUR          ");
            sb.Append(" ,a.THIRD_CNT_P  N_THIRD_CNT_P  ,  f.THIRD_CNT_P  O_THIRD_CNT_P                  ");
            sb.Append(" ,a.SECOND_CNT_P  N_SECOND_CNT_P  ,  f.SECOND_CNT_P  O_SECOND_CNT_P              ");
            sb.Append(" ,a.FIRST_CNT_P  N_FIRST_CNT_P  ,  f.FIRST_CNT_P  O_FIRST_CNT_P                  ");
            sb.Append(" ,a.THIRD_CNT_M  N_THIRD_CNT_M  ,  f.THIRD_CNT_M  O_THIRD_CNT_M                  ");
            sb.Append(" ,a.SECOND_CNT_M  N_SECOND_CNT_M  ,  f.SECOND_CNT_M  O_SECOND_CNT_M              ");
            sb.Append(" ,a.FIRST_CNT_M  N_FIRST_CNT_M  ,  f.FIRST_CNT_M  O_FIRST_CNT_M                  ");
            sb.Append(" ,a.ATTEND_DAYS  N_ATTEND_DAYS  ,  f.ATTEND_DAYS  O_ATTEND_DAYS                  ");
            sb.Append(" ,a.REWARD_DAYS  N_REWARD_DAYS  ,  f.REWARD_DAYS  O_REWARD_DAYS                  ");
            sb.Append(" ,a.DISCIPLINE_DAYS  N_DISCIPLINE_DAYS  ,  f.DISCIPLINE_DAYS  O_DISCIPLINE_DAYS  ");
            sb.Append(" ,a.AWARD_WORK_DAYS  N_AWARD_WORK_DAYS  ,  f.AWARD_WORK_DAYS  O_AWARD_WORK_DAYS  ");
            sb.Append(" ,a.AWARD_AMT  N_AWARD_AMT  ,  f.AWARD_AMT  O_AWARD_AMT                          ");
            sb.Append(" ,a.LEVELUP_FLAG  N_LEVELUP_FLAG  ,  f.LEVELUP_FLAG  O_LEVELUP_FLAG              ");
            sb.Append(" ,a.PAY_TYPE  N_PAY_TYPE  ,  f.PAY_TYPE  O_PAY_TYPE                              ");
            sb.Append(" ,a.CHG_STATUS  N_CHG_STATUS  ,  f.CHG_STATUS  O_CHG_STATUS                      ");
            sb.Append(" , a.CHG_STATUS + '-' + b.SUB_DESC N_CHG_STATUS_DESC   ");
            sb.Append(" , a.EMP_CD + '-' + c.SUB_DESC N_EMP_CD_DESC   ");
            sb.Append(" , a.EMP_CHG_CD + '-' + d.SUB_DESC N_EMP_CHG_CD_DESC   ");
            sb.Append(" , a.PAY_TYPE + '-' + e.SUB_DESC N_PAY_TYPE_DESC       ");
            sb.Append(" , f.CHG_STATUS + '-' + h.SUB_DESC O_CHG_STATUS_DESC   ");
            sb.Append(" , f.EMP_CD + '-' + i.SUB_DESC O_EMP_CD_DESC   ");
            sb.Append(" , f.EMP_CHG_CD + '-' + j.SUB_DESC O_EMP_CHG_CD_DESC   ");
            sb.Append(" , f.PAY_TYPE + '-' + g.SUB_DESC O_PAY_TYPE_DESC       ");
            sb.Append(" from TB_S_M_FR_AWARD_DM a left join  TB_S_S_AWARD_D f  on a.EMP_ID = f.EMP_ID      ");
            sb.Append("  left join TB_9_M_COMM_D b on  a.CHG_STATUS = b.SUB_CD and b.MAIN_CD = 'CHG_STATUS' and b.IS_VALID='Y'  and b.SYS_CD='SA'   ");
            sb.Append("  left join TB_9_M_COMM_D c on  a.EMP_CD = c.SUB_CD and c.MAIN_CD = 'EMP_CD' and b.IS_VALID='Y' and c.SYS_CD='HB' ");
            sb.Append("  left join TB_9_M_COMM_D d on  a.EMP_CHG_CD = d.SUB_CD and d.MAIN_CD = 'EMP_CHG_CD' and d.IS_VALID='Y' and d.SYS_CD='HB'    ");
            sb.Append("  left join TB_9_M_COMM_D e on  a.PAY_TYPE = e.SUB_CD and e.MAIN_CD = 'PAY_TYPE' and e.IS_VALID='Y'  and e.SYS_CD='SC'       ");
            sb.Append("  left join TB_9_M_COMM_D g on  f.PAY_TYPE = g.SUB_CD and g.MAIN_CD = 'PAY_TYPE' and g.IS_VALID='Y'  and g.SYS_CD='SC'       ");
            sb.Append("  left join TB_9_M_COMM_D h on  f.CHG_STATUS = h.SUB_CD and h.MAIN_CD = 'CHG_STATUS' and h.IS_VALID='Y'  and h.SYS_CD='SA'   ");
            sb.Append("  left join TB_9_M_COMM_D i on  f.EMP_CD = i.SUB_CD and i.MAIN_CD = 'EMP_CD' and i.IS_VALID='Y' and i.SYS_CD='HB' ");
            sb.Append("  left join TB_9_M_COMM_D j on  f.EMP_CHG_CD = j.SUB_CD and j.MAIN_CD = 'EMP_CHG_CD' and j.IS_VALID='Y' and j.SYS_CD='HB'    ");
            sb.Append(" where 1=1                                                                       ");
            sb.Append(" and a.AWARD_YEAR=@AWARD_YEAR and a.AWARD_ROUND=@AWARD_ROUND                     ");
            sb.Append(" and f.AWARD_YEAR=@AWARD_YEAR  and f.AWARD_ROUND=@AWARD_ROUND                    ");
            sb.Append(" and a.PRIMEVAL_FLAG <> 'N'                                                      ");

            //PK值
            ht.Add("@AWARD_YEAR", AWARD_YEAR);
            ht.Add("@AWARD_ROUND", AWARD_ROUND);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion

}