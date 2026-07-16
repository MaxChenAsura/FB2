<%@ WebHandler Language="C#" Class="WFBCOMMGEO" %>

using System;
using System.Web;

public class WFBCOMMGEO : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        string HR_CHG_CD = context.Request.QueryString["HR_CHG_CD"];
        COMMGEOBO service = new COMMGEOBO();
        HR_CHG_CD json = new HR_CHG_CD();
        try
        {
            if (HR_CHG_CD != "")
            {
                System.Data.DataTable dt = service.getCHANGE_CODEFile(HR_CHG_CD);
                if (dt.Rows.Count > 0)
                {
                    json.HR_CHG_CD_A = dt.Rows[0]["HR_CHG_CD"].ToString();
                    json.HR_CHG_DESC = dt.Rows[0]["HR_CHG_DESC"].ToString();
                   
                    json.errMsg = "";
                }
                else
                {
                    json.errMsg = "此代碼未存於人事異動代碼主檔";
                }

            }
            else
            {
                json.errMsg = "此代碼未存於人事異動代碼主檔";

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

    private class HR_CHG_CD
    {
        public string HR_CHG_CD_A { get; set; }
        public string HR_CHG_DESC { get; set; }
       
        public string errMsg { get; set; }
    }

}