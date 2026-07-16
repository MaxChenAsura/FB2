using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;
using System.Text;
using System.Collections;

/// <summary>
/// CFB2SI0300DAO 的摘要描述
/// </summary>
public class CFB2SI0300DAO : BaseDAO
{
    public string BONUS_YEAR { get; set; }
    public string BONUS_DAYS { get; set; }
    public string BONUS_DT { get; set; }
    public string BONUS_TOTAL_DECIMAL { get; set; }
    public string BONUS_TOTAL_AMOUNT { get; set; }
    public string APPROVE_STATUS { get; set; }
    public string EMP_ID { get; set; }
    public string REMARK { get; set; }
    public string SALARY_TRANS_DT { get; set; }
    public string FREEZE_FLAG { get; set; }
    public string APPROVE_MARK { get; set; }
    public string APPROVE_DT { get; set; }
    public string APPROVE_BY { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }
    public string RELEASE_DT { get; set; }
    //GetNum
    public string B_LEAVE_UC { get; set; }
    public string B_LEAVE_B { get; set; }
    public string B_LEAVE_Q { get; set; }
    public string B_LEAVE_OP { get; set; }
    public string B_FIRST_CNT_P { get; set; }
    public string B_SECOND_CNT_P { get; set; }
    public string B_THIRD_CNT_P { get; set; }
    public string B_FIRST_CNT_M { get; set; }
    public string B_SECOND_CNT_M { get; set; }
    public string B_THIRD_CNT_M { get; set; }
    public CFB2SI0300DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public DataTable GetData(int startRowIndex, int maximumRows, string sortExpression, string bonus_year_s, string bonus_year_e)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + ") As RowNumber,");
            sb.Append(" a.BONUS_YEAR,a.APPROVE_STATUS+'-'+b.SUB_DESC as APPROVE_STATUS,a.BONUS_DAYS,a.BONUS_DT,c.EMP_NAME,a.RELEASE_DT,a.APPROVE_BY,a.APPROVE_DT,a.SALARY_TRANS_DT");
            sb.Append(" from TB_S_M_BONUS_H a");
            sb.Append(" left join TB_9_M_COMM_D b on b.SYS_CD='SA' and b.MAIN_CD='APPROVE_STATUS' and b.SUB_CD=a.APPROVE_STATUS ");
            sb.Append(" left join TB_H_M_EMP c on a.RELEASE_BY=c.EMP_ID ");
            sb.Append(" where 1=1 ");

            //紅利年度start
            if (bonus_year_s.Trim() != "")
            {
                sb.Append("and a.BONUS_YEAR >= @bonus_year_s ");
                ht.Add("bonus_year_s", bonus_year_s);

            }
            if (bonus_year_e.Trim() != "")
            {
                sb.Append("and a.BONUS_YEAR <= @bonus_year_e ");
                ht.Add("bonus_year_e", bonus_year_e);


            }


            sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar) ");


            ht.Add("startRowIndex", startRowIndex);
            ht.Add("maximumRows", maximumRows);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public int GetCount(int startRowIndex, int maximumRows, string bonus_year_s, string bonus_year_e)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select COUNT(*) total_record From TB_S_M_BONUS_H");
            sb.Append(" where 1=1 ");

            //紅利年度start
            if (bonus_year_s.Trim() != "")
            {
                sb.Append("and BONUS_YEAR >= @bonus_year_s ");
                ht.Add("bonus_year_s", bonus_year_s);

            }
            if (bonus_year_e.Trim() != "")
            {
                sb.Append("and BONUS_YEAR <= @bonus_year_e ");
                ht.Add("bonus_year_e", bonus_year_e);


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


    /// <summary>
    /// WFB2SI0300_Dtl
    /// </summary>
    /// 
    //功能鍵disabled與否
    public int GetEmpCount()
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select count(c.EMP_ID) total_record");
            sb.Append(" from TB_S_M_BONUS_H a");
            sb.Append(" left join TB_H_M_EMP b on a.RELEASE_BY = b.EMP_ID");
            sb.Append(" left join TB_H_R_HEAD_DEPT c on b.DEPT_NO=c.MNG_DEPT_NO");
            sb.Append(" where  c.EMP_ID=@EMP_ID");
            ht.Add("EMP_ID", SessionHandle.Current.emp_id);

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
    public void GetDtlData()
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select BONUS_DAYS,BONUS_DT,BONUS_TOTAL_AMOUNT,BONUS_TOTAL_DECIMAL,SALARY_TRANS_DT,APPROVE_STATUS,");
            sb.Append(" REMARK,FREEZE_FLAG,APPROVE_DT,APPROVE_BY,RELEASE_DT");
            sb.Append(" from TB_S_M_BONUS_H  ");
            sb.Append(" where BONUS_YEAR = @BONUS_YEAR");
            ht.Add("BONUS_YEAR", BONUS_YEAR);
            DataTable dt = dbConn.Query(sb, ht);
            foreach (DataRow dr in dt.Rows)
            {
                this.BONUS_DAYS = Convert.ToString(dr["BONUS_DAYS"]);
                this.BONUS_DT = Convert.ToString(dr["BONUS_DT"]);
                this.BONUS_TOTAL_AMOUNT = Convert.ToString(dr["BONUS_TOTAL_AMOUNT"]);
                this.BONUS_TOTAL_DECIMAL = Convert.ToString(dr["BONUS_TOTAL_DECIMAL"]);
                this.SALARY_TRANS_DT = Convert.ToString(dr["SALARY_TRANS_DT"]);
                this.APPROVE_STATUS = Convert.ToString(dr["APPROVE_STATUS"]);
                this.REMARK = Convert.ToString(dr["REMARK"]);
                this.FREEZE_FLAG = Convert.ToString(dr["FREEZE_FLAG"]);
                this.APPROVE_DT = Convert.ToString(dr["APPROVE_DT"]);
                this.APPROVE_BY = Convert.ToString(dr["APPROVE_BY"]);
                this.RELEASE_DT = Convert.ToString(dr["RELEASE_DT"]);
            }

        }
        catch
        {
            throw;
        }

    }
    //查詢明細
    public DataTable GetDtlData2(int startRowIndex, int maximumRows, string sortExpression, string bonus_year, string emp_id, string emp_name)
    {
        try
        {
            if (sortExpression.Contains("APPROVE_MARK DESC,APPROVE_FLAG ASC,UPDATED_DT DESC,WS_CD ASC,EMP_ID"))
                sortExpression = sortExpression.Replace("APPROVE_MARK DESC,APPROVE_FLAG ASC,UPDATED_DT DESC,WS_CD ASC,EMP_ID", "a.APPROVE_MARK DESC,a.APPROVE_FLAG ASC,a.UPDATED_DT DESC,a.WS_CD ASC,a.EMP_ID");
            
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" Select * From");
            sb.AppendLine(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.AppendLine("       a.APPROVE_MARK,a.CHG_STATUS+'-'+c.SUB_DESC as CHG_STATUS,a.APPROVE_FLAG+'-'+b.SUB_DESC as APPROVE_FLAG");
            sb.AppendLine("      ,a.WS_CD+'-'+e.SUB_DESC as WS_CD,a.LEVEL_CD,a.EMP_ID,a.EMP_NAME                                        ");
            sb.AppendLine("      ,a.WORK_DAYS,a.ATTEND_DAYS,a.BONUS_WORK_DAYS,a.REWARD_DAYS,a.DISCIPLINE_DAYS,a.BONUS_AMT               ");
            sb.AppendLine("      ,a.PAY_TYPE+'-'+f.SUB_DESC as PAY_TYPE,a.EMP_CHG_CD+'-'+d.SUB_DESC as EMP_CHG_CD,a.UPDATED_DT          ");
            sb.AppendLine("from TB_S_M_BONUS_D a                                                                                        ");
            sb.AppendLine("left join TB_9_M_COMM_D b on b.SYS_CD='SA' and b.MAIN_CD='APPROVE_STATUS' and b.SUB_CD=a.APPROVE_FLAG        ");
            sb.AppendLine("left join TB_9_M_COMM_D c on c.SYS_CD='SA' and c.MAIN_CD='CHG_STATUS' and c.SUB_CD=a.CHG_STATUS              ");
            sb.AppendLine("left join TB_9_M_COMM_D d on d.SYS_CD='HB' and d.MAIN_CD='EMP_CHG_CD' and d.SUB_CD=a.EMP_CHG_CD              ");
            sb.AppendLine("left join TB_9_M_COMM_D e on e.SYS_CD='HB' and e.MAIN_CD='WS_CD' and e.SUB_CD=a.WS_CD                        ");
            sb.AppendLine("left join TB_9_M_COMM_D f on f.SYS_CD='SC' and f.MAIN_CD='PAY_TYPE' and f.SUB_CD=a.PAY_TYPE                  ");
            sb.AppendLine(" where a.BONUS_YEAR = @BONUS_YEAR ");
            if (emp_id.Trim() != "")
            {
                sb.Append(" and a.EMP_ID = @emp_id ");
                ht.Add("emp_id", emp_id);

            }
            if (emp_name.Trim() != "")
            {
                sb.Append(" and a.EMP_NAME like @emp_name ");
                ht.Add("emp_name", emp_name + "%");

            }
            sb.AppendLine(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");

            sb.AppendLine(" AND CAST(@startRowIndex+@maximumRows as varchar) ");


            ht.Add("BONUS_YEAR", bonus_year);
            ht.Add("startRowIndex", startRowIndex);
            ht.Add("maximumRows", maximumRows);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public int GetDtlCount(int startRowIndex, int maximumRows, string bonus_year, string emp_id, string emp_name)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select COUNT(*) total_record From TB_S_M_BONUS_D");
            sb.Append(" where BONUS_YEAR = @BONUS_YEAR ");
            if (emp_id.Trim() != "")
            {
                sb.Append(" and EMP_ID = @emp_id ");
                ht.Add("emp_id", emp_id);

            }
            if (emp_name.Trim() != "")
            {
                sb.Append(" and EMP_NAME like @emp_name ");
                ht.Add("emp_name", emp_name + "%");

            }
            ht.Add("BONUS_YEAR", bonus_year);

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

    //excel下載資料table
    public DataTable getExcelData(string data, string BONUS_YEAR)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            if (data == "this")
            {
                sb.AppendLine("Select * ,a.EMP_CHG_CD+'-'+c.sub_desc as EMP_CHG_CD_desc                                       ");
                sb.AppendLine("      ,a.EMP_CD+'-'+d.sub_desc as EMP_CD_desc,a.CHG_STATUS+'-'+e.sub_desc as CHG_STATUS_desc   ");
                sb.AppendLine("from TB_S_M_BONUS_D a                                                                          ");
                sb.AppendLine("left join TB_9_M_COMM_D c on c.SYS_CD='HB' and c.MAIN_CD='EMP_CHG_CD' and c.SUB_CD=a.EMP_CHG_CD");
                sb.AppendLine("left join TB_9_M_COMM_D d on d.SYS_CD='HB' and d.MAIN_CD='EMP_CD' and d.SUB_CD=a.EMP_CD        ");
                sb.AppendLine("left join TB_9_M_COMM_D e on e.SYS_CD='SA' and e.MAIN_CD='CHG_STATUS' and e.SUB_CD=a.CHG_STATUS");
                sb.AppendLine("where a.BONUS_YEAR = @BONUS_YEAR");
                sb.Append(" ORDER BY a.DEPT_NO, a.APPROVE_MARK desc, a.APPROVE_FLAG, a.UPDATED_DT desc, a.WS_CD, a.EMP_ID");

                //sb.Append(" Select *");
                //sb.Append(" from TB_S_M_BONUS_D ");
                //sb.Append(" where BONUS_YEAR = @BONUS_YEAR ");
                //sb.Append(" ORDER BY DEPT_NO, APPROVE_MARK desc, APPROVE_FLAG, UPDATED_DT desc, WS_CD, EMP_ID");
            }
            else if (data == "prev")
            {
                sb.AppendLine("Select a.EMP_ID M_EMP_ID,b.EMP_ID S_EMP_ID                                                     ");
	            sb.AppendLine("      ,a.EMP_NAME M_EMP_NAME ,b.EMP_NAME S_EMP_NAME                                            ");
	            sb.AppendLine("      ,a.EMP_CHG_CD+'-'+c.SUB_DESC M_EMP_CHG_CD ,b.EMP_CHG_CD+'-'+f.SUB_DESC S_EMP_CHG_CD      ");
	            sb.AppendLine("      ,a.WS_CD M_WS_CD ,b.WS_CD S_WS_CD                                                        ");
	            sb.AppendLine("      ,a.JPN_CD M_JPN_CD ,b.JPN_CD S_JPN_CD                                                    ");
	            sb.AppendLine("      ,a.DEPT_NO M_DEPT_NO ,b.DEPT_NO S_DEPT_NO                                                ");
	            sb.AppendLine("      ,a.LEVEL_CD M_LEVEL_CD ,b.LEVEL_CD S_LEVEL_CD                                            ");
	            sb.AppendLine("      ,a.PJOB_CD M_PJOB_CD ,b.PJOB_CD S_PJOB_CD                                                ");
	            sb.AppendLine("      ,a.JOIN_DT M_JOIN_DT ,b.JOIN_DT S_JOIN_DT                                                ");
	            sb.AppendLine("      ,a.LEAVE_DT M_LEAVE_DT ,b.LEAVE_DT S_LEAVE_DT                                            ");
	            sb.AppendLine("      ,a.STAY_DT M_STAY_DT ,b.STAY_DT S_STAY_DT                                                ");
	            sb.AppendLine("      ,a.BE_CONTRACT_DT M_BE_CONTRACT_DT ,b.BE_CONTRACT_DT S_BE_CONTRACT_DT                    ");
	            sb.AppendLine("      ,a.BE_EMP_DT M_BE_EMP_DT ,b.BE_EMP_DT S_BE_EMP_DT                                        ");
	            sb.AppendLine("      ,a.EMP_CD+'-'+d.SUB_DESC M_EMP_CD ,b.EMP_CD+'-'+g.SUB_DESC S_EMP_CD                      ");
	            sb.AppendLine("      ,a.ID_DESC M_ID_DESC ,b.ID_DESC S_ID_DESC                                                ");
                sb.AppendLine("                                                                                               ");
	            sb.AppendLine("      ,a.WORK_DAYS M_WORK_DAYS ,b.WORK_DAYS S_WORK_DAYS                                        ");
	            sb.AppendLine("      ,a.LEVEL_PAY M_LEVEL_PAY,b.LEVEL_PAY S_LEVEL_PAY                                         ");
	            sb.AppendLine("      ,a.ABILITY_PAY M_ABILITY_PAY,b.ABILITY_PAY S_ABILITY_PAY                                 ");
	            sb.AppendLine("      ,a.PJOB_PAY M_PJOB_PAY,b.PJOB_PAY S_PJOB_PAY                                             ");
	            sb.AppendLine("      ,a.PROFESSION_PAY M_PROFESSION_PAY,b.PROFESSION_PAY S_PROFESSION_PAY                     ");
	            sb.AppendLine("      ,a.FOOD_SUBSIDY M_FOOD_SUBSIDY,b.FOOD_SUBSIDY S_FOOD_SUBSIDY                             ");
	            sb.AppendLine("      ,a.LEAVE_A_HOUR M_LEAVE_A_HOUR,b.LEAVE_A_HOUR S_LEAVE_A_HOUR                             ");
	            sb.AppendLine("      ,a.LEAVE_B_HOUR M_LEAVE_B_HOUR,b.LEAVE_B_HOUR S_LEAVE_B_HOUR                             ");
	            sb.AppendLine("      ,a.LEAVE_C_HOUR M_LEAVE_C_HOUR,b.LEAVE_C_HOUR S_LEAVE_C_HOUR                             ");
	            sb.AppendLine("      ,a.LEAVE_Q_HOUR M_LEAVE_Q_HOUR,b.LEAVE_Q_HOUR S_LEAVE_Q_HOUR                             ");
	            sb.AppendLine("      ,a.LEAVE_OP_HOUR M_LEAVE_OP_HOUR,b.LEAVE_OP_HOUR S_LEAVE_OP_HOUR                         ");
	            sb.AppendLine("      ,a.THIRD_CNT_P M_THIRD_CNT_P,b.THIRD_CNT_P S_THIRD_CNT_P                                 ");
	            sb.AppendLine("      ,a.SECOND_CNT_P M_SECOND_CNT_P,b.SECOND_CNT_P S_SECOND_CNT_P                             ");
	            sb.AppendLine("      ,a.FIRST_CNT_P M_FIRST_CNT_P,b.FIRST_CNT_P S_FIRST_CNT_P                                 ");
	            sb.AppendLine("      ,a.THIRD_CNT_M M_THIRD_CNT_M,b.THIRD_CNT_M S_THIRD_CNT_M                                 ");
	            sb.AppendLine("      ,a.SECOND_CNT_M M_SECOND_CNT_M,b.SECOND_CNT_M S_SECOND_CNT_M                             ");
	            sb.AppendLine("      ,a.FIRST_CNT_M M_FIRST_CNT_M,b.FIRST_CNT_M S_FIRST_CNT_M                                 ");
	            sb.AppendLine("      ,a.ATTEND_DAYS M_ATTEND_DAYS,b.ATTEND_DAYS S_ATTEND_DAYS                                 ");
	            sb.AppendLine("      ,a.REWARD_DAYS M_REWARD_DAYS,b.REWARD_DAYS S_REWARD_DAYS                                 ");
	            sb.AppendLine("      ,a.DISCIPLINE_DAYS M_DISCIPLINE_DAYS,b.DISCIPLINE_DAYS S_DISCIPLINE_DAYS                 ");
	            sb.AppendLine("      ,a.BONUS_WORK_DAYS M_BONUS_WORK_DAYS,b.BONUS_WORK_DAYS S_BONUS_WORK_DAYS                 ");
	            sb.AppendLine("      ,a.BONUS_AMT M_BONUS_AMT,b.BONUS_AMT S_BONUS_AMT                                         ");
	            sb.AppendLine("      ,a.BONUS_TAX M_BONUS_TAX,b.BONUS_TAX S_BONUS_TAX                                         ");
	            sb.AppendLine("      ,a.BONUS_AMT_R M_BONUS_AMT_R,b.BONUS_AMT_R S_BONUS_AMT_R                                 ");
	            sb.AppendLine("      ,a.PAY_TYPE M_PAY_TYPE,b.PAY_TYPE S_PAY_TYPE                                             ");
	            sb.AppendLine("      ,a.CHG_STATUS+'-'+e.SUB_DESC M_CHG_STATUS,b.CHG_STATUS+'-'+h.SUB_DESC S_CHG_STATUS       ");
                sb.AppendLine("from TB_S_M_BONUS_D a                                                                          ");
                sb.AppendLine("left join TB_S_R_BONUS_D b on a.BONUS_YEAR=b.BONUS_YEAR                                        ");
                sb.AppendLine("left join TB_9_M_COMM_D c on c.SYS_CD='HB' and c.MAIN_CD='EMP_CHG_CD' and c.SUB_CD=a.EMP_CHG_CD");
                sb.AppendLine("left join TB_9_M_COMM_D d on d.SYS_CD='HB' and d.MAIN_CD='EMP_CD' and d.SUB_CD=a.EMP_CD        ");
                sb.AppendLine("left join TB_9_M_COMM_D e on e.SYS_CD='SA' and e.MAIN_CD='CHG_STATUS' and e.SUB_CD=a.CHG_STATUS");
                sb.AppendLine("left join TB_9_M_COMM_D f on f.SYS_CD='HB' and f.MAIN_CD='EMP_CHG_CD' and f.SUB_CD=b.EMP_CHG_CD");
                sb.AppendLine("left join TB_9_M_COMM_D g on g.SYS_CD='HB' and g.MAIN_CD='EMP_CD' and g.SUB_CD=b.EMP_CD        ");
                sb.AppendLine("left join TB_9_M_COMM_D h on h.SYS_CD='SA' and h.MAIN_CD='CHG_STATUS' and h.SUB_CD=b.CHG_STATUS");
                sb.AppendLine("                                                                                               ");
                sb.AppendLine("where a.BONUS_YEAR = @BONUS_YEAR and b.BONUS_YEAR = @BONUS_YEAR                                ");
                sb.AppendLine("and a.APPROVE_FLAG = 'N' and a.EMP_ID=b.EMP_ID                                                 ");
            }
            else if (data == "original")
            {
                sb.AppendLine(" Select a.EMP_ID M_EMP_ID,b.EMP_ID S_EMP_ID");
                sb.AppendLine(" ,a.EMP_NAME M_EMP_NAME ,b.EMP_NAME S_EMP_NAME");
                sb.AppendLine(" ,a.EMP_CHG_CD+'-'+c.SUB_DESC M_EMP_CHG_CD ,b.EMP_CHG_CD+'-'+f.SUB_DESC S_EMP_CHG_CD");
                sb.AppendLine(" ,a.WS_CD M_WS_CD ,b.WS_CD S_WS_CD");
                sb.AppendLine(" ,a.JPN_CD M_JPN_CD ,b.JPN_CD S_JPN_CD");
                sb.AppendLine(" ,a.DEPT_NO M_DEPT_NO ,b.DEPT_NO S_DEPT_NO");
                sb.AppendLine(" ,a.LEVEL_CD M_LEVEL_CD ,b.LEVEL_CD S_LEVEL_CD");
                sb.AppendLine(" ,a.PJOB_CD M_PJOB_CD ,b.PJOB_CD S_PJOB_CD");
                sb.AppendLine(" ,a.JOIN_DT M_JOIN_DT ,b.JOIN_DT S_JOIN_DT");
                sb.AppendLine(" ,a.LEAVE_DT M_LEAVE_DT ,b.LEAVE_DT S_LEAVE_DT");
                sb.AppendLine(" ,a.STAY_DT M_STAY_DT ,b.STAY_DT S_STAY_DT");
                sb.AppendLine(" ,a.BE_CONTRACT_DT M_BE_CONTRACT_DT ,b.BE_CONTRACT_DT S_BE_CONTRACT_DT");
                sb.AppendLine(" ,a.BE_EMP_DT M_BE_EMP_DT ,b.BE_EMP_DT S_BE_EMP_DT");
                sb.AppendLine(" ,a.EMP_CD+'-'+d.SUB_DESC M_EMP_CD ,b.EMP_CD+'-'+g.SUB_DESC S_EMP_CD");
                sb.AppendLine(" ,a.ID_DESC M_ID_DESC ,b.ID_DESC S_ID_DESC");
                         
                sb.AppendLine(" ,a.WORK_DAYS M_WORK_DAYS ,b.WORK_DAYS S_WORK_DAYS");
                sb.AppendLine(" ,a.LEVEL_PAY M_LEVEL_PAY,b.LEVEL_PAY S_LEVEL_PAY");
                sb.AppendLine(" ,a.ABILITY_PAY M_ABILITY_PAY,b.ABILITY_PAY S_ABILITY_PAY");
                sb.AppendLine(" ,a.PJOB_PAY M_PJOB_PAY,b.PJOB_PAY S_PJOB_PAY");
                sb.AppendLine(" ,a.PROFESSION_PAY M_PROFESSION_PAY,b.PROFESSION_PAY S_PROFESSION_PAY");
                sb.AppendLine(" ,a.FOOD_SUBSIDY M_FOOD_SUBSIDY,b.FOOD_SUBSIDY S_FOOD_SUBSIDY");
                sb.AppendLine(" ,a.LEAVE_A_HOUR M_LEAVE_A_HOUR,b.LEAVE_A_HOUR S_LEAVE_A_HOUR");
                sb.AppendLine(" ,a.LEAVE_B_HOUR M_LEAVE_B_HOUR,b.LEAVE_B_HOUR S_LEAVE_B_HOUR");
                sb.AppendLine(" ,a.LEAVE_C_HOUR M_LEAVE_C_HOUR,b.LEAVE_C_HOUR S_LEAVE_C_HOUR");
                sb.AppendLine(" ,a.LEAVE_Q_HOUR M_LEAVE_Q_HOUR,b.LEAVE_Q_HOUR S_LEAVE_Q_HOUR");
                sb.AppendLine(" ,a.LEAVE_OP_HOUR M_LEAVE_OP_HOUR,b.LEAVE_OP_HOUR S_LEAVE_OP_HOUR");
                sb.AppendLine(" ,a.THIRD_CNT_P M_THIRD_CNT_P,b.THIRD_CNT_P S_THIRD_CNT_P");
                sb.AppendLine(" ,a.SECOND_CNT_P M_SECOND_CNT_P,b.SECOND_CNT_P S_SECOND_CNT_P");
                sb.AppendLine(" ,a.FIRST_CNT_P M_FIRST_CNT_P,b.FIRST_CNT_P S_FIRST_CNT_P");
                sb.AppendLine(" ,a.THIRD_CNT_M M_THIRD_CNT_M,b.THIRD_CNT_M S_THIRD_CNT_M");
                sb.AppendLine(" ,a.SECOND_CNT_M M_SECOND_CNT_M,b.SECOND_CNT_M S_SECOND_CNT_M");
                sb.AppendLine(" ,a.FIRST_CNT_M M_FIRST_CNT_M,b.FIRST_CNT_M S_FIRST_CNT_M");
                sb.AppendLine(" ,a.ATTEND_DAYS M_ATTEND_DAYS,b.ATTEND_DAYS S_ATTEND_DAYS");
                sb.AppendLine(" ,a.REWARD_DAYS M_REWARD_DAYS,b.REWARD_DAYS S_REWARD_DAYS");
                sb.AppendLine(" ,a.DISCIPLINE_DAYS M_DISCIPLINE_DAYS,b.DISCIPLINE_DAYS S_DISCIPLINE_DAYS");
                sb.AppendLine(" ,a.BONUS_WORK_DAYS M_BONUS_WORK_DAYS,b.BONUS_WORK_DAYS S_BONUS_WORK_DAYS");
                sb.AppendLine(" ,a.BONUS_AMT M_BONUS_AMT,b.BONUS_AMT S_BONUS_AMT");
                sb.AppendLine(" ,a.BONUS_TAX M_BONUS_TAX,b.BONUS_TAX S_BONUS_TAX");
                sb.AppendLine(" ,a.BONUS_AMT_R M_BONUS_AMT_R,b.BONUS_AMT_R S_BONUS_AMT_R");
                sb.AppendLine(" ,a.PAY_TYPE M_PAY_TYPE,b.PAY_TYPE S_PAY_TYPE");
                sb.AppendLine(" ,a.CHG_STATUS+'-'+e.SUB_DESC M_CHG_STATUS,b.CHG_STATUS+'-'+h.SUB_DESC S_CHG_STATUS");
                sb.AppendLine(" from TB_S_M_BONUS_D a ");
                sb.AppendLine("left join TB_S_S_BONUS_D b on a.BONUS_YEAR=b.BONUS_YEAR                                        ");
                sb.AppendLine("left join TB_9_M_COMM_D c on c.SYS_CD='HB' and c.MAIN_CD='EMP_CHG_CD' and c.SUB_CD=a.EMP_CHG_CD");
                sb.AppendLine("left join TB_9_M_COMM_D d on d.SYS_CD='HB' and d.MAIN_CD='EMP_CD' and d.SUB_CD=a.EMP_CD        ");
                sb.AppendLine("left join TB_9_M_COMM_D e on e.SYS_CD='SA' and e.MAIN_CD='CHG_STATUS' and e.SUB_CD=a.CHG_STATUS");
                sb.AppendLine("left join TB_9_M_COMM_D f on f.SYS_CD='HB' and f.MAIN_CD='EMP_CHG_CD' and f.SUB_CD=b.EMP_CHG_CD");
                sb.AppendLine("left join TB_9_M_COMM_D g on g.SYS_CD='HB' and g.MAIN_CD='EMP_CD' and g.SUB_CD=b.EMP_CD        ");
                sb.AppendLine("left join TB_9_M_COMM_D h on h.SYS_CD='SA' and h.MAIN_CD='CHG_STATUS' and h.SUB_CD=b.CHG_STATUS");
                sb.AppendLine(" where a.BONUS_YEAR = @BONUS_YEAR and b.BONUS_YEAR = @BONUS_YEAR ");
                sb.AppendLine(" and a.PRIMEVAL_FLAG <> 'N' and a.EMP_ID=b.EMP_ID");
            }
            ht.Add("BONUS_YEAR", BONUS_YEAR);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //取得前次核可的資料
    public string getPrevData(string BONUS_YEAR)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select *");
            sb.Append(" from TB_S_R_BONUS_D");
            sb.Append(" where BONUS_YEAR = @BONUS_YEAR");
            ht.Add("BONUS_YEAR", BONUS_YEAR);
            DataTable checkdt = dbConn.Query(sb, ht);
            if (checkdt.Rows.Count <= 0)
            {
                return "N";
            }
            else
            {
                return "Y";
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
    //取得新增的對象資料(原始資料、前次核可)
    public DataTable getAddExcelData(string data, string BONUS_YEAR)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            if (data == "prev")
            {
                sb.AppendLine("Select distinct a.*,a.EMP_CHG_CD+'-'+c.SUB_DESC as EMP_CHG_CD_desc                             ");
			    sb.AppendLine("               ,a.EMP_CD+'-'+d.SUB_DESC as EMP_CD_desc                                         ");
			    sb.AppendLine("               ,a.CHG_STATUS+'-'+e.SUB_DESC as CHG_STATUS_desc                                 ");
                sb.AppendLine("from TB_S_M_BONUS_D a                                                                          ");
                sb.AppendLine("left join TB_9_M_COMM_D c on c.SYS_CD='HB' and c.MAIN_CD='EMP_CHG_CD' and c.SUB_CD=a.EMP_CHG_CD");
                sb.AppendLine("left join TB_9_M_COMM_D d on d.SYS_CD='HB' and d.MAIN_CD='EMP_CD' and d.SUB_CD=a.EMP_CD        ");
                sb.AppendLine("left join TB_9_M_COMM_D e on e.SYS_CD='SA' and e.MAIN_CD='CHG_STATUS' and e.SUB_CD=a.CHG_STATUS");
                sb.AppendLine("where not exists                                                                               ");
                sb.AppendLine("(Select * from TB_S_R_BONUS_D b                                                                ");
                sb.AppendLine("where a.EMP_ID=b.EMP_ID and a.BONUS_YEAR = b.BONUS_YEAR)                                       ");
                sb.AppendLine("and a.BONUS_YEAR = @BONUS_YEAR                                                                 ");
            }
            else if (data == "original")
            {
                sb.AppendLine(" Select distinct a.*,a.EMP_CHG_CD+'-'+c.SUB_DESC as EMP_CHG_CD_desc");
                sb.AppendLine("               ,a.EMP_CD+'-'+d.SUB_DESC as EMP_CD_desc                                         ");
                sb.AppendLine("               ,a.CHG_STATUS+'-'+e.SUB_DESC as CHG_STATUS_desc                                 ");
                sb.AppendLine(" from TB_S_M_BONUS_D a");
                sb.AppendLine("left join TB_9_M_COMM_D c on c.SYS_CD='HB' and c.MAIN_CD='EMP_CHG_CD' and c.SUB_CD=a.EMP_CHG_CD");
                sb.AppendLine("left join TB_9_M_COMM_D d on d.SYS_CD='HB' and d.MAIN_CD='EMP_CD' and d.SUB_CD=a.EMP_CD        ");
                sb.AppendLine("left join TB_9_M_COMM_D e on e.SYS_CD='SA' and e.MAIN_CD='CHG_STATUS' and e.SUB_CD=a.CHG_STATUS");
                sb.AppendLine(" where not exists");
                sb.AppendLine(" (Select * from TB_S_S_BONUS_D b");
                sb.AppendLine(" where a.EMP_ID=b.EMP_ID and a.BONUS_YEAR = b.BONUS_YEAR)");
                sb.AppendLine(" and a.BONUS_YEAR = @BONUS_YEAR");
            }

            ht.Add("BONUS_YEAR", BONUS_YEAR);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //取得刪除的對象資料(原始資料、前次核可)
    public DataTable getDelExcelData(string data, string BONUS_YEAR)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            if (data == "prev")
            {
                sb.AppendLine(" Select distinct a.*,a.EMP_CHG_CD+'-'+c.SUB_DESC as EMP_CHG_CD_desc");
                sb.AppendLine("               ,a.EMP_CD+'-'+d.SUB_DESC as EMP_CD_desc                                         ");
                sb.AppendLine("               ,a.CHG_STATUS+'-'+e.SUB_DESC as CHG_STATUS_desc                                 ");
                sb.AppendLine(" from TB_S_R_BONUS_D a");
                sb.AppendLine("left join TB_9_M_COMM_D c on c.SYS_CD='HB' and c.MAIN_CD='EMP_CHG_CD' and c.SUB_CD=a.EMP_CHG_CD");
                sb.AppendLine("left join TB_9_M_COMM_D d on d.SYS_CD='HB' and d.MAIN_CD='EMP_CD' and d.SUB_CD=a.EMP_CD        ");
                sb.AppendLine("left join TB_9_M_COMM_D e on e.SYS_CD='SA' and e.MAIN_CD='CHG_STATUS' and e.SUB_CD=a.CHG_STATUS");
                sb.AppendLine(" where not exists");
                sb.AppendLine(" (Select * from TB_S_M_BONUS_D b");
                sb.AppendLine(" where a.EMP_ID=b.EMP_ID and a.BONUS_YEAR = b.BONUS_YEAR)");
                sb.AppendLine(" and a.BONUS_YEAR = @BONUS_YEAR");
            }
            else if (data == "original")
            {
                sb.AppendLine(" Select distinct a.*,a.EMP_CHG_CD+'-'+c.SUB_DESC as EMP_CHG_CD_desc");
                sb.AppendLine("               ,a.EMP_CD+'-'+d.SUB_DESC as EMP_CD_desc                                         ");
                sb.AppendLine("               ,a.CHG_STATUS+'-'+e.SUB_DESC as CHG_STATUS_desc                                 ");
                sb.AppendLine(" from TB_S_S_BONUS_D a");
                sb.AppendLine("left join TB_9_M_COMM_D c on c.SYS_CD='HB' and c.MAIN_CD='EMP_CHG_CD' and c.SUB_CD=a.EMP_CHG_CD");
                sb.AppendLine("left join TB_9_M_COMM_D d on d.SYS_CD='HB' and d.MAIN_CD='EMP_CD' and d.SUB_CD=a.EMP_CD        ");
                sb.AppendLine("left join TB_9_M_COMM_D e on e.SYS_CD='SA' and e.MAIN_CD='CHG_STATUS' and e.SUB_CD=a.CHG_STATUS");
                sb.AppendLine(" where not exists");
                sb.AppendLine(" (Select * from TB_S_M_BONUS_D b");
                sb.AppendLine(" where a.EMP_ID=b.EMP_ID and a.BONUS_YEAR = b.BONUS_YEAR)");
                sb.AppendLine(" and a.BONUS_YEAR = @BONUS_YEAR");
            }

            ht.Add("BONUS_YEAR", BONUS_YEAR);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //紅利維護檔
    public DataTable TB_S_M_BONUS_H(string BONUS_YEAR)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select *");
            sb.Append(" from TB_S_M_BONUS_H");
            sb.Append(" where BONUS_YEAR = @BONUS_YEAR");
            ht.Add("BONUS_YEAR", BONUS_YEAR);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //更新紅利維護檔
    public void Update_TB_S_M_BONUS_H(string type, string BONUS_YEAR,DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            if (type == "approve")
            {
                sb.Append("Update TB_S_M_BONUS_H ");
                sb.Append(" Set REMARK = @REMARK,APPROVE_DT = GETDATE(),APPROVE_BY = @APPROVE_BY,");
                sb.Append(" APPROVE_STATUS = @APPROVE_STATUS,FREEZE_FLAG = @FREEZE_FLAG,");
                sb.Append(" BONUS_TOTAL_DECIMAL=(select count(*) from TB_S_M_BONUS_D where BONUS_YEAR=@BONUS_YEAR),");
                sb.Append(" BONUS_TOTAL_AMOUNT=(select sum(BONUS_AMT) from TB_S_M_BONUS_D where BONUS_YEAR=@BONUS_YEAR),");
                sb.Append(" UPDATED_BY=@UPDATED_BY,UPDATED_DT=@UPDATED_DT,FUNC_ID=@FUNC_ID");
                sb.Append(" where BONUS_YEAR = @BONUS_YEAR");
                ht.Clear();
                ht.Add("BONUS_YEAR", BONUS_YEAR);
                ht.Add("REMARK", "");
                ht.Add("APPROVE_BY", SessionHandle.Current.emp_id);
                ht.Add("APPROVE_STATUS", "Y");
                ht.Add("FREEZE_FLAG", "N");
                ht.Add("UPDATED_BY", SessionHandle.Current.emp_id);
                ht.Add("UPDATED_DT", now);
                ht.Add("FUNC_ID", "FB2SI030");
            }
            else if (type == "reject")
            {
                sb.Append("Update TB_S_M_BONUS_H ");
                sb.Append(" Set REMARK = @REMARK,RELEASE_DT=@RELEASE_DT,RELEASE_BY=@RELEASE_BY,");
                sb.Append(" APPROVE_DT = @APPROVE_DT,APPROVE_BY = @APPROVE_BY,");
                sb.Append(" APPROVE_STATUS = @APPROVE_STATUS,FREEZE_FLAG = @FREEZE_FLAG,");
                sb.Append(" UPDATED_BY=@UPDATED_BY,UPDATED_DT=@UPDATED_DT,FUNC_ID=@FUNC_ID");
                sb.Append(" where BONUS_YEAR = @BONUS_YEAR");
                ht.Clear();
                ht.Add("BONUS_YEAR", BONUS_YEAR);
                ht.Add("REMARK", REMARK);
                ht.Add("RELEASE_DT", DBNull.Value);
                ht.Add("RELEASE_BY", "");
                ht.Add("APPROVE_DT", DBNull.Value);
                ht.Add("APPROVE_BY", "");
                ht.Add("APPROVE_STATUS", "B");
                ht.Add("FREEZE_FLAG", "N");
                ht.Add("UPDATED_BY", SessionHandle.Current.emp_id);
                ht.Add("UPDATED_DT", now);
                ht.Add("FUNC_ID", "FB2SI030");
            }

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //紅利明細維護檔
    public DataTable TB_S_M_BONUS_D(string BONUS_YEAR)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select *");
            sb.Append(" from TB_S_M_BONUS_D");
            sb.Append(" where BONUS_YEAR = @BONUS_YEAR");
            ht.Add("BONUS_YEAR", BONUS_YEAR);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //異常註記-update 備註說明
    public void updateMarkData_H(DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_S_M_BONUS_H ");
            sb.Append(" set REMARK = @REMARK ");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT = @UPDATED_DT");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where BONUS_YEAR = @BONUS_YEAR ");
            //set值
            ht.Add("@REMARK", REMARK);

            //新修日期
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@UPDATED_DT", now);
            ht.Add("@FUNC_ID", FUNC_ID);

            //PK值
            ht.Add("@BONUS_YEAR", BONUS_YEAR);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }
    //異常註記-update 異常註記為空白 或V 
    public void updateMarkData_D(DateTime now, string approve_mark)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_S_M_BONUS_D ");
            sb.Append(" set APPROVE_MARK = @APPROVE_MARK ");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY");
            sb.Append(" ,UPDATED_DT = @UPDATED_DT");
            sb.Append(" ,FUNC_ID = @FUNC_ID ");
            sb.Append(" where BONUS_YEAR = @BONUS_YEAR ");
            sb.Append("  and EMP_ID = @EMP_ID");
            ht.Add("@APPROVE_MARK", approve_mark);

            //PK值
            ht.Add("@BONUS_YEAR", BONUS_YEAR);
            ht.Add("@EMP_ID", EMP_ID);


            //新修日期
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
    //核可前檢查異常註記
    public int getMarkData(string BONUS_YEAR)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) resultCount ");
            sb.Append(" from TB_S_M_BONUS_D a ");
            sb.Append(" where BONUS_YEAR = @BONUS_YEAR ");
            sb.Append("  and APPROVE_MARK = @APPROVE_MARK");
            //PK值
            ht.Add("@BONUS_YEAR", BONUS_YEAR);
            ht.Add("@APPROVE_MARK", "V");
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
    //更新紅利明細維護檔(核可)
    public void Update_TB_S_M_BONUS_D_Approve(string BONUS_YEAR,DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Update TB_S_M_BONUS_D ");
            sb.Append(" Set APPROVE_MARK = @APPROVE_MARK,APPROVE_FLAG = @APPROVE_FLAG,");
            sb.Append(" UPDATED_BY=@UPDATED_BY,UPDATED_DT=@UPDATED_DT,FUNC_ID=@FUNC_ID");
            sb.Append(" where BONUS_YEAR = @BONUS_YEAR");
            ht.Clear();
            ht.Add("BONUS_YEAR", BONUS_YEAR);
            ht.Add("APPROVE_MARK", "");
            ht.Add("APPROVE_FLAG", "Y");
            ht.Add("UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("UPDATED_DT", now);
            ht.Add("FUNC_ID", "FB2SI030");
            dbConn.ExecuteT(sb, ht, true);
            StringBuilder sb2 = new StringBuilder();
            Hashtable ht2 = new Hashtable();
            sb2.Append("Delete From TB_S_M_BONUS_D");
            sb2.Append(" where BONUS_YEAR = @BONUS_YEAR and CHG_STATUS='D'");
            ht2.Clear();
            ht2.Add("BONUS_YEAR", BONUS_YEAR);
            dbConn.ExecuteT(sb2, ht2, true);

        }
        catch (Exception)
        {
            throw;
        }
    }
    //更新紅利明細維護檔(駁回)
    public void Update_TB_S_M_BONUS_D_Reject(string BONUS_YEAR, string EMP_ID,DateTime now)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            
            
                sb.Append("Update TB_S_M_BONUS_D ");
                sb.Append(" Set APPROVE_MARK = @APPROVE_MARK,");
                sb.Append(" UPDATED_BY=@UPDATED_BY,UPDATED_DT=@UPDATED_DT,FUNC_ID=@FUNC_ID");
                sb.Append(" where BONUS_YEAR = @BONUS_YEAR and EMP_ID=@EMP_ID");
                ht.Clear();
                ht.Add("BONUS_YEAR", BONUS_YEAR);
                ht.Add("EMP_ID", EMP_ID);
                ht.Add("APPROVE_MARK", "V");
                ht.Add("UPDATED_BY", SessionHandle.Current.emp_id);
                ht.Add("UPDATED_DT", now);
                ht.Add("FUNC_ID", "FB2SI030");
                dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }
    //更新紅利明細主檔
    public void Update_TB_S_R_BONUS_D(string BONUS_YEAR)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Delete From TB_S_R_BONUS_D");
            sb.Append(" where BONUS_YEAR = @BONUS_YEAR");
            ht.Clear();
            ht.Add("BONUS_YEAR", BONUS_YEAR);
            dbConn.ExecuteT(sb, ht, true);

            StringBuilder sb2 = new StringBuilder();
            Hashtable ht2 = new Hashtable();
            sb2.Append("Insert into TB_S_R_BONUS_D ");
            sb2.Append(@" (BONUS_YEAR,BONUS_COUNT,BONUS_DT,EMP_ID,EMP_NAME
                        ,SEX_CD,EMP_CHG_CD,WS_CD,JPN_CD,COMPANY_CD
                        ,DEPT_NO,LEVEL_CD,PJOB_CD,JOIN_DT,LEAVE_DT
                        ,STAY_DT,BE_CONTRACT_DT,BE_EMP_DT,WORK_DAYS,EMP_CD
                        ,ID_DESC,LEVEL_PAY,ABILITY_PAY,PJOB_PAY,PROFESSION_PAY
                        ,FOOD_SUBSIDY,LEAVE_A_HOUR,LEAVE_B_HOUR,LEAVE_C_HOUR,LEAVE_Q_HOUR
                        ,LEAVE_OP_HOUR,THIRD_CNT_P,SECOND_CNT_P,FIRST_CNT_P,THIRD_CNT_M
                        ,SECOND_CNT_M,FIRST_CNT_M,ATTEND_DAYS,REWARD_DAYS,DISCIPLINE_DAYS
                        ,BONUS_WORK_DAYS,BONUS_AMT,BONUS_TAX,BONUS_AMT_R,PAY_TYPE
                        ,CHG_STATUS,PRIMEVAL_FLAG,APPROVE_FLAG,APPROVE_MARK
                        ,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID) 
                       ");
            sb2.Append(@" select
                         BONUS_YEAR,BONUS_COUNT,BONUS_DT,EMP_ID,EMP_NAME
                         ,SEX_CD,EMP_CHG_CD,WS_CD,JPN_CD,COMPANY_CD
                         ,DEPT_NO,LEVEL_CD,PJOB_CD,JOIN_DT,LEAVE_DT
                         ,STAY_DT,BE_CONTRACT_DT,BE_EMP_DT,WORK_DAYS,EMP_CD
                         ,ID_DESC,LEVEL_PAY,ABILITY_PAY,PJOB_PAY,PROFESSION_PAY
                         ,FOOD_SUBSIDY,LEAVE_A_HOUR,LEAVE_B_HOUR,LEAVE_C_HOUR,LEAVE_Q_HOUR
                         ,LEAVE_OP_HOUR,THIRD_CNT_P,SECOND_CNT_P,FIRST_CNT_P,THIRD_CNT_M
                         ,SECOND_CNT_M,FIRST_CNT_M,ATTEND_DAYS,REWARD_DAYS,DISCIPLINE_DAYS
                         ,BONUS_WORK_DAYS,BONUS_AMT,BONUS_TAX,BONUS_AMT_R,PAY_TYPE
                         ,CHG_STATUS,PRIMEVAL_FLAG,APPROVE_FLAG,APPROVE_MARK
                         ,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID  
                    ");
            sb2.Append(" from TB_S_M_BONUS_D");
            sb2.Append(" where BONUS_YEAR=@BONUS_YEAR");
            ht2.Clear();
            ht2.Add("BONUS_YEAR", BONUS_YEAR);
            dbConn.ExecuteT(sb2, ht2, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
}