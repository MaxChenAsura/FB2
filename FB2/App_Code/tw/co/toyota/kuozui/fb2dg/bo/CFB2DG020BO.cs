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
/// CFB2DG020BO 的摘要描述
/// </summary>
public class CFB2DG020BO : BaseService
{
	public CFB2DG020BO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}
    public DataTable get_PDF_Data2()
    {
        DataTable retVal = new DataTable(); ;
        CFB2DG020DAO fb2dg = new CFB2DG020DAO();
        try
        {
            retVal = fb2dg.get_PDF_Data2();
            return retVal;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable get_PDF_Data3()
    {
        DataTable retVal = new DataTable(); ;
        CFB2DG020DAO fb2dg = new CFB2DG020DAO();
        try
        {
            retVal = fb2dg.get_PDF_Data3();
            return retVal;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable get_PDF_Data4(string txt_UPDATED_DT_S,string txt_UPDATED_DT_E)
    {
        DataTable retVal = new DataTable(); ;
        CFB2DG020DAO fb2dg = new CFB2DG020DAO();
        try
        {
            retVal = fb2dg.get_PDF_Data4(txt_UPDATED_DT_S, txt_UPDATED_DT_E);
            return retVal;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public System.Data.DataTable getJPN_CD()
    {
        CFB2DG020DAO wfb2dg = new CFB2DG020DAO();
        try
        {
            return wfb2dg.getJPN_CD();
        }
        catch (Exception)
        {

            throw;
        }
    }
    public string deleteData(string login_emp_id)
    {
        CFB2DG020DAO wfb2dg = new CFB2DG020DAO();
        try
        {
            BeginTransaction();

            wfb2dg.deleteData(login_emp_id);
            Commit();

            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }


    }
    public string addData(CFB2DG020DAO wfb2dg)
    {
        try
        {
            BeginTransaction();
            wfb2dg.addData();
            Commit();
            return "0";

        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    public byte[] createExcel(CFB2DG020DAO wfb2dg, string type)
    {
        try
        {

            IWorkbook workbook;
            ISheet sheet;
          
            ICellStyle style1;
            ICellStyle style1_1;
            ICellStyle style2;
            ICellStyle style3;
            int mRows = 0;
            DataTable tmp = wfb2dg.searchResult();
            if (tmp.Rows.Count > 0)
            {
                if (type == "xls")
                {
                    workbook = new HSSFWorkbook();
                    sheet = (HSSFSheet)workbook.CreateSheet("停車格發放月度統計表");
                    style1 = (HSSFCellStyle)workbook.CreateCellStyle();
                    style1_1 = (HSSFCellStyle)workbook.CreateCellStyle();
                    style2 = (HSSFCellStyle)workbook.CreateCellStyle();
                    style3 = (HSSFCellStyle)workbook.CreateCellStyle();
                }
                else
                {
                    workbook = new XSSFWorkbook();
                    sheet = workbook.CreateSheet("停車格發放月度統計表");
                    style1 = (XSSFCellStyle)workbook.CreateCellStyle();
                    style1_1 = (XSSFCellStyle)workbook.CreateCellStyle();
                    style2 = (XSSFCellStyle)workbook.CreateCellStyle();
                    style3 = (XSSFCellStyle)workbook.CreateCellStyle();
                }

                IFont font1 = workbook.CreateFont();
                font1.FontName = "新細明體";
                font1.FontHeightInPoints = 12;
                IFont font1_1 = workbook.CreateFont();
                font1_1.FontName = "新細明體";
                font1_1.FontHeightInPoints = 7;

                style1.SetFont(font1);
                style1_1.SetFont(font1_1);

                style2.SetFont(font1);

                style2.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                style2.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                style2.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                style2.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                style1.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                style1.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                style1.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                style1.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;

                style1_1.BorderBottom = NPOI.SS.UserModel.BorderStyle.Thin;
                style1_1.BorderLeft = NPOI.SS.UserModel.BorderStyle.Thin;
                style1_1.BorderRight = NPOI.SS.UserModel.BorderStyle.Thin;
                style1_1.BorderTop = NPOI.SS.UserModel.BorderStyle.Thin;
                style1_1.WrapText = true;
                style1_1.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;

                style3.Alignment = HorizontalAlignment.Center;
                style3.VerticalAlignment = VerticalAlignment.Center;
                style3.SetFont(font1);

                IRow row = sheet.CreateRow(0);
                IRow rowD;
                IRow row2 = sheet.CreateRow(5);
                IRow row3 = sheet.CreateRow(6);
                ICell cell;
                ICell cell2;
                ICell cell3;
                style1.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                sheet.AddMergedRegion(new CellRangeAddress(0, 0, 1, 15));
                cell = row.CreateCell(1);
                cell.CellStyle = style1;
                cell.SetCellValue("中壢廠&觀音廠停車格發放月度統計表");
                style1.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                for (int i = 0; i <= 0; i++)
                {
                    IRow row1 = sheet.GetRow(i);
                    for (int j = 1 + 1; j <= 15; j++)
                    {
                        ICell cell1 = row.CreateCell(j);
                        cell1.CellStyle = style1;
                    }
                }

                cell = row.CreateCell(1);
                cell.CellStyle = style1;
                cell.SetCellValue("中壢廠&觀音廠停車格發放月度統計表");

                style2.WrapText = true;

                style2.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
                style1.FillForegroundColor = 56;
                IRow row12 = sheet.CreateRow(4);

                IRow row4 = sheet.CreateRow(7);
                IRow row5 = sheet.CreateRow(8);
                IRow row6 = sheet.CreateRow(9);
                IRow row7 = sheet.CreateRow(10);
                IRow row8 = sheet.CreateRow(11);
                IRow row9 = sheet.CreateRow(12);
                IRow row10 = sheet.CreateRow(13);
                IRow row11 = sheet.CreateRow(14);

                sheet.AddMergedRegion(new CellRangeAddress(5, 5, 1, 5));
                cell2 = row2.CreateCell(1);
                cell2.CellStyle = style2;
                cell2.SetCellValue("汽車停車場");
                cell2 = row2.CreateCell(2);
                cell2.CellStyle = style2;
                cell2 = row2.CreateCell(3);
                cell2.CellStyle = style2;
                cell2 = row2.CreateCell(4);
                cell2.CellStyle = style2;
                cell2 = row2.CreateCell(5);
                cell2.CellStyle = style2;

                int par1 = 0;
                int par2 = 0;
                int n1 = 0;
                int n2 = 0;
                int r1 = 0;
                int r2 = 0;
                int y1 = 0;
                int y2 = 0;
                double d1 = 0;
                double d2 = 0;
                double d = 0;
                double e1 = 0;
                double e2 = 0;

                int n = 0;
                int x = 0;
                for (int i = 0; i < tmp.Rows.Count; i++)
                {
                    x = i + 6;
                    int P1_N = Convert.ToInt32(tmp.Rows[0]["中壢汽車"].ToString());
                    int P2_N = Convert.ToInt32(tmp.Rows[0]["觀音汽車"].ToString());
                    mRows = 5 + P1_N + P1_N + 2;//最大欄位
                    //int P1_N = Convert.ToInt32(tmp.Rows[0]["中壢廠"].ToString());
                    //int P2_N = Convert.ToInt32(tmp.Rows[0]["觀音廠"].ToString());

                    //製表日期
                    //rowD
                    rowD = sheet.CreateRow(3);
                    cell = rowD.CreateCell(5 + P1_N + P2_N+2 - 1);
                    cell.CellStyle = style3;
                    cell.SetCellValue("製表日期:");

                    cell = rowD.CreateCell(5 + P1_N + P2_N + 2);
                    cell.CellStyle = style3;
                    cell.SetCellValue(DateTime.Now.ToString("yyyy/MM/dd"));


                    if (n == 0 && i == P1_N)
                    {
                        cell2 = row2.CreateCell(x);
                        cell2.CellStyle = style2;
                        cell2.SetCellValue("合計");
                        cell2 = row3.CreateCell(x);
                        cell2.CellStyle = style2;


                        cell2 = row3.CreateCell(x);
                        cell2.CellStyle = style2;
                        cell2.SetCellValue(par1);
                        cell2 = row4.CreateCell(x);
                        cell2.CellStyle = style2;
                        cell2.SetCellValue(n1);
                        cell2 = row5.CreateCell(x);
                        cell2.CellStyle = style2;
                        cell2.SetCellValue(r1);
                        cell2 = row6.CreateCell(x);
                        cell2.CellStyle = style2;
                        cell2.SetCellValue(y1);
                        cell2 = row7.CreateCell(x);
                        cell2.CellStyle = style2;
                        cell2.SetCellValue(n1 + r1 + y1);
                        cell2 = row8.CreateCell(x);
                        cell2.CellStyle = style2;
                        cell2.SetCellValue(Math.Floor(par1 - n1 * 0.95));
                        cell2 = row9.CreateCell(x);
                        cell2.CellStyle = style2;
                        cell2.SetCellValue(Math.Floor(d1));
                        cell2 = row10.CreateCell(x);
                        cell2.CellStyle = style2;
                        cell2.SetCellValue(Math.Floor(e1));
                        cell2 = row11.CreateCell(x);
                        cell2.CellStyle = style2;
                        cell2.SetCellValue("-");
                        n = 1;
                        i = i - 1;
                        x = x + 1;
                    }
                    else
                    {
                        cell2 = row2.CreateCell(x + n);
                        cell2.CellStyle = style2;
                        cell2.SetCellValue(tmp.Rows[i]["CAR_PARK_NO"].ToString());
                        cell2 = row3.CreateCell(x + n);
                        cell2.CellStyle = style2;
                        cell2.SetCellValue(tmp.Rows[i]["PARKING_SPOT"].ToString());
                        cell2 = row4.CreateCell(x + n);
                        cell2.CellStyle = style2;
                        cell2.SetCellValue(tmp.Rows[i]["常日"].ToString());
                        cell2 = row5.CreateCell(x + n);
                        cell2.CellStyle = style2;
                        cell2.SetCellValue(tmp.Rows[i]["紅直"].ToString());
                        cell2 = row6.CreateCell(x + n);
                        cell2.CellStyle = style2;
                        cell2.SetCellValue(tmp.Rows[i]["黃直"].ToString());
                        cell2 = row7.CreateCell(x + n);
                        cell2.CellStyle = style2;
                        cell2.SetCellValue(Convert.ToInt32(tmp.Rows[i]["常日"].ToString()) + Convert.ToInt32(tmp.Rows[i]["紅直"].ToString()) + Convert.ToInt32(tmp.Rows[i]["黃直"].ToString()));
                        cell2 = row8.CreateCell(x + n);
                        cell2.CellStyle = style2;
                        cell2.SetCellValue(Math.Floor(Convert.ToInt32(tmp.Rows[i]["PARKING_SPOT"].ToString()) - Convert.ToInt32(tmp.Rows[i]["常日"].ToString()) * 0.95));

                        if (Convert.ToInt32(tmp.Rows[i]["紅直"].ToString()) > Convert.ToInt32(tmp.Rows[i]["黃直"].ToString()))
                        {
                            cell2 = row9.CreateCell(x + n);
                            cell2.CellStyle = style2;
                            cell2.SetCellValue(Math.Floor((Convert.ToInt32(tmp.Rows[i]["紅直"].ToString()) * 1.4 + Convert.ToInt32(tmp.Rows[i]["常日"].ToString())) * 0.95));
                            d = (Convert.ToInt32(tmp.Rows[i]["紅直"].ToString()) * 1.4 + Convert.ToInt32(tmp.Rows[i]["常日"].ToString())) * 0.95;
                        }
                        else
                        {
                            cell2 = row9.CreateCell(x + n);
                            cell2.CellStyle = style2;
                            cell2.SetCellValue(Math.Floor((Convert.ToInt32(tmp.Rows[i]["黃直"].ToString()) * 1.4 + Convert.ToInt32(tmp.Rows[i]["常日"].ToString())) * 0.95));
                            d = (Convert.ToInt32(tmp.Rows[i]["黃直"].ToString()) * 1.4 + Convert.ToInt32(tmp.Rows[i]["常日"].ToString())) * 0.95;
                        }
                        cell2 = row10.CreateCell(x + n);
                        cell2.CellStyle = style2;
                        cell2.SetCellValue(Math.Floor(Convert.ToInt32(tmp.Rows[i]["PARKING_SPOT"].ToString()) - d));

                        cell2 = row11.CreateCell(x + n);
                        cell2.CellStyle = style2;
                        if (d != 0)
                        {
                            double ddd = ((Math.Floor(Convert.ToInt32(tmp.Rows[i]["PARKING_SPOT"].ToString()) - Convert.ToInt32(tmp.Rows[i]["常日"].ToString()) * 0.95)) - d) / d;                            
                            string st = String.Format("{0:0.00%}", ddd);
                            cell2.SetCellValue(st);
                        }
                        else
                        {
                            cell2.SetCellValue("-");
                        }

                        if (tmp.Rows[i]["SUB_DESC"].ToString() == "中壢廠")
                        {
                            par1 = par1 + Convert.ToInt32(tmp.Rows[i]["PARKING_SPOT"].ToString());
                            n1 = n1 + Convert.ToInt32(tmp.Rows[i]["常日"].ToString());
                            r1 = r1 + Convert.ToInt32(tmp.Rows[i]["紅直"].ToString());
                            y1 = y1 + Convert.ToInt32(tmp.Rows[i]["黃直"].ToString());
                            d1 = d1 + d;
                            e1 = par1 - d1;


                        }
                        else
                        {

                            par2 = par2 + Convert.ToInt32(tmp.Rows[i]["PARKING_SPOT"].ToString());
                            n2 = n2 + Convert.ToInt32(tmp.Rows[i]["常日"].ToString());
                            r2 = r2 + Convert.ToInt32(tmp.Rows[i]["紅直"].ToString());
                            y2 = y2 + Convert.ToInt32(tmp.Rows[i]["黃直"].ToString());
                            d2 = d2 + d;
                            e2 = par2 - d2;
                        }


                    }

                    if (i == tmp.Rows.Count - 1)
                    {

                        cell2 = row2.CreateCell(x + n + 1);
                        cell2.CellStyle = style2;
                        cell2.SetCellValue("合計");
                        cell2 = row3.CreateCell(x + n + 1);
                        cell2.CellStyle = style2;
                        cell2.SetCellValue(par2);
                        cell2 = row4.CreateCell(x + n + 1);
                        cell2.CellStyle = style2;
                        cell2.SetCellValue(n2);
                        cell2 = row5.CreateCell(x + n + 1);
                        cell2.CellStyle = style2;
                        cell2.SetCellValue(r2);
                        cell2 = row6.CreateCell(x + n + 1);
                        cell2.CellStyle = style2;
                        cell2.SetCellValue(y2);
                        cell2 = row7.CreateCell(x + n + 1);
                        cell2.CellStyle = style2;
                        cell2.SetCellValue(n2 + r2 + y2);
                        cell2 = row8.CreateCell(x + n + 1);
                        cell2.CellStyle = style2;
                        cell2.SetCellValue(Math.Floor(par2 - n2 * 0.95));
                        cell2 = row9.CreateCell(x + n + 1);
                        cell2.CellStyle = style2;
                        cell2.SetCellValue(Math.Floor(d2));
                        cell2 = row10.CreateCell(x + n + 1);
                        cell2.CellStyle = style2;
                        cell2.SetCellValue(Math.Floor(e2));
                        cell2 = row11.CreateCell(x + n + 1);
                        cell2.CellStyle = style2;
                        cell2.SetCellValue("-");
                    }


                }

                int p1 = Convert.ToInt32(tmp.Rows[0]["中壢汽車"].ToString());
                //int p1 = Convert.ToInt32(tmp.Rows[0]["中壢廠"].ToString());
                ICell cell13;
                sheet.AddMergedRegion(new CellRangeAddress(4, 4, 6, 6 + p1));


                for (int i = 0; i <= p1; i++)
                {
                    cell13 = row12.CreateCell(6);
                    cell13.CellStyle = style2;
                    cell13.SetCellValue("中壢廠");
                    cell13 = row12.CreateCell(6 + i);
                    cell13.CellStyle = style2;

                }
                int p2 = Convert.ToInt32(tmp.Rows[0]["觀音汽車"].ToString());
                //int p2 = Convert.ToInt32(tmp.Rows[0]["觀音廠"].ToString());
                ICell cell14;
                sheet.AddMergedRegion(new CellRangeAddress(4, 4, 6 + p1 + 1, 6 + p1 + p2 + 1));

                for (int i = 0; i <= p2; i++)
                {
                    cell14 = row12.CreateCell(6 + p1 + 1);
                    cell14.CellStyle = style2;
                    cell14.SetCellValue("觀音廠");
                    cell14 = row12.CreateCell(6 + p1 + 1 + i);
                    cell14.CellStyle = style2;

                }
                sheet.AddMergedRegion(new CellRangeAddress(6, 14, 1, 1));

                cell3 = row3.CreateCell(1);
                cell3.CellStyle = style2;
                cell3.SetCellValue("汽車");
                cell3 = row3.CreateCell(2);
                cell3.CellStyle = style1;
                cell3.SetCellValue("停車格");
                //cell3.Sheet.GetRow(12).HeightInPoints = 2 * sheet.DefaultRowHeight / 15;
                cell3 = row3.CreateCell(5);
                cell3.CellStyle = style1;
                cell3.SetCellValue("A");
                cell3 = row4.CreateCell(5);
                cell3.CellStyle = style1;
                cell3.SetCellValue("X");
                cell3 = row5.CreateCell(5);
                cell3.CellStyle = style1;
                cell3.SetCellValue("Y1");
                cell3 = row6.CreateCell(5);
                cell3.CellStyle = style1;
                cell3.SetCellValue("Y2");
                cell3 = row7.CreateCell(5);
                cell3.CellStyle = style1;
                

                cell3.SetCellValue("B=X+Y1+Y2");

                cell3 = row8.CreateCell(5);
                cell3.CellStyle = style1;
                cell3.SetCellValue("C=A-X*95%");
                cell3 = row9.CreateCell(5);
                cell3.CellStyle = style1_1;
                cell3.SetCellValue("D={Y1(1+40%)+X}*95% \n Y1或Y2取大數");
                cell3 = row10.CreateCell(5);
                cell3.CellStyle = style1;
                cell3.SetCellValue("E=A-D");
                cell3 = row11.CreateCell(5);
                cell3.CellStyle = style1_1;
               
                cell3.SetCellValue("F=(C-Y1*0.95)/Y1*0.95");
                cell3.Sheet.AutoSizeColumn(5);
                

                cell3 = row4.CreateCell(1);
                cell3.CellStyle = style2;
                cell3 = row5.CreateCell(1);
                cell3.CellStyle = style2;
                cell3 = row6.CreateCell(1);
                cell3.CellStyle = style2;
                cell3 = row7.CreateCell(1);
                cell3.CellStyle = style2;
                cell3 = row8.CreateCell(1);
                cell3.CellStyle = style2;
                cell3 = row9.CreateCell(1);
                cell3.CellStyle = style2;
                cell3 = row10.CreateCell(1);
                cell3.CellStyle = style2;
                cell3 = row11.CreateCell(1);
                cell3.CellStyle = style2;









                ICell cell4;
                sheet.AddMergedRegion(new CellRangeAddress(7, 10, 2, 2));
                ICell cell5;
                sheet.AddMergedRegion(new CellRangeAddress(7, 7, 3, 4));
                ICell cell6;
                sheet.AddMergedRegion(new CellRangeAddress(8, 8, 3, 4));
                ICell cell7;
                sheet.AddMergedRegion(new CellRangeAddress(9, 9, 3, 4));
                ICell cell8;
                sheet.AddMergedRegion(new CellRangeAddress(10, 10, 3, 4));
                ICell cell9;
                sheet.AddMergedRegion(new CellRangeAddress(11, 11, 2, 4));
                ICell cell10;
                sheet.AddMergedRegion(new CellRangeAddress(12, 12, 2, 4));
                ICell cell11;
                sheet.AddMergedRegion(new CellRangeAddress(13, 13, 2, 4));
                ICell cell12;
                sheet.AddMergedRegion(new CellRangeAddress(14, 14, 2, 4));


                cell4 = row4.CreateCell(2);
                cell4.CellStyle = style2;
                cell4.SetCellValue("停車格發放數(12月末)");

                cell5 = row4.CreateCell(3);
                cell5.CellStyle = style2;
                cell5.SetCellValue("常日");
                cell5 = row4.CreateCell(4);
                cell5.CellStyle = style2;
                cell6 = row5.CreateCell(3);
                cell6.CellStyle = style2;
                cell6.SetCellValue("紅直");
                cell6 = row5.CreateCell(4);
                cell6.CellStyle = style2;
                cell7 = row6.CreateCell(3);
                cell7.CellStyle = style2;
                cell7.SetCellValue("黃直");
                cell7 = row6.CreateCell(4);
                cell7.CellStyle = style2;
                cell8 = row7.CreateCell(3);
                cell8.CellStyle = style2;
                cell8.SetCellValue("計");
                cell8 = row7.CreateCell(4);
                cell8.CellStyle = style2;
                cell9 = row8.CreateCell(2);
                cell9.CellStyle = style2;
                cell9.SetCellValue("扣除常日停車格後停車格");
                cell9 = row8.CreateCell(3);
                cell9.CellStyle = style2;
                cell9 = row8.CreateCell(4);
                cell9.CellStyle = style2;
                cell10 = row9.CreateCell(2);
                cell10.CellStyle = style2;
                cell10.SetCellValue("必要停車格(重疊率40%)");
                cell10 = row9.CreateCell(3);
                cell10.CellStyle = style2;
                cell10 = row9.CreateCell(4);
                cell10.CellStyle = style2;
                cell11 = row10.CreateCell(2);
                cell11.CellStyle = style2;
                cell11.SetCellValue("停車格過不足(重疊率40%)");
                cell11 = row10.CreateCell(3);
                cell11.CellStyle = style2;
                cell11 = row10.CreateCell(4);
                cell11.CellStyle = style2;
                cell12 = row11.CreateCell(2);
                cell12.CellStyle = style2;
                cell12.SetCellValue("1月末可允許重疊率");
                cell12 = row11.CreateCell(3);
                cell12.CellStyle = style2;
                cell12 = row11.CreateCell(4);
                cell12.CellStyle = style2;

                ICell cell2_2;
                ICell cell3_2;
                IRow row_2 = sheet.CreateRow(0 + 15);
                IRow row2_2 = sheet.CreateRow(5 + 15);
                IRow row3_2 = sheet.CreateRow(6 + 15);

                IRow row12_2 = sheet.CreateRow(4 + 15);

                IRow row4_2 = sheet.CreateRow(7 + 15);
                IRow row5_2 = sheet.CreateRow(8 + 15);
                IRow row6_2 = sheet.CreateRow(9 + 15);
                IRow row7_2 = sheet.CreateRow(10 + 15);
                IRow row8_2 = sheet.CreateRow(26);
                IRow row9_2 = sheet.CreateRow(12 + 15);
                IRow row10_2 = sheet.CreateRow(13 + 15);
                IRow row11_2 = sheet.CreateRow(14 + 15);


                DataTable tmp2 = wfb2dg.searchResult2();
                int p1_2 = Convert.ToInt32(tmp2.Rows[0]["中壢機車"].ToString());
                //int p1_2 = Convert.ToInt32(tmp2.Rows[0]["中壢廠"].ToString());
                ICell cell13_2;
                sheet.AddMergedRegion(new CellRangeAddress(19, 19, 6, 6 + p1_2));
                for (int i = 0; i <= p1_2; i++)
                {
                    cell13_2 = row12_2.CreateCell(6);
                    cell13_2.CellStyle = style2;
                    cell13_2.SetCellValue("中壢廠");
                    cell13_2 = row12_2.CreateCell(6 + i);
                    cell13_2.CellStyle = style2;

                }

                int p2_2 = Convert.ToInt32(tmp2.Rows[0]["觀音機車"].ToString());
                //int p2_2 = Convert.ToInt32(tmp2.Rows[0]["觀音廠"].ToString());
                ICell cell14_2;
                sheet.AddMergedRegion(new CellRangeAddress(19, 19, 6 + p1_2 + 1, 6 + p1_2 + p2_2 + 1));
                for (int i = 0; i <= p2_2; i++)
                {
                    cell14_2 = row12_2.CreateCell(6 + p1_2 + 1);
                    cell14_2.CellStyle = style2;
                    cell14_2.SetCellValue("觀音廠");
                    cell14_2 = row12_2.CreateCell(6 + p1_2 + 1 + i);
                    cell14_2.CellStyle = style2;

                }


                sheet.AddMergedRegion(new CellRangeAddress(20, 20, 1, 5));
                cell2_2 = row2_2.CreateCell(1);
                cell2_2.CellStyle = style2;
                cell2_2.SetCellValue("機車停車場");
                cell2_2 = row2_2.CreateCell(2);
                cell2_2.CellStyle = style2;
                cell2_2 = row2_2.CreateCell(3);
                cell2_2.CellStyle = style2;
                cell2_2 = row2_2.CreateCell(4);
                cell2_2.CellStyle = style2;
                cell2_2 = row2_2.CreateCell(5);
                cell2_2.CellStyle = style2;
                int par1_2 = 0;
                int par2_2 = 0;
                int n1_2 = 0;
                int n2_2 = 0;
                int r1_2 = 0;
                int r2_2 = 0;
                int y1_2 = 0;
                int y2_2 = 0;
                double d1_2 = 0;
                double d2_2 = 0;
                double d_2 = 0;
                double e1_2 = 0;
                double e2_2 = 0;

                int n_2 = 0;
                int x_2 = 0;
                for (int i = 0; i < tmp2.Rows.Count; i++)
                {
                    x_2 = i + 6;

                    int P1_N = Convert.ToInt32(tmp2.Rows[0]["中壢機車"].ToString());
                    int P2_N = Convert.ToInt32(tmp2.Rows[0]["觀音機車"].ToString());

                    //int P1_N = Convert.ToInt32(tmp2.Rows[0]["中壢廠"].ToString());
                    //int P2_N = Convert.ToInt32(tmp2.Rows[0]["觀音廠"].ToString());
                    if (n_2 == 0 && i == P1_N)
                    {
                        cell2_2 = row2_2.CreateCell(x_2);
                        cell2_2.CellStyle = style2;
                        cell2_2.SetCellValue("合計");
                        cell2_2 = row3_2.CreateCell(x_2);
                        cell2_2.CellStyle = style2;


                        cell2_2 = row3_2.CreateCell(x_2);
                        cell2_2.CellStyle = style2;
                        cell2_2.SetCellValue(par1_2);
                        cell2_2 = row4_2.CreateCell(x_2);
                        cell2_2.CellStyle = style2;
                        cell2_2.SetCellValue(n1_2);
                        cell2_2 = row5_2.CreateCell(x_2);
                        cell2_2.CellStyle = style2;
                        cell2_2.SetCellValue(r1_2);
                        cell2_2 = row6_2.CreateCell(x_2);
                        cell2_2.CellStyle = style2;
                        cell2_2.SetCellValue(y1_2);
                        cell2_2 = row7_2.CreateCell(x_2);
                        cell2_2.CellStyle = style2;
                        cell2_2.SetCellValue(n1_2 + r1_2 + y1_2);
                        cell2_2 = row8_2.CreateCell(x_2);
                        cell2_2.CellStyle = style2;
                        cell2_2.SetCellValue(Math.Floor(par1_2 - n1_2 * 0.95));
                        cell2_2 = row9_2.CreateCell(x_2);
                        cell2_2.CellStyle = style2;
                        cell2_2.SetCellValue(Math.Floor(d1_2));
                        cell2_2 = row10_2.CreateCell(x_2);
                        cell2_2.CellStyle = style2;
                        cell2_2.SetCellValue(Math.Floor(e1_2));
                        cell2_2 = row11_2.CreateCell(x_2);
                        cell2_2.CellStyle = style2;
                        cell2_2.SetCellValue("-");
                        n_2 = 1;
                        i = i - 1;
                        x_2 = x_2 + 1;
                    }
                    else
                    {
                        cell2_2 = row2_2.CreateCell(x_2 + n_2);
                        cell2_2.CellStyle = style2;
                        cell2_2.SetCellValue(tmp2.Rows[i]["CAR_PARK_NO"].ToString());
                        cell2_2 = row3_2.CreateCell(x_2 + n_2);
                        cell2_2.CellStyle = style2;
                        cell2_2.SetCellValue(tmp2.Rows[i]["PARKING_SPOT"].ToString());
                        cell2_2 = row4_2.CreateCell(x_2 + n_2);
                        cell2_2.CellStyle = style2;
                        cell2_2.SetCellValue(tmp2.Rows[i]["常日"].ToString());
                        cell2_2 = row5_2.CreateCell(x_2 + n_2);
                        cell2_2.CellStyle = style2;
                        cell2_2.SetCellValue(tmp2.Rows[i]["紅直"].ToString());
                        cell2_2 = row6_2.CreateCell(x_2 + n_2);
                        cell2_2.CellStyle = style2;
                        cell2_2.SetCellValue(tmp2.Rows[i]["黃直"].ToString());
                        cell2_2 = row7_2.CreateCell(x_2 + n_2);
                        cell2_2.CellStyle = style2;
                        cell2_2.SetCellValue(Convert.ToInt32(tmp2.Rows[i]["常日"].ToString()) + Convert.ToInt32(tmp2.Rows[i]["紅直"].ToString()) + Convert.ToInt32(tmp2.Rows[i]["黃直"].ToString()));
                        cell2_2 = row8_2.CreateCell(x_2 + n_2);
                        cell2_2.CellStyle = style2;
                        cell2_2.SetCellValue(Math.Floor(Convert.ToInt32(tmp2.Rows[i]["PARKING_SPOT"].ToString()) - Convert.ToInt32(tmp2.Rows[i]["常日"].ToString()) * 0.95));

                        if (Convert.ToInt32(tmp2.Rows[i]["紅直"].ToString()) > Convert.ToInt32(tmp2.Rows[i]["黃直"].ToString()))
                        {
                            cell2_2 = row9_2.CreateCell(x_2 + n_2);
                            cell2_2.CellStyle = style2;
                            cell2_2.SetCellValue(Math.Floor((Convert.ToInt32(tmp2.Rows[i]["紅直"].ToString()) * 1.4 + Convert.ToInt32(tmp2.Rows[i]["常日"].ToString())) * 0.95));
                            d_2 = (Convert.ToInt32(tmp2.Rows[i]["紅直"].ToString()) * 1.4 + Convert.ToInt32(tmp2.Rows[i]["常日"].ToString())) * 0.95;
                        }
                        else
                        {
                            cell2_2 = row9_2.CreateCell(x_2 + n_2);
                            cell2_2.CellStyle = style2;
                            cell2_2.SetCellValue(Math.Floor((Convert.ToInt32(tmp2.Rows[i]["黃直"].ToString()) * 1.4 + Convert.ToInt32(tmp2.Rows[i]["常日"].ToString())) * 0.95));
                            d_2 = (Convert.ToInt32(tmp2.Rows[i]["黃直"].ToString()) * 1.4 + Convert.ToInt32(tmp2.Rows[i]["常日"].ToString())) * 0.95;
                        }
                        cell2_2 = row10_2.CreateCell(x_2 + n_2);
                        cell2_2.CellStyle = style2;
                        cell2_2.SetCellValue(Math.Floor(Convert.ToInt32(tmp2.Rows[i]["PARKING_SPOT"].ToString()) - d));
                        cell2_2 = row11_2.CreateCell(x_2 + n_2);
                        cell2_2.CellStyle = style2;
                        if (d_2 != 0)
                        {                            
                            double gsdd = ((Math.Floor(Convert.ToInt32(tmp2.Rows[i]["PARKING_SPOT"].ToString()) - Convert.ToInt32(tmp2.Rows[i]["常日"].ToString()) * 0.95)) - d_2) / d_2;
                            string st = String.Format("{0:0.00%}", gsdd);
                            cell2_2.SetCellValue(st);
                        }
                        else
                        {
                            cell2_2.SetCellValue("-");
                        }

                        if (tmp2.Rows[i]["SUB_DESC"].ToString() == "中壢廠")
                        {
                            par1_2 = par1_2 + Convert.ToInt32(tmp2.Rows[i]["PARKING_SPOT"].ToString());
                            n1_2 = n1_2 + Convert.ToInt32(tmp2.Rows[i]["常日"].ToString());
                            r1_2 = r1_2 + Convert.ToInt32(tmp2.Rows[i]["紅直"].ToString());
                            y1_2 = y1_2 + Convert.ToInt32(tmp2.Rows[i]["黃直"].ToString());
                            d1_2 = d1_2 + d_2;
                            e1_2 = par1_2 - d1_2;


                        }
                        else
                        {

                            par2_2 = par2_2 + Convert.ToInt32(tmp2.Rows[i]["PARKING_SPOT"].ToString());
                            n2_2 = n2_2 + Convert.ToInt32(tmp2.Rows[i]["常日"].ToString());
                            r2_2 = r2_2 + Convert.ToInt32(tmp2.Rows[i]["紅直"].ToString());
                            y2_2 = y2_2 + Convert.ToInt32(tmp2.Rows[i]["黃直"].ToString());
                            d2_2 = d2_2 + d_2;
                            e2_2 = par2_2 - d2_2;
                        }


                    }

                    if (i == tmp2.Rows.Count - 1)
                    {

                        cell2_2 = row2_2.CreateCell(x_2 + n_2 + 1);
                        cell2_2.CellStyle = style2;
                        cell2_2.SetCellValue("合計");
                        cell2_2 = row3_2.CreateCell(x_2 + n_2 + 1);
                        cell2_2.CellStyle = style2;
                        cell2_2.SetCellValue(par2_2);
                        cell2_2 = row4_2.CreateCell(x_2 + n_2 + 1);
                        cell2_2.CellStyle = style2;
                        cell2_2.SetCellValue(n2_2);
                        cell2_2 = row5_2.CreateCell(x_2 + n_2 + 1);
                        cell2_2.CellStyle = style2;
                        cell2_2.SetCellValue(r2_2);
                        cell2_2 = row6_2.CreateCell(x_2 + n_2 + 1);
                        cell2_2.CellStyle = style2;
                        cell2_2.SetCellValue(y2_2);
                        cell2_2 = row7_2.CreateCell(x_2 + n_2 + 1);
                        cell2_2.CellStyle = style2;
                        cell2_2.SetCellValue(n2_2 + r2_2 + y2_2);
                        cell2_2 = row8_2.CreateCell(x_2 + n_2 + 1);
                        cell2_2.CellStyle = style2;
                        cell2_2.SetCellValue(Math.Floor(par2_2 - n2_2 * 0.95));
                        cell2_2 = row9_2.CreateCell(x_2 + n_2 + 1);
                        cell2_2.CellStyle = style2;
                        cell2_2.SetCellValue(Math.Floor(d2_2));
                        cell2_2 = row10_2.CreateCell(x_2 + n_2 + 1);
                        cell2_2.CellStyle = style2;
                        cell2_2.SetCellValue(Math.Floor(e2_2));
                        cell2_2 = row11_2.CreateCell(x_2 + n_2 + 1);
                        cell2_2.CellStyle = style2;
                        cell2_2.SetCellValue("-");
                    }


                }






                sheet.AddMergedRegion(new CellRangeAddress(6 + 15, 14 + 15, 1, 1));

                cell3_2 = row3_2.CreateCell(1);
                cell3_2.CellStyle = style2;
                cell3_2.SetCellValue("機車");

                cell3_2 = row3_2.CreateCell(2);
                cell3_2.CellStyle = style1;
                cell3_2.SetCellValue("停車格");

                cell3_2 = row3_2.CreateCell(5);
                cell3_2.CellStyle = style1;
                cell3_2.SetCellValue("A");
                cell3_2 = row4_2.CreateCell(5);
                cell3_2.CellStyle = style1;
                cell3_2.SetCellValue("X");
                cell3_2 = row5_2.CreateCell(5);
                cell3_2.CellStyle = style1;
                cell3_2.SetCellValue("Y1");
                cell3_2 = row6_2.CreateCell(5);
                cell3_2.CellStyle = style1;
                cell3_2.SetCellValue("Y2");
                cell3_2 = row7_2.CreateCell(5);
                cell3_2.CellStyle = style1;
                cell3_2.SetCellValue("B=X+Y1+Y2");
                cell3_2 = row8_2.CreateCell(5);
                cell3_2.CellStyle = style1;
                cell3_2.SetCellValue("C=A-X*95%");
                cell3_2 = row9_2.CreateCell(5);
                cell3_2.CellStyle = style1_1;
                cell3_2.SetCellValue("D={Y1(1+40%)+X}*95% \n Y1或Y2取大數");
                cell3_2 = row10_2.CreateCell(5);
                cell3_2.CellStyle = style1;
                cell3_2.SetCellValue("E=A-D");
                cell3_2 = row11_2.CreateCell(5);
                cell3_2.CellStyle = style1_1;

                cell3.SetCellValue("F=(C-Y1*0.95)/Y1*0.95");
                cell3.Sheet.AutoSizeColumn(5);



                cell3_2 = row4_2.CreateCell(1);
                cell3_2.CellStyle = style2;
                cell3_2 = row5_2.CreateCell(1);
                cell3_2.CellStyle = style2;
                cell3_2 = row6_2.CreateCell(1);
                cell3_2.CellStyle = style2;
                cell3_2 = row7_2.CreateCell(1);
                cell3_2.CellStyle = style2;
                cell3_2 = row8_2.CreateCell(1);
                cell3_2.CellStyle = style2;
                cell3_2 = row9_2.CreateCell(1);
                cell3_2.CellStyle = style2;
                cell3_2 = row10_2.CreateCell(1);
                cell3_2.CellStyle = style2;
                cell3_2 = row11_2.CreateCell(1);
                cell3_2.CellStyle = style2;





                ICell cell4_2;
                sheet.AddMergedRegion(new CellRangeAddress(7 + 15, 10 + 15, 2, 2));
                ICell cell5_2;
                sheet.AddMergedRegion(new CellRangeAddress(7 + 15, 7 + 15, 3, 4));
                ICell cell6_2;
                sheet.AddMergedRegion(new CellRangeAddress(8 + 15, 8 + 15, 3, 4));
                ICell cell7_2;
                sheet.AddMergedRegion(new CellRangeAddress(9 + 15, 9 + 15, 3, 4));
                ICell cell8_2;
                sheet.AddMergedRegion(new CellRangeAddress(10 + 15, 10 + 15, 3, 4));
                ICell cell9_2;
                sheet.AddMergedRegion(new CellRangeAddress(11 + 15, 11 + 15, 2, 4));
                ICell cell10_2;
                sheet.AddMergedRegion(new CellRangeAddress(12 + 15, 12 + 15, 2, 4));
                ICell cell11_2;
                sheet.AddMergedRegion(new CellRangeAddress(13 + 15, 13 + 15, 2, 4));
                ICell cell12_2;
                sheet.AddMergedRegion(new CellRangeAddress(14 + 15, 14 + 15, 2, 4));


                cell4_2 = row4_2.CreateCell(2);
                cell4_2.CellStyle = style2;
                cell4_2.SetCellValue("停車格發放數(12月末)");
                cell4_2 = row4_2.CreateCell(3);
                cell4_2.CellStyle = style2;
                cell4_2 = row4_2.CreateCell(4);
                cell4_2.CellStyle = style2;
                cell5_2 = row4_2.CreateCell(3);
                cell5_2.CellStyle = style2;
                cell5_2.SetCellValue("常日");
                cell5_2 = row4_2.CreateCell(4);
                cell5_2.CellStyle = style2;

                cell6_2 = row5_2.CreateCell(3);
                cell6_2.CellStyle = style2;
                cell6_2.SetCellValue("紅直");
                cell6_2 = row5_2.CreateCell(4);
                cell6_2.CellStyle = style2;
                cell7_2 = row6_2.CreateCell(3);
                cell7_2.CellStyle = style2;
                cell7_2.SetCellValue("黃直");
                cell7_2 = row6_2.CreateCell(4);
                cell7_2.CellStyle = style2;
                cell8_2 = row7_2.CreateCell(3);
                cell8_2.CellStyle = style2;
                cell8_2.SetCellValue("計");
                cell8_2 = row7_2.CreateCell(4);
                cell8_2.CellStyle = style2;
                cell9_2 = row8_2.CreateCell(2);
                cell9_2.CellStyle = style2;
                cell9_2.SetCellValue("扣除常日停車格後停車格");
                cell9_2 = row8_2.CreateCell(3);
                cell9_2.CellStyle = style2;
                cell9_2 = row8_2.CreateCell(4);
                cell9_2.CellStyle = style2;
                //cell9_2 = row8_2.CreateCell(5);
                //cell9_2.CellStyle = style2;
                cell10_2 = row9_2.CreateCell(2);
                cell10_2.CellStyle = style2;
                cell10_2.SetCellValue("必要停車格(重疊率40%)");
                cell10_2 = row9_2.CreateCell(3);
                cell10_2.CellStyle = style2;
                cell10_2 = row9_2.CreateCell(4);
                cell10_2.CellStyle = style2;
                //cell10_2 = row9_2.CreateCell(5);
                //cell10_2.CellStyle = style2;
                cell11_2 = row10_2.CreateCell(2);
                cell11_2.CellStyle = style2;
                cell11_2.SetCellValue("停車格過不足(重疊率40%)");
                cell11_2 = row10_2.CreateCell(3);
                cell11_2.CellStyle = style2;
                cell11_2 = row10_2.CreateCell(4);
                cell11_2.CellStyle = style2;
                //cell11_2 = row10_2.CreateCell(5);
                //cell11_2.CellStyle = style2;
                cell12_2 = row11_2.CreateCell(2);
                cell12_2.CellStyle = style2;
                cell12_2.SetCellValue("1月末可允許重疊率");
                cell12_2 = row11_2.CreateCell(3);
                cell12_2.CellStyle = style2;
                cell12_2 = row11_2.CreateCell(4);
                cell12_2.CellStyle = style2;
                //cell12_2 = row11_2.CreateCell(5);
                //cell12_2.CellStyle = style2;








                style2 = workbook.CreateCellStyle();

                style2.SetFont(font1);

                for (int i = 0; i < mRows; i++)
                {
                    sheet.AutoSizeColumn(i);
                }
                //sheet.AutoSizeColumn(0);
                //sheet.AutoSizeColumn(1);
                //sheet.AutoSizeColumn(2);
                //ExcelHandle.exportExcel(workbook, "FB2DG020Excel_1." + type);
                MemoryStream ms = new MemoryStream();
                workbook.Write(ms);
                return ms.ToArray();
            }
            else
            {
                return null;
            }
        }
        catch
        {
            throw;
        }
    }







}
