using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.bo;



/// <summary>
/// CFB2SC2600BO 的摘要描述
/// </summary>
public class CFB2SC2600BO : BaseService
{
    public CFB2SC2600BO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public static string wrongMsg = "";

    //檢查資料是否已轉傳票
    public string CheckVoucher(string pa_pay_id)
    {
        string rtnmessage = "0";
        try
        {
            CFB2SC2600DAO fb2sc = new CFB2SC2600DAO();
            DataTable dt = fb2sc.getS_M_VOUCHER(pa_pay_id);
            if ((int)dt.Rows[0]["resultCount"] == 0)
            {
                rtnmessage = "該項目尚未生成正式會計傳票,無法執行月結作業!! \\n";
            }
            dt.Clear();
            return rtnmessage;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    //檢查資料是否已轉傳票
    public string check_VOUCHER(string PAY_ID)
    {
        string rtnmessage = "0";
        try
        {
            CFB2SC2600DAO fb2sc = new CFB2SC2600DAO();
            DataTable dt = fb2sc.check_VOUCHER(PAY_ID);
            if ((int)dt.Rows[0]["resultCount"] > 0)
            {
                rtnmessage = "該次月薪資已切轉傳票,無法重複切轉! \\n";
            }
            
            dt.Clear();
            return rtnmessage;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    //檢查是否已執行彙計表
    public string CheckSALARY_REPORT_D(string pa_pay_id, string pa_salary_type, string pa_salary_ym, string pa_salary_dt, string pa_pay_kind)
    {
        string rtnmessage = "0";
        int row = 0;
        try
        {
            CFB2SC2600DAO fb2sc = new CFB2SC2600DAO();
            //檢查是否有暫不發薪改為發薪，薪資關帳主檔會有兩筆(SALARY_DT.SALARY_YM.SALARY_TYPE.PAY_KIND)的相同資料，此時就不再往下檢查必須執行彙計表列印作業
            row = fb2sc.checkTB_S_M_SALARY_PAY_H(pa_salary_type, pa_salary_ym.Replace("/",""), pa_salary_dt, pa_pay_kind);

            if (row == 1)
            {
                DataTable dt = fb2sc.getSALARY_REPORT_D(pa_pay_id, pa_salary_type);
                if ((int)dt.Rows[0]["resultCount"] == 0)
                {
                    rtnmessage = "請先執行對應發放項目之彙計表列印作業!! \\n";
                }
                dt.Clear();
            }
            
            
            return rtnmessage;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }
    public DataTable getANALYSIS(string SALARY_DT)
    {

        try
        {
            CFB2SC2600DAO fbsSC = new CFB2SC2600DAO();

            return fbsSC.getANALYSIS(SALARY_DT);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public string getBatchPatch()
    {
       
        try
        {
            CFB2SC2600DAO fbsSC = new CFB2SC2600DAO();



            return fbsSC.getBatchPatch(); 
        }
        catch (Exception)
        {
            throw;
        }
    }

    //薪資月結
    public string Month_Close(string vSalary_type, string vSalary_dt, string vPay_kind, string vProcess_status, string vSalary_ym)
    {
        string msg = "0";
        try
        {
            CFB2SC2600DAO fbsSC = new CFB2SC2600DAO();

            BeginTransaction();
            msg = fbsSC.Month_Close_dao1(vSalary_type, vSalary_dt, vPay_kind, vProcess_status, vSalary_ym);
            Commit();
            /* 在14.4測試要註解   */
            if (msg == "0")//做財務需要的資料
	        {
                msg = fbsSC.Month_Close_dao2(vSalary_type, vSalary_dt, vPay_kind, vProcess_status, vSalary_ym);
	        }
            

            if (msg == "0")
            {
                BeginTransaction();
                msg = fbsSC.Month_Close_dao3(vSalary_type, vSalary_dt, vPay_kind, vProcess_status, vSalary_ym);
                Commit();   
            }
            
            return msg;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //製作傳票
    //-- @salary_dt 發薪日期,@salary_type 發薪類別,@pay_kind 發放項目,@pay_id 關帳代號
    //--@tmc_pay_type TMC付款,@other_remit_dt (媒體轉帳對象外) 實際匯款日, @salary_ym 薪資年月
    //--@company_cd1 聘用公司1,@invno11 支付發票號碼1,@invtpe11 支付發票格式1,@intdt11 支付發票號日期,@invno12 收入發票號碼1,@invtpe12 收入發票格式1,@intdt12 收入發票號日期
    //--@company_cd2 聘用公司2,invno21 支付發票號碼21,@invtype21 支付發票格式21,@intdt21 支付發票號日期,@invno22 收入發票號碼2,@invtpe22 收入發票格式2,@intdt22 收入發票號日期2
    public bool MarkVouch(string salary_dt, string salary_type, string pay_kind, string pay_id, string tmc_pay_type, string other_remit_dt, string salary_ym
         , string company_cd1, string invno11, string invtype11, string intdt11
         , string company_cd2, string invno21, string invtype21, string intdt21, string iadat)
    {
        bool successed = true;
        int itm = 0;//舊項次       
        int seq2 = 0;//傳票後四碼
        string vno = "";//舊傳票號碼
        string vno_head = "";//支付傳票開頭
        string record_cd = "";//有發票(憑證別)的CD，此值為原始正負傳票金額加總後的CD別
        string record_tax = "";//有發票(憑證別)的g稅額，此值為原始正負傳票稅額加總後的CD別
        try
        {
            CFB2SC2600DAO dao = new CFB2SC2600DAO();
            DataTable para_dt = new DataTable();
            //生成傳票時入帳週期由入帳日期年月取代

            string iacyc = iadat.Substring(0, 7);
            BeginTransaction();
            dao.MarkVouch_dao(salary_dt, salary_type, pay_kind, pay_id, tmc_pay_type, other_remit_dt, salary_ym
                                , company_cd1, invno11, invtype11, intdt11
                                , company_cd2, invno21, invtype21, intdt21, iacyc
                                , iadat
                                );
            Commit();

            //取參數

            dao.CREATED_BY = SessionHandle.Current.emp_id.Trim();
            dao.UPDATED_BY = SessionHandle.Current.emp_id.Trim();
            dao.FUNC_ID = "FB2SC260";
            dao.SALARY_DT = salary_dt;
            dao.SALARY_TYPE = salary_type;
            dao.PAY_KIND = pay_kind;
            dao.SALARY_YM = salary_ym;
            dao.PAY_ID = pay_id;
            dao.IACYC = iacyc;
            dao.CO = "KZ";//公司
            //入帳日期 2016/08/19 財務變邏輯 TERRY修改
            DateTime mdt = Convert.ToDateTime(dao.IACYC + "/01");
           

            //20161005 改為  所有傳票界接檔的入帳日期欄位 = SC260生成傳票頁面的入帳日期欄位值。
            dao.IaDat = iadat;

            //薪資發放資料別
            dao.getSys_cd();
            dao.SlyPrvdDtid = dao.SYS_CD;       
            //傳票流水號
            dao.getSEQ2("5Y");

            //原本有憑證別的資料CD，目前僅有才庫，未來有多家時需再修改 
            DataTable dt_cd = dao.getOriginalData();
            
            if (dt_cd.Rows.Count > 0)
            {
                record_tax = dt_cd.Rows[0]["Total_tax"].ToString();
                int money = Convert.ToInt32( dt_cd.Rows[0]["Amount"].ToString());
                if (money > 0)
                {
                    record_cd = "D";
                }
                else
                {
                    record_cd = "C";
                }
            }


            //批號
            para_dt = dao.getLno();
            if (para_dt.Rows.Count > 0)
            {
                dao.Lno = para_dt.Rows[0]["Lno"].ToString();
            }
            para_dt.Clear();
            //買受人
            para_dt = utilities.getParameter("SC", "Cu");
            if (para_dt.Rows.Count > 0)
            {
                dao.Cu = para_dt.Rows[0]["CODE_VAL1"].ToString();
            }
            para_dt.Clear();

            //支付傳票開頭
            para_dt = utilities.getParameter("SC", "B_VOUCHER_SEQ1");
            if (para_dt.Rows.Count > 0)
            {
                vno_head = para_dt.Rows[0]["CODE_VAL1"].ToString();
            }
            para_dt.Clear();

            //沖轉或一般傳票
            string wkStatus = "1"; //wk狀態 1.一般
            para_dt = dao.getStatus();
            if (para_dt.Rows.Count > 0)
            {
                if ((int)para_dt.Rows[0]["bb"] > 1)
                {
                    wkStatus = "2"; //2.沖轉
                }
            }
            para_dt.Clear();

            //幣別
            dao.Cucy = "TWD";
            dao.Exr = "1";
            
            DataTable dt = new DataTable();

            try
            {
                BeginTransaction();
                //刪除暫存檔資料
                dao.deleteFB_TB_S_VOUCHER_TEMP();
                string getMoney = "";//保留此筆受款人
                string voucherid = "";//傳票號碼

                if (wkStatus.Equals("1"))
                {
                    dt = dao.getS1Data();//找出所有一般傳票
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            dao.Pamennm = "";
                            dao.Vochno = dt.Rows[i]["VOUCHER_ID"].ToString();//傳票號碼
                            dao.Wtmen = "";//領款人
                            dao.WtmenNm = "";//領款人名稱
                            dao.Rpamtpes = dt.Rows[i]["H007"].ToString();//受款人
                            //受款人名稱
                            para_dt = dao.getComCode(dao.Rpamtpes);
                            if (para_dt.Rows.Count > 0)
                            {
                                dao.Pamennm = para_dt.Rows[0]["sub_desc"].ToString();
                            }
                            para_dt.Clear();

                            if (dao.Rpamtpes == "" )
                            {
                                dao.Rpamtpes = "12488060";
                            }
                            if (dao.Pamennm == "")
                            {
                                dao.Pamennm = "國瑞汽車股份有限公司";
                            }

                            //dao.Pamennm = "";
                            dao.Itm = (dt.Rows[i]["H001"].ToString()).PadLeft(5, '0');//項次
                            dao.Dc = dt.Rows[i]["H028"].ToString();//借貸
                            dao.Dp = dt.Rows[i]["H021"].ToString();//負擔部門
                            dao.BgDp = dt.Rows[i]["H020"].ToString();//預算部門
                            dao.Sumr = "";
                            dao.AcctUrId = "";
                            dao.Ca = "";
                            dao.RemSumr = dt.Rows[i]["H027"].ToString();//備註摘要
                            dao.Acct = dt.Rows[i]["H016"].ToString();//會計科目
                            dao.Vchid = "";
                            dao.Vchno = "";                          

                            dao.VochAmt = dt.Rows[i]["H025"].ToString(); ;
                            dao.Vochtaxamt = dt.Rows[i]["H026"].ToString(); ;
                            dao.Relno = dao.Vochno + dao.Itm;//相關號碼
                            dao.Obj = dao.Rpamtpes;
                            dao.DdaAmt = "0";
                            dao.Ddataxamt = "0";
                            dao.OcryAmt = "0";//原幣金額
                            dao.Ocrytaxamt = "0";//原幣稅額
                            dao.BkAcno = "";
                            dao.WrEdDat = "";
                            dao.StrnEntryMk = "";
                            if (dt.Rows[i]["H014"].ToString() == "2" || dt.Rows[i]["H014"].ToString() == "3")//支付方式
                            {
                                dao.Padty = "B";
                            }
                            else if (dt.Rows[i]["H014"].ToString() == "4")
                            {
                                dao.Padty = "F";
                            }
                            dao.Cserid = "1";
                            dao.NcrDat = "";
                            dao.IncmTy = "";
                            dao.RcvPcAcid = "";
                            dao.Ckno = "";
                            dao.PayTrm = "";
                            dao.IvDat = "";//?發票日期
                            if (dt.Rows[i]["H014"].ToString() == "4")
                            {
                                dao.CkEdDat = dt.Rows[i]["H009"].ToString();
                            }
                            else
                            {
                                dao.CkEdDat = "";
                            }
                            dao.CkBkId = "";
                            dao.CkBkAccno = "";
                            dao.Clckno = "";
                            dao.CkTrm = "";
                            dao.PaySqno = "";
                            dao.PayMk = "N";
                            dao.VochHcode = "";

                            dao.insertTB_S_VOUCHER_TEMP();
                        }
                    }
                    dt.Clear();
                    
                    dt = dao.getS2Data();//找出所有支付傳票(借貸都已有))
 
                    //刪除薪資傳票明細暫存檔的支付傳票
                    dao.delete_TB_S_S_SALARY_VOUCHER_D_3X();

                    if (dt.Rows.Count > 0)
                    {
                        getMoney = "";
                        voucherid="";
                        vno = "";
                        itm = 0;
                        //傳票流水號
                        dao.getSEQ2("3X");
                        //傳票號碼後5碼
                        seq2 = Convert.ToInt32(dao.SEQ_NO2);

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string st = dt.Rows[i]["H007"].ToString();
                            if( dao.SALARY_TYPE == "C" ) //獎金類
                            { //voucherid 傳票號碼
                            	 if (voucherid != dt.Rows[i]["VOUCHER_ID"].ToString())   //產生新傳票 
		                           {
		                                seq2 = seq2 + 1;
		                                dao.Vochno = vno_head + dao.SYS_CD+ (Convert.ToString(seq2)).PadLeft(5, '0');
		                                //項次
		                                dao.Itm = "00001";
		                                itm = 1;
		                            }
		                            else
		                            {
		                                //相同傳票
		                                dao.Vochno = vno;//傳票號碼
		                                dao.Itm = (itm.ToString()).PadLeft(5, '0');//項次
		                            }
                            	
                            }
                            else{ //非獎金類 (月薪/預付薪
                            	//getMoney 支付對象 
	                            if (getMoney != dt.Rows[i]["H007"].ToString())   //產生新傳票 
	                            {
	                                seq2 = seq2 + 1;
	                                dao.Vochno = vno_head + dao.SYS_CD+ (Convert.ToString(seq2)).PadLeft(5, '0');
	                                //項次
	                                dao.Itm = "00001";
	                                itm = 1;
	                            }
	                            else
	                            {
	                                //相同傳票
	                                dao.Vochno = vno;//傳票號碼
	                                dao.Itm = (itm.ToString()).PadLeft(5, '0');//項次
	                            }
                           }

                            dao.Pamennm = "";
                            dao.Rpamtpes = dt.Rows[i]["H007"].ToString();//受款人
                            //受款人名稱
                            para_dt = dao.getComCode(dao.Rpamtpes);
                            if (para_dt.Rows.Count > 0)
                            {
                                dao.Pamennm = para_dt.Rows[0]["sub_desc"].ToString();
                            }
                            para_dt.Clear();

                            if (dao.Rpamtpes == "")
                            {
                                dao.Rpamtpes = "12488060";
                            }
                            if (dao.Pamennm == "")
                            {
                                dao.Pamennm = "國瑞汽車股份有限公司";
                            }
                            
                            //dao.Pamennm = "";
                            //dao.Itm = (dt.Rows[i]["H001"].ToString()).PadLeft(5, '0');//項次
                            dao.Dc = dt.Rows[i]["H028"].ToString();//借貸
                            dao.Dp = dt.Rows[i]["H021"].ToString();//負擔部門
                            dao.BgDp = dt.Rows[i]["H020"].ToString();//預算部門
                            dao.Sumr = "";
                            dao.AcctUrId = "";
                            dao.Ca = "";
                            dao.RemSumr = dt.Rows[i]["H027"].ToString();//備註摘要


                            if (dt.Rows[i]["H002"].ToString() == "1" && dao.Dc == "D")//支付傳票才有 (H0020 =1 為才庫或其他, = 0為國瑞)
                            {
                                dao.Vchid = "21";//憑證別
                                dao.Vchno = dt.Rows[i]["H003"].ToString();//憑證號碼    
                                
                            }
                            else
                            {
                                dao.Vchid = "";
                                dao.Vchno = "";
                                
                            }

                            if (dao.Dc == "C")
                            {
                                dao.Acct = dt.Rows[i]["ACCOUNTING_NO5"].ToString();//會計科目改抓ACCOUNTING_NO5
                            }
                            else
                            {
                                dao.Acct = dt.Rows[i]["H016"].ToString();//會計科目
                            }
                            //dao.Acct = dt.Rows[i]["H016"].ToString();//會計科目
                            dao.VochAmt = dt.Rows[i]["H025"].ToString();//傳票金額
                            dao.Vochtaxamt = dt.Rows[i]["H026"].ToString();//傳票稅額
                            dao.Relno = dao.Vochno + dao.Itm;//相關號碼
                            dao.Obj = dao.Rpamtpes;
                            dao.DdaAmt = "0";
                            dao.Ddataxamt = "0";
                            dao.OcryAmt = "0";//原幣金額
                            dao.Ocrytaxamt = "0";//原幣稅額
                            dao.BkAcno = "";
                            
                            dao.StrnEntryMk = "";

                            dao.Padty = "";
                            dao.PayTrm = "";
                            /* 20201214
                            //到介接檔取得廠商的相關支付方式與付款條件
                            para_dt = dao.getPaymentData(dt.Rows[i]["H007"].ToString());
                            if (para_dt.Rows.Count >0)
                            {
                                dao.Padty = para_dt.Rows[0]["Padty"].ToString();
                                dao.PayTrm = para_dt.Rows[0]["PayTrm"].ToString();
                            }
                            else
                            {
                                dao.Padty = "";
                                dao.PayTrm = "";
                            }
                            para_dt.Clear();
                            */


                            if (dt.Rows[i]["H014"].ToString() == "4")//票據
                            {                                
                                dao.IvDat = "";//?發票日期
                                dao.WrEdDat = "";
                            }
                            else
                            {
                                
                                dao.IvDat = dt.Rows[i]["H011"].ToString();//?發票日期
                                dao.WrEdDat = dt.Rows[i]["H009"].ToString();
                            }
                            dao.NcrDat = dt.Rows[i]["H009"].ToString();
                            dao.Cserid = "1";
                            
                            dao.IncmTy = "";
                            dao.RcvPcAcid = "";
                            dao.Ckno = "";
                            //dao.PayTrm = "";
                            
                            if (dt.Rows[i]["H014"].ToString() == "4")
                            {
                                dao.CkEdDat = dt.Rows[i]["H009"].ToString();
                            }
                            else
                            {
                                dao.CkEdDat = "";
                            }
                            dao.CkBkId = "";
                            dao.CkBkAccno = "";
                            dao.Clckno = "";
                            dao.CkTrm = "";
                            dao.PaySqno = "";
                            dao.PayMk = "N";
                            dao.VochHcode = "";

                            dao.insertTB_S_VOUCHER_TEMP();
                            itm = itm + 1 ;
                            if(dao.SALARY_TYPE == "C" ) //獎金類
                            {
                              voucherid = dt.Rows[i]["VOUCHER_ID"].ToString();
                            }  
                            else
                            {
                            	getMoney = dt.Rows[i]["H007"].ToString();
                            }  
                            vno = dao.Vochno;

                            /*將資料更新*/
                            dao.DATA_TYPE = dt.Rows[i]["DATA_TYPE"].ToString();
                            dao.VOUCHER_ID = dao.Vochno;
                            dao.GROUP_ID = dt.Rows[i]["GROUP_ID"].ToString();
                            dao.H001 = Convert.ToString(itm-1);
                            dao.H002 = dt.Rows[i]["H002"].ToString();
                            
                            dao.H003 = dt.Rows[i]["H003"].ToString();
                            dao.H004 = dt.Rows[i]["H004"].ToString();
                            dao.H005 = dt.Rows[i]["H005"].ToString();
                            dao.H006 = dt.Rows[i]["H006"].ToString();
                            dao.H007 = dt.Rows[i]["H007"].ToString();

                            dao.H008 = dt.Rows[i]["H008"].ToString();
                            dao.H009 = dt.Rows[i]["H009"].ToString();
                            dao.H010 = dt.Rows[i]["H010"].ToString();
                            dao.H011 = dt.Rows[i]["H011"].ToString();
                            dao.H012 = dt.Rows[i]["H012"].ToString();

                            dao.H013 = dt.Rows[i]["H013"].ToString();
                            dao.H014 = dt.Rows[i]["H014"].ToString();
                            dao.H015 = dt.Rows[i]["H015"].ToString();
                            dao.H016 = dt.Rows[i]["H016"].ToString();
                            dao.H017 = dt.Rows[i]["H017"].ToString();

                            dao.H018 = dt.Rows[i]["H018"].ToString();
                            dao.H019 = dt.Rows[i]["H019"].ToString();
                            dao.H020 = dt.Rows[i]["H020"].ToString();
                            dao.H021 = dt.Rows[i]["H021"].ToString();
                            dao.H022 = dt.Rows[i]["H022"].ToString();

                            dao.H023 = dt.Rows[i]["H023"].ToString();
                            dao.H024 = dt.Rows[i]["H024"].ToString();
                            dao.H025 = dt.Rows[i]["H025"].ToString();
                            dao.H026 = dt.Rows[i]["H026"].ToString();
                            dao.H027 = dt.Rows[i]["H027"].ToString();

                            dao.H028 = dt.Rows[i]["H028"].ToString();
                            dao.DEL_MARK = dt.Rows[i]["DEL_MARK"].ToString();

                            dao.insert_TB_S_S_SALARY_VOUCHER_D();
                        }
                    }

                    dt.Clear();
                    /*
                     *進項稅額
                     */
                    dt = dao.getVoucherTaxData();
                    dao.Wtmen = "";
                    dao.WtmenNm = "";
              

                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (dt.Rows[i]["ROWNO"].ToString() == "1") //可能有多筆資料，只看第一筆做進項稅額
                            {
                                dao.Vochno = dt.Rows[i]["VOUCHER_ID"].ToString();//傳票號碼
                                dao.Itm = "";//項次?

                                dao.Pamennm = "";
                                dao.Rpamtpes = dt.Rows[i]["H007"].ToString();//受款人
                                //受款人名稱
                                para_dt = dao.getComCode(dao.Rpamtpes);
                                if (para_dt.Rows.Count > 0)
                                {
                                    dao.Pamennm = para_dt.Rows[0]["sub_desc"].ToString();
                                }
                                para_dt.Clear();

                                if (dao.Rpamtpes == "")
                                {
                                    dao.Rpamtpes = "12488060";
                                }
                                if (dao.Pamennm == "")
                                {
                                    dao.Pamennm = "國瑞汽車股份有限公司";
                                }

                                //用受款人去暫存檔找最大的項次
                                para_dt = dao.getItemData();
                                if (para_dt.Rows.Count > 0)
                                {
                                    dao.Itm = Convert.ToString(Convert.ToInt32(para_dt.Rows[0]["Itm"].ToString()) + 1).PadLeft(5, '0');
                                }

                                para_dt.Clear();

                                dao.Dc = record_cd;//借貸
                                
                                dao.Dp = dt.Rows[i]["H021"].ToString();//負擔部門
                                dao.BgDp = dt.Rows[i]["H020"].ToString();//預算部門
                                dao.Sumr = "";
                                dao.AcctUrId = "";
                                dao.Ca = "";
                                dao.RemSumr = dt.Rows[i]["H027"].ToString();//備註摘要


                                //財務部確認不需此兩欄位資料
                                //dao.Vchid = "21";//憑證別
                                //dao.Vchno = dt.Rows[i]["H003"].ToString();//憑證號碼
                                dao.Vchid = "";//憑證別
                                dao.Vchno = "";//憑證號碼
                                dao.VochAmt = record_tax;//傳票金額
                                dao.Vochtaxamt = "0";//傳票稅額    

                                dao.Acct = "1190149005";
                                
                                              

                                dao.Relno = dao.Vochno + dao.Itm;//相關號碼
                                dao.Obj = dao.Rpamtpes;
                                dao.DdaAmt = "0";
                                dao.Ddataxamt = "0";
                                dao.OcryAmt = "0";//原幣金額
                                dao.Ocrytaxamt = "0";//原幣稅額
                                dao.BkAcno = "";
                                dao.WrEdDat = "";
                                dao.StrnEntryMk = "";

                                dao.Padty = "";
                                dao.PayTrm = "";
                                /*
                                //到介接檔取得廠商的相關支付方式與付款條件
                                para_dt = dao.getPaymentData(dt.Rows[i]["H007"].ToString());
                                if (para_dt.Rows.Count > 0)
                                {
                                    dao.Padty = para_dt.Rows[0]["Padty"].ToString();
                                    dao.PayTrm = para_dt.Rows[0]["PayTrm"].ToString();
                                }
                                else
                                {
                                    dao.Padty = "";
                                    dao.PayTrm = "";
                                }
                                para_dt.Clear();
                                */

                                dao.NcrDat = dt.Rows[i]["H009"].ToString();
                                dao.Cserid = "1";
                                
                                dao.IncmTy = "";
                                dao.RcvPcAcid = "";
                                dao.Ckno = "";
                                //dao.PayTrm = "";
                                dao.IvDat = "";//?發票日期
                                if (dt.Rows[i]["H014"].ToString() == "4")
                                {
                                    dao.CkEdDat = dt.Rows[i]["H009"].ToString();
                                }
                                else
                                {
                                    dao.CkEdDat = "";
                                }
                                dao.CkBkId = "";
                                dao.CkBkAccno = "";
                                dao.Clckno = "";
                                dao.CkTrm = "";
                                dao.PaySqno = "";
                                dao.PayMk = "N";
                                dao.VochHcode = "";

                                dao.insertTB_S_VOUCHER_TEMP();   

                                //更新此傳票貸方(正常情況下是貸方C，基本上應該會跟這次的借貸相反，取來update
                                dao.updtae_VOUCHER_TEMP();

                            }
                                                     
                        }
                    }
 
                }//wkStatus.Equals("1") end
                else if (wkStatus.Equals("2")) //沖轉
                {
                    dt.Clear();
                    dao.getD_Mark1Data();
                    /*沖轉的一般傳票 */
                    if (dt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            dao.Pamennm = "";
                            dao.Vochno = dt.Rows[i]["VOUCHER_ID"].ToString();//傳票號碼
                            dao.Wtmen = "";//領款人
                            dao.WtmenNm = "";//領款人名稱
                            dao.Rpamtpes = dt.Rows[i]["H007"].ToString();//受款人
                            //受款人名稱
                            para_dt = dao.getComCode(dao.Rpamtpes);
                            if (para_dt.Rows.Count > 0)
                            {
                                dao.Pamennm = para_dt.Rows[0]["sub_desc"].ToString();
                            }
                            para_dt.Clear();

                            if (dao.Rpamtpes == "")
                            {
                                dao.Rpamtpes = "12488060";
                            }
                            if (dao.Pamennm == "")
                            {
                                dao.Pamennm = "國瑞汽車股份有限公司";
                            }
                            //dao.Pamennm = "";
                            dao.Itm = (dt.Rows[i]["H001"].ToString()).PadLeft(5, '0');//項次
                            if (dt.Rows[i]["H028"].ToString()=="C")
                            {
                                dao.Dc = "D";
                            }else
	                        {
                                dao.Dc = "C";
	                        }
                            //dao.Dc = dt.Rows[i]["H028"].ToString();//借貸
                            dao.Dp = dt.Rows[i]["H021"].ToString();//負擔部門
                            dao.BgDp = dt.Rows[i]["H020"].ToString();//預算部門
                            dao.Sumr = "";
                            dao.AcctUrId = "";
                            dao.Ca = "";
                            dao.RemSumr = dt.Rows[i]["H027"].ToString();//備註摘要
                            dao.Acct = dt.Rows[i]["H016"].ToString();//會計科目
                            
                            dao.Vchid = "";
                            dao.Vchno = "";
                         
                            dao.VochAmt = dt.Rows[i]["H025"].ToString();//傳票金額
                            dao.Vochtaxamt = dt.Rows[i]["H026"].ToString();//傳票稅額
                            if (dao.Dc == "C")//?
                            {
                                dao.VochAmt = Convert.ToString(Convert.ToInt32(dt.Rows[i]["H025"].ToString()) + Convert.ToInt32(dt.Rows[i]["H026"].ToString()));
                                dao.Vochtaxamt = "0";
                            }
                            dao.Relno = dao.Vochno + dao.Itm;//相關號碼
                            dao.Obj = dao.Rpamtpes;
                            dao.DdaAmt = "0";
                            dao.Ddataxamt = "0";
                            dao.OcryAmt = "0";//原幣金額
                            dao.Ocrytaxamt = "0";//原幣稅額
                            dao.BkAcno = "";
                            dao.WrEdDat = "";
                            dao.StrnEntryMk = "";
                            if (dt.Rows[i]["H014"].ToString() == "2" || dt.Rows[i]["H014"].ToString() == "3")//支付方式
                            {
                                dao.Padty = "B";
                            }
                            else if (dt.Rows[i]["H014"].ToString() == "4")
                            {
                                dao.Padty = "F";
                            }
                            dao.Cserid = "1";
                            dao.NcrDat = "";
                            dao.IncmTy = "";
                            dao.RcvPcAcid = "";
                            dao.Ckno = "";
                            dao.PayTrm = "";
                            dao.IvDat = "";//?發票日期
                            if (dt.Rows[i]["H014"].ToString() == "4")
                            {
                                dao.CkEdDat = dt.Rows[i]["H009"].ToString();
                            }
                            dao.CkBkId = "";
                            dao.CkBkAccno = "";
                            dao.Clckno = "";
                            dao.CkTrm = "";
                            dao.PaySqno = "";
                            dao.PayMk = "N";
                            dao.VochHcode = "";

                            dao.insertTB_S_VOUCHER_TEMP();
                        }
                    }
                    dt.Clear();
                    /*沖轉的支付傳票 */
                    dt = dao.getD_MarkOtherData();
                    if (dt.Rows.Count > 0)
                    {
                        getMoney = "";
                        vno = "";
                        itm = 0;
                        //傳票流水號
                        dao.getSEQ2("3X");
                        //傳票號碼後四碼
                        seq2 = Convert.ToInt32(dao.SEQ_NO2);

                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            if (getMoney != dt.Rows[i]["H007"].ToString())   //產生新傳票 
                            {                                
                                //傳票號碼
                                dao.Vochno = vno_head + dao.SYS_CD + (Convert.ToString(seq2 + 1)).PadLeft(5, '0');
                                //項次
                                dao.Itm = "00001";
                                itm = 1;
                            }
                            else
                            {
                                //相同傳票
                                dao.Vochno = vno;//傳票號碼
                                dao.Itm = (itm.ToString()).PadLeft(5, '0');//項次
                            }

                            dao.Pamennm = "";                           
                            dao.Wtmen = "";//領款人
                            dao.WtmenNm = "";//領款人名稱
                            dao.Rpamtpes = dt.Rows[i]["H007"].ToString();//受款人
                            //受款人名稱
                            para_dt = dao.getComCode(dao.Rpamtpes);
                            if (para_dt.Rows.Count > 0)
                            {
                                dao.Pamennm = para_dt.Rows[0]["sub_desc"].ToString();
                            }
                            para_dt.Clear();

                            if (dao.Rpamtpes == "")
                            {
                                dao.Rpamtpes = "12488060";
                            }
                            if (dao.Pamennm == "")
                            {
                                dao.Pamennm = "國瑞汽車股份有限公司";
                            }
                                                   
                            dao.Dc = dt.Rows[i]["H028"].ToString();//借貸
                            dao.Dp = dt.Rows[i]["H021"].ToString();//負擔部門
                            dao.BgDp = dt.Rows[i]["H020"].ToString();//預算部門
                            dao.Sumr = "";
                            dao.AcctUrId = "";
                            dao.Ca = "";
                            dao.RemSumr = dt.Rows[i]["H027"].ToString();//備註摘要
                            dao.Acct = dt.Rows[i]["H016"].ToString();//會計科目

                            if (dt.Rows[i]["H002"].ToString() == "1" && dao.Dc =="C")//支付傳票才有 (H0020 =1 為才庫或其他, = 0為國瑞)
                            {
                                dao.Vchid = "21";//憑證別
                                dao.Vchno = dt.Rows[i]["H003"].ToString();//憑證號碼                               
                            }
                            else
                            {
                                dao.Vchid = "";
                                dao.Vchno = "";                               
                            }
                           
                            dao.VochAmt = dt.Rows[i]["H025"].ToString();//傳票金額
                            dao.Vochtaxamt = dt.Rows[i]["H026"].ToString();//傳票稅額
                            dao.Relno = dao.Vochno + dao.Itm;//相關號碼
                            dao.Obj = dao.Rpamtpes;
                            dao.DdaAmt = "0";
                            dao.Ddataxamt = "0";
                            dao.OcryAmt = "0";//原幣金額
                            dao.Ocrytaxamt = "0";//原幣稅額
                            dao.BkAcno = "";
                            dao.WrEdDat = "";
                            dao.StrnEntryMk = "";
                            if (dt.Rows[i]["H014"].ToString() == "2" || dt.Rows[i]["H014"].ToString() == "3")//支付方式
                            {
                                dao.Padty = "B";
                            }
                            else if (dt.Rows[i]["H014"].ToString() == "4")
                            {
                                dao.Padty = "F";
                            }

                            //if (dt.Rows[i]["H014"].ToString() == "4")//票據
                            //{
                            //    dao.NcrDat = "";
                            //}
                            //else
                            //{
                            //    dao.NcrDat = dt.Rows[i]["H009"].ToString();
                            //}
                            dao.NcrDat = dt.Rows[i]["H009"].ToString();
                            dao.Cserid = "1";
                            
                            dao.IncmTy = "";
                            dao.RcvPcAcid = "";
                            dao.Ckno = "";
                            dao.PayTrm = "";
                            dao.IvDat = "";//?發票日期
                            if (dt.Rows[i]["H014"].ToString() == "4")
                            {
                                dao.CkEdDat = dt.Rows[i]["H009"].ToString();
                            }
                            else
                            {
                                dao.CkEdDat = "";
                            }
                            dao.CkBkId = "";
                            dao.CkBkAccno = "";
                            dao.Clckno = "";
                            dao.CkTrm = "";
                            dao.PaySqno = "";
                            dao.PayMk = "N";
                            dao.VochHcode = "";

                            dao.insertTB_S_VOUCHER_TEMP();
                            itm = itm + 1;
                            getMoney = dt.Rows[i]["H007"].ToString();
                            vno = dao.Vochno;
                        }
                    }
                    dt.Clear();
                }

                //刪除舊傳票 in TB_S_M_SALARY_VOUCHER
                dao.delete_TB_S_M_SALARY_VOUCHER(pay_id);
                //重新長出傳票 in TB_S_M_SALARY_VOUCHER
                dao.insert_TB_S_M_SALARY_VOUCHER(pay_id);
                //寫入傳票號碼序號檔
                dao.insertTB_S_M_VOUCHER_SEQ();


                Commit();


                

                /*
                //將資料寫到FF1
                dao.RunSP_I_FF1_VOUCHER();
                //拿FF1 log
                DataTable dt_sp = dao.checkSP();
                if (dt_sp.Rows.Count > 0)
                {
                    if (dt_sp.Rows[0]["ERROR_FLAG"].ToString() != "")
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
                */

               

            }
            catch (Exception)
            {
                
                throw;
            }
         
            
            return successed;
        }
        catch (Exception)
        {
            this.RollBack();
            throw;
        }
    }

    public string getSQLLNO(string Lno, string TblId)
    {
        string errormessage = "0";
        try
        {
            CFB2SC2600DAO dao = new CFB2SC2600DAO();
            DataTable dt = dao.getSQLLNO(Lno, TblId);
            if (dt.Rows.Count > 0)
            {
                string GCM = dt.Rows[0]["GetChveMrtMk"].ToString();//抓入成功註記
                string AWM = dt.Rows[0]["AvWgtcmpsMk"].ToString();//可重作註記
                if (GCM == "Y" && AWM != "Y")
                {
                    errormessage += "此薪資已進入財務系統，不能再重新計算\\n";
                    return errormessage;
                }
            }

            return errormessage;

        }
        catch (Exception)
        {
            throw;
        }
    }

    public string chek_SAP_DONE(string is_VOUCHER,string is_SAP,string salary_type,string pay_id)
    {
        string errormessage = "0";
        try
        {
            //未計算
            if (is_VOUCHER == "N")
            {
                return errormessage;
            }

            //未傳送SAP
            if (is_SAP == "N")
            {
                return errormessage;
            }

            //已計算但未傳送SAP
            if (is_VOUCHER == "Y" && is_SAP == "N")
            {
                return errormessage;
            }

            //已傳送SAP
            if (is_SAP == "Y") {
                CFB2SC2600DAO dao = new CFB2SC2600DAO();
                if (dao.chek_SAP_DONE(salary_type, pay_id) == "E")
                    errormessage = "傳票SAP已立帳,不允執行!";

                return errormessage;
            }

            return errormessage;
        }
        catch (Exception)
        {
            throw;
        }
    }


    public string VOUCHER_SAP(string pay_id)
    {
        string errormessage = "0";
        try
        { 
            CFB2SC2600DAO dao = new CFB2SC2600DAO();
            errormessage= dao.VOUCHER_SAP(pay_id) ;               
            return errormessage;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //結轉傳票_TOSAP
    public string SP_S_SC2600_VOUCHER_SAP0(string pay_id)
    {
        string errormessage = "0";
        try
        {
            CFB2SC2600DAO dao = new CFB2SC2600DAO();
            errormessage = dao.SP_S_SC2600_VOUCHER_SAP0(pay_id);
            return errormessage;
        }
        catch (Exception)
        {
            throw;
        }
    }
    
    public string delete(List<Tuple<string, string, string>> deleteList, CFB2SC2600DAO dao)
    {
        string msg = "0";
        DataTable dt = new DataTable();
        try
        {           
            
            //bool pass = true;
            //int countD01M = 0;
            //薪資發放資料別
            dao.getSys_cd();
            dao.SlyPrvdDtid = dao.SYS_CD;

            BeginTransaction();
            if (msg == "0")
            {
                foreach (var index in deleteList)
                {
                    dao.ACCT_ID = index.Item2;
                    //改以群組代號來刪
                    dao.GROUP_ID = index.Item3;

                    //刪除傳票暫存檔
                    dao.delete_VOUCHER_TEMP();
                    // 刪除 薪資傳票檔
                    dao.delete_VOUCHER();
                                                      
                }

                if (msg == "0")
                {
                    Commit();                   
                }
                else
                    RollBack();

               
            }
            return msg;
        }
        catch (Exception ex)
        {
            RollBack();
            return ex.Message.Replace("\r\n", "").Replace("'", "\"");
        }
    }

    public string deleteFlow(Tuple<string, string> index, CFB2SC2600DAO dao)
    {       
        string msg = "";
        int count_DCCC82M = 0;
        int count_DCCC84M = 0;
        int count_DCCC85M = 0;

        try
        {
            count_DCCC82M = dao.checkDCCisExist("DCCC82M");
            count_DCCC84M = dao.checkDCCisExist("DCCC84M");
            count_DCCC85M = dao.checkDCCisExist("DCCC85M");
            
            //以 傳票群組號碼 = 資料列.部門傳票號碼 +部門傳票號碼 <>傳票群組號碼 為條件  讀取 DCCC82M一般傳票
            if (count_DCCC82M > 0)
            {
                
                //以 部門傳票號碼 = 資料列.部門傳票號碼 為條件  讀取 DCCC01M (傳票處理收據明細暫存檔)
                if (dao.getDCCC01Mcount() > 0)
                {
                    msg += index.Item2 + "該筆資料傳票號碼已生成,無法執行傳票刪除作業!!";                    
                }
                else
                {
                    dao.deleteDCCC82M_DEL();
                    dao.deleteTB_S_S_SALARY_VOUCHER_D();
                    dao.deleteTB_S_M_SALARY_VOUCHER();
                    msg = "0";
                }
            }
            else
            {                
                count_DCCC82M = dao.checkDCCisExist_Equal("DCCC82M");//DCCC82M 傳票群組號碼=部門傳票號碼
                
                if (count_DCCC82M > 0)
                {                    
                    dao.deleteDCCC82M_DEL();
                    dao.deleteTB_S_S_SALARY_VOUCHER_D();
                    dao.deleteTB_S_M_SALARY_VOUCHER();
                    msg = "0";
                }                
            }
                       
            //以 傳票群組號碼 = 資料列.部門傳票號碼 +部門傳票號碼 <>傳票群組號碼 為條件  讀取 DCCC84M一般傳票            
            if (count_DCCC84M > 0)
            {
                //以 部門傳票號碼 = 資料列.部門傳票號碼 為條件  讀取 DCCC01M (傳票處理收據明細暫存檔)                
                
                DataTable cc1 = dao.getDCCC01Mcount1();
                
                if (cc1.Rows.Count > 0)
                {
                    dao.temp = Convert.ToInt32( cc1.Rows[0]["resultCount"].ToString().Trim()); 
                }
                
                if (dao.temp > 0)
                {                    
                    msg = index.Item2 + "該筆資料傳票號碼已生成,無法執行傳票刪除作業!!";                    
                }
                else
                {                                        
                    dao.deleteDCCC84M_DEL();
                    dao.deleteTB_S_S_SALARY_VOUCHER_D();
                    dao.deleteTB_S_M_SALARY_VOUCHER();
                    msg = "0";
                }
            }
            else
            {
                
                count_DCCC84M = dao.checkDCCisExist_Equal("DCCC84M");//DCCC84M 傳票群組號碼=部門傳票號碼
                
                dao.temp = count_DCCC84M;
                if (count_DCCC84M > 0)
                {                    
                    dao.deleteDCCC84M_DEL();                    
                    dao.deleteTB_S_S_SALARY_VOUCHER_D();
                    dao.deleteTB_S_M_SALARY_VOUCHER();
                    
                    msg = "0";
                }
            }

            //以 傳票群組號碼 = 資料列.部門傳票號碼 +部門傳票號碼 <>傳票群組號碼 為條件  讀取 DCCC85M一般傳票
            if (count_DCCC85M > 0)
            {
                //以 部門傳票號碼 = 資料列.部門傳票號碼 為條件  讀取 DCCC01M (傳票處理收據明細暫存檔)
                if (dao.getDCCC01Mcount() > 0)
                {
                    msg += index.Item2 + "該筆資料傳票號碼已生成,無法執行傳票刪除作業!!";                    
                }
                else
                {
                    dao.deleteDCCC85M_DEL();
                    dao.deleteTB_S_S_SALARY_VOUCHER_D();
                    dao.deleteTB_S_M_SALARY_VOUCHER();
                    msg = "0";
                }
            }
            else
            {
                count_DCCC85M = dao.checkDCCisExist_Equal("DCCC85M");//DCCC85M 傳票群組號碼=部門傳票號碼
                if (count_DCCC85M > 0)
                {
                    dao.deleteDCCC85M_DEL();
                    dao.deleteTB_S_S_SALARY_VOUCHER_D();
                    dao.deleteTB_S_M_SALARY_VOUCHER();
                    msg = "0";
                }
            }

            return msg;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
       
    }
    

    public string excute3(List<Tuple<string, string>> excuteList, string pay_id)
    {
        try
        {
            CFB2SC2600DAO dao = new CFB2SC2600DAO();
            BeginTransaction();
            foreach (var index in excuteList)
            {
                dao.PAY_ID = pay_id;
                dao.GROUP_ID = index.Item2;
                dao.DEPT_ACCT_ID = index.Item1;
                DataTable dt82 = dao.checkDCCisExist_excute3("DCCC82M");
                if (dt82.Rows.Count > 0)
                {
                    foreach (DataRow row in dt82.Rows)
                    {
                        dao.DEPT_ACCT_ID = row["DEPT_ACCT_ID"].ToString();
                        dao.updateTB_S_M_SALARY_VOUCHER();
                        dao.deleteDCCC82M();
                    }
                }

                DataTable dt84 = dao.checkDCCisExist_excute3("DCCC84M");
                if (dt84.Rows.Count > 0)
                {
                    foreach (DataRow row in dt84.Rows)
                    {
                        dao.DEPT_ACCT_ID = row["DEPT_ACCT_ID"].ToString();
                        dao.updateTB_S_M_SALARY_VOUCHER();
                        dao.deleteDCCC84M();
                    }
                }

                DataTable dt85 = dao.checkDCCisExist_excute3("DCCC85M");
                if (dt85.Rows.Count > 0)
                {
                    foreach (DataRow row in dt85.Rows)
                    {
                        dao.DEPT_ACCT_ID = row["DEPT_ACCT_ID"].ToString();
                        dao.updateTB_S_M_SALARY_VOUCHER();
                        dao.deleteDCCC85M();
                    }
                }
            }
            Commit();

            return "0";
        }

        catch
        {
            RollBack();
            throw;
        }
    }

}