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
/// CFB2DL0510BO 的摘要描述
/// </summary>
public class CFB2DL0510DAO : BaseDAO
{
    public CFB2DL0510DAO()
    {
        // 
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public string HR_CHG_CD { get; set; }
    public string DL_GEN_CD { get; set; }
    public string HR_CHG_DESC { get; set; }
    public string IS_BIND_PJOB { get; set; }
    public string SALARY_SETTLE_CD { get; set; }
    public string REMARK { get; set; }
    public string PJOB_CD { get; set; }

    public string RowNumber { get; set; }
    public string PROC_CD { get; set; }
    public string SDT_CD { get; set; }
    public string EDT_CD { get; set; }
    public string LOGI_CD { get; set; }
    public string DL_GENDT_CD { get; set; }
    public string IS_D01_SAME { get; set; }
    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }



    //查詢主檔
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression
        , string hr_chg_cd, string is_bind_pjob )
    {
        try
        {
            if (sortExpression.Contains("HR_CHG_CD") )
            {
                sortExpression = sortExpression.Replace("HR_CHG_CD", "H.HR_CHG_CD"); 
            }

            
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * From");
            sb.Append(" (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber ");
            sb.Append(@", H.HR_CHG_CD,H.DL_GEN_CD, H.HR_CHG_CD+'-'+H.HR_CHG_DESC HR_CHG_DESC
                        ,H.IS_BIND_PJOB,H.SALARY_SETTLE_CD,H.REMARK
                        ,DL.DL_GEN_DESC,DL.IS_D01_SAME
                        ,DL.PROC_CD,DL.SDT_CD,DL.EDT_CD,DL.LOGI_CD,DL.DL_GENDT_CD
                        ,DL.PROC_CD_DESC,DL.SDT_CD_DESC,DL.EDT_CD_DESC,DL.LOGI_CD_DESC,DL.DL_GENDT_CD_DESC
                        ,DL.IS_D01_SAME_DESC
                        ,H.SALARY_SETTLE_CD +'-'+ isnull(SC.SUB_DESC,'') as SALARY_SETTLE_CD_DESC 
                        from TB_D_M_D0_EMP_CHG_PJOB_H H
                        left join 
                        (
	                        select J.DL_GEN_CD,J.DL_GEN_DESC,J.IS_D01_SAME
	                        ,J.PROC_CD,J.SDT_CD,EDT_CD ,J.LOGI_CD,J.DL_GENDT_CD
	                        ,PROC_CD +'-'+ isnull(C.SUB_DESC,'') as PROC_CD_DESC 
	                        ,SDT_CD +'-'+ isnull(D.SUB_DESC,'') as SDT_CD_DESC 
	                        ,EDT_CD +'-'+ isnull(E.SUB_DESC,'') as EDT_CD_DESC
	                        ,LOGI_CD +'-'+ isnull(F.SUB_DESC,'') as LOGI_CD_DESC 
	                        ,DL_GENDT_CD +'-'+ isnull(G.SUB_DESC,'') as DL_GENDT_CD_DESC 
                            ,J.IS_D01_SAME +'-'+ isnull(H.SUB_DESC,'') as IS_D01_SAME_DESC 
	                        FROM  TB_D_M_D0_DL_GEN_CD as J  
	                        LEFT JOIN TB_9_M_COMM_D as C ON j.PROC_CD =C.SUB_CD and C.main_cd = 'PROC_CD' and C.IS_VALID='Y'  
	                        LEFT JOIN TB_9_M_COMM_D as D ON j.SDT_CD = D.SUB_CD and D.main_cd = 'SDT_CD'  and D.IS_VALID='Y'  
	                        LEFT JOIN TB_9_M_COMM_D as E ON j.EDT_CD = E.SUB_CD and E.main_cd = 'EDT_CD'  and E.IS_VALID='Y'  
	                        LEFT JOIN TB_9_M_COMM_D as F ON j.LOGI_CD = F.SUB_CD and F.main_cd = 'LOGI_CD'  and F.IS_VALID='Y'  
	                        LEFT JOIN TB_9_M_COMM_D as G ON j.DL_GENDT_CD = G.SUB_CD and G.main_cd = 'DL_GENDT_CD'  and G.IS_VALID='Y' 
                            LEFT JOIN TB_9_M_COMM_D as H ON j.IS_D01_SAME = H.SUB_CD and H.main_cd = 'IS_D01_SAME'  and H.IS_VALID='Y' 
                        ) DL on  H.DL_GEN_CD= DL.DL_GEN_CD
                        left join TB_9_M_COMM_D as SC ON H.SALARY_SETTLE_CD = SC.SUB_CD and SC.main_cd = 'SALARY_SETTLE_CD'  and SC.IS_VALID='Y'
                        where 1=1 ");
            if (hr_chg_cd != "")
            {
                 sb.Append(" and H.HR_CHG_CD LIKE @HR_CHG_CD  ");
                 ht.Add("@HR_CHG_CD", string.Format("%{0}%", hr_chg_cd));
            }
            if (is_bind_pjob != "-1")
            {
                sb.Append(" and H.IS_BIND_PJOB = @IS_BIND_PJOB  ");
                ht.Add("@IS_BIND_PJOB", is_bind_pjob);
            }
            /*
            if (pjob_cd != "")
            {
                sb.Append(" and D.PJOB_CD = @PJOB_CD  ");
                ht.Add("@PJOB_CD", pjob_cd);
            }
            */
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
    public int getCount(int startRowIndex, int maximumRows
        , string hr_chg_cd, string is_bind_pjob )
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) total_record ");
            sb.Append(@" from TB_D_M_D0_EMP_CHG_PJOB_H H");
            sb.Append(" where 1=1");

            if (hr_chg_cd != "")
            {
                sb.Append(" and H.HR_CHG_CD LIKE @HR_CHG_CD  ");
                ht.Add("@HR_CHG_CD", string.Format("%{0}%", hr_chg_cd));
            }
            if (is_bind_pjob != "-1")
            {
                sb.Append(" and H.IS_BIND_PJOB = @IS_BIND_PJOB  ");
                ht.Add("@IS_BIND_PJOB", is_bind_pjob);
            }
            /*
            if (pjob_cd != "")
            {
                sb.Append(" and D.PJOB_CD = @PJOB_CD  ");
                ht.Add("@PJOB_CD", pjob_cd);
            }
             * */
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


    //查詢明細檔
    public DataTable getDtlData(int startRowIndex, int maximumRows
                    , string hr_chg_cd, string dl_gen_Cd, string sortExpression)
    {
        try
        {
            if (sortExpression.Contains("PJOB_CD"))
            {
                sortExpression = sortExpression.Replace("PJOB_CD", "A.PJOB_CD");
            }
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * From");
            sb.Append(" (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber ");
            sb.Append(@",A.PJOB_CD,isnull(B.PJOB_DESC,'') as PJOB_DESC
                        from TB_D_M_D0_EMP_CHG_PJOB_D  A
                        left join VW_TB_H_M_PJOB B on B.PJOB_CD = A.PJOB_CD
                        where 1=1  ");
            if (hr_chg_cd != "")
            {
                sb.Append(" and A.HR_CHG_CD = @HR_CHG_CD  ");
                ht.Add("@HR_CHG_CD", hr_chg_cd);
            }
            if (dl_gen_Cd != "")
            {
                sb.Append(" and A.DL_GEN_CD = @DL_GEN_CD  ");
                ht.Add("@DL_GEN_CD", dl_gen_Cd);
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
    public int getDtlCount(int startRowIndex, int maximumRows
        , string hr_chg_cd, string dl_gen_Cd)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) total_record ");
            sb.Append(@" from TB_D_M_D0_EMP_CHG_PJOB_D  A ");
            sb.Append(" where 1=1");

            if (hr_chg_cd != "")
            {
                sb.Append(" and A.HR_CHG_CD = @HR_CHG_CD  ");
                ht.Add("@HR_CHG_CD", hr_chg_cd);
            }
            if (dl_gen_Cd != "")
            {
                sb.Append(" and A.DL_GEN_CD = @DL_GEN_CD  ");
                ht.Add("@DL_GEN_CD", dl_gen_Cd);
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


    //下拉選單
    public DataTable getGEN_CD()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" select DL_GEN_CD,DL_GEN_DESC
                         from TB_D_M_D0_DL_GEN_CD
                      ");
            return dbConn.Query(sb);
        }
        catch
        {
            throw;
        }
    }

    //下拉選單
    public DataTable getGEN_DATA()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" select J.DL_GEN_CD,J.DL_GEN_DESC,J.IS_D01_SAME
                        ,J.PROC_CD,J.SDT_CD,EDT_CD ,J.LOGI_CD,J.DL_GENDT_CD
                        ,PROC_CD +'-'+ isnull(C.SUB_DESC,'') as PROC_CD_DESC 
                        ,SDT_CD +'-'+ isnull(D.SUB_DESC,'') as SDT_CD_DESC 
                        ,EDT_CD +'-'+ isnull(E.SUB_DESC,'') as EDT_CD_DESC
                        ,LOGI_CD +'-'+ isnull(F.SUB_DESC,'') as LOGI_CD_DESC 
                        ,DL_GENDT_CD +'-'+ isnull(G.SUB_DESC,'') as DL_GENDT_CD_DESC 
                        ,J.IS_D01_SAME +'-'+ isnull(H.SUB_DESC,'') as IS_D01_SAME_DESC 
                        FROM  TB_D_M_D0_DL_GEN_CD as J  
                        LEFT JOIN TB_9_M_COMM_D as C ON j.PROC_CD =C.SUB_CD and C.main_cd = 'PROC_CD' and C.IS_VALID='Y'  
                        LEFT JOIN TB_9_M_COMM_D as D ON j.SDT_CD = D.SUB_CD and D.main_cd = 'SDT_CD'  and D.IS_VALID='Y'  
                        LEFT JOIN TB_9_M_COMM_D as E ON j.EDT_CD = E.SUB_CD and E.main_cd = 'EDT_CD'  and E.IS_VALID='Y'  
                        LEFT JOIN TB_9_M_COMM_D as F ON j.LOGI_CD = F.SUB_CD and F.main_cd = 'LOGI_CD'  and F.IS_VALID='Y'  
                        LEFT JOIN TB_9_M_COMM_D as G ON j.DL_GENDT_CD = G.SUB_CD and G.main_cd = 'DL_GENDT_CD'  and G.IS_VALID='Y' 
                        LEFT JOIN TB_9_M_COMM_D as H ON j.IS_D01_SAME = H.SUB_CD and H.main_cd = 'IS_D01_SAME'  and H.IS_VALID='Y' 
                        where DL_GEN_CD=@DL_GEN_CD
                      ");

            ht.Add("@DL_GEN_CD", DL_GEN_CD);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    //取得資料
    public DataTable getData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" select H.HR_CHG_CD,H.DL_GEN_CD,H.HR_CHG_DESC as HR_CHG_DESC, H.HR_CHG_CD+'-'+H.HR_CHG_DESC HR_CHG_CD_DESC
                        ,H.IS_BIND_PJOB,H.SALARY_SETTLE_CD,H.REMARK
                        ,DL.DL_GEN_DESC,DL.IS_D01_SAME
                        ,DL.PROC_CD,DL.SDT_CD,DL.EDT_CD,DL.LOGI_CD,DL.DL_GENDT_CD
                        ,DL.PROC_CD_DESC,DL.SDT_CD_DESC,DL.EDT_CD_DESC,DL.LOGI_CD_DESC,DL.DL_GENDT_CD_DESC
                        ,DL.IS_D01_SAME_DESC
                        ,H.SALARY_SETTLE_CD +'-'+ isnull(SC.SUB_DESC,'') as SALARY_SETTLE_CD_DESC 
                        from TB_D_M_D0_EMP_CHG_PJOB_H H
                        left join 
                        (
	                        select J.DL_GEN_CD,J.DL_GEN_DESC,J.IS_D01_SAME
	                        ,J.PROC_CD,J.SDT_CD,EDT_CD ,J.LOGI_CD,J.DL_GENDT_CD
	                        ,PROC_CD +'-'+ isnull(C.SUB_DESC,'') as PROC_CD_DESC 
	                        ,SDT_CD +'-'+ isnull(D.SUB_DESC,'') as SDT_CD_DESC 
	                        ,EDT_CD +'-'+ isnull(E.SUB_DESC,'') as EDT_CD_DESC
	                        ,LOGI_CD +'-'+ isnull(F.SUB_DESC,'') as LOGI_CD_DESC 
	                        ,DL_GENDT_CD +'-'+ isnull(G.SUB_DESC,'') as DL_GENDT_CD_DESC 
                            ,J.IS_D01_SAME +'-'+ isnull(H.SUB_DESC,'') as IS_D01_SAME_DESC 
	                        FROM  TB_D_M_D0_DL_GEN_CD as J  
	                        LEFT JOIN TB_9_M_COMM_D as C ON j.PROC_CD =C.SUB_CD and C.main_cd = 'PROC_CD' and C.IS_VALID='Y'  
	                        LEFT JOIN TB_9_M_COMM_D as D ON j.SDT_CD = D.SUB_CD and D.main_cd = 'SDT_CD'  and D.IS_VALID='Y'  
	                        LEFT JOIN TB_9_M_COMM_D as E ON j.EDT_CD = E.SUB_CD and E.main_cd = 'EDT_CD'  and E.IS_VALID='Y'  
	                        LEFT JOIN TB_9_M_COMM_D as F ON j.LOGI_CD = F.SUB_CD and F.main_cd = 'LOGI_CD'  and F.IS_VALID='Y'  
	                        LEFT JOIN TB_9_M_COMM_D as G ON j.DL_GENDT_CD = G.SUB_CD and G.main_cd = 'DL_GENDT_CD'  and G.IS_VALID='Y' 
                            LEFT JOIN TB_9_M_COMM_D as H ON j.IS_D01_SAME = H.SUB_CD and H.main_cd = 'IS_D01_SAME'  and H.IS_VALID='Y' 
                        ) DL on  H.DL_GEN_CD= DL.DL_GEN_CD
                        left join TB_9_M_COMM_D as SC ON H.SALARY_SETTLE_CD = SC.SUB_CD and SC.main_cd = 'SALARY_SETTLE_CD'  and SC.IS_VALID='Y'
                        where 1=1  ");
            sb.Append(" and H.HR_CHG_CD = @HR_CHG_CD and H.DL_GEN_CD = @DL_GEN_CD ");
            ht.Add("@HR_CHG_CD", HR_CHG_CD);            
            ht.Add("@DL_GEN_CD", DL_GEN_CD);            
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
    
    //刪除主檔
    public string deleteData_H()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Delete from TB_D_M_D0_EMP_CHG_PJOB_H ");
            sb.Append(" where HR_CHG_CD=@HR_CHG_CD and DL_GEN_CD=@DL_GEN_CD  ;");
            ht.Add("@HR_CHG_CD", HR_CHG_CD);
            ht.Add("@DL_GEN_CD", DL_GEN_CD);

            dbConn.ExecuteT(sb, ht, true);

            return "0";
        }
        catch (Exception)
        {

            throw;
        }
    }

    //刪除明細檔
    public string deleteData_D()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Delete from TB_D_M_D0_EMP_CHG_PJOB_D ");
            sb.Append(" where HR_CHG_CD=@HR_CHG_CD and DL_GEN_CD=@DL_GEN_CD  ;");
            ht.Add("@HR_CHG_CD", HR_CHG_CD);
            ht.Add("@DL_GEN_CD", DL_GEN_CD);

            dbConn.ExecuteT(sb, ht, true);

            return "0";
        }
        catch (Exception)
        {

            throw;
        }
    }
    //刪除明細檔
    public string deleteDtlData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Delete from TB_D_M_D0_EMP_CHG_PJOB_D ");
            sb.Append(" where HR_CHG_CD=@HR_CHG_CD and DL_GEN_CD=@DL_GEN_CD  and PJOB_CD=@PJOB_CD ;");
            ht.Add("@HR_CHG_CD", HR_CHG_CD);
            ht.Add("@DL_GEN_CD", DL_GEN_CD);
            ht.Add("@PJOB_CD", PJOB_CD);

            dbConn.ExecuteT(sb, ht, true);

            return "0";
        }
        catch (Exception)
        {

            throw;
        }
    }
    //判斷PK值
    internal DataTable getExistData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select * from TB_D_M_D0_EMP_CHG_PJOB_H where HR_CHG_CD = @HR_CHG_CD and DL_GEN_CD = @DL_GEN_CD ;");
            ht.Add("@HR_CHG_CD", HR_CHG_CD);
            ht.Add("@DL_GEN_CD", DL_GEN_CD);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //判斷同職務,其人事異動代碼 + 特休代碼
    internal DataTable getDtlExistData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@"Select * from TB_D_M_D0_EMP_CHG_PJOB_D 
                       where  HR_CHG_CD = @HR_CHG_CD and DL_GEN_CD = @DL_GEN_CD  and PJOB_CD = @PJOB_CD ;");
            
            ht.Add("@HR_CHG_CD", HR_CHG_CD);
            ht.Add("@DL_GEN_CD", DL_GEN_CD);
            ht.Add("@PJOB_CD", PJOB_CD);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //判斷已有設定相同的人事異動代碼
    internal DataTable getSameHRCD()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@"Select * from TB_D_M_D0_EMP_CHG_PJOB_D 
                       where  HR_CHG_CD = @HR_CHG_CD   and PJOB_CD = @PJOB_CD ;");

            ht.Add("@HR_CHG_CD", HR_CHG_CD);
            ht.Add("@PJOB_CD", PJOB_CD);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //判斷人事異動代碼是否存在
    internal DataTable getCHG_CD()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append( @"Select *  from TB_H_M_HR_CHANGE_CODE
                        where HR_CHG_CD = @HR_CHG_CD ;");
            ht.Add("@HR_CHG_CD", HR_CHG_CD);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    //主檔新增
    internal void addData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" INSERT INTO TB_D_M_D0_EMP_CHG_PJOB_H 
                        (HR_CHG_CD,DL_GEN_CD
                        ,HR_CHG_DESC,IS_BIND_PJOB,SALARY_SETTLE_CD,REMARK
                        ,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID
                        )
                        select TOP 1
                         @HR_CHG_CD,@DL_GEN_CD
                        ,HR_CHG_DESC,@IS_BIND_PJOB,@SALARY_SETTLE_CD,@REMARK
                        ,@CREATED_BY,getdate(),@UPDATED_BY,getdate(),@FUNC_ID
                        from TB_H_M_HR_CHANGE_CODE
                        where HR_CHG_CD = @HR_CHG_CD
                        ");

            ht.Add("@HR_CHG_CD", HR_CHG_CD);
            ht.Add("@DL_GEN_CD", DL_GEN_CD);
            ht.Add("@IS_BIND_PJOB", IS_BIND_PJOB);
            ht.Add("@SALARY_SETTLE_CD", SALARY_SETTLE_CD);
            ht.Add("@REMARK", REMARK);
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

    //修改
    internal void updData_H()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" update  TB_D_M_D0_EMP_CHG_PJOB_H 
                        SET IS_BIND_PJOB = @IS_BIND_PJOB
                        ,SALARY_SETTLE_CD = @SALARY_SETTLE_CD
                        ,REMARK = @REMARK
                        ,UPDATED_BY = @UPDATED_BY 
                        ,UPDATED_DT = getdate()
                        ,FUNC_ID = @FUNC_ID     
                        where  HR_CHG_CD = @HR_CHG_CD and DL_GEN_CD = @DL_GEN_CD           
                        ");

            ht.Add("@HR_CHG_CD", HR_CHG_CD);
            ht.Add("@DL_GEN_CD", DL_GEN_CD);

            ht.Add("@IS_BIND_PJOB", IS_BIND_PJOB);
            ht.Add("@SALARY_SETTLE_CD", SALARY_SETTLE_CD);
            ht.Add("@REMARK", REMARK);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }


    //判斷職務代碼是否存在
    internal DataTable getPJOB_CD()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@"Select *  from VW_TB_H_M_PJOB
                        where PJOB_CD = @PJOB_CD ;");
            ht.Add("@PJOB_CD", PJOB_CD);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }


    //明細檔新增
    internal void addDtlData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" INSERT INTO TB_D_M_D0_EMP_CHG_PJOB_D 
                        (HR_CHG_CD,DL_GEN_CD,PJOB_CD
                        ,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID
                        )
                        VALUES(@HR_CHG_CD,@DL_GEN_CD,@PJOB_CD
                        ,@CREATED_BY,getdate(),@UPDATED_BY,getdate(),@FUNC_ID)
                        ");

            ht.Add("@HR_CHG_CD", HR_CHG_CD);
            ht.Add("@DL_GEN_CD", DL_GEN_CD);
            ht.Add("@PJOB_CD", PJOB_CD);
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

   
}