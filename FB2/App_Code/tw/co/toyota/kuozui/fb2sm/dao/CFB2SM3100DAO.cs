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
/// CFB2SM3100DAO 的摘要描述
/// </summary>
public class CFB2SM3100DAO : BaseDAO
{
    //dj030基本欄位
    public string ASSESS_YEAR { get; set; }
    public string ASSESS_TYPE { get; set; }
    public string EMP_ID { get; set; }
    public string EMP_NAME { get; set; }
    public string DEPT_NO { get; set; }
    public string DEPT_NAME { get; set; }
    public string LEVEL_CD { get; set; }
    public string WS_CD { get; set; }

    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }

    //提案資料
    public string PROPOSAL_TOTAL { get; set; }
    public string PROPOSAL_GRADE { get; set; }
    public string PROPOSAL_GRADE_MEAN { get; set; }
    public string PROPOSAL_6 { get; set; }
    public string ASSESS_YM_S { get; set; }
    public string ASSESS_YM_E { get; set; }

    //ACES
    bool isSuper { get; set; }  //是否為supervisor(擔當)

    ACESLib.ACES aces;
    public CFB2SM3100DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public DataTable getMaxAssessYear()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select isnull(max(ASSESS_YEAR),0) as MAX_ASSESS_YEAR from TB_S_R_ASSESS_TARGET");
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //資料生成
    internal void execSP_S_ASSESS_REVIEW()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_S_ASSESS_REVIEW");
            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@USERID", CREATED_BY);//CREATED_BY
            ht.Add("@FUNCID", FUNC_ID);
            dbConn.ExecuteSPT(sb, ht, true);

        }
        catch (Exception)
        {

            throw;
        }

    }

    //EXCEL下載資料-
    public DataTable getExcelData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(@" Select "
                      + " a.*   "
                      + " , a.WS_CD + '-'       + c.SUB_DESC WS_CD_DESC   "
                      + " , a.SEX_CD + '-'      + d.SUB_DESC SEX_CD_DESC   "
                      + " , a.PLANT_CD + '-'    + f.SUB_DESC PLANT_CD_DESC  "
                      + " , a.EMP_CD + '-'      + g.SUB_DESC EMP_CD_DESC   "
                      + " , a.EDUCATION_CD + '-'+ h.SUB_DESC EDUCATION_CD_DESC   "
                      + " , a.EMP_CHG_CD + '-'  + i.SUB_DESC EMP_CHG_CD_DESC   "
                      + " , a.OVERTIME_CTL_CD + '-' + j.SUB_DESC OVERTIME_CTL_CD_DESC    "
                      + " from TB_S_R_ASSESS_REVIEW a "
                      + "  left join TB_9_M_COMM_D c on  a.WS_CD = c.SUB_CD       and c.MAIN_CD = 'WS_CD'        and c.IS_VALID='Y'  and c.SYS_CD='HB' "
                      + "  left join TB_9_M_COMM_D d on  a.SEX_CD = d.SUB_CD      and d.MAIN_CD = 'SEX_CD'       and d.IS_VALID='Y'  and d.SYS_CD='HB' "
                      + "  left join TB_9_M_COMM_D f on  a.PLANT_CD = f.SUB_CD    and f.MAIN_CD = 'PLANT_CD'     and f.IS_VALID='Y'  and f.SYS_CD='HB' "
                      + "  left join TB_9_M_COMM_D g on  a.EMP_CD = g.SUB_CD      and g.MAIN_CD = 'EMP_CD'       and g.IS_VALID='Y'  and g.SYS_CD='HB' "
                      + "  left join TB_9_M_COMM_D h on  a.EDUCATION_CD = h.SUB_CD and h.MAIN_CD = 'EDUCATION_CD' and h.IS_VALID='Y'  and h.SYS_CD='HB' "
                      + "  left join TB_9_M_COMM_D i on  a.EMP_CHG_CD = i.SUB_CD  and i.MAIN_CD = 'EMP_CHG_CD'   and i.IS_VALID='Y'  and i.SYS_CD='HB' "
                      + "  left join TB_9_M_COMM_D j on  a.OVERTIME_CTL_CD = j.SUB_CD and j.MAIN_CD = 'OVERTIME_CTL_CD'  and j.IS_VALID='Y'  and j.SYS_CD='HB'  "

                      );
            sb.Append(" where 1=1 ");

            //查詢條件

            if (ASSESS_YEAR != "")
            {
                sb.Append(" and ASSESS_YEAR = @assess_year ");
                ht.Add("@assess_year", ASSESS_YEAR);
            }
            if (EMP_ID != "")
            {
                sb.Append(" and EMP_ID like @EMP_ID ");
                ht.Add("@EMP_ID", EMP_ID + "%");
            }
            if (EMP_NAME != "")
            {
                sb.Append(" and EMP_NAME like @EMP_NAME ");
                ht.Add("@EMP_NAME", EMP_NAME + "%");
            }
            if (DEPT_NO != "")
            {
                sb.Append(" and DEPT_NO like @DEPT_NO ");
                ht.Add("@DEPT_NO", DEPT_NO + "%");
            }
            /*
            if (DEPT_NAME != "")
            {
                sb.Append(" and DEPT_NAME like @DEPT_NAME ");
                ht.Add("@DEPT_NAME", DEPT_NAME + "%");
            }
             */ 
            if (LEVEL_CD != "")
            {
                sb.Append(" and LEVEL_CD like @LEVEL_CD ");
                ht.Add("@LEVEL_CD", LEVEL_CD + "%");
            }
            if (WS_CD != "-1")
            {
                sb.Append(" and WS_CD = @WS_CD ");
                ht.Add("@WS_CD", WS_CD);
            }
            sb.Append(" order by a.EMP_ID ASC, a.ASSESS_YEAR DESC, a.DEPT_NO ASC ");


            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }


    //AS400  B1CLIB.DB1CMB10EMP(提案用個人檔)
    public DataTable getCMB10EMP()
    {
        try
        {
            string sub_Str = " ";

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" select "
                      + " FB3WNO,count(FB3TID) as total, sum(FB3TSC) as grade  "
                      + " ,case when count(FB3TID)=0 then 0 else (sum(FB3TSC) / count(FB3TID) ) end as mean "
                      + " ,( select count(FB3RWD) from [B1CLIB].[Q1A_DATABASE_SRVR].B1CLIB.DB1CMB30   where FB3TYM>=" + ASSESS_YM_S + "  and FB3TYM<=" + ASSESS_YM_E + " and FB3WNO=a.FB3WNO and  FB3RWD>=200  "
                      + " and FB3WNO in (select emp_id from TB_S_R_ASSESS_REVIEW where ASSESS_YEAR='" + ASSESS_YEAR + "')	 ) as LTotal "
                      + " from [B1CLIB].[Q1A_DATABASE_SRVR].B1CLIB.DB1CMB30  a   "
                      + " where FB3TYM>=" + ASSESS_YM_S + "  and FB3TYM<=" + ASSESS_YM_E + " "
                      + " and FB3WNO in (select emp_id from TB_S_R_ASSESS_REVIEW where ASSESS_YEAR='" + ASSESS_YEAR + "')	  "
                      + " group by FB3WNO order by FB3WNO ");
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }


    //更新提案資料
    internal void updateData(DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_S_R_ASSESS_REVIEW ");
            sb.Append(" set PROPOSAL_TOTAL = @PROPOSAL_TOTAL ");
            sb.Append(" ,PROPOSAL_GRADE = @PROPOSAL_GRADE");
            sb.Append(" ,PROPOSAL_GRADE_MEAN = @PROPOSAL_GRADE_MEAN");
            sb.Append(" ,PROPOSAL_6 = @PROPOSAL_6");
            sb.Append(" where ASSESS_YEAR = @ASSESS_YEAR ");
            sb.Append("  and EMP_ID = @EMP_ID");

            //SET值
            ht.Add("@PROPOSAL_TOTAL", PROPOSAL_TOTAL);
            ht.Add("@PROPOSAL_GRADE", PROPOSAL_GRADE);
            ht.Add("@PROPOSAL_GRADE_MEAN", PROPOSAL_GRADE_MEAN);
            ht.Add("@PROPOSAL_6", PROPOSAL_6);

            //PK值
            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@EMP_ID", EMP_ID);


            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }




    #region Gridview 資料
    //Gridview 查詢資料
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression
            , string assess_year, string emp_id, string emp_name, string dept_no, string level_cd, string ws_cd
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
            sb.Append(" a.*   ");
            sb.Append(" , a.DEPT_NAME_20 + ' ' +a.DEPT_NAME_30 + ' '+ DEPT_NAME_40  DEPT_NAME_DESC");
            sb.Append(" , a.WS_CD + '-' + c.SUB_DESC WS_CD_DESC   ");
            sb.Append(" , a.OVERTIME_CTL_CD + '-' + d.SUB_DESC OVERTIME_CTL_CD_DESC   ");
            sb.Append(" from TB_S_R_ASSESS_REVIEW a ");
            sb.Append("  left join TB_9_M_COMM_D c on  a.WS_CD = c.SUB_CD and c.MAIN_CD = 'WS_CD'  and c.IS_VALID='Y'  and c.SYS_CD='HB' ");
            sb.Append("  left join TB_9_M_COMM_D d on  a.OVERTIME_CTL_CD = d.SUB_CD and d.MAIN_CD = 'OVERTIME_CTL_CD'  and d.IS_VALID='Y'  and d.SYS_CD='HB' ");
            sb.Append(" where 1=1 ");



            //查詢條件
            if (assess_year != "")
            {
                sb.Append(" and ASSESS_YEAR = @assess_year ");
                ht.Add("@assess_year", assess_year);
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

            if (dept_no != "")
            {
                sb.Append(" and DEPT_NO like @DEPT_NO ");
                ht.Add("@DEPT_NO", dept_no + "%");
            }
            /*
            if (dept_name != "")
            {
                sb.Append(" and DEPT_NAME like @DEPT_NAME ");
                ht.Add("@DEPT_NAME", dept_name + "%");
            }
            */
            if (level_cd != "")
            {
                sb.Append(" and LEVEL_CD like @LEVEL_CD ");
                ht.Add("@LEVEL_CD", level_cd + "%");
            }
            if (ws_cd != "-1")
            {
                sb.Append(" and WS_CD = @WS_CD ");
                ht.Add("@WS_CD", ws_cd);
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
            , string assess_year, string emp_id, string emp_name, string dept_no, string level_cd, string ws_cd)
    {
        try
        {
            int result = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) resultCount ");
            sb.Append(" from TB_S_R_ASSESS_REVIEW ");
            sb.Append(" where 1=1 ");


            //查詢條件
            if (assess_year != "")
            {
                sb.Append(" and ASSESS_YEAR = @assess_year ");
                ht.Add("@assess_year", assess_year);
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
            if (dept_no != "")
            {
                sb.Append(" and DEPT_NO like @DEPT_NO ");
                ht.Add("@DEPT_NO", dept_no + "%");
            }
            /*
            if (dept_name != "")
            {
                sb.Append(" and DEPT_NAME like @DEPT_NAME ");
                ht.Add("@DEPT_NAME", dept_name + "%");
            }
            */
            if (level_cd != "")
            {
                sb.Append(" and LEVEL_CD like @LEVEL_CD ");
                ht.Add("@LEVEL_CD", level_cd + "%");
            }
            if (ws_cd != "-1")
            {
                sb.Append(" and WS_CD = @WS_CD ");
                ht.Add("@WS_CD", ws_cd);
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






}