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
/// CFB2SD1200DAO 的摘要描述
/// </summary>
public class CFB2SD1200DAO : BaseDAO
{
    public CFB2SD1200DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public DataTable chkHaveData(string remit_dt, string pay_kind, string SALARY_ACCOUNT_BANK, string pay_id)
    {
        try
        {
            //SALARY_ACCOUNT_BANK='050' 為台灣企銀的銀行代碼
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("SELECT count(*) CNT from (");
            sb.AppendLine("select a.EMP_ID ,isnull(sum(a.AMOUNT*a.IS_PLUS),0) as AMT ");
            sb.AppendLine("from TB_S_S_SALARY_PAY a ");
            sb.AppendLine("left join TB_S_M_SALARY_PAY_H b on a.PAY_ID=b.PAY_ID and a.SALARY_TYPE=b.SALARY_TYPE ");
            sb.AppendLine("left join TB_H_M_EMP c on a.EMP_ID=c.EMP_ID ");
            if (pay_kind.Equals("9999")) //月薪類
            {
                sb.AppendLine("left join TB_S_M_EMP_RESULT e on e.SALARY_YM=a.DATA_YM and e.EMP_ID=a.EMP_ID and e.SALARY_DT = a.SALARY_DT ");
            }
            else
            {
                sb.AppendLine("left join TB_S_M_EMP_RESULT_TMP e on e.SALARY_DT=a.SALARY_DT and e.EMP_ID=a.EMP_ID and e.PAY_KIND=a.PAY_KIND and e.SALARY_TYPE=a.SALARY_TYPE ");
            }
            sb.AppendLine("left join TB_S_M_SALARY_ITEM I on a.SALARY_ID = I.SALARY_ID ");
            //sb.AppendLine("where c.SALARY_ACCOUNT_BANK='050' and e.COMPANY_CD='K' and a.PAY_TYPE in ('Y','V')  and I.PAY_OBJECT = 'E' and isnull(c.SALARY_ACCOUNT_NO,'')<>''	");
            sb.AppendLine("where e.COMPANY_CD='K' and a.PAY_TYPE in ('Y','V')  and I.PAY_OBJECT = 'E' and isnull(e.SALARY_ACCOUNT_NO,'')<>''	");
            sb.AppendLine(" and b.REMIT_DT=@remit_dt and a.PAY_KIND=@pay_kind	and e.SALARY_ACCOUNT_BANK = @SALARY_ACCOUNT_BANK and a.PAY_ID = @PAY_ID ");
            sb.AppendLine(" and e.SALARY_PAY_METHOD = 'T' ");
            ht.Add("@remit_dt", remit_dt);
            ht.Add("@pay_kind", pay_kind);
            ht.Add("@SALARY_ACCOUNT_BANK", SALARY_ACCOUNT_BANK);
            ht.Add("@PAY_ID", pay_id);
            sb.AppendLine("group by a.EMP_ID ");
            sb.AppendLine(" having isnull(sum(a.AMOUNT*a.IS_PLUS),0)>0 ");
            sb.AppendLine(" )tt ");
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //取得文字檔第一行
    //PAY_TYPE in ('Y','V') 支付.代付 
    public DataTable TxtFirstLine(string remit_dt, string pay_kind, string SALARY_ACCOUNT_BANK)
    {
        try
        {
            //SALARY_ACCOUNT_BANK='050' 為台灣企銀的銀行代碼
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("SELECT count(*) CNT ,SUM(tt.AMT) as SREAL_AMT from (");
            sb.AppendLine("select a.EMP_ID ,isnull(sum(a.AMOUNT*a.IS_PLUS),0) as AMT ");
            sb.AppendLine("from TB_S_S_SALARY_PAY a ");
            sb.AppendLine("left join TB_S_M_SALARY_PAY_H b on a.PAY_ID=b.PAY_ID and a.SALARY_TYPE=b.SALARY_TYPE ");
            sb.AppendLine("left join TB_H_M_EMP c on a.EMP_ID=c.EMP_ID ");
            if (pay_kind.Equals("9999")) //月薪類
            {
                sb.AppendLine("left join TB_S_M_EMP_RESULT e on e.SALARY_YM=a.DATA_YM and e.EMP_ID=a.EMP_ID and e.SALARY_DT = a.SALARY_DT ");
            }
            else
            {
                sb.AppendLine("left join TB_S_M_EMP_RESULT_TMP e on e.SALARY_DT=a.SALARY_DT and e.EMP_ID=a.EMP_ID and e.PAY_KIND=a.PAY_KIND and e.SALARY_TYPE=a.SALARY_TYPE ");
            }
            sb.AppendLine("left join TB_S_M_SALARY_ITEM I on a.SALARY_ID = I.SALARY_ID ");
            //sb.AppendLine("where c.SALARY_ACCOUNT_BANK='050' and e.COMPANY_CD='K' and a.PAY_TYPE in ('Y','V')  and I.PAY_OBJECT = 'E' and isnull(c.SALARY_ACCOUNT_NO,'')<>''	");
            sb.AppendLine("where e.COMPANY_CD='K' and a.PAY_TYPE in ('Y','V')  and I.PAY_OBJECT = 'E' and isnull(e.SALARY_ACCOUNT_NO,'')<>''	");
            sb.AppendLine(" and b.REMIT_DT=@remit_dt and a.PAY_KIND=@pay_kind	and e.SALARY_ACCOUNT_BANK = @SALARY_ACCOUNT_BANK");
            sb.AppendLine(" and e.SALARY_PAY_METHOD = 'T' ");//T =銀行轉帳
            ht.Add("@remit_dt", remit_dt);
            ht.Add("@pay_kind", pay_kind);
            ht.Add("@SALARY_ACCOUNT_BANK", SALARY_ACCOUNT_BANK);
            sb.AppendLine("group by a.EMP_ID ");
            sb.AppendLine(" having isnull(sum(a.AMOUNT*a.IS_PLUS),0)>0 ");
            sb.AppendLine(" )tt ");
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //取得文字檔第二行(含)以後資料
    public DataTable TxtNoFirstLine(string remit_dt, string pay_kind,string pay_id)
    {
        try
        {
          //SALARY_ACCOUNT_BANK='050' 為台灣企銀的銀行代碼
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("select r.SALARY_ACCOUNT_BANK,r.SALARY_ACCOUNT_NO,r.EMP_ID,SUM(p.IS_PLUS * p.AMOUNT ) as REAL_AMT ");
            sb.AppendLine("from [dbo].[TB_S_M_SALARY_PAY_H] a ");
            sb.AppendLine("left join [dbo].[TB_S_S_SALARY_PAY] p on a.PAY_ID = p.PAY_ID  and a.SALARY_DT = p.SALARY_DT and a.SALARY_TYPE = p.SALARY_TYPE");
            if (pay_kind.Equals("9999")) //月薪類
                sb.AppendLine("left join [dbo].[TB_S_M_EMP_RESULT] r on a.SALARY_YM = r.SALARY_YM and p.EMP_ID = r.EMP_ID  and p.SALARY_DT = r.SALARY_DT");
            else
                sb.AppendLine("left join [dbo].[TB_S_M_EMP_RESULT_TMP] r on p.SALARY_DT = r.SALARY_DT and p.EMP_ID = r.EMP_ID and r.PAY_KIND=p.PAY_KIND and r.SALARY_TYPE=p.SALARY_TYPE ");
            sb.AppendLine("left join TB_H_M_EMP d on r.EMP_ID = d.EMP_ID ");
            sb.AppendLine("left join TB_S_M_SALARY_ITEM I on p.SALARY_ID = I.SALARY_ID ");
            sb.AppendLine(" where REMIT_DT =@remit_dt and r.SALARY_ACCOUNT_BANK='050' and a.PAY_KIND=@pay_kind and r.COMPANY_CD='K' and I.PAY_OBJECT = 'E' and p.PAY_TYPE  in ('Y','V')  and a.PAY_ID =@pay_id ");
            sb.AppendLine(" and r.SALARY_PAY_METHOD = 'T' ");
            //sb.AppendLine(" and d.SALARY_ACCOUNT_NO='310' ");
            //sb.AppendLine(" and substring(d.SALARY_ACCOUNT_NO,1,3) = '310' ");

            ht.Add("@remit_dt", remit_dt);
            ht.Add("@pay_kind", pay_kind);
            ht.Add("@pay_id", pay_id);
            sb.AppendLine("GROUP BY r.SALARY_ACCOUNT_BANK,r.SALARY_ACCOUNT_NO,r.EMP_ID	");
            sb.AppendLine(" having isnull(sum(p.AMOUNT*p.IS_PLUS),0)>0 ");
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //取得文字檔第二行(含)以後資料 822 中信
    public DataTable TxtNoFirstLine(string remit_dt, string pay_kind, string Bank_Id, string pay_id)
    {
        try
        {
            //SALARY_ACCOUNT_BANK='822' 為中國信託的銀行代碼
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine("select r.SALARY_ACCOUNT_BANK,r.SALARY_ACCOUNT_NO,r.EMP_ID,d.LICENSE_ID,SUM(p.IS_PLUS * p.AMOUNT ) as REAL_AMT ");
            sb.AppendLine("from [dbo].[TB_S_M_SALARY_PAY_H] a ");
            sb.AppendLine("left join [dbo].[TB_S_S_SALARY_PAY] p on a.PAY_ID = p.PAY_ID  and a.SALARY_DT = p.SALARY_DT and a.SALARY_TYPE = p.SALARY_TYPE");
            if (pay_kind.Equals("9999")) //月薪類
                sb.AppendLine("left join [dbo].[TB_S_M_EMP_RESULT] r on a.SALARY_YM = r.SALARY_YM and p.EMP_ID = r.EMP_ID  and p.SALARY_DT = r.SALARY_DT");
            else
                sb.AppendLine("left join [dbo].[TB_S_M_EMP_RESULT_TMP] r on p.SALARY_DT = r.SALARY_DT and p.EMP_ID = r.EMP_ID and r.PAY_KIND=p.PAY_KIND and r.SALARY_TYPE=p.SALARY_TYPE ");
            sb.AppendLine("left join TB_H_M_EMP d on r.EMP_ID = d.EMP_ID ");
            sb.AppendLine("left join TB_S_M_SALARY_ITEM I on p.SALARY_ID = I.SALARY_ID ");
            sb.AppendLine(" where REMIT_DT =@remit_dt and r.SALARY_ACCOUNT_BANK=@SALARY_ACCOUNT_BANK and a.PAY_KIND=@pay_kind and r.COMPANY_CD='K' and I.PAY_OBJECT = 'E' and p.PAY_TYPE  in ('Y','V') and  a.PAY_ID =@pay_id");
            sb.AppendLine(" and r.SALARY_PAY_METHOD = 'T' ");
            //sb.AppendLine(" and d.SALARY_ACCOUNT_NO='310' ");
            //sb.AppendLine(" and substring(d.SALARY_ACCOUNT_NO,1,3) = '310' ");

            ht.Add("@remit_dt", remit_dt);
            ht.Add("@pay_kind", pay_kind);
            ht.Add("@pay_id", pay_id);
            ht.Add("@SALARY_ACCOUNT_BANK", Bank_Id);
            sb.AppendLine("GROUP BY r.SALARY_ACCOUNT_BANK,r.SALARY_ACCOUNT_NO,r.EMP_ID,d.LICENSE_ID	");
            sb.AppendLine(" having isnull(sum(p.AMOUNT*p.IS_PLUS),0)>0 ");
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable paykind(string PAY_KIND)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.Append(" Select SALARY_ID,SALARY_NAME From VW_SALARYAND9999");
        sb.Append(" where SALARY_ID=@PAY_KIND");
        ht.Add("@PAY_KIND", PAY_KIND);
        return dbConn.Query(sb, ht);

    }
    public DataTable get_Salary_pay_H(string REMIT_DT,string PAY_KIND)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.Append(" Select count(*) as resultCount From TB_S_M_SALARY_PAY_H");
        sb.Append(" where REMIT_DT=@REMIT_DT and PAY_KIND=@PAY_KIND and isnull(TRANSCLASS,'') <>'' ");
        ht.Add("@REMIT_DT", REMIT_DT);
        ht.Add("@PAY_KIND", PAY_KIND);
        return dbConn.Query(sb, ht);

    }
    public void update_Salary_pay_H(string REMIT_DT, string PAY_KIND,string TRANSCLASS)
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.Append(" update TB_S_M_SALARY_PAY_H set TRANSCLASS=@TRANSCLASS");
        sb.Append(" where REMIT_DT=@REMIT_DT and PAY_KIND=@PAY_KIND");
        ht.Add("@TRANSCLASS", TRANSCLASS);
        ht.Add("@REMIT_DT", REMIT_DT);
        ht.Add("@PAY_KIND", PAY_KIND);
        dbConn.Execute(sb, ht);

    }

}