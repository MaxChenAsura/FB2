using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;
using System.Text;
using System.Collections;

/// <summary>
/// CFB2PA0100DAO 的摘要描述
/// </summary>
public class CFB2PA0100DAO : BaseDAO
{
    public CFB2PA0100DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public string BARCODE_NO { get; set; }
    public string EMP_ID { get; set; }
    public decimal PERSONNEL { get; set; }
    public decimal AMT_TENS { get; set; }
    public decimal AMT_DIGITS { get; set; }
    public decimal TIME_TENS { get; set; }
    public decimal TIME_DIGITS { get; set; }
    public decimal SPACE_SCORE { get; set; }
    public decimal DISCOUNT_RATE { get; set; }
    public decimal BONUS_TOT_TENS { get; set; }
    public decimal BONUS_TOT_DIGITS { get; set; }
    public string SIGN_G { get; set; }
    public string SIGN_ROOM { get; set; }
    public string SIGN_M { get; set; }
    public string SIGN_AFFAIRS { get; set; }
    public string SIGN_CHAIRMAN { get; set; }
    public string YM { get; set; }
    public decimal EFFECT_SCORE { get; set; }
    public decimal DISCOUNT_SCORE { get; set; }
    public decimal EFFECT_FINAL { get; set; }
    public string DEPT_NO_20 { get; set; }
    public string DEPT_NO_30 { get; set; }
    public string DEPT_NO_40 { get; set; }
    public string DEPT_NO { get; set; }
    public decimal PEO_DIGITAL { get; set; }
    public decimal AMT_DIGITAL { get; set; }
    public decimal TIME_DIGITAL { get; set; }
    public decimal SPACE_DIGITAL { get; set; }
    public decimal BONUS_SCR_FIRST { get; set; }
    public decimal BONUS_SCR_FINAL { get; set; }
    public string GRADE_CD { get; set; }
    public decimal GROUP_INTEGRAL { get; set; }
    public decimal PRO_BONUS { get; set; }
    public string IS_YN { get; set; }
    public string SALARY_YM { get; set; }
    public string WS_CD { get; set; }
    public string LEVEL_CD { get; set; }
    public string EMP_CD { get; set; }
    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }


    //檢查條碼編號不可存在<<提案資料檔>>
    public bool IsExitProposalData(string barcodeNo)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@"SELECT  
                                  BARCODE_NO
                                  FROM TB_P_M_PROPOSAL_DATA
                                where BARCODE_NO= @BARCODE_NO AND YM<>@YM
                                ");

            ht.Add("@BARCODE_NO", barcodeNo);
            ht.Add("@YM", YM);

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                return false;
            }
           /// return true;
        }
        catch (Exception)
        {

            throw;
        }
    }
    public DataTable getLastCloseYm()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select  top 1 YM   ");
            sb.Append("FROM TB_P_M_CLOSE_YM  ");
            sb.Append("ORDER BY YM DESC ");

            DataTable dt = dbConn.Query(sb, ht);


            return dt;
        }
        catch
        {
            throw;
        }
    }
    public DataTable empData(string empId)
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select  *  ");
            sb.Append("FROM VW_H_EMP_DATA  ");
            sb.Append("where EMP_ID=@EMP_ID ");
            ht.Add("@EMP_ID", empId);

            DataTable dt = dbConn.Query(sb, ht);


            return dt;
        }
        catch
        {
            throw;
        }
    }
    public DataTable getSubData(string type)
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select  *  ");
            sb.Append("FROM TB_9_M_COMM_D  ");
            sb.Append("where SYS_CD='PR' ");
            sb.Append(" AND MAIN_CD=@MAIN_CD AND IS_VALID='Y' ");
            ht.Add("@MAIN_CD", type);

            DataTable dt = dbConn.Query(sb, ht);


            return dt;
        }
        catch
        {
            throw;
        }
    }
    public DataTable getPJOBData()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(@"select PJOB_CD,PJOB_FLOW_LEVEL  
                                from tb_h_m_PJOB
                                WHERE GETDATE() BETWEEN START_DT AND END_DT ");

            DataTable dt = dbConn.Query(sb, ht);


            return dt;
        }
        catch
        {
            throw;
        }
    }
    public DataTable getEvaluationSetData()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(@"select *
                                from TB_P_M_EVALUATION_SET
                                 ");

            DataTable dt = dbConn.Query(sb, ht);


            return dt;
        }
        catch
        {
            throw;
        }
    }
    

    public void DeleteByYM(){
          try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@"DELETE FROM TB_P_M_PROPOSAL_DATA WHERE YM=@YM");
            ht.Add("@YM", YM );
            dbConn.ExecuteT(sb, ht, true);
          }
        catch (Exception)
        {

            throw;
        }
    }

    //提案資料一括更新
    public void Insert_ALL()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(@" INSERT INTO  TB_P_M_PROPOSAL_DATA 
                                    (BARCODE_NO, EMP_ID, PERSONNEL, AMT_TENS, AMT_DIGITS, TIME_TENS, TIME_DIGITS, SPACE_SCORE,
                                     DISCOUNT_RATE, BONUS_TOT_TENS, BONUS_TOT_DIGITS, SIGN_G, SIGN_ROOM, SIGN_M, SIGN_AFFAIRS, SIGN_CHAIRMAN,                                    
                                     YM, EFFECT_SCORE, DISCOUNT_SCORE, EFFECT_FINAL, DEPT_NO_20, DEPT_NO_30, DEPT_NO_40, DEPT_NO,
                                     PEO_DIGITAL, AMT_DIGITAL, TIME_DIGITAL, SPACE_DIGITAL, BONUS_SCR_FIRST, BONUS_SCR_FINAL, GRADE_CD, GROUP_INTEGRAL,
                                     PRO_BONUS, IS_YN, SALARY_YM, WS_CD, LEVEL_CD, EMP_CD, CREATED_BY, CREATED_DT, UPDATED_BY, UPDATED_DT, FUNC_ID)VALUES(
                                     @BARCODE_NO, @EMP_ID, @PERSONNEL, @AMT_TENS, @AMT_DIGITS, @TIME_TENS, @TIME_DIGITS, @SPACE_SCORE,
                                     @DISCOUNT_RATE, @BONUS_TOT_TENS, @BONUS_TOT_DIGITS, @SIGN_G, @SIGN_ROOM, @SIGN_M, @SIGN_AFFAIRS, @SIGN_CHAIRMAN,                                    
                                     @YM, @EFFECT_SCORE, @DISCOUNT_SCORE, @EFFECT_FINAL, @DEPT_NO_20, @DEPT_NO_30, @DEPT_NO_40, @DEPT_NO,
                                     @PEO_DIGITAL, @AMT_DIGITAL, @TIME_DIGITAL, @SPACE_DIGITAL, @BONUS_SCR_FIRST, @BONUS_SCR_FINAL, @GRADE_CD, @GROUP_INTEGRAL,
                                     @PRO_BONUS, @IS_YN, @SALARY_YM, @WS_CD, @LEVEL_CD, @EMP_CD, @CREATED_BY, GETDATE(), @UPDATED_BY,  GETDATE(),@FUNC_ID
                                )");
        
            ht.Add("@BARCODE_NO", BARCODE_NO );
            ht.Add("@EMP_ID", EMP_ID );
            ht.Add("@PERSONNEL", PERSONNEL );
            ht.Add("@AMT_TENS", AMT_TENS );
            ht.Add("@AMT_DIGITS", AMT_DIGITS );
            ht.Add("@TIME_TENS", TIME_TENS );
            ht.Add("@TIME_DIGITS", TIME_DIGITS );
            ht.Add("@SPACE_SCORE", SPACE_SCORE );
            ht.Add("@DISCOUNT_RATE", DISCOUNT_RATE );
            ht.Add("@BONUS_TOT_TENS", BONUS_TOT_TENS );
            ht.Add("@BONUS_TOT_DIGITS", BONUS_TOT_DIGITS );
            ht.Add("@SIGN_G", SIGN_G );
            ht.Add("@SIGN_ROOM", SIGN_ROOM );
            ht.Add("@SIGN_M", SIGN_M );
            ht.Add("@SIGN_AFFAIRS", SIGN_AFFAIRS );
            ht.Add("@SIGN_CHAIRMAN", SIGN_CHAIRMAN );
            ht.Add("@YM", YM );
            ht.Add("@EFFECT_SCORE", EFFECT_SCORE );
            ht.Add("@DISCOUNT_SCORE", DISCOUNT_SCORE );
            ht.Add("@EFFECT_FINAL", EFFECT_FINAL );
            ht.Add("@DEPT_NO_20", DEPT_NO_20 );
            ht.Add("@DEPT_NO_30", DEPT_NO_30 );
            ht.Add("@DEPT_NO_40", DEPT_NO_40 );
            ht.Add("@DEPT_NO", DEPT_NO );
            ht.Add("@PEO_DIGITAL", PEO_DIGITAL );
            ht.Add("@AMT_DIGITAL", AMT_DIGITAL );
            ht.Add("@TIME_DIGITAL", TIME_DIGITAL );
            ht.Add("@SPACE_DIGITAL", SPACE_DIGITAL );
            ht.Add("@BONUS_SCR_FIRST", BONUS_SCR_FIRST );
            ht.Add("@BONUS_SCR_FINAL", BONUS_SCR_FINAL );
            ht.Add("@GRADE_CD", GRADE_CD );
            ht.Add("@GROUP_INTEGRAL", GROUP_INTEGRAL );
            ht.Add("@PRO_BONUS", PRO_BONUS );
            ht.Add("@IS_YN", IS_YN );
            ht.Add("@SALARY_YM", SALARY_YM );
            ht.Add("@WS_CD", WS_CD );
            ht.Add("@LEVEL_CD", LEVEL_CD );
            ht.Add("@EMP_CD", EMP_CD );
            ht.Add("@CREATED_BY", CREATED_BY );
            ht.Add("@UPDATED_BY", UPDATED_BY );
            
            ht.Add("@FUNC_ID", "FB2PA010");

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }
    //LOG檔
    public void Insert_Log(string empId,string sourceForm,string changeDesc)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(@" INSERT INTO  TB_P_M_UPLOAD_LOG 
                                    (EMP_ID, PROCESS_DT, SOURCE_FORM, CHANGE_DESC)VALUES(
                                     @EMP_ID, GETDATE(), @SOURCE_FORM, @CHANGE_DESC )");

            ht.Add("@EMP_ID", empId);
            ht.Add("@SOURCE_FORM",sourceForm);
            ht.Add("@CHANGE_DESC", changeDesc );

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }
    public DataTable getLastLog(string sourceForm,string empId)
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select  top 1 CHANGE_DESC  ");
            sb.Append("FROM TB_P_M_UPLOAD_LOG  ");
            sb.Append("WHERE SOURCE_FORM=@SOURCE_FORM AND EMP_ID=@EMP_ID ");
            sb.Append("ORDER BY PROCESS_DT DESC ");
            ht.Add("@SOURCE_FORM", sourceForm);
            ht.Add("@EMP_ID", EMP_ID);

            DataTable dt = dbConn.Query(sb, ht);


            return dt;
        }
        catch
        {
            throw;
        }
    }
}