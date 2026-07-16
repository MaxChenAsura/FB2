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
/// CFB2SL3110DAO 的摘要描述
/// </summary>
public class CFB2SL3110DAO : BaseDAO
{
    public CFB2SL3110DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public string RowNumber { get; set; }
    public string vSendto { get; set; }
    public string EFFECT_YEAR { get; set; }
    public string STD_YM { get; set; }
    public string END_YM { get; set; }
    public string EMP_ID { get; set; }
    public string EMP_ID_TA { get; set; }
    public string SALARY_EMAIL { get; set; }
    public string MAIL_DT { get; set; }
    public string TITLE { get; set; }
    public string MAIL_DESC { get; set; }        
    public string CREATED_BY { get; set; }    
    public string UPDATED_BY { get; set; }    
    public string FUNC_ID { get; set; }

    public DataTable getNoData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select count(*) cnt from TB_I_R_GROUP_MONTH ");
            sb.Append(" where LEFT(SALARY_YM,4) = @EFFECT_YEAR ");

            if (EMP_ID != "")
            {
                sb.Append(" and EMP_ID= @EMP_ID");
            }

            ht.Add("@EFFECT_YEAR", EFFECT_YEAR);
            ht.Add("@EMP_ID", EMP_ID);

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getTemp2()
    {
        try
        {
            //取寄信人的MAIL
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select a.SALARY_EMAIL from TB_H_M_EMP a");
            sb.Append(" WHERE a.EMP_ID=@EMP_ID");
            ht.Add("@EMP_ID", SessionHandle.Current.emp_id);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getTempCHK1()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select distinct t.EMP_ID,t2.EMP_NAME ");
            sb.Append(@" from (
                                                    select EMP_ID 
                                                    from (																																																	
                                                    SELECT M.COMPANY_CD AS WK_COMPANY_CD, M.SUB_DESC AS WK_IDENTITY_KIND, M.EMP_ID , M.LICENSE_ID AS WK_LICENSE_ID, M.EMP_NAME AS WK_INS_NAME,'團保' AS WK_INS_TYPE,																																																	
                                                                                   SUM(M.GFEES_SELF +ISNULL((CASE WHEN K.TRACE_TYPE = 'A' THEN K.TRACE_AMT ELSE K.TRACE_AMT * -1 END), 0)  ) AS WK_INS_TOTAL																																																	
                                                                          FROM 																																																	
                                                                        (																																																	
                                                                            SELECT M.COMPANY_CD, '本人'  AS SUB_DESC, V.EMP_ID, V.LICENSE_ID,V.EMP_NAME,'團保' AS INS_TYPE,
														                    SUM(M.GFEES_SELF) AS GFEES_SELF,M.SALARY_YM 																																																	
                                                                            FROM TB_I_R_GROUP_MONTH M																																																	
                                                                                 JOIN VW_H_EMP_DATA V ON M.EMP_ID = V.EMP_ID																																																	
                                                                            WHERE 
															                      M.IDENTITY_KIND = '1'																																																	
                                                                                  AND M.IS_YN = 'Y' 
															                      AND M.SALARY_YM BETWEEN @STD_YM AND @END_YM																																																
                                                                                  AND M.GFEES_SELF <> 0                      																																																	
                                                                                  AND M.COMPANY_CD = 'K'																  																																															
                                                                            GROUP BY M.COMPANY_CD, M.IDENTITY_KIND,V.EMP_ID,V.LICENSE_ID,V.EMP_NAME,M.SALARY_YM																																																	
                                                                        ) M																																																	
                                                                        LEFT JOIN TB_I_M_FEES_TRACEBACK K ON M.EMP_ID = K.EMP_ID  AND K.INS_TYPE = 'D' AND M.SALARY_YM = K.SALARY_YM AND M.LICENSE_ID = K.LICENSE_ID AND K.IDENTITY_KIND = '1'																																																	
                                                                        GROUP BY M.COMPANY_CD,M.SUB_DESC,M.EMP_ID,M.LICENSE_ID,M.EMP_NAME																																																	
                                                    Union all																																																	
                                                    select M.COMPANY_CD AS WK_COMPANY_CD,M.SUB_DESC AS WK_IDENTITY_KIND,M.EMP_ID,M.FAMILY_LICENSE_ID AS WK_LICENSE_ID,M.FAMILY_NAME AS WK_INS_NAME,'團保' AS WK_INS_TYPE,																																																	
                                                                                   SUM(M.GFEES_SELF +ISNULL((CASE WHEN K.TRACE_TYPE = 'A' THEN K.TRACE_AMT  ELSE K.TRACE_AMT * -1 END), 0) ) AS WK_INS_TOTAL																																																	
                                                                          from 																																																	
                                                                        (																																																	
                                                                            SELECT M.COMPANY_CD,T.SUB_DESC,M.EMP_ID,F.FAMILY_LICENSE_ID,F.FAMILY_NAME,'團保' AS INS_TYPE,
														                    SUM(M.GFEES_SELF) AS GFEES_SELF,M.SALARY_YM 																																																	
                                                                            FROM TB_I_R_GROUP_MONTH M																																																	
                                                                                 LEFT JOIN TB_H_M_EMP_FAMILY F ON M.EMP_ID = F.EMP_ID AND M.LICENSE_ID = F.FAMILY_LICENSE_ID																																																	
                                                                                 LEFT JOIN VW_H_EMP_DATA V ON M.EMP_ID = V.EMP_ID																																																	
                                                                                 LEFT JOIN TB_9_M_COMM_D T ON T.SYS_CD = 'HB' AND T.MAIN_CD = 'FAMILY_RELATION' AND T.SUB_CD = F.FAMILY_RELATION																																																	
                                                                            WHERE 
															                      M.IDENTITY_KIND = '2' 
															                      AND M.IS_YN = 'Y' 
															                      AND M.SALARY_YM BETWEEN @STD_YM AND @END_YM																																																
                                                                                  AND M.GFEES_SELF <> 0  AND M.COMPANY_CD = 'K'																																																	
                                                                            GROUP BY M.COMPANY_CD,T.SUB_DESC,M.EMP_ID,F.FAMILY_LICENSE_ID,F.FAMILY_NAME,M.SALARY_YM 																																																	
	                                                    ) M																																																
	                                                                LEFT JOIN TB_I_M_FEES_TRACEBACK K ON M.EMP_ID = K.EMP_ID AND K.INS_TYPE = 'D' AND M.SALARY_YM = K.SALARY_YM   																																																
                                                                        AND M.FAMILY_LICENSE_ID = K.LICENSE_ID AND K.IDENTITY_KIND = '2'																																																	
                                                                        GROUP BY M.COMPANY_CD,M.SUB_DESC,M.EMP_ID,M.FAMILY_LICENSE_ID,M.FAMILY_NAME																																																	
                                                    ) M1																																																	
                                                    left join TB_S_R_IMX_COMPANY cy on M1.WK_COMPANY_CD = cy.COMPANY_CD and cy.DATA_YM = @EFFECT_YEAR																																																
                                                    WHERE M1.WK_INS_TOTAL > 0 
								                    GROUP BY M1.EMP_ID ) t");
            sb.Append(" left join TB_H_M_EMP t2 on t.EMP_ID=t2.EMP_ID");
            sb.Append(" where isnull(t2.SALARY_EMAIL,'')='' and isnull(t2.JPN_CD,'')='' ");
            if (EMP_ID != "")
            {
                sb.Append(" and t.EMP_ID=@EMP_ID");
                ht.Add("@EMP_ID", EMP_ID);
            }
            ht.Add("@EFFECT_YEAR", EFFECT_YEAR);
            ht.Add("@STD_YM", STD_YM);
            ht.Add("@END_YM", END_YM);

            return dbConn.Query(sb, ht);

        }
        catch
        {
            throw;
        }
    }
    public void deleteData()
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();

        sb.Append(" delete from TB_S_M_IMX_MAIL_BAT_H where EFFECT_YEAR=@EFFECT_YEAR AND SEND_DT =@SEND_DT  ");
        sb.Append(" and QRY_EMP_ID= @EMP_ID");


        ht.Add("@EMP_ID", EMP_ID);
        ht.Add("@SEND_DT", MAIL_DT);
        ht.Add("@EFFECT_YEAR", EFFECT_YEAR);
        dbConn.ExecuteT(sb, ht, true);
        sb.Clear();
        ht.Clear();

        sb.Append("  delete from TB_S_M_IMX_MAIL_BAT_D where EFFECT_YEAR=@EFFECT_YEAR AND SEND_DT =@SEND_DT  ");
        sb.Append(" and QRY_EMP_ID = @EMP_ID");

        ht.Add("@EMP_ID", EMP_ID);
        ht.Add("@SEND_DT", MAIL_DT);
        ht.Add("@EFFECT_YEAR", EFFECT_YEAR);
        dbConn.ExecuteT(sb, ht, true);
    }
    internal void addData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("INSERT INTO TB_S_M_IMX_MAIL_BAT_H (SEND_DT,MAIL_TITLE,MAIL_DESC,EFFECT_YEAR,QRY_EMP_ID,SENDTO_MAIL,CREATED_BY,CREATED_DT,FUNC_ID)");
            sb.Append(" Values (@SEND_DT,@MAIL_TITLE,@MAIL_DESC,@EFFECT_YEAR,@QRY_EMP_ID,@SENDTO_MAIL,@CREATED_BY,GETDATE(),@FUNC_ID)");
            ht.Add("@SEND_DT", MAIL_DT);
            ht.Add("@MAIL_TITLE", TITLE);
            ht.Add("@MAIL_DESC", MAIL_DESC);
            ht.Add("@EFFECT_YEAR", EFFECT_YEAR);
            ht.Add("@QRY_EMP_ID", EMP_ID);
            ht.Add("@SENDTO_MAIL", vSendto);
            ht.Add("@CREATED_BY", CREATED_BY);
            ht.Add("@FUNC_ID", "FB2SL311");

            dbConn.ExecuteT(sb, ht, true);


        }
        catch (Exception)
        {
            throw;
        }
    }
    public void addData2()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("INSERT INTO TB_S_M_IMX_MAIL_BAT_D (SEND_DT,EMP_ID,EFFECT_YEAR,EMAIL,MAIL_YN,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID,QRY_EMP_ID)");
            sb.Append(" select distinct @SEND_DT,t.EMP_ID,@EFFECT_YEAR ,t2.SALARY_EMAIL,'N',@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),'FB2SL311',@EMP_ID  ");
            sb.Append(@" from  (
                                                    select EMP_ID 
                                                    from (																																																	
                                                    SELECT M.COMPANY_CD AS WK_COMPANY_CD, M.SUB_DESC AS WK_IDENTITY_KIND, M.EMP_ID , M.LICENSE_ID AS WK_LICENSE_ID, M.EMP_NAME AS WK_INS_NAME,'團保' AS WK_INS_TYPE,																																																	
                                                                                   SUM(M.GFEES_SELF +ISNULL((CASE WHEN K.TRACE_TYPE = 'A' THEN K.TRACE_AMT ELSE K.TRACE_AMT * -1 END), 0)  ) AS WK_INS_TOTAL																																																	
                                                                          FROM 																																																	
                                                                        (																																																	
                                                                            SELECT M.COMPANY_CD, '本人'  AS SUB_DESC, V.EMP_ID, V.LICENSE_ID,V.EMP_NAME,'團保' AS INS_TYPE,
														                    SUM(M.GFEES_SELF) AS GFEES_SELF,M.SALARY_YM 																																																	
                                                                            FROM TB_I_R_GROUP_MONTH M																																																	
                                                                                 JOIN VW_H_EMP_DATA V ON M.EMP_ID = V.EMP_ID																																																	
                                                                            WHERE 
															                      M.IDENTITY_KIND = '1'																																																	
                                                                                  AND M.IS_YN = 'Y' 
															                      AND M.SALARY_YM BETWEEN @STD_YM AND @END_YM																																																
                                                                                  AND M.GFEES_SELF <> 0                      																																																	
                                                                                  AND M.COMPANY_CD = 'K'																  																																															
                                                                            GROUP BY M.COMPANY_CD, M.IDENTITY_KIND,V.EMP_ID,V.LICENSE_ID,V.EMP_NAME,M.SALARY_YM																																																	
                                                                        ) M																																																	
                                                                        LEFT JOIN TB_I_M_FEES_TRACEBACK K ON M.EMP_ID = K.EMP_ID  AND K.INS_TYPE = 'D' AND M.SALARY_YM = K.SALARY_YM AND M.LICENSE_ID = K.LICENSE_ID AND K.IDENTITY_KIND = '1'																																																	
                                                                        GROUP BY M.COMPANY_CD,M.SUB_DESC,M.EMP_ID,M.LICENSE_ID,M.EMP_NAME																																																	
                                                    Union all																																																	
                                                    select M.COMPANY_CD AS WK_COMPANY_CD,M.SUB_DESC AS WK_IDENTITY_KIND,M.EMP_ID,M.FAMILY_LICENSE_ID AS WK_LICENSE_ID,M.FAMILY_NAME AS WK_INS_NAME,'團保' AS WK_INS_TYPE,																																																	
                                                                                   SUM(M.GFEES_SELF +ISNULL((CASE WHEN K.TRACE_TYPE = 'A' THEN K.TRACE_AMT  ELSE K.TRACE_AMT * -1 END), 0) ) AS WK_INS_TOTAL																																																	
                                                                          from 																																																	
                                                                        (																																																	
                                                                            SELECT M.COMPANY_CD,T.SUB_DESC,M.EMP_ID,F.FAMILY_LICENSE_ID,F.FAMILY_NAME,'團保' AS INS_TYPE,
														                    SUM(M.GFEES_SELF) AS GFEES_SELF,M.SALARY_YM 																																																	
                                                                            FROM TB_I_R_GROUP_MONTH M																																																	
                                                                                 LEFT JOIN TB_H_M_EMP_FAMILY F ON M.EMP_ID = F.EMP_ID AND M.LICENSE_ID = F.FAMILY_LICENSE_ID																																																	
                                                                                 LEFT JOIN VW_H_EMP_DATA V ON M.EMP_ID = V.EMP_ID																																																	
                                                                                 LEFT JOIN TB_9_M_COMM_D T ON T.SYS_CD = 'HB' AND T.MAIN_CD = 'FAMILY_RELATION' AND T.SUB_CD = F.FAMILY_RELATION																																																	
                                                                            WHERE 
															                      M.IDENTITY_KIND = '2' 
															                      AND M.IS_YN = 'Y' 
															                      AND M.SALARY_YM BETWEEN @STD_YM AND @END_YM																																																
                                                                                  AND M.GFEES_SELF <> 0  AND M.COMPANY_CD = 'K'																																																	
                                                                            GROUP BY M.COMPANY_CD,T.SUB_DESC,M.EMP_ID,F.FAMILY_LICENSE_ID,F.FAMILY_NAME,M.SALARY_YM 																																																	
	                                                    ) M																																																
	                                                                LEFT JOIN TB_I_M_FEES_TRACEBACK K ON M.EMP_ID = K.EMP_ID AND K.INS_TYPE = 'D' AND M.SALARY_YM = K.SALARY_YM   																																																
                                                                        AND M.FAMILY_LICENSE_ID = K.LICENSE_ID AND K.IDENTITY_KIND = '2'																																																	
                                                                        GROUP BY M.COMPANY_CD,M.SUB_DESC,M.EMP_ID,M.FAMILY_LICENSE_ID,M.FAMILY_NAME																																																	
                                                    ) M1																																																	
                                                    left join TB_S_R_IMX_COMPANY cy on M1.WK_COMPANY_CD = cy.COMPANY_CD and cy.DATA_YM = @EFFECT_YEAR																																																
                                                    WHERE M1.WK_INS_TOTAL > 0 
								                    GROUP BY M1.EMP_ID ) t  ");
            sb.Append(" left join TB_H_M_EMP t2 on t.EMP_ID=t2.EMP_ID ");
            sb.Append(" where isnull(t2.SALARY_EMAIL,'')<>'' and isnull(t2.JPN_CD,'')='' ");
            
            if (EMP_ID != "")
            {
                sb.Append(" and t.EMP_ID= @EMP_ID");

            }
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SEND_DT", MAIL_DT);
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@EFFECT_YEAR", EFFECT_YEAR);
            ht.Add("@STD_YM", STD_YM);
            ht.Add("@END_YM", END_YM);
            dbConn.ExecuteT(sb, ht, true);

        }
        catch
        {
            throw;
        }
    }



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
    public DataTable getSEND_DT(string EFFECT_YM)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select CONVERT(varchar,SEND_DT,111) SEND_DT");
            sb.Append(" from TB_S_M_MAIL_BAT_H a");
            sb.Append(" where a.EFFECT_YM=@EFFECT_YM");
            ht.Add("@EFFECT_YM", EFFECT_YM);
            return dbConn.Query(sb,ht);

        }
        catch
        {
            throw;
        }
    }
    
    
    public DataTable getNot_ADJ(string txt_EFFECT_YM)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select count(1) cnt from TB_S_M_SALARY_ADJ_H a");
            sb.Append(" where isnull(a.MEM_CREATE_BY,'')=''");
            sb.Append(" and a.EFFECT_YM=@EFFECT_YM");
            ht.Add("@EFFECT_YM", txt_EFFECT_YM);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    
    
    
    
        internal DataTable getExistData()
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                Hashtable ht = new Hashtable();
                sb.Append("Select * from TB_S_M_MAIL_BAT_H where SEND_DT = @SEND_DT");
                ht.Add("@SEND_DT", MAIL_DT);

                return dbConn.Query(sb, ht);
            }
            catch (Exception)
            {

                throw;
            }
        }
        internal DataTable getExistData2(string deleteitem)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                Hashtable ht = new Hashtable();
                char[] ch1 = new Char[] { '|' };
                string[] split1 = deleteitem.Split(ch1);
                string EMP_ID_TA2 = split1[0].ToString();
                string SALARY_EMAIL2 = split1[1].ToString();
                string MAIL_DT2 = split1[2].ToString();
                sb.Append("Select * from TB_S_M_MAIL_BAT_D where SEND_DT = @SEND_DT and EMP_ID = @EMP_ID");
                ht.Add("@SEND_DT", MAIL_DT2);
                ht.Add("@EMP_ID", EMP_ID_TA2);
                return dbConn.Query(sb, ht);
            }
            catch (Exception)
            {

                throw;
            }
        }
    
  
    

}