using FB2.tw.co.toyota.kuozui.bo;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

/// <summary>
/// CFB2DF0300BO 的摘要描述
/// </summary>
public class CFB2DF0300BO : BaseService
{
	public CFB2DF0300BO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    public void checkSalaryClose(CFB2DF0300DAO dao)
    {

        try
        {
            DataTable dt = dao.checkSalaryClose();

            if (dt.Rows.Count > 0)
            {
                dao.SALARY_LOCKED = dt.Rows[0]["SALARY_LOCKED"].ToString();
            }

        }
        catch (Exception)
        {
            throw;
        }
    }

    public string getManagerDT(string MANAGER_YM)
    {
        string TAKE_OUT_DT = "", STATUS = "", errormessage = "";
        try
        {
            CFB2DF0300DAO dao = new CFB2DF0300DAO();
            DataTable dt = dao.getManagerDT(MANAGER_YM);

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    TAKE_OUT_DT = dt.Rows[i]["SALARY_TAKE_OUT_DT"].ToString().Replace("-", "/");
                    STATUS = dt.Rows[i]["STATUS"].ToString();

                    if (TAKE_OUT_DT != "9999/12/31" && STATUS == "Y")
                    {
                        errormessage = "0";
                        return errormessage;
                    }
                }

            }

            return errormessage;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public string insertMonth(CFB2DF0300DAO dao)
    {
        try
        {
            string m_START_DT = "";//主檔住宿日
            string m_END_DT = "";//主檔退宿日   
            string m_AMOUNT = "";//主檔住宿費
            string m_BASE_NO = "";//主檔基準
            string m_OTHER_AMOUNT = "";//主檔其他費
            string h_START_DT = "";//歷史檔住宿日
            string h_END_DT = "";//歷史檔退宿日  
            string h_AMOUNT = "";//歷史檔住宿費
            string h_BASE_NO = "";//歷史檔基準
            string h_OTHER_AMOUNT = "";//歷史檔其他費
            
            dao.CREATED_BY = SessionHandle.Current.emp_id;
            dao.UPDATED_BY = SessionHandle.Current.emp_id;
            dao.FUNC_ID = "FB2DF030";

            DataTable dt = dao.getAccomMain();

            if (dt.Rows.Count > 0)
            {
                BeginTransaction();

                dao.deleteACCOM_MONTH();
                //刪除薪資月結控制檔
                dao.deleteSALARY_MONTH_CTRL();

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dao.EMP_ID = dt.Rows[i]["EMP_ID"].ToString();
                    //if (dao.EMP_ID == "24584")
                    //{
                    //    string g1 = "";
                    //}
                    //if (dao.EMP_ID == "26350")
                    //{
                    //    string g1 = "";
                    //}
                    dao.START_DT = dt.Rows[i]["START_DT"].ToString();
                   
                    m_START_DT = dt.Rows[i]["START_DT"].ToString();
                    m_END_DT = dt.Rows[i]["END_DT"].ToString() == "" ? "9999/12/31" : dt.Rows[i]["END_DT"].ToString();
                    m_AMOUNT = dt.Rows[i]["AMOUNT"].ToString();
                    m_OTHER_AMOUNT = dt.Rows[i]["OTHER_AMOUNT"].ToString();
                    m_BASE_NO = dt.Rows[i]["BASE_NO"].ToString();
                    //到歷史檔看是否有資料是 住宿主檔生效日等於歷史檔退宿日期
                    DataTable dt1 = dao.getHistory();
                    if (dt1.Rows.Count > 0)
                    {
                        h_START_DT = dt1.Rows[0]["START_DT"].ToString();
                        h_END_DT = dt1.Rows[0]["END_DT"].ToString() == "" ? "9999/12/31" : dt1.Rows[0]["END_DT"].ToString();
                        h_AMOUNT = dt1.Rows[0]["AMOUNT"].ToString();
                        h_BASE_NO = dt1.Rows[0]["BASE_NO"].ToString();                      

                        //開始判斷
                        //退宿日 = 計算年月的1號 (2015/01/01)  則不計算
                        string rr = Convert.ToString(DateTime.Parse(m_END_DT).ToString("dd"));
                        if (Convert.ToString(DateTime.Parse(m_END_DT).ToString("dd")) == "01" && Convert.ToInt32(DateTime.Parse(m_END_DT).ToString("yyyyMM")) == Convert.ToInt32(dao.MANAGER_YM))
                        {
                            continue;
                        }

                        //主檔.住宿日是否 > 計算年月
                        if (Convert.ToInt32(DateTime.Parse(m_START_DT).ToString("yyyyMM")) > Convert.ToInt32(dao.MANAGER_YM))
                        {
                            //以歷史檔為準
                            dao.START_DT = dt1.Rows[0]["START_DT"].ToString();
                            dao.END_DT = dt1.Rows[0]["END_DT"].ToString() == "" ? "9999/12/31" : dt1.Rows[0]["END_DT"].ToString();
                            dao.AMOUNT = dt1.Rows[0]["AMOUNT"].ToString();
                            dao.OTHER_AMOUNT = dt1.Rows[0]["OTHER_AMOUNT"].ToString();

                            getMoney(dao, dao.AMOUNT, dao.OTHER_AMOUNT, dao.START_DT, dao.END_DT);
                            
                            dao.NEW_AMOUNT = dao.TEMP_AMOUNT;
                            dao.NEW_OTHER_AMOUNT = dao.TEMP_OTHER_AMOUNT;
                            dao.TOTAL_AMOUNT = Convert.ToString(Convert.ToInt32(dao.NEW_AMOUNT) + Convert.ToInt32(dao.NEW_OTHER_AMOUNT));
                        }
                        else if (Convert.ToInt32(DateTime.Parse(m_START_DT).ToString("yyyyMM")) < Convert.ToInt32(dao.MANAGER_YM))//主檔.住宿日.年月 < 計算年月
                        {
                            dao.START_DT = dt.Rows[i]["START_DT"].ToString();
                            dao.END_DT = dt.Rows[i]["END_DT"].ToString() == "" ? "9999/12/31" : dt.Rows[i]["END_DT"].ToString();
                            dao.AMOUNT = dt.Rows[i]["AMOUNT"].ToString();
                            dao.OTHER_AMOUNT = dt.Rows[i]["OTHER_AMOUNT"].ToString();

                            getMoney(dao, dao.AMOUNT, dao.OTHER_AMOUNT, dao.START_DT, dao.END_DT);

                            dao.NEW_AMOUNT = dao.TEMP_AMOUNT;
                            dao.NEW_OTHER_AMOUNT = dao.TEMP_OTHER_AMOUNT;
                            dao.TOTAL_AMOUNT = Convert.ToString(Convert.ToInt32(dao.NEW_AMOUNT) + Convert.ToInt32(dao.NEW_OTHER_AMOUNT));
                        }
                        else if (Convert.ToInt32(DateTime.Parse(m_START_DT).ToString("yyyyMM")) == Convert.ToInt32(dao.MANAGER_YM))//主檔.住宿日.年月 = 計算年月
	                    {
                            //主檔住宿基準 = 歷史檔住宿基準
                            if (m_BASE_NO == h_BASE_NO || m_AMOUNT == h_AMOUNT)
                            {
                                dao.START_DT = dt1.Rows[0]["START_DT"].ToString();//歷史檔生效日
                                dao.END_DT = dt.Rows[i]["END_DT"].ToString() == "" ? "9999/12/31" : dt.Rows[i]["END_DT"].ToString();//主檔退宿日
                                dao.AMOUNT = dt.Rows[i]["AMOUNT"].ToString();
                                dao.OTHER_AMOUNT = dt.Rows[i]["OTHER_AMOUNT"].ToString();

                                getMoney(dao, dao.AMOUNT, dao.OTHER_AMOUNT, dao.START_DT, dao.END_DT);

                                dao.NEW_AMOUNT = dao.TEMP_AMOUNT;
                                dao.NEW_OTHER_AMOUNT = dao.TEMP_OTHER_AMOUNT;
                                dao.TOTAL_AMOUNT = Convert.ToString(Convert.ToInt32(dao.NEW_AMOUNT) + Convert.ToInt32(dao.NEW_OTHER_AMOUNT));
                            }
                            else
                            {
                                if (m_BASE_NO == "2")//延長住宿
                                {
                                    string tt = Convert.ToString(DateTime.Parse(m_START_DT).ToString("dd"));//測一下
                                    if (Convert.ToString(DateTime.Parse(m_START_DT).ToString("dd")) == "01")//主檔.住宿日.日期 = 1，本月生效
                                    {
                                        dao.START_DT = dt.Rows[i]["START_DT"].ToString();
                                        dao.END_DT = dt.Rows[i]["END_DT"].ToString() == "" ? "9999/12/31" : dt.Rows[i]["END_DT"].ToString();
                                        dao.AMOUNT = dt.Rows[i]["AMOUNT"].ToString();
                                        dao.OTHER_AMOUNT = dt.Rows[i]["OTHER_AMOUNT"].ToString();

                                        //getMoney(dao, dao.AMOUNT, dao.OTHER_AMOUNT, dao.START_DT, dao.END_DT);

                                        dao.NEW_AMOUNT = dao.AMOUNT;
                                        dao.NEW_OTHER_AMOUNT = dao.OTHER_AMOUNT;
                                        dao.TOTAL_AMOUNT = Convert.ToString(Convert.ToInt32(dao.NEW_AMOUNT) + Convert.ToInt32(dao.NEW_OTHER_AMOUNT));
                                    }
                                    else //次月生效，以歷史檔為準
                                    {
                                        dao.START_DT = dt1.Rows[0]["START_DT"].ToString();
                                        dao.END_DT = dt.Rows[i]["END_DT"].ToString() == "" ? "9999/12/31" : dt.Rows[i]["END_DT"].ToString();//用主檔的退宿日來計算
                                        dao.AMOUNT = dt1.Rows[0]["AMOUNT"].ToString();
                                        dao.OTHER_AMOUNT = dt1.Rows[0]["OTHER_AMOUNT"].ToString();

                                        getMoney(dao, dao.AMOUNT, dao.OTHER_AMOUNT, dao.START_DT, dao.END_DT);

                                        dao.NEW_AMOUNT = dao.TEMP_AMOUNT;
                                        dao.NEW_OTHER_AMOUNT = dao.TEMP_OTHER_AMOUNT;
                                        dao.TOTAL_AMOUNT = Convert.ToString(Convert.ToInt32(dao.NEW_AMOUNT) + Convert.ToInt32(dao.NEW_OTHER_AMOUNT));
                                    }
                                }
                                else //住宿基準 != "2"
                                {
                                    if (Convert.ToInt32(DateTime.Parse(m_START_DT).ToString("dd")) < 15 || Convert.ToInt32(DateTime.Parse(m_END_DT).ToString("dd")) > 15)
                                    {
                                        //總和
                                        dao.NEW_AMOUNT = Convert.ToString(Convert.ToInt32(m_AMOUNT) + 0.5 * Convert.ToInt32(h_AMOUNT));
                                        dao.NEW_OTHER_AMOUNT = m_OTHER_AMOUNT;
                                        dao.TOTAL_AMOUNT = Convert.ToString(Convert.ToInt32(dao.NEW_AMOUNT) + Convert.ToInt32(dao.NEW_OTHER_AMOUNT));
                                    }
                                    if (Convert.ToInt32(DateTime.Parse(m_START_DT).ToString("dd")) > 15 || Convert.ToInt32(DateTime.Parse(m_END_DT).ToString("dd")) < 15)
                                    {
                                        //總和
                                        dao.NEW_AMOUNT = Convert.ToString( 0.5 * Convert.ToInt32(m_AMOUNT) +  Convert.ToInt32(h_AMOUNT));
                                        dao.NEW_OTHER_AMOUNT = m_OTHER_AMOUNT;
                                        dao.TOTAL_AMOUNT = Convert.ToString(Convert.ToInt32(dao.NEW_AMOUNT) + Convert.ToInt32(dao.NEW_OTHER_AMOUNT));
                                    }
                                }
                            }
                        }



                    }
                    else//主檔與歷史檔日期不連續，以主檔為主
                    {
                        if (Convert.ToString(DateTime.Parse(m_END_DT).ToString("dd")) == "01" && Convert.ToInt32(DateTime.Parse(m_END_DT).ToString("yyyyMM")) == Convert.ToInt32(dao.MANAGER_YM))
                        {
                            continue;
                        }

                        dao.START_DT = dt.Rows[i]["START_DT"].ToString();
                        dao.END_DT = dt.Rows[i]["END_DT"].ToString() == "" ? "9999/12/31" : dt.Rows[i]["END_DT"].ToString();
                        dao.AMOUNT = dt.Rows[i]["AMOUNT"].ToString();
                        dao.OTHER_AMOUNT = dt.Rows[i]["OTHER_AMOUNT"].ToString();                        

                        getMoney(dao, dao.AMOUNT, dao.OTHER_AMOUNT, dao.START_DT, dao.END_DT);

                        dao.NEW_AMOUNT = dao.TEMP_AMOUNT;
                        dao.NEW_OTHER_AMOUNT = dao.TEMP_OTHER_AMOUNT;
                        dao.TOTAL_AMOUNT = Convert.ToString(Convert.ToInt32(dao.NEW_AMOUNT) + Convert.ToInt32(dao.NEW_OTHER_AMOUNT));
                    }  

                    dao.insertACCOM_MONTH();

                }//for end

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

    //計算後的住宿費用、其他費用計算
    public void getMoney(CFB2DF0300DAO dao,string amount,string other_amount,string start_dt,string end_dt)
    {
        string tDays = "0";
        try
        {
            if (Convert.ToInt32(amount) != 0 || Convert.ToInt32(other_amount) != 0)
                    {
                        
                        
                        //住宿主檔.住宿日＞０ 且 ( 住宿主檔.退宿日＝０ 或 住宿主檔.退宿日＞畫面.管理年月 )                
                        if (start_dt != "")
                        {
                            if (end_dt == "")
                            {
                                dao.TEMP_AMOUNT = amount;
                                dao.TEMP_OTHER_AMOUNT = other_amount;
                            }
                            else
                            {
                                if (Convert.ToInt32(DateTime.Parse(dao.END_DT).ToString("yyyyMM")) > Convert.ToInt32(dao.MANAGER_YM))
                                {
                                    dao.TEMP_AMOUNT = amount;
                                    dao.TEMP_OTHER_AMOUNT = other_amount;
                                }
                            }
                        }

                        //住宿主檔.住宿日(年月)＞ 畫面.管理年月
                        if (Convert.ToInt32(DateTime.Parse(start_dt).ToString("yyyyMM")) > Convert.ToInt32(dao.MANAGER_YM))
                        {
                            dao.TEMP_AMOUNT = "0";
                            dao.TEMP_OTHER_AMOUNT = "0";
                        }

                        //共幾日
                        if (Convert.ToInt32(DateTime.Parse(start_dt).ToString("yyyyMM")) == Convert.ToInt32(dao.MANAGER_YM) &&
                            Convert.ToInt32(DateTime.Parse(end_dt).ToString("yyyyMM")) == Convert.ToInt32(dao.MANAGER_YM)) //住宿日(年月)＝畫面.管理年月 & 退宿日(年月)＝畫面.管理年月
                        {
                            tDays = DateTime.Parse(end_dt).Subtract(DateTime.Parse(start_dt)).Days.ToString();
                        }
                        else if (Convert.ToInt32(DateTime.Parse(start_dt).ToString("yyyyMM")) < Convert.ToInt32(dao.MANAGER_YM))//住宿日(年月) < 畫面.管理年月
                        {
                            if (end_dt != "")
	                        {                                
                                tDays = Convert.ToString( Convert.ToInt32(DateTime.Parse(end_dt).ToString("dd")));
	                        }
                            
                        }                   
                        
                        if (end_dt != "")
                        {
                            //住宿主檔.住宿日(年月)＝畫面.管理年月 且 住宿主檔.住宿日(日)＞15日 ) 或 ( 住宿主檔.退宿日(年月)＝畫面.管理年月 且 住宿主檔.退宿日(日)＜15日
                            if ((Convert.ToInt32(DateTime.Parse(start_dt).ToString("yyyyMM")) == Convert.ToInt32(dao.MANAGER_YM) && Convert.ToInt32(tDays) > 15)
                             || (Convert.ToInt32(DateTime.Parse(end_dt).ToString("yyyyMM")) == Convert.ToInt32(dao.MANAGER_YM) && Convert.ToInt32(tDays) < 15))
                            {
                                dao.TEMP_AMOUNT = Convert.ToString(Math.Ceiling((Convert.ToDouble(amount)) / 2));
                                dao.TEMP_OTHER_AMOUNT = other_amount;                                
                            }
                            //住宿主檔.住宿日(年月) < 畫面.管理年月 且 住宿主檔.退宿日(年月)＝畫面.管理年月  住宿主檔.退宿日(日) > 15日
                            if ((Convert.ToInt32(DateTime.Parse(start_dt).ToString("yyyyMM")) < Convert.ToInt32(dao.MANAGER_YM) && Convert.ToInt32(DateTime.Parse(end_dt).ToString("yyyyMM")) == Convert.ToInt32(dao.MANAGER_YM)
                                && Convert.ToInt32(tDays) > 15))
                            {
                                dao.TEMP_AMOUNT = amount;
                                dao.TEMP_OTHER_AMOUNT = other_amount;
                            }
                            //住宿主檔.住宿日(年月) < 畫面.管理年月 且 住宿主檔.退宿日(年月)＝畫面.管理年月  住宿主檔.退宿日(日) < 15日
                            if ((Convert.ToInt32(DateTime.Parse(start_dt).ToString("yyyyMM")) < Convert.ToInt32(dao.MANAGER_YM) && Convert.ToInt32(DateTime.Parse(end_dt).ToString("yyyyMM")) == Convert.ToInt32(dao.MANAGER_YM)
                                && Convert.ToInt32(tDays) < 15))
                            {
                                dao.TEMP_AMOUNT = Convert.ToString(Math.Ceiling((Convert.ToDouble(amount)) / 2));
                                dao.TEMP_OTHER_AMOUNT = other_amount;
                            }
                        }
                        


                    }
                    else
                    {
                        dao.TEMP_AMOUNT = "0";
                        dao.TEMP_OTHER_AMOUNT = "0";                        
                    }
        }
        catch (Exception)
        {
            
            throw;
        }
    }

/*
    public string insertMonth(CFB2DF0300DAO dao)
    {        
        try
        {
            dao.CREATED_BY = SessionHandle.Current.emp_id;
            dao.UPDATED_BY = SessionHandle.Current.emp_id;
            dao.FUNC_ID = "FB2DF030";

            DataTable dt = dao.getAccomMain();

            if (dt.Rows.Count > 0)
            {
                BeginTransaction();

                dao.deleteACCOM_MONTH();
                //刪除薪資月結控制檔
                dao.deleteSALARY_MONTH_CTRL();

                for (int i = 0; i < dt.Rows.Count;i++ )
                {                   
                    dao.EMP_ID = dt.Rows[i]["EMP_ID"].ToString();
                    dao.START_DT = dt.Rows[i]["START_DT"].ToString();
                    //到歷史檔看是否有資料是 住宿主檔生效日等於歷史檔退宿日期
                    DataTable dt1 = dao.getHistory();
                    if (dt1.Rows.Count > 0)
                    {
                        dao.START_DT = dt1.Rows[0]["START_DT"].ToString();
                        dao.END_DT = dt1.Rows[0]["END_DT"].ToString() == "" ? "9999/12/31" : dt1.Rows[0]["END_DT"].ToString();
                        dao.AMOUNT = dt1.Rows[0]["AMOUNT"].ToString();
                        dao.OTHER_AMOUNT = dt1.Rows[0]["OTHER_AMOUNT"].ToString();
                    }
                    else
                    {
                        dao.START_DT = dt.Rows[i]["START_DT"].ToString();
                        dao.END_DT = dt.Rows[i]["END_DT"].ToString() == "" ? "9999/12/31" : dt.Rows[i]["END_DT"].ToString();
                        dao.AMOUNT = dt.Rows[i]["AMOUNT"].ToString();
                        dao.OTHER_AMOUNT = dt.Rows[i]["OTHER_AMOUNT"].ToString();
                    }                    

                    dao.NEW_AMOUNT = dao.AMOUNT;
                    dao.NEW_OTHER_AMOUNT = dao.OTHER_AMOUNT;
                    dao.TOTAL_AMOUNT = Convert.ToString(Convert.ToInt32(dao.NEW_AMOUNT) + Convert.ToInt32(dao.NEW_OTHER_AMOUNT));
                    
                    
                    if (Convert.ToInt32(dao.AMOUNT) != 0 || Convert.ToInt32(dao.OTHER_AMOUNT) != 0)
                    {
                        //住宿主檔.住宿日＞０ 且 ( 住宿主檔.退宿日＝０ 或 住宿主檔.退宿日＞畫面.管理年月 )                
                        if (dao.START_DT != "")
                        {
                            if (dao.END_DT == "")
                            {
                                dao.NEW_AMOUNT = dao.AMOUNT;
                                dao.NEW_OTHER_AMOUNT = dao.OTHER_AMOUNT;
                                dao.TOTAL_AMOUNT = Convert.ToString(Convert.ToInt32(dao.NEW_AMOUNT) + Convert.ToInt32(dao.NEW_OTHER_AMOUNT));
                            }
                            else
                            {
                                if (Convert.ToInt32(DateTime.Parse(dao.END_DT).ToString("yyyyMM")) > Convert.ToInt32(dao.MANAGER_YM))
                                {
                                    dao.NEW_AMOUNT = dao.AMOUNT;
                                    dao.NEW_OTHER_AMOUNT = dao.OTHER_AMOUNT;
                                    dao.TOTAL_AMOUNT = Convert.ToString(Convert.ToInt32(dao.NEW_AMOUNT) + Convert.ToInt32(dao.NEW_OTHER_AMOUNT));
                                }
                            }
                        }

                        //住宿主檔.住宿日(年月)＞ 畫面.管理年月
                        if (Convert.ToInt32(DateTime.Parse(dao.START_DT).ToString("yyyyMM")) > Convert.ToInt32(dao.MANAGER_YM))
                        {
                            dao.NEW_AMOUNT = "0";
                            dao.NEW_OTHER_AMOUNT = "0";
                            dao.TOTAL_AMOUNT = "0";
                        }

                        //住宿主檔.住宿日(年月)＝畫面.管理年月 且 住宿主檔.住宿日(日)＞15日 ) 或 ( 住宿主檔.退宿日(年月)＝畫面.管理年月 且 住宿主檔.退宿日(日)＜15日
                        if(dao.END_DT != ""){
                           if ((Convert.ToInt32(DateTime.Parse(dao.START_DT).ToString("yyyyMM")) == Convert.ToInt32(dao.MANAGER_YM) && Convert.ToInt32(DateTime.Parse(dao.START_DT).ToString("dd")) > 15)
                            || (Convert.ToInt32(DateTime.Parse(dao.END_DT).ToString("yyyyMM")) == Convert.ToInt32(dao.MANAGER_YM) && Convert.ToInt32(DateTime.Parse(dao.END_DT).ToString("dd")) < 15))
                            {                               
                                dao.NEW_AMOUNT = Convert.ToString(Math.Ceiling((Convert.ToDouble(dao.AMOUNT)) / 2));
                                dao.NEW_OTHER_AMOUNT = dao.OTHER_AMOUNT;
                                dao.TOTAL_AMOUNT = Convert.ToString(Convert.ToInt32(dao.NEW_AMOUNT) + Convert.ToInt32(dao.NEW_OTHER_AMOUNT));
                            }                            
                        }
                        


                    }
                    else
                    {
                        dao.NEW_AMOUNT = "0";
                        dao.NEW_OTHER_AMOUNT = "0";
                        dao.TOTAL_AMOUNT = "0";
                    }
                    
                    
                    dao.insertACCOM_MONTH();

                }//for end
                             
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
*/
    //產生Excel
    public void createExcel(CFB2DF0300DAO dao, string type)
    {
        try
        {
            IWorkbook workbook;
            ISheet sheet;
            ICellStyle style1;
            ICellStyle style2;
            ICellStyle style3;
            DataTable tmp = dao.selectMonthData();
            if (tmp.Rows.Count > 0)
            {
                if (type == "xls")
                {
                    workbook = new HSSFWorkbook();
                    sheet = (HSSFSheet)workbook.CreateSheet("月度住宿費用檔");
                    style1 = (HSSFCellStyle)workbook.CreateCellStyle();
                    style3 = (HSSFCellStyle)workbook.CreateCellStyle();
                }
                else
                {
                    workbook = new XSSFWorkbook();
                    sheet = workbook.CreateSheet("月度住宿費用檔");
                    style1 = (XSSFCellStyle)workbook.CreateCellStyle();
                    style3 = (XSSFCellStyle)workbook.CreateCellStyle();
                }

                IFont font1 = workbook.CreateFont();
                font1.FontName = "新細明體";
                font1.FontHeightInPoints = 12;
                style1.SetFont(font1);

                style3.Alignment = HorizontalAlignment.Right;
                style3.VerticalAlignment = VerticalAlignment.Center;
                style3.SetFont(font1);

                IRow row = sheet.CreateRow(0);
                ICell cell;
                cell = row.CreateCell(0);
                cell.CellStyle = style1;
                cell.SetCellValue("管理年月");

                cell = row.CreateCell(1);
                cell.CellStyle = style1;
                cell.SetCellValue("工號");

                cell = row.CreateCell(2);
                cell.CellStyle = style1;
                cell.SetCellValue("姓名");

                cell = row.CreateCell(3);
                cell.CellStyle = style1;
                cell.SetCellValue("員工區分");

                cell = row.CreateCell(4);
                cell.CellStyle = style1;
                cell.SetCellValue("職務");

                cell = row.CreateCell(5);
                cell.CellStyle = style1;
                cell.SetCellValue("聘用單位");

                cell = row.CreateCell(6);
                cell.CellStyle = style1;
                cell.SetCellValue("部門代號");

                cell = row.CreateCell(7);
                cell.CellStyle = style1;
                cell.SetCellValue("住宿日");

                cell = row.CreateCell(8);
                cell.CellStyle = style1;
                cell.SetCellValue("退宿日");

                cell = row.CreateCell(9);
                cell.CellStyle = style1;
                cell.SetCellValue("宿舍別");

                cell = row.CreateCell(10);
                cell.CellStyle = style1;
                cell.SetCellValue("房間號碼");

                cell = row.CreateCell(11);
                cell.CellStyle = style1;
                cell.SetCellValue("住宿費");

                cell = row.CreateCell(12);
                cell.CellStyle = style1;
                cell.SetCellValue("其他費用");

                cell = row.CreateCell(13);
                cell.CellStyle = style1;
                cell.SetCellValue("總費用");

                cell = row.CreateCell(14);
                cell.CellStyle = style1;
                cell.SetCellValue("薪資轉出日期");

                cell = row.CreateCell(15);
                cell.CellStyle = style1;
                cell.SetCellValue("薪資轉出人員");

                cell = row.CreateCell(16);
                cell.CellStyle = style1;
                cell.SetCellValue("處理狀態");

                cell = row.CreateCell(17);
                cell.CellStyle = style1;
                cell.SetCellValue("發薪日期");

                cell = row.CreateCell(18);
                cell.CellStyle = style1;
                cell.SetCellValue("新增人員");

                cell = row.CreateCell(19);
                cell.CellStyle = style1;
                cell.SetCellValue("新增日期時間");

                cell = row.CreateCell(20);
                cell.CellStyle = style1;
                cell.SetCellValue("更新人員");

                cell = row.CreateCell(21);
                cell.CellStyle = style1;
                cell.SetCellValue("更新日期時間");

                cell = row.CreateCell(22);
                cell.CellStyle = style1;
                cell.SetCellValue("更新作業FunctionID");



                style2 = workbook.CreateCellStyle();

                style2.SetFont(font1);

                int column1 = 0,column2=0,column3=0;

                int x = 0;
                for (int i = 0; i < tmp.Rows.Count; i++)
                {
                    column1 = Convert.ToInt32( tmp.Rows[i]["AMOUNT"].ToString());
                    column2 = Convert.ToInt32(tmp.Rows[i]["OTHER_AMOUNT"].ToString());
                    column3 = Convert.ToInt32(tmp.Rows[i]["TOTAL_AMOUNT"].ToString());
                    

                    x = i + 1;
                    row = sheet.CreateRow(x);
                    cell = row.CreateCell(0);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["MANAGER_YM"].ToString());

                    cell = row.CreateCell(1);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["EMP_ID"].ToString());


                    cell = row.CreateCell(2);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["EMP_NAME"].ToString());

                    cell = row.CreateCell(3);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["EMP_CD"].ToString());

                    cell = row.CreateCell(4);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["PJOB_CD"].ToString());

                    cell = row.CreateCell(5);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["COMPANY_CD"].ToString());

                    cell = row.CreateCell(6);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["DEPT_NO"].ToString());

                    cell = row.CreateCell(7);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["START_DT"].ToString());

                    cell = row.CreateCell(8);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["END_DT"].ToString());

                    cell = row.CreateCell(9);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["ACCOM_CD"].ToString());

                    cell = row.CreateCell(10);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["ROOM_NO"].ToString());

                    cell = row.CreateCell(11);
                    cell.CellStyle = style3;
                    cell.SetCellValue(column1.ToString("N0"));

                    cell = row.CreateCell(12);
                    cell.CellStyle = style3;
                    cell.SetCellValue(column2.ToString("N0"));

                    cell = row.CreateCell(13);
                    cell.CellStyle = style3;
                    cell.SetCellValue(column3.ToString("N0"));

                    cell = row.CreateCell(14);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["SALARY_TAKE_OUT_DT"].ToString());

                    cell = row.CreateCell(15);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["SALARY_TAKE_OUT_BY"].ToString());

                    cell = row.CreateCell(16);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["STATUS"].ToString());

                    cell = row.CreateCell(17);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["GIVE_SALARY_DT"].ToString());

                    cell = row.CreateCell(18);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["CREATED_BY"].ToString());

                    cell = row.CreateCell(19);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["CREATED_DT"].ToString());

                    cell = row.CreateCell(20);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["UPDATED_BY"].ToString());

                    cell = row.CreateCell(21);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["UPDATED_DT"].ToString());

                    cell = row.CreateCell(22);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["FUNC_ID"].ToString());


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
                ExcelHandle.exportExcel(workbook, "FB2DF030." + type);
            }
        }
        catch
        {
            throw;
        }
    }

    public string getSalaryCode(CFB2DF0300DAO dao)
    {
        string errormessage = "";

        try
        {
            DataTable dt = dao.getSalaryCode();

            if (dt.Rows.Count > 0)
            {
                dao.SALARY_DT = dt.Rows[0]["SALARY_DT"].ToString();
                dao.SALARY_YM = dt.Rows[0]["SALARY_YM"].ToString();
                dao.SALARY_TYPE = dt.Rows[0]["SALARY_TYPE"].ToString();
                dao.SALARY_SDT = dt.Rows[0]["SALARY_SDT"].ToString();
                dao.SALARY_EDT = dt.Rows[0]["SALARY_EDT"].ToString();


            }
            else
            {
                errormessage += "薪資類別尚未建立最新月薪\\n";
                return errormessage; ;
            }

            return errormessage; ;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public DataTable getSalaryCTL(CFB2DF0300DAO dao)
    {

        try
        {
            DataTable dt = dao.getSalaryCTL();

            return dt;


        }
        catch (Exception)
        {
            throw;
        }
    }

    public void update_Month(CFB2DF0300DAO dao)
    {


        try
        {
            BeginTransaction();

            dao.update_Month();

            Commit();

        }
        catch (Exception)
        {
            RollBack();
            throw;
        }
    }

    public string updateSALARY_MONTH_CTRL(CFB2DF0300DAO dao)
    {


        try
        {
            dao.TAKE_OUT_BY = SessionHandle.Current.emp_id;

            BeginTransaction();

            dao.updateSALARY_MONTH_CTRL();

            //更新
            dao.update_MONTH_FIN();

            Commit();
            return "0";

        }
        catch (Exception)
        {
            RollBack();
            throw;
        }
    }

    public string insertSALARY_MONTH_CTRL(CFB2DF0300DAO dao)
    {


        try
        {
            dao.TAKE_OUT_BY = SessionHandle.Current.emp_id;

            BeginTransaction();

            dao.insertSALARY_MONTH_CTRL();

            //更新
            dao.update_MONTH_FIN();

            Commit();
            return "0";

        }
        catch (Exception)
        {
            RollBack();
            throw;
        }
    }
}