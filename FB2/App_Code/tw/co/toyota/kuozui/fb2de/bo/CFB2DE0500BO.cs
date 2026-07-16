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
/// CFB2DE0500BO 的摘要描述
/// </summary>
public class CFB2DE0500BO : BaseService
{
	public CFB2DE0500BO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    public IWorkbook uploadExcel(Stream fs, string type, CFB2DE0500DAO dao)
    {        
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

                    //刪除與EXCEL相同年月的資料
                    dao.del_Old_Res_Bond();

                    //巡覽每row的資料第一列為title跳過
                    for (int i = 1; i <= sheet.LastRowNum; i++)
                    {
                        bool b = true;
                        if (sheet.GetRow(i) != null)
                        {
                            //讀取cell資料，第一欄為檢核結果欄位跳過
                            string cell1 = sheet.GetRow(i).GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            string cell2 = sheet.GetRow(i).GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            string cell3 = sheet.GetRow(i).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            string cell4 = sheet.GetRow(i).GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            string cell5 = sheet.GetRow(i).GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            string cell6 = sheet.GetRow(i).GetCell(6, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            string cell7 = sheet.GetRow(i).GetCell(7, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            string cell8 = sheet.GetRow(i).GetCell(8, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            string cell9 = sheet.GetRow(i).GetCell(9, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            string cell10 = sheet.GetRow(i).GetCell(10, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            string cell11 = sheet.GetRow(i).GetCell(11, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                           
                            string error = "";

                            //開始檢查            

                            //負擔部門
                            if (cell1 == "")
                                error += "負擔部門不可為空白\n";
                            else
                            {
                                if (cell1.Length != 5)
                                {
                                    error += "負擔部門長度錯誤\n";
                                }
                                else
                                {
                                    if (!utilities.IsNatural_Number(cell1))
                                    {
                                        error += "負擔部門只能輸入英數字\n";
                                    }
                                    else
                                    {
                                        //讀取薪資部門區分設定檔(TB_H_M_DEPT_ACC)，如欄位一不存在於COST_DEPT_NO中，則顯示錯誤訊息"負擔部門不存在"
                                        dao.COST_DEPT_NO = cell1;
                                        DataTable dt_Dept_Acc = dao.select_DEPT_ACC();
                                        if (dt_Dept_Acc.Rows.Count > 0)
                                        {
                                            int acc_row = Convert.ToInt32(dt_Dept_Acc.Rows[0]["rows"].ToString());
                                            if (acc_row == 0)
                                            {
                                                error += "負擔部門不存在\n";
                                            }
                                        }
                                    }
                                }

                            }

                            

                            //餐券用途區分
                            //if (cell2 == "")
                            //    error += "餐券用途區分不可為空白\n";
                            //else
                            //{
                            //    if (cell2.Length != 1)
                            //    {
                            //        error += "餐券用途區分長度錯誤\n";
                            //    }
                            //    else
                            //    {
                            //        if (!IsNumeric(cell2))
                            //        {
                            //            error += "餐券用途區分只能輸入數字\n";
                            //        }
                            //        else
                            //        {
                            //            DataTable dt_Bond_For = dao.select_Bond_For();
                            //            if (dt_Bond_For.Rows.Count > 0)
                            //            {
                            //                int bond_row = Convert.ToInt32(dt_Bond_For.Rows[0]["rows"].ToString());
                            //                if (bond_row == 0)
                            //                {
                            //                    error += "餐券用途區分不存在\n";
                            //                }
                            //            }
                            //        }
                            //    }

                            //}


                            //來賓餐券數量
                            //if (cell2 == "")
                            //    error += "餐券用途區分不可為空白\n";
                            //else
                            if (cell2 != "")
                            {
                                //有數量時，單價不能空白
                                if (cell3.Length == 0)
                                {
                                    error += "來賓餐券單價錯誤，不能為空白\n";
                                }
                                if (cell2.Length > 5)
                                {
                                    error += "來賓餐券數量長度錯誤\n";
                                }
                                else
                                {
                                    if (!IsNumeric(cell2))
                                    {
                                        error += "來賓餐券數量只能輸入數字\n";
                                    }
                                   
                                }

                            }

                            //來賓餐券單價
                            //if (cell3 == "")
                            //    error += "來賓餐券單價不可為空白\n";
                            //else
                            if (cell3 != "")                            
                            {
                                //有單價時，數量不能空白
                                if (cell2.Length == 0)
                                {
                                    error += "來賓餐券1數量錯誤，不能為空白\n";
                                }
                                if (cell3.Length > 5)
                                {
                                    error += "來賓餐券單價長度錯誤\n";
                                }
                                else
                                {
                                    if (!IsNumeric(cell3))
                                    {
                                        error += "來賓餐券單價只能輸入數字\n";
                                    }
                                }
                            }

                            //貴賓餐券1數量
                            //if (cell4 == "")
                            //    error += "貴賓餐券1數量不可為空白\n";
                            //else
                            if (cell4 != "")
                            {
                                //有數量時，單價不能空白
                                if (cell5.Length == 0)
                                {
                                    error += "貴賓餐券1單價錯誤，不能為空白\n";
                                }
                                if (cell4.Length > 5)
                                {
                                    error += "貴賓餐券1數量長度錯誤\n";
                                }
                                else
                                {
                                    if (!IsNumeric(cell4))
                                    {
                                        error += "貴賓餐券1數量只能輸入數字\n";
                                    }

                                }

                            }

                            //貴賓餐券1單價
                            //if (cell5 == "")
                            //    error += "貴賓餐券1單價不可為空白\n";
                            //else
                            if (cell5 != "")
                            {
                                //有單價時，數量不能空白
                                if (cell4.Length == 0)
                                {
                                    error += "貴賓餐券1數量錯誤，不能為空白\n";
                                }
                                if (cell5.Length > 5)
                                {
                                    error += "貴賓餐券1單價長度錯誤\n";
                                }
                                else
                                {
                                    if (!IsNumeric(cell5))
                                    {
                                        error += "貴賓餐券1單價只能輸入數字\n";
                                    }
                                }
                            }

                            //貴賓餐券2數量
                            //if (cell6 == "")
                            //    error += "貴賓餐券2數量不可為空白\n";
                            //else
                            if (cell6 != "")
                            {
                                //有數量時，單價不能空白
                                if (cell7.Length == 0)
                                {
                                    error += "貴賓餐券2單價錯誤，不能為空白\n";
                                }
                                if (cell6.Length > 5)
                                {
                                    error += "貴賓餐券2數量長度錯誤\n";
                                }
                                else
                                {
                                    if (!IsNumeric(cell6))
                                    {
                                        error += "貴賓餐券2數量只能輸入數字\n";
                                    }

                                }

                            }

                            //貴賓餐券2單價
                            //if (cell7 == "")
                            //    error += "貴賓餐券2單價不可為空白\n";
                            //else
                            if (cell7 != "")
                            {
                                //有單價時，數量不能空白
                                if (cell6.Length == 0)
                                {
                                    error += "貴賓餐券2數量錯誤，不能為空白\n";
                                }
                                if (cell7.Length > 5)
                                {
                                    error += "貴賓餐券2單價長度錯誤\n";
                                }
                                else
                                {
                                    if (!IsNumeric(cell7))
                                    {
                                        error += "貴賓餐券2單價只能輸入數字\n";
                                    }
                                }
                            }

                            //貴賓餐券3數量
                            //if (cell8 == "")
                            //    error += "貴賓餐券3數量不可為空白\n";
                            //else
                            if (cell8 != "")
                            {
                                //有數量時，單價不能空白
                                if (cell9.Length == 0)
                                {
                                    error += "貴賓餐券3單價錯誤，不能為空白\n";
                                }
                                if (cell8.Length > 5)
                                {
                                    error += "貴賓餐券3數量長度錯誤\n";
                                }
                                else
                                {
                                    if (!IsNumeric(cell8))
                                    {
                                        error += "貴賓餐券3數量只能輸入數字\n";
                                    }

                                }

                            }

                            //貴賓餐券3單價
                            //if (cell9 == "")
                            //    error += "貴賓餐券3單價不可為空白\n";
                            //else
                            if (cell9 != "")
                            {
                                //有單價時，數量不能空白
                                if (cell8.Length == 0)
                                {
                                    error += "貴賓餐券3數量錯誤，不能為空白\n";
                                }
                                if (cell9.Length > 5)
                                {
                                    error += "貴賓餐券3單價長度錯誤\n";
                                }
                                else
                                {
                                    if (!IsNumeric(cell9))
                                    {
                                        error += "貴賓餐券3單價只能輸入數字\n";
                                    }
                                }
                            }

                            //教育餐券數量
                            //if (cell10 == "")
                            //    error += "教育餐券數量不可為空白\n";
                            //else
                            if (cell10 != "")
                            {
                                //有數量時，單價不能空白
                                if (cell11.Length == 0)
                                {
                                    error += "教育餐券單價錯誤，不能為空白\n";
                                }
                                if (cell10.Length > 5)
                                {
                                    error += "教育餐券數量長度錯誤\n";
                                }
                                else
                                {
                                    if (!IsNumeric(cell10))
                                    {
                                        error += "教育餐券數量只能輸入數字\n";
                                    }

                                }

                            }

                            //教育餐券單價
                            //if (cell11 == "")
                            //    error += "教育餐券單價不可為空白\n";
                            //else
                            if (cell11 != "")
                            {
                                //有單價時，數量不能空白
                                if (cell10.Length == 0)
                                {
                                    error += "教育餐券單價錯誤，不能為空白\n";
                                }
                                if (cell11.Length > 5)
                                {
                                    error += "教育餐券單價長度錯誤\n";
                                }
                                else
                                {
                                    if (!IsNumeric(cell11))
                                    {
                                        error += "教育餐券單價只能輸入數字\n";
                                    }
                                }
                            }                           

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
                                dao.COST_DEPT_NO = cell1;
                                dao.L_AMOUNT = cell2 == "" ? "0" : cell2;
                                dao.L_PRICE = cell3 == "" ? "0" : cell3;
                                dao.G1_AMOUNT = cell4 == "" ? "0" : cell4;
                                dao.G1_PRICE = cell5 == "" ? "0" : cell5;
                                dao.G2_AMOUNT = cell6 == "" ? "0" : cell6;
                                dao.G2_PRICE = cell7 == "" ? "0" : cell7;
                                dao.G3_AMOUNT = cell8 == "" ? "0" : cell8;
                                dao.G3_PRICE = cell9 == "" ? "0" : cell9;
                                dao.E1_AMOUNT = cell10 == "" ? "0" : cell10;
                                dao.E1_PRICE = cell11 == "" ? "0" : cell11;

                                dao.G_TOTAL_AMOUNT = Convert.ToString(Convert.ToInt32(dao.G1_AMOUNT) + Convert.ToInt32(dao.G2_AMOUNT) + Convert.ToInt32(dao.G3_AMOUNT));                                                                      
                                                                     
                                dao.G_TOTAL_PRICE = Convert.ToString(Convert.ToInt32(dao.G1_AMOUNT) * Convert.ToInt32(dao.G1_PRICE) +
                                                                      Convert.ToInt32(dao.G2_AMOUNT) * Convert.ToInt32(dao.G2_PRICE) +
                                                                      Convert.ToInt32(dao.G3_AMOUNT) * Convert.ToInt32(dao.G3_PRICE)
                                                                     );
                                
                                dao.CREATED_BY = SessionHandle.Current.emp_id.Trim();
                                dao.UPDATED_BY = SessionHandle.Current.emp_id.Trim();
                                dao.FUNC_ID = "FB2DE050";


                                dao.insert_Detail();
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
                        //ExcelHandle.exportExcel(workbook, "檢核錯誤說明" + type);
                        return workbook;
                    }
                    else
                        Commit();
                    return null;
                }
                catch (Exception ex)
                {
                    RollBack();
                    //return ex.Message;
                    throw;
                }
            }

            //return "0";
            return null;
        }
        catch (Exception ex)
        {
            //return ex.Message;
            throw;
        }

    }

    //產生月報表Excel
    public IWorkbook createExcelDateMonth(CFB2DE0500DAO dao, string type)
    {
        try
        {
            IWorkbook workbook;
            ISheet sheet;
            ISheet sheet1;
            ICellStyle style1;
            ICellStyle style2;
            ICellStyle style3;

           
            string now = DateTime.Now.ToString("yyyy/MM/dd");
                       
            if (type == "xls")
            {
                workbook = new HSSFWorkbook();
                sheet = (HSSFSheet)workbook.CreateSheet("FB2DE050");
                sheet1 = (HSSFSheet)workbook.CreateSheet("負擔部門參考");
                style1 = (HSSFCellStyle)workbook.CreateCellStyle();                
            }
            else
            {
                workbook = new XSSFWorkbook();
                sheet = workbook.CreateSheet("FB2DE050");
                sheet1 = workbook.CreateSheet("負擔部門參考");
                style1 = (XSSFCellStyle)workbook.CreateCellStyle();               
            }
            
            style1.Alignment = HorizontalAlignment.Center;
            style1.VerticalAlignment = VerticalAlignment.Center;

            IFont font1 = workbook.CreateFont();
            font1.FontName = "新細明體";
            font1.FontHeightInPoints = 10;
            style1.SetFont(font1);
            
            IRow row;
            ICell cell;

            //第1列
            row = sheet.CreateRow(0);
            cell = row.CreateCell(1);
            cell.CellStyle = style1;
            cell.SetCellValue("負擔部門");

            cell = row.CreateCell(2);
            cell.CellStyle = style1;
            cell.SetCellValue("來賓餐券數量");

            cell = row.CreateCell(3);
            cell.CellStyle = style1;
            cell.SetCellValue("來賓餐券單價");

            cell = row.CreateCell(4);
            cell.CellStyle = style1;
            cell.SetCellValue("貴賓餐券1數量");

            cell = row.CreateCell(5);
            cell.CellStyle = style1;
            cell.SetCellValue("貴賓餐券1單價");

            cell = row.CreateCell(6);
            cell.CellStyle = style1;
            cell.SetCellValue("貴賓餐券2數量");

            cell = row.CreateCell(7);
            cell.CellStyle = style1;
            cell.SetCellValue("貴賓餐券2單價");

            cell = row.CreateCell(8);
            cell.CellStyle = style1;
            cell.SetCellValue("貴賓餐券3數量");

            cell = row.CreateCell(9);
            cell.CellStyle = style1;
            cell.SetCellValue("貴賓餐券3單價");

            cell = row.CreateCell(10);
            cell.CellStyle = style1;
            cell.SetCellValue("教育餐券數量");

            cell = row.CreateCell(11);
            cell.CellStyle = style1;
            cell.SetCellValue("教育餐券單價");

            sheet.AutoSizeColumn(0);
            sheet.AutoSizeColumn(1);
            sheet.AutoSizeColumn(2);
            sheet.AutoSizeColumn(3);
            sheet.AutoSizeColumn(4);
            sheet.AutoSizeColumn(5);
            sheet.AutoSizeColumn(6);
            sheet.AutoSizeColumn(7);
            sheet.AutoSizeColumn(8);
            sheet.AutoSizeColumn(9);
            sheet.AutoSizeColumn(10);
            sheet.AutoSizeColumn(11);

            //負擔部門參考sheet

            //第1列
            row = sheet1.CreateRow(0);
            cell = row.CreateCell(1);
            cell.CellStyle = style1;
            cell.SetCellValue("負擔部門");


            DataTable tmp = dao.select_COST_DEPT_NO();
            if (tmp.Rows.Count > 0)
            {
                for (int i = 0; i < tmp.Rows.Count; i++)
                {
                    //第2列後
                    row = sheet1.CreateRow(i+1);
                    cell = row.CreateCell(1);
                    cell.CellStyle = style1;
                    cell.SetCellValue(tmp.Rows[i]["COST_DEPT_NO"].ToString());

                }      
           
                //sheet.SetColumnWidth(0, (int)((8 + 0.72) * 256));
                sheet.AutoSizeColumn(1);
                
                //ExcelHandle.exportExcel(workbook, "FB2DE040_MONTHLY." + type);
                
            }
            return workbook;
        }
        catch
        {
            throw;
        }
    }

    public bool IsNumeric(String strNumber)
    {
        Regex NumberPattern = new Regex("[^0-9.-]");
        return !NumberPattern.IsMatch(strNumber);
    }
}