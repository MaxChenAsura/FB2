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
using System.Data.SqlClient;

/// <summary>
/// CFB2DB0400BO 的摘要描述
/// </summary>
public class WFB2DB0400BO : BaseService
{
    public WFB2DB0400BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public string deleteData(List<Tuple<string, string, string, string>> deleteList)
    {
        try
        {
            WFB2DB0400DAO dao = new WFB2DB0400DAO();
            BeginTransaction();

            foreach (var deleteitem in deleteList)
            {
                //刪除勤務班表日期類型異動檔資料
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

    public string SP_DB040_01(WFB2DB0400DAO dao)
    {
        try
        {
            string result = "0";
            //call sp
            int err = dao.SP_DB040_01(dao);

            //確認SP有無成功
            DataTable dtSPresult = dao.checkSP("SP_DB040_01");
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

    public string SP_DB040_02(WFB2DB0400DAO dao)
    {
        try
        {
            string result = "0";
            //call sp
            int err = dao.SP_DB040_02(dao);

            //確認SP有無成功
            DataTable dtSPresult = dao.checkSP("SP_DB040_02");
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

    public IWorkbook uploadExcel1(Stream fs, string type, WFB2DB0400DAO dao)
    {
        //取得登入者
        string userid = SessionHandle.Current.emp_id;
        //要載入的資料表名稱
        string tableName = "TB_D_M_EMP_DUTY_DT_CHG";

        //'(系統日)YYYYMMDD+流水號4碼(同一次上傳,只要一個表單編號即可) 寫個共用SP, SP_GET_FLOWNO(p_FLOWNO output)
        string chg_no = dao.SP_D_GET_FLOWNO();
        string duty_close_dt = dao.getDUTY_CLOSE_DT();

        bool valid = true;
        DataTable myTable = new DataTable("myTable");
        DataTable excel_dt = new DataTable();
        string[] excel_pk;
        DataTable excel_dt2 = new DataTable();
        string[] excel_pk2;
        DataTable EMP_ID_dt = new DataTable();
        DataTable parts_dt = new DataTable();
        try
        {
            string error = "";
            DateTime dt3;
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
                myTable.Columns.Add("CHG_NO", System.Type.GetType("System.String"));
                myTable.Columns.Add("EMP_ID", System.Type.GetType("System.String"));
                myTable.Columns.Add("CALENDAR_DT", System.Type.GetType("System.DateTime"));
                myTable.Columns.Add("DT_TYPE_O", System.Type.GetType("System.String"));
                myTable.Columns.Add("DT_TYPE_N", System.Type.GetType("System.String"));
                myTable.Columns.Add("PROC_STATUS", System.Type.GetType("System.String"));
                myTable.Columns.Add("PROC_DT", System.Type.GetType("System.DateTime"));
                myTable.Columns.Add("IFLOW_NO", System.Type.GetType("System.String"));
                myTable.Columns.Add("CREATED_BY", System.Type.GetType("System.String"));
                myTable.Columns.Add("CREATED_DT", System.Type.GetType("System.DateTime"));
                myTable.Columns.Add("UPDATED_BY", System.Type.GetType("System.String"));
                myTable.Columns.Add("UPDATED_DT", System.Type.GetType("System.DateTime"));
                myTable.Columns.Add("FUNC_ID", System.Type.GetType("System.String"));

                #endregion

                #region 建立excel PK值

                DataRow excel_row;
                excel_dt.Columns.Add("EMP_ID", System.Type.GetType("System.String"));
                excel_dt.Columns.Add("FLOWNO", System.Type.GetType("System.String"));
                excel_dt.Columns.Add("CALENDAR_DT", System.Type.GetType("System.String"));
                excel_dt.Columns.Add("DT_TYPE_O", System.Type.GetType("System.String"));

                #endregion

                if (sheet.LastRowNum != 0)
                {
                    #region 取得班別主檔的PK值
                    EMP_ID_dt = dao.getAll_EMP_ID();
                    EMP_ID_dt.PrimaryKey = new DataColumn[] { EMP_ID_dt.Columns["EMP_ID"] };
                    #endregion
                }

                #region 保留的資料
                string CALENDAR_DT_O_1 = "";        //第1筆原日期
                string DT_TYPE_O_1 = "";            //第1筆原日期類型
                string DT_CH_1 = "";                //第1筆對調日期
                string DT_TYPE_CH_1 = "";           //第1筆對調日期類型

                string CALENDAR_DT_O_9 = "";        //最後1筆原日期
                string DT_TYPE_O_9 = "";            //最後1筆原日期類型
                string DT_CH_9 = "";                //最後1筆對調日期
                string DT_TYPE_CH_9 = "";           //最後1筆對調日期類型

                string CALENDAR_DT_O_pre = "";      //前1筆原日期
                string DT_TYPE_O_pre = "";          //前1筆原日期類型
                string DT_CH_pre = "";              //前1筆對調日期
                string DT_TYPE_CH_pre = "";         //前1筆對調日期類型

                #endregion
               

                       

                //巡覽每row的資料第一列為title跳過
                for (int i = 1; i <= sheet.LastRowNum; i++)
                {
                    if (sheet.GetRow(i) != null)
                    {
                        error = "";
                        excel_pk = new string[3];
                        excel_pk2 = new string[2];
                        #region 讀取cell資料，第一欄為檢核結果欄位跳過
                        dao.EMP_ID = sheet.GetRow(i).GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().ToUpper();
                        dao.CALENDAR_DT = sheet.GetRow(i).GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().ToUpper();
                        dao.DT_TYPE_O = sheet.GetRow(i).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().ToUpper();
                        dao.DT_CH = sheet.GetRow(i).GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().ToUpper();
                        dao.DT_TYPE_CH = sheet.GetRow(i).GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().ToUpper();
                        dao.FLOWNO = sheet.GetRow(i).GetCell(6, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim().ToUpper();
                        dao.CHG_NO = chg_no;

                        //防呆用
                        if (DateTime.TryParse(dao.CALENDAR_DT, out dt3) == true)
                        {
                            dao.CALENDAR_DT = Convert.ToDateTime(dao.CALENDAR_DT).ToString("yyyy/MM/dd");
                            /*
                            if (Convert.ToDateTime(dao.CALENDAR_DT) <= Convert.ToDateTime(duty_close_dt))
                            {
                                error += "原日期需大於已薪資月結前1月月底\n"; 
                            }
                             * */
                        
                        }
                        if (DateTime.TryParse(dao.DT_CH, out dt3) == true)
                        {
                            dao.DT_CH = Convert.ToDateTime(dao.DT_CH).ToString("yyyy/MM/dd");
                            /*
                            if (Convert.ToDateTime(dao.DT_CH) <= Convert.ToDateTime(duty_close_dt))
                            {
                                error += "對調日期需大於已薪資月結前1月月底\n";
                            }
                            */
                        }

                        //該員工,第1筆時,清空
                        if (dao.FLOWNO == "1")
                        {
                            CALENDAR_DT_O_1 = dao.CALENDAR_DT;          //第1筆原日期
                            DT_TYPE_O_1 = dao.DT_TYPE_O;                //第1筆原日期類型
                            DT_CH_1 = dao.DT_CH;                        //第1筆對調日期
                            DT_TYPE_CH_1 = dao.DT_TYPE_CH;              //第1筆對調日期類型

                            CALENDAR_DT_O_pre = dao.CALENDAR_DT;        //前1筆原日期
                            DT_TYPE_O_pre = dao.DT_TYPE_O;              //前1筆原日期類型
                            DT_CH_pre = dao.DT_CH;                      //前1筆對調日期
                            DT_TYPE_CH_pre = dao.DT_TYPE_CH;            //前1筆對調日期類型

                            CALENDAR_DT_O_9 = "";                       //最後1筆原日期
                            DT_TYPE_O_9 = "";                           //最後1筆原日期類型
                            DT_CH_9 = "";                               //最後1筆對調日期
                            DT_TYPE_CH_9 = "";                          //最後1筆對調日期類型
                        }

                        //該員工,最後1筆時
                        if (dao.FLOWNO == "9")
                        {
                            CALENDAR_DT_O_9 = dao.CALENDAR_DT;          //最後1筆原日期
                            DT_TYPE_O_9 = dao.DT_TYPE_O;                //最後1筆原日期類型
                            DT_CH_9 = dao.DT_CH;                        //最後1筆對調日期
                            DT_TYPE_CH_9 = dao.DT_TYPE_CH;              //最後1筆對調日期類型
                        }

                        #endregion
                        excel_pk[0] = dao.EMP_ID;
                        excel_pk[1] = dao.FLOWNO;
                        excel_pk[2] = dao.CALENDAR_DT;

                        #region 檢核基本邏輯
                        //長度檢核
                        error += utilities.checkLength(dao.EMP_ID, "工號", 5, false);
                        error += utilities.checkLength(dao.DT_TYPE_O, "原日期類型", 1, false);
                        error += utilities.checkLength(dao.DT_TYPE_CH, "對調日期類型", 1, false);

                        error += utilities.checkDateFormat(dao.CALENDAR_DT, "原日期", false);
                        error += utilities.checkDateFormat(dao.DT_CH, "對調日期", false);

                        //格式檢核
                        //職種
                        DataRow dr;

                        //(1)excel的PK值
                        if (excel_dt.Rows.Count == 0 || excel_dt.Rows.Find(excel_pk) ==null)
                        {
                            #region 建立excel PK值資料
                            excel_row = excel_dt.NewRow();
                            excel_row["EMP_ID"] = dao.EMP_ID;
                            excel_row["CALENDAR_DT"] = dao.CALENDAR_DT;
                            excel_row["DT_TYPE_O"] = dao.DT_TYPE_O;
                            excel_dt.Rows.Add(excel_row);
                            excel_dt.PrimaryKey =
                            new DataColumn[] { 
                                    excel_dt.Columns["EMP_ID"], 
                                    excel_dt.Columns["CALENDAR_DT"],
                                    excel_dt.Columns["DT_TYPE_O"]
                                };
                            #endregion
                        }
                        else
                        {
                            error += "工號+原日期+原日期類型+有重覆\n";
                        }

                        /*
                        //(2)
                        if (excel_dt2.Rows.Count == 0 || excel_dt2.Rows.Find(excel_pk2)==null)
                        {
                            #region 建立excel PK值資料

                            excel_row2 = excel_dt2.NewRow();
                            excel_row2["EMP_ID"] = dao.EMP_ID;
                            excel_row2["CALENDAR_DT"] = dao.CALENDAR_DT;
                            excel_dt2.Rows.Add(excel_row2);

                            excel_dt2.PrimaryKey =
                            new DataColumn[] { 
                                    excel_dt2.Columns["EMP_ID"], 
                                    excel_dt2.Columns["CALENDAR_DT"]
                                };
                            #endregion
                        }
                        else
                        {
                            error += "工號 + 原日期有重覆\n";
                        }
                        */


                        //(3)
                        if (dao.EMP_ID != "")
                        {
                            //若TB_D_M_EMP_DUTY_DT_CHG(勤務班表日期類型異動檔) 有處理狀態為N-未執行時,不可新增 
                            dr = EMP_ID_dt.Rows.Find(dao.EMP_ID);
                            if (dr != null)
                            {
                                error += "勤務日期類型異動檔有未執行資料\n";
                            }
                        }

                        //(4)第1筆(流水號=1)的原日期資料要 與 最後一筆(流水號=9) 的 對調資料相同
                        if (dao.FLOWNO == "9") {
                            if (CALENDAR_DT_O_1 != DT_CH_9 || DT_TYPE_O_1 != DT_TYPE_CH_9)
                                error += "第1筆的原日期資料要 與 最後一筆 的 對調資料不相同\n";
                        }

                        //(5)每1筆的原資料要與上1筆的對調資料相同(第1筆除外:流水號=1時)
                        if (dao.FLOWNO != "1")
                        {
                            if (dao.CALENDAR_DT != DT_CH_pre || dao.DT_TYPE_O != DT_TYPE_CH_pre)
                                error += "原日期資料與前一筆 的 對調資料不相同\n";
                        }

                        //(6)原日期類型不可等於對調日期類型
                        if (dao.DT_TYPE_O == dao.DT_TYPE_CH)
                        {
                            error += "原日期類型與對調日期類型不可相同\n";
                        }

                        //3.資料檢查
                        //(1)該工號的(原,對調)日期及日期類型要跟  TB_D_M_EMP_DAY_DUTY(日勤務班表資料檔) 的 日期及日期類型相同
                        if (!dao.checkDt(dao.EMP_ID, dao.CALENDAR_DT, dao.DT_TYPE_O))
                        {
                            error += "原日期類型與班表不相同\n";
                        }
                        if (!dao.checkDt(dao.EMP_ID, dao.DT_CH, dao.DT_TYPE_CH))
                        {
                            error += "對調日期類型與班表不相同\n";
                        }

                        #region 4.調整規則檢查(前3個檢查皆OK，才執行第4項)
                        
                        //4.調整規則檢查(前3個檢查皆OK，才執行第4項)
                        if (error == "")
                        {
                            
                            if (dao.DT_TYPE_O == "3" || dao.DT_TYPE_CH == "3")
                            {
                                //(1)若其中有3-例假日,則需同一週
                                if (!dao.checkWEEKLY(dao.CALENDAR_DT, dao.DT_CH))
                                    error += "日期類型有3-例假日，原、對調日期需同一週\n";
                            }                            
                            else if ((dao.DT_TYPE_O == "4" && dao.DT_TYPE_CH == "5") ||
                                (dao.DT_TYPE_O == "5" && dao.DT_TYPE_CH == "4"))
                            {
                                //(2)若為4,5,則同一年即可
                                string yesr_o = dao.CALENDAR_DT.Substring(0, 4);
                                string yesr_ch = dao.DT_CH.Substring(0, 4);
                                if (yesr_o != yesr_ch)
                                    error += "原、對調日期需同一年\n";
                            }
                            else
                            {
                                //(3)其餘為同一群組代碼
                                if (!dao.checkGROUP_CD(dao.EMP_ID,dao.CALENDAR_DT, dao.DT_CH) )
                                    error += "原、對調日期需同一群組代碼\n";
                            }

                        }
                        #endregion

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
                            myRow["CHG_NO"] = chg_no;
                            myRow["EMP_ID"] = dao.EMP_ID;
                            myRow["CALENDAR_DT"] = dao.DT_CH;
                            myRow["DT_TYPE_O"] = dao.DT_TYPE_CH;
                            myRow["DT_TYPE_N"] = dao.DT_TYPE_O;
                            myRow["PROC_STATUS"] = "N";
                            myRow["PROC_DT"] = DBNull.Value;
                            myRow["IFLOW_NO"] = "";
                            myRow["CREATED_BY"] = userid;
                            myRow["CREATED_DT"] = DateTime.Now;
                            myRow["UPDATED_BY"] = userid;
                            myRow["PROC_DT"] = DateTime.Now;
                            myRow["FUNC_ID"] = "FB2DB040";
                            myTable.Rows.Add(myRow);

                            #endregion
                        }

                        //最後保留前一筆的資料
                        CALENDAR_DT_O_pre = dao.CALENDAR_DT;       //前1筆原日期
                        DT_TYPE_O_pre = dao.DT_TYPE_O;          //前1筆原日期類型
                        DT_CH_pre = dao.DT_CH;              //前1筆對調日期
                        DT_TYPE_CH_pre = dao.DT_TYPE_CH;        //前1筆對調日期類型



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
                        //新增勤務班表日期類型異動檔
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
            parts_dt.Clear();
        }
    }
    internal void WriteToDatabase(string tableName, DataTable myTable)
    {
        try
        {
            // get your connection string
            string connString = utilities.connstr;
            // connect to SQL
            using (SqlConnection connection =
                    new SqlConnection(connString))
            {
                // make sure to enable triggers
                // more on triggers in next post
                SqlBulkCopy bulkCopy =
                    new SqlBulkCopy
                    (
                    connection,
                    SqlBulkCopyOptions.TableLock |
                    SqlBulkCopyOptions.FireTriggers |
                    SqlBulkCopyOptions.UseInternalTransaction,
                    null
                    );

                // set the destination table name
                bulkCopy.DestinationTableName = tableName;
                connection.Open();

                // write the data in the "dataTable"
                bulkCopy.WriteToServer(myTable);
                connection.Close();
            }
            // reset
            myTable.Clear();
        }
        catch (Exception)
        {

            throw;
        }
    }

}