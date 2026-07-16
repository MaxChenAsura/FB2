using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.Util;

/// <summary>
/// CFB2HB0100BO 的摘要描述
/// </summary>
public class CFB2HB0100BO : BaseService
{
    public CFB2HB0100BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public System.Data.DataTable getLEVEL_CD()
    {
        CFB2HB0100DAO dao = new CFB2HB0100DAO();
        try
        {
            return dao.getLevelCD();
        }
        catch (Exception)
        {

            throw;
        }
    }
    public System.Data.DataTable getCOMPANY_CD()
    {
        CFB2HB0100DAO dao = new CFB2HB0100DAO();
        try
        {
            return dao.getCOMPANY_CD();
        }
        catch (Exception)
        {

            throw;
        }
    }
    public string callSP()
    {
        CFB2HB0100DAO dao = new CFB2HB0100DAO();
        try
        {
            dao.SP_H_HEAD_DEPT();
            dao.SP_H_UPD_EMP_HEAD();
            dao.SP_H_DEPT_DATA();
            //dao.SP_H_HEAD_EMP(); //已棄用
            dao.SP_H_EMP_DATA();

            return "0";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    //外勞居留證檢查
    public IWorkbook uploadLICENSEID(Stream fs, string type)
    {
        try
        {
            bool valid = true;
            CFB2HB0100DAO dao = new CFB2HB0100DAO();
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
                DateTime rtn = new DateTime();
                //巡覽每row的資料第一列為title跳過

                int numCheckResult = 0;
                string error = "";
                string empID = "";          //工號
                string empName = "";          //姓名
                string licenseID= "";       //居留證號
                DataTable tmp = null;

                for (int i = 1; i <= sheet.LastRowNum; i++)
                {
                    if (sheet.GetRow(i) != null)
                    {
                        //讀取cell資料，第一欄為檢核結果欄位跳過
                        empID = sheet.GetRow(i).GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                        empName = sheet.GetRow(i).GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                        licenseID = sheet.GetRow(i).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();

                        error = "";
                        numCheckResult = 0;
                        //檢查第一欄
                        if (empID == "")
                            error += "工號欄位不可空白\n";
                        else
                        {

                            if (empID.Trim().Length != 5 || !int.TryParse(empID.Trim(), out numCheckResult))
                                error += "工號必須為數字, 且長度必須為5\n";

                        }

                        if (licenseID == "")
                            error += "居留證(身份證)不可空白\n";
                        else
                        {
                            //檢查 居留證(身份證) 正確性
                            if (licenseID.Length != 10)
                                error += "居留證(身份證)長度必須為10\n";
                        }

                        //工號是否存在
                        if (empID != "")
                        {
                            tmp = dao.getEmpData(empID);
                            if (tmp.Rows.Count == 0)
                            {
                                error += "此工號不存在 \n";
                            }

                            tmp = dao.getEmpData(empID,"PJ16");
                            if (tmp.Rows.Count == 0)
                            {
                                error += "此工號非外籍技術員 \n";
                            }
                        }

                        //傳出錯誤訊息
                        style1.SetFont(font1);
                        sheet.GetRow(i).CreateCell(0).CellStyle = style1;
                        sheet.GetRow(i).GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(error);
                        if (error.Trim() != "")
                        {
                            valid = false;
                        }
                    }
                }

                if (valid)
                {
                    //BeginTransaction();

                    for (int i = 1; i <= sheet.LastRowNum; i++)
                    {
                        if (sheet.GetRow(i) != null)
                        {
                            //讀取cell資料，第一欄為檢核結果欄位跳過
                            dao.EMP_ID = sheet.GetRow(i).GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            dao.LICENSE_ID = sheet.GetRow(i).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();

                            dao.CREATED_BY = SessionHandle.Current.emp_id;
                            dao.UPDATED_BY = SessionHandle.Current.emp_id;
                            dao.FUNC_ID = "FB2HB010";
                            /*
                             人事主檔				(TB_H_M_EMP)
                            人事月檔				(TB_H_R_EMP_DATA_MONTH)

                            團保主檔				(TB_I_M_GROUP_TXN)
                            保險資料主檔			(TB_I_M_PERSONDATA)
                            保險資料更新歷史檔		(TB_I_R_DATAUPDAE_HIS)
                            勞保健保勞退履歷主檔	(TB_I_M_3IN1_TXN)
                            保險減免資料履歷檔		(TB_I_M_REDUCE_TXN)
                            保險一括異動記錄檔		(TB_I_M_CHG_TXN)
                            月份團保保費代扣資料檔	(TB_I_R_GROUP_MONTH)
                            月份保費代扣資料檔		(TB_I_R_FEES_MONTH)
                            保費追溯資料檔			(TB_I_M_FEES_TRACEBACK)
                            保費帳單轉入暫存檔		(TB_I_S_BILLS)
                            保費比對異常資料檔		(TB_I_M_BILLS_COMPARE)
                            眷屬計算健保保費暫存檔	(TB_I_S_FAMILY_FEES)
                             */

                            dao.updateLICENSEID("TB_D_M_ACCOM_MAIN");                                                    
                            dao.updateLICENSEID("TB_I_M_3IN1_TXN");
                            dao.updateLICENSEID("TB_I_M_BILLS_COMPARE");
                            dao.updateLICENSEID("TB_I_M_CHG_TXN");
                            dao.updateLICENSEID("TB_I_M_FEES_TRACEBACK");
                            dao.updateLICENSEID("TB_I_M_GROUP_TXN");
                            dao.updateLICENSEID("TB_I_M_PERSONDATA");
                            dao.updateLICENSEID("TB_I_M_REDUCE_TXN");
                            dao.updateLICENSEID("TB_I_R_DATAUPDAE_HIS");
                            dao.updateLICENSEID("TB_I_R_FEES_MONTH");
                            dao.updateLICENSEID("TB_I_R_GROUP_MONTH");
                            dao.updateLICENSEID("TB_I_S_BILLS");
                            dao.updateLICENSEID("TB_I_S_FAMILY_FEES");
                            dao.updateLICENSEID("TB_S_M_MUTUAL_EMP");
                            dao.updateLICENSEID("TB_H_M_EMP");
                            dao.updateLICENSEID("TB_H_R_EMP_DATA_MONTH");
                            dao.updateLICENSEID("TB_H_R_EMP_DATA_MONTH_HIS");

                        }
                    }
                    //Commit();
                    return null;
                }
                else
                {
                    return workbook;
                }
            }
            else
            {
                return workbook;
            }
        }
        catch
        {
            //RollBack();
            throw;
        }
    }

    //加班管制對象
    public IWorkbook uploadExcel(Stream fs, string type)
    {
        try
        {
            bool valid = true;
            CFB2HB0100DAO dao = new CFB2HB0100DAO();
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
            if (sheet != null)
            {
                DateTime rtn = new DateTime();
                //巡覽每row的資料第一列為title跳過
                for (int i = 1; i <= sheet.LastRowNum; i++)
                {
                    if (sheet.GetRow(i) != null)
                    {
                        //讀取cell資料，第一欄為檢核結果欄位跳過
                        string cell1 = sheet.GetRow(i).GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                        string cell2 = sheet.GetRow(i).GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                        string licenseID = sheet.GetRow(i).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                        string cell4 = sheet.GetRow(i).GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                        //string cell5 = sheet.GetRow(i).GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();

                        string error = "";
                        int numCheckResult = 0;
                        //檢查第一欄
                        if (cell1 == "")
                            error += "工號欄位不可空白\n";
                        else
                        {

                            if (cell1.Trim().Length != 5 || !int.TryParse(cell1.Trim(), out numCheckResult))
                                error += "工號必須為數字, 且長度必須為5\n";

                        }
                        //檢查必填欄位
                        if (cell2 == "")
                            error += "姓名欄位不可空白\n";

                        if (licenseID == "")
                            error += "加班管制區分欄位不可空白\n";
                        else
                        {
                            //檢查是否存在代碼
                            DataTable tmp = dao.getOVERTIME_CTL(licenseID);
                            if (tmp.Rows.Count == 0)
                            {
                                error += "加班管制區分不存在 \n";
                            }
                        }

                        if (cell4 == "")
                            error += "體檢年度不可空白\n";
                        else
                        {
                            if (cell4.Trim().Length != 4 || !int.TryParse(cell4.Trim(), out numCheckResult))
                                error += "體檢年度為數字, 且長度必須為4\n";
                        }
                        if (cell1 != "")
                        {
                            DataTable tmp = dao.getEmpData(cell1);
                            if (tmp.Rows.Count == 0)
                            {
                                error += "此工號不存在 \n";
                            }
                        }
                        /*
                        //檢查日期格式是否正確
                        rtn = new DateTime();
                        if (string.IsNullOrEmpty(cell5))
                        {
                            error += "開始日期不可空白 \n";
                        }
                        else
                        {
                            if (DateTime.TryParse(cell5, out  rtn) == false)
                            {
                                error += "開始日期格式不正確 \n";
                            }
                        }
                        */
                        sheet.GetRow(i).GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(error);
                        if (error.Trim() != "")
                        {
                            valid = false;
                        }
                    }
                }

                if (valid)
                {
                    BeginTransaction();
                    //更新 所有員工人事主檔 的在職員工 加班管制區分 為 1(一般員工)
                    dao.updateALLOVERTIME_CTL_CD();
                    //dao.updateALLOVERTIME_CTL_CD_HIS();

                    for (int i = 1; i <= sheet.LastRowNum; i++)
                    {
                        if (sheet.GetRow(i) != null)
                        {
                            //讀取cell資料，第一欄為檢核結果欄位跳過
                            dao.EMP_ID = sheet.GetRow(i).GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            dao.EMP_NAME = sheet.GetRow(i).GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            dao.OVERTIME_CTL_CD = sheet.GetRow(i).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            dao.HEALTH_YEAR = sheet.GetRow(i).GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            //dao.OVERTIME_CTL_DT = sheet.GetRow(i).GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();

                            dao.CREATED_BY = SessionHandle.Current.emp_id;
                            dao.UPDATED_BY = SessionHandle.Current.emp_id;
                            dao.FUNC_ID = "FB2HB010";
                            dao.updateOVERTIME_CTL_CD();
                        }
                    }
                    Commit();
                    return null;
                }
                else
                {
                    return workbook;
                }
            }
            else
            {
                return workbook;
            }
        }
        catch
        {
            RollBack();
            throw;
        }
    }

    public DataTable getData(string emp_id)
    {
        CFB2HB0100DAO dao = new CFB2HB0100DAO();
        try
        {
            return dao.getDefaultData(emp_id);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //儲存人事主檔
    public string updateEmpData(CFB2HB0100DAO wfb2hb)
    {
        try
        {
            BeginTransaction();
            //基本資料
            wfb2hb.updateEmpData();
            if (wfb2hb.ORI_EMP_NAME != wfb2hb.EMP_NAME || wfb2hb.ORI_BIRTH_DT != wfb2hb.BIRTH_DT || wfb2hb.ORI_LICENSE_ID != wfb2hb.LICENSE_ID)
            {
                //更新保險資料主檔
                wfb2hb.updatePERSONDATA();

                //更新勞保健保勞退履歷主檔等數個檔案
                wfb2hb.update3IN1_TXN();

                //新增保險歷史檔
                wfb2hb.insertDATAUPDAE_HIS();
            }
            //新增或更新外籍赴任
            if (wfb2hb.JPN_CD != "-1")
            {
                if (wfb2hb.IS_DURATION == "Y")
                {
                    wfb2hb.updateEMP_DURATION();
                }
                else
                {
                    wfb2hb.addEMP_DURATION_BY_EMP();
                }
            }

            //更新家庭成員檔
            /*
            wfb2hb.updateFamData();
            for (int i = 0; i < wfb2hb.EMP_FAMILY.Rows.Count; i++)
            {
                if (wfb2hb.EMP_FAMILY.Rows[i]["FAMILY_ORI_LICENSE_ID"].ToString() != wfb2hb.EMP_FAMILY.Rows[i]["FAMILY_LICENSE_ID"].ToString() ||
                    wfb2hb.EMP_FAMILY.Rows[i]["FAMILY_ORI_BIRTH_DT"].ToString() != wfb2hb.EMP_FAMILY.Rows[i]["FAMILY_BIRTH_DT"].ToString())
                {
                    //更新家庭成員保險資料主檔
                    wfb2hb.updateFam_PERSONDATA(wfb2hb.EMP_FAMILY.Rows[i]);
                    //新增家庭成員保險歷史檔
                    wfb2hb.insertFam_DATAUPDAE_HIS(wfb2hb.EMP_FAMILY.Rows[i]);
                }
            }

            //更新學歷檔
            wfb2hb.updateEduData();
            //更新經歷檔
            wfb2hb.updateExpData();
            */
            //更新 員工刷卡管制設定歷史檔
            wfb2hb.updateDUTY_CHECK_HIS();
            //更新 日勤務狀態資料檔
            wfb2hb.update_EMP_DUTY_CHECK();

            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    public DataTable getEmpFamily(CFB2HB0100DAO dao, string sortExpression)
    {
        try
        {
            return dao.getEmpFamily(sortExpression);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getEdu(CFB2HB0100DAO dao, string sortExpression)
    {
        try
        {
            return dao.getEdu(sortExpression);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getExp(CFB2HB0100DAO dao, string sortExpression)
    {
        try
        {
            return dao.getExp(sortExpression);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getUNION_PJOB_CD()
    {
        try
        {
            CFB2HB0100DAO dao = new CFB2HB0100DAO();

            return dao.getUNION_PJOB_CD();
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getLevelCD(string join_dt)
    {
        CFB2HB0100DAO dao = new CFB2HB0100DAO();
        try
        {
            return dao.getLevelCD(join_dt);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getGRADECD(string LEVEL_CD)
    {
        CFB2HB0100DAO dao = new CFB2HB0100DAO();
        try
        {
            return dao.getGRADECD(LEVEL_CD);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public string getNewEMP_ID()
    {
        try
        {
            CFB2HB0100DAO dao = new CFB2HB0100DAO();
            string next_emp_id = dao.getNextEMP_ID();

            if (next_emp_id != "")
            {
                string new_emp_id = Convert.ToString(Convert.ToInt32(next_emp_id) + 1);
                BeginTransaction();
                dao.update_next_emp_id(new_emp_id);
                Commit();
            }

            return next_emp_id;
        }
        catch (Exception)
        {
            RollBack();
            throw;
        }
    }

    public string addEmpData(CFB2HB0100DAO dao)
    {
        try
        {
            string errStr = "";
            //檢查重複
            DataTable dup_dt = dao.getDUP_EMP();
            if (dup_dt.Rows.Count > 0)
            {
                errStr += "工號重覆 \\n";
            }
            //檢查薪資匯款帳號重覆
            DataTable dup_salary = dao.getDUP_SALARY();
            if (dao.SALARY_ACCOUNT_NO != "")
            {
                if (dup_salary.Rows.Count > 0)
                {
                    errStr += "薪資匯款帳號重覆 \\n";
                }
            }
            if (errStr == "")
            {
                //取得流水號
                DataTable dt = dao.getNEW_HR_CHG_NO();
                if (dt.Rows.Count > 0)
                {
                    dao.HR_CHG_NO = DateTime.Parse(dao.JOIN_DT).ToString("yyyyMMdd") + dt.Rows[0]["NEW_HR_CHG_NO"].ToString();
                }
                else
                {
                    dao.HR_CHG_NO = DateTime.Parse(dao.JOIN_DT).ToString("yyyyMMdd") + "0001";
                    dao.insertNEW_HR_CHG_NO();
                }
                //取得序號
                DataTable dt2 = dao.getCHG_SEQ();
                if (dt2.Rows.Count > 0)
                {
                    dao.CHG_SEQ = dt2.Rows[0]["CHG_SEQ"].ToString();
                }
                else
                {
                    dao.CHG_SEQ = "1";
                }

                BeginTransaction();
                //新增基本資料
                dao.addEmpData();

                //新增外籍赴任
                if (dao.JPN_CD != "" && DateTime.Parse(dao.JOIN_DT) <= DateTime.Parse(DateTime.Now.ToShortDateString()))
                {
                    dao.addEMP_DURATION();
                }

                //新增家庭成員檔
                dao.addFamData();

                //新增學歷檔
                dao.addEduData();
                //新增經歷檔
                dao.addExpData();

                //新增 人事異動主檔
                dao.addHR_CHANGE_H();
                //新增 人事異動明細檔，共10筆
                dao.addHR_CHANGE_D();
                Commit();

                //啟動生效處理的作業
                if (DateTime.Parse(dao.JOIN_DT) <= DateTime.Parse(DateTime.Now.ToShortDateString()))
                {
                    //呼叫-人事異動生效作業
                    dao.SP_H_HR_CHG_PROC();
                    //呼叫-維護員工卡片資料檔
                    dao.SP_D_UPD_CARD_DATA();
                    //呼叫-員工申請異常刷卡時間
                    dao.SP_D_M_EMP_AVAILABLE_LEAVE();
                    //呼叫-員工人事履歷生成
                    dao.SP_H_EMP_HR_CHG_RECORD();
                    //呼叫-部門主管更新作業
                    dao.SP_H_UPD_DEPT_HEAD();
                    //呼叫-員工主管更新作業
                    dao.SP_H_UPD_EMP_HEAD();
                    //呼叫-主管可管理部門資料生成
                    dao.SP_H_HEAD_DEPT();
                    //呼叫-部門資料生成
                    dao.SP_H_DEPT_DATA();
                    //呼叫-員工資料生成
                    dao.SP_H_EMP_DATA();

                    //dao.SP_H_HEAD_EMP();(已棄用)

                }
                return "0";
            }
            else
                return errStr;

        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    public DataTable getEMP_DURATIONdata(string emp_id)
    {
        try
        {
            CFB2HB0100DAO dao = new CFB2HB0100DAO();
            return dao.getDurationData(emp_id);

        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getEXAM_DAYS()
    {
        try
        {
            CFB2HB0100DAO dao = new CFB2HB0100DAO();
            return dao.getEXAM_DAYS();

        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getWorkShift(string work_cd = "")
    {
        try
        {
            CFB2HB0100DAO dao = new CFB2HB0100DAO();
            return dao.getWorkShift();

        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getDUP_ALLOWANCE(string family_license_id)
    {
        try
        {
            CFB2HB0100DAO dao = new CFB2HB0100DAO();
            return dao.getDUP_ALLOWANCE(family_license_id);

        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getOTH1_CONTRACT_MONTHS()
    {
        try
        {
            CFB2HB0100DAO dao = new CFB2HB0100DAO();
            return dao.getOTH1_CONTRACT_MONTHS();
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getW_OTH1_CONTRACT_EDT()
    {
        try
        {
            CFB2HB0100DAO dao = new CFB2HB0100DAO();
            return dao.getW_OTH1_CONTRACT_EDT();
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getKZ_CONTRACT_MONTHS()
    {
        try
        {
            CFB2HB0100DAO dao = new CFB2HB0100DAO();
            return dao.getKZ_CONTRACT_MONTHS();
        }
        catch (Exception)
        {

            throw;
        }
    }

    public string getFilePath()
    {
        try
        {
            CFB2HB0100DAO dao = new CFB2HB0100DAO();
            DataTable dt = dao.getFilePath();
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["CODE_VAL1"].ToString();
            }
            else
                return "";

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
            CFB2HB0100DAO dao = new CFB2HB0100DAO();
            return dao.getDEPT_DATA(dept_no);


        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getPJOB_DATA(string pjob_cd)
    {
        try
        {
            CFB2HB0100DAO dao = new CFB2HB0100DAO();
            return dao.getPJOB_DATA(pjob_cd);
        }
        catch (Exception)
        {

            throw;
        }
    }


    #region "家庭成員檔增修"
    //新增
    public string insertFamData(CFB2HB0100DAO hb010DAO)
    {
        string rtnmessage = "";
        try
        {
            //若需要則要進行邏輯檢查
            DataTable dt = new DataTable();

            //1.檢查PK值有無重覆
            dt = hb010DAO.getFamPKData();
            if ((int)dt.Rows[0]["resultCount"] > 0)
            {
                rtnmessage += "家庭成員不可重複輸入 \\n";
            }


            //檢查OK更新
            if (rtnmessage == "")
            {
                try
                {
                    BeginTransaction();
                    hb010DAO.insertFamData();
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

    //修改家庭成員檔
    public string updateFamData(CFB2HB0100DAO hb010DAO)
    {
        string rtnmessage = "";
        try
        {
            //若需要則要進行邏輯檢查
            DataTable dt = new DataTable();
            bool ischgBirthday = false;
   
            //2.檢查生日是否有變更
            dt = hb010DAO.getBirthdayDay();
            if ((int)dt.Rows[0]["resultCount"] == 0)
            {
                ischgBirthday = true;
            }

            //檢查OK更新
            if (rtnmessage == "")
            {
                try
                {
                    BeginTransaction();
                    //若身份證字號或生日有修改,要更改保險
                    if (ischgBirthday)
                    {
                        //更新家庭成員保險資料主檔
                        if (hb010DAO.updateFam_PERSONDATA() > 0)
                        {
                            //新增家庭成員保險歷史檔
                            hb010DAO.insertFam_DATAUPDAE_HIS();
                        }
                    }

                    hb010DAO.updateFamilyData();
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

    //刪除家庭成員檔
    public string deleteFamData(List<Tuple<string, string>> keysList, CFB2HB0100DAO hb010DAO)
    {
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
                        hb010DAO.EMP_ID = item.Item1;
                        hb010DAO.FAMILY_LICENSE_ID = item.Item2;
                        hb010DAO.deleteFamData();
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



    #region "學歷增修"
    //新增
    public string insertEduData(CFB2HB0100DAO hb010DAO)
    {
        string rtnmessage = "";
        try
        {
            //若需要則要進行邏輯檢查
            DataTable dt = new DataTable();

            //1.檢查PK值有無重覆
            dt = hb010DAO.getEducationPKData();
            if ((int)dt.Rows[0]["resultCount"] > 0)
            {
                rtnmessage += "教育程度代碼不可重複 \\n";
            }


            //檢查OK更新
            if (rtnmessage == "")
            {
                try
                {
                    BeginTransaction();
                    hb010DAO.insertEducationData();
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

    //修改
    public string updateEduData(CFB2HB0100DAO hb010DAO)
    {
        string rtnmessage = "";
        try
        {
            //若需要則要進行邏輯檢查
            DataTable dt = new DataTable();

            //檢查OK更新
            if (rtnmessage == "")
            {
                try
                {
                    BeginTransaction();

                    hb010DAO.updateEducationData();

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

    //刪除 學歷檔
    public string deleteEduData(List<Tuple<string, string>> keysList, CFB2HB0100DAO hb010DAO)
    {
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
                        hb010DAO.EMP_ID = item.Item1;
                        hb010DAO.EDUCATION_CD = item.Item2;
                        hb010DAO.deleteEducationData();
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



    #region "經歷增修"
    //新增
    public string insertExpData(CFB2HB0100DAO hb010DAO)
    {
        string rtnmessage = "";
        try
        {
            //若需要則要進行邏輯檢查
            DataTable dt = new DataTable();

            //1.檢查PK值有無重覆
            dt = hb010DAO.getExperPKData();
            if ((int)dt.Rows[0]["resultCount"] > 0)
            {
                rtnmessage += "公司名稱不可重複 \\n";
            }


            //檢查OK更新
            if (rtnmessage == "")
            {
                try
                {
                    BeginTransaction();
                    hb010DAO.insertExperData();
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

    //修改
    public string UpdateExpData(CFB2HB0100DAO hb010DAO)
    {
        string rtnmessage = "";
        try
        {
            //若需要則要進行邏輯檢查
            DataTable dt = new DataTable();

            //檢查OK更新
            if (rtnmessage == "")
            {
                try
                {
                    BeginTransaction();

                    hb010DAO.updateExperData();

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

    //刪除 
    public string deleteExpData(List<Tuple<string, string>> keysList, CFB2HB0100DAO hb010DAO)
    {
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
                        hb010DAO.EMP_ID = item.Item1;
                        hb010DAO.EXP_COMPANY_NAME = item.Item2;
                        hb010DAO.deleteExperData();
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

}