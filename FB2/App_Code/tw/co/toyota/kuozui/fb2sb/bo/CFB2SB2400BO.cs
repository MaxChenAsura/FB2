using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Collections;
/// <summary>
/// CFB2SB2400BO 的摘要描述
/// </summary>
public class CFB2SB2400BO : BaseService
{
	public CFB2SB2400BO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    public System.Data.DataTable getSALARY_ITEM (string salary_id)
    {
        CFB2SB2400DAO wfb2sb = new CFB2SB2400DAO();
        try
        {
            return wfb2sb.getSALARY_ITEM( salary_id);
        }
        catch (Exception)
        {

            throw;
        }
    }
    public System.Data.DataTable get_SEQ_NO(string EMP_ID, string DATA_YM, string SALARY_ID)
    {
        CFB2SB2400DAO wfb2sb = new CFB2SB2400DAO();
        try
        {
            return wfb2sb.get_SEQ_NO(EMP_ID, DATA_YM, SALARY_ID);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public int getCHG_AMT_A(string txt_SALARY_ID, string txt_DATA_YM, string ddl_EMP_CD, string txt_EMP_ID, string txt_EMP_NAME)
    {
        CFB2SB2400DAO wfb2sb = new CFB2SB2400DAO();
        try
        {
            return wfb2sb.getCHG_AMT_A(txt_SALARY_ID,txt_DATA_YM,ddl_EMP_CD,txt_EMP_ID,txt_EMP_NAME);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getSALARYFile(string SALARY_ID)
    {
        try
        {
            CFB2SB2400DAO wfb2sb = new CFB2SB2400DAO();
            wfb2sb.SALARY_ID = SALARY_ID;
            return wfb2sb.getSALARYFile();

        }
        catch (Exception)
        {

            throw;
        }
    }

    public string addData_11(CFB2SB2400DAO fb2sb)
    {
        try
        {
            //取得現有資料
            //DataTable tmp = fb2sb.getExistData();
            //if (tmp.Rows.Count > 0)
            //{
            //    return "資料重覆!";
            //}
            BeginTransaction();
            fb2sb.addData_11();
            Commit();
            return "0";

        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    public string doReject(CFB2SB2400DAO dao, ArrayList datas)
    {
        try
        {
            DataTable dt = new DataTable();
            dao.FUNC_ID = "FB2SB240";
            dao.CREATED_BY = SessionHandle.Current.emp_id.Trim();
            dao.UPDATED_BY = SessionHandle.Current.emp_id.Trim();
            BeginTransaction();

            foreach (string[] item in datas)
            {
                dao.CHG_STATUS = item[0];
                dao.EMP_ID = item[1];
                dao.EMP_NAME = item[2].Trim();
                dao.SALARY_ID = item[3];
                dao.DATA_YM = item[4];
                dao.STATUS_MARK = item[5];
                dao.HID_SEQ_NO = item[6];
                dao.HID_SEQ_NO_B = item[7];
                dao.REPAY_DT = item[8];
                dao.REPAY_TYPE = item[9];
                dao.CHG_AMT_A = item[10] == "" ? "0" : item[10];
                dao.REMARK = item[11];
                dao.APP_REMARK = item[12];
                                
                dt = getSALARY_ITEM(dao.SALARY_ID);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dao.IS_PLUS = dt.Rows[0]["IS_PLUS"].ToString();//加減項
                        dao.IS_TAX = dt.Rows[0]["IS_TAX"].ToString();//加減項
                    }
                }

                switch (dao.STATUS_MARK)
                {
                    case "1":
                        dao.updateData_reject();                        
                        break;
                    case "2":
                        dao.updateData_reject2();                        
                        break;
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

    public string doApprove(CFB2SB2400DAO dao, ArrayList datas)
    {
        try
        {
            DataTable dt = new DataTable();
            dao.FUNC_ID = "FB2SB240";

            BeginTransaction();

            foreach (string[] item in datas)
            {
                dao.CHG_STATUS = item[0];
                dao.EMP_ID = item[1];
                dao.EMP_NAME = item[2].Trim();
                dao.SALARY_ID = item[3];
                dao.DATA_YM = item[4];
                dao.STATUS_MARK = item[5];
                dao.HID_SEQ_NO = item[6];
                dao.HID_SEQ_NO_B = item[7];
                dao.REPAY_DT = item[8];
                dao.REPAY_TYPE = item[9];
                dao.CHG_AMT_A = item[10] == "" ? "0" : item[10];
                dao.REMARK = item[11];
                dao.APP_REMARK = item[12];
				dao.REPAY_SUB_ID = item[13];
               
                dt = getSALARY_ITEM(dao.SALARY_ID);
                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dao.IS_PLUS = dt.Rows[0]["IS_PLUS"].ToString();//加減項
                        dao.IS_TAX = dt.Rows[0]["IS_TAX"].ToString();//加減項
                    }
                }

                switch (dao.STATUS_MARK)
                {
                    case "1":
                        switch (dao.CHG_STATUS)
                        {
                            case "N":
                                dt = get_SEQ_NO(dao.EMP_ID, dao.DATA_YM, dao.SALARY_ID);
                                if (dt.Rows.Count > 0)
                                {
                                    dao.SEQ_NO = Convert.ToString(Convert.ToDecimal(dt.Rows[0]["SEQ_NO"]) + 1);
                                }
                                else
                                {
                                    dao.SEQ_NO = "1";
                                }
                                dao.addData_11();
                              
                                break;
                            case "U":
                                dao.updateData_21();
                              
                                break;
                            case "D":
                                dao.deleteData_31();
                                dao.updateData_32();
                                
                                break;
                        }

                        break;
                    case "2":
                        dt = dao.get_SEQ_NO(dao.EMP_ID, dao.DATA_YM, dao.SALARY_ID);
                        if (dt.Rows.Count > 0)
                        {
                            dao.SEQ_NO = Convert.ToString(Convert.ToDecimal(dt.Rows[0]["SEQ_NO"]) + 1);
                        }
                        else
                        {
                            dao.SEQ_NO = "1";
                        }
                        dao.addData_41();
                       
                        break;
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

    public string updateData_21(CFB2SB2400DAO fb2sb)
    {
        try
        {
            BeginTransaction();
            fb2sb.updateData_21();
            Commit();
            return "0";

        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    public string deleteData_31(CFB2SB2400DAO fb2sb)
    {
        try
        {
            BeginTransaction();
            fb2sb.deleteData_31();
            fb2sb.updateData_32();
            Commit();
            return "0";
        }
        catch (Exception)
        {
            RollBack();
            throw;
        }
    }
    //public string updateData_32(CFB2SB2400DAO fb2sb)
    //{
    //    try
    //    {
    //        BeginTransaction();
    //        fb2sb.updateData_32();
    //        Commit();
    //        return "0";

    //    }
    //    catch (Exception ex)
    //    {
    //        RollBack();
    //        return ex.Message;
    //    }
    //}
    public string addData_41(CFB2SB2400DAO fb2sb)
    {
        try
        {
            //取得現有資料
            //DataTable tmp = fb2sb.getExistData();
            //if (tmp.Rows.Count > 0)
            //{
            //    return "資料重覆!";
            //}
            BeginTransaction();
            fb2sb.addData_41();
            Commit();
            return "0";

        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    public string updateData_reject(CFB2SB2400DAO fb2sb)
    {
        try
        {
            BeginTransaction();
            fb2sb.updateData_reject();
            Commit();
            return "0";

        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
    public string updateData_reject2(CFB2SB2400DAO fb2sb)
    {
        try
        {
            BeginTransaction();
            fb2sb.updateData_reject2();
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
