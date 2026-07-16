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
/// CFB2SQ0200DAO 的摘要描述
/// </summary>
public class CFB2SQ0200DAO : BaseDAO
{
    //dj030基本欄位
    public string EMP_ID { get; set; }
    public string IS_CLOSE { get; set; }
    public string REMARK { get; set; }
    public string SALARY_YM { get; set; }

    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }

    //ACES
    bool isSuper { get; set; }  //是否為supervisor(擔當)

    ACESLib.ACES aces;
    public CFB2SQ0200DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
      

    //修改 結案否
    public void updateIS_CLOSE_YN()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            DateTime now = DateTime.Parse(DateTime.Now.ToString());
            sb.Append(" update TB_S_M_MATERNITY_LEAVE_H ");
            sb.Append(" set IS_CLOSE=@IS_CLOSE ");
            sb.Append(" ,REMARK = @REMARK");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT = GETDATE()");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where 1=1");
            sb.Append(" and EMP_ID = @EMP_ID ");
            sb.Append(" and SALARY_YM = @SALARY_YM ");

            ht.Add("@REMARK", REMARK);
            ht.Add("@IS_CLOSE", IS_CLOSE);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            //PK值
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SALARY_YM", SALARY_YM);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }


    }


    public DataTable geExceltData()
    {
        try
        {            
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
           

            sb.Append(" Select ");
            sb.Append(" a.EMP_ID,a.SALARY_YM,CALENDAR_SUMDAY,TOTAL_COMPUTER_DAY,TOTAL_SUM_PAY,SPECIAL_PAY,OTHER_PAY,TOTAL_PAY,AVG_PAY,  ");
            sb.Append(" LAST_MONTH_YM,THIS_MONTH_YM,convert(varchar(10),MATERNITY_SDT,111) MATERNITY_SDT,LAST_MONTH_ABILITY_PAY ");
            sb.Append(" ,LAST_MONTH_LEVEL_PAY,LAST_MONTH_PROFESSION_PAY,LAST_MONTH_PJOB_PAY,LAST_MONTH_FOOD_PAY ");
            sb.Append(" ,LAST_MONTH_ADJ_PAY,LAST_MONTH_OUT_PAY,LAST_MONTH_OTHER_PAY,LAST_MONTH_SUM_PAY ");
            sb.Append(" ,LAST_MONTH_WORK_SHIFT_PAY,LAST_MONTH_ENV_PAY,LAST_PLANT_PAY,PLANT_PAY,LAST_MONTH_OVERTIME_PAY,LAST_MONTH_LEAVE_A_AMT ");
            sb.Append(" ,LAST_MONTH_LEAVE_Q_AMT,LAST_MONTH_LEAVE_OP_AMT,LAST_MONTH_SUM_PAY2 ");
            sb.Append(" ,THIS_MONTH_ABILITY_PAY,THIS_MONTH_LEVEL_PAY,THIS_MONTH_PROFESSION_PAY,THIS_MONTH_PJOB_PAY ");
            sb.Append("   ,THIS_MONTH_FOOD_PAY,THIS_MONTH_ADJ_PAY,THIS_MONTH_OUT_PAY,THIS_MONTH_OTHER_PAY,THIS_MONTH_PLANT_PAY,THIS_MONTH_SUM_PAY2 ");
            sb.Append(" ,convert(varchar(10),APPLY_LEAVE_SDT,111)  APPLY_LEAVE_SDT,convert(varchar(10),APPLY_LEAVE_EDT,111)  APPLY_LEAVE_EDT ");
            sb.Append(" ,MATERNITY_SUMDAY,SIX_MONTH_DAILY,LAST_MONTH_DAILY,THIS_MONTH_DAILY,MATERNITY_AMOUNT,a.REMARK, IIF(IS_CLOSE='Y','Y-是','N-否')  IS_CLOSE_DESC,  IS_CLOSE   ");
            sb.Append(" ,b.EMP_NAME  ");
            sb.Append(" ,c.YM,c.CALENDAR_DAY,c.LEAVE_B_HOUR,c.LEAVE_B_DAY,c.LEAVE_M_DAY,c.LEAVE_W_DAY,c.LEAVE_H_DAY,c.LEAVE_G_DAY,c.LEAVE_S_DAY,c.COMPUTER_DAY,c.LEAVE_A_HOURS,c.LEAVE_OP_TIMES,c.LEAVE_Q_HOURS,c.ABILITY_PAY,c.LEVEL_PAY,c.PROFESSION_PAY,c.PJOB_PAY,c.FOOD_PAY,c.ADJ_PAY,c.OUT_PAY,c.SUM_PAY,c.SUM_PAY_BYDAY,c.WORK_SHIFT_PAY,c.ENV_PAY,c.OVERTIME_PAY,c.LEAVE_A_AMT,c.LEAVE_Q_AMT,c.LEAVE_OP_AMT,c.SUM_PAY2  ");
            sb.Append(" from TB_S_M_MATERNITY_LEAVE_H a ");
            sb.Append(" left join TB_H_M_EMP b on a.EMP_ID=b.EMP_ID ");
            sb.Append(" left join TB_S_M_MATERNITY_LEAVE_D c on a.EMP_ID=c.EMP_ID and a.SALARY_YM =c.SALARY_YM ");
            sb.Append(" where a.EMP_ID = @EMP_ID and a.SALARY_YM =  @SALARY_YM");

            ht.Add("@EMP_ID" , EMP_ID);
            ht.Add("@SALARY_YM", SALARY_YM);

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    public DataTable geExceltData2(List<Tuple<string, string>> dataList)
    {
        try
        {
            int now = 0;
            int dataRow = 0;
            dataRow = dataList.Count;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select ");
            sb.Append(" a.EMP_ID,SALARY_YM,convert(varchar(10),MATERNITY_SDT,111) MATERNITY_SDT  ");
            sb.Append(" ,convert(varchar(10),APPLY_LEAVE_SDT,111)  APPLY_LEAVE_SDT,convert(varchar(10),APPLY_LEAVE_EDT,111)  APPLY_LEAVE_EDT ");
            sb.Append(" ,MATERNITY_SUMDAY,SIX_MONTH_DAILY,LAST_MONTH_DAILY,THIS_MONTH_DAILY,MATERNITY_AMOUNT,a.REMARK, IIF(IS_CLOSE='Y','Y-是','N-否')  IS_CLOSE_DESC,  IS_CLOSE   ");
            sb.Append(" ,b.EMP_NAME  ");
            sb.Append(" from TB_S_M_MATERNITY_LEAVE_H a ");
            sb.Append(" left join TB_H_M_EMP b on a.EMP_ID=b.EMP_ID ");
            sb.Append(" where 1=1 and ");

            
            foreach (var dataitem in dataList)
            {
                sb.Append(" ( a.EMP_ID = @EMP_ID" + now.ToString() + "  and a.SALARY_YM =  @SALARY_YM" + now.ToString() + " )");
                ht.Add("@EMP_ID" + now.ToString(), dataitem.Item1);
                ht.Add("@SALARY_YM" + now.ToString(), dataitem.Item2);
                now = now + 1;
                if (now < dataRow)
                {
                    sb.Append(" or " );
                }
                
            }         

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    public DataTable geExceltDataH()
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select ");
            sb.Append(" *   ");
            sb.Append(" ,Convert(int,round((AVG_PAY*RETIRE_BASE_MONTH ),0)) as RETIRE_PAY_EMP  ");
            sb.Append(" from TB_S_M_OLD_PENSION_H H ");
            sb.Append(" where 1=1 ");
            sb.Append(" and H.COMPUTER_TYPE = @COMPUTER_TYPE ");
            sb.Append(" and H.EMP_ID = @EMP_ID ");
            //ht.Add("@COMPUTER_TYPE", COMPUTER_TYPE);
            ht.Add("@EMP_ID", EMP_ID);

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }


    #region Qry Gridview 資料
    //Gridview 查詢資料
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression
            , string ym_st, string ym_ed, string emp_id, string is_close
                           )
    {
        try
        {

            if (sortExpression.Contains("EMP_ID"))
                sortExpression = sortExpression.Replace("EMP_ID", "a.EMP_ID");
            if (sortExpression.Contains("REMARK"))
                sortExpression = sortExpression.Replace("REMARK", "a.REMARK");
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + ") As RowNumber,");
            sb.Append(" a.EMP_ID,SALARY_YM,convert(varchar(10),MATERNITY_SDT,111) MATERNITY_SDT  ");
            sb.Append(" ,convert(varchar(10),APPLY_LEAVE_SDT,111)  APPLY_LEAVE_SDT,convert(varchar(10),APPLY_LEAVE_EDT,111)  APPLY_LEAVE_EDT ");
            sb.Append(" ,MATERNITY_SUMDAY,SIX_MONTH_DAILY,LAST_MONTH_DAILY,THIS_MONTH_DAILY,MATERNITY_AMOUNT,a.REMARK, IIF(IS_CLOSE='Y','Y-是','N-否')  IS_CLOSE_DESC,  IS_CLOSE   ");
            sb.Append(" ,b.EMP_NAME  ");
            sb.Append(" from TB_S_M_MATERNITY_LEAVE_H a ");
            sb.Append(" left join TB_H_M_EMP b on a.EMP_ID=b.EMP_ID ");
            sb.Append(" where 1=1 ");
            
            //查詢條件
            if (ym_st != "")
            {
                sb.Append(" and a.SALARY_YM >= @ym_st ");
                ht.Add("@ym_st", ym_st.Replace("/",""));
            }
            if (ym_ed != "")
            {
                sb.Append(" and a.SALARY_YM <= @ym_ed ");
                ht.Add("@ym_ed", ym_ed.Replace("/", ""));
            }
            if (emp_id != "")
            {
                sb.Append(" and a.EMP_ID like @EMP_ID ");
                ht.Add("@EMP_ID", emp_id + "%");
            }

            if (is_close != "-1")
            {
                sb.Append(" and a.IS_CLOSE=@is_close ");
                ht.Add("@is_close", is_close);
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
              , string ym_st, string ym_ed, string emp_id, string is_close)
    {
        try
        {
            int result = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) resultCount ");
            sb.Append(" from TB_S_M_MATERNITY_LEAVE_H  a");
            sb.Append(" left join TB_H_M_EMP b on a.EMP_ID=b.EMP_ID ");
            sb.Append(" where 1=1 ");

            //查詢條件
            if (ym_st != "")
            {
                sb.Append(" and a.SALARY_YM >= @ym_st ");
                ht.Add("@ym_st", ym_st.Replace("/", ""));
            }
            if (ym_ed != "")
            {
                sb.Append(" and a.SALARY_YM <= @ym_ed ");
                ht.Add("@ym_ed", ym_ed.Replace("/", ""));
            }
            if (emp_id != "")
            {
                sb.Append(" and a.EMP_ID like @EMP_ID ");
                ht.Add("@EMP_ID", emp_id + "%");
            }

            if (is_close != "-1")
            {
                sb.Append(" and a.IS_CLOSE=@is_close ");
                ht.Add("@is_close", is_close);
            }

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                result = (int)dt.Rows[0]["resultCount"];
            }
            return result;
        }
        catch (Exception)
        {

            throw;
        }

    }

    #endregion



    #region Dtl Gridview 資料
   
    #endregion




}