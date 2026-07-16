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
/// CFB2SB1100BO 的摘要描述
/// </summary>
public class CFB2SB1100BO : BaseService
{
	public CFB2SB1100BO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}
 
    # region Qry
    //public DataTable getData(string sys_cd)
    //{
    //    try
    //    {
    //        CFB2SB1100DAO wfb2sb = new CFB2SB1100DAO();
    //        wfb2sb.SYS_CD = sys_cd;
    //        return wfb2sb.getData();
    //    }
    //    catch (Exception)
    //    {
    //        throw;
    //    }
    //}

    public System.Data.DataTable getSYS_ID()
    {
        CFB2SB1100DAO wfb2sb = new CFB2SB1100DAO();
        try
        {
            return wfb2sb.getSYS_ID();
        }
        catch (Exception)
        {

            throw;
        }
    }

    public System.Data.DataTable getSYS_ID(string SUB_CD)
    {
        CFB2SB1100DAO wfb2sb = new CFB2SB1100DAO();
        try
        {
            return wfb2sb.getSYS_ID(SUB_CD);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getUnSelectedData1(string ID, string TYPE)
    {
        CFB2SB1100DAO wfb2sb = new CFB2SB1100DAO();
        try
        {
            return wfb2sb.getUnSelectedData1(ID, TYPE);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getSelectedData2(string ID, string TYPE)
    {
        CFB2SB1100DAO wfb2sb = new CFB2SB1100DAO();
        try
        {
            return wfb2sb.getSelectedData2(ID, TYPE);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getUnselectedData2(string TYPE)
    {
        CFB2SB1100DAO wfb2sb = new CFB2SB1100DAO();
        try
        {
            return wfb2sb.getUnselectedData2(TYPE);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public System.Data.DataTable getEMP_ID(string ID)
    {
        CFB2SB1100DAO wfb2sb = new CFB2SB1100DAO();
        try
        {
            return wfb2sb.getEMP_ID(ID);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public string deleteData(List<string> deleteList)
    {
        try
        {
            CFB2SB1100DAO wfb2sb = new CFB2SB1100DAO();
            BeginTransaction();

            foreach (string deleteitem in deleteList)
            {
                //刪除主檔資料
                wfb2sb.deleteData(deleteitem);
                //刪除明細檔資料
                wfb2sb.deleteDetail(deleteitem);
            }
            Commit();
            return "0";
        }
        catch (Exception)
        {
            RollBack();
            throw;
        }
    }
    //public string updateData(CFB2SB1100DAO fb2ib)
    //{
    //    try
    //    {
    //        BeginTransaction();
    //        fb2ib.updateData();
    //        Commit();
    //        return "0";
    //    }
    //    catch (Exception ex)
    //    {
    //        RollBack();
    //        return ex.Message;
    //    }
    //}
    public string addData(CFB2SB1100DAO fb2ib)
    {
        try
        {
            //取得現有資料
            DataTable tmp = fb2ib.getExistData();
            if (tmp.Rows.Count > 0)
            {
                return "資料重覆!";
            }
            BeginTransaction();
            fb2ib.addData();
            Commit();
            return "0";

        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    #endregion
    public string add_SYS_D_Data(CFB2SB1100DAO fb2sb)
    {
        try
        {
            //取得現有資料
            DataTable tmp = fb2sb.getExist_SYS_D_Data();
            if (tmp.Rows.Count > 0)
            {
                return "無變更資料!";
            }
            BeginTransaction();
            fb2sb.add_SYS_D_Data();
            Commit();
            return "0";

        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    public DataTable getEMP_STATUS(string EMP_ID)
    {        
        try
        {
            CFB2SB1100DAO dao = new CFB2SB1100DAO();
            DataTable dt = dao.getEmp_Status(EMP_ID);
            

            return dt;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public string doSave(CFB2SB1100DAO dao, string selectedItem)
    {
        try
        {
            dao.CREATED_BY = SessionHandle.Current.emp_id.Trim();
            dao.UPDATED_BY = SessionHandle.Current.emp_id.Trim();
            dao.FUNC_ID = "FB2SB110";  
            
            BeginTransaction();

            //刪除加扣款項目人員權限明細檔(TB_S_M_SUBSIDY_MEM_D)
            dao.deleteAllData();

            //新增至 加扣款項目人員權限明細檔(TB_S_M_SUBSIDY_MEM_D)
            int i = 0;
            foreach (string SALARY_ID in selectedItem.Split(",".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
            {
                i++;
                dao.insertDetail(SALARY_ID, i);
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

}