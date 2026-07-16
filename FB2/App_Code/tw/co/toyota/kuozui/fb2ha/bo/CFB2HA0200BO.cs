using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;
using NPOI.SS.UserModel;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.Util;

/// <summary>
/// CFB2HA020BO 的摘要描述
/// </summary>
public class CFB2HA0200BO : BaseService
{
    public CFB2HA0200BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }



    public string delete_DeptNo(List<Tuple<string, string>> dept_no)
    {
        try
        {
            CFB2HA0200DAO wfb2ha = new CFB2HA0200DAO();
            string rtnmessage = "";
            string emp_id = "";
            foreach (var item in dept_no)
            {
                //檢查是否已存在公司組織設定檔
                DataTable tmp = wfb2ha.getExistDeptOrg(item.Item1);
                if ((int)tmp.Rows[0]["deptcount"] > 0)
                {
                    rtnmessage += "部門代號" + item + "，其下已建立子階部門資料，不可刪除 \\n";
                }
                //20200330是否有在職員工(員工檔)在該部門
                emp_id = wfb2ha.getH_EMP_ID(item.Item1);
                if (emp_id != "")
                    rtnmessage += emp_id + "...等員工在其部門(" + item.Item1 + ")，不可刪除\\n";
                //20200330是否有應受援員工(應受援履歷檔)在該部門
                emp_id = wfb2ha.getASSIST_EMP_ID(item.Item1);
                if (emp_id != "")
                    rtnmessage += emp_id + "...等應受援員工原部門(" + item.Item1 + ")，不可刪除\\n";
            }

            //檢查OK逐筆刪除
            if (rtnmessage == "")
            {
                try
                {
                    BeginTransaction();
                    foreach (var item in dept_no)
                    {
                        wfb2ha.deleteDeptNo(item.Item1, item.Item2);
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
                return rtnmessage;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public DataTable getData(string dept_no, string start_dt)
    {
        try
        {
            CFB2HA0200DAO wfb2ha = new CFB2HA0200DAO();
            wfb2ha.DEPT_NO = dept_no;
            wfb2ha.START_DT = start_dt;
            return wfb2ha.getData();
        }
        catch (Exception)
        {

            throw;
        }
    }

    public string updateDEPT(CFB2HA0200DAO wfb2ha)
    {
        try
        {
            //取得現有資料
            DataTable tmp = wfb2ha.getExistData();
            if (tmp.Rows.Count > 0)
            {
                DateTime end_dt = DateTime.Parse(wfb2ha.END_DT);
                DateTime start_dt = DateTime.Parse(tmp.Rows[0]["START_DT"].ToString());
                if (end_dt >= start_dt)
                {
                    return "結束日期與較大生效日期的資料 重疊有效期間, 請修改為 " + start_dt.AddDays(-1).ToString("yyyy/MM/dd");
                }
                if (end_dt < start_dt.AddDays(-1))
                {
                    return "結束日期與較大生效日期的資料 有效期間中斷未連續, 請修改為 " + start_dt.AddDays(-1).ToString("yyyy/MM/dd");
                }
            }

            if (wfb2ha.END_DT != "9999/12/31")
            {
                DataTable dt = wfb2ha.getExistSubData();
                if (dt.Rows.Count > 0)
                    return "部門代號之下存在有效期較長的子階部門資料，不可以比子階部門提早結束有效期";

                string emp_id = "";
                //20200330是否有在職員工在該部門
                emp_id = wfb2ha.getH_EMP_ID(wfb2ha.DEPT_NO);
                if (emp_id != "")
                    return emp_id + "...等員工在其部門(" + wfb2ha.DEPT_NO + ")，不可結束有效期\\n";
                //20200330是否有應受援員工(應受援履歷檔)在該部門
                emp_id = wfb2ha.getASSIST_EMP_ID(wfb2ha.DEPT_NO);
                if (emp_id != "")
                    return emp_id + "...等應受援員工原部門(" + wfb2ha.DEPT_NO + ")，不可結束有效期\\n";
            }




            try
            {
                BeginTransaction();
                wfb2ha.updateDept();
                Commit();

            }
            catch (Exception ex)
            {
                RollBack();
                return ex.Message;
            }



            return "0";

        }
        catch (Exception ex)
        {

            return ex.Message;
        }
    }

    public string addDEPT(CFB2HA0200DAO wfb2ha)
    {
        try
        {
            DataTable tmp = wfb2ha.getSalaryData();

            if (tmp.Rows.Count > 0)
            {
                string YM = tmp.Rows[0]["SALARY_YM"].ToString();
                if (YM == "")
                {
                    return "無法取得最近一次薪資計算年月";
                }

                int days = DateTime.DaysInMonth(int.Parse(YM.Substring(0, 4)), int.Parse(YM.Substring(4, 2)));
                DateTime salaryEndDate = DateTime.Parse(YM.Substring(0, 4) + "/" + YM.Substring(4, 2) + "/" + days.ToString());
                DateTime startDate = DateTime.Parse(wfb2ha.START_DT);
                if (startDate <= salaryEndDate)
                {
                    return "生效日期不可以是已薪結日期";
                }

                DataTable dupdt = wfb2ha.getDupData();
                if (dupdt.Rows.Count > 0)
                {
                    return "部門代號+生效日期重覆";
                }

                DataTable dupdt2 = wfb2ha.getMaxEndDTByType();
                if (dupdt2.Rows.Count > 0 && dupdt2.Rows[0]["maxEndDT"].ToString() != "")
                {
                    DateTime maxStartDT = (DateTime)dupdt2.Rows[0]["maxEndDT"];
                    if (startDate <= maxStartDT)
                    {
                        return "部門代號在此期間已存在";
                    }
                }

                DataTable dt = wfb2ha.getExistData();
                if (dt.Rows.Count > 0)
                {
                    DateTime end_dt = DateTime.Parse(wfb2ha.END_DT);
                    DateTime start_dt = DateTime.Parse(dt.Rows[0]["START_DT"].ToString());
                    if (end_dt >= start_dt)
                    {
                        return "結束日期與既存較大生效日期的資料 重疊有效期間, 請修改為 " + start_dt.AddDays(-1).ToString("yyyy/MM/dd");
                    }
                    if (end_dt < start_dt.AddDays(-1))
                    {
                        return "結束日期與既存較大生效日期的資料 有效期間中斷未連續, 請修改為 " + start_dt.AddDays(-1).ToString("yyyy/MM/dd");
                    }
                }

                /*
                if (dupdt2.Rows.Count > 0)
                    return "部門代號在此期間已存在";
                */

                try
                {

                    BeginTransaction();
                    wfb2ha.addDept();
                    Commit();

                }
                catch (Exception ex)
                {
                    RollBack();
                    return ex.Message;
                }


            }
            else
                return "無法取得最近一次薪資計算年月";

            return "0";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public DataTable getEmpName(string emp_id)
    {
        try
        {
            CFB2HA0200DAO wfb2dh = new CFB2HA0200DAO();
            return wfb2dh.getEmpName(emp_id);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getACC_DEPT_Name(string acc_dept_no)
    {
        try
        {
            CFB2HA0200DAO wfb2dh = new CFB2HA0200DAO();
            return wfb2dh.getACC_DEPT_Name(acc_dept_no);
        }
        catch (Exception)
        {

            throw;
        }
    }

    #region 部門資料上傳

    //excel上傳
    public IWorkbook uploadExcel(Stream fs, string type)
    {
        CFB2HA0200DAO ha020DAO = new CFB2HA0200DAO();
        IWorkbook workbook = null;

        //取得範本sheet
        ISheet sheet = null;
        try
        {
            bool valid = true;

            //IWorkbook workbook;
            //依附檔名判斷要用哪種方式讀取
            if (type == ".xls")
            {
                workbook = new HSSFWorkbook(fs);
            }
            else if (type == ".xlsx")
            {
                workbook = new XSSFWorkbook(fs);
            }

            //取得sheet
            sheet = workbook.GetSheetAt(0);
            //錯誤訊息的字型
            ICellStyle errorStyle = workbook.CreateCellStyle();
            IFont font1 = workbook.CreateFont();
            font1.Color = HSSFColor.Red.Index;
            errorStyle.SetFont(font1);

            if (sheet != null)
            {
                #region cell陣列
                string[] dept_no = new string[sheet.LastRowNum + 1];
                string[] start_dt = new string[sheet.LastRowNum + 1];
                string[] end_dt = new string[sheet.LastRowNum + 1];
                string[] dept_name = new string[sheet.LastRowNum + 1];
                string[] dept_sname = new string[sheet.LastRowNum + 1];
                string[] dept_ename = new string[sheet.LastRowNum + 1];
                string[] head_emp_id = new string[sheet.LastRowNum + 1];
                string[] dept_level = new string[sheet.LastRowNum + 1];
                string[] org_type = new string[sheet.LastRowNum + 1];
                string[] acc_cd = new string[sheet.LastRowNum + 1];
                string[] acc_dept_no = new string[sheet.LastRowNum + 1];
                string[] remark = new string[sheet.LastRowNum + 1];
                //20150603 預設廠區
                string[] default_plant = new string[sheet.LastRowNum + 1];

                string[] checkDept_no = new string[sheet.LastRowNum + 1];
                bool[] isUpdate = new bool[sheet.LastRowNum + 1];

                #endregion


                string error = string.Empty; ;
                DataTable dt = new DataTable();
                DateTime dateFormat;
                DateTime startDT;
                DateTime endDT;
                //巡覽每row的資料第一列為title跳過(故i從1開始)
                //取得已最後已計薪的年月月底
                DateTime salaryDT = Convert.ToDateTime(utilities.getSalaryDT());

                for (int i = 1; i <= sheet.LastRowNum; i++)
                {
                    error = string.Empty; ;
                    if (sheet.GetRow(i) != null)
                    {
                        #region 讀取cell資料，第一欄為檢核結果欄位跳過
                        dept_no[i] = sheet.GetRow(i).GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().ToUpper();
                        start_dt[i] = sheet.GetRow(i).GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                        end_dt[i] = sheet.GetRow(i).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                        dept_name[i] = sheet.GetRow(i).GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                        dept_sname[i] = sheet.GetRow(i).GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                        dept_ename[i] = sheet.GetRow(i).GetCell(6, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                        head_emp_id[i] = sheet.GetRow(i).GetCell(7, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                        dept_level[i] = sheet.GetRow(i).GetCell(8, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                        org_type[i] = sheet.GetRow(i).GetCell(9, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                        acc_cd[i] = sheet.GetRow(i).GetCell(10, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();     //科目別
                        acc_dept_no[i] = sheet.GetRow(i).GetCell(11, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();//薪資部門區分
                        default_plant[i] = sheet.GetRow(i).GetCell(12, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();//預設廠別
                        remark[i] = sheet.GetRow(i).GetCell(13, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                        
                        #endregion

                        #region 檢核基本邏輯
                        //若結束日期為空白,則為9999/12/31
                        if (string.IsNullOrEmpty(end_dt[i]))
                        {
                            end_dt[i] = "9999/12/31";
                        }


                        //結束日期須大於生效日期
                        if (DateTime.TryParse(start_dt[i], out dateFormat) && DateTime.TryParse(end_dt[i], out dateFormat))
                        {
                            startDT = Convert.ToDateTime(start_dt[i]);
                            endDT = Convert.ToDateTime(end_dt[i]);
                            if (startDT >= endDT)
                            {
                                error += "生效日期需大於結束日期,\n";
                            }
                        }

                        error += utilities.checkEngNumber_fixLength(dept_no[i], "部門代號", 7, false);
                        error += utilities.checkDateFormat(start_dt[i], "生效日期", false);
                        error += utilities.checkDateFormat(end_dt[i], "結束日期", true);
                        error += utilities.checkLength(dept_name[i], "部門名稱", 60, false);
                        error += utilities.checkLength(dept_sname[i], "部門簡稱", 60, false);
                        error += utilities.checkLength(dept_ename[i], "部門英文名稱", 60, true);
                        error += utilities.checkLength(remark[i], "備註", 210, true);
                        error += utilities.checkLength(default_plant[i], "預設廠別", 1, true);

                        //預設廠別
                        if (dept_no[i].Length == 7)
	                    {
                            if (dept_no[i].Substring(0,2) == "KI" || dept_no[i].Substring(0,2) == "KJ")
                            {
                                if (default_plant[i] != "1")
                                {
                                    error += "部門代號為KI、KJ時，預設廠區只能為1,\n";
                                }
                            }
                            if (dept_no[i].Substring(0, 2) == "KK" || dept_no[i].Substring(0, 2) == "KP")
                            {
                                if (default_plant[i] != "2")
                                {
                                    error += "部門代號為KK、KP時，預設廠區只能為2,\n";
                                }
                            }
	                    }
                        

                        //工號需為5碼且部門主管工號需在職狀態
                        if (head_emp_id[i] == "")
                        {
                            error += "部門主管工號欄位不可空白,\n";
                        }
                        else
                        {
                            dt.Clear();
                            dt = ha020DAO.getEmpCount(head_emp_id[i]);
                            if ((int)dt.Rows[0]["resultCount"] == 0)
                            {
                                error += "部門主管工號不存在或已離職,\n";
                            }
                        }
                        //部門層級需存在於部門層級檔(TB_H_M_DEPT_LEVEL)
                        if (dept_level[i] == "")
                        {
                            error += "部門層級欄位不可空白,\n";
                        }
                        else
                        {
                            dt.Clear();
                            dt = ha020DAO.getDeptLevelCount(dept_level[i]);
                            if ((int)dt.Rows[0]["resultCount"] == 0)
                            {
                                error += "部門層級不存在或已失效,\n";
                            }
                        }
                        //組織類型需存在於共用代碼檔(ORG_TYPE)
                        if (org_type[i] == "")
                        {
                            error += "組織類型欄位不可空白,\n";
                        }
                        else
                        {
                            dt.Clear();
                            dt = ha020DAO.getCommCodeCount("HA", "ORG_TYPE", org_type[i]);
                            if ((int)dt.Rows[0]["resultCount"] == 0)
                            {
                                error += "組織類型不存在或已失效,\n";
                            }
                        }
                        //科目別需存在於共用代碼檔(ACC_CD)
                        if (acc_cd[i] == "")
                        {
                            error += "科目別欄位不可空白,\n";
                        }
                        else
                        {
                            dt.Clear();
                            dt = ha020DAO.getCommCodeCount("HA", "ACC_CD", acc_cd[i]);
                            if ((int)dt.Rows[0]["resultCount"] == 0)
                            {
                                error += "科目別不存在或已失效,\n";
                            }
                        }

                        //薪資部門區分存在於薪資部門區分設定檔(TB_H_M_DEPT_ACC)
                        if (acc_dept_no[i] == "")
                        {
                            error += "薪資部門區分欄位不可空白,\n";
                        }
                        else
                        {
                            dt.Clear();
                            dt = ha020DAO.getAccDeptNOCount(acc_dept_no[i]);
                            if ((int)dt.Rows[0]["resultCount"] == 0)
                            {
                                error += "薪資部門區分不存在或已失效,\n";
                            }
                        }

                        #endregion


                        #region 資料檢核
                        //1.不能同時有同部門代號及同生效日期
                        int t1 = Array.IndexOf(dept_no, dept_no[i]);
                        int t2 = Array.LastIndexOf(dept_no, dept_no[i]);
                        if (Array.IndexOf(checkDept_no, dept_no[i]) > -1)
                        {
                            error += "本文件內有相同的部門代號,\n";
                        }
                        else
                        {
                            checkDept_no[i] = dept_no[i];
                        }

                        //2.與DB的PK值不能重覆
                        dt.Clear();
                        if (dept_no[i].Length == 7 && DateTime.TryParse(start_dt[i], out dateFormat))
                        {
                            dt = ha020DAO.getPKDupData(dept_no[i], start_dt[i]);
                            if ((int)dt.Rows[0]["resultCount"] > 0)
                            {
                                error += "資料庫已有相同的部門代號及生效日期,\n";
                            }
                        }
                        //3.生效日期必須大於薪資結算月
                        dt.Clear();
                        if (DateTime.TryParse(start_dt[i], out dateFormat)) {
                            //salaryDT
                            startDT = Convert.ToDateTime(start_dt[i]);
                            if (startDT < salaryDT)
                            {
                                error += "生效日期需大於已計薪年月,\n";
                            }
                        }

                        //4.部門的起始日期區間不可重疊
                        //(1)生效日<DB生效日, 結束日 >= DB 生效日  >不處理,會有錯誤訊息
                        dt.Clear();
                        if (DateTime.TryParse(start_dt[i], out dateFormat) && DateTime.TryParse(end_dt[i], out dateFormat))
                        {
                            dt = ha020DAO.getDupTimeData(dept_no[i], start_dt[i], end_dt[i]);
                            if ((int)dt.Rows[0]["resultCount"] > 0)
                            {
                                error += "與資料庫生效區間重疊,\n";
                            }
                        }
                        //(2)生效日between DB生效日及DB結束日, 結束日between DB生效日及DB結束日(生效日 < 結束日 )=>進行update 前一筆的結束日期
                        dt.Clear();
                        isUpdate[i] = false;
                        if (DateTime.TryParse(start_dt[i], out dateFormat))
                        {
                            dt = ha020DAO.getDupTimeData_update(dept_no[i], start_dt[i]);
                            if ((int)dt.Rows[0]["resultCount"] > 0)
                            {
                                isUpdate[i] = true;
                            }
                        }


                        #endregion

                        //儲存錯誤訊息,將錯誤訊息寫進EXCEL第一欄
                        sheet.GetRow(i).CreateCell(0).CellStyle = errorStyle;
                        sheet.GetRow(i).GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(error);
                        if (error != "")
                        {
                            valid = false;
                        }
                    }
                } //檢核end

                #region 有錯匯出excel,沒錯寫入DB
                if (!valid)
                {
                    return workbook;
                    //檢核有錯，匯出附加說明的excel
                    //ExcelHandle.exportExcel(workbook, "error" + type);
                }
                else
                {
                    DateTime now = DateTime.Now;
                    BeginTransaction();
                    for (int i = 1; i <= sheet.LastRowNum; i++)
                    {
                        //新增
                        ha020DAO = new CFB2HA0200DAO();
                        try
                        {
                            ha020DAO.DEPT_NO = dept_no[i];
                            ha020DAO.START_DT = start_dt[i];
                            ha020DAO.END_DT = end_dt[i];
                            ha020DAO.DEPT_NAME = dept_name[i];
                            ha020DAO.DEPT_SNAME = dept_sname[i];
                            ha020DAO.DEPT_ENAME = dept_ename[i];
                            ha020DAO.HEAD_EMP_ID = head_emp_id[i];
                            ha020DAO.DEPT_LEVEL = dept_level[i];
                            ha020DAO.ORG_TYPE = org_type[i];
                            ha020DAO.ACC_CD = acc_cd[i];
                            ha020DAO.ACC_DEPT_NO = acc_dept_no[i];
                            ha020DAO.REMARK = remark[i];
                            ha020DAO.DEFAULT_PLANT = default_plant[i];

                            ha020DAO.CREATED_BY = SessionHandle.Current.emp_id;
                            ha020DAO.CREATED_DT = now;
                            ha020DAO.UPDATED_BY = SessionHandle.Current.emp_id;
                            ha020DAO.UPDATED_DT = now;
                            ha020DAO.FUNC_ID = "FB2HA020";

                            //更新 重覆的結束日期為前一天
                            if (isUpdate[i])
                            {
                                ha020DAO.updateDeptBefore();
                            }
                            //新增
                            ha020DAO.addDept();
                        }
                        catch (Exception ex)
                        {
                            RollBack();
                            throw;
                        }
                    }
                    Commit();
                }
                #endregion
            }
            return null;
            //return "0";
        }
        catch (Exception ex)
        {
            //return ex.Message;
            throw;
        }
    }
    #endregion


}
