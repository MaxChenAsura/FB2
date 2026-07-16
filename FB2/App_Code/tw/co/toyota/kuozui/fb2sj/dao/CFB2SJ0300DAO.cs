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
/// CFB2SJ0300DAO 的摘要描述
/// </summary>
public class CFB2SJ0300DAO : BaseDAO
{
    //考核資料維護檔 欄位
    public string ASSESS_YEAR { get; set; }
    public string ASSESS_TYPE { get; set; }
    public string ASSESS_TYPE_DESC { get; set; }
    public string TARGET_GEN_DT { get; set; }
    public string GEN_DT { get; set; }
    public string RELEASE_DT { get; set; }
    public string RELEASE_BY { get; set; }
    public string APPROVE_DT { get; set; }
    public string APPROVE_BY { get; set; }
    public string APPROVE_STATUS { get; set; }
    public string APPROVE_STATUS_DESC { get; set; }
    public string REMARK { get; set; }
    public string ASSESS_RELEASE_DT { get; set; }
    public string ASSESS_RELEASE_BY { get; set; }
    public string FREEZE_FLAG { get; set; }

    //考核人事資料維護檔
    public string EMP_ID { get; set; }
    public string SCORE_FINAL { get; set; }
    public string SCORE_DEPT { get; set; }

    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }




    public CFB2SJ0300DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }


    #region 資料取得

    //取得Dtl的表頭資料
    public void getTitleData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" select  a.*   ");
            sb.Append(" , a.APPROVE_STATUS + '-' + c.SUB_DESC APPROVE_STATUS_DESC   ");
            sb.Append(" , a.ASSESS_TYPE+'-'+e.SUB_DESC ASSESS_TYPE_DESC   ");
            sb.Append(" from TB_S_M_ASSESS_DATA a ");
            sb.Append("  left join TB_9_M_COMM_D c on  a.APPROVE_STATUS = c.SUB_CD and c.MAIN_CD = 'APPROVE_STATUS'  and c.SYS_CD='SA' and c.IS_VALID='Y' ");
            sb.Append("  left join TB_9_M_COMM_D e on  a.ASSESS_TYPE = e.SUB_CD  and e.MAIN_CD = 'ASSESS_TYPE'  and e.IS_VALID='Y' and  e.SYS_CD='SJ'  ");
            sb.Append(" where 1=1 ");
            if (ASSESS_YEAR != "")
            {
                sb.Append(" and ASSESS_YEAR = @ASSESS_YEAR ");
                ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            }
            if (ASSESS_TYPE != "")
            {
                sb.Append(" and ASSESS_TYPE = @ASSESS_TYPE ");
                ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            }

            DataTable dt = dbConn.Query(sb, ht);
            foreach (DataRow dr in dt.Rows)
            {
                this.ASSESS_RELEASE_DT = dr["ASSESS_RELEASE_DT"].ToString() != "" ? Convert.ToDateTime(dr["ASSESS_RELEASE_DT"].ToString()).ToString("yyyy/MM/dd") : "";
                this.ASSESS_TYPE_DESC = Convert.ToString(dr["ASSESS_TYPE_DESC"]);
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


    //判斷登入者是否為提出核可者的直屬長官
    public DataTable isDirectHeadEmp()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select count(a.RELEASE_BY) resultCount ");
            sb.Append(" from TB_S_M_ASSESS_DATA a ");
            sb.Append(" left join VW_H_EMP_DATA b on a.RELEASE_BY = b.EMP_ID ");
            sb.Append(" left join TB_H_R_HEAD_DEPT c on b.DEPT_NO=c.MNG_DEPT_NO ");
            sb.Append(" where 1=1 ");
            sb.Append(" and a.ASSESS_YEAR = @ASSESS_YEAR  ");
            sb.Append(" and a.ASSESS_TYPE = @ASSESS_TYPE  ");
            sb.Append(" and c.EMP_ID = @EMP_ID  ");

            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@EMP_ID", SessionHandle.Current.emp_id);

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
                            , string assess_year_s, string assess_year_e, string assess_type
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
            sb.Append(" a.*  ");
            sb.Append(" , a.ASSESS_TYPE+'-'+b.SUB_DESC ASSESS_TYPE_DESC   ");
            sb.Append(" , a.APPROVE_STATUS + '-' + c.SUB_DESC APPROVE_STATUS_DESC   ");
            sb.Append(" ,  isnull(d.EMP_NAME,'')   RELEASE_BY_NAME   ");
            sb.Append(" ,  isnull(e.EMP_NAME,'')   APPROVE_BY_NAME   ");
            sb.Append(" from TB_S_M_ASSESS_DATA a ");
             sb.Append("  left join TB_9_M_COMM_D b on  a.ASSESS_TYPE = b.SUB_CD  and b.MAIN_CD = 'ASSESS_TYPE'  and b.IS_VALID='Y' and  b.SYS_CD='SJ'  ");
            sb.Append("  left join TB_9_M_COMM_D c on  a.APPROVE_STATUS = c.SUB_CD and c.MAIN_CD = 'APPROVE_STATUS' and c.IS_VALID='Y' and c.SYS_CD='SA' ");
            sb.Append("  left join  VW_H_EMP_DATA d on a.RELEASE_BY = d.EMP_ID ");
            sb.Append("  left join  VW_H_EMP_DATA e on a.APPROVE_BY = e.EMP_ID ");
            sb.Append(" where 1=1 ");
            //查詢條件
            if (assess_year_s != "")
            {
                sb.Append(" and ASSESS_YEAR >= @ASSESS_YEAR_S ");
                ht.Add("@ASSESS_YEAR_S", assess_year_s);
            }
            if (assess_year_e != "")
            {
                sb.Append(" and ASSESS_YEAR <= @ASSESS_YEAR_E ");
                ht.Add("@ASSESS_YEAR_E", assess_year_e);
            }
            if (assess_type != "-1")
            {
                sb.Append(" and ASSESS_TYPE = @ASSESS_TYPE ");
                ht.Add("@ASSESS_TYPE", assess_type);
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
                             , string assess_year_s, string assess_year_e, string assess_type)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) resultCount ");
            sb.Append(" from TB_S_M_ASSESS_DATA ");
            sb.Append(" where 1=1 ");


            //查詢條件
            if (assess_year_s != "")
            {
                sb.Append(" and ASSESS_YEAR >= @ASSESS_YEAR_S ");
                ht.Add("@ASSESS_YEAR_S", assess_year_s);
            }
            if (assess_year_e != "")
            {
                sb.Append(" and ASSESS_YEAR <= @ASSESS_YEAR_E ");
                ht.Add("@ASSESS_YEAR_E", assess_year_e);
            }
            if (assess_type != "-1")
            {
                sb.Append(" and ASSESS_TYPE = @ASSESS_TYPE ");
                ht.Add("@ASSESS_TYPE", assess_type);
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
                                , string assess_year, string assess_type, string emp_id, string emp_name
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
            sb.Append(",  a.DEPT_NAME_20 +' '+a.DEPT_NAME_30+' '+ a.DEPT_NAME_40  DEPT_NAME_DESC   ");
            sb.Append(" , a.WS_CD + '-' + b.SUB_DESC WS_CD_DESC   ");
            sb.Append(" from TB_S_M_ASSESS_TARGET a ");
            sb.Append("  left join TB_9_M_COMM_D b on  a.WS_CD = b.SUB_CD and b.MAIN_CD = 'WS_CD' and b.IS_VALID='Y'  and b.SYS_CD='HB'   ");
            sb.Append(" where 1=1 ");

            //查詢條件
            if (assess_year != "")
            {
                sb.Append(" and a.ASSESS_YEAR = @ASSESS_YEAR ");
                ht.Add("@ASSESS_YEAR", assess_year);
            }
            if (assess_type != "")
            {
                sb.Append(" and a.ASSESS_TYPE = @ASSESS_TYPE ");
                ht.Add("@ASSESS_TYPE", assess_type);
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
                            , string assess_year, string assess_type, string emp_id, string emp_name
                        )
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) resultCount ");
            sb.Append(" from TB_S_M_ASSESS_TARGET a ");
            sb.Append(" where 1=1 ");

            //查詢條件
            if (assess_year != "")
            {
                sb.Append(" and a.ASSESS_YEAR = @ASSESS_YEAR ");
                ht.Add("@ASSESS_YEAR", assess_year);
            }
            if (assess_type != "")
            {
                sb.Append(" and a.ASSESS_TYPE = @ASSESS_TYPE ");
                ht.Add("@ASSESS_TYPE", assess_type);
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
	
	
    #endregion

    #region 駁回DB存取(Dtl)

    //駁回 更新-年獎明細維護檔
    public void updateRejectData_D(DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_S_M_ASSESS_TARGET ");
            sb.Append(" set APPROVE_MARK = @APPROVE_MARK ");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT = @UPDATED_DT");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where ASSESS_YEAR = @ASSESS_YEAR ");
            sb.Append("  and ASSESS_TYPE = @ASSESS_TYPE");
            sb.Append("  and EMP_ID = @EMP_ID");
            ht.Add("@APPROVE_MARK", "V");

            //PK值
            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
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

    //駁回 更新-(將全部的異動註記設為空白)
    public void updateAllRejectData_D(DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_S_M_ASSESS_TARGET ");
            sb.Append(" set CHG_STATUS = @CHG_STATUS ");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT = @UPDATED_DT");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where ASSESS_YEAR = @ASSESS_YEAR ");
            sb.Append("  and ASSESS_TYPE = @ASSESS_TYPE");
            ht.Add("@CHG_STATUS", ""); //異動註記為空白

            //PK值
            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);

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


    //異常註記-update 備註說明  (考核資料維護檔 DTL)
    public void updateMarkData_H(DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_S_M_ASSESS_DATA ");
            sb.Append(" set REMARK = @REMARK ");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT = @UPDATED_DT");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where ASSESS_YEAR = @ASSESS_YEAR ");
            sb.Append("  and ASSESS_TYPE = @ASSESS_TYPE");
            //set值
            ht.Add("@REMARK", REMARK);

            //新修日期
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@UPDATED_DT", now);
            ht.Add("@FUNC_ID", FUNC_ID);

            //PK值
            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }
    //異常註記-update 異常註記為空白 或V (考核資料維護檔 DTL)
    public void updateMarkData_D(DateTime now,string approve_mark )
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_S_M_ASSESS_TARGET ");
            sb.Append(" set APPROVE_MARK = @APPROVE_MARK ");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT = @UPDATED_DT");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where ASSESS_YEAR = @ASSESS_YEAR ");
            sb.Append("  and ASSESS_TYPE = @ASSESS_TYPE");
            sb.Append("  and EMP_ID = @EMP_ID");
            ht.Add("@APPROVE_MARK", approve_mark);

            //PK值
            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
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

    //Grid的部門資料
    public int getMarkData()
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) resultCount ");
            sb.Append(" from TB_S_M_ASSESS_TARGET a ");
            sb.Append(" where ASSESS_YEAR = @ASSESS_YEAR ");
            sb.Append("  and ASSESS_TYPE = @ASSESS_TYPE");
            sb.Append("  and APPROVE_MARK = @APPROVE_MARK");
            //PK值
            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
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


    //駁回-回復成未核可前狀態  (考核資料維護檔 DTL)
    public void updateRejectData_H(DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_S_M_ASSESS_DATA ");
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
            sb.Append(" where ASSESS_YEAR = @ASSESS_YEAR ");
            sb.Append("  and ASSESS_TYPE = @ASSESS_TYPE");
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
            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    #endregion


    #region 核可DB存取(Dtl)
    //核可-回復成核可狀態  (年獎維護檔 DTL)
    public void updateApproveData_H(DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_S_M_ASSESS_DATA ");
            //sb.Append(" set REMARK = @REMARK ");
            sb.Append("set APPROVE_DT = @APPROVE_DT");
            sb.Append(" ,APPROVE_BY = @APPROVE_BY");
            sb.Append(" ,APPROVE_STATUS = @APPROVE_STATUS");
            sb.Append(" ,FREEZE_FLAG = @FREEZE_FLAG");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT = @UPDATED_DT");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where ASSESS_YEAR = @ASSESS_YEAR ");
            sb.Append("  and ASSESS_TYPE = @ASSESS_TYPE");

            //set值
            //ht.Add("@REMARK", "");
            ht.Add("@APPROVE_DT", now);
            ht.Add("@APPROVE_BY", APPROVE_BY);
            ht.Add("@APPROVE_STATUS", "Y");
            ht.Add("@FREEZE_FLAG", "N");

            //新修日期
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@UPDATED_DT", now);
            ht.Add("@FUNC_ID", FUNC_ID);

            //PK值
            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }


    //核可 更新-考核人事資料維護檔
    public void updateAllApproveData_D(DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_S_M_ASSESS_TARGET ");
            sb.Append(" set APPROVE_MARK = @APPROVE_MARK ");
            sb.Append(" , CHG_STATUS = @CHG_STATUS ");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT = @UPDATED_DT");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where ASSESS_YEAR = @ASSESS_YEAR ");
            sb.Append("  and ASSESS_TYPE = @ASSESS_TYPE");
            //set值
            ht.Add("@APPROVE_MARK", "");
            ht.Add("@CHG_STATUS", "");

            //PK值
            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);

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


    //核可,刪除-考核人事資料主檔
    public void deleteApproveData_D_H(DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" delete from TB_S_R_ASSESS_TARGET ");
            sb.Append(" where ASSESS_YEAR = @ASSESS_YEAR ");
            sb.Append("  and ASSESS_TYPE = @ASSESS_TYPE");

            //PK值
            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //核可, 新增-考核人事資料主檔
    public void insertApproveData_D_H(DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" insert into TB_S_R_ASSESS_TARGET  ( ");
            sb.Append("  ASSESS_YEAR,ASSESS_TYPE,EMP_ID,EMP_NAME,DEPT_NO ");
            sb.Append(" ,DEPT_NAME,DEPT_NO_20,DEPT_NAME_20,DEPT_NAME_30,DEPT_NO_40,DEPT_NAME_40 ");
            sb.Append(" ,DEPT_NAME_50,DEPT_NAME_60,DEPT_NAME_70,SEX_CD,LINE_CD,WS_CD ");
            sb.Append(" ,PLANT_CD,EMP_CD,LEVEL_CD,LEVEL_ORDER_SEQ,GRADE_CD ");
            sb.Append(" ,PJOB_CD,PJOB_DESC,EDUCATION_CD,BIRTH_DT,RECENT_LEVEL_WORK_YEARS ");
            sb.Append(" ,AGE,WORK_YEARS,EMP_STATUS,EMP_CHG_CD,SCORE_1H_1,SCORE_1H_2,SCORE_1H_3,SCORE_2H_1 ");
            sb.Append(" ,SCORE_2H_2,SCORE_2H_3,OVERTIME_HOUR_MEAN,LEAVE_O,LEAVE_P,LEAVE_Q,LEAVE_A,LEAVE_B,RETENTION_DAYS ");
            sb.Append(" ,THIRD_CNT_P,SECOND_CNT_P,FIRST_CNT_P,THIRD_CNT_M,SECOND_CNT_M,FIRST_CNT_M,PROPOSAL_TOTAL ");
            sb.Append(" ,PROPOSAL_GRADE,PROPOSAL_GRADE_MEAN,PROPOSAL_6,END_PLANT_CD,END_DEPT_NO,END_DEPT_NAME ");
            sb.Append(" ,ORI_PLANT_CD,ORI_DEPT_NO,ORI_DEPT_NAME,START_DT,PLAN_END_DT,DEPT_NO_TY ");
            sb.Append(" ,DEPT_NAME_TY,DEPT_NO_LY,DEPT_NAME_LY,DEPT_FLAG,LEVELUP_FLAG,ARREARS_FLAG ");
            sb.Append(" ,SCORE_LY,SCORE_DEPT,SCORE_FINAL,SCORE_FINAL_FLAG,SCORE_FLAG,APPROVE_MARK,CHG_STATUS ");
            sb.Append(" ,UNION_PJOB_CD,IS_OUT,SUGGEST_DESC,RECOMM_DESC,COMMENTS,DISTING_REMARK,LIMIT_RATE ");
            sb.Append(" ,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID ");
            sb.Append(" ) ");
            sb.Append(" select  ");
            sb.Append("  ASSESS_YEAR,ASSESS_TYPE,EMP_ID,EMP_NAME,DEPT_NO ");
            sb.Append(" ,DEPT_NAME,DEPT_NO_20,DEPT_NAME_20,DEPT_NAME_30,DEPT_NO_40,DEPT_NAME_40 ");
            sb.Append(" ,DEPT_NAME_50,DEPT_NAME_60,DEPT_NAME_70,SEX_CD,LINE_CD,WS_CD ");
            sb.Append(" ,PLANT_CD,EMP_CD,LEVEL_CD,LEVEL_ORDER_SEQ,GRADE_CD ");
            sb.Append(" ,PJOB_CD,PJOB_DESC,EDUCATION_CD,BIRTH_DT,RECENT_LEVEL_WORK_YEARS ");
            sb.Append(" ,AGE,WORK_YEARS,EMP_STATUS,EMP_CHG_CD,SCORE_1H_1,SCORE_1H_2,SCORE_1H_3,SCORE_2H_1 ");
            sb.Append(" ,SCORE_2H_2,SCORE_2H_3,OVERTIME_HOUR_MEAN,LEAVE_O,LEAVE_P,LEAVE_Q,LEAVE_A,LEAVE_B,RETENTION_DAYS ");
            sb.Append(" ,THIRD_CNT_P,SECOND_CNT_P,FIRST_CNT_P,THIRD_CNT_M,SECOND_CNT_M,FIRST_CNT_M,PROPOSAL_TOTAL ");
            sb.Append(" ,PROPOSAL_GRADE,PROPOSAL_GRADE_MEAN,PROPOSAL_6,END_PLANT_CD,END_DEPT_NO,END_DEPT_NAME ");
            sb.Append(" ,ORI_PLANT_CD,ORI_DEPT_NO,ORI_DEPT_NAME,START_DT,PLAN_END_DT,DEPT_NO_TY ");
            sb.Append(" ,DEPT_NAME_TY,DEPT_NO_LY,DEPT_NAME_LY,DEPT_FLAG,LEVELUP_FLAG,ARREARS_FLAG ");
            sb.Append(" ,SCORE_LY,SCORE_DEPT,SCORE_FINAL,SCORE_FINAL_FLAG,SCORE_FLAG,APPROVE_MARK,CHG_STATUS ");
            sb.Append(" ,UNION_PJOB_CD,IS_OUT,SUGGEST_DESC,RECOMM_DESC,COMMENTS,DISTING_REMARK,LIMIT_RATE ");
            sb.Append(" ,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID ");
            sb.Append(" from TB_S_M_ASSESS_TARGET ");
            sb.Append(" where ASSESS_YEAR = @ASSESS_YEAR ");
            sb.Append("  and ASSESS_TYPE = @ASSESS_TYPE");
            //PK值
            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    #endregion

    

}