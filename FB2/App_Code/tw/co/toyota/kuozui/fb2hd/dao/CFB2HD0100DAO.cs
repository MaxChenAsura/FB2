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
/// CFB2HD0100BO 的摘要描述
/// </summary>
public class CFB2HD0100DAO : BaseDAO
{
    public CFB2HD0100DAO()
    {
        // 
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public string RowNumber { get; set; }
    public string QDATAKEY { get; set; }
    public string EMP_ID { get; set; }
    public string DOC_NO { get; set; }
    public string EMP_NAME { get; set; }
    public string EMP_CD { get; set; }
    public string DEPT_NO { get; set; }
    public string DEPT_NAME { get; set; }
    public string LEVEL_CD { get; set; }
    public string PJOB_DESC { get; set; }
    public string START_DT { get; set; }
    public string JUDGEMENT_TYPE { get; set; }
    public string REASON_CD { get; set; }
    public string FIRST_CNT { get; set; }
    public string SECOND_CNT { get; set; }
    public string THIRD_CNT { get; set; }
    public string IS_FIRE { get; set; }
    public string REMARK { get; set; }
    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }
    //for查詢欄位
    public string ddl_SYS_ID { get; set; }



    public DataTable getJUDGEMENT_TYPE()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select SYS_CD , MAIN_CD , SUB_CD ,SUB_DESC from TB_9_M_COMM_D");
            sb.Append(" where SYS_CD='HD' and MAIN_CD='JUDGEMENT_TYPE'  ");
            return dbConn.Query(sb);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getREASON_CD()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select SYS_CD , MAIN_CD , SUB_CD ,SUB_DESC from TB_9_M_COMM_D");
            sb.Append(" where SYS_CD='HD' and MAIN_CD='REASON_CD'  ");
            return dbConn.Query(sb);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getREASON_CD(string CODE_VAL1)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select SYS_CD , MAIN_CD , SUB_CD ,SUB_DESC from TB_9_M_COMM_D");
            sb.Append(" where SYS_CD='HD' and MAIN_CD='REASON_CD' and CODE_VAL1=@CODE_VAL1  ");
            ht.Add("@CODE_VAL1", CODE_VAL1);
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
            sb.Append(" SELECT        *");
            sb.Append(" FROM            TB_9_M_SYS_M");
            sb.Append(" WHERE SYS_ID+MODE_ID = @ID");
            ht.Add("@ID", ID);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    //internal System.Data.DataTable getSYS_ID()
    //{
    //    try
    //    {
    //        StringBuilder sb = new StringBuilder();
    //        Hashtable ht = new Hashtable();
    //        sb.Append(" select SUB_CD ,SUB_DESC from TB_9_M_COMM_D where 1=1 and sys_cd = '99' and main_cd = 'sys_id' and is_valid ='Y' order by SUB_CD");
    //        return dbConn.Query(sb);

    //    }
    //    catch (Exception)
    //    {

    //        throw;
    //    }
    //}
    //取得基本資料
    public DataTable getEMPFile()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select E.EMP_NAME, E.EMP_CD, COMM.SUB_DESC, E.DEPT_NO, D.DEPT_NAME, E.PJOB_CD, E.PJOB_DESC, E.LEVEL_CD, E.WORK_SHIFT_CD, E.WORK_SHIFT_DESC");
            sb.Append(" , CONVERT(char(10), E.JOIN_DT, 120) JOIN_DT ,E.REGISTER_ADDR");
            sb.Append(" , (select top 1 ADDRESS from TB_D_M_TRANS_ALLOWANCE_D where E.EMP_ID = TB_D_M_TRANS_ALLOWANCE_D.EMP_ID) CONTACT_ADDR");
            sb.Append(" , E.MOBILE_TEL_1, E.CONTACT_TEL,AGE ");
            sb.Append(" , E.DEPT_NAME_20, E.DEPT_NAME_30, E.DEPT_NAME_40, E.DEPT_FULL_NAME, E.DIV_DEPT_FULL_NAME ");
            sb.Append(" FROM VW_H_EMP_DATA AS E");
            sb.Append(" INNER JOIN VW_H_DEPT_DATA AS D ON E.DEPT_NO = D.DEPT_NO");
            sb.Append(" INNER JOIN TB_9_M_COMM_D AS COMM ON E.EMP_CD = COMM.SUB_CD and COMM.MAIN_CD = 'EMP_CD' AND COMM.SYS_CD = 'HB'");
            sb.Append(" WHERE EMP_ID = @EMP_ID");
            ht.Add("@EMP_ID", EMP_ID);

            return dbConn.Query(sb, ht);

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
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string txt_EMP_ID, string txt_EMP_NAME, string ddl_JUDGEMENT_TYPE, string txt_DOC_NO, string txt_START_DT_S, string txt_START_DT_E, string ddl_REASON_CD)
    {
        try
        {
            if (sortExpression == "")
            {
                sortExpression = "START_DT DESC";
            }
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * From");
            sb.Append(" (select ROW_NUMBER() OVER(ORDER BY j." + sortExpression + " ) As RowNumber,j.EMP_ID + j.DOC_NO as qdatakey ");
            sb.Append(" 				, j.EMP_ID, j.DOC_NO ");
            sb.Append("                 , j.EMP_NAME, j.EMP_CD, j.DEPT_NO, j.DEPT_NAME, j.LEVEL_CD ");
            sb.Append("                 , j.PJOB_DESC, j.START_DT, j.JUDGEMENT_TYPE, j.REASON_CD ");
            sb.Append("                 , j.FIRST_CNT, j.SECOND_CNT, j.THIRD_CNT, j.IS_FIRE, j.REMARK ");
            sb.Append("                 , c.SUB_CD + '-' + c.SUB_DESC as JUDGEMENT_TYPE_str, d.SUB_CD + '-' + d.SUB_DESC as REASON_CD_str");
            sb.Append("                 , convert(bit,case when IS_FIRE ='y' then  1 else 0 end) as IS_FIRE_b");
            sb.Append(" FROM            TB_H_M_EMP_JUDGEMENT as j ");
            sb.Append(" 				LEFT OUTER JOIN TB_9_M_COMM_D as c ON j.JUDGEMENT_TYPE = c.SUB_CD and c.main_cd = 'JUDGEMENT_TYPE'");
            sb.Append(" 				LEFT OUTER JOIN TB_9_M_COMM_D as d ON j.REASON_CD = d.SUB_CD and d.main_cd = 'REASON_CD'");
            //sb.Append("                 LEFT OUTER JOIN VW_H_EMP_DATA as e ON j.EMP_ID=e.EMP_ID");
            sb.Append(" where 1=1 ");

            if (txt_EMP_ID != "")
            {
                sb.Append(" and j.EMP_ID LIKE @EMP_ID  ");
                ht.Add("@EMP_ID", string.Format("%{0}%", txt_EMP_ID));
            }
            if (txt_EMP_NAME != "")
            {
                sb.Append(" and j.EMP_NAME LIKE @EMP_NAME  ");
                ht.Add("@EMP_NAME", string.Format("{0}%", txt_EMP_NAME));
            }
            if (ddl_JUDGEMENT_TYPE.Trim() != "" && ddl_JUDGEMENT_TYPE != "-1")
            {
                sb.Append(" and j.JUDGEMENT_TYPE = @JUDGEMENT_TYPE ");
                ht.Add("@JUDGEMENT_TYPE", ddl_JUDGEMENT_TYPE);
            }
            if (txt_DOC_NO != "")
            {
                sb.Append(" and j.DOC_NO = @DOC_NO ");
                ht.Add("@DOC_NO", txt_DOC_NO);
            }
            if (txt_START_DT_S != "")
            {
                sb.Append(" and j.START_DT >= @START_DT_S ");
                ht.Add("@START_DT_S", txt_START_DT_S);
            }
            if (txt_START_DT_E != "")
            {
                sb.Append(" and j.START_DT <= @START_DT_E ");
                ht.Add("@START_DT_E", txt_START_DT_E);
            }
            if (ddl_REASON_CD.Trim() != "" && ddl_REASON_CD != "-1")
            {
                sb.Append(" and j.REASON_CD = @REASON_CD ");
                ht.Add("@REASON_CD", ddl_REASON_CD);
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
    public int getCount(int startRowIndex, int maximumRows, string txt_EMP_ID, string txt_EMP_NAME, string ddl_JUDGEMENT_TYPE, string txt_DOC_NO, string txt_START_DT_S, string txt_START_DT_E, string ddl_REASON_CD)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) total_record ");
            sb.Append(" from TB_H_M_EMP_JUDGEMENT j ");
            sb.Append(" where 1=1");
            if (txt_EMP_ID != "")
            {
                sb.Append(" and j.EMP_ID LIKE @EMP_ID  ");
                ht.Add("@EMP_ID", string.Format("%{0}%", txt_EMP_ID));
            }
            if (txt_EMP_NAME != "")
            {
                sb.Append(" and j.EMP_NAME LIKE @EMP_NAME  ");
                ht.Add("@EMP_NAME", string.Format("{0}%", txt_EMP_NAME));
            }
            if (ddl_JUDGEMENT_TYPE.Trim() != "" && ddl_JUDGEMENT_TYPE != "-1")
            {
                sb.Append(" and j.JUDGEMENT_TYPE = @JUDGEMENT_TYPE ");
                ht.Add("@JUDGEMENT_TYPE", ddl_JUDGEMENT_TYPE);
            }
            if (txt_DOC_NO != "")
            {
                sb.Append(" and j.DOC_NO = @DOC_NO ");
                ht.Add("@DOC_NO", txt_DOC_NO);
            }
            if (txt_START_DT_S != "")
            {
                sb.Append(" and j.START_DT >= @START_DT_S ");
                ht.Add("@START_DT_S", txt_START_DT_S);
            }
            if (txt_START_DT_E != "")
            {
                sb.Append(" and j.START_DT <= @START_DT_E ");
                ht.Add("@START_DT_E", txt_START_DT_E);
            }
            if (ddl_REASON_CD.Trim() != "" && ddl_REASON_CD != "-1")
            {
                sb.Append(" and j.REASON_CD = @REASON_CD ");
                ht.Add("@REASON_CD", ddl_REASON_CD);
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


    public DataTable getData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select * ");
            sb.Append(" From TB_H_M_EMP_JUDGEMENT");
            //sb.Append(" INNER JOIN VW_H_EMP_DATA AS E ON E.EMP_ID = J.EMP_ID");
            sb.Append(" where 1=1");

            if (QDATAKEY != "")
            {
                 sb.Append(" and EMP_ID+DOC_NO = @qdatakey ");
                 ht.Add("@qdatakey", QDATAKEY);
            }

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
    public string deleteData(string deleteitem)
    {
        //刪除共用代碼主檔
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        char[] ch1 = new Char[] { '|' };
        string[] split1 = deleteitem.Split(ch1);
        string a = split1[0].ToString();
        string b = split1[1].ToString();
        //寫log
        sb.Append(" update TB_H_M_EMP_JUDGEMENT set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2HD010' ");
        sb.Append(" where EMP_ID=@EMP_ID and DOC_NO = @DOC_NO;");
        ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);

        sb.Append(" Delete from TB_H_M_EMP_JUDGEMENT ");
        sb.Append(" where EMP_ID=@EMP_ID and DOC_NO = @DOC_NO;");
        ht.Add("@EMP_ID", a);
        ht.Add("@DOC_NO", b);

        dbConn.ExecuteT(sb, ht, true);

        return "0";
    }
    internal DataTable getExistData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select * from TB_H_M_EMP_JUDGEMENT where EMP_ID + DOC_NO = @EMP_ID+@DOC_NO");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@DOC_NO", DOC_NO);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
    internal void addData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" INSERT INTO TB_H_M_EMP_JUDGEMENT");
            sb.Append("                          (EMP_ID, DOC_NO, EMP_NAME, EMP_CD, DEPT_NO, DEPT_NAME, LEVEL_CD, PJOB_DESC, START_DT, JUDGEMENT_TYPE, REASON_CD, FIRST_CNT ");
            sb.Append("                          , SECOND_CNT, THIRD_CNT, IS_FIRE, REMARK, CREATED_BY, CREATED_DT, UPDATED_BY, UPDATED_DT, FUNC_ID)");
            sb.Append(" VALUES        (@EMP_ID, @DOC_NO, @EMP_NAME, @EMP_CD, @DEPT_NO, @DEPT_NAME, @LEVEL_CD, @PJOB_DESC, @START_DT, @JUDGEMENT_TYPE, @REASON_CD, @FIRST_CNT ");
            sb.Append("                          , @SECOND_CNT, @THIRD_CNT, @IS_FIRE, @REMARK, @CREATED_BY, GETDATE(), @UPDATED_BY, GETDATE(), @FUNC_ID)");

            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@EMP_NAME", EMP_NAME);
            ht.Add("@EMP_CD", EMP_CD);
            ht.Add("@DEPT_NO", DEPT_NO);
            ht.Add("@DEPT_NAME", DEPT_NAME);
            ht.Add("@LEVEL_CD", LEVEL_CD);
            ht.Add("@PJOB_DESC", PJOB_DESC);
            ht.Add("@DOC_NO", DOC_NO);
            ht.Add("@START_DT", START_DT);
            ht.Add("@JUDGEMENT_TYPE", JUDGEMENT_TYPE);
            ht.Add("@REASON_CD", REASON_CD);
            ht.Add("@FIRST_CNT", FIRST_CNT);
            ht.Add("@SECOND_CNT", SECOND_CNT);
            ht.Add("@THIRD_CNT", THIRD_CNT);
            ht.Add("@IS_FIRE", IS_FIRE);
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
    internal void updateData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update       TB_H_M_EMP_JUDGEMENT");
            sb.Append(" set                 START_DT = @START_DT, JUDGEMENT_TYPE = @JUDGEMENT_TYPE, REASON_CD = @REASON_CD, FIRST_CNT = @FIRST_CNT, SECOND_CNT = @SECOND_CNT ");
            sb.Append("                     , THIRD_CNT = @THIRD_CNT,IS_FIRE = @IS_FIRE, REMARK = @REMARK, UPDATED_BY = @UPDATED_BY, UPDATED_DT = GETDATE(), FUNC_ID = @FUNC_ID");
            sb.Append(" where EMP_ID + DOC_NO = @qdatakey");

            ht.Add("@qdatakey", QDATAKEY);
            ht.Add("@START_DT", START_DT);
            ht.Add("@JUDGEMENT_TYPE", JUDGEMENT_TYPE);
            ht.Add("@REASON_CD", REASON_CD);
            ht.Add("@FIRST_CNT", FIRST_CNT);
            ht.Add("@SECOND_CNT", SECOND_CNT);
            ht.Add("@THIRD_CNT", THIRD_CNT);
            ht.Add("@IS_FIRE", IS_FIRE);
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
}