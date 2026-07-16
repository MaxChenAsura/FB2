using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.Util;
/// <summary>
/// CFB2SC2100BO 的摘要描述
/// </summary>
public class CFB2SC2100BO : BaseService
{
    public CFB2SC2100BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    IWorkbook workbook;
    ICellStyle stringLeftStyle;

    # region Qry
    public string deleteData(List<string> deleteList)
    {
        try
        {
            CFB2SC2100DAO dao = new CFB2SC2100DAO();
            BeginTransaction();

            foreach (string deleteitem in deleteList)
            {
                //刪除主檔資料
                dao.deleteData(deleteitem);
            }
            Commit();
            return "0";
        }
        catch (Exception)
        {
            RollBack();
            throw;
        }
    }
    //public string addData(CFB2SC2100DAO fb2sc)
    //{
    //    try
    //    {
    //        //取得現有資料
    //        DataTable tmp = fb2sc.getExistData();
    //        if (tmp.Rows.Count > 0)
    //        {
    //            return "資料重覆!";
    //        }
    //        BeginTransaction();
    //        fb2sc.addData();
    //        Commit();
    //        return "0";

    //    }
    //    catch (Exception ex)
    //    {
    //        RollBack();
    //        return ex.Message;
    //    }
    //}
    //public string updateData(CFB2SC2100DAO fb2sc)
    //{
    //    try
    //    {
    //        BeginTransaction();
    //        fb2sc.updateData();
    //        Commit();
    //        return "0";
    //    }
    //    catch (Exception ex)
    //    {
    //        RollBack();
    //        return ex.Message;
    //    }
    //}
    #endregion

    #region " Add "
    public DataTable getAddData(int startRowIndex, int maximumRows, string sortExpression, string salary_type)
    {
        try
        {
            CFB2SC2100DAO dao = new CFB2SC2100DAO();
            DataTable dt = new DataTable();
            if (salary_type == "A")
                dt = dao.getAddDataA(startRowIndex, maximumRows, sortExpression, salary_type);
            else if (salary_type == "B")
                dt = dao.getAddDataB(startRowIndex, maximumRows, sortExpression, salary_type);
            else if (salary_type == "C")
                dt = dao.getAddDataC(startRowIndex, maximumRows, sortExpression, salary_type);
            else if (salary_type == "D")
                dt = dao.getAddDataD(startRowIndex, maximumRows, sortExpression, salary_type);
            return dt;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public int getAddCount(int startRowIndex, int maximumRows, string salary_type)
    {
        try
        {
            CFB2SC2100DAO dao = new CFB2SC2100DAO();
            int count = 0;
            if (salary_type == "A")
                count = dao.getAddCountA(startRowIndex, maximumRows, salary_type);
            else if (salary_type == "B")
                count = dao.getAddCountB(startRowIndex, maximumRows, salary_type);
            else if (salary_type == "C")
                count = dao.getAddCountC(startRowIndex, maximumRows, salary_type);
            else if (salary_type == "D")
                count = dao.getAddCountD(startRowIndex, maximumRows, salary_type);
            return count;
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    public string saveAddData(CFB2SC2100DAO dao, List<string> salary_idList)
    {
        try
        {
            //取得現有資料
            DataTable tmp = dao.getExistData(salary_idList);
            if (tmp.Rows.Count > 0)
            {
                return "資料重覆!";
            }

            BeginTransaction();
            dao.saveAddData(salary_idList);
            foreach (string salary_id in salary_idList)
            {
                dao.saveAddDtl(salary_id);
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
    #endregion

    #region " Detail "
    public string chkMonthClose(string salary_dt,string salary_type,string pay_kind)
    {
        try
        {
            string msg = "0";
            CFB2SC2100DAO dao = new CFB2SC2100DAO();
            DataTable dt = dao.chkMonthClose(salary_dt, salary_type, pay_kind);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["CNT"].ToString() != "0")
                {
                    msg = "薪資計算前，需先將其他舊的計算月結後才能開始!!";
                }
            }

            return msg;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public string Execute1(string salary_type, string pay_kind, string salary_dt, string salary_ym, string salary_sdt, string salary_edt, string duty_sdt, string duty_edt)
    {
        try
        {
            string msg = "";
            bool isA01_Pass = false;
            CFB2SC2100DAO dao = new CFB2SC2100DAO();
            dao.SALARY_TYPE = salary_type;
            dao.SALARY_YM = salary_ym;
            dao.SALARY_DT = salary_dt;//發薪日期
            dao.SALARY_SDT = salary_sdt;
            dao.SALARY_EDT = salary_edt;

            if (salary_type == "A")
            {
                DataTable dtData = dao.getTB_S_M_SALARY_CTRL(salary_ym);
                if (dtData.Rows.Count > 0)
                {
                    BeginTransaction();
                    for (int i = 0; i < dtData.Rows.Count; i++)
                    {
                        dao.SALARY_TYPE = salary_type;
                        dao.SALARY_YM = salary_ym;
                        dao.SALARY_DT = salary_dt;//發薪日期
                        dao.OPERATION_ID = Convert.ToString(dtData.Rows[i]["OPERATION_ID"]);
                        dao.SALARY_SDT = salary_sdt;
                        dao.SALARY_EDT = salary_edt;


                        if (dao.OPERATION_ID == "A01")
                        {
                            dao.RunSP_S_EMP_DATA_MONTH_EXEC();
                            //確認SP有無成功
                            DataTable dtSPresult = dao.checkSP("SP_S_EMP_DATA_MONTH_EXEC");
                            if (dtSPresult.Rows.Count > 0)
                            {
                                //PROC_STATUS：Y = 成功,N = 失敗,E = Exception  ,PROC_LOG：處理結果中文訊息
                                if (Convert.ToString(dtSPresult.Rows[0]["PROC_STATUS"]) == "Y")
                                    isA01_Pass = true;
                                else
                                    msg += Convert.ToString(dtSPresult.Rows[0]["PROC_LOG"]) + "\\n";
                            }
                        }
                        //(1.2)若 畫面.發薪類別(SALARY_TYPE) = 'A'(月薪資類) 且資料列.前工程代號(OPERATION_ID) = G01(其他加扣月結)時,
                        if (dao.OPERATION_ID == "G01")
                        {
                            dao.UpdateTB_S_M_SUBSIDY_DEDUCTIONS_1_Dateial2(true);
                            dao.UpdateTB_S_M_SUBSIDY_DEDUCTIONS_D_Dateial2(true);
                            dao.InsertTB_S_M_SALARY_MONTH_CTRL_Dateial2("G01");
                        }

                        // (1.3)若 畫面.發薪類別(SALARY_TYPE)= 'A'(月薪資類) 且 資料列.前工程代號(OPERATION_ID) = I01(預付薪)時,
                        if (dao.OPERATION_ID == "I01")
                        {
                            if (dao.CheckTB_S_M_SALARY_CAL_H() > 0)
                                dao.InsertTB_S_M_SALARY_MONTH_CTRL_Dateial2("I01");
                            else
                                throw new Exception(Resources.Resource.wfb2sc_Salary_NotFound);
                        }

                        //(1.4)若 畫面.發薪類別(SALARY_TYPE) = 'A'(月薪資類) 且資料列.前工程代號(OPERATION_ID) = J01(其他類獎金月結)時,
                        if (dao.OPERATION_ID == "J01")
                        {
                            dao.UpdateTB_S_OTHER_BOUNS_D_Dateial2(true);
                            dao.InsertTB_S_M_SALARY_MONTH_CTRL_Dateial2("J01");
                        }
                    }
                    Commit();
                }
            }
            if (isA01_Pass)
            {
                BeginTransaction();
                dao.InsertTB_S_M_SALARY_MONTH_CTRL_Dateial2("A01");
                Commit();
            }
            if (salary_type == "A")
            {
                //將AS400 提案－其他加扣款檔 的 薪資處理狀態 薪資處理日期時間 發薪日期 清空
                //dao.update_DB1CMBC0_1();
            }

            //【薪資計算SP - SP_S_SALARY_CAL_EXEC】
            dao.RunSP_S_SALARY_CAL_EXEC(salary_type, pay_kind, salary_dt, salary_ym, salary_sdt, salary_edt, duty_sdt, duty_edt);

            if (salary_type == "A")
            {
                //將AS400 提案－其他加扣款檔 的 薪資處理狀態=Y 薪資處理日期時間=目前年月日 發薪日期=畫面發薪日期
                //dao.update_DB1CMBC0_2();
            }

            DataTable dtSPresult2 = dao.checkSP2("SP_S_SALARY_CAL_EXEC");
            if (dtSPresult2.Rows.Count > 0)
            {
                //PROC_STATUS：Y = 成功,N = 失敗,E = Exception  ,PROC_LOG：處理結果中文訊息
                if (Convert.ToString(dtSPresult2.Rows[0]["PROC_STATUS"]) == "Y")
                    msg = "0";
                else
                    msg = Convert.ToString(dtSPresult2.Rows[0]["PROC_LOG"]);
            }
            return msg;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public string Execute2_RunSP_S_SALARY_ABNORMAL_EXEC(string salary_type, string pay_kind, string salary_dt, string salary_ym, string salary_sdt, string salary_edt, string duty_sdt, string duty_edt)
    {
        try
        {
            string msg = "";
            CFB2SC2100DAO dao = new CFB2SC2100DAO();
            dao.RunSP_S_SALARY_ABNORMAL_EXEC(salary_type, pay_kind, salary_dt, salary_ym, salary_sdt, salary_edt, duty_sdt, duty_edt);
            string result = string.Empty;
            DataTable dtSPresult = dao.checkSP2("SP_S_SALARY_CAL_EXEC");
            if (dtSPresult.Rows.Count > 0)
            {
                //PROC_STATUS：Y = 成功,N = 失敗,E = Exception  ,PROC_LOG：處理結果中文訊息
                if (Convert.ToString(dtSPresult.Rows[0]["PROC_STATUS"]) == "Y")
                    msg = "0";
                else
                    msg = Convert.ToString(dtSPresult.Rows[0]["PROC_LOG"]);
            }
            return msg;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    #endregion

    #region "Excel Import"
    //下載Excel資料
    public IWorkbook createExcelFromTemplate(string salary_type, string salary_type_desc, string salary_dt, string excelPath)
    {
        try
        {
            //Excel初始化
            string type = "xlsx";
            FileStream fs = new FileStream(excelPath, FileMode.Open, FileAccess.Read);
            //依type判斷要用哪種方式產生
            if (type == "xls")
                workbook = new HSSFWorkbook(fs);
            else
                workbook = new XSSFWorkbook(fs);
            this.stringLeftStyle = this.setCellStyle(workbook, "left", false, 0);

            //取得範本sheet
            ISheet sheet = workbook.GetSheetAt(0);
            if (sheet != null)
            {
                CFB2SC2100DAO dao = new CFB2SC2100DAO();
                DataTable dtExcelData = dao.getExcelData(salary_type, salary_dt);
                if (dtExcelData.Rows.Count > 0)
                {
                    createHeader(sheet, salary_dt, salary_type_desc);
                    for (int i = 0; i < dtExcelData.Rows.Count; i++)
                    {
                        createSingleRow(sheet, dtExcelData.Rows[i], i + 3);
                    }
                    //匯出Excel
                    //ExcelHandle.exportExcel(workbook, "薪資計算異常解析資料." + type);
                    return workbook;
                }
                else
                    return null;

            }
            else
            {
                return null;
            }
        }
        catch (Exception)
        {
            throw;
        }
    }
    private void createHeader(ISheet sheet, string salary_dt, string salary_type_desc)
    {

        //整份第一行
        sheet.GetRow(0).CreateCell(1).SetCellValue(Convert.ToDateTime(salary_dt).ToString("yyyyMMdd"));   //發薪日期
        //整份第二行
        sheet.GetRow(1).CreateCell(1).SetCellValue(salary_type_desc);                                     //發薪類別

        sheet.GetRow(0).GetCell(1).CellStyle = stringLeftStyle;
        sheet.GetRow(1).GetCell(1).CellStyle = stringLeftStyle;
    }
    private void createSingleRow(ISheet sheet, DataRow RowExcel, int excelindex)
    {
        //每筆第一行
        IRow row = sheet.CreateRow(excelindex);
        ICell cell;

        cell = row.CreateCell(0);
        cell.CellStyle = stringLeftStyle;
        cell.SetCellValue(RowExcel["EMP_ID"].ToString());           //工號

        cell = row.CreateCell(1);
        cell.CellStyle = stringLeftStyle;
        cell.SetCellValue(RowExcel["EMP_NAME"].ToString());         //姓名

        cell = row.CreateCell(2);
        cell.CellStyle = stringLeftStyle;
        cell.SetCellValue(RowExcel["SALARY_ID"].ToString());     //薪資項目代號

        cell = row.CreateCell(3);
        cell.CellStyle = stringLeftStyle;
        cell.SetCellValue(RowExcel["SALARY_NAME"].ToString());   //薪資項目名稱

        cell = row.CreateCell(4);
        cell.CellStyle = stringLeftStyle;
        cell.SetCellValue(RowExcel["SEQ_NO"].ToString());       //序號

        cell = row.CreateCell(5);
        cell.CellStyle = stringLeftStyle;
        cell.SetCellValue(RowExcel["MSG_TYPE"].ToString());       //錯誤類別

        cell = row.CreateCell(6);
        cell.CellStyle = stringLeftStyle;
        cell.SetCellValue(RowExcel["OP_MSG"].ToString());       //處理訊息2

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
    #endregion
}