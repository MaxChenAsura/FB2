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
/// CFB2SC5300BO 的摘要描述
/// </summary>
public class CFB2SC5300BO : BaseService
{
    public CFB2SC5300BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public static ICellStyle stringRightStyle;  //數字style
    public static ICellStyle stringRightThickStyle;  //數字粗框style
    public static ICellStyle stringCenterStyle;  //置中style
    public static ICellStyle stringCenterThickStyle;  //置中粗框style
    public static ICellStyle stringLeftStyle;   //標題style
    public static ICellStyle stringLeftNoBorStyle;   //無邊框style
    public static ICellStyle stringCenterNoBorStyle;   //置中無邊框style
    public static int rowIndex = 0 ;                 //現在寫入的row  dept_no_20 != "" 使用
    public static int rowIndex2 = 0 ;                 //現在寫入的row 無歸屬部門資料 使用
    public static string flag = "";                 //有無create 無歸屬部門資料工作表
    IWorkbook workbook;
    ISheet sheet;
    CFB2SC5300DAO dao = new CFB2SC5300DAO();
    public System.Data.DataTable getJPN_CD()
    {
        CFB2SC5300DAO wfb2sc = new CFB2SC5300DAO();
        try
        {
            return wfb2sc.getJPN_CD();
        }
        catch (Exception)
        {

            throw;
        }
    }
    public string deleteData(CFB2SC5300DAO wfb2sc)
    {
        try
        {
            BeginTransaction();

            wfb2sc.deleteData();
            Commit();

            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }


    }
    public string addData_H(CFB2SC5300DAO wfb2sc)
    {
        try
        {
            BeginTransaction();
            wfb2sc.addData_H();
            Commit();
            return "0";

        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    public string addData_D(CFB2SC5300DAO wfb2sc)
    {
        try
        {
            BeginTransaction();
            wfb2sc.addData_D();
            Commit();
            return "0";

        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    public bool getProcess_Status(CFB2SC5300DAO wfb2sc)
    {
        try
        {
            return wfb2sc.getProcess_Status();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public IWorkbook createExcel(string SALARY_DT, string SALARY_TYPE, string DEPT_NO, string EMP_ID, string JPN_CD, string CREATED_BY, bool isTB_S_S)
    {
        ////FileStream fs = null;
        rowIndex2 = 0;
        flag = "";
        workbook = null;
        stringRightStyle = null;
        stringLeftStyle = null;
        stringCenterStyle = null;
        int noDeptRow = 0;//總共幾筆無歸屬部門資料
        int nowNoDept = 0; //目前是第幾筆無歸屬部門資料

        //取得範本sheet
        sheet = null;
        try
        {            
            string title = "";
            string dept_no_20 = "";
            string dept_no_40 = "";
            string acc_cd = "";
            string jpn_cd = "";
            string tableName = "";
            if (isTB_S_S)
                tableName = "TB_S_S_SALARY_PAY";
            else
                tableName = "TB_S_M_SALARY_PAY";
            workbook = new XSSFWorkbook();
            //ISheet sheet;

            stringRightStyle = setCellStyle(workbook, "right", true, 0,false);
            stringRightThickStyle = setCellStyle(workbook, "right", true, 0, true);
            stringCenterStyle = setCellStyle(workbook, "center", true, 0, false);
            stringCenterThickStyle = setCellStyle(workbook, "center", true, 0, true);
            stringCenterNoBorStyle = setCellStyle(workbook, "center", false, 0, false);
            stringLeftNoBorStyle = setCellStyle(workbook, "left", false, 0, false);
            stringLeftStyle = setCellStyle(workbook, "left", true, 0, false);
            //數字格式,有千分位,
            stringRightStyle.DataFormat = workbook.CreateDataFormat().GetFormat("#,##0");
            //數字格式,有千分位,
            stringRightThickStyle.DataFormat = workbook.CreateDataFormat().GetFormat("#,##0");
            DataTable dtSub = dao.searchSubject(SALARY_DT,SALARY_TYPE, CREATED_BY);
            DataTable dtEmp = dao.searchBy_EMP_ID(SALARY_DT, SALARY_TYPE, DEPT_NO, EMP_ID, JPN_CD, CREATED_BY, tableName);
            DataTable dtDept40 = dao.searchBy_Dept40(SALARY_DT, SALARY_TYPE, DEPT_NO, EMP_ID, JPN_CD, CREATED_BY, tableName);
            DataTable dtDept20 = dao.searchBy_Dept20(SALARY_DT, SALARY_TYPE, DEPT_NO, EMP_ID, JPN_CD, CREATED_BY, tableName);
            DataTable dtCompany = dao.searchBy_Company(SALARY_DT, SALARY_TYPE, DEPT_NO, EMP_ID, JPN_CD, CREATED_BY, tableName);

            //取得excel每個sheet名稱
            DataTable dtSheet = dao.searchResult_DEPT(SALARY_DT, SALARY_TYPE, DEPT_NO, EMP_ID, JPN_CD, CREATED_BY);
            //沒歸屬部門有幾筆
            DataTable dtSheetROW = dao.searchResult_DEPTRow(SALARY_DT, SALARY_TYPE, DEPT_NO, EMP_ID, JPN_CD, CREATED_BY);
            if (dtSheetROW.Rows.Count > 0)
            {
                noDeptRow = dtSheetROW.Rows.Count;
            }

            if (dtSheet.Rows.Count > 0)
            {
                for (int i = 0; i < dtSheet.Rows.Count; i++)
                {
                    rowIndex = 0;
                    dept_no_20 = Convert.ToString(dtSheet.Rows[i]["DEPT_NO_20"]);
                    dept_no_40 = Convert.ToString(dtSheet.Rows[i]["DEPT_NO_40"]);
                    acc_cd = Convert.ToString(dtSheet.Rows[i]["ACC_CD"]);
                    jpn_cd = Convert.ToString(dtSheet.Rows[i]["JPN_CD"]);

                    //if (dept_no_20 != "")
                    //    title += Convert.ToString(dtSheet.Rows[i]["DEPT_NAME_20"]);
                    //if (dept_no_40 != "")
                    //    title += "-" + Convert.ToString(dtSheet.Rows[i]["DEPT_NAME_20"]);
                    //if (acc_cd != "")
                    //    title += "-" + dtSheet.Rows[i]["JPN_DESC"].ToString();
                    //if (jpn_cd != "")
                    //    title += "-" + dtSheet.Rows[i]["ACC_DESC"].ToString();
                    //產生Excel sheet
                    if (dept_no_20 != "")
                    {                                            
                        sheet = workbook.CreateSheet(Convert.ToString(dtSheet.Rows[i]["DEPT_NAME_20"]).Replace("／", "").Replace("/", "") + "-"
                            + Convert.ToString(dtSheet.Rows[i]["DEPT_NAME_40"]).Replace("／", "").Replace("/", "")
                            + "-" + dtSheet.Rows[i]["JPN_DESC"].ToString() + "-" + dtSheet.Rows[i]["ACC_DESC"].ToString());
                    }
                    else
                    {                        
                        sheet = workbook.GetSheet("無歸屬部門資料");
                        if (sheet == null)
                        {
                            sheet = workbook.CreateSheet("無歸屬部門資料");
                        }
                        else
                        {
                            flag = "0";//表示已經有create 無歸屬部門資料
                        }
                        nowNoDept ++;
                    }
                   
                    sheet.PrintSetup.FitHeight = 1;
                    sheet.PrintSetup.FitWidth = 1;
                    sheet.CreateFreezePane(0, 12, 0, 15);
                    sheet.PrintSetup.PaperSize = 8;
                    sheet.PrintSetup.Landscape = true;
                    sheet.PrintSetup.UsePage = true;
                    sheet.FitToPage = true;
                    //workbook.SetRepeatingRowsAndColumns(0, 0, 0, 0, 10);

                    if (dept_no_20 != "")
                    {
                        create_Header(dtSheet.Rows[i]);
                        create_Subject(dtSub);
                        rowIndex = 12;
                        create_By_Emp_ID(dtEmp, dept_no_20, dept_no_40, acc_cd, jpn_cd,"Y");
                        create_By_Dept_40(dtDept40, dept_no_40, "Y");
                        create_By_Dept_20(dtDept20, dept_no_20, "Y");
                        create_All_Company(dtCompany, "Y");
                    }
                    else
                    {
                        //無歸屬部門資料
                        if (flag != "0")
                        {
                            create_Header(dtSheet.Rows[i]);
                            create_Subject(dtSub);
                            rowIndex2 = 12;
                        }
                        //create_Header(dtSheet.Rows[i]);
                        //create_Subject(dtSub);
                        //rowIndex = 12;
                        create_By_Emp_ID(dtEmp, dept_no_20, dept_no_40, acc_cd, jpn_cd, "N");
                        create_By_Dept_40(dtDept40, dept_no_40, "N");
                        create_By_Dept_20(dtDept20, dept_no_20, "N");

                        if (nowNoDept == noDeptRow)
	                    {
                            create_All_Company(dtCompany, "N");
	                    }                        
                    }
                    
                    for (int j = 0; j <= 15; j++)
                    {
                        sheet.SetColumnWidth(j, 15 * 256);
                    }
                }

                //for (int i = 0; i < dtSheet.Rows.Count; i++)
                //{
                //    sheet.PrintSetup.FitHeight = 1;
                //    sheet.PrintSetup.FitWidth = 1;
                //    sheet.CreateFreezePane(0, 12, 0, 15);
                //    sheet.PrintSetup.PaperSize = 8;
                //    sheet.PrintSetup.Landscape = true;
                //    sheet.PrintSetup.UsePage = true;
                //    sheet.FitToPage = true;
                    
                //    //create_Header(dtSheet.Rows[i]);
                //    //create_Subject(dtSub);
                //    rowIndex = 12;
                //    create_By_Emp_ID(dtEmp, dept_no_20, dept_no_40, acc_cd, jpn_cd);
                //    create_By_Dept_40(dtDept40, dept_no_40);
                //    create_By_Dept_20(dtDept20, dept_no_20);
                //    create_All_Company(dtCompany);
                //    for (int j = 0; j <= 15; j++)
                //    {
                //        sheet.SetColumnWidth(j, 15 * 256);
                //    }
                //}
            }
            else
                workbook = null;

            return workbook;
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    //sheet 表頭
    public void create_Header(DataRow rowSheet)
    {
        try
        {
            IRow row = sheet.CreateRow(0);
            ICell cell;

            cell = row.CreateCell(0);
            cell.CellStyle = stringCenterNoBorStyle;
            cell.SetCellValue(Convert.ToString(rowSheet["DATA_YM"]).Substring(0, 4) + "年" + Convert.ToString(rowSheet["DATA_YM"]).Substring(4) + "月 部門別員工薪資明細表(" + rowSheet["ACC_DESC"].ToString() + ")");
            sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 15));

            row = sheet.CreateRow(1);
            cell = row.CreateCell(0);
            cell.CellStyle = stringLeftNoBorStyle;
            cell.SetCellValue("外籍會社:");
            if (Convert.ToString(rowSheet["JPN_CD"]) != "")
            {
                cell = row.CreateCell(1);
                cell.CellStyle = stringLeftNoBorStyle;
                cell.SetCellValue(rowSheet["JPN_DESC"].ToString());
            }

            row = sheet.CreateRow(2);
            cell = row.CreateCell(0);
            cell.CellStyle = stringLeftNoBorStyle;
            string dept = "";
            if (Convert.ToString(rowSheet["DEPT_NO_20"]).Length == 7)
                dept = Convert.ToString(rowSheet["DEPT_NO_20"]).Substring(0, 3) + "(部級)";
            else
                dept = "(部級)";
            cell.SetCellValue(dept);
            cell = row.CreateCell(1);
            cell.CellStyle = stringLeftNoBorStyle;
            cell.SetCellValue(rowSheet["DEPT_NAME_20"].ToString());
       
            row = sheet.CreateRow(3);
            cell = row.CreateCell(1);
            cell.CellStyle = stringLeftNoBorStyle;
            cell.SetCellValue(rowSheet["DEPT_NAME_40"].ToString());
            cell = row.CreateCell(13);
            cell.CellStyle = stringLeftNoBorStyle;
            cell.SetCellValue("製表日期:");
            cell = row.CreateCell(14);
            cell.CellStyle = stringLeftNoBorStyle;
            cell.SetCellValue(DateTime.Now.ToString("yyyy/MM/dd"));

            sheet.AddMergedRegion(new CellRangeAddress(2, 2, 1, 2));
            sheet.AddMergedRegion(new CellRangeAddress(3, 3, 1, 2));
        }
        catch (Exception)
        {
            
            throw;
        }
       
    }
    public void create_Subject(DataTable dtSub)
    {
        try
        {
            //第一行
            IRow row = sheet.CreateRow(4);
            ICell cell;

            if (dtSub.Rows.Count > 0)
            {

                cell = row.CreateCell(0);
                cell.CellStyle = stringCenterStyle;
                cell.SetCellValue("工號");

                cell = row.CreateCell(1);
                cell.CellStyle = stringCenterStyle;
                cell.SetCellValue("姓名");

                #region"加項"
                for (int i = 1; i <= 12; i++)
                {
                    string index = "";
                    if (i < 10)
                        index = "0" + i;
                    else
                        index = i.ToString();
                    cell = row.CreateCell(i + 1);
                    cell.CellStyle = stringLeftStyle;
                    cell.SetCellValue(dtSub.Rows[0]["add" + index].ToString());
                }
                cell = row.CreateCell(14);
                cell.CellStyle = stringLeftStyle;
                cell.SetCellValue("");
                cell = row.CreateCell(15);
                cell.CellStyle = stringLeftStyle;
                cell.SetCellValue("");
                //第二行
                row = sheet.CreateRow(5);
                cell = row.CreateCell(0);
                cell.CellStyle = stringLeftStyle;
                cell = row.CreateCell(1);
                cell.CellStyle = stringLeftStyle;

                for (int i = 2; i <= 13; i++)
                {
                    cell = row.CreateCell(i);
                    cell.CellStyle = stringLeftStyle;
                    cell.SetCellValue(dtSub.Rows[0]["add" + (i + 11).ToString()].ToString());
                }

                cell = row.CreateCell(14);
                cell.CellStyle = stringLeftStyle;
                cell.SetCellValue("");
                cell = row.CreateCell(15);
                cell.CellStyle = stringLeftStyle;
                cell.SetCellValue("");
                //第三行
                row = sheet.CreateRow(6);
                cell = row.CreateCell(0);
                cell.CellStyle = stringLeftStyle;
                cell = row.CreateCell(1);
                cell.CellStyle = stringLeftStyle;

                for (int i = 2; i <= 13; i++)
                {
                    cell = row.CreateCell(i);
                    cell.CellStyle = stringLeftStyle;
                    cell.SetCellValue(dtSub.Rows[0]["add" + (i + 23).ToString()].ToString());
                }

                cell = row.CreateCell(14);
                cell.CellStyle = stringLeftStyle;
                cell.SetCellValue("");

                cell = row.CreateCell(15);
                cell.CellStyle = stringLeftStyle;
                cell.SetCellValue("所得總額");
                //第四行
                row = sheet.CreateRow(7);
                cell = row.CreateCell(0);
                cell.CellStyle = stringLeftStyle;
                cell = row.CreateCell(1);
                cell.CellStyle = stringLeftStyle;

                for (int i = 2; i <= 13; i++)
                {
                    cell = row.CreateCell(i);
                    cell.CellStyle = stringLeftStyle;
                    cell.SetCellValue(dtSub.Rows[0]["add" + (i + 35).ToString()].ToString());
                }

                cell = row.CreateCell(14);
                cell.CellStyle = stringLeftStyle;
                cell.SetCellValue("加項合計");

                cell = row.CreateCell(15);
                cell.CellStyle = stringLeftStyle;
                cell.SetCellValue("課稅合計");
                #endregion

                #region "扣項"
                //第五行
                row = sheet.CreateRow(8);
                cell = row.CreateCell(0);
                cell.CellStyle = stringLeftStyle;
                cell = row.CreateCell(1);
                cell.CellStyle = stringLeftStyle;

                for (int i = 1; i <= 12; i++)
                {
                    string index = "";
                    if (i < 10)
                        index = "0" + i;
                    else
                        index = i.ToString();
                    cell = row.CreateCell(i + 1);
                    cell.CellStyle = stringLeftStyle;
                    cell.SetCellValue(dtSub.Rows[0]["sub" + index].ToString());
                }

                cell = row.CreateCell(14);
                cell.CellStyle = stringLeftStyle;
                cell.SetCellValue("");
                cell = row.CreateCell(15);
                cell.CellStyle = stringLeftStyle;
                cell.SetCellValue("");
                //第六行
                row = sheet.CreateRow(9);
                cell = row.CreateCell(0);
                cell.CellStyle = stringLeftStyle;
                cell = row.CreateCell(1);
                cell.CellStyle = stringLeftStyle;
                for (int i = 2; i <= 13; i++)
                {
                    cell = row.CreateCell(i);
                    cell.CellStyle = stringLeftStyle;
                    cell.SetCellValue(dtSub.Rows[0]["sub" + (i + 11).ToString()].ToString());
                }

                cell = row.CreateCell(14);
                cell.CellStyle = stringLeftStyle;
                cell.SetCellValue("扣項合計");
                cell = row.CreateCell(15);
                cell.CellStyle = stringLeftStyle;
                cell.SetCellValue("");
                //第七行
                row = sheet.CreateRow(10);
                cell = row.CreateCell(0);
                cell.CellStyle = stringLeftStyle;
                cell = row.CreateCell(1);
                cell.CellStyle = stringLeftStyle;

                for (int i = 2; i <= 13; i++)
                {
                    cell = row.CreateCell(i);
                    cell.CellStyle = stringLeftStyle;
                    cell.SetCellValue(dtSub.Rows[0]["sub" + (i + 23).ToString()].ToString());
                }

                cell = row.CreateCell(14);
                cell.CellStyle = stringLeftStyle;
                cell.SetCellValue("實扣金額");

                cell = row.CreateCell(15);
                cell.CellStyle = stringLeftStyle;
                cell.SetCellValue("所得代扣");
                //第八行
                row = sheet.CreateRow(11);
                cell = row.CreateCell(0);
                cell.CellStyle = stringLeftStyle;
                cell = row.CreateCell(1);
                cell.CellStyle = stringLeftStyle;

                for (int i = 2; i <= 13; i++)
                {
                    cell = row.CreateCell(i);
                    cell.CellStyle = stringLeftStyle;
                    cell.SetCellValue(dtSub.Rows[0]["sub" + (i + 35).ToString()].ToString());
                }

                cell = row.CreateCell(14);
                cell.CellStyle = stringLeftStyle;
                cell.SetCellValue("積欠總額");

                cell = row.CreateCell(15);
                cell.CellStyle = stringLeftStyle;
                cell.SetCellValue("實領總額");
                #endregion
                sheet.AddMergedRegion(new CellRangeAddress(4, 11, 0, 0));
                sheet.AddMergedRegion(new CellRangeAddress(4, 11, 1, 1));
            }
        }
        catch (Exception)
        {
            
            throw;
        }
       
    }
    public void create_By_Emp_ID(DataTable dtEmp, string dept_no_20, string dept_no_40, string acc_cd, string jpn_cd,string haveDept)
    {
        try
        {
            for (int i = 0; i < dtEmp.Rows.Count; i++)
            {
                if (Convert.ToString(dtEmp.Rows[i]["DEPT_NO_40"]) == dept_no_40 && Convert.ToString(dtEmp.Rows[i]["DEPT_NO_20"]) == dept_no_20
                    && Convert.ToString(dtEmp.Rows[i]["ACC_CD"]) == acc_cd && Convert.ToString(dtEmp.Rows[i]["JPN_CD"]) == jpn_cd)
                {
                    createFillCell("emp", dtEmp.Rows[i], haveDept);
                }
            }
        }
        catch (Exception)
        {
            
            throw;
        }
        
    }
    public void create_By_Dept_40(DataTable dtDept40, string dept_no_40, string haveDept)
    {
        try
        {
            for (int i = 0; i < dtDept40.Rows.Count; i++)
            {
                if (dtDept40.Rows[i]["DEPT_NO_40"].ToString() == dept_no_40)
                {
                    createFillCell("dept40", dtDept40.Rows[i], haveDept);
                }
            }
        }
        catch (Exception)
        {
            
            throw;
        }
       
    }
    public void create_By_Dept_20(DataTable dtDept20, string dept_no_20, string haveDept)
    {
        try
        {
            for (int i = 0; i < dtDept20.Rows.Count; i++)
            {
                if (dtDept20.Rows[i]["DEPT_NO_20"].ToString() == dept_no_20)
                {
                    createFillCell("dept20", dtDept20.Rows[i], haveDept);
                }
            }
        }
        catch (Exception)
        {
            
            throw;
        }
       
    }
    public void create_All_Company(DataTable dtCompany, string haveDept)
    {
        createFillCell("company", dtCompany.Rows[0], haveDept);
    }
    public void createFillCell(string fill_kind, DataRow rowData, string haveDept)
    {
        try
        {
            if (haveDept == "N") {
                rowIndex = rowIndex2;
            }
            ICell cell;
            IRow row00 = sheet.CreateRow(rowIndex);
            IRow row00_2 = sheet.CreateRow(rowIndex + 1);
            IRow row00_3 = sheet.CreateRow(rowIndex + 2);
            IRow row00_4 = sheet.CreateRow(rowIndex + 3);
            IRow row00_5 = sheet.CreateRow(rowIndex + 4);
            IRow row00_6 = sheet.CreateRow(rowIndex + 5);
            IRow row00_7 = sheet.CreateRow(rowIndex + 6);
            IRow row00_8 = sheet.CreateRow(rowIndex + 7);

            cell = row00.CreateCell(0);
            cell.CellStyle = stringCenterThickStyle;
            if (fill_kind == "emp")
                cell.SetCellValue(rowData["EMP_ID"].ToString());
            else if (fill_kind == "dept40")
                cell.SetCellValue("合計");
            else if (fill_kind == "dept20")
                cell.SetCellValue(rowData["DEPT_NAME_20"].ToString() + "合計");
            else if (fill_kind == "company")
                cell.SetCellValue("公司合計");
            cell = row00_2.CreateCell(0);
            cell.CellStyle = stringCenterThickStyle;
            cell = row00_3.CreateCell(0);
            cell.CellStyle = stringCenterThickStyle;
            cell = row00_4.CreateCell(0);
            cell.CellStyle = stringCenterThickStyle;
            cell = row00_5.CreateCell(0);
            cell.CellStyle = stringCenterThickStyle;
            cell = row00_6.CreateCell(0);
            cell.CellStyle = stringCenterThickStyle;
            cell = row00_7.CreateCell(0);
            cell.CellStyle = stringCenterThickStyle;
            cell = row00_8.CreateCell(0);
            cell.CellStyle = stringCenterThickStyle;
            sheet.AddMergedRegion(new CellRangeAddress(rowIndex, rowIndex + 7, 0, 0));

            cell = row00.CreateCell(1);
            cell.CellStyle = stringCenterThickStyle;
            if (fill_kind == "emp")
                cell.SetCellValue(rowData["EMP_NAME"].ToString());
            else
                cell.SetCellValue(rowData["EMP_COUNT"].ToString() + "人");
            cell = row00_2.CreateCell(1);
            cell.CellStyle = stringCenterThickStyle;
            cell = row00_3.CreateCell(1);
            cell.CellStyle = stringCenterThickStyle;
            cell = row00_4.CreateCell(1);
            cell.CellStyle = stringCenterThickStyle;
            cell = row00_5.CreateCell(1);
            cell.CellStyle = stringCenterThickStyle;
            cell = row00_6.CreateCell(1);
            cell.CellStyle = stringCenterThickStyle;
            cell = row00_7.CreateCell(1);
            cell.CellStyle = stringCenterThickStyle;
            cell = row00_8.CreateCell(1);
            cell.CellStyle = stringCenterThickStyle;
            sheet.AddMergedRegion(new CellRangeAddress(rowIndex, rowIndex + 7, 1, 1));

            #region "加項"
            cell = row00.CreateCell(2);
            cell.CellStyle = stringRightThickStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_A_01"]));

            cell = row00.CreateCell(3);
            cell.CellStyle = stringRightThickStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_A_02"]));

            cell = row00.CreateCell(4);
            cell.CellStyle = stringRightThickStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_A_03"]));

            cell = row00.CreateCell(5);
            cell.CellStyle = stringRightThickStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_A_04"]));

            cell = row00.CreateCell(6);
            cell.CellStyle = stringRightThickStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_A_05"]));

            cell = row00.CreateCell(7);
            cell.CellStyle = stringRightThickStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_A_06"]));

            cell = row00.CreateCell(8);
            cell.CellStyle = stringRightThickStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_A_07"]));

            cell = row00.CreateCell(9);
            cell.CellStyle = stringRightThickStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_A_08"]));

            cell = row00.CreateCell(10);
            cell.CellStyle = stringRightThickStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_A_09"]));

            cell = row00.CreateCell(11);
            cell.CellStyle = stringRightThickStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_A_10"]));

            cell = row00.CreateCell(12);
            cell.CellStyle = stringRightThickStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_A_11"]));

            cell = row00.CreateCell(13);
            cell.CellStyle = stringRightThickStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_A_12"]));

            cell = row00.CreateCell(14);
            cell.CellStyle = stringRightThickStyle;
            cell.SetCellValue("");

            cell = row00.CreateCell(15);
            cell.CellStyle = stringRightThickStyle;
            cell.SetCellValue("");


            cell = row00_2.CreateCell(2);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_A_13"]));

            cell = row00_2.CreateCell(3);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_A_14"]));

            cell = row00_2.CreateCell(4);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_A_15"]));

            cell = row00_2.CreateCell(5);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_A_16"]));

            cell = row00_2.CreateCell(6);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_A_17"]));

            cell = row00_2.CreateCell(7);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_A_18"]));

            cell = row00_2.CreateCell(8);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_A_19"]));

            cell = row00_2.CreateCell(9);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_A_20"]));

            cell = row00_2.CreateCell(10);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_A_21"]));

            cell = row00_2.CreateCell(11);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_A_22"]));

            cell = row00_2.CreateCell(12);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_A_23"]));

            cell = row00_2.CreateCell(13);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_A_24"]));

            cell = row00_2.CreateCell(14);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue("");

            cell = row00_2.CreateCell(15);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue("");


            cell = row00_3.CreateCell(2);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_A_25"]));

            cell = row00_3.CreateCell(3);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_A_26"]));

            cell = row00_3.CreateCell(4);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_A_27"]));

            cell = row00_3.CreateCell(5);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_A_28"]));

            cell = row00_3.CreateCell(6);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_A_29"]));

            cell = row00_3.CreateCell(7);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_A_30"]));

            cell = row00_3.CreateCell(8);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_A_31"]));

            cell = row00_3.CreateCell(9);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_A_32"]));

            cell = row00_3.CreateCell(10);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_A_33"]));

            cell = row00_3.CreateCell(11);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_A_34"]));

            cell = row00_3.CreateCell(12);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_A_35"]));

            cell = row00_3.CreateCell(13);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_A_36"]));

            cell = row00_3.CreateCell(14);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue("");

            cell = row00_3.CreateCell(15);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["TOTAL_SUM"]));

            //第四行
            cell = row00_4.CreateCell(2);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_A_37"]));

            cell = row00_4.CreateCell(3);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_A_38"]));

            cell = row00_4.CreateCell(4);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_A_39"]));

            cell = row00_4.CreateCell(5);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_A_40"]));

            cell = row00_4.CreateCell(6);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_A_41"]));

            cell = row00_4.CreateCell(7);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_A_42"]));

            cell = row00_4.CreateCell(8);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_A_43"]));

            cell = row00_4.CreateCell(9);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_A_44"]));

            cell = row00_4.CreateCell(10);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_A_45"]));

            cell = row00_4.CreateCell(11);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_A_46"]));

            cell = row00_4.CreateCell(12);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_A_47"]));

            cell = row00_4.CreateCell(13);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_A_48"]));

            cell = row00_4.CreateCell(14);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["ADD_SUM"]));

            cell = row00_4.CreateCell(15);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["TAXATION_SUM"]));
            #endregion

            #region"減項"
            cell = row00_5.CreateCell(2);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_D_01"]));

            cell = row00_5.CreateCell(3);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_D_02"]));

            cell = row00_5.CreateCell(4);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_D_03"]));

            cell = row00_5.CreateCell(5);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_D_04"]));

            cell = row00_5.CreateCell(6);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_D_05"]));

            cell = row00_5.CreateCell(7);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_D_06"]));

            cell = row00_5.CreateCell(8);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_D_07"]));

            cell = row00_5.CreateCell(9);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_D_08"]));

            cell = row00_5.CreateCell(10);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_D_09"]));

            cell = row00_5.CreateCell(11);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_D_10"]));

            cell = row00_5.CreateCell(12);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_D_11"]));

            cell = row00_5.CreateCell(13);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_D_12"]));

            cell = row00_5.CreateCell(14);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue("");

            cell = row00_5.CreateCell(15);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue("");

            cell = row00_6.CreateCell(2);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_D_13"]));

            cell = row00_6.CreateCell(3);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_D_14"]));

            cell = row00_6.CreateCell(4);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_D_15"]));

            cell = row00_6.CreateCell(5);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_D_16"]));

            cell = row00_6.CreateCell(6);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_D_17"]));

            cell = row00_6.CreateCell(7);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_D_18"]));

            cell = row00_6.CreateCell(8);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_D_19"]));

            cell = row00_6.CreateCell(9);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_D_20"]));

            cell = row00_6.CreateCell(10);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_D_21"]));

            cell = row00_6.CreateCell(11);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_D_22"]));

            cell = row00_6.CreateCell(12);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_D_23"]));

            cell = row00_6.CreateCell(13);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_D_24"]));

            cell = row00_6.CreateCell(14);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["SUB_SUM"]));

            cell = row00_6.CreateCell(15);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue("");

            cell = row00_7.CreateCell(2);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_D_25"]));

            cell = row00_7.CreateCell(3);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_D_26"]));

            cell = row00_7.CreateCell(4);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_D_27"]));

            cell = row00_7.CreateCell(5);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_D_28"]));

            cell = row00_7.CreateCell(6);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_D_29"]));

            cell = row00_7.CreateCell(7);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_D_30"]));

            cell = row00_7.CreateCell(8);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_D_31"]));

            cell = row00_7.CreateCell(9);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_D_32"]));

            cell = row00_7.CreateCell(10);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_D_33"]));

            cell = row00_7.CreateCell(11);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_D_34"]));

            cell = row00_7.CreateCell(12);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_D_35"]));

            cell = row00_7.CreateCell(13);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_D_36"]));

            cell = row00_7.CreateCell(14);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["REAL_SUB_SUM"]));

            cell = row00_7.CreateCell(15);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["TOTAL_TAX"]));

            cell = row00_8.CreateCell(2);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_D_37"]));

            cell = row00_8.CreateCell(3);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_D_38"]));

            cell = row00_8.CreateCell(4);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_D_39"]));

            cell = row00_8.CreateCell(5);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_D_40"]));

            cell = row00_8.CreateCell(6);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_D_41"]));

            cell = row00_8.CreateCell(7);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_D_42"]));

            cell = row00_8.CreateCell(8);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_D_43"]));

            cell = row00_8.CreateCell(9);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_D_44"]));

            cell = row00_8.CreateCell(10);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_D_45"]));

            cell = row00_8.CreateCell(11);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_D_46"]));

            cell = row00_8.CreateCell(12);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_D_47"]));

            cell = row00_8.CreateCell(13);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["AMOUNT_D_48"]));

            cell = row00_8.CreateCell(14);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["ARREARS_SUM"]));

            cell = row00_8.CreateCell(15);
            cell.CellStyle = stringRightStyle;
            cell.SetCellValue(Convert.ToDouble(rowData["REAL_SUM"]));
            #endregion            
            rowIndex = rowIndex + 8;
            if (haveDept == "N")
            {
                rowIndex2 = rowIndex;
            }
        }
        catch (Exception)
        {
            
            throw;
        }
       
    }

    #region "Excel Style"
    public ICellStyle setCellStyle(IWorkbook workbook, string align, bool isBorder, int colorCD,bool isThick)
    {
        ICellStyle style = workbook.CreateCellStyle();

        //資料的字型
        IFont cellFont = workbook.CreateFont();
        cellFont.FontName = "新細明體";
        cellFont.FontHeightInPoints = 12;  //字型大小
        cellFont.Color = HSSFColor.Black.Index;   //字型顏色
        cellFont.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Normal;   //bold:粗體字
        style.SetFont(cellFont);

        //是否要有邊框
        if (isBorder)
        {
            if(isThick)
                style.BorderTop = BorderStyle.Thick;
            else
                style.BorderTop = BorderStyle.Thin;

            style.BorderBottom = BorderStyle.Thin;
            style.BorderLeft = BorderStyle.Thin;
            style.BorderRight = BorderStyle.Thin;
        }

        //文字位置 (預設靠左)
        if (align.ToLower() == "center")
        {
            style.Alignment = HorizontalAlignment.Center;
        }
        else if (align.ToLower() == "right")
        {
            style.Alignment = HorizontalAlignment.Right;
        }
        else
        {
            style.Alignment = HorizontalAlignment.Left;
        }

        //背景顏色
        if (colorCD > 0)
        {
            style.FillForegroundColor = (short)colorCD;
            style.FillPattern = FillPattern.SolidForeground;
        }
        return style;
    }
    #endregion
}
