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
/// CFB2HF010DAO 的摘要描述
/// </summary>
public class CFB2HF0100DAO : BaseDAO
{
    //HF010基本欄位
    public string DECLARA_YEAR { get; set; }
    public string APPROVE_STATUS { get; set; }
    public string EMP_ID { get; set; }
    public string SEQ { get; set; }
     
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

    //業務性質百分比檔
    public string BIZ_TYPE { get; set; }
    public string BIZ_ITEM { get; set; }
    public string BIZ_PERCENT { get; set; }

    //工作評價檔
    public string WORK_TYPE { get; set; }
    public string WORK_ITEM { get; set; }
    public string WORK_GRADES { get; set; }

    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }


    public CFB2HF0100DAO()
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


    //檢核是否在申告期間
    public string checkDatePeriod() 
    {
        string result = "";
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" DECLARE @sys_dt DateTime = CURRENT_TIMESTAMP;
                        DECLARE @sys_year varchar(4) = YEAR(@sys_dt);	--系統年
                        DECLARE @start_dt_MMdd varchar(10);  			--自我申告開始日期(參數檔日期)
                        DECLARE @end_dt_MMdd   varchar(10);				--自我申告結束日(參數檔日期)
                        DECLARE @start_dt DateTime;  					--自我申告開始日
                        DECLARE @end_dt   DateTime;						--自我申告結束日

                        select @start_dt_MMdd = CODE_VAL1 from TB_9_M_PARAMETER with (nolock) where SYS_CD='HF' and MAIN_CD='DECLARATION_START_DT';
                        select @end_dt_MMdd = CODE_VAL1 from TB_9_M_PARAMETER with (nolock) where SYS_CD='HF' and MAIN_CD='DECLARATION_END_DT';
                        SET @start_dt = CONVERT(datetime,(@sys_year+'/' +@start_dt_MMdd));
                        SET @end_dt = CONVERT(datetime,(@sys_year+'/' +@end_dt_MMdd));
                        IF @sys_dt < @start_dt or @sys_dt> @end_dt 
	                        select '非自我申告期間，無法處理！' result ;
                        else
	                        select '' result ;
                        ");
            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                result = dt.Rows[0]["result"].ToString();
            }
            else {
                result = "";
            }
            return result;
        }
        catch (Exception)
        {
            throw;
        }
    
    }

    //是否為申告對象
    public int checkTarget() {
        int result = 0;
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" select count(*) resultCount from TB_H_M_DECLARATION_TARGET  with (nolock)
                        where 1=1
                        and DECLARA_YEAR = Convert(varchar(4),YEAR(GETDATE()))
                        and EMP_ID = @EMP_ID
                        and APPROVE_STATUS in ('N','B')
                        ");
            ht.Add("@EMP_ID", SessionHandle.Current.emp_id);
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

    //基本資料
    public DataTable getData() {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" select A.*
                        ,A.DEPT_NO+' '+A.DEPT_FULL_NAME as DEPT_DESC
                        ,A.LEVEL_CD+''+A.GRADE_CD as LEVEL_CD_DESC
                        ,A.PJOB_CD+'-'+A.PJOB_DESC as PJOB_CD_DESC
                        ,D.SUB_CD + '-' + D.SUB_DESC APPROVE_STATUS_DESC
                        ,E.SUB_CD + '-' + E.SUB_DESC PLANT_CD_DESC
                        from TB_H_M_DECLARATION_TARGET A with (nolock)
                        left join  TB_9_M_COMM_D D  with (nolock) on D.MAIN_CD = 'APPROVE_STATUS'  and D.IS_VALID='Y'  and D.SYS_CD='HF' and D.SUB_CD=A.APPROVE_STATUS
                        left join  TB_9_M_COMM_D E with (nolock) on E.MAIN_CD = 'PLANT_CD'  and E.IS_VALID='Y'  and E.SYS_CD='HB' and E.SUB_CD=A.PLANT_CD
                        where 1=1
                        and DECLARA_YEAR = @DECLARA_YEAR
                        and EMP_ID = @EMP_ID
                        ");
            ht.Add("@DECLARA_YEAR", DECLARA_YEAR);
            ht.Add("@EMP_ID", EMP_ID);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //自我申告內容檔
    public DataTable getContentData()
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
            ht.Add("@DECLARA_YEAR", DECLARA_YEAR);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SEQ", SEQ);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

   
    //主要擔當業務性質
    public DataTable getBIZData()
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
                        ");
            ht.Add("@DECLARA_YEAR", DECLARA_YEAR);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SEQ", SEQ);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //工作評價檔
    public DataTable getWORKData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@"
                        select A.*
                        from TB_H_M_DECLARATION_WORK A with (nolock)
                        where 1=1
                        and DECLARA_YEAR = @DECLARA_YEAR
                        and EMP_ID = @EMP_ID
                        and SEQ = @SEQ
                        ");
            ht.Add("@DECLARA_YEAR", DECLARA_YEAR);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SEQ", SEQ);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //主管簽核意見檔
    public DataTable getCommentData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@"
                        select A.*
                        from TB_H_M_DECLARATION_COMMENT A with (nolock)
                        where 1=1
                        and DECLARA_YEAR = @DECLARA_YEAR
                        and EMP_ID = @EMP_ID
                        and SEQ = @SEQ
                        ");
            ht.Add("@DECLARA_YEAR", DECLARA_YEAR);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SEQ", SEQ);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //判斷某員工是否已為某種核可狀態,1:表示為該狀態
    public int checkStatus(string year,string empId,string approve_status)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" select count(*) resultCount from TB_H_M_DECLARATION_TARGET with (nolock)
                        where 1=1
                        and DECLARA_YEAR = @DECLARA_YEAR
                        and EMP_ID = @EMP_ID
                        and APPROVE_STATUS=@APPROVE_STATUS
                        ");
            ht.Add("@DECLARA_YEAR", year);
            ht.Add("@EMP_ID", empId);
            ht.Add("@APPROVE_STATUS", approve_status);
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

    //判斷某員工的核可狀態 是否已為 未核可 以外的 狀態 
    public int checkStatus(string year, string empId)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" select count(*) resultCount from TB_H_M_DECLARATION_TARGET with (nolock)
                         where 1=1
                         and DECLARA_YEAR = @DECLARA_YEAR
                         and EMP_ID = @EMP_ID
                         and APPROVE_STATUS !='N'
                        ");
            ht.Add("@DECLARA_YEAR", year);
            ht.Add("@EMP_ID", empId);
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

    //取得對象檔的 職能領域
    public DataTable getCOMPET_AREA()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" declare @max_seq decimal(3,0); 
                         select @max_seq = MAX(SEQ) from TB_H_M_DECLARATION_CONTENT  with (nolock) where DECLARA_YEAR=@DECLARA_YEAR and EMP_ID=@EMP_ID
                        select
                        COMPET_AREA_CD1, COMPET_AREA_CD2, COMPET_AREA_CD3  
                        ,COMPET_AREA_CD1+','+COMPET_AREA_CD2+','+COMPET_AREA_CD3 as COMPET_AREA_CD 
                        ,COMPET_AREA_DESC1+','+COMPET_AREA_DESC2+','+COMPET_AREA_DESC3 as COMPET_AREA_DESC 
                        from TB_H_M_DECLARATION_CONTENT  with (nolock)
                        where 1=1
                        and DECLARA_YEAR=@DECLARA_YEAR
                        and EMP_ID=@EMP_ID
                        and SEQ=@SEQ
                        ");
            ht.Add("@DECLARA_YEAR", DECLARA_YEAR);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SEQ", SEQ);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得已選取的 職能領域
    public DataTable getSelectedData(string selectedCompetArea)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" select  SUB_CD,SUB_DESC  
                        from TB_9_M_COMM_D  with (nolock)
                        where  SYS_CD='HF' and MAIN_CD='COMPET_AREA' and IS_VALID='Y'  
                        and SUB_CD  in (select SUB_CD from  [dbo].[FN_SPLIT_CHARACTOR](',',@selectedCompetArea))
                        order by ORDER_SEQ
                        ");
            ht.Add("@selectedCompetArea", selectedCompetArea);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得 未選取 的 職能領域
    public DataTable getNonSelectedData(string selectedCompetArea)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" select  SUB_CD,SUB_DESC  
                        from TB_9_M_COMM_D  with (nolock)
                        where  SYS_CD='HF' and MAIN_CD='COMPET_AREA' and IS_VALID='Y'  
                        and SUB_CD  not in (select SUB_CD from  [dbo].[FN_SPLIT_CHARACTOR](',',@selectedCompetArea))
                        order by ORDER_SEQ
                        ");
            ht.Add("@selectedCompetArea", selectedCompetArea);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得 部門名稱
    public string getDEPT_FULL_NAME(string dept_no)
    {
        string result = "";
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" select DEPT_FULL_NAME from TB_H_R_DEPT_DATA  with (nolock)
                            where DEPT_NO=@DEPT_NO
                        ");
            ht.Add("@DEPT_NO", dept_no);
            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                result = dt.Rows[0]["DEPT_FULL_NAME"].ToString();
            }
            else
            {
                result = "";
            }
            return result;
        }
        catch (Exception)
        {
            throw;
        }

    }

    //是否為部,室部門單位
    public string dept_level_2030(string dept_no)
    {
        string result = "";
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" select * from TB_H_R_DEPT_DATA  with (nolock)
                            where DEPT_NO=@DEPT_NO
                            and DEPT_LEVEL in('20','30')
                        ");
            ht.Add("@DEPT_NO", dept_no);
            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                result = "Y";
            }
            else
            {
                result = "N";
            }
            return result;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //是否為課級單位
    public string dept_level_3040(string dept_no)
    {
        string result = "";
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" select * from TB_H_R_DEPT_DATA  with (nolock)
                            where DEPT_NO=@DEPT_NO
                            and DEPT_LEVEL in('40','30')
                        ");
            ht.Add("@DEPT_NO", dept_no);
            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                result = "Y";
            }
            else
            {
                result = "N";
            }
            return result;
        }
        catch (Exception)
        {
            throw;
        }
    }


    #endregion

    #region Gridview Qry資料
    //Gridview 查詢資料
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression
                            , string declara_year_s, string declara_year_e, string approve_status, string emp_id, string emp_name, string dept_no
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
                        left join  TB_9_M_COMM_D D  with (nolock) on D.MAIN_CD = 'APPROVE_STATUS'  and IS_VALID='Y'  and SYS_CD='HF' and D.SUB_CD=A.APPROVE_STATUS ");
            sb.Append(" where 1=1 ");

            //權限條件
            if (SessionHandle.Current.is_super != "Y")
            {
                sb.Append(" and EMP_ID = @SELF_EMP_ID ");
                ht.Add("@SELF_EMP_ID", SessionHandle.Current.emp_id);

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
                        , string declara_year_s, string declara_year_e, string approve_status, string emp_id, string emp_name, string dept_no)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) resultCount ");
            sb.Append(@" from TB_H_M_DECLARATION_TARGET A  with (nolock) ");
            sb.Append(" where 1=1 ");
            //權限條件
            if (SessionHandle.Current.is_super != "Y")
            {
                sb.Append(" and EMP_ID = @SELF_EMP_ID ");
                ht.Add("@SELF_EMP_ID", SessionHandle.Current.emp_id);

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
                            , string declara_year, string emp_id, string approve_status
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
            if (approve_status == "Y")
            {
                sb.Append(" DECLARA_YEAR,EMP_ID,SEQ,  BIZ_ITEM ,BIZ_TYPE,BIZ_PERCENT, BIZ_PERCENT_P  ");
            }
            else {
                sb.Append(" DECLARA_YEAR,EMP_ID,SEQ,  BIZ_ITEM ,BIZ_TYPE,BIZ_PERCENT,'' as BIZ_PERCENT_P   ");
            }

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
                            , string declara_year, string emp_id, string approve_status
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
                        , string declara_year, string emp_id, string approve_status
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
            if (approve_status == "Y")
            {
                sb.Append(" DECLARA_YEAR,EMP_ID,SEQ,WORK_TYPE, WORK_ITEM ,WORK_GRADES,WORK_GRADES_P  ");
            }
            else
            {
                sb.Append(" DECLARA_YEAR,EMP_ID,SEQ,WORK_TYPE, WORK_ITEM,WORK_GRADES,'' as WORK_GRADES_P ");
            }
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
                        , string declara_year, string emp_id, string approve_status
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

    //刪除 
    public void deleteData(string tableName)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" delete from ");
            sb.Append(tableName );
            sb.Append(@" where DECLARA_YEAR = @DECLARA_YEAR 
                        and EMP_ID = @EMP_ID
                        ");
            ht.Add("@DECLARA_YEAR", DECLARA_YEAR);
            ht.Add("@EMP_ID", EMP_ID);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //呼叫對象生成SP
    internal void execSP_H_DECLARATION_DATA()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_H_DECLARATION_DATA");
            ht.Add("@USERID", SessionHandle.Current.emp_id );
            ht.Add("@FUNCID", "FB2HF010");
            dbConn.ExecuteSPT(sb, ht, true);

        }
        catch (Exception)
        {

            throw;
        }

    }

    //修改 自我申告內容檔
    public void updateCONTENT()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            DateTime now = DateTime.Parse(DateTime.Now.ToString());
            sb.Append(@" update TB_H_M_DECLARATION_CONTENT
                        set 
                         BIZ_C1 = @BIZ_C1
                        ,BIZ_C2 = @BIZ_C2
                        ,BIZ_C3 = @BIZ_C3
                        ,PJOB_DEVE_CD=@PJOB_DEVE_CD
                        ,PJOB_DEVE_DESC= isnull(( select SUB_DESC from TB_9_M_COMM_D D where MAIN_CD = 'PJOB_DEVE_CD'  and IS_VALID='Y'  and SYS_CD='HF' and D.SUB_CD=@PJOB_DEVE_CD ),'')
                        ,COMPET_AREA_CD1=@COMPET_AREA_CD1
                        ,COMPET_AREA_DESC1=@COMPET_AREA_DESC1
                        ,COMPET_AREA_CD2=@COMPET_AREA_CD2
                        ,COMPET_AREA_DESC2=@COMPET_AREA_DESC2
                        ,COMPET_AREA_CD3=@COMPET_AREA_CD3
                        ,COMPET_AREA_DESC3=@COMPET_AREA_DESC3
                        ,RETIRE_AGE=@RETIRE_AGE
                        ,DEV_ABILITY1=@DEV_ABILITY1
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
                        ,HEALTH_STATUS=@HEALTH_STATUS
                        ,REMARK=@REMARK
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
            ht.Add("@HEALTH_STATUS", HEALTH_STATUS);
            ht.Add("@REMARK", REMARK);
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
                         BIZ_PERCENT = @BIZ_PERCENT
                        ,UPDATED_BY= @UPDATED_BY
                        ,UPDATED_DT = @UPDATED_DT
                        ,FUNC_ID=@FUNC_ID
                        where DECLARA_YEAR = @DECLARA_YEAR 
                        and EMP_ID = @EMP_ID
                        and SEQ = @SEQ
                        and BIZ_TYPE = @BIZ_TYPE
                    ");

            //修改值
            ht.Add("@BIZ_PERCENT", BIZ_PERCENT);
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
                         WORK_GRADES = @WORK_GRADES
                        ,UPDATED_BY= @UPDATED_BY
                        ,UPDATED_DT = @UPDATED_DT
                        ,FUNC_ID=@FUNC_ID
                        where DECLARA_YEAR = @DECLARA_YEAR 
                        and EMP_ID = @EMP_ID
                        and SEQ = @SEQ
                        and WORK_TYPE = @WORK_TYPE
                    ");

            //修改值
            ht.Add("@WORK_GRADES", WORK_GRADES);
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


    //修改 自我申告對象檔
    public void updateTARGET()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            DateTime now = DateTime.Parse(DateTime.Now.ToString());
            sb.Append(@" update TB_H_M_DECLARATION_TARGET
                        set 
                         APPROVE_STATUS = @APPROVE_STATUS
                        ,RELEASE_DT= @RELEASE_DT
                        ,UPDATED_BY= @UPDATED_BY
                        ,UPDATED_DT = @UPDATED_DT
                        ,FUNC_ID=@FUNC_ID
                        where DECLARA_YEAR = @DECLARA_YEAR 
                        and EMP_ID = @EMP_ID
                    ");

            //修改值
            ht.Add("@APPROVE_STATUS", "P");
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

    //新增
    internal void insertData(DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" INSERT INTO TB_S_M_ASSESS_REMARK
                         (ASSESS_YEAR,ASSESS_TYPE,EMP_ID,REMARK
                          ,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID
                         )
                        VALUES
                        (@ASSESS_YEAR,@ASSESS_TYPE,@EMP_ID,@REMARK
                          ,@CREATED_BY,@CREATED_DT,@UPDATED_BY,@UPDATED_DT,@FUNC_ID
                         )
                    ");

            ht.Add("@ASSESS_YEAR", DECLARA_YEAR);
            ht.Add("@ASSESS_TYPE", APPROVE_STATUS);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@REMARK", REMARK);

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


    #endregion

     

    #region 抽單

    //修改 自我申告對象檔
    public void updateTARGET(string submitFlag)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            DateTime now = DateTime.Parse(DateTime.Now.ToString());
            sb.Append(@" update TB_H_M_DECLARATION_TARGET
                        set 
                         APPROVE_STATUS = @APPROVE_STATUS
                        ,APPROVE_DT = null
                        ,APPROVE_BY = ''
                        ,RELEASE_DT= null
                        ,UPDATED_BY= @UPDATED_BY
                        ,UPDATED_DT = @UPDATED_DT
                        ,FUNC_ID=@FUNC_ID
                        where DECLARA_YEAR = @DECLARA_YEAR 
                        and EMP_ID = @EMP_ID
                    ");

            //修改值
            ht.Add("@APPROVE_STATUS", submitFlag);//Y:核可, B:駁回, N:未核可(抽單)
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

    //抽單時,修改本次的 主管簽核意見檔,簽核狀態為空白
    public void updateCOMMENT(string approve_status)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            DateTime now = DateTime.Parse(DateTime.Now.ToString());
            sb.Append(@" update TB_H_M_DECLARATION_COMMENT
                        set APPROVE_STATUS=@APPROVE_STATUS
                        ,UPDATED_BY = @UPDATED_BY, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID
                        where DECLARA_YEAR=@DECLARA_YEAR and EMP_ID=@EMP_ID and SEQ=@SEQ
                    ");
            //PK值
            ht.Add("@DECLARA_YEAR", DECLARA_YEAR);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SEQ", SEQ);

            ht.Add("@APPROVE_STATUS", approve_status);
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


    //抽單時,新增 主管簽核意見檔,簽核狀態為N
    public void insertCOMMENT()
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

            ht.Add("@APPROVE_STATUS", 'N');
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