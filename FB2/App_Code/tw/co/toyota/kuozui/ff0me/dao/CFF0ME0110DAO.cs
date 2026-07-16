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
/// CFF0ME0110 的摘要描述
/// </summary>
public class CFF0ME0110DAO : BaseDAO
{
    public string BILL_YM { get; set; }
    public string VENDOR_ID { get; set; }
    public string VENDOR_AREA { get; set; }
    public string SAP_INV_FLAG { get; set; }
    public string TRANS_FLAG { get; set; }
    public string CREATED_BY { get; set; }
    public string UPDATED_BY { get; set; }
    public string FUNC_ID { get; set; }

    public CFF0ME0110DAO()
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
                         from FI_TB_C_M_BILL_H A  with (nolock) 
                         where 1=1  and  TRANS_FLAG ='N'
                         and SAPINV_YM = @BillYm
                         --and T06TRM = @T06TRM
                        ");
            ht.Add("@BillYm", BILL_YM.Replace("/", ""));
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
    //從 ORACLE 傳票生成暫存檔 生成資料至 CQZK傳票生成檔_Invoice
    internal string exec_SP_DC2_TRANS()
    {
        try
        {
            string rtnMessage = "0";
            string spName = "FSP_MM_005_DC2_TRANS";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(spName);
            ht.Add("@p_YM", BILL_YM.Replace("/", ""));
            // ht.Add("@p_TRM", ACCOUNT_TRM);
            ht.Add("@p_VENDORID", "");
            ht.Add("@p_VENDORAREA", "");
            ht.Add("@p_SPINVFLAG", "");
            ht.Add("@p_USERID", CREATED_BY);
            ht.Add("@p_FUNCID", "CFF0ME0110");
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
            //return ex.Message;
            throw;
        }
    }
    //EXCEL下載資料-發票資料下載
    public DataTable getT060_INVOICE()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(@" SELECT T.*,B.SUB_DESC AS SAP_INV_FLAG_DESC FROM (
                        SELECT T2.DC2GUID, T2.VENDOR_ID, T2.VENDOR_AREA, T2.BILL_YM, T2.NUMBER_TIMES,
                               T1.INV_DT, T1.INV_NO, T1.SAP_TAX_CODE, '' SAP_DEDUCT_FLAG, 
	                           T1.INV_UNTAX_AMT UNTAX_AMT, T1.INV_TAX TAX, T1.INV_TOTAL_AMT TOTAL_AMT, T1.SAP_INV_FLAG, T2.TRANS_FLAG, T2.PRICE_SYMBOL
                        FROM   FI_TB_C_M_INVOICE T1 JOIN 
	                           FI_TB_C_M_BILL_H T2 ON T1.DC2GUID=T2.DC2GUID
                        WHERE  T2.SAPINV_YM=@BillYm 
                        UNION
                        SELECT T2.DC2GUID, T2.VENDOR_ID, T2.VENDOR_AREA, T2.BILL_YM, T2.NUMBER_TIMES,
                               T1.INV_DT, T1.INV_NO, T1.SAP_TAX_CODE, T1.SAP_DEDUCT_FLAG, 
	                           T1.UNTAX_AMOUNT UNTAX_AMT, T1.TAX, (T1.UNTAX_AMOUNT+T1.TAX) TOTAL_AMT, T2.SAP_INV_FLAG, T2.TRANS_FLAG, T2.PRICE_SYMBOL
                        FROM   FI_TB_C_M_DISCOUNTLIST T1 JOIN 
	                           FI_TB_C_M_BILL_H T2 ON T1.DC2GUID=T2.DC2GUID
                        WHERE  T2.SAPINV_YM=@BillYm  
                        )T left join 
                         TB_9_M_COMM_D B on B.SYS_CD='MM' and B.MAIN_CD='T06FLG' and B.IS_VALID ='Y' and T.SAP_INV_FLAG=B.SUB_CD 
                         WHERE 1=1                  
                        ");

            if (VENDOR_ID != "")
            {
                sb.Append(@" and T.VENDOR_ID = @VendorId  ");
                ht.Add("@VendorId", VENDOR_ID);
            }
            if (VENDOR_AREA != "")
            {
                sb.Append(@" and T.VENDOR_AREA = @VendorArea  ");
                ht.Add("@VendorArea", VENDOR_AREA);
            }

            if (SAP_INV_FLAG != "")
            {
                sb.Append(@" and T.SAP_INV_FLAG = @SpInvFlag  ");
                ht.Add("@SpInvFlag", SAP_INV_FLAG);
            }
            if (TRANS_FLAG != "")
            {
                sb.Append(@" and T.TRANS_FLAG = @TransFlag  ");
                ht.Add("@TransFlag", TRANS_FLAG);
            }
           

            sb.Append(@" order by T.INV_DT, T.INV_NO ");

            ht.Add("@BillYm", BILL_YM.Replace("/", ""));
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    

    
   

    
}