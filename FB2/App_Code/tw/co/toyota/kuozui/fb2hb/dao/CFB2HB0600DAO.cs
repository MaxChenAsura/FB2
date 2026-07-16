using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;
using System.Data.OleDb;
using System.Data.Odbc;

/// <summary>
/// CFB2HB0600DAO 的摘要描述
/// </summary>
public class CFB2HB0600DAO : BaseDAO
{
    public CFB2HB0600DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    //Qry Gridview 查詢資料
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string emp_id, string dept_no, string pjob_cd,
                            string level_cd_s, string level_cd_e, string is_super, string is_dept, string departments, string emp_name, string emp_chg_cd)
    {
        try
        {
            if (sortExpression.Contains("EMP_ID"))
                sortExpression = sortExpression.Replace("EMP_ID", "a.EMP_ID");
            if (sortExpression.Contains("EMP_NAME"))
                sortExpression = sortExpression.Replace("EMP_NAME", "a.EMP_NAME");
            if (sortExpression.Contains("DEPT_NO"))
                sortExpression = sortExpression.Replace("DEPT_NO", "a.DEPT_NO");
            if (sortExpression.Contains("WS_CD"))
                sortExpression = sortExpression.Replace("WS_CD", "a.WS_CD");
            if (sortExpression.Contains("LEVEL_CD"))
                sortExpression = sortExpression.Replace("LEVEL_CD", "a.LEVEL_CD");
            if (sortExpression.Contains("PJOB_CD"))
                sortExpression = sortExpression.Replace("PJOB_CD", "a.PJOB_CD");
            if (sortExpression.Contains("AGE"))
                sortExpression = sortExpression.Replace("AGE", "a.AGE");
            if (sortExpression.Contains("WORK_YEARS"))
                sortExpression = sortExpression.Replace("WORK_YEARS", "a.WORK_YEARS");
            if (sortExpression.Contains("RECENT_LEVEL_WORK_DAYS"))
                sortExpression = sortExpression.Replace("RECENT_LEVEL_WORK_DAYS", "a.RECENT_LEVEL_WORK_DAYS");
            if (sortExpression.Contains("EMP_CHG_CD"))
                sortExpression = sortExpression.Replace("EMP_CHG_CD", "a.EMP_CHG_CD");
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

         
            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" a.EMP_ID,a.EMP_NAME,a.DEPT_NO,a.DEPT_NO + ' ' + a.DEPT_FULL_NAME DEPT_NAME,a.WS_CD,a.LEVEL_CD + isnull(a.GRADE_CD,'') as LEVEL_CD,");
            sb.Append(" a.PJOB_CD + ' ' + a.PJOB_DESC PJOB_DESC,a.PJOB_CD,a.AGE,a.LEVEL_CD as LEVEL_CD_O ");
           // sb.Append(" case a.WORK_YEARS when 0 then a.SERVICE_YEARS else a.WORK_YEARS end as WORK_YEARS,"); BY EVA MARK 2015/3/23 惠菁說:外調履歷之work_year 以服務年資為主
            sb.Append(" , a.SERVICE_YEARS as WORK_YEARS,"); //BY EVAR 2015/3/23 update
            sb.Append(" convert(varchar(50),convert(float(50),round(a.RECENT_LEVEL_WORK_DAYS / 365,2))) RECENT_LEVEL_WORK_DAYS,");
            sb.Append(" a.EMP_CHG_CD,a.EMP_CHG_DESC");
            sb.Append(" from VW_H_EMP_DATA a");
            sb.Append(" left join TB_H_M_EMP b on b.EMP_ID = a.EMP_ID");
            sb.Append(" left join TB_H_R_HEAD_DEPT c on a.EMP_ID=c.EMP_ID and a.DEPT_NO=c.MNG_DEPT_NO ");
            sb.Append(" where 1=1 ");

            if (emp_id != "")
            {
                sb.Append(" and a.EMP_ID like @EMP_ID ");
                ht.Add("@EMP_ID", emp_id + "%");
            }
            if (emp_name != "")
            {
                sb.Append(" and a.EMP_NAME like @EMP_NAME ");
                ht.Add("@EMP_NAME", emp_name + "%");
            }
            if (emp_chg_cd != "-1")
            {
                sb.Append(" and a.EMP_CHG_CD = @EMP_CHG_CD");
                ht.Add("@EMP_CHG_CD", emp_chg_cd);
            }

            if (dept_no != "")
            {
                sb.Append(" and a.DEPT_NO like @DEPT_NO ");
                ht.Add("@DEPT_NO", dept_no + "%");
            }
            if (pjob_cd != "")
            {
                sb.Append(" and a.PJOB_CD like @PJOB_CD ");
                ht.Add("@PJOB_CD", pjob_cd + "%");
            }

            if (level_cd_s != "-1")
            {
                sb.Append(" and a.LEVEL_CD >= @LEVEL_CD_S");
                ht.Add("@LEVEL_CD_S", level_cd_s);
            }
            if (level_cd_e != "-1")
            {
                sb.Append(" and a.LEVEL_CD <= @LEVEL_CD_E");
                ht.Add("@LEVEL_CD_E", level_cd_e);
            }
            //顯示資料權限設定
            if (SessionHandle.Current.is_super != "Y")
            {
                sb.Append(@" AND a.EMP_ID IN(	 select EMP_ID from dbo.FN_H_GET_AUTH_EMP(@loginID,@departments)  )");
                ht.Add("@loginID", SessionHandle.Current.emp_id);
                ht.Add("@departments", SessionHandle.Current.departments);
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
    //Qry Gridview 查詢總筆數
    public int getCount(int startRowIndex, int maximumRows, string emp_id, string dept_no, string pjob_cd,
                            string level_cd_s, string level_cd_e, string is_super, string is_dept, string departments, string emp_name, string emp_chg_cd)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            
            sb.Append("Select COUNT(*) total_record ");
            sb.Append(" from VW_H_EMP_DATA a");
            sb.Append(" left join TB_H_M_EMP b on b.EMP_ID = a.EMP_ID");
            sb.Append(" left join TB_H_R_HEAD_DEPT c on a.EMP_ID=c.EMP_ID and a.DEPT_NO=c.MNG_DEPT_NO ");
            sb.Append(" where 1=1 ");

            
            if (emp_id != "")
            {
                sb.Append(" and a.EMP_ID like @EMP_ID ");
                ht.Add("@EMP_ID", emp_id + "%");
            }
            if (emp_name != "")
            {
                sb.Append(" and a.EMP_NAME like @EMP_NAME ");
                ht.Add("@EMP_NAME", emp_name + "%");
            }
            if (emp_chg_cd != "-1")
            {
                sb.Append(" and a.EMP_CHG_CD = @EMP_CHG_CD");
                ht.Add("@EMP_CHG_CD", emp_chg_cd);
            }
            if (dept_no != "")
            {
                sb.Append(" and a.DEPT_NO like @DEPT_NO ");
                ht.Add("@DEPT_NO", dept_no + "%");
            }
            if (pjob_cd != "")
            {
                sb.Append(" and a.PJOB_CD like @PJOB_CD ");
                ht.Add("@PJOB_CD", pjob_cd + "%");
            }

            if (level_cd_s != "-1")
            {
                sb.Append(" and a.LEVEL_CD >= @LEVEL_CD_S");
                ht.Add("@LEVEL_CD_S", level_cd_s);
            }
            if (level_cd_e != "-1")
            {
                sb.Append(" and a.LEVEL_CD <= @LEVEL_CD_E");
                ht.Add("@LEVEL_CD_E", level_cd_e);
            }

            //顯示資料權限設定
            if (SessionHandle.Current.is_super != "Y")
            {
                sb.Append(@" AND a.EMP_ID IN(	 select EMP_ID from dbo.FN_H_GET_AUTH_EMP(@loginID,@departments)  )");
                ht.Add("@loginID", SessionHandle.Current.emp_id);
                ht.Add("@departments", SessionHandle.Current.departments);
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

    internal DataTable getDefaultData(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select EMP_ID,EMP_NAME,EMP_CHG_DESC,LEVEL_CD+ isnull(GRADE_CD,'') as LEVEL_CD,PJOB_CD +'-'+PJOB_DESC as PJOB_DESC,PLANT_NAME,DEPT_NO + '-' + DEPT_FULL_NAME DEPT_NAME,");
            sb.Append(" REPLACE(CONVERT(char(10), JOIN_DT, 120),'-','/') JOIN_DT,WS_DESC,WORK_YEARS,MODEL_YEAR,");
            sb.Append(" a.SEX_CD+ '-' + b.SUB_DESC SEX_CD , ");
            sb.Append(" REPLACE(CONVERT(char(10), BIRTH_DT, 120),'-','/') BIRTH_DT,dbo.FN_S_BASE_SALARY(@EMP_ID,IIF(LEAVE_DT=null,null,DATEADD(DAY,-1,LEAVE_DT)))  BASE_SALARY,");
            sb.Append(" convert(varchar(50),convert(float(50),round(RECENT_LEVEL_WORK_DAYS / 365,2))) RECENT_LEVEL_WORK_DAYS,AGE,");
            sb.Append(" URGENT_CONTACT_NAME,URGENT_CONTACT_TEL,URGENT_CONTACT_RELATION,REGISTER_ADDR,REGISTER_TEL,CONTACT_ADDR,");
            sb.Append(" CONTACT_TEL,MOBILE_TEL_1,MOBILE_TEL_2,");
            sb.Append(" PERSONAL_EMAIL,COMPANY_EMAIL,");
            sb.Append(" (select CODE_VAL1+ @EMP_ID + '.jpg'  from TB_9_M_PARAMETER where TB_9_M_PARAMETER.SYS_CD = 'HB' and TB_9_M_PARAMETER.MAIN_CD = 'PHOTO_PATH') PHOTO_PATH");
            sb.Append(" ,(select CODE_VAL1+ 'kuozui' + '.jpg'  from TB_9_M_PARAMETER where TB_9_M_PARAMETER.SYS_CD = 'HB' and TB_9_M_PARAMETER.MAIN_CD = 'PHOTO_PATH') PHOTO_PATH_KUOZUI");
            sb.Append(" ,b.SUB_DESC SEX_DESC  ");
            sb.Append(" from VW_H_EMP_DATA a ");
            sb.Append(" left join TB_9_M_COMM_D b on  a.SEX_CD = b.SUB_CD and b.MAIN_CD = 'SEX_CD'  and b.IS_VALID='Y'  and b.SYS_CD='HB'  ");
            sb.Append(" where EMP_ID = @EMP_ID ");
            ht.Add("@EMP_ID", emp_id);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //連線至Oracle 讀取 課程對象名單報表檔 取得日文或toeic成績
    public DataTable getTOTAL_SCORE(string emp_id)
    {
       
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            /*
            sb.Append(" select ");
            sb.Append(" isnull( (select TOP(1) isnull(TOTAL_SCORE,'') from " + utilities.ORACLEName + "..TFB1U001.TB_M_ACT_COURSE_MEM ");
            sb.Append("  where WORK_ID = @EMP_ID and COURSE_ID = 'QL002-001' order by UPDATED_DT desc ) ,'')  TOTAL_SCORE_JPN ");
            sb.Append("  ,isnull( (select isnull(max(TOTAL_SCORE),'') from " + utilities.ORACLEName + "..TFB1U001.TB_M_ACT_COURSE_MEM  ");
            sb.Append(" where WORK_ID = @EMP_ID and COURSE_ID = 'QL001-001'   ),'') TOTAL_SCORE_TOEIC ");
            ht.Add("@EMP_ID", emp_id);
            */

            sb.Append(" select * from TB_H_R_EMP_LANGUAGE ");
            sb.Append(" where EMP_ID =@EMP_ID ");
            ht.Add("@EMP_ID", emp_id);
            return dbConn.Query(sb, ht);

        }
        catch(Exception ex)
        {
            //throw;
            return new DataTable();
        }
    }


    //技能 Gridview 查詢資料
    public DataTable getSkillData(int startRowIndex, int maximumRows, string sortExpression, string emp_id)
    {
        try
        {
            if (sortExpression.Contains("EMP_ID"))
            {
                sortExpression = sortExpression.Replace("EMP_ID", "a.EMP_ID");
            }

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" a.EMP_ID,b.EMP_NAME,a.ORI_DEPT_NO,a.ORI_DEPT_NO + ' ' + a.ORI_DEPT_NAME_20 ORI_DEPT_FULL_NAME,");
            sb.Append(" a.SKILL_TYPE,a.SKILL_TYPE + '-' + c.SUB_DESC SKILL_TYPE_NAME,a.SKILL_DESC,a.SKILL_GRADE,a.SKILL_ORG,");
            sb.Append(" REPLACE(CONVERT(char(10), a.AWARD_DT, 120),'-','/') AWARD_DT");
            sb.Append(" from TB_H_M_EMP_SKILL a ");
            sb.Append(" left join TB_H_M_EMP b on a.EMP_ID = b.EMP_ID ");
            sb.Append(" left join TB_9_M_COMM_D c on a.SKILL_TYPE = c.SUB_CD and c.MAIN_CD = 'SKILL_TYPE' and c.SYS_CD = 'HB' ");
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
    //技能 Gridview 查詢總筆數
    public int getSkillCount(int startRowIndex, int maximumRows, string emp_id)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select COUNT(*) total_record ");
            sb.Append(" from TB_H_M_EMP_SKILL a ");
            sb.Append(" left join TB_H_M_EMP b on a.EMP_ID = b.EMP_ID ");
            sb.Append(" left join TB_9_M_COMM_D c on a.SKILL_TYPE = c.SUB_CD and c.MAIN_CD = 'SKILL_TYPE' and c.SYS_CD = 'HB' ");
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


    //員工國外研修 Gridview 查詢資料
    public DataTable getTrainData(int startRowIndex, int maximumRows, string sortExpression, string emp_id)
    {
        try
        {
            if (sortExpression.Contains("EMP_ID"))
            {
                sortExpression = sortExpression.Replace("EMP_ID", "a.EMP_ID");
            }

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" a.EMP_ID,b.EMP_NAME,a.ORI_DEPT_NO,a.ORI_DEPT_NO + ' ' + a.ORI_DEPT_NAME_20 ORI_DEPT_FULL_NAME,");
            sb.Append(" a.TRAINING_COMPANY,a.TRAINING_GOAL,");
            sb.Append(" REPLACE(CONVERT(char(10), a.START_DT, 120),'-','/') START_DT,");
            sb.Append(" REPLACE(CONVERT(char(10), a.END_DT, 120),'-','/') END_DT");
            sb.Append(" from TB_H_M_EMP_TRAINING a");
            sb.Append(" left join TB_H_M_EMP b on a.EMP_ID = b.EMP_ID ");
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
    //員工國外研修 Gridview 查詢總筆數
    public int getTrainCount(int startRowIndex, int maximumRows, string emp_id)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select COUNT(*) total_record ");
            sb.Append(" from TB_H_M_EMP_TRAINING a");
            sb.Append(" left join TB_H_M_EMP b on a.EMP_ID = b.EMP_ID ");
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


    //員工外調履歷檔 Gridview 查詢資料
    public DataTable getTransData(int startRowIndex, int maximumRows, string sortExpression, string emp_id)
    {
        try
        {
            if (sortExpression.Contains("EMP_ID"))
            {
                sortExpression = sortExpression.Replace("EMP_ID", "a.EMP_ID");
            }
            if (sortExpression.Contains("EMP_NAME"))
            {
                sortExpression = sortExpression.Replace("EMP_NAME", "d.EMP_NAME");
            }
            if (sortExpression.Contains("TRANSFER_COMPANY_CD"))
            {
                sortExpression = sortExpression.Replace("TRANSFER_COMPANY_CD", "a.TRANSFER_COMPANY_CD");
            }
            if (sortExpression.Contains("TRANSFER_NATION_CD"))
            {
                sortExpression = sortExpression.Replace("TRANSFER_NATION_CD", "a.TRANSFER_NATION_CD");
            }
            if (sortExpression.Contains("TRANSFER_DEPT"))
            {
                sortExpression = sortExpression.Replace("TRANSFER_DEPT", "a.TRANSFER_DEPT");
            }
            if (sortExpression.Contains("START_DT"))
            {
                sortExpression = sortExpression.Replace("START_DT", "a.START_DT");
            }
            if (sortExpression.Contains("HR_CHG_NO"))
            {
                sortExpression = sortExpression.Replace("HR_CHG_NO", "a.HR_CHG_NO");
            }
            if (sortExpression.Contains("ICT_TYPE"))
            {
                sortExpression = sortExpression.Replace("ICT_TYPE", "a.ICT_TYPE");
            }
            if (sortExpression.Contains("HR_CHG_CD"))
            {
                sortExpression = sortExpression.Replace("HR_CHG_CD", "a.TRANSFER_REASON");
            }

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" a.TRANSFER_REASON HR_CHG_CD,b.HR_CHG_DESC,a.ICT_TYPE,a.EMP_ID,d.EMP_NAME,c.ORI_DEPT_NAME_20,c.ORI_LEVEL_CD,");
            sb.Append(" d.DEPT_NAME_20,a.TRANSFER_NATION_CD,e.SUB_DESC TRANSFER_NATION,a.TRANSFER_COMPANY_CD,");
            sb.Append(" case a.TRANSFER_REASON when 'B07' then (select SUB_DESC from TB_9_M_COMM_D where TB_9_M_COMM_D.MAIN_CD = 'SUPPORT_COMPANY_CD' and TB_9_M_COMM_D.SUB_CD = a.TRANSFER_COMPANY_CD)");
            sb.Append(" when 'B08' then (select SUB_DESC from TB_9_M_COMM_D where TB_9_M_COMM_D.MAIN_CD = 'GCC_COMPANY_CD' and TB_9_M_COMM_D.SUB_CD = a.TRANSFER_COMPANY_CD)");
            sb.Append(" when 'B09' then (select SUB_DESC from TB_9_M_COMM_D where TB_9_M_COMM_D.MAIN_CD = 'ICT_COMPANY_CD' and TB_9_M_COMM_D.SUB_CD = a.TRANSFER_COMPANY_CD)");
            sb.Append(" END as TRANSFER_COMPANY,a.TRANSFER_DEPT,");
            sb.Append(" REPLACE(CONVERT(char(10), a.START_DT, 120),'-','/') START_DT,");
            sb.Append(" REPLACE(CONVERT(char(10), a.PLAN_END_DT, 120),'-','/') PLAN_END_DT,");
            sb.Append(" REPLACE(CONVERT(char(10), a.END_DT, 120),'-','/') END_DT,a.HR_CHG_NO");
            sb.Append(" from TB_H_R_EMP_TRANSFER a");
            sb.Append(" left join TB_H_M_HR_CHANGE_CODE b on a.TRANSFER_REASON = b.HR_CHG_CD ");
            sb.Append(" left join TB_H_M_EMP_HR_CHANGE_H c on a.HR_CHG_NO = c.HR_CHG_NO and a.EMP_ID = c.EMP_ID ");
            sb.Append(" left join VW_H_EMP_DATA d on a.EMP_ID = d.EMP_ID ");
            sb.Append(" left join TB_9_M_COMM_D e on a.TRANSFER_NATION_CD = e.SUB_CD and e.MAIN_CD = 'NATION_CD' ");
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
    //員工外調履歷檔 Gridview 查詢總筆數
    public int getTransCount(int startRowIndex, int maximumRows, string emp_id)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select COUNT(*) total_record ");
            sb.Append(" from TB_H_R_EMP_TRANSFER a");
            sb.Append(" left join TB_H_M_HR_CHANGE_CODE b on a.TRANSFER_REASON = b.HR_CHG_CD ");
            sb.Append(" left join TB_H_M_EMP_HR_CHANGE_H c on a.HR_CHG_NO = c.HR_CHG_NO and a.EMP_ID = c.EMP_ID ");
            sb.Append(" left join VW_H_EMP_DATA d on a.EMP_ID = d.EMP_ID ");
            sb.Append(" left join TB_9_M_COMM_D e on a.TRANSFER_NATION_CD = e.SUB_CD and e.MAIN_CD = 'NATION_CD' ");
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


    //兼任履歷 Gridview 查詢資料
    public DataTable getOtherJobData(int startRowIndex, int maximumRows, string sortExpression, string emp_id)
    {
        try
        {

            if (sortExpression.Contains("START_DT"))
            {
                sortExpression = sortExpression.Replace("START_DT", "a.START_DT");
            }


            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" a.HR_CHG_NO,a.EMP_ID,OTHER_DEPT_NO,OTHER_DEPT_NAME,OTHER_PJOB_CD,OTHER_PJOB_DESC,");
            sb.Append(" ORI_DIV_DEPT_FULL_NAME,ORI_DEPT_NO,");
            sb.Append(" REPLACE(CONVERT(char(10), a.START_DT, 120),'-','/') START_DT,");
            sb.Append(" REPLACE(CONVERT(char(10), a.END_DT, 120),'-','/') END_DT");
            sb.Append(" from TB_H_R_EMP_OTHER_PJOB a");
            sb.Append(" left join TB_H_M_EMP_HR_CHANGE_H b on a.HR_CHG_NO = b.HR_CHG_NO and a.EMP_ID = b.EMP_ID");
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
    //兼任履歷 Gridview 查詢總筆數
    public int getOtherJobTrainCount(int startRowIndex, int maximumRows, string emp_id)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select COUNT(*) total_record ");
            sb.Append(" from TB_H_R_EMP_OTHER_PJOB a");
            sb.Append(" left join TB_H_M_EMP_HR_CHANGE_H b on a.HR_CHG_NO = b.HR_CHG_NO and a.EMP_ID = b.EMP_ID");
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

    //員工學歷 Gridview 查詢資料
    public DataTable getEduData(int startRowIndex, int maximumRows, string sortExpression, string emp_id)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" a.EMP_ID,EDUCATION_CD,SCHOOL_NATION_CD,b.SUB_DESC SCHOOL_NATION_DESC,c.SUB_DESC EDUCATION_DESC, ");
            sb.Append(" SCHOOL_NAME,DEPARTMENT_NAME,GRADUATION_YEAR");

            sb.Append(" from TB_H_M_EMP_EDUCATION a");
            sb.Append(" left join TB_9_M_COMM_D b on a.SCHOOL_NATION_CD = b.SUB_CD and b.MAIN_CD = 'NATION_CD' ");
            sb.Append(" left join TB_9_M_COMM_D c on a.EDUCATION_CD = c.SUB_CD and c.MAIN_CD = 'EDUCATION_CD' ");
            sb.Append(" where 1=1 ");
            sb.Append(" and a.IS_VIRTUAL_SCHOOL != 'Y' ");
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
    //員工學歷 Gridview 查詢總筆數
    public int getEduCount(int startRowIndex, int maximumRows, string emp_id)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select COUNT(*) total_record ");
            sb.Append(" from TB_H_M_EMP_EDUCATION a");
            sb.Append(" left join TB_9_M_COMM_D b on a.SCHOOL_NATION_CD = b.SUB_CD and b.MAIN_CD = 'NATION_CD' ");
            sb.Append(" left join TB_9_M_COMM_D c on a.EDUCATION_CD = c.SUB_CD and c.MAIN_CD = 'EDUCATION_CD' ");
            sb.Append(" where 1=1 ");
            sb.Append(" and a.IS_VIRTUAL_SCHOOL != 'Y' ");
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

    //家庭成員 Gridview 查詢資料
    public DataTable getFamData(int startRowIndex, int maximumRows, string sortExpression, string emp_id)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" a.EMP_ID,FAMILY_RELATION,b.SUB_DESC FAMILY_RELATION_DESC,FAMILY_NAME,REPLACE(CONVERT(char(10), a.FAMILY_BIRTH_DT, 120),'-','/') FAMILY_BIRTH_DT, ");
            sb.Append(" FAMILY_LICENSE_ID,FAMILY_WORK_DESC");

            sb.Append(" from TB_H_M_EMP_FAMILY a ");
            sb.Append(" left join TB_9_M_COMM_D b on a.FAMILY_RELATION = b.SUB_CD and b.MAIN_CD = 'FAMILY_RELATION' ");
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
    //家庭成員 Gridview 查詢總筆數
    public int getFamCount(int startRowIndex, int maximumRows, string emp_id)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select COUNT(*) total_record ");
            sb.Append(" from TB_H_M_EMP_FAMILY a ");
            sb.Append(" left join TB_9_M_COMM_D b on a.FAMILY_RELATION = b.SUB_CD and b.MAIN_CD = 'FAMILY_RELATION' ");
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

    //人事履歷 Gridview 查詢資料
    public DataTable getChgData(int startRowIndex, int maximumRows, string sortExpression, string emp_id)
    {
        try
        {

            if (sortExpression.Contains("HR_CHG_DESC"))
            {
                sortExpression = sortExpression.Replace("HR_CHG_DESC", "a.HR_CHG_DESC");
            }

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" a.EMP_ID,a.HR_CHG_DESC,a.DEPT_FULL_NAME ,LEVEL_CD+ isnull(GRADE_CD,'') as LEVEL_CD,REPLACE(CONVERT(char(10), a.START_DT, 120),'-','/') START_DT,PJOB_DESC ");
            sb.Append(" from TB_H_R_EMP_HR_CHG_RECORD a ");
            sb.Append(" left join TB_H_M_HR_CHANGE_CODE b on a.HR_CHG_CD = b.HR_CHG_CD and b.IS_SHOW  = 'Y' ");
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
    //人事履歷 Gridview 查詢總筆數
    public int getChgCount(int startRowIndex, int maximumRows, string emp_id)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select COUNT(*) total_record ");
            sb.Append(" from TB_H_R_EMP_HR_CHG_RECORD a ");
            sb.Append(" left join TB_H_M_HR_CHANGE_CODE b on a.HR_CHG_CD = b.HR_CHG_CD and b.IS_SHOW  = 'Y' ");
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

    internal DataTable getEduData(string emp_id)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY a.GRADUATION_YEAR desc ) As RowNumber,");
            sb.Append(" a.EMP_ID,EDUCATION_CD,SCHOOL_NATION_CD,b.SUB_DESC SCHOOL_NATION_DESC,c.SUB_DESC EDUCATION_DESC, ");
            sb.Append(" SCHOOL_NAME,DEPARTMENT_NAME,GRADUATION_YEAR");

            sb.Append(" from TB_H_M_EMP_EDUCATION a ");
            sb.Append(" left join TB_9_M_COMM_D b on a.SCHOOL_NATION_CD = b.SUB_CD and b.MAIN_CD = 'NATION_CD' ");
            sb.Append(" left join TB_9_M_COMM_D c on a.EDUCATION_CD = c.SUB_CD and c.MAIN_CD = 'EDUCATION_CD' ");
            sb.Append(" where  1=1 ");
            sb.Append(" and a.IS_VIRTUAL_SCHOOL != 'Y' ");
            if (emp_id != "")
            {
                sb.Append(" and a.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
            }


            sb.Append(" )god_data ");
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    internal DataTable getfamData(string emp_id)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY FAMILY_RELATION ) As RowNumber,");
            sb.Append(" a.EMP_ID,FAMILY_RELATION,b.SUB_DESC FAMILY_RELATION_DESC,FAMILY_NAME,REPLACE(CONVERT(char(10), a.FAMILY_BIRTH_DT, 120),'-','/') FAMILY_BIRTH_DT, ");
            sb.Append(" FAMILY_LICENSE_ID,FAMILY_WORK_DESC");

            sb.Append(" from TB_H_M_EMP_FAMILY a ");
            sb.Append(" left join TB_9_M_COMM_D b on a.FAMILY_RELATION = b.SUB_CD and b.MAIN_CD = 'FAMILY_RELATION' ");
            sb.Append(" where 1=1 ");
            if (emp_id != "")
            {
                sb.Append(" and a.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
            }


            sb.Append(" )god_data  ");

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    internal DataTable getChgData(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY a.START_DT desc ) As RowNumber,");
            sb.Append(" a.EMP_ID,a.HR_CHG_DESC,a.DEPT_FULL_NAME ,LEVEL_CD+ isnull(GRADE_CD,'') as LEVEL_CD,REPLACE(CONVERT(char(10), a.START_DT, 120),'-','/') START_DT,PJOB_DESC ");
            sb.Append(" from TB_H_R_EMP_HR_CHG_RECORD a ");
            sb.Append(" left join TB_H_M_HR_CHANGE_CODE b on a.HR_CHG_CD = b.HR_CHG_CD  and b.IS_SHOW  = 'Y'");
            sb.Append(" where 1=1 ");
            if (emp_id != "")
            {
                sb.Append(" and a.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
            }

            sb.Append(" )god_data ");
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    internal DataTable getTransData(string emp_id)
    {
        try
        {


            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY a.TRANSFER_REASON ) As RowNumber,");
            sb.Append(" a.TRANSFER_REASON HR_CHG_CD,b.HR_CHG_DESC,a.ICT_TYPE,a.EMP_ID,d.EMP_NAME,c.ORI_DEPT_NAME_20,c.ORI_LEVEL_CD,");
            sb.Append(" d.DEPT_NAME_20,a.TRANSFER_NATION_CD,e.SUB_DESC TRANSFER_NATION,a.TRANSFER_COMPANY_CD,");
            sb.Append(" case a.TRANSFER_REASON when 'B07' then (select SUB_DESC from TB_9_M_COMM_D where TB_9_M_COMM_D.MAIN_CD = 'SUPPORT_COMPANY_CD' and TB_9_M_COMM_D.SUB_CD = a.TRANSFER_COMPANY_CD)");
            sb.Append(" when 'B08' then (select SUB_DESC from TB_9_M_COMM_D where TB_9_M_COMM_D.MAIN_CD = 'GCC_COMPANY_CD' and TB_9_M_COMM_D.SUB_CD = a.TRANSFER_COMPANY_CD)");
            sb.Append(" when 'B09' then (select SUB_DESC from TB_9_M_COMM_D where TB_9_M_COMM_D.MAIN_CD = 'ICT_COMPANY_CD' and TB_9_M_COMM_D.SUB_CD = a.TRANSFER_COMPANY_CD)");
            sb.Append(" END as TRANSFER_COMPANY,a.TRANSFER_DEPT,");
            sb.Append(" REPLACE(CONVERT(char(10), a.START_DT, 120),'-','/') START_DT,");
            sb.Append(" REPLACE(CONVERT(char(10), a.PLAN_END_DT, 120),'-','/') PLAN_END_DT,");
            sb.Append(" REPLACE(CONVERT(char(10), a.END_DT, 120),'-','/') END_DT,a.HR_CHG_NO");
            sb.Append(" from TB_H_R_EMP_TRANSFER a");
            sb.Append(" left join TB_H_M_HR_CHANGE_CODE b on a.TRANSFER_REASON = b.HR_CHG_CD ");
            sb.Append(" left join TB_H_M_EMP_HR_CHANGE_H c on a.HR_CHG_NO = c.HR_CHG_NO and a.EMP_ID = c.EMP_ID ");
            sb.Append(" left join VW_H_EMP_DATA d on a.EMP_ID = d.EMP_ID ");
            sb.Append(" left join TB_9_M_COMM_D e on a.TRANSFER_NATION_CD = e.SUB_CD and e.MAIN_CD = 'NATION_CD' ");
            sb.Append(" where 1=1 ");
            if (emp_id != "")
            {
                sb.Append(" and a.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
            }

            sb.Append(" )god_data ");

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    internal DataTable getTrainData(string emp_id)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY a.START_DT desc ) As RowNumber,");
            sb.Append(" a.EMP_ID,b.EMP_NAME,a.ORI_DEPT_NO,a.ORI_DEPT_NO + ' ' + a.ORI_DEPT_NAME_20 ORI_DEPT_FULL_NAME,");
            sb.Append(" a.TRAINING_COMPANY,a.TRAINING_GOAL,");
            sb.Append(" REPLACE(CONVERT(char(10), a.START_DT, 120),'-','/') START_DT,");
            sb.Append(" REPLACE(CONVERT(char(10), a.END_DT, 120),'-','/') END_DT");
            sb.Append(" ,a.ORI_DEPT_NAME_20 ORI_DEPT_FULL_NAME_2 ");
            sb.Append(" from TB_H_M_EMP_TRAINING a");
            sb.Append(" left join TB_H_M_EMP b on a.EMP_ID = b.EMP_ID ");
            sb.Append(" where 1=1 ");
            if (emp_id != "")
            {
                sb.Append(" and a.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
            }

            sb.Append(" )god_data ");

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    internal DataTable getOtherJobData(string emp_id)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY a.START_DT desc ) As RowNumber,");
            sb.Append(" a.HR_CHG_NO,a.EMP_ID,OTHER_DEPT_NO,OTHER_DEPT_NAME,OTHER_PJOB_CD,OTHER_PJOB_DESC,");
            sb.Append(" ORI_DIV_DEPT_FULL_NAME,ORI_DEPT_NO,");
            sb.Append(" REPLACE(CONVERT(char(10), a.START_DT, 120),'-','/') START_DT,");
            sb.Append(" REPLACE(CONVERT(char(10), a.END_DT, 120),'-','/') END_DT");
            sb.Append(" from TB_H_R_EMP_OTHER_PJOB a");
            sb.Append(" left join TB_H_M_EMP_HR_CHANGE_H b on a.HR_CHG_NO = b.HR_CHG_NO and a.EMP_ID = b.EMP_ID");
            sb.Append(" where 1=1 ");
            if (emp_id != "")
            {
                sb.Append(" and a.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
            }



            sb.Append(" )god_data");

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    //考績
    internal DataTable getAssessData(string emp_id)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" select top 5 ASSESS_YEAR");
            sb.Append(@" ,SCORE_1H =  case 
                         when LEVEL_FLAG_1H='V' and SCORE_1H='D' and ASSESS_YEAR<2017  then SCORE_1H+'*' 
                         when LEVEL_FLAG_1H='V' and SCORE_1H='B' and ASSESS_YEAR>=2017  then SCORE_1H+'*' 
                         ELSE SCORE_1H  
                         end 
                         ,SCORE_2H =  case 
                         when LEVEL_FLAG_2H='V' and SCORE_2H='D' and ASSESS_YEAR<2017 then SCORE_2H+'*' 
                         when LEVEL_FLAG_2H='V' and SCORE_2H='B' and ASSESS_YEAR>=2017 then SCORE_2H+'*' 
                         ELSE SCORE_2H  
                         end");

            sb.Append(" from TB_S_M_ASSESS");
            sb.Append(" where ASSESS_YEAR <= YEAR(GETDATE()) and EMP_ID = @EMP_ID");
            sb.Append(" order by ASSESS_YEAR desc");
            ht.Add("@EMP_ID", emp_id);


            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    internal DataTable getEmpName(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select EMP_ID,EMP_NAME ");
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

    internal DataTable getDEPT_NAME(string dept_no)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select DEPT_NO,DEPT_NAME ");
            sb.Append(" from VW_H_DEPT_DATA ");
            sb.Append(" where DEPT_NO=@DEPT_NO ");
            ht.Add("@DEPT_NO", dept_no);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getPJOB_DESC(string pjob_cd)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select PJOB_CD,PJOB_DESC ");
            sb.Append(" from TB_H_M_PJOB ");
            sb.Append(" where PJOB_CD=@PJOB_CD ");
            ht.Add("@PJOB_CD", pjob_cd);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
}