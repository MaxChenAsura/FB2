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
/// CFB2HF020DAO 的摘要描述
/// </summary>
public class CFB2HF0200DAO : BaseDAO
{
    //HF010基本欄位
    public string DECLARA_YEAR { get; set; }
    public string APPROVE_STATUS { get; set; }
    public string EMP_ID { get; set; }
    public string EMP_NAME { get; set; }
    public string DEPT_NO { get; set; }
    public bool isDIRECT { get; set; }
    public string SEQ { get; set; }
    public string DECLARA_YEAR_S { get; set; }
    public string DECLARA_YEAR_E { get; set; }


    //自我申告內容檔 
    public string BIZ_C1 { get; set; }
    public string BIZ_C2 { get; set; }
    public string BIZ_C3 { get; set; }
    public string PJOB_DEVE_CD { get; set; }
    public string PJOB_DEVE_DESC { get; set; }
    public string COMPET_AREA_CD1 { get; set; }
    public string COMPET_AREA_DESC1 { get; set; }
    public string COMPET_AREA_CD2 { get; set; }
    public string COMPET_AREA_DESC2 { get; set; }
    public string COMPET_AREA_CD3 { get; set; }
    public string COMPET_AREA_DESC3 { get; set; }
    public string RETIRE_AGE { get; set; }

    //自我申告內容檔 (主管簽核意見檔)
    public string DEV_ABILITY1 { get; set; }
    public string DEV_PLAN1 { get; set; }
    public string PREDICT_YEAR1 { get; set; }
    public string PREDICT_MONTH1 { get; set; }
    public string DEV_ABILITY2 { get; set; }
    public string DEV_PLAN2 { get; set; }
    public string PREDICT_YEAR2 { get; set; }
    public string PREDICT_MONTH2 { get; set; }
    public string DEV_ABILITY3 { get; set; }
    public string DEV_PLAN3 { get; set; }
    public string PREDICT_YEAR3 { get; set; }
    public string PREDICT_MONTH3 { get; set; }
    public string BIZ_CHG_TYPE1 { get; set; }
    public string BIZ_CHG_ITEM1 { get; set; }
    public string CHG_DEPT_NO1 { get; set; }
    public string CHG_DEPT_NAME1 { get; set; }
    public string ICT_COMPANY_CD1 { get; set; }
    public string ICT_COMPANY1 { get; set; }
    public string BIZ_CHG_TYPE2 { get; set; }
    public string BIZ_CHG_ITEM2 { get; set; }
    public string CHG_DEPT_NO2 { get; set; }
    public string CHG_DEPT_NAME2 { get; set; }
    public string ICT_COMPANY_CD2 { get; set; }
    public string ICT_COMPANY2 { get; set; }
    public string BIZ_CHG_TYPE3 { get; set; }
    public string BIZ_CHG_ITEM3 { get; set; }
    public string CHG_DEPT_NO3 { get; set; }
    public string CHG_DEPT_NAME3 { get; set; }
    public string ICT_COMPANY_CD3 { get; set; }
    public string ICT_COMPANY3 { get; set; }
    public string WORK_C1 { get; set; }
    public string WORK_C2 { get; set; }
    public string WORK_C3 { get; set; }
    public string ADJUST_TIME { get; set; }
    public string ADJUST_REASON { get; set; }
    public string HEALTH_STATUS { get; set; }
    public string REMARK { get; set; }
    public string G_COMMENT { get; set; }

    //業務性質百分比檔
    public string BIZ_TYPE { get; set; }
    public string BIZ_ITEM { get; set; }
    public string BIZ_PERCENT { get; set; }
    public string BIZ_PERCENT_P { get; set; }

    //工作評價檔
    public string WORK_TYPE { get; set; }
    public string WORK_ITEM { get; set; }
    public string WORK_GRADES { get; set; }
    public string WORK_GRADES_P { get; set; }


    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }


    public CFB2HF0200DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }


    #region 資料取得
    //取得最大序號
    public string getMaxSeq(string year, string emp_id)
    {
        try
        {
            string result = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" 
                         select MAX(SEQ) as result from TB_H_M_DECLARATION_CONTENT with (nolock) where DECLARA_YEAR=@DECLARA_YEAR and EMP_ID=@EMP_ID
                        ");
            ht.Add("@DECLARA_YEAR", year);
            ht.Add("@EMP_ID", emp_id);
            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                result = dt.Rows[0]["result"].ToString();
            }
            else
            {
                result = "0";
            }
            return result;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //判斷是否為申告對象
    public int checkTarget()
    {
        int result = 0;
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" select count(*) resultCount from TB_H_M_DECLARATION_TARGET   with (nolock) 
                        where 1=1
                        and DECLARA_YEAR = Convert(varchar(4),YEAR(GETDATE()))
                        and EMP_ID = @EMP_ID
                        ");
            ht.Add("@EMP_ID", EMP_ID);
            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                result = (int)dt.Rows[0]["resultCount"];
            }
            else
            {
                result = 0;
            }
            return result;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //判斷修改者是否 登入者的直屬部屬
    public int checkDIRECT()
    {
        int result = 0;
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" select count(*) resultCount from TB_H_M_EMP  with (nolock) 
                        where DIRECT_HEAD_EMP_ID=@DIRECT_HEAD_EMP_ID
                        and EMP_ID=@EMP_ID
                        ");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@DIRECT_HEAD_EMP_ID", SessionHandle.Current.emp_id);
            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                result = (int)dt.Rows[0]["resultCount"];
            }
            else
            {
                result = 0;
            }
            return result;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //自我申告內容檔
    public DataTable getContentData(string year,string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@"declare @max_seq decimal(3,0); 
                         select @max_seq = MAX(SEQ) from TB_H_M_DECLARATION_CONTENT  with (nolock) where DECLARA_YEAR=@DECLARA_YEAR and EMP_ID=@EMP_ID
                        select A.*
                        from TB_H_M_DECLARATION_CONTENT A with (nolock)
                        where 1=1
                        and DECLARA_YEAR = @DECLARA_YEAR
                        and EMP_ID = @EMP_ID
                        and SEQ = @max_seq
                        ");
            ht.Add("@DECLARA_YEAR", year);
            ht.Add("@EMP_ID", emp_id);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //主管簽核意見檔
    public DataTable getCommentData(string year, string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@"declare @max_seq decimal(3,0); 
                         select @max_seq = MAX(SEQ) from TB_H_M_DECLARATION_COMMENT  with (nolock) where DECLARA_YEAR=@DECLARA_YEAR and EMP_ID=@EMP_ID
                        select A.*
                        from TB_H_M_DECLARATION_COMMENT A with (nolock)
                        where 1=1
                        and DECLARA_YEAR = @DECLARA_YEAR
                        and EMP_ID = @EMP_ID
                        and SEQ = @max_seq
                        ");
            ht.Add("@DECLARA_YEAR", year);
            ht.Add("@EMP_ID", emp_id);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //主要擔當業務性質
    public DataTable getBIZData(string year, string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@"declare @max_seq decimal(3,0); 
                         select @max_seq = MAX(SEQ) from TB_H_M_DECLARATION_BIZ  with (nolock) where DECLARA_YEAR=@DECLARA_YEAR and EMP_ID=@EMP_ID
                        select A.*
                        from TB_H_M_DECLARATION_BIZ A with (nolock)
                        where 1=1
                        and DECLARA_YEAR = @DECLARA_YEAR
                        and EMP_ID = @EMP_ID
                        and SEQ = @max_seq
                        order by BIZ_TYPE
                        ");
            ht.Add("@DECLARA_YEAR", year);
            ht.Add("@EMP_ID", emp_id);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //工作評價檔
    public DataTable getWorkData(string year, string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@"declare @max_seq decimal(3,0); 
                         select @max_seq = MAX(SEQ) from TB_H_M_DECLARATION_WORK  with (nolock) where DECLARA_YEAR=@DECLARA_YEAR and EMP_ID=@EMP_ID
                        select A.*
                        from TB_H_M_DECLARATION_WORK A with (nolock)
                        where 1=1
                        and DECLARA_YEAR = @DECLARA_YEAR
                        and EMP_ID = @EMP_ID
                        and SEQ = @max_seq
                        order by WORK_TYPE
                        ");
            ht.Add("@DECLARA_YEAR", year);
            ht.Add("@EMP_ID", emp_id);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //EXCEL匯出對象
    public DataTable getExcelData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
      
            sb.Append(@" Select A.*
                        ,A.LEVEL_CD+''+A.GRADE_CD as LEVEL_CD_DESC
                        ,A.PJOB_CD+'-'+A.PJOB_DESC as PJOB_CD_DESC
                        ,D.SUB_CD + '-' + D.SUB_DESC APPROVE_STATUS_DESC
                        from TB_H_M_DECLARATION_TARGET A  with (nolock) 
                        left join  TB_9_M_COMM_D D  with (nolock)  on D.MAIN_CD = 'APPROVE_STATUS'  and IS_VALID='Y'  and SYS_CD='HF' and D.SUB_CD=A.APPROVE_STATUS ");
            sb.Append(" where 1=1 ");

            //權限條件
            if (SessionHandle.Current.is_super != "Y")
            {
                sb.Append(@" AND a.EMP_ID IN(	 select EMP_ID from dbo.FN_H_GET_AUTH_EMP(@loginID,@departments)  )");
                ht.Add("@loginID", SessionHandle.Current.emp_id);
                ht.Add("@departments", SessionHandle.Current.departments);
            }

            //只顯示可審核對象
            if (isDIRECT)
            {
                sb.Append(@" AND a.EMP_ID IN(	 select EMP_ID from TB_H_M_EMP  with (nolock)  where DIRECT_HEAD_EMP_ID=@DIRECT_HEAD_EMP_ID  )");
                ht.Add("@DIRECT_HEAD_EMP_ID", SessionHandle.Current.emp_id);
            }

            //查詢條件
            if (DECLARA_YEAR_S != "")
            {
                sb.Append(" and A.DECLARA_YEAR >= @DECLARA_YEAR_S ");
                ht.Add("@DECLARA_YEAR_S", DECLARA_YEAR_S);
            }
            if (DECLARA_YEAR_E != "")
            {
                sb.Append(" and A.DECLARA_YEAR <= @DECLARA_YEAR_E ");
                ht.Add("@DECLARA_YEAR_E", DECLARA_YEAR_E);
            }

            if (APPROVE_STATUS != "-1")
            {
                sb.Append(" and A.APPROVE_STATUS = @APPROVE_STATUS ");
                ht.Add("@APPROVE_STATUS", APPROVE_STATUS);
            }
            if (EMP_ID != "")
            {
                sb.Append(" and A.EMP_ID like @EMP_ID ");
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

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    //EXCEL匯出對象(紀錄檔)
    public DataTable getExcelData_Record()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(@" select 
                         B.SEQ
                        ,CONVERT(VARCHAR(20),B.CREATED_DT,120) CREATED_TIME
                        ,A.*
                        ,A.LEVEL_CD+''+A.GRADE_CD as LEVEL_CD_DESC
                        ,A.PJOB_CD+'-'+A.PJOB_DESC as PJOB_CD_DESC
                        ,D.SUB_CD + '-' + D.SUB_DESC APPROVE_STATUS_DESC
                         from TB_H_M_DECLARATION_TARGET A
                        left join  TB_H_M_DECLARATION_CONTENT B	on A.DECLARA_YEAR = B.DECLARA_YEAR and A.EMP_ID = B.EMP_ID
                        left join  TB_9_M_COMM_D D  with (nolock)  on D.MAIN_CD = 'APPROVE_STATUS'  and IS_VALID='Y'  and SYS_CD='HF' and D.SUB_CD=A.APPROVE_STATUS
                       ");
            sb.Append(" where 1=1 ");

            //權限條件
            if (SessionHandle.Current.is_super != "Y")
            {
                sb.Append(@" AND a.EMP_ID IN(	 select EMP_ID from dbo.FN_H_GET_AUTH_EMP(@loginID,@departments)  )");
                ht.Add("@loginID", SessionHandle.Current.emp_id);
                ht.Add("@departments", SessionHandle.Current.departments);
            }

            //只顯示可審核對象
            if (isDIRECT)
            {
                sb.Append(@" AND a.EMP_ID IN(	 select EMP_ID from TB_H_M_EMP  with (nolock)  where DIRECT_HEAD_EMP_ID=@DIRECT_HEAD_EMP_ID  )");
                ht.Add("@DIRECT_HEAD_EMP_ID", SessionHandle.Current.emp_id);
            }

            //查詢條件
            if (DECLARA_YEAR_S != "")
            {
                sb.Append(" and A.DECLARA_YEAR >= @DECLARA_YEAR_S ");
                ht.Add("@DECLARA_YEAR_S", DECLARA_YEAR_S);
            }
            if (DECLARA_YEAR_E != "")
            {
                sb.Append(" and A.DECLARA_YEAR <= @DECLARA_YEAR_E ");
                ht.Add("@DECLARA_YEAR_E", DECLARA_YEAR_E);
            }


            if (APPROVE_STATUS != "-1")
            {
                sb.Append(" and A.APPROVE_STATUS = @APPROVE_STATUS ");
                ht.Add("@APPROVE_STATUS", APPROVE_STATUS);
            }
            if (EMP_ID != "")
            {
                sb.Append(" and A.EMP_ID like @EMP_ID ");
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

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    //自我申告內容檔(紀錄檔)
    public DataTable getContentData_Record(string year, string emp_id,string seq)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@"
                        select A.*
                        from TB_H_M_DECLARATION_CONTENT A with (nolock)
                        where 1=1
                        and DECLARA_YEAR = @DECLARA_YEAR
                        and EMP_ID = @EMP_ID
                        and SEQ = @SEQ
                        ");
            ht.Add("@DECLARA_YEAR", year);
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@SEQ", seq);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //主管簽核意見檔(紀錄檔)
    public DataTable getCommentData_Record(string year, string emp_id, string seq)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@"
                        select A.*
                        ,isnull(D.SUB_CD + '-' + D.SUB_DESC,'') APPROVE_STATUS_DESC
                        from TB_H_M_DECLARATION_COMMENT A with (nolock)
                        left join  TB_9_M_COMM_D D  with (nolock)  on D.MAIN_CD = 'APPROVE_STATUS'  and IS_VALID='Y'  and SYS_CD='HF' and D.SUB_CD=A.APPROVE_STATUS
                        where 1=1
                        and DECLARA_YEAR = @DECLARA_YEAR
                        and EMP_ID = @EMP_ID
                        and SEQ = @SEQ
                        ");
            ht.Add("@DECLARA_YEAR", year);
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@SEQ", seq);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //主要擔當業務性質(紀錄檔)
    public DataTable getBIZData_Record(string year, string emp_id, string seq)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@"
                        select A.*
                        from TB_H_M_DECLARATION_BIZ A with (nolock)
                        where 1=1
                        and DECLARA_YEAR = @DECLARA_YEAR
                        and EMP_ID = @EMP_ID
                        and SEQ = @SEQ
                        order by BIZ_TYPE
                        ");
            ht.Add("@DECLARA_YEAR", year);
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@SEQ", seq);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //工作評價檔(紀錄檔)
    public DataTable getWorkData_Record(string year, string emp_id, string seq)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@"select A.*
                        from TB_H_M_DECLARATION_WORK A with (nolock)
                        where 1=1
                        and DECLARA_YEAR = @DECLARA_YEAR
                        and EMP_ID = @EMP_ID
                        and SEQ = @SEQ
                        order by WORK_TYPE
                        ");
            ht.Add("@DECLARA_YEAR", year);
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@SEQ", seq);
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
                             , string declara_year_s, string declara_year_e, string approve_status, string emp_id, string emp_name, string dept_no, bool isDIRECT
                           )
    {
        try
        {
            /*
            if (sortExpression.Contains("REMARK"))
            {
                sortExpression = sortExpression.Replace("REMARK", "a.REMARK");
            }
            */
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + ") As RowNumber, ");
            sb.Append(@" A.*
                        ,A.LEVEL_CD+''+A.GRADE_CD as LEVEL_CD_DESC
                        ,A.PJOB_CD+'-'+A.PJOB_DESC as PJOB_CD_DESC
                        ,D.SUB_CD + '-' + D.SUB_DESC APPROVE_STATUS_DESC
                        from TB_H_M_DECLARATION_TARGET A  with (nolock) 
                        left join  TB_9_M_COMM_D D  with (nolock)  on D.MAIN_CD = 'APPROVE_STATUS'  and IS_VALID='Y'  and SYS_CD='HF' and D.SUB_CD=A.APPROVE_STATUS ");
            sb.Append(" where 1=1 ");

            //權限條件
            if (SessionHandle.Current.is_super != "Y")
            {
                sb.Append(@" AND a.EMP_ID IN(	 select EMP_ID from dbo.FN_H_GET_AUTH_EMP(@loginID,@departments)  )");
                ht.Add("@loginID", SessionHandle.Current.emp_id);
                ht.Add("@departments", SessionHandle.Current.departments);

            }

            //只顯示可審核對象
            if (isDIRECT)
            {
                sb.Append(@" AND a.EMP_ID IN(	 select EMP_ID from TB_H_M_EMP  with (nolock)  where DIRECT_HEAD_EMP_ID=@DIRECT_HEAD_EMP_ID  )");
                ht.Add("@DIRECT_HEAD_EMP_ID", SessionHandle.Current.emp_id);
            }

            //查詢條件
            if (declara_year_s != "")
            {
                sb.Append(" and A.DECLARA_YEAR >= @DECLARA_YEAR_S ");
                ht.Add("@DECLARA_YEAR_S", declara_year_s);
            }
            if (declara_year_e != "")
            {
                sb.Append(" and A.DECLARA_YEAR <= @DECLARA_YEAR_E ");
                ht.Add("@DECLARA_YEAR_E", declara_year_e);
            }

            if (approve_status != "-1")
            {
                sb.Append(" and A.APPROVE_STATUS = @APPROVE_STATUS ");
                ht.Add("@APPROVE_STATUS", approve_status);
            }
            if (emp_id != "")
            {
                sb.Append(" and A.EMP_ID like @EMP_ID ");
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
                        , string declara_year_s, string declara_year_e, string approve_status, string emp_id, string emp_name, string dept_no, bool isDIRECT)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) resultCount ");
            sb.Append(@" from TB_H_M_DECLARATION_TARGET A   with (nolock) ");
            sb.Append(" where 1=1 ");
            //權限條件
            if (SessionHandle.Current.is_super != "Y")
            {
                sb.Append(@" AND a.EMP_ID IN(	 select EMP_ID from dbo.FN_H_GET_AUTH_EMP(@loginID,@departments)  )");
                ht.Add("@loginID", SessionHandle.Current.emp_id);
                ht.Add("@departments", SessionHandle.Current.departments);

            }

            //只顯示可審核對象
            if (isDIRECT)
            {
                sb.Append(@" AND a.EMP_ID IN(	 select EMP_ID from TB_H_M_EMP  with (nolock)  where DIRECT_HEAD_EMP_ID=@DIRECT_HEAD_EMP_ID  )");
                ht.Add("@DIRECT_HEAD_EMP_ID", SessionHandle.Current.emp_id);
            }

            //查詢條件
            if (declara_year_s != "")
            {
                sb.Append(" and A.DECLARA_YEAR >= @DECLARA_YEAR_S ");
                ht.Add("@DECLARA_YEAR_S", declara_year_s);
            }
            if (declara_year_e != "")
            {
                sb.Append(" and A.DECLARA_YEAR <= @DECLARA_YEAR_E ");
                ht.Add("@DECLARA_YEAR_E", declara_year_e);
            }
            if (approve_status != "-1")
            {
                sb.Append(" and A.APPROVE_STATUS = @APPROVE_STATUS ");
                ht.Add("@APPROVE_STATUS", approve_status);
            }
            if (emp_id != "")
            {
                sb.Append(" and A.EMP_ID like @EMP_ID ");
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


    #region Gridview1 資料
    //Gridview1 查詢資料
    public DataTable getData_EDIT1(int startRowIndex, int maximumRows, string sortExpression
                            , string declara_year, string emp_id
                           )
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(@"declare @max_seq decimal(3,0); 
                        select @max_seq = MAX(SEQ) from TB_H_M_DECLARATION_BIZ  with (nolock) where DECLARA_YEAR=@DECLARA_YEAR and EMP_ID=@EMP_ID ");
            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + ") As RowNumber, ");
            sb.Append(" DECLARA_YEAR,EMP_ID,SEQ,  BIZ_ITEM ,BIZ_TYPE,BIZ_PERCENT,BIZ_PERCENT_P  ");

            sb.Append(@"from TB_H_M_DECLARATION_BIZ with (nolock) 
                        where 1=1
                        and DECLARA_YEAR=@DECLARA_YEAR
                        and EMP_ID=@EMP_ID
                        and SEQ = @max_seq 
                    ");
            ht.Add("@DECLARA_YEAR", declara_year);
            ht.Add("@EMP_ID", emp_id);


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


    //Gridview1 查詢總筆數
    public int getCount_EDIT1(int startRowIndex, int maximumRows
                            , string declara_year, string emp_id
                        )
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(@"declare @max_seq decimal(3,0); 
                        select @max_seq = MAX(SEQ) from TB_H_M_DECLARATION_BIZ  with (nolock) where DECLARA_YEAR=@DECLARA_YEAR and EMP_ID=@EMP_ID ");
            sb.Append(" Select COUNT(*) resultCount ");
            sb.Append(@" from TB_H_M_DECLARATION_BIZ with (nolock) 
                        where 1=1
                        and DECLARA_YEAR=@DECLARA_YEAR
                        and EMP_ID=@EMP_ID
                        and SEQ = @max_seq  ");
            ht.Add("@DECLARA_YEAR", declara_year);
            ht.Add("@EMP_ID", emp_id);

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

    //Gridview1 查詢資料
    public DataTable getData_EDIT2(int startRowIndex, int maximumRows, string sortExpression2
                        , string declara_year, string emp_id
                           )
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@"declare @max_seq decimal(3,0); 
                        select @max_seq = MAX(SEQ) from TB_H_M_DECLARATION_WORK  with (nolock) where DECLARA_YEAR=@DECLARA_YEAR and EMP_ID=@EMP_ID ");
            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression2 + ") As RowNumber, ");
            sb.Append(" DECLARA_YEAR,EMP_ID,SEQ,WORK_TYPE, WORK_ITEM,WORK_GRADES,WORK_GRADES_P ");
            sb.Append(@"
                        from TB_H_M_DECLARATION_WORK with (nolock) 
                        where 1=1
                        and DECLARA_YEAR=@DECLARA_YEAR
                        and EMP_ID=@EMP_ID
                        and SEQ = @max_seq 
                    ");
            ht.Add("@DECLARA_YEAR", declara_year);
            ht.Add("@EMP_ID", emp_id);


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

    //Gridview1 查詢總筆數
    public int getCount_EDIT2(int startRowIndex, int maximumRows
                        , string declara_year, string emp_id
                        )
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@"declare @max_seq decimal(3,0); 
                        select @max_seq = MAX(SEQ) from TB_H_M_DECLARATION_WORK  with (nolock) where DECLARA_YEAR=@DECLARA_YEAR and EMP_ID=@EMP_ID ");
            sb.Append(" Select COUNT(*) resultCount ");
            sb.Append(@" from TB_H_M_DECLARATION_WORK with (nolock) 
                        where 1=1
                        and DECLARA_YEAR=@DECLARA_YEAR
                        and EMP_ID=@EMP_ID
                        and SEQ = @max_seq  ");
            ht.Add("@DECLARA_YEAR", declara_year);
            ht.Add("@EMP_ID", emp_id);

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
    //修改 業務性質百分比檔
    public void updateBIZ()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            DateTime now = DateTime.Parse(DateTime.Now.ToString());
            sb.Append(@" update TB_H_M_DECLARATION_BIZ
                        set 
                         BIZ_PERCENT_P = @BIZ_PERCENT_P
                        ,UPDATED_BY= @UPDATED_BY
                        ,UPDATED_DT = @UPDATED_DT
                        ,FUNC_ID=@FUNC_ID
                        where DECLARA_YEAR = @DECLARA_YEAR 
                        and EMP_ID = @EMP_ID
                        and SEQ = @SEQ
                        and BIZ_TYPE = @BIZ_TYPE
                    ");

            //修改值
            ht.Add("@BIZ_PERCENT_P", BIZ_PERCENT_P);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@UPDATED_DT", now);
            ht.Add("@FUNC_ID", FUNC_ID);
            //PK值
            ht.Add("@DECLARA_YEAR", DECLARA_YEAR);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SEQ", SEQ);
            ht.Add("@BIZ_TYPE", BIZ_TYPE);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //修改 自我申告對象檔
    public void updateTARGET(string isSubmit)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            DateTime now = DateTime.Parse(DateTime.Now.ToString());
            sb.Append(@" update TB_H_M_DECLARATION_TARGET
                        set 
                         APPROVE_STATUS = @APPROVE_STATUS
                        ,APPROVE_DT = @APPROVE_DT
                        ,APPROVE_BY = @APPROVE_BY 
                        ,RELEASE_DT= @RELEASE_DT
                        ,UPDATED_BY= @UPDATED_BY
                        ,UPDATED_DT = @UPDATED_DT
                        ,FUNC_ID=@FUNC_ID
                        where DECLARA_YEAR = @DECLARA_YEAR 
                        and EMP_ID = @EMP_ID
                    ");

            //修改值
            ht.Add("@APPROVE_STATUS", isSubmit);//Y:核可, B:駁回, N:未核可(抽單)
            ht.Add("@APPROVE_DT", now);
            ht.Add("@APPROVE_BY", UPDATED_BY);
            ht.Add("@RELEASE_DT", now);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@UPDATED_DT", now);
            ht.Add("@FUNC_ID", FUNC_ID);
            //PK值
            ht.Add("@DECLARA_YEAR", DECLARA_YEAR);
            ht.Add("@EMP_ID", EMP_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //修改 主管簽核意見檔
    public void updateCOMMENT(string submitFlag)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            DateTime now = DateTime.Parse(DateTime.Now.ToString());
            sb.Append(@" update TB_H_M_DECLARATION_COMMENT
                        set 
                         DEV_ABILITY1=@DEV_ABILITY1
                        ,DEV_PLAN1=@DEV_PLAN1
                        ,PREDICT_YEAR1=@PREDICT_YEAR1
                        ,PREDICT_MONTH1=@PREDICT_MONTH1
                        ,DEV_ABILITY2=@DEV_ABILITY2
                        ,DEV_PLAN2=@DEV_PLAN2
                        ,PREDICT_YEAR2=@PREDICT_YEAR2
                        ,PREDICT_MONTH2=@PREDICT_MONTH2
                        ,DEV_ABILITY3=@DEV_ABILITY3
                        ,DEV_PLAN3=@DEV_PLAN3
                        ,PREDICT_YEAR3=@PREDICT_YEAR3
                        ,PREDICT_MONTH3=@PREDICT_MONTH3
                        ,BIZ_CHG_TYPE1=@BIZ_CHG_TYPE1
                        ,BIZ_CHG_ITEM1=isnull(( select SUB_DESC from TB_9_M_COMM_D D where MAIN_CD = 'BIZ_CHG_TYPE'  and IS_VALID='Y'  and SYS_CD='HF' and D.SUB_CD=@BIZ_CHG_TYPE1 ),'')
                        ,CHG_DEPT_NO1=@CHG_DEPT_NO1
                        ,CHG_DEPT_NAME1= isnull((select DEPT_FULL_NAME from TB_H_R_DEPT_DATA where DEPT_NO=@CHG_DEPT_NO1 ),'')
                        ,ICT_COMPANY_CD1=@ICT_COMPANY_CD1
                        ,ICT_COMPANY1=isnull((select SUB_DESC from TB_9_M_COMM_D D where MAIN_CD = 'ICT_COMPANY_CD'  and IS_VALID='Y'  and SYS_CD='HC' and D.SUB_CD=@ICT_COMPANY_CD1),'')
                        ,BIZ_CHG_TYPE2=@BIZ_CHG_TYPE2
                        ,BIZ_CHG_ITEM2=isnull(( select SUB_DESC from TB_9_M_COMM_D D where MAIN_CD = 'BIZ_CHG_TYPE'  and IS_VALID='Y'  and SYS_CD='HF' and D.SUB_CD=@BIZ_CHG_TYPE2 ),'')
                        ,CHG_DEPT_NO2=@CHG_DEPT_NO2
                        ,CHG_DEPT_NAME2= isnull((select DEPT_FULL_NAME from TB_H_R_DEPT_DATA where DEPT_NO=@CHG_DEPT_NO2 ),'')
                        ,ICT_COMPANY_CD2=@ICT_COMPANY_CD2
                        ,ICT_COMPANY2=isnull((select SUB_DESC from TB_9_M_COMM_D D where MAIN_CD = 'ICT_COMPANY_CD'  and IS_VALID='Y'  and SYS_CD='HC' and D.SUB_CD=@ICT_COMPANY_CD2),'')
                        ,BIZ_CHG_TYPE3=@BIZ_CHG_TYPE3
                        ,BIZ_CHG_ITEM3=isnull(( select SUB_DESC from TB_9_M_COMM_D D where MAIN_CD = 'BIZ_CHG_TYPE'  and IS_VALID='Y'  and SYS_CD='HF' and D.SUB_CD=@BIZ_CHG_TYPE3 ),'')
                        ,CHG_DEPT_NO3=@CHG_DEPT_NO3
                        ,CHG_DEPT_NAME3=isnull((select DEPT_FULL_NAME from TB_H_R_DEPT_DATA where DEPT_NO=@CHG_DEPT_NO3 ),'')
                        ,ICT_COMPANY_CD3=@ICT_COMPANY_CD3
                        ,ICT_COMPANY3=isnull((select SUB_DESC from TB_9_M_COMM_D D where MAIN_CD = 'ICT_COMPANY_CD'  and IS_VALID='Y'  and SYS_CD='HC' and D.SUB_CD=@ICT_COMPANY_CD3),'')
                        ,WORK_C1=@WORK_C1
                        ,WORK_C2=@WORK_C2
                        ,WORK_C3=@WORK_C3
                        ,ADJUST_TIME=@ADJUST_TIME
                        ,ADJUST_REASON=@ADJUST_REASON
                        ,G_COMMENT=@G_COMMENT
                        ,APPROVE_STATUS = @APPROVE_STATUS
                        ,UPDATED_BY= @UPDATED_BY
                        ,UPDATED_DT = @UPDATED_DT
                        ,FUNC_ID=@FUNC_ID
                        where DECLARA_YEAR = @DECLARA_YEAR 
                        and EMP_ID = @EMP_ID
                        and SEQ = @SEQ
                    ");

            //修改值
            ht.Add("@BIZ_C1", BIZ_C1);
            ht.Add("@BIZ_C2", BIZ_C2);
            ht.Add("@BIZ_C3", BIZ_C3);
            ht.Add("@PJOB_DEVE_CD", PJOB_DEVE_CD);
            ht.Add("@RETIRE_AGE", RETIRE_AGE);
            ht.Add("@COMPET_AREA_CD1", COMPET_AREA_CD1);
            ht.Add("@COMPET_AREA_DESC1", COMPET_AREA_DESC1);
            ht.Add("@COMPET_AREA_CD2", COMPET_AREA_CD2);
            ht.Add("@COMPET_AREA_DESC2", COMPET_AREA_DESC2);
            ht.Add("@COMPET_AREA_CD3", COMPET_AREA_CD3);
            ht.Add("@COMPET_AREA_DESC3", COMPET_AREA_DESC3);
            ht.Add("@DEV_ABILITY1", DEV_ABILITY1);
            ht.Add("@DEV_PLAN1", DEV_PLAN1);
            ht.Add("@PREDICT_YEAR1", PREDICT_YEAR1);
            ht.Add("@PREDICT_MONTH1", PREDICT_MONTH1);
            ht.Add("@DEV_ABILITY2", DEV_ABILITY2);
            ht.Add("@DEV_PLAN2", DEV_PLAN2);
            ht.Add("@PREDICT_YEAR2", PREDICT_YEAR2);
            ht.Add("@PREDICT_MONTH2", PREDICT_MONTH2);
            ht.Add("@DEV_ABILITY3", DEV_ABILITY3);
            ht.Add("@DEV_PLAN3", DEV_PLAN3);
            ht.Add("@PREDICT_YEAR3", PREDICT_YEAR3);
            ht.Add("@PREDICT_MONTH3", PREDICT_MONTH3);
            ht.Add("@BIZ_CHG_TYPE1", BIZ_CHG_TYPE1);
            ht.Add("@CHG_DEPT_NO1", CHG_DEPT_NO1);
            ht.Add("@ICT_COMPANY_CD1", ICT_COMPANY_CD1);
            ht.Add("@BIZ_CHG_TYPE2", BIZ_CHG_TYPE2);
            ht.Add("@CHG_DEPT_NO2", CHG_DEPT_NO2);
            ht.Add("@ICT_COMPANY_CD2", ICT_COMPANY_CD2);
            ht.Add("@BIZ_CHG_TYPE3", BIZ_CHG_TYPE3);
            ht.Add("@CHG_DEPT_NO3", CHG_DEPT_NO3);
            ht.Add("@ICT_COMPANY_CD3", ICT_COMPANY_CD3);
            ht.Add("@WORK_C1", WORK_C1);
            ht.Add("@WORK_C2", WORK_C2);
            ht.Add("@WORK_C3", WORK_C3);
            ht.Add("@ADJUST_TIME", ADJUST_TIME);
            ht.Add("@ADJUST_REASON", ADJUST_REASON);
            ht.Add("@G_COMMENT", G_COMMENT);
            ht.Add("@APPROVE_STATUS", submitFlag);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@UPDATED_DT", now);
            ht.Add("@FUNC_ID", FUNC_ID);
            //PK值
            ht.Add("@DECLARA_YEAR", DECLARA_YEAR);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SEQ", SEQ);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }


    }

    //修改 工作評價檔
    public void updateWORK()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            DateTime now = DateTime.Parse(DateTime.Now.ToString());
            sb.Append(@" update TB_H_M_DECLARATION_WORK
                        set 
                         WORK_GRADES_P = @WORK_GRADES_P
                        ,UPDATED_BY= @UPDATED_BY
                        ,UPDATED_DT = @UPDATED_DT
                        ,FUNC_ID=@FUNC_ID
                        where DECLARA_YEAR = @DECLARA_YEAR 
                        and EMP_ID = @EMP_ID
                        and SEQ = @SEQ
                        and WORK_TYPE = @WORK_TYPE
                    ");

            //修改值
            ht.Add("@WORK_GRADES_P", WORK_GRADES_P);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@UPDATED_DT", now);
            ht.Add("@FUNC_ID", FUNC_ID);
            //PK值
            ht.Add("@DECLARA_YEAR", DECLARA_YEAR);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SEQ", SEQ);
            ht.Add("@WORK_TYPE", WORK_TYPE);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //新增 自我申告內容檔
    public void insertCONTENT()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            DateTime now = DateTime.Parse(DateTime.Now.ToString());
            sb.Append(@" INSERT INTO TB_H_M_DECLARATION_CONTENT
                        (DECLARA_YEAR,EMP_ID,SEQ,BIZ_C1,BIZ_C2
                        ,BIZ_C3,PJOB_DEVE_CD,PJOB_DEVE_DESC,COMPET_AREA_CD1,COMPET_AREA_DESC1
                        ,COMPET_AREA_CD2,COMPET_AREA_DESC2,COMPET_AREA_CD3,COMPET_AREA_DESC3,RETIRE_AGE
                        ,DEV_ABILITY1,DEV_PLAN1,PREDICT_YEAR1,PREDICT_MONTH1,DEV_ABILITY2
                        ,DEV_PLAN2,PREDICT_YEAR2,PREDICT_MONTH2,DEV_ABILITY3,DEV_PLAN3
                        ,PREDICT_YEAR3,PREDICT_MONTH3,BIZ_CHG_TYPE1,BIZ_CHG_ITEM1,CHG_DEPT_NO1
                        ,CHG_DEPT_NAME1,ICT_COMPANY_CD1,ICT_COMPANY1,BIZ_CHG_TYPE2,BIZ_CHG_ITEM2
                        ,CHG_DEPT_NO2,CHG_DEPT_NAME2,ICT_COMPANY_CD2,ICT_COMPANY2,BIZ_CHG_TYPE3
                        ,BIZ_CHG_ITEM3,CHG_DEPT_NO3,CHG_DEPT_NAME3,ICT_COMPANY_CD3,ICT_COMPANY3
                        ,WORK_C1,WORK_C2,WORK_C3,ADJUST_TIME,ADJUST_REASON,HEALTH_STATUS,REMARK
                        ,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)
                        select DECLARA_YEAR,EMP_ID,SEQ + 1,BIZ_C1,BIZ_C2
                        ,BIZ_C3,PJOB_DEVE_CD,PJOB_DEVE_DESC,COMPET_AREA_CD1,COMPET_AREA_DESC1
                        ,COMPET_AREA_CD2,COMPET_AREA_DESC2,COMPET_AREA_CD3,COMPET_AREA_DESC3,RETIRE_AGE
                        ,DEV_ABILITY1,DEV_PLAN1,PREDICT_YEAR1,PREDICT_MONTH1,DEV_ABILITY2
                        ,DEV_PLAN2,PREDICT_YEAR2,PREDICT_MONTH2,DEV_ABILITY3,DEV_PLAN3
                        ,PREDICT_YEAR3,PREDICT_MONTH3,BIZ_CHG_TYPE1,BIZ_CHG_ITEM1,CHG_DEPT_NO1
                        ,CHG_DEPT_NAME1,ICT_COMPANY_CD1,ICT_COMPANY1,BIZ_CHG_TYPE2,BIZ_CHG_ITEM2
                        ,CHG_DEPT_NO2,CHG_DEPT_NAME2,ICT_COMPANY_CD2,ICT_COMPANY2,BIZ_CHG_TYPE3
                        ,BIZ_CHG_ITEM3,CHG_DEPT_NO3,CHG_DEPT_NAME3,ICT_COMPANY_CD3,ICT_COMPANY3
                        ,WORK_C1,WORK_C2,WORK_C3,ADJUST_TIME,ADJUST_REASON,HEALTH_STATUS,REMARK
                        ,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID
                         from TB_H_M_DECLARATION_CONTENT
                        where DECLARA_YEAR=@DECLARA_YEAR and EMP_ID=@EMP_ID and SEQ=@SEQ
                    ");
            //PK值
            ht.Add("@DECLARA_YEAR", DECLARA_YEAR);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SEQ", SEQ);

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

    //新增 業務性質百分比檔
    public void insertBIZ()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            DateTime now = DateTime.Parse(DateTime.Now.ToString());
            sb.Append(@" INSERT INTO  TB_H_M_DECLARATION_BIZ
                        (DECLARA_YEAR,EMP_ID,SEQ ,BIZ_TYPE,BIZ_ITEM,BIZ_PERCENT,BIZ_PERCENT_P
                        ,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)
                        select 
                        DECLARA_YEAR,EMP_ID,SEQ + 1 ,BIZ_TYPE,BIZ_ITEM,BIZ_PERCENT,BIZ_PERCENT_P
                        ,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID
                         from TB_H_M_DECLARATION_BIZ
                        where DECLARA_YEAR=@DECLARA_YEAR and EMP_ID=@EMP_ID and SEQ=@SEQ
                    ");
            //PK值
            ht.Add("@DECLARA_YEAR", DECLARA_YEAR);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SEQ", SEQ);

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

    //新增 工作評價檔
    public void insertWORK()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            DateTime now = DateTime.Parse(DateTime.Now.ToString());
            sb.Append(@" INSERT INTO TB_H_M_DECLARATION_WORK
                        (DECLARA_YEAR
                        ,EMP_ID,SEQ,WORK_TYPE,WORK_ITEM,WORK_GRADES,WORK_GRADES_P
                        ,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)
                        select DECLARA_YEAR,EMP_ID,SEQ + 1,WORK_TYPE,WORK_ITEM,WORK_GRADES,WORK_GRADES_P
                        ,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID
                         from TB_H_M_DECLARATION_WORK
                        where DECLARA_YEAR=@DECLARA_YEAR and EMP_ID=@EMP_ID and SEQ=@SEQ
                    ");
            //PK值
            ht.Add("@DECLARA_YEAR", DECLARA_YEAR);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SEQ", SEQ);

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

    //新增 主管簽核意見檔
    public void insertCOMMENT(string approve)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            DateTime now = DateTime.Parse(DateTime.Now.ToString());
            sb.Append(@" INSERT INTO TB_H_M_DECLARATION_COMMENT(
                        DECLARA_YEAR,EMP_ID,SEQ,DEV_ABILITY1,DEV_PLAN1
                        ,PREDICT_YEAR1,PREDICT_MONTH1,DEV_ABILITY2,DEV_PLAN2,PREDICT_YEAR2,PREDICT_MONTH2,DEV_ABILITY3
                        ,DEV_PLAN3,PREDICT_YEAR3,PREDICT_MONTH3,BIZ_CHG_TYPE1,BIZ_CHG_ITEM1,CHG_DEPT_NO1,CHG_DEPT_NAME1
                        ,ICT_COMPANY_CD1,ICT_COMPANY1,BIZ_CHG_TYPE2,BIZ_CHG_ITEM2,CHG_DEPT_NO2,CHG_DEPT_NAME2,ICT_COMPANY_CD2
                        ,ICT_COMPANY2,BIZ_CHG_TYPE3,BIZ_CHG_ITEM3,CHG_DEPT_NO3,CHG_DEPT_NAME3,ICT_COMPANY_CD3
                        ,ICT_COMPANY3,WORK_C1,WORK_C2,WORK_C3,ADJUST_TIME,ADJUST_REASON,G_COMMENT,APPROVE_STATUS
                        ,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID
                        )
                        select DECLARA_YEAR,EMP_ID,SEQ+1,DEV_ABILITY1,DEV_PLAN1
                        ,PREDICT_YEAR1,PREDICT_MONTH1,DEV_ABILITY2,DEV_PLAN2,PREDICT_YEAR2,PREDICT_MONTH2,DEV_ABILITY3
                        ,DEV_PLAN3,PREDICT_YEAR3,PREDICT_MONTH3,BIZ_CHG_TYPE1,BIZ_CHG_ITEM1,CHG_DEPT_NO1,CHG_DEPT_NAME1
                        ,ICT_COMPANY_CD1,ICT_COMPANY1,BIZ_CHG_TYPE2,BIZ_CHG_ITEM2,CHG_DEPT_NO2,CHG_DEPT_NAME2,ICT_COMPANY_CD2
                        ,ICT_COMPANY2,BIZ_CHG_TYPE3,BIZ_CHG_ITEM3,CHG_DEPT_NO3,CHG_DEPT_NAME3,ICT_COMPANY_CD3
                        ,ICT_COMPANY3,WORK_C1,WORK_C2,WORK_C3,ADJUST_TIME,ADJUST_REASON,G_COMMENT,@APPROVE_STATUS
                        ,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID
                         from TB_H_M_DECLARATION_COMMENT
                        where DECLARA_YEAR=@DECLARA_YEAR and EMP_ID=@EMP_ID and SEQ=@SEQ
                    ");
            //PK值
            ht.Add("@DECLARA_YEAR", DECLARA_YEAR);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SEQ", SEQ);
            ht.Add("@APPROVE_STATUS", approve);

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

    #endregion



}