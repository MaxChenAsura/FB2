using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.Util;
/// <summary>
/// CFB2SC2350BO 的摘要描述
/// </summary>
public class CFB2SC2350BO : BaseService
{
    public CFB2SC2350BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    IWorkbook workbook;
    ICellStyle stringLeftStyle;

    public DataTable getTotal(string salary_dt, string salary_type, string pay_kind)
    {
        try
        {
            CFB2SC2350DAO dao = new CFB2SC2350DAO();
            return dao.getTotal(salary_dt, salary_type, pay_kind);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public bool checkClose(string salary_dt, string salary_type, string pay_kind)
    {
        try
        {
            CFB2SC2350DAO dao = new CFB2SC2350DAO();
            return dao.checkClose(salary_dt, salary_type, pay_kind);

        }
        catch (Exception)
        {

            throw;
        }
    }

    public bool checkHaveData(string salary_dt, string salary_type, string pay_kind)
    {
        try
        {
            bool haveData = false;
            CFB2SC2350DAO dao = new CFB2SC2350DAO();
            DataTable dt = dao.getTotal(salary_dt, salary_type, pay_kind);
            int countA = 0;
            int countExceptA = 0;

            if (dt.Rows.Count > 0)
            {
                countA = Convert.ToInt32(dt.Rows[0]["CASH_TOT"].ToString());
                countExceptA = Convert.ToInt32(dt.Rows[0]["TRANS_TOT"].ToString());
                if( ( countA + countExceptA ) > 0 ){
                    haveData = true;
                }
            }
            return haveData;

        }
        catch (Exception)
        {

            throw;
        }
    }

    #region " Update "

    public string updateData(CFB2SC2350DAO dao)
    {
        try
        {

            BeginTransaction();

            foreach (string emp_id in dao.EMP_ID_AREA.Split(','))
            {
                if (dao.SALARY_TYPE == "A")
                {
                    dao.updateDataA(emp_id);
                }
                else
                {
                    dao.updateDataExceptA(emp_id);
                }
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
    #endregion



}