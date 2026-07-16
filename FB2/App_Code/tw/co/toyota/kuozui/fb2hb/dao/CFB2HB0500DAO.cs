using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;

/// <summary>
/// CFB2HB0500DAO 的摘要描述
/// </summary>
public class CFB2HB0500DAO : BaseDAO
{
    public string EMP_ID { get; set; }

    public string SKILL_TYPE { get; set; }

    public string SKILL_DESC { get; set; }

    public string SKILL_GRADE { get; set; }

    public string SKILL_ORG { get; set; }

    public string AWARD_DT { get; set; }

    public string CREATED_BY { get; set; }

    public string UPDATED_BY { get; set; }

    public string FUNC_ID { get; set; }

    public CFB2HB0500DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    //Gridview 查詢資料
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string emp_id, string skill_type, string skill_grade)
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
            sb.Append(" from TB_H_M_EMP_SKILL a,TB_H_M_EMP b,TB_9_M_COMM_D c");
            sb.Append(" where a.EMP_ID = b.EMP_ID");
            sb.Append(" and a.SKILL_TYPE = c.SUB_CD");
            sb.Append(" and c.MAIN_CD = 'SKILL_TYPE' and c.SYS_CD = 'HB'");
            if (emp_id != "")
            {
                sb.Append(" and a.EMP_ID like @EMP_ID ");
                ht.Add("@EMP_ID", emp_id + "%");
            }
            if (skill_type != "-1" && skill_type != "")
            {
                sb.Append(" and a.SKILL_TYPE = @SKILL_TYPE ");
                ht.Add("@SKILL_TYPE", skill_type);
            }
            if (skill_grade != "-1" && skill_grade != "")
            {
                sb.Append(" and a.SKILL_GRADE = @SKILL_GRADE ");
                ht.Add("@SKILL_GRADE", skill_grade);
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
    public int getCount(int startRowIndex, int maximumRows, string emp_id, string skill_type, string skill_grade)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select COUNT(*) total_record ");
            sb.Append(" from TB_H_M_EMP_SKILL a,TB_H_M_EMP b,TB_9_M_COMM_D c");
            sb.Append(" where a.EMP_ID = b.EMP_ID");
            sb.Append(" and a.SKILL_TYPE = c.SUB_CD");
            sb.Append(" and c.MAIN_CD = 'SKILL_TYPE' and c.SYS_CD = 'HB'");
            if (emp_id != "")
            {
                sb.Append(" and a.EMP_ID like @EMP_ID ");
                ht.Add("@EMP_ID", emp_id + "%");
            }
            if (skill_type != "-1" && skill_type != "")
            {
                sb.Append(" and a.SKILL_TYPE = @SKILL_TYPE ");
                ht.Add("@SKILL_TYPE", skill_type);
            }
            if (skill_grade != "-1" && skill_grade != "")
            {
                sb.Append(" and a.SKILL_GRADE = @SKILL_GRADE ");
                ht.Add("@SKILL_GRADE", skill_grade);
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

    internal System.Data.DataTable getSKILL_GRADE()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select distinct SKILL_GRADE from TB_H_M_EMP_SKILL ");
            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }



    internal DataTable getExistData(string emp_id, string skill_type, string skill_desc)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select EMP_ID from TB_H_M_EMP_SKILL where EMP_ID = @EMP_ID and SKILL_TYPE = @SKILL_TYPE and SKILL_DESC = @SKILL_DESC");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@SKILL_TYPE", skill_type);
            ht.Add("@SKILL_DESC", skill_desc);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void addSkill()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("insert into TB_H_M_EMP_SKILL ");
            sb.Append("(EMP_ID,SKILL_TYPE,SKILL_DESC,SKILL_GRADE,SKILL_ORG,AWARD_DT,ORI_DEPT_NO,ORI_DEPT_FULL_NAME,ORI_DIV_DEPT_FULL_NAME,ORI_DEPT_NAME_20,ORI_DEPT_NAME_30,ORI_DEPT_NAME_40,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID) ");
            sb.Append(" select @EMP_ID,@SKILL_TYPE,@SKILL_DESC,@SKILL_GRADE,@SKILL_ORG,@AWARD_DT,DEPT_NO,DEPT_FULL_NAME,DIV_DEPT_FULL_NAME,DEPT_NAME_20,DEPT_NAME_30,DEPT_NAME_40,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID");
            sb.Append(" from VW_H_EMP_DATA where EMP_ID = @EMP_ID");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SKILL_TYPE", SKILL_TYPE);
            ht.Add("@SKILL_DESC", SKILL_DESC);
            ht.Add("@SKILL_GRADE", SKILL_GRADE);
            ht.Add("@SKILL_ORG", SKILL_ORG);
            if (AWARD_DT == "")
                ht.Add("@AWARD_DT", DBNull.Value);
            else
                ht.Add("@AWARD_DT", AWARD_DT);
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

    internal void updateSkill()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_H_M_EMP_SKILL ");
            sb.Append(" set SKILL_GRADE = @SKILL_GRADE,SKILL_ORG = @SKILL_ORG,AWARD_DT = @AWARD_DT,UPDATED_BY = @UPDATED_BY, UPDATED_DT = GETDATE()");
            sb.Append(" where EMP_ID = @EMP_ID and SKILL_TYPE = @SKILL_TYPE and SKILL_DESC = @SKILL_DESC");
            ht.Add("@SKILL_GRADE", SKILL_GRADE);
            ht.Add("@SKILL_ORG", SKILL_ORG);
            if (AWARD_DT == "")
                ht.Add("@AWARD_DT", DBNull.Value);
            else
                ht.Add("@AWARD_DT", AWARD_DT);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SKILL_TYPE", SKILL_TYPE);
            ht.Add("@SKILL_DESC", SKILL_DESC);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void delete_Skill(string emp_id, string skill_type, string skill_desc)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("delete from TB_H_M_EMP_SKILL where EMP_ID = @EMP_ID and SKILL_TYPE = @SKILL_TYPE and SKILL_DESC = @SKILL_DESC");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@SKILL_TYPE", skill_type);
            ht.Add("@SKILL_DESC", skill_desc);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getDeptData(string emp_id, string dept_no)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select DEPT_NO from VW_H_EMP_DATA Where EMP_ID = @EMP_ID AND substring(DEPT_NO,0,3) = substring(@DEPT_NO, 0, 3)");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@DEPT_NO", dept_no);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
}