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
public class CFB2SS0400BO : BaseService
{
    public CFB2SS0400BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    //是否已轉傳薪資,節金是否已存在
    public string chkIS_SEND(CFB2SS0400DAO dao)
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

    public string exec_SP_S_SS040(CFB2SS0400DAO dao)
    {

        string rtnmessage = "";//檢查後的訊息
        try
        {
            dao.exec_SP_S_SS040();
            rtnmessage += utilities.getSPLOG("SP_S_SS040");
            if (rtnmessage != "")
            {
                return rtnmessage;
            }
            return "0";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    #region EXCEL上傳
    public IWorkbook uploadExcel(Stream fs, string type, CFB2SS0400DAO dao)
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


            dao.CREATED_BY = SessionHandle.Current.emp_id.Trim();
            dao.UPDATED_BY = SessionHandle.Current.emp_id.Trim();
            dao.FUNC_ID = "FB2SS040";

            BeginTransaction();           
            //一開始就刪除
            dao.deleteINCENTIVE_PAY("TB_S_M_INCENTIVE_PAY_H");
            dao.deleteINCENTIVE_PAY("TB_S_M_INCENTIVE_PAY_D");
            Commit();


            if (sheet != null)
            {
                #region cell陣列
                string[] EMP_ID = new string[sheet.LastRowNum + 1];
                string[] EMP_NAME = new string[sheet.LastRowNum + 1];

                #endregion
                try
                {
                    bool checkBool = true;
                    string checkMsg = "";
                    
                    //巡覽每row的資料第一列為title跳過
                    for (int i = 1; i <= sheet.LastRowNum; i++)
                    {
                        checkBool = true;
                        if (sheet.GetRow(i) != null)
                        {
                            #region 讀取cell資料，第一欄為檢核結果欄位跳過
                            EMP_ID[i] = sheet.GetRow(i).GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().ToUpper();
                            EMP_NAME[i] = sheet.GetRow(i).GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().ToUpper();

                            #endregion

                            string error = "";

                            //開始檢查
                            #region 檢核基本邏輯
                            //長度檢核
                            error += utilities.checkNumber(EMP_ID[i], "工號", 5, false);
                            error += utilities.checkLength(EMP_NAME[i], "姓名", 50, false);
                            
                            //格式檢核                            
                            checkBool = dao.checkNAME(EMP_ID[i], EMP_NAME[i]);
                            if (!checkBool)
                            {
                                error += "此工號與姓名不相符,無法計算\n";
                            }

                            checkMsg = dao.checkFunction(EMP_ID[i]);
                            if (checkMsg != "")
                            {
                                error += checkMsg + "\n";
                            }
                            
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
                        string error = "請輸入上傳資料\n";
                        style1.SetFont(font1);
                        sheet.GetRow(0).CreateCell(0).CellStyle = style1;
                        //傳出錯誤訊息  
                        sheet.GetRow(0).GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(error);
                        if (error != ""  )
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

                        //新增
                        dao.insertINCENTIVE_PAY_H();

                        for (int i = 1; i <= sheet.LastRowNum; i++)
                        {
                            //新增                            
                            try
                            {
                                dao.EMP_ID = EMP_ID[i];
                                //新增資遣費名單暫存檔
                                dao.insertINCENTIVE_PAY_D();
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

    #endregion

}