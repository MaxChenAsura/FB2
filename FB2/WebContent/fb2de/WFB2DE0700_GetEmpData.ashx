<%@ WebHandler Language="C#" Class="WFB2DE0700_GetEmpData" %>

using System;
using System.Web;

public class WFB2DE0700_GetEmpData : IHttpHandler
{
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        string EMP_ID = context.Request.QueryString["EMP_ID"];
        CFB2DE0700BO service = new CFB2DE0700BO();
        EMP_DATA json = new EMP_DATA();
        try
        {
            if (EMP_ID != "")
            {
                System.Data.DataTable dt = service.getEMPData(EMP_ID);
                if (dt.Rows.Count > 0)
                {
                    json.EMP_ID = EMP_ID;   
                    json.EMP_NAME = dt.Rows[0]["EMP_NAME"].ToString();                 
                    json.DEPT_NAME = dt.Rows[0]["DEPT_NAME"].ToString();
                    json.PLANT_NAME = dt.Rows[0]["PLANT_NAME"].ToString();
                    json.errMsg = "";
                }
                else
                {
                    json.errMsg = "此工號未存於人事主檔";
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
        public string EMP_ID { get; set; } 
        public string EMP_NAME { get; set; }      
        public string DEPT_NAME { get; set; }
        public string PLANT_NAME { get; set; }
        public string errMsg { get; set; }
        
    }

}