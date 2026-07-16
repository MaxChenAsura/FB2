using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Web.UI;
using NPOI.HSSF.Util;

/// <summary>
/// CFB2SC3100BO 的摘要描述
/// </summary>
public class CFB2SC3100BO : BaseService
{
    IWorkbook workbook;
    ICellStyle stringRightStyle;
    ICellStyle stringLeftStyle;
    ICellStyle stringCenterStyle;
    ICellStyle stringCenterTopLeftStyle;
    ICellStyle stringTitleStyle;
    ICellStyle stringLeftTileStyle;
    ICellStyle numbericStyle;
    static ICellStyle _doubleCellStyle;
    static ICellStyle _intCellStyle;

    public CFB2SC3100BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    
    public string delete_sm(string salary_type, string salary_dt)
    {
        CFB2SC3100DAO fb2sc = new CFB2SC3100DAO();
        string rtnmessage = "";
        //try
        //{
        //    foreach (var item in salary_data)
        //    {
        //        //檢查是否有資料
        //        DataTable dt = fb2sc.count_sm(salary_data[0].Item2, salary_dt);
        //        if ((int)dt.Rows[0]["smcount"] == 0)
        //        {
        //            rtnmessage += "月薪資所得別彙計表無相關資料";
        //        }
        //    }
        if (rtnmessage == "")
        {
            try
            {
                BeginTransaction();
                //foreach (var item in salary_data)
                //{
                fb2sc.delete_sm(salary_type, salary_dt);
                //}
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
            return rtnmessage;
        //}
        //catch (Exception ex)
        //{
        //    return ex.Message;
        //}
    }
    public string delete_srd(string salary_type, string salary_dt)
    {
        CFB2SC3100DAO fb2sc = new CFB2SC3100DAO();
        string rtnmessage = "";
        //try
        //{
        //    foreach (var item in salary_data)
        //    {
        //        //檢查是否有資料
        //        DataTable dt = fb2sc.count_srd(salary_data[0].Item2, salary_dt);
        //        if ((int)dt.Rows[0]["srdcount"] == 0)
        //        {
        //            rtnmessage += "其他類薪資彙計表無相關資料";
        //        }
        //    }
        //檢查OK逐筆刪除
        if (rtnmessage == "")
        {
            try
            {
                BeginTransaction();
                //foreach (var item in salary_data)
                //{
                fb2sc.delete_srd(salary_type, salary_dt);
                //}
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
            return rtnmessage;
        //}
        //catch (Exception ex)
        //{
        //    return ex.Message;
        //}
    }
    public string delete_so(string salary_type, string salary_dt)
    {
        CFB2SC3100DAO fb2sc = new CFB2SC3100DAO();
        string rtnmessage = "";
        //try
        //{
        //    foreach (var item in salary_data)
        //    {
        //        //檢查是否有資料
        //        DataTable dt = fb2sc.count_so(salary_data[0].Item2, salary_dt);
        //        if ((int)dt.Rows[0]["socount"] == 0)
        //        {
        //            rtnmessage += "其他類薪資彙計表無相關資料";
        //        }
        //    }
        //檢查OK逐筆刪除
        if (rtnmessage == "")
        {
            try
            {
                BeginTransaction();
                //foreach (var item in salary_data)
                //{
                fb2sc.delete_so(salary_type, salary_dt);
                //}
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
            return rtnmessage;
        //}
        //catch (Exception ex)
        //{
        //    return ex.Message;
        //}
    }
    public string delete_srod(string salary_type, string salary_dt)
    {
        CFB2SC3100DAO fb2sc = new CFB2SC3100DAO();
        string rtnmessage = "";
        //try
        //{
        //    foreach (var item in salary_data)
        //    {
        //        //檢查是否有資料
        //        DataTable dt = fb2sc.count_srod(salary_data[0].Item2, salary_dt);
        //        if ((int)dt.Rows[0]["srodcount"] == 0)
        //        {
        //            rtnmessage += "員工其他類薪資明細表無相關資料";
        //        }
        //    }
        //檢查OK逐筆刪除
        if (rtnmessage == "")
        {
            try
            {
                BeginTransaction();
                //foreach (var item in salary_data)
                //{
                fb2sc.delete_srod(salary_type, salary_dt);
                //}
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
            return rtnmessage;
        //}
        //catch (Exception ex)
        //{
        //    return ex.Message;
        //}
    }
    public DataTable count_sm(string salary_type, string salary_dt)
    {
        CFB2SC3100DAO fb2sc = new CFB2SC3100DAO();
        try
        {
            return fb2sc.count_sm(salary_type, salary_dt);
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    public DataTable count_srd(string salary_type, string salary_dt)
    {
        CFB2SC3100DAO fb2sc = new CFB2SC3100DAO();
        try
        {
            return fb2sc.count_srd(salary_type, salary_dt);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public DataTable count_so(string salary_type, string salary_dt)
    {
        CFB2SC3100DAO fb2sc = new CFB2SC3100DAO();
        try
        {
            return fb2sc.count_so(salary_type, salary_dt);
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    public DataTable count_srod(string salary_type, string salary_dt)
    {
        CFB2SC3100DAO fb2sc = new CFB2SC3100DAO();
        try
        {
            return fb2sc.count_srod(salary_type, salary_dt);
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    public DataTable getEmpResult(string salary_type, string salary_dt, string pay_kind)
    {
        CFB2SC3100DAO fb2sc = new CFB2SC3100DAO();
        try
        {
            return fb2sc.getEmpResult(salary_type, salary_dt, pay_kind);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public DataTable getEmpResultTemp(string salary_type, string salary_dt, string pay_kind)
    {
        CFB2SC3100DAO fb2sc = new CFB2SC3100DAO();
        try
        {
            return fb2sc.getEmpResultTemp(salary_type, salary_dt, pay_kind);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public DataTable getSalaryPay(string salary_type, string salary_dt, string pay_kind, string emp_id)
    {
        CFB2SC3100DAO fb2sc = new CFB2SC3100DAO();
        try
        {
            return fb2sc.getSalaryPay(salary_type, salary_dt, pay_kind, emp_id);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public DataTable getSalaryGroupH(string salary_type)
    {
        CFB2SC3100DAO fb2sc = new CFB2SC3100DAO();
        try
        {
            return fb2sc.getSalaryGroupH(salary_type);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public DataTable getSalaryPay2(string salary_type, string salary_dt, string pay_kind, string emp_id, string group_id)
    {
        CFB2SC3100DAO fb2sc = new CFB2SC3100DAO();
        try
        {
            return fb2sc.getSalaryPay2(salary_type, salary_dt, pay_kind, emp_id, group_id);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public DataTable getSalaryPay3(string salary_type, string salary_dt, string pay_kind, string emp_id, string group_id)
    {
        CFB2SC3100DAO fb2sc = new CFB2SC3100DAO();
        try
        {
            return fb2sc.getSalaryPay3(salary_type, salary_dt, pay_kind, emp_id, group_id);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public string addData(CFB2SC3100DAO fb2sc, string salary_type, string salary_dt, string pay_kind, string AMT_B)
    {
        string errStr = "";
        //try
        //{
        //    DataTable existdata = fb2sc.getExistSrd(salary_dt);
        //    if ((int)existdata.Rows[0]["srd_count"] > 0)
        //        errStr += "員工月薪資明細表資料已存在";
        if (errStr == "")
        {
            try
            {
                BeginTransaction();
                fb2sc.addData(salary_type, salary_dt, pay_kind, AMT_B);
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
            return errStr;
        //}
        //catch (Exception ex)
        //{
        //    return ex.Message;
        //}
    }

    public string addData2(CFB2SC3100DAO fb2sc, string salary_type, string salary_dt, string pay_kind, string AMT_B)
    {
        string errStr = "";
        //try
        //{
        //    DataTable existdata = fb2sc.getExistSrod(salary_data[0].Item2, salary_dt, pay_kind);
        //    if ((int)existdata.Rows[0]["srod_count"] > 0)
        //        errStr += "員工其他類薪資明細表資料已存在";
        if (errStr == "")
        {
            try
            {
                BeginTransaction();
                fb2sc.addData2(salary_type, salary_dt, pay_kind, AMT_B);
                Commit();
                return "0";
            }
            catch (Exception ex)
            {
                RollBack();
                return ex.Message;
            }
        }
        return errStr;
        //}
        //catch (Exception ex)
        //{
        //    return ex.Message;
        //}
    }

    public string insertSALARY_MONTH(CFB2SC3100DAO fb2sc, string salary_dt)
    {
        string errStr = "";
        //try
        //{
        //    DataTable existdata = fb2sc.getExistSm(salary_dt);
        //    if ((int)existdata.Rows[0]["sm_count"] > 0)
        //        errStr += "月薪資所得別彙計表資料已存在";
        if (errStr == "")
        {
            try
            {
                BeginTransaction();
                fb2sc.insertSALARY_MONTH(salary_dt);
                Commit();
                return "0";
            }
            catch (Exception ex)
            {
                RollBack();
                return ex.Message;
            }
        }
        return errStr;
        //}
        //catch (Exception ex)
        //{
        //    return ex.Message;
        //}
    }

    public string insertSALARY_OTHER(CFB2SC3100DAO fb2sc, string salary_type, string salary_dt, string pay_kind)
    {
        string errStr = "";
        //try
        //{
        //    DataTable existdata = fb2sc.getExistSo(salary_data[0].Item2, salary_dt, pay_kind);
        //    if ((int)existdata.Rows[0]["so_count"] > 0)
        //        errStr += "其他類薪資彙計表資料已存在";
        if (errStr == "")
        {
            try
            {
                BeginTransaction();
                fb2sc.insertSALARY_OTHER(salary_dt);
                Commit();
                return "0";
            }
            catch (Exception ex)
            {
                RollBack();
                return ex.Message;
            }
        }
        return errStr;
        //}
        //catch (Exception ex)
        //{
        //    return ex.Message;
        //}
    }
    public DataTable getSalaryGroupH_xls_header(string salary_type)
    {
        CFB2SC3100DAO fb2sc = new CFB2SC3100DAO();
        try
        {
            return fb2sc.getSalaryGroupH_xls_header(salary_type);
        }
        catch (Exception ex)
        {
            throw;
        }
    }
   
    private ISheet createSheet(ISheet sheet, string type, string sheetName)
    {
        if (type == "xls")
        {
            //workbook = new HSSFWorkbook();
            sheet = (HSSFSheet)workbook.CreateSheet(sheetName);
        }
        else
        {
            //workbook = new XSSFWorkbook();
            sheet = workbook.CreateSheet(sheetName);
        }
        return sheet;
    }
    //sheet產生流程
    private void createDataFlow(ISheet sheet, int checkedCount, string salary_type, CFB2SC3100DAO fb2sc, string sheetName, string company_cd)
    {
        int nextRow = 1;
        List<Tuple<string, string>> subjectAMT = new List<Tuple<string, string>>();
        nextRow += createTile(sheet, checkedCount, salary_type, fb2sc, sheetName, nextRow);//sheet title
        subjectAMT = createHeader(sheet, checkedCount, salary_type, fb2sc, company_cd, nextRow);//sheet header
        nextRow++;

        nextRow += createSubjectAndDept(sheet, checkedCount, salary_type, fb2sc, company_cd, nextRow, subjectAMT);

        for (int col2 = 0; col2 <= subjectAMT.Count+2; col2++)
        {
            sheet.AutoSizeColumn(col2);
        }
    }
    //sheet title
    private int createTile(ISheet sheet, int checkedCount, string salary_type, CFB2SC3100DAO fb2sc, string sheetName, int currentRow)
    {
        ICell cell;
        IRow row = sheet.CreateRow(0);

        cell = row.CreateCell(0);
        cell.CellStyle = stringTitleStyle;
        cell.SetCellValue("薪資所得彙計表");
        sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 0, 8));

        row = sheet.CreateRow(1);
        cell = row.CreateCell(0);
        cell.CellStyle = stringLeftTileStyle;
        cell.SetCellValue("發薪類別");

        cell = row.CreateCell(1);
        cell.CellStyle = stringLeftTileStyle;
        cell.SetCellValue(fb2sc.SALARY_TYPE);
        sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(1, 1, 1, 2));

        cell = row.CreateCell(3);
        cell.CellStyle = stringLeftTileStyle;
        cell.SetCellValue("處理狀態");

        cell = row.CreateCell(4);
        cell.CellStyle = stringLeftTileStyle;
        cell.SetCellValue(fb2sc.PROCESS_STATUS);
        sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(1, 1, 4, 5));


        row = sheet.CreateRow(2);
        cell = row.CreateCell(0);
        cell.CellStyle = stringLeftTileStyle;
        cell.SetCellValue("發薪年月");

        cell = row.CreateCell(1);
        cell.CellStyle = stringLeftTileStyle;
        cell.SetCellValue(fb2sc.SALARY_YM);
        sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(2, 2, 1, 2));

        cell = row.CreateCell(3);
        cell.CellStyle = stringLeftTileStyle;
        cell.SetCellValue("關帳代號");

        cell = row.CreateCell(4);
        cell.CellStyle = stringLeftTileStyle;
        cell.SetCellValue(fb2sc.PAY_ID);
        sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(2, 2, 4, 5));

        if (checkedCount == 1)
        {
            row = sheet.CreateRow(3);
            cell = row.CreateCell(0);
            cell.CellStyle = stringLeftTileStyle;
            cell.SetCellValue("發薪日期");

            cell = row.CreateCell(1);
            cell.CellStyle = stringLeftTileStyle;
            cell.SetCellValue(fb2sc.SALARY_DT);
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(3, 3, 1, 2));

            cell = row.CreateCell(3);
            cell.CellStyle = stringLeftTileStyle;
            cell.SetCellValue("發放項目");

            cell = row.CreateCell(4);
            cell.CellStyle = stringLeftTileStyle;
            cell.SetCellValue(fb2sc.PAY_KIND_DESC);
            sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(3, 3, 4, 5));
        }
        row = sheet.CreateRow(4);
        cell = row.CreateCell(0);
        cell.CellStyle = stringLeftTileStyle;
        cell.SetCellValue("公司別");

        cell = row.CreateCell(1);
        cell.CellStyle = stringLeftTileStyle;
        cell.SetCellValue(sheetName);
        sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(4, 4, 1, 2));

        cell = row.CreateCell(3);
        cell.CellStyle = stringLeftTileStyle;
        cell.SetCellValue("列印日期");

        cell = row.CreateCell(4);
        cell.CellStyle = stringLeftTileStyle;
        cell.SetCellValue(DateTime.Today.ToString("yyyy/MM/dd"));
        sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(4, 4, 4, 5));

        return 4;
    }
    //sheet header
    private List<Tuple<string, string>> createHeader(ISheet sheet, int checkedCount, string salary_type, CFB2SC3100DAO fb2sc, string company_cd, int currentRow )
    {
        List<Tuple<string, string>> subjectAMT = new List<Tuple<string, string>>();
        ICell cell;
        IRow row = sheet.CreateRow(5);
        cell = row.CreateCell(0);
        cell.CellStyle = stringLeftStyle;
        cell.SetCellValue("科目");

        cell = row.CreateCell(1);
        cell.CellStyle = stringLeftStyle;
        cell.SetCellValue("部門別");

        cell = row.CreateCell(2);
        cell.CellStyle = stringLeftStyle;
        cell.SetCellValue("人數");

        int cellIndex = 3;
        DataTable dtGroup = getSalaryGroupH_xls_header(salary_type);
        DataTable dtHeader = new DataTable();
        for (int i = 0; i < dtGroup.Rows.Count; i++)
        {
            string AMT = "";
            if (checkedCount > 1)
            {
                AMT = "AMT_" + dtGroup.Rows[i]["GROUP_ID"].ToString();
                dtHeader = fb2sc.getSalaryMonth_xls2_header(company_cd, AMT);
            }
            else
            {
                if (salary_type == "A")
                {
                    AMT = "AMT_" + dtGroup.Rows[i]["GROUP_ID"].ToString();
                    dtHeader = fb2sc.getSalaryMonth_xls1_header(company_cd, AMT);
                }
                else
                {
                    AMT = "AMT_BC" + dtGroup.Rows[i]["GROUP_ID"].ToString().Substring(2);
                    dtHeader = fb2sc.getSalaryOther_xls1_header(salary_type, company_cd, AMT);
                }
            }
            if (dtHeader.Rows.Count > 0)
            {
                if (dtHeader.Rows[0]["AMT"].ToString() != "0")
                {
                    if (salary_type == "A")
                        subjectAMT.Add(new Tuple<string, string>(dtGroup.Rows[i]["GROUP_ID"].ToString(), Convert.ToString(dtHeader.Rows[0]["AMT"])));
                    else
                        subjectAMT.Add(new Tuple<string, string>(dtGroup.Rows[i]["GROUP_ID"].ToString().Substring(2), Convert.ToString(dtHeader.Rows[0]["AMT"])));

                    cell = row.CreateCell(cellIndex);
                    cell.CellStyle = stringLeftStyle;
                    cell.SetCellValue(dtGroup.Rows[i]["GROUP_NAME"].ToString());
                    cellIndex++;
                }
            }
        }

        return subjectAMT;

    }
    //sheet subject and department data
    private int createSubjectAndDept(ISheet sheet, int checkedCount, string salary_type, CFB2SC3100DAO fb2sc, string company_cd, int currentRow, List<Tuple<string, string>> subjectAMT)
    {
        IRow row;
        ICell cell;
        DataTable dtBody = new DataTable();
        if (checkedCount > 1)
        {
            dtBody = fb2sc.getSalaryMonth_xls2_body(company_cd);
        }
        else
        {
            if (salary_type == "A")
            {
                dtBody = fb2sc.getSalaryMonth_xls1_body(company_cd);
            }
            else
            {
                dtBody = fb2sc.getSalaryOther_xls1_body(salary_type, company_cd);
            }
        }
        //int[,] AMTvalue = new int[SalaryOther_xls_body.Rows.Count, i];
        string newDesc = "";
        int sameSubjectCount = 0; //同樣科目部門數目
        int totalPerson = 0; //總人數
        for (int indexBody = 0; indexBody < dtBody.Rows.Count; indexBody++)
        {
            row = sheet.CreateRow(currentRow);
            //科目
            if (newDesc != dtBody.Rows[indexBody]["DESC1"].ToString())
            {
                if (indexBody != 0)
                {
                    sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(currentRow - sameSubjectCount, currentRow - 1, 0, 0));
                }
                sameSubjectCount = 1;
                newDesc = dtBody.Rows[indexBody]["DESC1"].ToString();
                cell = row.CreateCell(0);
                cell.CellStyle = stringCenterStyle;
                cell.SetCellValue(newDesc);
            }
            else
            {
                cell = row.CreateCell(0);
                cell.CellStyle = stringCenterStyle;
                sameSubjectCount++;
            }

            //部門別
            cell = row.CreateCell(1);
            cell.CellStyle = stringLeftStyle;
            cell.SetCellValue(dtBody.Rows[indexBody]["ACC_DEPT_NAME"].ToString());

            cell = row.CreateCell(2);
            cell.CellStyle = numbericStyle;
            cell.SetCellValue(Convert.ToDouble(dtBody.Rows[indexBody]["CNT"]));
            totalPerson += Convert.ToInt32(dtBody.Rows[indexBody]["CNT"]);

            //每個部門項目的金額
            for (int headerIndex = 0; headerIndex < subjectAMT.Count; headerIndex++)
            {
                string AMT = subjectAMT[headerIndex].Item1; //group_id
                DataTable dtEach = new DataTable();
                if (checkedCount > 1)
                {
                    dtEach = fb2sc.eachSalary_Month_2(company_cd, AMT);
                }
                else
                {
                    if (salary_type == "A")
                        dtEach = fb2sc.eachSalary_Month(company_cd, AMT);
                    else
                        dtEach = fb2sc.eachSalary_Other(salary_type, company_cd, AMT);
                }
                cell = row.CreateCell(headerIndex + 3);
                cell.CellStyle = numbericStyle;

                cell.SetCellValue(Convert.ToDouble(dtEach.Rows[indexBody]["AMT"]));
            }

            currentRow++;

            if (indexBody == dtBody.Rows.Count - 1)
            {
                sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(currentRow - sameSubjectCount, currentRow - 1, 0, 0));
            }
        }

        //總計 1行
        row = sheet.CreateRow(currentRow);
        cell = row.CreateCell(0);
        cell.CellStyle = stringLeftStyle;
        cell.SetCellValue("總計");

        cell = row.CreateCell(1);
        cell.CellStyle = stringLeftStyle;
        cell.SetCellValue("");

        cell = row.CreateCell(2);
        cell.CellStyle = numbericStyle;
        cell.SetCellValue(Convert.ToDouble(totalPerson));

        for (int cellSum = 0; cellSum < subjectAMT.Count; cellSum++)
        {
            cell = row.CreateCell(3 + cellSum);
            cell.CellStyle = numbericStyle;
            cell.SetCellValue(Convert.ToDouble(subjectAMT[cellSum].Item2));
        }
        currentRow++;

        return currentRow;
    }
    
    public IWorkbook createExcel1(int checkedCount, string salary_type, CFB2SC3100DAO fb2sc, string type)
    {
        workbook = null;
        stringRightStyle = null;
        stringLeftStyle = null;
        stringCenterStyle = null;
        stringCenterTopLeftStyle = null;
        stringTitleStyle = null;
        stringLeftTileStyle = null;
        numbericStyle = null;
        _doubleCellStyle = null;
        _intCellStyle = null;

        //取得範本sheet
        ISheet sheet = null;
        try
        {
            if (type == "xls")
                workbook = new HSSFWorkbook();
            else
                workbook = new XSSFWorkbook();

            this.stringRightStyle = this.setCellStyle(workbook, "right", true, 0, false, "");
            this.stringLeftStyle = this.setCellStyle(workbook, "left", true, 0, false, "");
            this.stringCenterStyle = this.setCellStyle(workbook, "center", false, 0, true, "left");
            this.stringCenterTopLeftStyle = this.setCellStyle(workbook, "center", false, 0, true, "first");
            this.stringTitleStyle = this.setCellStyle(workbook, "center", false, 0, true, "");
            this.stringLeftTileStyle = this.setCellStyle(workbook, "left", false, 0, false, "");
            //數字格式,有千分位,
            this.numbericStyle = this.workbook.CreateCellStyle();
            this.numbericStyle = this.stringRightStyle;
            this.numbericStyle.DataFormat = this.workbook.CreateDataFormat().GetFormat("#,##0");

            //20201026 改迴圈處理[公司代碼,公司簡稱]
            List<Tuple<string, string>> compList = new List<Tuple<string, string>>();
            //全選
            if (fb2sc.COMPANY_CD == "-1")
            {
                DataTable dt = fb2sc.getCOMPANY_CD();
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        compList.Add(new Tuple<string, string>(dt.Rows[i]["COMPANY_CD"].ToString().ToUpper()
                                                               , dt.Rows[i]["COMPANY_SNAME"].ToString()));
                    }
                }
            }
            else {
                //單選
                string company_sname = fb2sc.getCOMPANY_SNAME(fb2sc.COMPANY_CD.ToUpper());
                compList.Add(new Tuple<string, string>(fb2sc.COMPANY_CD.ToUpper()
                                                      , company_sname));
            }

            foreach (var item in compList)
            {
                sheet = createSheet(sheet, type, item.Item2);
                createDataFlow(sheet, checkedCount, salary_type, fb2sc, item.Item2, item.Item1);
                
            }

            return workbook;
        }
        catch
        {
            throw;
        }
        finally
        {
            workbook.Clear();
            //fs.Close();
            sheet = null;
            workbook = null;
        }
    }

    public IWorkbook createExcel2(int count, List<Tuple<string, string, string>> salary_data, CFB2SC3100DAO fb2sc, string type)
    {
        ////FileStream fs = null;
        workbook = null;
        stringRightStyle = null;
        stringLeftStyle = null;
        stringCenterStyle = null;
        stringCenterTopLeftStyle = null;
        stringTitleStyle = null;
        stringLeftTileStyle = null;
        numbericStyle = null;
        _doubleCellStyle = null;
        _intCellStyle = null;
        //取得範本sheet
        ISheet sheet = null;
        try
        {
            int changeStyleIndex = 0;
            ICellStyle style1;
            ICellStyle style2;
            if (type == "xls")
            {
                workbook = new HSSFWorkbook();
            }
            else
            {
                workbook = new XSSFWorkbook();
            }

            if (_doubleCellStyle == null)
            {
                _doubleCellStyle = workbook.CreateCellStyle();
                _doubleCellStyle.DataFormat = workbook.CreateDataFormat().GetFormat("#,##0.00");
            }

            if (_intCellStyle == null)
            {
                _intCellStyle = workbook.CreateCellStyle();
                _intCellStyle.DataFormat = workbook.CreateDataFormat().GetFormat("#,##0");
            }

            //靠右
            this.stringRightStyle = this.setCellStyle(workbook, "right", true, 0, false, "");
            this.stringCenterStyle = this.setCellStyle(workbook, "left", false, 0, true, "left");
            
            #region 公司別國瑞
            if (fb2sc.COMPANY_CD == "-1" || fb2sc.COMPANY_CD.ToUpper() == "K")
            {
                if (type == "xls")
                {
                    //workbook = new HSSFWorkbook();
                    sheet = (HSSFSheet)workbook.CreateSheet("KZ");
                    style1 = (HSSFCellStyle)workbook.CreateCellStyle();
                }
                else
                {
                    //workbook = new XSSFWorkbook();
                    sheet = workbook.CreateSheet("KZ");
                    style1 = (XSSFCellStyle)workbook.CreateCellStyle();
                }


                IFont font1 = workbook.CreateFont();
                font1.FontName = "新細明體";
                font1.FontHeightInPoints = 12;
                style1.SetFont(font1);

                style2 = workbook.CreateCellStyle();
                style2.SetFont(font1);

                #region 月薪資類
                if (fb2sc.SALARY_TYPE.Substring(0, 1) == "A")
                {
                    #region 月薪資類表頭
                    IRow row = sheet.CreateRow(0);
                    ICell cell;
                    cell = row.CreateCell(0);
                    cell.CellStyle = style1;
                    cell.SetCellValue("發薪類別");
                    sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 1, 2));

                    cell = row.CreateCell(1);
                    cell.CellStyle = style1;
                    cell.SetCellValue("月薪資類");

                    row = sheet.CreateRow(1);
                    cell = row.CreateCell(0);
                    cell.CellStyle = style1;
                    cell.SetCellValue("發薪日期");

                    cell = row.CreateCell(1);
                    cell.CellStyle = style1;
                    cell.SetCellValue(fb2sc.SALARY_DT);

                    row = sheet.CreateRow(2);
                    cell = row.CreateCell(0);
                    cell.CellStyle = style1;
                    cell.SetCellValue("公司別");

                    cell = row.CreateCell(1);
                    cell.CellStyle = style1;
                    cell.SetCellValue("KZ");

                    cell = row.CreateCell(3);
                    cell.CellStyle = style1;
                    cell.SetCellValue("列印日期");

                    cell = row.CreateCell(4);
                    cell.CellStyle = style1;
                    cell.SetCellValue(DateTime.Today.ToString("yyyy/MM/dd"));
                    sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(3, 3, 4, 5));

                    IRow row4 = sheet.CreateRow(3);
                    cell = row4.CreateCell(0);
                    cell.CellStyle = style1;
                    cell.SetCellValue("基本資料");

                    IRow row5 = sheet.CreateRow(4);
                    cell = row5.CreateCell(0);
                    cell.CellStyle = style1;
                    cell.SetCellValue("工號");

                    cell = row5.CreateCell(1);
                    cell.CellStyle = style1;
                    cell.SetCellValue("姓名");

                    cell = row5.CreateCell(2);
                    cell.CellStyle = style1;
                    cell.SetCellValue("月終狀態");

                    cell = row5.CreateCell(3);
                    cell.CellStyle = style1;
                    cell.SetCellValue("本月在職天數");


                    //DataTable dt = fb2sc.getSalaryItem1();
                    //DataTable dt2 = fb2sc.getSalaryItem2();
                    //DataTable dt3 = fb2sc.getSalaryItem3();
                    //DataTable dt4 = fb2sc.getSalaryItem4();
                    //DataTable dt5 = fb2sc.getSalaryItem5();
                    //DataTable dt6 = fb2sc.getSalaryItem6();
                    //DataTable dt7 = fb2sc.getSalaryItem7();
                    //DataTable dt8 = fb2sc.getSalaryItem8();
                    //DataTable dt9 = fb2sc.getSalaryItem9();

                    //DataTable dtAll = fb2sc.getTITLE("");
                    int row5count = 4;
                    for (int i = 1; i <= 8; i++)
                    {
                        if (i == 5)
                            changeStyleIndex = row5count;
                        DataTable dt = fb2sc.getTITLE(i.ToString());
                        if (dt.Rows.Count > 0)
                        {
                            cell = row4.CreateCell(row5count);
                            cell.CellStyle = style1;
                            cell.SetCellValue(dt.Rows[0]["TITLE_TYPE_NAME"].ToString());
                            for (int k = 0; k < dt.Rows.Count; k++)
                            {
                                cell = row5.CreateCell(row5count);
                                cell.CellStyle = style2;
                                cell.SetCellValue(dt.Rows[k]["TITLE_NAME"].ToString());
                                sheet.AutoSizeColumn(row5count);
                                row5count++;
                            }
                        }
                    }
                    #endregion

                    int contentStartRow = 0;
                    int maxAMT = fb2sc.getmaxAMT();

                    for (int a = 0; a < count; a++) //count:選取筆數
                    {
                        DataTable dt0 = fb2sc.getContent(salary_data[a].Item2, salary_data[a].Item3, maxAMT, "K");
                        if (dt0.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt0.Rows.Count; i++)
                            {
                                int cellcount = 0;
                                row = sheet.CreateRow(contentStartRow + 5);
                                contentStartRow++;
                                cell = row.CreateCell(cellcount);
                                cell.CellStyle = style2;
                                cell.SetCellValue(dt0.Rows[i]["EMP_ID"].ToString());
                                cellcount++;

                                cell = row.CreateCell(cellcount);
                                cell.CellStyle = style2;
                                cell.SetCellValue(dt0.Rows[i]["EMP_NAME"].ToString());
                                cellcount++;

                                cell = row.CreateCell(cellcount);
                                cell.CellStyle = style2;
                                cell.SetCellValue(dt0.Rows[i]["EMP_CHG_CD"].ToString());
                                cellcount++;

                                cell = row.CreateCell(cellcount);
                                cell.SetCellValue(Convert.ToDouble(dt0.Rows[i]["WORK_DAYS_MONTH"]));
                                cell.SetCellType(CellType.Numeric);
                                cell.CellStyle = _intCellStyle;
                                cellcount++;

                                for (int k = 1; k <= maxAMT; k++)
                                {
                                    cell = row.CreateCell(cellcount);
                                    cell.SetCellType(CellType.Numeric);
                                    cell.SetCellValue(Convert.ToDouble(dt0.Rows[i]["AMOUNT_" + k.ToString().PadLeft(3, '0')]));
                                    if (cellcount >= changeStyleIndex)
                                        cell.CellStyle = _doubleCellStyle;
                                    else
                                        cell.CellStyle = _intCellStyle;

                                    cellcount++;
                                }
                            }
                        }
                    }
                }
                #endregion

                #region 其他類薪資
                else
                {
                    font1.FontName = "新細明體";
                    font1.FontHeightInPoints = 12;
                    style1.SetFont(font1);

                    style2 = workbook.CreateCellStyle();
                    style2.SetFont(font1);
                    #region 其他類薪資表頭
                    IRow row_2 = sheet.CreateRow(0);
                    ICell cell_2;
                    cell_2 = row_2.CreateCell(0);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue("發薪類別");
                    sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 1, 2));

                    cell_2 = row_2.CreateCell(1);
                    cell_2.CellStyle = style2;
                    cell_2.SetCellValue(fb2sc.SALARY_TYPE);

                    row_2 = sheet.CreateRow(1);
                    cell_2 = row_2.CreateCell(0);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue("發薪日期");

                    cell_2 = row_2.CreateCell(1);
                    cell_2.CellStyle = style2;
                    cell_2.SetCellValue(fb2sc.SALARY_DT);

                    row_2 = sheet.CreateRow(2);
                    cell_2 = row_2.CreateCell(0);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue("發放項目");
                    sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(2, 2, 1, 2));

                    cell_2 = row_2.CreateCell(1);
                    cell_2.CellStyle = style2;
                    cell_2.SetCellValue(fb2sc.PAY_KIND);

                    row_2 = sheet.CreateRow(3);
                    cell_2 = row_2.CreateCell(0);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue("公司別");

                    cell_2 = row_2.CreateCell(1);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue("KZ");

                    cell_2 = row_2.CreateCell(3);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue("列印日期");

                    cell_2 = row_2.CreateCell(4);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue(DateTime.Today.ToString("yyyy/MM/dd"));
                    //sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(3, 3, 4, 5));

                    row_2 = sheet.CreateRow(4);
                    cell_2 = row_2.CreateCell(0);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue("工號");

                    cell_2 = row_2.CreateCell(1);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue("姓名");

                    cell_2 = row_2.CreateCell(2);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue("公司別");

                    cell_2 = row_2.CreateCell(3);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue("廠別");

                    cell_2 = row_2.CreateCell(4);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue("職種區分");

                    cell_2 = row_2.CreateCell(5);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue("職務代號");

                    cell_2 = row_2.CreateCell(6);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue("職務名稱");

                    cell_2 = row_2.CreateCell(7);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue("資格");

                    cell_2 = row_2.CreateCell(8);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue("部名");

                    cell_2 = row_2.CreateCell(9);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue("室名");

                    cell_2 = row_2.CreateCell(10);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue("課名");

                    cell_2 = row_2.CreateCell(11);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue("帳號");

                    cell_2 = row_2.CreateCell(12);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue("應付金額");

                    cell_2 = row_2.CreateCell(13);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue("稅額");

                    cell_2 = row_2.CreateCell(14);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue("補充保費");

                    //增加法扣欄位
                    cell_2 = row_2.CreateCell(15);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue("法扣");

                    cell_2 = row_2.CreateCell(16);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue("薪資積欠");

                    cell_2 = row_2.CreateCell(17);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue("淨額");
                    #endregion

                    DataTable dt0_2 = null;
                    for (int a = 0; a < count; a++)
                    {
                        dt0_2 = null;
                        //發薪狀態='2'(薪資計算)或'3'(關帳)
                        if (salary_data[a].Item1 == "2" || salary_data[a].Item1 == "3")
                        {
                            dt0_2 = fb2sc.getSheet2(salary_data[a].Item2, salary_data[a].Item3, "K");
                        }
                        //發薪狀態='4'(月結)
                        if (salary_data[a].Item1 == "4")
                        {
                            dt0_2 = fb2sc.getSheet2_2(salary_data[a].Item2, salary_data[a].Item3, "K");
                        }
                        if (dt0_2.Rows.Count > 0)
                        {

                            for (int i = 0; i < dt0_2.Rows.Count; i++)
                            {
                                row_2 = sheet.CreateRow(i + 5);
                                cell_2 = row_2.CreateCell(0);
                                cell_2.CellStyle = style2;
                                cell_2.SetCellValue(dt0_2.Rows[i]["EMP_ID"].ToString());

                                cell_2 = row_2.CreateCell(1);
                                cell_2.CellStyle = style2;
                                cell_2.SetCellValue(dt0_2.Rows[i]["EMP_NAME"].ToString());

                                cell_2 = row_2.CreateCell(2);
                                cell_2.CellStyle = style2;
                                cell_2.SetCellValue(dt0_2.Rows[i]["COMPANY_CD"].ToString());

                                cell_2 = row_2.CreateCell(3);
                                cell_2.CellStyle = style2;
                                cell_2.SetCellValue(dt0_2.Rows[i]["PLANT_CD"].ToString());

                                cell_2 = row_2.CreateCell(4);
                                cell_2.CellStyle = style2;
                                cell_2.SetCellValue(dt0_2.Rows[i]["WS_CD"].ToString());

                                cell_2 = row_2.CreateCell(5);
                                cell_2.CellStyle = style2;
                                cell_2.SetCellValue(dt0_2.Rows[i]["PJOB_CD"].ToString());

                                cell_2 = row_2.CreateCell(6);
                                cell_2.CellStyle = style2;
                                cell_2.SetCellValue(dt0_2.Rows[i]["PJOB_DESC"].ToString());

                                cell_2 = row_2.CreateCell(7);
                                cell_2.CellStyle = style2;
                                //cell_2.SetCellValue(dt0_2.Rows[i]["DEPT_NO"].ToString());
                                cell_2.SetCellValue(dt0_2.Rows[i]["LEVEL_CD"].ToString());

                                cell_2 = row_2.CreateCell(8);
                                cell_2.CellStyle = style2;
                                cell_2.SetCellValue(dt0_2.Rows[i]["DEPT_NAME_20"].ToString());

                                cell_2 = row_2.CreateCell(9);
                                cell_2.CellStyle = style2;
                                cell_2.SetCellValue(dt0_2.Rows[i]["DEPT_NAME_30"].ToString());

                                cell_2 = row_2.CreateCell(10);
                                cell_2.CellStyle = style2;
                                cell_2.SetCellValue(dt0_2.Rows[i]["DEPT_NAME_40"].ToString());

                                cell_2 = row_2.CreateCell(11);
                                cell_2.CellStyle = style2;
                                cell_2.SetCellValue(dt0_2.Rows[i]["SALARY_ACCOUNT_NO"].ToString());

                                cell_2 = row_2.CreateCell(12);
                                //cell_2.CellStyle = style2;
                                cell_2.SetCellType(CellType.Numeric);
                                cell_2.CellStyle = _intCellStyle;
                                if (dt0_2.Rows[i]["AMT"].ToString() == "")
                                    cell_2.SetCellValue(0);
                                else
                                {
                                    //cell_2.SetCellValue(String.Format("{0:N0}", int.Parse(dt0_2.Rows[i]["AMT"].ToString())));
                                    cell_2.SetCellValue(Convert.ToDouble(dt0_2.Rows[i]["AMT"]));
                                }
                                cell_2 = row_2.CreateCell(13);
                                cell_2.SetCellType(CellType.Numeric);
                                cell_2.CellStyle = _intCellStyle;
                                if (dt0_2.Rows[i]["TAX"].ToString() == "")
                                    cell_2.SetCellValue(0);
                                else
                                {
                                    cell_2.SetCellValue(Convert.ToDouble(dt0_2.Rows[i]["TAX"]));
                                }
                                cell_2 = row_2.CreateCell(14);
                                cell_2.SetCellType(CellType.Numeric);
                                cell_2.CellStyle = _intCellStyle;
                                if (dt0_2.Rows[i]["INS2_AMT"].ToString() == "")
                                    cell_2.SetCellValue(0);
                                else
                                {
                                    cell_2.SetCellValue(Convert.ToDouble(dt0_2.Rows[i]["INS2_AMT"]));
                                }
                                cell_2 = row_2.CreateCell(15);
                                cell_2.CellStyle = _intCellStyle;
                                if (dt0_2.Rows[i]["AMT2"].ToString() == "")
                                    cell_2.SetCellValue(0);
                                else
                                {
                                    cell_2.SetCellValue(Convert.ToDouble(dt0_2.Rows[i]["AMT2"]));
                                }
                                cell_2 = row_2.CreateCell(16);
                                cell_2.SetCellType(CellType.Numeric);
                                cell_2.CellStyle = _intCellStyle;
                                if (dt0_2.Rows[i]["REPAY"].ToString() == "")
                                    cell_2.SetCellValue(0);
                                else
                                {
                                    cell_2.SetCellValue(Convert.ToDouble(dt0_2.Rows[i]["REPAY"]));
                                }

                                cell_2 = row_2.CreateCell(17);
                                cell_2.SetCellType(CellType.Numeric);
                                cell_2.CellStyle = _intCellStyle;
                                if (dt0_2.Rows[i]["total"].ToString() == "")
                                    cell_2.SetCellValue(0);
                                else
                                {
                                    cell_2.SetCellValue(Convert.ToDouble(dt0_2.Rows[i]["total"]));
                                }
                            }
                        }
                    }

                }
                #endregion

            }

            #endregion  
            
            #region 非國瑞的改迴圈
            //20201026 非國瑞的改迴圈
            List<Tuple<string, string>> compList = new List<Tuple<string, string>>();
            //全選
            if (fb2sc.COMPANY_CD == "-1")
            {
                DataTable dt = fb2sc.getCOMPANY_CD();
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        if (dt.Rows[i]["COMPANY_CD"].ToString().ToUpper() != "K")
                            compList.Add(new Tuple<string, string>(dt.Rows[i]["COMPANY_CD"].ToString().ToUpper()
                                                               , dt.Rows[i]["COMPANY_SNAME"].ToString()));
                    }
                }
            }
            else if (fb2sc.COMPANY_CD != "-1" && fb2sc.COMPANY_CD.ToUpper() != "K")
            {
                //單選
                compList.Add(new Tuple<string, string>(fb2sc.COMPANY_CD.ToUpper()
                                                      , fb2sc.getCOMPANY_SNAME(fb2sc.COMPANY_CD.ToUpper())
                                                      ));
            }
            foreach (var item in compList)
            {
                string company_cd = item.Item1;
                string company_sname = item.Item2;

                if (type == "xls")
                {
                    sheet = (HSSFSheet)workbook.CreateSheet(company_sname);
                    style1 = (HSSFCellStyle)workbook.CreateCellStyle();
                }
                else
                {
                    sheet = workbook.CreateSheet(company_sname);
                    style1 = (XSSFCellStyle)workbook.CreateCellStyle();
                }

                IFont font1 = workbook.CreateFont();
                font1.FontName = "新細明體";
                font1.FontHeightInPoints = 12;
                style1.SetFont(font1);

                style2 = workbook.CreateCellStyle();
                style2.SetFont(font1);

                #region 月薪資類
                if (fb2sc.SALARY_TYPE.Substring(0, 1) == "A")
                {
                    #region 月薪資類表頭
                    IRow row = sheet.CreateRow(0);
                    ICell cell;
                    cell = row.CreateCell(0);
                    cell.CellStyle = style1;
                    cell.SetCellValue("發薪類別");
                    sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 1, 2));

                    cell = row.CreateCell(1);
                    cell.CellStyle = style1;
                    cell.SetCellValue("月薪資類");

                    row = sheet.CreateRow(1);
                    cell = row.CreateCell(0);
                    cell.CellStyle = style1;
                    cell.SetCellValue("發薪日期");

                    cell = row.CreateCell(1);
                    cell.CellStyle = style1;
                    cell.SetCellValue(fb2sc.SALARY_DT);

                    row = sheet.CreateRow(2);
                    cell = row.CreateCell(0);
                    cell.CellStyle = style1;
                    cell.SetCellValue("公司別");

                    cell = row.CreateCell(1);
                    cell.CellStyle = style1;
                    cell.SetCellValue(company_sname);

                    cell = row.CreateCell(3);
                    cell.CellStyle = style1;
                    cell.SetCellValue("列印日期");

                    cell = row.CreateCell(4);
                    cell.CellStyle = style1;
                    cell.SetCellValue(DateTime.Today.ToString("yyyy/MM/dd"));
                    sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(3, 3, 4, 5));

                    IRow row4 = sheet.CreateRow(3);
                    cell = row4.CreateCell(0);
                    cell.CellStyle = style1;
                    cell.SetCellValue("基本資料");

                    IRow row5 = sheet.CreateRow(4);
                    cell = row5.CreateCell(0);
                    cell.CellStyle = style1;
                    cell.SetCellValue("工號");

                    cell = row5.CreateCell(1);
                    cell.CellStyle = style1;
                    cell.SetCellValue("姓名");

                    cell = row5.CreateCell(2);
                    cell.CellStyle = style1;
                    cell.SetCellValue("月終狀態");

                    cell = row5.CreateCell(3);
                    cell.CellStyle = style1;
                    cell.SetCellValue("本月在職天數");


                    int row5count = 4;
                    for (int i = 1; i <= 8; i++)
                    {
                        if (i == 5)
                            changeStyleIndex = row5count;
                        DataTable dt = fb2sc.getTITLE(i.ToString());
                        if (dt.Rows.Count > 0)
                        {
                            cell = row4.CreateCell(row5count);
                            cell.CellStyle = style1;
                            cell.SetCellValue(dt.Rows[0]["TITLE_TYPE_NAME"].ToString());
                            for (int k = 0; k < dt.Rows.Count; k++)
                            {
                                cell = row5.CreateCell(row5count);
                                cell.CellStyle = style2;
                                cell.SetCellValue(dt.Rows[k]["TITLE_NAME"].ToString());
                                sheet.AutoSizeColumn(row5count);
                                row5count++;
                            }
                        }
                    }

                    #endregion

                    int contentStartRow = 0;
                    int maxAMT = fb2sc.getmaxAMT();
                    for (int a = 0; a < count; a++) //count:選取筆數
                    {
                        DataTable dt0 = fb2sc.getContent(salary_data[a].Item2, salary_data[a].Item3, maxAMT, company_cd);
                        if (dt0.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt0.Rows.Count; i++)
                            {
                                int cellcount = 0;
                                row = sheet.CreateRow(contentStartRow + 5);
                                contentStartRow++;
                                cell = row.CreateCell(cellcount);
                                cell.CellStyle = style2;
                                cell.SetCellValue(dt0.Rows[i]["EMP_ID"].ToString());
                                cellcount++;

                                cell = row.CreateCell(cellcount);
                                cell.CellStyle = style2;
                                cell.SetCellValue(dt0.Rows[i]["EMP_NAME"].ToString());
                                cellcount++;

                                cell = row.CreateCell(cellcount);
                                cell.CellStyle = style2;
                                cell.SetCellValue(dt0.Rows[i]["EMP_CHG_CD"].ToString());
                                cellcount++;

                                cell = row.CreateCell(cellcount);
                                cell.SetCellValue(Convert.ToDouble(dt0.Rows[i]["WORK_DAYS_MONTH"]));
                                cell.SetCellType(CellType.Numeric);
                                cell.CellStyle = _intCellStyle;
                                cellcount++;

                                for (int k = 1; k <= maxAMT; k++)
                                {
                                    cell = row.CreateCell(cellcount);
                                    cell.SetCellType(CellType.Numeric);
                                    cell.SetCellValue(Convert.ToDouble(dt0.Rows[i]["AMOUNT_" + k.ToString().PadLeft(3, '0')]));
                                    if (cellcount >= changeStyleIndex)
                                        cell.CellStyle = _doubleCellStyle;
                                    else
                                        cell.CellStyle = _intCellStyle;
                                    cellcount++;
                                }
                            }
                        }
                    }
                }
                #endregion

                #region 其他類薪資
                else
                {
                    font1.FontName = "新細明體";
                    font1.FontHeightInPoints = 12;
                    style1.SetFont(font1);

                    style2 = workbook.CreateCellStyle();
                    style2.SetFont(font1);
                    #region 其他類薪資表頭
                    IRow row_2 = sheet.CreateRow(0);
                    ICell cell_2;
                    cell_2 = row_2.CreateCell(0);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue("發薪類別");
                    sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 1, 2));

                    cell_2 = row_2.CreateCell(1);
                    cell_2.CellStyle = style2;
                    cell_2.SetCellValue(fb2sc.SALARY_TYPE);

                    row_2 = sheet.CreateRow(1);
                    cell_2 = row_2.CreateCell(0);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue("發薪日期");

                    cell_2 = row_2.CreateCell(1);
                    cell_2.CellStyle = style2;
                    cell_2.SetCellValue(fb2sc.SALARY_DT);

                    row_2 = sheet.CreateRow(2);
                    cell_2 = row_2.CreateCell(0);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue("發放項目");
                    sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(2, 2, 1, 2));

                    cell_2 = row_2.CreateCell(1);
                    cell_2.CellStyle = style2;
                    cell_2.SetCellValue(fb2sc.PAY_KIND);

                    row_2 = sheet.CreateRow(3);
                    cell_2 = row_2.CreateCell(0);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue("公司別");

                    cell_2 = row_2.CreateCell(1);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue(company_sname);

                    cell_2 = row_2.CreateCell(3);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue("列印日期");

                    cell_2 = row_2.CreateCell(4);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue(DateTime.Today.ToString("yyyy/MM/dd"));
                    //sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(3, 3, 4, 5));

                    row_2 = sheet.CreateRow(4);
                    cell_2 = row_2.CreateCell(0);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue("工號");

                    cell_2 = row_2.CreateCell(1);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue("姓名");

                    cell_2 = row_2.CreateCell(2);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue("公司別");

                    cell_2 = row_2.CreateCell(3);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue("廠別");

                    cell_2 = row_2.CreateCell(4);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue("職種區分");

                    cell_2 = row_2.CreateCell(5);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue("職務代號");

                    cell_2 = row_2.CreateCell(6);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue("職務名稱");

                    cell_2 = row_2.CreateCell(7);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue("資格");

                    cell_2 = row_2.CreateCell(8);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue("部名");

                    cell_2 = row_2.CreateCell(9);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue("室名");

                    cell_2 = row_2.CreateCell(10);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue("課名");

                    cell_2 = row_2.CreateCell(11);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue("帳號");

                    cell_2 = row_2.CreateCell(12);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue("應付金額");

                    cell_2 = row_2.CreateCell(13);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue("稅額");

                    cell_2 = row_2.CreateCell(14);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue("補充保費");

                    //增加法扣欄位
                    cell_2 = row_2.CreateCell(15);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue("法扣");

                    cell_2 = row_2.CreateCell(16);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue("薪資積欠");

                    cell_2 = row_2.CreateCell(17);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue("淨額");
                    #endregion

                    DataTable dt0_2 = null;
                    for (int a = 0; a < count; a++)
                    {
                        dt0_2 = null;
                        //發薪狀態='2'(薪資計算)或'3'(關帳)
                        if (salary_data[a].Item1 == "2" || salary_data[a].Item1 == "3")
                        {
                            dt0_2 = fb2sc.getSheet2(salary_data[a].Item2, salary_data[a].Item3, company_cd);
                        }
                        //發薪狀態='4'(月結)
                        if (salary_data[a].Item1 == "4")
                        {
                            dt0_2 = fb2sc.getSheet2_2(salary_data[a].Item2, salary_data[a].Item3, company_cd);
                        }
                        if (dt0_2.Rows.Count > 0)
                        {

                            for (int i = 0; i < dt0_2.Rows.Count; i++)
                            {
                                row_2 = sheet.CreateRow(i + 5);
                                cell_2 = row_2.CreateCell(0);
                                cell_2.CellStyle = style2;
                                cell_2.SetCellValue(dt0_2.Rows[i]["EMP_ID"].ToString());

                                cell_2 = row_2.CreateCell(1);
                                cell_2.CellStyle = style2;
                                cell_2.SetCellValue(dt0_2.Rows[i]["EMP_NAME"].ToString());

                                cell_2 = row_2.CreateCell(2);
                                cell_2.CellStyle = style2;
                                cell_2.SetCellValue(dt0_2.Rows[i]["COMPANY_CD"].ToString());

                                cell_2 = row_2.CreateCell(3);
                                cell_2.CellStyle = style2;
                                cell_2.SetCellValue(dt0_2.Rows[i]["PLANT_CD"].ToString());

                                cell_2 = row_2.CreateCell(4);
                                cell_2.CellStyle = style2;
                                cell_2.SetCellValue(dt0_2.Rows[i]["WS_CD"].ToString());

                                cell_2 = row_2.CreateCell(5);
                                cell_2.CellStyle = style2;
                                cell_2.SetCellValue(dt0_2.Rows[i]["PJOB_CD"].ToString());

                                cell_2 = row_2.CreateCell(6);
                                cell_2.CellStyle = style2;
                                cell_2.SetCellValue(dt0_2.Rows[i]["PJOB_DESC"].ToString());

                                cell_2 = row_2.CreateCell(7);
                                cell_2.CellStyle = style2;
                                cell_2.SetCellValue(dt0_2.Rows[i]["DEPT_NO"].ToString());

                                cell_2 = row_2.CreateCell(8);
                                cell_2.CellStyle = style2;
                                cell_2.SetCellValue(dt0_2.Rows[i]["DEPT_NAME_20"].ToString());

                                cell_2 = row_2.CreateCell(9);
                                cell_2.CellStyle = style2;
                                cell_2.SetCellValue(dt0_2.Rows[i]["DEPT_NAME_30"].ToString());

                                cell_2 = row_2.CreateCell(10);
                                cell_2.CellStyle = style2;
                                cell_2.SetCellValue(dt0_2.Rows[i]["DEPT_NAME_40"].ToString());

                                cell_2 = row_2.CreateCell(11);
                                cell_2.CellStyle = style2;
                                cell_2.SetCellValue(dt0_2.Rows[i]["SALARY_ACCOUNT_NO"].ToString());

                                cell_2 = row_2.CreateCell(12);
                                cell_2.SetCellType(CellType.Numeric);
                                //cell_2.CellStyle = style2;
                                cell_2.CellStyle = _intCellStyle;
                                if (dt0_2.Rows[i]["AMT"].ToString() == "")
                                    cell_2.SetCellValue(0);
                                else
                                {
                                    cell_2.SetCellValue(Convert.ToDouble(dt0_2.Rows[i]["AMT"]));
                                }
                                cell_2 = row_2.CreateCell(13);
                                cell_2.SetCellType(CellType.Numeric);
                                cell_2.CellStyle = _intCellStyle;
                                if (dt0_2.Rows[i]["TAX"].ToString() == "")
                                    cell_2.SetCellValue(0);
                                else
                                {
                                    cell_2.SetCellValue(Convert.ToDouble(dt0_2.Rows[i]["TAX"]));
                                }
                                cell_2 = row_2.CreateCell(14);
                                cell_2.SetCellType(CellType.Numeric);
                                cell_2.CellStyle = _intCellStyle;
                                if (dt0_2.Rows[i]["INS2_AMT"].ToString() == "")
                                    cell_2.SetCellValue(0);
                                else
                                {
                                    cell_2.SetCellValue(Convert.ToDouble(dt0_2.Rows[i]["INS2_AMT"]));
                                }
                                cell_2 = row_2.CreateCell(15);
                                cell_2.SetCellType(CellType.Numeric);
                                cell_2.CellStyle = _intCellStyle;
                                if (dt0_2.Rows[i]["AMT2"].ToString() == "")
                                    cell_2.SetCellValue(0);
                                else
                                {
                                    cell_2.SetCellValue(Convert.ToDouble(dt0_2.Rows[i]["AMT2"]));
                                }
                                cell_2 = row_2.CreateCell(16);
                                cell_2.SetCellType(CellType.Numeric);
                                cell_2.CellStyle = _intCellStyle;
                                if (dt0_2.Rows[i]["REPAY"].ToString() == "")
                                    cell_2.SetCellValue(0);
                                else
                                {
                                    cell_2.SetCellValue(Convert.ToDouble(dt0_2.Rows[i]["REPAY"]));
                                }

                                cell_2 = row_2.CreateCell(17);
                                cell_2.SetCellType(CellType.Numeric);
                                cell_2.CellStyle = _intCellStyle;
                                //才庫的淨額 不用扣 補充保費 和 法扣 
                                if (dt0_2.Rows[i]["total"].ToString() == "")
                                    cell_2.SetCellValue(0);
                                else
                                {
                                    cell_2.SetCellValue(Convert.ToDouble(dt0_2.Rows[i]["total"]));
                                }
                            }
                        }
                    }

                }
                #endregion


            }




            /*
            if (fb2sc.COMPANY_CD == "-1" || fb2sc.COMPANY_CD.ToUpper() != "K" )
            {
                string company_sname = fb2sc.getCOMPANY_SNAME(fb2sc.COMPANY_CD.ToUpper());
                if (type == "xls")
                {
                    sheet = (HSSFSheet)workbook.CreateSheet(company_sname);
                    style1 = (HSSFCellStyle)workbook.CreateCellStyle();
                }
                else
                {
                    sheet = workbook.CreateSheet(company_sname);
                    style1 = (XSSFCellStyle)workbook.CreateCellStyle();
                }

                IFont font1 = workbook.CreateFont();
                font1.FontName = "新細明體";
                font1.FontHeightInPoints = 12;
                style1.SetFont(font1);

                style2 = workbook.CreateCellStyle();
                style2.SetFont(font1);

                #region 月薪資類
                if (fb2sc.SALARY_TYPE.Substring(0, 1) == "A")
                {
                    #region 月薪資類表頭
                    IRow row = sheet.CreateRow(0);
                    ICell cell;
                    cell = row.CreateCell(0);
                    cell.CellStyle = style1;
                    cell.SetCellValue("發薪類別");
                    sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 1, 2));

                    cell = row.CreateCell(1);
                    cell.CellStyle = style1;
                    cell.SetCellValue("月薪資類");

                    row = sheet.CreateRow(1);
                    cell = row.CreateCell(0);
                    cell.CellStyle = style1;
                    cell.SetCellValue("發薪日期");

                    cell = row.CreateCell(1);
                    cell.CellStyle = style1;
                    cell.SetCellValue(fb2sc.SALARY_DT);

                    row = sheet.CreateRow(2);
                    cell = row.CreateCell(0);
                    cell.CellStyle = style1;
                    cell.SetCellValue("公司別");

                    cell = row.CreateCell(1);
                    cell.CellStyle = style1;
                    cell.SetCellValue(company_sname);

                    cell = row.CreateCell(3);
                    cell.CellStyle = style1;
                    cell.SetCellValue("列印日期");

                    cell = row.CreateCell(4);
                    cell.CellStyle = style1;
                    cell.SetCellValue(DateTime.Today.ToString("yyyy/MM/dd"));
                    sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(3, 3, 4, 5));

                    IRow row4 = sheet.CreateRow(3);
                    cell = row4.CreateCell(0);
                    cell.CellStyle = style1;
                    cell.SetCellValue("基本資料");

                    IRow row5 = sheet.CreateRow(4);
                    cell = row5.CreateCell(0);
                    cell.CellStyle = style1;
                    cell.SetCellValue("工號");

                    cell = row5.CreateCell(1);
                    cell.CellStyle = style1;
                    cell.SetCellValue("姓名");

                    cell = row5.CreateCell(2);
                    cell.CellStyle = style1;
                    cell.SetCellValue("月終狀態");

                    cell = row5.CreateCell(3);
                    cell.CellStyle = style1;
                    cell.SetCellValue("本月在職天數");


                    int row5count = 4;
                    for (int i = 1; i <= 8; i++)
                    {
                        if (i == 5)
                            changeStyleIndex = row5count;
                        DataTable dt = fb2sc.getTITLE(i.ToString());
                        if (dt.Rows.Count > 0)
                        {
                            cell = row4.CreateCell(row5count);
                            cell.CellStyle = style1;
                            cell.SetCellValue(dt.Rows[0]["TITLE_TYPE_NAME"].ToString());
                            for (int k = 0; k < dt.Rows.Count; k++)
                            {
                                cell = row5.CreateCell(row5count);
                                cell.CellStyle = style2;
                                cell.SetCellValue(dt.Rows[k]["TITLE_NAME"].ToString());
                                sheet.AutoSizeColumn(row5count);
                                row5count++;
                            }
                        }
                    }

                    #endregion

                    int contentStartRow = 0;
                    int maxAMT = fb2sc.getmaxAMT();
                    for (int a = 0; a < count; a++) //count:選取筆數
                    {
                        DataTable dt0 = fb2sc.getContent(salary_data[a].Item2, salary_data[a].Item3, maxAMT, fb2sc.COMPANY_CD.ToUpper());
                        if (dt0.Rows.Count > 0)
                        {
                            for (int i = 0; i < dt0.Rows.Count; i++)
                            {
                                int cellcount = 0;
                                row = sheet.CreateRow(contentStartRow + 5);
                                contentStartRow++;
                                cell = row.CreateCell(cellcount);
                                cell.CellStyle = style2;
                                cell.SetCellValue(dt0.Rows[i]["EMP_ID"].ToString());
                                cellcount++;

                                cell = row.CreateCell(cellcount);
                                cell.CellStyle = style2;
                                cell.SetCellValue(dt0.Rows[i]["EMP_NAME"].ToString());
                                cellcount++;

                                cell = row.CreateCell(cellcount);
                                cell.CellStyle = style2;
                                cell.SetCellValue(dt0.Rows[i]["EMP_CHG_CD"].ToString());
                                cellcount++;

                                cell = row.CreateCell(cellcount);
                                cell.SetCellValue(Convert.ToDouble(dt0.Rows[i]["WORK_DAYS_MONTH"]));
                                cell.SetCellType(CellType.Numeric);
                                cell.CellStyle = _intCellStyle;
                                cellcount++;

                                for (int k = 1; k <= maxAMT; k++)
                                {
                                    cell = row.CreateCell(cellcount);
                                    cell.SetCellType(CellType.Numeric);
                                    cell.SetCellValue(Convert.ToDouble(dt0.Rows[i]["AMOUNT_" + k.ToString().PadLeft(3, '0')]));
                                    if (cellcount >= changeStyleIndex)
                                        cell.CellStyle = _doubleCellStyle;
                                    else
                                        cell.CellStyle = _intCellStyle;
                                    cellcount++;
                                }
                            }
                        }
                    }
                }
                #endregion

                #region 其他類薪資
                else
                {
                    font1.FontName = "新細明體";
                    font1.FontHeightInPoints = 12;
                    style1.SetFont(font1);

                    style2 = workbook.CreateCellStyle();
                    style2.SetFont(font1);
                    #region 其他類薪資表頭
                    IRow row_2 = sheet.CreateRow(0);
                    ICell cell_2;
                    cell_2 = row_2.CreateCell(0);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue("發薪類別");
                    sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(0, 0, 1, 2));

                    cell_2 = row_2.CreateCell(1);
                    cell_2.CellStyle = style2;
                    cell_2.SetCellValue(fb2sc.SALARY_TYPE);

                    row_2 = sheet.CreateRow(1);
                    cell_2 = row_2.CreateCell(0);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue("發薪日期");

                    cell_2 = row_2.CreateCell(1);
                    cell_2.CellStyle = style2;
                    cell_2.SetCellValue(fb2sc.SALARY_DT);

                    row_2 = sheet.CreateRow(2);
                    cell_2 = row_2.CreateCell(0);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue("發放項目");
                    sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(2, 2, 1, 2));

                    cell_2 = row_2.CreateCell(1);
                    cell_2.CellStyle = style2;
                    cell_2.SetCellValue(fb2sc.PAY_KIND);

                    row_2 = sheet.CreateRow(3);
                    cell_2 = row_2.CreateCell(0);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue("公司別");

                    cell_2 = row_2.CreateCell(1);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue(company_sname);

                    cell_2 = row_2.CreateCell(3);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue("列印日期");

                    cell_2 = row_2.CreateCell(4);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue(DateTime.Today.ToString("yyyy/MM/dd"));
                    //sheet.AddMergedRegion(new NPOI.SS.Util.CellRangeAddress(3, 3, 4, 5));

                    row_2 = sheet.CreateRow(4);
                    cell_2 = row_2.CreateCell(0);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue("工號");

                    cell_2 = row_2.CreateCell(1);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue("姓名");

                    cell_2 = row_2.CreateCell(2);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue("公司別");

                    cell_2 = row_2.CreateCell(3);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue("廠別");

                    cell_2 = row_2.CreateCell(4);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue("職種區分");

                    cell_2 = row_2.CreateCell(5);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue("職務代號");

                    cell_2 = row_2.CreateCell(6);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue("職務名稱");

                    cell_2 = row_2.CreateCell(7);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue("資格");

                    cell_2 = row_2.CreateCell(8);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue("部名");

                    cell_2 = row_2.CreateCell(9);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue("室名");

                    cell_2 = row_2.CreateCell(10);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue("課名");

                    cell_2 = row_2.CreateCell(11);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue("帳號");

                    cell_2 = row_2.CreateCell(12);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue("應付金額");

                    cell_2 = row_2.CreateCell(13);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue("稅額");

                    cell_2 = row_2.CreateCell(14);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue("補充保費");

                    //增加法扣欄位
                    cell_2 = row_2.CreateCell(15);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue("法扣");

                    cell_2 = row_2.CreateCell(16);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue("薪資積欠");

                    cell_2 = row_2.CreateCell(17);
                    cell_2.CellStyle = style1;
                    cell_2.SetCellValue("淨額");
                    #endregion

                    DataTable dt0_2 = null;
                    for (int a = 0; a < count; a++)
                    {
                        dt0_2 = null;
                        //發薪狀態='2'(薪資計算)或'3'(關帳)
                        if (salary_data[a].Item1 == "2" || salary_data[a].Item1 == "3")
                        {
                            dt0_2 = fb2sc.getSheet2(salary_data[a].Item2, salary_data[a].Item3, fb2sc.COMPANY_CD.ToUpper());
                        }
                        //發薪狀態='4'(月結)
                        if (salary_data[a].Item1 == "4")
                        {
                            dt0_2 = fb2sc.getSheet2_2(salary_data[a].Item2, salary_data[a].Item3, fb2sc.COMPANY_CD.ToUpper());
                        }
                        if (dt0_2.Rows.Count > 0)
                        {

                            for (int i = 0; i < dt0_2.Rows.Count; i++)
                            {
                                row_2 = sheet.CreateRow(i + 5);
                                cell_2 = row_2.CreateCell(0);
                                cell_2.CellStyle = style2;
                                cell_2.SetCellValue(dt0_2.Rows[i]["EMP_ID"].ToString());

                                cell_2 = row_2.CreateCell(1);
                                cell_2.CellStyle = style2;
                                cell_2.SetCellValue(dt0_2.Rows[i]["EMP_NAME"].ToString());

                                cell_2 = row_2.CreateCell(2);
                                cell_2.CellStyle = style2;
                                cell_2.SetCellValue(dt0_2.Rows[i]["COMPANY_CD"].ToString());

                                cell_2 = row_2.CreateCell(3);
                                cell_2.CellStyle = style2;
                                cell_2.SetCellValue(dt0_2.Rows[i]["PLANT_CD"].ToString());

                                cell_2 = row_2.CreateCell(4);
                                cell_2.CellStyle = style2;
                                cell_2.SetCellValue(dt0_2.Rows[i]["WS_CD"].ToString());

                                cell_2 = row_2.CreateCell(5);
                                cell_2.CellStyle = style2;
                                cell_2.SetCellValue(dt0_2.Rows[i]["PJOB_CD"].ToString());

                                cell_2 = row_2.CreateCell(6);
                                cell_2.CellStyle = style2;
                                cell_2.SetCellValue(dt0_2.Rows[i]["PJOB_DESC"].ToString());

                                cell_2 = row_2.CreateCell(7);
                                cell_2.CellStyle = style2;
                                cell_2.SetCellValue(dt0_2.Rows[i]["DEPT_NO"].ToString());

                                cell_2 = row_2.CreateCell(8);
                                cell_2.CellStyle = style2;
                                cell_2.SetCellValue(dt0_2.Rows[i]["DEPT_NAME_20"].ToString());

                                cell_2 = row_2.CreateCell(9);
                                cell_2.CellStyle = style2;
                                cell_2.SetCellValue(dt0_2.Rows[i]["DEPT_NAME_30"].ToString());

                                cell_2 = row_2.CreateCell(10);
                                cell_2.CellStyle = style2;
                                cell_2.SetCellValue(dt0_2.Rows[i]["DEPT_NAME_40"].ToString());

                                cell_2 = row_2.CreateCell(11);
                                cell_2.CellStyle = style2;
                                cell_2.SetCellValue(dt0_2.Rows[i]["SALARY_ACCOUNT_NO"].ToString());

                                cell_2 = row_2.CreateCell(12);
                                cell_2.SetCellType(CellType.Numeric);
                                //cell_2.CellStyle = style2;
                                cell_2.CellStyle = _intCellStyle;
                                if (dt0_2.Rows[i]["AMT"].ToString() == "")
                                    cell_2.SetCellValue(0);
                                else
                                {
                                    cell_2.SetCellValue(Convert.ToDouble(dt0_2.Rows[i]["AMT"]));
                                }
                                cell_2 = row_2.CreateCell(13);
                                cell_2.SetCellType(CellType.Numeric);
                                cell_2.CellStyle = _intCellStyle;
                                if (dt0_2.Rows[i]["TAX"].ToString() == "")
                                    cell_2.SetCellValue(0);
                                else
                                {
                                    cell_2.SetCellValue(Convert.ToDouble(dt0_2.Rows[i]["TAX"]));
                                }
                                cell_2 = row_2.CreateCell(14);
                                cell_2.SetCellType(CellType.Numeric);
                                cell_2.CellStyle = _intCellStyle;
                                if (dt0_2.Rows[i]["INS2_AMT"].ToString() == "")
                                    cell_2.SetCellValue(0);
                                else
                                {
                                    cell_2.SetCellValue(Convert.ToDouble(dt0_2.Rows[i]["INS2_AMT"]));
                                }
                                cell_2 = row_2.CreateCell(15);
                                cell_2.SetCellType(CellType.Numeric);
                                cell_2.CellStyle = _intCellStyle;
                                if (dt0_2.Rows[i]["AMT2"].ToString() == "")
                                    cell_2.SetCellValue(0);
                                else
                                {
                                    cell_2.SetCellValue(Convert.ToDouble(dt0_2.Rows[i]["AMT2"]));
                                }
                                cell_2 = row_2.CreateCell(16);
                                cell_2.SetCellType(CellType.Numeric);
                                cell_2.CellStyle = _intCellStyle;
                                if (dt0_2.Rows[i]["REPAY"].ToString() == "")
                                    cell_2.SetCellValue(0);
                                else
                                {
                                    cell_2.SetCellValue(Convert.ToDouble(dt0_2.Rows[i]["REPAY"]));
                                }

                                cell_2 = row_2.CreateCell(17);
                                cell_2.SetCellType(CellType.Numeric);
                                cell_2.CellStyle = _intCellStyle;
                                //才庫的淨額 不用扣 補充保費 和 法扣 
                                if (dt0_2.Rows[i]["total"].ToString() == "")
                                    cell_2.SetCellValue(0);
                                else
                                {
                                    cell_2.SetCellValue(Convert.ToDouble(dt0_2.Rows[i]["total"]));
                                }
                            }
                        }
                    }
                    
                }
                #endregion

            }
            */

            #endregion
            
            

            return workbook;
        }
        catch
        {
            throw;
        }
        finally
        {
            workbook.Clear();
            //fs.Close();
            sheet = null;
            workbook = null;
        }
    }
    #region 改成SP
    private void getBody(ICellStyle stringRightStyle, ICell cell, string[] checknull, IRow row, int i, int cellcount, int totaldtcount, DataTable dt0, DataTable dt, DataTable dt2, DataTable dt3,
                                                       DataTable dt4, DataTable dt5, DataTable dt6, DataTable dt7, DataTable dt8, DataTable dt9)
    {
        //ICellStyle stringRightStyle = this.setCellStyle(workbook, "right", false);
        for (int kk = 0; kk < dt.Rows.Count; kk++)
        {
            if (dt.Rows[kk]["SALARY_ID"].ToString() == dt0.Rows[i]["SALARY_ID_1"].ToString())
            {
                cell = row.CreateCell(cellcount);
                cell.CellStyle = stringRightStyle;
                cell.SetCellValue(String.Format("{0:N0}", int.Parse(dt0.Rows[i]["AMOUNT_1"].ToString())));
                //cell.SetCellValue(dt0.Rows[i]["AMOUNT_1"].ToString());
            }
            cellcount++;
        }
        for (int kk = 0; kk < dt2.Rows.Count; kk++)
        {
            if (dt2.Rows[kk]["SALARY_ID"].ToString() == dt0.Rows[i]["SALARY_ID_2"].ToString())
            {
                cell = row.CreateCell(cellcount);
                cell.CellStyle = stringRightStyle;
                cell.SetCellValue(String.Format("{0:N0}", int.Parse(dt0.Rows[i]["AMOUNT_2"].ToString())));
                //cell.SetCellValue(dt0.Rows[i]["AMOUNT_2"].ToString());
            }
            cellcount++;
        }
        for (int kk = 0; kk < dt3.Rows.Count; kk++)
        {
            if (dt3.Rows[kk]["SALARY_ID"].ToString() == dt0.Rows[i]["SALARY_ID_3"].ToString())
            {
                cell = row.CreateCell(cellcount);
                cell.CellStyle = stringRightStyle;
                cell.SetCellValue(String.Format("{0:N0}", int.Parse(dt0.Rows[i]["AMOUNT_3"].ToString())));
                //cell.SetCellValue(dt0.Rows[i]["AMOUNT_3"].ToString());
            }
            cellcount++;
        }
        for (int kk = 0; kk < dt4.Rows.Count; kk++)
        {
            if (dt4.Rows[kk]["SALARY_ID"].ToString() == dt0.Rows[i]["SALARY_ID_4"].ToString())
            {
                cell = row.CreateCell(cellcount);
                cell.CellStyle = stringRightStyle;
                cell.SetCellValue(String.Format("{0:N0}", int.Parse(dt0.Rows[i]["AMOUNT_4"].ToString())));
                //cell.SetCellValue(dt0.Rows[i]["AMOUNT_4"].ToString());
            }
            cellcount++;
        }
        for (int kk = 0; kk < dt5.Rows.Count; kk++)
        {
            if (dt5.Rows[kk]["SUB_CD"].ToString() == dt0.Rows[i]["OVERTIME_PAY_TYPE"].ToString())
            {
                cell = row.CreateCell(cellcount);
                cell.SetCellValue(dt0.Rows[i]["TOTAL_HOURS_1"].ToString());
            }
            cellcount++;
        }
        for (int kk = 0; kk < dt6.Rows.Count; kk++)
        {
            if (dt6.Rows[kk]["SUB_CD"].ToString() == dt0.Rows[i]["SHIFT_TIME_CD"].ToString())
            {
                cell = row.CreateCell(cellcount);
                cell.SetCellValue(dt0.Rows[i]["SHIFT_DAYS"].ToString());
            }
            cellcount++;
        }
        for (int kk = 0; kk < dt7.Rows.Count; kk++)
        {
            if (dt7.Rows[kk]["ENV_ALLOWANCE_VALUE"].ToString() == dt0.Rows[i]["ENV_ALLOWANCE_VALUE"].ToString())
            {
                cell = row.CreateCell(cellcount);
                cell.SetCellValue(dt0.Rows[i]["SAPPLY_HOUR"].ToString());
            }
            cellcount++;
        }
        for (int kk = 0; kk < dt8.Rows.Count; kk++)
        {
            if (dt8.Rows[kk]["SUB_LEAVE_CD"].ToString() == dt0.Rows[i]["SUB_LEAVE_CD"].ToString())
            {
                cell = row.CreateCell(cellcount);
                cell.SetCellValue(dt0.Rows[i]["TOTAL_HOURS_2"].ToString());
            }
            cellcount++;
        }
        for (int kk = 0; kk < dt9.Rows.Count; kk++)
        {
            if (dt9.Rows[kk]["SUB_CD"].ToString() == dt0.Rows[i]["LEAVE_ALLOWANCE_TYPE"].ToString())
            {
                cell = row.CreateCell(cellcount);
                cell.SetCellValue(dt0.Rows[i]["TOTAL_HOURS_3"].ToString());
            }
            cellcount++;
        }
    }
    #endregion



    /// <summary>
    /// 設定資料的格式
    /// </summary>
    /// <param name="workbook"></param>
    /// <param name="align">文字位置</param>
    /// <param name="isBorder">是否要有邊框</param>
    /// <param name="color">背景顏色設定(10:紅,13:黃,14:pink.... )</param>
    /// <returns></returns>
    private ICellStyle setCellStyle(IWorkbook workbook, string align, bool isBorder, int colorCD, bool SetCenter, string showBorder)
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

        if (showBorder == "left")
            style.BorderLeft = BorderStyle.Thin;
        if (showBorder == "right")
            style.BorderRight = BorderStyle.Thin;
        if (showBorder == "first")
        {
            style.BorderLeft = BorderStyle.Thin;
            style.BorderTop = BorderStyle.Thin;
        }
        //文字位置 (預設靠左)
        if (align.ToLower() == "center")
        {
            //style.VerticalAlignment = VerticalAlignment.Center;
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
        else
            style.FillForegroundColor = HSSFColor.White.Index;

        if (SetCenter)
        {
            style.FillPattern = NPOI.SS.UserModel.FillPattern.SolidForeground;
            style.VerticalAlignment = NPOI.SS.UserModel.VerticalAlignment.Center;
            style.Alignment = NPOI.SS.UserModel.HorizontalAlignment.Center;
        }
        return style;
    }


}