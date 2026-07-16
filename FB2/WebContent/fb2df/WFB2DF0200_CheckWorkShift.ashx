<%@ WebHandler Language="C#" Class="WFB2DF0200_CheckWorkShift" %>

using System;
using System.Web;

public class WFB2DF0200_CheckWorkShift : IHttpHandler {
    
    public void ProcessRequest (HttpContext context) {
        context.Response.ContentType = "text/plain";
        string EMP_ID = context.Request.QueryString["EMP_ID"];
        string ROOM_NO = context.Request.QueryString["ROOM_NO"];
        string WORK_SHIFT_CD = context.Request.QueryString["WORK_SHIFT_CD"];
        CFB2DF0200BO service = new CFB2DF0200BO();
        try
        {
            string result = service.checkWorkShift(EMP_ID,ROOM_NO, WORK_SHIFT_CD);
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