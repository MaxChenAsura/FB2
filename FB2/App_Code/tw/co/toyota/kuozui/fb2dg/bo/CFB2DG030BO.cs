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
/// CFB2DG030BO 的摘要描述
/// </summary>
public class CFB2DG030BO : BaseService
{
	public CFB2DG030BO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}
    public DataTable getData(string emp_id)
    {
        CFB2DG030DAO dao = new CFB2DG030DAO();
        try
        {
            return dao.getDefaultData(emp_id);
        }
        catch (Exception)
        {

            throw;
        }
    }
    # region Qry
    //public DataTable getData(string sys_cd)
    //{
    //    try
    //    {
    //        CFB2DG030DAO fb2dg = new CFB2DG030DAO();
    //        fb2dg.SYS_CD = sys_cd;
    //        return fb2dg.getData();
    //    }
    //    catch (Exception)
    //    {
    //        throw;
    //    }
    //}

    public System.Data.DataTable checkNEED_SELECT(string emp_id)
    {
        CFB2DG030DAO fb2dg = new CFB2DG030DAO();
        try
        {
            return fb2dg.checkNEEDSELECT(emp_id);
        }
        catch (Exception)
        {

            throw;
        }
    }
    public DataTable getCarParkNo(string emp_id)
    {
        CFB2DG030DAO fb2dg = new CFB2DG030DAO();
        try
        {
            return fb2dg.getCarParkNo(emp_id);
        }
        catch (Exception)
        {

            throw;
        }
    }
    public System.Data.DataTable getPLANT_CD()
    {
        CFB2DG030DAO fb2dg = new CFB2DG030DAO();
        try
        {
            return fb2dg.getPARKING_PLANT_CD();
        }
        catch (Exception)
        {

            throw;
        }
    }
    public System.Data.DataTable getCAR_TYPE()
    {
        CFB2DG030DAO fb2dg = new CFB2DG030DAO();
        try
        {
            return fb2dg.getCAR_TYPE();
        }
        catch (Exception)
        {

            throw;
        }
    }
    public System.Data.DataTable getPARKING_CD()
    {
        CFB2DG030DAO fb2dg = new CFB2DG030DAO();
        try
        {
            return fb2dg.getPARKING_CD();
        }
        catch (Exception)
        {

            throw;
        }
    }
    public DataTable getEMP_CAR_BRAND(string emp_id)
    {
        CFB2DG030DAO dao = new CFB2DG030DAO();
        try
        {
            return dao.getCB(emp_id);
        }
        catch (Exception)
        {

            throw;
        }
    }
    public System.Data.DataTable getCAR_TYPE(CFB2DG030DAO fb2dg)
    {

        try
        {
            return fb2dg.getCAR_TYPE();
        }
        catch (Exception)
        {

            throw;
        }
    }
    public System.Data.DataTable getCAR_PARK_NO(CFB2DG030DAO fb2dg)
    {

        try
        {
            return fb2dg.getCAR_PARK_NO();
        }
        catch (Exception)
        {

            throw;
        }
    }
    public System.Data.DataTable getREMAINDER_PARKING_SPOT(CFB2DG030DAO fb2dg)
    {

        try
        {
            return fb2dg.getREMAINDER_PARKING_SPOT();
        }
        catch (Exception)
        {

            throw;
        }
    }
    public System.Data.DataTable getCAR_BRAND()
    {
        CFB2DG030DAO fb2dg = new CFB2DG030DAO();
        try
        {
            return fb2dg.getCAR_BRAND();
        }
        catch (Exception)
        {

            throw;
        }
    }



    public System.Data.DataTable getSYS_ID(string SUB_CD)
    {
        CFB2DG030DAO fb2dg = new CFB2DG030DAO();
        try
        {
            return fb2dg.getSYS_ID(SUB_CD);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public System.Data.DataTable getaddData(CFB2DG030DAO fb2dg)
    {

        try
        {
            return fb2dg.getaddData();
        }
        catch (Exception)
        {

            throw;
        }
    }
    public System.Data.DataTable getaddData2(string ID)
    {
        CFB2DG030DAO fb2dg = new CFB2DG030DAO();
        try
        {
            return fb2dg.getaddData2(ID);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public System.Data.DataTable getModeData(CFB2DG030DAO fb2dg)
    {
        
        try
        {
            return fb2dg.getModeData();
        }
        catch (Exception)
        {

            throw;
        }
    }
    public System.Data.DataTable getModeData2(string ID)
    {
        CFB2DG030DAO fb2dg = new CFB2DG030DAO();
        try
        {
            return fb2dg.getModeData2(ID);
        }
        catch (Exception)
        {

            throw;
        }
    }


    public System.Data.DataTable getFUNC_ID(string ID)
    {
        CFB2DG030DAO fb2dg = new CFB2DG030DAO();
        try
        {
            return fb2dg.getFUNC_ID(ID);
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
            CFB2DG030DAO wfb2dg = new CFB2DG030DAO();
            BeginTransaction();


            foreach (string deleteitem in deleteList)
            {
                //刪除主檔資料

                wfb2dg.addData_1(deleteitem);
            }

            foreach (string deleteitem in deleteList)
            {
                //刪除主檔資料
                wfb2dg.deleteData_2(deleteitem);
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
    public string updateData(CFB2DG030DAO fb2dg)
    {
        try
        {
            BeginTransaction();
            fb2dg.updateData();
            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    public string checkParking(string CAR_PARK_NO)
    {
        try
        {
            CFB2DG030DAO dao = new CFB2DG030DAO();
            return dao.checkParking(CAR_PARK_NO);
        }
        catch (Exception)
        {

            throw;
        }
    }
    public string addData_1(CFB2DG030DAO fb2sb)
    {
        try
        {
            //取得現有資料
            DataTable tmp = fb2sb.getExistData();
            if (tmp.Rows.Count > 0)
            {
                BeginTransaction();
                fb2sb.addData_1_1();
                Commit();
                return "0";
            }
            else
            {
                BeginTransaction();
                fb2sb.addData_1_2();
                Commit();
                return "0";
            }
           
            

        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    public DataTable addData_2(string CAR_PARK)
    {
        CFB2DG030DAO dao = new CFB2DG030DAO();
        try
        {
            return dao.addData_2(CAR_PARK);
        }
        catch (Exception)
        {

            throw;
        }
        
    }
    public string addData_2_1(CFB2DG030DAO fb2dg2)
    {
        try
        {
            //取得現有資料

            BeginTransaction();
            fb2dg2.addData_2_1();
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
   
    public System.Data.DataTable getREMAINDER_PARKING_SPOT_1()
    {
        CFB2DG030DAO wfb2dg = new CFB2DG030DAO();
        try
        {
            return wfb2dg.getREMAINDER_PARKING_SPOT_1();
        }
        catch (Exception)
        {

            throw;
        }
    }
    public string re_Cal_REMainder(string car_park_no)
    {
        try
        {
            CFB2DG030DAO fb2dg = new CFB2DG030DAO();
            BeginTransaction();
            fb2dg.re_Cal_REMainder(car_park_no);
            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    public string REMAINDER_PARKING_SPOT_2(CFB2DG030DAO fb2dg)
    {
        try
        {
            BeginTransaction();
            fb2dg.REMAINDER_PARKING_SPOT_2();
            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    public string getREMAINDER_PARKING_SPOT_2(CFB2DG030DAO dao)
    {
        try
        {
            string st = "";
            DataTable dt = dao.getREMAINDER_PARKING_SPOT_2(dao.CAR_PARK_NO);
            if (dt.Rows.Count > 0)
            {
                st = dt.Rows[0]["REMAINDER_PARKING_SPOT"].ToString();
            }

            return st;
        }
        catch (Exception ex)
        {            
            return ex.Message;
        }
    }
    public string addData_3(CFB2DG030DAO fb2sb)
    {
        try
        {
            BeginTransaction();
            fb2sb.addData_3();
            Commit();
            return "0";

        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    public string CLOCK(CFB2DG030DAO fb2dg)
    {
        try
        {
            //取得現有資料
            
            BeginTransaction();
            fb2dg.CLOCK();
            Commit();
            return "0";

        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    public string delCLOCK(CFB2DG030DAO fb2dg)
    {
        try
        {
            //取得現有資料

            BeginTransaction();
            fb2dg.delCLOCK();
            Commit();
            return "0";

        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    public DataTable getREMAINDER_PARKING(string CAR_PARK_NO)
    {
        try
        {
            CFB2DG030DAO dao = new CFB2DG030DAO();
            return dao.getREMAINDER_PARKING_SPOT_2(CAR_PARK_NO);
        }
        catch (Exception)
        {

            throw;
        }
    }
}