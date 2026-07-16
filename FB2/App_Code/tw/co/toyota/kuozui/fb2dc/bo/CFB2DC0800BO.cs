using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;

/// <summary>
/// CFB2DC0800BO 的摘要描述
/// </summary>
public class CFB2DC0800BO : BaseService
{
    public CFB2DC0800BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public IWorkbook createExcel(CFB2DC0800DAO dao, string type)
    {
        IWorkbook workbook = null;
        ISheet sheet = null;
        try
        {
            ICellStyle style1;
            ICellStyle style2;
            ICellStyle style3;
            ICellStyle style4;
            ICellStyle style5;
            ICellStyle style6;
            DataTable tmp = dao.searchResult();
            //string rtnmessage = "";
            //if (tmp.Rows.Count == 0)
            //{
            //    rtnmessage = "無匯出資料";
            //}

            if (tmp.Rows.Count > 0)
            {
                if (type == "xls")
                {
                    workbook = new HSSFWorkbook();
                    sheet = (HSSFSheet)workbook.CreateSheet("日勤務狀態表");
                    style1 = (HSSFCellStyle)workbook.CreateCellStyle();
                }
                else
                {
                    workbook = new XSSFWorkbook();
                    sheet = workbook.CreateSheet("日勤務狀態表");
                    style1 = (XSSFCellStyle)workbook.CreateCellStyle();
                }

                IFont font1 = workbook.CreateFont();
                font1.FontName = "新細明體";
                font1.FontHeightInPoints = 12;

                IFont font2 = workbook.CreateFont();
                font2.FontName = "新細明體";
                font2.FontHeightInPoints = 14;

                //標題 樣式
                style3 = (XSSFCellStyle)workbook.CreateCellStyle();
                style3.SetFont(font2);
                style3.Alignment = HorizontalAlignment.Center;
                style3.VerticalAlignment = VerticalAlignment.Center;

                //grid header 樣式
                style4 = (XSSFCellStyle)workbook.CreateCellStyle();
                ((XSSFCellStyle)style4).SetFillForegroundColor(new XSSFColor(Color.LightGray));
                ((XSSFCellStyle)style4).FillPattern = FillPattern.SolidForeground;
                ((XSSFCellStyle)style4).BorderBottom = BorderStyle.Thin;
                ((XSSFCellStyle)style4).BorderLeft = BorderStyle.Thin;
                ((XSSFCellStyle)style4).BorderRight = BorderStyle.Thin;
                ((XSSFCellStyle)style4).BorderTop = BorderStyle.Thin;
                style4.SetFont(font1);
                style4.Alignment = HorizontalAlignment.Center;
                style4.VerticalAlignment = VerticalAlignment.Center;

                style1.SetFont(font1);

                style6 = (XSSFCellStyle)workbook.CreateCellStyle();
                style6.SetFont(font1);
                style6.Alignment = HorizontalAlignment.Right;

                IRow row = sheet.CreateRow(0);
                ICell cell;
                cell = row.CreateCell(1);
                cell.SetCellValue("日勤務狀態表");
                cell.CellStyle = style3;
                sheet.AddMergedRegion(new CellRangeAddress(0, 0, 1, 25));

                row = sheet.CreateRow(1);
                cell = row.CreateCell(1);
                cell.SetCellValue("工號：" + dao.EMP_ID + "  " + dao.EMP_NAME);
                cell.CellStyle = style1;
                sheet.AddMergedRegion(new CellRangeAddress(1, 1, 1, 6));

                row = sheet.CreateRow(2);
                cell = row.CreateCell(1);
                cell.SetCellValue("勤務日期：" + dao.CALENDAR_DT_S + "~" + dao.CALENDAR_DT_E);
                cell.CellStyle = style1;
                sheet.AddMergedRegion(new CellRangeAddress(2, 2, 1, 7));

                row = sheet.CreateRow(3);
                cell = row.CreateCell(1);
                cell.SetCellValue("部門：" + dao.DEPT_NO + "  " + dao.DEPT_NAME);
                cell.CellStyle = style1;
                sheet.AddMergedRegion(new CellRangeAddress(3, 3, 1, 6));

                row = sheet.CreateRow(4);
                cell = row.CreateCell(1);
                cell.SetCellValue("刷卡比對狀態：" + dao.DUTY_CHECK_RESULT_DESC);
                cell.CellStyle = style1;
                sheet.AddMergedRegion(new CellRangeAddress(4, 4, 1, 4));

                cell = row.CreateCell(22);
                cell.CellStyle = style6;
                cell.CellStyle.Alignment = HorizontalAlignment.Right;
                //cell.SetCellValue("列印日期：" + DateTime.Now.ToString("yyyy/MM/dd HH:mm"));
                cell.SetCellValue("製表日期：" + DateTime.Now.ToString("yyyy/MM/dd"));
                sheet.AddMergedRegion(new CellRangeAddress(4, 4, 22, 25));

                row = sheet.CreateRow(5);

                cell = row.CreateCell(1);
                cell.CellStyle = style4;
                cell.SetCellValue("部門");

                cell = row.CreateCell(2);
                cell.CellStyle = style4;
                cell.SetCellValue("工號");

                cell = row.CreateCell(3);
                cell.CellStyle = style4;
                cell.SetCellValue("姓名");

                cell = row.CreateCell(4);
                cell.CellStyle = style4;
                cell.SetCellValue("勤務日");

                cell = row.CreateCell(5);
                cell.CellStyle = style4;
                cell.SetCellValue("出勤別");

                cell = row.CreateCell(6);
                cell.CellStyle = style4;
                cell.SetCellValue("勤務上班時間");

                cell = row.CreateCell(7);
                cell.CellStyle = style4;
                cell.SetCellValue("勤務下班時間");

                cell = row.CreateCell(8);
                cell.CellStyle = style4;
                cell.SetCellValue("刷卡上班時間");

                cell = row.CreateCell(9);
                cell.CellStyle = style4;
                cell.SetCellValue("刷卡上班資料來源");

                cell = row.CreateCell(10);
                cell.CellStyle = style4;
                cell.SetCellValue("刷卡下班時間");

                cell = row.CreateCell(11);
                cell.CellStyle = style4;
                cell.SetCellValue("刷卡下班資料來源");

                cell = row.CreateCell(12);
                cell.CellStyle = style4;
                cell.SetCellValue("刷卡比對狀態");

                cell = row.CreateCell(13);
                cell.CellStyle = style4;
                cell.SetCellValue("遲到時數");

                cell = row.CreateCell(14);
                cell.CellStyle = style4;
                cell.SetCellValue("早退時數");

                cell = row.CreateCell(15);
                cell.CellStyle = style4;
                cell.SetCellValue("欠勤時數");

                cell = row.CreateCell(16);
                cell.CellStyle = style4;
                cell.SetCellValue("出勤時數");

                cell = row.CreateCell(17);
                cell.CellStyle = style4;
                cell.SetCellValue("請假核准時數");

                cell = row.CreateCell(18);
                cell.CellStyle = style4;
                cell.SetCellValue("請假資訊");

                cell = row.CreateCell(19);
                cell.CellStyle = style4;
                cell.SetCellValue("加班申請時數");

                cell = row.CreateCell(20);
                cell.CellStyle = style4;
                cell.SetCellValue("加班核准時數");

                cell = row.CreateCell(21);
                cell.CellStyle = style4;
                cell.SetCellValue("加班計算時數");

                cell = row.CreateCell(22);
                cell.CellStyle = style4;
                cell.SetCellValue("勤前加班超時時數");

                cell = row.CreateCell(23);
                cell.CellStyle = style4;
                cell.SetCellValue("勤後加班超時時數");

                cell = row.CreateCell(24);
                cell.CellStyle = style4;
                cell.SetCellValue("加班資訊");

                cell = row.CreateCell(25);
                cell.CellStyle = style4;
                cell.SetCellValue("備註");

                cell = row.CreateCell(26);
                cell.CellStyle = style4;
                cell.SetCellValue("輪班津貼");

                cell = row.CreateCell(27);
                cell.CellStyle = style4;
                cell.SetCellValue("日期類型");


                style2 = workbook.CreateCellStyle();
                ((XSSFCellStyle)style2).BorderBottom = BorderStyle.Thin;
                ((XSSFCellStyle)style2).BorderLeft = BorderStyle.Thin;
                ((XSSFCellStyle)style2).BorderRight = BorderStyle.Thin;
                ((XSSFCellStyle)style2).BorderTop = BorderStyle.Thin;
                style2.SetFont(font1);

                style5 = workbook.CreateCellStyle();
                ((XSSFCellStyle)style5).BorderBottom = BorderStyle.Thin;
                ((XSSFCellStyle)style5).BorderLeft = BorderStyle.Thin;
                ((XSSFCellStyle)style5).BorderRight = BorderStyle.Thin;
                ((XSSFCellStyle)style5).BorderTop = BorderStyle.Thin;
                style5.Alignment = HorizontalAlignment.Center;
                style5.SetFont(font1);

                int x = 0;
                for (int i = 0; i < tmp.Rows.Count; i++)
                {
                    x = i + 6;
                    row = sheet.CreateRow(x);

                    cell = row.CreateCell(1);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["DIV_DEPT_NAME"].ToString());


                    cell = row.CreateCell(2);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["EMP_ID"].ToString());

                    cell = row.CreateCell(3);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["EMP_NAME"].ToString());

                    cell = row.CreateCell(4);
                    cell.CellStyle = style5;
                    cell.SetCellValue(tmp.Rows[i]["CALENDAR_DT"].ToString());

                    cell = row.CreateCell(5);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["WORK_DAY_DESC"].ToString());

                    cell = row.CreateCell(6);
                    cell.CellStyle = style5;
                    cell.SetCellValue(tmp.Rows[i]["DUTY_STIME"].ToString());

                    cell = row.CreateCell(7);
                    cell.CellStyle = style5;
                    cell.SetCellValue(tmp.Rows[i]["DUTY_ETIME"].ToString());

                    cell = row.CreateCell(8);
                    cell.CellStyle = style5;
                    cell.SetCellValue(tmp.Rows[i]["CLOCK_IN_DT"].ToString());

                    cell = row.CreateCell(9);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["IN_DATA_SOURCE_DESC"].ToString());

                    cell = row.CreateCell(10);
                    cell.CellStyle = style5;
                    cell.SetCellValue(tmp.Rows[i]["CLOCK_OUT_DT"].ToString());

                    cell = row.CreateCell(11);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["OUT_DATA_SOURCE_DESC"].ToString());

                    cell = row.CreateCell(12);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["DUTY_CHECK_RESULT_DESC"].ToString());

                    cell = row.CreateCell(13);
                    cell.CellStyle = style5;
                    cell.SetCellValue(utilities.toHourMinute(tmp.Rows[i]["LATE_HOUR"].ToString()));

                    cell = row.CreateCell(14);
                    cell.CellStyle = style5;
                    cell.SetCellValue(utilities.toHourMinute(tmp.Rows[i]["LEAVE_EARLY_HOUR"].ToString()));

                    cell = row.CreateCell(15);
                    cell.CellStyle = style5;
                    cell.SetCellValue(utilities.toHourMinute(tmp.Rows[i]["LACK_HOUR"].ToString()));

                    cell = row.CreateCell(16);
                    cell.CellStyle = style5;
                    cell.SetCellValue(utilities.toHourMinute(tmp.Rows[i]["DUTY_HOUR"].ToString()));

                    cell = row.CreateCell(17);
                    cell.CellStyle = style5;
                    cell.SetCellValue(utilities.toHourMinute(tmp.Rows[i]["LEAVE_HOUR"].ToString()));

                    cell = row.CreateCell(18);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["LEAVE_INFO"].ToString());

                    cell = row.CreateCell(19);
                    cell.CellStyle = style5;
                    cell.SetCellValue(utilities.toHourMinute(tmp.Rows[i]["OVERTIME_HOUR_APPLY"].ToString()));

                    cell = row.CreateCell(20);
                    cell.CellStyle = style5;
                    cell.SetCellValue(utilities.toHourMinute(tmp.Rows[i]["OVERTIME_HOUR_APPROVE"].ToString()));

                    cell = row.CreateCell(21);
                    cell.CellStyle = style5;
                    cell.SetCellValue(utilities.toHourMinute(tmp.Rows[i]["OVERTIME_PAY_HOUR"].ToString()));
                    
                    cell = row.CreateCell(22);
                    cell.CellStyle = style5;
                    cell.SetCellValue(utilities.toHourMinute(tmp.Rows[i]["VIOLATE_BEFORE_HOUR"].ToString()));

                    cell = row.CreateCell(23);
                    cell.CellStyle = style5;
                    cell.SetCellValue(utilities.toHourMinute(tmp.Rows[i]["VIOLATE_AFTER_HOUR"].ToString()));

                    cell = row.CreateCell(24);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["OVERTIME_INFO"].ToString());

                    cell = row.CreateCell(25);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["REMARK"].ToString());

                    cell = row.CreateCell(26);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["WORK_SHIFT_ALLOWANCE_TYPE_DESC"].ToString());

                    cell = row.CreateCell(27);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["DT_TYPE_DESC"].ToString());

                }
                sheet.SetColumnWidth(2, 8 * 256);
                sheet.SetColumnWidth(3, 10 * 256);
                sheet.SetColumnWidth(4, 12 * 256);
                sheet.SetColumnWidth(5, 10 * 256);
                sheet.SetColumnWidth(6, 18 * 256);
                sheet.SetColumnWidth(7, 18 * 256);
                sheet.SetColumnWidth(8, 18 * 256);
                sheet.SetColumnWidth(9, 20 * 256);
                sheet.SetColumnWidth(10, 18 * 256);
                sheet.SetColumnWidth(11, 20 * 256);
                sheet.SetColumnWidth(13, 12 * 256);
                sheet.SetColumnWidth(14, 12 * 256);
                sheet.SetColumnWidth(15, 12 * 256);
                sheet.SetColumnWidth(16, 12 * 256);
                sheet.SetColumnWidth(17, 16 * 256);
                sheet.SetColumnWidth(19, 16 * 256);
                sheet.SetColumnWidth(20, 16 * 256);
                sheet.SetColumnWidth(21, 16 * 256);
                sheet.SetColumnWidth(22, 22 * 256);
                sheet.SetColumnWidth(23, 22 * 256);
                sheet.SetColumnWidth(24, 16 * 256);
                sheet.SetColumnWidth(25, 20 * 256);

                sheet.AutoSizeColumn(1);
                sheet.AutoSizeColumn(12);
                sheet.AutoSizeColumn(18);
                sheet.AutoSizeColumn(23);
                sheet.AutoSizeColumn(26);
                sheet.AutoSizeColumn(27);

                //ExcelHandle.exportExcel(workbook, "FB2DC080_1." + type);

                return workbook;
            }
            else
            {
                return null;
            }
            //return rtnmessage;
        }
        catch
        {
            throw;
        }
        finally
        {
            sheet = null;
        }

    }

    public string call_SP_DC_CARD_COMPARE()
    {
        try
        {
            CFB2DC0800DAO dao = new CFB2DC0800DAO();

            dao.call_SP_DC_CARD_COMPARE();

            return "0";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }


    public string emp_DUTY_CHECK_STATUS_RE_OPEN(List<Tuple<string, string>> keysList)
    {
        string rtnmessage = "";//存在檢查後的訊息
        CFB2DC0800DAO dc080DAO = new CFB2DC0800DAO();
        try
        {
            DataTable dt = new DataTable();
            //0.是否已小於薪結月份的前一月
            foreach (var item in keysList)
            {
                dc080DAO.EMP_ID = item.Item1;
                dc080DAO.CALENDAR_DT = item.Item2;
                dt = dc080DAO.check_CALENDAR_DT();
                if ((int)dt.Rows[0]["resultCount"] ==0)
                {
                    rtnmessage += "員工:" + item.Item1 + "之勤務日期:" + item.Item2 + "已無法進行reopen \\n";
                }
            }
            if (rtnmessage == "")
            {
                foreach (var item in keysList)
                {
                    dc080DAO.EMP_ID = item.Item1;
                    dc080DAO.CALENDAR_DT = item.Item2;
                    dc080DAO.SP_D_EMP_DUTY_CHECK_STATUS_REOPEN();
                }
                return "0";  
            }
            
            return rtnmessage;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }



}