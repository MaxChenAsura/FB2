<%@ WebHandler Language="C#" Class="WFB2SM1400_Check" %>

using System;
using System.Web;

public class WFB2SM1400_Check : IHttpHandler, System.Web.SessionState.IRequiresSessionState
{
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        string CHECK_ITEM = context.Request.QueryString["CHECK_ITEM"];
        string DATA_YEAR = context.Request.QueryString["DATA_YEAR"];
        string EMP_ID = context.Request.QueryString["EMP_ID"];
        string DATA_SEQ = context.Request.QueryString["DATA_SEQ"];
        CFB2SM1400BO service = new CFB2SM1400BO();
        try
        {
            string result = "";
            if (CHECK_ITEM == "1")
            {
                //檢查指定年度是否已完成簽核動作
                result = service.checkSign(DATA_YEAR, DATA_SEQ);
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
                //檢查是否有晉昇名單中,發薪MAIL 為空白的人員
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