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
/// CFB2HB0500BO 的摘要描述
/// </summary>
public class CFB2HB0500BO : BaseService
{
    public CFB2HB0500BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public System.Data.DataTable getSKILL_GRADE()
    {
        try
        {
            CFB2HB0500DAO dao = new CFB2HB0500DAO();
            return dao.getSKILL_GRADE();
        }
        catch (Exception)
        {

            throw;
        }
    }

    public string delete_Skill(List<Tuple<string, string, string>> emp_id)
    {
        CFB2HB0500DAO dao = new CFB2HB0500DAO();
        try
        {
            BeginTransaction();

            foreach (var item in emp_id)
            {
                dao.delete_Skill(item.Item1, item.Item2, item.Item3);
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

    public string addSkill(CFB2HB0500DAO fb2hb050)
    {
        try
        {
            DataTable tmp = fb2hb050.getExistData(fb2hb050.EMP_ID, fb2hb050.SKILL_TYPE, fb2hb050.SKILL_DESC);
            if (tmp.Rows.Count > 0)
                return "技能專長資料重覆";
            else
            {
                try
                {
                    BeginTransaction();

                    fb2hb050.addSkill();

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

    public string updateSkill(CFB2HB0500DAO fb2hb050)
    {
        try
        {
            BeginTransaction();

            fb2hb050.updateSkill();

            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    public string uploadExcel(Stream fs, string type)
    {
        try
        {
            bool valid = true;
            CFB2HB0500DAO dao = new CFB2HB0500DAO();
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
                            string cell3 = sheet.GetRow(i).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            string cell4 = sheet.GetRow(i).GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            string cell5 = sheet.GetRow(i).GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            string cell6 = sheet.GetRow(i).GetCell(6, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            string cell7 = sheet.GetRow(i).GetCell(7, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();


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
                                error += "部門代號欄位不可空白\n";
                            else
                            {
                                if (cell2.Trim().Length != 7 || !utilities.IsNatural_Number(cell2))
                                {
                                    error += "部門代號必須為英數字, 且長度必須為7\n";
                                }
                            }
                            if (cell3 == "")
                                error += "技能專長類別欄位不可空白\n";
                            else
                            {
                                if (cell3.Trim() != "1" && cell3.Trim() != "2" && cell3.Trim() != "3")
                                    error += "技能專長類別必須為1或2或3 \n";
                                if (cell3.Trim() == "1" || cell3.Trim() == "2")
                                {
                                    if (cell5.Trim() == "")
                                        error += "技能專長類別為外語或證照, 則外語等級/證照等級欄 不可為空白 \n";
                                    if (cell6.Trim() == "")
                                        error += "技能專長類別為外語或證照, 則認證機構欄 不可為空白 \n";
                                }
                                if (cell3== "3")
                                {
                                    if (cell7 == "")
                                        error += "技能專長類別為獲獎, 則獲獎日期欄 不可為空白 \n";
                                }
                            }
                            if (cell4 == "")
                                error += "外語名稱/證照名稱/獲獎獎項欄位不可空白\n";

                            if (cell7 != "")
                            {
                                DateTime rtn;
                                if (!DateTime.TryParse(cell7, out rtn))
                                {
                                    error += "獲獎日期不是合理的日期\n";
                                    date2 = false;
                                }
                                else
                                    date2 = true;
                            }
                            

                            if (cell1 != "" && cell2 != "" && cell3 != "")
                            {
                                DataTable tmp = dao.getExistData(cell1,cell2, cell3);
                                if (tmp.Rows.Count > 0)
                                {
                                    error += "技能專長資料重覆 \n";
                                }
                            }


                            if (cell1 != "" && cell2 != "")
                            {
                                DataTable tmp = dao.getDeptData(cell1, cell2);
                                if (tmp.Rows.Count == 0)
                                {
                                    error += "部門代號與員工所屬部級部門不符 \n";
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
                                dao.SKILL_TYPE = cell3;
                                dao.SKILL_DESC = cell4;
                                dao.SKILL_GRADE = cell5;
                                dao.SKILL_ORG = cell6;
                                dao.AWARD_DT = cell7;

                                dao.CREATED_BY = SessionHandle.Current.emp_id;
                                dao.UPDATED_BY = SessionHandle.Current.emp_id;
                                dao.FUNC_ID = "FB2HB050";

                                
                                dao.addSkill();

                            }

                        }
                    }
                    if (!valid)
                    {
                        RollBack();
                        //檢核有錯，匯出附加說明的excel
                        ExcelHandle.exportExcel(workbook, "檢核錯誤說明" + type);
                    }
                    else
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
}