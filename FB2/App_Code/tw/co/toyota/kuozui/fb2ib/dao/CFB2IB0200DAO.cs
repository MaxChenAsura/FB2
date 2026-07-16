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
/// CFB2HA0300BO 的摘要描述
/// </summary>
public class CFB2IB0200DAO : BaseDAO
{
    public CFB2IB0200DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public string RowNumber { get; set; }
    public string QDATAKEY { get; set; }
    public string EMP_ID { get; set; }
    public string LICENSE_ID { get; set; }
    public string EMP_NAME { get; set; }
    public string NONPAY_CAT { get; set; }
    public string REMARK { get; set; }
    public string START_YM { get; set; }
    public string END_YM { get; set; }
    public string BIRTH_DT { get; set; }
    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }
    //for查詢欄位
    public string ddl_SYS_ID { get; set; }



    public DataTable getSYS_ID()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select SYS_CD , MAIN_CD , SUB_CD ,SUB_DESC from TB_9_M_COMM_D");
            sb.Append(" where SYS_CD='IB' and MAIN_CD='NONPAY_CAT'  ");
            return dbConn.Query(sb);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getSYS_ID(string SUB_CD)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select SUB_CD ,SUB_DESC from TB_9_M_COMM_D where 1=1 and sys_cd = '99' and main_cd = 'sys_id' and is_valid ='Y' and SUB_CD = @SUB_CD order by SUB_CD");
            ht.Add("@SUB_CD", SUB_CD);
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

    public DataTable get_SYS_ID_Data()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select SYS_CD , MAIN_CD , SUB_CD ,SUB_DESC from TB_9_M_COMM_D");
            sb.Append(" where SYS_CD='IB' and MAIN_CD=NONPAY_CAT  ");

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string txt_EMP_ID, string txt_START_YM, string txt_LICENSE_ID)
    {
        try
        {
            if (sortExpression == "")
            {
                sortExpression = "t.EMP_ID";
            }
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * From");
            sb.Append(" (select ROW_NUMBER() OVER(ORDER BY t.EMP_ID ) As RowNumber,");
            sb.Append("  t.*,CONVERT(CHAR(10),t.BIRTH_DT, 111) as BIRTH_DT2,t.NONPAY_CAT+'-'+t2.SUB_DESC as 'NONPAY'");
            sb.Append(" from TB_S_M_INS2_NONPAY_SET  t");
            sb.Append(" left join TB_H_M_EMP h on t.EMP_ID=h.EMP_ID");
            sb.Append(" left join TB_9_M_COMM_D t2 on t2.MAIN_CD='NONPAY_CAT' and t2.SUB_CD=t.NONPAY_CAT");
            sb.Append(" where 1=1  ");
            if (txt_EMP_ID != "")
            {
                sb.Append(" and t.EMP_ID LIKE @EMP_ID  ");
                ht.Add("@EMP_ID", string.Format("{0}%", txt_EMP_ID));
            }
            if (txt_START_YM != "")
            {

                sb.Append(" and substring(t.START_YM,1,4) <= " + txt_START_YM + " and substring(case when t.END_YM = '' then '9999/12/31' else t.END_YM end,1,4) >= " + txt_START_YM + " ");
            }
            if (txt_LICENSE_ID != "")
            {
                sb.Append(" and h.LICENSE_ID = @LICENSE_ID");
                ht.Add("@LICENSE_ID", txt_LICENSE_ID);
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
    public int getCount(int startRowIndex, int maximumRows, string txt_EMP_ID, string txt_START_YM, string txt_LICENSE_ID)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) total_record ");
            sb.Append(" from TB_S_M_INS2_NONPAY_SET  t");
            sb.Append(" left join TB_H_M_EMP h on t.EMP_ID=h.EMP_ID");
            sb.Append(" left join TB_9_M_COMM_D t2 on t2.MAIN_CD='NONPAY_CAT' and t2.SUB_CD=t.NONPAY_CAT");
            sb.Append(" where 1=1  ");
            if (txt_EMP_ID != "")
            {
                sb.Append(" and t.EMP_ID LIKE @EMP_ID  ");
                ht.Add("@EMP_ID", string.Format("{0}%", txt_EMP_ID));
            }
            if (txt_START_YM != "")
            {

                sb.Append(" and substring(t.START_YM,1,4) <= " + txt_START_YM + " and substring(case when t.END_YM = '' then '9999/12/31' else t.END_YM end,1,4) >= " + txt_START_YM + " ");
            }
            if (txt_LICENSE_ID != "")
            {
                sb.Append(" and h.LICENSE_ID = @LICENSE_ID");
                ht.Add("@LICENSE_ID", txt_LICENSE_ID);
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

    public DataTable getModeData(string id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("     select ROW_NUMBER() OVER(ORDER BY m.FUNC_ID) As RowNumber,");
            sb.Append("     d.* ");
            sb.Append("     from TB_9_M_SYS_M as m					");
            sb.Append("     inner join TB_9_M_SYS_D as d on m.FUNC_ID = d.FUNC_ID and m.SYS_ID+m.MODE_ID = @ID");
            sb.Append("     where 1=1");

            ht.Add("@ID", id);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    public DataTable getModeData(int startRowIndex, int maximumRows, string sortExpression, string id)
    {
        try
        {
            if (sortExpression == "")
            {
                sortExpression = "MODE_ID";
            }
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * From");
            sb.Append(" (");
            sb.Append("     select ROW_NUMBER() OVER(ORDER BY m.FUNC_ID) As RowNumber,");
            sb.Append("     d.* ");
            sb.Append("     from TB_9_M_SYS_M as m					");
            sb.Append("     inner join TB_9_M_SYS_D as d on m.FUNC_ID = d.FUNC_ID and m.SYS_ID+m.MODE_ID = @ID");
            sb.Append("     where 1=1");
            sb.Append(" ) god_data where RowNumber between CAST(@startRowIndex as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar)");

            ht.Add("@ID", id);
            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public int getModeCount(int startRowIndex, int maximumRows, string id)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select COUNT(*) total_record from (");
            sb.Append(" 	select d.MODE_ID, d.FUNCTION_ID, d.FUNCTION_NAME");
            sb.Append("     from TB_9_M_SYS_M as m					");
            sb.Append("     inner join TB_9_M_SYS_D as d on m.FUNC_ID = d.FUNC_ID and m.SYS_ID+m.MODE_ID = @ID");
            sb.Append("     where 1=1");
            sb.Append(" ) as tb1");

            ht.Add("@ID", id);
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

    //public DataTable getData()
    //{
    //    try
    //    {
    //        StringBuilder sb = new StringBuilder();
    //        sb.Append(" Select * From TB_9_M_COMM_H";
    //         sb.Append(" where 1=1";

    //        if (SYS_CD != "")
    //        {
    //             sb.Append(" and SYS_CD = @SYS_CD ";
    //            ht.Add("@SYS_CD", SYS_CD);
    //        }

    //        return dbConn.Query(sb, ht);
    //    }
    //    catch (Exception)
    //    {

    //        throw;
    //    }
    //}
    public string deleteData(string deleteitem)
    {
        //刪除共用代碼主檔
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        //寫log
        sb.AppendLine(" update TB_S_M_INS2_NONPAY_SET set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2IB020' ");
        sb.AppendLine(" where EMP_ID = @EMP_ID; ");
        ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);

        sb.Append(" Delete from TB_S_M_INS2_NONPAY_SET ");
        sb.Append(" where EMP_ID = @EMP_ID;");
        ht.Add("@EMP_ID", deleteitem);
        dbConn.ExecuteT(sb, ht, true);

        return "0";
    }
    internal DataTable getExistData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select * from TB_S_M_INS2_NONPAY_SET where EMP_ID = @EMP_ID");
            ht.Add("@EMP_ID", EMP_ID);
           
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
            sb.Append("Select * from TB_S_M_INS2_DETAIL_TMP where EMP_ID = @EMP_ID");
            ht.Add("@EMP_ID", deleteitem);

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
            sb.Append("INSERT INTO TB_S_M_INS2_NONPAY_SET (EMP_ID,LICENSE_ID,EMP_NAME,NONPAY_CAT,REMARK,START_YM,END_YM,BIRTH_DT,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" Values (@EMP_ID,@LICENSE_ID,@EMP_NAME,@NONPAY_CAT,@REMARK,@START_YM,@END_YM,@BIRTH_DT,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@LICENSE_ID", LICENSE_ID);
            ht.Add("@EMP_NAME", EMP_NAME);
            ht.Add("@NONPAY_CAT", NONPAY_CAT);
            ht.Add("@REMARK", REMARK);
            ht.Add("@START_YM", START_YM);
            ht.Add("@CREATED_BY", CREATED_BY);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@END_YM", END_YM);
          
            ht.Add("@BIRTH_DT", BIRTH_DT);
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
            sb.Append("Update TB_S_M_INS2_NONPAY_SET ");
            sb.Append(" Set NONPAY_CAT=@NONPAY_CAT,REMARK=@REMARK,START_YM=@START_YM,END_YM=@END_YM,UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE()");
            sb.Append(" where EMP_ID = @EMP_ID");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@NONPAY_CAT", NONPAY_CAT);
            ht.Add("@REMARK", REMARK);
            ht.Add("@START_YM", START_YM);
            ht.Add("@UPDATED_BY", UPDATED_BY);
                ht.Add("@END_YM", END_YM);
          

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
}