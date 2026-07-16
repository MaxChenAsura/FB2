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


/// <summary>
/// CFB2SC5500BO 的摘要描述
/// </summary>
public class CFB2SC5500DAO : BaseDAO
{
    public CFB2SC5500DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯  -->薪資所得扣繳額繳款書
        //
    }
    public string RowNumber { get; set; }
    public string QDATAKEY { get; set; }
    public string DATA_YM { get; set; }
    public string EMP_ID { get; set; }
    public string DEPT_NO { get; set; }
    public string JPN_CD { get; set; }
    public string EMP_NAME { get; set; }
    public string EMP_CD { get; set; }
    public string SALARY_ID { get; set; }
    public string SALARY_YM { get; set; }
    public string SALARY_NAME { get; set; }
    public string CHG_AMT_A { get; set; }
    public string CHG_AMT_B { get; set; }
    public string PROCESS_STATUS { get; set; }
    public string START_DT_B { get; set; }
    public string START_DT_A { get; set; }
    public string END_DATE_B { get; set; }
    public string END_DATE_A { get; set; }
    public string CHG_STATUS { get; set; }
    public string APPROVE_DT { get; set; }
    public string APPROVE_BY { get; set; }
    public string REMARK { get; set; }
    public string APP_REMARK { get; set; }
    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }

    public string IS_PLUS { get; set; }
    public string IS_TAX { get; set; }

    public DataTable getJPN_CD()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select SYS_CD , MAIN_CD , SUB_CD ,SUB_DESC from TB_9_M_COMM_D");
            sb.Append(" where SYS_CD='HB' and MAIN_CD='JPN_CD' and IS_VALID = 'Y' ");
            return dbConn.Query(sb);
        }
        catch
        {
            throw;
        }
    }

    public string getLast_SALARY_YM()
    {
        try
        {
            string t = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select dbo.FN_S_SALARY_YM() ");

            DataTable dt = dbConn.Query(sb);
            if (dt.Rows.Count > 0)
            {
                t = dt.Rows[0][0].ToString();
            }
            return t;
        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable get_PDF_Data(string SALARY_YM)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
//            sb.Append(@"select tr.INCOME_CD,sum(tr.CNT) as cnt,sum(tr.tax) as tax,sum(tr.amount) as amount from
//                        (           
//	                        select td.CNT,td.TAX,td.AMOUNT ,
//	                            CASE td.INCOME_CD                
//				                        when '1'  then '(1)固定薪資按扣繳稅額表扣繳'              
//				                        when '2'  then '(2)非固定薪資及兼職薪資一律按5%扣繳'              
//				                        when '3'  then '(3)給付中華民國境內居住之個人薪資按10%扣繳'              
//				                        when '4'  then case td.JPN_CD when '' then '(9)給付中華民國境內居住之個人薪資按18%扣繳' else '(4)給付非中華民國境內居住之個人薪資按18%扣繳'  end            
//				                        when '5'  then '(5)給付非中華民國境內居住之個人薪資按30%扣繳'              
//				                        when '6'  then '(6)給付非中華民國境內居住之個人薪資按40%扣繳'              
//				                        when '0'  then '(7)固定薪資定額扣繳'              
//				                        when 'N'  then '(8)免扣繳人員'  end as 'INCOME_CD' 
//	                        from 
//	                        (
//		                    Select tt.INCOME_CD,tt.JPN_CD,Count(tt.EMP_ID) CNT,Sum(tt.TAX) as TAX,Sum(tt.AMOUNT) as AMOUNT	 					
//		                    From (
//		                        Select B.EMP_ID,B.JPN_CD,(Case ISNULL(A.TAX,0) When 0 Then 'N' Else A.INCOME_CD End) As INCOME_CD,ISNULL(A.TAX,'0') TAX ,B.AMOUNT  
//		                        From ( 
//				                        Select tamt.EMP_ID,isnull(tamt1.JPN_CD,'') as JPN_CD ,tamt1.INCOME_CD,Sum(tamt.AMOUNT * tamt.IS_PLUS) AMOUNT  
//				                        From TB_S_M_SALARY_PAY as tamt
//				                        left join TB_S_M_EMP_RESULT tamt1 on tamt1.emp_id = tamt.emp_id and tamt1.SALARY_YM =@SALARY_YM and tamt1.SALARY_DT = tamt.SALARY_DT
//				                        Where tamt1.company_cd='K' and tamt.SALARY_TYPE = 'A' And tamt.SALARY_ID <> '4001' And tamt.IS_TAX = 'Y' And tamt.PAY_ID Is Not Null And tamt.DATA_YM =@SALARY_YM 
//				                        Group By tamt.EMP_ID,tamt1.JPN_CD,tamt1.INCOME_CD
//			                    ) B
//			                    left Join (
//				                        Select tax0.INCOME_CD,tax0.EMP_ID,Sum(tax0.AMOUNT) TAX	
//				                        From TB_S_M_SALARY_PAY tax0
//                                       left join TB_S_M_EMP_RESULT tax1 on tax0.emp_id = tax1.emp_id and tax1.SALARY_YM =@SALARY_YM and tax0.SALARY_DT =  tax1.SALARY_DT
//				                        Where tax1.company_cd='K' and tax0.SALARY_TYPE = 'A' And tax0.SALARY_ID = '4001' And tax0.PAY_ID Is Not Null And tax0.DATA_YM =@SALARY_YM	
//				                        Group By tax0.EMP_ID,tax0.INCOME_CD
//			                    ) As A On A.EMP_ID = B.EMP_ID 
//	                        )tt group by tt.INCOME_CD,tt.JPN_CD
//	                      )td 
//                        )tr where 1=1  group by tr.income_cd order by tr.income_cd  ");

            sb.Append(" select * from [dbo].[FN_S_TABLE_SC550_PDF_DATA] (@salary_ym) ");
            ht.Add("@salary_ym", SALARY_YM);
            return dbConn.Query(sb,ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
}