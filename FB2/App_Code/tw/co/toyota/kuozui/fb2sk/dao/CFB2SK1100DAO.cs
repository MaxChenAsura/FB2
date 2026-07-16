using FB2.tw.co.toyota.kuozui.dao;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

/// <summary>
/// CFB2SK1100DAO 的摘要描述
/// </summary>
public class CFB2SK1100DAO : BaseDAO
{

    public string YEAR { get; set; }
    public string EMP_ID { get; set; }
    public string PAYMENT_AMT { get; set; }
    public string LICENSE_ID { get; set; }
    public string EXCEED_183 { get; set; }
    public string SEQ { get; set; } 
    public string CREATED_BY { get; set; }
    public string UPDATED_BY { get; set; }
    public string FUNC_ID { get; set; }

    public CFB2SK1100DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
      
    #region EXCEL上傳
    public bool getPJOB(string pjob_cd)
    {
        try
        {
            bool b = false;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select * ");
            sb.Append(" from TB_H_M_PJOB");
            sb.Append(" where 1=1");

            if (pjob_cd != "")
            {
                sb.Append(" and PJOB_CD = @PJOB_CD ");
                ht.Add("@PJOB_CD", pjob_cd);
            }

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                b = true;
            }
            return b;
        }
        catch (Exception)
        {
            throw;
        }
    }
    /* 查 在共用代碼明細檔是否存在 */
    public bool getCOmm(string main_CD, string sub_CD)
    {
        try
        {
            bool b = false;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select * ");
            sb.Append(" from TB_9_M_COMM_D");
            sb.Append(" where 1=1");

            if (main_CD != "")
            {
                sb.Append(" and MAIN_CD = @MAIN_CD ");
                ht.Add("@MAIN_CD", main_CD);
            }
            if (sub_CD != "")
            {
                sb.Append(" and SUB_CD = @SUB_CD ");
                ht.Add("@SUB_CD", sub_CD);
            }


            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                b = true;
            }
            return b;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable getJPN_CD(string emp_id)
    {
        try
        {            
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select isnull(JPN_CD,'') JPN_CD");
            sb.Append(" from TB_H_M_EMP");
            sb.Append(" where 1=1");

            if (emp_id != "")
            {
                sb.Append(" and emp_id = @emp_id ");
                ht.Add("@emp_id", emp_id);
            }

            DataTable dt = dbConn.Query(sb, ht);
            
            return dt;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable getEMP_DURATION(string emp_id)
    {
        try
        {            
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select Convert(varchar,MAX(START_DT),111) START_DT, Convert(varchar,MAX(END_DT),111) END_DT,@YEAR + '/01/01' max_date, @YEAR + '/12/31' min_date");
            sb.Append(" from TB_H_M_EMP_DURATION");
            sb.Append(" where emp_id = @emp_id");

            ht.Add("@YEAR", YEAR);
            ht.Add("@emp_id", emp_id);

            DataTable dt = dbConn.Query(sb, ht);

            return dt;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable checkEMP(string emp_id,string license_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select emp_id from TB_H_M_EMP");
            sb.Append(" where emp_id = @emp_id and license_id = @license_id");
           
            ht.Add("@emp_id", emp_id);
            ht.Add("@license_id", license_id);

            DataTable dt = dbConn.Query(sb, ht);

            return dt;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void delMUTUAL_YEAR_DTL()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Delete from TB_S_R_MUTUAL_YEAR_DTL");
            sb.Append(" where YEAR = @YEAR  and LICENSE_ID = @LICENSE_ID");

            ht.Add("@YEAR", YEAR);
            ht.Add("@LICENSE_ID", LICENSE_ID);

            dbConn.ExecuteT(sb, ht);

        }
        catch (Exception)
        {
            throw;
        }
    }
        
    internal void addMUTUAL_YEAR_DTL()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("insert into TB_S_R_MUTUAL_YEAR_DTL ");
            sb.Append("(YEAR,EMP_ID,LICENSE_ID,EMP_NAME,JPN_CD,"+
                        "REGISTER_ADDR,PAYMENT_AMT,EXCEED_183,CREATED_BY,CREATED_DT," +
                        "UPDATED_BY,UPDATED_DT,FUNC_ID,MUTUAL_SEQ)");

            sb.Append(" select @YEAR,@EMP_ID,@LICENSE_ID,EMP_NAME,JPN_CD,");
            sb.Append("REGISTER_ADDR,@PAYMENT_AMT,@EXCEED_183,@CREATED_BY,getdate(),");
            sb.Append("@UPDATED_BY,getdate(),@FUNC_ID,@SEQ ");
            sb.Append(" from TB_H_M_EMP ");
            sb.Append(" where EMP_ID = @EMP_ID and LICENSE_ID = @LICENSE_ID");

            ht.Add("@LICENSE_ID", LICENSE_ID);
            ht.Add("@YEAR", YEAR);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@PAYMENT_AMT", PAYMENT_AMT);
            ht.Add("@EXCEED_183", EXCEED_183);
            ht.Add("@SEQ", SEQ);
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

    #endregion
}