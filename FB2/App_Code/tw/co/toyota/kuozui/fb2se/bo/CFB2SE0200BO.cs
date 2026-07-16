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
/// CFB2SE0200BO 的摘要描述
/// </summary>
public class CFB2SE0200BO : BaseService
{
	public CFB2SE0200BO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}
    public string Add(CFB2SE0200DAO fb2se, DataTable dtSend, string EFFECT_YM)
    {
        try
        {
            //取得現有資料(檢查重複)
            //DataTable tmp = fb2se.getExistData();
            BeginTransaction();
            fb2se.Add_TB_S_M_2BSALARY_SET_H(EFFECT_YM);
            foreach (DataRow dr in dtSend.Rows)
            {
                fb2se.EFFECT_YM = Convert.ToString(dr["EFFECT_YM"]);
                fb2se.LEVEL_CD = Convert.ToString(dr["LEVEL_CD"]);
                fb2se.PJOB_TYPE = Convert.ToString(dr["PJOB_TYPE"]);                
                fb2se.EXAMINE_A = Convert.ToString(dr["EXAMINE_A"]);
                fb2se.EXAMINE_B = Convert.ToString(dr["EXAMINE_B"]);
                fb2se.EXAMINE_C1 = Convert.ToString(dr["EXAMINE_C1"]);
                fb2se.EXAMINE_C2 = Convert.ToString(dr["EXAMINE_C2"]);
                fb2se.EXAMINE_D = Convert.ToString(dr["EXAMINE_D"]);
                fb2se.EXAMINE_E = Convert.ToString(dr["EXAMINE_E"]);
                fb2se.ORDER_SEQ = Convert.ToString(dr["ORDER_SEQ"]);

                fb2se.Add_TB_S_M_2BSALARYSET_D();
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
    public string Update(CFB2SE0200DAO fb2se)
    {
        try
        {
            BeginTransaction();
            fb2se.Update_TB_S_M_2BSALARY_SET_D();
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
    public IWorkbook createExcelResult(string excelPath, CFB2SE0200DAO se020DAO)
    {

        FileStream fs = null;
        IWorkbook workbook = null;

        //取得範本sheet
        ISheet sheet = null;
        try
        {
            //取得下載資料
            DataTable dt = se020DAO.getExcelResultData();

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

                        //職務區分
                        cell = row.CreateCell(2);
                        cell.CellStyle = stringLeftStyle;  //先
                        cell.SetCellValue(dt.Rows[i]["PJOB_TYPE"].ToString()); //後                       

                        //金額
                        for (int j = 3; j <= 13; j++)
                        {
                            cell = row.CreateCell(j);
                            cell.CellStyle = stringRightStyle;
                            cell.SetCellValue(0);
                        }

                    }
                    //製表日期
                    ICellStyle stringLeftStyleDate = this.setCellStyle(workbook, "left", false, 14);
                    row = sheet.GetRow(0);
                    cell = row.CreateCell(14);
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
    public IWorkbook uploadExcel1(Stream fs, string type, CFB2SE0200DAO se020DAO)
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
            string str_PJOB_TYPE = se020DAO.getScore_Str();//所有職務區分代碼

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
                    excel_data.Columns.Add("PJOB_TYPE", System.Type.GetType("System.String")); //職務區分
                    excel_data.Columns.Add("EXAMINE_S", System.Type.GetType("System.String"));
                    excel_data.Columns.Add("EXAMINE_A", System.Type.GetType("System.String"));
                    excel_data.Columns.Add("EXAMINE_B", System.Type.GetType("System.String"));
                    excel_data.Columns.Add("EXAMINE_C", System.Type.GetType("System.String"));
                    excel_data.Columns.Add("EXAMINE_D", System.Type.GetType("System.String"));
                    excel_data.Columns.Add("EXAMINE_E", System.Type.GetType("System.String"));
                    excel_data.Columns.Add("EXAMINE_F", System.Type.GetType("System.String"));
                    excel_data.Columns.Add("EXAMINE_G", System.Type.GetType("System.String"));
                    excel_data.Columns.Add("EXAMINE_H", System.Type.GetType("System.String"));
                    excel_data.Columns.Add("EXAMINE_I", System.Type.GetType("System.String"));
                    excel_data.Columns.Add("EXAMINE_J", System.Type.GetType("System.String"));
                    //存放EXCEL 檢查能否重複的資料
                    DataRow excel_pk_row;
                    excel_pk_data.Columns.Add("LEVEL_CD", System.Type.GetType("System.String"));
                    excel_pk_data.Columns.Add("PJOB_TYPE", System.Type.GetType("System.String"));

                    #endregion

                    //2.取得excel的資料
                    string cell_LEVEL_CD = "";	//資格代號
                    string cell_PJOB_TYPE = "";	//職務區分
                    string cell_EXAMINE_S = "";	//	考績E調額
                    string cell_EXAMINE_A = "";	//	考績A調額
                    string cell_EXAMINE_B = "";	//	考績B調額
                    string cell_EXAMINE_C = "";	//	考績C調額
                    string cell_EXAMINE_D = "";	//	考績D調額
                    string cell_EXAMINE_E = "";	//	考績E調額
                    string cell_EXAMINE_F = "";	//	考績E調額
                    string cell_EXAMINE_G = "";	//	考績E調額
                    string cell_EXAMINE_H = "";	//	考績E調額
                    string cell_EXAMINE_I = "";	//	考績E調額
                    string cell_EXAMINE_J = "";	//	考績E調額


                    string error = "";

                    //巡覽每row的資料第一列為title跳過(故i從3開始)
                    for (int i = 1; i <= sheet.LastRowNum; i++)
                    {
                        error = "";
                        if (sheet.GetRow(i) != null)
                        {
                            cell_LEVEL_CD = sheet.GetRow(i).GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            cell_PJOB_TYPE = sheet.GetRow(i).GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            cell_EXAMINE_S = sheet.GetRow(i).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().Replace(",", "");
                            cell_EXAMINE_A = sheet.GetRow(i).GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().Replace(",", "");
                            cell_EXAMINE_B = sheet.GetRow(i).GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().Replace(",", "");
                            cell_EXAMINE_C = sheet.GetRow(i).GetCell(6, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().Replace(",", "");
                            cell_EXAMINE_D = sheet.GetRow(i).GetCell(7, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().Replace(",", "");
                            cell_EXAMINE_E = sheet.GetRow(i).GetCell(8, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().Replace(",", "");
                            cell_EXAMINE_F = sheet.GetRow(i).GetCell(9, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().Replace(",", "");
                            cell_EXAMINE_G = sheet.GetRow(i).GetCell(10, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().Replace(",", "");
                            cell_EXAMINE_H = sheet.GetRow(i).GetCell(11, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().Replace(",", "");
                            cell_EXAMINE_I = sheet.GetRow(i).GetCell(12, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().Replace(",", "");
                            cell_EXAMINE_J = sheet.GetRow(i).GetCell(13, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().Replace(",", "");

                            //檢查文字欄位
                            error += utilities.checkLength(cell_LEVEL_CD, "資格", 3, false);
                            error += utilities.checkLength(cell_PJOB_TYPE, "職務區分",1 , true);

                            //金額欄位
                            error += utilities.checkNumber(cell_EXAMINE_S, "S金額", 7, false);
                            error += utilities.checkNumber(cell_EXAMINE_A, "A金額", 7, false);
                            error += utilities.checkNumber(cell_EXAMINE_B, "B金額", 7, false);
                            error += utilities.checkNumber(cell_EXAMINE_C, "C金額", 7, false);
                            error += utilities.checkNumber(cell_EXAMINE_D, "D金額", 7, false);
                            error += utilities.checkNumber(cell_EXAMINE_E, "E金額", 7, false);
                            error += utilities.checkNumber(cell_EXAMINE_F, "F金額", 7, false);
                            error += utilities.checkNumber(cell_EXAMINE_G, "G金額", 7, false);
                            error += utilities.checkNumber(cell_EXAMINE_H, "H金額", 7, false);
                            error += utilities.checkNumber(cell_EXAMINE_I, "I金額", 7, false);
                            error += utilities.checkNumber(cell_EXAMINE_J, "J金額", 7, false);
   

                            //資格級數是否存在
                            if (cell_LEVEL_CD != "" && se020DAO.chklevelcd(cell_LEVEL_CD) == 0)
                            {
                                error += "資格不存在\n";
                            }
                            //資格級數是否存在
                            if (cell_PJOB_TYPE != "" && str_PJOB_TYPE.IndexOf(cell_PJOB_TYPE) < 0)
                            {
                                error += "職務區分不存在\n";
                            }


                            //若有值,檢查資格級數是否重覆
                            excel_pk_arr[0] = cell_LEVEL_CD;
                            excel_pk_arr[1] = cell_PJOB_TYPE;
                            if (excel_pk_data.Rows.Count > 0)
                            {
                                dr = excel_pk_data.Rows.Find(excel_pk_arr);
                                if (dr != null)
                                {
                                    error += "此EXCEL有相同的資格,職務區分\n";
                                }
                                else
                                {
                                    excel_pk_row = excel_pk_data.NewRow();
                                    excel_pk_row["LEVEL_CD"] = cell_LEVEL_CD;
                                    excel_pk_row["PJOB_TYPE"] = cell_PJOB_TYPE;
                                    excel_pk_data.Rows.Add(excel_pk_row);
                                    excel_pk_data.PrimaryKey = new DataColumn[] { excel_pk_data.Columns["LEVEL_CD"], excel_pk_data.Columns["PJOB_TYPE"], };
                                }
                            }
                            else
                            {
                                excel_pk_row = excel_pk_data.NewRow();
                                excel_pk_row["LEVEL_CD"] = cell_LEVEL_CD;
                                excel_pk_row["PJOB_TYPE"] = cell_PJOB_TYPE;
                                excel_pk_data.Rows.Add(excel_pk_row);
                                excel_pk_data.PrimaryKey = new DataColumn[] { excel_pk_data.Columns["LEVEL_CD"], excel_pk_data.Columns["PJOB_TYPE"], };
                            }

                            excel_row = excel_data.NewRow();
                            excel_row["LEVEL_CD"] = cell_LEVEL_CD;
                            excel_row["PJOB_TYPE"] = cell_PJOB_TYPE;
                            excel_row["EXAMINE_S"] = cell_EXAMINE_S;
                            excel_row["EXAMINE_A"] = cell_EXAMINE_A;
                            excel_row["EXAMINE_B"] = cell_EXAMINE_B;
                            excel_row["EXAMINE_C"] = cell_EXAMINE_C;
                            excel_row["EXAMINE_D"] = cell_EXAMINE_D;
                            excel_row["EXAMINE_E"] = cell_EXAMINE_E;
                            excel_row["EXAMINE_F"] = cell_EXAMINE_F;
                            excel_row["EXAMINE_G"] = cell_EXAMINE_G;
                            excel_row["EXAMINE_H"] = cell_EXAMINE_H;
                            excel_row["EXAMINE_I"] = cell_EXAMINE_I;
                            excel_row["EXAMINE_J"] = cell_EXAMINE_J;

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
                        se020DAO.CREATED_BY = SessionHandle.Current.emp_id;
                        se020DAO.UPDATED_BY = SessionHandle.Current.emp_id;
                        se020DAO.FUNC_ID = "FB2SE020";
                        try
                        {
                            BeginTransaction();
                            //TB_S_M_2BSALARY_SET_H	2B以上本薪調整主檔
                            //TB_S_M_2BSALARY_SET_D	2B以上本薪調整明細檔
                            //刪除
                            se020DAO.del_TB_S_M_SALARYSET("TB_S_M_2BSALARY_SET_H");
                            se020DAO.del_TB_S_M_SALARYSET("TB_S_M_2BSALARY_SET_D");

                            //新增 TB_S_M_SALARYSET_H	3A以下調薪金額主檔
                            se020DAO.Add_TB_S_M_2BSALARY_SET_H(se020DAO.EFFECT_YM);

                            for (int j = 0; j < excel_data.Rows.Count; j++)
                            {
                                se020DAO.LEVEL_CD = excel_data.Rows[j]["LEVEL_CD"].ToString();
                                se020DAO.PJOB_TYPE = excel_data.Rows[j]["PJOB_TYPE"].ToString();
                                se020DAO.EXAMINE_S = excel_data.Rows[j]["EXAMINE_S"].ToString();
                                se020DAO.EXAMINE_A = excel_data.Rows[j]["EXAMINE_A"].ToString();
                                se020DAO.EXAMINE_B = excel_data.Rows[j]["EXAMINE_B"].ToString();
                                se020DAO.EXAMINE_C = excel_data.Rows[j]["EXAMINE_C"].ToString();
                                se020DAO.EXAMINE_D = excel_data.Rows[j]["EXAMINE_D"].ToString();
                                se020DAO.EXAMINE_E = excel_data.Rows[j]["EXAMINE_E"].ToString();
                                se020DAO.EXAMINE_F = excel_data.Rows[j]["EXAMINE_F"].ToString();
                                se020DAO.EXAMINE_G = excel_data.Rows[j]["EXAMINE_G"].ToString();
                                se020DAO.EXAMINE_H = excel_data.Rows[j]["EXAMINE_H"].ToString();
                                se020DAO.EXAMINE_I = excel_data.Rows[j]["EXAMINE_I"].ToString();
                                se020DAO.EXAMINE_J = excel_data.Rows[j]["EXAMINE_J"].ToString();
                                se020DAO.ORDER_SEQ = "0";

                                //新增 TB_S_M_2BSALARY_SET_D	2B以上本薪調整明細檔
                                se020DAO.insert_TB_S_M_2BSALARYSET_D();
                            }
                            se020DAO.upd_TB_S_M_SALARYSET_D();

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