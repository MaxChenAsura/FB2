using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;

/// <summary>
/// CFB2DJ010BO 的摘要描述
/// </summary>
public class CFB2SG0300BO : BaseService
{
    public CFB2SG0300BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    //Grid的員工工號 fro  ajax
    public DataTable getEmpData(string emp_id)
    {
        CFB2SG0300DAO sg030DAO = new CFB2SG0300DAO();
        try
        {
            return sg030DAO.getEmpData(emp_id);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //提出核可
    public string updateRelease(CFB2SG0300DAO sg030DAO)
    {

        string rtnmessage = "";//存在檢查後的訊息
        try
        {
            if (rtnmessage == "")
            {
                try
                {
                    BeginTransaction();

                    sg030DAO.updateRelease();

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

    //薪資轉出
    public string updateAnnounce(CFB2SG0300DAO sg030DAO)
    {

        string rtnmessage = "";//存在檢查後的訊息
        try
        {
            if (rtnmessage == "")
            {
                try
                {
                    BeginTransaction();
                    DateTime now = DateTime.Parse(DateTime.Now.ToString());
                    sg030DAO.updateAnnounce(now);
                    //刪除-節金條件設定歷史檔
                    sg030DAO.deleteLog();
                    //刪除-節金條件設定歷史檔
                    sg030DAO.insertLog(now);
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


    //修改 明細的節金額或支付狀態
    public string updateDataDtl(CFB2SG0300DAO sg030DAO)
    {

        string rtnmessage = "";//存在檢查後的訊息
        try
        {
            if (rtnmessage == "")
            {
                try
                {
                    DateTime now = DateTime.Parse(DateTime.Now.ToString());
                    BeginTransaction();
                    DataTable dt = sg030DAO.getDetailData();
                    string payType_old = dt.Rows[0]["PAY_TYPE"].ToString();
                    string festivalAMT_old = dt.Rows[0]["FESTIVAL_AMT"].ToString();
                    string approveFlag = dt.Rows[0]["APPROVE_FLAG"].ToString();
                    string chgStatus = (string)dt.Rows[0]["CHG_STATUS"];
                    if (chgStatus == "N" && approveFlag == "N")
                    {
                        sg030DAO.CHG_STATUS = "N"; //新增
                    }
                    else
                    {
                        sg030DAO.CHG_STATUS = "U";  //修改
                    }
                    sg030DAO.FESTIVAL_AMT_OLD = festivalAMT_old;
                    sg030DAO.PAY_TYPE_OLD = payType_old;

                    //取得參數檔-獎金類所得稅率
                    dt = utilities.getParameter("SL", "BOUNS_TAX_RATE");
                    double taxRate = 0;
                    if (dt.Rows[0]["CODE_VAL1"].ToString() != "")
                    {
                        taxRate = Convert.ToDouble(dt.Rows[0]["CODE_VAL1"].ToString());
                    }
                    //取得參數檔-所得稅代扣金額下限
                    dt = utilities.getParameter("SL", "INCOME_LIMIT_LOW");
                    double incomeLimit = 0;
                    if (dt.Rows[0]["CODE_VAL1"].ToString() != "")
                    {
                        incomeLimit = Convert.ToDouble(dt.Rows[0]["CODE_VAL1"].ToString());
                    }


                    //計算節金稅額
                    double amt = Convert.ToDouble(sg030DAO.FESTIVAL_AMT);
                    int amtR = 0;
                    int festivalTax = 0;
                    sg030DAO.FESTIVAL_TAX = Convert.ToString(festivalTax);
                    sg030DAO.FESTIVAL_AMT_R = Convert.ToString(amtR);
                    if (amt < incomeLimit)
                    {
                        sg030DAO.FESTIVAL_TAX = "0";
                    }
                    else
                    {
                        festivalTax = Convert.ToInt32(amt * taxRate);
                        amtR = Convert.ToInt32(amt) - festivalTax;
                        sg030DAO.FESTIVAL_TAX = Convert.ToString(festivalTax);
                        sg030DAO.FESTIVAL_AMT_R = Convert.ToString(amtR);
                    }


                    //明細核可狀態
                    dt = sg030DAO.getHeaderData();
                    string approveStatus = dt.Rows[0]["APPROVE_STATUS"].ToString();
                    if (approveStatus == "Y")
                    {
                        sg030DAO.APPROVE_FLAG = "N"; //新增
                    }
                    else
                    {
                        sg030DAO.APPROVE_FLAG = approveFlag;
                    }
                    sg030DAO.updateDataDtl_D(now);

                    Commit();

                    BeginTransaction();
                    //更新節金維護檔 及更新總金額及總人數 
                    sg030DAO.updateStatus_H(now);

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


    //新增 明細的資料
    public string insertDataDtl(CFB2SG0300DAO sg030DAO)
    {

        string rtnmessage = "";//存在檢查後的訊息
        try
        {
            //若需要則要進行邏輯檢查
            DataTable dt = sg030DAO.getEmpBasicData();

            //0.檢查PK值有無重覆
            //.判斷是否可以新增該類型的員工區分
            string empCD = dt.Rows[0]["EMP_CD"].ToString();
            DataTable dt_dmpCD = sg030DAO.getEMPCDData(empCD);
            if ((int)dt_dmpCD.Rows[0]["resultCount"] == 0)
            {
                rtnmessage += " 此員工區分不能在此組節金類別新增 \\n";
            }
            else
            {

                DataTable dtPK = sg030DAO.getPKData();
                if ((int)dtPK.Rows[0]["resultCount"] > 0)
                {
                    rtnmessage += "工號 重覆 \\n";
                }
            }


            if (rtnmessage == "")
            {
                try
                {
                    DateTime now = DateTime.Parse(DateTime.Now.ToString());
                    BeginTransaction();
                    sg030DAO.EMP_NAME = dt.Rows[0]["EMP_NAME"].ToString();
                    sg030DAO.DEPT_NO = dt.Rows[0]["DEPT_NO"].ToString();
                    sg030DAO.PLANT_CD = dt.Rows[0]["PLANT_CD"].ToString();
                    sg030DAO.JPN_CD = dt.Rows[0]["JPN_CD"].ToString();
                    sg030DAO.COMPANY_CD = dt.Rows[0]["COMPANY_CD"].ToString();
                    sg030DAO.LEVEL_CD = dt.Rows[0]["LEVEL_CD"].ToString();
                    sg030DAO.GRADE_CD = dt.Rows[0]["GRADE_CD"].ToString();
                    sg030DAO.PJOB_CD = dt.Rows[0]["PJOB_CD"].ToString();
                    sg030DAO.JOIN_DT = dt.Rows[0]["JOIN_DT"].ToString();
                    sg030DAO.WORK_DAYS = dt.Rows[0]["WORK_DAYS"].ToString();
                    sg030DAO.EMP_CD = dt.Rows[0]["EMP_CD"].ToString();
                    sg030DAO.EMP_CHG_CD = dt.Rows[0]["EMP_CHG_CD"].ToString();
                    sg030DAO.WS_CD = dt.Rows[0]["WS_CD"].ToString();
                    sg030DAO.SEX_CD = dt.Rows[0]["SEX_CD"].ToString();

                    //取得參數檔-獎金類所得稅率
                    dt = utilities.getParameter("SL", "BOUNS_TAX_RATE");
                    double taxRate = 0;
                    if (dt.Rows.Count > 0)
                    {
                        taxRate = Convert.ToDouble(dt.Rows[0]["CODE_VAL1"].ToString());
                    }
                    //取得參數檔-所得稅代扣金額下限
                    dt = utilities.getParameter("SL", "INCOME_LIMIT_LOW");
                    double incomeLimit = 0;
                    if (dt.Rows.Count > 0)
                    {
                        incomeLimit = Convert.ToDouble(dt.Rows[0]["CODE_VAL1"].ToString());
                    }


                    //計算節金稅額
                    double amt = Convert.ToDouble(sg030DAO.FESTIVAL_AMT);
                    int amtR = 0;
                    int festivalTax = 0;
                    sg030DAO.FESTIVAL_TAX = Convert.ToString(festivalTax);
                    sg030DAO.FESTIVAL_AMT_R = Convert.ToString(amtR);
                    if (amt < incomeLimit)
                    {
                        sg030DAO.FESTIVAL_TAX = "0";
                    }
                    else
                    {
                        festivalTax = Convert.ToInt32(amt * taxRate);
                        amtR = Convert.ToInt32(amt) - festivalTax;
                        sg030DAO.FESTIVAL_TAX = Convert.ToString(festivalTax);
                        sg030DAO.FESTIVAL_AMT_R = Convert.ToString(amtR);
                    }

                    sg030DAO.APPROVE_FLAG = "N";
                    sg030DAO.CHG_STATUS = "N";


                    //新增 節金明細維護檔
                    sg030DAO.insertDataDtl_D(now);

                    Commit();

                    //更新節金維護檔-重新計算總金額及人數，及回復成未核可的狀態
                    BeginTransaction();
                    sg030DAO.updateStatus_H(now);
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



    //支付狀態一括更新(Dtl)
    public string updatePayType(List<Tuple<string, string, string, string, string>> keysList, string updatePayType, string target_gen_dt)
    {
        CFB2SG0300DAO sg030DAO = new CFB2SG0300DAO();
        DataTable dt = new DataTable();
        string rtnmessage = "";//存在檢查後的訊息
        try
        {

            //檢查完成後，逐筆進行刪除
            if (rtnmessage == "")
            {
                try
                {

                    DateTime now = DateTime.Parse(DateTime.Now.ToString());
                    //存放更新節金維護檔的員工區分
                    Dictionary<string, string> empCD_D = new Dictionary<string, string>();

                    foreach (var item in keysList)
                    {

                        BeginTransaction();
                        try
                        {
                            empCD_D.Add(item.Item4, item.Item4);
                        }
                        catch
                        {
                            //不處理
                        }

                        sg030DAO = new CFB2SG0300DAO();
                        sg030DAO.FESTIVAL_TYPE = item.Item1;
                        sg030DAO.FESTIVAL_DT = item.Item2;
                        sg030DAO.FESTIVAL_PAY_DT = item.Item3;
                        sg030DAO.EMP_CD = item.Item4;
                        sg030DAO.EMP_ID = item.Item5;

                        //異動狀態
                        dt = sg030DAO.getDetailData();
                        string payType_old = dt.Rows[0]["PAY_TYPE"].ToString();
                        string approveFlag = dt.Rows[0]["APPROVE_FLAG"].ToString();
                        string chgStatus = (string)dt.Rows[0]["CHG_STATUS"];
                        if (chgStatus == "N" && approveFlag == "N")
                        {
                            sg030DAO.CHG_STATUS = "N"; //新增
                        }
                        else
                        {
                            sg030DAO.CHG_STATUS = "U";  //修改
                        }


                        sg030DAO.APPROVE_FLAG = "N"; //未核可
                        sg030DAO.PAY_TYPE = updatePayType;
                        sg030DAO.PAY_TYPE_OLD = payType_old;
                        sg030DAO.UPDATED_BY = SessionHandle.Current.emp_id;
                        sg030DAO.FUNC_ID = "FB2SG030";

                        //更新節金明細維護檔
                        sg030DAO.updatePayType_D(now);
                        Commit();
                    }


                    //更新節金維護檔-重新計算總金額及人數，及回復成未核可的狀態
                    BeginTransaction();
                    sg030DAO.TARGET_GEN_DT = target_gen_dt;
                    Dictionary<string, string>.ValueCollection valueColl = empCD_D.Values;
                    foreach (string empCD in valueColl)
                    {
                        sg030DAO.EMP_CD = empCD;
                        sg030DAO.updateStatus_H(now);
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



    //刪除-將異動狀態更新為D(Dtl)
    public string updateStatus2DeleteDtl(List<Tuple<string, string, string, string, string>> keysList, string target_gen_dt)
    {
        CFB2SG0300DAO sg030DAO = new CFB2SG0300DAO();
        DataTable dt = new DataTable();
        string rtnmessage = "";//存在檢查後的訊息
        try
        {

            //檢查完成後，逐筆進行刪除
            if (rtnmessage == "")
            {
                try
                {
                    DateTime now = DateTime.Parse(DateTime.Now.ToString());
                    Dictionary<string, string> empCD_D = new Dictionary<string, string>();


                    BeginTransaction();
                    foreach (var item in keysList)
                    {
                        //要更新節金維護檔的員工區分
                        try
                        {
                            empCD_D.Add(item.Item4, item.Item4);
                        }
                        catch
                        {
                            //不處理
                        }


                        sg030DAO = new CFB2SG0300DAO();
                        sg030DAO.FESTIVAL_TYPE = item.Item1;
                        sg030DAO.FESTIVAL_DT = item.Item2;
                        sg030DAO.FESTIVAL_PAY_DT = item.Item3;
                        sg030DAO.EMP_CD = item.Item4;
                        sg030DAO.EMP_ID = item.Item5;
                        sg030DAO.UPDATED_BY = SessionHandle.Current.emp_id;
                        sg030DAO.FUNC_ID = "FB2SG030";

                        //異動狀態
                        sg030DAO.CHG_STATUS = "D"; //刪除
                        sg030DAO.APPROVE_FLAG = "N";
                        //更新 節金明細維護檔 的異動狀態為N
                        sg030DAO.updateStatus2DeleteDtl_D(now);
                    }
                    Commit();

                    //更新節金維護檔-重新計算總金額及人數，及回復成未核可的狀態
                    BeginTransaction();
                    sg030DAO.TARGET_GEN_DT = target_gen_dt;
                    Dictionary<string, string>.ValueCollection valueColl = empCD_D.Values;
                    foreach (string empCD in valueColl)
                    {
                        sg030DAO.EMP_CD = empCD;
                        sg030DAO.updateStatus_H(now);
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


    //本次維護資料下載
    public IWorkbook createExcelFromTemplate(string excelPath, CFB2SG0300DAO sg030DAO)
    {

        FileStream fs = null;
        IWorkbook workbook = null;

        //取得範本sheet
        ISheet sheet = null;

        try
        {
            fs = new FileStream(excelPath, FileMode.Open, FileAccess.ReadWrite);
            workbook = new XSSFWorkbook(fs); //xlsx的方法

            //取得範本sheet
            sheet = workbook.GetSheetAt(0);

            if (sheet != null)
            {

                //DataTable dt = sg010DAO.getCondLogData();
                DataTable dt = sg030DAO.getMaintainData();
                if (dt.Rows.Count > 0)
                {
                    IRow row;
                    ICell cell;
                    int x = 0;

                    ICellStyle stringLeftStyle = this.setCellStyle(workbook, "left", true);
                    ICellStyle stringRightStyle = this.setCellStyle(workbook, "right", true);
                    ICellStyle stringCenterStyle = this.setCellStyle(workbook, "center", true);

                    //數字格式,有千分位,
                    ICellStyle numbericStyle = workbook.CreateCellStyle();
                    numbericStyle = stringRightStyle;
                    numbericStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("#,##0");

                    //數字格式小數2位,
                    //ICellStyle twoDotStyle = workbook.CreateCellStyle();
                    //twoDotStyle = stringRightStyle;
                    //twoDotStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("###0.00");

                    //CellType celltype = this.setCellType("left", true);
                    string dtFormat = "";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        x = i + 1;//從第幾列開始insert 資料
                        //將資料寫入範本
                        row = sheet.CreateRow(x);

                        //節金類別
                        cell = row.CreateCell(1);
                        cell.CellStyle = stringLeftStyle;  //先
                        cell.SetCellValue(dt.Rows[i]["FESTIVAL_TYPE_DESC"].ToString()); //後

                        //節日日期
                        cell = row.CreateCell(2);
                        cell.CellStyle = stringLeftStyle;
                        dtFormat = dt.Rows[i]["FESTIVAL_DT"].ToString() != "" ? Convert.ToDateTime(dt.Rows[i]["FESTIVAL_DT"].ToString()).ToString("yyyy/MM/dd") : "";
                        cell.SetCellValue(dtFormat);

                        //節金發放日期
                        cell = row.CreateCell(3);
                        cell.CellStyle = stringLeftStyle;
                        dtFormat = dt.Rows[i]["FESTIVAL_PAY_DT"].ToString() != "" ? Convert.ToDateTime(dt.Rows[i]["FESTIVAL_PAY_DT"].ToString()).ToString("yyyy/MM/dd") : "";
                        cell.SetCellValue(dtFormat);
                        //節金說明
                        cell = row.CreateCell(4);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["FESTIVAL_DESC"].ToString());
                        //異動狀態
                        cell = row.CreateCell(5);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["CHG_STATUS_DESC"].ToString());
                        //6.員工工號
                        cell = row.CreateCell(6);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["EMP_ID"].ToString());
                        //員工姓名
                        cell = row.CreateCell(7);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["EMP_NAME"].ToString().Trim());
                        //部門代號
                        cell = row.CreateCell(8);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["DEPT_NO"].ToString());
                        //工廠區分
                        cell = row.CreateCell(9);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["PLANT_CD_DESC"].ToString());
                        //外籍會社
                        cell = row.CreateCell(10);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["JPN_CD_DESC"].ToString());

                        //11資格代號
                        cell = row.CreateCell(11);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["LEVEL_CD"].ToString());
                        //級數代號
                        cell = row.CreateCell(12);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["GRADE_CD"].ToString());
                        //職務代號
                        cell = row.CreateCell(13);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["PJOB_CD"].ToString());
                        //入社日期
                        cell = row.CreateCell(14);
                        cell.CellStyle = stringLeftStyle;
                        dtFormat = dt.Rows[i]["JOIN_DT"].ToString() != "" ? Convert.ToDateTime(dt.Rows[i]["JOIN_DT"].ToString()).ToString("yyyy/MM/dd") : "";
                        cell.SetCellValue(dtFormat);
                        //在職年資(年)
                        cell = row.CreateCell(15);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["WORK_YEARS"].ToString());


                        //16.在職年資(天)
                        cell = row.CreateCell(16);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["WORK_DAYS"].ToString());
                        //員工區分
                        cell = row.CreateCell(17);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["EMP_CD_DESC"].ToString());
                        //職種
                        cell = row.CreateCell(18);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["WS_CD"].ToString());
                        //在職區分
                        cell = row.CreateCell(19);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["EMP_CHG_CD_DESC"].ToString());
                        //節金金額
                        cell = row.CreateCell(20);
                        cell.CellStyle = numbericStyle;
                        cell.SetCellValue(Convert.ToDouble(dt.Rows[i]["FESTIVAL_AMT"].ToString()));

                        //21節金金額(前次)
                        cell = row.CreateCell(21);
                        cell.CellStyle = numbericStyle;
                        cell.SetCellValue(Convert.ToDouble(dt.Rows[i]["FESTIVAL_AMT_OLD"].ToString()));
                        //節金稅額
                        cell = row.CreateCell(22);
                        cell.CellStyle = numbericStyle;
                        cell.SetCellValue(Convert.ToDouble(dt.Rows[i]["FESTIVAL_TAX"].ToString()));
                        //節金實額
                        cell = row.CreateCell(23);
                        cell.CellStyle = numbericStyle;
                        cell.SetCellValue(Convert.ToDouble(dt.Rows[i]["FESTIVAL_AMT_R"].ToString()));
                        //支付狀態
                        cell = row.CreateCell(24);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["PAY_TYPE_DESC"].ToString());
                        //支付狀態(前次)
                        cell = row.CreateCell(25);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["PAY_TYPE_OLD_DESC"].ToString());

                        //26職能俸
                        cell = row.CreateCell(26);
                        cell.CellStyle = numbericStyle;
                        cell.SetCellValue(Convert.ToDouble(dt.Rows[i]["ABILITY_PAY"].ToString()));
                        //資格俸
                        cell = row.CreateCell(27);
                        cell.CellStyle = numbericStyle;
                        cell.SetCellValue(Convert.ToDouble(dt.Rows[i]["LEVEL_PAY"].ToString()));
                        //職務俸
                        cell = row.CreateCell(28);
                        cell.CellStyle = numbericStyle;
                        cell.SetCellValue(Convert.ToDouble(dt.Rows[i]["PJOB_PAY"].ToString()));
                        //專業俸
                        cell = row.CreateCell(29);
                        cell.CellStyle = numbericStyle;
                        cell.SetCellValue(Convert.ToDouble(dt.Rows[i]["PROFESSION_PAY"].ToString()));
                        //伙食津貼       
                        cell = row.CreateCell(30);
                        cell.CellStyle = numbericStyle;
                        cell.SetCellValue(Convert.ToDouble(dt.Rows[i]["FOOD_SUBSIDY"].ToString()));


                        ////金額的格式
                        //cell = row.CreateCell(4);
                        //cell.CellStyle = numbericStyle;
                        //cell.SetCellValue(Convert.ToDouble(dt.Rows[i]["FESTIVAL_AMT"].ToString()));

                        ////轉型成數字格式，存到EXCEL即為數字
                        //cell = row.CreateCell(5);
                        //cell.CellStyle = stringRightStyle;
                        //cell.SetCellValue(Convert.ToDouble(dt.Rows[i]["WORK_YEARS_SDT"].ToString()));

                        //cell = row.CreateCell(6);
                        //cell.CellStyle = stringRightStyle;
                        //cell.SetCellValue(Convert.ToDouble(dt.Rows[i]["WORK_YEARS_EDT"].ToString()));

                        //cell = row.CreateCell(7);
                        //cell.CellStyle = stringLeftStyle;
                        //cell.SetCellValue(dt.Rows[i]["PRID_CD"].ToString());

                    }
                    //製表日期
                    ICellStyle stringLeftStyleDate = this.setCellStyle(workbook, "left", false);
                    row = sheet.GetRow(0);
                    cell = row.CreateCell(31);
                    cell.CellStyle = stringLeftStyleDate;
                    cell.SetCellValue("製表日期:" + DateTime.Now.ToString("yyyy/MM/dd"));  
                    for (int i = 0; i <= 31; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                    string yearNow = DateTime.Now.ToString("yyyy");

                    //ExcelHandle.exportExcel(workbook, yearNow + "節金維護資料.xlsx");

                }
                return workbook;
            }
            return null;
        }
        catch (Exception)
        {

            throw;
        }
        finally {
            if (workbook != null)
            {
                workbook.Clear();
            }
            if (fs != null)
            {
                fs.Close();
            }
            sheet = null;
            workbook = null;
        }
    }




    /// <summary>
    /// 設定資料的格式
    /// </summary>
    /// <param name="workbook"></param>
    /// <param name="align">文字位置</param>
    /// <param name="isBorder">是否要有邊框</param>
    /// <returns></returns>
    private ICellStyle setCellStyle(IWorkbook workbook, string align, bool isBorder)
    {
        return setCellStyle(workbook, align, isBorder, 0);
    }

    /// <summary>
    /// 設定資料的格式
    /// </summary>
    /// <param name="workbook"></param>
    /// <param name="align">文字位置</param>
    /// <param name="isBorder">是否要有邊框</param>
    /// <param name="color">背景顏色設定(10:紅,13:黃,14:pink.... )</param>
    /// <returns></returns>
    private ICellStyle setCellStyle(IWorkbook workbook, string align, bool isBorder, int colorCD)
    {
        ICellStyle style = workbook.CreateCellStyle();

        //資料的字型
        IFont cellFont = workbook.CreateFont();
        cellFont.FontName = "新細明體";
        cellFont.FontHeightInPoints = 12;  //字型大小
        cellFont.Color = HSSFColor.Black.Index;   //字型顏色
        cellFont.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Normal;   //bold:粗體字
        style.SetFont(cellFont);

        //是否要有邊框
        if (isBorder)
        {
            //style.BottomBorderColor = HSSFColor.White.Index;
            style.BorderBottom = BorderStyle.Thin;
            style.BorderTop = BorderStyle.Thin;
            style.BorderLeft = BorderStyle.Thin;
            style.BorderRight = BorderStyle.Thin;
        }

        //文字位置 (預設靠左)
        if (align.ToLower() == "center")
        {
            style.Alignment = HorizontalAlignment.Center;
        }
        else if (align.ToLower() == "right")
        {
            style.Alignment = HorizontalAlignment.Right;
        }
        else
        {
            style.Alignment = HorizontalAlignment.Left;
        }

        //背景顏色
        if (colorCD > 0)
        {
            style.FillForegroundColor = (short)colorCD;
            style.FillPattern = FillPattern.SolidForeground;
            //style.FillBackgroundColor = HSSFColor.Yellow.Index;
        }



        return style;
    }

}