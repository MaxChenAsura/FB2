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
/// CFB2HA0500DAO 的摘要描述
/// </summary>
public class CFB2HA0500DAO : BaseDAO
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

    public CFB2HA0500DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    //Gridview 查詢資料
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string pjob_cd, string level_cd,
                            string ws_cd, string pjob_age_limit, string pjob_level, string business_trip_grp,
                            string start_dt_s, string start_dt_e, string end_dt_s, string end_dt_e, string is_valid)
    {
        try
        {

            if (sortExpression.Contains("REMARK"))
            {
                sortExpression = sortExpression.Replace("REMARK", "a.REMARK");
            }

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" PJOB_CD,PJOB_DESC,REPLACE(CONVERT(char(10), a.START_DT, 120),'-','/') START_DT,REPLACE(CONVERT(char(10), a.END_DT, 120),'-','/') END_DT,LEVEL_CD,WS_CD,MANAGEMENT_ALLOWANCE,");
            sb.Append(" PROFESSION_ALLOWANCE,PJOB_AGE_LIMIT,PJOB_LEVEL,PJOB_LEVEL + '-' + b.SUB_DESC PJOB_LEVEL_DESC, ");
            sb.Append(" PJOB_FLOW_LEVEL,BUSINESS_TRIP_GRP,BUSINESS_TRIP_GRP + '-' + c.SUB_DESC BUSINESS_TRIP_GRP_DESC,a.REMARK");
            sb.Append(" from TB_H_M_PJOB a left join TB_9_M_COMM_D b on a.PJOB_LEVEL = b.SUB_CD and b.SYS_CD = 'HA' and b.MAIN_CD = 'PJOB_LEVEL' ");
            sb.Append(" left join  TB_9_M_COMM_D c on a.BUSINESS_TRIP_GRP = c.SUB_CD and c.SYS_CD = 'HA' and c.MAIN_CD = 'BUSINESS_TRIP_GRP'");
            sb.Append(" where a.PJOB_CD is not null ");

            if (pjob_cd != "")
            {
                sb.Append(" and PJOB_CD like @PJOB_CD ");
                ht.Add("@PJOB_CD", pjob_cd + "%");
            }
            if (level_cd != "" && level_cd != "-1")
            {
                sb.Append(" and LEVEL_CD = @LEVEL_CD ");
                ht.Add("@LEVEL_CD", level_cd);
            }
            if (ws_cd != "" && ws_cd != "-1")
            {
                sb.Append(" and WS_CD = @WS_CD ");
                ht.Add("@WS_CD", ws_cd);
            }
            if (pjob_age_limit != "" && pjob_age_limit != "-1")
            {
                sb.Append(" and PJOB_AGE_LIMIT = @PJOB_AGE_LIMIT ");
                ht.Add("@PJOB_AGE_LIMIT", pjob_age_limit);
            }
            if (pjob_level != "" && pjob_level != "-1")
            {
                sb.Append(" and PJOB_LEVEL = @PJOB_LEVEL ");
                ht.Add("@PJOB_LEVEL", pjob_level);
            }
            if (business_trip_grp != "" && business_trip_grp != "-1")
            {
                sb.Append(" and BUSINESS_TRIP_GRP = @BUSINESS_TRIP_GRP ");
                ht.Add("@BUSINESS_TRIP_GRP", business_trip_grp);
            }


            //if (start_dt_s != "")
            //{
            //    if (start_dt_e != "")
            //    {
            //        sb.Append(" and a.START_DT >= CONVERT(datetime,@start_dt_s) and a.END_DT <= CONVERT(datetime,@start_dt_e)");
            //        ht.Add("@start_dt_s", start_dt_s);
            //        ht.Add("@start_dt_e", start_dt_e);
            //    }
            //    else
            //    {
            //        sb.Append(" and a.START_DT >= CONVERT(datetime,@start_dt_s) ");
            //        ht.Add("@start_dt_s", start_dt_s);
            //    }
            //}
            //else if (start_dt_e != "")
            //{
            //    sb.Append(" and a.END_DT <= CONVERT(datetime,@start_dt_e) ");
            //    ht.Add("@start_dt_e", start_dt_e);
            //}



            //if (start_dt_e != "" && is_valid == "Y")
            //{
            //    sb.Append(" and a.END_DT >= @END_DT2 ");
            //    ht.Add("@END_DT2", start_dt_e);
            //}
            //if (start_dt_e != "" && is_valid == "N")
            //{
            //    sb.Append(" and a.END_DT < @END_DT2 ");
            //    ht.Add("@END_DT2", start_dt_e);
            //}

            //if (start_dt_s != "")
            //{
            //    sb.AppendLine(" and a.END_DT >= @START_DT ");
            //    ht.Add("@START_DT", start_dt_s);
            //}
            //if (start_dt_e != "")
            //{
            //    sb.AppendLine(" and a.START_DT <= @END_DT ");
            //    ht.Add("@END_DT", start_dt_e);
            //}
            //if (start_dt_e != "" && is_valid == "Y")
            //{
            //    sb.AppendLine(" and a.END_DT >= @END_DT2 ");
            //    ht.Add("@END_DT2", start_dt_e);
            //}
            //if (start_dt_e != "" && is_valid == "N")
            //{
            //    sb.AppendLine(" and a.END_DT < @END_DT2 ");
            //    ht.Add("@END_DT2", start_dt_e);
            //}
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
    public int getCount(int startRowIndex, int maximumRows, string pjob_cd, string level_cd,
                            string ws_cd, string pjob_age_limit, string pjob_level, string business_trip_grp,
                            string start_dt_s, string start_dt_e, string end_dt_s, string end_dt_e, string is_valid)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select COUNT(*) total_record ");
            sb.Append(" from TB_H_M_PJOB a left join TB_9_M_COMM_D b on a.PJOB_LEVEL = b.SUB_CD and b.SYS_CD = 'HA' and b.MAIN_CD = 'PJOB_LEVEL' ");
            sb.Append(" left join  TB_9_M_COMM_D c on a.BUSINESS_TRIP_GRP = c.SUB_CD and c.SYS_CD = 'HA' and c.MAIN_CD = 'BUSINESS_TRIP_GRP'");
            sb.Append(" where a.PJOB_CD is not null ");
            if (pjob_cd != "")
            {
                sb.Append(" and PJOB_CD like @PJOB_CD ");
                ht.Add("@PJOB_CD", pjob_cd + "%");
            }
            if (level_cd != "" && level_cd != "-1")
            {
                sb.Append(" and LEVEL_CD = @LEVEL_CD ");
                ht.Add("@LEVEL_CD", level_cd);
            }
            if (ws_cd != "" && ws_cd != "-1")
            {
                sb.Append(" and WS_CD = @WS_CD ");
                ht.Add("@WS_CD", ws_cd);
            }
            if (pjob_age_limit != "" && pjob_age_limit != "-1")
            {
                sb.Append(" and PJOB_AGE_LIMIT = @PJOB_AGE_LIMIT ");
                ht.Add("@PJOB_AGE_LIMIT", pjob_age_limit);
            }
            if (pjob_level != "" && pjob_level != "-1")
            {
                sb.Append(" and PJOB_LEVEL = @PJOB_LEVEL ");
                ht.Add("@PJOB_LEVEL", pjob_level);
            }
            if (business_trip_grp != "" && business_trip_grp != "-1")
            {
                sb.Append(" and BUSINESS_TRIP_GRP = @BUSINESS_TRIP_GRP ");
                ht.Add("@BUSINESS_TRIP_GRP", business_trip_grp);
            }


            //if (start_dt_s != "")
            //{
            //    if (start_dt_e != "")
            //    {
            //        sb.Append(" and a.START_DT >= CONVERT(datetime,@start_dt_s) and a.END_DT <= CONVERT(datetime,@start_dt_e)");
            //        ht.Add("@start_dt_s", start_dt_s);
            //        ht.Add("@start_dt_e", start_dt_e);
            //    }
            //    else
            //    {
            //        sb.Append(" and a.START_DT >= CONVERT(datetime,@start_dt_s) ");
            //        ht.Add("@start_dt_s", start_dt_s);
            //    }
            //}
            //else if (start_dt_e != "")
            //{
            //    sb.Append(" and a.END_DT <= CONVERT(datetime,@start_dt_e) ");
            //    ht.Add("@start_dt_e", start_dt_e);
            //}
            //if (start_dt_e != "" && is_valid == "Y")
            //{
            //    sb.Append(" and a.END_DT >= @END_DT2 ");
            //    ht.Add("@END_DT2", start_dt_e);
            //}
            //if (start_dt_e != "" && is_valid == "N")
            //{
            //    sb.Append(" and a.END_DT < @END_DT2 ");
            //    ht.Add("@END_DT2", start_dt_e);
            //}
            //if (start_dt_s != "")
            //{
            //    sb.AppendLine(" and a.END_DT >= @START_DT ");
            //    ht.Add("@START_DT", start_dt_s);
            //}
            //if (start_dt_e != "")
            //{
            //    sb.AppendLine(" and a.START_DT <= @END_DT ");
            //    ht.Add("@END_DT", start_dt_e);
            //}
            //if (start_dt_e != "" && is_valid == "Y")
            //{
            //    sb.AppendLine(" and a.END_DT >= @END_DT2 ");
            //    ht.Add("@END_DT2", start_dt_e);
            //}
            //if (start_dt_e != "" && is_valid == "N")
            //{
            //    sb.AppendLine(" and a.END_DT < @END_DT2 ");
            //    ht.Add("@END_DT2", start_dt_e);
            //}
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
    internal System.Data.DataTable getEditLevelCD()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select distinct LEVEL_CD from TB_H_M_LEVEL order by LEVEL_CD");
            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }


    internal DataTable getTB_H_M_EMP()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select count(0) empcount from TB_H_M_EMP");
            sb.Append(" where PJOB_CD=@PJOB_CD");
            sb.Append(" AND JOIN_DT>@END_DT");
            ht.Add("@PJOB_CD", PJOB_CD);
            ht.Add("@END_DT", END_DT);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getTB_H_M_EMP_HR_CHANGE_H()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select count(0) empcount from TB_H_M_EMP_HR_CHANGE_H a, TB_H_M_EMP_HR_CHANGE_D b");
            sb.Append(" where a.HR_CHG_NO=b.HR_CHG_NO");
            sb.Append(" and b.HR_CHG_ITEM='08'");
            sb.Append(" and b.AFTER_CD=@PJOB_CD");
            sb.Append(" and a.START_DT>@END_DT");
            ht.Add("@PJOB_CD", PJOB_CD);
            ht.Add("@END_DT", END_DT);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }


    internal DataTable getExistData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select * ");
            sb.Append(" from ( ");
            sb.Append("     select TOP 1 PJOB_CD,END_DT from TB_H_M_PJOB ");
            sb.Append(" where PJOB_CD = @PJOB_CD ");
            sb.Append(" order by START_DT desc ");
            sb.Append(" ) data ");
            sb.Append(" where END_DT >= @START_DT ");
            ht.Add("@PJOB_CD", PJOB_CD);
            ht.Add("@START_DT", START_DT);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getPJOB_CD(string pjob_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * from VW_TB_H_M_PJOB  ");
            sb.Append(" where PJOB_CD = @PJOB_CD ");
            ht.Add("@PJOB_CD", pjob_cd);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void addPjob()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("insert into TB_H_M_PJOB (PJOB_CD,START_DT,END_DT,PJOB_DESC,LEVEL_CD,WS_CD,MANAGEMENT_ALLOWANCE,PROFESSION_ALLOWANCE,");
            sb.Append(" PJOB_AGE_LIMIT,PJOB_LEVEL,PJOB_FLOW_LEVEL,BUSINESS_TRIP_GRP,REMARK,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" values (UPPER(@PJOB_CD),@START_DT,@END_DT,@PJOB_DESC,@LEVEL_CD,@WS_CD,@MANAGEMENT_ALLOWANCE,@PROFESSION_ALLOWANCE,");
            sb.Append(" @PJOB_AGE_LIMIT,@PJOB_LEVEL,@PJOB_FLOW_LEVEL,@BUSINESS_TRIP_GRP,@REMARK,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@PJOB_CD", PJOB_CD.ToUpper());
            ht.Add("@START_DT", START_DT);
            ht.Add("@END_DT", END_DT);
            ht.Add("@PJOB_DESC", PJOB_DESC);
            ht.Add("@LEVEL_CD", LEVEL_CD);
            ht.Add("@WS_CD", WS_CD);
            ht.Add("@MANAGEMENT_ALLOWANCE", MANAGEMENT_ALLOWANCE == "" ? "0" : MANAGEMENT_ALLOWANCE.Replace(",", ""));
            ht.Add("@PROFESSION_ALLOWANCE", PROFESSION_ALLOWANCE == "" ? "0" : PROFESSION_ALLOWANCE.Replace(",", ""));
            ht.Add("@PJOB_AGE_LIMIT", PJOB_AGE_LIMIT);
            ht.Add("@PJOB_LEVEL", PJOB_LEVEL);
            ht.Add("@PJOB_FLOW_LEVEL", PJOB_FLOW_LEVEL);
            ht.Add("@BUSINESS_TRIP_GRP", BUSINESS_TRIP_GRP);
            ht.Add("@REMARK", REMARK);
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

    internal void updatePjob()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_H_M_PJOB set END_DT = @END_DT,PJOB_DESC = @PJOB_DESC,LEVEL_CD = @LEVEL_CD,WS_CD = @WS_CD,");
            sb.Append(" MANAGEMENT_ALLOWANCE = @MANAGEMENT_ALLOWANCE,PROFESSION_ALLOWANCE = @PROFESSION_ALLOWANCE,");
            sb.Append(" PJOB_AGE_LIMIT = @PJOB_AGE_LIMIT,PJOB_LEVEL = @PJOB_LEVEL,PJOB_FLOW_LEVEL = @PJOB_FLOW_LEVEL,BUSINESS_TRIP_GRP = @BUSINESS_TRIP_GRP,");
            sb.Append(" REMARK = @REMARK,UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID ");
            sb.Append(" where PJOB_CD = @PJOB_CD and START_DT = @START_DT");

            ht.Add("@PJOB_CD", PJOB_CD.ToUpper());
            ht.Add("@START_DT", START_DT);
            ht.Add("@END_DT", END_DT);
            ht.Add("@PJOB_DESC", PJOB_DESC);
            ht.Add("@LEVEL_CD", LEVEL_CD);
            ht.Add("@WS_CD", WS_CD);
            ht.Add("@MANAGEMENT_ALLOWANCE", MANAGEMENT_ALLOWANCE == "" ? "0" : MANAGEMENT_ALLOWANCE.Replace(",", ""));
            ht.Add("@PROFESSION_ALLOWANCE", PROFESSION_ALLOWANCE == "" ? "0" : PROFESSION_ALLOWANCE.Replace(",", ""));
            ht.Add("@PJOB_AGE_LIMIT", PJOB_AGE_LIMIT);
            ht.Add("@PJOB_LEVEL", PJOB_LEVEL);
            ht.Add("@PJOB_FLOW_LEVEL", PJOB_FLOW_LEVEL);
            ht.Add("@BUSINESS_TRIP_GRP", BUSINESS_TRIP_GRP);
            ht.Add("@REMARK", REMARK);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getExistEmpData(string pjob_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select COUNT(0) empcount from TB_H_M_EMP ");
            sb.Append(" where PJOB_CD=@PJOB_CD ");
            ht.Add("@PJOB_CD", pjob_cd);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void deletePjobCD(string pjob_cd, string start_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //寫log
            sb.Append(" update TB_H_M_PJOB set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2HA050' ");
            sb.Append(" where PJOB_CD=@PJOB_CD and START_DT = @START_DT; ");
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);

            sb.Append(" delete from TB_H_M_PJOB ");
            sb.Append(" where PJOB_CD=@PJOB_CD and START_DT = @START_DT; ");
            ht.Add("@PJOB_CD", pjob_cd);
            ht.Add("@START_DT", start_dt);
            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {

            throw;
        }
    }
}