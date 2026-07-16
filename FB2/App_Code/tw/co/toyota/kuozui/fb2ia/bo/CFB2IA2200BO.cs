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
using FB2.tw.co.toyota.kuozui.bo;

using System.Text;
using NPOI.HSSF.Util;
using NPOI.HPSF;
using NPOI.POIFS.FileSystem;
using System.Drawing;

/// <summary>
/// CFB2IA2200BO 的摘要描述
/// </summary>
public class CFB2IA2200BO : BaseService
{
    public CFB2IA2200BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public string Execute(CFB2IA2200DAO fb2ia, string INS_DT)
    {
        try
        {
            string result = "";
            result = fb2ia.Execute(INS_DT);
            return result;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    //下載Excel資料
    public IWorkbook createExcelFromTemplate(string type, string excelPath, string data, string INS_YM)
    {
        CFB2IA2200DAO fb2ia = new CFB2IA2200DAO();
        FileStream fs = null;
        IWorkbook workbook = null;
        //取得範本sheet
        ISheet sheet = null;
        try
        {
             fs = new FileStream(excelPath, FileMode.Open, FileAccess.Read);
            //依type判斷要用哪種方式產生
            if (type == "xls")
                workbook = new HSSFWorkbook(fs);
            else
                workbook = new XSSFWorkbook(fs);

            //取得範本sheet
             sheet = workbook.GetSheetAt(0);
            int x = 0;
            if (sheet != null)
            {
                //設定表頭製表日期
                //sheet.Header.Right = DateTime.Now.ToString("yyyy/MM/dd");

                DataTable dt = fb2ia.getExcelData(data, INS_YM);
                IRow row;
                ICell cell;
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        x = i + 2;
                        //將資料寫入範本
                        row = sheet.CreateRow(x);
                        #region 團保每月加保名單
                        if (data == "2200")
                        {
                            //設定製表日期
                            sheet.GetRow(0).CreateCell(19).SetCellValue(DateTime.Now.ToString("yyyy/MM/dd"));
                            cell = row.CreateCell(0);
                            cell.SetCellValue(dt.Rows[i]["DEPT_NAME_20"].ToString());
                            cell = row.CreateCell(1);
                            cell.SetCellValue(dt.Rows[i]["DEPT_NAME_40"].ToString());
                            cell = row.CreateCell(2);
                            cell.SetCellValue(dt.Rows[i]["EMP_CD_NAME"].ToString());
                            cell = row.CreateCell(3);
                            cell.SetCellValue(dt.Rows[i]["PJOB_DESC"].ToString());
                            cell = row.CreateCell(4);
                            cell.SetCellValue(dt.Rows[i]["EMP_ID"].ToString());
                            cell = row.CreateCell(5);
                            cell.SetCellValue(dt.Rows[i]["EMP_NAME"].ToString());
                            cell = row.CreateCell(6);
                            cell.SetCellValue(dt.Rows[i]["REATION_NAME"].ToString());
                            cell = row.CreateCell(7);
                            if (dt.Rows[i]["BIRTH_DT"] != null && dt.Rows[i]["BIRTH_DT"] != DBNull.Value)
                            {
                                DateTime BIRTH_DT = Convert.ToDateTime(dt.Rows[i]["BIRTH_DT"]);
                                cell.SetCellValue((BIRTH_DT.Year - 1911).ToString().PadLeft(3, '0') + "/" + BIRTH_DT.Month.ToString().PadLeft(2, '0') + "/" + BIRTH_DT.Day.ToString().PadLeft(2, '0'));
                            }
                            else
                            {
                                cell.SetCellValue("0");
                            }
                            cell = row.CreateCell(8);
                            cell.SetCellValue(dt.Rows[i]["LICENSE_ID"].ToString());
                            cell = row.CreateCell(9);
                            if (dt.Rows[i]["JOIN_DT"] != null && dt.Rows[i]["JOIN_DT"] != DBNull.Value)
                            {
                                DateTime JOIN_DT = Convert.ToDateTime(dt.Rows[i]["JOIN_DT"]);
                                cell.SetCellValue((JOIN_DT.Year - 1911).ToString().PadLeft(3, '0') + "/" + JOIN_DT.Month.ToString().PadLeft(2, '0') + "/" + JOIN_DT.Day.ToString().PadLeft(2, '0'));
                            }
                            else
                            {
                                cell.SetCellValue("0");
                            }

                            cell = row.CreateCell(10);
                            if (dt.Rows[i]["AENTER_DT"] != null && dt.Rows[i]["AENTER_DT"] != DBNull.Value)
                            {
                                DateTime AENTER_DT = Convert.ToDateTime(dt.Rows[i]["AENTER_DT"]);
                                cell.SetCellValue((AENTER_DT.Year - 1911).ToString().PadLeft(3, '0') + "/" + AENTER_DT.Month.ToString().PadLeft(2, '0') + "/" + AENTER_DT.Day.ToString().PadLeft(2, '0'));
                            }
                            else
                            {
                                cell.SetCellValue("0");
                            }

                            cell = row.CreateCell(11);
                            if (dt.Rows[i]["AQUIT_DT"] != null && dt.Rows[i]["AQUIT_DT"] != DBNull.Value)
                            {
                                DateTime AQUIT_DT = Convert.ToDateTime(dt.Rows[i]["AQUIT_DT"]);
                                cell.SetCellValue((AQUIT_DT.Year - 1911).ToString().PadLeft(3, '0') + "/" + AQUIT_DT.Month.ToString().PadLeft(2, '0') + "/" + AQUIT_DT.Day.ToString().PadLeft(2, '0'));
                            }
                            else
                            {
                                cell.SetCellValue("0");
                            }

                            cell = row.CreateCell(12);
                            cell.SetCellValue(dt.Rows[i]["INS_COND_AMT"].ToString());
                            cell = row.CreateCell(13);
                            if (dt.Rows[i]["BENTER_DT"] != null && dt.Rows[i]["BENTER_DT"] != DBNull.Value)
                            {
                                DateTime BENTER_DT = Convert.ToDateTime(dt.Rows[i]["BENTER_DT"]);
                                cell.SetCellValue((BENTER_DT.Year - 1911).ToString().PadLeft(3, '0') + "/" + BENTER_DT.Month.ToString().PadLeft(2, '0') + "/" + BENTER_DT.Day.ToString().PadLeft(2, '0'));
                            }
                            else
                            {
                                cell.SetCellValue("0");
                            }

                            cell = row.CreateCell(14);
                            if (dt.Rows[i]["BQUIT_DT"] != null && dt.Rows[i]["BQUIT_DT"] != DBNull.Value)
                            {
                                DateTime BQUIT_DT = Convert.ToDateTime(dt.Rows[i]["BQUIT_DT"]);
                                cell.SetCellValue((BQUIT_DT.Year - 1911).ToString().PadLeft(3, '0') + "/" + BQUIT_DT.Month.ToString().PadLeft(2, '0') + "/" + BQUIT_DT.Day.ToString().PadLeft(2, '0'));
                            }
                            else
                            {
                                cell.SetCellValue("0");
                            }

                            cell = row.CreateCell(15);
                            if (dt.Rows[i]["CENTER_DT"] != null && dt.Rows[i]["CENTER_DT"] != DBNull.Value)
                            {
                                DateTime CENTER_DT = Convert.ToDateTime(dt.Rows[i]["CENTER_DT"]);
                                cell.SetCellValue((CENTER_DT.Year - 1911).ToString().PadLeft(3, '0') + "/" + CENTER_DT.Month.ToString().PadLeft(2, '0') + "/" + CENTER_DT.Day.ToString().PadLeft(2, '0'));
                            }
                            else
                            {
                                cell.SetCellValue("0");
                            }

                            cell = row.CreateCell(16);
                            if (dt.Rows[i]["CQUIT_DT"] != null && dt.Rows[i]["CQUIT_DT"] != DBNull.Value)
                            {
                                DateTime CQUIT_DT = Convert.ToDateTime(dt.Rows[i]["CQUIT_DT"]);
                                cell.SetCellValue((CQUIT_DT.Year - 1911).ToString().PadLeft(3, '0') + "/" + CQUIT_DT.Month.ToString().PadLeft(2, '0') + "/" + CQUIT_DT.Day.ToString().PadLeft(2, '0'));
                            }
                            else
                            {
                                cell.SetCellValue("0");
                            }

                            cell = row.CreateCell(17);
                            if (dt.Rows[i]["DENTER_DT"] != null && dt.Rows[i]["DENTER_DT"] != DBNull.Value)
                            {
                                DateTime DENTER_DT = Convert.ToDateTime(dt.Rows[i]["DENTER_DT"]);
                                cell.SetCellValue((DENTER_DT.Year - 1911).ToString().PadLeft(3, '0') + "/" + DENTER_DT.Month.ToString().PadLeft(2, '0') + "/" + DENTER_DT.Day.ToString().PadLeft(2, '0'));
                            }
                            else
                            {
                                cell.SetCellValue("0");
                            }

                            cell = row.CreateCell(18);
                            if (dt.Rows[i]["DQUIT_DT"] != null && dt.Rows[i]["DQUIT_DT"] != DBNull.Value)
                            {
                                DateTime DQUIT_DT = Convert.ToDateTime(dt.Rows[i]["DQUIT_DT"]);
                                cell.SetCellValue((DQUIT_DT.Year - 1911).ToString().PadLeft(3, '0') + "/" + DQUIT_DT.Month.ToString().PadLeft(2, '0') + "/" + DQUIT_DT.Day.ToString().PadLeft(2, '0'));
                            }
                            else
                            {
                                cell.SetCellValue("0");
                            }

                            //20150714 新增公司別
                            cell = row.CreateCell(19);
                            cell.SetCellValue(dt.Rows[i]["company_cd"].ToString());
                        }
                        #endregion

                        #region 團保每月退保名單
                        else if (data == "2201")
                        {
                            //設定製表日期
                            sheet.GetRow(0).CreateCell(19).SetCellValue(DateTime.Now.ToString("yyyy/MM/dd"));
                            cell = row.CreateCell(0);
                            cell.SetCellValue(dt.Rows[i]["DEPT_NAME_20"].ToString());
                            cell = row.CreateCell(1);
                            cell.SetCellValue(dt.Rows[i]["DEPT_NAME_40"].ToString());
                            cell = row.CreateCell(2);
                            cell.SetCellValue(dt.Rows[i]["EMP_CD_NAME"].ToString());
                            cell = row.CreateCell(3);
                            cell.SetCellValue(dt.Rows[i]["EMP_ID"].ToString());
                            cell = row.CreateCell(4);
                            cell.SetCellValue(dt.Rows[i]["EMP_NAME"].ToString());
                            cell = row.CreateCell(5);
                            cell.SetCellValue(dt.Rows[i]["PJOB_DESC"].ToString());
                            cell = row.CreateCell(6);
                            cell.SetCellValue(dt.Rows[i]["HR_CHG_DESC"].ToString());
                            cell = row.CreateCell(7);
                            if (dt.Rows[i]["LEAVE_DT"] != null && dt.Rows[i]["LEAVE_DT"] != DBNull.Value)
                            {
                                DateTime LEAVE_DT = Convert.ToDateTime(dt.Rows[i]["LEAVE_DT"]);
                                cell.SetCellValue((LEAVE_DT.Year - 1911).ToString().PadLeft(3, '0') + "/" + LEAVE_DT.Month.ToString().PadLeft(2, '0') + "/" + LEAVE_DT.Day.ToString().PadLeft(2, '0'));
                            }
                            else
                            {
                                cell.SetCellValue("0");
                            }
                            cell = row.CreateCell(8);
                            cell.SetCellValue(dt.Rows[i]["INS_NAME"].ToString());
                            cell = row.CreateCell(9);

                            cell.SetCellValue(dt.Rows[i]["REATION_NAME"].ToString());


                            cell = row.CreateCell(10);

                            cell.SetCellValue(dt.Rows[i]["LICENSE_ID"].ToString());


                            cell = row.CreateCell(11);
                            if (dt.Rows[i]["AENTER_DT"] != null && dt.Rows[i]["AENTER_DT"] != DBNull.Value)
                            {
                                DateTime AENTER_DT = Convert.ToDateTime(dt.Rows[i]["AENTER_DT"]);
                                cell.SetCellValue((AENTER_DT.Year - 1911).ToString().PadLeft(3, '0') + "/" + AENTER_DT.Month.ToString().PadLeft(2, '0') + "/" + AENTER_DT.Day.ToString().PadLeft(2, '0'));
                            }
                            else
                            {
                                cell.SetCellValue("0");
                            }

                            cell = row.CreateCell(12);
                            if (dt.Rows[i]["AQUIT_DT"] != null && dt.Rows[i]["AQUIT_DT"] != DBNull.Value)
                            {
                                DateTime AQUIT_DT = Convert.ToDateTime(dt.Rows[i]["AQUIT_DT"]);
                                cell.SetCellValue((AQUIT_DT.Year - 1911).ToString().PadLeft(3, '0') + "/" + AQUIT_DT.Month.ToString().PadLeft(2, '0') + "/" + AQUIT_DT.Day.ToString().PadLeft(2, '0'));
                            }
                            else
                            {
                                cell.SetCellValue("0");
                            }
                            cell = row.CreateCell(13);
                            if (dt.Rows[i]["BENTER_DT"] != null && dt.Rows[i]["BENTER_DT"] != DBNull.Value)
                            {
                                DateTime BENTER_DT = Convert.ToDateTime(dt.Rows[i]["BENTER_DT"]);
                                cell.SetCellValue((BENTER_DT.Year - 1911).ToString().PadLeft(3, '0') + "/" + BENTER_DT.Month.ToString().PadLeft(2, '0') + "/" + BENTER_DT.Day.ToString().PadLeft(2, '0'));
                            }
                            else
                            {
                                cell.SetCellValue("0");
                            }

                            cell = row.CreateCell(14);
                            if (dt.Rows[i]["BQUIT_DT"] != null && dt.Rows[i]["BQUIT_DT"] != DBNull.Value)
                            {
                                DateTime BQUIT_DT = Convert.ToDateTime(dt.Rows[i]["BQUIT_DT"]);
                                cell.SetCellValue((BQUIT_DT.Year - 1911).ToString().PadLeft(3, '0') + "/" + BQUIT_DT.Month.ToString().PadLeft(2, '0') + "/" + BQUIT_DT.Day.ToString().PadLeft(2, '0'));
                            }
                            else
                            {
                                cell.SetCellValue("0");
                            }

                            cell = row.CreateCell(15);
                            if (dt.Rows[i]["CENTER_DT"] != null && dt.Rows[i]["CENTER_DT"] != DBNull.Value)
                            {
                                DateTime CENTER_DT = Convert.ToDateTime(dt.Rows[i]["CENTER_DT"]);
                                cell.SetCellValue((CENTER_DT.Year - 1911).ToString().PadLeft(3, '0') + "/" + CENTER_DT.Month.ToString().PadLeft(2, '0') + "/" + CENTER_DT.Day.ToString().PadLeft(2, '0'));
                            }
                            else
                            {
                                cell.SetCellValue("0");
                            }

                            cell = row.CreateCell(16);
                            if (dt.Rows[i]["CQUIT_DT"] != null && dt.Rows[i]["CQUIT_DT"] != DBNull.Value)
                            {
                                DateTime CQUIT_DT = Convert.ToDateTime(dt.Rows[i]["CQUIT_DT"]);
                                cell.SetCellValue((CQUIT_DT.Year - 1911).ToString().PadLeft(3, '0') + "/" + CQUIT_DT.Month.ToString().PadLeft(2, '0') + "/" + CQUIT_DT.Day.ToString().PadLeft(2, '0'));
                            }
                            else
                            {
                                cell.SetCellValue("0");
                            }

                            cell = row.CreateCell(17);
                            if (dt.Rows[i]["DENTER_DT"] != null && dt.Rows[i]["DENTER_DT"] != DBNull.Value)
                            {
                                DateTime DENTER_DT = Convert.ToDateTime(dt.Rows[i]["DENTER_DT"]);
                                cell.SetCellValue((DENTER_DT.Year - 1911).ToString().PadLeft(3, '0') + "/" + DENTER_DT.Month.ToString().PadLeft(2, '0') + "/" + DENTER_DT.Day.ToString().PadLeft(2, '0'));
                            }
                            else
                            {
                                cell.SetCellValue("0");
                            }

                            cell = row.CreateCell(18);
                            if (dt.Rows[i]["DQUIT_DT"] != null && dt.Rows[i]["DQUIT_DT"] != DBNull.Value)
                            {
                                DateTime DQUIT_DT = Convert.ToDateTime(dt.Rows[i]["DQUIT_DT"]);
                                cell.SetCellValue((DQUIT_DT.Year - 1911).ToString().PadLeft(3, '0') + "/" + DQUIT_DT.Month.ToString().PadLeft(2, '0') + "/" + DQUIT_DT.Day.ToString().PadLeft(2, '0'));
                            }
                            else
                            {
                                cell.SetCellValue("0");
                            }

                            //20150714 新增公司別
                            cell = row.CreateCell(19);
                            cell.SetCellValue(dt.Rows[i]["company_cd"].ToString());
                        }
                        #endregion

                        #region 團保在保名單
                        else if (data == "2203")
                        {
                            //設定製表日期
                            sheet.GetRow(0).CreateCell(22).SetCellValue(DateTime.Now.ToString("yyyy/MM/dd"));
                            cell = row.CreateCell(0);
                            cell.SetCellValue(dt.Rows[i]["DEPT_NAME_20"].ToString());
                            cell = row.CreateCell(1);
                            cell.SetCellValue(dt.Rows[i]["DEPT_NAME_40"].ToString());
                            cell = row.CreateCell(2);
                            cell.SetCellValue(dt.Rows[i]["DEPT_NO"].ToString());
                            cell = row.CreateCell(3);
                            cell.SetCellValue(dt.Rows[i]["EMP_ID"].ToString());
                            cell = row.CreateCell(4);
                            cell.SetCellValue(dt.Rows[i]["COMPANY_SNAME"].ToString());
                            cell = row.CreateCell(5);
                            cell.SetCellValue(dt.Rows[i]["EMP_CD_NAME"].ToString());
                            cell = row.CreateCell(6);
                            cell.SetCellValue(dt.Rows[i]["PJOB_DESC"].ToString());
                            cell = row.CreateCell(7);

                            cell.SetCellValue(dt.Rows[i]["INS_NAME"].ToString());

                            cell = row.CreateCell(8);
                            cell.SetCellValue(dt.Rows[i]["REATION_NAME"].ToString());
                            cell = row.CreateCell(9);

                            cell.SetCellValue(dt.Rows[i]["TRANS_ACTION"].ToString());


                            cell = row.CreateCell(10);
                            if (dt.Rows[i]["BIRTH_DT"] != null && dt.Rows[i]["BIRTH_DT"] != DBNull.Value)
                            {
                                DateTime BIRTH_DT = Convert.ToDateTime(dt.Rows[i]["BIRTH_DT"]);
                                cell.SetCellValue((BIRTH_DT.Year - 1911).ToString().PadLeft(3, '0') + "/" + BIRTH_DT.Month.ToString().PadLeft(2, '0') + "/" + BIRTH_DT.Day.ToString().PadLeft(2, '0'));
                            }
                            else
                            {
                                cell.SetCellValue("0");
                            }

                            cell = row.CreateCell(11);

                            cell.SetCellValue(dt.Rows[i]["LICENSE_ID"].ToString());


                            cell = row.CreateCell(12);
                            if (dt.Rows[i]["AENTER_DT"] != null && dt.Rows[i]["AENTER_DT"] != DBNull.Value)
                            {
                                DateTime AENTER_DT = Convert.ToDateTime(dt.Rows[i]["AENTER_DT"]);
                                cell.SetCellValue((AENTER_DT.Year - 1911).ToString().PadLeft(3, '0') + "/" + AENTER_DT.Month.ToString().PadLeft(2, '0') + "/" + AENTER_DT.Day.ToString().PadLeft(2, '0'));
                            }
                            else
                            {
                                cell.SetCellValue("0");
                            }
                            cell = row.CreateCell(13);
                            if (dt.Rows[i]["AQUIT_DT"] != null && dt.Rows[i]["AQUIT_DT"] != DBNull.Value)
                            {
                                DateTime AQUIT_DT = Convert.ToDateTime(dt.Rows[i]["AQUIT_DT"]);
                                //utilities.DateToTw()
                                if (AQUIT_DT.ToString("yyyyMMdd") == "99991231")
                                    cell.SetCellValue("0");
                                else
                                    cell.SetCellValue((AQUIT_DT.Year - 1911).ToString().PadLeft(3, '0') + "/" + AQUIT_DT.Month.ToString().PadLeft(2, '0') + "/" + AQUIT_DT.Day.ToString().PadLeft(2, '0'));
                            }
                            else
                            {
                                cell.SetCellValue("0");
                            }

                            cell = row.CreateCell(14);

                            cell.SetCellValue(dt.Rows[i]["INS_COND_AMT"].ToString());


                            cell = row.CreateCell(15);
                            if (dt.Rows[i]["BENTER_DT"] != null && dt.Rows[i]["BENTER_DT"] != DBNull.Value)
                            {
                                DateTime BENTER_DT = Convert.ToDateTime(dt.Rows[i]["BENTER_DT"]);
                                cell.SetCellValue((BENTER_DT.Year - 1911).ToString().PadLeft(3, '0') + "/" + BENTER_DT.Month.ToString().PadLeft(2, '0') + "/" + BENTER_DT.Day.ToString().PadLeft(2, '0'));
                            }
                            else
                            {
                                cell.SetCellValue("0");
                            }

                            cell = row.CreateCell(16);
                            if (dt.Rows[i]["BQUIT_DT"] != null && dt.Rows[i]["BQUIT_DT"] != DBNull.Value)
                            {
                                DateTime BQUIT_DT = Convert.ToDateTime(dt.Rows[i]["BQUIT_DT"]);
                                if (BQUIT_DT.ToString("yyyyMMdd") == "99991231")
                                    cell.SetCellValue("0");
                                else
                                    cell.SetCellValue((BQUIT_DT.Year - 1911).ToString().PadLeft(3, '0') + "/" + BQUIT_DT.Month.ToString().PadLeft(2, '0') + "/" + BQUIT_DT.Day.ToString().PadLeft(2, '0'));
                            }
                            else
                            {
                                cell.SetCellValue("0");
                            }

                            cell = row.CreateCell(17);
                            if (dt.Rows[i]["CENTER_DT"] != null && dt.Rows[i]["CENTER_DT"] != DBNull.Value)
                            {
                                DateTime CENTER_DT = Convert.ToDateTime(dt.Rows[i]["CENTER_DT"]);
                                cell.SetCellValue((CENTER_DT.Year - 1911).ToString().PadLeft(3, '0') + "/" + CENTER_DT.Month.ToString().PadLeft(2, '0') + "/" + CENTER_DT.Day.ToString().PadLeft(2, '0'));
                            }
                            else
                            {
                                cell.SetCellValue("0");
                            }

                            cell = row.CreateCell(18);
                            if (dt.Rows[i]["CQUIT_DT"] != null && dt.Rows[i]["CQUIT_DT"] != DBNull.Value)
                            {
                                DateTime CQUIT_DT = Convert.ToDateTime(dt.Rows[i]["CQUIT_DT"]);
                                if (CQUIT_DT.ToString("yyyyMMdd") == "99991231")
                                    cell.SetCellValue("0");
                                else
                                    cell.SetCellValue((CQUIT_DT.Year - 1911).ToString().PadLeft(3, '0') + "/" + CQUIT_DT.Month.ToString().PadLeft(2, '0') + "/" + CQUIT_DT.Day.ToString().PadLeft(2, '0'));
                            }
                            else
                            {
                                cell.SetCellValue("0");
                            }
                            cell = row.CreateCell(19);
                            if (dt.Rows[i]["DENTER_DT"] != null && dt.Rows[i]["DENTER_DT"] != DBNull.Value)
                            {
                                DateTime DENTER_DT = Convert.ToDateTime(dt.Rows[i]["DENTER_DT"]);
                                cell.SetCellValue((DENTER_DT.Year - 1911).ToString().PadLeft(3, '0') + "/" + DENTER_DT.Month.ToString().PadLeft(2, '0') + "/" + DENTER_DT.Day.ToString().PadLeft(2, '0'));
                            }
                            else
                            {
                                cell.SetCellValue("0");
                            }
                            cell = row.CreateCell(20);
                            if (dt.Rows[i]["DQUIT_DT"] != null && dt.Rows[i]["DQUIT_DT"] != DBNull.Value)
                            {
                                DateTime DQUIT_DT = Convert.ToDateTime(dt.Rows[i]["DQUIT_DT"]);
                                if (DQUIT_DT.ToString("yyyyMMdd") == "99991231")
                                    cell.SetCellValue("0");
                                else
                                    cell.SetCellValue((DQUIT_DT.Year - 1911).ToString().PadLeft(3, '0') + "/" + DQUIT_DT.Month.ToString().PadLeft(2, '0') + "/" + DQUIT_DT.Day.ToString().PadLeft(2, '0'));
                            }
                            else
                            {
                                cell.SetCellValue("0");
                            }
                            cell = row.CreateCell(22);
                            cell.SetCellValue(dt.Rows[i]["SALARY_ACCOUNT_NO"].ToString());

                            cell = row.CreateCell(23);
                            cell.SetCellValue(dt.Rows[i]["SALARY_ACCOUNT_BANK"].ToString());
                        }
                        #endregion

                    }
                }
                sheet.AutoSizeColumn(0);
                sheet.AutoSizeColumn(1);
                sheet.AutoSizeColumn(2);
                sheet.AutoSizeColumn(3);
                sheet.AutoSizeColumn(4);
                sheet.AutoSizeColumn(5);
                sheet.AutoSizeColumn(6);
                sheet.AutoSizeColumn(7);
                sheet.AutoSizeColumn(8);
                sheet.AutoSizeColumn(9);
                sheet.AutoSizeColumn(10);
                sheet.AutoSizeColumn(11);
                sheet.AutoSizeColumn(12);
                sheet.AutoSizeColumn(13);
                sheet.AutoSizeColumn(14);
                sheet.AutoSizeColumn(15);
                sheet.AutoSizeColumn(16);
                sheet.AutoSizeColumn(17);
                sheet.AutoSizeColumn(18);
                sheet.AutoSizeColumn(19);
                sheet.AutoSizeColumn(20);
                sheet.AutoSizeColumn(21);
                sheet.AutoSizeColumn(22);
                return workbook;
                //匯出Excel
                //if (data == "2200")
                //    ExcelHandle.exportExcel(workbook, "團保每月加保名單." + type);
                //else if (data == "2201")
                //    ExcelHandle.exportExcel(workbook, "團保每月退保名單." + type);
                //else if (data == "2203")
                //    ExcelHandle.exportExcel(workbook, "團保在保名單." + type);
            }
            return null;
        }
        catch (Exception)
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
}
