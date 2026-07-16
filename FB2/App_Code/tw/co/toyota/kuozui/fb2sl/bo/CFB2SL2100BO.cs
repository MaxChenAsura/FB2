using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;
using System.Text;
using System.Text.RegularExpressions;

/// <summary>
/// CFB2SL2100BO 的摘要描述
/// </summary>
public class CFB2SL2100BO : BaseService
{
    CFB2SL2100DAO dao = new CFB2SL2100DAO();
    public CFB2SL2100BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        // 
    }
    //特休生成
    public string executeGenerate(string year, string salary_dt_s, string salary_dt_e, string login_id, string func_id) //2014-09-25 fixed by Stanley Chen, add 2 parameters: login.id、function.id
    {
        try
        {
            string msg = "";
            dao.RunProcSP_S_SALARY_TAX_EXEC(year, salary_dt_s, salary_dt_e, login_id, func_id);
            DataTable dtSPresult = dao.checkSP("SP_S_SALARY_TAX_EXEC");
            if (dtSPresult.Rows.Count > 0)
            {
                //PROC_STATUS：Y = 成功,N = 失敗,E = Exception  ,PROC_LOG：處理結果中文訊息
                if (Convert.ToString(dtSPresult.Rows[0]["PROC_STATUS"]) == "Y")
                    msg = "0";
                else
                    msg = Convert.ToString(dtSPresult.Rows[0]["PROC_LOG"]);
            }
            return msg;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public MemoryStream Action(string year)
    {
        try
        {
            MemoryStream outputStreamer = new MemoryStream();
            string company_cd = string.Empty;
            string company_id = string.Empty;
            string chineseYear = (Convert.ToInt32(year) - 1911).ToString();
            DataTable dtCompany_CD = dao.getCompany_CD();
            if (dtCompany_CD.Rows.Count > 0)
            {
                //System.Web.HttpContext.Current.Response.Clear();
                //System.Web.HttpContext.Current.Response.ClearHeaders();
                //System.Web.HttpContext.Current.Response.ClearContent();
                //System.Web.HttpContext.Current.Response.HeaderEncoding = System.Text.Encoding.GetEncoding("big5");
                ////System.Web.HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml";
                //System.Web.HttpContext.Current.Response.ContentType = "application/zip";
                //System.Web.HttpContext.Current.Response.AddHeader("content-disposition", "filename=" + HttpUtility.UrlEncode(chineseYear + "年度綜合所得稅電子媒體申報檔.zip"));
               

                using (Ionic.Zip.ZipFile zip = new Ionic.Zip.ZipFile())
                {
                    for (int i = 0; i < dtCompany_CD.Rows.Count; i++)
                    {
                        company_cd = Convert.ToString(dtCompany_CD.Rows[i]["COMPANY_CD"]);
                        company_id = Convert.ToString(dtCompany_CD.Rows[i]["COMPANY_ID"]);
                        MemoryStream fileStream = new MemoryStream();
                        exportFile(fileStream, year, company_cd, company_id);
                        zip.AddEntry(company_id + "." + chineseYear + ".txt", fileStream.ToArray());
                        fileStream.Close();
                        fileStream.Dispose();
                    }
                    zip.Save(outputStreamer);
                    //System.Web.HttpContext.Current.Response.BinaryWrite(outputStreamer.ToArray());
                }
                //System.Web.HttpContext.Current.Response.Buffer = false;
                //System.Web.HttpContext.Current.Response.End();
            }
            return outputStreamer;
        }
        catch
        {
            throw;
        }
    }
    public void exportFile(MemoryStream fileStream, string year, string company_cd, string company_id)
    {
        try
        {
            using (StreamWriter sw = new StreamWriter(fileStream, Encoding.GetEncoding("big5")))
            {
                sw.Flush();
                //所得人所得資料
                export_PersonData(sw, year, company_cd);
                //各類所得申報資料 20151130 財務上傳不需此匯總
                //export_GroupData(sw, year, company_cd);
                //申報單位基本資料
                export_CompanyData(sw, year, company_cd);
            }
        }
        catch
        {
            throw;
        }
    }
    public void export_PersonData(StreamWriter sw, string year, string company_cd)
    {
        DataTable dtPerson = dao.getPersonData(year, company_cd);
        string st = "";
        
        if (dtPerson.Rows.Count > 0)
        {
            for (int i = 0; i < dtPerson.Rows.Count; i++)
            {
                string st1 = dtPerson.Rows[i]["TAX_ORG_ID"].ToString();
                string st2 = dtPerson.Rows[i]["TAX_FORMAT"].ToString();
                string st3 = dtPerson.Rows[i]["LICENSE_ID"].ToString();
                string st4 = dtPerson.Rows[i]["EMP_ID"].ToString();


                // Add some text to the file.
                sw.Write(string.Format("{0,-3}", dtPerson.Rows[i]["TAX_ORG_ID"]));
                sw.Write(dtPerson.Rows[i]["TAX_SEQ"].ToString().PadLeft(8, '0'));
                sw.Write(string.Format("{0,-8}", dtPerson.Rows[i]["COMPANY_ID"]));
                sw.Write(string.Format("{0,-1}", dtPerson.Rows[i]["TAX_CD"]));
                sw.Write(string.Format("{0,-2}", (dtPerson.Rows[i]["TAX_FORMAT"].ToString()).Substring(0,2)));
                sw.Write(string.Format("{0,-10}", dtPerson.Rows[i]["LICENSE_ID"]));
                sw.Write(string.Format("{0,-1}", dtPerson.Rows[i]["LICENSE_CD"]));
                sw.Write(dtPerson.Rows[i]["AMOUNT"].ToString().PadLeft(10, '0'));
                sw.Write(dtPerson.Rows[i]["TAX"].ToString().PadLeft(10, '0'));
                sw.Write(dtPerson.Rows[i]["INCOME"].ToString().PadLeft(10, '0'));
                st = dtPerson.Rows[i]["EMP_ID"].ToString();
               
                if (dtPerson.Rows[i]["TAX_FORMAT"].ToString() == "54C")
                {
                    st = st + "0000000";
                }
                sw.Write(string.Format("{0,-12}", st));
                sw.Write(string.Format("{0,-1}", dtPerson.Rows[i]["SOFTWARE_CD"]));
                sw.Write(string.Format("{0,-1}", dtPerson.Rows[i]["ERROR_CD"]));
                sw.Write(string.Format("{0,-3}", dtPerson.Rows[i]["PAY_YR"]));                
                sw.Write(utilities.toWide(string.Format("{0,-20}", utilities.convertBig5(dtPerson.Rows[i]["EMP_NAME"].ToString()).Replace("?", "？"))));               
                if (dtPerson.Rows[i]["REGISTER_ADDR"].ToString().Length > 30)
                {                    
                    sw.Write(utilities.toWide(string.Format("{0,-30}", utilities.convertBig5(dtPerson.Rows[i]["REGISTER_ADDR"].ToString().Substring(0, 30)).Replace("?", "？"))));
                }
                else
                {
                    sw.Write(utilities.toWide(string.Format("{0,-30}", utilities.convertBig5(dtPerson.Rows[i]["REGISTER_ADDR"].ToString()).Replace("?", "？"))));
                }

                if (dtPerson.Rows[i]["TAX_FORMAT"].ToString() == "54C")
                {
                    sw.Write(string.Format("{0,-5}", (dtPerson.Rows[i]["PAY_YM_START"].ToString()).Substring(0, 3)));
                    sw.Write(string.Format("{0,-5}", (dtPerson.Rows[i]["PAY_YM_END"].ToString()).Substring(0,3)));
                    sw.Write(dtPerson.Rows[i]["INCOME"].ToString().PadLeft(10, '0'));
                    sw.Write("0".PadLeft(30,'0'));
                    sw.Write(string.Format("{0,8}", ""));
                }
                else if(dtPerson.Rows[i]["TAX_FORMAT"].ToString() == "50")
                {
                    sw.Write(string.Format("{0,-5}", dtPerson.Rows[i]["PAY_YM_START"]));
                    sw.Write(string.Format("{0,-5}", dtPerson.Rows[i]["PAY_YM_END"]));
                    sw.Write(dtPerson.Rows[i]["RETIRE_AMT"].ToString().PadLeft(10, '0'));
                    sw.Write(string.Format("{0,38}", ""));
                }
                else
                {
                    sw.Write(string.Format("{0,-5}", dtPerson.Rows[i]["PAY_YM_START"]));
                    sw.Write(string.Format("{0,-5}", dtPerson.Rows[i]["PAY_YM_END"]));
                    sw.Write(string.Format("{0,48}", ""));
                }
               
                
                sw.Write(string.Format("{0,-1}", dtPerson.Rows[i]["FORM_CD"]));
                sw.Write(string.Format("{0,-1}", dtPerson.Rows[i]["DUE_183"]));
                sw.Write(string.Format("{0,-2}", dtPerson.Rows[i]["COUNTRY_CD"]));
                sw.Write(string.Format("{0,-2}", dtPerson.Rows[i]["TAX_DEAL_CD"]));
                sw.Write(string.Format("{0,2}", ""));
                sw.WriteLine(string.Format("{0,-4}", DateTime.Now.Date.ToString("MMdd")));
            }
        }
    }
    public void export_GroupData(StreamWriter sw, string year, string company_cd)
    {
        DataTable dtGroup = dao.getGroupData(year, company_cd);
        if (dtGroup.Rows.Count > 0)
        {
            for (int i = 0; i < dtGroup.Rows.Count; i++)
            {
                // Add some text to the file.
                sw.Write(string.Format("{0,-3}", dtGroup.Rows[i]["TAX_ORG_ID"]));
                sw.Write(string.Format("{0,-8}", dtGroup.Rows[i]["NUMBER"]));
                sw.Write(string.Format("{0,-8}", dtGroup.Rows[i]["COMPANY_ID"]));
                sw.Write("9");
                sw.Write(string.Format("{0,-1}", dtGroup.Rows[i]["FORIEN_CD"]));
                sw.Write(string.Format("{0,-1}", dtGroup.Rows[i]["PERSON_CD"]));
                sw.Write(string.Format("{0,-1}", dtGroup.Rows[i]["TAX_FORMAT_CD1"]));
                sw.Write(string.Format("{0,-1}", dtGroup.Rows[i]["TAX_FORMAT_CD2"]));
                sw.Write(dtGroup.Rows[i]["LICENSE_ID_COUNT"].ToString().PadLeft(9, '0'));
                sw.Write(dtGroup.Rows[i]["AMOUNT"].ToString().PadLeft(14, '0'));
                sw.Write(dtGroup.Rows[i]["TAX"].ToString().PadLeft(14, '0'));
                sw.Write(string.Format("{0,-9}", dtGroup.Rows[i]["TAX_ID"]));
                sw.Write(string.Format("{0,1}", ""));// 列印位置
                sw.Write(string.Format("{0,12}", dtGroup.Rows[i]["HOUSE_TAX_ID"]));// H03080426050  申報單位房屋稅籍編號
                sw.Write(string.Format("{0,1}", ""));
                sw.Write(string.Format("{0,-1}", dtGroup.Rows[i]["PAY_YR"]));
                sw.Write(string.Format("{0,-7}", (DateTime.Now.Year - 1911).ToString() + DateTime.Now.ToString("MM") + DateTime.Now.ToString("dd")));
                sw.Write(string.Format("{0,-8}", dtGroup.Rows[i]["MIN_TAX_SEQ"]));
                sw.Write(string.Format("{0,-8}", dtGroup.Rows[i]["MAX_TAX_SEQ"]));
                sw.Write("0000");
                sw.Write("00");
                sw.Write(string.Format("{0,7}", "0000000"));
                //sw.Write(string.Format("{0,7}", "")); //改為7個0
                sw.Write(dtGroup.Rows[i]["RETIRE_AMT"].ToString().PadLeft(14, '0'));
                sw.Write("00000000000000");
                sw.Write(string.Format("{0,98}", ""));
                sw.WriteLine("C");
            }
        }
    }
    public void export_CompanyData(StreamWriter sw, string year, string company_cd)
    {
        DataTable dtCompany = dao.getCompanyData(year, company_cd);
        if (dtCompany.Rows.Count > 0)
        {
            for (int i = 0; i < dtCompany.Rows.Count; i++)
            {
                // Add some text to the file.
                sw.Write(string.Format("{0,-3}", dtCompany.Rows[i]["TAX_ORG_ID"]));
                sw.Write(string.Format("{0,8}", ""));
                sw.Write(string.Format("{0,-8}", dtCompany.Rows[i]["COMPANY_ID"]));
                sw.Write("1");                
                sw.Write(utilities.toWide(string.Format("{0,-18}", utilities.convertBig5(dtCompany.Rows[i]["COMPANY_NAME"].ToString()).Replace("?", "？"))));          
                
                if (dtCompany.Rows[i]["COMPANY_ADDR"].ToString().Length > 26)
                {
                    sw.Write(utilities.toWide(string.Format("{0,-26}", utilities.convertBig5(dtCompany.Rows[i]["COMPANY_ADDR"].ToString().Substring(0, 26)).Replace("?", "？"))));
                }
                else
                {
                    sw.Write(utilities.toWide(string.Format("{0,-26}", utilities.convertBig5(dtCompany.Rows[i]["COMPANY_ADDR"].ToString()).Replace("?", "？"))));
                }     
                
                sw.Write(dtCompany.Rows[i]["CHAIRMAN_NAME"].ToString().PadRight(20, '　'));                
                sw.Write(dtCompany.Rows[i]["CONTACTER_NAME"].ToString().PadRight(20, '　'));                
                sw.Write(string.Format("{0,-15}", dtCompany.Rows[i]["CONTACTER_TEL"]));
                sw.Write(string.Format("{0,-30}", dtCompany.Rows[i]["COMPANY_EMAIL"]));
                sw.Write(string.Format("{0,-9}", dtCompany.Rows[i]["TAX_ID"]));
                sw.Write("N");
                sw.Write("  ");
                sw.Write(string.Format("{0,2}", ""));
                sw.Write("N");
                sw.Write(string.Format("{0,1}", ""));
                sw.WriteLine("N");
            }
        }
    }
    
}