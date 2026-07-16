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
/// CFB2DC0300BO 的摘要描述
/// </summary>
public class CFB2DC0300BO : BaseService
{
    public CFB2DC0300BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public string deleteVENDOR_H(List<string> vendor_no)
    {
        try
        {
            CFB2DC0300DAO wfb2dc = new CFB2DC0300DAO();
            string errmsg = "";
            foreach (string item in vendor_no)
            {
                //取得現有資料(廠商主檔)
                DataTable tmp = wfb2dc.getTB_D_M_VENDOR_D(item);
                if (tmp.Rows.Count > 0)
                    errmsg += "廠商編號：" + item + ",廠商人員已存在卡片資料檔，不可刪除!\\n";

            }

            if (errmsg != "")
                return errmsg;

            BeginTransaction();
            foreach (string item in vendor_no)
            {
                wfb2dc.deleteVENDOR_H(item);

                wfb2dc.deleteVENDOR_D(item);
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

    public string addVENDOR_H(CFB2DC0300DAO wfb2dc)
    {
        try
        {
            //取得現有資料
            DataTable tmp = wfb2dc.getExistData();

            if (tmp.Rows.Count > 0)
                return "廠商編號重覆";
            else
            {
                BeginTransaction();
                wfb2dc.addVENDOR_H();
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

    public string updateVENDOR_H(CFB2DC0300DAO wfb2dc)
    {
        try
        {
            BeginTransaction();

            wfb2dc.updateVENDOR_H();

            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    public DataTable getVENDOR_NO(string vendor_no)
    {
        try
        {
            CFB2DC0300DAO wfb2dc = new CFB2DC0300DAO();
            wfb2dc.VENDOR_NO = vendor_no;
            return wfb2dc.getVENDOR_NO();
        }
        catch (Exception)
        {

            throw;
        }
    }

    public string deleteVENDOR_D(string vendor_no, List<string> vendor_member_no)
    {
        try
        {
            CFB2DC0300DAO wfb2dc = new CFB2DC0300DAO();
            string errmsg = "";
            foreach (string item in vendor_member_no)
            {
                //取得現有資料
                DataTable tmp = wfb2dc.getCARDData(item);
                if (tmp.Rows.Count > 0)
                    errmsg += "廠商人員編號：" + item + ",廠商人員已存在卡片資料檔，不可刪除!\\n";
            }

            if (errmsg != "")
                return errmsg;

            foreach (string item in vendor_member_no)
            {
                BeginTransaction();
                wfb2dc.deleteVENDOR_D(vendor_no, item);
                Commit();
                //執行SP
                int result = wfb2dc.SP_D_UPD_CARD_DATA(item);
            }

            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    public string addVENDOR_D(CFB2DC0300DAO wfb2dc)
    {
        try
        {
            //取得現有資料
            DataTable tmp = wfb2dc.getExistData2();

            if (tmp.Rows.Count > 0)
                return "廠商人員編號重覆";
            else
            {
                BeginTransaction();
                wfb2dc.addVENDOR_D();
                Commit();
                //執行SP
                int result = wfb2dc.SP_D_UPD_CARD_DATA2();
            }

            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    public string updateVENDOR_D(CFB2DC0300DAO wfb2dc)
    {
        try
        {
            BeginTransaction();

            wfb2dc.updateVENDOR_D();

            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    public DataTable getVENDOR_MEMBER_NAME(string vendor_no, string vendor_member_no)
    {
        try
        {
            CFB2DC0300DAO wfb2dc = new CFB2DC0300DAO();
            return wfb2dc.getVENDOR_MEMBER_NAME(vendor_no, vendor_member_no);
        }
        catch (Exception)
        {

            throw;
        }
    }
}