using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using NPOI.HSSF.UserModel;
using NPOI.HSSF.Util;
using NPOI.SS.Util;

/// <summary>
/// CFB2SJ3000BO 的摘要描述
/// </summary>
public class CFB2SJ3000BO : BaseService
{

    IRow row_G;
    ICell cell_G;

    int pageIndex = 0;     //該部門 需要的頁數(會持續累加)
    int fileTotalPage = 0;     //總頁數

    public CFB2SJ3000BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }


    //新增
    public string insertData(CFB2SJ3000DAO sj300DAO)
    {
        string rtnmessage = "";
        try
        {

            //若需要則要進行邏輯檢查(與DB相關的)
            //00.檢查PK值有無重覆
            DataTable dupdata = sj300DAO.getPKData();
            if ((int)dupdata.Rows[0]["resultCount"] > 0)
            {
                rtnmessage += "考核類別 重覆";
            }

            //檢查OK更新
            if (rtnmessage == "")
            {
                try
                {
                    BeginTransaction();

                    sj300DAO.insertData();

                    Commit();

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



    //刪除
    public string deleteData(List<Tuple<Int32, string>> keysList)
    {
        CFB2SJ3000DAO sj300DAO = new CFB2SJ3000DAO();
        string rtnmessage = "";//存在檢查後的訊息
        try
        {
            foreach (var item in keysList)
            {
                //檢查 
            }



            //檢查完成後，逐筆進行刪除
            if (rtnmessage == "")
            {
                try
                {
                    BeginTransaction();
                    foreach (var item in keysList)
                    {
                        //刪除 考核資料維護檔
                        sj300DAO.deleteData_H(item.Item1, item.Item2);
                      
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
            {
                return rtnmessage;
            }

        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    //更新
    public string updateData(CFB2SJ3000DAO sj300DAO)
    {
        try
        {
            BeginTransaction();

            sj300DAO.updateData();

            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }




















}