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
public class CFB2SJ0130BO : BaseService
{
    public CFB2SJ0130BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    //刪除
    public string deleteData(List<Tuple<string, string, string, string, string>> keysList)
    {
        CFB2SJ0130DAO sj013DAO = new CFB2SJ0130DAO();
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
                        sj013DAO.DISTING_CD = item.Item4;
                        sj013DAO.DATASOURCE = item.Item5;
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
            CFB2SJ0130DAO sj013DAO = new CFB2SJ0130DAO();
            CFB2SJ0400DAO sj040DAO = new CFB2SJ0400DAO();
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
                string cell_disting_cd = "";
                string cell_remark = "";
                string cell_abs_score = "";
                string cell_chg_ws_cd = "";
                string cell_except_e = "";
                Dictionary<string, Dictionary<string, string>> mData = new Dictionary<string, Dictionary<string, string>>();
                Dictionary<string, string> sData = new Dictionary<string, string>();
                List<Dictionary<string, Dictionary<string, string>>> liData = new List<Dictionary<string, Dictionary<string, string>>>();

                //錯誤訊息
                string error = "";
                List<string> empid_List = new List<string>();
                ICell cell;
                //巡覽每row的資料第一列為title跳過
                CFB2SJ0120DAO sj0120DAO = new CFB2SJ0120DAO();
                for (int i = 1; i <= sheet.LastRowNum; i++)
                {
                    //清空
                    error = "";

                    if (sheet.GetRow(i) != null)
                    {
                        //讀取cell資料，第一欄為檢核結果欄位跳過
                         cell_emp_id = sheet.GetRow(i).GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                         cell_disting_cd = sheet.GetRow(i).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                         cell_remark = sheet.GetRow(i).GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                         cell_abs_score = sheet.GetRow(i).GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                         cell_chg_ws_cd = sheet.GetRow(i).GetCell(6, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                         cell_except_e = sheet.GetRow(i).GetCell(7, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();

                         


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
                            if (cell_abs_score != "" && cell_abs_score != "A" && cell_abs_score != "B" && cell_abs_score != "C" && cell_abs_score != "D" && cell_abs_score != "E")
                            {
                                error += "絕對考課只能輸入A~E,\n";
                            }
                            if (!checkWSCD(dt,cell_chg_ws_cd))
                            {
                                error += "職種轉換輸入職種不存在,\n";
                            }
                            if (cell_except_e != "" && cell_except_e != "Y")
                            {
                                error += "E考課除外僅能空白或Y,\n";
                            }
                            if (cell_abs_score != "" && cell_except_e != "")
                            {
                                error += "絕對考課和E考課除外只能輸人一欄,\n";
                            }
                            sj0120DAO.DISTING_CD = cell_disting_cd;
                            DataTable dcDT=sj0120DAO.getUpdData();
                            if (dcDT.Rows.Count > 0)
                            {
                                if(dcDT.Rows[0]["USER_UP_YN"].ToString()!="Y"){

                                    error += "非當擔自行決定考核區分代碼,\n";
                                }
                            }
                            else
                            {
                                error += "考核區分代碼不存在,\n";
                            }

                        }
                        //判斷:同一工號可以一次二筆以上,也可以同一筆輸入二種指定方式(EX:職種輚換,E考課除外)
                        //判斷:PS:絕對考課和E考課除外二點一KEY IN
                        string ck_cell_remark = "";
                        string ck_cell_abs_score = "";
                        string ck_cell_chg_ws_cd = "";
                        string ck_cell_except_e = "";
                        for (int k = 1; k <= sheet.LastRowNum; k++)
                        {
                            //ck_cell_remark = sheet.GetRow(k).GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            //ck_cell_abs_score = sheet.GetRow(k).GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            //ck_cell_chg_ws_cd = sheet.GetRow(k).GetCell(6, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            //ck_cell_except_e = sheet.GetRow(k).GetCell(7, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            //if (i != k)
                            //{
                                if (cell_emp_id == sheet.GetRow(k).GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim() &&
                                    cell_disting_cd == sheet.GetRow(k).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim())
                                {

                                    if ("" != sheet.GetRow(k).GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim())
                                    {
                                        if (cell_remark != "" && cell_remark != sheet.GetRow(k).GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim())
                                        {
                                            error += "與第" + k + "筆備考內容填寫不一致,\n";
                                        }
                                        else
                                        {
                                            ck_cell_remark = sheet.GetRow(k).GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                                        }

                                    }
                                    if ("" != sheet.GetRow(k).GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim())
                                    {
                                        if (cell_abs_score != "" && cell_abs_score != sheet.GetRow(k).GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim())
                                        {
                                            error += "與第" + k + "筆絕對考課填寫不一致,\n";
                                        }
                                        else
                                        {
                                            ck_cell_abs_score = sheet.GetRow(k).GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                                        }

                                    }
                                    if ("" != sheet.GetRow(k).GetCell(6, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim())
                                    {
                                        if (cell_chg_ws_cd != "" && cell_chg_ws_cd != sheet.GetRow(k).GetCell(6, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim())
                                        {
                                            error += "與第" + k + "筆職種轉換填寫不一致,\n";
                                        }
                                        else
                                        {
                                            ck_cell_chg_ws_cd = sheet.GetRow(k).GetCell(6, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                                        }

                                    }
                                    if ("" != sheet.GetRow(k).GetCell(7, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim())
                                    {
                                        if (cell_except_e != "" && cell_except_e != sheet.GetRow(k).GetCell(7, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim())
                                        {
                                            error += "與第" + k + "筆E考課除外填寫不一致,\n";
                                        }
                                        else
                                        {
                                            ck_cell_except_e = sheet.GetRow(k).GetCell(7, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                                        }

                                    }
                                }
                            //}
                          
                        }
                        if (ck_cell_abs_score != "" && ck_cell_except_e != "")
                        {
                            error += "絕對考課和E考課除外只能輸人一欄,\n";
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
                                if (sData["remark"] == "") sData["remark"] = sheet.GetRow(i).GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                                 if (sData["abs_score"] == "") sData["abs_score"] = sheet.GetRow(i).GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                                 if (sData["chg_ws_cd"] == "") sData["chg_ws_cd"] = sheet.GetRow(i).GetCell(6, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                                 if (sData["except_e"] == "") sData["except_e"] = sheet.GetRow(i).GetCell(7, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                                 mData[sheet.GetRow(i).GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim() + "-" + sheet.GetRow(i).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim()] = sData;
                            }
                            else
                            {
                                //將資料加入lisData
                                sData = new Dictionary<string, string>();
                                sData.Add("emp_id", sheet.GetRow(i).GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim());
                                sData.Add("disting_cd", sheet.GetRow(i).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim());
                                sData.Add("remark", sheet.GetRow(i).GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim());
                                sData.Add("abs_score", sheet.GetRow(i).GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim());
                                sData.Add("chg_ws_cd", sheet.GetRow(i).GetCell(6, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim());
                                sData.Add("except_e", sheet.GetRow(i).GetCell(7, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim());
                                mData.Add(sheet.GetRow(i).GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim() + "-" + sheet.GetRow(i).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim(), sData);
                            }
                        }
                    }
                }

                if (valid)
                {
                    BeginTransaction();
                    sj013DAO = new CFB2SJ0130DAO();
                    sj013DAO.ASSESS_YEAR = assess_year;
                    sj013DAO.ASSESS_TYPE = assess_type;
                    sj013DAO.DATASOURCE = "U";
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
                        sj013DAO.ASSESS_YEAR = assess_year;
                        sj013DAO.ASSESS_TYPE = assess_type;
                        sj013DAO.EMP_ID = sData["emp_id"];
                        sj013DAO.DISTING_CD = sData["disting_cd"];
                        sj013DAO.DATASOURCE = "U";
                        sj013DAO.REMARK = sData["remark"];
                        sj013DAO.ABS_SCORE = sData["abs_score"];
                        sj013DAO.CHG_WS_CD = sData["chg_ws_cd"];
                        sj013DAO.EXCEPT_E = sData["except_e"];

                        sj013DAO.CREATED_BY = SessionHandle.Current.emp_id;
                        sj013DAO.UPDATED_BY = SessionHandle.Current.emp_id;
                        sj013DAO.FUNC_ID = "FB2SJ013";
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