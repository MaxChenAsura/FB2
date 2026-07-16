using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.Util;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;

/// <summary>
/// CFB2HB0600BO 的摘要描述
/// </summary>
public class CFB2HB0600BO : BaseService
{

    IRow row_class;
    ICell cell_class;
    //白底藍字
    ICellStyle stringLeftBlue_12;
    ICellStyle stringCenterBlue_12;

    //白底黑字
    ICellStyle stringCenterBlack_12;
    ICellStyle stringLeftBlack_12;
    ICellStyle stringRightBlack_12;

    //灰色底黑色字
    ICellStyle stringRightBlack_12_Grey;
    ICellStyle stringLeftBlack_12_Grey;

    public CFB2HB0600BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public System.Data.DataTable getData(string emp_id)
    {
        CFB2HB0600DAO dao = new CFB2HB0600DAO();
        try
        {
            return dao.getDefaultData(emp_id);
        }
        catch (Exception)
        {

            throw;
        }
    }
    #region EXCEL匯出
    public IWorkbook ExportExcelNew(string emp_id)
    {
        try
        {
            string is_exist = "0";
            string path = "";
            string photoPath = "";
            //Excel初始化
            IWorkbook workbook;
            ISheet sheet;
            workbook = new XSSFWorkbook();

            CFB2HB0600DAO dao = new CFB2HB0600DAO();
            List<string> List_emp_id = emp_id.Split(',').ToList();
            //產生Excel
            IRow row;
            ICell cell;

            //白底藍字,style =LB, CB
            stringLeftBlue_12 = this.setCellStyle(workbook, "left", true, 0, 12, 12);
            stringCenterBlue_12 = this.setCellStyle(workbook, "center", true, 0, 12, 12);

            //白底黑字,style =CB, LB
            stringCenterBlack_12 = this.setCellStyle(workbook, "center", true);
            stringLeftBlack_12 = this.setCellStyle(workbook, "left", true);
            stringRightBlack_12 = this.setCellStyle(workbook, "right", true);

            //灰色底黑色字,style =RBG, LBG
            stringRightBlack_12_Grey = this.setCellStyle(workbook, "right", true, 22, 12, 8);
            stringLeftBlack_12_Grey = this.setCellStyle(workbook, "left", true, 22, 12, 8);

            //表頭
            ICellStyle stringCenterBlack_title = this.setCellStyle(workbook, "center", false, 0, 18, 8);

            foreach (var emp in List_emp_id)
            {
                sheet = workbook.CreateSheet(emp);

                //每欄位的寬度
                for (int i = 0; i < 7; i++)
                {
                    sheet.SetColumnWidth(i, 18 * 256);
                }

                #region 取得基本資料
                DataTable defaultData = dao.getDefaultData(emp);  //基本資料
                if (defaultData.Rows.Count == 0)
                {
                    continue;
                }
                //第0行
                sheet = createRowCell(sheet, 0);
                cell = sheet.GetRow(0).GetCell(0);
                cell.CellStyle = stringCenterBlack_title;
                cell.SetCellValue("個人人事履歷資料");
                sheet.AddMergedRegion(new CellRangeAddress(0, 0, 0, 6));
                //第1行 
                sheet = createRowCell(sheet, 1);
                cell = sheet.GetRow(1).GetCell(0);
                cell.CellStyle = stringLeftBlack_12;
                cell.SetCellValue("【基本資料】");
                sheet.AddMergedRegion(new CellRangeAddress(1, 1, 0, 6));
                //第2行 
                sheet = createRowCell(sheet, 2);
                cell = sheet.GetRow(2).GetCell(0);
                cell.CellStyle = stringRightBlack_12_Grey;
                cell.SetCellValue("姓名:");
                cell = sheet.GetRow(2).GetCell(1);
                cell.CellStyle = stringCenterBlue_12;
                cell.SetCellValue(defaultData.Rows[0]["EMP_NAME"].ToString());
                cell = sheet.GetRow(2).GetCell(2);
                cell.CellStyle = stringRightBlack_12_Grey;
                cell.SetCellValue("工號:");
                cell = sheet.GetRow(2).GetCell(3);
                cell.CellStyle = stringCenterBlue_12;
                cell.SetCellValue(defaultData.Rows[0]["EMP_ID"].ToString());
                cell = sheet.GetRow(2).GetCell(4);
                cell.CellStyle = stringRightBlack_12_Grey;
                cell.SetCellValue("在職區分:");
                cell = sheet.GetRow(2).GetCell(5);
                cell.CellStyle = stringCenterBlue_12;
                cell.SetCellValue(defaultData.Rows[0]["EMP_CHG_DESC"].ToString());
                //第3行 
                sheet = createRowCell(sheet, 3);
                cell = sheet.GetRow(3).GetCell(0);
                cell.CellStyle = stringRightBlack_12_Grey;
                cell.SetCellValue("資格:");
                cell = sheet.GetRow(3).GetCell(1);
                cell.CellStyle = stringCenterBlue_12;
                cell.SetCellValue(defaultData.Rows[0]["LEVEL_CD"].ToString());
                cell = sheet.GetRow(3).GetCell(2);
                cell.CellStyle = stringRightBlack_12_Grey;
                cell.SetCellValue("職務:");
                cell = sheet.GetRow(3).GetCell(3);
                cell.CellStyle = stringCenterBlue_12;
                cell.SetCellValue(defaultData.Rows[0]["PJOB_DESC"].ToString());
                cell = sheet.GetRow(3).GetCell(4);
                cell.CellStyle = stringRightBlack_12_Grey;
                cell.SetCellValue("職種:");
                cell = sheet.GetRow(3).GetCell(5);
                cell.CellStyle = stringCenterBlue_12;
                cell.SetCellValue(defaultData.Rows[0]["WS_DESC"].ToString());
                //第4行
                sheet = createRowCell(sheet, 4);
                cell = sheet.GetRow(4).GetCell(0);
                cell.CellStyle = stringRightBlack_12_Grey;
                cell.SetCellValue("廠區:");
                cell = sheet.GetRow(4).GetCell(1);
                cell.CellStyle = stringCenterBlue_12;
                cell.SetCellValue(defaultData.Rows[0]["PLANT_NAME"].ToString());
                cell = sheet.GetRow(4).GetCell(2);
                cell.CellStyle = stringRightBlack_12_Grey;
                cell.SetCellValue("薪資:");
                cell = sheet.GetRow(4).GetCell(3);
                cell.CellStyle = stringCenterBlue_12;
                cell.SetCellValue(defaultData.Rows[0]["BASE_SALARY"].ToString() + "  (職能俸+資格俸+職務俸+專業俸+伙食津貼)");
                sheet.AddMergedRegion(new CellRangeAddress(4, 4, 3, 5));
                //第5行 
                sheet = createRowCell(sheet, 5);
                cell = sheet.GetRow(5).GetCell(0);
                cell.CellStyle = stringRightBlack_12_Grey;
                cell.SetCellValue("部門:");
                cell = sheet.GetRow(5).GetCell(1);
                cell.CellStyle = stringLeftBlue_12;
                cell.SetCellValue(defaultData.Rows[0]["DEPT_NAME"].ToString());
                sheet.AddMergedRegion(new CellRangeAddress(5, 5, 1, 5));

                //第6行 
                sheet = createRowCell(sheet, 6);
                cell = sheet.GetRow(6).GetCell(0);
                cell.CellStyle = stringRightBlack_12_Grey;
                cell.SetCellValue("入社日期:");
                cell = sheet.GetRow(6).GetCell(1);
                cell.CellStyle = stringCenterBlue_12;
                cell.SetCellValue(defaultData.Rows[0]["JOIN_DT"].ToString());
                cell = sheet.GetRow(6).GetCell(2);
                cell.CellStyle = stringRightBlack_12_Grey;
                cell.SetCellValue("出生日期:");
                cell = sheet.GetRow(6).GetCell(3);
                cell.CellStyle = stringCenterBlue_12;
                cell.SetCellValue(defaultData.Rows[0]["BIRTH_DT"].ToString());
                cell = sheet.GetRow(6).GetCell(4);
                cell.CellStyle = stringRightBlack_12_Grey;
                cell.SetCellValue("年齡:");
                cell = sheet.GetRow(6).GetCell(5);
                cell.CellStyle = stringCenterBlue_12;
                cell.SetCellValue(defaultData.Rows[0]["AGE"].ToString());
                //第7行 
                sheet = createRowCell(sheet, 7);
                cell = sheet.GetRow(7).GetCell(0);
                cell.CellStyle = stringRightBlack_12_Grey;
                cell.SetCellValue("入社年資:");
                cell = sheet.GetRow(7).GetCell(1);
                cell.CellStyle = stringCenterBlue_12;
                cell.SetCellValue(defaultData.Rows[0]["WORK_YEARS"].ToString());
                cell = sheet.GetRow(7).GetCell(2);
                cell.CellStyle = stringRightBlack_12_Grey;
                cell.SetCellValue("資格年資:");
                cell = sheet.GetRow(7).GetCell(3);
                cell.CellStyle = stringCenterBlue_12;
                cell.SetCellValue(defaultData.Rows[0]["RECENT_LEVEL_WORK_DAYS"].ToString());
                cell = sheet.GetRow(7).GetCell(4);
                cell.CellStyle = stringRightBlack_12_Grey;
                cell.SetCellValue("性別:");
                cell = sheet.GetRow(7).GetCell(5);
                cell.CellStyle = stringCenterBlue_12;
                cell.SetCellValue(defaultData.Rows[0]["SEX_DESC"].ToString());
                //第8行 
                sheet = createRowCell(sheet, 8);
                cell = sheet.GetRow(8).GetCell(0);
                cell.CellStyle = stringRightBlack_12_Grey;
                cell.SetCellValue("日文:");
                cell = sheet.GetRow(8).GetCell(2);
                cell.CellStyle = stringRightBlack_12_Grey;
                cell.SetCellValue("英文TOEIC:");
                cell = sheet.GetRow(8).GetCell(4);
                cell.CellStyle = stringRightBlack_12_Grey;
                cell.SetCellValue("模範員工年度:");
                cell = sheet.GetRow(8).GetCell(5);
                cell.CellStyle = stringCenterBlue_12;
                cell.SetCellValue(defaultData.Rows[0]["MODEL_YEAR"].ToString());
                DataTable dtScore = dao.getTOTAL_SCORE(emp);  //多益及日文成績
                if (dtScore.Rows.Count > 0)
                {
                    cell = sheet.GetRow(8).GetCell(1);
                    cell.CellStyle = stringCenterBlue_12;
                    cell.SetCellValue(dtScore.Rows[0]["LANGUAGE_JAPANESE"].ToString());
                    cell = sheet.GetRow(8).GetCell(3);
                    cell.CellStyle = stringCenterBlue_12;
                    cell.SetCellValue(dtScore.Rows[0]["LANGUAGE_TOEIC"].ToString());
                }

                //照片
                row = sheet.GetRow(2);
                cell = row.GetCell(6);
                sheet.AddMergedRegion(new CellRangeAddress(2, 8, 6, 6));
                path = defaultData.Rows[0]["PHOTO_PATH"].ToString();
                var patriarch = sheet.CreateDrawingPatriarch();
                int pictureIndex = 0;
                if (File.Exists(path))
                {
                    photoPath = path;
                }
                else if (File.Exists(defaultData.Rows[0]["PHOTO_PATH_KUOZUI"].ToString()))
                {
                    photoPath = defaultData.Rows[0]["PHOTO_PATH_KUOZUI"].ToString();
                }

                if (photoPath != "")
                {
                    try
                    {
                        System.Drawing.Image original = System.Drawing.Image.FromFile(photoPath);
                        System.Drawing.Image resized = ResizeImage(original, new Size(120, 140));

                        byte[] buffer = new byte[16 * 1024];
                        using (MemoryStream oMemoryStream = new MemoryStream())
                        {
                            using (Bitmap oBitmap = new Bitmap(resized))
                            {
                                //儲存圖片到 MemoryStream 物件，並且指定儲存影像之格式 
                                oBitmap.Save(oMemoryStream, ImageFormat.Jpeg);
                                //設定資料流位置 
                                oMemoryStream.Position = 0;
                                //設定 buffer 長度 
                                buffer = new byte[oMemoryStream.Length];
                                //將資料寫入 buffer 
                                oMemoryStream.Read(buffer, 0, Convert.ToInt32(oMemoryStream.Length));
                                //將所有緩衝區的資料寫入資料流 
                                oMemoryStream.Flush();
                                pictureIndex = workbook.AddPicture(oMemoryStream.ToArray(), PictureType.JPEG);

                                // 將縮圖定位到 worksheet 中
                                var anchor = new XSSFClientAnchor(5, 5, 0, 0, 6, 2, 7, 9);
                                var picture = patriarch.CreatePicture(anchor, pictureIndex);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //不處理
                    }
                }

                #endregion

                #region 考績主檔資料
                sheet = createRowCell(sheet, 11);
                sheet = createRowCell(sheet, 12);
                sheet = createRowCell(sheet, 13);
                sheet = createRowCell(sheet, 14);

                cell = sheet.GetRow(11).GetCell(0);
                cell.CellStyle = stringLeftBlack_12_Grey;
                cell.SetCellValue("【考核】");
                sheet.AddMergedRegion(new CellRangeAddress(11, 11, 0, 6));
                cell = sheet.GetRow(12).GetCell(0);
                cell.CellStyle = stringCenterBlack_12;
                cell.SetCellValue("考核類別");
                cell = sheet.GetRow(13).GetCell(0);
                cell.CellStyle = stringCenterBlack_12;
                cell.SetCellValue("能力考課");
                cell = sheet.GetRow(14).GetCell(0);
                cell.CellStyle = stringCenterBlack_12;
                cell.SetCellValue("業績考課");

                DataTable AssessData = dao.getAssessData(emp);
                if (AssessData.Rows.Count > 0)
                {
                    for (int i = 0; i < AssessData.Rows.Count; i++)
                    {
                        cell = sheet.GetRow(12).GetCell(i + 1);
                        cell.CellStyle = stringCenterBlue_12;
                        cell.SetCellValue(AssessData.Rows[i]["ASSESS_YEAR"].ToString());
                        cell = sheet.GetRow(13).GetCell(i + 1);
                        cell.CellStyle = stringCenterBlue_12;
                        cell.SetCellValue(AssessData.Rows[i]["SCORE_1H"].ToString());
                        cell = sheet.GetRow(14).GetCell(i + 1);
                        cell.CellStyle = stringCenterBlue_12;
                        cell.SetCellValue(AssessData.Rows[i]["SCORE_2H"].ToString());
                    }

                }

                #endregion

                #region 人事異動履歷
                sheet = createRowCell(sheet, 16);
                sheet = createRowCell(sheet, 17);
                cell = sheet.GetRow(16).GetCell(0);
                cell.CellStyle = stringLeftBlack_12_Grey;
                cell.SetCellValue("【人事異動履歷】");
                sheet.AddMergedRegion(new CellRangeAddress(16, 16, 0, 6));
                cell = sheet.GetRow(17).GetCell(0);
                cell.CellStyle = stringCenterBlack_12;
                cell.SetCellValue("生效日期");
                cell = sheet.GetRow(17).GetCell(1);
                cell.CellStyle = stringCenterBlack_12;
                cell.SetCellValue("異動區分");
                cell = sheet.GetRow(17).GetCell(2);
                cell.CellStyle = stringCenterBlack_12;
                cell.SetCellValue("部門");
                sheet.AddMergedRegion(new CellRangeAddress(17, 17, 2, 4));
                cell = sheet.GetRow(17).GetCell(5);
                cell.CellStyle = stringCenterBlack_12;
                cell.SetCellValue("資格");
                cell = sheet.GetRow(17).GetCell(6);
                cell.CellStyle = stringCenterBlack_12;
                cell.SetCellValue("職務");

                DataTable ChgData = dao.getChgData(emp);
                int rowNum = 18;
                for (int i = 0; i < ChgData.Rows.Count; i++)
                {
                    sheet = createRowCell(sheet, rowNum);
                    cell = sheet.GetRow(rowNum).GetCell(0);
                    cell.CellStyle = stringCenterBlue_12;
                    cell.SetCellValue(ChgData.Rows[i]["START_DT"].ToString());
                    cell = sheet.GetRow(rowNum).GetCell(1);
                    cell.CellStyle = stringCenterBlue_12;
                    cell.SetCellValue(ChgData.Rows[i]["HR_CHG_DESC"].ToString());
                    cell = sheet.GetRow(rowNum).GetCell(2);
                    cell.CellStyle = stringLeftBlue_12;
                    cell.SetCellValue(ChgData.Rows[i]["DEPT_FULL_NAME"].ToString());
                    sheet.AddMergedRegion(new CellRangeAddress(rowNum, rowNum, 2, 4));
                    cell = sheet.GetRow(rowNum).GetCell(5);
                    cell.CellStyle = stringCenterBlue_12;
                    cell.SetCellValue(ChgData.Rows[i]["LEVEL_CD"].ToString());
                    cell = sheet.GetRow(rowNum).GetCell(6);
                    cell.CellStyle = stringLeftBlue_12;
                    cell.SetCellValue(ChgData.Rows[i]["PJOB_DESC"].ToString().Trim());
                    rowNum++;
                }
                int nextStartIndex = rowNum + 1;//+1:表空一行



                #endregion

                #region 員工外調履歷檔資料
                sheet = createRowCell(sheet, nextStartIndex);
                sheet = createRowCell(sheet, nextStartIndex + 1);
                cell = sheet.GetRow(nextStartIndex).GetCell(0);
                cell.CellStyle = stringLeftBlack_12_Grey;
                cell.SetCellValue("【外調履歷】");
                sheet.AddMergedRegion(new CellRangeAddress(nextStartIndex, nextStartIndex, 0, 6));
                cell = sheet.GetRow(nextStartIndex + 1).GetCell(0);
                cell.CellStyle = stringCenterBlack_12;
                cell.SetCellValue("開始日期");
                cell = sheet.GetRow(nextStartIndex + 1).GetCell(1);
                cell.CellStyle = stringCenterBlack_12;
                cell.SetCellValue("結束日期");
                cell = sheet.GetRow(nextStartIndex + 1).GetCell(2);
                cell.CellStyle = stringCenterBlack_12;
                cell.SetCellValue("外調類別");
                cell = sheet.GetRow(nextStartIndex + 1).GetCell(3);
                cell.CellStyle = stringCenterBlack_12;
                cell.SetCellValue("ICT類別");
                cell = sheet.GetRow(nextStartIndex + 1).GetCell(4);
                cell.CellStyle = stringCenterBlack_12;
                cell.SetCellValue("原籍資格");
                cell = sheet.GetRow(nextStartIndex + 1).GetCell(5);
                cell.CellStyle = stringCenterBlack_12;
                cell.SetCellValue("原派遣部門");
                cell = sheet.GetRow(nextStartIndex + 1).GetCell(6);
                cell.CellStyle = stringCenterBlack_12;
                cell.SetCellValue("受入部門");

                DataTable TransData = dao.getTransData(emp);
                rowNum = nextStartIndex + 2;
                for (int i = 0; i < TransData.Rows.Count; i++)
                {
                    sheet = createRowCell(sheet, rowNum);
                    cell = sheet.GetRow(rowNum).GetCell(0);
                    cell.CellStyle = stringCenterBlue_12;
                    cell.SetCellValue(TransData.Rows[i]["START_DT"].ToString());
                    cell = sheet.GetRow(rowNum).GetCell(1);
                    cell.CellStyle = stringCenterBlue_12;
                    cell.SetCellValue(TransData.Rows[i]["END_DT"].ToString());
                    cell = sheet.GetRow(rowNum).GetCell(2);
                    cell.CellStyle = stringCenterBlue_12;
                    cell.SetCellValue(TransData.Rows[i]["HR_CHG_DESC"].ToString());
                    cell = sheet.GetRow(rowNum).GetCell(3);
                    cell.CellStyle = stringCenterBlue_12;
                    cell.SetCellValue(TransData.Rows[i]["ICT_TYPE"].ToString());
                    cell = sheet.GetRow(rowNum).GetCell(4);
                    cell.CellStyle = stringCenterBlue_12;
                    cell.SetCellValue(TransData.Rows[i]["ORI_LEVEL_CD"].ToString());
                    cell = sheet.GetRow(rowNum).GetCell(5);
                    cell.CellStyle = stringLeftBlue_12;
                    cell.SetCellValue(TransData.Rows[i]["ORI_DEPT_NAME_20"].ToString());
                    cell = sheet.GetRow(rowNum).GetCell(6);
                    cell.CellStyle = stringLeftBlue_12;
                    cell.SetCellValue(TransData.Rows[i]["TRANSFER_DEPT"].ToString());
                    rowNum++;
                }

                nextStartIndex = rowNum + 1;//+1:表空一行
                #endregion

                #region 員工國外研修資料檔資料
                sheet = createRowCell(sheet, nextStartIndex);
                sheet = createRowCell(sheet, nextStartIndex + 1);
                cell = sheet.GetRow(nextStartIndex).GetCell(0);
                cell.CellStyle = stringLeftBlack_12_Grey;
                cell.SetCellValue("【國外研修】");
                sheet.AddMergedRegion(new CellRangeAddress(nextStartIndex, nextStartIndex, 0, 6));
                cell = sheet.GetRow(nextStartIndex + 1).GetCell(0);
                cell.CellStyle = stringCenterBlack_12;
                cell.SetCellValue("開始日期");
                cell = sheet.GetRow(nextStartIndex + 1).GetCell(1);
                cell.CellStyle = stringCenterBlack_12;
                cell.SetCellValue("結束日期");
                cell = sheet.GetRow(nextStartIndex + 1).GetCell(2);
                cell.CellStyle = stringCenterBlack_12;
                cell.SetCellValue("原籍部門");
                cell = sheet.GetRow(nextStartIndex + 1).GetCell(3);
                cell.CellStyle = stringCenterBlack_12;
                cell.SetCellValue("受入單位");
                sheet.AddMergedRegion(new CellRangeAddress(nextStartIndex + 1, nextStartIndex + 1, 3, 4));
                cell = sheet.GetRow(nextStartIndex + 1).GetCell(5);
                cell.CellStyle = stringCenterBlack_12;
                cell.SetCellValue("研修目的");
                sheet.AddMergedRegion(new CellRangeAddress(nextStartIndex + 1, nextStartIndex + 1, 5, 6));

                DataTable TrainData = dao.getTrainData(emp);
                rowNum = nextStartIndex + 2;
                for (int i = 0; i < TrainData.Rows.Count; i++)
                {
                    sheet = createRowCell(sheet, rowNum);
                    cell = sheet.GetRow(rowNum).GetCell(0);
                    cell.CellStyle = stringCenterBlue_12;
                    cell.SetCellValue(TrainData.Rows[i]["START_DT"].ToString());
                    cell = sheet.GetRow(rowNum).GetCell(1);
                    cell.CellStyle = stringCenterBlue_12;
                    cell.SetCellValue(TrainData.Rows[i]["END_DT"].ToString());
                    cell = sheet.GetRow(rowNum).GetCell(2);
                    cell.CellStyle = stringLeftBlue_12;
                    cell.SetCellValue(TrainData.Rows[i]["ORI_DEPT_FULL_NAME_2"].ToString());
                    cell = sheet.GetRow(rowNum).GetCell(3);
                    cell.CellStyle = stringLeftBlue_12;
                    cell.SetCellValue(TrainData.Rows[i]["TRAINING_COMPANY"].ToString());
                    sheet.AddMergedRegion(new CellRangeAddress(rowNum, rowNum, 3, 4));
                    cell = sheet.GetRow(rowNum).GetCell(5);
                    cell.CellStyle = stringLeftBlue_12;
                    cell.SetCellValue(TrainData.Rows[i]["TRAINING_GOAL"].ToString());
                    sheet.AddMergedRegion(new CellRangeAddress(rowNum, rowNum, 5, 6));
                    rowNum++;
                }
                nextStartIndex = rowNum + 1;//+1:表空一行
                #endregion

                #region 員工兼任履歷檔資料
                sheet = createRowCell(sheet, nextStartIndex);
                sheet = createRowCell(sheet, nextStartIndex + 1);
                cell = sheet.GetRow(nextStartIndex).GetCell(0);
                cell.CellStyle = stringLeftBlack_12_Grey;
                cell.SetCellValue("【兼任】");
                sheet.AddMergedRegion(new CellRangeAddress(nextStartIndex, nextStartIndex, 0, 6));
                cell = sheet.GetRow(nextStartIndex + 1).GetCell(0);
                cell.CellStyle = stringCenterBlack_12;
                cell.SetCellValue("開始日期");
                cell = sheet.GetRow(nextStartIndex + 1).GetCell(1);
                cell.CellStyle = stringCenterBlack_12;
                cell.SetCellValue("結束日期");
                cell = sheet.GetRow(nextStartIndex + 1).GetCell(2);
                cell.CellStyle = stringCenterBlack_12;
                cell.SetCellValue("原籍部門");
                sheet.AddMergedRegion(new CellRangeAddress(nextStartIndex + 1, nextStartIndex + 1, 2, 3));
                cell = sheet.GetRow(nextStartIndex + 1).GetCell(4);
                cell.CellStyle = stringCenterBlack_12;
                cell.SetCellValue("兼任部門");
                sheet.AddMergedRegion(new CellRangeAddress(nextStartIndex + 1, nextStartIndex + 1, 4, 5));
                cell = sheet.GetRow(nextStartIndex + 1).GetCell(6);
                cell.CellStyle = stringCenterBlack_12;
                cell.SetCellValue("職務");

                DataTable OtherJobData = dao.getOtherJobData(emp);
                rowNum = nextStartIndex + 2;
                for (int i = 0; i < OtherJobData.Rows.Count; i++)
                {
                    sheet = createRowCell(sheet, rowNum);
                    cell = sheet.GetRow(rowNum).GetCell(0);
                    cell.CellStyle = stringCenterBlue_12;
                    cell.SetCellValue(OtherJobData.Rows[i]["START_DT"].ToString());
                    cell = sheet.GetRow(rowNum).GetCell(1);
                    cell.CellStyle = stringCenterBlue_12;
                    cell.SetCellValue(OtherJobData.Rows[i]["END_DT"].ToString());
                    cell = sheet.GetRow(rowNum).GetCell(2);
                    cell.CellStyle = stringLeftBlue_12;
                    cell.SetCellValue(OtherJobData.Rows[i]["ORI_DIV_DEPT_FULL_NAME"].ToString());
                    sheet.AddMergedRegion(new CellRangeAddress(rowNum, rowNum, 2, 3));
                    cell = sheet.GetRow(rowNum).GetCell(4);
                    cell.CellStyle = stringLeftBlue_12;
                    cell.SetCellValue(OtherJobData.Rows[i]["OTHER_DEPT_NAME"].ToString());
                    sheet.AddMergedRegion(new CellRangeAddress(rowNum, rowNum, 4, 5));
                    cell = sheet.GetRow(rowNum).GetCell(6);
                    cell.CellStyle = stringLeftBlue_12;
                    cell.SetCellValue(OtherJobData.Rows[i]["OTHER_PJOB_DESC"].ToString());
                    rowNum++;
                }
                nextStartIndex = rowNum + 1;//+1:表空一行
                #endregion

                #region 員工學歷檔資料
                sheet = createRowCell(sheet, nextStartIndex);
                sheet = createRowCell(sheet, nextStartIndex + 1);
                cell = sheet.GetRow(nextStartIndex).GetCell(0);
                cell.CellStyle = stringLeftBlack_12_Grey;
                cell.SetCellValue("【學歷】");
                sheet.AddMergedRegion(new CellRangeAddress(nextStartIndex, nextStartIndex, 0, 6));
                cell = sheet.GetRow(nextStartIndex + 1).GetCell(0);
                cell.CellStyle = stringCenterBlack_12;
                cell.SetCellValue("畢業年度");
                cell = sheet.GetRow(nextStartIndex + 1).GetCell(1);
                cell.CellStyle = stringCenterBlack_12;
                cell.SetCellValue("國家別");
                cell = sheet.GetRow(nextStartIndex + 1).GetCell(2);
                cell.CellStyle = stringCenterBlack_12;
                cell.SetCellValue("教育程度");
                cell = sheet.GetRow(nextStartIndex + 1).GetCell(3);
                cell.CellStyle = stringCenterBlack_12;
                cell.SetCellValue("學校名稱");
                sheet.AddMergedRegion(new CellRangeAddress(nextStartIndex + 1, nextStartIndex + 1, 3, 4));
                cell = sheet.GetRow(nextStartIndex + 1).GetCell(5);
                cell.CellStyle = stringCenterBlack_12;
                cell.SetCellValue("科系名稱");
                sheet.AddMergedRegion(new CellRangeAddress(nextStartIndex + 1, nextStartIndex + 1, 5, 6));

                DataTable eduData = dao.getEduData(emp);
                rowNum = nextStartIndex + 2;
                for (int i = 0; i < eduData.Rows.Count; i++)
                {
                    sheet = createRowCell(sheet, rowNum);
                    cell = sheet.GetRow(rowNum).GetCell(0);
                    cell.CellStyle = stringCenterBlue_12;
                    cell.SetCellValue(eduData.Rows[i]["GRADUATION_YEAR"].ToString());
                    cell = sheet.GetRow(rowNum).GetCell(1);
                    cell.CellStyle = stringCenterBlue_12;
                    cell.SetCellValue(eduData.Rows[i]["SCHOOL_NATION_DESC"].ToString());
                    cell = sheet.GetRow(rowNum).GetCell(2);
                    cell.CellStyle = stringCenterBlue_12;
                    cell.SetCellValue(eduData.Rows[i]["EDUCATION_DESC"].ToString());
                    cell = sheet.GetRow(rowNum).GetCell(3);
                    cell.CellStyle = stringLeftBlue_12;
                    cell.SetCellValue(eduData.Rows[i]["SCHOOL_NAME"].ToString());
                    sheet.AddMergedRegion(new CellRangeAddress(rowNum, rowNum, 3, 4));
                    cell = sheet.GetRow(rowNum).GetCell(5);
                    cell.CellStyle = stringLeftBlue_12;
                    cell.SetCellValue(eduData.Rows[i]["DEPARTMENT_NAME"].ToString());
                    sheet.AddMergedRegion(new CellRangeAddress(rowNum, rowNum, 5, 6));
                    rowNum++;
                }
                nextStartIndex = rowNum + 1;//+1:表空一行

                #endregion

                #region 員工家庭成員檔資料
                sheet = createRowCell(sheet, nextStartIndex);
                sheet = createRowCell(sheet, nextStartIndex + 1);
                cell = sheet.GetRow(nextStartIndex).GetCell(0);
                cell.CellStyle = stringLeftBlack_12_Grey;
                cell.SetCellValue("【家庭狀況】");
                sheet.AddMergedRegion(new CellRangeAddress(nextStartIndex, nextStartIndex, 0, 6));
                cell = sheet.GetRow(nextStartIndex + 1).GetCell(0);
                cell.CellStyle = stringCenterBlack_12;
                cell.SetCellValue("稱謂");
                cell = sheet.GetRow(nextStartIndex + 1).GetCell(1);
                cell.CellStyle = stringCenterBlack_12;
                cell.SetCellValue("姓名");
                cell = sheet.GetRow(nextStartIndex + 1).GetCell(2);
                cell.CellStyle = stringCenterBlack_12;
                cell.SetCellValue("出生日期");
                cell = sheet.GetRow(nextStartIndex + 1).GetCell(3);
                cell.CellStyle = stringCenterBlack_12;
                cell.SetCellValue("服務機構");
                sheet.AddMergedRegion(new CellRangeAddress(nextStartIndex + 1, nextStartIndex + 1, 3, 6));

                DataTable famData = dao.getfamData(emp);
                rowNum = nextStartIndex + 2;
                for (int i = 0; i < famData.Rows.Count; i++)
                {
                    sheet = createRowCell(sheet, rowNum);
                    cell = sheet.GetRow(rowNum).GetCell(0);
                    cell.CellStyle = stringCenterBlue_12;
                    cell.SetCellValue(famData.Rows[i]["FAMILY_RELATION_DESC"].ToString());
                    cell = sheet.GetRow(rowNum).GetCell(1);
                    cell.CellStyle = stringCenterBlue_12;
                    cell.SetCellValue(famData.Rows[i]["FAMILY_NAME"].ToString().Trim());
                    cell = sheet.GetRow(rowNum).GetCell(2);
                    cell.CellStyle = stringCenterBlue_12;
                    cell.SetCellValue(famData.Rows[i]["FAMILY_BIRTH_DT"].ToString());
                    cell = sheet.GetRow(rowNum).GetCell(3);
                    cell.CellStyle = stringLeftBlue_12;
                    cell.SetCellValue(famData.Rows[i]["FAMILY_WORK_DESC"].ToString());
                    sheet.AddMergedRegion(new CellRangeAddress(rowNum, rowNum, 3, 6));
                    rowNum++;
                }
                nextStartIndex = rowNum + 1;//+1:表空一行
                #endregion

                #region 連絡資料
                sheet = createRowCell(sheet, nextStartIndex);
                sheet = createRowCell(sheet, nextStartIndex + 1);
                sheet = createRowCell(sheet, nextStartIndex + 2);
                sheet = createRowCell(sheet, nextStartIndex + 3);
                sheet = createRowCell(sheet, nextStartIndex + 4);
                sheet = createRowCell(sheet, nextStartIndex + 5);

                cell = sheet.GetRow(nextStartIndex).GetCell(0);
                cell.CellStyle = stringLeftBlack_12_Grey;
                cell.SetCellValue("【聯絡資料】");
                sheet.AddMergedRegion(new CellRangeAddress(nextStartIndex, nextStartIndex, 0, 6));
                cell = sheet.GetRow(nextStartIndex + 1).GetCell(0);
                cell.CellStyle = stringRightBlack_12;
                cell.SetCellValue("緊急聯絡人:");
                cell = sheet.GetRow(nextStartIndex + 1).GetCell(2);
                cell.CellStyle = stringRightBlack_12;
                cell.SetCellValue("緊急聯絡人關係:");
                cell = sheet.GetRow(nextStartIndex + 1).GetCell(5);
                cell.CellStyle = stringRightBlack_12;
                cell.SetCellValue("緊急聯絡人電話:");

                cell = sheet.GetRow(nextStartIndex + 2).GetCell(0);
                cell.CellStyle = stringRightBlack_12;
                cell.SetCellValue("戶籍地址:");
                cell = sheet.GetRow(nextStartIndex + 2).GetCell(5);
                cell.CellStyle = stringRightBlack_12;
                cell.SetCellValue("戶籍電話:");

                cell = sheet.GetRow(nextStartIndex + 3).GetCell(0);
                cell.CellStyle = stringRightBlack_12;
                cell.SetCellValue("通訊地址:");
                cell = sheet.GetRow(nextStartIndex + 3).GetCell(5);
                cell.CellStyle = stringRightBlack_12;
                cell.SetCellValue("通訊電話:");

                cell = sheet.GetRow(nextStartIndex + 4).GetCell(0);
                cell.CellStyle = stringRightBlack_12;
                cell.SetCellValue("公司Email:");
                cell = sheet.GetRow(nextStartIndex + 4).GetCell(5);
                cell.CellStyle = stringRightBlack_12;
                cell.SetCellValue("行動電話1:");

                cell = sheet.GetRow(nextStartIndex + 5).GetCell(0);
                cell.CellStyle = stringRightBlack_12;
                cell.SetCellValue("個人Email:");
                cell = sheet.GetRow(nextStartIndex + 5).GetCell(5);
                cell.CellStyle = stringRightBlack_12;
                cell.SetCellValue("行動電話2:");
                if (defaultData.Rows.Count > 0)
                {
                    cell = sheet.GetRow(nextStartIndex + 1).GetCell(1);
                    cell.CellStyle = stringCenterBlue_12;
                    cell.SetCellValue(defaultData.Rows[0]["URGENT_CONTACT_NAME"].ToString());
                    cell = sheet.GetRow(nextStartIndex + 1).GetCell(3);
                    cell.CellStyle = stringCenterBlue_12;
                    cell.SetCellValue(defaultData.Rows[0]["URGENT_CONTACT_RELATION"].ToString());
                    sheet.AddMergedRegion(new CellRangeAddress(nextStartIndex + 1, nextStartIndex + 1, 3, 4));
                    cell = sheet.GetRow(nextStartIndex + 1).GetCell(6);
                    cell.CellStyle = stringLeftBlue_12;
                    cell.SetCellValue(defaultData.Rows[0]["URGENT_CONTACT_TEL"].ToString().Trim());

                    cell = sheet.GetRow(nextStartIndex + 2).GetCell(1);
                    cell.CellStyle = stringLeftBlue_12;
                    cell.SetCellValue(defaultData.Rows[0]["REGISTER_ADDR"].ToString());
                    sheet.AddMergedRegion(new CellRangeAddress(nextStartIndex + 2, nextStartIndex + 2, 1, 4));
                    cell = sheet.GetRow(nextStartIndex + 2).GetCell(6);
                    cell.CellStyle = stringLeftBlue_12;
                    cell.SetCellValue(defaultData.Rows[0]["REGISTER_TEL"].ToString());

                    cell = sheet.GetRow(nextStartIndex + 3).GetCell(1);
                    cell.CellStyle = stringLeftBlue_12;
                    cell.SetCellValue(defaultData.Rows[0]["CONTACT_ADDR"].ToString());
                    sheet.AddMergedRegion(new CellRangeAddress(nextStartIndex + 3, nextStartIndex + 3, 1, 4));
                    cell = sheet.GetRow(nextStartIndex + 3).GetCell(6);
                    cell.CellStyle = stringLeftBlue_12;
                    cell.SetCellValue(defaultData.Rows[0]["CONTACT_TEL"].ToString());

                    cell = sheet.GetRow(nextStartIndex + 4).GetCell(1);
                    cell.CellStyle = stringLeftBlue_12;
                    cell.SetCellValue(defaultData.Rows[0]["COMPANY_EMAIL"].ToString());
                    sheet.AddMergedRegion(new CellRangeAddress(nextStartIndex + 4, nextStartIndex + 4, 1, 4));
                    cell = sheet.GetRow(nextStartIndex + 4).GetCell(6);
                    cell.CellStyle = stringLeftBlue_12;
                    cell.SetCellValue(defaultData.Rows[0]["MOBILE_TEL_1"].ToString());

                    cell = sheet.GetRow(nextStartIndex + 5).GetCell(1);
                    cell.CellStyle = stringLeftBlue_12;
                    cell.SetCellValue(defaultData.Rows[0]["PERSONAL_EMAIL"].ToString());
                    sheet.AddMergedRegion(new CellRangeAddress(nextStartIndex + 5, nextStartIndex + 5, 1, 4));
                    cell = sheet.GetRow(nextStartIndex + 5).GetCell(6);
                    cell.CellStyle = stringLeftBlue_12;
                    cell.SetCellValue(defaultData.Rows[0]["MOBILE_TEL_2"].ToString());

                }


                #endregion
            }

            if (is_exist == "0")
            {
                return workbook;
            }
            else
            {
                return null;
            }
        }
        catch (Exception)
        {

            throw;
        }
    }

    //調整照片大小
    public static System.Drawing.Image ResizeImage(System.Drawing.Image image, Size size, bool preserveAspectRatio = true)
    {
        int newWidth;
        int newHeight;
        if (preserveAspectRatio)
        {
            int originalWidth = image.Width;
            int originalHeight = image.Height;
            float percentWidth = (float)size.Width / (float)originalWidth;
            float percentHeight = (float)size.Height / (float)originalHeight;
            float percent = percentHeight < percentWidth ? percentHeight : percentWidth;
            newWidth = (int)(originalWidth * percent);
            newHeight = (int)(originalHeight * percent);
        }
        else
        {
            newWidth = size.Width;
            newHeight = size.Height;
        }
        System.Drawing.Image newImage = new Bitmap(newWidth, newHeight);
        using (Graphics graphicsHandle = Graphics.FromImage(newImage))
        {
            graphicsHandle.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphicsHandle.DrawImage(image, 0, 0, newWidth, newHeight);
        }
        return newImage;
    }

    //產生該row產生7個cell
    public ISheet createRowCell(ISheet sheet, int rowNum)
    {
        row_class = sheet.CreateRow(rowNum);
        for (int i = 0; i < 7; i++)
        {
            cell_class = row_class.CreateCell(i);
            cell_class.CellStyle = stringLeftBlue_12;
        }

        return sheet;
    }


    /// <summary>
    /// 設定資料的格式(預設值:字型大小12，黑色字8, 藍色字62)
    /// </summary>
    /// <param name="workbook"></param>
    /// <param name="align">文字位置</param>
    /// <param name="isBorder">是否要有邊框</param>
    /// <returns></returns>
    private ICellStyle setCellStyle(IWorkbook workbook, string align, bool isBorder)
    {
        return setCellStyle(workbook, align, isBorder, 0, 12, 8);
    }

    /// <summary>
    /// 設定資料的格式
    /// </summary>
    /// <param name="workbook"></param>
    /// <param name="align">文字位置</param>
    /// <param name="isBorder">是否要有邊框</param>
    /// <param name="bgColorCD">背景顏色設定(10:紅,13:黃,14:pink....GREY_25_PERCENT(淺灰色):22)</param>
    /// <param name="fontSize">字型大小</param>
    /// <param name="fontColor">字型顏色(8:黑色，62:INDIGO,13:黃,14:pink.... )</param>
    /// <returns></returns>
    private ICellStyle setCellStyle(IWorkbook workbook, string align, bool isBorder, int bgColorCD, short fontSize, short fontColor)
    {
        ICellStyle style_class = workbook.CreateCellStyle();


        //資料的字型
        IFont cellFont = workbook.CreateFont();
        cellFont.FontName = "微軟正黑體";
        cellFont.FontHeightInPoints = fontSize;  //字型大小
        cellFont.Color = fontColor;   //字型顏色
        cellFont.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Normal;   //bold:粗體字
        style_class.SetFont(cellFont);

        //是否要有邊框
        if (isBorder)
        {
            //style.BottomBorderColor = HSSFColor.White.Index;
            style_class.BorderBottom = BorderStyle.Thin;
            style_class.BorderTop = BorderStyle.Thin;
            style_class.BorderLeft = BorderStyle.Thin;
            style_class.BorderRight = BorderStyle.Thin;
        }

        //文字位置 (預設靠左)
        if (align.ToLower() == "center")
        {
            style_class.Alignment = HorizontalAlignment.Center;
        }
        else if (align.ToLower() == "right")
        {
            style_class.Alignment = HorizontalAlignment.Right;
        }
        else
        {
            style_class.Alignment = HorizontalAlignment.Left;
        }

        //背景顏色
        if (bgColorCD > 0)
        {
            style_class.FillForegroundColor = (short)bgColorCD;
            style_class.FillPattern = FillPattern.SolidForeground;
            //style.FillBackgroundColor = HSSFColor.Yellow.Index;
        }
        return style_class;
    }


    #endregion


    //舊的-已棄用
    public IWorkbook ExportExcel(string emp_id)
    {
        try
        {
            string is_exist = "";
            string path = "";
            //Excel初始化
            IWorkbook workbook;
            ISheet sheet;
            ICellStyle style1;
            ICellStyle style2;
            ICellStyle style3;
            workbook = new XSSFWorkbook();

            CFB2HB0600DAO dao = new CFB2HB0600DAO();
            List<string> List_emp_id = emp_id.Split(',').ToList();
            //產生Excel
            foreach (var emp in List_emp_id)
            {
                sheet = workbook.CreateSheet(emp);
                //欄位名稱
                style1 = (XSSFCellStyle)workbook.CreateCellStyle();
                ((XSSFCellStyle)style1).SetFillForegroundColor(new XSSFColor(Color.LightGray));
                ((XSSFCellStyle)style1).FillPattern = FillPattern.SolidForeground;
                //style1.BorderBottom = BorderStyle.Thin;
                //style1.BorderTop = BorderStyle.Thin;
                //style1.BorderLeft = BorderStyle.Thin;
                //style1.BorderRight = BorderStyle.Thin;
                IFont font1 = workbook.CreateFont();
                font1.FontName = "新細明體";
                font1.FontHeightInPoints = 12;
                style1.SetFont(font1);
                //欄位資料
                style2 = workbook.CreateCellStyle();
                style2.SetFont(font1);
                //style2.BorderBottom = BorderStyle.Thin;
                //style2.BorderTop = BorderStyle.Thin;
                //style2.BorderLeft = BorderStyle.Thin;
                //style2.BorderRight = BorderStyle.Thin;

                style3 = (XSSFCellStyle)workbook.CreateCellStyle();
                ((XSSFCellStyle)style3).SetFillForegroundColor(new XSSFColor(Color.LightGray));
                ((XSSFCellStyle)style3).FillPattern = FillPattern.SolidForeground;
                style3.SetFont(font1);
                style3.Alignment = HorizontalAlignment.Center;
                style3.VerticalAlignment = VerticalAlignment.Center;
                //style3.BorderBottom = BorderStyle.Thin;
                //style3.BorderTop = BorderStyle.Thin;
                //style3.BorderLeft = BorderStyle.Thin;
                //style3.BorderRight = BorderStyle.Thin;


                #region 取得基本資料
                //取得基本資料
                DataTable defaultData = dao.getDefaultData(emp);
                if (defaultData.Rows.Count > 0)
                {
                    //第一行
                    IRow row = sheet.CreateRow(2);
                    ICell cell;
                    cell = row.CreateCell(1);
                    cell.CellStyle = style1;
                    cell.SetCellValue("工號:");
                    cell = row.CreateCell(2);
                    cell.CellStyle = style1;
                    sheet.AddMergedRegion(new CellRangeAddress(2, 2, 1, 2));

                    cell = row.CreateCell(3);
                    cell.CellStyle = style2;
                    cell.SetCellValue(defaultData.Rows[0]["EMP_ID"].ToString());
                    cell = row.CreateCell(4);
                    cell.CellStyle = style2;
                    sheet.AddMergedRegion(new CellRangeAddress(2, 2, 3, 4));

                    cell = row.CreateCell(5);
                    cell.CellStyle = style1;
                    cell.SetCellValue("姓名:");
                    cell = row.CreateCell(6);
                    cell.CellStyle = style1;
                    sheet.AddMergedRegion(new CellRangeAddress(2, 2, 5, 6));

                    cell = row.CreateCell(7);
                    cell.CellStyle = style2;
                    cell.SetCellValue(defaultData.Rows[0]["EMP_NAME"].ToString());
                    cell = row.CreateCell(8);
                    cell.CellStyle = style2;
                    sheet.AddMergedRegion(new CellRangeAddress(2, 2, 7, 8));

                    cell = row.CreateCell(9);
                    cell.CellStyle = style1;
                    cell.SetCellValue("在職區分:");
                    cell = row.CreateCell(10);
                    cell.CellStyle = style1;
                    sheet.AddMergedRegion(new CellRangeAddress(2, 2, 9, 10));

                    cell = row.CreateCell(11);
                    cell.CellStyle = style2;
                    cell.SetCellValue(defaultData.Rows[0]["EMP_CHG_DESC"].ToString());
                    cell = row.CreateCell(12);
                    cell.CellStyle = style2;
                    sheet.AddMergedRegion(new CellRangeAddress(2, 2, 11, 12));

                    //第二行
                    row = sheet.CreateRow(3);
                    cell = row.CreateCell(1);
                    cell.CellStyle = style1;
                    cell.SetCellValue("資格:");
                    cell = row.CreateCell(2);
                    cell.CellStyle = style1;
                    sheet.AddMergedRegion(new CellRangeAddress(3, 3, 1, 2));

                    cell = row.CreateCell(3);
                    cell.CellStyle = style2;
                    cell.SetCellValue(defaultData.Rows[0]["LEVEL_CD"].ToString());
                    cell = row.CreateCell(4);
                    cell.CellStyle = style2;
                    sheet.AddMergedRegion(new CellRangeAddress(3, 3, 3, 4));

                    cell = row.CreateCell(5);
                    cell.CellStyle = style1;
                    cell.SetCellValue("職務:");
                    cell = row.CreateCell(6);
                    cell.CellStyle = style1;
                    sheet.AddMergedRegion(new CellRangeAddress(3, 3, 5, 6));

                    cell = row.CreateCell(7);
                    cell.CellStyle = style2;
                    cell.SetCellValue(defaultData.Rows[0]["PJOB_DESC"].ToString());
                    cell = row.CreateCell(8);
                    cell.CellStyle = style2;
                    sheet.AddMergedRegion(new CellRangeAddress(3, 3, 7, 8));

                    //第三行
                    row = sheet.CreateRow(4);
                    cell = row.CreateCell(1);
                    cell.CellStyle = style1;
                    cell.SetCellValue("廠區:");
                    cell = row.CreateCell(2);
                    cell.CellStyle = style1;
                    sheet.AddMergedRegion(new CellRangeAddress(4, 4, 1, 2));

                    cell = row.CreateCell(3);
                    cell.CellStyle = style2;
                    cell.SetCellValue(defaultData.Rows[0]["PLANT_NAME"].ToString());
                    cell = row.CreateCell(4);
                    cell.CellStyle = style2;
                    sheet.AddMergedRegion(new CellRangeAddress(4, 4, 3, 4));

                    cell = row.CreateCell(5);
                    cell.CellStyle = style1;
                    cell.SetCellValue("部門:");
                    cell = row.CreateCell(6);
                    cell.CellStyle = style1;
                    sheet.AddMergedRegion(new CellRangeAddress(4, 4, 5, 6));

                    cell = row.CreateCell(7);
                    cell.CellStyle = style2;
                    cell.SetCellValue(defaultData.Rows[0]["DEPT_NAME"].ToString());
                    cell = row.CreateCell(8);
                    cell.CellStyle = style2;
                    cell = row.CreateCell(9);
                    cell.CellStyle = style2;
                    sheet.AddMergedRegion(new CellRangeAddress(4, 4, 7, 9));

                    //第四行
                    row = sheet.CreateRow(5);
                    cell = row.CreateCell(1);
                    cell.CellStyle = style1;
                    cell.SetCellValue("入社日期:");
                    cell = row.CreateCell(2);
                    cell.CellStyle = style1;
                    sheet.AddMergedRegion(new CellRangeAddress(5, 5, 1, 2));

                    cell = row.CreateCell(3);
                    cell.CellStyle = style2;
                    cell.SetCellValue(defaultData.Rows[0]["JOIN_DT"].ToString());
                    cell = row.CreateCell(4);
                    cell.CellStyle = style2;
                    sheet.AddMergedRegion(new CellRangeAddress(5, 5, 3, 4));

                    cell = row.CreateCell(5);
                    cell.CellStyle = style1;
                    cell.SetCellValue("職種:");
                    cell = row.CreateCell(6);
                    cell.CellStyle = style1;
                    sheet.AddMergedRegion(new CellRangeAddress(5, 5, 5, 6));

                    cell = row.CreateCell(7);
                    cell.CellStyle = style2;
                    cell.SetCellValue(defaultData.Rows[0]["WS_DESC"].ToString());
                    cell = row.CreateCell(8);
                    cell.CellStyle = style1;
                    sheet.AddMergedRegion(new CellRangeAddress(5, 5, 7, 8));

                    cell = row.CreateCell(9);
                    cell.CellStyle = style1;
                    cell.SetCellValue("性別:");
                    cell = row.CreateCell(10);
                    cell.CellStyle = style1;
                    sheet.AddMergedRegion(new CellRangeAddress(5, 5, 9, 10));

                    cell = row.CreateCell(11);
                    cell.CellStyle = style2;
                    cell.SetCellValue(defaultData.Rows[0]["SEX_CD"].ToString());
                    cell = row.CreateCell(12);
                    cell.CellStyle = style2;
                    sheet.AddMergedRegion(new CellRangeAddress(5, 5, 11, 12));

                    //第五行
                    row = sheet.CreateRow(6);
                    cell = row.CreateCell(1);
                    cell.CellStyle = style1;
                    cell.SetCellValue("入社年資:");
                    cell = row.CreateCell(2);
                    cell.CellStyle = style1;
                    sheet.AddMergedRegion(new CellRangeAddress(6, 6, 1, 2));

                    cell = row.CreateCell(3);
                    cell.CellStyle = style2;
                    cell.SetCellValue(defaultData.Rows[0]["WORK_YEARS"].ToString());
                    cell = row.CreateCell(4);
                    cell.CellStyle = style2;
                    sheet.AddMergedRegion(new CellRangeAddress(6, 6, 3, 4));

                    cell = row.CreateCell(5);
                    cell.CellStyle = style1;
                    cell.SetCellValue("模範員工年度:");
                    cell = row.CreateCell(6);
                    cell.CellStyle = style1;
                    sheet.AddMergedRegion(new CellRangeAddress(6, 6, 5, 6));

                    cell = row.CreateCell(7);
                    cell.CellStyle = style2;
                    cell.SetCellValue(defaultData.Rows[0]["MODEL_YEAR"].ToString());
                    cell = row.CreateCell(8);
                    cell.CellStyle = style1;
                    sheet.AddMergedRegion(new CellRangeAddress(6, 6, 7, 8));

                    cell = row.CreateCell(9);
                    cell.CellStyle = style1;
                    cell.SetCellValue("出生年月日:");
                    cell = row.CreateCell(10);
                    cell.CellStyle = style1;
                    sheet.AddMergedRegion(new CellRangeAddress(6, 6, 9, 10));

                    cell = row.CreateCell(11);
                    cell.CellStyle = style2;
                    cell.SetCellValue(defaultData.Rows[0]["BIRTH_DT"].ToString());
                    cell = row.CreateCell(12);
                    cell.CellStyle = style2;
                    sheet.AddMergedRegion(new CellRangeAddress(6, 6, 11, 12));

                    //第六行
                    row = sheet.CreateRow(7);
                    cell = row.CreateCell(1);
                    cell.CellStyle = style1;
                    cell.SetCellValue("資格年資:");
                    cell = row.CreateCell(2);
                    cell.CellStyle = style1;
                    sheet.AddMergedRegion(new CellRangeAddress(7, 7, 1, 2));

                    cell = row.CreateCell(3);
                    cell.CellStyle = style2;
                    cell.SetCellValue(defaultData.Rows[0]["RECENT_LEVEL_WORK_DAYS"].ToString());
                    cell = row.CreateCell(4);
                    cell.CellStyle = style2;
                    sheet.AddMergedRegion(new CellRangeAddress(7, 7, 3, 4));

                    cell = row.CreateCell(5);
                    cell.CellStyle = style1;
                    cell.SetCellValue("基本薪資:");
                    cell = row.CreateCell(6);
                    cell.CellStyle = style1;
                    sheet.AddMergedRegion(new CellRangeAddress(7, 7, 5, 6));

                    cell = row.CreateCell(7);
                    cell.CellStyle = style2;
                    cell.SetCellValue(defaultData.Rows[0]["BASE_SALARY"].ToString());
                    cell = row.CreateCell(8);
                    cell.CellStyle = style1;
                    sheet.AddMergedRegion(new CellRangeAddress(7, 7, 7, 8));

                    cell = row.CreateCell(9);
                    cell.CellStyle = style1;
                    cell.SetCellValue("年齡:");
                    cell = row.CreateCell(10);
                    cell.CellStyle = style1;
                    sheet.AddMergedRegion(new CellRangeAddress(7, 7, 9, 10));

                    cell = row.CreateCell(11);
                    cell.CellStyle = style2;
                    cell.SetCellValue(defaultData.Rows[0]["AGE"].ToString());
                    cell = row.CreateCell(12);
                    cell.CellStyle = style2;
                    sheet.AddMergedRegion(new CellRangeAddress(7, 7, 11, 12));

                    path = defaultData.Rows[0]["PHOTO_PATH"].ToString();
                    //圖片
                    var patriarch = sheet.CreateDrawingPatriarch();
                    int pictureIndex = 0;
                    if (File.Exists(path + emp_id + ".jpg"))
                    {
                        using (FileStream fs = new FileStream(path + emp_id + ".jpg", FileMode.Open))
                        {
                            byte[] buffer = new byte[16 * 1024];
                            using (MemoryStream ms = new MemoryStream())
                            {
                                int read;
                                while ((read = fs.Read(buffer, 0, buffer.Length)) > 0)
                                {
                                    ms.Write(buffer, 0, read);
                                }
                                pictureIndex = workbook.AddPicture(ms.ToArray(), PictureType.JPEG);

                                // 將縮圖定位到 worksheet 中
                                var anchor = new XSSFClientAnchor(0, 0, 0, 0, row.Cells.Count + 2, row.RowNum - 5, 0, 0);
                                var picture = patriarch.CreatePicture(anchor, pictureIndex);
                                //var size = picture.GetPreferredSize();
                                //row.HeightInPoints = size.Row1;
                                picture.Resize();

                                // 為了不讓圖片壓線，必須讓圖片有一點位移，你可以把它移除掉看看會產生什麼情況
                                // (我得承認這裡是程式中的魔術數字 Orz，但是一時找不到更好的方法)
                                anchor.Dx1 = 5;
                                anchor.Dy1 = 2;


                            }
                            fs.Close();
                        }
                    }
                    is_exist = "0";
                }
                #endregion

                #region 考績主檔資料

                //考績主檔資料
                IRow rowAssess = sheet.CreateRow(9);
                rowAssess = sheet.CreateRow(10);
                ICell cellAssess;
                cellAssess = rowAssess.CreateCell(1);
                cellAssess.CellStyle = style2;
                cellAssess.SetCellValue("【考核】");
                rowAssess = sheet.CreateRow(11);
                DataTable AssessData = dao.getAssessData(emp);
                if (AssessData.Rows.Count > 0)
                {
                    cellAssess = rowAssess.CreateCell(1);
                    cellAssess.CellStyle = style1;
                    cellAssess.SetCellValue("序號");
                    cellAssess = rowAssess.CreateCell(2);
                    cellAssess.CellStyle = style1;
                    cellAssess.SetCellValue("考核類別");
                    int assesscell = 3;
                    for (int i = 0; i < AssessData.Rows.Count; i++)
                    {
                        cellAssess = rowAssess.CreateCell(assesscell);
                        cellAssess.CellStyle = style1;
                        cellAssess.SetCellValue(AssessData.Rows[i]["ASSESS_YEAR"].ToString());
                        assesscell++;

                    }
                    rowAssess = sheet.CreateRow(12);
                    cellAssess = rowAssess.CreateCell(1);
                    cellAssess.CellStyle = style2;
                    cellAssess.SetCellValue("1");
                    cellAssess = rowAssess.CreateCell(2);
                    cellAssess.CellStyle = style2;
                    cellAssess.SetCellValue("能力考核");
                    assesscell = 3;
                    for (int i = 0; i < AssessData.Rows.Count; i++)
                    {
                        //rowAssess = sheet.CreateRow(12);
                        cellAssess = rowAssess.CreateCell(assesscell);
                        cellAssess.CellStyle = style2;
                        cellAssess.SetCellValue(AssessData.Rows[i]["SCORE_1H"].ToString());
                        assesscell++;
                    }

                    rowAssess = sheet.CreateRow(13);
                    cellAssess = rowAssess.CreateCell(1);
                    cellAssess.CellStyle = style2;
                    cellAssess.SetCellValue("2");
                    cellAssess = rowAssess.CreateCell(2);
                    cellAssess.CellStyle = style2;
                    cellAssess.SetCellValue("業績考核");
                    assesscell = 3;
                    for (int i = 0; i < AssessData.Rows.Count; i++)
                    {
                        //rowAssess = sheet.CreateRow(12);
                        cellAssess = rowAssess.CreateCell(assesscell);
                        cellAssess.CellStyle = style2;
                        cellAssess.SetCellValue(AssessData.Rows[i]["SCORE_2H"].ToString());
                        assesscell++;
                    }
                    //Label lb_SCORE_TYPE = (Label)this.Page.FindControl("lb_YEAR" + (i + 1).ToString() + "_SCOREH1");
                    //if (lb_SCORE_TYPE != null)
                    //    lb_SCORE_TYPE.Text = data.Rows[i]["SCORE_1H"].ToString();

                    //Label lb_SCORE_TYPE2 = (Label)this.Page.FindControl("lb_YEAR" + (i + 1).ToString() + "_SCOREH2");
                    //if (lb_SCORE_TYPE2 != null)
                    //    lb_SCORE_TYPE2.Text = data.Rows[i]["SCORE_2H"].ToString();

                    is_exist = "0";
                }
                #endregion

                #region 連絡資料

                //連絡資料
                IRow rowContact = sheet.CreateRow(16);
                rowContact = sheet.CreateRow(17);
                ICell cellContact;
                cellContact = rowContact.CreateCell(1);
                cellContact.CellStyle = style2;
                cellContact.SetCellValue("【連絡資料】");
                cellContact = rowContact.CreateCell(2);
                sheet.AddMergedRegion(new CellRangeAddress(17, 17, 1, 2));
                if (defaultData.Rows.Count > 0)
                {
                    rowContact = sheet.CreateRow(18);
                    cellContact = rowContact.CreateCell(1);
                    cellContact.CellStyle = style1;
                    cellContact.SetCellValue("緊急聯絡人:");
                    cellContact = rowContact.CreateCell(2);
                    cellContact.CellStyle = style1;
                    sheet.AddMergedRegion(new CellRangeAddress(18, 18, 1, 2));

                    cellContact = rowContact.CreateCell(3);
                    cellContact.CellStyle = style2;
                    cellContact.SetCellValue(defaultData.Rows[0]["URGENT_CONTACT_NAME"].ToString());
                    cellContact = rowContact.CreateCell(4);
                    cellContact.CellStyle = style2;
                    sheet.AddMergedRegion(new CellRangeAddress(18, 18, 3, 4));

                    rowContact = sheet.CreateRow(19);
                    cellContact = rowContact.CreateCell(1);
                    cellContact.CellStyle = style1;
                    cellContact.SetCellValue("緊急聯絡人電話:");
                    cellContact = rowContact.CreateCell(2);
                    cellContact.CellStyle = style1;
                    sheet.AddMergedRegion(new CellRangeAddress(19, 19, 1, 2));

                    cellContact = rowContact.CreateCell(3);
                    cellContact.CellStyle = style2;
                    cellContact.SetCellValue(defaultData.Rows[0]["URGENT_CONTACT_TEL"].ToString());
                    cellContact = rowContact.CreateCell(4);
                    cellContact.CellStyle = style2;
                    sheet.AddMergedRegion(new CellRangeAddress(19, 19, 3, 4));


                    rowContact = sheet.CreateRow(20);
                    cellContact = rowContact.CreateCell(1);
                    cellContact.CellStyle = style1;
                    cellContact.SetCellValue("緊急聯絡人關係:");
                    cellContact = rowContact.CreateCell(2);
                    cellContact.CellStyle = style1;
                    sheet.AddMergedRegion(new CellRangeAddress(20, 20, 1, 2));

                    cellContact = rowContact.CreateCell(3);
                    cellContact.CellStyle = style2;
                    cellContact.SetCellValue(defaultData.Rows[0]["URGENT_CONTACT_RELATION"].ToString());
                    cellContact = rowContact.CreateCell(4);
                    cellContact.CellStyle = style2;
                    sheet.AddMergedRegion(new CellRangeAddress(20, 20, 3, 4));

                    rowContact = sheet.CreateRow(21);

                    rowContact = sheet.CreateRow(22);
                    cellContact = rowContact.CreateCell(1);
                    cellContact.CellStyle = style1;
                    cellContact.SetCellValue("戶籍地址:");
                    cellContact = rowContact.CreateCell(2);
                    cellContact.CellStyle = style1;
                    sheet.AddMergedRegion(new CellRangeAddress(22, 22, 1, 2));

                    cellContact = rowContact.CreateCell(3);
                    cellContact.CellStyle = style2;
                    cellContact.SetCellValue(defaultData.Rows[0]["REGISTER_ADDR"].ToString());
                    cellContact = rowContact.CreateCell(4);
                    cellContact.CellStyle = style2;
                    cellContact = rowContact.CreateCell(5);
                    cellContact.CellStyle = style2;
                    cellContact = rowContact.CreateCell(6);
                    cellContact.CellStyle = style2;
                    cellContact = rowContact.CreateCell(7);
                    cellContact.CellStyle = style2;
                    cellContact = rowContact.CreateCell(8);
                    cellContact.CellStyle = style2;
                    sheet.AddMergedRegion(new CellRangeAddress(22, 22, 3, 8));

                    cellContact = rowContact.CreateCell(9);
                    cellContact.CellStyle = style1;
                    cellContact.SetCellValue("戶籍電話:");
                    cellContact = rowContact.CreateCell(10);
                    cellContact.CellStyle = style1;
                    sheet.AddMergedRegion(new CellRangeAddress(22, 22, 9, 10));

                    cellContact = rowContact.CreateCell(11);
                    cellContact.CellStyle = style2;
                    cellContact.SetCellValue(defaultData.Rows[0]["REGISTER_TEL"].ToString());
                    cellContact = rowContact.CreateCell(12);
                    cellContact.CellStyle = style2;
                    sheet.AddMergedRegion(new CellRangeAddress(22, 22, 11, 12));


                    rowContact = sheet.CreateRow(23);
                    cellContact = rowContact.CreateCell(1);
                    cellContact.CellStyle = style1;
                    cellContact.SetCellValue("通訊地址:");
                    cellContact = rowContact.CreateCell(2);
                    cellContact.CellStyle = style1;
                    sheet.AddMergedRegion(new CellRangeAddress(23, 23, 1, 2));

                    cellContact = rowContact.CreateCell(3);
                    cellContact.CellStyle = style2;
                    cellContact.SetCellValue(defaultData.Rows[0]["CONTACT_ADDR"].ToString());
                    cellContact = rowContact.CreateCell(4);
                    cellContact.CellStyle = style2;
                    cellContact = rowContact.CreateCell(5);
                    cellContact.CellStyle = style2;
                    cellContact = rowContact.CreateCell(6);
                    cellContact.CellStyle = style2;
                    cellContact = rowContact.CreateCell(7);
                    cellContact.CellStyle = style2;
                    cellContact = rowContact.CreateCell(8);
                    cellContact.CellStyle = style2;
                    sheet.AddMergedRegion(new CellRangeAddress(23, 23, 3, 8));

                    cellContact = rowContact.CreateCell(9);
                    cellContact.CellStyle = style1;
                    cellContact.SetCellValue("通訊電話:");
                    cellContact = rowContact.CreateCell(10);
                    cellContact.CellStyle = style1;
                    sheet.AddMergedRegion(new CellRangeAddress(23, 23, 9, 10));

                    cellContact = rowContact.CreateCell(11);
                    cellContact.CellStyle = style2;
                    cellContact.SetCellValue(defaultData.Rows[0]["CONTACT_TEL"].ToString());
                    cellContact = rowContact.CreateCell(12);
                    cellContact.CellStyle = style2;
                    sheet.AddMergedRegion(new CellRangeAddress(23, 23, 11, 12));


                    rowContact = sheet.CreateRow(24);
                    cellContact = rowContact.CreateCell(1);
                    cellContact.CellStyle = style1;
                    cellContact.SetCellValue("行動電話一:");
                    cellContact = rowContact.CreateCell(2);
                    cellContact.CellStyle = style1;
                    sheet.AddMergedRegion(new CellRangeAddress(24, 24, 1, 2));

                    cellContact = rowContact.CreateCell(3);
                    cellContact.CellStyle = style2;
                    cellContact.SetCellValue(defaultData.Rows[0]["MOBILE_TEL_1"].ToString());
                    cellContact = rowContact.CreateCell(4);
                    cellContact.CellStyle = style2;
                    sheet.AddMergedRegion(new CellRangeAddress(24, 24, 3, 4));

                    cellContact = rowContact.CreateCell(5);
                    cellContact.CellStyle = style1;
                    cellContact.SetCellValue("行動電話二:");
                    cellContact = rowContact.CreateCell(6);
                    cellContact.CellStyle = style1;
                    sheet.AddMergedRegion(new CellRangeAddress(24, 24, 5, 6));


                    cellContact = rowContact.CreateCell(7);
                    cellContact.CellStyle = style2;
                    cellContact.SetCellValue(defaultData.Rows[0]["MOBILE_TEL_2"].ToString());
                    cellContact = rowContact.CreateCell(8);
                    cellContact.CellStyle = style2;
                    sheet.AddMergedRegion(new CellRangeAddress(24, 24, 7, 8));

                    cellContact = rowContact.CreateCell(9);
                    cellContact.CellStyle = style1;
                    cellContact.SetCellValue("公司分機:");
                    cellContact = rowContact.CreateCell(10);
                    cellContact.CellStyle = style1;
                    sheet.AddMergedRegion(new CellRangeAddress(24, 24, 9, 10));

                    cellContact = rowContact.CreateCell(11);
                    cellContact.CellStyle = style2;
                    cellContact.SetCellValue(defaultData.Rows[0]["COMPANY_EXT"].ToString());
                    cellContact = rowContact.CreateCell(12);
                    cellContact.CellStyle = style2;
                    sheet.AddMergedRegion(new CellRangeAddress(24, 24, 11, 12));

                    rowContact = sheet.CreateRow(25);
                    cellContact = rowContact.CreateCell(1);
                    cellContact.CellStyle = style1;
                    cellContact.SetCellValue("個人Email:");
                    cellContact = rowContact.CreateCell(2);
                    cellContact.CellStyle = style1;
                    sheet.AddMergedRegion(new CellRangeAddress(25, 25, 1, 2));

                    cellContact = rowContact.CreateCell(3);
                    cellContact.CellStyle = style2;
                    cellContact.SetCellValue(defaultData.Rows[0]["PERSONAL_EMAIL"].ToString());
                    cellContact = rowContact.CreateCell(4);
                    cellContact.CellStyle = style2;
                    sheet.AddMergedRegion(new CellRangeAddress(25, 25, 3, 4));

                    cellContact = rowContact.CreateCell(5);
                    cellContact.CellStyle = style1;
                    cellContact.SetCellValue("公司Email:");
                    cellContact = rowContact.CreateCell(6);
                    cellContact.CellStyle = style1;
                    sheet.AddMergedRegion(new CellRangeAddress(25, 25, 5, 6));


                    cellContact = rowContact.CreateCell(7);
                    cellContact.CellStyle = style2;
                    cellContact.SetCellValue(defaultData.Rows[0]["COMPANY_EMAIL"].ToString());
                    cellContact = rowContact.CreateCell(8);
                    cellContact.CellStyle = style2;
                    sheet.AddMergedRegion(new CellRangeAddress(25, 25, 7, 8));

                    is_exist = "0";
                }
                #endregion

                int GridRow = 28;
                #region 員工學歷檔資料

                //員工學歷檔資料
                IRow rowEdu = sheet.CreateRow(28);
                rowEdu = sheet.CreateRow(29);
                ICell cellEdu;
                cellEdu = rowEdu.CreateCell(1);
                cellEdu.CellStyle = style2;
                cellEdu.SetCellValue("【學歷】");
                rowEdu = sheet.CreateRow(30);
                DataTable eduData = dao.getEduData(emp);
                if (eduData.Rows.Count > 0)
                {
                    rowEdu = sheet.CreateRow(31);
                    cellEdu = rowEdu.CreateCell(1);
                    cellEdu.CellStyle = style1;
                    cellEdu.SetCellValue("序號");
                    cellEdu = rowEdu.CreateCell(2);
                    cellEdu.CellStyle = style1;
                    sheet.AddMergedRegion(new CellRangeAddress(31, 31, 1, 2));

                    cellEdu = rowEdu.CreateCell(3);
                    cellEdu.CellStyle = style1;
                    cellEdu.SetCellValue("國家別");
                    cellEdu = rowEdu.CreateCell(4);
                    cellEdu.CellStyle = style1;
                    sheet.AddMergedRegion(new CellRangeAddress(31, 31, 3, 4));

                    cellEdu = rowEdu.CreateCell(5);
                    cellEdu.CellStyle = style1;
                    cellEdu.SetCellValue("教育程度");
                    cellEdu = rowEdu.CreateCell(6);
                    cellEdu.CellStyle = style1;
                    sheet.AddMergedRegion(new CellRangeAddress(31, 31, 5, 6));

                    cellEdu = rowEdu.CreateCell(7);
                    cellEdu.CellStyle = style1;
                    cellEdu.SetCellValue("學校");
                    cellEdu = rowEdu.CreateCell(8);
                    cellEdu.CellStyle = style1;
                    sheet.AddMergedRegion(new CellRangeAddress(31, 31, 7, 8));

                    cellEdu = rowEdu.CreateCell(9);
                    cellEdu.CellStyle = style1;
                    cellEdu.SetCellValue("科系");
                    cellEdu = rowEdu.CreateCell(10);
                    cellEdu.CellStyle = style1;
                    sheet.AddMergedRegion(new CellRangeAddress(31, 31, 9, 10));

                    cellEdu = rowEdu.CreateCell(11);
                    cellEdu.CellStyle = style1;
                    cellEdu.SetCellValue("畢業年度");
                    cellEdu = rowEdu.CreateCell(12);
                    cellEdu.CellStyle = style1;
                    sheet.AddMergedRegion(new CellRangeAddress(31, 31, 11, 12));

                    GridRow = 32;


                    for (int i = 0; i < eduData.Rows.Count; i++)
                    {
                        rowEdu = sheet.CreateRow(GridRow);
                        cellEdu = rowEdu.CreateCell(1);
                        cellEdu.CellStyle = style2;
                        cellEdu.SetCellValue(eduData.Rows[i]["RowNumber"].ToString());
                        cellEdu = rowEdu.CreateCell(2);
                        cellEdu.CellStyle = style2;
                        sheet.AddMergedRegion(new CellRangeAddress(GridRow, GridRow, 1, 2));

                        cellEdu = rowEdu.CreateCell(3);
                        cellEdu.CellStyle = style2;
                        cellEdu.SetCellValue(eduData.Rows[i]["SCHOOL_NATION_DESC"].ToString());
                        cellEdu = rowEdu.CreateCell(4);
                        cellEdu.CellStyle = style2;
                        sheet.AddMergedRegion(new CellRangeAddress(GridRow, GridRow, 3, 4));

                        cellEdu = rowEdu.CreateCell(5);
                        cellEdu.CellStyle = style2;
                        cellEdu.SetCellValue(eduData.Rows[i]["EDUCATION_DESC"].ToString());
                        cellEdu = rowEdu.CreateCell(6);
                        cellEdu.CellStyle = style2;
                        sheet.AddMergedRegion(new CellRangeAddress(GridRow, GridRow, 5, 6));

                        cellEdu = rowEdu.CreateCell(7);
                        cellEdu.CellStyle = style2;
                        cellEdu.SetCellValue(eduData.Rows[i]["SCHOOL_NAME"].ToString());
                        cellEdu = rowEdu.CreateCell(8);
                        cellEdu.CellStyle = style2;
                        sheet.AddMergedRegion(new CellRangeAddress(GridRow, GridRow, 7, 8));

                        cellEdu = rowEdu.CreateCell(9);
                        cellEdu.CellStyle = style2;
                        cellEdu.SetCellValue(eduData.Rows[i]["DEPARTMENT_NAME"].ToString());
                        cellEdu = rowEdu.CreateCell(10);
                        cellEdu.CellStyle = style2;
                        sheet.AddMergedRegion(new CellRangeAddress(GridRow, GridRow, 9, 10));

                        cellEdu = rowEdu.CreateCell(11);
                        cellEdu.CellStyle = style2;
                        cellEdu.SetCellValue(eduData.Rows[i]["GRADUATION_YEAR"].ToString());
                        cellEdu = rowEdu.CreateCell(12);
                        cellEdu.CellStyle = style2;
                        sheet.AddMergedRegion(new CellRangeAddress(GridRow, GridRow, 11, 12));

                        GridRow++;
                    }
                    is_exist = "0";
                }
                #endregion

                #region 員工家庭成員檔資料

                //員工家庭成員檔資料
                GridRow++;
                IRow rowFam = sheet.CreateRow(GridRow);
                GridRow++;
                rowFam = sheet.CreateRow(GridRow);
                ICell cellFam;
                cellFam = rowFam.CreateCell(1);
                cellFam.CellStyle = style2;
                cellFam.SetCellValue("【家庭情況】");
                GridRow++;
                DataTable famData = dao.getfamData(emp);
                if (famData.Rows.Count > 0)
                {
                    rowFam = sheet.CreateRow(GridRow);
                    cellFam = rowFam.CreateCell(1);
                    cellFam.CellStyle = style1;
                    cellFam.SetCellValue("序號");
                    cellFam = rowFam.CreateCell(2);
                    cellFam.CellStyle = style1;
                    sheet.AddMergedRegion(new CellRangeAddress(GridRow, GridRow, 1, 2));

                    cellFam = rowFam.CreateCell(3);
                    cellFam.CellStyle = style1;
                    cellFam.SetCellValue("稱謂");
                    cellFam = rowFam.CreateCell(4);
                    cellFam.CellStyle = style1;
                    sheet.AddMergedRegion(new CellRangeAddress(GridRow, GridRow, 3, 4));

                    cellFam = rowFam.CreateCell(5);
                    cellFam.CellStyle = style1;
                    cellFam.SetCellValue("姓名");
                    cellFam = rowFam.CreateCell(6);
                    cellFam.CellStyle = style1;
                    sheet.AddMergedRegion(new CellRangeAddress(GridRow, GridRow, 5, 6));

                    cellFam = rowFam.CreateCell(7);
                    cellFam.CellStyle = style1;
                    cellFam.SetCellValue("出生年月日");
                    cellFam = rowFam.CreateCell(8);
                    cellFam.CellStyle = style1;
                    sheet.AddMergedRegion(new CellRangeAddress(GridRow, GridRow, 7, 8));

                    cellFam = rowFam.CreateCell(9);
                    cellFam.CellStyle = style1;
                    cellFam.SetCellValue("服務機構");
                    cellFam = rowFam.CreateCell(10);
                    cellFam.CellStyle = style1;
                    cellFam = rowFam.CreateCell(11);
                    cellFam.CellStyle = style1;
                    cellFam = rowFam.CreateCell(12);
                    cellFam.CellStyle = style1;
                    sheet.AddMergedRegion(new CellRangeAddress(GridRow, GridRow, 9, 12));

                    GridRow++;


                    for (int i = 0; i < famData.Rows.Count; i++)
                    {
                        rowFam = sheet.CreateRow(GridRow);
                        cellFam = rowFam.CreateCell(1);
                        cellFam.CellStyle = style2;
                        cellFam.SetCellValue(famData.Rows[i]["RowNumber"].ToString());
                        cellFam = rowFam.CreateCell(2);
                        cellFam.CellStyle = style2;
                        sheet.AddMergedRegion(new CellRangeAddress(GridRow, GridRow, 1, 2));

                        cellFam = rowFam.CreateCell(3);
                        cellFam.CellStyle = style2;
                        cellFam.SetCellValue(famData.Rows[i]["FAMILY_RELATION_DESC"].ToString());
                        cellFam = rowFam.CreateCell(4);
                        cellFam.CellStyle = style2;
                        sheet.AddMergedRegion(new CellRangeAddress(GridRow, GridRow, 3, 4));

                        cellFam = rowFam.CreateCell(5);
                        cellFam.CellStyle = style2;
                        cellFam.SetCellValue(famData.Rows[i]["FAMILY_NAME"].ToString());
                        cellFam = rowFam.CreateCell(6);
                        cellFam.CellStyle = style2;
                        sheet.AddMergedRegion(new CellRangeAddress(GridRow, GridRow, 5, 6));

                        cellFam = rowFam.CreateCell(7);
                        cellFam.CellStyle = style2;
                        cellFam.SetCellValue(famData.Rows[i]["FAMILY_BIRTH_DT"].ToString());
                        cellFam = rowFam.CreateCell(8);
                        cellFam.CellStyle = style2;
                        sheet.AddMergedRegion(new CellRangeAddress(GridRow, GridRow, 7, 8));

                        cellFam = rowFam.CreateCell(9);
                        cellFam.CellStyle = style2;
                        cellFam.SetCellValue(famData.Rows[i]["FAMILY_WORK_DESC"].ToString());
                        cellFam = rowFam.CreateCell(10);
                        cellFam.CellStyle = style2;
                        cellFam = rowFam.CreateCell(11);
                        cellFam.CellStyle = style2;
                        cellFam = rowFam.CreateCell(12);
                        cellFam.CellStyle = style2;
                        sheet.AddMergedRegion(new CellRangeAddress(GridRow, GridRow, 9, 12));

                        GridRow++;
                    }
                    is_exist = "0";
                }
                #endregion

                #region 員工人事履歷檔資料

                //員工人事履歷檔資料
                GridRow++;
                IRow rowChg = sheet.CreateRow(GridRow);
                GridRow++;
                rowChg = sheet.CreateRow(GridRow);
                ICell cellChg;
                cellChg = rowChg.CreateCell(1);
                cellChg.CellStyle = style2;
                cellChg.SetCellValue("【人事異動履歷】");
                GridRow++;
                DataTable ChgData = dao.getChgData(emp);
                if (ChgData.Rows.Count > 0)
                {
                    rowChg = sheet.CreateRow(GridRow);
                    cellChg = rowChg.CreateCell(1);
                    cellChg.CellStyle = style1;
                    cellChg.SetCellValue("序號");
                    cellChg = rowChg.CreateCell(2);
                    cellChg.CellStyle = style1;
                    sheet.AddMergedRegion(new CellRangeAddress(GridRow, GridRow, 1, 2));

                    cellChg = rowChg.CreateCell(3);
                    cellChg.CellStyle = style1;
                    cellChg.SetCellValue("生效日期");
                    cellChg = rowChg.CreateCell(4);
                    cellChg.CellStyle = style1;
                    sheet.AddMergedRegion(new CellRangeAddress(GridRow, GridRow, 3, 4));

                    cellChg = rowChg.CreateCell(5);
                    cellChg.CellStyle = style1;
                    cellChg.SetCellValue("異動區分");
                    cellChg = rowChg.CreateCell(6);
                    cellChg.CellStyle = style1;
                    sheet.AddMergedRegion(new CellRangeAddress(GridRow, GridRow, 5, 6));

                    cellChg = rowChg.CreateCell(7);
                    cellChg.CellStyle = style1;
                    cellChg.SetCellValue("部門");
                    cellChg = rowChg.CreateCell(8);
                    cellChg.CellStyle = style1;
                    sheet.AddMergedRegion(new CellRangeAddress(GridRow, GridRow, 7, 8));

                    cellChg = rowChg.CreateCell(9);
                    cellChg.CellStyle = style1;
                    cellChg.SetCellValue("資格");
                    cellChg = rowChg.CreateCell(10);
                    cellChg.CellStyle = style1;
                    sheet.AddMergedRegion(new CellRangeAddress(GridRow, GridRow, 9, 10));
                    cellChg = rowChg.CreateCell(11);
                    cellChg.CellStyle = style1;
                    cellChg.SetCellValue("職務");
                    cellChg = rowChg.CreateCell(12);
                    cellChg.CellStyle = style1;
                    sheet.AddMergedRegion(new CellRangeAddress(GridRow, GridRow, 11, 12));

                    GridRow++;


                    for (int i = 0; i < ChgData.Rows.Count; i++)
                    {
                        rowChg = sheet.CreateRow(GridRow);
                        cellChg = rowChg.CreateCell(1);
                        cellChg.CellStyle = style2;
                        cellChg.SetCellValue(ChgData.Rows[i]["RowNumber"].ToString());
                        cellChg = rowChg.CreateCell(2);
                        cellChg.CellStyle = style2;
                        sheet.AddMergedRegion(new CellRangeAddress(GridRow, GridRow, 1, 2));

                        cellChg = rowChg.CreateCell(3);
                        cellChg.CellStyle = style2;
                        cellChg.SetCellValue(ChgData.Rows[i]["START_DT"].ToString());
                        cellChg = rowChg.CreateCell(4);
                        cellChg.CellStyle = style2;
                        sheet.AddMergedRegion(new CellRangeAddress(GridRow, GridRow, 3, 4));

                        cellChg = rowChg.CreateCell(5);
                        cellChg.CellStyle = style2;
                        cellChg.SetCellValue(ChgData.Rows[i]["HR_CHG_DESC"].ToString());
                        cellChg = rowChg.CreateCell(6);
                        cellChg.CellStyle = style2;
                        sheet.AddMergedRegion(new CellRangeAddress(GridRow, GridRow, 5, 6));

                        cellChg = rowChg.CreateCell(7);
                        cellChg.CellStyle = style2;
                        cellChg.SetCellValue(ChgData.Rows[i]["DEPT_FULL_NAME"].ToString());
                        cellChg = rowChg.CreateCell(8);
                        cellChg.CellStyle = style2;
                        sheet.AddMergedRegion(new CellRangeAddress(GridRow, GridRow, 7, 8));

                        cellChg = rowChg.CreateCell(9);
                        cellChg.CellStyle = style2;
                        cellChg.SetCellValue(ChgData.Rows[i]["LEVEL_CD"].ToString());
                        cellChg = rowChg.CreateCell(10);
                        cellChg.CellStyle = style2;
                        sheet.AddMergedRegion(new CellRangeAddress(GridRow, GridRow, 9, 10));
                        cellChg = rowChg.CreateCell(11);
                        cellChg.CellStyle = style2;
                        cellChg.SetCellValue(ChgData.Rows[i]["PJOB_DESC"].ToString());
                        cellChg = rowChg.CreateCell(12);
                        cellChg.CellStyle = style2;
                        sheet.AddMergedRegion(new CellRangeAddress(GridRow, GridRow, 11, 12));

                        GridRow++;
                    }
                    is_exist = "0";
                }
                #endregion

                #region 員工外調履歷檔資料

                //員工外調履歷檔資料
                GridRow++;
                IRow rowTrans = sheet.CreateRow(GridRow);
                GridRow++;
                rowTrans = sheet.CreateRow(GridRow);
                ICell cellTrans;
                cellTrans = rowTrans.CreateCell(1);
                cellTrans.CellStyle = style2;
                cellTrans.SetCellValue("【外調履歷】");
                GridRow++;
                DataTable TransData = dao.getTransData(emp);
                if (TransData.Rows.Count > 0)
                {
                    rowTrans = sheet.CreateRow(GridRow);

                    cellTrans = rowTrans.CreateCell(1);
                    cellTrans.CellStyle = style3;
                    cellTrans.SetCellValue("序號");
                    cellTrans = rowTrans.CreateCell(2);
                    cellTrans.CellStyle = style3;


                    cellTrans = rowTrans.CreateCell(3);
                    cellTrans.CellStyle = style3;
                    cellTrans.SetCellValue("外調類別");
                    cellTrans = rowTrans.CreateCell(4);
                    cellTrans.CellStyle = style3;


                    cellTrans = rowTrans.CreateCell(5);
                    cellTrans.CellStyle = style3;
                    cellTrans.SetCellValue("ICT類別");
                    cellTrans = rowTrans.CreateCell(6);
                    cellTrans.CellStyle = style3;


                    cellTrans = rowTrans.CreateCell(7);
                    cellTrans.CellStyle = style3;
                    cellTrans.SetCellValue("原籍部門");
                    cellTrans = rowTrans.CreateCell(8);
                    cellTrans.CellStyle = style3;


                    cellTrans = rowTrans.CreateCell(9);
                    cellTrans.CellStyle = style3;
                    cellTrans.SetCellValue("原籍資格");
                    cellTrans = rowTrans.CreateCell(10);
                    cellTrans.CellStyle = style3;



                    cellTrans = rowTrans.CreateCell(11);
                    cellTrans.CellStyle = style3;
                    cellTrans.SetCellValue("受入");
                    cellTrans = rowTrans.CreateCell(12);
                    cellTrans.CellStyle = style3;
                    cellTrans = rowTrans.CreateCell(13);
                    cellTrans.CellStyle = style3;
                    cellTrans = rowTrans.CreateCell(14);
                    cellTrans.CellStyle = style3;
                    cellTrans = rowTrans.CreateCell(15);
                    cellTrans.CellStyle = style3;


                    cellTrans = rowTrans.CreateCell(16);
                    cellTrans.CellStyle = style3;
                    cellTrans.SetCellValue("開始日期");
                    cellTrans = rowTrans.CreateCell(17);
                    cellTrans.CellStyle = style3;

                    cellTrans = rowTrans.CreateCell(18);
                    cellTrans.CellStyle = style3;
                    cellTrans.SetCellValue("開始日期");
                    cellTrans = rowTrans.CreateCell(19);
                    cellTrans.CellStyle = style3;

                    GridRow++;
                    rowTrans = sheet.CreateRow(GridRow);

                    sheet.AddMergedRegion(new CellRangeAddress(GridRow - 1, GridRow, 1, 2));
                    sheet.AddMergedRegion(new CellRangeAddress(GridRow - 1, GridRow, 3, 4));
                    sheet.AddMergedRegion(new CellRangeAddress(GridRow - 1, GridRow, 5, 6));
                    sheet.AddMergedRegion(new CellRangeAddress(GridRow - 1, GridRow, 7, 8));
                    sheet.AddMergedRegion(new CellRangeAddress(GridRow - 1, GridRow, 9, 10));
                    sheet.AddMergedRegion(new CellRangeAddress(GridRow - 1, GridRow - 1, 11, 15));
                    sheet.AddMergedRegion(new CellRangeAddress(GridRow - 1, GridRow, 16, 17));
                    sheet.AddMergedRegion(new CellRangeAddress(GridRow - 1, GridRow, 18, 19));
                    cellTrans = rowTrans.CreateCell(11);
                    cellTrans.CellStyle = style3;
                    cellTrans.SetCellValue("國家");
                    cellTrans = rowTrans.CreateCell(12);
                    cellTrans.CellStyle = style3;
                    cellTrans.SetCellValue("公司");
                    cellTrans = rowTrans.CreateCell(13);
                    cellTrans.CellStyle = style3;
                    cellTrans.SetCellValue("部門");
                    cellTrans = rowTrans.CreateCell(14);
                    cellTrans.CellStyle = style3;
                    cellTrans = rowTrans.CreateCell(15);
                    cellTrans.CellStyle = style3;

                    sheet.AddMergedRegion(new CellRangeAddress(GridRow, GridRow, 13, 15));


                    GridRow++;


                    for (int i = 0; i < TransData.Rows.Count; i++)
                    {
                        rowTrans = sheet.CreateRow(GridRow);
                        cellTrans = rowTrans.CreateCell(1);
                        cellTrans.CellStyle = style2;
                        cellTrans.SetCellValue(TransData.Rows[i]["RowNumber"].ToString());
                        cellTrans = rowTrans.CreateCell(2);
                        cellTrans.CellStyle = style2;
                        sheet.AddMergedRegion(new CellRangeAddress(GridRow, GridRow, 1, 2));

                        cellTrans = rowTrans.CreateCell(3);
                        cellTrans.CellStyle = style2;
                        cellTrans.SetCellValue(TransData.Rows[i]["HR_CHG_DESC"].ToString());
                        cellTrans = rowTrans.CreateCell(4);
                        cellTrans.CellStyle = style2;
                        sheet.AddMergedRegion(new CellRangeAddress(GridRow, GridRow, 3, 4));

                        cellTrans = rowTrans.CreateCell(5);
                        cellTrans.CellStyle = style2;
                        cellTrans.SetCellValue(TransData.Rows[i]["ICT_TYPE"].ToString());
                        cellTrans = rowTrans.CreateCell(6);
                        cellTrans.CellStyle = style2;
                        sheet.AddMergedRegion(new CellRangeAddress(GridRow, GridRow, 5, 6));

                        cellTrans = rowTrans.CreateCell(7);
                        cellTrans.CellStyle = style2;
                        cellTrans.SetCellValue(TransData.Rows[i]["ORI_DEPT_NAME_20"].ToString());
                        cellTrans = rowTrans.CreateCell(8);
                        cellTrans.CellStyle = style2;
                        sheet.AddMergedRegion(new CellRangeAddress(GridRow, GridRow, 7, 8));

                        cellTrans = rowTrans.CreateCell(9);
                        cellTrans.CellStyle = style2;
                        cellTrans.SetCellValue(TransData.Rows[i]["ORI_LEVEL_CD"].ToString());
                        cellTrans = rowTrans.CreateCell(10);
                        cellTrans.CellStyle = style2;
                        sheet.AddMergedRegion(new CellRangeAddress(GridRow, GridRow, 9, 10));
                        cellTrans = rowTrans.CreateCell(11);
                        cellTrans.CellStyle = style2;
                        cellTrans.SetCellValue(TransData.Rows[i]["TRANSFER_NATION"].ToString());
                        cellTrans = rowTrans.CreateCell(12);
                        cellTrans.CellStyle = style2;
                        cellTrans.SetCellValue(TransData.Rows[i]["TRANSFER_COMPANY"].ToString());

                        cellTrans = rowTrans.CreateCell(13);
                        cellTrans.CellStyle = style2;
                        cellTrans.SetCellValue(TransData.Rows[i]["TRANSFER_DEPT"].ToString());
                        cellTrans = rowTrans.CreateCell(14);
                        cellTrans.CellStyle = style2;
                        cellTrans = rowTrans.CreateCell(15);
                        cellTrans.CellStyle = style2;
                        sheet.AddMergedRegion(new CellRangeAddress(GridRow, GridRow, 13, 15));

                        cellTrans = rowTrans.CreateCell(16);
                        cellTrans.CellStyle = style2;
                        cellTrans.SetCellValue(TransData.Rows[i]["START_DT"].ToString());
                        cellTrans = rowTrans.CreateCell(17);
                        cellTrans.CellStyle = style2;
                        sheet.AddMergedRegion(new CellRangeAddress(GridRow, GridRow, 16, 17));

                        cellTrans = rowTrans.CreateCell(18);
                        cellTrans.CellStyle = style2;
                        cellTrans.SetCellValue(TransData.Rows[i]["END_DT"].ToString());
                        cellTrans = rowTrans.CreateCell(19);
                        cellTrans.CellStyle = style2;
                        sheet.AddMergedRegion(new CellRangeAddress(GridRow, GridRow, 18, 19));


                        GridRow++;
                    }
                    is_exist = "0";
                }
                #endregion

                #region 員工國外研修資料檔資料

                //員工國外研修資料檔資料
                GridRow++;
                IRow rowTrain = sheet.CreateRow(GridRow);
                GridRow++;
                rowTrain = sheet.CreateRow(GridRow);
                ICell cellTrain;
                cellTrain = rowTrain.CreateCell(1);
                cellTrain.CellStyle = style2;
                cellTrain.SetCellValue("【國外研修】");
                GridRow++;
                DataTable TrainData = dao.getTrainData(emp);
                if (TrainData.Rows.Count > 0)
                {
                    rowTrain = sheet.CreateRow(GridRow);
                    cellTrain = rowTrain.CreateCell(1);
                    cellTrain.CellStyle = style1;
                    cellTrain.SetCellValue("序號");
                    cellTrain = rowTrain.CreateCell(2);
                    cellTrain.CellStyle = style1;
                    sheet.AddMergedRegion(new CellRangeAddress(GridRow, GridRow, 1, 2));

                    cellTrain = rowTrain.CreateCell(3);
                    cellTrain.CellStyle = style1;
                    cellTrain.SetCellValue("原籍部門");
                    cellTrain = rowTrain.CreateCell(4);
                    cellTrain.CellStyle = style1;
                    sheet.AddMergedRegion(new CellRangeAddress(GridRow, GridRow, 3, 4));

                    cellTrain = rowTrain.CreateCell(5);
                    cellTrain.CellStyle = style1;
                    cellTrain.SetCellValue("研修起日");
                    cellTrain = rowTrain.CreateCell(6);
                    cellTrain.CellStyle = style1;
                    sheet.AddMergedRegion(new CellRangeAddress(GridRow, GridRow, 5, 6));

                    cellTrain = rowTrain.CreateCell(7);
                    cellTrain.CellStyle = style1;
                    cellTrain.SetCellValue("研修迄日");
                    cellTrain = rowTrain.CreateCell(8);
                    cellTrain.CellStyle = style1;
                    sheet.AddMergedRegion(new CellRangeAddress(GridRow, GridRow, 7, 8));

                    cellTrain = rowTrain.CreateCell(9);
                    cellTrain.CellStyle = style1;
                    cellTrain.SetCellValue("受入單位");
                    cellTrain = rowTrain.CreateCell(10);
                    cellTrain.CellStyle = style1;
                    sheet.AddMergedRegion(new CellRangeAddress(GridRow, GridRow, 9, 10));
                    cellTrain = rowTrain.CreateCell(11);
                    cellTrain.CellStyle = style1;
                    cellTrain.SetCellValue("研修目的");
                    cellTrain = rowTrain.CreateCell(12);
                    cellTrain.CellStyle = style1;
                    sheet.AddMergedRegion(new CellRangeAddress(GridRow, GridRow, 11, 12));

                    GridRow++;


                    for (int i = 0; i < TrainData.Rows.Count; i++)
                    {
                        rowTrain = sheet.CreateRow(GridRow);
                        cellTrain = rowTrain.CreateCell(1);
                        cellTrain.CellStyle = style2;
                        cellTrain.SetCellValue(TrainData.Rows[i]["RowNumber"].ToString());
                        cellTrain = rowTrain.CreateCell(2);
                        cellTrain.CellStyle = style2;
                        sheet.AddMergedRegion(new CellRangeAddress(GridRow, GridRow, 1, 2));

                        cellTrain = rowTrain.CreateCell(3);
                        cellTrain.CellStyle = style2;
                        cellTrain.SetCellValue(TrainData.Rows[i]["ORI_DEPT_FULL_NAME"].ToString());
                        cellTrain = rowTrain.CreateCell(4);
                        cellTrain.CellStyle = style2;
                        sheet.AddMergedRegion(new CellRangeAddress(GridRow, GridRow, 3, 4));

                        cellTrain = rowTrain.CreateCell(5);
                        cellTrain.CellStyle = style2;
                        cellTrain.SetCellValue(TrainData.Rows[i]["START_DT"].ToString());
                        cellTrain = rowTrain.CreateCell(6);
                        cellTrain.CellStyle = style2;
                        sheet.AddMergedRegion(new CellRangeAddress(GridRow, GridRow, 5, 6));

                        cellTrain = rowTrain.CreateCell(7);
                        cellTrain.CellStyle = style2;
                        cellTrain.SetCellValue(TrainData.Rows[i]["END_DT"].ToString());
                        cellTrain = rowTrain.CreateCell(8);
                        cellTrain.CellStyle = style2;
                        sheet.AddMergedRegion(new CellRangeAddress(GridRow, GridRow, 7, 8));

                        cellTrain = rowTrain.CreateCell(9);
                        cellTrain.CellStyle = style2;
                        cellTrain.SetCellValue(TrainData.Rows[i]["TRAINING_COMPANY"].ToString());
                        cellTrain = rowTrain.CreateCell(10);
                        cellTrain.CellStyle = style2;
                        sheet.AddMergedRegion(new CellRangeAddress(GridRow, GridRow, 9, 10));
                        cellTrain = rowTrain.CreateCell(11);
                        cellTrain.CellStyle = style2;
                        cellTrain.SetCellValue(TrainData.Rows[i]["TRAINING_GOAL"].ToString());
                        cellTrain = rowTrain.CreateCell(12);
                        cellTrain.CellStyle = style2;
                        sheet.AddMergedRegion(new CellRangeAddress(GridRow, GridRow, 11, 12));

                        GridRow++;
                    }
                    is_exist = "0";
                }
                #endregion

                #region 員工兼任履歷檔資料

                //員工兼任履歷檔資料
                GridRow++;
                IRow rowOtherJob = sheet.CreateRow(GridRow);
                GridRow++;
                rowOtherJob = sheet.CreateRow(GridRow);
                ICell cellOtherJob;
                cellOtherJob = rowOtherJob.CreateCell(1);
                cellOtherJob.CellStyle = style2;
                cellOtherJob.SetCellValue("【兼任】");
                GridRow++;
                DataTable OtherJobData = dao.getOtherJobData(emp);
                if (OtherJobData.Rows.Count > 0)
                {
                    rowOtherJob = sheet.CreateRow(GridRow);

                    cellOtherJob = rowOtherJob.CreateCell(1);
                    cellOtherJob.CellStyle = style3;
                    cellOtherJob.SetCellValue("序號");
                    cellOtherJob = rowOtherJob.CreateCell(2);
                    cellOtherJob.CellStyle = style3;


                    cellOtherJob = rowOtherJob.CreateCell(3);
                    cellOtherJob.CellStyle = style3;
                    cellOtherJob.SetCellValue("原籍部門");
                    cellOtherJob = rowOtherJob.CreateCell(4);
                    cellOtherJob.CellStyle = style3;


                    cellOtherJob = rowOtherJob.CreateCell(5);
                    cellOtherJob.CellStyle = style3;
                    cellOtherJob.SetCellValue("兼任");
                    cellOtherJob = rowOtherJob.CreateCell(6);
                    cellOtherJob.CellStyle = style3;
                    cellOtherJob = rowOtherJob.CreateCell(7);
                    cellOtherJob.CellStyle = style3;
                    cellOtherJob = rowOtherJob.CreateCell(8);
                    cellOtherJob.CellStyle = style3;
                    cellOtherJob = rowOtherJob.CreateCell(9);
                    cellOtherJob.CellStyle = style3;

                    cellOtherJob = rowOtherJob.CreateCell(10);
                    cellOtherJob.CellStyle = style3;
                    cellOtherJob.SetCellValue("開始日期");
                    cellOtherJob = rowOtherJob.CreateCell(11);
                    cellOtherJob.CellStyle = style3;

                    cellOtherJob = rowOtherJob.CreateCell(12);
                    cellOtherJob.CellStyle = style3;
                    cellOtherJob.SetCellValue("結束日期");
                    cellOtherJob = rowOtherJob.CreateCell(13);
                    cellOtherJob.CellStyle = style3;



                    GridRow++;
                    rowOtherJob = sheet.CreateRow(GridRow);

                    sheet.AddMergedRegion(new CellRangeAddress(GridRow - 1, GridRow, 1, 2));
                    sheet.AddMergedRegion(new CellRangeAddress(GridRow - 1, GridRow, 3, 4));
                    sheet.AddMergedRegion(new CellRangeAddress(GridRow - 1, GridRow - 1, 5, 9));
                    sheet.AddMergedRegion(new CellRangeAddress(GridRow - 1, GridRow, 10, 11));
                    sheet.AddMergedRegion(new CellRangeAddress(GridRow - 1, GridRow, 12, 13));
                    cellOtherJob = rowOtherJob.CreateCell(5);
                    cellOtherJob.CellStyle = style3;
                    cellOtherJob.SetCellValue("部門");
                    cellOtherJob = rowOtherJob.CreateCell(6);
                    cellOtherJob.CellStyle = style3;
                    cellOtherJob = rowOtherJob.CreateCell(7);
                    cellOtherJob.CellStyle = style3;
                    sheet.AddMergedRegion(new CellRangeAddress(GridRow, GridRow, 5, 7));
                    cellOtherJob = rowOtherJob.CreateCell(8);
                    cellOtherJob.CellStyle = style3;
                    cellOtherJob.SetCellValue("職務");
                    cellOtherJob = rowOtherJob.CreateCell(9);
                    cellOtherJob.CellStyle = style3;
                    sheet.AddMergedRegion(new CellRangeAddress(GridRow, GridRow, 8, 9));


                    GridRow++;


                    for (int i = 0; i < OtherJobData.Rows.Count; i++)
                    {
                        rowOtherJob = sheet.CreateRow(GridRow);
                        cellOtherJob = rowOtherJob.CreateCell(1);
                        cellOtherJob.CellStyle = style2;
                        cellOtherJob.SetCellValue(OtherJobData.Rows[i]["RowNumber"].ToString());
                        cellOtherJob = rowOtherJob.CreateCell(2);
                        cellOtherJob.CellStyle = style2;
                        sheet.AddMergedRegion(new CellRangeAddress(GridRow, GridRow, 1, 2));

                        cellOtherJob = rowOtherJob.CreateCell(3);
                        cellOtherJob.CellStyle = style2;
                        cellOtherJob.SetCellValue(OtherJobData.Rows[i]["ORI_DIV_DEPT_FULL_NAME"].ToString());
                        cellOtherJob = rowOtherJob.CreateCell(4);
                        cellOtherJob.CellStyle = style2;
                        sheet.AddMergedRegion(new CellRangeAddress(GridRow, GridRow, 3, 4));

                        cellOtherJob = rowOtherJob.CreateCell(5);
                        cellOtherJob.CellStyle = style2;
                        cellOtherJob.SetCellValue(OtherJobData.Rows[i]["OTHER_DEPT_NAME"].ToString());
                        cellOtherJob = rowOtherJob.CreateCell(6);
                        cellOtherJob.CellStyle = style2;
                        cellOtherJob = rowOtherJob.CreateCell(7);
                        cellOtherJob.CellStyle = style2;
                        sheet.AddMergedRegion(new CellRangeAddress(GridRow, GridRow, 5, 7));


                        cellOtherJob = rowOtherJob.CreateCell(8);
                        cellOtherJob.CellStyle = style2;
                        cellOtherJob.SetCellValue(OtherJobData.Rows[i]["OTHER_PJOB_DESC"].ToString());
                        cellOtherJob = rowOtherJob.CreateCell(9);
                        cellOtherJob.CellStyle = style2;
                        sheet.AddMergedRegion(new CellRangeAddress(GridRow, GridRow, 8, 9));



                        cellOtherJob = rowOtherJob.CreateCell(10);
                        cellOtherJob.CellStyle = style2;
                        cellOtherJob.SetCellValue(OtherJobData.Rows[i]["START_DT"].ToString());
                        cellOtherJob = rowOtherJob.CreateCell(11);
                        cellOtherJob.CellStyle = style2;
                        sheet.AddMergedRegion(new CellRangeAddress(GridRow, GridRow, 10, 11));

                        cellOtherJob = rowOtherJob.CreateCell(12);
                        cellOtherJob.CellStyle = style2;
                        cellOtherJob.SetCellValue(OtherJobData.Rows[i]["END_DT"].ToString());
                        cellOtherJob = rowOtherJob.CreateCell(13);
                        cellOtherJob.CellStyle = style2;
                        sheet.AddMergedRegion(new CellRangeAddress(GridRow, GridRow, 12, 13));



                        GridRow++;
                    }
                    is_exist = "0";
                }
                #endregion

            }

            if (is_exist == "0")
            {
                //ExcelHandle.exportExcel(workbook, "員工履歷清冊.xlsx");
                return workbook;
            }
            else
            {
                return null;
            }
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getAssessData(string emp_id)
    {
        CFB2HB0600DAO dao = new CFB2HB0600DAO();
        try
        {
            return dao.getAssessData(emp_id);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getEmpName(string emp_id)
    {
        try
        {
            CFB2HB0600DAO wfb2hb = new CFB2HB0600DAO();
            return wfb2hb.getEmpName(emp_id);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getDEPT_NAME(string dept_no)
    {
        try
        {
            CFB2HB0600DAO wfb2hb = new CFB2HB0600DAO();
            return wfb2hb.getDEPT_NAME(dept_no);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getPJOB_DESC(string pjob_cd)
    {
        try
        {
            CFB2HB0600DAO wfb2hb = new CFB2HB0600DAO();
            return wfb2hb.getPJOB_DESC(pjob_cd);
        }
        catch (Exception)
        {

            throw;
        }
    }
}