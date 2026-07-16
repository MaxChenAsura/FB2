using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.SS.Util;
using NPOI.XSSF.UserModel;

/// <summary>
/// CFB2DC1600BO 的摘要描述
/// </summary>
public class CFB2DC1600BO : BaseService
{
    public CFB2DC1600BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    //取得卡鐘編號
    public DataTable getCLOCK_DESC(string clock_no)
    {
        try
        {
            CFB2DC1600DAO wfb2dc = new CFB2DC1600DAO();
            return wfb2dc.getCLOCK_DESC(clock_no);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //修改
    public string updateData(CFB2DC1600DAO dc160DAO)
    {
        string rtnmessage = "";


        try
        {
            //檢查
            //1.工號長度是否存在
            if (dc160DAO.PERSON_ID != "")
            {
                DataTable dt = utilities.getEmpData(dc160DAO.PERSON_ID);
                if ((int)dt.Rows.Count == 0)
                {
                    rtnmessage += "工號不存在 \\n";
                }
                else {
                    dc160DAO.PLANT_CD = dt.Rows[0]["PLANT_CD"].ToString();
                    dc160DAO.CARD_NAME = dt.Rows[0]["EMP_NAME"].ToString();
                }
                dt.Clear();
            }

            //檢查OK更新
            if (rtnmessage == "")
            {
                try
                {
                    BeginTransaction();

                    dc160DAO.updateData();

                    Commit();

                    //reopen
                    if (string.IsNullOrEmpty(dc160DAO.PERSON_ID) == false)
                    {
                        dc160DAO.SP_D_EMP_DUTY_CHECK_STATUS_RE_OPEN(dc160DAO.PERSON_ID);
                    }
                    if (string.IsNullOrEmpty(dc160DAO.PERSON_ID_ORI) == false)
                    {
                        dc160DAO.SP_D_EMP_DUTY_CHECK_STATUS_RE_OPEN(dc160DAO.PERSON_ID_ORI);
                    }
                    return "0";
                }
                catch (Exception ex)
                {
                    RollBack();
                    return ex.Message;
                }

            }
            return rtnmessage;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

}