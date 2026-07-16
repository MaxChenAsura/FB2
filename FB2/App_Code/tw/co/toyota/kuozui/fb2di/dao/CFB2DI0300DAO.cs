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
/// CFB2DI0300DAO 的摘要描述
/// </summary>
public class CFB2DI0300DAO : BaseDAO
{
    public string DEPT_NO { get; set; }
    public string TARGET_TYPE { get; set; }
    public string TARGET_YEAR { get; set; }
    public string TARGET_VALUE_01 { get; set; }
    public string TARGET_VALUE_02 { get; set; }
    public string TARGET_VALUE_03 { get; set; }
    public string TARGET_VALUE_04 { get; set; }
    public string TARGET_VALUE_05 { get; set; }
    public string TARGET_VALUE_06 { get; set; }
    public string TARGET_VALUE_07 { get; set; }
    public string TARGET_VALUE_08 { get; set; }
    public string TARGET_VALUE_09 { get; set; }
    public string TARGET_VALUE_10 { get; set; }
    public string TARGET_VALUE_11 { get; set; }
    public string TARGET_VALUE_12 { get; set; }
    public string CREATED_BY { get; set; }
    public string UPDATED_BY { get; set; }
    public string FUNC_ID { get; set; }

    public string WS_CD { get; set; }
    public string PJOB_CD { get; set; }
    public string WORK_CD { get; set; }
    public CFB2DI0300DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string dept_no, string target_type,
                string target_ym_s, string target_ym_e)
    {
        try
        {
            if (sortExpression.Contains("DEPT_NO"))
                sortExpression = sortExpression.Replace("DEPT_NO", "a.DEPT_NO");

            if (sortExpression.Contains("TARGET_TYPE"))
                sortExpression = sortExpression.Replace("TARGET_TYPE", "a.TARGET_TYPE");

            if (sortExpression.Contains("TARGET_YEAR"))
                sortExpression = sortExpression.Replace("TARGET_YEAR", "a.TARGET_YEAR");

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" b.DEPT_NO + '-' + b.DEPT_NAME DEPT_NO,c.SUB_CD + '-' + c.SUB_DESC TARGET_TYPE,a.TARGET_YEAR, ");
            sb.Append(" a.TARGET_VALUE_01,a.TARGET_VALUE_02,a.TARGET_VALUE_03,a.TARGET_VALUE_04,a.TARGET_VALUE_05, ");
            sb.Append(" a.TARGET_VALUE_06,a.TARGET_VALUE_07,a.TARGET_VALUE_08,a.TARGET_VALUE_09,a.TARGET_VALUE_10, ");
            sb.Append(" a.TARGET_VALUE_11,a.TARGET_VALUE_12 ");
            sb.Append(" from TB_D_M_OVERTIME_TARGET a ");
            sb.Append(" left join TB_H_M_DEPT b on b.DEPT_NO=a.DEPT_NO ");
            sb.Append(" left join TB_9_M_COMM_D c on c.SYS_CD = 'DI' and c.MAIN_CD = 'TARGET_TYPE' and c.IS_VALID='Y' and c.SUB_CD=a.TARGET_TYPE ");
            sb.Append(" where 1=1 ");

            if (dept_no != "")
            {
                sb.Append(" and a.DEPT_NO = @dept_no ");
                ht.Add("@dept_no", dept_no);
            }

            if (target_type != "-1" && target_type != null)
            {
                sb.Append(" and a.TARGET_TYPE = @target_type ");
                ht.Add("@target_type", target_type);
            }

            if (target_ym_s != "")
            {
                if (target_ym_e != "")
                {
                    sb.Append(" and a.TARGET_YEAR >= @target_ym_s and a.TARGET_YEAR <= @target_ym_e ");
                    ht.Add("@target_ym_s", target_ym_s);
                    ht.Add("@target_ym_e", target_ym_e);
                }
                else
                {
                    sb.Append(" and a.TARGET_YEAR >= @target_ym_s  ");
                    ht.Add("@target_ym_s", target_ym_s);
                }

            }
            else if (target_ym_e != "")
            {
                sb.Append(" and a.TARGET_YEAR <= @target_ym_e  ");
                ht.Add("@target_ym_e", target_ym_e);
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

    public int getCount(int startRowIndex, int maximumRows, string dept_no, string target_type,
                string target_ym_s, string target_ym_e)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select COUNT(*) total_record ");
            sb.Append(" from TB_D_M_OVERTIME_TARGET a ");
            sb.Append(" left join TB_H_M_DEPT b on b.DEPT_NO=a.DEPT_NO ");
            sb.Append(" left join TB_9_M_COMM_D c on c.SYS_CD = 'DI' and c.MAIN_CD = 'TARGET_TYPE' and c.IS_VALID='Y' and c.SUB_CD=a.TARGET_TYPE ");
            sb.Append(" where 1=1 ");

            if (dept_no != "")
            {
                sb.Append(" and a.DEPT_NO = @dept_no ");
                ht.Add("@dept_no", dept_no);
            }

            if (target_type != "-1" && target_type != null)
            {
                sb.Append(" and a.TARGET_TYPE = @target_type ");
                ht.Add("@target_type", target_type);
            }

            if (target_ym_s != "")
            {
                if (target_ym_e != "")
                {
                    sb.Append(" and a.TARGET_YEAR >= @target_ym_s and a.TARGET_YEAR <= @target_ym_e ");
                    ht.Add("@target_ym_s", target_ym_s);
                    ht.Add("@target_ym_e", target_ym_e);
                }
                else
                {
                    sb.Append(" and a.TARGET_YEAR >= @target_ym_s  ");
                    ht.Add("@target_ym_s", target_ym_s);
                }

            }
            else if (target_ym_e != "")
            {
                sb.Append(" and a.TARGET_YEAR <= @target_ym_e  ");
                ht.Add("@target_ym_e", target_ym_e);
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

    //適用人員設定
    public DataTable getData2(int startRowIndex, int maximumRows, string sortExpression, string target_type, string target_year)
    {
        try
        { 
            if (sortExpression.Contains("DEPT_NO"))
                sortExpression = sortExpression.Replace("DEPT_NO", "a.DEPT_NO");

            if (sortExpression.Contains("TARGET_TYPE"))
                sortExpression = sortExpression.Replace("TARGET_TYPE", "a.TARGET_TYPE");

            if (sortExpression.Contains("WS_CD"))
                sortExpression = sortExpression.Replace("WS_CD", "a.WS_CD");

            if (sortExpression.Contains("PJOB_CD"))
                sortExpression = sortExpression.Replace("PJOB_CD", "a.PJOB_CD");

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" a.TARGET_YEAR,a.DEPT_NO,a.TARGET_TYPE,b.SUB_CD+'-'+b.SUB_DESC WS_CD,c.PJOB_CD+'-'+c.PJOB_DESC PJOB_CD ");
            sb.Append(" ,a.WORK_CD,a.WORK_CD+'-'+d.SUB_DESC as WORK_DESC ");
            sb.Append(" from TB_D_M_OVERTIME_TARGET_EMP a ");
            sb.Append(" left join TB_9_M_COMM_D b on b.SYS_CD = 'HB' and b.MAIN_CD = 'WS_CD' and b.IS_VALID='Y' and b.SUB_CD=a.WS_CD ");
            sb.Append(" left join TB_9_M_COMM_D d on d.SYS_CD = 'HB' and d.MAIN_CD = 'WORK_CD' and d.IS_VALID='Y' and d.SUB_CD=a.WORK_CD ");
            sb.Append(" left join TB_H_M_PJOB c on c.PJOB_CD=a.PJOB_CD and c.END_DT >= GETDATE() ");
            sb.Append(" where 1=1 ");

            if (target_type != "")
            {
                sb.Append(" and a.TARGET_TYPE = @target_type ");
                ht.Add("@target_type", target_type.Split('-')[0]);
            }
            if (target_year != "")
            {
                sb.Append(" and a.TARGET_YEAR = @target_year ");
                ht.Add("@target_year", target_year);
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

    public int getCount2(int startRowIndex, int maximumRows, string target_type, string target_year)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select COUNT(*) total_record ");
            sb.Append(" from TB_D_M_OVERTIME_TARGET_EMP a ");
            sb.Append(" left join TB_9_M_COMM_D b on b.SYS_CD = 'HB' and b.MAIN_CD = 'WS_CD' and b.IS_VALID='Y' and b.SUB_CD=a.WS_CD ");
            sb.Append(" left join TB_H_M_PJOB c on c.PJOB_CD=a.PJOB_CD and c.END_DT >= GETDATE() ");
            sb.Append(" where 1=1 ");

            if (target_type != "")
            {
                sb.Append(" and a.TARGET_TYPE = @target_type ");
                ht.Add("@target_type", target_type.Split('-')[0]);
            }
            if (target_year != "")
            {
                sb.Append(" and a.TARGET_YEAR = @target_year ");
                ht.Add("@target_year", target_year);
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

    public void deleteOVERTIME_TARGET(Tuple<string, string, string> item)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //寫log
            sb.Append(" update TB_D_M_OVERTIME_TARGET set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2DI030' ");
            sb.Append(" where DEPT_NO = @DEPT_NO");
            sb.Append(" and TARGET_TYPE = @TARGET_TYPE and TARGET_YEAR=@TARGET_YEAR;");
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);

            sb.Append(" delete from TB_D_M_OVERTIME_TARGET");
            sb.Append(" where DEPT_NO = @DEPT_NO");
            sb.Append(" and TARGET_TYPE = @TARGET_TYPE and TARGET_YEAR=@TARGET_YEAR;");
            ht.Add("@DEPT_NO", item.Item1);
            ht.Add("@TARGET_TYPE", item.Item2);
            ht.Add("@TARGET_YEAR", item.Item3);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getExistData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select DEPT_NO from TB_D_M_OVERTIME_TARGET");
            sb.Append(" where DEPT_NO = @DEPT_NO");
            sb.Append(" and TARGET_TYPE = @TARGET_TYPE and TARGET_YEAR=@TARGET_YEAR");
            ht.Add("@DEPT_NO", DEPT_NO);
            ht.Add("@TARGET_TYPE", TARGET_TYPE);
            ht.Add("@TARGET_YEAR", TARGET_YEAR);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public void addOVERTIME_TARGET()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("insert into TB_D_M_OVERTIME_TARGET( ");
            sb.Append(" DEPT_NO,TARGET_TYPE,TARGET_YEAR,TARGET_VALUE_01,TARGET_VALUE_02,TARGET_VALUE_03, ");
            sb.Append(" TARGET_VALUE_04,TARGET_VALUE_05,TARGET_VALUE_06,TARGET_VALUE_07,TARGET_VALUE_08, ");
            sb.Append(" TARGET_VALUE_09,TARGET_VALUE_10,TARGET_VALUE_11,TARGET_VALUE_12, ");
            sb.Append(" CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID) ");
            sb.Append(" values ( ");
            sb.Append(" @DEPT_NO,@TARGET_TYPE,@TARGET_YEAR,@TARGET_VALUE_01,@TARGET_VALUE_02,@TARGET_VALUE_03, ");
            sb.Append(" @TARGET_VALUE_04,@TARGET_VALUE_05,@TARGET_VALUE_06,@TARGET_VALUE_07,@TARGET_VALUE_08, ");
            sb.Append(" @TARGET_VALUE_09,@TARGET_VALUE_10,@TARGET_VALUE_11,@TARGET_VALUE_12, ");
            sb.Append(" @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@DEPT_NO", DEPT_NO);
            ht.Add("@TARGET_TYPE", TARGET_TYPE);
            ht.Add("@TARGET_YEAR", TARGET_YEAR);
            if (TARGET_VALUE_01 == "")
                ht.Add("@TARGET_VALUE_01", DBNull.Value);
            else
                ht.Add("@TARGET_VALUE_01", TARGET_VALUE_01);
            if (TARGET_VALUE_02 == "")
                ht.Add("@TARGET_VALUE_02", DBNull.Value);
            else
                ht.Add("@TARGET_VALUE_02", TARGET_VALUE_02);
            if (TARGET_VALUE_03 == "")
                ht.Add("@TARGET_VALUE_03", DBNull.Value);
            else
                ht.Add("@TARGET_VALUE_03", TARGET_VALUE_03);
            if (TARGET_VALUE_04 == "")
                ht.Add("@TARGET_VALUE_04", DBNull.Value);
            else
                ht.Add("@TARGET_VALUE_04", TARGET_VALUE_04);
            if (TARGET_VALUE_05 == "")
                ht.Add("@TARGET_VALUE_05", DBNull.Value);
            else
                ht.Add("@TARGET_VALUE_05", TARGET_VALUE_05);
            if (TARGET_VALUE_06 == "")
                ht.Add("@TARGET_VALUE_06", DBNull.Value);
            else
                ht.Add("@TARGET_VALUE_06", TARGET_VALUE_06);
            if (TARGET_VALUE_07 == "")
                ht.Add("@TARGET_VALUE_07", DBNull.Value);
            else
                ht.Add("@TARGET_VALUE_07", TARGET_VALUE_07);
            if (TARGET_VALUE_08 == "")
                ht.Add("@TARGET_VALUE_08", DBNull.Value);
            else
                ht.Add("@TARGET_VALUE_08", TARGET_VALUE_08);
            if (TARGET_VALUE_09 == "")
                ht.Add("@TARGET_VALUE_09", DBNull.Value);
            else
                ht.Add("@TARGET_VALUE_09", TARGET_VALUE_09);
            if (TARGET_VALUE_10 == "")
                ht.Add("@TARGET_VALUE_10", DBNull.Value);
            else
                ht.Add("@TARGET_VALUE_10", TARGET_VALUE_10);
            if (TARGET_VALUE_11 == "")
                ht.Add("@TARGET_VALUE_11", DBNull.Value);
            else
                ht.Add("@TARGET_VALUE_11", TARGET_VALUE_11);
            if (TARGET_VALUE_12 == "")
                ht.Add("@TARGET_VALUE_12", DBNull.Value);
            else
                ht.Add("@TARGET_VALUE_12", TARGET_VALUE_12);
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

    public void updateOVERTIME_TARGET()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_D_M_OVERTIME_TARGET ");
            sb.Append(" set TARGET_VALUE_01=@TARGET_VALUE_01,TARGET_VALUE_02=@TARGET_VALUE_02,TARGET_VALUE_03=@TARGET_VALUE_03, ");
            sb.Append(" TARGET_VALUE_04=@TARGET_VALUE_04,TARGET_VALUE_05=@TARGET_VALUE_05,TARGET_VALUE_06=@TARGET_VALUE_06, ");
            sb.Append(" TARGET_VALUE_07=@TARGET_VALUE_07,TARGET_VALUE_08=@TARGET_VALUE_08,TARGET_VALUE_09=@TARGET_VALUE_09, ");
            sb.Append(" TARGET_VALUE_10=@TARGET_VALUE_10,TARGET_VALUE_11=@TARGET_VALUE_11,TARGET_VALUE_12=@TARGET_VALUE_12, ");
            sb.Append(" UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID");
            sb.Append(" where DEPT_NO = @DEPT_NO");
            sb.Append(" and TARGET_TYPE = @TARGET_TYPE and TARGET_YEAR=@TARGET_YEAR");

            ht.Add("@DEPT_NO", DEPT_NO);
            ht.Add("@TARGET_TYPE", TARGET_TYPE);
            ht.Add("@TARGET_YEAR", TARGET_YEAR);
            if (TARGET_VALUE_01 == "")
                ht.Add("@TARGET_VALUE_01", DBNull.Value);
            else
                ht.Add("@TARGET_VALUE_01", TARGET_VALUE_01);
            if (TARGET_VALUE_02 == "")
                ht.Add("@TARGET_VALUE_02", DBNull.Value);
            else
                ht.Add("@TARGET_VALUE_02", TARGET_VALUE_02);
            if (TARGET_VALUE_03 == "")
                ht.Add("@TARGET_VALUE_03", DBNull.Value);
            else
                ht.Add("@TARGET_VALUE_03", TARGET_VALUE_03);
            if (TARGET_VALUE_04 == "")
                ht.Add("@TARGET_VALUE_04", DBNull.Value);
            else
                ht.Add("@TARGET_VALUE_04", TARGET_VALUE_04);
            if (TARGET_VALUE_05 == "")
                ht.Add("@TARGET_VALUE_05", DBNull.Value);
            else
                ht.Add("@TARGET_VALUE_05", TARGET_VALUE_05);
            if (TARGET_VALUE_06 == "")
                ht.Add("@TARGET_VALUE_06", DBNull.Value);
            else
                ht.Add("@TARGET_VALUE_06", TARGET_VALUE_06);
            if (TARGET_VALUE_07 == "")
                ht.Add("@TARGET_VALUE_07", DBNull.Value);
            else
                ht.Add("@TARGET_VALUE_07", TARGET_VALUE_07);
            if (TARGET_VALUE_08 == "")
                ht.Add("@TARGET_VALUE_08", DBNull.Value);
            else
                ht.Add("@TARGET_VALUE_08", TARGET_VALUE_08);
            if (TARGET_VALUE_09 == "")
                ht.Add("@TARGET_VALUE_09", DBNull.Value);
            else
                ht.Add("@TARGET_VALUE_09", TARGET_VALUE_09);
            if (TARGET_VALUE_10 == "")
                ht.Add("@TARGET_VALUE_10", DBNull.Value);
            else
                ht.Add("@TARGET_VALUE_10", TARGET_VALUE_10);
            if (TARGET_VALUE_11 == "")
                ht.Add("@TARGET_VALUE_11", DBNull.Value);
            else
                ht.Add("@TARGET_VALUE_11", TARGET_VALUE_11);
            if (TARGET_VALUE_12 == "")
                ht.Add("@TARGET_VALUE_12", DBNull.Value);
            else
                ht.Add("@TARGET_VALUE_12", TARGET_VALUE_12);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void deleteOVERTIME_TARGET_EMP(Tuple<string, string, string, string, string> item)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //寫log
            sb.Append(" update TB_D_M_OVERTIME_TARGET_EMP set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2DI030' ");
            sb.Append(" where TARGET_YEAR = @TARGET_YEAR");
            sb.Append(" and TARGET_TYPE = @TARGET_TYPE ");
            sb.Append(" and WS_CD = @WS_CD and PJOB_CD = @PJOB_CD and WORK_CD = @WORK_CD; ");
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);

            sb.Append(" delete from TB_D_M_OVERTIME_TARGET_EMP");
            sb.Append(" where TARGET_YEAR = @TARGET_YEAR");
            sb.Append(" and TARGET_TYPE = @TARGET_TYPE ");
            sb.Append(" and WS_CD = @WS_CD and PJOB_CD = @PJOB_CD and WORK_CD = @WORK_CD; ");

            ht.Add("@TARGET_YEAR", item.Item1);
            ht.Add("@TARGET_TYPE", item.Item2);
            ht.Add("@WS_CD", item.Item3);
            ht.Add("@PJOB_CD", item.Item4);
            ht.Add("@WORK_CD", item.Item5);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getExistData2()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select WS_CD from TB_D_M_OVERTIME_TARGET_EMP");
            sb.Append(" where ");
            sb.Append(" TARGET_TYPE = @TARGET_TYPE ");
            sb.Append(" and WS_CD = @WS_CD and PJOB_CD = @PJOB_CD and TARGET_YEAR = @TARGET_YEAR and WORK_CD = @WORK_CD");

            ht.Add("@TARGET_TYPE", TARGET_TYPE);
            ht.Add("@WS_CD", WS_CD);
            ht.Add("@PJOB_CD", PJOB_CD);
            ht.Add("@TARGET_YEAR", TARGET_YEAR);
            ht.Add("@WORK_CD", WORK_CD);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public void addOVERTIME_TARGET_EMP()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" insert into TB_D_M_OVERTIME_TARGET_EMP( ");
            sb.Append(" DEPT_NO,TARGET_TYPE,TARGET_YEAR,WS_CD,PJOB_CD,WORK_CD, ");
            sb.Append(" CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID) ");
            sb.Append(" values ( ");
            sb.Append(" @DEPT_NO,@TARGET_TYPE,@TARGET_YEAR,@WS_CD,@PJOB_CD,@WORK_CD, ");
            sb.Append(" @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@DEPT_NO","");
            ht.Add("@TARGET_TYPE", TARGET_TYPE);
            ht.Add("@TARGET_YEAR", TARGET_YEAR);
            ht.Add("@WS_CD", WS_CD);
            ht.Add("@PJOB_CD", PJOB_CD);
            ht.Add("@WORK_CD", WORK_CD);
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

    public DataTable getDEPT_NAME(string dept_no)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select DEPT_NO,DEPT_NAME ");
            sb.Append(" from VW_H_DEPT_DATA ");
            sb.Append(" where DEPT_NO=@DEPT_NO and DEPT_LEVEL = '20' ");
            ht.Add("@DEPT_NO", dept_no);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
}