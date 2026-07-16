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
/// wfb2df0200 的摘要描述
/// </summary>

public class CFB2DF0200DAO : BaseDAO
{
    //基本欄位
    public string EMP_ID { get; set; }
    public string EMP_NAME { get; set; }
    public string EMP_CHG_CD { get; set; }
    public string COMPANY_CD { get; set; }
    public string DEPT_NO { get; set; }
    public string START_DT { get; set; }
    public string END_DT { get; set; }
    public string LEAVE_DT_S { get; set; }
    public string LEAVE_DT_E { get; set; }
    public string ACCOM_CD { get; set; }
    public string ACCOM_BUILD_CD { get; set; }
    public string ROOM_NO { get; set; }
    public string BASE_NO { get; set; }
    public string AMOUNT { get; set; }
    public string OTHER_AMOUNT { get; set; }
    public string REMARK { get; set; }
    public string BUS_CD { get; set; }
    public string MOTOR_NO { get; set; }
    public string CAR_NO { get; set; }
    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }

    //for查詢欄位
    public string AGE { get; set; }
    public string age_where { get; set; }
    public string start_dt_where { get; set; }
    public string work_year { get; set; }
    public string work_year_where { get; set; }

    public CFB2DF0200DAO()
    {
        //
        // 建立db連線
        //

    }
    //取得基本資料
    public DataTable getEMPFile()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select EMP_NAME,b.SUB_DESC EMP_CD,e.DEPT_NO,d.DEPT_NAME,PJOB_DESC,WORK_SHIFT_DESC WORK_SHIFT_CD,CONVERT(char(10), JOIN_DT, 111) JOIN_DT");
            sb.Append(" ,REGISTER_ADDR,(select top 1 ADDRESS from TB_D_M_TRANS_ALLOWANCE_D where e.EMP_ID = TB_D_M_TRANS_ALLOWANCE_D.EMP_ID) CONTACT_ADDR,");
            sb.Append(" MOBILE_TEL_1,CONTACT_TEL,AGE,LICENSE_ID ");
            sb.Append(" from VW_H_EMP_DATA e,VW_H_DEPT_DATA d,TB_9_M_COMM_D b");
            sb.Append(" where e.DEPT_NO = d.DEPT_NO and e.EMP_CD = b.SUB_CD and b.MAIN_CD = 'EMP_CD' ");
            sb.Append(" and EMP_ID = @EMP_ID");
            ht.Add("@EMP_ID", EMP_ID);

            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {
            throw;
        }
    }
    //取得住宿費基準檔資料
    public DataTable getAMOUNT()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select BASE_NO + '-' + BASE_NAME BASE_NAME,BASE_NO + '-' + convert(varchar,AMOUNT) BASE_NO from TB_D_M_ACCOM_BASE  ");
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //Gridview 查詢資料
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string emp_id, string dept_no, string emp_chg_cd,
                string leave_dt_s, string leave_dt_e, string accom, string accom_build, string room_no, string age, string age_where, string start_year,
                string start_dt_where, string join_years, string join_years_where)
    {
        try
        {
            if (sortExpression.Contains("EMP_ID"))
            {
                sortExpression = sortExpression.Replace("EMP_ID", "a.EMP_ID");
            }
            if (sortExpression.Contains("EMP_NAME"))
            {
                sortExpression = sortExpression.Replace("EMP_NAME", "a.EMP_NAME");
            }
            if (sortExpression.Contains("EMP_CHG_CD_DESC"))
            {
                sortExpression = sortExpression.Replace("EMP_CHG_CD_DESC", "d.EMP_CHG_CD");
            }
            if (sortExpression.Contains("WORK_SHIFT_CD_DESC"))
            {
                sortExpression = sortExpression.Replace("WORK_SHIFT_CD_DESC", "d.WORK_SHIFT_DESC");
            }
            if (sortExpression.Contains("END_DT"))
            {
                sortExpression = sortExpression.Replace("END_DT", "a.END_DT");
            }
            if (sortExpression.Contains("START_DT"))
            {
                sortExpression = sortExpression.Replace("START_DT", "a.START_DT");
            }
            
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select * From(");
            sb.Append(" Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + "  ) As RowNumber, a.ACCOM_CD,a.ACCOM_BUILD_CD,CONVERT(char(10), a.END_DT, 111) END_DT,");   //a.EMP_ID ASC
            sb.Append(" a.ACCOM_CD + '-' +b.SUB_DESC ACCOM_CD_DESC ,a.ACCOM_BUILD_CD+'-'+ b2.SUB_DESC ACCOM_BUILD_CD_DESC,a.ROOM_NO, a.EMP_ID,d.EMP_NAME,");
            sb.Append(" d.EMP_CD+ '-' + b3.SUB_DESC EMP_CD,d.EMP_CHG_CD + '-' +d.EMP_CHG_DESC EMP_CHG_CD_DESC,");
            sb.Append(" d.DEPT_NO +'-' + d.DIV_DEPT_FULL_NAME DEPT_NAME,");
            sb.Append(" d.WORK_SHIFT_CD + '-' +d.WORK_SHIFT_DESC WORK_SHIFT_CD_DESC,AGE, CONVERT(char(10), d.JOIN_DT, 111) JOIN_DT,");
            sb.Append(" WORK_YEARS, CONVERT(char(10), a.START_DT, 111) START_DT,a.AMOUNT,a.OTHER_AMOUNT,d.REGISTER_ADDR");
            sb.Append(" from TB_D_M_ACCOM_MAIN a");
            sb.Append(" left join TB_9_M_COMM_D b on a.ACCOM_CD = b.SUB_CD and b.MAIN_CD = 'ACCOM_CD' and b.SYS_CD = 'DF'");
            sb.Append(" left join TB_9_M_COMM_D b2 on a.ACCOM_BUILD_CD = b2.SUB_CD and b2.MAIN_CD = 'ACCOM_BUILD_CD' and b2.SYS_CD = 'DF'");
            sb.Append(" left join TB_H_M_DEPT c on a.DEPT_NO = c.DEPT_NO");
            sb.Append(" left join VW_H_EMP_DATA d on a.EMP_ID = d.EMP_ID");
            sb.Append(" left join TB_9_M_COMM_D b3 on d.EMP_CD = b3.SUB_CD and b3.SYS_CD = 'HB' and b3.MAIN_CD = 'EMP_CD'");
            sb.Append(" where  1 = 1");

            if (emp_id != "")
            {
                sb.Append(" and a.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
            }
            if (dept_no != "")
            {
                sb.Append(" and d.DEPT_NO = @DEPT_NO ");
                ht.Add("@DEPT_NO", dept_no);
            }
            if (emp_chg_cd != "-1")
            {
                sb.Append(" and d.EMP_CHG_CD = @EMP_CHG_CD ");
                ht.Add("@EMP_CHG_CD", emp_chg_cd);
            }
            if (accom != "-1" && accom != null)
            {
                sb.Append(" and b.SUB_CD = @accom ");
                ht.Add("@accom", accom);
            }
            if (accom_build != "-1" && accom_build != null)
            {
                sb.Append(" and b2.SUB_CD = @accom_build ");
                ht.Add("@accom_build", accom_build);
            }
            if (leave_dt_s != "")
            {
                if (leave_dt_e != "")
                {
                    sb.Append(" and LEAVE_DT >= CONVERT(datetime,@leave_dt_s) and LEAVE_DT <= CONVERT(datetime,@leave_dt_e)");
                    ht.Add("@leave_dt_s", leave_dt_s);
                    ht.Add("@leave_dt_e", leave_dt_e);
                }
                else
                {
                    sb.Append(" and LEAVE_DT >= CONVERT(datetime,@leave_dt_s) ");
                    ht.Add("@leave_dt_s", leave_dt_s);
                }
            }
            else if (leave_dt_e != "")
            {
                sb.Append(" and LEAVE_DT <= CONVERT(datetime,@leave_dt_e) ");
                ht.Add("@leave_dt_e", leave_dt_e);
            }
            if (room_no != "")
            {
                sb.Append(" and a.ROOM_NO = @room_no ");
                ht.Add("@room_no", room_no);
            }
            if (age != "")
            {
                switch (age_where)
                {
                    case "greater":
                        sb.Append(" and AGE > @age ");
                        break;
                    case "less":
                        sb.Append(" and AGE < @age ");
                        break;
                    case "equal":
                        sb.Append(" and AGE = @age ");
                        break;
                    default:
                        break;
                }

                ht.Add("@age", age);
            }
            if (start_year != "")
            {
                switch (start_dt_where)
                {
                    case "greater":
                        sb.Append(" and a.START_DT > CONVERT(datetime,@start_year) ");
                        break;
                    case "less":
                        sb.Append(" and a.START_DT < CONVERT(datetime,@start_year) ");
                        break;
                    case "equal":
                        sb.Append(" and a.START_DT = CONVERT(datetime,@start_year) ");
                        break;
                    default:
                        break;
                }

                ht.Add("@start_year", start_year);
            }
            if (join_years != "")
            {
                switch (join_years_where)
                {
                    case "greater":
                        sb.Append(" and WORK_YEARS > @join_years ");
                        break;
                    case "less":
                        sb.Append(" and WORK_YEARS < @join_years ");
                        break;
                    case "equal":
                        sb.Append(" and WORK_YEARS = @join_years  ");
                        break;
                    default:
                        break;
                }
                ht.Add("@join_years", join_years);
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
    public int getCount(int startRowIndex, int maximumRows, string emp_id, string dept_no, string emp_chg_cd,
                string leave_dt_s, string leave_dt_e, string accom, string accom_build, string room_no, string age, string age_where, string start_year,
                string start_dt_where, string join_years, string join_years_where)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select COUNT(*) total_record ");
            sb.Append(" from TB_D_M_ACCOM_MAIN a");
            sb.Append(" left join TB_9_M_COMM_D b on a.ACCOM_CD = b.SUB_CD and b.MAIN_CD = 'ACCOM_CD' and b.SYS_CD = 'DF'");
            sb.Append(" left join TB_9_M_COMM_D b2 on a.ACCOM_BUILD_CD = b2.SUB_CD and b2.MAIN_CD = 'ACCOM_BUILD_CD' and b2.SYS_CD = 'DF'");
            sb.Append(" left join TB_H_M_DEPT c on a.DEPT_NO = c.DEPT_NO");
            sb.Append(" left join VW_H_EMP_DATA d on a.EMP_ID = d.EMP_ID");
            sb.Append(" left join TB_9_M_COMM_D b3 on d.EMP_CD = b3.SUB_CD and b3.SYS_CD = 'HB' and b3.MAIN_CD = 'EMP_CD'");
            sb.Append(" where  1 = 1");
            if (emp_id != "")
            {
                sb.Append(" and a.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
            }
            if (dept_no != "")
            {
                sb.Append(" and d.DEPT_NO = @DEPT_NO ");
                ht.Add("@DEPT_NO", dept_no);
            }
            if (emp_chg_cd != "-1")
            {
                sb.Append(" and d.EMP_CHG_CD = @EMP_CHG_CD ");
                ht.Add("@EMP_CHG_CD", emp_chg_cd);
            }
            if (leave_dt_s != "")
            {
                if (leave_dt_e != "")
                {
                    sb.Append(" and LEAVE_DT >= CONVERT(datetime,@leave_dt_s) and LEAVE_DT <= CONVERT(datetime,@leave_dt_e)");
                    ht.Add("@leave_dt_s", leave_dt_s);
                    ht.Add("@leave_dt_e", leave_dt_e);
                }
                else
                {
                    sb.Append(" and LEAVE_DT >= CONVERT(datetime,@leave_dt_s) ");
                    ht.Add("@leave_dt_s", leave_dt_s);
                }
            }
            else if (leave_dt_e != "")
            {
                sb.Append(" and LEAVE_DT <= CONVERT(datetime,@leave_dt_e) ");
                ht.Add("@leave_dt_e", leave_dt_e);
            }
            if (accom != "-1" && accom != null)
            {
                sb.Append(" and b.SUB_CD = @accom ");
                ht.Add("@accom", accom);
            }
            if (accom_build != "-1" && accom_build != null)
            {
                sb.Append(" and b2.SUB_CD = @accom_build ");
                ht.Add("@accom_build", accom_build);
            }
            if (room_no != "")
            {
                sb.Append(" and a.ROOM_NO = @room_no ");
                ht.Add("@room_no", room_no);
            }
            if (age != "")
            {
                switch (age_where)
                {
                    case "greater":
                        sb.Append(" and AGE > @age ");
                        break;
                    case "less":
                        sb.Append(" and AGE < @age ");
                        break;
                    case "equal":
                        sb.Append(" and AGE = @age ");
                        break;
                    default:
                        break;
                }

                ht.Add("@age", age);
            }
            if (start_year != "")
            {
                switch (start_dt_where)
                {
                    case "greater":
                        sb.Append(" and a.START_DT > CONVERT(datetime,@start_year) ");
                        break;
                    case "less":
                        sb.Append(" and a.START_DT < CONVERT(datetime,@start_year) ");
                        break;
                    case "equal":
                        sb.Append(" and a.START_DT = CONVERT(datetime,@start_year) ");
                        break;
                    default:
                        break;
                }

                ht.Add("@start_year", start_year);
            }
            if (join_years != "")
            {
                switch (join_years_where)
                {
                    case "greater":
                        sb.Append(" and WORK_YEARS > @join_years ");
                        break;
                    case "less":
                        sb.Append(" and WORK_YEARS < @join_years ");
                        break;
                    case "equal":
                        sb.Append(" and WORK_YEARS = @join_years  ");
                        break;
                    default:
                        break;
                }
                ht.Add("@join_years", join_years);
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
    //Excel檔查詢資料
    public DataTable searchResult()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select b.SUB_DESC ACCOM_CD,b2.SUB_DESC ACCOM_BUILD_CD,a.ROOM_NO,a.EMP_ID,a.EMP_NAME,b3.SUB_DESC EMP_CD,");
            sb.Append(" d.EMP_CHG_DESC EMP_CHG_CD_DESC,a.DEPT_NO,d.WORK_SHIFT_DESC,d.DEPT_NAME,d.AGE,");
            sb.Append(" CONVERT(char(10), d.JOIN_DT, 111) JOIN_DT,WORK_YEARS,CONVERT(char(10), d.BIRTH_DT, 111) BIRTH_DT,CONVERT(char(10), d.LEAVE_DT, 111) LEAVE_DT,");
            sb.Append(" CONVERT(char(10), a.END_DT, 111) END_DT,a.MOTOR_NO,a.CAR_NO,a.BUS_CD,a.REMARK,d.CONTACT_TEL,");
            sb.Append(" CONVERT(char(10), a.START_DT, 111) START_DT,a.AMOUNT,a.OTHER_AMOUNT,d.CONTACT_ADDR,d.REGISTER_ADDR");
            sb.Append(" from TB_D_M_ACCOM_MAIN a");
            sb.Append(" left join TB_9_M_COMM_D b on a.ACCOM_CD = b.SUB_CD and b.MAIN_CD = 'ACCOM_CD'");
            sb.Append(" left join TB_9_M_COMM_D b2 on a.ACCOM_BUILD_CD = b2.SUB_CD and b2.MAIN_CD = 'ACCOM_BUILD_CD'");
            sb.Append(" left join VW_H_EMP_DATA d on a.EMP_ID = d.EMP_ID");
            sb.Append(" left join TB_9_M_COMM_D b3 on d.EMP_CD = b3.SUB_CD and b3.SYS_CD = 'HB' and b3.MAIN_CD = 'EMP_CD'");
            sb.Append(" where 1 = 1");
            if (EMP_ID != "")
            {
                sb.Append(" and a.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", EMP_ID);
            }
            if (DEPT_NO != "")
            {
                sb.Append(" and d.DEPT_NO = @DEPT_NO ");
                ht.Add("@DEPT_NO", DEPT_NO);
            }
            if (EMP_CHG_CD != "-1")
            {
                sb.Append(" and d.EMP_CHG_CD = @EMP_CHG_CD ");
                ht.Add("@EMP_CHG_CD", EMP_CHG_CD);
            }
            if (LEAVE_DT_S != "")
            {
                if (LEAVE_DT_E != "")
                {
                    sb.Append(" and LEAVE_DT >= CONVERT(datetime,@LEAVE_DT_S) and LEAVE_DT <= CONVERT(datetime,@LEAVE_DT_E)");
                    ht.Add("@LEAVE_DT_S", LEAVE_DT_S);
                    ht.Add("@LEAVE_DT_E", LEAVE_DT_E);
                }
                else
                {
                    sb.Append(" and LEAVE_DT >= CONVERT(datetime,@LEAVE_DT_S) ");
                    ht.Add("@LEAVE_DT_S", LEAVE_DT_S);
                }
            }
            else if (LEAVE_DT_E != "")
            {
                sb.Append(" and LEAVE_DT <= CONVERT(datetime,@LEAVE_DT_E) ");
                ht.Add("@leave_dt_e", LEAVE_DT_E);
            }
            if (ACCOM_CD != "-1" && ACCOM_CD != null)
            {
                sb.Append(" and b.SUB_CD = @accom ");
                ht.Add("@accom", ACCOM_CD);
            }
            if (ACCOM_BUILD_CD != "-1" && ACCOM_BUILD_CD != null)
            {
                sb.Append(" and b2.SUB_CD = @accom_build ");
                ht.Add("@accom_build", ACCOM_BUILD_CD);
            }
            if (ROOM_NO != "")
            {
                sb.Append(" and a.ROOM_NO = @room_no ");
                ht.Add("@room_no", ROOM_NO);
            }
            if (AGE != "")
            {
                switch (age_where)
                {
                    case "greater":
                        sb.Append(" and AGE > @AGE ");
                        break;
                    case "less":
                        sb.Append(" and AGE < @AGE ");
                        break;
                    case "equal":
                        sb.Append(" and AGE = @AGE ");
                        break;
                    default:
                        break;
                }

                ht.Add("@AGE", AGE);
            }
            if (START_DT != "")
            {
                switch (start_dt_where)
                {
                    case "greater":
                        sb.Append(" and a.START_DT > CONVERT(datetime,@START_DT) ");
                        break;
                    case "less":
                        sb.Append(" and a.START_DT < CONVERT(datetime,@START_DT) ");
                        break;
                    case "equal":
                        sb.Append(" and a.START_DT = CONVERT(datetime,@START_DT) ");
                        break;
                    default:
                        break;
                }

                ht.Add("@START_DT", START_DT);
            }
            if (work_year != "")
            {
                switch (work_year_where)
                {
                    case "greater":
                        sb.Append(" and WORK_YEARS > @work_year ");
                        break;
                    case "less":
                        sb.Append(" and WORK_YEARS < @work_year ");
                        break;
                    case "equal":
                        sb.Append(" and WORK_YEARS = @work_year  ");
                        break;
                    default:
                        break;
                }
                ht.Add("@work_year", work_year);
            }

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //取得修改資料
    public DataTable getData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select a.ACCOM_CD ,a.ACCOM_BUILD_CD,a.ROOM_NO,");
            sb.Append(" a.EMP_ID,d.DEPT_NO,d.EMP_NAME,b3.SUB_DESC EMP_CD,c.DEPT_NAME,d.WORK_SHIFT_DESC WORK_SHIFT_CD,d.AGE ,");
            sb.Append(" REPLACE(CONVERT(char(10), d.JOIN_DT, 120),'-','/') JOIN_DT,d.MOBILE_TEL_1,d.CONTACT_TEL,");
            sb.Append(" REPLACE(CONVERT(char(10), a.END_DT, 120),'-','/') END_DT,a.MOTOR_NO,a.CAR_NO,a.BUS_CD,a.REMARK,");
            sb.Append(" REPLACE(CONVERT(char(10), a.START_DT, 120),'-','/') START_DT,a.BASE_NO,a.AMOUNT,a.OTHER_AMOUNT,");
            sb.Append(" (select top 1 ADDRESS from TB_D_M_TRANS_ALLOWANCE_D where d.EMP_ID = TB_D_M_TRANS_ALLOWANCE_D.EMP_ID) CONTACT_ADDR,d.REGISTER_ADDR");
            sb.Append(" from TB_D_M_ACCOM_MAIN a");
            sb.Append(" left join TB_9_M_COMM_D b on a.ACCOM_CD = b.SUB_CD and b.MAIN_CD = 'ACCOM_CD' and b.SYS_CD = 'DF'");
            sb.Append(" left join TB_9_M_COMM_D b2 on a.ACCOM_BUILD_CD = b2.SUB_CD and b2.MAIN_CD = 'ACCOM_BUILD_CD' and b2.SYS_CD = 'DF'");
            sb.Append(" left join TB_H_M_DEPT c on a.DEPT_NO = c.DEPT_NO");
            sb.Append(" left join VW_H_EMP_DATA d on a.EMP_ID = d.EMP_ID");
            sb.Append(" left join TB_9_M_COMM_D b3 on d.EMP_CD = b3.SUB_CD and b3.SYS_CD = 'HB' and b3.MAIN_CD = 'EMP_CD'");            
            sb.Append(" where a.EMP_ID = @EMP_ID ");
            ht.Add("@EMP_ID", EMP_ID);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //新增歷史檔 
    //將結束日期改為主檔.生效日，作為比對資料用
    internal void addHistory(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("insert into TB_D_M_ACCOM_HISTORY(EMP_ID,EMP_NAME,LICENSE_ID,COMPANY_CD,DEPT_NO,DEPT_NAME,START_DT,END_DT,");
            sb.Append("ACCOM_CD,ACCOM_BUILD_CD,ROOM_NO,BASE_NO,AMOUNT,OTHER_AMOUNT,REMARK,BUS_CD,MOTOR_NO,CAR_NO,CREATED_BY,CREATED_DT,");
            sb.Append("UPDATED_BY,UPDATED_DT,FUNC_ID)  ");
            sb.Append(" select a.EMP_ID,a.EMP_NAME,a.LICENSE_ID,a.COMPANY_CD,a.DEPT_NO,DEPT_NAME,START_DT,@START_DT,");
            sb.Append(" ACCOM_CD,ACCOM_BUILD_CD,ROOM_NO,BASE_NO,AMOUNT,OTHER_AMOUNT,a.REMARK,BUS_CD,MOTOR_NO,CAR_NO,CREATED_BY,CREATED_DT,");
            sb.Append(" @UPDATED_BY,getdate(),FUNC_ID from TB_D_M_ACCOM_MAIN a,VW_H_EMP_DATA b where a.EMP_ID = b.EMP_ID and a.EMP_ID = @EMP_ID  ");

            ht.Add("@EMP_ID", emp_id);
            ht.Add("@START_DT", START_DT);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {

            throw;
        }
    }

    //刪除資料
    public void deleteData(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //寫log
            sb.Append(" update TB_D_M_ACCOM_MAIN set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2DF020' ");
            sb.Append(" where EMP_ID = @EMP_ID;");
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);

            sb.Append(" Delete from TB_D_M_ACCOM_MAIN   ");
            sb.Append(" where EMP_ID = @EMP_ID;");
            ht.Add("@EMP_ID", emp_id);
            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得是否需住宿費
    internal DataTable getCode_Val(string sub_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select CODE_VAL1 from TB_9_M_COMM_D where MAIN_CD = 'ACCOM_CD' and SUB_CD = @SUB_CD ");
            ht.Add("@SUB_CD", sub_cd);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //查詢同房間人員輪值別
    internal DataTable checkWorkShift(string EMP_ID, string ROOM_NO)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select WORK_SHIFT_CD from VW_H_EMP_DATA a,TB_D_M_ACCOM_MAIN b where a.EMP_ID = b.EMP_ID and b.ROOM_NO = @ROOM_NO");
            sb.Append(" and b.EMP_ID <> @EMP_ID");
            ht.Add("@ROOM_NO", ROOM_NO);
            ht.Add("@EMP_ID", EMP_ID);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //查詢現有資料
    internal DataTable getExistData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select EMP_ID,CONVERT(char(10), END_DT, 120) END_DT from TB_D_M_ACCOM_MAIN where EMP_ID = @EMP_ID");
            ht.Add("@EMP_ID", EMP_ID);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //更新住宿主檔
    internal void updateAccom()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_D_M_ACCOM_MAIN");
            sb.Append(" set EMP_NAME = S.EMP_NAME,");
            sb.Append(" COMPANY_CD = S.COMPANY_CD,DEPT_NO = S.DEPT_NO,");
            sb.Append(" START_DT = @START_DT,END_DT = @END_DT,ACCOM_CD = @ACCOM_CD,ACCOM_BUILD_CD = @ACCOM_BUILD_CD,");
            sb.Append(" ROOM_NO = @ROOM_NO,BASE_NO = @BASE_NO,AMOUNT = @AMOUNT,OTHER_AMOUNT = @OTHER_AMOUNT,REMARK = @REMARK,");
            sb.Append(" BUS_CD = @BUS_CD,MOTOR_NO = @MOTOR_NO,CAR_NO = @CAR_NO,UPDATED_BY =@UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID");
            sb.Append(" FROM TB_D_M_ACCOM_MAIN T inner join TB_H_M_EMP S ");
            sb.Append(" on T.EMP_ID = S.EMP_ID");
            sb.Append(" where S.EMP_ID = @EMP_ID");
            
            ht.Add("@START_DT", START_DT);
            if (END_DT == "")
                ht.Add("@END_DT", "9999/12/31");
            else
                ht.Add("@END_DT", END_DT);
            ht.Add("@ACCOM_CD", ACCOM_CD);
            ht.Add("@ACCOM_BUILD_CD", ACCOM_BUILD_CD);
            ht.Add("@ROOM_NO", ROOM_NO);
            ht.Add("@BASE_NO", BASE_NO);
            ht.Add("@AMOUNT", AMOUNT);
            if (OTHER_AMOUNT == "")
                ht.Add("@OTHER_AMOUNT", 0);
            else
                ht.Add("@OTHER_AMOUNT", OTHER_AMOUNT);
            ht.Add("@REMARK", REMARK);
            ht.Add("@BUS_CD", BUS_CD);
            ht.Add("@MOTOR_NO", MOTOR_NO);
            ht.Add("@CAR_NO", CAR_NO);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@FUNC_ID", FUNC_ID);
            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {

            throw;
        }
    }

    //新增住宿主檔
    internal void addAccom()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Insert into TB_D_M_ACCOM_MAIN (EMP_ID,LICENSE_ID,EMP_NAME,COMPANY_CD,DEPT_NO,START_DT,END_DT,ACCOM_CD,ACCOM_BUILD_CD,");
            sb.Append(" ROOM_NO,BASE_NO,AMOUNT,OTHER_AMOUNT,REMARK,BUS_CD,MOTOR_NO,CAR_NO,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" select s.EMP_ID,s.LICENSE_ID,s.EMP_NAME,s.COMPANY_CD,s.DEPT_NO,@START_DT,@END_DT,@ACCOM_CD,@ACCOM_BUILD_CD,");
            sb.Append(" @ROOM_NO,@BASE_NO,@AMOUNT,@OTHER_AMOUNT,@REMARK,@BUS_CD,@MOTOR_NO,@CAR_NO,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID");
            sb.Append(" from TB_H_M_EMP S where S.EMP_ID = @EMP_ID");
            
            ht.Add("@START_DT", START_DT);
            if (END_DT == "")
                ht.Add("@END_DT", "9999/12/31");
            else
                ht.Add("@END_DT", END_DT);
            ht.Add("@ACCOM_CD", ACCOM_CD);
            ht.Add("@ACCOM_BUILD_CD", ACCOM_BUILD_CD);
            ht.Add("@ROOM_NO", ROOM_NO);
            ht.Add("@BASE_NO", BASE_NO);
            ht.Add("@AMOUNT", AMOUNT);
            if (OTHER_AMOUNT == "")
                ht.Add("@OTHER_AMOUNT", 0);
            else
                ht.Add("@OTHER_AMOUNT", OTHER_AMOUNT);
            ht.Add("@REMARK", REMARK);
            ht.Add("@BUS_CD", BUS_CD);
            ht.Add("@MOTOR_NO", MOTOR_NO);
            ht.Add("@CAR_NO", CAR_NO);
            ht.Add("@CREATED_BY", CREATED_BY);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getEmpData()
    {
        try
        {
             StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select EMP_ID from TB_H_M_EMP where EMP_ID = @EMP_ID");
            ht.Add("@EMP_ID", EMP_ID);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            
            throw;
        }
    }
}
