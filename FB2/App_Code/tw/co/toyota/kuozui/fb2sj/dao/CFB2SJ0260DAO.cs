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
/// WFB2IA0100 的摘要描述
/// </summary>
public class CFB2SJ0260DAO : BaseDAO
{
    public string ASSESS_TYPE { get; set; }
    public string ASSESS_YEAR { get; set; }
    public string EMP_ID { get; set; }
    public string DEPT_NO_NEW { get; set; }
    public string DEPT_NAME_NEW { get; set; }
    public string HEAD_EMP_ID_NEW { get; set; }
    public string DEPT_NO_OLD { get; set; }
    public string DEPT_NAME_OLD { get; set; }
    public string HEAD_EMP_ID_OLD { get; set; }
    public string ITEM_CD { get; set; }
    public string SURE_YN { get; set; }
    public string CREATED_BY { get; set; }
    public string UPDATED_BY { get; set; }
    public string FUNC_ID { get; set; }
    public string DEPT_NO { get; set; }

    public CFB2SJ0260DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string assess_year, string assess_type, string emp_id, string sure_yn)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * From");
            sb.Append(" (select ROW_NUMBER() OVER(ORDER BY A.EMP_ID ) As RowNumber,");
            sb.Append(@"  A.ASSESS_YEAR,A.ASSESS_TYPE, A.ASSESS_TYPE+'-'+B.SUB_DESC as ASSESS_TYPE_DESC, A.EMP_ID, D.EMP_NAME, 
                          A.DEPT_NO_NEW, A.DEPT_NAME_NEW, A.HEAD_EMP_ID_NEW, C.EMP_NAME HEAD_EMP_NAME_NEW,
                          A.DEPT_NO_OLD, A.DEPT_NAME_OLD, A.HEAD_EMP_ID_OLD, D.EMP_NAME HEAD_EMP_NAME_OLD, 
                          A.ITEM_CD ,A.SURE_YN, (CASE WHEN A.SURE_YN='Y' THEN '是' ELSE '否' END) SURE_YN_DESC
                         from TB_S_M_ASSESS_EMP_CHG A LEFT jOIN
                              TB_9_M_COMM_D B  with (nolock)  on B.SYS_CD='SJ' and B.MAIN_CD='ASSESS_TYPE' and B.SUB_CD= A.ASSESS_TYPE and B.IS_VALID='Y' left join
                              TB_H_M_EMP C on A.HEAD_EMP_ID_NEW=C.EMP_ID left join 
                              TB_H_M_EMP D on A.HEAD_EMP_ID_OLD=D.EMP_ID  ");
            sb.Append(" where 1=1 and A.ASSESS_YEAR=@ASSESS_YEAR and A.ASSESS_TYPE=@ASSESS_TYPE  ");

            ht.Add("@ASSESS_YEAR", assess_year);
            ht.Add("@ASSESS_TYPE", assess_type);
            if (emp_id != "")
            {
                sb.Append(" and A.EMP_ID =@EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
            }

            if (sure_yn != "-1")
            {
                sb.Append(" and A.SURE_YN=@SURE_YN ");
                ht.Add("@SURE_YN", sure_yn);
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

    public int getCount(int startRowIndex, int maximumRows, string assess_year, string assess_type, string emp_id, string sure_yn)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@"select COUNT(*) total_record from TB_S_M_ASSESS_EMP_CHG A ");
            sb.Append(" where 1=1 and A.ASSESS_YEAR=@ASSESS_YEAR and A.ASSESS_TYPE=@ASSESS_TYPE ");

            ht.Add("@ASSESS_YEAR", assess_year);
            ht.Add("@ASSESS_TYPE", assess_type);
            if (emp_id != "")
            {
                sb.Append(" and A.EMP_ID =@EMP_ID ");
                ht.Add("@EMP_ID", emp_id);
            }

            if (sure_yn != "-1")
            {
                sb.Append(" and A.SURE_YN=@SURE_YN ");
                ht.Add("@SURE_YN", sure_yn);
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

    //取得現有資料
    public DataTable getExistData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select EMP_ID from TB_S_M_ASSESS_EMP_CHG ");
            sb.Append(" where EMP_ID = @EMP_ID  and ASSESS_YEAR=@ASSESS_YEAR and ASSESS_TYPE=@ASSESS_TYPE ");
            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@EMP_ID", EMP_ID);

            return dbConn.Query(sb, ht);
        }
        catch (Exception ex)
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
                          A.ASSESS_YEAR, A.ASSESS_TYPE, A.ASSESS_TYPE+'-'+B.SUB_DESC as ASSESS_TYPE_DESC, A.EMP_ID, D.EMP_NAME, 
                          A.DEPT_NO_NEW, A.DEPT_NAME_NEW, A.HEAD_EMP_ID_NEW, C.EMP_NAME HEAD_EMP_NAME_NEW,
                          A.DEPT_NO_OLD, A.DEPT_NAME_OLD, A.HEAD_EMP_ID_OLD, D.EMP_NAME HEAD_EMP_NAME_OLD, 
                          A.ITEM_CD ,A.SURE_YN, (CASE WHEN A.SURE_YN='Y' THEN '是' ELSE '否' END) SURE_YN_DESC
                         from TB_S_M_ASSESS_EMP_CHG A LEFT jOIN
                              TB_9_M_COMM_D B  with (nolock)  on B.SYS_CD='SJ' and B.MAIN_CD='ASSESS_TYPE' and B.SUB_CD= A.ASSESS_TYPE and B.IS_VALID='Y' left join
                              TB_H_M_EMP C on A.HEAD_EMP_ID_NEW=C.EMP_ID left join 
                              TB_H_M_EMP D on A.HEAD_EMP_ID_OLD=D.EMP_ID
            ");
           
            sb.Append(@" 
                where 1=1
                and A.EMP_ID =@EMP_ID  and A.ASSESS_YEAR=@ASSESS_YEAR and A.ASSESS_TYPE=@ASSESS_TYPE
            ");

            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@EMP_ID", this.EMP_ID);
            //ht.Add("@DEPT_NO_NEW", this.DEPT_NO_NEW);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //新增 TB_S_M_ASSESS_EMP_CHG
    public void addEMP_CHG()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("insert into TB_S_M_ASSESS_EMP_CHG ( ");
            sb.Append(" ASSESS_YEAR, ASSESS_TYPE,EMP_ID,DEPT_NO_NEW,DEPT_NAME_NEW,HEAD_EMP_ID_NEW, ");
            sb.Append(" DEPT_NO_OLD, DEPT_NAME_OLD, HEAD_EMP_ID_OLD, ");
            sb.Append(" ITEM_CD, SURE_YN, ");
            sb.Append(" CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" values ( ");
            sb.Append("  @ASSESS_YEAR, @ASSESS_TYPE,@EMP_ID,@DEPT_NO_NEW,@DEPT_NAME_NEW,@HEAD_EMP_ID_NEW, ");
            sb.Append("  @DEPT_NO_OLD, @DEPT_NAME_OLD, @HEAD_EMP_ID_OLD, ");
            sb.Append("  @ITEM_CD, @SURE_YN, ");
            sb.Append("  @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@DEPT_NO_NEW", DEPT_NO_NEW);
            ht.Add("@DEPT_NAME_NEW", DEPT_NAME_NEW);
            ht.Add("@HEAD_EMP_ID_NEW", HEAD_EMP_ID_NEW);
            ht.Add("@DEPT_NO_OLD", DEPT_NO_OLD);
            ht.Add("@DEPT_NAME_OLD", DEPT_NAME_OLD);
            ht.Add("@HEAD_EMP_ID_OLD", HEAD_EMP_ID_OLD);
            ht.Add("@ITEM_CD", ITEM_CD);
            ht.Add("@SURE_YN", SURE_YN);

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

    //更新 TB_S_M_ASSESS_EMP_CHG
    public void updateEMP_CHG()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_S_M_ASSESS_EMP_CHG ");
            sb.Append(" set DEPT_NO_NEW=@DEPT_NO_NEW,");
            sb.Append(" DEPT_NAME_NEW=@DEPT_NAME_NEW,HEAD_EMP_ID_NEW=@HEAD_EMP_ID_NEW,");
            sb.Append(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE()");
            sb.Append(" where EMP_ID =@EMP_ID and ASSESS_YEAR=@ASSESS_YEAR and ASSESS_TYPE=@ASSESS_TYPE  ");

            ht.Add("@DEPT_NO_NEW", DEPT_NO_NEW);
            ht.Add("@DEPT_NAME_NEW", DEPT_NAME_NEW);
            ht.Add("@HEAD_EMP_ID_NEW", HEAD_EMP_ID_NEW);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@UPDATED_BY", UPDATED_BY);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //刪除 TB_S_M_ASSESS_EMP_CHG
    public void deleteEMP_CHG(string assess_year, string assess_type, string emp_id, string dept_no)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //寫log
            sb.AppendLine(" update TB_S_M_ASSESS_EMP_CHG set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2SJ0260' ");
            sb.AppendLine(" where EMP_ID = @EMP_ID and ASSESS_YEAR=@ASSESS_YEAR and ASSESS_TYPE=@ASSESS_TYPE and DEPT_NO_OLD=@DEPT_NO_OLD ; ");
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);

            sb.Append(" delete from TB_S_M_ASSESS_EMP_CHG ");
            sb.Append(" where EMP_ID = @EMP_ID and ASSESS_YEAR=@ASSESS_YEAR and ASSESS_TYPE=@ASSESS_TYPE  ");
            ht.Add("@ASSESS_TYPE", assess_type);
            ht.Add("@ASSESS_YEAR", assess_year);
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@DEPT_NO_OLD", dept_no);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //取得員工DEPT資料
    public DataTable getEmpDeptData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" Select                         
                                A.EMP_NAME,B.DEPT_NO,B.DEPT_NAME,B.DEPT_FULL_NAME,B.HEAD_EMP_ID,B.HEAD_EMP_NAME
                        FROM TB_S_M_ASSESS_TARGET A join
                             TB_H_R_DEPT_DATA_AD B on A.DEPT_NO=B.DEPT_NO  ");


            sb.Append(@" 
                where 1=1
                and  A.ASSESS_YEAR=@ASSESS_YEAR and A.ASSESS_TYPE=@ASSESS_TYPE 
                and  A.EMP_ID=@EMP_ID
                
            ");

            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@EMP_ID", EMP_ID);

          
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //取得員工DEPT資料
    public DataTable getDeptData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" Select                         
                                B.DEPT_NO,B.DEPT_NAME,B.DEPT_FULL_NAME,B.HEAD_EMP_ID,B.HEAD_EMP_NAME
                        FROM TB_H_R_DEPT_DATA_AD B ");


            sb.Append(@" 
                where 1=1
                and  B.DEPT_NO=@DEPT_NO
            ");

            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@DEPT_NO", DEPT_NO);


            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //呼叫員工部門轉移- 確認作業
    internal void execSP_S_ASSESS_EMP_CHG_CONFIRM()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_S_ASSESS_EMP_CHG_CONFIRM");
            ht.Add("@ASSESS_YEAR", ASSESS_YEAR);
            ht.Add("@ASSESS_TYPE", ASSESS_TYPE);
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@DEPT_NO_OLD", DEPT_NO_OLD);
            ht.Add("@DEPT_NO_NEW", DEPT_NO_NEW);
            ht.Add("@USERID", CREATED_BY);//CREATED_BY
            ht.Add("@FUNCID", "FB2SJ026");
            dbConn.ExecuteSPT(sb, ht, true);

        }
        catch (Exception)
        {

            throw;
        }

    }
}