using FB2.tw.co.toyota.kuozui.dao;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.Odbc;
using System.Linq;
using System.Text;
using System.Web;

/// <summary>
/// CFB2IB0400DAO 的摘要描述
/// </summary>
public class CFB2IB0400DAO : BaseDAO
{
    //screen PARA
    public string SALARY_YM { get; set; }
    public string CHINESE_YM { get; set; }//轉民國年
   
    //AS400 G50   
    public string BILL_NO { get; set; }
    public string INVOICE_NO { get; set; }
    public string ACC_NO { get; set; }
    public string DEPT_BILL_NO { get; set; }
    public string BILL_CD { get; set; }
    public string PLUS_MINUS_CD { get; set; }
    public string BUDGET_ACC { get; set; }
    public string ACCOUNTING_ACC { get; set; }
    public string MONEY_CD { get; set; }
    public string CASE_CD { get; set; }
    public string NT_AMOUNT { get; set; }
    public string INS2_INSTEAD_AMOUNT { get; set; }
    public string SMMARY { get; set; }
    public string GET_MONEY_DT { get; set; }
    public string PAY_TO { get; set; }
    public string PAY_TO_LICENSE_ID { get; set; }
    public string WANT_DATE { get; set; }
    public string SWITCH_TO { get; set; }    
    public string CREATED_BY { get; set; }
    public string UPDATED_BY { get; set; }
    public string FUNC_ID { get; set; }
   
    //補充保費負擔部門月度檔
    public string SALARY_YM_M { get; set; }
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

    //OTHERPARA
    public string DEPT { get; set; }
    public string PLANT { get; set; }

    //ins PARA
    public string INS_RATE_COMP { get; set; }

	public CFB2IB0400DAO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    public DataTable getYM()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select MAX(SALARY_YM) YM  from TB_S_R_INS2_SALARY_MONTH");
            
            DataTable dt = dbConn.Query(sb, ht);
            return dt;
        }
        catch
        {
            throw;
        }
    }

    public DataTable checkSalary(string YM)
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select * from TB_S_M_SALARY_PAY");
            sb.Append(" where SALARY_TYPE = 'A' and left(Convert(varchar, SALARY_DT,112),6)=@YM");
            //sb.Append(" where SALARY_TYPE = 'A' and DATA_YM=@YM");
            ht.Add("@YM", YM);

            DataTable dt = dbConn.Query(sb, ht);


            return dt;
        }
        catch
        {
            throw;
        }
    }

    public DataTable checkCOMPANY_BILL(string YM)
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select * from TB_S_M_INS2_COMPANY_BILL");
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

    public DataTable checkSALARY_MONTH(string YM)
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

    public DataTable selectG50()
    {
        DBConnector_ODBC odbc = new DBConnector_ODBC(utilities.ODBCconnstr);
        try
        {            

            ////查詢AS400資料
            OdbcCommand ocomm = new OdbcCommand();
            ocomm.CommandText += "Select G5CNO,G5CSEQ,G5CSE1,G5CDNO,G5CTYP,G5CDC,G5BCC,G5CACC,G5CSCD,G5CASE,G5AMT1,G5AMT2,";
            ocomm.CommandText += "G5MEMO,G5ATDT,G5VRCD,G5PID,G5HDAT,G5TRDT,G5TRTM";
            ocomm.CommandText += " from DATLIB.DB3KG50";
            ocomm.CommandText += " where  substr(G5ATDT,1,5) = ?";

            ocomm.Parameters.AddWithValue("", CHINESE_YM);


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

    public void insertVANDOR_BILL()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();          


            sb.Append(" insert into TB_S_M_INS2_VANDOR");
            sb.Append(" (BILL_NO,INVOICE_NO,ACC_NO,DEPT_BILL_NO,BILL_CD,PLUS_MINUS_CD,BUDGET_ACC,ACCOUNTING_ACC,MONEY_CD,");
            sb.Append("  CASE_CD,NT_AMOUNT,INS2_INSTEAD_AMOUNT,SMMARY,GET_MONEY_DT,PAY_TO,PAY_TO_LICENSE_ID,WANT_DATE,SWITCH_TO,");
            sb.Append(" CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" values(@BILL_NO,@INVOICE_NO,@ACC_NO,@DEPT_BILL_NO,@BILL_CD,@PLUS_MINUS_CD,@BUDGET_ACC,@ACCOUNTING_ACC,@MONEY_CD,");
            sb.Append(" @CASE_CD,@NT_AMOUNT,@INS2_INSTEAD_AMOUNT,@SMMARY,@GET_MONEY_DT,@PAY_TO,@PAY_TO_LICENSE_ID,");
            sb.Append(" (CASE WHEN @WANT_DATE ='' THEN NULL ELSE CONVERT(varchar(4),CONVERT(int,substring(@WANT_DATE,0,4))+1911, 101)+'/'+substring(@WANT_DATE,4,2)+'/'+substring(@WANT_DATE,6,2) END) ,@SWITCH_TO,");
            sb.Append(" @CREATED_BY,getdate(),@UPDATED_BY,getdate(),@FUNC_ID)");
            ht.Add("@BILL_NO", BILL_NO);
            ht.Add("@INVOICE_NO", INVOICE_NO);
            ht.Add("@ACC_NO", ACC_NO);
            ht.Add("@DEPT_BILL_NO", DEPT_BILL_NO);
            ht.Add("@BILL_CD", BILL_CD);
            ht.Add("@PLUS_MINUS_CD", PLUS_MINUS_CD);
            ht.Add("@BUDGET_ACC", BUDGET_ACC);
            ht.Add("@ACCOUNTING_ACC", ACCOUNTING_ACC);
            ht.Add("@MONEY_CD", MONEY_CD);
            ht.Add("@CASE_CD", CASE_CD);
            ht.Add("@NT_AMOUNT", NT_AMOUNT);
            ht.Add("@INS2_INSTEAD_AMOUNT", INS2_INSTEAD_AMOUNT);
            ht.Add("@SMMARY", SMMARY);
            ht.Add("@GET_MONEY_DT", GET_MONEY_DT);
            ht.Add("@PAY_TO", PAY_TO);
            ht.Add("@PAY_TO_LICENSE_ID", PAY_TO_LICENSE_ID);
            ht.Add("@WANT_DATE", WANT_DATE);
            ht.Add("@SWITCH_TO", SWITCH_TO);
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

    public void deleteVANDOR_BILL()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();


            sb.Append(" delete from TB_S_M_INS2_VANDOR");
            sb.Append(" where left(Convert(varchar, GET_MONEY_DT,112),6) = @SALARY_YM");
            ht.Add("@SALARY_YM", SALARY_YM);

            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }

    public void deleteTB_S_R_INS2_SALARY_MONTH()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();


            sb.Append(" delete from TB_S_R_INS2_SALARY_MONTH");
            sb.Append(" where SALARY_YM = @SALARY_YM");
            ht.Add("@SALARY_YM", SALARY_YM);

            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable selectMonthData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            
            sb.Append(@"select A.SALARY_DT,A.ACC_CD,A.ACC_WS,A.ACC_DEPT_NO,A.PLANT_CD,A.CAR_TYPE,A.COST_DEPT_NO,
                        A.BUDGET_DEPT_NO,sum(A.fixTotal)fixTotal,sum(A.floatTotal)floatTotal,sum(A.INS_AMT)INS_AMT,sum(A.BOSS_TAX)BOSS_TAX
                         from
                         (			             
			             select LEFT(CONVERT(varchar,A.SALARY_DT,112),6) as SALARY_DT, isnull(e.ACC_CD,'') as ACC_CD,
                         CASE e.ACC_CD WHEN '4' THEN '1' ELSE '2' END ACC_WS,isnull(e.ACC_DEPT_NO,'') as ACC_DEPT_NO,isnull(b1.PLANT_CD,'') as PLANT_CD,
                         isnull(e.CAR_TYPE,'') as CAR_TYPE,isnull(e.COST_DEPT_NO,'') as COST_DEPT_NO ,isnull(e.BUDGET_DEPT_NO,'') as BUDGET_DEPT_NO,
                         isnull(B.fixTotal,0) as fixTotal,0 as floatTotal, 0 as INS_AMT,0 as BOSS_TAX
                         from TB_S_M_SALARY_PAY A  
                         left join TB_S_M_EMP_RESULT b1 on b1.EMP_ID = A.EMP_ID and LEFT(CONVERT(varchar,b1.SALARY_DT,112),6) = @SALARY_YM                        
                         --left join (select DEPT_NO,EMP_ID,PLANT_CD from TB_H_M_EMP )g    --應該抓當時月份的部門代號                   
                         --on g.EMP_ID = A.EMP_ID
                         left join ( 
                         select distinct(h.DEPT_NO),h.ACC_CD,h.ACC_DEPT_NO,i.BUDGET_DEPT_NO,i.CAR_TYPE,i.COST_DEPT_NO from TB_H_M_DEPT h left join TB_H_M_DEPT_ACC i on h.ACC_DEPT_NO = i.ACC_DEPT_NO where END_DT > (left(@SALARY_YM,4)+'/'+right(@SALARY_YM,2)+'/01' )
                         ) e 
                        on b1.DEPT_NO = e.DEPT_NO                        
                         left join ( 
                        --1
                         select b.COMPANY_CD,LEFT(CONVERT(varchar,s.SALARY_DT,112),6) as DATA_YM,e.ACC_CD,
                         (SELECT ACC_WS = CASE e.ACC_CD WHEN '4' THEN '1' ELSE '2' END)ACC_WS,e.ACC_DEPT_NO,
                        --a.PLANT_CD,
						 b.PLANT_CD,
                         e.CAR_TYPE, e.COST_DEPT_NO,e.BUDGET_DEPT_NO,SUM(s.AMOUNT*s.IS_PLUS)as fixTotal from TB_S_M_SALARY_PAY s
                         left join TB_S_M_EMP_RESULT b on b.EMP_ID = s.EMP_ID and LEFT(CONVERT(varchar,b.SALARY_DT,112),6) = @SALARY_YM   --跟獎金一樣  抓計算當時員工的資料，但2018/02/08 月薪資料來自一月底的員工月檔，故與2月份獎金會差一個月的基本資料落差        
                         left join (select DEPT_NO,EMP_ID,PLANT_CD from TB_H_M_EMP )k                         
                         on k.EMP_ID = s.EMP_ID
                         left join (
                         select distinct(h.DEPT_NO),h.ACC_CD,h.ACC_DEPT_NO,d.BUDGET_DEPT_NO,d.CAR_TYPE,d.COST_DEPT_NO
                         from TB_H_M_DEPT h left join TB_H_M_DEPT_ACC d on h.ACC_DEPT_NO = d.ACC_DEPT_NO where END_DT > (left(@SALARY_YM,4)+'/'+right(@SALARY_YM,2)+'/01' )
                         ) e 
                         on b.DEPT_NO = e.DEPT_NO
                         --on k.DEPT_NO = e.DEPT_NO /* 原本都抓最新的 但若年初部門異動會有問題 */
                         left join TB_H_M_EMP a on a.EMP_ID = s.EMP_ID
                         where s.salary_id not in (select salary_id from TB_S_M_SALARY_ITEM where INS_D = 'Y' ) 
			             and LEFT(CONVERT(varchar,s.SALARY_DT,112),6) = @SALARY_YM --201512 固定薪抓INS_D <> 'Y'且SALARY_TYPE='A'的部分
                         and b.COMPANY_CD ='K' and s.SALARY_TYPE = 'A' and s.TAX_FORMAT ='50' and s.SALARY_ID not in ('1029','4001','1018','1012')
                         group by b.COMPANY_CD,LEFT(CONVERT(varchar,s.SALARY_DT,112),6),e.ACC_CD,e.ACC_DEPT_NO,
                         --a.PLANT_CD, 
                         b.PLANT_CD, e.CAR_TYPE, e.COST_DEPT_NO,e.BUDGET_DEPT_NO
                         )B
                         on A.DATA_YM = B.DATA_YM and e.ACC_CD = B.ACC_CD
                         and ACC_WS = B.ACC_WS and e.ACC_DEPT_NO = B.ACC_DEPT_NO and b1.PLANT_CD = B.PLANT_CD and e.CAR_TYPE = B.CAR_TYPE and e.COST_DEPT_NO = B.COST_DEPT_NO and e.BUDGET_DEPT_NO = B.BUDGET_DEPT_NO 
                         where LEFT(CONVERT(varchar,A.SALARY_DT,112),6) = @SALARY_YM
                         and (B.COMPANY_CD ='K' or isnull(B.COMPANY_CD,'')  ='' )
                         group by LEFT(CONVERT(varchar,A.SALARY_DT,112),6),e.ACC_CD,e.ACC_DEPT_NO,b1.PLANT_CD, e.CAR_TYPE,e.COST_DEPT_NO,e.BUDGET_DEPT_NO,B.fixTotal
                         union 
                        --2  20150901  獎金改抓發放日期年月 並分別抓TB_S_M_EMP_RESULT 與TB_S_M_EMP_RESULT_TMP 人頭檔
                        select LEFT(CONVERT(varchar,M.SALARY_DT,112),6) SALARY_DT ,
                        IIF (ISNULL(M.ACC_CD,'')='',e.ACC_CD,M.ACC_CD  ) as ACC_CD, 
                        CASE IIF (ISNULL(M.ACC_CD,'')='',e.ACC_CD,M.ACC_CD  ) WHEN '4' THEN '1' ELSE '2' END  ACC_WS,
                         IIF (ISNULL(M.ACC_DEPT_NO,'')='',e.ACC_DEPT_NO,M.ACC_DEPT_NO  ) as ACC_DEPT_NO,
                         M.PLANT_CD, 
                         IIF (ISNULL(M.CAR_TYPE,'')='',e.CAR_TYPE,M.CAR_TYPE  ) as CAR_TYPE,                                     
                         IIF (ISNULL(M.COST_DEPT_NO,'')='',e.COST_DEPT_NO,M.COST_DEPT_NO  ) as COST_DEPT_NO,
                         IIF (ISNULL(M.BUDGET_DEPT_NO,'')='',e.BUDGET_DEPT_NO,M.BUDGET_DEPT_NO  ) as BUDGET_DEPT_NO,
                        0 fixTotal,M.floatTotal, 0 as INS_AMT,0 as BOSS_TAX
                                    from 
                                    (
                                     --月薪類 雇主其他非固定薪(獎金)
                                     select t.COMPANY_CD,LEFT(CONVERT(varchar,b.SALARY_DT,112),6)SALARY_DT,e.ACC_CD, CASE e.ACC_CD WHEN '4' THEN '1' ELSE '2' END ACC_WS_M,e.ACC_DEPT_NO,t.PLANT_CD, e.CAR_TYPE,
						             e.COST_DEPT_NO,e.BUDGET_DEPT_NO, SUM(b.AMOUNT*b.IS_PLUS) floatTotal from TB_S_M_SALARY_PAY b						 
						             left join TB_S_M_EMP_RESULT t on t.SALARY_DT = b.SALARY_DT and t.EMP_ID = b.EMP_ID
						             left join TB_S_M_SALARY_ITEM c on b.SALARY_ID = c.SALARY_ID and c.INS_D = 'Y' 
						             --left join ( select DEPT_NO,EMP_ID,PLANT_CD from TB_H_M_EMP  )a on a.EMP_ID = b.EMP_ID 
						             left join ( select distinct(h.DEPT_NO),h.ACC_CD,h.ACC_DEPT_NO,d.BUDGET_DEPT_NO,d.CAR_TYPE,d.COST_DEPT_NO from TB_H_M_DEPT h join TB_H_M_DEPT_ACC d 
									             on h.ACC_DEPT_NO = d.ACC_DEPT_NO where END_DT > (left(@SALARY_YM,4)+'/'+right(@SALARY_YM,2)+'/01' ) ) e on t.DEPT_NO = e.DEPT_NO 
			                         where LEFT(CONVERT(varchar,b.SALARY_DT,112),6) = @SALARY_YM and b.SALARY_TYPE='A' and b.TAX_FORMAT='50' and t.COMPANY_CD ='K' and c.INS_D = 'Y' 
			                         group by t.COMPANY_CD,LEFT(CONVERT(varchar,b.SALARY_DT,112),6),e.ACC_CD, e.ACC_DEPT_NO,t.PLANT_CD,e.CAR_TYPE,e.COST_DEPT_NO,e.BUDGET_DEPT_NO 
                                     --非月薪類 雇主其他非固定薪(獎金)
                                     union all
                                     select t.COMPANY_CD,LEFT(CONVERT(varchar,b.SALARY_DT,112),6)SALARY_DT,e.ACC_CD, CASE e.ACC_CD WHEN '4' THEN '1' ELSE '2' END ACC_WS_M,e.ACC_DEPT_NO,t.PLANT_CD, e.CAR_TYPE,
						             e.COST_DEPT_NO,e.BUDGET_DEPT_NO, SUM(b.AMOUNT*b.IS_PLUS) floatTotal from TB_S_M_SALARY_PAY b 
						             left join TB_S_M_EMP_RESULT_TMP t on t.SALARY_DT = b.SALARY_DT and t.SALARY_TYPE = b.SALARY_TYPE and t.PAY_KIND = b.PAY_KIND and t.EMP_ID = b.EMP_ID 						 
						             left join TB_S_M_SALARY_ITEM c on b.SALARY_ID = c.SALARY_ID and c.INS_D = 'Y' 
						             --left join ( select DEPT_NO,EMP_ID,PLANT_CD from TB_H_M_EMP  )a on a.EMP_ID = b.EMP_ID 
						             left join ( select distinct(h.DEPT_NO),h.ACC_CD,h.ACC_DEPT_NO,d.BUDGET_DEPT_NO,d.CAR_TYPE,d.COST_DEPT_NO from TB_H_M_DEPT h join TB_H_M_DEPT_ACC d 
									             on h.ACC_DEPT_NO = d.ACC_DEPT_NO where END_DT > (left(@SALARY_YM,4)+'/'+right(@SALARY_YM,2)+'/01' ) ) e on t.DEPT_NO = e.DEPT_NO 
			                         where LEFT(CONVERT(varchar,b.SALARY_DT,112),6) = @SALARY_YM and (b.SALARY_TYPE='C' or b.SALARY_TYPE='D') and b.TAX_FORMAT='50' and t.COMPANY_CD ='K' and c.INS_D = 'Y' 
			                         group by t.COMPANY_CD,LEFT(CONVERT(varchar,b.SALARY_DT,112),6),e.ACC_CD, e.ACC_DEPT_NO,t.PLANT_CD,e.CAR_TYPE,e.COST_DEPT_NO,e.BUDGET_DEPT_NO
                          )M     
                         left join (select distinct(h.DEPT_NO),h.ACC_CD,h.ACC_DEPT_NO,d.BUDGET_DEPT_NO,d.CAR_TYPE, d.COST_DEPT_NO 
                                          from TB_H_M_DEPT h 
                                          left join TB_H_M_DEPT_ACC d on h.ACC_DEPT_NO = d.ACC_DEPT_NO 
                                          where END_DT > (left(@SALARY_YM,4)+'/'+right(@SALARY_YM,2)+'/01' ) 
						 )e 
						            on e.DEPT_NO = 'KB00000'
						             where LEFT(CONVERT(varchar,M.SALARY_DT,112),6) = @SALARY_YM 
						             and (M.COMPANY_CD= 'K' or isnull(M.COMPANY_CD,'')  ='' ) 
                                     group by LEFT(CONVERT(varchar,M.SALARY_DT,112),6),IIF (ISNULL(M.ACC_CD,'')='',e.ACC_CD,M.ACC_CD  ),IIF (ISNULL(M.ACC_DEPT_NO,'')='',e.ACC_DEPT_NO,M.ACC_DEPT_NO  ),
									 M.PLANT_CD, IIF (ISNULL(M.CAR_TYPE,'')='',e.CAR_TYPE,M.CAR_TYPE  ),IIF (ISNULL(M.COST_DEPT_NO,'')='',e.COST_DEPT_NO,M.COST_DEPT_NO  ), 
									 IIF (ISNULL(M.BUDGET_DEPT_NO,'')='',e.BUDGET_DEPT_NO,M.BUDGET_DEPT_NO  ),M.floatTotal
                         union
                        --3
                         select f.SALARY_YM as SALARY_DT,e.ACC_CD,
                         (SELECT ACC_WS = CASE e.ACC_CD WHEN '4' THEN '1' ELSE '2' END)ACC_WS_N,e.ACC_DEPT_NO,a.PLANT_CD, e.CAR_TYPE,
                         e.COST_DEPT_NO,e.BUDGET_DEPT_NO,0 as fixTotal,0 as floatTotal,SUM(INS_AMT)INS_AMT,0 as BOSS_TAX
                         from TB_I_R_FEES_MONTH f           
                         left join (select distinct(DEPT_NO),COMPANY_CD,SALARY_YM,EMP_ID,PLANT_CD from TB_S_M_EMP_RESULT where SALARY_YM = @SALARY_YM ) a on a.SALARY_YM = f.SALARY_YM and a.EMP_ID = f.EMP_ID
                         left join ( select distinct(h.DEPT_NO),h.ACC_CD,h.ACC_DEPT_NO,d.BUDGET_DEPT_NO,d.CAR_TYPE,d.COST_DEPT_NO
                         from TB_H_M_DEPT h join TB_H_M_DEPT_ACC d on h.ACC_DEPT_NO = d.ACC_DEPT_NO where END_DT > (left(@SALARY_YM,4)+'/'+right(@SALARY_YM,2)+'/01' ) ) e on a.DEPT_NO = e.DEPT_NO
                         where INS_TYPE = 'B' and IDENTITY_KIND = '1' and a.COMPANY_CD = 'K' and f.SALARY_YM = @SALARY_YM 
                         group by f.SALARY_YM,e.ACC_CD, e.ACC_DEPT_NO,a.PLANT_CD, e.CAR_TYPE,e.COST_DEPT_NO,e.BUDGET_DEPT_NO
                         union
                        --4 二代健保代扣保費上傳檔
                         select left(Convert(varchar, PAYMENT_DATE,112),6)PAYMENT_DATE,e.ACC_CD,
                        (SELECT ACC_WS = CASE e.ACC_CD WHEN '4' THEN '1' ELSE '2' END)ACC_WS_P,e.ACC_DEPT_NO,'1' as PLANT_CD, 
                        e.CAR_TYPE,e.COST_DEPT_NO,e.BUDGET_DEPT_NO,0 as fixTotal,0 as floatTotal,0 as INS_AMT, 
                        SUM(AMOUNT) as BOSS_TAX 
                        from TB_S_R_INS2_UPD_DATA 
                        left join (select distinct(h.DEPT_NO),h.ACC_CD,h.ACC_DEPT_NO,d.BUDGET_DEPT_NO,d.CAR_TYPE, 
                        d.COST_DEPT_NO from TB_H_M_DEPT h left join TB_H_M_DEPT_ACC d on h.ACC_DEPT_NO = d.ACC_DEPT_NO where END_DT > (left(@SALARY_YM,4)+'/'+right(@SALARY_YM,2)+'/01' ) )e 
                        on e.DEPT_NO = @DEPT where left(Convert(varchar, PAYMENT_DATE,112),6) = @SALARY_YM 
                        group by left(Convert(varchar, PAYMENT_DATE,112),6),e.ACC_CD, e.ACC_DEPT_NO,e.CAR_TYPE,e.COST_DEPT_NO,e.BUDGET_DEPT_NO             

                         )A
                         group by A.SALARY_DT,A.ACC_CD,A.ACC_WS,A.ACC_DEPT_NO,A.PLANT_CD, A.CAR_TYPE,A.COST_DEPT_NO,A.BUDGET_DEPT_NO ");                
            
/*
            //用union 拆5段分開看
            sb.Append("select A.SALARY_DT,A.ACC_CD,A.ACC_WS,A.ACC_DEPT_NO,A.PLANT_CD,A.CAR_TYPE,A.COST_DEPT_NO,");
            sb.Append("A.BUDGET_DEPT_NO,sum(A.fixTotal)fixTotal,sum(A.floatTotal)floatTotal,sum(A.INS_AMT)INS_AMT,sum(A.BOSS_TAX)BOSS_TAX");
            sb.Append(" from");
            sb.Append(" (select A.DATA_YM as SALARY_DT,e.ACC_CD,");
            sb.Append(" CASE e.ACC_CD WHEN '4' THEN '1' ELSE '2' END ACC_WS,e.ACC_DEPT_NO,g.PLANT_CD,");
            sb.Append(" e.CAR_TYPE,e.COST_DEPT_NO,e.BUDGET_DEPT_NO,B.fixTotal,0 as floatTotal,");
            sb.Append(" 0 as INS_AMT,0 as BOSS_TAX");
            sb.Append(" from TB_S_M_SALARY_PAY A");
            sb.Append(" left join (select DEPT_NO,EMP_ID,PLANT_CD from TB_H_M_EMP )g");
            sb.Append(" on g.EMP_ID = A.EMP_ID");
            sb.Append(" left join ( ");
            sb.Append(" select distinct(h.DEPT_NO),h.ACC_CD,h.ACC_DEPT_NO,i.BUDGET_DEPT_NO,i.CAR_TYPE,i.COST_DEPT_NO from TB_H_M_DEPT h left join TB_H_M_DEPT_ACC i on h.ACC_DEPT_NO = i.ACC_DEPT_NO");
            sb.Append(" ) e on g.DEPT_NO = e.DEPT_NO");
            sb.Append(" left join ( ");
            //--1
            sb.Append(" select b.COMPANY_CD,s.DATA_YM,e.ACC_CD,");
            sb.Append(" (SELECT ACC_WS = CASE e.ACC_CD WHEN '4' THEN '1' ELSE '2' END)ACC_WS,e.ACC_DEPT_NO,a.PLANT_CD, e.CAR_TYPE,");
            sb.Append(" e.COST_DEPT_NO,e.BUDGET_DEPT_NO,SUM(s.AMOUNT*s.IS_PLUS)as fixTotal from TB_S_M_SALARY_PAY s");
            sb.Append(" left join TB_S_M_EMP_RESULT b on b.EMP_ID = s.EMP_ID and b.SALARY_YM = @SALARY_YM ");
            
            sb.Append(" left join (select DEPT_NO,EMP_ID,PLANT_CD from TB_H_M_EMP )k");
            sb.Append(" on k.EMP_ID = s.EMP_ID");
            sb.Append(" left join (");
            sb.Append(" select distinct(h.DEPT_NO),h.ACC_CD,h.ACC_DEPT_NO,d.BUDGET_DEPT_NO,d.CAR_TYPE,d.COST_DEPT_NO");
            sb.Append(" from TB_H_M_DEPT h left join TB_H_M_DEPT_ACC d on h.ACC_DEPT_NO = d.ACC_DEPT_NO");
            sb.Append(" ) e on k.DEPT_NO = e.DEPT_NO");
            sb.Append(" left join TB_H_M_EMP a on a.EMP_ID = s.EMP_ID");
            sb.Append(" where s.salary_id not in (select salary_id from TB_S_M_SALARY_ITEM where INS_D = 'Y' ) and s.DATA_YM = @SALARY_YM");//201512 固定薪抓INS_D <> 'Y'且SALARY_TYPE='A'的部分
            sb.Append(" and b.COMPANY_CD ='K' and s.SALARY_TYPE = 'A' and s.TAX_FORMAT ='50' and s.SALARY_ID not in ('1029')");
            sb.Append(" group by b.COMPANY_CD,s.DATA_YM,e.ACC_CD,e.ACC_DEPT_NO,a.PLANT_CD, e.CAR_TYPE,");
            sb.Append(" e.COST_DEPT_NO,e.BUDGET_DEPT_NO");
            sb.Append(" )B");
            sb.Append(" on A.DATA_YM = B.DATA_YM and e.ACC_CD = B.ACC_CD");
            sb.Append(" and ACC_WS = B.ACC_WS and e.ACC_DEPT_NO = B.ACC_DEPT_NO and g.PLANT_CD = B.PLANT_CD and e.CAR_TYPE = B.CAR_TYPE and e.COST_DEPT_NO = B.COST_DEPT_NO and e.BUDGET_DEPT_NO = B.BUDGET_DEPT_NO ");
            sb.Append(" where A.DATA_YM = @SALARY_YM");//
            sb.Append(" and (B.COMPANY_CD ='K' or isnull(B.COMPANY_CD,'')  ='' )");
            sb.Append(" group by A.DATA_YM,e.ACC_CD,e.ACC_DEPT_NO,g.PLANT_CD, e.CAR_TYPE,e.COST_DEPT_NO,e.BUDGET_DEPT_NO,B.fixTotal");
            sb.Append(" union ");
            //--2  20150901  獎金改抓發放日期年月 並分別抓TB_S_M_EMP_RESULT 與TB_S_M_EMP_RESULT_TMP 人頭檔
            sb.Append(@" select LEFT(CONVERT(varchar,M.SALARY_DT,112),6) SALARY_DT ,M.ACC_CD, CASE M.ACC_CD WHEN '4' THEN '1' ELSE '2' END  ACC_WS,M.ACC_DEPT_NO,M.PLANT_CD, M.CAR_TYPE,
                        M.COST_DEPT_NO,M.BUDGET_DEPT_NO,0 fixTotal,M.floatTotal, 0 as INS_AMT,0 as BOSS_TAX
                        from 
                        ( ");            

            sb.Append(@" --月薪類 雇主其他非固定薪(獎金)
                         select t.COMPANY_CD,LEFT(CONVERT(varchar,b.SALARY_DT,112),6)SALARY_DT,e.ACC_CD, CASE e.ACC_CD WHEN '4' THEN '1' ELSE '2' END ACC_WS_M,e.ACC_DEPT_NO,t.PLANT_CD, e.CAR_TYPE,
						 e.COST_DEPT_NO,e.BUDGET_DEPT_NO, SUM(b.AMOUNT*b.IS_PLUS) floatTotal from TB_S_M_SALARY_PAY b						 
						 left join TB_S_M_EMP_RESULT t on t.SALARY_DT = b.SALARY_DT and t.EMP_ID = b.EMP_ID
						 left join TB_S_M_SALARY_ITEM c on b.SALARY_ID = c.SALARY_ID and c.INS_D = 'Y' 
						 --left join ( select DEPT_NO,EMP_ID,PLANT_CD from TB_H_M_EMP  )a on a.EMP_ID = b.EMP_ID 
						 left join ( select distinct(h.DEPT_NO),h.ACC_CD,h.ACC_DEPT_NO,d.BUDGET_DEPT_NO,d.CAR_TYPE,d.COST_DEPT_NO from TB_H_M_DEPT h join TB_H_M_DEPT_ACC d 
									 on h.ACC_DEPT_NO = d.ACC_DEPT_NO ) e on t.DEPT_NO = e.DEPT_NO 
			             where LEFT(CONVERT(varchar,b.SALARY_DT,112),6) = @SALARY_YM and b.SALARY_TYPE='A' and b.TAX_FORMAT='50' and t.COMPANY_CD ='K' and c.INS_D = 'Y' 
			             group by t.COMPANY_CD,LEFT(CONVERT(varchar,b.SALARY_DT,112),6),e.ACC_CD, e.ACC_DEPT_NO,t.PLANT_CD,e.CAR_TYPE,e.COST_DEPT_NO,e.BUDGET_DEPT_NO 
                         --非月薪類 雇主其他非固定薪(獎金)
                         union all
                         select t.COMPANY_CD,LEFT(CONVERT(varchar,b.SALARY_DT,112),6)SALARY_DT,e.ACC_CD, CASE e.ACC_CD WHEN '4' THEN '1' ELSE '2' END ACC_WS_M,e.ACC_DEPT_NO,t.PLANT_CD, e.CAR_TYPE,
						 e.COST_DEPT_NO,e.BUDGET_DEPT_NO, SUM(b.AMOUNT*b.IS_PLUS) floatTotal from TB_S_M_SALARY_PAY b 
						 left join TB_S_M_EMP_RESULT_TMP t on t.SALARY_DT = b.SALARY_DT and t.SALARY_TYPE = b.SALARY_TYPE and t.PAY_KIND = b.PAY_KIND and t.EMP_ID = b.EMP_ID 						 
						 left join TB_S_M_SALARY_ITEM c on b.SALARY_ID = c.SALARY_ID and c.INS_D = 'Y' 
						 --left join ( select DEPT_NO,EMP_ID,PLANT_CD from TB_H_M_EMP  )a on a.EMP_ID = b.EMP_ID 
						 left join ( select distinct(h.DEPT_NO),h.ACC_CD,h.ACC_DEPT_NO,d.BUDGET_DEPT_NO,d.CAR_TYPE,d.COST_DEPT_NO from TB_H_M_DEPT h join TB_H_M_DEPT_ACC d 
									 on h.ACC_DEPT_NO = d.ACC_DEPT_NO ) e on t.DEPT_NO = e.DEPT_NO 
			             where LEFT(CONVERT(varchar,b.SALARY_DT,112),6) = @SALARY_YM and (b.SALARY_TYPE='C' or b.SALARY_TYPE='D') and b.TAX_FORMAT='50' and t.COMPANY_CD ='K' and c.INS_D = 'Y' 
			             group by t.COMPANY_CD,LEFT(CONVERT(varchar,b.SALARY_DT,112),6),e.ACC_CD, e.ACC_DEPT_NO,t.PLANT_CD,e.CAR_TYPE,e.COST_DEPT_NO,e.BUDGET_DEPT_NO");
            sb.Append(@"  )M
						 where LEFT(CONVERT(varchar,M.SALARY_DT,112),6) = @SALARY_YM 
						 and (M.COMPANY_CD= 'K' or isnull(M.COMPANY_CD,'')  ='' ) 
                         group by LEFT(CONVERT(varchar,M.SALARY_DT,112),6),M.ACC_CD,M.ACC_DEPT_NO,M.PLANT_CD, M.CAR_TYPE,M.COST_DEPT_NO, M.BUDGET_DEPT_NO,M.floatTotal ");
            
            
            sb.Append(" union");
            //--3
            sb.Append(" select f.SALARY_YM as SALARY_DT,e.ACC_CD,");
            sb.Append(" (SELECT ACC_WS = CASE e.ACC_CD WHEN '4' THEN '1' ELSE '2' END)ACC_WS_N,e.ACC_DEPT_NO,a.PLANT_CD, e.CAR_TYPE,");
            sb.Append(" e.COST_DEPT_NO,e.BUDGET_DEPT_NO,0 as fixTotal,0 as floatTotal,SUM(INS_AMT)INS_AMT,0 as BOSS_TAX");
            sb.Append(" from TB_I_R_FEES_MONTH f");           
            sb.Append(" left join (select distinct(DEPT_NO),COMPANY_CD,SALARY_YM,EMP_ID,PLANT_CD from TB_S_M_EMP_RESULT where SALARY_YM = @SALARY_YM ) a on a.SALARY_YM = f.SALARY_YM and a.EMP_ID = f.EMP_ID");
            sb.Append(" left join ( select distinct(h.DEPT_NO),h.ACC_CD,h.ACC_DEPT_NO,d.BUDGET_DEPT_NO,d.CAR_TYPE,d.COST_DEPT_NO");
            sb.Append(" from TB_H_M_DEPT h join TB_H_M_DEPT_ACC d on h.ACC_DEPT_NO = d.ACC_DEPT_NO ) e on a.DEPT_NO = e.DEPT_NO");
            sb.Append(" where INS_TYPE = 'B' and IDENTITY_KIND = '1' and a.COMPANY_CD = 'K' and f.SALARY_YM = @SALARY_YM ");
            sb.Append(" group by f.SALARY_YM,e.ACC_CD, e.ACC_DEPT_NO,a.PLANT_CD, e.CAR_TYPE,e.COST_DEPT_NO,e.BUDGET_DEPT_NO");
            sb.Append(" union");

            //--4 二代健保代扣保費上傳檔
            sb.Append(" select left(Convert(varchar, PAYMENT_DATE,112),6)PAYMENT_DATE,e.ACC_CD,");
            sb.Append("(SELECT ACC_WS = CASE e.ACC_CD WHEN '4' THEN '1' ELSE '2' END)ACC_WS_P,e.ACC_DEPT_NO,'' as PLANT_CD, ");
            sb.Append("e.CAR_TYPE,e.COST_DEPT_NO,e.BUDGET_DEPT_NO,0 as fixTotal,0 as floatTotal,0 as INS_AMT, ");
            sb.Append("SUM(AMOUNT) as BOSS_TAX ");
            sb.Append("from TB_S_R_INS2_UPD_DATA ");
            sb.Append("left join (select distinct(h.DEPT_NO),h.ACC_CD,h.ACC_DEPT_NO,d.BUDGET_DEPT_NO,d.CAR_TYPE, ");
            sb.Append("d.COST_DEPT_NO from TB_H_M_DEPT h left join TB_H_M_DEPT_ACC d on h.ACC_DEPT_NO = d.ACC_DEPT_NO )e ");
            sb.Append("on e.DEPT_NO = @DEPT where left(Convert(varchar, PAYMENT_DATE,112),6) = @SALARY_YM ");
            sb.Append("group by left(Convert(varchar, PAYMENT_DATE,112),6),e.ACC_CD, e.ACC_DEPT_NO,e.CAR_TYPE,e.COST_DEPT_NO,e.BUDGET_DEPT_NO ");            

            sb.Append(" )A");
            sb.Append(" group by A.SALARY_DT,A.ACC_CD,A.ACC_WS,A.ACC_DEPT_NO,A.PLANT_CD, A.CAR_TYPE,A.COST_DEPT_NO,A.BUDGET_DEPT_NO");                   
*/
            ht.Add("@SALARY_YM", SALARY_YM);
            ht.Add("@PLANT", PLANT);
            ht.Add("@DEPT", DEPT);



            DataTable dt = dbConn.Query(sb, ht);


            return dt;

        }
        catch (Exception)
        {
            throw;
        }
    }

    public void insertM1Data()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            

            sb.Append(" insert into TB_S_R_INS2_SALARY_MONTH");
            sb.Append(" (SALARY_YM,ACC_CD,ACC_WS,SALARY_DEPT,PLANT_CD,CAR_KIND,COST_DEPT_NO,BUDGET_DEPT_NO,");
            sb.Append(" FLOAT_S_TOTAL,MONTH_S_TOTAL,TOTAL_INS,BOSS_FLOAT_SALARY,BOSS_OTHER_SALARY,BOSS_TAX,INS2_BASE,INS2_COST,");
            sb.Append(" CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" values(@SALARY_YM_M,@ACC_CD,@ACC_WS,@SALARY_DEPT,@PLANT_CD,@CAR_KIND,@COST_DEPT_NO,@BUDGET_DEPT_NO,");
            sb.Append(" @FLOAT_S_TOTAL,@MONTH_S_TOTAL,@TOTAL_INS,0,0,@BOSS_TAX,@INS2_BASE,@INS2_COST,");
            sb.Append(" @CREATED_BY,getdate(),@UPDATED_BY,getdate(),@FUNC_ID)");

            ht.Add("@SALARY_YM_M", SALARY_YM_M);
            ht.Add("@ACC_CD", ACC_CD);
            ht.Add("@ACC_WS", ACC_WS);
            ht.Add("@SALARY_DEPT", SALARY_DEPT);
            ht.Add("@PLANT_CD", PLANT_CD);
            ht.Add("@CAR_KIND", CAR_KIND);
            ht.Add("@COST_DEPT_NO", COST_DEPT_NO);
            ht.Add("@BUDGET_DEPT_NO", BUDGET_DEPT_NO);
            ht.Add("@FLOAT_S_TOTAL", FLOAT_S_TOTAL);
            ht.Add("@MONTH_S_TOTAL", MONTH_S_TOTAL);
            ht.Add("@TOTAL_INS", TOTAL_INS);
            ht.Add("@BOSS_TAX", BOSS_TAX);            
            ht.Add("@INS2_BASE", INS2_BASE);
            ht.Add("@INS2_COST", INS2_COST);
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

    public DataTable selectMonth2Data()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            
            sb.Append(" select left(Convert(varchar, b.SALARY_DT,112),6)SALARY_DT,e.ACC_CD,");
            sb.Append(" (SELECT ACC_WS = CASE e.ACC_CD WHEN '4' THEN '1' ELSE '2' END)ACC_WS,e.ACC_DEPT_NO,a.PLANT_CD,");
            sb.Append(" e.CAR_TYPE,e.COST_DEPT_NO,e.BUDGET_DEPT_NO,SUM(b.AMOUNT*b.IS_PLUS) floatTotal");
            sb.Append(" from TB_S_M_SALARY_PAY b");
            sb.Append(" left join TB_S_M_SALARY_ITEM c on b.SALARY_ID = c.SALARY_ID and c.INS_D = 'Y'");
            sb.Append(" left join (");
            sb.Append(" select h.DEPT_NO,h.ACC_CD,h.ACC_DEPT_NO,d.BUDGET_DEPT_NO,d.CAR_TYPE,d.COST_DEPT_NO");
            sb.Append(" from TB_H_M_DEPT h ");
            sb.Append(" join TB_H_M_DEPT_ACC d on h.ACC_DEPT_NO = d.ACC_DEPT_NO");
            sb.Append(" ) e on b.DEPT_NO = e.DEPT_NO");
            sb.Append(" left join TB_H_M_EMP a on b.EMP_ID = a.EMP_ID");
            sb.Append(" where left(Convert(varchar, b.SALARY_DT,112),6) = @SALARY_YM");
            sb.Append(" and (b.SALARY_TYPE='C' or b.SALARY_TYPE='D')and b.TAX_FORMAT='50'");
            sb.Append(" group by left(Convert(varchar, b.SALARY_DT,112),6),e.ACC_CD,");
            sb.Append(" e.ACC_DEPT_NO,a.PLANT_CD,e.CAR_TYPE,e.COST_DEPT_NO,e.BUDGET_DEPT_NO");           


            ht.Add("@SALARY_YM", SALARY_YM);



            DataTable dt = dbConn.Query(sb, ht);


            return dt;

        }
        catch (Exception)
        {
            throw;
        }
    }


    public void insertM2Data()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" insert into TB_S_R_INS2_SALARY_MONTH");
            sb.Append(" (SALARY_YM,ACC_CD,ACC_WS,SALARY_DEPT,PLANT_CD,CAR_KIND,COST_DEPT_NO,BUDGET_DEPT_NO,");
            sb.Append("  MONTH_S_TOTAL,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" values(@SALARY_YM_M,@ACC_CD,@ACC_WS,@SALARY_DEPT,@PLANT_CD,@CAR_KIND,@COST_DEPT_NO,@BUDGET_DEPT_NO,");
            sb.Append(" @MONTH_S_TOTAL,@CREATED_BY,getdate(),@UPDATED_BY,getdate(),@FUNC_ID)");

            ht.Add("@SALARY_YM_M", SALARY_YM_M);
            ht.Add("@ACC_CD", ACC_CD);
            ht.Add("@ACC_WS", ACC_WS);
            ht.Add("@SALARY_DEPT", SALARY_DEPT);
            ht.Add("@PLANT_CD", PLANT_CD);
            ht.Add("@CAR_KIND", CAR_KIND);
            ht.Add("@COST_DEPT_NO", COST_DEPT_NO);
            ht.Add("@BUDGET_DEPT_NO", BUDGET_DEPT_NO);
            ht.Add("@MONTH_S_TOTAL", MONTH_S_TOTAL);
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

    public void getManageDept()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select SUB_CD,CODE_VAL1 from TB_9_M_COMM_D");
            sb.Append(" where SYS_CD ='IB' and MAIN_CD='MANAGER_DEPT' and IS_VALID='Y'");
           

            DataTable dt = dbConn.Query(sb, ht);
            if(dt.Rows.Count>0){
                for(int i=0;i<dt.Rows.Count;i++){
                    DEPT = dt.Rows[0]["SUB_CD"].ToString();
                    PLANT = dt.Rows[0]["CODE_VAL1"].ToString();
                }
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





}