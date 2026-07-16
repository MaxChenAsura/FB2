using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;

/// <summary>
/// CFB2DJ020BO 的摘要描述
/// </summary>
public class CFB2DJ0200BO : BaseService
{
    public CFB2DJ0200BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    //取得查詢條件的環境津貼等級
    public DataTable getEnvType()
    {
        CFB2DJ0200DAO dj020DAO = new CFB2DJ0200DAO();
        try
        {
            return dj020DAO.getEnvType();
        }
        catch (Exception)
        {

            throw;
        }
    }

    //Grid的部門資料
    public DataTable getDeptData(string dept_no)
    {
        CFB2DJ0200DAO dj020DAO = new CFB2DJ0200DAO();
        try
        {
            return dj020DAO.getDeptData(dept_no);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //新增
    public string insertData(CFB2DJ0200DAO dj020DAO)
    {
        string rtnmessage = "";
        try
        {

            //若需要則要進行邏輯檢查
            DataTable dt = new DataTable();
            //0.檢查PK值有無重覆
            dt = dj020DAO.getPKData();
            if ((int)dt.Rows[0]["resultCount"] > 0)
            {
                rtnmessage += "部門代號+LAYOUT NO+環境津貼等級+生效日期 重覆 \\n";
            }
            //1.檢查新的等級的生效日期是否在相同的津貼等級生效起迄之間
            DateTime start_dt = DateTime.Parse(dj020DAO.START_DT);
            dt = dj020DAO.getMaxEndDTByType();
            if (dt.Rows[0]["maxEndDT"].ToString() != "")
            {
                DateTime maxStartDT = (DateTime)dt.Rows[0]["maxEndDT"];
                if (start_dt <= maxStartDT)
                {
                    rtnmessage += "與相同津貼等級的生效期間重疊 \\n";
                }
            }
         


            //1.檢查結束日期

            DateTime now = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd"));
            if (!dj020DAO.END_DT.Trim().Equals(""))
            {
                DateTime end_dt = DateTime.Parse(dj020DAO.END_DT);
                if (end_dt < now)
                {
                    rtnmessage += "結束日期必須大於系統日 \\n";
                }

                if (start_dt > end_dt)
                {
                    rtnmessage += "結束日期不得小於生效日期 \\n";
                }
            }

            //2.2014/11/12 依國瑞user需求增加判斷 相同的課級單位及loyoutNO僅能有一種等級。
            dt = dj020DAO.getDeptLayoutData();
            if (dt.Rows.Count > 0)
            {
                string type = dt.Rows[0]["ENV_ALLOWANCE_TYPE"].ToString();
                if (type != dj020DAO.ENV_ALLOWANCE_TYPE)
                {
                    rtnmessage += "環境津貼等級與入力不同 \\n";
                }
            }


            //檢查OK更新
            if (rtnmessage == "")
            {
                try
                {
                    BeginTransaction();

                    dj020DAO.insertData();

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

    //修改
    public string updateData(CFB2DJ0200DAO dao)
    {
        string rtnmessage = "";
        try
        {

            //若需要則要進行邏輯檢查

            //DateTime start_dt = DateTime.Parse(dao.START_DT);
            DateTime end_dt = DateTime.Parse(dao.END_DT);
            DateTime now = DateTime.Parse(DateTime.Now.ToString("yyyy/MM/dd"));

            if (end_dt < now)
            {
                rtnmessage += "結束日期不得小於系統日 \\n";
            }

            //檢查OK更新
            if (rtnmessage == "")
            {
                try
                {
                    BeginTransaction();

                    dao.updateData();

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
    public string deleteData(List<Tuple<string, string, string, string>> keysList, List<Tuple<string, string, string, string, string>> checkDataList)
    {
        CFB2DJ0200DAO dj020DAO = new CFB2DJ0200DAO();
        string rtnmessage = "";//存在檢查後的訊息
        try
        {
            foreach (var item in checkDataList)
            {
                string dept_no = item.Item1;
                string layout_no = item.Item2;
                string type = item.Item3;
                string startDT = item.Item4;
                string endDT = item.Item5;

                //檢查 環境津貼申請資料檔 是否已使用, 已使用則無法刪除
                DataTable tmp = dj020DAO.getExistType(dept_no, layout_no, type, startDT, endDT);
                if ((int)tmp.Rows[0]["typecount"] > 0)
                {
                    rtnmessage += "部門代號-" + dept_no + "、Layout NO.-" + layout_no + "津貼等級-" + type + "，已存在環境津貼申請資料，不可刪除 \\n";
                }

            }

            //檢查完成後，逐筆進行刪除
            if (rtnmessage == "")
            {
                try
                {
                    BeginTransaction();
                    foreach (var item in keysList)
                    {
                        dj020DAO.deleteData(item.Item1, item.Item2, item.Item3, item.Item4);
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

}