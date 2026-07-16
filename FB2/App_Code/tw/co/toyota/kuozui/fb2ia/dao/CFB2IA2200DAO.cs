using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
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
/// CFB2IA2200DAO 的摘要描述
/// </summary>
public class CFB2IA2200DAO : BaseDAO
{
    public string INS_DT { get; set; }
    public CFB2IA2200DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    //public bool RunProcSP_I_EMP_INCOMPANY( string INS_DT, bool OnTransaction)
    //{
    //    try
    //    {
    //        StringBuilder sb = new StringBuilder();
    //        Hashtable ht = new Hashtable();
    //        sb.Append("SP_I_EMP_INCOMPANY");
    //        //ht.Add("@pDesc", "WFB2IA220");
    //        ht.Add("@pDate", INS_DT);
    //        //ht.Add("@pUserID", SessionHandle.Current.emp_id);
    //        //ht.Add("@FUNC_ID", FUNCTION_ID);
    //        ht.Add("@pFuncID", "WFB2IA220");
    //        if (OnTransaction)
    //            dbConn.ExecuteSPT(sb, ht, true);
    //        else
    //            dbConn.ExecuteSP(sb, ht, true);
    //        return true;
    //    }
    //    catch (Exception)
    //    {
    //        return false;
    //        throw;
    //    }
    //}
    public string Execute(string INS_DT)
    {
        try
        {
            var Kcomp_no = "";
            string result = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select CODE_VAL1 from TB_9_M_PARAMETER where MAIN_CD='KZ_COMP_NO'");
            DataTable dt = dbConn.Query(sb, ht);
            foreach (DataRow dr in dt.Rows)
            {
                Kcomp_no = Convert.ToString(dr["CODE_VAL1"]);
            }
            sb.Clear();
            ht.Clear();
            dt.Clear();
            sb.Append("select sum(a.GFEES_SELF) as SELF_TOTAL from TB_I_R_GROUP_MONTH a");
            sb.Append(" where a.SALARY_YM=@SALARY_YM and a.COMPANY_CD=@COMPANY_CD");
            ht.Add("@SALARY_YM", INS_DT.Substring(0,6));
            ht.Add("@COMPANY_CD", Kcomp_no);
            dt = dbConn.Query(sb, ht);
            foreach (DataRow dr in dt.Rows)
            {
                result = Convert.ToString(dr["SELF_TOTAL"]);
            }
            return result;
        }
        catch (Exception)
        {

            throw;
        }
    }
    //excel下載資料table
    public DataTable getExcelData(string data, string INS_YM)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            if (data == "2200")
            {
                sb.Append(" select b.DEPT_NAME_20,b.DEPT_NAME_40,c.SUB_DESC as EMP_CD_NAME,b.PJOB_DESC,a.EMP_ID,d.EMP_NAME,d.REATION_NAME,d.BIRTH_DT,a.LICENSE_ID,b.JOIN_DT,");
                sb.Append(" ia.INS_ENTRY_DT as AENTER_DT,ib.INS_ENTRY_DT as BENTER_DT,ic.INS_ENTRY_DT as CENTER_DT, id.INS_ENTRY_DT as DENTER_DT,case when b.company_cd = 'K' then '國瑞' else '才庫' end as company_cd,");
                sb.Append(" null as AQUIT_DT,null as BQUIT_DT,null as CQUIT_DT,null as DQUIT_DT,ia.INS_COND_AMT");
                sb.Append(" from (select distinct t.EMP_ID,t.IDENTITY_KIND,t.LICENSE_ID from TB_I_M_GROUP_TXN t where CONVERT(VARCHAR(6),t.INS_ENTRY_DT,112) = @INS_YM) a");
                sb.Append(" left join VW_H_EMP_DATA b on a.EMP_ID= b.EMP_ID");
                sb.Append(" left join TB_9_M_COMM_D c on c.SYS_CD='HB' and c.MAIN_CD='EMP_CD' and b.EMP_CD=c.SUB_CD");
                sb.Append(" left join VW_H_EMP_FAMILY_NAME d on d.EMP_ID=a.EMP_ID and a.LICENSE_ID= d.LICENSE_ID and a.IDENTITY_KIND = d.IDENTITY_KIND");
                sb.Append(" left join TB_I_M_GROUP_TXN ia on CONVERT(VARCHAR(6),ia.INS_ENTRY_DT,112) = @INS_YM and ia.GINS_KIND='A' and ia.EMP_ID=a.EMP_ID and ia.LICENSE_ID=a.LICENSE_ID");
                sb.Append(" left join TB_I_M_GROUP_TXN ib on CONVERT(VARCHAR(6),ib.INS_ENTRY_DT,112) = @INS_YM and ib.GINS_KIND='B' and ib.EMP_ID=a.EMP_ID and ib.LICENSE_ID=a.LICENSE_ID");
                sb.Append(" left join TB_I_M_GROUP_TXN ic on CONVERT(VARCHAR(6),ic.INS_ENTRY_DT,112) = @INS_YM and ic.GINS_KIND='C' and ic.EMP_ID=a.EMP_ID and ic.LICENSE_ID=a.LICENSE_ID");
                sb.Append(" left join TB_I_M_GROUP_TXN id on CONVERT(VARCHAR(6),id.INS_ENTRY_DT,112) = @INS_YM and id.GINS_KIND='D' and id.EMP_ID=a.EMP_ID and id.LICENSE_ID=a.LICENSE_ID");
            }
            else if (data == "2201")
            {
                sb.Append(" select b.DEPT_NAME_20,b.DEPT_NAME_40,b.PJOB_DESC,a.EMP_ID,b.EMP_NAME ,d.EMP_NAME AS INS_NAME,d.REATION_NAME,a.LICENSE_ID,c.HR_CHG_DESC,b.LEAVE_DT,");
                sb.Append(" ia.INS_ENTRY_DT as AENTER_DT,ib.INS_ENTRY_DT as BENTER_DT,ic.INS_ENTRY_DT as CENTER_DT,id.INS_ENTRY_DT as DENTER_DT,case when b.company_cd = 'K' then '國瑞' else '才庫' end as company_cd,");
                sb.Append(" ia.INS_QUIT_DT as AQUIT_DT,ib.INS_QUIT_DT as BQUIT_DT,ic.INS_QUIT_DT as CQUIT_DT,id.INS_QUIT_DT as DQUIT_DT, c1.SUB_DESC as EMP_CD_NAME");
                sb.Append(" from (select distinct t.EMP_ID,t.IDENTITY_KIND,t.LICENSE_ID from TB_I_M_GROUP_TXN t where CONVERT(VARCHAR(6),t.INS_QUIT_DT,112) = @INS_YM) a");
                sb.Append(" left join VW_H_EMP_DATA b on a.EMP_ID= b.EMP_ID");
                sb.Append(" left join TB_H_M_HR_CHANGE_CODE c on b.LEAVE_REASON= c.HR_CHG_CD");
                sb.Append(" left join VW_H_EMP_FAMILY_NAME d on d.EMP_ID=a.EMP_ID AND a.LICENSE_ID= d.LICENSE_ID and a.IDENTITY_KIND = d.IDENTITY_KIND");
                sb.Append(" left join TB_I_M_GROUP_TXN ia on CONVERT(VARCHAR(6),ia.INS_QUIT_DT,112) = @INS_YM and ia.GINS_KIND='A' and ia.EMP_ID=a.EMP_ID and ia.LICENSE_ID=a.LICENSE_ID");
                sb.Append(" left join TB_I_M_GROUP_TXN ib on CONVERT(VARCHAR(6),ib.INS_QUIT_DT,112) = @INS_YM and ib.GINS_KIND='B' and ib.EMP_ID=a.EMP_ID and ib.LICENSE_ID=a.LICENSE_ID");
                sb.Append(" left join TB_I_M_GROUP_TXN ic on CONVERT(VARCHAR(6),ic.INS_QUIT_DT,112) = @INS_YM and ic.GINS_KIND='C' and ic.EMP_ID=a.EMP_ID and ic.LICENSE_ID=a.LICENSE_ID");
                sb.Append(" left join TB_I_M_GROUP_TXN id on CONVERT(VARCHAR(6),id.INS_QUIT_DT,112) = @INS_YM and id.GINS_KIND='D' and id.EMP_ID=a.EMP_ID and id.LICENSE_ID=a.LICENSE_ID");
                sb.Append(" left join TB_9_M_COMM_D c1 on c1.SYS_CD='HB' and c1.MAIN_CD='EMP_CD' and b.EMP_CD=c1.SUB_CD");
            }
            else if (data == "2203")
            {
                sb.Append(" select b.DEPT_NAME_20,b.DEPT_NAME_40,b.DEPT_NO,b.PJOB_DESC,a.EMP_ID,d.EMP_NAME as INS_NAME,d.REATION_NAME,a.LICENSE_ID,a.INS_COND_AMT,");
                sb.Append(" c.SUB_DESC as EMP_CD_NAME,e.COMPANY_SNAME,d.BIRTH_DT,b.EMP_CHG_DESC+isnull((b.TRANSFER_NATION_NAME+b.TRANSFER_COMPANY_NAME),'') as TRANS_ACTION,");
                sb.Append(" a.INS_ENTRY_DT as AENTER_DT,ib.INS_ENTRY_DT as BENTER_DT,ic.INS_ENTRY_DT as CENTER_DT,id.INS_ENTRY_DT as DENTER_DT, ");
                sb.Append(" a.INS_QUIT_DT as AQUIT_DT,ib.INS_QUIT_DT as BQUIT_DT,ic.INS_QUIT_DT as CQUIT_DT,id.INS_QUIT_DT as DQUIT_DT ,b.SALARY_ACCOUNT_NO,b.SALARY_ACCOUNT_BANK");
                sb.Append(" from  TB_I_M_GROUP_TXN a");
                sb.Append(" left join VW_H_EMP_DATA b on a.EMP_ID= b.EMP_ID");
                sb.Append(" left join TB_9_M_COMM_D c on c.SYS_CD='HB' and c.MAIN_CD='EMP_CD' and b.EMP_CD=c.SUB_CD");
                sb.Append(" left join VW_H_EMP_FAMILY_NAME d on d.EMP_ID=a.EMP_ID and a.LICENSE_ID= d.LICENSE_ID and a.IDENTITY_KIND = d.IDENTITY_KIND");
                sb.Append(" left join TB_H_M_COMPANY e on e.COMPANY_CD = b.COMPANY_CD");
                sb.Append(" left join TB_I_M_GROUP_TXN ib on ib.INS_QUIT_DT = '9999/12/31' and ib.GINS_KIND='B' and ib.EMP_ID=a.EMP_ID and ib.LICENSE_ID=a.LICENSE_ID");
                sb.Append(" left join TB_I_M_GROUP_TXN ic on ic.INS_QUIT_DT = '9999/12/31' and ic.GINS_KIND='C' and ic.EMP_ID=a.EMP_ID and ic.LICENSE_ID=a.LICENSE_ID");
                sb.Append(" left join TB_I_M_GROUP_TXN id on id.INS_QUIT_DT = '9999/12/31' and id.GINS_KIND='D' and id.EMP_ID=a.EMP_ID and id.LICENSE_ID=a.LICENSE_ID");
                sb.Append(" where a.INS_QUIT_DT = '9999/12/31' and a.GINS_KIND='A'");
            }

            ht.Add("@INS_YM", INS_YM);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
}