using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;
using NPOI.SS.UserModel;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.Util;

/// <summary>
/// CFB2SS050BO 的摘要描述
/// </summary>
public class CFB2SS0500BO : BaseService
{
    public CFB2SS0500BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    //是否已轉薪資-轉薪資判斷用
    public string chkIS_SEND(CFB2SS0500DAO dao)
    {
        try
        {
            string msg = "0";
            int cnt = dao.chkIS_SEND();
            if (cnt > 0)
            {
                msg = "此發薪日期+獎金類型已轉薪資！";
                return msg;
            }

            //節金檔是否有相同節金類型及發放日期
            string rtnMsg = dao.checkFN_SS_CHK_FESTIVAL("A"); //A-節金是否已存在
            if (rtnMsg != "") {
                msg = rtnMsg;
            }

            return msg;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //是否已轉傳薪資-取消轉薪資判斷用
    public string chkIS_CANCEL_SEND(CFB2SS0500DAO dao)
    {
        try
        {
            string msg = "0";
            int cnt = dao.chkIS_SEND();
            if (cnt == 0)
            {
                msg = "此發薪日期+獎金類型未轉薪資！";
                return msg;
            }
            //節金是否已發薪
            string rtnMsg = dao.checkFN_SS_CHK_FESTIVAL("P"); //P-節金是否已發薪
            if (rtnMsg != "")
            {
                msg = rtnMsg;
            }

            return msg;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //刪除
    public string delSave(List<Tuple<string, string, string>> keysList)
    {
        CFB2SS0500DAO ss050DAO = new CFB2SS0500DAO();
        string rtnmessage = "";//存在檢查後的訊息
        try
        {
            //檢查完成後，逐筆進行刪除
            if (rtnmessage == "")
            {
                BeginTransaction();
                foreach (var item in keysList)
                {
                    ss050DAO.SALARY_DT = item.Item1;
                    ss050DAO.INCENTIVE_TYPE = item.Item2;
                    ss050DAO.delTableSave("TB_S_M_INCENTIVE_PAY_H");
                    ss050DAO.delTableSave("TB_S_M_INCENTIVE_PAY_D");
                }
                Commit();
                return "0";
            }
            else
            {
                return rtnmessage;
            }
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
 

    //EXCEL匯出
    public IWorkbook excelDownload(string excelPath, CFB2SS0500DAO ss050DAO)
    {
        FileStream fs = null;
        IWorkbook workbook = null;

        //取得範本sheet
        ISheet sheet = null;
        try
        {
            //取得下載資料
            DataTable dt = ss050DAO.getExcelData();

            fs = new FileStream(excelPath, FileMode.Open, FileAccess.ReadWrite);
            workbook = new XSSFWorkbook(fs); //xlsx的方法

            //取得範本sheet
            sheet = workbook.GetSheetAt(0);

            if (sheet != null)
            {

                IRow row;
                IRow row_title;
                ICell cell;
                ICellStyle stringLeft = this.setCellStyle(workbook, "left", false, 12);
                ICellStyle stringLeftStyle = this.setCellStyle(workbook, "left", true, 12);
                ICellStyle stringRightStyle = this.setCellStyle(workbook, "right", true, 12);
                ICellStyle stringCenterStyle = this.setCellStyle(workbook, "center", true, 12);

                ///cell.SetCellValue((Convert.ToDouble(dt.Rows[i][tableCD + "LEVEL_PAY"].ToString())).ToString("N0"));

                int x = 0;
                if (dt.Rows.Count > 0)
                {
                    row_title = sheet.GetRow(1);
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        x = i + 1;//從第1列開始insert 資料
                        row = sheet.CreateRow(x);

                        //將基本資料寫入範本
                        //序號

                        //發薪日期 	
                        cell = row.CreateCell(0);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["SALARY_DT"].ToString());
                        //獎金類型    	   						
                        cell = row.CreateCell(1);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["INCENTIVE_TYPE"].ToString());
                        //工號     	   						
                        cell = row.CreateCell(2);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["EMP_ID"].ToString());
                        //姓名    	   						
                        cell = row.CreateCell(3);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["EMP_NAME"].ToString());
                        //計算起日    	   						
                        cell = row.CreateCell(4);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["START_DT"].ToString());

                        //計算迄日    	   						
                        cell = row.CreateCell(5);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["END_DT"].ToString());
                        //在職天數    	   						
                        cell = row.CreateCell(6);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["WORK_DAYS"].ToString());
                        //基本月薪    	   						
                        cell = row.CreateCell(7);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue((Convert.ToDouble(dt.Rows[i]["BASE_MONTH"].ToString())).ToString("N0"));
                        //發放月數    	   						
                        cell = row.CreateCell(8);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["INCENTIVE_MONTH"].ToString());
                        //勤怠金額    	   						
                        cell = row.CreateCell(9);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue((Convert.ToDouble(dt.Rows[i]["ATTENDANCE_AMT"].ToString())).ToString("N0"));

                        				

                        //獎懲金額    	   						
                        cell = row.CreateCell(10);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue((Convert.ToDouble(dt.Rows[i]["REWARD_AMT"].ToString())).ToString("N0"));
                        //紀律金額    	   						
                        cell = row.CreateCell(11);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue((Convert.ToDouble(dt.Rows[i]["DISCIPLINE_AMT"].ToString())).ToString("N0"));
                        //實發金額    	   						
                        cell = row.CreateCell(12);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue((Convert.ToDouble(dt.Rows[i]["INCENTIVE_AMT"].ToString())).ToString("N0"));
                        //基本日薪    	   						
                        cell = row.CreateCell(13);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue((Convert.ToDouble(dt.Rows[i]["BASE_DAY"].ToString())).ToString("N0"));
                        //事假日數    	   						
                        cell = row.CreateCell(14);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["LEAVE_A_DAY"].ToString());

                        //病假日數    	   						
                        cell = row.CreateCell(15);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["LEAVE_B_DAY"].ToString());
                        //事病假扣除日數    	   						
                        cell = row.CreateCell(16);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["LEAVE_AB_DAYS"].ToString());
                        //曠工日數    	   						
                        cell = row.CreateCell(17);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["LEAVE_Q_DAY"].ToString());
                        //曠工扣除日數    	   						
                        cell = row.CreateCell(18);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["LEAVE_Q_DAYS"].ToString());
                        //嘉獎次數    	   						
                        cell = row.CreateCell(19);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["THIRD_CNT_REWARD"].ToString());

                        //小功次數    	   						
                        cell = row.CreateCell(20);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["SECOND_CNT_REWARD"].ToString());
                        //大功次數    	   						
                        cell = row.CreateCell(21);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["FIRST_CNT_REWARD"].ToString());
                        //申誡次數    	   						
                        cell = row.CreateCell(22);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["THIRD_CNT_PUNISH"].ToString());
                        //小過次數    	   						
                        cell = row.CreateCell(23);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["SECOND_CNT_PUNISH"].ToString());
                        //大過次數    	   						
                        cell = row.CreateCell(24);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["FIRST_CNT_PUNISH"].ToString());
                        //獎懲日數    	   						
                        cell = row.CreateCell(25);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["JUDGEMENT_DAYS"].ToString());
                    }
                   for (int i = 0; i <= 25; i++)
                   {
                       sheet.AutoSizeColumn(i);
                   }

                    row = sheet.GetRow(0);
                    cell = row.CreateCell(26);
                    cell.CellStyle = stringLeft;
                    cell.SetCellValue("製表日期:" + DateTime.Now.ToString("yyyy/MM/dd"));

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

    //獎金轉至薪資(節金檔)
    public string exec_SP_SS_SEND_FESTIVAL(CFB2SS0500DAO dao)
    {

        string rtnmessage = "";//檢查後的訊息
        try
        {
            dao.exec_SP_SS_SEND_FESTIVAL();
            rtnmessage += utilities.getSPLOG("exec_SP_SS_SEND_FESTIVAL");
            if (rtnmessage != "")
            {
                return rtnmessage;
            }
            return "0";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    //獎金取消轉至薪資(節金檔)
    public string exec_SP_SS_CANCEL_FESTIVAL(CFB2SS0500DAO dao)
    {

        string rtnmessage = "";//檢查後的訊息
        try
        {
            dao.exec_SP_SS_CANCEL_FESTIVAL();
            rtnmessage += utilities.getSPLOG("SP_SS_CANCEL_FESTIVAL");
            if (rtnmessage != "")
            {
                return rtnmessage;
            }
            return "0";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    #region EXCEL 樣示

    //無底色的基本款+字型大小
    private ICellStyle setCellStyle(IWorkbook workbook, string align, bool isBorder, short fontSize)
    {
        return setCellStyle(workbook, align, isBorder, fontSize, 0, false, false);
    }
    //無底色的基本款+ 是否換行
    private ICellStyle setCellStyle(IWorkbook workbook, string align, bool isBorder, short fontSize, bool isWrap)
    {
        return setCellStyle(workbook, align, isBorder, fontSize, 0, false, isWrap);
    }

    /// <summary>
    /// 設定資料的格式
    /// </summary>
    /// <param name="workbook"></param>
    /// <param name="align">文字位置</param>
    /// <param name="isBorder">是否要有邊框</param>
    /// <param name="color">背景顏色設定(10:紅,13:黃,14:pink.... )</param>
    /// <param name="color">背景顏色設定(10:紅,13:黃,14:pink.... )</param>
    /// <returns></returns>
    private ICellStyle setCellStyle(IWorkbook workbook, string align, bool isBorder, short fontSize, int colorCD, bool isBold, bool isWrap)
    {
        ICellStyle style = workbook.CreateCellStyle();

        //自動換列
        if (isWrap)
        {
            style.WrapText = isWrap;
        }
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


    #endregion
}