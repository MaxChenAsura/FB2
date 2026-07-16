<%@ WebHandler Language="C#" Class="WFB2HB0600_ExportExcel" %>

using System;
using System.Web;

public class WFB2HB0600_ExportExcel : IHttpHandler
{
    public log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    //Service 物件
    private CFB2HB0600BO service = new CFB2HB0600BO();
    string emp_id = "";
    
    public void ProcessRequest(HttpContext context) 
    {
        if (context.Request.Form["emp_id"] != null)
        {
            emp_id = context.Request.Form["emp_id"].ToString();
            ExportExcel(context);
        }
    }
    private void ExportExcel(HttpContext context)
    {
        try
        {
            service.ExportExcel(emp_id);

        }
        catch (Exception ex)
        {
            //context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
    }

    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

}