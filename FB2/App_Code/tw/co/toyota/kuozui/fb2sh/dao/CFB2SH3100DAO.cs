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
/// WFB2SH3100 的摘要描述
/// </summary>
public class CFB2SH3100DAO : BaseDAO
{
    ///<summary>
    ///職務代號
    ///</summary>
    public string PJOB_CD { get; set; }

    ///<summary>
    ///年獎考績
    ///</summary>
    public string AWARD { get; set; }

    ///<summary>
    ///年獎格差
    ///</summary>
    public decimal? AWARD_DIFFER { get; set; }

    ///<summary>
    ///年獎格差說明
    ///</summary>
    public string AWARD_DESC { get; set; }

    ///<summary>
    ///年資起
    ///</summary>
    public decimal? YEAR_S { get; set; }

    ///<summary>
    ///年資訖
    ///</summary>
    public decimal? YEAR_E { get; set; }

    ///<summary>
    ///基準獎金
    ///</summary>
    public decimal? BONUS_BASE { get; set; }

    ///<summary>
    ///新增人員
    ///</summary>
    public string CREATED_BY { get; set; }

    ///<summary>
    ///新增日期時間
    ///</summary>
    public DateTime? CREATED_DT { get; set; }

    ///<summary>
    ///更新人員
    ///</summary>
    public string UPDATED_BY { get; set; }

    ///<summary>
    ///更新日期時間
    ///</summary>
    public DateTime? UPDATED_DT { get; set; }

    ///<summary>
    ///更新作業FunctionID
    ///</summary>
    public string FUNC_ID { get; set; }



    public CFB2SH3100DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public DataTable getAward_Data(int startRowIndex, int maximumRows, string sortExpression, string pjob_cd)
    {
        try
        {
            if (sortExpression.Contains("PJOB_CD"))
                sortExpression = sortExpression.Replace("PJOB_CD", "A.PJOB_CD");

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * From");
            sb.Append(" (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(@"  A.PJOB_CD,B.PJOB_DESC,A.AWARD,A.AWARD_DIFFER,A.AWARD_DESC
                                from TB_S_M_FR_AWARD A with (nolock) LEFT JOIN
                                     TB_H_M_PJOB B ON A.PJOB_CD=B.PJOB_CD 
                                     ");
            sb.Append(" where 1=1 ");

            if (pjob_cd != "")
            {
                sb.Append(" and A.PJOB_CD  LIKE '%'+@PJOB_CD+'%' ");
                ht.Add("@PJOB_CD", pjob_cd);
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

    public int getAwardCount(int startRowIndex, int maximumRows, string pjob_cd)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select COUNT(*) total_record from TB_S_M_FR_AWARD A ");
            sb.Append(" where 1=1 ");

            if (pjob_cd != "")
            {
                sb.Append(" and PJOB_CD  LIKE '%'+@PJOB_CD+'%' ");
                ht.Add("@PJOB_CD", pjob_cd);
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
    public DataTable getExistAwardData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select A.PJOB_CD,A.AWARD,A.AWARD_DIFFER,A.AWARD_DESC  from TB_S_M_FR_AWARD A ");
            sb.Append(" WHERE A.PJOB_CD  = @PJOB_CD ");
            ht.Add("@PJOB_CD", PJOB_CD);

            return dbConn.Query(sb, ht);
        }
        catch (Exception ex)
        {

            throw;
        }
    }
    //取得修改資料
    public DataTable getUpdAwardData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" Select 
                                    A.PJOB_CD,A.AWARD,A.AWARD_DIFFER,A.AWARD_DESC  from TB_S_M_FR_AWARD A ");
                                
           
            sb.Append(@" 
                where 1=1 AND      A.PJOB_CD  = @PJOB_CD 
            ");

            ht.Add("@PJOB_CD", PJOB_CD);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //新增 TB_S_M_FR_AWARD
    public void addAwardITEM()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("insert into TB_S_M_FR_AWARD  ( ");
            sb.Append(" PJOB_CD,AWARD,AWARD_DIFFER,AWARD_DESC ");
            sb.Append(" ,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" values ( ");
            sb.Append(" @PJOB_CD,@AWARD,@AWARD_DIFFER,@AWARD_DESC ");
            sb.Append(" ,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@PJOB_CD", PJOB_CD);//VARCHAR
            ht.Add("@AWARD", AWARD);//VARCHAR
            ht.Add("@AWARD_DIFFER", AWARD_DIFFER);//DECIMAL
            ht.Add("@AWARD_DESC", AWARD_DESC);//NVARCHAR

            ht.Add("@CREATED_BY", CREATED_BY);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //更新 TB_S_M_FR_AWARD
    public void updateAwardITEM()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_S_M_FR_AWARD ");
            sb.Append(" set  AWARD = @AWARD, AWARD_DIFFER = @AWARD_DIFFER, AWARD_DESC = @AWARD_DESC,  ");
            sb.Append(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE()");
            sb.Append(" where PJOB_CD = @PJOB_CD  ");


           
            ht.Add("@AWARD", AWARD);//VARCHAR
            ht.Add("@AWARD_DIFFER", AWARD_DIFFER);//DECIMAL
            ht.Add("@AWARD_DESC", AWARD_DESC);//NVARCHAR

            ht.Add("@CREATED_BY", CREATED_BY);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            ht.Add("@PJOB_CD", PJOB_CD);//VARCHAR

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //刪除 TB_S_M_FR_AWARD
    public void deleteAwardITEM(string pjobCd,string award)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //寫log
            sb.AppendLine(" update TB_S_M_FR_AWARD set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2SH3100' ");
            sb.Append(" where PJOB_CD = @PJOB_CD  AND AWARD=@AWARD ");
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);
            ht.Add("@PJOB_CD", pjobCd);//VARCHAR
            ht.Add("@AWARD", award);//VARCHAR


            sb.Append(" delete from TB_S_M_FR_AWARD ");
            sb.Append(" where PJOB_CD = @PJOB_CD AND AWARD=@AWARD  ");
            //ht.Add("@ASSESS_TYPE", assess_type);
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

    public DataTable getBaseBouns_Data(int startRowIndex, int maximumRows, string sortExpression, string pjob_cd)
    {
        try
        {
            if (sortExpression.Contains("PJOB_CD"))
                sortExpression = sortExpression.Replace("PJOB_CD", "A.PJOB_CD");

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * From");
            sb.Append(" (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(@"  A.PJOB_CD,A.YEAR_S = @YEAR_S, YEAR_E = @YEAR_E, BONUS_BASE = @BONUS_BASE
                                from TB_S_M_FR_BASEBONUS A with (nolock)  LEFT JOIN
                                     TB_H_M_PJOB B ON A.PJOB_CD=B.PJOB_CD 
                                     ");
            sb.Append(" where 1=1 ");

            if (pjob_cd != "")
            {
                sb.Append(" and A.PJOB_CD  LIKE '%'+@PJOB_CD+'%' ");
                ht.Add("@PJOB_CD", pjob_cd);
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

    public int getBaseBounsCount(int startRowIndex, int maximumRows, string pjob_cd)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select COUNT(*) total_record from TB_S_M_FR_BASEBONUS A ");
            sb.Append(" where 1=1 ");

            if (pjob_cd != "")
            {
                sb.Append(" and PJOB_CD  LIKE '%'+@PJOB_CD+'%' ");
                ht.Add("@PJOB_CD", pjob_cd);
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
    public DataTable getExistBaseBounsData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select A.PJOB_CD,A.YEAR_S,A.YEAR_E,A.BONUS_BASE  from TB_S_M_FR_BASEBONUS A ");
            sb.Append(" WHERE A.PJOB_CD  = @PJOB_CD ");
            ht.Add("@PJOB_CD", PJOB_CD);

            return dbConn.Query(sb, ht);
        }
        catch (Exception ex)
        {

            throw;
        }
    }
    //取得修改資料
    public DataTable getUpdBaseBounsData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" Select 
                                    A.PJOB_CD,A.YEAR_S,A.YEAR_E,A.BONUS_BASE  from TB_S_M_FR_BASEBONUS A ");


            sb.Append(@" 
                where 1=1 AND      A.PJOB_CD  = @PJOB_CD 
            ");

            ht.Add("@PJOB_CD", PJOB_CD);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //新增 TB_S_M_FR_BASEBONUS
    public void addBaseBounsITEM()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("insert into TB_S_M_FR_BASEBONUS  ( ");
            sb.Append(" PJOB_CD,YEAR_S,YEAR_E,BONUS_BASE ");
            sb.Append(" ,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" values ( ");
            sb.Append(" @PJOB_CD,@YEAR_S,@YEAR_E,@BONUS_BASE ");
            sb.Append(" ,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@PJOB_CD", PJOB_CD);//VARCHAR
            ht.Add("@YEAR_S" , YEAR_S );//DECIMAL
            ht.Add("@YEAR_E", YEAR_E);//DECIMAL
            ht.Add("@BONUS_BASE", BONUS_BASE);//DECIMAL


            ht.Add("@CREATED_BY", CREATED_BY);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //更新 TB_S_M_FR_BASEBONUS
    public void updateBaseBounsITEM()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_S_M_FR_BASEBONUS ");
            sb.Append(" set YEAR_S = @YEAR_S, YEAR_E = @YEAR_E, BONUS_BASE = @BONUS_BASE   ");
            sb.Append(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE()");
            sb.Append(" where PJOB_CD = @PJOB_CD  ");



            ht.Add("@YEAR_S", YEAR_S);//DECIMAL
            ht.Add("@YEAR_E", YEAR_E);//DECIMAL
            ht.Add("@BONUS_BASE", BONUS_BASE);//DECIMAL

            ht.Add("@CREATED_BY", CREATED_BY);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            ht.Add("@PJOB_CD", PJOB_CD);//VARCHAR

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //刪除 TB_S_M_FR_BASEBONUS
    public void deleteBaseBounsITEM(string pjob_cd, string year_s)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //寫log
            sb.AppendLine(" update TB_S_M_FR_BASEBONUS set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2SH3100' ");
            sb.Append(" where PJOB_CD = @PJOB_CD  ");
            sb.Append(" AND YEAR_S = @PYEAR_S  ");
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);
            ht.Add("@PJOB_CD", pjob_cd);//VARCHAR
            ht.Add("@PJOB_CD", year_s);//VARCHAR

            sb.Append(" delete from TB_S_M_FR_BASEBONUS ");
            sb.Append(" where PJOB_CD = @PJOB_CD   AND YEAR_S = @PYEAR_S ");
            //ht.Add("@ASSESS_TYPE", assess_type);
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
    public DataTable getPJOB_NAME(string pjob_cd)
    {
        try
        {
               StringBuilder sb = new StringBuilder();
                Hashtable ht = new Hashtable();

            sb.Append("Select * from TB_H_M_PJOB where PJOB_CD is not null ");

            if (PJOB_CD != "")
            {
                sb.Append(" and PJOB_CD = @PJOB_CD");
                ht.Add("@PJOB_CD", pjob_cd);
            }
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
}