using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;
using NPOI.SS.UserModel;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.Util;

/// <summary>
/// CFB2SJ040BO 的摘要描述
/// </summary>
public class CFB2SJ0400BO : BaseService
{
	public CFB2SJ0400BO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    //修改
    public string updateData(CFB2SJ0400DAO dao)
    {
        string rtnmessage = "";
        try
        {

            //若需要則要進行邏輯檢查

            //檢查OK更新
            if (rtnmessage == "")
            {
                try
                {
                    BeginTransaction();

                    dao.updateData();

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

    //刪除
    public string deleteData(List<Tuple<string, string, string>> keysList)
    {
        CFB2SJ0400DAO sj040DAO = new CFB2SJ0400DAO();
        string rtnmessage = "";//存在檢查後的訊息
        try
        {

            //檢查完成後，逐筆進行刪除
            if (rtnmessage == "")
            {
                try
                {
                    BeginTransaction();
                    foreach (var item in keysList)
                    {
                        sj040DAO.ASSESS_YEAR = item.Item1;
                        sj040DAO.ASSESS_TYPE = item.Item2;
                        sj040DAO.EMP_ID = item.Item3;
                        sj040DAO.deleteData();
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
            {
                return rtnmessage;
            }

        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }



    //EXCEL上傳
    public IWorkbook uploadExcel(Stream fs, string type)
    {

        try
        {
            CFB2SJ0400DAO sj040DAO = new CFB2SJ0400DAO();

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
            style1.SetFont(font1);
            if (sheet != null)
            {
                //2.取得excel的資料
                string cell_assess_year = "";
                string cell_assess_type = "";
                string cell_emp_id = "";
                string cell_remark = "";

                string assess_year_first = "";
                string assess_type_first = "";

                //錯誤訊息
                string error = "";
                List<string> empid_List = new List<string>();
                ICell cell;
                //巡覽每row的資料第一列為title跳過
                for (int i = 1; i <= sheet.LastRowNum; i++)
                {
                    //清空
                    error = "";

                    if (sheet.GetRow(i) != null)
                    {
                        //讀取cell資料，第一欄為檢核結果欄位跳過
                        cell_assess_year = sheet.GetRow(i).GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                        cell_assess_type = sheet.GetRow(i).GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                        cell_emp_id = sheet.GetRow(i).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                        cell_remark = sheet.GetRow(i).GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();

                        //比較第一筆的考核年度及類別
                        if (i == 1)
                        {
                            assess_year_first = cell_assess_year;
                            assess_type_first = cell_assess_type;
                        }
                        else {
                            if (cell_assess_year != assess_year_first) {
                                error += "考核年度不一致,\n";
                            }
                            if (cell_assess_type != assess_type_first)
                            {
                                error += "考核類別不一致,\n";
                            }
                        }
                       
                        if (cell_assess_year == "")
                        {
                            error += "考核年度不可空白,\n";
                        }
                        else
                        {
                            error += this.checkNumber(cell_assess_year, "考核年度", 4, "");
                        }

                        if (cell_assess_type == "")
                        {
                            error += "考核類別不可空白,\n";
                        }
                        else {
                            if (sj040DAO.checkAsessType(cell_assess_type) == 0) {
                                error += "考核類別不存在,\n";
                            }
                        }


                        if (cell_emp_id == "")
                        {
                            error += "工號不可空白,\n";
                        }
                        else {
                            if (sj040DAO.checkEMPID(assess_year_first, assess_type_first, cell_emp_id) == 0)
                            {
                                error += "工號不存在考核對象內,\n";
                            }
                            //EXCEL 工號重覆
                            if (empid_List.Contains(cell_emp_id))
                            {
                                error += "工號重覆,\n";
                            }
                            else
                            {
                                empid_List.Add(cell_emp_id);
                            }

                        }
                        if (cell_remark == "")
                        {
                            error += "備註不可空白,\n";
                        }
                        cell = sheet.GetRow(i).GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                        cell.CellStyle = style1;
                        cell.SetCellValue(error);
                        if (error.Trim() != "")
                        {
                            valid = false;
                        }
                    }
                }

                if (valid)
                {
                    BeginTransaction();
                    sj040DAO = new CFB2SJ0400DAO();
                    sj040DAO.ASSESS_YEAR = assess_year_first;
                    sj040DAO.ASSESS_TYPE = assess_type_first;
                    sj040DAO.deleteAllData();
                    DateTime now = DateTime.Parse(DateTime.Now.ToString());
                    for (int i = 1; i <= sheet.LastRowNum; i++)
                    {
                        if (sheet.GetRow(i) != null)
                        {
                            //讀取cell資料，第一欄為檢核結果欄位跳過
                            sj040DAO.ASSESS_YEAR = sheet.GetRow(i).GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            sj040DAO.ASSESS_TYPE = sheet.GetRow(i).GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            sj040DAO.EMP_ID = sheet.GetRow(i).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            sj040DAO.REMARK = sheet.GetRow(i).GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();

                            sj040DAO.CREATED_BY = SessionHandle.Current.emp_id;
                            sj040DAO.UPDATED_BY = SessionHandle.Current.emp_id;
                            sj040DAO.FUNC_ID = "FB2SJ040";
                            sj040DAO.insertData(now);
                        }
                    }
                    Commit();
                    return null;
                }
                else
                {
                    return workbook;
                }
            }
            else
            {
                return workbook;
            }

        }
        catch (Exception ex)
        {
            RollBack();
            throw;

        }

    }


    //檢查是否為數字(正整數)
    public string checkNumber(string cellData, string cellName, int cellLength, string error)
    {
        try
        {
            int numCheckResult = 0;
            cellData = cellData.Replace(",", "");
            if (cellData == "")
                error += cellName + "不可空白\n";
            else
            {
                if (cellData.Trim().Length > cellLength || !int.TryParse(cellData.Trim(), out numCheckResult))
                {
                    error += cellName + "必須為數字, 且長度必須為" + cellLength + ", \n";
                }
            }
            return error;
        }
        catch (Exception)
        {
            throw;
        }



    }



}