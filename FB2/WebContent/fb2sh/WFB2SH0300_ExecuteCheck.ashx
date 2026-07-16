<%@ WebHandler Language="C#" Class="WFB2SH0300_ExecuteCheck" %>

using System;
using System.Web;

public class WFB2SH0300_ExecuteCheck : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        string award_year = context.Request.QueryString["AWARD_YEAR"];
        string award_round = context.Request.QueryString["AWARD_ROUND"];
        CFB2SH0300BO sh030BO = new CFB2SH0300BO();
        JSON_DATA json = new JSON_DATA();
        try
        {

            System.Data.DataTable dt = sh030BO.executeCheck(award_year, award_round);
            if (dt.Rows.Count > 0)
            {
                string result_year = dt.Rows[0]["AWARD_YEAR"].ToString();
                string result_target = dt.Rows[0]["TARGET_GEN_DT"].ToString();
                string freezeFlag = dt.Rows[0]["FREEZE_FLAG"].ToString();
                string genDT = dt.Rows[0]["GEN_DT"].ToString() != "" ? Convert.ToDateTime(dt.Rows[0]["GEN_DT"].ToString()).ToString("yyyy/MM/dd") : "";

                if (freezeFlag == "Y")
                {
                    json.errMsg = "此年獎回數已無法進行計算!";
                }
                else if (result_target == "")
                {
                    json.errMsg = "請先建立年獎對象";
                }
                else
                {
                    json.GEN_DT = genDT;
                    json.FREEZE_FLAG = freezeFlag;
                    json.errMsg = "";
                }
            }
            else
            {
                json.GEN_DT = "";
                json.errMsg = "請先建立該年度年獎資料";
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

    private class JSON_DATA
    {
        //取回來的資料
        public string AWARD_YEAR { get; set; }
        public string TARGET_GEN_DT { get; set; }
        public string GEN_DT { get; set; }
        public string FREEZE_FLAG { get; set; }

        //顯示的錯誤訊息
        public string errMsg { get; set; }
    }

}