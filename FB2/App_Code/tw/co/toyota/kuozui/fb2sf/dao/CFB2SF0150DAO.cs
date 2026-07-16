using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;

/// <summary>
/// CFB2SF0150DAO 的摘要描述
/// </summary>
public class CFB2SF0150DAO : BaseDAO
{
    public string EMP_ID { get; set; }
    public string DOC_NO { get; set; }

    public CFB2SF0150DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public DataTable GetData(int startRowIndex, int maximumRows, string sortExpression, string approve_status)
    {
        try
        {
            if (sortExpression.Contains("EMP_ID"))
                sortExpression = sortExpression.Replace("EMP_ID", "a.EMP_ID");
            if (sortExpression.Contains("EMP_NAME"))
                sortExpression = sortExpression.Replace("EMP_NAME", "e.EMP_NAME");
            if (sortExpression.Contains("OPELATION_BY_NAME"))
                sortExpression = sortExpression.Replace("OPELATION_BY_NAME", "f.EMP_NAME");
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" Select * From");
            sb.AppendLine(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + ") As RowNumber,");
            sb.AppendLine("a.EMP_ID,replace(e.EMP_NAME,' ','') as EMP_NAME,a.DOC_NO,a.START_DT,a.AMOUNT,a.TOTAL_AMT");
            sb.AppendLine(",f.EMP_NAME AS OPELATION_BY_NAME,a.APP_REMARK,a.APPROVE_STATUS");
            sb.AppendLine(",a.EMP_ID+a.DOC_NO as qdatakey");
            sb.AppendLine("from TB_S_M_ARREARS_COURT_H a");
            //sb.AppendLine("left join TB_H_M_EMP b on b.EMP_ID = a.CREATED_BY");
            //sb.AppendLine("left join TB_H_R_HEAD_DEPT c on b.DEPT_NO=c.MNG_DEPT_NO");
            sb.AppendLine("left join VW_H_EMP_DATA e on e.EMP_ID=a.EMP_ID");
            sb.AppendLine("left join VW_H_EMP_DATA f on f.EMP_ID=a.CREATED_BY");
            sb.AppendLine("where /* c.EMP_ID=@EMP_ID and */ a.APPROVE_STATUS=@APPROVE_STATUS and SURE_YN='Y'");

            ht.Add("@EMP_ID", SessionHandle.Current.emp_id);
            ht.Add("@APPROVE_STATUS", approve_status);

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
    public int GetCount(int startRowIndex, int maximumRows, string approve_status)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select COUNT(*) total_record ");
            sb.AppendLine("from TB_S_M_ARREARS_COURT_H a");
            //sb.AppendLine("left join TB_H_M_EMP b on b.EMP_ID = a.CREATED_BY");
            //sb.AppendLine("left join TB_H_R_HEAD_DEPT c on b.DEPT_NO=c.MNG_DEPT_NO");
            sb.AppendLine("left join VW_H_EMP_DATA e on e.EMP_ID=a.EMP_ID");
            sb.AppendLine("left join VW_H_EMP_DATA f on f.EMP_ID=a.CREATED_BY");
            sb.AppendLine("where  /* c.EMP_ID=@EMP_ID and */ a.APPROVE_STATUS=@APPROVE_STATUS and SURE_YN='Y'");

            ht.Add("@EMP_ID", SessionHandle.Current.emp_id);
            ht.Add("@APPROVE_STATUS", approve_status);

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
    public DataTable GetData2(int startRowIndex, int maximumRows, string sortExpression, string EMP_ID, string DOC_NO)
    {
        try
        {
            if (sortExpression=="")
                sortExpression = "CHG_STATUS";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" Select * From");
            sb.AppendLine(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + ") As RowNumber,");
            sb.AppendLine("b.SUB_CD+'-'+b.SUB_DESC as CHG_STATUS,c.EMP_NAME,a.DOC_NO,d.SUB_CD+'-'+d.SUB_DESC as PAY_TARGET,a.CREDITOR");
            sb.AppendLine(",a.AMOUNT,a.TOTAL_AMT,a.MEMO,a.EFFECT_EDT,a.IS_VAILD,a.MEMODESC");
            sb.AppendLine(",a.EMP_ID+a.DOC_NO+CAST(a.SEQ AS varchar) as qdatakey2");
            sb.AppendLine("from TB_S_M_ARREARS_TARGET a");
            sb.AppendLine("left join TB_9_M_COMM_D b on b.SYS_CD='SA' and b.MAIN_CD='CHG_STATUS' and b.SUB_CD=a.CHG_STATUS");
            sb.AppendLine("left join VW_H_EMP_DATA c on c.EMP_ID=a.EMP_ID");
            sb.AppendLine("left join TB_9_M_COMM_D d on d.SYS_CD='SF' and d.MAIN_CD='PAY_TARGET' and d.IS_VALID='Y' and a.PAY_TARGET=d.SUB_CD");
            sb.AppendLine("where  a.EMP_ID=@EMP_ID and a.DOC_NO=@DOC_NO");

            sb.AppendLine(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.AppendLine(" AND CAST(@startRowIndex+@maximumRows as varchar) ");

            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@DOC_NO", DOC_NO);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public int GetCount2(int startRowIndex, int maximumRows, string EMP_ID, string DOC_NO)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("Select COUNT(*) total_record");
            sb.AppendLine("from TB_S_M_ARREARS_TARGET a");
            sb.AppendLine("left join TB_9_M_COMM_D b on b.SYS_CD='SA' and b.MAIN_CD='CHG_STATUS' and b.SUB_CD=a.CHG_STATUS");
            sb.AppendLine("left join VW_H_EMP_DATA c on c.EMP_ID=a.EMP_ID");
            sb.AppendLine("left join TB_9_M_COMM_D d on d.SYS_CD='SF' and d.MAIN_CD='PAY_TARGET' and d.IS_VALID='Y' and a.PAY_TARGET=d.SUB_CD");
            sb.AppendLine("where  a.EMP_ID=@EMP_ID and a.DOC_NO=@DOC_NO");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@DOC_NO", DOC_NO);
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

    #region Approve
    public void Update_TB_S_M_ARREARS_COURT_H(string appitem,string APP_REMARK)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("Update TB_S_M_ARREARS_COURT_H ");
            sb.AppendLine(" Set APPROVE_DT=GETDATE(),APPROVE_STATUS=@APPROVE_STATUS,APP_REMARK=@APP_REMARK,APPROVE_BY=@APPROVE_BY,");
            sb.AppendLine(" UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID");
            sb.AppendLine(" where EMP_ID+DOC_NO = @data_key");

            ht.Add("@data_key", appitem);
            ht.Add("@APPROVE_STATUS", "Y");
            ht.Add("@APP_REMARK", APP_REMARK);
            ht.Add("@APPROVE_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SF015");
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void Del_TB_S_M_ARREARS_TARGET(string appitem)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("delete from TB_S_M_ARREARS_TARGET  ");
            sb.AppendLine(" where CHG_STATUS=@CHG_STATUS and EMP_ID+DOC_NO = @data_key");

            ht.Add("@data_key", appitem);
            ht.Add("@CHG_STATUS", "D");
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void Update_TB_S_M_ARREARS_TARGET(string appitem)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("Update TB_S_M_ARREARS_TARGET ");
            sb.AppendLine(" Set CHG_STATUS=@CHG_STATUS,UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID");
            sb.AppendLine(" where EMP_ID+DOC_NO = @data_key");

            ht.Add("@data_key", appitem);
            ht.Add("@CHG_STATUS", "X");
            ht.Add("@APPROVE_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SF015");
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //取出總金額
    public int GET_TOTAMOUNT(string emp_id)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("select sum(a.AMOUNT) as TOTAMOUNT");
            sb.AppendLine("from TB_S_M_ARREARS_TARGET a");
            sb.AppendLine("left join TB_S_M_ARREARS_COURT_H b on a.EMP_ID=b.EMP_ID and a.DOC_NO=b.DOC_NO");
            sb.AppendLine("where a.EMP_ID=@EMP_ID and a.PAY_TARGET in ('B','C','D') and a.IS_VAILD='Y' AND b.APPROVE_STATUS='Y'");

            ht.Add("@EMP_ID", emp_id);
            DataTable dt = dbConn.QueryT(sb, ht, true);
            if (dt.Rows.Count > 0)
            {
                if (Convert.ToString(dt.Rows[0]["TOTAMOUNT"]) != "")
                    t = Convert.ToInt32(dt.Rows[0]["TOTAMOUNT"]);
            }
            return t;

        }
        catch (Exception)
        {

            throw;
        }

    }
    //找出須須異動債權比例的金額
    public DataTable GET_EACHAMOUNT(string emp_id)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("select a.EMP_ID,a.DOC_NO,a.SEQ,a.AMOUNT,a.EMP_ID+a.DOC_NO+CAST(a.SEQ AS varchar) as AMOUNTKEY");
            sb.AppendLine("from TB_S_M_ARREARS_TARGET a");
            sb.AppendLine("left join TB_S_M_ARREARS_COURT_H b on a.EMP_ID=b.EMP_ID and a.DOC_NO=b.DOC_NO");
            sb.AppendLine("where a.EMP_ID=@EMP_ID AND a.PAY_TARGET in ('B','C','D') and IS_VAILD='Y' AND b.APPROVE_STATUS='Y'");


            ht.Add("@EMP_ID", emp_id);

            return dbConn.QueryT(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }
    //異動[TB_S_M_ARREARS_TARGET 法扣分配對象檔] 之債權比例
    public void Update_RATIO(string amountkey, decimal ratio)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("Update TB_S_M_ARREARS_TARGET ");
            sb.AppendLine(" Set RATIO=@RATIO,UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID");
            sb.AppendLine(" where EMP_ID+DOC_NO+CAST(SEQ AS varchar) = @data_key");

            ht.Add("@data_key", amountkey);
            ht.Add("@RATIO", ratio);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SF015");
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion

    #region Reject
    public void Reject(string rejitem, string APP_REMARK)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Update TB_S_M_ARREARS_COURT_H ");
            sb.Append(" Set APPROVE_DT=GETDATE(),APPROVE_STATUS=@APPROVE_STATUS,APP_REMARK=@APP_REMARK,APPROVE_BY=@APPROVE_BY,");
            sb.Append(" UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID");
            sb.Append(" where EMP_ID+DOC_NO = @data_key");

            ht.Add("@data_key", rejitem);
            ht.Add("@APPROVE_STATUS", "B");
            ht.Add("@APP_REMARK", APP_REMARK);
            ht.Add("@APPROVE_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SF015");
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion

}