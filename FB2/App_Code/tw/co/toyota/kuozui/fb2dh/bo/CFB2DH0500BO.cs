using FB2.tw.co.toyota.kuozui.bo;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;

/// <summary>
/// CFB2DH0500BO 的摘要描述
/// </summary>
public class CFB2DH0500BO : BaseService
{
    public CFB2DH0500BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public System.Data.DataTable getSubLeaveCD(string main_leave_cd, string sub_leave_cd)
    {
        CFB2DH0500DAO wfb2dh = new CFB2DH0500DAO();
        try
        {
            return wfb2dh.getSubLeaveCD(main_leave_cd, sub_leave_cd);
        }
        catch (Exception)
        {

            throw;
        }
    }



    public System.Data.DataTable getsubleave(string p)
    {
        CFB2DH0500DAO wfb2dh = new CFB2DH0500DAO();
        try
        {
            return wfb2dh.getsubleave(p);
        }
        catch (Exception)
        {

            throw;
        }
    }

    /*
    public DataTable getNewIFLOW_NO(string p)
    {
        CFB2DH0500DAO wfb2dh = new CFB2DH0500DAO();
        try
        {
            return wfb2dh.getNewIFLOW_NO(p);
        }
        catch (Exception)
        {

            throw;
        }
    }
    */

    //新增-儲存
    public string addLeave(CFB2DH0500DAO fb2dh0500)
    {
        try
        {
            DataTable dt = fb2dh0500.getEmpData();
            if (dt.Rows.Count > 0)
            {
                fb2dh0500.DEPT_NO = dt.Rows[0]["DEPT_NO"].ToString();
                fb2dh0500.EMP_CD = dt.Rows[0]["EMP_CD"].ToString();
                fb2dh0500.UNION_PJOB_CD = dt.Rows[0]["UNION_PJOB_CD"].ToString();
                fb2dh0500.LEVEL_CD = dt.Rows[0]["LEVEL_CD"].ToString();
            }
            //20191112先取得IFLOWNO
            dt = fb2dh0500.getIFLOW_NO();
            if (dt.Rows.Count > 0)
            {
                fb2dh0500.IFLOW_NO = dt.Rows[0]["IFLOW_NO"].ToString();
            }



            BeginTransaction();
            //新增至請假日檔
            fb2dh0500.addLeave();
            //新增至請假主檔
            fb2dh0500.addLeaveMain();
            //reopen 
            //fb2dh0500.SaveDUTY_CHECK_STATUS(fb2dh0500.EMP_ID, "", fb2dh0500.APPLY_LEAVE_SDT);
            //3.日勤務狀態reopen
            fb2dh0500.update_TB_D_M_EMP_DUTY_CHECK_STATUS();
                  
            Commit();

            //20190812 先執行分配作業,若失敗回傳錯誤訊息
            if (fb2dh0500.SUB_LEAVE_CD == "X0")
            {
                string errMsg = "";
                errMsg = fb2dh0500.SP_D_X0_MAPPING("U");  //U.不存在新增,存在修改
                string rtn_flag = errMsg.Split(';')[0];
                string rtn_msg = errMsg.Split(';')[1];

                if (rtn_flag != "Y")
                    return errMsg;
            }


            return "0";

        }

        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }

    }

    public DataTable getData(string emp_id, string iflow_no)
    {
        try
        {
            CFB2DH0500DAO fb2dh0500 = new CFB2DH0500DAO();
            return fb2dh0500.getData(emp_id, iflow_no);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //取得修改請假資料日檔
    public DataTable getData(string emp_id, string iflow_no, string leave_s_dt)
    {
        try
        {
            CFB2DH0500DAO fb2dh0500 = new CFB2DH0500DAO();
            return fb2dh0500.getData(emp_id, iflow_no, leave_s_dt);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //修改-儲存
    public string updateLeaveData(CFB2DH0500DAO dh050DAO)
    {
        try
        {

            string errMsg = "";

            //執行修改 假日換休請假單分配 
            if (dh050DAO.SUB_LEAVE_CD == "X0")
            {
                errMsg = dh050DAO.SP_D_X0_MAPPING("O");  //O.只修改單筆日檔資料
                string rtn_flag = errMsg.Split(';')[0];
                string rtn_msg = errMsg.Split(';')[1];
                if (rtn_flag != "Y")
                    return errMsg;
            }

            //初始值
            errMsg = "";
            if (errMsg == "")
            {
                DataTable dt = dh050DAO.getEMP_DAY_DUTY();
                DataTable empData = dh050DAO.getEmpData();
                if (empData.Rows.Count > 0)
                {
                    dh050DAO.DEPT_NO = empData.Rows[0]["DEPT_NO"].ToString();
                    dh050DAO.EMP_CD = empData.Rows[0]["EMP_CD"].ToString();
                    dh050DAO.UNION_PJOB_CD = empData.Rows[0]["UNION_PJOB_CD"].ToString();
                    dh050DAO.LEVEL_CD = empData.Rows[0]["LEVEL_CD"].ToString();
                }

              
                        

                BeginTransaction();

                //基本資料
                dh050DAO.updateLeaveData();
                //reopen
                //dao.SaveDUTY_CHECK_STATUS(dao.EMP_ID, "", dao.APPLY_LEAVE_SDT);
                //3.日勤務狀態reopen
                dh050DAO.update_TB_D_M_EMP_DUTY_CHECK_STATUS();
                Commit();
                return "0";
            }
            else
            {
                return errMsg;
            }
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }

    }
    //註銷
    public string Cancal(List<Tuple<string, string, string, string, string, string>> leave_apply, CFB2DH0500DAO dh050DAO)
    {
        try
        {
            BeginTransaction();
            foreach (var item in leave_apply)
            {
                dh050DAO.CancalD(item.Item1, item.Item2);

                //更新主檔時間
                dh050DAO.updateMainLeaveTime(item.Item1, item.Item2);
                //reopen
                //fb2dh0500.SaveDUTY_CHECK_STATUS(item.Item1,item.Item3);
                dh050DAO.EMP_ID = item.Item1;
                dh050DAO.IFLOW_NO = item.Item2;
                dh050DAO.APPLY_LEAVE_SDT = item.Item3;
                dh050DAO.APPLY_LEAVE_EDT = item.Item4;
                dh050DAO.APPLY_OVERTIME_DT = item.Item5;
                dh050DAO.SUB_LEAVE_CD = item.Item6;        //子假別
                dh050DAO.UPDATED_BY = SessionHandle.Current.emp_id;
                dh050DAO.FUNC_ID = "FB2DH050";
                //3.日勤務狀態reopen
                dh050DAO.update_TB_D_M_EMP_DUTY_CHECK_STATUS();

                //註銷加班分配單
                dh050DAO.SP_D_X0_MAPPING("D");

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

    public DataTable getMainLeave(string p)
    {
        CFB2DH0500DAO wfb2dh = new CFB2DH0500DAO();
        try
        {
            return wfb2dh.getMainLeave(p);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public string Save(List<Tuple<string, string, string>> leave_apply, CFB2DH0500DAO fb2dh0500)
    {
        try
        {
            BeginTransaction();

            foreach (var item in leave_apply)
            {
                fb2dh0500.SaveD(item.Item1, item.Item2);
                fb2dh0500.SaveDUTY_CHECK_STATUS(item.Item1, item.Item2, item.Item3);
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

    public DataTable getleavecountcd(string p)
    {
        try
        {
            //BeginTransaction();
            CFB2DH0500DAO wfb2dh = new CFB2DH0500DAO();
            return wfb2dh.getleavecountcd(p);
            //Commit();
        }
        catch (Exception)
        {
            RollBack();
            throw;
        }
    }

    public string testeachemp(string EMP_ID, string MAIN_LEAVE_CD, string SUB_LEAVE_CD)
    {
        string rtnmessage = "";
        CFB2DH0500DAO wfb2dh = new CFB2DH0500DAO();
        try
        {
            DataTable dt = wfb2dh.testeachemp(EMP_ID, MAIN_LEAVE_CD, SUB_LEAVE_CD);
            if (dt.Rows.Count == 0)
            {
                rtnmessage = "適用人員不符合！";
            }

            return rtnmessage;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public string getsex(string EMP_ID, string SUB_LEAVE_CD)
    {
        string rtnmessage = "";
        CFB2DH0500DAO wfb2dh = new CFB2DH0500DAO();
        try
        {
            DataTable dt = wfb2dh.getsex(EMP_ID);
            if (SUB_LEAVE_CD == "W0")
            {
                if (dt.Rows[0]["SEX_CD"].ToString() != "1")
                {
                    rtnmessage = "適用人員不符合！";
                }
            }
            else
            {
                if (dt.Rows[0]["SEX_CD"].ToString() != "2")
                {
                    rtnmessage = "適用人員不符合！";
                }
            }

            return rtnmessage;
        }
        catch (Exception)
        {

            throw;
        }
    }


    //檢核
    public string checkValid(CFB2DH0500DAO dh050DAO, string emp_id = "", bool checkDup = true)
    {
        try
        {
            string errMsg = "";
            errMsg = dh050DAO.SP_DH_LEAVE_CHK();
            return errMsg;


        }
        catch (Exception ex)
        {
            throw;
            //return ex.Message;
        }
    }


    //假日換休註銷,修改檢核 檢核
    public string checkX0_Valid(List<Tuple<string, string, string, string, string, string>> leave_apply)
    {
        try
        {
            CFB2DH0500DAO dh050DAO = new CFB2DH0500DAO();
            string errMsg = "";
            string rtnMsg = "";
            foreach (var item in leave_apply)
            {
                //假日換休才檢查
                if (item.Item6!="X0")
                    continue;

                dh050DAO.EMP_ID = item.Item1;
                dh050DAO.IFLOW_NO = item.Item2;
                dh050DAO.APPLY_LEAVE_SDT = item.Item3;
                dh050DAO.APPLY_LEAVE_EDT = item.Item4;
                dh050DAO.UPDATED_BY = SessionHandle.Current.emp_id;
                dh050DAO.FUNC_ID = "FB2DH050";
                errMsg = dh050DAO.SP_DH_LEAVE_DELUPD_CHK_X0();
                if (errMsg != "")
                {
                    rtnMsg += dh050DAO.EMP_ID+" "+dh050DAO.APPLY_LEAVE_SDT + " " + errMsg + ";\\n";
                }
            }

            return rtnMsg;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public DataTable getDayDuty(CFB2DH0500DAO dao)
    {

        try
        {
            return dao.getEMP_DAY_DUTY();

        }
        catch (Exception)
        {

            throw;
        }
    }

    public string getDUTY_RESULT_H(string APPLY_LEAVE_SDT)
    {
        try
        {
            CFB2DH0500DAO dao = new CFB2DH0500DAO();
            DataTable dt = dao.getDUTY_RESULT_H(APPLY_LEAVE_SDT);
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["SALARY_DT"].ToString();
            }
            else
                return "";
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getD_GET_SHIFT(CFB2DH0500DAO dao, string emp_id, string CALENDAR_DT)
    {
        try
        {
            return dao.getD_GET_SHIFT(emp_id, CALENDAR_DT);

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
            CFB2DH0400DAO dao = new CFB2DH0400DAO();
            return dao.getEMP_DATA(emp_id);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public IWorkbook createExcel(CFB2DH0500DAO dao, string type)
    {
        IWorkbook workbook = null;
        ISheet sheet = null;

        try
        {
            ICellStyle style1;
            ICellStyle style2;
            ICellStyle style3;
            ICellStyle style4;
            DataTable dt = dao.getExcelData();

            if (dt.Rows.Count > 0)
            {
                if (type == "xls")
                {
                    workbook = new HSSFWorkbook();
                    sheet = (HSSFSheet)workbook.CreateSheet("請假實績");
                    style1 = (HSSFCellStyle)workbook.CreateCellStyle();
                }
                else
                {
                    workbook = new XSSFWorkbook();
                    sheet = workbook.CreateSheet("請假實績");
                    style1 = (XSSFCellStyle)workbook.CreateCellStyle();
                }
                IFont font1 = workbook.CreateFont();
                font1.FontName = "新細明體";
                font1.FontHeightInPoints = 18;

                IFont font2 = workbook.CreateFont();
                font2.FontName = "新細明體";
                font2.FontHeightInPoints = 12;

                //標題 樣式
                style3 = (XSSFCellStyle)workbook.CreateCellStyle();
                style3.SetFont(font1);
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
                style4.SetFont(font2);
                style4.Alignment = HorizontalAlignment.Center;
                style4.VerticalAlignment = VerticalAlignment.Center;

                IRow row = sheet.CreateRow(1);
                ICell cell;
                cell = row.CreateCell(1);
                cell.SetCellValue("請假實績");
                cell.CellStyle = style3;
                sheet.AddMergedRegion(new CellRangeAddress(1, 1, 1, 18));

                row = sheet.CreateRow(2);
                cell = row.CreateCell(1);
                cell.SetCellValue("請假日期：" + dao.APPLY_LEAVE_SDT + "~" + dao.APPLY_LEAVE_EDT);
                cell.CellStyle = style1;
                sheet.AddMergedRegion(new CellRangeAddress(2, 2, 1, 5));

                cell = row.CreateCell(15);
                cell.SetCellValue("製表日期：" + DateTime.Now.ToString("yyyy/MM/dd"));
                cell.CellStyle = style1;
                sheet.AddMergedRegion(new CellRangeAddress(2, 2, 15, 18));

                row = sheet.CreateRow(3);
                cell = row.CreateCell(1);
                cell.SetCellValue("序號");
                cell.CellStyle = style4;

                cell = row.CreateCell(2);
                cell.SetCellValue("部門");
                cell.CellStyle = style4;


                cell = row.CreateCell(3);
                cell.SetCellValue("工號");
                cell.CellStyle = style4;

                cell = row.CreateCell(4);
                cell.SetCellValue("姓名");
                cell.CellStyle = style4;

                cell = row.CreateCell(5);
                cell.SetCellValue("主假別");
                cell.CellStyle = style4;

                cell = row.CreateCell(6);
                cell.SetCellValue("子假別");
                cell.CellStyle = style4;

                cell = row.CreateCell(7);
                cell.SetCellValue("事實發生日");
                cell.CellStyle = style4;

                cell = row.CreateCell(8);
                cell.SetCellValue("請假日期");
                cell.CellStyle = style4;

                cell = row.CreateCell(9);
                cell.SetCellValue("開始時間");
                cell.CellStyle = style4;

                cell = row.CreateCell(10);
                cell.SetCellValue("請假迄日");
                cell.CellStyle = style4;

                cell = row.CreateCell(11);
                cell.SetCellValue("結束時間");
                cell.CellStyle = style4;

                cell = row.CreateCell(12);
                cell.SetCellValue("請假合計(時：分)");
                cell.CellStyle = style4;

                cell = row.CreateCell(13);
                cell.SetCellValue("請假合計(分)");
                cell.CellStyle = style4;

                cell = row.CreateCell(14);
                cell.SetCellValue("核准年月");
                cell.CellStyle = style4;

                cell = row.CreateCell(15);
                cell.SetCellValue("計薪狀態");
                cell.CellStyle = style4;

                cell = row.CreateCell(16);
                cell.SetCellValue("發薪日期");
                cell.CellStyle = style4;

                cell = row.CreateCell(17);
                cell.SetCellValue("表單狀態");
                cell.CellStyle = style4;

                cell = row.CreateCell(18);
                cell.SetCellValue("申請單號");
                cell.CellStyle = style4;

                cell = row.CreateCell(19);
                cell.SetCellValue("備註");
                cell.CellStyle = style4;

                //style2 = workbook.CreateCellStyle();
                //style2.SetFont(font1);

                style1 = workbook.CreateCellStyle();
                ((XSSFCellStyle)style1).BorderBottom = BorderStyle.Thin;
                ((XSSFCellStyle)style1).BorderLeft = BorderStyle.Thin;
                ((XSSFCellStyle)style1).BorderRight = BorderStyle.Thin;
                ((XSSFCellStyle)style1).BorderTop = BorderStyle.Thin;
                style1.SetFont(font2);
                style2 = style1;
                style2.Alignment = HorizontalAlignment.Right;

                //int x = 5;
                int x = 4;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    row = sheet.CreateRow(x);
                    cell = row.CreateCell(1);
                    cell.CellStyle = style1;
                    cell.SetCellValue((i + 1).ToString());

                    cell = row.CreateCell(2);
                    cell.CellStyle = style1;
                    cell.SetCellValue(dt.Rows[i]["DEPT_NAME"].ToString());

                    cell = row.CreateCell(3);
                    cell.CellStyle = style1;
                    cell.SetCellValue(dt.Rows[i]["EMP_ID"].ToString());

                    cell = row.CreateCell(4);
                    cell.CellStyle = style1;
                    cell.SetCellValue(dt.Rows[i]["EMP_NAME"].ToString());

                    cell = row.CreateCell(5);
                    cell.CellStyle = style1;
                    cell.SetCellValue(dt.Rows[i]["MAIN_LEAVE_CD"].ToString());

                    cell = row.CreateCell(6);
                    cell.CellStyle = style1;
                    cell.SetCellValue(dt.Rows[i]["SUB_LEAVE_CD"].ToString());

                    cell = row.CreateCell(7);
                    cell.CellStyle = style1;
                    cell.SetCellValue(dt.Rows[i]["FACT_HAPPEN_DT"].ToString());

                    cell = row.CreateCell(8);
                    cell.CellStyle = style1;
                    cell.SetCellValue(dt.Rows[i]["APPLY_LEAVE_SDT"].ToString());

                    cell = row.CreateCell(9);
                    cell.CellStyle = style1;
                    cell.SetCellValue(dt.Rows[i]["APPLY_LEAVE_STIME"].ToString());

                    cell = row.CreateCell(10);
                    cell.CellStyle = style1;
                    cell.SetCellValue(dt.Rows[i]["APPLY_LEAVE_EDT"].ToString());

                    cell = row.CreateCell(11);
                    cell.CellStyle = style1;
                    cell.SetCellValue(dt.Rows[i]["APPLY_LEAVE_ETIME"].ToString());

                    cell = row.CreateCell(12);
                    cell.CellStyle = style1;
                    cell.SetCellValue( utilities.toHourMinute(dt.Rows[i]["TOTAL_TIME_APPROVE"].ToString()));

                    cell = row.CreateCell(13);
                    cell.CellStyle = style2;
                    cell.SetCellValue(Convert.ToDouble(dt.Rows[i]["TOTAL_TIME_APPROVE"].ToString()));

                    cell = row.CreateCell(14);
                    cell.CellStyle = style1;
                    cell.SetCellValue(dt.Rows[i]["IFLOW_APPROVE_DT"].ToString());

                    cell = row.CreateCell(15);
                    cell.CellStyle = style1;
                    cell.SetCellValue(dt.Rows[i]["SALARY_SETTLE_STATUS"].ToString());

                    cell = row.CreateCell(16);
                    cell.CellStyle = style1;
                    cell.SetCellValue(dt.Rows[i]["PAY_DT"].ToString());

                    cell = row.CreateCell(17);
                    cell.CellStyle = style1;
                    cell.SetCellValue(dt.Rows[i]["FORM_STATUS"].ToString());

                    cell = row.CreateCell(18);
                    cell.CellStyle = style1;
                    cell.SetCellValue(dt.Rows[i]["IFLOW_NO"].ToString());

                    cell = row.CreateCell(19);
                    cell.CellStyle = style1;
                    cell.SetCellValue(dt.Rows[i]["REMARK"].ToString());
                    x++;
                }


                sheet.AutoSizeColumn(1);
                sheet.AutoSizeColumn(2);
                sheet.AutoSizeColumn(3);
                sheet.AutoSizeColumn(4);
                sheet.AutoSizeColumn(5);
                sheet.AutoSizeColumn(6);
                sheet.AutoSizeColumn(7);
                sheet.AutoSizeColumn(8);
                sheet.AutoSizeColumn(9);
                sheet.AutoSizeColumn(10);
                sheet.AutoSizeColumn(11);
                sheet.AutoSizeColumn(12);
                sheet.AutoSizeColumn(13);
                sheet.AutoSizeColumn(14);
                sheet.AutoSizeColumn(15);
                sheet.AutoSizeColumn(16);
                sheet.AutoSizeColumn(17);
                sheet.AutoSizeColumn(18);

                //ExcelHandle.exportExcel(workbook, "FB2DH050_RPT." + type);
                return workbook;
            }
            else
                return null;
        }
        catch (Exception)
        {

            throw;
        }
        finally
        {
            sheet = null;
        }

    }



    public DataTable getSalaryStatus(string emp_id, string iflow_no)
    {
        CFB2DH0500DAO wfb2dh = new CFB2DH0500DAO();
        try
        {
            return wfb2dh.getSalaryStatus(emp_id, iflow_no);
        }
        catch (Exception)
        {

            throw;
        }
    }



}