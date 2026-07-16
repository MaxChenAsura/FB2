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
/// CFB2DA0400BO 的摘要描述
/// </summary>
public class WFB2DA0400BO : BaseService
{
    public WFB2DA0400BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    ICellStyle style_class;
    public string deleteData(List<Tuple<string, string, string>> deleteList)
    {
        try
        {
            WFB2DA0400DAO dao = new WFB2DA0400DAO();
            BeginTransaction();

            foreach (var deleteitem in deleteList)
            {
                //刪除行事曆群組設定檔資料
                dao.deleteData(deleteitem.Item1, deleteitem.Item2, deleteitem.Item3);
            }
            Commit();
            return "0";
        }
        catch (Exception)
        {
            RollBack();
            throw;
        }
    }

    public string deleteAllData(string year, string calendar_cd, string group_cd)
    {
        try
        {
            WFB2DA0400DAO dao = new WFB2DA0400DAO();
            BeginTransaction();
            //刪除行事曆群組設定檔資料
            dao.deleteAllData(year, calendar_cd, group_cd);
            Commit();
            return "0";
        }
        catch (Exception)
        {
            RollBack();
            throw;
        }
    }

    public string SP_DA040_01(WFB2DA0400DAO dao)
    {
        try
        {
            string result = "0";
            //call sp
            int err = dao.SP_DA040_01(dao);

            //確認SP有無成功
            DataTable dtSPresult = dao.checkSP("SP_DA040_01");
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

    public IWorkbook uploadExcel1(Stream fs, string type, WFB2DA0400DAO dao)
    {
        //取得登入者
        string userid = SessionHandle.Current.emp_id;
        //要載入的資料表名稱
        string tableName = "TB_D_M_CALENDAR_GROUP";

        bool valid = true;
        DataTable myTable = new DataTable("myTable");
        DataTable excel_dt = new DataTable();
        string[] excel_pk;
        DataTable calendar_cd_dt = new DataTable();
        DataTable parts_dt = new DataTable();

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
                myTable.Columns.Add("CALENDAR_CD", System.Type.GetType("System.String"));
                myTable.Columns.Add("GROUP_CD", System.Type.GetType("System.String"));
                myTable.Columns.Add("START_DT", System.Type.GetType("System.DateTime"));
                myTable.Columns.Add("END_DT", System.Type.GetType("System.DateTime"));
                myTable.Columns.Add("CREATED_BY", System.Type.GetType("System.String"));
                myTable.Columns.Add("CREATED_DT", System.Type.GetType("System.DateTime"));
                myTable.Columns.Add("UPDATED_BY", System.Type.GetType("System.String"));
                myTable.Columns.Add("UPDATED_DT", System.Type.GetType("System.DateTime"));
                myTable.Columns.Add("FUNC_ID", System.Type.GetType("System.String"));

                #endregion

                #region 建立excel PK值

                DataRow excel_row;
                excel_dt.Columns.Add("CALENDAR_CD", System.Type.GetType("System.String"));
                excel_dt.Columns.Add("GROUP_CD", System.Type.GetType("System.String"));
                excel_dt.Columns.Add("START_DT", System.Type.GetType("System.String"));
                #endregion

                if (sheet.LastRowNum != 0)
                {
                    #region 取得行事曆主檔的PK值

                    calendar_cd_dt = dao.getAll_CALENDAR_CD();
                    calendar_cd_dt.PrimaryKey = new DataColumn[] { calendar_cd_dt.Columns["CALENDAR_CD"] };

                    #endregion
                }

                //巡覽每row的資料第一列為title跳過
                for (int i = 1; i <= sheet.LastRowNum; i++)
                {
                    if (sheet.GetRow(i) != null)
                    {
                        error = "";
                        excel_pk = new string[3];

                        #region 讀取cell資料，第一欄為檢核結果欄位跳過
                        dao.CALENDAR_CD = sheet.GetRow(i).GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().ToUpper();
                        dao.GROUP_CD = sheet.GetRow(i).GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Replace(",", "").Trim();
                        dao.START_DT = sheet.GetRow(i).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().ToUpper();
                        dao.END_DT = sheet.GetRow(i).GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().ToUpper();

                        excel_pk[0] = dao.CALENDAR_CD;
                        excel_pk[1] = dao.GROUP_CD;
                        excel_pk[2] = dao.START_DT;
                        #endregion

                        #region 檢核基本邏輯
                        //長度檢核
                        error += utilities.checkLength(dao.CALENDAR_CD, "行事曆", 3, false);
                        error += utilities.checkLength(dao.GROUP_CD, "群組代碼", 4, false);
                        //日期檢核
                        error += utilities.checkDateFormat(dao.START_DT, "開始日期", false);
                        error += utilities.checkDateFormat(dao.END_DT, "結束日期", false);

                        //格式檢核
                        //行事曆代碼
                        DataRow dr;
                        if (dao.CALENDAR_CD != "" & dao.CALENDAR_CD != "All")
                        {
                            //存在否 行事曆主檔 
                            dr = calendar_cd_dt.Rows.Find(dao.CALENDAR_CD);
                            if (dr == null)
                            {
                                error += "行事曆代碼 不存在 行事曆主檔\n";
                            }
                        }

                        //excel的PK值
                        if (excel_dt.Rows.Count > 0)
                        {
                            dr = excel_dt.Rows.Find(excel_pk);
                            if (dr != null)
                            {
                                error += "此EXCEL有相同的 行事曆代碼+群組代碼+日期起\n";
                            }
                            else
                            {
                                #region 建立excel PK值資料

                                excel_row = excel_dt.NewRow();
                                excel_row["CALENDAR_CD"] = dao.CALENDAR_CD;
                                excel_row["GROUP_CD"] = dao.GROUP_CD;
                                excel_row["START_DT"] = dao.START_DT;
                                excel_dt.Rows.Add(excel_row);

                                excel_dt.PrimaryKey =
                                new DataColumn[] { 
                                    excel_dt.Columns["CALENDAR_CD"], 
                                    excel_dt.Columns["GROUP_CD"],
                                    excel_dt.Columns["START_DT"]
                                };

                                #endregion
                            }
                        }
                        else
                        {
                            #region 建立excel PK值資料

                            excel_row = excel_dt.NewRow();
                            excel_row["CALENDAR_CD"] = dao.CALENDAR_CD;
                            excel_row["GROUP_CD"] = dao.GROUP_CD;
                            excel_row["START_DT"] = dao.START_DT;
                            excel_dt.Rows.Add(excel_row);

                            excel_dt.PrimaryKey =
                            new DataColumn[] { 
                                    excel_dt.Columns["CALENDAR_CD"], 
                                    excel_dt.Columns["GROUP_CD"],
                                    excel_dt.Columns["START_DT"]
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
                            if (dao.CALENDAR_CD != "All")
                            {
                                #region 建立資料

                                // 建立資料
                                myRow = myTable.NewRow();
                                myRow["CALENDAR_CD"] = dao.CALENDAR_CD;
                                myRow["GROUP_CD"] = dao.GROUP_CD;
                                myRow["START_DT"] = dao.START_DT;
                                myRow["END_DT"] = dao.END_DT;
                                myRow["CREATED_BY"] = userid;
                                myRow["CREATED_DT"] = DateTime.Now;
                                myRow["UPDATED_BY"] = userid;
                                myRow["UPDATED_DT"] = DateTime.Now;
                                myRow["FUNC_ID"] = "FB2DA040";
                                myTable.Rows.Add(myRow);

                                #endregion
                            }
                            else
                            {
                                for (int j = 0; j < calendar_cd_dt.Rows.Count; j++)
                                {
                                    #region 建立資料

                                    // 建立資料
                                    myRow = myTable.NewRow();
                                    myRow["CALENDAR_CD"] = calendar_cd_dt.Rows[j]["CALENDAR_CD"].ToString();
                                    myRow["GROUP_CD"] = dao.GROUP_CD;
                                    myRow["START_DT"] = dao.START_DT;
                                    myRow["END_DT"] = dao.END_DT;
                                    myRow["CREATED_BY"] = userid;
                                    myRow["CREATED_DT"] = DateTime.Now;
                                    myRow["UPDATED_BY"] = userid;
                                    myRow["UPDATED_DT"] = DateTime.Now;
                                    myRow["FUNC_ID"] = "FB2DA040";
                                    myTable.Rows.Add(myRow);

                                    #endregion
                                }

                            }
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
                        if (excel_dt.Rows.Count < 700)
                        {
                            //內送參數最多2100個
                            dao.deleteAll_TB_D_M_CALENDAR_GROUP(excel_dt);
                        }
                        else
                        {
                            int flag = 0;
                            #region 建立暫存excel PK值

                            DataRow parts_row;
                            parts_dt.Columns.Add("CALENDAR_CD", System.Type.GetType("System.String"));
                            parts_dt.Columns.Add("GROUP_CD", System.Type.GetType("System.String"));
                            parts_dt.Columns.Add("USER_TYPE", System.Type.GetType("System.String"));
                            #endregion

                            for (int i = 0; i < excel_dt.Rows.Count; i++)
                            {
                                flag++;

                                #region 建立暫存excel PK值資料

                                parts_row = parts_dt.NewRow();
                                parts_row["CALENDAR_CD"] = excel_dt.Rows[i]["CALENDAR_CD"];
                                parts_row["GROUP_CD"] = excel_dt.Rows[i]["GROUP_CD"];
                                parts_row["USER_TYPE"] = excel_dt.Rows[i]["USER_TYPE"];
                                parts_dt.Rows.Add(parts_row);
                                #endregion

                                if (flag == 700)
                                {
                                    dao.deleteAll_TB_D_M_CALENDAR_GROUP(parts_dt);

                                    flag = 0;

                                    #region 建立暫存excel PK值
                                    parts_dt = new DataTable();
                                    parts_dt.Columns.Add("CALENDAR_CD", System.Type.GetType("System.String"));
                                    parts_dt.Columns.Add("GROUP_CD", System.Type.GetType("System.String"));
                                    parts_dt.Columns.Add("USER_TYPE", System.Type.GetType("System.String"));
                                    #endregion

                                    continue;
                                }
                            }

                            if (flag != 0)
                            {
                                dao.deleteAll_TB_D_M_CALENDAR_GROUP(parts_dt);
                            }
                            parts_dt.Clear();
                        }

                        Commit();

                        //新增小車合意價格檔
                        //使用SqlBulkCopy
                        dao.WriteToDatabase(tableName, myTable);
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
            calendar_cd_dt.Clear();
            parts_dt.Clear();
            calendar_cd_dt.Clear();
        }

    }



    public string createExcel(HttpServerUtility Server, WFB2DA0400DAO dao, string toPath)
    {
        FileStream fs = null;
        IWorkbook workbook = null;
        //取得範本sheet
        ISheet sheet = null;
        IRow row;
        ICell cell;
        DataTable dt = new DataTable();

        try
        {
            string CALENDAR_DT = "";
            //無群組代碼的行事曆日期
            string excelPath = Server.MapPath("~/ExcelTemplate/WFB2DA040_Upload2.xlsx");
            fs = new FileStream(excelPath, FileMode.Open, FileAccess.ReadWrite);
            workbook = new XSSFWorkbook(fs); //xlsx的方法
            //取得範本sheet
            sheet = workbook.GetSheetAt(0);
            if (sheet != null)
            {
                int r = 0;

                dt = dao.getTB_D_M_CALENDAR_D();
                if (dt.Rows.Count > 0)
                {
                    ICellStyle stringLeftStyle = this.setCellStyle(workbook, "left", true);
                    ICellStyle stringRightStyle = this.setCellStyle(workbook, "right", true);
                    foreach (DataRow dr in dt.Rows)
                    {
                        r++;
                        row = sheet.CreateRow(r);

                        cell = row.CreateCell(0);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dr["CALENDAR_CD"].ToString()); //行事曆代碼
                        cell = row.CreateCell(1);
                        cell.CellStyle = stringLeftStyle;
                        CALENDAR_DT = (string.IsNullOrWhiteSpace(dr["CALENDAR_DT"].ToString())) ? "" : Convert.ToDateTime(dr["CALENDAR_DT"]).ToString("yyyy/MM/dd");
                        cell.SetCellValue(dr["CALENDAR_DT"].ToString()); //日曆日期 	
                        cell = row.CreateCell(2);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dr["WORK_DAY_CD"].ToString()); //出勤別
                        cell = row.CreateCell(3);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dr["DT_TYPE"].ToString()); //日期類型
                    }

                    FileStream file = new FileStream(toPath, FileMode.Create);//產生檔案
                    workbook.Write(file);
                    file.Close();

                }
                else
                {
                    return "無匯出資料";
                }
            }

            return "0";
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
        finally
        {
            dt.Clear();
            fs.Close();
            workbook.Clear();
            sheet = null;

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




}


