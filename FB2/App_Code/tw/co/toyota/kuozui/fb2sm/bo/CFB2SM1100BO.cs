using FB2.tw.co.toyota.kuozui.bo;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// CFB2SM1100BO 的摘要描述
/// </summary>
public class CFB2SM1100BO : BaseService
{
    System.Text.ASCIIEncoding asciiEncoding = new System.Text.ASCIIEncoding();
    public CFB2SM1100BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    #region "Qry"
    //取得現在晉昇回數
    public DataTable getdata_seq(int current_year)
    {
        try
        {
            CFB2SM1100DAO fb2sm = new CFB2SM1100DAO();
            return fb2sm.getdata_seq(current_year);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public string addPromotion(CFB2SM1100DAO fb2sm110)
    {
        try
        {
            string rtnmessage = "";
            //(1)以 新增明細畫面.(年度==年度+晉昇回數==晉昇回數)讀取 晉昇作業主檔，如 晉昇作業主檔 或 晉昇主檔 資料已存在，則提示視窗顯示「年度+晉昇回數重覆」
            DataTable dtexit = fb2sm110.checkRepead();
            if (dtexit.Rows.Count > 0)
            {
                rtnmessage += " 年度+晉昇回數重覆" + "\\n";
            }
            if (rtnmessage.Trim().Length == 0)
            {
                BeginTransaction();
                fb2sm110.addPromotion();
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
    public string updataPromotion(CFB2SM1100DAO fb2sm110)
    {
        try
        {
            BeginTransaction();
            fb2sm110.updatePromotion();
            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    public string delete_Promotion(List<Tuple<string, string>> data_year)
    {
        CFB2SM1100DAO fb2sm110 = new CFB2SM1100DAO();
        string rtnmessage = "";
        try
        {
            foreach (var item in data_year)
            {
                //2.檢核「是否已核可」(核可狀態==Y)，顯示訊息提示視窗「已核可無法刪除」
                DataTable tmp = fb2sm110.getGenerateDT_Gp(item.Item1, item.Item2);
                if ((int)tmp.Rows[0]["total_record_Gp"] != 0)
                {
                    rtnmessage += "核可狀態" + item.Item1 + "，已核可無法刪除 \\n";
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
                        fb2sm110.deletePromotion_H(item.Item1, item.Item2);  //刪除 晉昇作業主檔
                        fb2sm110.deletePromotion_TXN(item.Item1, item.Item2);  //刪除 晉昇人員生成檔
                        fb2sm110.deletePromotion(item.Item1);      //刪除 晉昇人員主檔
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
    public DataTable getMainEmp_id(string p)
    {
        try
        {
            CFB2SM1100DAO fb2sm = new CFB2SM1100DAO();
            return fb2sm.getMainEmp_id(p);
        }
        catch (Exception)
        {

            throw;
        }
    }
    public string Release(DataTable dtSelect)
    {
        try
        {
            CFB2SM1100DAO fb2sm110 = new CFB2SM1100DAO();
            BeginTransaction();
            foreach (DataRow row in dtSelect.Rows)
            {
                fb2sm110.updateRelease_H(Convert.ToString(row["DATA_YEAR"]), Convert.ToString(row["DATA_SEQ"]));
            }
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
            CFB2SM1100DAO fb2sm210 = new CFB2SM1100DAO();
            return fb2sm210.getHeader(data_year, data_seq);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public string addPromotiondtl(CFB2SM1100DAO fb2sm110)
    {
        string rtnmessage = "";
        try
        {
            //檢查重複資料
            DataTable emp_id = fb2sm110.getemp_id();
            if (emp_id.Rows.Count > 0)
            {
                rtnmessage += "工號已存在，無法新增\\n";
            }
            //檢查OK新增
            if (rtnmessage == "")
            {
                BeginTransaction();
                try
                {
                    fb2sm110.addPromotiondtl("N");

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
    public string updataPromotiondtl(CFB2SM1100DAO fb2sm110)
    {
        string rtnmessage = "";
        try
        {
            rtnmessage = checkInsertData(fb2sm110);
            //檢查OK更新
            if (rtnmessage == "")
            {
                BeginTransaction();
                fb2sm110.updatePromotiondtl("U");
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
    public void updataPH(CFB2SM1100DAO fb2sm)
    {
        try
        {
            BeginTransaction();
            //先取總筆數
            DataTable dt = fb2sm.getPROMOTION_TOTAL();
            //去完後更新PH
            fb2sm.updataPH(dt.Rows[0]["cnt"].ToString());
            Commit();
        }
        catch (Exception)
        {
            RollBack();
        }
    }
    //手動修改前 檢查核可狀態
    public DataTable getPROCESS_STATUS(CFB2SM1100DAO fb2sm110)
    {
        try
        {
            return fb2sm110.getPROCESS_STATUS();
        }
        catch (Exception)
        {

            throw;
        }
    }
    //刪除明細
    public string delete_Promotion_Dtl(List<Tuple<string, string>> data_year, string DATA_SEQ)
    {

        string rtnmessage = "";
        try
        {
            CFB2SM1100DAO fb2sm110 = new CFB2SM1100DAO();
            bool pass = true;

            //2.檢核「是否已核可」(核可狀態==Y)，顯示訊息提示視窗「已核可無法刪除」
            DataTable tmp = fb2sm110.getPromotionHDT(data_year[0].Item1, DATA_SEQ);
            if ((int)tmp.Rows[0]["total_record"] != 0)
            {
                pass = false;
                rtnmessage += "年度:" + data_year[0].Item1 + " 回數:" + DATA_SEQ + "，已核可無法刪除 \\n";
            }
            ////檢查OK逐筆刪除
            if (pass)
            {
                BeginTransaction();
                foreach (var item in data_year)
                {
                    fb2sm110.deletePromotionDtlYtoN(item.Item1, item.Item2, DATA_SEQ);
                }
                Commit();
                return "0";
            }
            else
                return rtnmessage;
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    #endregion

    #region "檢核"
    public string checkInsertData(CFB2SM1100DAO fb2sm110)
    {
        string rtnmessage = "";
        try
        {

            //檢查級數是否存在
            DataTable cd_new = fb2sm110.checkGRADE_LEVEL();
            if (Convert.ToInt32(cd_new.Rows[0]["cnt"]) == 0)
            {
                rtnmessage += "晉昇資格、級數不符\\n";
            }
            //檢查職務是否存在
            DataTable job_new = fb2sm110.checkPJOB_LEVEL();
            if (Convert.ToInt32(job_new.Rows[0]["cnt"]) == 0)
            {
                rtnmessage += "晉昇資格、職務不符\\n";
            }
            //檢查有無晉升
            DataTable checkgrade = fb2sm110.checkgrade();
            DataTable checkLevelCd = fb2sm110.checkLevelCd();

            if (fb2sm110.LEVEL_CD_NEW != "" && fb2sm110.GRADE_CD_NEW == "")
            {
                //seq = 1 是最大的
                string oriLevel = fb2sm110.getOriLevelData();
                string newLevel = fb2sm110.getNewiLevelData();

                if (Convert.ToInt32(oriLevel) <= Convert.ToInt32(newLevel))
                {
                    rtnmessage += "晉昇資格不可低於原資格\\n";
                }

                //是否晉昇1級以上
                if (Convert.ToInt32(oriLevel) - Convert.ToInt32(newLevel) > 1)
                {
                    rtnmessage += "不可連續晉昇1級以上\\n";
                }else if (fb2sm110.LEVEL_CD_NEW=="4B")
                {
                    if (fb2sm110.GRADE_CD!="X")
                    rtnmessage += "不可連續晉昇1級以上\\n";
                }

            }

            //20180119 俊賢表示已不需要
            //if (fb2sm110.LEVEL_CD_NEW == "RA")
            //{
            //    DataTable checkRA = fb2sm110.check_LevelIsRA();
            //    if (checkRA.Rows.Count == 0)
            //    {
            //        rtnmessage += "此員工無達到晉升資格年齡(必須大於等於46歲)，或能力考核不符\\n";
            //    }
            //}

            /*//20150427改到晉級功能SM210
            if (fb2sm110.LEVEL_CD_NEW == "RB" && fb2sm110.GRADE_CD_NEW != "")
            {
                if (checkgrade.Rows.Count == 0)
                {
                    rtnmessage += "晉昇資格級數不可低於原資格級數\\n";
                }
            } 
             
            if (fb2sm110.LEVEL_CD_NEW == "RB")
            {
                DataTable checkRB = fb2sm110.check_LevelIsRB();
                if (checkRB.Rows.Count == 0)
                {
                    rtnmessage += "此員工能力考核不符\\n";
                }
                else
                {
                    if (fb2sm110.GRADE_CD_NEW == "2" && Convert.ToInt32(checkRB.Rows[0]["AGE"]) < 18)
                        rtnmessage += "此員工無達到晉升資格年齡(必須大於等於18歲)，或能力考核不符\\n";
                    if (fb2sm110.GRADE_CD_NEW == "3" && Convert.ToInt32(checkRB.Rows[0]["AGE"]) < 26)
                        rtnmessage += "此員工無達到晉升資格年齡(必須大於等於26歲)，或能力考核不符\\n";
                    if (fb2sm110.GRADE_CD_NEW == "4" && Convert.ToInt32(checkRB.Rows[0]["AGE"]) < 36)
                        rtnmessage += "此員工無達到晉升資格年齡(必須大於等於36歲)，或能力考核不符\\n";
                }
            }
            */

            /*
            //判斷級數-改到晉級功能SM210
            if (fb2sm110.LEVEL_CD_NEW != "" && fb2sm110.GRADE_CD_NEW != "")
            {
                string oriLevel = fb2sm110.getOriLevelCD();
                string oriGrade = fb2sm110.getOriGrade();
                string oldGradeCDSeq = "0";
                string newGradeCDSeq = fb2sm110.getGradeSeq(fb2sm110.LEVEL_CD_NEW, fb2sm110.GRADE_CD_NEW);
                if (oriGrade.Trim() == "")
                {
                    oriGrade = "0";
                }
                else
                {
                    oldGradeCDSeq = fb2sm110.getGradeSeq(oriLevel, oriGrade);
                }
                if (Convert.ToInt32(newGradeCDSeq) <= Convert.ToInt32(oldGradeCDSeq))
                {
                    rtnmessage += "晉昇資格級數不可低於原資格級數\\n";
                }
                else if (Convert.ToInt32(newGradeCDSeq) - Convert.ToInt32(oldGradeCDSeq) > 1)
                {
                    rtnmessage += "不可連續晉昇1級以上\\n";
                }

                
                //if (Convert.ToInt32(oriGrade) - Convert.ToInt32(fb2sm110.GRADE_CD_NEW) > 1)
                // {
                //    rtnmessage += "不可連續晉昇1級以上\\n";
                //}
                
            }
            */


            return rtnmessage;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    #endregion

    #region "Initial Page"
    public DataTable getLEVEL_CD()
    {
        CFB2SM1100DAO wfb2sm = new CFB2SM1100DAO();
        try
        {
            return wfb2sm.getLEVEL_CD();
        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable getPJOB_CD_NEW()
    {
        CFB2SM1100DAO wfb2sm = new CFB2SM1100DAO();
        try
        {
            return wfb2sm.getPJOB_CD_NEW();
        }
        catch (Exception)
        {

            throw;
        }
    }
    #endregion

    #region "Import"
    public IWorkbook uploadExcel(System.IO.Stream fs, string type, string DATA_YEAR, string DATA_SEQ)
    {
        try
        {
            bool valid = true;
            CFB2SM1100DAO dao = new CFB2SM1100DAO();
            IWorkbook workbook;
            //依附檔名判斷要用哪種方式讀取
            if (type == ".xls")
                workbook = new HSSFWorkbook(fs);
            else
                workbook = new XSSFWorkbook(fs);

            //取得sheet
            ISheet sheet = workbook.GetSheetAt(0);
            ICellStyle style1 = workbook.CreateCellStyle();
            IFont font1 = workbook.CreateFont();
            font1.Color = HSSFColor.Red.Index;
            style1.SetFont(font1);
            if (sheet != null)
            {
                //List<string> EMPIDs = new List<string>();
                ////先撈出晉昇人員生成檔的所有EMP_ID，放到list
                //DataTable Emp_dt = dao.checkAllTEXemp_id();
                //if (Emp_dt.Rows.Count > 0)
                //{
                //    for (int i = 0; i < Emp_dt.Rows.Count; i++)
                //    {
                //        EMPIDs.Add(Emp_dt.Rows[i]["EMP_ID"].ToString());
                //    }
                //}

                string emp_ids = "";
                //巡覽每row的資料第一列為title跳過
                for (int i = 3; i <= sheet.LastRowNum; i++)
                {
                    dao.DATA_YEAR = DATA_YEAR;
                    dao.DATA_SEQ = DATA_SEQ;
                    if (sheet.GetRow(i) != null)
                    {
                        //讀取cell資料，第一欄為檢核結果欄位跳過
                        string cell1 = sheet.GetRow(i).GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();//工號
                        string cell2 = sheet.GetRow(i).GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();//姓名
                        string cell3 = sheet.GetRow(i).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();//晉昇資料
                        string cell4 = sheet.GetRow(i).GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();//晉昇級數
                        string cell5 = sheet.GetRow(i).GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();//晉昇職務代號

                        string error = "";
                        int numCheckResult = 0;
                        //工號
                        if (cell1 == "")
                        {
                            error += "工號欄位不可空白 \n";
                            dao.EMP_ID = "";
                        }
                        else
                        {
                            if (cell1.Trim().Length != 5 || !int.TryParse(cell1.Trim(), out numCheckResult))
                            {
                                error += "工號必須為數字, 且長度必須為5 \n";
                                dao.EMP_ID = "";
                            }
                            else
                            {
                                dao.EMP_ID = cell1;
                            }
                        }

                        //姓名
                        if (cell2 == "")
                        {
                            dao.EMP_NAME = "";
                            error += "姓名欄位不可空白\n";
                        }
                        else
                        {
                            dao.EMP_NAME = cell2;
                        }

                        //晉昇資格
                        if (cell3 == "")
                        {
                            dao.LEVEL_CD_NEW = "";
                            error += "晉昇資格欄位不可空白\n";
                        }
                        else
                        {
                            dao.LEVEL_CD_NEW = cell3;
                        }

                        //晉昇級數-因是晉昇,故上傳時,級數需為空(RB除外)
                        if (cell4 != "")
                        {
                            error += "晉昇時,級數需為空白\n";
                        }

                        /*
                        if (cell4 != "")
                        {
                            dao.GRADE_CD_NEW = cell4;
                        }
                        else
                        {
                            dao.GRADE_CD_NEW = "";
                        }
                        */

                        //晉昇職務代號
                        if (cell5 == "")
                        {
                            dao.PJOB_CD_NEW = "";
                            error += "晉昇職務代號欄位不可空白\n";
                        }
                        else
                        {
                            dao.PJOB_CD_NEW = cell5;
                        }

                        //工號需存在人事主檔
                        if (cell1 != "" && cell1 != "error")
                        {
                            DataTable tmp = dao.getEmpData(cell1);
                            if (tmp.Rows.Count == 0)
                            {
                                error += "此工號不存在 \n";
                            }
                        }

                        //工號 + 姓名需存在人事主檔
                        if (cell1 != "" && cell2 != "")
                        {
                            DataTable tmp = dao.getEmpName(cell1, cell2);
                            if (tmp.Rows.Count == 0)
                            {
                                error += "工號、姓名不符 \n";
                            }
                        }

                        //晉昇資格需存在資格檔
                        if (cell3 != "")
                        {
                            DataTable tmp = dao.getLeaveCd(cell3);
                            if (tmp.Rows.Count == 0)
                            {
                                error += "無此晉昇資格 \n";
                            }
                        }

                        //晉昇職務需存在職務檔
                        if (cell5 != "")
                        {
                            DataTable tmp = dao.getPjobCd(cell5);
                            if (tmp.Rows.Count == 0)
                            {
                                error += "無此晉昇職務 \n";
                            }
                        }
                        //其它的檢核
                        error += checkInsertData(dao);

                        //員工工號 存在於 晉昇人員生成檔(檢查DB)
                        DataTable emp_id = dao.getemp_idImport();
                        if (emp_id.Rows.Count > 0)
                        {
                            error += "此工號在同一年度已存在晉升資料";
                        }

                        //員工工號 存在於 晉昇人員生成檔(檢查EXCEL)
                        if (emp_ids.Contains(cell1))
                        {
                            error += "此工號在此檔案重覆出現";
                        }
                        else
                        {
                            emp_ids += cell1 + ",";
                        }




                        error = error.Replace("\\n", "\r\n");
                        sheet.GetRow(i).CreateCell(0).CellStyle = style1;
                        sheet.GetRow(i).GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(error);

                        if (error.Trim().Length > 0)
                        {
                            valid = false;
                        }
                    }
                }

            }
            if (!valid)
            {
                //RollBack();
                return workbook;
            }
            else
            {
                BeginTransaction();
                for (int i = 3; i <= sheet.LastRowNum; i++)
                {
                    dao.DATA_YEAR = DATA_YEAR;
                    dao.DATA_SEQ = DATA_SEQ;
                    if (sheet.GetRow(i) != null)
                    {
                        //讀取cell資料，第一欄為檢核結果欄位跳過
                        string cell1 = sheet.GetRow(i).GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                        string cell2 = sheet.GetRow(i).GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                        string cell3 = sheet.GetRow(i).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                        string cell4 = sheet.GetRow(i).GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                        string cell5 = sheet.GetRow(i).GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();

                        dao.EMP_ID = cell1;
                        dao.EMP_NAME = cell2;
                        dao.LEVEL_CD_NEW = cell3;
                        dao.PJOB_CD_NEW = cell5;


                        DateTime currentDate = DateTime.Now;
                        DateTime current_yearEndDate = new DateTime(DateTime.Now.Year, 12, 31);
                        TimeSpan tsDay = current_yearEndDate - currentDate;
                        int dayCount = Convert.ToInt32(tsDay.Days) + 1;

                        DataTable dtPjob = dao.getPJOB_CD_DESC();
                        if (dtPjob.Rows.Count > 0)
                        {
                            dao.PJOB_DESC_NEW = dtPjob.Rows[0]["PJOB_DESC"].ToString();
                        }
                        DataTable dt = dao.getEMP_ID_data(cell1);


                        dao.LEVEL_CD = dt.Rows[0]["LEVEL_CD"].ToString();
                        dao.GRADE_CD = dt.Rows[0]["GRADE_CD"].ToString();
                        dao.DEPT_NO = dt.Rows[0]["DEPT_NO"].ToString();
                        dao.DEPT_NAME = dt.Rows[0]["DEPT_NAME"].ToString();
                        dao.EMP_NAME = dt.Rows[0]["EMP_NAME"].ToString();
                        dao.WS_CD = dt.Rows[0]["WS_CD"].ToString();
                        dao.PJOB_CD = dt.Rows[0]["PJOB_CD"].ToString();
                        dao.PJOB_DESC = dt.Rows[0]["PJOB_DESC"].ToString();

                        dao.EMP_CHG_CD = dt.Rows[0]["EMP_CHG_CD"].ToString();
                        dao.EMP_CHG_DESC = dt.Rows[0]["EMP_CHG_DESC"].ToString();

                        double work_year_toEndDay = (Convert.ToDouble(dt.Rows[0]["WORK_DAYS"]) + dayCount) / 365;                  //在職天數算到年底 /365算年
                        double level_work_year_toEndDay = Convert.ToDouble(dt.Rows[0]["LEVEL_WORK_DAYS_toEnd"]) / 365;      //任現資格天數算到年底 /365算年
                        dao.WORK_YEARS = Math.Round(work_year_toEndDay, 1, MidpointRounding.AwayFromZero).ToString();
                        dao.LEVEL_WORK_YEARS = Math.Round(level_work_year_toEndDay, 1, MidpointRounding.AwayFromZero).ToString();

                        DataTable dt2score = dao.get2score(cell1);
                        if (dt2score.Rows.Count > 0)
                            dao.ASSESS_SCORE_1 = dt2score.Rows[0]["SCORE_1H"].ToString();
                        else
                            dao.ASSESS_SCORE_1 = "";

                        if (dt2score.Rows.Count > 1)
                            dao.ASSESS_SCORE_2 = dt2score.Rows[1]["SCORE_1H"].ToString();
                        else
                            dao.ASSESS_SCORE_2 = "";

                        if (dt2score.Rows.Count > 2)
                            dao.ASSESS_SCORE_3 = dt2score.Rows[2]["SCORE_1H"].ToString();
                        else
                            dao.ASSESS_SCORE_3 = "";

                        if (dt2score.Rows.Count > 3)
                            dao.ASSESS_SCORE_4 = dt2score.Rows[3]["SCORE_1H"].ToString();
                        else
                            dao.ASSESS_SCORE_4 = "";

                        if (dt2score.Rows.Count > 4)
                            dao.ASSESS_SCORE_5 = dt2score.Rows[4]["SCORE_1H"].ToString();
                        else
                            dao.ASSESS_SCORE_5 = "";

                        dao.CREATED_BY = SessionHandle.Current.emp_id;
                        dao.UPDATED_BY = SessionHandle.Current.emp_id;
                        dao.FUNC_ID = "FB2SM100";
                        dao.addPromotiondtl("I");
                    }
                }

                Commit();
                return null;
            }
        }
        catch
        {
            RollBack();
            throw;
        }
    }
    #endregion

}
