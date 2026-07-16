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
/// CFB2DH0200DAO 的摘要描述
/// </summary>
public class CFB2DH0200DAO : BaseDAO
{
    public string UNION_PJOB_CD { get; set; }
    public string UNION_PJOB_DESC { get; set; }
    public string LEAVE_MAX_HOUR_01 { get; set; }
    public string LEAVE_MAX_HOUR_02 { get; set; }
    public string LEAVE_MAX_HOUR_03 { get; set; }
    public string LEAVE_MAX_HOUR_04 { get; set; }
    public string LEAVE_MAX_HOUR_05 { get; set; }
    public string LEAVE_MAX_HOUR_06 { get; set; }
    public string LEAVE_MAX_HOUR_07 { get; set; }
    public string LEAVE_MAX_HOUR_08 { get; set; }
    public string LEAVE_MAX_HOUR_09 { get; set; }
    public string LEAVE_MAX_HOUR_10 { get; set; }
    public string LEAVE_MAX_HOUR_11 { get; set; }
    public string LEAVE_MAX_HOUR_12 { get; set; }
    public string CREATED_BY { get; set; }
    public string UPDATED_BY { get; set; }
    public string FUNC_ID { get; set; }

	public CFB2DH0200DAO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string union_pjob_cd)
    {
        try
        {   
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * From");
            sb.Append(" (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" UNION_PJOB_CD,UNION_PJOB_DESC,LEAVE_MAX_HOUR_01,LEAVE_MAX_HOUR_02,LEAVE_MAX_HOUR_03, ");
            sb.Append(" LEAVE_MAX_HOUR_04,LEAVE_MAX_HOUR_05,LEAVE_MAX_HOUR_06,LEAVE_MAX_HOUR_07,LEAVE_MAX_HOUR_08, ");
            sb.Append(" LEAVE_MAX_HOUR_09,LEAVE_MAX_HOUR_10,LEAVE_MAX_HOUR_11,LEAVE_MAX_HOUR_12 ");
            sb.Append(" from TB_D_M_UNION_PJOB ");
            sb.Append(" where 1=1 ");

            if (union_pjob_cd != "-1" && union_pjob_cd != null)
            {
                sb.Append(" and UNION_PJOB_CD = @union_pjob_cd ");
                ht.Add("@union_pjob_cd", union_pjob_cd);
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

    public int getCount(int startRowIndex, int maximumRows, string union_pjob_cd)
    {
        try
        {    
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select COUNT(*) total_record ");
            sb.Append(" from TB_D_M_UNION_PJOB ");
            sb.Append(" where 1=1 ");

            if (union_pjob_cd != "-1" && union_pjob_cd != null)
            {
                sb.Append(" and UNION_PJOB_CD = @union_pjob_cd ");
                ht.Add("@union_pjob_cd", union_pjob_cd);
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

    public void deleteUNION_PJOB(string item)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //寫log
            sb.Append(" update TB_D_M_UNION_PJOB set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2DH020' ");
            sb.Append("  where UNION_PJOB_CD = @UNION_PJOB_CD; ");
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);

            sb.Append(" delete from TB_D_M_UNION_PJOB");
            sb.Append(" where UNION_PJOB_CD = @UNION_PJOB_CD;");
            ht.Add("@UNION_PJOB_CD", item);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getDEPT_ORG(string item)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select EMP_ID from TB_H_M_EMP");
            sb.Append(" where UNION_PJOB_CD = @UNION_PJOB_CD");
            ht.Add("@UNION_PJOB_CD", item);

            return dbConn.Query(sb, ht);
        }
        catch (Exception ex)
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
            sb.Append("select UNION_PJOB_CD from TB_D_M_UNION_PJOB");
            sb.Append(" where UNION_PJOB_CD = @UNION_PJOB_CD");
            ht.Add("@UNION_PJOB_CD", UNION_PJOB_CD);

            return dbConn.Query(sb, ht);
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    public void addUNION_PJOB()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("insert into TB_D_M_UNION_PJOB ( ");
            sb.Append(" UNION_PJOB_CD,UNION_PJOB_DESC,LEAVE_MAX_HOUR_01,LEAVE_MAX_HOUR_02,LEAVE_MAX_HOUR_03, ");
            sb.Append(" LEAVE_MAX_HOUR_04,LEAVE_MAX_HOUR_05,LEAVE_MAX_HOUR_06,LEAVE_MAX_HOUR_07,LEAVE_MAX_HOUR_08, ");
            sb.Append(" LEAVE_MAX_HOUR_09,LEAVE_MAX_HOUR_10,LEAVE_MAX_HOUR_11,LEAVE_MAX_HOUR_12, ");
            sb.Append(" CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" values ( ");
            sb.Append(" @UNION_PJOB_CD,@UNION_PJOB_DESC,@LEAVE_MAX_HOUR_01,@LEAVE_MAX_HOUR_02,@LEAVE_MAX_HOUR_03, ");
            sb.Append(" @LEAVE_MAX_HOUR_04,@LEAVE_MAX_HOUR_05,@LEAVE_MAX_HOUR_06,@LEAVE_MAX_HOUR_07,@LEAVE_MAX_HOUR_08, ");
            sb.Append(" @LEAVE_MAX_HOUR_09,@LEAVE_MAX_HOUR_10,@LEAVE_MAX_HOUR_11,@LEAVE_MAX_HOUR_12, ");
            sb.Append(" @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@UNION_PJOB_CD", UNION_PJOB_CD);
            ht.Add("@UNION_PJOB_DESC", UNION_PJOB_DESC);
            ht.Add("@LEAVE_MAX_HOUR_01", LEAVE_MAX_HOUR_01);
            ht.Add("@LEAVE_MAX_HOUR_02", LEAVE_MAX_HOUR_02);
            ht.Add("@LEAVE_MAX_HOUR_03", LEAVE_MAX_HOUR_03);
            ht.Add("@LEAVE_MAX_HOUR_04", LEAVE_MAX_HOUR_04);
            ht.Add("@LEAVE_MAX_HOUR_05", LEAVE_MAX_HOUR_05);
            ht.Add("@LEAVE_MAX_HOUR_06", LEAVE_MAX_HOUR_06);
            ht.Add("@LEAVE_MAX_HOUR_07", LEAVE_MAX_HOUR_07);
            ht.Add("@LEAVE_MAX_HOUR_08", LEAVE_MAX_HOUR_08);
            ht.Add("@LEAVE_MAX_HOUR_09", LEAVE_MAX_HOUR_09);
            ht.Add("@LEAVE_MAX_HOUR_10", LEAVE_MAX_HOUR_10);
            ht.Add("@LEAVE_MAX_HOUR_11", LEAVE_MAX_HOUR_11);
            ht.Add("@LEAVE_MAX_HOUR_12", LEAVE_MAX_HOUR_12);
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

    public void updateUNION_PJOB()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_D_M_UNION_PJOB ");
            sb.Append(" set UNION_PJOB_DESC=@UNION_PJOB_DESC,LEAVE_MAX_HOUR_01=@LEAVE_MAX_HOUR_01,LEAVE_MAX_HOUR_02=@LEAVE_MAX_HOUR_02, ");
            sb.Append(" LEAVE_MAX_HOUR_03=@LEAVE_MAX_HOUR_03,LEAVE_MAX_HOUR_04=@LEAVE_MAX_HOUR_04,LEAVE_MAX_HOUR_05=@LEAVE_MAX_HOUR_05, ");
            sb.Append(" LEAVE_MAX_HOUR_06=@LEAVE_MAX_HOUR_06,LEAVE_MAX_HOUR_07=@LEAVE_MAX_HOUR_07,LEAVE_MAX_HOUR_08=@LEAVE_MAX_HOUR_08, ");
            sb.Append(" LEAVE_MAX_HOUR_09=@LEAVE_MAX_HOUR_09,LEAVE_MAX_HOUR_10=@LEAVE_MAX_HOUR_10,LEAVE_MAX_HOUR_11=@LEAVE_MAX_HOUR_11, ");
            sb.Append(" LEAVE_MAX_HOUR_12=@LEAVE_MAX_HOUR_12, ");
            sb.Append(" UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID ");
            sb.Append(" where UNION_PJOB_CD=@UNION_PJOB_CD ");

            ht.Add("@UNION_PJOB_CD", UNION_PJOB_CD);
            ht.Add("@UNION_PJOB_DESC", UNION_PJOB_DESC);
            ht.Add("@LEAVE_MAX_HOUR_01", LEAVE_MAX_HOUR_01);
            ht.Add("@LEAVE_MAX_HOUR_02", LEAVE_MAX_HOUR_02);
            ht.Add("@LEAVE_MAX_HOUR_03", LEAVE_MAX_HOUR_03);
            ht.Add("@LEAVE_MAX_HOUR_04", LEAVE_MAX_HOUR_04);
            ht.Add("@LEAVE_MAX_HOUR_05", LEAVE_MAX_HOUR_05);
            ht.Add("@LEAVE_MAX_HOUR_06", LEAVE_MAX_HOUR_06);
            ht.Add("@LEAVE_MAX_HOUR_07", LEAVE_MAX_HOUR_07);
            ht.Add("@LEAVE_MAX_HOUR_08", LEAVE_MAX_HOUR_08);
            ht.Add("@LEAVE_MAX_HOUR_09", LEAVE_MAX_HOUR_09);
            ht.Add("@LEAVE_MAX_HOUR_10", LEAVE_MAX_HOUR_10);
            ht.Add("@LEAVE_MAX_HOUR_11", LEAVE_MAX_HOUR_11);
            ht.Add("@LEAVE_MAX_HOUR_12", LEAVE_MAX_HOUR_12);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable getUNION_PJOB_CD()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select distinct UNION_PJOB_CD+'-'+UNION_PJOB_DESC UNION_PJOB_DESC,UNION_PJOB_CD ");
            sb.Append(" from TB_D_M_UNION_PJOB");
            sb.Append(" order by UNION_PJOB_CD");
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
}