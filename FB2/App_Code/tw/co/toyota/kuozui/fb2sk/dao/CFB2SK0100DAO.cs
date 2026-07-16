using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;



/// <summary>
/// CFB2SK0100DAO 的摘要描述
/// </summary>
public class CFB2SK0100DAO : BaseDAO
{
    public string EMP_ID { get; set; }
    public string WELFARE_AMT { get; set; }
    public string SALARY_AMT { get; set; }
    public string SALARY_ID { get; set; }
    public string AMOUNT { get; set; }
    public CFB2SK0100DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    //刪除福利會用對象檔
    public void Delete_TB_S_MUTUAL_TARGET()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append( "Delete From TB_S_M_MUTUAL_TARGET ");
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //讀取薪資明細歷史檔
    public DataTable TB_S_M_SALARY_PAY(string DATA_YM)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@"select EMP_ID, sum(AMOUNT_1020) as AMOUNT_1020, sum(AMOUNT_1030) as AMOUNT_1030
                        from (
                        select EMP_ID,
                        sum(AMOUNT) 
                        as AMOUNT_1020
                        ,0 as AMOUNT_1030
                         from 
                         (  select EMP_ID,SALARY_ID,
	                        case when SALARY_ID in ('2020','2030') then -AMOUNT 
	                        else AMOUNT
	                        end	 as AMOUNT
	                         from TB_S_M_SALARY_PAY                                                    
                            where DATA_YM = @DATA_YM                                                  
                              and SALARY_TYPE = 'A'                                                   
                              and SALARY_ID in ('1020','2020','2030','1030','2030','2030') 
                        ) A                                                
                        where  SALARY_ID in ('1020','2020','2030') 
                        group by EMP_ID	 
                        union ALL
                        select EMP_ID,0 as AMOUNT_1020, sum(AMOUNT) as AMOUNT_1030
                         from 
                         (  select EMP_ID,SALARY_ID,
	                        case when SALARY_ID in ('2020','2030') then -AMOUNT 
	                        else AMOUNT
	                        end	 as AMOUNT
	                         from TB_S_M_SALARY_PAY                                                    
                            where DATA_YM = @DATA_YM                                                  
                              and SALARY_TYPE = 'A'                                                   
                              and SALARY_ID in ('1020','2020','2030','1030','2030','2030') 
                        ) B                                                  
                            where  SALARY_ID in ('1030','2030','2030') 
                        group by EMP_ID	     
                        )  z
                        group by EMP_ID   
"); 

            ht.Add("@DATA_YM", DATA_YM);
            return dbConn.Query(sb, ht);
            
        }
        catch (Exception)
        {

            throw;
        }
    }
    //新增福利會用對象檔
    public void Add_TB_S_MUTUAL_TARGET()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append( "INSERT INTO TB_S_M_MUTUAL_TARGET (EMP_ID,WELFARE_AMT,SALARY_AMT,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append( " Values (@EMP_ID,@WELFARE_AMT,@SALARY_AMT,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");
            ht.Clear();
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@WELFARE_AMT", WELFARE_AMT);
            ht.Add("@SALARY_AMT", SALARY_AMT);
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SK0100");
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //讀取福利會用對象檔
    public DataTable TB_S_MUTUAL_TARGET()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append( "Select * from TB_S_M_MUTUAL_TARGET");
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

}