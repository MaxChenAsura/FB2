using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
/// <summary>
/// CFB2SJ010BO 的摘要描述
/// </summary>
public class CFB2SJ0100BO : BaseService
{
	public CFB2SJ0100BO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}


    //考核資料下載(用來下載有block的用法)
    public IWorkbook createExcelFromTemplateDefault(string excelPath, CFB2SJ0100DAO sj010DAO, string functionID)
    {

        FileStream fs = null;
        IWorkbook workbook = null;

        //取得範本sheet
        ISheet sheet = null;
        try
        {
            DataTable dt = new DataTable();
            //取得下載資料(sj010歷史檔)
            if (functionID.ToUpper().Equals("SJ010"))
            {
                dt = sj010DAO.getExcelDataSJ010();
            }
            else if (functionID.ToUpper().Equals("SJ020"))
            {
                dt = sj010DAO.getExcelDataSJ020();
            }



            fs = new FileStream(excelPath, FileMode.Open, FileAccess.ReadWrite);
            workbook = new XSSFWorkbook(fs); //xlsx的方法

            //取得範本sheet
            sheet = workbook.GetSheetAt(0);

            if (sheet != null)
            {

                ICellStyle stringRedLeftStyle = this.setCellStyle(workbook, "left", true, 10);
                IRow row;
                ICell cell;
                //若只有title時 ,儲存錯誤訊息
                if (dt.Rows.Count == 0)
                {
                    row = sheet.CreateRow(2);
                    cell = row.CreateCell(1);
                    cell.CellStyle = stringRedLeftStyle;  //先
                    cell.SetCellValue("無考核資料"); //後

                }

                if (dt.Rows.Count > 0)
                {

                    int x = 0;
                    ICellStyle stringLeftStyle = this.setCellStyle(workbook, "left", true);
                    ICellStyle stringRightStyle = this.setCellStyle(workbook, "right", true);
                    ICellStyle stringCenterStyle = this.setCellStyle(workbook, "center", true);

                    //數字格式,有千分位,
                    //ICellStyle numbericStyle = workbook.CreateCellStyle();
                    //numbericStyle = stringRightStyle;
                    //numbericStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("#,##0");

                    //CellType celltype = this.setCellType("left", true);
                    //cell.SetCellValue((Convert.ToDouble(dt.Rows[i][tableCD + "LEVEL_PAY"].ToString())).ToString("N0"));
                    string dtFormat = "";
                    //dtFormat = dt.Rows[i]["FESTIVAL_DT"].ToString() != "" ? Convert.ToDateTime(dt.Rows[i]["FESTIVAL_DT"].ToString()).ToString("yyyy/MM/dd") : "";

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        x = i + 3;//從第3列開始insert 資料
                        //將資料寫入範本
                        row = sheet.CreateRow(x);

                        //年度
                        cell = row.CreateCell(1);
                        cell.CellStyle = stringLeftStyle;  //先
                        cell.SetCellValue(dt.Rows[i]["ASSESS_YEAR"].ToString()); //後
                        //考核類別
                        cell = row.CreateCell(2);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["ASSESS_TYPE_DESC"].ToString());
                        //工號
                        cell = row.CreateCell(3);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["EMP_ID"].ToString());
                        //姓名
                        cell = row.CreateCell(4);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["EMP_NAME"].ToString());
                        //部門代號
                        cell = row.CreateCell(5);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["DEPT_NO"].ToString());

                        //部級部門名稱
                        cell = row.CreateCell(6);
                        cell.CellStyle = stringLeftStyle;  //先
                        cell.SetCellValue(dt.Rows[i]["DEPT_NAME_20"].ToString()); //後
                        //室級部門名稱
                        cell = row.CreateCell(7);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["DEPT_NAME_30"].ToString());
                        //課級部門名稱
                        cell = row.CreateCell(8);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["DEPT_NAME_40"].ToString());
                        //工級部門名稱
                        cell = row.CreateCell(9);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["DEPT_NAME_50"].ToString());
                        //組級部門名稱
                        cell = row.CreateCell(10);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["DEPT_NAME_60"].ToString());

                        //班級部門名稱
                        cell = row.CreateCell(11);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["DEPT_NAME_70"].ToString());
                        //性別
                        cell = row.CreateCell(12);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["SEX_CD_DESC"].ToString());
                        //直別
                        cell = row.CreateCell(13);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["LINE_CD_DESC"].ToString());
                        //職種
                        cell = row.CreateCell(14);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["WS_CD_DESC"].ToString());
                        //工廠區分
                        cell = row.CreateCell(15);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["PLANT_CD_DESC"].ToString());

                        				

                        //員工區分
                        cell = row.CreateCell(16);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["EMP_CD_DESC"].ToString());
                        //資格
                        cell = row.CreateCell(17);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["LEVEL_CD"].ToString());
                        //級數
                        cell = row.CreateCell(18);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["GRADE_CD"].ToString());
                        //職務代號
                        cell = row.CreateCell(19);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["PJOB_CD"].ToString());
                        //職務名稱
                        cell = row.CreateCell(20);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["PJOB_DESC"].ToString());



                        //教育程度
                        cell = row.CreateCell(21);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["EDUCATION_CD_DESC"].ToString());
                        //出生日期
                        cell = row.CreateCell(22);
                        cell.CellStyle = stringLeftStyle;
                        dtFormat = dt.Rows[i]["BIRTH_DT"].ToString() != "" ? Convert.ToDateTime(dt.Rows[i]["BIRTH_DT"].ToString()).ToString("yyyy/MM/dd") : "";
                        cell.SetCellValue(dtFormat);
                        //資格年資
                        cell = row.CreateCell(23);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["RECENT_LEVEL_WORK_YEARS"].ToString());
                        //年齡
                        cell = row.CreateCell(24);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["AGE"].ToString());
                        //入社年資
                        cell = row.CreateCell(25);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["WORK_YEARS"].ToString());


                        //在職區分
                        cell = row.CreateCell(26);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["EMP_CHG_CD_DESC"].ToString());
                        //能力前3回
                        cell = row.CreateCell(27);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["SCORE_1H_3"].ToString());
                        //能力前2回
                        cell = row.CreateCell(28);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["SCORE_1H_2"].ToString());
                        //能力前1回
                        cell = row.CreateCell(29);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["SCORE_1H_1"].ToString());
                        //業績前3回
                        cell = row.CreateCell(30);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["SCORE_2H_3"].ToString());

                        				

                        //業績前2回
                        cell = row.CreateCell(31);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["SCORE_2H_2"].ToString());
                        //業績前1回
                        cell = row.CreateCell(32);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["SCORE_2H_1"].ToString());
                        //殘業月平均時數
                        cell = row.CreateCell(33);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["OVERTIME_HOUR_MEAN"].ToString());
                        //遲到次數
                        cell = row.CreateCell(34);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["LEAVE_O"].ToString());
                        //早退次數
                        cell = row.CreateCell(35);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["LEAVE_P"].ToString());

                        //曠工日數
                        cell = row.CreateCell(36);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["LEAVE_Q"].ToString());
                        //事假日數
                        cell = row.CreateCell(37);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["LEAVE_A"].ToString());
                        //病假日數
                        cell = row.CreateCell(38);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["LEAVE_B"].ToString());
                        //留職日數
                        cell = row.CreateCell(39);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["RETENTION_DAYS"].ToString());
                        //嘉獎
                        cell = row.CreateCell(40);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["THIRD_CNT_P"].ToString());
                        
                        //小功
                        cell = row.CreateCell(41);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["SECOND_CNT_P"].ToString());
                        //大功
                        cell = row.CreateCell(42);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["FIRST_CNT_P"].ToString());
                        //申誡
                        cell = row.CreateCell(43);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["THIRD_CNT_M"].ToString());
                        //小過
                        cell = row.CreateCell(44);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["SECOND_CNT_M"].ToString());
                        //大過
                        cell = row.CreateCell(45);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["FIRST_CNT_M"].ToString());

                        //總件數
                        cell = row.CreateCell(46);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["PROPOSAL_TOTAL"].ToString());
                        //總分數
                        cell = row.CreateCell(47);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["PROPOSAL_GRADE"].ToString());
                        //平均分數
                        cell = row.CreateCell(48);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["PROPOSAL_GRADE_MEAN"].ToString());
                        //6級件數
                        cell = row.CreateCell(49);
                        cell.CellStyle = stringRightStyle;
                        cell.SetCellValue(dt.Rows[i]["PROPOSAL_6"].ToString());
                        //受援廠別
                        cell = row.CreateCell(50);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["END_PLANT_CD"].ToString());
                        				

                        //受援部門名稱
                        cell = row.CreateCell(51);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["END_DEPT_NAME"].ToString());
                        //受援部門代號
                        cell = row.CreateCell(52);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["END_DEPT_NO"].ToString());
                        //原籍工廠區分
                        cell = row.CreateCell(53);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["ORI_PLANT_CD"].ToString());
                        //原籍部門名稱
                        cell = row.CreateCell(54);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["ORI_DEPT_NAME"].ToString());
                        //原籍部門代號
                        cell = row.CreateCell(55);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["ORI_DEPT_NO"].ToString());

                        				

                        //應援日期
                        cell = row.CreateCell(56);
                        cell.CellStyle = stringLeftStyle;
                        dtFormat = dt.Rows[i]["START_DT"].ToString() != "" ? Convert.ToDateTime(dt.Rows[i]["START_DT"].ToString()).ToString("yyyy/MM/dd") : "";
                        cell.SetCellValue(dtFormat);
                        //預歸建日
                        cell = row.CreateCell(57);
                        cell.CellStyle = stringLeftStyle;
                        dtFormat = dt.Rows[i]["PLAN_END_DT"].ToString() != "" ? Convert.ToDateTime(dt.Rows[i]["PLAN_END_DT"].ToString()).ToString("yyyy/MM/dd") : "";
                        cell.SetCellValue(dtFormat);
                        //當年部門代號
                        cell = row.CreateCell(58);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["DEPT_NO_TY"].ToString());
                        //當年部門名稱
                        cell = row.CreateCell(59);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["DEPT_NAME_TY"].ToString());
                        //去年部門代號
                        cell = row.CreateCell(60);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["DEPT_NO_LY"].ToString());
                        				

                        //去年部門名稱
                        cell = row.CreateCell(61);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["DEPT_NAME_LY"].ToString());
                        //跨部異動
                        cell = row.CreateCell(62);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["DEPT_FLAG"].ToString());
                        //當年昇格
                        cell = row.CreateCell(63);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["LEVELUP_FLAG"].ToString());
                        //法扣對象
                        cell = row.CreateCell(64);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["ARREARS_FLAG"].ToString());
                        //部門提出
                        cell = row.CreateCell(65);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["SCORE_DEPT_DESC"].ToString());
                        //最終考績
                        cell = row.CreateCell(66);
                        cell.CellStyle = stringCenterStyle;
                        cell.SetCellValue(dt.Rows[i]["SCORE_FINAL_DESC"].ToString());


                    }
                    //製表日期
                    ICellStyle stringLeftStyleDate = this.setCellStyle(workbook, "left", false, 14);
                    row = sheet.GetRow(0);
                    cell = row.CreateCell(67);
                    cell.CellStyle = stringLeftStyleDate;
                    cell.SetCellValue("製表日期:"+DateTime.Now.ToString("yyyy/MM/dd"));

                    for (int i = 0; i <= 67; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }


                }

                return workbook;
            }

            return null;
        }
        catch (Exception ex)
        {
            throw;
        }
        finally
        {
            workbook.Clear();
            fs.Close();
            sheet = null;
            workbook = null;
        }
    }


    //有底色的的基本款
    private ICellStyle setCellStyle(IWorkbook workbook, string align, bool isBorder, short fontSize, int colorCD)
    {
        return setCellStyle(workbook, align, isBorder, fontSize, colorCD, false);
    }

    //無底色的基本款
    private ICellStyle setCellStyle(IWorkbook workbook, string align, bool isBorder, short fontSize)
    {
        return setCellStyle(workbook, align, isBorder, fontSize, 0, false);
    }

    //無底色的基本款
    private ICellStyle setCellStyle(IWorkbook workbook, string align, bool isBorder)
    {
        return setCellStyle(workbook, align, isBorder, 12, 0, false);
    }


    //有粗體,無邊框
    private ICellStyle setCellStyle(IWorkbook workbook, string align, short fontSize, bool isBold)
    {
        return setCellStyle(workbook, align, false, fontSize, 0, isBold);
    }

    /// <summary>
    /// 設定資料的格式
    /// </summary>
    /// <param name="workbook"></param>
    /// <param name="align">文字位置</param>
    /// <param name="isBorder">是否要有邊框</param>
    /// <param name="color">背景顏色設定(10:紅,13:黃,14:pink.... )</param>
    /// <returns></returns>
    private ICellStyle setCellStyle(IWorkbook workbook, string align, bool isBorder, short fontSize, int colorCD, bool isBold)
    {
        ICellStyle style = workbook.CreateCellStyle();

        //資料的字型
        IFont cellFont = workbook.CreateFont();
        cellFont.FontName = "微軟正黑體";
        cellFont.FontHeightInPoints = fontSize;  //字型大小
        cellFont.Color = HSSFColor.Black.Index;   //字型顏色
        //是否要有邊框
        if (isBold)
        {
            cellFont.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Bold;   //Bold:粗體字
        }
        else
        {
            cellFont.Boldweight = (short)NPOI.SS.UserModel.FontBoldWeight.Normal;
        }



        style.SetFont(cellFont);

        //是否要有邊框
        if (isBorder)
        {
            //style.BottomBorderColor = HSSFColor.White.Index;
            style.BorderBottom = BorderStyle.Thin;
            style.BorderTop = BorderStyle.Thin;
            style.BorderLeft = BorderStyle.Thin;
            style.BorderRight = BorderStyle.Thin;
        }

        //文字位置 (預設靠左)
        if (align.ToLower() == "center")
        {
            style.Alignment = HorizontalAlignment.Center;
        }
        else if (align.ToLower() == "right")
        {
            style.Alignment = HorizontalAlignment.Right;
        }
        else
        {
            style.Alignment = HorizontalAlignment.Left;
        }

        //背景顏色
        if (colorCD > 0)
        {
            style.FillForegroundColor = (short)colorCD;
            style.FillPattern = FillPattern.SolidForeground;
            //style.FillBackgroundColor = HSSFColor.Yellow.Index;
        }



        return style;
    }

   
}