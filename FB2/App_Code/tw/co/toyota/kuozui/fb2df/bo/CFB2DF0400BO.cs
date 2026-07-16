using FB2.tw.co.toyota.kuozui.bo;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;

/// <summary>
/// CFB2DF0400BO 的摘要描述
/// </summary>
public class CFB2DF0400BO : BaseService
{
	public CFB2DF0400BO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    public DataTable searchData(CFB2DF0400DAO dao)
    {
        try
        {
            return dao.selectMainData();
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable checkData(CFB2DF0400DAO dao)
    {
        try
        {
            return dao.checkData();
        }
        catch (Exception)
        {

            throw;
        }
    }

    public IWorkbook createExcel(CFB2DF0400DAO dao, string type)
    {
        try
        {
            IWorkbook workbook;
            ISheet sheet;
            ICellStyle style1;

            dao.deleteRECORD();

            dao.insertRECORD();
                        
            DataTable dt = dao.selectData();

            if (dt.Rows.Count > 0)
            {
                if (type == "xls")
                {
                    workbook = new HSSFWorkbook();
                    sheet = (HSSFSheet)workbook.CreateSheet("住宿人員資料下載");
                    style1 = (HSSFCellStyle)workbook.CreateCellStyle();
                }
                else
                {
                    workbook = new XSSFWorkbook();
                    sheet = workbook.CreateSheet("住宿人員資料下載");
                    style1 = (XSSFCellStyle)workbook.CreateCellStyle();
                }

                IFont font1 = workbook.CreateFont();
                font1.FontName = "新細明體";
                font1.FontHeightInPoints = 12;
                style1.SetFont(font1);

                IRow row = sheet.CreateRow(1);
                ICell cell;
                cell = row.CreateCell(0);
                cell.CellStyle = style1;
                cell.SetCellValue("工號");

                cell = row.CreateCell(1);
                cell.CellStyle = style1;
                cell.SetCellValue("姓名");

                cell = row.CreateCell(2);
                cell.CellStyle = style1;
                cell.SetCellValue("身份");

                cell = row.CreateCell(3);
                cell.CellStyle = style1;
                cell.SetCellValue("部門代號");

                cell = row.CreateCell(4);
                cell.CellStyle = style1;
                cell.SetCellValue("部門名稱");

                cell = row.CreateCell(5);
                cell.CellStyle = style1;
                cell.SetCellValue("直別");

                cell = row.CreateCell(6);
                cell.CellStyle = style1;
                cell.SetCellValue("有效卡號檔");

                cell = row.CreateCell(7);
                cell.CellStyle = style1;
                cell.SetCellValue("宿舍號碼");

                cell = row.CreateCell(8);
                cell.CellStyle = style1;
                cell.SetCellValue("住宿日");

                cell = row.CreateCell(9);
                cell.CellStyle = style1;
                cell.SetCellValue("退宿日");

                cell = row.CreateCell(10);
                cell.CellStyle = style1;
                cell.SetCellValue("汽車");

                cell = row.CreateCell(11);
                cell.CellStyle = style1;
                cell.SetCellValue("機車");

                cell = row.CreateCell(12);
                cell.CellStyle = style1;
                cell.SetCellValue("汽車牌照");

                cell = row.CreateCell(13);
                cell.CellStyle = style1;
                cell.SetCellValue("機車牌照");

                cell = row.CreateCell(14);
                cell.CellStyle = style1;
                cell.SetCellValue("生日");

                cell = row.CreateCell(15);
                cell.CellStyle = style1;
                cell.SetCellValue("離職日期");

                //製表日期
                row = sheet.CreateRow(0);
                cell = row.CreateCell(152);
                cell.CellStyle = style1;
                cell.SetCellValue("製表日期:");

                cell = row.CreateCell(15);
                cell.CellStyle = style1;
                cell.SetCellValue(DateTime.Now.ToString("yyyy/MM/dd"));


                int x = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //dao.EMP_ID = dt.Rows[i]["EMP_ID"].ToString();
                    //dao.EMP_NAME = dt.Rows[i]["EMP_NAME"].ToString();
                    //dao.EMP_CD = dt.Rows[i]["EMP_CD"].ToString();
                    //dao.DEPT_NO = dt.Rows[i]["DEPT_NO"].ToString();
                    //dao.CLASS_NAME = dt.Rows[i]["CLASS_NAME"].ToString();
                    //dao.POTO = dt.Rows[i]["POTO"].ToString();
                    //dao.CARD_NO = dt.Rows[i]["CARD_NO"].ToString();
                    //dao.ROOM_NO = dt.Rows[i]["ROOM_NO"].ToString();
                   /*
                    if (dt.Rows[i]["START_DT"].ToString() == "9999-12-31")
                    {
                        dao.START_DT = DateTime.Parse(dt.Rows[i]["START_DT"].ToString()).ToString("yyyymMMdd");
                        //dao.START_DT = "9991231";
                    }
                    else {

                        dao.START_DT = chtdate(dt.Rows[i]["START_DT"].ToString());
                    }

                    if (dt.Rows[i]["END_DT"].ToString() == "9999-12-31")
                    {
                        dao.START_DT = DateTime.Parse(dt.Rows[i]["START_DT"].ToString()).ToString("yyyymMMdd");
                        //dao.END_DT = "9991231";
                    }
                    else
                    {

                        dao.END_DT = chtdate(dt.Rows[i]["END_DT"].ToString());
                    }
                    */
                    if (dt.Rows[i]["BIRTH_DT"].ToString() != "")
                    {
                        dao.BIRTH_DT = chtdate(dt.Rows[i]["BIRTH_DT"].ToString());
                    }
                    else
                    {
                        dao.BIRTH_DT = "";
                    }
                    /*
                    string tt = dt.Rows[i]["LEAVE_DT"].ToString();
                    if (dt.Rows[i]["LEAVE_DT"].ToString() != "")
                    {
                        dao.LEAVE_DT = chtdate(dt.Rows[i]["LEAVE_DT"].ToString());
                    }
                    else
                    {
                        dao.LEAVE_DT = "";
                    }
                    */

                    //dao.CAR = dt.Rows[i]["CAR"].ToString();
                    //dao.MOTOR = dt.Rows[i]["MOTOR"].ToString();
                    //dao.CAR_NO = dt.Rows[i]["CAR_NO"].ToString();
                    //dao.MOTOR_NO = dt.Rows[i]["MOTOR_NO"].ToString();

                    x = i + 2;
                    row = sheet.CreateRow(x);
                    cell = row.CreateCell(0);
                    cell.CellStyle = style1;
                    cell.SetCellValue(dt.Rows[i]["EMP_ID"].ToString());

                    cell = row.CreateCell(1);
                    cell.CellStyle = style1;
                    cell.SetCellValue(dt.Rows[i]["EMP_NAME"].ToString());


                    cell = row.CreateCell(2);
                    cell.CellStyle = style1;
                    cell.SetCellValue(dt.Rows[i]["EMP_CD"].ToString());

                    cell = row.CreateCell(3);
                    cell.CellStyle = style1;
                    cell.SetCellValue(dt.Rows[i]["DEPT_NO"].ToString());

                    cell = row.CreateCell(4);
                    cell.CellStyle = style1;
                    cell.SetCellValue(dt.Rows[i]["CLASS_NAME"].ToString());

                    cell = row.CreateCell(5);
                    cell.CellStyle = style1;
                    cell.SetCellValue(dt.Rows[i]["POTO"].ToString());

                    cell = row.CreateCell(6);
                    cell.CellStyle = style1;
                    cell.SetCellValue(dt.Rows[i]["CARD_NO"].ToString());

                    cell = row.CreateCell(7);
                    cell.CellStyle = style1;
                    cell.SetCellValue(dt.Rows[i]["ROOM_NO"].ToString());

                    cell = row.CreateCell(8);
                    cell.CellStyle = style1;
                    cell.SetCellValue(dt.Rows[i]["START_DT_AD"].ToString());

                    cell = row.CreateCell(9);
                    cell.CellStyle = style1;
                    cell.SetCellValue(dt.Rows[i]["END_DT_AD"].ToString());

                    cell = row.CreateCell(10);
                    cell.CellStyle = style1;
                    cell.SetCellValue(dt.Rows[i]["CAR"].ToString());

                    cell = row.CreateCell(11);
                    cell.CellStyle = style1;
                    cell.SetCellValue(dt.Rows[i]["MOTOR"].ToString());

                    cell = row.CreateCell(12);
                    cell.CellStyle = style1;
                    cell.SetCellValue(dt.Rows[i]["CAR_NO"].ToString());

                    cell = row.CreateCell(13);
                    cell.CellStyle = style1;
                    cell.SetCellValue(dt.Rows[i]["MOTOR_NO"].ToString());

                    cell = row.CreateCell(14);
                    cell.CellStyle = style1;
                    cell.SetCellValue(dt.Rows[i]["BIRTH_DT_AD"].ToString());

                    cell = row.CreateCell(15);
                    cell.CellStyle = style1;
                    cell.SetCellValue(dt.Rows[i]["LEAVE_DT_AD"].ToString());

                }//for end

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
                sheet.AutoSizeColumn(12);
                sheet.AutoSizeColumn(13);
                sheet.AutoSizeColumn(14);
                sheet.AutoSizeColumn(15);
                //ExcelHandle.exportExcel(workbook, "FB2DF040_EMP." + type);
                return workbook;
            }
            return null;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public string chtdate(string str)   
    {
        TaiwanCalendar twC = new TaiwanCalendar();
        String st = DateTime.Parse(str).ToString("yyyy");
        string st1 = DateTime.Parse(str).ToString("MMdd");        
        string tdate = Convert.ToString(Convert.ToString(Convert.ToInt32(st)-1911))+ st1; 
        return tdate;   
    }

    public void createTxt(CFB2DF0400DAO dao)
    {
        try
        {
            dao.deleteRECORD();

            dao.insertRECORD();


            MemoryStream ms = null;
            TextWriter tw = null;
            ms = new MemoryStream();
            tw = new StreamWriter(ms);


            DataTable dt = dao.selectData();

            if (dt.Rows.Count > 0)
            {               
                
                for (int i = 0; i < dt.Rows.Count; i++)
                {

                    //dao.EMP_ID = dt.Rows[i]["EMP_ID"].ToString();
                    dao.EMP_NAME = dt.Rows[i]["EMP_NAME"].ToString();
                    dao.EMP_CD = dt.Rows[i]["EMP_CD"].ToString();
                    //dao.DEPT_NO = dt.Rows[i]["DEPT_NO"].ToString();
                    //dao.CLASS_NAME = dt.Rows[i]["CLASS_NAME"].ToString();
                    //dao.POTO = dt.Rows[i]["POTO"].ToString();
                    //dao.CARD_NO = dt.Rows[i]["CARD_NO"].ToString();
                    //dao.ROOM_NO = dt.Rows[i]["ROOM_NO"].ToString();

                    
                    if (dt.Rows[i]["START_DT"].ToString() == "9999-12-31")
                    {
                        //dao.START_DT = DateTime.Parse(dt.Rows[i]["START_DT"].ToString()).ToString("yyyymMMdd");
                        dao.START_DT = "9991231";
                    }
                    else
                    {

                        dao.START_DT = chtdate(dt.Rows[i]["START_DT"].ToString());
                    }

                    if (dt.Rows[i]["END_DT"].ToString() == "9999-12-31")
                    {
                        dao.END_DT = "9991231";
                        
                    }
                    else
                    {
                        if (DateTime.Parse(dt.Rows[i]["END_DT"].ToString()) >= DateTime.Now)
                            dao.END_DT = "9991231";
                        else
                            dao.END_DT = chtdate(dt.Rows[i]["END_DT"].ToString());
                    }
                    
                    //dao.CAR = dt.Rows[i]["CAR"].ToString();
                    //dao.MOTOR = dt.Rows[i]["MOTOR"].ToString();
                    //dao.CAR_NO = dt.Rows[i]["CAR_NO"].ToString();
                    //dao.MOTOR_NO = dt.Rows[i]["MOTOR_NO"].ToString();
                    string b1 = "";
                    string b2 = "";
                    string b3 = "";
                    string b4 = "";
                    string b5 = "";
                    string b6 = "";
                    string b7 = "";
                    string b8 = "";
                    string b9 = "";
                    string b10 = "";

                    for(int j = 0 ; j < ( 16 - 2 * (dt.Rows[i]["EMP_NAME"].ToString().Trim().Length));j++){
                        b1 += " "; 
                    }
                    for (int j = 0; j < (30 - 2 * (dt.Rows[i]["CLASS_NAME"].ToString().Trim().Length));j++)
                    {
                        b2 += " ";
                    }
                    for (int j = 0; j < (5 - dt.Rows[i]["ACCOM_BUILD_CD"].ToString().Trim().Length - dt.Rows[i]["ROOM_NO"].ToString().Trim().Length); j++)
                    {
                        b3 += " ";
                    }
                    for (int j = 0; j < (7 - dao.START_DT.Length); j++)
                    {
                        b10 += " ";
                    }
                    for (int j = 0; j < (6 - dao.END_DT.Length); j++)
                    {
                        b4 += " ";
                    }
                    for (int j = 0; j < (11 - dt.Rows[i]["CAR_NO"].ToString().Trim().Length); j++)
                    {
                        b5 += " ";
                    }
                    for (int j = 0; j < (8 - dt.Rows[i]["CARD_NO"].ToString().Trim().Length); j++)
                    {
                        b6 += " ";
                    }
                    for (int j = 0; j < (1 - dt.Rows[i]["CAR"].ToString().Trim().Length); j++)
                    {
                        b7 += " ";
                    }
                    for (int j = 0; j < (1 - dt.Rows[i]["MOTOR"].ToString().Trim().Length); j++)
                    {
                        b8 += " ";
                    }
                    for (int j = 0; j < (8 - dt.Rows[i]["CAR_NO"].ToString().Trim().Replace("-","").Length); j++)
                    {
                        b9 += " ";
                    }




                    string s1 = dt.Rows[i]["CAR_NO"].ToString().Trim();
                    string s2 = dt.Rows[i]["CARD_NO"].ToString().Trim();                   
                    string s4 = dt.Rows[i]["ROOM_NO"].ToString().Trim();

                    string s3 = dt.Rows[i]["CAR"].ToString().Trim();

                    //舊的格式  20150702 陽鼎要改規格
                    //tw.WriteLine(
                    //        dt.Rows[i]["EMP_ID"].ToString().Trim() + " " +
                    //        dt.Rows[i]["EMP_NAME"].ToString().Trim() + b1 +
                    //        dt.Rows[i]["EMP_CD"].ToString() +
                    //        dt.Rows[i]["DEPT_NO"].ToString() +
                    //        dt.Rows[i]["CLASS_NAME"].ToString().Trim() + b2 +
                    //        dt.Rows[i]["POTO"].ToString().Trim() +
                    //        dt.Rows[i]["CARD_NO"].ToString().Trim() + dt.Rows[i]["ACCOM_BUILD_CD"].ToString().Trim() + b6 +
                    //        dt.Rows[i]["ROOM_NO"].ToString().Trim() + b3 + b10 +
                    //        dao.START_DT + " " + b4 +
                    //        dao.END_DT +
                    //        dt.Rows[i]["CAR"].ToString().Trim() + b7 +
                    //        dt.Rows[i]["MOTOR"].ToString().Trim() + b8 +
                    //        dt.Rows[i]["CAR_NO"].ToString().Trim().Replace("-", "") + b9 +
                    //        dt.Rows[i]["MOTOR_NO"].ToString().Trim().Replace("-", ""));
                        
                    //}

                    //改為用逗號隔開各個欄位
                   tw.WriteLine(
                            dt.Rows[i]["EMP_ID"].ToString().Trim() + "," +
                            dt.Rows[i]["EMP_NAME"].ToString().Trim() + "," +
                            dt.Rows[i]["EMP_CD"].ToString() + "," +
                            dt.Rows[i]["DEPT_NO"].ToString() + "," +
                            dt.Rows[i]["CLASS_NAME"].ToString().Trim() + "," +
                            dt.Rows[i]["POTO"].ToString().Trim() + "," +
                            dt.Rows[i]["CARD_NO"].ToString().Trim() + "," + dt.Rows[i]["ACCOM_BUILD_CD"].ToString().Trim() + "," +
                            dt.Rows[i]["ROOM_NO"].ToString().Trim() + "," +
                            dao.START_DT + "," +
                            dao.END_DT + "," +
                            dt.Rows[i]["CAR"].ToString().Trim() + "," +
                            dt.Rows[i]["MOTOR"].ToString().Trim() + "," +
                            dt.Rows[i]["CAR_NO"].ToString().Trim().Replace("-", "") + "," +
                            dt.Rows[i]["MOTOR_NO"].ToString().Trim().Replace("-", ""));
                        
                    }
                    //產生文字檔下載
                    string fileName = "FB2DF040_EMP.txt";
                    tw.Flush();
                    System.Web.HttpContext.Current.Response.Clear();
                    System.Web.HttpContext.Current.Response.ClearHeaders();
                    System.Web.HttpContext.Current.Response.ClearContent();
                    System.Web.HttpContext.Current.Response.HeaderEncoding = System.Text.Encoding.GetEncoding("big5");
                    //System.Web.HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml";
                    System.Web.HttpContext.Current.Response.ContentType = "application/octet-stream";
                    System.Web.HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + HttpUtility.UrlEncode(fileName));
                    System.Web.HttpContext.Current.Response.BinaryWrite(ms.ToArray());
                    System.Web.HttpContext.Current.Response.Buffer = false;
                    ms.Close();
                    ms.Dispose();
                    System.Web.HttpContext.Current.Response.End();

                                
            }

        }
        catch (Exception)
        {
            throw;
        }
    }

}