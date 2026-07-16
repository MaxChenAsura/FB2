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
public class CFB2SS0100DAO : BaseDAO
{
    public string SALARY_DT { get; set; }
    public string FIRED_TYPE { get; set; }
    public string FIRED_DT { get; set; }
    public string SALARY_YM { get; set; }
    public string EMP_ID { get; set; }
    public string SPECIAL_PAY { get; set; }
    public string OTHER_PAY { get; set; }
    public string RETENTION_YY { get; set; }
    public string RETENTION_MM { get; set; }
    public string RETENTION_DD { get; set; }
    public string YM { get; set; }
       
    //共用欄位
    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }



    public CFB2SS0100DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    //資遺費計算
    internal void SP_S_SS010()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_S_SS010");
            ht.Add("@p_SALARY_DT", SALARY_DT);
            ht.Add("@p_FIRED_TYPE", FIRED_TYPE);
            ht.Add("@p_EMP_ID", DBNull.Value);
            ht.Add("@p_USERID", SessionHandle.Current.emp_id);
            ht.Add("@p_FUNCID", "FB2SS010");
            dbConn.ExecuteSP(sb, ht, true);            
                
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable checkSP(string PROC_ID)
    {

        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select top (1) PROC_STATUS, PROC_LOG  ");
            sb.AppendLine("   from TB_H_R_SP_LOG                  ");
            sb.AppendLine("  where  PROC_ID = @PROC_ID            ");
            sb.AppendLine("  order by PROC_DT desc                ");
            ht.Add("@PROC_ID", PROC_ID);
            return dbConn.Query(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }

    /*是否已轉傳薪資*/
    public DataTable chkIS_SEND()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@"select count(*) cnt 
                        from TB_S_M_FIRED_PAY
                        where 1=1
                        and SALARY_DT = @SALARY_DT         
                        and FIRED_TYPE = @FIRED_TYPE         
                        and PRE_STATUS = 'Y' 
            ");
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@FIRED_TYPE", FIRED_TYPE);
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
            sb.AppendLine(" select dbo.FN_SS_CHK_FESTIVAL(@SALARY_DT,@FIRED_TYPE,@CHECK_CD) as result;   ");

            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@FIRED_TYPE", FIRED_TYPE);
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

    //檢核 上傳資料
    public string checkFN_SS010_CHK(string emp_id, string emp_name, string fired_dt)
    {

        try
        {
            string result = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select dbo.FN_SS010_CHK(@EMP_ID,@EMP_NAME,@FIRED_DT) as result;   ");

            ht.Add("@EMP_ID", emp_id);
            ht.Add("@EMP_NAME", emp_name);
            ht.Add("@FIRED_DT", fired_dt);
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



    #region EXCEL 上傳處理 資料
    //新增暫存檔
    internal void addTmpData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("INSERT INTO TB_S_S_FIRED_TEMP (SALARY_DT,FIRED_TYPE,EMP_ID,FIRED_DT,");
            sb.Append(" SPECIAL_PAY,OTHER_PAY,RETENTION_YY,RETENTION_MM,RETENTION_DD, ");
            sb.Append(" CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" Values (@SALARY_DT,@FIRED_TYPE,@EMP_ID,@FIRED_DT,");
            sb.Append(" @SPECIAL_PAY,@OTHER_PAY,@RETENTION_YY,@RETENTION_MM,@RETENTION_DD, ");
            sb.Append(" @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@FIRED_TYPE", FIRED_TYPE);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@FIRED_DT", FIRED_DT);

            ht.Add("@SPECIAL_PAY", SPECIAL_PAY);
            ht.Add("@OTHER_PAY", OTHER_PAY);
            ht.Add("@RETENTION_YY", RETENTION_MM);
            ht.Add("@RETENTION_MM", RETENTION_MM);
            ht.Add("@RETENTION_DD", RETENTION_DD);

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

    //刪除計算檔
    internal void deleteFiredPayTable(string tableName)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("delete from  ");
            sb.Append(tableName);
            sb.Append(" where 1=1 ");
            sb.Append(" and SALARY_DT = @SALARY_DT");
            sb.Append(" and FIRED_TYPE = @FIRED_TYPE");
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@FIRED_TYPE", FIRED_TYPE);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    #endregion
   

  
}