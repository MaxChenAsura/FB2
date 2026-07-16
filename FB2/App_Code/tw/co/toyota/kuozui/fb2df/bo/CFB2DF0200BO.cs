using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;

/// <summary>
/// WFB2DF0200Service 的摘要描述
/// </summary>
public class CFB2DF0200BO : BaseService
{
    public CFB2DF0200BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    //刪除資料
    public string deleteData(List<string> emp_ids)
    {
        CFB2DF0200DAO wfb2df = new CFB2DF0200DAO();
        try
        {
            BeginTransaction();

            foreach (string emp_id in emp_ids)
            {
                //新增一筆到歷史檔
                wfb2df.addHistory(emp_id);
                //刪除主檔資料
                wfb2df.deleteData(emp_id);
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
    //產生Excel
    public IWorkbook createExcel(CFB2DF0200DAO wfb2df, string type)
    {
        try
        {
            IWorkbook workbook;
            ISheet sheet;
            ICellStyle style1;
            ICellStyle style2;
            ICellStyle style3;
            DataTable tmp = wfb2df.searchResult();
            string rtnmessage = "";
            if (tmp.Rows.Count == 0)
            {
                rtnmessage = "查無資料！";
            }
            if (tmp.Rows.Count > 0)
            {
                if (type == "xls")
                {
                    workbook = new HSSFWorkbook();
                    sheet = (HSSFSheet)workbook.CreateSheet("用戶清冊");
                    style1 = (HSSFCellStyle)workbook.CreateCellStyle();
                }
                else
                {
                    workbook = new XSSFWorkbook();
                    sheet = workbook.CreateSheet("用戶清冊");
                    style1 = (XSSFCellStyle)workbook.CreateCellStyle();
                }

                IFont font1 = workbook.CreateFont();
                font1.FontName = "新細明體";
                font1.FontHeightInPoints = 12;
                style1.SetFont(font1);

                //style3
                style3 = workbook.CreateCellStyle();
                style3.Alignment = HorizontalAlignment.Right;
                style3.VerticalAlignment = VerticalAlignment.Center;
                style3.SetFont(font1);

                IRow row = sheet.CreateRow(1);
                ICell cell;
                cell = row.CreateCell(0);
                cell.CellStyle = style1;
                cell.SetCellValue("宿舍別");

                cell = row.CreateCell(1);
                cell.CellStyle = style1;
                cell.SetCellValue("宿舍棟別");

                cell = row.CreateCell(2);
                cell.CellStyle = style1;
                cell.SetCellValue("房間號碼");

                cell = row.CreateCell(3);
                cell.CellStyle = style1;
                cell.SetCellValue("工號");

                cell = row.CreateCell(4);
                cell.CellStyle = style1;
                cell.SetCellValue("姓名");

                cell = row.CreateCell(5);
                cell.CellStyle = style1;
                cell.SetCellValue("員工區分");

                cell = row.CreateCell(6);
                cell.CellStyle = style1;
                cell.SetCellValue("在職區分");

                cell = row.CreateCell(7);
                cell.CellStyle = style1;
                cell.SetCellValue("部門代號");

                cell = row.CreateCell(8);
                cell.CellStyle = style1;
                cell.SetCellValue("輪值別");

                cell = row.CreateCell(9);
                cell.CellStyle = style1;
                cell.SetCellValue("部名");

                cell = row.CreateCell(10);
                cell.CellStyle = style1;
                cell.SetCellValue("年齡");

                cell = row.CreateCell(11);
                cell.CellStyle = style1;
                cell.SetCellValue("入社日");

                cell = row.CreateCell(12);
                cell.CellStyle = style1;
                cell.SetCellValue("入社年資");

                cell = row.CreateCell(13);
                cell.CellStyle = style1;
                cell.SetCellValue("住宿日");

                cell = row.CreateCell(14);
                cell.CellStyle = style1;
                cell.SetCellValue("退宿日");

                cell = row.CreateCell(15);
                cell.CellStyle = style1;
                cell.SetCellValue("住宿費");

                cell = row.CreateCell(16);
                cell.CellStyle = style1;
                cell.SetCellValue("其他費用");

                cell = row.CreateCell(17);
                cell.CellStyle = style1;
                cell.SetCellValue("交通車區分");

                cell = row.CreateCell(18);
                cell.CellStyle = style1;
                cell.SetCellValue("機車牌照");

                cell = row.CreateCell(19);
                cell.CellStyle = style1;
                cell.SetCellValue("汽車牌照");

                cell = row.CreateCell(20);
                cell.CellStyle = style1;
                cell.SetCellValue("戶籍地");

                cell = row.CreateCell(21);
                cell.CellStyle = style1;
                cell.SetCellValue("現籍地");

                cell = row.CreateCell(22);
                cell.CellStyle = style1;
                cell.SetCellValue("聯絡電話");
                //20150603 增加生日  離職日
                cell = row.CreateCell(23);
                cell.CellStyle = style1;
                cell.SetCellValue("生日");

                cell = row.CreateCell(24);
                cell.CellStyle = style1;
                cell.SetCellValue("離職日");

                style2 = workbook.CreateCellStyle();
                style2.SetFont(font1);

                //製表日期
                row = sheet.CreateRow(0);
                cell = row.CreateCell(24);
                cell.CellStyle = style3;
                cell.SetCellValue("製表日期:" + DateTime.Now.ToString("yyyy/MM/dd"));                
                

                int x = 0;
                for (int i = 0; i < tmp.Rows.Count; i++)
                {
                    x = i + 2;
                    row = sheet.CreateRow(x);
                    cell = row.CreateCell(0);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["ACCOM_CD"].ToString());

                    cell = row.CreateCell(1);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["ACCOM_BUILD_CD"].ToString());


                    cell = row.CreateCell(2);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["ROOM_NO"].ToString());

                    cell = row.CreateCell(3);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["EMP_ID"].ToString());

                    cell = row.CreateCell(4);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["EMP_NAME"].ToString());

                    cell = row.CreateCell(5);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["EMP_CD"].ToString());

                    cell = row.CreateCell(6);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["EMP_CHG_CD_DESC"].ToString());

                    cell = row.CreateCell(7);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["DEPT_NO"].ToString());

                    cell = row.CreateCell(8);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["WORK_SHIFT_DESC"].ToString());

                    cell = row.CreateCell(9);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["DEPT_NAME"].ToString());

                    cell = row.CreateCell(10);
                    cell.CellStyle = style3;
                    cell.SetCellValue(tmp.Rows[i]["AGE"].ToString());

                    cell = row.CreateCell(11);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["JOIN_DT"].ToString());

                    cell = row.CreateCell(12);
                    cell.CellStyle = style3;
                    cell.SetCellValue(tmp.Rows[i]["WORK_YEARS"].ToString());

                    cell = row.CreateCell(13);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["START_DT"].ToString());

                    cell = row.CreateCell(14);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["END_DT"].ToString());

                    cell = row.CreateCell(15);
                    cell.CellStyle = style3;
                    cell.SetCellValue(tmp.Rows[i]["AMOUNT"].ToString());

                    cell = row.CreateCell(16);
                    cell.CellStyle = style3;
                    cell.SetCellValue(tmp.Rows[i]["OTHER_AMOUNT"].ToString());

                    cell = row.CreateCell(17);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["BUS_CD"].ToString());

                    cell = row.CreateCell(18);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["MOTOR_NO"].ToString());

                    cell = row.CreateCell(19);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["CAR_NO"].ToString());

                    cell = row.CreateCell(20);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["REGISTER_ADDR"].ToString());

                    cell = row.CreateCell(21);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["CONTACT_ADDR"].ToString());

                    cell = row.CreateCell(22);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["CONTACT_TEL"].ToString());

                    cell = row.CreateCell(23);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["BIRTH_DT"].ToString());

                    cell = row.CreateCell(24);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["LEAVE_DT"].ToString());
                }
                sheet.AutoSizeColumn(0);
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
                sheet.AutoSizeColumn(19);
                sheet.AutoSizeColumn(20);
                sheet.AutoSizeColumn(21);
                sheet.AutoSizeColumn(22);
                sheet.AutoSizeColumn(23);
                sheet.AutoSizeColumn(24);
                //ExcelHandle.exportExcel(workbook, "住宿清冊." + type);
                return workbook;
            }
            return null;
            //else
            //    rtnmessage = "無匯出資料";
            //return rtnmessage;
        }
        catch
        {
            throw;
        }
    }
    //取得修改資料
    public DataTable getData(string emp_id)
    {
        try
        {
            CFB2DF0200DAO wfb2df = new CFB2DF0200DAO();
            wfb2df.EMP_ID = emp_id;
            return wfb2df.getData();
        }
        catch (Exception)
        {

            throw;
        }
    }
    //儲存
    public string saveACCOM_MAIN(CFB2DF0200DAO wfb2df, string mod)
    {
        try
        {
            //取得現有資料
            DataTable tmp = wfb2df.getExistData();
            DataTable empData = wfb2df.getEmpData();
            if (empData.Rows.Count == 0)
            {
                return "此工號未存於人事主檔";
            }
            BeginTransaction();

            //更新模式
            if (mod == "mod")
            {
                //新增歷史檔
                wfb2df.addHistory(wfb2df.EMP_ID);

                //更新
                wfb2df.updateAccom();
            }
            else
            {
                //新增模式
                if (tmp.Rows.Count > 0)
                {
                    //存在資料但尚未退宿
                    if (tmp.Rows[0]["END_DT"].ToString() == "" || tmp.Rows[0]["END_DT"].ToString() == "9999-12-31")
                    {
                        //不更新
                        return "此員工正在住宿中，新增前請先至『修改』中填寫退宿日期並儲存";
                    }
                    else
                    {
                        //已退宿則更新舊資料
                        wfb2df.updateAccom();

                        //新增歷史檔
                        wfb2df.addHistory(wfb2df.EMP_ID);

                    }

                }
                else
                {
                    //不存在資料直接新增
                    wfb2df.addAccom();

                }
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
    //取得住宿費基準檔資料
    public DataTable getAMOUNT()
    {
        try
        {
            CFB2DF0200DAO wfb2df = new CFB2DF0200DAO();
            return wfb2df.getAMOUNT();
        }
        catch (Exception)
        {

            throw;
        }
    }
    //取得員工基本資料
    public DataTable getEMPFile(string emp_id)
    {
        try
        {
            CFB2DF0200DAO wfb2df = new CFB2DF0200DAO();
            wfb2df.EMP_ID = emp_id;
            return wfb2df.getEMPFile();

        }
        catch (Exception)
        {

            throw;
        }
    }
    //檢查是否存在
    public bool checkExist(string EMP_ID)
    {
        try
        {
            CFB2DF0200DAO wfb2df = new CFB2DF0200DAO();
            wfb2df.EMP_ID = EMP_ID;
            DataTable tmp = wfb2df.getData();
            if (tmp.Rows.Count > 0)
                return true;
            else
                return false;
        }
        catch (Exception)
        {

            throw;
        }
    }
    //取得是否需住宿費
    public string getCode_Val(string sub_cd)
    {
        try
        {
            CFB2DF0200DAO wfb2df = new CFB2DF0200DAO();
            DataTable tmp = wfb2df.getCode_Val(sub_cd);
            if (tmp.Rows.Count > 0)
                return tmp.Rows[0]["CODE_VAL1"].ToString();
            else
                return "";
        }
        catch (Exception)
        {

            throw;
        }
    }
    //檢查同房間是否有不同輪值別人員
    public string checkWorkShift(string EMP_ID, string ROOM_NO, string WORK_SHIFT_CD)
    {
        try
        {
            CFB2DF0200DAO wfb2df = new CFB2DF0200DAO();
            DataTable tmp = wfb2df.checkWorkShift(EMP_ID, ROOM_NO);
            if (tmp.Rows.Count > 0)
            {
                for (int i = 0; i < tmp.Rows.Count; i++)
                {
                    if (WORK_SHIFT_CD != tmp.Rows[i]["WORK_SHIFT_CD"].ToString())
                        return "Y";
                }
                return "N";
            }
            else
                return "N";

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

    public DataTable getDEPT_DATA(string dept_no)
    {
        try
        {
            CFB2DH0400DAO dao = new CFB2DH0400DAO();
            return dao.getDEPT_DATA(dept_no);
        }
        catch (Exception)
        {

            throw;
        }
    }
}