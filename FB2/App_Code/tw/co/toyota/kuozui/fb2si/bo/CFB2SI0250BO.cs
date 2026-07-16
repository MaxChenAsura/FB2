using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;

/// <summary>
/// CFB2SI0250BO 的摘要描述
/// </summary>
public class CFB2SI0250BO : BaseService
{
    public CFB2SI0250BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    ICellStyle style_class;
  

   
    public IWorkbook uploadExcel1(Stream fs, string type, WFB2SI0250DAO si025dao)
    {
        //取得登入者
        string userid = SessionHandle.Current.emp_id;
          
        try
        {
            IWorkbook workbook;
            //依附檔名判斷要用哪種方式讀取
            if (type == ".xls")
            {
                workbook = new HSSFWorkbook(fs);
            }
            else if (type == ".xlsx")
            {
                workbook = new XSSFWorkbook(fs);
            }
            else
            {
                return null;
            }
          
            //取得sheet
            ISheet sheet = workbook.GetSheetAt(0);
            ICellStyle style1 = workbook.CreateCellStyle();
            IFont font1 = workbook.CreateFont();
            font1.Color = HSSFColor.Red.Index;
            style1.SetFont(font1);

            if (sheet != null)
            {
                try
                {
                    //1.初始值
                    DataTable excel_data = new DataTable();   //記錄EXCEL的資料
                    DataTable excel_pk_data = new DataTable();   //記錄EXCEL的資料
                    string[] excel_pk_arr = new string[1];         //用來判斷是否工號重複
                    DataRow dr;                     //查檢pk用

                    //取得考績的範圍
                    bool valid = true;

                    #region 建立 excel
                    //建立 DataTable,存放EXCEL的資料
                    DataRow excel_row; 
                    //建立 FieldSchema
                    excel_data.Columns.Add("EMP_ID", System.Type.GetType("System.String"));
                    excel_data.Columns.Add("EMP_NAME", System.Type.GetType("System.String"));
                    excel_data.Columns.Add("BONUS_AMT", System.Type.GetType("System.String"));

                    //存放EXCEL 檢查能否重複的資料
                    DataRow excel_pk_row;
                    excel_pk_data.Columns.Add("EMP_ID", System.Type.GetType("System.String"));

                    #endregion               

                    //2.取得excel的資料
                    string cell_empId = "";        //工號
                    string cell_empName= "";     //姓名
                    string cell_awardAMT = "";   //年獎金額

                    string error = "";

                    //巡覽每row的資料第一列為title跳過(故i從3開始)
                    for (int i = 1; i <= sheet.LastRowNum; i++)
                    {
                        
                        error = "";
                        si025dao.CREATED_BY = SessionHandle.Current.emp_id;
                        si025dao.UPDATED_BY = SessionHandle.Current.emp_id;

                        if (sheet.GetRow(i) != null)
                        {
                            cell_empId = sheet.GetRow(i).GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            cell_empName = sheet.GetRow(i).GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().ToUpper();
                            cell_awardAMT = sheet.GetRow(i).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().Replace(",","");
                            
                            //工號 不可空白
                            if (cell_empId == "")
                            {
                                error += "工號不可空白,\n";
                            }
                            else { 
                                //檢核是否在年獎對象內
                                if(si025dao.chkEMP_ID(cell_empId)==0)
                                    error += "工號不在紅利對象內或非2S以上人員,\n";
                            }

                           //姓名不可空白
                            if (cell_empName == "")
                            {
                                error += "姓名不可空白,\n";
                            }    
                        
                            //檢查工號,姓名是否一致
                            if (cell_empId != "" && cell_empName != "")
                            {
                                if (si025dao.chkEMP_ID_NAME(cell_empId, cell_empName) == 0)
                                    error += "工號與姓名不在紅利對象內,\n";
                            }

                            //檢查數字欄位
                            error += utilities.checkNumber(cell_awardAMT, "金額", 8, false);

                            //若有值,檢查工號是否重覆
                            excel_pk_arr[0] = cell_empId;
                            if (excel_pk_data.Rows.Count > 0)
                            {
                                dr = excel_pk_data.Rows.Find(excel_pk_arr);
                                if (dr != null)
                                {
                                    error += "此EXCEL有相同的工號\n";
                                }
                                else
                                {
                                    excel_pk_row = excel_pk_data.NewRow();
                                    excel_pk_row["EMP_ID"] = cell_empId;
                                    excel_pk_data.Rows.Add(excel_pk_row);
                                    excel_pk_data.PrimaryKey =new DataColumn[] { excel_pk_data.Columns["EMP_ID"]};
                                }
                            }
                            else
                            {
                                excel_pk_row = excel_pk_data.NewRow();
                                excel_pk_row["EMP_ID"] = cell_empId;
                                excel_pk_data.Rows.Add(excel_pk_row);
                                excel_pk_data.PrimaryKey = new DataColumn[] { excel_pk_data.Columns["EMP_ID"] };
                            }

                            excel_row = excel_data.NewRow();
                            excel_row["EMP_ID"] = cell_empId;
                            excel_row["EMP_NAME"] = cell_empName;
                            excel_row["BONUS_AMT"] = cell_awardAMT;
                            excel_data.Rows.Add(excel_row);


                            //傳出錯誤訊息
                            style1.SetFont(font1);
                            sheet.GetRow(i).CreateCell(0).CellStyle = style1;
                            sheet.GetRow(i).GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(error);
                            if (error != "")
                            {
                                valid = false;
                            }
                        }
                    }

                    //若只有title時 ,儲存錯誤訊息
                    if (sheet.LastRowNum < 1)
                    {
                        error = "EXCEL無資料";
                        sheet.CreateRow(1);
                        sheet.GetRow(1).CreateCell(0).CellStyle = style1;
                        sheet.GetRow(1).GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(error);
                        if (error != "")
                        {
                            valid = false;
                        }
                    }

                    //檢核有錯，匯出附加說明的excel
                    if (!valid)
                    {
                        return workbook;
                        //ExcelHandle.exportExcel(workbook, "檢核錯誤說明" + type);
                    }

                    //檢核正確,修改考績
                    if (valid)
                    {
                        try
                        {
                            BeginTransaction();
                            for (int j = 0; j < excel_data.Rows.Count; j++)
                            {
                                si025dao.EMP_ID = excel_data.Rows[j]["EMP_ID"].ToString();
                                si025dao.BONUS_AMT = excel_data.Rows[j]["BONUS_AMT"].ToString();
                                si025dao.CREATED_BY = userid;
                                si025dao.UPDATED_BY = userid;
                                si025dao.FUNC_ID = "FB2SI025";
                                si025dao.updateAward_DM();
                            }

                            si025dao.updateAward_H();

                            Commit();
                        }
                        catch (Exception ex)
                        {
                            RollBack();
                            throw;
                            //return ex.Message;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw;
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
 
        }

    }

  

}


