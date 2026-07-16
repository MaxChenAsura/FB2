<%@ WebHandler Language="C#" Class="WFB2SJ0420GetEmpTargetData" %>

using System;
using System.Web;

public class WFB2SJ0420GetEmpTargetData : IHttpHandler
{

    public void ProcessRequest(HttpContext context)
    {
        context.Response.ContentType = "text/plain";
        string ASSESS_YEAR = context.Request.QueryString["ASSESS_YEAR"];
        string ASSESS_TYPE = context.Request.QueryString["ASSESS_TYPE"];
        string EMP_ID = context.Request.QueryString["EMP_ID"];
        string CREATED_BY = context.Request.QueryString["CREATED_BY"];
        EMP_DATA json = new EMP_DATA();
        try
        {
            if (ASSESS_YEAR != ""&&ASSESS_TYPE != ""&&EMP_ID != "")
            {
                CFB2SJ0410DAO sj0410Dao = new CFB2SJ0410DAO();
                sj0410Dao.ASSESS_YEAR = ASSESS_YEAR;
                sj0410Dao.ASSESS_TYPE = ASSESS_TYPE;
                sj0410Dao.EMP_ID = EMP_ID;
                sj0410Dao.CREATED_BY = CREATED_BY;
                System.Data.DataTable dt = sj0410Dao.getEmpTargetData();
                
                if (dt.Rows.Count > 0)
                {
                    json.EMP_NAME = dt.Rows[0]["EMP_NAME"].ToString().Trim();
                    json.LEVEL_CD = dt.Rows[0]["LEVEL_CD"].ToString();
                    json.SCORE_DEPT = dt.Rows[0]["SCORE_DEPT"].ToString();
                    json.WS_CD = dt.Rows[0]["WS_CD"].ToString();
                    json.PJOB_DESC = dt.Rows[0]["PJOB_DESC"].ToString();
                    json.DEPT_NAME = dt.Rows[0]["DEPT_NAME"].ToString();
                    json.AGE = dt.Rows[0]["AGE"].ToString();
                    json.WORK_YEARS = dt.Rows[0]["WORK_YEARS"].ToString();
                    json.RECENT_LEVEL_WORK_YEARS = dt.Rows[0]["RECENT_LEVEL_WORK_YEARS"].ToString();
                    json.DISTING_REMARK = dt.Rows[0]["DISTING_REMARK"].ToString();
                    json.SCORE_1H_1 = dt.Rows[0]["SCORE_1H_1"].ToString();
                    json.SCORE_1H_2 = dt.Rows[0]["SCORE_1H_2"].ToString();
                    json.SCORE_1H_3 = dt.Rows[0]["SCORE_1H_3"].ToString();
                    json.SCORE_2H_1 = dt.Rows[0]["SCORE_2H_1"].ToString();
                    json.SCORE_2H_2 = dt.Rows[0]["SCORE_2H_2"].ToString();
                    json.SCORE_2H_3 = dt.Rows[0]["SCORE_2H_3"].ToString();
                    json.LEAVE_OP = dt.Rows[0]["LEAVE_OP"].ToString();
                    json.LEAVE_AB = dt.Rows[0]["LEAVE_AB"].ToString();
                    json.LEAVE_Q = dt.Rows[0]["LEAVE_Q"].ToString();
                    json.DEPT20_EMP_ID = dt.Rows[0]["DEPT20_EMP_ID"].ToString();
                    json.MA_A_EMP_ID = dt.Rows[0]["MA_A_EMP_ID"].ToString();
                    json.MA_B_EMP_ID = dt.Rows[0]["MA_B_EMP_ID"].ToString();
                    json.LIMIT_RATE = dt.Rows[0]["LIMIT_RATE"].ToString();
                    json.errMsg = "";
                }
                else
                {
                    json.errMsg = "無資料或非管理屬下";
                }

            }
            else
            {
                json.errMsg = "無資料或非管理屬下";

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

    private class EMP_DATA
    {
        public string EMP_NAME { get; set; }
        public string LEVEL_CD { get; set; }
        public string SCORE_DEPT { get; set; }
        public string WS_CD { get; set; }
        public string DEPT_NAME { get; set; }
        public string PJOB_DESC { get; set; }
        public string AGE { get; set; }
        public string WORK_YEARS { get; set; }
        public string RECENT_LEVEL_WORK_YEARS { get; set; }
        public string DISTING_REMARK { get; set; }
        public string SCORE_1H_1 { get; set; }
        public string SCORE_1H_2 { get; set; }
        public string SCORE_1H_3 { get; set; }
        public string SCORE_2H_1 { get; set; }
        public string SCORE_2H_2 { get; set; }
        public string SCORE_2H_3 { get; set; }
        public string LEAVE_OP { get; set; }
        public string LEAVE_AB { get; set; }
        public string LEAVE_Q { get; set; }
        public string DEPT20_EMP_ID { get; set; }
        public string MA_A_EMP_ID { get; set; }
        public string MA_B_EMP_ID { get; set; }
        public string LIMIT_RATE { get; set; }
        public string errMsg { get; set; }
    }

}