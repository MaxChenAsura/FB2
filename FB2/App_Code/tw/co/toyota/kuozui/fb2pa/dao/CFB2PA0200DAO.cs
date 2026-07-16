using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using FB2.tw.co.toyota.kuozui.dao;
using System.Text;
using System.Collections;

/// <summary>
/// WFB2PA0200 的摘要描述
/// </summary>
public class CFB2PA0200DAO : BaseDAO
{
    public string BARCODE_NO { get; set; }
    public string EMP_ID { get; set; }
    public string PERSONNEL { get; set; }
    public string AMT_TENS { get; set; }
    public string AMT_DIGITS { get; set; }
    public string TIME_TENS { get; set; }
    public string TIME_DIGITS { get; set; }
    public string SPACE_SCORE { get; set; }
    public string DISCOUNT_RATE { get; set; }
    public string BONUS_TOT_TENS { get; set; }
    public string BONUS_TOT_DIGITS { get; set; }
    public string SIGN_G { get; set; }
    public string SIGN_ROOM { get; set; }
    public string SIGN_M { get; set; }
    public string SIGN_AFFAIRS { get; set; }
    public string SIGN_CHAIRMAN { get; set; }
    public string YM { get; set; }
    public string EFFECT_SCORE { get; set; }
    public string DISCOUNT_SCORE { get; set; }
    public string EFFECT_FINAL { get; set; }
    public string DEPT_NO_20 { get; set; }
    public string DEPT_NO_30 { get; set; }
    public string DEPT_NO_40 { get; set; }
    public string DEPT_NO { get; set; }
    public string PEO_DIGITAL { get; set; }
    public string AMT_DIGITAL { get; set; }
    public string TIME_DIGITAL { get; set; }
    public string SPACE_DIGITAL { get; set; }
    public string BONUS_SCR_FIRST { get; set; }
    public string BONUS_SCR_FINAL { get; set; }
    public string GRADE_CD { get; set; }
    public string GROUP_INTEGRAL { get; set; }
    public string PRO_BONUS { get; set; }
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

    public CFB2PA0200DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string ym, string barcodeNo, string isYn, string empId)
    {
        try
        {
            if (sortExpression.Contains("YM")&&!sortExpression.Contains("SALARY_YM"))
                sortExpression = sortExpression.Replace("YM", "A.YM");
            if (sortExpression.Contains("SALARY_YM"))
                sortExpression = sortExpression.Replace("SALARY_YM", "A.SALARY_YM");
            if (sortExpression.Contains("EMP_ID"))
                sortExpression = sortExpression.Replace("EMP_ID", "A.EMP_ID");
            if (sortExpression.Contains("GRADE_CD"))
                sortExpression = sortExpression.Replace("GRADE_CD", "A.GRADE_CD");

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * From");
            sb.Append(" (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(@" A.YM ,  A.BARCODE_NO, 
                         A.EMP_ID, B.EMP_NAME, A.BONUS_SCR_FINAL, A.PRO_BONUS, 
                         A.GRADE_CD, C.GRADE_NAME, A.IS_YN+'-'+D.SUB_DESC AS IS_YN_DESC ,
                         A.SALARY_YM 
                         from TB_P_M_PROPOSAL_DATA A with (nolock)
                         left join VW_H_EMP_DATA B  with (nolock)  on A.EMP_ID=B.EMP_ID 
                         left join TB_P_M_EVALUATION_SET C  with (nolock)  on A.GRADE_CD=C.GRADE_CD
                         left join TB_9_M_COMM_D D  with (nolock)  on D.SYS_CD='99' and D.MAIN_CD='IS_YN' and D.SUB_CD= A.IS_YN and D.IS_VALID='Y' ");
            sb.Append(" where 1=1 ");

            if (ym != "")
            {
                sb.Append(" and A.YM = @YM ");
                ht.Add("@YM", ym.Replace("/",""));
            }

            if (barcodeNo != "")
            {
                sb.Append(" and A.BARCODE_NO = @BARCODE_NO ");
                ht.Add("@BARCODE_NO", barcodeNo);
            }
            if (isYn != "-1")
            {
                sb.Append(" and A.IS_YN = @IS_YN ");
                ht.Add("@IS_YN", isYn);
            }
            if (empId != "")
            {
                sb.Append(" and A.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", empId);
            }
            sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar) ");

            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    public int getCount(int startRowIndex, int maximumRows, string ym, string barcodeNo, string isYn, string empId)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select COUNT(*) total_record from TB_P_M_PROPOSAL_DATA A ");
            sb.Append(" where 1=1 ");


            if (ym != "")
            {
                sb.Append(" and A.YM = @YM ");
                ht.Add("@YM", ym.Replace("/", ""));
            }

            if (barcodeNo != "")
            {
                sb.Append(" and A.BARCODE_NO = @BARCODE_NO ");
                ht.Add("@BARCODE_NO", barcodeNo);
            }
            if (isYn != "-1")
            {
                sb.Append(" and A.IS_YN = @IS_YN ");
                ht.Add("@IS_YN", isYn);
            }
            if (empId != "")
            {
                sb.Append(" and A.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", empId);
            }

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                t = (int)dt.Rows[0]["total_record"];
            }
            return t;
        }
        catch (Exception)
        {
            throw;
        }

    }

    //取得現有資料
    public DataTable getExistData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select * from TB_P_M_PROPOSAL_DATA");
            sb.Append(" where BARCODE_NO = @BARCODE_NO ");
            ht.Add("@BARCODE_NO", BARCODE_NO);

            return dbConn.Query(sb, ht);
        }
        catch (Exception ex)
        {

            throw;
        }
    }
    //取得修改資料
    public DataTable getUpdData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" SELECT A.YM ,  A.BARCODE_NO, 
                         A.EMP_ID, B.EMP_NAME, A.BONUS_SCR_FINAL, A.PRO_BONUS, 
                         A.GRADE_CD, C.GRADE_NAME, A.IS_YN+'-'+D.SUB_DESC AS IS_YN_DESC ,
                         A.SALARY_YM  ,A.BONUS_SCR_FIRST, A.GROUP_INTEGRAL,A.IS_YN
                         from TB_P_M_PROPOSAL_DATA A with (nolock)
                         left join VW_H_EMP_DATA B  with (nolock)  on A.EMP_ID=B.EMP_ID 
                         left join TB_P_M_EVALUATION_SET C  with (nolock)  on A.GRADE_CD=C.GRADE_CD
                         left join TB_9_M_COMM_D D  with (nolock)  on D.SYS_CD='99' and D.MAIN_CD='IS_YN' and D.SUB_CD= A.IS_YN and D.IS_VALID='Y' ");
            
           
            sb.Append(@" 
                where 1=1
                and A.BARCODE_NO = @BARCODE_NO 
            ");

            ht.Add("@BARCODE_NO", BARCODE_NO);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //取得提案奬金評價設定檔資料
    public DataTable getEVASetByScore(decimal score)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" SELECT A.GRADE_CD, A.GRADE_NAME, A.BONUS_AMT, A.GROUP_POINT, A.TRANS_KEEP_YN,A.SCORE_S,A.SCORE_E,A.TRANS_KEEP_YN+'_'+B.SUB_DESC AS TRANS_KEEP_YN_DESC
                                 FROM TB_P_M_EVALUATION_SET A with (nolock)
                                             left join TB_9_M_COMM_D B  with (nolock)  on B.SYS_CD='99' and B.MAIN_CD='IS_YN' and B.SUB_CD= A.TRANS_KEEP_YN and B.IS_VALID='Y'
                                 WHERE @SCORE  BETWEEN A.SCORE_S AND A.SCORE_E 
                                           ");




            ht.Add("@SCORE", score);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //更新 TB_P_M_PROPOSAL_DATA
    public void updateData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_P_M_PROPOSAL_DATA ");
            sb.Append(" set BONUS_SCR_FINAL=@BONUS_SCR_FINAL,SALARY_YM=@SALARY_YM, ");
            sb.Append("      GRADE_CD=@GRADE_CD,GROUP_INTEGRAL=@GROUP_INTEGRAL, ");
            sb.Append("      PRO_BONUS=@PRO_BONUS,IS_YN=@IS_YN, ");
            sb.Append(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE()");
            sb.Append(" where BARCODE_NO = @BARCODE_NO  ");

            ht.Add("@BONUS_SCR_FINAL", BONUS_SCR_FINAL);
            ht.Add("@SALARY_YM", SALARY_YM.Replace("/", ""));
            ht.Add("@GRADE_CD", GRADE_CD);
            ht.Add("@GROUP_INTEGRAL", GROUP_INTEGRAL);
            ht.Add("@PRO_BONUS", PRO_BONUS);
            ht.Add("@IS_YN", IS_YN);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@BARCODE_NO", BARCODE_NO);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //刪除 TB_P_M_PROPOSAL_DATA
    public void deleteData(string barcodeNo)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" delete from TB_P_M_PROPOSAL_DATA ");
            sb.Append(" where BARCODE_NO = @BARCODE_NO  ");
            ht.Add("@BARCODE_NO", barcodeNo);
            //ht.Add("@WS_CD", ws_cd);
            //ht.Add("@LEVEL_CD", level_cd);
            //ht.Add("@PJOB_TYPE", pjob_cd);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
   
}