using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.Util;
using iTextSharp;
using iTextSharp.text;
using iTextSharp.text.pdf;

/// <summary>
/// CFB2SC5100BO 的摘要描述
/// </summary>
public class CFB2SC5100BO : BaseService
{
    CFB2SC5100DAO dao = new CFB2SC5100DAO();
    //Font ChFont;
    public CFB2SC5100BO()
    {

    }
    public DataTable getExcelData(string pay_id, string salary_type, string dept_no, string emp_id)
    {
        try
        {
            DataTable dt = new DataTable();

            if (salary_type == "A")
            {
                dt = dao.getSuperData_TypeIsA(pay_id, salary_type, dept_no, emp_id);
            }
            else if (salary_type == "B")
            {
                dt = dao.getSuperData_TypeIsB(pay_id, salary_type, dept_no, emp_id);
            }
            else if (salary_type == "C")
            {
                dt = dao.getSuperData_TypeIsC(pay_id, salary_type, dept_no, emp_id);
            }
            else if (salary_type == "D")
            {
                dt = dao.getSuperData_TypeIsD(pay_id, salary_type, dept_no, emp_id);
            }
            return dt;
        }
        catch
        {
            throw;
        }
    }

    //產生pdf資料
    public MemoryStream createExcelFromTemplate(string fontpath, string pay_id, string salary_type, string dept_no, string emp_id, DataTable dtPDFData)
    {
        try
        {
            Document doc1 = new Document(PageSize.A4, 50, 50, 50, 50); //設定pagesize級margin left,right,top,bottom
            TwoColumnHeaderFooter pdf = new TwoColumnHeaderFooter();
            pdf.path = fontpath;
            MemoryStream fileStream = new MemoryStream();
            PdfWriter pdfwr = PdfWriter.GetInstance(doc1, fileStream);
            pdfwr.PageEvent = pdf;

            //字型設定
            BaseFont bfChinese = BaseFont.CreateFont(fontpath, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            Font ChFont = new Font(bfChinese, 14, Font.NORMAL, BaseColor.BLACK);

            if (dtPDFData.Rows.Count > 0)
            {
                doc1.Open();
                //產生PDF流程
                creatpdfFlow(doc1, ChFont, dtPDFData, salary_type);
                doc1.Close();
            }
            return fileStream;
        }
        catch
        {
            throw;
        }
    }
    //產生PDF流程
    private void creatpdfFlow(Document doc1, Font ChFont, DataTable dtPDFData, string salary_type)
    {
        string title = "";  //頁首標題
        //不同發薪類，取得表頭標題
        if (salary_type == "A" || salary_type == "B")
        {
            title = dao.getSALARY_TYPE(salary_type);
        }
        //string title = string.Empty;   //頁首標題
        string group_id = "default";     //群組名稱
        string data_ym = "";             //群組 薪資年月
        int currentRow = 0;              //此頁目前的rowIndex
        int pageCount = 0;               //分頁筆數
        double pageAmount = 0.0;         //分頁總金額
        int AllCount = 0;                //報表總筆數
        double AllAmount = 0.0;          //報表總金額
        double groupAmount = 0.0;        //群組總金額
        int groupCount = 0;              //群組總筆數
        string flag = "default";     //新分類名稱 202103 以後 用此來分類

        for (int dataCount = 0; dataCount < dtPDFData.Rows.Count; dataCount++)
        {
            string tt = dtPDFData.Rows[dataCount]["GROUP_ID"].ToString();
            string name = dtPDFData.Rows[dataCount]["EMP_NAME"].ToString();
            if (Convert.ToString(dtPDFData.Rows[dataCount]["Flag"]) != flag)//確認 改為 SALARY_ACCOUNT_BANK 看是否OK
            {
                if (pageCount != 0)
                {
                    //分頁表尾
                    currentRow = creatPageFooter(doc1, ChFont, pageCount, pageAmount, dtPDFData.Rows[dataCount - 1], currentRow,groupAmount,groupCount,"Y");

                    if (pageCount >= 30)
                    {
                        pageCount = pageCount - 30;
                    }
                    //if (currentRow >= 39)
                    //{
                    //    currentRow = currentRow - 39;
                    //}
                    pageCount = 0;           //分頁筆數清空
                    pageAmount = 0.0;        //分頁總金額清空
                    groupAmount = 0.0;
                    groupCount = 0;
                    //插入分頁
                    doc1.NewPage();
                    
                }
                flag = Convert.ToString(dtPDFData.Rows[dataCount]["Flag"]);
                data_ym = Convert.ToString(dtPDFData.Rows[dataCount]["SALARY_YM"]);
                //分頁表頭
                currentRow = createPageHeader(doc1, ChFont, data_ym, salary_type, dtPDFData.Rows[dataCount], title, currentRow);               
            }
            //每筆資料
            currentRow = createSingleRow(doc1, ChFont, dtPDFData.Rows[dataCount], currentRow);
            groupCount++;
            pageCount++;
            AllCount++;
            if (dtPDFData.Rows[dataCount]["AMOUNT"] != DBNull.Value && dtPDFData.Rows[dataCount]["AMOUNT"].ToString() != "")
            {
                pageAmount = pageAmount + Convert.ToDouble(dtPDFData.Rows[dataCount]["AMOUNT"]);
                groupAmount = groupAmount + Convert.ToDouble(dtPDFData.Rows[dataCount]["AMOUNT"]);
                AllAmount = AllAmount + Convert.ToDouble(dtPDFData.Rows[dataCount]["AMOUNT"]);
            }
            //全部的最後一筆要分頁表尾
            if (dataCount == dtPDFData.Rows.Count - 1)
            {
                currentRow = creatPageFooter(doc1, ChFont, pageCount, pageAmount, dtPDFData.Rows[dataCount], currentRow, groupAmount,groupCount,"Y");
                if (pageCount >= 30)
                    pageCount = pageCount - 30;
                //if (currentRow >= 39)
                //    currentRow = currentRow -39;
            }
            //每頁超過39行要換頁
            //else if (currentRow >= 39 && group_id == Convert.ToString(dtPDFData.Rows[dataCount + 1]["GROUP_ID"]))//
            else if (pageCount >= 30 && flag == Convert.ToString(dtPDFData.Rows[dataCount + 1]["flag"]))//
            {
                currentRow = 0;
                //分頁表尾
                currentRow = creatPageFooter(doc1, ChFont, pageCount, pageAmount, dtPDFData.Rows[dataCount], currentRow);
                pageCount = 0;
                pageAmount = 0.0;
                //插入分頁
                doc1.NewPage();
                //分頁表頭
                currentRow = createPageHeader(doc1, ChFont, data_ym, salary_type, dtPDFData.Rows[dataCount], title, currentRow);
            }
        }

        //總表尾
        createMianFooter(doc1, ChFont, AllCount, AllAmount);
    }
    //每筆資料 共1行
    private int createSingleRow(Document doc1, Font ChFont, DataRow rowExcel, int currentRow)
    {
        string salary_account_subject = string.Empty;
        string salary_account = string.Empty;
        //確認銀行代碼不為0，才能取得科目4-5碼
        if (Convert.ToString(rowExcel["SALARY_ACCOUNT_NO"]) != "0" && Convert.ToString(rowExcel["SALARY_ACCOUNT_NO"]).Trim() != "" && Convert.ToString(rowExcel["SALARY_ACCOUNT_NO"]).Length >= 5)
            salary_account_subject = Convert.ToString(rowExcel["SALARY_ACCOUNT_NO"]).Substring(3, 2);
        else if (Convert.ToString(rowExcel["SALARY_ACCOUNT_NO"]).Length == 4)
            salary_account_subject = Convert.ToString(rowExcel["SALARY_ACCOUNT_NO"]).Substring(3);
        //確認銀行代碼不為0，才能取得帳號6-12碼
        if (Convert.ToString(rowExcel["SALARY_ACCOUNT_NO"]) != "0" && Convert.ToString(rowExcel["SALARY_ACCOUNT_NO"]).Trim() != "" && Convert.ToString(rowExcel["SALARY_ACCOUNT_NO"]).Length >= 12)
            salary_account = Convert.ToString(rowExcel["SALARY_ACCOUNT_NO"]).Substring(5, 7);
        else if (Convert.ToString(rowExcel["SALARY_ACCOUNT_NO"]).Length > 5 && Convert.ToString(rowExcel["SALARY_ACCOUNT_NO"]).Length < 12)
            salary_account = Convert.ToString(rowExcel["SALARY_ACCOUNT_NO"]).Substring(5);

        PdfPTable table = new PdfPTable(new float[] { 1, 1, 1, 1, 1, 1, 1, 1, 1 });
        table.TotalWidth = 500f;
        table.LockedWidth = true;
        PdfPCell cell = new PdfPCell();
        cell.Phrase = new Phrase(salary_account_subject, ChFont);   //科目  1
        cell.HorizontalAlignment = Element.ALIGN_CENTER;
        cell.BorderWidth = 0;
        table.AddCell(cell);

        cell.Phrase = new Phrase("  ", ChFont);                       //空白格2
        cell.BorderWidth = 0;
        table.AddCell(cell);

        cell.Phrase = new Phrase(salary_account, ChFont);           //帳號  3
        cell.HorizontalAlignment = Element.ALIGN_CENTER;
        cell.BorderWidth = 0;
        table.AddCell(cell);

        cell.Phrase = new Phrase("  ", ChFont);                       //空白格4
        cell.BorderWidth = 0;
        table.AddCell(cell);

        cell.Phrase = new Phrase(rowExcel["EMP_NAME"].ToString().Trim(), ChFont);           //戶名  5.6
        cell.Colspan = 2;
        cell.PaddingLeft = 20f;
        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        cell.BorderWidth = 0;
        table.AddCell(cell);

        if (rowExcel["AMOUNT"] == DBNull.Value || rowExcel["AMOUNT"].ToString() == "")
            cell.Phrase = new Phrase("0", ChFont);
        else
            cell.Phrase = new Phrase(Convert.ToDouble(rowExcel["AMOUNT"]).ToString("n0"), ChFont);            //存款金額7.8
        cell.Colspan = 2;
        cell.PaddingLeft = 0;
        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
        cell.BorderWidth = 0;
        table.AddCell(cell);

        cell.Phrase = new Phrase("  ", ChFont);       //空白格9
        cell.BorderWidth = 0;
        table.AddCell(cell);

        doc1.Add(table);
        return currentRow + 1;
    }
    //群組表頭 共6行
    private int createPageHeader(Document doc1, Font ChFont, string data_ym, string salary_type, DataRow rowExcel, string title, int currentRow)
    {
        string salary_account_no = "";
        //不同發薪類，取得表頭標題
        if (salary_type == "A" || salary_type == "B")
        {
        }
        else if (salary_type == "C")
        {
            if (Convert.ToString(rowExcel["PAY_KIND"]) == "1031")
                title = "";//20160729 惠菁要求拿掉 title = "年獎";
            else if (Convert.ToString(rowExcel["PAY_KIND"]) == "1033")
                title = ""; //20160729 惠菁要求拿掉 title = "紅利";
        }
        else if (salary_type == "D")
        {
            if (Convert.ToString(rowExcel["PAY_KIND"]) == "1035" && Convert.ToString(rowExcel["FESTIVAL_TYPE"]) == "1")
                title = "端午節金";
            else if (Convert.ToString(rowExcel["PAY_KIND"]) == "1035" && Convert.ToString(rowExcel["FESTIVAL_TYPE"]) == "2")
                title = "中秋節金";
        }

        PdfPTable table = new PdfPTable(new float[] { 1, 1, 1, 1, 1, 1, 1, 1, 1 });
        table.TotalWidth = 500f;
        table.LockedWidth = true;
        PdfPCell cell = new PdfPCell();
        cell.MinimumHeight = 20f;
        //表頭第一行
        cell.Phrase = new Phrase(title + "存入銀行存款單", ChFont);
        cell.HorizontalAlignment = Element.ALIGN_CENTER;
        cell.BorderWidth = 0;
        cell.Colspan = 9;
        table.AddCell(cell);
        //表頭第二行
        cell.Phrase = new Phrase(Convert.ToInt32(data_ym.Substring(0, 4)) - 1911 + "年" + data_ym.Substring(4, 2) + "月", ChFont);    //年月
        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        cell.BorderWidth = 0;
        cell.Colspan = 9;
        table.AddCell(cell);
        //表頭第三行
        int year = DateTime.Now.Year;
        int month = DateTime.Now.Month;
        int date = DateTime.Now.Day;
        string time = (year - 1911) + "/" + month + "/" + date;
        cell.Phrase = new Phrase("存款日：" + time, ChFont);
        cell.BorderWidth = 0;
        cell.Colspan = 9;
        table.AddCell(cell);
        //int nowPage = pdfwr.PageNumber;
        //cell.Phrase = new Phrase("頁   次：" + nowPage, ChFont);
        //cell.BorderWidth = 0;
        //cell.Colspan = 4;
        //table.AddCell(cell);

        //表頭第四行
        if (Convert.ToString(rowExcel["SALARY_ACCOUNT_BANK"]) != "0" && Convert.ToString(rowExcel["SALARY_ACCOUNT_BANK"]).Trim() != "" && Convert.ToString(rowExcel["SALARY_ACCOUNT_BANK"]).Length >= 3)
        {
            //salary_account_no = Convert.ToString(rowExcel["SALARY_ACCOUNT_NO"]).Substring(0, 3);
            //202103 現金支付的話 改為現金支付
            if (Convert.ToString(rowExcel["Flag"]) == "5" ) //現金
            {
                salary_account_no = "現金支付";
            }
            else
            {
                salary_account_no = Convert.ToString(rowExcel["SALARY_ACCOUNT_BANK"]);
            }
            
        }
        else if (Convert.ToString(rowExcel["SALARY_ACCOUNT_BANK"]).Length > 0 && Convert.ToString(rowExcel["SALARY_ACCOUNT_BANK"]).Length < 3)
        {
            //202103 現金支付的話 改為現金支付
            if (Convert.ToString(rowExcel["Flag"]) == "5") //現金
            {
                salary_account_no = "現金支付";
            }
            else
            {
                salary_account_no = Convert.ToString(rowExcel["SALARY_ACCOUNT_BANK"]);
            }
            
        }
        cell.Phrase = new Phrase("銀行代碼：" + salary_account_no, ChFont);
        cell.BorderWidth = 0;
        cell.Colspan = 5;
        table.AddCell(cell);
        cell.Phrase = new Phrase("製表日期：" + DateTime.Now.ToString("yyyy/MM/dd HH:mm"), ChFont);
        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
        cell.BorderWidth = 0;
        cell.Colspan = 4;
        table.AddCell(cell);
        //表頭第五行
        cell.Phrase = new Phrase("  ", ChFont);
        cell.BorderWidth = 0;
        cell.Colspan = 9;
        table.AddCell(cell);

        //表頭第六行
        cell.Phrase = new Phrase("科目", ChFont);
        cell.HorizontalAlignment = Element.ALIGN_CENTER;
        cell.Colspan = 1;
        cell.BorderWidthTop = 2;
        cell.BorderWidthBottom = 2;
        cell.BorderWidthLeft = 0;
        cell.BorderWidthRight = 0;
        table.AddCell(cell);

        cell.Phrase = new Phrase("  ", ChFont);                       //空白格
        cell.Colspan = 1;
        table.AddCell(cell);

        cell.Phrase = new Phrase("帳號", ChFont);
        cell.HorizontalAlignment = Element.ALIGN_CENTER;
        cell.Colspan = 1;
        table.AddCell(cell);

        cell.Phrase = new Phrase("  ", ChFont);                       //空白格
        cell.Colspan = 1;
        table.AddCell(cell);

        cell.Phrase = new Phrase("戶名", ChFont);
        cell.HorizontalAlignment = Element.ALIGN_CENTER;
        cell.Colspan = 2;
        table.AddCell(cell);

        cell.Phrase = new Phrase("存款金額", ChFont);
        cell.HorizontalAlignment = Element.ALIGN_CENTER;
        cell.Colspan = 2;
        table.AddCell(cell);

        cell.Phrase = new Phrase("  ", ChFont);
        cell.Colspan = 1;
        table.AddCell(cell);

        doc1.Add(table);

        return currentRow + 6;
    }
    //群組表尾 共3行
    //private int creatPageFooter(Document doc1, Font ChFont, int pageCount, double pageAmount, DataRow rowExcel, int currentRow)
    //{
    //    string footerString = string.Empty;
    //    string footerCompanyName = string.Empty;
    //    string salary_account_no = string.Empty;
    //    //確認銀行代碼不為0，才能取得前三碼
    //    if (Convert.ToString(rowExcel["SALARY_ACCOUNT_NO"]) != "0" && Convert.ToString(rowExcel["SALARY_ACCOUNT_NO"]) != "")
    //        salary_account_no = Convert.ToString(rowExcel["SALARY_ACCOUNT_NO"]).Substring(0, 3);
    //    //外籍人士分組
    //    if (Convert.ToString(rowExcel["JPN_CD"]) != "0")
    //    {
    //        footerString = "外籍會社 " + Convert.ToString(rowExcel["JPN_NAME"]) + "共" + pageCount.ToString("n0") + "戶";
    //    }
    //    else
    //    {
    //        footerString = "銀行代碼 " + salary_account_no + "共" + pageCount.ToString("n0") + "戶";
    //    }
    //    //委託機關名稱
    //    if (Convert.ToString(rowExcel["COMPANY_CD"]) == "K")
    //        footerCompanyName = Convert.ToString(rowExcel["COMPANY_NAME"]) + "汽車股份有限公司";
    //    else
    //        footerCompanyName = Convert.ToString(rowExcel["COMPANY_NAME"]);

    //    PdfPTable table = new PdfPTable(new float[] { 1, 1, 1, 1, 1, 1, 1, 1, 1 });
    //    table.TotalWidth = 500f;
    //    table.LockedWidth = true;
    //    PdfPCell cell = new PdfPCell();
    //    //表尾第一行
    //    cell.Phrase = new Phrase("  ", ChFont);
    //    cell.Colspan = 9;
    //    cell.BorderWidthTop = 2;
    //    cell.BorderWidthRight = 0;
    //    cell.BorderWidthLeft = 0;
    //    cell.BorderWidthBottom = 0;
    //    table.AddCell(cell);
    //    //表尾第二行
    //    cell.Phrase = new Phrase(footerString, ChFont);
    //    cell.BorderWidthTop = 0;
    //    cell.BorderWidthRight = 0;
    //    cell.BorderWidthLeft = 0;
    //    cell.BorderWidthBottom = 0;
    //    cell.Colspan = 5;
    //    table.AddCell(cell);

    //    cell.Phrase = new Phrase("存入金額小計： ", ChFont);
    //    cell.BorderWidth = 0;
    //    cell.Colspan = 2;
    //    table.AddCell(cell);
    //    cell.Phrase = new Phrase(pageAmount.ToString("n0") + "元", ChFont);
    //    cell.HorizontalAlignment = Element.ALIGN_RIGHT;
    //    cell.BorderWidth = 0;
    //    cell.Colspan = 2;
    //    table.AddCell(cell);
    //    //表尾第三行
    //    cell.Phrase = new Phrase("委託機關名稱：" + footerCompanyName, ChFont);
    //    cell.HorizontalAlignment = Element.ALIGN_LEFT;
    //    cell.BorderWidth = 0;
    //    cell.Colspan = 9;
    //    table.AddCell(cell);

    //    doc1.Add(table);

    //    return currentRow + 3;
    //}
    private int creatPageFooter(Document doc1, Font ChFont, int pageCount, double pageAmount, DataRow rowExcel, int currentRow)
    {
        string footerString = string.Empty;
        string footerCompanyName = string.Empty;
        string salary_account_no = string.Empty;
        
        //確認銀行代碼不為0，才能取得前三碼
        if (Convert.ToString(rowExcel["SALARY_ACCOUNT_BANK"]) != "0" && Convert.ToString(rowExcel["SALARY_ACCOUNT_BANK"]) != "")
            salary_account_no = Convert.ToString(rowExcel["SALARY_ACCOUNT_BANK"]).Substring(0, 3);
        //外籍人士分組
        if (Convert.ToString(rowExcel["JPN_CD"]) != "0")
        {
            footerString = "外籍會社 " + Convert.ToString(rowExcel["JPN_NAME"]) + "共" + pageCount.ToString("n0") + "戶";
        }
        else
        {
            //202103 現金支付
            if (Convert.ToString(rowExcel["Flag"]) == "5") //現金
            {
                footerString = "現金支付  共"  + pageCount.ToString("n0") + "戶";
            }
            else
            {
                footerString = "銀行代碼 " + salary_account_no + "共" + pageCount.ToString("n0") + "戶";
            }
           
        }
        //委託機關名稱
        if (Convert.ToString(rowExcel["COMPANY_CD"]) == "K")
            footerCompanyName = Convert.ToString(rowExcel["COMPANY_NAME"]) + "汽車股份有限公司";
        else
            footerCompanyName = Convert.ToString(rowExcel["COMPANY_NAME"]);

        PdfPTable table = new PdfPTable(new float[] { 1, 1, 1, 1, 1, 1, 1, 1, 1 });
        table.TotalWidth = 500f;
        table.LockedWidth = true;
        PdfPCell cell = new PdfPCell();
        //表尾第一行
        cell.Phrase = new Phrase("  ", ChFont);
        cell.Colspan = 9;
        cell.BorderWidthTop = 2;
        cell.BorderWidthRight = 0;
        cell.BorderWidthLeft = 0;
        cell.BorderWidthBottom = 0;
        table.AddCell(cell);
        //表尾第二行
        cell.Phrase = new Phrase("本頁戶數" + "共" + pageCount.ToString("n0") + "戶", ChFont);
        cell.BorderWidthTop = 0;
        cell.BorderWidthRight = 0;
        cell.BorderWidthLeft = 0;
        cell.BorderWidthBottom = 0;
        cell.Colspan = 5;
        table.AddCell(cell);

        cell.Phrase = new Phrase("存入金額小計： ", ChFont);
        cell.BorderWidth = 0;
        cell.Colspan = 2;
        table.AddCell(cell);
        cell.Phrase = new Phrase(pageAmount.ToString("n0") + "元", ChFont);
        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
        cell.BorderWidth = 0;
        cell.Colspan = 2;
        table.AddCell(cell);
        //表尾第三行
        cell.Phrase = new Phrase("委託機關名稱：" + footerCompanyName, ChFont);
        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        cell.BorderWidth = 0;
        cell.Colspan = 9;
        table.AddCell(cell);

        doc1.Add(table);

        return currentRow + 3;
    }
    private int creatPageFooter(Document doc1, Font ChFont, int pageCount, double pageAmount, DataRow rowExcel, int currentRow, double groupAmount, int groupCount, string flag)
    {
        string footerString = string.Empty;
        string footerCompanyName = string.Empty;
        string salary_account_no = string.Empty;

        //確認銀行代碼不為0，才能取得前三碼
        if (Convert.ToString(rowExcel["SALARY_ACCOUNT_BANK"]) != "0" && Convert.ToString(rowExcel["SALARY_ACCOUNT_BANK"]) != "")
            salary_account_no = Convert.ToString(rowExcel["SALARY_ACCOUNT_BANK"]).Substring(0, 3);
        //外籍人士分組
        if (Convert.ToString(rowExcel["JPN_CD"]) != "0" && Convert.ToString(rowExcel["JPN_CD"]) != "" && Convert.ToString(rowExcel["JPN_CD"]) != null)
        {
            footerString = "外籍會社 " + Convert.ToString(rowExcel["JPN_NAME"]) + "共" + groupCount.ToString("n0") + "戶";
        }
        else
        {
            //202103 現金支付
            if (Convert.ToString(rowExcel["Flag"]) == "5") //現金
            {
                footerString = "現金支付  共" + pageCount.ToString("n0") + "戶";
            }
            else
            {
                footerString = "銀行代碼 " + salary_account_no + "共" + groupCount.ToString("n0") + "戶";
            }            
        }
        //委託機關名稱
        if (Convert.ToString(rowExcel["COMPANY_CD"]) == "K")
            footerCompanyName = Convert.ToString(rowExcel["COMPANY_NAME"]) + "汽車股份有限公司";
        else
            footerCompanyName = Convert.ToString(rowExcel["COMPANY_NAME"]);

        PdfPTable table = new PdfPTable(new float[] { 1, 1, 1, 1, 1, 1, 1, 1, 1 });
        table.TotalWidth = 500f;
        table.LockedWidth = true;
        PdfPCell cell = new PdfPCell();
        //表尾第一行
        cell.Phrase = new Phrase("  ", ChFont);
        cell.Colspan = 9;
        cell.BorderWidthTop = 2;
        cell.BorderWidthRight = 0;
        cell.BorderWidthLeft = 0;
        cell.BorderWidthBottom = 0;
        table.AddCell(cell);
        //表尾第二行
        cell.Phrase = new Phrase("本頁戶數" + "共" + pageCount.ToString("n0") + "戶", ChFont);
        cell.BorderWidthTop = 0;
        cell.BorderWidthRight = 0;
        cell.BorderWidthLeft = 0;
        cell.BorderWidthBottom = 0;
        cell.Colspan = 5;
        table.AddCell(cell);

        cell.Phrase = new Phrase("存入金額小計： ", ChFont);
        cell.BorderWidth = 0;
        cell.Colspan = 2;
        table.AddCell(cell);
        cell.Phrase = new Phrase(pageAmount.ToString("n0") + "元", ChFont);
        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
        cell.BorderWidth = 0;
        cell.Colspan = 2;
        table.AddCell(cell);
        //表尾第三行
        cell.Phrase = new Phrase("委託機關名稱：" + footerCompanyName, ChFont);
        cell.HorizontalAlignment = Element.ALIGN_LEFT;
        cell.BorderWidth = 0;
        cell.Colspan = 9;
        table.AddCell(cell);
        //表尾第四行
        cell.Phrase = new Phrase(footerString, ChFont);
        cell.BorderWidthTop = 0;
        cell.BorderWidthRight = 0;
        cell.BorderWidthLeft = 0;
        cell.BorderWidthBottom = 0;
        cell.Colspan = 5;
        table.AddCell(cell);

        cell.Phrase = new Phrase("金額總計： ", ChFont);
        cell.BorderWidth = 0;
        cell.Colspan = 2;
        table.AddCell(cell);
        cell.Phrase = new Phrase(groupAmount.ToString("n0") + "元", ChFont);
        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
        cell.BorderWidth = 0;
        cell.Colspan = 2;
        table.AddCell(cell);

        doc1.Add(table);

        return currentRow + 4;
    }
    //報表總表尾
    private void createMianFooter(Document doc1, Font ChFont, int AllCount, double AllAmount)
    {
        PdfPTable table = new PdfPTable(new float[] { 1, 1, 1, 1, 1, 1, 1, 1, 1 });
        table.TotalWidth = 500f;
        table.LockedWidth = true;
        PdfPCell cell = new PdfPCell();
        //總表尾第一行
        cell.Phrase = new Phrase("  ", ChFont);
        cell.Colspan = 9;
        cell.BorderWidth = 0;
        table.AddCell(cell);
        //表尾第二行
        cell.Phrase = new Phrase("總計共 " + AllCount.ToString("n0") + "戶", ChFont);
        cell.BorderWidth = 0;
        cell.Colspan = 5;
        table.AddCell(cell);

        cell.Phrase = new Phrase("存入金額總計：");
        cell.BorderWidth = 0;
        cell.Colspan = 2;
        table.AddCell(cell);
        cell.Phrase = new Phrase(AllAmount.ToString("n0") + "元", ChFont);
        cell.HorizontalAlignment = Element.ALIGN_RIGHT;
        cell.BorderWidth = 0;
        cell.Colspan = 2;
        table.AddCell(cell);

        //cell.Phrase = new Phrase("委託機關名稱：國瑞汽車股份有限公司", ChFont);
        //cell.BorderWidth = 0;
        //cell.Colspan = 9;
        //table.AddCell(cell);
        doc1.Add(table);
    }
}

public class TwoColumnHeaderFooter : PdfPageEventHelper
{
    public string path;
    BaseFont bf = null;
    PdfContentByte cb;
    PdfTemplate headerTemplate;

    public override void OnOpenDocument(PdfWriter writer, Document document)
    {
        bf = BaseFont.CreateFont(path, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
        cb = writer.DirectContent;
        headerTemplate = cb.CreateTemplate(100, 100);
    }

    public override void OnEndPage(iTextSharp.text.pdf.PdfWriter writer, iTextSharp.text.Document document)
    {
        base.OnEndPage(writer, document);

        //Create PdfTable object
        PdfPTable pdfTab = new PdfPTable(1);

        //We will have to create separate cells to include image logo and 2 separate strings
        //Row 1
        PdfPCell pdfCell3 = new PdfPCell();
        //String text = "頁次：第" + writer.PageNumber + "頁 共";
        String text = "頁次：" + writer.PageNumber + "/";
        //Add paging to header
        {
            cb.BeginText();
            cb.SetFontAndSize(bf, 12);
            cb.SetTextMatrix(document.PageSize.GetRight(110), document.PageSize.GetTop(45));
            cb.ShowText(text);
            cb.EndText();
            float len = bf.GetWidthPoint(text, 12);
            //Adds "12" in Page 1 of 12
            cb.AddTemplate(headerTemplate, document.PageSize.GetRight(110) + len, document.PageSize.GetTop(45));
        }

        pdfCell3.HorizontalAlignment = Element.ALIGN_RIGHT;
        pdfCell3.VerticalAlignment = Element.ALIGN_MIDDLE;
        pdfCell3.Border = 0;
        pdfTab.AddCell(pdfCell3);
        //pdfTab.TotalWidth = document.PageSize.Width - 50f;
        //pdfTab.WidthPercentage = 100;

    }

    public override void OnCloseDocument(PdfWriter writer, Document document)
    {
        base.OnCloseDocument(writer, document);

        headerTemplate.BeginText();
        headerTemplate.SetFontAndSize(bf, 12);
        headerTemplate.SetTextMatrix(0, 0);
        //headerTemplate.ShowText((writer.PageNumber - 1).ToString() + "頁");
        headerTemplate.ShowText((writer.PageNumber - 1).ToString());
        headerTemplate.EndText();

    }
}