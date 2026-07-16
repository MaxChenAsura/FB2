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
public class CFB2IA0100DAO : BaseDAO
{
    public string REDUCE_CD { get; set; }
    public string EFFECT_DT { get; set; }
    public string REDUCE_DESC { get; set; }
    public string LAB_RATE { get; set; }
    public string HEA_RATE { get; set; }
    public string GOV_AMOUNT { get; set; }
    public string REMARK { get; set; }
    public string UNEFFECT_DT { get; set; }
    public string CREATED_BY { get; set; }
    public string UPDATED_BY { get; set; }
    public string FUNC_ID { get; set; }

    public CFB2IA0100DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string reduce_cd, string reduce_desc)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * From");
            sb.Append(" (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,REDUCE_CD,");
            sb.Append(" EFFECT_DT,");
            sb.Append(" REDUCE_DESC,LAB_RATE,HEA_RATE,GOV_AMOUNT,REMARK,");
            sb.Append(" UNEFFECT_DT");
            sb.Append(" from TB_I_M_REDUCE ");
            sb.Append(" where 1=1 ");

            if (reduce_cd != "")
            {
                sb.Append(" and REDUCE_CD LIKE @reduce_cd ");
                ht.Add("@reduce_cd", reduce_cd + "%");
            }

            if (reduce_desc != "")
            {
                sb.Append(" and REDUCE_DESC LIKE @reduce_desc ");
                ht.Add("@reduce_desc", "%" + reduce_desc + "%");
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

    public int getCount(int startRowIndex, int maximumRows, string reduce_cd, string reduce_desc)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select COUNT(*) total_record from TB_I_M_REDUCE");
            sb.Append(" where 1=1 ");

            if (reduce_cd != "")
            {
                sb.Append(" and REDUCE_CD LIKE @reduce_cd ");
                ht.Add("@reduce_cd", reduce_cd + "%");
            }

            if (reduce_desc != "")
            {
                sb.Append(" and REDUCE_DESC LIKE @reduce_desc ");
                ht.Add("@reduce_desc", "%" + reduce_desc + "%");
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
            sb.Append("select REDUCE_CD from TB_I_M_REDUCE");
            sb.Append(" where REDUCE_CD = @REDUCE_CD and EFFECT_DT = @EFFECT_DT ");
            ht.Add("@REDUCE_CD", REDUCE_CD);
            ht.Add("@EFFECT_DT", EFFECT_DT);

            return dbConn.Query(sb, ht);
        }
        catch (Exception ex)
        {

            throw;
        }
    }
    //取得該等級的最大的生效時間
    internal DataTable getMaxEndDTByType()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select MAX(UNEFFECT_DT) maxEndDT from TB_I_M_REDUCE ");
            sb.Append(" where REDUCE_CD=@REDUCE_CD ");
            ht.Add("@REDUCE_CD", REDUCE_CD);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
    //新增 TB_I_M_REDUCE
    public void addREDUCE()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("insert into TB_I_M_REDUCE ( ");
            sb.Append(" REDUCE_CD,EFFECT_DT,REDUCE_DESC,LAB_RATE,HEA_RATE,GOV_AMOUNT,REMARK,UNEFFECT_DT");
            sb.Append(" ,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" values ( ");
            sb.Append(" @REDUCE_CD,@EFFECT_DT,@REDUCE_DESC,@LAB_RATE,@HEA_RATE,@GOV_AMOUNT,@REMARK,@UNEFFECT_DT");
            sb.Append(" ,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@REDUCE_CD", REDUCE_CD);
            ht.Add("@EFFECT_DT", EFFECT_DT);
            ht.Add("@REDUCE_DESC", REDUCE_DESC);
            ht.Add("@LAB_RATE", LAB_RATE);
            ht.Add("@HEA_RATE", HEA_RATE);
            ht.Add("@GOV_AMOUNT", GOV_AMOUNT);
            ht.Add("@REMARK", REMARK);
            if (UNEFFECT_DT == "")
                ht.Add("@UNEFFECT_DT", "9999/12/31");
            else
                ht.Add("@UNEFFECT_DT", UNEFFECT_DT);

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

    //更新 TB_I_M_REDUCE
    public void updateREDUCE()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_I_M_REDUCE ");
            sb.Append(" set REDUCE_DESC=@REDUCE_DESC,");
            sb.Append(" LAB_RATE=@LAB_RATE,HEA_RATE=@HEA_RATE,GOV_AMOUNT=@GOV_AMOUNT,");
            sb.Append(" REMARK=@REMARK,UNEFFECT_DT=@UNEFFECT_DT,UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE()");
            sb.Append(" where REDUCE_CD = @REDUCE_CD and EFFECT_DT = @EFFECT_DT ");

            ht.Add("@REDUCE_CD", REDUCE_CD);
            ht.Add("@EFFECT_DT", EFFECT_DT);
            ht.Add("@REDUCE_DESC", REDUCE_DESC);
            ht.Add("@LAB_RATE", LAB_RATE);
            ht.Add("@HEA_RATE", HEA_RATE);
            ht.Add("@GOV_AMOUNT", GOV_AMOUNT);
            ht.Add("@REMARK", REMARK);
            if (UNEFFECT_DT == "")
                ht.Add("@UNEFFECT_DT", "9999/12/31");
            else
                ht.Add("@UNEFFECT_DT", UNEFFECT_DT);

            ht.Add("@UPDATED_BY", UPDATED_BY);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //刪除 TB_I_M_REDUCE
    public void deleteREDUCE(string reduce_cd, string effect_dt)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //寫log
            sb.AppendLine(" update TB_I_M_REDUCE set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2IA010' ");
            sb.AppendLine(" where REDUCE_CD = @REDUCE_CD and EFFECT_DT = @EFFECT_DT; ");
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);

            sb.Append(" delete from TB_I_M_REDUCE ");
            sb.Append(" where REDUCE_CD = @REDUCE_CD and EFFECT_DT = @EFFECT_DT; ");
            ht.Add("@REDUCE_CD", reduce_cd);
            ht.Add("@EFFECT_DT", effect_dt);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
}