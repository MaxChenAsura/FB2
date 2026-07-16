using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;
/// <summary>
/// CFB2SF0100BO 的摘要描述
/// </summary>
public class CFB2SF0100BO : BaseService
{
    public CFB2SF0100BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    //資料確認
    public string Update_TB_S_M_ARREARS_COURT_H(CFB2SF0100DAO fb2sf, string EMP_ID, string DOC_NO)
    {
        try
        {
            BeginTransaction();
            fb2sf.Update_TB_S_M_ARREARS_COURT_H(EMP_ID, DOC_NO);
            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    #region gv_result新刪修
    public string Add(CFB2SF0100DAO fb2sf)
    {
        try
        {
            //取得現有資料(檢查重複)
            DataTable tmp = fb2sf.getExistData();
            BeginTransaction();
            if (tmp.Rows.Count > 0)
            {
                return "資料重複!";
            }
            else
            {
                fb2sf.Add();
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
    public string Update(CFB2SF0100DAO fb2sf)
    {
        try
        {
            BeginTransaction();
            fb2sf.Update();
            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    public string Delete(List<string> delitem_list)
    {
        CFB2SF0100DAO fb2sf = new CFB2SF0100DAO();
        try
        {
            for (int i = 0; i < delitem_list.Count; i++)
            {
                string delitem = delitem_list[i];
                BeginTransaction();
                fb2sf.Delete(delitem);
                fb2sf.Delete2(delitem);
            }
            Commit();
            return "0";
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion
    #region gv_result2新刪修
    public string Add_Dtl(CFB2SF0100DAO fb2sf,string EMP_ID,string DOC_NO)
    {
        try
        {
            //取得現有資料(檢查重複)
            DataTable tmp = fb2sf.getExistData_Dtl();
            BeginTransaction();
            if (tmp.Rows.Count == 0)
            {
                return "工號:" + EMP_ID + "之發文字號:" + DOC_NO + "未建立法扣主檔資料,不允新增";
            }
            else
            {
                fb2sf.Add_TB_S_M_ARREARS_TARGET();
                fb2sf.Update_TB_S_M_ARREARS_COURT_H();
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
    public string Update_Dtl(CFB2SF0100DAO fb2sf)
    {
        try
        {
            BeginTransaction();
            fb2sf.Update_TB_S_M_ARREARS_TARGET();
            fb2sf.Update_TB_S_M_ARREARS_COURT_H2();
            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    public string Update_Dtl_Other(CFB2SF0100DAO fb2sf)
    {
        try
        {
            BeginTransaction();
            fb2sf.Update_TB_S_M_ARREARS_TARGET_Other();
            //fb2sf.Update_TB_S_M_ARREARS_COURT_H2();
            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    public string Delete_Dtl(List<string> delitem_list, List<string> doc_no_item_list, List<string> amountitem_list, List<string> chg_statusitem_list, string emp_id)
    {
        CFB2SF0100DAO fb2sf = new CFB2SF0100DAO();
        try
        {
            for (int i = 0; i < delitem_list.Count; i++)
            {
                string delitem = delitem_list[i];
                string doc_no_item = doc_no_item_list[i];
                string amountitem = amountitem_list[i];
                string chg_statusitem = chg_statusitem_list[i];
                //若此筆資料已有扣款記錄不允刪除
                int tmp = fb2sf.getExistData_Delete(delitem);
                BeginTransaction();
                if (tmp > 0)
                {
                    return "已有法扣款分配資料不允刪除";
                }
                else
                {
                    if (chg_statusitem.Substring(0, 1) != "N")
                    {
                        fb2sf.Update_TB_S_M_ARREARS_TARGET_DEL(delitem);
                    }
                    else {
                        fb2sf.Delete_Dtl(delitem);
                    }
                    
                    fb2sf.Update_TB_S_M_ARREARS_COURT_H3(emp_id, doc_no_item, amountitem);
                }
            }
            Commit();
            return "0";
        }
        catch (Exception)
        {
            throw;
        }
    }
    #endregion

    public bool checkData(CFB2SF0100DAO dao)
    {
        try
        {
            bool b = true;
            DataTable dt = dao.checkData(dao);
            if (dt.Rows.Count > 0)
            {
                if (dt.Rows[0]["r1"].ToString() == "0")
                {
                    dt.Clear();
                    dt = dao.checkData2(dao);
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0]["r1"].ToString() != "0")
                        {
                            b = false;
                        }
                    }

                }
                
            }

            return b;
            
        }
        catch (Exception ex)
        {
            throw;
        }
    }

}