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
/// WFB2SJ0210 的摘要描述
/// </summary>
public class CFB2SJ0210DAO : BaseDAO
{
    

    public string ASSESS_YEAR { get; set; }
    public string ASSESS_TYPE { get; set; }
    public string EMP_ID { get; set; }
    public string DISTING_CD { get; set; }
    public string DATASOURCE { get; set; }
    public string REMARK { get; set; }
    public string ABS_SCORE { get; set; }
    public string CHG_WS_CD { get; set; }
    public string EXCEPT_E { get; set; }
    public string CREATED_BY { get; set; }
    public string UPDATED_BY { get; set; }
    public string FUNC_ID { get; set; }


    public CFB2SJ0210DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string assess_year, string assess_type, string emp_id, string datasource, string is_out )
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * From");
            sb.Append(" (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(@" A.ASSESS_YEAR , A.ASSESS_TYPE ,  A.ASSESS_TYPE+'-'+D.SUB_DESC as ASSESS_TYPE_DESC, A.EMP_ID ,E.EMP_NAME, 
                         A.DISTING_CD ,A.DISTING_CD+'-'+B.DISTING_DESC as DISTING_CD_DESC ,B.IS_OUT,
                         A.DATASOURCE ,(case when A.DATASOURCE='S' then 'S-系統生成' else 'U-檔案上傳' end) as DATASOURCE_DESC, A.REMARK , 
                         A.ABS_SCORE , A.CHG_WS_CD, A.CHG_WS_CD+'-'+C.SUB_DESC as CHG_WS_CD_DESC, A.EXCEPT_E
                         from TB_S_M_ASSESS_DISTING_EMP A with (nolock)
                         left join TB_S_M_ASSESS_DISTING B  with (nolock)  on B.DISTING_CD= A.DISTING_CD 
                         left join TB_9_M_COMM_D C  with (nolock)  on C.SYS_CD='HB' and C.MAIN_CD='WS_CD' and C.SUB_CD= A.CHG_WS_CD and C.IS_VALID='Y'
                         left join TB_9_M_COMM_D D  with (nolock)  on D.SYS_CD='SJ' and D.MAIN_CD='ASSESS_TYPE' and D.SUB_CD= A.ASSESS_TYPE and D.IS_VALID='Y' 
                         left join TB_H_M_EMP E with (nolock) on A.EMP_ID=E.EMP_ID ");
            sb.Append(" where 1=1 ");
          
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

            if (emp_id != "")
            {
                sb.Append(" and A.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
            }

            if (datasource != "-1")
            {
                sb.Append(" and A.DATASOURCE = @DATASOURCE ");
                ht.Add("@DATASOURCE", datasource);
            }


            if (is_out != "-1")
            {
                sb.Append(" and B.IS_OUT = @IS_OUT ");
                ht.Add("@IS_OUT", is_out);
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

    public int getCount(int startRowIndex, int maximumRows, string assess_year, string assess_type, string emp_id, string datasource, string is_out)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@"select COUNT(*) total_record 
                        from TB_S_M_ASSESS_DISTING_EMP A with (nolock)
                         left join TB_S_M_ASSESS_DISTING B  with (nolock)  on B.DISTING_CD= A.DISTING_CD 
                         left join TB_9_M_COMM_D C  with (nolock)  on C.SYS_CD='HB' and C.MAIN_CD='WS_CD' and C.SUB_CD= A.CHG_WS_CD and C.IS_VALID='Y'
                         left join TB_9_M_COMM_D D  with (nolock)  on D.SYS_CD='SJ' and D.MAIN_CD='ASSESS_TYPE' and D.SUB_CD= A.ASSESS_TYPE and D.IS_VALID='Y' 
                         left join TB_H_M_EMP E with (nolock) on A.EMP_ID=E.EMP_ID
                        ");
            sb.Append(" where 1=1 ");
           
            if (assess_year != "")
            {
                sb.Append(" and A.ASSESS_YEAR = @ASSESS_YEAR ");
                ht.Add("@ASSESS_YEAR", assess_year);
            }
           
            if (assess_type  != "-1")
            {
                sb.Append(" and A.ASSESS_TYPE = @ASSESS_TYPE ");
                ht.Add("@ASSESS_TYPE", assess_type);
            }

            if (emp_id != "")
            {
                sb.Append(" and A.EMP_ID = @EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
            }
            if (datasource != "-1")
            {
                sb.Append(" and A.DATASOURCE = @DATASOURCE ");
                ht.Add("@DATASOURCE", datasource);
            }


            if (is_out != "-1")
            {
                sb.Append(" and B.IS_OUT = @IS_OUT ");
                ht.Add("@IS_OUT", is_out);
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

    
    
}