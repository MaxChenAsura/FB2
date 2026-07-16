using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;
using System.Text;

/// <summary>
/// CFB2SD1200BO 的摘要描述
/// </summary>
public class CFB2SD1200BO : BaseService
{
    public CFB2SD1200BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    //每一家銀行的媒體檔格式不同  有新銀行都必須新寫一段
    //台企銀 050
    public MemoryStream getTxtData(DataTable dt, CFB2SD1200DAO fb2sd, string remit_dt, string paykind, string transclassname, string transclassid ,string pay_id)
    {
        try
        {
            String vemptyString =" ";
            //DataTable dt = fb2sd.TxtFirstLine(remit_dt, paykind);
            DataTable dt2 = fb2sd.TxtNoFirstLine(remit_dt, paykind, pay_id);
            //if (dt.Rows.Count > 0 && Convert.ToInt16(dt.Rows[0]["CNT"].ToString()) > 0)
            //{
                //將轉帳組別寫回薪資關帳主檔 (20201217 轉帳組別 沒有其他地方會用到...只做為標誌同一天同一家銀行收到不同批的概念)
                fb2sd.update_Salary_pay_H(remit_dt, paykind, transclassid);
                MemoryStream fileStream = new MemoryStream();
                using (StreamWriter sw = new StreamWriter(fileStream))
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        // Add some text to the file.
                        sw.Write("*");
                        sw.Write(transclassname);
                        sw.Write(utilities.DateToTw(remit_dt, ""));
                        sw.Write(dt.Rows[i]["CNT"].ToString().PadLeft(9, '0'));
                        sw.Write(dt.Rows[i]["SREAL_AMT"].ToString().PadLeft(11, '0'));
                        sw.Write("0");
                        sw.WriteLine(vemptyString.ToString().PadRight(92, ' '));  //WriteLine 換行
                    }
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        // Add some text to the file.
                        sw.Write(" ");
                        sw.Write(dt2.Rows[i]["SALARY_ACCOUNT_NO"].ToString().Trim().Substring(0, 11));
                        sw.Write(dt2.Rows[i]["REAL_AMT"].ToString().PadLeft(7, '0'));
                        sw.Write("0");
                        sw.WriteLine(vemptyString.ToString().PadRight(108, ' '));  //WriteLine 換行
                    }

                    sw.Flush();
                    //System.Web.HttpContext.Current.Response.Clear();
                    //System.Web.HttpContext.Current.Response.ClearHeaders();
                    //System.Web.HttpContext.Current.Response.ClearContent();
                    //System.Web.HttpContext.Current.Response.HeaderEncoding = System.Text.Encoding.GetEncoding("big5");
                    ////System.Web.HttpContext.Current.Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml";
                    //System.Web.HttpContext.Current.Response.ContentType = "application/octet-stream";
                    //System.Web.HttpContext.Current.Response.AddHeader("content-disposition", "attachment;filename=" + HttpUtility.UrlEncode("WAGE"));
                    //System.Web.HttpContext.Current.Response.BinaryWrite(fileStream.ToArray());
                    //System.Web.HttpContext.Current.Response.Buffer = false;
                    //fileStream.Close();
                    //fileStream.Dispose();
                    //System.Web.HttpContext.Current.Response.End();
                }
            return fileStream;
            //}
            //else
            //{
            //    return "無匯出資料!";
            //}
            
            //return "0";
        }
        catch 
        {
            RollBack();
            throw;
        }
    }

    //中信 822
    public MemoryStream getTxtData_2(DataTable dt, CFB2SD1200DAO fb2sd, string remit_dt, string paykind, string transclassname, string transclassid, string Bank_Id, string Remark, string pay_id)
    {
        try
        {
            DataTable kz_account_dt = utilities.getParameter("SD", "CS_BANK_ACCOUNT");
            string kz_account = "";//國瑞中信帳戶
            string taiwan_dt = utilities.DateToTw(remit_dt);
            if (kz_account_dt.Rows.Count > 0)
            {
                kz_account = kz_account_dt.Rows[0]["CODE_VAL1"].ToString();
                kz_account = kz_account.Substring(0, 12);//國瑞帳戶 12碼長
            }
            String vemptyString = " ";
            //自訂附言處理 總長14  /* 2021/03/25 只有獎金類的可以使用 */ 
            int need_lenth = 0;
            need_lenth = 14-System.Text.Encoding.Default.GetBytes(Remark).Length;//待補的總長度
            string kind = " ";//入帳種類代碼 先預設為1半形空白
            if (Remark.Length > 0)
            {
                kind = "X";//X 表示使用自訂附言  空白表示使用"薪資"這兩字
            }

            //DataTable dt = fb2sd.TxtFirstLine(remit_dt, paykind);
            DataTable dt2 = fb2sd.TxtNoFirstLine(remit_dt, paykind, Bank_Id, pay_id);
            //if (dt.Rows.Count > 0 && Convert.ToInt16(dt.Rows[0]["CNT"].ToString()) > 0)
            //{
            //將轉帳組別寫回薪資關帳主檔 (20201217 轉帳組別 沒有其他地方會用到...只做為標誌同一天同一家銀行收到不同批的概念)

            //fb2sd.update_Salary_pay_H(remit_dt, paykind, transclassid);
            MemoryStream fileStream = new MemoryStream();
            //20210325 區分成月薪類(含預付薪)和獎金類
            if (paykind == "9999" || paykind == "1061")
            {
                using (StreamWriter sw = new StreamWriter(fileStream, Encoding.GetEncoding("big5")))
                {
                    // 第一行
                    sw.Write(kz_account.PadRight(12, ' '));//公司帳號 1-12
                    sw.Write((taiwan_dt.Replace("/", "")).PadLeft(10, '0'));//入帳日期 13-22
                    sw.Write("A");//代發薪資/整批入帳 23
                    sw.WriteLine(vemptyString.ToString().PadRight(80, ' '));//統一編號 24-34  關係戶戶名 24-103
                    //sw.WriteLine("A");//WriteLine 換行 

                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        // Add some text to the file.
                        sw.Write(dt2.Rows[i]["SALARY_ACCOUNT_NO"].ToString().Trim().Substring(0, 12));//入帳帳號 1-12
                        sw.Write(dt2.Rows[i]["REAL_AMT"].ToString().PadLeft(10, '0'));//入帳金額 13-22
                        sw.Write(vemptyString.ToString().PadRight(10, ' '));//收款人戶名 23-32
                        sw.Write(dt2.Rows[i]["LICENSE_ID"].ToString().PadRight(10, ' '));//身份証號 33-42
                        sw.Write(vemptyString.ToString());//入帳種類代碼 43-43   空白 =薪資 **這邊只能入空白**
                        sw.WriteLine("Y");//檢查碼功能 44-44                    
                        
                        //sw.WriteLine(vemptyString.ToString().PadRight(22, ' '));  //WriteLine 換行
                    }

                    sw.Flush();

                }
            }
            else
            {
                using (StreamWriter sw = new StreamWriter(fileStream, Encoding.GetEncoding("big5")))
                {
                    // 第一行
                    sw.Write(kz_account.PadRight(12, ' '));//公司帳號 1-12
                    sw.Write((taiwan_dt.Replace("/", "")).PadLeft(10, '0'));//入帳日期 13-22
                    sw.Write("A");//代發薪資/整批入帳 23
                    sw.WriteLine(vemptyString.ToString().PadRight(91, ' '));//統一編號 24-34  關係戶戶名 35-114
                    //sw.WriteLine("A");//WriteLine 換行 

                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        // Add some text to the file.
                        sw.Write(dt2.Rows[i]["SALARY_ACCOUNT_NO"].ToString().Trim().Substring(0, 12));//入帳帳號 1-12
                        sw.Write(dt2.Rows[i]["REAL_AMT"].ToString().PadLeft(10, '0'));//入帳金額 13-22
                        sw.Write(vemptyString.ToString().PadRight(10, ' '));//收款人戶名 23-32
                        sw.Write(dt2.Rows[i]["LICENSE_ID"].ToString().PadRight(10, ' '));//身份証號 33-42
                        sw.Write(kind);//入帳種類代碼 43-43   空白 =薪資 , X = 使用自附留言
                        sw.Write("Y");//檢查碼功能 44-44                    
                        if (need_lenth > 0)
                        {
                            sw.Write(Remark.ToString());//客戶自訂附言 45-58   <7碼中文/14碼英數字(半形)>
                            sw.WriteLine(vemptyString.ToString().PadRight(need_lenth, ' '));//補滿14 Byte 然後換行
                        }
                        else
                        {
                            sw.WriteLine(Remark.ToString());
                        }

                        //sw.WriteLine(vemptyString.ToString().PadRight(22, ' '));  //WriteLine 換行
                    }

                    sw.Flush();

                }
            }
            /*
            using (StreamWriter sw = new StreamWriter(fileStream))
            {
                // 第一行
                sw.Write(kz_account.PadRight(12, ' '));//公司帳號 1-12
                sw.Write((taiwan_dt.Replace("/", "")).PadLeft(10, '0'));//入帳日期 13-22
                sw.Write("A");//代發薪資/整批入帳 23
                sw.WriteLine(vemptyString.ToString().PadRight(91, ' '));//統一編號 24-34  關係戶戶名 35-114
                //sw.WriteLine("A");//WriteLine 換行 

                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    // Add some text to the file.
                    sw.Write(dt2.Rows[i]["SALARY_ACCOUNT_NO"].ToString().Trim().Substring(0, 12));//入帳帳號 1-12
                    sw.Write(dt2.Rows[i]["REAL_AMT"].ToString().PadLeft(10, '0'));//入帳金額 13-22
                    sw.Write(vemptyString.ToString().PadRight(10, ' '));//收款人戶名 23-32
                    sw.Write(dt2.Rows[i]["LICENSE_ID"].ToString().PadRight(10, ' '));//身份証號 33-42
                    sw.Write(kind);//入帳種類代碼 43-43   空白 =薪資 , X = 使用自附留言
                    sw.Write("Y");//檢查碼功能 44-44                    
                    if (need_lenth > 0)
                    {
                        sw.Write(Remark.ToString());//客戶自訂附言 45-58   <7碼中文/14碼英數字(半形)>
                        sw.WriteLine(vemptyString.ToString().PadRight(need_lenth, ' '));//補滿14 Byte 然後換行
                    }
                    else
                    {
                        sw.WriteLine(Remark.ToString());
                    }                
                   
                    //sw.WriteLine(vemptyString.ToString().PadRight(22, ' '));  //WriteLine 換行
                }

                sw.Flush();
                
            }
             * */
            return fileStream;
           
        }
        catch
        {
            RollBack();
            throw;
        }
    }

    //檢查資料是否鎖定
    public string CheckData(CFB2SD1200DAO fb2sd, string remit_dt, string pay_kind)
    {
        string rtnmessage = "0";
        try
        {
            DataTable dt = fb2sd.get_Salary_pay_H(remit_dt, pay_kind);
            if ((int)dt.Rows[0]["resultCount"] > 0)
            {
                rtnmessage = "此查詢資料,已轉出媒體檔,是否重新匯出媒體檔。";
            }
            dt.Clear();
            return rtnmessage;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
}