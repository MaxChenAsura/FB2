<%@ WebHandler Language="C#" Class="WFB2DD0100_GetEmpData" %>

using System;
using System.Web;

public class WFB2DD0100_GetEmpData : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        string EMP_ID = context.Request.QueryString["EMP_ID"];
        CFB2SB1100BO service = new CFB2SB1100BO();
        EMP_DATA json = new EMP_DATA();
        try
        {
            if (EMP_ID != "")
            {
                System.Data.DataTable dt = service.getEMP_STATUS(EMP_ID);
                //getEMP_STATUS
                if (dt.Rows.Count > 0)
                {
                    json.EMP_ID = dt.Rows[0]["EMP_ID"].ToString().Trim();
                    json.EMP_NAME = dt.Rows[0]["EMP_NAME"].ToString().Trim();
                    json.EMP_STATUS = dt.Rows[0]["EMP_STATUS"].ToString().Trim();
                    json.errMsg = "";
                }
                else
                {
                    json.errMsg = "輸入工號不存在!";
                }

            }
            else
            {
                json.errMsg = "此工號未存於人事主檔";

            }
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
        catch (Exception ex)
        {
            json.errMsg = ex.Message;
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

    private class EMP_DATA
    {
        public string EMP_NAME { get; set; }
        public string EMP_ID { get; set; }
        public string EMP_STATUS { get; set; }
        public string errMsg { get; set; }
        
    }

}