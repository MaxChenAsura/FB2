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
/// CFB2SH0200DAO 的摘要描述
/// </summary>
public class CFB2SH0200DAO : BaseDAO
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
    public string AWARD_TOTAL_DECIMAL { get; set; }
    public string GEN_DT { get; set; }
    public string AWARD_TOTAL_AMOUNT { get; set; }
    public string RELEASE_DT { get; set; }
    public string RELEASE_BY { get; set; }
    public string APPROVE_DT { get; set; }
    public string APPROVE_BY { get; set; }
    public string APPROVE_STATUS { get; set; }
    public string APPROVE_STATUS_DESC { get; set; }
    public string REMARK { get; set; }
    public string SALARY_TRANS_DT { get; set; }
    public string SALARY_TRANS_BY { get; set; }
    public string PROCESS_STATUS { get; set; }
    public string SALARY_DT { get; set; }
    public string FREEZE_FLAG { get; set; }


    //年獎明細維護檔
    public string AWARD_DAYS_D { get; set; }
    public string EMP_ID { get; set; }
    public string EMP_NAME { get; set; }
    public string SEX_CD { get; set; }
    public string EMP_CHG_CD { get; set; }
    public string WS_CD { get; set; }
    public string JPN_CD { get; set; }
    public string COMPANY_CD { get; set; }
    public string DEPT_NO { get; set; }
    public string LEVEL_CD { get; set; }
    public string PJOB_CD { get; set; }
    public string JOIN_DT { get; set; }
    public string LEAVE_DT { get; set; }
    public string STAY_DT { get; set; }
    public string BE_CONTRACT_DT { get; set; }
    public string BE_EMP_DT { get; set; }
    public string WORK_DAYS { get; set; }
    public string EMP_CD { get; set; }
    public string ID_DESC { get; set; }
    public string LEVEL_PAY { get; set; }
    public string ABILITY_PAY { get; set; }
    public string PJOB_PAY { get; set; }
    public string PROFESSION_PAY { get; set; }
    public string FOOD_SUBSIDY { get; set; }

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
    public string LEAVE_A_HOUR { get; set; }
    public string LEAVE_B_HOUR { get; set; }
    public string LEAVE_C_HOUR { get; set; }
    public string LEAVE_Q_HOUR { get; set; }
    public string LEAVE_OP_HOUR { get; set; }
    public string THIRD_CNT_P { get; set; }
    public string SECOND_CNT_P { get; set; }
    public string FIRST_CNT_P { get; set; }
    public string THIRD_CNT_M { get; set; }
    public string SECOND_CNT_M { get; set; }
    public string FIRST_CNT_M { get; set; }
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
    public string CHG_STATUS { get; set; }
    public string PRIMEVAL_FLAG { get; set; }
    public string APPROVE_FLAG { get; set; }
    public string APPROVE_MARK { get; set; }

    //年獎考績格差設定檔
    public string AWARD { get; set; }
    public string EMP_STATUS { get; set; }

    //共用欄位
    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }


    public CFB2SH0200DAO()
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
            sb.Append("select count(0) resultCount from TB_S_M_AWARD_H");
            sb.Append(" where AWARD_YEAR=@AWARD_YEAR");
            sb.Append(" and AWARD_ROUND = @AWARD_ROUND");
            ht.Add("@AWARD_YEAR", AWARD_YEAR);
            ht.Add("@AWARD_ROUND", AWARD_ROUND);
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
            sb.Append(" , e.SUB_DESC AWARD_ROUND_DESC   ");
            sb.Append(" from TB_S_M_AWARD_H a ");
            sb.Append("  left join TB_9_M_COMM_D c on  a.APPROVE_STATUS = c.SUB_CD and c.MAIN_CD = 'APPROVE_STATUS'  and c.SYS_CD='SA' and c.IS_VALID='Y' ");
            sb.Append("  left join TB_9_M_COMM_D e on  a.AWARD_ROUND = e.SUB_CD  and e.MAIN_CD = 'AWARD_ROUND'  and e.IS_VALID='Y' and  e.SYS_CD='SH'  ");
            sb.Append(" where 1=1 ");
            if (AWARD_YEAR != "")
            {
                sb.Append(" and AWARD_YEAR = @AWARD_YEAR ");
                ht.Add("@AWARD_YEAR", AWARD_YEAR);
            }
            if (AWARD_ROUND != "")
            {
                sb.Append(" and AWARD_ROUND = @AWARD_ROUND ");
                ht.Add("@AWARD_ROUND", AWARD_ROUND);
            }

            DataTable dt = dbConn.Query(sb, ht);
            foreach (DataRow dr in dt.Rows)
            {
                this.AWARD_DAYS = Convert.ToString(dr["AWARD_DAYS"]);
                this.AWARD_ROUND_DESC = Convert.ToString(dr["AWARD_ROUND_DESC"]);
                this.AWARD_TOTAL_AMOUNT = Convert.ToString(dr["AWARD_TOTAL_AMOUNT"]);
                this.AWARD_TOTAL_DECIMAL = Convert.ToString(dr["AWARD_TOTAL_DECIMAL"]);
                this.AWARD_DT = dr["AWARD_DT"].ToString() != "" ? Convert.ToDateTime(dr["AWARD_DT"].ToString()).ToString("yyyy/MM/dd") : "";
                this.SALARY_TRANS_DT = dr["SALARY_TRANS_DT"].ToString() != "" ? Convert.ToDateTime(dr["SALARY_TRANS_DT"].ToString()).ToString("yyyy/MM/dd") : "";
                this.APPROVE_STATUS = Convert.ToString(dr["APPROVE_STATUS"]);
                this.APPROVE_STATUS_DESC = Convert.ToString(dr["APPROVE_STATUS_DESC"]);
                this.REMARK = Convert.ToString(dr["REMARK"]);
                this.FREEZE_FLAG = Convert.ToString(dr["FREEZE_FLAG"]);

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
            sb.Append(" from TB_S_M_AWARD_H a ");
            sb.Append(" where 1=1 ");
            if (AWARD_YEAR != "")
            {
                sb.Append(" and AWARD_YEAR = @AWARD_YEAR ");
                ht.Add("@AWARD_YEAR", award_year);
            }
            if (AWARD_ROUND != "")
            {
                sb.Append(" and AWARD_ROUND = @AWARD_ROUND ");
                ht.Add("@AWARD_ROUND", award_round);
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
                            , string award_year_s, string award_year_e, string award_dt_s, string award_dt_e, string award_round
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
            sb.Append(" , e.SUB_DESC AWARD_ROUND_DESC   ");
            sb.Append(" from TB_S_M_AWARD_H a ");
            sb.Append(" left join TB_9_M_COMM_D e on  a.AWARD_ROUND = e.SUB_CD  and e.MAIN_CD = 'AWARD_ROUND'  and e.IS_VALID='Y' and  e.SYS_CD='SH'  ");
            sb.Append(" where 1=1 ");
            //查詢條件-dropDownList
            if (award_round !="" && award_round != "-1")
            {
                sb.Append(" and AWARD_ROUND = @AWARD_ROUND ");
                ht.Add("@AWARD_ROUND", award_round);
            }
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
            if (award_dt_s != "")
            {
                sb.Append(" and AWARD_DT >= @AWARD_DT1 ");
                ht.Add("@AWARD_DT1", Convert.ToDateTime(award_dt_s).ToString("yyyy/MM/dd"));
            }
            if (award_dt_e != "")
            {
                sb.Append(" and AWARD_DT <= @AWARD_DT2 ");
                ht.Add("@AWARD_DT2", Convert.ToDateTime(award_dt_e).ToString("yyyy/MM/dd"));
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
                          , string award_year_s, string award_year_e, string award_dt_s, string award_dt_e, string award_round
                         )
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) resultCount ");
            sb.Append(" from TB_S_M_AWARD_H ");
            sb.Append(" where 1=1 ");

            //查詢條件-dropDownList
            if (award_round != "" && award_round != "-1")
            {
                sb.Append(" and AWARD_ROUND = @AWARD_ROUND ");
                ht.Add("@AWARD_ROUND", award_round);
            }
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
            if (award_dt_s != "")
            {
                sb.Append(" and AWARD_DT >= @AWARD_DT1 ");
                ht.Add("@AWARD_DT1", Convert.ToDateTime(award_dt_s).ToString("yyyy/MM/dd"));
            }
            if (award_dt_e != "")
            {
                sb.Append(" and AWARD_DT <= @AWARD_DT2 ");
                ht.Add("@AWARD_DT2", Convert.ToDateTime(award_dt_e).ToString("yyyy/MM/dd"));
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
                                , string award_round, string award_year, string emp_id, string emp_name, string emp_chg_cd
                                , string level_cd, string pay_type
                           )
    {
        try
        {

            if (sortExpression.Contains("UPDATED_DT"))
                sortExpression = sortExpression.Replace("UPDATED_DT", "a.UPDATED_DT");

            StringBuilder sb_amt1 = new StringBuilder();
            sb_amt1.Append(" ,isnull(( select  AWARD_AMT from TB_S_M_AWARD_DM   ");
            sb_amt1.Append(" where AWARD_YEAR=@AWARD_YEAR and AWARD_ROUND= '1'  ");
            sb_amt1.Append(" and EMP_ID =a.EMP_ID ),0) as amt1                  ");

            StringBuilder sb_amt2 = new StringBuilder();
            sb_amt2.Append(" ,isnull(( select  AWARD_AMT from TB_S_M_AWARD_DM   ");
            sb_amt2.Append(" where AWARD_YEAR=@AWARD_YEAR and AWARD_ROUND= '2'  ");
            sb_amt2.Append(" and EMP_ID =a.EMP_ID ),0) as amt2                  ");
            StringBuilder sb_amt3 = new StringBuilder();
            sb_amt3.Append(" ,isnull(( select  AWARD_AMT from TB_S_M_AWARD_DM   ");
            sb_amt3.Append(" where AWARD_YEAR=@AWARD_YEAR and AWARD_ROUND= '3'  ");
            sb_amt3.Append(" and EMP_ID =a.EMP_ID ),0) as amt3                  ");
            StringBuilder sb_amt_total = new StringBuilder();
            sb_amt_total.Append(" ,isnull( (select  sum(AWARD_AMT) from TB_S_M_AWARD_DM ");
            sb_amt_total.Append(" where AWARD_YEAR=@AWARD_YEAR                          ");
            sb_amt_total.Append(" and EMP_ID =a.EMP_ID and PAY_TYPE !='R' ),0) as amtTotal                 ");

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + ") As RowNumber,");
            sb.Append(" a.*   ");
            sb.Append(" , a.CHG_STATUS + '-' + b.SUB_DESC CHG_STATUS_DESC   ");
            sb.Append(" , a.EMP_CHG_CD + '-' + isnull(d.SUB_DESC,'') EMP_CHG_CD_DESC   ");
            sb.Append(" , a.PAY_TYPE + '-' + e.SUB_DESC PAY_TYPE_DESC       ");
            sb.Append(sb_amt1);
            sb.Append(sb_amt2);
            sb.Append(sb_amt3);
            sb.Append(sb_amt_total);
            sb.Append(" from TB_S_M_AWARD_DM a ");
            sb.Append("  left join TB_9_M_COMM_D b on  a.CHG_STATUS = b.SUB_CD and b.MAIN_CD = 'CHG_STATUS' and b.IS_VALID='Y'  and b.SYS_CD='SA'   ");
            sb.Append("  left join TB_9_M_COMM_D d on  a.EMP_CHG_CD = d.SUB_CD and d.MAIN_CD = 'EMP_CHG_CD' and d.IS_VALID='Y' and d.SYS_CD='HB'    ");
            sb.Append("  left join TB_9_M_COMM_D e on  a.PAY_TYPE = e.SUB_CD and e.MAIN_CD = 'PAY_TYPE' and e.IS_VALID='Y'  and e.SYS_CD='SC'       ");
            sb.Append(" where 1=1 ");

            //查詢條件
            if (award_year != "")
            {
                sb.Append(" and a.AWARD_YEAR = @AWARD_YEAR ");
                ht.Add("@AWARD_YEAR", award_year);
            }
            if (award_round != "")
            {
                sb.Append(" and a.AWARD_ROUND = @AWARD_ROUND ");
                ht.Add("@AWARD_ROUND", award_round);
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
            if (emp_chg_cd != "-1")
            {
                sb.Append(" and EMP_CHG_CD = @EMP_CHG_CD ");
                ht.Add("@EMP_CHG_CD", emp_chg_cd);
            }
            if (level_cd != "")
            {
                sb.Append(" and LEVEL_CD like @LEVEL_CD ");
                ht.Add("@LEVEL_CD", level_cd + "%");
            }
            if (pay_type != "-1")
            {
                sb.Append(" and PAY_TYPE = @PAY_TYPE ");
                ht.Add("@PAY_TYPE", pay_type);
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
                            , string award_round, string award_year, string emp_id, string emp_name, string emp_chg_cd
                            , string level_cd, string pay_type
                        )
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) resultCount ");
            sb.Append(" from TB_S_M_AWARD_DM ");
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
            if (award_round != "")
            {
                sb.Append(" and AWARD_ROUND = @AWARD_ROUND ");
                ht.Add("@AWARD_ROUND", award_round);
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
            if (emp_chg_cd != "-1")
            {
                sb.Append(" and EMP_CHG_CD = @EMP_CHG_CD ");
                ht.Add("@EMP_CHG_CD", emp_chg_cd);
            }
            if (level_cd != "")
            {
                sb.Append(" and LEVEL_CD like @LEVEL_CD ");
                ht.Add("@LEVEL_CD", level_cd + "%");
            }
            if (pay_type != "-1")
            {
                sb.Append(" and PAY_TYPE = @PAY_TYPE ");
                ht.Add("@PAY_TYPE", pay_type);
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
            sb.Append(" select CHG_STATUS, PRIMEVAL_FLAG, APPROVE_FLAG  from  TB_S_M_AWARD_DM ");
            sb.Append(" where AWARD_YEAR = @AWARD_YEAR ");
            sb.Append("  and AWARD_ROUND = @AWARD_ROUND");
            sb.Append("  and EMP_ID = @EMP_ID");

            ht.Add("@AWARD_YEAR", AWARD_YEAR);
            ht.Add("@AWARD_ROUND", AWARD_ROUND);
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
    public void deleteDataH(string year, string round)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("delete from TB_S_M_AWARD_H ");
            sb.Append(" where AWARD_YEAR = @AWARD_YEAR  ");
            sb.Append("  and AWARD_ROUND = @AWARD_ROUND ");
            ht.Add("@AWARD_YEAR", year);
            ht.Add("@AWARD_ROUND", round);
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
            sb.Append("  and AWARD_ROUND = @AWARD_ROUND ");
            ht.Add("@AWARD_YEAR", year);
            ht.Add("@AWARD_ROUND", round);
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
            sb.Append(" update TB_S_M_AWARD_H ");
            sb.Append(" set AWARD_DT = @AWARD_DT ");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT = @UPDATED_DT");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where AWARD_YEAR = @AWARD_YEAR  ");
            sb.Append("  and AWARD_ROUND = @AWARD_ROUND");
            ht.Add("@AWARD_DT", Convert.ToDateTime(AWARD_DT));
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@UPDATED_DT", now);
            ht.Add("@FUNC_ID", FUNC_ID);

            //pk值
            ht.Add("@AWARD_YEAR", AWARD_YEAR);
            ht.Add("@AWARD_ROUND", AWARD_ROUND);

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
            sb.Append(" INSERT INTO TB_S_M_AWARD_H ");
            sb.Append(" ( ");
            sb.Append(" AWARD_YEAR, AWARD_ROUND, AWARD_DT, AWARD_STIME, AWARD_ETIME ");
            sb.Append(" ,CREATED_BY, CREATED_DT, UPDATED_BY, UPDATED_DT, FUNC_ID  ");
            sb.Append(" ) ");
            sb.Append(" VALUES ");
            sb.Append(" ( @AWARD_YEAR,  @AWARD_ROUND,  @AWARD_DT,  @AWARD_STIME,  @AWARD_ETIME  ");
            sb.Append("   ,@CREATED_BY,  @CREATED_DT,  @UPDATED_BY,  @UPDATED_DT,  @FUNC_ID )");

            ht.Add("@AWARD_YEAR", AWARD_YEAR);
            ht.Add("@AWARD_ROUND", AWARD_ROUND);
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
            ht.Add("@AWARD_ROUND", AWARD_ROUND);
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
            sb.Append(" update TB_S_M_AWARD_H ");
            sb.Append(" set RELEASE_DT = @RELEASE_DT ");
            sb.Append(" ,RELEASE_BY = @RELEASE_BY ");
            sb.Append(" ,APPROVE_DT = @APPROVE_DT ");
            sb.Append(" ,APPROVE_BY = @APPROVE_BY ");
            //sb.Append(" ,APPROVE_STATUS = @APPROVE_STATUS ");
            sb.Append(" ,FREEZE_FLAG = @FREEZE_FLAG ");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT = @UPDATED_DT");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where AWARD_YEAR = @AWARD_YEAR ");
            sb.Append("  and AWARD_ROUND = @AWARD_ROUND");

            ht.Add("@RELEASE_DT", DateTime.Now.ToString("yyyy/MM/dd"));
            ht.Add("@RELEASE_BY", SessionHandle.Current.emp_id);
            ht.Add("@APPROVE_DT", DBNull.Value);
            ht.Add("@APPROVE_BY", "");
            //ht.Add("@APPROVE_STATUS", "N"); 因有可能是駁回狀態
            ht.Add("@FREEZE_FLAG", "Y");

            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_DT", now);
            ht.Add("@FUNC_ID", FUNC_ID);

            //pk值
            ht.Add("@AWARD_YEAR", AWARD_YEAR);
            ht.Add("@AWARD_ROUND", AWARD_ROUND);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }


    //薪資轉出
    public void updateAnnounce(DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();


            sb.Append(" update TB_S_M_AWARD_H ");
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
    }


    #endregion


    #region DB存取(Dtl)

    //支付狀態一括更新(年獎明細維護檔)
    public void updatePayType_D(DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" update TB_S_M_AWARD_DM ");
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
            sb_AMT.Append(" , AWARD_TOTAL_AMOUNT = ( select isnull( sum(AWARD_AMT),0) from  TB_S_M_AWARD_DM  ");
            sb_AMT.Append(" where AWARD_YEAR = @AWARD_YEAR ");
            sb_AMT.Append("  and AWARD_ROUND = @AWARD_ROUND ");
            sb_AMT.Append("  and CHG_STATUS <> @CHG_STATUS ");
            sb_AMT.Append("  ) ");
            //總人數
            StringBuilder sb_NUM = new StringBuilder();
            sb_NUM.Append(" , AWARD_TOTAL_DECIMAL = ( select   count(EMP_ID) from  TB_S_M_AWARD_DM ");
            sb_NUM.Append(" where AWARD_YEAR = @AWARD_YEAR ");
            sb_NUM.Append("  and AWARD_ROUND = @AWARD_ROUND ");
            sb_NUM.Append("  and CHG_STATUS <> @CHG_STATUS ");
            sb_NUM.Append("  ) ");


            sb.Append(" update TB_S_M_AWARD_H ");
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
            sb.Append(" update TB_S_M_AWARD_DM          ");
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
            sb_AMT.Append(" AWARD_TOTAL_AMOUNT = ( select isnull( sum(AWARD_AMT),0) from  TB_S_M_AWARD_DM  ");
            sb_AMT.Append(" where AWARD_YEAR = @AWARD_YEAR ");
            sb_AMT.Append("  and AWARD_ROUND = @AWARD_ROUND ");
            sb_AMT.Append("  and CHG_STATUS <> @CHG_STATUS ");
            sb_AMT.Append("  ) ");
            //總人數
            StringBuilder sb_NUM = new StringBuilder();
            sb_NUM.Append(" , AWARD_TOTAL_DECIMAL = ( select   count(EMP_ID) from  TB_S_M_AWARD_DM ");
            sb_NUM.Append(" where AWARD_YEAR = @AWARD_YEAR ");
            sb_NUM.Append("  and AWARD_ROUND = @AWARD_ROUND ");
            sb_NUM.Append("  and CHG_STATUS <> @CHG_STATUS ");
            sb_NUM.Append("  ) ");

            sb.Append(" update TB_S_M_AWARD_H ");
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

    #endregion


    #region DB存取 (EXCEL的新增,修改)
    //判斷工號是否存在於年獎明細維護檔
    internal DataTable getAwardEmpCount(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select count(0) resultCount from TB_S_M_AWARD_DM ");
            sb.Append(" where AWARD_YEAR = @AWARD_YEAR ");
            sb.Append(" and AWARD_ROUND = @AWARD_ROUND ");
            sb.Append(" and EMP_ID = @EMP_ID ");
            ht.Add("@AWARD_YEAR", AWARD_YEAR);
            ht.Add("@AWARD_ROUND", AWARD_ROUND);
            ht.Add("@EMP_ID", emp_id);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //判斷原始考績是否存在
    internal DataTable getAwardBase(string award)
    {
        try
        {
            DataTable dt = new DataTable();
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select count(0) resultCount  from TB_S_M_AWARD_COND  ");
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
            sb.Append("select count(0) resultCount  from TB_S_M_AWARD_COND  ");
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
                sb.Append(" select AWARD_BASE from TB_S_M_AWARD_COND  ");
                sb.Append(" where LEVEL_CD = (select LEVEL_CD from VW_H_EMP_DATA where EMP_ID=@empid) ");
                sb.Append(" and WS_CD=(select WS_CD from VW_H_EMP_DATA where EMP_ID=@empid) ");
                sb.Append(" and AWARD = @AWARD");
                ht.Add("@empid", empid);
                ht.Add("@AWARD", award);
            }
            else {
                sb.Append(" select AWARD_BASE from TB_S_M_AWARD_COND  ");
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
    }
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
                this.SEX_CD = Convert.ToString(dr["SEX_CD"]);
                this.EMP_CHG_CD = Convert.ToString(dr["EMP_CHG_CD"]);
                this.WS_CD = Convert.ToString(dr["WS_CD"]);
                this.JPN_CD = Convert.ToString(dr["JPN_CD"]);
                this.COMPANY_CD = Convert.ToString(dr["COMPANY_CD"]);
                this.LEVEL_CD = Convert.ToString(dr["LEVEL_CD"]);
                this.PJOB_CD = Convert.ToString(dr["PJOB_CD"]);
                this.JOIN_DT =  dr["JOIN_DT"].ToString() != "" ? Convert.ToDateTime(dr["JOIN_DT"].ToString()).ToString("yyyy/MM/dd") : "";
                this.LEAVE_DT = dr["LEAVE_DT"].ToString() != "" ? Convert.ToDateTime(dr["LEAVE_DT"].ToString()).ToString("yyyy/MM/dd") : "";
                //this.STAY_DT =  dr["STAY_DT"].ToString() != "" ? Convert.ToDateTime(dr["STAY_DT"].ToString()).ToString("yyyy/MM/dd") : "";
                this.BE_CONTRACT_DT = dr["BE_CONTRACT_DT"].ToString() != "" ? Convert.ToDateTime(dr["BE_CONTRACT_DT"].ToString()).ToString("yyyy/MM/dd") : "";
                this.BE_EMP_DT = dr["BE_EMP_DT"].ToString() != "" ? Convert.ToDateTime(dr["BE_EMP_DT"].ToString()).ToString("yyyy/MM/dd") : "";
                this.EMP_CD = Convert.ToString(dr["EMP_CD"]);

                //留職停工日
                this.EMP_STATUS = Convert.ToString(dr["EMP_STATUS"]);
                if(this.EMP_STATUS == "02"){
                    this.STAY_DT = this.LEAVE_DT;
                }else{
                    this.STAY_DT = "";
                }


            }

        }
        catch (Exception)
        {

            throw;
        }
    }
    //依EXCEL上傳資料進行修改
    public void updateEMPByUpload_D(DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" declare @wk_ATTEND_DAYS decimal(6,3) =  -1*dbo.FN_S_GET_ATTEND_DAYS_AWARD(@EMP_ID,@EMP_CD,@AWARD_STIME,@AWARD_ETIME) ");
            sb.Append(" update TB_S_M_AWARD_DM ");
            sb.Append(" set  ");
            sb.Append("  WORK_DAYS = @WORK_DAYS ");
            sb.Append(" ,ID_DESC = @ID_DESC ");
            sb.Append(" ,LEVEL_PAY = @LEVEL_PAY ");
            sb.Append(" ,ABILITY_PAY = @ABILITY_PAY ");
            sb.Append(" ,PJOB_PAY = @PJOB_PAY ");
            sb.Append(" ,PROFESSION_PAY = @PROFESSION_PAY ");
            sb.Append(" ,FOOD_SUBSIDY = @FOOD_SUBSIDY ");
            sb.Append(" ,SCORE_2H = @SCORE_2H ");
            sb.Append(" ,AWARD_BASE = @AWARD_BASE ");
            sb.Append(" ,LEAVE_A_HOUR = @LEAVE_A_HOUR ");
            sb.Append(" ,LEAVE_B_HOUR = @LEAVE_B_HOUR ");
            sb.Append(" ,LEAVE_C_HOUR = @LEAVE_C_HOUR ");
            sb.Append(" ,LEAVE_Q_HOUR = @LEAVE_Q_HOUR ");
            sb.Append(" ,LEAVE_OP_HOUR = @LEAVE_OP_HOUR ");
            sb.Append(" ,THIRD_CNT_P = @THIRD_CNT_P ");
            sb.Append(" ,SECOND_CNT_P = @SECOND_CNT_P ");
            sb.Append(" ,FIRST_CNT_P = @FIRST_CNT_P ");
            sb.Append(" ,THIRD_CNT_M = @THIRD_CNT_M ");
            sb.Append(" ,SECOND_CNT_M = @SECOND_CNT_M ");
            sb.Append(" ,FIRST_CNT_M = @FIRST_CNT_M ");
            sb.Append(" ,ATTEND_DAYS = @wk_ATTEND_DAYS ");
            sb.Append(" ,REWARD_DAYS = @REWARD_DAYS ");
            sb.Append(" ,DISCIPLINE_DAYS = @DISCIPLINE_DAYS ");
            sb.Append(" ,AWARD_WORK_DAYS = @AWARD_WORK_DAYS + @wk_ATTEND_DAYS ");
            sb.Append(" ,CHG_STATUS = CASE WHEN APPROVE_FLAG='N'and CHG_STATUS='N' THEN 'N' ELSE 'U' END ");
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
            ht.Add("@WORK_DAYS", WORK_DAYS);
            ht.Add("@ID_DESC", ID_DESC);
            ht.Add("@LEVEL_PAY", LEVEL_PAY);
            ht.Add("@ABILITY_PAY", ABILITY_PAY);
            ht.Add("@PJOB_PAY", PJOB_PAY);
            ht.Add("@PROFESSION_PAY", PROFESSION_PAY);
            ht.Add("@FOOD_SUBSIDY", FOOD_SUBSIDY);
            ht.Add("@SCORE_2H", SCORE_2H);
            ht.Add("@AWARD_BASE", AWARD_BASE);
            ht.Add("@LEAVE_A_HOUR", LEAVE_A_HOUR);
            ht.Add("@LEAVE_B_HOUR", LEAVE_B_HOUR);
            ht.Add("@LEAVE_C_HOUR", LEAVE_C_HOUR);
            ht.Add("@LEAVE_Q_HOUR", LEAVE_Q_HOUR);
            ht.Add("@LEAVE_OP_HOUR", LEAVE_OP_HOUR);
            ht.Add("@THIRD_CNT_P", THIRD_CNT_P);
            ht.Add("@SECOND_CNT_P", SECOND_CNT_P);
            ht.Add("@FIRST_CNT_P", FIRST_CNT_P);
            ht.Add("@THIRD_CNT_M", THIRD_CNT_M);
            ht.Add("@SECOND_CNT_M", SECOND_CNT_M);
            ht.Add("@FIRST_CNT_M", FIRST_CNT_M);
            ht.Add("@ATTEND_DAYS", ATTEND_DAYS);
            ht.Add("@REWARD_DAYS", REWARD_DAYS);
            ht.Add("@DISCIPLINE_DAYS", DISCIPLINE_DAYS);
            ht.Add("@AWARD_WORK_DAYS", AWARD_WORK_DAYS);
            ht.Add("@PAY_TYPE", PAY_TYPE);
            ht.Add("@APPROVE_FLAG", "N");

            //PK值
            ht.Add("@AWARD_YEAR", AWARD_YEAR);
            ht.Add("@AWARD_ROUND", AWARD_ROUND);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@AWARD_STIME", AWARD_STIME);
            ht.Add("@AWARD_ETIME", AWARD_ETIME);
            ht.Add("@EMP_CD", EMP_CD);

            //新修日期
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_DT", now);
            ht.Add("@FUNC_ID", "FB2SH020");

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //依EXCEL上傳資料進行新增
    internal void insertEMPByUpload_D(DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" declare @wk_ATTEND_DAYS decimal(6,3)=  -1*dbo.FN_S_GET_ATTEND_DAYS_AWARD(@EMP_ID,@EMP_CD,@AWARD_STIME,@AWARD_ETIME)          ");
            sb.Append(" INSERT INTO TB_S_M_AWARD_DM ");
            sb.Append(" ( ");
            sb.Append(" AWARD_YEAR, AWARD_ROUND, AWARD_DAYS, EMP_ID, EMP_NAME ");
            sb.Append(" , SEX_CD, EMP_CHG_CD, WS_CD, JPN_CD, COMPANY_CD  ");
            sb.Append(" , DEPT_NO, LEVEL_CD, PJOB_CD, JOIN_DT, LEAVE_DT  ");
            sb.Append(" , STAY_DT, BE_CONTRACT_DT, BE_EMP_DT, WORK_DAYS, EMP_CD  ");
            sb.Append(" , ID_DESC, LEVEL_PAY, ABILITY_PAY, PJOB_PAY, PROFESSION_PAY  ");
            sb.Append(" , FOOD_SUBSIDY, SCORE_2H, AWARD_BASE, LEAVE_A_HOUR, LEAVE_B_HOUR  ");
            sb.Append(" , LEAVE_C_HOUR, LEAVE_Q_HOUR, LEAVE_OP_HOUR, THIRD_CNT_P, SECOND_CNT_P  ");
            sb.Append(" , FIRST_CNT_P, THIRD_CNT_M, SECOND_CNT_M, FIRST_CNT_M, ATTEND_DAYS  ");
            sb.Append(" , REWARD_DAYS, DISCIPLINE_DAYS, AWARD_WORK_DAYS, PAY_TYPE, CHG_STATUS  ");
            sb.Append(" , PRIMEVAL_FLAG, APPROVE_FLAG, APPROVE_MARK  ");
            sb.Append(" , CREATED_BY, CREATED_DT, UPDATED_BY, UPDATED_DT, FUNC_ID  ");
            sb.Append(" ) ");
            sb.Append(" select    @AWARD_YEAR, @AWARD_ROUND, @AWARD_DAYS, @EMP_ID, EMP_NAME  ");
            sb.Append(" , SEX_CD, EMP_CHG_CD, WS_CD, JPN_CD,COMPANY_CD ");
            sb.Append(" , DEPT_NO, LEVEL_CD, PJOB_CD, JOIN_DT, LEAVE_DT ");
            sb.Append(" , @STAY_DT, BE_CONTRACT_DT, BE_EMP_DT, @WORK_DAYS, EMP_CD ");
            sb.Append(" , @ID_DESC, @LEVEL_PAY, @ABILITY_PAY, @PJOB_PAY, @PROFESSION_PAY ");
            sb.Append(" , @FOOD_SUBSIDY, @SCORE_2H, @AWARD_BASE, @LEAVE_A_HOUR, @LEAVE_B_HOUR ");
            sb.Append(" , @LEAVE_C_HOUR, @LEAVE_Q_HOUR, @LEAVE_OP_HOUR, @THIRD_CNT_P, @SECOND_CNT_P ");
            sb.Append(" , @FIRST_CNT_P, @THIRD_CNT_M, @SECOND_CNT_M, @FIRST_CNT_M, @wk_ATTEND_DAYS   ");
            sb.Append(" , @REWARD_DAYS, @DISCIPLINE_DAYS, @AWARD_WORK_DAYS + @wk_ATTEND_DAYS , @PAY_TYPE, @CHG_STATUS ");
            sb.Append(" , @PRIMEVAL_FLAG, @APPROVE_FLAG, @APPROVE_MARK ");
            sb.Append(" ,@CREATED_BY,  @CREATED_DT,  @UPDATED_BY,  @UPDATED_DT,  @FUNC_ID ");
            sb.Append(" from VW_H_EMP_DATA  where EMP_ID =@EMP_ID ");

            /*
            sb.Append(" VALUES ");
            sb.Append(" (  ");
            sb.Append("   @AWARD_YEAR, @AWARD_ROUND, @AWARD_DAYS, @EMP_ID, @EMP_NAME  ");
            sb.Append(" , @SEX_CD, @EMP_CHG_CD, @WS_CD, @JPN_CD, @COMPANY_CD ");
            sb.Append(" , @DEPT_NO, @LEVEL_CD, @PJOB_CD, @JOIN_DT, @LEAVE_DT ");
            sb.Append(" , @STAY_DT, @BE_CONTRACT_DT, @BE_EMP_DT, @WORK_DAYS, @EMP_CD ");
            sb.Append(" , @ID_DESC, @LEVEL_PAY, @ABILITY_PAY, @PJOB_PAY, @PROFESSION_PAY ");
            sb.Append(" , @FOOD_SUBSIDY, @SCORE_2H, @AWARD_BASE, @LEAVE_A_HOUR, @LEAVE_B_HOUR ");
            sb.Append(" , @LEAVE_C_HOUR, @LEAVE_Q_HOUR, @LEAVE_OP_HOUR, @THIRD_CNT_P, @SECOND_CNT_P ");
            sb.Append(" , @FIRST_CNT_P, @THIRD_CNT_M, @SECOND_CNT_M, @FIRST_CNT_M, @wk_ATTEND_DAYS   ");
            sb.Append(" , @REWARD_DAYS, @DISCIPLINE_DAYS, @AWARD_WORK_DAYS + @wk_ATTEND_DAYS , @PAY_TYPE, @CHG_STATUS ");
            sb.Append(" , @PRIMEVAL_FLAG, @APPROVE_FLAG, @APPROVE_MARK ");
            sb.Append(" ,@CREATED_BY,  @CREATED_DT,  @UPDATED_BY,  @UPDATED_DT,  @FUNC_ID )");
            */

            ht.Add("@AWARD_YEAR", AWARD_YEAR);
            ht.Add("@AWARD_ROUND", AWARD_ROUND);
            ht.Add("@AWARD_DAYS", AWARD_DAYS);
            ht.Add("@AWARD_STIME", AWARD_STIME);
            ht.Add("@AWARD_ETIME", AWARD_ETIME);

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@EMP_NAME", EMP_NAME);

            ht.Add("@SEX_CD", SEX_CD);
            ht.Add("@EMP_CHG_CD", EMP_CHG_CD);
            ht.Add("@WS_CD", WS_CD);
            ht.Add("@JPN_CD", JPN_CD);
            ht.Add("@COMPANY_CD", COMPANY_CD);

            ht.Add("@DEPT_NO", DEPT_NO);
            ht.Add("@LEVEL_CD", LEVEL_CD);
            ht.Add("@PJOB_CD", PJOB_CD);
            if (JOIN_DT == "") { ht.Add("@JOIN_DT", DBNull.Value); } else { ht.Add("@JOIN_DT", JOIN_DT); }
            if (LEAVE_DT == "") { ht.Add("@LEAVE_DT", DBNull.Value); } else { ht.Add("@LEAVE_DT", LEAVE_DT); }


            if (STAY_DT == "") { ht.Add("@STAY_DT", DBNull.Value); } else { ht.Add("@STAY_DT", STAY_DT); }
            if (BE_CONTRACT_DT == "") { ht.Add("@BE_CONTRACT_DT", DBNull.Value); } else { ht.Add("@BE_CONTRACT_DT", BE_CONTRACT_DT); }
            if (BE_EMP_DT == "") { ht.Add("@BE_EMP_DT", DBNull.Value); } else { ht.Add("@BE_EMP_DT", BE_EMP_DT); }
            ht.Add("@WORK_DAYS", WORK_DAYS);
            ht.Add("@EMP_CD", EMP_CD);

            ht.Add("@ID_DESC", ID_DESC);
            ht.Add("@LEVEL_PAY", LEVEL_PAY);
            ht.Add("@ABILITY_PAY", ABILITY_PAY);
            ht.Add("@PJOB_PAY", PJOB_PAY);
            ht.Add("@PROFESSION_PAY", PROFESSION_PAY);

            ht.Add("@FOOD_SUBSIDY", FOOD_SUBSIDY);
            ht.Add("@SCORE_2H", SCORE_2H);
            ht.Add("@AWARD_BASE", AWARD_BASE);
            ht.Add("@LEAVE_A_HOUR", LEAVE_A_HOUR);
            ht.Add("@LEAVE_B_HOUR", LEAVE_B_HOUR);

            ht.Add("@LEAVE_C_HOUR", LEAVE_C_HOUR);
            ht.Add("@LEAVE_Q_HOUR", LEAVE_Q_HOUR);
            ht.Add("@LEAVE_OP_HOUR", LEAVE_OP_HOUR);
            ht.Add("@THIRD_CNT_P", THIRD_CNT_P);
            ht.Add("@SECOND_CNT_P", SECOND_CNT_P);

            ht.Add("@FIRST_CNT_P", FIRST_CNT_P);
            ht.Add("@THIRD_CNT_M", THIRD_CNT_M);
            ht.Add("@SECOND_CNT_M", SECOND_CNT_M);
            ht.Add("@FIRST_CNT_M", FIRST_CNT_M);
            ht.Add("@ATTEND_DAYS", ATTEND_DAYS);

            ht.Add("@REWARD_DAYS", REWARD_DAYS);
            ht.Add("@DISCIPLINE_DAYS", DISCIPLINE_DAYS);
            ht.Add("@AWARD_WORK_DAYS", AWARD_WORK_DAYS);
            ht.Add("@PAY_TYPE", PAY_TYPE);
            ht.Add("@CHG_STATUS", "N");//新增

            ht.Add("@PRIMEVAL_FLAG", "");
            ht.Add("@APPROVE_FLAG", "N");
            ht.Add("@APPROVE_MARK", "");

            ht.Add("@CREATED_BY", UPDATED_BY);
            ht.Add("@CREATED_DT", now);
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


    //更新年獎維護檔(對象新修時)，還原提出核可、年獎對象及金額、支付狀態(年獎維護檔)、反映項目、計算生成日、總金額=0
    public void updateEMPByUpload_H(DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //總金額
            StringBuilder sb_AMT = new StringBuilder();
            sb_AMT.Append(" , AWARD_TOTAL_AMOUNT = @AWARD_TOTAL_AMOUNT  ");
            //總人數
            StringBuilder sb_NUM = new StringBuilder();
            sb_NUM.Append(" , AWARD_TOTAL_DECIMAL = ( select   count(EMP_ID) from  TB_S_M_AWARD_DM ");
            sb_NUM.Append(" where AWARD_YEAR = @AWARD_YEAR ");
            sb_NUM.Append("  and AWARD_ROUND = @AWARD_ROUND ");
            sb_NUM.Append("  and CHG_STATUS <> @CHG_STATUS ");
            sb_NUM.Append("  ) ");

            sb.Append(" update TB_S_M_AWARD_H ");
            sb.Append(" set AWARD_DAYS = @AWARD_DAYS ");
            sb.Append(sb_AMT);
            sb.Append(sb_NUM);
            sb.Append(" ,GEN_DT = @GEN_DT");
            sb.Append(" ,RELEASE_DT = @RELEASE_DT");
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
            ht.Add("@AWARD_TOTAL_AMOUNT", 0);
            ht.Add("@AWARD_DAYS", 0);
            ht.Add("@GEN_DT", DBNull.Value);
            ht.Add("@RELEASE_DT", DBNull.Value);
            ht.Add("@RELEASE_BY", "");
            ht.Add("@APPROVE_DT", DBNull.Value);
            ht.Add("@APPROVE_BY", "");
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

    //更新明細維護檔的年獎金額為0
    public void updateT0Zero_D(DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" update TB_S_M_AWARD_DM ");
            sb.Append(" set  ");
            sb.Append("  AWARD_AMT = @AWARD_AMT ");
            sb.Append(" ,AWARD_TAX = @AWARD_TAX ");
            sb.Append(" ,AWARD_AMT_R = @AWARD_AMT_R ");
            sb.Append(" ,AWARD_AMT_TMEP = @AWARD_AMT_TMEP ");
            sb.Append(" ,AWARD_AMT_LEVEL = @AWARD_AMT_LEVEL ");
            sb.Append(" where AWARD_YEAR = @AWARD_YEAR ");
            sb.Append("  and AWARD_ROUND = @AWARD_ROUND");

            //set值
            ht.Add("@AWARD_AMT", 0);
            ht.Add("@AWARD_TAX", 0);
            ht.Add("@AWARD_AMT_R", 0);
            ht.Add("@AWARD_AMT_TMEP", 0);
            ht.Add("@AWARD_AMT_LEVEL", 0);

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



    //取得EXCEL下載資料(維護檔)，
    public DataTable getMaintainData(string tableName)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            StringBuilder sb_amt1 = new StringBuilder();
            sb_amt1.Append(" ,isnull(( select  AWARD_AMT from TB_S_M_AWARD_DM   ");
            sb_amt1.Append(" where AWARD_YEAR=@AWARD_YEAR and AWARD_ROUND= '1'  ");
            sb_amt1.Append(" and EMP_ID =a.EMP_ID ),0) as amt1                  ");

            StringBuilder sb_amt2 = new StringBuilder();
            sb_amt2.Append(" ,isnull(( select  AWARD_AMT from TB_S_M_AWARD_DM   ");
            sb_amt2.Append(" where AWARD_YEAR=@AWARD_YEAR and AWARD_ROUND= '2'  ");
            sb_amt2.Append(" and EMP_ID =a.EMP_ID ),0) as amt2                  ");
            StringBuilder sb_amt3 = new StringBuilder();
            sb_amt3.Append(" ,isnull(( select  AWARD_AMT from TB_S_M_AWARD_DM   ");
            sb_amt3.Append(" where AWARD_YEAR=@AWARD_YEAR and AWARD_ROUND= '3'  ");
            sb_amt3.Append(" and EMP_ID =a.EMP_ID ),0) as amt3                  ");
            StringBuilder sb_amt_total = new StringBuilder();
            sb_amt_total.Append(" ,isnull( (select  sum(AWARD_AMT) from TB_S_M_AWARD_DM ");
            sb_amt_total.Append(" where AWARD_YEAR=@AWARD_YEAR                          ");
            sb_amt_total.Append(" and EMP_ID =a.EMP_ID ),0) as amtTotal                 ");

            StringBuilder items = new StringBuilder();
            items.Append(" ,( select   ");
            items.Append("    CASE   when AWARD_ITEM_A ='Y'    then '考績,' ELSE '' END         ");
            items.Append("  + CASE   when AWARD_ITEM_RP ='Y'    then '獎懲,' ELSE '' END        ");
            items.Append("  + CASE   when AWARD_ITEM_AL ='Y'    then '勤怠,' ELSE '' END        ");
            items.Append("  + CASE   when AWARD_ITEM_D ='Y'    then '紀律' ELSE '' END          ");
            items.Append(" from TB_S_M_AWARD_H                                                  ");
            items.Append(" where AWARD_YEAR=@AWARD_YEAR and AWARD_ROUND=@AWARD_ROUND            ");
            items.Append(" ) as items                                                           ");


            sb.Append(" select a.*   ");
            sb.Append(" , a.CHG_STATUS + '-' + b.SUB_DESC CHG_STATUS_DESC   ");
            sb.Append(" , a.EMP_CD + '-' +      isnull(c.SUB_DESC,'') EMP_CD_DESC   ");
            sb.Append(" , a.EMP_CHG_CD + '-' +  isnull( d.SUB_DESC,'')  EMP_CHG_CD_DESC   ");
            sb.Append(" , a.PAY_TYPE + '-' + e.SUB_DESC PAY_TYPE_DESC       ");
            sb.Append(sb_amt1);
            sb.Append(sb_amt2);
            sb.Append(sb_amt3);
            sb.Append(sb_amt_total);
            sb.Append(items);
            //sb.Append(" from TB_S_M_AWARD_DM a ");
            sb.Append(" from " + tableName + " a ");
            sb.Append("  left join TB_9_M_COMM_D b on  a.CHG_STATUS = b.SUB_CD and b.MAIN_CD = 'CHG_STATUS' and b.IS_VALID='Y'  and b.SYS_CD='SA'   ");
            sb.Append("  left join TB_9_M_COMM_D c on  a.EMP_CD = c.SUB_CD and c.MAIN_CD = 'EMP_CD' and b.IS_VALID='Y' and c.SYS_CD='HB' ");
            sb.Append("  left join TB_9_M_COMM_D d on  a.EMP_CHG_CD = d.SUB_CD and d.MAIN_CD = 'EMP_CHG_CD' and d.IS_VALID='Y' and d.SYS_CD='HB'    ");
            sb.Append("  left join TB_9_M_COMM_D e on  a.PAY_TYPE = e.SUB_CD and e.MAIN_CD = 'PAY_TYPE' and e.IS_VALID='Y'  and e.SYS_CD='SC'       ");
            sb.Append(" where 1=1 ");

            sb.Append(" and a.AWARD_YEAR = @AWARD_YEAR ");
            ht.Add("@AWARD_YEAR", AWARD_YEAR);
            sb.Append(" and a.AWARD_ROUND = @AWARD_ROUND ");
            ht.Add("@AWARD_ROUND", AWARD_ROUND);

            sb.Append(" order by APPROVE_MARK DESC, UPDATED_DT DESC, EMP_ID ASC ");

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //取得EXCEL下載資料(昇格檔)，
    public DataTable getLevelUpData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            StringBuilder sb_amt1 = new StringBuilder();
            sb_amt1.Append(" ,isnull(( select  AWARD_AMT_TMEP from TB_S_M_AWARD_DM   ");
            sb_amt1.Append(" where AWARD_YEAR=@AWARD_YEAR and AWARD_ROUND= '1'  ");
            sb_amt1.Append(" and EMP_ID =a.EMP_ID ),0) as amt1                  ");

            StringBuilder sb_amt2 = new StringBuilder();
            sb_amt2.Append(" ,isnull(( select  AWARD_AMT_TMEP from TB_S_M_AWARD_DM   ");
            sb_amt2.Append(" where AWARD_YEAR=@AWARD_YEAR and AWARD_ROUND= '2'  ");
            sb_amt2.Append(" and EMP_ID =a.EMP_ID ),0) as amt2                  ");
            StringBuilder sb_amt3 = new StringBuilder();
            sb_amt3.Append(" ,isnull(( select  AWARD_AMT_TMEP from TB_S_M_AWARD_DM   ");
            sb_amt3.Append(" where AWARD_YEAR=@AWARD_YEAR and AWARD_ROUND= '3'  ");
            sb_amt3.Append(" and EMP_ID =a.EMP_ID ),0) as amt3                  ");

            StringBuilder sb_amt1_b = new StringBuilder();
            sb_amt1.Append(" ,isnull(( select  AWARD_AMT_LEVEL from TB_S_M_AWARD_DM   ");
            sb_amt1.Append(" where AWARD_YEAR=@AWARD_YEAR and AWARD_ROUND= '1'  ");
            sb_amt1.Append(" and EMP_ID =a.EMP_ID ),0) as amt1_before                  ");

            StringBuilder sb_amt2_b = new StringBuilder();
            sb_amt2.Append(" ,isnull(( select  AWARD_AMT_LEVEL from TB_S_M_AWARD_DM   ");
            sb_amt2.Append(" where AWARD_YEAR=@AWARD_YEAR and AWARD_ROUND= '2'  ");
            sb_amt2.Append(" and EMP_ID =a.EMP_ID ),0) as amt2_before                  ");
            StringBuilder sb_amt3_b = new StringBuilder();
            sb_amt3.Append(" ,isnull(( select  AWARD_AMT_LEVEL from TB_S_M_AWARD_DM   ");
            sb_amt3.Append(" where AWARD_YEAR=@AWARD_YEAR and AWARD_ROUND= '3'  ");
            sb_amt3.Append(" and EMP_ID =a.EMP_ID ),0) as amt3_before                  ");

            sb.Append(" select  a.EMP_ID, a.EMP_NAME ,a.LEVEL_CD  ");
            sb.Append(" ,LEVEL_PAY,ABILITY_PAY,PJOB_PAY,PROFESSION_PAY,FOOD_SUBSIDY  ");
            sb.Append(" ,LEVEL_PAY_BEFORE,ABILITY_PAY_BEFORE,PJOB_PAY_BEFORE,PROFESSION_PAY_BEFORE,FOOD_SUBSIDY_BEFORE  ");
            sb.Append(sb_amt1);
            sb.Append(sb_amt2);
            sb.Append(sb_amt3);
            //sb.Append(" from TB_S_M_AWARD_DM a ");
            sb.Append(" from TB_S_M_AWARD_DM a ");
            sb.Append(" where 1=1 ");
            sb.Append(" and a.LEVELUP_FLAG = @LEVELUP_FLAG ");
            sb.Append(" and a.AWARD_YEAR = @AWARD_YEAR ");
            sb.Append(" and a.AWARD_ROUND = @AWARD_ROUND ");
            ht.Add("@LEVELUP_FLAG", "V");
            ht.Add("@AWARD_YEAR", AWARD_YEAR);
            ht.Add("@AWARD_ROUND", AWARD_ROUND);

            sb.Append(" order by APPROVE_MARK DESC, UPDATED_DT DESC, EMP_ID ASC ");

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }



}