using FB2.tw.co.toyota.kuozui.bo;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;


public class billTmpDTL
{
    public String W26H13 { get; set; }
    public String W26H14 { get; set; }
    public String W26H16 { get; set; }
    public String W26H17 { get; set; }
    public String W26H20 { get; set; }
    public String W26H22 { get; set; }
    public String W26H23 { get; set; }
    public String W26H26 { get; set; }  

}

public class billTmpTotal
{
    public String W26H08 { get; set; }
    public String W26H13 { get; set; }
    public String W26H14 { get; set; }
    public String W26H16 { get; set; }
    public String W26H26 { get; set; }   
}



/// <summary>
/// CFB2IB0500BO 的摘要描述
/// </summary>

public class CFB2IB0500BO : BaseService
{
	public CFB2IB0500BO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    public DataTable checkData(string YM)
    {
        try
        {
            CFB2IB0500DAO dao = new CFB2IB0500DAO();

            return dao.selectData(YM);

        }
        catch (Exception)
        {

            throw;
        }
    }

    public string selectMonthData(string YM)
    {
        string errormessage = "";
        try
        {
            CFB2IB0500DAO dao = new CFB2IB0500DAO();
            DataTable dt  = dao.selectMonthData(YM);
            if (dt.Rows.Count == 0)
            {
                errormessage += "雇主補充保費尚未計算\\n";
                return errormessage;
            }

            return errormessage;
            

        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getYM()
    {
        try
        {
            CFB2IB0500DAO dao = new CFB2IB0500DAO();
            DataTable dt = dao.getYM();

            return dt;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public string selectAFT_INS2_COST(string YM)
    {
        try
        {
            CFB2IB0500DAO dao = new CFB2IB0500DAO();
            return dao.selectAFT_INS2_COST(YM);
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public string selectIACYC(string YM)
    {
        try
        {
            CFB2IB0500DAO dao = new CFB2IB0500DAO();
            return dao.selectIACYC(YM);            
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    public string getSQLLNO(CFB2IB0500DAO dao)
    {
        string errormessage = "";
        try
        {
            //CFB2IB0500DAO dao = new CFB2IB0500DAO();
            DataTable dt = dao.getSQLLNO();
            if (dt.Rows.Count > 0)
            {
                string GCM = dt.Rows[0]["GetChveMrtMk"].ToString();//抓入成功註記
                string AWM = dt.Rows[0]["AvWgtcmpsMk"].ToString();//可重作註記
                if (GCM == "Y" && AWM != "Y")
                {
                    errormessage += "雇主補充保費已進入財務系統，不能再重新計算\\n";
                    return errormessage;
                }                
            }            

            return errormessage;

        }
        catch (Exception)
        {

            throw;
        }
    }

    public string getLogFlag(CFB2IB0500DAO dao)
    {
        string errormessage = "";
        try
        {
            //CFB2IB0500DAO dao = new CFB2IB0500DAO();
            DataTable dt = dao.getLogFlag();
            if (dt.Rows.Count > 0)
            {
                string GCM = dt.Rows[0]["GetChveMrtMk"].ToString();//抓入成功註記
                string AWM = dt.Rows[0]["AvWgtcmpsMk"].ToString();//可重作註記
                if (GCM == "Y" && AWM != "Y")
                {
                    errormessage += "雇主補充保費已進入財務系統，不能再重新計算\\n";
                    return errormessage;
                }
            }

            return errormessage;

        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable chk_SQLLNO(CFB2IB0500DAO dao)
    {
        try
        {            
            DataTable dt = dao.getSQLLNO();            
            return dt;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public string checkExcelData(CFB2IB0500DAO dao)
    {
        string errormessage = "";
        try
        {            
            DataTable dt = dao.getSALARY_MONTH_DATA();
            if (dt.Rows.Count == 0)
            {
                errormessage += "此年月無資料可匯出\\n";
                return errormessage;
            }

            return errormessage;


        }
        catch (Exception)
        {

            throw;
        }
    }

    public IWorkbook createExcel(CFB2IB0500DAO dao, string type)
    {
        int a8 = 0;
        int a9 = 0;
        int a10 = 0;
        int a11 = 0;
        int a12 = 0;
        int a13 = 0;
        int a14 = 0;
        //int a15 = 0;
        //int a16 = 0;
        //int a17 = 0;
        //int a18 = 0;



        try
        {
            IWorkbook workbook;
            ISheet sheet;
            ICellStyle style1, style2, style3;

            DataTable dt = dao.getSALARY_MONTH_DATA();

            if (dt.Rows.Count > 0)
            {
                if (type == "xls")
                {
                    workbook = new HSSFWorkbook();
                    sheet = (HSSFSheet)workbook.CreateSheet("補充保費調整後Excel");
                    style1 = (HSSFCellStyle)workbook.CreateCellStyle();
                    style2 = (HSSFCellStyle)workbook.CreateCellStyle();
                    style3 = (HSSFCellStyle)workbook.CreateCellStyle();
                }
                else
                {
                    workbook = new XSSFWorkbook();
                    sheet = workbook.CreateSheet("補充保費調整後Excel");
                    style1 = (XSSFCellStyle)workbook.CreateCellStyle();
                    style2 = (XSSFCellStyle)workbook.CreateCellStyle();
                    style3 = (XSSFCellStyle)workbook.CreateCellStyle();
                }

                //邊框
                style2.BorderBottom = BorderStyle.Dashed;
                style2.BorderTop = BorderStyle.Dashed;                

                IFont font1 = workbook.CreateFont();
                font1.FontName = "新細明體";
                font1.FontHeightInPoints = 12;
                style1.SetFont(font1);
                style2.SetFont(font1);																

                //style3
                style3.Alignment = HorizontalAlignment.Right;
                style3.VerticalAlignment = VerticalAlignment.Center;
                style3.SetFont(font1);

                IRow row ;
                ICell cell;

                //製表日期     
                row = sheet.CreateRow(0);
                cell = row.CreateCell(13);
                cell.CellStyle = style3;
                cell.SetCellValue("製表日期:");

                cell = row.CreateCell(14);
                cell.CellStyle = style3;
                cell.SetCellValue(DateTime.Now.ToString("yyyy/MM/dd"));

                row = sheet.CreateRow(1);
                cell = row.CreateCell(0);
                cell.CellStyle = style2;
                cell.SetCellValue("年月");

                cell = row.CreateCell(1);
                cell.CellStyle = style2;
                cell.SetCellValue("科目");

                cell = row.CreateCell(2);
                cell.CellStyle = style2;
                cell.SetCellValue("直/間接");

                cell = row.CreateCell(3);
                cell.CellStyle = style2;
                cell.SetCellValue("薪資部門");

                cell = row.CreateCell(4);
                cell.CellStyle = style2;
                cell.SetCellValue("工廠");

                cell = row.CreateCell(5);
                cell.CellStyle = style2;
                cell.SetCellValue("大小車");

                cell = row.CreateCell(6);
                cell.CellStyle = style2;
                cell.SetCellValue("負擔部門");

                cell = row.CreateCell(7);
                cell.CellStyle = style2;
                cell.SetCellValue("預算部門");

                cell = row.CreateCell(8);
                cell.CellStyle = style2;
                cell.SetCellValue("員工非固定薪總額");

                cell = row.CreateCell(9);
                cell.CellStyle = style2;
                cell.SetCellValue("員工月薪總額");

                cell = row.CreateCell(10);
                cell.CellStyle = style2;
                //cell.SetCellValue("離職員工非固定薪額");
                cell.SetCellValue("傳票匯入");

                cell = row.CreateCell(11);
                cell.CellStyle = style2;
                //cell.SetCellValue("雇主非固定薪先扣額");
                cell.SetCellValue("本月健保投總額");

                cell = row.CreateCell(12);
                cell.CellStyle = style2;
                //cell.SetCellValue("雇主其他非固定薪額");
                cell.SetCellValue("本月健保投總額N");

                cell = row.CreateCell(13);
                cell.CellStyle = style2;
                //cell.SetCellValue("本月健保投總額");
                cell.SetCellValue("補充保費基準N");

                cell = row.CreateCell(14);
                cell.CellStyle = style2;
                //cell.SetCellValue("本月健保投總額N");
                cell.SetCellValue("補充保險費N");
/*
                cell = row.CreateCell(15);
                cell.CellStyle = style2;
                //cell.SetCellValue("補充保費基準");

                cell = row.CreateCell(16);
                cell.CellStyle = style2;
                //cell.SetCellValue("補充保費基準N");

                cell = row.CreateCell(17);
                cell.CellStyle = style2;
                //cell.SetCellValue("補充保險費");

                cell = row.CreateCell(18);
                cell.CellStyle = style2;
                //cell.SetCellValue("補充保險費N");
*/
                

                //系統日期
                string st = DateTime.Now.ToString("yyyyMMdd");

                int x = 0;
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    //dao.EMP_ID = dt.Rows[i]["EMP_ID"].ToString();
                    //dao.EMP_NAME = dt.Rows[i]["EMP_NAME"].ToString();
                    //dao.EMP_CD = dt.Rows[i]["EMP_CD"].ToString();
                    //dao.DEPT_NO = dt.Rows[i]["DEPT_NO"].ToString();
                    //dao.CLASS_NAME = dt.Rows[i]["CLASS_NAME"].ToString();
                    //dao.POTO = dt.Rows[i]["POTO"].ToString();
                    //dao.CARD_NO = dt.Rows[i]["CARD_NO"].ToString();
                    //dao.ROOM_NO = dt.Rows[i]["ROOM_NO"].ToString();

                    //if (dt.Rows[i]["START_DT"].ToString() == "9999-12-31")
                    //{
                    //    //dao.START_DT = DateTime.Parse(dt.Rows[i]["START_DT"].ToString()).ToString("yyyymMMdd");
                    //    dao.START_DT = "9991231";
                    //}
                    //else
                    //{

                    //    dao.START_DT = chtdate(dt.Rows[i]["START_DT"].ToString());
                    //}

                    //if (dt.Rows[i]["END_DT"].ToString() == "9999-12-31")
                    //{
                    //    //dao.START_DT = DateTime.Parse(dt.Rows[i]["START_DT"].ToString()).ToString("yyyymMMdd");
                    //    dao.END_DT = "9991231";
                    //}
                    //else
                    //{

                    //    dao.END_DT = chtdate(dt.Rows[i]["END_DT"].ToString());
                    //}

                    //dao.CAR = dt.Rows[i]["CAR"].ToString();
                    //dao.MOTOR = dt.Rows[i]["MOTOR"].ToString();
                    //dao.CAR_NO = dt.Rows[i]["CAR_NO"].ToString();
                    //dao.MOTOR_NO = dt.Rows[i]["MOTOR_NO"].ToString();

                    if (dt.Rows[i]["SALARY_YM"].ToString() != "")
                    {
                        dao.YM = Convert.ToString(Convert.ToInt32(dt.Rows[i]["SALARY_YM"].ToString().Substring(0, 4)) - 1911) +
                                 dt.Rows[i]["SALARY_YM"].ToString().Substring(4, 2);
                    }
                    else {
                        dao.YM = "";
                    }                   

                    x = i + 2;
                    row = sheet.CreateRow(x);
                    cell = row.CreateCell(0);
                    cell.CellStyle = style1;
                    cell.SetCellValue(dao.YM);

                    cell = row.CreateCell(1);
                    cell.CellStyle = style1;
                    cell.SetCellValue(dt.Rows[i]["ACC_CD"].ToString());


                    cell = row.CreateCell(2);
                    cell.CellStyle = style1;
                    cell.SetCellValue(dt.Rows[i]["ACC_WS"].ToString());

                    cell = row.CreateCell(3);
                    cell.CellStyle = style1;
                    cell.SetCellValue(dt.Rows[i]["SALARY_DEPT"].ToString());

                    cell = row.CreateCell(4);
                    cell.CellStyle = style1;
                    cell.SetCellValue(dt.Rows[i]["PLANT_CD"].ToString());

                    cell = row.CreateCell(5);
                    cell.CellStyle = style1;
                    cell.SetCellValue(dt.Rows[i]["CAR_KIND"].ToString());

                    cell = row.CreateCell(6);
                    cell.CellStyle = style1;
                    cell.SetCellValue(dt.Rows[i]["COST_DEPT_NO"].ToString());

                    cell = row.CreateCell(7);
                    cell.CellStyle = style1;
                    cell.SetCellValue(dt.Rows[i]["BUDGET_DEPT_NO"].ToString());

                    cell = row.CreateCell(8);
                    cell.CellStyle = style3;
                    //cell.SetCellValue(String.Format("{0:#,0}", Convert.ToInt32(dt.Rows[i]["FLOAT_S_TOTAL"].ToString())));
                    cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["FLOAT_S_TOTAL"].ToString()));
                    a8 = a8 + Convert.ToInt32(dt.Rows[i]["FLOAT_S_TOTAL"].ToString());

                    cell = row.CreateCell(9);
                    cell.CellStyle = style3;
                    cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["MONTH_S_TOTAL"].ToString()));
                    a9 = a9 + Convert.ToInt32(dt.Rows[i]["MONTH_S_TOTAL"].ToString());

                    cell = row.CreateCell(10);
                    cell.CellStyle = style3;
                    cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["BOSS_TAX"].ToString()));
                    a10 = a10 + Convert.ToInt32(dt.Rows[i]["BOSS_TAX"].ToString());

                    cell = row.CreateCell(11);
                    cell.CellStyle = style3;
                    cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["TOTAL_INS"].ToString()));
                    a11 = a11 + Convert.ToInt32(dt.Rows[i]["TOTAL_INS"].ToString());

                    cell = row.CreateCell(12);
                    cell.CellStyle = style3;
                    cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["AFT_INS_TOTAL"].ToString()));
                    a12 = a12 + Convert.ToInt32(dt.Rows[i]["AFT_INS_TOTAL"].ToString());

                    cell = row.CreateCell(13);
                    cell.CellStyle = style3;
                    cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["AFT_INS2_BASE"].ToString()));
                    a13 = a13 + Convert.ToInt32(dt.Rows[i]["AFT_INS2_BASE"].ToString());

                    cell = row.CreateCell(14);
                    cell.CellStyle = style3;
                    cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["AFT_INS2_COST"].ToString()));
                    a14 = a14 + Convert.ToInt32(dt.Rows[i]["AFT_INS2_COST"].ToString());

                    //cell = row.CreateCell(15);
                    //cell.CellStyle = style3;
                    //cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["INS2_BASE"].ToString()));
                    //a15 = a15 + Convert.ToInt32(dt.Rows[i]["INS2_BASE"].ToString());

                    //cell = row.CreateCell(16);
                    //cell.CellStyle = style3;
                    //cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["AFT_INS2_BASE"].ToString()));
                    //a16 = a16 + Convert.ToInt32(dt.Rows[i]["AFT_INS2_BASE"].ToString());

                    //cell = row.CreateCell(17);
                    //cell.CellStyle = style3;
                    //cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["INS2_COST"].ToString()));
                    //a17 = a17 + Convert.ToInt32(dt.Rows[i]["INS2_COST"].ToString());

                    //cell = row.CreateCell(18);
                    //cell.CellStyle = style3;
                    //cell.SetCellValue(Convert.ToInt32(dt.Rows[i]["AFT_INS2_COST"].ToString()));
                    //a18 = a18 + Convert.ToInt32(dt.Rows[i]["AFT_INS2_COST"].ToString());        

                }//for end

                //terry add
                //合計欄位
                x = x + 1;
                row = sheet.CreateRow(x);
                cell = row.CreateCell(7);
                cell.CellStyle = style1;
                cell.SetCellValue("合計");

                cell = row.CreateCell(8);
                cell.CellStyle = style1;
                cell.SetCellValue(a8);

                cell = row.CreateCell(9);
                cell.CellStyle = style1;
                cell.SetCellValue(a9);

                cell = row.CreateCell(10);
                cell.CellStyle = style1;
                cell.SetCellValue(a10);

                cell = row.CreateCell(11);
                cell.CellStyle = style1;
                cell.SetCellValue(a11);

                cell = row.CreateCell(12);
                cell.CellStyle = style1;
                cell.SetCellValue(a12);

                cell = row.CreateCell(13);
                cell.CellStyle = style1;
                cell.SetCellValue(a13);

                cell = row.CreateCell(14);
                cell.CellStyle = style1;
                cell.SetCellValue(a14);

                //cell = row.CreateCell(15);
                //cell.CellStyle = style1;
                //cell.SetCellValue(a15);

                //cell = row.CreateCell(16);
                //cell.CellStyle = style1;
                //cell.SetCellValue(a16);

                //cell = row.CreateCell(17);
                //cell.CellStyle = style1;
                //cell.SetCellValue(a17);

                //cell = row.CreateCell(18);
                //cell.CellStyle = style1;
                //cell.SetCellValue(a18);



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
                //sheet.AutoSizeColumn(15);
                //sheet.AutoSizeColumn(16);
                //sheet.AutoSizeColumn(17);
                //sheet.AutoSizeColumn(18);
                //ExcelHandle.exportExcel(workbook, st + "_FB2IB050." + type);
                return workbook;
            }
            return null;
        }
        catch (Exception)
        {
            throw;
        }
    }


    //檢查能否轉出SAP
    public string chek_SAP_DONE(CFB2IB0500DAO dao)
    {
        string errormessage = "0";
        try
        {
            
            if (dao.chek_SAP_DONE() == "E")
                errormessage = "傳票SAP已立帳,不允執行!";
       
            return errormessage;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //轉出傳票 
    public string transToBill(CFB2IB0500DAO dao)
    {        
        DataTable para_dt  = new DataTable();

        List<INS2_SALARY_MONTH> list = new List<INS2_SALARY_MONTH>();
        List<INS2_SALARY_MONTH> list1 = new List<INS2_SALARY_MONTH>();
        List<billTmpDTL> billTmpDTL = new List<billTmpDTL>();
        List<billTmpTotal> billTmpTotal = new List<billTmpTotal>();        

        try
        {
            dao.CREATED_BY = SessionHandle.Current.emp_id.Trim();
            dao.UPDATED_BY = SessionHandle.Current.emp_id.Trim();
            dao.FUNC_ID = "FB2IB050";

            //取得總投保金額
            dao.getTotalINS();
            //取得補充保險費率－雇主
            dao.getInsPara();
            //取得管理部門
            dao.getManageDept();
            //補充保費代號
            para_dt = utilities.getParameter("IB","INS2");
            if (para_dt.Rows.Count > 0)
            {
                dao.PAY_KIND = para_dt.Rows[0]["CODE_VAL1"].ToString();
            }
            para_dt.Clear();

            //補充保費受款人
            para_dt = utilities.getParameter("IB", "Rpamtpes");
            if (para_dt.Rows.Count > 0)
            {
                dao.Rpamtpes = para_dt.Rows[0]["CODE_VAL1"].ToString();
                dao.Obj = dao.Rpamtpes;
            }
            para_dt.Clear();

            if (dao.Rpamtpes == "")
            {
                dao.Obj = "12488060";
            }            

            //到介接檔取得廠商的相關支付方式與付款條件
            /*para_dt = dao.getPaymentData(dao.Rpamtpes);
            if (para_dt.Rows.Count > 0)
            {
                dao.Padty = para_dt.Rows[0]["Padty"].ToString();
                dao.PayTrm = para_dt.Rows[0]["PayTrm"].ToString();
            }
            else
            {
                dao.Padty = "";
                dao.PayTrm = "";
            }
            */
            dao.Padty = "";
            dao.PayTrm = "";
            para_dt.Clear();

            //薪資發放資料別
            dao.getSys_cd();

            //傳票流水號
            dao.getSEQ2();            

            //轉帳傳票代號:傳票號碼的前2碼
            para_dt = utilities.getParameter("SC", "B_VOUCHER_SEQ1");
            if (para_dt.Rows.Count > 0)
            {
                dao.B_VOUCHER_SEQ1 = para_dt.Rows[0]["CODE_VAL1"].ToString();
            }
            //傳票號碼
            dao.SEQ_NO2 = Convert.ToString(Convert.ToInt32(dao.SEQ_NO2) + 1);
            dao.Vochno = dao.B_VOUCHER_SEQ1 + dao.SYS_CD + (dao.SEQ_NO2).PadLeft(5, '0');

            para_dt.Clear();

            //補充保費負擔部門月度檔.科目=1~3(間接)時，為9222
            para_dt = utilities.getParameter("IB", "BUDGET_C");
            if (para_dt.Rows.Count > 0)
            {
                dao.BUDGET_C = para_dt.Rows[0]["CODE_VAL1"].ToString();
            }
            para_dt.Clear();

            //貸方
            para_dt = utilities.getCommCodeVal("IB","BUDGET_C","1");            
            if (para_dt.Rows.Count > 0)
            {
                dao.BUDGET_C = para_dt.Rows[0]["CODE_VAL1"].ToString();
            }
            para_dt.Clear();

            //借方
            para_dt = utilities.getCommCodeVal("IB", "BUDGET_D", "1");
            if (para_dt.Rows.Count > 0)
            {
                dao.BUDGET_D1 = para_dt.Rows[0]["CODE_VAL1"].ToString();
            }
            para_dt.Clear();
            para_dt = utilities.getCommCodeVal("IB", "BUDGET_D", "2");
            if (para_dt.Rows.Count > 0)
            {
                dao.BUDGET_D2 = para_dt.Rows[0]["CODE_VAL1"].ToString();
            }
            para_dt.Clear();
            para_dt = utilities.getCommCodeVal("IB", "BUDGET_D", "3");
            if (para_dt.Rows.Count > 0)
            {
                dao.BUDGET_D3 = para_dt.Rows[0]["CODE_VAL1"].ToString();
            }
            para_dt.Clear();
            para_dt = utilities.getCommCodeVal("IB", "BUDGET_D", "4");
            if (para_dt.Rows.Count > 0)
            {
                dao.BUDGET_D4 = para_dt.Rows[0]["CODE_VAL1"].ToString();
            }
            para_dt.Clear();

            //入帳日期 2016/08/19 財務變邏輯 TERRY修改
            //DateTime mdt = Convert.ToDateTime(dao.IACYC + "/01");
            //dao.IaDat = ((mdt.AddMonths(1)).AddDays(-1)).ToString("yyyy/MM/dd");

            //入帳日期 20161005 再改成 畫面上的指定日期
            //dao.IaDat = DateTime.Now.ToString("yyyy/MM/dd");

            //para_dt = dao.getLno();
            para_dt = dao.getTB_S_M_VOUCHER_SEQ();
            string tmp_no = "";
            if (para_dt.Rows.Count > 0)
            {
                if ((para_dt.Rows[0]["Lno"].ToString()).Length == 10)
                {
                    if (((para_dt.Rows[0]["Lno"].ToString()).Substring(0, 7)).Equals(dao.SYS_CD + (dao.IACYC.Replace("/", "")).Substring(2, 4)))
                    {
                        tmp_no = (para_dt.Rows[0]["Lno"].ToString()).Substring(7, 3);
                        if (tmp_no == "999")
                        {
                            tmp_no = "0";
                        }
                        dao.Lno = dao.SYS_CD + (dao.IACYC.Replace("/", "")).Substring(2, 4) + Convert.ToString(Convert.ToInt32(tmp_no) + 1).PadLeft(3, '0');
                    }
                    else
                    {
                        dao.Lno = dao.SYS_CD + (dao.IACYC.Replace("/", "")).Substring(2, 4) + "001";
                    }
                }
                else
                {
                    dao.Lno = dao.SYS_CD + (dao.IACYC.Replace("/", "")).Substring(2, 4) + "001";
                }
            }
            else
            {
                dao.Lno = dao.SYS_CD + (dao.IACYC.Replace("/", "")).Substring(2, 4) + "001";
            }

            para_dt.Clear();
            //批號
            //dao.BTSQNO = dao.PAY_KIND + dao.IACYC.Replace("/", "");
            para_dt.Clear();
            //買受人
            para_dt = utilities.getParameter("SC", "Cu");
            if (para_dt.Rows.Count > 0)
            {
                dao.Cu = para_dt.Rows[0]["CODE_VAL1"].ToString();
            }
            para_dt.Clear();

            /* 20160712 財務部確認不需此兩欄位資料 */
            //para_dt = utilities.getParameter("SC", "SC_GETMONEY");
            //if (para_dt.Rows.Count > 0)
            //{
            //    dao.Wtmen = para_dt.Rows[0]["CODE_VAL1"].ToString();//領款人
            //    dao.WtmenNm = dao.getName(dao.Wtmen);//領款人名稱
            //}
            //para_dt.Clear();
            dao.Wtmen = "";
            dao.WtmenNm = "";

            string fno = "1";
            try
            {
                //取得所有資料
                DataTable dt = dao.getSALARY_MONTH();
                DataTable dt_total = dao.getTOTAL_COMPANY_SUMMARY();

                if (dt.Rows.Count > 0)
                {
                    BeginTransaction();
                    //刪除暫存檔資料
                    dao.deleteFB_TB_S_VOUCHER_TEMP();

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dao.YM = dt.Rows[i]["SALARY_YM"].ToString().Trim();
                        dao.BUDGET_DEPT_NO = dt.Rows[i]["BUDGET_DEPT_NO"].ToString().Trim();
                        dao.COST_DEPT_NO = dt.Rows[i]["COST_DEPT_NO"].ToString().Trim();
                        dao.AFT_INS2_COST = dt.Rows[i]["AFT_INS2_COST"].ToString().Trim();
                        dao.ACC_CD = dt.Rows[i]["ACC_CD"].ToString().Trim();
                        if (dao.ACC_CD == "1")
                        {
                            dao.Acct = dao.BUDGET_D1;
                        }
                        if (dao.ACC_CD == "2")
                        {
                            dao.Acct = dao.BUDGET_D2;
                        }
                        if (dao.ACC_CD == "3")
                        {
                            dao.Acct = dao.BUDGET_D3;
                        }
                        if (dao.ACC_CD == "4")
                        {
                            dao.Acct = dao.BUDGET_D4;
                        }
                        if (Convert.ToInt32(dao.AFT_INS2_COST) >= 0)
                        {
                            dao.Dc = "D";
                            
                        }
                        else
                        {
                            dao.Dc = "C";                            
                        }

                        dao.Itm = fno.PadLeft(5, '0');//項次
                        dao.Dp = dao.COST_DEPT_NO;//負擔部門
                        dao.BgDp = dao.BUDGET_DEPT_NO;//預算部門
                        dao.RemSumr = Convert.ToString(Convert.ToInt32(dao.SALARY_YM.Substring(0, 4)) - 1911) + dao.SALARY_YM.Substring(4, 2) + "二代健保雇主補充保費";
                        dao.Relno = dao.Vochno + dao.Itm;//相關號碼
                        dao.VochAmt = Convert.ToString(Math.Abs(Convert.ToInt32(dao.AFT_INS2_COST)));//傳票金額
                        dao.Vochtaxamt = "0";//傳票金額

                        //dao.Padty = "1";//支付方式

                        //新增到介接檔
                        dao.insertTB_S_VOUCHER_TEMP();
                        int uu = Convert.ToInt32(fno);
                        fno = Convert.ToString(Convert.ToInt32(fno) + 1);
                    }

                    //總計
                    dao.AFT_INS2_COST = dt_total.Rows[0]["AFT_INS2_COST"].ToString().Trim();
                    dao.Dc = "C";
                    dao.Acct = dao.BUDGET_C;

                    dao.Itm = fno.PadLeft(5, '0');//項次
                    dao.Dp = "K000000";//負擔部門
                    dao.BgDp = "";//預算部門
                    dao.RemSumr = Convert.ToString(Convert.ToInt32(dao.SALARY_YM.Substring(0, 4)) - 1911) + dao.SALARY_YM.Substring(4, 2) + "二代健保雇主補充保費";
                    dao.Relno = dao.Vochno + dao.Itm;//相關號碼
                    dao.VochAmt = Convert.ToString(Math.Abs(Convert.ToInt32(dao.AFT_INS2_COST)));//傳票金額
                    dao.Vochtaxamt = "0";//傳票稅額                   
                    //dao.Padty = "1";//支付方式

                    dao.insertTB_S_VOUCHER_TEMP();


                    //更新二代健保傳票記錄檔
                    dao.updateBILL_RECORD();

                    //寫入傳票號碼序號檔
                    dao.insertTB_S_M_VOUCHER_SEQ();
                    
                    Commit();
                }

            }
            catch (Exception)
            {
                RollBack();
                throw;
            }
            
            //將資料寫到SAP
            string errMsg = dao.VOUCHER_SAP();
            if (errMsg != "")
                return errMsg;

            /*
            //將資料寫到FF1
            dao.RunSP_I_FF1_VOUCHER();
            //拿FF1 log
            DataTable dt_sp = dao.checkSP();
            if (dt_sp.Rows.Count > 0)
            {
                if (dt_sp.Rows[0]["ERROR_FLAG"].ToString() != "")
                {
                    return dt_sp.Rows[0]["LOG_CONTENT"].ToString();
                }
            }
            else
            {
                return "傳票轉出失敗!!沒有傳送資料到目標資料庫中";
            }
            */
 
                        
               
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();            
            return ex.Message;
        }
    }

    public string billDataUpload(CFB2IB0500DAO dao)
    {
        Double minusResult, DeptRate;
        int sum_1 = 0, sum_2 = 0, sum_3 = 0;
        string Dp = "", baseToIns = "";
        List<INS2_SALARY_MONTH> list = new List<INS2_SALARY_MONTH>();
        List<INS2_SALARY_MONTH> list1 = new List<INS2_SALARY_MONTH>();
        List<billTmpDTL> billTmpDTL = new List<billTmpDTL>();
        List<billTmpTotal> billTmpTotal = new List<billTmpTotal>();
        
        try
        {
            dao.CREATED_BY = SessionHandle.Current.emp_id.Trim();
            dao.UPDATED_BY = SessionHandle.Current.emp_id.Trim();
            dao.FUNC_ID = "FB2IB050";

            //取得總投保金額
            dao.getTotalINS();
            //取得補充保險費率－雇主
            dao.getInsPara();
            //取得管理部門
            dao.getManageDept();
            //薪資部門的管理單位 ,用來補回餘額用
            DataTable Dp_dt = utilities.getParameter("IB", "IB_DEPT");
            if (Dp_dt.Rows.Count > 0)
            {
                Dp = Dp_dt.Rows[0]["CODE_VAL1"].ToString();//B0000
            }

            //取得所有資料
            DataTable dt = dao.getSALARY_MONTH_EXEC();
            if (dt.Rows.Count > 0)
            {
                BeginTransaction();

                for (int i = 0; i < dt.Rows.Count; i++)
                {                   
                   
                    dao.YM = dt.Rows[i]["SALARY_YM"].ToString().Trim();
                    dao.ACC_CD = dt.Rows[i]["ACC_CD"].ToString().Trim();
                    dao.ACC_WS = dt.Rows[i]["ACC_WS"].ToString().Trim();
                    dao.SALARY_DEPT = dt.Rows[i]["SALARY_DEPT"].ToString().Trim();
                    dao.PLANT_CD = dt.Rows[i]["PLANT_CD"].ToString().Trim();
                    dao.CAR_KIND = dt.Rows[i]["CAR_KIND"].ToString().Trim();
                    dao.BUDGET_DEPT_NO = dt.Rows[i]["BUDGET_DEPT_NO"].ToString().Trim();
                    dao.COST_DEPT_NO = dt.Rows[i]["COST_DEPT_NO"].ToString().Trim();
                    dao.FLOAT_S_TOTAL = dt.Rows[i]["FLOAT_S_TOTAL"].ToString().Trim() == "" ? "0" : dt.Rows[i]["FLOAT_S_TOTAL"].ToString().Trim();
                    dao.MONTH_S_TOTAL = dt.Rows[i]["MONTH_S_TOTAL"].ToString().Trim() == "" ? "0" : dt.Rows[i]["MONTH_S_TOTAL"].ToString().Trim();
                    dao.OFFLINE_F_S_TOTAL = dt.Rows[i]["OFFLINE_F_S_TOTAL"].ToString().Trim() == "" ? "0" : dt.Rows[i]["OFFLINE_F_S_TOTAL"].ToString().Trim();
                    dao.BOSS_TAX = dt.Rows[i]["BOSS_TAX"].ToString().Trim() == "" ? "0" : dt.Rows[i]["BOSS_TAX"].ToString().Trim();
                    dao.TOTAL_INS = dt.Rows[i]["TOTAL_INS"].ToString().Trim() == "" ? "0" : dt.Rows[i]["TOTAL_INS"].ToString().Trim();
                    dao.INS2_BASE = dt.Rows[i]["INS2_BASE"].ToString().Trim() == "" ? "0" : dt.Rows[i]["INS2_BASE"].ToString().Trim();
                    dao.INS2_COST = dt.Rows[i]["INS2_COST"].ToString().Trim() == "" ? "0" : dt.Rows[i]["INS2_COST"].ToString().Trim();
                    
                    double q1 = Convert.ToInt32(dao.AFT_TOTAL) - Convert.ToInt32(dao.ori_Total);
                    double q2 = Convert.ToDouble(dao.TOTAL_INS) / Convert.ToDouble(dao.ori_Total);

                    minusResult = Convert.ToDouble(Convert.ToInt32(dao.AFT_TOTAL) - Convert.ToInt32(dao.ori_Total));//總差額                         
                    DeptRate = Convert.ToDouble(dao.TOTAL_INS) / Convert.ToDouble(dao.ori_Total);//佔比例
                    dao.AFT_INS_TOTAL = Convert.ToString(Math.Round(minusResult * DeptRate)+Convert.ToInt32(dao.TOTAL_INS));
                    dao.AFT_INS2_BASE = Convert.ToString(Convert.ToInt32(dao.FLOAT_S_TOTAL) + Convert.ToInt32(dao.MONTH_S_TOTAL) +
                                        Convert.ToInt32(dao.BOSS_TAX) - Convert.ToInt32(dao.AFT_INS_TOTAL));
                                       
                    dao.AFT_INS2_COST = Convert.ToString(Math.Round(Convert.ToDouble(Convert.ToInt32(dao.AFT_INS2_BASE) * Convert.ToDouble(dao.INS_RATE_COMP) / 100)));
                    
                    sum_1 = sum_1 + Convert.ToInt32(dao.AFT_INS_TOTAL);//累加調整後本月健保投總額
                    sum_2 = sum_2 + Convert.ToInt32(dao.AFT_INS2_BASE);//累加調整後保費基準
                    sum_3 = sum_3 + Convert.ToInt32(dao.AFT_INS2_COST);//累加調整後保費
                    //存到LIST中
                    list.Add(new INS2_SALARY_MONTH(dao.YM, dao.ACC_CD, dao.ACC_WS, dao.SALARY_DEPT, dao.PLANT_CD,
                                                   dao.CAR_KIND, dao.BUDGET_DEPT_NO,dao.COST_DEPT_NO, dao.FLOAT_S_TOTAL, dao.MONTH_S_TOTAL,
                                                   dao.OFFLINE_F_S_TOTAL, dao.BOSS_TAX, dao.TOTAL_INS, dao.AFT_INS_TOTAL, dao.INS2_BASE, 
                                                   dao.AFT_INS2_BASE,dao.INS2_COST, dao.AFT_INS2_COST));
                }

                
                int s1 = Convert.ToInt32(sum_1) - Convert.ToInt32(dao.AFT_TOTAL);
                baseToIns = Convert.ToString(Math.Round(Convert.ToDouble(sum_2 * Convert.ToDouble(dao.INS_RATE_COMP) / 100)));//調整後應繳補充保費= 調整後保費基準*雇主補充保費率
                int icost = sum_3 - Convert.ToInt32(baseToIns);//調整後應繳補充保費 與 累加調整後保費 差額

                string NEW_AFT_INS_TOTAL = "", NEW_AFT_INS2_BASE = "", NEW_AFT_INS2_COST = "";
                string sum_e_float = "0", sum_e_month = "0", sum_offline = "0", sum_boss_float = "0", sum_boss_other = "0",
                       sum_total_ins = "0", sum_aft_ins_total = "0", sum_ins2_base = "0", sum_aft_ins2_base = "0", sum_ins2_cost = "0",
                       sum_aft_ins2_cost = "0",sum_boss_tax = "0";

                //將餘額s1補到管理部門，並更新到補充保費負擔部門月度檔
                for (int i = 0; i < list.Count; i++)
                {
                    
                    if (dao.ACC.Equals(list[i].ACC_CD.ToString()) && dao.PLANT.Equals(list[i].PLANT_CD.ToString()) &&
                        (dao.ACC_DEPT.Equals(list[i].SALARY_DEPT.ToString()) || Dp.Equals(list[i].SALARY_DEPT.ToString())) && 
                        dao.BUDGET_DEPT.Equals(list[i].BUDGET_DEPT_NO.ToString()) &&
                        dao.CAR.Equals(list[i].CAR_KIND.ToString()) && dao.COST_DEPT.Equals(list[i].COST_DEPT_NO.ToString()))
                    {
                        NEW_AFT_INS_TOTAL = Convert.ToString(Convert.ToInt32(list[i].AFT_INS_TOTAL.ToString()) - s1);
                        NEW_AFT_INS2_BASE = Convert.ToString(Convert.ToInt32(list[i].FLOAT_S_TOTAL.ToString()) + Convert.ToInt32(list[i].MONTH_S_TOTAL.ToString()) +
                                            Convert.ToInt32(list[i].BOSS_TAX.ToString()) + Convert.ToInt32(list[i].OFFLINE_F_S_TOTAL.ToString()) 
                                            - Convert.ToInt32(NEW_AFT_INS_TOTAL));
                        //NEW_AFT_INS2_COST = Convert.ToString(Math.Round(Convert.ToDouble(Convert.ToInt32(NEW_AFT_INS2_BASE) * Convert.ToDouble(dao.INS_RATE_COMP) / 100)));
                        NEW_AFT_INS2_COST = Convert.ToString(
                                                Convert.ToInt32(Convert.ToString(Math.Round(Convert.ToDouble(Convert.ToInt32(NEW_AFT_INS2_BASE) * Convert.ToDouble(dao.INS_RATE_COMP) / 100)))) - icost
                                            );
                    }
                    else
                    {
                        NEW_AFT_INS_TOTAL = list[i].AFT_INS_TOTAL.ToString();
                        NEW_AFT_INS2_BASE = list[i].AFT_INS2_BASE.ToString();
                        NEW_AFT_INS2_COST = list[i].AFT_INS2_COST.ToString();
                    }

                    dao.YM = list[i].YM.ToString();
                    dao.ACC_CD = list[i].ACC_CD.ToString();
                    dao.ACC_WS = list[i].ACC_WS.ToString();
                    dao.SALARY_DEPT = list[i].SALARY_DEPT.ToString();
                    dao.PLANT_CD = list[i].PLANT_CD.ToString();
                    dao.CAR_KIND = list[i].CAR_KIND.ToString();
                    dao.BUDGET_DEPT_NO = list[i].BUDGET_DEPT_NO.ToString();
                    dao.COST_DEPT_NO = list[i].COST_DEPT_NO.ToString();
                    dao.FLOAT_S_TOTAL = list[i].FLOAT_S_TOTAL.ToString();
                    dao.MONTH_S_TOTAL = list[i].MONTH_S_TOTAL.ToString();
                    dao.OFFLINE_F_S_TOTAL = list[i].OFFLINE_F_S_TOTAL.ToString();
                    dao.BOSS_TAX = list[i].BOSS_TAX.ToString();                    
                    dao.TOTAL_INS = list[i].TOTAL_INS.ToString();
                    dao.AFT_INS_TOTAL = NEW_AFT_INS_TOTAL;
                    dao.INS2_BASE = list[i].INS2_BASE.ToString();
                    dao.AFT_INS2_BASE = NEW_AFT_INS2_BASE;
                    dao.INS2_COST = list[i].INS2_COST.ToString();
                    dao.AFT_INS2_COST = NEW_AFT_INS2_COST;


                    //將結果寫回補充保費負擔部門月度檔
                    dao.updateINS2_SALARY_MONTH();

                    //集計金額欄位
                    sum_e_float = Convert.ToString(Convert.ToInt32(sum_e_float) + Convert.ToInt32(dao.FLOAT_S_TOTAL));//員工非固定薪總額
                    sum_e_month = Convert.ToString(Convert.ToInt32(sum_e_month) + Convert.ToInt32(dao.MONTH_S_TOTAL));//員工月薪總額
                    sum_offline = Convert.ToString(Convert.ToInt32(sum_offline) + Convert.ToInt32(dao.OFFLINE_F_S_TOTAL));//離職員工非固定薪額
                    sum_boss_float = "0";//雇主非固定薪先扣額
                    sum_boss_other = "0";//雇主其他非固定薪額
                    sum_boss_tax = Convert.ToString(Convert.ToInt32(sum_boss_tax) + Convert.ToInt32(dao.BOSS_TAX));//雇主代扣補充保費
                    sum_total_ins =  Convert.ToString(Convert.ToInt32(sum_total_ins) + Convert.ToInt32(dao.TOTAL_INS));//本月健保投總額
                    sum_aft_ins_total = Convert.ToString(Convert.ToInt32(sum_aft_ins_total) + Convert.ToInt32(dao.AFT_INS_TOTAL));//調整後本月健保投總額
                    sum_ins2_base = Convert.ToString(Convert.ToInt32(sum_ins2_base) + Convert.ToInt32(dao.INS2_BASE));//補充保費基準
                    sum_aft_ins2_base = Convert.ToString(Convert.ToInt32(sum_aft_ins2_base) + Convert.ToInt32(dao.AFT_INS2_BASE));//調整後補充保費基準
                    sum_ins2_cost = Convert.ToString(Convert.ToInt32(sum_ins2_cost) + Convert.ToInt32(dao.INS2_COST));//補充保險費
                    sum_aft_ins2_cost = Convert.ToString(Convert.ToInt32(sum_aft_ins2_cost) + Convert.ToInt32(dao.AFT_INS2_COST));//調整後補充保險費
                    
                }//for end

                //刪除相同年月的雇主補充保費記錄檔資料
                dao.deleteCOMPANY_SUMMARY();

                //集計後的資料存到雇主補充保費記錄檔
                dao.insertCOMPANY_SUMMARY(sum_e_float, sum_e_month, sum_offline, sum_boss_float, sum_boss_other, sum_total_ins,
                                         sum_aft_ins_total, sum_ins2_base, sum_aft_ins2_base, sum_ins2_cost, sum_aft_ins2_cost,
                                         SessionHandle.Current.emp_id.Trim(), SessionHandle.Current.emp_id.Trim(), "FB2IB050", sum_boss_tax);

                //二代健保傳票記錄檔

                //delete
                dao.deleteBILL_RECORD();

                //insert
                string DEPT_BILL_NO = "#" + dao.SALARY_YM + "001";
                dao.insertBILL_RECORD(dao.YM, sum_aft_ins_total, DEPT_BILL_NO);

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

    public void insertAS400Dtl(DataTable dt)
    {
        try
        {
            CFB2IB0500DAO dao = new CFB2IB0500DAO();
            if(dt.Rows.Count > 0){
                for (int i = 0; i < dt.Rows.Count;i++ )
                {
                    dao.insert26WH_DTL(
                                       dt.Rows[i]["W26H13"].ToString(),
                                       dt.Rows[i]["W26H14"].ToString(),
                                       dt.Rows[i]["W26H16"].ToString(),
                                       dt.Rows[i]["W26H17"].ToString(),
                                       dt.Rows[i]["W26H20"].ToString(),
                                       dt.Rows[i]["W26H22"].ToString(),
                                       dt.Rows[i]["W26H23"].ToString(),
                                       dt.Rows[i]["W26H26"].ToString()
                                      );
                }
            }           

        }
        catch (Exception)
        {
            throw;
        }
    }

    public void DeleteAS400Dtl(List<billTmpDTL> list)
    {
        try
        {
            CFB2IB0500DAO dao = new CFB2IB0500DAO();            

            for (int i = 0; i < list.Count; i++)
            {
                dao.delete26WH_DTL(list[i].W26H13,
                                    list[i].W26H14,
                                    list[i].W26H16,
                                    list[i].W26H17,
                                    list[i].W26H20,
                                    list[i].W26H22,
                                    list[i].W26H23,
                                    list[i].W26H26                                   
                                );
            }

        }
        catch (Exception)
        {
            throw;
        }
    }

    public void insert26WH_Total(DataTable dt)
    {
        try
        {
            CFB2IB0500DAO dao = new CFB2IB0500DAO();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dao.insert26WH_Total(
                                       dt.Rows[i]["W26H08"].ToString(),
                                       dt.Rows[i]["W26H13"].ToString(),
                                       dt.Rows[i]["W26H14"].ToString(),
                                       dt.Rows[i]["W26H16"].ToString(),
                                       dt.Rows[i]["W26H26"].ToString()                                     
                                      );
                }
            }

        }
        catch (Exception)
        {
            throw;
        }
    }

    public void Delete26WH_Total(List<billTmpTotal> list)
    {
        try
        {
            CFB2IB0500DAO dao = new CFB2IB0500DAO();

            for (int i = 0; i < list.Count; i++)
            {
                dao.delete26WH_Total(list[i].W26H08,
                                    list[i].W26H13,
                                    list[i].W26H14,
                                    list[i].W26H16,
                                    list[i].W26H26
                                );
            }

        }
        catch (Exception)
        {
            throw;
        }
    }

    public class INS2_SALARY_MONTH
    {
     private string _YM;
     private string _ACC_CD;
     private string _ACC_WS;
     private string _SALARY_DEPT;
     private string _PLANT_CD;
     private string _CAR_KIND;
     private string _BUDGET_DEPT_NO;
     private string _COST_DEPT_NO;
     private string _FLOAT_S_TOTAL;
     private string _MONTH_S_TOTAL;
     private string _OFFLINE_F_S_TOTAL;
     private string _BOSS_TAX;     
     private string _TOTAL_INS;
     private string _AFT_INS_TOTAL;
     private string _INS2_BASE;
     private string _AFT_INS2_BASE;
     private string _INS2_COST;
     private string _AFT_INS2_COST;
    
     //INS2_SALARY_MONTH
     public INS2_SALARY_MONTH(string YM, string ACC_CD, string ACC_WS, string SALARY_DEPT, string PLANT_CD, string CAR_KIND, string BUDGET_DEPT_NO, string COST_DEPT_NO,string FLOAT_S_TOTAL,
                            string MONTH_S_TOTAL, string OFFLINE_F_S_TOTAL, string BOSS_TAX, string TOTAL_INS, string AFT_INS_TOTAL, string INS2_BASE, string AFT_INS2_BASE,
                            string INS2_COST, string AFT_INS2_COST)  
     {
         _YM = YM;
         _ACC_CD = ACC_CD;
         _ACC_WS = ACC_WS;
         _SALARY_DEPT = SALARY_DEPT;
         _PLANT_CD = PLANT_CD;
         _CAR_KIND = CAR_KIND;
         _BUDGET_DEPT_NO = BUDGET_DEPT_NO;
         _COST_DEPT_NO = COST_DEPT_NO;
         _FLOAT_S_TOTAL = FLOAT_S_TOTAL;
         _MONTH_S_TOTAL = MONTH_S_TOTAL;
         _OFFLINE_F_S_TOTAL = OFFLINE_F_S_TOTAL;
         _BOSS_TAX = BOSS_TAX;        
         _TOTAL_INS = TOTAL_INS;
         _AFT_INS_TOTAL = AFT_INS_TOTAL;
         _INS2_BASE = INS2_BASE;
         _AFT_INS2_BASE = AFT_INS2_BASE;
         _INS2_COST = INS2_COST;
         _AFT_INS2_COST = AFT_INS2_COST;

     }

     public string YM
     {
         set { _YM = value; }
         get { return _YM; }  
     }

     public string ACC_CD
     {
         set { _ACC_CD = value; }
         get { return _ACC_CD; }
     }

     public string ACC_WS
     {
         set { _ACC_WS = value; }
         get { return _ACC_WS; }
     }

     public string SALARY_DEPT
     {
         set { _SALARY_DEPT = value; }
         get { return _SALARY_DEPT; }
     }

     public string PLANT_CD
     {
         set { _PLANT_CD = value; }
         get { return _PLANT_CD; }
     }

     public string CAR_KIND
     {
         set { _CAR_KIND = value; }
         get { return _CAR_KIND; }
     }

     public string BUDGET_DEPT_NO
     {
         set { _BUDGET_DEPT_NO = value; }
         get { return _BUDGET_DEPT_NO; }
     }

     public string COST_DEPT_NO
     {
         set { _COST_DEPT_NO = value; }
         get { return _COST_DEPT_NO; }
     } 
     public string FLOAT_S_TOTAL
     {
         set { _FLOAT_S_TOTAL = value; }
         get { return _FLOAT_S_TOTAL; }
     }

     public string MONTH_S_TOTAL
     {
         set { _MONTH_S_TOTAL = value; }
         get { return _MONTH_S_TOTAL; }
     }

     public string OFFLINE_F_S_TOTAL
     {
         set { _OFFLINE_F_S_TOTAL = value; }
         get { return _OFFLINE_F_S_TOTAL; }
     }

     public string BOSS_TAX
     {
         set { BOSS_TAX = value; }
         get { return _BOSS_TAX; }
     }
            
     public string TOTAL_INS
     {
         set { _TOTAL_INS = value; }
         get { return _TOTAL_INS; }
     }

     public string AFT_INS_TOTAL
     {
         set { _AFT_INS_TOTAL = value; }
         get { return _AFT_INS_TOTAL; }
     }

     public string INS2_BASE
     {
         set { _INS2_BASE = value; }
         get { return _INS2_BASE; }
     }

     public string AFT_INS2_BASE
     {
         set { _AFT_INS2_BASE = value; }
         get { return _AFT_INS2_BASE; }
     }

     public string INS2_COST
     {
         set { _INS2_COST = value; }
         get { return _INS2_COST; }
     }

     public string AFT_INS2_COST
     {
         set { _AFT_INS2_COST = value; }
         get { return _AFT_INS2_COST; }
     }
             

     //public override string ToString()  
     //{  
     //   return string.Format("ID：{0}，Name：{1}，Age：{2}", _ID, _Name, _Age);  
     //}  

 } 





}
