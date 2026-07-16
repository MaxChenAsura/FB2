using FB2.tw.co.toyota.kuozui.bo;
using Ionic.Zip;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

public class pagetxt
{
    public String filename { get; set; }
    public MemoryStream ms { get; set; }
}

/// <summary>
/// CFB2IB0600BO 的摘要描述
/// </summary>
public class CFB2IB0600BO : BaseService
{


    public CFB2IB0600BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public string checkINS2_DETAIL(CFB2IB0600DAO dao)
    {
        string errormessage = "";
        try
        {
            DataTable dt = dao.checkINS2_DETAIL();

            if (dt.Rows.Count == 0)
            {
                errormessage += "個人健保補充保費扣繳檔沒有資料可計算\\n";
                return errormessage;
            }

            return errormessage;

        }
        catch (Exception)
        {

            throw;
        }
    }

    public string checkNOT_BONUS(CFB2IB0600DAO dao)
    {
        string errormessage = "";
        try
        {
            DataTable dt = dao.checkNOT_BONUS();

            if (dt.Rows.Count == 0)
            {
                errormessage += "非獎金類補充保費申報檔沒有資料可計算\\n";
                return errormessage;
            }

            return errormessage;

        }
        catch (Exception)
        {

            throw;
        }
    }


    public string uploadExcel(Stream fs, string type)
    {
        CFB2IB0600DAO dao = new CFB2IB0600DAO();
        try
        {
            //取得登入者
            string userid = SessionHandle.Current.emp_id;

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
            ICellStyle style1 = workbook.CreateCellStyle();
            IFont font1 = workbook.CreateFont();

            font1.Color = HSSFColor.Red.Index;


            if (sheet != null)
            {
                try
                {
                    BeginTransaction();

                    //取得所有EXCEL中的KEY
                    List<string> keys = new List<string>();
                    for (int i = 1; i <= sheet.LastRowNum; i++)
                    {
                        if (sheet.GetRow(i) != null)
                        {
                            string c2 = sheet.GetRow(i).GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            string c3 = sheet.GetRow(i).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            string c6 = sheet.GetRow(i).GetCell(6, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            keys.Add(c2 + "," + c3 + "," + c6);
                        }
                    }

                    //巡覽每row的資料第一列為title跳過
                    for (int i = 1; i <= sheet.LastRowNum; i++)
                    {
                        bool b = true;
                        if (sheet.GetRow(i) != null)
                        {
                            //讀取cell資料，第一欄為檢核結果欄位跳過
                            string cell1 = sheet.GetRow(i).GetCell(1, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            string cell2 = sheet.GetRow(i).GetCell(2, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            string cell3 = sheet.GetRow(i).GetCell(3, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            string cell4 = sheet.GetRow(i).GetCell(4, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            string cell5 = sheet.GetRow(i).GetCell(5, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();
                            string cell6 = sheet.GetRow(i).GetCell(6, MissingCellPolicy.CREATE_NULL_AS_BLANK).ToString().Trim();

                            string error = "";

                            //開始檢查   
                            //所得人姓名
                            if (cell1 == "")
                                error += "所得人姓名不可為空白\n";
                            else
                            {
                                if (cell1.Length > 20)
                                {
                                    error += "所得人姓名長度錯誤\n";
                                }
                            }

                            //支付對象身分證ID
                            if (cell2 == "")
                                error += "身分證不可為空白\n";
                            else
                            {
                                if (cell2.Length > 20)
                                {
                                    error += "身分證長度錯誤\n";
                                }
                                else
                                {
                                    if (!IdCheck(cell2))
                                    {
                                        error += "身份證錯誤\n";
                                    }
                                }

                            }

                            //所得類別 (上傳目前只有63  68類)
                            if (cell3 == "")
                                error += "所得類別不可為空白\n";
                            else
                                if (cell3 != "")
                                {
                                    if (cell3.Length != 2)
                                    {
                                        error += "所得類別長度錯誤\n";
                                    }
                                    else
                                    {
                                        if (!IsNumeric(cell3))
                                        {
                                            error += "傳票號碼只能輸入數字\n";
                                        }
                                    }
                                }

                            //台幣金額
                            if (cell4 == "")
                                error += "台幣金額不可為空白\n";
                            else
                            {
                                if (cell4.Length > 10)
                                {
                                    error += "台幣金額長度錯誤\n";
                                }
                                else
                                {
                                    if (!IsNumeric(cell4))
                                    {
                                        error += "台幣金額只能輸入數字\n";
                                    }
                                }
                            }

                            //補充保費代扣台幣額
                            if (cell5 == "")
                                error += "補充保費代扣台幣額不可為空白\n";
                            else
                                if (cell5 != "")
                                {
                                    if (cell5.Length > 7)
                                    {
                                        error += "補充保費代扣台幣額長度錯誤\n";
                                    }
                                    else
                                    {
                                        if (!IsNumeric(cell5))
                                        {
                                            error += "補充保費代扣台幣額只能輸入數字\n";
                                        }
                                    }
                                }

                            //發生日期
                            if (cell6 == "")
                                error += "發生日期不可為空白\n";
                            else
                                if (cell6 != "")
                                {
                                    if (cell6.Length > 7)
                                    {
                                        error += "發生日期長度錯誤\n";
                                    }
                                    else
                                    {
                                        if (!IsNumeric(cell6))
                                        {
                                            error += "發生日期只能輸入數字\n";
                                        }
                                    }
                                }

                            //傳出錯誤訊息
                            style1.SetFont(font1);
                            sheet.GetRow(i).CreateCell(0).CellStyle = style1;
                            sheet.GetRow(i).GetCell(0, MissingCellPolicy.CREATE_NULL_AS_BLANK).SetCellValue(error);
                            if (error != "")
                            {
                                valid = false;
                            }
                            else
                            {
                                dao.EMP_NAME = cell1;
                                dao.LICENSE_ID = cell2;
                                dao.CODE_CD = cell3;
                                dao.NT_AMOUNT = cell4;
                                dao.INS_COST = cell5;
                                dao.PAYMENT_DATE = cell6;

                                dao.CREATED_BY = SessionHandle.Current.emp_id.Trim();
                                dao.UPDATED_BY = SessionHandle.Current.emp_id.Trim();
                                dao.FUNC_ID = "FB2IB060";

                                //如檢核無誤，先刪除相同 身分證 .所得類別.發生日期的資料
                                dao.deleteINS2_NOT_BONUS();

                                dao.insertINS2_NOT_BONUS();
                            }

                        }

                    } if (sheet.LastRowNum == 0)
                    {
                        string error = "請輸入上傳資料\n";
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

    public void createTxt(CFB2IB0600DAO dao)
    {
        int seq1 = 0, sum_ONE_TIME_AMOUNT = 0, sum_INS_COST = 0, rowCount = 0, pg = 0;
        int t = 16000;//幾筆要分頁
        string b1 = "", b2 = "", b3 = "", b4 = "", b5 = "", b6 = "", b7 = "", b8 = "", b9 = "", b10 = "", b11 = "";
        string totalCount = "", now = "";
        double s = 0;


        String fileName = "";
        int h = 0, g = 0;

        totalCount = dao.get62DataCount();
        if (totalCount != "0")
        {
            s = Convert.ToDouble(totalCount);
        }
        int page = Convert.ToInt16(Math.Ceiling(s / t));
        //int page = Convert.ToInt16(Math.Ceiling(s / 3));//假設一頁3筆  找到共幾頁
        List<pagetxt> list = new List<pagetxt>();

        MemoryStream ms = null;
        TextWriter tw = null;
        pagetxt txt = null;

        try
        {
            dao.getCompany();//公司資料            
            dao.getUserEmail();//擔當Email
            dao.getUserName();//擔當姓名
            dao.getUserPHONE();//擔當電話分機
            dao.getYM();//txt 的最大 最小年月
            dao.nowDate = utilities.DateToTw(DateTime.Now.ToString("yyyy/MM/dd"), "");
            seq1 = 620000001;//62流水序號
            now = DateTime.Now.ToString("yyyyMMdd");//西元年
            now = Convert.ToString(Convert.ToInt32(now.Substring(0, 4)) - 1911) + now.Substring(4, 4);//民國年

            //補空白
            b1 = "        "; //總機構統一編號 補空白
            for (int j = 0; j < (30 - dao.USER_EMAIL.Length); j++)
            {
                b2 += " "; //電子郵件  補空白
            }
            for (int j = 0; j < (25 - dao.CHAIRMAN_NAME.Length); j++)
            {
                b3 += "　"; //負責人  補全形空白
            }
            for (int j = 0; j < (30 - seq1.ToString().Length); j++)
            {
                b4 += " "; //流水序號  補空白
            }

            b5 = "               ";//保留欄位

            for (int j = 0; j < (15 - dao.USER_PHONE.Length); j++)
            {
                b7 += " "; //連絡電話分機  補空白
            }
            for (int j = 0; j < (50 - 2 * (dao.USER_NAME.Length)); j++)
            {
                b8 += " "; //擔當者  補空白
            }

            for (int j = 0; j < 79; j++)
            {
                b9 += " ";
            }
            for (int j = 0; j < 84; j++)
            {
                b10 += " ";
            }

            for (int k = 0; k < page; k++)//62類別共有幾頁
            {
                ms = new MemoryStream();
                tw = new StreamWriter(ms, System.Text.Encoding.GetEncoding("big5"));
                //tw = new StreamWriter(ms);

                //若每頁需集計補充保費 獎金金額 則需要底下這兩行，否則就mark掉
                sum_ONE_TIME_AMOUNT = 0;
                sum_INS_COST = 0;
                rowCount = 0;
                //End

                //檔名 依照健保局規定: DPR + 申報單位統一編號 (8碼)+處理/申報日期 (yyymmdd) + 序號(3碼)
                //fileName = dao.C_YEAR+"_62_" + Convert.ToString(k + 1) + ".txt";
                pg = pg + 1;//紀錄目前編號到第幾個
                fileName = "DPR" + dao.COMPANY_ID + now + Convert.ToString(pg).PadLeft(3, '0') + ".txt";
                txt = new pagetxt();
                txt.filename = fileName;

                //用k來決定筆數起迄
                g = k * t;
                //g = k * 3;
                h = (k + 1) * t;//如K=0,表示16000筆時要分頁
                //h = (k + 1) * 3;
                //62-1
                tw.WriteLine(
                                "1" +
                                dao.COMPANY_ID +
                                "62" +
                                dao.MINYM +
                                dao.MAXYM +
                                dao.nowDate + b1 +
                                dao.USER_EMAIL + b2 +
                                utilities.toWide(string.Format("{0,-25}", utilities.convertBig5(dao.CHAIRMAN_NAME).Replace("?", "？")))
                    //dao.CHAIRMAN_NAME + b3
                                + b10
                            );
                //tw.WriteLine();//空一行

                //62-2
                DataTable dt1 = dao.get62Data();
                if (dt1.Rows.Count > 0)
                {
                    for (int i = 0; i < dt1.Rows.Count; i++)
                    {
                        b6 = "";
                        b11 = "";
                        for (int j = 0; j < (25 - dt1.Rows[i]["EMP_NAME"].ToString().Length); j++)
                        {
                            b6 += "　"; //所得人姓名  補全形空白
                        }
                        for (int j = 0; j < (10 - dt1.Rows[i]["LICENSE_ID"].ToString().Length); j++)
                        {
                            b11 += " ";//
                        }
                        if (dt1.Rows[i]["EMP_NAME"].ToString() == "許文龍")
                        {
                            string tt = "";
                            tt = dt1.Rows[i]["ONE_TIME_AMOUNT"].ToString();
                        }
                        if (i >= g & i < h)
                        {
                            tw.WriteLine(
                                "2" +
                                dao.COMPANY_ID +
                                "62" +
                                seq1 +
                                "I" +
                                (utilities.DateToTw(dt1.Rows[i]["PAYMENT_DATE"].ToString().Replace("-", "/"))).Replace("/", "") +
                                dt1.Rows[i]["LICENSE_ID"].ToString() +
                                b11 + seq1 + b4 +
                                dt1.Rows[i]["ONE_TIME_AMOUNT"].ToString() +
                                dt1.Rows[i]["INS_COST"].ToString() +
                                dao.HEALTH_ORG_ID +
                                dt1.Rows[i]["INS_MONTH_AMOUNT"].ToString() +
                                dt1.Rows[i]["ACCU_AMOUNT"].ToString() +
                                b5 +
                                " " +
                                utilities.toWide(string.Format("{0,-25}", utilities.convertBig5(dt1.Rows[i]["EMP_NAME"].ToString()).Replace("?", "？"))) +
                                //dt1.Rows[i]["EMP_NAME"].ToString() + b6 +
                                "                "
                            );

                            //20151015 遇到負數金額的處理
                            if (dt1.Rows[i]["ONE_TIME_AMOUNT"].ToString().IndexOf("-") == -1)
                            {
                                sum_ONE_TIME_AMOUNT = sum_ONE_TIME_AMOUNT + Convert.ToInt32(dt1.Rows[i]["ONE_TIME_AMOUNT"].ToString());//所得(收入)給付總額
                            }
                            else
                            {
                                string st = (dt1.Rows[i]["ONE_TIME_AMOUNT"].ToString()).Substring(dt1.Rows[i]["ONE_TIME_AMOUNT"].ToString().IndexOf("-"));
                                sum_ONE_TIME_AMOUNT = sum_ONE_TIME_AMOUNT + Convert.ToInt32(st);
                            }
                            //sum_ONE_TIME_AMOUNT = sum_ONE_TIME_AMOUNT + Convert.ToInt32(dt1.Rows[i]["ONE_TIME_AMOUNT"].ToString());//所得(收入)給付總額

                            if (dt1.Rows[i]["INS_COST"].ToString().IndexOf("-") == -1)
                            {
                                sum_INS_COST = sum_INS_COST + Convert.ToInt32(dt1.Rows[i]["INS_COST"].ToString());
                            }
                            else
                            {
                                string st = (dt1.Rows[i]["INS_COST"].ToString()).Substring(dt1.Rows[i]["INS_COST"].ToString().IndexOf("-"));
                                sum_ONE_TIME_AMOUNT = sum_ONE_TIME_AMOUNT + Convert.ToInt32(st);
                            }
                            //sum_INS_COST = sum_INS_COST + Convert.ToInt32(dt1.Rows[i]["INS_COST"].ToString());
                            rowCount = rowCount + 1;
                            seq1 = seq1 + 1;

                        }//if end              
                    }
                }//if(dt1.Rows.Count > 0) end

                //62-3 目前是每頁都印出這段，待確認
                tw.WriteLine(
                    "3" +
                    dao.COMPANY_ID +
                    "62" +
                    Convert.ToString(rowCount).PadLeft(9, '0') +
                    Convert.ToString(sum_ONE_TIME_AMOUNT).PadLeft(20, '0') +
                    Convert.ToString(sum_INS_COST).PadLeft(16, '0') +
                    dao.USER_PHONE + b7 +
                    utilities.toWide(string.Format("{0,-25}", utilities.convertBig5(dao.USER_NAME).Replace("?", "？"))) +
                    //dao.USER_NAME + b8 +
                    b9
                );



                tw.Flush();
                txt.ms = ms;
                list.Add(txt);
            }

            //63兼職所得             
            s = 0;
            seq1 = 630000001;//63流水序號
            totalCount = dao.get63DataCount();
            if (totalCount != "0")
            {
                s = Convert.ToDouble(totalCount);
            }
            page = Convert.ToInt16(Math.Ceiling(s / t));
            //page = Convert.ToInt16(Math.Ceiling(s / 3));//假設一頁3筆  找到共幾頁
            dao.get63YM();//63 txt的最大最小年月
            string d1 = "";
            for (int i = 0; i < 40; i++)
            {
                d1 += " ";//共用欄位區 補空白
            }

            for (int k = 0; k < page; k++)//63類別共有幾頁
            {
                ms = new MemoryStream();
                tw = new StreamWriter(ms, System.Text.Encoding.GetEncoding("big5"));
                //tw = new StreamWriter(ms);

                rowCount = 0;
                sum_ONE_TIME_AMOUNT = 0;
                sum_INS_COST = 0;

                //檔名
                //fileName = dao.C_YEAR+"_63_" + Convert.ToString(k + 1) + ".txt";
                pg = pg + 1;//紀錄目前編號到第幾個
                fileName = "DPR" + dao.COMPANY_ID + now + Convert.ToString(pg).PadLeft(3, '0') + ".txt";
                txt = new pagetxt();
                txt.filename = fileName;

                //用k來決定筆數起迄
                g = k * t;
                //g = k * 3;
                h = (k + 1) * t;//如K=0,表示16000筆時要分頁
                //h = (k + 1) * 3;
                //63-1 目前是每頁都印出這段，待確認
                tw.WriteLine(
                            "1" +
                            dao.COMPANY_ID +
                            "63" +
                            dao.MINYM +
                            dao.MAXYM +
                            dao.nowDate + b1 +
                            dao.USER_EMAIL + b2 +
                             utilities.toWide(string.Format("{0,-25}", utilities.convertBig5(dao.CHAIRMAN_NAME).Replace("?", "？")))
                    //dao.CHAIRMAN_NAME + b3
                            + b10
                            );
                //tw.WriteLine();//空一行

                //63-2
                DataTable dt63 = dao.get63Data();
                if (dt63.Rows.Count > 0)
                {
                    for (int i = 0; i < dt63.Rows.Count; i++)
                    {
                        b6 = ""; b11 = "";

                        for (int j = 0; j < (10 - dt63.Rows[i]["LICENSE_ID"].ToString().Length); j++)
                        {
                            b11 += " ";//
                        }
                        for (int j = 0; j < (25 - dt63.Rows[i]["EMP_NAME"].ToString().Length); j++)
                        {
                            b6 += "　"; //所得人姓名  補全形空白
                        }

                        if (i >= g & i < h)
                        {
                            tw.WriteLine(
                                "2" +
                                dao.COMPANY_ID +
                                "63" +
                                seq1 +
                                "I" +
                                dt63.Rows[i]["PAYMENT_DATE"].ToString() +
                                dt63.Rows[i]["LICENSE_ID"].ToString() +
                                b11 + seq1 + b4 +
                                dt63.Rows[i]["NT_AMOUNT"].ToString() +
                                dt63.Rows[i]["INS_COST"].ToString() +
                                d1 +
                                " " +
                                utilities.toWide(string.Format("{0,-25}", utilities.convertBig5(dt63.Rows[i]["EMP_NAME"].ToString()).Replace("?", "？"))) +
                                //dt63.Rows[i]["EMP_NAME"].ToString() + b6 +
                                " " +
                                "                "
                            );
                            sum_ONE_TIME_AMOUNT = sum_ONE_TIME_AMOUNT + Convert.ToInt32(dt63.Rows[i]["NT_AMOUNT"].ToString());
                            sum_INS_COST = sum_INS_COST + Convert.ToInt32(dt63.Rows[i]["INS_COST"].ToString());
                            rowCount = rowCount + 1;
                            seq1 = seq1 + 1;

                        }//if end              
                    }
                }//if(dt1.Rows.Count > 0) end

                //63-3 目前是每頁都印出這段，待確認
                tw.WriteLine(
                    "3" +
                    dao.COMPANY_ID +
                    "63" +
                    Convert.ToString(rowCount).PadLeft(9, '0') +
                    Convert.ToString(sum_ONE_TIME_AMOUNT).PadLeft(20, '0') +
                    Convert.ToString(sum_INS_COST).PadLeft(16, '0') +
                    dao.USER_PHONE + b7 +
                    utilities.toWide(string.Format("{0,-25}", utilities.convertBig5(dao.USER_NAME).Replace("?", "？"))) +
                    //dao.USER_NAME + b8 +
                    b9
                );

                tw.Flush();
                txt.ms = ms;
                list.Add(txt);
            }


            //68租金收入
            s = 0;
            seq1 = 680000001;//68流水序號
            totalCount = dao.get68DataCount();
            if (totalCount != "0")
            {
                s = Convert.ToDouble(totalCount);
            }
            page = Convert.ToInt16(Math.Ceiling(s / t));
            //page = Convert.ToInt16(Math.Ceiling(s / 3));//假設一頁3筆  找到共幾頁
            dao.get68YM();//68 txt的最大最小年月
            d1 = "";
            for (int i = 0; i < 40; i++)
            {
                d1 += " ";//共用欄位區 補空白
            }

            for (int k = 0; k < page; k++)//63類別共有幾頁
            {
                ms = new MemoryStream();
                tw = new StreamWriter(ms, System.Text.Encoding.GetEncoding("big5"));
                //tw = new StreamWriter(ms);

                rowCount = 0;
                sum_ONE_TIME_AMOUNT = 0;
                sum_INS_COST = 0;

                //檔名
                //fileName = dao.C_YEAR + "_68_" + Convert.ToString(k + 1) + ".txt";
                pg = pg + 1;//紀錄目前編號到第幾個
                fileName = "DPR" + dao.COMPANY_ID + now + Convert.ToString(pg).PadLeft(3, '0') + ".txt";
                txt = new pagetxt();
                txt.filename = fileName;

                //用k來決定筆數起迄
                g = k * t;
                //g = k * 3;
                h = (k + 1) * t;//如K=0,表示16000筆時要分頁
                //h = (k + 1) * 3;
                //68-1
                tw.WriteLine(
                            "1" +
                            dao.COMPANY_ID +
                            "68" +
                            dao.MINYM +
                            dao.MAXYM +
                            dao.nowDate + b1 +
                            dao.USER_EMAIL + b2 +
                            utilities.toWide(string.Format("{0,-25}", utilities.convertBig5(dao.CHAIRMAN_NAME).Replace("?", "？")))
                    // dao.CHAIRMAN_NAME + b3
                            + b10
                            );
                //tw.WriteLine();//空一行

                //68-2
                DataTable dt68 = dao.get68Data();
                if (dt68.Rows.Count > 0)
                {
                    for (int i = 0; i < dt68.Rows.Count; i++)
                    {
                        b6 = ""; b11 = "";

                        for (int j = 0; j < (10 - dt68.Rows[i]["LICENSE_ID"].ToString().Length); j++)
                        {
                            b11 += " ";//
                        }
                        for (int j = 0; j < (25 - dt68.Rows[i]["EMP_NAME"].ToString().Length); j++)
                        {
                            b6 += "　"; //所得人姓名  補全形空白
                        }

                        if (i >= g & i < h)
                        {
                            tw.WriteLine(
                                "2" +
                                dao.COMPANY_ID +
                                "68" +
                                seq1 +
                                "I" +
                                dt68.Rows[i]["PAYMENT_DATE"].ToString() +
                                dt68.Rows[i]["LICENSE_ID"].ToString() +
                                b11 + seq1 + b4 +
                                dt68.Rows[i]["NT_AMOUNT"].ToString() +
                                dt68.Rows[i]["INS_COST"].ToString() +
                                d1 +
                                " " +
                                utilities.toWide(string.Format("{0,-25}", utilities.convertBig5(dt68.Rows[i]["EMP_NAME"].ToString()).Replace("?", "？"))) +
                                // dt68.Rows[i]["EMP_NAME"].ToString() + b6 +
                                " " +
                                "                "
                            );
                            sum_ONE_TIME_AMOUNT = sum_ONE_TIME_AMOUNT + Convert.ToInt32(dt68.Rows[i]["NT_AMOUNT"].ToString());
                            sum_INS_COST = sum_INS_COST + Convert.ToInt32(dt68.Rows[i]["INS_COST"].ToString());
                            rowCount = rowCount + 1;
                            seq1 = seq1 + 1;

                        }//if end              
                    }
                }//if(dt1.Rows.Count > 0) end

                //68-3
                tw.WriteLine(
                    "3" +
                    dao.COMPANY_ID +
                    "68" +
                    Convert.ToString(rowCount).PadLeft(9, '0') +
                    Convert.ToString(sum_ONE_TIME_AMOUNT).PadLeft(20, '0') +
                    Convert.ToString(sum_INS_COST).PadLeft(16, '0') +
                    dao.USER_PHONE + b7 +
                    utilities.toWide(string.Format("{0,-25}", utilities.convertBig5(dao.USER_NAME).Replace("?", "？"))) +
                    // dao.USER_NAME + b8 +
                    b9
                );



                tw.Flush();
                txt.ms = ms;
                list.Add(txt);
            }

            //65執行業務收入           
            s = 0;
            seq1 = 650000001;//65流水序號
            totalCount = dao.get65DataCount();
            if (totalCount != "0")
            {
                s = Convert.ToDouble(totalCount);
            }
            page = Convert.ToInt16(Math.Ceiling(s / t));
            //page = Convert.ToInt16(Math.Ceiling(s / 3));//假設一頁3筆  找到共幾頁
            dao.get65YM();//65 txt的最大最小年月
            d1 = "";
            for (int i = 0; i < 40; i++)
            {
                d1 += " ";//共用欄位區 補空白
            }

            for (int k = 0; k < page; k++)//65類別共有幾頁
            {
                ms = new MemoryStream();
                tw = new StreamWriter(ms, System.Text.Encoding.GetEncoding("big5"));
                //tw = new StreamWriter(ms);

                sum_ONE_TIME_AMOUNT = 0;
                sum_INS_COST = 0;
                rowCount = 0;

                //檔名
                //fileName = dao.C_YEAR + "_65_" + Convert.ToString(k + 1) + ".txt";
                pg = pg + 1;//紀錄目前編號到第幾個
                fileName = "DPR" + dao.COMPANY_ID + now + Convert.ToString(pg).PadLeft(3, '0') + ".txt";
                txt = new pagetxt();
                txt.filename = fileName;
                //list.Add(fileName);

                //用k來決定筆數起迄
                g = k * t;
                //g = k * 3;
                h = (k + 1) * t;//如K=0,表示16000筆時要分頁
                //h = (k + 1) * 3;
                //65-1
                tw.WriteLine(
                            "1" +
                            dao.COMPANY_ID +
                            "65" +
                            dao.MINYM +
                            dao.MAXYM +
                            dao.nowDate + b1 +
                            dao.USER_EMAIL + b2 +
                             utilities.toWide(string.Format("{0,-25}", utilities.convertBig5(dao.CHAIRMAN_NAME).Replace("?", "？")))
                    // dao.CHAIRMAN_NAME + b3
                            + b10
                            );
                //tw.WriteLine();//空一行

                //65-2
                DataTable dt65 = dao.get65Data();
                if (dt65.Rows.Count > 0)
                {
                    for (int i = 0; i < dt65.Rows.Count; i++)
                    {
                        b6 = ""; b11 = "";

                        for (int j = 0; j < (10 - dt65.Rows[i]["LICENSE_ID"].ToString().Length); j++)
                        {
                            b11 += " ";//
                        }
                        for (int j = 0; j < (25 - dt65.Rows[i]["EMP_NAME"].ToString().Length); j++)
                        {
                            b6 += "　"; //所得人姓名  補全形空白
                        }

                        if (i >= g & i < h)
                        {
                            tw.WriteLine(
                                "2" +
                                dao.COMPANY_ID +
                                "65" +
                                seq1 +
                                "I" +
                                dt65.Rows[i]["PAYMENT_DATE"].ToString() +
                                dt65.Rows[i]["LICENSE_ID"].ToString() +
                                b11 + seq1 + b4 +
                                dt65.Rows[i]["NT_AMOUNT"].ToString() +
                                dt65.Rows[i]["INS_COST"].ToString() +
                                d1 +
                                " " +
                                utilities.toWide(string.Format("{0,-25}", utilities.convertBig5(dt65.Rows[i]["EMP_NAME"].ToString()).Replace("?", "？"))) +
                                //dt65.Rows[i]["EMP_NAME"].ToString() + b6 +
                                " " +
                                "                "
                            );
                            sum_ONE_TIME_AMOUNT = sum_ONE_TIME_AMOUNT + Convert.ToInt32(dt65.Rows[i]["NT_AMOUNT"].ToString());
                            sum_INS_COST = sum_INS_COST + Convert.ToInt32(dt65.Rows[i]["INS_COST"].ToString());
                            rowCount = rowCount + 1;
                            seq1 = seq1 + 1;

                        }//if end              
                    }
                }//if(dt1.Rows.Count > 0) end

                //65-3 
                tw.WriteLine(
                    "3" +
                    dao.COMPANY_ID +
                    "65" +
                    Convert.ToString(rowCount).PadLeft(9, '0') +
                    Convert.ToString(sum_ONE_TIME_AMOUNT).PadLeft(20, '0') +
                    Convert.ToString(sum_INS_COST).PadLeft(16, '0') +
                    dao.USER_PHONE + b7 +
                    utilities.toWide(string.Format("{0,-25}", utilities.convertBig5(dao.USER_NAME).Replace("?", "？"))) +
                    //dao.USER_NAME + b8 +
                    b9
                );
                tw.Flush();
                txt.ms = ms;
                list.Add(txt);
            }

            CrateTXT_ZIP(list);
            tw.Flush();

        }
        catch
        {
            throw;
        }

    }

    protected void CrateTXT_ZIP(List<pagetxt> list)
    {
        // ZipFile zf = new ZipFile();
        MemoryStream mem_zip = new MemoryStream();
        String zipName = DateTime.Now.ToString("yyyy") + "_ins2.zip";

        //using (ZipFile zip = new ZipFile(System.Text.Encoding.Default))
        using (ZipFile zip = new ZipFile(System.Text.Encoding.GetEncoding("big5")))
        {
            foreach (var item in list)
            {
                item.ms.Seek(0, SeekOrigin.Begin);
                zip.AddEntry(item.filename, item.ms);
            }


            //zip.AddFileFromString("Readme.txt", "", ReadmeText);
            zip.Save(mem_zip);
        }
        HttpContext.Current.Response.Clear();
        HttpContext.Current.Response.ContentType = "application/zip";
        HttpContext.Current.Response.AddHeader("content-disposition", "filename=" + zipName);
        HttpContext.Current.Response.BinaryWrite(mem_zip.ToArray());
        HttpContext.Current.Response.End();
    }



    public bool IdCheck(string strUserID)
    {
        int intAreaNo = 0; //區域碼變數。  
        int intCheckSum = 0;//檢核碼變數。  
        int intCount = 0;//計數變數。  
        string strAreaCode;//區域碼變數。    
        //轉換為大寫。  
        strUserID = strUserID.ToString().ToUpper();
        //取得首碼字母。  
        strAreaCode = strUserID.Substring(0, 1);
        //設定起始值。  
        bool check = false;
        //確定身份證有10碼。  
        if (strUserID.Length == 10)
        {
            //確定首碼在A-Z之間。  
            if (IsNatural_English(strAreaCode))
            {
                //確定第二碼是數字 1 或 2。(1為男生, 2為女生)  
                if (strUserID.Substring(1, 1) == "1" || strUserID.Substring(1, 1) == "2")
                {
                    //取得英文字母對應編號。A -> 10, B -> 11 等等。  
                    string abc = "ABCDEFGHJKLMNPQRSTUVXYWZIO";
                    for (int i = 0; i < abc.Length; i++)
                    {
                        if (strAreaCode == abc.Substring(i, 1))
                        {
                            intAreaNo = i + 10;
                        }
                    }

                    strUserID = intAreaNo.ToString() + strUserID.Substring(1, 9);
                    int count = 0;
                    for (int j = 10; j >= 0; j--)
                    {
                        if (j == 0)
                        {
                            count += Convert.ToInt32(strUserID.ToString().Substring(10, 1)) * 1;
                        }
                        else
                        {
                            int a = strUserID.Length - j - 1;
                            count += Convert.ToInt32(j.ToString().Substring(0, 1)) * Convert.ToInt32(strUserID.Substring(a, 1));
                        }
                    }
                    if ((count * 1.0) % 10 == 0)
                    {
                        check = true;
                    }
                }
                else
                {

                }
            }
            else
            {

            }
        }
        else
        {

        }
        return check;
    }

    //判斷是否為英文字母  
    public bool IsNatural_English(string str)
    {
        System.Text.RegularExpressions.Regex reg1 = new System.Text.RegularExpressions.Regex(@"^[A-Za-z]+$");
        return reg1.IsMatch(str);
    }

    public bool IsNumeric(String strNumber)
    {
        Regex NumberPattern = new Regex("[^0-9.-]");
        return !NumberPattern.IsMatch(strNumber);
    }

    public static bool FullWidthWord(string values)
    {
        bool result = false;
        string pattern = @"^[\u4E00-\u9fa5]+$";
        foreach (char item in values)
        {
            //以Regex判斷是否為中文字，中文字視為全形  
            if (!Regex.IsMatch(item.ToString(), pattern))
            {
                //以16進位值長度判斷是否為全形字  
                if (string.Format("{0:X}", Convert.ToInt32(item)).Length != 2)
                {
                    result = true;
                    break;
                }
            }
        }
        return result;
    }

    public static bool IsChinese(string values)
    {
        bool result = false;
        string pattern = @"^[\u4E00-\u9fa5]+$";
        foreach (char item in values)
        {
            //以Regex判斷是否為中文字，中文字視為全形  
            if (!Regex.IsMatch(item.ToString(), pattern))
            {
                result = true;
                break;
            }
        }
        return result;
    }
}