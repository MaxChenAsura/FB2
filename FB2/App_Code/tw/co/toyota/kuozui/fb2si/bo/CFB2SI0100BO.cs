using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;

using System.Text;
using NPOI.HSSF.Util;
using NPOI.HPSF;
using NPOI.POIFS.FileSystem;
using System.Drawing;

/// <summary>
/// CFB2SI0100BO 的摘要描述
/// </summary>
public class CFB2SI0100BO : BaseService
{
    public CFB2SI0100BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    //刪除
    public string Delete_S_M_BONUS_H(List<string> BONUS_YEARs)
    {

        CFB2SI0100DAO fb2si = new CFB2SI0100DAO();
        try
        {
            BeginTransaction();
            foreach (string BONUS_YEAR in BONUS_YEARs)
            {
                fb2si.Delete_S_M_BONUS_H(BONUS_YEAR);
                fb2si.Delete_TB_S_R_BONUS_D(BONUS_YEAR);
                fb2si.Delete_TB_S_M_BONUS_D(BONUS_YEAR);
                fb2si.Delete_TB_S_S_BONUS_D(BONUS_YEAR);
            }
            Commit();

            return "0";
        }
        catch (Exception)
        {
            throw;
        }
    }
    //新增
    public string Add_S_M_BONUS_H(CFB2SI0100DAO fb2si)
    {
        try
        {
            //取得現有資料(檢查重複)
            DataTable tmp = fb2si.getExistData();
            BeginTransaction();
            if (tmp.Rows.Count > 0)
            {
                return "紅利年度重複!";
            }
            else
            {
                fb2si.Add_S_M_BONUS_H();
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
    //修改
    public string Update_S_M_BONUS_H(CFB2SI0100DAO fb2si)
    {
        try
        {
            BeginTransaction();
            fb2si.Update_S_M_BONUS_H();
            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    //紅利對象生成
    public string execSP_S_BONUS_DATA(CFB2SI0100DAO fb2si)
    {
        string rtnmessage = "";//存在檢查後的訊息

        try
        {

            //檢查完成後，逐筆進行刪除
            if (rtnmessage == "")
            {
                fb2si.execSP_S_BONUS_DATA();

                rtnmessage += utilities.getSPLOG("SP_S_BONUS_DATA");
                if (rtnmessage != "")
                {
                    return rtnmessage;
                }
                
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
    //薪資轉出
    public string Announce_S_M_BONUS_H(CFB2SI0100DAO fb2si)
    {
        try
        {
            BeginTransaction();
            fb2si.Announce_S_M_BONUS_H();
            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    //提出核可
    public string Release_S_M_BONUS_H(CFB2SI0100DAO fb2si)
    {
        try
        {
            BeginTransaction();
            fb2si.Release_S_M_BONUS_H();
            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    public string Delete_S_M_BONUS_D(CFB2SI0100DAO fb2si)
    {
        try
        {
            //DateTime now = DateTime.Now;
            BeginTransaction();
            fb2si.Delete_S_M_BONUS_D();
            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }

    }
    public string Status_S_M_BONUS_D(CFB2SI0100DAO fb2si)
    {
        try
        {

            BeginTransaction();
            fb2si.Status_S_M_BONUS_D();
            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    //下載Excel資料
    public IWorkbook createExcelFromTemplate(string type, string excelPath, string data, string BONUS_YEAR)
    {
        CFB2SI0100DAO fb2si = new CFB2SI0100DAO();
        FileStream fs = null;
        IWorkbook workbook = null;

        //取得範本sheet
        ISheet sheet = null;
        try
        {
            fs = new FileStream(excelPath, FileMode.Open, FileAccess.Read);
            //依type判斷要用哪種方式產生
            if (type == "xls")
                workbook = new HSSFWorkbook(fs);
            else
                workbook = new XSSFWorkbook(fs);

            //取得範本sheet
            if (data == "mantain" || data == "original")
            {
                sheet = workbook.GetSheetAt(0);
                int x = 0;
                if (sheet != null)
                {
                    IRow row;
                    ICellStyle stringRightStyle = this.setCellStyle(workbook, "right", false);
                    DataTable dt = fb2si.getExcelData(data, BONUS_YEAR);
                    if (dt.Rows.Count > 0)
                    {
                        //取得紅利發放天數
                        string bonus_days = "0";
                        DataTable dt_TB_S_M_BONUS_H = fb2si.Get_TB_S_M_BONUS_H(BONUS_YEAR);
                        foreach (DataRow dr in dt_TB_S_M_BONUS_H.Rows)
                        {
                            bonus_days = Convert.ToString(dr["BONUS_DAYS"]);
                        }

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            x = i + 3;
                            row = sheet.CreateRow(x);
                            //將資料寫入範本
                            row.CreateCell(1).SetCellValue(dt.Rows[i]["EMP_ID"].ToString());
                            row.CreateCell(2).SetCellValue(dt.Rows[i]["EMP_NAME"].ToString());
                            row.CreateCell(3).SetCellValue(dt.Rows[i]["EMP_CHG_CD_desc"].ToString());
                            row.CreateCell(4).SetCellValue(dt.Rows[i]["WS_CD"].ToString());
                            row.CreateCell(5).SetCellValue(dt.Rows[i]["JPN_CD"].ToString());
                            row.CreateCell(6).SetCellValue(dt.Rows[i]["DEPT_NO"].ToString());
                            row.CreateCell(7).SetCellValue(dt.Rows[i]["LEVEL_CD"].ToString());
                            row.CreateCell(8).SetCellValue(dt.Rows[i]["PJOB_CD"].ToString());
                            if (dt.Rows[i]["JOIN_DT"].ToString() == "")
                                row.CreateCell(9).SetCellValue("");
                            else
                                row.CreateCell(9).SetCellValue(Convert.ToDateTime(dt.Rows[i]["JOIN_DT"]).ToString("yyyy/MM/dd"));
                            if (dt.Rows[i]["LEAVE_DT"].ToString() == "")
                                row.CreateCell(10).SetCellValue("");
                            else
                                row.CreateCell(10).SetCellValue(Convert.ToDateTime(dt.Rows[i]["LEAVE_DT"]).ToString("yyyy/MM/dd"));
                            if (dt.Rows[i]["STAY_DT"].ToString() == "")
                                row.CreateCell(11).SetCellValue("");
                            else
                                row.CreateCell(11).SetCellValue(Convert.ToDateTime(dt.Rows[i]["STAY_DT"]).ToString("yyyy/MM/dd"));
                            if (dt.Rows[i]["BE_CONTRACT_DT"].ToString() == "")
                                row.CreateCell(12).SetCellValue("");
                            else
                                row.CreateCell(12).SetCellValue(Convert.ToDateTime(dt.Rows[i]["BE_CONTRACT_DT"]).ToString("yyyy/MM/dd"));
                            if (dt.Rows[i]["BE_EMP_DT"].ToString() == "")
                                row.CreateCell(13).SetCellValue("");
                            else
                                row.CreateCell(13).SetCellValue(Convert.ToDateTime(dt.Rows[i]["BE_EMP_DT"]).ToString("yyyy/MM/dd"));
                            row.CreateCell(14).SetCellValue(dt.Rows[i]["WORK_DAYS"].ToString());
                            row.CreateCell(15).SetCellValue(dt.Rows[i]["EMP_CD_desc"].ToString());
                            row.CreateCell(16).SetCellValue(dt.Rows[i]["ID_DESC"].ToString());
                            if (dt.Rows[i]["ABILITY_PAY"].ToString() == "")
                                row.CreateCell(17).SetCellValue(String.Format("{0:N0}", int.Parse("0")));
                            else
                                row.CreateCell(17).SetCellValue(String.Format("{0:N0}", int.Parse(dt.Rows[i]["ABILITY_PAY"].ToString())));
                            if (dt.Rows[i]["LEVEL_PAY"].ToString() == "")
                                row.CreateCell(18).SetCellValue(String.Format("{0:N0}", int.Parse("0")));
                            else
                                row.CreateCell(18).SetCellValue(String.Format("{0:N0}", int.Parse(dt.Rows[i]["LEVEL_PAY"].ToString())));
                            if (dt.Rows[i]["PJOB_PAY"].ToString() == "")
                                row.CreateCell(19).SetCellValue(String.Format("{0:N0}", int.Parse("0")));
                            else
                                row.CreateCell(19).SetCellValue(String.Format("{0:N0}", int.Parse(dt.Rows[i]["PJOB_PAY"].ToString())));
                            if (dt.Rows[i]["PROFESSION_PAY"].ToString() == "")
                                row.CreateCell(20).SetCellValue(String.Format("{0:N0}", int.Parse("0")));
                            else
                                row.CreateCell(20).SetCellValue(String.Format("{0:N0}", int.Parse(dt.Rows[i]["PROFESSION_PAY"].ToString())));
                            if (dt.Rows[i]["FOOD_SUBSIDY"].ToString() == "")
                                row.CreateCell(21).SetCellValue(String.Format("{0:N0}", int.Parse("0")));
                            else
                                row.CreateCell(21).SetCellValue(String.Format("{0:N0}", int.Parse(dt.Rows[i]["FOOD_SUBSIDY"].ToString())));
                            row.CreateCell(22).SetCellValue(dt.Rows[i]["LEAVE_A_HOUR"].ToString());
                            row.CreateCell(23).SetCellValue(dt.Rows[i]["LEAVE_B_HOUR"].ToString());
                            row.CreateCell(24).SetCellValue(dt.Rows[i]["LEAVE_C_HOUR"].ToString());
                            row.CreateCell(25).SetCellValue(dt.Rows[i]["LEAVE_Q_HOUR"].ToString());
                            row.CreateCell(26).SetCellValue(dt.Rows[i]["LEAVE_OP_HOUR"].ToString());
                            row.CreateCell(27).SetCellValue(dt.Rows[i]["THIRD_CNT_P"].ToString());
                            row.CreateCell(28).SetCellValue(dt.Rows[i]["SECOND_CNT_P"].ToString());
                            row.CreateCell(29).SetCellValue(dt.Rows[i]["FIRST_CNT_P"].ToString());
                            row.CreateCell(30).SetCellValue(dt.Rows[i]["THIRD_CNT_M"].ToString());
                            row.CreateCell(31).SetCellValue(dt.Rows[i]["SECOND_CNT_M"].ToString());
                            row.CreateCell(32).SetCellValue(dt.Rows[i]["FIRST_CNT_M"].ToString());
                            row.CreateCell(33).SetCellValue(dt.Rows[i]["ATTEND_DAYS"].ToString());
                            row.CreateCell(34).SetCellValue(dt.Rows[i]["REWARD_DAYS"].ToString());
                            row.CreateCell(35).SetCellValue(dt.Rows[i]["DISCIPLINE_DAYS"].ToString());
                            row.CreateCell(36).SetCellValue(dt.Rows[i]["BONUS_WORK_DAYS"].ToString());
                            row.CreateCell(37).SetCellValue(bonus_days);
                            if (dt.Rows[i]["BONUS_AMT"].ToString() == "")
                                row.CreateCell(38).SetCellValue(String.Format("{0:N0}", int.Parse("0")));
                            else
                                row.CreateCell(38).SetCellValue(String.Format("{0:N0}", int.Parse(dt.Rows[i]["BONUS_AMT"].ToString())));
                            if (dt.Rows[i]["BONUS_TAX"].ToString() == "")
                                row.CreateCell(39).SetCellValue(String.Format("{0:N0}", int.Parse("0")));
                            else
                                row.CreateCell(39).SetCellValue(String.Format("{0:N0}", int.Parse(dt.Rows[i]["BONUS_TAX"].ToString())));
                            if (dt.Rows[i]["BONUS_AMT_R"].ToString() == "")
                                row.CreateCell(40).SetCellValue(String.Format("{0:N0}", int.Parse("0")));
                            else
                                row.CreateCell(40).SetCellValue(String.Format("{0:N0}", int.Parse(dt.Rows[i]["BONUS_AMT_R"].ToString())));
                            row.CreateCell(41).SetCellValue(dt.Rows[i]["PAY_TYPE"].ToString());
                            row.CreateCell(42).SetCellValue(dt.Rows[i]["CHG_STATUS_desc"].ToString());
                            row.GetCell(14).CellStyle = stringRightStyle;
                            for (int j = 17; j <= 40; j++)
                            {
                                row.GetCell(j).CellStyle = stringRightStyle;
                            }

                        }

                    }
                    //製表日期
                    ICellStyle stringLeftStyleDate = this.setCellStyle(workbook, "left", false);
                    row = sheet.GetRow(0);
                    ICell cell = row.CreateCell(43);
                    cell.CellStyle = stringLeftStyleDate;
                    cell.SetCellValue("製表日期:" + DateTime.Now.ToString("yyyy/MM/dd"));

                    for (int k = 0; k <= 42; k++)
                    {
                        if (k != 37)
                            sheet.AutoSizeColumn(k);
                    }
                    return workbook;
                    //匯出Excel
                    //if (data == "mantain")
                    //    ExcelHandle.exportExcel(workbook, "維護資料." + type);
                    //else
                    //    ExcelHandle.exportExcel(workbook, "原始資料." + type);
                }
                return null;
            }
            else if (data == "example")
            {
                return workbook;
                //匯出Excel
                //ExcelHandle.exportExcel(workbook, "上傳範例." + type);
            }
            return null;


        }
        catch (Exception)
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
    //excel上傳
    public IWorkbook updateExcelData(Stream fs, string type, string BONUS_YEAR, string BONUS_DT)
    {
        CFB2SI0100DAO si010DAO = new CFB2SI0100DAO();
        IWorkbook workbook = null;
        si010DAO.getSatrtAndEndDT(BONUS_YEAR);
        //年獎開始日期,結束日期
        string start_DT = si010DAO.BONUS_SDT;
        string end_DT = si010DAO.BONUS_EDT;


        //取得範本sheet
        ISheet sheet = null;
        try
        {
            bool valid = true;

            //IWorkbook workbook;
            //依附檔名判斷要用哪種方式讀取
            if (type == ".xls")
            {
                workbook = new HSSFWorkbook(fs);
            }
            else if (type == ".xlsx")
            {
                workbook = new XSSFWorkbook(fs);
            }

            //取得sheet
            sheet = workbook.GetSheetAt(0);
            if (sheet != null)
            {
                #region cell陣列
                string[] cell1 = new string[sheet.LastRowNum + 1];
                string[] cell2 = new string[sheet.LastRowNum + 1];
                string[] cell3 = new string[sheet.LastRowNum + 1];
                string[] cell4 = new string[sheet.LastRowNum + 1];
                string[] cell5 = new string[sheet.LastRowNum + 1];
                string[] cell6 = new string[sheet.LastRowNum + 1];
                string[] cell7 = new string[sheet.LastRowNum + 1];
                string[] cell8 = new string[sheet.LastRowNum + 1];
                string[] cell9 = new string[sheet.LastRowNum + 1];
                string[] cell10 = new string[sheet.LastRowNum + 1];
                string[] cell11 = new string[sheet.LastRowNum + 1];
                string[] cell12 = new string[sheet.LastRowNum + 1];
                string[] cell13 = new string[sheet.LastRowNum + 1];
                string[] cell14 = new string[sheet.LastRowNum + 1];
                string[] cell15 = new string[sheet.LastRowNum + 1];
                string[] cell16 = new string[sheet.LastRowNum + 1];
                string[] cell17 = new string[sheet.LastRowNum + 1];
                string[] cell18 = new string[sheet.LastRowNum + 1];
                string[] cell19 = new string[sheet.LastRowNum + 1];
                string[] cell20 = new string[sheet.LastRowNum + 1];
                string[] cell21 = new string[sheet.LastRowNum + 1];
                string[] cell22 = new string[sheet.LastRowNum + 1];
                string[] cell23 = new string[sheet.LastRowNum + 1];
                string[] cell24 = new string[sheet.LastRowNum + 1];
                string[] cell25 = new string[sheet.LastRowNum + 1];
                string[] cell26 = new string[sheet.LastRowNum + 1];
                string[] cell27 = new string[sheet.LastRowNum + 1];
                string[] cell28 = new string[sheet.LastRowNum + 1];
                string[] cell29 = new string[sheet.LastRowNum + 1];
                string[] cell30 = new string[sheet.LastRowNum + 1];
                string[] cell31 = new string[sheet.LastRowNum + 1];
                string[] cell32 = new string[sheet.LastRowNum + 1];
                string[] cell33 = new string[sheet.LastRowNum + 1];
                //string[] cell34 = new string[sheet.LastRowNum + 1];
                //string[] cell35 = new string[sheet.LastRowNum + 1];
                //string[] cell36 = new string[sheet.LastRowNum + 1];
                //string[] cell37 = new string[sheet.LastRowNum + 1];
                string[] operate = new string[sheet.LastRowNum + 1];
                decimal[] WK_ATTEND_DAYS = new decimal[sheet.LastRowNum + 1]; //WK勤怠扣除天數
                decimal[] WK_REWARD_DAYS = new decimal[sheet.LastRowNum + 1]; //WK獎懲加減天數
                decimal[] WK_DISCIPLINE_DAYS = new decimal[sheet.LastRowNum + 1]; //WK紀律扣除天數
                decimal[] WK_BONUS_WORK_DAYS = new decimal[sheet.LastRowNum + 1]; //實際在職天數(WK在職天數 - WK勤怠扣除天數)
                decimal wk_leave_B0 = 0;            //有薪病假時數
                decimal wk_leave_B0_over30 = 0;     //有薪病假時數_超過30天

                #endregion

                #region 參數檔取值
                decimal B_LEAVE_UC = 0; //紅利-勤怠事假
                decimal B_LEAVE_B = 0;   //紅利-勤怠有薪病假
                decimal B_LEAVE_B_over30 = 0;   //紅利-勤怠有薪病假
                decimal B_LEAVE_Q = 0;   //紅利-曠工
                decimal B_LEAVE_OP = 0; //紅利-遲/早
                decimal B_FIRST_CNT_P = 0;   //紅利-嘉獎
                decimal B_SECOND_CNT_P = 0; //紅利-小功
                decimal B_THIRD_CNT_P = 0;   //紅利-大功
                decimal B_FIRST_CNT_M = 0;   //紅利-申誡
                decimal B_SECOND_CNT_M = 0; //紅利-小過
                decimal B_THIRD_CNT_M = 0;   //紅利-大過
                DataTable dt_param = utilities.getParameter("SI", "B_LEAVE_UC");
                if (dt_param.Rows.Count > 0)
                {
                    B_LEAVE_UC = Convert.ToDecimal(dt_param.Rows[0]["CODE_VAL1"]);
                }
                dt_param = utilities.getParameter("SI", "B_LEAVE_B");
                if (dt_param.Rows.Count > 0)
                {
                    B_LEAVE_B = Convert.ToDecimal(dt_param.Rows[0]["CODE_VAL1"]);
                }

                dt_param = utilities.getParameter("SI", "B_LEAVE_B_OVER30");
                if (dt_param.Rows.Count > 0)
                {
                    B_LEAVE_B_over30 = Convert.ToDecimal(dt_param.Rows[0]["CODE_VAL1"]);
                }

                dt_param = utilities.getParameter("SI", "B_LEAVE_Q");
                if (dt_param.Rows.Count > 0)
                {
                    B_LEAVE_Q = Convert.ToDecimal(dt_param.Rows[0]["CODE_VAL1"]);
                }

                dt_param = utilities.getParameter("SI", "B_LEAVE_OP");
                if (dt_param.Rows.Count > 0)
                {
                    B_LEAVE_OP = Convert.ToDecimal(dt_param.Rows[0]["CODE_VAL1"]);
                }

                dt_param = utilities.getParameter("SI", "B_FIRST_CNT_P");
                if (dt_param.Rows.Count > 0)
                {
                    B_FIRST_CNT_P = Convert.ToDecimal(dt_param.Rows[0]["CODE_VAL1"]);
                }

                dt_param = utilities.getParameter("SI", "B_SECOND_CNT_P");
                if (dt_param.Rows.Count > 0)
                {
                    B_SECOND_CNT_P = Convert.ToDecimal(dt_param.Rows[0]["CODE_VAL1"]);
                }

                dt_param = utilities.getParameter("SI", "B_THIRD_CNT_P");
                if (dt_param.Rows.Count > 0)
                {
                    B_THIRD_CNT_P = Convert.ToDecimal(dt_param.Rows[0]["CODE_VAL1"]);
                }

                dt_param = utilities.getParameter("SI", "B_FIRST_CNT_M");
                if (dt_param.Rows.Count > 0)
                {
                    B_FIRST_CNT_M = Convert.ToDecimal(dt_param.Rows[0]["CODE_VAL1"]);
                }

                dt_param = utilities.getParameter("SI", "B_SECOND_CNT_M");
                if (dt_param.Rows.Count > 0)
                {
                    B_SECOND_CNT_M = Convert.ToDecimal(dt_param.Rows[0]["CODE_VAL1"]);
                }

                dt_param = utilities.getParameter("SI", "B_THIRD_CNT_M");
                if (dt_param.Rows.Count > 0)
                {
                    B_THIRD_CNT_M = Convert.ToDecimal(dt_param.Rows[0]["CODE_VAL1"]);
                }

                #endregion

                #region 紅利維護檔.紅利反映項目取值
                string BONUS_ITEM_RP = ""; //紅利反映項目-獎懲
                string BONUS_ITEM_AL = "";   //紅利反映項目-勤怠
                string BONUS_ITEM_D = "";   //紅利反映項目-紀律
                decimal BONUS_DAYS = 0; //紅利發放天數
                DataTable dt_TB_S_M_BONUS_H = si010DAO.Get_TB_S_M_BONUS_H(BONUS_YEAR);
                foreach (DataRow dr in dt_TB_S_M_BONUS_H.Rows)
                {
                    BONUS_ITEM_RP = Convert.ToString(dr["BONUS_ITEM_RP"]);
                    BONUS_ITEM_AL = Convert.ToString(dr["BONUS_ITEM_AL"]);
                    BONUS_ITEM_D = Convert.ToString(dr["BONUS_ITEM_D"]);
                    BONUS_DAYS = Convert.ToDecimal(dr["BONUS_DAYS"]);
                }
                #endregion
                //巡覽每row的資料第一列為title跳過
                for (int i = 3; i <= sheet.LastRowNum; i++)
                {
                    if (sheet.GetRow(i) != null)
                    {
                        #region 讀取cell資料，第一欄為檢核結果欄位跳過
                        cell1[i] = sheet.GetRow(i).GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString();
                        cell2[i] = sheet.GetRow(i).GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString();
                        cell3[i] = sheet.GetRow(i).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString();
                        cell4[i] = sheet.GetRow(i).GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString();
                        cell5[i] = sheet.GetRow(i).GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString();
                        cell6[i] = sheet.GetRow(i).GetCell(6, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString();
                        cell7[i] = sheet.GetRow(i).GetCell(7, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString();
                        cell8[i] = sheet.GetRow(i).GetCell(8, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString();
                        cell9[i] = sheet.GetRow(i).GetCell(9, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString();
                        cell10[i] = sheet.GetRow(i).GetCell(10, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString();
                        cell11[i] = sheet.GetRow(i).GetCell(11, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString();
                        cell12[i] = sheet.GetRow(i).GetCell(12, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString();
                        cell13[i] = sheet.GetRow(i).GetCell(13, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString();
                        cell14[i] = sheet.GetRow(i).GetCell(14, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString();
                        cell15[i] = sheet.GetRow(i).GetCell(15, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString();
                        cell16[i] = sheet.GetRow(i).GetCell(16, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString();
                        cell17[i] = sheet.GetRow(i).GetCell(17, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString();
                        cell18[i] = sheet.GetRow(i).GetCell(18, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString();
                        cell19[i] = sheet.GetRow(i).GetCell(19, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString();
                        cell20[i] = sheet.GetRow(i).GetCell(20, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString();
                        cell21[i] = sheet.GetRow(i).GetCell(21, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString();
                        cell22[i] = sheet.GetRow(i).GetCell(22, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString();
                        cell23[i] = sheet.GetRow(i).GetCell(23, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString();
                        cell24[i] = sheet.GetRow(i).GetCell(24, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString();
                        cell25[i] = sheet.GetRow(i).GetCell(25, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString();
                        cell26[i] = sheet.GetRow(i).GetCell(26, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString();
                        cell27[i] = sheet.GetRow(i).GetCell(27, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString();
                        cell28[i] = sheet.GetRow(i).GetCell(28, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString();
                        cell29[i] = sheet.GetRow(i).GetCell(29, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString();
                        cell30[i] = sheet.GetRow(i).GetCell(30, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString();
                        cell31[i] = sheet.GetRow(i).GetCell(31, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString();
                        cell32[i] = sheet.GetRow(i).GetCell(32, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString();
                        cell33[i] = sheet.GetRow(i).GetCell(33, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString();
                        //cell34[i] = sheet.GetRow(i).GetCell(34, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString();
                        //cell35[i] = sheet.GetRow(i).GetCell(35, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString();
                        //cell36[i] = sheet.GetRow(i).GetCell(36, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString();
                        //cell37[i] = sheet.GetRow(i).GetCell(37, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString();
                        operate[i] = "";
                        #endregion

                        string error = "";
                        //int numCheckResult = 0;
                        DateTime dateCheckResult = DateTime.MinValue;
                        decimal decCheckResult = 0;
                        //檢查工號是否在TB_S_M_BONUS_D
                        DataTable tmp = si010DAO.check_EMP_ID(BONUS_YEAR, cell1[i]);
                        #region 新增系統抓值
                        if (tmp.Rows.Count == 0)
                        {
                            //檢查工號是否在VW_H_EMP_DATA
                            DataTable tmp2 = si010DAO.check_EMP_ID2(cell1[i]);
                            //BeginTransaction();
                            if (tmp2.Rows.Count == 0)
                            {
                                error += "工號不存在\n";
                            }

                            else
                            {
                                operate[i] = "add";

                                try
                                {
                                    //將view資料放進cell
                                    DataTable dt = si010DAO.viewToCell(cell1[i]);
                                    DataTable dt2 = si010DAO.getID_DESC(cell1[i], start_DT, end_DT);

                                    foreach (DataRow dr in dt.Rows)
                                    {
                                        cell2[i] = Convert.ToString(dr["EMP_NAME"]);
                                        cell3[i] = Convert.ToString(dr["EMP_CHG_CD"]);
                                        cell4[i] = Convert.ToString(dr["WS_CD"]);
                                        cell5[i] = Convert.ToString(dr["JPN_CD"]);
                                        cell6[i] = Convert.ToString(dr["DEPT_NO"]);
                                        cell7[i] = Convert.ToString(dr["LEVEL_CD"]);
                                        cell8[i] = Convert.ToString(dr["PJOB_CD"]);
                                        cell9[i] = Convert.ToString(dr["JOIN_DT"]);
                                        cell10[i] = Convert.ToString(dr["LEAVE_DT"]);
                                        string status = Convert.ToString(dr["EMP_STATUS"]);
                                        if (status == "02")
                                        {
                                            cell11[i] = Convert.ToString(dr["LEAVE_DT"]);
                                        }
                                        else
                                        {
                                            cell11[i] = "";
                                        }

                                        cell12[i] = Convert.ToString(dr["BE_CONTRACT_DT"]);
                                        cell13[i] = Convert.ToString(dr["BE_EMP_DT"]);
                                        cell15[i] = Convert.ToString(dr["EMP_CD"]);

                                    }
                                    //身分標示
                                    foreach (DataRow dr in dt2.Rows)
                                    {
                                        cell16[i] = Convert.ToString(dr["ID_DESC"]);
                                    }
                                }
                                catch
                                {
                                    throw;
                                }
                            }
                        }
                        #endregion

                        #region 修改
                        else
                        {
                            operate[i] = "modify";
                        }
                        #endregion

                        #region 檢核
                        //檢查長度
                        if (cell14[i].Length > 5)
                            error += "在職天數-紅利期間長度大於DB欄位長度\n";
                        //if (cell16[i].Length > 30)
                        //    error += "身份標示長度大於DB欄位長度\n";
                        if (cell17[i].Replace(",", "").Length > 7)
                            error += "職能俸長度大於DB欄位長度\n";
                        if (cell18[i].Replace(",", "").Length > 7)
                            error += "資格俸長度大於DB欄位長度\n";
                        if (cell19[i].Replace(",", "").Length > 7)
                            error += "職務俸長度大於DB欄位長度\n";
                        if (cell20[i].Replace(",", "").Length > 7)
                            error += "專業俸長度大於DB欄位長度\n";
                        if (cell21[i].Replace(",", "").Length > 10)
                            error += "伙食津貼長度大於DB欄位長度\n";
                        error += this.checkNumberWithPoint(cell22[i], "事假時數", 4, 1);
                        error += this.checkNumberWithPoint(cell23[i], "有薪病假時數", 4, 1);
                        error += this.checkNumberWithPoint(cell24[i], "無薪病假時數", 4, 1);
                        error += this.checkNumberWithPoint(cell25[i], "曠工時數", 4, 1);
                        //if (cell22[i].Length > 5)
                        //    error += "事假時數長度大於DB欄位長度\n";
                        //if (cell23[i].Length > 5)
                        //    error += "有薪病假時數長度大於DB欄位長度\n";
                        //if (cell24[i].Length > 5)
                        //    error += "無薪病假時數長度大於DB欄位長度\n";
                        //if (cell25[i].Length > 5)
                        //    error += "曠工時數長度大於DB欄位長度\n";
                        if (cell26[i].Length > 3)
                            error += "遲到/早退次數長度大於DB欄位長度\n";
                        if (cell27[i].Length > 2)
                            error += "嘉獎長度大於DB欄位長度\n";
                        if (cell28[i].Length > 2)
                            error += "小功長度大於DB欄位長度\n";
                        if (cell29[i].Length > 2)
                            error += "大功長度大於DB欄位長度\n";
                        if (cell30[i].Length > 2)
                            error += "申誡長度大於DB欄位長度\n";
                        if (cell31[i].Length > 2)
                            error += "小過長度大於DB欄位長度\n";
                        if (cell32[i].Length > 2)
                            error += "大過長度大於DB欄位長度\n";
                        if (cell33[i].Length > 1)
                            error += "支付狀態長度大於DB欄位長度\n";

                        //檢查必填
                        if (cell1[i] == "")
                            error += "員工工號必須有值!\n";
                        if (cell14[i] == "")
                            error += "在職天數-紅利期間必須有值!\n";
                        if (cell33[i] == "")
                            error += "支付狀態必須有值!\n";
                        //檢查數字
                        if (cell17[i] == "")
                            error += "職能俸必須有值!\n";
                        else
                        {
                            if (!decimal.TryParse(cell17[i].Trim(), out decCheckResult))
                                error += "職能俸非正確數字格式!\n";
                            //if (numCheckResult < 0)
                            //    error += "職能俸非正確數字格式!\n";
                        }

                        if (cell18[i] == "")
                            error += "資格俸必須有值!\n";
                        else
                        {
                            if (!decimal.TryParse(cell18[i].Trim(), out decCheckResult))
                                error += "資格俸非正確數字格式!\n";
                        }

                        if (cell19[i] == "")
                            error += "職務俸必須有值!\n";
                        else
                        {
                            if (!decimal.TryParse(cell19[i].Trim(), out decCheckResult))
                                error += "職務俸非正確數字格式!\n";
                        }

                        if (cell20[i] == "")
                            error += "專業俸必須有值!\n";
                        else
                        {
                            if (!decimal.TryParse(cell20[i].Trim(), out decCheckResult))
                                error += "專業俸非正確數字格式!\n";
                        }

                        if (cell21[i] == "")
                            error += "伙食津貼必須有值!\n";
                        else
                        {
                            if (!decimal.TryParse(cell21[i].Trim(), out decCheckResult))
                                error += "伙食津貼非正確數字格式!\n";
                        }

                        if (cell22[i] == "")
                            error += "事假時數必須有值!\n";
                        else
                        {
                            if (!decimal.TryParse(cell22[i].Trim(), out decCheckResult))
                                error += "事假時數非正確數字格式!\n";
                        }

                        if (cell23[i] == "")
                            error += "有薪病假時數必須有值!\n";
                        else
                        {
                            if (!decimal.TryParse(cell23[i].Trim(), out decCheckResult))
                                error += "有薪病假時數非正確數字格式!\n";
                        }

                        if (cell24[i] == "")
                            error += "無薪病假時數必須有值!\n";
                        else
                        {
                            if (!decimal.TryParse(cell24[i].Trim(), out decCheckResult))
                                error += "無薪病假時數非正確數字格式!\n";
                        }

                        if (cell25[i] == "")
                            error += "曠工時數必須有值!\n";
                        else
                        {
                            if (!decimal.TryParse(cell25[i].Trim(), out decCheckResult))
                                error += "曠工時數非正確數字格式!\n";
                        }

                        if (cell26[i] == "")
                            error += "遲到/早退次數必須有值!\n";
                        else
                        {
                            if (!decimal.TryParse(cell26[i].Trim(), out decCheckResult))
                                error += "遲到/早退次數非正確數字格式!\n";
                        }

                        if (cell27[i] == "")
                            error += "嘉獎必須有值!\n";
                        else
                        {
                            if (!decimal.TryParse(cell27[i].Trim(), out decCheckResult))
                                error += "嘉獎非正確數字格式!\n";
                        }

                        if (cell28[i] == "")
                            error += "小功必須有值!\n";
                        else
                        {
                            if (!decimal.TryParse(cell28[i].Trim(), out decCheckResult))
                                error += "小功非正確數字格式!\n";
                        }

                        if (cell29[i] == "")
                            error += "大功必須有值!\n";
                        else
                        {
                            if (!decimal.TryParse(cell29[i].Trim(), out decCheckResult))
                                error += "大功非正確數字格式!\n";
                        }

                        if (cell30[i] == "")
                            error += "申誡必須有值!\n";
                        else
                        {
                            if (!decimal.TryParse(cell30[i].Trim(), out decCheckResult))
                                error += "申誡非正確數字格式!\n";
                        }

                        if (cell31[i] == "")
                            error += "小過必須有值!\n";
                        else
                        {
                            if (!decimal.TryParse(cell31[i].Trim(), out decCheckResult))
                                error += "小過非正確數字格式!\n";
                        }

                        if (cell32[i] == "")
                            error += "大過必須有值!\n";
                        else
                        {
                            if (!decimal.TryParse(cell32[i].Trim(), out decCheckResult))
                                error += "大過非正確數字格式!\n";
                        }
                        #endregion

                        //將錯誤訊息寫進EXCEL第一欄
                        sheet.GetRow(i).GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(error);
                        if (error != "")
                        {
                            valid = false;
                        }
                        else
                        {
                            #region WK計算
                            try
                            {
                                wk_leave_B0 = 0;
                                wk_leave_B0_over30 = 0;
                                //WK勤怠扣除天數計算
                                if (Convert.ToDecimal(cell23[i]) > 30 * 8)
                                {
                                    wk_leave_B0_over30 = Convert.ToDecimal(cell23[i]) - 30 * 8;
                                    wk_leave_B0 = 30 * 8;
                                }
                                else
                                {
                                    wk_leave_B0 = Convert.ToDecimal(cell23[i]);
                                    wk_leave_B0_over30 = 0;
                                }

                                /*
                                WK_ATTEND_DAYS[i] = (Convert.ToDecimal(cell22[i]) + Convert.ToDecimal(cell24[i])) / 8 * B_LEAVE_UC
                                    + wk_leave_B0 / 8 * B_LEAVE_B + wk_leave_B0_over30 / 8 * B_LEAVE_B_over30;
                                */
                                //實際在職天數
                                WK_BONUS_WORK_DAYS[i] = Convert.ToDecimal(cell14[i]);

                                //WK紀律扣除天數計算
                                WK_DISCIPLINE_DAYS[i] = Convert.ToDecimal(cell25[i]) / 8 * B_LEAVE_Q;
                                if (Convert.ToDecimal(cell26[i]) >= 19)
                                    WK_DISCIPLINE_DAYS[i] = WK_DISCIPLINE_DAYS[i] + (Convert.ToDecimal(cell26[i]) - 18) * B_LEAVE_OP;
                                //WK獎懲加減天數
                                WK_REWARD_DAYS[i] = Convert.ToDecimal(cell29[i]) * B_FIRST_CNT_P + Convert.ToDecimal(cell28[i]) * B_SECOND_CNT_P + Convert.ToDecimal(cell27[i]) * B_THIRD_CNT_P 
                                                    + (Convert.ToDecimal(cell32[i]) * B_FIRST_CNT_M + Convert.ToDecimal(cell31[i]) * B_SECOND_CNT_M + Convert.ToDecimal(cell30[i]) * B_THIRD_CNT_M);


                                //發放Base																																																																																							
                                decimal BASE = Math.Round((Convert.ToDecimal(cell17[i]) + Convert.ToDecimal(cell18[i]) + Convert.ToDecimal(cell19[i]) + Convert.ToDecimal(cell20[i]) + Convert.ToDecimal(cell21[i])) / 30, 2);
                                //在職比例																																																																																				
                                decimal CHG_RATE = Math.Round((Convert.ToDecimal(cell14[i]) - WK_ATTEND_DAYS[i]) / 365, 2);
                                if (BONUS_ITEM_AL != "Y")
                                    CHG_RATE = Math.Round((Convert.ToDecimal(cell14[i]) - 0) / 365, 2);
                                //紀律反映
                                decimal DISCIPLINE = WK_REWARD_DAYS[i] + WK_DISCIPLINE_DAYS[i];
                                if (BONUS_ITEM_RP != "Y")
                                    DISCIPLINE = WK_DISCIPLINE_DAYS[i];
                                if (BONUS_ITEM_D != "Y")
                                    DISCIPLINE = WK_REWARD_DAYS[i];
                                if (BONUS_ITEM_RP != "Y" && BONUS_ITEM_D != "Y")
                                    DISCIPLINE = 0;
                            }
                            catch
                            {
                                throw;
                            }
                            #endregion
                        }
                    }
                }
                #region 有錯匯出excel,沒錯寫入DB
                if (!valid)
                {
                    return workbook;
                    //檢核有錯，匯出附加說明的excel
                    //ExcelHandle.exportExcel(workbook, "error" + type);
                }
                else
                {
                    DateTime now = DateTime.Now;
                    for (int i = 3; i <= sheet.LastRowNum; i++)
                    {
                        //新增
                        if (operate[i] == "add")
                        {
                            try
                            {
                                BeginTransaction();
                                si010DAO.Add(BONUS_YEAR, BONUS_DT,
                                         cell1[i], cell2[i], cell3[i], cell4[i], cell5[i], cell6[i], cell7[i], cell8[i], cell9[i], cell10[i],
                                         cell11[i], cell12[i], cell13[i], cell14[i], cell15[i], cell16[i], cell17[i], cell18[i], cell19[i], cell20[i],
                                         cell21[i], cell22[i], cell23[i], cell24[i], cell25[i], cell26[i], cell27[i], cell28[i], cell29[i], cell30[i],
                                         cell31[i], cell32[i], WK_ATTEND_DAYS[i], WK_REWARD_DAYS[i], WK_DISCIPLINE_DAYS[i], WK_BONUS_WORK_DAYS[i], cell33[i], now);
                                Commit();
                            }
                            catch (Exception ex)
                            {
                                RollBack();
                                throw;
                                //return ex.Message;
                            }
                        }
                        //修改
                        else if (operate[i] == "modify")
                        {
                            string CHG_STATUS = "";
                            string APPROVE_FLAG = "";
                            string PRIMEVAL_FLAG = "";
                            string APPROVE_STATUS = "";
                            DataTable dt = si010DAO.premodify(BONUS_YEAR, cell1[i]);
                            foreach (DataRow dr in dt.Rows)
                            {
                                CHG_STATUS = Convert.ToString(dr["CHG_STATUS"]);
                                APPROVE_FLAG = Convert.ToString(dr["APPROVE_FLAG"]);
                                PRIMEVAL_FLAG = Convert.ToString(dr["PRIMEVAL_FLAG"]);

                            }

                            dt = si010DAO.premodify2(BONUS_YEAR);
                            foreach (DataRow dr in dt.Rows)
                            {
                                APPROVE_STATUS = Convert.ToString(dr["APPROVE_STATUS"]);
                            }
                            si010DAO.EMP_CD = cell15[i].Split('-')[0];
                            try
                            {
                                BeginTransaction();

                                si010DAO.modify(CHG_STATUS, APPROVE_FLAG, PRIMEVAL_FLAG, APPROVE_STATUS, BONUS_YEAR, cell1[i], cell14[i],
                                            cell17[i], cell18[i], cell19[i], cell20[i], cell21[i], cell22[i], cell23[i], cell24[i], cell25[i], cell26[i],
                                            cell27[i], cell28[i], cell29[i], cell30[i], cell31[i], cell32[i], WK_ATTEND_DAYS[i], WK_REWARD_DAYS[i],
                                            WK_DISCIPLINE_DAYS[i], WK_BONUS_WORK_DAYS[i], cell33[i], now);
                                Commit();
                            }
                            catch (Exception ex)
                            {
                                RollBack();
                                throw;
                                //return ex.Message;
                            }
                        }
                        
                        try
                        {
                            BeginTransaction();
                            //更新紅利維護檔
                            si010DAO.update(BONUS_YEAR);

                            //更新明細維護檔的年獎金額為0
                            si010DAO.updateT0Zero_D("TB_S_M_BONUS_D", BONUS_YEAR,now);
                            si010DAO.updateT0Zero_D("TB_S_S_BONUS_D", BONUS_YEAR,now);
                            Commit();
                        }
                        catch (Exception ex)
                        {
                            RollBack();
                            throw;
                            //return ex.Message;
                        }
                    }

                }
                #endregion
            }
            return null;
            //return "0";
        }
        catch (Exception ex)
        {
            //return ex.Message;
            throw;
        }
    }

    //檢查是否為數字(含小數)
    public string checkNumberWithPoint(string cellData, string cellName, int cellLength, int dotLength)
    {
        try
        {
            String error = "";
            double numCheckResult = 0;
            cellData = cellData.Replace(",", "");
            double maxValue = Math.Pow(10, cellLength);

            if (cellData == "")
                error += cellName + "不可空白\n";
            else
            {

                if (!double.TryParse(cellData.Trim(), out numCheckResult))
                {
                    error += cellName + "必須為數字, 且必須為整數" + cellLength + "位，小數" + dotLength + "位, \n";
                }
                else
                {
                    if (double.Parse(cellData.Trim()) > maxValue)
                    {
                        error += cellName + "必須為數字, 且必須為整數" + cellLength + "位，小數" + dotLength + "位, \n";
                    }
                }

            }

            return error;
        }
        catch (Exception)
        {
            throw;
        }
    }





}