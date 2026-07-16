using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
/// <summary>
/// CFB2SB2200BO 的摘要描述
/// </summary>
public class CFB2SB2200BO : BaseService
{
    public CFB2SB2200BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }


    public System.Data.DataTable getPROCESS_STATUS()
    {
        CFB2SB2200DAO wfb2sb = new CFB2SB2200DAO();
        try
        {
            return wfb2sb.getPROCESS_STATUS();
        }
        catch (Exception)
        {

            throw;
        }
    }

    public System.Data.DataTable getEMP_CD()
    {
        CFB2SB2200DAO wfb2sb = new CFB2SB2200DAO();
        try
        {
            return wfb2sb.getEMP_CD();
        }
        catch (Exception)
        {

            throw;
        }
    }

    public System.Data.DataTable getSALARY_ITEM(string tablename, string columnname, string qrystr)
    {
        CFB2SB2200DAO wfb2sb = new CFB2SB2200DAO();
        try
        {
            return wfb2sb.getSALARY_ITEM(tablename, columnname, qrystr);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public string Approve(List<CFB2SB2200DAO> listApprove)
    {
        string msg = "0";
        DateTime dt1, dt2;
        int months;
        try
        {
            BeginTransaction();
            foreach (CFB2SB2200DAO fb2sb in listApprove)
            {
                switch (fb2sb.CHG_STATUS)
                {
                    case "N":
                        fb2sb.addData_11();

                        dt1 = Convert.ToDateTime(fb2sb.START_DT_B);
                        dt2 = Convert.ToDateTime(fb2sb.END_DATE_B);
                        months = (dt2.Year - dt1.Year) * 12 + dt2.Month - dt1.Month + 1;
                        for (int i = 0; i < months; i++)
                        {
                            fb2sb.DATA_YM = dt1.AddMonths(i).ToString("yyyyMM");
                            fb2sb.addData_12();
                        }
                        fb2sb.updateData_13();
                        break;
                    case "U":
                        fb2sb.updateData_21();
                        if (fb2sb.END_DATE_A == fb2sb.END_DATE_B)
                        {
                            fb2sb.updateData_22();
                        }
                        if (fb2sb.END_DATE_A != fb2sb.END_DATE_B)
                        {
                            if (Convert.ToDateTime(fb2sb.END_DATE_A) > Convert.ToDateTime(fb2sb.END_DATE_B))
                            {
                                dt1 = Convert.ToDateTime(fb2sb.END_DATE_B);
                                dt2 = Convert.ToDateTime(fb2sb.END_DATE_A);
                                //若 異動前加扣款期間迄 的年月+1 <= 資料列.異動後加扣款期間迄 的年月資料
                                if (Convert.ToInt32(dt1.AddMonths(1).ToString("yyyyMM")) <= Convert.ToInt32(dt2.ToString("yyyyMM"))) 
                                {
                                    months = (dt2.Year - dt1.Year) * 12 + dt2.Month - dt1.Month;
                                    for (int i = 1; i <= months; i++)
                                    {
                                        fb2sb.DATA_YM = dt1.AddMonths(i).ToString("yyyyMM");
                                        fb2sb.addData_23_1();
                                    }
                                }
                            }
                            else if (Convert.ToDateTime(fb2sb.END_DATE_A) < Convert.ToDateTime(fb2sb.END_DATE_B))
                            {
                                fb2sb.deleteData_23_2();
                                fb2sb.updateData_23_2();
                            }

                            
                        }
                        fb2sb.updateData_24();
                        break;

                    case "D":
                        fb2sb.deleteData_31();
                        fb2sb.updateData_32();
                        break;
                }
            }
            Commit();
        }
        catch (Exception ex)
        {
            RollBack();
            msg = ex.Message;
        }
        return msg;
    }

    public string updateData_reject(List<CFB2SB2200DAO> listReject)
    {
        try
        {
            BeginTransaction();
            foreach (CFB2SB2200DAO fb2sb in listReject)
            {
                fb2sb.updateData_reject();
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
}
