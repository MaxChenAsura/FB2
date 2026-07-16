using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
//using FB2.tw.co.toyota.kuozui.bo;
using FB2.tw.co.toyota.kuozui.bo;
using NPOI.SS.UserModel;
using System.IO;
using NPOI.HSSF.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.Util;

/// <summary>
/// CFF0ME0310Service 的摘要描述
/// </summary>
public class CFF0ME0310BO : BaseService
{
    public CFF0ME0310BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public string exec_SP_D2CT060_D5C_MM(CFF0ME0310DAO ME030DAO)
    {
        string rtnmessage = "";//處理結果說明 Y:失敗,空白:成功

        try
        {
            rtnmessage = ME030DAO.exec_SP_D2CT060_IN_OUT("FSP_MM_005_D5C_IN_DC2");
            //取得回傳訊息
            if (rtnmessage != "")
                return rtnmessage.Replace("\r\n", "").Replace("'", "\"");

            //rtnmessage = ME030DAO.exec_SP_D2CT060_IN_OUT("FSP_MM_005_D5C_OUT");
            //if (rtnmessage != "")
            //    return rtnmessage.Replace("\r\n", "").Replace("'", "\"");

            rtnmessage = ME030DAO.exec_FSP_MM_TO_SAP("FSP_MM_TO_SAP", "MM005_D5C");
            //取得回傳訊息
            if (rtnmessage != "")
                return rtnmessage.Replace("\r\n", "").Replace("'", "\"");

            return "0";
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    public string exec_SP_D2CT060_D5C_FI(CFF0ME0310DAO ME030DAO)
    {
        string rtnmessage = "";//處理結果說明 Y:失敗,空白:成功

        try
        {
            rtnmessage = ME030DAO.exec_SP_D2CT060_IN_OUT("FSP_FI_004_MODUL_DC2");
            //取得回傳訊息
            if (rtnmessage != "")
                return rtnmessage.Replace("\r\n", "").Replace("'", "\"");

            rtnmessage = ME030DAO.exec_SP_D2CT060_IN_OUT("FSP_FI_004_RECOUP_DC2");
            //取得回傳訊息
            if (rtnmessage != "")
                return rtnmessage.Replace("\r\n", "").Replace("'", "\"");

            rtnmessage = ME030DAO.exec_FSP_MM_TO_SAP("FSP_FI_TO_SAP", "FI_PR004");
            //取得回傳訊息
            if (rtnmessage != "")
                return rtnmessage.Replace("\r\n", "").Replace("'", "\"");

            rtnmessage = ME030DAO.exec_FSP_MM_TO_SAP("FSP_FI_TO_SAP", "FI_PR004_M");
            //取得回傳訊息
            if (rtnmessage != "")
                return rtnmessage.Replace("\r\n", "").Replace("'", "\"");

            return "0";
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    public string exec_SP_D2CT060_TRANS(CFF0ME0310DAO ME030DAO)
    {
        string rtnmessage = "";//處理結果說明 Y:失敗,空白:成功

        try
        {
            //檢核
            rtnmessage = chekTransOut(ME030DAO);
            if (rtnmessage != "")
                return rtnmessage;

            //執行
            if (ME030DAO.INVOICE_TYPE == "MM")
                rtnmessage = exec_SP_D2CT060_D5C_MM(ME030DAO);
            if (rtnmessage != "")
                return rtnmessage;

            if (ME030DAO.INVOICE_TYPE == "FI")
                rtnmessage = exec_SP_D2CT060_D5C_FI(ME030DAO);
            if (rtnmessage != "")
                return rtnmessage;

            return "0";
        }
        catch (Exception ex)
        {
            //RollBack();
            //return ex.Message;
            throw;
        }

    }

    public string chekTransOut(CFF0ME0310DAO ME030DAO)
    {
        string rtnmessage = "";//處理結果說明 Y:失敗,空白:成功
        try
        {

            if (ME030DAO.INVOICE_TYPE == "MM")
            {

                //檢查2:是否有待轉出發票
                if (ME030DAO.getresultCount() == 0)
                    return "發票數為0，不必轉出SAP";

                //檢查3:SAP是否已關帳


            }
            return rtnmessage;
        }
        catch (Exception ex)
        {
            throw;
        }
    }
    
}