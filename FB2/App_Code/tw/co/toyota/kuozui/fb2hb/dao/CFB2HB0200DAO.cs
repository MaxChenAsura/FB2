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
/// CFB2HB0200DAO 的摘要描述
/// </summary>
public class CFB2HB0200DAO : BaseDAO
{
    public CFB2HB0200DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    //Gridview 查詢資料
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string emp_id, string hr_chg_cd, string ict_type,
                string transfer_nation_cd, string transfer_company_cd, string start_dt_s, string start_dt_e, string is_valid)
    {
        try
        {
            //if (sortExpression.Contains("EMP_ID"))
            //{
            //    sortExpression = sortExpression.Replace("EMP_ID", "a.EMP_ID");
            //}
            //if (sortExpression.Contains("EMP_NAME"))
            //{
            //    sortExpression = sortExpression.Replace("EMP_NAME", "d.EMP_NAME");
            //}
            //if (sortExpression.Contains("TRANSFER_COMPANY_CD"))
            //{
            //    sortExpression = sortExpression.Replace("TRANSFER_COMPANY_CD", "a.TRANSFER_COMPANY_CD");
            //}
            //if (sortExpression.Contains("TRANSFER_NATION_CD"))
            //{
            //    sortExpression = sortExpression.Replace("TRANSFER_NATION_CD", "a.TRANSFER_NATION_CD");
            //}
            //if (sortExpression.Contains("TRANSFER_DEPT"))
            //{
            //    sortExpression = sortExpression.Replace("TRANSFER_DEPT", "a.TRANSFER_DEPT");
            //}
            //if (sortExpression.Contains("START_DT"))
            //{
            //    sortExpression = sortExpression.Replace("START_DT", "a.START_DT");
            //}
            //if (sortExpression.Contains("HR_CHG_NO"))
            //{
            //    sortExpression = sortExpression.Replace("HR_CHG_NO", "a.HR_CHG_NO");
            //}
            //if (sortExpression.Contains("ICT_TYPE"))
            //{
            //    sortExpression = sortExpression.Replace("ICT_TYPE", "a.ICT_TYPE");
            //}
            //if (sortExpression.Contains("TRANSFER_REASON"))
            //{
            //    sortExpression = sortExpression.Replace("TRANSFER_REASON", "a.TRANSFER_REASON");
            //}
           

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,* ");
            sb.Append("     from (select ");
            sb.Append(" a.TRANSFER_REASON,b.HR_CHG_DESC,a.ICT_TYPE, a.ICT_TYPE + '-' + f.SUB_DESC ICT_TYPE_DESC,a.EMP_ID,d.EMP_NAME, c.ORI_DEPT_NO, c.ORI_DEPT_NAME_20,c.ORI_LEVEL_CD,");
            sb.Append(" d.DEPT_NAME_20,a.TRANSFER_NATION_CD,a.TRANSFER_NATION_CD + '-' + e.SUB_DESC TRANSFER_NATION,a.TRANSFER_COMPANY_CD,");
            sb.Append(" case a.TRANSFER_REASON when 'B07' then a.TRANSFER_COMPANY_CD + '-' + (select SUB_DESC from TB_9_M_COMM_D where TB_9_M_COMM_D.MAIN_CD = 'SUPPORT_COMPANY_CD' and TB_9_M_COMM_D.SUB_CD = a.TRANSFER_COMPANY_CD)");
            sb.Append(" when 'B08' then a.TRANSFER_COMPANY_CD + '-' + (select SUB_DESC from TB_9_M_COMM_D where TB_9_M_COMM_D.MAIN_CD = 'GCC_COMPANY_CD' and TB_9_M_COMM_D.SUB_CD = a.TRANSFER_COMPANY_CD)");
            sb.Append(" when 'B09' then a.TRANSFER_COMPANY_CD + '-' + (select SUB_DESC from TB_9_M_COMM_D where TB_9_M_COMM_D.MAIN_CD = 'ICT_COMPANY_CD' and TB_9_M_COMM_D.SUB_CD = a.TRANSFER_COMPANY_CD)");
            sb.Append(" END as TRANSFER_COMPANY,a.TRANSFER_DEPT,");
            sb.Append(" REPLACE(CONVERT(char(10), a.START_DT, 120),'-','/') START_DT,");
            sb.Append(" REPLACE(CONVERT(char(10), a.PLAN_END_DT, 120),'-','/') PLAN_END_DT,");
            sb.Append(" REPLACE(CONVERT(char(10), a.END_DT, 120),'-','/') END_DT,a.HR_CHG_NO");
            sb.Append(" from TB_H_R_EMP_TRANSFER a "); 
            sb.Append(" left join TB_H_M_HR_CHANGE_CODE b ");
            sb.Append("     on a.TRANSFER_REASON = b.HR_CHG_CD ");
            sb.Append(" left join TB_H_M_EMP_HR_CHANGE_H c ");
            sb.Append("     on a.HR_CHG_NO = c.HR_CHG_NO and a.EMP_ID = c.EMP_ID ");
            sb.Append(" left join VW_H_EMP_DATA d ");
            sb.Append("     on a.EMP_ID = d.EMP_ID ");
            sb.Append(" left join TB_9_M_COMM_D e ");
            sb.Append("     on  a.TRANSFER_NATION_CD = e.SUB_CD ");
            sb.Append("     and e.MAIN_CD = 'NATION_CD'");
            sb.Append(" left join TB_9_M_COMM_D f ");
            sb.Append("     on f.SYS_CD = 'HC' and f.MAIN_CD = 'ICT_TYPE' and a.ICT_TYPE = f.SUB_CD ");
            sb.Append(" where 1 = 1 ");            
            if (emp_id != "")
            {
                sb.Append(" and a.EMP_ID like @EMP_ID ");
                ht.Add("@EMP_ID", emp_id + "%");
            }
            if (hr_chg_cd != "-1" && hr_chg_cd != "")
            {
                sb.Append(" and a.TRANSFER_REASON = @HR_CHG_CD ");
                ht.Add("@HR_CHG_CD", hr_chg_cd);
            }
            if (ict_type != "-1" && ict_type != "")
            {
                sb.Append(" and a.ICT_TYPE = @ICT_TYPE ");
                ht.Add("@ICT_TYPE", ict_type);
            }
            if (transfer_nation_cd != "-1" && transfer_nation_cd != "")
            {
                sb.Append(" and a.TRANSFER_NATION_CD = @TRANSFER_NATION_CD ");
                ht.Add("@TRANSFER_NATION_CD", transfer_nation_cd);
            }
            if (transfer_company_cd != "-1" && transfer_company_cd != "")
            {
                sb.Append(" and a.TRANSFER_COMPANY_CD = @TRANSFER_COMPANY_CD ");
                ht.Add("@TRANSFER_COMPANY_CD", transfer_company_cd);
            }
            if (start_dt_s != "")
            {

                //sb.Append(" and a.END_DT >= CONVERT(datetime,@start_dt_s) and a.PLAN_END_DT >= CONVERT(datetime,@start_dt_s)");
                sb.Append(" and a.PLAN_END_DT >= CONVERT(datetime,@start_dt_s)");
                ht.Add("@start_dt_s", start_dt_s);

            }
            if (start_dt_e != "")
            {
                sb.Append(" and a.START_DT <= CONVERT(datetime,@start_dt_e) ");
                ht.Add("@start_dt_e", start_dt_e);
            }
            if (is_valid == "1")
            {
                sb.Append(" and a.END_DT is NULL ");
            }
            if (is_valid == "2")
            {
                sb.Append(" and a.END_DT is not NULL ");
            }

            sb.Append(" )alltb ");
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
    public int getCount(int startRowIndex, int maximumRows, string emp_id, string hr_chg_cd, string ict_type,
                string transfer_nation_cd, string transfer_company_cd, string start_dt_s, string start_dt_e, string is_valid)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select COUNT(*) total_record ");
            sb.Append(" from TB_H_R_EMP_TRANSFER a ");
            sb.Append(" left join TB_H_M_HR_CHANGE_CODE b ");
            sb.Append("     on a.TRANSFER_REASON = b.HR_CHG_CD ");
            sb.Append(" left join TB_H_M_EMP_HR_CHANGE_H c ");
            sb.Append("     on a.HR_CHG_NO = c.HR_CHG_NO and a.EMP_ID = c.EMP_ID ");
            sb.Append(" left join VW_H_EMP_DATA d ");
            sb.Append("     on a.EMP_ID = d.EMP_ID ");
            sb.Append(" left join TB_9_M_COMM_D e ");
            sb.Append("     on  a.TRANSFER_NATION_CD = e.SUB_CD ");
            sb.Append("     and e.MAIN_CD = 'NATION_CD'");
            sb.Append(" left join TB_9_M_COMM_D f ");
            sb.Append("     on f.SYS_CD = 'HC' and f.MAIN_CD = 'ICT_TYPE' and a.ICT_TYPE = f.SUB_CD ");
            sb.Append(" where 1 = 1 ");
            if (emp_id != "")
            {
                sb.Append(" and a.EMP_ID like @EMP_ID ");
                ht.Add("@EMP_ID", emp_id + "%");
            }
            if (hr_chg_cd != "-1" && hr_chg_cd != "")
            {
                sb.Append(" and a.TRANSFER_REASON = @HR_CHG_CD ");
                ht.Add("@HR_CHG_CD", hr_chg_cd);
            }
            if (ict_type != "-1" && ict_type != "")
            {
                sb.Append(" and a.ICT_TYPE = @ICT_TYPE ");
                ht.Add("@ICT_TYPE", ict_type);
            }
            if (transfer_nation_cd != "-1" && transfer_nation_cd != "")
            {
                sb.Append(" and a.TRANSFER_NATION_CD = @TRANSFER_NATION_CD ");
                ht.Add("@TRANSFER_NATION_CD", transfer_nation_cd);
            }
            if (transfer_company_cd != "-1" && transfer_company_cd != "")
            {
                sb.Append(" and a.TRANSFER_COMPANY_CD = @TRANSFER_COMPANY_CD ");
                ht.Add("@TRANSFER_COMPANY_CD", transfer_company_cd);
            }
            if (start_dt_s != "")
            {

                //sb.Append(" and a.END_DT >= CONVERT(datetime,@start_dt_s) and a.PLAN_END_DT >= CONVERT(datetime,@start_dt_s)");
                sb.Append(" and a.PLAN_END_DT >= CONVERT(datetime,@start_dt_s)");
                ht.Add("@start_dt_s", start_dt_s);

            }
            if (start_dt_e != "")
            {
                sb.Append(" and a.START_DT <= CONVERT(datetime,@start_dt_e) ");
                ht.Add("@start_dt_e", start_dt_e);
            }
            if (is_valid == "1")
            {
                sb.Append(" and a.END_DT is NULL ");
            }
            if (is_valid == "2")
            {
                sb.Append(" and a.END_DT is not NULL ");
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

    internal DataTable getHR_CHG_CD()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select HR_CHG_CD,HR_CHG_DESC from TB_H_M_HR_CHANGE_CODE where TB_H_M_HR_CHANGE_CODE.EMP_CHG_STATUS = '21'  and HR_CHG_CD<>'D02' order by HR_CHG_CD ");

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
}