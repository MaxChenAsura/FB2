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
/// CFB2DL0500BO 的摘要描述
/// </summary>
public class CFB2DL0500DAO : BaseDAO
{
    public CFB2DL0500DAO()
    {
        // 
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public string RowNumber { get; set; }
    public string DL_GEN_CD { get; set; }
    public string DL_GEN_DESC { get; set; }
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




    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string dl_gen_desc)
    {
        try
        {
            if (sortExpression == "")
            {
                sortExpression = "DL_GEN_CD ASC";
            }
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * From");
            sb.Append(" (select ROW_NUMBER() OVER(ORDER BY j." + sortExpression + " ) As RowNumber");
            sb.Append(",J.* ");
            sb.Append(",PROC_CD +'-'+ isnull(C.SUB_DESC,'') as PROC_CD_DESC ");
            sb.Append(",SDT_CD +'-'+ isnull(D.SUB_DESC,'') as SDT_CD_DESC ");
            sb.Append(",EDT_CD +'-'+ isnull(E.SUB_DESC,'') as EDT_CD_DESC ");
            sb.Append(",LOGI_CD +'-'+ isnull(F.SUB_DESC,'') as LOGI_CD_DESC ");
            sb.Append(",DL_GENDT_CD +'-'+ isnull(G.SUB_DESC,'') as DL_GENDT_CD_DESC ");
            sb.Append(",J.IS_D01_SAME +'-'+ isnull(H.SUB_DESC,'') as IS_D01_SAME_DESC  ");
            sb.Append(" FROM  TB_D_M_D0_DL_GEN_CD as J ");
            sb.Append(" LEFT JOIN TB_9_M_COMM_D as C ON j.PROC_CD =C.SUB_CD and C.main_cd = 'PROC_CD' and C.IS_VALID='Y' ");
            sb.Append(" LEFT JOIN TB_9_M_COMM_D as D ON j.SDT_CD = D.SUB_CD and D.main_cd = 'SDT_CD'  and D.IS_VALID='Y' ");
            sb.Append(" LEFT JOIN TB_9_M_COMM_D as E ON j.EDT_CD = E.SUB_CD and E.main_cd = 'EDT_CD'  and E.IS_VALID='Y' ");
            sb.Append(" LEFT JOIN TB_9_M_COMM_D as F ON j.LOGI_CD = F.SUB_CD and F.main_cd = 'LOGI_CD'  and F.IS_VALID='Y' ");
            sb.Append(" LEFT JOIN TB_9_M_COMM_D as G ON j.DL_GENDT_CD = G.SUB_CD and G.main_cd = 'DL_GENDT_CD'  and G.IS_VALID='Y' ");
            sb.Append(" LEFT JOIN TB_9_M_COMM_D as H ON j.IS_D01_SAME = H.SUB_CD and H.main_cd = 'IS_D01_SAME'  and H.IS_VALID='Y'  ");
            sb.Append(" where 1=1 ");

            if (dl_gen_desc != "")
             {
                 sb.Append(" and J.DL_GEN_DESC LIKE @dl_gen_desc  ");
                 ht.Add("@dl_gen_desc", string.Format("%{0}%", dl_gen_desc));
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
    public int getCount(int startRowIndex, int maximumRows, string dl_gen_desc)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) total_record ");
            sb.Append(" from TB_D_M_D0_DL_GEN_CD j ");
            sb.Append(" where 1=1");

            if (dl_gen_desc != "")
            {
                sb.Append(" and j.DL_GEN_DESC LIKE @dl_gen_desc  ");
                ht.Add("@dl_gen_desc", string.Format("%{0}%", dl_gen_desc));
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
  
    public DataTable getData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select * ");
            sb.Append(" From TB_D_M_D0_DL_GEN_CD");
            sb.Append(" where 1=1 and DL_GEN_CD = @DL_GEN_CD ");
            ht.Add("@DL_GEN_CD", DL_GEN_CD);            
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
    public string deleteData()
    {
        //刪除 
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();       
        sb.Append(" Delete from TB_D_M_D0_DL_GEN_CD ");
        sb.Append(" where DL_GEN_CD=@DL_GEN_CD  ;");
        ht.Add("@DL_GEN_CD", DL_GEN_CD);

        dbConn.ExecuteT(sb, ht, true);

        return "0";
    }
    internal DataTable getExistData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select * from TB_D_M_D0_DL_GEN_CD where DL_GEN_CD = @DL_GEN_CD ;");
            ht.Add("@DL_GEN_CD", DL_GEN_CD);

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
            sb.Append(@" INSERT INTO TB_D_M_D0_DL_GEN_CD 
                        (DL_GEN_CD,DL_GEN_DESC,PROC_CD,SDT_CD,EDT_CD
                        ,LOGI_CD,DL_GENDT_CD,IS_D01_SAME
                        ,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID
                        )
                        VALUES
                        (@DL_GEN_CD,@DL_GEN_DESC,@PROC_CD,@SDT_CD,@EDT_CD
                        ,@LOGI_CD,@DL_GENDT_CD,@IS_D01_SAME
                        ,@CREATED_BY,getdate(),@UPDATED_BY,getdate(),@FUNC_ID
                        )
                        ");

            ht.Add("@DL_GEN_CD", DL_GEN_CD);
            ht.Add("@DL_GEN_DESC", DL_GEN_DESC);
            ht.Add("@PROC_CD", PROC_CD);
            ht.Add("@SDT_CD", SDT_CD);
            ht.Add("@EDT_CD", EDT_CD);
            ht.Add("@LOGI_CD", LOGI_CD);
            ht.Add("@DL_GENDT_CD", DL_GENDT_CD);
            ht.Add("@IS_D01_SAME", IS_D01_SAME);
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

    internal void updData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" update  TB_D_M_D0_DL_GEN_CD 
                        SET DL_GEN_DESC = @DL_GEN_DESC
                        ,UPDATED_BY = @UPDATED_BY 
                        ,UPDATED_DT = getdate()
                        ,FUNC_ID = @FUNC_ID     
                        where  DL_GEN_CD = @DL_GEN_CD           
                        ");

            ht.Add("@DL_GEN_CD", DL_GEN_CD);
            ht.Add("@DL_GEN_DESC", DL_GEN_DESC);
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