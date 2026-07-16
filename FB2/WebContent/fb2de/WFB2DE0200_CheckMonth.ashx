<%@ WebHandler Language="C#" Class="WFB2DE0200_CheckMonth" %>

using System;
using System.Web;

public class WFB2DE0200_CheckMonth : IHttpHandler
{
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        string START_DT = context.Request.QueryString["START_DT"];
        string END_DT = context.Request.QueryString["END_DT"];
        CFB2DE0200BO service = new CFB2DE0200BO();
        try
        {
            string result = service.getCal(START_DT, END_DT);
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