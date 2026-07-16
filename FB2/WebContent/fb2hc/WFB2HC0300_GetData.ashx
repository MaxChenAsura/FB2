<%@ WebHandler Language="C#" Class="WFB2HC0300" %>

using System;
using System.Web;

public class WFB2HC0300 : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        string DATA_TYPE = context.Request.QueryString["DATA_TYPE"];
        if (DATA_TYPE == "GET_DEPT_NAME") {
            string DEPT_NO = context.Request.QueryString["DEPT_NO"];
            getDept_name(context, DEPT_NO);
        }
        else if (DATA_TYPE == "GET_EMP_NAME")
        {
            string EMP_ID = context.Request.QueryString["EMP_ID"];
            getEmp_name(context, EMP_ID);
        }
        
    }

    private void getDept_name(HttpContext context, string dept_no)
    {
        CFB2HC0300BO service = new CFB2HC0300BO();
        DATA json = new DATA();
        try
        {
            if (dept_no != "")
            {
                string dept_name = service.getDept_name(dept_no);
                if (dept_name != "")
                {
                    json.DEPT_NAME = dept_name;
                    json.errMsg = "";
                }
                else
                {
                    json.errMsg = "此部門代碼未存於部門主檔";
                }
            }
            else
            {
                json.errMsg = "此部門代碼未存於部門主檔";

            }
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
        catch (Exception ex)
        {
            json.errMsg = ex.Message;
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
    }

    private void getEmp_name(HttpContext context, string emp_id)
    {
        CFB2HC0300BO service = new CFB2HC0300BO();
        DATA json = new DATA();
        try
        {
            if (emp_id != "")
            {
                string emp_name = service.getEmp_name(emp_id);
                if (emp_name != "")
                {
                    json.EMP_NAME = emp_name;                    
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
    
    public bool IsReusable
    {
        get
        {
            return false;
        }
    }

    private class DATA
    {
        public string DEPT_NAME { get; set; }
        public string EMP_NAME { get; set; }        
        public string errMsg { get; set; }
    }

}