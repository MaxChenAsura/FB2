using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;

/// <summary>
/// CFB2SH0400BO 的摘要描述
/// </summary>
public class CFB2SH0400BO : BaseService
{
    ICellStyle stringLeftStyle = null;
    ICellStyle stringRightStyle = null;
    ICellStyle stringCenterStyle = null;
    ICellStyle stringLeftRedStyle = null;
    ICellStyle stringRighRedStyle = null;

    IRow row = null;
    ICell cell = null;
    string dtFormat = "";
    int x = 2;//從第2列開始insert 資料

    //數字格式,有千分位,
    ICellStyle numbericStyle = null;

    public CFB2SH0400BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    //一括異常註記-(Dtl)
    public string mark(List<Tuple<string, string, string>> keysListMark, List<Tuple<string, string, string>> keysList, CFB2SH0400DAO sh040DAO)
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
                    sh040DAO.updateMarkData_H(now);

                    //先清空該頁的異常註記
                    foreach (var item in keysList)
                    {
                        sh040DAO = new CFB2SH0400DAO();
                        sh040DAO.AWARD_YEAR = item.Item1;
                        sh040DAO.AWARD_ROUND = item.Item2;
                        sh040DAO.EMP_ID = item.Item3;

                        //更新 考核人事資料維護檔 的異常註記為空白
                        sh040DAO.updateMarkData_D(now, "");

                    }


                    foreach (var item in keysListMark)
                    {
                        sh040DAO = new CFB2SH0400DAO();
                        sh040DAO.AWARD_YEAR = item.Item1;
                        sh040DAO.AWARD_ROUND = item.Item2;
                        sh040DAO.EMP_ID = item.Item3;

                        //更新 考核人事資料維護檔 的異常註記為V
                        sh040DAO.updateMarkData_D(now, "V");

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

    //駁回-(Dtl)
    public string reject(CFB2SH0400DAO sh040DAO)
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
                    //更新年獎維護檔,回復成未核可前狀態 
                    sh040DAO.updateRejectData_H(now);

                    /* 因改為分頁 且  增加一括異常註記後，不需要了
                    //更新年獎明細維護檔,將異常註記皆變為空白
                    //sh040DAO.updateAllRejectData_D(now);
                    foreach (var item in keysList)
                    {
                        sh040DAO = new CFB2SH0400DAO();
                        sh040DAO.AWARD_YEAR = item.Item1;
                        sh040DAO.AWARD_ROUND = item.Item2;
                        sh040DAO.EMP_ID = item.Item3;

                        //更新 年獎明細維護檔 的異常註記為V
                        sh040DAO.updateRejectData_D(now);

                    }
                    */
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

    //核可-(Dtl)
    public string approve(CFB2SH0400DAO sh040DAO)
    {
        DataTable dt = new DataTable();
        string rtnmessage = "";//存在檢查後的訊息
        try
        {

            int result = sh040DAO.getMarkData();
            if (result > 0)
            {
                rtnmessage += "請取消異常註記! \\n";

            }

            //檢查完成後，逐筆進行刪除
            if (rtnmessage == "")
            {
                try
                {
                    DateTime now = DateTime.Parse(DateTime.Now.ToString());
                    BeginTransaction();


                    //更新年獎明細維護檔,
                    sh040DAO.updateAllApproveData_D(now);

                    //刪除異動狀態為D的資料
                    sh040DAO.deleteStatusData_D();

                    //刪除 年獎明細主檔
                    sh040DAO.deleteApproveData_D_H(now);

                    //新増 年獎明細主檔
                    sh040DAO.insertApproveData_D_H(now);

                    //更新年獎維護檔 
                    sh040DAO.updateApproveData_H(now);

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


    //前次核可資料比對
    public IWorkbook createExcelFromTemplateToPre(string excelPath, CFB2SH0400DAO sh040DAO)
    {

        FileStream fs = null;
        IWorkbook workbook = null;

        //取得範本sheet
        ISheet sheet = null;
        try
        {
            fs = new FileStream(excelPath, FileMode.Open, FileAccess.ReadWrite);
            workbook = new XSSFWorkbook(fs); //xlsx的方法

            stringLeftStyle = this.setCellStyle(workbook, "left", true);
            stringRightStyle = this.setCellStyle(workbook, "right", true);
            stringCenterStyle = this.setCellStyle(workbook, "center", true);
            stringLeftRedStyle = this.setCellStyle(workbook, "left", true, 10);
            stringRighRedStyle = this.setCellStyle(workbook, "right", true, 10);
            //數字格式,有千分位,
            numbericStyle = workbook.CreateCellStyle();
            numbericStyle = stringRightStyle;
            numbericStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("#,##0");


            //取得範本sheet
            sheet = workbook.GetSheetAt(0);

            //若無差異資料時
            int resultCount = 0;

            if (sheet != null)
            {
                //取得新增的資料
                DataTable dt = sh040DAO.getAddExcelData("prev");
                if (dt.Rows.Count > 0)
                {

                    //dtFormat = dt.Rows[i]["FESTIVAL_DT"].ToString() != "" ? Convert.ToDateTime(dt.Rows[i]["FESTIVAL_DT"].ToString()).ToString("yyyy/MM/dd") : "";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        x += 1;
                        sheet = insertCell(sheet, dt, i, "N", "");
                    }
                    resultCount += 1;
                }

                //取得刪除的資料
                dt = new DataTable();
                dt = sh040DAO.getDelExcelData("prev");
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        x += 1;
                        sheet = insertCell(sheet, dt, i, "O", "");
                    }
                    resultCount += 1;
                }
                //取得比較的資料
                dt = new DataTable();
                dt = sh040DAO.getPreCompareData();
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        x += 1;
                        sheet = insertCell(sheet, dt, i, "N", "N_");//明細維護檔資料
                        x += 1;
                        sheet = insertCell(sheet, dt, i, "O", "O_"); //明細主檔資料
                    }
                    resultCount += 1;
                }

                //無任何差異時
                if (resultCount == 0)
                {
                    //將資料寫入範本
                    row = sheet.CreateRow(3);
                    //類別
                    cell = row.CreateCell(1);
                    cell.CellStyle = stringLeftStyle;  //先
                    cell.SetCellValue("無差異"); //後
                }
                ICellStyle stringLeftStyleDate = this.setCellStyle(workbook, "left", false);
                row = sheet.GetRow(0);
                cell = row.CreateCell(44);
                cell.CellStyle = stringLeftStyleDate;
                cell.SetCellValue("製表日期:" + DateTime.Now.ToString("yyyy/MM/dd"));

                for (int i = 0; i <= 44; i++)
                {
                    sheet.AutoSizeColumn(i);
                }
                //ExcelHandle.exportExcel(workbook, sh040DAO.AWARD_YEAR + "第" + sh040DAO.AWARD_ROUND + "回年獎前次核可資料比對.xlsx");
                return workbook;
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

    //原始資料比對
    public IWorkbook createExcelFromTemplateOriginal(string excelPath, CFB2SH0400DAO sh040DAO)
    {

        FileStream fs = null;
        IWorkbook workbook = null;

        //取得範本sheet
        ISheet sheet = null;
        try
        {
            fs = new FileStream(excelPath, FileMode.Open, FileAccess.ReadWrite);
            workbook = new XSSFWorkbook(fs); //xlsx的方法

            stringLeftStyle = this.setCellStyle(workbook, "left", true);
            stringRightStyle = this.setCellStyle(workbook, "right", true);
            stringCenterStyle = this.setCellStyle(workbook, "center", true);
            stringLeftRedStyle = this.setCellStyle(workbook, "left", true, 10);
            stringRighRedStyle = this.setCellStyle(workbook, "right", true, 10);
            //數字格式,有千分位,
            numbericStyle = workbook.CreateCellStyle();
            numbericStyle = stringRightStyle;
            numbericStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("#,##0");

            //若無差異資料時
            int resultCount = 0;

            //取得範本sheet
            sheet = workbook.GetSheetAt(0);

            if (sheet != null)
            {

                //取得新增的資料
                DataTable dt = sh040DAO.getAddExcelData("original");
                if (dt.Rows.Count > 0)
                {
                    //dtFormat = dt.Rows[i]["FESTIVAL_DT"].ToString() != "" ? Convert.ToDateTime(dt.Rows[i]["FESTIVAL_DT"].ToString()).ToString("yyyy/MM/dd") : "";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        x += 1;
                        sheet = insertCell(sheet, dt, i, "N", "");
                    }
                    resultCount += 1;
                }
                //取得刪除的資料
                dt = new DataTable();
                dt = sh040DAO.getDelExcelData("original");
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        x += 1;
                        sheet = insertCell(sheet, dt, i, "O", "");
                    }
                    resultCount += 1;
                }

                //取得比較的資料
                dt = new DataTable();
                dt = sh040DAO.getOriginalCompareData();
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        x += 1;
                        sheet = insertCell(sheet, dt, i, "N", "N_");//明細維護檔資料
                        x += 1;
                        sheet = insertCell(sheet, dt, i, "O", "O_"); //明細主檔資料
                    }
                    resultCount += 1;
                }

                //無任何差異時
                if (resultCount == 0)
                {
                    //將資料寫入範本
                    row = sheet.CreateRow(3);
                    //類別
                    cell = row.CreateCell(1);
                    cell.CellStyle = stringLeftStyle;  //先
                    cell.SetCellValue("無差異"); //後
                }
                ICellStyle stringLeftStyleDate = this.setCellStyle(workbook, "left", false);
                row = sheet.GetRow(0);
                cell = row.CreateCell(44);
                cell.CellStyle = stringLeftStyleDate;
                cell.SetCellValue("製表日期:" + DateTime.Now.ToString("yyyy/MM/dd"));


                for (int i = 0; i <= 49; i++)
                {
                    sheet.AutoSizeColumn(i);
                }
                //ExcelHandle.exportExcel(workbook, sh040DAO.AWARD_YEAR + "第" + sh040DAO.AWARD_ROUND + "回年獎原始資料比對.xlsx");
                return workbook;
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

    //比對資料的純新增及刪除
    public ISheet insertCell(ISheet sheet, DataTable dt, int i, string type, string tableCD)
    {

        try
        {
            //x = i + 3;//從第2列開始insert 資料
            //將資料寫入範本
            row = sheet.CreateRow(x);

            //類別
            cell = row.CreateCell(1);
            cell.CellStyle = stringLeftStyle;  //先
            cell.SetCellValue(type); //後
            //工號
            cell = row.CreateCell(2);
            cell.CellStyle = stringLeftStyle;  //先
            cell.SetCellValue(dt.Rows[i][tableCD + "EMP_ID"].ToString()); //後
            //姓名
            cell = row.CreateCell(3);
            cell.CellStyle = stringLeftStyle;
            cell.SetCellValue(dt.Rows[i][tableCD + "EMP_NAME"].ToString().Trim());
            //在職區分
            cell = row.CreateCell(4);
            cell.CellStyle = stringLeftStyle;
            cell.SetCellValue(dt.Rows[i][tableCD + "EMP_CHG_CD_DESC"].ToString());
            //職種
            cell = row.CreateCell(5);
            cell.CellStyle = stringLeftStyle;
            cell.SetCellValue(dt.Rows[i][tableCD + "WS_CD"].ToString());
            //外籍會社
            cell = row.CreateCell(6);
            cell.CellStyle = stringLeftStyle;
            cell.SetCellValue(dt.Rows[i][tableCD + "JPN_CD"].ToString());

            //6.部門代號
            cell = row.CreateCell(7);
            cell.CellStyle = stringLeftStyle;
            cell.SetCellValue(dt.Rows[i][tableCD + "DEPT_NO"].ToString());
            //資格代號
            cell = row.CreateCell(8);
            cell.CellStyle = stringLeftStyle;
            cell.SetCellValue(dt.Rows[i][tableCD + "LEVEL_CD"].ToString());
            //職務代號
            cell = row.CreateCell(9);
            cell.CellStyle = stringLeftStyle;
            cell.SetCellValue(dt.Rows[i][tableCD + "PJOB_CD"].ToString());
            //入社日期
            cell = row.CreateCell(10);
            cell.CellStyle = stringLeftStyle;
            dtFormat = dt.Rows[i][tableCD + "JOIN_DT"].ToString() != "" ? Convert.ToDateTime(dt.Rows[i][tableCD + "JOIN_DT"].ToString()).ToString("yyyy/MM/dd") : "";
            cell.SetCellValue(dtFormat);
            //離社日期
            cell = row.CreateCell(11);
            cell.CellStyle = stringLeftStyle;
            dtFormat = dt.Rows[i][tableCD + "LEAVE_DT"].ToString() != "" ? Convert.ToDateTime(dt.Rows[i][tableCD + "LEAVE_DT"].ToString()).ToString("yyyy/MM/dd") : "";
            cell.SetCellValue(dtFormat);

            //12留職日(留職停工日)
            cell = row.CreateCell(12);
            cell.CellStyle = stringLeftStyle;
            dtFormat = dt.Rows[i][tableCD + "STAY_DT"].ToString() != "" ? Convert.ToDateTime(dt.Rows[i][tableCD + "STAY_DT"].ToString()).ToString("yyyy/MM/dd") : "";
            cell.SetCellValue(dtFormat);
            //留廠日(轉期間工日)
            cell = row.CreateCell(13);
            cell.CellStyle = stringLeftStyle;
            dtFormat = dt.Rows[i][tableCD + "BE_CONTRACT_DT"].ToString() != "" ? Convert.ToDateTime(dt.Rows[i][tableCD + "BE_CONTRACT_DT"].ToString()).ToString("yyyy/MM/dd") : "";
            cell.SetCellValue(dtFormat);
            //轉正社員日
            cell = row.CreateCell(14);
            cell.CellStyle = stringLeftStyle;
            dtFormat = dt.Rows[i][tableCD + "BE_EMP_DT"].ToString() != "" ? Convert.ToDateTime(dt.Rows[i][tableCD + "BE_EMP_DT"].ToString()).ToString("yyyy/MM/dd") : "";
            cell.SetCellValue(dtFormat);
            //在職天數(年獎期間)
            cell = row.CreateCell(15);
            if (tableCD != "" && dt.Rows[i]["N_WORK_DAYS"].ToString() != dt.Rows[i]["O_WORK_DAYS"].ToString())
            {
                cell.CellStyle = stringRighRedStyle;
            }
            else
            {
                cell.CellStyle = stringRightStyle;
            }
            cell.SetCellValue(dt.Rows[i][tableCD + "WORK_DAYS"].ToString());
            //員工區分
            cell = row.CreateCell(16);
            cell.CellStyle = stringLeftStyle;
            cell.SetCellValue(dt.Rows[i][tableCD + "EMP_CD_DESC"].ToString());

            //17.身份標示
            cell = row.CreateCell(17);
            cell.CellStyle = stringLeftStyle;
            cell.SetCellValue(dt.Rows[i][tableCD + "ID_DESC"].ToString());
            //職能俸
            cell = row.CreateCell(18);
            cell.CellStyle = numbericStyle;
            cell = row.CreateCell(15);
            if (tableCD != "" && dt.Rows[i]["N_ABILITY_PAY"].ToString() != dt.Rows[i]["O_ABILITY_PAY"].ToString())
            {
                cell.CellStyle = stringRighRedStyle;
            }
            else
            {
                cell.CellStyle = stringRightStyle;
            }
            cell.SetCellValue((Convert.ToDouble(dt.Rows[i][tableCD + "ABILITY_PAY"].ToString())).ToString("N0"));
            //資格俸
            cell = row.CreateCell(19);
            if (tableCD != "" && dt.Rows[i]["N_LEVEL_PAY"].ToString() != dt.Rows[i]["O_LEVEL_PAY"].ToString())
            {
                cell.CellStyle = stringRighRedStyle;
            }
            else
            {
                cell.CellStyle = stringRightStyle;
            }
            cell.SetCellValue((Convert.ToDouble(dt.Rows[i][tableCD + "LEVEL_PAY"].ToString())).ToString("N0"));
            //職務俸
            cell = row.CreateCell(20);
            if (tableCD != "" && dt.Rows[i]["N_PJOB_PAY"].ToString() != dt.Rows[i]["O_PJOB_PAY"].ToString())
            {
                cell.CellStyle = stringRighRedStyle;
            }
            else
            {
                cell.CellStyle = stringRightStyle;
            }
            cell.SetCellValue((Convert.ToDouble(dt.Rows[i][tableCD + "PJOB_PAY"].ToString())).ToString("N0"));
            //專業俸
            cell = row.CreateCell(21);
            if (tableCD != "" && dt.Rows[i]["N_PROFESSION_PAY"].ToString() != dt.Rows[i]["O_PROFESSION_PAY"].ToString())
            {
                cell.CellStyle = stringRighRedStyle;
            }
            else
            {
                cell.CellStyle = stringRightStyle;
            }
            cell.SetCellValue((Convert.ToDouble(dt.Rows[i][tableCD + "PROFESSION_PAY"].ToString())).ToString("N0"));

            //21伙食津貼
            cell = row.CreateCell(22);
            if (tableCD != "" && dt.Rows[i]["N_FOOD_SUBSIDY"].ToString() != dt.Rows[i]["O_FOOD_SUBSIDY"].ToString())
            {
                cell.CellStyle = stringRighRedStyle;
            }
            else
            {
                cell.CellStyle = stringRightStyle;
            }
            cell.SetCellValue((Convert.ToDouble(dt.Rows[i][tableCD + "FOOD_SUBSIDY"].ToString())).ToString("N0"));
            //原始考績(業績)
            cell = row.CreateCell(23);
            if (tableCD != "" && dt.Rows[i]["N_SCORE_2H"].ToString() != dt.Rows[i]["O_SCORE_2H"].ToString())
            {
                cell.CellStyle = stringLeftRedStyle;
            }
            else
            {
                cell.CellStyle = stringLeftStyle;
            }
            cell.SetCellValue(dt.Rows[i][tableCD + "SCORE_2H"].ToString());
            //考績反映(年獎格差)
            cell = row.CreateCell(24);
            if (tableCD != "" && dt.Rows[i]["N_AWARD_BASE"].ToString() != dt.Rows[i]["O_AWARD_BASE"].ToString())
            {
                cell.CellStyle = stringLeftRedStyle;
            }
            else
            {
                cell.CellStyle = stringLeftStyle;
            }
            cell.SetCellValue(dt.Rows[i][tableCD + "AWARD_BASE"].ToString());
            //事假時數
            cell = row.CreateCell(25);
            if (tableCD != "" && dt.Rows[i]["N_LEAVE_A_HOUR"].ToString() != dt.Rows[i]["O_LEAVE_A_HOUR"].ToString())
            {
                cell.CellStyle = stringRighRedStyle;
            }
            else
            {
                cell.CellStyle = stringRightStyle;
            }
            cell.SetCellValue(dt.Rows[i][tableCD + "LEAVE_A_HOUR"].ToString());
            //有薪病假時數
            cell = row.CreateCell(26);
            if (tableCD != "" && dt.Rows[i]["N_LEAVE_B_HOUR"].ToString() != dt.Rows[i]["O_LEAVE_B_HOUR"].ToString())
            {
                cell.CellStyle = stringRighRedStyle;
            }
            else
            {
                cell.CellStyle = stringRightStyle;
            }
            cell.SetCellValue(dt.Rows[i][tableCD + "LEAVE_B_HOUR"].ToString());

            //26無薪病假時數
            cell = row.CreateCell(27);
            if (tableCD != "" && dt.Rows[i]["N_LEAVE_C_HOUR"].ToString() != dt.Rows[i]["O_LEAVE_C_HOUR"].ToString())
            {
                cell.CellStyle = stringRighRedStyle;
            }
            else
            {
                cell.CellStyle = stringRightStyle;
            }
            cell.SetCellValue(dt.Rows[i][tableCD + "LEAVE_C_HOUR"].ToString());
            //曠工時數
            cell = row.CreateCell(28);
            if (tableCD != "" && dt.Rows[i]["N_LEAVE_Q_HOUR"].ToString() != dt.Rows[i]["O_LEAVE_Q_HOUR"].ToString())
            {
                cell.CellStyle = stringRighRedStyle;
            }
            else
            {
                cell.CellStyle = stringRightStyle;
            };
            cell.SetCellValue(dt.Rows[i][tableCD + "LEAVE_Q_HOUR"].ToString());
            //遲到/早退 次數
            cell = row.CreateCell(29);
            if (tableCD != "" && dt.Rows[i]["N_LEAVE_OP_HOUR"].ToString() != dt.Rows[i]["O_LEAVE_OP_HOUR"].ToString())
            {
                cell.CellStyle = stringRighRedStyle;
            }
            else
            {
                cell.CellStyle = stringRightStyle;
            };
            cell.SetCellValue(dt.Rows[i][tableCD + "LEAVE_OP_HOUR"].ToString());
            //嘉獎
            cell = row.CreateCell(30);
            if (tableCD != "" && dt.Rows[i]["N_THIRD_CNT_P"].ToString() != dt.Rows[i]["O_THIRD_CNT_P"].ToString())
            {
                cell.CellStyle = stringRighRedStyle;
            }
            else
            {
                cell.CellStyle = stringRightStyle;
            }
            cell.SetCellValue(dt.Rows[i][tableCD + "THIRD_CNT_P"].ToString());
            //小功       
            cell = row.CreateCell(31);
            if (tableCD != "" && dt.Rows[i]["N_SECOND_CNT_P"].ToString() != dt.Rows[i]["O_SECOND_CNT_P"].ToString())
            {
                cell.CellStyle = stringRighRedStyle;
            }
            else
            {
                cell.CellStyle = stringRightStyle;
            }
            cell.SetCellValue(dt.Rows[i][tableCD + "SECOND_CNT_P"].ToString());

            //31大功
            cell = row.CreateCell(32);
            if (tableCD != "" && dt.Rows[i]["N_FIRST_CNT_P"].ToString() != dt.Rows[i]["O_FIRST_CNT_P"].ToString())
            {
                cell.CellStyle = stringRighRedStyle;
            }
            else
            {
                cell.CellStyle = stringRightStyle;
            }
            cell.SetCellValue(dt.Rows[i][tableCD + "FIRST_CNT_P"].ToString());
            //申誡
            cell = row.CreateCell(33);
            if (tableCD != "" && dt.Rows[i]["N_THIRD_CNT_M"].ToString() != dt.Rows[i]["O_THIRD_CNT_M"].ToString())
            {
                cell.CellStyle = stringRighRedStyle;
            }
            else
            {
                cell.CellStyle = stringRightStyle;
            }
            cell.SetCellValue(dt.Rows[i][tableCD + "THIRD_CNT_M"].ToString());
            //小過
            cell = row.CreateCell(34);
            if (tableCD != "" && dt.Rows[i]["N_SECOND_CNT_M"].ToString() != dt.Rows[i]["O_SECOND_CNT_M"].ToString())
            {
                cell.CellStyle = stringRighRedStyle;
            }
            else
            {
                cell.CellStyle = stringRightStyle;
            }
            cell.SetCellValue(dt.Rows[i][tableCD + "SECOND_CNT_M"].ToString());
            //大過
            cell = row.CreateCell(35);
            if (tableCD != "" && dt.Rows[i]["N_FIRST_CNT_M"].ToString() != dt.Rows[i]["O_FIRST_CNT_M"].ToString())
            {
                cell.CellStyle = stringRighRedStyle;
            }
            else
            {
                cell.CellStyle = stringRightStyle;
            }
            cell.SetCellValue(dt.Rows[i][tableCD + "FIRST_CNT_M"].ToString());
            //勤怠扣除天數       
            cell = row.CreateCell(36);
            if (tableCD != "" && dt.Rows[i]["N_ATTEND_DAYS"].ToString() != dt.Rows[i]["O_ATTEND_DAYS"].ToString())
            {
                cell.CellStyle = stringRighRedStyle;
            }
            else
            {
                cell.CellStyle = stringRightStyle;
            }
            cell.SetCellValue(dt.Rows[i][tableCD + "ATTEND_DAYS"].ToString());

            //36獎懲加減天數
            cell = row.CreateCell(37);
            if (tableCD != "" && dt.Rows[i]["N_REWARD_DAYS"].ToString() != dt.Rows[i]["O_REWARD_DAYS"].ToString())
            {
                cell.CellStyle = stringRighRedStyle;
            }
            else
            {
                cell.CellStyle = stringRightStyle;
            }
            cell.SetCellValue(dt.Rows[i][tableCD + "REWARD_DAYS"].ToString());
            //紀律扣除天數
            cell = row.CreateCell(38);
            if (tableCD != "" && dt.Rows[i]["N_DISCIPLINE_DAYS"].ToString() != dt.Rows[i]["O_DISCIPLINE_DAYS"].ToString())
            {
                cell.CellStyle = stringRighRedStyle;
            }
            else
            {
                cell.CellStyle = stringRightStyle;
            }
            cell.SetCellValue(dt.Rows[i][tableCD + "DISCIPLINE_DAYS"].ToString());
            //實際在職天數
            cell = row.CreateCell(39);
            if (tableCD != "" && dt.Rows[i]["N_AWARD_WORK_DAYS"].ToString() != dt.Rows[i]["O_AWARD_WORK_DAYS"].ToString())
            {
                cell.CellStyle = stringRighRedStyle;
            }
            else
            {
                cell.CellStyle = stringRightStyle;
            }
            cell.SetCellValue(dt.Rows[i][tableCD + "AWARD_WORK_DAYS"].ToString());
            //昇格者Y / N       
            cell = row.CreateCell(40);
            if (tableCD != "" && dt.Rows[i]["N_LEVELUP_FLAG"].ToString() != dt.Rows[i]["O_LEVELUP_FLAG"].ToString())
            {
                cell.CellStyle = stringRighRedStyle;
            }
            else
            {
                cell.CellStyle = stringRightStyle;
            }
            cell.SetCellValue(dt.Rows[i][tableCD + "LEVELUP_FLAG"].ToString());

            //41年獎金額
            cell = row.CreateCell(41);
            if (tableCD != "" && dt.Rows[i]["N_AWARD_AMT"].ToString() != dt.Rows[i]["O_AWARD_AMT"].ToString())
            {
                cell.CellStyle = stringRighRedStyle;
            }
            else
            {
                cell.CellStyle = stringRightStyle;
            }
            cell.SetCellValue((Convert.ToDouble(dt.Rows[i][tableCD + "AWARD_AMT"].ToString())).ToString("N0"));
            //支付狀態
            cell = row.CreateCell(42);
            if (tableCD != "" && dt.Rows[i]["N_PAY_TYPE"].ToString() != dt.Rows[i]["O_PAY_TYPE"].ToString())
            {
                cell.CellStyle = stringLeftRedStyle;
            }
            else
            {
                cell.CellStyle = stringLeftStyle;
            }
            cell.SetCellValue(dt.Rows[i][tableCD + "PAY_TYPE_DESC"].ToString());
            //異動狀態
            cell = row.CreateCell(43);
            cell.CellStyle = stringLeftStyle;
            if (tableCD != "" && dt.Rows[i]["N_CHG_STATUS"].ToString() != dt.Rows[i]["O_CHG_STATUS"].ToString())
            {
                cell.CellStyle = stringLeftRedStyle;
            }
            else
            {
                cell.CellStyle = stringLeftStyle;
            }
            cell.SetCellValue(dt.Rows[i][tableCD + "CHG_STATUS_DESC"].ToString());
            return sheet;
        }
        catch (Exception)
        {
            throw;
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








}