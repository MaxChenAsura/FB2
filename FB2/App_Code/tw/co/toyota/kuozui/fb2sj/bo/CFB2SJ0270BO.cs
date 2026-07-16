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
public class CFB2SJ0270BO : BaseService
{
    public CFB2SJ0270BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    



    //EXCEL上傳
    public IWorkbook uploadExcel(Stream fs, string type, string assess_year, string assess_type)
    {

        try
        {
            CFB2SJ0270DAO sj027DAO = new CFB2SJ0270DAO(); 
            CFB2SJ0130DAO sj013DAO = new CFB2SJ0130DAO();
            DataTable dt = utilities.getCommCode("HB", "WS_CD", "", "");
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
                string cell_ws_cd = "";
                Dictionary<string, Dictionary<string, string>> mData = new Dictionary<string, Dictionary<string, string>>();
                Dictionary<string, string> sData = new Dictionary<string, string>();
                List<Dictionary<string, Dictionary<string, string>>> liData = new List<Dictionary<string, Dictionary<string, string>>>();

                //錯誤訊息
                string error = "";
                List<string> empid_List = new List<string>();
                ICell cell;
                //巡覽每row的資料第一列為title跳過
                for (int i = 1; i <= sheet.LastRowNum; i++)
                {
                    //清空
                    error = "";

                    if (sheet.GetRow(i) != null)
                    {
                        //讀取cell資料，第一欄為檢核結果欄位跳過
                         cell_emp_id = sheet.GetRow(i).GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                         cell_ws_cd = sheet.GetRow(i).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();

                        


                        if (cell_emp_id == "")
                        {
                            error += "工號不可空白,\n";
                        }
                        else
                        {
                            if (sj013DAO.checkEMPID(cell_emp_id) == 0)
                            {
                                error += assess_year + " " + assess_type + " " + cell_emp_id + ":工號不存在或非在職人員,\n";
                            }
                            if (!checkWSCD(dt,cell_ws_cd))
                            {
                                error += "職種轉換輸入職種不存在,\n";
                            }


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
                            if (mData.ContainsKey(sheet.GetRow(i).GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim() + "-" + sheet.GetRow(i).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim()))
                            {
                                sData = mData[sheet.GetRow(i).GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim() + "-" + sheet.GetRow(i).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim()];
                                if (sData["ws_cd"] == "") sData["ws_cd"] = sheet.GetRow(i).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                                mData[sheet.GetRow(i).GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim() + "-" + sheet.GetRow(i).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim()] = sData;
                            }
                            else
                            {
                                //將資料加入lisData
                                sData = new Dictionary<string, string>();
                                sData.Add("emp_id", sheet.GetRow(i).GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim());
                                sData.Add("ws_cd", sheet.GetRow(i).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim());
                                mData.Add(sheet.GetRow(i).GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim() + "-" + sheet.GetRow(i).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim(), sData);
                            }
                        }
                    }
                }

                if (valid)
                {
                    BeginTransaction();
                    sj027DAO = new CFB2SJ0270DAO();
                    sj027DAO.ASSESS_YEAR = assess_year;
                    sj027DAO.ASSESS_TYPE = assess_type;
                    sj027DAO.deleteAllData();
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
                            sj013DAO.FUNC_ID = "FB2SJ013";
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
                        sj027DAO.ASSESS_YEAR = assess_year;
                        sj027DAO.ASSESS_TYPE = assess_type;
                        sj027DAO.EMP_ID = sData["emp_id"];
                        sj027DAO.WS_CD = sData["ws_cd"];

                        sj027DAO.CREATED_BY = SessionHandle.Current.emp_id;
                        sj027DAO.UPDATED_BY = SessionHandle.Current.emp_id;
                        sj027DAO.FUNC_ID = "FB2SJ027";
                        sj027DAO.insertData(now);
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
    public Boolean checkWSCD(DataTable dtWSCD, String wscd)
    {
        Boolean result=false;
        if (wscd == "") return true;
        if (dtWSCD.Rows.Count > 0)
        {
            for (int i = 0; i < dtWSCD.Rows.Count; i++)
            {
                if (wscd == dtWSCD.Rows[i]["SUB_CD"].ToString()) return true;
            }
        }
        return result;

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