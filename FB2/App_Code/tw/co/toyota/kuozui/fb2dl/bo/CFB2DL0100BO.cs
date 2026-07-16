using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;

/// <summary>
/// CFB2DL0100BO 的摘要描述
/// </summary>
public class CFB2DL0100BO : BaseService
{
    public CFB2DL0100BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public string SYS_CD { get; set; }
    public string MAIN_CD { get; set; }
    public string MAIN_DESC { get; set; }
    public string USER_UPD { get; set; }
    # region "Qry"

    //得一齊轉預借的生成子假別
    public DataTable getSub_Leave_CD()
    {
        try
        {
            CFB2DL0100DAO dl010DAO = new CFB2DL0100DAO();
            return dl010DAO.getSub_Leave_CD();
        }
        catch
        {
            throw;
        }
    }

    public string deleteData(List<string> deleteList)
    {
        try
        {
            CFB2DL0100DAO dao = new CFB2DL0100DAO();
            BeginTransaction();

            foreach (string deleteitem in deleteList)
            {
                //刪除主檔資料
                dao.deleteData(deleteitem);
            }
            Commit();
            return "0";
        }
        catch (Exception)
        {
            RollBack();
            throw;
        }
    }


    #endregion

    #region "Mod & Add"
    //儲存
    public string saveData(CFB2DL0100DAO dao, string mod, string emp_id, string main_leave_cd, string sub_leave_cd, string sub_leave_desc, string base_year, string start_dt)
    {
        try
        {
            //修改模式
            if (mod == "mod")
            {
                BeginTransaction();
                dao.updateData(emp_id, main_leave_cd, sub_leave_cd, start_dt);
                Commit();
                return "0";
            }
            else //新增模式
            {
                //DataTable dtBASE_YEAR = dao.getBASE_YEAR_Repeat(emp_id, main_leave_cd, sub_leave_cd, base_year);
                DataTable dt = null;   
                //結算方式=Y,1個年度只能有1個
                if (dao.SALARY_SETTLE_CD == "Y")
                {
                    dt = dao.getBASE_YEAR_Repeat();
                    if (Convert.ToInt16(dt.Rows[0]["total"]) > 0)
                    {
                        return "該員工該年度" + sub_leave_desc + "結算方式(Y-年結)已經存在，請以修改模式修改資料";
                    }
                }
                //取得現有資料
                dt = dao.getExistData(emp_id, main_leave_cd, sub_leave_cd, start_dt);
                if (Convert.ToInt16(dt.Rows[0]["total"]) > 0)
                {
                    return "資料重覆";
                }
                else
                {
                    BeginTransaction();
                    if (sub_leave_cd == "M0")
                    {
                        dao.addHonor(emp_id);
                    }
                    dao.addData();
                    Commit();
                    return "0";
                }
            }
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    #endregion

    #region "other"
    //特休生成
    /*
		SET @proc_cnt = (SELECT COUNT(*)
						FROM TB_D_M_EMP_AVAILABLE_LEAVE
						WHERE MAIN_LEAVE_CD = 'D'
						AND PROC_OTH_DESC = 'D0'
						AND BASE_YEAR = @Year);
		
		IF @proc_cnt > 0 BEGIN
			SELECT '生成年度的特休假已生成，是否覆蓋舊資料？';
			GOTO WriteLog;
		END
		SET @proc_cnt = (SELECT COUNT(*)
						FROM TB_D_M_EMP_AVAILABLE_LEAVE
						WHERE MAIN_LEAVE_CD = 'D'
						AND PROC_OTH_DESC = 'D0'
						AND BASE_YEAR = @Year
						AND SALARY_SETTLE_STATUS <> 'N');
		
		IF @proc_cnt > 0 BEGIN
			SELECT '生成年度的特休假已計薪，無法重新生成！';
			GOTO WriteLog;
		END
    */
    public string beforeExecutePayLeaveGen(string Year)
    {
        try
        {
            string msg = "";
            CFB2DL0100DAO dao = new CFB2DL0100DAO();
            if (dao.checkStatusIsN(Year))
            {
                if (dao.checkGenerateIsExsit(Year))
                    msg = "confirm";  //生成年度的特休假已生成，是否覆蓋舊資料？
                else
                    msg = "0";
            }
            else
            {
                msg = "alert";      //生成年度的特休假已計薪，無法重新生成！
            }
            return msg;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public string executePayLeaveGen(string Year)
    {
        try
        {
            string msg = "";
            CFB2DL0100DAO dao = new CFB2DL0100DAO();
            dao.RunProcSP_H_EMP_PAY_LEAVE(Year);
            DataTable dtSPresult = dao.checkSP("SP_H_EMP_PAY_LEAVE");
            if (dtSPresult.Rows.Count > 0)
            {
                //PROC_STATUS：Y = 成功,N = 失敗,E = Exception  ,PROC_LOG：處理結果中文訊息
                if (Convert.ToString(dtSPresult.Rows[0]["PROC_STATUS"]) == "Y")
                    msg = "0";
                else
                    msg = Convert.ToString(dtSPresult.Rows[0]["PROC_LOG"]);
            }
            return msg;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    //一齊轉預借
    public string executeFollowLeave(string Year, string sub_leave_cd)
    {
        try
        {
            string msg = "";
            CFB2DL0100DAO dao = new CFB2DL0100DAO();
            dao.RunProcSP_H_EMP_POLICY_PAY_LEAVE(Year, sub_leave_cd);
            DataTable dtSPresult = dao.checkSP("SP_H_EMP_POLICY_PAY_LEAVE");
            if (dtSPresult.Rows.Count > 0)
            {
                //PROC_STATUS：Y = 成功,N = 失敗,E = Exception  ,PROC_LOG：處理結果中文訊息
                if (Convert.ToString(dtSPresult.Rows[0]["PROC_STATUS"]) == "Y")
                    msg = "0";
                else
                    msg = Convert.ToString(dtSPresult.Rows[0]["PROC_LOG"]);
            }
            return msg;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    #endregion


    private void createSingleRow(ISheet sheet, ICellStyle style1, DataTable dtExcelData, int i)
    {
        IRow row = null;
        ICell cell = null;
        if (i == 0)
        {
            for (int excelindex = 0; excelindex <= 5; excelindex++)
            {
                row = sheet.CreateRow(excelindex);
                row.Height = 375;
                if (excelindex == 5)
                {
                    row.Height = 420;
                }
            }
            createSingleRowFisrt(sheet, style1, dtExcelData, i, 0);
            createSingleRowSecond(sheet, style1, dtExcelData, i, 1);
            createSingleRowThird(sheet, style1, dtExcelData, i, 2);
            createSingleRowFourth(sheet, style1, dtExcelData, i, 3);
            createSingleRowFifth(sheet, style1, dtExcelData, i, 4);
        }
        else
        {
            for (int excelindex = (i * 6) + 0; excelindex <= (i * 6) + 5; excelindex++)
            {
                row = sheet.CreateRow(excelindex);
                row.Height = 375;
                if (excelindex == (i * 6) + 5)
                {
                    row.Height = 420;
                }
            }
            createSingleRowFisrt(sheet, style1, dtExcelData, i, (i * 6));
            createSingleRowSecond(sheet, style1, dtExcelData, i, (i * 6) + 1);
            createSingleRowThird(sheet, style1, dtExcelData, i, (i * 6) + 2);
            createSingleRowFourth(sheet, style1, dtExcelData, i, (i * 6) + 3);
            createSingleRowFifth(sheet, style1, dtExcelData, i, (i * 6) + 4);
        }
    }

    private void createSingleRowFisrt(ISheet sheet, ICellStyle style1, DataTable dtExcelData, int i, int excelindex)
    {
        //int excelindex = i + 2;

        //每筆第一行
        //IRow row = sheet.CreateRow(excelindex);
        //row.Height = 375;//(315=21)
        IRow row = sheet.GetRow(excelindex);
        ICell cell;
        cell = row.CreateCell(0);
        cell.CellStyle = style1;
        cell.SetCellValue("年度:" + (Convert.ToInt16(dtExcelData.Rows[i]["BASE_YEAR"]) - 1911).ToString());//核假年度

        cell = row.CreateCell(1);
        cell.CellStyle = style1;
        cell.SetCellValue(dtExcelData.Rows[i]["PLANT_NAME"].ToString());//工廠名稱 

        cell = row.CreateCell(2);
        cell.CellStyle = style1;
        cell.SetCellValue(dtExcelData.Rows[i]["EMP_ID"].ToString() + "  " + dtExcelData.Rows[i]["EMP_NAME"].ToString().Trim());//工號  //姓名  

        cell = row.CreateCell(3);
        cell.CellStyle = style1;
        cell.SetCellValue("");

        cell = row.CreateCell(4);
        cell.CellStyle = style1;
        cell.SetCellValue("");

        //區塊二
        if (i <= dtExcelData.Rows.Count - 1)
        {
            cell = row.CreateCell(6);
            cell.CellStyle = style1;
            cell.SetCellValue("年度:" + (Convert.ToInt16(dtExcelData.Rows[i]["BASE_YEAR"]) - 1911).ToString());//核假年度

            cell = row.CreateCell(7);
            cell.CellStyle = style1;
            cell.SetCellValue(dtExcelData.Rows[i]["PLANT_NAME"].ToString());//工廠名稱

            cell = row.CreateCell(8);
            cell.CellStyle = style1;
            cell.SetCellValue(dtExcelData.Rows[i]["EMP_ID"].ToString() + "  " + dtExcelData.Rows[i]["EMP_NAME"].ToString().Trim());//工號  //姓名

            cell = row.CreateCell(9);
            cell.CellStyle = style1;
            cell.SetCellValue("");

            cell = row.CreateCell(10);
            cell.CellStyle = style1;
            cell.SetCellValue("");
        }
    }
    private void createSingleRowSecond(ISheet sheet, ICellStyle style1, DataTable dtExcelData, int i, int excelindex)
    {
        //每筆第二行
        //IRow row = sheet.CreateRow(excelindex);
        //row.Height = 375;//(315=21)

        string level_cd1 = dtExcelData.Rows[i]["LEVEL_CD"].ToString(); //資格代號
        if (dtExcelData.Rows[i]["GRADE_CD"] != null && dtExcelData.Rows[i]["GRADE_CD"] != DBNull.Value && dtExcelData.Rows[i]["GRADE_CD"].ToString().Trim() != "")
        {
            level_cd1 += "-" + dtExcelData.Rows[i]["GRADE_CD"].ToString(); //級數代號
        }

        IRow row = sheet.GetRow(excelindex);
        ICell cell;
        cell = row.CreateCell(0);
        cell.CellStyle = style1;
        cell.SetCellValue("資格:" + level_cd1);

        cell = row.CreateCell(1);
        cell.CellStyle = style1;
        cell.SetCellValue("");

        cell = row.CreateCell(2);
        cell.CellStyle = style1;
        cell.SetCellValue(dtExcelData.Rows[i]["DEPT_NO"].ToString()); //部門代號

        //區塊二
        if (i <= dtExcelData.Rows.Count - 1)
        {
            level_cd1 = dtExcelData.Rows[i]["LEVEL_CD"].ToString(); //資格代號
            if (dtExcelData.Rows[i]["GRADE_CD"] != null && dtExcelData.Rows[i]["GRADE_CD"] != DBNull.Value && dtExcelData.Rows[i]["GRADE_CD"].ToString().Trim() != "")
            {
                level_cd1 += "-" + dtExcelData.Rows[i]["GRADE_CD"].ToString(); //級數代號
            }
            cell = row.CreateCell(6);
            cell.CellStyle = style1;
            cell.SetCellValue("資格:" + level_cd1);

            cell = row.CreateCell(7);
            cell.CellStyle = style1;
            cell.SetCellValue("");

            cell = row.CreateCell(8);
            cell.CellStyle = style1;
            cell.SetCellValue(dtExcelData.Rows[i]["DEPT_NO"].ToString()); //部門代號
        }
    }
    private void createSingleRowThird(ISheet sheet, ICellStyle style1, DataTable dtExcelData, int i, int excelindex)
    {
        //每筆第三行
        //IRow row = sheet.CreateRow(excelindex);
        //row.Height = 375;//(315=21)
        int year1 = 0;
        string month = "";
        string day = "";
        string month_date1 = "";
        if (dtExcelData.Rows[i]["JOIN_DT"] != DBNull.Value)   //入社日
        {
            DateTime join_date1 = Convert.ToDateTime(dtExcelData.Rows[i]["JOIN_DT"]);
            year1 = join_date1.Year - 1911;
            month = join_date1.Month < 10 ? "0" + join_date1.Month.ToString() : join_date1.Month.ToString();
            day = join_date1.Day < 10 ? "0" + join_date1.Day.ToString() : join_date1.Day.ToString();
            month_date1 = month + "/" + day;
        }

        IRow row = sheet.GetRow(excelindex);
        ICell cell;
        cell = row.CreateCell(0);
        cell.CellStyle = style1;
        cell.SetCellValue("到職日:" + year1 + "/" + month_date1);

        cell = row.CreateCell(1);
        cell.CellStyle = style1;
        cell.SetCellValue("");

        cell = row.CreateCell(2);
        cell.CellStyle = style1;
        cell.SetCellValue(dtExcelData.Rows[i]["DEPT_NAME_20"].ToString()); //部級部門名稱

        cell = row.CreateCell(3);
        cell.CellStyle = style1;
        cell.SetCellValue(""); //部級部門名稱
        cell = row.CreateCell(4);
        cell.CellStyle = style1;
        //sheet.AddMergedRegion(new CellRangeAddress(excelindex, excelindex, 3, 4)); //合併儲存格

        //區塊二
        if (i <= dtExcelData.Rows.Count - 1)
        {
            cell = row.CreateCell(6);
            cell.CellStyle = style1;
            cell.SetCellValue("到職日:" + year1 + "/" + month_date1);

            cell = row.CreateCell(7);
            cell.CellStyle = style1;
            cell.SetCellValue("");

            cell = row.CreateCell(8);
            cell.CellStyle = style1;
            cell.SetCellValue(dtExcelData.Rows[i]["DEPT_NAME_20"].ToString()); //部級部門名稱

            cell = row.CreateCell(9);
            cell.CellStyle = style1;
            cell.SetCellValue("");
            cell = row.CreateCell(10);
            cell.CellStyle = style1;
            //sheet.AddMergedRegion(new CellRangeAddress(excelindex, excelindex, 9, 10));  //合併儲存格
        }
    }
    private void createSingleRowFourth(ISheet sheet, ICellStyle style1, DataTable dtExcelData, int i, int excelindex)
    {
        //每筆第四行
        //IRow row = sheet.CreateRow(excelindex);
        //row.Height = 375;//(315=21)
        IRow row = sheet.GetRow(excelindex);
        //row.Height = 315;
        ICell cell;
        cell = row.CreateCell(2);
        cell.CellStyle = style1;
        if (dtExcelData.Rows[i]["DEPT_NAME_30"] != null && dtExcelData.Rows[i]["DEPT_NAME_30"] != DBNull.Value)
        {
            cell.SetCellValue(dtExcelData.Rows[i]["DEPT_NAME_30"].ToString()); //室級部門名稱
        }
        else
        {
            cell.SetCellValue(dtExcelData.Rows[i]["DEPT_NAME_40"].ToString()); //課級部門名稱
        }
        cell = row.CreateCell(3);
        cell.CellStyle = style1;
        //sheet.AddMergedRegion(new CellRangeAddress(excelindex, excelindex, 3, 4));
        //區塊二
        if (i <= dtExcelData.Rows.Count - 1)
        {
            cell = row.CreateCell(8);
            cell.CellStyle = style1;
            if (dtExcelData.Rows[i]["DEPT_NAME_30"] != null && dtExcelData.Rows[i]["DEPT_NAME_30"] != DBNull.Value)
            {
                cell.SetCellValue(dtExcelData.Rows[i]["DEPT_NAME_30"].ToString()); //室級部門名稱
            }
            else
            {
                cell.SetCellValue(dtExcelData.Rows[i]["DEPT_NAME_40"].ToString()); //課級部門名稱
            }
            cell = row.CreateCell(9);
            cell.CellStyle = style1;
            //sheet.AddMergedRegion(new CellRangeAddress(excelindex, excelindex, 9, 10));
        }
    }
    private void createSingleRowFifth(ISheet sheet, ICellStyle style1, DataTable dtExcelData, int i, int excelindex)
    {
        //每筆第五行
        //IRow row = sheet.CreateRow(excelindex);
        //row.Height = 375;//(315=21)
        IRow row = sheet.GetRow(excelindex);
        //row.Height = 315;
        ICell cell;
        string special1 = "";//核給特休天數
        string honor1 = ""; //榮譽天數
        if (dtExcelData.Rows[i]["SPECIAL"] != null && dtExcelData.Rows[i]["SPECIAL"] != DBNull.Value)
        {
            special1 = Convert.ToInt32(dtExcelData.Rows[i]["SPECIAL"]).ToString();
            cell = row.CreateCell(0);
            cell.CellStyle = style1;
            cell.SetCellValue("核給特休:" + special1 + "天");

            cell = row.CreateCell(1);
            cell.CellStyle = style1;
            cell.SetCellValue("");
        }

        if (dtExcelData.Rows[i]["HONOR"] != null && dtExcelData.Rows[i]["HONOR"] != DBNull.Value)
        {
            honor1 = Convert.ToInt32(dtExcelData.Rows[i]["HONOR"]).ToString();
            cell = row.CreateCell(2);
            cell.CellStyle = style1;
            cell.SetCellValue("榮譽:" + honor1 + "天");

            cell = row.CreateCell(3);
            cell.CellStyle = style1;
            cell.SetCellValue("");
        }
        //區塊二
        if (i <= dtExcelData.Rows.Count - 1)
        {
            if (dtExcelData.Rows[i]["SPECIAL"] != null && dtExcelData.Rows[i]["SPECIAL"] != DBNull.Value)
            {
                cell = row.CreateCell(6);
                cell.CellStyle = style1;
                cell.SetCellValue("核給特休:" + special1 + "天");

                cell = row.CreateCell(7);
                cell.CellStyle = style1;
                cell.SetCellValue("");
            }

            if (dtExcelData.Rows[i]["HONOR"] != null && dtExcelData.Rows[i]["HONOR"] != DBNull.Value)
            {
                cell = row.CreateCell(8);
                cell.CellStyle = style1;
                cell.SetCellValue("榮譽:" + honor1 + "天");
                cell = row.CreateCell(9);
                cell.CellStyle = style1;
                cell.SetCellValue(""); //榮譽天數
            }
        }
        //row = sheet.CreateRow(excelindex + 1); //第六行
        //row.Height = 390; // 19.5 * 20;
    }

    //下載Excel資料
    public IWorkbook createExcelFromTemplate(string base_year, string dept_no, string emp_cd, string emp_id, DataTable dtExcelData)
    {
        try
        {
            //Excel初始化
            IWorkbook workbook;
            ISheet sheet;
            ICellStyle style1;
            workbook = new XSSFWorkbook();


            //產生Excel
            sheet = workbook.CreateSheet("特休假標籤列印");
            //FileStream fs = new FileStream(excelPath, FileMode.Open, FileAccess.ReadWrite);
            //workbook = new XSSFWorkbook(fs); //xlsx的方法
            //sheet = workbook.GetSheetAt(0);

            CFB2DL0100DAO dao = new CFB2DL0100DAO();
            //欄位名稱
            style1 = (XSSFCellStyle)workbook.CreateCellStyle();
            IFont font1 = workbook.CreateFont();
            font1.FontName = "Arial Unicode MS";
            font1.FontHeightInPoints = 11;
            style1.SetFont(font1);
            if (dtExcelData.Rows.Count > 0)
            {
                for (int i = 0; i < dtExcelData.Rows.Count; i++)
                {
                    createSingleRow(sheet, style1, dtExcelData, i);
                }
                //for (int j = 0; j <= 10; j++)
                //{
                //sheet.AutoSizeColumn(j);
                //}
                sheet.SetColumnWidth(0, 10 * 256);
                sheet.SetColumnWidth(1, 9 * 256);
                sheet.SetColumnWidth(2, 10 * 256);
                sheet.SetColumnWidth(3, 6 * 256);
                sheet.SetColumnWidth(4, 8 * 256);
                sheet.SetColumnWidth(5, 3 * 256);
                sheet.SetColumnWidth(6, 10 * 256);
                sheet.SetColumnWidth(7, 9 * 256);
                sheet.SetColumnWidth(8, 10 * 256);
                sheet.SetColumnWidth(9, 6 * 256);
                sheet.SetColumnWidth(10, 8 * 256);

                //ExcelHandle.exportExcel(workbook, "特休假標籤列印.xlsx");
            }
            return workbook;
        }
        catch (Exception ex)
        {
            throw;
        }

    }


}