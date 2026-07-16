using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
/// <summary>
/// CFB2SE220BO 的摘要描述
/// </summary>
public class CFB2SE220BO : BaseService
{
	public CFB2SE220BO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}
    //public DataTable get_PDF_Data()
    //{
    //    DataTable retVal = new DataTable(); ;
    //    CFB2SE220DAO fb2sc = new CFB2SE220DAO();
    //    try
    //    {
    //        retVal = fb2sc.get_PDF_Data();
    //        return retVal;
    //    }
    //    catch (Exception)
    //    {
    //        throw;
    //    }
    //}
    public System.Data.DataTable getTemp1(string txt_EFFECT_YM,string txt_EMP_ID)
    {
        CFB2SE2200DAO wfb2se = new CFB2SE2200DAO();
        try
        {
            return wfb2se.getTemp1(txt_EFFECT_YM, txt_EMP_ID);
        }
        catch (Exception)
        {

            throw;
        }
    }
    public System.Data.DataTable getNot_ADJ(string txt_EFFECT_YM)
    {
        CFB2SE2200DAO wfb2se = new CFB2SE2200DAO();
        try
        {
            return wfb2se.getNot_ADJ(txt_EFFECT_YM);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public System.Data.DataTable getTemp2(string M_EMP_ID)
    {
        CFB2SE2200DAO wfb2se = new CFB2SE2200DAO();
        try
        {
            return wfb2se.getTemp2(M_EMP_ID);
        }
        catch (Exception)
        {

            throw;
        }
    }
    public System.Data.DataTable getTempCHK1(string EFFECT_YM,string EMP_ID)
    {
        CFB2SE2200DAO wfb2se = new CFB2SE2200DAO();
        try
        {
            return wfb2se.getTempCHK1(EFFECT_YM, EMP_ID);
        }
        catch (Exception)
        {

            throw;
        }
    }
    //public System.Data.DataTable getTempA(string txt_EMP_ID, string txt_EFFECT_YM)
    //{
    //    CFB2SE2200DAO wfb2se = new CFB2SE2200DAO();
    //    try
    //    {
    //        return wfb2se.getTempA(txt_EMP_ID, txt_EFFECT_YM);
    //    }
    //    catch (Exception)
    //    {

    //        throw;
    //    }
    //}

    public string excute(CFB2SE2200DAO dao)
    {
        DataTable dt = new DataTable();        
        try
        {
            //寄信人的MAIL
            dt = dao.getTemp2(SessionHandle.Current.emp_id);
            if (dt.Rows.Count > 0)
            {
                dao.vSendto = dt.Rows[0]["SALARY_EMAIL"].ToString();
            }

            BeginTransaction();
            dao.deleteData();
            //新增發送MAIL 主檔資料 
            dao.addData();
            //INSERT 發送MAIL 明細資料 
            dao.addData2();
            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.ToString();
        }


    }

    //public string  deleteData(string txt_EMP_ID, string txt_EFFECT_YM, string txt_MAIL_DT)
    //{
    //    CFB2SE2200DAO wfb2se = new CFB2SE2200DAO();
    //    try
    //    {
    //        BeginTransaction();
    //        wfb2se.deleteData(txt_EMP_ID, txt_EFFECT_YM, txt_MAIL_DT);           
    //        Commit();
    //        return "0";

           

            
    //    }
    //    catch (Exception ex)
    //    {
    //        throw;
    //    }


    //}
    //public string addData(CFB2SE2200DAO wfb2se, string EMP_ID, string EFFECT_YM, string MAIL_DT)
    //{
    //    try
    //    {
    //        DataTable tmp = wfb2se.getExistData();

    //      //  if (tmp.Rows.Count > 0)
    //       // {
    //      //      BeginTransaction();
    //       //     wfb2se.deleteData(EMP_ID, EFFECT_YM, MAIL_DT);
    //      //      Commit();
    //      //  }
           
    //        BeginTransaction();
    //        //寫發送MAIL 表頭資料  
    //        wfb2se.addData();
    //        Commit();
    //        return "0";

    //    }
    //    catch (Exception ex)
    //    {
    //        RollBack();
    //        return ex.Message;
    //    }
    //}
    //public string addData2(string EMP_ID, string EFFECT_YM, string MAIL_DT)
    //{
    //    try
    //    {
    //        CFB2SE2200DAO wfb2se = new CFB2SE2200DAO();
    //        BeginTransaction();

    //        wfb2se.addData2(EMP_ID, EFFECT_YM, MAIL_DT);
    //        Commit();
    //        return "0";
    //    }
    //    catch (Exception)
    //    {
    //        RollBack();
    //        throw;
    //    }
    //}


    //產生Excel
    //public void createExcel(CFB2SE220DAO wfb2sc, string type)
    //{
    //    try
    //    {
    //        IWorkbook workbook;
    //        ISheet sheet;
    //        ICellStyle style1;
    //        ICellStyle style2;
    //        DataTable tmp = wfb2sc.searchResult();
    //        if (tmp.Rows.Count > 0)
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
    //            cell = row.CreateCell(0);
    //            cell.CellStyle = style1;
    //            cell.SetCellValue("部門代號");

    //            cell = row.CreateCell(1);
    //            cell.CellStyle = style1;
    //            cell.SetCellValue("部門");

    //            cell = row.CreateCell(2);
    //            cell.CellStyle = style1;
    //            cell.SetCellValue("課別");

    //            cell = row.CreateCell(3);
    //            cell.CellStyle = style1;
    //            cell.SetCellValue("工號");

    //            cell = row.CreateCell(4);
    //            cell.CellStyle = style1;
    //            cell.SetCellValue("姓名");

                

    //            cell = row.CreateCell(5);
    //            cell.CellStyle = style1;
    //            cell.SetCellValue("積欠金額");

                

    //            style2 = workbook.CreateCellStyle();

    //            style2.SetFont(font1);

    //            int x = 0;
    //            for (int i = 0; i < tmp.Rows.Count; i++)
    //            {
    //                x = i + 1;
    //                row = sheet.CreateRow(x);
    //                cell = row.CreateCell(0);
    //                cell.CellStyle = style2;
    //                cell.SetCellValue(tmp.Rows[i]["DEPT_NO"].ToString());

    //                cell = row.CreateCell(1);
    //                cell.CellStyle = style2;
    //                cell.SetCellValue(tmp.Rows[i]["DEPT_NAME_20"].ToString());


    //                cell = row.CreateCell(2);
    //                cell.CellStyle = style2;
    //                cell.SetCellValue(tmp.Rows[i]["DEPT_NAME_40"].ToString());

    //                cell = row.CreateCell(3);
    //                cell.CellStyle = style2;
    //                cell.SetCellValue(tmp.Rows[i]["EMP_ID"].ToString());

    //                cell = row.CreateCell(4);
    //                cell.CellStyle = style2;
    //                cell.SetCellValue(tmp.Rows[i]["EMP_NAME"].ToString());

                    

    //                cell = row.CreateCell(5);
    //                cell.CellStyle = style2;
    //                cell.SetCellValue(tmp.Rows[i]["AMOUNT"].ToString());

                    
    //            }
    //            sheet.AutoSizeColumn(0);
    //            sheet.AutoSizeColumn(1);
    //            sheet.AutoSizeColumn(2);
    //            sheet.AutoSizeColumn(3);
    //            sheet.AutoSizeColumn(4);
    //            sheet.AutoSizeColumn(5);
               

    //            ExcelHandle.exportExcel(workbook, "薪資積欠公司人員名單." + type);
    //        }
    //    }
    //    catch
    //    {
    //        throw;
    //    }
    //}






}
