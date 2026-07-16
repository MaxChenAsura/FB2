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
public class CFB2SG0400DAO : BaseDAO
{

    //節金維護檔 基本欄位
    public string FESTIVAL_TYPE { get; set; }
    public string FESTIVAL_DT { get; set; }
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
    public string WORK_DAYS { get; set; }

    public string EMP_CHG_CD { get; set; }
    public string WS_CD { get; set; }
    public string SEX_CD { get; set; }
    public string LEVEL_PAY { get; set; }
    public string ABILITY_PAY { get; set; }
    public string PJOB_PAY { get; set; }
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

    //查詢明細表頭欄位
    public string FESTIVAL_TYPE_DESC { get; set; }
    public string APPROVE_STATUS_DESC { get; set; }


    public CFB2SG0400DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    #region Gridview 資料(Qry)
    //Gridview 查詢資料
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression
                            , string festival_type, string festival_dt_s, string festival_dt_e
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
            sb.Append(" FESTIVAL_TYPE, FESTIVAL_DT, FESTIVAL_PAY_DT, RELEASE_DT, APPROVE_DT  ");
            sb.Append(" , SALARY_TRANS_DT, APPROVE_STATUS, FREEZE_FLAG, RELEASE_BY, APPROVE_BY   ");
            sb.Append(" , FESTIVAL_TYPE + '-' + b.SUB_DESC FESTIVAL_TYPE_DESC   ");
            sb.Append(" , APPROVE_STATUS + '-' + c.SUB_DESC APPROVE_STATUS_DESC   ");
            sb.Append(" ,  isnull(d.EMP_NAME,'')   RELEASE_BY_NAME   ");
            sb.Append(" ,  isnull(e.EMP_NAME,'')   APPROVE_BY_NAME   ");
            sb.Append(" from TB_S_M_FESTIVAL_H a ");
            sb.Append("  left join TB_9_M_COMM_D b on  a.FESTIVAL_TYPE = b.SUB_CD and b.MAIN_CD = 'FESTIVAL_TYPE' and b.IS_VALID='Y'  and b.SYS_CD='SG' ");
            sb.Append("  left join TB_9_M_COMM_D c on  a.APPROVE_STATUS = c.SUB_CD and c.MAIN_CD = 'APPROVE_STATUS' and c.IS_VALID='Y' and c.SYS_CD='SA' ");
            sb.Append("  left join  VW_H_EMP_DATA d on a.RELEASE_BY = d.EMP_ID ");
            sb.Append("  left join  VW_H_EMP_DATA e on a.APPROVE_BY = e.EMP_ID ");
            sb.Append(" where 1=1 ");


            //查詢條件-dropDownList
            if (festival_type != "-1")
            {
                sb.Append(" and FESTIVAL_TYPE = @FESTIVAL_TYPE ");
                ht.Add("@FESTIVAL_TYPE", festival_type);
            }
            //查詢條件-日期
            if (festival_dt_s != "")
            {
                sb.Append(" and   @FESTIVAL_DT_S  <= FESTIVAL_DT");
                ht.Add("@FESTIVAL_DT_S", Convert.ToDateTime(festival_dt_s).ToString("yyyy/MM/dd"));
            }
            if (festival_dt_e != "")
            {
                sb.Append(" and  FESTIVAL_DT <= @FESTIVAL_DT_E ");
                ht.Add("@FESTIVAL_DT_E", Convert.ToDateTime(festival_dt_e).ToString("yyyy/MM/dd"));
            }

            sb.Append(" group by  FESTIVAL_TYPE, FESTIVAL_DT, FESTIVAL_PAY_DT, RELEASE_DT, APPROVE_DT,SALARY_TRANS_DT, APPROVE_STATUS, FREEZE_FLAG, RELEASE_BY, APPROVE_BY, b.SUB_DESC,c.SUB_DESC, d.EMP_NAME, e.EMP_NAME ");

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
                         , string festival_type, string festival_dt_s, string festival_dt_e
        )
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select COUNT(*) resultCount ");
            sb.Append(" from (  ");
            sb.Append(" select   ");
            sb.Append(" FESTIVAL_TYPE, FESTIVAL_DT, FESTIVAL_PAY_DT, RELEASE_DT, APPROVE_DT  ");
            sb.Append(" , SALARY_TRANS_DT, APPROVE_STATUS, FREEZE_FLAG, RELEASE_BY, APPROVE_BY   ");
            sb.Append(" from TB_S_M_FESTIVAL_H a ");

            sb.Append(" where 1=1 ");

            if (festival_type != "-1")
            {
                sb.Append(" and FESTIVAL_TYPE = @FESTIVAL_TYPE ");
                ht.Add("@FESTIVAL_TYPE", festival_type);
            }
            //查詢條件-日期
            if (festival_dt_s != "")
            {
                sb.Append(" and   @FESTIVAL_DT_S  <= FESTIVAL_DT");
                ht.Add("@FESTIVAL_DT_S", Convert.ToDateTime(festival_dt_s).ToString("yyyy/MM/dd"));
            }
            if (festival_dt_e != "")
            {
                sb.Append(" and  FESTIVAL_DT <= @FESTIVAL_DT_E ");
                ht.Add("@FESTIVAL_DT_E", Convert.ToDateTime(festival_dt_e).ToString("yyyy/MM/dd"));
            }
            sb.Append(" group by  FESTIVAL_TYPE, FESTIVAL_DT, FESTIVAL_PAY_DT, RELEASE_DT, APPROVE_DT,SALARY_TRANS_DT, APPROVE_STATUS, FREEZE_FLAG, RELEASE_BY, APPROVE_BY ");
            sb.Append(" ) as z");

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

    #region GridView資料取得(Dtl)
    //Gridview 查詢資料(Dtl)
    public DataTable getDataDtl(int startRowIndex, int maximumRows, string sortExpression
                           , string festival_type, string festival_dt, string festival_pay_dt, string release_dt
                           , string emp_id, string emp_name
                           // , string level_cd, string pjob_cd, string emp_cd, string pay_type, string emp_chg_cd
                           )
    {
        try
        {

            if (sortExpression.Contains("UPDATED_DT"))
                sortExpression = sortExpression.Replace("UPDATED_DT", "a.UPDATED_DT");
            if (sortExpression.Contains("FESTIVAL_TYPE"))
                sortExpression = sortExpression.Replace("FESTIVAL_TYPE", "a.FESTIVAL_TYPE");
            if (sortExpression.Contains("FESTIVAL_DT"))
                sortExpression = sortExpression.Replace("FESTIVAL_DT", "a.FESTIVAL_DT");
            if (sortExpression.Contains("EMP_CD"))
                sortExpression = sortExpression.Replace("EMP_CD", "a.EMP_CD");
            if (sortExpression.Contains("FESTIVAL_PAY_DT"))
                sortExpression = sortExpression.Replace("FESTIVAL_PAY_DT", "a.FESTIVAL_PAY_DT");
            if (sortExpression.Contains("EMP_ID"))
                sortExpression = sortExpression.Replace("EMP_ID", "a.EMP_ID");
            if (sortExpression.Contains("EMP_NAME"))
                sortExpression = sortExpression.Replace("EMP_NAME", "a.EMP_NAME");

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + ") As RowNumber,");
            sb.Append(" a.FESTIVAL_TYPE, a.FESTIVAL_DT, a.FESTIVAL_PAY_DT, a.EMP_ID, a.EMP_CD");  //pk值
            sb.Append(" , a.EMP_NAME, a.LEVEL_CD, a.JOIN_DT, a.UPDATED_DT ");
            sb.Append(" , a.WORK_DAYS, a.FESTIVAL_AMT, a.PAY_TYPE, a.EMP_CHG_CD, a.PJOB_CD ");
            sb.Append(" , a.APPROVE_MARK, a.CHG_STATUS, a.APPROVE_FLAG ");
            sb.Append(" , case a.Festival_AMT_OLD when 0  then '' end as Festival_AMT_OLD ");
            //sb.Append(" , a.Festival_AMT_OLD  ");
            sb.Append(" , a.CHG_STATUS + '-' + b.SUB_DESC CHG_STATUS_DESC   ");
            sb.Append(" , a.EMP_CD + '-' + c.SUB_DESC EMP_CD_DESC   ");
            sb.Append(" , a.EMP_CHG_CD + '-' + d.SUB_DESC EMP_CHG_CD_DESC   ");
            sb.Append(" , a.PAY_TYPE + '-' + e.SUB_DESC PAY_TYPE_DESC   ");
            sb.Append(" , isnull(a.PAY_TYPE_OLD,'') + '-' + isnull(g.SUB_DESC,'')  PAY_TYPE_OLD_DESC   ");
            sb.Append(" from TB_S_M_FESTIVAL_D a ");
            sb.Append("  left join TB_9_M_COMM_D b on  a.CHG_STATUS = b.SUB_CD and b.MAIN_CD = 'CHG_STATUS'  and b.SYS_CD='SA' and b.IS_VALID='Y' ");
            sb.Append("  left join TB_9_M_COMM_D c on  a.EMP_CD = c.SUB_CD and c.MAIN_CD = 'EMP_CD'  and c.SYS_CD='HB' and c.IS_VALID='Y' ");
            sb.Append("  left join TB_9_M_COMM_D d on  a.EMP_CHG_CD = d.SUB_CD and d.MAIN_CD = 'EMP_CHG_CD'  and d.SYS_CD='HB' and d.IS_VALID='Y' ");
            sb.Append("  left join TB_9_M_COMM_D e on  a.PAY_TYPE = e.SUB_CD and e.MAIN_CD = 'PAY_TYPE'  and e.SYS_CD='SC' and e.IS_VALID='Y' ");
            sb.Append("  left join TB_9_M_COMM_D g on  a.PAY_TYPE_OLD = g.SUB_CD and g.MAIN_CD = 'PAY_TYPE'  and e.SYS_CD='SC' and g.IS_VALID='Y' ");
            sb.Append("  left join TB_S_M_FESTIVAL_H f  on a.FESTIVAL_TYPE = f.FESTIVAL_TYPE  and a.FESTIVAL_DT = f.FESTIVAL_DT and a.FESTIVAL_PAY_DT = f.FESTIVAL_PAY_DT and a.EMP_CD = f.EMP_CD ");
            sb.Append(" where 1=1 ");

            if (festival_type != "")
            {
                sb.Append(" and a.FESTIVAL_TYPE = @FESTIVAL_TYPE ");
                ht.Add("@FESTIVAL_TYPE", festival_type);
            }
            if (festival_dt != "")
            {
                sb.Append(" and a.FESTIVAL_DT = @FESTIVAL_DT ");
                ht.Add("@FESTIVAL_DT", Convert.ToDateTime(festival_dt).ToString("yyyy/MM/dd"));
            }
            if (festival_pay_dt != "")
            {
                sb.Append(" and a.FESTIVAL_PAY_DT = @FESTIVAL_PAY_DT ");
                ht.Add("@FESTIVAL_PAY_DT", Convert.ToDateTime(festival_pay_dt).ToString("yyyy/MM/dd"));
            }

            //需加此段以免相同的節金日期，不同的提出核可日時
            if (release_dt != "")
            {
                sb.Append(" and f.RELEASE_DT = @RELEASE_DT ");
                ht.Add("@RELEASE_DT", Convert.ToDateTime(release_dt).ToString("yyyy/MM/dd"));
            }
            if (emp_id != "")
            {
                sb.Append(" and a.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
            }
            if (emp_name != "")
            {
                sb.Append(" and a.EMP_NAME = @EMP_NAME ");
                ht.Add("@EMP_NAME", emp_name);
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
                            , string festival_type, string festival_dt, string festival_pay_dt, string release_dt
                            , string emp_id, string emp_name
                           // , string level_cd, string pjob_cd, string emp_cd, string pay_type, string emp_chg_cd
                        )
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) resultCount  ");
            sb.Append(" from TB_S_M_FESTIVAL_D a ");
            sb.Append(" left join TB_S_M_FESTIVAL_H f  on a.FESTIVAL_TYPE = f.FESTIVAL_TYPE  and a.FESTIVAL_DT = f.FESTIVAL_DT and a.FESTIVAL_PAY_DT = f.FESTIVAL_PAY_DT and a.EMP_CD = f.EMP_CD ");
            sb.Append(" where 1=1 ");

            if (festival_type != "")
            {
                sb.Append(" and a.FESTIVAL_TYPE = @FESTIVAL_TYPE ");
                ht.Add("@FESTIVAL_TYPE", festival_type);
            }
            if (festival_dt != "")
            {
                sb.Append(" and a.FESTIVAL_DT = @FESTIVAL_DT ");
                ht.Add("@FESTIVAL_DT", Convert.ToDateTime(festival_dt).ToString("yyyy/MM/dd"));
            }
            if (festival_pay_dt != "")
            {
                sb.Append(" and a.FESTIVAL_PAY_DT = @FESTIVAL_PAY_DT ");
                ht.Add("@FESTIVAL_PAY_DT", Convert.ToDateTime(festival_pay_dt).ToString("yyyy/MM/dd"));
            }
            //需加此段以免相同的節金日期，不同的提出核可日時
            if (release_dt != "")
            {
                sb.Append(" and f.RELEASE_DT = @RELEASE_DT ");
                ht.Add("@RELEASE_DT", Convert.ToDateTime(release_dt).ToString("yyyy/MM/dd"));
            }
            if (emp_id != "")
            {
                sb.Append(" and a.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
            }
            if (emp_name != "")
            {
                sb.Append(" and a.EMP_NAME = @EMP_NAME ");
                ht.Add("@EMP_NAME", emp_name);
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

    #region GridView2資料取得(Dtl)
    //Gridview 查詢資料(Dtl)
    public DataTable getFestivalCond(string sortExpression
        // , string emp_id, string emp_name, string level_cd, string pjob_cd, string emp_cd, string pay_type, string emp_chg_cd
                           )
    {
        try
        {

            //if (sortExpression.Contains("UPDATED_DT"))
            //    sortExpression = sortExpression.Replace("UPDATED_DT", "a.UPDATED_DT");

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + ") As RowNumber,");
            sb.Append(" FESTIVAL_TYPE + '-' + b.SUB_DESC FESTIVAL_TYPE_DESC   ");
            sb.Append(" , FESTIVAL_TYPE, FESTIVAL_PAY_COND, FESTIVAL_AMT, WORK_YEARS_SDT, WORK_YEARS_EDT, PRID_CD   ");
            sb.Append(" from TB_S_M_FESTIVAL_COND a ");
            sb.Append("  left join TB_9_M_COMM_D b on  a.FESTIVAL_TYPE = b.SUB_CD and b.MAIN_CD = 'FESTIVAL_TYPE' and b.IS_VALID='Y' ");
            sb.Append(" where 1=1 ");


            if (FESTIVAL_TYPE != "")
            {
                sb.Append(" and a.FESTIVAL_TYPE = @FESTIVAL_TYPE ");
                ht.Add("@FESTIVAL_TYPE", FESTIVAL_TYPE);
            }
            sb.Append(" ) as z");
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    public DataTable getDataDtl2(int startRowIndex, int maximumRows, string SortExpression, string festival_type)
    {
        try
        {

            //if (sortExpression.Contains("UPDATED_DT"))
            //    sortExpression = sortExpression.Replace("UPDATED_DT", "a.UPDATED_DT");

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + SortExpression + ") As RowNumber,");
            sb.Append(" FESTIVAL_TYPE + '-' + b.SUB_DESC FESTIVAL_TYPE_DESC   ");
            sb.Append(" , FESTIVAL_TYPE, FESTIVAL_PAY_COND, FESTIVAL_AMT, WORK_YEARS_SDT, WORK_YEARS_EDT, PRID_CD   ");
            sb.Append(" from TB_S_M_FESTIVAL_COND a ");
            sb.Append("  left join TB_9_M_COMM_D b on  a.FESTIVAL_TYPE = b.SUB_CD and b.MAIN_CD = 'FESTIVAL_TYPE' and b.IS_VALID='Y' ");
            sb.Append(" where 1=1 ");

            if (festival_type != "")
            {
                sb.Append(" and a.FESTIVAL_TYPE = @FESTIVAL_TYPE ");
                ht.Add("@FESTIVAL_TYPE", festival_type);
            }

            //sb.Append(" ) as z");

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
    public int getCountDtl2(int startRowIndex, int maximumRows, string festival_type
                        )
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) resultCount  ");
            sb.Append(" from TB_S_M_FESTIVAL_COND a ");
            sb.Append("  left join TB_9_M_COMM_D b on  a.FESTIVAL_TYPE = b.SUB_CD and b.MAIN_CD = 'FESTIVAL_TYPE' and b.IS_VALID='Y' ");
            sb.Append(" where 1=1 ");
            if (festival_type != "")
            {
                sb.Append(" and a.FESTIVAL_TYPE = @FESTIVAL_TYPE ");
                ht.Add("@FESTIVAL_TYPE", festival_type);
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

    #region 資料取得(Dtl)

    //判斷登入者是否為提出核可者的直屬長官
    public DataTable isDirectHeadEmp()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select count(a.RELEASE_BY) resultCount ");
            sb.Append(" from TB_S_M_FESTIVAL_H a ");
            sb.Append(" left join VW_H_EMP_DATA b on a.RELEASE_BY = b.EMP_ID ");
            sb.Append(" left join TB_H_R_HEAD_DEPT c on b.DEPT_NO=c.MNG_DEPT_NO ");
            sb.Append(" where 1=1 ");
            sb.Append(" and a.FESTIVAL_TYPE = @FESTIVAL_TYPE  ");
            sb.Append(" and a.FESTIVAL_DT = @FESTIVAL_DT  ");
            sb.Append(" and a.FESTIVAL_PAY_DT = @FESTIVAL_PAY_DT  ");
            sb.Append(" and c.EMP_ID = @EMP_ID  ");

            ht.Add("@FESTIVAL_TYPE", FESTIVAL_TYPE);
            ht.Add("@FESTIVAL_DT", Convert.ToDateTime(FESTIVAL_DT));
            ht.Add("@FESTIVAL_PAY_DT", Convert.ToDateTime(FESTIVAL_PAY_DT));
            ht.Add("@EMP_ID", SessionHandle.Current.emp_id);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //依PK值取得資料(EXCEL下載用)
    public DataTable getMaintainData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" select a.* ");
            sb.Append(" , CHG_STATUS + '-' + b.SUB_DESC CHG_STATUS_DESC    ");
            sb.Append(" , a.EMP_CD + '-' + c.SUB_DESC EMP_CD_DESC     ");
            sb.Append(" , EMP_CHG_CD + '-' + d.SUB_DESC EMP_CHG_CD_DESC  ");
            sb.Append(" , PAY_TYPE + '-' + e.SUB_DESC PAY_TYPE_DESC    ");
            sb.Append(" , isnull(PAY_TYPE_OLD,'') + '-' + isnull(f.SUB_DESC,'')  PAY_TYPE_OLD_DESC    ");
            sb.Append(" , PLANT_CD + '-' + g.SUB_DESC PLANT_CD_DESC    ");
            sb.Append(" , a.FESTIVAL_TYPE + '-' + h.SUB_DESC FESTIVAL_TYPE_DESC    ");
            sb.Append(" , isnull(a.JPN_CD,'') + '-' + isnull(j.SUB_DESC,'')  JPN_CD_DESC    ");
            sb.Append(" , i.FESTIVAL_DESC   ");
            sb.Append(" from TB_S_M_FESTIVAL_D a ");
            sb.Append("  left join TB_9_M_COMM_D b on  a.CHG_STATUS = b.SUB_CD and b.MAIN_CD = 'CHG_STATUS'  and b.SYS_CD='SA' and b.IS_VALID='Y' ");
            sb.Append("  left join TB_9_M_COMM_D c on  a.EMP_CD = c.SUB_CD and c.MAIN_CD = 'EMP_CD'  and c.SYS_CD='HB' and c.IS_VALID='Y' ");
            sb.Append("  left join TB_9_M_COMM_D d on  a.EMP_CHG_CD = d.SUB_CD and d.MAIN_CD = 'EMP_CHG_CD'  and d.SYS_CD='HB' and d.IS_VALID='Y' ");
            sb.Append("  left join TB_9_M_COMM_D e on  a.PAY_TYPE = e.SUB_CD and e.MAIN_CD = 'PAY_TYPE'  and e.SYS_CD='SC'  and e.IS_VALID='Y' ");
            sb.Append("  left join TB_9_M_COMM_D f on  a.PAY_TYPE_OLD = f.SUB_CD and f.MAIN_CD = 'PAY_TYPE'  and f.SYS_CD='SC' and f.IS_VALID='Y' ");
            sb.Append("  left join TB_9_M_COMM_D g on  a.PLANT_CD = g.SUB_CD and g.MAIN_CD = 'PLANT_CD'  and g.SYS_CD='HB' and g.IS_VALID='Y' ");
            sb.Append("  left join TB_9_M_COMM_D h on  a.FESTIVAL_TYPE = h.SUB_CD and h.MAIN_CD = 'FESTIVAL_TYPE'  and h.SYS_CD='SG' and h.IS_VALID='Y' ");
            sb.Append("  left join TB_9_M_COMM_D j on  a.JPN_CD = j.SUB_CD and j.MAIN_CD = 'JPN_CD'  and j.SYS_CD='HB' and j.IS_VALID='Y' ");
            sb.Append("  left  join TB_S_M_FESTIVAL_H i on a.FESTIVAL_TYPE =  i.FESTIVAL_TYPE and a.EMP_CD = i.EMP_CD and a.FESTIVAL_DT = i.FESTIVAL_DT and a.FESTIVAL_PAY_DT = i.FESTIVAL_PAY_DT ");
            sb.Append(" where 1=1 ");

            if (FESTIVAL_TYPE != "")
            {
                sb.Append(" and a.FESTIVAL_TYPE = @FESTIVAL_TYPE ");
                ht.Add("@FESTIVAL_TYPE", FESTIVAL_TYPE);
            }
            if (FESTIVAL_DT != "")
            {
                sb.Append(" and a.FESTIVAL_DT = @FESTIVAL_DT ");
                ht.Add("@FESTIVAL_DT", Convert.ToDateTime(FESTIVAL_DT).ToString("yyyy/MM/dd"));
            }
            if (FESTIVAL_PAY_DT != "")
            {
                sb.Append(" and a.FESTIVAL_PAY_DT = @FESTIVAL_PAY_DT ");
                ht.Add("@FESTIVAL_PAY_DT", Convert.ToDateTime(FESTIVAL_PAY_DT).ToString("yyyy/MM/dd"));
            }
            sb.Append(" order by EMP_ID ASC ");

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

            sb.Append(" select  FESTIVAL_TYPE, FESTIVAL_DT, FESTIVAL_PAY_DT, SALARY_TRANS_DT, APPROVE_STATUS, a.REMARK, FREEZE_FLAG, TARGET_GEN_DT   ");
            sb.Append(" , FESTIVAL_TYPE + '-' + b.SUB_DESC FESTIVAL_TYPE_DESC   ");
            sb.Append(" , APPROVE_STATUS + '-' + c.SUB_DESC APPROVE_STATUS_DESC   ");
            sb.Append(" from TB_S_M_FESTIVAL_H a ");
            sb.Append("  left join TB_9_M_COMM_D b on  a.FESTIVAL_TYPE = b.SUB_CD and b.MAIN_CD = 'FESTIVAL_TYPE'  and b.SYS_CD='SG' and b.IS_VALID='Y' ");
            sb.Append("  left join TB_9_M_COMM_D c on  a.APPROVE_STATUS = c.SUB_CD and c.MAIN_CD = 'APPROVE_STATUS'  and c.SYS_CD='SA' and c.IS_VALID='Y' ");
            sb.Append(" where 1=1 ");
            sb.Append(" and FESTIVAL_TYPE = @FESTIVAL_TYPE ");
            ht.Add("@FESTIVAL_TYPE", FESTIVAL_TYPE);

            if (FESTIVAL_DT != "")
            {
                sb.Append(" and FESTIVAL_DT  = @FESTIVAL_DT");
                ht.Add("@FESTIVAL_DT", Convert.ToDateTime(FESTIVAL_DT).ToString("yyyy/MM/dd"));
            }

            if (FESTIVAL_PAY_DT != "")
            {
                sb.Append(" and FESTIVAL_PAY_DT  = @FESTIVAL_PAY_DT");
                ht.Add("@FESTIVAL_PAY_DT", Convert.ToDateTime(FESTIVAL_PAY_DT).ToString("yyyy/MM/dd"));
            }

            if (RELEASE_DT != "")
            {
                sb.Append(" and RELEASE_DT  = @RELEASE_DT");
                ht.Add("@RELEASE_DT", Convert.ToDateTime(RELEASE_DT).ToString("yyyy/MM/dd"));
            }

            DataTable dt = dbConn.Query(sb, ht);
            foreach (DataRow dr in dt.Rows)
            {
                this.FESTIVAL_TYPE = Convert.ToString(dr["FESTIVAL_TYPE"]);
                this.FESTIVAL_DT = dr["FESTIVAL_DT"].ToString() != "" ? Convert.ToDateTime(dr["FESTIVAL_DT"].ToString()).ToString("yyyy/MM/dd") : "";
                this.FESTIVAL_PAY_DT = dr["FESTIVAL_PAY_DT"].ToString() != "" ? Convert.ToDateTime(dr["FESTIVAL_PAY_DT"].ToString()).ToString("yyyy/MM/dd") : "";
                this.SALARY_TRANS_DT = dr["SALARY_TRANS_DT"].ToString() != "" ? Convert.ToDateTime(dr["SALARY_TRANS_DT"].ToString()).ToString("yyyy/MM/dd") : "";
                this.APPROVE_STATUS = Convert.ToString(dr["APPROVE_STATUS"]);
                this.REMARK = Convert.ToString(dr["REMARK"]);
                this.FREEZE_FLAG = Convert.ToString(dr["FREEZE_FLAG"]);
                this.FESTIVAL_TYPE_DESC = Convert.ToString(dr["FESTIVAL_TYPE_DESC"]);
                this.APPROVE_STATUS_DESC = Convert.ToString(dr["APPROVE_STATUS_DESC"]);
                //this.TARGET_GEN_DT = Convert.ToString(dr["RELEASE_DT"]);
            }


            sb = new StringBuilder();
            ht = new Hashtable();
            sb.Append(" select sum(FESTIVAL_TOTAL_AMT) amt, sum(FESTIVAL_TOTAL_NUM) num   ");
            sb.Append(" from TB_S_M_FESTIVAL_H a ");
            sb.Append(" where 1=1 ");
            sb.Append(" and FESTIVAL_TYPE = @FESTIVAL_TYPE ");
            ht.Add("@FESTIVAL_TYPE", FESTIVAL_TYPE);

            if (FESTIVAL_DT != "")
            {
                sb.Append(" and FESTIVAL_DT  = @FESTIVAL_DT");
                ht.Add("@FESTIVAL_DT", Convert.ToDateTime(FESTIVAL_DT).ToString("yyyy/MM/dd"));
            }

            if (FESTIVAL_PAY_DT != "")
            {
                sb.Append(" and FESTIVAL_PAY_DT  = @FESTIVAL_PAY_DT");
                ht.Add("@FESTIVAL_PAY_DT", Convert.ToDateTime(FESTIVAL_PAY_DT).ToString("yyyy/MM/dd"));
            }
            if (RELEASE_DT != "")
            {
                sb.Append(" and RELEASE_DT  = @RELEASE_DT");
                ht.Add("@RELEASE_DT", Convert.ToDateTime(RELEASE_DT).ToString("yyyy/MM/dd"));
            }

            dt = dbConn.Query(sb, ht);
            foreach (DataRow dr in dt.Rows)
            {
                this.FESTIVAL_TOTAL_AMT = Convert.ToString(dr["amt"]);
                this.FESTIVAL_TOTAL_NUM = Convert.ToString(dr["num"]);
            }

        }
        catch
        {
            throw;
        }

    }
    #endregion



    #region 駁回DB存取(Dtl)

    //駁回 更新-節金明細維護檔
    public void updateRejectData_D(DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_S_M_FESTIVAL_D ");
            sb.Append(" set APPROVE_MARK = @APPROVE_MARK ");
            sb.Append(" ,APPROVE_FLAG = @APPROVE_FLAG");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT = @UPDATED_DT");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where FESTIVAL_TYPE = @FESTIVAL_TYPE ");
            sb.Append("  and FESTIVAL_DT = @FESTIVAL_DT");
            sb.Append("  and FESTIVAL_PAY_DT = @FESTIVAL_PAY_DT");
            sb.Append("  and EMP_ID = @EMP_ID");
            ht.Add("@APPROVE_MARK", "V");
            ht.Add("@APPROVE_FLAG", "N");

            //PK值
            ht.Add("@FESTIVAL_TYPE", FESTIVAL_TYPE);
            ht.Add("@FESTIVAL_DT", Convert.ToDateTime(FESTIVAL_DT).ToString("yyyy/MM/dd"));
            ht.Add("@FESTIVAL_PAY_DT", Convert.ToDateTime(FESTIVAL_PAY_DT).ToString("yyyy/MM/dd"));
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
            sb.Append(" update TB_S_M_FESTIVAL_D ");
            sb.Append(" set APPROVE_MARK = @APPROVE_MARK ");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT = @UPDATED_DT");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where FESTIVAL_TYPE = @FESTIVAL_TYPE ");
            sb.Append("  and FESTIVAL_DT = @FESTIVAL_DT");
            sb.Append("  and FESTIVAL_PAY_DT = @FESTIVAL_PAY_DT");
            ht.Add("@APPROVE_MARK", "");

            //PK值
            ht.Add("@FESTIVAL_TYPE", FESTIVAL_TYPE);
            ht.Add("@FESTIVAL_DT", Convert.ToDateTime(FESTIVAL_DT).ToString("yyyy/MM/dd"));
            ht.Add("@FESTIVAL_PAY_DT", Convert.ToDateTime(FESTIVAL_PAY_DT).ToString("yyyy/MM/dd"));


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

    //駁回-回復成未核可前狀態  (節金維護檔 DTL)
    public void updateRejectData_H(DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_S_M_FESTIVAL_H ");
            sb.Append(" set REMARK = @REMARK ");
            sb.Append(" ,RELEASE_DT = @RELEASE_DT_set");
            sb.Append(" ,RELEASE_BY = @RELEASE_BY");
            sb.Append(" ,APPROVE_DT = @APPROVE_DT");
            sb.Append(" ,APPROVE_BY = @APPROVE_BY");
            sb.Append(" ,APPROVE_STATUS = @APPROVE_STATUS");
            sb.Append(" ,FREEZE_FLAG = @FREEZE_FLAG");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT = @UPDATED_DT");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where FESTIVAL_TYPE = @FESTIVAL_TYPE ");
            sb.Append("  and FESTIVAL_DT = @FESTIVAL_DT");
            sb.Append("  and FESTIVAL_PAY_DT = @FESTIVAL_PAY_DT");
            sb.Append("  and RELEASE_DT = @RELEASE_DT_qry");
            //set值
            ht.Add("@REMARK", REMARK);
            ht.Add("@RELEASE_DT_set", DBNull.Value);
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
            ht.Add("@FESTIVAL_TYPE", FESTIVAL_TYPE);
            ht.Add("@FESTIVAL_DT", Convert.ToDateTime(FESTIVAL_DT).ToString("yyyy/MM/dd"));
            ht.Add("@FESTIVAL_PAY_DT", Convert.ToDateTime(FESTIVAL_PAY_DT).ToString("yyyy/MM/dd"));
            ht.Add("@RELEASE_DT_qry", Convert.ToDateTime(RELEASE_DT).ToString("yyyy/MM/dd"));

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    #endregion

    #region 核可DB存取(Dtl)
    //核可-回復成核可狀態  (節金維護檔 DTL)
    public void updateApproveData_H(DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_S_M_FESTIVAL_H ");
            sb.Append(" set REMARK = @REMARK ");
            sb.Append(" ,APPROVE_DT = @APPROVE_DT");
            sb.Append(" ,APPROVE_BY = @APPROVE_BY");
            sb.Append(" ,APPROVE_STATUS = @APPROVE_STATUS");

            sb.Append(" ,FREEZE_FLAG = @FREEZE_FLAG");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT = @UPDATED_DT");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where FESTIVAL_TYPE = @FESTIVAL_TYPE ");
            sb.Append("  and FESTIVAL_DT = @FESTIVAL_DT");
            sb.Append("  and FESTIVAL_PAY_DT = @FESTIVAL_PAY_DT");
            sb.Append("  and RELEASE_DT = @RELEASE_DT_qry");

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
            ht.Add("@FESTIVAL_TYPE", FESTIVAL_TYPE);
            ht.Add("@FESTIVAL_DT", Convert.ToDateTime(FESTIVAL_DT).ToString("yyyy/MM/dd"));
            ht.Add("@FESTIVAL_PAY_DT", Convert.ToDateTime(FESTIVAL_PAY_DT).ToString("yyyy/MM/dd"));
            ht.Add("@RELEASE_DT_qry", Convert.ToDateTime(RELEASE_DT).ToString("yyyy/MM/dd"));

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }


    //核可 更新-節金明細維護檔
    public void updateAllApproveData_D(DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_S_M_FESTIVAL_D ");
            sb.Append(" set APPROVE_MARK = @APPROVE_MARK ");
            sb.Append(" ,APPROVE_FLAG = @APPROVE_FLAG");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT = @UPDATED_DT");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where FESTIVAL_TYPE = @FESTIVAL_TYPE ");
            sb.Append("  and FESTIVAL_DT = @FESTIVAL_DT");
            sb.Append("  and FESTIVAL_PAY_DT = @FESTIVAL_PAY_DT");
            ht.Add("@APPROVE_MARK", "");
            ht.Add("@APPROVE_FLAG", "Y");

            //PK值
            ht.Add("@FESTIVAL_TYPE", FESTIVAL_TYPE);
            ht.Add("@FESTIVAL_DT", Convert.ToDateTime(FESTIVAL_DT).ToString("yyyy/MM/dd"));
            ht.Add("@FESTIVAL_PAY_DT", Convert.ToDateTime(FESTIVAL_PAY_DT).ToString("yyyy/MM/dd"));

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

    //核可 更新-節金明細維護檔
    public void deleteStatusData_D()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" delete from  TB_S_M_FESTIVAL_D ");
            sb.Append(" where FESTIVAL_TYPE = @FESTIVAL_TYPE ");
            sb.Append("  and FESTIVAL_DT = @FESTIVAL_DT");
            sb.Append("  and FESTIVAL_PAY_DT = @FESTIVAL_PAY_DT");
            sb.Append("   and CHG_STATUS = @CHG_STATUS");
            ht.Add("@CHG_STATUS", "D");
            //PK值
            ht.Add("@FESTIVAL_TYPE", FESTIVAL_TYPE);
            ht.Add("@FESTIVAL_DT", Convert.ToDateTime(FESTIVAL_DT).ToString("yyyy/MM/dd"));
            ht.Add("@FESTIVAL_PAY_DT", Convert.ToDateTime(FESTIVAL_PAY_DT).ToString("yyyy/MM/dd"));
           
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //核可,刪除-節金明細主檔
    public void deleteApproveData_D_H(DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" delete from TB_S_R_FESTIVAL_D ");
            sb.Append(" where FESTIVAL_TYPE = @FESTIVAL_TYPE ");
            sb.Append("  and FESTIVAL_DT = @FESTIVAL_DT");
            sb.Append("  and FESTIVAL_PAY_DT = @FESTIVAL_PAY_DT");

            //PK值
            ht.Add("@FESTIVAL_TYPE", FESTIVAL_TYPE);
            ht.Add("@FESTIVAL_DT", Convert.ToDateTime(FESTIVAL_DT).ToString("yyyy/MM/dd"));
            ht.Add("@FESTIVAL_PAY_DT", Convert.ToDateTime(FESTIVAL_PAY_DT).ToString("yyyy/MM/dd"));

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //核可, 新增-節金明細主檔
    public void insertApproveData_D_H(DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" insert into TB_S_R_FESTIVAL_D ");
            sb.Append(@"(FESTIVAL_TYPE,FESTIVAL_DT,FESTIVAL_PAY_DT,EMP_ID,EMP_NAME
                        ,DEPT_NO,PLANT_CD,JPN_CD,COMPANY_CD,LEVEL_CD
                        ,GRADE_CD,PJOB_CD,JOIN_DT,WORK_YEARS,WORK_DAYS
                        ,EMP_CD,EMP_CHG_CD,WS_CD,SEX_CD,LEVEL_PAY
                        ,ABILITY_PAY,PJOB_PAY,PROFESSION_PAY,FOOD_SUBSIDY,FESTIVAL_AMT
                        ,FESTIVAL_AMT_OLD,FESTIVAL_TAX,FESTIVAL_AMT_R,PAY_TYPE,PAY_TYPE_OLD
                        ,APPROVE_FLAG,CHG_STATUS,APPROVE_MARK
                        ,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)
                        ");
            sb.Append(@"select  FESTIVAL_TYPE,FESTIVAL_DT,FESTIVAL_PAY_DT,EMP_ID,EMP_NAME
                       ,DEPT_NO,PLANT_CD,JPN_CD,COMPANY_CD,LEVEL_CD
                       ,GRADE_CD,PJOB_CD,JOIN_DT,WORK_YEARS,WORK_DAYS
                       ,EMP_CD,EMP_CHG_CD,WS_CD,SEX_CD,LEVEL_PAY
                       ,ABILITY_PAY,PJOB_PAY,PROFESSION_PAY,FOOD_SUBSIDY,FESTIVAL_AMT
                       ,FESTIVAL_AMT_OLD,FESTIVAL_TAX,FESTIVAL_AMT_R,PAY_TYPE,PAY_TYPE_OLD
                       ,APPROVE_FLAG,CHG_STATUS,APPROVE_MARK
                       ,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID
                       ");
            sb.Append(" from TB_S_M_FESTIVAL_D ");
            sb.Append(" where FESTIVAL_TYPE = @FESTIVAL_TYPE ");
            sb.Append("  and FESTIVAL_DT = @FESTIVAL_DT");
            sb.Append("  and FESTIVAL_PAY_DT = @FESTIVAL_PAY_DT");

            //PK值
            ht.Add("@FESTIVAL_TYPE", FESTIVAL_TYPE);
            ht.Add("@FESTIVAL_DT", Convert.ToDateTime(FESTIVAL_DT).ToString("yyyy/MM/dd"));
            ht.Add("@FESTIVAL_PAY_DT", Convert.ToDateTime(FESTIVAL_PAY_DT).ToString("yyyy/MM/dd"));

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //核可,刪除-節金條件設定歷史檔
    public void deleteApproveData_LOG(DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" delete from TB_S_M_FESTIVAL_COND_LOG ");
            sb.Append(" where FESTIVAL_TYPE = @FESTIVAL_TYPE ");
            sb.Append("  and FESTIVAL_YEAR = @FESTIVAL_YEAR");

            //PK值
            ht.Add("@FESTIVAL_TYPE", FESTIVAL_TYPE);
            ht.Add("@FESTIVAL_YEAR", DateTime.Now.ToString("yyyy"));

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //核可, 新增-節金條件設定歷史檔
    public void insertApproveData_LOG(DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" insert into TB_S_M_FESTIVAL_COND_LOG(  ");
            sb.Append(" FESTIVAL_YEAR ");
            sb.Append(" ,FESTIVAL_TYPE,FESTIVAL_PAY_COND,FESTIVAL_AMT,WORK_YEARS_SDT,WORK_YEARS_EDT,PRID_CD ");
            sb.Append(" ,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID  ");
            sb.Append(" ) ");
            sb.Append(" select  YEAR(GETDATE()) ");
            sb.Append(" , FESTIVAL_TYPE,FESTIVAL_PAY_COND,FESTIVAL_AMT,WORK_YEARS_SDT,WORK_YEARS_EDT,PRID_CD ");
            sb.Append(" ,@CREATED_BY, @CREATED_DT, @UPDATED_BY, @UPDATED_DT, @FUNC_ID  ");
            sb.Append("   from TB_S_M_FESTIVAL_COND ");
            sb.Append(" where FESTIVAL_TYPE = @FESTIVAL_TYPE ");

            //新修日期
            ht.Add("@CREATED_BY", UPDATED_BY);
            ht.Add("@CREATED_DT", now);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@UPDATED_DT", now);
            ht.Add("@FUNC_ID", FUNC_ID);

            //PK值
            ht.Add("@FESTIVAL_TYPE", FESTIVAL_TYPE);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion

    internal void updateFESTIVAL_H(DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_S_M_FESTIVAL_H ");
            sb.Append(" set REMARK = @REMARK ");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT = @UPDATED_DT");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where FESTIVAL_TYPE = @FESTIVAL_TYPE ");
            sb.Append("  and FESTIVAL_DT = @FESTIVAL_DT");
            sb.Append("  and FESTIVAL_PAY_DT = @FESTIVAL_PAY_DT");
            sb.Append("  and RELEASE_DT = @RELEASE_DT_qry");
            //set值
            ht.Add("@REMARK", REMARK);
            //新修日期
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@UPDATED_DT", now);
            ht.Add("@FUNC_ID", FUNC_ID);
            //PK值
            ht.Add("@FESTIVAL_TYPE", FESTIVAL_TYPE);
            ht.Add("@FESTIVAL_DT", Convert.ToDateTime(FESTIVAL_DT).ToString("yyyy/MM/dd"));
            ht.Add("@FESTIVAL_PAY_DT", Convert.ToDateTime(FESTIVAL_PAY_DT).ToString("yyyy/MM/dd"));
            ht.Add("@RELEASE_DT_qry", Convert.ToDateTime(RELEASE_DT).ToString("yyyy/MM/dd"));
           
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    internal void updateFESTIVAL_D(DateTime now, string approve_mark)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_S_M_FESTIVAL_D ");
            sb.Append(" set APPROVE_MARK = @APPROVE_MARK ");
            if (approve_mark != "")
            {
                sb.Append(" ,APPROVE_FLAG = @APPROVE_FLAG");
            }
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT = @UPDATED_DT");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where FESTIVAL_TYPE = @FESTIVAL_TYPE ");
            sb.Append("  and FESTIVAL_DT = @FESTIVAL_DT");
            sb.Append("  and FESTIVAL_PAY_DT = @FESTIVAL_PAY_DT");
            sb.Append("  and EMP_ID = @EMP_ID");
            ht.Add("@APPROVE_MARK", approve_mark);
            if (approve_mark != "")
            {
                ht.Add("@APPROVE_FLAG", "N");
            }

            //PK值
            ht.Add("@FESTIVAL_TYPE", FESTIVAL_TYPE);
            ht.Add("@FESTIVAL_DT", Convert.ToDateTime(FESTIVAL_DT).ToString("yyyy/MM/dd"));
            ht.Add("@FESTIVAL_PAY_DT", Convert.ToDateTime(FESTIVAL_PAY_DT).ToString("yyyy/MM/dd"));
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

    //取得異常註記的筆數
    public int getMarkData()
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) resultCount ");
            sb.Append(" from TB_S_M_FESTIVAL_D  ");
            sb.Append(" where FESTIVAL_TYPE = @FESTIVAL_TYPE ");
            sb.Append("  and FESTIVAL_DT = @FESTIVAL_DT");
            sb.Append("  and FESTIVAL_PAY_DT = @FESTIVAL_PAY_DT");
            sb.Append("  and APPROVE_MARK = @APPROVE_MARK");
            //PK值
            ht.Add("@FESTIVAL_TYPE", FESTIVAL_TYPE);
            ht.Add("@FESTIVAL_DT", Convert.ToDateTime(FESTIVAL_DT).ToString("yyyy/MM/dd"));
            ht.Add("@FESTIVAL_PAY_DT", Convert.ToDateTime(FESTIVAL_PAY_DT).ToString("yyyy/MM/dd"));

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



}
