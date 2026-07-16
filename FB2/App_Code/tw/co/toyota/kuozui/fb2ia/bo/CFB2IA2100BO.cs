using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;
using iTextSharp.text;
using iTextSharp.text.pdf;



/// <summary>
/// CFB2IA2100BO 的摘要描述
/// </summary>
public class CFB2IA2100BO : BaseService
{
    public CFB2IA2100BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public string Add(CFB2IA2100DAO fb2ia)
    {
        try
        {
            //取得現有資料(檢查重複)
            DataTable tmp = fb2ia.getExistData();
            BeginTransaction();
            if (tmp.Rows.Count > 0)
            {
                return "資料重覆!";
            }
            else if (fb2ia.CheckINS_ENTRY_DT() > 0)
            {
                return "日期區間不可重覆!";
            }
            else
            {
                fb2ia.Add();
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
    public string Update(CFB2IA2100DAO fb2ia, string edititem_list)
    {
        try
        {
            BeginTransaction();
            fb2ia.Update(edititem_list);
            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    public string Delete(List<string> delitem_list, List<string> IDENTITY_KIND_list, List<string> LICENSE_ID_list, List<string> GINS_KIND_list, List<string> INS_ENTRY_DT_list, string EMP_ID)
    {
        CFB2IA2100DAO fb2ia = new CFB2IA2100DAO();
        try
        {
            DataTable tmp;
            for (int i = 0; i < IDENTITY_KIND_list.Count; i++)
            {
                string IDENTITY_KIND = IDENTITY_KIND_list[i];
                string LICENSE_ID = LICENSE_ID_list[i];
                string GINS_KIND = GINS_KIND_list[i];
                string INS_ENTRY_DT = INS_ENTRY_DT_list[i];
                string delitem = delitem_list[i];
                tmp = fb2ia.checkDelData(IDENTITY_KIND, LICENSE_ID, GINS_KIND, INS_ENTRY_DT, EMP_ID);
                BeginTransaction();
                if (tmp.Rows.Count > 0)
                {
                    return "此筆資料,已計算過團保費用,不允刪除";


                }
                else
                {

                    fb2ia.Delete(delitem);

                }
            }

            Commit();

            return "0";
        }
        catch (Exception)
        {
            throw;
        }
    }
    public MemoryStream createPDF(string fontpath, DataTable dt)
    {
        CFB2IA2100DAO fb2ia = new CFB2IA2100DAO();
        try
        {


            var doc1 = new Document(PageSize.A4, 50, 50, 50, 50); //設定pagesize級margin left,right,top,bottom
            TwoColumnHeaderFooter pdf = new TwoColumnHeaderFooter();
            pdf.path = fontpath;
            MemoryStream fileStream = new MemoryStream();
            PdfWriter pdfwr = PdfWriter.GetInstance(doc1, fileStream);
            pdfwr.PageEvent = pdf;
            //字型設定
            BaseFont bfChinese = BaseFont.CreateFont(fontpath, BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
            Font ChFont = new Font(bfChinese, 12, Font.NORMAL, BaseColor.BLACK);
            Font ChFont_title = new Font(bfChinese, 20, Font.BOLD, BaseColor.BLACK);
            //行數
            int count = 0;


            //DataTable dt = fb2ia.pdf_data();
            if (dt.Rows.Count > 0)
            {
                doc1.Open();

                PdfPTable table = new PdfPTable(new float[] { 1, 2, 1, 2, 2, 2, 1 });
                table.TotalWidth = 500f;
                table.LockedWidth = true;
                PdfPCell cell = new PdfPCell();
                cell.HorizontalAlignment = Element.ALIGN_CENTER;
                cell.Phrase = new Phrase("團保子女滿25歲、父母滿85歲未退保清單", ChFont_title);
                cell.BorderWidth = 0;
                cell.Colspan = 7;
                table.AddCell(cell);
                cell.HorizontalAlignment = Element.ALIGN_LEFT;
                cell.Phrase = new Phrase(" ");
                table.AddCell(cell);
                cell.BorderWidthBottom = 1;
                cell.Phrase = new Phrase("工號", ChFont);
                cell.Colspan = 1;
                table.AddCell(cell);
                cell.Phrase = new Phrase("姓名", ChFont);
                table.AddCell(cell);
                cell.Phrase = new Phrase("對象", ChFont);
                table.AddCell(cell);
                cell.Phrase = new Phrase("被保險人", ChFont);
                table.AddCell(cell);
                cell.Phrase = new Phrase("出生日期", ChFont);
                table.AddCell(cell);
                cell.Phrase = new Phrase("加保日", ChFont);
                table.AddCell(cell);
                cell.Phrase = new Phrase("退保日", ChFont);
                table.AddCell(cell);
                cell.BorderWidthBottom = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    if (i == dt.Rows.Count - 1)
                        cell.BorderWidthBottom = 1;
                    cell.Phrase = new Phrase(dt.Rows[i]["EMP_ID"].ToString(), ChFont);
                    table.AddCell(cell);
                    cell.Phrase = new Phrase(dt.Rows[i]["EMP_NAME"].ToString(), ChFont);
                    table.AddCell(cell);
                    cell.Phrase = new Phrase(dt.Rows[i]["SUB_DESC"].ToString(), ChFont);
                    table.AddCell(cell);
                    cell.Phrase = new Phrase(dt.Rows[i]["FAMILY_NAME"].ToString(), ChFont);
                    table.AddCell(cell);
                    cell.Phrase = new Phrase(Convert.ToDateTime(dt.Rows[i]["FAMILY_BIRTH_DT"]).ToString("yyyy/MM/dd"), ChFont);
                    table.AddCell(cell);
                    cell.Phrase = new Phrase(Convert.ToDateTime(dt.Rows[i]["INS_ENTRY_DT"]).ToString("yyyy/MM/dd"), ChFont);
                    table.AddCell(cell);
                    cell.Phrase = new Phrase(" ");
                    table.AddCell(cell);
                    if (i > 41)
                        count++;
                    if (i == 41 || (i > 41 && count % 42 == 0))
                    {
                        cell.HorizontalAlignment = Element.ALIGN_CENTER;
                        cell.Phrase = new Phrase("團保子女滿25歲、父母滿85歲未退保清單", ChFont_title);
                        cell.BorderWidth = 0;
                        cell.Colspan = 7;
                        table.AddCell(cell);
                        cell.HorizontalAlignment = Element.ALIGN_LEFT;
                        cell.Phrase = new Phrase(" ");
                        table.AddCell(cell);
                        cell.BorderWidthBottom = 1;
                        cell.Phrase = new Phrase("工號", ChFont);
                        cell.Colspan = 1;
                        table.AddCell(cell);
                        cell.Phrase = new Phrase("姓名", ChFont);
                        table.AddCell(cell);
                        cell.Phrase = new Phrase("對象", ChFont);
                        table.AddCell(cell);
                        cell.Phrase = new Phrase("被保險人", ChFont);
                        table.AddCell(cell);
                        cell.Phrase = new Phrase("出生日期", ChFont);
                        table.AddCell(cell);
                        cell.Phrase = new Phrase("加保日", ChFont);
                        table.AddCell(cell);
                        cell.Phrase = new Phrase("退保日", ChFont);
                        table.AddCell(cell);
                        cell.BorderWidthBottom = 0;
                    }


                }

                doc1.Add(table);
                doc1.Close();
                //System.Web.HttpContext.Current.Response.Clear();
                //System.Web.HttpContext.Current.Response.ClearHeaders();
                //System.Web.HttpContext.Current.Response.ClearContent();
                //System.Web.HttpContext.Current.Response.HeaderEncoding = System.Text.Encoding.GetEncoding("big5");
                ////System.Web.HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml";
                //System.Web.HttpContext.Current.Response.ContentType = "application/octet-stream";
                //System.Web.HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + HttpUtility.UrlEncode("FB2IA210.pdf"));
                //System.Web.HttpContext.Current.Response.BinaryWrite(fileStream.ToArray());
                //System.Web.HttpContext.Current.Response.Buffer = false;
                //fileStream.Close();
                //fileStream.Dispose();
                //System.Web.HttpContext.Current.Response.End();
            }
            //else
            //{
            //    return "無匯出資料!";
            //}
            return fileStream;
        }
        catch
        {
            throw;
        }
    }
    //pdf的header&footer
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
            string now = DateTime.Now.ToString("yyyy/MM/dd");
            String text = now+ " 頁次：" + writer.PageNumber + "/";

            //Add paging to header
            {
                cb.BeginText();
                cb.SetFontAndSize(bf, 12);
                cb.SetTextMatrix(document.PageSize.GetRight(145), document.PageSize.GetTop(45));
                cb.ShowText(text);
                cb.EndText();
                float len = bf.GetWidthPoint(text, 12);
                //Adds "12" in Page 1 of 12
                cb.AddTemplate(headerTemplate, document.PageSize.GetRight(145) + len, document.PageSize.GetTop(45));
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
            headerTemplate.ShowText((writer.PageNumber - 1).ToString());
            headerTemplate.EndText();

        }
    }
}