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
/// CFB2DG010BO 的摘要描述
/// </summary>
public class CFB2DG010BO : BaseService
{
	public CFB2DG010BO()
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
    //        CFB2DG010DAO wfb2ib = new CFB2DG010DAO();
    //        wfb2ib.SYS_CD = sys_cd;
    //        return wfb2ib.getData();
    //    }
    //    catch (Exception)
    //    {
    //        throw;
    //    }
    //}
    public string re_Cal_REMainder(string car_park_no)
    {
        try
        {
            CFB2DG010DAO fb2dg = new CFB2DG010DAO();
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

    public System.Data.DataTable getREMAINDER_PARKING_SPOT_1()
    {
        CFB2DG010DAO wfb2dg = new CFB2DG010DAO();
        try
        {
            return wfb2dg.getREMAINDER_PARKING_SPOT_1();
        }
        catch (Exception)
        {

            throw;
        }
    }
    public System.Data.DataTable getSYS_ID(CFB2DG010DAO wfb2dg)
    {
        try
        {
            return wfb2dg.getSYS_ID();
        }
        catch (Exception)
        {

            throw;
        }
    }
    public System.Data.DataTable getPARKING_TYPE()
    {
        CFB2DG010DAO wfb2dg = new CFB2DG010DAO();
        try
        {
            return wfb2dg.getPARKING_TYPE();
        }
        catch (Exception)
        {

            throw;
        }
    }

    public System.Data.DataTable getSYS_ID(string SUB_CD)
    {
        CFB2DG010DAO wfb2ib = new CFB2DG010DAO();
        try
        {
            return wfb2ib.getSYS_ID(SUB_CD);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public System.Data.DataTable getModeData(string ID)
    {
        CFB2DG010DAO wfb2ib = new CFB2DG010DAO();
        try
        {
            return wfb2ib.getModeData(ID);
        }
        catch (Exception)
        {

            throw;
        }
    }


    public System.Data.DataTable getFUNC_ID(string ID)
    {
        CFB2DG010DAO wfb2ib = new CFB2DG010DAO();
        try
        {
            return wfb2ib.getFUNC_ID(ID);
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
            CFB2DG010DAO wfb2ib = new CFB2DG010DAO();
            BeginTransaction();

            foreach (string deleteitem in deleteList)
            {
                //刪除主檔資料
                wfb2ib.deleteData(deleteitem);
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
    public string updateData(CFB2DG010DAO fb2ib)
    {
        try
        {
            BeginTransaction();
            fb2ib.updateData();
            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    public string REMAINDER_PARKING_SPOT_2(CFB2DG010DAO fb2dg)
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

    public string getUSING_PARKING_SPOT(string CAR_PARK_NO)
    {
        try
        {
            string USING_PARKING_SPOT = string.Empty;
            CFB2DG010DAO fb2dg = new CFB2DG010DAO();
            USING_PARKING_SPOT = Convert.ToString(fb2dg.getUSING_PARKING_SPOT(CAR_PARK_NO));
            return USING_PARKING_SPOT;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public string getREMAINDER_PARKING_SPOT(string CAR_PARK_NO, string PARKING_SPOT, string OVERLAP)
    {
        string REMAINDER_PARKING_SPOT = "";//剩餘數
        string wk1 = "";//常日
        string wkR = "";//紅直
        string wkY = "";//黃直       
        string rate = "";//停車場出勤率設定值

        try
        {            
            CFB2DG010DAO dao = new CFB2DG010DAO();          
            wk1 = dao.getTotalShift(CAR_PARK_NO,"1");            
            wkR = dao.getTotalShift(CAR_PARK_NO, "R");            
            wkY = dao.getTotalShift(CAR_PARK_NO, "Y");
            rate = dao.getSetRate();
            //停車場主檔.車位數 －( MAX( WK紅直, WK黃直 ) ＊ ( 1＋停車場主檔.重疊率 / 100 ) ＋WK常日班  ) ＊ WK停車場出勤率設定值  / 100
            int maxShift = 0;
            if (Convert.ToInt32(wkR) - Convert.ToInt32(wkY) >= 0)
            {
                maxShift = Convert.ToInt32(wkR);
            }else
	        {
                maxShift = Convert.ToInt32(wkY);
	        }

            int kk = Convert.ToInt32(PARKING_SPOT) - (maxShift * (1 + (Convert.ToInt32(OVERLAP) / 100) + Convert.ToInt32(wk1))) * (Convert.ToInt32(rate) / 100);
            REMAINDER_PARKING_SPOT = Convert.ToString(kk);

            return REMAINDER_PARKING_SPOT;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    public string addData(CFB2DG010DAO fb2ib)
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
}