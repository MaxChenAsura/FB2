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
/// CFB2DC0200BO 的摘要描述
/// </summary>
public class CFB2DC0200BO : BaseService
{
    public CFB2DC0200BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public string deleteCARD_TYPE(List<string> card_type)
    {
        try
        {
            CFB2DC0200DAO wfb2dc = new CFB2DC0200DAO();
            BeginTransaction();
            foreach (string item in card_type)
            {
                wfb2dc.deleteCARD_TYPE(item);
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

    public string addCLOCK_TYPE(CFB2DC0200DAO wfb2dc)
    {
        try
        {
            //取得現有資料
            DataTable tmp = wfb2dc.getExistData();

            if (tmp.Rows.Count > 0)
                return "卡片屬性重覆";
            else
            {
                BeginTransaction();
                wfb2dc.addCLOCK_TYPE();
                Commit();
            }

            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    public string updateCARD_TYPE(CFB2DC0200DAO wfb2dc)
    {
        try
        {
            BeginTransaction();

            wfb2dc.updateCARD_TYPE();

            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    public DataTable getCARD_TYPE()
    {
        try
        {
            CFB2DC0200DAO wfb2dc = new CFB2DC0200DAO();
            return wfb2dc.getCARD_TYPE();
        }
        catch (Exception)
        {
            
            throw;
        }
    }
}