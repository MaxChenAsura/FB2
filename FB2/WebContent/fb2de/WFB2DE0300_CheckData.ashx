<%@ WebHandler Language="C#" Class="WFB2DE0300_CheckData" %>

using System;
using System.Web;

public class WFB2DE0300_CheckData : IHttpHandler
{
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        string MANAGER_YM = context.Request.QueryString["MANAGER_YM"].Replace("/","");
        string PLANT_CD = context.Request.QueryString["PLANT_CD"];
        CFB2DE0300BO service = new CFB2DE0300BO();
        try
        {
            string result = service.getCal(MANAGER_YM, PLANT_CD);
            context.Response.Write(result);
        }
        catch (Exception ex)
        {
            context.Response.Write(ex.Message);
        }
    }
 
    public bool IsReusable {
        get {
            return false;
        }
    }

}