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
/// CFB2SH3200DAO 的摘要描述
/// </summary>
public class CFB2SH3200DAO : BaseDAO
{
    //年獎維護檔
    public string AWARD_YEAR { get; set; }
    //public string AWARD_ROUND { get; set; }
   // public string AWARD_ROUND_DESC { get; set; }
    //public string AWARD_DAYS { get; set; }
    public string AWARD_DT { get; set; }
    public string AWARD_STIME { get; set; }
    public string AWARD_ETIME { get; set; }
    //public string AWARD_ITEM_A { get; set; }
    //public string AWARD_ITEM_RP { get; set; }
    //public string AWARD_ITEM_AL { get; set; }
    //public string AWARD_ITEM_D { get; set; }
    public string TARGET_GEN_DT { get; set; }
    public decimal AWARD_TOTAL_PEOPLE{ get; set; }
    public string GEN_DT { get; set; }
    public decimal AWARD_TOTAL_AMOUNT { get; set; }
    public string RELEASE_DT { get; set; }
    public string RELEASE_BY { get; set; }
    public string APPROVE_DT { get; set; }
    public string APPROVE_BY { get; set; }
    public string APPROVE_STATUS { get; set; }
    public string APPROVE_STATUS_DESC { get; set; }
    public string REMARK { get; set; }
    //public string SALARY_TRANS_DT { get; set; }
    //public string SALARY_TRANS_BY { get; set; }
    //public string PROCESS_STATUS { get; set; }
    //public string SALARY_DT { get; set; }
    //public string FREEZE_FLAG { get; set; }


    //年獎明細維護檔
    public string AWARD_DAYS_D { get; set; }
    public string EMP_ID { get; set; }
    public string EMP_NAME { get; set; }
    //public string SEX_CD { get; set; }
    public string EMP_CHG_CD { get; set; }
    public string PJOB_CD { get; set; }
    public string WS_CD { get; set; }
    public string DEPT_NO { get; set; }
    public string LEVEL_CD { get; set; }
    public string SCORE_FINAL { get; set; }
    public decimal AWARD_DAYS { get; set; }
    public decimal AWARD_DIFFER { get; set; }
    public string JOIN_DT { get; set; }
    public string STAY_SDT { get; set; }
    public string STAY_EDT { get; set; }
    public string AWARD_SDT { get; set; }
    public string AWARD_EDT { get; set; }
    public decimal JOB_DT { get; set; }
    public decimal LEAVE_A_HOUR { get; set; }
    public decimal LEAVE_B_HOUR { get; set; }
    public decimal LEAVE_C_HOUR { get; set; }
    public decimal PROFESSION_PAY_BEFORE { get; set; }
    public decimal PERSONAL_LEAVE_DAYS { get; set; }
    public decimal SICK_LEAVE_DAYS { get; set; }
    public decimal NOPAID_SICK_LEAVE_DAYS { get; set; }
    public decimal ATTEND_DAYS { get; set; }
    public decimal ACTUAL_JOB_DAYS { get; set; }
    public decimal RULE_DECAMT { get; set; }
    public decimal SHOULD_AMT { get; set; }
    public decimal TAX_AMT { get; set; }
    public decimal ACTUAL_AMT { get; set; }
    public string EMP_CD { get; set; }
    public decimal LEVEL_PAY { get; set; }
    public string ID_DESC { get; set; }
    public string ABILITY_PAY { get; set; }
    public string PJOB_PAY { get; set; }
    public string PROFESSION_PAY { get; set; }
    public string FOOD_SUBSIDY { get; set; }
    public string LEAVE_Q_HOUR { get; set; }
    public string LEAVE_OP_HOUR { get; set; }
    public string THIRD_CNT_P { get; set; }
    public string SECOND_CNT_P { get; set; }
    public string FIRST_CNT_P { get; set; }
    public string THIRD_CNT_M { get; set; }
    public string SECOND_CNT_M { get; set; }
    public string FIRST_CNT_M { get; set; }
    public string CHG_STATUS { get; set; }
    public string DELETE_MEMO { get; set; }
    public string APPROVE_MARK { get; set; }
    /**
    public string LEVELUP_FLAG { get; set; }
    public string LEVEL_PAY_BEFORE { get; set; }
    public string ABILITY_PAY_BEFORE { get; set; }
    public string PJOB_PAY_BEFORE { get; set; }
    public string PROFESSION_PAY_BEFORE { get; set; }
    public string FOOOD_SUBSIDY_BEFORE { get; set; }
    public string SCORE_2H { get; set; }
    public string AWARD_BASE { get; set; }
    public string SCORE_2H_BEFORE { get; set; }
    public string AWARD_BASE_BEFORE { get; set; }
    public string ATTEND_DAYS { get; set; }
    public string REWARD_DAYS { get; set; }

    public string DISCIPLINE_DAYS { get; set; }
    public string AWARD_WORK_DAYS { get; set; }
    public string AWARD_AMT { get; set; }
    public string AWARD_TAX { get; set; }
    public string AWARD_AMT_R { get; set; }
    public string AWARD_AMT_TMEP { get; set; }
    public string AWARD_AMT_LEVEL { get; set; }
    public string PAY_TYPE { get; set; }
    public string PRIMEVAL_FLAG { get; set; }
    public string APPROVE_FLAG { get; set; }**/

    //年獎考績格差設定檔
    public string AWARD { get; set; }
    public string EMP_STATUS { get; set; }

    //共用欄位
    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }


    public CFB2SH3200DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }


    #region 資料取得
    //是否已調薪
    internal DataTable getSALARY_ADJ()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select count(0) resultCount from TB_S_M_SALARYSET_H");
            sb.Append(" where Left(effect_YM,4)= YEAR(GETDATE()) ");
            sb.Append(" and APPROVE_STATUS ='Y' ");
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
     


    //依PK值取得資料，
    internal DataTable getPKData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select count(0) resultCount from TB_S_M_FR_AWARD_H");
            sb.Append(" where AWARD_YEAR=@AWARD_YEAR");
            ht.Add("@AWARD_YEAR", AWARD_YEAR);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }


    //取得Dtl的表頭資料
    public void getTitleData()
    { try
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
                
               
                this.AWARD_TOTAL_AMOUNT = Convert.ToDecimal(dr["AWARD_TOTAL_AMOUNT"]);
                this.AWARD_TOTAL_PEOPLE = Convert.ToDecimal(dr["AWARD_TOTAL_PEOPLE"]);
                this.AWARD_DT = dr["AWARD_DT"].ToString() != "" ? Convert.ToDateTime(dr["AWARD_DT"].ToString()).ToString("yyyy/MM/dd") : "";
                 this.APPROVE_STATUS = Convert.ToString(dr["APPROVE_STATUS"]);
                this.APPROVE_STATUS_DESC = Convert.ToString(dr["APPROVE_STATUS_DESC"]);
                this.REMARK = Convert.ToString(dr["REMARK"]);

            }

        }
        catch
        {
            throw;
        }
       


    }

    //取得年獎的開始及結束日期
    public void getSatrtAndEndDT(string award_year, string award_round)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" select  a.*   ");
            sb.Append(" from TB_S_M_FR_AWARD_H a ");
            sb.Append(" where 1=1 ");
            if (AWARD_YEAR != "")
            {
                sb.Append(" and AWARD_YEAR = @AWARD_YEAR ");
                ht.Add("@AWARD_YEAR", award_year);
            }
        

            DataTable dt = dbConn.Query(sb, ht);
            foreach (DataRow dr in dt.Rows)
            {
                this.AWARD_STIME = dr["AWARD_STIME"].ToString() != "" ? Convert.ToDateTime(dr["AWARD_STIME"].ToString()).ToString("yyyy/MM/dd") : "";
                this.AWARD_ETIME = dr["AWARD_ETIME"].ToString() != "" ? Convert.ToDateTime(dr["AWARD_ETIME"].ToString()).ToString("yyyy/MM/dd") : "";
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
                            , string award_year_s, string award_year_e
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
            sb.Append(" a.* ");
            sb.Append(" from TB_S_M_FR_AWARD_H a ");
            sb.Append(" where 1=1 ");
            //查詢條件-dropDownList
         
            if (award_year_s != "")
            {
                sb.Append(" and AWARD_YEAR >= @AWARD_YEAR1 ");
                ht.Add("@AWARD_YEAR1", award_year_s);
            }
            if (award_year_e != "")
            {
                sb.Append(" and AWARD_YEAR <= @AWARD_YEAR2 ");
                ht.Add("@AWARD_YEAR2", award_year_e);
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
                          , string award_year_s, string award_year_e
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

            //查詢條件-dropDownList
          
            if (award_year_s != "")
            {
                sb.Append(" and AWARD_YEAR >= @AWARD_YEAR1 ");
                ht.Add("@AWARD_YEAR1", award_year_s);
            }
            if (award_year_e != "")
            {
                sb.Append(" and AWARD_YEAR <= @AWARD_YEAR2 ");
                ht.Add("@AWARD_YEAR2", award_year_e);
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
                               , string award_year, string emp_id, string emp_name
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
            sb.Append(" , a.EMP_CHG_CD + '-' + isnull(d.SUB_DESC,'') EMP_CHG_CD_DESC   ");
            sb.Append(" ,e.PJOB_DESC   ");
            sb.Append(" from TB_S_M_FR_AWARD_D a ");
            sb.Append("  left join TB_9_M_COMM_D b on  a.CHG_STATUS = b.SUB_CD and b.MAIN_CD = 'CHG_STATUS' and b.IS_VALID='Y'  and b.SYS_CD='SA'   ");
            sb.Append("  left join TB_9_M_COMM_D d on  a.EMP_CHG_CD = d.SUB_CD and d.MAIN_CD = 'EMP_CHG_CD' and d.IS_VALID='Y' and d.SYS_CD='HB'    ");
            sb.Append("  left join TB_H_M_PJOB e on  a.PJOB_CD = e.PJOB_CD and getDate()>=e.START_DT  ");

            //查詢條件
            if (award_year != "")
            {
                sb.Append(" and a.AWARD_YEAR = @AWARD_YEAR ");
                ht.Add("@AWARD_YEAR", award_year);
            }
            if (emp_id != "")
            {
                sb.Append(" and EMP_ID like @EMP_ID ");
                ht.Add("@EMP_ID", emp_id + "%");
            }
            if (emp_name != "")
            {
                sb.Append(" and EMP_NAME like @EMP_NAME ");
                ht.Add("@EMP_NAME", emp_name + "%");
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
                            , string award_year, string emp_id, string emp_name
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
            if ( award_year == "")
            {
                sb.Append(" and 1=2 ");
            }
            //查詢條件
            if (award_year != "")
            {
                sb.Append(" and AWARD_YEAR = @AWARD_YEAR ");
                ht.Add("@AWARD_YEAR", award_year);
            }
          
            if (emp_id != "")
            {
                sb.Append(" and EMP_ID like @EMP_ID ");
                ht.Add("@EMP_ID", emp_id + "%");
            }
            if (emp_name != "")
            {
                sb.Append(" and EMP_NAME like @EMP_NAME ");
                ht.Add("@EMP_NAME", emp_name + "%");
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

    //年獎明細維護檔 取得異動狀態, 生成資料異動flag
    internal DataTable getDetailData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select CHG_STATUS from  TB_S_M_FR_AWARD_D ");
            sb.Append(" where AWARD_YEAR = @AWARD_YEAR ");
            sb.Append("  and EMP_ID = @EMP_ID");

            ht.Add("@AWARD_YEAR", AWARD_YEAR);
            ht.Add("@EMP_ID", EMP_ID);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

   


    #endregion




    #region DB存取 (Qry)
    //刪除 年獎維護檔
    public void deleteDataH(string year)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("delete from TB_S_M_FR_AWARD_H ");
            sb.Append(" where AWARD_YEAR = @AWARD_YEAR  ");
            ht.Add("@AWARD_YEAR", year);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //刪除 年獎明細維護檔
    public void deleteDataD(string year, string round, string tableName)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("delete from  " + tableName);
            sb.Append(" where AWARD_YEAR = @AWARD_YEAR  ");
            ht.Add("@AWARD_YEAR", year);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //刪除 年獎明細維護檔
    public void updateDataD(string year,string empId,string delMemo)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("UPDATE TB_S_M_FR_AWARD_D ");
            sb.Append("SET DELETE_FLAG='Y', DELETE_MEMO=@DELETE_MEMO ");
            sb.Append(" where AWARD_YEAR = @AWARD_YEAR  AND  EMP_ID = @EMP_ID ");
            ht.Add("@AWARD_YEAR", year);
            ht.Add("@EMP_ID", empId);
            ht.Add("@DELETE_MEMO", delMemo);
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
            sb.Append(" update TB_S_M_FR_AWARD_H ");
            sb.Append(" set AWARD_DT = @AWARD_DT ");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT = @UPDATED_DT");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where AWARD_YEAR = @AWARD_YEAR  ");
            ht.Add("@AWARD_DT", Convert.ToDateTime(AWARD_DT));
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@UPDATED_DT", now);
            ht.Add("@FUNC_ID", FUNC_ID);

            //pk值
            ht.Add("@AWARD_YEAR", AWARD_YEAR);

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
            sb.Append(" INSERT INTO TB_S_M_FR_AWARD_H ");
            sb.Append(" ( ");
            sb.Append(" AWARD_YEAR, AWARD_DT, AWARD_STIME, AWARD_ETIME ");
            sb.Append(" ,CREATED_BY, CREATED_DT, UPDATED_BY, UPDATED_DT, FUNC_ID  ");
            sb.Append(" ) ");
            sb.Append(" VALUES ");
            sb.Append(" ( @AWARD_YEAR,    @AWARD_DT,  @AWARD_STIME,  @AWARD_ETIME  ");
            sb.Append("   ,@CREATED_BY,  @CREATED_DT,  @UPDATED_BY,  @UPDATED_DT,  @FUNC_ID )");

            ht.Add("@AWARD_YEAR", AWARD_YEAR);
            ht.Add("@AWARD_DT", Convert.ToDateTime(AWARD_DT));
            ht.Add("@AWARD_STIME", Convert.ToDateTime(AWARD_STIME));
            ht.Add("@AWARD_ETIME", Convert.ToDateTime(AWARD_ETIME));
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

    //呼叫年獎對象生成SP
    internal void execSP_S_AWARD_DATA()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_S_AWARD_DATA");
            ht.Add("@AWARD_YEAR", AWARD_YEAR);
            ht.Add("@AWARD_DT", Convert.ToDateTime(AWARD_DT).ToString("yyyy/MM/dd"));
            ht.Add("@AWARD_STIME", Convert.ToDateTime(AWARD_STIME).ToString("yyyy/MM/dd"));
            ht.Add("@AWARD_ETIME", Convert.ToDateTime(AWARD_ETIME).ToString("yyyy/MM/dd"));
            ht.Add("@USERID", CREATED_BY);//CREATED_BY
            ht.Add("@FUNCID", "FB2SH020");
            dbConn.ExecuteSPT(sb, ht, true);

        }
        catch (Exception)
        {

            throw;
        }

    }

    //提出核可
    public void updateRelease()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            DateTime now = DateTime.Parse(DateTime.Now.ToString());
            sb.Append(" update TB_S_M_FR_AWARD_H ");
            sb.Append(" set RELEASE_DT = @RELEASE_DT ");
            sb.Append(" ,RELEASE_BY = @RELEASE_BY ");
            sb.Append(" ,APPROVE_DT = @APPROVE_DT ");
            sb.Append(" ,APPROVE_BY = @APPROVE_BY ");
            ////sb.Append(" ,APPROVE_STATUS = @APPROVE_STATUS ");
           // sb.Append(" ,FREEZE_FLAG = @FREEZE_FLAG ");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT = @UPDATED_DT");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where AWARD_YEAR = @AWARD_YEAR ");

            ht.Add("@RELEASE_DT", DateTime.Now.ToString("yyyy/MM/dd"));
            ht.Add("@RELEASE_BY", SessionHandle.Current.emp_id);
            ht.Add("@APPROVE_DT", DBNull.Value);
            ht.Add("@APPROVE_BY", "");
            //ht.Add("@APPROVE_STATUS", "N"); 因有可能是駁回狀態
            //ht.Add("@FREEZE_FLAG", "Y");

            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_DT", now);
            ht.Add("@FUNC_ID", FUNC_ID);

            //pk值
            ht.Add("@AWARD_YEAR", AWARD_YEAR);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    /**
    //薪資轉出
    public void updateAnnounce(DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();


            sb.Append(" update TB_S_M_FR_AWARD_H ");
            sb.Append(" set SALARY_TRANS_DT = @SALARY_TRANS_DT ");
            sb.Append(" ,SALARY_TRANS_BY = @SALARY_TRANS_BY ");
            sb.Append(" ,FREEZE_FLAG = @FREEZE_FLAG ");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT = @UPDATED_DT");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where AWARD_YEAR = @AWARD_YEAR ");
            sb.Append("  and AWARD_ROUND = @AWARD_ROUND");

            ht.Add("@SALARY_TRANS_DT", SALARY_TRANS_DT);
            ht.Add("@SALARY_TRANS_BY", SALARY_TRANS_BY);
            ht.Add("@FREEZE_FLAG", FREEZE_FLAG);

            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@UPDATED_DT", now);
            ht.Add("@FUNC_ID", FUNC_ID);

            ht.Add("@AWARD_YEAR", AWARD_YEAR);
            ht.Add("@AWARD_ROUND", AWARD_ROUND);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }**/


    #endregion


    #region DB存取(Dtl)
    /**
    //支付狀態一括更新(年獎明細維護檔)
    public void updatePayType_D(DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" update TB_S_M_FR_AWARD_DM ");
            sb.Append(" set CHG_STATUS = CASE WHEN APPROVE_FLAG='N'and CHG_STATUS='N' THEN 'N' ELSE 'U' END  ");
            sb.Append(" ,PAY_TYPE = @PAY_TYPE");
            sb.Append(" ,PRIMEVAL_FLAG = CASE  WHEN PRIMEVAL_FLAG='N' THEN 'Y' ELSE PRIMEVAL_FLAG END  ");
            sb.Append(" ,APPROVE_FLAG = @APPROVE_FLAG");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT = @UPDATED_DT");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where AWARD_YEAR = @AWARD_YEAR ");
            sb.Append("  and AWARD_ROUND = @AWARD_ROUND");
            sb.Append("  and EMP_ID = @EMP_ID");

            //set值
            //ht.Add("@CHG_STATUS", CHG_STATUS);
            ht.Add("@PAY_TYPE", PAY_TYPE);
            //ht.Add("@PRIMEVAL_FLAG", PRIMEVAL_FLAG);
            ht.Add("@APPROVE_FLAG", APPROVE_FLAG);

            //PK值
            ht.Add("@AWARD_YEAR", AWARD_YEAR);
            ht.Add("@AWARD_ROUND", AWARD_ROUND);
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

    //支付狀態一括更新,還原提出核可、年獎對象及金額、支付狀態(年獎維護檔)
    public void updatePayType_H(DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //總金額
            StringBuilder sb_AMT = new StringBuilder();
            sb_AMT.Append(" , AWARD_TOTAL_AMOUNT = ( select isnull( sum(AWARD_AMT),0) from  TB_S_M_FR_AWARD_DM  ");
            sb_AMT.Append(" where AWARD_YEAR = @AWARD_YEAR ");
            sb_AMT.Append("  and AWARD_ROUND = @AWARD_ROUND ");
            sb_AMT.Append("  and CHG_STATUS <> @CHG_STATUS ");
            sb_AMT.Append("  ) ");
            //總人數
            StringBuilder sb_NUM = new StringBuilder();
            sb_NUM.Append(" , AWARD_TOTAL_DECIMAL = ( select   count(EMP_ID) from  TB_S_M_FR_AWARD_DM ");
            sb_NUM.Append(" where AWARD_YEAR = @AWARD_YEAR ");
            sb_NUM.Append("  and AWARD_ROUND = @AWARD_ROUND ");
            sb_NUM.Append("  and CHG_STATUS <> @CHG_STATUS ");
            sb_NUM.Append("  ) ");


            sb.Append(" update TB_S_M_FR_AWARD_H ");
            sb.Append(" set RELEASE_DT = @RELEASE_DT ");
            sb.Append(sb_AMT);
            sb.Append(sb_NUM);
            sb.Append(" ,RELEASE_BY = @RELEASE_BY");
            sb.Append(" ,APPROVE_DT = @APPROVE_DT");
            sb.Append(" ,APPROVE_BY = @APPROVE_BY");
            sb.Append(" ,APPROVE_STATUS = CASE ");
            sb.Append(" when APPROVE_STATUS='B'   then  'B' ");
            sb.Append(" ELSE 'N' ");
            sb.Append(" END");
            sb.Append(" ,FREEZE_FLAG = @FREEZE_FLAG");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT = @UPDATED_DT");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where AWARD_YEAR = @AWARD_YEAR ");
            sb.Append("  and AWARD_ROUND = @AWARD_ROUND");

            //set值
            ht.Add("@RELEASE_DT", DBNull.Value);
            ht.Add("@RELEASE_BY", "");
            ht.Add("@APPROVE_DT", DBNull.Value);
            ht.Add("@APPROVE_BY", "");
            //ht.Add("@APPROVE_STATUS", "N");
            ht.Add("@FREEZE_FLAG", "N");

            //PK值
            ht.Add("@AWARD_YEAR", AWARD_YEAR);
            ht.Add("@AWARD_ROUND", AWARD_ROUND);
            ht.Add("@CHG_STATUS", "D");

            //新修日期
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@UPDATED_DT", now);
            ht.Add("@FUNC_ID", "FB2SH020");

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }
    
    //刪除- 更新 年獎明細維護檔的異動狀態(DTL)
    public void updateStatus2DeleteDtl_D(DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_S_M_FR_AWARD_DM          ");
            sb.Append(" set APPROVE_FLAG = @APPROVE_FLAG    ");
            sb.Append(" ,CHG_STATUS =  @CHG_STATUS  ");
            sb.Append(" ,PRIMEVAL_FLAG = CASE WHEN PRIMEVAL_FLAG='N' THEN 'Y'  ELSE PRIMEVAL_FLAG  END    ");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY       ");
            sb.Append(" ,UPDATED_DT = @UPDATED_DT       ");
            sb.Append(" ,FUNC_ID = @FUNC_ID             ");
            sb.Append(" where AWARD_YEAR = @AWARD_YEAR  ");
            sb.Append("  and AWARD_ROUND = @AWARD_ROUND ");
            sb.Append("  and EMP_ID = @EMP_ID           ");
            //set值
            ht.Add("@CHG_STATUS", CHG_STATUS);
            ht.Add("@APPROVE_FLAG", APPROVE_FLAG);
            //PK值
            ht.Add("@AWARD_YEAR", AWARD_YEAR);
            ht.Add("@AWARD_ROUND", AWARD_ROUND);
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

    //刪除 - 更新年獎維護檔,還原提出核可 總金額及總人數(DTL)
    public void updateTotal2Dtl(DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //總金額
            StringBuilder sb_AMT = new StringBuilder();
            sb_AMT.Append(" AWARD_TOTAL_AMOUNT = ( select isnull( sum(AWARD_AMT),0) from  TB_S_M_FR_AWARD_DM  ");
            sb_AMT.Append(" where AWARD_YEAR = @AWARD_YEAR ");
            sb_AMT.Append("  and AWARD_ROUND = @AWARD_ROUND ");
            sb_AMT.Append("  and CHG_STATUS <> @CHG_STATUS ");
            sb_AMT.Append("  ) ");
            //總人數
            StringBuilder sb_NUM = new StringBuilder();
            sb_NUM.Append(" , AWARD_TOTAL_DECIMAL = ( select   count(EMP_ID) from  TB_S_M_FR_AWARD_DM ");
            sb_NUM.Append(" where AWARD_YEAR = @AWARD_YEAR ");
            sb_NUM.Append("  and AWARD_ROUND = @AWARD_ROUND ");
            sb_NUM.Append("  and CHG_STATUS <> @CHG_STATUS ");
            sb_NUM.Append("  ) ");

            sb.Append(" update TB_S_M_FR_AWARD_H ");
            sb.Append(" set ");
            sb.Append(sb_AMT);
            sb.Append(sb_NUM);
            sb.Append(" ,RELEASE_DT = @RELEASE_DT ");
            sb.Append(" ,RELEASE_BY = @RELEASE_BY");
            sb.Append(" ,APPROVE_DT = @APPROVE_DT");
            sb.Append(" ,APPROVE_BY = @APPROVE_BY");
            sb.Append(" ,APPROVE_STATUS = @APPROVE_STATUS");
            sb.Append(" ,FREEZE_FLAG = @FREEZE_FLAG");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT = @UPDATED_DT");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where AWARD_YEAR = @AWARD_YEAR ");
            sb.Append("  and AWARD_ROUND = @AWARD_ROUND");

            //set值
            ht.Add("@RELEASE_DT", DBNull.Value);
            ht.Add("@RELEASE_BY", "");
            ht.Add("@APPROVE_DT", DBNull.Value);
            ht.Add("@APPROVE_BY", "");
            ht.Add("@APPROVE_STATUS", "N");
            ht.Add("@FREEZE_FLAG", "N");
            //PK值
            ht.Add("@AWARD_YEAR", AWARD_YEAR);
            ht.Add("@AWARD_ROUND", AWARD_ROUND);
            ht.Add("@CHG_STATUS", "D");

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
    **/
    #endregion


    #region DB存取 (EXCEL的新增,修改)
    //判斷工號是否存在於年獎明細維護檔
    internal DataTable getAwardEmpCount(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select count(0) resultCount from TB_S_M_FR_AWARD_D");
            sb.Append(" where AWARD_YEAR = @AWARD_YEAR ");
            sb.Append(" and EMP_ID = @EMP_ID ");
            ht.Add("@AWARD_YEAR", AWARD_YEAR);
            ht.Add("@EMP_ID", emp_id);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
    /**
    //判斷原始考績是否存在
    internal DataTable getAwardBase(string award)
    {
        try
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select count(0) resultCount  from TB_S_M_FR_AWARD_COND  ");
            sb.Append(" where ");
            sb.Append(" AWARD = @AWARD");
            ht.Add("@AWARD", award);
            dt = dbConn.Query(sb, ht);
            
            return dt;
        }
        catch (Exception)
        {

            throw;
        }
    }
    //依工號及考績取得年獎格差
    internal DataTable getAwardBase(String empid, string award)
    {
        try
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select count(0) resultCount  from TB_S_M_FR_AWARD_COND  ");
            sb.Append(" where LEVEL_CD = (select LEVEL_CD from VW_H_EMP_DATA where EMP_ID=@empid) ");
            sb.Append(" and WS_CD=(select WS_CD from VW_H_EMP_DATA where EMP_ID=@empid) ");
            sb.Append(" and AWARD = @AWARD");
            ht.Add("@empid", empid);
            ht.Add("@AWARD", award);
            dt =dbConn.Query(sb, ht);

            sb.Clear();
            ht.Clear();
            if ((int)dt.Rows[0]["resultCount"] > 0)
            {
                sb.Append(" select AWARD_BASE from TB_S_M_FR_AWARD_COND  ");
                sb.Append(" where LEVEL_CD = (select LEVEL_CD from VW_H_EMP_DATA where EMP_ID=@empid) ");
                sb.Append(" and WS_CD=(select WS_CD from VW_H_EMP_DATA where EMP_ID=@empid) ");
                sb.Append(" and AWARD = @AWARD");
                ht.Add("@empid", empid);
                ht.Add("@AWARD", award);
            }
            else {
                sb.Append(" select AWARD_BASE from TB_S_M_FR_AWARD_COND  ");
                sb.Append(" where LEVEL_CD = @LEVEL_CD ");
                sb.Append(" and WS_CD= @WS_CD ");
                sb.Append(" and AWARD = @AWARD");
                ht.Add("@LEVEL_CD", "");
                ht.Add("@WS_CD", "");
                ht.Add("@AWARD", award);
            }
            dt.Clear();
            dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch (Exception)
        {

            throw;
        }
    }


    //依工號取得身份標示
    internal DataTable getID_DESC(String empid,String start_DT,String end_DT)
    {
        try
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select dbo.FN_S_AWARD_ID_DESC(@empid,@start_DT,@end_DT) as ID_DESC  ");
            ht.Add("@empid", empid);
            ht.Add("@start_DT", start_DT);
            ht.Add("@end_DT", end_DT);
            dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch (Exception)
        {

            throw;
        }
    }**/
    //取得該員工相關資料
    internal void getAddEmpData()
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
                //this.SEX_CD = Convert.ToString(dr["SEX_CD"]);
                this.EMP_CHG_CD = Convert.ToString(dr["EMP_CHG_CD"]);
                this.WS_CD = Convert.ToString(dr["WS_CD"]);
               // this.JPN_CD = Convert.ToString(dr["JPN_CD"]);
               // this.COMPANY_CD = Convert.ToString(dr["COMPANY_CD"]);
                this.LEVEL_CD = Convert.ToString(dr["LEVEL_CD"]);
                this.PJOB_CD = Convert.ToString(dr["PJOB_CD"]);
                this.JOIN_DT =  dr["JOIN_DT"].ToString() != "" ? Convert.ToDateTime(dr["JOIN_DT"].ToString()).ToString("yyyy/MM/dd") : "";
                //this.LEAVE_DT = dr["LEAVE_DT"].ToString() != "" ? Convert.ToDateTime(dr["LEAVE_DT"].ToString()).ToString("yyyy/MM/dd") : "";
                //this.STAY_DT =  dr["STAY_DT"].ToString() != "" ? Convert.ToDateTime(dr["STAY_DT"].ToString()).ToString("yyyy/MM/dd") : "";
                //this.BE_CONTRACT_DT = dr["BE_CONTRACT_DT"].ToString() != "" ? Convert.ToDateTime(dr["BE_CONTRACT_DT"].ToString()).ToString("yyyy/MM/dd") : "";
                //this.BE_EMP_DT = dr["BE_EMP_DT"].ToString() != "" ? Convert.ToDateTime(dr["BE_EMP_DT"].ToString()).ToString("yyyy/MM/dd") : "";
               // this.EMP_CD = Convert.ToString(dr["EMP_CD"]);


            }

        }
        catch (Exception)
        {

            throw;
        }
    }
  

    
    

    #endregion



    //取得EXCEL下載資料(維護檔)，
    public DataTable getMaintainData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" select a.*   ");
            sb.Append(" , a.CHG_STATUS + '-' + b.SUB_DESC CHG_STATUS_DESC   ");
            sb.Append(" , a.EMP_CD + '-' +      isnull(c.SUB_DESC,'') EMP_CD_DESC   ");
            sb.Append(" , a.EMP_CHG_CD + '-' +  isnull( d.SUB_DESC,'')  EMP_CHG_CD_DESC   ");
            sb.Append(" , e.PJOB_DESC  ");
            sb.Append(" from TB_S_M_FR_AWARD_D a ");
            sb.Append("  left join TB_9_M_COMM_D b on  a.CHG_STATUS = b.SUB_CD and b.MAIN_CD = 'CHG_STATUS' and b.IS_VALID='Y'  and b.SYS_CD='SA'   ");
            sb.Append("  left join TB_9_M_COMM_D c on  a.EMP_CD = c.SUB_CD and c.MAIN_CD = 'EMP_CD' and b.IS_VALID='Y' and c.SYS_CD='HB' ");
            sb.Append("  left join TB_9_M_COMM_D d on  a.EMP_CHG_CD = d.SUB_CD and d.MAIN_CD = 'EMP_CHG_CD' and d.IS_VALID='Y' and d.SYS_CD='HB'    ");
            sb.Append("  left join TB_H_M_PJOB e on  a.PJOB_CD = e.PJOB_CD and getDate()>=e.START_DT  ");
            sb.Append(" where 1=1 ");

            sb.Append(" and a.AWARD_YEAR = @AWARD_YEAR ");
            ht.Add("@AWARD_YEAR", AWARD_YEAR);
          

            sb.Append(" order by APPROVE_MARK DESC, UPDATED_DT DESC, EMP_ID ASC ");

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    



}