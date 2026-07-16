using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;

/// <summary>
/// CFB2SL1100BO 的摘要描述
/// </summary>
public class CFB2SL1100BO : BaseService
{
    CFB2SL1100DAO dao = new CFB2SL1100DAO();
    public CFB2SL1100BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public void deleteData(string company_cd, string data_ym, string data_format)
    {
        try
        {
            BeginTransaction();
            dao.deleteData(company_cd, data_ym, data_format);
            Commit();
        }
        catch (Exception)
        {
            RollBack();
            throw;
        }
    }
    //下載Excel資料
    public void createExcelFromTemplate(string type, string excelPath, string data_format)
    {
        try
        {
            IWorkbook workbook;
            FileStream fs = new FileStream(excelPath, FileMode.Open, FileAccess.Read);
            //依type判斷要用哪種方式產生
            if (type == "xls")
                workbook = new HSSFWorkbook(fs);
            else
                workbook = new XSSFWorkbook(fs);

            if (data_format == "A" || data_format == "D")
                ExcelHandle.exportExcel(workbook, "WFB2SL110_Import_ExampleA&D." + type);
            else
                ExcelHandle.exportExcel(workbook, "WFB2SL110_Import_ExampleV." + type);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public IWorkbook importExcel(Stream fs, string type, string company_cd, string data_ym, string data_format)
    {
        try
        {
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

            string msg = string.Empty;
            if (sheet != null)
            {
                if (data_format == "A" || data_format == "D")
                    workbook = checkImportData_typeIsAorD(workbook, sheet,style1, company_cd, data_ym, data_format);
                else if (data_format == "V")
                {
                    workbook = checkImportData_typeIsV(workbook, sheet, style1,company_cd, data_ym, data_format);
                }
            }
            return workbook;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public IWorkbook checkImportData_typeIsAorD(IWorkbook workbook, ISheet sheet,ICellStyle style1, string company_cd, string data_ym, string data_format)
    {
        try
        {
            string[] cell1 = new string[sheet.LastRowNum + 1];
            string[] cell2 = new string[sheet.LastRowNum + 1];
            string[] cell3 = new string[sheet.LastRowNum + 1];
            string[] cell4 = new string[sheet.LastRowNum + 1];
            string[] cell5 = new string[sheet.LastRowNum + 1];
            string[] emp_name = new string[sheet.LastRowNum + 1];
            string[] license_id = new string[sheet.LastRowNum + 1];
            string[] contact_zip_cd = new string[sheet.LastRowNum + 1];
            string[] contact_addr = new string[sheet.LastRowNum + 1];            
            string error = "";
            bool pass = true;

            BeginTransaction();

            string checkImport = sheet.GetRow(0).GetCell(6, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString();
            if (checkImport.Trim() != "")
            {
                error += "挑選之檔案不符員工所得格式";
            }
            else
            {
                //巡覽每row的資料第一列為title跳過
                for (int i = 1; i <= sheet.LastRowNum; i++)
                {
                    if (sheet.GetRow(i) != null)
                    {
                        error = "";
                        string line = Convert.ToString(i + 1);
                        //讀取cell資料，第一欄為檢核結果欄位跳過
                        cell1[i] = sheet.GetRow(i).GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().ToUpper();
                        cell2[i] = sheet.GetRow(i).GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                        cell3[i] = sheet.GetRow(i).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                        cell4[i] = sheet.GetRow(i).GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                        cell5[i] = sheet.GetRow(i).GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().ToUpper();
                        decimal decCheckResult = 0;
                        //檢查資料格式
                        if (data_format == "A")
                        {
                            if (cell1[i] != "A")
                                error += "第" + line + "列 欄位「資料格式」必須為A,\n";
                        }
                        if (data_format == "D")
                        {
                            if (cell1[i] != "D")
                                error += "第" + line + "列 欄位「資料格式」必須為D,\n";
                        }

                        //檢查工號是否在，工號存在取得員工資料
                        DataTable dtEMP_data = dao.getEMP_Data(cell2[i]);
                        if (dtEMP_data.Rows.Count == 0)
                        {
                            error += "第" + line + "列 欄位「工號」" + cell2[i] + "不存在,\n";
                        }
                        else
                        {
                            emp_name[i] = Convert.ToString(dtEMP_data.Rows[0]["EMP_NAME"]).Trim();
                            license_id[i] = Convert.ToString(dtEMP_data.Rows[0]["LICENSE_ID"]).Trim();
                            contact_zip_cd[i] = Convert.ToString(dtEMP_data.Rows[0]["CONTACT_ZIP_CD"]).Trim();
                            contact_addr[i] = Convert.ToString(dtEMP_data.Rows[0]["CONTACT_ADDR"]).Trim();
                        }

                        //檢查數字格式
                        if (!decimal.TryParse(cell3[i], out decCheckResult))
                            error += "第" + line + "列 欄位「總額」格式不符,\n";

                        if (!decimal.TryParse(cell4[i], out decCheckResult))
                            error += "第" + line + "列 欄位「稅額」格式不符,\n";

                        //檢查長度
                        if (cell3[i].Length > 10)
                            error += "第" + line + "列 欄位「總額」長度錯誤,\n";
                        if (cell4[i].Length > 10)
                            error += "第" + line + "列 欄位「稅額」長度錯誤,\n";
                        if (cell5[i].Length > 2)
                            error += "第" + line + "列 欄位「所得格式」長度錯誤,\n";

                        if (error.Trim().Length > 0)
                        {
                            sheet.GetRow(i).CreateCell(0).CellStyle = style1;
                            sheet.GetRow(i).GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(error);
                            pass = false;
                        }
                    }
                }
            }
            if (pass)
            {
                
                for (int j = 1; j <= sheet.LastRowNum; j++)
                {
                    dao.addImportData_type_IsAorD(company_cd, data_format, data_ym, cell1[j], cell2[j], cell3[j], cell4[j], cell5[j]
                                                  , emp_name[j], license_id[j], contact_zip_cd[j], contact_addr[j]);
                }
                Commit();
                return null;
            }
            else
            {
                RollBack();
                return workbook;
            }
        }
        catch
        {
            RollBack();
            throw;
        }
    }
    public IWorkbook checkImportData_typeIsV(IWorkbook workbook, ISheet sheet,ICellStyle style1, string company_cd, string data_ym, string data_format)
    {
        try
        {
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
            string error = "";
            bool pass = true;
            string checkImport = sheet.GetRow(0).GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString();
            if (checkImport.Trim() == "")
            {
                error += "挑選之檔案不符非員工所得格式";
            }
            else
            {
                for (int i = 1; i <= sheet.LastRowNum; i++)
                {
                    if (sheet.GetRow(i) != null)
                    {
                        error = "";
                        string line = Convert.ToString(i + 1);
                        //讀取cell資料，第一欄為檢核結果欄位跳過
                        cell1[i] = sheet.GetRow(i).GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().ToUpper();
                        cell2[i] = sheet.GetRow(i).GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                        cell3[i] = sheet.GetRow(i).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                        cell4[i] = sheet.GetRow(i).GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                        cell5[i] = sheet.GetRow(i).GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                        cell6[i] = sheet.GetRow(i).GetCell(6, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                        cell7[i] = sheet.GetRow(i).GetCell(7, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                        cell8[i] = sheet.GetRow(i).GetCell(8, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                        cell9[i] = sheet.GetRow(i).GetCell(9, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                        cell10[i] = sheet.GetRow(i).GetCell(10, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                        decimal decCheckResult = 0;
                        BeginTransaction();

                        //檢查資料格式
                        if (cell1[i] != "V")
                            error += "第" + line + "列 欄位「資料格式」必須為V,\n";

                        //檢查非所得項目代碼
                        if (dao.checkTAX_FORMATIsExist(cell7[i]))
                            error += "第" + line + "列 找不到所得項目" + cell7[i] + ",\n";
                        //檢查數字格式
                        if (!decimal.TryParse(cell9[i], out decCheckResult))
                            error += "第" + line + "列 欄位「總額」格式不符,\n";

                        if (!decimal.TryParse(cell10[i], out decCheckResult))
                            error += "第" + line + "列 欄位「稅額」格式不符,\n";

                        //檢查長度
                        if (cell2[i].Length > 5)
                            error += "第" + line + "列 欄位「工號/廠商代號」長度錯誤,\n";
                        if (cell3[i].Length > 10)
                            error += "第" + line + "列 欄位「身份證統一編號」長度錯誤,\n";
                        if (cell4[i].Length > 30)
                            error += "第" + line + "列 欄位「姓名」長度錯誤,\n";
                        if (cell5[i].Length > 5)
                            error += "第" + line + "列 欄位「郵遞區號」長度錯誤,\n";
                        if (cell6[i].Length > 150)
                            error += "第" + line + "列 欄位「住址」長度錯誤,\n";
                        if (cell7[i].Length > 3)
                            error += "第" + line + "列 欄位「所得代號1」長度錯誤,\n";
                        if (cell8[i].Length > 2)
                            error += "第" + line + "列 欄位「所得代號2」長度錯誤,\n";
                        if (cell9[i].Length > 10)
                            error += "第" + line + "列 欄位「總額」長度錯誤,\n";
                        if (cell10[i].Length > 10)
                            error += "第" + line + "列 欄位「稅額」長度錯誤,\n";

                        if (error.Trim().Length > 0)
                        {
                            sheet.GetRow(i).CreateCell(0).CellStyle = style1;
                            sheet.GetRow(i).GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(error);
                            pass = false;
                        }
                    }
                }
            }
            if (pass)
            {
                
                for (int j = 1; j <= sheet.LastRowNum; j++)
                {
                    dao.addImportData_type_IsV(company_cd, data_format, data_ym, cell1[j], cell2[j], cell3[j], cell4[j], cell5[j], cell6[j], cell7[j], cell8[j], cell9[j], cell10[j]);
                    error = "0";
                }
                Commit();
                return null;
            }
            else
            {
                RollBack();
                return workbook;
            }
        }
        catch
        {
            RollBack();
            throw;
        }
    }
}