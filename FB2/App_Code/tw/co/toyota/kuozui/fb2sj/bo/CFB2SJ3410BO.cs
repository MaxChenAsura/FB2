using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;
using NPOI.SS.UserModel;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.Util;

/// <summary>
/// WFB2SJ0120Service 的摘要描述
/// </summary>
public class CFB2SJ3410BO : BaseService
{
    public CFB2SJ3410BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    //刪除
    public string deleteData(List<Tuple<string, string, string>> keysList)
    {
        CFB2SJ3410DAO sj013DAO = new CFB2SJ3410DAO();
        string rtnmessage = "";//存在檢查後的訊息
        try
        {

            //檢查完成後，逐筆進行刪除
            if (rtnmessage == "")
            {
                try
                {
                    BeginTransaction();
                    foreach (var item in keysList)
                    {
                        sj013DAO.ASSESS_YEAR = item.Item1;
                        sj013DAO.ASSESS_TYPE = item.Item2;
                        sj013DAO.EMP_ID = item.Item3;
                        sj013DAO.deleteData();
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
            else
            {
                return rtnmessage;
            }

        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }



    //EXCEL上傳
    public IWorkbook uploadExcel(Stream fs, string type, string assess_year, string assess_type)
    {

        try
        {
            CFB2SJ3410DAO sj013DAO = new CFB2SJ3410DAO();
            CFB2SJ0400DAO sj040DAO = new CFB2SJ0400DAO();
           // DataTable dt = utilities.getCommCode("HB", "WS_CD", "", "");
            //取得登入者
            string userid = SessionHandle.Current.emp_id;

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
            style1.SetFont(font1);
            if (sheet != null)
            {
                //2.取得excel的資料
                string cell_emp_id = "";
                string cell_memo = "";
                Dictionary<string, Dictionary<string, string>> mData = new Dictionary<string, Dictionary<string, string>>();
                Dictionary<string, string> sData = new Dictionary<string, string>();
                List<Dictionary<string, Dictionary<string, string>>> liData = new List<Dictionary<string, Dictionary<string, string>>>();

                //錯誤訊息
                string error = "";
                List<string> empid_List = new List<string>();
                ICell cell;
                //巡覽每row的資料第一列為title跳過
               // CFB2SJ0120DAO sj0120DAO = new CFB2SJ0120DAO();
                for (int i = 2; i <= sheet.LastRowNum; i++)
                {
                    //清空
                    error = "";

                    if (sheet.GetRow(i) != null)
                    {
                        //讀取cell資料，第一欄為檢核結果欄位跳過
                         cell_emp_id = sheet.GetRow(i).GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();                       
                         cell_memo = sheet.GetRow(i).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();

                         


                        if (cell_emp_id == "")
                        {
                            error += "工號不可空白,\n";
                        }
                        else
                        {
                            if (sj013DAO.checkEMPID(cell_emp_id) == 0)
                            {
                                error +=  cell_emp_id + ":工號不存在,\n";
                            }
                          
                            

                        }
                        //判斷:同一工號可以一次二筆以上,也可以同一筆輸入二種指定方式(EX:職種輚換,E考課除外)
                        //判斷:PS:絕對考課和E考課除外二點一KEY IN
                        string ck_cell_memo = "";
                        for (int k = 2; k <= sheet.LastRowNum; k++)
                        {
                            //ck_cell_remark = sheet.GetRow(k).GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            //ck_cell_abs_score = sheet.GetRow(k).GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            //ck_cell_chg_ws_cd = sheet.GetRow(k).GetCell(6, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            //ck_cell_except_e = sheet.GetRow(k).GetCell(7, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            //if (i != k)
                            //{
                             
                            //}
                          
                        }
                        
                       // error += assess_year + ":" + assess_type + ":" + cell_emp_id + ":" + cell_disting_cd + ":" + cell_remark + ":" + cell_abs_score + ":";
                        cell = sheet.GetRow(i).GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK);
                        cell.CellStyle = style1;
                        cell.SetCellValue(error);
                        if (error.Trim() != "")
                        {
                            valid = false;
                        }
                        else
                        {
                            if (mData.ContainsKey(sheet.GetRow(i).GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim()))
                            {
                             
                            }
                            else
                            {
                                //將資料加入lisData
                                sData = new Dictionary<string, string>();
                                sData.Add("emp_id", sheet.GetRow(i).GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim());                              
                                sData.Add("memo", sheet.GetRow(i).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim());
                                mData.Add(sheet.GetRow(i).GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim(), sData);
                            }
                        }
                    }
                }

                if (valid)
                {
                    BeginTransaction();
                    sj013DAO = new CFB2SJ3410DAO();
                    sj013DAO.ASSESS_YEAR = assess_year;
                    sj013DAO.ASSESS_TYPE = assess_type;
                    sj013DAO.deleteAllData();
                    empid_List = new List<string>();
                    DateTime now = DateTime.Parse(DateTime.Now.ToString());
                    /***
                    for (int i = 1; i <= sheet.LastRowNum; i++)
                    {
                        if (sheet.GetRow(i) != null)
                        {
                            //讀取cell資料，第一欄為檢核結果欄位跳過
                            sj013DAO.ASSESS_YEAR = assess_year;
                            sj013DAO.ASSESS_TYPE = assess_type;
                            sj013DAO.EMP_ID = sheet.GetRow(i).GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            sj013DAO.DISTING_CD = sheet.GetRow(i).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            sj013DAO.DATASOURCE = "U";
                            sj013DAO.REMARK = sheet.GetRow(i).GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            sj013DAO.ABS_SCORE = sheet.GetRow(i).GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            sj013DAO.CHG_WS_CD = sheet.GetRow(i).GetCell(6, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            sj013DAO.EXCEPT_E = sheet.GetRow(i).GetCell(7, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();

                            sj013DAO.CREATED_BY = SessionHandle.Current.emp_id;
                            sj013DAO.UPDATED_BY = SessionHandle.Current.emp_id;
                            sj013DAO.FUNC_ID = "FB2SJ341";
                            if (empid_List.Contains(sheet.GetRow(i).GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim() + "-" + sheet.GetRow(i).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim()))
                            {
                                sj013DAO.updateData();
                            }
                            else
                            {
                                sj013DAO.insertData(now);
                                empid_List.Add(sheet.GetRow(i).GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim() + "-" + sheet.GetRow(i).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim());
                            }
                           
                          
                        }
                    }**/
                    foreach (string k in mData.Keys)
                    {
                        sData = mData[k];
                        sj013DAO.ASSESS_YEAR = assess_year;
                        sj013DAO.ASSESS_TYPE = assess_type;
                        sj013DAO.MEMO = sData["memo"];
                        sj013DAO.EMP_ID = sData["emp_id"];

                        sj013DAO.CREATED_BY = SessionHandle.Current.emp_id;
                        sj013DAO.UPDATED_BY = SessionHandle.Current.emp_id;
                        sj013DAO.FUNC_ID = "FB2SJ341";
                        sj013DAO.insertData(now);
                    }
                    Commit();
                    return null;
                }
                else
                {
                    return workbook;
                }
            }
            else
            {
                return workbook;
            }

        }
        catch (Exception ex)
        {
            RollBack();
            throw;

        }

    }
   
    //檢查是否為數字(正整數)
    public string checkNumber(string cellData, string cellName, int cellLength, string error)
    {
        try
        {
            int numCheckResult = 0;
            cellData = cellData.Replace(",", "");
            if (cellData == "")
                error += cellName + "不可空白\n";
            else
            {
                if (cellData.Trim().Length > cellLength || !int.TryParse(cellData.Trim(), out numCheckResult))
                {
                    error += cellName + "必須為數字, 且長度必須為" + cellLength + ", \n";
                }
            }
            return error;
        }
        catch (Exception)
        {
            throw;
        }



    }
}