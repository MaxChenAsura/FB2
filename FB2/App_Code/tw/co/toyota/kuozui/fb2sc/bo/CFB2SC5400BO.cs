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
/// CFB2SC5400BO 的摘要描述
/// </summary>
public class CFB2SC5400BO : BaseService
{
	public CFB2SC5400BO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}



    public DataTable get_SE2200_PDF_DATA()
        {
            try
            {
                CFB2SC5400DAO dao = new CFB2SC5400DAO();
                return dao.get_SE2200_PDF_DATA();
                
            }
            catch (Exception)
            {
                
                throw;
            }
        }

    public DataTable get_PDF_Data(string SALARY_YM, string DEPT_NO, string EMP_ID, string SALARY_DT_S, string SALARY_DT_E)
    {
        DataTable retVal = new DataTable(); ;
        CFB2SC5400DAO fb2sc = new CFB2SC5400DAO();
        try
        {
            retVal = fb2sc.get_PDF_Data(SALARY_YM, DEPT_NO, EMP_ID, SALARY_DT_S, SALARY_DT_E);
            return retVal;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public System.Data.DataTable getJPN_CD()
    {
        CFB2SC5400DAO wfb2sc = new CFB2SC5400DAO();
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
        CFB2SC5400DAO wfb2sc = new CFB2SC5400DAO();
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
    public string addData(CFB2SC5400DAO wfb2sc)
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
    //public void createExcel(CFB2SC5400DAO wfb2sc, string type)
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
