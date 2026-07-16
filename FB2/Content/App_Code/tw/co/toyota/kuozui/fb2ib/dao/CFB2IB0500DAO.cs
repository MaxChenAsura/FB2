using FB2.tw.co.toyota.kuozui.dao;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Web;

/// <summary>
/// CFB2IB0500DAO 的摘要描述
/// </summary>
public class CFB2IB0500DAO : BaseDAO
{
    //Screen Para
    public string Excel_YM { get; set; }
    public string SALARY_YM { get; set; }
    public string AFT_TOTAL { get; set; }
    public string BILL_NO { get; set; }


    //Excel Export
    public string YM { get; set; }
    public string ACC_CD { get; set; }
    public string ACC_WS { get; set; }
    public string SALARY_DEPT { get; set; }
    public string PLANT_CD { get; set; }
    public string CAR_KIND { get; set; }
    public string COST_DEPT_NO { get; set; }
    public string BUDGET_DEPT_NO { get; set; }
    public string FLOAT_S_TOTAL { get; set; }
    public string MONTH_S_TOTAL { get; set; }
    public string OFFLINE_F_S_TOTAL { get; set; }
    public string BOSS_FLOAT_SALARY { get; set; }
    public string BOSS_OTHER_SALARY { get; set; }
    public string TOTAL_INS { get; set; }
    public string AFT_INS_TOTAL { get; set; }
    public string INS2_BASE { get; set; }
    public string AFT_INS2_BASE { get; set; }
    public string INS2_COST { get; set; }
    public string AFT_INS2_COST { get; set; }
    public string BOSS_TAX { get; set; }
    public string IACYC { get; set; }//入帳週期

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
    

    //Other Para
    public string ori_Total { get; set; }
    public string tp_result { get; set; }
    public string INS_RATE_COMP { get; set; }
    public string CREATED_BY { get; set; }
    public string UPDATED_BY { get; set; }
    public string FUNC_ID { get; set; }
    public string DEPT { get; set; }
    public string PLANT { get; set; }
    public string ACC { get; set; }
    public string ACC_DEPT { get; set; }
    public string BUDGET_DEPT { get; set; }
    public string CAR { get; set; }
    public string COST_DEPT { get; set; }

    //財務界接
    public string PAY_KIND { get; set; } //補充保費發放項目
    public string SYS_CD { get; set; } //薪資發放資料別
    public string SEQ_NO2 { get; set; } //傳票流水號
    public string B_VOUCHER_SEQ1 { get; set; } //支付傳票代號
    public string BUDGET_C { get; set; } //預算CD_C；貸方，為21825 
    public string BUDGET_D1 { get; set; } //預算CD_D；科目=1時，為71292
    public string BUDGET_D2 { get; set; } //預算CD_D；科目=2時，為81292
    public string BUDGET_D3 { get; set; } //預算CD_D；科目=3時，為61292
    public string BUDGET_D4 { get; set; } //預算CD_D；科目=4時，為61192
    

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
    public string BTSQNO { get; set; } //批號
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





	public CFB2IB0500DAO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    //Gridview 查詢資料
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string ym, string bill_no)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" case when LEN(YM) = 6 then SUBSTRING(YM,1,4)+'/'+SUBSTRING(YM,5,2) else YM end as YM,");
            sb.Append(" ISNULL(CONVERT(char(10),TRANS_DT, 111),'')TRANS_DT,BILL_NO,Lno,TOTAL_INS,");
            sb.Append(" case when LEN(IACYC) = 6 then SUBSTRING(IACYC,1,4)+'/'+SUBSTRING(IACYC,5,2) else IACYC end as IACYC");            
            sb.Append(" from TB_S_R_INS2_BILL_RECORD");            
            sb.Append(" where 1=1");

            if (ym != "")
            {
                sb.Append(" and YM = @YM");
                ht.Add("@YM", ym.Replace("/", ""));               
            }
            if (bill_no != "")
            {
                sb.Append(" and BILL_NO = @BILL_NO");
                ht.Add("@BILL_NO", bill_no);
            }

            sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar)");


            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    //Gridview 查詢總筆數
    public int getCount(int startRowIndex, int maximumRows, string ym, string bill_no)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select COUNT(*) total_record ");
            sb.Append(" from TB_S_R_INS2_BILL_RECORD");
            sb.Append(" where 1=1");

            if (ym != "")
            {
                sb.Append(" and YM = @YM");
                ht.Add("@YM", ym.Replace("/", ""));
            }
            if (bill_no != "")
            {
                sb.Append(" and BILL_NO = @BILL_NO");
                ht.Add("@BILL_NO", bill_no);
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

    public DataTable selectData(string YM)
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select * from TB_S_R_INS2_COMPANY_SUMMARY");
            sb.Append(" where YM = @YM");

            ht.Add("@YM", YM);

            DataTable dt = dbConn.Query(sb, ht);


            return dt;
        }
        catch
        {
            throw;
        }
    }

    public DataTable selectMonthData(string YM)
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select * from TB_S_R_INS2_SALARY_MONTH");
            sb.Append(" where SALARY_YM = @YM");

            ht.Add("@YM", YM);

            DataTable dt = dbConn.Query(sb, ht);


            return dt;
        }
        catch
        {
            throw;
        }
    }

    public string selectAFT_INS2_COST(string YM)
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            string st = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select SUM(AFT_INS2_COST)AFT_INS2_COST from TB_S_R_INS2_SALARY_MONTH");
            sb.Append(" where SALARY_YM = @YM");

            ht.Add("@YM", YM);

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                st = dt.Rows[0]["AFT_INS2_COST"].ToString();
            }

            return st;
        }
        catch
        {
            throw;
        }
    }

    public string selectIACYC(string YM)
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            string st = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select IACYC from TB_S_R_INS2_BILL_RECORD");
            sb.Append(" where YM = @YM");

            ht.Add("@YM", YM);

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count >0)
            {
                st = dt.Rows[0]["IACYC"].ToString();
            }

            return st;
        }
        catch
        {
            throw;
        }
    }

    public DataTable getSALARY_MONTH_DATA()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select SALARY_YM, ACC_CD, b.SUB_DESC ACC_WS, SALARY_DEPT, PLANT_CD, CAR_KIND, COST_DEPT_NO, BUDGET_DEPT_NO,");
            sb.Append(" isnull(FLOAT_S_TOTAL,0)FLOAT_S_TOTAL,isnull( MONTH_S_TOTAL,0)MONTH_S_TOTAL,isnull( BOSS_TAX,0)BOSS_TAX,isnull( TOTAL_INS,0)TOTAL_INS,");
            sb.Append(" isnull(AFT_INS_TOTAL,0)AFT_INS_TOTAL,isnull( AFT_INS2_BASE,0)AFT_INS2_BASE,isnull( AFT_INS2_COST,0)AFT_INS2_COST");
            sb.Append(" from TB_S_R_INS2_SALARY_MONTH a");
            sb.Append(" left join TB_9_M_COMM_D b");
            sb.Append(" on a.ACC_WS = b.SUB_CD");
            sb.Append(" and b.SYS_CD = 'IB' and b.MAIN_CD = 'DIRECT_INDIRECT' and b.IS_VALID = 'Y'");
            sb.Append(" where SALARY_YM = @SALARY_YM and a.AFT_INS2_COST <> 0");

            ht.Add("@SALARY_YM", Excel_YM);

            DataTable dt = dbConn.Query(sb, ht);          


            return dt;
        }
        catch
        {
            throw;
        }
    }

    public DataTable getSALARY_MONTH()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select SALARY_YM, ACC_CD, b.SUB_DESC ACC_WS, SALARY_DEPT, PLANT_CD, CAR_KIND, COST_DEPT_NO, BUDGET_DEPT_NO,");
            //sb.Append(" FLOAT_S_TOTAL, MONTH_S_TOTAL, OFFLINE_F_S_TOTAL, BOSS_FLOAT_SALARY, BOSS_OTHER_SALARY, TOTAL_INS,");
            sb.Append(" FLOAT_S_TOTAL, MONTH_S_TOTAL, OFFLINE_F_S_TOTAL, BOSS_TAX, TOTAL_INS,");
            sb.Append(" AFT_INS_TOTAL, INS2_BASE, AFT_INS2_BASE, INS2_COST, AFT_INS2_COST");
            sb.Append(" from TB_S_R_INS2_SALARY_MONTH a");
            sb.Append(" left join TB_9_M_COMM_D b");
            sb.Append(" on a.ACC_WS = b.SUB_CD");
            sb.Append(" and b.SYS_CD = 'IB' and b.MAIN_CD = 'DIRECT_INDIRECT' and b.IS_VALID = 'Y'");
            sb.Append(" where SALARY_YM = @SALARY_YM and a.AFT_INS2_COST <> 0");

            ht.Add("@SALARY_YM", SALARY_YM);

            DataTable dt = dbConn.Query(sb, ht);


            return dt;
        }
        catch
        {
            throw;
        }
    }

    public DataTable getSALARY_MONTH_EXEC()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select SALARY_YM, ACC_CD, b.SUB_DESC ACC_WS, SALARY_DEPT, PLANT_CD, CAR_KIND, COST_DEPT_NO, BUDGET_DEPT_NO,");
            //sb.Append(" FLOAT_S_TOTAL, MONTH_S_TOTAL, OFFLINE_F_S_TOTAL, BOSS_FLOAT_SALARY, BOSS_OTHER_SALARY, TOTAL_INS,");
            sb.Append(" FLOAT_S_TOTAL, MONTH_S_TOTAL, OFFLINE_F_S_TOTAL, BOSS_TAX, TOTAL_INS,");
            sb.Append(" AFT_INS_TOTAL, INS2_BASE, AFT_INS2_BASE, INS2_COST, AFT_INS2_COST");
            sb.Append(" from TB_S_R_INS2_SALARY_MONTH a");
            sb.Append(" left join TB_9_M_COMM_D b");
            sb.Append(" on a.ACC_WS = b.SUB_CD");
            sb.Append(" and b.SYS_CD = 'IB' and b.MAIN_CD = 'DIRECT_INDIRECT' and b.IS_VALID = 'Y'");
            sb.Append(" where SALARY_YM = @SALARY_YM ");

            ht.Add("@SALARY_YM", SALARY_YM);

            DataTable dt = dbConn.Query(sb, ht);


            return dt;
        }
        catch
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

            sb.Append("select distinct Lno  from TB_S_R_INS2_BILL_RECORD");
            sb.Append(" where YM = @YM ");

            ht.Add("@YM", SALARY_YM);

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

    public void getSEQ2()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select isnull(RIGHT(REPLICATE('0', 4) + CAST(MAX(SEQ_NO2)  as NVARCHAR), 4) ,'') SEQ_NO2  from TB_S_M_VOUCHER_SEQ");
            sb.Append(" where SYS_CD = @SYS_CD and PAY_KIND = @PAY_KIND and IACYC = @IACYC");

            ht.Add("@SYS_CD", SYS_CD);
            ht.Add("@PAY_KIND", PAY_KIND);
            ht.Add("@IACYC", IACYC.Replace("/",""));

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {                
                SEQ_NO2 = dt.Rows[0]["SEQ_NO2"].ToString();
                if (SEQ_NO2 == "")
                {
                    SEQ_NO2 = "0000";
                }
            }
        }
        catch
        {
            throw;
        }
    }

    public void getTotalINS()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select SUM(TOTAL_INS)TOTAL_INS from TB_S_R_INS2_SALARY_MONTH");           
            sb.Append(" where SALARY_YM = @SALARY_YM");

            ht.Add("@SALARY_YM", SALARY_YM);

            DataTable dt = dbConn.Query(sb, ht);
            if(dt.Rows.Count >0){
                ori_Total = dt.Rows[0]["TOTAL_INS"].ToString();
            }
            
        }
        catch
        {
            throw;
        }
    }

    public void getInsPara()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select INS_RATE_COMP from TB_S_M_INS2_BASIC_SET a");
            sb.Append(" join (");
            sb.Append(" Select MAX(YEAR_MONTH)YEAR_MONTH from TB_S_M_INS2_BASIC_SET");
            sb.Append(" where YEAR_MONTH <= @SALARY_YM");
            sb.Append(" )b");
            sb.Append(" on a.YEAR_MONTH = b.YEAR_MONTH");

            ht.Add("@SALARY_YM", SALARY_YM);

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    INS_RATE_COMP = dt.Rows[0]["INS_RATE_COMP"].ToString();
                }
            }

        }
        catch
        {
            throw;
        }
    }

    public void updateINS2_SALARY_MONTH()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" update TB_S_R_INS2_SALARY_MONTH");
            sb.Append(" set AFT_INS_TOTAL=@AFT_INS_TOTAL, AFT_INS2_BASE=@AFT_INS2_BASE, AFT_INS2_COST=@AFT_INS2_COST,");           
            sb.Append(" UPDATED_BY=@UPDATED_BY, UPDATED_DT=getdate(), FUNC_ID=@FUNC_ID");
            sb.Append(" where SALARY_YM = @YM and ACC_CD = @ACC_CD and SALARY_DEPT = @SALARY_DEPT and PLANT_CD = @PLANT_CD");
            sb.Append(" and CAR_KIND = @CAR_KIND and COST_DEPT_NO =@COST_DEPT_NO and BUDGET_DEPT_NO =@BUDGET_DEPT_NO");


            ht.Add("@YM", YM);
            ht.Add("@ACC_CD", ACC_CD);           
            ht.Add("@SALARY_DEPT", SALARY_DEPT);
            ht.Add("@PLANT_CD", PLANT_CD);
            ht.Add("@CAR_KIND", CAR_KIND);
            ht.Add("@COST_DEPT_NO", COST_DEPT_NO);
            ht.Add("@BUDGET_DEPT_NO", BUDGET_DEPT_NO);
            ht.Add("@AFT_INS_TOTAL", AFT_INS_TOTAL); 
            ht.Add("@AFT_INS2_BASE", AFT_INS2_BASE);
            ht.Add("@AFT_INS2_COST", AFT_INS2_COST);                              
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);


            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }

    public void getManageDept()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select SUB_CD,CODE_VAL1,e.ACC_CD,e.ACC_DEPT_NO,e.BUDGET_DEPT_NO,e.CAR_TYPE,e.COST_DEPT_NO from TB_9_M_COMM_D a");
            sb.Append(" left join(select h.DEPT_NO,h.ACC_CD,h.ACC_DEPT_NO,d.BUDGET_DEPT_NO,d.CAR_TYPE,d.COST_DEPT_NO");
            sb.Append(" from TB_H_M_DEPT h join TB_H_M_DEPT_ACC d");
            sb.Append(" on h.ACC_DEPT_NO = d.ACC_DEPT_NO");
            sb.Append(" ) e on a.SUB_CD = e.DEPT_NO");
            sb.Append(" where SYS_CD ='IB' and MAIN_CD='MANAGER_DEPT' and IS_VALID='Y'");


            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {                    
                    DEPT = dt.Rows[0]["SUB_CD"].ToString();
                    PLANT = dt.Rows[0]["CODE_VAL1"].ToString();
                    ACC = dt.Rows[0]["ACC_CD"].ToString();
                    ACC_DEPT = dt.Rows[0]["ACC_DEPT_NO"].ToString();
                    BUDGET_DEPT = dt.Rows[0]["BUDGET_DEPT_NO"].ToString();
                    CAR = dt.Rows[0]["CAR_TYPE"].ToString();
                    COST_DEPT = dt.Rows[0]["COST_DEPT_NO"].ToString();
                }
            }

        }
        catch
        {
            throw;
        }
    }

    public void deleteCOMPANY_SUMMARY()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();



            sb.Append(" delete from TB_S_R_INS2_COMPANY_SUMMARY");
            sb.Append(" where YM = @YM");           

            ht.Add("@YM", SALARY_YM);
           

            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }

    public void insertCOMPANY_SUMMARY(string FLOAT_S_TOTAL, string MONTH_S_TOTAL,string OFFLINE_F_S_TOTAL,string BOSS_FLOAT_SALARY,
                                     string BOSS_OTHER_SALARY,string ORI_INS_TOTAL,string AFT_INS_TOTAL,string ORI_INS2_BASE,string AFT_INS2_BASE,
                                     string ORI_INS2_COST, string AFT_INS2_COST, string CREATED_BY, string UPDATED_BY, string FUNC_ID, string BOSS_TAX)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();


              
            sb.Append(" insert into TB_S_R_INS2_COMPANY_SUMMARY");
            sb.Append(" (YM, FLOAT_S_TOTAL, MONTH_S_TOTAL, OFFLINE_F_S_TOTAL, BOSS_FLOAT_SALARY, BOSS_OTHER_SALARY,BOSS_TAX,");
            sb.Append(" ORI_INS_TOTAL, AFT_INS_TOTAL, ORI_INS2_BASE, AFT_INS2_BASE, ORI_INS2_COST, AFT_INS2_COST,");
            sb.Append(" CREATED_BY, CREATED_DT, UPDATED_BY, UPDATED_DT, FUNC_ID)");
            sb.Append(" values(@YM, @FLOAT_S_TOTAL, @MONTH_S_TOTAL, @OFFLINE_F_S_TOTAL, @BOSS_FLOAT_SALARY, @BOSS_OTHER_SALARY,@BOSS_TAX,");
            sb.Append(" @ORI_INS_TOTAL, @AFT_INS_TOTAL, @ORI_INS2_BASE, @AFT_INS2_BASE, @ORI_INS2_COST, @AFT_INS2_COST,");
            sb.Append(" @CREATED_BY, getdate(), @UPDATED_BY, getdate(), @FUNC_ID)");

            ht.Add("@YM", SALARY_YM);
            ht.Add("@FLOAT_S_TOTAL", FLOAT_S_TOTAL);
            ht.Add("@MONTH_S_TOTAL", MONTH_S_TOTAL);
            ht.Add("@OFFLINE_F_S_TOTAL", OFFLINE_F_S_TOTAL);
            ht.Add("@BOSS_FLOAT_SALARY", BOSS_FLOAT_SALARY);
            ht.Add("@BOSS_OTHER_SALARY", BOSS_OTHER_SALARY);
            ht.Add("@BOSS_TAX", BOSS_TAX);
            ht.Add("@ORI_INS_TOTAL", ORI_INS_TOTAL);
            ht.Add("@AFT_INS_TOTAL", AFT_INS_TOTAL);
            ht.Add("@ORI_INS2_BASE", ORI_INS2_BASE);
            ht.Add("@AFT_INS2_BASE", AFT_INS2_BASE);
            ht.Add("@ORI_INS2_COST", ORI_INS2_COST);
            ht.Add("@AFT_INS2_COST", AFT_INS2_COST);           
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

    public string getMoneyDate(string MoneyDay)
    {
        string st = "";
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select CONVERT(char(10),MAX(CALENDAR_DT), 120)CALENDAR_DT from TB_D_M_CALENDAR_D a");
            sb.Append(" left join TB_9_M_PARAMETER b");
            sb.Append(" on a.CALENDAR_CD = b.CODE_VAL1");
            sb.Append(" and b.SYS_CD = 'DA' and MAIN_CD='DEFAULT_CALENDAR'");
            sb.Append(" where WORK_DAY_CD = '1' and CALENDAR_DT <= @MoneyDay");

            ht.Add("@MoneyDay", MoneyDay);

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    st = dt.Rows[0]["CALENDAR_DT"].ToString();                   
                }
            }

            return st;

        }
        catch
        {
            throw;
        }
    }

    //insert 集計資料
    public void insert26WH_Total(string W26H08, string W26H13, string W26H14, string W26H16, string W26H26)
    {
        DBConnector_ODBC odbc = new DBConnector_ODBC(utilities.ODBCconnstr);
        try
        {         

            ////AS400
            OdbcCommand ocomm = new OdbcCommand();
            ocomm.CommandText += "insert into CCCCLIB.DCCC81M (W26H01,W26H06,W26H07,W26H08,W26H09,W26H10,W26H12,W26H13,W26H14,W26H15,W26H16,W26H17,W26H26)";
            ocomm.CommandText += "  values ('0','A0011','1',?,'N','00100000','1',?,?,'000000000000',?,'5799',?)";

            //20150724  DICAR
            //ocomm.Parameters.AddWithValue("", Convert.ToString(W26H08).PadLeft(8, '0'));
            ocomm.Parameters.AddWithValue("", "00000000");
            ocomm.Parameters.AddWithValue("", Convert.ToString(Convert.ToDecimal(W26H13) * 100).PadLeft(14, '0'));
            ocomm.Parameters.AddWithValue("", Convert.ToString(W26H14).PadLeft(12, '0')); 
            //ocomm.Parameters.AddWithValue("", W26H13);
            //ocomm.Parameters.AddWithValue("", W26H14);
            ocomm.Parameters.AddWithValue("", W26H16);           
            ocomm.Parameters.AddWithValue("", W26H26);           


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

    //delete 集計資料
    public void delete26WH_Total(string W26H08, string W26H13, string W26H14, string W26H16, string W26H26)
    {
        DBConnector_ODBC odbc = new DBConnector_ODBC(utilities.ODBCconnstr);
        try
        {            

            ////AS400
            OdbcCommand ocomm = new OdbcCommand();
            ocomm.CommandText += "delete from CCCCLIB.DCCC81M";
            ocomm.CommandText += "  where W26H08=? and W26H13=? and W26H14=? and W26H16=? and W26H26=?";

            ocomm.Parameters.AddWithValue("", W26H08);
            ocomm.Parameters.AddWithValue("", W26H13);         
            ocomm.Parameters.AddWithValue("", W26H14);
            ocomm.Parameters.AddWithValue("", W26H16);
            ocomm.Parameters.AddWithValue("", W26H26);
            string ff = ocomm.ToString();

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
   
    //select 集計資料
    public DataTable select26WH_Total(string W26H08, string W26H13, string W26H14, string W26H16, string W26H26)
    {
        DBConnector_ODBC odbc = new DBConnector_ODBC(utilities.ODBCconnstr);
        try
        {            

            ////AS400
            OdbcCommand ocomm = new OdbcCommand();
            ocomm.CommandText += "select * from CCCCLIB.DCCC81M";
            ocomm.CommandText += "  where W26H08=? and W26H13=? and W26H14=? and W26H16=? and W26H26=?";

            //ocomm.Parameters.AddWithValue("", W26H13);
            //ocomm.Parameters.AddWithValue("", W26H14);
            
            ocomm.Parameters.AddWithValue("", W26H08);
            ocomm.Parameters.AddWithValue("", Convert.ToString(Convert.ToDecimal(W26H13) * 100).PadLeft(14, '0'));
            ocomm.Parameters.AddWithValue("", Convert.ToString(W26H14).PadLeft(12, '0')); 
            ocomm.Parameters.AddWithValue("", W26H16);
            ocomm.Parameters.AddWithValue("", W26H26);
            string ff = ocomm.ToString();

            DataTable dt =  odbc.getDataTable(ocomm);

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

    //insert 每筆資料
    public void insert26WH_DTL(string W26H13, string W26H14, string W26H16, string W26H17,string W26H20,
                                    string W26H22, string W26H23, string W26H26)
    {
        DBConnector_ODBC odbc = new DBConnector_ODBC(utilities.ODBCconnstr);
        try
        {            

            ////AS400
            OdbcCommand ocomm = new OdbcCommand();
            ocomm.CommandText += "insert into CCCCLIB.DCCC81M (W26H01,W26H08,W26H09,W26H10,W26H13,W26H14,W26H15,W26H16,W26H17,W26H20,W26H22,W26H23,W26H26)";
            ocomm.CommandText += "  values ('0','00000000','N','00100000',?,?,'000000000000',?,?,?,?,?,?)";

            //ocomm.Parameters.AddWithValue("", W26H13);
            //ocomm.Parameters.AddWithValue("", W26H14);
            ocomm.Parameters.AddWithValue("", Convert.ToString(Convert.ToDecimal(W26H13) * 100).PadLeft(14, '0'));
            ocomm.Parameters.AddWithValue("", Convert.ToString(W26H14).PadLeft(12, '0')); 
            ocomm.Parameters.AddWithValue("", W26H16);
            ocomm.Parameters.AddWithValue("", W26H17);
            ocomm.Parameters.AddWithValue("", W26H20);
            ocomm.Parameters.AddWithValue("", W26H22);
            ocomm.Parameters.AddWithValue("", W26H23);
            ocomm.Parameters.AddWithValue("", W26H26);

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

    //delete 每筆資料
    public void delete26WH_DTL(string W26H13, string W26H14, string W26H16, string W26H17, string W26H20,
                                    string W26H22, string W26H23, string W26H26)
    {
        DBConnector_ODBC odbc = new DBConnector_ODBC(utilities.ODBCconnstr);
        try
        {            

            ////AS400
            OdbcCommand ocomm = new OdbcCommand();
            ocomm.CommandText += "delete from CCCCLIB.DCCC81M";
            ocomm.CommandText += "  where W26H13=? and W26H14=? and W26H16=? and W26H17=? and W26H20=?";
            ocomm.CommandText += "  and W26H22=? and W26H23=? and W26H26=?";
            

            ocomm.Parameters.AddWithValue("", W26H13);
            ocomm.Parameters.AddWithValue("", W26H14);
            ocomm.Parameters.AddWithValue("", W26H16);
            ocomm.Parameters.AddWithValue("", W26H17);
            ocomm.Parameters.AddWithValue("", W26H20);
            ocomm.Parameters.AddWithValue("", W26H22);
            ocomm.Parameters.AddWithValue("", W26H23);
            ocomm.Parameters.AddWithValue("", W26H26);

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

    public DataTable select26WH_DTL(string W26H13, string W26H14, string W26H16, string W26H17, string W26H20,
                                    string W26H22, string W26H23, string W26H26)
    {
        DBConnector_ODBC odbc = new DBConnector_ODBC(utilities.ODBCconnstr);
        try
        {            

            ////AS400
            OdbcCommand ocomm = new OdbcCommand();
            ocomm.CommandText += "select * from CCCCLIB.DCCC81M";
            ocomm.CommandText += "  where W26H13=? and W26H14=? and W26H16=? and W26H17=? and W26H20=?";
            ocomm.CommandText += "  and W26H22=? and W26H23=? and W26H26=?";


            ocomm.Parameters.AddWithValue("", W26H13);
            ocomm.Parameters.AddWithValue("", W26H14);
            ocomm.Parameters.AddWithValue("", W26H16);
            ocomm.Parameters.AddWithValue("", W26H17);
            ocomm.Parameters.AddWithValue("", W26H20);
            ocomm.Parameters.AddWithValue("", W26H22);
            ocomm.Parameters.AddWithValue("", W26H23);
            ocomm.Parameters.AddWithValue("", W26H26);

            DataTable tmp = odbc.getDataTable(ocomm);

            return tmp;
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

    public void insertBILL_RECORD(string YM, string TOTAL_INS, string DEPT_BILL_NO)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();           


            sb.Append(" insert into TB_S_R_INS2_BILL_RECORD");
            sb.Append(" (YM, TOTAL_INS, DEPT_BILL_NO, BILL_NO, Lno, TRANS_DT,IACYC,");
            sb.Append(" CREATED_BY, CREATED_DT, UPDATED_BY, UPDATED_DT, FUNC_ID)");
            sb.Append(" values(@YM, @TOTAL_INS, @DEPT_BILL_NO, '','',@TRANS_DT,'',");          
            sb.Append(" @CREATED_BY,getdate(),@UPDATED_BY,getdate(),@FUNC_ID)");

            ht.Add("@YM", YM);
            ht.Add("@TOTAL_INS", TOTAL_INS);
            ht.Add("@DEPT_BILL_NO", DEPT_BILL_NO);
            ht.Add("@TRANS_DT", DBNull.Value);
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

    public DataTable getTB_S_M_VOUCHER_SEQ()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(@"
                        select distinct LNO as Lno from TB_S_M_VOUCHER_SEQ
                        where  SYS_CD = @SYS_CD and IACYC = @IACYC");


            ht.Add("@SYS_CD", SYS_CD);
            ht.Add("@IACYC", IACYC.Replace("/", ""));

            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch
        {
            throw;
        }
    }

    public DataTable getYM()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select MAX(YM) YM  from TB_S_R_INS2_BILL_RECORD");

            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch
        {
            throw;
        }
    }

    public DataTable getLogFlag()
    {
        try
        {
            dbConn.OtherCommStr = utilities.FF1connstr;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            
            sb.Append(" SELECT distinct isnull(GetChveMrtMk,'') GetChveMrtMk,isnull(AvWgtcmpsMk,'') AvWgtcmpsMk");
            sb.Append(" FROM SQLLNO ");
            sb.Append(" where substring(Lno,1,7) = @Lno and TblId = @TblId ");

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

    public DataTable getSQLLNO()
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

    //public DataTable select26WH_DTL()
    //{
    //    DBConnector_ODBC odbc = new DBConnector_ODBC(utilities.ODBCconnstr);
    //    try
    //    {            

    //        ////AS400
    //        OdbcCommand ocomm = new OdbcCommand();
    //        ocomm.CommandText += "select W26H27 from CCCCLIB.DCCC81M";
    //        ocomm.CommandText += "  where W26H27 <> ''";
            

    //        DataTable tmp = odbc.getDataTable(ocomm);

    //        return tmp;

    //    }
    //    catch (Exception)
    //    {
    //        throw;
    //    }
    //    finally
    //    {
    //        odbc.connectionClose();
    //    }
    //}

    public void deleteBILL_RECORD()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();



            sb.Append(" delete from TB_S_R_INS2_BILL_RECORD");
            sb.Append(" where YM = @YM");

            ht.Add("@YM", YM);



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

            DataTable dt = dbConn.Query(sb, ht);
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


            ht.Add("@CO", "KZ");
            ht.Add("@SYS_CD", SYS_CD);
            ht.Add("@IACYC", IACYC.Replace("/",""));
            ht.Add("@IaDat", IaDat.Replace("/",""));
            ht.Add("@Vochno", Vochno);

            ht.Add("@Wtmen", Wtmen);
            ht.Add("@WtmenNm", WtmenNm);
            ht.Add("@Rpamtpes", Rpamtpes);
            ht.Add("@Pamennm", "健保局");
            ht.Add("@Cu", Cu);

            ht.Add("@Itm", Itm);
            ht.Add("@Dc", Dc);
            ht.Add("@Dp", Dp);
            ht.Add("@BgDp", BgDp);
            ht.Add("@Sumr", DBNull.Value);

            ht.Add("@AUrId", DBNull.Value);
            ht.Add("@Ca", DBNull.Value);
            ht.Add("@RemSumr", RemSumr);
            ht.Add("@Acct", Acct);
            ht.Add("@Vchid", DBNull.Value);

            ht.Add("@Vchno", DBNull.Value);
            ht.Add("@VochAmt", VochAmt);
            ht.Add("@Vochtaxamt", Vochtaxamt);
            ht.Add("@Relno", Relno);
            ht.Add("@Obj", Obj);

            ht.Add("@DdaAmt", "0");
            ht.Add("@Ddataxamt", "0");
            ht.Add("@Cucy", "");
            ht.Add("@Exr", "1");
            ht.Add("@OcryAmt", "0");

            ht.Add("@Ocrytaxamt", "0");
            ht.Add("@BkAcno", DBNull.Value);
            ht.Add("@WrEdDat", DBNull.Value);
            ht.Add("@StrnEntryMk", DBNull.Value);
            ht.Add("@Padty", Padty);

            ////20161005 IB050 轉傳票畫面 需增加入帳日期欄位，傳票檔案的入帳週期=入帳日期的年月
            //               需款週期欄位修改成需款日期 直接指定哪一天付款
            ht.Add("@Cserid", "1");
            //ht.Add("@NcrDat", IACYC.Replace("/", "")+"15");
            ht.Add("@NcrDat", NcrDat.Replace("/",""));
            ht.Add("@IncmTy", "");
            ht.Add("@RcvPcAcid", "");
            ht.Add("@Ckno", DBNull.Value);

            ht.Add("@PayTrm", PayTrm);
            ht.Add("@IvDat", DBNull.Value);
            ht.Add("@CkEdDat", DBNull.Value);
            ht.Add("@CkBkId", DBNull.Value);
            ht.Add("@CkBkAccno", DBNull.Value);

            ht.Add("@Clckno", DBNull.Value);
            ht.Add("@CkTrm", DBNull.Value);
            ht.Add("@PaySqno", DBNull.Value);
            ht.Add("@PayMk", "N");
            ht.Add("@VochHcode", DBNull.Value);

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

    public DataTable getFB_TB_S_VOUCHER_TEMP()
    {
        try
        {
            //dbConn.OtherCommStr = utilities.FF1connstr;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" SELECT * from TB_S_VOUCHER_TEMP");       
            
            DataTable dt = dbConn.Query(sb, ht);
            
            return dt;
        }
        catch
        {
            throw;
        }
    }

    public void deleteFF1_TB_S_VOUCHER_TEMP()
    {
        try
        {
            dbConn.OtherCommStr = utilities.FF1connstr;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(@" delete from TB_S_VOUCHER_TEMP 
                        where BTSQNO = @BTSQNO ");
            
            ht.Add("@BTSQNO", BTSQNO);

            dbConn.Execute(sb, ht, true);
            dbConn.OtherCommStr = "";
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void insertFF1_TB_S_VOUCHER_TEMP()
    {
        try
        {
            dbConn.OtherCommStr = utilities.FF1connstr;
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
                            TxEmp,TxDat,TxTm,BTSQNO)
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
                            @TxEmp,@TxDat,@TxTm,@BTSQNO)  ");


            ht.Add("@CO", CO);
            ht.Add("@SYS_CD", SlyPrvdDtid);
            ht.Add("@IACYC", IACYC);
            ht.Add("@IaDat", IaDat);
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
            ht.Add("@WrEdDat", WrEdDat);
            ht.Add("@StrnEntryMk", StrnEntryMk);
            ht.Add("@Padty", Padty);

            ht.Add("@Cserid", Cserid);
            ht.Add("@NcrDat", NcrDat);
            ht.Add("@IncmTy", IncmTy);
            ht.Add("@RcvPcAcid", RcvPcAcid);
            ht.Add("@Ckno", Ckno);

            ht.Add("@PayTrm", PayTrm);
            ht.Add("@IvDat", IvDat);
            ht.Add("@CkEdDat", CkEdDat);
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
            ht.Add("@BTSQNO", BTSQNO);


            dbConn.ExecuteT(sb, ht, true);
            dbConn.OtherCommStr = "";
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void insertFF1_SQLLNO()
    {
        try
        {
            dbConn.OtherCommStr = utilities.FF1connstr;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" delete from SQLLNO
                            where TblId = @TblId and Co = @Co and Lno = @Lno;");

            sb.Append(@" insert into SQLLNO(
                            TblId,Co,Lno,DtCnt,BegDat,
                            BegTm,EndDat,EndTm,WrsIaChveMrtMk,GetChveMrtMk,
                            PsComt,AvWgtcmpsMk,TxEmp,TxDat,TxTm
                            )
                        values (
                            @TblId,@Co,@Lno,@DtCnt,@BegDat,
                            @BegTm,@EndDat,@EndTm,@WrsIaChveMrtMk,@GetChveMrtMk,
                            @PsComt,@AvWgtcmpsMk,@TxEmp,@TxDat,@TxTm)  ");


            ht.Add("@TblId", "TB_S_VOUCHER_TEMP");
            ht.Add("@Co", "KZ");
            ht.Add("@Lno", BTSQNO);
            ht.Add("@DtCnt", Convert.ToString(Convert.ToInt32(Itm)));
            ht.Add("@BegDat", DateTime.Now.ToString("yyyyMMdd"));

            ht.Add("@BegTm", DateTime.Now.ToString("HHmmss") + "00");
            ht.Add("@EndDat", DateTime.Now.ToString("yyyyMMdd"));
            ht.Add("@EndTm", DateTime.Now.ToString("HHmmss") + "00");
            ht.Add("@WrsIaChveMrtMk", "Y");
            ht.Add("@GetChveMrtMk", "");

            ht.Add("@PsComt", "");
            ht.Add("@AvWgtcmpsMk", "");
            ht.Add("@TxEmp", SessionHandle.Current.emp_id);
            ht.Add("@TxDat", DateTime.Now.ToString("yyyyMMdd"));
            ht.Add("@TxTm", DateTime.Now.ToString("HHmmss") + "00");
            
            dbConn.Execute(sb, ht, true);
            dbConn.OtherCommStr = "";
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void updateBILL_RECORD()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" update TB_S_R_INS2_BILL_RECORD");
            sb.Append(" set BILL_NO = @Vochno , Lno = @Lno , TRANS_DT = getdate() , IACYC = @IACYC");
            sb.Append(" , UPDATED_BY = @UPDATED_BY , UPDATED_DT = getdate() , FUNC_ID = @FUNC_ID");
            sb.Append(" where YM = @YM");

            ht.Add("@YM", SALARY_YM);
            ht.Add("@Vochno", Vochno);
            ht.Add("@Lno", Lno);
            ht.Add("@IACYC", IACYC.Replace("/",""));
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable getTOTAL_COMPANY_SUMMARY()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select YM, AFT_INS2_COST");
            sb.Append(" from TB_S_R_INS2_COMPANY_SUMMARY");
            sb.Append(" where YM = @YM");

            ht.Add("@YM", SALARY_YM);

            DataTable dt = dbConn.Query(sb, ht);


            return dt;
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
            sb.Append(" SEQ_NO2,Lno,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,");
            sb.Append(" FUNC_ID)");
            sb.Append(" values(@SYS_CD,@PAY_KIND,@IACYC,@SEQ_NO1,");
            sb.Append(" @SEQ_NO2,@Lno,@CREATED_BY,getdate(),@UPDATED_BY,getdate(),");
            sb.Append(" @FUNC_ID)");

            ht.Add("@SYS_CD", SYS_CD);
            ht.Add("@PAY_KIND", PAY_KIND);
            ht.Add("@IACYC", IACYC.Replace("/", ""));
            ht.Add("@SEQ_NO1", B_VOUCHER_SEQ1);
            ht.Add("@SEQ_NO2", Convert.ToString(Convert.ToInt32(SEQ_NO2)));
            ht.Add("@Lno", Lno);
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
   
    /*
     *待改ODBC連法 
     */
    public void insertVOUCHER_TEMP()
    {
        DBConnector_ODBC odbc = new DBConnector_ODBC(utilities.FF1connstr);
        try
        {            
         
            OdbcCommand ocomm = new OdbcCommand();
            ocomm.CommandText += "insert into TB_S_VOUCHER_TEMP("
                            +"CO,SlyPrvdDtid,IaCyc,IaDat,Vochno,"
                            +"Wtmen,WtmenNm,Rpamtpes,Pamennm,Cu,"
                            + "Itm,Dc,Dp,BgDp,Sumr,"
                            +"AcctUrId,Ca,RemSumr,Acct,Vchid,"
                            +"Vchno,VochAmt,Vochtaxamt,Relno,Obj,"
                            +"DdaAmt,Ddataxamt,Cucy,Exr,OcryAmt,"
                            +"Ocrytaxamt,BkAcno,WrEdDat,StrnEntryMk,Padty,"
                            +"Cserid,NcrDat,IncmTy,RcvPcAcid,Ckno,"
                            +"PayTrm,IvDat,CkEdDat,CkBkId,CkBkAccno,"
                            +"Clckno,CkTrm,PaySqno,PayMk,VochHcode,"
                            +"TxEmp,TxDat,TxTm,BTSQNO)"
                        +"values ("
                            +"'KZ',?,?,?,?,"
                            +"'','','','',?,"
                            +"?,?,?,?,'',"
                            +"'','',?,?,'',"
                            +"'','0','0',?,'',"
                            + "'0','0','TWD','1',?,"
                            +"'0','','0','','B',"
                            +"'1','0','0','0','',"
                            +"'','','','','',"
                            +"'','','','N','',"
                            +"?,?,?,?)";


            ocomm.Parameters.AddWithValue("", SYS_CD);
            ocomm.Parameters.AddWithValue("", IACYC.Replace("/", ""));
            ocomm.Parameters.AddWithValue("", IaDat.Replace("/", ""));
            ocomm.Parameters.AddWithValue("", Vochno);
            ocomm.Parameters.AddWithValue("", Cu);
            ocomm.Parameters.AddWithValue("", Itm);
            ocomm.Parameters.AddWithValue("", Dc);
            ocomm.Parameters.AddWithValue("", Dp);
            ocomm.Parameters.AddWithValue("", BgDp);
            ocomm.Parameters.AddWithValue("", RemSumr);
            ocomm.Parameters.AddWithValue("", Acct);
            ocomm.Parameters.AddWithValue("", Relno);
            ocomm.Parameters.AddWithValue("", OcryAmt);
            ocomm.Parameters.AddWithValue("", SessionHandle.Current.emp_id);
            ocomm.Parameters.AddWithValue("", DateTime.Now.ToString("yyyyMMdd"));
            ocomm.Parameters.AddWithValue("", DateTime.Now.ToString("HHmmss") + "00");
            ocomm.Parameters.AddWithValue("", BTSQNO);

            odbc.executeNonQueryWithTrans(ocomm);

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


    //轉傳票檢查
    public string chek_SAP_DONE()
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
                comm.CommandText = "SP_S_IB050_VOU_OUTCHK";
                comm.Parameters.AddWithValue("@P_SAP_HR_NO", BILL_NO);
                comm.Parameters.Add("@P_ERR_MSG", SqlDbType.NVarChar, 500).Direction = ParameterDirection.Output;

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


    //結轉傳票(SAP)
    public string VOUCHER_SAP()
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
                comm.CommandText = "SP_S_IB050_TO_SAP";
                comm.Parameters.AddWithValue("@P_YYYYMM", SALARY_YM);
                comm.Parameters.AddWithValue("@P_ACC_DT", IaDat.Replace("/",""));
                comm.Parameters.AddWithValue("@P_NEED_DT", NcrDat.Replace("/", ""));
                comm.Parameters.AddWithValue("@P_USER_ID", SessionHandle.Current.emp_id);
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
}