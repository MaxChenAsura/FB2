<%@ WebHandler Language="C#" Class="WFB2IB0300_CheckData" %>

using System;
using System.Web;
using System.Data;

public class WFB2IB0300_CheckData : IHttpHandler
{
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        string txt_YM = context.Request.QueryString["txt_YM"].Replace("/","");
            
        string result = "";
        CFB2IB0300BO service = new CFB2IB0300BO();
        try
        {           
            DataTable dt = service.checkData(txt_YM);
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