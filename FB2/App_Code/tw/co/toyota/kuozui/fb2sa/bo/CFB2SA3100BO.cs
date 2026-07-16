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
using Microsoft.Reporting.WebForms;
using System.Configuration;
using iTextSharp.text.pdf;

/// <summary>
/// CFB2SA310BO 的摘要描述
/// </summary>
public class CFB2SA3100BO : BaseService
{
    public CFB2SA3100BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    


    //對象生成
    public string exec_GEN_2SCHG_SALARY_EMP(CFB2SA3100DAO SA310DAO)
    {
        string rtnmessage = "";//檢查後的訊息
        try
        {
            SA310DAO.exec_GEN_2SCHG_SALARY_EMP();
            rtnmessage += utilities.getSPLOG("SP_S_GEN_2SCHG_SALARY_EMP");
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

    //(修改)
    public string updSave(List<Tuple<string,string>> keysList,string ismail)
    {
        CFB2SA3100DAO SA310DAO = new CFB2SA3100DAO();
        string rtnmessage = "";//存在檢查後的訊息
        try
        {

            SA310DAO.IS_MAIL = ismail;
            SA310DAO.UPDATED_BY = SessionHandle.Current.emp_id;
            SA310DAO.FUNC_ID = "FBSA310";


            //檢查完成後，逐筆進行(修改)
            if (rtnmessage == "")
            {
                BeginTransaction();
                foreach (var item in keysList)
                {
                    SA310DAO.HR_CHG_NO = item.Item1;
                    SA310DAO.EMP_ID = item.Item2;                   
                    SA310DAO.updSave();
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
    public IWorkbook excelDownload(string excelPath, CFB2SA3100DAO SA310DAO)
    {
        FileStream fs = null;
        IWorkbook workbook = null;

        //取得範本sheet
        ISheet sheet = null;
        try
        {
            //取得下載資料
            DataTable dt = SA310DAO.getExcelData();

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

                        //工號
                        cell = row.CreateCell(0);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["EMP_ID"].ToString());
                        //姓名    	   						
                        cell = row.CreateCell(1);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["EMP_NAME"].ToString());
                        //生效日期    	   						
                        cell = row.CreateCell(2);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["STARTDT"].ToString());
                        //寄件否    	   						
                        cell = row.CreateCell(3);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["IS_MAIL_DESC"].ToString());
                        //異動代碼    	   						
                        cell = row.CreateCell(4);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["HR_CHG_CD_DESC"].ToString());


                        //職務(原)     	   						
                        cell = row.CreateCell(5);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["PJOB_CD_OLD_DESC"].ToString());
                        //本薪(原)    	   						
                        cell = row.CreateCell(6);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue((Convert.ToDouble(dt.Rows[i]["ABILITY_PAY_OLD"].ToString())).ToString("N0"));
                        //職務俸(原)    	   						
                        cell = row.CreateCell(7);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue((Convert.ToDouble(dt.Rows[i]["PJOB_PAY_OLD"].ToString())).ToString("N0"));

                        //職務    	   						
                        cell = row.CreateCell(8);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["PJOB_CD_DESC"].ToString());
                        //本薪   	   						
                        cell = row.CreateCell(9);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue((Convert.ToDouble(dt.Rows[i]["ABILITY_PAY"].ToString())).ToString("N0"));
                        //職務俸    	   						
                        cell = row.CreateCell(10);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue((Convert.ToDouble(dt.Rows[i]["PJOB_PAY"].ToString())).ToString("N0"));
                        //伴食津貼    	   						
                        cell = row.CreateCell(11);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue((Convert.ToDouble(dt.Rows[i]["FOOD_PAY"].ToString())).ToString("N0"));

                        //人事異動單號    	   						
                        cell = row.CreateCell(12);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["HR_CHG_NO"].ToString());

                         
                    }
                   for (int i = 0; i <= 12; i++)
                   {
                       sheet.AutoSizeColumn(i);
                   }

                    row = sheet.GetRow(0);
                    cell = row.CreateCell(13);
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


    //寄送mail
    public string execSendMail(CFB2SA3100DAO SA310DAO,string doFlag)
    {
       
        string rtnmessage = "";//檢查後的訊息
        try
        {
            #region 新增寄件主檔/明細檔
            BeginTransaction();            
            //新增郵件主檔
            SA310DAO.insert_Mail_H();
            SA310DAO.insert_Mail_D();
            Commit();
            #endregion

            //執行寄信動作
            rtnmessage =sendMail(SA310DAO, doFlag);

            return rtnmessage;
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    //寄信作業
    public string sendMail(CFB2SA3100DAO SA310DAO, string doFlag)
    {
        string rtnmessage = "";//檢查後的訊息
        try
        {
            ReportViewer reportviewer1 = null;
            ReportParameter[] parameters = null;
            //員工基本資料
            string HR_CHG_NO = "";
            string EMP_ID = "";
            string EMP_NAME = "";
            string EFFECT_YM = "";
            string DEPT_FULL_NAME = "";            
            string LEVEL_CD = "";

            //調整前
            string PJOB_CD_OLD ="";
            decimal ABILITY_PAY_OLD = 0;
            decimal PJOB_PAY_OLD = 0;
            decimal FOOD_PAY_OLD = 0;
            decimal TOTAL_AMT_OLD = 0;

            //調整後
            string PJOB_CD = "";
            decimal ABILITY_PAY = 0;
            decimal PJOB_PAY = 0;
            decimal FOOD_PAY = 0;
            decimal TOTAL_AMT = 0;

            //郵件參數
            string  MAIL_TITLE  ="";
            string  MAIL_DESC   ="";
            string  MAIL_CONTENT = "";
            string  SENDTO_MAIL ="";
            string  MAIL_TO     = "";
            string  PASSWORD    ="";
            List<string> mailtoList = null;
            //string photoPath = "D:/2S_ALT.gif";

            //
            Warning[] warnings;
            byte[] bytes;
            string[] streamids;
            string mimeType;
            string encoding;
            string filenameExtension;

            //取得寄信員工資料
            DataTable mailData = SA310DAO.getMailData();
            if (mailData.Rows.Count > 0)
            {
               
                for (int row = 0; row < mailData.Rows.Count; row++)
                {
                    reportviewer1 = new ReportViewer();
                    //將ReportViewer1的DataSources集合清除
                    reportviewer1.LocalReport.DataSources.Clear();
                    //將ReportViewer1重置為初始狀態           
                    reportviewer1.Reset();
                    // 設定 ReportViewer1 的 DataSources
                    reportviewer1.LocalReport.Refresh();

                    //員工資料
                    HR_CHG_NO = mailData.Rows[row]["HR_CHG_NO"].ToString();
                    EMP_ID = mailData.Rows[row]["EMP_ID"].ToString();
                    EMP_NAME = mailData.Rows[row]["EMP_NAME"].ToString();
                    EFFECT_YM = mailData.Rows[row]["EFFECT_YM"].ToString();
                    DEPT_FULL_NAME = mailData.Rows[row]["DEPT_FULL_NAME"].ToString();
                    LEVEL_CD = mailData.Rows[row]["LEVEL_CD"].ToString();

                    MAIL_TITLE = mailData.Rows[row]["MAIL_TITLE"].ToString();   //主旨
                    MAIL_DESC = mailData.Rows[row]["MAIL_DESC"].ToString();     //內文
                    SENDTO_MAIL = mailData.Rows[row]["SENDTO_MAIL"].ToString();    //寄件者mail  

                    //清空
                    MAIL_CONTENT = "";
                    MAIL_CONTENT += EMP_NAME + @" 您好 <br>";
                    MAIL_CONTENT += MAIL_DESC.Replace("\r", "").Replace("\n", "<br>") + @" <br>";
                    MAIL_CONTENT += @"【2S職務別考核範圍】年度能力考課，原則將以新職務之考課範圍進行評價。<br>";
                    MAIL_CONTENT += @"<img src=cid:Image1 alt='image description'> <br />";//  << 附圖相關程式1

                    if (doFlag == "USER")
                    {
                        MAIL_TO = mailData.Rows[row]["MAIL_USER"].ToString();        //收件者maill
                        PASSWORD = mailData.Rows[row]["PW_USER"].ToString();       //PDF密碼
                    }
                    if (doFlag == "2S") {
                        MAIL_TO = mailData.Rows[row]["MAIL_2S"].ToString();        //收件者maill
                        PASSWORD = mailData.Rows[row]["PW_2S"].ToString();       //PDF密碼
                    }
                    
                    //調整前
                    PJOB_CD_OLD = mailData.Rows[row]["PJOB_DESC_OLD"].ToString();
                    if (!decimal.TryParse(mailData.Rows[row]["ABILITY_PAY_OLD"].ToString(), out ABILITY_PAY_OLD))
                    {
                        ABILITY_PAY_OLD = 0;
                    }
                    if (!decimal.TryParse(mailData.Rows[row]["PJOB_PAY_OLD"].ToString(), out PJOB_PAY_OLD))
                    {
                        PJOB_PAY_OLD = 0;
                    }
                    if (!decimal.TryParse(mailData.Rows[row]["FOOD_PAY"].ToString(), out FOOD_PAY_OLD))
                    {
                        FOOD_PAY_OLD = 0;
                    }

                    TOTAL_AMT_OLD = ABILITY_PAY_OLD + PJOB_PAY_OLD + FOOD_PAY_OLD;
                    //調整後
                    PJOB_CD = mailData.Rows[row]["PJOB_DESC"].ToString();
                    if (!decimal.TryParse(mailData.Rows[row]["ABILITY_PAY"].ToString(), out ABILITY_PAY))
                    {
                        ABILITY_PAY = 0;
                    }
                    if (!decimal.TryParse(mailData.Rows[row]["PJOB_PAY"].ToString(), out PJOB_PAY))
                    {
                        PJOB_PAY = 0;
                    }
                    if (!decimal.TryParse(mailData.Rows[row]["FOOD_PAY"].ToString(), out FOOD_PAY))
                    {
                        FOOD_PAY = 0;
                    }

                    TOTAL_AMT = ABILITY_PAY + PJOB_PAY + FOOD_PAY;

                    parameters = new ReportParameter[16];
                    parameters[0] = new ReportParameter("EFFECT_YM", EFFECT_YM);
                    parameters[1] = new ReportParameter("DEPT_FULL_NAME", DEPT_FULL_NAME);
                    parameters[2] = new ReportParameter("EMP_NAME", EMP_NAME);
                    parameters[3] = new ReportParameter("EMP_ID", EMP_ID);
                    parameters[4] = new ReportParameter("LEVEL_CD", LEVEL_CD);

                    parameters[5] = new ReportParameter("PJOB_CD_OLD", PJOB_CD_OLD);
                    parameters[6] = new ReportParameter("ABILITY_PAY_OLD", ABILITY_PAY_OLD.ToString("N0"));
                    parameters[7] = new ReportParameter("TOTAL_AMT_OLD", TOTAL_AMT_OLD.ToString("N0"));
                    parameters[8] = new ReportParameter("PJOB_PAY_OLD", PJOB_PAY_OLD.ToString("N0"));                    
                    parameters[9] = new ReportParameter("FOOD_PAY_OLD", FOOD_PAY_OLD.ToString("N0"));
                    parameters[10] = new ReportParameter("TOTAL_AMT_OLD", TOTAL_AMT_OLD.ToString("N0"));


                    parameters[11] = new ReportParameter("PJOB_CD", PJOB_CD);
                    parameters[12] = new ReportParameter("ABILITY_PAY", ABILITY_PAY.ToString("N0"));
                    parameters[13] = new ReportParameter("PJOB_PAY", PJOB_PAY.ToString("N0"));   
                    parameters[14] = new ReportParameter("FOOD_PAY", FOOD_PAY.ToString("N0"));
                    parameters[15] = new ReportParameter("TOTAL_AMT", TOTAL_AMT.ToString("N0"));


                    reportviewer1.LocalReport.ReportPath = "report/WFB2SA310.rdlc";
                    reportviewer1.LocalReport.SetParameters(parameters);
                    reportviewer1.LocalReport.DataSources.Add(new ReportDataSource("dsSA310", mailData));
                                
                    //生成bytes的PDF
                    bytes = reportviewer1.LocalReport.Render(
                           "PDF", null, out mimeType, out encoding, out filenameExtension,
                           out streamids, out warnings);
                    /*
                    //生成實體PDF
                    FileStream fs = new FileStream("D:\\Kuozui\\" + EMP_ID + "_1" + ".pdf", FileMode.Create);
                    fs.Write(bytes, 0, bytes.Length);
                    fs.Close();
                    //加密
                    using (PdfReader reader = new PdfReader("D:\\Kuozui\\" + EMP_ID + "_1" + ".pdf"))
                    {
                        using (var os = new FileStream("D:\\Kuozui\\" + EMP_ID + "_1.pdf", FileMode.Create))
                        {
                            PdfEncryptor.Encrypt(reader,
                                                 os,
                                                 true,
                                                 "12345",
                                                 "12345",
                                                 PdfWriter.ALLOW_PRINTING);

                        }
                    }
                    */        
                
                    warnings=null;
                    streamids=null;
                    mimeType="";
                    encoding = "";
                    filenameExtension = "";

                    //收件者
                    mailtoList = new List<string>();
                    mailtoList.Add(MAIL_TO);
                    

                    /* 寄出信件*/
                    PdfCreate pdfcreate = new PdfCreate();
                    pdfcreate.bf = BaseFont.CreateFont(SA310DAO.fontsPath, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
                    
                    /*
                    MemoryStream ms = new MemoryStream();
                    DataTable dt = new DataTable();
                    ms = pdfcreate.createPDF_Salary(dt);
                    string FileName = "薪資單.pdf";
                    using (MemoryStream dolly = new MemoryStream(ms.ToArray()))
                    {
                        utilities.SendMail("測試信件", "測試內容", ConfigurationManager.AppSettings["smtpServerMail"], mailtoList, file_name: FileName, attch: dolly);
                    }
                    */
                    
                    using (MemoryStream sendStream = new MemoryStream(bytes))
                    {
                        using (PdfReader reader2 = new PdfReader(sendStream))
                        {
                            using (MemoryStream outputsendStream = new MemoryStream())
                            {
                                //加密
                                PdfEncryptor.Encrypt(reader2, outputsendStream, true, PASSWORD, PASSWORD, PdfWriter.ALLOW_PRINTING);
                            
                                using (MemoryStream dolly = new MemoryStream(outputsendStream.ToArray()))
                                {
                                    utilities.SendMailAlt(MAIL_TITLE                   //主旨
                                                       , MAIL_CONTENT    //內容
                                                       ,ConfigurationManager.AppSettings["smtpServerMail"]                 //寄件者 mail
                                                       ,mailtoList                  //收件者清單    
                                                       , SA310DAO.photoPath         //內容圖檔路徑
                                                       ,true
                                                       ,file_name: EMP_ID + "薪資調整通知.pdf"
                                                       ,attch: dolly);
                                }
                            }
                        }

                    }  
                    
                    //更新已寄出,寄信主檔及對象檔的FLAG
                    SA310DAO.update_Mail_D(EMP_ID, HR_CHG_NO);

                    //寄給2S才修改
                    if (doFlag == "2S")
                    {
                        SA310DAO.update_2SCHG(EMP_ID, HR_CHG_NO);
                    }

                } //迴圈結束

                //清記憶體                            
                reportviewer1.Dispose();
                bytes = null;
                parameters = null;
            }
          

            return rtnmessage;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
        finally
        { 
            //清空記憶體
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