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
public class CFB2SQ0100DAO : BaseDAO
{
    public string SALARY_YM { get; set; }
    public string EMP_ID { get; set; }
   
    //共用欄位
    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }
    public string SPECIAL_PAY { get; set; }
    public string OTHER_PAY { get; set; }


    public CFB2SQ0100DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    /* 本月無符合產假津貼的員工 */
    public DataTable chkMATERNITY_LEAVE()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@"select count(*) cnt 
                        from TB_D_M_LEAVE_APPLY
                        where CONVERT(varchar(6),APPLY_LEAVE_SDT,112) = @YM and FORM_STATUS not in ('N','D') 
                        and MAIN_LEAVE_CD = 'J' and SUB_LEAVE_CD <> 'J7'            
            ");
            if (EMP_ID != "")
            {
                sb.Append(" and EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", EMP_ID);
            }
            ht.Add("@YM", SALARY_YM);            

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
    /*本月已結案*/
    public DataTable chkIS_CLOSE()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@"select count(*) cnt 
                        from TB_S_M_MATERNITY_LEAVE_H
                        where SALARY_YM = @YM and IS_CLOSE = 'Y'           
            ");
            if (EMP_ID != "")
            {
                sb.Append(" and EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", EMP_ID);
            }
            ht.Add("@YM", SALARY_YM);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void SP_S_MATERNITY_COMPUTE()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_S_MATERNITY_COMPUTE");
            ht.Add("@p_SALARY_YM", SALARY_YM);
            ht.Add("@p_EMP_ID", EMP_ID);
            ht.Add("@p_SPECIAL_PAY", SPECIAL_PAY);
            ht.Add("@p_OTHER_PAY", OTHER_PAY);
            ht.Add("@p_USERID", SessionHandle.Current.emp_id);
            ht.Add("@p_FUNCID", "FB2SQ010");
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

}