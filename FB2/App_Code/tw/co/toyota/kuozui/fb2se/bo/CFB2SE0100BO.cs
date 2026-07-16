using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.Util;


/// <summary>
/// CFB2SE0100BO 的摘要描述
/// </summary>
public class CFB2SE0100BO : BaseService
{
	public CFB2SE0100BO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    public string Add(CFB2SE0100DAO fb2se, DataTable dtSend, string EFFECT_YM)
    {
        try
        {
            //取得現有資料(檢查重複)
            int count = fb2se.GetCount_TB_S_M_SALARYSET_H();
            if (count > 0)
            {
                return "該生效年月之考核調薪入力作業已有資料,不允執行此功能。";
            }
            BeginTransaction();
            fb2se.Add_TB_S_M_SALARYSET_H(EFFECT_YM);
            foreach (DataRow dr in dtSend.Rows)
            {
                fb2se.EFFECT_YM = Convert.ToString(dr["EFFECT_YM"]);
                fb2se.LEVEL_CD = Convert.ToString(dr["LEVEL_CD"]);
                fb2se.GRADE_CD = Convert.ToString(dr["GRADE_CD"]);
                fb2se.EXAMINE_A = Convert.ToString(dr["EXAMINE_A"]);
                fb2se.EXAMINE_B = Convert.ToString(dr["EXAMINE_B"]);
                fb2se.EXAMINE_C = Convert.ToString(dr["EXAMINE_C"]);
                fb2se.EXAMINE_D = Convert.ToString(dr["EXAMINE_D"]);
                fb2se.EXAMINE_E = Convert.ToString(dr["EXAMINE_E"]);
                fb2se.ABILITY_ADJ = Convert.ToString(dr["ABILITY_ADJ"]);
                fb2se.LEVEL_ADJ = Convert.ToString(dr["LEVEL_ADJ"]);
                fb2se.LEVEL_PAY_LOW = Convert.ToString(dr["LEVEL_PAY_LOW"]);
                fb2se.LEVEL_PAY_AVG = Convert.ToString(dr["LEVEL_PAY_AVG"]);
                fb2se.LEVEL_PAY_UP = Convert.ToString(dr["LEVEL_PAY_UP"]);
                fb2se.ORDER_SEQ = Convert.ToString(dr["ORDER_SEQ"]);
                
                fb2se.Add_TB_S_M_SALARYSET_D();
            }
            
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
    public string Update(CFB2SE0100DAO fb2se)
    {
        try
        {
            BeginTransaction();
            fb2se.Update_TB_S_M_SALARYSET_D();
            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    public string Update_Edit(CFB2SE0100DAO fb2se)
    {
        try
        {
            BeginTransaction();
            fb2se.Update_TB_S_M_SALARYSET_D_Edit();
            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    //資格級數下載
    public IWorkbook createExcelResult(string excelPath, CFB2SE0100DAO se010DAO)
    {

        FileStream fs = null;
        IWorkbook workbook = null;

        //取得範本sheet
        ISheet sheet = null;
        try
        {
            //取得下載資料
            DataTable dt = se010DAO.getExcelResultData();

            fs = new FileStream(excelPath, FileMode.Open, FileAccess.ReadWrite);
            workbook = new XSSFWorkbook(fs); //xlsx的方法

            //取得範本sheet
            sheet = workbook.GetSheetAt(0);

            if (sheet != null)
            {

                ICellStyle stringRedLeftStyle = this.setCellStyle(workbook, "left", true, 12, 10);
                IRow row;
                ICell cell;
                //若只有title時 ,儲存錯誤訊息
                if (dt.Rows.Count == 0)
                {
                    row = sheet.CreateRow(1);
                    cell = row.CreateCell(1);
                    cell.CellStyle = stringRedLeftStyle;  //先
                    cell.SetCellValue("無資料"); //後

                }

                if (dt.Rows.Count > 0)
                {

                    int x = 0;
                    ICellStyle stringLeftStyle = this.setCellStyle(workbook, "left", true, 12);
                    ICellStyle stringRightStyle = this.setCellStyle(workbook, "right", true, 12);
                    ICellStyle stringCenterStyle = this.setCellStyle(workbook, "center", true, 12);

                    row = sheet.GetRow(0);
                    cell = row.GetCell(0);

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        x = i + 1;//從第1列開始insert 資料
                        //將資料寫入範本
                        row = sheet.CreateRow(x);

                        //資格
                        cell = row.CreateCell(1);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["LEVEL_CD"].ToString());

                        //級數
                        cell = row.CreateCell(2);
                        cell.CellStyle = stringLeftStyle;  //先
                        cell.SetCellValue(dt.Rows[i]["GRADE_CD"].ToString()); //後                       

                        //金額
                        for (int j = 3; j <= 12; j++) {
                            cell = row.CreateCell(j);
                            cell.CellStyle = stringRightStyle;
                            cell.SetCellValue(0);
                        }

                    }
                    //製表日期
                    ICellStyle stringLeftStyleDate = this.setCellStyle(workbook, "left", false, 14);
                    row = sheet.GetRow(0);
                    cell = row.CreateCell(13);
                    cell.CellStyle = stringLeftStyleDate;
                    cell.SetCellValue("製表日期:" + DateTime.Now.ToString("yyyy/MM/dd"));

                    /*
                    for (int i = 0; i <= 10; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                    */

                }

                return workbook;
            }

            return null;
        }
        catch (Exception ex)
        {
            throw;
        }
        finally
        {
            workbook.Clear();
            fs.Close();
            sheet = null;
            workbook = null;
        }
    }

    //EXCEL上傳
    public IWorkbook uploadExcel1(Stream fs, string type, CFB2SE0100DAO se010DAO)
    {
        //取得登入者
        string userid = SessionHandle.Current.emp_id;
        try
        {
            IWorkbook workbook;
            //依附檔名判斷要用哪種方式讀取
            if (type == ".xls")
            {
                workbook = new HSSFWorkbook(fs);
            }
            else if (type == ".xlsx")
            {
                workbook = new XSSFWorkbook(fs);
            }
            else
            {
                return null;
            }

            //取得sheet
            ISheet sheet = workbook.GetSheetAt(0);
            ICellStyle style1 = workbook.CreateCellStyle();
            IFont font1 = workbook.CreateFont();
            font1.Color = HSSFColor.Red.Index;
            style1.SetFont(font1);

            if (sheet != null)
            {
                try
                {
                    //1.初始值
                    DataTable excel_data = new DataTable();      //記錄EXCEL的資料
                    DataTable excel_pk_data = new DataTable();   //記錄EXCEL的PK資料
                    string[] excel_pk_arr = new string[2];       //用來判斷是否工號重複
                    DataRow dr;                                   //查檢pk用

                    //取得考績的範圍
                    bool valid = true;

                    #region 建立 excel
                    //建立 DataTable,存放EXCEL的資料
                    DataRow excel_row;
                    //建立 FieldSchema
                    excel_data.Columns.Add("LEVEL_CD", System.Type.GetType("System.String"));
                    excel_data.Columns.Add("GRADE_CD", System.Type.GetType("System.String"));
                    excel_data.Columns.Add("EXAMINE_A", System.Type.GetType("System.String"));
                    excel_data.Columns.Add("EXAMINE_B", System.Type.GetType("System.String"));
                    excel_data.Columns.Add("EXAMINE_C", System.Type.GetType("System.String"));
                    excel_data.Columns.Add("EXAMINE_D", System.Type.GetType("System.String"));
                    excel_data.Columns.Add("EXAMINE_E", System.Type.GetType("System.String"));
                    excel_data.Columns.Add("ABILITY_ADJ", System.Type.GetType("System.String"));
                    excel_data.Columns.Add("LEVEL_ADJ", System.Type.GetType("System.String"));
                    excel_data.Columns.Add("LEVEL_PAY_LOW", System.Type.GetType("System.String"));
                    excel_data.Columns.Add("LEVEL_PAY_AVG", System.Type.GetType("System.String"));
                    excel_data.Columns.Add("LEVEL_PAY_UP", System.Type.GetType("System.String"));


                    //存放EXCEL 檢查能否重複的資料
                    DataRow excel_pk_row;
                    excel_pk_data.Columns.Add("LEVEL_CD", System.Type.GetType("System.String"));
                    excel_pk_data.Columns.Add("GRADE_CD", System.Type.GetType("System.String"));

                    #endregion

                    //2.取得excel的資料
                    string cell_LEVEL_CD="";	//資格代號
                    string cell_GRADE_CD="";	//級數
                    string cell_EXAMINE_A="";	//	考績A調額
                    string cell_EXAMINE_B="";	//	考績B調額
                    string cell_EXAMINE_C="";	//	考績C調額
                    string cell_EXAMINE_D="";	//	考績D調額
                    string cell_EXAMINE_E="";	//	考績E調額
                    string cell_ABILITY_ADJ="";	//	資格調額
                    string cell_LEVEL_ADJ="";	//	職能調額
                    string cell_LEVEL_PAY_LOW="";	//	職能下限
                    string cell_LEVEL_PAY_AVG="";	//	職能中數
                    string cell_LEVEL_PAY_UP="";	//	職能上限

                    string error = "";

                    //巡覽每row的資料第一列為title跳過(故i從3開始)
                    for (int i = 1; i <= sheet.LastRowNum; i++)
                    {
                        error = "";
                        if (sheet.GetRow(i) != null)
                        {
                            cell_LEVEL_CD = sheet.GetRow(i).GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            cell_GRADE_CD = sheet.GetRow(i).GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            cell_EXAMINE_A= sheet.GetRow(i).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().Replace(",", "");
                            cell_EXAMINE_B= sheet.GetRow(i).GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().Replace(",", "");
                            cell_EXAMINE_C= sheet.GetRow(i).GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().Replace(",", "");
                            cell_EXAMINE_D= sheet.GetRow(i).GetCell(6, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().Replace(",", "");
                            cell_EXAMINE_E= sheet.GetRow(i).GetCell(7, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().Replace(",", "");
                            cell_ABILITY_ADJ= sheet.GetRow(i).GetCell(8, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().Replace(",", "");
                            cell_LEVEL_ADJ= sheet.GetRow(i).GetCell(9, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().Replace(",", "");
                            cell_LEVEL_PAY_LOW= sheet.GetRow(i).GetCell(10, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().Replace(",", "");
                            cell_LEVEL_PAY_AVG = sheet.GetRow(i).GetCell(11, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().Replace(",", "");
                            cell_LEVEL_PAY_UP = sheet.GetRow(i).GetCell(12, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().Replace(",", "");


                            //檢查文字欄位
                            error += utilities.checkLength(cell_LEVEL_CD, "資格", 3, false);
                            error += utilities.checkLength(cell_GRADE_CD, "級數", 1, true);

                           

                            //金額欄位
                            error += utilities.checkNumber(cell_EXAMINE_A, "A金額", 7, false);
                            error += utilities.checkNumber(cell_EXAMINE_B, "B金額", 7, false);
                            error += utilities.checkNumber(cell_EXAMINE_C, "C金額", 7, false);
                            error += utilities.checkNumber(cell_EXAMINE_D, "D金額", 7, false);
                            error += utilities.checkNumber(cell_EXAMINE_E, "E金額", 7, false);
                            error += utilities.checkNumber(cell_ABILITY_ADJ, "資格BU", 7, false);
                            error += utilities.checkNumber(cell_LEVEL_ADJ, "職能BU", 7, false);
                            error += utilities.checkNumber(cell_LEVEL_PAY_LOW, "職能(下)", 7, false);
                            error += utilities.checkNumber(cell_LEVEL_PAY_AVG, "職能(中)", 7, false);
                            error += utilities.checkNumber(cell_LEVEL_PAY_UP, "職能(上)", 7, false);

                            //資格級數是否存在
                            if (cell_LEVEL_CD != "" && se010DAO.chklevelcd(cell_LEVEL_CD, cell_GRADE_CD) == 0)
                            {
                                error += "資格,級數不存在\n";
                            }
                            //若有值,檢查資格級數是否重覆
                            excel_pk_arr[0] = cell_LEVEL_CD;
                            excel_pk_arr[1] = cell_GRADE_CD;
                            if (excel_pk_data.Rows.Count > 0)
                            {
                                dr = excel_pk_data.Rows.Find(excel_pk_arr);
                                if (dr != null)
                                {
                                    error += "此EXCEL有相同的資格,級數\n";
                                }
                                else
                                {
                                    excel_pk_row = excel_pk_data.NewRow();
                                    excel_pk_row["LEVEL_CD"] = cell_LEVEL_CD;
                                    excel_pk_row["GRADE_CD"] = cell_GRADE_CD;
                                    excel_pk_data.Rows.Add(excel_pk_row);
                                    excel_pk_data.PrimaryKey = new DataColumn[] { excel_pk_data.Columns["LEVEL_CD"], excel_pk_data.Columns["GRADE_CD"], };
                                }
                            }
                            else
                            {
                                excel_pk_row = excel_pk_data.NewRow();
                                excel_pk_row["LEVEL_CD"] = cell_LEVEL_CD;
                                excel_pk_row["GRADE_CD"] = cell_GRADE_CD;
                                excel_pk_data.Rows.Add(excel_pk_row);
                                excel_pk_data.PrimaryKey = new DataColumn[] { excel_pk_data.Columns["LEVEL_CD"], excel_pk_data.Columns["GRADE_CD"], };
                            }

                            excel_row = excel_data.NewRow();
                            excel_row["LEVEL_CD"] = cell_LEVEL_CD;
                            excel_row["GRADE_CD"] = cell_GRADE_CD;
                            excel_row["EXAMINE_A"] = cell_EXAMINE_A;
                            excel_row["EXAMINE_B"] = cell_EXAMINE_B;
                            excel_row["EXAMINE_C"] = cell_EXAMINE_C;
                            excel_row["EXAMINE_D"] = cell_EXAMINE_D;
                            excel_row["EXAMINE_E"] = cell_EXAMINE_E;
                            excel_row["ABILITY_ADJ"] = cell_ABILITY_ADJ;
                            excel_row["LEVEL_ADJ"] = cell_LEVEL_ADJ;
                            excel_row["LEVEL_PAY_LOW"] = cell_LEVEL_PAY_LOW;
                            excel_row["LEVEL_PAY_AVG"] = cell_LEVEL_PAY_AVG;
                            excel_row["LEVEL_PAY_UP"] = cell_LEVEL_PAY_UP;
                            excel_data.Rows.Add(excel_row);                            

                            //傳出錯誤訊息
                            style1.SetFont(font1);
                            sheet.GetRow(i).CreateCell(0).CellStyle = style1;
                            sheet.GetRow(i).GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(error);
                            if (error != "")
                            {
                                valid = false;
                            }
                        }
                    }

                    //若只有title時 ,儲存錯誤訊息
                    if (sheet.LastRowNum < 1)
                    {
                        error = "EXCEL無資料";
                        sheet.CreateRow(1);
                        sheet.GetRow(1).CreateCell(0).CellStyle = style1;
                        sheet.GetRow(1).GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(error);
                        if (error != "")
                        {
                            valid = false;
                        }
                    }

                    //檢核有錯，匯出附加說明的excel
                    if (!valid)
                    {
                        return workbook;
                        //ExcelHandle.exportExcel(workbook, "檢核錯誤說明" + type);
                    }

                    //檢核正確,修改考績
                    if (valid)
                    {
                        se010DAO.CREATED_BY = SessionHandle.Current.emp_id;
                        se010DAO.UPDATED_BY = SessionHandle.Current.emp_id;
                        se010DAO.FUNC_ID = "FB2SE010";
                        try
                        {
                            BeginTransaction();
                            //TB_S_M_SALARYSET_H	3A以下調薪金額主檔
                            //TB_S_M_SALARYSET_D	3A以下調薪金額明細檔
                            //刪除
                            se010DAO.del_TB_S_M_SALARYSET("TB_S_M_SALARYSET_H");
                            se010DAO.del_TB_S_M_SALARYSET("TB_S_M_SALARYSET_D");

                            //新增 TB_S_M_SALARYSET_H	3A以下調薪金額主檔
                            se010DAO.Add_TB_S_M_SALARYSET_H(se010DAO.EFFECT_YM);

                            for (int j = 0; j < excel_data.Rows.Count; j++)
                            {
                                se010DAO.LEVEL_CD = excel_data.Rows[j]["LEVEL_CD"].ToString();
                                se010DAO.GRADE_CD = excel_data.Rows[j]["GRADE_CD"].ToString();
                                se010DAO.EXAMINE_A = excel_data.Rows[j]["EXAMINE_A"].ToString();
                                se010DAO.EXAMINE_B = excel_data.Rows[j]["EXAMINE_B"].ToString();
                                se010DAO.EXAMINE_C = excel_data.Rows[j]["EXAMINE_C"].ToString();
                                se010DAO.EXAMINE_D = excel_data.Rows[j]["EXAMINE_D"].ToString();
                                se010DAO.EXAMINE_E = excel_data.Rows[j]["EXAMINE_E"].ToString();
                                se010DAO.ABILITY_ADJ = excel_data.Rows[j]["ABILITY_ADJ"].ToString();
                                se010DAO.LEVEL_ADJ = excel_data.Rows[j]["LEVEL_ADJ"].ToString();
                                se010DAO.LEVEL_PAY_LOW = excel_data.Rows[j]["LEVEL_PAY_LOW"].ToString();
                                se010DAO.LEVEL_PAY_AVG = excel_data.Rows[j]["LEVEL_PAY_AVG"].ToString();
                                se010DAO.LEVEL_PAY_UP = excel_data.Rows[j]["LEVEL_PAY_UP"].ToString();
                                se010DAO.ORDER_SEQ = "0";

                                //新增 TB_S_M_SALARYSET_D	3A以下調薪金額明細檔
                                se010DAO.Add_TB_S_M_SALARYSET_D();
                            }
                            se010DAO.upd_TB_S_M_SALARYSET_D();

                            Commit();
                        }
                        catch (Exception ex)
                        {
                            RollBack();
                            throw;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            return null;
        }
        catch (Exception)
        {

            throw;
        }
        finally
        {

        }

    }


    #region EXCEL 樣示
    //有底色的的基本款
    private ICellStyle setCellStyle(IWorkbook workbook, string align, bool isBorder, short fontSize, int colorCD)
    {
        return setCellStyle(workbook, align, isBorder, fontSize, colorCD, false);
    }

    //無底色的基本款+字型大小
    private ICellStyle setCellStyle(IWorkbook workbook, string align, bool isBorder, short fontSize)
    {
        return setCellStyle(workbook, align, isBorder, fontSize, 0, false);
    }

    //無底色的基本款
    private ICellStyle setCellStyle(IWorkbook workbook, string align, bool isBorder)
    {
        return setCellStyle(workbook, align, isBorder, 12, 0, false);
    }


    //有粗體,無邊框
    private ICellStyle setCellStyle(IWorkbook workbook, string align, short fontSize, bool isBold)
    {
        return setCellStyle(workbook, align, false, fontSize, 0, isBold);
    }

    //workbook,位置,邊框,字型大小,粗體否
    private ICellStyle setCellStyle(IWorkbook workbook, string align, bool isBorder, short fontSize, bool isBold)
    {
        return setCellStyle(workbook, align, isBorder, fontSize, 0, isBold);
    }

    /// <summary>
    /// 設定資料的格式
    /// </summary>
    /// <param name="workbook"></param>
    /// <param name="align">文字位置</param>
    /// <param name="isBorder">是否要有邊框</param>
    /// <param name="color">背景顏色設定(10:紅,13:黃,14:pink.... )</param>
    /// <returns></returns>
    private ICellStyle setCellStyle(IWorkbook workbook, string align, bool isBorder, short fontSize, int colorCD, bool isBold)
    {
        ICellStyle style = workbook.CreateCellStyle();

        //資料的字型
        IFont cellFont = workbook.CreateFont();
        cellFont.FontName = "微軟正黑體";
        cellFont.FontHeightInPoints = fontSize;  //字型大小
        cellFont.Color = HSSFColor.Black.Index;   //字型顏色

        //是否要有邊框
        if (isBold)
        {
            cellFont.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;   //Bold:粗體字
        }
        else
        {
            cellFont.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Normal;
        }


        style.SetFont(cellFont);
        //是否要有邊框
        if (isBorder)
        {
            //style.BottomBorderColor = HSSFColor.White.Index;
            //style.BorderBottom = BorderStyle.Thin;
            //style.BorderTop = BorderStyle.Thin;
            //style.BorderLeft = BorderStyle.Thin;
            //style.BorderRight = BorderStyle.Thin;
            style.BorderBottom = BorderStyle.Thin;
            style.BorderTop = BorderStyle.None;
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
            //style.FillBackgroundColor = HSSFColor.Yellow.Index;
        }



        return style;
    }

    private ICellStyle setCellStyleTopNone(IWorkbook workbook)
    {
        short fontSize = 8;

        ICellStyle style = workbook.CreateCellStyle();

        //資料的字型
        IFont cellFont = workbook.CreateFont();
        cellFont.FontName = "微軟正黑體";
        cellFont.FontHeightInPoints = fontSize;  //字型大小
        cellFont.Color = HSSFColor.Black.Index;   //字型顏色

        style.SetFont(cellFont);

        style.BorderBottom = BorderStyle.Thin;
        style.BorderTop = BorderStyle.None;
        style.BorderLeft = BorderStyle.Thin;
        style.BorderRight = BorderStyle.Thin;

        style.Alignment = HorizontalAlignment.Center;
        style.VerticalAlignment = VerticalAlignment.Center;
        return style;
    }
    private ICellStyle setCellStyleBottomNone(IWorkbook workbook)
    {
        short fontSize = 8;

        ICellStyle style = workbook.CreateCellStyle();

        //資料的字型
        IFont cellFont = workbook.CreateFont();
        cellFont.FontName = "微軟正黑體";
        cellFont.FontHeightInPoints = fontSize;  //字型大小
        cellFont.Color = HSSFColor.Black.Index;   //字型顏色

        style.SetFont(cellFont);

        style.BorderBottom = BorderStyle.None;
        style.BorderTop = BorderStyle.Thin;
        style.BorderLeft = BorderStyle.Thin;
        style.BorderRight = BorderStyle.Thin;

        style.Alignment = HorizontalAlignment.Center;
        style.VerticalAlignment = VerticalAlignment.Center;
        return style;
    }

    #endregion


}