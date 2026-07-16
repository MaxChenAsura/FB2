using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;
using System.Text;
using System.Collections;
using System.Data.Odbc;


/// <summary>
/// CFB2HA0300BO 的摘要描述
/// </summary>
public class CFB2SF1300DAO : BaseDAO
{
    public CFB2SF1300DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public string RowNumber { get; set; }
    public string QDATAKEY { get; set; }
    public string YEAR_MONTH { get; set; }
    public string INS_RATE_PERSON { get; set; }
    public string INS_RATE_COMP { get; set; }
    public string INS_MAX_MONTH { get; set; }
    public string INS_MIN_AMOUNT { get; set; }
    public string INS_MAX_AMOUNT { get; set; }
    public string DEPT_ACCT_ID { get; set; }
    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }
    //for查詢欄位
    public string ddl_SYS_ID { get; set; }
    public string tmpNO { get; set; }

    //AS400用
    public string DOC_SEQ { get; set; } //單據序號
    public string DOC_KIND { get; set; } //單據種類
    public string DOC_NO { get; set; } //單據號碼
    public string INVOICE_CD { get; set; } //發票格式CD
    public string BUSINESS_TAX { get; set; } //營業稅課稅別
    public string BUSINESS_TAX_CD { get; set; } //營業稅扣抵CD
    public string VENDOR_ID { get; set; } //支付對象CD  勾選資料.廠商CODE
    public string VENDOR_CD { get; set; } //支付區分CD if 勾選資料.支付對象 = "D" or "E" //個人或本人 給"6" else 給"1" end
    public string HOPE_PAT_DT { get; set; } //希望領取日 勾選資料.希望匯款日
    public string S_DT { get; set; } //發生期間起
    public string E_DT { get; set; } //發生期間訖
    public string MONEY_CD { get; set; } //貨幣CD
    public string EXCHENGE_RATE { get; set; } //匯率
    public string PAYMONEY_TYPE { get; set; } //付款方式
    public string AS400_PAYMONEY_TYPE { get; set; } //AS400_付款方式
    public string DAYS_CD { get; set; } //天期CD
    public string BUDGET_CD { get; set; } //預算CD
    public string CHARACTERISTIC { get; set; } //特性
    public string CASE_CD { get; set; } //案件CD
    public string PLANT_CD { get; set; } //工廠CD
    public string BUDGET_DEPT { get; set; } //預算部門CD
    public string BURDEN_DEPT { get; set; } //負擔部門CD
    public string CAR_CD { get; set; } //大小車CD
    public string CARTYPE_CD { get; set; } //車種區分
    public string ENGINEER_CD { get; set; } //工程區分
    public string ORIGINAL_CURRENCY { get; set; } //原幣金額
    public string NT { get; set; } //台幣金額
    public string TAX { get; set; } //稅額
    public string REMARK { get; set; } //摘要
    public string SUMMONS_GROUP { get; set; } //傳票群組號碼
    public string SUMMONS_DEPT { get; set; } //部門傳票號碼
    public string IS_OVER { get; set; } //結清

    //財務界接    
    public string SYS_CD { get; set; } //薪資發放資料別
    public string SEQ_NO2 { get; set; } //傳票流水號
    public string B_VOUCHER_SEQ1 { get; set; } //支付傳票代號
    public string EMP_ID { get; set; } //部門傳票號碼
    public string SALARY_DT { get; set; } //發薪日期
    public string SALARY_TYPE { get; set; } //發薪類別
    public string PAY_KIND { get; set; } //法扣的代號
    public string P_KIND { get; set; } //薪資發放項目
    public string SEQ { get; set; } //發放項目
    public string TOTAL_AMT { get; set; } //已扣款金額
    public string EFFECT_EDT { get; set; } //結束日期
    public string IS_VAILD { get; set; } //失效
    public string AMOUNT { get; set; } //已扣款金額
    public string PAY_TARGET { get; set; } //支付對象
    public string BUDGET_C { get; set; } //預算CD_C；5799 
    public string BUDGET_D { get; set; } //預算CD_D；6510

    //TEMP TABLE
    public string EMP_NAME { get; set; }
    public string SALARY_NAME { get; set; }
    public string PAYMONEY_NAME { get; set; }
    public string ACCT_ID { get; set; }
    public string TMP_LNO { get; set; }

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


    //傳票界接檔
    public string IaDat { get; set; } //入帳日期
    //public string Cu { get; set; } //買受人 12488060
    //public string Itm { get; set; } //項次
    public string Vochno { get; set; } //傳票號碼
    //public string Dc { get; set; } //借貸
    //public string Dp { get; set; } //成本負擔部門
    //public string BgDp { get; set; } //預算部門
    //public string RemSumr { get; set; } //備註摘要
    //public string Acct { get; set; } //會計科目
    //public string Relno { get; set; } //相關號碼
    //public string OcryAmt { get; set; } //原幣金額
    //public string Ocrytaxamt { get; set; } //原幣稅額
    //public string Padty { get; set; } //支付方式
    //public string CO { get; set; }
    public string SlyPrvdDtid { get; set; }
    //public string Wtmen { get; set; }
    //public string WtmenNm { get; set; }
    //public string Rpamtpes { get; set; }
    //public string Pamennm { get; set; }
    //public string Sumr { get; set; }
    //public string Ca { get; set; }
    //public string Vchid { get; set; }
    //public string Vchno { get; set; }
    //public string VochAmt { get; set; }
    //public string Vochtaxamt { get; set; }
    //public string Obj { get; set; }
    //public string DdaAmt { get; set; }
    //public string Ddataxamt { get; set; }
    //public string Cucy { get; set; }
    //public string Exr { get; set; }
    //public string BkAcno { get; set; }
    //public string WrEdDat { get; set; }
    //public string StrnEntryMk { get; set; }
    //public string Cserid { get; set; }
    //public string NcrDat { get; set; }
    //public string IncmTy { get; set; }
    //public string RcvPcAcid { get; set; }
    //public string Ckno { get; set; }
    //public string PayTrm { get; set; }
    //public string IvDat { get; set; }
    //public string CkEdDat { get; set; }
    //public string CkBkId { get; set; }
    //public string CkBkAccno { get; set; }
    //public string Clckno { get; set; }
    //public string CkTrm { get; set; }
    //public string PaySqno { get; set; }
    //public string PayMk { get; set; }
    //public string VochHcode { get; set; }
    //public string AcctUrId { get; set; }
    public string IACYC { get; set; }//入帳週期




    public DataTable getALLOCATION_D()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * from TB_S_M_ALLOCATION_D WHERE 1 = 1");

            if (ACCT_ID != "")
            {
                sb.Append(" and ACCT_ID = @ACCT_ID");
                ht.Add("@ACCT_ID", ACCT_ID);
            }
            if (Lno != "")
            {
                sb.Append(" and Lno = @Lno");
                ht.Add("@Lno", Lno);
            }            
            
            return dbConn.QueryT(sb,ht);
        }
        catch
        {
            throw;
        }
    }

    public DataTable gettmpNO()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select CODE_VAL1 from TB_9_M_PARAMETER where SYS_CD='SF' AND MAIN_CD='TEMPACCTIDSEQ'");
            return dbConn.QueryT(sb);
        }
        catch
        {
            throw;
        }
    }

    public DataTable getSALARY_TYPE()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select SYS_CD , MAIN_CD , SUB_CD ,SUB_DESC from TB_9_M_COMM_D");
            sb.Append(" where SYS_CD='SC' and MAIN_CD='SALARY_TYPE' and IS_VALID='Y'");
            return dbConn.Query(sb);
        }
        catch
        {
            throw;
        }
    }

    public DataTable getSYS_ID(string SUB_CD)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select SUB_CD ,SUB_DESC from TB_9_M_COMM_D where 1=1 and sys_cd = '99' and main_cd = 'sys_id' and is_valid ='Y' and SUB_CD = @SUB_CD order by SUB_CD");
            ht.Add("@SUB_CD", SUB_CD);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    public DataTable getFUNC_ID(string ID)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" SELECT * FROM TB_9_M_SYS_M ");
            sb.Append(" WHERE SYS_ID+MODE_ID = @ID");
            ht.Add("@ID", ID);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    public DataTable getSORT(string EMP_ID_LIST)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" SELECT * FROM (");
            sb.Append(" select CONVERT(VARCHAR(10),A.SALARY_DT,111) + A.SALARY_TYPE + A.PAY_KIND + A.EMP_ID + A.DOC_NO + CONVERT(VARCHAR,A.SEQ) AS QRYKEY,");
            sb.Append(" a.*,c.EMP_NAME,d.SUB_DESC as SALARY_NAME,e.VENDOR_ID,d2.SUB_DESC as PAYMONEY_TYPE2");
            sb.Append(" from TB_S_M_ALLOCATION_D a");
            sb.Append(" left join TB_S_M_ARREARS_COURT_D b on a.SALARY_DT=b.SALARY_DT and a.SALARY_TYPE=b.SALARY_TYPE and a.PAY_KIND=b.PAY_KIND and a.EMP_ID=b.EMP_ID");
            sb.Append(" left join TB_H_M_EMP c on a.EMP_ID=c.EMP_ID");
            sb.Append(" left join TB_9_M_COMM_D d on d.SYS_CD='SC' and d.MAIN_CD='SALARY_TYPE' and d.SUB_CD= a.SALARY_TYPE");
            sb.Append(" left join TB_S_M_ARREARS_TARGET e on a.EMP_ID=e.EMP_ID and a.DOC_NO=e.DOC_NO and a.SEQ=e.SEQ");
            sb.Append(" left join TB_9_M_COMM_D d2 on d2.SYS_CD='SF' and d2.MAIN_CD='PAYMONEY_TYPE' and d2.SUB_CD= a.PAYMONEY_TYPE");
            sb.Append(" where b.SURE_YN='Y') TB");
            sb.Append(" where QRYKEY IN(" + EMP_ID_LIST + ")");
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    
    public DataTable get_PDF_Data(string ACCT_ID,string Lno)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.AppendLine(" select Convert(varchar(10),a.SALARY_DT ,111) as SALARY_DT,c.VENDOR_ID+'-'+c.CREDITOR as VENDOR_ID,");
            sb.AppendLine("     b.SALARY_NAME,a.AMOUNT, a.ACCT_ID ");
            sb.AppendLine(" from TB_S_M_ALLOCATION_D a	");
            sb.AppendLine(" left join VW_SALARYAND9999 b on b.SALARY_ID=a.PAY_KIND	");
            sb.AppendLine(" left join TB_S_M_ARREARS_TARGET c on a.EMP_ID=c.EMP_ID and a.DOC_NO=c.DOC_NO and a.SEQ=c.SEQ ");
            sb.AppendLine(" where 1=1 ");
            if (ACCT_ID != "")
            {
                sb.AppendLine(" and ACCT_ID = @ACCT_ID");
                ht.Add("@ACCT_ID", ACCT_ID);
            }
            if (Lno != "")
            {
                sb.AppendLine(" and Lno = @Lno");
                ht.Add("@Lno", Lno);
            }
           
            //sb.Append(" where a.DEPT_ACCT_ID=@DEPT_ACCT_ID");
            //ht.Add("@DEPT_ACCT_ID", DEPT_ACCT_ID);
            return dbConn.Query(sb,ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable getARREARS_COURT_D(string SALARY_DT, string SALARY_TYPE)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" select count(1) as cnt from TB_S_M_ARREARS_COURT_D ");
            sb.Append(" where SALARY_DT=@SALARY_DT AND SALARY_TYPE=@SALARY_TYPE AND SURE_YN='N'");
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);
           
            return dbConn.Query(sb,ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    
    public DataTable get_SYS_ID_Data()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select SYS_CD , MAIN_CD , SUB_CD ,SUB_DESC from TB_9_M_COMM_D");
            sb.Append(" where SYS_CD='HB' and MAIN_CD=CAR_TYPE  ");

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public string getLnoPara()
    {
        try
        {
            string st = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select distinct Lno from TB_S_M_ALLOCATION_D");
            sb.Append(" where ACCT_ID = @ACCT_ID  ");

            ht.Add("@ACCT_ID", ACCT_ID);

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                st = dt.Rows[0]["Lno"].ToString();
            }

            return st;
        }
        catch
        {
            throw;
        }
    }
    public DataTable getLogFlag(string Lno, string TblId)
    {
        try
        {
            dbConn.OtherCommStr = utilities.FF1connstr;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

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

    public DataTable getPAYMONEY_TYPE()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select SYS_CD , MAIN_CD , SUB_CD ,SUB_DESC from TB_9_M_COMM_D");
            sb.Append(" where SYS_CD='SF' and MAIN_CD='PAYMONEY_TYPE' and IS_VALID='Y'  ");

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
     //public DataTable getData(int startRowIndex, int maximumRows, string sortExpression,string txt_SALARY_DT,string ddl_SALARY_TYPE,string ddl_ACCT_ID,string txt_EMP_ID,string txt_HOPE_PAT_DT_S,string txt_HOPE_PAT_DT_E,string txt_VENDOR_ID,string txt_DEPT_ACCT_ID)
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string txt_SALARY_DT, string ddl_SALARY_TYPE, string ddl_ACCT_ID,
                            string txt_EMP_ID, string txt_HOPE_PAT_DT_S, string txt_HOPE_PAT_DT_E, string txt_VENDOR_ID, string txt_DEPT_ACCT_ID, 
                            string txt_ACCT_ID, string txt_Lno)
    {
        try
        {
            if (sortExpression.Contains("EMP_ID") )
            {
                sortExpression = string.Format("a.{0}", sortExpression);
            }

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * From");
            sb.Append(" (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" a.*,c.EMP_NAME,d.SUB_DESC as SALARY_NAME,e.VENDOR_ID, a.PAYMONEY_TYPE+'-'+ d2.SUB_DESC as PAYMONEY_TYPE2,e.CREDITOR, e.PAY_TARGET  ");
            sb.Append(" from TB_S_M_ALLOCATION_D a");
            sb.Append(" left join TB_S_M_ARREARS_COURT_D b on a.SALARY_DT=b.SALARY_DT and a.SALARY_TYPE=b.SALARY_TYPE and a.PAY_KIND=b.PAY_KIND and a.EMP_ID=b.EMP_ID");
            sb.Append(" left join TB_H_M_EMP c on a.EMP_ID=c.EMP_ID");
            sb.Append(" left join TB_9_M_COMM_D d on d.SYS_CD='SC' and d.MAIN_CD='SALARY_TYPE' and d.SUB_CD= a.SALARY_TYPE");
            sb.Append(" left join TB_S_M_ARREARS_TARGET e on a.EMP_ID=e.EMP_ID and a.DOC_NO=e.DOC_NO and a.SEQ=e.SEQ");
            sb.Append(" left join TB_9_M_COMM_D d2 on d2.SYS_CD='SF' and d2.MAIN_CD='PAYMONEY_TYPE' and d2.SUB_CD= a.PAYMONEY_TYPE");
            sb.Append(" where b.SURE_YN='Y' and a.AMOUNT >0 ");
            if (txt_SALARY_DT != "")
            {
                sb.Append(" and a.SALARY_DT = CONVERT(DATETIME, @SALARY_DT)");
                ht.Add("@SALARY_DT", txt_SALARY_DT);
            }
            if (ddl_SALARY_TYPE != "-1")
            {
                sb.Append(" and a.SALARY_TYPE=@SALARY_TYPE");
                ht.Add("@SALARY_TYPE", ddl_SALARY_TYPE.Substring(0, 1));
            }
            if (ddl_ACCT_ID == "Y")
            {
                sb.Append(" and a.ACCT_ID is not null	");
            }
            else if (ddl_ACCT_ID == "N")
            {
                sb.Append(" and a.ACCT_ID is null	");
            }

            if (txt_EMP_ID != "")
            {
                sb.Append(" and a.EMP_ID=@EMP_ID");
                ht.Add("@EMP_ID", txt_EMP_ID);
            }
            if (txt_HOPE_PAT_DT_S != "")
            {
                sb.Append(" and a.HOPE_PAT_DT BETWEEN @HOPE_PAT_DT_S and @HOPE_PAT_DT_E");
                ht.Add("@HOPE_PAT_DT_S", txt_HOPE_PAT_DT_S);
                ht.Add("@HOPE_PAT_DT_E", txt_HOPE_PAT_DT_E);
            }
            if (txt_VENDOR_ID != "")
            {
                sb.Append(" and e.VENDOR_ID=@VENDOR_ID");
                ht.Add("@VENDOR_ID", txt_VENDOR_ID);
              
            }
            if (txt_DEPT_ACCT_ID != "")
            {
                sb.Append(" and a.DEPT_ACCT_ID=@DEPT_ACCT_ID");
                ht.Add("@DEPT_ACCT_ID", txt_DEPT_ACCT_ID);

            }
            if (txt_ACCT_ID != "")
            {
                sb.Append(" and a.ACCT_ID=@ACCT_ID");
                ht.Add("@ACCT_ID", txt_ACCT_ID);

            }
            if (txt_Lno != "")
            {
                sb.Append(" and a.Lno=@Lno");
                ht.Add("@Lno", txt_Lno);

            }
            sb.Append(" )god_data");
            //sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            //sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar)");
            //ht.Add("@startRowIndex", startRowIndex);
            //ht.Add("@maximumRows", maximumRows);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public int getCount(int startRowIndex, int maximumRows,string txt_SALARY_DT,string ddl_SALARY_TYPE,string ddl_ACCT_ID,string txt_EMP_ID,
                        string txt_HOPE_PAT_DT_S, string txt_HOPE_PAT_DT_E, string txt_VENDOR_ID, string txt_DEPT_ACCT_ID, string txt_ACCT_ID, 
                        string txt_Lno)
    {
        try
        {
           int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) total_record ");
            sb.Append(" from TB_S_M_ALLOCATION_D a");
            sb.Append(" left join TB_S_M_ARREARS_COURT_D b on a.SALARY_DT=b.SALARY_DT and a.SALARY_TYPE=b.SALARY_TYPE and a.PAY_KIND=b.PAY_KIND and a.EMP_ID=b.EMP_ID");
            //sb.Append(" left join TB_H_M_EMP c on a.EMP_ID=c.EMP_ID");
            //sb.Append(" left join TB_9_M_COMM_D d on d.SYS_CD='SC' and d.MAIN_CD='SALARY_TYPE' and d.SUB_CD= a.SALARY_TYPE");
            sb.Append(" left join TB_S_M_ARREARS_TARGET e on a.EMP_ID=e.EMP_ID and a.DOC_NO=e.DOC_NO and a.SEQ=e.SEQ");
            //sb.Append(" left join TB_9_M_COMM_D d2 on d2.SYS_CD='SF' and d2.MAIN_CD='PAYMONEY_TYPE' and d2.SUB_CD= a.PAYMONEY_TYPE");
            sb.Append(" where b.SURE_YN='Y'  and a.AMOUNT >0 ");
            if (txt_SALARY_DT != "")
            {
                sb.Append(" and CONVERT(DATETIME, a.SALARY_DT) = CONVERT(DATETIME, @SALARY_DT)");
                ht.Add("@SALARY_DT", txt_SALARY_DT);
            }
            if (ddl_SALARY_TYPE != "-1")
            {
                sb.Append(" and a.SALARY_TYPE=@SALARY_TYPE");
                ht.Add("@SALARY_TYPE", ddl_SALARY_TYPE.Substring(0, 1));
            }
            if (ddl_ACCT_ID == "Y")
            {
                sb.Append(" and a.ACCT_ID is not null	");
            }
            else if (ddl_ACCT_ID == "N")
            {
                sb.Append(" and a.ACCT_ID is null	");
            }
            if (txt_EMP_ID != "")
            {
                sb.Append(" and a.EMP_ID=@EMP_ID");
                ht.Add("@EMP_ID", txt_EMP_ID);
            }
            if (txt_HOPE_PAT_DT_S != "")
            {
                sb.Append(" and a.HOPE_PAT_DT BETWEEN @HOPE_PAT_DT_S and @HOPE_PAT_DT_E");
                ht.Add("@HOPE_PAT_DT_S", txt_HOPE_PAT_DT_S);
                ht.Add("@HOPE_PAT_DT_E", txt_HOPE_PAT_DT_E);
            }
            if (txt_VENDOR_ID != "")
            {
                sb.Append(" and e.VENDOR_ID=@VENDOR_ID");
                ht.Add("@VENDOR_ID", txt_VENDOR_ID);

            }
            if (txt_DEPT_ACCT_ID != "")
            {
                sb.Append(" and a.DEPT_ACCT_ID=@DEPT_ACCT_ID");
                ht.Add("@DEPT_ACCT_ID", txt_DEPT_ACCT_ID);
            }
            if (txt_ACCT_ID != "")
            {
                sb.Append(" and a.ACCT_ID=@ACCT_ID");
                ht.Add("@ACCT_ID", txt_ACCT_ID);

            }
            if (txt_Lno != "")
            {
                sb.Append(" and a.Lno=@Lno");
                ht.Add("@Lno", txt_Lno);

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

    public DataTable getTOTAL_AMT()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * ");
            sb.Append(" from TB_S_M_ARREARS_COURT_H as m ");
            sb.Append("     where EMP_ID=@EMP_ID and DOC_NO=@DOC_NO");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@DOC_NO", DOC_NO);
            return dbConn.QueryT(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    public DataTable getTOTAL_AMT1()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("     select * ");
            sb.Append("     from TB_S_M_ARREARS_TARGET as m					");
            sb.Append("     where EMP_ID=@EMP_ID and DOC_NO=@DOC_NO and SEQ=@SEQ");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@DOC_NO", DOC_NO);
            ht.Add("@SEQ", SEQ);
            return dbConn.QueryT(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    public DataTable getModeData(string id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("     select ROW_NUMBER() OVER(ORDER BY m.FUNC_ID) As RowNumber,");
            sb.Append("     d.* ");
            sb.Append("     from TB_9_M_SYS_M as m					");
            sb.Append("     inner join TB_9_M_SYS_D as d on m.FUNC_ID = d.FUNC_ID and m.SYS_ID+m.MODE_ID = @ID");
            sb.Append("     where 1=1");

            ht.Add("@ID", id);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    public DataTable getModeData(int startRowIndex, int maximumRows, string sortExpression, string id)
    {
        try
        {
            if (sortExpression == "")
            {
                sortExpression = "MODE_ID";
            }
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * From");
            sb.Append(" (");
            sb.Append("     select ROW_NUMBER() OVER(ORDER BY m.FUNC_ID) As RowNumber,");
            sb.Append("     d.* ");
            sb.Append("     from TB_9_M_SYS_M as m					");
            sb.Append("     inner join TB_9_M_SYS_D as d on m.FUNC_ID = d.FUNC_ID and m.SYS_ID+m.MODE_ID = @ID");
            sb.Append("     where 1=1");
            sb.Append(" ) god_data where RowNumber between CAST(@startRowIndex as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar)");

            ht.Add("@ID", id);
            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public int getModeCount(int startRowIndex, int maximumRows, string id)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select COUNT(*) total_record from (");
            sb.Append(" 	select d.MODE_ID, d.FUNCTION_ID, d.FUNCTION_NAME");
            sb.Append("     from TB_9_M_SYS_M as m					");
            sb.Append("     inner join TB_9_M_SYS_D as d on m.FUNC_ID = d.FUNC_ID and m.SYS_ID+m.MODE_ID = @ID");
            sb.Append("     where 1=1");
            sb.Append(" ) as tb1");

            ht.Add("@ID", id);
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


    public string deleteAS400()
    {
        //刪除共用代碼主檔
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        sb.Append("Delete from DCCC83M where  ");
        sb.Append(" W28H30 = @W28H30");
        ht.Add("@W28H30", DEPT_ACCT_ID);
        dbConn.ExecuteT(sb, ht, true);

        return "0";
    }
    internal DataTable getExistData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select * from TB_S_M_ALLOCATION_D where DEPT_ACCT_ID = @DEPT_ACCT_ID");
            ht.Add("@DEPT_ACCT_ID", DEPT_ACCT_ID);
           
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
    
    internal void addAs400()
    {
        DBConnector_ODBC odbc = new DBConnector_ODBC(utilities.ODBCconnstr);
        try
        {           

            ////AS400
            OdbcCommand ocomm = new OdbcCommand();
            ocomm.CommandText += "INSERT INTO CCCCLIB.DCCC83M";
            ocomm.CommandText += "  (W28H01, W28H02, W28H03, W28H04, W28H05, W28H06, W28H07, W28H08, W28H09, W28H10, W28H11, W28H12, W28H13, W28H14, W28H15, W28H16, W28H17, W28H18, W28H19, W28H20, W28H21,";
            ocomm.CommandText += "  W28H22, W28H23, W28H24, W28H25, W28H26, W28H27, W28H28, W28H30,W28H37)";
            ocomm.CommandText += "  VALUES  (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ? ,?)";            

            ocomm.Parameters.AddWithValue("", DOC_SEQ);
            ocomm.Parameters.AddWithValue("", DOC_KIND);
            ocomm.Parameters.AddWithValue("", DOC_NO);
            ocomm.Parameters.AddWithValue("", INVOICE_CD);
            ocomm.Parameters.AddWithValue("", BUSINESS_TAX);
            ocomm.Parameters.AddWithValue("", BUSINESS_TAX_CD);
            ocomm.Parameters.AddWithValue("", VENDOR_ID);
            ocomm.Parameters.AddWithValue("", VENDOR_CD);
            ocomm.Parameters.AddWithValue("", HOPE_PAT_DT);
            ocomm.Parameters.AddWithValue("", S_DT);
            ocomm.Parameters.AddWithValue("", E_DT);
            ocomm.Parameters.AddWithValue("", MONEY_CD);
            ocomm.Parameters.AddWithValue("", EXCHENGE_RATE);
            ocomm.Parameters.AddWithValue("", AS400_PAYMONEY_TYPE);
            ocomm.Parameters.AddWithValue("", DAYS_CD);
            ocomm.Parameters.AddWithValue("", BUDGET_CD);
            ocomm.Parameters.AddWithValue("", CHARACTERISTIC);
            ocomm.Parameters.AddWithValue("", CASE_CD);
            ocomm.Parameters.AddWithValue("", PLANT_CD);
            ocomm.Parameters.AddWithValue("", BUDGET_DEPT);
            ocomm.Parameters.AddWithValue("", BURDEN_DEPT);
            ocomm.Parameters.AddWithValue("", CAR_CD);
            ocomm.Parameters.AddWithValue("", CARTYPE_CD);
            ocomm.Parameters.AddWithValue("", ENGINEER_CD);
           // ocomm.Parameters.AddWithValue("", ORIGINAL_CURRENCY);
            ocomm.Parameters.AddWithValue("",  Convert.ToString(ORIGINAL_CURRENCY).PadLeft(14, '0'));
            ocomm.Parameters.AddWithValue("", Convert.ToString(NT).PadLeft(12,'0'));  //W28S26,COLHDG('台幣金額')
            ocomm.Parameters.AddWithValue("", Convert.ToString(TAX).PadLeft(12,'0'));  //W28S26,COLHDG('台幣金額')
          /* ocomm.Parameters.AddWithValue("", NT); */
            //ocomm.Parameters.AddWithValue("", TAX);
            ocomm.Parameters.AddWithValue("", REMARK);
            ocomm.Parameters.AddWithValue("", SUMMONS_GROUP);
            ocomm.Parameters.AddWithValue("", SUMMONS_DEPT);           

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

    public void getSys_cd()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select SYS_CD from TB_S_M_SALARY_NAME_DATA");
            sb.Append(" where SALARY_TYPE = 'D' and PAY_KIND = @PAY_KIND");
            
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

    //舊傳票
//    public void insertTB_S_VOUCHER_TEMP()
//    {
//        try
//        {
//            StringBuilder sb = new StringBuilder();
//            Hashtable ht = new Hashtable();

//            sb.Append(@" insert into TB_S_VOUCHER_TEMP(
//                            CO,SlyPrvdDtid,IaCyc,IaDat,Vochno,
//                            Wtmen,WtmenNm,Rpamtpes,Pamennm,Cu,
//                            Itm,Dc,Dp,BgDp,Sumr,
//                            AcctUrId,Ca,RemSumr,Acct,Vchid,
//                            Vchno,VochAmt,Vochtaxamt,Relno,Obj,
//                            DdaAmt,Ddataxamt,Cucy,Exr,OcryAmt,
//                            Ocrytaxamt,BkAcno,WrEdDat,StrnEntryMk,Padty,
//                            Cserid,NcrDat,IncmTy,RcvPcAcid,Ckno,
//                            PayTrm,IvDat,CkEdDat,CkBkId,CkBkAccno,
//                            Clckno,CkTrm,PaySqno,PayMk,VochHcode,
//                            TxEmp,TxDat,TxTm,Lno)
//                        values (
//                            @CO,@SYS_CD,@IACYC,@IaDat,@Vochno,
//                            @Wtmen,@WtmenNm,@Rpamtpes,@Pamennm,@Cu,
//                            @Itm,@Dc,@Dp,@BgDp,@Sumr,
//                            @AUrId,@Ca,@RemSumr,@Acct,@Vchid,
//                            @Vchno,@VochAmt,@Vochtaxamt,@Relno,@Obj,
//                            @DdaAmt,@Ddataxamt,@Cucy,@Exr,@OcryAmt,
//                            @Ocrytaxamt,@BkAcno,@WrEdDat,@StrnEntryMk,@Padty,
//                            @Cserid,@NcrDat,@IncmTy,@RcvPcAcid,@Ckno,
//                            @PayTrm,@IvDat,@CkEdDat,@CkBkId,@CkBkAccno,
//                            @Clckno,@CkTrm,@PaySqno,@PayMk,@VochHcode,
//                            @TxEmp,@TxDat,@TxTm,@Lno)  ");


//            ht.Add("@CO", CO);
//            ht.Add("@SYS_CD", SYS_CD);
//            ht.Add("@IACYC", IACYC.Replace("/", ""));
//            ht.Add("@IaDat", IaDat.Replace("/", ""));
//            ht.Add("@Vochno", Vochno);

//            ht.Add("@Wtmen", Wtmen);
//            ht.Add("@WtmenNm", WtmenNm);
//            ht.Add("@Rpamtpes", Rpamtpes);
//            ht.Add("@Pamennm", Pamennm);
//            ht.Add("@Cu", Cu);

//            ht.Add("@Itm", Itm);
//            ht.Add("@Dc", Dc);
//            ht.Add("@Dp", Dp);
//            ht.Add("@BgDp", BgDp);
//            ht.Add("@Sumr", Sumr);

//            ht.Add("@AUrId", AcctUrId);
//            ht.Add("@Ca", Ca);
//            ht.Add("@RemSumr", RemSumr);
//            ht.Add("@Acct", Acct);
//            ht.Add("@Vchid", Vchid);

//            ht.Add("@Vchno", Vchno);
//            ht.Add("@VochAmt", VochAmt);
//            ht.Add("@Vochtaxamt", Vochtaxamt);
//            ht.Add("@Relno", Relno);
//            ht.Add("@Obj", Obj);

//            ht.Add("@DdaAmt", DdaAmt);
//            ht.Add("@Ddataxamt", Ddataxamt);
//            ht.Add("@Cucy", Cucy);
//            ht.Add("@Exr", Exr);
//            ht.Add("@OcryAmt", OcryAmt);

//            ht.Add("@Ocrytaxamt", Ocrytaxamt);
//            ht.Add("@BkAcno", BkAcno);

//            if (WrEdDat == "")
//            {
//                ht.Add("@WrEdDat", DBNull.Value);
//            }
//            else
//            {
//                ht.Add("@WrEdDat", WrEdDat);
//            }
//            ht.Add("@StrnEntryMk", StrnEntryMk);
//            ht.Add("@Padty", Padty);

//            ht.Add("@Cserid", Cserid);
//            if (NcrDat == "")
//            {
//                ht.Add("@NcrDat", DBNull.Value);
//            }
//            else
//            {
//                ht.Add("@NcrDat", NcrDat);
//            }
           
//            ht.Add("@IncmTy", IncmTy);
//            ht.Add("@RcvPcAcid", RcvPcAcid);
//            ht.Add("@Ckno", Ckno);

//            ht.Add("@PayTrm", PayTrm);
//            if (IvDat == "")
//            {
//                ht.Add("@IvDat", DBNull.Value);
//            }
//            else
//            {
//                ht.Add("@IvDat", IvDat);
//            }
//            if (CkEdDat == "")
//            {
//                ht.Add("@CkEdDat", DBNull.Value);
//            }
//            else
//            {
//                ht.Add("@CkEdDat", CkEdDat);
//            }
//            ht.Add("@CkBkId", CkBkId);
//            ht.Add("@CkBkAccno", CkBkAccno);

//            ht.Add("@Clckno", Clckno);
//            ht.Add("@CkTrm", CkTrm);
//            ht.Add("@PaySqno", PaySqno);
//            ht.Add("@PayMk", PayMk);
//            ht.Add("@VochHcode", VochHcode);

//            ht.Add("@TxEmp", SessionHandle.Current.emp_id);
//            ht.Add("@TxDat", DateTime.Now.ToString("yyyyMMdd"));
//            ht.Add("@TxTm", DateTime.Now.ToString("HHmmss") + "00");
//            ht.Add("@Lno", Lno);


//            dbConn.ExecuteT(sb, ht, true);
//        }
//        catch (Exception)
//        {
//            throw;
//        }
//    }

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

    public DataTable getVono()
    {

        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.AppendLine(@" select Vochno,Lno from TB_SF_TEMP a
                            left join TB_S_VOUCHER_TEMP b on a.AMOUNT = b.VochAmt and a.VENDOR_ID = b.Rpamtpes 
                            and b.SlyPrvdDtid = 'D52' and Dc = 'D'  
                            where emp_id = @emp_id and AMOUNT = @AMOUNT and VENDOR_ID = @VENDOR_ID and SEQ = @SEQ
            ");


            ht.Add("@emp_id", EMP_ID);
            ht.Add("@AMOUNT", AMOUNT);
            ht.Add("@VENDOR_ID", VENDOR_ID);
            ht.Add("@SEQ", SEQ);

            return dbConn.QueryT(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }

    internal void updateTB_S_M_ARREARS_TARGET()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" UPDATE       TB_S_M_ARREARS_TARGET");
            sb.Append(" SET                TOTAL_AMT = @TOTAL_AMT");
            if (PAYMONEY_TYPE == "A"){
                sb.Append(" , EFFECT_EDT = @EFFECT_EDT,IS_VAILD = @IS_VAILD");
            }
            sb.Append(" , UPDATED_BY = @UPDATED_BY, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID");
            sb.Append(" WHERE        (EMP_ID = @EMP_ID) AND (DOC_NO = @DOC_NO) AND (SEQ = @SEQ)");
            if (EFFECT_EDT == "null")
	        {
		         ht.Add("@EFFECT_EDT", DBNull.Value);
	        }else
	        {
                 ht.Add("@EFFECT_EDT", EFFECT_EDT);
	        }
            
            ht.Add("@TOTAL_AMT", TOTAL_AMT);           
            ht.Add("@IS_VAILD", IS_VAILD);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@DOC_NO", DOC_NO);
            ht.Add("@SEQ", SEQ);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    internal void updateTB_S_M_ALLOCATION_D()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" UPDATE  TB_S_M_ALLOCATION_D");
            sb.Append(" SET     Lno = @Lno,DEPT_ACCT_ID = @DEPT_ACCT_ID,ACCT_ID=@ACCT_ID,PAYMONEY_TYPE=@PAYMONEY_TYPE,HOPE_PAT_DT=@HOPE_PAT_DT,S_DT=@S_DT,E_DT=@E_DT, UPDATED_BY = @UPDATED_BY, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID");
            sb.Append(" WHERE   (SALARY_DT = @SALARY_DT) AND (SALARY_TYPE = @SALARY_TYPE) AND (PAY_KIND = @PAY_KIND) AND (EMP_ID = @EMP_ID) AND (DOC_NO = @DOC_NO) AND (SEQ = @SEQ)");
            sb.Append(" UPDATE       TB_S_M_ARREARS_COURT_H");
            sb.Append(" SET                TOTAL_AMT = @TOTAL_AMT, UPDATED_BY = @UPDATED_BY, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID");
            sb.Append(" WHERE        (EMP_ID = @EMP_ID) AND (DOC_NO = @DOC_NO)");

            ht.Add("@Lno", Lno);
            ht.Add("@DEPT_ACCT_ID", DEPT_ACCT_ID);
            ht.Add("@ACCT_ID", Vochno);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            ht.Add("@PAY_KIND", P_KIND);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@DOC_NO", DOC_NO);
            ht.Add("@SEQ", SEQ);

            ht.Add("@PAYMONEY_TYPE", PAYMONEY_TYPE);
            ht.Add("@HOPE_PAT_DT", HOPE_PAT_DT);
            ht.Add("@S_DT", S_DT);
            ht.Add("@E_DT", E_DT);
            ht.Add("@TOTAL_AMT", TOTAL_AMT);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    internal void delTempTable()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" delete from TB_SF_TEMP ;");     

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

    internal void insertTempTable()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" insert into TB_SF_TEMP 
                         (DOC_NO,EMP_ID,EMP_NAME,AMOUNT,SALARY_NAME
                        ,VENDOR_ID,HOPE_PAT_DT,S_DT,E_DT,PAYMONEY_TYPE
                        ,PAYMONEY_NAME,DEPT_ACCT_ID,ACCT_ID,SEQ,SALARY_DT
                        ,SALARY_TYPE,PAY_KIND,PAY_TARGET,CREATED_BY,CREATED_DT
                        ,UPDATED_BY,UPDATED_DT,FUNC_ID
                        ) values (
                         @DOC_NO,@EMP_ID,@EMP_NAME,@AMOUNT,@SALARY_NAME
                        ,@VENDOR_ID,@HOPE_PAT_DT,@S_DT,@E_DT,@PAYMONEY_TYPE
                        ,@PAYMONEY_NAME,@DEPT_ACCT_ID,@ACCT_ID,@SEQ,@SALARY_DT
                        ,@SALARY_TYPE,@PAY_KIND,@PAY_TARGET,@CREATED_BY,getdate()
                        ,@UPDATED_BY,getdate(),@FUNC_ID) ");

            ht.Add("@DOC_NO", DOC_NO);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@EMP_NAME", EMP_NAME);
            ht.Add("@AMOUNT", AMOUNT);
            ht.Add("@SALARY_NAME", SALARY_NAME);

            ht.Add("@VENDOR_ID", VENDOR_ID);
            ht.Add("@HOPE_PAT_DT", HOPE_PAT_DT);
            ht.Add("@S_DT", S_DT);
            ht.Add("@E_DT", E_DT);
            ht.Add("@PAYMONEY_TYPE", PAYMONEY_TYPE);

            ht.Add("@PAYMONEY_NAME", PAYMONEY_NAME);
            ht.Add("@DEPT_ACCT_ID", DEPT_ACCT_ID);
            ht.Add("@ACCT_ID", ACCT_ID);
            ht.Add("@SEQ", SEQ);
            ht.Add("@SALARY_DT", SALARY_DT);

            ht.Add("@SALARY_TYPE", SALARY_TYPE);
            ht.Add("@PAY_KIND", PAY_KIND);
            ht.Add("@PAY_TARGET", PAY_TARGET);
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);

            ht.Add("@FUNC_ID", "FB2SF130");          
            
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    internal DataTable selectTempTable()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
//            sb.Append(@"                         
//                        select a.AMOUNT,a.PAY_TARGET,a.EMP_ID,DOC_NO,EMP_NAME,SALARY_NAME,VENDOR_ID,Convert(varchar,HOPE_PAT_DT,111) HOPE_PAT_DT,Convert(varchar,S_DT,111) S_DT,
//                        Convert(varchar,E_DT,111)E_DT,PAYMONEY_TYPE,PAYMONEY_NAME,DEPT_ACCT_ID,ACCT_ID,SEQ,Convert(varchar,SALARY_DT,111)SALARY_DT,SALARY_TYPE,PAY_KIND from(
//                        select SUM(AMOUNT)AMOUNT,PAY_TARGET,EMP_ID
//                        from TB_SF_TEMP
//                        group by PAY_TARGET,EMP_ID
//                        )a
//                        left join TB_SF_TEMP b on a.EMP_ID = b.EMP_ID and a.PAY_TARGET = b.PAY_TARGET;  ");
            sb.Append(@" select AMOUNT, PAY_TARGET, EMP_ID, DOC_NO, EMP_NAME, SALARY_NAME, VENDOR_ID, 
                            Convert(varchar,HOPE_PAT_DT,111) HOPE_PAT_DT, Convert(varchar,S_DT,111) S_DT, Convert(varchar,E_DT,111)E_DT, 
                            PAYMONEY_TYPE, PAYMONEY_NAME, DEPT_ACCT_ID, ACCT_ID, SEQ, 
                            Convert(varchar,SALARY_DT,111)SALARY_DT, SALARY_TYPE, PAY_KIND, HR_NO
                         from TB_SF_TEMP ");

            return dbConn.QueryT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    internal string selectCREDITOR()
    {
        try
        {
            string st = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@"                         
                        select CREDITOR  from TB_S_M_ARREARS_TARGET
                        where emp_id = @emp_id and DOC_NO = @DOC_NO and VENDOR_ID = @VENDOR_ID  ");

            ht.Add("@emp_id", EMP_ID);
            ht.Add("@DOC_NO", DOC_NO);
            ht.Add("@VENDOR_ID", VENDOR_ID);

            DataTable dt = dbConn.QueryT(sb, ht, true);
            if (dt.Rows.Count > 0)
            {
                st = dt.Rows[0]["CREDITOR"].ToString();
            }

            return st;
        }
        catch (Exception)
        {
            throw;
        }
    }
    
    //舊傳票
    //public void insertTB_S_M_VOUCHER_SEQ()
    //{
    //    try
    //    {
    //        StringBuilder sb = new StringBuilder();
    //        Hashtable ht = new Hashtable();
    //        sb.Append(" delete from TB_S_M_VOUCHER_SEQ");
    //        sb.Append(" where SYS_CD = @SYS_CD and PAY_KIND = @PAY_KIND and IACYC = @IACYC ;");

    //        sb.Append(" insert into TB_S_M_VOUCHER_SEQ");
    //        sb.Append(" (SYS_CD,PAY_KIND,IACYC,SEQ_NO1,SEQ_NO2,");
    //        sb.Append(" LNO,CREATED_BY,CREATED_DT,UPDATED_BY,");
    //        sb.Append(" UPDATED_DT,FUNC_ID)");
    //        sb.Append(" values(@SYS_CD,@PAY_KIND,@IACYC,@SEQ_NO1, @SEQ_NO2,");
    //        sb.Append("@LNO,@CREATED_BY,getdate(),@UPDATED_BY,getdate(),");
    //        sb.Append(" @FUNC_ID)");

    //        ht.Add("@SYS_CD", SYS_CD);
    //        ht.Add("@PAY_KIND", PAY_KIND);
    //        ht.Add("@IACYC", IACYC.Replace("/", ""));
    //        ht.Add("@SEQ_NO1", B_VOUCHER_SEQ1);
    //        ht.Add("@SEQ_NO2", Convert.ToString(Convert.ToInt32(SEQ_NO2)));
    //        ht.Add("@LNO", Lno);            
    //        ht.Add("@CREATED_BY", CREATED_BY);
    //        ht.Add("@UPDATED_BY", UPDATED_BY);
    //        ht.Add("@FUNC_ID", FUNC_ID);

    //        dbConn.ExecuteT(sb, ht, true);

    //    }
    //    catch (Exception)
    //    {
    //        throw;
    //    }
    //}

    public void RunSP_I_FF1_VOUCHER()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(utilities.FF1ServerName + @".[FF1DB].dbo.SP_I_FF1_VOUCHER");
            if (string.IsNullOrEmpty(SYS_CD))
                ht.Add("@SlyPrvdDtid", DBNull.Value);
            else
                ht.Add("@SlyPrvdDtid", SYS_CD);

            if (string.IsNullOrEmpty(Lno))
                ht.Add("@LNO", DBNull.Value);
            else
                ht.Add("@LNO", Lno);

            ht.Add("@USERID", SessionHandle.Current.emp_id);
            ht.Add("@FUNCID", "FB2SF130");
            ht.Add("@ERROR_FLAG", "");

            dbConn.ExecuteSP(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

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
            ht.Add("@LOG_ID", "FB2SF130");
            return dbConn.Query(sb, ht, true);
        }
        catch
        {
            throw;
        }
    }

    //舊傳票
    //public void getSEQ2()
    //{
    //    DBConnector dbConn = new DBConnector();
    //    try
    //    {
    //        StringBuilder sb = new StringBuilder();
    //        Hashtable ht = new Hashtable();

    //        sb.Append("select isnull(RIGHT(REPLICATE('0', 4) + CAST(MAX(SEQ_NO2)  as NVARCHAR), 4) ,'') SEQ_NO2  from TB_S_M_VOUCHER_SEQ");
    //        sb.Append(" where SYS_CD = @SYS_CD and PAY_KIND = @PAY_KIND and IACYC = @IACYC");

    //        ht.Add("@SYS_CD", SYS_CD);
    //        ht.Add("@PAY_KIND", PAY_KIND);
    //        ht.Add("@IACYC", IACYC.Replace("/", ""));

    //        DataTable dt = dbConn.Query(sb, ht);
    //        if (dt.Rows.Count > 0)
    //        {
    //            SEQ_NO2 = dt.Rows[0]["SEQ_NO2"].ToString();
    //            if (SEQ_NO2 == "")
    //            {
    //                SEQ_NO2 = "0001";
    //            }
    //        }
    //    }
    //    catch
    //    {
    //        throw;
    //    }
    //}

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

    //舊 取批號
//    public DataTable getTB_S_M_VOUCHER_SEQ()
//    {
//        DBConnector dbConn = new DBConnector();
//        try
//        {
//            StringBuilder sb = new StringBuilder();
//            Hashtable ht = new Hashtable();

//            sb.Append(@"
//                        select distinct LNO as Lno from TB_S_M_VOUCHER_SEQ
//                        where  SYS_CD = @SYS_CD and IACYC = @IACYC");


//            ht.Add("@SYS_CD", SYS_CD);
//            ht.Add("@IACYC", IACYC.Replace("/",""));

//            DataTable dt = dbConn.QueryT(sb, ht);
//            return dt;
//        }
//        catch
//        {
//            throw;
//        }
//    }

    public DataTable getLno()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select MAX(Lno) Lno  from TB_S_M_ALLOCATION_D");
            sb.Append(" where SALARY_DT = @SALARY_DT and PAY_KIND = @PAY_KIND and Lno <> ''");

            ht.Add("@SALARY_DT", SALARY_DT);
            ht.Add("@PAY_KIND", P_KIND);

            DataTable dt = dbConn.QueryT(sb, ht);
            return dt;
        }
        catch
        {
            throw;
        }
    }

    internal void updateTB_9_M_PARAMETER()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" UPDATE       TB_9_M_PARAMETER");
            sb.Append(" SET                CODE_VAL1 = @CODE_VAL1");
            sb.Append(" WHERE        (SYS_CD = 'SF') AND (MAIN_CD = 'TEMPACCTIDSEQ')");
            ht.Add("@CODE_VAL1", tmpNO);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    internal void DeleteData_1()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Update TB_S_M_ALLOCATION_D  ");
            sb.Append(" Set DEPT_ACCT_ID='',UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE()");
            sb.Append(" where DEPT_ACCT_ID = @DEPT_ACCT_ID");
            ht.Add("@DEPT_ACCT_ID", DEPT_ACCT_ID);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    internal void updateData_1()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Update TB_S_M_ALLOCATION_D  ");
            sb.Append(" Set DEPT_ACCT_ID='',PAYMONEY_TYPE='',HOPE_PAT_DT=@HOPE_PAT_DT,S_DT=@S_DT,E_DT=@E_DT,UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(), FUNC_ID = @FUNC_ID");
            sb.Append(" where DEPT_ACCT_ID = @DEPT_ACCT_ID");
            ht.Add("@DEPT_ACCT_ID", DEPT_ACCT_ID);
            ht.Add("@HOPE_PAT_DT", DBNull.Value);
            ht.Add("@S_DT", DBNull.Value);
            ht.Add("@E_DT", DBNull.Value);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", "FB2SF130");
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    internal void update_COURT_H()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Update TB_S_M_ARREARS_COURT_H");
            if (PAYMONEY_TYPE =="C" )
            {
                sb.Append(" SET TOTAL_AMT = TOTAL_AMT,");
            }
            else
            {
                sb.Append(" SET TOTAL_AMT = TOTAL_AMT - @TOTAL_AMT,");
            }

            sb.Append(" UPDATED_BY = @UPDATED_BY, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID");
            sb.Append(" WHERE EMP_ID = @EMP_ID AND DOC_NO = @DOC_NO");
            ht.Add("@TOTAL_AMT", TOTAL_AMT);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@DOC_NO", DOC_NO);
           
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    internal void update_TARGET()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Update TB_S_M_ARREARS_TARGET");
            sb.Append(" set TOTAL_AMT = TOTAL_AMT - @AMOUNT, UPDATED_BY = @UPDATED_BY, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID ");
            if (PAY_TARGET == "A")
            {
                sb.Append(" , EFFECT_EDT = @EFFECT_EDT, IS_VAILD = @IS_VAILD");
                ht.Add("@EFFECT_EDT", DBNull.Value);
                ht.Add("@IS_VAILD", "Y");

            }           
            sb.Append(" WHERE EMP_ID = @EMP_ID AND DOC_NO = @DOC_NO AND SEQ = @SEQ");

            ht.Add("@AMOUNT", AMOUNT);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@DOC_NO", DOC_NO);
            ht.Add("@SEQ", SEQ);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    internal void update_ALLOCATION_D()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Update TB_S_M_ALLOCATION_D");
            sb.Append(" set DEPT_ACCT_ID = '',ACCT_ID = '',Lno = '', UPDATED_BY = @UPDATED_BY, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID,");
            sb.Append(" PAYMONEY_TYPE = '' , HOPE_PAT_DT = @HPT , S_DT = @SDT, E_DT = @EDT ");
            sb.Append(" WHERE 1 = 1");

            if (ACCT_ID != "")
            {
                sb.Append(" and ACCT_ID = @ACCT_ID");
                ht.Add("@ACCT_ID", ACCT_ID);
            }
            if (Lno != "")
            {
                sb.Append(" and Lno = @Lno");
                ht.Add("@Lno", Lno);
            }       

            ht.Add("@HPT", DBNull.Value);
            ht.Add("@SDT", DBNull.Value);
            ht.Add("@EDT", DBNull.Value);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            ht.Add("@DEPT_ACCT_ID", DEPT_ACCT_ID);           

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

   

    internal void updateData_2()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" UPDATE       TB_S_M_ARREARS_COURT_H");
            sb.Append(" SET                TOTAL_AMT = TOTAL_AMT - @TOTAL_AMT, UPDATED_BY = @UPDATED_BY, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID");
            sb.Append(" WHERE        (EMP_ID = @EMP_ID) AND (DOC_NO = @DOC_NO)");
            sb.Append(" ");
            sb.Append(" UPDATE       TB_S_M_ARREARS_TARGET");
            sb.Append(" SET                AMOUNT = AMOUNT - @AMOUNT");
            if (PAYMONEY_TYPE =="A"){
                sb.Append(" , EFFECT_EDT = @EFFECT_EDT, IS_VAILD = @IS_VAILD");
                ht.Add("@EFFECT_EDT", DBNull.Value);
                ht.Add("@IS_VAILD", "Y");

            }
            sb.Append(" , UPDATED_BY = @UPDATED_BY, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID");
            sb.Append(" WHERE        (EMP_ID = @EMP_ID) AND (DOC_NO = @DOC_NO) AND (SEQ = SEQ)");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@DOC_NO", DOC_NO);
            ht.Add("@TOTAL_AMT", AMOUNT);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            ht.Add("@SEQ", SEQ);
            ht.Add("@AMOUNT", AMOUNT);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable selectDCCC83M()
    {
        DBConnector_ODBC odbc = new DBConnector_ODBC(utilities.ODBCconnstr);
        try
        {            

            ////AS400
            OdbcCommand ocomm = new OdbcCommand();
            ocomm.CommandText += "select * from CCCCLIB.DCCC83M";
            ocomm.CommandText += "  where W28H30=?";

            ocomm.Parameters.AddWithValue("", SUMMONS_GROUP);            
            

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

    public DataTable selectDTL()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * from TB_S_M_ALLOCATION_D WHERE DEPT_ACCT_ID = @DEPT_ACCT_ID");
            ht.Add("@DEPT_ACCT_ID", SUMMONS_GROUP);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    //delete 集計資料
    public void deleteDCCC83M()
    {
        DBConnector_ODBC odbc = new DBConnector_ODBC(utilities.ODBCconnstr);
        try
        {           

            ////AS400
            OdbcCommand ocomm = new OdbcCommand();
            ocomm.CommandText += "delete from CCCCLIB.DCCC83M";
            ocomm.CommandText += "  where W28H30=?";

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

    public void delete_VOUCHER()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();          

            sb.Append(" delete from TB_S_VOUCHER_TEMP where 1=1 ");
            if (ACCT_ID != "")
            {
                sb.Append(" and Vochno = @Vochno");
                ht.Add("@Vochno", ACCT_ID);
            }
            if (Lno != "")
            {
                sb.Append(" and Lno = @Lno");
                ht.Add("@Lno", Lno);
            }             

            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }

    public string selectTarget()
    {
        try
        {
            string st = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select PAY_TARGET from TB_S_M_ARREARS_TARGET WHERE EMP_ID = @EMP_ID");
            sb.Append(" and DOC_NO = @DOC_NO and SEQ = @SEQ");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@DOC_NO", DOC_NO);
            ht.Add("@SEQ", SEQ);

            DataTable dt = dbConn.QueryT(sb, ht);
            if (dt.Rows.Count >0)
            {
                st = dt.Rows[0]["PAY_TARGET"].ToString();
            }

            return st;
        }
        catch
        {
            throw;
        }
    }

    public SqlParameterCollection SP_S_SF130_TO_SAP(string iaDat, string userId)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            Hashtable htOut = new Hashtable();

            sb.Append("SP_S_SF130_TO_SAP");

            ht.Add("@P_IA_DAT", iaDat.Replace("/",""));
            ht.Add("@P_USER_ID", userId);
            htOut.Add("@P_LNO", "");
            htOut.Add("@P_ERR_MSG", "");

            return dbConn.ExecuteSP(sb, ht, htOut, true);
        }
        catch
        {
            throw;
        }
    }
}