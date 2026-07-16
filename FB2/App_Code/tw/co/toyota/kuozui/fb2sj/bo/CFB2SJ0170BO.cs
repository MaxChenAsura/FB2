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
public class CFB2SJ0170BO : BaseService
{
    public CFB2SJ0170BO()
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
            CFB2SJ0170DAO sj0170DAO = new CFB2SJ0170DAO(); 
            CFB2SJ0130DAO sj013DAO = new CFB2SJ0130DAO();
            //DataTable dt = utilities.getCommCode("HB", "WS_CD", "", "");
            DataTable dtPointGroup = sj0170DAO.getPointGroupData(assess_year, assess_type);
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
                string cell_dept_no = "";
                string cell_ws_cd = "";
                string cell_point_group = "";
                string s_cell_pg_peo = "";
                string s_cell_point = "";
                int cell_pg_peo = 0;
                int cell_point = 0;
                string cell_assess_year = "";
                string cell_assess_type = "";
                //:依畫面.考核年度+畫面.考核類別至<< TB_S_M_ASSESS_TARGET 考核人事資料維護檔>>是否有資料,若無資料則 MSG:畫面.考核年度+"/"+畫面.考核類別+"尚未執行對象生成,不允匯入。"
                cell_assess_year = sheet.GetRow(1).GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                cell_assess_type = sheet.GetRow(1).GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                
                if (sheet.GetRow(0).GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim() != "部門核定點數表")
                {
                    throw new Exception("XLSX格式不符。");
                }
                if (assess_year != cell_assess_year || assess_type != cell_assess_type)
                {
                    throw new Exception("畫面.(考核年度+考核類別)與XLXS 內容不符。");
                }
                int targetCount = sj0170DAO.getTargetCount(assess_year, assess_type);
                if (targetCount == 0)
                {
                    throw new Exception(assess_year+"/"+assess_type+"尚未執行對象生成,不允匯入。");
                }
                
                Dictionary<string, Dictionary<string, string>> mData = new Dictionary<string, Dictionary<string, string>>();
                Dictionary<string, string> sData = new Dictionary<string, string>();
                List<Dictionary<string, Dictionary<string, string>>> liData = new List<Dictionary<string, Dictionary<string, string>>>();

                //錯誤訊息
                string error = "";
                List<string> empid_List = new List<string>();
                ICell cell;
                bool ckPointGroup = false;
                DataTable dtWSCD = sj0170DAO.getWSCD();
                //巡覽每row的資料第一列為title跳過
                for (int i = 3; i <= sheet.LastRowNum; i++)
                {
                    //清空
                    error = "";
                    ckPointGroup = false;
                    if (sheet.GetRow(i) != null)
                    {
                        //讀取cell資料，第一欄為檢核結果欄位跳過
                        cell_dept_no = sheet.GetRow(i).GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                        cell_ws_cd = sheet.GetRow(i).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                        cell_point_group = sheet.GetRow(i).GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                        s_cell_pg_peo = sheet.GetRow(i).GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                        s_cell_point = sheet.GetRow(i).GetCell(6, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                        bool ckWSCD=false;
                        if (dtWSCD.Rows.Count > 0)
                        {
                            for (int k= 0;k < dtWSCD.Rows.Count; k++)
                            {
                                if (dtWSCD.Rows[k]["WS_CD"].ToString() == cell_ws_cd.ToUpper()) ckWSCD = true;
                            }
                        }
                        if (!ckWSCD)
                        {
                            error += "職種不存在,\n";
                        }

                        if (cell_dept_no == "")
                        {
                            error += "部門代號不可空白,\n";
                        }
                        if (cell_ws_cd == "")
                        {
                            error += "職種不可空白,\n";
                        }
                        if (cell_point_group == "")
                        {
                            error += "點數群組編號不可空白,\n";
                        }
                        else
                        {
                            if (dtPointGroup.Rows.Count > 0)
                            {
                                for (int j = 0;j< dtPointGroup.Rows.Count; j++)
                                {
                                    if (cell_point_group == dtPointGroup.Rows[j]["POINT_GROUP"].ToString() && cell_ws_cd == dtPointGroup.Rows[j]["WS_CD"].ToString()) ckPointGroup = true;
                                }
                            }
                            if (!ckPointGroup) error += "點數資格群組錯誤,\n";
                        }
                        bool isNumeric = int.TryParse(s_cell_pg_peo, out cell_pg_peo);
                        if (!isNumeric)
                        {
                            error += "群組人數不允許空白,不允許0,不允許負數,\n";
                        }

                        if (cell_pg_peo < 0)
                        {
                            error += "群組人數不允許空白,不允許0,不允許負數,\n";
                        }
                        else
                        {
                            if (cell_pg_peo > 9999) error += "群組人數僅能輸入4碼,\n";

                        }
                        int realPeo = sj0170DAO.getRealDeptPeo(assess_year, assess_type, cell_dept_no,cell_ws_cd, cell_point_group);
                        if (realPeo != cell_pg_peo) error += "系統目前統計人數,與XLSX.人數不符,\n";
                        isNumeric = int.TryParse(s_cell_point, out cell_point);
                        if (!isNumeric)
                        {
                            error += "核定點數不允許空白,不允許0,不允許負數,\n";
                        }
                        if (cell_point < 0)
                        {
                            error += "核定點數不允許空白,不允許0,不允許負數,\n";
                        }
                        else
                        {
                            if (cell_point > 9999) error += "核定點數僅能輸入4碼,\n";

                        }
                        if (mData.ContainsKey(cell_dept_no + "-" + cell_ws_cd + "-" + cell_point_group))
                        {
                            error += "序號" + (i+1).ToString() + "與序號" + mData[cell_dept_no + "-" + cell_point_group]["sn"] + ",部門代號+職種+點數群組重複,\n";                           
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
                                //將資料加入lisData
                                sData = new Dictionary<string, string>(); 
                                sData.Add("sn", (i+1).ToString());
                                sData.Add("dept_no_20", cell_dept_no);
                                sData.Add("dept_name_20", sheet.GetRow(i).GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim());
                                sData.Add("ws_cd", sheet.GetRow(i).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim());
                                sData.Add("point_group", cell_point_group);
                                sData.Add("dept_peo", cell_pg_peo.ToString());
                                sData.Add("dept_point", cell_point.ToString());
                                mData.Add(cell_dept_no + "-" +cell_ws_cd+"-"+ cell_point_group, sData);
                          
                        }
                    }
                }

                if (valid)
                {
                    BeginTransaction();
                    sj0170DAO = new CFB2SJ0170DAO();
                    sj0170DAO.ASSESS_YEAR = assess_year;
                    sj0170DAO.ASSESS_TYPE = assess_type;
                    sj0170DAO.deleteAllData();
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
                        sj0170DAO.ASSESS_YEAR = assess_year;
                        sj0170DAO.ASSESS_TYPE = assess_type;
                        sj0170DAO.DEPT_NO_20 = sData["dept_no_20"];
                        sj0170DAO.DEPT_NAME_20 = sData["dept_name_20"];
                        sj0170DAO.WS_CD = sData["ws_cd"];
                        sj0170DAO.POINT_GROUP = sData["point_group"];
                        sj0170DAO.DEPT_PEO =Convert.ToInt32( sData["dept_peo"]);
                        sj0170DAO.DEPT_POINT = Convert.ToInt32(sData["dept_point"]);

                        sj0170DAO.CREATED_BY = SessionHandle.Current.emp_id;
                        sj0170DAO.UPDATED_BY = SessionHandle.Current.emp_id;
                        sj0170DAO.FUNC_ID = "FB2SJ0170";
                        sj0170DAO.insertData(now);
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