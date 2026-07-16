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
/// CFB2DI0100BO 的摘要描述
/// </summary>
public class CFB2DI0100BO : BaseService
{
    public CFB2DI0100BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public DataTable getOVERTIME_CD()
    {
        try
        {
            CFB2DI0100DAO wfb2di = new CFB2DI0100DAO();
            return wfb2di.getOVERTIME_CD();
        }
        catch (Exception)
        {

            throw;
        }
    }

    public string deleteOVERTIME_TYPE(List<Tuple<string, string>> overtime_cd)
    {
        try
        {
            CFB2DI0100DAO wfb2di = new CFB2DI0100DAO();
            //歷史資料太多(暫時先MARK, 不檢核)
            //string msg = "";
            //foreach (var item in overtime_cd)
            //{
            //    //取得加班申請資料檔資料
            //    DataTable tmp = wfb2di.getOVERTIME_APPLY(item.Item1, item.Item2);
            //    if (tmp.Rows.Count > 0)
            //    {
            //        msg += "加班類型:" + item.Item1 + "，已存在加班資料，不可刪除 !\\n";
            //        continue;
            //    }

            //    //取得IFLOW申請中加班資料VIEW資料 
            //    DataTable tmp2 = wfb2di.getOVERTIME_IFLOW(item.Item1);
            //    if (tmp2.Rows.Count > 0)
            //    {
            //        msg += "加班類型:" + item.Item1 + "，已存在加班資料，不可刪除 !\\n";
            //        continue;
            //    }
            //}

            //if (msg != "")
            //    return msg;

            BeginTransaction();
            foreach (var item in overtime_cd)
            {
                wfb2di.deleteOVERTIME_TYPE(item);

                wfb2di.deleteOVERTIME_ALLOW(item);
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

    public DataTable getDefaultData(string overtime_cd, string overtime_dt_type)
    {
        try
        {
            CFB2DI0100DAO wfb2di = new CFB2DI0100DAO();
            return wfb2di.getDefaultData(overtime_cd, overtime_dt_type);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public string saveOVERTIME_TYPE(CFB2DI0100DAO wfb2di, string mod)
    {
        try
        {
            //取得現有資料
            DataTable tmp = wfb2di.getOVERTIME_TYPE(wfb2di.OVERTIME_CD, wfb2di.OVERTIME_DT_TYPE);

            BeginTransaction();

            //更新模式
            if (mod == "mod")
            {
                //更新
                wfb2di.updateOVERTIME_TYPE();
            }
            else
            {
                //新增模式
                if (tmp.Rows.Count > 0)
                {
                    //不新增
                    return "加班類型+加班日期類型 資料重覆 !";
                }
                else
                {
                    //不存在資料直接新增
                    wfb2di.addOVERTIME_TYPE();
                }
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

    public string deleteOVERTIME_ALLOW(List<Tuple<string, string, string, string, string>> overtime_cd)
    {
        try
        {
            CFB2DI0100DAO wfb2di = new CFB2DI0100DAO();
            //歷史資料太多(暫時先MARK, 不檢核)
            //string msg = "";
            //foreach (var item in overtime_cd)
            //{
            //    //取得加班申請資料檔資料
            //    DataTable tmp = wfb2di.getOVERTIME_APPLY2(item.Item1, item.Item2, item.Item3, item.Item4, item.Item5);
            //    if (tmp.Rows.Count > 0)
            //    {
            //        msg += "職種:" + item.Item3 + "，該資料已存在加班申請資料，不可刪除 !\\n";
            //        continue;
            //    }

            //    //取得IFLOW申請中加班資料VIEW資料 
            //    DataTable tmp2 = wfb2di.getOVERTIME_IFLOW2(item.Item1, item.Item3, item.Item4, item.Item5);
            //    if (tmp2.Rows.Count > 0)
            //    {
            //        msg += "職種:" + item.Item3 + "，該資料已存在加班申請資料，不可刪除 !\\n";
            //        continue;
            //    }
            //}

            //if (msg != "")
            //    return msg;

            BeginTransaction();
            foreach (var item in overtime_cd)
            {
                wfb2di.deleteOVERTIME_ALLOW2(item);
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

    public string addOVERTIME_ALLOW(CFB2DI0100DAO wfb2di, List<string> pjob_cd_list)
    {
        try
        {
            for (int i = 0; i < pjob_cd_list.Count; i++)
            {
                wfb2di.PJOB_CD = pjob_cd_list[i];
                //取得現有資料
                DataTable tmp = wfb2di.getExistData();
                if (tmp.Rows.Count > 0)
                    return "加班類型+加班日期類型+職種(W/S區分)+職務代號+工數區分 資料重覆";
            }

            BeginTransaction();
            for (int i = 0; i < pjob_cd_list.Count; i++)
            {
                wfb2di.PJOB_CD = pjob_cd_list[i];

                wfb2di.addOVERTIME_ALLOW();
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

    public DataTable getO_HOUR_CD()
    {
        try
        {
            CFB2DI0100DAO wfb2di = new CFB2DI0100DAO();
            return wfb2di.getO_HOUR_CD();
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getO_MUL_CD()
    {
        try
        {
            CFB2DI0100DAO wfb2di = new CFB2DI0100DAO();
            return wfb2di.getO_MUL_CD();
        }
        catch (Exception)
        {

            throw;
        }
    }

    public string getCHG_WORK_CD(string chg_work_cd)
    {
        try
        {
            //用「,」隔開的代碼需存在  共用代碼檔('HB','WORK_CD')
            string result = "0";
            DataRow dr;
            DataTable dt = new DataTable();
            CFB2DI0100DAO wfb2di = new CFB2DI0100DAO();
            string[] chg_work_cd_list = chg_work_cd.Split(',');
            dt = utilities.getCommCode("HB", "WORK_CD", "", "");
            dt.PrimaryKey = new DataColumn[] { dt.Columns["sub_cd"] };

            for (int i = 0; i < chg_work_cd_list.Count(); i++)
            {
                if (chg_work_cd_list[i] != "")
                {
                    //存在否 共用代碼檔 
                    dr = dt.Rows.Find(chg_work_cd_list[i]);
                    if (dr == null)
                    {
                        result = "換休對象需存在共用代碼檔\\n";
                        return result;
                    }
                }
            }
            return result;
        }
        catch (Exception)
        {

            throw;
        }
    }
}