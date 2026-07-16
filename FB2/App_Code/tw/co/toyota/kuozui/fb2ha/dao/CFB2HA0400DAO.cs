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
/// CFB2HA0400DAO 的摘要描述
/// </summary>
public class CFB2HA0400DAO : BaseDAO
{

    public string CREATED_BY { get; set; }
    public string UPDATED_BY { get; set; }
    public string FUNC_ID { get; set; }
    public string PJOB_CD { get; set; }
    public string PJOB_DESC { get; set; }
    public string START_DT { get; set; }
    public string END_DT { get; set; }
    public string LEVEL_CD { get; set; }
    public string WS_CD { get; set; }
    public string MANAGEMENT_ALLOWANCE { get; set; }
    public string PROFESSION_ALLOWANCE { get; set; }
    public string PJOB_AGE_LIMIT { get; set; }
    public string PJOB_LEVEL { get; set; }
    public string PJOB_FLOW_LEVEL { get; set; }
    public string BUSINESS_TRIP_GRP { get; set; }
    public string REMARK { get; set; }

    public CFB2HA0400DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    //Gridview 查詢資料
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string level_cd, string start_dt_s, string start_dt_e, string end_dt_s, string end_dt_e, string is_valid)
    {
        try
        {

            //if (sortExpression.Contains("LEVEL_CD"))
            //{
            //    sortExpression = sortExpression.Replace("LEVEL_CD", "a.LEVEL_CD");
            //}

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(@" Select * From
                            (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,* from ( ");//t1
            
            sb.Append("     Select ");
            sb.Append(@"    LEVEL_CD,REPLACE(CONVERT(char(10), a.START_DT, 120),'-','/') START_DT,REPLACE(CONVERT(char(10), a.END_DT, 120),'-','/') END_DT,
                            LEVEL_PAY, TOP_LEVEL_PAY, ABILITY_PAY_LOW, ABILITY_PAY_MID, ABILITY_PAY_HIGH, IS_UNION_MEMBER, REMARK 
                            from TB_H_M_LEVEL a
                            where 1=1
            ");
            //有選擇資格代號
            if (level_cd != "-1")
            {
                sb.Append(" and a.LEVEL_CD = @LEVEL_CD ");
                ht.Add("@LEVEL_CD", level_cd);
            }
            if (start_dt_s != "")
            {
                sb.AppendLine(" and a.START_DT >= @START_DT_S ");
                ht.Add("@START_DT_S", start_dt_s);
            }
            if (start_dt_e != "")
            {
                sb.AppendLine(" and a.START_DT <= @START_DT_E ");
                ht.Add("@START_DT_E", start_dt_e);
            }

            if (end_dt_s != "")
            {
                sb.AppendLine(" and a.END_DT >= @END_DT_S ");
                ht.Add("@END_DT_S", end_dt_s);
            }
            if (end_dt_e != "")
            {
                sb.AppendLine(" and a.END_DT <= @END_DT_E ");
                ht.Add("@END_DT_E", end_dt_e);
            }

            if (is_valid == "Y")
            {
                sb.AppendLine(" and (a.START_DT <= @CURRENT_DT and a.END_DT >= @CURRENT_DT) ");
                ht.Add("@CURRENT_DT", DateTime.Now.Date.ToString("yyyy/MM/dd"));
            }
            if (is_valid == "N")
            {
                sb.AppendLine(" and (a.START_DT >= @CURRENT_DT  or a.END_DT <= @CURRENT_DT ) ");
                ht.Add("@CURRENT_DT", DateTime.Now.Date.ToString("yyyy/MM/dd"));
            }

            sb.Append("     )t1");
       

            
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
    public int getCount(int startRowIndex, int maximumRows, string level_cd, string start_dt_s, string start_dt_e, string end_dt_s, string end_dt_e, string is_valid)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select COUNT(*) total_record ");
            sb.Append(" from TB_H_M_LEVEL a  ");
            sb.Append(" where 1 = 1 ");
            //有選擇資格代號
            if (level_cd != "-1")
            {
                sb.Append(" and a.LEVEL_CD = @LEVEL_CD ");
                ht.Add("@LEVEL_CD", level_cd);
            }
            if (start_dt_s != "")
            {
                sb.AppendLine(" and a.START_DT >= @START_DT_S ");
                ht.Add("@START_DT_S", start_dt_s);
            }
            if (start_dt_e != "")
            {
                sb.AppendLine(" and a.START_DT <= @START_DT_E ");
                ht.Add("@START_DT_E", start_dt_e);
            }

            if (end_dt_s != "")
            {
                sb.AppendLine(" and a.END_DT >= @END_DT_S ");
                ht.Add("@END_DT_S", end_dt_s);
            }
            if (end_dt_e != "")
            {
                sb.AppendLine(" and a.END_DT <= @END_DT_E ");
                ht.Add("@END_DT_E", end_dt_e);
            }
            if (is_valid == "Y")
            {
                sb.AppendLine(" and (a.START_DT <= @CURRENT_DT and a.END_DT >= @CURRENT_DT) ");
                ht.Add("@CURRENT_DT", DateTime.Now.Date.ToString("yyyy/MM/dd"));
            }
            if (is_valid == "N")
            {
                sb.AppendLine(" and (a.START_DT >= @CURRENT_DT  or a.END_DT <= @CURRENT_DT ) ");
                ht.Add("@CURRENT_DT", DateTime.Now.Date.ToString("yyyy/MM/dd"));
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

    //明細頁面 Gridview 查詢資料
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string qdatakey)
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
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY IS_VALID desc, GRADE_CD ) As RowNumber,");
            sb.Append("  a.GRADE_CD as qdatakey ,a.IS_VALID,a.REMARK");
            //sb.Append(" GRADE_CD, REMARK");
            sb.Append(" from TB_H_M_LEVEL_GRADE a");
            sb.Append(" where 1 = 1 ");
            sb.Append(" and a.LEVEL_CD = @LEVEL_CD ");
            sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar)");

            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            ht.Add("@LEVEL_CD", qdatakey);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    //明細頁面Gridview 查詢總筆數
    public int getCount(int startRowIndex, int maximumRows, string qdatakey)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select COUNT(*) total_record ");
            sb.Append(" from TB_H_M_LEVEL_GRADE a");
            sb.Append(" where 1 = 1 ");
            sb.Append(" and a.LEVEL_CD = @LEVEL_CD  ");
            ht.Add("@LEVEL_CD", qdatakey);

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
            sb.Append(" and IS_VALID = 'Y' ");
            ht.Add("@LEVEL_CD", LEVEL_CD);
            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }

   
}