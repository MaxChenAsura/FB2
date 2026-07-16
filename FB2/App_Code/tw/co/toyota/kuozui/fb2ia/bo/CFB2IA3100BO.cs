using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;
using System.Collections;
using System.Text;

/// <summary>
/// CFB2IA3100BO 的摘要描述
/// </summary>
public class CFB2IA3100BO : BaseService
{
    public CFB2IA3100BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public ArrayList getExcelData(Stream fs, string type)
    {
        ArrayList arr = new ArrayList();

        IWorkbook workbook;
        //依附檔名判斷要用哪種方式讀取
        if (type == ".XLS")
        {
            workbook = new HSSFWorkbook(fs);
        }
        else
        {
            workbook = new XSSFWorkbook(fs);
        }
        //取得sheet
        ISheet sheet = workbook.GetSheetAt(0);
        if (sheet != null)
        {
            string error = "";
            if (sheet.GetRow(0).LastCellNum != 23)
                error += "挑選之檔案,非健保費帳單格式\\n";
            //巡覽每row的資料第一列為title跳過
            for (int i = 1; i <= sheet.LastRowNum; i++)
            {
                if (sheet.GetRow(i) != null)
                {
                    ArrayList arr2 = new ArrayList();
                    string checkEmpty = "";
                    for (int j = 0; j <= 22; j++)
                        checkEmpty += sheet.GetRow(i).GetCell(j, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                    if (checkEmpty != "")
                    {
                        for (int j = 0; j <= 22; j++)
                        {
                            arr2.Add(sheet.GetRow(i).GetCell(j, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString());
                        }
                        arr.Add(arr2);
                    }
                    else
                    {
                        break;
                    }
                }
            }

        }
        return arr;

    }
    public ArrayList getTxtData(Stream fs)
    {
        ArrayList arr = new ArrayList();
        //讀取文字檔，匯入資料到 DataTable 
        using (StreamReader sr = new StreamReader(fs, System.Text.Encoding.Default))
        {
            string str = "";
            while ((str = sr.ReadLine()) != null)
            {
                arr.Add(str);

            }
        }
        return arr;

    }
    //excel上傳
    public string updateExcelData(ArrayList fs, string type, string BILLS_KIND, string COMPANY_CD, string FEES_YM, string HEALTH_ORG_ID, string COMPANY_NAME)
    {
        CFB2IA3100DAO fb2ia = new CFB2IA3100DAO();
        try
        {
            bool valid = true;
            string error = "";

            //檢查
            if (((ArrayList)fs[0]).Count != 23)
                error += "挑選之檔案,非健保費帳單格式\\n";



            if (Convert.ToString(((ArrayList)fs[1])[0]).Length != 6)
                error += "挑選之檔案,非健保費帳單格式\\n";
            else
            {
                if (Convert.ToString(Convert.ToInt64(Convert.ToString(((ArrayList)fs[1])[0]).Trim().Replace("/", "")) + 191100) != FEES_YM.Replace("/", ""))
                    error += "指定的檔案計費年月不正確\\n";
            }
            if (Convert.ToString(((ArrayList)fs[1])[1]) != HEALTH_ORG_ID)
                error += "指定的檔案不為" + COMPANY_NAME + "的健保費明細\\n";
            if (error != "")
            {
                valid = false;
            }
            if (!valid)
            {
                return error;
            }
            else
            {
                BeginTransaction();
                //刪除[TB_I_S_BILLS 保費帳單轉入暫存檔]
                fb2ia.Delete(BILLS_KIND, COMPANY_CD, FEES_YM);
               
                for (int i = 0; i < fs.Count; i++)
                {
                    try
                    {
                        //新增                      
                       
                        fb2ia.Add(COMPANY_CD, Convert.ToString(((ArrayList)fs[i])[0]), Convert.ToString(((ArrayList)fs[i])[1]), Convert.ToString(((ArrayList)fs[i])[2]), Convert.ToString(((ArrayList)fs[i])[3]), Convert.ToString(((ArrayList)fs[i])[4])
                            , Convert.ToString(((ArrayList)fs[i])[5]), Convert.ToString(((ArrayList)fs[i])[6]), Convert.ToString(((ArrayList)fs[i])[7]), Convert.ToString(((ArrayList)fs[i])[8]), Convert.ToString(((ArrayList)fs[i])[9])
                            , Convert.ToString(((ArrayList)fs[i])[10]), Convert.ToString(((ArrayList)fs[i])[11]), Convert.ToString(((ArrayList)fs[i])[12]), Convert.ToString(((ArrayList)fs[i])[13]), Convert.ToString(((ArrayList)fs[i])[14])
                            , Convert.ToString(((ArrayList)fs[i])[15]), Convert.ToString(((ArrayList)fs[i])[16]), Convert.ToString(((ArrayList)fs[i])[17]), Convert.ToString(((ArrayList)fs[i])[18]), Convert.ToString(((ArrayList)fs[i])[19])
                            , Convert.ToString(((ArrayList)fs[i])[20]), Convert.ToString(((ArrayList)fs[i])[21]), Convert.ToString(((ArrayList)fs[i])[22]));
                       
                    }
                    catch (Exception ex)
                    {
                        RollBack();
                        return ex.Message;
                        throw;
                    }
                }
                //return "0";
                Commit();
            }
            //}
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
            throw;
        }
    }
    //txt上傳
    public string updateTxtData(ArrayList fs, string LABOR_ORG_ID, string COMPANY_CD, string BILLS_KIND, string FEES_YM, string COMPANY_NAME)
    {
        CFB2IA3100DAO fb2ia = new CFB2IA3100DAO();
        try
        {
            bool valid = true;
            string error = "";
            int count = 0;
            int length = Encoding.Default.GetBytes(Convert.ToString(fs[0])).Length;   //編碼Byte數
            string[] line = Convert.ToString(fs[0]).Split(',');
            string labor = "";
            //檢核
            if (BILLS_KIND == "B")
            {

                labor = line[0] + line[1];                
                /*
                if (length != 84)
                {
                    error += "挑選之檔案,非勞保費帳單格式\\n";
                }
                */
                if (line.Length != 14) { 
                    error += "挑選之檔案,非勞保費帳單格式\\n";
                }               
                else
                {
                    string ym = "";
                    if (line[6].Trim().ToString() != "" && (line[6].Trim().Length == 4 || line[6].Trim().Length == 5))
                        ym = Convert.ToString(Convert.ToInt64(line[6].Trim()) + 191100);
                    if (labor != LABOR_ORG_ID)
                    {
                        error += "指定的檔案不為" + COMPANY_NAME + "的勞保費帳單\\n";
                    }
                    if (ym != FEES_YM.Replace("/", ""))
                    {
                        error += "指定的檔案計費年月不正確\\n";
                    }
                }
                
            }
            if (BILLS_KIND == "C")
            {
                labor = line[0] + line[1];
                
                if (length != 115)
                {
                    error += "挑選之檔案,非勞退自提帳單格式\\n";
                }
                else
                {
                    string ym = "";
                    if (line[2].Trim().ToString() != "" && (line[2].Trim().Length == 4 || line[2].Trim().Length == 5))
                        ym = Convert.ToString(Convert.ToInt64(line[2].Trim()) + 191100);
                    if (labor != "P" + LABOR_ORG_ID)
                    {
                        error += "指定的檔案不為" + COMPANY_NAME + "的勞退自提名冊檔\\n";
                    }
                    if (line[3] != "92")
                    {
                        error += "指定的檔案不為" + COMPANY_NAME + "的勞退自提名冊檔\\n";
                    }
                    if (ym != FEES_YM.Replace("/", ""))
                    {
                        error += "指定的檔案計費年月不正確\\n";
                    }
                }
                
            }
            if (BILLS_KIND == "D")
            {
                labor = line[0] + line[1];
                
                if (length != 115)
                {
                    error += "挑選之檔案,非勞退雇主提撥帳單格式\\n";
                }
                else
                {
                    string ym = "";
                    if (line[2].Trim().ToString() != "" && (line[2].Trim().Length == 4 || line[2].Trim().Length == 5))
                        ym = Convert.ToString(Convert.ToInt64(line[2].Trim()) + 191100);
                    if (labor != "P" + LABOR_ORG_ID)
                    {
                        error += "指定的檔案不為" + COMPANY_NAME + "的勞退雇主提撥名冊檔\\n";
                    }
                    if (line[3] != "91")
                    {
                        error += "指定的檔案不為" + COMPANY_NAME + "的雇主提繳名冊檔\\n";
                    }
                    if (ym != FEES_YM.Replace("/", ""))
                    {
                        error += "指定的檔案計費年月不正確\\n";
                    }
                }                
            }

            if (error != "")
            {
                valid = false;
            }
            if (!valid)
            {
                return error;
            }
            else
            {
                BeginTransaction();
                //刪除[TB_I_S_BILLS 保費帳單轉入暫存檔]
                fb2ia.Delete(BILLS_KIND, COMPANY_CD, FEES_YM);
                //Commit();
                foreach(string sr in fs)
                {
                    string[] aryStr = sr.Split(',');
                    count++;
                    
                    //BeginTransaction();
                    fb2ia.count = count;
                    //新增_B
                    if (BILLS_KIND == "B")
                    {
                        //20200918 上傳檔案,增加姓名的羅馬拼音[10],但姓名羅馬拼音不需要，故修改傳入值而已
                        fb2ia.Add_B(COMPANY_CD, aryStr[0], aryStr[1], aryStr[2], aryStr[3], aryStr[4], aryStr[5], aryStr[6],
                                    aryStr[7], aryStr[8], aryStr[9], aryStr[11],aryStr[12]);
                    }
                    if (BILLS_KIND == "C")
                    {
                        fb2ia.Add_C(COMPANY_CD, aryStr[0], aryStr[1], aryStr[2], aryStr[3], aryStr[4], aryStr[5], aryStr[6],
                                    aryStr[7], aryStr[8], aryStr[9], aryStr[10], aryStr[11], aryStr[12], aryStr[13], aryStr[14], aryStr[15]);
                    }
                    if (BILLS_KIND == "D")
                    {
                        //if (aryStr[6] == "E125398533")
                        //{
                        //    string tt = "";
                        //    tt = aryStr[9].Trim();
                        //}
                        //原因:停繳，金額是空白的，過濾掉
                        if (aryStr[9].Trim() == "")
                        {
                            continue;
                        }
                        fb2ia.Add_D(COMPANY_CD, aryStr[0], aryStr[1], aryStr[2], aryStr[3], aryStr[4], aryStr[5], aryStr[6],
                                    aryStr[7], aryStr[8], aryStr[9], aryStr[10], aryStr[11], aryStr[12], aryStr[13], aryStr[14], aryStr[15]);
                    }
                    //Commit();

                }
                Commit();
            }
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
            throw;
        }
    }
}