<%@ WebHandler Language="C#" Class="WFB2HC0400" %>

using System;
using System.Web;

public class WFB2HC0400 : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        string DATA_TYPE = context.Request.QueryString["DATA_TYPE"];
        //結算
        if (DATA_TYPE == "WFB2HC0400StlAmt_Click")
        {
            string PAY_YM = context.Request.QueryString["PAY_YM"];
            do_WFB2HC0400StlAmt_proc(context, PAY_YM);
        }
        else if (DATA_TYPE == "WFB2HC0400StlLock_Click_step1")
        {
            string PAY_YM = context.Request.QueryString["PAY_YM"];
            do_WFB2HC0400StlLock_proc_step1(context, PAY_YM);
        }
        else if (DATA_TYPE == "WFB2HC0400StlLock_Click_step2")
        {
            string PAY_YM = context.Request.QueryString["PAY_YM"];
            do_WFB2HC0400StlLock_proc_step2(context, PAY_YM);
        }
        else if (DATA_TYPE == "WFB2HC0400StlUnLock_Click")
        {
            string PAY_YM = context.Request.QueryString["PAY_YM"];
            do_WFB2HC0400StlUnLock_proc(context, PAY_YM);
        }
    }
    //結算作業
    private void do_WFB2HC0400StlAmt_proc(HttpContext context, string pay_ym)
    {
        CFB2HC0400BO service = new CFB2HC0400BO();
        DATA json = new DATA();
        try
        {
            if (pay_ym != "")
            {
                string strMsg = service.WFB2HC0400StlAmt_proc(pay_ym);
                json.procMsg = strMsg;
            }
            else {
                //必須輸入發放年月
                json.procMsg = Resources.Resource.wfb2hc_Required_PAY_YM;
            }           
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
        catch (Exception ex)
        {
            json.procMsg = ex.Message;
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
    }

    private void do_WFB2HC0400StlLock_proc_step1(HttpContext context, string pay_ym)
    {
        CFB2HC0400BO service = new CFB2HC0400BO();
        DATA json = new DATA();
        try
        {
            if (pay_ym != "")
            {
                string strMsg = service.WFB2HC0400StlLock_proc_step1(pay_ym);
                json.procMsg = strMsg;
            }
            else
            {
                //必須輸入發放年月
                json.procMsg = Resources.Resource.wfb2hc_Required_PAY_YM;
            }
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
        catch (Exception ex)
        {
            json.procMsg = ex.Message;
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
    }

    private void do_WFB2HC0400StlLock_proc_step2(HttpContext context, string pay_ym)
    {
        CFB2HC0400BO service = new CFB2HC0400BO();
        DATA json = new DATA();
        try
        {
            if (pay_ym != "")
            {
                string strMsg = service.WFB2HC0400StlLock_proc_step2(pay_ym);
                json.procMsg = strMsg;
            }
            else
            {
                //必須輸入發放年月
                json.procMsg = Resources.Resource.wfb2hc_Required_PAY_YM;
            }
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
        catch (Exception ex)
        {
            json.procMsg = ex.Message;
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
    }

    private void do_WFB2HC0400StlUnLock_proc(HttpContext context, string pay_ym)
    {
        CFB2HC0400BO service = new CFB2HC0400BO();
        DATA json = new DATA();
        try
        {
            if (pay_ym != "")
            {
                string strMsg = service.WFB2HC0400StlUnLock_proc(pay_ym);
                json.procMsg = strMsg;
            }
            else
            {
                //必須輸入發放年月
                json.procMsg = Resources.Resource.wfb2hc_Required_PAY_YM;
            }
            context.Response.Write(Newtonsoft.Json.JsonConvert.SerializeObject(json, Newtonsoft.Json.Formatting.None));
        }
        catch (Exception ex)
        {
            json.procMsg = ex.Message;
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
        public string procMsg { get; set; }
    }

}