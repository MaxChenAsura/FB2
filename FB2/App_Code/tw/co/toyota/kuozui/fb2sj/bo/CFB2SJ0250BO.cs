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
/// CFB2SJ0250BO 的摘要描述
/// </summary>
public class CFB2SJ0250BO : BaseService
{
    public CFB2SJ0250BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    ICellStyle style_class;
  

    /// <summary>
    /// 判斷考核類,資格,及考績結果的正確性
    /// </summary>
    /// <param name="assess_type">考核類型</param>
    /// <param name="assess_score">考績</param>
    /// <param name="level_cd">資格</param>
    /// <param name="score_str">考績範圍 S~J/ A~E</param>
    /// <param name="remark">部門提出/最終考績</param>
    /// <returns></returns>
    private string chkScore(string assess_type, string assess_score,string level_cd,string score_str,string remark) 
    {
        string rtnmessage = "";
        try
        {
            //檢查能力(S~J)/業績考課(A~E)的範圍正確性
            if (assess_type == "1" && score_str.IndexOf(assess_score) < 0)
            {
                rtnmessage = "能力考課-"+remark+"無法為" + assess_score + ",\n";        
            }
            if (assess_type == "2" && score_str.IndexOf(assess_score) < 0)
            {
                rtnmessage = "業績考課-" + remark + "無法為" + assess_score + ",\n";
            }


            // 能力考課時,2S 考績才能  SFGHIJ
            if (assess_type == "1" && "SFGHIJ".IndexOf(assess_score) > -1)
            {
                //檢查2S 考績才能  SFGHIJ
                if (level_cd != "2S")
                {
                    rtnmessage = "非2S人員-能力考課" + remark + "無法為" + assess_score + ",\n";
                }            
            }

            return rtnmessage;
        }
        catch (Exception)
        {

            throw;
        }
        finally
        {
 
        }
    }

    public IWorkbook uploadExcel1(Stream fs, string type, WFB2SJ0250DAO sj025dao)
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
                    string score_str = sj025dao.getScore_Str(sj025dao.ASSESS_TYPE);
                    bool valid = true;


                    #region 建立 excel
                    //建立 DataTable,存放EXCEL的資料
                    DataRow excel_row; 
                    //建立 FieldSchema
                    excel_data.Columns.Add("EMP_ID", System.Type.GetType("System.String"));
                    excel_data.Columns.Add("LEVEL_CD", System.Type.GetType("System.String"));
                    excel_data.Columns.Add("SCORE_DEPT", System.Type.GetType("System.String"));
                    excel_data.Columns.Add("SCORE_FINAL", System.Type.GetType("System.String"));

                    //存放EXCEL 檢查能否重複的資料
                    DataRow excel_pk_row;
                    excel_pk_data.Columns.Add("EMP_ID", System.Type.GetType("System.String"));

                    #endregion               

                    //2.取得excel的資料
                    string cell_empId = "";        //工號
                    string cell_level_cd = "";     //資格
                    string cell_score_dept = "";   //部門提出
                    string cell_score_final = "";  //最終考績

                    string error = "";


                    //巡覽每row的資料第一列為title跳過(故i從3開始)
                    for (int i = 3; i <= sheet.LastRowNum; i++)
                    {
                        
                        error = "";
                        sj025dao.CREATED_BY = SessionHandle.Current.emp_id;
                        sj025dao.UPDATED_BY = SessionHandle.Current.emp_id;


                        if (sheet.GetRow(i) != null)
                        {
                            cell_empId = sheet.GetRow(i).GetCell(6, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            cell_level_cd = sheet.GetRow(i).GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().ToUpper();
                            cell_score_dept = sheet.GetRow(i).GetCell(8, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().ToUpper();
                            cell_score_final = sheet.GetRow(i).GetCell(9, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().ToUpper();
                            

                            //工號 不可空白
                            if (cell_empId == "")
                            {
                                error += "工號不可空白,\n";
                            }
                           //資格不可空白
                            if (cell_level_cd == "")
                            {
                                error += "資格不可空白,\n";
                            }                            
                            //部門提出不可空白
                            if (cell_score_dept == "")
                            {
                                error += "部門提出不可空白,\n";
                            }
                            else
                            {
                                //部門提出考績範圍
                                error += chkScore(sj025dao.ASSESS_TYPE, cell_score_dept, cell_level_cd, score_str, "部門提出");
                            }
                            //最終考課不可空白
                            if (cell_score_final == "")
                            {
                                error += "最終考課不可空白,\n";
                            }
                            else {
                                //最終考課考績範圍
                                error += chkScore(sj025dao.ASSESS_TYPE, cell_score_final, cell_level_cd, score_str, "最終考課"); 
                            }

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
                            excel_row["LEVEL_CD"] = cell_level_cd;
                            excel_row["SCORE_DEPT"] = cell_score_dept;
                            excel_row["SCORE_FINAL"] = cell_score_final;
                            excel_data.Rows.Add(excel_row);


                            //傳出錯誤訊息
                            style1.SetFont(font1);
                            sheet.GetRow(i).CreateCell(12).CellStyle = style1;
                            sheet.GetRow(i).GetCell(12, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(error);
                            if (error != "")
                            {
                                valid = false;
                            }
                        }
                    }

                    //若只有title時 ,儲存錯誤訊息
                    if (sheet.LastRowNum < 3)
                    {
                        error = "EXCEL無資料";
                        sheet.CreateRow(3);
                        sheet.GetRow(3).CreateCell(12).CellStyle = style1;
                        sheet.GetRow(3).GetCell(12, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(error);
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
                                sj025dao.EMP_ID = excel_data.Rows[j]["EMP_ID"].ToString();
                                sj025dao.SCORE_DEPT = excel_data.Rows[j]["SCORE_DEPT"].ToString();
                                sj025dao.SCORE_FINAL = excel_data.Rows[j]["SCORE_FINAL"].ToString();
                                sj025dao.SCORE_FLAG = "";
                                if (sj025dao.SCORE_DEPT != sj025dao.SCORE_FINAL)
                                    sj025dao.SCORE_FLAG="V";
                                sj025dao.CREATED_BY = userid;
                                sj025dao.UPDATED_BY = userid;
                                sj025dao.FUNC_ID = "FB2SJ025";
                                sj025dao.updateAssessScore_ALL();
                            }
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


