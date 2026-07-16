<%@ WebHandler Language="C#" Class="WFB2PA0200_GetEVASetByScore" %>

using System;
using System.Web;

public class WFB2PA0200_GetEVASetByScore : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        string SCORE = context.Request.QueryString["SCORE"];

        EVA_DATA json = new EVA_DATA();
        try
        {
            if (SCORE != "")
            {
                CFB2PA0200DAO pa0200Dao = new CFB2PA0200DAO();

                System.Data.DataTable dt = pa0200Dao.getEVASetByScore(decimal.Parse(SCORE));
                
                if (dt.Rows.Count > 0)
                {
                    json.GRADE_CD = dt.Rows[0]["GRADE_CD"].ToString().Trim();
                    json.GRADE_NAME = dt.Rows[0]["GRADE_NAME"].ToString();
                    json.SCORE_S = dt.Rows[0]["SCORE_S"].ToString();
                    json.SCORE_E = dt.Rows[0]["SCORE_E"].ToString();
                    json.BONUS_AMT = dt.Rows[0]["BONUS_AMT"].ToString();
                    json.GROUP_POINT = dt.Rows[0]["GROUP_POINT"].ToString();
                    json.TRANS_KEEP_YN = dt.Rows[0]["TRANS_KEEP_YN"].ToString();
                    json.TRANS_KEEP_YN_DESC = dt.Rows[0]["TRANS_KEEP_YN_DESC"].ToString();
                    json.errMsg = "";
                }
                else
                {
                    json.errMsg = "此分數未存於提案奬金評價設定檔";
                }

            }
            else
            {
                json.errMsg = "此分數未存於提案奬金評價設定檔";

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

    private class EVA_DATA
    {
        public string GRADE_CD { get; set; }
        public string GRADE_NAME { get; set; }
        public string SCORE_S { get; set; }
        public string SCORE_E { get; set; }
        public string BONUS_AMT { get; set; }
        public string GROUP_POINT { get; set; }
        public string TRANS_KEEP_YN { get; set; }
        public string TRANS_KEEP_YN_DESC { get; set; }
        public string errMsg { get; set; }
    }

}