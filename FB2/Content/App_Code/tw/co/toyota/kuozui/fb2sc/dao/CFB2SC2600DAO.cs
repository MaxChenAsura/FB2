using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;
using System.Text;
using System.Collections;

/// <summary>
/// CFB2SC2600DAO 的摘要描述
/// </summary>
public class CFB2SC2600DAO : BaseDAO
{
    public int count { get; set; }
    public Int64 RowNumber { get; set; }
    public string DEPT_ACCT_ID { get; set; }
    public string PAY_ID { get; set; }
    public string GROUP_ID { get; set; }
    public string ACCT_ID { get; set; }
    public string SALARY_TYPE { get; set; }
    public string SALARY_DT { get; set; }
    public string SALARY_YM { get; set; }
    public int temp { get; set; }
    public string temp1 { get; set; }


    //  public string is_valid { get; set; }
    public string CREATED_BY { get; set; }
    public string UPDATED_BY { get; set; }
    public string FUNC_ID { get; set; }

    //SQLLNO Para
    public string TblId { get; set; }
    public string Co { get; set; }
    public string Lno { get; set; }
    public string DtCnt { get; set; }
    public string BegDat { get; set; }
    public string BegTm { get; set; }
    public string EndDat { get; set; }
    public string EndTm { get; set; }
    public string WrsIaChveMrtMk { get; set; }
    public string GetChveMrtMk { get; set; }
    public string PsComt { get; set; }
    public string AvWgtcmpsMk { get; set; }
    public string TxEmp { get; set; } //異動人員
    public string TxDat { get; set; } //異動日期
    public string TxTm { get; set; } //異動時間   

    //財務界接
    public string PAY_KIND { get; set; } //薪資項目
    public string SYS_CD { get; set; } //薪資發放資料別
    public string SEQ_NO2 { get; set; } //傳票流水號
    public string A_VOUCHER_SEQ1 { get; set; } //轉帳傳票代號
    public string BUDGET_C { get; set; } //預算CD_C；補充保費負擔部門月度檔.科目=1~3(間接)時，為9222 
    public string BUDGET_D { get; set; } //預算CD_D；科目=4(直接)時，為9122
    public string BUDGET_T { get; set; } //預算CD_T；最後一列(合計)為5799

    //傳票界接檔
    public string IaDat { get; set; } //入帳日期
    public string Cu { get; set; } //買受人 12488060
    public string Itm { get; set; } //項次
    public string Vochno { get; set; } //傳票號碼
    public string Dc { get; set; } //借貸
    public string Dp { get; set; } //成本負擔部門
    public string BgDp { get; set; } //預算部門
    public string RemSumr { get; set; } //備註摘要
    public string Acct { get; set; } //會計科目
    public string Relno { get; set; } //相關號碼
    public string OcryAmt { get; set; } //原幣金額
    public string Ocrytaxamt { get; set; } //原幣稅額
    public string Padty { get; set; } //支付方式
    //public string Lno { get; set; } //批號
    public string CO { get; set; }
    public string SlyPrvdDtid { get; set; }
    public string Wtmen { get; set; }
    public string WtmenNm { get; set; }
    public string Rpamtpes { get; set; }
    public string Pamennm { get; set; }
    public string Sumr { get; set; }
    public string Ca { get; set; }
    public string Vchid { get; set; }
    public string Vchno { get; set; }
    public string VochAmt { get; set; }
    public string Vochtaxamt { get; set; }
    public string Obj { get; set; }
    public string DdaAmt { get; set; }
    public string Ddataxamt { get; set; }
    public string Cucy { get; set; }
    public string Exr { get; set; }
    public string BkAcno { get; set; }
    public string WrEdDat { get; set; }
    public string StrnEntryMk { get; set; }
    public string Cserid { get; set; }
    public string NcrDat { get; set; }
    public string IncmTy { get; set; }
    public string RcvPcAcid { get; set; }
    public string Ckno { get; set; }
    public string PayTrm { get; set; }
    public string IvDat { get; set; }
    public string CkEdDat { get; set; }
    public string CkBkId { get; set; }
    public string CkBkAccno { get; set; }
    public string Clckno { get; set; }
    public string CkTrm { get; set; }
    public string PaySqno { get; set; }
    public string PayMk { get; set; }
    public string VochHcode { get; set; }
    public string AcctUrId { get; set; }
    public string IACYC { get; set; }//入帳週期

    //薪資傳票明細暫存檔
    public string DATA_TYPE { get; set; }
    public string VOUCHER_ID { get; set; }    
    public string DEL_MARK { get; set; }
    public string H001 { get; set; }
    public string H002 { get; set; }
    public string H003 { get; set; }
    public string H004 { get; set; }
    public string H005 { get; set; }
    public string H006 { get; set; }
    public string H007 { get; set; }
    public string H008 { get; set; }
    public string H009 { get; set; }
    public string H010 { get; set; }
    public string H011 { get; set; }
    public string H012 { get; set; }
    public string H013 { get; set; }
    public string H014 { get; set; }
    public string H015 { get; set; }
    public string H016 { get; set; }
    public string H017 { get; set; }
    public string H018 { get; set; }
    public string H019 { get; set; }
    public string H020 { get; set; }
    public string H021 { get; set; }
    public string H022 { get; set; }
    public string H023 { get; set; }
    public string H024 { get; set; }
    public string H025 { get; set; }
    public string H026 { get; set; }
    public string H027 { get; set; }
    public string H028 { get; set; }      


    public CFB2SC2600DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public DataTable GetData(int startRowIndex, int maximumRows, string sortExpression, string qry_salary_dt_s, string qry_salary_dt_e, string salary_type,
                                string qry_pay_dt_s, string qry_pay_dt_e, string qry_pay_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select * From");
            sb.Append(" (select  ROW_NUMBER() OVER(ORDER BY a.SALARY_YM desc,a.SALARY_TYPE) As RowNumber ");
            sb.Append(" ,a.SALARY_TYPE,c.SUB_DESC as SALARY_TYPE_NAME,left(a.SALARY_YM,4)+'/'+right(a.SALARY_YM,2) as SALARY_YM");
            sb.Append(" ,a.SALARY_DT,a.PAY_KIND,e.SALARY_NAME as PAY_KIND_NAME ");
            sb.Append(" ,iif(SAP_HR_NO='','N','Y') AS IS_SAP ");
            sb.Append(" ,a.IS_VOUCHER ");
            //sb.Append(" ,'N' AS IS_VOUCHER  ");            
            sb.Append(" ,b.PROCESS_STATUS,d.SUB_DESC as PROCESS_STATUS_NAME,a.PAY_ID,a.PAY_DT,a.EMAIL_DT ");
            sb.Append(" ,a.CLOSED_DT,left(b.IACYC,4)+'/'+right(b.IACYC,2) as IACYC,f.Lno  ");
            sb.Append(" from  TB_S_M_SALARY_PAY_H a");
            sb.Append(" left join TB_S_M_SALARY_CAL_H b on a.SALARY_DT=b.SALARY_DT and a.SALARY_TYPE=b.SALARY_TYPE and a.PAY_KIND=b.PAY_KIND");
            sb.Append(" left join TB_9_M_COMM_D c on c.SYS_CD='SC' and c.MAIN_CD='SALARY_TYPE' and c.sub_cd=a.SALARY_TYPE");
            sb.Append(" left join TB_9_M_COMM_D d on d.SYS_CD='SC' and d.MAIN_CD='PROCESS_STATUS' and d.sub_cd=b.PROCESS_STATUS");
            sb.Append(" left join VW_SALARYAND9999 e on e.SALARY_ID=a.PAY_KIND");
            sb.Append(" left join (select PAY_ID,Lno,isnull(MAX(SAP_HR_NO),'') SAP_HR_NO from TB_S_M_SALARY_VOUCHER GROUP BY PAY_ID,Lno  ) f on a.PAY_ID = f.PAY_ID");
            sb.Append(" where b.PROCESS_STATUS in ('3','4')  ");
            if (qry_salary_dt_s != "")
            {
                sb.Append(" and a.SALARY_DT >= @qry_salary_dt_s ");
                ht.Add("@qry_salary_dt_s", qry_salary_dt_s);
            }
            if (qry_salary_dt_e != "")
            {
                sb.Append(" and a.SALARY_DT <= @qry_salary_dt_e ");
                ht.Add("@qry_salary_dt_e", qry_salary_dt_e);
            }
            if (qry_pay_dt_s != "")
            {
                sb.Append(" and a.PAY_DT >= @qry_pay_dt_s ");
                ht.Add("@qry_pay_dt_s", qry_pay_dt_s);
            }
            if (qry_pay_dt_e != "")
            {
                sb.Append(" and a.PAY_DT <= @qry_pay_dt_e ");
                ht.Add("@qry_pay_dt_e", qry_pay_dt_e);
            }
            if (salary_type != "")
            {
                sb.Append(" and a.SALARY_TYPE = @salary_type ");
                ht.Add("@salary_type", salary_type);
            }
            if (qry_pay_id != "")
            {
                sb.Append(" and a.pay_id = @qry_pay_id ");
                ht.Add("@qry_pay_id", qry_pay_id);
            }

            sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar) ");

            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public int GetCount(int startRowIndex, int maximumRows, string qry_salary_dt_s, string qry_salary_dt_e, string salary_type, string qry_pay_dt_s, string qry_pay_dt_e, string qry_pay_id)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select COUNT(*) total_record");
            sb.Append(" from TB_S_M_SALARY_PAY_H a ");
            sb.Append(" left join TB_S_M_SALARY_CAL_H b on a.SALARY_DT=b.SALARY_DT and a.SALARY_TYPE=b.SALARY_TYPE and a.PAY_KIND=b.PAY_KIND");
            //sb.Append(" left join (select distinct PAY_ID,Lno from TB_S_M_SALARY_VOUCHER )f on a.PAY_ID = f.PAY_ID");
            sb.Append(" where b.PROCESS_STATUS in ('3','4')");
            if (qry_salary_dt_s != "")
            {
                sb.Append(" and a.SALARY_DT >= @qry_salary_dt_s ");
                ht.Add("@qry_salary_dt_s", qry_salary_dt_s);
            }
            if (qry_salary_dt_e != "")
            {
                sb.Append(" and a.SALARY_DT <= @qry_salary_dt_e ");
                ht.Add("@qry_salary_dt_e", qry_salary_dt_e);
            }
            if (qry_pay_dt_s != "")
            {
                sb.Append(" and a.PAY_DT >= @qry_pay_dt_s ");
                ht.Add("@qry_pay_dt_s", qry_pay_dt_s);
            }
            if (qry_pay_dt_e != "")
            {
                sb.Append(" and a.PAY_DT <= @qry_pay_dt_e ");
                ht.Add("@qry_pay_dt_e", qry_pay_dt_e);
            }
            if (salary_type != "")
            {
                sb.Append(" and a.SALARY_TYPE = @salary_type ");
                ht.Add("@salary_type", salary_type);
            }
            if (qry_pay_id != "")
            {
                sb.Append(" and a.pay_id = @qry_pay_id ");
                ht.Add("@qry_pay_id", qry_pay_id);
            }
            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                t = (int)dt.Rows[0]["total_record"];
            }
            return t;
        }
        catch (Exception)
        {
            throw;
        }
    }
    //檢查計算保費之種類是否已被薪資擔當鎖定,若已鎖定不允重新計算
    internal DataTable getS_M_VOUCHER(string p_pay_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select count(*) resultCount from TB_S_M_SALARY_VOUCHER");
            sb.Append(" where PAY_ID=@p_pay_id and isnull(ACCT_ID,'')<>'' ");
            ht.Add("@p_pay_id", p_pay_id);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //檢查薪資差異解析表是否已有資料
    internal DataTable getANALYSIS(string salary_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select count(*) resultCount from TB_S_M_SALARY_ANALYSIS");
            sb.Append(" where SALARY_DT=@SALARY_DT ");
            ht.Add("@SALARY_DT", salary_dt);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //檢查關帳代號 是否已存在
    internal DataTable check_VOUCHER(string PAY_ID)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select count(*) resultCount from TB_S_M_SALARY_VOUCHER");
            sb.Append(" where PAY_ID=@PAY_ID ");
            ht.Add("@PAY_ID", PAY_ID);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //檢查是否已列印彙計表
    internal DataTable getSALARY_REPORT_D(string p_pay_id, string p_salary_type)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            if (p_salary_type.Equals("A"))
            {
                sb.Append("select count(*) resultCount from TB_S_M_SALARY_REPORT_D");
            }
            else
            {
                sb.Append("select count(*) resultCount from TB_S_M_SALARY_REPORT_O_D");
            }
            sb.Append(" where PAY_ID=@p_pay_id  ");
            ht.Add("@p_pay_id", p_pay_id);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

    internal int checkTB_S_M_SALARY_PAY_H(string pa_salary_type, string pa_salary_ym, string pa_salary_dt, string pa_pay_kind)
    {
        try
        {
            int row = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select count(*) resultCount from TB_S_M_SALARY_PAY_H");
            sb.Append(" where convert(varchar,SALARY_DT,111) = @SALARY_DT and SALARY_YM = @SALARY_YM and SALARY_TYPE = @SALARY_TYPE and PAY_KIND = @PAY_KIND  ");
            ht.Add("@SALARY_DT", pa_salary_dt);
            ht.Add("@SALARY_YM", pa_salary_ym);
            ht.Add("@SALARY_TYPE", pa_salary_type);
            ht.Add("@PAY_KIND", pa_pay_kind);

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
	        {
		        row = Convert.ToInt32( dt.Rows[0]["resultCount"].ToString());
	        }

            return row;
        }
        catch (Exception)
        {
            throw;
        }
    }
    
	//薪資月結1
    public string Month_Close_dao1(string pvSalary_type, string pvSalary_dt, string pvPay_kind, string pvProcess_status, string pSalary_ym)
    {
        try
        {
            string msg = "0";            
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" delete from  TB_S_M_SALARY_PAY  ");
            sb.Append(" where SALARY_DT=@pvSalary_dt and SALARY_TYPE=@pvSalary_type and PAY_KIND=@pvPay_kind ;");

            //1 薪資明細計算檔(TB_S_S_SALARY_PAY) 寫入 薪資明細歷史檔
            sb.Append(" insert into TB_S_M_SALARY_PAY (SALARY_DT,DATA_YM,SALARY_TYPE,EMP_ID,EMP_NAME,SALARY_ID ");
            sb.Append(",SALARY_NAME,AMOUNT,DATA_SRC,FORMULA,IS_PLUS,IS_TAX,TAX_FORMAT,PAY_KIND,PAY_TYPE,INCOME_CD,PAY_DT,PAY_ID ");
            sb.Append(",CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID) ");
            sb.Append("select a.SALARY_DT,a.DATA_YM,a.SALARY_TYPE,a.EMP_ID,b.EMP_NAME,a.SALARY_ID ");
            sb.Append(",a.SALARY_NAME,a.AMOUNT,a.DATA_SRC,a.FORMULA,a.IS_PLUS,a.IS_TAX,a.TAX_FORMAT,a.PAY_KIND,a.PAY_TYPE,a.INCOME_CD,a.PAY_DT,a.PAY_ID ");
            sb.Append(",@user_id,getdate(),@user_id1,getdate(),'FB2SC260' ");
            sb.Append("from TB_S_S_SALARY_PAY a ");
            sb.Append("left join TB_H_M_EMP b on a.EMP_ID=b.EMP_ID ");
            sb.Append("where a.SALARY_DT=@pvSalary_dt and a.SALARY_TYPE=@pvSalary_type and a.PAY_KIND=@pvPay_kind ");
            ht.Add("@user_id", SessionHandle.Current.emp_id);
            ht.Add("@user_id1", SessionHandle.Current.emp_id);
            ht.Add("@pvSalary_dt", pvSalary_dt);
            ht.Add("@pvSalary_type", pvSalary_type);
            ht.Add("@pvPay_kind", pvPay_kind);
            dbConn.ExecuteT(sb, ht, true);
            
            return msg;
        }
        catch (Exception)
        {
            throw;            
        }
    }

    //薪資月結2
    public string Month_Close_dao2(string pvSalary_type, string pvSalary_dt, string pvPay_kind, string pvProcess_status, string pSalary_ym)
    {
        try
        {
            string msg = "0";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();          

            //2016/12/20 原價系統所需資料
            sb.Append(utilities.FB3ServerName + @".[FB3DB].dbo.SP_S_TB_MONTH_SALARY_DATA");
            ht.Add("@SALARY_TYPE", pvSalary_type);
            ht.Add("@SALARY_YM", pSalary_ym);
            ht.Add("@SALARY_DT", pvSalary_dt);
            ht.Add("@PAY_KIND", pvPay_kind);
            ht.Add("@USERID", SessionHandle.Current.emp_id);
            ht.Add("@FUNCID", "FB2SC260");
            dbConn.ExecuteSP(sb, ht, true);

            sb.AppendLine(" select top (1) PROC_STATUS, PROC_LOG from  ");
            sb.AppendLine(utilities.FB3ServerName + @".[FB3DB].dbo.TB_SP_LOG   ");
            sb.AppendLine("  where  PROC_ID = 'SP_S_TB_MONTH_SALARY_DATA'            ");
            sb.AppendLine("  order by PROC_DT desc                ");
            DataTable spt = dbConn.Query(sb, ht, true);
            if (spt.Rows.Count > 0)
            {
                if (spt.Rows[0]["PROC_STATUS"].ToString() != "Y")
                {
                    return spt.Rows[0]["PROC_LOG"].ToString();
                }
            }
            return msg;
        }
        catch (Exception)
        {
            throw;
        }
    }

    //薪資月結3
    public string Month_Close_dao3(string pvSalary_type, string pvSalary_dt, string pvPay_kind, string pvProcess_status, string pSalary_ym)
    {
        try
        {
            string msg = "0";
            decimal subamt = 0;
            decimal amt = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();            
            //2 以依點選的資料列發薪類別+發薪日期+發放項目+關帳代號+薪資項目代號='1019'(自強基金)條件,讀取  薪資明細計算檔(TB_S_S_SALARY_PAY) 資料;取得資料後,逐筆 取出工號資料
            // 以工號讀取  個人別工會會費繳費次數檔(TB_S_UNION_FEE_DATA) 若存在則更新 繳費次數(PAY_CNT) = PAY_CNT+1;
            sb.Append(" Update TB_S_M_UNION_FEE_DATA Set PAY_CNT += 1,UPDATED_BY=@user_id,UPDATED_DT=GETDATE() From TB_S_M_UNION_FEE_DATA a ");
            sb.Append(" Inner Join TB_S_S_SALARY_PAY b On a.EMP_ID=b.EMP_ID and b.SALARY_TYPE = @pvSalary_type ");
            sb.Append(" and b.PAY_KIND=@pvPay_kind and b.SALARY_ID='1019' ");
            ht.Add("@user_id", SessionHandle.Current.emp_id);
            ht.Add("@pvSalary_dt", pvSalary_dt);
            ht.Add("@pvSalary_type", pvSalary_type);
            ht.Add("@pvPay_kind", pvPay_kind);
            dbConn.ExecuteT(sb, ht, true);
            sb.Clear();
            ht.Clear();
            //3 若不存在則 新增個人別工會會費繳費次數檔(TB_S_UNION_FEE_DATA)
            sb.Append(" Insert Into TB_S_M_UNION_FEE_DATA (EMP_ID,EMP_NAME,PAY_CNT,START_YM,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" Select a.EMP_ID,b.EMP_NAME,1,@pSalary_ym,@USER_ID,GETDATE(),'FB2SC260' ");
            sb.Append(" From  TB_S_S_SALARY_PAY a Inner Join VW_H_EMP_DATA b On a.EMP_ID=b.EMP_ID ");
            sb.Append(" Where a.SALARY_TYPE = @pvSalary_type and a.PAY_KIND=@pvPay_kind and a.SALARY_ID='1019' ");
            sb.Append(" and a.EMP_ID Not In (Select EMP_ID From TB_S_M_UNION_FEE_DATA ) ");
            ht.Add("@user_id", SessionHandle.Current.emp_id);
            ht.Add("@pvSalary_dt", pvSalary_dt);
            ht.Add("@pvSalary_type", pvSalary_type);
            ht.Add("@pvPay_kind", pvPay_kind);
            ht.Add("@pSalary_ym", pSalary_ym);
            dbConn.ExecuteT(sb, ht, true);
            sb.Clear();
            ht.Clear();

            //5 取得 個人健保補充保費扣繳暫存檔(TB_S_M_INS2_DETAIL_TMP) 後 ,寫入個人健保補充保費扣繳檔(TB_S_M_INS2_DETAIL) 資料如下:
            sb.Append(" insert into TB_S_M_INS2_DETAIL (PAYMENT_DATE,DATA_SOURCE,SALARY_TYPE,SALARY_ID,PAY_KIND,LICENSE_ID,EMP_ID, ");
            sb.Append(" EMP_CD,INS_MONTH_AMOUNT,FOUR_AMOUNT,ONE_TIME_AMOUNT,ACCU_AMOUNT,ACCU_OVER_AMOUNT, ");
            sb.Append(" INS_COST_BASE,INS_COST,INS_COST_YM,FINISH_INS_COST_DT,IS_NOT_CAL,SALARY_COUNT_TYPE,");
            sb.Append(" CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" select a.SALARY_DT,'A',a.SALARY_TYPE,a.SALARY_ID,a.PAY_KIND,b.LICENSE_ID,a.EMP_ID,");
            sb.Append(" a.EMP_CD,a.INS_LEVEL,a.INS_LEVEL_4TIMES,a.AMOUNT,a.ACCU_AMOUNT,a.ACCU_AMT_OVER,");
            sb.Append(" a.INS2_BASE_AMT,a.INS2_AMT,a.INS2_YYMM,a.INS2_COUNT_DATE,a.NONPAY_CAT,a.SALARY_COUNT_TYPE,");
            sb.Append(" @user_id,GETDATE(),@user_id1,GETDATE(),'FB2SC260'");
            sb.Append(" from TB_S_M_INS2_DETAIL_TMP a");
            sb.Append(" left join TB_H_M_EMP b on a.EMP_ID=b.EMP_ID");
            sb.Append(" where a.SALARY_DT=@pvSalary_dt and a.SALARY_TYPE=@pvSalary_type and a.PAY_KIND=@pvPay_kind ");
            ht.Add("@user_id", SessionHandle.Current.emp_id);
            ht.Add("@user_id1", SessionHandle.Current.emp_id);
            ht.Add("@pvSalary_dt", pvSalary_dt);
            ht.Add("@pvSalary_type", pvSalary_type);
            ht.Add("@pvPay_kind", pvPay_kind);
            dbConn.ExecuteT(sb, ht, true);
            sb.Clear();
            ht.Clear();
            //6 刪除 個人健保補充保費扣繳暫存檔(TB_S_M_INS2_DETAIL_TMP) 
            sb.Append("delete from TB_S_M_INS2_DETAIL_TMP where SALARY_DT=@pvSalary_dt and SALARY_TYPE=@pvSalary_type and PAY_KIND=@pvPay_kind ");
            ht.Add("@pvSalary_dt", pvSalary_dt);
            ht.Add("@pvSalary_type", pvSalary_type);
            ht.Add("@pvPay_kind", pvPay_kind);
            dbConn.ExecuteT(sb, ht, true);

            //7 寫入(員工欠薪還款明細檔)TB_S_M_STAFF_REPAYMENT_D
            sb.Append(" insert into TB_S_M_STAFF_REPAYMENT_D (EMP_ID,DEBIT_DT,SALARY_DT,SALARY_TYPE,REPAY_YM,ORG_AMT,REPAY_AMT,SALARY_ID ");
            sb.Append(" ,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" select EMP_ID,DEBIT_DT,SALARY_DT,SALARY_TYPE,REPAY_YM,ORG_AMT,REPAY_AMT,@pvPay_kind,");
            sb.Append(" @user_id,GETDATE(),@user_id1,GETDATE(),'FB2SC260'");
            sb.Append(" from TB_S_M_STAFF_REPAY_TMP ");
            sb.Append(" where SALARY_DT=@pvSalary_dt and SALARY_TYPE=@pvSalary_type and salary_id = @pvPay_kind  ");
            ht.Add("@pvPay_kind", pvPay_kind);
            ht.Add("@user_id", SessionHandle.Current.emp_id);
            ht.Add("@user_id1", SessionHandle.Current.emp_id);
            ht.Add("@pvSalary_dt", pvSalary_dt);
            ht.Add("@pvSalary_type", pvSalary_type);
            dbConn.ExecuteT(sb, ht, true);
            sb.Clear();
            ht.Clear();

            //7-1 寫入(員工欠薪明細檔)TB_S_M_STAFF_ARREARS_D  1041 -->員工欠款代墊  BY EVA ADD 2015/6/22
            sb.Append(" insert into TB_S_M_STAFF_ARREARS_D (EMP_ID,DEBIT_DT,SALARY_YM,AMOUNT ");
            sb.Append(" ,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" select EMP_ID,SALARY_DT,DATA_YM,AMOUNT,");
            sb.Append(" @user_id,GETDATE(),@user_id1,GETDATE(),'FB2SC260'");
            sb.Append(" from TB_S_S_SALARY_PAY ");
            sb.Append(" where SALARY_DT=@pvSalary_dt and SALARY_TYPE=@pvSalary_type and PAY_KIND=@pvPay_kind AND salary_id='1041' ");
            ht.Add("@user_id", SessionHandle.Current.emp_id);
            ht.Add("@user_id1", SessionHandle.Current.emp_id);
            ht.Add("@pvSalary_dt", pvSalary_dt);
            ht.Add("@pvSalary_type", pvSalary_type);
            ht.Add("@pvPay_kind", pvPay_kind);
            dbConn.ExecuteT(sb, ht, true);
            sb.Clear();
            ht.Clear();

            //8 UPDATE 員工欠薪主檔 (TB_S_M_STAFF_ARREARS_H) 2016/06/01 TERRY MODIFY 主檔須再合計已還款金額
            sb.Append(" select EMP_ID,CONVERT(char(10), DEBIT_DT, 111) as sDEBIT_DT,sum(REPAY_AMT) as TTAMT from TB_S_M_STAFF_REPAY_TMP");
            sb.Append("  where SALARY_DT=@pvSalary_dt and SALARY_TYPE=@pvSalary_type ");
            sb.Append("  group by  EMP_ID,DEBIT_DT ");
            ht.Add("@pvSalary_dt", pvSalary_dt);
            ht.Add("@pvSalary_type", pvSalary_type);
            DataTable dt = dbConn.QueryT(sb, ht);
            sb.Clear();
            ht.Clear();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                //subamt = (decimal)dt.Rows[i]["TTAMT"];//此次還款金額 這個金額已經insert到TB_S_M_STAFF_REPAYMENT_D 故不必再累計
                //TERRY 取得總還款金額
                sb.Append(" select EMP_ID,CONVERT(char(10), DEBIT_DT, 111) as sDEBIT_DT,sum(REPAY_AMT) as AMT from TB_S_M_STAFF_REPAYMENT_D");
                sb.Append(" where EMP_ID=@EMP_ID and DEBIT_DT=@DEBIT_DT ");
                sb.Append(" group by  EMP_ID,DEBIT_DT ");

                ht.Add("@EMP_ID", dt.Rows[i]["EMP_ID"].ToString());
                ht.Add("@DEBIT_DT", dt.Rows[i]["sDEBIT_DT"].ToString());
                DataTable dt2 = dbConn.QueryT(sb, ht, true);
                if (dt2.Rows.Count > 0)
                {
                    amt = (decimal)dt2.Rows[0]["AMT"];//已還款金額
                    subamt = amt;
                }
                else
                {
                    subamt = 0;
                }
                sb.Clear();
                ht.Clear();
                //TERRY MODIFY END

                sb.Append(" update TB_S_M_STAFF_ARREARS_H set TOTAL_AMT=@AMT ,UPDATED_BY=@user_id,UPDATED_DT=getdate(),FUNC_ID='FB2SC260'");
                sb.Append(" where EMP_ID=@EMP_ID and DEBIT_DT=@DEBIT_DT ");
                ht.Add("@AMT", subamt);
                ht.Add("@user_id", SessionHandle.Current.emp_id);
                ht.Add("@EMP_ID", dt.Rows[i]["EMP_ID"].ToString());
                ht.Add("@DEBIT_DT", dt.Rows[i]["sDEBIT_DT"].ToString());
                dbConn.ExecuteT(sb, ht, true);
                sb.Clear();
                ht.Clear();
            }
            sb.Append(" update TB_S_M_STAFF_ARREARS_H set IS_VAILD='N'");
            sb.Append(" where IS_VAILD='Y' and AMOUNT<=TOTAL_AMT ");
            dbConn.ExecuteT(sb, ht, true);
            sb.Clear();
            ht.Clear();

            //update by Terry 20161206 (若有2種以上薪水在同一天發..發生於同一人身上欠薪..會KEY值重複)
            //8-1 UPDATE 員工欠薪主檔 (TB_S_M_STAFF_ARREARS_H)  by eva 2015/06/22 
            sb.Append(" select EMP_ID,convert(varchar(10),SALARY_DT,111) SALARY_DT,AMOUNT from TB_S_S_SALARY_PAY");
            sb.Append("  where SALARY_DT=@pvSalary_dt and SALARY_TYPE=@pvSalary_type and PAY_KIND=@pvPay_kind AND salary_id='1041'");

            ht.Add("@pvSalary_dt", pvSalary_dt);
            ht.Add("@pvSalary_type", pvSalary_type);
            ht.Add("@pvPay_kind", pvPay_kind);
            DataTable dt1 = dbConn.QueryT(sb, ht);
            sb.Clear();
            ht.Clear();

            amt = 0;
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                //TERRY 取得是否已有資料列
                sb.Append(" select EMP_ID,convert(varchar(10),DEBIT_DT,111) DEBIT_DT ,AMOUNT from TB_S_M_STAFF_ARREARS_H");
                sb.Append(" where EMP_ID=@EMP_ID and DEBIT_DT=convert(varchar(10),@DEBIT_DT,111) ");

                ht.Add("@EMP_ID", dt1.Rows[i]["EMP_ID"].ToString());
                ht.Add("@DEBIT_DT", dt1.Rows[i]["SALARY_DT"].ToString());
                DataTable dt2 = dbConn.QueryT(sb, ht, true);
                if (dt2.Rows.Count > 0)
                {
                    amt = (decimal)dt1.Rows[i]["AMOUNT"] + (decimal)dt2.Rows[i]["AMOUNT"];
                    //已有資料則用update
                    sb.Append(" update TB_S_M_STAFF_ARREARS_H set AMOUNT=@AMT ,UPDATED_BY=@user_id,UPDATED_DT=getdate(),FUNC_ID='FB2SC260'");
                    sb.Append(" where EMP_ID=@EMP_ID and DEBIT_DT=convert(varchar(10),@DEBIT_DT,111) ");
                    ht.Add("@AMT", amt);
                    ht.Add("@user_id", SessionHandle.Current.emp_id);
                    ht.Add("@EMP_ID", dt1.Rows[i]["EMP_ID"].ToString());
                    ht.Add("@DEBIT_DT", dt1.Rows[i]["SALARY_DT"].ToString());
                    dbConn.ExecuteT(sb, ht, true);
                    sb.Clear();
                    ht.Clear();
                }
                else
                {
                    //無資料則用insert
                    sb.Append(" insert into TB_S_M_STAFF_ARREARS_H (EMP_ID,DEBIT_DT,AMOUNT,TOTAL_AMT,ARREARS_TYPE ");
                    sb.Append(" ,CAL_ORDER,REPAY_TYPE,VALUE,REPAY_SRC,OTHER_COND");
                    sb.Append(" ,IS_VAILD,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
                    sb.Append(" values( @EMP_ID,@SALARY_DT,@AMOUNT,0,'1',");
                    sb.Append(" 1,'2',100,'3','', ");
                    sb.Append(" 'Y',@user_id,GETDATE(),@user_id1,GETDATE(),'FB2SC260')");
                    ht.Add("@EMP_ID", dt1.Rows[i]["EMP_ID"].ToString());
                    ht.Add("@SALARY_DT", dt1.Rows[i]["SALARY_DT"].ToString());
                    ht.Add("@AMOUNT", (decimal)dt1.Rows[i]["AMOUNT"]);
                    ht.Add("@user_id", SessionHandle.Current.emp_id);
                    ht.Add("@user_id1", SessionHandle.Current.emp_id);

                    dbConn.ExecuteT(sb, ht, true);
                    sb.Clear();
                    ht.Clear();
                }

                //TERRY MODIFY END


            }

            
            //9 delete 以資料列.發薪類別+發薪日期+發放項目 刪除 員工欠薪還款暫存檔(TB_S_M_STAFF_REPAY_TMP) 
            sb.Append("delete from TB_S_M_STAFF_REPAY_TMP where SALARY_DT=@pvSalary_dt and SALARY_TYPE=@pvSalary_type and SALARY_ID=@pvPay_kind ");
            ht.Add("@pvSalary_dt", pvSalary_dt);
            ht.Add("@pvSalary_type", pvSalary_type);
            ht.Add("@pvPay_kind", pvPay_kind);
            dbConn.ExecuteT(sb, ht, true);

            //10 更新 薪資關帳主檔(TB_S_M_SALARY_PAY_H) 
            sb.Append("update TB_S_M_SALARY_PAY_H set CLOSED_DT=getdate() where SALARY_DT=@pvSalary_dt and SALARY_TYPE=@pvSalary_type and PAY_KIND=@pvPay_kind ");
            ht.Add("@pvSalary_dt", pvSalary_dt);
            ht.Add("@pvSalary_type", pvSalary_type);
            ht.Add("@pvPay_kind", pvPay_kind);
            dbConn.ExecuteT(sb, ht, true);

            //11 異動薪資計算主檔(TB_S_SALARY_CAL_H) 
            if (pvProcess_status.Equals("3"))
            { //處理狀態 ='3'(關帳)
                sb.Append("UPDATE TB_S_M_SALARY_CAL_H SET PROCESS_STATUS='4' ,SALARY_CLOSED='Y',UPDATED_BY=@user_id,UPDATED_DT=GETDATE(),FUNC_ID='FB2SC260'  ");
                sb.Append("where SALARY_DT=@pvSalary_dt and SALARY_TYPE=@pvSalary_type and PAY_KIND=@pvPay_kind ");
                ht.Add("@user_id", SessionHandle.Current.emp_id);
                ht.Add("@pvSalary_dt", pvSalary_dt);
                ht.Add("@pvSalary_type", pvSalary_type);
                ht.Add("@pvPay_kind", pvPay_kind);
                dbConn.ExecuteT(sb, ht, true);
                sb.Clear();
                ht.Clear();
            }           
            return msg;
        }
        catch (Exception)
        {
            throw;
        }
    }
    
    //結轉傳票
    public void MarkVouch_dao(string salary_dt, string salary_type, string pay_kind, string pay_id, string tmc_pay_type, string other_remit_dt, string salary_ym
         , string company_cd1, string invno11, string invtype11, string intdt11
         , string company_cd2, string invno21, string invtype21, string intdt21, string iacyc, string iadat)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_S_SC2600_VOUCHER");

            //--@s_salary_dt 發薪日期,@salary_type 發薪類別,@pay_kind 發放項目,@pay_id 關帳代號
            //--@tmc_pay_type TMC付款,@other_remit_dt (媒體轉帳對象外) 實際匯款日, @salary_ym 薪資年月
            //--@company_cd1 聘用公司1,@invno11 支付發票號碼1,@invtype11 支付發票格式1,@intdt11 支付發票號日期
            //--@company_cd2 聘用公司2,invno21 支付發票號碼21,@invtype21 支付發票格式21,@intdt21 支付發票號日期
            //--IACYC	入帳週期

            ht.Add("@s_salary_dt", salary_dt);
            ht.Add("@salary_type", salary_type);
            ht.Add("@pay_kind", pay_kind);
            ht.Add("@pay_id", pay_id);
            ht.Add("@tmc_pay_type", tmc_pay_type);
            ht.Add("@other_remit_dt", other_remit_dt.Replace("/", ""));
            ht.Add("@salary_ym", salary_ym);
            ht.Add("@company_cd1", company_cd1);
            ht.Add("@invno11", invno11);
            ht.Add("@invtype11", invtype11);
            ht.Add("@invdt11", intdt11.Replace("/", ""));
            ht.Add("@company_cd2", company_cd2);
            ht.Add("@invno21", invno21);
            ht.Add("@invtype21", invtype21);
            ht.Add("@invdt21", intdt21.Replace("/", ""));
            ht.Add("@iacyc", iacyc);
            ht.Add("@qry_user_id", SessionHandle.Current.emp_id);
            ht.Add("@acc_dt", iadat.Replace("/", ""));
            dbConn.ExecuteSP(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }



    //結轉傳票(SAP)
    public string VOUCHER_SAP(string pay_id )
    {
        try
        {
            string rtnMessage = "";
            using (SqlConnection conn = new SqlConnection(utilities.connstr))
            using (SqlCommand comm = new SqlCommand())
            {
                conn.Open();
                comm.Connection = conn;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = "SP_S_SC2600_VOUCHER_SAP";
                comm.Parameters.AddWithValue("@pay_id", pay_id);
                comm.Parameters.AddWithValue("@gry_user_id", SessionHandle.Current.emp_id);
                comm.Parameters.Add("@P_ERR_MSG", SqlDbType.NVarChar, 600).Direction = ParameterDirection.Output;

                comm.ExecuteNonQuery();
                rtnMessage = (string)comm.Parameters["@P_ERR_MSG"].Value;
                conn.Close();
            }
            return  rtnMessage;

        }
        catch (Exception)
        {
            throw;
        }
    }

    //結轉傳票(SAP)檢查
    public string chek_SAP_DONE(string salary_type, string pay_id)
    {
        try
        {
            string rtnMessage = "";
            using (SqlConnection conn = new SqlConnection(utilities.connstr))
            using (SqlCommand comm = new SqlCommand())
            {
                conn.Open();
                comm.Connection = conn;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = "SP_S_SC2600_VOU_OUTCHK";
                comm.Parameters.AddWithValue("@salary_type", salary_type);
                comm.Parameters.AddWithValue("@pay_id", pay_id);
                comm.Parameters.Add("@P_ERR_MSG", SqlDbType.NVarChar, 600).Direction = ParameterDirection.Output;

                comm.ExecuteNonQuery();
                rtnMessage = (string)comm.Parameters["@P_ERR_MSG"].Value;
                conn.Close();
            }
            return rtnMessage;

        }
        catch (Exception)
        {
            throw;
        }
    }

    //結轉傳票_TOSAP
    public string SP_S_SC2600_VOUCHER_SAP0(string pay_id)
    {
        try
        {
            string rtnMessage = "";
            using (SqlConnection conn = new SqlConnection(utilities.connstr))
            using (SqlCommand comm = new SqlCommand())
            {
                conn.Open();
                comm.Connection = conn;
                comm.CommandType = CommandType.StoredProcedure;
                comm.CommandText = "SP_S_SC2600_VOUCHER_SAP0";
                comm.Parameters.AddWithValue("@pay_id", pay_id);
                comm.Parameters.AddWithValue("@gry_user_id", SessionHandle.Current.emp_id);
                comm.Parameters.Add("@P_ERR_MSG", SqlDbType.NVarChar, 600).Direction = ParameterDirection.Output;

                comm.ExecuteNonQuery();
                rtnMessage = (string)comm.Parameters["@P_ERR_MSG"].Value;
                conn.Close();
            }
            return rtnMessage;

        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable getLno()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select distinct Lno  from TB_S_M_SALARY_VOUCHER");
            sb.Append(" where PAY_ID = @PAY_ID ");

            ht.Add("@PAY_ID", PAY_ID);          

            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch
        {
            throw;
        }
    }

    public void getSys_cd()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select SYS_CD from TB_S_M_SALARY_NAME_DATA");
            sb.Append(" where SALARY_TYPE = @SALARY_TYPE and PAY_KIND = @PAY_KIND");

            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            ht.Add("@PAY_KIND", PAY_KIND);

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    SYS_CD = dt.Rows[0]["SYS_CD"].ToString();
                }
            }

        }
        catch
        {
            throw;
        }
    }

    public void getSEQ2(string SEQ_NO1)
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select isnull(RIGHT(REPLICATE('0', 4) + CAST(MAX(SEQ_NO2)  as NVARCHAR), 4) ,'') SEQ_NO2  from TB_S_M_VOUCHER_SEQ");
            sb.Append(" where SYS_CD = @SYS_CD and PAY_KIND = @PAY_KIND and IACYC = @IACYC and SEQ_NO1 = @SEQ_NO1");

            ht.Add("@SYS_CD", SYS_CD);
            ht.Add("@PAY_KIND", PAY_KIND);
            ht.Add("@IACYC", IACYC.Replace("/",""));
            ht.Add("@SEQ_NO1", SEQ_NO1);

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                SEQ_NO2 = dt.Rows[0]["SEQ_NO2"].ToString();
                if (SEQ_NO2 == "")
                {
                    SEQ_NO2 = "00001";
                }
            }
        }
        catch
        {
            throw;
        }
    }

    public DataTable getStatus()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select COUNT(*) bb from TB_S_M_SALARY_PAY_H ");
            sb.Append(" where  SALARY_TYPE=@salary_type AND SALARY_DT=@salary_dt and PAY_ID<>@pay_id and PAY_KIND = @PAY_KIND ");
            ht.Add("@salary_type", SALARY_TYPE);
            ht.Add("@salary_dt", Convert.ToDateTime(SALARY_DT).ToString("yyyy/MM/dd"));
            ht.Add("@pay_id", PAY_ID);
            ht.Add("@PAY_KIND", PAY_KIND);
            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch
        {
            throw;
        }
    }

    public DataTable getS1Data()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select distinct b.DATA_SCOPE,a.* from TB_S_S_SALARY_VOUCHER_D a  ");
            sb.Append(" left join TB_S_M_SALARY_GROUP_D b on a.GROUP_ID = b.GROUP_ID and GROUP_TYPE = @GROUP_TYPE  ");
            sb.Append(" where  DATA_TYPE='1' ");  //傳票類型=1.一般

            ht.Add("@GROUP_TYPE", SALARY_TYPE);

            DataTable dt = dbConn.QueryT(sb, ht);
            return dt;
        }
        catch
        {
            throw;
        }
    }

    public DataTable getS2Data()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            //20160607 改成支付傳票，每張一個受款人，可多筆            
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" select distinct b.ACCOUNTING_NO5,a.* from TB_S_S_SALARY_VOUCHER_D a  ");
            sb.Append(" left join TB_S_M_SALARY_GROUP_D b on a.GROUP_ID = b.GROUP_ID and GROUP_TYPE = @GROUP_TYPE and b.ACCOUNTING_NO1 = a.H016 ");
            sb.Append(" where  DATA_TYPE='2' ");  //支付傳票
            sb.Append(" order by VOUCHER_ID,H007 desc ");

            ht.Add("@GROUP_TYPE", SALARY_TYPE);

            DataTable dt = dbConn.QueryT(sb, ht);
            return dt;
        }
        catch
        {
            throw;
        }
    }

    public DataTable getVoucherTaxData()
    {
        DBConnector dbConn = new DBConnector();
        try
        {                    
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(@" with getVoucher as
                        (
                        select VOUCHER_ID,SUM(H025 * (case when H028 = 'D' then 1 else -1 end))Total_Amount,SUM(H026 * (case when H028 = 'D' then 1 else -1 end))Total_tax
                         from TB_S_S_SALARY_VOUCHER_D a
                        left join TB_S_M_SALARY_GROUP_D b on a.GROUP_ID = b.GROUP_ID and GROUP_TYPE = @GROUP_TYPE and b.ACCOUNTING_NO1 = a.H016 
                        where  DATA_TYPE='2' and H004 <> '' and LEN(H001) < 4 and H001 not like '50%' 
                        group by VOUCHER_ID
                        )
                        select ROW_NUMBER() OVER (PARTITION BY b.VOUCHER_ID order by b.VOUCHER_ID,b.H001 desc) ROWNO,a.Total_Amount,a.Total_tax,
                        c.ACCOUNTING_NO5,b.* from getVoucher a
                        left join TB_S_S_SALARY_VOUCHER_D b on a.VOUCHER_ID = b.VOUCHER_ID
                        left join TB_S_M_SALARY_GROUP_D c on c.GROUP_ID = b.GROUP_ID and GROUP_TYPE = @GROUP_TYPE and c.ACCOUNTING_NO1 = b.H016 
                        where  DATA_TYPE='2' and H004 <> '' and LEN(H001) < 4 and H001 not like '50%'  ");           

            ht.Add("@GROUP_TYPE", SALARY_TYPE);

            DataTable dt = dbConn.QueryT(sb, ht);
            return dt;
        }
        catch
        {
            throw;
        }
    }

    public DataTable getOriginalData()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(@" select VOUCHER_ID,SUM(H025 * (case when H028 = 'D' then 1 else -1 end)) Amount,SUM(H026 * (case when H028 = 'D' then 1 else -1 end))Total_tax
                         from TB_S_S_SALARY_VOUCHER_D a
                        left join TB_S_M_SALARY_GROUP_D b on a.GROUP_ID = b.GROUP_ID and GROUP_TYPE = @GROUP_TYPE and b.ACCOUNTING_NO1 = a.H016 
                        where  DATA_TYPE='2' and H004 <> '' and LEN(H001) < 4 and H001 not like '50%' 
                        group by VOUCHER_ID  ");

            ht.Add("@GROUP_TYPE", SALARY_TYPE);

            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch
        {
            throw;
        }
    }

    public DataTable getItemData()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(@" select MAX(Itm)Itm from TB_S_VOUCHER_TEMP
                         where Rpamtpes = @Rpamtpes");

            ht.Add("@Rpamtpes", Rpamtpes);

            DataTable dt = dbConn.QueryT(sb, ht);
            return dt;
        }
        catch
        {
            throw;
        }
    }

    public void updtae_VOUCHER_TEMP()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(@" update TB_S_VOUCHER_TEMP
                        set VochAmt = VochAmt + Vochtaxamt,
                            Vochtaxamt = 0,
                            OcryAmt = OcryAmt + Ocrytaxamt,
                            Ocrytaxamt = 0
                        where Rpamtpes = @Rpamtpes
            ");
            if (Dc == "D")
            {
                sb.Append(" and Dc = 'C' ");
            }
            else
            {
                sb.Append(" and Dc = 'D' ");
            }
            
            ht.Add("@Rpamtpes", Rpamtpes);           

            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }

    public void delete_TB_S_M_SALARY_VOUCHER(string pay_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" delete from TB_S_M_SALARY_VOUCHER where PAY_ID = @pay_id and ACCT_ID like '3X%' ;");


            ht.Add("@pay_id", pay_id);
           
            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }

    public void insert_TB_S_S_SALARY_VOUCHER_D()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(@" insert into TB_S_S_SALARY_VOUCHER_D (
                            DATA_TYPE,VOUCHER_ID,GROUP_ID,DEL_MARK,H001,
                            H002,H003,H004,H005,H006,
                            H007,H008,H009,H010,H011,
                            H012,H013,H014,H015,H016,
                            H017,H018,H019,H020,H021,
                            H022,H023,H024,H025,H026,
                            H027,H028,CREATED_BY,CREATED_DT,UPDATED_BY,
                            UPDATED_DT,FUNC_ID)
                         values 
                            (@DATA_TYPE,@VOUCHER_ID,@GROUP_ID,@DEL_MARK,@H001,
                            @H002,@H003,@H004,@H005,@H006,
                            @H007,@H008,@H009,@H010,@H011,
                            @H012,@H013,@H014,@H015,@H016,
                            @H017,@H018,@H019,@H020,@H021,
                            @H022,@H023,@H024,@H025,@H026,
                            @H027,@H028,@CREATED_BY,getdate(),@UPDATED_BY,
                            getdate(),@FUNC_ID)  ");

            ht.Add("@DATA_TYPE", DATA_TYPE);
            ht.Add("@VOUCHER_ID", VOUCHER_ID);
            ht.Add("@GROUP_ID", GROUP_ID);
            ht.Add("@DEL_MARK", DEL_MARK);
            ht.Add("@H001", H001);

            ht.Add("@H002", H002);
            ht.Add("@H003", H003);
            ht.Add("@H004", H004);
            ht.Add("@H005", H005);
            ht.Add("@H006", H006);

            ht.Add("@H007", H007);
            ht.Add("@H008", H008);
            ht.Add("@H009", H009);
            ht.Add("@H010", H010);
            ht.Add("@H011", H011);

            ht.Add("@H012", H012);
            ht.Add("@H013", H013);
            ht.Add("@H014", H014);
            ht.Add("@H015", H015);
            ht.Add("@H016", H016);

            ht.Add("@H017", H017);
            ht.Add("@H018", H018);
            ht.Add("@H019", H019);
            ht.Add("@H020", H020);
            ht.Add("@H021", H021);

            ht.Add("@H022", H022);
            ht.Add("@H023", H023);
            ht.Add("@H024", H024);
            ht.Add("@H025", H025);
            ht.Add("@H026", H026);

            ht.Add("@H027", H027);
            ht.Add("@H028", H028);
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SC260");

           
           
            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }

    public void delete_TB_S_S_SALARY_VOUCHER_D_3X()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(@" delete from TB_S_S_SALARY_VOUCHER_D 
                         where VOUCHER_ID like '3X%' ");
            
            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }

    public void insert_TB_S_M_SALARY_VOUCHER(string pay_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(@" insert into TB_S_M_SALARY_VOUCHER (PAY_ID,GROUP_ID,DEPT_ACCT_ID,ACCT_ID,Lno
                                      ,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)
                         select @pay_id,a.GROUP_ID,'',a.VOUCHER_ID,@BTSQNO,@qry_user_id,GETDATE(),@qry_user_id,GETDATE(),'WFB2C260' 
                         from (select distinct VOUCHER_ID,GROUP_ID from TB_S_S_SALARY_VOUCHER_D )a
                         where a.VOUCHER_ID like '3X%'  ");
            
            ht.Add("@pay_id", pay_id);
            ht.Add("@qry_user_id", SessionHandle.Current.emp_id);
            ht.Add("@BTSQNO", Lno);

            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }

    public string getName(string eid)
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            string st = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select EMP_NAME from TB_H_M_EMP  ");
            sb.Append(" where  emp_id = @eid ");  //財務擔當性名

            ht.Add("@eid", eid);

            DataTable dt = dbConn.QueryT(sb, ht);
            if (dt.Rows.Count > 0)
            {
                st = dt.Rows[0]["EMP_NAME"].ToString();
            }
            return st;
        }
        catch
        {
            throw;
        }
    }

    public void insertTB_S_VOUCHER_TEMP()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(@" insert into TB_S_VOUCHER_TEMP(
                            CO,SlyPrvdDtid,IaCyc,IaDat,Vochno,
                            Wtmen,WtmenNm,Rpamtpes,Pamennm,Cu,
                            Itm,Dc,Dp,BgDp,Sumr,
                            AcctUrId,Ca,RemSumr,Acct,Vchid,
                            Vchno,VochAmt,Vochtaxamt,Relno,Obj,
                            DdaAmt,Ddataxamt,Cucy,Exr,OcryAmt,
                            Ocrytaxamt,BkAcno,WrEdDat,StrnEntryMk,Padty,
                            Cserid,NcrDat,IncmTy,RcvPcAcid,Ckno,
                            PayTrm,IvDat,CkEdDat,CkBkId,CkBkAccno,
                            Clckno,CkTrm,PaySqno,PayMk,VochHcode,
                            TxEmp,TxDat,TxTm,Lno)
                        values (
                            @CO,@SYS_CD,@IACYC,@IaDat,@Vochno,
                            @Wtmen,@WtmenNm,@Rpamtpes,@Pamennm,@Cu,
                            @Itm,@Dc,@Dp,@BgDp,@Sumr,
                            @AUrId,@Ca,@RemSumr,@Acct,@Vchid,
                            @Vchno,@VochAmt,@Vochtaxamt,@Relno,@Obj,
                            @DdaAmt,@Ddataxamt,@Cucy,@Exr,@OcryAmt,
                            @Ocrytaxamt,@BkAcno,@WrEdDat,@StrnEntryMk,@Padty,
                            @Cserid,@NcrDat,@IncmTy,@RcvPcAcid,@Ckno,
                            @PayTrm,@IvDat,@CkEdDat,@CkBkId,@CkBkAccno,
                            @Clckno,@CkTrm,@PaySqno,@PayMk,@VochHcode,
                            @TxEmp,@TxDat,@TxTm,@Lno)  ");


            ht.Add("@CO", CO);
            ht.Add("@SYS_CD", SYS_CD);
            ht.Add("@IACYC", IACYC.Replace("/", ""));
            ht.Add("@IaDat", IaDat.Replace("/", ""));
            ht.Add("@Vochno", Vochno);

            ht.Add("@Wtmen", Wtmen);
            ht.Add("@WtmenNm", WtmenNm);
            ht.Add("@Rpamtpes", Rpamtpes);
            ht.Add("@Pamennm", Pamennm);
            ht.Add("@Cu", Cu);

            ht.Add("@Itm", Itm);
            ht.Add("@Dc", Dc);
            ht.Add("@Dp", Dp);
            ht.Add("@BgDp", BgDp);
            ht.Add("@Sumr", Sumr);

            ht.Add("@AUrId", AcctUrId);
            ht.Add("@Ca", Ca);
            ht.Add("@RemSumr", RemSumr);
            ht.Add("@Acct", Acct);
            ht.Add("@Vchid", Vchid);

            ht.Add("@Vchno", Vchno);
            ht.Add("@VochAmt", VochAmt);
            ht.Add("@Vochtaxamt", Vochtaxamt);
            ht.Add("@Relno", Relno);
            ht.Add("@Obj", Obj);

            ht.Add("@DdaAmt", DdaAmt);
            ht.Add("@Ddataxamt", Ddataxamt);
            ht.Add("@Cucy", Cucy);
            ht.Add("@Exr", Exr);
            ht.Add("@OcryAmt", OcryAmt);

            ht.Add("@Ocrytaxamt", Ocrytaxamt);
            ht.Add("@BkAcno", BkAcno);
            if (WrEdDat == "")
            {
                ht.Add("@WrEdDat", DBNull.Value);
            }
            else
            {
                ht.Add("@WrEdDat", WrEdDat);
            }
            
            ht.Add("@StrnEntryMk", StrnEntryMk);
            ht.Add("@Padty", Padty);

            ht.Add("@Cserid", Cserid);
            if (NcrDat == "")
            {
                ht.Add("@NcrDat", DBNull.Value);
            }
            else
            {
                ht.Add("@NcrDat", NcrDat);
            }
           
            ht.Add("@IncmTy", IncmTy);
            ht.Add("@RcvPcAcid", RcvPcAcid);
            ht.Add("@Ckno", Ckno);

            ht.Add("@PayTrm", PayTrm);
            if (IvDat == "")
            {
                ht.Add("@IvDat", DBNull.Value);
            }
            else
            {
                ht.Add("@IvDat", IvDat);
            }
            
            if (CkEdDat == "")
            {
                ht.Add("@CkEdDat", DBNull.Value);
            }
            else
            {
                ht.Add("@CkEdDat", CkEdDat);
            }
           
            ht.Add("@CkBkId", CkBkId);
            ht.Add("@CkBkAccno", CkBkAccno);

            ht.Add("@Clckno", Clckno);
            ht.Add("@CkTrm", CkTrm);
            ht.Add("@PaySqno", PaySqno);
            ht.Add("@PayMk", PayMk);
            ht.Add("@VochHcode", VochHcode);

            ht.Add("@TxEmp", SessionHandle.Current.emp_id);
            ht.Add("@TxDat", DateTime.Now.ToString("yyyyMMdd"));
            ht.Add("@TxTm", DateTime.Now.ToString("HHmmss") + "00");
            ht.Add("@Lno", Lno);


            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    /*沖轉的一般傳票 */
    public DataTable getD_Mark1Data()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select distinct b.DATA_SCOPE,a.* from TB_S_S_SALARY_VOUCHER_D a  ");
            sb.Append(" left join TB_S_M_SALARY_GROUP_D b on a.GROUP_ID = b.GROUP_ID and GROUP_TYPE = @GROUP_TYPE  ");
            sb.Append(" where DEL_MARK='Y' and DATA_TYPE='1' ");  //傳票類型=1.一般

            ht.Add("@GROUP_TYPE", SALARY_TYPE);

            DataTable dt = dbConn.QueryT(sb, ht);
            return dt;
        }
        catch
        {
            throw;
        }
    }

    public DataTable getD_MarkOtherData()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select distinct b.DATA_SCOPE,a.* from TB_S_S_SALARY_VOUCHER_D a  ");
            sb.Append(" left join TB_S_M_SALARY_GROUP_D b on a.GROUP_ID = b.GROUP_ID and GROUP_TYPE = @GROUP_TYPE  ");
            sb.Append(" where DEL_MARK <> 'Y' and a.GROUP_ID in ('DA107','DB106','DC108') ");  

            ht.Add("@GROUP_TYPE", SALARY_TYPE);

            DataTable dt = dbConn.QueryT(sb, ht);
            return dt;
        }
        catch
        {
            throw;
        }
    }

    public DataTable getVouData()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select ACCT_ID from TB_S_M_SALARY_VOUCHER a  ");
            sb.Append(" where GROUP_ID = @GROUP_ID ");

            ht.Add("@GROUP_ID", GROUP_ID);

            DataTable dt = dbConn.QueryT(sb, ht);
            return dt;
        }
        catch
        {
            throw;
        }
    }
    
    public void delete_VOUCHER_TEMP()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();           

            sb.Append(" delete from TB_S_VOUCHER_TEMP where Lno = @Lno and Vochno = @Vochno ;");


            ht.Add("@Lno", Lno);            
            ht.Add("@Vochno", ACCT_ID);

            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }
    

    public void delete_VOUCHER()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" delete from TB_S_M_SALARY_VOUCHER where Lno = @Lno and GROUP_ID = @GROUP_ID  and ACCT_ID = @ACCT_ID ;");    
            

            ht.Add("@Lno", Lno);
            ht.Add("@GROUP_ID", GROUP_ID);
            ht.Add("@ACCT_ID", ACCT_ID);    

            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }

    public void deleteFB_TB_S_VOUCHER_TEMP()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" delete from TB_S_VOUCHER_TEMP");


            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable getSQLLNO(string Lno, string TblId)
    {
        try
        {
            dbConn.OtherCommStr = utilities.FF1connstr;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //sb.Append(" select count(*) as dtt ");
            sb.Append(" SELECT distinct isnull(GetChveMrtMk,'') GetChveMrtMk,isnull(AvWgtcmpsMk,'') AvWgtcmpsMk");
            sb.Append(" FROM SQLLNO ");
            sb.Append(" where Lno = @Lno and TblId = @TblId ");

            ht.Add("@Lno", Lno);
            ht.Add("@TblId", TblId);

            DataTable dt1 = dbConn.Query(sb, ht);

            dbConn.OtherCommStr = "";
            return dt1;
        }
        catch
        {
            throw;
        }
    }

    public void insertTB_S_M_VOUCHER_SEQ()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" delete from TB_S_M_VOUCHER_SEQ");
            sb.Append(" where SYS_CD = @SYS_CD and PAY_KIND = @PAY_KIND and IACYC = @IACYC ;");

            sb.Append(" insert into TB_S_M_VOUCHER_SEQ");
            sb.Append(" (SYS_CD,PAY_KIND,IACYC,SEQ_NO1,");
            sb.Append(" SEQ_NO2,LNO,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,");
            sb.Append(" FUNC_ID)");
            sb.Append(" select @SYS_CD ,@PAY_KIND ,@IACYC , LEFT(VOUCHER_ID,2)");
            sb.Append(" ,Convert(int,MAX(RIGHT(VOUCHER_ID,5))) ,@LNO,@CREATED_BY,getdate(),@UPDATED_BY,getdate() ");
            sb.Append(" ,@FUNC_ID from TB_S_S_SALARY_VOUCHER_D");
            sb.Append(" group by LEFT(VOUCHER_ID,2)");

            ht.Add("@SYS_CD", SYS_CD);
            ht.Add("@PAY_KIND", PAY_KIND);
            ht.Add("@IACYC", IACYC.Replace("/", ""));
            ht.Add("@SEQ_NO1", A_VOUCHER_SEQ1);
            //ht.Add("@SEQ_NO2", Convert.ToString(Convert.ToInt32(Itm)));
            ht.Add("@LNO", Lno);
            ht.Add("@CREATED_BY", CREATED_BY);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }

    /* 20201214
    public void RunSP_I_FF1_VOUCHER()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(utilities.FF1ServerName + @".[FF1DB].dbo.SP_I_FF1_VOUCHER");
            if (string.IsNullOrEmpty(SlyPrvdDtid))
                ht.Add("@SlyPrvdDtid", DBNull.Value);
            else
                ht.Add("@SlyPrvdDtid", SlyPrvdDtid);

            if (string.IsNullOrEmpty(Lno))
                ht.Add("@Lno", DBNull.Value);
            else
                ht.Add("@Lno", Lno);

            ht.Add("@USERID", SessionHandle.Current.emp_id);
            ht.Add("@FUNCID", "FB2SC260");
            ht.Add("@ERROR_FLAG", "");

            dbConn.ExecuteSP(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    */

    /*20201214
    public DataTable checkSP()
    {

        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select top (1) ERROR_FLAG, LOG_CONTENT from  ");
            sb.AppendLine(utilities.FF1ServerName + @".[FF1DB].dbo.TB_S_TRANS_LOG   ");
            sb.AppendLine("  where  LOG_ID = @LOG_ID            ");
            sb.AppendLine("  order by LOG_DATE desc                ");
            ht.Add("@LOG_ID", "FB2SC260");
            return dbConn.Query(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }

     */

    /* 20201214
    public DataTable getPaymentData(string VndNo)
    {

        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(" select Padty,PayTrm from  ");
            sb.AppendLine(utilities.FF1ServerName + @".[FF1DB].dbo.T15060FAAB1   ");
            sb.AppendLine("  where  VndNo = @VndNo            ");

            ht.Add("@VndNo", VndNo);
            return dbConn.QueryT(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }
    */

    public DataTable getComCode(string st)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select sub_desc From TB_9_M_COMM_D ");
            sb.Append(" Where main_cd = 'VOU_VENDOR_CD' and SYS_CD = 'SC' and CODE_VAL1 = @CODE_VAL1 and IS_VALID = 'Y' ");
            ht.Add("@CODE_VAL1", st);
            return dbConn.QueryT(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //insert 集計資料
    public void AS400_INSERT_DCCC26_28_29WS(string p_salary_type, string p_salary_dt, string p_pay_id)
    {
        DBConnector_ODBC odbc = new DBConnector_ODBC(utilities.ODBCconnstr);  //as 400 連線
        try
        {
            string wkStatus = "1"; //wk狀態 1.一般

            
            OdbcCommand ocomm = new OdbcCommand();

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select COUNT(*) bb from TB_S_M_SALARY_PAY_H ");
            sb.Append(" where  SALARY_TYPE=@salary_type AND SALARY_DT=@salary_dt and PAY_ID<>@pay_id ");
            ht.Add("@salary_type", p_salary_type);
            ht.Add("@salary_dt", Convert.ToDateTime(p_salary_dt).ToString("yyyy/MM/dd"));
            ht.Add("@pay_id", p_pay_id);
            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                if ((int)dt.Rows[0]["bb"] > 1)
                {
                    wkStatus = "2"; //2.沖轉
                }
            }
            sb.Clear();
            ht.Clear();
            if (wkStatus.Equals("1"))
            {
                sb.Append(" select * from TB_S_S_SALARY_VOUCHER_D  ");
                sb.Append(" where  DATA_TYPE='1' ");  //傳票類型=1.一般

                ocomm.CommandText = "insert into CCCCLIB.DCCC82M (W26S01,W26S02,W26S03,W26S04,W26S05,W26S06,W26S07,W26S08,W26S09,W26S10,";
                ocomm.CommandText += "                              W26S11,W26S12,W26S13,W26S14,W26S15,W26S16,W26S17,W26S18,W26S19,W26S20,";
                ocomm.CommandText += "                              W26S21,W26S22,W26S23,W26S24,W26S25,W26S26,W26S31,W26S32)";
                ocomm.CommandText += " values (?,?,?,?,?,?,?,?,?,? ,?,?,?,?,?,?,?,?,?,?, ?,?,?,?,?,?,?,?)";

                DataTable dt26WS = dbConn.Query(sb, ht);
                for (int i = 0; i < dt26WS.Rows.Count; i++)
                {
                    //AS400 [DCCC26WS 一般傳票 作業暫存檔] 
                    ocomm.Parameters.Clear();
                    ocomm.Parameters.AddWithValue("", dt26WS.Rows[i]["H002"]);  // W26S01,COLHDG('單據種類')
                    ocomm.Parameters.AddWithValue("", dt26WS.Rows[i]["H003"]);  // W26S02,COLHDG('單據號碼')
                    ocomm.Parameters.AddWithValue("", dt26WS.Rows[i]["H004"]);  // W26S03,COLHDG('發票格式CD')
                    ocomm.Parameters.AddWithValue("", dt26WS.Rows[i]["H005"]);  // W26S04,COLHDG('營業稅課稅別')
                    ocomm.Parameters.AddWithValue("", dt26WS.Rows[i]["H006"]);  // W26S05,COLHDG('營業稅扣抵CD')
                    ocomm.Parameters.AddWithValue("", dt26WS.Rows[i]["H007"]);  //W26S06,COLHDG('支付對象CD')
                    ocomm.Parameters.AddWithValue("", dt26WS.Rows[i]["H008"]);  //W26S07,COLHDG('支付區分')
                    ocomm.Parameters.AddWithValue("", dt26WS.Rows[i]["H009"]);  //W26S08,COLHDG('希望領取日')
                    ocomm.Parameters.AddWithValue("", dt26WS.Rows[i]["H012"]);  // W26S09,COLHDG('貨幣CD')
                    ocomm.Parameters.AddWithValue("", dt26WS.Rows[i]["H013"]);  // W26S10,COLHDG('匯率')
                    ocomm.Parameters.AddWithValue("", "");  //W26S11,COLHDG('對帳號碼') 
                    ocomm.Parameters.AddWithValue("", dt26WS.Rows[i]["H014"]);  // W26S12,COLHDG('付款方式')
                    ocomm.Parameters.AddWithValue("", Convert.ToString(Convert.ToDecimal(dt26WS.Rows[i]["H025"]) * 100).PadLeft(14, '0'));  // W26S13,COLHDG('原幣金額計')
                    ocomm.Parameters.AddWithValue("", Convert.ToString(dt26WS.Rows[i]["H025"]).PadLeft(12, '0'));  //W26S14,COLHDG('台幣金額計')
                    ocomm.Parameters.AddWithValue("", Convert.ToString(dt26WS.Rows[i]["H026"]).PadLeft(12, '0'));  //W26S15,COLHDG('稅額計')
                    ocomm.Parameters.AddWithValue("", dt26WS.Rows[i]["H028"]);  //W26S16,COLHDG('借貸區分')
                    ocomm.Parameters.AddWithValue("", dt26WS.Rows[i]["H016"]);  //W26S17,COLHDG('預算CD')
                    ocomm.Parameters.AddWithValue("", dt26WS.Rows[i]["H017"]);  // W26S18,COLHDG('特性')
                    ocomm.Parameters.AddWithValue("", dt26WS.Rows[i]["H018"]);  //W26S19,COLHDG('案件CD')
                    ocomm.Parameters.AddWithValue("", dt26WS.Rows[i]["H019"]);  //W26S20,COLHDG('工廠CD')
                    ocomm.Parameters.AddWithValue("", dt26WS.Rows[i]["H020"]);  //W26S21,COLHDG('預算部門CD')
                    ocomm.Parameters.AddWithValue("", dt26WS.Rows[i]["H021"]);  //W26S22,COLHDG('負擔部門CD')
                    ocomm.Parameters.AddWithValue("", dt26WS.Rows[i]["H022"]);  //W26S23,COLHDG('大小車CD')
                    ocomm.Parameters.AddWithValue("", dt26WS.Rows[i]["H023"]);  //W26S24,COLHDG('車種區分')
                    ocomm.Parameters.AddWithValue("", dt26WS.Rows[i]["H024"]);  //W26S25,COLHDG('工程區分')
                    ocomm.Parameters.AddWithValue("", dt26WS.Rows[i]["H027"]);  //W26S26,COLHDG('摘要')
                    ocomm.Parameters.AddWithValue("", dt26WS.Rows[i]["VOUCHER_ID"]);  //W26S31,COLHDG('傳票群組號碼')
                    ocomm.Parameters.AddWithValue("", dt26WS.Rows[i]["VOUCHER_ID"]);  //W26S32,COLHDG('部門傳票號碼')
                    odbc.getDataTable(ocomm);
                }
                sb.Clear();
                sb.Append(" select * from TB_S_S_SALARY_VOUCHER_D  ");
                sb.Append(" where  DATA_TYPE='2' ");  //傳票類型=2.支付
                ocomm.CommandText = "insert into CCCCLIB.DCCC84M (W28S01,W28S02,W28S03,W28S04,W28S05,W28S06,W28S07,W28S08,W28S09,W28S10,";
                ocomm.CommandText += "                              W28S11,W28S12,W28S13,W28S14,W28S15,W28S16,W28S17,W28S18,W28S19,W28S20,";
                ocomm.CommandText += "                              W28S21,W28S22,W28S23,W28S24,W28S25,W28S26,W28S27,W28S28,W28S31,W28S32)";
                ocomm.CommandText += " values (?,?,?,?,?,?,?,?,?,? ,?,?,?,?,?,?,?,?,?,? ,?,?,?,?,?,?,?,?,?,?)";

                DataTable dt28WS = dbConn.Query(sb, ht);
                for (int i = 0; i < dt28WS.Rows.Count; i++)
                {
                    //AS400 [DCCC28WS 支付傳票轉入作業暫存檔] 
                    ocomm.Parameters.Clear();
                    ocomm.Parameters.AddWithValue("", dt28WS.Rows[i]["H001"]);  // W28S01,COLHDG('單據序號')
                    ocomm.Parameters.AddWithValue("", dt28WS.Rows[i]["H002"]);  // W28S02,COLHDG('單據種類')
                    ocomm.Parameters.AddWithValue("", dt28WS.Rows[i]["H003"]);  // W28S03,COLHDG('單據號碼')
                    ocomm.Parameters.AddWithValue("", dt28WS.Rows[i]["H004"]);  // W28S04,COLHDG('發票格式CD')
                    ocomm.Parameters.AddWithValue("", dt28WS.Rows[i]["H005"]);  // W28S05,COLHDG('營業稅課稅別')
                    ocomm.Parameters.AddWithValue("", dt28WS.Rows[i]["H006"]);  // W28S06,COLHDG('營業稅扣抵CD')
                    ocomm.Parameters.AddWithValue("", dt28WS.Rows[i]["H007"]);  //W28S07,COLHDG('支付對象CD')
                    ocomm.Parameters.AddWithValue("", dt28WS.Rows[i]["H008"]);  //W28S08,COLHDG('支付區分')
                    ocomm.Parameters.AddWithValue("", dt28WS.Rows[i]["H009"]);  //W28S09,COLHDG('希望領取日')
                    ocomm.Parameters.AddWithValue("", dt28WS.Rows[i]["H010"]);  //W28S10,COLHDG('發生期間起')
                    ocomm.Parameters.AddWithValue("", dt28WS.Rows[i]["H011"]);  //W28S11,COLHDG('發生期間迄')
                    ocomm.Parameters.AddWithValue("", dt28WS.Rows[i]["H012"]);  //W28S12,COLHDG('貨幣CD')
                    ocomm.Parameters.AddWithValue("", dt28WS.Rows[i]["H013"]);  // W28S13,COLHDG('匯率')
                    ocomm.Parameters.AddWithValue("", dt28WS.Rows[i]["H014"]);  // W28S14,COLHDG('付款方式')
                    ocomm.Parameters.AddWithValue("", dt28WS.Rows[i]["H015"]);  // W28S15,COLHDG('天期CD')
                    ocomm.Parameters.AddWithValue("", dt28WS.Rows[i]["H016"]);  //W28S16,COLHDG('預算CD')
                    ocomm.Parameters.AddWithValue("", dt28WS.Rows[i]["H017"]);  //W28S17,COLHDG('特性')
                    ocomm.Parameters.AddWithValue("", dt28WS.Rows[i]["H018"]);  //W28S18,COLHDG('案件CD')
                    ocomm.Parameters.AddWithValue("", dt28WS.Rows[i]["H019"]);  //W28S19,COLHDG('工廠CD')
                    ocomm.Parameters.AddWithValue("", dt28WS.Rows[i]["H020"]);  // W28S20,COLHDG('預算部門CD')
                    ocomm.Parameters.AddWithValue("", dt28WS.Rows[i]["H021"]);  //W28S21,COLHDG('負擔部門CD')
                    ocomm.Parameters.AddWithValue("", dt28WS.Rows[i]["H022"]);  //W28S22,COLHDG('大小車CD')
                    ocomm.Parameters.AddWithValue("", dt28WS.Rows[i]["H023"]);  //W28S23,COLHDG('車種區分')
                    ocomm.Parameters.AddWithValue("", dt28WS.Rows[i]["H024"]);  //W28S24,COLHDG('工程區分')
                    ocomm.Parameters.AddWithValue("", Convert.ToString(Convert.ToDecimal(dt28WS.Rows[i]["H025"]) * 100).PadLeft(14, '0'));  //W28S25,COLHDG('原幣金額')
                    ocomm.Parameters.AddWithValue("", Convert.ToString(dt28WS.Rows[i]["H025"]).PadLeft(12, '0'));  //W28S26,COLHDG('台幣金額')
                    ocomm.Parameters.AddWithValue("", Convert.ToString(dt28WS.Rows[i]["H026"]).PadLeft(12, '0'));  //W28S27,COLHDG('稅額')
                    ocomm.Parameters.AddWithValue("", dt28WS.Rows[i]["H027"]);  //W28S28,COLHDG('摘要')
                    ocomm.Parameters.AddWithValue("", dt28WS.Rows[i]["VOUCHER_ID"]);  //W28S31,COLHDG('傳票群組號碼')
                    ocomm.Parameters.AddWithValue("", dt28WS.Rows[i]["VOUCHER_ID"]);  //W28S32,COLHDG('部門傳票號碼')
                    odbc.getDataTable(ocomm);
                }
                sb.Clear();
                sb.Append(" select * from TB_S_S_SALARY_VOUCHER_D  ");
                sb.Append(" where  DATA_TYPE='3' ");  //傳票類型=3.收入
                ocomm.CommandText = "insert into CCCCLIB.DCCC85M (W29S01,W29S02,W29S03,W29S04,W29S05,W29S06,W29S07,W29S08,W29S09,W29S10,";
                ocomm.CommandText += "                              W29S11,W29S12,W29S13,W29S14,W29S15,W29S16,W29S17,W29S18,W29S19,W29S20,";
                ocomm.CommandText += "                              W29S21,W29S22,W29S23,W29S24,W29S25,W29S26,W29S31,W29S32)";
                ocomm.CommandText += " values (?,?,?,?,?,?,?,?,?,? ,?,?,?,?,?,?,?,?,?,? ,?,?,?,?,?,?,?,?)";

                DataTable dt29WS = dbConn.Query(sb, ht);
                for (int i = 0; i < dt29WS.Rows.Count; i++)
                {
                    //AS400 [DCCC85M 收入傳票轉入作業暫存檔] 
                    ocomm.Parameters.Clear();
                    ocomm.Parameters.AddWithValue("", dt29WS.Rows[i]["H001"]);  // W29S01,COLHDG('單據序號')
                    ocomm.Parameters.AddWithValue("", dt29WS.Rows[i]["H002"]);  // W29S02,COLHDG('單據種類')
                    ocomm.Parameters.AddWithValue("", dt29WS.Rows[i]["H003"]);  // W29S03,COLHDG('單據號碼')
                    ocomm.Parameters.AddWithValue("", dt29WS.Rows[i]["H004"]);  // W29S04,COLHDG('發票格式CD')
                    ocomm.Parameters.AddWithValue("", dt29WS.Rows[i]["H005"]);  // W29S05,COLHDG('營業稅課稅別')
                    ocomm.Parameters.AddWithValue("", dt29WS.Rows[i]["H006"]);  // W29S06,COLHDG('營業稅扣抵CD')
                    ocomm.Parameters.AddWithValue("", dt29WS.Rows[i]["H007"]);  //W29S07,COLHDG('支付對象CD')
                    ocomm.Parameters.AddWithValue("", dt29WS.Rows[i]["H009"]);  //W29S08,COLHDG('希望領取日')
                    ocomm.Parameters.AddWithValue("", dt29WS.Rows[i]["H010"]);  //W29S09,COLHDG('發生期間起')
                    ocomm.Parameters.AddWithValue("", dt29WS.Rows[i]["H011"]);  //W29S10,COLHDG('發生期間迄')
                    ocomm.Parameters.AddWithValue("", dt29WS.Rows[i]["H012"]);  //W29S11,COLHDG('貨幣CD')
                    ocomm.Parameters.AddWithValue("", dt29WS.Rows[i]["H013"]);  // W29S12,COLHDG('匯率')
                    ocomm.Parameters.AddWithValue("", dt29WS.Rows[i]["H014"]);  // W29S13,COLHDG('付款方式')
                    ocomm.Parameters.AddWithValue("", dt29WS.Rows[i]["H016"]);  //W29S14,COLHDG('預算CD')
                    ocomm.Parameters.AddWithValue("", dt29WS.Rows[i]["H017"]);  //W29S15,COLHDG('特性')
                    ocomm.Parameters.AddWithValue("", dt29WS.Rows[i]["H018"]);  //W29S16,COLHDG('案件CD')
                    ocomm.Parameters.AddWithValue("", dt29WS.Rows[i]["H019"]);  //W29S17,COLHDG('工廠CD')
                    ocomm.Parameters.AddWithValue("", dt29WS.Rows[i]["H020"]);  // W29S18,COLHDG('預算部門CD')
                    ocomm.Parameters.AddWithValue("", dt29WS.Rows[i]["H021"]);  //W29S19,COLHDG('負擔部門CD')
                    ocomm.Parameters.AddWithValue("", dt29WS.Rows[i]["H022"]);  //W29S20,COLHDG('大小車CD')
                    ocomm.Parameters.AddWithValue("", dt29WS.Rows[i]["H023"]);  //W29S21,COLHDG('車種區分')
                    ocomm.Parameters.AddWithValue("", dt29WS.Rows[i]["H024"]);  //W29S22,COLHDG('工程區分')
                    ocomm.Parameters.AddWithValue("", Convert.ToString(Convert.ToDecimal(dt29WS.Rows[i]["H025"]) * 100).PadLeft(14, '0'));  //W29S23,COLHDG('原幣金額')
                    ocomm.Parameters.AddWithValue("", Convert.ToString(dt29WS.Rows[i]["H025"]).PadLeft(12, '0'));  //W29S24,COLHDG('台幣金額')
                    ocomm.Parameters.AddWithValue("", Convert.ToString(dt29WS.Rows[i]["H026"]).PadLeft(12, '0'));  //W29S25,COLHDG('稅額')
                    ocomm.Parameters.AddWithValue("", dt29WS.Rows[i]["H027"]);  //W29S26,COLHDG('摘要')
                    ocomm.Parameters.AddWithValue("", dt29WS.Rows[i]["VOUCHER_ID"]);  //W29S31,COLHDG('傳票群組號碼')
                    ocomm.Parameters.AddWithValue("", dt29WS.Rows[i]["VOUCHER_ID"]);  //W29S32,COLHDG('部門傳票號碼')
                    odbc.getDataTable(ocomm);
                }
            } // end if (wkStatus.Equals("1")) 
            if (wkStatus.Equals("2")) //沖轉
            {
                sb.Append(" select * from TB_S_S_SALARY_VOUCHER_D  ");
                sb.Append(" where DEL_MARK='Y' and DATA_TYPE='1' ");  //傳票類型=1.一般
                ocomm.CommandText = "insert into CCCCLIB.DCCC82M (W26S01,W26S02,W26S03,W26S04,W26S05,W26S06,W26S07,W26S08,W26S09,W26S10,";
                ocomm.CommandText += "                              W26S11,W26S12,W26S13,W26S14,W26S15,W26S16,W26S17,W26S18,W26S19,W26S20,";
                ocomm.CommandText += "                              W26S21,W26S22,W26S23,W26S24,W26S25,W26S26,W26S31,W26S32)";
                ocomm.CommandText += " values (?,?,?,?,?,?,?,?,?,? ,?,?,?,?,?,?,?,?,?,?, ?,?,?,?,?,?,?,?)";

                DataTable dt26WS = dbConn.Query(sb, ht);
                for (int i = 0; i < dt26WS.Rows.Count; i++)
                {
                    //AS400 [DCCC82M 一般傳票 作業暫存檔] 
                    ocomm.Parameters.Clear();
                    ocomm.Parameters.AddWithValue("", dt26WS.Rows[i]["H002"]);  // W26S01,COLHDG('單據種類')
                    ocomm.Parameters.AddWithValue("", dt26WS.Rows[i]["H003"]);  // W26S02,COLHDG('單據號碼')
                    ocomm.Parameters.AddWithValue("", dt26WS.Rows[i]["H004"]);  // W26S03,COLHDG('發票格式CD')
                    ocomm.Parameters.AddWithValue("", dt26WS.Rows[i]["H005"]);  // W26S04,COLHDG('營業稅課稅別')
                    ocomm.Parameters.AddWithValue("", dt26WS.Rows[i]["H006"]);  // W26S05,COLHDG('營業稅扣抵CD')
                    ocomm.Parameters.AddWithValue("", dt26WS.Rows[i]["H007"]);  //W26S06,COLHDG('支付對象CD')
                    ocomm.Parameters.AddWithValue("", dt26WS.Rows[i]["H008"]);  //W26S07,COLHDG('支付區分')
                    ocomm.Parameters.AddWithValue("", dt26WS.Rows[i]["H009"]);  //W26S08,COLHDG('希望領取日')
                    ocomm.Parameters.AddWithValue("", dt26WS.Rows[i]["H012"]);  // W26S09,COLHDG('貨幣CD')
                    ocomm.Parameters.AddWithValue("", dt26WS.Rows[i]["H013"]);  // W26S10,COLHDG('匯率')
                    ocomm.Parameters.AddWithValue("", "");  //W26S11,COLHDG('對帳號碼') 
                    ocomm.Parameters.AddWithValue("", dt26WS.Rows[i]["H014"]);  // W26S12,COLHDG('付款方式')
                    ocomm.Parameters.AddWithValue("", Convert.ToString(Convert.ToDecimal(dt26WS.Rows[i]["H025"]) * 100).PadLeft(14, '0'));  // W26S13,COLHDG('原幣金額計')
                    ocomm.Parameters.AddWithValue("", Convert.ToString(dt26WS.Rows[i]["H025"]).PadLeft(12, '0'));  //W26S14,COLHDG('台幣金額計')
                    ocomm.Parameters.AddWithValue("", Convert.ToString(dt26WS.Rows[i]["H026"]).PadLeft(12, '0'));  //W26S15,COLHDG('稅額計')
                    if (dt26WS.Rows[i]["H028"].ToString().Equals("D"))
                    {  //因為沖轉故借貸相反
                        ocomm.Parameters.AddWithValue("", "C");  //W26S16,COLHDG('借貸區分')
                    }
                    else
                    {
                        ocomm.Parameters.AddWithValue("", "D");  //W26S16,COLHDG('借貸區分')
                    }
                    ocomm.Parameters.AddWithValue("", dt26WS.Rows[i]["H016"]);  //W26S17,COLHDG('預算CD')
                    ocomm.Parameters.AddWithValue("", dt26WS.Rows[i]["H017"]);  // W26S18,COLHDG('特性')
                    ocomm.Parameters.AddWithValue("", dt26WS.Rows[i]["H018"]);  //W26S19,COLHDG('案件CD')
                    ocomm.Parameters.AddWithValue("", dt26WS.Rows[i]["H019"]);  //W26S20,COLHDG('工廠CD')
                    ocomm.Parameters.AddWithValue("", dt26WS.Rows[i]["H020"]);  //W26S21,COLHDG('預算部門CD')
                    ocomm.Parameters.AddWithValue("", dt26WS.Rows[i]["H021"]);  //W26S22,COLHDG('負擔部門CD')
                    ocomm.Parameters.AddWithValue("", dt26WS.Rows[i]["H022"]);  //W26S23,COLHDG('大小車CD')
                    ocomm.Parameters.AddWithValue("", dt26WS.Rows[i]["H023"]);  //W26S24,COLHDG('車種區分')
                    ocomm.Parameters.AddWithValue("", dt26WS.Rows[i]["H024"]);  //W26S25,COLHDG('工程區分')
                    ocomm.Parameters.AddWithValue("", dt26WS.Rows[i]["H027"]);  //W26S26,COLHDG('摘要')
                    ocomm.Parameters.AddWithValue("", dt26WS.Rows[i]["VOUCHER_ID"]);  //W26S31,COLHDG('傳票群組號碼')
                    ocomm.Parameters.AddWithValue("", dt26WS.Rows[i]["VOUCHER_ID"]);  //W26S32,COLHDG('部門傳票號碼')
                    odbc.getDataTable(ocomm);
                }
                sb.Clear();
                sb.Append(" select * from TB_S_S_SALARY_VOUCHER_D  ");
                sb.Append(" where DEL_MARK<>'Y' and GROUP_ID in ('DA107','DB106','DC108') ");
                ocomm.CommandText = "insert into CCCCLIB.DCCC84M (W28S01,W28S02,W28S03,W28S04,W28S05,W28S06,W28S07,W28S08,W28S09,W28S10,";
                ocomm.CommandText += "                              W28S11,W28S12,W28S13,W28S14,W28S15,W28S16,W28S17,W28S18,W28S19,W28S20,";
                ocomm.CommandText += "                              W28S21,W28S22,W28S23,W28S24,W28S25,W28S26,W28S27,W28S28,W28S31,W28S32)";
                ocomm.CommandText += " values (?,?,?,?,?,?,?,?,?,? ,?,?,?,?,?,?,?,?,?,? ,?,?,?,?,?,?,?,?,?,?)";

                DataTable dt28WS = dbConn.Query(sb, ht);
                for (int i = 0; i < dt28WS.Rows.Count; i++)
                {
                    //AS400 [DCCC84M 支付傳票轉入作業暫存檔] 
                    ocomm.Parameters.Clear();
                    ocomm.Parameters.AddWithValue("", dt28WS.Rows[i]["H001"]);  // W28S01,COLHDG('單據序號')
                    ocomm.Parameters.AddWithValue("", dt28WS.Rows[i]["H002"]);  // W28S02,COLHDG('單據種類')
                    ocomm.Parameters.AddWithValue("", dt28WS.Rows[i]["H003"]);  // W28S03,COLHDG('單據號碼')
                    ocomm.Parameters.AddWithValue("", dt28WS.Rows[i]["H004"]);  // W28S04,COLHDG('發票格式CD')
                    ocomm.Parameters.AddWithValue("", dt28WS.Rows[i]["H005"]);  // W28S05,COLHDG('營業稅課稅別')
                    ocomm.Parameters.AddWithValue("", dt28WS.Rows[i]["H006"]);  // W28S06,COLHDG('營業稅扣抵CD')
                    ocomm.Parameters.AddWithValue("", dt28WS.Rows[i]["H007"]);  //W28S07,COLHDG('支付對象CD')
                    ocomm.Parameters.AddWithValue("", dt28WS.Rows[i]["H008"]);  //W28S08,COLHDG('支付區分')
                    ocomm.Parameters.AddWithValue("", dt28WS.Rows[i]["H009"]);  //W28S09,COLHDG('希望領取日')
                    ocomm.Parameters.AddWithValue("", dt28WS.Rows[i]["H010"]);  //W28S10,COLHDG('發生期間起')
                    ocomm.Parameters.AddWithValue("", dt28WS.Rows[i]["H011"]);  //W28S11,COLHDG('發生期間迄')
                    ocomm.Parameters.AddWithValue("", dt28WS.Rows[i]["H012"]);  //W28S12,COLHDG('貨幣CD')
                    ocomm.Parameters.AddWithValue("", dt28WS.Rows[i]["H013"]);  // W28S13,COLHDG('匯率')
                    ocomm.Parameters.AddWithValue("", dt28WS.Rows[i]["H014"]);  // W28S14,COLHDG('付款方式')
                    ocomm.Parameters.AddWithValue("", dt28WS.Rows[i]["H015"]);  // W28S15,COLHDG('天期CD')
                    ocomm.Parameters.AddWithValue("", dt28WS.Rows[i]["H016"]);  //W28S16,COLHDG('預算CD')
                    ocomm.Parameters.AddWithValue("", dt28WS.Rows[i]["H017"]);  //W28S17,COLHDG('特性')
                    ocomm.Parameters.AddWithValue("", dt28WS.Rows[i]["H018"]);  //W28S18,COLHDG('案件CD')
                    ocomm.Parameters.AddWithValue("", dt28WS.Rows[i]["H019"]);  //W28S19,COLHDG('工廠CD')
                    ocomm.Parameters.AddWithValue("", dt28WS.Rows[i]["H020"]);  //W28S20,COLHDG('預算部門CD')
                    ocomm.Parameters.AddWithValue("", dt28WS.Rows[i]["H021"]);  //W28S21,COLHDG('負擔部門CD')
                    ocomm.Parameters.AddWithValue("", dt28WS.Rows[i]["H022"]);  //W28S22,COLHDG('大小車CD')
                    ocomm.Parameters.AddWithValue("", dt28WS.Rows[i]["H023"]);  //W28S23,COLHDG('車種區分')
                    ocomm.Parameters.AddWithValue("", dt28WS.Rows[i]["H024"]);  //W28S24,COLHDG('工程區分')
                    ocomm.Parameters.AddWithValue("", Convert.ToString(Convert.ToDecimal(dt28WS.Rows[i]["H025"]) * 100).PadLeft(14, '0'));  //W28S25,COLHDG('原幣金額')
                    ocomm.Parameters.AddWithValue("", Convert.ToString(dt28WS.Rows[i]["H025"]).PadLeft(12, '0'));  //W28S26,COLHDG('台幣金額')
                    ocomm.Parameters.AddWithValue("", Convert.ToString(dt28WS.Rows[i]["H026"]).PadLeft(12, '0'));  //W28S27,COLHDG('稅額')
                    ocomm.Parameters.AddWithValue("", dt28WS.Rows[i]["H027"]);  //W28S28,COLHDG('摘要')
                    ocomm.Parameters.AddWithValue("", dt28WS.Rows[i]["VOUCHER_ID"]);  //W28S31,COLHDG('傳票群組號碼')
                    ocomm.Parameters.AddWithValue("", dt28WS.Rows[i]["VOUCHER_ID"]);  //W28S32,COLHDG('部門傳票號碼')
                    odbc.getDataTable(ocomm);
                }
            } // if (wkStatus.Equals("2"))
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            odbc.connectionClose();
        }
    }

    public DataTable getDtlData2(int startRowIndex, int maximumRows, string sortExpression, string salary_type, string salary_dt, string pay_kind, string pay_id)
    {
        try
        {
            if (sortExpression.Contains("GROUP_ID"))
                sortExpression = sortExpression.Replace("GROUP_ID", "t1.GROUP_ID");
            if (sortExpression.Contains("GROUP_NAME"))
                sortExpression = sortExpression.Replace("GROUP_NAME", "h.GROUP_NAME");
            if (sortExpression.Contains("DEPT_ACCT_ID"))
                sortExpression = sortExpression.Replace("DEPT_ACCT_ID", "t1.DEPT_ACCT_ID");

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select * From (");
            sb.Append(" select ROW_NUMBER() OVER( ORDER BY  " + sortExpression + "  ) As RowNumber");
            sb.Append(" ,t1.GROUP_ID,h.GROUP_NAME,t1.DEPT_ACCT_ID,t1.ACCT_ID,t1.SAP_HR_NO ");
            sb.Append(" , convert(varchar(20), FORMAT( D_TOTAMT, 'N0')) as D_TOTAMT  ");
           
            sb.Append(" from TB_S_M_SALARY_VOUCHER t1 ");
            sb.Append(" left join TB_S_M_SALARY_GROUP_H h on h.KIND_CD ='D' and h.GROUP_TYPE = @SALARY_TYPE and h.GROUP_ID = t1.GROUP_ID ");
            sb.Append(" where t1.PAY_ID = @PAY_ID ");            
            sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar) ");

            ht.Add("@SALARY_TYPE", salary_type);
            ht.Add("@SALARY_DT", salary_dt);
            ht.Add("@PAY_KIND", pay_kind);
            ht.Add("@PAY_ID", pay_id);
            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    public int getDtlCount2(int startRowIndex, int maximumRows, string salary_type, string salary_dt, string pay_kind, string pay_id)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select COUNT(*) total_record");
            sb.Append(" from TB_S_M_SALARY_VOUCHER t1 ");
            sb.Append(" left join TB_S_M_SALARY_GROUP_H h on h.KIND_CD ='D' and h.GROUP_TYPE = @SALARY_TYPE and h.GROUP_ID =t1.GROUP_ID ");
            sb.Append(" where t1.PAY_ID = @PAY_ID   ");
            ht.Add("@SALARY_TYPE", salary_type);
            ht.Add("@SALARY_DT", salary_dt);
            ht.Add("@PAY_KIND", pay_kind);
            ht.Add("@PAY_ID", pay_id);
            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                t = (int)dt.Rows[0]["total_record"];
            }
            return t;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public int checkDCCisExist(string tableName)
    {
        DBConnector_ODBC odbc = new DBConnector_ODBC(utilities.ODBCconnstr);  //as 400 連線
        try
        {
            int i = 0;
            string st = "";
            
            OdbcCommand ocomm = new OdbcCommand();
            ocomm.CommandText += "select count(*) resultCount ";
            if (tableName.Equals("DCCC82M"))
            {
                ocomm.CommandText += " from CCCCLIB.DCCC82M ";
                ocomm.CommandText += " where W26S31 = ? and W26S31 <> W26S32 ";
            }
            else if (tableName.Equals("DCCC84M"))
            {
                ocomm.CommandText += " from CCCCLIB.DCCC84M ";
                ocomm.CommandText += " where W28S31 = ? and W28S31 <> W28S32 ";
            }
            else if (tableName.Equals("DCCC85M"))
            {
                ocomm.CommandText += " from CCCCLIB.DCCC85M ";
                ocomm.CommandText += " where W29S31 = ? and W29S31 <> W29S32 ";
            }


            ocomm.Parameters.AddWithValue("", DEPT_ACCT_ID);

            DataTable dt = odbc.getDataTable(ocomm);
            st = dt.Rows[0]["resultCount"].ToString();
            i = Convert.ToInt32(st);
            return i;
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            odbc.connectionClose();
        }
    }

    public int checkDCCisExist_Equal(string tableName)
    {
        DBConnector_ODBC odbc = new DBConnector_ODBC(utilities.ODBCconnstr);  //as 400 連線
        try
        {
            int i = 0;
            string st = "";
            
            OdbcCommand ocomm = new OdbcCommand();
            ocomm.CommandText += "select count(*) resultCount ";
            if (tableName.Equals("DCCC82M"))
            {
                ocomm.CommandText += " from CCCCLIB.DCCC82M ";
                ocomm.CommandText += " where W26S31 = ? and W26S31 = W26S32 ";
            }
            else if (tableName.Equals("DCCC84M"))
            {
                ocomm.CommandText += " from CCCCLIB.DCCC84M ";
                ocomm.CommandText += " where W28S31 = ? and W28S31 = W28S32 ";
            }
            else if (tableName.Equals("DCCC85M"))
            {
                ocomm.CommandText += " from CCCCLIB.DCCC85M ";
                ocomm.CommandText += " where W29S31 = ? and W29S31 = W29S32 ";
            }


            ocomm.Parameters.AddWithValue("", DEPT_ACCT_ID);

            DataTable dt = odbc.getDataTable(ocomm);
            st = dt.Rows[0]["resultCount"].ToString();
            i = Convert.ToInt32(st);
            return i;
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            odbc.connectionClose();
        }
    }

    public DataTable checkDCCisExist_excute3(string tableName)
    {
        DBConnector_ODBC odbc = new DBConnector_ODBC(utilities.ODBCconnstr);  //as 400 連線
        try
        {
            int i = 0;
            
            OdbcCommand ocomm = new OdbcCommand();
            if (tableName.Equals("DCCC82M"))
            {
                ocomm.CommandText += " select W26S32 as DEPT_ACCT_ID ";
                ocomm.CommandText += " from CCCCLIB.DCCC82M ";
                ocomm.CommandText += " where W26S31 = ? and W26S32 <> ? ";
            }
            else if (tableName.Equals("DCCC84M"))
            {
                ocomm.CommandText += " select W28S32 as DEPT_ACCT_ID ";
                ocomm.CommandText += " from CCCCLIB.DCCC84M ";
                ocomm.CommandText += " where W28S31 = ? and W28S32 <> ? ";
            }
            else if (tableName.Equals("DCCC85M"))
            {
                ocomm.CommandText += " select W29S32 as DEPT_ACCT_ID ";
                ocomm.CommandText += " from CCCCLIB.DCCC85M ";
                ocomm.CommandText += " where W29S31 = ? and W29S32 <> ? ";
            }


            ocomm.Parameters.AddWithValue("", DEPT_ACCT_ID);
            ocomm.Parameters.AddWithValue("", DEPT_ACCT_ID);
            DataTable dt = odbc.getDataTable(ocomm);
            return dt;
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            odbc.connectionClose();
        }
    }

    public int getDCCC01Mcount()
    {
        DBConnector_ODBC odbc = new DBConnector_ODBC(utilities.ODBCconnstr);  //as 400 連線
        try
        {
            string st = "";
            int i = 0;
            
            OdbcCommand ocomm = new OdbcCommand();
            ocomm.CommandText += "select count(*) resultCount ";
            ocomm.CommandText += " from CCCCLIB.DCCC01M ";
            ocomm.CommandText += " where M01001 = ? "; //部門傳票號碼 = 資料列.部門傳票號碼

            ocomm.Parameters.AddWithValue("", DEPT_ACCT_ID);

            DataTable dt = odbc.getDataTable(ocomm);
            st = dt.Rows[0]["resultCount"].ToString();
            i = Convert.ToInt32(st);
            return i;
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            odbc.connectionClose();
        }
    }
    public DataTable getDCCC01Mcount1()
    {
        DBConnector_ODBC odbc = new DBConnector_ODBC(utilities.ODBCconnstr);  //as 400 連線
        try
        {            
            
            OdbcCommand ocomm = new OdbcCommand();
            ocomm.CommandText += "select count(*) resultCount ";
            ocomm.CommandText += " from CCCCLIB.DCCC01M ";
            ocomm.CommandText += " where M01001 = ? "; //部門傳票號碼 = 資料列.部門傳票號碼

            ocomm.Parameters.AddWithValue("", DEPT_ACCT_ID);

            DataTable dt = odbc.getDataTable(ocomm);
            //st = dt.Rows[0]["resultCount"].ToString();
            //i = Convert.ToInt32(st);
            return dt;
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            odbc.connectionClose();
        }
    }

    public void deleteDCCC82M()
    {
        DBConnector_ODBC odbc = new DBConnector_ODBC(utilities.ODBCconnstr);  //as 400 連線
        try
        {
            
            OdbcCommand ocomm = new OdbcCommand();
            ocomm.CommandText += " delete from CCCCLIB.DCCC82M ";
            ocomm.CommandText += " where W26S32 = ? "; //傳票群組代號 = 資料列.部門傳票號碼

            ocomm.Parameters.AddWithValue("", DEPT_ACCT_ID);
            odbc.getDataTable(ocomm);
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            odbc.connectionClose();
        }
    }
    public void deleteDCCC82M_DEL()
    {
        DBConnector_ODBC odbc = new DBConnector_ODBC(utilities.ODBCconnstr);  //as 400 連線
        try
        {
            
            OdbcCommand ocomm = new OdbcCommand();
            ocomm.CommandText += " delete from CCCCLIB.DCCC82M ";
            ocomm.CommandText += " where W26S31 = ? "; //傳票群組代號 = 資料列.部門傳票號碼

            ocomm.Parameters.AddWithValue("", DEPT_ACCT_ID);
            odbc.getDataTable(ocomm);
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            odbc.connectionClose();
        }
    }
    public void deleteDCCC84M()
    {
        DBConnector_ODBC odbc = new DBConnector_ODBC(utilities.ODBCconnstr);  //as 400 連線
        try
        {            
            OdbcCommand ocomm = new OdbcCommand();
            ocomm.CommandText += " delete from CCCCLIB.DCCC84M ";
            ocomm.CommandText += " where W28S32 = ? "; //傳票群組代號 = 資料列.部門傳票號碼

            ocomm.Parameters.AddWithValue("", DEPT_ACCT_ID);
            odbc.getDataTable(ocomm);
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            odbc.connectionClose();
        }
    }
    public void deleteDCCC84M_DEL()
    {
        DBConnector_ODBC odbc = new DBConnector_ODBC(utilities.ODBCconnstr);  //as 400 連線
        try
        {            
            OdbcCommand ocomm = new OdbcCommand();
            ocomm.CommandText += " delete from CCCCLIB.DCCC84M ";
            ocomm.CommandText += " where W28S31 = ? "; //傳票群組代號 = 資料列.部門傳票號碼

            ocomm.Parameters.AddWithValue("", DEPT_ACCT_ID);
            odbc.getDataTable(ocomm);
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            odbc.connectionClose();
        }
    }
    public void deleteDCCC85M()
    {
        DBConnector_ODBC odbc = new DBConnector_ODBC(utilities.ODBCconnstr);  //as 400 連線
        try
        {            
            OdbcCommand ocomm = new OdbcCommand();
            ocomm.CommandText += " delete from CCCCLIB.DCCC85M ";
            ocomm.CommandText += " where W29S32 = ? "; //傳票群組代號 = 資料列.部門傳票號碼

            ocomm.Parameters.AddWithValue("", DEPT_ACCT_ID);
            odbc.getDataTable(ocomm);
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            odbc.connectionClose();
        }
    }
    public void deleteDCCC85M_DEL()
    {
        DBConnector_ODBC odbc = new DBConnector_ODBC(utilities.ODBCconnstr);  //as 400 連線
        try
        {            
            OdbcCommand ocomm = new OdbcCommand();
            ocomm.CommandText += " delete from CCCCLIB.DCCC85M ";
            ocomm.CommandText += " where W29S31 = ? "; //傳票群組代號 = 資料列.部門傳票號碼

            ocomm.Parameters.AddWithValue("", DEPT_ACCT_ID);
            odbc.getDataTable(ocomm);
        }
        catch (Exception)
        {
            throw;
        }
        finally
        {
            odbc.connectionClose();
        }
    }
    public void deleteTB_S_S_SALARY_VOUCHER_D()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" delete from TB_S_S_SALARY_VOUCHER_D ");
            sb.Append(" where VOUCHER_ID = @VOUCHER_ID ");
            ht.Add("@VOUCHER_ID", DEPT_ACCT_ID);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public void deleteTB_S_M_SALARY_VOUCHER()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" delete from TB_S_M_SALARY_VOUCHER ");
            sb.Append(" where DEPT_ACCT_ID = @DEPT_ACCT_ID ");
            ht.Add("@DEPT_ACCT_ID", DEPT_ACCT_ID);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //以關帳代號+傳票群組代號  更新 薪資傳票檔 TB_S_M_SALARY_VOUCHER
    public void updateTB_S_M_SALARY_VOUCHER()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_S_M_SALARY_VOUCHER ");
            sb.Append(" set DEPT_ACCT_ID = @DEPT_ACCT_ID,UPDATED_BY = @UPDATED_BY,UPDATED_DT= GETDATE(),FUNC_ID = 'FB2SC260' ");
            sb.Append(" where PAY_ID = @PAY_ID and GROUP_ID = @GROUP_ID");
            ht.Add("@PAY_ID", PAY_ID);
            ht.Add("@GROUP_ID", GROUP_ID);

            ht.Add("@DEPT_ACCT_ID", DEPT_ACCT_ID);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            dbConn.ExecuteT(sb, ht, true);
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
            string st = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select CODE_VAL1 from TB_9_M_PARAMETER where SYS_CD='SC' ");
            sb.Append(" AND MAIN_CD='VOUVHER_BATCH_PATH'  ");

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                st = dt.Rows[0]["CODE_VAL1"].ToString();
            }
            return st;
        }
        catch (Exception)
        {
            throw;
        }
    }

}