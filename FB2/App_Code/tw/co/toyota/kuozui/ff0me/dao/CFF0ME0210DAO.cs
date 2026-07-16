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
/// CFF0ME0210 的摘要描述
/// </summary>
public class CFF0ME0210DAO : BaseDAO
{
    public string BILL_YM { get; set; }
    public string VENDOR_ID { get; set; }
    public string VENDOR_AREA { get; set; }
    public string SAP_INV_FLAG { get; set; }
    public string TRANS_FLAG { get; set; }
    public string CREATED_BY { get; set; }
    public string UPDATED_BY { get; set; }
    public string FUNC_ID { get; set; }

    public CFF0ME0210DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    //檢查 CURRENCY 是否存在
    public DataTable check_INVOICE_H()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@"  if exists (
                            select 1 resultCount from FI_TB_C_M_BILL_H A  with (nolock) 
                            where 1=1  and  TRANS_FLAG ='Y'  
                            and SAPINV_YM =@BillYm      
                            and VENDOR_ID =@VendorId 
                            and VENDOR_AREA =@VendorArea 
                            and SAP_INV_FLAG =@SapInvFlag     
                        )
                            select 1 as resultCount
                        else
                            select 0  as resultCount
                        ");
            ht.Add("@BillYm", BILL_YM.Replace("/", ""));
            ht.Add("@VendorId", VENDOR_ID);
            ht.Add("@VendorArea", VENDOR_AREA);
            ht.Add("@SapInvFlag", SAP_INV_FLAG);

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    internal string exec_SP_D2C_TRANS()
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
            ht.Add("@p_VENDORID", VENDOR_ID);
            ht.Add("@p_VENDORAREA", VENDOR_AREA);
            ht.Add("@p_SPINVFLAG", SAP_INV_FLAG);
            ht.Add("@p_USERID", CREATED_BY);
            ht.Add("@p_FUNCID", "CFF0ME0210");
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
                         and SAPINV_YM =@BillYm      
                         and VENDOR_ID =@VendorId 
                         and VENDOR_AREA =@VendorArea  
                         and SAP_INV_FLAG =@SapInvFlag
                        ");
            ht.Add("@BillYm", BILL_YM.Replace("/", ""));
            ht.Add("@VendorId", VENDOR_ID);
            ht.Add("@VendorArea", VENDOR_AREA);
            ht.Add("@SapInvFlag", SAP_INV_FLAG);
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
    
    

    
   

    
}