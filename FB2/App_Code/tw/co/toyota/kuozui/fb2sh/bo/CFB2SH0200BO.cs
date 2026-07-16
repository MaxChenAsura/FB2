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
/// CFB2SH0200BO 的摘要描述
/// </summary>
public class CFB2SH0200BO : BaseService
{

    ICellStyle style_class;
    public CFB2SH0200BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public string getSALARY_ADJ()
    {
        string rtnmessage = "N";
        CFB2SH0200DAO sh020DAO = new CFB2SH0200DAO();
        DataTable dt = sh020DAO.getSALARY_ADJ();
        if ((int)dt.Rows[0]["resultCount"] > 0)
        {
            rtnmessage += "Y";
        }
        dt.Clear();
        return rtnmessage;
    }


    //新增
    public string insertData(CFB2SH0200DAO sh020DAO)
    {
        string rtnmessage = "";
        try
        {

            //若需要則要進行邏輯檢查(與DB相關的)
            //00.檢查PK值有無重覆
            DataTable dupdata = sh020DAO.getPKData();
            if ((int)dupdata.Rows[0]["resultCount"] > 0)
            {
                rtnmessage += "年度+年獎回數 重覆";
            }


            //檢查OK更新
            if (rtnmessage == "")
            {
                try
                {
                    BeginTransaction();

                    sh020DAO.insertData();

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
    public string updateData(CFB2SH0200DAO dao)
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

    //刪除
    public string deleteData(List<Tuple<string, string>> keysList)
    {
        CFB2SH0200DAO sh020DAO = new CFB2SH0200DAO();
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
                        //刪除 年獎維護檔
                        sh020DAO.deleteDataH(item.Item1, item.Item2);
                        //刪除 年獎明細維護檔
                        sh020DAO.deleteDataD(item.Item1, item.Item2, "TB_S_M_AWARD_DM");
                        //刪除 年獎明細原始檔
                        sh020DAO.deleteDataD(item.Item1, item.Item2, "TB_S_S_AWARD_D");
                        //刪除 年獎明細主檔
                        sh020DAO.deleteDataD(item.Item1, item.Item2, "TB_S_M_AWARD_D");

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


    //年獎對象生成
    public string execSP_S_AWARD_DATA(CFB2SH0200DAO sh020DAO)
    {
        string rtnmessage = "";//存在檢查後的訊息

        try
        {

            //檢查完成後，逐筆進行刪除
            if (rtnmessage == "")
            {
                sh020DAO.execSP_S_AWARD_DATA();
                rtnmessage += utilities.getSPLOG("SP_S_AWARD_DATA");
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

    //提出核可
    public string updateRelease(CFB2SH0200DAO sh020DAO)
    {

        string rtnmessage = "";//存在檢查後的訊息
        try
        {
            if (rtnmessage == "")
            {
                try
                {
                    BeginTransaction();

                    sh020DAO.updateRelease();

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
    public string updateAnnounce(CFB2SH0200DAO sh020DAO)
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
                    sh020DAO.updateAnnounce(now);
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
    public string updatePayType(List<Tuple<string, string, string>> keysList, string pay_type)
    {
        CFB2SH0200DAO sh020DAO = new CFB2SH0200DAO();
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


                    BeginTransaction();
                    foreach (var item in keysList)
                    {
                        sh020DAO = new CFB2SH0200DAO();
                        sh020DAO.AWARD_YEAR = item.Item1;
                        sh020DAO.AWARD_ROUND = item.Item2;
                        sh020DAO.EMP_ID = item.Item3;
                        sh020DAO.PAY_TYPE = pay_type;
                        sh020DAO.APPROVE_FLAG = "N"; //未核可
                        sh020DAO.UPDATED_BY = SessionHandle.Current.emp_id;
                        sh020DAO.FUNC_ID = "FB2SH020";

                        //更新年獎明細維護檔
                        sh020DAO.updatePayType_D(now);

                    }
                    Commit();
                    BeginTransaction();
                    //更新年獎維護檔
                    //sh020DAO.RELEASE_DT= null; 改直接用DBNull
                    sh020DAO.RELEASE_BY = "";
                    //sh020DAO.APPROVE_DT= null;
                    sh020DAO.APPROVE_BY = "";
                    sh020DAO.APPROVE_STATUS = "N";
                    sh020DAO.FREEZE_FLAG = "N";
                    sh020DAO.updatePayType_H(now);
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
    public string updateStatus2DeleteDtl(List<Tuple<string, string, string>> keysList)
    {
        CFB2SH0200DAO sh020DAO = new CFB2SH0200DAO();
        string rtnmessage = "";//存在檢查後的訊息
        try
        {

            //檢查完成後，逐筆進行刪除
            if (rtnmessage == "")
            {
                try
                {
                    DateTime now = DateTime.Parse(DateTime.Now.ToString());
                    BeginTransaction();
                    foreach (var item in keysList)
                    {
                        sh020DAO = new CFB2SH0200DAO();
                        sh020DAO.AWARD_YEAR = item.Item1;
                        sh020DAO.AWARD_ROUND = item.Item2;
                        sh020DAO.EMP_ID = item.Item3;

                        //異動狀態
                        sh020DAO.CHG_STATUS = "D"; //刪除
                        sh020DAO.APPROVE_FLAG = "N"; //未核可
                        sh020DAO.UPDATED_BY = SessionHandle.Current.emp_id;
                        sh020DAO.FUNC_ID = "FB2SH020";

                        //更新 節金明細維護檔 的異動狀態為N
                        sh020DAO.updateStatus2DeleteDtl_D(now);

                    }
                    Commit();
                    //計算總金額及總人數
                    BeginTransaction();
                    //更新節金維護檔 
                    sh020DAO.RELEASE_BY = "";
                    sh020DAO.APPROVE_BY = "";
                    sh020DAO.APPROVE_STATUS = "N";
                    sh020DAO.FREEZE_FLAG = "N";
                    sh020DAO.UPDATED_BY = SessionHandle.Current.emp_id;
                    sh020DAO.FUNC_ID = "FB2SH020";
                    sh020DAO.updateTotal2Dtl(now);

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


    //本次維護資料/原始資料下載
    public IWorkbook createExcelFromTemplate(string excelPath, CFB2SH0200DAO sh020DAO, DataTable dt)
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
                    //dtFormat = dt.Rows[i]["FESTIVAL_DT"].ToString() != "" ? Convert.ToDateTime(dt.Rows[i]["FESTIVAL_DT"].ToString()).ToString("yyyy/MM/dd") : "";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        x = i + 3;//從第2列開始insert 資料
                        //將資料寫入範本
                        row = sheet.CreateRow(x);



                        //工號
                        cell = row.CreateCell(1);
                        cell.CellStyle = stringLeftStyle;  //先
                        cell.SetCellValue(dt.Rows[i]["EMP_ID"].ToString()); //後
                        //姓名
                        cell = row.CreateCell(2);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["EMP_NAME"].ToString().Trim());
                        //在職區分
                        cell = row.CreateCell(3);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["EMP_CHG_CD_DESC"].ToString());
                        //職種
                        cell = row.CreateCell(4);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["WS_CD"].ToString());
                        //外籍會社
                        cell = row.CreateCell(5);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["JPN_CD"].ToString());

                        //6.部門代號
                        cell = row.CreateCell(6);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["DEPT_NO"].ToString());
                        //資格代號
                        cell = row.CreateCell(7);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["LEVEL_CD"].ToString());
                        //職務代號
                        cell = row.CreateCell(8);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["PJOB_CD"].ToString());
                        //入社日期
                        cell = row.CreateCell(9);
                        cell.CellStyle = stringLeftStyle;
                        dtFormat = dt.Rows[i]["JOIN_DT"].ToString() != "" ? Convert.ToDateTime(dt.Rows[i]["JOIN_DT"].ToString()).ToString("yyyy/MM/dd") : "";
                        cell.SetCellValue(dtFormat);
                        //離社日期
                        cell = row.CreateCell(10);
                        cell.CellStyle = stringLeftStyle;
                        dtFormat = dt.Rows[i]["LEAVE_DT"].ToString() != "" ? Convert.ToDateTime(dt.Rows[i]["LEAVE_DT"].ToString()).ToString("yyyy/MM/dd") : "";
                        cell.SetCellValue(dtFormat);

                        //11留職日(留職停工日)
                        cell = row.CreateCell(11);
                        cell.CellStyle = stringLeftStyle;
                        dtFormat = dt.Rows[i]["STAY_DT"].ToString() != "" ? Convert.ToDateTime(dt.Rows[i]["STAY_DT"].ToString()).ToString("yyyy/MM/dd") : "";
                        cell.SetCellValue(dtFormat);
                        //留廠日(轉期間工日)
                        cell = row.CreateCell(12);
                        cell.CellStyle = stringLeftStyle;
                        dtFormat = dt.Rows[i]["BE_CONTRACT_DT"].ToString() != "" ? Convert.ToDateTime(dt.Rows[i]["BE_CONTRACT_DT"].ToString()).ToString("yyyy/MM/dd") : "";
                        cell.SetCellValue(dtFormat);
                        //轉正社員日
                        cell = row.CreateCell(13);
                        cell.CellStyle = stringLeftStyle;
                        dtFormat = dt.Rows[i]["BE_EMP_DT"].ToString() != "" ? Convert.ToDateTime(dt.Rows[i]["BE_EMP_DT"].ToString()).ToString("yyyy/MM/dd") : "";
                        cell.SetCellValue(dtFormat);
                        //在職天數(年獎期間)
                        cell = row.CreateCell(14);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["WORK_DAYS"].ToString());
                        //員工區分
                        cell = row.CreateCell(15);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["EMP_CD_DESC"].ToString());


                        //16.身份標示
                        cell = row.CreateCell(16);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["ID_DESC"].ToString());
                        //職能俸
                        cell = row.CreateCell(17);
                        cell.CellStyle = numbericStyle;
                        cell.SetCellValue(Convert.ToDouble(dt.Rows[i]["ABILITY_PAY"].ToString()));
                        //資格俸
                        cell = row.CreateCell(18);
                        cell.CellStyle = numbericStyle;
                        cell.SetCellValue(Convert.ToDouble(dt.Rows[i]["LEVEL_PAY"].ToString()));
                        //職務俸
                        cell = row.CreateCell(19);
                        cell.CellStyle = numbericStyle;
                        cell.SetCellValue(Convert.ToDouble(dt.Rows[i]["PJOB_PAY"].ToString()));
                        //專業俸
                        cell = row.CreateCell(20);
                        cell.CellStyle = numbericStyle;
                        cell.SetCellValue(Convert.ToDouble(dt.Rows[i]["PROFESSION_PAY"].ToString()));

                        //21伙食津貼
                        cell = row.CreateCell(21);
                        cell.CellStyle = numbericStyle;
                        cell.SetCellValue(Convert.ToDouble(dt.Rows[i]["FOOD_SUBSIDY"].ToString()));
                        //原始考績(業績)
                        cell = row.CreateCell(22);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["SCORE_2H"].ToString());
                        //考績反映(年獎格差)
                        cell = row.CreateCell(23);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["AWARD_BASE"].ToString());
                        //事假時數
                        cell = row.CreateCell(24);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["LEAVE_A_HOUR"].ToString());
                        //有薪病假時數
                        cell = row.CreateCell(25);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["LEAVE_B_HOUR"].ToString());

                        //26無薪病假時數
                        cell = row.CreateCell(26);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["LEAVE_C_HOUR"].ToString());
                        //曠工時數
                        cell = row.CreateCell(27);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["LEAVE_Q_HOUR"].ToString());
                        //遲到/早退 次數
                        cell = row.CreateCell(28);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["LEAVE_OP_HOUR"].ToString());
                        //嘉獎
                        cell = row.CreateCell(29);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["THIRD_CNT_P"].ToString());
                        //小功       
                        cell = row.CreateCell(30);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["SECOND_CNT_P"].ToString());

                        //31大功
                        cell = row.CreateCell(31);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["FIRST_CNT_P"].ToString());
                        //申誡
                        cell = row.CreateCell(32);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["THIRD_CNT_M"].ToString());
                        //小過
                        cell = row.CreateCell(33);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["SECOND_CNT_M"].ToString());
                        //大過
                        cell = row.CreateCell(34);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["FIRST_CNT_M"].ToString());
                        //勤怠扣除天數       
                        cell = row.CreateCell(35);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["ATTEND_DAYS"].ToString());

                        //36獎懲加減天數
                        cell = row.CreateCell(36);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["REWARD_DAYS"].ToString());
                        //紀律扣除天數
                        cell = row.CreateCell(37);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["DISCIPLINE_DAYS"].ToString());
                        //實際在職天數
                        cell = row.CreateCell(38);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["AWARD_WORK_DAYS"].ToString());
                        //反映項目
                        cell = row.CreateCell(39);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["items"].ToString());
                        //昇格者Y / N       
                        cell = row.CreateCell(40);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["LEVELUP_FLAG"].ToString());



                        //41年獎合計
                        cell = row.CreateCell(41);
                        cell.CellStyle = numbericStyle;
                        cell.SetCellValue(Convert.ToDouble(dt.Rows[i]["amtTotal"].ToString()));
                        //年獎第一回
                        cell = row.CreateCell(42);
                        cell.CellStyle = numbericStyle;
                        cell.SetCellValue(Convert.ToDouble(dt.Rows[i]["amt1"].ToString()));
                        //年獎第二回
                        cell = row.CreateCell(43);
                        cell.CellStyle = numbericStyle;
                        cell.SetCellValue(Convert.ToDouble(dt.Rows[i]["amt2"].ToString()));
                        //年獎第三回
                        cell = row.CreateCell(44);
                        cell.CellStyle = numbericStyle;
                        cell.SetCellValue(Convert.ToDouble(dt.Rows[i]["amt3"].ToString()));
                        //年獎稅額
                        cell = row.CreateCell(45);
                        cell.CellStyle = numbericStyle;
                        cell.SetCellValue(Convert.ToDouble(dt.Rows[i]["AWARD_TAX"].ToString()));
                        //年獎實額
                        cell = row.CreateCell(46);
                        cell.CellStyle = numbericStyle;
                        cell.SetCellValue(Convert.ToDouble(dt.Rows[i]["AWARD_AMT_R"].ToString()));
                        //支付狀態
                        cell = row.CreateCell(47);
                        cell.CellStyle = stringLeftStyle;
                        //cell.SetCellValue(dt.Rows[i]["PAY_TYPE_DESC"].ToString());
                        cell.SetCellValue(dt.Rows[i]["PAY_TYPE"].ToString());
                        //異動狀態
                        cell = row.CreateCell(48);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["CHG_STATUS_DESC"].ToString());


                        //if (i % 50 == 0)
                        //{

                        //    ((SXSSFSheet)sheet).flushRows(50);  // retain 100 last rows and flush all others
                        //}


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
                    cell = row.CreateCell(49);
                    cell.CellStyle = stringLeftStyleDate;
                    cell.SetCellValue("製表日期:" + DateTime.Now.ToString("yyyy/MM/dd"));

                    /*
                    for (int i = 0; i <= 48; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                    */
                    //if (tableName == "TB_S_M_AWARD_DM")
                    //{
                    //    ExcelHandle.exportExcel(workbook, sh020DAO.AWARD_YEAR + "第" + sh020DAO.AWARD_ROUND + "回年獎維護資料.xlsx");
                    //}
                    //else if (tableName == "TB_S_S_AWARD_D")
                    //{
                    //    ExcelHandle.exportExcel(workbook, sh020DAO.AWARD_YEAR + "第" + sh020DAO.AWARD_ROUND + "回年獎原始資料.xlsx");
                    //}

                }
                return workbook;
            }
            return null;
        }
        catch (Exception ex)
        {
            throw;
        }
        finally
        {
            workbook.Clear();
            fs.Close();
            sheet = null;
            workbook = null;
        }
    }



    //昇格資料下載(用來下載有block的用法)
    public IWorkbook testcreateExcelFromTemplateDefault(string excelPath, CFB2SH0200DAO sh020DAO, DataTable dt)
    {

        //DataTable dt = sh020DAO.getLevelUpData(); 
        //if (dt.Rows.Count == 0)
        //{
        //    return "無昇格名單";
        //}

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

                ICellStyle stringRedLeftStyle = this.setCellStyle(workbook, "left", true, 10);

                IRow row;
                ICell cell;
                //若只有title時 ,儲存錯誤訊息
                if (dt.Rows.Count == 0)
                {
                    row = sheet.CreateRow(2);
                    cell = row.CreateCell(1);
                    cell.CellStyle = stringRedLeftStyle;  //先
                    cell.SetCellValue("無昇格名單"); //後

                }

                if (dt.Rows.Count > 0)
                {

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
                    //dtFormat = dt.Rows[i]["FESTIVAL_DT"].ToString() != "" ? Convert.ToDateTime(dt.Rows[i]["FESTIVAL_DT"].ToString()).ToString("yyyy/MM/dd") : "";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        x = i + 2;//從第2列開始insert 資料
                        //將資料寫入範本
                        row = sheet.CreateRow(x);

                        //工號
                        cell = row.CreateCell(1);
                        cell.CellStyle = stringLeftStyle;  //先
                        cell.SetCellValue(dt.Rows[i]["EMP_ID"].ToString()); //後
                        //姓名
                        cell = row.CreateCell(2);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["EMP_NAME"].ToString().Trim());
                        //資格代號
                        cell = row.CreateCell(3);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["LEVEL_CD"].ToString());


                        //職能俸(昇格後)
                        cell = row.CreateCell(4);
                        cell.CellStyle = numbericStyle;
                        cell.SetCellValue(Convert.ToDouble(dt.Rows[i]["ABILITY_PAY"].ToString()));
                        //資格俸(昇格後)
                        cell = row.CreateCell(5);
                        cell.CellStyle = numbericStyle;
                        cell.SetCellValue(Convert.ToDouble(dt.Rows[i]["LEVEL_PAY"].ToString()));
                        //職務俸(昇格後)
                        cell = row.CreateCell(6);
                        cell.CellStyle = numbericStyle;
                        cell.SetCellValue(Convert.ToDouble(dt.Rows[i]["PJOB_PAY"].ToString()));
                        //專業俸(昇格後)
                        cell = row.CreateCell(7);
                        cell.CellStyle = numbericStyle;
                        cell.SetCellValue(Convert.ToDouble(dt.Rows[i]["PROFESSION_PAY"].ToString()));
                        //伙食津貼(昇格後)
                        cell = row.CreateCell(8);
                        cell.CellStyle = numbericStyle;
                        cell.SetCellValue(Convert.ToDouble(dt.Rows[i]["FOOD_SUBSIDY"].ToString()));
                        //年獎第一回(昇格後)
                        cell = row.CreateCell(9);
                        cell.CellStyle = numbericStyle;
                        cell.SetCellValue(Convert.ToDouble(dt.Rows[i]["amt1"].ToString()));
                        //年獎第二回(昇格後)
                        cell = row.CreateCell(10);
                        cell.CellStyle = numbericStyle;
                        cell.SetCellValue(Convert.ToDouble(dt.Rows[i]["amt2"].ToString()));
                        //年獎第三回(昇格後)
                        cell = row.CreateCell(11);
                        cell.CellStyle = numbericStyle;
                        cell.SetCellValue(Convert.ToDouble(dt.Rows[i]["amt3"].ToString()));

                        //職能俸(昇格前)
                        cell = row.CreateCell(12);
                        cell.CellStyle = numbericStyle;
                        cell.SetCellValue(Convert.ToDouble(dt.Rows[i]["ABILITY_PAY_BEFORE"].ToString()));
                        //資格俸(昇格前)
                        cell = row.CreateCell(13);
                        cell.CellStyle = numbericStyle;
                        cell.SetCellValue(Convert.ToDouble(dt.Rows[i]["LEVEL_PAY_BEFORE"].ToString()));
                        //職務俸(昇格前)
                        cell = row.CreateCell(14);
                        cell.CellStyle = numbericStyle;
                        cell.SetCellValue(Convert.ToDouble(dt.Rows[i]["PJOB_PAY_BEFORE"].ToString()));
                        //專業俸(昇格前)
                        cell = row.CreateCell(15);
                        cell.CellStyle = numbericStyle;
                        cell.SetCellValue(Convert.ToDouble(dt.Rows[i]["PROFESSION_PAY_BEFORE"].ToString()));
                        //伙食津貼(昇格前)
                        cell = row.CreateCell(16);
                        cell.CellStyle = numbericStyle;
                        cell.SetCellValue(Convert.ToDouble(dt.Rows[i]["FOOD_SUBSIDY_BEFORE"].ToString()));
                        //年獎第一回(昇格前)
                        cell = row.CreateCell(17);
                        cell.CellStyle = numbericStyle;
                        cell.SetCellValue(Convert.ToDouble(dt.Rows[i]["amt1_before"].ToString()));
                        //年獎第二回(昇格前)
                        cell = row.CreateCell(18);
                        cell.CellStyle = numbericStyle;
                        cell.SetCellValue(Convert.ToDouble(dt.Rows[i]["amt2_before"].ToString()));
                        //年獎第三回(昇格前)
                        cell = row.CreateCell(19);
                        cell.CellStyle = numbericStyle;
                        cell.SetCellValue(Convert.ToDouble(dt.Rows[i]["amt3_before"].ToString()));

                    }


                    //製表日期
                    ICellStyle stringLeftStyleDate = this.setCellStyle(workbook, "left", false);
                    row = sheet.GetRow(0);
                    cell = row.CreateCell(20);
                    cell.CellStyle = stringLeftStyleDate;
                    cell.SetCellValue("製表日期:" + DateTime.Now.ToString("yyyy/MM/dd"));

                    //for (int i = 0; i <= 20; i++)
                    //{
                    //    sheet.AutoSizeColumn(i);
                    //}

                }

                return workbook;
            }

            return null;
        }
        catch (Exception ex)
        {
            throw;
        }
        finally
        {
            workbook.Clear();
            fs.Close();
            sheet = null;
            workbook = null;
        }
    }

    //檔案上傳(匯入)
    public IWorkbook uploadExcel(Stream fs, string type, string award_year, string award_round, string award_days)
    {
        try
        {
            bool valid = true;
            DateTime now = DateTime.Parse(DateTime.Now.ToString());

            IWorkbook workbook;
            //依附檔名判斷要用哪種方式讀取
            if (type == ".xls")
            {
                workbook = new HSSFWorkbook(fs);
            }
            else if (type == ".xlsx")
            {
                workbook = new XSSFWorkbook(fs);
            }
            else {
                return null;
            }
            

            //取得參數檔-獎金類所得稅率
            CFB2SG0200DAO sg020DAO = new CFB2SG0200DAO();
            DataTable dt = sg020DAO.getTaxRate();
            double taxRate = dt.Rows[0]["taxRate"].ToString() != "" ? Convert.ToDouble(dt.Rows[0]["taxRate"].ToString()) : 0;

            //取得參數檔-所得稅代扣金額下限
            dt.Clear();
            dt = sg020DAO.getLimitLow();
            int limitLow = dt.Rows[0]["limitLow"].ToString() != "" ? Convert.ToInt32(dt.Rows[0]["limitLow"].ToString()) : 0;

            //年獎-勤怠事假
            dt.Clear();
            dt = utilities.getParameter("SH", "Y_LEAVE_UC");
            double y_leave_uc = dt.Rows[0]["CODE_VAL1"].ToString() != "" ? Convert.ToDouble(dt.Rows[0]["CODE_VAL1"].ToString()) : 0;

            //年獎-勤怠有薪病假
            dt.Clear();
            dt = utilities.getParameter("SH", "Y_LEAVE_B");
            double y_leave_b = dt.Rows[0]["CODE_VAL1"].ToString() != "" ? Convert.ToDouble(dt.Rows[0]["CODE_VAL1"].ToString()) : 0;

            //年獎-勤怠有薪病假(超過30天)
            dt.Clear();
            dt = utilities.getParameter("SH", "Y_LEAVE_B_OVER30");
            double y_leave_b_over30 = dt.Rows[0]["CODE_VAL1"].ToString() != "" ? Convert.ToDouble(dt.Rows[0]["CODE_VAL1"].ToString()) : 0;


            //年獎-曠工
            dt.Clear();
            dt = utilities.getParameter("SH", "Y_LEAVE_Q");
            double y_leave_q = dt.Rows[0]["CODE_VAL1"].ToString() != "" ? Convert.ToDouble(dt.Rows[0]["CODE_VAL1"].ToString()) : 0;

            //年獎-遲/早
            dt.Clear();
            dt = utilities.getParameter("SH", "Y_LEAVE_OP");
            double y_leave_op = dt.Rows[0]["CODE_VAL1"].ToString() != "" ? Convert.ToDouble(dt.Rows[0]["CODE_VAL1"].ToString()) : 0;

            //年獎-大功
            dt.Clear();
            dt = utilities.getParameter("SH", "Y_FIRST_CNT_P");
            double y_first_cnt_p = dt.Rows[0]["CODE_VAL1"].ToString() != "" ? Convert.ToDouble(dt.Rows[0]["CODE_VAL1"].ToString()) : 0;

            //年獎-小功
            dt.Clear();
            dt = utilities.getParameter("SH", "Y_SECOND_CNT_P");
            double y_second_cnt_p = dt.Rows[0]["CODE_VAL1"].ToString() != "" ? Convert.ToDouble(dt.Rows[0]["CODE_VAL1"].ToString()) : 0;

            //年獎-嘉獎
            dt.Clear();
            dt = utilities.getParameter("SH", "Y_THIRD_CNT_P");
            double y_third_cnt_p = dt.Rows[0]["CODE_VAL1"].ToString() != "" ? Convert.ToDouble(dt.Rows[0]["CODE_VAL1"].ToString()) : 0;

            //年獎-大過
            dt.Clear();
            dt = utilities.getParameter("SH", "Y_FIRST_CNT_M");
            double y_first_cnt_m = dt.Rows[0]["CODE_VAL1"].ToString() != "" ? Convert.ToDouble(dt.Rows[0]["CODE_VAL1"].ToString()) : 0;

            //年獎-小過
            dt.Clear();
            dt = utilities.getParameter("SH", "Y_SECOND_CNT_M");
            double y_second_cnt_m = dt.Rows[0]["CODE_VAL1"].ToString() != "" ? Convert.ToDouble(dt.Rows[0]["CODE_VAL1"].ToString()) : 0;

            //年獎-申誡
            dt.Clear();
            dt = utilities.getParameter("SH", "Y_THIRD_CNT_M");
            double y_third_cnt_m = dt.Rows[0]["CODE_VAL1"].ToString() != "" ? Convert.ToDouble(dt.Rows[0]["CODE_VAL1"].ToString()) : 0;

            double wk_leave_B0 = 0;            //有薪病假時數
            double wk_leave_B0_over30 = 0;     //有薪病假時數_超過30天


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
                    //新增/修改的筆數
                    int addNum = 0;
                    int updateNum = 0;

                    //2.取得excel的資料
                    string cell_empId = "";
                    string cell_work_days = "";
                    string cell_id_desc = "";
                    string cell_level_pay = "";
                    string cell_ability_pay = "";
                    string cell_pjob_pay = "";
                    string cell_profession_pay = "";
                    string cell_food_subsidy = "";
                    string cell_score_2h = "";
                    string cell_award_base = "";
                    string cell_leave_a_hour = "";
                    string cell_leave_b_hour = "";
                    string cell_leave_c_hour = "";
                    string cell_leave_q_hour = "";
                    string cell_leave_op_hour = "";
                    string cell_third_cnt_p = "";
                    string cell_second_cnt_p = "";
                    string cell_first_cnt_p = "";
                    string cell_third_cnt_m = "";
                    string cell_second_cnt_m = "";
                    string cell_first_cnt_m = "";
                    string cell_pay_type = "";

                    //計算天數用資料-獎懲
                    double award_work_days = 0;
                    double wk_third_cnt_p = 0;
                    double wk_second_cnt_p = 0;
                    double wk_first_cnt_p = 0;
                    double wk_third_cnt_m = 0;
                    double wk_second_cnt_m = 0;
                    double wk_first_cnt_m = 0;

                    double third_cnt_p = 0;
                    double second_cnt_p = 0;
                    double first_cnt_p = 0;
                    double third_cnt_m = 0;
                    double second_cnt_m = 0;
                    double first_cnt_m = 0;
                    double reward_days = 0;

                    //計算天數用資料-紀律
                    double leave_op_hour = 0;
                    double discipline_days = 0;
                    double leave_q_hour = 0;

                    //計算天數用資料-勤怠
                    double leave_a_hour = 0;
                    double leave_b_hour = 0;
                    double leave_c_hour = 0;
                    double attend_days = 0;

                    //判斷是新增-true還是修改-false
                    bool isAdd = true;
                    string error = ""; //錯誤訊息

                    CFB2SH0200DAO sh020DAO = null;

                    sh020DAO = new CFB2SH0200DAO();
                    sh020DAO.AWARD_YEAR = award_year;
                    sh020DAO.AWARD_ROUND = award_round;
                    sh020DAO.getSatrtAndEndDT(award_year, award_round);
                    //年獎開始日期,結束日期
                    string start_DT = sh020DAO.AWARD_STIME;
                    string end_DT = sh020DAO.AWARD_ETIME;

                    //巡覽每row的資料第一列為title跳過(故i從3開始)
                    for (int i = 3; i <= sheet.LastRowNum; i++)
                    {
                        BeginTransaction();
                        error = "";
                        sh020DAO = new CFB2SH0200DAO();
                        sh020DAO.AWARD_YEAR = award_year;
                        sh020DAO.AWARD_ROUND = award_round;
                        sh020DAO.AWARD_DAYS = award_days;
                        sh020DAO.AWARD_STIME = start_DT;
                        sh020DAO.AWARD_ETIME = end_DT;
                        sh020DAO.CREATED_BY = SessionHandle.Current.emp_id;
                        sh020DAO.UPDATED_BY = SessionHandle.Current.emp_id;


                        if (sheet.GetRow(i) != null)
                        {
                            cell_empId = sheet.GetRow(i).GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            cell_work_days = sheet.GetRow(i).GetCell(14, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            cell_id_desc = sheet.GetRow(i).GetCell(16, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            cell_ability_pay = sheet.GetRow(i).GetCell(17, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            cell_level_pay = sheet.GetRow(i).GetCell(18, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            cell_pjob_pay = sheet.GetRow(i).GetCell(19, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            cell_profession_pay = sheet.GetRow(i).GetCell(20, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            cell_food_subsidy = sheet.GetRow(i).GetCell(21, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            cell_score_2h = sheet.GetRow(i).GetCell(22, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            cell_award_base = sheet.GetRow(i).GetCell(23, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            cell_leave_a_hour = sheet.GetRow(i).GetCell(24, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            cell_leave_b_hour = sheet.GetRow(i).GetCell(25, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            cell_leave_c_hour = sheet.GetRow(i).GetCell(26, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            cell_leave_q_hour = sheet.GetRow(i).GetCell(27, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            cell_leave_op_hour = sheet.GetRow(i).GetCell(28, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            cell_third_cnt_p = sheet.GetRow(i).GetCell(29, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            cell_second_cnt_p = sheet.GetRow(i).GetCell(30, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            cell_first_cnt_p = sheet.GetRow(i).GetCell(31, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            cell_third_cnt_m = sheet.GetRow(i).GetCell(32, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            cell_second_cnt_m = sheet.GetRow(i).GetCell(33, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            cell_first_cnt_m = sheet.GetRow(i).GetCell(34, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            cell_pay_type = sheet.GetRow(i).GetCell(35, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();

                            //工號 不可空白
                            if (cell_empId == "")
                            {
                                error += "工號欄位不可空白,\n";
                            }
                            else
                            {
                                dt.Clear();
                                dt = sg020DAO.getEmpCount(cell_empId);
                                if ((int)dt.Rows[0]["resultCount"] == 0)
                                {
                                    error += "工號不存在,\n";
                                }
                                else
                                {
                                    //判斷此工號是新增或者是修改
                                    dt.Clear();
                                    dt = sh020DAO.getAwardEmpCount(cell_empId);
                                    if ((int)dt.Rows[0]["resultCount"] == 0)
                                    {
                                        isAdd = true;
                                        //error += "工號新增,\n";
                                    }
                                    else
                                    {
                                        isAdd = false;
                                        //error += "工號修改,\n";
                                    }
                                }
                            }

                            //原始考績-若是第2,3回,則不可空白
                            if (award_round != "1")
                            {

                                if (cell_score_2h == "")
                                {
                                    error += "原始考績不可空白,\n";
                                }
                                else
                                {
                                    dt.Clear();
                                    dt = sh020DAO.getAwardBase(cell_score_2h);
                                    if ((int)dt.Rows[0]["resultCount"] == 0)
                                    {
                                        error += "原始考績不存在,\n";
                                    }
                                }
                            }

                            //支付狀態  不可空白
                            if (cell_pay_type == "")
                            {
                                error += "支付狀態不可空白,\n";
                            }
                            else
                            {
                                dt.Clear();
                                dt = sg020DAO.getPayTypeCount(cell_pay_type);
                                if ((int)dt.Rows[0]["resultCount"] == 0)
                                {
                                    error += "此支付狀態不存在,\n";
                                }
                            }


                            //檢查數字欄位
                            error += this.checkNumber(cell_work_days, "在職天數", 5, "");
                            error += this.checkNumber(cell_ability_pay, "職能俸", 7, "");
                            error += this.checkNumber(cell_level_pay, "資格俸", 7, "");
                            error += this.checkNumber(cell_pjob_pay, "職務俸", 7, "");
                            error += this.checkNumber(cell_profession_pay, "專業俸", 7, "");
                            error += this.checkNumber(cell_food_subsidy, "伙食津貼", 7, "");

                            error += this.checkNumber(cell_leave_op_hour, "遲到/早退 次數", 3, "");
                            error += this.checkNumber(cell_third_cnt_p, "嘉獎", 2, "");
                            error += this.checkNumber(cell_second_cnt_p, "小功", 2, "");
                            error += this.checkNumber(cell_first_cnt_p, "大功", 2, "");
                            error += this.checkNumber(cell_third_cnt_m, "申誡", 2, "");
                            error += this.checkNumber(cell_second_cnt_m, "小過", 2, "");
                            error += this.checkNumber(cell_first_cnt_m, "大過", 2, "");

                            error += this.checkNumberWithPoint(cell_leave_a_hour, "事假時數", 4, 1);
                            error += this.checkNumberWithPoint(cell_leave_b_hour, "有薪病假時數", 4, 1);
                            error += this.checkNumberWithPoint(cell_leave_c_hour, "無薪病假時數", 4, 1);
                            error += this.checkNumberWithPoint(cell_leave_q_hour, "曠工時數", 4, 1);


                            //儲存錯誤訊息
                            sheet.GetRow(i).CreateCell(0).CellStyle = style1;
                            sheet.GetRow(i).GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(error);
                            if (error != "")
                            {
                                valid = false;
                            }

                            //兩者共通的部份
                            if (valid)
                            {
                                dt.Clear();
                                if (award_round != "1")
                                {
                                    dt = sh020DAO.getAwardBase(cell_empId, cell_score_2h);
                                    cell_award_base = dt.Rows[0]["AWARD_BASE"].ToString();
                                }
                                else {
                                    cell_score_2h = "";
                                    cell_award_base = "1.000";
                                }
                              

                                //身份標示
                                /*
                                dt.Clear();
                                dt = sh020DAO.getID_DESC(cell_empId,start_DT,end_DT);
                                cell_id_desc = dt.Rows[0]["ID_DESC"].ToString();
                                */
                                //要修改的值
                                sh020DAO.EMP_ID = cell_empId;
                                sh020DAO.WORK_DAYS = cell_work_days;
                                sh020DAO.ID_DESC = cell_id_desc;
                                sh020DAO.ABILITY_PAY = cell_ability_pay.Replace(",", "");
                                sh020DAO.LEVEL_PAY = cell_level_pay.Replace(",", ""); ;
                                sh020DAO.PJOB_PAY = cell_pjob_pay.Replace(",", ""); ;
                                sh020DAO.PROFESSION_PAY = cell_profession_pay.Replace(",", ""); ;
                                sh020DAO.FOOD_SUBSIDY = cell_food_subsidy.Replace(",", "");
                                sh020DAO.SCORE_2H = cell_score_2h;
                                sh020DAO.AWARD_BASE = cell_award_base;
                                sh020DAO.LEAVE_A_HOUR = cell_leave_a_hour.Replace(",", "");
                                sh020DAO.LEAVE_B_HOUR = cell_leave_b_hour.Replace(",", "");
                                sh020DAO.LEAVE_C_HOUR = cell_leave_c_hour.Replace(",", "");
                                sh020DAO.LEAVE_Q_HOUR = cell_leave_q_hour.Replace(",", "");
                                sh020DAO.LEAVE_OP_HOUR = cell_leave_op_hour.Replace(",", "");
                                sh020DAO.PAY_TYPE = cell_pay_type;

                                //獎懲加減天數
                                wk_third_cnt_p = Convert.ToDouble(cell_third_cnt_p);
                                wk_second_cnt_p = Convert.ToDouble(cell_second_cnt_p);
                                wk_first_cnt_p = Convert.ToDouble(cell_first_cnt_p);
                                wk_third_cnt_m = Convert.ToDouble(cell_third_cnt_m);
                                wk_second_cnt_m = Convert.ToDouble(cell_second_cnt_m);
                                wk_first_cnt_m = Convert.ToDouble(cell_first_cnt_m);
                                third_cnt_p = wk_third_cnt_p % 3;
                                second_cnt_p = Convert.ToInt32((Math.Floor(wk_third_cnt_p / 3) + wk_second_cnt_p)) % 3;
                                first_cnt_p = wk_first_cnt_p + Math.Floor((Math.Floor(wk_third_cnt_p / 3) + wk_second_cnt_p) / 3);
                                third_cnt_m = wk_third_cnt_m % 3;
                                second_cnt_m = Convert.ToInt32((Math.Floor(wk_third_cnt_m / 3) + wk_second_cnt_m)) % 3;
                                first_cnt_m = wk_first_cnt_m + Math.Floor((Math.Floor(wk_third_cnt_m / 3) + wk_second_cnt_m) / 3);

                                reward_days = (third_cnt_p * y_third_cnt_p) + (second_cnt_p * y_second_cnt_p) + (first_cnt_p * y_first_cnt_p)
                                             + (third_cnt_m * y_third_cnt_m) + (second_cnt_m * y_second_cnt_m) + (first_cnt_m * y_first_cnt_m);

                                sh020DAO.THIRD_CNT_P = Convert.ToString(third_cnt_p);
                                sh020DAO.SECOND_CNT_P = Convert.ToString(second_cnt_p);
                                sh020DAO.FIRST_CNT_P = Convert.ToString(first_cnt_p);
                                sh020DAO.THIRD_CNT_M = Convert.ToString(third_cnt_m);
                                sh020DAO.SECOND_CNT_M = Convert.ToString(second_cnt_m);
                                sh020DAO.FIRST_CNT_M = Convert.ToString(first_cnt_m);


                                //勤怠扣除天數
                                wk_leave_B0 = 0;
                                wk_leave_B0_over30 = 0;
                                leave_a_hour = Convert.ToDouble(cell_leave_a_hour);
                                leave_b_hour = Convert.ToDouble(cell_leave_b_hour);
                                leave_c_hour = Convert.ToDouble(cell_leave_c_hour);

                                if (leave_b_hour > 30 * 8)
                                {
                                    wk_leave_B0_over30 = leave_b_hour - 30 * 8;
                                    wk_leave_B0 = 30 * 8;
                                }
                                else {
                                    wk_leave_B0 = leave_b_hour;
                                    wk_leave_B0_over30 = 0;
                                }
                                //attend_days = (leave_a_hour + leave_c_hour) /8* y_leave_uc +wk_leave_B0 / 8 * y_leave_b + wk_leave_B0_over30 / 8 * y_leave_b_over30;
                                
                                //實際在職天數
                                award_work_days = Convert.ToDouble(cell_work_days);
                                award_work_days = award_work_days + attend_days;

                                //紀律扣除天數
                                leave_q_hour = Convert.ToDouble(cell_leave_q_hour);
                                leave_op_hour = Convert.ToDouble(cell_leave_op_hour);
                                if (leave_op_hour >= 19)
                                {
                                    discipline_days = (leave_q_hour * y_leave_q) / 8 + (leave_op_hour - 18) * y_leave_op;
                                }
                                else
                                {
                                    discipline_days = (leave_q_hour * y_leave_q) / 8;
                                }

                                sh020DAO.ATTEND_DAYS = Convert.ToString(attend_days);
                                sh020DAO.AWARD_WORK_DAYS = Convert.ToString(award_work_days);
                                sh020DAO.REWARD_DAYS = Convert.ToString(reward_days);
                                sh020DAO.DISCIPLINE_DAYS = Convert.ToString(discipline_days);

                            }
                            //檢核無誤時,進行新增的作業
                            sh020DAO.getAddEmpData();
                            if (valid && isAdd == true)
                            {
                                sh020DAO.insertEMPByUpload_D(now);
                                addNum += 1;
                                Commit();
                            }
                            else if (valid && isAdd == false)   //檢核無誤時,進行修改的作業
                            {
                                sh020DAO.updateEMPByUpload_D(now);
                                updateNum += 1;
                                Commit();
                            }
                            else
                            {
                                RollBack();
                            }


                        }
                    }


                    //若只有title時 ,儲存錯誤訊息
                    if (sheet.LastRowNum < 3)
                    {
                        error = "EXCEL無資料";
                        sheet.CreateRow(3);
                        sheet.GetRow(3).CreateCell(0).CellStyle = style1;
                        sheet.GetRow(3).GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(error);
                        if (error != "")
                        {
                            valid = false;
                        }
                    }

                    if (!valid)
                    {
                        return workbook;
                        //檢核有錯，匯出附加說明的excel
                        //ExcelHandle.exportExcel(workbook, "檢核錯誤說明" + type);
                    }
                    else
                    {
                        if ((updateNum + addNum) > 0)
                        {
                            //更新年獎維護檔
                            BeginTransaction();
                            sh020DAO = new CFB2SH0200DAO();
                            sh020DAO.AWARD_YEAR = award_year;
                            sh020DAO.AWARD_ROUND = award_round;
                            sh020DAO.AWARD_DAYS = award_days;
                            sh020DAO.UPDATED_BY = SessionHandle.Current.emp_id;
                            sh020DAO.updateEMPByUpload_H(now);

                            //更新明細維護檔的年獎金額為0
                            sh020DAO.updateT0Zero_D(now);

                            Commit();
                        }

                    }


                }
                catch (Exception ex)
                {
                    RollBack();
                    throw;
                    //return ex.Message;
                }
            }
            return null;
            //return "0";
        }
        catch (Exception ex)
        {
            //return ex.Message;
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

    //檢查是否為數字(含小數)
    public string checkNumberWithPoint(string cellData, string cellName, int cellLength, int dotLength)
    {
        try
        {
            String error = "";
            double numCheckResult = 0;
            cellData = cellData.Replace(",", "");         //去除數字的,
            double maxValue = Math.Pow(10, cellLength );  //10^長度 

            int pointIndex = cellData.IndexOf(".");       //小數點的位置
            string dotData = "";                          //小數的資料
            if (pointIndex > -1)
            {
                dotData = cellData.Substring(pointIndex);
            }


            if (cellData == "")
                error += cellName + "不可空白\n";
            else
            {

                if (!double.TryParse(cellData.Trim(), out numCheckResult))
                {
                    error += cellName + "必須為數字, 且必須為整數" + cellLength + "位，小數" + dotLength + "位, \n";
                }
                else
                {
                    if (double.Parse(cellData.Trim()) > maxValue)
                    {
                        error += cellName + "必須為數字, 且必須為整數" + cellLength + "位，小數" + dotLength + "位, \n";
                    }
                }

            }

            return error;
        }
        catch (Exception)
        {
            throw;
        }
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
                error += cellName + "必須為數字, 且長度最大為" + cellLength + ", \n";
            }
        }

        return error;
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
        style_class = workbook.CreateCellStyle();


        //資料的字型
        IFont cellFont = workbook.CreateFont();
        cellFont.FontName = "新細明體";
        cellFont.FontHeightInPoints = 12;  //字型大小
        cellFont.Color = HSSFColor.Black.Index;   //字型顏色
        cellFont.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Normal;   //bold:粗體字
        style_class.SetFont(cellFont);

        //是否要有邊框
        if (isBorder)
        {
            //style.BottomBorderColor = HSSFColor.White.Index;
            style_class.BorderBottom = BorderStyle.Thin;
            style_class.BorderTop = BorderStyle.Thin;
            style_class.BorderLeft = BorderStyle.Thin;
            style_class.BorderRight = BorderStyle.Thin;
        }

        //文字位置 (預設靠左)
        if (align.ToLower() == "center")
        {
            style_class.Alignment = HorizontalAlignment.Center;
        }
        else if (align.ToLower() == "right")
        {
            style_class.Alignment = HorizontalAlignment.Right;
        }
        else
        {
            style_class.Alignment = HorizontalAlignment.Left;
        }

        //背景顏色
        if (colorCD > 0)
        {
            style_class.FillForegroundColor = (short)colorCD;
            style_class.FillPattern = FillPattern.SolidForeground;
            //style.FillBackgroundColor = HSSFColor.Yellow.Index;
        }



        return style_class;
    }



}