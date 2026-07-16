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

/// <summary>
/// CFB2HB0400BO 的摘要描述
/// </summary>
public class CFB2HB0400BO : BaseService
{
    public CFB2HB0400BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public string delete_Training(List<Tuple<string, string>> training)
    {
        CFB2HB0400DAO dao = new CFB2HB0400DAO();
        try
        {
            BeginTransaction();

            foreach (var item in training)
            {
                dao.delete_Training(item.Item1, item.Item2);
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

    public string updateTraining(CFB2HB0400DAO fb2hb040)
    {
        try
        {
            BeginTransaction();

            fb2hb040.updateTraining();

            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }



    public string addTraining(CFB2HB0400DAO fb2hb040)
    {
        try
        {
            DataTable ExistEMP = fb2hb040.getExitEmp(fb2hb040.EMP_ID);
            if (ExistEMP.Rows.Count == 0)
                return "工號不存在人事主檔!";
            DataTable tmp = fb2hb040.getExistData(fb2hb040.EMP_ID, fb2hb040.START_DT);
            if (tmp.Rows.Count > 0)
                return "國外研修資料重覆";
            else
            {
                try
                {
                    BeginTransaction();

                    fb2hb040.addTraining();

                    Commit();
                }
                catch (Exception ex)
                {
                    RollBack();
                    return ex.Message;
                }


            }
            return "0";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public System.Data.DataTable getEMPData(string EMP_ID)
    {
        try
        {
            CFB2HB0400DAO dao = new CFB2HB0400DAO();

            return dao.getEMPData(EMP_ID);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public IWorkbook uploadExcel(Stream fs, string type)
    {
        try
        {
            bool valid = true;
            CFB2HB0400DAO dao = new CFB2HB0400DAO();
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
                try
                {
                    BeginTransaction();
                    //巡覽每row的資料第一列為title跳過
                    for (int i = 1; i <= sheet.LastRowNum; i++)
                    {
                        bool date1 = true;
                        bool date2 = true;
                        if (sheet.GetRow(i) != null)
                        {
                            //讀取cell資料，第一欄為檢核結果欄位跳過
                            string cell1 = sheet.GetRow(i).GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            string cell2 = sheet.GetRow(i).GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            //string cell3 = Convert.ToDateTime(sheet.GetRow(i).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim()).ToString("yyyy/MM/dd");
                            //string cell4 = Convert.ToDateTime(sheet.GetRow(i).GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim()).ToString("yyyy/MM/dd");
                            string cell3 = sheet.GetRow(i).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            string cell4 = sheet.GetRow(i).GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            string cell5 = sheet.GetRow(i).GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            string cell6 = sheet.GetRow(i).GetCell(6, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();


                            string error = "";
                            int numCheckResult = 0;
                            //檢查第一欄
                            if (cell1 == "")
                            {
                                error += "工號欄位不可空白\n";
                            }
                            else
                            {

                                if (cell1.Trim().Length != 5 || !int.TryParse(cell1.Trim(), out numCheckResult))
                                    error += "工號必須為數字, 且長度必須為5\n";

                            }

                            //檢查必填欄位
                            if (cell2 == "")
                                error += "部門代號欄位不可空白\n";
                            else
                            {
                                if (cell2.Trim().Length != 7 || !utilities.IsNatural_Number(cell2))
                                {
                                    error += "部門代號必須為英數字, 且長度必須為7\n";
                                }
                                else
                                {
                                    DataTable tmp = dao.getDeptData(cell2);
                                    if (tmp.Rows.Count == 0)
                                    {
                                        error += "部門代號不存在或失效 \n";
                                    }
                                }
                            }
                            if (cell3 == "")
                                error += "研修起日欄位不可空白\n";
                            else
                            {
                                DateTime rtn;
                                if (!DateTime.TryParse(cell3, out rtn))
                                {
                                    error += "研修起日的內容不是合理的日期\n";
                                    date1 = false;
                                }
                                else
                                    date1 = true;
                            }
                            if (cell4 == "")
                                error += "研修迄日欄位不可空白\n";
                            else
                            {
                                DateTime rtn;
                                if (!DateTime.TryParse(cell4, out rtn))
                                {
                                    error += "研修迄日的內容不是合理的日期\n";
                                    date2 = false;
                                }
                                else
                                    date2 = true;
                            }

                            DateTime tmp13;
                            DateTime tmp14;
                            if (DateTime.TryParse(cell3, out tmp13) && DateTime.TryParse(cell4, out tmp14))
                            {
                                if (tmp13 > tmp14)
                                {
                                    error += "研修起日不可大於研修迄日\n";
                                }
                            }

                            if (cell5 == "")
                                error += "受入單位欄位不可空白\n";
                            else
                            {
                                if (cell5.Trim().Length > 150)
                                    error += "受入單位長度必須為150 \n";
                            }
                            if (cell6 == "")
                                error += "研修目的欄位不可空白\n";
                            else
                            {
                                if (cell6.Trim().Length > 150)
                                    error += "研修目的長度必須為150 \n";
                            }

                            if (cell1 != "" && cell3 != "" && date1)
                            {
                                DataTable tmp = dao.getExistDataT(cell1, cell3);
                                if (tmp.Rows.Count > 0)
                                {
                                    error += "國外研修資料重覆 \n";
                                }
                            }

                            /*
                            if (cell1 != "" && cell2 != "" && date2)
                            {
                                DataTable tmp = dao.getDeptData(cell1, cell2);
                                if (tmp.Rows.Count == 0)
                                {
                                    error += "部門代號與員工所屬部級部門不符 \n";
                                }
                            }
                            */
                            if (cell1 != "" && cell3 != "" && cell4 != "" && date1 && date2)
                            {
                                DataTable tmp = dao.getDupData(cell1, cell3, cell4);
                                if (tmp.Rows.Count > 0)
                                {
                                    error += "研修期間重疊 \n";
                                }
                            }

                            sheet.GetRow(i).GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(error);
                            if (error != "")
                            {
                                valid = false;

                            }
                            else
                            {
                                dao.EMP_ID = cell1;
                                dao.START_DT = cell3;
                                dao.END_DT = cell4;
                                dao.TRAINING_COMPANY = cell5;
                                dao.TRAINING_GOAL = cell6;

                                dao.CREATED_BY = SessionHandle.Current.emp_id;
                                dao.UPDATED_BY = SessionHandle.Current.emp_id;
                                dao.FUNC_ID = "FB2HB040";
                                dao.addTraining();

                            }

                        }
                    }
                    if (!valid)
                    {
                        RollBack();
                        return workbook;
                        //檢核有錯，匯出附加說明的excel
                        //ExcelHandle.exportExcel(workbook, "檢核錯誤說明" + type);
                    }
                    else
                        Commit();
                }
                catch (Exception ex)
                {
                    RollBack();
                    throw;
                }
            }

            //return "0";
            return null;
        }
        catch (Exception ex)
        {
            throw;
            //return ex.Message;

        }

    }

    public string getEmpName(string emp_id)
    {
        try
        {
            CFB2HB0400DAO dao = new CFB2HB0400DAO();
            DataTable dt = dao.getEmpName(emp_id);
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["EMP_NAME"].ToString();
            }
            else
                return "";
        }
        catch (Exception)
        {

            throw;
        }
    }
}