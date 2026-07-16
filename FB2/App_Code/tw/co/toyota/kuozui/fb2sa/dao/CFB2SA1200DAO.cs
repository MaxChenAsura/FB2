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
/// CFB2SA1200DAO 的摘要描述
/// </summary>
public class CFB2SA1200DAO : BaseDAO
{
    public string sys_cd { get; set; }
    public string main_cd { get; set; }
    public string is_valid { get; set; }
    public string code_val1 { get; set; }

	public CFB2SA1200DAO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    //public DataTable getddl_EDUCATION_CD()
    //{
    //    try
    //    {

    //        StringBuilder sb = new StringBuilder();
    //        Hashtable ht = new Hashtable();
    //        sb.Append("select SUB_CD ,SUB_CD+'-'+SUB_DESC SUB_DESC from TB_9_M_COMM_D ");
    //        sb.Append(" where SYS_CD=@sys_cd and MAIN_CD = @main_cd and IS_VALID=@is_valid");
    //        ht.Add("@sys_cd", sys_cd);
    //        ht.Add("@main_cd", main_cd);
    //        ht.Add("@is_valid", is_valid);
    //        return dbConn.Query(sb, ht);
    //    }
    //    catch
    //    {
    //        throw;
    //    }
    //}
    //public DataTable getddl_WS_CD()
    //{
    //    try
    //    {

    //        StringBuilder sb = new StringBuilder();
    //        Hashtable ht = new Hashtable();
    //        sb.Append("select SUB_CD ,SUB_CD+'-'+SUB_DESC SUB_DESC from TB_9_M_COMM_D ");
    //        sb.Append(" where SYS_CD=@sys_cd and MAIN_CD = @main_cd and IS_VALID=@is_valid and CODE_VAL1=@code_val1");
    //        ht.Add("@sys_cd", sys_cd);
    //        ht.Add("@main_cd", main_cd);
    //        ht.Add("@is_valid", is_valid);
    //        ht.Add("@code_val1", code_val1);
    //        return dbConn.Query(sb, ht);
    //    }
    //    catch
    //    {
    //        throw;
    //    }
    //}

    public DataTable GetData(int startRowIndex, int maximumRows, string sortExpression, string data_year, string education_cd, string level_cd
                             , string grade_cd, string ws_cd, string grade_year)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" a.WS_CD+'-'+b.SUB_DESC as WS_CD,a.LEVEL_CD,a.GRADE_CD,a.EDUCATION_CD+'-'+c.SUB_DESC as EDUCATION_CD");
            sb.Append(" ,a.GRADE_YEAR,a.LEVEL_PAY1,a.LEVEL_PAY2,a.LEVEL_PAY3,a.EFFECT_SDT,a.EFFECT_EDT");
            sb.Append(" from TB_S_M_HIRING_SALARY a");
            sb.Append(" left join TB_9_M_COMM_D b on a.WS_CD=b.SUB_CD and b.SYS_CD='HB' and b.MAIN_CD='WS_CD'");
            sb.Append(" left join TB_9_M_COMM_D c on a.EDUCATION_CD=c.SUB_CD and c.SYS_CD='HB' and c.MAIN_CD='EDUCATION_CD'");
            sb.Append(" where a.DATA_YEAR=@DATA_YEAR");
            ht.Add("@DATA_YEAR", data_year);
            if (education_cd != "" && education_cd != "-1")
            {
                sb.Append(" and a.EDUCATION_CD=@EDUCATION_CD  ");
                ht.Add("@EDUCATION_CD", education_cd);
            }
            if (level_cd != "")
            {
                sb.Append(" and a.LEVEL_CD like '%' + @LEVEL_CD + '%'  ");
                ht.Add("@LEVEL_CD", level_cd);
            }
            if (grade_cd != "")
            {
                sb.Append(" and a.GRADE_CD like '%' + @GRADE_CD + '%'");
                ht.Add("@GRADE_CD", grade_cd);
            }
            if (ws_cd != "" && ws_cd != "-1")
            {
                sb.Append(" and a.WS_CD = @WS_CD  ");
                ht.Add("@WS_CD", ws_cd);
            }
            if (grade_year != "")
            {
                sb.Append(" and a.GRADE_YEAR = @GRADE_YEAR  ");
                ht.Add("@GRADE_YEAR", grade_year);
            }
            sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar) ");

            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public int GetCount(int startRowIndex, int maximumRows, string data_year, string education_cd, string level_cd
                             , string grade_cd, string ws_cd, string grade_year)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select COUNT(*) total_record from TB_S_M_HIRING_SALARY a");
            sb.Append(" left join TB_9_M_COMM_D b on a.WS_CD=b.SUB_CD and b.SYS_CD='HB' and b.MAIN_CD='WS_CD'");
            sb.Append(" left join TB_9_M_COMM_D c on a.EDUCATION_CD=c.SUB_CD and c.SYS_CD='HB' and c.MAIN_CD='EDUCATION_CD'");
            sb.Append(" where a.DATA_YEAR=@DATA_YEAR");
            ht.Add("@DATA_YEAR", data_year);
            if (education_cd != "" && education_cd != "-1")
            {
                sb.Append(" and a.EDUCATION_CD=@EDUCATION_CD  ");
                ht.Add("@EDUCATION_CD", education_cd);
            }
            if (level_cd != "")
            {
                sb.Append(" and a.LEVEL_CD like '%' + @LEVEL_CD + '%'");
                ht.Add("@LEVEL_CD", level_cd);
            }
            if (grade_cd != "")
            {
                sb.Append(" and a.GRADE_CD like '%' + @GRADE_CD + '%'");
                ht.Add("@GRADE_CD", grade_cd);
            }
            if (ws_cd != "" && ws_cd != "-1")
            {
                sb.Append(" and a.WS_CD = @WS_CD  ");
                ht.Add("@WS_CD", ws_cd);
            }
            if (grade_year != "")
            {
                sb.Append(" and a.GRADE_YEAR = @GRADE_YEAR  ");
                ht.Add("@GRADE_YEAR", grade_year);
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
}