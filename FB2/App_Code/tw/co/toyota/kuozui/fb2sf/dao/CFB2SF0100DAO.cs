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
/// CFB2SF0100DAO 的摘要描述
/// </summary>
public class CFB2SF0100DAO : BaseDAO
{
    public string EMP_ID { get; set; }
    public string DOC_NO { get; set; }
    public string data_key { get; set; }
    public string START_DT { get; set; }
    public string SALARY_RATE { get; set; }
    public string BONUS_RATE { get; set; }
    //Dtl
    public string CHG_STATUS { get; set; }
    public string PAY_TARGET { get; set; }
    public string CREDITOR { get; set; }
    public string VENDOR_ID { get; set; }
    public string AMOUNT { get; set; }
    public string RATIO { get; set; }
    public string EFFECT_SDT { get; set; }
    public string EFFECT_EDT { get; set; }
    public string IS_VAILD { get; set; }
    public string MEMO { get; set; }
    public string MEMODESC { get; set; }

    public CFB2SF0100DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public DataTable emp(string EMP_ID)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.Append(" Select EMP_ID,EMP_NAME,LICENSE_ID From TB_H_M_EMP");
        sb.Append(" where EMP_ID=@EMP_ID");
        ht.Add("@EMP_ID", EMP_ID);
        return dbConn.Query(sb, ht);

    }
    //主檔
    public DataTable GetData(int startRowIndex, int maximumRows, string sortExpression, string emp_id, string start_sdt, string start_edt, string doc_no)
    {
        try
        {
            if (sortExpression=="")
                sortExpression = "a.EMP_ID";
            if (sortExpression.Contains("EMP_ID"))
                sortExpression = sortExpression.Replace("EMP_ID", "a.EMP_ID");
            if (sortExpression.Contains("EMP_NAME"))
                sortExpression = sortExpression.Replace("EMP_NAME", "b.EMP_NAME");
            if (sortExpression.Contains("APPROVE_STATUS_DESC"))
                sortExpression = sortExpression.Replace("APPROVE_STATUS_DESC", "a.APPROVE_STATUS");
            if (sortExpression.Contains("APPROVE_BY_NAME"))
                sortExpression = sortExpression.Replace("APPROVE_BY_NAME", "d.EMP_NAME");
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" Select * From");
            sb.AppendLine(" (Select ROW_NUMBER() OVER(order by " + sortExpression + ") As RowNumber,");
            sb.AppendLine("a.EMP_ID,b.EMP_NAME,a.DOC_NO,a.START_DT,a.SALARY_RATE,a.BONUS_RATE,a.AMOUNT,a.TOTAL_AMT,a.SURE_YN");
            sb.AppendLine(",a.APPROVE_STATUS+'-'+c.SUB_DESC as APPROVE_STATUS_DESC,a.APPROVE_DT,d.EMP_NAME as APPROVE_BY_NAME,a.APP_REMARK");
            sb.AppendLine(",a.EMP_ID+a.DOC_NO as qdatakey");
            sb.AppendLine("from TB_S_M_ARREARS_COURT_H a");
            sb.AppendLine("left join VW_H_EMP_DATA b on a.EMP_ID=b.EMP_ID");
            sb.AppendLine("left join TB_9_M_COMM_D c on SYS_CD='SA' and MAIN_CD='APPROVE_STATUS' and SUB_CD=a.APPROVE_STATUS");
            sb.AppendLine("left join TB_H_M_EMP d on d.EMP_ID=a.APPROVE_BY");
            sb.AppendLine("where 1=1");

            if (emp_id != "")
            {
                sb.Append(" and a.EMP_ID= @emp_id");
                ht.Add("@emp_id", emp_id);
            }
            if (start_sdt != "")
            {
                sb.Append(" and a.START_DT >=@START_SDT");
                ht.Add("@START_SDT", start_sdt);
            }
            if (start_edt != "")
            {
                sb.Append(" and a.START_DT <=@START_EDT");
                ht.Add("@START_EDT", start_edt);
            }
            if (doc_no != "")
            {
                sb.Append(" and a.DOC_NO like '%'+@DOC_NO+'%'");
                ht.Add("@DOC_NO", doc_no);
            }
            //sb.AppendLine("order by a.EMP_ID,a.START_DT desc");
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
    public int GetCount(int startRowIndex, int maximumRows, string emp_id, string start_sdt, string start_edt, string doc_no)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("Select COUNT(*) total_record ");
            sb.AppendLine("from TB_S_M_ARREARS_COURT_H a");
            sb.AppendLine("left join VW_H_EMP_DATA b on a.EMP_ID=b.EMP_ID");
            sb.AppendLine("left join TB_9_M_COMM_D c on SYS_CD='SA' and MAIN_CD='APPROVE_STATUS' and SUB_CD=a.APPROVE_STATUS");
            sb.AppendLine("left join TB_H_M_EMP d on d.EMP_ID=a.APPROVE_BY");
            sb.AppendLine("where 1=1");

            if (emp_id != "")
            {
                sb.Append(" and a.EMP_ID= @emp_id");
                ht.Add("@emp_id", emp_id);
            }
            if (start_sdt != "")
            {
                sb.Append(" and a.START_DT >=@START_SDT");
                ht.Add("@START_SDT", start_sdt);
            }
            if (start_edt != "")
            {
                sb.Append(" and a.START_DT <=@START_EDT");
                ht.Add("@START_EDT", start_edt);
            }
            if (doc_no != "")
            {
                sb.Append(" and a.DOC_NO like '%'+@DOC_NO+'%'");
                ht.Add("@DOC_NO", doc_no);
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
    //明細
    public DataTable GetData2(int startRowIndex, int maximumRows, string sortExpression, string EMP_ID)
    {
        try
        {
            if (sortExpression=="")
                sortExpression = "a.CHG_STATUS";
            if (sortExpression.Contains("CHG_STATUS_DESC"))
                sortExpression = sortExpression.Replace("CHG_STATUS_DESC", "a.CHG_STATUS");
            if (sortExpression.Contains("PAY_TARGET_DESC"))
                sortExpression = sortExpression.Replace("PAY_TARGET_DESC", "a.PAY_TARGET");
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" Select * From");
            sb.AppendLine(" (Select ROW_NUMBER() OVER(order by " + sortExpression + ") As RowNumber,");
            sb.AppendLine("a.EMP_ID,d.EMP_NAME,a.CHG_STATUS,a.CHG_STATUS+'-'+c.SUB_DESC as CHG_STATUS_DESC,a.DOC_NO,a.PAY_TARGET+'-'+b.SUB_DESC as PAY_TARGET_DESC,a.CREDITOR");
            sb.AppendLine(",a.VENDOR_ID,a.AMOUNT,a.TOTAL_AMT,a.RATIO,a.MEMO,a.EFFECT_SDT,a.EFFECT_EDT,a.IS_VAILD,a.MEMODESC");
            sb.AppendLine(",a.EMP_ID+a.DOC_NO+CONVERT(varchar(3),a.SEQ) as qdatakey2");
            sb.AppendLine("from TB_S_M_ARREARS_TARGET a");
            sb.AppendLine("left join TB_9_M_COMM_D b on b.SYS_CD='SF' and b.MAIN_CD='PAY_TARGET' and b.SUB_CD=a.PAY_TARGET");
            sb.AppendLine("left join TB_9_M_COMM_D c on c.SYS_CD='SA' and c.MAIN_CD='CHG_STATUS' and c.SUB_CD=a.CHG_STATUS");
            sb.AppendLine("left join VW_H_EMP_DATA d on a.EMP_ID=d.EMP_ID");
            sb.AppendLine("where a.EMP_ID=@EMP_ID and a.CHG_STATUS<>@CHG_STATUS");

            sb.AppendLine(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.AppendLine(" AND CAST(@startRowIndex+@maximumRows as varchar) ");

            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@CHG_STATUS", "D");
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public int GetCount2(int startRowIndex, int maximumRows, string EMP_ID)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("Select COUNT(*) total_record");
            sb.AppendLine("from TB_S_M_ARREARS_TARGET a");
            sb.AppendLine("left join TB_9_M_COMM_D b on b.SYS_CD='SF' and b.MAIN_CD='PAY_TARGET' and b.SUB_CD=a.PAY_TARGET");
            sb.AppendLine("left join TB_9_M_COMM_D c on c.SYS_CD='SA' and c.MAIN_CD='CHG_STATUS' and c.SUB_CD=a.CHG_STATUS");
            sb.AppendLine("where a.EMP_ID=@EMP_ID and a.CHG_STATUS<>@CHG_STATUS");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@CHG_STATUS", "D");
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
    //資料確認
    public int Get_TB_S_M_ARREARS_TARGET_Count()
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("select count(*) cnt from TB_S_M_ARREARS_TARGET a");
            sb.AppendLine("where a.PAY_TARGET=@PAY_TARGET and a.EMP_ID=@EMP_ID and a.DOC_NO=@DOC_NO and a.CHG_STATUS<>'D'");
            ht.Add("@PAY_TARGET", "E");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@DOC_NO", DOC_NO);
            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                t = (int)dt.Rows[0]["cnt"];
            }
            return t;

        }
        catch (Exception)
        {

            throw;
        }

    }
    public void Update_TB_S_M_ARREARS_COURT_H(string EMP_ID, string DOC_NO)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Update TB_S_M_ARREARS_COURT_H ");
            sb.Append(" Set SURE_YN = @SURE_YN,UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID");
            sb.Append(" where EMP_ID = @EMP_ID and DOC_NO=@DOC_NO");

            ht.Add("@SURE_YN", "Y");
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SF010");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@DOC_NO", DOC_NO);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }
    //gv_result新刪修
    internal DataTable getExistData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select * from TB_S_M_ARREARS_COURT_H");
            sb.Append(" where EMP_ID+DOC_NO = @data_key");
            ht.Add("@data_key", data_key);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
    public void Add()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("insert into TB_S_M_ARREARS_COURT_H (EMP_ID,DOC_NO,START_DT,AMOUNT,TOTAL_AMT,SALARY_RATE,BONUS_RATE,SURE_YN,");
            sb.AppendLine("                                    APPROVE_STATUS,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.AppendLine("values (@EMP_ID,@DOC_NO,@START_DT,@AMOUNT,@TOTAL_AMT,@SALARY_RATE,@BONUS_RATE,@SURE_YN,");
            sb.AppendLine("        @APPROVE_STATUS,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@DOC_NO", DOC_NO);
            ht.Add("@START_DT", START_DT);
            ht.Add("@AMOUNT", "0");
            ht.Add("@TOTAL_AMT", "0");
            if (SALARY_RATE == "")
            {
                ht.Add("@SALARY_RATE", DBNull.Value);
            }
            else
            {
                ht.Add("@SALARY_RATE", SALARY_RATE);
            }
            if (BONUS_RATE == "")
            {
                ht.Add("@BONUS_RATE", DBNull.Value);
            }
            else
            {
                ht.Add("@BONUS_RATE", BONUS_RATE);
            }
            ht.Add("@SURE_YN", "N");
            ht.Add("@APPROVE_STATUS", "N");
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SF010");
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void Update()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("Update TB_S_M_ARREARS_COURT_H ");
            sb.AppendLine("Set SALARY_RATE=@SALARY_RATE,BONUS_RATE=@BONUS_RATE,START_DT=@START_DT,");
            sb.AppendLine("    SURE_YN='N',APPROVE_STATUS='N',APPROVE_DT=@APPROVE_DT,APPROVE_BY='',APP_REMARK='',");
            sb.AppendLine("    UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID");
            sb.AppendLine(" where EMP_ID+DOC_NO = @data_key");

            if (SALARY_RATE == "")
                ht.Add("@SALARY_RATE", DBNull.Value);
            else
                ht.Add("@SALARY_RATE", SALARY_RATE);
            if (BONUS_RATE == "")
                ht.Add("@BONUS_RATE", DBNull.Value);
            else
                ht.Add("@BONUS_RATE", BONUS_RATE);
            ht.Add("@START_DT", START_DT);
            ht.Add("@APPROVE_DT", DBNull.Value);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SF010");
            ht.Add("@data_key", data_key);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }
    public void Delete(string delitem)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("Delete From TB_S_M_ARREARS_COURT_H ");
            sb.AppendLine(" where EMP_ID+DOC_NO = @delitem");
            ht.Add("@delitem", delitem);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void Delete2(string delitem)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("Delete From TB_S_M_ARREARS_TARGET ");
            sb.AppendLine(" where EMP_ID+DOC_NO = @delitem");
            ht.Add("@delitem", delitem);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //gv_result2新刪修
    internal DataTable getExistData_Dtl()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select * from TB_S_M_ARREARS_COURT_H");
            sb.Append(" where EMP_ID+DOC_NO = @data_key");
            ht.Add("@data_key", data_key);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
    public void Add_TB_S_M_ARREARS_TARGET()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("insert into TB_S_M_ARREARS_TARGET (EMP_ID,CHG_STATUS,DOC_NO,SEQ,PAY_TARGET,CREDITOR,VENDOR_ID,AMOUNT,RATIO");
            sb.AppendLine("                                    ,EFFECT_SDT,EFFECT_EDT,IS_VAILD,MEMO,MEMODESC,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.AppendLine("values (@EMP_ID,@CHG_STATUS,@DOC_NO");
            sb.AppendLine(",(select isnull(max(SEQ)+1,1)");
            sb.AppendLine("  from TB_S_M_ARREARS_TARGET");
            sb.AppendLine("  where EMP_ID=@EMP_ID and DOC_NO=@DOC_NO)");
            sb.AppendLine(",@PAY_TARGET,@CREDITOR,@VENDOR_ID,@AMOUNT,@RATIO,@EFFECT_SDT,@EFFECT_EDT,@IS_VAILD,@MEMO,@MEMODESC");
            sb.AppendLine(",@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@CHG_STATUS", CHG_STATUS);
            ht.Add("@DOC_NO", DOC_NO);
            ht.Add("@PAY_TARGET", PAY_TARGET);
            ht.Add("@CREDITOR", CREDITOR);
            ht.Add("@VENDOR_ID", VENDOR_ID);
            ht.Add("@AMOUNT", AMOUNT);
            ht.Add("@RATIO", RATIO);
            ht.Add("@EFFECT_SDT", EFFECT_SDT);
            if (EFFECT_EDT!="")
            ht.Add("@EFFECT_EDT", EFFECT_EDT);
            else
                ht.Add("@EFFECT_EDT", DBNull.Value);
            ht.Add("@IS_VAILD", IS_VAILD);
            ht.Add("@MEMO", MEMO);
            ht.Add("@MEMODESC", MEMODESC);
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SF010");
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void Update_TB_S_M_ARREARS_COURT_H()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("Update TB_S_M_ARREARS_COURT_H ");
            sb.AppendLine("Set AMOUNT=AMOUNT+CONVERT(varchar(7),@AMOUNT),SURE_YN=@SURE_YN,APPROVE_STATUS=@APPROVE_STATUS,APPROVE_DT=@APPROVE_DT,APPROVE_BY=@APPROVE_BY,");
            sb.AppendLine("    APP_REMARK='',UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID");
            sb.AppendLine(" where EMP_ID+DOC_NO = @data_key");

            ht.Add("@data_key", data_key);
            ht.Add("@AMOUNT", AMOUNT);
            ht.Add("@SURE_YN", "N");
            ht.Add("@APPROVE_STATUS", "N");
            ht.Add("@APPROVE_DT", DBNull.Value);
            ht.Add("@APPROVE_BY", "");
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SF010");
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void Update_TB_S_M_ARREARS_TARGET()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("Update TB_S_M_ARREARS_TARGET ");
            sb.AppendLine("Set CHG_STATUS=@CHG_STATUS,PAY_TARGET=@PAY_TARGET,CREDITOR=@CREDITOR,VENDOR_ID=@VENDOR_ID,AMOUNT=@AMOUNT");
            sb.AppendLine("    ,RATIO=@RATIO,EFFECT_EDT=@EFFECT_EDT,IS_VAILD=@IS_VAILD,MEMO=@MEMO,MEMODESC=@MEMODESC,");
            sb.AppendLine("    UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID");
            sb.AppendLine(" where EMP_ID+DOC_NO+CONVERT(varchar(3),SEQ) = @data_key");

            ht.Add("@CHG_STATUS", CHG_STATUS);
            ht.Add("@PAY_TARGET", PAY_TARGET);
            ht.Add("@CREDITOR", CREDITOR);
            ht.Add("@VENDOR_ID", VENDOR_ID);
            ht.Add("@AMOUNT", AMOUNT);
            ht.Add("@RATIO", RATIO);
            if (EFFECT_EDT != "")
                ht.Add("@EFFECT_EDT", EFFECT_EDT);
            else
                ht.Add("@EFFECT_EDT", DBNull.Value);
            ht.Add("@IS_VAILD", IS_VAILD);
            ht.Add("@MEMO", MEMO);
            ht.Add("@MEMODESC", MEMODESC);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SF010");
            ht.Add("@data_key", data_key);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }
    public void Update_TB_S_M_ARREARS_TARGET_Other()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("Update TB_S_M_ARREARS_TARGET ");
            sb.AppendLine("Set VENDOR_ID=@VENDOR_ID");
            sb.AppendLine("    ,MEMO=@MEMO,MEMODESC=@MEMODESC,");
            sb.AppendLine("    UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID");
            sb.AppendLine(" where EMP_ID+DOC_NO+CONVERT(varchar(3),SEQ) = @data_key");

            ht.Add("@VENDOR_ID", VENDOR_ID);
            if (EFFECT_EDT != "")
                ht.Add("@EFFECT_EDT", EFFECT_EDT);
            else
                ht.Add("@EFFECT_EDT", DBNull.Value);
            ht.Add("@IS_VAILD", IS_VAILD);
            ht.Add("@MEMO", MEMO);
            ht.Add("@MEMODESC", MEMODESC);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SF010");
            ht.Add("@data_key", data_key);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }
    public void Update_TB_S_M_ARREARS_COURT_H2()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("Update TB_S_M_ARREARS_COURT_H ");
            sb.AppendLine("Set AMOUNT=(select sum(AMOUNT) as AMOUNT from TB_S_M_ARREARS_TARGET where EMP_ID =@EMP_ID and DOC_NO=@DOC_NO)");
            sb.AppendLine("    ,SURE_YN=@SURE_YN,APPROVE_STATUS=@APPROVE_STATUS,APPROVE_DT=@APPROVE_DT,APPROVE_BY=@APPROVE_BY,");
            sb.AppendLine("    APP_REMARK='',UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID");
            sb.AppendLine(" where EMP_ID=@EMP_ID and DOC_NO = @DOC_NO");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@DOC_NO", DOC_NO);
            ht.Add("@SURE_YN", "N");
            ht.Add("@APPROVE_STATUS", "N");
            ht.Add("@APPROVE_DT", DBNull.Value);
            ht.Add("@APPROVE_BY", "");
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SF010");
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    internal int getExistData_Delete(string delitem)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select count(1) as cnt from TB_S_M_ALLOCATION_D");
            sb.Append(" where EMP_ID+DOC_NO+CONVERT(varchar(3),SEQ) = @delitem");
            ht.Add("@delitem", delitem);

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                t = (int)dt.Rows[0]["cnt"];
            }
            return t;
        }
        catch (Exception)
        {

            throw;
        }
    }
    public void Update_TB_S_M_ARREARS_TARGET_DEL(string delitem)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("Update TB_S_M_ARREARS_TARGET ");
            sb.AppendLine("Set CHG_STATUS=@CHG_STATUS");
            sb.AppendLine("    ,UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID");
            sb.AppendLine(" where EMP_ID+DOC_NO+CONVERT(varchar(3),SEQ) = @data_key");

            ht.Add("@CHG_STATUS", "D");
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SF010");
            ht.Add("@data_key", delitem);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }
    public void Delete_Dtl(string delitem)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("Delete From TB_S_M_ARREARS_TARGET ");
            sb.AppendLine(" where EMP_ID+DOC_NO+CONVERT(varchar(3),SEQ) = @delitem");
            ht.Add("@delitem", delitem);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void Update_TB_S_M_ARREARS_COURT_H3(string emp_id, string doc_no_item, string amountitem)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("Update TB_S_M_ARREARS_COURT_H ");
            sb.AppendLine("Set AMOUNT=AMOUNT-CONVERT(varchar(7),@AMOUNT),SURE_YN=@SURE_YN,APPROVE_STATUS=@APPROVE_STATUS,APPROVE_DT=@APPROVE_DT,APPROVE_BY=@APPROVE_BY,");
            sb.AppendLine("    APP_REMARK='',UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID");
            sb.AppendLine(" where EMP_ID=@EMP_ID and DOC_NO = @DOC_NO");

            ht.Add("@EMP_ID", emp_id);
            ht.Add("@DOC_NO", doc_no_item);
            ht.Add("@AMOUNT", amountitem.Replace(",", ""));
            ht.Add("@SURE_YN", "N");
            ht.Add("@APPROVE_STATUS", "N");
            ht.Add("@APPROVE_DT", DBNull.Value);
            ht.Add("@APPROVE_BY", "");
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SF010");
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    internal DataTable checkData(CFB2SF0100DAO dao)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@"Select count(*) r1 From TB_S_M_ARREARS_TARGET
                        where emp_id = @emp_id and DOC_NO = @DOC_NO
                        and PAY_TARGET <> 'E' and IS_VAILD = 'Y'");

            ht.Add("@emp_id", dao.EMP_ID);
            ht.Add("@DOC_NO", dao.DOC_NO);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
    internal DataTable checkData2(CFB2SF0100DAO dao)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@"Select count(*) r1 From TB_S_M_ARREARS_TARGET
                        where emp_id = @emp_id and DOC_NO = @DOC_NO
                        and PAY_TARGET = 'E' and IS_VAILD = 'Y'");

            ht.Add("@emp_id", dao.EMP_ID);
            ht.Add("@DOC_NO", dao.DOC_NO);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
}