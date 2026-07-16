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
/// CFB2SE0200DAO 的摘要描述
/// </summary>
public class CFB2SE0200DAO : BaseDAO
{
    public string data_key { get; set; }
    public string EFFECT_YM { get; set; }
    public string LEVEL_CD { get; set; }
    public string PJOB_TYPE { get; set; }
    public string EXAMINE_A { get; set; }
    public string EXAMINE_B { get; set; }
    public string EXAMINE_C1 { get; set; }
    public string EXAMINE_C2 { get; set; }
    public string EXAMINE_D { get; set; }
    public string EXAMINE_E { get; set; }
    public string ORDER_SEQ { get; set; }

    public string EXAMINE_S { get; set; }
    public string EXAMINE_C { get; set; }
    public string EXAMINE_F { get; set; }
    public string EXAMINE_G { get; set; }
    public string EXAMINE_H { get; set; }
    public string EXAMINE_I { get; set; }
    public string EXAMINE_J { get; set; }

    public string CREATED_BY { get; set; }
    public string UPDATED_BY { get; set; }
    public string FUNC_ID { get; set; }

	public CFB2SE0200DAO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}
    public DataTable getDDL()
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select LEVEL_CD from TB_H_M_LEVEL ");
            sb.Append(" where IS_UNION_MEMBER ='N' and GETDATE() >=START_DT  and GETDATE()<=END_DT");
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public DataTable GetData(int startRowIndex, int maximumRows, string sortExpression, string effect_ym, string level_cd)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" Select * From");
            sb.AppendLine(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.AppendLine(" LEVEL_CD,PJOB_TYPE,EXAMINE_A,EXAMINE_B,EXAMINE_C1,EXAMINE_C2,EXAMINE_D,EXAMINE_E,ORDER_SEQ ");
            sb.AppendLine(" ,EXAMINE_S,EXAMINE_C,EXAMINE_F,EXAMINE_G,EXAMINE_H,EXAMINE_I,EXAMINE_J ");
            sb.AppendLine(" ,EFFECT_YM+LEVEL_CD as qdatakey,CASE WHEN PJOB_TYPE = 'M' then '管理職' WHEN PJOB_TYPE = 'P' then '專業職' ELSE '' END as PJOB_NAME");
            sb.AppendLine(" from TB_S_M_2BSALARY_SET_D");
            sb.AppendLine(" where 1=1 and EFFECT_YM=@effect_ym");

            ht.Add("@effect_ym", effect_ym.Replace("/", ""));
            if (level_cd != "" && level_cd != "-1")
            {
                sb.AppendLine(" and LEVEL_CD=@level_cd  ");
                ht.Add("@level_cd", level_cd);
            }
            sb.AppendLine(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.AppendLine(" AND CAST(@startRowIndex+@maximumRows as varchar) ");

            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public int GetCount(int startRowIndex, int maximumRows, string effect_ym, string level_cd)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("Select COUNT(*) total_record");
            sb.AppendLine(" from TB_S_M_2BSALARY_SET_D");
            sb.AppendLine(" where 1=1 and EFFECT_YM=@effect_ym");

            ht.Add("@effect_ym", effect_ym.Replace("/", ""));
            if (level_cd != "" && level_cd != "-1")
            {
                sb.AppendLine(" and LEVEL_CD=@level_cd  ");
                ht.Add("@level_cd", level_cd);
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


    #region 載入
    //依畫面.生效年月至[考核調薪金額設定主檔],找有無該生效年月的資料
    public int GetCount_TB_S_M_2BSALARY_SET_H()
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("Select COUNT(*) total_record");
            sb.AppendLine(" from TB_S_M_2BSALARY_SET_H");
            sb.AppendLine(" where EFFECT_YM=@EFFECT_YM");
            ht.Add("@EFFECT_YM", EFFECT_YM);

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

    //載入的資料
    public DataTable GetData_Add(int startRowIndex, int maximumRows, string sortExpression, string effect_ym, string level_cd)
    {
        try
        {
            
                if (sortExpression != "LEVEL_CD")
                    sortExpression = "a.ORDER_SEQ,a.LEVEL_CD";
                if (sortExpression.Contains("ORDER_SEQ,LEVEL_CD"))
                    sortExpression = sortExpression.Replace("ORDER_SEQ,LEVEL_CD", "a.ORDER_SEQ,a.LEVEL_CD");
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" Select * From");
            sb.AppendLine(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + ") As RowNumber,");
            sb.AppendLine(" a.LEVEL_CD,a.ORDER_SEQ,b.PJOB_TYPE,");
            sb.AppendLine(" CASE WHEN b.PJOB_TYPE = 'M' then '管理職' WHEN b.PJOB_TYPE = 'P' then '專業職' ELSE '' END as PJOB_NAME ");
            sb.AppendLine(" from TB_H_M_LEVEL a");
            sb.AppendLine(" left join TB_S_M_2B_LEVEL_SALARY b on a.LEVEL_CD = b.LEVEL_CD and a.start_dt = b.start_dt ");
            sb.AppendLine(" where a.IS_UNION_MEMBER ='N' and (GETDATE() BETWEEN a.START_DT AND a.END_DT)");            
            sb.AppendLine(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.AppendLine(" AND CAST(@startRowIndex+@maximumRows as varchar) ");

            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public int GetCount_Add(int startRowIndex, int maximumRows, string effect_ym, string level_cd)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("Select COUNT(*) total_record");
            sb.AppendLine(" from TB_H_M_LEVEL a");
            sb.AppendLine(" left join TB_S_M_2B_LEVEL_SALARY b on a.LEVEL_CD = b.LEVEL_CD and a.start_dt = b.start_dt ");
            sb.AppendLine(" where a.IS_UNION_MEMBER ='N' and GETDATE() >=a.START_DT  and GETDATE()<=a.END_DT");

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
    public void Add_TB_S_M_2BSALARY_SET_H(string EFFECT_YM)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("INSERT INTO TB_S_M_2BSALARY_SET_H (EFFECT_YM,RELEASE_DT,RELEASE_BY,APPROVE_DT,APPROVE_BY,APPROVE_STATUS,");
            sb.AppendLine(" CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.AppendLine(" Values (@EFFECT_YM,@RELEASE_DT,@RELEASE_BY,@APPROVE_DT,@APPROVE_BY,@APPROVE_STATUS,");
            sb.AppendLine(" @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@EFFECT_YM", EFFECT_YM);
            ht.Add("@RELEASE_DT", DBNull.Value);
            ht.Add("@RELEASE_BY", DBNull.Value);
            ht.Add("@APPROVE_DT", DBNull.Value);
            ht.Add("@APPROVE_BY", DBNull.Value);
            ht.Add("@APPROVE_STATUS", "N");
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SE020");
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void Add_TB_S_M_2BSALARYSET_D()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("INSERT INTO TB_S_M_2BSALARY_SET_D (EFFECT_YM,LEVEL_CD,PJOB_TYPE,EXAMINE_A,EXAMINE_B,EXAMINE_C1,EXAMINE_C2,EXAMINE_D,EXAMINE_E,APPROVE_MARK,ORDER_SEQ");
            sb.AppendLine("                               ,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.AppendLine(" Values (@EFFECT_YM,@LEVEL_CD,@PJOB_TYPE,@EXAMINE_A,@EXAMINE_B,@EXAMINE_C1,@EXAMINE_C2,@EXAMINE_D,@EXAMINE_E,@APPROVE_MARK,@ORDER_SEQ");
            sb.AppendLine("        ,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@EFFECT_YM", EFFECT_YM);
            ht.Add("@LEVEL_CD", LEVEL_CD);
            ht.Add("@PJOB_TYPE", PJOB_TYPE);            
            ht.Add("@EXAMINE_A", EXAMINE_A);
            ht.Add("@EXAMINE_B", EXAMINE_B);
            ht.Add("@EXAMINE_C1", EXAMINE_C1);
            ht.Add("@EXAMINE_C2", EXAMINE_C2);
            ht.Add("@EXAMINE_D", EXAMINE_D);
            ht.Add("@EXAMINE_E", EXAMINE_E);
            ht.Add("@APPROVE_MARK", "N");
            ht.Add("@ORDER_SEQ", ORDER_SEQ);
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SE020");
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion

    #region 修改
    //依畫面.生效年月至[考核調薪金額設定主檔],找有無該生效年月的資料
    public DataTable Get_H_RELEASE_DT()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("Select RELEASE_DT");
            sb.AppendLine(" from  TB_S_M_2BSALARY_SET_H");
            sb.AppendLine(" where EFFECT_YM=@EFFECT_YM");
            ht.Add("@EFFECT_YM", EFFECT_YM);

            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }

    }
    public void Update_TB_S_M_2BSALARY_SET_D()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Update TB_S_M_2BSALARY_SET_D ");
            sb.Append(" Set EXAMINE_A=@EXAMINE_A,EXAMINE_B=@EXAMINE_B,EXAMINE_C1=@EXAMINE_C1,EXAMINE_C2=@EXAMINE_C2,EXAMINE_D=@EXAMINE_D,EXAMINE_E=@EXAMINE_E");
            sb.Append(" ,UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID");
            sb.Append(" where EFFECT_YM+LEVEL_CD = @data_key and PJOB_TYPE = @PJOB_TYPE");

            ht.Add("@EXAMINE_A", EXAMINE_A);
            ht.Add("@EXAMINE_B", EXAMINE_B);
            ht.Add("@EXAMINE_C1", EXAMINE_C1);
            ht.Add("@EXAMINE_C2", EXAMINE_C2);
            ht.Add("@EXAMINE_D", EXAMINE_D);
            ht.Add("@EXAMINE_E", EXAMINE_E);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SE020");
            ht.Add("@data_key", data_key);
            ht.Add("@PJOB_TYPE", PJOB_TYPE);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }
    #endregion


    #region EXCEL下載及上傳

    //檢核資格級數
    public int chklevelcd(string level_cd)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@"
                IF EXISTS 
                (	select 1 from VW_TB_H_M_LEVEL  WHERE LEVEL_CD=@LEVEL_CD and IS_UNION_MEMBER='N'  )
                  SELECT 1 AS resultCount
                ELSE
                  SELECT 0 AS resultCount

            ");
            //PK值
            ht.Add("@LEVEL_CD", level_cd);

            //dbConn.ExecuteT(sb, ht, true);
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

    //取得職務區分
    public string getScore_Str()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" DECLARE @Columns VARCHAR(MAX)='' ");
            sb.Append(@" SELECT @Columns = @Columns + sub_cd + ','  FROM TB_9_M_COMM_D where SYS_CD='SE' and MAIN_CD='PJOB_TYPE' AND IS_VALID='Y' ");
            sb.Append(@" SELECT @Columns AS [PJOB_TYPE]");

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                return (string)dt.Rows[0]["PJOB_TYPE"];
            }
            else
            {
                return "";
            }
        }
        catch (Exception)
        {

            throw;
        }
    }

    //取得EXCEL的資料
    public DataTable getExcelResultData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(@"SELECT a.LEVEL_CD,SUB_CD AS PJOB_TYPE
                from VW_TB_H_M_LEVEL a
                LEFT JOIN TB_9_M_COMM_D  b on b.SYS_CD='SE' and b.MAIN_CD='PJOB_TYPE' AND b.IS_VALID='Y'
                where a.IS_UNION_MEMBER ='N'
                ORDER BY A.ORDER_SEQ,b.ORDER_SEQ
            ");

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    //刪除 TB_S_M_2BSALARY_SET_H	2B以上本薪調整主檔   && TB_S_M_2BSALARY_SET_D	2B以上本薪調整明細檔
    public void del_TB_S_M_SALARYSET(string tableName)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("delete from  " + tableName + "  where EFFECT_YM = @EFFECT_YM ");
            ht.Add("@EFFECT_YM", EFFECT_YM);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //新增 TB_S_M_2BSALARY_SET_D	2B以上本薪調整明細檔
    public void insert_TB_S_M_2BSALARYSET_D()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(@"INSERT INTO TB_S_M_2BSALARY_SET_D (
             EFFECT_YM,LEVEL_CD,PJOB_TYPE
            ,EXAMINE_S,EXAMINE_A,EXAMINE_B,EXAMINE_C,EXAMINE_D,EXAMINE_E,EXAMINE_F,EXAMINE_G,EXAMINE_H,EXAMINE_I,EXAMINE_J
            ,APPROVE_MARK,ORDER_SEQ ,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)
            Values (
            @EFFECT_YM,@LEVEL_CD,@PJOB_TYPE
            ,@EXAMINE_S,@EXAMINE_A,@EXAMINE_B,@EXAMINE_C,@EXAMINE_D,@EXAMINE_E,@EXAMINE_F,@EXAMINE_G,@EXAMINE_H,@EXAMINE_I,@EXAMINE_J
            ,@APPROVE_MARK,@ORDER_SEQ,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)
          ");

            ht.Add("@EFFECT_YM", EFFECT_YM);
            ht.Add("@LEVEL_CD", LEVEL_CD);
            ht.Add("@PJOB_TYPE", PJOB_TYPE);
            ht.Add("@EXAMINE_S", EXAMINE_S);
            ht.Add("@EXAMINE_A", EXAMINE_A);
            ht.Add("@EXAMINE_B", EXAMINE_B);
            ht.Add("@EXAMINE_C", EXAMINE_C);           
            ht.Add("@EXAMINE_D", EXAMINE_D);
            ht.Add("@EXAMINE_E", EXAMINE_E);
            ht.Add("@EXAMINE_F", EXAMINE_F);
            ht.Add("@EXAMINE_G", EXAMINE_G);
            ht.Add("@EXAMINE_H", EXAMINE_H);
            ht.Add("@EXAMINE_I", EXAMINE_I);
            ht.Add("@EXAMINE_J", EXAMINE_J);
            ht.Add("@APPROVE_MARK", "N");
            ht.Add("@ORDER_SEQ", 0);
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SE020");
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //更新 ORDER_SEQ
    public void upd_TB_S_M_SALARYSET_D()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(@"    
                UPDATE  A
                SET A.ORDER_SEQ=isnull(B.ORDER_SEQ,999)
                from
                (
                select * from TB_S_M_2BSALARY_SET_D
                where effect_YM=@EFFECT_YM
                )A left join VW_TB_H_M_LEVEL B ON A.LEVEL_CD=B.LEVEL_CD
                ");
            ht.Add("@EFFECT_YM", EFFECT_YM);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion

}