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
/// CFB2SI0300BO 的摘要描述
/// </summary>
public class CFB2SI0300BO : BaseService
{
    public CFB2SI0300BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    //下載Excel資料
    public IWorkbook createExcelFromTemplate(string type, string excelPath, string data, string BONUS_YEAR)
    {
        CFB2SI0300DAO fb2si = new CFB2SI0300DAO();
        FileStream fs = null;
        IWorkbook workbook = null;

        //取得範本sheet
        ISheet sheet = null;
        try
        {
            //IWorkbook workbook;
            fs = new FileStream(excelPath, FileMode.Open, FileAccess.Read);
            //依type判斷要用哪種方式產生
            if (type == "xls")
                workbook = new HSSFWorkbook(fs);
            else
                workbook = new XSSFWorkbook(fs);

            //取得範本sheet
            #region 本次核可資料
            if (data == "this")
            {
                sheet = workbook.GetSheetAt(0);
                int x = 0;
                if (sheet != null)
                {
                    DataTable dt = fb2si.getExcelData(data, BONUS_YEAR);
                    IRow row;
                    ICell cell;
                    ICellStyle stringRightStyle = this.setCellStyle(workbook, "right", false);
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            x = i + 3;
                            //將資料寫入範本
                            row = sheet.CreateRow(x);
                            cell = row.CreateCell(1);
                            cell.SetCellValue(dt.Rows[i]["EMP_ID"].ToString());
                            cell = row.CreateCell(2);
                            cell.SetCellValue(dt.Rows[i]["EMP_NAME"].ToString());
                            cell = row.CreateCell(3);
                            cell.SetCellValue(dt.Rows[i]["EMP_CHG_CD_desc"].ToString());
                            cell = row.CreateCell(4);
                            cell.SetCellValue(dt.Rows[i]["WS_CD"].ToString());
                            cell = row.CreateCell(5);
                            cell.SetCellValue(dt.Rows[i]["JPN_CD"].ToString());
                            cell = row.CreateCell(6);
                            cell.SetCellValue(dt.Rows[i]["DEPT_NO"].ToString());
                            cell = row.CreateCell(7);
                            cell.SetCellValue(dt.Rows[i]["LEVEL_CD"].ToString());
                            cell = row.CreateCell(8);
                            cell.SetCellValue(dt.Rows[i]["PJOB_CD"].ToString());

                            if (dt.Rows[i]["JOIN_DT"] != DBNull.Value)
                            {
                                cell = row.CreateCell(9);
                                cell.SetCellValue(Convert.ToDateTime(dt.Rows[i]["JOIN_DT"]).ToString("yyyy/MM/dd"));
                            }

                            if (dt.Rows[i]["LEAVE_DT"] != DBNull.Value)
                            {
                                cell = row.CreateCell(10);
                                cell.SetCellValue(Convert.ToDateTime(dt.Rows[i]["LEAVE_DT"]).ToString("yyyy/MM/dd"));
                            }
                            if (dt.Rows[i]["STAY_DT"] != DBNull.Value)
                            {
                                cell = row.CreateCell(11);
                                cell.SetCellValue(Convert.ToDateTime(dt.Rows[i]["STAY_DT"]).ToString("yyyy/MM/dd"));
                            }
                            if (dt.Rows[i]["BE_CONTRACT_DT"] != DBNull.Value)
                            {
                                cell = row.CreateCell(12);
                                cell.SetCellValue(Convert.ToDateTime(dt.Rows[i]["BE_CONTRACT_DT"]).ToString("yyyy/MM/dd"));
                            }
                            if (dt.Rows[i]["BE_EMP_DT"] != DBNull.Value)
                            {
                                cell = row.CreateCell(13);
                                cell.SetCellValue(Convert.ToDateTime(dt.Rows[i]["BE_EMP_DT"]).ToString("yyyy/MM/dd"));
                            }
                            cell = row.CreateCell(14);
                            cell.CellStyle = stringRightStyle;
                            cell.SetCellValue(dt.Rows[i]["WORK_DAYS"].ToString());
                            cell = row.CreateCell(15);
                            cell.SetCellValue(dt.Rows[i]["EMP_CD_desc"].ToString());
                            cell = row.CreateCell(16);
                            cell.SetCellValue(dt.Rows[i]["ID_DESC"].ToString());
                            cell = row.CreateCell(17);
                            cell.CellStyle = stringRightStyle;
                            //2021.07.27 fix 
                            if (dt.Rows[i]["ABILITY_PAY"].ToString() == "")
                                cell.SetCellValue(String.Format("{0:N0}", int.Parse("0")));
                            else
                                cell.SetCellValue(String.Format("{0:N0}", int.Parse(dt.Rows[i]["ABILITY_PAY"].ToString())));
                            cell = row.CreateCell(18);
                            cell.CellStyle = stringRightStyle;
                            if (dt.Rows[i]["LEVEL_PAY"].ToString() == "")
                                cell.SetCellValue(String.Format("{0:N0}", int.Parse("0")));
                            else
                                cell.SetCellValue(String.Format("{0:N0}", int.Parse(dt.Rows[i]["LEVEL_PAY"].ToString())));
                            cell = row.CreateCell(19);
                            cell.CellStyle = stringRightStyle;
                            if (dt.Rows[i]["PJOB_PAY"].ToString() == "")
                                cell.SetCellValue(String.Format("{0:N0}", int.Parse("0")));
                            else
                                cell.SetCellValue(String.Format("{0:N0}", int.Parse(dt.Rows[i]["PJOB_PAY"].ToString())));
                            cell = row.CreateCell(20);
                            cell.CellStyle = stringRightStyle;
                            if (dt.Rows[i]["PROFESSION_PAY"].ToString() == "")
                                cell.SetCellValue(String.Format("{0:N0}", int.Parse("0")));
                            else
                                cell.SetCellValue(String.Format("{0:N0}", int.Parse(dt.Rows[i]["PROFESSION_PAY"].ToString())));
                            cell = row.CreateCell(21);
                            cell.CellStyle = stringRightStyle;
                            if (dt.Rows[i]["FOOD_SUBSIDY"].ToString() == "")
                                cell.SetCellValue(String.Format("{0:N0}", int.Parse("0")));
                            else
                                cell.SetCellValue(String.Format("{0:N0}", int.Parse(dt.Rows[i]["FOOD_SUBSIDY"].ToString())));
                            cell = row.CreateCell(22);
                            cell.CellStyle = stringRightStyle;
                            cell.SetCellValue(dt.Rows[i]["LEAVE_A_HOUR"].ToString());
                            cell = row.CreateCell(23);
                            cell.CellStyle = stringRightStyle;
                            cell.SetCellValue(dt.Rows[i]["LEAVE_B_HOUR"].ToString());
                            cell = row.CreateCell(24);
                            cell.CellStyle = stringRightStyle;
                            cell.SetCellValue(dt.Rows[i]["LEAVE_C_HOUR"].ToString());
                            cell = row.CreateCell(25);
                            cell.CellStyle = stringRightStyle;
                            cell.SetCellValue(dt.Rows[i]["LEAVE_Q_HOUR"].ToString());
                            cell = row.CreateCell(26);
                            cell.CellStyle = stringRightStyle;
                            cell.SetCellValue(dt.Rows[i]["LEAVE_OP_HOUR"].ToString());
                            cell = row.CreateCell(27);
                            cell.CellStyle = stringRightStyle;
                            cell.SetCellValue(dt.Rows[i]["THIRD_CNT_P"].ToString());
                            cell = row.CreateCell(28);
                            cell.CellStyle = stringRightStyle;
                            cell.SetCellValue(dt.Rows[i]["SECOND_CNT_P"].ToString());
                            cell = row.CreateCell(29);
                            cell.CellStyle = stringRightStyle;
                            cell.SetCellValue(dt.Rows[i]["FIRST_CNT_P"].ToString());
                            cell = row.CreateCell(30);
                            cell.CellStyle = stringRightStyle;
                            cell.SetCellValue(dt.Rows[i]["THIRD_CNT_M"].ToString());
                            cell = row.CreateCell(31);
                            cell.CellStyle = stringRightStyle;
                            cell.SetCellValue(dt.Rows[i]["SECOND_CNT_M"].ToString());
                            cell = row.CreateCell(32);
                            cell.CellStyle = stringRightStyle;
                            cell.SetCellValue(dt.Rows[i]["FIRST_CNT_M"].ToString());
                            cell = row.CreateCell(33);
                            cell.CellStyle = stringRightStyle;
                            cell.SetCellValue(dt.Rows[i]["ATTEND_DAYS"].ToString());
                            cell = row.CreateCell(34);
                            cell.CellStyle = stringRightStyle;
                            cell.SetCellValue(dt.Rows[i]["REWARD_DAYS"].ToString());
                            cell = row.CreateCell(35);
                            cell.CellStyle = stringRightStyle;
                            cell.SetCellValue(dt.Rows[i]["DISCIPLINE_DAYS"].ToString());
                            cell = row.CreateCell(36);
                            cell.CellStyle = stringRightStyle;
                            cell.SetCellValue(dt.Rows[i]["BONUS_WORK_DAYS"].ToString());
                            //cell = row.CreateCell(37);
                            //cell.SetCellValue(dt.Rows[i]["紅利發放天數"].ToString());
                            cell = row.CreateCell(38);
                            cell.CellStyle = stringRightStyle;
                            if (dt.Rows[i]["BONUS_AMT"].ToString() == "")
                                cell.SetCellValue(String.Format("{0:N0}", int.Parse("0")));
                            else
                                cell.SetCellValue(String.Format("{0:N0}", int.Parse(dt.Rows[i]["BONUS_AMT"].ToString())));
                            cell = row.CreateCell(39);
                            cell.CellStyle = stringRightStyle;
                            if (dt.Rows[i]["BONUS_TAX"].ToString() == "")
                                cell.SetCellValue(String.Format("{0:N0}", int.Parse("0")));
                            else
                                cell.SetCellValue(String.Format("{0:N0}", int.Parse(dt.Rows[i]["BONUS_TAX"].ToString())));
                            cell = row.CreateCell(40);
                            cell.CellStyle = stringRightStyle;
                            if (dt.Rows[i]["BONUS_AMT_R"].ToString() == "")
                                cell.SetCellValue(String.Format("{0:N0}", int.Parse("0")));
                            else
                                cell.SetCellValue(String.Format("{0:N0}", int.Parse(dt.Rows[i]["BONUS_AMT_R"].ToString())));
                            cell = row.CreateCell(41);
                            cell.SetCellValue(dt.Rows[i]["PAY_TYPE"].ToString());
                            cell = row.CreateCell(42);
                            cell.SetCellValue(dt.Rows[i]["CHG_STATUS_desc"].ToString());

                            row.GetCell(14).CellStyle = stringRightStyle;
                            for (int j = 17; j <= 40; j++)
                            {
                                if (j != 37)
                                    row.GetCell(j).CellStyle = stringRightStyle;
                            }
                        }
                        //製表日期
                        ICellStyle stringLeftStyleDate = this.setCellStyle(workbook, "left", false);
                        row = sheet.GetRow(0);
                        cell = row.CreateCell(43);
                        cell.CellStyle = stringLeftStyleDate;
                        cell.SetCellValue("製表日期:" + DateTime.Now.ToString("yyyy/MM/dd"));

                    }
                    return workbook;
                    //匯出Excel
                    //ExcelHandle.exportExcel(workbook, "本次核可資料." + type);
                }
                return null;
            }
            #endregion
            #region 前次核可資料比對.原始資料比對
            else if (data == "prev" || data == "original")
            {
                sheet = workbook.GetSheetAt(0);
                if (sheet != null)
                {
                    DataTable dt = fb2si.getExcelData(data, BONUS_YEAR);
                    DataTable dt2 = fb2si.getAddExcelData(data, BONUS_YEAR);
                    DataTable dt3 = fb2si.getDelExcelData(data, BONUS_YEAR);
                    int j = 3;
                    ICellStyle stringRightStyle = this.setCellStyle(workbook, "right", false);
                    if (dt.Rows.Count > 0)
                    {

                        string print = "N";
                        ICellStyle style;
                        style = (XSSFCellStyle)workbook.CreateCellStyle();

                        ((XSSFCellStyle)style).SetFillForegroundColor(new XSSFColor(Color.Red));
                        ((XSSFCellStyle)style).FillPattern = FillPattern.SolidForeground;
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            sheet.CreateRow(j);
                            sheet.CreateRow(j + 1);
                            for (int c = 0; c <= 43; c++)
                            {
                                sheet.GetRow(j).CreateCell(c);
                                sheet.GetRow(j + 1).CreateCell(c);
                            }
                            //修改
                            if (dt.Rows[i]["M_WORK_DAYS"].ToString() != dt.Rows[i]["S_WORK_DAYS"].ToString())
                            {
                                sheet.GetRow(j).GetCell(15).CellStyle = style;
                                sheet.GetRow(j + 1).GetCell(15).CellStyle = style;
                                print = "Y";
                            }
                            if (dt.Rows[i]["M_ABILITY_PAY"].ToString() != dt.Rows[i]["S_ABILITY_PAY"].ToString())
                            {
                                sheet.GetRow(j).GetCell(18).CellStyle = style;
                                sheet.GetRow(j + 1).GetCell(18).CellStyle = style;
                                print = "Y";
                            }
                            if (dt.Rows[i]["M_LEVEL_PAY"].ToString() != dt.Rows[i]["S_LEVEL_PAY"].ToString())
                            {
                                sheet.GetRow(j).GetCell(19).CellStyle = style;
                                sheet.GetRow(j + 1).GetCell(19).CellStyle = style;
                                print = "Y";
                            }
                            if (dt.Rows[i]["M_PJOB_PAY"].ToString() != dt.Rows[i]["S_PJOB_PAY"].ToString())
                            {
                                sheet.GetRow(j).GetCell(20).CellStyle = style;
                                sheet.GetRow(j + 1).GetCell(20).CellStyle = style;
                                print = "Y";
                            }
                            if (dt.Rows[i]["M_PROFESSION_PAY"].ToString() != dt.Rows[i]["S_PROFESSION_PAY"].ToString())
                            {
                                sheet.GetRow(j).GetCell(21).CellStyle = style;
                                sheet.GetRow(j + 1).GetCell(21).CellStyle = style;
                                print = "Y";
                            }
                            if (dt.Rows[i]["M_FOOD_SUBSIDY"].ToString() != dt.Rows[i]["S_FOOD_SUBSIDY"].ToString())
                            {
                                sheet.GetRow(j).GetCell(22).CellStyle = style;
                                sheet.GetRow(j + 1).GetCell(22).CellStyle = style;
                                print = "Y";
                            }
                            if (dt.Rows[i]["M_LEAVE_A_HOUR"].ToString() != dt.Rows[i]["S_LEAVE_A_HOUR"].ToString())
                            {
                                sheet.GetRow(j).GetCell(23).CellStyle = style;
                                sheet.GetRow(j + 1).GetCell(23).CellStyle = style;
                                print = "Y";
                            }
                            if (dt.Rows[i]["M_LEAVE_B_HOUR"].ToString() != dt.Rows[i]["S_LEAVE_B_HOUR"].ToString())
                            {
                                sheet.GetRow(j).GetCell(24).CellStyle = style;
                                sheet.GetRow(j + 1).GetCell(24).CellStyle = style;
                                print = "Y";
                            }
                            if (dt.Rows[i]["M_LEAVE_C_HOUR"].ToString() != dt.Rows[i]["S_LEAVE_C_HOUR"].ToString())
                            {
                                sheet.GetRow(j).GetCell(25).CellStyle = style;
                                sheet.GetRow(j + 1).GetCell(25).CellStyle = style;
                                print = "Y";
                            }
                            if (dt.Rows[i]["M_LEAVE_Q_HOUR"].ToString() != dt.Rows[i]["S_LEAVE_Q_HOUR"].ToString())
                            {
                                sheet.GetRow(j).GetCell(26).CellStyle = style;
                                sheet.GetRow(j + 1).GetCell(26).CellStyle = style;
                                print = "Y";
                            }
                            if (dt.Rows[i]["M_LEAVE_OP_HOUR"].ToString() != dt.Rows[i]["S_LEAVE_OP_HOUR"].ToString())
                            {
                                sheet.GetRow(j).GetCell(27).CellStyle = style;
                                sheet.GetRow(j + 1).GetCell(27).CellStyle = style;
                                print = "Y";
                            }
                            if (dt.Rows[i]["M_THIRD_CNT_P"].ToString() != dt.Rows[i]["S_THIRD_CNT_P"].ToString())
                            {
                                sheet.GetRow(j).GetCell(28).CellStyle = style;
                                sheet.GetRow(j + 1).GetCell(28).CellStyle = style;
                                print = "Y";
                            }
                            if (dt.Rows[i]["M_SECOND_CNT_P"].ToString() != dt.Rows[i]["S_SECOND_CNT_P"].ToString())
                            {
                                sheet.GetRow(j).GetCell(29).CellStyle = style;
                                sheet.GetRow(j + 1).GetCell(29).CellStyle = style;
                                print = "Y";
                            }
                            if (dt.Rows[i]["M_FIRST_CNT_P"].ToString() != dt.Rows[i]["S_FIRST_CNT_P"].ToString())
                            {
                                sheet.GetRow(j).GetCell(30).CellStyle = style;
                                sheet.GetRow(j + 1).GetCell(30).CellStyle = style;
                                print = "Y";
                            }
                            if (dt.Rows[i]["M_THIRD_CNT_M"].ToString() != dt.Rows[i]["S_THIRD_CNT_M"].ToString())
                            {
                                sheet.GetRow(j).GetCell(31).CellStyle = style;
                                sheet.GetRow(j + 1).GetCell(31).CellStyle = style;
                                print = "Y";
                            }
                            if (dt.Rows[i]["M_SECOND_CNT_M"].ToString() != dt.Rows[i]["S_SECOND_CNT_M"].ToString())
                            {
                                sheet.GetRow(j).GetCell(32).CellStyle = style;
                                sheet.GetRow(j + 1).GetCell(32).CellStyle = style;
                                print = "Y";
                            }
                            if (dt.Rows[i]["M_FIRST_CNT_M"].ToString() != dt.Rows[i]["S_FIRST_CNT_M"].ToString())
                            {
                                sheet.GetRow(j).GetCell(33).CellStyle = style;
                                sheet.GetRow(j + 1).GetCell(33).CellStyle = style;
                                print = "Y";
                            }
                            if (dt.Rows[i]["M_ATTEND_DAYS"].ToString() != dt.Rows[i]["S_ATTEND_DAYS"].ToString())
                            {
                                sheet.GetRow(j).GetCell(34).CellStyle = style;
                                sheet.GetRow(j + 1).GetCell(34).CellStyle = style;
                                print = "Y";
                            }
                            if (dt.Rows[i]["M_REWARD_DAYS"].ToString() != dt.Rows[i]["S_REWARD_DAYS"].ToString())
                            {
                                sheet.GetRow(j).GetCell(35).CellStyle = style;
                                sheet.GetRow(j + 1).GetCell(35).CellStyle = style;
                                print = "Y";
                            }
                            if (dt.Rows[i]["M_DISCIPLINE_DAYS"].ToString() != dt.Rows[i]["S_DISCIPLINE_DAYS"].ToString())
                            {
                                sheet.GetRow(j).GetCell(36).CellStyle = style;
                                sheet.GetRow(j + 1).GetCell(36).CellStyle = style;
                                print = "Y";
                            }
                            if (dt.Rows[i]["M_BONUS_WORK_DAYS"].ToString() != dt.Rows[i]["S_BONUS_WORK_DAYS"].ToString())
                            {
                                sheet.GetRow(j).GetCell(37).CellStyle = style;
                                sheet.GetRow(j + 1).GetCell(37).CellStyle = style;
                                print = "Y";
                            }
                            if (dt.Rows[i]["M_BONUS_AMT"].ToString() != dt.Rows[i]["S_BONUS_AMT"].ToString())
                            {
                                sheet.GetRow(j).GetCell(39).CellStyle = style;
                                sheet.GetRow(j + 1).GetCell(39).CellStyle = style;
                                print = "Y";
                            }
                            if (dt.Rows[i]["M_BONUS_TAX"].ToString() != dt.Rows[i]["S_BONUS_TAX"].ToString())
                            {
                                sheet.GetRow(j).GetCell(40).CellStyle = style;
                                sheet.GetRow(j + 1).GetCell(40).CellStyle = style;
                                print = "Y";
                            }
                            if (dt.Rows[i]["M_BONUS_AMT_R"].ToString() != dt.Rows[i]["S_BONUS_AMT_R"].ToString())
                            {
                                sheet.GetRow(j).GetCell(41).CellStyle = style;
                                sheet.GetRow(j + 1).GetCell(41).CellStyle = style;

                                print = "Y";
                            }
                            if (dt.Rows[i]["M_PAY_TYPE"].ToString() != dt.Rows[i]["S_PAY_TYPE"].ToString())
                            {
                                sheet.GetRow(j).GetCell(42).CellStyle = style;
                                sheet.GetRow(j + 1).GetCell(42).CellStyle = style;
                                print = "Y";
                            }
                            if (dt.Rows[i]["M_CHG_STATUS"].ToString() != dt.Rows[i]["S_CHG_STATUS"].ToString())
                            {
                                sheet.GetRow(j).GetCell(43).CellStyle = style;
                                sheet.GetRow(j + 1).GetCell(43).CellStyle = style;
                                print = "Y";
                            }

                            if (print == "Y")
                            {
                                sheet.GetRow(j).GetCell(0).SetCellValue("修改");
                                sheet.GetRow(j).GetCell(1).SetCellValue("N");
                                sheet.GetRow(j).GetCell(2).SetCellValue(dt.Rows[i]["M_EMP_ID"].ToString());
                                sheet.GetRow(j).GetCell(3).SetCellValue(dt.Rows[i]["M_EMP_NAME"].ToString());
                                sheet.GetRow(j).GetCell(4).SetCellValue(dt.Rows[i]["M_EMP_CHG_CD"].ToString());
                                sheet.GetRow(j).GetCell(5).SetCellValue(dt.Rows[i]["M_WS_CD"].ToString());
                                sheet.GetRow(j).GetCell(6).SetCellValue(dt.Rows[i]["M_JPN_CD"].ToString());
                                sheet.GetRow(j).GetCell(7).SetCellValue(dt.Rows[i]["M_DEPT_NO"].ToString());
                                sheet.GetRow(j).GetCell(8).SetCellValue(dt.Rows[i]["M_LEVEL_CD"].ToString());
                                sheet.GetRow(j).GetCell(9).SetCellValue(dt.Rows[i]["M_PJOB_CD"].ToString());
                                if (dt.Rows[i]["M_JOIN_DT"] != DBNull.Value)
                                    sheet.GetRow(j).GetCell(10).SetCellValue(Convert.ToDateTime(dt.Rows[i]["M_JOIN_DT"]).ToString("yyyy/MM/dd"));
                                if (dt.Rows[i]["M_LEAVE_DT"] != DBNull.Value)
                                    sheet.GetRow(j).GetCell(11).SetCellValue(Convert.ToDateTime(dt.Rows[i]["M_LEAVE_DT"]).ToString("yyyy/MM/dd"));
                                if (dt.Rows[i]["M_STAY_DT"] != DBNull.Value)
                                    sheet.GetRow(j).GetCell(12).SetCellValue(Convert.ToDateTime(dt.Rows[i]["M_STAY_DT"]).ToString("yyyy/MM/dd"));
                                if (dt.Rows[i]["M_BE_CONTRACT_DT"] != DBNull.Value)
                                    sheet.GetRow(j).GetCell(13).SetCellValue(Convert.ToDateTime(dt.Rows[i]["M_BE_CONTRACT_DT"]).ToString("yyyy/MM/dd"));
                                if (dt.Rows[i]["M_BE_EMP_DT"] != DBNull.Value)
                                    sheet.GetRow(j).GetCell(14).SetCellValue(Convert.ToDateTime(dt.Rows[i]["M_BE_EMP_DT"]).ToString("yyyy/MM/dd"));
                                sheet.GetRow(j).GetCell(15).SetCellValue(dt.Rows[i]["M_WORK_DAYS"].ToString());
                                sheet.GetRow(j).GetCell(16).SetCellValue(dt.Rows[i]["M_EMP_CD"].ToString());
                                sheet.GetRow(j).GetCell(17).SetCellValue(dt.Rows[i]["M_ID_DESC"].ToString());
                                if (dt.Rows[i]["M_ABILITY_PAY"].ToString() == "")
                                    sheet.GetRow(j).CreateCell(18).SetCellValue(String.Format("{0:N0}", int.Parse("0")));
                                else
                                    sheet.GetRow(j).CreateCell(18).SetCellValue(String.Format("{0:N0}", int.Parse(dt.Rows[i]["M_ABILITY_PAY"].ToString())));
                                if (dt.Rows[i]["M_LEVEL_PAY"].ToString() == "")
                                    sheet.GetRow(j).CreateCell(19).SetCellValue(String.Format("{0:N0}", int.Parse("0")));
                                else
                                    sheet.GetRow(j).CreateCell(19).SetCellValue(String.Format("{0:N0}", int.Parse(dt.Rows[i]["M_LEVEL_PAY"].ToString())));
                                if (dt.Rows[i]["M_PJOB_PAY"].ToString() == "")
                                    sheet.GetRow(j).CreateCell(20).SetCellValue(String.Format("{0:N0}", int.Parse("0")));
                                else
                                    sheet.GetRow(j).CreateCell(20).SetCellValue(String.Format("{0:N0}", int.Parse(dt.Rows[i]["M_PJOB_PAY"].ToString())));
                                if (dt.Rows[i]["M_PROFESSION_PAY"].ToString() == "")
                                    sheet.GetRow(j).CreateCell(21).SetCellValue(String.Format("{0:N0}", int.Parse("0")));
                                else
                                    sheet.GetRow(j).CreateCell(21).SetCellValue(String.Format("{0:N0}", int.Parse(dt.Rows[i]["M_PROFESSION_PAY"].ToString())));
                                if (dt.Rows[i]["M_FOOD_SUBSIDY"].ToString() == "")
                                    sheet.GetRow(j).CreateCell(22).SetCellValue(String.Format("{0:N0}", int.Parse("0")));
                                else
                                    sheet.GetRow(j).CreateCell(22).SetCellValue(String.Format("{0:N0}", int.Parse(dt.Rows[i]["M_FOOD_SUBSIDY"].ToString())));
                                sheet.GetRow(j).GetCell(23).SetCellValue(dt.Rows[i]["M_LEAVE_A_HOUR"].ToString());
                                sheet.GetRow(j).GetCell(24).SetCellValue(dt.Rows[i]["M_LEAVE_B_HOUR"].ToString());
                                sheet.GetRow(j).GetCell(25).SetCellValue(dt.Rows[i]["M_LEAVE_C_HOUR"].ToString());
                                sheet.GetRow(j).GetCell(26).SetCellValue(dt.Rows[i]["M_LEAVE_Q_HOUR"].ToString());
                                sheet.GetRow(j).GetCell(27).SetCellValue(dt.Rows[i]["M_LEAVE_OP_HOUR"].ToString());
                                sheet.GetRow(j).GetCell(28).SetCellValue(dt.Rows[i]["M_THIRD_CNT_P"].ToString());
                                sheet.GetRow(j).GetCell(29).SetCellValue(dt.Rows[i]["M_SECOND_CNT_P"].ToString());
                                sheet.GetRow(j).GetCell(30).SetCellValue(dt.Rows[i]["M_FIRST_CNT_P"].ToString());
                                sheet.GetRow(j).GetCell(31).SetCellValue(dt.Rows[i]["M_THIRD_CNT_M"].ToString());
                                sheet.GetRow(j).GetCell(32).SetCellValue(dt.Rows[i]["M_SECOND_CNT_M"].ToString());
                                sheet.GetRow(j).GetCell(33).SetCellValue(dt.Rows[i]["M_FIRST_CNT_M"].ToString());
                                sheet.GetRow(j).GetCell(34).SetCellValue(dt.Rows[i]["M_ATTEND_DAYS"].ToString());
                                sheet.GetRow(j).GetCell(35).SetCellValue(dt.Rows[i]["M_REWARD_DAYS"].ToString());
                                sheet.GetRow(j).GetCell(36).SetCellValue(dt.Rows[i]["M_DISCIPLINE_DAYS"].ToString());
                                sheet.GetRow(j).GetCell(37).SetCellValue(dt.Rows[i]["M_BONUS_WORK_DAYS"].ToString());
                                //sheet.GetRow(j).GetCell(38).SetCellValue(dt.Rows[i]["紅利發放天數"].ToString());
                                if (dt.Rows[i]["M_BONUS_AMT"].ToString() == "")
                                    sheet.GetRow(j).CreateCell(39).SetCellValue(String.Format("{0:N0}", int.Parse("0")));
                                else
                                    sheet.GetRow(j).CreateCell(39).SetCellValue(String.Format("{0:N0}", int.Parse(dt.Rows[i]["M_BONUS_AMT"].ToString())));
                                if (dt.Rows[i]["M_BONUS_TAX"].ToString() == "")
                                    sheet.GetRow(j).CreateCell(40).SetCellValue(String.Format("{0:N0}", int.Parse("0")));
                                else
                                    sheet.GetRow(j).CreateCell(40).SetCellValue(String.Format("{0:N0}", int.Parse(dt.Rows[i]["M_BONUS_TAX"].ToString())));
                                if (dt.Rows[i]["M_BONUS_AMT_R"].ToString() == "")
                                    sheet.GetRow(j).CreateCell(41).SetCellValue(String.Format("{0:N0}", int.Parse("0")));
                                else
                                    sheet.GetRow(j).CreateCell(41).SetCellValue(String.Format("{0:N0}", int.Parse(dt.Rows[i]["M_BONUS_AMT_R"].ToString())));
                                sheet.GetRow(j).GetCell(42).SetCellValue(dt.Rows[i]["M_PAY_TYPE"].ToString());
                                sheet.GetRow(j).GetCell(43).SetCellValue(dt.Rows[i]["M_CHG_STATUS"].ToString());

                                sheet.GetRow(j).GetCell(15).CellStyle = stringRightStyle;
                                for (int m = 18; m <= 41; m++)
                                {
                                    if (m != 38)
                                        sheet.GetRow(j).GetCell(m).CellStyle = stringRightStyle;
                                }
                                j++;
                                //sheet.CreateRow(j);
                                //sheet.CreateRow(j + 1);
                                sheet.GetRow(j).GetCell(1).SetCellValue("O");
                                sheet.GetRow(j).GetCell(2).SetCellValue(dt.Rows[i]["S_EMP_ID"].ToString());
                                sheet.GetRow(j).GetCell(3).SetCellValue(dt.Rows[i]["S_EMP_NAME"].ToString());
                                sheet.GetRow(j).GetCell(4).SetCellValue(dt.Rows[i]["S_EMP_CHG_CD"].ToString());
                                sheet.GetRow(j).GetCell(5).SetCellValue(dt.Rows[i]["S_WS_CD"].ToString());
                                sheet.GetRow(j).GetCell(6).SetCellValue(dt.Rows[i]["S_JPN_CD"].ToString());
                                sheet.GetRow(j).GetCell(7).SetCellValue(dt.Rows[i]["S_DEPT_NO"].ToString());
                                sheet.GetRow(j).GetCell(8).SetCellValue(dt.Rows[i]["S_LEVEL_CD"].ToString());
                                sheet.GetRow(j).GetCell(9).SetCellValue(dt.Rows[i]["S_PJOB_CD"].ToString());
                                if (dt.Rows[i]["S_JOIN_DT"] != DBNull.Value)
                                    sheet.GetRow(j).GetCell(10).SetCellValue(Convert.ToDateTime(dt.Rows[i]["S_JOIN_DT"]).ToString("yyyy/MM/dd"));
                                if (dt.Rows[i]["S_LEAVE_DT"] != DBNull.Value)
                                    sheet.GetRow(j).GetCell(11).SetCellValue(Convert.ToDateTime(dt.Rows[i]["S_LEAVE_DT"]).ToString("yyyy/MM/dd"));
                                if (dt.Rows[i]["S_STAY_DT"] != DBNull.Value)
                                    sheet.GetRow(j).GetCell(12).SetCellValue(Convert.ToDateTime(dt.Rows[i]["S_STAY_DT"]).ToString("yyyy/MM/dd"));
                                if (dt.Rows[i]["S_BE_CONTRACT_DT"] != DBNull.Value)
                                    sheet.GetRow(j).GetCell(13).SetCellValue(Convert.ToDateTime(dt.Rows[i]["S_BE_CONTRACT_DT"]).ToString("yyyy/MM/dd"));
                                if (dt.Rows[i]["S_BE_EMP_DT"] != DBNull.Value)
                                    sheet.GetRow(j).GetCell(14).SetCellValue(Convert.ToDateTime(dt.Rows[i]["S_BE_EMP_DT"]).ToString("yyyy/MM/dd"));
                                sheet.GetRow(j).GetCell(15).SetCellValue(dt.Rows[i]["S_WORK_DAYS"].ToString());
                                sheet.GetRow(j).GetCell(16).SetCellValue(dt.Rows[i]["S_EMP_CD"].ToString());
                                sheet.GetRow(j).GetCell(17).SetCellValue(dt.Rows[i]["S_ID_DESC"].ToString());
                                if (dt.Rows[i]["S_ABILITY_PAY"].ToString() == "")
                                    sheet.GetRow(j).CreateCell(18).SetCellValue(String.Format("{0:N0}", int.Parse("0")));
                                else
                                    sheet.GetRow(j).CreateCell(18).SetCellValue(String.Format("{0:N0}", int.Parse(dt.Rows[i]["S_ABILITY_PAY"].ToString())));
                                if (dt.Rows[i]["S_LEVEL_PAY"].ToString() == "")
                                    sheet.GetRow(j).CreateCell(19).SetCellValue(String.Format("{0:N0}", int.Parse("0")));
                                else
                                    sheet.GetRow(j).CreateCell(19).SetCellValue(String.Format("{0:N0}", int.Parse(dt.Rows[i]["S_LEVEL_PAY"].ToString())));
                                if (dt.Rows[i]["S_PJOB_PAY"].ToString() == "")
                                    sheet.GetRow(j).CreateCell(20).SetCellValue(String.Format("{0:N0}", int.Parse("0")));
                                else
                                    sheet.GetRow(j).CreateCell(20).SetCellValue(String.Format("{0:N0}", int.Parse(dt.Rows[i]["S_PJOB_PAY"].ToString())));
                                if (dt.Rows[i]["S_PROFESSION_PAY"].ToString() == "")
                                    sheet.GetRow(j).CreateCell(21).SetCellValue(String.Format("{0:N0}", int.Parse("0")));
                                else
                                    sheet.GetRow(j).CreateCell(21).SetCellValue(String.Format("{0:N0}", int.Parse(dt.Rows[i]["S_PROFESSION_PAY"].ToString())));
                                if (dt.Rows[i]["S_FOOD_SUBSIDY"].ToString() == "")
                                    sheet.GetRow(j).CreateCell(22).SetCellValue(String.Format("{0:N0}", int.Parse("0")));
                                else
                                    sheet.GetRow(j).CreateCell(22).SetCellValue(String.Format("{0:N0}", int.Parse(dt.Rows[i]["S_FOOD_SUBSIDY"].ToString())));
                                sheet.GetRow(j).GetCell(23).SetCellValue(dt.Rows[i]["S_LEAVE_A_HOUR"].ToString());
                                sheet.GetRow(j).GetCell(24).SetCellValue(dt.Rows[i]["S_LEAVE_B_HOUR"].ToString());
                                sheet.GetRow(j).GetCell(25).SetCellValue(dt.Rows[i]["S_LEAVE_C_HOUR"].ToString());
                                sheet.GetRow(j).GetCell(26).SetCellValue(dt.Rows[i]["S_LEAVE_Q_HOUR"].ToString());
                                sheet.GetRow(j).GetCell(27).SetCellValue(dt.Rows[i]["S_LEAVE_OP_HOUR"].ToString());
                                sheet.GetRow(j).GetCell(28).SetCellValue(dt.Rows[i]["S_THIRD_CNT_P"].ToString());
                                sheet.GetRow(j).GetCell(29).SetCellValue(dt.Rows[i]["S_SECOND_CNT_P"].ToString());
                                sheet.GetRow(j).GetCell(30).SetCellValue(dt.Rows[i]["S_FIRST_CNT_P"].ToString());
                                sheet.GetRow(j).GetCell(31).SetCellValue(dt.Rows[i]["S_THIRD_CNT_M"].ToString());
                                sheet.GetRow(j).GetCell(32).SetCellValue(dt.Rows[i]["S_SECOND_CNT_M"].ToString());
                                sheet.GetRow(j).GetCell(33).SetCellValue(dt.Rows[i]["S_FIRST_CNT_M"].ToString());
                                sheet.GetRow(j).GetCell(34).SetCellValue(dt.Rows[i]["S_ATTEND_DAYS"].ToString());
                                sheet.GetRow(j).GetCell(35).SetCellValue(dt.Rows[i]["S_REWARD_DAYS"].ToString());
                                sheet.GetRow(j).GetCell(36).SetCellValue(dt.Rows[i]["S_DISCIPLINE_DAYS"].ToString());
                                sheet.GetRow(j).GetCell(37).SetCellValue(dt.Rows[i]["S_BONUS_WORK_DAYS"].ToString());
                                //sheet.GetRow(j).GetCell(38).SetCellValue(dt.Rows[i]["紅利發放天數"].ToString());
                                if (dt.Rows[i]["S_BONUS_AMT"].ToString() == "")
                                    sheet.GetRow(j).CreateCell(39).SetCellValue(String.Format("{0:N0}", int.Parse("0")));
                                else
                                    sheet.GetRow(j).CreateCell(39).SetCellValue(String.Format("{0:N0}", int.Parse(dt.Rows[i]["S_BONUS_AMT"].ToString())));
                                if (dt.Rows[i]["S_BONUS_TAX"].ToString() == "")
                                    sheet.GetRow(j).CreateCell(40).SetCellValue(String.Format("{0:N0}", int.Parse("0")));
                                else
                                    sheet.GetRow(j).CreateCell(40).SetCellValue(String.Format("{0:N0}", int.Parse(dt.Rows[i]["S_BONUS_TAX"].ToString())));
                                if (dt.Rows[i]["S_BONUS_AMT_R"].ToString() == "")
                                    sheet.GetRow(j).CreateCell(41).SetCellValue(String.Format("{0:N0}", int.Parse("0")));
                                else
                                    sheet.GetRow(j).CreateCell(41).SetCellValue(String.Format("{0:N0}", int.Parse(dt.Rows[i]["S_BONUS_AMT_R"].ToString())));
                                sheet.GetRow(j).GetCell(42).SetCellValue(dt.Rows[i]["S_PAY_TYPE"].ToString());
                                sheet.GetRow(j).GetCell(43).SetCellValue(dt.Rows[i]["S_CHG_STATUS"].ToString());

                                sheet.GetRow(j).GetCell(15).CellStyle = stringRightStyle;
                                for (int m = 18; m <= 41; m++)
                                {
                                    if (m != 38)
                                        sheet.GetRow(j).GetCell(m).CellStyle = stringRightStyle;
                                }
                                j++;
                                sheet.CreateRow(j);
                                //sheet.CreateRow(j + 1);
                                print = "N";
                            }
                        }
                        //製表日期
                        ICellStyle stringLeftStyleDate = this.setCellStyle(workbook, "left", false);
                        IRow row = sheet.GetRow(0);
                        ICell cell = row.CreateCell(44);
                        cell.CellStyle = stringLeftStyleDate;
                        cell.SetCellValue("製表日期:" + DateTime.Now.ToString("yyyy/MM/dd"));
                        #region 新增
                        if (dt2.Rows.Count > 0)
                        {
                            for (int k = 0; k < dt2.Rows.Count; k++)
                            {
                                sheet.GetRow(j).CreateCell(0).SetCellValue("新增");
                                sheet.GetRow(j).CreateCell(1).SetCellValue("N");
                                sheet.GetRow(j).CreateCell(2).SetCellValue(dt2.Rows[k]["EMP_ID"].ToString());
                                sheet.GetRow(j).CreateCell(3).SetCellValue(dt2.Rows[k]["EMP_NAME"].ToString());
                                sheet.GetRow(j).CreateCell(4).SetCellValue(dt2.Rows[k]["EMP_CHG_CD_desc"].ToString());
                                sheet.GetRow(j).CreateCell(5).SetCellValue(dt2.Rows[k]["WS_CD"].ToString());
                                sheet.GetRow(j).CreateCell(6).SetCellValue(dt2.Rows[k]["JPN_CD"].ToString());
                                sheet.GetRow(j).CreateCell(7).SetCellValue(dt2.Rows[k]["DEPT_NO"].ToString());
                                sheet.GetRow(j).CreateCell(8).SetCellValue(dt2.Rows[k]["LEVEL_CD"].ToString());
                                sheet.GetRow(j).CreateCell(9).SetCellValue(dt2.Rows[k]["PJOB_CD"].ToString());
                                if (dt2.Rows[k]["JOIN_DT"] != DBNull.Value)
                                    sheet.GetRow(j).CreateCell(10).SetCellValue(Convert.ToDateTime(dt2.Rows[k]["JOIN_DT"]).ToString("yyyy/MM/dd"));
                                if (dt2.Rows[k]["LEAVE_DT"] != DBNull.Value)
                                    sheet.GetRow(j).CreateCell(11).SetCellValue(Convert.ToDateTime(dt2.Rows[k]["LEAVE_DT"]).ToString("yyyy/MM/dd"));
                                if (dt2.Rows[k]["STAY_DT"] != DBNull.Value)
                                    sheet.GetRow(j).CreateCell(12).SetCellValue(Convert.ToDateTime(dt2.Rows[k]["STAY_DT"]).ToString("yyyy/MM/dd"));
                                if (dt2.Rows[k]["BE_CONTRACT_DT"] != DBNull.Value)
                                    sheet.GetRow(j).CreateCell(13).SetCellValue(Convert.ToDateTime(dt2.Rows[k]["BE_CONTRACT_DT"]).ToString("yyyy/MM/dd"));
                                if (dt2.Rows[k]["BE_EMP_DT"] != DBNull.Value)
                                    sheet.GetRow(j).CreateCell(14).SetCellValue(Convert.ToDateTime(dt2.Rows[k]["BE_EMP_DT"]).ToString("yyyy/MM/dd"));
                                sheet.GetRow(j).CreateCell(15).SetCellValue(dt2.Rows[k]["WORK_DAYS"].ToString());
                                sheet.GetRow(j).CreateCell(16).SetCellValue(dt2.Rows[k]["EMP_CD_desc"].ToString());
                                sheet.GetRow(j).CreateCell(17).SetCellValue(dt2.Rows[k]["ID_DESC"].ToString());
                                if (dt2.Rows[k]["ABILITY_PAY"].ToString() == "")
                                    sheet.GetRow(j).CreateCell(18).SetCellValue(String.Format("{0:N0}", int.Parse("0")));
                                else
                                    sheet.GetRow(j).CreateCell(18).SetCellValue(String.Format("{0:N0}", int.Parse(dt2.Rows[k]["ABILITY_PAY"].ToString())));
                                if (dt2.Rows[k]["LEVEL_PAY"].ToString() == "")
                                    sheet.GetRow(j).CreateCell(19).SetCellValue(String.Format("{0:N0}", int.Parse("0")));
                                else
                                    sheet.GetRow(j).CreateCell(19).SetCellValue(String.Format("{0:N0}", int.Parse(dt2.Rows[k]["LEVEL_PAY"].ToString())));
                                if (dt2.Rows[k]["PJOB_PAY"].ToString() == "")
                                    sheet.GetRow(j).CreateCell(20).SetCellValue(String.Format("{0:N0}", int.Parse("0")));
                                else
                                    sheet.GetRow(j).CreateCell(20).SetCellValue(String.Format("{0:N0}", int.Parse(dt2.Rows[k]["PJOB_PAY"].ToString())));
                                if (dt2.Rows[k]["PROFESSION_PAY"].ToString() == "")
                                    sheet.GetRow(j).CreateCell(21).SetCellValue(String.Format("{0:N0}", int.Parse("0")));
                                else
                                    sheet.GetRow(j).CreateCell(21).SetCellValue(String.Format("{0:N0}", int.Parse(dt2.Rows[k]["PROFESSION_PAY"].ToString())));
                                if (dt2.Rows[k]["FOOD_SUBSIDY"].ToString() == "")
                                    sheet.GetRow(j).CreateCell(22).SetCellValue(String.Format("{0:N0}", int.Parse("0")));
                                else
                                    sheet.GetRow(j).CreateCell(22).SetCellValue(String.Format("{0:N0}", int.Parse(dt2.Rows[k]["FOOD_SUBSIDY"].ToString())));
                                sheet.GetRow(j).CreateCell(23).SetCellValue(dt2.Rows[k]["LEAVE_A_HOUR"].ToString());
                                sheet.GetRow(j).CreateCell(24).SetCellValue(dt2.Rows[k]["LEAVE_B_HOUR"].ToString());
                                sheet.GetRow(j).CreateCell(25).SetCellValue(dt2.Rows[k]["LEAVE_C_HOUR"].ToString());
                                sheet.GetRow(j).CreateCell(26).SetCellValue(dt2.Rows[k]["LEAVE_Q_HOUR"].ToString());
                                sheet.GetRow(j).CreateCell(27).SetCellValue(dt2.Rows[k]["LEAVE_OP_HOUR"].ToString());
                                sheet.GetRow(j).CreateCell(28).SetCellValue(dt2.Rows[k]["THIRD_CNT_P"].ToString());
                                sheet.GetRow(j).CreateCell(29).SetCellValue(dt2.Rows[k]["SECOND_CNT_P"].ToString());
                                sheet.GetRow(j).CreateCell(30).SetCellValue(dt2.Rows[k]["FIRST_CNT_P"].ToString());
                                sheet.GetRow(j).CreateCell(31).SetCellValue(dt2.Rows[k]["THIRD_CNT_M"].ToString());
                                sheet.GetRow(j).CreateCell(32).SetCellValue(dt2.Rows[k]["SECOND_CNT_M"].ToString());
                                sheet.GetRow(j).CreateCell(33).SetCellValue(dt2.Rows[k]["FIRST_CNT_M"].ToString());
                                sheet.GetRow(j).CreateCell(34).SetCellValue(dt2.Rows[k]["ATTEND_DAYS"].ToString());
                                sheet.GetRow(j).CreateCell(35).SetCellValue(dt2.Rows[k]["REWARD_DAYS"].ToString());
                                sheet.GetRow(j).CreateCell(36).SetCellValue(dt2.Rows[k]["DISCIPLINE_DAYS"].ToString());
                                sheet.GetRow(j).CreateCell(37).SetCellValue(dt2.Rows[k]["BONUS_WORK_DAYS"].ToString());
                                //sheet.GetRow(j).CreateCell(38).SetCellValue(dt2.Rows[k]["紅利發放天數"].ToString());
                                if (dt2.Rows[k]["BONUS_AMT"].ToString() == "")
                                    sheet.GetRow(j).CreateCell(39).SetCellValue(String.Format("{0:N0}", int.Parse("0")));
                                else
                                    sheet.GetRow(j).CreateCell(39).SetCellValue(String.Format("{0:N0}", int.Parse(dt2.Rows[k]["BONUS_AMT"].ToString())));
                                if (dt2.Rows[k]["BONUS_TAX"].ToString() == "")
                                    sheet.GetRow(j).CreateCell(40).SetCellValue(String.Format("{0:N0}", int.Parse("0")));
                                else
                                    sheet.GetRow(j).CreateCell(40).SetCellValue(String.Format("{0:N0}", int.Parse(dt2.Rows[k]["BONUS_TAX"].ToString())));
                                if (dt2.Rows[k]["BONUS_AMT_R"].ToString() == "")
                                    sheet.GetRow(j).CreateCell(41).SetCellValue(String.Format("{0:N0}", int.Parse("0")));
                                else
                                    sheet.GetRow(j).CreateCell(41).SetCellValue(String.Format("{0:N0}", int.Parse(dt2.Rows[k]["BONUS_AMT_R"].ToString())));
                                sheet.GetRow(j).CreateCell(42).SetCellValue(dt2.Rows[k]["PAY_TYPE"].ToString());
                                sheet.GetRow(j).CreateCell(43).SetCellValue(dt2.Rows[k]["CHG_STATUS_desc"].ToString());

                                sheet.GetRow(j).GetCell(15).CellStyle = stringRightStyle;
                                for (int m = 18; m <= 41; m++)
                                {
                                    if (m != 38)
                                        sheet.GetRow(j).GetCell(m).CellStyle = stringRightStyle;
                                }
                                j++;
                                sheet.CreateRow(j);
                            }
                        }
                        #endregion

                        #region 刪除
                        if (dt3.Rows.Count > 0)
                        {
                            for (int l = 0; l < dt3.Rows.Count; l++)
                            {
                                sheet.GetRow(j).CreateCell(0).SetCellValue("刪除");
                                sheet.GetRow(j).CreateCell(1).SetCellValue("O");
                                sheet.GetRow(j).CreateCell(2).SetCellValue(dt3.Rows[l]["EMP_ID"].ToString());
                                sheet.GetRow(j).CreateCell(3).SetCellValue(dt3.Rows[l]["EMP_NAME"].ToString());
                                sheet.GetRow(j).CreateCell(4).SetCellValue(dt3.Rows[l]["EMP_CHG_CD_desc"].ToString());
                                sheet.GetRow(j).CreateCell(5).SetCellValue(dt3.Rows[l]["WS_CD"].ToString());
                                sheet.GetRow(j).CreateCell(6).SetCellValue(dt3.Rows[l]["JPN_CD"].ToString());
                                sheet.GetRow(j).CreateCell(7).SetCellValue(dt3.Rows[l]["DEPT_NO"].ToString());
                                sheet.GetRow(j).CreateCell(8).SetCellValue(dt3.Rows[l]["LEVEL_CD"].ToString());
                                sheet.GetRow(j).CreateCell(9).SetCellValue(dt3.Rows[l]["PJOB_CD"].ToString());
                                if (dt3.Rows[l]["JOIN_DT"] != DBNull.Value)
                                    sheet.GetRow(j).CreateCell(10).SetCellValue(Convert.ToDateTime(dt3.Rows[l]["JOIN_DT"]).ToString("yyyy/MM/dd"));
                                if (dt3.Rows[l]["LEAVE_DT"] != DBNull.Value)
                                    sheet.GetRow(j).CreateCell(11).SetCellValue(Convert.ToDateTime(dt3.Rows[l]["LEAVE_DT"]).ToString("yyyy/MM/dd"));
                                if (dt3.Rows[l]["STAY_DT"] != DBNull.Value)
                                    sheet.GetRow(j).CreateCell(12).SetCellValue(Convert.ToDateTime(dt3.Rows[l]["STAY_DT"]).ToString("yyyy/MM/dd"));
                                if (dt3.Rows[l]["BE_CONTRACT_DT"] != DBNull.Value)
                                    sheet.GetRow(j).CreateCell(13).SetCellValue(Convert.ToDateTime(dt3.Rows[l]["BE_CONTRACT_DT"]).ToString("yyyy/MM/dd"));
                                if (dt3.Rows[l]["BE_EMP_DT"] != DBNull.Value)
                                    sheet.GetRow(j).CreateCell(14).SetCellValue(Convert.ToDateTime(dt3.Rows[l]["BE_EMP_DT"]).ToString("yyyy/MM/dd"));
                                sheet.GetRow(j).CreateCell(15).SetCellValue(dt3.Rows[l]["WORK_DAYS"].ToString());
                                sheet.GetRow(j).CreateCell(16).SetCellValue(dt3.Rows[l]["EMP_CD_desc"].ToString());
                                sheet.GetRow(j).CreateCell(17).SetCellValue(dt3.Rows[l]["ID_DESC"].ToString());
                                if (dt3.Rows[l]["ABILITY_PAY"].ToString() == "")
                                    sheet.GetRow(j).CreateCell(18).SetCellValue(String.Format("{0:N0}", int.Parse("0")));
                                else
                                    sheet.GetRow(j).CreateCell(18).SetCellValue(String.Format("{0:N0}", int.Parse(dt3.Rows[l]["ABILITY_PAY"].ToString())));
                                if (dt3.Rows[l]["LEVEL_PAY"].ToString() == "")
                                    sheet.GetRow(j).CreateCell(19).SetCellValue(String.Format("{0:N0}", int.Parse("0")));
                                else
                                    sheet.GetRow(j).CreateCell(19).SetCellValue(String.Format("{0:N0}", int.Parse(dt3.Rows[l]["LEVEL_PAY"].ToString())));
                                if (dt3.Rows[l]["PJOB_PAY"].ToString() == "")
                                    sheet.GetRow(j).CreateCell(20).SetCellValue(String.Format("{0:N0}", int.Parse("0")));
                                else
                                    sheet.GetRow(j).CreateCell(20).SetCellValue(String.Format("{0:N0}", int.Parse(dt3.Rows[l]["PJOB_PAY"].ToString())));
                                if (dt3.Rows[l]["PROFESSION_PAY"].ToString() == "")
                                    sheet.GetRow(j).CreateCell(21).SetCellValue(String.Format("{0:N0}", int.Parse("0")));
                                else
                                    sheet.GetRow(j).CreateCell(21).SetCellValue(String.Format("{0:N0}", int.Parse(dt3.Rows[l]["PROFESSION_PAY"].ToString())));
                                if (dt3.Rows[l]["FOOD_SUBSIDY"].ToString() == "")
                                    sheet.GetRow(j).CreateCell(22).SetCellValue(String.Format("{0:N0}", int.Parse("0")));
                                else
                                    sheet.GetRow(j).CreateCell(22).SetCellValue(String.Format("{0:N0}", int.Parse(dt3.Rows[l]["FOOD_SUBSIDY"].ToString())));
                                sheet.GetRow(j).CreateCell(23).SetCellValue(dt3.Rows[l]["LEAVE_A_HOUR"].ToString());
                                sheet.GetRow(j).CreateCell(24).SetCellValue(dt3.Rows[l]["LEAVE_B_HOUR"].ToString());
                                sheet.GetRow(j).CreateCell(25).SetCellValue(dt3.Rows[l]["LEAVE_C_HOUR"].ToString());
                                sheet.GetRow(j).CreateCell(26).SetCellValue(dt3.Rows[l]["LEAVE_Q_HOUR"].ToString());
                                sheet.GetRow(j).CreateCell(27).SetCellValue(dt3.Rows[l]["LEAVE_OP_HOUR"].ToString());
                                sheet.GetRow(j).CreateCell(28).SetCellValue(dt3.Rows[l]["THIRD_CNT_P"].ToString());
                                sheet.GetRow(j).CreateCell(29).SetCellValue(dt3.Rows[l]["SECOND_CNT_P"].ToString());
                                sheet.GetRow(j).CreateCell(30).SetCellValue(dt3.Rows[l]["FIRST_CNT_P"].ToString());
                                sheet.GetRow(j).CreateCell(31).SetCellValue(dt3.Rows[l]["THIRD_CNT_M"].ToString());
                                sheet.GetRow(j).CreateCell(32).SetCellValue(dt3.Rows[l]["SECOND_CNT_M"].ToString());
                                sheet.GetRow(j).CreateCell(33).SetCellValue(dt3.Rows[l]["FIRST_CNT_M"].ToString());
                                sheet.GetRow(j).CreateCell(34).SetCellValue(dt3.Rows[l]["ATTEND_DAYS"].ToString());
                                sheet.GetRow(j).CreateCell(35).SetCellValue(dt3.Rows[l]["REWARD_DAYS"].ToString());
                                sheet.GetRow(j).CreateCell(36).SetCellValue(dt3.Rows[l]["DISCIPLINE_DAYS"].ToString());
                                sheet.GetRow(j).CreateCell(37).SetCellValue(dt3.Rows[l]["BONUS_WORK_DAYS"].ToString());
                                //sheet.GetRow(j).CreateCell(38).SetCellValue(dt3.Rows[l]["紅利發放天數"].ToString());
                                if (dt3.Rows[l]["BONUS_AMT"].ToString() == "")
                                    sheet.GetRow(j).CreateCell(39).SetCellValue(String.Format("{0:N0}", int.Parse("0")));
                                else
                                    sheet.GetRow(j).CreateCell(39).SetCellValue(String.Format("{0:N0}", int.Parse(dt3.Rows[l]["BONUS_AMT"].ToString())));
                                if (dt3.Rows[l]["BONUS_TAX"].ToString() == "")
                                    sheet.GetRow(j).CreateCell(40).SetCellValue(String.Format("{0:N0}", int.Parse("0")));
                                else
                                    sheet.GetRow(j).CreateCell(40).SetCellValue(String.Format("{0:N0}", int.Parse(dt3.Rows[l]["BONUS_TAX"].ToString())));
                                if (dt3.Rows[l]["BONUS_AMT_R"].ToString() == "")
                                    sheet.GetRow(j).CreateCell(41).SetCellValue(String.Format("{0:N0}", int.Parse("0")));
                                else
                                    sheet.GetRow(j).CreateCell(41).SetCellValue(String.Format("{0:N0}", int.Parse(dt3.Rows[l]["BONUS_AMT_R"].ToString())));
                                sheet.GetRow(j).CreateCell(42).SetCellValue(dt3.Rows[l]["PAY_TYPE"].ToString());
                                sheet.GetRow(j).CreateCell(43).SetCellValue(dt3.Rows[l]["CHG_STATUS_desc"].ToString());

                                sheet.GetRow(j).GetCell(15).CellStyle = stringRightStyle;
                                for (int m = 18; m <= 41; m++)
                                {
                                    if (m != 38)
                                        sheet.GetRow(j).GetCell(m).CellStyle = stringRightStyle;
                                }
                                j++;
                                sheet.CreateRow(j);

                            }
                        }
                        #endregion
                        //刪除


                    }
                    return workbook;
                    //if (data == "prev")
                    //    ExcelHandle.exportExcel(workbook, "前次核可資料比對." + type);
                    //else if (data == "original")
                    //    ExcelHandle.exportExcel(workbook, "原始資料比對." + type);
                }
                return null;
            }
            #endregion

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

    //一括異常註記-(Dtl)
    public string mark(List<Tuple<string, string>> keysListMark, List<Tuple<string, string>> keysList, CFB2SI0300DAO fb2si)
    {
        DataTable dt = new DataTable();
        string rtnmessage = "";//存在檢查後的訊息
        try
        {

            //檢查完成後，逐筆進行刪除
            if (rtnmessage == "")
            {
                try
                {
                    DateTime now = DateTime.Parse(DateTime.Now.ToString());
                    BeginTransaction();
                    //考核資料維護檔,備註說明 
                    fb2si.updateMarkData_H(now);

                    //先清空該頁的異常註記
                    foreach (var item in keysList)
                    {
                        fb2si = new CFB2SI0300DAO();
                        fb2si.BONUS_YEAR = item.Item1;
                        fb2si.EMP_ID = item.Item2;

                        //更新 考核人事資料維護檔 的異常註記為V
                        fb2si.updateMarkData_D(now, "");

                    }


                    foreach (var item in keysListMark)
                    {
                        fb2si = new CFB2SI0300DAO();
                        fb2si.BONUS_YEAR = item.Item1;
                        fb2si.EMP_ID = item.Item2;

                        //更新 考核人事資料維護檔 的異常註記為V
                        fb2si.updateMarkData_D(now, "V");

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

    //Approve
    public string Approve(string type, string BONUS_YEAR, CFB2SI0300DAO fb2si)
    {
        try
        {

            string rtnmessage = "";//存在檢查後的訊息

            int result = fb2si.getMarkData(BONUS_YEAR);
            if (result > 0)
            {
                rtnmessage += "請取消異常註記 \\n";

            }


            //檢查完成後，逐筆進行刪除
            if (rtnmessage == "")
            {
                try
                {
                    DateTime now = DateTime.Now;
                    DataTable dt = fb2si.TB_S_M_BONUS_H(BONUS_YEAR);
                    DataTable dt2 = fb2si.TB_S_M_BONUS_D(BONUS_YEAR);
                    BeginTransaction();

                    if (dt2.Rows.Count > 0)
                    {
                        fb2si.Update_TB_S_M_BONUS_D_Approve(BONUS_YEAR, now);
                    }
                    if (dt.Rows.Count > 0)
                    {
                        fb2si.Update_TB_S_M_BONUS_H(type, BONUS_YEAR, now);
                    }
                    fb2si.Update_TB_S_R_BONUS_D(BONUS_YEAR);
                    Commit();
                    return "0";
                }
                catch (Exception ex)
                {
                    RollBack();
                    return ex.Message;
                }
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
    //Reject
    public string Reject(string type, string BONUS_YEAR, CFB2SI0300DAO fb2si, List<string> EMP_IDs)
    {
        //CFB2SI0300DAO fb2si = new CFB2SI0300DAO();
        try
        {
            DateTime now = DateTime.Now;
            DataTable dt = fb2si.TB_S_M_BONUS_H(BONUS_YEAR);
            DataTable dt2 = fb2si.TB_S_M_BONUS_D(BONUS_YEAR);
            BeginTransaction();
            if (dt.Rows.Count > 0)
            {
                fb2si.Update_TB_S_M_BONUS_H(type, BONUS_YEAR, now);
            }
            if (dt2.Rows.Count > 0)
            {
                foreach (string EMP_ID in EMP_IDs)
                {
                    fb2si.Update_TB_S_M_BONUS_D_Reject(BONUS_YEAR, EMP_ID, now);
                }
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
}