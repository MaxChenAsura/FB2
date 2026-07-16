using FB2.tw.co.toyota.kuozui.bo;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

/// <summary>
/// CFB2HE0200BO 的摘要描述
/// </summary>
public class CFB2HE0200BO : BaseService
{
    ICellStyle style_class;
    public CFB2HE0200BO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    public DataTable getLEVEL_CD()
    {
        try
        {
            CFB2HE0200DAO dao = new CFB2HE0200DAO();
            return dao.getLEVEL_CD();


        }
        catch (Exception ex)
        {
            throw;
        }
    }
    public DataTable getGRADE_CD(CFB2HE0200DAO dao)
    {
        try
        {            
            return dao.getGRADE_CD();


        }
        catch (Exception ex)
        {
            throw;
        }
    }
    public DataTable getEMPDATA(string license_id, string pjob_cd, string apply_dt)
    {
        try
        {
            CFB2HE0200DAO dao = new CFB2HE0200DAO();
            return dao.getEMPDATA(license_id, pjob_cd, apply_dt);


        }
        catch (Exception ex)
        {
            throw;
        }
    }
    public string updateEmp(ArrayList datas, CFB2HE0200DAO dao)
    {
        try
        {            
            dao.UPDATED_BY = SessionHandle.Current.emp_id.Trim();
            dao.FUNC_ID = "FB2HE020";

            BeginTransaction();

            foreach (string[] item in datas)
            {
                dao.LICENSE_ID = item[0];
                int index = (item[1]).LastIndexOf("-");
                index = index > 0 ? index :item[1].Length;
                dao.PJOB_CD = item[1].Substring(0, index);
                dao.APPLY_DT = item[2];              

                dao.updateEmp();
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

    public string updateNewEmp(CFB2HE0200DAO dao)
    {
        try
        {
            dao.UPDATED_BY = SessionHandle.Current.emp_id.Trim();
            dao.FUNC_ID = "FB2HE020";

            BeginTransaction();

            dao.updateNewEmp();

            Commit();

            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    //儲存範本
    public string saveSample(CFB2HE0200DAO he020DAO)
    {
        try
        {
            BeginTransaction();
            //參數 郵件主旨
            he020DAO.SYS_CD = "HE";
            he020DAO.MAIN_CD = he020DAO.MAIN_CD_SUBJECT;
            he020DAO.REMARK = he020DAO.MAIL_SUBJECT;
            he020DAO.saveSample();

            //參數 郵件內容
            he020DAO.SYS_CD = "HE";
            he020DAO.MAIN_CD = he020DAO.MAIN_CD_CONTENT;
            he020DAO.REMARK = he020DAO.MAIL_CONTENT;
            he020DAO.saveSample();
            Commit();

            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }


    #region 產生Excel
    public IWorkbook createExcelFromTemplate(string excelPath, CFB2HE0200DAO sh020DAO, DataTable dt)
    {

        FileStream fs = null;
        IWorkbook workbook = null;

        //取得範本sheet
        ISheet sheet = null;
        try
        {
            fs = new FileStream(excelPath, FileMode.Open, FileAccess.ReadWrite);
            workbook = new XSSFWorkbook(fs); //xlsx的方法

            //取得範本sheet
            sheet = workbook.GetSheetAt(0);

            if (sheet != null)
            {
                
                if (dt.Rows.Count > 0)
                {
                    IRow row;
                    ICell cell;
                    int x = 0;

                    ICellStyle stringLeftStyle = this.setCellStyle(workbook, "left", true);
                    ICellStyle stringRightStyle = this.setCellStyle(workbook, "right", true);
                    ICellStyle stringCenterStyle = this.setCellStyle(workbook, "center", true);

                    //數字格式,有千分位,
                    ICellStyle numbericStyle = workbook.CreateCellStyle();
                    numbericStyle = stringRightStyle;
                    numbericStyle.DataFormat = HSSFDataFormat.GetBuiltinFormat("#,##0");
                               
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        x = i + 1;//從第2列開始insert 資料
                        //將資料寫入範本
                        row = sheet.CreateRow(x);

                        //身份證字號
                        cell = row.CreateCell(1);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["LICENSE_ID"].ToString().Trim());
                        //護照號碼   
                        cell = row.CreateCell(2);
                        cell.CellStyle = stringLeftStyle;
                        //姓名
                        cell = row.CreateCell(3);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["EMP_NAME"].ToString());
                        //英文姓名
                        cell = row.CreateCell(4);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["EMP_ENGNAME"].ToString());
                        //職種
                        cell = row.CreateCell(5);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["WS_CD"].ToString());
                        //聘用單位
                        cell = row.CreateCell(6);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["COMPANY_CD"].ToString());
                        //工廠區分    
                        cell = row.CreateCell(7);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["PLANT_CD"].ToString());
                        //部門代號  
                        cell = row.CreateCell(8);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["DEPT_NO"].ToString());
                        //員工區分  
                        cell = row.CreateCell(9);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["EMP_CD"].ToString());
                        //資格代號   
                        cell = row.CreateCell(10);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["LEVEL_CD"].ToString());
                        //級數代號     
                        cell = row.CreateCell(11);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["GRADE_CD"].ToString());
                        //職務代號     
                        cell = row.CreateCell(12);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["PJOB_CD"].ToString());
                        //工數區分     
                        cell = row.CreateCell(13);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["WORK_CD"].ToString());
                        //預計入社日期     
                        cell = row.CreateCell(14);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["JOIN_DT"].ToString());
                        //試用期滿日     
                        cell = row.CreateCell(15);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["EXAM_EXPIRE_DT"].ToString());
                        //預計派遣日     
                        cell = row.CreateCell(16);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["PLAN_DESPATCH_DT"].ToString());
                        //刷卡管制對象     
                        cell = row.CreateCell(17);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue("Y");
                        //國籍別     
                        cell = row.CreateCell(18);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["NATION_CD"].ToString());
                        //外籍會社   
                        cell = row.CreateCell(19);
                        cell.CellStyle = stringLeftStyle;
                        //房租津貼   
                        cell = row.CreateCell(20);
                        cell.CellStyle = stringLeftStyle;
                        //赴任迄日   
                        cell = row.CreateCell(21);
                        cell.CellStyle = stringLeftStyle;
                        //性別   
                        cell = row.CreateCell(22);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["SEX_CD"].ToString());
                        //出生日期    
                        cell = row.CreateCell(23);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["BIRTH_DT"].ToString());
                        //出生地    
                        cell = row.CreateCell(24);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["BIRTHPLACE"].ToString());
                        //身高    
                        cell = row.CreateCell(25);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["HEIGHT"].ToString());
                        //體重    
                        cell = row.CreateCell(26);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["WEIGHT"].ToString());
                        //血型      
                        cell = row.CreateCell(27);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["BLOOD_TYPE"].ToString());
                        //兵役狀態      
                        cell = row.CreateCell(28);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["ARMY_CD"].ToString());
                        //通訊電話        
                        cell = row.CreateCell(29);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["CONTACT_TEL"].ToString());
                        //行動電話一        
                        cell = row.CreateCell(30);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["MOBILE_TEL_1"].ToString());
                        //個人mail        
                        cell = row.CreateCell(31);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["PERSONAL_EMAIL"].ToString());
                        //緊急連絡人姓名        
                        cell = row.CreateCell(32);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["URG_CONTACT_NAME"].ToString());
                        //緊急連絡人關係說明        
                        cell = row.CreateCell(33);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["URG_CONTACT_RELATION"].ToString());
                        //緊急連絡電話        
                        cell = row.CreateCell(34);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["URG_CONTACT_TEL"].ToString());
                        //戶籍地址郵遞區號        
                        cell = row.CreateCell(35);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["REGISTER_ZIP_CD"].ToString());
                        //戶籍地址         
                        cell = row.CreateCell(36);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["REGISTER_ADDR"].ToString());
                        //通訊地址郵遞區號         
                        cell = row.CreateCell(37);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["CONTACT_ZIP_CD"].ToString());
                        //通訊地址             
                        cell = row.CreateCell(38);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["CONTACT_ADDR"].ToString());
                        //教育程度代碼(最高學歷)             
                        cell = row.CreateCell(39);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["EDUCATION_CD"].ToString());
                        //國家別(最高學歷)           
                        cell = row.CreateCell(40);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["SCHOOL_NATION_CD"].ToString());
                        //學校名稱(最高學歷)           
                        cell = row.CreateCell(41);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["SCHOOL_NAME"].ToString());
                        //科系名稱(最高學歷)         
                        cell = row.CreateCell(42);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["DEPARTMENT_NAME"].ToString());
                        //畢業年度(最高學歷)      
                        cell = row.CreateCell(43);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["GRADUATION_YEAR"].ToString());
                        //公司名稱     
                        cell = row.CreateCell(44);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["EXP_COMPANY_NAME"].ToString());
                        //職稱     
                        cell = row.CreateCell(45);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["EXP_TITLE_DESC"].ToString());
                        //開始年月     
                        cell = row.CreateCell(46);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["START_YEAR"].ToString());
                        //結束年月     
                        cell = row.CreateCell(47);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["END_YEAR"].ToString());
                        //經歷認定總年資     
                        cell = row.CreateCell(48);
                        cell.CellStyle = stringLeftStyle;
                        cell.SetCellValue(dt.Rows[i]["APPROVE_WORK_YEARS"].ToString());               

                    }
                    ////製表日期
                    //ICellStyle stringLeftStyleDate = this.setCellStyle(workbook, "left", false);
                    //row = sheet.GetRow(0);
                    //cell = row.CreateCell(43);
                    //cell.CellStyle = stringLeftStyleDate;
                    //cell.SetCellValue("製表日期:" + DateTime.Now.ToString("yyyy/MM/dd"));


                    for (int i = 0; i <= 49; i++)
                    {
                        sheet.AutoSizeColumn(i);
                    }
                    
                    //if (tableName == "TB_S_M_AWARD_DM")
                    //{
                    //    ExcelHandle.exportExcel(workbook, sh020DAO.AWARD_YEAR + "第" + sh020DAO.AWARD_ROUND + "回年獎維護資料.xlsx");
                    //}
                    //else if (tableName == "TB_S_S_AWARD_D")
                    //{
                    //    ExcelHandle.exportExcel(workbook, sh020DAO.AWARD_YEAR + "第" + sh020DAO.AWARD_ROUND + "回年獎原始資料.xlsx");
                    //}

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
    #endregion

}