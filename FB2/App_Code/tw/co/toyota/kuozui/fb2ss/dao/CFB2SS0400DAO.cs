using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;

/// <summary>
/// CFB2SQ0100DAO 的摘要描述
/// </summary>
public class CFB2SS0400DAO : BaseDAO
{


    public string EMP_ID { get; set; }
    public string SALARY_DT { get; set; }
    public string INCENTIVE_TYPE { get; set; }
    //共用欄位
    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }



    public CFB2SS0400DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    /*是否已轉傳薪資*/
    public DataTable chkIS_SEND()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@"select count(*) cnt 
                        from TB_S_M_INCENTIVE_PAY_H
                        where 1=1
                        and SALARY_DT = @SALARY_DT         
                        and INCENTIVE_TYPE = @INCENTIVE_TYPE         
                        and PRE_STATUS = 'Y' 

            ");
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@INCENTIVE_TYPE", INCENTIVE_TYPE);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //檢核 節金檔 P-節金檔是否已發薪,A-節金是否已存在
    public string checkFN_SS_CHK_FESTIVAL(string check_cd)
    {
        try
        {
            string result = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select dbo.FN_SS_CHK_FESTIVAL(@SALARY_DT,@INCENTIVE_TYPE,@CHECK_CD) as result;   ");

            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@INCENTIVE_TYPE", INCENTIVE_TYPE);
            ht.Add("@CHECK_CD", check_cd);      //P-節金檔是否已發薪,A-節金是否已存在
            DataTable dt = dbConn.Query(sb, ht, true);

            if (dt.Rows.Count > 0)
            {
                result = dt.Rows[0]["result"].ToString();
            }
            return result;
        }
        catch
        {
            throw;
        }
    }

    public bool checkNAME(string EMP_ID,string EMP_NAME )
    {

        try
        {
            bool b = true;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select count(*)cnt from TB_H_M_EMP  ");
            sb.AppendLine("  where  EMP_ID = @EMP_ID and EMP_NAME = @EMP_NAME        ");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@EMP_NAME", EMP_NAME);
            DataTable dt = dbConn.Query(sb, ht, true);
            
            if (dt.Rows.Count > 0)
	        {
		        if (dt.Rows[0]["cnt"].ToString() == "0")
	            {
		            b = false;
	            }
	        }

            return b;
        }
        catch
        {
            throw;
        }
    }

    //檢核 見習技術員上傳資料檢核
    public string checkFunction(string emp_id)
    {

        try
        {
            string result = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select dbo.FN_SS040_CHK(@EMP_ID,@SALARY_DT,@INCENTIVE_TYPE) as result;   ");

            ht.Add("@EMP_ID", emp_id);
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@INCENTIVE_TYPE", INCENTIVE_TYPE);    
            DataTable dt = dbConn.Query(sb, ht, true);

            if (dt.Rows.Count > 0)
            {
                result = dt.Rows[0]["result"].ToString();
            }

            return result;
        }
        catch
        {
            throw;
        }
    }
    
    //刪除期間工資遺激勵金主檔 /期間工資遺激勵金計算檔
    internal void deleteINCENTIVE_PAY(string tableName)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("delete from ");
            sb.Append( tableName );
            sb.Append(" where SALARY_DT = @SALARY_DT and INCENTIVE_TYPE = @INCENTIVE_TYPE ");

            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@INCENTIVE_TYPE", INCENTIVE_TYPE);            

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    
    //新增 期間工資遺激勵金主檔
    internal void insertINCENTIVE_PAY_H()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("INSERT INTO TB_S_M_INCENTIVE_PAY_H (SALARY_DT,INCENTIVE_TYPE,");
            sb.Append(" CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" Values (@SALARY_DT,@INCENTIVE_TYPE ");
            sb.Append(" ,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@INCENTIVE_TYPE", INCENTIVE_TYPE);            
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
    
    //新增 期間工資遺激勵金計算檔
    internal void insertINCENTIVE_PAY_D()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("INSERT INTO TB_S_M_INCENTIVE_PAY_D (EMP_ID,SALARY_DT,INCENTIVE_TYPE,");
            sb.Append(" CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" Values (@EMP_ID,@SALARY_DT,@INCENTIVE_TYPE");
            sb.Append(" ,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@INCENTIVE_TYPE", INCENTIVE_TYPE);
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

    //計算獎金
    internal void exec_SP_S_SS040()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_S_SS040");
            ht.Add("@p_SALARY_DT", SALARY_DT);
            ht.Add("@p_INCENTIVE_TYPE", INCENTIVE_TYPE);
            ht.Add("@p_EMP_ID", DBNull.Value);
            ht.Add("@p_UserID", CREATED_BY);
            ht.Add("@p_FuncID", "FB2SA160");
            dbConn.ExecuteSPT(sb, ht, true);

        }
        catch (Exception)
        {

            throw;
        }
    }

}