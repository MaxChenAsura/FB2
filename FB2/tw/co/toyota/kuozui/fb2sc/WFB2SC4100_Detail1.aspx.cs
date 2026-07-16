using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using iTextSharp.tool.xml;
using System.Text;

public class UnicodeFontFactory : FontFactoryImp
{
    private static readonly string arialFontPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "arialuni.ttf");//arial unicode MS是完整的unicode字型。 
    private static readonly string cPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Fonts), "KAIU.TTF");//標楷體 
    public override Font GetFont(string fontname, string encoding, bool embedded, float size, int style, BaseColor color,
        bool cached)
    {
        //可用Arial或標楷體，自己選一個 
        BaseFont baseFont = BaseFont.CreateFont(cPath, BaseFont.IDENTITY_H, BaseFont.EMBEDDED);
        return new Font(baseFont, size, style, color);
    }

}

public partial class WebContent_fb2sc_WFB2SC4100_Detail1 : BasePage
{
    WFB2SC4100BO bo = new WFB2SC4100BO();
    string IS_SUPER;
    string SALARY_TYPE;
    DateTime SALARY_DT;
    string EMP_ID;
    string PAY_KIND;
    string SALARY_EMAIL;
    string SALARY_YM;
    string SALARY_PAY_METHOD ="";
    string BANK = "";
    string ACCOUNT_NO = "";
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.RegisterClientScriptBlock(this.Page, this.GetType(), "init", "iniForm();", true);
        IS_SUPER = Server.UrlDecode(this.Request.QueryString["IS_SUPER"]);
        SALARY_TYPE = Server.UrlDecode(this.Request.QueryString["SALARY_TYPE"]);
        SALARY_DT = Convert.ToDateTime(Server.UrlDecode(this.Request.QueryString["SALARY_DT"]));
        EMP_ID = Server.UrlDecode(this.Request.QueryString["EMP_ID"]);
        PAY_KIND = Server.UrlDecode(this.Request.QueryString["PAY_KIND"]);
        SALARY_EMAIL = Server.UrlDecode(this.Request.QueryString["SALARY_EMAIL"]);
        SALARY_YM = Server.UrlDecode(this.Request.QueryString["SALARY_YM"]);
        WFB2SC4100DtlDAO dao;
        if (SALARY_TYPE.ToUpper() == "A")
        {
            dao = bo.GetDetailHeaderByTypeA(SALARY_DT, SALARY_TYPE, EMP_ID, PAY_KIND);
            DataTable SDT_EDT = bo.GetTB_S_M_DUTY_RESULT_H_SDT_EDT(SALARY_DT);
            if (SDT_EDT.Rows.Count > 0)
            {
                hid_SALARY_TYPE.Value = SALARY_TYPE;
                hid_SALARY_DT.Value = SALARY_DT.ToString("yyyyMMdd");
                hid_SALARY_YM.Value = SALARY_YM;
                hid_EMP_ID.Value = EMP_ID;
                hid_DATA_SDT.Value = Convert.ToDateTime(SDT_EDT.Rows[0]["DATA_SDT"]).ToString("yyyyMMdd");
                hid_DATA_EDT.Value = Convert.ToDateTime(SDT_EDT.Rows[0]["DATA_EDT"]).ToString("yyyyMMdd");

                //WFB2SC4100OOverDtl.Attributes.Add("onclick", "window.showModalDialog('/WebContent/fb2di/WFB2di0700_Qry.aspx?EMP_ID=" + EMP_ID + "&DATA_SDT=" + Convert.ToDateTime(SDT_EDT.Rows[0]["DATA_SDT"]).ToString("yyyyMMdd") + "&DATA_EDT=" + Convert.ToDateTime(SDT_EDT.Rows[0]["DATA_EDT"]).ToString("yyyyMMdd") + "&parentFuncId=" + hid_parentFuncId.Value + "', self, 'dialogWidth=1020px;dialogHeight=700px;scroll=auto');");
                //WFB2SC4100LeaveDtl.Attributes.Add("onclick", "window.showModalDialog('/WebContent/fb2dh/WFB2dh0700_Qry.aspx?EMP_ID=" + EMP_ID + "&DATA_SDT=" + Convert.ToDateTime(SDT_EDT.Rows[0]["DATA_SDT"]).ToString("yyyyMMdd") + "&DATA_EDT=" + Convert.ToDateTime(SDT_EDT.Rows[0]["DATA_EDT"]).ToString("yyyyMMdd") + "&parentFuncId=" + hid_parentFuncId.Value + "', self, 'dialogWidth=1020px;dialogHeight=700px;scroll=auto');");
                //WFB2SC4100ShiftDtl.Attributes.Add("onclick", "window.showModalDialog('/WebContent/fb2dc/WFB2DC0800_Qry.aspx?EMP_ID=" + EMP_ID + "&DATA_SDT=" + Convert.ToDateTime(SDT_EDT.Rows[0]["DATA_SDT"]).ToString("yyyyMMdd") + "&DATA_EDT=" + Convert.ToDateTime(SDT_EDT.Rows[0]["DATA_EDT"]).ToString("yyyyMMdd") + "&parentFuncId=" + hid_parentFuncId.Value + "', self, 'dialogWidth=1020px;dialogHeight=700px;scroll=auto');");
            }
            //WFB2SC4100InsDtl.Attributes.Add("onclick", "window.showModalDialog('/WebContent/fb2ia/WFB2IA5200_Qry.aspx?EMP_ID=" + EMP_ID + "&SALARY_YM=" + SALARY_DT.ToString("yyyyMM") + "&parentFuncId=" + hid_parentFuncId.Value + "', self, 'dialogWidth=1020px;dialogHeight=700px;scroll=auto');");
            //WFB2SC4100EnvDtl.Attributes.Add("onclick", "window.showModalDialog('/WebContent/fb2dj/WFB2DJ0400_Qry.aspx?EMP_ID=" + EMP_ID + "&SALARY_DT=" + SALARY_DT.ToString("yyyyMMdd") + "&parentFuncId=" + hid_parentFuncId.Value + "', self, 'dialogWidth=1020px;dialogHeight=700px;scroll=auto');");
            WFB2SC4100InsDtl.Visible = true;
            WFB2SC4100OOverDtl.Visible = true;
            WFB2SC4100LeaveDtl.Visible = true;
            WFB2SC4100ShiftDtl.Visible = true;
        }
        else
        {
            dao = bo.GetDetailHeaderByTypeNotA(SALARY_DT, SALARY_TYPE, EMP_ID, PAY_KIND);
            WFB2SC4100InsDtl.Visible = false;
            WFB2SC4100OOverDtl.Visible = false;
            WFB2SC4100LeaveDtl.Visible = false;
            WFB2SC4100ShiftDtl.Visible = false;
        }
        if (dao != null)
        {
            lb_EMP_NAME_Value.Text = dao.EMP_NAME;
            lb_EMP_NO_Value.Text = EMP_ID;
            lb_ON_DEPT_Value.Text = dao.DEPT_NAME_40;
            lb_Payroll_YM_Value.Text = dao.SALARY_YM;
            lb_REMIT_DT1_Value.Text = dao.REMIT_DT.ToString("yyyy/MM/dd");
                     
            lb_SALARY_PAY_METHOD_Value.Text = dao.SALARY_PAY_METHOD_Value;
            if (dao.SALARY_PAY_METHOD  == "C") //現金支付
            {
                SALARY_PAY_METHOD = "C";
                lb_To_Bank_Name_Value.Text = "";
                lb_SALARY_ACCOUNT_NO_Value.Text = "";   
            }
            else
            {
                lb_To_Bank_Name_Value.Text = dao.TITLE;
                lb_SALARY_ACCOUNT_NO_Value.Text = dao.SALARY_ACCOUNT_NO;   

            }
            
        }
        GrantSalary_DetailHeader1();
        GrantSalary_DetailHeader2();
        GrantSalary_Detail_Content();

        if (Convert.ToString(Request.QueryString["PDF"]) == "Y")
            PrintPDF();
    }

    private void PrintPDF()
    {
        var pdfFile = new Document(PageSize.A4.Rotate(), 10, 10, 10, 10);
        MemoryStream MS = new MemoryStream();
        BaseFont bf = BaseFont.CreateFont(Server.MapPath("~/Fonts/kaiu.ttf"), BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);
        float fontsize = 12;
        int border = 0;
        PdfWriter PW = PdfWriter.GetInstance(pdfFile, MS);
        pdfFile.Open();
        PdfPTable HeaderTable = new PdfPTable(7);
        HeaderTable.DefaultCell.Border = border;
        HeaderTable.TotalWidth = 800f;
        HeaderTable.SetWidthPercentage(new float[] { 25f, 8f, 18f, 5f, 18f, 8f, 18f }, pdfFile.PageSize);
        HeaderTable.LockedWidth = true;
        //Logo
        iTextSharp.text.Image bmp = iTextSharp.text.Image.GetInstance(Server.MapPath("~/images/kzlogo-c.bmp"));
        bmp.ScaleToFit(210, 150f);

        PdfPCell HeaderCell = new PdfPCell(bmp);
        HeaderCell.PaddingTop = 3f;
        HeaderCell.PaddingLeft = -12f;
        HeaderCell.Colspan = 0;
        HeaderCell.Rowspan = 3;
        HeaderCell.Border = border;
        HeaderTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
        HeaderTable.AddCell(HeaderCell);
       
        //PdfPCell HeaderTitle = new PdfPCell(new Phrase(SALARY_DT.ToString("yyyy") + "年" + SALARY_DT.ToString("MM") + "月薪資單", new Font(bf, fontsize)));
        PdfPCell HeaderTitle = new PdfPCell(new Phrase(SALARY_YM.Substring(0, 4) + "年" + (SALARY_YM.Replace("/","")).Substring(4,2) + "月薪資單", new Font(bf, fontsize)));
        HeaderTitle.Colspan = 6;
        HeaderTitle.Border = border;
        HeaderTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
        HeaderTable.AddCell(HeaderTitle);

        PdfPCell HeaderEMPNamelbl = new PdfPCell(new Phrase("員工姓名:", new Font(bf, fontsize)));
        HeaderEMPNamelbl.Colspan = 0;
        HeaderEMPNamelbl.Border = border;
        HeaderTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        HeaderTable.AddCell(HeaderEMPNamelbl);

        PdfPCell HeaderEMPName = new PdfPCell(new Phrase(lb_EMP_NAME_Value.Text, new Font(bf, fontsize)));
        HeaderEMPName.Colspan = 0;
        HeaderEMPName.Border = border;
        HeaderTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
        HeaderTable.AddCell(HeaderEMPName);

        PdfPCell HeaderEMPNOlbl = new PdfPCell(new Phrase("工號:", new Font(bf, fontsize)));
        HeaderEMPNOlbl.Colspan = 0;
        HeaderEMPNOlbl.Border = border;
        HeaderTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        HeaderTable.AddCell(HeaderEMPNOlbl);

        PdfPCell HeaderEMPNO = new PdfPCell(new Phrase(lb_EMP_NO_Value.Text, new Font(bf, fontsize)));
        HeaderEMPNO.Colspan = 0;
        HeaderEMPNO.Border = border;
        HeaderTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
        HeaderTable.AddCell(HeaderEMPNO);

        PdfPCell HeaderDEPTlbl = new PdfPCell(new Phrase("任用單位:", new Font(bf, fontsize)));
        HeaderDEPTlbl.Colspan = 0;
        HeaderDEPTlbl.Border = border;
        HeaderTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        HeaderTable.AddCell(HeaderDEPTlbl);

        PdfPCell HeaderDEPT = new PdfPCell(new Phrase(lb_ON_DEPT_Value.Text, new Font(bf, fontsize)));
        HeaderDEPT.Colspan = 0;
        HeaderDEPT.Border = border;
        HeaderTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
        HeaderTable.AddCell(HeaderDEPT);
        
        PdfPCell HeaderBankNamelbl = new PdfPCell(new Phrase("匯款銀行:", new Font(bf, fontsize)));
        HeaderBankNamelbl.Colspan = 0;
        HeaderBankNamelbl.Border = border;
        HeaderTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        HeaderTable.AddCell(HeaderBankNamelbl);

        if (SALARY_PAY_METHOD == "C")
        {
            BANK = "";
            ACCOUNT_NO = "";
        }
        else
        {
            BANK = lb_To_Bank_Name_Value.Text;
            ACCOUNT_NO = lb_SALARY_ACCOUNT_NO_Value.Text;
        }
        SALARY_PAY_METHOD = "";
        PdfPCell HeaderBankName = new PdfPCell(new Phrase(BANK, new Font(bf, fontsize)));
        //PdfPCell HeaderBankName = new PdfPCell(new Phrase("BANK", new Font(bf, fontsize)));
        HeaderBankName.Colspan = 0;
        HeaderBankName.Border = border;
        HeaderTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
        HeaderTable.AddCell(HeaderBankName);

        PdfPCell HeaderBankAcclbl = new PdfPCell(new Phrase("帳號:", new Font(bf, fontsize)));
        HeaderBankAcclbl.Colspan = 0;
        HeaderBankAcclbl.Border = border;
        HeaderTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        HeaderTable.AddCell(HeaderBankAcclbl);

        PdfPCell HeaderBankAcc = new PdfPCell(new Phrase(ACCOUNT_NO, new Font(bf, fontsize)));
        //PdfPCell HeaderBankAcc = new PdfPCell(new Phrase("54545555", new Font(bf, fontsize)));
        HeaderBankAcc.Colspan = 0;
        HeaderBankAcc.Border = border;
        HeaderTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
        HeaderTable.AddCell(HeaderBankAcc);

        PdfPCell HeaderToAccDatelbl = new PdfPCell(new Phrase("匯款日期:", new Font(bf, fontsize)));
        HeaderToAccDatelbl.Colspan = 0;
        HeaderToAccDatelbl.Border = border;
        HeaderTable.DefaultCell.HorizontalAlignment = Element.ALIGN_RIGHT;
        HeaderTable.AddCell(HeaderToAccDatelbl);

        PdfPCell HeaderToAccDate = new PdfPCell(new Phrase(lb_REMIT_DT1_Value.Text, new Font(bf, fontsize)));
        HeaderToAccDate.Colspan = 0;
        HeaderToAccDate.Border = border;
        HeaderTable.DefaultCell.HorizontalAlignment = Element.ALIGN_LEFT;
        HeaderTable.AddCell(HeaderToAccDate);

        pdfFile.Add(HeaderTable);

        MemoryStream outputStream = new MemoryStream();//要把PDF寫到哪個串流 
        byte[] data = Encoding.UTF8.GetBytes(ConvertSalaryToPdfFormat());//字串轉成byte[] 
        MemoryStream msInput = new MemoryStream(data);
        XMLWorkerHelper.GetInstance().ParseXHtml(PW, pdfFile, msInput, null, Encoding.UTF8, new UnicodeFontFactory());

        pdfFile.Close();
        byte[] bytes = MS.ToArray();
        MemoryStream input = new MemoryStream(bytes);
        PdfReader reader = new PdfReader(input);
        MemoryStream output = new MemoryStream();
        string LICENSE_ID = bo.getLICENSE_ID(lb_EMP_NO_Value.Text);
        PdfEncryptor.Encrypt(reader, output, true, LICENSE_ID, LICENSE_ID, PdfWriter.ALLOW_SCREENREADERS);
        //PdfEncryptor.Encrypt(reader, output, true, "123", "123", PdfWriter.ALLOW_SCREENREADERS);


        string FileName = lb_EMP_NO_Value.Text;

        Response.Clear();
        Response.AddHeader("Content-Disposition", string.Format("attachment; filename=" + HttpUtility.UrlEncode(FileName) + ".pdf"));
        Response.ContentType = "application/pdf; name=" + HttpUtility.UrlEncode(FileName) + ".pdf";
        Response.OutputStream.Write(output.GetBuffer(), 0, output.GetBuffer().Length);
        Response.OutputStream.Flush();
        Response.OutputStream.Close();
        Response.Flush();
        output.Dispose();
        Response.End();
        Response.Close();
    }

    private void GrantSalary_DetailHeader1()
    {
        HtmlTableRow SalaryHearder1 = new HtmlTableRow();
        SalaryHearder1.BgColor = "#ff0000";
        SalaryHearder1.Style.Add("color", "#000000");

        SalaryHearder1.Cells.Add(GrantHeader1Cell1("應稅加項(A)"));
        SalaryHearder1.Cells.Add(GrantHeader1Cell1("免稅加項(B)"));
        SalaryHearder1.Cells.Add(GrantHeader1Cell1("稅前扣項(C)"));
        SalaryHearder1.Cells.Add(GrantHeader1Cell1("稅後扣項(D)"));
        if (SALARY_TYPE.ToUpper() == "A")
        {
            SalaryHearder1.Cells.Add(GrantHeader1Cell1("勤怠紀錄"));
            SalaryHearder1.Cells.Add(GrantHeader1Cell1("公司提撥退休金"));
        }
        else
        {
            SalaryHearder1.Cells.Add(GrantHeader1Cell1("發放內容說明"));
        }
        tbSalary_Detail.Rows.Add(SalaryHearder1);
    }

    private void GrantSalary_DetailHeader2()
    {

        HtmlTableRow SalaryHearder2 = new HtmlTableRow();
        SalaryHearder2.BgColor = "#7F7F7F";
        SalaryHearder2.Style.Add("color", "#000000");
        SalaryHearder2.Style.Add("font-weight", "bold");
        string width1 = string.Empty;
        string width2 = string.Empty;
        if (SALARY_TYPE.ToUpper() == "A")
        {
            width1 = "10%";
            width2 = "6.67%";
        }
        else
        {
            width1 = "12%";
            width2 = "8%";
        }
        SalaryHearder2.Cells.Add(GrantHeader1Cell2("項目", width1));
        SalaryHearder2.Cells.Add(GrantHeader1Cell2("金額", width2));
        SalaryHearder2.Cells.Add(GrantHeader1Cell2("項目", width1));
        SalaryHearder2.Cells.Add(GrantHeader1Cell2("金額", width2));
        SalaryHearder2.Cells.Add(GrantHeader1Cell2("項目", width1));
        SalaryHearder2.Cells.Add(GrantHeader1Cell2("金額", width2));
        SalaryHearder2.Cells.Add(GrantHeader1Cell2("項目", width1));
        SalaryHearder2.Cells.Add(GrantHeader1Cell2("金額", width2));
        if (SALARY_TYPE.ToUpper() == "A")
        {
            SalaryHearder2.Cells.Add(GrantHeader1Cell2("項目", width1));
            SalaryHearder2.Cells.Add(GrantHeader1Cell2("內容", width2));
            SalaryHearder2.Cells.Add(GrantHeader1Cell2("項目", width1));
            SalaryHearder2.Cells.Add(GrantHeader1Cell2("內容", width2));
        }
        else
        {
            SalaryHearder2.Cells.Add(GrantHeader1Cell2("項目", width1));
            SalaryHearder2.Cells.Add(GrantHeader1Cell2("內容", width2));
        }
        tbSalary_Detail.Rows.Add(SalaryHearder2);
    }

    private void GrantSalary_Detail_Content()
    {
        DataTable dt = bo.GetShouldBeAddedData(SALARY_DT, SALARY_TYPE, EMP_ID, PAY_KIND, "Y", 1);
        decimal ShouldBeAddedSum = 0;
        if (dt.Rows.Count > 0)
            ShouldBeAddedSum = (decimal)dt.Compute("Sum(AMOUNT)", string.Empty);
        GrantBlockData(dt, 0, 2, "SALARY_NAME", "AMOUNT", "#,0", "");

        dt = bo.GetShouldBeAddedData(SALARY_DT, SALARY_TYPE, EMP_ID, PAY_KIND, "N", 1);

        decimal ShouldBeAddedFreeTaxSum = 0;
        if (dt.Rows.Count > 0)
            ShouldBeAddedFreeTaxSum = (decimal)dt.Compute("Sum(AMOUNT)", string.Empty);
        GrantBlockData(dt, 2, 2, "SALARY_NAME", "AMOUNT", "#,0", "");

        dt = bo.GetShouldBeAddedData(SALARY_DT, SALARY_TYPE, EMP_ID, PAY_KIND, "Y", -1);
        decimal ShouldBeLessSum = 0;
        if (dt.Rows.Count > 0)
            ShouldBeLessSum = (decimal)dt.Compute("Sum(AMOUNT)", string.Empty);
        GrantBlockData(dt, 4, 2, "SALARY_NAME", "AMOUNT", "#,0", "");

        dt = bo.GetShouldBeAddedData(SALARY_DT, SALARY_TYPE, EMP_ID, PAY_KIND, "N", -1);
        decimal ShouldBeLessFreeTaxSum = 0;
        if (dt.Rows.Count > 0)
            ShouldBeLessFreeTaxSum = (decimal)dt.Compute("Sum(AMOUNT)", string.Empty);

        GrantBlockData(dt, 6, 2, "SALARY_NAME", "AMOUNT", "#,0", "");
        if (SALARY_TYPE.ToUpper() == "A")
            GrantSalary_Detail_KindA(ShouldBeAddedSum, ShouldBeAddedFreeTaxSum, ShouldBeLessSum, ShouldBeLessFreeTaxSum);
        else
        {
            if (SALARY_TYPE.ToUpper() == "C" && (PAY_KIND == "1031" || PAY_KIND == "1033"))
                GrantSalary_Deatil_KindC();
            if (SALARY_TYPE.ToUpper() == "B")
                GrantSalary_Deatil_KindC();
        }
        GrantFoot1(ShouldBeAddedSum, ShouldBeAddedFreeTaxSum, ShouldBeLessSum, ShouldBeLessFreeTaxSum);
        GrantFoot2(ShouldBeAddedSum, ShouldBeAddedFreeTaxSum, ShouldBeLessSum, ShouldBeLessFreeTaxSum);

    }

    private string ConvertSalaryToPdfFormat()
    {
        //tbSalary_Detail.Style.Remove("border");
        tbSalary_Detail.Width = "100%";
        int i = 0;
        foreach (HtmlTableRow row in tbSalary_Detail.Rows)
        {
            row.BgColor = "";
            row.Style.Remove("color");
            foreach (HtmlTableCell cell in row.Cells)
            {
                cell.BgColor = "";
                cell.Style.Remove("color");
                cell.Style.Remove("border");
                cell.Style.Add("border", "2px solid #000");
                cell.Style.Remove("border-width");
                cell.Style.Remove("border-color");
                if (i < 2)
                {
                    cell.Style.Add("border", "1px solid #000");
                }
                else if (i > tbSalary_Detail.Rows.Count - 4)
                {
                    cell.Style.Add("border", "1px solid #000");
                }
                else
                {
                    cell.Style.Add("border-width", "0px 1px 0px 1px");
                }
                cell.Style.Add("border-color", "#000");
                cell.Style.Add("border-collapse", "collapse");
            }
            i++;
        }
        StringWriter sw = new StringWriter();
        HtmlTextWriter htw = new HtmlTextWriter(sw);
        tbSalary_Detail.RenderControl(htw);
        return sw.ToString();
    }

    private void GrantSalary_Detail_KindA(decimal ShouldBeAddedSum, decimal ShouldBeAddedFreeTaxSum, decimal ShouldBeLessSum, decimal ShouldBeLessFreeTaxSum)
    {
        DataTable dt = bo.GetOverTime(EMP_ID, SALARY_DT);
        int startRowIndex = 2;
        GrantBlockData(dt, 8, startRowIndex, "DESC1", "DESC2", "#,0.00", "時");

        startRowIndex += dt.Rows.Count;
        dt = bo.GetLeave(EMP_ID, SALARY_DT);
        GrantBlockData(dt, 8, startRowIndex, "SUB_LEAVE_DESC", "DESC2", "", "");

        startRowIndex += dt.Rows.Count;
        dt = bo.GetWorkShift(EMP_ID, SALARY_DT);
        GrantBlockData(dt, 8, startRowIndex, "DESC1", "DESC2", "#,0.00", "天");


        startRowIndex += dt.Rows.Count;
        dt = bo.GetAvailableLeave(EMP_ID, SALARY_DT);
        GrantBlockData(dt, 8, startRowIndex, "DESC1", "DESC2", "#,0.00", "時");

        dt = bo.GetPension(EMP_ID, SALARY_DT);
        if (dt.Rows.Count > 0)
        {
            GrantBlockData(dt, 10, 2, "[月提繳工資]", "INS_AMT", "#,0", "");
            GrantBlockData(dt, 10, 3, "[公司提撥率]", "COMP_RATE", "0.00", "%");
            GrantBlockData(dt, 10, 4, "[提撥金額]", "SELF_D_AMT", "#,0", "");
            GrantBlockData(dt, 10, 5, "[員工自提率]", "SLEF_RATE", "0.00", "%");
            GrantBlockData(dt, 10, 6, "[提撥金額]", "INS_FEES", "#,0", "");
        }
        else
        {
            if (tbSalary_Detail.Rows.Count < 3)
            {                
                GrantBlockData(dt, 10, 2, "[月提繳工資]", "", "#,0", "");
            }
            else
            {
                tbSalary_Detail.Rows[2].Cells.Add(GrantContentCell("月提繳工資", "left"));
                tbSalary_Detail.Rows[2].Cells.Add(GrantContentCell("&nbsp;", "right"));
            }
            if (tbSalary_Detail.Rows.Count < 4)
            {
                GrantBlockData(dt, 10, 3, "[公司提撥率]", "", "0.00", "");                         
            }
            else
            {
                tbSalary_Detail.Rows[3].Cells.Add(GrantContentCell("公司提撥率", "left"));
                tbSalary_Detail.Rows[3].Cells.Add(GrantContentCell("&nbsp;", "right"));      
            }
            if (tbSalary_Detail.Rows.Count < 5)
            {
                GrantBlockData(dt, 10, 4, "[提撥金額]", "", "#,0", "");                 
            }
            else
            {
                tbSalary_Detail.Rows[4].Cells.Add(GrantContentCell("提撥金額", "left"));
                tbSalary_Detail.Rows[4].Cells.Add(GrantContentCell("&nbsp;", "right"));           
            }
            if (tbSalary_Detail.Rows.Count < 6)
            {
                GrantBlockData(dt, 10, 5, "[員工自提率]", "", "0.00", "");                       
            }
            else
            {
                tbSalary_Detail.Rows[5].Cells.Add(GrantContentCell("員工自提率", "left"));
                tbSalary_Detail.Rows[5].Cells.Add(GrantContentCell("&nbsp;", "right"));    
            }
            if (tbSalary_Detail.Rows.Count < 7)
            {
                GrantBlockData(dt, 10, 6, "[提撥金額]", "", "#,0", "");
            }
            else
            {
                tbSalary_Detail.Rows[6].Cells.Add(GrantContentCell("提撥金額", "left"));
                tbSalary_Detail.Rows[6].Cells.Add(GrantContentCell("&nbsp;", "right"));    
            }
            

            //tbSalary_Detail.Rows[2].Cells.Add(GrantContentCell("月投保金額", "left"));
            //tbSalary_Detail.Rows[2].Cells.Add(GrantContentCell("&nbsp;", "left"));          
            //tbSalary_Detail.Rows[3].Cells.Add(GrantContentCell("公司提撥率", "left"));
            //tbSalary_Detail.Rows[3].Cells.Add(GrantContentCell("&nbsp;", "left"));
            //tbSalary_Detail.Rows[4].Cells.Add(GrantContentCell("提撥金額", "left"));
            //tbSalary_Detail.Rows[4].Cells.Add(GrantContentCell("&nbsp;", "left"));
            //tbSalary_Detail.Rows[5].Cells.Add(GrantContentCell("員工自提率", "left"));
            //tbSalary_Detail.Rows[5].Cells.Add(GrantContentCell("&nbsp;", "left"));
            //tbSalary_Detail.Rows[6].Cells.Add(GrantContentCell("提撥金額", "left"));
            //tbSalary_Detail.Rows[6].Cells.Add(GrantContentCell("&nbsp;", "left"));
        }
        if (tbSalary_Detail.Rows.Count < 8)
            tbSalary_Detail.Rows.Add(new HtmlTableRow());
        while (tbSalary_Detail.Rows[7].Cells.Count != 10)
        {
            if (tbSalary_Detail.Rows[7].Cells.Count < 10)
                tbSalary_Detail.Rows[7].Cells.Add(GrantContentCell("&nbsp;", "left"));
            else
                tbSalary_Detail.Rows[7].Cells.RemoveAt(tbSalary_Detail.Rows[7].Cells.Count - 1);
        }
        tbSalary_Detail.Rows[7].Cells.Add(GrantHeader1Cell1("(健保)補充保費說明"));

        dt = bo.GetINS2(EMP_ID, SALARY_DT);
        if (dt.Rows.Count > 0)
        {
            GrantBlockData(dt, 10, 8, "[月投保金額]", "INS_MONTH_AMOUNT", ("#,0"), "");
            GrantBlockData(dt, 10, 9, "[全年累計獎金]", "ACCU_AMOUNT", ("#,0"), "");
            GrantBlockData(dt, 10, 10, "[本月計費費基]", "INS_COST_BASE", ("#,0"), "");
        }
        else
        {
            if (tbSalary_Detail.Rows.Count < 11)
            {
                int rowCount = tbSalary_Detail.Rows.Count;
                for (int i = 0; i < 11 - rowCount; i++)
                {
                    HtmlTableRow row = new HtmlTableRow();
                    tbSalary_Detail.Rows.Add(row);
                    for (int j = 0; j <= 10; j++)
                    {
                        tbSalary_Detail.Rows[rowCount + i].Cells.Add(GrantContentCell("&nbsp;", "left"));
                    }
                }
            }
            if (tbSalary_Detail.Rows[8].Cells.Count == 12)
                tbSalary_Detail.Rows[8].Cells.RemoveAt(11);
            if (tbSalary_Detail.Rows[8].Cells.Count == 11)
                tbSalary_Detail.Rows[8].Cells.RemoveAt(10);
            tbSalary_Detail.Rows[8].Cells.Add(GrantContentCell("月投保金額", "left"));
            tbSalary_Detail.Rows[8].Cells.Add(GrantContentCell("&nbsp;", "left"));
            if (tbSalary_Detail.Rows[9].Cells.Count == 12)
                tbSalary_Detail.Rows[9].Cells.RemoveAt(11);
            if (tbSalary_Detail.Rows[9].Cells.Count == 11)
                tbSalary_Detail.Rows[9].Cells.RemoveAt(10);
            tbSalary_Detail.Rows[9].Cells.Add(GrantContentCell("全年累計獎金", "left"));
            tbSalary_Detail.Rows[9].Cells.Add(GrantContentCell("&nbsp;", "left"));
            if (tbSalary_Detail.Rows[10].Cells.Count == 12)
                tbSalary_Detail.Rows[10].Cells.RemoveAt(11);
            if (tbSalary_Detail.Rows[10].Cells.Count == 11)
                tbSalary_Detail.Rows[10].Cells.RemoveAt(10);
            tbSalary_Detail.Rows[10].Cells.Add(GrantContentCell("本月計費費基", "left"));
            tbSalary_Detail.Rows[10].Cells.Add(GrantContentCell("&nbsp;", "left"));
        }
    }

    private void GrantSalary_Deatil_KindC()
    {
        DataTable dt = new DataTable();
        if (PAY_KIND == "1031")
            dt = bo.GetItemByTypeC_1031(EMP_ID, SALARY_DT);
        if (PAY_KIND == "1033")
            dt = bo.GetItemByTypeC_1033(EMP_ID, SALARY_DT);

        if (dt.Rows.Count > 0)
        {
            int ProcessRowIndex = 2;
            //年獎發放回數(若畫面.發薪類別(隱藏欄位) = 'C'(獎金類) 且畫面.發放項目(隱藏欄位)='1031'(年終獎金) 才顯示)
            if (PAY_KIND == "1031" && dt.Rows[0]["AWARD_ROUND"] != DBNull.Value && Convert.ToDecimal(dt.Rows[0]["AWARD_ROUND"]) > 0)
            {
                GrantBlockData(dt, 8, ProcessRowIndex, "[年獎發放回數]", "AWARD_ROUND", "#,0", "回");
                ProcessRowIndex++;
            }
            //考績(業績)(若畫面.發薪類別(隱藏欄位) = 'C'(獎金類) 且畫面.發放項目(隱藏欄位)='1031'(年終獎金)才顯示)					
            if (PAY_KIND == "1031" && Convert.ToString(dt.Rows[0]["SCORE_2H"]) != string.Empty)
            {
                GrantBlockData(dt, 8, ProcessRowIndex, "[考績(業績)]", "SCORE_2H", "", "");
                ProcessRowIndex++;
            }
            if (dt.Rows[0]["ABILITY_PAY"] != DBNull.Value && Convert.ToDecimal(dt.Rows[0]["ABILITY_PAY"]) > 0)
            {
                DataTable dt2B = bo.Get2B( EMP_ID,  SALARY_DT,  SALARY_TYPE,  PAY_KIND);
                if (dt2B.Rows.Count>0)
                {
                    if (PAY_KIND == "1031" && dt2B.Rows[0]["is2B"].ToString() =="Y" && dt2B.Rows[0]["is1031"].ToString() == "Y")
                    {
                        GrantBlockData(dt, 8, ProcessRowIndex, "[本薪(C2)]", "ABILITY_PAY", "#,0", "元");
                    }else if (PAY_KIND == "1031" && dt2B.Rows[0]["is2B"].ToString() == "Y" && dt2B.Rows[0]["is1031"].ToString() == "N")
                    {
                        GrantBlockData(dt, 8, ProcessRowIndex, "[本薪]", "ABILITY_PAY", "#,0", "元");
                    }
                    else if (PAY_KIND == "1033" && dt2B.Rows[0]["is2B"].ToString() == "Y")
                    {
                        GrantBlockData(dt, 8, ProcessRowIndex, "[本薪]", "ABILITY_PAY", "#,0", "元");
                    }
                    else
                    {
                        GrantBlockData(dt, 8, ProcessRowIndex, "[職能俸]", "ABILITY_PAY", "#,0", "元");
                    }
                }
                else
                {
                    GrantBlockData(dt, 8, ProcessRowIndex, "[職能俸]", "ABILITY_PAY", "#,0", "元");
                }                
                ProcessRowIndex++;
            }
            if (dt.Rows[0]["LEVEL_PAY"] != DBNull.Value && Convert.ToDecimal(dt.Rows[0]["LEVEL_PAY"]) > 0)
            {
                GrantBlockData(dt, 8, ProcessRowIndex, "[資格俸]", "LEVEL_PAY", "#,0", "元");
                ProcessRowIndex++;
            }
            if (dt.Rows[0]["PJOB_PAY"] != DBNull.Value && Convert.ToDecimal(dt.Rows[0]["PJOB_PAY"]) > 0)
            {
                GrantBlockData(dt, 8, ProcessRowIndex, "[職務津貼]", "PJOB_PAY", "#,0", "元");
                ProcessRowIndex++;
            }
            if (dt.Rows[0]["PROFESSION_PAY"] != DBNull.Value && Convert.ToDecimal(dt.Rows[0]["PROFESSION_PAY"]) > 0)
            {
                GrantBlockData(dt, 8, ProcessRowIndex, "[專業津貼]", "PROFESSION_PAY", "#,0", "元");
                ProcessRowIndex++;
            }
            if (dt.Rows[0]["FOOD_SUBSIDY"] != DBNull.Value && Convert.ToDecimal(dt.Rows[0]["FOOD_SUBSIDY"]) > 0)
            {
                GrantBlockData(dt, 8, ProcessRowIndex, "[伙食津貼]", "FOOD_SUBSIDY", "#,0", "元");
                ProcessRowIndex++;
            }
            if (dt.Rows[0]["LEAVE_Q_HOUR"] != DBNull.Value && Convert.ToDecimal(dt.Rows[0]["LEAVE_Q_HOUR"]) > 0)
            {
                GrantBlockData(dt, 8, ProcessRowIndex, "[曠工時數]", "LEAVE_Q_HOUR", "#,0", "時");
                ProcessRowIndex++;
            }
            if (dt.Rows[0]["LEAVE_OP_HOUR"] != DBNull.Value && Convert.ToDecimal(dt.Rows[0]["LEAVE_OP_HOUR"]) > 0)
            {
                GrantBlockData(dt, 8, ProcessRowIndex, "[遲到/早退]", "LEAVE_OP_HOUR", "#,0", "次");
                ProcessRowIndex++;
            }
            if (dt.Rows[0]["THIRD_CNT_P"] != DBNull.Value && Convert.ToDecimal(dt.Rows[0]["THIRD_CNT_P"]) > 0)
            {
                GrantBlockData(dt, 8, ProcessRowIndex, "[嘉獎]", "THIRD_CNT_P", "#,0", "次");
                ProcessRowIndex++;
            }
            if (dt.Rows[0]["SECOND_CNT_P"] != DBNull.Value && Convert.ToDecimal(dt.Rows[0]["SECOND_CNT_P"]) > 0)
            {
                GrantBlockData(dt, 8, ProcessRowIndex, "[小功]", "SECOND_CNT_P", "#,0", "次");
                ProcessRowIndex++;
            }
            if (dt.Rows[0]["FIRST_CNT_P"] != DBNull.Value && Convert.ToDecimal(dt.Rows[0]["FIRST_CNT_P"]) > 0)
            {
                GrantBlockData(dt, 8, ProcessRowIndex, "[大功]", "FIRST_CNT_P", "#,0", "次");
                ProcessRowIndex++;
            }
            if (dt.Rows[0]["THIRD_CNT_M"] != DBNull.Value && Convert.ToDecimal(dt.Rows[0]["THIRD_CNT_M"]) > 0)
            {
                GrantBlockData(dt, 8, ProcessRowIndex, "[申誡]", "THIRD_CNT_M", "#,0", "次");
                ProcessRowIndex++;
            }
            if (dt.Rows[0]["SECOND_CNT_M"] != DBNull.Value && Convert.ToDecimal(dt.Rows[0]["SECOND_CNT_M"]) > 0)
            {
                GrantBlockData(dt, 8, ProcessRowIndex, "[小過]", "SECOND_CNT_M", "#,0", "次");
                ProcessRowIndex++;
            }
            if (dt.Rows[0]["FIRST_CNT_M"] != DBNull.Value && Convert.ToDecimal(dt.Rows[0]["FIRST_CNT_M"]) > 0)
            {
                GrantBlockData(dt, 8, ProcessRowIndex, "[大過]", "FIRST_CNT_M", "#,0", "次");
                ProcessRowIndex++;
            }
            if (dt.Rows[0]["ATTEND_DAYS"] != DBNull.Value && Convert.ToDecimal(dt.Rows[0]["ATTEND_DAYS"]) > 0)
            {
                GrantBlockData(dt, 8, ProcessRowIndex, "[在職天數(1/1~12/31)]", "ATTEND_DAYS", "#,0", "天");
                ProcessRowIndex++;
            }
        }
        //terry add
        else
        {
            tbSalary_Detail.Rows[2].Cells.Add(GrantContentCell("&nbsp;", "left"));
            tbSalary_Detail.Rows[2].Cells.Add(GrantContentCell("&nbsp;", "right"));
        }
    }
    private void GrantFoot2(decimal ShouldBeAddedSum, decimal ShouldBeAddedFreeTaxSum, decimal ShouldBeLessSum, decimal ShouldBeLessFreeTaxSum)
    {
        HtmlTableRow footer1 = new HtmlTableRow();
        footer1.Cells.Add(GrantFooter2Cell("應付薪資總計(E)=(A)+(B)", "left", "90%", "black"));
        footer1.Cells.Add(GrantFooter2Cell((ShouldBeAddedSum + ShouldBeAddedFreeTaxSum).ToString("#,0"), "right", "100%", "black"));
        footer1.Cells.Add(GrantFooter2Cell("應扣薪資總計(F)=(C)+(D)", "left", "90%", "black"));
        footer1.Cells.Add(GrantFooter2Cell((ShouldBeLessSum + ShouldBeLessFreeTaxSum).ToString("#,0"), "right", "100%", "red"));
        footer1.Cells.Add(GrantFooter2Cell("&nbsp;", "right", "100%", "black"));
        if (SALARY_TYPE.ToUpper() == "A")
        {
            footer1.Cells.Add(GrantFooter2Cell("&nbsp;", "right", "100%", "black"));
        }
        tbSalary_Detail.Rows.Add(footer1);

        HtmlTableRow footer2 = new HtmlTableRow();
        footer2.Cells.Add(GrantFooter2Cell("實發金額(G)=(E)-(F)", "left", "90%", "black"));
        footer2.Cells.Add(GrantFooter2Cell(((ShouldBeAddedSum + ShouldBeAddedFreeTaxSum) - (ShouldBeLessSum + ShouldBeLessFreeTaxSum)).ToString("#,0"), "right", "100%", "black"));
        footer2.Cells.Add(GrantFooter2Cell("&nbsp;", "right", "100%", "black"));
        footer2.Cells.Add(GrantFooter2Cell("&nbsp;", "right", "100%", "black"));
        footer2.Cells.Add(GrantFooter2Cell("&nbsp;", "right", "100%", "black"));
        if (SALARY_TYPE.ToUpper() == "A")
        {
            footer2.Cells.Add(GrantFooter2Cell("&nbsp;", "right", "100%", "black"));
        }
        tbSalary_Detail.Rows.Add(footer2);
    }
    private void GrantFoot1(decimal ShouldBeAddedSum, decimal ShouldBeAddedFreeTaxSum, decimal ShouldBeLessSum, decimal ShouldBeLessFreeTaxSum)
    {
        HtmlTableRow footer1 = new HtmlTableRow();
        footer1.Cells.Add(GrantFooterCell("小計(A)", "left", "black"));
        footer1.Cells.Add(GrantFooterCell(ShouldBeAddedSum.ToString("#,0"), "right", "black"));
        footer1.Cells.Add(GrantFooterCell("小計(B)", "left", "black"));
        footer1.Cells.Add(GrantFooterCell(ShouldBeAddedFreeTaxSum.ToString("#,0"), "right", "black"));
        footer1.Cells.Add(GrantFooterCell("小計(C)", "left", "black"));
        footer1.Cells.Add(GrantFooterCell(ShouldBeLessSum.ToString("#,0"), "right", "red"));
        footer1.Cells.Add(GrantFooterCell("小計(D)", "left", "black"));
        footer1.Cells.Add(GrantFooterCell(ShouldBeLessFreeTaxSum.ToString("#,0"), "right", "red"));
        footer1.Cells.Add(GrantFooterCell("&nbsp;", "right", "black"));
        footer1.Cells.Add(GrantFooterCell("&nbsp;", "right", "black"));
        if (SALARY_TYPE.ToUpper() == "A")
        {
            footer1.Cells.Add(GrantFooterCell("&nbsp;", "right", "black"));
            footer1.Cells.Add(GrantFooterCell("&nbsp;", "right", "black"));
        }
        tbSalary_Detail.Rows.Add(footer1);

    }

    private void GrantBlockData(DataTable Dt, int StartCol, int StartRow, string Title, string Value, string ValueFormat, string ValueAfter)
    {
        HtmlTableRow ProcessRow;
        foreach (DataRow row in Dt.Rows)
        {
            if (tbSalary_Detail.Rows.Count < Dt.Rows.IndexOf(row) + StartRow + 1)
            {
                ProcessRow = new HtmlTableRow();
                tbSalary_Detail.Rows.Add(ProcessRow);
            }
            else
                ProcessRow = tbSalary_Detail.Rows[Dt.Rows.IndexOf(row) + StartRow];
            if (ProcessRow.Cells.Count < StartCol)
            {
                for (int i = ProcessRow.Cells.Count; i < StartCol; i++)
                    ProcessRow.Cells.Add(GrantContentCell("&nbsp;", "left"));
            }
            if (ProcessRow.Cells.Count - 1 > StartCol)
            {
                for (int i = ProcessRow.Cells.Count - 1; i >= StartCol; i--)
                    ProcessRow.Cells.RemoveAt(i);
            }
            if (Title.Contains("[") && Title.Contains("]"))
                ProcessRow.Cells.Add(GrantContentCell(Title.Trim('[').Trim(']'), "left"));
            else
                ProcessRow.Cells.Add(GrantContentCell(Convert.ToString(row[Title]), "left"));
            string FillValue = string.Empty;
            string[] arrCol = Value.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            foreach (string col in arrCol)
            {
                if (Array.IndexOf(arrCol, col) == 0)
                {
                    if (ValueFormat == string.Empty)
                        FillValue += Convert.ToString(row[col]) + ValueAfter;
                    else
                        FillValue += Convert.ToDecimal(row[col]).ToString(ValueFormat) + ValueAfter;
                }
                else
                    FillValue += Convert.ToString(row[col]);
            }
            ProcessRow.Cells.Add(GrantContentCell(FillValue, "right"));
        }
        if (Title.Contains("[") && Title.Contains("]") && Dt.Rows.Count == 0)
        {
            if (tbSalary_Detail.Rows.Count <= StartRow)
                tbSalary_Detail.Rows.Add(new HtmlTableRow());

            ProcessRow = tbSalary_Detail.Rows[StartRow];
            while (ProcessRow.Cells.Count < StartCol)
            {
                ProcessRow.Cells.Add(GrantContentCell("&nbsp;", "left"));
                if (ProcessRow.Cells.Count > 20)
                    break;
            }
            ProcessRow.Cells.Add(GrantContentCell(Title.Trim('[').Trim(']'), "left"));
            ProcessRow.Cells.Add(GrantContentCell("&nbsp;", "left"));
        }

        if (tbSalary_Detail.Rows.Count > Dt.Rows.Count + StartRow)
        {
            for (int i = Dt.Rows.Count + StartRow; i < tbSalary_Detail.Rows.Count; i++)
            {
                if (tbSalary_Detail.Rows[i].Cells.Count < StartCol + 2)
                {
                    tbSalary_Detail.Rows[i].Cells.Add(GrantContentCell("&nbsp;", "left"));
                    tbSalary_Detail.Rows[i].Cells.Add(GrantContentCell("&nbsp;", "right"));
                }
            }
        }
    }

    private HtmlTableCell GrantContentCell(string CellContent, string Align)
    {
        HtmlTableCell ContentCell = new HtmlTableCell();
        ContentCell.InnerHtml = CellContent;
        ContentCell.Align = Align;
        ContentCell.Style.Add("border-width", "0px 1px 0px 1px");
        ContentCell.Style.Add("border-style", "solid");
        ContentCell.Style.Add("border-color", "#000");
        ContentCell.Style.Add("border-collapse", "collapse");
        return ContentCell;
    }

    private HtmlTableCell GrantHeader1Cell1(String CellContent)
    {
        HtmlTableCell Header1Cell1 = new HtmlTableCell();
        Header1Cell1.ColSpan = 2;
        Header1Cell1.Style.Add("color", "White");
        Header1Cell1.BgColor = "#ff0000";
        Header1Cell1.Align = "center";
        Header1Cell1.Style.Add("border", "1px solid #000");

        Header1Cell1.InnerHtml = CellContent;
        return Header1Cell1;
    }

    private HtmlTableCell GrantHeader1Cell2(String CellContent, string CellWidth)
    {
        HtmlTableCell Header1Cell2 = new HtmlTableCell();
        Header1Cell2.Style.Add("color", "White");
        Header1Cell2.Width = CellWidth;
        Header1Cell2.BgColor = "#7F7F7F";
        Header1Cell2.Align = "center";
        Header1Cell2.Style.Add("border", "1px solid #000");
        Header1Cell2.InnerHtml = CellContent;
        return Header1Cell2;
    }

    private HtmlTableCell GrantFooterCell(String CellContent, string Align, string clolor)
    {
        HtmlTableCell FooterCell = new HtmlTableCell();
        FooterCell.Style.Add("color", clolor);
        FooterCell.BgColor = "#EAEAEA";
        FooterCell.Align = Align;
        FooterCell.Style.Add("border", "1px solid #000");
        FooterCell.InnerHtml = CellContent;
        return FooterCell;
    }
    private HtmlTableCell GrantFooter2Cell(String CellContent, string Align, string Font_size, string clolor)
    {
        HtmlTableCell FooterCell = new HtmlTableCell();
        FooterCell.ColSpan = 2;
        FooterCell.BgColor = "#EAEAEA";
        FooterCell.Align = Align;
        FooterCell.Style.Add("color", clolor);
        FooterCell.Style.Add("font-size", Font_size);
        FooterCell.Style.Add("border", "1px solid #000");
        FooterCell.InnerHtml = CellContent;
        return FooterCell;
    }
    protected void WFB2SC4100Email_Click(object sender, EventArgs e)
    {
        try
        {
            string salaryYM = lb_Payroll_YM_Value.Text;
            salaryYM = salaryYM.Substring(0, 4) + "年" + salaryYM.Substring(4, 2) + "月";//薪資年月
            string yy = salaryYM.Substring(0,4);
            string mm = salaryYM.Substring(4,2);
            string ym = salaryYM.Substring(0, 4) + "年" + salaryYM.Substring(4, 2) + "月";

            DataTable ReSendData = null;
            //(A)若發放項目(PAY_KIND) = '1031'(年獎),則
            if (PAY_KIND == "1031")
                ReSendData = bo.GetReSendDataBy1031(SALARY_TYPE, SALARY_DT);
            //(A)若發放項目(PAY_KIND) = '1033'(紅利),則 by eva add 2015/8/12 補上紅利也要發送mail 邏輯
            if (PAY_KIND == "1033")
                ReSendData = bo.GetReSendDataBy1033(SALARY_TYPE, SALARY_DT);
            //(B)若發放項目(PAY_KIND) = '1035'(節金)或 '1032'(一時金)或 '1062'(優退金) 或'1056'(遣散費)或'1070'(離職金),
            if (PAY_KIND == "1035" || PAY_KIND == "1032" || PAY_KIND == "1062" || PAY_KIND == "1056" || PAY_KIND == "1070")
                ReSendData = bo.GetReSendDataBy1035_1032_1062_1056_1070(SALARY_TYPE, SALARY_DT);
            //(C)若發放項目(PAY_KIND) = '9999'(月薪資) 或 '1061'(預付薪)或 '1038'(先發金) 或'1039'(期滿金),則以 畫面發薪類別(隱藏欄位)+發薪日期(隱藏欄位)
            if (PAY_KIND == "9999" || PAY_KIND == "1061" || PAY_KIND == "1038" || PAY_KIND == "1039")
                ReSendData = bo.GetReSendDataBy9999_1061_1038_1039(SALARY_TYPE, SALARY_DT);
            //(D)若發放項目(PAY_KIND) = '1035'(節金),則以 畫面發薪類別(隱藏欄位)+發薪日期(隱藏欄位)+發放項目(PAY_KIND)讀取 薪資計算主檔(TB_S_M_SALARY_CAL_H) 取得MAIL主旨及內文等資料,
            if (PAY_KIND == "1035")
                ReSendData = bo.GetReSendDataBy1035(SALARY_TYPE, SALARY_DT);
            if (ReSendData != null && ReSendData.Rows.Count > 0) {                
                bo.ReSent(Convert.ToString(ReSendData.Rows[0]["DESC1"]),                    
                          Convert.ToString(ReSendData.Rows[0]["DESC2"]),
                          SALARY_DT,
                          SALARY_TYPE,
                          PAY_KIND,
                          EMP_ID,
                          SALARY_EMAIL);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Success", "alert(' 薪資單重送作業完成!!預計於隔日重新發送!!');", true);
            }      

        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "error", "alert('" + ex.Message + "');", true);
        }
    }
    protected void btn_back_Click(object sender, EventArgs e)
    {
        Session["SC4100_Is_Search"] = "Y";
        Response.Redirect("WFB2SC4100_Qry.aspx");
    }
}