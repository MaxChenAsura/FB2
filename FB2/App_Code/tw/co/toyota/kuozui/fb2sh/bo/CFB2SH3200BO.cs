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
/// CFB2SH3200BO 的摘要描述
/// </summary>
public class CFB2SH3200BO : BaseService
{

    ICellStyle style_class;
    public CFB2SH3200BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

 


    //新增
    public string insertData(CFB2SH3200DAO sh320DAO)
    {
        string rtnmessage = "";
        try
        {

            //若需要則要進行邏輯檢查(與DB相關的)
            //00.檢查PK值有無重覆
            DataTable dupdata = sh320DAO.getPKData();
            if ((int)dupdata.Rows[0]["resultCount"] > 0)
            {
                rtnmessage += "年度 重覆";
            }


            //檢查OK更新
            if (rtnmessage == "")
            {
                try
                {
                    BeginTransaction();

                    sh320DAO.insertData();

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
    public string updateData(CFB2SH3200DAO dao)
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
        CFB2SH3200DAO sh320DAO = new CFB2SH3200DAO();
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
                        sh320DAO.deleteDataH(item.Item1);
                      
                        //刪除 年獎明細主檔
                        sh320DAO.deleteDataD(item.Item1, item.Item2, "TB_S_M_FR_AWARD_D");

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
    //刪除明細資料
    public string updateDataD(List<Tuple<string, string>> keysList,string deleteMemo)
    {
        CFB2SH3200DAO sh320DAO = new CFB2SH3200DAO();
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

                        sh320DAO.updateDataD(item.Item1, item.Item2,deleteMemo);

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
    public string execSP_S_AWARD_DATA(CFB2SH3200DAO sh320DAO)
    {
        string rtnmessage = "";//存在檢查後的訊息

        try
        {

            //檢查完成後，逐筆進行刪除
            if (rtnmessage == "")
            {
                sh320DAO.execSP_S_AWARD_DATA();
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
    public string updateRelease(CFB2SH3200DAO sh320DAO)
    {

        string rtnmessage = "";//存在檢查後的訊息
        try
        {
            if (rtnmessage == "")
            {
                try
                {
                    BeginTransaction();

                    sh320DAO.updateRelease();

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
    public IWorkbook createExcelFromTemplate(string excelPath, CFB2SH3200DAO sh320DAO, DataTable dt)
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
                        //部門代號
                        cell = row.CreateCell(5);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["DEPT_NO"].ToString());
                        //資格代號
                        cell = row.CreateCell(6);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["LEVEL_CD"].ToString());
                        //職務代號
                        cell = row.CreateCell(7);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["PJOB_CD"].ToString());
                        //職務代號
                        cell = row.CreateCell(8);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["PJOB_DESC"].ToString());
                        //入社日期
                        cell = row.CreateCell(9);
                        cell.CellStyle = stringLeftStyle;
                        dtFormat = dt.Rows[i]["JOIN_DT"].ToString() != "" ? Convert.ToDateTime(dt.Rows[i]["JOIN_DT"].ToString()).ToString("yyyy/MM/dd") : "";
                        cell.SetCellValue(dtFormat);
                        //年度天數
                        cell = row.CreateCell(10);
                        cell.CellStyle = stringLeftStyle;
                        dtFormat = dt.Rows[i]["LEAVE_DT"].ToString() != "" ? Convert.ToDateTime(dt.Rows[i]["LEAVE_DT"].ToString()).ToString("yyyy/MM/dd") : "";
                        cell.SetCellValue(dtFormat);
                        //服務年資
                        cell = row.CreateCell(11);
                        cell.CellStyle = stringLeftStyle;
                        dtFormat = dt.Rows[i]["LEAVE_DT"].ToString() != "" ? Convert.ToDateTime(dt.Rows[i]["LEAVE_DT"].ToString()).ToString("yyyy/MM/dd") : "";
                        //本回留停起日
                        cell = row.CreateCell(12);
                        cell.CellStyle = stringLeftStyle;
                        dtFormat = dt.Rows[i]["STAY_SDT"].ToString() != "" ? Convert.ToDateTime(dt.Rows[i]["STAY_SDT"].ToString()).ToString("yyyy/MM/dd") : "";
                        cell.SetCellValue(dtFormat);
                        //本回留停訖日
                        cell = row.CreateCell(13);
                        cell.CellStyle = stringLeftStyle;
                        dtFormat = dt.Rows[i]["STAY_EDT"].ToString() != "" ? Convert.ToDateTime(dt.Rows[i]["STAY_EDT"].ToString()).ToString("yyyy/MM/dd") : "";
                        cell.SetCellValue(dtFormat);

                        //在職天數
                        cell = row.CreateCell(14);
                        cell.CellStyle = stringLeftStyle;
                        //dtFormat = dt.Rows[i]["JOB_DT"].ToString() != "" ? Convert.ToDateTime(dt.Rows[i]["STAY_DT"].ToString()).ToString("yyyy/MM/dd") : "";
                        cell.SetCellValue(dt.Rows[i]["JOB_DT"].ToString());

                        //員工區分
                        cell = row.CreateCell(15);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["EMP_CD_DESC"].ToString());

                        //最終考績
                        cell = row.CreateCell(16);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["SCORE_FINAL"].ToString()); 
                        
                        //最終考績
                        cell = row.CreateCell(17);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["AWARD_DIFFER"].ToString());

                        //事假時數
                        cell = row.CreateCell(18);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["LEAVE_A_HOUR"].ToString());


                        //有薪病假時數 (扣0.5)
                        cell = row.CreateCell(19);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["LEAVE_B_HOUR"].ToString());

                        //無薪病假時數 (扣1)
                        cell = row.CreateCell(20);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["LEAVE_C_HOUR"].ToString());

                        //曠工時數
                        cell = row.CreateCell(21);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["LEAVE_Q_HOUR"].ToString());

                        //遲到/早退 次數
                        cell = row.CreateCell(22);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["LEAVE_OP_HOUR"].ToString());

                        //嘉獎
                        cell = row.CreateCell(23);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["THIRD_CNT_P"].ToString());

                        //小功
                        cell = row.CreateCell(24);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["SECOND_CNT_P"].ToString());

                        //大功
                        cell = row.CreateCell(25);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["FIRST_CNT_P"].ToString());

                        //申誡
                        cell = row.CreateCell(26);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["THIRD_CNT_M"].ToString());

                        //小過
                        cell = row.CreateCell(27);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["SECOND_CNT_M"].ToString());

                        //大過
                        cell = row.CreateCell(28);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["FIRST_CNT_M"].ToString());

                        //事假扣除天數
                        cell = row.CreateCell(29);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["PERSONAL_LEAVE_DAYS"].ToString());

                        //病假日除天數
                        cell = row.CreateCell(30);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["SICK_LEAVE_DAYS"].ToString());

                        //
                        cell = row.CreateCell(31);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["FIRST_CNT_M"].ToString());

                        //實際在職天數
                        cell = row.CreateCell(32);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["ACTUAL_JOB_DAYS"].ToString());

                        //年資獎金 ( C )
                        cell = row.CreateCell(33);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["AWARD_DAYS"].ToString());

                        //年資獎金*格差
                        cell = row.CreateCell(34);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["AWARD_DIFFER"].ToString());

                        //在職比例
                        cell = row.CreateCell(35);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["FIRST_CNT_M"].ToString());

                        //記律金額
                        cell = row.CreateCell(36);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["RULE_DECAMT"].ToString());

                        //年獎金額
                        cell = row.CreateCell(37);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["SHOULD_AMT"].ToString());

                        //稅額
                        cell = row.CreateCell(38);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["TAX_AMT"].ToString());

                        //實發年獎
                        cell = row.CreateCell(39);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["ACTUAL_AMT"].ToString());

                        //異動狀態
                        cell = row.CreateCell(40);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["CHG_STATUS_DESC"].ToString());

                        //刪除原因
                        cell = row.CreateCell(41);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["DELETE_MEMO"].ToString());

                       


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
                    cell = row.CreateCell(40);
                    cell.CellStyle = stringLeftStyleDate;
                    cell.SetCellValue("製表日期:" + DateTime.Now.ToString("yyyy/MM/dd"));

                    /*
                    for (int i = 0; i <= 48; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                    */
                    //if (tableName == "TB_S_M_FR_AWARD_DM")
                    //{
                    //    ExcelHandle.exportExcel(workbook, sh320DAO.AWARD_YEAR + "第" + sh320DAO.AWARD_ROUND + "回年獎維護資料.xlsx");
                    //}
                    //else if (tableName == "TB_S_S_AWARD_D")
                    //{
                    //    ExcelHandle.exportExcel(workbook, sh320DAO.AWARD_YEAR + "第" + sh320DAO.AWARD_ROUND + "回年獎原始資料.xlsx");
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