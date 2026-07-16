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
/// wfb2hb0100 的摘要描述
/// </summary>
public class CFB2HB0100DAO : BaseDAO
{

    public string EMP_ID { get; set; }
    public string EMP_NAME { get; set; }
    public string NATION_CD { get; set; }
    public string JPN_CD { get; set; }
    public string BIRTH_DT { get; set; }
    public string BLOOD_TYPE { get; set; }
    public string SEX_CD { get; set; }
    public string ARMY_CD { get; set; }
    public string HEIGHT { get; set; }
    public string LICENSE_ID { get; set; }
    public string BIRTHPLACE { get; set; }
    public string PASSPORT_ID { get; set; }
    public string SALARY_ACCOUNT_NO { get; set; }
    public string REMARK { get; set; }
    public string JOIN_DT { get; set; }
    public string EXAM_EXPIRE_DT { get; set; }
    public string IS_MASTER { get; set; }
    public string COMPANY_CD { get; set; }
    public string PLAN_DESPATCH_DT { get; set; }
    public string IS_UPD_HEAD { get; set; }
    public string PLANT_CD { get; set; }
    public string BE_DESPATCH_DT { get; set; }
    public string DIRECT_HEAD_EMP_ID { get; set; }
    public string DEPT_NO { get; set; }
    public string DEPT_NAME { get; set; }
    public string KEEP_DESPATCH_DT { get; set; }
    public string HEALTH_YEAR { get; set; }
    public string WS_CD { get; set; }
    public string BE_CONTRACT_DT { get; set; }
    public string IS_DUTY_CHECK { get; set; }
    public string EMP_CD { get; set; }
    public string BE_EMP_DT { get; set; }
    public string MODEL_YEAR { get; set; }
    public string LEVEL_CD { get; set; }
    public string GRADE_CD { get; set; }
    public string HONOR_YEAR { get; set; }
    public string PJOB_CD { get; set; }
    public string UNION_PJOB_CD { get; set; }
    public string WORK_SHIFT_CD { get; set; }
    public string WORK_CD { get; set; }
    public string REGISTER_ZIP_CD { get; set; }
    public string REGISTER_COUNTY { get; set; }
    public string REGISTER_REGION { get; set; }
    public string REGISTER_ADDR { get; set; }
    public string REGISTER_TEL { get; set; }
    public string CONTACT_ZIP_CD { get; set; }
    public string CONTACT_COUNTY { get; set; }
    public string CONTACT_REGION { get; set; }
    public string CONTACT_ADDR { get; set; }
    public string CONTACT_TEL { get; set; }
    public string PERSONAL_EMAIL { get; set; }
    public string MOBILE_TEL_1 { get; set; }
    public string MOBILE_TEL_2 { get; set; }
    public string COMPANY_EMAIL { get; set; }
    public string OVERTIME_CTL_DT { get; set; }

    //public string COMPANY_EXT { get; set; }
    public string URGENT_CONTACT_NAME { get; set; }
    public string URGENT_CONTACT_TEL { get; set; }
    public string URGENT_CONTACT_RELATION { get; set; }
    public string RELATIVES { get; set; }
    public string INCOME_CD { get; set; }
    public string WEIGHT { get; set; }
    public string SALARY_EMAIL_CD { get; set; }
    public string SALARY_EMAIL { get; set; }
    public string CHG_SEQ { get; set; }
    public string COMPANY_DESC { get; set; }
    public string PLANT_DESC { get; set; }
    public string WS_DESC { get; set; }
    public string EMP_DESC { get; set; }
    public string PJOB_DESC { get; set; }
    public string WORK_SHIFT_DESC { get; set; }
    public string WORK_DESC { get; set; }
    public DataTable EMP_FAMILY { get; set; }
    public DataTable EDU_DATA { get; set; }
    public DataTable EXP_DATA { get; set; }
    public string SALARY_ACCOUNT_BRANCH { get; set; }

    //家庭成員檔
    public string FAMILY_LICENSE_ID { get; set; }
    public string FAMILY_PASSPORT_ID { get; set; }
    public string FAMILY_NATION_CD { get; set; }
    public string FAMILY_SEX_CD { get; set; }
    public string FAMILY_NAME { get; set; }
    public string FAMILY_RELATION { get; set; }
    public string FAMILY_BIRTH_DT { get; set; }
    public string FAMILY_WORK_DESC { get; set; }
    public string IS_ALLOWANCE { get; set; }
    public string BENEFICIARY { get; set; }
    public string VENDOR_ID { get; set; }
    public string IS_VALID { get; set; }

    //員工經歷檔
    public string EXP_COMPANY_NAME { get; set; }
    public string EXP_TITLE_DESC { get; set; }
    public string START_YEAR { get; set; }
    public string END_YEAR { get; set; }
    public string APPROVE_WORK_YEARS { get; set; }

    //員工學歷檔
    public string EDUCATION_CD { get; set; }
    public string SCHOOL_NATION_CD { get; set; }
    public string SCHOOL_NAME { get; set; }
    public string DEPARTMENT_NAME { get; set; }
    public string GRADUATION_YEAR { get; set; }
    public string IS_SALARY_SCHOOL { get; set; }
    public string IS_VIRTUAL_SCHOOL { get; set; }


    public CFB2HB0100DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //

    }

    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string emp_id, string emp_name,
                string emp_cd, string license_id, string dept_no, string emp_chg_cd, string ws_cd, string company_cd, string plant_cd, string level_cd, string pjob_cd,
                string work_cd, string jpn_cd, string work_shift_cd, string join_dt_s,
                string join_dt_e, string leave_dt_s, string leave_dt_e, string be_contract_dt_s, string be_contract_dt_e,
                string be_emp_dt_s, string be_emp_dt_e, bool is_duty_check, bool overtime_ctl_cd, bool model, bool is_upd_head, bool union_pjob_cd)
    {
        try
        {
            if (sortExpression.Contains("UNION_PJOB_CD"))
            {
                sortExpression = sortExpression.Replace("UNION_PJOB_CD", "a.UNION_PJOB_CD");
            }
            if (sortExpression.Contains("COMPANY_CD"))
            {
                sortExpression = sortExpression.Replace("COMPANY_CD", "a.COMPANY_CD");
            }
            if (sortExpression.Contains("DEPT_NAME"))
            {
                sortExpression = sortExpression.Replace("DEPT_NAME", "a.DEPT_NO");
            }

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" C.COMPANY_SNAME as COMPANY_CD, ");
            sb.Append(" (select SUB_CD+'-' + SUB_DESC from TB_9_M_COMM_D where SYS_CD='HB' and MAIN_CD='PLANT_CD' and SUB_CD=PLANT_CD) as PLANT_CD, ");
            sb.Append(" a.DEPT_NO + '-' + isnull(dept.DEPT_NAME,'') DEPT_NAME,EMP_ID,EMP_NAME,LICENSE_ID,PASSPORT_ID,");
            sb.Append(" EMP_DESC, ");
            sb.Append(" (select SUB_CD+'-' + SUB_DESC from TB_9_M_COMM_D where SYS_CD='HB' and MAIN_CD='EMP_CD' and SUB_CD=EMP_CD) as EMP_CD, ");
            sb.Append(" (select SUB_CD+'-' + SUB_DESC from TB_9_M_COMM_D where SYS_CD='HB' and MAIN_CD='WS_CD' and SUB_CD=WS_CD) as WS_CD, ");
            sb.Append(" LEVEL_CD,PJOB_CD+'-'+PJOB_DESC as PJOB_CD,D1.SUB_DESC JPN_DESC,JPN_CD,CONVERT(char(10), JOIN_DT, 111) JOIN_DT,CONVERT(char(10), LEAVE_DT, 111) LEAVE_DT,");
            sb.Append(" LEAVE_REASON_DESC,LEAVE_REASON,CONVERT(char(10), BE_CONTRACT_DT, 111) BE_CONTRACT_DT,");
            sb.Append(" CONVERT(char(10),BE_EMP_DT, 111) BE_EMP_DT,WORK_SHIFT_CD, ");
            sb.Append(" DIRECT_HEAD_EMP_ID,");
            sb.Append(" (select EMP_NAME from TB_H_M_EMP  with (nolock)  where a.DIRECT_HEAD_EMP_ID = TB_H_M_EMP.EMP_ID) DIRECT_HEAD_EMP_NAME,UNION_PJOB_DESC,a.UNION_PJOB_CD,");
            sb.Append(" WORK_CD+'-'+WORK_DESC WORK_DESC,WORK_CD,SERVICE_YEARS,WORK_YEARS,IS_DUTY_CHECK,D2.SUB_DESC OVERTIME_CTL_DESC, OVERTIME_CTL_CD,MODEL_YEAR,IS_UPD_HEAD");
            sb.Append(" , a.EMP_CHG_CD + '-' + isnull(d.SUB_DESC,'') EMP_CHG_CD_DESC, a.EMP_CHG_CD   ");
            sb.Append(" from VW_H_EMP_DATA a  with (nolock)  left join TB_9_M_COMM_D D1  with (nolock)  on a.JPN_CD = D1.SUB_CD and D1.MAIN_CD = 'JPN_CD' and D1.SYS_CD = 'HB'");
            sb.Append(" LEFT JOIN VW_H_DEPT_DATA dept  with (nolock)  on A.DEPT_NO=dept.DEPT_NO  ");
            sb.Append(" left join TB_9_M_COMM_D D2   with (nolock)  on a.OVERTIME_CTL_CD = D2.SUB_CD and D2.MAIN_CD = 'OVERTIME_CTL_CD' and D2.SYS_CD = 'HB' ");
            sb.Append(" left join TB_D_M_UNION_PJOB D3  with (nolock)  on a.UNION_PJOB_CD = D3.UNION_PJOB_CD");
            sb.Append(" left join TB_H_M_COMPANY C   with (nolock) on a.COMPANY_CD = C.COMPANY_CD ");
            sb.Append(" left join TB_9_M_COMM_D d   with (nolock) on a.EMP_CHG_CD = d.SUB_CD and d.MAIN_CD = 'EMP_CHG_CD' and d.IS_VALID='Y' and d.SYS_CD='HB'    ");
            sb.Append(" where 1=1 ");
            if (emp_id != "")
            {
                sb.Append(" and a.EMP_ID like @EMP_ID ");
                ht.Add("@EMP_ID", emp_id + "%");
            }
            if (emp_name != "")
            {
                sb.Append(" and a.emp_name like @emp_name ");
                ht.Add("@emp_name", "%" + emp_name + "%");
            }
            if (emp_cd != "-1" && emp_cd != null)
            {
                sb.Append(" and a.emp_cd = @emp_cd ");
                ht.Add("@emp_cd", emp_cd);
            }
            if (license_id != "")
            {
                sb.Append(" and (a.LICENSE_ID like @LICENSE_ID or a.PASSPORT_ID like @LICENSE_ID )");
                ht.Add("@LICENSE_ID", license_id + "%");
            }
            if (dept_no != "")
            {
                sb.Append(" and a.dept_no like @dept_no ");
                ht.Add("@dept_no", dept_no + "%");
            }
            if (emp_chg_cd != "-1" && emp_chg_cd != null)
            {
                sb.Append(" and a.emp_chg_cd = @emp_chg_cd ");
                ht.Add("@emp_chg_cd", emp_chg_cd);
            }
            if (ws_cd != "-1" && ws_cd != null)
            {
                sb.Append(" and a.ws_cd = @ws_cd ");
                ht.Add("@ws_cd", ws_cd);
            }
            if (company_cd != "-1" && company_cd != null)
            {
                sb.Append(" and a.company_cd = @company_cd ");
                ht.Add("@company_cd", company_cd);
            }
            if (plant_cd != "-1" && plant_cd != null)
            {
                sb.Append(" and a.plant_cd = @plant_cd ");
                ht.Add("@plant_cd", plant_cd);
            }
            if (level_cd != "-1" && level_cd != null)
            {
                sb.Append(" and a.level_cd = @level_cd ");
                ht.Add("@level_cd", level_cd);
            }
            if (pjob_cd != "")
            {
                sb.Append(" and a.pjob_cd like @pjob_cd ");
                ht.Add("@pjob_cd", pjob_cd + "%");
            }
            if (work_cd != "-1" && work_cd != null)
            {
                sb.Append(" and a.work_cd = @work_cd ");
                ht.Add("@work_cd", work_cd);
            }
            if (jpn_cd != "-1" && jpn_cd != null)
            {
                sb.Append(" and a.jpn_cd = @jpn_cd ");
                ht.Add("@jpn_cd", jpn_cd);
            }


            if (work_shift_cd != "")
            {
                sb.Append(" and a.work_shift_cd like @work_shift_cd ");
                ht.Add("@work_shift_cd", work_shift_cd + "%");
            }
            if (join_dt_s != "")
            {
                sb.Append(" and a.join_dt >= CONVERT(datetime,@join_dt_s) ");
                ht.Add("@join_dt_s", join_dt_s);
            }
            if (join_dt_e != "")
            {
                sb.Append(" and a.join_dt <= CONVERT(datetime,@join_dt_e) ");
                ht.Add("@join_dt_e", join_dt_e);
            }

            if (leave_dt_s != "")
            {

                sb.Append(" and a.leave_dt is not null and a.leave_dt >= CONVERT(datetime,@leave_dt_s) ");
                //sb.Append(" and exists (select emp_id from TB_H_M_EMP_HR_CHANGE_H b,TB_H_M_HR_CHANGE_CODE c where b.HR_CHG_CD = c.HR_CHG_CD and b.emp_id = a.emp_id ");
                //sb.Append(" and c.EMP_CHG_STATUS in ('2','M','O','1') and b.HR_CHG_PROC_STATUS = 'Y' ) ");
                ht.Add("@leave_dt_s", leave_dt_s);

            }
            if (leave_dt_e != "")
            {
                sb.Append(" and a.leave_dt is not null and a.leave_dt <= CONVERT(datetime,@leave_dt_e) ");
                //sb.Append(" and exists (select emp_id from TB_H_M_EMP_HR_CHANGE_H b,TB_H_M_HR_CHANGE_CODE c where b.HR_CHG_CD = c.HR_CHG_CD and b.emp_id = a.emp_id ");
                //sb.Append(" and c.EMP_CHG_STATUS in ('2','M','O','1') and b.HR_CHG_PROC_STATUS = 'Y') ");
                ht.Add("@leave_dt_e", leave_dt_e);
            }

            if (be_contract_dt_s != "")
            {
                sb.Append(" and be_contract_dt >= CONVERT(datetime,@be_contract_dt_s) ");
                ht.Add("@be_contract_dt_s", be_contract_dt_s);
            }
            if (be_contract_dt_e != "")
            {
                sb.Append(" and be_contract_dt <= CONVERT(datetime,@be_contract_dt_e) ");
                ht.Add("@be_contract_dt_e", be_contract_dt_e);
            }

            if (be_emp_dt_s != "")
            {
                sb.Append(" and be_emp_dt >= CONVERT(datetime,@be_emp_dt_s) ");
                ht.Add("@be_emp_dt_s", be_emp_dt_s);
            }
            if (be_emp_dt_e != "")
            {
                sb.Append(" and be_emp_dt <= CONVERT(datetime,@be_emp_dt_e) ");
                ht.Add("@be_emp_dt_e", be_emp_dt_e);
            }

            if (is_duty_check || overtime_ctl_cd || model || is_upd_head || union_pjob_cd)
            {
                string checkString = "";
                if (is_duty_check)
                    checkString += "or IS_DUTY_CHECK = 'N' ";

                if (overtime_ctl_cd)
                    checkString += "or OVERTIME_CTL_CD <> '1' ";

                if (model)
                    checkString += "or isnull(MODEL_YEAR,'') <> '' ";

                if (is_upd_head)
                    checkString += "or IS_UPD_HEAD = 'N' ";

                if (union_pjob_cd)
                    checkString += "or isnull(a.UNION_PJOB_CD,'') <> '' ";
                checkString = checkString.TrimStart('o');
                checkString = checkString.TrimStart('r');

                sb.Append(" and ( " + checkString + ")");
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

    public int getCount(int startRowIndex, int maximumRows, string emp_id, string emp_name,
                string emp_cd, string license_id, string dept_no, string emp_chg_cd, string ws_cd, string company_cd, string plant_cd, string level_cd, string pjob_cd,
                string work_cd, string jpn_cd, string work_shift_cd, string join_dt_s,
                string join_dt_e, string leave_dt_s, string leave_dt_e, string be_contract_dt_s, string be_contract_dt_e,
                string be_emp_dt_s, string be_emp_dt_e, bool is_duty_check, bool overtime_ctl_cd, bool model, bool is_upd_head, bool union_pjob_cd)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select COUNT(*) total_record ");
            sb.Append(" from VW_H_EMP_DATA a  with (nolock)  left join TB_9_M_COMM_D D1 on a.JPN_CD = D1.SUB_CD and D1.MAIN_CD = 'JPN_CD' and D1.SYS_CD = 'HB'");
            sb.Append(" left join TB_9_M_COMM_D D2 on a.OVERTIME_CTL_CD = D2.SUB_CD and D2.MAIN_CD = 'OVERTIME_CTL_CD' and D2.SYS_CD = 'HB' ");
            sb.Append(" left join TB_D_M_UNION_PJOB D3 on a.UNION_PJOB_CD = D3.UNION_PJOB_CD");
            sb.Append(" left join TB_H_M_COMPANY C on a.COMPANY_CD = C.COMPANY_CD ");
            sb.Append(" where EMP_ID is not null");
            if (emp_id != "")
            {
                sb.Append(" and a.EMP_ID like @EMP_ID ");
                ht.Add("@EMP_ID", emp_id + "%");
            }
            if (emp_name != "")
            {
                sb.Append(" and a.emp_name like @emp_name ");
                ht.Add("@emp_name", "%" + emp_name + "%");
            }
            if (emp_cd != "-1" && emp_cd != null)
            {
                sb.Append(" and a.emp_cd = @emp_cd ");
                ht.Add("@emp_cd", emp_cd);
            }
            if (license_id != "")
            {
                sb.Append(" and (a.LICENSE_ID like @LICENSE_ID or a.PASSPORT_ID like @LICENSE_ID )");
                ht.Add("@LICENSE_ID", license_id + "%");
            }
            if (dept_no != "")
            {
                sb.Append(" and a.dept_no like @dept_no ");
                ht.Add("@dept_no", dept_no + "%");
            }
            if (emp_chg_cd != "-1" && emp_chg_cd != null)
            {
                sb.Append(" and a.emp_chg_cd = @emp_chg_cd ");
                ht.Add("@emp_chg_cd", emp_chg_cd);
            }
            if (ws_cd != "-1" && ws_cd != null)
            {
                sb.Append(" and a.ws_cd = @ws_cd ");
                ht.Add("@ws_cd", ws_cd);
            }
            if (company_cd != "-1" && company_cd != null)
            {
                sb.Append(" and a.company_cd = @company_cd ");
                ht.Add("@company_cd", company_cd);
            }
            if (plant_cd != "-1" && plant_cd != null)
            {
                sb.Append(" and a.plant_cd = @plant_cd ");
                ht.Add("@plant_cd", plant_cd);
            }
            if (level_cd != "-1" && level_cd != null)
            {
                sb.Append(" and a.level_cd = @level_cd ");
                ht.Add("@level_cd", level_cd);
            }
            if (pjob_cd != "")
            {
                sb.Append(" and a.pjob_cd like @pjob_cd ");
                ht.Add("@pjob_cd", pjob_cd + "%");
            }
            if (work_cd != "-1" && work_cd != null)
            {
                sb.Append(" and a.work_cd = @work_cd ");
                ht.Add("@work_cd", work_cd);
            }
            if (jpn_cd != "-1" && jpn_cd != null)
            {
                sb.Append(" and a.jpn_cd = @jpn_cd ");
                ht.Add("@jpn_cd", jpn_cd);
            }

            if (work_shift_cd != "")
            {
                sb.Append(" and a.work_shift_cd like @work_shift_cd ");
                ht.Add("@work_shift_cd", work_shift_cd + "%");
            }

            if (join_dt_s != "")
            {
                sb.Append(" and a.join_dt >= CONVERT(datetime,@join_dt_s) ");
                ht.Add("@join_dt_s", join_dt_s);
            }
            if (join_dt_e != "")
            {
                sb.Append(" and a.join_dt <= CONVERT(datetime,@join_dt_e) ");
                ht.Add("@join_dt_e", join_dt_e);
            }

            if (leave_dt_s != "")
            {

                sb.Append(" and a.leave_dt is not null and a.leave_dt >= CONVERT(datetime,@leave_dt_s) ");
                //sb.Append(" and exists (select emp_id from TB_H_M_EMP_HR_CHANGE_H b,TB_H_M_HR_CHANGE_CODE c where b.HR_CHG_CD = c.HR_CHG_CD and b.emp_id = a.emp_id ");
                //sb.Append(" and c.EMP_CHG_STATUS in ('2','M','O','1') and b.HR_CHG_PROC_STATUS = 'Y' ) ");
                ht.Add("@leave_dt_s", leave_dt_s);

            }
            if (leave_dt_e != "")
            {
                sb.Append(" and a.leave_dt is not null and a.leave_dt <= CONVERT(datetime,@leave_dt_e) ");
                //sb.Append(" and exists (select emp_id from TB_H_M_EMP_HR_CHANGE_H b,TB_H_M_HR_CHANGE_CODE c where b.HR_CHG_CD = c.HR_CHG_CD and b.emp_id = a.emp_id ");
                //sb.Append(" and c.EMP_CHG_STATUS in ('2','M','O','1') and b.HR_CHG_PROC_STATUS = 'Y') ");
                ht.Add("@leave_dt_e", leave_dt_e);
            }

            if (be_contract_dt_s != "")
            {
                sb.Append(" and be_contract_dt >= CONVERT(datetime,@be_contract_dt_s) ");
                ht.Add("@be_contract_dt_s", be_contract_dt_s);
            }
            if (be_contract_dt_e != "")
            {
                sb.Append(" and be_contract_dt <= CONVERT(datetime,@be_contract_dt_e) ");
                ht.Add("@be_contract_dt_e", be_contract_dt_e);
            }

            if (be_emp_dt_s != "")
            {
                sb.Append(" and be_emp_dt >= CONVERT(datetime,@be_emp_dt_s) ");
                ht.Add("@be_emp_dt_s", be_emp_dt_s);
            }
            if (be_emp_dt_e != "")
            {
                sb.Append(" and be_emp_dt <= CONVERT(datetime,@be_emp_dt_e) ");
                ht.Add("@be_emp_dt_e", be_emp_dt_e);
            }

            if (is_duty_check || overtime_ctl_cd || model || is_upd_head || union_pjob_cd)
            {
                string checkString = "";
                if (is_duty_check)
                    checkString += "or IS_DUTY_CHECK = 'N' ";

                if (overtime_ctl_cd)
                    checkString += "or OVERTIME_CTL_CD <> '1' ";

                if (model)
                    checkString += "or isnull(MODEL_YEAR,'') <> '' ";

                if (is_upd_head)
                    checkString += "or IS_UPD_HEAD = 'N' ";

                if (union_pjob_cd)
                    checkString += "or isnull(a.UNION_PJOB_CD,'') <> '' ";
                checkString = checkString.TrimStart('o');
                checkString = checkString.TrimStart('r');

                sb.Append(" and ( " + checkString + ")");
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

    public DataTable getEmpData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select b.*,CONVERT(char(10),b.BIRTH_DT, 111) BIRTH_DT_STR,CONVERT(char(10),b.JOIN_DT, 111) JOIN_DT_STR,");
            sb.Append(" a.DEPT_NAME,c.EMP_NAME DIRECT_HEAD_EMP_NAME ");
            sb.Append(" from VW_H_DEPT_DATA a,TB_H_M_EMP b,TB_H_M_EMP c");
            sb.Append(" where a.DEPT_NO = b.DEPT_NO and b.DIRECT_HEAD_EMP_ID = c.EMP_ID and b.EMP_ID = @EMP_ID ");
            ht.Add("@EMP_ID", EMP_ID);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public void updateEmpData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //BeginTransaction();

            //基本資料
            sb.Append(" Update TB_H_M_EMP Set ");
            sb.Append(" EMP_NAME = @EMP_NAME,IS_MASTER = @IS_MASTER,IS_UPD_HEAD = @IS_UPD_HEAD,DIRECT_HEAD_EMP_ID = @DIRECT_HEAD_EMP_ID,OVERTIME_CTL_CD = @OVERTIME_CTL_CD, ");
            sb.Append(" HEALTH_YEAR = @HEALTH_YEAR,UNION_PJOB_CD = @UNION_PJOB_CD,MODEL_YEAR = @MODEL_YEAR,NATION_CD = @NATION_CD,JPN_CD = @JPN_CD,");
            sb.Append(" LICENSE_ID = @LICENSE_ID,PASSPORT_ID = @PASSPORT_ID,SEX_CD = @SEX_CD,BIRTH_DT = @BIRTH_DT,BLOOD_TYPE = @BLOOD_TYPE,");
            sb.Append(" HEIGHT = @HEIGHT,WEIGHT = @WEIGHT,BIRTHPLACE = @BIRTHPLACE,ARMY_CD = @ARMY_CD, ");
            sb.Append(" SALARY_ACCOUNT_BANK = @ACCOUNT_BANK, SALARY_ACCOUNT_BRANCH = @SALARY_ACCOUNT_BRANCH, SALARY_ACCOUNT_NO = @SALARY_ACCOUNT_NO,");
            sb.Append(" REMARK = @REMARK,RELATIVES = @RELATIVES,INCOME_CD = @INCOME_CD,URGENT_CONTACT_NAME = @URGENT_CONTACT_NAME,");
            sb.Append(" URGENT_CONTACT_RELATION = @URGENT_CONTACT_RELATION,URGENT_CONTACT_TEL = @URGENT_CONTACT_TEL,REGISTER_ZIP_CD = @REGISTER_ZIP_CD,REGISTER_COUNTY = @REGISTER_COUNTY,");
            sb.Append(" REGISTER_REGION = @REGISTER_REGION,REGISTER_ADDR = @REGISTER_ADDR,REGISTER_TEL = @REGISTER_TEL,CONTACT_ZIP_CD = @CONTACT_ZIP_CD,CONTACT_COUNTY = @CONTACT_COUNTY,");
            sb.Append(" CONTACT_REGION = @CONTACT_REGION,CONTACT_ADDR = @CONTACT_ADDR,CONTACT_TEL = @CONTACT_TEL,MOBILE_TEL_1 = @MOBILE_TEL_1,MOBILE_TEL_2 = @MOBILE_TEL_2,");
            sb.Append(" PERSONAL_EMAIL = @PERSONAL_EMAIL,COMPANY_EMAIL = @COMPANY_EMAIL,SALARY_EMAIL_CD = @SALARY_EMAIL_CD,SALARY_EMAIL=@SALARY_EMAIL, ");
            sb.Append(" IS_DUTY_CHECK = @IS_DUTY_CHECK, ");
            //sb.Append(" COMPANY_EXT = @COMPANY_EXT, ");
            sb.Append(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID");
            sb.Append(" where EMP_ID = @EMP_ID ");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@EMP_NAME", EMP_NAME);
            ht.Add("@IS_MASTER", IS_MASTER);
            ht.Add("@IS_UPD_HEAD", IS_UPD_HEAD);
            ht.Add("@DIRECT_HEAD_EMP_ID", DIRECT_HEAD_EMP_ID);
            ht.Add("@OVERTIME_CTL_CD", OVERTIME_CTL_CD);
            ht.Add("@IS_DUTY_CHECK", IS_DUTY_CHECK);
            ht.Add("@HEALTH_YEAR", HEALTH_YEAR);
            if (UNION_PJOB_CD == "-1")
                ht.Add("@UNION_PJOB_CD", "");
            else
                ht.Add("@UNION_PJOB_CD", UNION_PJOB_CD);
            ht.Add("@MODEL_YEAR", MODEL_YEAR);
            ht.Add("@NATION_CD", NATION_CD);
            if (JPN_CD == "-1")
                ht.Add("@JPN_CD", "");
            else
                ht.Add("@JPN_CD", JPN_CD);
            ht.Add("@LICENSE_ID", LICENSE_ID);
            ht.Add("@PASSPORT_ID", PASSPORT_ID);
            ht.Add("@SEX_CD", SEX_CD);
            ht.Add("@BIRTH_DT", BIRTH_DT);
            ht.Add("@BLOOD_TYPE", BLOOD_TYPE);
            ht.Add("@HEIGHT", HEIGHT);
            ht.Add("@WEIGHT", WEIGHT);
            ht.Add("@BIRTHPLACE", BIRTHPLACE);
            if (ARMY_CD == "-1")
                ht.Add("@ARMY_CD", "");
            else
                ht.Add("@ARMY_CD", ARMY_CD);
            ht.Add("@ACCOUNT_BANK", ACCOUNT_BANK);
            ht.Add("@SALARY_ACCOUNT_BRANCH", SALARY_ACCOUNT_BRANCH);
            ht.Add("@SALARY_ACCOUNT_NO", SALARY_ACCOUNT_NO);
            ht.Add("@REMARK", REMARK);
            ht.Add("@RELATIVES", RELATIVES == "" ? "0" : RELATIVES);
            ht.Add("@INCOME_CD", INCOME_CD);
            ht.Add("@URGENT_CONTACT_NAME", URGENT_CONTACT_NAME);
            ht.Add("@URGENT_CONTACT_RELATION", URGENT_CONTACT_RELATION);
            ht.Add("@URGENT_CONTACT_TEL", URGENT_CONTACT_TEL);
            ht.Add("@REGISTER_ZIP_CD", REGISTER_ZIP_CD);
            ht.Add("@REGISTER_COUNTY", REGISTER_COUNTY);
            ht.Add("@REGISTER_REGION", REGISTER_REGION);
            ht.Add("@REGISTER_ADDR", REGISTER_ADDR);
            ht.Add("@REGISTER_TEL", REGISTER_TEL);
            ht.Add("@CONTACT_ZIP_CD", CONTACT_ZIP_CD);
            ht.Add("@CONTACT_COUNTY", CONTACT_COUNTY);
            ht.Add("@CONTACT_REGION", CONTACT_REGION);
            ht.Add("@CONTACT_ADDR", CONTACT_ADDR);
            ht.Add("@CONTACT_TEL", CONTACT_TEL);
            ht.Add("@MOBILE_TEL_1", MOBILE_TEL_1);
            ht.Add("@MOBILE_TEL_2", MOBILE_TEL_2);
            ht.Add("@PERSONAL_EMAIL", PERSONAL_EMAIL);
            ht.Add("@COMPANY_EMAIL", COMPANY_EMAIL);
            ht.Add("@SALARY_EMAIL", SALARY_EMAIL);
            //ht.Add("@COMPANY_EXT", COMPANY_EXT);
            if (SALARY_EMAIL_CD == null)
                ht.Add("@SALARY_EMAIL_CD", "");
            else
                ht.Add("@SALARY_EMAIL_CD", SALARY_EMAIL_CD);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            //RollBack();
            throw;
        }
    }

    public DataTable getEmpFamily(string sortExpression)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" a.EMP_ID,FAMILY_RELATION,FAMILY_RELATION + '-' + b.SUB_DESC FAMILY_RELATION_DESC,FAMILY_NAME,FAMILY_NAME FAMILY_ORI_NAME,REPLACE(CONVERT(char(10), a.FAMILY_BIRTH_DT, 120),'-','/') FAMILY_BIRTH_DT, ");
            sb.Append(" FAMILY_LICENSE_ID,FAMILY_WORK_DESC,FAMILY_PASSPORT_ID,FAMILY_NATION_CD + '-' + c.SUB_DESC FAMILY_NATION_DESC, FAMILY_NATION_CD,FAMILY_SEX_CD,IS_ALLOWANCE,BENEFICIARY,VENDOR_ID,a.IS_VALID,");
            sb.Append(" case FAMILY_SEX_CD when '1' then '1-男' when '2' then '2-女' end as FAMILY_SEX_DESC,FAMILY_LICENSE_ID FAMILY_ORI_LICENSE_ID,REPLACE(CONVERT(char(10), a.FAMILY_BIRTH_DT, 120),'-','/') FAMILY_ORI_BIRTH_DT");
            sb.Append(" from TB_H_M_EMP_FAMILY a,TB_9_M_COMM_D b ,TB_9_M_COMM_D c");
            sb.Append(" where a.FAMILY_RELATION = b.SUB_CD and b.MAIN_CD = 'FAMILY_RELATION' ");
            sb.Append(" and a.FAMILY_NATION_CD = c.SUB_CD and c.MAIN_CD = 'NATION_CD' ");
            if (EMP_ID != "")
            {
                sb.Append(" and a.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", EMP_ID);
            }

            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }

    }

    internal DataTable getLevelCD(string join_dt = "")
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select distinct LEVEL_CD from TB_H_M_LEVEL ");
            if (join_dt != "")
            {
                sb.Append(" where @join_dt >=START_DT and @join_dt <= END_DT ");
                ht.Add("@join_dt", join_dt);
            }
            sb.Append(" order by LEVEL_CD");
            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getCOMPANY_CD()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select COMPANY_CD,COMPANY_SNAME ");
            sb.Append(" from TB_H_M_COMPANY ");
            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void SP_H_HEAD_DEPT()
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(utilities.connstr))
            using (SqlCommand comm = new SqlCommand())
            {
                conn.Open();
                comm.Connection = conn;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = "SP_H_HEAD_DEPT";
                comm.Parameters.AddWithValue("@pDate", DateTime.Now.ToString("yyyy/MM/dd"));
                comm.Parameters.AddWithValue("@pUserID", SessionHandle.Current.emp_id);
                comm.Parameters.AddWithValue("@pFuncID", "FB2HB010");

                comm.CommandTimeout = 1000;
                comm.ExecuteNonQuery();

                conn.Close();
            }

        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void SP_H_UPD_EMP_HEAD()
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(utilities.connstr))
            using (SqlCommand comm = new SqlCommand())
            {
                conn.Open();
                comm.Connection = conn;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = "SP_H_UPD_EMP_HEAD";
                comm.Parameters.AddWithValue("@pDate", DateTime.Now.ToString("yyyy/MM/dd"));
                comm.Parameters.AddWithValue("@pUserID", SessionHandle.Current.emp_id);
                comm.Parameters.AddWithValue("@pFuncID", "FB2HB010");

                comm.CommandTimeout = 1000;
                comm.ExecuteNonQuery();

                conn.Close();
            }


        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void SP_H_DEPT_DATA()
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(utilities.connstr))
            using (SqlCommand comm = new SqlCommand())
            {
                conn.Open();
                comm.Connection = conn;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = "SP_H_DEPT_DATA";
                comm.Parameters.AddWithValue("@pDate", DateTime.Now.ToString("yyyy/MM/dd"));
                comm.Parameters.AddWithValue("@pProcID", "1");
                comm.Parameters.AddWithValue("@pUserID", SessionHandle.Current.emp_id);
                comm.Parameters.AddWithValue("@pFuncID", "FB2HB010");

                comm.CommandTimeout = 1000;
                comm.ExecuteNonQuery();

                conn.Close();
            }

        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void SP_H_HEAD_EMP()
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(utilities.connstr))
            using (SqlCommand comm = new SqlCommand())
            {
                conn.Open();
                comm.Connection = conn;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = "SP_H_HEAD_EMP";
                comm.Parameters.AddWithValue("@pDate", DateTime.Now.ToString("yyyy/MM/dd"));
                comm.Parameters.AddWithValue("@pUserID", SessionHandle.Current.emp_id);
                comm.Parameters.AddWithValue("@pFuncID", "FB2HB010");

                comm.CommandTimeout = 1000;
                comm.ExecuteNonQuery();

                conn.Close();
            }


        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void SP_H_EMP_DATA()
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(utilities.connstr))
            using (SqlCommand comm = new SqlCommand())
            {
                conn.Open();
                comm.Connection = conn;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = "SP_H_EMP_DATA";
                comm.Parameters.AddWithValue("@pDate", DateTime.Now.ToString("yyyy/MM/dd"));
                comm.Parameters.AddWithValue("@pUserID", SessionHandle.Current.emp_id);
                comm.Parameters.AddWithValue("@pFuncID", "FB2HB010");

                comm.CommandTimeout = 1000;
                comm.ExecuteNonQuery();

                conn.Close();
            }

        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getEmpData(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select EMP_ID from TB_H_M_EMP with(nolock) Where EMP_ID = @EMP_ID ");
            ht.Add("@EMP_ID", emp_id);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }


    internal DataTable getEmpData(string emp_id,string pjob_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select EMP_ID from TB_H_M_EMP with(nolock) Where EMP_ID = @EMP_ID and PJOB_CD = @PJOB_CD");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@PJOB_CD", pjob_cd);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getOVERTIME_CTL(string cell3)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select SUB_CD from TB_9_M_COMM_D Where SYS_CD = 'HB' and SUB_CD = @SUB_CD and MAIN_CD = 'OVERTIME_CTL_CD' ");
            ht.Add("@SUB_CD", cell3);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public string OVERTIME_CTL_CD { get; set; }

    public string CREATED_BY { get; set; }

    public string UPDATED_BY { get; set; }

    public string FUNC_ID { get; set; }

    //修改外籍人員身份證
    internal void updateLICENSEID(string tableName)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update " + tableName + " set LICENSE_ID = @LICENSE_ID");
            if (tableName == "TB_I_R_DATAUPDAE_HIS" || tableName == "TB_I_S_FAMILY_FEES")
                sb.Append("  where EMP_ID = @EMP_ID   ");
            else
                sb.Append(" ,UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID  where EMP_ID = @EMP_ID ");

            sb.Append(" and  LICENSE_ID = (select LICENSE_ID from TB_H_M_EMP where PJOB_CD='PJ16' and  EMP_ID=@EMP_ID ) ");
            ht.Add("@LICENSE_ID", LICENSE_ID);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            ht.Add("@EMP_ID", EMP_ID);

            dbConn.Execute(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }
    internal void updateALLOVERTIME_CTL_CD()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_H_M_EMP set OVERTIME_CTL_CD = @OVERTIME_CTL_CD ");
            sb.Append(" ,UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID  ");
            sb.Append(" where LEAVE_DT is null  ");
            ht.Add("@OVERTIME_CTL_CD", '1');
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2HB010");

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void updateOVERTIME_CTL_CD()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_H_M_EMP set OVERTIME_CTL_CD = @OVERTIME_CTL_CD,HEALTH_YEAR = @HEALTH_YEAR,");
            sb.Append(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID from TB_H_M_EMP where EMP_ID = @EMP_ID ");
            ht.Add("@OVERTIME_CTL_CD", OVERTIME_CTL_CD);
            ht.Add("@HEALTH_YEAR", HEALTH_YEAR);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            ht.Add("@EMP_ID", EMP_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getDefaultData(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select EMP_ID,EMP_NAME, NATION_CD,JPN_CD,BLOOD_TYPE,GRADE,");
            sb.Append(" (select sub_cd+'-'+ sub_desc From TB_9_M_COMM_D Where SYS_CD = 'HB' and MAIN_CD = 'NATION_CD' and sub_cd = NATION_CD) as NATION_CD_DESC, ");
            sb.Append(" (select sub_cd+'-'+ sub_desc From TB_9_M_COMM_D Where SYS_CD = 'HB' and MAIN_CD = 'JPN_CD' and sub_cd = JPN_CD) as JPN_CD_DESC, ");
            sb.Append(" (select sub_cd+'-'+ sub_desc From TB_9_M_COMM_D Where SYS_CD = 'HB' and MAIN_CD = 'ARMY_CD' and IS_VALID='Y' and sub_cd = ARMY_CD) as ARMY_CD_DESC, ");
            sb.Append(" (select sub_cd+'-'+ sub_desc From TB_9_M_COMM_D Where SYS_CD = 'HB' and MAIN_CD = 'OVERTIME_CTL_CD' and IS_VALID='Y' and sub_cd = OVERTIME_CTL_CD) as OVERTIME_CTL_CD_DESC, ");
            sb.Append(" (select sub_cd+'-'+ sub_desc From TB_9_M_COMM_D Where SYS_CD = 'SC' and MAIN_CD = 'INCOME_CD' and IS_VALID='Y' and sub_cd = INCOME_CD) as INCOME_CD_DESC, ");
            //sb.Append(" (select sub_cd+'-'+ sub_desc From TB_9_M_COMM_D Where SYS_CD = 'HB' and MAIN_CD = 'FAMILY_RELATION' and IS_VALID='Y' and sub_cd = URGENT_CONTACT_RELATION) as FAMILY_RELATION_DESC, ");
            sb.Append(" URGENT_CONTACT_RELATION as FAMILY_RELATION_DESC, ");
            sb.Append(" (select sub_cd+'-'+ sub_desc From TB_9_M_COMM_D Where SYS_CD = 'HB' and MAIN_CD = 'WS_CD' and IS_VALID='Y'  and sub_cd = WS_CD) as WS_CD_DESC, ");
            sb.Append(" SEX_CD,ARMY_CD,LICENSE_ID,PASSPORT_ID,a.UNION_PJOB_CD,");
            sb.Append(" REPLACE(CONVERT(char(10), BIRTH_DT, 120),'-','/') BIRTH_DT,");
            sb.Append(" BLOOD_TYPE, HEIGHT,[WEIGHT],BIRTHPLACE,SALARY_ACCOUNT_BANK, ");
            sb.Append(" (select SUB_DESC from TB_9_M_COMM_D where SUB_CD = SALARY_ACCOUNT_BANK and SYS_CD = 'HB' and MAIN_CD = 'SALARY_ACCOUNT_BANK') as  SALARY_ACCOUNT_BANK_NAME, ");
            sb.Append(" SALARY_ACCOUNT_NO, SALARY_ACCOUNT_BRANCH,");
            sb.Append(" (select CODE_VAL1 + @EMP_ID + '.jpg' from TB_9_M_PARAMETER where TB_9_M_PARAMETER.SYS_CD = 'HB' and TB_9_M_PARAMETER.MAIN_CD = 'PHOTO_PATH') PHOTO_PATH,");
            sb.Append(" a.REMARK,COMPANY_CD + '-' + COMPANY_NAME as COMPANY_NAME,PLANT_CD + '-' + PLANT_NAME PLANT_NAME,");
            sb.Append(" EMP_CD + '-' + EMP_DESC EMP_DESC,WS_CD,WORK_CD + '-' + WORK_DESC WORK_DESC,");
            sb.Append(" DEPT_NO + ' ' + DEPT_FULL_NAME DEPT_FULL_NAME,DEPT_FULL_NAME DEPT_FULL_NAME2,DEPT_NO,LEVEL_CD,GRADE_CD,PJOB_CD + ' ' + PJOB_DESC PJOB_DESC,");
            sb.Append(" WORK_SHIFT_CD + WORK_SHIFT_DESC WORK_SHIFT_DESC,CALENDAR_CD + ' ' + CALENDAR_DESC CALENDAR_DESC,");
            sb.Append(" REPLACE(CONVERT(char(10), JOIN_DT, 120),'-','/') JOIN_DT,");
            sb.Append(" REPLACE(CONVERT(char(10), DL_GEN_DT, 120),'-','/') DL_GEN_DT,");
            sb.Append(" REPLACE(CONVERT(char(10), EXAM_EXPIRE_DT, 120),'-','/') EXAM_EXPIRE_DT,");
            sb.Append(" REPLACE(CONVERT(char(10), PLAN_DESPATCH_DT, 120),'-','/') PLAN_DESPATCH_DT,");
            sb.Append(" REPLACE(CONVERT(char(10), BE_DESPATCH_DT, 120),'-','/') BE_DESPATCH_DT,");
            sb.Append(" REPLACE(CONVERT(char(10), KEEP_DESPATCH_DT, 120),'-','/') KEEP_DESPATCH_DT,");
            sb.Append(" REPLACE(CONVERT(char(10), BE_CONTRACT_DT, 120),'-','/') BE_CONTRACT_DT,");
            sb.Append(" REPLACE(CONVERT(char(10), BE_EMP_DT, 120),'-','/') BE_EMP_DT,");
            sb.Append(" IS_MASTER,IS_UPD_HEAD,");
            sb.Append(" (select EMP_NAME from TB_H_M_EMP where a.DIRECT_HEAD_EMP_ID = TB_H_M_EMP.EMP_ID) DIRECT_HEAD_EMP_NAME,DIRECT_HEAD_EMP_ID,");
            sb.Append(" OVERTIME_CTL_CD,HEALTH_YEAR,IS_DUTY_CHECK,MODEL_YEAR,HONOR_YEAR,");
            sb.Append(" (select UNION_PJOB_DESC from TB_D_M_UNION_PJOB where a.UNION_PJOB_CD = TB_D_M_UNION_PJOB.UNION_PJOB_CD) UNION_PJOB_DESC,");
            sb.Append(" ACC_CD + '-' + e.SUB_DESC ACC_CD,");
            sb.Append(" REPLACE(CONVERT(char(10), RECENT_LEVEL_DT, 120),'-','/') RECENT_LEVEL_DT,");
            sb.Append(" REPLACE(CONVERT(char(10), RECENT_PJOB_DT, 120),'-','/') RECENT_PJOB_DT,");
            sb.Append(" REPLACE(CONVERT(char(10), RECENT_DEPT_DT, 120),'-','/') RECENT_DEPT_DT,");
            sb.Append(" REPLACE(CONVERT(char(10), RECENT_DIV_DT, 120),'-','/') RECENT_DIV_DT,");
            sb.Append(" convert(varchar(50),convert(float(50),round(RECENT_LEVEL_WORK_DAYS / 365,2))) RECENT_LEVEL_WORK_DAYS,");
            sb.Append(" convert(varchar(50),convert(float(50),round(RECENT_PJOB_WORK_DAYS / 365,2))) RECENT_PJOB_WORK_DAYS,");
            sb.Append(" convert(varchar(50),convert(float(50),round(RECENT_DEPT_WORK_DAYS / 365,2))) RECENT_DEPT_WORK_DAYS,");
            sb.Append(" convert(varchar(50),convert(float(50),round(RECENT_DIV_WORK_DAYS / 365,2))) RECENT_DIV_WORK_DAYS,");
            sb.Append(" convert(varchar(50),convert(float(50),round(STUDENT_WORK_DAYS / 365,2))) STUDENT_WORK_DAYS,");
            sb.Append(" convert(varchar(50),convert(float(50),round(K_WORK_DAYS / 365,2))) K_WORK_DAYS,");
            sb.Append(" convert(varchar(50),convert(float(50),round(T_WORK_DAYS / 365,2))) T_WORK_DAYS,");
            sb.Append(" WORK_YEARS,SERVICE_YEARS,");
            sb.Append(" REPLACE(CONVERT(char(10), LEAVE_DT, 120),'-','/') LEAVE_DT,");
            sb.Append(" LEAVE_REASON_DESC,");
            sb.Append(" REPLACE(CONVERT(char(10), PLAN_RETENTION_EDT, 120),'-','/') PLAN_RETENTION_EDT,");
            sb.Append(" REPLACE(CONVERT(char(10), RETENTION_EDT, 120),'-','/') RETENTION_EDT,");
            sb.Append(" REPLACE(CONVERT(char(10), TRANSFER_SDT, 120),'-','/') TRANSFER_SDT,");
            sb.Append(" TRANSFER_REASON_DESC,");
            sb.Append(" REPLACE(CONVERT(char(10), PLAN_TRANSFER_EDT, 120),'-','/') PLAN_TRANSFER_EDT,");
            sb.Append(" REPLACE(CONVERT(char(10), TRANSFER_EDT, 120),'-','/') TRANSFER_EDT,");
            sb.Append(" REPLACE(CONVERT(char(10), BACK_SCHOOL_DT, 120),'-','/') BACK_SCHOOL_DT,");
            sb.Append(" REPLACE(CONVERT(char(10), BACK_PLANT_DT, 120),'-','/') BACK_PLANT_DT,");
            sb.Append(" REGISTER_ZIP_CD,REGISTER_COUNTY,REGISTER_REGION,REGISTER_ADDR,REGISTER_TEL,");
            sb.Append(" CONTACT_ZIP_CD,CONTACT_COUNTY,CONTACT_REGION,CONTACT_ADDR,CONTACT_TEL,");
            sb.Append(" PERSONAL_EMAIL,SALARY_EMAIL_CD,MOBILE_TEL_1,MOBILE_TEL_2,COMPANY_EMAIL, ");
            //sb.Append(" COMPANY_EXT,");
            sb.Append(" URGENT_CONTACT_NAME,URGENT_CONTACT_RELATION,URGENT_CONTACT_TEL,RELATIVES,INCOME_CD");
            sb.Append(" from VW_H_EMP_DATA a left join TB_9_M_COMM_D b on a.NATION_CD = b.SUB_CD AND b.SYS_CD = 'HB' and b.MAIN_CD = 'NATION_CD' ");
            sb.Append(" left join TB_9_M_COMM_D c on a.JPN_CD = c.SUB_CD AND c.SYS_CD = 'HB' AND c.MAIN_CD = 'JPN_CD'");
            sb.Append(" left join TB_9_M_COMM_D d on a.ARMY_CD = d.SUB_CD AND d.SYS_CD = 'HB' AND d.MAIN_CD = 'ARMY_CD'");
            sb.Append(" left join TB_9_M_COMM_D e on a.ACC_CD = e.SUB_CD AND e.SYS_CD = 'HA' AND e.MAIN_CD = 'ACC_CD'");
            sb.Append(" WHERE a.EMP_ID = @EMP_ID");
            ht.Add("@EMP_ID", emp_id);
            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }

    //Gridview 查詢資料
    public DataTable getFamData(int startRowIndex, int maximumRows, string sortExpression, string emp_id)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" a.EMP_ID,FAMILY_RELATION,FAMILY_RELATION + '-' + b.SUB_DESC FAMILY_RELATION_DESC,FAMILY_NAME,REPLACE(CONVERT(char(10), a.FAMILY_BIRTH_DT, 120),'-','/') FAMILY_BIRTH_DT, ");
            sb.Append(" FAMILY_LICENSE_ID,FAMILY_LICENSE_ID FAMILY_ORI_LICENSE_ID,FAMILY_WORK_DESC,FAMILY_PASSPORT_ID,c.SUB_DESC FAMILY_NATION_DESC, FAMILY_NATION_CD,FAMILY_SEX_CD,IS_ALLOWANCE,");
            sb.Append(" BENEFICIARY, VENDOR_ID, a.IS_VALID,  ");
            sb.Append(" case FAMILY_SEX_CD when '1' then '1-男' when '2' then '2-女' end as FAMILY_SEX_DESC,REPLACE(CONVERT(char(10), a.FAMILY_BIRTH_DT, 120),'-','/') FAMILY_ORI_BIRTH_DT");
            sb.Append(" from TB_H_M_EMP_FAMILY a,TB_9_M_COMM_D b ,TB_9_M_COMM_D c");
            sb.Append(" where a.FAMILY_RELATION = b.SUB_CD and b.MAIN_CD = 'FAMILY_RELATION' ");
            sb.Append(" and a.FAMILY_NATION_CD = c.SUB_CD and c.MAIN_CD = 'NATION_CD' ");
            if (emp_id != "")
            {
                sb.Append(" and a.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
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
    public int getFamCount(int startRowIndex, int maximumRows, string emp_id)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select COUNT(*) total_record ");
            sb.Append(" from TB_H_M_EMP_FAMILY a,TB_9_M_COMM_D b ");
            sb.Append(" where a.FAMILY_RELATION = b.SUB_CD and b.MAIN_CD = 'FAMILY_RELATION' ");
            if (emp_id != "")
            {
                sb.Append(" and a.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
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

    //Gridview 查詢資料
    public DataTable getEduData(int startRowIndex, int maximumRows, string sortExpression, string emp_id)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" a.EMP_ID,EDUCATION_CD,SCHOOL_NATION_CD,SCHOOL_NATION_CD + '-' + b.SUB_DESC SCHOOL_NATION_DESC,EDUCATION_CD + '-' + c.SUB_DESC EDUCATION_DESC, ");
            sb.Append(" SCHOOL_NAME,DEPARTMENT_NAME,GRADUATION_YEAR,IS_SALARY_SCHOOL,IS_VIRTUAL_SCHOOL");

            sb.Append(" from TB_H_M_EMP_EDUCATION a,TB_9_M_COMM_D b,TB_9_M_COMM_D c");
            sb.Append(" where a.SCHOOL_NATION_CD = b.SUB_CD and b.MAIN_CD = 'NATION_CD' ");
            sb.Append(" and a.EDUCATION_CD = c.SUB_CD and c.MAIN_CD = 'EDUCATION_CD' ");

            if (emp_id != "")
            {
                sb.Append(" and a.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
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
    public int getEduCount(int startRowIndex, int maximumRows, string emp_id)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select COUNT(*) total_record ");
            sb.Append(" from TB_H_M_EMP_EDUCATION a,TB_9_M_COMM_D b,TB_9_M_COMM_D c");
            sb.Append(" where a.SCHOOL_NATION_CD = b.SUB_CD and b.MAIN_CD = 'NATION_CD' ");
            sb.Append(" and a.EDUCATION_CD = c.SUB_CD and c.MAIN_CD = 'EDUCATION_CD' ");

            if (emp_id != "")
            {
                sb.Append(" and a.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
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

    //Gridview 查詢資料
    public DataTable getExpData(int startRowIndex, int maximumRows, string sortExpression, string emp_id)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" EMP_ID,EXP_COMPANY_NAME,EXP_TITLE_DESC,START_YEAR,END_YEAR,APPROVE_WORK_YEARS ");

            sb.Append(" from TB_H_M_EMP_EXPERIENCE");
            sb.Append(" where EMP_ID is not null ");

            if (emp_id != "")
            {
                sb.Append(" and EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
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
    public int getExpCount(int startRowIndex, int maximumRows, string emp_id)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select COUNT(*) total_record ");
            sb.Append(" from TB_H_M_EMP_EXPERIENCE");
            sb.Append(" where EMP_ID is not null ");

            if (emp_id != "")
            {
                sb.Append(" and EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
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

    //Gridview 查詢資料
    public DataTable getOtherJobData(int startRowIndex, int maximumRows, string sortExpression, string emp_id)
    {
        try
        {


            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" a.HR_CHG_NO,a.EMP_ID,OTHER_DEPT_NO,OTHER_DEPT_NAME,OTHER_PJOB_CD,OTHER_PJOB_DESC,");
            sb.Append(" ORI_DIV_DEPT_FULL_NAME,ORI_DEPT_NO");
            sb.Append(" from (select * from  TB_H_R_EMP_OTHER_PJOB where END_DT is null )a ");
            sb.Append(" left join TB_H_M_EMP_HR_CHANGE_H b on a.HR_CHG_NO = b.HR_CHG_NO and a.EMP_ID = b.EMP_ID ");
            sb.Append(" where 1=1 ");
            if (emp_id != "")
            {
                sb.Append(" and a.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
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
    public int getOtherJobTrainCount(int startRowIndex, int maximumRows, string emp_id)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select COUNT(*) total_record ");
            sb.Append(" from TB_H_R_EMP_OTHER_PJOB a ");
            sb.Append(" left join TB_H_M_EMP_HR_CHANGE_H b on a.HR_CHG_NO = b.HR_CHG_NO and a.EMP_ID = b.EMP_ID ");
            sb.Append(" where 1=1 ");
            if (emp_id != "")
            {
                sb.Append(" and a.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
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

    public DataTable getEdu(string sortExpression)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" a.EMP_ID,EDUCATION_CD,SCHOOL_NATION_CD,SCHOOL_NATION_CD + '-' + b.SUB_DESC SCHOOL_NATION_DESC,EDUCATION_CD + '-' + c.SUB_DESC EDUCATION_DESC, ");
            sb.Append(" SCHOOL_NAME,DEPARTMENT_NAME,GRADUATION_YEAR,IS_SALARY_SCHOOL,IS_VIRTUAL_SCHOOL");

            sb.Append(" from TB_H_M_EMP_EDUCATION a left join TB_9_M_COMM_D b on a.SCHOOL_NATION_CD = b.SUB_CD and b.MAIN_CD = 'NATION_CD' inner join TB_9_M_COMM_D c");
            sb.Append(" on a.EDUCATION_CD = c.SUB_CD and c.MAIN_CD = 'EDUCATION_CD' ");
            sb.Append(" where a.EMP_ID is not null ");

            if (EMP_ID != "")
            {
                sb.Append(" and a.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", EMP_ID);
            }

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    public DataTable getExp(string sortExpression)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();


            sb.Append(" Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" EMP_ID,EXP_COMPANY_NAME,EXP_TITLE_DESC,START_YEAR,END_YEAR,APPROVE_WORK_YEARS ");

            sb.Append(" from TB_H_M_EMP_EXPERIENCE");
            sb.Append(" where EMP_ID is not null ");

            if (EMP_ID != "")
            {
                sb.Append(" and EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", EMP_ID);
            }



            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    internal DataTable getUNION_PJOB_CD()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();


            sb.Append(" Select UNION_PJOB_CD,UNION_PJOB_DESC");

            sb.Append(" from TB_D_M_UNION_PJOB");
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void update3IN1_TXN()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //更新 勞保健保勞退履歷主檔
            sb.Append(" Update TB_I_M_3IN1_TXN Set LICENSE_ID = @LICENCE_ID,");
            sb.Append(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID");
            sb.Append(" where EMP_ID = @EMP_ID and CONVERT(varchar,EFFECT_EDT,111) = '9999/12/31' and IDENTITY_KIND = '1' and LICENSE_ID = @OLICENSE_ID ;");

            //更新 月份保費代扣資料檔
            sb.Append(" Update TB_I_R_FEES_MONTH Set LICENSE_ID = @LICENCE_ID,");
            sb.Append(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID");
            sb.Append(" where EMP_ID = @EMP_ID and IDENTITY_KIND = '1' and LICENSE_ID = @OLICENSE_ID ;");

            //更新 團保主檔
            sb.Append(" Update TB_I_M_GROUP_TXN Set LICENSE_ID = @LICENCE_ID,");
            sb.Append(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID ");
            sb.Append(" where EMP_ID = @EMP_ID and IDENTITY_KIND = '1' and LICENSE_ID = @OLICENSE_ID ;");

            //更新 保險減免資料履歷檔
            sb.Append(" Update TB_I_M_REDUCE_TXN Set LICENSE_ID = @LICENCE_ID,");
            sb.Append(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID");
            sb.Append(" where EMP_ID = @EMP_ID and CONVERT(varchar,EFFECT_EDT,111) = '9999/12/31' and IDENTITY_KIND = '1' and LICENSE_ID = @OLICENSE_ID ;");

            //更新 月份團保保費代扣資料檔
            sb.Append(" Update TB_I_R_GROUP_MONTH Set LICENSE_ID = @LICENCE_ID,");
            sb.Append(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID");
            sb.Append(" where EMP_ID = @EMP_ID and IDENTITY_KIND = '1' and LICENSE_ID = @OLICENSE_ID ;");

            //更新 保費比對異常資料檔
            sb.Append(" Update TB_I_M_BILLS_COMPARE Set LICENSE_ID = @LICENCE_ID,");
            sb.Append(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID");
            sb.Append(" where EMP_ID = @EMP_ID and LICENSE_ID = @OLICENSE_ID ;");

            //更新 保費追溯資料檔
            sb.Append(" Update TB_I_M_FEES_TRACEBACK Set LICENSE_ID = @LICENCE_ID,");
            sb.Append(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID");
            sb.Append(" where EMP_ID = @EMP_ID and IDENTITY_KIND = '1'and LICENSE_ID = @OLICENSE_ID ;");

            ht.Add("@LICENCE_ID", LICENSE_ID);
            ht.Add("@OLICENSE_ID", ORI_LICENSE_ID);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            ht.Add("@EMP_ID", EMP_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }


    #region "家庭成員檔新刪修"

    //檢查PK值有無重覆
    internal DataTable getFamPKData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" select count(0) resultCount 
                         from TB_H_M_EMP_FAMILY
                         where EMP_ID=@EMP_ID and FAMILY_LICENSE_ID=@FAMILY_LICENSE_ID ");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@FAMILY_LICENSE_ID", FAMILY_LICENSE_ID);
            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //檢查PK值有無重覆
    internal DataTable getBirthdayDay()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" select count(0) resultCount 
                         from TB_H_M_EMP_FAMILY
                         where EMP_ID=@EMP_ID and FAMILY_LICENSE_ID=@FAMILY_LICENSE_ID 
                         and FAMILY_BIRTH_DT = @FAMILY_BIRTH_DT
                        ");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@FAMILY_LICENSE_ID", FAMILY_LICENSE_ID);
            ht.Add("@FAMILY_BIRTH_DT", FAMILY_BIRTH_DT);
            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //新增家庭成員檔
    internal void insertFamData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Insert into TB_H_M_EMP_FAMILY (EMP_ID,FAMILY_LICENSE_ID,FAMILY_PASSPORT_ID,FAMILY_NATION_CD,");
            sb.Append(" FAMILY_SEX_CD,FAMILY_NAME,FAMILY_RELATION,FAMILY_BIRTH_DT,FAMILY_WORK_DESC,IS_ALLOWANCE,BENEFICIARY,VENDOR_ID,IS_VALID,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" Values (@EMP_ID,@FAMILY_LICENSE_ID,@FAMILY_PASSPORT_ID,@FAMILY_NATION_CD,");
            sb.Append(" @FAMILY_SEX_CD,@FAMILY_NAME,@FAMILY_RELATION,@FAMILY_BIRTH_DT,@FAMILY_WORK_DESC,@IS_ALLOWANCE,@BENEFICIARY,@VENDOR_ID,@IS_VALID,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@FAMILY_LICENSE_ID", FAMILY_LICENSE_ID);
            ht.Add("@FAMILY_PASSPORT_ID", FAMILY_PASSPORT_ID);
            ht.Add("@FAMILY_NATION_CD", FAMILY_NATION_CD);
            ht.Add("@FAMILY_SEX_CD", FAMILY_SEX_CD);
            ht.Add("@FAMILY_NAME", FAMILY_NAME);
            ht.Add("@FAMILY_RELATION", FAMILY_RELATION);
            ht.Add("@FAMILY_BIRTH_DT", FAMILY_BIRTH_DT != "" ? DateTime.Parse(FAMILY_BIRTH_DT).ToShortDateString() : "");
            ht.Add("@FAMILY_WORK_DESC", FAMILY_WORK_DESC);
            //若是 無效的,則IS_ALLOWANCE, BENEFICIARY 為N
            if (IS_VALID == "N")
            {
                ht.Add("@IS_ALLOWANCE", "N");
                ht.Add("@BENEFICIARY", "N");
            }
            else
            {
                ht.Add("@IS_ALLOWANCE", IS_ALLOWANCE);
                ht.Add("@BENEFICIARY", BENEFICIARY);
            }


            ht.Add("@VENDOR_ID", VENDOR_ID);
            ht.Add("@IS_VALID", IS_VALID);
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

    //修改家庭成員檔
    internal void updateFamilyData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(@" update TB_H_M_EMP_FAMILY 
                        set  FAMILY_PASSPORT_ID = @FAMILY_PASSPORT_ID
                            ,FAMILY_NAME = @FAMILY_NAME
                            ,FAMILY_RELATION =@FAMILY_RELATION
                            ,FAMILY_BIRTH_DT = @FAMILY_BIRTH_DT
                            ,FAMILY_WORK_DESC = @FAMILY_WORK_DESC
                            ,IS_ALLOWANCE =@IS_ALLOWANCE
                            ,BENEFICIARY =@BENEFICIARY
                            ,VENDOR_ID=@VENDOR_ID
                            ,IS_VALID = @IS_VALID
                            , UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID
                        where EMP_ID=@EMP_ID
                        and FAMILY_LICENSE_ID=@FAMILY_LICENSE_ID            

                         ");
            //PK值
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@FAMILY_LICENSE_ID", FAMILY_LICENSE_ID);

            //修改值
            ht.Add("@FAMILY_PASSPORT_ID", FAMILY_PASSPORT_ID);
            ht.Add("@FAMILY_NAME", FAMILY_NAME);
            ht.Add("@FAMILY_RELATION", FAMILY_RELATION);
            ht.Add("@FAMILY_BIRTH_DT", FAMILY_BIRTH_DT != "" ? DateTime.Parse(FAMILY_BIRTH_DT).ToShortDateString() : "");
            ht.Add("@FAMILY_WORK_DESC", FAMILY_WORK_DESC);
            //若是 無效的,則IS_ALLOWANCE, BENEFICIARY 為N
            if (IS_VALID == "N")
            {
                ht.Add("@IS_ALLOWANCE", "N");
                ht.Add("@BENEFICIARY", "N");
            }
            else
            {
                ht.Add("@IS_ALLOWANCE", IS_ALLOWANCE);
                ht.Add("@BENEFICIARY", BENEFICIARY);
            }


            ht.Add("@VENDOR_ID", VENDOR_ID);
            ht.Add("@IS_VALID", IS_VALID);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //更新家庭成員保險資料主檔
    internal int updateFam_PERSONDATA()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //更新 保險資料主檔
            sb.Append(" Update TB_I_M_PERSONDATA Set LICENSE_ID = @LICENCE_ID,EMP_NAME = @EMP_NAME,BIRTH_DT = @BIRTH_DT,");
            sb.Append(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID");
            sb.Append(" where EMP_ID = @EMP_ID and LICENSE_ID_FIRST = @LICENCE_ID_FIRST");
            ht.Add("@LICENCE_ID", FAMILY_LICENSE_ID);
            ht.Add("@EMP_NAME", FAMILY_NAME);
            ht.Add("@BIRTH_DT", FAMILY_BIRTH_DT);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@LICENCE_ID_FIRST", FAMILY_LICENSE_ID);

            return dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }


    //新增家庭成員保險歷史檔
    internal void insertFam_DATAUPDAE_HIS()
    {
        try
        {
            //新增 保險資料更新歷史檔
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Insert into TB_I_R_DATAUPDAE_HIS (EMP_ID,LICENSE_ID,CREATED_DT,LICENSE_ID_FIRST,");
            sb.Append(" EMP_NAME,BIRTH_DT,CREATED_BY,FUNC_ID)");
            sb.Append(" Values (@EMP_ID,@LICENCE_ID,GETDATE(),@LICENCE_ID_FIRST,");
            sb.Append(" @EMP_NAME,@BIRTH_DT,@CREATED_BY,@FUNC_ID)");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@LICENCE_ID", FAMILY_LICENSE_ID);
            ht.Add("@LICENCE_ID_FIRST", FAMILY_LICENSE_ID);
            ht.Add("@EMP_NAME", FAMILY_NAME);
            ht.Add("@BIRTH_DT", FAMILY_BIRTH_DT);
            ht.Add("@CREATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //刪除家庭成員檔
    internal void deleteFamData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" update TB_H_M_EMP_FAMILY 
                        set 
                        IS_VALID = @IS_VALID
                        ,UPDATED_DT =getdate(),UPDATED_BY = @UPDATED_BY,FUNC_ID = @FUNC_ID 
                        Where EMP_ID = @EMP_ID and FAMILY_LICENSE_ID =@FAMILY_LICENSE_ID
                        ");
            ht.Add("@IS_VALID", "N");//無效(非使用中)
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@FAMILY_LICENSE_ID", FAMILY_LICENSE_ID);
            ht.Add("@FUNC_ID", FUNC_ID);
            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {

            throw;
        }
    }


    #endregion


    #region "學歷檔新刪修"

    //檢查PK值有無重覆
    internal DataTable getEducationPKData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" select count(0) resultCount 
                         from TB_H_M_EMP_EDUCATION
                         where EMP_ID=@EMP_ID and EDUCATION_CD=@EDUCATION_CD ");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@EDUCATION_CD", EDUCATION_CD);
            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //新增 學歷檔
    internal void insertEducationData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Insert into TB_H_M_EMP_EDUCATION (EMP_ID,EDUCATION_CD,SCHOOL_NATION_CD,SCHOOL_NAME,");
            sb.Append(" DEPARTMENT_NAME,GRADUATION_YEAR,IS_SALARY_SCHOOL,IS_VIRTUAL_SCHOOL,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" Values (@EMP_ID,@EDUCATION_CD,@SCHOOL_NATION_CD,@SCHOOL_NAME,");
            sb.Append(" @DEPARTMENT_NAME,@GRADUATION_YEAR,@IS_SALARY_SCHOOL,@IS_VIRTUAL_SCHOOL,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SCHOOL_NATION_CD", SCHOOL_NATION_CD);
            ht.Add("@EDUCATION_CD", EDUCATION_CD);
            ht.Add("@SCHOOL_NAME", SCHOOL_NAME);
            ht.Add("@DEPARTMENT_NAME", DEPARTMENT_NAME);
            ht.Add("@GRADUATION_YEAR", GRADUATION_YEAR);
            ht.Add("@IS_SALARY_SCHOOL", IS_SALARY_SCHOOL);
            ht.Add("@IS_VIRTUAL_SCHOOL", IS_VIRTUAL_SCHOOL);
            ht.Add("@CREATED_BY", UPDATED_BY);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            dbConn.ExecuteT(sb, ht, true);


        }
        catch (Exception)
        {

            throw;
        }
    }

    //修改 學歷檔
    internal void updateEducationData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(@" update TB_H_M_EMP_EDUCATION 
                        set  SCHOOL_NATION_CD=@SCHOOL_NATION_CD
                            ,SCHOOL_NAME=@SCHOOL_NAME
                            ,DEPARTMENT_NAME=@DEPARTMENT_NAME
                            ,GRADUATION_YEAR=@GRADUATION_YEAR
                            ,IS_SALARY_SCHOOL=@IS_SALARY_SCHOOL
                            ,IS_VIRTUAL_SCHOOL=@IS_VIRTUAL_SCHOOL
                            ,UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID
                        where EMP_ID=@EMP_ID
                        and EDUCATION_CD=@EDUCATION_CD            
                         ");
            //PK值
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@EDUCATION_CD", EDUCATION_CD);

            //修改值
            ht.Add("@SCHOOL_NATION_CD", SCHOOL_NATION_CD);
            ht.Add("@SCHOOL_NAME", SCHOOL_NAME);
            ht.Add("@DEPARTMENT_NAME", DEPARTMENT_NAME);
            ht.Add("@GRADUATION_YEAR", GRADUATION_YEAR);
            ht.Add("@IS_SALARY_SCHOOL", IS_SALARY_SCHOOL);
            ht.Add("@IS_VIRTUAL_SCHOOL", IS_VIRTUAL_SCHOOL);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //刪除 學歷檔
    internal void deleteEducationData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            //寫log
            sb.Append(" update TB_H_M_EMP_EDUCATION set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = @FUNC_ID  ");
            sb.Append(" Where EMP_ID = @EMP_ID and EDUCATION_CD =@EDUCATION_CD ");
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", FUNC_ID);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@EDUCATION_CD", EDUCATION_CD);
            dbConn.ExecuteT(sb, ht, true);
            sb.Clear();
            ht.Clear();

            sb.Append(@" delete from  TB_H_M_EMP_EDUCATION 
                        Where EMP_ID = @EMP_ID and EDUCATION_CD =@EDUCATION_CD
                        ");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@EDUCATION_CD", EDUCATION_CD);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }


    #endregion
    

    #region "經歷檔 新刪修"
    //檢查經歷檔PK值有無重覆
    internal DataTable getExperPKData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" select count(0) resultCount 
                         from TB_H_M_EMP_EXPERIENCE
                         where EMP_ID=@EMP_ID and EXP_COMPANY_NAME=@EXP_COMPANY_NAME ");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@EXP_COMPANY_NAME", EXP_COMPANY_NAME);
            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //新增 經歷檔
    internal void insertExperData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Insert into TB_H_M_EMP_EXPERIENCE (EMP_ID,EXP_COMPANY_NAME,EXP_TITLE_DESC,START_YEAR,");
            sb.Append(" END_YEAR,APPROVE_WORK_YEARS,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" Values (@EMP_ID,@EXP_COMPANY_NAME,@EXP_TITLE_DESC,@START_YEAR,");
            sb.Append(" @END_YEAR,@APPROVE_WORK_YEARS,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@EXP_COMPANY_NAME", EXP_COMPANY_NAME);
            ht.Add("@EXP_TITLE_DESC", EXP_TITLE_DESC);
            ht.Add("@START_YEAR", START_YEAR);
            ht.Add("@END_YEAR", END_YEAR);
            ht.Add("@APPROVE_WORK_YEARS", APPROVE_WORK_YEARS);
            ht.Add("@CREATED_BY", UPDATED_BY);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {

            throw;
        }
    }

    //修改 經歷檔
    internal void updateExperData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(@" update TB_H_M_EMP_EXPERIENCE 
                        set  EXP_TITLE_DESC=@EXP_TITLE_DESC
                            ,START_YEAR=@START_YEAR
                            ,END_YEAR=@END_YEAR
                            ,APPROVE_WORK_YEARS=@APPROVE_WORK_YEARS
                            ,UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID
                        where EMP_ID=@EMP_ID
                        and EXP_COMPANY_NAME=@EXP_COMPANY_NAME            
                         ");
            //PK值
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@EXP_COMPANY_NAME", EXP_COMPANY_NAME);

            //修改值
            ht.Add("@EXP_TITLE_DESC", EXP_TITLE_DESC);
            ht.Add("@START_YEAR", START_YEAR);
            ht.Add("@END_YEAR", END_YEAR);
            ht.Add("@APPROVE_WORK_YEARS", APPROVE_WORK_YEARS);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //刪除 經歷檔
    internal void deleteExperData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            //寫log
            sb.Append(" update TB_H_M_EMP_EXPERIENCE set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = @FUNC_ID  ");
            sb.Append(" Where EMP_ID = @EMP_ID and EXP_COMPANY_NAME =@EXP_COMPANY_NAME ");
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", FUNC_ID);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@EXP_COMPANY_NAME", EXP_COMPANY_NAME);
            dbConn.ExecuteT(sb, ht, true);
            sb.Clear();
            ht.Clear();
            sb.Append(@" delete from  TB_H_M_EMP_EXPERIENCE 
                        Where EMP_ID = @EMP_ID and EXP_COMPANY_NAME =@EXP_COMPANY_NAME
                        ");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@EXP_COMPANY_NAME", EXP_COMPANY_NAME);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    #endregion


    internal void updateFamData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            //寫log
            sb.Append(" update TB_H_M_EMP_FAMILY set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2HB010' ");
            sb.Append(" Where EMP_ID = @EMP_ID;");
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);
            ht.Add("@EMP_ID", EMP_ID);
            dbConn.ExecuteT(sb, ht, true);
            //更新家庭成員
            sb.Clear();
            //先刪除全部再新增
            sb.Append(" Delete From TB_H_M_EMP_FAMILY Where EMP_ID = @EMP_ID;");
            ht.Add("@EMP_ID", EMP_ID);
            dbConn.ExecuteT(sb, ht, true);

            for (int i = 0; i < EMP_FAMILY.Rows.Count; i++)
            {
                sb.Append("Insert into TB_H_M_EMP_FAMILY (EMP_ID,FAMILY_LICENSE_ID,FAMILY_PASSPORT_ID,FAMILY_NATION_CD,");
                sb.Append(" FAMILY_SEX_CD,FAMILY_NAME,FAMILY_RELATION,FAMILY_BIRTH_DT,FAMILY_WORK_DESC,IS_ALLOWANCE,BENEFICIARY,VENDOR_ID,IS_VALID,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
                sb.Append(" Values (@EMP_ID,@FAMILY_LICENSE_ID,@FAMILY_PASSPORT_ID,@FAMILY_NATION_CD,");
                sb.Append(" @FAMILY_SEX_CD,@FAMILY_NAME,@FAMILY_RELATION,@FAMILY_BIRTH_DT,@FAMILY_WORK_DESC,@IS_ALLOWANCE,@BENEFICIARY,@VENDOR_ID,@IS_VALID,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

                ht.Add("@EMP_ID", EMP_FAMILY.Rows[i]["EMP_ID"].ToString());
                ht.Add("@FAMILY_LICENSE_ID", EMP_FAMILY.Rows[i]["FAMILY_LICENSE_ID"].ToString());
                ht.Add("@FAMILY_PASSPORT_ID", EMP_FAMILY.Rows[i]["FAMILY_PASSPORT_ID"].ToString());
                ht.Add("@FAMILY_NATION_CD", EMP_FAMILY.Rows[i]["FAMILY_NATION_CD"].ToString());
                ht.Add("@FAMILY_SEX_CD", EMP_FAMILY.Rows[i]["FAMILY_SEX_CD"].ToString());
                ht.Add("@FAMILY_NAME", EMP_FAMILY.Rows[i]["FAMILY_NAME"].ToString());
                ht.Add("@FAMILY_RELATION", EMP_FAMILY.Rows[i]["FAMILY_RELATION"].ToString());
                ht.Add("@FAMILY_BIRTH_DT", EMP_FAMILY.Rows[i]["FAMILY_BIRTH_DT"].ToString() != "" ? DateTime.Parse(EMP_FAMILY.Rows[i]["FAMILY_BIRTH_DT"].ToString()).ToShortDateString() : "");
                ht.Add("@FAMILY_WORK_DESC", EMP_FAMILY.Rows[i]["FAMILY_WORK_DESC"].ToString());
                //若是 無效的,則IS_ALLOWANCE, BENEFICIARY 為N
                if (EMP_FAMILY.Rows[i]["IS_VALID"].ToString() == "N")
                {
                    ht.Add("@IS_ALLOWANCE", "N");
                    ht.Add("@BENEFICIARY", "N");
                }
                else
                {
                    ht.Add("@IS_ALLOWANCE", EMP_FAMILY.Rows[i]["IS_ALLOWANCE"].ToString());
                    ht.Add("@BENEFICIARY", EMP_FAMILY.Rows[i]["BENEFICIARY"].ToString());
                }


                ht.Add("@VENDOR_ID", EMP_FAMILY.Rows[i]["VENDOR_ID"].ToString());
                ht.Add("@IS_VALID", EMP_FAMILY.Rows[i]["IS_VALID"].ToString());
                ht.Add("@CREATED_BY", UPDATED_BY);
                ht.Add("@UPDATED_BY", UPDATED_BY);
                ht.Add("@FUNC_ID", FUNC_ID);
                dbConn.ExecuteT(sb, ht, true);
            }
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void updateEduData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //寫log
            sb.Append(" update TB_H_M_EMP_EDUCATION set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2HB010' ");
            sb.Append(" Where EMP_ID = @EMP_ID;");
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);
            ht.Add("@EMP_ID", EMP_ID);
            dbConn.ExecuteT(sb, ht, true);
            sb.Clear();

            //更新學歷
            //先刪除全部再新增
            sb.Append(" Delete From TB_H_M_EMP_EDUCATION Where EMP_ID = @EMP_ID;");
            ht.Add("@EMP_ID", EMP_ID);
            dbConn.ExecuteT(sb, ht, true);
            sb.Clear();

            for (int i = 0; i < EDU_DATA.Rows.Count; i++)
            {
                sb.Append("Insert into TB_H_M_EMP_EDUCATION (EMP_ID,EDUCATION_CD,SCHOOL_NATION_CD,SCHOOL_NAME,");
                sb.Append(" DEPARTMENT_NAME,GRADUATION_YEAR,IS_SALARY_SCHOOL,IS_VIRTUAL_SCHOOL,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
                sb.Append(" Values (@EMP_ID,@EDUCATION_CD,@SCHOOL_NATION_CD,@SCHOOL_NAME,");
                sb.Append(" @DEPARTMENT_NAME,@GRADUATION_YEAR,@IS_SALARY_SCHOOL,@IS_VIRTUAL_SCHOOL,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

                ht.Add("@EMP_ID", EDU_DATA.Rows[i]["EMP_ID"].ToString());
                ht.Add("@SCHOOL_NATION_CD", EDU_DATA.Rows[i]["SCHOOL_NATION_CD"].ToString());
                ht.Add("@EDUCATION_CD", EDU_DATA.Rows[i]["EDUCATION_CD"].ToString());
                ht.Add("@SCHOOL_NAME", EDU_DATA.Rows[i]["SCHOOL_NAME"].ToString());
                ht.Add("@DEPARTMENT_NAME", EDU_DATA.Rows[i]["DEPARTMENT_NAME"].ToString());
                ht.Add("@GRADUATION_YEAR", EDU_DATA.Rows[i]["GRADUATION_YEAR"].ToString());
                ht.Add("@IS_SALARY_SCHOOL", EDU_DATA.Rows[i]["IS_SALARY_SCHOOL"].ToString());
                ht.Add("@IS_VIRTUAL_SCHOOL", EDU_DATA.Rows[i]["IS_VIRTUAL_SCHOOL"].ToString());
                ht.Add("@CREATED_BY", UPDATED_BY);
                ht.Add("@UPDATED_BY", UPDATED_BY);
                ht.Add("@FUNC_ID", FUNC_ID);
                dbConn.ExecuteT(sb, ht, true);
            }
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void updateExpData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //寫log
            sb.Append(" update TB_H_M_EMP_EXPERIENCE set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2HB010' ");
            sb.Append(" Where EMP_ID = @EMP_ID;");
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);
            ht.Add("@EMP_ID", EMP_ID);
            dbConn.ExecuteT(sb, ht, true);
            sb.Clear();

            //更新經歷
            //先刪除全部再新增
            sb.Append(" Delete From TB_H_M_EMP_EXPERIENCE Where EMP_ID = @EMP_ID;");

            ht.Add("@EMP_ID", EMP_ID);
            dbConn.ExecuteT(sb, ht, true);

            for (int i = 0; i < EXP_DATA.Rows.Count; i++)
            {
                sb.Append("Insert into TB_H_M_EMP_EXPERIENCE (EMP_ID,EXP_COMPANY_NAME,EXP_TITLE_DESC,START_YEAR,");
                sb.Append(" END_YEAR,APPROVE_WORK_YEARS,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
                sb.Append(" Values (@EMP_ID,@EXP_COMPANY_NAME,@EXP_TITLE_DESC,@START_YEAR,");
                sb.Append(" @END_YEAR,@APPROVE_WORK_YEARS,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

                ht.Add("@EMP_ID", EXP_DATA.Rows[i]["EMP_ID"].ToString());
                ht.Add("@EXP_COMPANY_NAME", EXP_DATA.Rows[i]["EXP_COMPANY_NAME"].ToString());
                ht.Add("@EXP_TITLE_DESC", EXP_DATA.Rows[i]["EXP_TITLE_DESC"].ToString());
                ht.Add("@START_YEAR", EXP_DATA.Rows[i]["START_YEAR"].ToString());
                ht.Add("@END_YEAR", EXP_DATA.Rows[i]["END_YEAR"].ToString());
                ht.Add("@APPROVE_WORK_YEARS", EXP_DATA.Rows[i]["APPROVE_WORK_YEARS"].ToString());
                ht.Add("@CREATED_BY", UPDATED_BY);
                ht.Add("@UPDATED_BY", UPDATED_BY);
                ht.Add("@FUNC_ID", FUNC_ID);

                dbConn.ExecuteT(sb, ht, true);
            }
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void updateDUTY_CHECK_HIS()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //更新 員工刷卡管制設定歷史檔
            sb.Append(@" update TB_D_M_IS_DUTY_CHECK_HIS
                        set OVERTIME_CTL_CD=@OVERTIME_CTL_CD
                        , UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE()
                        where EMP_ID=@EMP_ID
                        and CALENDAR_DT>=@CALENDAR_DT
            ");

            ht.Add("@OVERTIME_CTL_CD", OVERTIME_CTL_CD);
            ht.Add("@CALENDAR_DT", OVERTIME_CTL_DT);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            ;

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    internal void update_EMP_DUTY_CHECK()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //更新 員工刷卡管制設定歷史檔
            sb.Append(@" update TB_D_M_EMP_DUTY_CHECK_STATUS
                        set  OVERTIME_CTL_CD=@OVERTIME_CTL_CD
                        ,UPDATED_BY = @UPDATED_BY
                        ,UPDATED_DT = GETDATE()
                        , FUNC_ID = @FUNC_ID
                        where EMP_ID=@EMP_ID
                        and CALENDAR_DT>=@CALENDAR_DT
            ");

            ht.Add("@OVERTIME_CTL_CD", OVERTIME_CTL_CD);
            ht.Add("@CALENDAR_DT", OVERTIME_CTL_DT);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            ;

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }


    internal void updatePERSONDATA()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //更新 保險資料主檔
            sb.Append(" Update TB_I_M_PERSONDATA Set LICENSE_ID = @LICENCE_ID,EMP_NAME = @EMP_NAME,BIRTH_DT = @BIRTH_DT,");
            sb.Append(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID");
            sb.Append(" where EMP_ID = @EMP_ID and LICENSE_ID_FIRST = @LICENCE_ID_FIRST");
            ht.Add("@LICENCE_ID", LICENSE_ID);
            ht.Add("@EMP_NAME", EMP_NAME);
            ht.Add("@BIRTH_DT", BIRTH_DT);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@LICENCE_ID_FIRST", ORI_LICENSE_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void insertDATAUPDAE_HIS()
    {
        try
        {
            //新增 保險資料更新歷史檔
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Insert into TB_I_R_DATAUPDAE_HIS (EMP_ID,LICENSE_ID,CREATED_DT,LICENSE_ID_FIRST,");
            sb.Append(" EMP_NAME,BIRTH_DT,CREATED_BY,FUNC_ID)");
            sb.Append(" Values (@EMP_ID,@LICENCE_ID,GETDATE(),@LICENCE_ID_FIRST,");
            sb.Append(" @EMP_NAME,@BIRTH_DT,@CREATED_BY,@FUNC_ID)");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@LICENCE_ID", LICENSE_ID);
            ht.Add("@LICENCE_ID_FIRST", ORI_LICENSE_ID);
            ht.Add("@EMP_NAME", EMP_NAME);
            ht.Add("@BIRTH_DT", BIRTH_DT);
            ht.Add("@CREATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //更新家庭成員保險資料主檔
    internal void updateFam_PERSONDATA(DataRow dataRow)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //更新 保險資料主檔
            sb.Append(" Update TB_I_M_PERSONDATA Set LICENSE_ID = @LICENCE_ID,EMP_NAME = @EMP_NAME,BIRTH_DT = @BIRTH_DT,");
            sb.Append(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID");
            sb.Append(" where EMP_ID = @EMP_ID and LICENSE_ID_FIRST = @LICENCE_ID_FIRST");
            ht.Add("@LICENCE_ID", dataRow["FAMILY_LICENSE_ID"].ToString());
            ht.Add("@EMP_NAME", dataRow["FAMILY_NAME"].ToString());
            ht.Add("@BIRTH_DT", dataRow["FAMILY_BIRTH_DT"].ToString());
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@LICENCE_ID_FIRST", dataRow["FAMILY_ORI_LICENSE_ID"].ToString());

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }
    //新增家庭成員保險歷史檔
    internal void insertFam_DATAUPDAE_HIS(DataRow dataRow)
    {
        try
        {
            //新增 保險資料更新歷史檔
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Insert into TB_I_R_DATAUPDAE_HIS (EMP_ID,LICENSE_ID,CREATED_DT,LICENSE_ID_FIRST,");
            sb.Append(" EMP_NAME,BIRTH_DT,CREATED_BY,FUNC_ID)");
            sb.Append(" Values (@EMP_ID,@LICENCE_ID,GETDATE(),@LICENCE_ID_FIRST,");
            sb.Append(" @EMP_NAME,@BIRTH_DT,@CREATED_BY,@FUNC_ID)");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@LICENCE_ID", dataRow["FAMILY_LICENSE_ID"].ToString());
            ht.Add("@LICENCE_ID_FIRST", dataRow["FAMILY_ORI_LICENSE_ID"].ToString());
            ht.Add("@EMP_NAME", dataRow["FAMILY_NAME"].ToString());
            ht.Add("@BIRTH_DT", dataRow["FAMILY_BIRTH_DT"].ToString());
            ht.Add("@CREATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public string ORI_EMP_NAME { get; set; }

    public string ORI_BIRTH_DT { get; set; }

    public string ORI_LICENSE_ID { get; set; }


    internal DataTable getGRADECD(string LEVEL_CD)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select GRADE_CD from TB_H_M_LEVEL_GRADE where LEVEL_CD = @LEVEL_CD order by GRADE_CD");
            ht.Add("@LEVEL_CD", LEVEL_CD);
            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }

    //會因為有觸發程序而無法正常運作
    internal DataTable getNewEMP_ID()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" with cte as (");
            sb.Append(" select CODE_VAL1,CODE_VAL1 + 1 NEW_EMP_ID");
            sb.Append(" from TB_9_M_PARAMETER");
            sb.Append(" where SYS_CD = 'HB' and MAIN_CD = 'NEXT_EMP_ID'");
            sb.Append(" )");
            sb.Append(" update cte ");
            sb.Append(" set cte.CODE_VAL1 = cte.NEW_EMP_ID");
            sb.Append(" output inserted.CODE_VAL1 NEW_EMP_ID;");
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal string getNextEMP_ID()
    {
        try
        {
            string next_emp_id = "";
            string st = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select * ");
            sb.Append(" from TB_9_M_PARAMETER");
            sb.Append(" where 1=1");
            sb.Append(" and SYS_CD = @SYS_CD ");
            sb.Append(" and MAIN_CD = @MAIN_CD ");
            ht.Add("@SYS_CD", "HB");
            ht.Add("@MAIN_CD", "NEXT_EMP_ID");


            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                next_emp_id = dt.Rows[0]["CODE_VAL1"].ToString();
            }
            return next_emp_id;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //UPDATE 參數檔 next emp id +1
    public void update_next_emp_id(string next_emp_id)
    {
        try
        {
            string st = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("update TB_9_M_PARAMETER ");
            if (next_emp_id != "")
            {
                sb.Append(" set CODE_VAL1 =@CODE_VAL1 ");
                sb.Append(" , UPDATED_BY =@UPDATED_BY ");
                sb.Append(" , UPDATED_DT =@UPDATED_DT ");
                sb.Append(" , FUNC_ID =@FUNC_ID ");
                ht.Add("@CODE_VAL1", next_emp_id);
                ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
                ht.Add("@UPDATED_DT", DateTime.Parse(DateTime.Now.ToString()));
                ht.Add("@FUNC_ID", "FB2HB010");
            }
            sb.Append(" where SYS_CD ='HB' and MAIN_CD = 'NEXT_EMP_ID'");


            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }

    internal void addEmpData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Insert into TB_H_M_EMP (EMP_ID,EMP_NAME,WS_CD,COMPANY_CD,PLANT_CD,DEPT_NO,EMP_CD,LEVEL_CD,GRADE_CD,PJOB_CD,WORK_SHIFT_CD,");
            sb.Append(" WORK_CD,JOIN_DT,EXAM_EXPIRE_DT,PLAN_DESPATCH_DT,IS_MASTER,IS_UPD_HEAD,DIRECT_HEAD_EMP_ID,OVERTIME_CTL_CD,HEALTH_YEAR,");
            sb.Append(" IS_DUTY_CHECK,UNION_PJOB_CD,MODEL_YEAR,NATION_CD,JPN_CD,LICENSE_ID,PASSPORT_ID,SEX_CD,BIRTH_DT,BLOOD_TYPE,HEIGHT,WEIGHT,");
            sb.Append(" BIRTHPLACE,ARMY_CD,SALARY_ACCOUNT_BANK,SALARY_ACCOUNT_BRANCH,SALARY_ACCOUNT_NO,REMARK,RELATIVES,INCOME_CD,URGENT_CONTACT_NAME,URGENT_CONTACT_RELATION,URGENT_CONTACT_TEL,");
            sb.Append(" REGISTER_ZIP_CD,REGISTER_COUNTY,REGISTER_REGION,REGISTER_ADDR,REGISTER_TEL,CONTACT_ZIP_CD,CONTACT_COUNTY,CONTACT_REGION,CONTACT_ADDR,");
            sb.Append(" CONTACT_TEL,MOBILE_TEL_1,MOBILE_TEL_2,PERSONAL_EMAIL,COMPANY_EMAIL,SALARY_EMAIL_CD,SALARY_EMAIL, ");
            sb.Append(" RECENT_LEVEL_DT,RECENT_PJOB_DT,RECENT_DEPT_DT,RECENT_DIV_DT, ");
            //sb.Append(" COMPANY_EXT,");
            sb.Append(" CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" values (@EMP_ID,@EMP_NAME,@WS_CD,@COMPANY_CD,@PLANT_CD,@DEPT_NO,@EMP_CD,@LEVEL_CD,@GRADE_CD,@PJOB_CD,@WORK_SHIFT_CD,");
            sb.Append(" @WORK_CD,@JOIN_DT,@EXAM_EXPIRE_DT,@PLAN_DESPATCH_DT,@IS_MASTER,@IS_UPD_HEAD,@DIRECT_HEAD_EMP_ID,@OVERTIME_CTL_CD,@HEALTH_YEAR,");
            sb.Append(" @IS_DUTY_CHECK,@UNION_PJOB_CD,@MODEL_YEAR,@NATION_CD,@JPN_CD,@LICENSE_ID,@PASSPORT_ID,@SEX_CD,@BIRTH_DT,@BLOOD_TYPE,@HEIGHT,@WEIGHT,");
            sb.Append(" @BIRTHPLACE,@ARMY_CD,@ACCOUNT_BANK,@SALARY_ACCOUNT_BRANCH,@SALARY_ACCOUNT_NO,@REMARK,@RELATIVES,@INCOME_CD,@URGENT_CONTACT_NAME,@URGENT_CONTACT_RELATION,@URGENT_CONTACT_TEL,");
            sb.Append(" @REGISTER_ZIP_CD,@REGISTER_COUNTY,@REGISTER_REGION,@REGISTER_ADDR,@REGISTER_TEL,@CONTACT_ZIP_CD,@CONTACT_COUNTY,@CONTACT_REGION,@CONTACT_ADDR,");
            sb.Append(" @CONTACT_TEL,@MOBILE_TEL_1,@MOBILE_TEL_2,@PERSONAL_EMAIL,@COMPANY_EMAIL,@SALARY_EMAIL_CD,@SALARY_EMAIL,");
            sb.Append(" @RECENT_LEVEL_DT,@RECENT_PJOB_DT,@RECENT_DEPT_DT,@RECENT_DIV_DT, ");
            //sb.Append(" @COMPANY_EXT,");
            sb.Append(" @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@EMP_NAME", EMP_NAME);
            ht.Add("@WS_CD", WS_CD);
            ht.Add("@COMPANY_CD", COMPANY_CD);
            ht.Add("@PLANT_CD", PLANT_CD);
            ht.Add("@DEPT_NO", DEPT_NO);
            ht.Add("@EMP_CD", EMP_CD);
            ht.Add("@LEVEL_CD", LEVEL_CD);
            ht.Add("@GRADE_CD", GRADE_CD);
            ht.Add("@PJOB_CD", PJOB_CD);
            ht.Add("@WORK_SHIFT_CD", WORK_SHIFT_CD);
            ht.Add("@WORK_CD", WORK_CD);
            ht.Add("@JOIN_DT", JOIN_DT);
            ht.Add("@RECENT_LEVEL_DT", JOIN_DT);
            ht.Add("@RECENT_PJOB_DT", JOIN_DT);
            ht.Add("@RECENT_DEPT_DT", JOIN_DT);
            ht.Add("@RECENT_DIV_DT", JOIN_DT);
            ht.Add("@EXAM_EXPIRE_DT", EXAM_EXPIRE_DT);
            if (PLAN_DESPATCH_DT == "")
                ht.Add("@PLAN_DESPATCH_DT", DBNull.Value);
            else
                ht.Add("@PLAN_DESPATCH_DT", PLAN_DESPATCH_DT);
            ht.Add("@IS_MASTER", IS_MASTER);
            ht.Add("@IS_UPD_HEAD", IS_UPD_HEAD);
            ht.Add("@DIRECT_HEAD_EMP_ID", DIRECT_HEAD_EMP_ID);
            ht.Add("@OVERTIME_CTL_CD", OVERTIME_CTL_CD);
            ht.Add("@HEALTH_YEAR", HEALTH_YEAR);
            ht.Add("@IS_DUTY_CHECK", IS_DUTY_CHECK);
            ht.Add("@UNION_PJOB_CD", UNION_PJOB_CD == "-1" ? "" : UNION_PJOB_CD);
            ht.Add("@MODEL_YEAR", MODEL_YEAR);
            ht.Add("@NATION_CD", NATION_CD);
            ht.Add("@JPN_CD", JPN_CD);
            ht.Add("@LICENSE_ID", LICENSE_ID);
            ht.Add("@PASSPORT_ID", PASSPORT_ID);
            ht.Add("@SEX_CD", SEX_CD);
            ht.Add("@BIRTH_DT", BIRTH_DT);
            ht.Add("@BLOOD_TYPE", BLOOD_TYPE);
            ht.Add("@HEIGHT", HEIGHT);
            ht.Add("@WEIGHT", WEIGHT);
            ht.Add("@BIRTHPLACE", BIRTHPLACE);
            ht.Add("@ARMY_CD", ARMY_CD);
            ht.Add("@ACCOUNT_BANK", ACCOUNT_BANK);
            ht.Add("@SALARY_ACCOUNT_BRANCH", SALARY_ACCOUNT_BRANCH);
            ht.Add("@SALARY_ACCOUNT_NO", SALARY_ACCOUNT_NO);
            ht.Add("@REMARK", REMARK);
            ht.Add("@RELATIVES", RELATIVES);
            ht.Add("@INCOME_CD", INCOME_CD);
            ht.Add("@URGENT_CONTACT_NAME", URGENT_CONTACT_NAME);
            ht.Add("@URGENT_CONTACT_RELATION", URGENT_CONTACT_RELATION);
            ht.Add("@URGENT_CONTACT_TEL", URGENT_CONTACT_TEL);
            ht.Add("@REGISTER_ZIP_CD", REGISTER_ZIP_CD);
            ht.Add("@REGISTER_COUNTY", REGISTER_COUNTY);
            ht.Add("@REGISTER_REGION", REGISTER_REGION);
            ht.Add("@REGISTER_ADDR", REGISTER_ADDR);
            ht.Add("@REGISTER_TEL", REGISTER_TEL);
            ht.Add("@CONTACT_ZIP_CD", CONTACT_ZIP_CD);
            ht.Add("@CONTACT_COUNTY", CONTACT_COUNTY);
            ht.Add("@CONTACT_REGION", CONTACT_REGION);
            ht.Add("@CONTACT_ADDR", CONTACT_ADDR);
            ht.Add("@CONTACT_TEL", CONTACT_TEL);
            ht.Add("@MOBILE_TEL_1", MOBILE_TEL_1);
            ht.Add("@MOBILE_TEL_2", MOBILE_TEL_2);
            ht.Add("@PERSONAL_EMAIL", PERSONAL_EMAIL);
            ht.Add("@COMPANY_EMAIL", COMPANY_EMAIL);
            ht.Add("@SALARY_EMAIL", SALARY_EMAIL);
            ht.Add("@SALARY_EMAIL_CD", SALARY_EMAIL_CD);
            //ht.Add("@COMPANY_EXT", COMPANY_EXT);
            ht.Add("@CREATED_BY", UPDATED_BY);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void addFamData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            for (int i = 0; i < EMP_FAMILY.Rows.Count; i++)
            {

                sb.Append("Insert into TB_H_M_EMP_FAMILY (EMP_ID,FAMILY_LICENSE_ID,FAMILY_PASSPORT_ID,FAMILY_NATION_CD,");
                sb.Append(" FAMILY_SEX_CD,FAMILY_NAME,FAMILY_RELATION,FAMILY_BIRTH_DT,FAMILY_WORK_DESC,IS_ALLOWANCE,BENEFICIARY,VENDOR_ID,IS_VALID,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
                sb.Append(" Values (@EMP_ID,@FAMILY_LICENSE_ID,@FAMILY_PASSPORT_ID,@FAMILY_NATION_CD,");
                sb.Append(" @FAMILY_SEX_CD,@FAMILY_NAME,@FAMILY_RELATION,@FAMILY_BIRTH_DT,@FAMILY_WORK_DESC,@IS_ALLOWANCE,@BENEFICIARY,@VENDOR_ID,@IS_VALID,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

                ht.Add("@EMP_ID", EMP_FAMILY.Rows[i]["EMP_ID"].ToString());
                ht.Add("@FAMILY_LICENSE_ID", EMP_FAMILY.Rows[i]["FAMILY_LICENSE_ID"].ToString());
                ht.Add("@FAMILY_PASSPORT_ID", EMP_FAMILY.Rows[i]["FAMILY_PASSPORT_ID"].ToString());
                ht.Add("@FAMILY_NATION_CD", EMP_FAMILY.Rows[i]["FAMILY_NATION_CD"].ToString());
                ht.Add("@FAMILY_SEX_CD", EMP_FAMILY.Rows[i]["FAMILY_SEX_CD"].ToString());
                ht.Add("@FAMILY_NAME", EMP_FAMILY.Rows[i]["FAMILY_NAME"].ToString());
                ht.Add("@FAMILY_RELATION", EMP_FAMILY.Rows[i]["FAMILY_RELATION"].ToString());
                ht.Add("@FAMILY_BIRTH_DT", EMP_FAMILY.Rows[i]["FAMILY_BIRTH_DT"].ToString() != "" ? DateTime.Parse(EMP_FAMILY.Rows[i]["FAMILY_BIRTH_DT"].ToString()).ToShortDateString() : "");
                ht.Add("@FAMILY_WORK_DESC", EMP_FAMILY.Rows[i]["FAMILY_WORK_DESC"].ToString());
                ht.Add("@IS_ALLOWANCE", EMP_FAMILY.Rows[i]["IS_ALLOWANCE"].ToString());
                ht.Add("@BENEFICIARY", EMP_FAMILY.Rows[i]["BENEFICIARY"].ToString());
                ht.Add("@VENDOR_ID", EMP_FAMILY.Rows[i]["VENDOR_ID"].ToString());
                ht.Add("@IS_VALID", EMP_FAMILY.Rows[i]["IS_VALID"].ToString());
                ht.Add("@CREATED_BY", UPDATED_BY);
                ht.Add("@UPDATED_BY", UPDATED_BY);
                ht.Add("@FUNC_ID", FUNC_ID);
                dbConn.ExecuteT(sb, ht, true);
            }
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void addEduData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            for (int i = 0; i < EDU_DATA.Rows.Count; i++)
            {
                sb.Append("Insert into TB_H_M_EMP_EDUCATION (EMP_ID,EDUCATION_CD,SCHOOL_NATION_CD,SCHOOL_NAME,");
                sb.Append(" DEPARTMENT_NAME,GRADUATION_YEAR,IS_SALARY_SCHOOL,IS_VIRTUAL_SCHOOL,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
                sb.Append(" Values (@EMP_ID,@EDUCATION_CD,@SCHOOL_NATION_CD,@SCHOOL_NAME,");
                sb.Append(" @DEPARTMENT_NAME,@GRADUATION_YEAR,@IS_SALARY_SCHOOL,@IS_VIRTUAL_SCHOOL,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

                ht.Add("@EMP_ID", EDU_DATA.Rows[i]["EMP_ID"].ToString());
                ht.Add("@SCHOOL_NATION_CD", EDU_DATA.Rows[i]["SCHOOL_NATION_CD"].ToString() == "-1" ? "" : EDU_DATA.Rows[i]["SCHOOL_NATION_CD"].ToString());
                ht.Add("@EDUCATION_CD", EDU_DATA.Rows[i]["EDUCATION_CD"].ToString());
                ht.Add("@SCHOOL_NAME", EDU_DATA.Rows[i]["SCHOOL_NAME"].ToString());
                ht.Add("@DEPARTMENT_NAME", EDU_DATA.Rows[i]["DEPARTMENT_NAME"].ToString());
                ht.Add("@GRADUATION_YEAR", EDU_DATA.Rows[i]["GRADUATION_YEAR"].ToString());
                ht.Add("@IS_SALARY_SCHOOL", EDU_DATA.Rows[i]["IS_SALARY_SCHOOL"].ToString());
                ht.Add("@IS_VIRTUAL_SCHOOL", EDU_DATA.Rows[i]["IS_VIRTUAL_SCHOOL"].ToString());
                ht.Add("@CREATED_BY", UPDATED_BY);
                ht.Add("@UPDATED_BY", UPDATED_BY);
                ht.Add("@FUNC_ID", FUNC_ID);
                dbConn.ExecuteT(sb, ht, true);
            }
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void addExpData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            for (int i = 0; i < EXP_DATA.Rows.Count; i++)
            {
                sb.Append("Insert into TB_H_M_EMP_EXPERIENCE (EMP_ID,EXP_COMPANY_NAME,EXP_TITLE_DESC,START_YEAR,");
                sb.Append(" END_YEAR,APPROVE_WORK_YEARS,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
                sb.Append(" Values (@EMP_ID,@EXP_COMPANY_NAME,@EXP_TITLE_DESC,@START_YEAR,");
                sb.Append(" @END_YEAR,@APPROVE_WORK_YEARS,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

                ht.Add("@EMP_ID", EXP_DATA.Rows[i]["EMP_ID"].ToString());
                ht.Add("@EXP_COMPANY_NAME", EXP_DATA.Rows[i]["EXP_COMPANY_NAME"].ToString());
                ht.Add("@EXP_TITLE_DESC", EXP_DATA.Rows[i]["EXP_TITLE_DESC"].ToString());
                ht.Add("@START_YEAR", EXP_DATA.Rows[i]["START_YEAR"].ToString());
                ht.Add("@END_YEAR", EXP_DATA.Rows[i]["END_YEAR"].ToString());
                ht.Add("@APPROVE_WORK_YEARS", EXP_DATA.Rows[i]["APPROVE_WORK_YEARS"].ToString());
                ht.Add("@CREATED_BY", UPDATED_BY);
                ht.Add("@UPDATED_BY", UPDATED_BY);
                ht.Add("@FUNC_ID", FUNC_ID);

                dbConn.ExecuteT(sb, ht, true);
            }
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void addHR_CHANGE_H()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Insert into TB_H_M_EMP_HR_CHANGE_H (HR_CHG_NO,HR_CHG_CD,EMP_ID,START_DT ");
            sb.Append(" ,CHG_SEQ,INS_PLAN_PROC_DT,PLAN_END_DT,IS_END,IS_PAY_SUBSIST,HR_CHG_PROC_STATUS,INS_CHG_PROC_STATUS");
            sb.Append(" ,ORI_WS_CD,ORI_COMPANY_CD,ORI_PLANT_CD,ORI_DEPT_NO,ORI_EMP_CD,ORI_LEVEL_CD,ORI_GRADE_CD,ORI_PJOB_CD,ORI_WORK_SHIFT_CD,ORI_WORK_CD ");
            sb.Append(" ,ORI_DEPT_NAME,ORI_DEPT_FULL_NAME,ORI_DIV_DEPT_FULL_NAME,ORI_DEPT_NAME_20,ORI_DEPT_NAME_30,ORI_DEPT_NAME_40,ORI_DEPT_NAME_50,ORI_DEPT_NAME_60,ORI_DEPT_NAME_70");
            sb.Append(" ,ORI_PJOB_DESC ");
            sb.Append(" ,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");

            sb.Append(" select @HR_CHG_NO,@HR_CHG_CD,@EMP_ID,@START_DT");
            sb.Append("       ,@CHG_SEQ,@INS_PLAN_PROC_DT,@PLAN_END_DT,@IS_END,@IS_PAY_SUBSIST,@HR_CHG_PROC_STATUS,@INS_CHG_PROC_STATUS");
            sb.Append("       ,@WS_CD,@COMPANY_CD,@PLANT_CD,@DEPT_NO,@EMP_CD,@LEVEL_CD,@GRADE_CD,@PJOB_CD,@WORK_SHIFT_CD,@WORK_CD ");
            sb.Append("       ,D.DEPT_NAME,D.DEPT_FULL_NAME,D.DIV_DEPT_FULL_NAME,D.DEPT_NAME_20,D.DEPT_NAME_30,D.DEPT_NAME_40,D.DEPT_NAME_50,D.DEPT_NAME_60,D.DEPT_NAME_70 ");
            sb.Append("       ,(SELECT PJOB_DESC FROM VW_TB_H_M_PJOB WHERE PJOB_CD = @PJOB_CD)AS PJOB_DESC");
            sb.Append("       ,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID");
            sb.Append(" from VW_H_DEPT_DATA D WHERE DEPT_NO = @DEPT_NO ");


            ht.Add("@HR_CHG_NO", HR_CHG_NO);
            if (EMP_CD == "1" && PJOB_CD != "PJ60")
                ht.Add("@HR_CHG_CD", "A01");
            else if (EMP_CD == "2" || PJOB_CD == "PJ60")
                ht.Add("@HR_CHG_CD", "A02");
            else if (EMP_CD == "3")
                ht.Add("@HR_CHG_CD", "A03");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@START_DT", JOIN_DT);
            ht.Add("@CHG_SEQ", CHG_SEQ == "" ? "1" : CHG_SEQ);
            ht.Add("@INS_PLAN_PROC_DT", JOIN_DT);
            if (PLAN_DESPATCH_DT == "")
                ht.Add("@PLAN_END_DT", DBNull.Value);
            else
                ht.Add("@PLAN_END_DT", DateTime.Parse(PLAN_DESPATCH_DT).AddDays(-1).ToShortDateString());
            ht.Add("@IS_END", "N");
            ht.Add("@IS_PAY_SUBSIST", "N");
            ht.Add("@HR_CHG_PROC_STATUS", "N");
            ht.Add("@INS_CHG_PROC_STATUS", "N");

            ht.Add("@WS_CD", WS_CD);
            ht.Add("@COMPANY_CD", COMPANY_CD);
            ht.Add("@PLANT_CD", PLANT_CD);
            ht.Add("@DEPT_NO", DEPT_NO);
            ht.Add("@EMP_CD", EMP_CD);
            ht.Add("@LEVEL_CD", LEVEL_CD);
            ht.Add("@GRADE_CD", GRADE_CD);
            ht.Add("@PJOB_CD", PJOB_CD);
            ht.Add("@WORK_SHIFT_CD", WORK_SHIFT_CD);
            ht.Add("@WORK_CD", WORK_CD);

            ht.Add("@CREATED_BY", UPDATED_BY);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void addHR_CHANGE_D()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //第一筆
            sb.Append("Insert into TB_H_M_EMP_HR_CHANGE_D (HR_CHG_NO,EMP_ID,HR_CHG_ITEM,AFTER_CD,AFTER_DESC,");
            sb.Append(" CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" Values (@HR_CHG_NO,@EMP_ID,@HR_CHG_ITEM,@AFTER_CD,@AFTER_DESC,");
            sb.Append(" @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@HR_CHG_NO", HR_CHG_NO);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@HR_CHG_ITEM", "01");
            ht.Add("@AFTER_CD", COMPANY_CD);
            ht.Add("@AFTER_DESC", COMPANY_DESC);
            ht.Add("@CREATED_BY", UPDATED_BY);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);
            //第二筆
            sb.Append("Insert into TB_H_M_EMP_HR_CHANGE_D (HR_CHG_NO,EMP_ID,HR_CHG_ITEM,AFTER_CD,AFTER_DESC,");
            sb.Append(" CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" Values (@HR_CHG_NO,@EMP_ID,@HR_CHG_ITEM,@AFTER_CD,@AFTER_DESC,");
            sb.Append(" @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@HR_CHG_NO", HR_CHG_NO);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@HR_CHG_ITEM", "02");
            ht.Add("@AFTER_CD", PLANT_CD);
            ht.Add("@AFTER_DESC", PLANT_DESC);
            ht.Add("@CREATED_BY", UPDATED_BY);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);

            //第三筆
            sb.Append("Insert into TB_H_M_EMP_HR_CHANGE_D (HR_CHG_NO,EMP_ID,HR_CHG_ITEM,AFTER_CD,AFTER_DESC,");
            sb.Append(" CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" Values (@HR_CHG_NO,@EMP_ID,@HR_CHG_ITEM,@AFTER_CD,@AFTER_DESC,");
            sb.Append(" @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@HR_CHG_NO", HR_CHG_NO);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@HR_CHG_ITEM", "03");
            ht.Add("@AFTER_CD", WS_CD);
            ht.Add("@AFTER_DESC", WS_DESC);
            ht.Add("@CREATED_BY", UPDATED_BY);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);

            //第四筆
            sb.Append("Insert into TB_H_M_EMP_HR_CHANGE_D (HR_CHG_NO,EMP_ID,HR_CHG_ITEM,AFTER_CD,AFTER_DESC,");
            sb.Append(" CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" Values (@HR_CHG_NO,@EMP_ID,@HR_CHG_ITEM,@AFTER_CD,@AFTER_DESC,");
            sb.Append(" @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@HR_CHG_NO", HR_CHG_NO);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@HR_CHG_ITEM", "04");
            ht.Add("@AFTER_CD", EMP_CD);
            ht.Add("@AFTER_DESC", EMP_DESC);
            ht.Add("@CREATED_BY", UPDATED_BY);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);

            //第五筆
            sb.Append("Insert into TB_H_M_EMP_HR_CHANGE_D (HR_CHG_NO,EMP_ID,HR_CHG_ITEM,AFTER_CD,AFTER_DESC,");
            sb.Append(" CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" Values (@HR_CHG_NO,@EMP_ID,@HR_CHG_ITEM,@AFTER_CD,@AFTER_DESC,");
            sb.Append(" @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@HR_CHG_NO", HR_CHG_NO);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@HR_CHG_ITEM", "05");
            ht.Add("@AFTER_CD", DEPT_NO);
            ht.Add("@AFTER_DESC", DEPT_NAME);
            ht.Add("@CREATED_BY", UPDATED_BY);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);

            //第六筆
            sb.Append("Insert into TB_H_M_EMP_HR_CHANGE_D (HR_CHG_NO,EMP_ID,HR_CHG_ITEM,AFTER_CD,");
            sb.Append(" CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" Values (@HR_CHG_NO,@EMP_ID,@HR_CHG_ITEM,@AFTER_CD,");
            sb.Append(" @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@HR_CHG_NO", HR_CHG_NO);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@HR_CHG_ITEM", "06");
            ht.Add("@AFTER_CD", LEVEL_CD);
            ht.Add("@CREATED_BY", UPDATED_BY);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);

            //第七筆
            sb.Append("Insert into TB_H_M_EMP_HR_CHANGE_D (HR_CHG_NO,EMP_ID,HR_CHG_ITEM,AFTER_CD,");
            sb.Append(" CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" Values (@HR_CHG_NO,@EMP_ID,@HR_CHG_ITEM,@AFTER_CD,");
            sb.Append(" @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@HR_CHG_NO", HR_CHG_NO);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@HR_CHG_ITEM", "07");
            ht.Add("@AFTER_CD", GRADE_CD);
            ht.Add("@CREATED_BY", UPDATED_BY);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);

            //第八筆
            sb.Append("Insert into TB_H_M_EMP_HR_CHANGE_D (HR_CHG_NO,EMP_ID,HR_CHG_ITEM,AFTER_CD,AFTER_DESC,");
            sb.Append(" CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" Values (@HR_CHG_NO,@EMP_ID,@HR_CHG_ITEM,@AFTER_CD,@AFTER_DESC,");
            sb.Append(" @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@HR_CHG_NO", HR_CHG_NO);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@HR_CHG_ITEM", "08");
            ht.Add("@AFTER_CD", PJOB_CD);
            ht.Add("@AFTER_DESC", PJOB_DESC);
            ht.Add("@CREATED_BY", UPDATED_BY);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);

            //第九筆
            sb.Append("Insert into TB_H_M_EMP_HR_CHANGE_D (HR_CHG_NO,EMP_ID,HR_CHG_ITEM,AFTER_CD,AFTER_DESC,");
            sb.Append(" CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" Values (@HR_CHG_NO,@EMP_ID,@HR_CHG_ITEM,@AFTER_CD,@AFTER_DESC,");
            sb.Append(" @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@HR_CHG_NO", HR_CHG_NO);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@HR_CHG_ITEM", "09");
            ht.Add("@AFTER_CD", WORK_SHIFT_CD);
            ht.Add("@AFTER_DESC", WORK_SHIFT_DESC);
            ht.Add("@CREATED_BY", UPDATED_BY);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);

            //第十筆
            sb.Append("Insert into TB_H_M_EMP_HR_CHANGE_D (HR_CHG_NO,EMP_ID,HR_CHG_ITEM,AFTER_CD,AFTER_DESC,");
            sb.Append(" CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" Values (@HR_CHG_NO,@EMP_ID,@HR_CHG_ITEM,@AFTER_CD,@AFTER_DESC,");
            sb.Append(" @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@HR_CHG_NO", HR_CHG_NO);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@HR_CHG_ITEM", "10");
            ht.Add("@AFTER_CD", WORK_CD);
            ht.Add("@AFTER_DESC", WORK_DESC);
            ht.Add("@CREATED_BY", UPDATED_BY);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getNEW_HR_CHG_NO()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("with cte as (");
            sb.Append(" select SERIAL_NUMBER,RIGHT(REPLICATE('0', 4) + CAST(SERIAL_NUMBER as NVARCHAR), 4) NEW_HR_CHG_NO");
            sb.Append(" from TB_H_M_AUTO_NUMBER_CTL");
            sb.Append(" where AUTO_NUMBER_TYPE = 'HR_CHG_NO' and convert(varchar(10),AUTO_NUMBER_DT,120) = @AUTO_NUMBER_DT ");
            sb.Append(" )");
            sb.Append(" update cte ");
            sb.Append(" set cte.SERIAL_NUMBER = cte.SERIAL_NUMBER +1");
            sb.Append(" output inserted.NEW_HR_CHG_NO;");
            ht.Add("@AUTO_NUMBER_DT", JOIN_DT.Replace("/", "-"));
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public string HR_CHG_NO { get; set; }

    /*
    internal void updateNEW_HR_CHG_NO()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_H_M_AUTO_NUMBER_CTL set SERIAL_NUMBER = 2,AUTO_NUMBER_DT = @AUTO_NUMBER_DT");
            sb.Append(" from TB_H_M_AUTO_NUMBER_CTL where TB_H_M_AUTO_NUMBER_CTL.AUTO_NUMBER_TYPE = 'HR_CHG_NO'");
            ht.Add("@AUTO_NUMBER_DT", JOIN_DT.Replace("/", "-"));
            dbConn.Execute(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
    */
    internal void insertNEW_HR_CHG_NO()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("INSERT INTO TB_H_M_AUTO_NUMBER_CTL (AUTO_NUMBER_TYPE,AUTO_NUMBER_DT,SERIAL_NUMBER) ");
            sb.Append("VALUES( @AUTO_NUMBER_TYPE, @AUTO_NUMBER_DT, @SERIAL_NUMBER  )  ");
            ht.Add("@AUTO_NUMBER_TYPE", "HR_CHG_NO");
            ht.Add("@AUTO_NUMBER_DT", JOIN_DT.Replace("/", "-"));
            ht.Add("@SERIAL_NUMBER", "2");
            dbConn.Execute(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }


    internal DataTable getCHG_SEQ()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" select MAX(CHG_SEQ) + 1 CHG_SEQ ");
            sb.Append(" from TB_H_M_EMP_HR_CHANGE_H");
            sb.Append(" where EMP_ID = @EMP_ID and convert(varchar(10),START_DT,120) = @START_DT ");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@START_DT", JOIN_DT);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }



    public string ACCOUNT_BANK { get; set; }

    public string START_DT { get; set; }

    public string END_DT { get; set; }

    public string RENT_SUBSIDY { get; set; }

    internal void addEMP_DURATION()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Insert into TB_H_M_EMP_DURATION (EMP_ID,START_DT,END_DT,RENT_SUBSIDY,ORI_DEPT_NO,ORI_DEPT_FULL_NAME,ORI_DIV_DEPT_FULL_NAME,");
            sb.Append(" ORI_DEPT_NAME_20,ORI_DEPT_NAME_30,ORI_DEPT_NAME_40,ORI_LEVEL_CD,ORI_PJOB_CD,ORI_PJOB_DESC,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" select @EMP_ID,@START_DT,@END_DT,@RENT_SUBSIDY,@DEPT_NO,DEPT_FULL_NAME,DIV_DEPT_FULL_NAME,DEPT_NAME_20,DEPT_NAME_30,DEPT_NAME_40,@LEVEL_CD,@PJOB_CD,@PJOB_DESC,");
            sb.Append(" @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID ");
            sb.Append(" from VW_H_DEPT_DATA where DEPT_NO = @DEPT_NO ");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@START_DT", START_DT);
            ht.Add("@END_DT", END_DT);
            ht.Add("@RENT_SUBSIDY", RENT_SUBSIDY);
            ht.Add("@LEVEL_CD", LEVEL_CD);
            ht.Add("@PJOB_CD", PJOB_CD);
            ht.Add("@PJOB_DESC", PJOB_DESC);
            ht.Add("@DEPT_NO", DEPT_NO);
            ht.Add("@CREATED_BY", UPDATED_BY);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void addEMP_DURATION_BY_EMP()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Insert into TB_H_M_EMP_DURATION (EMP_ID,START_DT,END_DT,RENT_SUBSIDY,ORI_DEPT_NO,ORI_DEPT_FULL_NAME,ORI_DIV_DEPT_FULL_NAME,");
            sb.Append(" ORI_DEPT_NAME_20,ORI_DEPT_NAME_30,ORI_DEPT_NAME_40,ORI_LEVEL_CD,ORI_PJOB_CD,ORI_PJOB_DESC,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" select @EMP_ID,@START_DT,@END_DT,@RENT_SUBSIDY,DEPT_NO,DEPT_FULL_NAME,DIV_DEPT_FULL_NAME,DEPT_NAME_20,DEPT_NAME_30,DEPT_NAME_40,LEVEL_CD,PJOB_CD,PJOB_DESC, ");
            sb.Append(" @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID ");
            sb.Append(" from VW_H_EMP_DATA where EMP_ID = @EMP_ID ");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@START_DT", START_DT);
            ht.Add("@END_DT", END_DT);
            ht.Add("@RENT_SUBSIDY", RENT_SUBSIDY);
            ht.Add("@CREATED_BY", UPDATED_BY);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void SP_H_HR_CHG_PROC()
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(utilities.connstr))
            using (SqlCommand comm = new SqlCommand())
            {
                conn.Open();
                comm.Connection = conn;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = "SP_H_HR_CHG_PROC";
                comm.Parameters.AddWithValue("@pDate", JOIN_DT);
                comm.Parameters.AddWithValue("@pEmp_ID", EMP_ID);
                comm.Parameters.AddWithValue("@pUserID", SessionHandle.Current.emp_id);
                comm.Parameters.AddWithValue("@pFuncID", "FB2HB010");

                comm.CommandTimeout = 1000;
                comm.ExecuteNonQuery();

                conn.Close();
            }


        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void SP_H_EMP_HR_CHG_RECORD()
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(utilities.connstr))
            using (SqlCommand comm = new SqlCommand())
            {
                conn.Open();
                comm.Connection = conn;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = "SP_H_EMP_HR_CHG_RECORD";
                comm.Parameters.AddWithValue("@pDate", JOIN_DT);
                comm.Parameters.AddWithValue("@pUserID", SessionHandle.Current.emp_id);
                comm.Parameters.AddWithValue("@pFuncID", "FB2HB010");

                comm.CommandTimeout = 1000;
                comm.ExecuteNonQuery();

                conn.Close();
            }

        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void SP_H_UPD_DEPT_HEAD()
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(utilities.connstr))
            using (SqlCommand comm = new SqlCommand())
            {
                conn.Open();
                comm.Connection = conn;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = "SP_H_UPD_DEPT_HEAD";
                comm.Parameters.AddWithValue("@pDate", JOIN_DT);
                comm.Parameters.AddWithValue("@pUserID", SessionHandle.Current.emp_id);
                comm.Parameters.AddWithValue("@pFuncID", "FB2HB010");

                comm.CommandTimeout = 1000;
                comm.ExecuteNonQuery();

                conn.Close();
            }

        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void SP_D_UPD_CARD_DATA()
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(utilities.connstr))
            using (SqlCommand comm = new SqlCommand())
            {
                conn.Open();
                comm.Connection = conn;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = "SP_D_UPD_CARD_DATA";
                comm.Parameters.AddWithValue("@pHandleCd", "I");
                comm.Parameters.AddWithValue("@pEmpId", EMP_ID);
                // 2015/05/18 增加
                if (EMP_CD == "2")
                {
                    comm.Parameters.AddWithValue("@pCardUsedCd", "C");
                }
                else
                {
                    comm.Parameters.AddWithValue("@pCardUsedCd", "A");
                }

                comm.Parameters.AddWithValue("@pStartDt", JOIN_DT);
                comm.Parameters.AddWithValue("@pEndDt", "9999/12/31");
                comm.Parameters.AddWithValue("@pUserID", SessionHandle.Current.emp_id);
                comm.Parameters.AddWithValue("@pFuncID", "FB2HB010");

                comm.CommandTimeout = 1000;
                comm.ExecuteNonQuery();

                conn.Close();
            }


        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void SP_D_M_EMP_AVAILABLE_LEAVE()
    {
        try
        {
            using (SqlConnection conn = new SqlConnection(utilities.connstr))
            using (SqlCommand comm = new SqlCommand())
            {
                conn.Open();
                comm.Connection = conn;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = "SP_D_M_EMP_AVAILABLE_LEAVE";
                comm.Parameters.AddWithValue("@EMP_ID", EMP_ID);
                comm.Parameters.AddWithValue("@ABNORMAL_TYPE", "1");
                comm.Parameters.AddWithValue("@CALENDAR_DT", JOIN_DT);
                comm.Parameters.AddWithValue("@ABNORMAL_DT", JOIN_DT);
                comm.Parameters.AddWithValue("@ABNORMAL_REASON_CD", "4");
                comm.Parameters.AddWithValue("@USERID", SessionHandle.Current.emp_id);
                comm.Parameters.AddWithValue("@FUNCID", "FB2HB010");

                comm.CommandTimeout = 1000;
                comm.ExecuteNonQuery();

                conn.Close();
            }

        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getDurationData(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select REPLACE(CONVERT(char(10), START_DT, 120),'-','/') START_DT,REPLACE(CONVERT(char(10), END_DT, 120),'-','/') END_DT,RENT_SUBSIDY");
            sb.Append(" from TB_H_M_EMP_DURATION where EMP_ID = @EMP_ID");
            ht.Add("@EMP_ID", emp_id);
            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public string IS_DURATION { get; set; }

    internal void updateEMP_DURATION()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_H_M_EMP_DURATION set START_DT = @START_DT,END_DT = @END_DT,RENT_SUBSIDY = @RENT_SUBSIDY,");
            sb.Append(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID ");
            sb.Append(" from TB_H_M_EMP_DURATION a where EMP_ID = @EMP_ID");
            ht.Add("@START_DT", START_DT);
            ht.Add("@END_DT", END_DT);
            ht.Add("@RENT_SUBSIDY", RENT_SUBSIDY);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getEXAM_DAYS()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select CODE_VAL1 - 1 CODE_VAL1 from TB_9_M_PARAMETER");
            sb.Append(" where SYS_CD = 'HB' and MAIN_CD = 'EXAM_DAYS'");
            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getWorkShift(string work_shift_cd = "")
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select a.WORK_SHIFT_CD,a.WORK_SHIFT_DESC, a.CALENDAR_CD + '  ' + b.CALENDAR_DESC CALENDAR_DESC");
            sb.Append(" from TB_D_M_WORK_SHIFT_H a,TB_D_M_CALENDAR_H b,TB_9_M_PARAMETER c");
            sb.Append(" where a.CALENDAR_CD = b.CALENDAR_CD");
            sb.Append(" and a.WORK_SHIFT_CD = c.CODE_VAL1");
            sb.Append(" and c.SYS_CD = 'DB' and c.MAIN_CD = 'DEFAULT_WORK_SHIFT'");
            if (work_shift_cd != "")
            {
                sb.Append(" and a.WORK_SHIFT_CD = @WORK_SHIFT_CD");
            }
            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }



    internal DataTable getDUP_ALLOWANCE(string family_license_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select a.EMP_ID");
            sb.Append(" from TB_H_M_EMP a,TB_H_M_EMP_FAMILY b");
            sb.Append(" where a.EMP_ID = b.EMP_ID");
            sb.Append(" and a.LICENSE_ID = @LICENSE_ID");
            sb.Append(" and b.FAMILY_RELATION = '1' and b.IS_ALLOWANCE = 'Y'");
            ht.Add("@LICENSE_ID", family_license_id);
            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getDUP_EMP()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select EMP_ID");
            sb.Append(" from TB_H_M_EMP ");
            sb.Append(" where EMP_ID = @EMP_ID");
            ht.Add("@EMP_ID", EMP_ID);
            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getDUP_SALARY()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select a.EMP_ID");
            sb.Append(" from TB_H_M_EMP a,TB_H_R_EMP_DATA b ");
            sb.Append(" where a.EMP_ID = b.EMP_ID and SALARY_ACCOUNT_BANK = @ACCOUNT_BANK and SALARY_ACCOUNT_NO = @SALARY_ACCOUNT_NO and b.EMP_CHG_CD != '99'");
            ht.Add("@ACCOUNT_BANK", ACCOUNT_BANK);
            ht.Add("@SALARY_ACCOUNT_NO", SALARY_ACCOUNT_NO);
            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getOTH1_CONTRACT_MONTHS()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select CODE_VAL1 from TB_9_M_PARAMETER");
            sb.Append(" where SYS_CD = 'HB' and MAIN_CD = 'OTH1_CONTRACT_MONTHS'");
            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getW_OTH1_CONTRACT_EDT()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select CODE_VAL1 from TB_9_M_PARAMETER");
            sb.Append(" where SYS_CD = 'HB' and MAIN_CD = 'W_OTH1_CONTRACT_EDT'");
            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getKZ_CONTRACT_MONTHS()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select CODE_VAL1 from TB_9_M_PARAMETER");
            sb.Append(" where SYS_CD = 'HB' and MAIN_CD = 'KZ_CONTRACT_MONTHS'");
            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getFilePath()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select CODE_VAL1 from TB_9_M_PARAMETER where TB_9_M_PARAMETER.SYS_CD = 'HB' and TB_9_M_PARAMETER.MAIN_CD = 'PHOTO_PATH'");

            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getSalary_Account_Bank_Name(string Salary_Account_Bank)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select SUB_DESC from TB_9_M_COMM_D where MAIN_CD = 'SALARY_ACCOUNT_BANK' and SUB_CD = @SUB_CD ");
            ht.Add("@SUB_CD", Salary_Account_Bank);
            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public string SALARY_ACCOUNT_BANK { get; set; }

    internal DataTable getDEPT_DATA(string dept_no)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select DEPT_NAME from VW_H_DEPT_DATA where DEPT_NO = @DEPT_NO");
            ht.Add("@DEPT_NO", dept_no);
            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getPJOB_DATA(string pjob_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select PJOB_DESC from TB_H_M_PJOB where PJOB_CD = @PJOB_CD");
            ht.Add("@PJOB_CD", pjob_cd);
            return dbConn.Query(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

}