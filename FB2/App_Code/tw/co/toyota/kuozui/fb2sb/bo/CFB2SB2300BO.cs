using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using System.Text;
using NPOI.HSSF.Util;
using System.Collections;
/// <summary>
/// CFB2SB2300BO 的摘要描述
/// </summary>
public class CFB2SB2300BO : BaseService
{
    public CFB2SB2300BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public DataTable getDefaultData1(string EMP_ID, string SALARY_ID, string DATA_YM, string SEQ_NO)
    {
        CFB2SB2300DAO dao = new CFB2SB2300DAO();
        try
        {
            return dao.getDefaultData1(EMP_ID, SALARY_ID, DATA_YM, SEQ_NO);
        }
        catch (Exception)
        {

            throw;
        }
    }
    public DataTable getDefaultData2(string EMP_ID, string SALARY_ID, string DATA_YM, string SEQ_NO)
    {
        CFB2SB2300DAO dao = new CFB2SB2300DAO();
        try
        {
            return dao.getDefaultData2(EMP_ID, SALARY_ID, DATA_YM, SEQ_NO);
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
            CFB2SB2300DAO wfb2sb = new CFB2SB2300DAO();
            wfb2sb.EMP_ID = emp_id;
            return wfb2sb.getEMPFile();

        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getSALARYFile(string SALARY_ID)
    {
        try
        {
            CFB2SB2300DAO wfb2sb = new CFB2SB2300DAO();
            wfb2sb.SALARY_ID = SALARY_ID;
            return wfb2sb.getSALARYFile();

        }
        catch (Exception)
        {

            throw;
        }
    }


    public DataTable getSALARYFile(string SALARY_ID, string EMP_ID)
    {
        try
        {
            CFB2SB2300DAO wfb2sb = new CFB2SB2300DAO();
            wfb2sb.SALARY_ID = SALARY_ID;
            wfb2sb.EMP_ID = EMP_ID;
            return wfb2sb.getSALARYFile();

        }
        catch (Exception)
        {

            throw;
        }
    }
    # region Qry

    public System.Data.DataTable getSYS_ID()
    {
        CFB2SB2300DAO wfb2sb = new CFB2SB2300DAO();
        try
        {
            return wfb2sb.getSYS_ID();
        }
        catch (Exception)
        {

            throw;
        }
    }

    public System.Data.DataTable getSYS_ID(string SUB_CD)
    {
        CFB2SB2300DAO wfb2sb = new CFB2SB2300DAO();
        try
        {
            return wfb2sb.getSYS_ID(SUB_CD);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public System.Data.DataTable getModeData(string ID)
    {
        CFB2SB2300DAO wfb2sb = new CFB2SB2300DAO();
        try
        {
            return wfb2sb.getModeData(ID);
        }
        catch (Exception)
        {

            throw;
        }
    }


    public System.Data.DataTable getFUNC_ID(string ID)
    {
        CFB2SB2300DAO wfb2sb = new CFB2SB2300DAO();
        try
        {
            return wfb2sb.getFUNC_ID(ID);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public string deleteData(CFB2SB2300DAO fb2sb)
    {
        try
        {
            CFB2SB2300DAO wfb2sb = new CFB2SB2300DAO();
            BeginTransaction();


            //刪除主檔資料
            fb2sb.deleteData();

            Commit();
            return "0";
        }
        catch (Exception)
        {
            RollBack();
            throw;
        }
    }
    public string deleteTB_S_M_SUBSIDY_DEDU_1_TMP(CFB2SB2300DAO fb2sb)
    {
        try
        {
            CFB2SB2300DAO wfb2sb = new CFB2SB2300DAO();
            BeginTransaction();


            //刪除主檔資料
            fb2sb.deleteTB_S_M_SUBSIDY_DEDU_1_TMP();

            Commit();
            return "0";
        }
        catch (Exception)
        {
            RollBack();
            throw;
        }
    }

    public string updateData(CFB2SB2300DAO fb2sb)
    {
        try
        {
            BeginTransaction();
            fb2sb.updateData();
            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    //取得 最近一次薪資計算年月
    public string getLatestSalaryYM()
    {
        try
        {
            CFB2SB2300DAO wfb2sb = new CFB2SB2300DAO();
            DataTable dt = wfb2sb.getLatestSalaryYM();
            string result = "0";
            if (dt.Rows.Count > 0)
            {
                //result = dt.Rows[0]["SALARY_YM"].ToString();
                result = dt.Rows[0]["SALARY_YM"].ToString() != "" ? dt.Rows[0]["SALARY_YM"].ToString() : "0";
            }
            return result;

        }
        catch (Exception)
        {

            throw;
        }
    }

    public string checkData1(CFB2SB2300DAO fb2sb)
    {
        string result = "";
        //1.檢核1:   
        DataTable dt = fb2sb.getLatestSalaryYM();
        if (dt.Rows.Count > 0)
        {
            string latestSalaryYM = dt.Rows[0]["SALARY_YM"].ToString();
            int sYM = dt.Rows[0]["SALARY_YM"].ToString() != "" ? Convert.ToInt32(dt.Rows[0]["SALARY_YM"].ToString()) : 0;
            int dYM = Convert.ToInt32(fb2sb.DATA_YM);
            if (sYM != 0 && dYM <= sYM)
            {
                result += "此薪資年月已計薪,無法新增\\n";
            }
        }
        return result;
    }

    public string checkData2(CFB2SB2300DAO fb2sb)
    {
        string result = "";
        //檢核2:   
        DataTable dt = fb2sb.getIsLoked();
        if (dt.Rows.Count > 0)
        {
            string salaryLocked = dt.Rows[0]["SALARY_LOCKED"].ToString();
            if (salaryLocked == "Y")
            {
                result += "此薪資年月資料已鎖定,無法新增\\n";
            }
        }
        return result;
    }

    public string addData(CFB2SB2300DAO fb2sb)
    {
        try
        {

            string rtnmessage = "";
            DataTable dt = new DataTable();
            //若需要則要進行邏輯檢查
            string sysYM = DateTime.Now.ToString("yyyy/MM").Replace("/", "");//系統年月
            if (sysYM.Equals(fb2sb.DATA_YM) == false)
            {
                rtnmessage += checkData1(fb2sb);
                rtnmessage += checkData2(fb2sb);
            }

            //檢查是否有權限
            dt = fb2sb.getSubsidyCount();
            if ((int)dt.Rows[0]["resultCount"] == 0)
            {
                rtnmessage += "此薪資項目代號無權限使用,無法新增 \\n";
            }



            //檢查OK更新
            if (rtnmessage == "")
            {
                try
                {
                    BeginTransaction();
                    //取得序號
                    dt = fb2sb.getSeqNO();
                    int seq = dt.Rows[0]["SEQ_NO"].ToString() != "" ? Convert.ToInt32(dt.Rows[0]["SEQ_NO"].ToString()) : 0;
                    fb2sb.SEQ_NO = Convert.ToString(seq + 1);

                    
                    fb2sb.addData();
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
    public string addData1(CFB2SB2300DAO fb2sb)
    {
        try
        {

            string rtnmessage = "";
            DataTable dt = new DataTable();
            //若需要則要進行邏輯檢查
            string sysYM = DateTime.Now.ToString("yyyy/MM").Replace("/", "");//系統年月
            if (sysYM.Equals(fb2sb.DATA_YM) == false)
            {
                rtnmessage += checkData1(fb2sb);
                rtnmessage += checkData2(fb2sb);
            }

            //檢查是否有權限
            dt = fb2sb.getSubsidyCount();
            if ((int)dt.Rows[0]["resultCount"] == 0)
            {
                rtnmessage += "此薪資項目代號無權限使用,無法新增 \\n";
            }



            //檢查OK更新
            if (rtnmessage == "")
            {
                try
                {
                    BeginTransaction();
                    //取得序號
                    dt = fb2sb.getSeqNO();
                    int seq = dt.Rows[0]["SEQ_NO"].ToString() != "" ? Convert.ToInt32(dt.Rows[0]["SEQ_NO"].ToString()) : 0;
                    fb2sb.SEQ_NO = Convert.ToString(seq + 1);
                    
                    fb2sb.addData1();
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


    public string updateExcelData(Stream fs, string type)
    {
        CFB2SB2300DAO dao = new CFB2SB2300DAO();
        try
        {
            bool valid = true;
            IWorkbook workbook;
            //依附檔名判斷要用哪種方式讀取
            if (type == ".xls")
            {
                workbook = new HSSFWorkbook(fs);
            }
            else
            {
                workbook = new XSSFWorkbook(fs);
            }
            //取得sheet
            ISheet sheet = workbook.GetSheetAt(0);
            sheet.SetColumnWidth(4, 40 * 256);

            ICellStyle style1 = workbook.CreateCellStyle();
            IFont font1 = workbook.CreateFont();
            font1.Color = HSSFColor.Red.Index;
            font1.FontHeight = 12;
            font1.FontName = "新細明體";
            style1.VerticalAlignment = VerticalAlignment.Center;
            style1.WrapText = true;

            if (sheet != null)
            {

                string msg = string.Empty;
                string error = string.Empty;
                string salary_id = string.Empty;
                string emp_id = string.Empty;
                string emp_name = string.Empty;
                string CHG_AMT_A = string.Empty;
                string remark = string.Empty;
                string SALARY_YM = string.Empty;
                DataTable dt = new DataTable();

                List<string> id_remark = new List<string>();
                List<int> removerow = new List<int>();
                StringBuilder sb = new StringBuilder();
                StringBuilder ErrMsg = new StringBuilder();
                //巡覽每row的資料第一列為title跳過
                int total = 0;  //全部都對就不用產生檢核錯誤excel
                int r = 0;      //總和為5則寫入資料
               
                //2.以登入者工號 刪除其他加扣款資料上傳暫存檔(TB_S_S_SUBSIDY_TMP)
                BeginTransaction();

                msg = dao.deleteTmp();
                
                //刪除TMP
                //

                //5.WK資料年月= 讀取 共用DB Function FN_S_SALARY_YM 取得 最近一次薪資計算年月 +1
                dt.Clear();

                dt = dao.getLatestSalaryYM();
                SALARY_YM = dt.Rows[0]["SALARY_YM"].ToString().Trim();
                string WKData_YM = string.Empty;
                WKData_YM = Convert.ToDateTime(string.Format("{0}/{1}", SALARY_YM.Substring(0, 4), SALARY_YM.Substring(4, 2))).AddMonths(1).ToString("yyyyMM");
                for (int i = 1; i <= sheet.LastRowNum; i++)
                {
                    r = 0;
                    if (sheet.GetRow(i) != null)
                    {
                        ErrMsg.Clear();
                        //讀取cell資料，
                        emp_id = sheet.GetRow(i).GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString();
                        emp_name = sheet.GetRow(i).GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                        CHG_AMT_A = sheet.GetRow(i).GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString();
                        remark = sheet.GetRow(i).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString();
                        salary_id = sheet.GetRow(i).GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString();

                        //檢核工號
                        dt.Clear();
                        dt = dao.getEMPFile(emp_id);
                        if (!string.IsNullOrWhiteSpace(emp_id))
                        {
                            if (dt.Rows.Count > 0)
                            {
                                r = r + 1;
                            }
                            else
                            {
                                //此工號不存在,無法新增
                                ErrMsg.Append("此工號不存在,無法新增!\n");
                            }
                        }
                        else
                        { //工號不可空白
                            ErrMsg.Append("工號不可空白!\n");
                        }
                        //檢核姓名
                        if (!string.IsNullOrWhiteSpace(emp_name))
                        {
                            if (dt.Rows.Count > 0)
                            {
                                string name = dt.Rows[0]["EMP_NAME"].ToString().Trim();
                                if (name == emp_name)
                                {
                                    r = r + 1;
                                }
                                else
                                {
                                    //此工號與姓名不相符,無法新增
                                    ErrMsg.Append("此工號與姓名不相符,無法新增!\n");
                                }
                            }
                        }
                        else
                        { //姓名不可空白
                            ErrMsg.Append("姓名不可空白!\n");
                        }

                        //檢核加扣款金額
                        if (!string.IsNullOrWhiteSpace(CHG_AMT_A))
                        {
                            if (IsNumber(CHG_AMT_A))
                            {
                                r = r + 1;
                            }
                            else
                            {
                                //加扣款金額必須為數字,且不可為負數!
                                ErrMsg.Append("加扣款金額必須大於零!\n");
                            }
                        }
                        else
                        { //加扣款金額不可空白
                            ErrMsg.Append("加扣款金額不可空白!\n");
                        }
                        //檢核備註說明
                        if (!string.IsNullOrWhiteSpace(remark))
                        {
                            r = r + 1;
                        }
                        else
                        { //檢核備註說明不可空白
                            ErrMsg.Append("檢核備註說明不可空白!\n");
                        }

                        //薪資項目
                        dao.SALARY_ID = salary_id;
                        dao.EMP_ID = SessionHandle.Current.emp_id;
                        dt = dao.getSALARYFile();
                        if (string.IsNullOrEmpty(salary_id))
                        {
                            ErrMsg.Append("薪資項目代號不可空白!\n");
                        }
                        else if (dt.Rows.Count == 0)
                        {
                            ErrMsg.Append("此薪資項目代號無權限使用,無法新增!\n");
                        }
                        else
                        {
                            r = r + 1;
                        }

                        /*
                         * 20150302 此段已不需要
                        string idremark = string.Format("{0}{1}", emp_id, remark);
                        if (!id_remark.Contains(idremark))
                        {
                            r = r + 1;
                            id_remark.Add(string.Format("{0}{1}", emp_id, remark));
                        }
                        else {
                            // 2015.2.12 湯姐拿掉
                            //此筆資料已重複輸入! 
                            //ErrMsg.Append("此筆資料已重複輸入!\n");
                        }
                         */
                        style1.SetFont(font1);
                        sheet.GetRow(i).CreateCell(5).CellStyle = style1;
                        sheet.GetRow(i).GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(ErrMsg.ToString());

                        //BeginTransaction();

                        if (msg == "0")
                        {
                            dao.EMP_ID = emp_id;
                            dao.SALARY_ID = salary_id;
                            dao.SEQ_NO = Convert.ToString(i - 1);
                            dao.REMARK = remark;
                            dao.AMOUNT = CHG_AMT_A == "" ? "0" : CHG_AMT_A;
                            dao.OP_MSG = ErrMsg.ToString();
                            //3.依EXCEL資料內容 逐筆 新增至其他加扣款資料上傳暫存檔(TB_S_S_SUBSIDY_TMP)
                            dao.addExcelData();

                        }


                        //if (r == 5)
                        if (r == 5)
                        {
                            total = total + 1;
                        }
                        //Commit();
                    }
                }

                if (total != sheet.LastRowNum)
                {
                    sheet.GetRow(0).GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue("檢核錯誤說明");
                    ExcelHandle.exportExcel(workbook, "檢核錯誤說明" + type);
                }
                else
                {
                    //6.若資料無錯誤時,依EXCEL資料內容 逐筆 新增至其他加扣款暫存檔(TB_S_M_SUBSIDY_DEDU_1_TMP)
                    dao.DATA_YM = WKData_YM;


                    
                    for (int i = 1; i <= sheet.LastRowNum; i++)
                    {
                        
                        emp_id = sheet.GetRow(i).GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString();
                        emp_name = sheet.GetRow(i).GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString();
                        CHG_AMT_A = sheet.GetRow(i).GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString();
                        remark = sheet.GetRow(i).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString();
                        salary_id = sheet.GetRow(i).GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString();
                        dao.EMP_ID = emp_id;

                        dt.Clear();
                        dt = dao.getSeqNO();
                        int seq = dt.Rows[0]["SEQ_NO"].ToString() != "" ? Convert.ToInt32(dt.Rows[0]["SEQ_NO"].ToString()) : 0;
                        dao.SEQ_NO = Convert.ToString(seq + 1);
                        dao.CHG_AMT_A = CHG_AMT_A == "" ? "0" : CHG_AMT_A;
                        dao.REMARK = remark;
                        dao.SALARY_ID = salary_id;
                        dao.CREATED_BY = SessionHandle.Current.emp_id;
                        dao.UPDATED_BY = SessionHandle.Current.emp_id;
                        dao.CHG_STATUS = "N";
                                               

                        //開始新增
                        dao.addData();                        
                    }
                    
                }
                Commit();
            }
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();

            return ex.Message;
        }
    }
    public bool IsNumber(string Number)
    {
        bool b = false;
        int result;
        if (int.TryParse(Number, out result))
        {
            if (result > 0)
            {
                b = true;
            }
        }
        return b;
    }

    public string testData()
    {
        try
        {

            CFB2SB2300DAO dao = new CFB2SB2300DAO();
            BeginTransaction();

            for (int i = 0; i < 10; i++)
            {
                dao.EMP_ID = "10001";
                dao.EMP_NAME = "1013";
                dao.SEQ_NO = Convert.ToString(i);
                dao.REMARK = "123";
                dao.AMOUNT = "1231";

                dao.testData();
            }

            //檢查OK更新
            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    #endregion
}