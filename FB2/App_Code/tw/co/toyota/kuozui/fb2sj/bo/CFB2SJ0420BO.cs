using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

/// <summary>
/// WFB2SJ0420Service 的摘要描述
/// </summary>
public class CFB2SJ0420BO : BaseService
{
    public CFB2SJ0420BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    
    //更新 TB_S_M_ASSESS_EMP_SUGGEST
    public string updateEMP_SUGGEST(CFB2SJ0420DAO wfb2sj)
    {
        try
        {
           
            
            BeginTransaction();

            wfb2sj.updateEMP_SUGGEST();

            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    //二階主管簽核更新 TB_S_M_ASSESS_EMP_SUGGEST
    public string updateEMP_SUGGEST_MA_B(CFB2SJ0420DAO wfb2sj)
    {
        try
        {
            string msg = "";
            BeginTransaction();

            wfb2sj.updateEMP_SUGGEST();

            Commit();
            //取回該員工ASSSESS_TARGET
            CFB2SJ0500DAO sj0500dao = new CFB2SJ0500DAO();
            CFB2SJ0520DAO sj0520dao;
            sj0500dao.ASSESS_YEAR = wfb2sj.ASSESS_YEAR;
            sj0500dao.ASSESS_TYPE = wfb2sj.ASSESS_TYPE;
            sj0500dao.EMP_ID = wfb2sj.EMP_ID;
            DataTable dt = sj0500dao.getEmpTargetData();
           
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["SCORE_FINAL"].ToString() != wfb2sj.SUGGEST_SCORE && wfb2sj.AUDRESULT2_YN == "Y")
                {

                    String limitRate = dt.Rows[0]["LIMIT_RATE"].ToString();
                    String ws_cd = dt.Rows[0]["WS_CD"].ToString();
                    String rDesc = "";
                    String comments = dt.Rows[0]["COMMENTS"].ToString();
                    //if (comments != "") comments += "\r\n";
                    sj0520dao = new CFB2SJ0520DAO();
                    sj0520dao.ASSESS_YEAR = wfb2sj.ASSESS_YEAR;
                    sj0520dao.ASSESS_TYPE = wfb2sj.ASSESS_TYPE;
                    sj0520dao.EMP_ID = wfb2sj.EMP_ID;

                   // comments += sj0520dao.COMMENTS;

                    byte[] by = System.Text.Encoding.Default.GetBytes(wfb2sj.SUGGEST_SCORE.ToString());
                    //推薦說明
                    if (ws_cd == "G")
                    {
                        if (by[0] < 67)
                        {
                            if (rDesc.IndexOf("業務職C") < 0)
                            {
                                if (rDesc != "") rDesc += "/";
                                rDesc += "業務職C";
                            }
                        }
                    }
                    String preScore = "";
                    if (sj0520dao.ASSESS_TYPE == "1")
                    {
                        preScore = dt.Rows[0]["SCORE_1H_1"].ToString();
                    }
                    else
                    {

                        preScore = dt.Rows[0]["SCORE_2H_1"].ToString();
                    }
                    if (preScore != "")
                    {

                        byte[] by2 = System.Text.Encoding.Default.GetBytes(preScore);
                        if (by2[0] - by[0] >= 2)
                        {
                            if (rDesc.IndexOf("向上兩級") < 0)
                            {
                                if (rDesc != "") rDesc += "/";
                                rDesc += "向上兩級";
                            }
                        }
                    }
                    sj0520dao.RECOMM_DESC = rDesc;
                    sj0520dao.SCORE_FINAL = wfb2sj.SUGGEST_SCORE;
                    sj0520dao.COMMENTS = dt.Rows[0]["COMMENTS"].ToString();
                    sj0520dao.MA_TYPE = "A";
                    sj0520dao.CREATED_BY = wfb2sj.UPDATED_BY;
                    sj0520dao.FUNC_ID = "SJ0420";
                    sj0520dao.execSP_S_ASSESS_MA_APPROVE();
                    msg = utilities.getSPLOG("SP_S_ASSESS_MA_APPROVE");

                    if (msg != "") return msg;

                }
            }

            
           
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    //協理簽核更新 TB_S_M_ASSESS_EMP_SUGGEST
    public string updateEMP_SUGGEST_MA_A(CFB2SJ0420DAO wfb2sj)
    {
        try
        {
            string msg = "";
            BeginTransaction();

            wfb2sj.updateEMP_SUGGEST();

            Commit();
            //取回該員工ASSSESS_TARGET
            CFB2SJ0500DAO sj0500dao = new CFB2SJ0500DAO();
            CFB2SJ0520DAO sj0520dao;
            sj0500dao.ASSESS_YEAR = wfb2sj.ASSESS_YEAR;
            sj0500dao.ASSESS_TYPE = wfb2sj.ASSESS_TYPE;
            sj0500dao.EMP_ID = wfb2sj.EMP_ID;
            DataTable dt = sj0500dao.getEmpTargetData();

            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["SCORE_FINAL"].ToString() != wfb2sj.SUGGEST_SCORE && wfb2sj.AUDRESULT3_YN == "Y")
                {

                    String limitRate = dt.Rows[0]["LIMIT_RATE"].ToString();
                    String ws_cd = dt.Rows[0]["WS_CD"].ToString();
                    String rDesc ="";
                    String comments = dt.Rows[0]["COMMENTS"].ToString();
                    if (comments != "") comments += "\r\n";
                    sj0520dao = new CFB2SJ0520DAO();
                    sj0520dao.ASSESS_YEAR = wfb2sj.ASSESS_YEAR;
                    sj0520dao.ASSESS_TYPE = wfb2sj.ASSESS_TYPE;
                    sj0520dao.EMP_ID = wfb2sj.EMP_ID;

                    comments += sj0520dao.COMMENTS;

                    byte[] by = System.Text.Encoding.Default.GetBytes(wfb2sj.SUGGEST_SCORE.ToString());
                    //推薦說明
                    if (ws_cd == "G")
                    {
                        if (by[0] <=67)
                        {
                            if (rDesc.IndexOf("業務職C") < 0)
                            {
                                if (rDesc != "") rDesc += "/";
                                rDesc += "業務職C";
                            }
                        }
                    }
                    String preScore = "";
                    if (sj0520dao.ASSESS_TYPE == "1")
                    {
                        preScore = dt.Rows[0]["SCORE_1H_1"].ToString();
                    }
                    else
                    {

                        preScore = dt.Rows[0]["SCORE_2H_1"].ToString();
                    }
                    if (preScore != "")
                    {

                        byte[] by2 = System.Text.Encoding.Default.GetBytes(preScore);
                        if (by2[0] - by[0] >= 2)
                        {
                            if (rDesc.IndexOf("向上兩級") < 0)
                            {
                                if (rDesc != "") rDesc += "/";
                                rDesc += "向上兩級";
                            }
                        }
                    }
                    if (wfb2sj.SUGGEST_SCORE == "A")
                    {
                        if (rDesc.IndexOf("A考核") < 0)
                        {
                            if (rDesc != "") rDesc += "/";
                            rDesc += "A考核";
                        }
                    }
                    //if (rDesc != "") rDesc += "/";
                    //rDesc += dt.Rows[0]["SCORE_FINAL"].ToString() + "要望" + wfb2sj.SUGGEST_SCORE;
                    sj0520dao.RECOMM_DESC = rDesc;
                    sj0520dao.SUGGEST_DESC = dt.Rows[0]["SCORE_FINAL"].ToString() + "要望" + wfb2sj.SUGGEST_SCORE;
                    sj0520dao.SCORE_FINAL = wfb2sj.SUGGEST_SCORE;
                    sj0520dao.COMMENTS = dt.Rows[0]["COMMENTS"].ToString();
                    sj0520dao.MA_TYPE = "A";
                    sj0520dao.CREATED_BY = wfb2sj.UPDATED_BY;
                    sj0520dao.FUNC_ID = "SJ0420";
                    sj0520dao.execSP_S_ASSESS_MA_APPROVE();
                    msg = utilities.getSPLOG("SP_S_ASSESS_MA_APPROVE");

                    if (msg != "") return msg;

                }
                if (dt.Rows[0]["SCORE_FINAL"].ToString() == wfb2sj.SUGGEST_SCORE && wfb2sj.AUDRESULT3_YN == "N")
                {

                    
                    sj0520dao = new CFB2SJ0520DAO();
                    sj0520dao.ASSESS_YEAR = wfb2sj.ASSESS_YEAR;
                    sj0520dao.ASSESS_TYPE = wfb2sj.ASSESS_TYPE;
                    sj0520dao.EMP_ID = wfb2sj.EMP_ID;
                    sj0520dao.SUGGEST_DESC = "";
                    sj0520dao.SCORE_FINAL = dt.Rows[0]["SCORE_DEPT"].ToString();
                    sj0520dao.RECOMM_DESC = dt.Rows[0]["RECOMM_DESC"].ToString();
                    sj0520dao.COMMENTS = dt.Rows[0]["COMMENTS"].ToString();
                    sj0520dao.MA_TYPE = "A";
                    sj0520dao.CREATED_BY = wfb2sj.UPDATED_BY;
                    sj0520dao.FUNC_ID = "SJ0420";
                  

                    sj0520dao.execSP_S_ASSESS_MA_APPROVE();
                    msg = utilities.getSPLOG("SP_S_ASSESS_MA_APPROVE");

                    if (msg != "") return msg;

                }
            }



            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
}