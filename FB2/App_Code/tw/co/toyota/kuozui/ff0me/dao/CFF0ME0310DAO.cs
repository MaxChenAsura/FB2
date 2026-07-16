using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
//using FB2.tw.co.toyota.kuozui.dao;
using FB2.tw.co.toyota.kuozui.dao;
using System.Text;
using System.Collections;

/// <summary>
/// CFF0ME0310 的摘要描述
/// </summary>
public class CFF0ME0310DAO : BaseDAO
{
    public string BILL_YM { get; set; }
    public string TRANS_FLAG { get; set; }
    public string CREATED_BY { get; set; }
    public string UPDATED_BY { get; set; }
    public string FUNC_ID { get; set; }
    public string LOG_DATE { get; set; }
    public string INVOICE_TYPE { get; set; }

    public CFF0ME0310DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
   
   
    public int getresultCount()
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" select COUNT(*) resultCount
                         from FI_TB_C_M_BILL_H   with (nolock) 
                         where 1=1  and  TRANS_FLAG in ('N','P')
                         and SAPINV_YM =@BillYm  
                        ");
            ht.Add("@BillYm", BILL_YM.Replace("/", ""));
            if(INVOICE_TYPE=="MM")
                sb.Append(" and SAP_INV_FLAG in ( select SUB_CD from TB_9_M_COMM_D where SYS_CD='MM' and MAIN_CD='T06FLG' and IS_VALID='Y' and CODE_VAL1 ='D5C') ");   
            else
                sb.Append(" and SAP_INV_FLAG in ( select SUB_CD from TB_9_M_COMM_D where SYS_CD='MM' and MAIN_CD='T06FLG' and IS_VALID='Y' and CODE_VAL1!='D5C') ");
            //ht.Add("@T06TRM", ACCOUNT_TRM);

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                t = (int)dt.Rows[0]["resultCount"];
            }
            return t;
        }
        catch (Exception)
        {

            throw;
        }
    }
    //發票主檔轉入
    internal string exec_SP_D2CT060_IN_OUT(string spName)
    {
        try
        {
            string rtnMessage = "";
            //string spName = "FSP_MM_005_D5CT060_TRANS";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(spName);
            if (INVOICE_TYPE == "MM")
            {
                ht.Add("@p_YM", BILL_YM);
            }
            else
            {
                ht.Add("@p_YYYYMM", BILL_YM);
                //ht.Add("@p_TRM", ACCOUNT_TRM);
            }

            ht.Add("@p_this_pro_seq", LOG_DATE);
            ht.Add("@p_USERID", CREATED_BY);
            dbConn.ExecuteSP(sb, ht, true);

            DataTable dt = new DataTable();
            dt = utilities.getSPLOGDT(spName, LOG_DATE);
            if (dt.Rows.Count > 0 && dt.Rows[0]["PROC_STATUS"].ToString() == "E")
            {
                rtnMessage = dt.Rows[0]["PROC_LOG"].ToString();
            }
            return rtnMessage;
        }
        catch (Exception ex)
        {
            //return ex.Message;
            throw;
        }
    }
    //MM TO SAP
    internal string exec_FSP_MM_TO_SAP(string spName, string datasource)
    {
        try
        {
            string rtnMessage = "";
            //string spName = "FSP_MM_TO_SAP";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(spName);

            if (INVOICE_TYPE == "MM")
                ht.Add("@p_MM_ID", datasource);
            else
                ht.Add("@p_FI_ID", datasource);

            dbConn.ExecuteSP(sb, ht, true);

            DataTable dt = new DataTable();
            dt = utilities.getSPLOGDESC(spName);
            if (dt.Rows.Count > 0 && dt.Rows[0]["PROC_STATUS"].ToString() == "E")
            {
                rtnMessage = dt.Rows[0]["PROC_LOG"].ToString();
            }
            return rtnMessage;

        }
        catch (Exception ex)
        {
            throw;
        }
    }

    
   

    
}