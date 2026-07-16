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
/// CFF0ME0500DAO 的摘要描述
/// </summary>
public class CFF0ME0510DAO : BaseDAO
{
    // 基本欄位

    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }


    public CFF0ME0510DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

   
  

    internal string exec_SP(string spName)
    {
        try
        {
            string rtnMessage = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(spName);
            ht.Add("@DT_EXEC_DT", DateTime.Now.ToString("yyyy/MM/dd"));
            dbConn.ExecuteSP(sb, ht, true);

            return rtnMessage;

        }
        catch (Exception ex)
        {
            throw;
        }
    }
    internal string exec_SP_IMPORT(string spName)
    {
        try
        {
            string rtnMessage = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(spName);
            ht.Add("@DT_EXEC_DT", DateTime.Now.ToString("yyyy-MM-dd"));
            ht.Add("@CH_FORCE_EXEC_FLAG", "Y");
            ht.Add("@CH_SERVER_NAME", "DC2CLIB");
            dbConn.ExecuteSP(sb, ht, true);

            return rtnMessage;

        }
        catch (Exception ex)
        {
            throw;
        }
    }
    

}