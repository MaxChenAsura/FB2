using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;

/// <summary>
/// CFB2HA010BO 的摘要描述
/// </summary>
public class CFB2HA0100BO : BaseService
{
	public CFB2HA0100BO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    public DataTable getDeptLevel()
    {
        CFB2HA0100DAO wfb2ha = new CFB2HA0100DAO();
        try
        {
            return wfb2ha.getDeptLevel();
        }
        catch (Exception)
        {
            
            throw;
        }
    }

    public string addDept_Level(CFB2HA0100DAO fb2ha010)
    {
        CFB2HA0100DAO wfb2ha = new CFB2HA0100DAO();
        try
        {
            if (fb2ha010.IS_VALID == "N")
            {
                DataTable tmp = wfb2ha.getExistDept(fb2ha010.DEPT_LEVEL);
                if (tmp.Rows.Count > 0)
                {
                    return "部門層級之下仍存在有效的部門資料，不可設定為不使用";
                }
            }

            DataTable dt = wfb2ha.getExistLevel(fb2ha010.DEPT_LEVEL);
            if (dt.Rows.Count > 0)
                return "部門層級重覆";

            BeginTransaction();

            wfb2ha.addDept_Level(fb2ha010);

            Commit();

            return "0";
            

        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    public string updateDept_Level(CFB2HA0100DAO fb2ha010)
    {
        CFB2HA0100DAO wfb2ha = new CFB2HA0100DAO();
        try
        {
            if (fb2ha010.IS_VALID == "N")
            {
                DataTable tmp = wfb2ha.getExistDept(fb2ha010.DEPT_LEVEL);
                if (tmp.Rows.Count > 0)
                {
                    return "部門層級之下仍存在有效的部門資料，不可設定為不使用";
                }
            }

            

            BeginTransaction();

            wfb2ha.updateDept_Level(fb2ha010);

            Commit();

            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    public string delete_DeptLevel(List<string> dept_level)
    {
        CFB2HA0100DAO wfb2ha = new CFB2HA0100DAO();
        string rtnmessage = "";
        try
        {
            foreach (string item in dept_level)
            {
                //檢查是否已存在部門基本資料檔
                DataTable tmp = wfb2ha.getExistDeptLevel(item);
                if ((int)tmp.Rows[0]["deptcount"] > 0)
                {
                    rtnmessage += "部門層級" + item + "，其下已建立部門資料，不可刪除 \\n";
                }
            }

            //檢查OK逐筆刪除
            if (rtnmessage == "")
            {
                try
                {
                    BeginTransaction();
                    foreach (string item in dept_level)
                    {
                        wfb2ha.deleteDeptLevel(item);
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
}