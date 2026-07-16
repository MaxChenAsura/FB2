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
/// CFB2DB0500BO 的摘要描述
/// </summary>
public class WFB2DB0500BO : BaseService
{
	public WFB2DB0500BO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    public string deleteData(List<Tuple<string, string, string, string>> deleteList)
    {
        try
        {
            WFB2DB0500DAO dao = new WFB2DB0500DAO();
            BeginTransaction();

            foreach (var deleteitem in deleteList)
            {
                //刪除班表調整設定檔資料
                dao.deleteData(deleteitem.Item1, deleteitem.Item2, deleteitem.Item3, deleteitem.Item4);
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

    public string deleteAllData(string ws_cd, string work_cd, string work_day_cd, string shift_cd)
    {
        try
        {
            WFB2DB0500DAO dao = new WFB2DB0500DAO();
            BeginTransaction();
            //刪除行事曆群組設定檔資料
            dao.deleteAllData(ws_cd, work_cd, work_day_cd, shift_cd);
            Commit();
            return "0";
        }
        catch (Exception)
        {
            RollBack();
            throw;
        }
    }

    public IWorkbook uploadExcel1(Stream fs, string type, WFB2DB0500DAO dao)
    {
        //取得登入者
        string userid = SessionHandle.Current.emp_id;
        //要載入的資料表名稱
        string tableName = "TB_D_M_SHIFT_ADJ";

        bool valid = true;
        DataTable myTable = new DataTable("myTable");
        DataTable excel_dt = new DataTable();
        string[] excel_pk;
        string[] SHIFT_CD_arr; 
        DataTable SHIFT_CD_dt = new DataTable();
        DataTable WS_CD_dt = new DataTable();
        DataTable WORK_CD_dt = new DataTable();
        DataTable WORK_DAY_CD_dt = new DataTable();
        DataTable parts_dt = new DataTable();
        int arr_count = 0;
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
                myTable.Columns.Add("WS_CD", System.Type.GetType("System.String"));
                myTable.Columns.Add("WORK_CD", System.Type.GetType("System.String"));
                myTable.Columns.Add("WORK_DAY_CD", System.Type.GetType("System.String"));
                myTable.Columns.Add("SHIFT_CD", System.Type.GetType("System.String"));
                myTable.Columns.Add("CREATED_BY", System.Type.GetType("System.String"));
                myTable.Columns.Add("CREATED_DT", System.Type.GetType("System.DateTime"));
                myTable.Columns.Add("UPDATED_BY", System.Type.GetType("System.String"));
                myTable.Columns.Add("UPDATED_DT", System.Type.GetType("System.DateTime"));
                myTable.Columns.Add("FUNC_ID", System.Type.GetType("System.String"));

                #endregion

                #region 建立excel PK值

                DataRow excel_row;
                excel_dt.Columns.Add("WS_CD", System.Type.GetType("System.String"));
                excel_dt.Columns.Add("WORK_CD", System.Type.GetType("System.String"));
                excel_dt.Columns.Add("WORK_DAY_CD", System.Type.GetType("System.String"));
                excel_dt.Columns.Add("SHIFT_CD", System.Type.GetType("System.String"));
                #endregion

                if (sheet.LastRowNum != 0)
                {
                    #region 取得職種清單
                    WS_CD_dt = utilities.getCommCodeVal("HB", "WS_CD", "");
                    WS_CD_dt.PrimaryKey = new DataColumn[] { WS_CD_dt.Columns["sub_cd"] };

                    #endregion
                    #region 取得工數區分
                    WORK_CD_dt = utilities.getCommCodeVal("HB", "WORK_CD", "");
                    WORK_CD_dt.PrimaryKey = new DataColumn[] { WORK_CD_dt.Columns["sub_cd"] };

                    #endregion
                    #region 取得出勤別
                    WORK_DAY_CD_dt = utilities.getCommCodeVal("DA", "WORK_DAY_CD", "");
                    WORK_DAY_CD_dt.PrimaryKey = new DataColumn[] { WORK_DAY_CD_dt.Columns["sub_cd"] };

                    #endregion

                    #region 取得班別主檔的PK值 
                    SHIFT_CD_dt = dao.getAll_SHIFT_CD();
                    SHIFT_CD_dt.PrimaryKey = new DataColumn[] { SHIFT_CD_dt.Columns["SHIFT_CD"] };

                    #endregion
                }

                //巡覽每row的資料第一列為title跳過
                for (int i = 1; i <= sheet.LastRowNum; i++)
                {
                    if (sheet.GetRow(i) != null)
                    {
                        error = "";
                        excel_pk = new string[4];

                        #region 讀取cell資料，第一欄為檢核結果欄位跳過
                        dao.WS_CD = sheet.GetRow(i).GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().ToUpper();
                        dao.WORK_CD = sheet.GetRow(i).GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().ToUpper();
                        dao.WORK_DAY_CD = sheet.GetRow(i).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().ToUpper();
                        dao.SHIFT_CD = sheet.GetRow(i).GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().ToUpper();
                        
                        #endregion

                        SHIFT_CD_arr = dao.SHIFT_CD.Split(',');
                        for (int j = 0; j < SHIFT_CD_arr.Length; j++)
                        {
                            excel_pk[0] = dao.WS_CD;
                            excel_pk[1] = dao.WORK_CD;
                            excel_pk[2] = dao.WORK_DAY_CD;
                            excel_pk[3] = SHIFT_CD_arr[j];


                            #region 檢核基本邏輯
                            //長度檢核
                            error += utilities.checkLength(dao.WS_CD, "職種", 1, false);
                            error += utilities.checkLength(dao.WORK_CD, "工數區分", 1, false);
                            error += utilities.checkLength(dao.WORK_DAY_CD, "出勤別", 1, false);
                            error += utilities.checkLength(SHIFT_CD_arr[j], "班別", 2, false);

                            //格式檢核
                            //職種
                            DataRow dr;
                            if (dao.WS_CD != "")
                            {
                                //存在否 共用代碼檔 
                                dr = WS_CD_dt.Rows.Find(dao.WS_CD);
                                if (dr == null)
                                {
                                    error += "職種不存在共用代碼檔\n";
                                }
                            }

                            if (dao.WORK_CD != "")
                            {
                                //存在否 共用代碼檔 
                                dr = WORK_CD_dt.Rows.Find(dao.WORK_CD);
                                if (dr == null)
                                {
                                    error += "工數區分不存在共用代碼檔\n";
                                }
                            }

                            if (dao.WORK_DAY_CD != "")
                            {
                                //存在否 共用代碼檔 
                                dr = WORK_DAY_CD_dt.Rows.Find(dao.WORK_DAY_CD);
                                if (dr == null)
                                {
                                    error += "出勤別不存在共用代碼檔\n";
                                }
                            }

                            if (SHIFT_CD_arr[j] != "")
                            {
                                //存在否 班別主檔 
                                dr = SHIFT_CD_dt.Rows.Find(SHIFT_CD_arr[j]);
                                if (dr == null)
                                {
                                    error += "班別不存在班別主檔\n";
                                }
                            }

                            //excel的PK值
                            if (excel_dt.Rows.Count > 0)
                            {
                                dr = excel_dt.Rows.Find(excel_pk);
                                if (dr != null)
                                {
                                    error += "此EXCEL有相同的 職種+工數區分+出勤別+班別\n";
                                }
                                else
                                {
                                    #region 建立excel PK值資料

                                    excel_row = excel_dt.NewRow();
                                    excel_row["WS_CD"] = dao.WS_CD;
                                    excel_row["WORK_CD"] = dao.WORK_CD;
                                    excel_row["WORK_DAY_CD"] = dao.WORK_DAY_CD;
                                    excel_row["SHIFT_CD"] = SHIFT_CD_arr[j];
                                    excel_dt.Rows.Add(excel_row);

                                    excel_dt.PrimaryKey =
                                    new DataColumn[] { 
                                    excel_dt.Columns["WS_CD"], 
                                    excel_dt.Columns["WORK_CD"],
                                    excel_dt.Columns["WORK_DAY_CD"],
                                    excel_dt.Columns["SHIFT_CD"]
                                };

                                    #endregion
                                }
                            }
                            else
                            {
                                #region 建立excel PK值資料

                                excel_row = excel_dt.NewRow();
                                excel_row["WS_CD"] = dao.WS_CD;
                                excel_row["WORK_CD"] = dao.WORK_CD;
                                excel_row["WORK_DAY_CD"] = dao.WORK_DAY_CD;
                                excel_row["SHIFT_CD"] = SHIFT_CD_arr[j];
                                excel_dt.Rows.Add(excel_row);

                                excel_dt.PrimaryKey =
                                new DataColumn[] { 
                                    excel_dt.Columns["WS_CD"], 
                                    excel_dt.Columns["WORK_CD"],
                                    excel_dt.Columns["WORK_DAY_CD"],
                                    excel_dt.Columns["SHIFT_CD"]
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
                                myRow["WS_CD"] = dao.WS_CD;
                                myRow["WORK_CD"] = dao.WORK_CD;
                                myRow["WORK_DAY_CD"] = dao.WORK_DAY_CD;
                                myRow["SHIFT_CD"] = SHIFT_CD_arr[j];
                                myRow["CREATED_BY"] = userid;
                                myRow["CREATED_DT"] = DateTime.Now;
                                myRow["UPDATED_BY"] = userid;
                                myRow["UPDATED_DT"] = DateTime.Now;
                                myRow["FUNC_ID"] = "FB2DB050";
                                myTable.Rows.Add(myRow);

                                #endregion
                            }
                        } //j for end

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
                            dao.deleteAll_TB_D_M_SHIFT_ADJ(excel_dt);
                        }
                        else
                        {
                            int flag = 0;
                            #region 建立暫存excel PK值

                            DataRow parts_row;
                            parts_dt.Columns.Add("WS_CD", System.Type.GetType("System.String"));
                            parts_dt.Columns.Add("WORK_CD", System.Type.GetType("System.String"));
                            parts_dt.Columns.Add("WORK_DAY_CD", System.Type.GetType("System.String"));
                            #endregion

                            for (int i = 0; i < excel_dt.Rows.Count; i++)
                            {
                                flag++;

                                #region 建立暫存excel PK值資料

                                parts_row = parts_dt.NewRow();
                                parts_row["WS_CD"] = excel_dt.Rows[i]["WS_CD"];
                                parts_row["WORK_CD"] = excel_dt.Rows[i]["WORK_CD"];
                                parts_row["WORK_DAY_CD"] = excel_dt.Rows[i]["WORK_DAY_CD"];
                                parts_dt.Rows.Add(parts_row);
                                #endregion

                                if (flag == 700)
                                {
                                    dao.deleteAll_TB_D_M_SHIFT_ADJ(parts_dt);

                                    flag = 0;

                                    #region 建立暫存excel PK值
                                    parts_dt = new DataTable();
                                    parts_dt.Columns.Add("WS_CD", System.Type.GetType("System.String"));
                                    parts_dt.Columns.Add("WORK_CD", System.Type.GetType("System.String"));
                                    parts_dt.Columns.Add("WORK_DAY_CD", System.Type.GetType("System.String"));
                                    #endregion

                                    continue;
                                }
                            }

                            if (flag != 0)
                            {
                                dao.deleteAll_TB_D_M_SHIFT_ADJ(parts_dt);
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
                SHIFT_CD_dt.Clear();
                WS_CD_dt.Clear();
                WORK_CD_dt.Clear();
                WORK_DAY_CD_dt.Clear();
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
            SHIFT_CD_dt.Clear();
            WS_CD_dt.Clear();
            WORK_CD_dt.Clear();
            WORK_DAY_CD_dt.Clear();
            parts_dt.Clear();
        }
    }
}