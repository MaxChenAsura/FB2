using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;
using NPOI.SS.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using iTextSharp.text.pdf;
using iTextSharp.text;

/// <summary>
/// CFB2IA1300BO 的摘要描述
/// </summary>
public class CFB2IA1300BO : BaseService
{
    public CFB2IA1300BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    //薪調確定
    public bool Confirm_SALARY_ADJUSTMENT(CFB2IA1300DAO fbsIA)
    {
        bool successed = false;
        try
        {
            //依照畫面查詢條件找出
            DataTable dt = fbsIA.selectData();//薪調對像

            BeginTransaction();

            for (int i = 0; i < dt.Rows.Count; i++)
            {                
                fbsIA.EMP_ID = dt.Rows[i]["EMP_ID"].ToString();
                fbsIA.AVG_SALARY = dt.Rows[i]["AVG_SALARY"].ToString();
                fbsIA.LICENSE_ID = dt.Rows[i]["LICENSE_ID"].ToString();
                fbsIA.A_OLD_INSAMT = dt.Rows[i]["A_OLD_INSAMT"].ToString();
                fbsIA.A_NEW_INSAMT = dt.Rows[i]["A_NEW_INSAMT"].ToString();
                fbsIA.C_OLD_INSAMT = dt.Rows[i]["C_OLD_INSAMT"].ToString();
                fbsIA.C_NEW_INSAMT = dt.Rows[i]["C_NEW_INSAMT"].ToString();
                fbsIA.B_OLD_INSAMT = dt.Rows[i]["B_OLD_INSAMT"].ToString();
                fbsIA.B_NEW_INSAMT = dt.Rows[i]["B_NEW_INSAMT"].ToString();
                fbsIA.HOLD_YEAR = dt.Rows[i]["HOLD_YEAR"].ToString();

                if (fbsIA.Update_TB_I_M_LEVEL_CHG_EFFECT_DT())
                {
                    if (fbsIA.A_OLD_INSAMT != fbsIA.A_NEW_INSAMT)
                    {
                        if (fbsIA.Update_TB_I_M_3IN1_TXN_EFFECT_EDT("A"))
                            successed = fbsIA.Insert3IN1_TXN("A");
                    }

                    if (fbsIA.B_OLD_INSAMT != fbsIA.B_NEW_INSAMT)
                    {
                        if (fbsIA.Update_TB_I_M_3IN1_TXN_EFFECT_EDT("B"))
                            successed = fbsIA.Insert3IN1_TXN("B");
                    }

                    if (fbsIA.C_OLD_INSAMT != fbsIA.C_NEW_INSAMT)
                    {
                        if (fbsIA.Update_TB_I_M_3IN1_TXN_EFFECT_EDT("C"))
                            successed = fbsIA.Insert3IN1_TXN("C");
                    }
                }

            }


            //BeginTransaction();
            //if (fbsIA.Update_TB_I_M_LEVEL_CHG_EFFECT_DT())
            //{
            //    if (fbsIA.A_OLD_INSAMT != fbsIA.A_NEW_INSAMT)
            //    {
            //        if (fbsIA.Update_TB_I_M_3IN1_TXN_EFFECT_EDT("A"))
            //            successed = fbsIA.Insert3IN1_TXN("A");
            //    }

            //    if (fbsIA.B_OLD_INSAMT != fbsIA.B_NEW_INSAMT)
            //    {
            //        if (fbsIA.Update_TB_I_M_3IN1_TXN_EFFECT_EDT("B"))
            //            successed = fbsIA.Insert3IN1_TXN("B");
            //    }

            //    if (fbsIA.C_OLD_INSAMT != fbsIA.C_NEW_INSAMT)
            //    {
            //        if (fbsIA.Update_TB_I_M_3IN1_TXN_EFFECT_EDT("C"))
            //            successed = fbsIA.Insert3IN1_TXN("C");
            //    }

            //}
            Commit();

            return successed;
        }
        catch (Exception)
        {
            RollBack();
            throw;
        }
    }

    //刪除該日期明細
    public bool Delete_TB_I_M_LEVEL_CHG_ALL(string salary_sym)
    {
        CFB2IA1300DAO fbsIA = new CFB2IA1300DAO();
        fbsIA.SALARY_SYM = salary_sym;

        bool successed = false;
        BeginTransaction();
        if (fbsIA.Delete_TB_I_M_LEVEL_CHG_ALL())
        {
            successed = fbsIA.Delete_TB_I_R_3IN1_REPORTDATA_ALL();
        }
        Commit();
        return successed;
    }

    public bool Delete_TB_I_M_LEVEL_CHG(CFB2IA1300DAO fbsIA)
    {
        bool successed = false;
        BeginTransaction();
        if (fbsIA.Delete_TB_I_M_LEVEL_CHG())
        {
            successed = fbsIA.Delete_TB_I_R_3IN1_REPORTDATA();
        }
        Commit();
        return successed;
    }

    //薪調試算-計算資料寫入 TB_I_M_LEVEL_CHG 保險薪調記錄檔/TB_I_R_3IN1_REPORTDATA 保險三合一伸報資料
    public bool Calculate_SALARY_ADJUSTMENT(DataTable dt, string def_sym, string def_eym)
    {
        bool successed = true;
        try
        {
            CFB2IA1300DAO fbsIA = new CFB2IA1300DAO();
            BeginTransaction();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                fbsIA.EMP_ID = dt.Rows[i]["EMP_ID"].ToString();//工號
                fbsIA.SALARY_SYM = def_sym;//指定薪調年月起
                fbsIA.SALARY_EYM = def_eym;//指定薪調年月迄
                fbsIA.COMPANY_CD = dt.Rows[i]["COMPANY_CD"].ToString();//公司別
                fbsIA.EFFECT_DT = "";
                fbsIA.AVG_SALARY = (dt.Rows[i]["INS_AC_AVGSALARY"].ToString() != "" ? dt.Rows[i]["INS_AC_AVGSALARY"].ToString() : "0");//平均薪資
                fbsIA.A_OLD_INSAMT = dt.Rows[i]["INS_A_AMT_OLD"].ToString();//勞保-原投保金額
                fbsIA.A_NEW_INSAMT = (dt.Rows[i]["INS_A_AMT_OLD"].ToString() != "0") ? ((dt.Rows[i]["INS_A_AMT_NEW"].ToString() != "") ? dt.Rows[i]["INS_A_AMT_NEW"].ToString() : "0") : "0";//勞保-新投保金額
                fbsIA.B_OLD_INSAMT = dt.Rows[i]["INS_B_AMT_OLD"].ToString();//健保-原投保金額
                fbsIA.B_NEW_INSAMT = (dt.Rows[i]["INS_B_AMT_OLD"].ToString() != "0") ? ((dt.Rows[i]["INS_B_AMT_NEW"].ToString() != "") ? dt.Rows[i]["INS_B_AMT_NEW"].ToString() : "0") : "0";//健保-新投保金額
                fbsIA.C_OLD_INSAMT = dt.Rows[i]["INS_C_AMT_OLD"].ToString();//勞退-原提繳工資
                fbsIA.C_NEW_INSAMT = (dt.Rows[i]["INS_C_AMT_OLD"].ToString() != "0") ? ((dt.Rows[i]["INS_C_AMT_NEW"].ToString() != "") ? dt.Rows[i]["INS_C_AMT_NEW"].ToString() : "0") : "0";//勞退-新提繳工資
                fbsIA.CREATED_BY = SessionHandle.Current.emp_id;
                fbsIA.UPDATED_BY = SessionHandle.Current.emp_id;
                fbsIA.FUNC_ID = "FB2IA130";
                if ( fbsIA.A_OLD_INSAMT!=fbsIA.A_NEW_INSAMT || fbsIA.B_OLD_INSAMT !=fbsIA.B_NEW_INSAMT || fbsIA.C_OLD_INSAMT!=fbsIA.C_NEW_INSAMT )
                {
                    successed = successed & fbsIA.Insert_TB_I_M_LEVEL_CHG();

                    fbsIA.LICENSE_ID = dt.Rows[i]["LICENSE_ID"].ToString();//身份證/居留證
                    string nation_cd = dt.Rows[i]["NATION_CD"].ToString();
                    string emp_name = dt.Rows[i]["EMP_NAME"].ToString();
                    string birth_dt = dt.Rows[i]["BIRTH_DT"].ToString();
                    string is_pj50 = dt.Rows[i]["IS_PJ50"].ToString();

                    successed = successed & fbsIA.Insert_TB_I_R_3IN1_REPORTDATA(nation_cd, emp_name, birth_dt, is_pj50);
                }
            }
            Commit();
            //將勞退,勞保,健保 三個調整前及調整後之投保薪資一致者,刪除

            return successed;
        }
        catch (Exception ex)
        {
            RollBack();
            throw ex;
        }
    }

    //產生 Excel
    public IWorkbook createExcel(CFB2IA1300DAO wfb2ia,string excelPath, string type)
    {
        IWorkbook workbook = null;
        ISheet sheet = null;
        try
        {
            ICellStyle style1;
            ICellStyle style2;
            DataTable tmp = wfb2ia.get_Excel_Data();
            if (tmp.Rows.Count > 0)
            {
                FileStream fs = new FileStream(excelPath, FileMode.Open, FileAccess.Read);
                //依type判斷要用哪種方式產生
                if (type == "xls")
                    workbook = new HSSFWorkbook(fs);
                else
                    workbook = new XSSFWorkbook(fs);

                //取得範本sheet
                sheet = workbook.GetSheetAt(0);

                style1 = workbook.CreateCellStyle();

                IFont font1 = workbook.CreateFont();
                font1.FontName = "新細明體";
                font1.FontHeightInPoints = 12;
                style1.SetFont(font1);

                IRow row;
                ICell cell;
                style2 = workbook.CreateCellStyle();
                style2.SetFont(font1);

                int x = 0;
                for (int i = 0; i < tmp.Rows.Count; i++)
                {
                    x = i + 1;
                    row = sheet.CreateRow(x);
                    cell = row.CreateCell(0);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["CHG_APP_TYPE"].ToString());
                    cell.CellStyle.Alignment = HorizontalAlignment.Center;

                    cell = row.CreateCell(1);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["LAB8"].ToString());
                    cell.CellStyle.Alignment = HorizontalAlignment.Center;

                    cell = row.CreateCell(2);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["LAB_CHK_CD"].ToString());
                    cell.CellStyle.Alignment = HorizontalAlignment.Center;

                    cell = row.CreateCell(3);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["HEALTH_ORG_ID"].ToString());
                    cell.CellStyle.Alignment = HorizontalAlignment.Center;

                    cell = row.CreateCell(4);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["HEALTH_BUSINESS_ID"].ToString());
                    cell.CellStyle.Alignment = HorizontalAlignment.Center;

                    cell = row.CreateCell(5);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["LAB_FORIGN_YN"].ToString());
                    cell.CellStyle.Alignment = HorizontalAlignment.Center;

                    cell = row.CreateCell(6);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["EMP_NAME"].ToString());
                    cell.CellStyle.Alignment = HorizontalAlignment.Center;

                    cell = row.CreateCell(7);
                    cell.CellStyle = style2;
                    //cell.SetCellValue(tmp.Rows[i]["LAB_FORIGN_YN"].ToString() == "" ? tmp.Rows[i]["LICENSE_ID"].ToString() : "");
                    cell.SetCellValue(tmp.Rows[i]["LICENSE_ID"].ToString());
                    cell.CellStyle.Alignment = HorizontalAlignment.Center;
                    //if 勞保_被保險人外籍=空白 來源.身份證號/居留證號 else 空白 END

                    cell = row.CreateCell(8);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["LAB_FORIGN_YN"].ToString() == "Y" ? tmp.Rows[i]["LICENSE_ID"].ToString() : "");
                    cell.CellStyle.Alignment = HorizontalAlignment.Center;
                    //if 勞保_被保險人外籍='Y' 來源.身份證號/居留證號 else 空白 END

                    cell = row.CreateCell(9);
                    cell.CellStyle = style2;

                    cell.SetCellValue(utilities.DateToTw(Convert.ToDateTime(tmp.Rows[i]["EMP_BIRTH_DT"]).ToString("yyyy/MM/dd")).Replace("/",""));
                    cell.CellStyle.Alignment = HorizontalAlignment.Center;

                    cell = row.CreateCell(10);
                    cell.CellStyle = style2;
                    cell.SetCellValue(Convert.ToInt32(tmp.Rows[i]["SALARY"].ToString()));
                    //cell.SetCellValue(Convert.ToInt32(tmp.Rows[i]["SALARY"].ToString()).ToString("N0"));
                    cell.CellStyle.Alignment = HorizontalAlignment.Center;

                    cell = row.CreateCell(11);
                    cell.CellStyle = style2;
                    cell.SetCellValue(Convert.ToInt32(tmp.Rows[i]["HEA_BEF_AMT"].ToString()));
                    //cell.SetCellValue(Convert.ToInt32(tmp.Rows[i]["HEA_BEF_AMT"].ToString()).ToString("N0"));
                    cell.CellStyle.Alignment = HorizontalAlignment.Center;

                    cell = row.CreateCell(12);
                    cell.CellStyle = style2;
                    cell.SetCellValue(Convert.ToInt32(tmp.Rows[i]["HEA_AFT_AMT"].ToString()));
                    //cell.SetCellValue(Convert.ToInt32(tmp.Rows[i]["HEA_AFT_AMT"].ToString()).ToString("N0"));
                    cell.CellStyle.Alignment = HorizontalAlignment.Center;

                    cell = row.CreateCell(13);
                    cell.CellStyle = style2;
                    cell.SetCellValue(tmp.Rows[i]["SPTYP"].ToString());
                    cell.CellStyle.Alignment = HorizontalAlignment.Center;
                }
                return workbook;
            }
            else
            {
                return null;
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
        finally
        {
            sheet = null;
        }
    }

    //產生PDF來源
    public DataTable get_PDF_Data(string def_sym, string def_eym, string classqty, string company_cd,string orderby)
    {
        DataTable retVal = new DataTable(); ;
        CFB2IA1300DAO fb2IA = new CFB2IA1300DAO();
        try
        {
            retVal = fb2IA.get_PDF_Data(def_sym, def_eym, classqty, company_cd, orderby);
            return retVal;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable get_Company(string company_cd)
    {
        DataTable retVal = new DataTable(); ;
        CFB2IA1300DAO fb2IA = new CFB2IA1300DAO();
        try
        {
            retVal = fb2IA.get_Company_Name(company_cd);
            return retVal;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //取得最近一次薪資計算年月
    public string getLast_SALARY_YM()
    {
        string retVal = "";
        CFB2IA1300DAO fb2IA = new CFB2IA1300DAO();
        try
        {
            retVal = fb2IA.getLast_SALARY_YM();

            return retVal;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //查詢現有薪調資料筆數
    public int get_mon3avgsalry_count(string DEF_SYM, string DEF_EYM)
    {
        int retVal = 0;
        CFB2IA1300DAO fb2IA = new CFB2IA1300DAO();
        try
        {
            retVal = fb2IA.get_mon3avgsalry_count(DEF_SYM, DEF_EYM);
            return retVal;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //產生薪調資料
    public DataTable get_mon3avgsalry_Data(string DEF_SYM, string DEF_EYM)
    {
        DataTable retVal = new DataTable(); ;
        CFB2IA1300DAO fb2IA = new CFB2IA1300DAO();
        try
        {
            retVal = fb2IA.get_mon3avgsalry_Data(DEF_SYM, DEF_EYM);
            return retVal;
        }
        catch (Exception)
        {
            throw;
        }
    }
}