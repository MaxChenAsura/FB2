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
/// CFB2SF1100DAO 的摘要描述
/// </summary>
public class CFB2SF1100DAO : BaseDAO
{
    public string SALARY_DT { get; set; }
    public string SALARY_TYPE { get; set; }
    public string EMP_ID { get; set; }
    public string PAY_KIND { get; set; }
    public string TOT_AMOUNT { get; set; }
    public string AMOUNT3052 { get; set; }
	public CFB2SF1100DAO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    public DataTable Check_SALARY_CLOSED(string SALARY_DT, string SALARY_TYPE)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("select CLOSED_DT from TB_S_M_SALARY_PAY_H where SALARY_DT =@SALARY_DT and SALARY_TYPE=@SALARY_TYPE");
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable Check_TB_S_M_ALLOCATION_D(string SALARY_DT, string SALARY_TYPE)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("select count(1) cnt from TB_S_M_ALLOCATION_D where SALARY_DT =@SALARY_DT and SALARY_TYPE=@SALARY_TYPE");
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable Get_TempT3052(string SALARY_DT, string SALARY_TYPE)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
             sb.AppendLine("select t1.EMP_ID,t2.PAY_KIND,t2.TOT_AMOUNT,t1.AMOUNT3052 from");
             sb.AppendLine("(");
             sb.AppendLine("  select a.EMP_ID,a.PAY_KIND ,sum(a.AMOUNT) AMOUNT3052");
             sb.AppendLine("  from TB_S_S_SALARY_PAY a");
             sb.AppendLine("  left join TB_S_M_SALARY_PAY_H b on a.PAY_ID=b.PAY_ID");
             if (SALARY_TYPE == "A")
             {
                 sb.AppendLine("  left join TB_S_M_EMP_RESULT r on r.SALARY_DT = a.SALARY_DT and r.EMP_ID = a.EMP_ID");
             }
             else
             {
                 sb.AppendLine("  left join TB_S_M_EMP_RESULT_TMP r on r.SALARY_DT = a.SALARY_DT and r.EMP_ID = a.EMP_ID and r.SALARY_TYPE = a.SALARY_TYPE and r.PAY_KIND = a.PAY_KIND");
             }
             sb.AppendLine("  where a.SALARY_TYPE=@SALARY_TYPE and b.SALARY_DT =@SALARY_DT and a.SALARY_ID='1052' and r.COMPANY_CD = 'K'"); //淑萍說 要改成1052
             sb.AppendLine("  group by a.EMP_ID,a.PAY_KIND ");
             sb.AppendLine(")t1");
             sb.AppendLine("left join (");
             sb.AppendLine("  select a1.EMP_ID,a1.PAY_KIND,isnull(sum(a1.AMOUNT*a2.IS_PLUS),0) AS TOT_AMOUNT");
             sb.AppendLine("  from TB_S_S_SALARY_PAY a1");
             sb.AppendLine("  left join TB_S_M_SALARY_PAY_H b1 on a1.PAY_ID=b1.PAY_ID");
             sb.AppendLine("  left join TB_S_M_SALARY_ITEM a2 on a1.SALARY_ID=a2.SALARY_ID");
             sb.AppendLine("  where a2.IS_ARREARS='Y' and a1.SALARY_TYPE=@SALARY_TYPE and b1.SALARY_DT =@SALARY_DT and a1.SALARY_ID<>'1052' "); //IS_ARREARS 納入法扣
             sb.AppendLine("  and a2.PAY_OBJECT='E'");  //支付對象='員工'
             sb.AppendLine("  group by a1.EMP_ID,a1.PAY_KIND");
             sb.AppendLine(")  t2 on t1.EMP_ID=t2.EMP_ID and t1.PAY_KIND = t2.PAY_KIND");

            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void Del_TB_S_M_ARREARS_COURT_D(string SALARY_DT, string SALARY_TYPE)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine("Delete from TB_S_M_ARREARS_COURT_D  ");
            sb.AppendLine("where SALARY_TYPE=@SALARY_TYPE and SALARY_DT =@SALARY_DT");

            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void Add_TB_S_M_ARREARS_COURT_D()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine("insert into TB_S_M_ARREARS_COURT_D  ");
            sb.AppendLine("(SALARY_DT,SALARY_TYPE,PAY_KIND,EMP_ID,ORG_AMT,DEBIT_AMT,SURE_YN,OPELATION_BY,");
            sb.AppendLine("CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.AppendLine("values (@SALARY_DT,@SALARY_TYPE,@PAY_KIND,@EMP_ID,@ORG_AMT,@DEBIT_AMT,@SURE_YN,");
            sb.AppendLine("         (select top(1)CREATED_BY from TB_S_M_ARREARS_TARGET where EMP_ID=@EMP_ID and IS_VAILD='Y'),");
            sb.AppendLine("        @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            ht.Add("@PAY_KIND", PAY_KIND);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@ORG_AMT", TOT_AMOUNT);
            ht.Add("@DEBIT_AMT", AMOUNT3052);
            ht.Add("@SURE_YN", "N");
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SF110");
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    
}