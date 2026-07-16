using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;
using System.Drawing;
using System.IO;
using NPOI.SS.Util;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.XSSF.UserModel;


/// <summary>
/// CFB2DI0600BO 的摘要描述
/// </summary>
public class CFB2DI0600BO : BaseService
{
    public CFB2DI0600BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public DataTable getOVERTIME_CD(string is_used)
    {
        try
        {
            CFB2DI0600DAO wfb2di = new CFB2DI0600DAO();
            return wfb2di.getOVERTIME_CD(is_used);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getOVERTIME_CTL_CD(string emp_id)
    {
        try
        {
            CFB2DI0600DAO wfb2di = new CFB2DI0600DAO();
            wfb2di.EMP_ID = emp_id;
            return wfb2di.getOVERTIME_CTL_CD();
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getCLOCK_RECORDS(string emp_id, string apply_overtime_dt)
    {
        try
        {
            CFB2DI0600DAO wfb2di = new CFB2DI0600DAO();
            wfb2di.EMP_ID = emp_id;
            wfb2di.APPLY_OVERTIME_DT = apply_overtime_dt;
            return wfb2di.getCLOCK_RECORDS();
        }
        catch (Exception)
        {

            throw;
        }
    }

    public string CancelOVERTIME_APPLY(List<Tuple<string, string, string>> emp_id)
    {
        try
        {
            CFB2DI0600DAO wfb2di = new CFB2DI0600DAO();

            BeginTransaction();
            foreach (var item in emp_id)
            {
                wfb2di.CancelOVERTIME_APPLY(item);
                wfb2di.update_DUTY_CHECK_STATUS(item);
                //日勤務狀態reopen-代休日期
                wfb2di.update_DUTY_CHECK_STATUS2(item);
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

    public DataTable getOVERTIME_APPLY(string emp_id, string apply_overtime_dt, string iflow_no)
    {
        try
        {
            CFB2DI0600DAO wfb2di = new CFB2DI0600DAO();
            wfb2di.EMP_ID = emp_id;
            wfb2di.APPLY_OVERTIME_DT = apply_overtime_dt;
            wfb2di.IFLOW_NO = iflow_no;
            return wfb2di.getOVERTIME_APPLY();
        }
        catch (Exception)
        {

            throw;
        }
    }

    //一括更新
    public string BatchEdit(List<Tuple<string, string, string, string, string>> emp_id)
    {
        try
        {
            CFB2DI0600DAO wfb2di = new CFB2DI0600DAO();
            string msg = "";
            foreach (var item in emp_id)
            {
                DataTable tmp = wfb2di.getSALARY_MONTH_CTRL(item);
                if (tmp.Rows.Count > 0)
                {
                    msg = "發薪日期不可小於對應所屬薪資結算年月之發薪日期";
                    return msg;
                }
            }

            BeginTransaction();
            foreach (var item in emp_id)
            {
                wfb2di.BatchEditOVERTIME_APPLY(item);
                wfb2di.BatchEditEMP_DUTY_CHECK_STATUS(item);
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

    //EXCEL匯出
    public IWorkbook createWFB2DI0600ExportXLS(CFB2DI0600DAO wfb2di, string type, string apply_overtime_dt)
    {
        IWorkbook workbook = null;
        ISheet sheet = null;

        try
        {
            ICellStyle style1;
            ICellStyle style2;
            ICellStyle style3;
            ICellStyle style4;
            DataTable tmp = wfb2di.searchResult();
            if (tmp.Rows.Count > 0)
            {
                if (type == "xls")
                {
                    workbook = new HSSFWorkbook();
                    sheet = (HSSFSheet)workbook.CreateSheet("FB2DI060_1");
                    style1 = (HSSFCellStyle)workbook.CreateCellStyle();
                }
                else
                {
                    workbook = new XSSFWorkbook();
                    sheet = workbook.CreateSheet("FB2DI060_1");
                    style1 = (XSSFCellStyle)workbook.CreateCellStyle();
                }

                IFont font1 = workbook.CreateFont();
                font1.FontName = "新細明體";
                font1.FontHeightInPoints = 12;
                style1.SetFont(font1);

                IFont font2 = workbook.CreateFont();
                font2.FontName = "新細明體";
                font2.FontHeightInPoints = 14;

                //標題 樣式
                style3 = (XSSFCellStyle)workbook.CreateCellStyle();
                style3.SetFont(font2);
                style3.Alignment = HorizontalAlignment.Center;
                style3.VerticalAlignment = VerticalAlignment.Center;

                //grid header 樣式
                style4 = (XSSFCellStyle)workbook.CreateCellStyle();
                ((XSSFCellStyle)style4).SetFillForegroundColor(new XSSFColor(Color.LightGray));
                ((XSSFCellStyle)style4).FillPattern = FillPattern.SolidForeground;
                ((XSSFCellStyle)style4).BorderBottom = BorderStyle.Thin;
                ((XSSFCellStyle)style4).BorderLeft = BorderStyle.Thin;
                ((XSSFCellStyle)style4).BorderRight = BorderStyle.Thin;
                ((XSSFCellStyle)style4).BorderTop = BorderStyle.Thin;
                style4.SetFont(font1);
                style4.Alignment = HorizontalAlignment.Center;
                style4.VerticalAlignment = VerticalAlignment.Center;

                IRow row = sheet.CreateRow(0);
                ICell cell;
                ICellStyle stringLeftStyle = this.setCellStyle(workbook, "left", true);
                ICellStyle stringRightStyle = this.setCellStyle(workbook, "right", true);
                ICellStyle stringCenterStyle = this.setCellStyle(workbook, "center", true);

                cell = row.CreateCell(1);
                cell.CellStyle = style3;
                cell.SetCellValue("加班實績");
                sheet.AddMergedRegion(new CellRangeAddress(0, 0, 1, 21));

                row = sheet.CreateRow(1);
                cell = row.CreateCell(1);
                cell.CellStyle = style1;
                cell.SetCellValue("加班日期：" + apply_overtime_dt);
                sheet.AddMergedRegion(new CellRangeAddress(1, 1, 1, 6));

                cell = row.CreateCell(17);
                cell.CellStyle = style1;
                cell.SetCellValue("製表日期：" + DateTime.Now.ToString("yyyy/MM/dd"));
                sheet.AddMergedRegion(new CellRangeAddress(1, 1, 17, 21));

                row = sheet.CreateRow(2);
                cell = row.CreateCell(1);
                cell.CellStyle = style4;
                cell.SetCellValue("序號");

                cell = row.CreateCell(2);
                cell.CellStyle = style4;
                cell.SetCellValue("部門");

                cell = row.CreateCell(3);
                cell.CellStyle = style4;
                cell.SetCellValue("工號");

                cell = row.CreateCell(4);
                cell.CellStyle = style4;
                cell.SetCellValue("姓名");

                cell = row.CreateCell(5);
                cell.CellStyle = style4;
                cell.SetCellValue("職種");

                cell = row.CreateCell(6);
                cell.CellStyle = style4;
                cell.SetCellValue("工數區分");

                cell = row.CreateCell(7);
                cell.CellStyle = style4;
                cell.SetCellValue("加班日期");

                cell = row.CreateCell(8);
                cell.CellStyle = style4;
                cell.SetCellValue("加班類型");

                cell = row.CreateCell(9);
                cell.CellStyle = style4;
                cell.SetCellValue("加班時段別");

                cell = row.CreateCell(10);
                cell.CellStyle = style4;
                cell.SetCellValue("加班申請");

                cell = row.CreateCell(11);
                cell.CellStyle = style4;
                cell.SetCellValue("加班核淮");

                cell = row.CreateCell(12);
                cell.CellStyle = style4;
                cell.SetCellValue("加班計算");

                cell = row.CreateCell(13);
                cell.CellStyle = style4;
                cell.SetCellValue("班別");

                cell = row.CreateCell(14);  
                cell.CellStyle = style4;
                cell.SetCellValue("勤前時數");

                cell = row.CreateCell(15);
                cell.CellStyle = style4;
                cell.SetCellValue("勤前起迄時間");

                cell = row.CreateCell(16);
                cell.CellStyle = style4;
                cell.SetCellValue("勤後時數");

                cell = row.CreateCell(17);
                cell.CellStyle = style4;
                cell.SetCellValue("勤後起迄時間");

                cell = row.CreateCell(18);
                cell.CellStyle = style4;
                cell.SetCellValue("出差起迄");

                cell = row.CreateCell(19);
                cell.CellStyle = style4;
                cell.SetCellValue("出差申請時數");

                cell = row.CreateCell(20);
                cell.CellStyle = style4;
                cell.SetCellValue("核淮年月");

                cell = row.CreateCell(21);
                cell.CellStyle = style4;
                cell.SetCellValue("計薪狀態");

                cell = row.CreateCell(22);
                cell.CellStyle = style4;
                cell.SetCellValue("發薪日期");

                cell = row.CreateCell(23);
                cell.CellStyle = style4;
                cell.SetCellValue("表單狀態");

                cell = row.CreateCell(24);
                cell.CellStyle = style4;
                cell.SetCellValue("申請單號");

                cell = row.CreateCell(25);
                cell.CellStyle = style4;
                cell.SetCellValue("備註");

                cell = row.CreateCell(26);
                cell.CellStyle = style4;
                cell.SetCellValue("離社日期");

                style2 = workbook.CreateCellStyle();
                ((XSSFCellStyle)style2).BorderBottom = BorderStyle.Thin;
                ((XSSFCellStyle)style2).BorderLeft = BorderStyle.Thin;
                ((XSSFCellStyle)style2).BorderRight = BorderStyle.Thin;
                ((XSSFCellStyle)style2).BorderTop = BorderStyle.Thin;
                style2.SetFont(font1);

                int x = 0;
                string apply_overtime_hour = "";
                string approve_overtime_hour = "";
                string before_hour = "";
                string after_hour = "";
                string overtime_pay_hour = "";
                for (int i = 0; i < tmp.Rows.Count; i++)
                {
                    x = i + 3;
                    row = sheet.CreateRow(x);
                    cell = row.CreateCell(1);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["RowNumber"].ToString());

                    cell = row.CreateCell(2);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["DEPT_NAME"].ToString());

                    cell = row.CreateCell(3);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["EMP_ID"].ToString());

                    cell = row.CreateCell(4);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["EMP_NAME"].ToString());

                    cell = row.CreateCell(5);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["WS_CD"].ToString());

                    cell = row.CreateCell(6);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["WORK_CD"].ToString());

                    cell = row.CreateCell(7);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["APPLY_OVERTIME_DT"].ToString());

                    cell = row.CreateCell(8);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["OVERTIME_CD"].ToString());

                    cell = row.CreateCell(9);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["OVERTIME_TIME_CD"].ToString());

                    cell = row.CreateCell(10);
                    cell.CellStyle = style2;
                    apply_overtime_hour = utilities.toHourMinute(tmp.Rows[i]["APPLY_OVERTIME_HOUR"].ToString());
                    cell.SetCellValue(apply_overtime_hour);

                    cell = row.CreateCell(11);
                    cell.CellStyle = style2;
                    approve_overtime_hour = utilities.toHourMinute(tmp.Rows[i]["APPROVE_OVERTIME_HOUR"].ToString());
                    cell.SetCellValue(approve_overtime_hour);

                    cell = row.CreateCell(12);
                    cell.CellStyle = style2;
                    overtime_pay_hour = utilities.toHourMinute(tmp.Rows[i]["OVERTIME_PAY_HOUR"].ToString());
                    cell.SetCellValue(overtime_pay_hour);

                    cell = row.CreateCell(13);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["SHIFT_CD_DESC"].ToString());

                    cell = row.CreateCell(14);
                    cell.CellStyle = style2;
                    before_hour = utilities.toHourMinute(tmp.Rows[i]["BEFORE_HOUR"].ToString());
                    cell.SetCellValue(before_hour);

                    cell = row.CreateCell(15);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["BEFORE_TIME"].ToString());

                    cell = row.CreateCell(16);
                    cell.CellStyle = style2;
                    after_hour = utilities.toHourMinute(tmp.Rows[i]["AFTER_HOUR"].ToString());
                    cell.SetCellValue(after_hour);

                    cell = row.CreateCell(17);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["AFTER_TIME"].ToString());


                    cell = row.CreateCell(18);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["TRIP_TIME"].ToString());

                    cell = row.CreateCell(19);
                    cell.CellStyle = style2;
                    after_hour = utilities.toHourMinute(tmp.Rows[i]["TRIP_HOUR"].ToString());
                    cell.SetCellValue(after_hour);


                    cell = row.CreateCell(20);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["IFLOW_APPROVE_DT"].ToString());

                    cell = row.CreateCell(21);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["SALARY_SETTLE_STATUS"].ToString());

                    cell = row.CreateCell(22);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["PAY_DT"].ToString());

                    cell = row.CreateCell(23);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["FORM_STATUS"].ToString());

                    cell = row.CreateCell(24);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["IFLOW_NO"].ToString());

                    cell = row.CreateCell(25);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["REMARK"].ToString());

                    cell = row.CreateCell(26);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["LEAVE_DT"].ToString() != "" ? Convert.ToDateTime(tmp.Rows[i]["LEAVE_DT"].ToString()).ToString("yyyy/MM/dd") : "");
                }
                for (int i = 0; i <= 26; i++)
                {
                    sheet.AutoSizeColumn(i);
                }

                //ExcelHandle.exportExcel(workbook, "FB2DI060_1." + type);

                //return "0";
                return workbook;
            }
            else
            {
                return null;
            }
        }
        catch
        {
            throw;
        }
        finally
        {
            sheet = null;
        }

    }




    //假日加班匯出
    public IWorkbook createExcel(string excelPath, CFB2DI0600DAO di060DAO, DataTable dt)
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

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        x = i + 1;//從第1列開始insert 資料
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
                        //加班日期
                        cell = row.CreateCell(3);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["APPLY_OVERTIME_DT"].ToString());
                        //星期X
                        cell = row.CreateCell(4);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["WEEKDT"].ToString());
                        //FLOW單號
                        cell = row.CreateCell(5);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["IFLOW_NO"].ToString());

                        //加班日期類型
                        cell = row.CreateCell(6);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["OVERTIME_DESC"].ToString());
                        //加班類型
                        cell = row.CreateCell(7);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["OVERTIME_TIME_CD_DESC"].ToString());
                        //班別代碼
                        cell = row.CreateCell(8);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["SHIFT_CD_DESC"].ToString());
                        //勤務日期
                        cell = row.CreateCell(9);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["CALENDAR_DT"].ToString());
                        //勤前起迄
                        cell = row.CreateCell(10);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["BEFORE_TIME"].ToString());

                        //勤後起迄
                        cell = row.CreateCell(11);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["AFTER_TIME"].ToString());
                        //申請時數(分)
                        cell = row.CreateCell(12);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(Convert.ToDouble(dt.Rows[i]["APPLY_OVERTIME_HOUR"].ToString()));
                        //核准時數(分)
                        cell = row.CreateCell(13);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(Convert.ToDouble(dt.Rows[i]["APPROVE_OVERTIME_HOUR"].ToString()));
                        //計算時數(分)
                        cell = row.CreateCell(14);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(Convert.ToDouble(dt.Rows[i]["OVERTIME_PAY_HOUR"].ToString()));
                        //休息日時數(分)
                        cell = row.CreateCell(15);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(Convert.ToDouble(dt.Rows[i]["CAL_HOUR_2"].ToString()));

                        //例假日時數(分)
                        cell = row.CreateCell(16);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(Convert.ToDouble(dt.Rows[i]["CAL_HOUR_3"].ToString()));
                        //日期類型
                        cell = row.CreateCell(17);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["DT_TYPE"].ToString());
                        //是否換休
                        cell = row.CreateCell(18);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["IS_APPLY"].ToString());
                        //是否刷卡比對
                        cell = row.CreateCell(19);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["IS_DUTY_CHECK"].ToString());
                        //建立日期
                        cell = row.CreateCell(20);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["CREATED_DT"].ToString());

                        //加班特殊狀況
                        cell = row.CreateCell(21);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["O_SPECIAL_CD_DESC"].ToString());

                        //出差起迄
                        cell = row.CreateCell(22);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["TRIP_TIME"].ToString());

                        //出差時數
                        cell = row.CreateCell(23);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["TRIP_HOUR"].ToString());

                    }
                    //製表日期
                    ICellStyle stringLeftStyleDate = this.setCellStyle(workbook, "left", false);
                    row = sheet.GetRow(0);
                    cell = row.CreateCell(24);
                    cell.CellStyle = stringLeftStyleDate;
                    cell.SetCellValue("製表日期:" + DateTime.Now.ToString("yyyy/MM/dd"));


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


    //假日加班匯入
    public IWorkbook uploadExcel(Stream fs)
    {
        bool valid = true;

        bool isNewPK = true; //用來判斷是否要新增至excel PK TB

        //建一個跟要insert相同的TB (加班計算時數上傳檔)
        DataTable myTable = new DataTable("myTable");

        //存於PK值
        DataTable excel_pk_dt = new DataTable();

        //暫時的DT,之後要進行刪除上傳檔的PK值
        DataTable temp_dt = new DataTable();

        //日期類型資料
        DataTable dt_type_dt = new DataTable();

        //EXCEL資料的PK值
        string[] excel_pk;

        string[] dt_type_pk;

        string error = "";
        try
        {
            //取得登入者
            string userid = SessionHandle.Current.emp_id;

            CFB2DI0600DAO di060DAO = new CFB2DI0600DAO();

            IWorkbook workbook;
            workbook = new XSSFWorkbook(fs);
            //取得sheet
            ISheet sheet = workbook.GetSheetAt(0);
            ICellStyle style1 = workbook.CreateCellStyle();
            IFont font1 = workbook.CreateFont();

            font1.Color = HSSFColor.Red.Index;


            int overtime_h = 0;

            if (sheet != null)
            {
                #region 建立 DataTable

                //建立 DataTable
                DataRow myRow;

                //建立 FieldSchema
                myTable.Columns.Add("EMP_ID", System.Type.GetType("System.String"));
                myTable.Columns.Add("APPLY_OVERTIME_DT", System.Type.GetType("System.DateTime"));
                myTable.Columns.Add("IFLOW_NO", System.Type.GetType("System.String"));
                myTable.Columns.Add("CALENDAR_DT", System.Type.GetType("System.DateTime"));
                myTable.Columns.Add("CAL_OVERTIME_HOUR", System.Type.GetType("System.Decimal"));
                myTable.Columns.Add("DT_TYPE", System.Type.GetType("System.String"));
                myTable.Columns.Add("IS_DUTY_CHECK", System.Type.GetType("System.String"));
                myTable.Columns.Add("CREATED_BY", System.Type.GetType("System.String"));
                myTable.Columns.Add("CREATED_DT", System.Type.GetType("System.DateTime"));
                myTable.Columns.Add("UPDATED_BY", System.Type.GetType("System.String"));
                myTable.Columns.Add("UPDATED_DT", System.Type.GetType("System.DateTime"));
                myTable.Columns.Add("FUNC_ID", System.Type.GetType("System.String"));

                //建立PK值
                DataRow excel_pk_row;
                excel_pk_dt.Columns.Add("EMP_ID", System.Type.GetType("System.String"));
                excel_pk_dt.Columns.Add("APPLY_OVERTIME_DT", System.Type.GetType("System.DateTime"));
                excel_pk_dt.Columns.Add("IFLOW_NO", System.Type.GetType("System.String"));

                #endregion

                //巡覽每row的資料第一列為title跳過
                for (int i = 1; i <= sheet.LastRowNum; i++)
                {

                    if (sheet.LastRowNum == 0)
                    {
                        error = "請輸入上傳資料\n,";
                        style1.SetFont(font1);
                        sheet.GetRow(0).CreateCell(0).CellStyle = style1;
                        //傳出錯誤訊息  
                        sheet.GetRow(0).GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(error);
                        if (error != "")
                        {
                            valid = false;
                        }
                    }

                    if (sheet.LastRowNum != 0)
                    {

                        #region 取得使用者區分清單
                        dt_type_dt = utilities.getCommCodeVal("DA", "DT_TYPE", "");
                        dt_type_dt.PrimaryKey = new DataColumn[] { dt_type_dt.Columns["sub_cd"] };

                        #endregion

                    }


                    if (sheet.GetRow(i) != null)
                    {
                        error = "";
                        excel_pk = new string[3];

                        #region 讀取cell資料
                        di060DAO.EMP_ID = sheet.GetRow(i).GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                        di060DAO.APPLY_OVERTIME_DT = sheet.GetRow(i).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                        di060DAO.IFLOW_NO = sheet.GetRow(i).GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().ToUpper();
                        di060DAO.CALENDAR_DT = sheet.GetRow(i).GetCell(9, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().ToUpper();
                        di060DAO.CAL_OVERTIME_HOUR = sheet.GetRow(i).GetCell(14, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                        di060DAO.DT_TYPE = sheet.GetRow(i).GetCell(17, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().ToUpper();
                        di060DAO.IS_DUTY_CHECK = sheet.GetRow(i).GetCell(19, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().ToUpper();

                        excel_pk[0] = di060DAO.EMP_ID;
                        excel_pk[1] = di060DAO.APPLY_OVERTIME_DT;
                        excel_pk[2] = di060DAO.IFLOW_NO;

                        #endregion


                        #region 檢核基本邏輯
                        //長度檢核
                        error += utilities.checkLength(di060DAO.EMP_ID, "工號", 5, false);
                        error += utilities.checkLength(di060DAO.IFLOW_NO, "IFLOW單號", 20, false);
                        error += utilities.checkLength(di060DAO.DT_TYPE, "日期類型", 1, false);
                        error += utilities.checkLength(di060DAO.IS_DUTY_CHECK, "是否刷卡比對", 1, false);

                        //數字檢查
                        error += utilities.checkNumberWithPoint(di060DAO.CAL_OVERTIME_HOUR, "計算時數", 4, 0);

                        //日期檢查
                        error += utilities.checkDateFormat(di060DAO.CALENDAR_DT, "勤務日期", false);
                        error += utilities.checkDateFormat(di060DAO.APPLY_OVERTIME_DT, "加班日期", false);

                        if (error == "")
                        {
                            overtime_h = Convert.ToInt32(di060DAO.CAL_OVERTIME_HOUR);
                            if (overtime_h % 30 != 0)
                            {
                                error += "計算時數需為30倍數,";
                            }

                            //若日期類型為5 (公司給假時)時,沒有受限0,240,480,720
                            if (di060DAO.DT_TYPE != "" && di060DAO.DT_TYPE == "5")
                            {
                                //若日期類型為5,不處理
                            }else{
                                //計算時數<= 480時,只能為 0,240,480
                                if (overtime_h <= 480 && ((overtime_h != 480 && overtime_h != 240 && overtime_h != 0)))
                                {
                                    error += "計算時數需為0,240或480,";
                                }
                            }

                            if (overtime_h > 1440) {
                                error += "計算時數不可大於1440,";
                            } 
                        }


                        //存在否
                        //日期類型
                        dt_type_pk = new string[1];
                        dt_type_pk[0] = di060DAO.DT_TYPE;

                        DataRow dr;
                        if (di060DAO.DT_TYPE != "")
                        {
                            //存在否  
                            dr = dt_type_dt.Rows.Find(dt_type_pk);
                            if (dr == null)
                            {
                                error += "日期類型不存在\n";
                            }
                        }

                        //判斷EXCEL是否有相同的PK
                        isNewPK = true;
                        if (excel_pk_dt.Rows.Count > 0)
                        {
                            dr = excel_pk_dt.Rows.Find(excel_pk);
                            if (dr != null)
                            {
                                error += "此EXCEL有相同的 工號, 勤務日期,IFLOW單號\n,";
                                isNewPK = false;
                            }
                        }

                        //是否刷卡比對
                        if (di060DAO.IS_DUTY_CHECK != "")
                        {
                            if (di060DAO.IS_DUTY_CHECK != "Y" && di060DAO.IS_DUTY_CHECK != "N")
                            {
                                error += "是否刷卡比對為必填且只能為Y或N\n,";
                            }
                        }

                        //excel的PK值  建立excel PK值資料  
                        if (error == "" && (excel_pk_dt.Rows.Count == 0 || isNewPK == true))
                        {
                            excel_pk_row = excel_pk_dt.NewRow();
                            excel_pk_row["EMP_ID"] = di060DAO.EMP_ID;
                            excel_pk_row["APPLY_OVERTIME_DT"] = di060DAO.APPLY_OVERTIME_DT;
                            excel_pk_row["IFLOW_NO"] = di060DAO.IFLOW_NO;
                            excel_pk_dt.Rows.Add(excel_pk_row);

                            excel_pk_dt.PrimaryKey = new DataColumn[] { 
                                                         excel_pk_dt.Columns["EMP_ID"]
                                                        ,excel_pk_dt.Columns["APPLY_OVERTIME_DT"] 
                                                        ,excel_pk_dt.Columns["IFLOW_NO"] 
                                                    };
                        }

                        //傳出錯誤訊息
                        style1.SetFont(font1);
                        sheet.GetRow(i).CreateCell(0).CellStyle = style1;
                        sheet.GetRow(i).GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(error);
                        if (error != "")
                        {
                            valid = false;
                        }

                        #endregion



                        if (valid)
                        {
                            #region 建立資料

                            // 建立資料
                            myRow = myTable.NewRow();
                            myRow["EMP_ID"] = di060DAO.EMP_ID;
                            myRow["APPLY_OVERTIME_DT"] = Convert.ToDateTime(di060DAO.APPLY_OVERTIME_DT);
                            myRow["IFLOW_NO"] = di060DAO.IFLOW_NO;
                            myRow["CALENDAR_DT"] = Convert.ToDateTime(di060DAO.CALENDAR_DT);
                            myRow["CAL_OVERTIME_HOUR"] = di060DAO.CAL_OVERTIME_HOUR;
                            myRow["DT_TYPE"] = di060DAO.DT_TYPE;
                            myRow["IS_DUTY_CHECK"] = di060DAO.IS_DUTY_CHECK;
                            myRow["CREATED_BY"] = userid;
                            myRow["CREATED_DT"] = DateTime.Now;
                            myRow["UPDATED_BY"] = userid;
                            myRow["UPDATED_DT"] = DateTime.Now;
                            myRow["FUNC_ID"] = "FF2DI060";
                            myTable.Rows.Add(myRow);

                            #endregion
                        }

                    }
                }// for end


                if (valid == false)
                {
                    return workbook;
                }

                //若檢核成功
                try
                {
                    BeginTransaction();
                    //刪除相同KEY的舊檔
                    int flag = 0;
                    #region 建立暫存excel PK值

                    DataRow temp_row;
                    temp_dt.Columns.Add("EMP_ID", System.Type.GetType("System.String"));
                    temp_dt.Columns.Add("APPLY_OVERTIME_DT", System.Type.GetType("System.DateTime"));
                    temp_dt.Columns.Add("IFLOW_NO", System.Type.GetType("System.String"));
                    #endregion

                    for (int i = 0; i < excel_pk_dt.Rows.Count; i++)
                    {
                        flag++;
                        #region 建立暫存excel PK值資料,用於刪除用

                        temp_row = temp_dt.NewRow();
                        temp_row["EMP_ID"] = excel_pk_dt.Rows[i]["EMP_ID"];
                        temp_row["APPLY_OVERTIME_DT"] = excel_pk_dt.Rows[i]["APPLY_OVERTIME_DT"];
                        temp_row["IFLOW_NO"] = excel_pk_dt.Rows[i]["IFLOW_NO"];
                        temp_dt.Rows.Add(temp_row);
                        #endregion

                        //條件的內送參數最多2100個 /3 個參數故最大為700, 
                        if (flag == 680)
                        {
                            di060DAO.deleteExcelData(temp_dt);

                            //刪除後 flag 及temp_dt清除就好了(重建並建立PK值)
                            flag = 0;
                            temp_dt.Clear();
                            /*
                            temp_dt = new DataTable();
                            temp_dt.Columns.Add("EMP_ID", System.Type.GetType("System.String"));
                            temp_dt.Columns.Add("APPLY_OVERTIME_DT", System.Type.GetType("System.DateTime"));
                            temp_dt.Columns.Add("IFLOW_NO", System.Type.GetType("System.String"));
                            */
                            continue;
                        }
                    }
                    //若筆數小於680或有餘數時執行
                    if (flag != 0)
                    {
                        di060DAO.deleteExcelData(temp_dt);
                    }
                    Commit();

                    //使用SqlBulkCopy
                    di060DAO.WriteToDatabase("TB_D_M_OVERTIME_CAL", myTable);

                    //呼叫SP
                    di060DAO.execSP_D_OVERTIME_CAL(userid);

                }
                catch (Exception ex)
                {
                    RollBack();
                    throw;
                }


            }

            return null;
        }
        catch (Exception ex)
        {
            throw;

        }
        finally
        {
            //清空暫存的DT
            myTable.Clear();
            excel_pk_dt.Clear();
            dt_type_dt.Clear();
            temp_dt.Clear();

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

    public string saveOVERTIME_APPLY(CFB2DI0600DAO wfb2di, string mod)
    {
        try
        {
            string errMsg = "";
            //檢核畫面輸入之加班單與其他已存在加班單有效加班時段不可重疊  (檢核有效之加班記錄時段不可重疊)
            DataTable dupApplyData = wfb2di.getdupApplyData();
            if (mod == "add" && dupApplyData.Rows.Count > 0)
            {
                if ((int)dupApplyData.Rows[0]["datacount"] > 0)
                {
                    errMsg += "加班日期時間已存在，不可重複 ! \\n";
                }
            }

            if (wfb2di.OVERTIME_CD == "G")
            {
                if (!wfb2di.getLeaveGI("S0"))
                    errMsg += "未核准臨時停工，不允許申請此類加班 ! \\n";
            }

            if (wfb2di.OVERTIME_CD == "I")
            {
                if (!wfb2di.getLeaveGI("E6"))
                    errMsg += "未核准原住民假，不允許申請此類加班 !\\n";
            }

            if (errMsg.Trim().Length == 0)
            {
                BeginTransaction();

                //更新模式
                if (mod == "mod")
                {
                    //更新
                    wfb2di.updateOVERTIME_APPLY();
                }
                else
                {
                    //新增模式
                    wfb2di.addOVERTIME_APPLY();
                }
                //更新日勤務狀態資料檔及重新reopen
                wfb2di.updateEMP_DUTY_CHECK_STATUS("0");
                if (string.IsNullOrEmpty(wfb2di.REPLACE_DT) == false)
                {
                    wfb2di.updateEMP_DUTY_CHECK_STATUS("1");
                }

                Commit();
                return "0";
            }
            else
                return errMsg;
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    public string getIFLOW_NO(string apply_overtime_dt)
    {
        try
        {
            string iflow_no = "";
            CFB2DI0600DAO wfb2di = new CFB2DI0600DAO();
            DataTable tmp = wfb2di.getIFLOW_NO(apply_overtime_dt);
            if (tmp.Rows.Count > 0)
            {
                iflow_no = tmp.Rows[0]["IFLOW_NO"].ToString();
                int no = Convert.ToInt32(iflow_no.Substring(11));
                iflow_no = "HRO" + Convert.ToDateTime(apply_overtime_dt).ToString("yyyyMMdd") + (no + 1).ToString("00000");
            }
            else
            {
                iflow_no = "HRO" + Convert.ToDateTime(apply_overtime_dt).ToString("yyyyMMdd") + "00001";
            }
            return iflow_no;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public List<string> getWORK_CD(string emp_id)
    {
        try
        {
            List<string> temp = new List<string>();
            CFB2DI0600DAO wfb2di = new CFB2DI0600DAO();
            DataTable tmp = wfb2di.getWORK_CD(emp_id);
            if (tmp.Rows.Count > 0)
            {
                temp.Add(tmp.Rows[0]["WORK_CD"].ToString());
                temp.Add(tmp.Rows[0]["OVERTIME_CTL_CD"].ToString());
                temp.Add(tmp.Rows[0]["WS_CD"].ToString());
                temp.Add(tmp.Rows[0]["PJOB_CD"].ToString());
                temp.Add(tmp.Rows[0]["DEPT_NO"].ToString());
            }

            return temp;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getDefaultData(string emp_id, string apply_overtime_dt, string iflow_no)
    {
        try
        {
            CFB2DI0600DAO wfb2di = new CFB2DI0600DAO();
            return wfb2di.getDefaultData(emp_id, apply_overtime_dt, iflow_no);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getEMP_NAME(string emp_id)
    {
        try
        {
            CFB2DI0600DAO wfb2di = new CFB2DI0600DAO();
            return wfb2di.getEMP_NAME(emp_id);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getSUB_DESC(string main_cd, string sys_cd, string sub_cd)
    {
        try
        {
            CFB2DI0600DAO wfb2di = new CFB2DI0600DAO();
            return wfb2di.getSUB_DESC(main_cd, sys_cd, sub_cd);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public bool checkAPPLY_OVERTIME_DT(string emp_id, string apply_overtime_dt, string overtime_dt_type)
    {
        try
        {
            bool is_ok = false;
            CFB2DI0600DAO wfb2di = new CFB2DI0600DAO();
            DataTable tmp = wfb2di.checkAPPLY_OVERTIME_DT(emp_id, apply_overtime_dt, overtime_dt_type);
            if (tmp.Rows.Count > 0)
            {
                is_ok = true;
            }
            return is_ok;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public string getSHIFT_CD(string emp_id, string apply_overtime_dt)
    {
        try
        {
            string shift_cd = "";
            CFB2DI0600DAO wfb2di = new CFB2DI0600DAO();
            DataTable tmp = wfb2di.getSHIFT_CD(emp_id, apply_overtime_dt);
            if (tmp.Rows.Count > 0)
            {
                shift_cd = tmp.Rows[0]["SHIFT_CD"].ToString();
            }

            return shift_cd;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getSHIFT_DESC(string shift_cd)
    {
        try
        {
            CFB2DI0600DAO wfb2di = new CFB2DI0600DAO();
            return wfb2di.getSHIFT_DESC(shift_cd);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public string getIS_APPLY(string overtime_cd)
    {
        try
        {
            string is_apply = "";
            CFB2DI0600DAO wfb2di = new CFB2DI0600DAO();
            DataTable tmp = wfb2di.getOVERTIME_EXCHANGE_CD(overtime_cd);
            if (tmp.Rows.Count > 0)
            {
                is_apply = tmp.Rows[0]["OVERTIME_EXCHANGE_CD"].ToString();
            }

            return is_apply;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getTIME(string emp_id, string apply_overtime_dt, string stime, string etime, string WorkDayCd, string d, string ShiftCd)
    {
        try
        {
            CFB2DI0600DAO wfb2di = new CFB2DI0600DAO();
            return wfb2di.getTIME(emp_id, apply_overtime_dt, stime, etime, WorkDayCd, d, ShiftCd);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public bool checkDUTY_STIME(string emp_id, string apply_overtime_dt, string before_etime)
    {
        try
        {
            bool is_ok = false;
            CFB2DI0600DAO wfb2di = new CFB2DI0600DAO();
            DataTable tmp = wfb2di.checkDUTY_STIME(emp_id, apply_overtime_dt, before_etime);
            if (tmp.Rows.Count > 0)
            {
                is_ok = true;
            }
            return is_ok;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public bool checkDUTY_ETIME(string emp_id, string apply_overtime_dt, string after_stime)
    {
        try
        {
            bool is_ok = false;
            CFB2DI0600DAO wfb2di = new CFB2DI0600DAO();
            DataTable tmp = wfb2di.checkDUTY_ETIME(emp_id, apply_overtime_dt, after_stime);
            if (tmp.Rows.Count > 0)
            {
                is_ok = true;
            }
            return is_ok;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public bool checkOVERTIME_DT(string emp_id, string apply_overtime_dt,
        string before_stime, string before_etime, string after_stime, string after_etime, string before_time, string after_time)
    {
        try
        {
            bool is_ok = false;
            CFB2DI0600DAO wfb2di = new CFB2DI0600DAO();
            DataTable tmp =
                wfb2di.checkOVERTIME_DT(emp_id, apply_overtime_dt,
                before_stime, before_etime, after_stime, after_etime, before_time, after_time);
            if (tmp.Rows.Count > 0)
            {
                is_ok = true;
            }
            return is_ok;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public bool checkLEAVE_APPLY_DAY(string emp_id, string apply_overtime_dt,
        string before_stime, string before_etime, string after_stime, string after_etime, string before_time, string after_time)
    {
        try
        {
            bool is_ok = false;
            CFB2DI0600DAO wfb2di = new CFB2DI0600DAO();
            DataTable tmp =
                wfb2di.checkLEAVE_APPLY_DAY(emp_id, apply_overtime_dt,
                before_stime, before_etime, after_stime, after_etime, before_time, after_time);
            if (tmp.Rows.Count > 0)
            {
                is_ok = true;
            }
            return is_ok;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public int getSUM_HOUR(string emp_id, string apply_overtime_dt)
    {
        try
        {
            int SUM_HOUR = 0;
            CFB2DI0600DAO wfb2di = new CFB2DI0600DAO();
            apply_overtime_dt = apply_overtime_dt.Split('/')[0] + "-" + apply_overtime_dt.Split('/')[1];
            DataTable tmp = wfb2di.getSUM_HOUR(emp_id, apply_overtime_dt);
            if (tmp.Rows.Count > 0 && tmp.Rows[0]["SUM_HOUR"].ToString() != "")
            {
                SUM_HOUR = Convert.ToInt32(tmp.Rows[0]["SUM_HOUR"]);
            }
            return SUM_HOUR;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getEmpName(string emp_id)
    {
        try
        {
            CFB2DI0600DAO wfb2di = new CFB2DI0600DAO();
            return wfb2di.getEmpName(emp_id);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //取得發薪日期
    public DataTable getPAY_DT(string apply_overtime_dt)
    {
        try
        {
            CFB2DI0600DAO wfb2di = new CFB2DI0600DAO();
            return wfb2di.getPAY_DT(apply_overtime_dt);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //取得管理類別 
    public string getTARGET_TYPE(string dept_no, string ws_cd, string pjob_cd)
    {
        try
        {
            string target_type = "";
            CFB2DI0600DAO wfb2di = new CFB2DI0600DAO();
            DataTable tmp = wfb2di.getTARGET_TYPE(dept_no, ws_cd, pjob_cd);
            if (tmp.Rows.Count > 0)
            {
                target_type = tmp.Rows[0]["TARGET_TYPE"].ToString();
            }
            return target_type;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public bool checkDUTY_ETIME2(string emp_id, string apply_overtime_dt, string after_stime)
    {
        try
        {
            bool is_ok = false;
            CFB2DI0600DAO wfb2di = new CFB2DI0600DAO();
            DataTable tmp = wfb2di.checkDUTY_ETIME2(emp_id, apply_overtime_dt, after_stime);
            if (tmp.Rows.Count > 0)
            {
                is_ok = true;
            }
            return is_ok;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getOVERTIME_DT_TYPE(string overtime_cd)
    {
        try
        {
            CFB2DI0600DAO wfb2di = new CFB2DI0600DAO();
            return wfb2di.getOVERTIME_DT_TYPE(overtime_cd);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getOVERTIME(string emp_id, string apply_overtime_dt)
    {
        try
        {
            CFB2DI0600DAO wfb2di = new CFB2DI0600DAO();
            return wfb2di.checkOVERTIME(emp_id, apply_overtime_dt);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public string getOvertimeCD(string emp_id, string apply_overtime_dt, string apply_overtime_s, string apply_overtime_e)
    {
        try
        {
            CFB2DI0600DAO wfb2di = new CFB2DI0600DAO();
            DataTable dt = wfb2di.getOvertimeCD(emp_id, apply_overtime_dt, apply_overtime_s, apply_overtime_e);
            if (dt.Rows.Count > 0)
            {
                return "2-語文課時段";
            }
            else
            {
                return "1-一般時段";
            }

        }
        catch (Exception)
        {

            throw;
        }
    }

    public int getCTLHourType1(string emp_id, string apply_overtime_dt)
    {
        try
        {
            CFB2DI0600DAO wfb2di = new CFB2DI0600DAO();
            DataTable dt = wfb2di.getCTLHourType1(emp_id, apply_overtime_dt);
            return Convert.ToInt32(dt.Rows[0]["ctlsum"]);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //加班新增修改檢核
    public string SP_DI_OVERTIME_CHK(CFB2DI0600DAO dao)
    {
        try
        {
            string result = dao.SP_DI_OVERTIME_CHK();

            return result;
        }
        catch (Exception ex)
        {
            return "E" + ex.Message;
        }
    }

    //加班註銷檢核
    public string SP_DI_OVERTIME_X0_CHK(List<Tuple<string, string, string, string>> emp_id)
    {
        try
        {
            string result = "";
            string msg = "";
            CFB2DI0600DAO di060DAO = new CFB2DI0600DAO();
           
            foreach (var item in emp_id)
            {
                //有申告且非平日,才檢查
                if(item.Item3=="Y" && item.Item4!="1")

                    msg = di060DAO.SP_DI_OVERTIME_X0_CHK(item);
                if (msg != "")
                    result += msg + ";\\n";                  
            }           

            return result;
        }
        catch (Exception ex)
        {
            return "E" + ex.Message;
        }
    }

    public string getFN_D_GET_OVERTIME_APPLY_HOUR(CFB2DI0600DAO dao, string O_START_TIME, string O_END_TIME, string SORUCE_CD)
    {
        try
        {
            string result = "0";
            DataTable dt = dao.getFN_D_GET_OVERTIME_APPLY_HOUR(O_START_TIME, O_END_TIME, SORUCE_CD);
            if (dt.Rows.Count > 0)
            {
                result = dt.Rows[0]["OVERTIME_APPLY_HOUR"].ToString();
            }

            return result;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public string getHYPER_SHOUR(CFB2DI0600DAO dao, string h)
    {
        try
        {
            string result = "0";
            DataTable dt = dao.getHYPER_SHOUR();
            if (dt.Rows.Count > 0)
            {
                if (h == "1")
                    result = dt.Rows[0]["HYPER_SHOUR"].ToString();
                if (h == "2")
                    result = dt.Rows[0]["NORMAL_SHOUR"].ToString();
            }

            return result;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getTB_H_M_EMP(string EMP_ID)
    {
        try
        {
            CFB2DI0600DAO dao = new CFB2DI0600DAO();
            return dao.getTB_H_M_EMP(EMP_ID);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public string saveTB_D_M_OVERTIME_APPLY(CFB2DI0600DAO dao, string mod)
    {
        try
        {
            string errMsg = "";

            if (errMsg.Trim().Length == 0)
            {
                BeginTransaction();

                //更新模式
                if (mod == "mod")
                {
                    //更新
                    dao.updateOVERTIME_APPLY();
                }
                else
                {
                    //新增模式
                    dao.insertTB_D_M_OVERTIME_APPLY();
                }
                //(2)更新日勤務狀態檔- reopen
                //更新日勤務狀態資料檔及重新reopen
                dao.updateEMP_DUTY_CHECK_STATUS("0");
                if (string.IsNullOrEmpty(dao.REPLACE_DT) == false)
                {
                    dao.updateEMP_DUTY_CHECK_STATUS("1");
                }

                Commit();

                //啟動刷卡比對
                //啟動重新刷卡比對
                dao.SP_D_EMP_DUTY_CHECK_STATUS_RE_OPEN(dao.EMP_ID, dao.APPLY_OVERTIME_DT);

                return "0";
            }
            else
                return errMsg;
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    public DataTable getOvertimeCtlCD(string emp_id)
    {
        CFB2DI0600DAO dao = new CFB2DI0600DAO();
        try
        {
            return dao.getOvertimeCtlCD(emp_id);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getShiftCD(string emp_id, string apply_overtime_dt)
    {
        CFB2DI0600DAO dao = new CFB2DI0600DAO();
        try
        {
            return dao.getShiftCD(emp_id, apply_overtime_dt);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getClockTime(string emp_id, string apply_overtime_dt)
    {
        CFB2DI0600DAO dao = new CFB2DI0600DAO();
        try
        {
            return dao.getClockTime(emp_id, apply_overtime_dt);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getEMP_DATA(string emp_id)
    {
        try
        {
            CFB2DI0600DAO dao = new CFB2DI0600DAO();
            return dao.getEMP_DATA(emp_id);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getOvertimeDtType(string p)
    {
        CFB2DI0600DAO dao = new CFB2DI0600DAO();
        try
        {
            return dao.getOvertimeDtType(p);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getCalendarTime(string emp_id, string apply_overtime_dt)
    {
        try
        {
            CFB2DI0600DAO wfb2di = new CFB2DI0600DAO();
            wfb2di.EMP_ID = emp_id;
            wfb2di.APPLY_OVERTIME_DT = apply_overtime_dt;
            return wfb2di.getCalendarTime();
        }
        catch (Exception)
        {

            throw;
        }
    }

    public int chk_IS_APPLY(string empid, string overtime_cd)
    {
        CFB2DI0600DAO fb2di060 = new CFB2DI0600DAO();
        try
        {
            return fb2di060.chk_IS_APPLY(empid, overtime_cd);
        }
        catch (Exception)
        {
            throw;
        }
    }

}