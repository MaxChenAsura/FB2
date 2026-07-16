using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.Data.SqlClient;

/// <summary>
/// CFB2SF1300BO 的摘要描述
/// </summary>
public class CFB2SF1300BO : BaseService
{
    public CFB2SF1300BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    # region Qry


    public System.Data.DataTable selectDCCC83M(CFB2SF1300DAO dao)
    {
        try
        {
            return dao.selectDCCC83M();
        }
        catch (Exception)
        {

            throw;
        }
    }

    //public System.Data.DataTable getAS400(CFB2SF1300DAO fb2sf)
    //{
    //    try
    //    {
    //        return fb2sf.getAS400();
    //    }
    //    catch (Exception)
    //    {

    //        throw;
    //    }
    //}

    public System.Data.DataTable selectDTL(CFB2SF1300DAO fb2sf)
    {
        try
        {
            return fb2sf.selectDTL();
        }
        catch (Exception)
        {

            throw;
        }
    }

    public System.Data.DataTable getSALARY_TYPE()
    {
        CFB2SF1300DAO wfb2sf = new CFB2SF1300DAO();
        try
        {
            return wfb2sf.getSALARY_TYPE();
        }
        catch (Exception)
        {

            throw;
        }
    }
    public System.Data.DataTable gettmpNO()
    {
        CFB2SF1300DAO wfb2sf = new CFB2SF1300DAO();
        try
        {
            return wfb2sf.gettmpNO();
        }
        catch (Exception)
        {

            throw;
        }
    }

    public System.Data.DataTable getSORT(string EMP_ID_LIST)
    {
        CFB2SF1300DAO wfb2sf = new CFB2SF1300DAO();
        try
        {
            return wfb2sf.getSORT(EMP_ID_LIST);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getTOTAL_AMT(CFB2SF1300DAO fb2sf)
    {
        DataTable retVal = new DataTable(); ;
        try
        {
            retVal = fb2sf.getTOTAL_AMT();
            return retVal;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable getTOTAL_AMT1(CFB2SF1300DAO fb2sf)
    {
        DataTable retVal = new DataTable(); ;
        try
        {
            retVal = fb2sf.getTOTAL_AMT1();
            return retVal;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable get_PDF_Data(string ACCT_ID, string Lno)
    {
        DataTable retVal = new DataTable(); ;
        CFB2SF1300DAO fb2sf = new CFB2SF1300DAO();
        try
        {
            retVal = fb2sf.get_PDF_Data(ACCT_ID, Lno);
            return retVal;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable getARREARS_COURT_D(string SALARY_DT, string SALARY_TYPE)
    {
        DataTable dt = new DataTable(); ;
        CFB2SF1300DAO fb2sf = new CFB2SF1300DAO();
        try
        {
            dt = fb2sf.getARREARS_COURT_D(SALARY_DT, SALARY_TYPE);
            return dt;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable getPAYMONEY_TYPE()
    {
        DataTable dt = new DataTable(); ;
        CFB2SF1300DAO fb2sf = new CFB2SF1300DAO();
        try
        {
            dt = fb2sf.getPAYMONEY_TYPE();
            return dt;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public System.Data.DataTable getSYS_ID(string SUB_CD)
    {
        CFB2SF1300DAO wfb2ib = new CFB2SF1300DAO();
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
        CFB2SF1300DAO wfb2ib = new CFB2SF1300DAO();
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
        CFB2SF1300DAO wfb2ib = new CFB2SF1300DAO();
        try
        {
            return wfb2ib.getFUNC_ID(ID);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public string deleteAS400(CFB2SF1300DAO wfb2sf)
    {
        try
        {
            BeginTransaction();

            wfb2sf.deleteAS400();
            Commit();
            return "0";
        }
        catch (Exception)
        {
            RollBack();
            throw;
        }
    }


    public string updateTB_S_M_ARREARS_TARGET(CFB2SF1300DAO fb2sf)
    {
        try
        {
            BeginTransaction();
            fb2sf.updateTB_S_M_ARREARS_TARGET();
            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    public string updateTB_S_M_ALLOCATION_D(CFB2SF1300DAO fb2sf)
    {
        try
        {
            BeginTransaction();
            fb2sf.updateTB_S_M_ALLOCATION_D();
            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    public string updateTB_9_M_PARAMETER(CFB2SF1300DAO fb2sf)
    {
        try
        {
            BeginTransaction();
            fb2sf.updateTB_9_M_PARAMETER();
            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    public string DeleteData_1(CFB2SF1300DAO fb2sf)
    {
        try
        {
            DataTable tmp = fb2sf.getExistData();
            if (tmp.Rows.Count == 0)
            {
                return "資料不存在!";
            }
            BeginTransaction();
            fb2sf.DeleteData_1();
            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    public string updateData_1(CFB2SF1300DAO fb2sf)
    {
        try
        {

            BeginTransaction();
            fb2sf.updateData_1();
            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    public string addAs400(CFB2SF1300DAO fb2sf)
    {
        try
        {
            BeginTransaction();
            fb2sf.addAs400();
            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    public string getLno(CFB2SF1300DAO dao)
    {
        try
        {
            return dao.getLnoPara();
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    #endregion
    //結轉傳票
    public string transferToACC(CFB2SF1300DAO dao, DataTable dt)
    {
        string PAYMONEY_TYPE_tmp = "", tmpNO = "", msg = "";
        int b = 0;
        DataTable dt_temp = new DataTable();
        string temp = "";
        int fno = 1;
        string Wtmenid = "";
        string WtmenName = "";
        string sDp = "";
        string sa_dt = "";
        try
        {
            dao.CREATED_BY = SessionHandle.Current.emp_id.Trim();
            dao.UPDATED_BY = SessionHandle.Current.emp_id.Trim();
            dao.FUNC_ID = "FB2SF130";

            BeginTransaction();

            //刪除暫存檔
            dao.delTempTable();

            //存到暫存檔
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dao.DOC_NO = dt.Rows[i]["DOC_NO"].ToString();
                dao.EMP_ID = dt.Rows[i]["EMP_ID"].ToString();
                dao.EMP_NAME = dt.Rows[i]["EMP_NAME"].ToString();
                dao.AMOUNT = dt.Rows[i]["AMOUNT"].ToString();
                dao.SALARY_NAME = dt.Rows[i]["SALARY_NAME"].ToString();
                dao.VENDOR_ID = dt.Rows[i]["VENDOR_ID"].ToString();
                dao.HOPE_PAT_DT = dt.Rows[i]["HOPE_PAT_DT"].ToString();
                dao.S_DT = dt.Rows[i]["S_DT"].ToString();
                dao.E_DT = dt.Rows[i]["E_DT"].ToString();
                dao.PAYMONEY_TYPE = dt.Rows[i]["PAYMONEY_TYPE"].ToString();
                dao.PAYMONEY_NAME = dt.Rows[i]["PAYMONEY_NAME"].ToString();
                dao.DEPT_ACCT_ID = dt.Rows[i]["DEPT_ACCT_ID"].ToString();
                dao.ACCT_ID = dt.Rows[i]["ACCT_ID"].ToString();
                dao.SEQ = dt.Rows[i]["SEQ"].ToString();
                dao.SALARY_DT = Convert.ToString(Convert.ToDateTime(dt.Rows[i]["SALARY_DT"].ToString()).ToString("yyyy/MM/dd"));
                dao.SALARY_TYPE = dt.Rows[i]["SALARY_TYPE"].ToString();
                dao.PAY_KIND = dt.Rows[i]["PAY_KIND"].ToString();
                dao.PAY_TARGET = dt.Rows[i]["PAY_TARGET"].ToString();
                sa_dt = dao.SALARY_DT;
                dao.insertTempTable();
            }

            Commit();

            if (dt.Rows.Count > 0)
            {
                //call SP
                SqlParameterCollection param = dao.SP_S_SF130_TO_SAP(dao.IaDat, SessionHandle.Current.emp_id);
                if (!string.IsNullOrEmpty(param["@P_ERR_MSG"].Value.ToString()))
                {
                    return param["@P_ERR_MSG"].Value.ToString();
                }
                else
                {
                    if (!string.IsNullOrEmpty(param["@P_LNO"].Value.ToString()))
                    {
                        dao.Lno = param["@P_LNO"].Value.ToString();
                    }

                    //取得暫存檔資料
                    BeginTransaction();

                    dt_temp = dao.selectTempTable();

                    #region 依畫面勾選項目 更新法扣資料
                    for (int i = 0; i < dt_temp.Rows.Count; i++)
                    {
                        dao.IS_OVER = "N";
                        dao.IS_VAILD = "Y";

                        fno = 1;
                        //貸方
                        dao.DOC_NO = dt_temp.Rows[i]["DOC_NO"].ToString();
                        dao.EMP_ID = dt_temp.Rows[i]["EMP_ID"].ToString();
                        dao.EMP_NAME = dt_temp.Rows[i]["EMP_NAME"].ToString();
                        dao.AMOUNT = dt_temp.Rows[i]["AMOUNT"].ToString();
                        dao.SALARY_NAME = dt_temp.Rows[i]["SALARY_NAME"].ToString();
                        dao.VENDOR_ID = dt_temp.Rows[i]["VENDOR_ID"].ToString();
                        dao.HOPE_PAT_DT = dt_temp.Rows[i]["HOPE_PAT_DT"].ToString();
                        dao.S_DT = dt_temp.Rows[i]["S_DT"].ToString();
                        dao.E_DT = dt_temp.Rows[i]["E_DT"].ToString();
                        dao.PAYMONEY_TYPE = dt_temp.Rows[i]["PAYMONEY_TYPE"].ToString();
                        dao.PAYMONEY_NAME = dt_temp.Rows[i]["PAYMONEY_NAME"].ToString();
                        dao.DEPT_ACCT_ID = dt_temp.Rows[i]["DEPT_ACCT_ID"].ToString();
                        dao.ACCT_ID = dt_temp.Rows[i]["ACCT_ID"].ToString();
                        dao.SEQ = dt_temp.Rows[i]["SEQ"].ToString();
                        dao.SALARY_DT = dt_temp.Rows[i]["SALARY_DT"].ToString();
                        dao.SALARY_TYPE = dt_temp.Rows[i]["SALARY_TYPE"].ToString();
                        dao.P_KIND = dt_temp.Rows[i]["PAY_KIND"].ToString();
                        dao.PAY_TARGET = dt_temp.Rows[i]["PAY_TARGET"].ToString();
                        dao.IACYC = (dao.IaDat).Substring(0, 7);

                        //取得傳票號碼.批號 
                        dao.Vochno = dt_temp.Rows[i]["HR_NO"].ToString();

                        fno = fno + 1;
                        dao.SEQ_NO2 = Convert.ToString(Convert.ToInt32(dao.SEQ_NO2) + 1);

                        ///更新法扣主檔
                        DataTable dt1 = dao.getTOTAL_AMT();
                        if (dt1.Rows.Count > 0)
                        {
                            //已扣款金額+ ( if勾選資料.支付對象<>'E.本人'  then 勾選資料.法扣金額 else 0)
                            if (dao.PAY_TARGET == "E")
                            {
                                dao.TOTAL_AMT = Convert.ToString(Convert.ToInt32(dt1.Rows[0]["TOTAL_AMT"].ToString()));
                            }
                            else
                            {
                                dao.TOTAL_AMT = Convert.ToString(Convert.ToInt32(dt1.Rows[0]["TOTAL_AMT"].ToString()) + Convert.ToInt32(dt_temp.Rows[i]["AMOUNT"].ToString()));
                            }
                        }
                        else
                        {
                            dao.TOTAL_AMT = "0";
                        }

                        //更新 TB_S_M_ALLOCATION_D法扣分配明細檔
                        dao.updateTB_S_M_ALLOCATION_D();

                        dt1.Clear();

                        //至 TB_S_M_ARREARS_TARGET法扣分配對象檔 取得金額
                        dt1 = dao.getTOTAL_AMT1();
                        dao.EFFECT_EDT = "null";
                        if (dt1.Rows.Count > 0)
                        {
                            string s1 = dt1.Rows[0]["TOTAL_AMT"].ToString();//償還金額
                            string s2 = dt_temp.Rows[i]["AMOUNT"].ToString();//法扣金額

                            //if 支付對象='A.政府' AND (債權金額-償還金額) =0 THEN 系統日 end
                            //償還金額
                            dao.TOTAL_AMT = Convert.ToString(Convert.ToInt32(dt1.Rows[0]["TOTAL_AMT"].ToString()) + Convert.ToInt32(dt_temp.Rows[i]["AMOUNT"].ToString()));
                            if (dt1.Rows[0]["PAY_TARGET"].ToString() == "A" && (Convert.ToInt32(dt1.Rows[0]["AMOUNT"].ToString()) - Convert.ToInt32(dao.TOTAL_AMT)) == 0)
                            {
                                dao.EFFECT_EDT = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                                dao.IS_VAILD = "N";
                                dao.IS_OVER = "Y";
                            }
                        }

                        //更新 TB_S_M_ARREARS_TARGET法扣分配對象檔 償還金額TOTAL_AMT, 結束日EFFECT_EDT
                        dao.updateTB_S_M_ARREARS_TARGET();
                    }
                    #endregion 更新法扣資料
                    Commit();
                }
            }

            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    public string getLogFlag(string Lno, string TblId)
    {
        string errormessage = "";
        try
        {
            CFB2SF1300DAO dao = new CFB2SF1300DAO();
            DataTable dt = dao.getLogFlag(Lno, TblId);
            if (dt.Rows.Count > 0)
            {
                string GCM = dt.Rows[0]["GetChveMrtMk"].ToString();//抓入成功註記
                string AWM = dt.Rows[0]["AvWgtcmpsMk"].ToString();//可重作註記
                if (GCM == "Y" && AWM != "Y")
                {
                    //errormessage += "傳票已進入財務系統，不能再重新計算\\n";
                    errormessage += "N";
                }
            }

            return errormessage;

        }
        catch (Exception)
        {

            throw;
        }
    }

    public string execDel(CFB2SF1300DAO dao)
    {
        try
        {
            //法扣代號7777
            DataTable para_dt = utilities.getParameter("SF", "SFID");
            if (para_dt.Rows.Count > 0)
            {
                dao.PAY_KIND = para_dt.Rows[0]["CODE_VAL1"].ToString();
            }
            //薪資發放資料別
            dao.getSys_cd();
            dao.SlyPrvdDtid = dao.SYS_CD;

            BeginTransaction();

            //ALLOCATION_D
            DataTable dt = dao.getALLOCATION_D();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    dao.EMP_ID = dt.Rows[i]["EMP_ID"].ToString();
                    dao.DOC_NO = dt.Rows[i]["DOC_NO"].ToString();
                    dao.SEQ = dt.Rows[i]["SEQ"].ToString();
                    dao.AMOUNT = dt.Rows[i]["AMOUNT"].ToString();
                    dao.PAYMONEY_TYPE = dt.Rows[i]["PAYMONEY_TYPE"].ToString();
                    dao.TOTAL_AMT = dt.Rows[i]["AMOUNT"].ToString();

                    dao.PAY_TARGET = dao.selectTarget();

                    //*異動[TB_S_M_ARREARS_COURT_H 法扣主檔]  已扣款金額=已扣款金額-台幣金額
                    dao.update_COURT_H();
                    //*異動[TB_S_M_ARREARS_TARGET 法扣分配對象檔]  償還金額=償還金額-台幣金額
                    dao.update_TARGET();
                    //*異動[TB_S_M_ALLOCATION_D 月份法扣分配明細檔]  部門傳票號碼清空
                    dao.update_ALLOCATION_D();
                    // *刪除 AS400系統 [DCCC28WH] 支付傳票轉入作業暫存檔(FOR次世代人事－法扣)
                    //dao.deleteDCCC83M();
                    // *刪除 暫存傳票檔
                    dao.delete_VOUCHER();


                }
            }
            Commit();

            dao.Lno = dao.TMP_LNO;
            //將資料寫到FF1
            dao.RunSP_I_FF1_VOUCHER();

            //拿FF1 log
            DataTable dt_sp = dao.checkSP();
            if (dt_sp.Rows.Count > 0)
            {
                if (dt_sp.Rows[0]["ERROR_FLAG"].ToString() != "")
                {
                    return dt_sp.Rows[0]["LOG_CONTENT"].ToString();
                }
            }
            else
            {
                return "傳票轉出失敗!!沒有傳送資料到目標資料庫中";
            }


            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }
}