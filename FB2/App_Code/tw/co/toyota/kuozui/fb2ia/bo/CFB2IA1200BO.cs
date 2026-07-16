using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;

/// <summary>
/// WFB2IA1200Service 的摘要描述
/// </summary>
public class CFB2IA1200BO : BaseService
{
    public CFB2IA1200BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    //產生修改資料
    public DataTable getEmpData(string emp_id)
    {
        try
        {
            CFB2IA1200DAO wfb2ia = new CFB2IA1200DAO();
            wfb2ia.EMP_ID = emp_id;
            return wfb2ia.getEmpData();
        }
        catch (Exception)
        {

            throw;
        }
    }

    //找員工的company_cd資料
    public string getCompany(string sdt, string emp_id)
    {
        try
        {
            CFB2IA1200DAO wfb2ia = new CFB2IA1200DAO();            
            return wfb2ia.getCompany(sdt, emp_id);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //產生異動類別資料
    public DataTable getCHG_APP_TYPE()
    {
        try
        {
            CFB2IA1200DAO wfb2ia = new CFB2IA1200DAO();
            return wfb2ia.getCHG_APP_TYPE();
        }
        catch (Exception)
        {

            throw;
        }
    }

    //產生退保原因別資料
    public DataTable getCHG_TYPE_OUT()
    {
        try
        {
            CFB2IA1200DAO wfb2ia = new CFB2IA1200DAO();
            return wfb2ia.getCHG_TYPE_OUT();
        }
        catch (Exception)
        {

            throw;
        }
    }

    //產生身份別資料
    public DataTable getIDENTITY_KIND()
    {
        try
        {
            CFB2IA1200DAO wfb2ia = new CFB2IA1200DAO();
            return wfb2ia.getIDENTITY_KIND();
        }
        catch (Exception)
        {

            throw;
        }
    }

    //新增TB_I_M_3IN1_TXN 勞保健保勞退履歷主檔 (勞保、健保、勞退、健保眷屬)
    public string add3IN1_TXN(CFB2IA1200DAO wfb2ia)
    {
        try
        {
            //取得現有資料
            DataTable tmp = wfb2ia.get3IN1_TXNData();

            if (tmp.Rows.Count > 0)
                return "資料已存在!";
            else
            {
                BeginTransaction();
                wfb2ia.add3IN1_TXN();
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

    //更新 TB_I_M_3IN1_TXN 勞保健保勞退履歷主檔 (勞保、健保、勞退、健保眷屬)
    public string update3IN1_TXN(CFB2IA1200DAO wfb2ia)
    {
        try
        {
            BeginTransaction();

            wfb2ia.update3IN1_TXN();

            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    //新增[TB_I_M_RETIRE_SELFRATE 勞退自提履歷檔 ](勞退自提率)  
    public string addRETIRE_SELFRATE(CFB2IA1200DAO wfb2ia)
    {
        try
        {
            //檢核系統日期住前推一年內,不可有二筆以上的勞退自提變更的資料
            //取得現有資料
            DataTable tmp = wfb2ia.getRETIRE_SELFRATEData();

            if (tmp.Rows.Count > 2)
                return "資料已存在!";
            else
            {
                BeginTransaction();
                wfb2ia.addRETIRE_SELFRATE();
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

    //更新 [TB_I_M_RETIRE_SELFRATE 勞退自提履歷檔 ](勞退自提率)  
    public string updateRETIRE_SELFRATE(CFB2IA1200DAO wfb2ia)
    {
        try
        {
            BeginTransaction();

            wfb2ia.updateRETIRE_SELFRATE();

            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    //新增[TB_I_M_PERSONDATA 保險資料主檔] [TB_I_R_DATAUPDAE_HIS 保險資料更新歷史檔]
    public string addPERSONDATA(CFB2IA1200DAO wfb2ia)
    {
        try
        {
            //取得現有資料
            DataTable tmp = wfb2ia.getPERSONDATA();

            if (tmp.Rows.Count > 0)
                return "0"; //資料已存在
            else
            {
                BeginTransaction();

                wfb2ia.addPERSONDATA();

                wfb2ia.addDATAUPDAE_HIS();
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

    //新增[TB_I_M_REDUCE_TXN 保險減免資料履歷檔 ](減免設定)	
    public string addREDUCE_TXN(CFB2IA1200DAO wfb2ia)
    {
        try
        {
            //取得現有資料
            DataTable tmp = wfb2ia.getREDUCE_TXNData();

            if (tmp.Rows.Count > 0)
                return "資料已存在!";
            else
            {
                BeginTransaction();
                wfb2ia.addREDUCE_TXN();
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

    //更新[TB_I_M_REDUCE_TXN 保險減免資料履歷檔 ]
    public string updateREDUCE_TXN(CFB2IA1200DAO wfb2ia)
    {
        try
        {
            BeginTransaction();

            wfb2ia.updateREDUCE_TXN();

            Commit();
            return "0";
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message;
        }
    }

    //新增[TB_I_M_FEES_TRACEBACK 保費追溯資料檔 ]
    public string addFEES_TRACEBACK(CFB2IA1200DAO wfb2ia)
    {
        try
        {
            //取得現有資料
            DataTable tmp = wfb2ia.getFEES_TRACEBACKData();

            if (tmp.Rows.Count > 0)
                return "資料已存在!";
            else
            {
                BeginTransaction();
                wfb2ia.addFEES_TRACEBACK();
                //20150601 更新追溯處理否 = Y
                if (wfb2ia.fid != "FB2IA120" )
                {
                    wfb2ia.update_BILLS_COMPARE();
                }

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

    //取得核定人員
    public string getAPPROVE_BY(CFB2IA1200DAO wfb2ia)
    {
        try
        {
            string APPROVE_BY = "";

            DataTable tmp = wfb2ia.getAPPROVE_BY();
            if (tmp.Rows.Count > 0)
            {
                APPROVE_BY = tmp.Rows[0].ItemArray[0].ToString();
            }

            return APPROVE_BY;

        }
        catch (Exception)
        {

            throw;
        }
    }

    //取得眷屬出生日期(健保眷屬)
    public string getFAMILY_BIRTH_DT(CFB2IA1200DAO wfb2ia)
    {
        try
        {
            string FAMILY_BIRTH_DT = "";

            DataTable tmp = wfb2ia.getFAMILY_BIRTH_DT();
            if (tmp.Rows.Count > 0)
            {
                FAMILY_BIRTH_DT = tmp.Rows[0].ItemArray[0].ToString();
            }

            return FAMILY_BIRTH_DT;

        }
        catch (Exception)
        {

            throw;
        }
    }

    //刪除 TB_I_M_3IN1_TXN 勞保健保勞退履歷主檔 (勞保、健保、勞退、健保眷屬)
    //public string delete3IN1_TXN(CFB2IA1200DAO wfb2ia)
    //{
    //    try
    //    {
    //        BeginTransaction();

    //        wfb2ia.delete3IN1_TXN();

    //        Commit();

    //        return "0";
    //    }
    //    catch (Exception ex)
    //    {
    //        RollBack();
    //        return ex.Message;
    //    }
    //}

    //刪除 TB_I_M_3IN1_TXN 勞保健保勞退履歷主檔 (勞保、健保、勞退、健保眷屬)
    public string delete3IN1_TXN(List<Tuple<string, string, string, string, string>> ins_type)
    {
        try
        {
            CFB2IA1200DAO wfb2ia = new CFB2IA1200DAO();
            BeginTransaction();
            foreach (var item in ins_type)
            {
                wfb2ia.delete3IN1_TXN(item.Item1, item.Item2, item.Item3, item.Item4, item.Item5);
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

    //刪除 [TB_I_M_RETIRE_SELFRATE 勞退自提履歷檔 ](勞退自提率) 
    //public string deleteRETIRE_SELFRATE(CFB2IA1200DAO wfb2ia)
    //{
    //    try
    //    {
    //        BeginTransaction();

    //        wfb2ia.deleteRETIRE_SELFRATE();

    //        Commit();

    //        return "0";
    //    }
    //    catch (Exception ex)
    //    {
    //        RollBack();
    //        return ex.Message;
    //    }
    //}

    //刪除 [TB_I_M_RETIRE_SELFRATE 勞退自提履歷檔 ](勞退自提率) 
    public string deleteRETIRE_SELFRATE(List<Tuple<string, string>> emp_id)
    {
        try
        {
            CFB2IA1200DAO wfb2ia = new CFB2IA1200DAO();
            BeginTransaction();
            foreach (var item in emp_id)
            {
                wfb2ia.deleteRETIRE_SELFRATE(item.Item1, item.Item2);
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

    //刪除[TB_I_M_REDUCE_TXN 保險減免資料履歷檔](減免設定)	
    //public string deleteREDUCE_TXN(CFB2IA1200DAO wfb2ia)
    //{
    //    try
    //    {
    //        BeginTransaction();

    //        wfb2ia.deleteREDUCE_TXN();

    //        Commit();

    //        return "0";
    //    }
    //    catch (Exception ex)
    //    {
    //        RollBack();
    //        return ex.Message;
    //    }
    //}

    //刪除[TB_I_M_REDUCE_TXN 保險減免資料履歷檔](減免設定)
    public string deleteREDUCE_TXN(List<Tuple<string, string, string, string, string>> emp_id)
    {
        try
        {
            CFB2IA1200DAO wfb2ia = new CFB2IA1200DAO();
            BeginTransaction();
            foreach (var item in emp_id)
            {
                wfb2ia.deleteREDUCE_TXN(item.Item1, item.Item2, item.Item3, item.Item4, item.Item5);
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

    public List<string> get3IN1_TXN(CFB2IA1200DAO wfb2ia)
    {
        try
        {
            List<string> amt = new List<string>();

            DataTable tmp = wfb2ia.get3IN1_TXN();
            if (tmp.Rows.Count > 0)
            {
                amt.Add(tmp.Rows[0]["SALARY_AMT"].ToString());
                amt.Add(tmp.Rows[0]["INS_AMT"].ToString());
            }

            return amt;

        }
        catch (Exception)
        {

            throw;
        }
    }

    public bool checkINS_AMT(string ins_type, string ins_amt, string salary_amt)
    {
        try
        {
            CFB2IA1200DAO wfb2ia = new CFB2IA1200DAO();
            DataTable tmp = wfb2ia.checkINS_AMT(ins_type, ins_amt, salary_amt);
            if (tmp.Rows.Count > 0)
            {
                return false;
            }
            return true;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public bool check3IN1_TXN(string ins_type, string identity_kind, string emp_id, string license_id, string effect_sdt, string effect_edt)
    {
        try
        {
            CFB2IA1200DAO wfb2ia = new CFB2IA1200DAO();
            DataTable tmp = wfb2ia.check3IN1_TXN(ins_type, identity_kind, emp_id, license_id, effect_sdt, effect_edt);
            if (tmp.Rows.Count > 0)
            {
                return true;
            }
            return false;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public bool checkRETIRE_SELFRATE(string emp_id, string effect_sdt, string effect_edt)
    {
        try
        {
            CFB2IA1200DAO wfb2ia = new CFB2IA1200DAO();
            DataTable tmp = wfb2ia.checkRETIRE_SELFRATE(emp_id, effect_sdt, effect_edt);
            if (tmp.Rows.Count > 0)
            {
                return true;
            }
            return false;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public bool checkREDUCE_TXN(string emp_id, string identity_kind, string license_id, string effect_sdt, string effect_edt,string reduce_cd)
    {
        try
        {
            CFB2IA1200DAO wfb2ia = new CFB2IA1200DAO();
            DataTable tmp = wfb2ia.checkREDUCE_TXN(emp_id, identity_kind, license_id, effect_sdt, effect_edt, reduce_cd);
            if (tmp.Rows.Count > 0)
            {
                return true;
            }
            return false;
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getEmpName(string emp_id)
    {
        try
        {
            CFB2IA1200DAO wfb2ia = new CFB2IA1200DAO();
            return wfb2ia.getEmpName(emp_id);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getCOMPANY_SNAME(string company_cd)
    {
        try
        {
            CFB2IA1200DAO wfb2ia = new CFB2IA1200DAO();
            return wfb2ia.getCOMPANY_SNAME(company_cd);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //退保原因說明別1(修改)
    public DataTable getCHG_REASON_NAME(string chg_reason_cd, string chg_type_out)
    {
        try
        {
            CFB2IA1200DAO wfb2ia = new CFB2IA1200DAO();
            return wfb2ia.getCHG_REASON_NAME(chg_reason_cd, chg_type_out);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //身分證號4
    public DataTable getLICENSE_ID(string emp_id, string license_id)
    {
        try
        {
            CFB2IA1200DAO wfb2ia = new CFB2IA1200DAO();
            return wfb2ia.getLICENSE_ID(emp_id, license_id);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //身分證號5
    public DataTable getLICENSE_ID1(string emp_id, string license_id, string identity_kind)
    {
        try
        {
            CFB2IA1200DAO wfb2ia = new CFB2IA1200DAO();
            return wfb2ia.getLICENSE_ID1(emp_id, license_id, identity_kind);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //減免代碼5
    public DataTable getREDUCE_DESC(string reduce_cd)
    {
        try
        {
            CFB2IA1200DAO wfb2ia = new CFB2IA1200DAO();
            return wfb2ia.getREDUCE_DESC(reduce_cd);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //加保原因說明別4(修改)(健保眷屬)
    public DataTable getCHG_TYPE_IN_NAME(string chg_type_in)
    {
        try
        {
            CFB2IA1200DAO wfb2ia = new CFB2IA1200DAO();
            return wfb2ia.getCHG_TYPE_IN_NAME(chg_type_in);
        }
        catch (Exception)
        {

            throw;
        }
    }
}