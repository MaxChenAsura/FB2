using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI;
using NPOI.SS.Util;
using NPOI.HPSF;
using NPOI.HSSF;
using NPOI.HSSF.Util;
using NPOI.HSSF.Model;
using NPOI.HSSF.UserModel;
using NPOI.POIFS;
using NPOI.Util;
using System.Text;
using System.IO;



/// <summary>
/// CFB2SD1300BO 的摘要描述
/// </summary>
public class CFB2SD1300BO : BaseService
{
    public CFB2SD1300BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public DataTable get_PDF_Data()
    {
        DataTable retVal = new DataTable(); ;
        CFB2SD1300DAO fb2sc = new CFB2SD1300DAO();
        try
        {
            retVal = fb2sc.get_PDF_Data();
            return retVal;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public System.Data.DataTable getJPN_CD()
    {
        CFB2SD1300DAO wfb2sc = new CFB2SD1300DAO();
        try
        {
            return wfb2sc.getJPN_CD();
        }
        catch (Exception)
        {

            throw;
        }
    }
    public string deleteData(string login_emp_id)
    {
        CFB2SD1300DAO wfb2sc = new CFB2SD1300DAO();
        try
        {
            BeginTransaction();

            wfb2sc.deleteData(login_emp_id);
            Commit();

            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }


    }
    public string addData(CFB2SD1300DAO wfb2sc)
    {
        try
        {
            BeginTransaction();
            wfb2sc.addData();
            Commit();
            return "0";

        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    //產生Excel
    //public IWorkbook createExcel(string txt_REMIT_DT, string ddl_SALARY_TYPE)
    //{
    //    try
    //    {
    //        CFB2SD1300DAO wfb2sd = new CFB2SD1300DAO();
    //        string type = "xlsx";
    //        IWorkbook workbook;
    //        ISheet sheet;
    //        ICellStyle style1;
    //        ICellStyle style2;
    //        DataTable tmp = wfb2sd.searchResult();
    //        DataTable tmp2 = wfb2sd.searchResult2(txt_REMIT_DT, ddl_SALARY_TYPE);
    //        if (tmp2.Rows.Count > 0)
    //        {
    //            if (type == "xls")
    //            {
    //                workbook = new HSSFWorkbook();
    //                sheet = (HSSFSheet)workbook.CreateSheet("用戶清冊");
    //                style1 = (HSSFCellStyle)workbook.CreateCellStyle();
    //            }
    //            else
    //            {
    //                workbook = new XSSFWorkbook();
    //                sheet = workbook.CreateSheet("用戶清冊");
    //                style1 = (XSSFCellStyle)workbook.CreateCellStyle();
    //            }

    //            IFont font1 = workbook.CreateFont();
    //            font1.FontName = "新細明體";
    //            font1.FontHeightInPoints = 12;
    //            style1.SetFont(font1);
    //            IRow row = sheet.CreateRow(0);
    //            ICell cell;
    //            string U1 = tmp.Rows[0]["基本資料"].ToString();
    //            int U = Convert.ToInt32(U1);
    //            string A1 = tmp.Rows[0]["應稅加項"].ToString();
    //            int A = Convert.ToInt32(A1);
    //            string B1 = tmp.Rows[0]["應稅減項"].ToString();
    //            int B = Convert.ToInt32(B1);
    //            string C1 = tmp.Rows[0]["免稅加項"].ToString();
    //            int C = Convert.ToInt32(C1);
    //            string D1 = tmp.Rows[0]["免稅減項"].ToString();
    //            int D = Convert.ToInt32(D1);
    //            string E1 = tmp.Rows[0]["加班資料"].ToString();
    //            int E = Convert.ToInt32(E1);
    //            string F1 = tmp.Rows[0]["勤務資料"].ToString();
    //            int F = Convert.ToInt32(F1);
    //            string G1 = tmp.Rows[0]["環境津貼資料"].ToString();
    //            int G = Convert.ToInt32(G1);
    //            string H1 = tmp.Rows[0]["假別資料"].ToString();
    //            int H = Convert.ToInt32(H1);
    //            string J1 = tmp.Rows[0]["剩餘假別時數"].ToString();
    //            int J = Convert.ToInt32(J1);

    //            cell = row.CreateCell(0);
    //            cell.CellStyle = style1;
    //            cell.SetCellValue("基本資料");
    //            for (int i = 0; i < tmp.Rows.Count; i++)
    //            {
    //                if (i == 0 || i == U || i == U + A || i == U + A + B || i == U + A + B + C || i == U + A + B + C + D || i == U + A + B + C + D + E || i == U + A + B + C + D + E + F || i == U + A + B + C + D + E + F + G || i == U + A + B + C + D + E + F + G + H || i == U + A + B + C + D + E + F + G + H + J)
    //                {
    //                    cell = row.CreateCell(i);
    //                    cell.CellStyle = style1;
    //                    cell.SetCellValue(tmp.Rows[i]["名稱"].ToString());
    //                }
    //                else
    //                {
    //                    cell = row.CreateCell(i);
    //                    cell.CellStyle = style1;
    //                    cell.SetCellValue("");

    //                }

    //            }


    //            style2 = workbook.CreateCellStyle();

    //            style2.SetFont(font1);

    //            row = sheet.CreateRow(1);

    //            for (int i = 0; i < tmp.Rows.Count; i++)
    //            {
    //                cell = row.CreateCell(i);
    //                cell.CellStyle = style2;
    //                cell.SetCellValue(tmp.Rows[i]["內容"].ToString());

    //            }

    //            style2 = workbook.CreateCellStyle();

    //            style2.SetFont(font1);

    //            int x = 0;
    //            if (tmp2.Rows.Count > 0)
    //            {
    //                for (int i = 0; i < tmp2.Rows.Count; i++)
    //                {
    //                    x = i + 2;
    //                    row = sheet.CreateRow(x);
    //                    cell = row.CreateCell(0);
    //                    cell.CellStyle = style2;
    //                    cell.SetCellValue(tmp2.Rows[i]["EMP_ID"].ToString());

    //                    cell = row.CreateCell(1);
    //                    cell.CellStyle = style2;
    //                    cell.SetCellValue(tmp2.Rows[i]["EMP_NAME"].ToString());

    //                    cell = row.CreateCell(2);
    //                    cell.CellStyle = style2;
    //                    cell.SetCellValue(tmp2.Rows[i]["在職區分"].ToString());

    //                    cell = row.CreateCell(3);
    //                    cell.CellStyle = style2;
    //                    cell.SetCellValue(tmp2.Rows[i]["WORK_DAYS_MONTH"].ToString());
    //                    DataTable tmp3 = wfb2sd.searchResult3(txt_REMIT_DT, ddl_SALARY_TYPE);

    //                    for (int i2 = 0; i2 < tmp3.Rows.Count; i2++)
    //                    {
    //                        if (tmp2.Rows[i]["EMP_ID"].ToString() == tmp3.Rows[i2]["EMP_ID"].ToString())
    //                        {
    //                            for (int i3 = 0; i3 < tmp.Rows.Count; i3++)
    //                            {
    //                                if (tmp3.Rows[i2]["SALARY_NAME"].ToString() == tmp.Rows[i3]["內容"].ToString())
    //                                {
    //                                    string S_N = i3.ToString();
    //                                    string S_ID = tmp.Rows[i3]["內容"].ToString();

    //                                    cell = row.CreateCell(i3);
    //                                    cell.CellStyle = style2;
    //                                    cell.SetCellValue(tmp3.Rows[i2]["AMOUNT"].ToString());
    //                                }
    //                            }



    //                        }



    //                    }


    //                }
    //            }
    //            else
    //            {

    //            }





    //            for (int i = 0; i < tmp.Rows.Count; i++)
    //            {
    //                sheet.AutoSizeColumn(i);
    //            }



    //            return workbook;
    //        }
    //        return null;
    //    }
    //    catch
    //    {
    //        throw;
    //    }
    //}
    //public IWorkbook createExcel2(string txt_REMIT_DT, string ddl_SALARY_TYPE)
    //{
    //    try
    //    {
    //        IWorkbook workbook;
    //        IWorkbook workbook2;
    //        IWorkbook workbook3;
    //        string type = "xlsx";
    //        ISheet sheet2;
    //        CFB2SD1300DAO wfb2sd = new CFB2SD1300DAO();
    //        ICellStyle style1;
    //        ICellStyle style2;
    //        ICellStyle style1_2;
    //        ICellStyle style1_3;


    //        DataTable tmp2 = wfb2sd.searchResult1_2(txt_REMIT_DT, ddl_SALARY_TYPE);

    //        if (tmp2.Rows.Count > 0)
    //        {
    //            if (type == "xls")
    //            {
    //                workbook = new HSSFWorkbook();
    //                workbook2 = new HSSFWorkbook();
    //                workbook3 = new HSSFWorkbook();

    //                sheet2 = (HSSFSheet)workbook.CreateSheet("一般");

    //                style1 = (HSSFCellStyle)workbook.CreateCellStyle();
    //                style1_2 = (HSSFCellStyle)workbook.CreateCellStyle();
    //                style1_3 = (HSSFCellStyle)workbook.CreateCellStyle();
    //            }
    //            else
    //            {
    //                workbook = new XSSFWorkbook();
    //                workbook2 = new XSSFWorkbook();
    //                workbook3 = new XSSFWorkbook();

    //                sheet2 = workbook.CreateSheet("一般");

    //                style1 = (XSSFCellStyle)workbook.CreateCellStyle();
    //                style1_2 = (XSSFCellStyle)workbook.CreateCellStyle();
    //                style1_3 = (XSSFCellStyle)workbook.CreateCellStyle();
    //            }
    //            IFont font1 = workbook.CreateFont();
    //            IFont font1_2 = workbook2.CreateFont();
    //            IFont font1_3 = workbook3.CreateFont();
    //            font1.FontName = "新細明體";
    //            font1.FontHeightInPoints = 12;
    //            style1.SetFont(font1);

    //            font1_2.FontName = "新細明體";
    //            font1_2.FontHeightInPoints = 12;
    //            style1_2.SetFont(font1_2);

    //            font1_3.FontName = "新細明體";
    //            font1_3.FontHeightInPoints = 12;
    //            style1_3.SetFont(font1_3);


    //            IRow row_2 = sheet2.CreateRow(0);
    //            ICell cell2;





    //            style1_3.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
    //            style1_3.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
    //            style1_3.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
    //            style1_3.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;

    //            style1.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
    //            style1.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
    //            style1.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
    //            style1.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;

    //            style1_2.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
    //            style1_2.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
    //            style1_2.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
    //            style1_2.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;


    //            cell2 = row_2.CreateCell(0);
    //            cell2.CellStyle = style1_2;
    //            cell2.SetCellValue("工號");





    //            cell2 = row_2.CreateCell(1);
    //            cell2.CellStyle = style1_2;
    //            cell2.SetCellValue("姓名");



    //            cell2 = row_2.CreateCell(2);
    //            cell2.CellStyle = style1_2;
    //            cell2.SetCellValue("金額");






    //            style1_3.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;

    //            //sheet3.CreateRow(3).CreateCell(7).CellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;



    //            style2 = workbook.CreateCellStyle();

    //            style2.SetFont(font1);
    //            style2.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
    //            style2.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
    //            style2.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
    //            style2.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
    //            style2.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;



    //            int xx = 0;
    //            for (int i = 0; i < tmp2.Rows.Count; i++)
    //            {
    //                xx = i + 1;
    //                row_2 = sheet2.CreateRow(xx);
    //                cell2 = row_2.CreateCell(0);
    //                cell2.CellStyle = style2;
    //                cell2.SetCellValue(tmp2.Rows[i]["EMP_ID"].ToString());

    //                cell2 = row_2.CreateCell(1);
    //                cell2.CellStyle = style2;
    //                cell2.SetCellValue(tmp2.Rows[i]["EMP_NAME"].ToString());


    //                cell2 = row_2.CreateCell(2);
    //                cell2.CellStyle = style2;
    //                cell2.SetCellValue(tmp2.Rows[i]["AMOUNT"].ToString());


    //            }




    //            sheet2.AutoSizeColumn(0);
    //            sheet2.AutoSizeColumn(1);
    //            sheet2.AutoSizeColumn(2);


    //            //ExcelHandle.exportExcel(workbook, "WFB2SD130." + type);
    //            return workbook;
    //        }
    //        return null;
    //    }
    //    catch
    //    {

    //        throw;
    //    }
    //}
    public IWorkbook createExcel3(string txt_REMIT_DT)//, string ddl_SALARY_TYPE
    {
        try
        {
            CFB2SD1300DAO wfb2sd = new CFB2SD1300DAO();
            IWorkbook workbook;
            //IWorkbook workbook2;
            //IWorkbook workbook3;
            //ISheet sheet;
            //ISheet sheet2;
            ISheet sheet3;
            ICellStyle style1;
            ICellStyle style2;
            ICellStyle style1_2;
            ICellStyle style1_3;
            ICellStyle style1_3_1;
            //string type = "xlsx";
            DataTable tabAA = wfb2sd.tabAA(txt_REMIT_DT);
            if (tabAA != null && tabAA.Rows.Count > 0)
            {
                txt_REMIT_DT = Convert.ToDateTime(tabAA.Rows[0]["SALARY_DT"].ToString()).ToString("yyyy/MM/dd");
                DataTable Btab = wfb2sd.Btab(txt_REMIT_DT); //ddl_SALARY_TYPE
                //sheet = workbook.CreateSheet("時金節金");
                //sheet2 = workbook.CreateSheet("先發金");
                if (Btab != null && Btab.Rows.Count > 0)
                {
                    DataTable tmp3 = wfb2sd.searchResult2_3(txt_REMIT_DT);
                    if (tmp3 != null && tmp3.Rows.Count > 0)
                    {
                        workbook = new XSSFWorkbook();
                        sheet3 = workbook.CreateSheet("期滿金");

                        int AMT = 0;

                        style1 = (XSSFCellStyle)workbook.CreateCellStyle();
                        style1_2 = (XSSFCellStyle)workbook.CreateCellStyle();
                        style1_3 = (XSSFCellStyle)workbook.CreateCellStyle();
                        style1_3_1 = (XSSFCellStyle)workbook.CreateCellStyle();

                        sheet3.CreateRow(0).CreateCell(27).SetCellValue("製表日期:");
                        sheet3.GetRow(0).CreateCell(28).SetCellValue(DateTime.Now.ToString("yyyy/MM/dd"));

                        IRow row_3_1 = sheet3.CreateRow(1);
                        IRow row_h_1 = sheet3.CreateRow(2);
                        IRow row_h_2 = sheet3.CreateRow(3);
                        IRow row_h_3 = sheet3.CreateRow(4);
                        IRow row_h_4 = sheet3.CreateRow(5);
                        IRow row_h_5 = sheet3.CreateRow(6);
                        IRow row_h_6 = sheet3.CreateRow(7);
                        IRow row_h_7 = sheet3.CreateRow(8);
                        IRow row_3_2 = sheet3.CreateRow(10);
                        IRow row_3 = sheet3.CreateRow(11);

                        ICell cell3;
                        //IFont font1 = workbook.CreateFont();
                        IFont font1 = workbook.CreateFont();

                        sheet3.SetColumnWidth(2, 20 * 256);
                        sheet3.SetColumnWidth(3, 40 * 256);
                        sheet3.SetColumnWidth(4, 200);
                        font1.FontName = "新細明體";
                        font1.FontHeightInPoints = 12;
                        style1.SetFont(font1);
                        style2 = workbook.CreateCellStyle();

                        style2.SetFont(font1);
                        style2.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                        style2.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                        style2.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                        style2.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                        style2.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                        style1_3_1.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                        style1_3.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                        style1_3.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                        style1_3.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                        style1_3.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                        sheet3.PrintSetup.FitHeight = 1;

                        sheet3.PrintSetup.FitWidth = 1;
                        sheet3.HorizontallyCenter = true;
                        sheet3.FitToPage = true;
                        // 調整為一頁
                        sheet3.PrintSetup.PaperSize = 9;
                        sheet3.PrintSetup.Landscape = true;
                        //sheet3.PrintSetup.Scale = 65;
                        sheet3.PrintSetup.UsePage = true;

                        cell3 = row_3_1.CreateCell(0);


                        cell3.CellStyle = style1_3_1;
                        cell3.SetCellValue("派遣籍期間社員契約期滿獎金明細-" + System.DateTime.Now.Year + "/" + System.DateTime.Now.Month);
                        sheet3.AddMergedRegion(new CellRangeAddress(1, 1, 0, 28));

                        sheet3.AddMergedRegion(new CellRangeAddress(2, 2, 1, 3));
                        cell3 = row_h_1.CreateCell(1);
                        cell3.CellStyle = style1;
                        cell3.SetCellValue("1. 匯款日期：" + txt_REMIT_DT);

                        sheet3.AddMergedRegion(new CellRangeAddress(3, 3, 1, 3));
                        cell3 = row_h_2.CreateCell(1);
                        cell3.CellStyle = style1;
                        cell3.SetCellValue("2. 契約期滿獎金計算標準如下：");

                        sheet3.AddMergedRegion(new CellRangeAddress(4, 4, 1, 5));
                        cell3 = row_h_3.CreateCell(1);
                        cell3.CellStyle = style1;
                        cell3.SetCellValue("①期滿獎金日數= ( 契約期間曆日數÷ 365日 ) × 30日 × 5個月");

                        sheet3.AddMergedRegion(new CellRangeAddress(5, 5, 1, 7));
                        cell3 = row_h_4.CreateCell(1);
                        cell3.CellStyle = style1;
                        cell3.SetCellValue("②事病假扣款日數 = -[ (事假H÷8H) ×2 + 病假H÷8H) × 0.5 )÷ 365日× 365日×30日 × 5個月");

                        sheet3.AddMergedRegion(new CellRangeAddress(6, 6, 1, 8));
                        cell3 = row_h_5.CreateCell(1);
                        cell3.CellStyle = style1;
                        cell3.SetCellValue("③曠工扣款日數= - ( 曠工日H ÷8H)× 5個月，曠工日扣款最多以 15日日薪為上限 )");

                        sheet3.AddMergedRegion(new CellRangeAddress(7, 7, 1, 7));
                        cell3 = row_h_6.CreateCell(1);
                        cell3.CellStyle = style1;
                        cell3.SetCellValue("④懲處扣款 = 月薪 ÷ 30日 × ( 申誡次數 × 1 + 小過次數 × 3+ 大過次數 × 10 )");

                        sheet3.AddMergedRegion(new CellRangeAddress(8, 8, 1, 7));
                        cell3 = row_h_7.CreateCell(1);
                        cell3.CellStyle = style1;
                        cell3.SetCellValue("⑤獎勵加給 = 月薪 ÷ 30日 × ( 嘉獎次數 × 1 + 記功次數 × 3 + 記大功次數 × 10 )");

                        sheet3.AddMergedRegion(new CellRangeAddress(10, 10, 7, 10));
                        cell3 = row_3_2.CreateCell(7);
                        cell3.CellStyle = style1_3;
                        cell3.SetCellValue("期滿獎金日數");
                        cell3 = row_3_2.CreateCell(8);
                        cell3.CellStyle = style1_3;
                        cell3 = row_3_2.CreateCell(9);
                        cell3.CellStyle = style1_3;
                        cell3 = row_3_2.CreateCell(10);
                        cell3.CellStyle = style1_3;


                        sheet3.AddMergedRegion(new CellRangeAddress(10, 10, 11, 13));
                        cell3 = row_3_2.CreateCell(11);
                        cell3.CellStyle = style1_3;
                        cell3.SetCellValue("事病假扣除日數");
                        cell3 = row_3_2.CreateCell(12);
                        cell3.CellStyle = style1_3;
                        cell3 = row_3_2.CreateCell(13);
                        cell3.CellStyle = style1_3;


                        sheet3.AddMergedRegion(new CellRangeAddress(10, 10, 14, 15));
                        cell3 = row_3_2.CreateCell(14);
                        cell3.CellStyle = style1_3;
                        cell3.SetCellValue("曠工扣除日數");
                        cell3 = row_3_2.CreateCell(15);
                        cell3.CellStyle = style1_3;


                        sheet3.AddMergedRegion(new CellRangeAddress(10, 10, 16, 22));
                        cell3 = row_3_2.CreateCell(16);
                        cell3.CellStyle = style1_3;
                        cell3.SetCellValue("獎懲");
                        cell3 = row_3_2.CreateCell(17);
                        cell3.CellStyle = style1_3;
                        cell3 = row_3_2.CreateCell(18);
                        cell3.CellStyle = style1_3;
                        cell3 = row_3_2.CreateCell(19);
                        cell3.CellStyle = style1_3;
                        cell3 = row_3_2.CreateCell(20);
                        cell3.CellStyle = style1_3;
                        cell3 = row_3_2.CreateCell(21);
                        cell3.CellStyle = style1_3;
                        cell3 = row_3_2.CreateCell(22);
                        cell3.CellStyle = style1_3;


                        sheet3.AddMergedRegion(new CellRangeAddress(10, 10, 23, 25));
                        cell3 = row_3_2.CreateCell(23);
                        cell3.CellStyle = style1_3;
                        cell3.SetCellValue("應發獎金 A");
                        cell3 = row_3_2.CreateCell(24);
                        cell3.CellStyle = style1_3;
                        cell3 = row_3_2.CreateCell(25);
                        cell3.CellStyle = style1_3;


                        sheet3.AddMergedRegion(new CellRangeAddress(10, 10, 26, 27));
                        cell3 = row_3_2.CreateCell(26);
                        cell3.CellStyle = style1_3;
                        cell3.SetCellValue("應發獎金 B");
                        cell3 = row_3_2.CreateCell(27);
                        cell3.CellStyle = style1_3;



                        cell3 = row_3_2.CreateCell(28);
                        cell3.CellStyle = style1_3;
                        cell3.SetCellValue("剩餘獎金");


                        style1_3.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;

                        //sheet3.CreateRow(3).CreateCell(7).CellStyle.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;

                        cell3 = row_3.CreateCell(0);
                        cell3.CellStyle = style1_3;
                        cell3.SetCellValue("No.");

                        cell3 = row_3.CreateCell(1);
                        cell3.CellStyle = style1_3;
                        cell3.SetCellValue("區分");

                        cell3 = row_3.CreateCell(2);
                        cell3.CellStyle = style1_3;
                        cell3.SetCellValue("部門CODE");

                        cell3 = row_3.CreateCell(3);
                        cell3.CellStyle = style1_3;
                        cell3.SetCellValue("課");

                        cell3 = row_3.CreateCell(4);
                        cell3.CellStyle = style1_3;
                        cell3.SetCellValue("工號");

                        cell3 = row_3.CreateCell(5);
                        cell3.CellStyle = style1_3;
                        cell3.SetCellValue("姓名");

                        cell3 = row_3.CreateCell(6);
                        cell3.CellStyle = style1_3;
                        cell3.SetCellValue("基本薪");

                        cell3 = row_3.CreateCell(7);
                        cell3.CellStyle = style1_3;
                        cell3.SetCellValue("契約起日");

                        cell3 = row_3.CreateCell(8);
                        cell3.CellStyle = style1_3;
                        cell3.SetCellValue("契約迄日");

                        cell3 = row_3.CreateCell(9);
                        cell3.CellStyle = style1_3;
                        cell3.SetCellValue("契約歷 日數");

                        cell3 = row_3.CreateCell(10);
                        cell3.CellStyle = style1_3;
                        cell3.SetCellValue("①");

                        cell3 = row_3.CreateCell(11);
                        cell3.CellStyle = style1_3;
                        cell3.SetCellValue("事假H");

                        cell3 = row_3.CreateCell(12);
                        cell3.CellStyle = style1_3;
                        cell3.SetCellValue("病假H");

                        cell3 = row_3.CreateCell(13);
                        cell3.CellStyle = style1_3;
                        cell3.SetCellValue("②");

                        cell3 = row_3.CreateCell(14);
                        cell3.CellStyle = style1_3;
                        cell3.SetCellValue("曠工H");

                        cell3 = row_3.CreateCell(15);
                        cell3.CellStyle = style1_3;
                        cell3.SetCellValue("③");

                        cell3 = row_3.CreateCell(16);
                        cell3.CellStyle = style1_3;
                        cell3.SetCellValue("嘉獎(+1日)");

                        cell3 = row_3.CreateCell(17);
                        cell3.CellStyle = style1_3;
                        cell3.SetCellValue("小功(+3日)");

                        cell3 = row_3.CreateCell(18);
                        cell3.CellStyle = style1_3;
                        cell3.SetCellValue("大功(+10日)");

                        cell3 = row_3.CreateCell(19);
                        cell3.CellStyle = style1_3;
                        cell3.SetCellValue("申誡(-1日)");

                        cell3 = row_3.CreateCell(20);
                        cell3.CellStyle = style1_3;
                        cell3.SetCellValue("小過(-3日)");

                        cell3 = row_3.CreateCell(21);
                        cell3.CellStyle = style1_3;
                        cell3.SetCellValue("大過(-10日)");

                        cell3 = row_3.CreateCell(22);
                        cell3.CellStyle = style1_3;
                        cell3.SetCellValue("日數 ④+⑤");

                        cell3 = row_3.CreateCell(23);
                        cell3.CellStyle = style1_3;
                        cell3.SetCellValue("日數①+②+③+④+⑤");

                        cell3 = row_3.CreateCell(24);
                        cell3.CellStyle = style1_3;
                        cell3.SetCellValue("(月數)");

                        cell3 = row_3.CreateCell(25);
                        cell3.CellStyle = style1_3;
                        cell3.SetCellValue("");

                        cell3 = row_3.CreateCell(26);
                        cell3.CellStyle = style1_3;
                        cell3.SetCellValue("先發次數");

                        cell3 = row_3.CreateCell(27);
                        cell3.CellStyle = style1_3;
                        cell3.SetCellValue("");

                        cell3 = row_3.CreateCell(28);
                        cell3.CellStyle = style1_3;
                        cell3.SetCellValue("A-B");

                        int x3 = 0;
                        for (int i = 0; i < tmp3.Rows.Count; i++)
                        {
                            x3 = i + 12;
                            row_3 = sheet3.CreateRow(x3);

                            cell3 = row_3.CreateCell(0);
                            cell3.CellStyle = style2;
                            cell3.SetCellValue(i + 1);

                            cell3 = row_3.CreateCell(1);
                            cell3.CellStyle = style2;
                            cell3.SetCellValue(tmp3.Rows[i]["HR_CHG_DESC"].ToString());

                            cell3 = row_3.CreateCell(2);
                            cell3.CellStyle = style2;
                            cell3.SetCellValue(tmp3.Rows[i]["ORI_DEPT_NO"].ToString());
                            cell3 = row_3.CreateCell(3);
                            cell3.CellStyle = style2;
                            cell3.SetCellValue(tmp3.Rows[i]["ORI_DIV_DEPT_FULL_NAME"].ToString());
                            cell3 = row_3.CreateCell(4);
                            cell3.CellStyle = style2;
                            cell3.SetCellValue(tmp3.Rows[i]["EMP_ID"].ToString());
                            cell3 = row_3.CreateCell(5);
                            cell3.CellStyle = style2;
                            cell3.SetCellValue(tmp3.Rows[i]["EMP_NAME"].ToString());
                            cell3 = row_3.CreateCell(6);
                            cell3.CellStyle = style2;
                            cell3.SetCellValue(tmp3.Rows[i]["BASIC_SALARY"].ToString());
                            cell3 = row_3.CreateCell(7);
                            cell3.CellStyle = style2;
                            cell3.SetCellValue(tmp3.Rows[i]["START_DT"].ToString());
                            cell3 = row_3.CreateCell(8);
                            cell3.CellStyle = style2;
                            cell3.SetCellValue(tmp3.Rows[i]["END_DT"].ToString());
                            cell3 = row_3.CreateCell(9);
                            cell3.CellStyle = style2;
                            cell3.SetCellValue(tmp3.Rows[i]["WORK_DAYS"].ToString());
                            cell3 = row_3.CreateCell(10);
                            cell3.CellStyle = style2;
                            cell3.SetCellValue(Convert.ToDecimal(tmp3.Rows[i]["BOUNS_WORK_DAYS"].ToString()).ToString("N1"));
                            cell3 = row_3.CreateCell(11);
                            cell3.CellStyle = style2;
                            cell3.SetCellValue(tmp3.Rows[i]["LEAVE_A_HRS"].ToString());
                            cell3 = row_3.CreateCell(12);
                            cell3.CellStyle = style2;
                            cell3.SetCellValue(tmp3.Rows[i]["LEAVE_B_HRS"].ToString());
                            cell3 = row_3.CreateCell(13);
                            cell3.CellStyle = style2;
                            cell3.SetCellValue(Convert.ToDecimal(tmp3.Rows[i]["LEAVE_B_DAYS"].ToString()).ToString("N1"));
                            cell3 = row_3.CreateCell(14);
                            cell3.CellStyle = style2;
                            cell3.SetCellValue(tmp3.Rows[i]["LEAVE_Q_HRS"].ToString());
                            cell3 = row_3.CreateCell(15);
                            cell3.CellStyle = style2;
                            cell3.SetCellValue(Convert.ToDecimal(tmp3.Rows[i]["LEAVE_Q_DAYS"].ToString()).ToString("N1"));
                            cell3 = row_3.CreateCell(16);
                            cell3.CellStyle = style2;
                            cell3.SetCellValue(tmp3.Rows[i]["THIRD_CNT_REWARD"].ToString());
                            cell3 = row_3.CreateCell(17);
                            cell3.CellStyle = style2;
                            cell3.SetCellValue(tmp3.Rows[i]["FIRST_CNT_REWARD"].ToString());
                            cell3 = row_3.CreateCell(18);
                            cell3.CellStyle = style2;
                            cell3.SetCellValue(tmp3.Rows[i]["SECOND_CNT_REWARD"].ToString());
                            cell3 = row_3.CreateCell(19);
                            cell3.CellStyle = style2;
                            cell3.SetCellValue(tmp3.Rows[i]["THIRD_CNT_PUNISH"].ToString());
                            cell3 = row_3.CreateCell(20);
                            cell3.CellStyle = style2;
                            cell3.SetCellValue(tmp3.Rows[i]["SECOND_CNT_PUNISH"].ToString());
                            cell3 = row_3.CreateCell(21);
                            cell3.CellStyle = style2;
                            cell3.SetCellValue(tmp3.Rows[i]["FIRST_CNT_PUNISH"].ToString());
                            cell3 = row_3.CreateCell(22);
                            cell3.CellStyle = style2;
                            cell3.SetCellValue(tmp3.Rows[i]["JUDGEMENT_DAYS"].ToString());
                            cell3 = row_3.CreateCell(23);
                            cell3.CellStyle = style2;
                            cell3.SetCellValue(Convert.ToDecimal(tmp3.Rows[i]["PLAN_BONUS_DAYS"].ToString()).ToString("N1"));
                            cell3 = row_3.CreateCell(24);
                            cell3.CellStyle = style2;
                            cell3.SetCellValue(Convert.ToDecimal(tmp3.Rows[i]["PLAN_BONUS_DAYS_2"].ToString()).ToString("N1"));
                            cell3 = row_3.CreateCell(25);
                            cell3.CellStyle = style2;
                            cell3.SetCellValue(Convert.ToInt32(tmp3.Rows[i]["PLAN_BONUS_AMT"].ToString()).ToString("N0"));
                            cell3 = row_3.CreateCell(26);
                            cell3.CellStyle = style2;
                            cell3.SetCellValue(tmp3.Rows[i]["PAID_CNT"].ToString());
                            cell3 = row_3.CreateCell(27);
                            cell3.CellStyle = style2;
                            cell3.SetCellValue(tmp3.Rows[i]["PAID_AMT"].ToString());
                            cell3 = row_3.CreateCell(28);
                            cell3.CellStyle = style2;
                            cell3.SetCellValue(Convert.ToInt32(tmp3.Rows[i]["BONUS_AMT"].ToString()).ToString("N0"));

                            AMT = AMT + Convert.ToInt32(tmp3.Rows[i]["BONUS_AMT"].ToString());

                            sheet3.AutoSizeColumn(3);
                            sheet3.AutoSizeColumn(4);
                            sheet3.AutoSizeColumn(5);
                            sheet3.AutoSizeColumn(6);
                            sheet3.AutoSizeColumn(7);
                            sheet3.AutoSizeColumn(8);
                            sheet3.AutoSizeColumn(9);
                            sheet3.AutoSizeColumn(10);
                            sheet3.AutoSizeColumn(11);
                            sheet3.AutoSizeColumn(12);
                            sheet3.AutoSizeColumn(13);
                            sheet3.AutoSizeColumn(14);
                            sheet3.AutoSizeColumn(15);
                            sheet3.AutoSizeColumn(16);
                            sheet3.AutoSizeColumn(17);
                            sheet3.AutoSizeColumn(18);
                            sheet3.AutoSizeColumn(19);
                            sheet3.AutoSizeColumn(20);
                            sheet3.AutoSizeColumn(21);
                            sheet3.AutoSizeColumn(22);
                            sheet3.AutoSizeColumn(23);
                            sheet3.AutoSizeColumn(24);
                            sheet3.AutoSizeColumn(25);
                            sheet3.AutoSizeColumn(26);
                            sheet3.AutoSizeColumn(27);
                        }
                        x3 = x3 + 1;
                        row_3 = sheet3.CreateRow(x3);
                        sheet3.AddMergedRegion(new CellRangeAddress(x3, x3, 4, 5));
                        cell3 = row_3.CreateCell(4);
                        cell3.CellStyle = style1;
                        cell3.SetCellValue("合計人數：" + tmp3.Rows.Count.ToString("N0") + "人");
                        cell3 = row_3.CreateCell(27);
                        cell3.CellStyle = style1_3_1;
                        cell3.SetCellValue("合計：");
                        cell3 = row_3.CreateCell(28);
                        cell3.CellStyle = style1_3_1;
                        cell3.SetCellValue(AMT.ToString("N0"));

                        return workbook;
                    }
                }
            }
            return null;
        }
        catch
        {
            throw;
        }
    }




}
