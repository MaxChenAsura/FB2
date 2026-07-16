using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.SS.Util;

/// <summary>
/// WFB2SJ0530Service 的摘要描述
/// </summary>
public class CFB2SJ0530BO : BaseService
{
    public CFB2SJ0530BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    //取得部門資料By EMPID
    public DataTable getDeptDataByEmpId(CFB2SJ0530DAO dao)
    {
        try
        {
            return dao.getDeptDataByEmpId();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public IWorkbook createstatisticsExcel(string assess_year, string assess_type, string emp_id, string type)
    {
        try
        {
            IWorkbook workbook;
            ISheet sheet;
            ICellStyle styleTilte1;
            ICellStyle styleTilte2;
            ICellStyle style1;
            ICellStyle style2;
            ICellStyle style3;
            ICellStyle style4;
            CFB2SJ0500DAO dao0500 = new CFB2SJ0500DAO();
            dao0500.ASSESS_YEAR = assess_year;
            dao0500.ASSESS_TYPE = assess_type;
            dao0500.EMP_ID = emp_id;
            DataTable dt = dao0500.getEmpTargetData();
            if (dt.Rows.Count == 0 ) return null;
                CFB2SJCOMMBO styleBO = new CFB2SJCOMMBO();
                workbook = new XSSFWorkbook();
                sheet = workbook.CreateSheet("考核資料下載");
                ICellStyle style_label = styleBO.setCellStyle(workbook, "center", true, 10, 0, 47, false, "微軟正黑體");
                ICellStyle style_label2 = styleBO.setCellStyle(workbook, "center", true, 10, 0, 47, false, "微軟正黑體");
                ICellStyle style_body1 = styleBO.setCellStyle(workbook, "center", false, 10, 0, 0, false, "微軟正黑體");
                ICellStyle style_body2 = styleBO.setCellStyle(workbook, "center", true, 10, 0, 0, false, "微軟正黑體");
                ICellStyle style_body3 = styleBO.setCellStyle(workbook, "left", false, 10, 0, 0, false, "微軟正黑體");
                styleTilte1 = (XSSFCellStyle)workbook.CreateCellStyle();
                styleTilte2 = (XSSFCellStyle)workbook.CreateCellStyle();
                style1 = (XSSFCellStyle)workbook.CreateCellStyle();
                style2 = (XSSFCellStyle)workbook.CreateCellStyle();
                style3 = (XSSFCellStyle)workbook.CreateCellStyle();
                style4 = (XSSFCellStyle)workbook.CreateCellStyle();
            
            IFont fontTitle1 = workbook.CreateFont();
            fontTitle1.FontName = "微軟正黑體";
            fontTitle1.FontHeightInPoints = 22;
            //font1.Color = NPOI.HSSF.Util.HSSFColor.White.Index;
            styleTilte1.SetFont(fontTitle1);
            styleTilte1.Alignment = HorizontalAlignment.Center;
            //styleTitle1.BorderBottom = NPOI.SS.UserModel.BorderStyle.DOUBLE;

            IFont fontTitle2 = workbook.CreateFont();
            fontTitle2.FontName = "微軟正黑體";
            fontTitle2.FontHeightInPoints = 16;
            //font1.Color = NPOI.HSSF.Util.HSSFColor.White.Index;
            styleTilte2.SetFont(fontTitle2);
            styleTilte2.Alignment = HorizontalAlignment.Center;

            IFont font1 = workbook.CreateFont();
            font1.FontName = "新細明體";
            font1.FontHeightInPoints = 10;
            //font1.Color = NPOI.HSSF.Util.HSSFColor.White.Index;
            style1.SetFont(font1);
            style1.Alignment = HorizontalAlignment.Left;
            style1.BorderBottom = BorderStyle.Thin;
            style1.BorderTop = BorderStyle.Thin;
            style1.BorderRight = BorderStyle.Thin;
            style1.BorderLeft = BorderStyle.Thin;

            font1 = workbook.CreateFont();
            font1.FontName = "新細明體";
            font1.FontHeightInPoints = 10;
            style2.SetFont(font1);
            style2.Alignment = HorizontalAlignment.Center;
            style2.BorderBottom = BorderStyle.Thin;
            style2.BorderTop = BorderStyle.Thin;
            style2.BorderRight = BorderStyle.Thin;
            style2.BorderLeft = BorderStyle.Thin;
            
            style3.SetFont(font1);
            style3.Alignment = HorizontalAlignment.Left;
            style3.WrapText = true;
            style3.BorderBottom = BorderStyle.Thin;
            style3.BorderTop = BorderStyle.Thin;
            style3.BorderRight = BorderStyle.Thin;
            style3.BorderLeft = BorderStyle.Thin;
            //表頭
            IRow row = sheet.CreateRow(0);
            ICell cell;
            cell = row.CreateCell(0);
            cell.CellStyle = style_body1;
            cell.SetCellValue("SJ053");

            cell = row.CreateCell(1);
            cell.CellStyle = style_body3;
            cell.SetCellValue("【個人考核資料】");
            //line-2
            row = sheet.CreateRow(1);
            cell = row.CreateCell(1);
            cell.CellStyle = style_label;
            cell.SetCellValue("考核年度");

            cell = row.CreateCell(2);
            cell.CellStyle = style_body1;
            cell.SetCellValue(dt.Rows[0]["ASSESS_YEAR"].ToString());

            cell = row.CreateCell(3);
            cell.CellStyle = style_label;
            cell.SetCellValue("考核類別");

            cell = row.CreateCell(4);
            cell.CellStyle = style_body1;
            cell.SetCellValue(dt.Rows[0]["ASSESS_TYPE_NAME"].ToString());

            cell = row.CreateCell(5);
            cell.CellStyle = style_label;
            cell.SetCellValue("提出主管");

            cell = row.CreateCell(6);
            cell.CellStyle = style_body1;
            cell.SetCellValue(dt.Rows[0]["DIREC_EMP_NAME"].ToString());

            //line-3
            row = sheet.CreateRow(2);
            cell = row.CreateCell(1);
            cell.CellStyle = style_label;
            cell.SetCellValue("部門名稱");

            cell = row.CreateCell(2);
            cell.CellStyle = style_body3;
            cell.SetCellValue(dt.Rows[0]["DEPT_FULL_NAME"].ToString());
            cell = row.CreateCell(3);
            cell.CellStyle = style_body3;
            cell = row.CreateCell(4);
            cell.CellStyle = style_body3;
            cell = row.CreateCell(5);
            cell.CellStyle = style_body3;
            cell = row.CreateCell(6);
            cell.CellStyle = style_body3;
            sheet.AddMergedRegion(new CellRangeAddress(2, 2, 2, 6));           

            //line-4
            row = sheet.CreateRow(3);
            cell = row.CreateCell(1);
            cell.CellStyle = style_label;
            cell.SetCellValue("工號");

            cell = row.CreateCell(2);
            cell.CellStyle = style_body1;
            cell.SetCellValue(dt.Rows[0]["EMP_ID"].ToString());

            cell = row.CreateCell(3);
            cell.CellStyle = style_label;
            cell.SetCellValue("姓名");

            cell = row.CreateCell(4);
            cell.CellStyle = style_body1;
            cell.SetCellValue(dt.Rows[0]["EMP_NAME"].ToString());

            cell = row.CreateCell(5);
            cell.CellStyle = style_label;
            cell.SetCellValue("職種");

            cell = row.CreateCell(6);
            cell.CellStyle = style_body1;
            cell.SetCellValue(dt.Rows[0]["WS_CD_DESC"].ToString());

            

            //line-5
            row = sheet.CreateRow(4);
            cell = row.CreateCell(1);
            cell.CellStyle = style_label;
            cell.SetCellValue("資格");

            cell = row.CreateCell(2);
            cell.CellStyle = style_body1;
            cell.SetCellValue(dt.Rows[0]["LEVEL_CD"].ToString());

            cell = row.CreateCell(3);
            cell.CellStyle = style_label;
            cell.SetCellValue("職務名稱");

            cell = row.CreateCell(4);
            cell.CellStyle = style_body1;
            cell.SetCellValue(dt.Rows[0]["PJOB_DESC"].ToString());

            cell = row.CreateCell(5);
            cell.CellStyle = style_body1;
            cell = row.CreateCell(6);
            cell.CellStyle = style_body1;
            sheet.AddMergedRegion(new CellRangeAddress(4, 4, 4, 6));

            //line-6
            row = sheet.CreateRow(5);
            cell = row.CreateCell(1);
            cell.CellStyle = style_label;
            cell.SetCellValue("年齡");

            cell = row.CreateCell(2);
            cell.CellStyle = style_body1;
            cell.SetCellValue(dt.Rows[0]["AGE"].ToString());

            cell = row.CreateCell(3);
            cell.CellStyle = style_label;
            cell.SetCellValue("入社年資");

            cell = row.CreateCell(4);
            cell.CellStyle = style_body1;
            cell.SetCellValue(dt.Rows[0]["WORK_YEARS"].ToString());

            cell = row.CreateCell(5);
            cell.CellStyle = style_label;
            cell.SetCellValue("資格年資");

            cell = row.CreateCell(6);
            cell.CellStyle = style_body1;
            cell.SetCellValue(dt.Rows[0]["RECENT_LEVEL_WORK_YEARS"].ToString());

            //line-7
            row = sheet.CreateRow(6);
            cell = row.CreateCell(1);
            cell.CellStyle = style_label;
            cell.SetCellValue("備註內容");

            cell = row.CreateCell(2);
            cell.CellStyle = style_body3;
            cell.SetCellValue(dt.Rows[0]["DISTING_REMARK"].ToString());
            cell = row.CreateCell(3);
            cell.CellStyle = style_body1;
            cell = row.CreateCell(4);
            cell.CellStyle = style_body1;
            cell = row.CreateCell(5);
            cell.CellStyle = style_body1;
            cell = row.CreateCell(6);
            cell.CellStyle = style_body1;
            sheet.AddMergedRegion(new CellRangeAddress(6, 6, 2, 6));

            //line-8
            row = sheet.CreateRow(7);
            //line-9
            row = sheet.CreateRow(8); 
            cell = row.CreateCell(1);
            cell.CellStyle = style_label;
            cell.SetCellValue("考核履歷");

            cell = row.CreateCell(2);
            cell.CellStyle = style_body2;
            cell.SetCellValue("能力");

            cell = row.CreateCell(3);
            cell.CellStyle = style_body2;
            cell.SetCellValue("業績");

            cell = row.CreateCell(4);
            cell.CellStyle = style_body1;
            cell.SetCellValue("");

            cell = row.CreateCell(5);
            cell.CellStyle = style_label;
            cell.SetCellValue("勤怠記錄");

            cell = row.CreateCell(6);
            cell.CellStyle = style_body1;
            cell.SetCellValue("");

            //line-10
            row = sheet.CreateRow(9);
            cell = row.CreateCell(1);
            cell.CellStyle = style_body2;
            cell.SetCellValue("前1年");

            cell = row.CreateCell(2);
            cell.CellStyle = style_body2;
            cell.SetCellValue(dt.Rows[0]["SCORE_1H_1"].ToString());

            cell = row.CreateCell(3);
            cell.CellStyle = style_body2;
            cell.SetCellValue(dt.Rows[0]["SCORE_2H_1"].ToString());

            cell = row.CreateCell(4);
            cell.CellStyle = style_body1;
            cell.SetCellValue("");

            cell = row.CreateCell(5);
            cell.CellStyle = style_body1;
            cell.SetCellValue("遲/早(次)");

            cell = row.CreateCell(6);
            cell.CellStyle = style_body1;
            cell.SetCellValue(dt.Rows[0]["LEAVE_OP"].ToString());

            //line-11
            row = sheet.CreateRow(10);
            cell = row.CreateCell(1);
            cell.CellStyle = style_body2;
            cell.SetCellValue("前2年");

            cell = row.CreateCell(2);
            cell.CellStyle = style_body2;
            cell.SetCellValue(dt.Rows[0]["SCORE_1H_2"].ToString());

            cell = row.CreateCell(3);
            cell.CellStyle = style_body2;
            cell.SetCellValue(dt.Rows[0]["SCORE_2H_2"].ToString());

            cell = row.CreateCell(4);
            cell.CellStyle = style_body1;
            cell.SetCellValue("");

            cell = row.CreateCell(5);
            cell.CellStyle = style_body1;
            cell.SetCellValue("曠職(天)");

            cell = row.CreateCell(6);
            cell.CellStyle = style_body1;
            cell.SetCellValue(dt.Rows[0]["LEAVE_Q"].ToString());

            //line-12
            row = sheet.CreateRow(11);
            cell = row.CreateCell(1);
            cell.CellStyle = style_body2;
            cell.SetCellValue("前3年");

            cell = row.CreateCell(2);
            cell.CellStyle = style_body2;
            cell.SetCellValue(dt.Rows[0]["SCORE_1H_3"].ToString());

            cell = row.CreateCell(3);
            cell.CellStyle = style_body2;
            cell.SetCellValue(dt.Rows[0]["SCORE_2H_3"].ToString());

            cell = row.CreateCell(4);
            cell.CellStyle = style_body1;
            cell.SetCellValue("");

            cell = row.CreateCell(5);
            cell.CellStyle = style_body1;
            cell.SetCellValue("事/病假(天)");

            cell = row.CreateCell(6);
            cell.CellStyle = style_body1;
            cell.SetCellValue(dt.Rows[0]["LEAVE_AB"].ToString());

            //line-13
            row = sheet.CreateRow(12);
            //line-14
            row = sheet.CreateRow(13);
            cell = row.CreateCell(1);
            cell.CellStyle = style_body3;
            cell.SetCellValue("【考核評分欄】");
            cell = row.CreateCell(2);
            cell.CellStyle = style_body1;
            cell.SetCellValue(dt.Rows[0]["ASSESS_TYPE_NAME"].ToString());
            cell = row.CreateCell(3);
            cell.CellStyle = style_body1;
            cell = row.CreateCell(4);
            cell.CellStyle = style_body1;
            cell = row.CreateCell(5);
            cell.CellStyle = style_body1;
            cell = row.CreateCell(6);
            cell.CellStyle = style_body1;
            //sheet.AddMergedRegion(new CellRangeAddress(13, 13, 1, 6));

            DataTable dtEAS = dao0500.getEmpAssessScore(0, 20, "ASSESS_YEAR", assess_year, assess_type, emp_id);
            //line-15
            row = sheet.CreateRow(14);
            cell = row.CreateCell(1);
            cell.CellStyle = style_label2;
            cell.SetCellValue("考核評價要素");
            cell = row.CreateCell(2);
            cell.CellStyle = style_label2;
            sheet.AddMergedRegion(new CellRangeAddress(14, 14, 1, 2)); 
            cell = row.CreateCell(3);
            cell.CellStyle = style_label2;
            cell.SetCellValue("最高分");
            cell = row.CreateCell(4);
            cell.CellStyle = style_label2;
            cell.SetCellValue("分數");

            int iRowIndex = 15;
            int total_grande = 0;
            if (dtEAS.Rows.Count > 0)
            {
                for (int j = 0; j < dtEAS.Rows.Count; j++)
                {
                    row = sheet.CreateRow(iRowIndex);
                    cell = row.CreateCell(1);
                    cell.CellStyle = style_body2;
                    cell.SetCellValue(dtEAS.Rows[j]["ITEM_DESC"].ToString());
                    cell = row.CreateCell(2);
                    cell.CellStyle = style_body2;
                    sheet.AddMergedRegion(new CellRangeAddress(iRowIndex, iRowIndex, 1, 2));
                    cell = row.CreateCell(3);
                    cell.CellStyle = style_body2;
                    cell.SetCellValue(dtEAS.Rows[j]["MAX_GRADE"].ToString());
                    cell = row.CreateCell(4);
                    cell.CellStyle = style_body2;
                    cell.SetCellValue(dtEAS.Rows[j]["MNG_GRADE"].ToString());
                    total_grande += Int32.Parse(dtEAS.Rows[j]["MNG_GRADE"].ToString());
                    iRowIndex++;
                }

            }
            row = sheet.CreateRow(iRowIndex);
            cell = row.CreateCell(1);
            cell = row.CreateCell(2);
            cell = row.CreateCell(3);
            cell.CellStyle = style_label;
            cell.SetCellValue("分數合計");
            cell = row.CreateCell(4);
            cell.CellStyle = style_body1;
            cell.SetCellValue(total_grande);
            cell = row.CreateCell(5);
            cell.CellStyle = style_label;
            cell.SetCellValue("考課");
            cell = row.CreateCell(6);
            cell.CellStyle = style_body1;
            cell.SetCellValue(dt.Rows[0]["SCORE_FINAL"].ToString());
            iRowIndex++;

            row = sheet.CreateRow(iRowIndex);
            cell = row.CreateCell(1);
            cell.CellStyle = style_body3;
            cell.SetCellValue("【推薦區分】");
            cell = row.CreateCell(2);
            cell.CellStyle = style_body3;
            cell.SetCellValue(dt.Rows[0]["RECOMM_DESC"].ToString());
            cell = row.CreateCell(3);
            cell.CellStyle = style_body3;
            cell = row.CreateCell(4);
            cell.CellStyle = style_body3;
            cell = row.CreateCell(5);
            cell.CellStyle = style_body3;
            cell = row.CreateCell(6);
            cell.CellStyle = style_body3;
            sheet.AddMergedRegion(new CellRangeAddress(iRowIndex, iRowIndex, 2, 6));
            iRowIndex++;
            /**
            row = sheet.CreateRow(iRowIndex);
            cell = row.CreateCell(1);
            cell.CellStyle = style1;
            cell.SetCellValue(dt.Rows[0]["DISTING_REMARK"].ToString());
            cell = row.CreateCell(2);
            cell.CellStyle = style1;
            cell = row.CreateCell(3);
            cell.CellStyle = style1;
            cell = row.CreateCell(4);
            cell.CellStyle = style1;
            cell = row.CreateCell(5);
            cell.CellStyle = style1;
            cell = row.CreateCell(6);
            cell.CellStyle = style1;
            sheet.AddMergedRegion(new CellRangeAddress(iRowIndex, iRowIndex, 2, 6));
            iRowIndex++;
            **/
            row = sheet.CreateRow(iRowIndex);
            cell = row.CreateCell(1);
            cell.CellStyle = style_body3;
            cell.SetCellValue("【初核總評】");
            cell = row.CreateCell(2);
            cell.CellStyle = style_body1;
            cell = row.CreateCell(3);
            cell.CellStyle = style_body1;
            cell = row.CreateCell(4);
            cell.CellStyle = style_body1;
            cell = row.CreateCell(5);
            cell.CellStyle = style_body1;
            cell = row.CreateCell(6);
            cell.CellStyle = style_body1;
            //sheet.AddMergedRegion(new CellRangeAddress(iRowIndex, iRowIndex, 1, 6));
            iRowIndex++;

            row = sheet.CreateRow(iRowIndex);
            row.Height = 1000;
            cell = row.CreateCell(1);
            cell.CellStyle = style_body1;
            cell.SetCellValue(dt.Rows[0]["COMMENTS"].ToString());
            cell = row.CreateCell(2);
            cell.CellStyle = style_body1;
            cell = row.CreateCell(3);
            cell.CellStyle = style_body1;
            cell = row.CreateCell(4);
            cell.CellStyle = style_body1;
            cell = row.CreateCell(5);
            cell.CellStyle = style_body1;
            cell = row.CreateCell(6);
            cell.CellStyle = style_body1;
            sheet.AddMergedRegion(new CellRangeAddress(iRowIndex, iRowIndex, 1, 6));
            iRowIndex++;

            row = sheet.CreateRow(iRowIndex);
            cell = row.CreateCell(1);
            cell.CellStyle = style_body3;
            cell.SetCellValue("【考核歷程】");
            cell = row.CreateCell(2);
            cell.CellStyle = style_body3;
            cell = row.CreateCell(3);
            cell.CellStyle = style_body3;
            cell = row.CreateCell(4);
            cell.CellStyle = style_body3;
            cell = row.CreateCell(5);
            cell.CellStyle = style_body3;
            cell = row.CreateCell(6);
            cell.CellStyle = style_body3;
            sheet.AddMergedRegion(new CellRangeAddress(iRowIndex, iRowIndex, 2, 6));
            iRowIndex++;

            row = sheet.CreateRow(iRowIndex);
            cell = row.CreateCell(1);
            cell.CellStyle = style_label2;
            cell.SetCellValue("序號");
            cell = row.CreateCell(2);
            cell.CellStyle = style_label2;
            cell.SetCellValue("考課");
            cell = row.CreateCell(3);
            cell.CellStyle = style_label2;
            cell.SetCellValue("異動工號");
            cell = row.CreateCell(4);
            cell.CellStyle = style_label2;
            cell.SetCellValue("異動姓名");
            cell = row.CreateCell(5);
            cell.CellStyle = style_label2;
            cell.SetCellValue("異動日期");
            cell = row.CreateCell(6);
            cell.CellStyle = style_label2;
            cell.SetCellValue("更正說明");
            iRowIndex++;

            CFB2SJ0510DAO dao0510 = new CFB2SJ0510DAO();
            CFB2SJ0510BO bo0510 = new CFB2SJ0510BO();
            dao0510.ASSESS_YEAR = assess_year;
            dao0510.ASSESS_TYPE = assess_type;
            dao0510.EMP_ID = emp_id;
            DataTable dtLog = bo0510.getAssessLog(dao0510);
            if (dtLog.Rows.Count > 0)
            {
                for (int j = 0; j < dtLog.Rows.Count; j++)
                {
                    row = sheet.CreateRow(iRowIndex);
                    cell = row.CreateCell(1);
                    cell.CellStyle = style_body2;
                    cell.SetCellValue(dtLog.Rows[j]["RowNumber"].ToString());
                    cell = row.CreateCell(2);
                    cell.CellStyle = style_body2;
                    cell.SetCellValue(dtLog.Rows[j]["GRADE"].ToString());
                    cell = row.CreateCell(3);
                    cell.CellStyle = style_body2;
                    cell.SetCellValue(dtLog.Rows[j]["CREATED_BY"].ToString());
                    cell = row.CreateCell(4);
                    cell.CellStyle = style_body2;
                    cell.SetCellValue(dtLog.Rows[j]["EMP_NAME"].ToString());
                    cell = row.CreateCell(5);
                    cell.CellStyle = style_body2;
                    cell.SetCellValue(dtLog.Rows[j]["CREATED_DT"].ToString());
                    cell = row.CreateCell(6);
                    cell.CellStyle = style_body2;
                    cell.SetCellValue(dtLog.Rows[j]["MEMO"].ToString());
                    iRowIndex++;
                }

            }
            
            //for end
            for (int i = 0; i < 7; i++)
            {
                //sheet.AutoSizeColumn(i);
                sheet.SetColumnWidth(i, 4000);
            }

            //ExcelHandle.exportExcel(workbook, "FB2DF040_EMP." + type);
            return workbook;


        }
        catch (Exception)
        {
            throw;
        }
    }
    public string chtdate(string str)
    {
        //TaiwanCalendar twC = new TaiwanCalendar();
        String st = DateTime.Parse(str).ToString("yyyy");
        string st1 = DateTime.Parse(str).ToString("MMdd");
        string tdate = Convert.ToString(Convert.ToString(Convert.ToInt32(st) - 1911)) + st1;
        return tdate;
    }
}