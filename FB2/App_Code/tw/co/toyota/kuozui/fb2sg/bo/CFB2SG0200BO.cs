using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.Util;


/// <summary>
/// CFB2DJ010BO 的摘要描述
/// </summary>
public class CFB2SG0200BO : BaseService
{
    public CFB2SG0200BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }


    //Grid的資格檔
    public DataTable getEMPLevelData()
    {
        CFB2SG0200DAO sg020DAO = new CFB2SG0200DAO();
        try
        {
            return sg020DAO.getEMPLevelData();
        }
        catch (Exception)
        {

            throw;
        }
    }
    //Grid的職務檔
    public DataTable getPjobData()
    {
        CFB2SG0200DAO sg020DAO = new CFB2SG0200DAO();
        try
        {
            return sg020DAO.getPjobData();
        }
        catch (Exception)
        {

            throw;
        }
    }


    #region 相關檢核方法

    /// <summary>
    /// 檢核 PK值有無重覆
    /// </summary>
    public string checkPK(CFB2SG0200DAO sg020DAO, string rtnmessage)
    {
        DataTable dt = sg020DAO.getPKData();
        if ((int)dt.Rows[0]["resultCount"] > 0)
        {
            rtnmessage += "節金類別+節日日期+員工區分+節金發放日期 重覆 \\n";
        }
        dt.Clear();
        return rtnmessage;
    }

    /// <summary>
    /// 檢核 PK值(節金條件檔)有無重覆
    /// </summary>
    public string checkPKCAL(CFB2SG0200DAO sg020DAO, string rtnmessage)
    {
        DataTable dt = sg020DAO.getPKCAL();
        if ((int)dt.Rows[0]["resultCount"] > 0)
        {
            rtnmessage += "邏輯+欄位選項+條件+內容 重覆 \\n";
        }

        dt.Clear();
        return rtnmessage;
    }

    #endregion



    //新增(節金維護檔)
    public string insertData(CFB2SG0200DAO sg020DAO)
    {
        string rtnmessage = "";
        try
        {
            //若需要則要進行邏輯檢查(與DB相關的)
            rtnmessage = this.checkPK(sg020DAO, rtnmessage);

            //檢查OK更新
            if (rtnmessage == "")
            {
                try
                {
                    BeginTransaction();
                    sg020DAO.insertData();
                    Commit();

                    return "0";
                }
                catch (Exception ex)
                {
                    RollBack();
                    return ex.Message;
                }

            }
            return rtnmessage;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    //新增(節金條件檔)
    public string insertDataCAL(CFB2SG0200DAO sg020DAO)
    {
        string rtnmessage = "";
        try
        {
            //若需要則要進行邏輯檢查(與DB相關的)
            rtnmessage = this.checkPKCAL(sg020DAO, rtnmessage);

            //檢查OK更新
            if (rtnmessage == "")
            {
                try
                {
                    BeginTransaction();
                    sg020DAO.insertDataCAL();
                    Commit();

                    return "0";
                }
                catch (Exception ex)
                {
                    RollBack();
                    return ex.Message;
                }

            }
            return rtnmessage;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    //修改
    public string updateData(CFB2SG0200DAO dao)
    {
        string rtnmessage = "";
        try
        {

            //若需要則要進行邏輯檢查

            //檢查OK更新
            if (rtnmessage == "")
            {
                try
                {
                    BeginTransaction();

                    dao.updateData();

                    Commit();

                    return "0";
                }
                catch (Exception ex)
                {
                    RollBack();
                    return ex.Message;
                }

            }
            return rtnmessage;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    //刪除(Qry)
    public string deleteData(List<Tuple<string, string, string, string>> keysList)
    {
        CFB2SG0200DAO sg020DAO = new CFB2SG0200DAO();
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
                        //刪除 節金維護檔
                        sg020DAO.deleteDataMH(item.Item1, item.Item2, item.Item3, item.Item4);
                        //刪除 節金明細主檔
                        sg020DAO.deleteDataRD(item.Item1, item.Item2, item.Item3, item.Item4);
                        //刪除 節金明細維護檔
                        sg020DAO.deleteDataMD(item.Item1, item.Item2, item.Item3, item.Item4);
                        //刪除 節金條件檔
                        sg020DAO.deleteDataMCAL(item.Item1, item.Item2, item.Item3, item.Item4);
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


    //刪除(Dtl)
    public string deleteDataDtl(List<Tuple<string, string, string, string, string>> keysList)
    {
        CFB2SG0200DAO sg020DAO = new CFB2SG0200DAO();
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
                        //刪除 節金條件檔
                        sg020DAO.deleteDataDtl(item.Item1, item.Item2, item.Item3, item.Item4, item.Item5);
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

    //節金對象生成
    public string execSP_S_FESTIVAL_DATA(CFB2SG0200DAO sg020DAO)
    {
        string rtnmessage = "";//存在檢查後的訊息

        try
        {

            //檢查完成後，逐筆進行刪除
            if (rtnmessage == "")
            {
                sg020DAO.execSP_S_FESTIVAL_DATA();
                rtnmessage += utilities.getSPLOG("SP_S_FESTIVAL_DATA");
                if (rtnmessage != "")
                {
                    return rtnmessage;
                }
               
                return "0";
               
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

    //EXCEL匯入
    public IWorkbook uploadExcel(Stream fs, string type, CFB2SG0200DAO sg020DAO)
    {
        try
        {

            //1.建立一個tmp的節金明細維護檔
            BeginTransaction();
            sg020DAO.dropFestivaltemp();
            Commit();
            BeginTransaction();
            sg020DAO.createFestivaltemp();
            Commit();

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

            //取得參數檔-獎金類所得稅率
            DataTable dt = sg020DAO.getTaxRate();
            double taxRate = dt.Rows[0]["taxRate"].ToString() != "" ? Convert.ToDouble(dt.Rows[0]["taxRate"].ToString()) : 0;

            //取得參數檔-所得稅代扣金額下限
            dt = sg020DAO.getLimitLow();
            int limitLow = dt.Rows[0]["limitLow"].ToString() != "" ? Convert.ToInt32(dt.Rows[0]["limitLow"].ToString()) : 0;

            DateTime now = DateTime.Parse(DateTime.Now.ToString());

            //取得sheet
            ISheet sheet = workbook.GetSheetAt(0);
            ICellStyle style1 = workbook.CreateCellStyle();
            IFont font1 = workbook.CreateFont();
            font1.Color = HSSFColor.Red.Index;
            style1.SetFont(font1);

            if (sheet != null)
            {
                try
                {
                    BeginTransaction();
                    //1.刪除 節金明細維護檔
                    //sg020DAO.deleteTarget();
                   

                    //2.累計總金額及總人數
                    int amt_total = 0;
                    int num_total = 0;

                    //3.取得的資料
                    string cell_empID = "";
                    string cell_AMT = "";
                    string cell_payType = "";
                    string cell_workDays = "";
                    string cell_1001 = "";
                    string cell_1002 = "";
                    string cell_1003 = "";
                    string cell_1004 = "";
                    string cell_food = "";


                    string error = "";
                    //巡覽每row的資料第一列為title跳過(故i從1開始)
                    for (int i = 1; i <= sheet.LastRowNum; i++)
                    {
                        error = "";
                        if (sheet.GetRow(i) != null)
                        {
                            //讀取cell資料，第一欄為檢核結果欄位跳過
                            cell_empID = sheet.GetRow(i).GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            cell_AMT = sheet.GetRow(i).GetCell(8, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().Replace(",", "");
                            cell_payType = sheet.GetRow(i).GetCell(9, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            cell_workDays = sheet.GetRow(i).GetCell(10, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().Replace(",", "");
                            cell_1001 = sheet.GetRow(i).GetCell(11, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().Replace(",", "");
                            cell_1002 = sheet.GetRow(i).GetCell(12, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().Replace(",", "");
                            cell_1003 = sheet.GetRow(i).GetCell(13, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().Replace(",", "");
                            cell_1004 = sheet.GetRow(i).GetCell(14, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().Replace(",", "");
                            cell_food = sheet.GetRow(i).GetCell(15, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().Replace(",", "");

                             
                            //工號 不可空白
                            if (cell_empID == "")
                            {
                                error += "工號欄位不可空白,\n";
                            }
                            else
                            {
                                dt.Clear();
                                dt = sg020DAO.getEmpCount(cell_empID);
                                if ((int)dt.Rows[0]["resultCount"] == 0)
                                {
                                    error += "工號不存在,\n";
                                }
                                else
                                {
                                    sg020DAO.EMP_ID = cell_empID;
                                    sg020DAO.getEmpData();
                                    if (sg020DAO.EMP_CD != sg020DAO.EMP_CD_PK)
                                    {
                                        error += "此工號員工區分不符此節金, \n";
                                    }
                                }
                            }
                            //工號不能重覆
                            dt.Clear();
                            dt = sg020DAO.getFestivalEmpCountFromTemp(cell_empID);
                            if ((int)dt.Rows[0]["resultCount"] > 0)
                            {
                                //error += "工號已存在\n";
                                //刪除舊資料
                                sg020DAO.deleteFestivalEmp(cell_empID);
                            }

                            //支付狀態  不可空白
                            if (cell_payType == "")
                            {
                                error += "支付狀態不可空白,\n";
                            }
                            else
                            {
                                dt.Clear();
                                dt = sg020DAO.getPayTypeCount(cell_payType);
                                if ((int)dt.Rows[0]["resultCount"] == 0)
                                {
                                    error += "此支付狀態不存在,\n";
                                }

                            }
                            //檢查數字欄位
                            error += this.checkNumber(cell_AMT, "節金金額", 7, "");
                            error += this.checkNumber(cell_workDays, "在職年資(天)", 5, "");
                            error += this.checkNumber(cell_1001, "職能俸", 7, "");
                            error += this.checkNumber(cell_1002, "資格俸", 7, "");
                            error += this.checkNumber(cell_1003, "職務俸", 7, "");
                            error += this.checkNumber(cell_1004, "專業俸", 7, "");
                            error += this.checkNumber(cell_food, "伙食津貼", 7, "");

                            //儲存錯誤訊息
                            sheet.GetRow(i).CreateCell(0).CellStyle = style1;
                            sheet.GetRow(i).GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(error);
                            if (error != "")
                            {
                                valid = false;
                            }


                            //檢核無誤時,進行新增的作業
                            if (valid)
                            {
                                //新增節金明細維護檔
                                sg020DAO.EMP_ID = cell_empID;
                                sg020DAO.getEmpData();
                                
                                //俸給
                                sg020DAO.ABILITY_PAY = cell_1001;
                                sg020DAO.LEVEL_PAY = cell_1002;
                                sg020DAO.PJOB_PAY = cell_1003;
                                sg020DAO.PROFESSION_PAY = cell_1004;
                                sg020DAO.FOOD_SUBSIDY = cell_food;

                                //其它EXCEL資訊
                                sg020DAO.FESTIVAL_AMT = cell_AMT;
                                sg020DAO.PAY_TYPE = cell_payType;
                                sg020DAO.WORK_DAYS = cell_workDays;

                                int fAMT = Convert.ToInt32(cell_AMT);
                                double fTAX = 0;
                                double fAMT_R = 0;
                                if (fAMT < limitLow)
                                {
                                    fTAX = 0;
                                    fAMT_R = fAMT - fTAX;
                                }
                                else
                                {
                                    fTAX = fAMT * taxRate;
                                    fAMT_R = fAMT - fTAX;
                                }
                                sg020DAO.FESTIVAL_AMT = Convert.ToInt32(fAMT).ToString();
                                sg020DAO.FESTIVAL_TAX = Convert.ToInt32(fTAX).ToString();
                                sg020DAO.FESTIVAL_AMT_R = Convert.ToInt32(fAMT_R).ToString();

                                sg020DAO.CREATED_BY = SessionHandle.Current.emp_id;
                                sg020DAO.UPDATED_BY = SessionHandle.Current.emp_id;
                                sg020DAO.FUNC_ID = "FB2SG020";

                                amt_total += fAMT;
                                num_total += 1;

                                sg020DAO.insertTarget(now);
                            }
                        }
                    }


                    if (!valid)
                    {
                        RollBack();
                        
                        BeginTransaction();
                        sg020DAO.dropFestivaltemp();
                        Commit();
                        //檢核有錯，匯出附加說明的excel
                        return workbook;
                        //ExcelHandle.exportExcel(workbook, "檢核錯誤說明" + type);
                    }
                    else
                    {
                        //更新節金維護檔
                        sg020DAO.updateTarget_H(amt_total, num_total, now);
                        sg020DAO.dropFestivaltemp();
                        Commit();
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    RollBack();
                    BeginTransaction();
                    sg020DAO.dropFestivaltemp();
                    Commit();

                    throw;
                }
            }

            return null;
        }
        catch (Exception ex)
        {
            throw;
        }

    }


    //檢查是否為數字(正整數)
    public string checkNumber(string cellData, string cellName, int cellLength, string error)
    {
        int numCheckResult = 0;
        cellData = cellData.Replace(",", "");
        if (cellData == "")
            error += cellName + "不可空白\n";
        else
        {
            if (cellData.Trim().Length > cellLength || !int.TryParse(cellData.Trim(), out numCheckResult))
            {
                error += cellName + "必須為數字, 且長度必須為" + cellLength + " \n";
            }
        }

        return error;
    }


    //檢查是否為英數字
    public string checkEngNumber(string cellData, string cellName, int cellLength, string error)
    {
        if (cellData == "")
            error += cellName + "不可空白\n";
        else
        {
            if (cellData.Trim().Length > cellLength || !utilities.IsNatural_Number(cellData))
            {
                error += cellName + "必須為英數字, 且長度必須為" + cellLength + " \n";
            }
        }

        return error;
    }


}