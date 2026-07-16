using System;
using System.Collections.Generic;
using System.Data;
using FB2.tw.co.toyota.kuozui.bo;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;

/// <summary>
/// CFB2SC5B00BO 的摘要描述
/// </summary>
public class CFB2SC5B00BO : BaseService
{
    public CFB2SC5B00BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public string deleteData()
    {
        CFB2SC5B00DAO fb2sc = new CFB2SC5B00DAO();
        string rtnmessage = "";
        //檢查OK逐筆刪除
        if (rtnmessage == "")
        {
            try
            {
                BeginTransaction();
                //foreach (var item in salary_data)
                //{
                fb2sc.deleteData();
                //}
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
    public string ins3DayTmp()
    {
        CFB2SC5B00DAO fb2sc = new CFB2SC5B00DAO();
        string rtnmessage = "";
        //檢查OK逐筆刪除
        if (rtnmessage == "")
        {
            try
            {
                BeginTransaction();
                fb2sc.ins3DayTmpData();
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


    public IWorkbook CreatExcel(string salary_type, string salary_ym, string salary_dt, List<Tuple<string, string>> salary_data, string type)
    {
        CFB2SC5B00DAO fb2sc = new CFB2SC5B00DAO();
        IWorkbook workbook = null;
        //取得範本sheet
        ISheet sheet = null;
        try
        {
            int remarkrow = 0;
            ICellStyle style1, style2, style3, style4, style5, styleMerge;
            if (type == "xls")
            {
                workbook = new HSSFWorkbook();
                sheet = (HSSFSheet)workbook.CreateSheet("FB2SC5B0");
                style1 = (HSSFCellStyle)workbook.CreateCellStyle();
                style2 = (HSSFCellStyle)workbook.CreateCellStyle();
                style3 = (HSSFCellStyle)workbook.CreateCellStyle();
                style4 = (HSSFCellStyle)workbook.CreateCellStyle();
                style5 = (HSSFCellStyle)workbook.CreateCellStyle();
                styleMerge = (HSSFCellStyle)workbook.CreateCellStyle();
            }
            else
            {
                workbook = new XSSFWorkbook();
                sheet = workbook.CreateSheet("FB2SC5B0");
                style1 = (XSSFCellStyle)workbook.CreateCellStyle();
                style2 = (XSSFCellStyle)workbook.CreateCellStyle();
                style3 = (XSSFCellStyle)workbook.CreateCellStyle();
                style4 = (XSSFCellStyle)workbook.CreateCellStyle();
                style5 = (XSSFCellStyle)workbook.CreateCellStyle();
                styleMerge = (XSSFCellStyle)workbook.CreateCellStyle();
            }

            IFont font1 = workbook.CreateFont();
            font1.FontName = "新細明體";
            font1.FontHeightInPoints = 14;
            font1.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
            style1.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            style1.SetFont(font1);

            IFont font2 = workbook.CreateFont();
            font2.FontName = "新細明體";
            font2.FontHeightInPoints = 12;
            style2.SetFont(font2);


            styleMerge.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
            styleMerge.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Bottom;
            styleMerge.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            styleMerge.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;
            styleMerge.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            styleMerge.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            styleMerge.SetFont(font2);

            style3.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
            style3.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Bottom;
            style3.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            style3.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;
            style3.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            style3.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            style3.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            style3.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            style3.SetFont(font2);

            style4.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
            style4.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
            style4.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
            style4.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
            style4.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
            style4.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
            style4.SetFont(font2);

            IFont font3 = workbook.CreateFont();
            font3.FontName = "新細明體";
            font3.FontHeightInPoints = 12;
            font3.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;
            style5.SetFont(font3);

            IRow row = sheet.CreateRow(0);
            ICell cell;
            cell = row.CreateCell(1);
            cell.SetCellValue("薪資發放資格不符對象報表");
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 1, 12));
            cell.CellStyle = style1;

            row = sheet.CreateRow(1);
            cell = row.CreateCell(1);
            cell.SetCellValue("發薪日期：");
            cell.CellStyle = style2;

            cell = row.CreateCell(2);
            cell.SetCellValue(salary_dt);
            cell.CellStyle = style2;

            cell = row.CreateCell(3);
            cell.SetCellValue("發薪類別：");
            cell.CellStyle = style2;

            cell = row.CreateCell(4);
            cell.SetCellValue(salary_type);
            cell.CellStyle = style2;

            cell = row.CreateCell(13);
            cell.SetCellValue("列印日期：");
            cell.CellStyle = style2;

            cell = row.CreateCell(14);
            cell.SetCellValue(DateTime.Today.ToString("yyyy/MM/dd"));
            cell.CellStyle = style2;

            row = sheet.CreateRow(2);
            IRow row3 = sheet.CreateRow(3);
            cell = row.CreateCell(1);
            row3.CreateCell(1).CellStyle = styleMerge;
            cell.SetCellValue("勤務日期");
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(2, 3, 1, 1));
            cell.CellStyle = style3;
            

            cell = row.CreateCell(2);
            row3.CreateCell(2).CellStyle = styleMerge;
            cell.SetCellValue("工號");
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(2, 3, 2, 2));
            cell.CellStyle = style3;
            

            cell = row.CreateCell(3);
            row3.CreateCell(3).CellStyle = styleMerge;
            cell.SetCellValue("姓名");
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(2, 3, 3, 3));
            cell.CellStyle = style3;
            

            cell = row.CreateCell(4);
            row3.CreateCell(4).CellStyle = styleMerge;
            cell.SetCellValue("部門");
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(2, 3, 4, 4));
            cell.CellStyle = style3;
            

            cell = row.CreateCell(5);
            row3.CreateCell(5).CellStyle = styleMerge;
            cell.SetCellValue("部門代碼");
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(2, 3, 5, 5));
            cell.CellStyle = style3;
            

            cell = row.CreateCell(6);
            row3.CreateCell(6).CellStyle = styleMerge;
            cell.SetCellValue("職務");
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(2, 3, 6, 6));
            cell.CellStyle = style3;
            

            cell = row.CreateCell(7);
            row3.CreateCell(7).CellStyle = styleMerge;
            cell.SetCellValue("離職日期");
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(2, 3, 7, 7));
            cell.CellStyle = style3;
            

            cell = row.CreateCell(8);
            row3.CreateCell(8).CellStyle = styleMerge;
            cell.SetCellValue("日勤務班表");
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(2, 3, 8, 8));
            cell.CellStyle = style3;
            

            cell = row.CreateCell(9);
            row3.CreateCell(9).CellStyle = styleMerge;
            cell.SetCellValue("子假別代號");
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(2, 3, 9, 9));
            cell.CellStyle = style3;
            

            cell = row.CreateCell(10);
            row.CreateCell(11).CellStyle = style3;
            cell.SetCellValue("刷卡資料");
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(2, 2, 10, 11));
            cell.CellStyle = style3;
            

            cell = row.CreateCell(12);
            row3.CreateCell(12).CellStyle = styleMerge;
            cell.SetCellValue("異常原因");
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(2, 3, 12, 12));
            cell.CellStyle = style3;
            

            cell = row.CreateCell(13);
            row3.CreateCell(13).CellStyle = styleMerge;
            cell.SetCellValue("異常時數");
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(2, 3, 13, 13));
            cell.CellStyle = style3;
            

            cell = row.CreateCell(14);
            row3.CreateCell(14).CellStyle = styleMerge;
            cell.SetCellValue("異常時數累計");
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(2, 3, 14, 14));
            cell.CellStyle = style3;
            


            cell = row3.CreateCell(10);
            cell.SetCellValue("刷卡上班");
            cell.CellStyle = style3;

            cell = row3.CreateCell(11);
            cell.SetCellValue("刷卡下班");
            cell.CellStyle = style3;

            if (salary_data[0].Item1 == "A")
            {
                DataTable getData1 = fb2sc.getData1(salary_ym);
                remarkrow = getData1.Rows.Count + 5;
                for (int i = 0; i < getData1.Rows.Count; i++)
                {
                    row = sheet.CreateRow(i + 4);
                    cell = row.CreateCell(1);
                    cell.CellStyle = style4;
                    cell.SetCellValue(getData1.Rows[i]["CALENDAR_DT"].ToString());

                    cell = row.CreateCell(2);
                    cell.CellStyle = style4;
                    cell.SetCellValue(getData1.Rows[i]["EMP_ID"].ToString());

                    cell = row.CreateCell(3);
                    cell.CellStyle = style4;
                    cell.SetCellValue(getData1.Rows[i]["EMP_NAME"].ToString());

                    cell = row.CreateCell(4);
                    cell.CellStyle = style4;
                    cell.SetCellValue(getData1.Rows[i]["DEPT_NAME"].ToString());

                    cell = row.CreateCell(5);
                    cell.CellStyle = style4;
                    cell.SetCellValue(getData1.Rows[i]["DEPT_NO"].ToString());

                    cell = row.CreateCell(6);
                    cell.CellStyle = style4;
                    cell.SetCellValue(getData1.Rows[i]["PJOB_CD"].ToString() + "-" + getData1.Rows[i]["PJOB_DESC"].ToString());

                    cell = row.CreateCell(7);
                    cell.CellStyle = style4;
                    cell.SetCellValue(getData1.Rows[i]["LEAVE_DT"].ToString());

                    cell = row.CreateCell(8);
                    cell.CellStyle = style4;
                    cell.SetCellValue(getData1.Rows[i]["SHIFT_DESC"].ToString());

                    cell = row.CreateCell(9);
                    cell.CellStyle = style4;
                    cell.SetCellValue(getData1.Rows[i]["SUB_LEAVE_CD"].ToString());

                    cell = row.CreateCell(10);
                    cell.CellStyle = style4;
                    cell.SetCellValue(getData1.Rows[i]["CLOCK_IN_DT"].ToString());

                    cell = row.CreateCell(11);
                    cell.CellStyle = style4;
                    cell.SetCellValue(getData1.Rows[i]["CLOCK_OUT_DT"].ToString());

                    cell = row.CreateCell(12);
                    cell.CellStyle = style4;
                    cell.SetCellValue(getData1.Rows[i]["MSG"].ToString());

                    cell = row.CreateCell(13);
                    cell.CellStyle = style4;
                    cell.SetCellValue(getData1.Rows[i]["LACK_HOUR"].ToString());

                    cell = row.CreateCell(14);
                    cell.CellStyle = style4;
                    cell.SetCellValue(getData1.Rows[i]["TOTAL_LACK_HOUR"].ToString());
                }
            }
            if (salary_data[0].Item1 != "A")
            {
                DataTable getData2 = fb2sc.getData2(salary_dt, salary_data[0].Item1, salary_data[0].Item2);
                remarkrow = getData2.Rows.Count + 5;
                for (int i = 0; i < getData2.Rows.Count; i++)
                {
                    row = sheet.CreateRow(i + 4);
                    cell = row.CreateCell(1);
                    cell.CellStyle = style4;
                    cell.SetCellValue(getData2.Rows[i]["CALENDAR_DT"].ToString());

                    cell = row.CreateCell(2);
                    cell.CellStyle = style4;
                    cell.SetCellValue(getData2.Rows[i]["EMP_ID"].ToString());

                    cell = row.CreateCell(3);
                    cell.CellStyle = style4;
                    cell.SetCellValue(getData2.Rows[i]["EMP_NAME"].ToString());

                    cell = row.CreateCell(4);
                    cell.CellStyle = style4;
                    cell.SetCellValue(getData2.Rows[i]["DEPT_NAME"].ToString());

                    cell = row.CreateCell(5);
                    cell.CellStyle = style4;
                    cell.SetCellValue(getData2.Rows[i]["DEPT_NO"].ToString());

                    cell = row.CreateCell(6);
                    cell.CellStyle = style4;
                    cell.SetCellValue(getData2.Rows[i]["PJOB_CD"].ToString() + "-" + getData2.Rows[i]["PJOB_DESC"].ToString());

                    cell = row.CreateCell(7);
                    cell.CellStyle = style4;
                    cell.SetCellValue(getData2.Rows[i]["LEAVE_DT"].ToString());

                    cell = row.CreateCell(8);
                    cell.CellStyle = style4;
                    cell.SetCellValue(getData2.Rows[i]["SHIFT_DESC"].ToString());

                    cell = row.CreateCell(9);
                    cell.CellStyle = style4;
                    cell.SetCellValue(getData2.Rows[i]["SUB_LEAVE_CD"].ToString());

                    cell = row.CreateCell(10);
                    cell.CellStyle = style4;
                    cell.SetCellValue(getData2.Rows[i]["CLOCK_IN_DT"].ToString());

                    cell = row.CreateCell(11);
                    cell.CellStyle = style4;
                    cell.SetCellValue(getData2.Rows[i]["CLOCK_OUT_DT"].ToString());

                    cell = row.CreateCell(12);
                    cell.CellStyle = style4;
                    cell.SetCellValue(getData2.Rows[i]["MSG"].ToString());

                    cell = row.CreateCell(13);
                    cell.CellStyle = style4;
                    cell.SetCellValue(getData2.Rows[i]["LACK_HOUR"].ToString());

                    cell = row.CreateCell(14);
                    cell.CellStyle = style4;
                    cell.SetCellValue(getData2.Rows[i]["TOTAL_LACK_HOUR"].ToString());
                }
            }
            sheet.AutoSizeColumn(0, true);
            sheet.AutoSizeColumn(1, true);
            sheet.AutoSizeColumn(2, true);
            sheet.AutoSizeColumn(3, true);
            sheet.AutoSizeColumn(4, true);
            sheet.AutoSizeColumn(5, true);
            sheet.AutoSizeColumn(6, true);
            sheet.AutoSizeColumn(7, true);
            sheet.AutoSizeColumn(8, true);
            sheet.AutoSizeColumn(9, true);
            sheet.AutoSizeColumn(10, true);
            sheet.AutoSizeColumn(11, true);
            sheet.AutoSizeColumn(12, true);
            sheet.AutoSizeColumn(13, true);
            sheet.AutoSizeColumn(14, true);

            row = sheet.CreateRow(remarkrow);
            cell = row.CreateCell(1);
            cell.CellStyle = style5;
            cell.SetCellValue("資格不符條件定義：");
            //sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(16, 16, 1, 2));

            row = sheet.CreateRow(remarkrow+1);
            cell = row.CreateCell(1);
            cell.CellStyle = style5;
            cell.SetCellValue("1.勤務三日不明：作業當日(含)前三天中(由後往前推)連續2天以上欠勤(未刷卡也未請假)");
            //sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(17, 17, 1, 6));

            row = sheet.CreateRow(remarkrow+2);
            cell = row.CreateCell(1);
            cell.CellStyle = style5;
            cell.SetCellValue("2.當月累計事假+無薪病假達40小時");
            //sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(18, 18, 1, 3));

            return workbook;
            //ExcelHandle.exportExcel(workbook, "薪資發放資格不符對象報表." + type);

        }
        catch
        {
            throw;
        }
        finally
        {
            workbook.Clear();
            sheet = null;
            workbook = null;
        }
    }


    public DataTable selectEmp(string SALARY_YM)
    {
        CFB2SC5B00DAO fb2sc = new CFB2SC5B00DAO();
        try
        {
            return fb2sc.selectEmp(SALARY_YM);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public DataTable selectEmpTmp(string SALARY_DT, string SALARY_TYPE, string PAY_KIND)
    {
        CFB2SC5B00DAO fb2sc = new CFB2SC5B00DAO();
        try
        {
            return fb2sc.selectEmpTmp(SALARY_DT, SALARY_TYPE, PAY_KIND);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public string insertEmp(string emp_id)
    {
        CFB2SC5B00DAO fb2sc = new CFB2SC5B00DAO();
        string rtnmessage = "";

        if (rtnmessage == "")
        {
            try
            {
                BeginTransaction();
                fb2sc.insertEmp(emp_id);
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
}