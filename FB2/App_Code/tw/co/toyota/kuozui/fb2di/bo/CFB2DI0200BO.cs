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
/// CFB2DI0200BO 的摘要描述
/// </summary>
public class CFB2DI0200BO : BaseService
{
    public CFB2DI0200BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public string deleteOVERTIME_SPECIAL_HOUR(List<Tuple<string, string, string, string>> dept_no)
    {
        try
        {
            CFB2DI0200DAO wfb2di = new CFB2DI0200DAO();
            string msg = "";
            foreach (var item in dept_no)
            {
                //取得加班申請資料檔資料
                DataTable tmp = wfb2di.getOVERTIME_APPLY(item.Item1, item.Item2, item.Item3, item.Item4);
                if (tmp.Rows.Count > 0)
                {
                    msg += "部門:" + item.Item1 + "，該資料已存在加班申請資料，不可刪除 !\\n";
                    continue;
                }

                //取得IFLOW申請中加班資料VIEW資料 
                DataTable tmp2 = wfb2di.getOVERTIME_IFLOW(item.Item1, item.Item2, item.Item3, item.Item4);
                if (tmp2.Rows.Count > 0)
                {
                    msg += "部門:" + item.Item1 + "，該資料已存在加班申請資料，不可刪除 !\\n";
                    continue;
                }
            }

            if (msg != "")
                return msg;

            BeginTransaction();
            foreach (var item in dept_no)
            {
                wfb2di.deleteOVERTIME_SPECIAL_HOUR(item);
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

    public string addOVERTIME_SPECIAL_HOUR(CFB2DI0200DAO wfb2di, List<string> dept_no_list)
    {
        try
        {
            //20150611 檢查生效日期的月份 是否大於 薪資結算年月
            DataTable dt = new DataTable();
            //0.檢查PK值有無重覆
            dt = wfb2di.getSalaryYM();
            if ((int)dt.Rows[0]["resultCount"] > 0)
            {
                string salaryYM=dt.Rows[0]["salaryYM"].ToString();

                return "生效日期需大於薪資年月:" + salaryYM + "\\n";
            }

            for (int i = 0; i < dept_no_list.Count; i++)
            {
                wfb2di.DEPT_NO = dept_no_list[i];

                //取得現有資料
                DataTable tmp = wfb2di.getExistData();
                if (tmp.Rows.Count > 0)
                    return "部門代號+工數區分+生效日期 資料重覆";
            }
            BeginTransaction();
            for (int i = 0; i < dept_no_list.Count; i++)
            {
                wfb2di.DEPT_NO = dept_no_list[i];
                wfb2di.addOVERTIME_SPECIAL_HOUR();

                //20150611 將新增的部門,工數區分及生效日期進行reopen的動作
                wfb2di.emp_duty_check_status_reopen();
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

    public string updateOVERTIME_SPECIAL_HOUR(CFB2DI0200DAO wfb2di)
    {
        try
        {
            BeginTransaction();

            wfb2di.updateOVERTIME_SPECIAL_HOUR();

            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    public DataTable getDEPT_NAME(string dept_no)
    {
        try
        {
            CFB2DI0200DAO wfb2di = new CFB2DI0200DAO();
            return wfb2di.getDEPT_NAME(dept_no);
        }
        catch (Exception)
        {

            throw;
        }
    }
}