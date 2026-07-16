using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;
using System.Data.Odbc;

/// <summary>
/// CFB2SJ0200DAO 的摘要描述
/// </summary>
public class CFB2SJ0200DAO : BaseDAO
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
    public string MAIL_CHKDT { get; set; }
    public string DEADLINE { get; set; }

    //考核人事資料維護檔
    public string EMP_ID { get; set; }
    public string SCORE_FINAL { get; set; }
    public string SCORE_DEPT { get; set; }
    public string PROPOSAL_TOTAL { get; set; }
    public string PROPOSAL_GRADE { get; set; }
    public string PROPOSAL_GRADE_MEAN { get; set; }
    public string PROPOSAL_6 { get; set; }

    //考核表列印
    public string DEPT_NO { get; set; }
    public string DEPT_NO_20 { get; set; }
    public string DEPT_NAME_20 { get; set; }
    public string DEPT_NAME_30 { get; set; }
    public string DEPT_NO_40 { get; set; }
    public string WS_CD { get; set; }
    public string LEVEL_CD { get; set; }
    public string PLANT_CD { get; set; }
    public string PJOB_CD { get; set; }

    //長官的資訊
    public string HEAD_EMP_ID { get; set; }
    public string HEAD_EMP_NAME { get; set; }
    public string HEAD_DEPT_FULL_NAME { get; set; }

    public string resultCount { get; set; }
    public string year_title { get; set; }
    public string year_1_title { get; set; }    //前1年
    public string year_2_title { get; set; }    //前2年
    public string deptName_title { get; set; }  //部門
    public string levelCD_title { get; set; }   //資格
    public string levelCD_range_title { get; set; }   //資格區間
    public string plantCD_title { get; set; }   //(廠別)
    public string dept_level { get; set; }  //部門層級
    public string dept_level_name { get; set; }  //部門層級說明

    //AS400需要
    public string ASSESS_YM_S { get; set; }
    public string ASSESS_YM_E { get; set; }

    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }

    //考績主檔
    public string SCORE_1H { get; set; }
    public string LEVEL_FLAG_1H { get; set; }
    public string DEPT_FLAG_1H { get; set; }
    public string DEPT_NO_1H { get; set; }
    public string DEPT_NAME_1H { get; set; }
    public string SCORE_2H { get; set; }
    public string LEVEL_FLAG_2H { get; set; }
    public string DEPT_FLAG_2H { get; set; }
    public string DEPT_NO_2H { get; set; }
    public string DEPT_NAME_2H { get; set; }


    public CFB2SJ0200DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }


    #region 資料取得
    //AS400  B1CLIB.DB1CMB10EMP(提案用個人檔)
    public DataTable getCMB10EMP()
    {
        try
        {
            string sub_Str = " ";

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
			sb.Append(@" select "
						+ " EMP_ID,count(BONUS_SCR_FINAL) as total, sum(BONUS_SCR_FINAL) as grade "
						+ "  ,case when count(BONUS_SCR_FINAL)=0 then 0 else (sum(BONUS_SCR_FINAL) / count(BONUS_SCR_FINAL) ) end as mean "
						+ "  , ( select count(PRO_BONUS) from TB_P_M_PROPOSAL_DATA "
						+ "  where YM>= " + ASSESS_YM_S + " "
						+ " and YM<= " + ASSESS_YM_E + "  and EMP_ID=a.EMP_ID and  PRO_BONUS>=200 ) as LTotal "
						+ "  from TB_P_M_PROPOSAL_DATA  a "
						+ "  where YM>=" + ASSESS_YM_S + " and YM<= " + ASSESS_YM_E + " "
						+ "  group by EMP_ID order by EMP_ID");
			return dbConn.Query(sb, ht);
			
			/*
            sb.Append(@" select "
                      + " FB3WNO,count(FB3TID) as total, sum(FB3TSC) as grade  "
                      + " ,case when count(FB3TID)=0 then 0 else (sum(FB3TSC) / count(FB3TID) ) end as mean "
                      + " ,( select count(FB3RWD) from [B1CLIB].[Q1A_DATABASE_SRVR].B1CLIB.DB1CMB30   where FB3TYM>=" + ASSESS_YM_S + "  and FB3TYM<=" + ASSESS_YM_E + " and FB3WNO=a.FB3WNO and  FB3RWD>=200 ) as LTotal "
                      + " from [B1CLIB].[Q1A_DATABASE_SRVR].B1CLIB.DB1CMB30  a   "
                      + " where FB3TYM>=" + ASSESS_YM_S + "  and FB3TYM<=" + ASSESS_YM_E + " "
                      + " group by FB3WNO order by FB3WNO ");
            return dbConn.Query(sb, ht);
			*/
            /*
            DBConnector_ODBC odbc = new DBConnector_ODBC(utilities.ODBCconnstr);
            //AS400
            OdbcCommand ocomm = new OdbcCommand();
            ocomm.CommandText += " select  ";
            ocomm.CommandText += " FB1WNO,sum(FB1TIC) as total, sum(FB1TSC) as grade    ";
            ocomm.CommandText += " ,case when sum(FB1TIC)=0 then 0 else (sum(FB1TSC) / sum(FB1TIC) ) end as mean     ";
            ocomm.CommandText += ",( sum(FB1L6)+sum(FB1L7)+sum(FB1L8)+sum(FB1L9)+sum(FB1L10) ) as LTotal    ";

            ocomm.CommandText += "  from B1CLIB.DB1CMB10 ";

            ocomm.CommandText += "  where FB1YM>= " + ASSESS_YM_S + " and FB1YM<=" + ASSESS_YM_E + " ";
            ocomm.CommandText += "  group by FB1WNO ";

            //ocomm.Parameters.AddWithValue("", ASSESS_YM_S);
            //ocomm.Parameters.AddWithValue("", ASSESS_YM_E);

            DataTable tmp = odbc.getDataTable(ocomm);
            return tmp;
            */


        }
        catch (Exception)
        {
            throw;
        }
    }
    //AS400  B1CLIB.DB1CMB10(提案用個人檔)
    public DataTable getCMB10()
    {
        try
        {
            DBConnector_ODBC odbc = new DBConnector_ODBC(utilities.ODBCconnstr);

            //AS400
            OdbcCommand ocomm = new OdbcCommand();
            ocomm.CommandText += " select  ";
            ocomm.CommandText += " sum(FB1TIC) as total, sum(FB1TSC) as grade    ";
            ocomm.CommandText += " ,case when sum(FB1TIC)=0 then 0 else (sum(FB1TSC) / sum(FB1TIC) ) end as mean     ";
            ocomm.CommandText += ",( sum(FB1L6)+sum(FB1L7)+sum(FB1L8)+sum(FB1L9)+sum(FB1L10) ) as LTotal    ";
            ocomm.CommandText += "  from B1CLIB.DB1CMB10 ";
            ocomm.CommandText += "  where FB1WNO=? and FB1YM>=? and FB1YM<=? ";

            ocomm.Parameters.AddWithValue("", EMP_ID);
            ocomm.Parameters.AddWithValue("", ASSESS_YM_S);
            ocomm.Parameters.AddWithValue("", ASSESS_YM_E);

            DataTable tmp = odbc.getDataTable(ocomm);

            return tmp;

        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得 考核人事資料維護檔 員工資料
    internal DataTable getAssessEmpData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select a.* ");
            sb.Append(" from TB_S_M_ASSESS_TARGET a ");
            sb.Append(" where ASSESS_YEAR = @ASSESS_YEAR ");
            sb.Append(" and ASSESS_TYPE = @ASSESS_TYPE ");
            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
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
            sb.Append("select count(0) resultCount from TB_S_M_ASSESS_DATA");
            sb.Append(" where ASSESS_YEAR=@ASSESS_YEAR");
            sb.Append(" and ASSESS_TYPE = @ASSESS_TYPE");
            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

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


    //取得執行 年獎對象生成SP的錯誤訊息
    internal DataTable getSPLOG(string proc_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select proc_status, proc_log from TB_H_R_SP_LOG  ");
            sb.Append(" where PROC_ID= @PROC_ID ");
            sb.Append(" and PROC_DT=(select max(PROC_DT)  maxb from TB_H_R_SP_LOG ) ");
            ht.Add("@PROC_ID", proc_id);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }


    //取得長官是否已核可或駁回
    internal DataTable getIsApproveOrReject()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select count(0) resultCount from TB_S_M_ASSESS_DATA");
            sb.Append(" where ASSESS_YEAR=@ASSESS_YEAR");
            sb.Append(" and ASSESS_TYPE = @ASSESS_TYPE");
            sb.Append(" and APPROVE_STATUS in ('B','Y') ");

            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
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
            sb.Append(" a.*   ");
            sb.Append(" , a.ASSESS_TYPE + '-' + b.SUB_DESC ASSESS_TYPE_DESC   ");
            sb.Append(" from TB_S_M_ASSESS_DATA a with (nolock) ");
            sb.Append("  left join TB_9_M_COMM_D b on  a.ASSESS_TYPE = b.SUB_CD and b.MAIN_CD = 'ASSESS_TYPE'  and b.IS_VALID='Y'  and b.SYS_CD='SJ'  ");
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
                        , string assess_year_s, string assess_year_e, string assess_type
                       )
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
                                , string assess_year, string assess_type
                                , string emp_id, string emp_name, string level_cd, string ws_cd, string dept_no, string dept_name
                                , string head_emp_id, string assess_score
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
            sb.Append(" , a.DEPT_NAME_20 +' '+a.DEPT_NAME_30+' '+ a.DEPT_NAME_40  DEPT_NAME_DESC  ");
            sb.Append(" , a.WS_CD + '-' + b.SUB_DESC WS_CD_DESC   ");
            sb.Append(" from TB_S_M_ASSESS_TARGET a ");
            sb.Append("  left join TB_9_M_COMM_D b on  a.WS_CD = b.SUB_CD and b.MAIN_CD = 'WS_CD' and b.IS_VALID='Y'  and b.SYS_CD='HB'   ");
            sb.Append(" where 1=1 ");

            //查詢條件
            sb.Append(" and a.ASSESS_YEAR = @ASSESS_YEAR ");
            sb.Append(" and a.ASSESS_TYPE = @ASSESS_TYPE ");
            ht.Add("@ASSESS_YEAR", assess_year);
            ht.Add("@ASSESS_TYPE", assess_type);
            //直屬主管工號
            if (head_emp_id != "")
            {
                sb.Append(" and a.EMP_ID in ( select MNG_EMP_ID from TB_S_M_ASSESS_HEAD_EMP where EMP_ID=@HEAD_EMP_ID and ASSESS_YEAR=@ASSESS_YEAR and ASSESS_TYPE=@ASSESS_TYPE ) ");
                ht.Add("@HEAD_EMP_ID", head_emp_id);
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
            if (ws_cd != "-1")
            {
                sb.Append(" and a.WS_CD = @WS_CD ");
                ht.Add("@WS_CD", ws_cd);
            }
            if (dept_no != "")
            {
                sb.Append(" and a.DEPT_NO like @DEPT_NO ");
                ht.Add("@DEPT_NO", dept_no + "%");
            }
            if (dept_name != "")
            {
                sb.Append(" and a.DEPT_NAME like @DEPT_NAME ");
                ht.Add("@DEPT_NAME", dept_name + "%");
            }
            if (assess_score != "-1")
            {
                sb.Append(" and a.SCORE_FINAL = @SCORE_FINAL ");
                ht.Add("@SCORE_FINAL", assess_score);
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
                            , string assess_year, string assess_type
                            , string emp_id, string emp_name, string level_cd, string ws_cd, string dept_no, string dept_name
                            , string head_emp_id, string assess_score
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
            sb.Append(" and a.ASSESS_YEAR = @ASSESS_YEAR ");
            sb.Append(" and a.ASSESS_TYPE = @ASSESS_TYPE ");
            ht.Add("@ASSESS_YEAR", assess_year);
            ht.Add("@ASSESS_TYPE", assess_type);
            //直屬主管工號
            if (head_emp_id != "")
            {
                sb.Append(" and a.EMP_ID in ( select MNG_EMP_ID from TB_S_M_ASSESS_HEAD_EMP where EMP_ID=@HEAD_EMP_ID and ASSESS_YEAR=@ASSESS_YEAR and ASSESS_TYPE=@ASSESS_TYPE ) ");
                ht.Add("@HEAD_EMP_ID", head_emp_id);
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
                sb.Append(" and LEVEL_CD like @LEVEL_CD ");
                ht.Add("@LEVEL_CD", level_cd + "%");
            }
            if (ws_cd != "-1")
            {
                sb.Append(" and WS_CD = @WS_CD ");
                ht.Add("@WS_CD", ws_cd);
            }
            if (dept_no != "")
            {
                sb.Append(" and DEPT_NO like @DEPT_NO ");
                ht.Add("@DEPT_NO", dept_no + "%");
            }
            if (dept_name != "")
            {
                sb.Append(" and DEPT_NAME like @DEPT_NAME ");
                ht.Add("@DEPT_NAME", dept_name + "%");
            }
            if (assess_score != "-1")
            {
                sb.Append(" and a.SCORE_FINAL = @SCORE_FINAL ");
                ht.Add("@SCORE_FINAL", assess_score);
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

    //刪除_考核資料 
    public void deleteData_H(string assess_year, string assess_type)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("delete from TB_S_M_ASSESS_DATA ");
            sb.Append(" where ASSESS_YEAR = @ASSESS_YEAR and ASSESS_TYPE = @ASSESS_TYPE ");
            ht.Add("@ASSESS_YEAR", assess_year);
            ht.Add("@ASSESS_TYPE", assess_type);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //刪除_考核人事資料
    public void deleteData_D(string assess_year, string assess_type, string tableName)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("delete from  " + tableName + " ");
            sb.Append(" where ASSESS_YEAR = @ASSESS_YEAR and ASSESS_TYPE = @ASSESS_TYPE ");
            ht.Add("@ASSESS_YEAR", assess_year);
            ht.Add("@ASSESS_TYPE", assess_type);
            //ht.Add("@START_DT", start_dt);
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
            sb.Append(" INSERT INTO TB_S_M_ASSESS_DATA ");
            sb.Append(" ( ");
            sb.Append(" ASSESS_YEAR, ASSESS_TYPE, FREEZE_FLAG, MAIL_CHKDT, DEADLINE  ");
            sb.Append(" ,CREATED_BY, CREATED_DT, UPDATED_BY, UPDATED_DT, FUNC_ID  ");
            sb.Append(" ) ");
            sb.Append(" VALUES ");
            sb.Append(" ( @ASSESS_YEAR,  @ASSESS_TYPE, @FREEZE_FLAG, @MAIL_CHKDT, @DEADLINE  ");
            sb.Append("   ,@CREATED_BY,  @CREATED_DT,  @UPDATED_BY,  @UPDATED_DT,  @FUNC_ID )");

            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@FREEZE_FLAG", "N");
            ht.Add("@MAIL_CHKDT", MAIL_CHKDT);
            ht.Add("@DEADLINE", DEADLINE);

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

    //更新提案資料
    internal void updateMeanData(DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_S_M_ASSESS_TARGET ");
            sb.Append(" set PROPOSAL_GRADE_MEAN =  ");
            sb.Append(" case when PROPOSAL_TOTAL=0 ");
            sb.Append(" then 0 ");
            sb.Append(" else round(PROPOSAL_GRADE/PROPOSAL_TOTAL,2) end");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT = @UPDATED_DT");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where ASSESS_YEAR = @ASSESS_YEAR ");
            sb.Append("  and ASSESS_TYPE = @ASSESS_TYPE");

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

    //更新提案資料
    internal void updateData(DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_S_M_ASSESS_TARGET ");
            sb.Append(" set PROPOSAL_TOTAL = @PROPOSAL_TOTAL ");
            sb.Append(" ,PROPOSAL_GRADE = @PROPOSAL_GRADE");
            sb.Append(" ,PROPOSAL_GRADE_MEAN = @PROPOSAL_GRADE_MEAN");
            sb.Append(" ,PROPOSAL_6 = @PROPOSAL_6");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT = @UPDATED_DT");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where ASSESS_YEAR = @ASSESS_YEAR ");
            sb.Append("  and ASSESS_TYPE = @ASSESS_TYPE");
            sb.Append("  and EMP_ID = @EMP_ID");

            //SET值
            ht.Add("@PROPOSAL_TOTAL", PROPOSAL_TOTAL);
            ht.Add("@PROPOSAL_GRADE", PROPOSAL_GRADE);
            ht.Add("@PROPOSAL_GRADE_MEAN", PROPOSAL_GRADE_MEAN);
            ht.Add("@PROPOSAL_6", PROPOSAL_6);

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

    //提出核可
    public void updateRelease()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            DateTime now = DateTime.Parse(DateTime.Now.ToString());
            sb.Append(" update TB_S_M_ASSESS_DATA ");
            sb.Append(" set RELEASE_DT = @RELEASE_DT ");
            sb.Append(" ,RELEASE_BY = @RELEASE_BY ");
            sb.Append(" ,APPROVE_DT = @APPROVE_DT ");
            sb.Append(" ,APPROVE_BY = @APPROVE_BY ");
            //sb.Append(" ,APPROVE_STATUS = @APPROVE_STATUS ");
            sb.Append(" ,FREEZE_FLAG = @FREEZE_FLAG ");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT = @UPDATED_DT");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where ASSESS_YEAR = @ASSESS_YEAR ");
            sb.Append("  and ASSESS_TYPE = @ASSESS_TYPE");

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
            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //刪除 同年度考績主檔資料
    public void delete_M_ASSESS()
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();

        sb.Append(" delete from TB_S_M_ASSESS ");
        sb.Append(" where ASSESS_YEAR = @ASSESS_YEAR ");
        ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
        dbConn.ExecuteT(sb, ht, true);
    }
    //以年度和類別取得考核發佈資料
    public DataTable get_R_ASSESS()
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();

        sb.Append(" select EMP_ID, SCORE_FINAL, DEPT_FLAG, LEVELUP_FLAG, DEPT_NO, DEPT_NAME ");
        sb.Append("   from TB_S_R_ASSESS_TARGET ");
        sb.Append("  where ASSESS_YEAR = @ASSESS_YEAR ");
        sb.Append("    and ASSESS_TYPE = @ASSESS_TYPE ");
        ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
        ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
        return dbConn.QueryT(sb, ht, true);
    }

    public void insert_M_Assess1()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" INSERT INTO TB_S_M_ASSESS ");
            sb.Append(" VALUES ");
            sb.Append(" ( @EMP_ID, @ASSESS_YEAR,  @SCORE_1H, @LEVEL_FLAG_1H, @DEPT_FLAG_1H, @DEPT_NO_1H, @DEPT_NAME_1H ");
            sb.Append("  ,@SCORE_2H, @LEVEL_FLAG_2H, @DEPT_FLAG_2H, @DEPT_NO_2H, @DEPT_NAME_2H ");
            sb.Append("  ,@CREATED_BY, GETDATE(), @UPDATED_BY, GETDATE(), @FUNC_ID )");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@SCORE_1H", SCORE_1H);
            ht.Add("@LEVEL_FLAG_1H", LEVEL_FLAG_1H);
            ht.Add("@DEPT_FLAG_1H", DEPT_FLAG_1H);
            ht.Add("@DEPT_NO_1H", DEPT_NO_1H);
            ht.Add("@DEPT_NAME_1H", DEPT_NAME_1H);

            ht.Add("@SCORE_2H", "");
            ht.Add("@LEVEL_FLAG_2H", "");
            ht.Add("@DEPT_FLAG_2H", "");
            ht.Add("@DEPT_NO_2H", "");
            ht.Add("@DEPT_NAME_2H", "");

            ht.Add("@CREATED_BY", UPDATED_BY);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void insert_M_Assess2()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" INSERT INTO TB_S_M_ASSESS ");
            sb.Append(" VALUES ");
            sb.Append(" ( @EMP_ID, @ASSESS_YEAR,  @SCORE_1H, @LEVEL_FLAG_1H, @DEPT_FLAG_1H, @DEPT_NO_1H, @DEPT_NAME_1H ");
            sb.Append("  ,@SCORE_2H, @LEVEL_FLAG_2H, @DEPT_FLAG_2H, @DEPT_NO_2H, @DEPT_NAME_2H ");
            sb.Append("  ,@CREATED_BY, GETDATE(), @UPDATED_BY, GETDATE(), @FUNC_ID )");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@SCORE_1H", "");
            ht.Add("@LEVEL_FLAG_1H", "");
            ht.Add("@DEPT_FLAG_1H", "");
            ht.Add("@DEPT_NO_1H", "");
            ht.Add("@DEPT_NAME_1H", "");

            ht.Add("@SCORE_2H", SCORE_2H);
            ht.Add("@LEVEL_FLAG_2H", LEVEL_FLAG_2H);
            ht.Add("@DEPT_FLAG_2H", DEPT_FLAG_2H);
            ht.Add("@DEPT_NO_2H", DEPT_NO_2H);
            ht.Add("@DEPT_NAME_2H", DEPT_NAME_2H);

            ht.Add("@CREATED_BY", UPDATED_BY);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void update_M_Assess()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" update TB_S_M_ASSESS ");
            sb.Append(" set SCORE_2H = @SCORE_2H ");
            sb.Append(" ,LEVEL_FLAG_2H = @LEVEL_FLAG_2H ");
            sb.Append(" ,DEPT_FLAG_2H = @DEPT_FLAG_2H ");
            sb.Append(" ,DEPT_NO_2H = @DEPT_NO_2H ");
            sb.Append(" ,DEPT_NAME_2H = @DEPT_NAME_2H ");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT = GETDATE()");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where ASSESS_YEAR = @ASSESS_YEAR ");
            sb.Append("  and EMP_ID = @EMP_ID");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);

            ht.Add("@SCORE_2H", SCORE_2H);
            ht.Add("@LEVEL_FLAG_2H", LEVEL_FLAG_2H);
            ht.Add("@DEPT_FLAG_2H", DEPT_FLAG_2H);
            ht.Add("@DEPT_NO_2H", DEPT_NO_2H);
            ht.Add("@DEPT_NAME_2H", DEPT_NAME_2H);

            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }
    /// <summary>
    /// 檢查相同工號、年度資料存不存在，存在回傳，不存在回傳false
    /// </summary>
    /// <returns>bool</returns>
    public bool isAssessExist()
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.Append(" select count(1) as total ");
        sb.Append("   from TB_S_M_ASSESS ");
        sb.Append("  where EMP_ID = @EMP_ID ");
        sb.Append("    and ASSESS_YEAR = @ASSESS_YEAR ");
        ht.Add("@EMP_ID", EMP_ID);
        ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
        DataTable dt = dbConn.QueryT(sb, ht, true);
        if (Convert.ToInt32(dt.Rows[0]["total"]) > 0)
            return true;
        else
            return false;
    }

    //考核發佈
    public void updateAnnounce(DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" update TB_S_M_ASSESS_DATA ");
            sb.Append(" set ASSESS_RELEASE_DT = @ASSESS_RELEASE_DT ");
            sb.Append(" ,ASSESS_RELEASE_BY = @ASSESS_RELEASE_BY ");
            sb.Append(" ,FREEZE_FLAG = @FREEZE_FLAG ");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT = @UPDATED_DT");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where ASSESS_YEAR = @ASSESS_YEAR ");
            sb.Append("  and ASSESS_TYPE = @ASSESS_TYPE");

            ht.Add("@ASSESS_RELEASE_DT", ASSESS_RELEASE_DT);
            ht.Add("@ASSESS_RELEASE_BY", ASSESS_RELEASE_BY);
            ht.Add("@FREEZE_FLAG", FREEZE_FLAG);

            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@UPDATED_DT", now);
            ht.Add("@FUNC_ID", FUNC_ID);

            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }


    //考績一括更新
    public void updateAssessScore_ALL(DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" update TB_S_M_ASSESS_TARGET ");
            sb.Append(" set SCORE_DEPT =  @SCORE_DEPT  ");
            sb.Append(" ,SCORE_FINAL = @SCORE_FINAL");
            sb.Append(" ,SCORE_FLAG = @SCORE_FLAG");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT = @UPDATED_DT");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where ASSESS_YEAR = @ASSESS_YEAR ");
            sb.Append("  and ASSESS_TYPE = @ASSESS_TYPE");
            sb.Append("  and EMP_ID = @EMP_ID");

            //set值
            ht.Add("@SCORE_DEPT", SCORE_DEPT);
            ht.Add("@SCORE_FINAL", SCORE_FINAL);
            ht.Add("@SCORE_FLAG", 'V');
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

    //更新考核人事資料維護檔的異動狀態為V
    public void updateChgStatus_D(DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" update TB_S_M_ASSESS_TARGET ");
            sb.Append(" set CHG_STATUS =  @CHG_STATUS  ");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY ");
            sb.Append(" ,UPDATED_DT = @UPDATED_DT ");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where ASSESS_YEAR = @ASSESS_YEAR ");
            sb.Append("  and ASSESS_TYPE = @ASSESS_TYPE ");
            sb.Append("  and EMP_ID = @EMP_ID");

            //set值
            ht.Add("@CHG_STATUS", 'V');
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


    //最終考績一括更新
    public void updateAssessScore_Final(DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" update TB_S_M_ASSESS_TARGET ");
            sb.Append(" set ");
            sb.Append("  SCORE_FINAL = @SCORE_FINAL");
            sb.Append(" ,SCORE_FLAG = @SCORE_FLAG");
            sb.Append(" ,SCORE_FINAL_FLAG =   Case when SCORE_DEPT <>  @SCORE_FINAL then 'V'  ELSE '' END ");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT = @UPDATED_DT");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where ASSESS_YEAR = @ASSESS_YEAR ");
            sb.Append("  and ASSESS_TYPE = @ASSESS_TYPE");
            sb.Append("  and EMP_ID = @EMP_ID");

            //set值
            ht.Add("@SCORE_FINAL", SCORE_FINAL);
            ht.Add("@SCORE_FLAG", 'V');

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


    //一括更新時-回復成未核可前狀態  (考核資料維護檔 DTL)
    public void updateRejectData_H(DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_S_M_ASSESS_DATA ");
            //sb.Append(" set REMARK = @REMARK ");
            sb.Append(" set RELEASE_DT = @RELEASE_DT");
            sb.Append(" ,RELEASE_BY = @RELEASE_BY");
            sb.Append(" ,APPROVE_DT = @APPROVE_DT");
            sb.Append(" ,APPROVE_BY = @APPROVE_BY");
            sb.Append(" ,APPROVE_STATUS =  ");
            sb.Append(" case when APPROVE_STATUS ='N' then 'N' else 'B' end ");
            sb.Append(" ,FREEZE_FLAG = @FREEZE_FLAG");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT = @UPDATED_DT");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where ASSESS_YEAR = @ASSESS_YEAR ");
            sb.Append("  and ASSESS_TYPE = @ASSESS_TYPE");
            //set值
            //ht.Add("@REMARK", REMARK);
            ht.Add("@RELEASE_DT", DBNull.Value);
            ht.Add("@RELEASE_BY", "");
            ht.Add("@APPROVE_DT", DBNull.Value);
            ht.Add("@APPROVE_BY", "");
            //ht.Add("@APPROVE_STATUS", "N");//N:未核
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

    //呼叫考核對象生成SP
    internal void execSP_S_ASSESS_DATA()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_S_ASSESS_DATA");
            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@USERID", CREATED_BY);//CREATED_BY
            ht.Add("@FUNCID", "FB2SJ020");
            dbConn.ExecuteSPT(sb, ht, true);

        }
        catch (Exception)
        {

            throw;
        }

    }
    //呼叫考核對象生成SP
    internal void execSP_S_ASSESS_L2_DATA()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_S_ASSESS_GEN_L2_DATA");
            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@USERID", CREATED_BY);//CREATED_BY
            ht.Add("@FUNCID", "FB2SJ020");
            dbConn.ExecuteSPT(sb, ht, true);

        }
        catch (Exception)
        {

            throw;
        }

    }
    //呼叫考核表通知部長部門未結通知作業SP
    internal void execSP_S_ASSESS_DEP20_MAIL_CHKDT_MAIL()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_S_ASSESS_DEP20_MAIL_CHKDT_MAIL");
            dbConn.ExecuteSPT(sb, ht, true);

        }
        catch (Exception)
        {

            throw;
        }

    }
    //呼叫考績一括維護SP
    internal void execSP_S_ASSESS_UPDATE_SCORE()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_S_ASSESS_UPDATE_SCORE");
            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@USERID", CREATED_BY);//CREATED_BY
            ht.Add("@FUNCID", "FB2SJ020");
            dbConn.ExecuteSPT(sb, ht, true);

        }
        catch (Exception)
        {

            throw;
        }

    }
    #endregion

    #region 取得 EXCEL 資料

    //EXCEL下載資料(SJ010)
    public DataTable getExcelResultData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select ");
            sb.Append(" a.*   ");
            sb.Append(" , SCORE_FINAL_DESC =  case when a.LEVELUP_FLAG='V' and SCORE_FINAL='D'  then a.SCORE_FINAL+'*' ELSE a.SCORE_FINAL  end   ");
            sb.Append(" , SCORE_DEPT_DESC = case   when   LEVELUP_FLAG='V' and SCORE_DEPT='D'   then SCORE_DEPT+'*' ELSE SCORE_DEPT  end    ");
            sb.Append(" ,  a.DEPT_NAME_20 +' '+a.DEPT_NAME_30+' '+ a.DEPT_NAME_40  DEPT_NAME_DESC ");
            sb.Append(" ,                        b.SUB_DESC ASSESS_TYPE_DESC   ");
            sb.Append(" , a.WS_CD + '-'       + c.SUB_DESC WS_CD_DESC   ");
            sb.Append(" from TB_S_M_ASSESS_TARGET a  with (nolock) ");
            sb.Append("  left join TB_9_M_COMM_D b  with (nolock) on  a.ASSESS_TYPE = b.SUB_CD and b.MAIN_CD = 'ASSESS_TYPE'  and b.IS_VALID='Y'  and b.SYS_CD='SJ'  ");
            sb.Append("  left join TB_9_M_COMM_D c  with (nolock) on  a.WS_CD = c.SUB_CD       and c.MAIN_CD = 'WS_CD'        and c.IS_VALID='Y'  and c.SYS_CD='HB' ");
            sb.Append(" where 1=1 ");

            if (ASSESS_YEAR != "")
            {
                sb.Append(" and ASSESS_YEAR = @ASSESS_YEAR ");
                ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            }
            if (ASSESS_TYPE != "-1")
            {
                sb.Append(" and ASSESS_TYPE = @ASSESS_TYPE ");
                ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            }
            sb.Append(" order by   ");
            sb.Append("  c.ORDER_SEQ ASC, a.DEPT_NO ASC, a.LEVEL_ORDER_SEQ ASC, a.SCORE_FINAL  ASC    ");

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    #endregion

    #region 考核表列印(新)

    //找出各部級部門代號
    public DataTable getDeptNO_NEW(string deptNO20 = "")
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            //sb.Append(" select distinct PLANT_CD, DEPT_NO_20  ");
            //因為部門代號觀音及中壢不會有相關的部級部門代號，故不用工廠區分
            sb.Append(" select  DEPT_NO_20,DEPT_NAME_20  ");
            sb.Append(" from TB_S_M_ASSESS_TARGET   ");
            sb.Append(" where 1=1 ");
            sb.Append(" and ASSESS_YEAR = @ASSESS_YEAR ");
            sb.Append(" and ASSESS_TYPE = @ASSESS_TYPE ");
            if (deptNO20 != "")
            {
                sb.Append("  and DEPT_NO_20 =@DEPT_NO_20 ");//測試某個部門時
                ht.Add("@DEPT_NO_20", deptNO20);
            }
            sb.Append(" group by DEPT_NO_20,DEPT_NAME_20 ");
            sb.Append(" order by  DEPT_NO_20 ");

            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    //取得部門層級長官的SQL
    public string getDept_Level_Script(string mng_ws_cd)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.Append(@" select EMP_ID,EMP_NAME,DEPT_NO,DEPT_NO_20,DEPT_FULL_NAME,DEPT_LEVEL,count(*) as resultCount, CEILING(convert(decimal(5,2),count(*))/10) as pageCount 
                     from TB_S_M_ASSESS_HEAD_EMP
                     where ASSESS_YEAR = @ASSESS_YEAR and  ASSESS_TYPE=@ASSESS_TYPE
                     and MNG_DEPT_NO_20=@MNG_DEPT_NO_20	and DEPT_LEVEL=@DEPT_LEVEL	 
                    ");
        if (mng_ws_cd == "S")
        {
            sb.Append(@" and MNG_WS_CD in ('S','G','T','N') ");
        }
        else
        {
            sb.Append(@" and MNG_WS_CD in ('W') ");
        }
        sb.Append(@"
            group by emp_ID,EMP_NAME,DEPT_NO,DEPT_NO_20,DEPT_FULL_NAME,DEPT_LEVEL
            ");
        return sb.ToString();
    }

    //取得 各部門層級的頁數 
    public int getDept_Level_Total(string mng_ws_cd)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select isnull(sum(pageCount),0) resultCount from  (    ");
            sb.Append(getDept_Level_Script(mng_ws_cd));
            sb.Append(" ) z ");

            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);

            ht.Add("@MNG_DEPT_NO_20", DEPT_NO_20);
            ht.Add("@DEPT_LEVEL", dept_level);

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                t = Convert.ToInt32(dt.Rows[0]["resultCount"].ToString());
            }
            return t;
        }
        catch
        {
            throw;
        }
    }

    //取得 各部門層級的資料
    public DataTable getDept_Level_Data(string mng_ws_cd)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(getDept_Level_Script(mng_ws_cd));
            sb.Append(" order by DEPT_LEVEL,DEPT_NO ");

            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@MNG_DEPT_NO_20", DEPT_NO_20);
            ht.Add("@DEPT_LEVEL", dept_level);

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    //取得 要新增至EXCEL的資料
    public DataTable getExport_Data(string mng_ws_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" select isnull(R.REMARK,'') as REMARK , T.* 
                        ,convert(decimal(4,1),RECENT_LEVEL_WORK_YEARS) RECENT_LEVEL_WORK_YEARS_DESC
                        , convert(decimal(4,1),WORK_YEARS) WORK_YEARS_DESC
                        from TB_S_M_ASSESS_TARGET T
                        left join  TB_S_M_ASSESS_REMARK R on T.ASSESS_YEAR=R.ASSESS_YEAR  and T.ASSESS_TYPE=R.ASSESS_TYPE and T.EMP_ID = R.EMP_ID 
                        where 1=1 
                        and T.ASSESS_YEAR = @ASSESS_YEAR 
                        and T.ASSESS_TYPE = @ASSESS_TYPE
                        and T.EMP_ID in ( select MNG_EMP_ID from  TB_S_M_ASSESS_HEAD_EMP where EMP_ID = @HEAD_EMP_ID and ASSESS_YEAR = @ASSESS_YEAR and  ASSESS_TYPE=@ASSESS_TYPE)
                        and T.DEPT_NO_20 = @DEPT_NO_20
                         ");
            if (mng_ws_cd == "S")
            {
                sb.Append(@" and WS_CD in ('S','G','T','N') ");
            }
            else
            {
                sb.Append(@" and WS_CD in ('W') ");
            }
            sb.Append(@" order by DEPT_NO, LEVEL_CD,WS_CD,EMP_ID ");

            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@HEAD_EMP_ID", HEAD_EMP_ID);
            ht.Add("@DEPT_NO_20", DEPT_NO_20);

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    #endregion

    #region 考核表列印(舊)
    //找出各工廠的部級部門代號
    public DataTable getDeptNO(string deptNO20 = "")
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            //sb.Append(" select distinct PLANT_CD, DEPT_NO_20  ");
            //因為部門代號觀音及中壢不會有相關的部級部門代號，故不用工廠區分
            sb.Append(" select  DEPT_NO_20,DEPT_NAME_20  ");
            sb.Append(" from TB_S_M_ASSESS_TARGET   ");
            sb.Append(" where 1=1 ");
            sb.Append(" and ASSESS_YEAR = @ASSESS_YEAR ");
            sb.Append(" and ASSESS_TYPE = @ASSESS_TYPE ");
            if (deptNO20 != "")
            {
                sb.Append("  and DEPT_NO_20 =@DEPT_NO_20 ");//測試某個部門時
                ht.Add("@DEPT_NO_20", deptNO20);
            }
            sb.Append(" group by DEPT_NO_20,DEPT_NAME_20 ");
            sb.Append(" order by  DEPT_NO_20 ");
            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    //取得 事務系及資格為(3A~3B) 的分組資料(數量)
    public DataTable getDeptStaff_3A3B()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select count(0) as resultcount, WS_CD,LEVEL_CD,PLANT_CD, DEPT_NO_20,DEPT_NAME_30    ");
            sb.Append(" from TB_S_M_ASSESS_TARGET  ");
            sb.Append(" where 1=1 ");
            sb.Append(" and ASSESS_YEAR = @ASSESS_YEAR ");
            sb.Append(" and ASSESS_TYPE = @ASSESS_TYPE ");
            sb.Append(" and LEVEL_CD in ('3A','3B') ");
            sb.Append(" and WS_CD = @WS_CD ");
            sb.Append(" and DEPT_NO_20 = @DEPT_NO_20 ");
            sb.Append(" group by WS_CD,LEVEL_CD,PLANT_CD, DEPT_NO_20,DEPT_NAME_30 ");
            sb.Append(" order by PLANT_CD, DEPT_NO_20, LEVEL_CD ");

            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@WS_CD", "S");
            ht.Add("@DEPT_NO_20", DEPT_NO_20);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    //取得 事務系及資格為(4A~5A) 的資料(數量)
    public DataTable getDeptStaff_4A5A()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();


            sb.Append(" select  count(0) as resultcount,PLANT_CD, DEPT_NO_20,WS_CD,DEPT_NO_40   ");
            sb.Append(" from TB_S_M_ASSESS_TARGET   ");
            sb.Append(" where 1=1 ");
            sb.Append(" and ASSESS_YEAR = @ASSESS_YEAR ");
            sb.Append(" and ASSESS_TYPE = @ASSESS_TYPE ");
            sb.Append(" and LEVEL_CD in ( '4A', '4B','5A') ");
            sb.Append(" and WS_CD = @WS_CD ");
            sb.Append(" and DEPT_NO_20 = @DEPT_NO_20 ");
            sb.Append(" group by PLANT_CD, DEPT_NO_20,WS_CD,DEPT_NO_40 ");
            sb.Append(" order by PLANT_CD, DEPT_NO_40 ");
            /*
            sb.Append(" select  count(0) as resultcount,PLANT_CD, DEPT_NO_20,WS_CD,LEVEL_CD,DEPT_NO_40   ");
            sb.Append(" group by PLANT_CD, DEPT_NO_20,WS_CD,LEVEL_CD,DEPT_NO_40 ");
            sb.Append(" order by PLANT_CD, DEPT_NO_40,LEVEL_CD ");
            */
            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@WS_CD", "S");
            ht.Add("@DEPT_NO_20", DEPT_NO_20);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    //取得 事務系及資格為(3A~3B) 的個人資料
    public DataTable getStaff_3A3B()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select *    ");
            sb.Append(" ,convert(decimal(4,1),RECENT_LEVEL_WORK_YEARS) RECENT_LEVEL_WORK_YEARS_DESC   ");
            sb.Append(" , convert(decimal(4,1),WORK_YEARS) WORK_YEARS_DESC   ");
            sb.Append(" from TB_S_M_ASSESS_TARGET  ");
            sb.Append(" where 1=1 ");
            sb.Append(" and ASSESS_YEAR = @ASSESS_YEAR ");
            sb.Append(" and ASSESS_TYPE = @ASSESS_TYPE ");
            sb.Append(" and LEVEL_CD = @LEVEL_CD ");
            sb.Append(" and WS_CD = @WS_CD ");
            sb.Append(" and DEPT_NO_20 = @DEPT_NO_20 ");
            sb.Append(" and DEPT_NAME_30 = @DEPT_NAME_30 ");
            sb.Append(" and PLANT_CD = @PLANT_CD ");
            sb.Append("  order by LEVELUP_FLAG DESC, RECENT_LEVEL_WORK_YEARS ASC, AGE ASC, WORK_YEARS ASC ");

            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@LEVEL_CD", LEVEL_CD);
            ht.Add("@WS_CD", "S");
            ht.Add("@DEPT_NO_20", DEPT_NO_20);
            ht.Add("@DEPT_NAME_30", DEPT_NAME_30);
            ht.Add("@PLANT_CD", PLANT_CD);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    //取得 事務系及資格為(4A~5A) 的個人資料
    public DataTable getStaff_4A5A()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select *    ");
            sb.Append(" ,convert(decimal(4,1),RECENT_LEVEL_WORK_YEARS) RECENT_LEVEL_WORK_YEARS_DESC   ");
            sb.Append(" , convert(decimal(4,1),WORK_YEARS) WORK_YEARS_DESC   ");
            sb.Append(" from TB_S_M_ASSESS_TARGET  ");
            sb.Append(" where 1=1 ");
            sb.Append(" and ASSESS_YEAR = @ASSESS_YEAR ");
            sb.Append(" and ASSESS_TYPE = @ASSESS_TYPE ");
            sb.Append(" and LEVEL_CD in ( '4A', '4B','5A') ");
            sb.Append(" and WS_CD = @WS_CD ");
            sb.Append(" and PLANT_CD = @PLANT_CD ");
            sb.Append(" and DEPT_NO_20 = @DEPT_NO_20 ");
            sb.Append(" and DEPT_NO_40 = @DEPT_NO_40 ");
            sb.Append("  order by LEVEL_ORDER_SEQ ASC,GRADE_CD DESC, LEVELUP_FLAG DESC, RECENT_LEVEL_WORK_YEARS DESC, AGE DESC, WORK_YEARS DESC ");

            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            //ht.Add("@LEVEL_CD", LEVEL_CD);
            ht.Add("@WS_CD", "S");
            ht.Add("@DEPT_NO_20", DEPT_NO_20);
            ht.Add("@DEPT_NO_40", DEPT_NO_40);
            ht.Add("@PLANT_CD", PLANT_CD);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }


    //取得 現場系(Worker)及資格為(3A~4B) 的分組資料(數量)
    public DataTable getDeptWorker_3A4B()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select  count(0) as resultcount,PLANT_CD, DEPT_NO_20,WS_CD,LEVEL_CD,DEPT_NO_40   ");
            sb.Append(" from TB_S_M_ASSESS_TARGET   ");
            sb.Append(" where 1=1 ");
            sb.Append(" and ASSESS_YEAR = @ASSESS_YEAR ");
            sb.Append(" and ASSESS_TYPE = @ASSESS_TYPE ");
            sb.Append(" and LEVEL_CD in ( '3A','3B','4A', '4B') ");
            sb.Append(" and WS_CD = @WS_CD ");
            sb.Append(" and DEPT_NO_20 = @DEPT_NO_20 ");
            sb.Append(" group by PLANT_CD, DEPT_NO_20,WS_CD,LEVEL_CD,DEPT_NO_40 ");
            sb.Append(" order by PLANT_CD,DEPT_NO_40,LEVEL_CD ");

            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@WS_CD", "W");
            ht.Add("@DEPT_NO_20", DEPT_NO_20);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    //取得 現場系(Worker)及資格為(5A) 的分組資料(數量)
    public DataTable getDeptWorker_5A()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select count(0) as resultcount, WS_CD,LEVEL_CD,PLANT_CD, DEPT_NO    ");
            sb.Append(" from TB_S_M_ASSESS_TARGET  ");
            sb.Append(" where 1=1 ");
            sb.Append(" and ASSESS_YEAR = @ASSESS_YEAR ");
            sb.Append(" and ASSESS_TYPE = @ASSESS_TYPE ");
            sb.Append(" and LEVEL_CD =@LEVEL_CD ");
            sb.Append(" and WS_CD = @WS_CD ");
            sb.Append(" and DEPT_NO_20 = @DEPT_NO_20 ");
            sb.Append(" group by WS_CD,LEVEL_CD,PLANT_CD, DEPT_NO ");
            sb.Append(" order by PLANT_CD,DEPT_NO,LEVEL_CD ");

            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@WS_CD", "W");
            ht.Add("@DEPT_NO_20", DEPT_NO_20);
            ht.Add("@LEVEL_CD", "5A");
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }



    //取得 現場系(T-特勤)的分組資料(數量)
    public DataTable getDeptWorker_T()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select count(0) as resultcount,WS_CD,PLANT_CD, DEPT_NO_20, DEPT_NO_40    ");
            sb.Append(" from TB_S_M_ASSESS_TARGET   ");
            sb.Append(" where 1=1 ");
            sb.Append(" and ASSESS_YEAR = @ASSESS_YEAR ");
            sb.Append(" and ASSESS_TYPE = @ASSESS_TYPE ");
            sb.Append(" and WS_CD = @WS_CD ");
            sb.Append(" and DEPT_NO_20 = @DEPT_NO_20 ");
            sb.Append(" group by WS_CD,PLANT_CD, DEPT_NO_20,DEPT_NO_40 ");
            sb.Append(" order by PLANT_CD,DEPT_NO_20,DEPT_NO_40 ");

            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@WS_CD", "T");
            ht.Add("@DEPT_NO_20", DEPT_NO_20);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    //取得 現場系(G-業務)的分組資料(數量)
    public DataTable getDeptWorker_G()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select count(0) as resultcount ,WS_CD,PLANT_CD, DEPT_NO_20, DEPT_NO_40     ");
            sb.Append(" from TB_S_M_ASSESS_TARGET   ");
            sb.Append(" where 1=1 ");
            sb.Append(" and ASSESS_YEAR = @ASSESS_YEAR ");
            sb.Append(" and ASSESS_TYPE = @ASSESS_TYPE ");
            sb.Append(" and WS_CD = @WS_CD ");
            sb.Append(" and DEPT_NO_20 = @DEPT_NO_20 ");
            sb.Append(" group by WS_CD,PLANT_CD, DEPT_NO_20,DEPT_NO_40 ");
            sb.Append(" order by PLANT_CD,DEPT_NO_20,DEPT_NO_40 ");

            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@WS_CD", "G");
            ht.Add("@DEPT_NO_20", DEPT_NO_20);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    //取得 現場系(T-特勤) 的個人資料
    public DataTable getWorker_T()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select *    ");
            sb.Append(" from TB_S_M_ASSESS_TARGET  ");
            sb.Append(" where 1=1 ");
            sb.Append(" and ASSESS_YEAR = @ASSESS_YEAR ");
            sb.Append(" and ASSESS_TYPE = @ASSESS_TYPE ");
            //sb.Append(" and LEVEL_CD = @LEVEL_CD ");
            sb.Append(" and WS_CD = @WS_CD ");
            sb.Append(" and DEPT_NO_20 = @DEPT_NO_20 ");
            sb.Append(" and DEPT_NO_40 = @DEPT_NO_40 ");
            //sb.Append(" and PJOB_CD = @PJOB_CD ");
            sb.Append(" and PLANT_CD = @PLANT_CD ");
            sb.Append("  order by LEVEL_ORDER_SEQ ASC,GRADE_CD DESC, LEVELUP_FLAG DESC, RECENT_LEVEL_WORK_YEARS ASC, AGE ASC, WORK_YEARS ASC ");

            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            //ht.Add("@LEVEL_CD", LEVEL_CD);
            ht.Add("@WS_CD", "T");
            ht.Add("@DEPT_NO_20", DEPT_NO_20);
            ht.Add("@DEPT_NO_40", DEPT_NO_40);
            //ht.Add("@PJOB_CD", PJOB_CD);
            ht.Add("@PLANT_CD", PLANT_CD);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }




    //取得 現場系(G-業務) 的個人資料
    public DataTable getWorker_G()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select *    ");
            sb.Append(" from TB_S_M_ASSESS_TARGET  ");
            sb.Append(" where 1=1 ");
            sb.Append(" and ASSESS_YEAR = @ASSESS_YEAR ");
            sb.Append(" and ASSESS_TYPE = @ASSESS_TYPE ");
            //sb.Append(" and LEVEL_CD = @LEVEL_CD ");
            sb.Append(" and WS_CD = @WS_CD ");
            sb.Append(" and DEPT_NO_20 = @DEPT_NO_20 ");
            sb.Append(" and DEPT_NO_40 = @DEPT_NO_40 ");
            sb.Append(" and PLANT_CD = @PLANT_CD ");
            sb.Append("  order by LEVEL_ORDER_SEQ ASC,GRADE_CD DESC, LEVELUP_FLAG DESC, RECENT_LEVEL_WORK_YEARS DESC, AGE DESC, WORK_YEARS DESC ");

            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            //ht.Add("@LEVEL_CD", LEVEL_CD);
            ht.Add("@WS_CD", "G");
            ht.Add("@DEPT_NO_20", DEPT_NO_20);
            ht.Add("@DEPT_NO_40", DEPT_NO_40);
            ht.Add("@PLANT_CD", PLANT_CD);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }


    //取得 現場系及資格為(3A~4B) 的個人資料
    public DataTable getWorker_3A4B()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select *    ");
            sb.Append(" from TB_S_M_ASSESS_TARGET  ");
            sb.Append(" where 1=1 ");
            sb.Append(" and ASSESS_YEAR = @ASSESS_YEAR ");
            sb.Append(" and ASSESS_TYPE = @ASSESS_TYPE ");
            sb.Append(" and LEVEL_CD = @LEVEL_CD ");
            sb.Append(" and WS_CD = @WS_CD ");
            sb.Append(" and PLANT_CD = @PLANT_CD ");
            sb.Append(" and DEPT_NO_20 = @DEPT_NO_20 ");
            sb.Append(" and DEPT_NO_40 = @DEPT_NO_40 ");

            sb.Append("  order by LEVELUP_FLAG DESC, RECENT_LEVEL_WORK_YEARS DESC, AGE DESC, WORK_YEARS DESC ");

            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@LEVEL_CD", LEVEL_CD);
            ht.Add("@WS_CD", "W");
            ht.Add("@DEPT_NO_20", DEPT_NO_20);
            ht.Add("@DEPT_NO_40", DEPT_NO_40);
            ht.Add("@PLANT_CD", PLANT_CD);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    //取得 現場系及資格為(5A) 的個人資料
    public DataTable getWorker_5A()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select *    ");
            sb.Append(" from TB_S_M_ASSESS_TARGET  ");
            sb.Append(" where 1=1 ");
            sb.Append(" and ASSESS_YEAR = @ASSESS_YEAR ");
            sb.Append(" and ASSESS_TYPE = @ASSESS_TYPE ");
            sb.Append(" and LEVEL_CD = @LEVEL_CD ");
            sb.Append(" and WS_CD = @WS_CD ");
            sb.Append(" and PLANT_CD = @PLANT_CD ");
            sb.Append(" and DEPT_NO = @DEPT_NO ");
            //sb.Append(" and DEPT_NO_40 = @DEPT_NO_40 ");
            sb.Append("  order by LEVEL_ORDER_SEQ ASC,GRADE_CD DESC, LEVELUP_FLAG DESC, RECENT_LEVEL_WORK_YEARS DESC, AGE DESC, WORK_YEARS DESC ");

            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@LEVEL_CD", LEVEL_CD);
            ht.Add("@WS_CD", "W");
            ht.Add("@PLANT_CD", PLANT_CD);
            ht.Add("@DEPT_NO", DEPT_NO);
            // ht.Add("@DEPT_NO_40", DEPT_NO_40);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    #endregion

}