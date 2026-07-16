<%@ WebHandler Language="C#" Class="WFB2IB0500_ChecData" %>

using System;
using System.Web;
using System.Data;

public class WFB2IB0500_ChecData : IHttpHandler
{
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        string txt_SALARY_YM = context.Request.QueryString["txt_SALARY_YM"].Replace("/", "");
            
        string result = "";
        CFB2IB0500BO service = new CFB2IB0500BO();
        try
        {
            DataTable dt = service.checkData(txt_SALARY_YM);
            if(dt.Rows.Count > 0){
                result = "Y";
            }
            
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