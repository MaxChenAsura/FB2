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
/// CFB2SE1300DAO 的摘要描述
/// </summary>
public class CFB2SE1300DAO : BaseDAO
{

    public string EMP_ID { get; set; }
    public string EFFECT_YM { get; set; }
    public string EMP_NAME { get; set; }
    public string RELEASE_DT { get; set; }
    public string SUB_DESC { get; set; }
    public string APPROVE_NAME { get; set; }
    public string APPROVE_DT { get; set; }
    public string CREATED_BY { get; set; }
    public string UPDATED_BY { get; set; }
    public string FUNC_ID { get; set; }

    public CFB2SE1300DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    //Gridview 查詢資料
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string EFFECT_YM)
    {
        try
        {

            if (sortExpression.Contains("RELEASE_NAME"))
            {
                sortExpression = sortExpression.Replace("RELEASE_NAME", "a.RELEASE_BY");
            }
            if (sortExpression.Contains("APPROVE_NAME"))
            {
                sortExpression = sortExpression.Replace("APPROVE_NAME", "e.EMP_NAME");
            }

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" a.* , a.RELEASE_BY + '-' + b.EMP_NAME AS RELEASE_NAME,d.SUB_CD + '-' + d.SUB_DESC AS SUB_DESC,e.EMP_NAME AS APPROVE_NAME									");
            sb.Append(" from TB_S_M_SALARY_ADJ_H a	");
            sb.Append(" left join TB_H_M_EMP b on b.EMP_ID = a.RELEASE_BY 														");
            sb.Append(" left join TB_9_M_COMM_D d on d.SYS_CD='SA' and d.MAIN_CD='APPROVE_STATUS' and d.sub_cd=a.APPROVE_STATUS	");
            sb.Append(" left join TB_H_M_EMP e on e.EMP_ID = a.APPROVE_BY 														");
            sb.Append(" where 1=1																								");
            //有年月
            if (!string.IsNullOrEmpty(EFFECT_YM))
            {
                sb.Append(" and a.EFFECT_YM = @EFFECT_YM ");
                ht.Add("@EFFECT_YM", EFFECT_YM.Replace("/",""));
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
    public DataTable getData_DT(string EFFECT_YM)
    {
        try
        {

            //if (sortExpression.Contains("LEVEL_CD"))
            //{
            //    sortExpression = sortExpression.Replace("LEVEL_CD", "a.LEVEL_CD");
            //}

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select t.*,b.EMP_NAME as RELEASE_NAME,d.sub_desc,e.EMP_NAME as APPROVE_NAME");
            sb.Append(" from TB_S_M_SALARY_ADJ_H t ");
            sb.Append(" left join TB_H_M_EMP b on b.EMP_ID = t.RELEASE_BY									");
            sb.Append(" left join TB_9_M_COMM_D d on d.SYS_CD='SA' and d.MAIN_CD='APPROVE_STATUS' and d.sub_cd=t.APPROVE_STATUS																			");
            sb.Append(" left join TB_H_M_EMP e on e.EMP_ID = t.APPROVE_BY 														");
            sb.Append(" where 1=1																								");
            //有年月
            if (!string.IsNullOrEmpty(EFFECT_YM))
            {
                sb.Append(" and t.EFFECT_YM = @EFFECT_YM ");
                ht.Add("@EFFECT_YM", EFFECT_YM.Replace("/", ""));
            }
           

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    //Gridview 查詢總筆數
    public int getCount(int startRowIndex, int maximumRows, string EFFECT_YM)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select COUNT(*) total_record ");
            sb.Append(" from TB_S_M_SALARY_ADJ_H a");
            sb.Append(" where 1 = 1 ");
            //有年月
            if (!string.IsNullOrEmpty(EFFECT_YM))
            {
                sb.Append(" and a.EFFECT_YM = @EFFECT_YM ");
                ht.Add("@EFFECT_YM", EFFECT_YM.Replace("/", ""));
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

    public DataTable getData1(int startRowIndex, int maximumRows, string sortExpression, string qdatakey)
    {
        try
        {
            sortExpression = string.Format("a.{0}", sortExpression);
            if (sortExpression.Contains("a.EMP_NAME"))
            {
                sortExpression = sortExpression.Replace("a.EMP_NAME", "c.EMP_NAME");
            }

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber");
            sb.Append(" , SUBSTRING(a.EFFECT_YM,1,4) + '/' + SUBSTRING(a.EFFECT_YM,5,2) AS EFFECT_YM, a.EMP_ID, a.JPN_CD, a.DEPT_NO, a.LEVEL_CD, a.GRADE_CD, a.PJOB_CD, a.LEVEL_PAY_OLD, a.ABILITY_PAY_OLD, a.EXAMINE_ADJ");
            sb.Append(" , a.LEVEL_ADJ, a.ABILITY_ADJ, a.LEVEL_PAY_NEW, a.ABILITY_PAY_NEW, a.LEVEL_PAY_DIFF, a.NOPAYDIFF_YN, a.THIS_YEAR_GRADE,  convert(bit,case when a.APPROVE_MARK ='y' then  1 else 0 end) as APPROVE_MARK");
            sb.Append(" , a.CREATED_BY, a.CREATED_DT, a.UPDATED_BY, a.UPDATED_DT, a.FUNC_ID");
            sb.Append(" , convert(bit,case when a.APPROVE_MARK ='y' then  1 else 0 end) as IS_APPROVE_MARK, convert(bit,case when a.NOPAYDIFF_YN ='y' then  1 else 0 end) as IS_NOPAYDIFF_YN, a.CHG_STATUS + '-' + b.SUB_DESC as CHG_STATUS,c.EMP_NAME,d.DEPT_NAME_20,d.DEPT_NAME_30,d.DEPT_NAME_40 ");
            sb.Append(" from TB_S_M_SALARY_ADJ_D a");
            sb.Append(" left join TB_9_M_COMM_D b on b.SYS_CD='SA' and b.MAIN_CD='CHG_STATUS' and b.sub_cd=a.CHG_STATUS	");
            sb.Append(" left join TB_H_M_EMP c on c.EMP_ID=a.EMP_ID");
            sb.Append(" left join VW_H_DEPT_DATA d on a.DEPT_NO=d.DEPT_NO");
            sb.Append(" where EFFECT_YM = @EFFECT_YM");
            sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar)");


            ht.Add("@EFFECT_YM", qdatakey);
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
    public int getCount1(int startRowIndex, int maximumRows, string qdatakey)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select COUNT(*) total_record ");
            sb.Append(" from TB_S_M_SALARY_ADJ_D a");
            sb.Append(" where EFFECT_YM = @EFFECT_YM ");
            ht.Add("@EFFECT_YM", qdatakey);

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

    public DataTable getData2(int startRowIndex, int maximumRows, string sortExpression, string qdatakey)
    {
        try
        {

            //if (sortExpression.Contains("LEVEL_CD"))
            //{
            //    sortExpression = sortExpression.Replace("LEVEL_CD", "a.LEVEL_CD");
            //}

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" a.*, convert(bit,case when a.APPROVE_MARK ='y' then  1 else 0 end) as IS_APPROVE_MARK ");
            sb.Append(" from TB_S_M_SALARYSET_D a");
            sb.Append(" where EFFECT_YM = @EFFECT_YM");
            sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar)");


            ht.Add("@EFFECT_YM", qdatakey);
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
    public int getCount2(int startRowIndex, int maximumRows, string qdatakey)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select COUNT(*) total_record ");
            sb.Append(" from TB_S_M_SALARYSET_D a");
            sb.Append(" where EFFECT_YM = @EFFECT_YM ");
            ht.Add("@EFFECT_YM", qdatakey);

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

    public DataTable getData3(int startRowIndex, int maximumRows, string sortExpression, string qdatakey)
    {
        try
        {

            //if (sortExpression.Contains("LEVEL_CD"))
            //{
            //    sortExpression = sortExpression.Replace("LEVEL_CD", "a.LEVEL_CD");
            //}

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" a.*, convert(bit,case when a.APPROVE_MARK ='y' then  1 else 0 end) as IS_APPROVE_MARK  ");
            sb.Append(" ,CASE WHEN PJOB_TYPE = 'M' then '管理職' WHEN PJOB_TYPE = 'P' then '專業職' ELSE '' END as PJOB_NAME  ");
            sb.Append(" from TB_S_M_2BSALARY_SET_D a");
            sb.Append(" where EFFECT_YM = @EFFECT_YM");
            sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar)");


            ht.Add("@EFFECT_YM", qdatakey);
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
    public int getCount3(int startRowIndex, int maximumRows, string qdatakey)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select COUNT(*) total_record ");
            sb.Append(" from TB_S_M_2BSALARY_SET_D a");
            sb.Append(" where EFFECT_YM = @EFFECT_YM ");
            ht.Add("@EFFECT_YM", qdatakey);

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
    ////明細頁面 Gridview 查詢資料
    //public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string qdatakey)
    //{
    //    try
    //    {

    //        //if (sortExpression.Contains("LEVEL_CD"))
    //        //{
    //        //    sortExpression = sortExpression.Replace("LEVEL_CD", "a.LEVEL_CD");
    //        //}

    //        StringBuilder sb = new StringBuilder();
    //        Hashtable ht = new Hashtable();
    //        sb.Append(" Select * From");
    //        sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
    //        sb.Append(" GRADE_CD,REPLACE(CONVERT(char(10), a.START_DT, 120),'-','/') START_DT,REPLACE(CONVERT(char(10), a.END_DT, 120),'-','/') END_DT, REMARK");
    //        sb.Append(" from TB_H_M_LEVEL_GRADE a");
    //        sb.Append(" where 1 = 1 ");
    //        sb.Append(" and LEVEL_CD = @LEVEL_CD and IS_VALID = 'Y' ");
    //        sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
    //        sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar)");


    //        ht.Add("@startRowIndex", startRowIndex);
    //        ht.Add("@maximumRows", maximumRows);
    //        ht.Add("@LEVEL_CD", qdatakey);
    //        return dbConn.Query(sb, ht);
    //    }
    //    catch
    //    {
    //        throw;
    //    }
    //}
    ////明細頁面Gridview 查詢總筆數
    //public int getCount(int startRowIndex, int maximumRows, string qdatakey)
    //{
    //    try
    //    {
    //        int t = 0;
    //        StringBuilder sb = new StringBuilder();
    //        Hashtable ht = new Hashtable();
    //        sb.Append("Select COUNT(*) total_record ");
    //        sb.Append(" from TB_H_M_LEVEL_GRADE a");
    //        sb.Append(" where 1 = 1 ");
    //        sb.Append(" and LEVEL_CD = @LEVEL_CD and IS_VALID = 'Y' ");
    //        ht.Add("@LEVEL_CD", qdatakey);


    //        DataTable dt = dbConn.Query(sb, ht);
    //        if (dt.Rows.Count > 0)
    //        {
    //            t = (int)dt.Rows[0]["total_record"];
    //        }


    //        return t;


    //    }
    //    catch (Exception)
    //    {

    //        throw;
    //    }

    //}

    internal System.Data.DataTable getLevelCD(string start_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select LEVEL_CD from TB_H_M_LEVEL Where @START_DT >= START_DT and @START_DT <= END_DT order by LEVEL_CD");
            ht.Add("@START_DT", start_dt);
            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }
    internal System.Data.DataTable check_LEVEL_CD(string LEVEL_CD)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select COUNT(*) as counts from TB_H_M_LEVEL_GRADE");
            sb.Append(" where LEVEL_CD = @LEVEL_CD");
            sb.Append(" and getdate() between START_DT and  END_DT");
            ht.Add("@LEVEL_CD", LEVEL_CD);
            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }



    internal void updateData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" UPDATE       TB_S_M_SALARY_ADJ_H");
            sb.Append(" SET                APPROVE_STATUS = 'N',RELEASE_DT = GETDATE(), RELEASE_BY = @EMP_ID, UPDATED_BY = @EMP_ID, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID");
            sb.Append(" WHERE        (EFFECT_YM = @EFFECT_YM)");
            sb.Append(" ");
            sb.Append(" UPDATE       TB_S_M_SALARYSET_H");
            sb.Append(" SET                RELEASE_DT = GETDATE(), RELEASE_BY = @EMP_ID, UPDATED_BY = @EMP_ID, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID");
            sb.Append(" WHERE        (EFFECT_YM = @EFFECT_YM)");
            sb.Append(" ");
            sb.Append(" UPDATE       TB_S_M_2BSALARY_SET_H");
            sb.Append(" SET                RELEASE_DT = GETDATE(), RELEASE_BY = @EMP_ID, UPDATED_BY = @EMP_ID, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID");
            sb.Append(" WHERE        (EFFECT_YM = @EFFECT_YM)");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@FUNC_ID", FUNC_ID);
            ht.Add("@EFFECT_YM", EFFECT_YM);


            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }







}