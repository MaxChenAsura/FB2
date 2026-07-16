using log4net;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
//using Microsoft.SqlServer.Dts.Runtime;
using iTextSharp.text.pdf;
using iTextSharp.text;
using System.IO;
using System.Data;

public partial class WebContent_Default : BasePage
{

    ILog logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
    protected void Page_Load(object sender, EventArgs e)
    {
        //this.UCCommCodeDropDwonList.CommCodeDataBind();
        logger.Info("PageLoad");

        //DBConnector.DBConnector dbs = new DBConnector.DBConnector(utilities.connstr);
        //List<SqlCommand> listComm = new List<SqlCommand>();

        //StringBuilder sb = new StringBuilder();
        //sb.Append("Update TB_H_M_EMP set WORK_SHIFT_CD = 'C' where EMP_ID = @EMP_ID";
        //ht.Add("@EMP_ID", "10002");

        //listComm.Add(comm);

        //comm = new SqlCommand();
        //sb.Append("Update TB_H_M_EMP set WORK_SHIFT_CD = 'R' where EMP_ID = @EMP_ID";
        //ht.Add("@EMP_ID", "10003");

        //listComm.Add(comm);
        //string RtnMsg = "";
        //bool rtn = dbs.ExecuteMultiWithTrans(listComm, ref RtnMsg);
        //try
        //{
        //    StringBuilder sb = new StringBuilder();
        //    sb.Append("Update TB_H_M_EMP set WORK_SHIFT_CD = 'C' where EMP_ID = @EMP_ID";
        //    ht.Add("@EMP_ID", "10002");
        //    dbs.ExecuteNonQuery(comm);
        //}
        //catch (Exception ex)
        //{


        //}
        //StringBuilder sb = new StringBuilder();
        //sb.Append("Select * from TB_D_M_ACCOM_MAIN Where EMP_ID in ";
        //comm = utilities.sqlIn(comm, "10033,10064,10068");
        //DataTable dt = dbConn.Query(sb, ht);

        string strWide = "ａｂｃ１２３４５６７８９０，．；";
        string strNarrow = "abc1234567890,.;";
        string strBig5 = "今天天氣真好";
        //Response.Write(utilities.ToNarrow(strWide));
        //Response.Write(utilities.ToWide(strNarrow));
        //Response.Write(utilities.ToWide(strBig5));




    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        //pdf產製範例
        PdfCreate pdfcreate = new PdfCreate();
        pdfcreate.bf = BaseFont.CreateFont(Server.MapPath("~/Fonts/kaiu.ttf"), BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

        MemoryStream ms = new MemoryStream();
        DataTable dt = new DataTable();
        ms = pdfcreate.createPDF_Salary(dt);

        string FileName = "薪資單";

        Response.Clear();
        Response.AddHeader("Content-Disposition", string.Format("attachment; filename=" + HttpUtility.UrlEncode(FileName) + ".pdf"));
        Response.ContentType = "application/pdf; name=" + HttpUtility.UrlEncode(FileName) + ".pdf";
        Response.OutputStream.Write(ms.GetBuffer(), 0, ms.GetBuffer().Length);
        Response.OutputStream.Flush();
        Response.OutputStream.Close();
        Response.Flush();
        ms.Dispose();
        Response.End();
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        try
        {
            //pdf產製範例
            PdfCreate pdfcreate = new PdfCreate();
            pdfcreate.bf = BaseFont.CreateFont(Server.MapPath("~/Fonts/kaiu.ttf"), BaseFont.IDENTITY_H, BaseFont.NOT_EMBEDDED);

            MemoryStream ms = new MemoryStream();
            DataTable dt = new DataTable();
            ms = pdfcreate.createPDF_Salary(dt);
            
            string FileName = "薪資單.pdf";

            List<string> mailto = new List<string>();
            mailto.Add("laurenceliu@systex.com.tw");
            //mailto.Add("20295jean@mail.kuozui.com.tw");
            //utilities.SendMail("測試信件","測試內容","exch2012@mail.kuozui.com.tw",mailto,file_name:FileName,attch:ms);
            using (MemoryStream dolly = new MemoryStream(ms.ToArray()))
            {
                utilities.SendMail("測試信件", "測試內容", "laurenceliu1@gmail.com", mailto, file_name: FileName, attch: dolly);
            }
        }
        catch (Exception ex)
        {
            
            throw;
        }
    }
}