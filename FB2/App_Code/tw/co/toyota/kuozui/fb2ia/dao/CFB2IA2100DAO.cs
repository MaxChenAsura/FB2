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
/// CFB2IA2100DAO 的摘要描述
/// </summary>
public class CFB2IA2100DAO : BaseDAO
{
    public string EMP_ID { get; set; }
    public string IDENTITY_KIND { get; set; }
    public string LICENSE_ID { get; set; }
    public string EMP_NAME { get; set; }
    public string BIRTH_DT { get; set; }
    public string TARGET_TYPE { get; set; }
    public string GINS_KIND { get; set; }
    public string INS_COND_AMT { get; set; }
    public string INS_ENTRY_DT { get; set; }
    public string INS_QUIT_DT { get; set; }
    public string edititem { get; set; }
    public string FAMILY_NAME { get; set; }
    public string FAMILY_BIRTH_DT { get; set; }
    public CFB2IA2100DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public DataTable emp(string EMP_ID)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.AppendLine(" Select EMP_ID,EMP_NAME,LICENSE_ID From TB_H_M_EMP");
        sb.AppendLine(" where EMP_ID=@EMP_ID");
        ht.Add("@EMP_ID", EMP_ID);
        return dbConn.Query(sb, ht);

    }
    //眷屬
    public DataTable id(string LICENSE_ID, string EMP_ID)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.AppendLine("select *, z.TARGET_TYPE +'-'+ e.SUB_DESC as TARGET_TYPE_DESC ");
        sb.AppendLine(" from (");
        sb.AppendLine(" Select a.FAMILY_NAME, a.FAMILY_RELATION,a.FAMILY_BIRTH_DT   ");
        sb.AppendLine(" ,case when a.FAMILY_RELATION ='1' then '2' ");
        sb.AppendLine("       when a.FAMILY_RELATION ='2' then '4' ");
        sb.AppendLine("       when a.FAMILY_RELATION ='3' then '3' end as TARGET_TYPE ");
        sb.AppendLine(" From TB_H_M_EMP_FAMILY a ");
        sb.AppendLine(" where a.FAMILY_LICENSE_ID=@LICENSE_ID");
        if (EMP_ID != "")
        {
            sb.AppendLine(" and a.EMP_ID=@EMP_ID");
            ht.Add("@EMP_ID", EMP_ID);
        }
        sb.AppendLine(" )z ");
        sb.AppendLine(" left join TB_9_M_COMM_D e on e.SYS_CD = 'IA' and e.MAIN_CD = 'TARGET_TYPE' and e.SUB_CD = z.TARGET_TYPE ");
        ht.Add("@LICENSE_ID", LICENSE_ID);

        return dbConn.Query(sb, ht);
    }
    //本人
    public DataTable id2(string LICENSE_ID, string EMP_ID)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.AppendLine("select *, z.TARGET_TYPE +'-'+ e.SUB_DESC as TARGET_TYPE_DESC ");
        sb.AppendLine(" from (");
        sb.AppendLine(" Select EMP_NAME,'1' as TARGET_TYPE,BIRTH_DT From TB_H_M_EMP ");
        sb.AppendLine(" where LICENSE_ID=@LICENSE_ID");
        if (EMP_ID != "")
        {
            sb.AppendLine(" and EMP_ID=@EMP_ID");
            ht.Add("@EMP_ID", EMP_ID);
        }
        sb.AppendLine(" )z ");
        sb.AppendLine(" left join TB_9_M_COMM_D e on e.SYS_CD = 'IA' and e.MAIN_CD = 'TARGET_TYPE' and e.SUB_CD = z.TARGET_TYPE ");
        ht.Add("@LICENSE_ID", LICENSE_ID);
        return dbConn.Query(sb, ht);
    }
    public DataTable GetData(int startRowIndex, int maximumRows, string sortExpression, string emp_id, string license_id)
    {
        try
        {
            if (sortExpression.Contains("EMP_ID"))
                sortExpression = sortExpression.Replace("EMP_ID", "a.EMP_ID");
            if (sortExpression.Contains("EMP_NAME"))
                sortExpression = sortExpression.Replace("EMP_NAME", "a.EMP_NAME");
            if (sortExpression.Contains("LICENSE_ID"))
                sortExpression = sortExpression.Replace("LICENSE_ID", "b.LICENSE_ID");
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" Select * From");
            sb.AppendLine(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.AppendLine("     a.EMP_ID,a.EMP_NAME,b.LICENSE_ID,c.SUB_DESC,b.INS_ENTRY_DT,b.INS_QUIT_DT,d.COMPANY_SNAME,e.DIV_DEPT_FULL_NAME");
            sb.AppendLine(" ,a.BIRTH_DT");            
            sb.AppendLine("from TB_H_M_EMP a                                                                                                  ");
            sb.AppendLine("LEFT JOIN TB_I_M_GROUP_TXN b ON a.EMP_ID = b.EMP_ID AND b.IDENTITY_KIND='1' and b.GINS_KIND='A'                    ");
            sb.AppendLine("LEFT JOIN TB_9_M_COMM_D c ON a.EMP_CD=c.SUB_CD and c.SYS_CD='HB' and c.MAIN_CD='EMP_CD'                            ");
            sb.AppendLine("LEFT JOIN TB_H_M_COMPANY d ON a.COMPANY_CD=d.COMPANY_CD                                                            ");
            sb.AppendLine("LEFT JOIN VW_H_EMP_DATA e ON a.EMP_ID = e.EMP_ID                                                                   ");
            sb.AppendLine("where 1=1                                                                                            ");
            if (emp_id != "")
            {
                sb.AppendLine("and a.EMP_ID = @emp_id                                                                                 ");
                ht.Add("@emp_id", emp_id);
            }
            if (license_id != "")
            {
                sb.AppendLine("and b.LICENSE_ID like  @license_id +'%'                                                                                 ");
                ht.Add("@license_id", license_id );
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
    public int GetCount(int startRowIndex, int maximumRows, string emp_id, string license_id)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("Select COUNT(*) total_record");
            sb.AppendLine("from TB_H_M_EMP a                                                                                                  ");
            sb.AppendLine("LEFT JOIN TB_I_M_GROUP_TXN b ON a.EMP_ID = b.EMP_ID AND b.IDENTITY_KIND='1' and b.GINS_KIND='A'                    ");
            sb.AppendLine("LEFT JOIN TB_9_M_COMM_D c ON a.EMP_CD=c.SUB_CD and c.SYS_CD='HB' and c.MAIN_CD='EMP_CD'                            ");
            sb.AppendLine("LEFT JOIN TB_H_M_COMPANY d ON a.COMPANY_CD=d.COMPANY_CD                                                            ");
            sb.AppendLine("LEFT JOIN VW_H_EMP_DATA e ON a.EMP_ID = e.EMP_ID                                                                   ");
            sb.AppendLine("where 1=1                                                                                            ");
            if (emp_id != "")
            {
                sb.AppendLine("and a.EMP_ID = @emp_id                                                                                 ");
                ht.Add("@emp_id", emp_id);
            }
            if (license_id != "")
            {
                sb.AppendLine("and b.LICENSE_ID like @license_id +'%'                                                                                            ");
                ht.Add("@license_id", license_id);
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
    //讀取PDF資料
    public DataTable pdf_data()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("select a.EMP_ID,c.EMP_NAME,b.FAMILY_NAME,b.FAMILY_BIRTH_DT,a.INS_ENTRY_DT,a.INS_QUIT_DT,d.SUB_DESC");
            sb.AppendLine(" from TB_I_M_GROUP_TXN a");
            sb.AppendLine(" left join TB_H_M_EMP_FAMILY b on a.EMP_ID= b.EMP_ID and a.LICENSE_ID=b.FAMILY_LICENSE_ID");
            sb.AppendLine(" left join TB_H_M_EMP c on a.EMP_ID= c.EMP_ID");
            sb.AppendLine(" left join TB_9_M_COMM_D d  on  d.SYS_CD='IA' and d.MAIN_CD='TARGET_TYPE' and a.TARGET_TYPE=d.SUB_CD");
            sb.AppendLine(" where a.IDENTITY_KIND='2' and a.TARGET_TYPE in ('3','4') and CONVERT(varchar(8), INS_QUIT_DT,112)='99991231' and a.GINS_KIND='A' ");
            sb.AppendLine(" and (((CAST(CONVERT(varchar(4), GETDATE(),112) as decimal(4,0)))-(CAST(CONVERT(varchar(4), b.FAMILY_BIRTH_DT, 112) as decimal(4,0))))=25");
            sb.AppendLine(" or ((CAST(CONVERT(varchar(4), GETDATE(),112) as decimal(4,0)))-(CAST(CONVERT(varchar(4), b.FAMILY_BIRTH_DT, 112) as decimal(4,0))))=85)");

            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }
    //Dtl
    public DataTable GetDtlData(int startRowIndex, int maximumRows, string sortExpression, string emp_id)
    {
        try
        {
            if (sortExpression.Contains("EMP_ID"))
                sortExpression = sortExpression.Replace("EMP_ID", "a.EMP_ID");
            if (sortExpression.Contains("LICENSE_ID"))
                sortExpression = sortExpression.Replace("LICENSE_ID", "a.LICENSE_ID");
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" Select * From");
            sb.AppendLine(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.AppendLine(" a.EMP_ID+a.IDENTITY_KIND+a.LICENSE_ID+a.GINS_KIND+convert(varchar(8), a.INS_ENTRY_DT,112) as qdatakey,");
            sb.AppendLine(" a.EMP_ID,a.IDENTITY_KIND,a.LICENSE_ID,a.TARGET_TYPE,a.GINS_KIND,a.INS_COND_AMT,a.INS_ENTRY_DT,a.INS_QUIT_DT,");
            sb.AppendLine(" case when a.IDENTITY_KIND='1' then  b.EMP_NAME");
            sb.AppendLine(" when a.IDENTITY_KIND='2'  then  c.FAMILY_NAME end 'EMP_NAME',");
            sb.AppendLine(" case when a.IDENTITY_KIND='1' then  b.BIRTH_DT");
            sb.AppendLine(" when a.IDENTITY_KIND='2'  then  c.FAMILY_BIRTH_DT end 'BIRTH_DT'");
            sb.AppendLine(" ,a.IDENTITY_KIND +'-'+ d.SUB_DESC as IDENTITY_KIND_DESC ");
            sb.AppendLine(" ,a.TARGET_TYPE +'-'+ e.SUB_DESC as TARGET_TYPE_DESC ");
            sb.AppendLine(" from TB_I_M_GROUP_TXN a ");
            sb.AppendLine(" left join TB_H_M_EMP b on a.EMP_ID=b.EMP_ID");
            sb.AppendLine(" left join TB_H_M_EMP_FAMILY c on a.EMP_ID=c.EMP_ID  and a.LICENSE_ID=c.FAMILY_LICENSE_ID");
            sb.AppendLine(" left join TB_9_M_COMM_D d on d.SYS_CD = 'IA' and d.MAIN_CD = 'IDENTITY_KIND' and d.SUB_CD = a.IDENTITY_KIND ");
            sb.AppendLine(" left join TB_9_M_COMM_D e on e.SYS_CD = 'IA' and e.MAIN_CD = 'TARGET_TYPE' and e.SUB_CD = a.TARGET_TYPE ");
            sb.AppendLine(" where a.EMP_ID=@emp_id");
            sb.AppendLine(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.AppendLine(" AND CAST(@startRowIndex+@maximumRows as varchar) ");

            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            ht.Add("@emp_id", emp_id);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public int GetDtlCount(int startRowIndex, int maximumRows, string emp_id)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("Select COUNT(*) total_record");
            sb.AppendLine(" from TB_I_M_GROUP_TXN a ");
            sb.AppendLine(" left join TB_H_M_EMP b on a.EMP_ID=b.EMP_ID");
            sb.AppendLine(" left join TB_H_M_EMP_FAMILY c on a.EMP_ID=c.EMP_ID  and a.LICENSE_ID=c.FAMILY_LICENSE_ID");
            sb.AppendLine(" where a.EMP_ID=@emp_id");
            ht.Add("@emp_id", emp_id);
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
    internal DataTable getExistData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("Select * from TB_I_M_GROUP_TXN ");
            sb.AppendLine(" where EMP_ID=@EMP_ID and IDENTITY_KIND=@IDENTITY_KIND and LICENSE_ID=@LICENSE_ID and GINS_KIND=@GINS_KIND and INS_ENTRY_DT=@INS_ENTRY_DT");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@IDENTITY_KIND", IDENTITY_KIND);
            ht.Add("@LICENSE_ID", LICENSE_ID);
            ht.Add("@GINS_KIND", GINS_KIND);
            ht.Add("@INS_ENTRY_DT", INS_ENTRY_DT);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
    public int CheckINS_ENTRY_DT()
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("Select COUNT(*) count");
            sb.AppendLine(" from TB_I_M_GROUP_TXN a ");
            sb.AppendLine(" where EMP_ID=@EMP_ID and IDENTITY_KIND=@IDENTITY_KIND and LICENSE_ID=@LICENSE_ID and GINS_KIND=@GINS_KIND ");
            sb.AppendLine(" and @INS_ENTRY_DT>=INS_ENTRY_DT and @INS_ENTRY_DT<=INS_QUIT_DT ");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@IDENTITY_KIND", IDENTITY_KIND);
            ht.Add("@LICENSE_ID", LICENSE_ID);
            ht.Add("@GINS_KIND", GINS_KIND);
            ht.Add("@INS_ENTRY_DT", INS_ENTRY_DT);
            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                t = (int)dt.Rows[0]["count"];
            }
            return t;

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
            sb.AppendLine("INSERT INTO TB_I_M_GROUP_TXN (EMP_ID,IDENTITY_KIND,LICENSE_ID,GINS_KIND,TARGET_TYPE,INS_ENTRY_DT,INS_QUIT_DT,INS_COND_AMT,");
            sb.AppendLine(" CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.AppendLine(" Values (@EMP_ID,@IDENTITY_KIND,@LICENSE_ID,@GINS_KIND,@TARGET_TYPE,@INS_ENTRY_DT,@INS_QUIT_DT,@INS_COND_AMT,");
            sb.AppendLine(" @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@IDENTITY_KIND", IDENTITY_KIND);
            ht.Add("@LICENSE_ID", LICENSE_ID.ToUpper());
            ht.Add("@GINS_KIND", GINS_KIND);
            ht.Add("@TARGET_TYPE", TARGET_TYPE);
            ht.Add("@INS_ENTRY_DT", INS_ENTRY_DT);
            if (INS_QUIT_DT == "" || INS_QUIT_DT == null)
                ht.Add("@INS_QUIT_DT", "99991213");
            else
                ht.Add("@INS_QUIT_DT", INS_QUIT_DT);
            if (INS_COND_AMT == "" || INS_COND_AMT == null)
                ht.Add("@INS_COND_AMT", "0");
            else
                ht.Add("@INS_COND_AMT", INS_COND_AMT);
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2IA210");
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void Update(string edititem)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("Update TB_I_M_GROUP_TXN ");
            sb.AppendLine(" Set INS_COND_AMT = @INS_COND_AMT,INS_QUIT_DT = @INS_QUIT_DT,UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID");
            sb.AppendLine(" where EMP_ID+IDENTITY_KIND+LICENSE_ID+GINS_KIND+convert(varchar(8), INS_ENTRY_DT,112) = @edititem");

            if (INS_COND_AMT == "" || INS_COND_AMT == null)
                ht.Add("@INS_COND_AMT", "0");
            else
                ht.Add("@INS_COND_AMT", INS_COND_AMT);
            if (INS_QUIT_DT == "")
                ht.Add("@INS_QUIT_DT", "99991213");
            else
                ht.Add("@INS_QUIT_DT", INS_QUIT_DT);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2IA210");
            ht.Add("@edititem", edititem);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }
    internal DataTable checkDelData(string IDENTITY_KIND, string LICENSE_ID, string GINS_KIND, string INS_ENTRY_DT, string EMP_ID)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine("select * from TB_I_R_GROUP_MONTH ");
            sb.AppendLine(" where SALARY_YM>=@INS_ENTRY_DT and EMP_ID=@EMP_ID and IDENTITY_KIND=@IDENTITY_KIND");
            sb.AppendLine(" and LICENSE_ID=@LICENSE_ID and GINS_KIND=@GINS_KIND");
            ht.Add("@INS_ENTRY_DT", INS_ENTRY_DT.Replace("/", "").Substring(0, 6));
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@IDENTITY_KIND", IDENTITY_KIND.Substring(0,1));
            ht.Add("@LICENSE_ID", LICENSE_ID);
            ht.Add("@GINS_KIND", GINS_KIND);
            return dbConn.Query(sb, ht);
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
            //寫log
            sb.Append(" update TB_I_M_GROUP_TXN set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2IA210' ");
            sb.Append(" where EMP_ID + IDENTITY_KIND + LICENSE_ID + GINS_KIND + convert(varchar(8), INS_ENTRY_DT,112) = @delitem; ");
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);

            sb.AppendLine(" Delete From TB_I_M_GROUP_TXN ");
            sb.AppendLine(" where EMP_ID + IDENTITY_KIND + LICENSE_ID + GINS_KIND + convert(varchar(8), INS_ENTRY_DT,112) = @delitem; ");
            ht.Add("@delitem", delitem);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }
}