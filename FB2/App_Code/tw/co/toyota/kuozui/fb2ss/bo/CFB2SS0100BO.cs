using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

/// <summary>
/// CFB2SQ0100BO 的摘要描述
/// </summary>
public class CFB2SS0100BO : BaseService
{
    public CFB2SS0100BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public string doExec(CFB2SS0100DAO dao)
    {

        try
        {
            //call sp
            dao.SP_S_SS010();

            //確認SP有無成功(是SP_S_FIRED_PAY_COMPUTE,不是SP_S_SS010)
            DataTable dtSPresult = dao.checkSP("SP_S_FIRED_PAY_COMPUTE");
            if (dtSPresult.Rows.Count > 0)
            {
                //PROC_STATUS：Y = 成功,N = 失敗,E = Exception  ,PROC_LOG：處理結果中文訊息
                if (Convert.ToString(dtSPresult.Rows[0]["PROC_STATUS"]) != "Y")
                    return Convert.ToString(dtSPresult.Rows[0]["PROC_LOG"]) + "\\n";
            }

            return "0";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    //是否已轉傳薪資,節金是否已存在
    public string chkIS_SEND(CFB2SS0100DAO dao)
    {
        try
        {
            string st = "";
            string msg = "0";
            DataTable dt = dao.chkIS_SEND();
            if (dt.Rows.Count > 0)
            {
                st = dt.Rows[0]["cnt"].ToString();
                if (st != "0")
                {
                    msg = "此發薪日期+獎金類型已轉薪資！";
                    return msg;
                }
            }

            //節金檔是否有相同節金類型及發放日期
            string rtnMsg = dao.checkFN_SS_CHK_FESTIVAL("A"); //A-節金是否已存在
            if (rtnMsg != "")
            {
                msg = rtnMsg;
            }
            return msg;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public IWorkbook uploadExcel(Stream fs, string type, CFB2SS0100DAO dao)
    {
        try
        {
            //取得登入者
            string userid = SessionHandle.Current.emp_id;
            bool valid = true;
            ArrayList list = new ArrayList();

            IWorkbook workbook;
            //依附檔名判斷要用哪種方式讀取
            if (type == ".xls")
            {
                workbook = new HSSFWorkbook(fs);
            }
            else
            {
                workbook = new XSSFWorkbook(fs);
            }

            //取得sheet
            ISheet sheet = workbook.GetSheetAt(0);
            ICellStyle style1 = workbook.CreateCellStyle();
            IFont font1 = workbook.CreateFont();

            font1.Color = HSSFColor.Red.Index;


            if (sheet != null)
            {
                #region cell陣列
                string[] EMP_ID = new string[sheet.LastRowNum + 1];
                string[] EMP_NAME = new string[sheet.LastRowNum + 1];
                string[] FIRED_DT = new string[sheet.LastRowNum + 1];
                string[] SPECIAL_PAY = new string[sheet.LastRowNum + 1];   //特勤津貼
                string[] OTHER_PAY = new string[sheet.LastRowNum + 1];    //其他津貼
                string[] RETENTION_YY = new string[sheet.LastRowNum + 1];   //留停-年
                string[] RETENTION_MM = new string[sheet.LastRowNum + 1];   //留停-月
                string[] RETENTION_DD = new string[sheet.LastRowNum + 1];   //留停-日

                #endregion
                try
                {
                    string checkMsg = "";
                    string error = "";
                    //巡覽每row的資料第一列為title跳過
                    for (int i = 1; i <= sheet.LastRowNum; i++)
                    {
                        if (sheet.GetRow(i) != null)
                        {
                            #region 檢核邏輯
                            EMP_ID[i] = sheet.GetRow(i).GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().ToUpper();
                            EMP_NAME[i] = sheet.GetRow(i).GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().ToUpper();
                            FIRED_DT[i] = sheet.GetRow(i).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().ToUpper();
                            SPECIAL_PAY[i] = sheet.GetRow(i).GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().ToUpper();
                            OTHER_PAY[i] = sheet.GetRow(i).GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().ToUpper();
                            RETENTION_YY[i] = sheet.GetRow(i).GetCell(6, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().ToUpper();
                            RETENTION_MM[i] = sheet.GetRow(i).GetCell(7, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().ToUpper();
                            RETENTION_DD[i] = sheet.GetRow(i).GetCell(8, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().ToUpper();

                            //若空白則為0
                            SPECIAL_PAY[i] = SPECIAL_PAY[i] == "" ? "0" : SPECIAL_PAY[i];
                            OTHER_PAY[i] = OTHER_PAY[i] == "" ? "0" : OTHER_PAY[i];
                            RETENTION_YY[i] = RETENTION_YY[i] == "" ? "0" : RETENTION_YY[i];
                            RETENTION_MM[i] = RETENTION_MM[i] == "" ? "0" : RETENTION_MM[i];
                            RETENTION_DD[i] = RETENTION_DD[i] == "" ? "0" : RETENTION_DD[i];

                            #endregion
                            

                            //開始檢查
                            #region 檢核邏輯
                            error = "";
                            //長度檢核
                            error += utilities.checkNumber(EMP_ID[i], "工號", 5, false);
                            error += utilities.checkLength(EMP_NAME[i], "姓名", 30, false);
                            error += utilities.checkDateFormat(FIRED_DT[i], "資遣日", false);
                            error += utilities.checkNumber(SPECIAL_PAY[i], "特勤津貼", 7, true);
                            error += utilities.checkNumber(OTHER_PAY[i], "其他津貼", 7, true);
                            error += utilities.checkNumber(RETENTION_YY[i], "(留停)年", 2, true);
                            error += utilities.checkNumber(RETENTION_MM[i], "(留停)月", 2, true);
                            error += utilities.checkNumber(RETENTION_DD[i], "(留停)日", 2, true);

                            //資料正確性檢核(測試時可註解)
                            checkMsg = dao.checkFN_SS010_CHK(EMP_ID[i], EMP_NAME[i], FIRED_DT[i]);
                            if (checkMsg != "")
                            {
                                error += checkMsg + "\n";
                            }

                            /*
                            checkBool = dao.checkNAME(EMP_ID[i], EMP_NAME[i]);
                            if (!checkBool)
                            {
                                error += "此工號與姓名不相符,無法計算\n";
                            }
                            //檢查該員工是否已有其它資遺資料
                            checkMsg = this.chkIS_CLOSE(EMP_ID[i]);
                            if (checkMsg != "0") {
                                error += checkMsg+"\n";
                            }
                             * */
                            #endregion

                            //傳出錯誤訊息
                            style1.SetFont(font1);
                            sheet.GetRow(i).CreateCell(0).CellStyle = style1;
                            sheet.GetRow(i).GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(error);
                            if (error != "")
                            {
                                valid = false;
                            }

                        }//if end

                    } //for end

                    if (sheet.LastRowNum == 0)
                    {
                        error = "請輸入上傳資料\n";
                        style1.SetFont(font1);
                        sheet.GetRow(0).CreateCell(0).CellStyle = style1;
                        //傳出錯誤訊息  
                        sheet.GetRow(0).GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(error);
                        if (error != "")
                        {
                            valid = false;
                        }
                    }
                    if (!valid)
                    {
                        //檢核有錯，匯出附加說明的excel
                        return workbook;
                        //檢核有錯，匯出附加說明的excel
                        //ExcelHandle.exportExcel(workbook, "檢核錯誤說明" + type);
                    }
                    else
                    {
                        BeginTransaction();
                        //刪除暫存檔
                        dao.deleteFiredPayTable("TB_S_S_FIRED_TEMP");
                        //刪除TB_S_M_FIRED_PAY	資遺費計算主檔
                        dao.deleteFiredPayTable("TB_S_M_FIRED_PAY");
                        //刪除TB_S_M_FIRED_PAY_H	資遺費計算員工檔
                        dao.deleteFiredPayTable("TB_S_M_FIRED_PAY_H");
                        //刪除TB_S_M_FIRED_PAY_D	資遺費計算員工明細檔
                        dao.deleteFiredPayTable("TB_S_M_FIRED_PAY_D");

                        for (int i = 1; i <= sheet.LastRowNum; i++)
                        {
                                                   
                            try
                            {
                                dao.EMP_ID = EMP_ID[i];
                                dao.FIRED_DT = FIRED_DT[i];
                                dao.SPECIAL_PAY = SPECIAL_PAY[i];
                                dao.OTHER_PAY = OTHER_PAY[i];
                                dao.RETENTION_YY = RETENTION_YY[i];
                                dao.RETENTION_MM = RETENTION_MM[i];
                                dao.RETENTION_DD = RETENTION_DD[i];
                                dao.CREATED_BY = SessionHandle.Current.emp_id.Trim();
                                dao.UPDATED_BY = SessionHandle.Current.emp_id.Trim();
                                dao.FUNC_ID = "FB2SS010";
                                //新增資遣費名單暫存檔
                                dao.addTmpData();
                            }
                            catch (Exception ex)
                            {
                                RollBack();
                                throw;
                            }
                        }
                        Commit();
                    }
                }
                catch (Exception ex)
                {
                    RollBack();
                    throw;
                    //return ex.Message;
                }
            }

            return null;
        }
        catch (Exception ex)
        {
            throw;

        }

    }

}