using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;

/// <summary>
/// CFB2HA0210BO 的摘要描述
/// </summary>
public class CFB2HA0210BO : BaseService
{
    public CFB2HA0210BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public DataTable getDeptLevel()
    {
        CFB2HA0210DAO fb2ha0210 = new CFB2HA0210DAO();
        try
        {
            return fb2ha0210.getDeptLevel();
        }
        catch (Exception)
        {

            throw;
        }
    }
    public string updateDept_Org(CFB2HA0210DAO fb2ha0210)
    {
        try
        {

            BeginTransaction();

            fb2ha0210.updateDept_Org();

            Commit();

            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    public string add_DeptOrg(List<string> dept_no, string up_dept_no, string up_dept_level, string start_dt)
    {
        CFB2HA0210DAO wfb2ha = new CFB2HA0210DAO();
        string rtnmessage = "";
        try
        {
            foreach (string item in dept_no)
            {
                //檢查是否已存在公司組織設定檔
                DataTable tmp = wfb2ha.getExistDeptOrg(item, start_dt);
                if (tmp.Rows.Count > 0)
                {
                    rtnmessage += "部門代號" + item + "，+ 生效日期重覆 \\n";
                }
            }

            //檢查OK逐筆新增
            if (rtnmessage == "")
            {
                try
                {
                    wfb2ha.CREATED_BY = SessionHandle.Current.emp_id;
                    wfb2ha.UPDATED_BY = SessionHandle.Current.emp_id;
                    wfb2ha.FUNC_ID = "FB2HA021";
                    BeginTransaction();
                    foreach (string item in dept_no)
                    {
                        wfb2ha.addDeptLevel(item, up_dept_no, up_dept_level, start_dt);
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
            else
                return rtnmessage;

        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public DataTable getUpDeptData(string up_dept_no)
    {
        try
        {
            CFB2HA0210DAO wfb2ha = new CFB2HA0210DAO();

            return wfb2ha.getUpDeptData(up_dept_no);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public string deleteData(List<Tuple<string, string>> deleteList)
    {
        try
        {
            CFB2HA0210DAO dao = new CFB2HA0210DAO();
            string msg = "";
            DataTable dt = dao.getSalaryYm();
            int salary_ym = Convert.ToInt32(dt.Rows[0]["SALARY_YM"]);
            foreach (var deleteitem in deleteList)
            {
                if (Convert.ToInt32(Convert.ToDateTime(deleteitem.Item2).ToString("yyyyMM")) <= salary_ym)
                    msg += deleteitem.Item1 + "生效日須大於結算月份才可刪除";
            }
            if (msg.Length == 0)
            {
                BeginTransaction();
                foreach (var deleteitem in deleteList)
                {
                    //刪除主檔資料
                    dao.deleteDept_Org(deleteitem.Item1, deleteitem.Item2);
                }
                Commit();
                return "0";
            }
            else
                return msg;
        }
        catch (Exception)
        {
            RollBack();
            throw;
        }
    }
}