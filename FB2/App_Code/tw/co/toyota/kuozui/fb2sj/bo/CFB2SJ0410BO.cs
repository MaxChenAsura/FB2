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
/// WFB2SJ0410Service 的摘要描述
/// </summary>
public class CFB2SJ0410BO : BaseService
{
    public CFB2SJ0410BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    //新增 TB_S_M_ASSESS_EMP_SUGGEST
    public string addEMP_SUGGEST(CFB2SJ0410DAO wfb2sj)
    {
        try
        {
            //取得現有資料
            DataTable tmp = wfb2sj.getExistData();
            if (tmp.Rows.Count > 0)
                return "員工考核要望 資料重覆!";
           
                BeginTransaction();
                wfb2sj.addEMP_SUGGEST();
                Commit();
           

            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    //取得修改資料
    public DataTable getUpdData(CFB2SJ0410DAO dao)
    {
        try
        {
            return dao.getUpdData();
        }
        catch (Exception)
        {
            throw;
        }
    }
    //更新 TB_S_M_ASSESS_EMP_SUGGEST
    public string updateEMP_SUGGEST(CFB2SJ0410DAO wfb2sj)
    {
        try
        {
            BeginTransaction();

            wfb2sj.updateEMP_SUGGEST();

            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    //刪除 TB_S_M_ASSESS_EMP_SUGGEST
    public string deletEMP_SUGGEST(string assess_year, string assess_type,List<Tuple<string>> emp_id)
    {
        try
        {
            CFB2SJ0410DAO wfb2sj = new CFB2SJ0410DAO();
            BeginTransaction();
            foreach (var item in emp_id)
            {
                wfb2sj.deleteEMP_SUGGEST(assess_year, assess_type, item.Item1);
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
    //取得員工明細資料
    public DataTable getEmpTargetData(CFB2SJ0410DAO dao)
    {
        try
        {
            return dao.getEmpTargetData();
        }
        catch (Exception)
        {
            throw;
        }
    }

    public int getDept20UpSignCount(string assess_year, string assess_type, string emp_id, string dept_level)
    {
        try
        {
            CFB2SJ0410DAO wfb2sj = new CFB2SJ0410DAO();
            return wfb2sj.getDept20UpSignCount(assess_year, assess_type, emp_id, dept_level);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public string getFilePath()
    {
        try
        {
            CFB2SJ0410DAO dao = new CFB2SJ0410DAO();
            DataTable dt = dao.getFilePath();
            if (dt.Rows.Count > 0)
            {
                return dt.Rows[0]["CODE_VAL1"].ToString();
            }
            else
                return "";

        }
        catch (Exception)
        {

            throw;
        }
    }
}