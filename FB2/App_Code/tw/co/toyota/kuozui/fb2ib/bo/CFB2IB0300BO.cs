using FB2.tw.co.toyota.kuozui.bo;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

/// <summary>
/// CFB2IB0300BO 的摘要描述
/// </summary>
public class CFB2IB0300BO : BaseService
{
	public CFB2IB0300BO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    public DataTable checkData(string YM)
    {
        try
        {
            CFB2IB0300DAO dao = new CFB2IB0300DAO();

            return dao.selectData(YM);

        }
        catch (Exception)
        {

            throw;
        }
    }

    public string uploadExcel(Stream fs, string type)
    {
        CFB2IB0300DAO dao = new CFB2IB0300DAO();
        try
        {
            //取得登入者
            string userid = SessionHandle.Current.emp_id;

            bool valid = true;           

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
                try
                {
                    BeginTransaction();

                    //刪除 登入者 之前 所建立的資料
                    //dao.delBefore(userid);

                    //取得所有EXCEL中的KEY
                    //List<string> keys = new List<string>();
                    //for (int i = 1; i <= sheet.LastRowNum; i++)
                    //{
                    //    if (sheet.GetRow(i) != null)
                    //    {
                    //        string c1 = sheet.GetRow(i).GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                    //        string c2 = sheet.GetRow(i).GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                    //        if (c1 != "")
                    //        {
                    //            if (c1.Length == 5)
                    //            {
                    //                c1 = Convert.ToString(Convert.ToInt32(c1.Substring(0, 3)) + 1911) + c1.Substring(3, 2);
                    //            }
                    //            else
                    //            {
                    //                c1 = Convert.ToString(Convert.ToInt32(c1.Substring(0, 2)) + 1911) + c1.Substring(2, 2);
                    //            }
                    //        }

                    //        keys.Add(c1+c2);                         
                    //    }                       
                    //}

                    //取得所有EXCEL中的年月
                    List<string> allYM = new List<string>();
                    for (int i = 1; i <= sheet.LastRowNum; i++)
                    {
                        if (sheet.GetRow(i) != null)
                        {
                            string c1 = sheet.GetRow(i).GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().Replace("/","");
                            //if (c1 != "")
                            //{
                                //if (c1.Length == 5)
                                //{
                                //    c1 = Convert.ToString(Convert.ToInt32(c1.Substring(0, 3)) + 1911) + c1.Substring(3, 2);
                                //}
                                //else
                                //{
                                //    c1 = Convert.ToString(Convert.ToInt32(c1.Substring(0, 2)) + 1911) + c1.Substring(2, 2);
                                //}
                            //}
                            if (!allYM.Contains(c1))
                            {
                                allYM.Add(c1);
                            }                            
                        }
                    }

                    //刪除與EXCEL相同年月的資料
                    foreach (string item in allYM)
                    {
                        dao.deleteCOMPANY_BILL(item);
                    }

                    //Create Row
                    string[] cell1 = new string[sheet.LastRowNum + 1];
                    string[] cell2 = new string[sheet.LastRowNum + 1];
                    string[] cell3 = new string[sheet.LastRowNum + 1];
                    string[] cell4 = new string[sheet.LastRowNum + 1];
                    string[] cell5 = new string[sheet.LastRowNum + 1];
                    string[] cell6 = new string[sheet.LastRowNum + 1];
                    string[] cell7 = new string[sheet.LastRowNum + 1];
                    string[] cell8 = new string[sheet.LastRowNum + 1];
                    string[] cell9 = new string[sheet.LastRowNum + 1];
                    string[] cell10 = new string[sheet.LastRowNum + 1];
                    string[] cell11 = new string[sheet.LastRowNum + 1];
                    string[] cell12 = new string[sheet.LastRowNum + 1];
                    string[] cell13 = new string[sheet.LastRowNum + 1];

                    //巡覽每row的資料第一列為title跳過
                    for (int i = 1; i <= sheet.LastRowNum; i++)
                    {
                        bool b = true;
                        if (sheet.GetRow(i) != null)
                        {
                            #region 讀取cell資料，第一欄為檢核結果欄位跳過
                            cell1[i] = sheet.GetRow(i).GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().Replace("/","");//年月(6)
                            cell2[i] = sheet.GetRow(i).GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();//對象別(5)
                            cell3[i] = sheet.GetRow(i).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();//廠商名稱(30)
                            cell4[i] = sheet.GetRow(i).GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();//付款統一編號(20)
                            cell5[i] = sheet.GetRow(i).GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().Replace("/", "");//支付日期(8)
                            cell6[i] = sheet.GetRow(i).GetCell(6, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();//傳票號碼(10)
                            cell7[i] = sheet.GetRow(i).GetCell(7, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();//項次(MAX 5)
                            cell8[i] = sheet.GetRow(i).GetCell(8, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();//憑證別(2)
                            cell9[i] = sheet.GetRow(i).GetCell(9, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();//憑證別名稱(50)
                            cell10[i] = sheet.GetRow(i).GetCell(10, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();//憑證別格式代號(3)
                            cell11[i] = sheet.GetRow(i).GetCell(11, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();//補充保費類別(2)
                            cell12[i] = sheet.GetRow(i).GetCell(12, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().Replace(",","");//金額(10)
                            cell13[i] = sheet.GetRow(i).GetCell(13, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().Replace(",", "");//補充保費扣繳金額(7)                           
                            #endregion
                            string error = "";

                            #region 開始檢查
                            error += utilities.checkNumber(cell1[i], "年月", 6, false);
                            error += utilities.checkLength(cell2[i], "對象別", 5, false);
                            error += utilities.checkLength(cell3[i], "廠商名稱", 30, false);
                            error += utilities.checkLength(cell4[i], "付款統一編號", 20, false);
                            error += utilities.checkNumber(cell5[i], "支付日期", 8, false);
                            error += utilities.checkLength(cell6[i], "傳票號碼", 10, false);
                            error += utilities.checkNumber(cell7[i], "項次", 5, false);
                            error += utilities.checkLength(cell8[i], "憑證別", 2, false);
                            error += utilities.checkLength(cell9[i], "憑證別名稱", 50, false);
                            error += utilities.checkLength(cell10[i], "憑證別格式代號", 2, false);
                            error += utilities.checkNumber(cell11[i], "補充保費類別", 10, false);
                            error += utilities.checkNumber(cell12[i], "金額", 6, false);
                            error += utilities.checkNumber(cell13[i], "補充保費扣繳金額", 7, false);
                                                        
                            #endregion


                            
                            //傳出錯誤訊息
                            style1.SetFont(font1);
                            sheet.GetRow(i).CreateCell(0).CellStyle = style1;
                            sheet.GetRow(i).GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(error);
                            if (error != "")
                            {
                                valid = false;
                            }
                            else
                            {
                                dao.YM = cell1[i];
                                dao.EMP_ID = cell2[i];
                                dao.EMP_NAME = cell3[i];
                                dao.LICENSE_ID = cell4[i];
                                dao.PAYMENT_DATE = cell5[i];
                                dao.BILL_NO = cell6[i];
                                dao.ITEM_SEQ = cell7[i];
                                dao.VCHID = cell8[i];
                                dao.VCH_NAME = cell9[i];
                                dao.TAX_FORMAT = cell10[i];
                                dao.CODE_CD = cell11[i];
                                dao.AMOUNT = cell12[i];
                                dao.INS_COST = cell13[i];                                
                       
                                dao.CREATED_BY = SessionHandle.Current.emp_id.Trim();
                                dao.UPDATED_BY = SessionHandle.Current.emp_id.Trim();
                                dao.FUNC_ID = "FB2IB030";
                                
                                dao.insertCOMPANY_BILL();
                            }

                        }

                    } if (sheet.LastRowNum == 0)
                    {
                        string error = "請輸入上傳資料\n";
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
                        RollBack();
                        //檢核有錯，匯出附加說明的excel
                        ExcelHandle.exportExcel(workbook, "檢核錯誤說明" + type);
                    }
                    else
                        Commit();
                }
                catch (Exception ex)
                {
                    RollBack();
                    return ex.Message;
                }
            }

            return "0";
        }
        catch (Exception ex)
        {
            return ex.Message;

        }

    }

    public bool IsNumeric(String strNumber)
    {               
        Regex NumberPattern=new Regex("[^0-9.-]");  
        return !NumberPattern.IsMatch(strNumber);
    }

    public static bool FullWidthWord(string values)
    {
        bool result = false;
        string pattern = @"^[\u4E00-\u9fa5]+$";
        foreach (char item in values)
        {
            //以Regex判斷是否為中文字，中文字視為全形  
            if (!Regex.IsMatch(item.ToString(), pattern))
            {
                //以16進位值長度判斷是否為全形字  
                if (string.Format("{0:X}", Convert.ToInt32(item)).Length != 2)
                {
                    result = true;
                    break;
                }
            }
        }
        return result;
    }

    public static bool IsChinese(string values)
    {
        bool result = false;
        string pattern = @"^[\u4E00-\u9fa5]+$";
        foreach (char item in values)
        {
            //以Regex判斷是否為中文字，中文字視為全形  
            if (!Regex.IsMatch(item.ToString(), pattern))
            {
                result = true;
                break;             
            }
        }
        return result;
    }
    
}