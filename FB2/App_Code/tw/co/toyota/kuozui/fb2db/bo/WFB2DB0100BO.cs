using FB2.tw.co.toyota.kuozui.bo;
using log4net;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;

/// <summary>
/// WFB2DB0100BO 的摘要描述
/// </summary>
public class WFB2DB0100BO : BaseService
{
    private WFB2DB0100DL dl = null;
    ICellStyle style_class;

    public WFB2DB0100BO()
    {
        dl = new WFB2DB0100DL();
    }

    public DataTable GetGridData(int startRowIndex, int maximumRows, string calendar_cd, string WORK_SHIFT_CD, string WORK_SHIFT_DESC, string is_valid, string WORK_DAY_CD, string is_iflow_show, string sortExpression)
    {
        return dl.GetGridData(startRowIndex, maximumRows, calendar_cd, WORK_SHIFT_CD, WORK_SHIFT_DESC, is_valid, WORK_DAY_CD, is_iflow_show, sortExpression);
    }

    public int GetGridDataCount(int startRowIndex, int maximumRows, string calendar_cd, string WORK_SHIFT_CD, string WORK_SHIFT_DESC, string is_valid, string WORK_DAY_CD, string is_iflow_show)
    {
        return dl.GetGridDataCount(startRowIndex, maximumRows, calendar_cd, WORK_SHIFT_CD, WORK_SHIFT_DESC, is_valid, WORK_DAY_CD, is_iflow_show);
    }

    public bool DeleteItem(List<WFB2DB0100DAO> DelItems, out string Message)
    {
        this.BeginTransaction();
        foreach (WFB2DB0100DAO item in DelItems)
        {
            if (dl.Check_EMP_DATA(item, true) > 0)
            {
                Message = Resources.Resource.wfb2db_WORK_SHIFT_H_Mapped_EmpProfile_Message;
                return false;
            }
            else
            {
                if (dl.Check_WORK_SHIFT_EMP_DAY_DUTY(item, true) > 0)
                {
                    Message = Resources.Resource.wfb2db_WORK_SHIFT_H_Mapped_DayDuiy_Message;
                    return false;
                }
            }
        }
        bool ReturnValue = dl.Del_WORK_SHIFT_H_WORK_SHIFT_D(DelItems, true, out Message);
        if (ReturnValue)
            this.Commit();
        else
            this.RollBack();
        return ReturnValue;
    }

    public bool InsertItem(WFB2DB0100DAO InsertItem, out String Message)
    {
        int CheckDataCount = dl.Check_WORK_SHIFT_H_By_Key(InsertItem, false, out Message);
        if (CheckDataCount == 0)
            return dl.Insert_WORK_SHIFT_H(InsertItem, false, out Message);
        else
        {
            Message = Resources.Resource.wfb2db_WORK_SHIFT_CD_Already_Message;
            return false;
        }
    }

    public bool UpdateItem(WFB2DB0100DAO updateItem, out string Message)
    {
        Message = string.Empty;
        if (updateItem.IS_VALID == "N")
        {
            if (dl.Check_EMP_DATA(updateItem, false) > 0)
            {
                Message = Resources.Resource.wfb2db_NotSettingIS_VALID_N_Message;
                return false;
            }
        }
        return dl.Update_WorkShiftH(updateItem, false, out Message);
    }

    public WFB2DB0100DAO GetSingleWORK_SHIFTData(string strWORK_SHIFTY, string StartDate, string EndDate, out string Message)
    {
        Message = string.Empty;
        WFB2DB0100DAO ReturnValue = null;
        List<WFB2DB0100DAO> QueryData = dl.GetTB_D_M_WORK_SHIFT_H(strWORK_SHIFTY, false, out Message);
        if (string.IsNullOrEmpty(Message))
        {
            if (QueryData.Count == 1)
            {
                ReturnValue = QueryData[0];
                if (string.IsNullOrEmpty(StartDate) == false && string.IsNullOrEmpty(EndDate) == false)
                {
                    ReturnValue.Dtl = dl.GetTB_D_M_WORK_SHIFT_D(strWORK_SHIFTY, StartDate, EndDate, false, out Message);
                    if (string.IsNullOrEmpty(Message))
                        return ReturnValue;
                }
                else
                    return ReturnValue;
            }
        }
        return ReturnValue;
    }


    public WFB2DB0100DAO GetWorkShiftH(WFB2DB0100DAO InputDao, out string Message)
    {
        Message = string.Empty;
        WFB2DB0100DAO QueryData = dl.GetWorkShiftH(InputDao, false, out Message);
        if (string.IsNullOrEmpty(Message))
            return QueryData;
        return null;
    }

    //複製輪值表
    public bool CopyWORK_SHIFT(WFB2DB0100DAO Source, WFB2DB0100DAO Destination, string StartCALENDAR_DT, String EndCALENDAR_DT, out string Message)
    {
        Message = string.Empty;
        this.BeginTransaction();
        bool isNeedDelShiftD = false;    //是否需要刪除 輪值表明細檔

        try
        {
            //判斷 輪值表主檔是否存在>0:存在,==0:不存在
            if (dl.Check_WORK_SHIFT_H_By_Key(Destination, true, out Message) == 0)
            {
                //不存在,新增 輪值表主檔
                if (dl.Insert_WORK_SHIFT_H(Destination, true, out Message)==false)
                {
                    this.RollBack();
                    return false;
                }
            }
            else {
                //判斷輪值表明細檔是否存在,若存在,需先刪除
                if (dl.Check_WORK_SHIFT_D_By_Key(Destination.WORK_SHIFT_CD.ToUpper(), StartCALENDAR_DT, EndCALENDAR_DT, true, out Message) > 0)
                {
                    isNeedDelShiftD = true;
                }
            }

            //判斷是否需刪除輪值表明細檔
            if (isNeedDelShiftD) {
                if (dl.delete_WORK_SHIFT_D_By_Key(Destination, out Message) == false)
                {
                    this.RollBack();
                    return false;
                }
            }

            //新增 輪值表明細檔
            if (dl.Insert_WORK_SHIFT_D(Destination, out Message) == false)
            {
                this.RollBack();
                return false;
            }
            this.Commit();

            //修改班表及日勤務狀態檔reopen,因為工程巨大(會timeout),故用SP
            dl.execSP_D_WORK_SHIFT_COPY(Destination);

            return true;

            /*
            if (dl.Check_WORK_SHIFT_H_By_Key(Destination, true, out Message) == 0)
            {
                if (dl.Check_WORK_SHIFT_D_By_Key(Source.WORK_SHIFT_CD.ToUpper(), StartCALENDAR_DT, EndCALENDAR_DT, true, out Message) > 0)
                {
                    if (dl.Insert_WORK_SHIFT_H(Destination, true, out Message))
                    {
                        Destination.Dtl = dl.GetTB_D_M_WORK_SHIFT_D(Source.WORK_SHIFT_CD.ToUpper(), StartCALENDAR_DT, EndCALENDAR_DT, true, out Message);
                        foreach (WFB2DB0100DtlDAO dtl in Destination.Dtl)
                        {
                            dtl.WORK_SHIFT_CD = Destination.WORK_SHIFT_CD.ToUpper();
                            dtl.CREATED_BY = Destination.CREATED_BY;
                            dtl.CREATED_DT = Destination.CREATED_DT;
                            dtl.UPDATED_BY = Destination.UPDATED_BY;
                            dtl.UPDATED_DT = Destination.UPDATED_DT;
                        }
                        if (dl.Insert_WORK_SHIFT_D(Destination, true, out Message))
                        {
                            this.Commit();
                            return true;
                        }
                        else
                        {
                            this.RollBack();
                            return false;
                        }
                    }
                    else
                    {
                        this.RollBack();
                        return false;
                    }
                }
                else
                {
                    Message = Resources.Resource.wfd2da_Work_SHIFT_NotFound;
                    return false;
                }
            }
            else
            {
                Message = Resources.Resource.wfb2db_WORK_SHIFT_CD_Already_Message;
                return false;
            }
            */

        }
        catch (Exception ex)
        {
            this.RollBack();
            Message += ex.ToString();
            return false;
        }
    }

    public bool DtlSave(WFB2DB0100DAO dao, out string Message)
    {
        this.BeginTransaction();
        Message = string.Empty;
        bool ReturnValue = true;
        if (ReturnValue)
            ReturnValue = dl.Save_WORK_SHIFT_D(dao, true, out Message);
        /* 20170704 GRID的修改不要去異動到  TB_D_M_EMP_DAY_DUTY(日勤務班表資料檔)  
        foreach (WFB2DB0100DtlDAO dtl in dao.Dtl)
        {
            //改寫成直接執行SQL後
            try
            {
                utilities.UPD_EMP_DAY_DUTY3(dl.GetdbConn, string.Empty, dtl.SHIFT_CD, dao.WORK_SHIFT_CD, dtl.CALENDAR_DT, dtl.CALENDAR_DT, SessionHandle.Current.emp_id);
                ReturnValue = true;
            }
            catch (Exception ex)
            {
                ReturnValue = false;
                Message = ex.Message;
            }
        }
        */
        if (ReturnValue)
            this.Commit();
        else
            this.RollBack();

        return ReturnValue;
    }

    public List<WFB2DA0100DAO> getCALENDAR_Data(WFB2DA0100DAO dao)
    {
        string Message = string.Empty;
        try
        {
            WFB2DA0100DL dl = new WFB2DA0100DL();

            if (dao == null)
                return dl.getdll_CALENDAR_Data();
            else
            {
                List<WFB2DA0100DAO> returnValue = dl.GetTB_D_M_CALENDAR_H(dao.CALENDAR_CD, false, out Message);
                if (string.IsNullOrEmpty(Message))
                    return returnValue;
                else
                    throw new Exception(Message);
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }

    public string getWorkDayDesc(string SHIFT_CD, string WORK_SHIFTymd)
    {
        return dl.getWorkDayDesc(SHIFT_CD, WORK_SHIFTymd);
    }
    public DataTable getAllWorkShiftH()
    {
        return dl.getAllWorkShiftH();
    }

    public DataTable getCALENDAR_WORK_DAY_CD(string calendar_cd, string ym)
    {
        return dl.getCALENDAR_WORK_DAY_CD(calendar_cd, ym);
    }

    #region "循環規則代碼-Set"

    //取得循環規則說明
    public DataTable getRuleDesc(string ruleCD)
    {
        return dl.getRuleDesc(ruleCD);
    }

    //新增 循環規則代碼
    public string insertSetData(WFB2DB0100DAO db010DAO)
    {
        string rtnmessage = "";
        try
        {
            //DataTable dt = new DataTable();
            //rtnmessage = checkExistSomeST(sg010DAO, rtnmessage);

            //檢查OK更新
            if (rtnmessage == "")
            {
                try
                {
                    BeginTransaction();

                    dl.insertSetData(db010DAO);
                    dl.updateSetDescData(db010DAO);
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

    //修改 循環規則代碼
    public string updateSetData(WFB2DB0100DAO db010DAO)
    {
        string rtnmessage = "";
        try
        {
            //DataTable dt = new DataTable();
            //rtnmessage = checkExistSomeST(sg010DAO, rtnmessage);

            //檢查OK更新
            if (rtnmessage == "")
            {
                try
                {
                    BeginTransaction();

                    dl.updateSetData(db010DAO);
                    dl.updateSetDescData(db010DAO);
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

    //刪除循環規則代碼
    public string deleteSetData(List<Tuple<string, string>> keysList)
    {
        WFB2DB0100DL db010DAO = new WFB2DB0100DL();
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
                        db010DAO.deleteSetData(item.Item1, item.Item2);
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

    #endregion

    #region "循輪值表生產-Grant"

    //取得 循環規則代碼 
    public DataTable getRuleCD()
    {
        WFB2DB0100DL db010DAO = new WFB2DB0100DL();
        try
        {
            return db010DAO.getRuleCD();
        }
        catch (Exception)
        {

            throw;
        }
    }

    //輪值表生成區間起日 是否為已計薪的考勤日期迄日
    public bool checkIsSalaryDate(WFB2DB0100DAO db010DAO)
    {
        WFB2DB0100DL db010DAODL = new WFB2DB0100DL();
        bool result = false;
        try
        {
            DataTable dt = db010DAODL.checkIsSalaryDate(db010DAO);
            if (dt.Rows.Count > 0)
            {
                result = Convert.ToBoolean(dt.Rows[0]["isSalaryDate"]);
            }
            else
            {
                result = true;
            }
            return result;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //輪值表生成區間起日 是否為已計薪的考勤日期迄日
    public bool checkDutyCount(WFB2DB0100DAO db010DAO)
    {
        WFB2DB0100DL db010DAODL = new WFB2DB0100DL();
        bool result = false;
        try
        {
            DataTable dt = db010DAODL.checkDutyCount(db010DAO);
            if ((int)dt.Rows[0]["resultCount"] > 0)
            {
                result = false;
            }
            else
            {
                result = true;
            }
            return result;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //輪值表生成
    public string execSP_D_GEN_WORK_SHIFT_D(WFB2DB0100DAO db010DAO)
    {
        string rtnmessage = "";//存在檢查後的訊息
        WFB2DB0100DL db010DAODL = new WFB2DB0100DL();
        try
        {

            //檢查完成後，逐筆進行刪除
            if (rtnmessage == "")
            {
                //BeginTransaction();
                db010DAODL.execSP_D_GEN_WORK_SHIFT_D(db010DAO);

                rtnmessage += utilities.getSPLOG("SP_D_GEN_WORK_SHIFT_D");
                if (rtnmessage != "")
                {
                    return rtnmessage;
                }
                return "0";
                //Commit();
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
    #endregion


    public IWorkbook uploadExcel1(Stream fs, string type, WFB2DB0100DL dao)
    {
        //取得登入者
        string userid = SessionHandle.Current.emp_id;
        //要載入的資料表名稱
        string tableName = "TB_D_M_WORK_SHIFT_D";

        bool valid = true;
        DataTable myTable = new DataTable("myTable");
        DataTable EMP_DAY_DUTY_dt = new DataTable();
        DataTable excel_dt = new DataTable();
        string[] excel_pk;
        DataTable WORK_SHIFT_CD_dt = new DataTable();
        DataTable parts_dt = new DataTable();
        DataTable SHIFT_CD_dt = new DataTable();
        DataTable dt = new DataTable();
        WFB2DB0100DAO dao2 = new WFB2DB0100DAO();
        DataTable SHIFT_H = new DataTable();
        try
        {
            string error = "";
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
            ICellStyle style1 = workbook.CreateCellStyle();
            IFont font1 = workbook.CreateFont();

            font1.Color = HSSFColor.Red.Index;

            if (sheet != null)
            {
                #region 建立 DataTable

                //建立 DataTable
                DataRow myRow;

                //建立 FieldSchema 
                myTable.Columns.Add("WORK_SHIFT_CD", System.Type.GetType("System.String"));
                myTable.Columns.Add("CALENDAR_DT", System.Type.GetType("System.DateTime"));
                myTable.Columns.Add("SHIFT_CD", System.Type.GetType("System.String"));
                myTable.Columns.Add("CREATED_BY", System.Type.GetType("System.String"));
                myTable.Columns.Add("CREATED_DT", System.Type.GetType("System.DateTime"));
                myTable.Columns.Add("UPDATED_BY", System.Type.GetType("System.String"));
                myTable.Columns.Add("UPDATED_DT", System.Type.GetType("System.DateTime"));
                myTable.Columns.Add("FUNC_ID", System.Type.GetType("System.String"));

                DataRow EMP_DAY_DUTY_Row;
                EMP_DAY_DUTY_dt.Columns.Add("WORK_SHIFT_CD", System.Type.GetType("System.String"));
                EMP_DAY_DUTY_dt.Columns.Add("CALENDAR_DT", System.Type.GetType("System.String"));
                EMP_DAY_DUTY_dt.Columns.Add("SHIFT_CD_N", System.Type.GetType("System.String"));
                EMP_DAY_DUTY_dt.Columns.Add("SHIFT_CD_O", System.Type.GetType("System.String"));

                #endregion

                #region 建立excel PK值

                DataRow excel_row;
                excel_dt.Columns.Add("WORK_SHIFT_CD", System.Type.GetType("System.String"));
                excel_dt.Columns.Add("CALENDAR_DT", System.Type.GetType("System.String"));
                #endregion

                if (sheet.LastRowNum != 0)
                {
                    #region 取得輪值表主檔的PK值
                    WORK_SHIFT_CD_dt = dao.getAll_WORK_SHIFT_CD();
                    WORK_SHIFT_CD_dt.PrimaryKey = new DataColumn[] { WORK_SHIFT_CD_dt.Columns["WORK_SHIFT_CD"] };

                    #endregion
                    //#region 取得班別主檔的PK值
                    //SHIFT_CD_dt = dao.getAll_SHIFT_CD();
                    //SHIFT_CD_dt.PrimaryKey = new DataColumn[] { SHIFT_CD_dt.Columns["SHIFT_CD"] };

                    //#endregion

                }

                //巡覽每row的資料第一列為title跳過
                for (int i = 1; i <= sheet.LastRowNum; i++)
                {
                    if (sheet.GetRow(i) != null)
                    {
                        error = "";
                        excel_pk = new string[2];

                        #region 讀取cell資料，第一欄為檢核結果欄位跳過 

                        dao2.WORK_SHIFT_CD = sheet.GetRow(i).GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().ToUpper();
                        dao2.CALENDAR_DT = sheet.GetRow(i).GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().ToUpper();
                        dao2.SHIFT_CD_N = sheet.GetRow(i).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().ToUpper();
                        dao2.SHIFT_CD_O = sheet.GetRow(i).GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().ToUpper();

                        excel_pk[0] = dao2.WORK_SHIFT_CD;
                        excel_pk[1] = dao2.CALENDAR_DT;
                        #endregion

                        #region 檢核基本邏輯
                        //長度檢核
                        error += utilities.checkLength(dao2.WORK_SHIFT_CD, "輪值表代碼", 2, false);
                        error += utilities.checkLength(dao2.SHIFT_CD_N, "新班別代碼", 2, false);
                        error += utilities.checkLength(dao2.SHIFT_CD_O, "原班別代碼", 2, true);
                        //日期檢核
                        error += utilities.checkDateFormat(dao2.CALENDAR_DT, "勤務日期", false);

                        //格式檢核
                        //A.輪值表
                        DataRow dr;
                        if (dao2.WORK_SHIFT_CD != "")
                        {
                            //存在否 輪值表主檔 
                            dr = WORK_SHIFT_CD_dt.Rows.Find(dao2.WORK_SHIFT_CD);
                            if (dr == null)
                            {
                                error += "輪值表 不存在輪值表主檔\n";
                            }
                        }

                        //B.班別
                        if (dao2.SHIFT_CD_N != "")
                        {
                            //存在否 班別主檔
                            dt = dao.getTB_D_M_SHIFT_H(dao2.SHIFT_CD_N, dao2.CALENDAR_DT);
                            if (dt.Rows.Count == 0)
                            {
                                error += "原班別 不存在班別主檔\n";
                            }
                        }
                        if (dao2.SHIFT_CD_O != "")
                        {
                            //存在否 班別主檔
                            dt = dao.getTB_D_M_SHIFT_H(dao2.SHIFT_CD_O, dao2.CALENDAR_DT);
                            if (dt.Rows.Count == 0)
                            {
                                error += "新班別 不存在班別主檔\n";
                            }
                        }
                        if (dao2.SHIFT_CD_N != "" && dao2.SHIFT_CD_N == dao2.SHIFT_CD_O)
                        {
                            error += "新班別不可等於原班別\n";
                        }

                        //excel的PK值
                        if (excel_dt.Rows.Count > 0)
                        {
                            dr = excel_dt.Rows.Find(excel_pk);
                            if (dr != null)
                            {
                                error += "此EXCEL有相同的 輪值表代碼+勤務日期\n";
                            }
                            else
                            {
                                #region 建立excel PK值資料

                                excel_row = excel_dt.NewRow();
                                excel_row["WORK_SHIFT_CD"] = dao2.WORK_SHIFT_CD;
                                excel_row["CALENDAR_DT"] = dao2.CALENDAR_DT;
                                excel_dt.Rows.Add(excel_row);

                                excel_dt.PrimaryKey =
                                new DataColumn[] { 
                                    excel_dt.Columns["WORK_SHIFT_CD"], 
                                    excel_dt.Columns["CALENDAR_DT"]
                                };

                                #endregion
                            }
                        }
                        else
                        {
                            #region 建立excel PK值資料

                            excel_row = excel_dt.NewRow();
                            excel_row["WORK_SHIFT_CD"] = dao2.WORK_SHIFT_CD;
                            excel_row["CALENDAR_DT"] = dao2.CALENDAR_DT;
                            excel_dt.Rows.Add(excel_row);

                            excel_dt.PrimaryKey =
                            new DataColumn[] { 
                                    excel_dt.Columns["WORK_SHIFT_CD"], 
                                    excel_dt.Columns["CALENDAR_DT"]
                                };

                            #endregion
                        }

                        #endregion

                        //傳出錯誤訊息
                        style1.SetFont(font1);
                        sheet.GetRow(i).CreateCell(0).CellStyle = style1;
                        sheet.GetRow(i).GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(error);
                        if (error != "")
                        {
                            valid = false;
                        }

                        if (valid)
                        {
                            #region 建立資料

                            // 建立資料
                            myRow = myTable.NewRow();
                            myRow["WORK_SHIFT_CD"] = dao2.WORK_SHIFT_CD;
                            myRow["CALENDAR_DT"] = dao2.CALENDAR_DT;
                            myRow["SHIFT_CD"] = dao2.SHIFT_CD_N;
                            myRow["CREATED_BY"] = userid;
                            myRow["CREATED_DT"] = DateTime.Now;
                            myRow["UPDATED_BY"] = userid;
                            myRow["UPDATED_DT"] = DateTime.Now;
                            myRow["FUNC_ID"] = "FB2DB010";
                            myTable.Rows.Add(myRow);

                            EMP_DAY_DUTY_Row = EMP_DAY_DUTY_dt.NewRow();
                            EMP_DAY_DUTY_Row["WORK_SHIFT_CD"] = dao2.WORK_SHIFT_CD;
                            EMP_DAY_DUTY_Row["CALENDAR_DT"] = dao2.CALENDAR_DT;
                            EMP_DAY_DUTY_Row["SHIFT_CD_N"] = dao2.SHIFT_CD_N;
                            EMP_DAY_DUTY_Row["SHIFT_CD_O"] = dao2.SHIFT_CD_O;
                            EMP_DAY_DUTY_dt.Rows.Add(EMP_DAY_DUTY_Row);
                            #endregion
                        }

                    } //if end
                } //for end

                if (sheet.LastRowNum == 0)
                {
                    error = "請輸入上傳資料\n";
                    style1.SetFont(font1);
                    sheet.GetRow(0).CreateCell(0).CellStyle = style1;
                    //傳出錯誤訊息  
                    sheet.GetRow(0).GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(error);
                    if (error != "")
                    {
                        valid = false;
                    }
                }

                if (!valid)
                {
                    myTable.Clear();
                    excel_dt.Clear();

                    //檢核有錯，匯出附加說明的excel
                    return workbook;
                    //檢核有錯，匯出附加說明的excel
                    //ExcelHandle.exportExcel(workbook, "檢核錯誤說明" + type);
                }
                else
                {
                    try
                    {
                        BeginTransaction();
                        //刪除相同KEY的舊檔
                        if (excel_dt.Rows.Count < 1000)
                        {
                            //內送參數最多2100個
                            dao.deleteAll_TB_D_M_WORK_SHIFT_D(excel_dt);
                        }
                        else
                        {
                            int flag = 0;
                            #region 建立暫存excel PK值

                            DataRow parts_row;
                            parts_dt.Columns.Add("WORK_SHIFT_CD", System.Type.GetType("System.String"));
                            parts_dt.Columns.Add("CALENDAR_DT", System.Type.GetType("System.String"));
                            #endregion

                            for (int i = 0; i < excel_dt.Rows.Count; i++)
                            {
                                flag++;

                                #region 建立暫存excel PK值資料

                                parts_row = parts_dt.NewRow();
                                parts_row["WORK_SHIFT_CD"] = excel_dt.Rows[i]["WORK_SHIFT_CD"];
                                parts_row["CALENDAR_DT"] = excel_dt.Rows[i]["CALENDAR_DT"];
                                parts_dt.Rows.Add(parts_row);
                                #endregion

                                if (flag == 1000)
                                {
                                    dao.deleteAll_TB_D_M_WORK_SHIFT_D(parts_dt);

                                    flag = 0;

                                    #region 建立暫存excel PK值
                                    parts_dt = new DataTable();
                                    parts_dt.Columns.Add("WORK_SHIFT_CD", System.Type.GetType("System.String"));
                                    parts_dt.Columns.Add("CALENDAR_DT", System.Type.GetType("System.String"));
                                    #endregion

                                    continue;
                                }
                            }

                            if (flag != 0)
                            {
                                dao.deleteAll_TB_D_M_WORK_SHIFT_D(parts_dt);
                            }
                            parts_dt.Clear();
                        }

                        Commit();

                        //新增輪值表明細檔
                        //使用SqlBulkCopy
                        dao.WriteToDatabase(tableName, myTable);

                        //3.若原班別代碼有值，則要去修改  日勤務班表資料 ,
                        //可參考 utilities.UPD_EMP_DAY_DUTY3(…)的寫法
                        BeginTransaction();
                        for (int k = 0; k < EMP_DAY_DUTY_dt.Rows.Count; k++)
                        {
                            dao2 = new WFB2DB0100DAO();

                            if (EMP_DAY_DUTY_dt.Rows[k]["SHIFT_CD_O"].ToString() != "")
                            {
                                dao2.WORK_SHIFT_CD = EMP_DAY_DUTY_dt.Rows[k]["WORK_SHIFT_CD"].ToString();
                                dao2.CALENDAR_DT = EMP_DAY_DUTY_dt.Rows[k]["CALENDAR_DT"].ToString();
                                dao2.SHIFT_CD_N = EMP_DAY_DUTY_dt.Rows[k]["SHIFT_CD_N"].ToString();
                                dao2.SHIFT_CD_O = EMP_DAY_DUTY_dt.Rows[k]["SHIFT_CD_O"].ToString();

                                SHIFT_H = dao.getSHIFT_H(
                                    EMP_DAY_DUTY_dt.Rows[k]["SHIFT_CD_N"].ToString(),
                                    EMP_DAY_DUTY_dt.Rows[k]["CALENDAR_DT"].ToString()
                                    );

                                if (SHIFT_H.Rows.Count > 0)
                                {
                                    dao2.SHIFT_TIME_CD = SHIFT_H.Rows[0]["SHIFT_TIME_CD"].ToString();
                                    dao2.WORK_HOUR = SHIFT_H.Rows[0]["WORK_HOUR"].ToString();
                                    dao2.WORK_PERIOD_HOUR = SHIFT_H.Rows[0]["WORK_PERIOD_HOUR"].ToString();
                                    //dao2.DUTY_STIME = dao.getDUTY_TIME(dao2.CALENDAR_DT, SHIFT_H.Rows[0]["DUTY_STIME"].ToString());
                                    //dao2.DUTY_ETIME = dao.getDUTY_TIME(dao2.CALENDAR_DT, SHIFT_H.Rows[0]["DUTY_ETIME"].ToString());
                                    dao2.DUTY_STIME = SHIFT_H.Rows[0]["DUTY_STIME"].ToString();
                                    dao2.DUTY_ETIME = SHIFT_H.Rows[0]["DUTY_ETIME"].ToString();
                                    dao2.WORK_SHIFT_ALLOWANCE_TYPE = SHIFT_H.Rows[0]["WORK_SHIFT_ALLOWANCE_TYPE"].ToString();
                                }
                                else
                                {
                                    dao2.SHIFT_TIME_CD = "";
                                    dao2.WORK_HOUR = "";
                                    dao2.WORK_PERIOD_HOUR = "";
                                    dao2.DUTY_STIME = "";
                                    dao2.DUTY_ETIME = "";
                                    dao2.WORK_SHIFT_ALLOWANCE_TYPE = "";
                                }

                               
                                dao.updateEMP_DAY_DUTY(dao2);
                                dao.updateEMP_DUTY_CHECK_STATUS(dao2);
                            }
                        }
                        Commit();
                        
                    }
                    catch (Exception ex)
                    {
                        RollBack();
                        throw;
                    }
                }
                myTable.Clear();
                excel_dt.Clear();
                parts_dt.Clear();
            }
            return null;
        }
        catch (Exception)
        {

            throw;
        }
        finally
        {
            myTable.Clear();
            excel_dt.Clear();
            WORK_SHIFT_CD_dt.Clear();
            parts_dt.Clear();
            SHIFT_CD_dt.Clear();
            dt.Clear();
        }
    }


    public DataTable getTB_D_M_WORK_SHIFT_D_t(WFB2DB0100DAO dao)
    {
        try
        {
            return dl.getTB_D_M_WORK_SHIFT_D_t(dao);
        }
        catch (Exception)
        {
            
            throw;
        }
    }

    public IWorkbook createDownloadData(string excelPath, WFB2DB0100DAO dao, DataTable dt)
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

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        x = i + 1;//從第1列開始insert 資料
                        //將資料寫入範本
                        row = sheet.CreateRow(x);

                        //輪值表代碼
                        cell = row.CreateCell(1);
                        cell.CellStyle = stringLeftStyle;  //先
                        cell.SetCellValue(Convert.ToString(dt.Rows[i]["WORK_SHIFT_CD"]));//後
                        //勤務日期							
                        cell = row.CreateCell(2);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(Convert.ToString(dt.Rows[i]["CALENDAR_DT"]));
                        //新班別代碼
                        cell = row.CreateCell(3);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(Convert.ToString(dt.Rows[i]["SHIFT_CD"]));
                        //原班別代碼
                        cell = row.CreateCell(4);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(Convert.ToString(dt.Rows[i]["SHIFT_CD"]));

                    }
                    ////製表日期
                    //ICellStyle stringLeftStyleDate = this.setCellStyle(workbook, "left", false);
                    //row = sheet.GetRow(0);
                    //cell = row.CreateCell(5);
                    //cell.CellStyle = stringLeftStyleDate;
                    //cell.SetCellValue("製表日期:" + DateTime.Now.ToString("yyyy/MM/dd"));

                }
                return workbook;
            }
            return null;
        }
        catch
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


    public DataTable getTB_D_M_WORK_SHIFT_H()
    {
        try
        {
            return dl.getTB_D_M_WORK_SHIFT_H();
        }
        catch (Exception)
        {

            throw;
        }
    }
}