using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using FB2.tw.co.toyota.kuozui.dao;
using System.Text;
using System.Collections;

/// <summary>
/// WFB2SJ0240 的摘要描述
/// </summary>
public class CFB2SJ0240DAO : BaseDAO
{
    public string ASSESS_YEAR { get; set; }
    public string ASSESS_TYPE { get; set; }
    public string MA_EMP_ID { get; set; }
    public string GRP_CD { get; set; }
    public string MA_TYPE { get; set; }
    public int BASE_A { get; set; }
    public int BASE_B { get; set; }
    public int BASE_C { get; set; }
    public int BASE_D { get; set; }
    public int BASE_E { get; set; }
    public int BASE_TOT { get; set; }
    public int REAL_A { get; set; }
    public int REAL_B { get; set; }
    public int REAL_C { get; set; }
    public int REAL_D { get; set; }
    public int REAL_E { get; set; }
    public int REAL_TOTAL { get; set; }
    public string CHECK_OK { get; set; }
    public string CHECK_REMARK { get; set; }
    public int OUT_REAL_A { get; set; }
    public int OUT_REAL_B { get; set; }
    public int OUT_REAL_C { get; set; }
    public int OUT_REAL_D { get; set; }
    public int OUT_REAL_E { get; set; }
    public int OUT_REAL_TOTAL { get; set; }


    public string USER_UP_YN { get; set; }
    public string CREATED_BY { get; set; }
    public string UPDATED_BY { get; set; }
    public string FUNC_ID { get; set; }

    public CFB2SJ0240DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string assess_year, string assess_type, 
        string ma_emp_id, string grp_cd, string ma_type)
    {
        try
        {
            if (sortExpression.Contains("ASSESS_YEAR"))
                sortExpression = sortExpression.Replace("ASSESS_YEAR", "A.ASSESS_YEAR");

            if (sortExpression.Contains("ASSESS_TYPE"))
                sortExpression = sortExpression.Replace("ASSESS_TYPE", "A.ASSESS_TYPE");

            if (sortExpression.Contains("GRP_CD"))
                sortExpression = sortExpression.Replace("GRP_CD", "A.GRP_CD");
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * From");
            sb.Append(" (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(@" A.ASSESS_YEAR, A.ASSESS_TYPE ,  A.ASSESS_TYPE+'-'+D.SUB_DESC as ASSESS_TYPE_DESC, A.MA_TYPE,
	                     A.MA_EMP_ID ,B.EMP_NAME MA_EMP_NAME,A.GRP_CD , C.GRP_NAME, C.WS_CD,  C.WS_CD+'-'+D.SUB_DESC as WS_CD_DESC,
                         A.BASE_A, A.BASE_B, A.BASE_C, A.BASE_D, A.BASE_E, A.BASE_TOT 
                         from TB_S_M_ASSESS_MA_PEO A with (nolock)
						 left join TB_H_M_EMP B with (nolock) on  B.EMP_ID=A.MA_EMP_ID 
						 left join TB_S_M_ASSESS_GROUP_H C with (nolock) on C.ASSESS_YEAR = A.ASSESS_YEAR and C.ASSESS_TYPE= A.ASSESS_TYPE and C.GRP_CD=A.GRP_CD 
                         left join TB_9_M_COMM_D D  with (nolock)  on D.SYS_CD='HB' and D.MAIN_CD='WS_CD' and D.SUB_CD= B.WS_CD and D.IS_VALID='Y' 
                            ");
            sb.Append(" where 1=1 ");

            sb.Append(" and A.MA_TYPE = 'B' ");
           

            if (assess_year != "")
            {
                sb.Append(" and A.ASSESS_YEAR = @ASSESS_YEAR ");
                ht.Add("@ASSESS_YEAR", assess_year);
            }

            if (assess_type != "-1")
            {
                sb.Append(" and A.ASSESS_TYPE = @ASSESS_TYPE ");
                ht.Add("@ASSESS_TYPE", assess_type);
            }

            if (ma_emp_id != "")
            {
                sb.Append(" and A.MA_EMP_ID = @MA_EMP_ID ");
                ht.Add("@MA_EMP_ID", ma_emp_id);
            
            }
            if (grp_cd != "")
            {
                sb.Append(" and A.GRP_CD like @GRP_CD ");
                ht.Add("@GRP_CD", "%"+grp_cd + "%");
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

    public int getCount(int startRowIndex, int maximumRows, string assess_year, string assess_type, 
        string ma_emp_id, string grp_cd, string ma_type)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select COUNT(*) total_record from TB_S_M_ASSESS_MA_PEO A ");
            sb.Append(" where 1=1 ");
            sb.Append(" and A.MA_TYPE = 'B' ");

            if (assess_year != "")
            {
                sb.Append(" and A.ASSESS_YEAR = @ASSESS_YEAR ");
                ht.Add("@ASSESS_YEAR", assess_year);
            }

            if (assess_type != "-1")
            {
                sb.Append(" and A.ASSESS_TYPE = @ASSESS_TYPE ");
                ht.Add("@ASSESS_TYPE", assess_type);
            }

            if (ma_emp_id != "")
            {
                sb.Append(" and A.MA_EMP_ID = @MA_EMP_ID ");
                ht.Add("@MA_EMP_ID", ma_emp_id);
            }

            if (grp_cd != "")
            {
                sb.Append(" and A.GRP_CD like @GRP_CD ");
                ht.Add("@GRP_CD", "%"+grp_cd + "%");
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

    
    //取得修改資料
    public DataTable getUpdData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" Select 
                         A.ASSESS_YEAR, A.ASSESS_TYPE ,  A.ASSESS_TYPE+'-'+D.SUB_DESC as ASSESS_TYPE_DESC, 
	                     A.MA_EMP_ID ,B.EMP_NAME,A.GRP_CD , C.GRP_NAME, C.WS_CD,  C.WS_CD+'-'+D.SUB_DESC as WS_CD_DESC,
                         A.BASE_A, A.BASE_B, A.BASE_C, A.BASE_D, A.BASE_E, A.BASE_TOT,
                         A.REAL_A, A.REAL_B, A.REAL_C, A.REAL_D, A.REAL_E, A.REAL_TOTAL 
                         from TB_S_M_ASSESS_MA_PEO A with (nolock)
						 left join TB_H_M_EMP B with (nolock) on  B.EMP_ID=A.MA_EMP_ID 
						 left join TB_S_M_ASSESS_GROUP_H C with (nolock) on C.ASSESS_YEAR = A.ASSESS_YEAR and C.ASSESS_TYPE= A.ASSESS_TYPE and C.GRP_CD=A.GRP_CD 
                         left join TB_9_M_COMM_D D  with (nolock)  on D.SYS_CD='HB' and D.MAIN_CD='WS_CD' and D.SUB_CD= B.WS_CD and D.IS_VALID='Y' ");
            
           
            sb.Append(@" 
                where 1=1
                and A.ASSESS_YEAR = @ASSESS_YEAR  and  A.ASSESS_TYPE = @ASSESS_TYPE  and A.MA_EMP_ID=@MA_EMP_ID and A.GRP_CD=@GRP_CD  and A.MA_TYPE=@MA_TYPE
            ");

            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@MA_TYPE", MA_TYPE);
            ht.Add("@MA_EMP_ID", MA_EMP_ID);
            ht.Add("@GRP_CD", GRP_CD);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }


    //更新 TB_S_M_ASSESS_MA_PEO
    public void updateData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_S_M_ASSESS_MA_PEO ");
            sb.Append(" set BASE_A=@BASE_A, BASE_B=@BASE_B,BASE_C=@BASE_C,BASE_D=@BASE_D,BASE_E=@BASE_E,");
            sb.Append(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE()");
            sb.Append(" where  ASSESS_YEAR = @ASSESS_YEAR  and  ASSESS_TYPE = @ASSESS_TYPE  and MA_EMP_ID=@MA_EMP_ID and GRP_CD=@GRP_CD and MA_TYPE='B'  ");

            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@MA_EMP_ID", MA_EMP_ID);
            ht.Add("@GRP_CD", GRP_CD);
            ht.Add("@BASE_A", BASE_A);
            ht.Add("@BASE_B", BASE_B);
            ht.Add("@BASE_C", BASE_C);
            ht.Add("@BASE_D", BASE_D);
            ht.Add("@BASE_E", BASE_E);
            ht.Add("@UPDATED_BY", UPDATED_BY);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    public DataTable getPEOMAEmpId()
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select distinct MA_EMP_ID from TB_S_M_ASSESS_MA_PEO where  ");
            sb.Append("        ASSESS_YEAR = @ASSESS_YEAR and ASSESS_TYPE = @ASSESS_TYPE ");
            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
           



            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }

    }
    //呼叫協理二階理事重配置
    internal void execSP_S_ASSESS_GEN_DEPT20_MA()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_S_ASSESS_GEN_DEPT20_MA");
            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@MA_TYPE", MA_TYPE);
            ht.Add("@USERID", UPDATED_BY);//CREATED_BY
            ht.Add("@FUNCID", "FB2SJ024");
            dbConn.ExecuteSPT(sb, ht, true);

        }
        catch (Exception)
        {

            throw;
        }

    }

    //呼叫SP,重算PEO實際值
    internal void execSP_S_ASSESS_UPD_RO_MA_PEO()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_S_ASSESS_UPD_RO_MA_PEO");
            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@MA_EMP_ID", MA_EMP_ID);
            ht.Add("@USERID", UPDATED_BY);//CREATED_BY
            ht.Add("@FUNCID", "FB2SJ024");
            dbConn.ExecuteSPT(sb, ht, true);

        }
        catch (Exception)
        {

            throw;
        }

    }
}