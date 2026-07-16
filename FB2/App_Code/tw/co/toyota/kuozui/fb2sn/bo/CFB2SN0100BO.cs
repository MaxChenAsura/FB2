using FB2.tw.co.toyota.kuozui.bo;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

/// <summary>
/// CFB2SN0100BO 的摘要描述
/// </summary>
public class CFB2SN0100BO : BaseService
{
	public CFB2SN0100BO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}
    public DataTable afa_for_Data(CFB2SN0100DAO dao)
    {
        try
        {            

            return dao.afa_for_Data();

        }
        catch (Exception)
        {

            throw;
        }
    }
    public DataTable search_afa_for_Data(CFB2SN0100DAO dao)
    {
        try
        {

            return dao.search_afa_for_Data();

        }
        catch (Exception)
        {

            throw;
        }
    }
    public DataTable checkData(string YM)
    {
        try
        {
            CFB2IB0300DAO dao = new CFB2IB0300DAO();

            return dao.selectData(YM);

        }
        catch (Exception)
        {

            throw;
        }
    }
    public DataTable is_AWARD_approve(CFB2SN0100DAO dao)
    {
        try
        {
            return dao.is_AWARD_approve();
        }
        catch (Exception)
        {

            throw;
        }
    }
    public DataTable is_BONUS_approve(CFB2SN0100DAO dao)
    {
        try
        {
            return dao.is_BONUS_approve();
        }
        catch (Exception)
        {

            throw;
        }
    }
    public DataTable is_FESTIVAL_approve(CFB2SN0100DAO dao)
    {
        try
        {
            return dao.is_FESTIVAL_approve();
        }
        catch (Exception)
        {

            throw;
        }
    }
    public IWorkbook uploadExcel(Stream fs, string type, CFB2SN0100DAO dao)
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
                try
                {
                    BeginTransaction();
                    dao.UPDATED_BY = SessionHandle.Current.emp_id.Trim();
                    dao.FUNC_ID = "FB2SN010";

                    //更新資料為空值 start

                    //清除明細檔AFA值欄位
                    if (dao.TYPE == "a")
                    {
                        dao.clean_TB_S_M_AWARD_D();
                    }
                    else if (dao.TYPE == "b")
                    {
                        dao.clean_TB_S_R_BONUS_D();
                    }
                    else
                    {
                        dao.clean_TB_S_R_FESTIVAL_D();
                    }

                    //清除主檔AFA值欄位
                    if (dao.TYPE == "a")
                    {
                        dao.clean_TB_S_M_AWARD_H();
                    }
                    else if (dao.TYPE == "b")
                    {
                        dao.clean_TB_S_M_BONUS_H();
                    }
                    else
                    {
                        dao.clean_TB_S_M_FESTIVAL_H();
                    }
                    //end

                    //巡覽每row的資料第一列為title跳過
                    for (int i = 1; i <= sheet.LastRowNum; i++)
                    {
                        bool b = true;
                        if (sheet.GetRow(i) != null)
                        {
                            //讀取cell資料，第一欄為檢核結果欄位跳過
                            string cell1 = sheet.GetRow(i).GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            string cell2 = sheet.GetRow(i).GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            string cell3 = sheet.GetRow(i).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            
                            string error = "";

                            //開始檢查            

                            //工號
                            if (cell1 == "")
                                error += "工號不可為空白\n";
                            else
                            {
                                if (cell1.Length != 5)
                                {
                                    error += "工號長度錯誤\n";
                                }
                                else
                                {
                                    if (!IsNumeric(cell1))
                                    {
                                        error += "工號只能輸入數字\n";
                                    }
                                    else
                                    {
                                        //以畫面KEY值讀取所對應獎金的明細檔，如工號不存在於符合KEY值的明細檔中，則顯示錯誤訊息"工號不存在於獎金檔案中"
                                        dao.EMP_ID = cell1;
                                        if (dao.TYPE == "a")
                                        {
                                            DataTable is_Exist = dao.is_Exist_Emp_Id_AWARD();
                                            if (is_Exist.Rows.Count > 0)
                                            {
                                                int rows = Convert.ToInt32(is_Exist.Rows[0]["rows"].ToString());
                                                if (rows == 0)
                                                {
                                                    error += "工號不存在於年獎檔案中\n";
                                                }
                                            }
                                        }
                                        else if (dao.TYPE == "b")
                                        {
                                             DataTable is_Exist = dao.is_Exist_Emp_Id_BONUS();
                                            if (is_Exist.Rows.Count > 0)
                                            {
                                                int rows = Convert.ToInt32(is_Exist.Rows[0]["rows"].ToString());
                                                if (rows == 0)
                                                {
                                                    error += "工號不存在於紅利檔案中\n";
                                                }
                                            }
                                        }
                                        else
                                        {
                                            DataTable is_Exist = dao.is_Exist_Emp_Id_FESTIVAL();
                                            if (is_Exist.Rows.Count > 0)
                                            {
                                                int rows = Convert.ToInt32(is_Exist.Rows[0]["rows"].ToString());
                                                if (rows == 0)
                                                {
                                                    error += "工號不存在於一時金檔案中\n";
                                                }
                                            }
                                        }
                                       
                                    }
                                }

                            }

                            //姓名
                            if (cell2 == "")
                                error += "姓名不可為空白\n";
                            else{
                                //以欄位一和欄位二為條件，讀取員工人事主檔，如結果為0筆，則顯示錯誤訊息"工號與姓名不符"
                                dao.EMP_NAME = cell2;
                                DataTable is_Exist = dao.id_match_name();
                                if (is_Exist.Rows.Count > 0)
                                {
                                    int rows = Convert.ToInt32(is_Exist.Rows[0]["rows"].ToString());
                                    if (rows == 0)
                                    {
                                        error += "工號與姓名不符\n";
                                    }
                                }

                            }

                            //阿法值金額
                            if (cell3 == "")
                                error += "阿法值金額不可為空白\n";
                            else                            
                            {                               
                                if (cell3.Length > 7)
                                {
                                    error += "阿法值金額長度錯誤\n";
                                }
                                else
                                {
                                    if (!IsNumeric(cell3))
                                    {
                                        error += "阿法值金額只能輸入數字\n";
                                    }
                                }
                            }                           

                            //傳出錯誤訊息
                            style1.SetFont(font1);
                            sheet.GetRow(i).CreateCell(0).CellStyle = style1;
                            sheet.GetRow(i).GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(error);
                            if (error != "")
                            {
                                valid = false;
                            }
                            else
                            {
                                total_amount += Convert.ToInt32(cell3);
                                dao.AFA_AMOUNT = cell3;                                
                               

                                //更新明細檔
                                if (dao.TYPE == "a")
                                {
                                    dao.update_TB_S_M_AWARD_D();
                                }
                                else if (dao.TYPE == "b")
                                {
                                    dao.update_TB_S_R_BONUS_D();
                                }
                                else
                                {
                                    dao.update_TB_S_R_FESTIVAL_D();
                                }
                               
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
                        RollBack();
                        //檢核有錯，匯出附加說明的excel
                        //ExcelHandle.exportExcel(workbook, "檢核錯誤說明" + type);
                        return workbook;
                    }
                    else
                    {
                        //總金額 update
                        dao.AFA_TOTAL_PEOPLE = Convert.ToString(sheet.LastRowNum);
                        dao.AFA_TOTAL_AMOUNT = Convert.ToString(total_amount);
                        //更新主檔
                        if (dao.TYPE == "a")
                        {
                            dao.update_TB_S_M_AWARD_H();
                        }
                        else if (dao.TYPE == "b")
                        {
                            dao.update_TB_S_M_BONUS_H();
                        }
                        else
                        {
                            dao.update_TB_S_M_FESTIVAL_H();
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
           
            //return "0";
            return null;
        }
        catch (Exception ex)
        {
            //return ex.Message;
            throw;
        }

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