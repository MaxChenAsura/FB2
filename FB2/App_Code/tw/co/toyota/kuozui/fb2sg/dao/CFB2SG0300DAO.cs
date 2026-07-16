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
public class CFB2SG0300DAO : BaseDAO
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

    public CFB2SG0300DAO()
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
            sb.Append(" FESTIVAL_TYPE,FESTIVAL_DT,FESTIVAL_PAY_DT, TARGET_GEN_DT, RELEASE_DT, APPROVE_DT, SALARY_TRANS_DT, APPROVE_STATUS, FREEZE_FLAG   ");
            sb.Append(" , FESTIVAL_TYPE + '-' + b.SUB_DESC FESTIVAL_TYPE_DESC   ");
            sb.Append(" from TB_S_M_FESTIVAL_H a ");
            sb.Append("  left join TB_9_M_COMM_D b on  a.FESTIVAL_TYPE = b.SUB_CD and b.MAIN_CD = 'FESTIVAL_TYPE' and b.IS_VALID='Y' and b.SYS_CD='SG' ");
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

            sb.Append(" group by FESTIVAL_TYPE, FESTIVAL_DT, FESTIVAL_PAY_DT, TARGET_GEN_DT, APPROVE_DT, RELEASE_DT, SALARY_TRANS_DT, APPROVE_STATUS,FREEZE_FLAG, b.SUB_DESC ");

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
            sb.Append(" select FESTIVAL_TYPE,FESTIVAL_DT,FESTIVAL_PAY_DT, TARGET_GEN_DT, RELEASE_DT, APPROVE_DT, SALARY_TRANS_DT, APPROVE_STATUS, FREEZE_FLAG   ");
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
            sb.Append(" group by FESTIVAL_TYPE, FESTIVAL_DT, FESTIVAL_PAY_DT, TARGET_GEN_DT, APPROVE_DT, RELEASE_DT, SALARY_TRANS_DT, APPROVE_STATUS,FREEZE_FLAG ");
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
                            , string festival_type, string festival_dt, string festival_pay_dt, string target_gen_dt
                            , string emp_id, string emp_name, string level_cd, string pjob_cd, string emp_cd, string pay_type, string emp_chg_cd
                           )
    {
        try
        {

            if (sortExpression.Contains("FESTIVAL_TYPE"))
                sortExpression = sortExpression.Replace("FESTIVAL_TYPE", "a.FESTIVAL_TYPE");
            if (sortExpression.Contains("FESTIVAL_DT"))
                sortExpression = sortExpression.Replace("FESTIVAL_DT", "a.FESTIVAL_DT");
            if (sortExpression.Contains("EMP_CD"))
                sortExpression = sortExpression.Replace("EMP_CD", "a.EMP_CD");
            if (sortExpression.Contains("FESTIVAL_PAY_DT"))
                sortExpression = sortExpression.Replace("FESTIVAL_PAY_DT", "a.FESTIVAL_PAY_DT");
            if (sortExpression.Contains("UPDATED_DT"))
                sortExpression = sortExpression.Replace("UPDATED_DT", "a.UPDATED_DT");


            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + ") As RowNumber,");
            sb.Append(" a.FESTIVAL_TYPE, a.FESTIVAL_DT, a.FESTIVAL_PAY_DT, a.EMP_ID, a.EMP_CD");  //pk值
            sb.Append(" , a.EMP_NAME, a.LEVEL_CD, a.JOIN_DT, a.UPDATED_DT ");
            sb.Append(" , a.WORK_DAYS, a.FESTIVAL_AMT, a.PAY_TYPE, a.EMP_CHG_CD, a.PJOB_CD ");
            sb.Append(" , a.APPROVE_MARK, a.CHG_STATUS, a.APPROVE_FLAG ");
            sb.Append(" , a.CHG_STATUS + '-' + b.SUB_DESC CHG_STATUS_DESC   ");
            sb.Append(" , a.EMP_CD + '-' + c.SUB_DESC EMP_CD_DESC   ");
            sb.Append(" , a.EMP_CHG_CD + '-' + d.SUB_DESC EMP_CHG_CD_DESC   ");
            sb.Append(" , a.PAY_TYPE + '-' + e.SUB_DESC PAY_TYPE_DESC   ");
            sb.Append(" from TB_S_M_FESTIVAL_D a ");
            sb.Append("  left join TB_9_M_COMM_D b on  a.CHG_STATUS = b.SUB_CD and b.MAIN_CD = 'CHG_STATUS' and b.IS_VALID='Y'  and b.SYS_CD='SA' ");
            sb.Append("  left join TB_9_M_COMM_D c on  a.EMP_CD = c.SUB_CD and c.MAIN_CD = 'EMP_CD' and c.IS_VALID='Y' and c.SYS_CD='HB' ");
            sb.Append("  left join TB_9_M_COMM_D d on  a.EMP_CHG_CD = d.SUB_CD and d.MAIN_CD = 'EMP_CHG_CD' and d.IS_VALID='Y' and d.SYS_CD='HB' ");
            sb.Append("  left join TB_9_M_COMM_D e on  a.PAY_TYPE = e.SUB_CD and e.MAIN_CD = 'PAY_TYPE' and e.IS_VALID='Y'  and e.SYS_CD='SC' ");
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
            //需加此段以免相同的節金日期，不同的對象生成日時
            if (target_gen_dt != "")
            {
                sb.Append(" and f.TARGET_GEN_DT = @TARGET_GEN_DT ");
                ht.Add("@TARGET_GEN_DT", Convert.ToDateTime(target_gen_dt).ToString("yyyy/MM/dd"));
            }


            if (emp_id != "")
            {
                sb.Append(" and a.EMP_ID like @EMP_ID ");
                ht.Add("@EMP_ID", emp_id + "%");
            }

            if (emp_name != "")
            {
                sb.Append(" and a.EMP_NAME like @EMP_NAME ");
                ht.Add("@EMP_NAME", emp_name + "%");
            }

            if (level_cd != "")
            {
                sb.Append(" and a.LEVEL_CD like @LEVEL_CD ");
                ht.Add("@LEVEL_CD", level_cd + "%");
            }
            if (pjob_cd != "")
            {
                sb.Append(" and a.PJOB_CD like @PJOB_CD ");
                ht.Add("@PJOB_CD", pjob_cd + "%");
            }

            //查詢條件-dropDownList
            if (emp_cd != "-1")
            {
                sb.Append(" and a.EMP_CD = @EMP_CD ");
                ht.Add("@EMP_CD", emp_cd);
            }
            if (pay_type != "-1")
            {
                sb.Append(" and a.PAY_TYPE = @PAY_TYPE ");
                ht.Add("@PAY_TYPE", pay_type);
            }
            if (emp_chg_cd != "-1")
            {
                sb.Append(" and a.EMP_CHG_CD = @EMP_CHG_CD ");
                ht.Add("@EMP_CHG_CD", emp_chg_cd);
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
                            , string festival_type, string festival_dt, string festival_pay_dt, string target_gen_dt
                            , string emp_id, string emp_name, string level_cd, string pjob_cd, string emp_cd, string pay_type, string emp_chg_cd
                        )
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) resultCount ");
            sb.Append(" from TB_S_M_FESTIVAL_D a");
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
            //需加此段以免相同的節金日期，不同的對象生成日時
            if (target_gen_dt != "")
            {
                sb.Append(" and f.TARGET_GEN_DT = @TARGET_GEN_DT ");
                ht.Add("@TARGET_GEN_DT", Convert.ToDateTime(target_gen_dt).ToString("yyyy/MM/dd"));
                //sb.Append(" and a.EMP_CD in (  ");
                //sb.Append(" select m.EMP_CD from  TB_S_M_FESTIVAL_H  m ");
                //sb.Append(" where m.FESTIVAL_TYPE = @FESTIVAL_TYPE_m ");
                //sb.Append(" and m.FESTIVAL_DT = @FESTIVAL_DT_m ");
                //sb.Append(" and m.FESTIVAL_PAY_DT = @FESTIVAL_PAY_DT_m ");
                //sb.Append(" and m.TARGET_GEN_DT = @TARGET_GEN_DT_m ");
                //sb.Append(" ) ");
                //ht.Add("@FESTIVAL_TYPE_m", festival_type);
                //ht.Add("@FESTIVAL_DT_m", Convert.ToDateTime(festival_dt).ToString("yyyy/MM/dd"));
                //ht.Add("@FESTIVAL_PAY_DT_m", Convert.ToDateTime(festival_pay_dt).ToString("yyyy/MM/dd"));
                //ht.Add("@TARGET_GEN_DT_m", Convert.ToDateTime(target_gen_dt).ToString("yyyy/MM/dd"));
            }


            if (emp_id != "")
            {
                sb.Append(" and a.EMP_ID like @EMP_ID ");
                ht.Add("@EMP_ID", emp_id + "%");
            }

            if (emp_name != "")
            {
                sb.Append(" and a.EMP_NAME like @EMP_NAME ");
                ht.Add("@EMP_NAME", emp_name + "%");
            }

            if (level_cd != "")
            {
                sb.Append(" and a.LEVEL_CD like @LEVEL_CD ");
                ht.Add("@LEVEL_CD", level_cd + "%");
            }
            if (pjob_cd != "")
            {
                sb.Append(" and a.PJOB_CD like @PJOB_CD ");
                ht.Add("@PJOB_CD", pjob_cd + "%");
            }

            //查詢條件-dropDownList
            if (emp_cd != "-1")
            {
                sb.Append(" and a.EMP_CD = @EMP_CD ");
                ht.Add("@EMP_CD", emp_cd);
            }
            if (pay_type != "-1")
            {
                sb.Append(" and a.PAY_TYPE = @PAY_TYPE ");
                ht.Add("@PAY_TYPE", pay_type);
            }
            if (emp_chg_cd != "-1")
            {
                sb.Append(" and a.EMP_CHG_CD = @EMP_CHG_CD ");
                ht.Add("@EMP_CHG_CD", emp_chg_cd);
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

    #region DB存取 (Qry)
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




    //提出核可
    public void updateRelease()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            DateTime now = DateTime.Parse(DateTime.Now.ToString());
            sb.Append(" update TB_S_M_FESTIVAL_H ");
            sb.Append(" set RELEASE_DT = @RELEASE_DT ");
            sb.Append(" ,RELEASE_BY = @RELEASE_BY ");
            sb.Append(" ,APPROVE_DT = @APPROVE_DT ");
            sb.Append(" ,APPROVE_BY = @APPROVE_BY ");
            sb.Append(" ,APPROVE_STATUS = @APPROVE_STATUS ");
            sb.Append(" ,FREEZE_FLAG = @FREEZE_FLAG ");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT = @UPDATED_DT");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where FESTIVAL_TYPE = @FESTIVAL_TYPE ");
            sb.Append("  and FESTIVAL_DT = @FESTIVAL_DT");
            sb.Append("  and FESTIVAL_PAY_DT = @FESTIVAL_PAY_DT");
            sb.Append("  and TARGET_GEN_DT = @TARGET_GEN_DT");

            ht.Add("@RELEASE_DT", RELEASE_DT);
            ht.Add("@RELEASE_BY", RELEASE_BY);
            ht.Add("@APPROVE_DT", DBNull.Value);
            ht.Add("@APPROVE_BY", "");
            ht.Add("@APPROVE_STATUS", APPROVE_STATUS);
            ht.Add("@FREEZE_FLAG", FREEZE_FLAG);

            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@UPDATED_DT", now);
            ht.Add("@FUNC_ID", FUNC_ID);

            ht.Add("@FESTIVAL_TYPE", FESTIVAL_TYPE);
            ht.Add("@FESTIVAL_DT", Convert.ToDateTime(FESTIVAL_DT));
            ht.Add("@FESTIVAL_PAY_DT", Convert.ToDateTime(FESTIVAL_PAY_DT));
            ht.Add("@TARGET_GEN_DT", Convert.ToDateTime(TARGET_GEN_DT));

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

           
            sb.Append(" update TB_S_M_FESTIVAL_H ");
            sb.Append(" set SALARY_TRANS_DT = @SALARY_TRANS_DT ");
            sb.Append(" ,SALARY_TRANS_BY = @SALARY_TRANS_BY ");
            sb.Append(" ,FREEZE_FLAG = @FREEZE_FLAG ");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT = @UPDATED_DT");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where FESTIVAL_TYPE = @FESTIVAL_TYPE ");
            sb.Append("  and FESTIVAL_DT = @FESTIVAL_DT");
            sb.Append("  and FESTIVAL_PAY_DT = @FESTIVAL_PAY_DT");
            sb.Append("  and TARGET_GEN_DT = @TARGET_GEN_DT");

            ht.Add("@SALARY_TRANS_DT", SALARY_TRANS_DT);
            ht.Add("@SALARY_TRANS_BY", SALARY_TRANS_BY);
            ht.Add("@FREEZE_FLAG", FREEZE_FLAG);

            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@UPDATED_DT", now);
            ht.Add("@FUNC_ID", FUNC_ID);

            ht.Add("@FESTIVAL_TYPE", FESTIVAL_TYPE);
            ht.Add("@FESTIVAL_DT", Convert.ToDateTime(FESTIVAL_DT));
            ht.Add("@FESTIVAL_PAY_DT", Convert.ToDateTime(FESTIVAL_PAY_DT));
            ht.Add("@TARGET_GEN_DT", Convert.ToDateTime(TARGET_GEN_DT));

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //薪資轉出-刪除節金條件設定歷史檔
    public void deleteLog(){
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("delete from TB_S_M_FESTIVAL_COND_LOG ");
            sb.Append(" where FESTIVAL_TYPE = @FESTIVAL_TYPE and FESTIVAL_YEAR = YEAR(GETDATE())  ");
            ht.Add("@FESTIVAL_TYPE", FESTIVAL_TYPE);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //薪資轉出-刪除節金條件設定歷史檔
    public void insertLog(DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" insert into  TB_S_M_FESTIVAL_COND_LOG ");
            sb.Append(" ( ");
            sb.Append(" FESTIVAL_YEAR ");
            sb.Append(" ,FESTIVAL_TYPE,FESTIVAL_PAY_COND,FESTIVAL_AMT,WORK_YEARS_SDT,WORK_YEARS_EDT,PRID_CD ");
            sb.Append(" ,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID  ");
            sb.Append(" ) ");
            sb.Append(" select YEAR(GETDATE()), FESTIVAL_TYPE,FESTIVAL_PAY_COND,FESTIVAL_AMT,WORK_YEARS_SDT,WORK_YEARS_EDT,PRID_CD,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID from TB_S_M_FESTIVAL_COND ");
            sb.Append(" where FESTIVAL_TYPE = @FESTIVAL_TYPE   ");

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

    #region 資料取得(Dtl)

    //判斷PK值是否存在 
    public DataTable getPKData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select count(0) resultCount ");
            sb.Append(" from TB_S_M_FESTIVAL_D ");
            sb.Append(" where 1=1 ");
            sb.Append(" and FESTIVAL_TYPE = @FESTIVAL_TYPE      ");
            sb.Append(" and FESTIVAL_DT = @FESTIVAL_DT          ");
            sb.Append(" and FESTIVAL_PAY_DT = @FESTIVAL_PAY_DT  ");
            sb.Append(" and EMP_ID = @EMP_ID  ");

            ht.Add("@FESTIVAL_TYPE", FESTIVAL_TYPE);
            ht.Add("@FESTIVAL_DT", Convert.ToDateTime(FESTIVAL_DT));
            ht.Add("@FESTIVAL_PAY_DT", Convert.ToDateTime(FESTIVAL_PAY_DT));
            ht.Add("@EMP_ID", EMP_ID);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getEMPCDData(string empCD)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select count(0) resultCount ");
            sb.Append(" from TB_S_M_FESTIVAL_H ");
            sb.Append(" where 1=1 ");
            sb.Append(" and FESTIVAL_TYPE = @FESTIVAL_TYPE      ");
            sb.Append(" and FESTIVAL_DT = @FESTIVAL_DT          ");
            sb.Append(" and FESTIVAL_PAY_DT = @FESTIVAL_PAY_DT  ");
            sb.Append("  and TARGET_GEN_DT = @TARGET_GEN_DT     ");
            sb.Append("  and EMP_CD = @EMP_CD     ");

            ht.Add("@FESTIVAL_TYPE", FESTIVAL_TYPE);
            ht.Add("@FESTIVAL_DT", Convert.ToDateTime(FESTIVAL_DT));
            ht.Add("@FESTIVAL_PAY_DT", Convert.ToDateTime(FESTIVAL_PAY_DT));
            ht.Add("@TARGET_GEN_DT", Convert.ToDateTime(TARGET_GEN_DT));
            ht.Add("@EMP_CD", empCD);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //Grid的員工工號 fro  ajax
    public DataTable getEmpData(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" select EMP_ID, EMP_NAME,EMP_CD,LEVEL_CD, JOIN_DT,WORK_DAYS,EMP_CHG_CD,PJOB_CD ");
            sb.Append(" , EMP_CD + '-' + c.SUB_DESC EMP_CD_DESC ");
            sb.Append(" , EMP_CHG_CD + '-' + d.SUB_DESC EMP_CHG_CD_DESC   ");
            sb.Append(" from VW_H_EMP_DATA a ");
            sb.Append("  left join TB_9_M_COMM_D c on  a.EMP_CD = c.SUB_CD and c.MAIN_CD = 'EMP_CD'  and c.SYS_CD='HB' ");
            sb.Append("  left join TB_9_M_COMM_D d on  a.EMP_CHG_CD = d.SUB_CD and d.MAIN_CD = 'EMP_CHG_CD'  and d.SYS_CD='HB' ");
            sb.Append(" where 1=1 ");
            if (emp_id != "")
            {
                sb.Append(" and EMP_ID = @EMP_ID");
                ht.Add("@EMP_ID", emp_id);
            }

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //取得EXCEL下載資料，
    public  DataTable getMaintainData()
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
            sb.Append("  left join TB_9_M_COMM_D b on  a.CHG_STATUS = b.SUB_CD and b.MAIN_CD = 'CHG_STATUS'  and b.SYS_CD='SA' ");
            sb.Append("  left join TB_9_M_COMM_D c on  a.EMP_CD = c.SUB_CD and c.MAIN_CD = 'EMP_CD'  and c.SYS_CD='HB' ");
            sb.Append("  left join TB_9_M_COMM_D d on  a.EMP_CHG_CD = d.SUB_CD and d.MAIN_CD = 'EMP_CHG_CD'  and d.SYS_CD='HB' ");
            sb.Append("  left join TB_9_M_COMM_D e on  a.PAY_TYPE = e.SUB_CD and e.MAIN_CD = 'PAY_TYPE'  and e.SYS_CD='SC' ");
            sb.Append("  left join TB_9_M_COMM_D f on  a.PAY_TYPE_OLD = f.SUB_CD and f.MAIN_CD = 'PAY_TYPE'  and f.SYS_CD='SC' ");
            sb.Append("  left join TB_9_M_COMM_D g on  a.PLANT_CD = g.SUB_CD and g.MAIN_CD = 'PLANT_CD'  and g.SYS_CD='HB' ");
            sb.Append("  left join TB_9_M_COMM_D h on  a.FESTIVAL_TYPE = h.SUB_CD and h.MAIN_CD = 'FESTIVAL_TYPE'  and h.SYS_CD='SG' ");
            sb.Append("  left join TB_9_M_COMM_D j on  a.JPN_CD = j.SUB_CD and j.MAIN_CD = 'JPN_CD'  and j.SYS_CD='HB' ");
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

    //節金明細維護檔 取得核可狀態
    internal DataTable getHeaderData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select APPROVE_STATUS  from  TB_S_M_FESTIVAL_H ");
            sb.Append(" where FESTIVAL_TYPE = @FESTIVAL_TYPE ");
            sb.Append("  and FESTIVAL_DT = @FESTIVAL_DT");
            sb.Append("  and FESTIVAL_PAY_DT = @FESTIVAL_PAY_DT");
            sb.Append("  and EMP_CD = @EMP_CD");

            ht.Add("@FESTIVAL_TYPE", FESTIVAL_TYPE);
            ht.Add("@FESTIVAL_DT", Convert.ToDateTime(FESTIVAL_DT).ToString("yyyy/MM/dd"));
            ht.Add("@FESTIVAL_PAY_DT", Convert.ToDateTime(FESTIVAL_PAY_DT).ToString("yyyy/MM/dd"));
            ht.Add("@EMP_CD", EMP_CD);


            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
    //節金明細維護檔 取得異動狀態,明細核可狀態，
    internal DataTable getDetailData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select CHG_STATUS,  APPROVE_FLAG, PAY_TYPE, FESTIVAL_AMT   from  TB_S_M_FESTIVAL_D ");
            sb.Append(" where FESTIVAL_TYPE = @FESTIVAL_TYPE ");
            sb.Append("  and FESTIVAL_DT = @FESTIVAL_DT");
            sb.Append("  and FESTIVAL_PAY_DT = @FESTIVAL_PAY_DT");
            sb.Append("  and EMP_ID = @EMP_ID");

            ht.Add("@FESTIVAL_TYPE", FESTIVAL_TYPE);
            ht.Add("@FESTIVAL_DT", Convert.ToDateTime(FESTIVAL_DT).ToString("yyyy/MM/dd"));
            ht.Add("@FESTIVAL_PAY_DT", Convert.ToDateTime(FESTIVAL_PAY_DT).ToString("yyyy/MM/dd"));
            ht.Add("@EMP_ID", EMP_ID);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getEmpBasicData() {


        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("  select EMP_ID, EMP_NAME, DEPT_NO, PLANT_CD, JPN_CD  ");
            sb.Append("  , COMPANY_CD, LEVEL_CD, GRADE_CD, PJOB_CD, JOIN_DT  ");
            sb.Append("  , WORK_DAYS, EMP_CD, EMP_CHG_CD, WS_CD, SEX_CD ");
            sb.Append("   from VW_H_EMP_DATA  ");
            sb.Append(" where 1=1 ");
            sb.Append(" and EMP_ID = @EMP_ID ");
            ht.Add("@EMP_ID", EMP_ID);
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
            sb.Append("  left join TB_9_M_COMM_D b on  a.FESTIVAL_TYPE = b.SUB_CD and b.MAIN_CD = 'FESTIVAL_TYPE'  and b.SYS_CD='SG' ");
            sb.Append("  left join TB_9_M_COMM_D c on  a.APPROVE_STATUS = c.SUB_CD and c.MAIN_CD = 'APPROVE_STATUS'  and c.SYS_CD='SA' ");
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

            if (TARGET_GEN_DT != "")
            {
                sb.Append(" and TARGET_GEN_DT  = @TARGET_GEN_DT");
                ht.Add("@TARGET_GEN_DT", Convert.ToDateTime(TARGET_GEN_DT).ToString("yyyy/MM/dd"));
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
                this.TARGET_GEN_DT = Convert.ToString(dr["TARGET_GEN_DT"]);
                //this.FESTIVAL_TOTAL_AMT = Convert.ToString(dr["FESTIVAL_TOTAL_AMT"]);
                //this.FESTIVAL_TOTAL_NUM = Convert.ToString(dr["FESTIVAL_TOTAL_NUM"]);
            }


            sb = new StringBuilder();
            ht = new Hashtable();
            sb.Append(" select isnull(sum(FESTIVAL_TOTAL_AMT),0) amt, isnull(sum(FESTIVAL_TOTAL_NUM),0) num   ");
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

            if (TARGET_GEN_DT != "")
            {
                sb.Append(" and TARGET_GEN_DT  = @TARGET_GEN_DT");
                ht.Add("@TARGET_GEN_DT", Convert.ToDateTime(TARGET_GEN_DT).ToString("yyyy/MM/dd"));
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

    #region DB存取(Dtl)

    //新增(節金明細維護檔)
    public void insertDataDtl_D(DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" INSERT INTO TB_S_M_FESTIVAL_D ");
            sb.Append(" ( ");
            sb.Append(" FESTIVAL_TYPE, FESTIVAL_DT, FESTIVAL_PAY_DT, EMP_ID, EMP_NAME ");
            sb.Append(" , DEPT_NO, PLANT_CD, JPN_CD, COMPANY_CD, LEVEL_CD  ");
            sb.Append(" , GRADE_CD, PJOB_CD, JOIN_DT, WORK_DAYS, EMP_CD  ");
            sb.Append(" , EMP_CHG_CD, WS_CD, SEX_CD, FESTIVAL_AMT, FESTIVAL_TAX  ");
            sb.Append(" , FESTIVAL_AMT_R, PAY_TYPE, APPROVE_FLAG, CHG_STATUS  ");
            sb.Append(" , CREATED_BY, CREATED_DT, UPDATED_BY, UPDATED_DT, FUNC_ID  ");
            sb.Append(" ) ");
            sb.Append(" VALUES ");
            sb.Append(" ( ");
            sb.Append(" @FESTIVAL_TYPE, @FESTIVAL_DT, @FESTIVAL_PAY_DT, @EMP_ID, @EMP_NAME  ");
            sb.Append(" , @DEPT_NO, @PLANT_CD, @JPN_CD, @COMPANY_CD, @LEVEL_CD  ");
            sb.Append(" ,@GRADE_CD,  @PJOB_CD,  @JOIN_DT,  @WORK_DAYS,  @EMP_CD ");
            sb.Append(" ,@EMP_CHG_CD,  @WS_CD,  @SEX_CD,  @FESTIVAL_AMT,  @FESTIVAL_TAX ");
            sb.Append(" ,@FESTIVAL_AMT_R,  @PAY_TYPE,  @APPROVE_FLAG,  @CHG_STATUS ");
            sb.Append(" ,@CREATED_BY,  @CREATED_DT,  @UPDATED_BY,  @UPDATED_DT,  @FUNC_ID ");
            sb.Append(" ) ");
            //set值
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
            ht.Add("@APPROVE_FLAG", APPROVE_FLAG);
            ht.Add("@CHG_STATUS", CHG_STATUS);
            //新修日期
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

    //更新節金維護檔-重新計算總金額及人數，及回復成未核可的狀態
    public void updateStatus_H(DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //總金額
            StringBuilder sb_AMT = new StringBuilder();
            sb_AMT.Append("  FESTIVAL_TOTAL_AMT = ( select isnull( sum(FESTIVAL_AMT),0) from  TB_S_M_FESTIVAL_D  ");
            sb_AMT.Append("  where FESTIVAL_TYPE = @FESTIVAL_TYPE                                                ");
            sb_AMT.Append("  and FESTIVAL_DT = @FESTIVAL_DT                                                      ");
            sb_AMT.Append("  and FESTIVAL_PAY_DT = @FESTIVAL_PAY_DT                                              ");
            sb_AMT.Append("  and EMP_CD = @EMP_CD                                                                ");
            sb_AMT.Append("  and CHG_STATUS <> @CHG_STATUS                                                       ");
            sb_AMT.Append("  ) ");
            //總人數
            StringBuilder sb_NUM = new StringBuilder();
            sb_NUM.Append(" , FESTIVAL_TOTAL_NUM = ( select   count(FESTIVAL_AMT) from  TB_S_M_FESTIVAL_D ");
            sb_NUM.Append(" where FESTIVAL_TYPE = @FESTIVAL_TYPE ");
            sb_NUM.Append("  and FESTIVAL_DT = @FESTIVAL_DT ");
            sb_NUM.Append("  and FESTIVAL_PAY_DT = @FESTIVAL_PAY_DT ");
            sb_NUM.Append("  and EMP_CD = @EMP_CD ");
            sb_NUM.Append("  and CHG_STATUS <> @CHG_STATUS ");
            sb_NUM.Append("  ) ");


            sb.Append(" update TB_S_M_FESTIVAL_H ");
            sb.Append(" set  ");
            sb.Append(sb_AMT);
            sb.Append(sb_NUM);
            sb.Append(" ,RELEASE_DT = @RELEASE_DT");
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
            sb.Append("  and EMP_CD = @EMP_CD");

            //總金額,總人數條件
            ht.Add("@EMP_CD", EMP_CD);
            ht.Add("@CHG_STATUS", "D");


            //set值
            ht.Add("@RELEASE_DT", DBNull.Value);
            ht.Add("@RELEASE_BY", "");
            ht.Add("@APPROVE_DT", DBNull.Value);
            ht.Add("@APPROVE_BY", "");
            ht.Add("@APPROVE_STATUS", "N");
            ht.Add("@FREEZE_FLAG", "N");

            //新修日期
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_DT", now);
            ht.Add("@FUNC_ID", "FB2SG030");

            //PK值
            ht.Add("@FESTIVAL_TYPE", FESTIVAL_TYPE);
            if (FESTIVAL_DT != "")
            {
                this.FESTIVAL_DT = Convert.ToDateTime(FESTIVAL_DT).ToString("yyyy/MM/dd");
            }
            if (FESTIVAL_PAY_DT != "")
            {
                this.FESTIVAL_PAY_DT = Convert.ToDateTime(FESTIVAL_PAY_DT).ToString("yyyy/MM/dd");
            }
            if (TARGET_GEN_DT != "")
            {
                this.TARGET_GEN_DT = Convert.ToDateTime(TARGET_GEN_DT).ToString("yyyy/MM/dd");
                sb.Append("  and TARGET_GEN_DT = @TARGET_GEN_DT");
                ht.Add("@TARGET_GEN_DT", TARGET_GEN_DT);
            }
            else
            {
                sb.Append("  and TARGET_GEN_DT is null ");
            }
            ht.Add("@FESTIVAL_DT", FESTIVAL_DT);
            ht.Add("@FESTIVAL_PAY_DT", FESTIVAL_PAY_DT);
            
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }


    //刪除 更新異動狀態為(DTL員工)
    public void updateStatus2DeleteDtl_D(DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_S_M_FESTIVAL_D ");
            sb.Append(" set CHG_STATUS = @CHG_STATUS ");
            sb.Append(" ,APPROVE_FLAG = @APPROVE_FLAG");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT = @UPDATED_DT");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where FESTIVAL_TYPE = @FESTIVAL_TYPE ");
            sb.Append("  and FESTIVAL_DT = @FESTIVAL_DT");
            sb.Append("  and FESTIVAL_PAY_DT = @FESTIVAL_PAY_DT");
            sb.Append("  and EMP_ID = @EMP_ID");
            ht.Add("@CHG_STATUS", CHG_STATUS);
            ht.Add("@APPROVE_FLAG", APPROVE_FLAG);

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

    //修改-支付狀態及節金金額(節金明細維護檔)
    public void updateDataDtl_D(DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" update TB_S_M_FESTIVAL_D ");
            sb.Append(" set FESTIVAL_AMT = @FESTIVAL_AMT ");
            sb.Append(" ,FESTIVAL_TAX = @FESTIVAL_TAX");
            sb.Append(" ,FESTIVAL_AMT_R = @FESTIVAL_AMT_R");
            sb.Append(" ,FESTIVAL_AMT_OLD = @FESTIVAL_AMT_OLD");
            sb.Append(" ,PAY_TYPE = @PAY_TYPE");
            sb.Append(" ,PAY_TYPE_OLD = @PAY_TYPE_OLD");
            sb.Append(" ,CHG_STATUS = @CHG_STATUS ");
            sb.Append(" ,APPROVE_FLAG = @APPROVE_FLAG");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT = @UPDATED_DT");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where FESTIVAL_TYPE = @FESTIVAL_TYPE ");
            sb.Append("  and FESTIVAL_DT = @FESTIVAL_DT");
            sb.Append("  and FESTIVAL_PAY_DT = @FESTIVAL_PAY_DT");
            sb.Append("  and EMP_ID = @EMP_ID");
            //set值
            ht.Add("@FESTIVAL_AMT", FESTIVAL_AMT);
            ht.Add("@FESTIVAL_TAX", FESTIVAL_TAX);
            ht.Add("@FESTIVAL_AMT_R", FESTIVAL_AMT_R);
            ht.Add("@FESTIVAL_AMT_OLD", FESTIVAL_AMT_OLD);
            ht.Add("@PAY_TYPE", PAY_TYPE);
            ht.Add("@PAY_TYPE_OLD", PAY_TYPE_OLD);
            ht.Add("@CHG_STATUS", CHG_STATUS);
            ht.Add("@APPROVE_FLAG", APPROVE_FLAG);

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

    //支付狀態一括更新(節金明細維護檔)
    public void updatePayType_D(DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" update TB_S_M_FESTIVAL_D                ");
            sb.Append(" set CHG_STATUS = @CHG_STATUS            ");
            sb.Append(" ,PAY_TYPE = @PAY_TYPE                   ");
            sb.Append(" ,PAY_TYPE_OLD = @PAY_TYPE_OLD           ");
            sb.Append(" ,APPROVE_FLAG = @APPROVE_FLAG           ");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY               ");
            sb.Append(" ,UPDATED_DT = @UPDATED_DT               ");
            sb.Append(" ,FUNC_ID = @FUNC_ID                     ");
            sb.Append(" where FESTIVAL_TYPE = @FESTIVAL_TYPE    ");
            sb.Append("  and FESTIVAL_DT = @FESTIVAL_DT         ");
            sb.Append("  and FESTIVAL_PAY_DT = @FESTIVAL_PAY_DT ");
            sb.Append("  and EMP_ID = @EMP_ID                   ");

            //set值
            ht.Add("@CHG_STATUS", CHG_STATUS);
            ht.Add("@PAY_TYPE", PAY_TYPE);
            ht.Add("@PAY_TYPE_OLD", PAY_TYPE_OLD);
            ht.Add("@APPROVE_FLAG", APPROVE_FLAG);

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



    #endregion
}
