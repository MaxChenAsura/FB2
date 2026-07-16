<%@ WebHandler Language="C#" Class="QueryServices" %>

using System;
using System.Web;

public class QueryServices : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        
        string qrystr = HttpContext.Current.Request.Params["qrystr"].ToString();  //查詢字串
        FB2.tw.co.toyota.kuozui.dao.DBConnector dbConn = new FB2.tw.co.toyota.kuozui.dao.DBConnector();
        
        //查詢資料
        System.Text.StringBuilder sb = new System.Text.StringBuilder();
        System.Collections.Hashtable ht = new System.Collections.Hashtable();
        sb.Append("Select EMP_NAME from VW_H_EMP_DATA where EMP_NAME like @EMP_NAME");
        ht.Add("@EMP_NAME", "%" + qrystr + "%");

        System.Data.DataTable dt = dbConn.Query(sb, ht);
        string json = "-1";

        if (dt.Rows.Count > 0)
        {
            json = Newtonsoft.Json.JsonConvert.SerializeObject(dt, Newtonsoft.Json.Formatting.Indented);
        }
        //回傳json格式結果
        context.Response.Write(json);
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}