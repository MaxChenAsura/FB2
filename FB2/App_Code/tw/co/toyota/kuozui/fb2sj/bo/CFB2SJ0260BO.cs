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
/// WFB2SJ0260Service 的摘要描述
/// </summary>
public class CFB2SJ0260BO : BaseService
{
    public CFB2SJ0260BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    //新增 TB_S_M_ASSESS_EMP_CHG
    public string addEMP_CHG(CFB2SJ0260DAO wfb2sj)
    {
        try
        {
            //取得現有資料
            DataTable tmp = wfb2sj.getExistData();
            if (tmp.Rows.Count > 0)
                return "員工部門轉移 資料重覆!";
           
                BeginTransaction();
                wfb2sj.addEMP_CHG();
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
    public DataTable getUpdData(CFB2SJ0260DAO dao)
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
    //更新 TB_S_M_ASSESS_EMP_CHG
    public string updateEMP_CHG(CFB2SJ0260DAO wfb2sj)
    {
        try
        {
            BeginTransaction();

            wfb2sj.updateEMP_CHG();

            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    //刪除 TB_S_M_ASSESS_EMP_CHG
    public string deletEMP_CHG(string assess_year, string assess_type,List<Tuple<string,string>> liKey)
    {
        try
        {
            CFB2SJ0260DAO wfb2sj = new CFB2SJ0260DAO();
            BeginTransaction();
            foreach (var item in liKey)
            {
                wfb2sj.deleteEMP_CHG(assess_year, assess_type, item.Item1, item.Item2);
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
    //更新 TB_S_M_ASSESS_EMP_CHG
    public string confirmEMP_CHG(CFB2SJ0260DAO wfb2sj)
    {
        try
        {
            //BeginTransaction();

            wfb2sj.execSP_S_ASSESS_EMP_CHG_CONFIRM();
            string msg = utilities.getSPLOG("SP_S_ASSESS_EMP_CHG_CONFIRM");
            if (msg != "") return msg;
            //Commit();
            return "0";
        }
        catch (Exception ex)
        {
            //RollBack();
            return ex.Message;
        }
    }

}