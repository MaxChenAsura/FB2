using FB2.tw.co.toyota.kuozui.bo;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

/// <summary>
/// CFB2SL1110BO 的摘要描述
/// </summary>
public class CFB2SL1110BO : BaseService
{
    public CFB2SL1110BO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}
    //txt上傳
    public string updateTxtData(ArrayList fs, CFB2SL1110DAO dao)
    {        
        try
        {
            bool valid = true;
            string error = "";
            int count = 0;
            int length = Encoding.Default.GetBytes(Convert.ToString(fs[0])).Length;   //編碼Byte數
            string[] line = Convert.ToString(fs[0]).Split(',');

            //檢核首行長度 = 43
            string labor = line[0] + line[1];

            if (length != 43)
            {
                error += "挑選之檔案,非勞保費上傳格式\\n";
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
                //刪除年度所得勞保上傳檔的YEAR = 畫面.所得年度 資料
                dao.Delete_TB_S_R_IMX_LABOR_UPLOAD();
                //Commit();
                foreach (string sr in fs)
                {
                    string[] aryStr = sr.Split(',');
                    count++;
                    
                    dao.count = count;

                    dao.Insert_TB_S_R_IMX_LABOR_UPLOAD(aryStr[0], aryStr[3]);                   

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
    public IWorkbook uploadExcel(Stream fs, string type, CFB2SL1110DAO dao)
    {
        try
        {                
            //取得登入者
            string userid = SessionHandle.Current.emp_id;
            int total_amount = 0;
            bool valid = true;

            IWorkbook workbook;
            //依附檔名判斷要用哪種方式讀取
            if (type == ".xls")
            {
                workbook = new HSSFWorkbook(fs);
            }
            else
            {
                workbook = new XSSFWorkbook(fs);
            }


            //取得sheet
            ISheet sheet = workbook.GetSheetAt(0);
            ICellStyle style1 = workbook.CreateCellStyle();
            IFont font1 = workbook.CreateFont();

            font1.Color = HSSFColor.Red.Index;


            if (sheet != null)
            {
                #region cell陣列
                string[] IDENTITY_KIND = new string[sheet.LastRowNum + 1];
                string[] LICENSE_ID = new string[sheet.LastRowNum + 1];
                string[] LICENSE_ID_B = new string[sheet.LastRowNum + 1];
                string[] AMOUNT = new string[sheet.LastRowNum + 1];
                string[] EMP_NAME = new string[sheet.LastRowNum + 1];
                string[] FAMILY_NAME = new string[sheet.LastRowNum + 1];               

                #endregion
                try
                {
                    
                    dao.CREATED_BY = SessionHandle.Current.emp_id.Trim();
                    dao.UPDATED_BY = SessionHandle.Current.emp_id.Trim();
                    dao.FUNC_ID = "FB2SL111";                    

                    //巡覽每row的資料第一列為title跳過
                    for (int i = 1; i <= sheet.LastRowNum; i++)
                    {                        
                        if (sheet.GetRow(i) != null)
                        {
                            #region 讀取cell資料
                            IDENTITY_KIND[i] = sheet.GetRow(i).GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();//這邊沒有空下左邊給錯誤訊息用，若別的上傳需要返回錯誤訊息則從1開始
                            LICENSE_ID[i] = sheet.GetRow(i).GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            LICENSE_ID_B[i] = sheet.GetRow(i).GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            AMOUNT[i] = sheet.GetRow(i).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            EMP_NAME[i] = sheet.GetRow(i).GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            FAMILY_NAME[i] = sheet.GetRow(i).GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            #endregion
                            string error = "";                                              


                            //傳出錯誤訊息
                            style1.SetFont(font1);
                            sheet.GetRow(i).CreateCell(0).CellStyle = style1;
                            sheet.GetRow(i).GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(error);
                            if (error != "")
                            {
                                valid = false;
                            }                            

                        }

                    } if (sheet.LastRowNum == 0)
                    {
                        string error = "請輸入上傳資料\n";
                        style1.SetFont(font1);
                        sheet.GetRow(0).CreateCell(0).CellStyle = style1;
                        //傳出錯誤訊息  
                        sheet.GetRow(0).GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(error);
                        if (error != "")
                        {
                            valid = false;
                        }
                    }
                    if (!valid)
                    {                        
                        //檢核有錯，匯出附加說明的excel
                        //ExcelHandle.exportExcel(workbook, "檢核錯誤說明" + type);
                        return workbook;
                    }
                    else
                    {
                        BeginTransaction();

                        //刪除相同KEY的舊檔
                        dao.deleteExcelData();

                        for (int i = 1; i <= sheet.LastRowNum; i++)
                        {
                            //新增                            
                            try
                            {
                                dao.IDENTITY_KIND = IDENTITY_KIND[i];
                                dao.LICENSE_ID = LICENSE_ID[i];
                                dao.LICENSE_ID_B = LICENSE_ID_B[i];
                                dao.AMOUNT = AMOUNT[i];
                                dao.EMP_NAME = EMP_NAME[i];
                                dao.FAMILY_NAME = FAMILY_NAME[i];                   
                                

                                //新增檔
                                dao.addExcelData();
                            }
                            catch (Exception ex)
                            {
                                RollBack();
                                throw;
                            }
                        }
                        Commit();
                   
                    }
                    
                    return null;
                    
                }
                catch (Exception ex)
                {
                    RollBack();
                    //return ex.Message;
                    throw;
                }
            }            
            
            return null;
        }
        catch (Exception ex)
        {
            //return ex.Message;
            throw;
        }

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
    public bool IsNumeric(String strNumber)
    {               
        Regex NumberPattern=new Regex("[^0-9.-]");  
        return !NumberPattern.IsMatch(strNumber);
    }

    public static bool FullWidthWord(string values)
    {
        bool result = false;
        string pattern = @"^[\u4E00-\u9fa5]+$";
        foreach (char item in values)
        {
            //以Regex判斷是否為中文字，中文字視為全形  
            if (!Regex.IsMatch(item.ToString(), pattern))
            {
                //以16進位值長度判斷是否為全形字  
                if (string.Format("{0:X}", Convert.ToInt32(item)).Length != 2)
                {
                    result = true;
                    break;
                }
            }
        }
        return result;
    }

    public static bool IsChinese(string values)
    {
        bool result = false;
        string pattern = @"^[\u4E00-\u9fa5]+$";
        foreach (char item in values)
        {
            //以Regex判斷是否為中文字，中文字視為全形  
            if (!Regex.IsMatch(item.ToString(), pattern))
            {
                result = true;
                break;             
            }
        }
        return result;
    }





}