using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using iTextSharp.text;
using iTextSharp.text.pdf;

/// <summary>
/// PdfCreate 的摘要描述
/// </summary>
public class PdfCreate
{
    public string Title { get; set; }

    PdfContentByte cb;

    PdfTemplate template;

    public BaseFont bf { get; set; }


    public PdfCreate()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    //薪資單
    public MemoryStream createPDF_Salary(DataTable dt)
    {
        try
        {
            float tableWidth = 90f;
            float cellWidth = 45f;
            float cellborderwidth = 0.5f;
            float fontsize = 7;

            MemoryStream PDFData = new MemoryStream();
            Document document = new Document(PageSize.A4, 10, 10, 10, 10);
            PdfWriter PDFWriter = PdfWriter.GetInstance(document, PDFData);
            document.Open();

            //浮水印
            PdfContentByte over = PDFWriter.DirectContent;
            over.SaveState();
            float sinus = (float)Math.Sin(Math.PI / 10);
            float cosinus = (float)Math.Cos(Math.PI / 10);

            over.BeginText();
            over.SetTextRenderingMode(PdfContentByte.TEXT_RENDER_MODE_FILL_STROKE);
            over.SetLineWidth(1.5f);
            over.SetRGBColorStroke(0xD3, 0xD3, 0xD3);
            over.SetRGBColorFill(0xFF, 0xFF, 0xFF);
            over.SetFontAndSize(bf, 48f);
            over.SetTextMatrix(cosinus, sinus, -sinus, cosinus, 65, 324);
            over.ShowText("機密文件-國瑞薪資單");
            over.EndText();
            over.RestoreState();


            PdfPTable HeaderTable = new PdfPTable(2);
            HeaderTable.DefaultCell.Border = 1;
            HeaderTable.TotalWidth = tableWidth;
            HeaderTable.SetWidthPercentage(new float[] { cellWidth, cellWidth }, document.PageSize);
            PdfPCell HeaderCell = new PdfPCell(new Phrase("應稅加項(A)", new Font(bf, fontsize)));
            HeaderCell.Border = 1;
            HeaderCell.BorderWidthTop = cellborderwidth;
            HeaderCell.BorderWidthBottom = cellborderwidth;
            HeaderCell.BorderWidthLeft = cellborderwidth;
            HeaderCell.BorderWidthRight = cellborderwidth;
            HeaderCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            HeaderCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            HeaderCell.Colspan = 2;
            HeaderTable.AddCell(HeaderCell);
            //應稅加項(A)第二行開始
            HeaderCell = new PdfPCell(new Phrase("本薪", new Font(bf, fontsize)));
            HeaderCell.Border = 0;
            HeaderCell.BorderWidthLeft = cellborderwidth;
            HeaderCell.BorderWidthRight = cellborderwidth;
            HeaderCell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            HeaderCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            HeaderTable.AddCell(HeaderCell);

            HeaderCell = new PdfPCell(new Phrase("", new Font(bf, fontsize)));
            HeaderCell.Border = 0;
            HeaderCell.BorderWidthRight = cellborderwidth;
            HeaderCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            HeaderCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            HeaderTable.AddCell(HeaderCell);

            

            //應稅加項(A)第二行開始
            HeaderCell = new PdfPCell(new Phrase("職能俸", new Font(bf, fontsize)));
            HeaderCell.Border = 0;
            HeaderCell.BorderWidthLeft = cellborderwidth;
            HeaderCell.BorderWidthRight = cellborderwidth;
            HeaderCell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            HeaderCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            HeaderTable.AddCell(HeaderCell);

            HeaderCell = new PdfPCell(new Phrase("", new Font(bf, fontsize)));
            HeaderCell.Border = 0;
            HeaderCell.BorderWidthRight = cellborderwidth;
            HeaderCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            HeaderCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            HeaderTable.AddCell(HeaderCell);

            //應稅加項(A)第二行開始
            HeaderCell = new PdfPCell(new Phrase("資格俸", new Font(bf, fontsize)));
            HeaderCell.Border = 0;
            HeaderCell.BorderWidthLeft = cellborderwidth;
            HeaderCell.BorderWidthRight = cellborderwidth;
            HeaderCell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            HeaderCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            HeaderTable.AddCell(HeaderCell);

            HeaderCell = new PdfPCell(new Phrase("", new Font(bf, fontsize)));
            HeaderCell.Border = 0;
            HeaderCell.BorderWidthRight = cellborderwidth;
            HeaderCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            HeaderCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            HeaderTable.AddCell(HeaderCell);

            //應稅加項(A)第二行開始
            HeaderCell = new PdfPCell(new Phrase("職務津貼", new Font(bf, fontsize)));
            HeaderCell.Border = 0;
            HeaderCell.BorderWidthLeft = cellborderwidth;
            HeaderCell.BorderWidthRight = cellborderwidth;
            HeaderCell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            HeaderCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            HeaderTable.AddCell(HeaderCell);

            HeaderCell = new PdfPCell(new Phrase("", new Font(bf, fontsize)));
            HeaderCell.Border = 0;
            HeaderCell.BorderWidthRight = cellborderwidth;
            HeaderCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            HeaderCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            HeaderTable.AddCell(HeaderCell);

            //應稅加項(A)第二行開始
            HeaderCell = new PdfPCell(new Phrase("專業津貼", new Font(bf, fontsize)));
            HeaderCell.Border = 0;
            HeaderCell.BorderWidthLeft = cellborderwidth;
            HeaderCell.BorderWidthRight = cellborderwidth;
            HeaderCell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            HeaderCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            HeaderTable.AddCell(HeaderCell);

            HeaderCell = new PdfPCell(new Phrase("", new Font(bf, fontsize)));
            HeaderCell.Border = 0;
            HeaderCell.BorderWidthRight = cellborderwidth;
            HeaderCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            HeaderCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            HeaderTable.AddCell(HeaderCell);

            //應稅加項(A)第二行開始
            HeaderCell = new PdfPCell(new Phrase("交通津貼", new Font(bf, fontsize)));
            HeaderCell.Border = 0;
            HeaderCell.BorderWidthLeft = cellborderwidth;
            HeaderCell.BorderWidthRight = cellborderwidth;
            HeaderCell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            HeaderCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            HeaderTable.AddCell(HeaderCell);

            HeaderCell = new PdfPCell(new Phrase("", new Font(bf, fontsize)));
            HeaderCell.Border = 0;
            HeaderCell.BorderWidthRight = cellborderwidth;
            HeaderCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            HeaderCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            HeaderTable.AddCell(HeaderCell);

            for (int i = 0; i < 50; i++)
            {
                HeaderCell = new PdfPCell(new Phrase("  ", new Font(bf, fontsize)));
                HeaderCell.Border = 0;
                HeaderCell.BorderWidthLeft = cellborderwidth;
                HeaderCell.BorderWidthRight = cellborderwidth;
                HeaderCell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                HeaderCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                HeaderTable.AddCell(HeaderCell);

                HeaderCell = new PdfPCell(new Phrase("", new Font(bf, fontsize)));
                HeaderCell.Border = 0;
                HeaderCell.BorderWidthRight = cellborderwidth;
                HeaderCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                HeaderCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                HeaderTable.AddCell(HeaderCell);
            }

            HeaderCell = new PdfPCell(new Phrase("稅前加項合計(E)=(A)合計", new Font(bf, fontsize)));
            HeaderCell.Border = 1;
            HeaderCell.BorderWidthTop = cellborderwidth;
            HeaderCell.BorderWidthBottom = cellborderwidth;
            HeaderCell.BorderWidthLeft = cellborderwidth;
            HeaderCell.BorderWidthRight = cellborderwidth;
            HeaderCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            HeaderCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            HeaderCell.Colspan = 2;
            HeaderTable.AddCell(HeaderCell);

            HeaderCell = new PdfPCell(new Phrase("應付薪資總計(I)=(E)+(F)", new Font(bf, fontsize)));
            HeaderCell.Border = 1;
            HeaderCell.BorderWidthTop = cellborderwidth;
            HeaderCell.BorderWidthBottom = cellborderwidth;
            HeaderCell.BorderWidthLeft = cellborderwidth;
            HeaderCell.BorderWidthRight = cellborderwidth;
            HeaderCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            HeaderCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            HeaderCell.Colspan = 2;
            HeaderTable.AddCell(HeaderCell);

            HeaderTable.WriteSelectedRows(0, -1, document.Left, document.Top, PDFWriter.DirectContent);

            HeaderTable = new PdfPTable(2);
            HeaderTable.DefaultCell.Border = 1;
            HeaderTable.TotalWidth = tableWidth;
            HeaderTable.SetWidthPercentage(new float[] { cellWidth, cellWidth }, document.PageSize);
            HeaderCell = new PdfPCell(new Phrase("免稅加項(B)", new Font(bf, fontsize)));
            HeaderCell.Border = 0;
            HeaderCell.BorderWidthTop = cellborderwidth;
            HeaderCell.BorderWidthBottom = cellborderwidth;
            HeaderCell.BorderWidthLeft = cellborderwidth;
            HeaderCell.BorderWidthRight = cellborderwidth;
            HeaderCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            HeaderCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            HeaderCell.Colspan = 2;
            HeaderTable.AddCell(HeaderCell);
            //免稅加項(B)第二行開始
            HeaderCell = new PdfPCell(new Phrase("伙食津貼", new Font(bf, fontsize)));
            HeaderCell.Border = 0;
            HeaderCell.BorderWidthLeft = cellborderwidth;
            HeaderCell.BorderWidthRight = cellborderwidth;
            HeaderCell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            HeaderCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            HeaderTable.AddCell(HeaderCell);

            HeaderCell = new PdfPCell(new Phrase("", new Font(bf, fontsize)));
            HeaderCell.Border = 0;
            HeaderCell.BorderWidthRight = cellborderwidth;
            HeaderCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            HeaderCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            HeaderTable.AddCell(HeaderCell);

            for (int i = 0; i < 55; i++)
            {
                HeaderCell = new PdfPCell(new Phrase("  ", new Font(bf, fontsize)));
                HeaderCell.Border = 0;
                HeaderCell.BorderWidthLeft = cellborderwidth;
                HeaderCell.BorderWidthRight = cellborderwidth;
                HeaderCell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                HeaderCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                HeaderTable.AddCell(HeaderCell);

                HeaderCell = new PdfPCell(new Phrase("", new Font(bf, fontsize)));
                HeaderCell.Border = 0;
                HeaderCell.BorderWidthRight = cellborderwidth;
                HeaderCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                HeaderCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                HeaderTable.AddCell(HeaderCell);
            }
            HeaderCell = new PdfPCell(new Phrase("稅後加項合計(F)=(B)合計", new Font(bf, fontsize)));
            HeaderCell.Border = 1;
            HeaderCell.BorderWidthTop = cellborderwidth;
            HeaderCell.BorderWidthBottom = cellborderwidth;
            HeaderCell.BorderWidthLeft = cellborderwidth;
            HeaderCell.BorderWidthRight = cellborderwidth;
            HeaderCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            HeaderCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            HeaderCell.Colspan = 2;
            HeaderTable.AddCell(HeaderCell);

            HeaderCell = new PdfPCell(new Phrase("應扣薪資總計(J)=(G)+(H)", new Font(bf, fontsize)));
            HeaderCell.Border = 1;
            HeaderCell.BorderWidthTop = cellborderwidth;
            HeaderCell.BorderWidthBottom = cellborderwidth;
            HeaderCell.BorderWidthLeft = cellborderwidth;
            HeaderCell.BorderWidthRight = cellborderwidth;
            HeaderCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            HeaderCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            HeaderCell.Colspan = 2;
            HeaderTable.AddCell(HeaderCell);
            HeaderTable.WriteSelectedRows(0, -1, document.Left + tableWidth, document.Top, PDFWriter.DirectContent);


            HeaderTable = new PdfPTable(2);
            HeaderTable.DefaultCell.Border = 1;
            HeaderTable.TotalWidth = tableWidth;
            HeaderTable.SetWidthPercentage(new float[] { cellWidth, cellWidth }, document.PageSize);
            HeaderCell = new PdfPCell(new Phrase("稅前扣項(C)", new Font(bf, fontsize)));
            HeaderCell.Border = 0;
            HeaderCell.BorderWidthTop = cellborderwidth;
            HeaderCell.BorderWidthBottom = cellborderwidth;
            HeaderCell.BorderWidthLeft = cellborderwidth;
            HeaderCell.BorderWidthRight = cellborderwidth;
            HeaderCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            HeaderCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            HeaderCell.Colspan = 2;
            HeaderTable.AddCell(HeaderCell);
            //稅前扣項(C)第二行開始
            HeaderCell = new PdfPCell(new Phrase("勤怠扣款", new Font(bf, fontsize)));
            HeaderCell.Border = 0;
            HeaderCell.BorderWidthLeft = cellborderwidth;
            HeaderCell.BorderWidthRight = cellborderwidth;
            HeaderCell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            HeaderCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            HeaderTable.AddCell(HeaderCell);

            HeaderCell = new PdfPCell(new Phrase("", new Font(bf, fontsize)));
            HeaderCell.Border = 0;
            HeaderCell.BorderWidthRight = cellborderwidth;
            HeaderCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            HeaderCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            HeaderTable.AddCell(HeaderCell);

            for (int i = 0; i < 55; i++)
            {
                HeaderCell = new PdfPCell(new Phrase("  ", new Font(bf, fontsize)));
                HeaderCell.Border = 0;
                HeaderCell.BorderWidthLeft = cellborderwidth;
                HeaderCell.BorderWidthRight = cellborderwidth;
                HeaderCell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                HeaderCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                HeaderTable.AddCell(HeaderCell);

                HeaderCell = new PdfPCell(new Phrase("", new Font(bf, fontsize)));
                HeaderCell.Border = 0;
                HeaderCell.BorderWidthRight = cellborderwidth;
                HeaderCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                HeaderCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                HeaderTable.AddCell(HeaderCell);
            }
            HeaderCell = new PdfPCell(new Phrase("應稅扣款合計(G)=(C)合計", new Font(bf, fontsize)));
            HeaderCell.Border = 1;
            HeaderCell.BorderWidthTop = cellborderwidth;
            HeaderCell.BorderWidthBottom = cellborderwidth;
            HeaderCell.BorderWidthLeft = cellborderwidth;
            HeaderCell.BorderWidthRight = cellborderwidth;
            HeaderCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            HeaderCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            HeaderCell.Colspan = 2;
            HeaderTable.AddCell(HeaderCell);

            HeaderCell = new PdfPCell(new Phrase("上月積欠還款", new Font(bf, fontsize)));
            HeaderCell.Border = 1;
            HeaderCell.BorderWidthTop = cellborderwidth;
            HeaderCell.BorderWidthBottom = cellborderwidth;
            HeaderCell.BorderWidthLeft = cellborderwidth;
            HeaderCell.BorderWidthRight = cellborderwidth;
            HeaderCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            HeaderCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            HeaderCell.Colspan = 2;
            HeaderTable.AddCell(HeaderCell);
            HeaderTable.WriteSelectedRows(0, -1, document.Left + tableWidth * 2, document.Top, PDFWriter.DirectContent);

            HeaderTable = new PdfPTable(2);
            HeaderTable.DefaultCell.Border = 1;
            HeaderTable.TotalWidth = tableWidth;
            HeaderTable.SetWidthPercentage(new float[] { cellWidth, cellWidth }, document.PageSize);
            HeaderCell = new PdfPCell(new Phrase("稅後扣項(D)", new Font(bf, fontsize)));
            HeaderCell.Border = 0;
            HeaderCell.BorderWidthTop = cellborderwidth;
            HeaderCell.BorderWidthBottom = cellborderwidth;
            HeaderCell.BorderWidthLeft = cellborderwidth;
            HeaderCell.BorderWidthRight = cellborderwidth;
            HeaderCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            HeaderCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            HeaderCell.Colspan = 2;
            HeaderTable.AddCell(HeaderCell);
            //稅後扣項(D)第二行開始
            HeaderCell = new PdfPCell(new Phrase("所得稅代扣", new Font(bf, fontsize)));
            HeaderCell.Border = 0;
            HeaderCell.BorderWidthLeft = cellborderwidth;
            HeaderCell.BorderWidthRight = cellborderwidth;
            HeaderCell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            HeaderCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            HeaderTable.AddCell(HeaderCell);

            HeaderCell = new PdfPCell(new Phrase("", new Font(bf, fontsize)));
            HeaderCell.Border = 0;
            HeaderCell.BorderWidthRight = cellborderwidth;
            HeaderCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            HeaderCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            HeaderTable.AddCell(HeaderCell);

            for (int i = 0; i < 55; i++)
            {
                HeaderCell = new PdfPCell(new Phrase("  ", new Font(bf, fontsize)));
                HeaderCell.Border = 0;
                HeaderCell.BorderWidthLeft = cellborderwidth;
                HeaderCell.BorderWidthRight = cellborderwidth;
                HeaderCell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                HeaderCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                HeaderTable.AddCell(HeaderCell);

                HeaderCell = new PdfPCell(new Phrase("", new Font(bf, fontsize)));
                HeaderCell.Border = 0;
                HeaderCell.BorderWidthRight = cellborderwidth;
                HeaderCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                HeaderCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                HeaderTable.AddCell(HeaderCell);
            }
            HeaderCell = new PdfPCell(new Phrase("免稅扣款合計(H)=(D)合計", new Font(bf, fontsize)));
            HeaderCell.Border = 1;
            HeaderCell.BorderWidthTop = cellborderwidth;
            HeaderCell.BorderWidthBottom = cellborderwidth;
            HeaderCell.BorderWidthLeft = cellborderwidth;
            HeaderCell.BorderWidthRight = cellborderwidth;
            HeaderCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            HeaderCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            HeaderCell.Colspan = 2;
            HeaderTable.AddCell(HeaderCell);

            HeaderCell = new PdfPCell(new Phrase("本月積欠", new Font(bf, fontsize)));
            HeaderCell.Border = 1;
            HeaderCell.BorderWidthTop = cellborderwidth;
            HeaderCell.BorderWidthBottom = cellborderwidth;
            HeaderCell.BorderWidthLeft = cellborderwidth;
            HeaderCell.BorderWidthRight = cellborderwidth;
            HeaderCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            HeaderCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            HeaderCell.Colspan = 2;
            HeaderTable.AddCell(HeaderCell);
            HeaderTable.WriteSelectedRows(0, -1, document.Left + tableWidth * 3, document.Top, PDFWriter.DirectContent);

            HeaderTable = new PdfPTable(2);
            HeaderTable.DefaultCell.Border = 1;
            HeaderTable.TotalWidth = tableWidth +20f;
            HeaderTable.SetWidthPercentage(new float[] { cellWidth + 20f, cellWidth }, document.PageSize);
            HeaderCell = new PdfPCell(new Phrase("勤怠紀錄", new Font(bf, fontsize)));
            HeaderCell.Border = 0;
            HeaderCell.BorderWidthTop = cellborderwidth;
            HeaderCell.BorderWidthBottom = cellborderwidth;
            HeaderCell.BorderWidthLeft = cellborderwidth;
            HeaderCell.BorderWidthRight = cellborderwidth;
            HeaderCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            HeaderCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            HeaderCell.Colspan = 2;
            HeaderTable.AddCell(HeaderCell);
            //勤怠紀錄第二行開始
            HeaderCell = new PdfPCell(new Phrase("平日加班1.5.倍", new Font(bf, fontsize)));
            HeaderCell.Border = 0;
            HeaderCell.BorderWidthLeft = cellborderwidth;
            HeaderCell.BorderWidthRight = cellborderwidth;
            HeaderCell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            HeaderCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            HeaderTable.AddCell(HeaderCell);

            HeaderCell = new PdfPCell(new Phrase("", new Font(bf, fontsize)));
            HeaderCell.Border = 0;
            HeaderCell.BorderWidthRight = cellborderwidth;
            HeaderCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            HeaderCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            HeaderTable.AddCell(HeaderCell);

            for (int i = 0; i < 55; i++)
            {
                HeaderCell = new PdfPCell(new Phrase("  ", new Font(bf, fontsize)));
                HeaderCell.Border = 0;
                HeaderCell.BorderWidthLeft = cellborderwidth;
                HeaderCell.BorderWidthRight = cellborderwidth;
                HeaderCell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                HeaderCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                HeaderTable.AddCell(HeaderCell);

                HeaderCell = new PdfPCell(new Phrase("", new Font(bf, fontsize)));
                HeaderCell.Border = 0;
                HeaderCell.BorderWidthRight = cellborderwidth;
                HeaderCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                HeaderCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                HeaderTable.AddCell(HeaderCell);
            }
            HeaderCell = new PdfPCell(new Phrase(" ", new Font(bf, fontsize)));
            HeaderCell.Border = 1;
            HeaderCell.BorderWidthTop = cellborderwidth;
            HeaderCell.BorderWidthBottom = cellborderwidth;
            HeaderCell.BorderWidthLeft = cellborderwidth;
            HeaderCell.BorderWidthRight = cellborderwidth;
            HeaderCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            HeaderCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            HeaderCell.Colspan = 2;
            HeaderTable.AddCell(HeaderCell);

            HeaderCell = new PdfPCell(new Phrase("實發金額", new Font(bf, fontsize)));
            HeaderCell.Border = 1;
            HeaderCell.BorderWidthTop = cellborderwidth;
            HeaderCell.BorderWidthBottom = cellborderwidth;
            HeaderCell.BorderWidthLeft = cellborderwidth;
            HeaderCell.BorderWidthRight = cellborderwidth;
            HeaderCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            HeaderCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            HeaderCell.Colspan = 2;
            HeaderTable.AddCell(HeaderCell);
            HeaderTable.WriteSelectedRows(0, -1, document.Left + tableWidth * 4, document.Top, PDFWriter.DirectContent);

            HeaderTable = new PdfPTable(2);
            HeaderTable.DefaultCell.Border = 1;
            HeaderTable.TotalWidth = tableWidth + 20f;
            HeaderTable.SetWidthPercentage(new float[] { cellWidth + 20f, cellWidth }, document.PageSize);
            HeaderCell = new PdfPCell(new Phrase("公司提撥退休金", new Font(bf, fontsize)));
            HeaderCell.Border = 0;
            HeaderCell.BorderWidthTop = cellborderwidth;
            HeaderCell.BorderWidthBottom = cellborderwidth;
            HeaderCell.BorderWidthLeft = cellborderwidth;
            HeaderCell.BorderWidthRight = cellborderwidth;
            HeaderCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            HeaderCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            HeaderCell.Colspan = 2;
            HeaderTable.AddCell(HeaderCell);
            //公司提撥退休金第二行開始
            HeaderCell = new PdfPCell(new Phrase("提撥率", new Font(bf, fontsize)));
            HeaderCell.Border = 0;
            HeaderCell.BorderWidthLeft = cellborderwidth;
            HeaderCell.BorderWidthRight = cellborderwidth;
            HeaderCell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            HeaderCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            HeaderTable.AddCell(HeaderCell);

            HeaderCell = new PdfPCell(new Phrase("", new Font(bf, fontsize)));
            HeaderCell.Border = 0;
            HeaderCell.BorderWidthRight = cellborderwidth;
            HeaderCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            HeaderCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            HeaderTable.AddCell(HeaderCell);


            for (int i = 0; i < 5; i++)
            {
                HeaderCell = new PdfPCell(new Phrase("  ", new Font(bf, fontsize)));
                HeaderCell.Border = 0;
                HeaderCell.BorderWidthLeft = cellborderwidth;
                HeaderCell.BorderWidthRight = cellborderwidth;
                HeaderCell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                HeaderCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                HeaderTable.AddCell(HeaderCell);

                HeaderCell = new PdfPCell(new Phrase("", new Font(bf, fontsize)));
                HeaderCell.Border = 0;
                HeaderCell.BorderWidthRight = cellborderwidth;
                HeaderCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                HeaderCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                HeaderTable.AddCell(HeaderCell);
            }

            HeaderTable.WriteSelectedRows(0, -1, document.Left + tableWidth * 5 +20f, document.Top, PDFWriter.DirectContent);


            HeaderTable = new PdfPTable(2);
            HeaderTable.DefaultCell.Border = 1;
            HeaderTable.TotalWidth = tableWidth + 20f;
            HeaderTable.SetWidthPercentage(new float[] { cellWidth +20f, cellWidth }, document.PageSize);
            HeaderCell = new PdfPCell(new Phrase("(健保)補充保費說明", new Font(bf, fontsize)));
            HeaderCell.Border = 0;
            HeaderCell.BorderWidthTop = cellborderwidth;
            HeaderCell.BorderWidthBottom = cellborderwidth;
            HeaderCell.BorderWidthLeft = cellborderwidth;
            HeaderCell.BorderWidthRight = cellborderwidth;
            HeaderCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            HeaderCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            HeaderCell.Colspan = 2;
            HeaderTable.AddCell(HeaderCell);
            //公司提撥退休金第二行開始
            HeaderCell = new PdfPCell(new Phrase("健保月投保金額", new Font(bf, fontsize)));
            HeaderCell.Border = 0;
            HeaderCell.BorderWidthLeft = cellborderwidth;
            HeaderCell.BorderWidthRight = cellborderwidth;
            HeaderCell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
            HeaderCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            HeaderTable.AddCell(HeaderCell);

            HeaderCell = new PdfPCell(new Phrase("", new Font(bf, fontsize)));
            HeaderCell.Border = 0;
            HeaderCell.BorderWidthRight = cellborderwidth;
            HeaderCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            HeaderCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            HeaderTable.AddCell(HeaderCell);


            for (int i = 0; i < 48; i++)
            {
                HeaderCell = new PdfPCell(new Phrase("  ", new Font(bf, fontsize)));
                HeaderCell.Border = 0;
                HeaderCell.BorderWidthLeft = cellborderwidth;
                HeaderCell.BorderWidthRight = cellborderwidth;
                HeaderCell.HorizontalAlignment = PdfPCell.ALIGN_LEFT;
                HeaderCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                HeaderTable.AddCell(HeaderCell);

                HeaderCell = new PdfPCell(new Phrase("", new Font(bf, fontsize)));
                HeaderCell.Border = 0;
                HeaderCell.BorderWidthRight = cellborderwidth;
                HeaderCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
                HeaderCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
                HeaderTable.AddCell(HeaderCell);
            }
            HeaderCell = new PdfPCell(new Phrase(" ", new Font(bf, fontsize)));
            HeaderCell.Border = 1;
            HeaderCell.BorderWidthTop = cellborderwidth;
            HeaderCell.BorderWidthBottom = cellborderwidth;
            HeaderCell.BorderWidthLeft = cellborderwidth;
            HeaderCell.BorderWidthRight = cellborderwidth;
            HeaderCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            HeaderCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            HeaderCell.Colspan = 2;
            HeaderTable.AddCell(HeaderCell);

            HeaderCell = new PdfPCell(new Phrase(" ", new Font(bf, fontsize)));
            HeaderCell.Border = 1;
            HeaderCell.BorderWidthTop = cellborderwidth;
            HeaderCell.BorderWidthBottom = cellborderwidth;
            HeaderCell.BorderWidthLeft = cellborderwidth;
            HeaderCell.BorderWidthRight = cellborderwidth;
            HeaderCell.HorizontalAlignment = PdfPCell.ALIGN_CENTER;
            HeaderCell.VerticalAlignment = PdfPCell.ALIGN_MIDDLE;
            HeaderCell.Colspan = 2;
            HeaderTable.AddCell(HeaderCell);
            HeaderTable.WriteSelectedRows(0, -1, document.Left + tableWidth * 5 + 20f, document.Top - 77f, PDFWriter.DirectContent);

            document.Close();

            byte[] bytes = PDFData.ToArray();
            MemoryStream input = new MemoryStream(bytes);
            PdfReader reader = new PdfReader(input);
            MemoryStream output = new MemoryStream();


            PdfEncryptor.Encrypt(reader, output, true, "1234", "1234", PdfWriter.ALLOW_SCREENREADERS);
            return output;
        }
        catch (Exception)
        {

            throw;
        }
    }
}