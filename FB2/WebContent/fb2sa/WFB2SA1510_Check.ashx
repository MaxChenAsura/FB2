<%@ WebHandler Language="C#" Class="WFB2SA1510_Check" %>

using System;
using System.Web;

public class WFB2SA1510_Check : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        string CHECK_ITEM = context.Request.QueryString["CHECK_ITEM"];
        string DATA_YEAR = context.Request.QueryString["DATA_YEAR"];
        string EMP_ID = context.Request.QueryString["EMP_ID"];
        CFB2SA1510BO service = new CFB2SA1510BO();
        try
        {
            string result = "";
            if (CHECK_ITEM == "1")
            {
                //檢查指定年度是否已完成簽核動作
                result = service.checkSign(DATA_YEAR);
                if (result != "")
                {
                    result = result.Replace("\r\n", "");
                    result = result.Replace("'", "");
                    context.Response.Write(result);
                    return;
                }
            }

            if (CHECK_ITEM == "2")
            {
                //檢查執行者是否有mail 帳號
                result = service.checkEmpEmail();
                if (result != "")
                {
                    result = result.Replace("\r\n", "");
                    result = result.Replace("'", "");
                    context.Response.Write(result);
                    return;
                }
            }

            if (CHECK_ITEM == "3")
            {
                //檢查是否有初任薪名單中,發薪MAIL 為空白的人員且不為日籍會社
                result = service.checkRecvEmail(DATA_YEAR, EMP_ID);
                if (result != "")
                {
                    //result = result.Replace("\r\n", "");
                    //result = result.Replace("'", "");
                    context.Response.Write(result);
                    return;
                }
                else
                    context.Response.Write(result);
            }
            
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