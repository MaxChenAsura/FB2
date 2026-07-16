using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using FB2.tw.co.toyota.kuozui.bo;
using System.Drawing;
using NPOI.SS.Util;
using NPOI.HSSF.Util;
using System.IO;

/// <summary>
/// CFB2IA1100BO 的摘要描述
/// </summary>
public class CFB2IA1100BO : BaseService
{
    public CFB2IA1100BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    //加保三合一 excel
    public IWorkbook createWFB2IA1100Excel(CFB2IA1100DAO wfb2ia, string excelPath, string type)
    {
        IWorkbook workbook = null;
        ISheet sheet = null;
        try
        {
            FileStream fs = new FileStream(excelPath, FileMode.Open, FileAccess.Read);
            //依type判斷要用哪種方式產生
            if (type == "xls")
                workbook = new HSSFWorkbook(fs);
            else
                workbook = new XSSFWorkbook(fs);

            //取得範本sheet
            sheet = workbook.GetSheetAt(0);

            ICellStyle style1;
            ICellStyle style2;
            ICellStyle style4;
            DataTable tmp = wfb2ia.searchResult();
            if (tmp.Rows.Count > 0)
            {
                style1 = workbook.CreateCellStyle();

                IFont font1 = workbook.CreateFont();
                font1.FontName = "新細明體";
                font1.FontHeightInPoints = 12;
                style1.SetFont(font1);

                IFont font2 = workbook.CreateFont();
                font2.FontName = "新細明體";
                font2.FontHeightInPoints = 12;
                font2.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;   //bold:粗體字

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

                IRow row;
                ICell cell;
                ICellStyle stringLeftStyle = this.setCellStyle(workbook, "left", true);
                ICellStyle stringRightStyle = this.setCellStyle(workbook, "right", true);
                ICellStyle stringCenterStyle = this.setCellStyle(workbook, "center", true);

                ICellStyle stringBottomStyle;
                stringBottomStyle = workbook.CreateCellStyle();
                stringBottomStyle.BorderBottom = BorderStyle.Thick;
                stringBottomStyle.BorderLeft = BorderStyle.Thick;
                stringBottomStyle.BorderRight = BorderStyle.Thick;
                stringBottomStyle.BorderTop = BorderStyle.Thick;
                stringBottomStyle.VerticalAlignment = VerticalAlignment.Bottom;
                stringBottomStyle.Alignment = HorizontalAlignment.Center;
                stringBottomStyle.WrapText = true;
                stringBottomStyle.SetFont(font2);

                row = sheet.GetRow(0);
                for (int i = 0; i < 26; i++)
                {
                    cell = row.GetCell(i);
                    cell.CellStyle = stringBottomStyle;
                }

                style2 = workbook.CreateCellStyle();
                ((XSSFCellStyle)style2).BorderBottom = BorderStyle.Thin;
                ((XSSFCellStyle)style2).BorderLeft = BorderStyle.Thin;
                ((XSSFCellStyle)style2).BorderRight = BorderStyle.Thin;
                ((XSSFCellStyle)style2).BorderTop = BorderStyle.Thin;
                style2.SetFont(font1);

                string dtFormat = "";
                int x = 0;
                for (int i = 0; i < tmp.Rows.Count; i++)
                {
                    x++;
                    row = sheet.CreateRow(x);
                    cell = row.CreateCell(0);
                    cell.CellStyle = stringLeftStyle;
                    cell.SetCellValue(tmp.Rows[i]["CHG_APP_TYPE"].ToString());

                    cell = row.CreateCell(1);
                    cell.CellStyle = stringLeftStyle;
                    cell.SetCellValue(tmp.Rows[i]["LAB8"].ToString());

                    cell = row.CreateCell(2);
                    cell.CellStyle = stringLeftStyle;
                    cell.SetCellValue(tmp.Rows[i]["LAB_CHK_CD"].ToString());

                    cell = row.CreateCell(3);
                    cell.CellStyle = stringLeftStyle;
                    cell.SetCellValue(tmp.Rows[i]["HEALTH_ORG_ID"].ToString());

                    cell = row.CreateCell(4);
                    cell.CellStyle = stringLeftStyle;
                    cell.SetCellValue(tmp.Rows[i]["INS_HLR_TYPE"].ToString());

                    cell = row.CreateCell(5);
                    cell.CellStyle = stringLeftStyle;
                    cell.SetCellValue(tmp.Rows[i]["IDENTITY_KIND"].ToString());

                    cell = row.CreateCell(6);
                    cell.CellStyle = stringLeftStyle;
                    cell.SetCellValue(tmp.Rows[i]["LAB_FORIGN_YN"].ToString());

                    cell = row.CreateCell(7);
                    cell.CellStyle = stringLeftStyle;
                    cell.SetCellValue(tmp.Rows[i]["LICENSE_ID"].ToString());

                    cell = row.CreateCell(8);
                    cell.CellStyle = stringLeftStyle;
                    cell.SetCellValue(tmp.Rows[i]["EMP_NAME"].ToString());

                    //被保險人出生日期(需轉民國年yyyMMdd)
                    cell = row.CreateCell(9);
                    cell.CellStyle = stringLeftStyle;
                    dtFormat =
                        tmp.Rows[i]["EMP_BIRTH_DT"].ToString() != "" ? utilities.DateToTw(Convert.ToDateTime(tmp.Rows[i]["EMP_BIRTH_DT"].ToString()).ToString("yyyy/MM/dd"), "") : "";
                    cell.SetCellValue(dtFormat);

                    cell = row.CreateCell(10);
                    cell.CellStyle = stringRightStyle;
                    cell.SetCellValue(tmp.Rows[i]["SALARY"].ToString());

                    cell = row.CreateCell(11);
                    cell.CellStyle = stringRightStyle;
                    cell.SetCellValue(tmp.Rows[i]["HEA_AFT_AMT"].ToString());

                    cell = row.CreateCell(12);
                    cell.CellStyle = stringLeftStyle;
                    cell.SetCellValue(tmp.Rows[i]["SPTYP"].ToString());

                    //勞基法特殊身份別
                    cell = row.CreateCell(13);
                    cell.CellStyle = stringLeftStyle;
                    cell.SetCellValue("");

                    cell = row.CreateCell(14);
                    cell.CellStyle = stringLeftStyle;
                    cell.SetCellValue(tmp.Rows[i]["F_LICENCE_CD"].ToString());

                    cell = row.CreateCell(15);
                    cell.CellStyle = stringLeftStyle;
                    cell.SetCellValue(tmp.Rows[i]["F_NAME"].ToString());

                    //眷屬出生日期(需轉民國年yyyMMdd)
                    cell = row.CreateCell(16);
                    cell.CellStyle = stringLeftStyle;
                    dtFormat =
                        tmp.Rows[i]["F_BIRTH_DT"].ToString() != "" ? utilities.DateToTw(Convert.ToDateTime(tmp.Rows[i]["F_BIRTH_DT"].ToString()).ToString("yyyy/MM/dd"), "") : "";
                    cell.SetCellValue(dtFormat);

                    cell = row.CreateCell(17);
                    cell.CellStyle = stringLeftStyle;
                    cell.SetCellValue(tmp.Rows[i]["FAMILY_RELATION"].ToString());

                    cell = row.CreateCell(18);
                    cell.CellStyle = stringLeftStyle;
                    cell.SetCellValue(tmp.Rows[i]["HEALTH_BUSINESS_ID"].ToString());

                    cell = row.CreateCell(19);
                    cell.CellStyle = stringLeftStyle;
                    cell.SetCellValue(tmp.Rows[i]["CHG_TYPE"].ToString());

                    //健保加保原因發生日期(需轉民國年yyyMMdd)
                    cell = row.CreateCell(20);
                    cell.CellStyle = stringLeftStyle;
                    dtFormat =
                        tmp.Rows[i]["HEA_CHT_DT"].ToString() != "" ? utilities.DateToTw(Convert.ToDateTime(tmp.Rows[i]["HEA_CHT_DT"].ToString()).ToString("yyyy/MM/dd"), "") : "";
                    cell.SetCellValue(dtFormat);

                    cell = row.CreateCell(21);
                    cell.CellStyle = stringLeftStyle;
                    cell.SetCellValue(tmp.Rows[i]["INS_SEX"].ToString());

                    cell = row.CreateCell(22);
                    cell.CellStyle = stringLeftStyle;
                    cell.SetCellValue(tmp.Rows[i]["RET_DIFFENCT_TYPE"].ToString());

                    cell = row.CreateCell(23);
                    cell.CellStyle = stringRightStyle;
                    cell.SetCellValue(tmp.Rows[i]["BOSS_RATE"].ToString());

                    cell = row.CreateCell(24);
                    cell.CellStyle = stringRightStyle;
                    cell.SetCellValue(tmp.Rows[i]["SEFT_RATE"].ToString());

                    //勞退提繳日期(需轉民國年yyyMMdd)
                    cell = row.CreateCell(25);
                    cell.CellStyle = stringLeftStyle;
                    dtFormat = tmp.Rows[i]["RET_DT"].ToString() != "" ? utilities.DateToTw(Convert.ToDateTime(tmp.Rows[i]["RET_DT"].ToString()).ToString("yyyy/MM/dd"), "") : "";
                    cell.SetCellValue(dtFormat);
                }
                return workbook;
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
        finally
        {
            sheet = null;
        }

    }

    //退保三合一 excel
    public IWorkbook createWFB2IA1101Excel(CFB2IA1100DAO wfb2ia, string excelPath, string type)
    {
        IWorkbook workbook = null;
        ISheet sheet = null;
        try
        {
            FileStream fs = new FileStream(excelPath, FileMode.Open, FileAccess.Read);
            //依type判斷要用哪種方式產生
            if (type == "xls")
                workbook = new HSSFWorkbook(fs);
            else
                workbook = new XSSFWorkbook(fs);

            //取得範本sheet
            sheet = workbook.GetSheetAt(0);

            ICellStyle style1;
            ICellStyle style2;
            ICellStyle style4;
            DataTable tmp = wfb2ia.searchResult2();
            if (tmp.Rows.Count > 0)
            {
                style1 = workbook.CreateCellStyle();

                IFont font1 = workbook.CreateFont();
                font1.FontName = "新細明體";
                font1.FontHeightInPoints = 12;
                style1.SetFont(font1);
 
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

                IRow row;
                ICell cell;
                ICellStyle stringLeftStyle = this.setCellStyle(workbook, "left", true);
                ICellStyle stringRightStyle = this.setCellStyle(workbook, "right", true);
                ICellStyle stringCenterStyle = this.setCellStyle(workbook, "center", true);

                
                //政府用,不須製表日期
                //cell = row.CreateCell(14);
                //cell.CellStyle = style1;
                //cell.SetCellValue("製表日期：" + DateTime.Now.ToString("yyyy/MM/dd"));
                //sheet.AddMergedRegion(new CellRangeAddress(0, 0, 14, 15));

                style2 = workbook.CreateCellStyle();
                ((XSSFCellStyle)style2).BorderBottom = BorderStyle.Thin;
                ((XSSFCellStyle)style2).BorderLeft = BorderStyle.Thin;
                ((XSSFCellStyle)style2).BorderRight = BorderStyle.Thin;
                ((XSSFCellStyle)style2).BorderTop = BorderStyle.Thin;
                style2.SetFont(font1);

                string dtFormat = "";
                int x = 0;
                for (int i = 0; i < tmp.Rows.Count; i++)
                {
                    x++;
                    row = sheet.CreateRow(x);
                    cell = row.CreateCell(0);
                    cell.CellStyle = stringLeftStyle;
                    cell.SetCellValue(tmp.Rows[i]["CHG_APP_TYPE"].ToString());

                    cell = row.CreateCell(1);
                    cell.CellStyle = stringLeftStyle;
                    cell.SetCellValue(tmp.Rows[i]["LAB8"].ToString());

                    cell = row.CreateCell(2);
                    cell.CellStyle = stringLeftStyle;
                    cell.SetCellValue(tmp.Rows[i]["LAB_CHK_CD"].ToString());

                    cell = row.CreateCell(3);
                    cell.CellStyle = stringLeftStyle;
                    cell.SetCellValue(tmp.Rows[i]["HEALTH_ORG_ID"].ToString());

                    cell = row.CreateCell(4);
                    cell.CellStyle = stringLeftStyle;
                    cell.SetCellValue(tmp.Rows[i]["HEALTH_BUSINESS_ID"].ToString());

                    cell = row.CreateCell(5);
                    cell.CellStyle = stringLeftStyle;
                    cell.SetCellValue(tmp.Rows[i]["INS_HLR_TYPE"].ToString());

                    cell = row.CreateCell(6);
                    cell.CellStyle = stringLeftStyle;
                    cell.SetCellValue(tmp.Rows[i]["IDENTITY_KIND"].ToString());

                    cell = row.CreateCell(7);
                    cell.CellStyle = stringLeftStyle;
                    cell.SetCellValue(tmp.Rows[i]["LAB_FORIGN_YN"].ToString());

                    cell = row.CreateCell(8);
                    cell.CellStyle = stringLeftStyle;
                    cell.SetCellValue(tmp.Rows[i]["EMP_NAME"].ToString());

                    cell = row.CreateCell(9);
                    cell.CellStyle = stringLeftStyle;
                    cell.SetCellValue(tmp.Rows[i]["LICENSE_ID1"].ToString());

                    cell = row.CreateCell(10);
                    cell.CellStyle = stringLeftStyle;
                    cell.SetCellValue(tmp.Rows[i]["LICENSE_ID2"].ToString());

                    //被保險人出生日期(需轉民國年yyyMMdd)
                    cell = row.CreateCell(11);
                    cell.CellStyle = stringLeftStyle;
                    dtFormat =
                        tmp.Rows[i]["EMP_BIRTH_DT"].ToString() != "" ? utilities.DateToTw(Convert.ToDateTime(tmp.Rows[i]["EMP_BIRTH_DT"].ToString()).ToString("yyyy/MM/dd"), "") : "";
                    cell.SetCellValue(dtFormat);

                    cell = row.CreateCell(12);
                    cell.CellStyle = stringLeftStyle;
                    cell.SetCellValue(tmp.Rows[i]["CHG_TYPE"].ToString());

                    cell = row.CreateCell(13);
                    cell.CellStyle = stringLeftStyle;
                    cell.SetCellValue(tmp.Rows[i]["CHG_REASON_CD"].ToString());

                    cell = row.CreateCell(14);
                    cell.CellStyle = stringLeftStyle;
                    cell.SetCellValue(tmp.Rows[i]["CHG_REASON_CD_DESC"].ToString());

                    //健保退保日期(需轉民國年yyyMMdd)
                    cell = row.CreateCell(15);
                    cell.CellStyle = stringLeftStyle;
                    dtFormat =
                        tmp.Rows[i]["HEA_CHT_DT"].ToString() != "" ? utilities.DateToTw(Convert.ToDateTime(tmp.Rows[i]["HEA_CHT_DT"].ToString()).ToString("yyyy/MM/dd"), "") : "";
                    cell.SetCellValue(dtFormat);
                }
                return workbook;
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
        finally
        {
            sheet = null;
        }

    }

    /// <summary>
    /// 設定資料的格式
    /// </summary>
    /// <param name="workbook"></param>
    /// <param name="align">文字位置</param>
    /// <param name="isBorder">是否要有邊框</param>
    /// <returns></returns>
    private ICellStyle setCellStyle(IWorkbook workbook, string align, bool isBorder)
    {
        return setCellStyle(workbook, align, isBorder, 0);
    }

    /// <summary>
    /// 設定資料的格式
    /// </summary>
    /// <param name="workbook"></param>
    /// <param name="align">文字位置</param>
    /// <param name="isBorder">是否要有邊框</param>
    /// <param name="color">背景顏色設定(10:紅,13:黃,14:pink.... )</param>
    /// <returns></returns>
    private ICellStyle setCellStyle(IWorkbook workbook, string align, bool isBorder, int colorCD)
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
            //style.BottomBorderColor = HSSFColor.White.Index;
            style.BorderBottom = BorderStyle.Thin;
            style.BorderTop = BorderStyle.Thin;
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
        else if (align.ToLower() == "bottom")
        {
            style.VerticalAlignment = VerticalAlignment.Bottom;
            style.Alignment = HorizontalAlignment.Center;
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

    //取得伙食津貼
    public int getFOOD_SUBSIDY(CFB2IA1100DAO wfb2ia)
    {
        try
        {
            return wfb2ia.getFOOD_SUBSIDY();
        }
        catch (Exception)
        {

            throw;
        }
    }

    //勞退個人自提率是否超出勞退自提上限
    public bool isPENSION_SELF_RATIO(double ratio)
    {
        try
        {
            CFB2IA1100DAO wfb2ia = new CFB2IA1100DAO();

            double MaxPENSION_SELF_RATIO = wfb2ia.getMaxPENSION_SELF_RATIO();
            if (ratio > MaxPENSION_SELF_RATIO)
            {
                return true;
            }

            return false;
        }
        catch (Exception)
        {

            throw;
        }
    }

    //取得被保險人出生日期
    public string getEMP_BIRTH_DT(string emp_id)
    {
        try
        {
            CFB2IA1100DAO wfb2ia = new CFB2IA1100DAO();
            wfb2ia.EMP_ID = emp_id;
            return wfb2ia.getEMP_BIRTH_DT();
        }
        catch (Exception)
        {

            throw;
        }
    }

    //判斷團保是否已加保
    public bool isAbnormalGINS(string gins_kind, CFB2IA1100DAO wfb2ia, string msg)
    {
        try
        {
            bool b = false;
            BeginTransaction();
            DataTable tmp = wfb2ia.getGROUP_TXNData(gins_kind);
            if (tmp.Rows.Count > 0)
            {
                //紀錄團保處理異常
                wfb2ia.updateCHG_TXN(msg);
                
                b = true;
            }
            Commit();
            return b;
        }
        catch (Exception)
        {
            RollBack();
            throw;
        }
    }

    //判斷勞保、健保、勞退是否已加保
    public bool isAbnormal(string ins_type, CFB2IA1100DAO wfb2ia, string chg_dt,
        string msg, string operation_kind)
    {
        try
        {
            bool b = false;
            BeginTransaction();
            DataTable tmp = wfb2ia.get3IN1_TXNData(ins_type, chg_dt, operation_kind);
            if (tmp.Rows.Count > 0)
            {
                //紀錄勞保、健保、勞退處理異常
                wfb2ia.updateCHG_TXN(msg);

                b = true;
            }
            Commit();
            return b;
        }
        catch (Exception)
        {
            RollBack();
            throw;
        }
    }

    //判斷勞保、健保、勞退是否已退保
    public bool isAbnormal2(string ins_type, CFB2IA1100DAO wfb2ia, string msg)
    {
        try
        {
            bool b = false;
            BeginTransaction();
            DataTable tmp = wfb2ia.get3IN1_TXNData(ins_type);
            if (tmp.Rows.Count == 0)
            {
                //紀錄勞保、健保、勞退處理異常
                wfb2ia.updateCHG_TXN(msg);
                
                b = true;
            }
            Commit();
            return b;
        }
        catch (Exception)
        {
            RollBack();
            throw;
        }
    }

    //判斷團保是否已退保
    public bool isAbnormalGINS2(string gins_kind, CFB2IA1100DAO wfb2ia, string msg)
    {
        try
        {
            bool b = false;
            BeginTransaction();
            DataTable tmp = wfb2ia.getGROUP_TXNData2(gins_kind);
            if (tmp.Rows.Count == 0)
            {                
                //紀錄團保處理異常
                wfb2ia.updateCHG_TXN(msg);
                
                b = true;
            }
            Commit();
            return b;
        }
        catch (Exception)
        {
            RollBack();
            throw;
        }
    }

    //更新 [TB_I_M_CHG_TXN 保險一括異動記錄檔]
    public string updateCHG_TXN(CFB2IA1100DAO wfb2ia)
    {
        try
        {
            BeginTransaction();

            wfb2ia.updateCHG_TXN();

            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    //新增[TB_I_M_3IN1_TXN 勞保健保勞退履歷主檔 ]
    public string insert3IN1_TXN(CFB2IA1100DAO wfb2ia)
    {
        try
        {
            BeginTransaction();
            wfb2ia.insert3IN1_TXN(wfb2ia);
            Commit();

            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    //新增[TB_I_M_RETIRE_SELFRATE 勞退自提履歷檔 ]
    public string insertRETIRE_SELFRATE(CFB2IA1100DAO wfb2ia)
    {
        try
        {
            BeginTransaction();
            wfb2ia.insertRETIRE_SELFRATE(wfb2ia);
            Commit();

            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    //新增[TB_I_M_GROUP_TXN 團保主檔 ]
    public string insertGROUP_TXN(CFB2IA1100DAO wfb2ia)
    {
        try
        {
            BeginTransaction();
            wfb2ia.insertGROUP_TXN(wfb2ia);
            Commit();

            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    //新增[TB_I_R_3IN1_REPORTDATA保險三合申報檔資料]
    public string insert3IN1_REPORTDATA(CFB2IA1100DAO wfb2ia)
    {
        try
        {
            BeginTransaction();
            wfb2ia.insert3IN1_REPORTDATA(wfb2ia);
            Commit();

            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    //取得性別 // "M" //男  else "F" //女
    public string getINS_SEX(string emp_id)
    {
        try
        {
            CFB2IA1100DAO wfb2ia = new CFB2IA1100DAO();
            wfb2ia.EMP_ID = emp_id;
            return wfb2ia.getINS_SEX();
        }
        catch (Exception)
        {

            throw;
        }
    }

    //取得勞退提繳身份別
    public string getIS_MASTER(string emp_id)
    {
        try
        {
            CFB2IA1100DAO wfb2ia = new CFB2IA1100DAO();
            wfb2ia.EMP_ID = emp_id;
            return wfb2ia.getIS_MASTER();
        }
        catch (Exception)
        {

            throw;
        }
    }

    //取得雇主提撥率% 
    public string getINSC_COMP_RATE()
    {
        try
        {
            CFB2IA1100DAO wfb2ia = new CFB2IA1100DAO();
            return wfb2ia.getINSC_COMP_RATE();
        }
        catch (Exception)
        {

            throw;
        }
    }

    //TB_I_M_3IN1_TXN.生效日期迄='9999/12/31' //表示該建教生未退保就轉正社員
    public bool isMaxEFFECT_EDT(string license_id, string emp_id)
    {
        try
        {
            CFB2IA1100DAO wfb2ia = new CFB2IA1100DAO();
            wfb2ia.LICENSE_ID = license_id;
            wfb2ia.EMP_ID = emp_id;
            return wfb2ia.isMaxEFFECT_EDT();
        }
        catch (Exception)
        {

            throw;
        }
    }

    //異動[TB_I_M_3IN1_TXN 勞保健保勞退履歷主檔 ] //勞保退保
    public string update3IN1_TXN_A(CFB2IA1100DAO wfb2ia)
    {
        try
        {
            BeginTransaction();

            wfb2ia.update3IN1_TXN_A(wfb2ia);

            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    //異動[TB_I_M_3IN1_TXN 勞保健保勞退履歷主檔 ] //健保退保
    public string update3IN1_TXN_B(CFB2IA1100DAO wfb2ia)
    {
        try
        {
            BeginTransaction();

            wfb2ia.update3IN1_TXN_B(wfb2ia);

            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    //取得健保加保/退保原因別
    public string getTMPLEATAB(string hr_chg_cd)
    {
        try
        {
            CFB2IA1100DAO wfb2ia = new CFB2IA1100DAO();
            wfb2ia.HR_CHG_CD = hr_chg_cd;
            return wfb2ia.getTMPLEATAB();
        }
        catch (Exception)
        {

            throw;
        }
    }

    //異動[TB_I_M_3IN1_TXN 勞保健保勞退履歷主檔 ] //勞退退保
    public string update3IN1_TXN_C(CFB2IA1100DAO wfb2ia)
    {
        try
        {
            BeginTransaction();

            wfb2ia.update3IN1_TXN_C(wfb2ia);

            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    //異動[TB_I_M_RETIRE_SELFRATE 勞退自提履歷檔 ] //勞退退保
    public string updateRETIRE_SELFRATE(CFB2IA1100DAO wfb2ia)
    {
        try
        {
            BeginTransaction();

            wfb2ia.updateRETIRE_SELFRATE(wfb2ia);

            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    //異動[TB_I_M_GROUP_TXN 團保主檔 ] //團保退保
    public string updateGROUP_TXN(CFB2IA1100DAO wfb2ia)
    {
        try
        {
            BeginTransaction();

            wfb2ia.updateGROUP_TXN(wfb2ia);

            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    //新增[TB_I_R_3IN1_REPORTDATA保險三合申報檔資料] //退保
    public string insert3IN1_REPORTDATA2(CFB2IA1100DAO wfb2ia)
    {
        try
        {
            BeginTransaction();
            wfb2ia.insert3IN1_REPORTDATA2(wfb2ia);
            Commit();

            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    public bool chk3IN1_TXN(CFB2IA1100DAO wfb2ia)
    {
        bool b = false;
        try
        {
            BeginTransaction();
            //畫面.作業別="加保" or 畫面.作業別="身份轉換"
            if (wfb2ia.OPERATION_KIND == "I" || wfb2ia.OPERATION_KIND == "U" )
            {
                #region 加保或身份轉換
                //團保處理
                if (wfb2ia.OPERATION_KIND == "I" && wfb2ia.isGINS_IS_YN)
                {
                    wfb2ia.INS_TYPE = "A";
                    DataTable tmp = wfb2ia.getGROUP_TXNData(wfb2ia.INS_TYPE);
                    if (tmp.Rows.Count > 0)
                    {
                        //紀錄團保處理異常
                        wfb2ia.updateCHG_TXN("團保已加保");
                        b = true;
                    }
                }
                //健保處理
                if (wfb2ia.isHEALTH_IS_YN)
                {
                    wfb2ia.INS_TYPE = "B";
                    DataTable tmp = wfb2ia.get3IN1_TXNData(wfb2ia.INS_TYPE, wfb2ia.HEALTH_CHG_DT, wfb2ia.OPERATION_KIND);
                    if (tmp.Rows.Count > 0)
                    {
                        //紀錄勞保、健保、勞退處理異常
                        wfb2ia.updateCHG_TXN("健保已加保");
                        b = true;
                    }
                }
                //勞退處理
                if (wfb2ia.isPENSION_IS_YN) 
                {
                    wfb2ia.INS_TYPE = "C";
                    DataTable tmp = wfb2ia.get3IN1_TXNData(wfb2ia.INS_TYPE, wfb2ia.PENSION_CHG_DT, wfb2ia.OPERATION_KIND);
                    if (tmp.Rows.Count > 0)
                    {
                        //紀錄勞保、健保、勞退處理異常
                        wfb2ia.updateCHG_TXN("勞退已加保");
                        b = true;
                    }
                }
                 //勞保處理
                if (wfb2ia.is_LABOR_IS_YN && wfb2ia.HR_CHG_CD != "B14") 
                {
                    wfb2ia.INS_TYPE = "A";
                    DataTable tmp = wfb2ia.get3IN1_TXNData(wfb2ia.INS_TYPE, wfb2ia.LABOR_CHG_DT, wfb2ia.OPERATION_KIND);
                    if (tmp.Rows.Count > 0)
                    {
                        //紀錄勞保、健保、勞退處理異常
                        wfb2ia.updateCHG_TXN("勞保已加保");
                        b = true;
                    }
                }
                #endregion
            }
            else if (wfb2ia.OPERATION_KIND == "O")
            {
                #region 退保處理
                //健保處理
                if (wfb2ia.isHEALTH_IS_YN)
                {
                    wfb2ia.INS_TYPE = "B";
                    DataTable tmp = wfb2ia.get3IN1_TXNData(wfb2ia.INS_TYPE);
                    if (tmp.Rows.Count == 0)
                    {
                        //紀錄勞保、健保、勞退處理異常
                        wfb2ia.updateCHG_TXN("健保已退保");
                        b = true;
                    }
                }
                //勞退處理
                if (wfb2ia.isPENSION_IS_YN)
                {
                    wfb2ia.INS_TYPE = "C";
                    DataTable tmp = wfb2ia.get3IN1_TXNData(wfb2ia.INS_TYPE);
                    if (tmp.Rows.Count == 0)
                    {
                        //紀錄勞保、健保、勞退處理異常
                        wfb2ia.updateCHG_TXN("勞退已退保");
                        b = true;
                    }
                }
                //勞保處理
                if (wfb2ia.is_LABOR_IS_YN && wfb2ia.HR_CHG_CD != "B14") 
                {
                    wfb2ia.INS_TYPE = "A";
                    DataTable tmp = wfb2ia.get3IN1_TXNData(wfb2ia.INS_TYPE);
                    if (tmp.Rows.Count == 0)
                    {
                        //紀錄勞保、健保、勞退處理異常
                        wfb2ia.updateCHG_TXN("勞保已退保");
                        b = true;
                    }
                }
                //團保處理
                if (wfb2ia.isGINS_IS_YN) 
                {
                    wfb2ia.INS_TYPE = "A";
                    DataTable tmp = wfb2ia.getGROUP_TXNData2(wfb2ia.INS_TYPE);
                    if (tmp.Rows.Count == 0)
                    {
                        //紀錄團保處理異常
                        wfb2ia.updateCHG_TXN("團保已退保");
                        b = true;
                    }
                }

                #endregion
            }
            Commit();
            return b;
        }
        catch (Exception)
        {
            RollBack();
            throw;
        }
    }

    /*加保處理*/
    public string exec_IKind(CFB2IA1100DAO wfb2ia)
    {
        try
        {
            BeginTransaction();
            //勞保處理
            if (wfb2ia.is_LABOR_IS_YN)
            {
                wfb2ia.INS_TYPE = "A";
                wfb2ia.insert3IN1_TXN(wfb2ia);                
            }

            //健保處理
            if (wfb2ia.isHEALTH_IS_YN)
            {
                wfb2ia.INS_TYPE = "B";               
                wfb2ia.insert3IN1_TXN(wfb2ia);//新公司加保                            
            }

            //勞退處理
            if (wfb2ia.isPENSION_IS_YN)
            {
                wfb2ia.INS_TYPE = "C";
                wfb2ia.insert3IN1_TXN(wfb2ia);
               
                //勞退自提率
                if (Convert.ToDouble(wfb2ia.isPENSION_SELF_RATIO) > 0)
                {                    
                    wfb2ia.insertRETIRE_SELFRATE(wfb2ia);
                }
            }

             //團保處理
            if (wfb2ia.isGINS_IS_YN)
            {
                wfb2ia.insertGROUP_TXN(wfb2ia);
            }

            if (wfb2ia.is_LABOR_IS_YN || wfb2ia.isHEALTH_IS_YN || wfb2ia.isPENSION_IS_YN)
            {
                wfb2ia.insert3IN1_REPORTDATA(wfb2ia);
            }

            if (!wfb2ia.isPERSONDATA(wfb2ia))
            {
                //1.新增[TB_I_M_PERSONDATA 保險資料主檔]
                wfb2ia.insertPERSONDATA(wfb2ia);
                //2.新增[TB_I_R_DATAUPDAE_HIS 保險資料更新歷史檔]
                wfb2ia.insertDATAUPDAE_HIS(wfb2ia);
            }
            //最後upadte到TB_I_M_CHG_TXN 保險一括異動記錄檔
            wfb2ia.updateCHG_TXN();

            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();            
            return ex.Message;
        }
    }

    /*退保處理*/
    public string exec_OKind(CFB2IA1100DAO wfb2ia)
    {
        try
        {
            BeginTransaction();
            //勞保處理
            if (wfb2ia.is_LABOR_IS_YN)
            {
                wfb2ia.INS_TYPE = "A";
                wfb2ia.update3IN1_TXN_A(wfb2ia);
            }

            //健保處理
            if (wfb2ia.isHEALTH_IS_YN)
            {
                wfb2ia.INS_TYPE = "B";
                wfb2ia.update3IN1_TXN_B(wfb2ia);
            }

            //if (wfb2ia.EMP_ID == "10453")
            //{
            //    string tt = "123";
            //    tt = tt.Substring(0, 100);
            //}
            //勞退處理
            if (wfb2ia.isPENSION_IS_YN)
            {
                wfb2ia.INS_TYPE = "C";
                wfb2ia.update3IN1_TXN_C(wfb2ia);
                 //勞退自提率(退保時,介面不會顯示勞退自提率,須從資料庫抓取)
                double tmp3;
                string self_ratio = getPENSION_SELF_RATIO(wfb2ia.EMP_ID);

                if (double.TryParse(self_ratio, out tmp3) &&  Convert.ToDouble(self_ratio) > 0)
                {
                    wfb2ia.PENSION_SELF_RATIO = self_ratio;
                    wfb2ia.updateRETIRE_SELFRATE(wfb2ia);
                }
            }

            //團保處理(有眷屬加保,須一起退保)
            if (wfb2ia.isGINS_IS_YN)
            {
                wfb2ia.updateGROUP_TXN(wfb2ia);
            }

            if (wfb2ia.is_LABOR_IS_YN || wfb2ia.isHEALTH_IS_YN || wfb2ia.isPENSION_IS_YN)
            {
                wfb2ia.insert3IN1_REPORTDATA2(wfb2ia);
            }
            //最後upadte到TB_I_M_CHG_TXN 保險一括異動記錄檔
            wfb2ia.updateCHG_TXN();

            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();            
            return ex.Message;
        }
    }

    /*身份轉換*/
    public string exec_UKind(CFB2IA1100DAO wfb2ia, List<string> license_id_list)
    {
        try
        {
            BeginTransaction();
            //勞保處理
            if (wfb2ia.is_LABOR_IS_YN)
            {
                wfb2ia.INS_TYPE = "A";
                wfb2ia.update3IN1_TXN_A2(wfb2ia);
                wfb2ia.insert3IN1_TXN(wfb2ia);
            }

            //健保處理
            if (wfb2ia.isHEALTH_IS_YN)
            {
                wfb2ia.INS_TYPE = "B";
                wfb2ia.update3IN1_TXN_B2(wfb2ia);//原公司退保
                wfb2ia.insert3IN1_TXN(wfb2ia);//新公司加保
                //找尋有無眷屬資料,若有須一併加保至新公司別
                //本人身份轉換時,眷屬仍在保的找出	
                for (int i = 0; i < license_id_list.Count; i++)
                {                    
                    wfb2ia.insert3IN1_TXN_B(license_id_list[i]);                   
                }
            }

            //勞退處理
            if (wfb2ia.isPENSION_IS_YN)
            {
                wfb2ia.INS_TYPE = "C";
                wfb2ia.update3IN1_TXN_C2(wfb2ia);
                wfb2ia.updateRETIRE_SELFRATE2(wfb2ia);
                wfb2ia.insert3IN1_TXN(wfb2ia);
                //勞退自提率
                if (Convert.ToDouble(wfb2ia.isPENSION_SELF_RATIO) > 0) 
                {
                    wfb2ia.insertRETIRE_SELFRATE(wfb2ia);
                }
            }

            if (wfb2ia.is_LABOR_IS_YN || wfb2ia.isHEALTH_IS_YN || wfb2ia.isPENSION_IS_YN)
            {
                wfb2ia.insert3IN1_REPORTDATA5(wfb2ia);//新增[TB_I_R_3IN1_REPORTDATA保險三合申報檔資料](身分轉換_退保)
                wfb2ia.insert3IN1_REPORTDATA3(wfb2ia);//新增[TB_I_R_3IN1_REPORTDATA保險三合申報檔資料]	
                //本人身份轉換時,眷屬仍在保的找出	
                for (int i = 0; i < license_id_list.Count; i++)
                {
                    //取得眷屬姓名、眷屬出生日期和稱謂
                    string arr_FAMILY = wfb2ia.getFAMILY(license_id_list[i]);
                    wfb2ia.insert3IN1_REPORTDATA4(license_id_list[i], arr_FAMILY);                   
                }

            }

            if (!wfb2ia.isPERSONDATA(wfb2ia))
            {
                //1.新增[TB_I_M_PERSONDATA 保險資料主檔]
                wfb2ia.insertPERSONDATA(wfb2ia);
                //2.新增[TB_I_R_DATAUPDAE_HIS 保險資料更新歷史檔]
                wfb2ia.insertDATAUPDAE_HIS(wfb2ia);
            }

            //最後upadte到TB_I_M_CHG_TXN 保險一括異動記錄檔
            wfb2ia.updateCHG_TXN();

            #region 身分轉換+薪調
            //最後刪除保險薪調記錄檔
            if (wfb2ia.REMARK == "身份轉換+薪調")
            {
                wfb2ia.delLEVEL_CHG(wfb2ia.EMP_ID);
            }
            #endregion

            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();             
            return ex.Message;
        }
    }       

    //異動[TB_I_M_3IN1_TXN 勞保健保勞退履歷主檔 ] //勞保身分轉換
    public string update3IN1_TXN_A2(CFB2IA1100DAO wfb2ia)
    {
        try
        {
            BeginTransaction();

            wfb2ia.update3IN1_TXN_A2(wfb2ia);

            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    //異動[TB_I_M_3IN1_TXN 勞保健保勞退履歷主檔 ] //健保身分轉換
    public string update3IN1_TXN_B2(CFB2IA1100DAO wfb2ia)
    {
        try
        {
            BeginTransaction();

            wfb2ia.update3IN1_TXN_B2(wfb2ia);

            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    //加保 //找尋有無眷屬資料,若有須一併加保至新公司別 //健保處理
    public string insert3IN1_TXN_B(CFB2IA1100DAO wfb2ia, List<string> license_id_list)
    {
        try
        {
            //本人身份轉換時,眷屬仍在保的找出	
            for (int i = 0; i < license_id_list.Count; i++)
            {
                BeginTransaction();
                wfb2ia.insert3IN1_TXN_B(license_id_list[i]);
                Commit();
            }
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    //異動[TB_I_M_3IN1_TXN 勞保健保勞退履歷主檔 ] //勞退身分轉換
    public string update3IN1_TXN_C2(CFB2IA1100DAO wfb2ia)
    {
        try
        {
            BeginTransaction();

            wfb2ia.update3IN1_TXN_C2(wfb2ia);

            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    //異動[TB_I_M_RETIRE_SELFRATE 勞退自提履歷檔 ] //勞退身分轉換
    public string updateRETIRE_SELFRATE2(CFB2IA1100DAO wfb2ia)
    {
        try
        {
            BeginTransaction();

            wfb2ia.updateRETIRE_SELFRATE2(wfb2ia);

            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    //新增[TB_I_R_3IN1_REPORTDATA保險三合申報檔資料](退保)
    public string insert3IN1_REPORTDATA5(CFB2IA1100DAO wfb2ia)
    {
        try
        {
            BeginTransaction();

            wfb2ia.insert3IN1_REPORTDATA5(wfb2ia);

            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    //新增[TB_I_R_3IN1_REPORTDATA保險三合申報檔資料]	
    public string insert3IN1_REPORTDATA3(CFB2IA1100DAO wfb2ia)
    {
        try
        {
            BeginTransaction();

            wfb2ia.insert3IN1_REPORTDATA3(wfb2ia);

            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    //新增[TB_I_R_3IN1_REPORTDATA保險三合申報檔資料] 
    public string insert3IN1_REPORTDATA4(CFB2IA1100DAO wfb2ia, List<string> license_id_list)
    {
        try
        {
            //本人身份轉換時,眷屬仍在保的找出	
            for (int i = 0; i < license_id_list.Count; i++)
            {
                BeginTransaction();
                //取得眷屬姓名、眷屬出生日期和稱謂
                string arr_FAMILY = wfb2ia.getFAMILY(license_id_list[i]);

                

                wfb2ia.insert3IN1_REPORTDATA4(license_id_list[i], arr_FAMILY);
                Commit();
            }

            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    //1.新增[TB_I_M_PERSONDATA 保險資料主檔]   
    public string insertPERSONDATA(CFB2IA1100DAO wfb2ia)
    {
        try
        {
            BeginTransaction();

            wfb2ia.insertPERSONDATA(wfb2ia);

            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    //2.新增[TB_I_R_DATAUPDAE_HIS 保險資料更新歷史檔]
    public string insertDATAUPDAE_HIS(CFB2IA1100DAO wfb2ia)
    {
        try
        {
            BeginTransaction();

            wfb2ia.insertDATAUPDAE_HIS(wfb2ia);

            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    //是否找到[TB_I_M_PERSONDATA 保險資料主檔]的資料
    public bool isPERSONDATA(CFB2IA1100DAO wfb2ia)
    {
        try
        {            
            return wfb2ia.isPERSONDATA(wfb2ia);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //刪除[TB_I_M_CHG_TXN 保險一括加退保檔]
    public string deleteCHG_TXN(List<Tuple<string, string, string, string>> emp_id)
    {
        try
        {
            CFB2IA1100DAO wfb2ia = new CFB2IA1100DAO();
            BeginTransaction();
            foreach (var item in emp_id)
            {
                wfb2ia.deleteCHG_TXN(item.Item1, item.Item2, item.Item3, item.Item4);
            }
            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    //異動別
    public DataTable getHR_CHG_DESC(string hr_chg_cd)
    {
        try
        {
            CFB2IA1100DAO wfb2ia = new CFB2IA1100DAO();
            return wfb2ia.getHR_CHG_DESC(hr_chg_cd);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //公司別
    public DataTable getCOMPANY_SNAME(string company_cd_old)
    {
        try
        {
            CFB2IA1100DAO wfb2ia = new CFB2IA1100DAO();
            return wfb2ia.getCOMPANY_SNAME(company_cd_old);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //工號
    public DataTable getEmpName(string emp_id)
    {
        try
        {
            CFB2IA1100DAO wfb2ia = new CFB2IA1100DAO();
            return wfb2ia.getEmpName(emp_id);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //國籍代號
    public DataTable getNATION_Name(string nation_cd)
    {
        try
        {
            CFB2IA1100DAO wfb2ia = new CFB2IA1100DAO();
            return wfb2ia.getNATION_Name(nation_cd);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //取得勞退自提率
    public string getPENSION_SELF_RATIO(string emp_id)
    {
        try
        {
            string result = "";
            CFB2IA1100DAO wfb2ia = new CFB2IA1100DAO();
            DataTable tmp = wfb2ia.getPENSION_SELF_RATIO(emp_id);
            if (tmp.Rows.Count > 0)
            {
                result = tmp.Rows[0]["SLEF_RATE"].ToString();
            }
            return result;
        }
        catch (Exception)
        {

            throw;
        }
    }

    //找尋眷屬資料
    public DataTable getlicense_id(CFB2IA1100DAO wfb2ia)
    {
        try
        {
            return wfb2ia.getLICENSE_ID();
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getLEVEL_CHG_Count(string EMP_ID)
    {
        try
        {
            CFB2IA1100DAO wfb2ia = new CFB2IA1100DAO();
            return wfb2ia.getLEVEL_CHG_Count(EMP_ID);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getLEVEL_CHG(string EMP_ID)
    {
        try
        {
            CFB2IA1100DAO wfb2ia = new CFB2IA1100DAO();
            return wfb2ia.getLEVEL_CHG(EMP_ID);
        }
        catch (Exception)
        {

            throw;
        }
    }

}