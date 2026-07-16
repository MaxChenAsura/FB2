using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;

/// <summary>
/// CFB2SM2100BO 的摘要描述
/// </summary>
public class CFB2SM2100BO : BaseService
{
    public CFB2SM2100BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    #region "Initial"
    public DataTable getLEVEL_CD()
    {
        CFB2SM2100DAO wfb2sm = new CFB2SM2100DAO();
        try
        {
            return wfb2sm.getLEVEL_CD();
        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable getGRADE_CD(string level_cd)
    {
        CFB2SM2100DAO wfb2sm = new CFB2SM2100DAO();
        try
        {
            return wfb2sm.getGRADE_CD(level_cd);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable getLEVEL_CD_NEW()
    {
        CFB2SM2100DAO wfb2sm = new CFB2SM2100DAO();
        try
        {
            return wfb2sm.getLEVEL_CD_NEW();
        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable getPJOB_CD_NEW()
    {
        CFB2SM2100DAO wfb2sm = new CFB2SM2100DAO();
        try
        {
            return wfb2sm.getPJOB_CD_NEW();
        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable getPJOB_CD_NEW(string level_cd)
    {
        CFB2SM2100DAO wfb2sm = new CFB2SM2100DAO();
        try
        {
            return wfb2sm.getPJOB_CD_NEW(level_cd);
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion

    #region "Qry"
    //取得現在晉昇回數
    public DataTable getdata_seq(int current_year)
    {
        try
        {
            CFB2SM2100DAO fb2sm = new CFB2SM2100DAO();
            return fb2sm.getdata_seq(current_year);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //public DataTable getGenerateDT_Gg(string p1, string p2)
    //{
    //    CFB2SM2100DAO wfb2sm = new CFB2SM2100DAO();
    //    try
    //    {
    //        return wfb2sm.getGenerateDT_Gg(p1, p2);
    //    }
    //    catch (Exception)
    //    {

    //        throw;
    //    }
    //}

    //public DataTable GenerateDTisNULL(string p1, string p2)
    //{
    //    CFB2SM2100DAO wfb2sm = new CFB2SM2100DAO();
    //    try
    //    {
    //        return wfb2sm.GenerateDTisNULL(p1, p2);
    //    }
    //    catch (Exception)
    //    {

    //        throw;
    //    }
    //}

    //public DataTable getGenerateDT_Gp(string p1, string p2)
    //{
    //    CFB2SM2100DAO wfb2sm = new CFB2SM2100DAO();
    //    try
    //    {
    //        return wfb2sm.getGenerateDT_Gp(p1, p2);
    //    }
    //    catch (Exception)
    //    {

    //        throw;
    //    }
    //}

    //public DataTable getGenerateDT_Pg(string p1, string p2)
    //{
    //    CFB2SM2100DAO wfb2sm = new CFB2SM2100DAO();
    //    try
    //    {
    //        return wfb2sm.getGenerateDT_Pg(p1, p2);
    //    }
    //    catch (Exception)
    //    {

    //        throw;
    //    }
    //}
    public string deleteData(List<Tuple<string, string>> data_year)
    {
        CFB2SM2100DAO fb2sm210 = new CFB2SM2100DAO();
        string rtnmessage = "";
        try
        {
            foreach (var item in data_year)
            {
                //2.檢核「是否已核可」(核可狀態==Y)，顯示訊息提示視窗「已核可無法刪除」
                DataTable tmp = fb2sm210.getGenerateDT_Gp(item.Item1, item.Item2);
                if ((int)tmp.Rows[0]["total_record_Gp"] != 0)
                {
                    rtnmessage += "年度為" + item.Item1 + "，晉昇回數為" + item.Item2 + "的資料\\n";
                }
            }

            //檢查OK逐筆刪除
            if (rtnmessage == "")
            {
                try
                {
                    BeginTransaction();
                    foreach (var item in data_year)
                    {
                        fb2sm210.deletePromotion_TXN(item.Item1, item.Item2);
                        fb2sm210.deletePromotion(item.Item1, item.Item2);
                        fb2sm210.deletePromotion_H(item.Item1, item.Item2);
                    }
                    Commit();
                    return "0";
                }
                catch (Exception ex)
                {
                    RollBack();
                    return ex.Message;
                }
            }
            else
                return rtnmessage;

        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public string addData(CFB2SM2100DAO fb2sm210)
    {
        string rtnmessage = "";
        try
        {

            //(1)以 新增明細畫面.(年度==年度+晉昇回數==晉昇回數)讀取 晉昇作業主檔，如 晉昇作業主檔 或 晉昇主檔 資料已存在，則提示視窗顯示「年度+晉昇回數重覆」
            DataTable dtexit = fb2sm210.dtexit();
            if (dtexit.Rows.Count > 0)
            {
                rtnmessage += " 年度+晉昇回數重覆" + "\\n";
            }
            bool isYearNoSeq = fb2sm210.checkYearNoSeq();
            if (!isYearNoSeq)
            {
                rtnmessage += " 此年度已新增過晉級作業" + "\\n";
            }
            if (rtnmessage.Trim().Length == 0)
            {
                BeginTransaction();
                fb2sm210.addData();
                Commit();
                rtnmessage = "0";
            }
            return rtnmessage;
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    public string updateData(CFB2SM2100DAO fb2sm210)
    {
        try
        {
            BeginTransaction();
            fb2sm210.updateData();
            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    #endregion

    #region "Generate & Release"

    //資料生成
    public string Generate(string data_year, string data_seq, string isCover)
    {
        try
        {
            BeginTransaction();
            CFB2SM2100DAO fb2sm210 = new CFB2SM2100DAO();
            //isCover為"Y" 先刪除此比主檔所有明細
            if (isCover == "Y")
            {
                fb2sm210.deleteAllDtl(data_year, data_seq);
            }
            //系統自動審核其他晉昇條件，取員工人事資料VIEW(VH_H_EMP_DATA)
            DataTable dt = fb2sm210.getGenerateData();

            DataTable dt2score = new DataTable();
            DateTime currentDate = DateTime.Now;
            DateTime current_yearEndDate = new DateTime(DateTime.Now.Year, 12, 31);
            TimeSpan tsDay = current_yearEndDate - currentDate;
            int dayCount = Convert.ToInt32(tsDay.Days) + 1;   //算出今日到年底有幾天
            int grade_minus = 0;
            if (dt.Rows.Count > 0)
            {
                for (int a = 0; a < dt.Rows.Count; a++)
                {

                     dt2score = fb2sm210.get2score(dt.Rows[a]["EMP_ID"].ToString());
                    if (dt2score.Rows.Count > 0)
                    {
                        //取得 前一回能力考核成績、前二回能力考核成績																	
                        fb2sm210.ASSESS_SCORE_1 = dt2score.Rows[0]["SCORE_1H"].ToString();
                        if (dt2score.Rows.Count > 1)
                        {
                            fb2sm210.ASSESS_SCORE_2 = dt2score.Rows[1]["SCORE_1H"].ToString();
                        }
                        else
                            fb2sm210.ASSESS_SCORE_2 = "";
                    }
                    else
                    {
                        //取得 前一回能力考核成績、前二回能力考核成績																	
                        fb2sm210.ASSESS_SCORE_1 = "";
                        fb2sm210.ASSESS_SCORE_2 = "";
                    }
                    

                    fb2sm210.DATA_YEAR = data_year;
                    fb2sm210.EMP_ID = dt.Rows[a]["EMP_ID"].ToString();
                    fb2sm210.EMP_NAME = dt.Rows[a]["EMP_NAME"].ToString().Trim();
                    fb2sm210.EMP_CHG_CD = dt.Rows[a]["EMP_CHG_CD"].ToString();
                    fb2sm210.LEVEL_CD = dt.Rows[a]["LEVEL_CD"].ToString();
                    fb2sm210.GRADE_CD = dt.Rows[a]["GRADE_CD"].ToString().Trim();
                    fb2sm210.PJOB_CD = dt.Rows[a]["PJOB_CD"].ToString();
                    fb2sm210.PJOB_DESC = dt.Rows[a]["PJOB_DESC"].ToString().Trim();
                    fb2sm210.WS_CD = dt.Rows[a]["WS_CD"].ToString();
                    fb2sm210.DEPT_NO = dt.Rows[a]["DEPT_NO"].ToString();
                    fb2sm210.DIV_FULL_DEPT_NAME = dt.Rows[a]["DEPT_NAME"].ToString().Trim();
                    fb2sm210.WORK_YEARS = Math.Round(((Convert.ToDouble(dt.Rows[a]["WORK_DAYS"]) + dayCount) / 365),1,MidpointRounding.AwayFromZero).ToString();  //(在職天數 + 到年底天數)/365 四捨五入
                    fb2sm210.LEVEL_WORK_YEARS = Math.Round((Convert.ToDouble(dt.Rows[a]["LEVEL_WORK_DAYS_toEnd"]) / 365), 1, MidpointRounding.AwayFromZero).ToString();
                    fb2sm210.LEVEL_CD_NEW = dt.Rows[a]["LEVEL_CD"].ToString();
                    fb2sm210.PJOB_CD_NEW = dt.Rows[a]["PJOB_CD"].ToString();
                    fb2sm210.PJOB_DESC_NEW = dt.Rows[a]["PJOB_DESC"].ToString().Trim();
                    fb2sm210.DATA_SEQ = data_seq;
                    //計算以兩回能力考核 得到晉升級數
                    grade_minus = 0;
                    grade_minus = getGRADE_CD(fb2sm210.ASSESS_SCORE_1, fb2sm210.ASSESS_SCORE_2);
                    if (grade_minus > 0)
                    {
                        if (Convert.ToInt32(fb2sm210.GRADE_CD) + grade_minus > 6)
                            fb2sm210.GRADE_CD_NEW = "X";
                        else
                            fb2sm210.GRADE_CD_NEW = Convert.ToString(Convert.ToInt32(fb2sm210.GRADE_CD) + grade_minus);
                        //新增 晉昇人員生成檔
                        fb2sm210.addGenerateData();
                    }
                }
            }
            //晉昇人員生成檔 取得 總筆數
            DataTable dtPt = fb2sm210.getPROMOTION_TOTAL(data_year, data_seq);
            fb2sm210.PROMOTION_TOTAL = dtPt.Rows.Count.ToString();
            //更新晉昇作業主檔
            fb2sm210.updateGenerate_H(data_year, data_seq);
            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    //計算以兩回能力考核 得到晉升級數
    private int getGRADE_CD(string ASSESS_SCORE_1, string ASSESS_SCORE_2)
    {
        int SCORE1 = convertLetterToNumber(ASSESS_SCORE_1);//A:5,B:4,C:3,D:2,E:1, 無考績為0
        int SCORE2 = convertLetterToNumber(ASSESS_SCORE_2);
        int grade_minus = 0;
        //前一回是E時,停滯(回傳0)
        if (SCORE1 == 1) {
            return 0;
        }
        //只有前一回是D (回傳1)
        if (SCORE1 == 2 && SCORE2 == 0)
        {
            return 1;
        }
        //前一回是D,但前2回是<=D,停滯(回傳0)
        if (SCORE1 == 2 && SCORE2 <= 2) {
            return 0;
        }
        //前一回是D,但前2回是>D,晉昇1級(回傳1)
        if (SCORE1 == 2 && SCORE2 > 2)
        {
            return 1;
        }

        //前一回是C,不管前2回,晉昇1級(回傳1)
        if (SCORE1 == 3)
        {
            return 1;
        }
        //只有前一回是B (回傳1)
        if (SCORE1 == 4 && SCORE2 == 0)
        {
            return 1;
        }
        //前一回是B,前2回<B,晉昇1級(回傳1)
        if (SCORE1 == 4 && SCORE2 < 4)
        {
            return 1;
        }
        //前一回是B,前2回>=B,晉昇2級(回傳2)
        if (SCORE1 == 4 && SCORE2 >= 4)
        {
            return 2;
        }
        //前一回是A,不管前2回,晉昇2級(回傳2)
        if (SCORE1 == 5 )
        {
            return 2;
        }


        /*
        //有兩回能力考核成績
        if (SCORE2 != 0)
        {
            if (SCORE1 == 5)
                grade_minus = 2;
            else if (SCORE1 == 4)
            {
                if (SCORE2 >= 4)
                    grade_minus = 2;
                else
                    grade_minus = 1;
            }
            else if (SCORE1 == 3)
                grade_minus = 1;
            else if (SCORE1 == 2)
            {
                if (SCORE2 > 2)
                    grade_minus = 1;
                else
                    grade_minus = 0;
            }
            else if (SCORE1 == 1)
                grade_minus = 0;
        }
        //只有一回考核成績
        else
        {
            if (SCORE1 == 5) //A
                grade_minus = 2;
            else if (SCORE1 == 4 || SCORE1 == 3 || SCORE1 == 2)//B or C or D
                grade_minus = 1;
            else if (SCORE1 == 1)//E
                grade_minus = 0;
        }
        */
        return grade_minus;
    }
    public string Release(string DATA_YEAR, string DATA_SEQ)
    {
        try
        {
            CFB2SM2100DAO fb2sm210 = new CFB2SM2100DAO();
            BeginTransaction();
            fb2sm210.updateRelease_H(DATA_YEAR, DATA_SEQ);
            Commit();
            return "0";
        }
        catch (Exception)
        {

            throw;
        }
    }
    #endregion

    #region "Dtl"

    public DataTable getHeader(string data_year, string data_seq)
    {
        try
        {
            CFB2SM2100DAO fb2sm210 = new CFB2SM2100DAO();
            return fb2sm210.getHeader(data_year, data_seq);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public string deleteDtlData(List<Tuple<string, string>> dtldatakey, string data_year, string data_seq, string process_status)
    {

        try
        {
            CFB2SM2100DAO dao = new CFB2SM2100DAO();
            BeginTransaction();
            foreach (var item in dtldatakey)
            {
                dao.deletePromotionDtlYtoN(item.Item1, item.Item2, process_status, data_seq);
            }
            DataTable dtPt = dao.getPROMOTION_TOTAL(data_year, data_seq);
            dao.PROMOTION_TOTAL = dtPt.Rows.Count.ToString();
            dao.updatePROMOTION_H(data_year, data_seq);
            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    public string updateDtl(CFB2SM2100DAO fb2sm210)
    {
        string rtnmessage = "";
        try
        {

            //檢查級數是否存在
            DataTable cd_new = fb2sm210.checkCD_NEW();
            if (Convert.ToInt32(cd_new.Rows[0]["total"]) == 0)
            {
                rtnmessage += "晉昇資格、級數不符\r\n";
            }
            //檢查職務是否存在
            DataTable job_new = fb2sm210.checkJOB_NEW();
            if (Convert.ToInt32(job_new.Rows[0]["total"]) == 0)
            {
                rtnmessage += "晉昇資格、職務不符\r\n";
            }
            //檢查有無晉升
            int grade = 0;
            int new_grade = 0;
            if (fb2sm210.GRADE_CD.Trim() == "")
                grade = 0;
            else if (fb2sm210.GRADE_CD == "X")
                grade = 7;
            else
                grade = Convert.ToInt32(fb2sm210.GRADE_CD);

            if (fb2sm210.GRADE_CD_NEW.Trim() == "")
                new_grade = 0;
            else if (fb2sm210.GRADE_CD_NEW == "X")
                new_grade = 7;
            else
                new_grade = Convert.ToInt32(fb2sm210.GRADE_CD_NEW);

            if (grade > new_grade)
                rtnmessage += "晉昇資格級數不可低於原資格級數\r\n";

            if (new_grade - grade > 0)
            {
                if (new_grade - grade > 2)
                    rtnmessage += "級數最多只能晉升2級\r\n";

                if (new_grade - grade > getGRADE_CD(fb2sm210.ASSESS_SCORE_1, fb2sm210.ASSESS_SCORE_2))
                    rtnmessage += "級數晉升不符合規定\r\n";
            }

            //檢查OK逐筆更新
            if (rtnmessage == "")
            {
                try
                {
                    BeginTransaction();
                    fb2sm210.updatePromoyionDtlTXN();

                    DataTable dtPt = fb2sm210.getPROMOTION_TOTAL(fb2sm210.DATA_YEAR, fb2sm210.DATA_SEQ);
                    fb2sm210.PROMOTION_TOTAL = dtPt.Rows.Count.ToString();

                    fb2sm210.updatePROMOTION_H(fb2sm210.DATA_YEAR, fb2sm210.DATA_SEQ);
                    Commit();
                    return "0";

                }
                catch (Exception ex)
                {
                    RollBack();
                    return ex.Message;
                }
            }
            else
                return rtnmessage;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public string addDtl(CFB2SM2100DAO fb2sm210)
    {
        string rtnmessage = "";
        try
        {
            //檢查重複資料
            DataTable emp_id = fb2sm210.checkEMP_IDexist();
            if (emp_id.Rows.Count > 0)
            {
                rtnmessage += "工號已存在，無法新增\r\n";
            }
            //檢查級數是否存在
            DataTable grade_new = fb2sm210.checkCD_NEW();
            if (Convert.ToInt32(grade_new.Rows[0]["total"]) == 0)
            {
                rtnmessage += "晉昇資格、級數不符\r\n";
            }
            //檢查職務是否存在
            DataTable job_new = fb2sm210.checkJOB_NEW();
            if (Convert.ToInt32(job_new.Rows[0]["total"]) == 0)
            {
                rtnmessage += "晉昇資格、職務不符\r\n";
            }
            //檢查有無晉升
            int grade = 0;
            int new_grade = 0;
            if (fb2sm210.GRADE_CD.Trim() == "")
                grade = 0;
            else if (fb2sm210.GRADE_CD == "X")
                grade = 7;
            else
                grade = Convert.ToInt32(fb2sm210.GRADE_CD);

            if (fb2sm210.GRADE_CD_NEW.Trim() == "")
                new_grade = 0;
            else if (fb2sm210.GRADE_CD_NEW == "X")
                new_grade = 7;
            else
                new_grade = Convert.ToInt32(fb2sm210.GRADE_CD_NEW);

            if (grade > new_grade)
                rtnmessage += "晉昇資格級數不可低於原資格級數\r\n";

            //if (new_grade - grade > 0)
            //{
            //    //檢查晉升條件
            //    rtnmessage += checkGRADE_CDRule(new_grade - grade, fb2sm210.ASSESS_SCORE_1, fb2sm210.ASSESS_SCORE_2);
            //}

            if (fb2sm210.LEVEL_CD_NEW=="5A" && new_grade - grade > 0)
            {
                if (new_grade - grade > 2)
                    rtnmessage += "級數最多只能晉升2級\r\n";

                if (new_grade - grade > getGRADE_CD(fb2sm210.ASSESS_SCORE_1, fb2sm210.ASSESS_SCORE_2))
                    rtnmessage += "級數晉升不符合規定\r\n";
            }
            if (fb2sm210.LEVEL_CD_NEW == "RB" && new_grade - grade > 0)
            {
                if (new_grade - grade > 1)
                    rtnmessage += "級數最多只能晉升1級\r\n";
            }

            if (rtnmessage == "")
            {
                try
                {
                    BeginTransaction();
                    fb2sm210.addPromotiondtl();

                    DataTable dtPt = fb2sm210.getPROMOTION_TOTAL(fb2sm210.DATA_YEAR, fb2sm210.DATA_SEQ);
                    fb2sm210.PROMOTION_TOTAL = dtPt.Rows.Count.ToString();

                    fb2sm210.updatePROMOTION_H(fb2sm210.DATA_YEAR, fb2sm210.DATA_SEQ);
                    Commit();
                    return "0";
                }
                catch (Exception ex)
                {
                    RollBack();
                    return ex.Message;
                }
            }
            return rtnmessage;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    private int convertLetterToNumber(string letter)
    {
        int number = 0;
        if (letter == "A")
            number = 5;
        else if (letter == "B")
            number = 4;
        else if (letter == "C")
            number = 3;
        else if (letter == "D")
            number = 2;
        else if (letter == "E")
            number = 1;
        return number;
    }
    public DataTable getEMP_ID_data(string p)
    {
        try
        {
            CFB2SM2100DAO fb2sm = new CFB2SM2100DAO();
            return fb2sm.getEMP_ID_data(p);
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion


}