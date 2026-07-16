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
/// CFB2SF1200DAO 的摘要描述
/// </summary>
public class CFB2SF1200DAO : BaseDAO
{
    public string SALARY_DT { get; set; }
    public string SALARY_TYPE { get; set; }
    public string PAY_KIND_ID { get; set; }
    public string EMP_ID { get; set; }
    public string DOC_NO { get; set; }
    public string data_key { get; set; }
    public string START_DT { get; set; }
    //法扣金額分配
    public string vnowPAY_KIND { get; set; }
    public string vnowemp_id { get; set; }
    public decimal vCurrentRemaining { get; set; }
    public string SEQ { get; set; }
    //Dtl
    public string CHG_STATUS { get; set; }
    public string PAY_TARGET { get; set; }
    public string CREDITOR { get; set; }
    public string VENDOR_ID { get; set; }
    public decimal AMOUNT { get; set; }
    public decimal RATIO { get; set; }
    public string EFFECT_SDT { get; set; }
    public string EFFECT_EDT { get; set; }
    public string IS_VAILD { get; set; }
    public string MEMO { get; set; }
    public string MEMODESC { get; set; }
    public CFB2SF1200DAO()
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
    public DataTable GetData(int startRowIndex, int maximumRows, string sortExpression, string salary_dt, string salary_type, string emp_id, string opelation_by,string sure_yn)
    {
        try
        {
            
            if (sortExpression.Contains("EMP_ID"))
                sortExpression = sortExpression.Replace("EMP_ID", "a.EMP_ID");
            if (sortExpression.Contains("EMP_NAME"))
                sortExpression = sortExpression.Replace("EMP_NAME", "b.EMP_NAME");
            if (sortExpression == "")
                sortExpression = "a.EMP_ID";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" Select * From");
            sb.AppendLine(" (Select ROW_NUMBER() OVER(order by " + sortExpression + ") As RowNumber,");
            sb.AppendLine("       a.SALARY_DT,a.SALARY_TYPE,a.EMP_ID,b.EMP_NAME,a.PAY_KIND as PAY_KIND_ID");
            sb.AppendLine("      ,case when a.PAY_KIND='9999' then '月薪' else c.SALARY_NAME end as PAY_KIND");
            sb.AppendLine("      ,a.ORG_AMT,a.DEBIT_AMT,a.SURE_YN,d.EMP_NAME as OPELATION_BY");
            sb.AppendLine("      ,CONVERT(varchar(8), a.SALARY_DT, 112)+a.SALARY_TYPE+a.PAY_KIND+a.EMP_ID as qdatakey");
            sb.AppendLine("from TB_S_M_ARREARS_COURT_D a");
            sb.AppendLine("left join VW_H_EMP_DATA b on a.EMP_ID=b.EMP_ID");
            sb.AppendLine("left join TB_S_M_SALARY_ITEM c on c.SALARY_ID=a.PAY_KIND");
            sb.AppendLine("left join TB_H_M_EMP d on d.EMP_ID=a.OPELATION_BY");
            sb.AppendLine("where 1=1 and a.SALARY_DT=@SALARY_DT and a.SALARY_TYPE=@SALARY_TYPE");
            ht.Add("@SALARY_DT", salary_dt);
            ht.Add("@SALARY_TYPE", salary_type);

            if (emp_id != "")
            {
                sb.Append(" and a.EMP_ID=@EMP_ID");
                ht.Add("@EMP_ID", emp_id);
            }
            if (opelation_by != "")
            {
                sb.Append(" and d.EMP_ID=@opelation_by");
                ht.Add("@opelation_by", opelation_by);
            }
            if (sure_yn != "" && sure_yn != "-1")
            {
                sb.Append(" and a.SURE_YN=@sure_yn");
                ht.Add("@sure_yn", sure_yn);
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
    public int GetCount(int startRowIndex, int maximumRows, string salary_dt, string salary_type, string emp_id, string opelation_by, string sure_yn)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("Select COUNT(*) total_record ");
            sb.AppendLine("from TB_S_M_ARREARS_COURT_D a");
            sb.AppendLine("left join VW_H_EMP_DATA b on a.EMP_ID=b.EMP_ID");
            sb.AppendLine("left join TB_S_M_SALARY_ITEM c on c.SALARY_ID=a.PAY_KIND");
            sb.AppendLine("left join TB_H_M_EMP d on d.EMP_ID=a.OPELATION_BY");
            sb.AppendLine("where 1=1 and a.SALARY_DT=@SALARY_DT and a.SALARY_TYPE=@SALARY_TYPE");
            ht.Add("@SALARY_DT", salary_dt);
            ht.Add("@SALARY_TYPE", salary_type);

            if (emp_id != "")
            {
                sb.Append(" and a.EMP_ID=@EMP_ID");
                ht.Add("@EMP_ID", emp_id);
            }
            if (opelation_by != "")
            {
                sb.Append(" and d.EMP_ID=@opelation_by");
                ht.Add("@opelation_by", opelation_by);
            }
            if (sure_yn != "" && sure_yn != "-1")
            {
                sb.Append(" and a.SURE_YN=@sure_yn");
                ht.Add("@sure_yn", sure_yn);
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
    public DataTable GetData2(int startRowIndex, int maximumRows, string sortExpression
        , string EMP_ID, string PAY_KIND, string SALARY_DT, string SALARY_TYPE)
    {
        try
        {

            if (sortExpression.Contains("DOC_NO"))
                sortExpression = sortExpression.Replace("DOC_NO", "a.DOC_NO");
            if (sortExpression.Contains("SEQ"))
                sortExpression = sortExpression.Replace("SEQ", "a.SEQ");
            if (sortExpression.Contains("PAY_TARGET"))
                sortExpression = sortExpression.Replace("PAY_TARGET", "c.sub_cd");
            if (sortExpression.Contains("CREDITOR"))
                sortExpression = sortExpression.Replace("CREDITOR", "d.CREDITOR");
            if (sortExpression.Contains("AMOUNT"))
                sortExpression = sortExpression.Replace("AMOUNT", "a.AMOUNT");
            if (sortExpression == "")
                sortExpression = "a.DOC_NO";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" Select * From");
            sb.AppendLine(" (Select ROW_NUMBER() OVER(order by " + sortExpression + ") As RowNumber,");
            sb.AppendLine("        a.DOC_NO,a.SEQ,c.sub_cd+'-'+c.sub_desc as PAY_TARGET,d.CREDITOR,");
            sb.AppendLine("        a.AMOUNT,a.DEPT_ACCT_ID,a.ACCT_ID,");
            sb.AppendLine("        CONVERT(varchar(8), a.SALARY_DT, 112)+a.SALARY_TYPE+a.PAY_KIND+a.EMP_ID+a.DOC_NO+CAST(a.SEQ as varchar) as qdatakey2,");
            sb.AppendLine("        CONVERT(varchar(8), a.SALARY_DT, 112)+a.SALARY_TYPE+a.PAY_KIND+a.EMP_ID as qdatakey3");
            sb.AppendLine("        from TB_S_M_ALLOCATION_D a");
            sb.AppendLine("        left join TB_S_M_ARREARS_TARGET b on a.EMP_ID=b.EMP_ID and a.DOC_NO=b.DOC_NO and a.SEQ=b.SEQ");
            sb.AppendLine("        left join TB_9_M_COMM_D c on c.sys_cd='SF' and c.MAIN_CD='PAY_TARGET' and c.IS_VALID='Y' and b.PAY_TARGET=c.sub_cd");
            sb.AppendLine("        left join TB_S_M_ARREARS_TARGET d on a.EMP_ID=d.EMP_ID and a.DOC_NO=d.DOC_NO and a.SEQ=d.SEQ");
            sb.AppendLine("        where a.EMP_ID=@EMP_ID and a.PAY_KIND=@PAY_KIND");
            sb.AppendLine("        and a.SALARY_DT=@SALARY_DT and a.SALARY_TYPE=@SALARY_TYPE");
            sb.AppendLine(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.AppendLine(" AND CAST(@startRowIndex+@maximumRows as varchar) ");

            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@PAY_KIND", PAY_KIND);

            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);


            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public int GetCount2(int startRowIndex, int maximumRows, string EMP_ID, string PAY_KIND, string SALARY_DT, string SALARY_TYPE)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("Select COUNT(*) total_record");
            sb.AppendLine("        from TB_S_M_ALLOCATION_D a");
            //sb.AppendLine("        left join TB_S_M_ARREARS_TARGET b on a.EMP_ID=b.EMP_ID and a.DOC_NO=b.DOC_NO and a.SEQ=b.SEQ");
            //sb.AppendLine("        left join TB_9_M_COMM_D c on c.sys_cd='SF' and c.MAIN_CD='PAY_TARGET' and c.IS_VALID='Y' and b.PAY_TARGET=c.sub_cd");
            //sb.AppendLine("        left join TB_S_M_ARREARS_TARGET d on a.EMP_ID=d.EMP_ID and a.DOC_NO=d.DOC_NO and a.SEQ=d.SEQ");
            sb.AppendLine("        where a.EMP_ID=@EMP_ID and a.PAY_KIND=@PAY_KIND");
            sb.AppendLine("        and a.SALARY_DT=@SALARY_DT and a.SALARY_TYPE=@SALARY_TYPE");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@PAY_KIND", PAY_KIND);
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);


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
    #region 法扣金額分配
    public int Check_TB_S_M_ARREARS_COURT_D(string SALARY_DT, string SALARY_TYPE)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("select count(1) as cnt from TB_S_M_ARREARS_COURT_D ");
            sb.AppendLine("where SALARY_DT=@SALARY_DT and SALARY_TYPE=@SALARY_TYPE");
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);
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
    public int Check_TB_S_M_ALLOCATION_D(string SALARY_DT, string SALARY_TYPE)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("select count(1) as cnt from TB_S_M_ALLOCATION_D ");
            sb.AppendLine("where SALARY_DT=@SALARY_DT and SALARY_TYPE=@SALARY_TYPE  and isnull(ACCT_ID,'')<>''");
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);
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
    public DataTable Get_del_data(string SALARY_DT, string SALARY_TYPE)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("select AA.SALARY_DT,AA.SALARY_TYPE,AA.PAY_KIND,AA.EMP_ID,AA.DOC_NO,AA.SEQ");
            sb.AppendLine("from (");
            sb.AppendLine("    select a.SALARY_DT,a.SALARY_TYPE,a.PAY_KIND,a.EMP_ID,b.DOC_NO,b.SEQ");
            sb.AppendLine("    from TB_S_M_ARREARS_COURT_D a");
            sb.AppendLine("    join TB_S_M_ARREARS_TARGET b on a.EMP_ID=b.EMP_ID and b.IS_VAILD='Y' and b.RATIO>0");
            sb.AppendLine("union ");
            sb.AppendLine("    SELECT a.SALARY_DT,a.SALARY_TYPE,a.PAY_KIND,a.EMP_ID,b.DOC_NO,b.SEQ");
            sb.AppendLine("    from TB_S_M_ARREARS_COURT_D a");
            sb.AppendLine("     join TB_S_M_ARREARS_TARGET b on a.EMP_ID=b.EMP_ID and b.PAY_TARGET='E'");
            sb.AppendLine(") AA");
            sb.AppendLine("where AA.SALARY_DT=@SALARY_DT and AA.SALARY_TYPE=@SALARY_TYPE");

            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);

            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {
            throw;
        }

    }
    public void Del_del_data(string salary_dt, string salary_type, string PAY_KIND, string EMP_ID, string DOC_NO, string SEQ)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //寫log
            sb.AppendLine(" update TB_S_M_ALLOCATION_D set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2SF120' ");
            sb.AppendLine(" where SALARY_DT=@SALARY_DT and SALARY_TYPE=@SALARY_TYPE and PAY_KIND=@PAY_KIND and EMP_ID=@EMP_ID ");
            sb.AppendLine(" and DOC_NO=@DOC_NO and SEQ=@SEQ; ");
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);

            sb.AppendLine(" Delete from TB_S_M_ALLOCATION_D ");
            sb.AppendLine(" where SALARY_DT=@SALARY_DT and SALARY_TYPE=@SALARY_TYPE and PAY_KIND=@PAY_KIND and EMP_ID=@EMP_ID ");
            sb.AppendLine(" and DOC_NO=@DOC_NO and SEQ=@SEQ; ");

            ht.Add("@SALARY_DT", salary_dt);
            ht.Add("@SALARY_TYPE", salary_type);
            ht.Add("@PAY_KIND", PAY_KIND);
            ht.Add("@EMP_ID", EMP_ID);
            if (DOC_NO != "")
                ht.Add("@DOC_NO", DOC_NO);
            else
                ht.Add("@DOC_NO", DBNull.Value);
            if (SEQ != "")
                ht.Add("@SEQ", SEQ);
            else
                ht.Add("@SEQ", DBNull.Value);
            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }

    }
    public DataTable Get_TB_S_M_ARREARS_COURT_D(string SALARY_DT, string SALARY_TYPE)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("select a.SALARY_DT,a.SALARY_TYPE,a.PAY_KIND,a.EMP_ID,b.DOC_NO,b.SEQ,b.PAY_TARGET,isnull(b.RATIO,0) as RATIO");
            sb.AppendLine("      ,a.DEBIT_AMT,isnull(b.AMOUNT,0) as AMOUNT,isnull((b.AMOUNT-b.TOTAL_AMT),0) as RAMOUNT");
            sb.AppendLine("from TB_S_M_ARREARS_COURT_D a");
            sb.AppendLine(" join TB_S_M_ARREARS_TARGET b on a.EMP_ID=b.EMP_ID and b.IS_VAILD='Y' and b.RATIO>0 ");
            sb.AppendLine("where a.SALARY_DT=@SALARY_DT and a.SALARY_TYPE=@SALARY_TYPE");
            sb.AppendLine("order by a.EMP_ID,a.PAY_KIND,b.PAY_TARGET,b.SEQ");

            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);

            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {
            throw;
        }

    }
    public void Update_SURE_YN()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("Update TB_S_M_ARREARS_COURT_D ");
            sb.AppendLine("Set SURE_YN=@SURE_YN,UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID");
            sb.AppendLine(" where SALARY_DT=@SALARY_DT and SALARY_TYPE=@SALARY_TYPE and PAY_KIND=@PAY_KIND and EMP_ID=@EMP_ID");

            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            ht.Add("@PAY_KIND", vnowPAY_KIND);
            ht.Add("@EMP_ID", vnowemp_id);
            ht.Add("@SURE_YN", "Y");
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SF120");
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable Get_TB_S_M_ARREARS_TARGET(string vnowemp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("select top 1 a.DOC_NO,a.SEQ from TB_S_M_ARREARS_TARGET a");
            sb.AppendLine("where  a.EMP_ID=@vnowemp_id and a.PAY_TARGET='E'");

            ht.Add("@vnowemp_id", vnowemp_id);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }

    }
    public void Add_TB_S_M_ALLOCATION_D(decimal vThisAmount)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("insert into TB_S_M_ALLOCATION_D (SALARY_DT,SALARY_TYPE,PAY_KIND,EMP_ID,DOC_NO,SEQ,AMOUNT");
            sb.AppendLine("                                    ,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.AppendLine("values (@SALARY_DT,@SALARY_TYPE,@PAY_KIND,@EMP_ID,@DOC_NO,@SEQ,@AMOUNT");
            sb.AppendLine(",@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            ht.Add("@PAY_KIND", vnowPAY_KIND);
            ht.Add("@EMP_ID", vnowemp_id);
            ht.Add("@DOC_NO", DOC_NO);
            ht.Add("@SEQ", SEQ);
            ht.Add("@AMOUNT", vThisAmount);
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SF120");
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void Add_TB_S_M_ALLOCATION_D2(string salary_dt, string salary_type, string PAY_KIND, string EMP_ID, string DOC_NO, string SEQ, decimal vCurrentAmount)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("insert into TB_S_M_ALLOCATION_D (SALARY_DT,SALARY_TYPE,PAY_KIND,EMP_ID,DOC_NO,SEQ,AMOUNT");
            sb.AppendLine("                                    ,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.AppendLine("values (@SALARY_DT,@SALARY_TYPE,@PAY_KIND,@EMP_ID,@DOC_NO,@SEQ,@AMOUNT");
            sb.AppendLine(",@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@SALARY_DT", salary_dt);
            ht.Add("@SALARY_TYPE", salary_type);
            ht.Add("@PAY_KIND", PAY_KIND);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@DOC_NO", DOC_NO);
            ht.Add("@SEQ", SEQ);
            ht.Add("@AMOUNT", vCurrentAmount);
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SF120");
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void Update_SURE_YN2(string SALARY_DT, string SALARY_TYPE, string vnowPAY_KIND, string vnowemp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("Update TB_S_M_ARREARS_COURT_D ");
            sb.AppendLine("Set SURE_YN=@SURE_YN,UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID");
            sb.AppendLine(" where SALARY_DT=@SALARY_DT and SALARY_TYPE=@SALARY_TYPE and PAY_KIND=@PAY_KIND and EMP_ID=@EMP_ID");

            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            ht.Add("@PAY_KIND", vnowPAY_KIND);
            ht.Add("@EMP_ID", vnowemp_id);
            ht.Add("@SURE_YN", "Y");
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SF120");
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //public DataTable Get_TB_S_M_ARREARS_TARGET2(string vnowemp_id)
    //{
    //    try
    //    {
    //        StringBuilder sb = new StringBuilder();
    //        Hashtable ht = new Hashtable();
    //        sb.AppendLine("select top 1 a.DOC_NO,a.SEQ from TB_S_M_ARREARS_TARGET a");
    //        sb.AppendLine("where  a.EMP_ID=@vnowemp_id and a.PAY_TARGET='E'");

    //        ht.Add("@vnowemp_id", vnowemp_id);
    //        return dbConn.Query(sb, ht);
    //    }
    //    catch (Exception)
    //    {

    //        throw;
    //    }

    //}
    public void Add_TB_S_M_ALLOCATION_D3(string SALARY_DT, string SALARY_TYPE, string vnowPAY_KIND, string vnowemp_id, decimal vThisAmount)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("insert into TB_S_M_ALLOCATION_D (SALARY_DT,SALARY_TYPE,PAY_KIND,EMP_ID,DOC_NO,SEQ,AMOUNT");
            sb.AppendLine("                                    ,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.AppendLine("values (@SALARY_DT,@SALARY_TYPE,@PAY_KIND,@EMP_ID,@DOC_NO,@SEQ,@AMOUNT");
            sb.AppendLine(",@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            ht.Add("@PAY_KIND", vnowPAY_KIND);
            ht.Add("@EMP_ID", vnowemp_id);
            ht.Add("@DOC_NO", DOC_NO);
            ht.Add("@SEQ", SEQ);
            ht.Add("@AMOUNT", vThisAmount);
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SF120");
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion

    //資料確認
    public DataTable Check_TB_S_M_ARREARS_COURT_D_AMT()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("SELECT a.DEBIT_AMT,sum(b.AMOUNT) as BAMOUNT");
            sb.AppendLine("FROM  TB_S_M_ARREARS_COURT_D a");
            sb.AppendLine("left join TB_S_M_ALLOCATION_D b on a.SALARY_DT = b.SALARY_DT and a.SALARY_TYPE=b.SALARY_TYPE and a.PAY_KIND=b.PAY_KIND and a.EMP_ID=b.EMP_ID");
            sb.AppendLine("where a.SALARY_DT =@SALARY_DT and a.SALARY_TYPE=@SALARY_TYPE and a.PAY_KIND=@PAY_KIND and a.EMP_ID=@EMP_ID");
            sb.AppendLine("GROUP BY a.DEBIT_AMT");

            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            ht.Add("@PAY_KIND", PAY_KIND_ID);
            ht.Add("@EMP_ID", EMP_ID);

            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }

    }
    public void Update_TB_S_M_ARREARS_COURT_D()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Update TB_S_M_ARREARS_COURT_D ");
            sb.Append(" Set SURE_YN = @SURE_YN,UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID");
            sb.Append(" where SALARY_DT = @SALARY_DT and SALARY_TYPE=@SALARY_TYPE and PAY_KIND=@PAY_KIND_ID and EMP_ID=@EMP_ID");

            ht.Add("@SURE_YN", "Y");
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SF120");
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            ht.Add("@PAY_KIND_ID", PAY_KIND_ID);
            ht.Add("@EMP_ID", EMP_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }
    #region gv_result2新刪修
    //新增
    internal DataTable getExistData_Dtl()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select * from TB_S_M_ALLOCATION_D");
            sb.Append(" where CONVERT(varchar(8),SALARY_DT, 112)+SALARY_TYPE+PAY_KIND+EMP_ID+DOC_NO+CAST(SEQ as varchar) = @data_key");
            ht.Add("@data_key", data_key);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
    public void NEW_TB_S_M_ALLOCATION_D()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("insert into TB_S_M_ALLOCATION_D (SALARY_DT,SALARY_TYPE,PAY_KIND,EMP_ID,DOC_NO,SEQ,AMOUNT,DEPT_ACCT_ID,ACCT_ID");
            sb.AppendLine("                                    ,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.AppendLine("values (@SALARY_DT,@SALARY_TYPE,@PAY_KIND,@EMP_ID,@DOC_NO,@SEQ,@AMOUNT,@DEPT_ACCT_ID,@ACCT_ID");
            sb.AppendLine(",@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            ht.Add("@PAY_KIND", PAY_KIND_ID);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@DOC_NO", DOC_NO);
            ht.Add("@SEQ", SEQ);
            ht.Add("@AMOUNT", AMOUNT);
            ht.Add("@DEPT_ACCT_ID", DBNull.Value);
            ht.Add("@ACCT_ID", DBNull.Value);
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SF120");
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void NEW_Update_TB_S_M_ARREARS_COURT_D()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("Update TB_S_M_ARREARS_COURT_D ");
            sb.AppendLine("Set SURE_YN=@SURE_YN,UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID");
            //sb.AppendLine(" where CONVERT(varchar(8), SALARY_DT, 112)+SALARY_TYPE+PAY_KIND = @data_key ");
            sb.AppendLine(" where CONVERT(varchar(8), SALARY_DT, 112) = @SALARY_DT and SALARY_TYPE = @SALARY_TYPE and PAY_KIND = @PAY_KIND and EMP_ID =@EMP_ID ");

            //ht.Add("@data_key", data_key);
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            ht.Add("@PAY_KIND", PAY_KIND_ID);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SURE_YN", "N");
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SF120");
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //修改
    public void Edit_TB_S_M_ALLOCATION_D()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("Update TB_S_M_ALLOCATION_D ");
            sb.AppendLine("Set AMOUNT=@AMOUNT,UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID");
            sb.AppendLine(" where CONVERT(varchar(8), SALARY_DT, 112)+SALARY_TYPE+PAY_KIND+EMP_ID+DOC_NO+CAST(SEQ as varchar) = @data_key");

            ht.Add("@AMOUNT", AMOUNT);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SF120");
            ht.Add("@data_key", data_key);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }
    public void Edit_TB_S_M_ARREARS_COURT_D(string HID_qdatakey3)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("Update TB_S_M_ARREARS_COURT_D ");
            sb.AppendLine("Set SURE_YN=@SURE_YN,UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID");
            sb.AppendLine(" where CONVERT(varchar(8), SALARY_DT, 112)+SALARY_TYPE+PAY_KIND+EMP_ID = @HID_qdatakey3");

            ht.Add("@HID_qdatakey3", HID_qdatakey3);
            ht.Add("@SURE_YN", "N");
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SF120");
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //刪除
    public void Delete_TB_S_M_ALLOCATION_D(string delitem)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //寫log
            sb.AppendLine(" update TB_S_M_ALLOCATION_D set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2SF120' ");
            sb.AppendLine(" where CONVERT(varchar(8), SALARY_DT, 112)+SALARY_TYPE+PAY_KIND+EMP_ID+DOC_NO+CAST(SEQ as varchar) = @delitem; ");
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);

            sb.AppendLine(" Delete From TB_S_M_ALLOCATION_D ");
            sb.AppendLine(" where CONVERT(varchar(8), SALARY_DT, 112)+SALARY_TYPE+PAY_KIND+EMP_ID+DOC_NO+CAST(SEQ as varchar) = @delitem; ");
            ht.Add("@delitem", delitem);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void Update_Del_TB_S_M_ARREARS_COURT_D(string qdatakey3_item)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("Update TB_S_M_ARREARS_COURT_D ");
            sb.AppendLine("Set SURE_YN=@SURE_YN,UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID");
            sb.AppendLine(" where CONVERT(varchar(8), SALARY_DT, 112)+SALARY_TYPE+PAY_KIND+EMP_ID = @qdatakey3");

            ht.Add("@qdatakey3", qdatakey3_item);
            ht.Add("@SURE_YN", "N");
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SF120");
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion

}