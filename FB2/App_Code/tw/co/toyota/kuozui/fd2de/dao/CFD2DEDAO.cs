using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;
using System.Text;
using System.Collections;

/// <summary>
/// wfd2de 的摘要描述
/// </summary>
public class CFD2DEDAO : BaseDAO
{

    public string MANAGER_YM { get; set; }

    public CFD2DEDAO()
    {

    }

    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string MANAGER_YM_S, string MANAGER_YM_E)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" MANAGER_YM,CONVERT(char(10), SALARY_TAKE_OUT_DT,111) SALARY_TAKE_OUT_DT ,'' remark");
            sb.Append(" from TB_D_M_ACCOM_MONTH ");
            sb.Append(" where MANAGER_YM is not null");

            if (MANAGER_YM_S != "")
            {
                if (MANAGER_YM_E != "")
                {
                    sb.Append(" and MANAGER_YM >= @MANAGER_YM_S and MANAGER_YM <= @MANAGER_YM_E ");
                    ht.Add("@MANAGER_YM_S", MANAGER_YM_S);
                    ht.Add("@MANAGER_YM_E", MANAGER_YM_E);
                }
                else
                {
                    sb.Append(" and MANAGER_YM >= @MANAGER_YM_S  ");
                    ht.Add("@MANAGER_YM_S", MANAGER_YM_S);
                }

            }
            else if (MANAGER_YM_E != "")
            {
                sb.Append(" and MANAGER_YM <= @MANAGER_YM_E  ");
                ht.Add("@MANAGER_YM_E", MANAGER_YM_E);
            }

            sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar)");


            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    public int getCount(int startRowIndex, int maximumRows, string MANAGER_YM_S, string MANAGER_YM_E)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select COUNT(*) total_record From TB_D_M_ACCOM_MONTH Where MANAGER_YM is not null");
            if (MANAGER_YM_S != "")
            {
                if (MANAGER_YM_E != "")
                {
                    sb.Append(" and MANAGER_YM >= @MANAGER_YM_S and MANAGER_YM <= @MANAGER_YM_E ");
                    ht.Add("@MANAGER_YM_S", MANAGER_YM_S);
                    ht.Add("@MANAGER_YM_E", MANAGER_YM_E);
                }
                else
                {
                    sb.Append(" and MANAGER_YM >= @MANAGER_YM_S  ");
                    ht.Add("@MANAGER_YM_S", MANAGER_YM_S);
                }

            }
            else if (MANAGER_YM_E != "")
            {
                sb.Append(" and MANAGER_YM <= @MANAGER_YM_E  ");
                ht.Add("@MANAGER_YM_E", MANAGER_YM_E);
            }
            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                t = (int)dt.Rows[0]["total_record"];
            }


            return t;


        }
        catch (Exception)
        {

            throw;
        }

    }

    public void setSALARY_TAKE_OUT_DT(string MANAGER_YM)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //BeginTransaction();
            sb.Append("Update TB_D_M_ACCOM_MONTH Set SALARY_TAKE_OUT_DT = GETDATE() where MANAGER_YM = @MANAGER_YM");
            ht.Add("@MANAGER_YM", MANAGER_YM);
            dbConn.ExecuteT(sb, ht, true);
            // Commit();
        }
        catch (Exception)
        {
            //RollBack();
            throw;
        }
    }

    public void createExcelFromTemplate(string type, string excelPath)
    {
        try
        {
            IWorkbook workbook;
            FileStream fs = new FileStream(excelPath, FileMode.Open, FileAccess.Read);
            //依type判斷要用哪種方式產生
            if (type == "xls")
                workbook = new HSSFWorkbook(fs);
            else
                workbook = new XSSFWorkbook(fs);

            //取得範本sheet
            ISheet sheet = workbook.GetSheetAt(0);
            if (sheet != null)
            {
                DataTable dt = getExcelData();
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        //將資料寫入範本
                        sheet.GetRow(i + 1).GetCell(0).SetCellValue(dt.Rows[i]["MANAGER_YM"].ToString());
                    }
                }
                //匯出Excel
                ExcelHandle.exportExcel(workbook, "住宿清冊." + type);
            }


        }
        catch (Exception)
        {

            throw;
        }
    }

    private DataTable getExcelData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select ");
            sb.Append(" MANAGER_YM,CONVERT(char(10), SALARY_TAKE_OUT_DT,111) SALARY_TAKE_OUT_DT ,'' remark");
            sb.Append(" from TB_D_M_ACCOM_MONTH ");
            sb.Append(" where MANAGER_YM = @MANAGER_YM ");


            ht.Add("@MANAGER_YM", MANAGER_YM);


            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public void getExcelData(Stream fs, string type)
    {
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
            if (sheet != null)
            {
                //巡覽每row的資料第一列為title跳過
                for (int i = 1; i <= sheet.LastRowNum; i++)
                {
                    if (sheet.GetRow(i) != null)
                    {
                        //讀取cell資料，第一欄為檢核結果欄位跳過
                        string cell1 = sheet.GetRow(i).GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString();
                        string cell2 = sheet.GetRow(i).GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString();
                        string cell3 = sheet.GetRow(i).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString();
                        string cell4 = sheet.GetRow(i).GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString();
                        string cell5 = sheet.GetRow(i).GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString();
                        string cell6 = sheet.GetRow(i).GetCell(6, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString();


                        string error = "";
                        int numCheckResult = 0;
                        //檢查第一欄
                        if (cell1 == "")
                            error += "資料年月欄位不可空白\n";
                        else
                        {

                            if (cell1.Trim().Length != 6 || !int.TryParse(cell1.Trim(), out numCheckResult))
                                error += "資料年月日期錯誤\n";
                            string year = cell1.Trim().Substring(0, 4);
                            string month = cell1.Trim().Substring(4, 2);

                            if (numCheckResult > int.Parse(DateTime.Now.ToString("yyyyMM")))
                            {
                                error += "資料年月不正確\n";
                            }
                        }



                        //檢查必填欄位
                        if (cell2 == "")
                            error += "薪資項目代號欄位不可空白\n";
                        if (cell3 == "")
                            error += "工號欄位不可空白\n";
                        if (cell4 == "")
                            error += "姓名欄位不可空白\n";
                        if (cell5 == "")
                            error += "加扣款金額欄位不可空白\n";
                        if (cell6 == "")
                            error += "備註說明欄位不可空白\n";


                        if (!int.TryParse(cell5.Trim(), out numCheckResult))
                            error += "加扣款金額必須為數字,且不可為負數!\n";
                        if (numCheckResult < 0)
                            error += "加扣款金額必須為數字,且不可為負數!\n";


                        sheet.GetRow(i).GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(error);
                        if (error != "")
                            valid = false;

                    }
                }
                if (!valid)
                {
                    //檢核有錯，匯出附加說明的excel
                    ExcelHandle.exportExcel(workbook, "檢核錯誤說明" + type);
                }
            }
        }
        catch (Exception)
        {

            throw;
        }
    }
}