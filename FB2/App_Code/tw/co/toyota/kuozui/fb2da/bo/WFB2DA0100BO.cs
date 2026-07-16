using FB2.tw.co.toyota.kuozui.bo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Web;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;

/// <summary>
/// WFB2DA0100BO 的摘要描述
/// </summary>
public class WFB2DA0100BO : BaseService
{
    private WFB2DA0100DL dl = null;

    public WFB2DA0100BO()
    {
        dl = new WFB2DA0100DL();
    }


    public DataTable GetGridData(int startRowIndex, int maximumRows, string calendar_cd, string is_valid, string sortExpression)
    {
        return dl.GetGridData(startRowIndex, maximumRows, calendar_cd, is_valid, sortExpression);
    }

    public int GetGridDataCount(int startRowIndex, int maximumRows, string calendar_cd, string is_valid)
    {
        return dl.GetGridDataCount(startRowIndex, maximumRows, calendar_cd, is_valid);
    }

    public bool DeleteItem(List<WFB2DA0100DAO> DelItems, out string Message)
    {
        this.BeginTransaction();
        foreach (WFB2DA0100DAO item in DelItems)
        {
            if (dl.Check_EMP_DATA(item, true) > 0)
            {
                Message = Resources.Resource.wfb2da_CALENDAR_H_Mapped_EmpProfile_Message;
                return false;
            }
            else
            {
                if (dl.Check_WORK_SHIFT(item, true) > 0)
                {
                    Message = Resources.Resource.wfb2da_CALENDAR_H_Mapped_WorkSift_Message;
                    return false;
                }
                else
                {
                    if (dl.Check_WORK_SHIFT_EMP_DAY_DUTY(item, true) > 0)
                    {
                        Message = Resources.Resource.wfb2da_CALENDAR_H_Mapped_DayDuiy_Message;
                        return false;
                    }
                }
            }
        }
        bool ReturnValue = dl.Del_CALENDAR_H_CALENDAR_D(DelItems, true, out Message);
        if (ReturnValue)
            this.Commit();
        else
            this.RollBack();
        return ReturnValue;
    }

    public bool InsertItem(WFB2DA0100DAO InsertItem, out String Message)
    {
        int CheckDataCount = dl.Check_CALENDAR_H_By_Key(InsertItem, false, out Message);
        if (CheckDataCount == 0)
            return dl.Insert_CALENDAR_H(InsertItem, false, out Message);
        else
        {
            Message = Resources.Resource.wfb2da_CALENDAR_CD_Already_Message;
            return false;
        }
    }

    public bool UpdateItem(WFB2DA0100DAO updateItem, out string Message)
    {
        Message = string.Empty;
        if (updateItem.IS_VALID == "N")
        {
            if (dl.Check_EMP_DATA(updateItem, false) > 0)
            {
                Message = Resources.Resource.wfb2da_NotSettingIS_VALID_N_Message;
                return false;
            }
        }
        return dl.Update_CALENDAR_H(updateItem, false, out Message);
    }

    public WFB2DA0100DAO GetSingleCalendarData(string strCALENDAR_CD, string StartDate, string EndDate, out string Message)
    {
        Message = string.Empty;
        List<WFB2DA0100DAO> QueryData = dl.GetTB_D_M_CALENDAR_H(strCALENDAR_CD, false, out Message);
        if (string.IsNullOrEmpty(Message))
        {
            if (QueryData.Count == 1)
            {
                WFB2DA0100DAO ReturnValue = QueryData[0];
                if (string.IsNullOrEmpty(StartDate) == false && string.IsNullOrEmpty(EndDate) == false)
                {
                    ReturnValue.Dtl = dl.GetTB_D_M_CALENDAR_D(strCALENDAR_CD, StartDate, EndDate, false, out Message);
                    if (string.IsNullOrEmpty(Message))
                        return ReturnValue;
                }
                else
                    return ReturnValue;
            }
        }
        return null;
    }

    public bool CheckAndGrantCalendar(string Calendar_CD, DateTime StartDate, DateTime EndDate, WFB2DA0100DAO GrantDates, out string Message)
    {
        Message = string.Empty;
        List<WFB2DA0100DtlDAO> dao = dl.GetTB_D_M_CALENDAR_D(Calendar_CD, StartDate.ToString("yyyy-MM-dd"), EndDate.ToString("yyyy-MM-dd"), false, out Message);
        if (dao.Count == 0 && string.IsNullOrEmpty(Message))
        {
            bool ProcessState = GrantCalendar_D(GrantDates, out Message);
            if (ProcessState && string.IsNullOrEmpty(Message))
                return true;
            else
                return false;
        }
        else if (dao.Count > 0 && string.IsNullOrEmpty(Message))
        {
            Message = Resources.Resource.wfb2da_Calendar_Already_Confirm;
            return true;
        }
        else
            return false;
    }

    public bool GrantCalendar_D(WFB2DA0100DAO GrantDates, out string Message)
    {
        Message = string.Empty;
        this.BeginTransaction();
        //修改行事曆
        bool ProcessState = dl.GrantCALENDAR_D(GrantDates, true, out Message);
        DateTime DtlStartDate = GrantDates.Dtl.First<WFB2DA0100DtlDAO>().CALENDAR_DT;
        DateTime DtlEndDate = GrantDates.Dtl.Last<WFB2DA0100DtlDAO>().CALENDAR_DT;

        /*
        //SP 改SQL Command
        try
        {
            if (ProcessState && string.IsNullOrEmpty(Message))
            {
                utilities.UPD_EMP_DAY_DUTY2(dl.GetdbConn, GrantDates.CALENDAR_CD, DtlStartDate, DtlEndDate, SessionHandle.Current.emp_id);
                ProcessState &= true;
            }
        }
        catch (Exception ex)
        {

            Message = ex.Message;
            ProcessState &= false;
        }
        */
        if (ProcessState && string.IsNullOrEmpty(Message))
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

    public List<UCCommCodeDropDwonListDAO> GetlWorkDayCommCode()
    {
        UCCommCodeDropDwonListDL uccddll = new UCCommCodeDropDwonListDL();
        UCCommCodeDropDwonListDAO uccddldao = new UCCommCodeDropDwonListDAO();
        uccddldao.WhereIS_VALID = Kuozui.BooleanProperty.True;
        uccddldao.WhereSYS_CDs = "DA";
        uccddldao.WhereMAIN_CDs = "DT_TYPE";
        return uccddll.getData("SUB_CD,SUB_DESC", "{0} - {1}", "SUB_CD", "", uccddldao);
    }

    public WFB2DA0100DAO GetWorkShiftH(WFB2DA0100DAO InputDao, out string Message)
    {
        Message = string.Empty;
        WFB2DA0100DAO QueryData = dl.GetWorkShiftH(InputDao, false, out Message);
        if (string.IsNullOrEmpty(Message))
            return QueryData;
        return null;
    }

    //複製行事曆
    public bool CopyCalendar(WFB2DA0100DAO Source, WFB2DA0100DAO Destination, string StartCALENDAR_DT, String EndCALENDAR_DT, out string Message)
    {
        Message = string.Empty;
        this.BeginTransaction();
        bool isNeedDelCalendarD = false; //是否存在明細檔

        try
        {
            //判斷行事曆主檔是否存在,>0:存在,==0:不存在
            if (dl.Check_CALENDAR_H_By_Key(Destination, true, out Message) == 0)
            {
                //不存在,新增行事曆主檔
                if (dl.Insert_CALENDAR_H(Destination, true, out Message) == false)
                {
                    this.RollBack();
                    return false;
                }
            }
            else
            {
                //判斷行事歷明細檔是否存在,若存在,需先刪除
                if (dl.Check_CALENDAR_D_By_Key(Destination.CALENDAR_CD, StartCALENDAR_DT, EndCALENDAR_DT, true, out Message) > 0)
                {
                    isNeedDelCalendarD = true;
                }
            }

            //判斷是否需刪除行事曆明細檔
            if (isNeedDelCalendarD)
            {
                //先進行 刪除 行事歷明細檔
                if (dl.delete_CALENDAR_D(Destination.CALENDAR_CD, StartCALENDAR_DT, EndCALENDAR_DT, true, out Message) == false)
                {
                    this.RollBack();
                    return false;
                }
            }
            //再進行 新增 行事歷明細檔
            if (dl.Insert_CALENDAR_D(Destination, out Message) == false)
            {
                this.RollBack();
                return false;
            }
            this.Commit();

            //修改班表及日勤務狀態檔reopen,因為工程巨大(會timeout),故用SP
            //dl.execSP_D_CALENDAR_COPY(Destination);

            return true;
        }
        catch (Exception ex)
        {
            this.RollBack();
            Message += ex.ToString();
            return false;
        }


    }

    public bool DtlSave(WFB2DA0100DAO dao, out string Message)
    {
        this.BeginTransaction();
        Message = string.Empty;
        bool ReturnValue = true;
        if (ReturnValue)
            ReturnValue = dl.Save_CALENDAR_D(dao, true, out Message);

        if (ReturnValue)
            this.Commit();
        else
            this.RollBack();

        return ReturnValue;

        //if (ReturnValue)
        //    this.Commit();
        //else
        //    this.RollBack();
        //List<WFB2DA0100DtlDAO> DBDtlDao = dl.GetTB_D_M_CALENDAR_D(dao.CALENDAR_CD, dao.Dtl.First().CALENDAR_DT.ToString("yyyy/MM/dd"), dao.Dtl.Last().CALENDAR_DT.AddDays(1).ToString("yyyy/MM/dd"), true, out Message);

        //20170703 (3)現有程式若有變更到 行事曆明細檔除外的TB，一律都不處理
        //foreach (WFB2DA0100DtlDAO dtl in dao.Dtl)
        //{
        //    //SP 設計更改前做法
        //    //Debug.WriteLine("calendar dt = " + dtl.CALENDAR_DT);
        //    //List<WFB2DAEMP_DAY_DUTY> EmpDayDutys = dl.GetTB_D_M_EMP_DAY_DUTY(dao.CALENDAR_CD, dtl.CALENDAR_DT, true, out Message);
        //    //foreach (WFB2DAEMP_DAY_DUTY EmpDayDuty in EmpDayDutys)
        //    //{
        //    //    if (dl.RunProcSP_D_UPD_EMP_DAY_DUTY2(EmpDayDuty.EMP_ID, EmpDayDuty.CALENDAR_DT, dtl.WORK_DAY_CD, SessionHandle.Current.emp_id, "FB2DA010", true, out Message) == false)
        //    //    {
        //    //        ReturnValue = false;
        //    //        break;
        //    //    }
        //    //}
        //    //SP 設計更改後做法
        //    //if (dl.RunProcSP_D_UPD_EMP_DAY_DUTY2(dao.CALENDAR_CD, dtl.CALENDAR_DT, dtl.CALENDAR_DT, SessionHandle.Current.emp_id, "FB2DA010", true, out Message) == false)

        //    //SP 改SQL Command
        //    try
        //    {
        //        utilities.UPD_EMP_DAY_DUTY2(dl.GetdbConn, dao.CALENDAR_CD, dtl.CALENDAR_DT, dtl.CALENDAR_DT, SessionHandle.Current.emp_id);
        //        ReturnValue = true;
        //    }
        //    catch (Exception ex)
        //    {

        //        Message = ex.Message;
        //        ReturnValue = false;
        //        break;
        //    }

        //}

        //if (ReturnValue)
        //    ReturnValue = dl.Save_CALENDAR_D(dao, true, out Message);


       
    }

    //public bool SaveLoopRule(string SUB_CD, string SUB_DESC, out string Message)
    //{
    //    bool SaveState;
    //    if (string.IsNullOrEmpty(SUB_CD))
    //    {
    //        int RuleCount = this.GetLoopRuleCount(1, 10);
    //        int ORDER_SEQ = 1;
    //        DataTable DtRule = this.GetLoopRule(0, RuleCount, "ORDER_SEQ");
    //        string InsertSUB_CD = "1";
    //        if (RuleCount > 0)
    //        {
    //            ORDER_SEQ = Convert.ToInt16(DtRule.Rows[DtRule.Rows.Count - 1]["ORDER_SEQ"]) + 1;
    //            int WorkDayCD = dl.GetLoopRuleMaxNumbericWORK_DAY_CD(false);
    //            InsertSUB_CD = (WorkDayCD + 1).ToString();
    //        }
    //        WFB2DA0100LoopRule dao = new WFB2DA0100LoopRule();
    //        dao.MAIN_CD = "WORK_DAY_CD";
    //        dao.IS_VALID = "Y";
    //        dao.SUB_CD = InsertSUB_CD;
    //        dao.SUB_DESC = SUB_DESC;
    //        dao.SYS_CD = "DA";
    //        dao.ORDER_SEQ = ORDER_SEQ;
    //        SaveState = dl.InsertLoopRule(dao, false, out Message);
    //    }
    //    else
    //    {
    //        WFB2DA0100LoopRule dao = new WFB2DA0100LoopRule();
    //        dao.MAIN_CD = "WORK_DAY_CD";
    //        dao.IS_VALID = "Y";
    //        dao.SUB_CD = SUB_CD;
    //        dao.SUB_DESC = SUB_DESC;
    //        dao.SYS_CD = "DA";
    //        SaveState = dl.UpdateLoopRule(dao, false, out Message);
    //    }
    //    return SaveState;
    //}

    //public bool DeleteLoopRules(List<WFB2DA0100LoopRule> DelItems, out string Message)
    //{
    //    this.BeginTransaction();
    //    bool ReturnValue = dl.DeleteLoopRules(DelItems, true, out Message);
    //    if (ReturnValue)
    //        this.Commit();
    //    else
    //        this.RollBack();
    //    return ReturnValue;
    //}

    public IWorkbook uploadExcel1(Stream fs, string type, WFB2DA0100DL dao)
    {
        //取得登入者
        string userid = SessionHandle.Current.emp_id;
        //要載入的資料表名稱(行事曆明細檔)
        string tableName = "TB_D_M_CALENDAR_D";

        bool valid = true;
        DataTable CALENDAR_D_Table = new DataTable("CALENDAR_D_Table");
        string[] excel_pk;
        DataTable CALENDAR_dt = new DataTable();
        DataTable calendar_cd_dt = new DataTable();
        DataTable dt_type_dt = new DataTable();
        string calendar_dt = "";
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
            WFB2DA0100DAO dao2 = new WFB2DA0100DAO();
            font1.Color = HSSFColor.Red.Index;
            if (sheet != null)
            {
                #region MyRegion

                //(行事曆明細檔)
                //建立 DataTable
                DataRow CALENDAR_D_Row;

                //建立 FieldSchema
                CALENDAR_D_Table.Columns.Add("CALENDAR_CD", System.Type.GetType("System.String"));
                CALENDAR_D_Table.Columns.Add("CALENDAR_DT", System.Type.GetType("System.DateTime"));
                CALENDAR_D_Table.Columns.Add("WORK_DAY_CD", System.Type.GetType("System.String"));
                CALENDAR_D_Table.Columns.Add("DT_TYPE", System.Type.GetType("System.String"));
                CALENDAR_D_Table.Columns.Add("GROUP_CD", System.Type.GetType("System.String"));
                CALENDAR_D_Table.Columns.Add("CREATED_BY", System.Type.GetType("System.String"));
                CALENDAR_D_Table.Columns.Add("CREATED_DT", System.Type.GetType("System.DateTime"));
                CALENDAR_D_Table.Columns.Add("UPDATED_BY", System.Type.GetType("System.String"));
                CALENDAR_D_Table.Columns.Add("UPDATED_DT", System.Type.GetType("System.DateTime"));
                CALENDAR_D_Table.Columns.Add("FUNC_ID", System.Type.GetType("System.String"));

                #endregion

                #region 建立excel PK值

                DataRow CALENDAR_row;
                CALENDAR_dt.Columns.Add("CALENDAR_CD", System.Type.GetType("System.String"));
                CALENDAR_dt.Columns.Add("CALENDAR_DT", System.Type.GetType("System.DateTime"));
                #endregion

                if (sheet.LastRowNum != 0)
                {
                    #region 取得行事曆明細檔的PK值

                    calendar_cd_dt = dao.getAll_CALENDAR_CD();
                    calendar_cd_dt.PrimaryKey =
                        new DataColumn[] { calendar_cd_dt.Columns["CALENDAR_CD"], calendar_cd_dt.Columns["CALENDAR_DT"] };

                    #endregion
                    #region 取得日期類型 清單
                    dt_type_dt = utilities.getCommCodeVal("DA", "DT_TYPE", "");
                    dt_type_dt.PrimaryKey = new DataColumn[] { dt_type_dt.Columns["sub_cd"] };

                    #endregion

                }

                //巡覽每row的資料第一列為title跳過
                for (int i = 1; i <= sheet.LastRowNum; i++)
                {
                    if (sheet.GetRow(i) != null)
                    {
                        error = "";
                        excel_pk = new string[2];
                        #region 讀取cell資料，第一欄為檢核結果欄位跳過
                        dao2.CALENDAR_CD = sheet.GetRow(i).GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().ToUpper();
                        dao2.CALENDAR_DT = sheet.GetRow(i).GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().ToUpper();
                        dao2.DT_TYPE = sheet.GetRow(i).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().ToUpper();

                        //calendar_dt = dao2.CALENDAR_DT.Replace("/", "");
                        calendar_dt = dao2.CALENDAR_DT;
                        excel_pk[0] = dao2.CALENDAR_CD;
                        excel_pk[1] = calendar_dt;

                        #endregion

                        #region 檢核基本邏輯
                        //長度檢核
                        error += utilities.checkLength(dao2.CALENDAR_CD, "行事曆代碼", 1, false);
                        error += utilities.checkLength(calendar_dt, "日曆日期", 10, false);
                        error += utilities.checkLength(dao2.DT_TYPE, "日期類型", 1, false);

                        //格式檢核
                        //A.行事曆 + 日期																																										
                        //若行事曆代碼 + 日期 不存在行事曆明細檔,則顯示訊息「行事曆代碼 +  日期 不存在」					
                        DataRow dr;
                        if (dao2.CALENDAR_CD != "")
                        {
                            //存在否 行事曆明細檔 
                            dr = calendar_cd_dt.Rows.Find(excel_pk);
                            if (dr == null)
                            {
                                error += "行事曆代碼 + 日期 不存在\n";
                            }
                        }

                        //B.日期類型																																
                        //若 日期類型 不存在共用代碼檔,則顯示訊息「日期類型不存在」
                        if (dao2.DT_TYPE != "")
                        {
                            //存在否 日期類型 
                            dr = dt_type_dt.Rows.Find(dao2.DT_TYPE);
                            if (dr == null)
                            {
                                error += "日期類型不存在\n";
                            }
                        }				

                        //C.EXCEL 的 行事曆+日期 不可重覆 																			
                        //則顯示訊息「行事曆代碼+日期 重覆」														

                        //excel的PK值
                        if (CALENDAR_dt.Rows.Count > 0)
                        {
                            dr = CALENDAR_dt.Rows.Find(excel_pk);
                            if (dr != null)
                            {
                                error += "此EXCEL有相同的行事曆代碼+日期\n";
                            }
                            else
                            {
                                #region 建立excel PK值資料

                                CALENDAR_row = CALENDAR_dt.NewRow();
                                CALENDAR_row["CALENDAR_CD"] = dao2.CALENDAR_CD;
                                CALENDAR_row["CALENDAR_DT"] = dao2.CALENDAR_DT;
                                CALENDAR_dt.Rows.Add(CALENDAR_row);

                                CALENDAR_dt.PrimaryKey =
                                new DataColumn[] { CALENDAR_dt.Columns["CALENDAR_CD"], CALENDAR_dt.Columns["CALENDAR_DT"] };
                                #endregion
                            }
                        }
                        else
                        {
                            #region 建立excel PK值資料

                            CALENDAR_row = CALENDAR_dt.NewRow();
                            CALENDAR_row["CALENDAR_CD"] = dao2.CALENDAR_CD;
                            CALENDAR_row["CALENDAR_DT"] = dao2.CALENDAR_DT;
                            CALENDAR_dt.Rows.Add(CALENDAR_row);

                            CALENDAR_dt.PrimaryKey =
                            new DataColumn[] { CALENDAR_dt.Columns["CALENDAR_CD"], CALENDAR_dt.Columns["CALENDAR_DT"] };
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
                            #region 建立資料(行事曆明細檔)

                            CALENDAR_D_Row = CALENDAR_D_Table.NewRow();
                            CALENDAR_D_Row["CALENDAR_CD"] = dao2.CALENDAR_CD;
                            CALENDAR_D_Row["CALENDAR_DT"] = dao2.CALENDAR_DT;
                            CALENDAR_D_Row["WORK_DAY_CD"] = (dao2.DT_TYPE == "1") ? "1" : "2";
                            CALENDAR_D_Row["DT_TYPE"] = dao2.DT_TYPE;
                            CALENDAR_D_Row["GROUP_CD"] = "";
                            CALENDAR_D_Row["CREATED_BY"] = userid;
                            CALENDAR_D_Row["CREATED_DT"] = DateTime.Now;
                            CALENDAR_D_Row["UPDATED_BY"] = userid;
                            CALENDAR_D_Row["UPDATED_DT"] = DateTime.Now;
                            CALENDAR_D_Row["FUNC_ID"] = "FB2DA010";
                            CALENDAR_D_Table.Rows.Add(CALENDAR_D_Row);

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
                    CALENDAR_D_Table.Clear();
                    CALENDAR_dt.Clear();
                    calendar_cd_dt.Clear();
                    dt_type_dt.Clear();

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

                        for (int i = 0; i < CALENDAR_D_Table.Rows.Count; i++)
                        {
                            dao.updateAll_TB_D_M_CALENDAR_D(CALENDAR_D_Table.Rows[i]);
                        }

                        Commit();

                    }
                    catch (Exception ex)
                    {
                        RollBack();
                        throw;
                    }
                }

            }
            return null;

        }
        catch (Exception)
        {

            throw;
        }
        finally
        {
            CALENDAR_D_Table.Clear();
            CALENDAR_dt.Clear();
            calendar_cd_dt.Clear();
            dt_type_dt.Clear();
        }

    }

    public DataTable getCALENDAR_CD()
    {
        try
        {
            WFB2DA0100DL dao = new WFB2DA0100DL();
            return dao.getCALENDAR_CD();
        }
        catch (Exception)
        {

            throw;
        }
    }

    public string SP_DA010_01(WFB2DA0100DAO dao)
    {
        try
        {
            string result = "0";
            //call sp
            int err = dl.SP_DA010_01(dao);

            //確認SP有無成功
            DataTable dtSPresult = dl.checkSP("SP_DA010_01");
            if (dtSPresult.Rows.Count > 0)
            {
                //PROC_STATUS：Y = 成功,N = 失敗,E = Exception  ,PROC_LOG：處理結果中文訊息
                if (Convert.ToString(dtSPresult.Rows[0]["PROC_STATUS"]) != "Y")
                    return Convert.ToString(dtSPresult.Rows[0]["PROC_LOG"]) + "\\n";
            }

            return result;
        }
        catch (Exception)
        {

            throw;
        }
    }
}