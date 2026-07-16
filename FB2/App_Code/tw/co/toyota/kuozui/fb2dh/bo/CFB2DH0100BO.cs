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
/// CFB2DH0100BO 的摘要描述
/// </summary>
public class CFB2DH0100BO : BaseService
{
    public CFB2DH0100BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public string deleteLEAVE_TYPE_H(List<string> main_leave_cd)
    {
        try
        {
            CFB2DH0100DAO wfb2dh = new CFB2DH0100DAO();
            string msg = "";
            foreach (string item in main_leave_cd)
            {
                //取得請假申請資料檔資料
                DataTable tmp = wfb2dh.getLEAVE_APPLY(item,"");
                if (tmp.Rows.Count > 0)
                {
                    msg += "主假別:" + item + ",已存在請假資料,不可刪除 !\\n";
                    continue;
                }

                //取得請假資料日檔資料
                DataTable tmp5 = wfb2dh.getLEAVE_APPLY_DAY(item, "");
                if (tmp5.Rows.Count > 0)
                {
                    msg += "主假別:" + item + ",已存在日請假資料,不可刪除 !\\n";
                    continue;
                }

                //取得其他相關檔案資料
                DataTable tmp1 = wfb2dh.getLEAVE_TYPE_D(item, "");
                if (tmp1.Rows.Count > 0)
                {
                    msg += "主假別:" + item + ",已存在其他相關檔案,不可刪除 !\\n";
                    continue;
                }

                DataTable tmp2 = wfb2dh.getLEAVE_MAX_DAY(item, "");
                if (tmp2.Rows.Count > 0)
                {
                    msg += "主假別:" + item + ",已存在其他相關檔案,不可刪除 !\\n";
                    continue;
                }
                DataTable tmp3 = wfb2dh.getLEAVE_TIME_LIMIT(item, "");
                if (tmp3.Rows.Count > 0)
                {
                    msg += "主假別:" + item + ",已存在其他相關檔案,不可刪除 !\\n";
                    continue;
                }
                DataTable tmp4 = wfb2dh.getLEAVE_ALLOW(item, "");
                if (tmp4.Rows.Count > 0)
                {
                    msg += "主假別:" + item + ",已存在其他相關檔案,不可刪除 !\\n";
                    continue;
                }
            }

            if (msg != "")
                return msg;

            BeginTransaction();
            foreach (string item in main_leave_cd)
            {
                wfb2dh.deleteLEAVE_TYPE_H(item);
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

    public string addLEAVE_TYPE_H(CFB2DH0100DAO wfb2dh)
    {
        try
        {
            //取得現有資料
            DataTable tmp = wfb2dh.getExistData();

            if (tmp.Rows.Count > 0)
                return "主假別資料重覆!";
            else
            {
                BeginTransaction();
                wfb2dh.addLEAVE_TYPE_H();
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

    public string updateLEAVE_TYPE_H(CFB2DH0100DAO wfb2dh)
    {
        try
        {
            BeginTransaction();

            wfb2dh.updateLEAVE_TYPE_H();

            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    public string deleteLEAVE_TYPE_D(List<Tuple<string, string>> main_leave_cd)
    {
        try
        {
            CFB2DH0100DAO wfb2dh = new CFB2DH0100DAO();
            string msg = "";
            foreach (var item in main_leave_cd)
            {
                //取得請假實績資料檔資料
                DataTable tmp = wfb2dh.getLEAVE_APPLY(item.Item1, item.Item2);
                if (tmp.Rows.Count > 0)
                {
                    msg += "子假別:" + item.Item2 + ",該主假別及子假別，已存在請假申請資料，不可刪除 !\\n";
                    continue;
                }

                //取得請假資料日檔資料
                DataTable tmp5 = wfb2dh.getLEAVE_APPLY_DAY(item.Item1, item.Item2);
                if (tmp5.Rows.Count > 0)
                {
                    msg += "子假別:" + item.Item2 + ",該主假別及子假別，已存在日請假資料，不可刪除 !\\n";
                    continue;
                }

                //子假別使用上限控管條件檔
                DataTable tmp2 = wfb2dh.getLEAVE_MAX_DAY(item.Item1, item.Item2);
                if (tmp2.Rows.Count > 0)
                {
                    msg += "主假別:" + item.Item1 + ",已存在假別使用上限控管條件檔，不可刪除 !\\n";
                    continue;
                }

                //子假別請假時段限制條件檔
                DataTable tmp3 = wfb2dh.getLEAVE_TIME_LIMIT(item.Item1, item.Item2);
                if (tmp3.Rows.Count > 0)
                {
                    msg += "主假別:" + item.Item1 + ",已存在請假時段限制條件檔，不可刪除 !\\n";
                    continue;
                }

                //子假別適用人員設定檔
                DataTable tmp4 = wfb2dh.getLEAVE_ALLOW(item.Item1, item.Item2);
                if (tmp4.Rows.Count > 0)
                {
                    msg += "主假別:" + item.Item1 + ",已存在請假時段限制條件檔，不可刪除 !\\n";
                    continue;
                }
            }

            if (msg != "")
                return msg;

            BeginTransaction();
            foreach (var item in main_leave_cd)
            {
                wfb2dh.deleteLEAVE_TYPE_D(item);
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

    public DataTable getDefaultData(string main_leave_cd, string sub_leave_cd)
    {
        CFB2DH0100DAO wfb2dh = new CFB2DH0100DAO();
        try
        {
            return wfb2dh.getDefaultData(main_leave_cd, sub_leave_cd);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public string saveLEAVE_TYPE_D(CFB2DH0100DAO wfb2dh, string mod)
    {
        try
        {
            //取得現有資料
            DataTable tmp = wfb2dh.getLEAVE_TYPE_D(wfb2dh.MAIN_LEAVE_CD, wfb2dh.SUB_LEAVE_CD);

            BeginTransaction();

            //更新模式
            if (mod == "mod")
            {
                //更新
                wfb2dh.updateLEAVE_TYPE_D();
            }
            else
            {
                //新增模式
                if (tmp.Rows.Count > 0)
                {
                    //不新增
                    return "主假別+子假別資料重覆 !";
                }
                else
                {
                    //不存在資料直接新增
                    wfb2dh.addLEAVE_TYPE_D();
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

    public string deleteLEAVE_MAX_DAY(List<Tuple<string, string, string, string>> main_leave_cd)
    {
        try
        {
            CFB2DH0100DAO wfb2dh = new CFB2DH0100DAO();
            BeginTransaction();
            foreach (var item in main_leave_cd)
            {
                wfb2dh.deleteLEAVE_MAX_DAY(item);
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

    public string addLEAVE_MAX_DAY(CFB2DH0100DAO wfb2dh)
    {
        try
        {
            //取得現有資料
            DataTable tmp = wfb2dh.getLEAVE_MAX_DAY2();

            if (tmp.Rows.Count > 0)
                return "主假別+合併子假別 資料重覆！";
            else
            {
                BeginTransaction();
                wfb2dh.addLEAVE_MAX_DAY();
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

    public string updateLEAVE_MAX_DAY(CFB2DH0100DAO wfb2dh)
    {
        try
        {
            BeginTransaction();

            wfb2dh.updateLEAVE_MAX_DAY();

            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    public string deleteLEAVE_TIME_LIMIT(List<Tuple<string, string, string, string>> main_leave_cd)
    {
        try
        {
            CFB2DH0100DAO wfb2dh = new CFB2DH0100DAO();
            BeginTransaction();
            foreach (var item in main_leave_cd)
            {
                wfb2dh.deleteLEAVE_TIME_LIMIT(item);
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

    public string addLEAVE_TIME_LIMIT(CFB2DH0100DAO wfb2dh)
    {
        try
        {
            //取得現有資料
            DataTable tmp = wfb2dh.getLEAVE_TIME_LIMIT2();

            if (tmp.Rows.Count > 0)
                return "主假別+子假別+開始時段+結束時段 資料重覆 !";
            else
            {
                BeginTransaction();
                wfb2dh.addLEAVE_TIME_LIMIT();
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

    public string updateLEAVE_TIME_LIMIT(CFB2DH0100DAO wfb2dh)
    {
        try
        {
            BeginTransaction();

            wfb2dh.updateLEAVE_TIME_LIMIT();

            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    public string deleteLEAVE_ALLOW(List<Tuple<string, string, string, string>> main_leave_cd)
    {
        try
        {
            CFB2DH0100DAO wfb2dh = new CFB2DH0100DAO();
            string msg = "";
            /*
            foreach (var item in main_leave_cd)
            {
                
                //取得請假申請資料檔資料
                DataTable tmp = wfb2dh.getLEAVE_APPLY2(item);
                if (tmp.Rows.Count > 0)
                {
                    msg += "該資料已存在請假申請資料，不可刪除 !\\n";
                    continue;
                }
               
                //取得請假資料日檔資料
                DataTable tmp2 = wfb2dh.getLEAVE_APPLY_DAY2(item);
                if (tmp2.Rows.Count > 0)
                {
                    msg += "該資料已存在日請假資料，不可刪除 !\\n";
                    continue;
                }

            }
             */
                
            if (msg != "")
                return msg;

            BeginTransaction();
            foreach (var item in main_leave_cd)
            {
                wfb2dh.deleteLEAVE_ALLOW(item);
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

    public string addLEAVE_ALLOW(CFB2DH0100DAO dh101DAO)
    {
        try
        {
            //取得現有資料
            DataTable tmp = dh101DAO.getLEAVE_ALLOW2();
            if (tmp.Rows.Count > 0)
                return "主假別+子假別+員工區分+職務代碼 資料重覆 !";
            if (dh101DAO.PJOB_CD.Trim() != "")
            {
                tmp = dh101DAO.getPJOB();
                if (tmp.Rows.Count == 0)
                    return "職務代碼不存在 !";
            }
            BeginTransaction();
            dh101DAO.addLEAVE_ALLOW();
            Commit();

            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }


    public DataTable getMAIN_LEAVE_DESC(string main_leave_cd)
    {
        try
        {
            CFB2DH0100DAO wfb2dh = new CFB2DH0100DAO();
            return wfb2dh.getMAIN_LEAVE_DESC(main_leave_cd);
        }
        catch (Exception)
        {

            throw;
        }
    }
}