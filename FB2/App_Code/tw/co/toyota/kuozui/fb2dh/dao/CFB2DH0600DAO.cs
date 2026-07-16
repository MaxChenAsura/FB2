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
/// CFB2DH0600DAO 的摘要描述
/// </summary>
public class CFB2DH0600DAO : BaseDAO
{
    public CFB2DH0600DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string apply_leave_ym, string emp_id,
        string is_super, string is_dept, string departments)
    {
        try
        {

            if (sortExpression.Contains("MAIN_LEAVE_CD_DESC"))
                sortExpression = sortExpression.Replace("MAIN_LEAVE_CD_DESC", "RData.MAIN_LEAVE_CD_DESC");

            if (sortExpression.Contains("SUB_LEAVE_CD_DESC"))
                sortExpression = sortExpression.Replace("SUB_LEAVE_CD_DESC", "RData.SUB_LEAVE_CD_DESC");


            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("  select * from  ( select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber, *   ");
            sb.Append("   FROM ( ");
            sb.Append(" select * from [dbo].[FN_DH060_01](@EMP_ID, @query_YM) ");
            sb.Append(") RData");
            sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar) ");


            ht.Add("@EMP_ID", emp_id);
            ht.Add("@query_YM", apply_leave_ym.Replace("/", ""));

            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    public int getCount(int startRowIndex, int maximumRows, string apply_leave_ym, string emp_id,
        string is_super, string is_dept, string departments)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("  select count(*)	total_record   ");
            sb.Append("   FROM ( ");
            sb.Append(" select * from [dbo].[FN_DH060_01](@EMP_ID, @query_YM) ");
            sb.Append(") RData");

            ht.Add("@emp_id", emp_id);
            ht.Add("@query_YM", apply_leave_ym.Replace("/", ""));

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
    
    //主假別
    public DataTable getMAIN_LEAVE_CD(string main_leave_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select MAIN_LEAVE_CD+'-'+MAIN_LEAVE_DESC MAIN_LEAVE_DESC ");
            sb.Append(" from TB_D_M_LEAVE_TYPE_H ");
            sb.Append(" where MAIN_LEAVE_CD=@MAIN_LEAVE_CD ");
            ht.Add("@MAIN_LEAVE_CD", main_leave_cd);

            return dbConn.Query(sb, ht);
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    //子假別
    public DataTable getSUB_LEAVE_CD(string main_leave_cd, string sub_leave_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select SUB_LEAVE_CD+'-'+SUB_LEAVE_DESC SUB_LEAVE_DESC ");
            sb.Append(" from TB_D_M_LEAVE_TYPE_D ");
            sb.Append(" where MAIN_LEAVE_CD=@MAIN_LEAVE_CD ");
            sb.Append(" and SUB_LEAVE_CD=@SUB_LEAVE_CD ");
            ht.Add("@MAIN_LEAVE_CD", main_leave_cd);
            ht.Add("@SUB_LEAVE_CD", sub_leave_cd);

            return dbConn.Query(sb, ht);
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    //時間單位
    public DataTable getLEAVE_TIME_UNIT(string leave_time_unit)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select SUB_DESC from TB_9_M_COMM_D ");
            sb.Append(" where MAIN_CD = 'LEAVE_TIME_UNIT' and SYS_CD = 'DH' and IS_VALID='Y' and SUB_CD=@LEAVE_TIME_UNIT ");
            ht.Add("@LEAVE_TIME_UNIT", leave_time_unit);

            return dbConn.Query(sb, ht);
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    //本月可休數
    public DataTable getMAY_REST_TIMES_M(string emp_id, string apply_leave_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select SUM(APPROVE_OVERTIME_HOUR) MAY_REST_TIMES_M ");
            sb.Append(" from TB_D_M_OVERTIME_APPLY ");
            sb.Append(" where FORM_STATUS not in ('N','D') and CHECK_STATUS='Y' ");
            sb.Append(" and OVERTIME_CD='A' and OVERTIME_DT_TYPE='1' ");
            sb.Append(" and EMP_ID=@EMP_ID ");
            sb.Append(" and YEAR(APPLY_OVERTIME_DT)=@APPLY_OVERTIME_DT_Y ");
            sb.Append(" and MONTH(APPLY_OVERTIME_DT)=@APPLY_OVERTIME_DT_M");
            sb.Append(" group by EMP_ID");

            ht.Add("@EMP_ID", emp_id);
            ht.Add("@APPLY_OVERTIME_DT_Y", apply_leave_dt.Split('/')[0]);
            ht.Add("@APPLY_OVERTIME_DT_M", apply_leave_dt.Split('/')[1]);

            return dbConn.Query(sb, ht);
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    //本年可休數 
    public DataTable getMAY_REST_TIMES_Y(string emp_id, string apply_leave_dt_y)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select SUM(APPROVE_OVERTIME_HOUR) MAY_REST_TIMES_Y ");
            sb.Append(" from TB_D_M_OVERTIME_APPLY ");
            sb.Append(" where FORM_STATUS not in ('N','D') and CHECK_STATUS='Y' ");
            sb.Append(" and OVERTIME_CD='B' and OVERTIME_DT_TYPE='2' ");
            sb.Append(" and EMP_ID=@EMP_ID ");
            sb.Append(" and YEAR(APPLY_OVERTIME_DT)=@APPLY_OVERTIME_DT_Y ");
            sb.Append(" group by EMP_ID");

            ht.Add("@EMP_ID", emp_id);
            ht.Add("@APPLY_OVERTIME_DT_Y", apply_leave_dt_y);

            return dbConn.Query(sb, ht);
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    //本年可休數2 (讀取 員工可用假明細檔, 依 工號+主假別+子假別+請假年度, 統計 各假別之當年休假核定日數)
    public DataTable getMAY_REST_TIMES_Y2(string emp_id, string apply_leave_dt_y, string main_leave_cd, string sub_leave_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select SUM(APPROVE_VALUE) MAY_REST_TIMES_Y ");
            sb.Append(" from TB_D_M_EMP_AVAILABLE_LEAVE ");
            sb.Append(" where EMP_ID=@EMP_ID and MAIN_LEAVE_CD=@MAIN_LEAVE_CD and SUB_LEAVE_CD=@SUB_LEAVE_CD");
            sb.Append(" and BASE_YEAR=@BASE_YEAR ");
            sb.Append(" group by EMP_ID,MAIN_LEAVE_CD,SUB_LEAVE_CD");

            ht.Add("@EMP_ID", emp_id);
            ht.Add("@MAIN_LEAVE_CD", main_leave_cd);
            ht.Add("@SUB_LEAVE_CD", sub_leave_cd);
            ht.Add("@BASE_YEAR", apply_leave_dt_y);

            return dbConn.Query(sb, ht);
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    //TB_D_M_LEAVE_TYPE_D 主假別資料檔
    //LEAVE_MAX_DAY_CD	上限控管方式 C.條件控管  T.明細檔  N.不控管  G.特殊身份
    public DataTable getLEAVE_MAX_DAY_CD(string main_leave_cd, string sub_leave_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select LEAVE_MAX_DAY_CD ");
            sb.Append(" from TB_D_M_LEAVE_TYPE_D ");
            sb.Append(" where LEAVE_MAX_DAY_CD='T' and MAIN_LEAVE_CD=@MAIN_LEAVE_CD ");
            sb.Append(" and SUB_LEAVE_CD=@SUB_LEAVE_CD ");
            ht.Add("@MAIN_LEAVE_CD", main_leave_cd);
            ht.Add("@SUB_LEAVE_CD", sub_leave_cd);

            return dbConn.Query(sb, ht);
        }
        catch (Exception ex)
        {

            throw;
        }
    }


    public DataTable getEmpName(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select EMP_ID,EMP_NAME,DEPT_NO,DEPT_NAME ");
            sb.Append(" from VW_H_EMP_DATA ");
            sb.Append(" where EMP_ID=@EMP_ID ");
            ht.Add("@EMP_ID", emp_id);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getOvertimeData(string emp_id, string ym)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" 
                        select SUM(overtime1)overtime1,SUM(overtime2)overtime2,SUM(overtime3)overtime3,SUM(overtime4)overtime4 from (
                            --平日換休
                            select TOTAL_TIME_REMAING overtime1,0 overtime2,0 overtime3,0 overtime4 from FN_D_GET_LEAVE_R_DATA_Z0(@emp_id, @ym)
                            UNION ALL
                            --假日換休
                            select 0,TOTAL_TIME_REMAING overtime2,0,0 from FN_D_GET_LEAVE_R_DATA_X0(@emp_id, @ym)
                            UNION ALL
                            --特休假
                            select 0,0,TOTAL_TIME_REMAING overtime3 ,0 from FN_D_GET_LEAVE_R_DATA_DM(@emp_id, @ym) where sub_leave_cd = 'D0'
                            UNION ALL
                            --榮譽假
                            select 0,0,0,TOTAL_TIME_REMAING overtime4 from FN_D_GET_LEAVE_R_DATA_DM(@emp_id, @ym) where sub_leave_cd = 'M0'
                        ) a 
            ");


            ht.Add("@emp_id", emp_id);
            ht.Add("@ym", ym);          

            return dbConn.Query(sb, ht);
        }
        catch (Exception ex)
        {

            throw;
        }
    }

}