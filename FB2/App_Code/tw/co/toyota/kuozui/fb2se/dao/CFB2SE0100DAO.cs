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
/// CFB2SE0100DAO 的摘要描述
/// </summary>
public class CFB2SE0100DAO : BaseDAO
{
    public string data_key { get; set; }
    public string EFFECT_YM { get; set; }
    public string LEVEL_CD { get; set; }
    public string GRADE_CD { get; set; }						
    public string EXAMINE_A { get; set; }						
    public string EXAMINE_B	{ get; set; }					
    public string EXAMINE_C	{ get; set; }					
    public string EXAMINE_D	{ get; set; }					
    public string EXAMINE_E	{ get; set; }					
    public string ABILITY_ADJ { get; set; }						
    public string LEVEL_ADJ { get; set; }						
    public string LEVEL_PAY_LOW { get; set; }						
    public string LEVEL_PAY_AVG { get; set; }
    public string LEVEL_PAY_UP { get; set; }
    public string ORDER_SEQ { get; set; }

    public string CREATED_BY { get; set; }
    public string UPDATED_BY { get; set; }
    public string FUNC_ID { get; set; }	
    

	public CFB2SE0100DAO()
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
            sb.Append(" where IS_UNION_MEMBER ='Y' and GETDATE() >=START_DT  and GETDATE()<=END_DT");
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
            sb.AppendLine(" LEVEL_CD,GRADE_CD,EXAMINE_A,EXAMINE_B,EXAMINE_C,EXAMINE_D,EXAMINE_E,ABILITY_ADJ,");
            sb.AppendLine(" LEVEL_ADJ,LEVEL_PAY_LOW,LEVEL_PAY_AVG,LEVEL_PAY_UP,ORDER_SEQ,");
            sb.AppendLine(" EFFECT_YM+LEVEL_CD+GRADE_CD as qdatakey");
            sb.AppendLine(" from TB_S_M_SALARYSET_D");
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
            sb.AppendLine(" from TB_S_M_SALARYSET_D");
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
    public int GetCount_TB_S_M_SALARYSET_H()
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("Select COUNT(*) total_record");
            sb.AppendLine(" from TB_S_M_SALARYSET_H");
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

    //取得EXCEL的資料
    public DataTable getExcelResultData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(@"select a.LEVEL_CD,a.GRADE_CD
            from TB_H_M_LEVEL_GRADE a                                                               
            left join VW_TB_H_M_LEVEL b on a.LEVEL_CD = b.LEVEL_CD                                   
            where a.IS_VALID='Y' and b.IS_UNION_MEMBER='Y'
            order by a.LEVEL_CD,a.GRADE_CD,b.ORDER_SEQ
            ");
            
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    //載入新增的資料
    public DataTable GetData_Add(int startRowIndex, int maximumRows, string sortExpression, string effect_ym, string level_cd)
    {
        try
        {
            
            if (sortExpression.Contains("ORDER_SEQ,LEVEL_CD,GRADE_CD"))
                sortExpression = sortExpression.Replace("ORDER_SEQ,LEVEL_CD,GRADE_CD", "b.ORDER_SEQ,a.LEVEL_CD,a.GRADE_CD");
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" Select * From");
            sb.AppendLine(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.AppendLine("     a.LEVEL_CD,a.GRADE_CD,b.ORDER_SEQ                                                  ");
            sb.AppendLine("from TB_H_M_LEVEL_GRADE a                                                               ");
            sb.AppendLine("left join VW_TB_H_M_LEVEL b on a.LEVEL_CD = b.LEVEL_CD                                     ");
            sb.AppendLine("where a.IS_VALID='Y' and b.IS_UNION_MEMBER='Y'");
            //sb.AppendLine("order by b.ORDER_SEQ,a.LEVEL_CD,a.GRADE_CD                                             ");
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
            sb.AppendLine("from TB_H_M_LEVEL_GRADE a                                                               ");
            sb.AppendLine("left join VW_TB_H_M_LEVEL b on a.LEVEL_CD = b.LEVEL_CD                                     ");
            sb.AppendLine("where a.IS_VALID='Y' and b.IS_UNION_MEMBER='Y'");

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

    //刪除 TB_S_M_SALARYSET_H(3A以下調薪金額主檔)/TB_S_M_SALARYSET_D(3A以下調薪金額明細檔)
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
                    select * from TB_S_M_SALARYSET_D
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

    //新增 3A以下調薪金額主檔
    public void Add_TB_S_M_SALARYSET_H(string EFFECT_YM)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("INSERT INTO TB_S_M_SALARYSET_H (EFFECT_YM,RELEASE_DT,RELEASE_BY,APPROVE_DT,APPROVE_BY,APPROVE_STATUS,");
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
            ht.Add("@FUNC_ID", "FB2SE010");
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void Add_TB_S_M_SALARYSET_D()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("INSERT INTO TB_S_M_SALARYSET_D (EFFECT_YM,LEVEL_CD,GRADE_CD,EXAMINE_A,EXAMINE_B,EXAMINE_C,EXAMINE_D,EXAMINE_E");
            sb.AppendLine("                               ,ABILITY_ADJ,LEVEL_ADJ,LEVEL_PAY_LOW,LEVEL_PAY_AVG,LEVEL_PAY_UP,APPROVE_MARK,ORDER_SEQ");
            sb.AppendLine("                               ,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.AppendLine(" Values (@EFFECT_YM,@LEVEL_CD,@GRADE_CD,@EXAMINE_A,@EXAMINE_B,@EXAMINE_C,@EXAMINE_D,@EXAMINE_E");
            sb.AppendLine("        ,@ABILITY_ADJ,@LEVEL_ADJ,@LEVEL_PAY_LOW,@LEVEL_PAY_AVG,@LEVEL_PAY_UP,@APPROVE_MARK,@ORDER_SEQ");
            sb.AppendLine("        ,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@EFFECT_YM", EFFECT_YM);
            ht.Add("@LEVEL_CD", LEVEL_CD);
            ht.Add("@GRADE_CD", GRADE_CD);
            ht.Add("@EXAMINE_A", EXAMINE_A);
            ht.Add("@EXAMINE_B", EXAMINE_B);
            ht.Add("@EXAMINE_C", EXAMINE_C);
            ht.Add("@EXAMINE_D", EXAMINE_D);
            ht.Add("@EXAMINE_E", EXAMINE_E);
            ht.Add("@ABILITY_ADJ", ABILITY_ADJ);
            ht.Add("@LEVEL_ADJ", LEVEL_ADJ);
            ht.Add("@LEVEL_PAY_LOW", LEVEL_PAY_LOW);
            ht.Add("@LEVEL_PAY_AVG", LEVEL_PAY_AVG);
            ht.Add("@LEVEL_PAY_UP", LEVEL_PAY_UP);
            ht.Add("@APPROVE_MARK", "N");
            ht.Add("@ORDER_SEQ", ORDER_SEQ);
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SE010");
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //檢核資格級數
    public int chklevelcd(string level_cd,string grade_cd)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@"
                IF EXISTS 
                (	select 1 from TB_H_M_LEVEL_GRADE  WHERE LEVEL_CD=@LEVEL_CD AND GRADE_CD=@GRADE_CD AND IS_VALID='Y' )
                  SELECT 1 AS resultCount
                ELSE
                  SELECT 0 AS resultCount

            ");
            //PK值
            ht.Add("@LEVEL_CD", level_cd);
            ht.Add("@GRADE_CD", grade_cd);

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
            sb.AppendLine(" from TB_S_M_SALARYSET_H");
            sb.AppendLine(" where EFFECT_YM=@EFFECT_YM");
            ht.Add("@EFFECT_YM", EFFECT_YM);

            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }

    }
    public void Update_TB_S_M_SALARYSET_D()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Update TB_S_M_SALARYSET_D ");
            sb.Append(" Set EXAMINE_A=@EXAMINE_A,EXAMINE_B=@EXAMINE_B,EXAMINE_C=@EXAMINE_C,EXAMINE_D=@EXAMINE_D,EXAMINE_E=@EXAMINE_E");
            sb.Append(" ,ABILITY_ADJ=@ABILITY_ADJ,LEVEL_ADJ=@LEVEL_ADJ,LEVEL_PAY_LOW=@LEVEL_PAY_LOW,LEVEL_PAY_AVG=@LEVEL_PAY_AVG,LEVEL_PAY_UP=@LEVEL_PAY_UP,");
            sb.Append(" UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID");
            sb.Append(" where EFFECT_YM+LEVEL_CD+GRADE_CD = @data_key");

            ht.Add("@EXAMINE_A", EXAMINE_A);
            ht.Add("@EXAMINE_B", EXAMINE_B);
            ht.Add("@EXAMINE_C", EXAMINE_C);
            ht.Add("@EXAMINE_D", EXAMINE_D);
            ht.Add("@EXAMINE_E", EXAMINE_E);
            ht.Add("@ABILITY_ADJ", ABILITY_ADJ);
            ht.Add("@LEVEL_ADJ", LEVEL_ADJ);
            ht.Add("@LEVEL_PAY_LOW", LEVEL_PAY_LOW);
            ht.Add("@LEVEL_PAY_AVG", LEVEL_PAY_AVG);
            ht.Add("@LEVEL_PAY_UP", LEVEL_PAY_UP);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SE010");
            ht.Add("@data_key", data_key);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }
    #endregion

    #region 修改資格B/U_下限_中數_上限
    public DataTable getDDL_Edit()
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("select distinct LEVEL_CD from TB_S_M_SALARYSET_D ");
            sb.AppendLine(" where EFFECT_YM=@EFFECT_YM");
            ht.Add("@EFFECT_YM", EFFECT_YM.Replace("/", ""));
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getEditText()
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("select distinct ABILITY_ADJ,LEVEL_PAY_LOW,LEVEL_PAY_AVG,LEVEL_PAY_UP from TB_S_M_SALARYSET_D ");
            sb.AppendLine(" where EFFECT_YM=@EFFECT_YM and LEVEL_CD=@LEVEL_CD");
            ht.Add("@EFFECT_YM", EFFECT_YM);
            ht.Add("@LEVEL_CD", LEVEL_CD);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public void Update_TB_S_M_SALARYSET_D_Edit()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Update TB_S_M_SALARYSET_D ");
            sb.Append(" Set ABILITY_ADJ=@ABILITY_ADJ,LEVEL_PAY_LOW=@LEVEL_PAY_LOW,LEVEL_PAY_AVG=@LEVEL_PAY_AVG,LEVEL_PAY_UP=@LEVEL_PAY_UP,");
            sb.Append(" UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID");
            sb.Append(" where EFFECT_YM=@EFFECT_YM and LEVEL_CD=@LEVEL_CD");

            ht.Add("@ABILITY_ADJ", ABILITY_ADJ.Replace(",",""));
            ht.Add("@LEVEL_PAY_LOW", LEVEL_PAY_LOW.Replace(",", ""));
            ht.Add("@LEVEL_PAY_AVG", LEVEL_PAY_AVG.Replace(",", ""));
            ht.Add("@LEVEL_PAY_UP", LEVEL_PAY_UP.Replace(",", ""));
            ht.Add("@EFFECT_YM", EFFECT_YM);
            ht.Add("@LEVEL_CD", LEVEL_CD);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SE010");
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }
    #endregion
    
}