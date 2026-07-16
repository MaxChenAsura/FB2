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
/// CFB2DC0100DAO 的摘要描述
/// </summary>
public class CFB2DC0100DAO : BaseDAO
{
    public string CLOCK_NO { get; set; }
    public string CLOCK_DESC { get; set; }
    public string CLOCK_TYPE { get; set; }
    public string IS_VALID { get; set; }
    public string CLOCK_USED_CD { get; set; }
    public string PLANT_CD { get; set; }
    public string PARK_DEFAULT { get; set; }
    public string CLOCK_IP { get; set; }
    public string CREATED_BY { get; set; }
    public string UPDATED_BY { get; set; }
    public string FUNC_ID { get; set; }

    public CFB2DC0100DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string clock_type, string plant_cd, string clock_no)
    {
        try
        {
            if (sortExpression.Contains("CLOCK_TYPE"))
                sortExpression = sortExpression.Replace("CLOCK_TYPE", "a.CLOCK_TYPE");

            if (sortExpression.Contains("CLOCK_NO"))
                sortExpression = sortExpression.Replace("CLOCK_NO", "a.CLOCK_NO");

            if (sortExpression.Contains("IS_VALID"))
                sortExpression = sortExpression.Replace("IS_VALID", "a.IS_VALID");

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * From");
            sb.Append(" (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber, ");
            sb.Append(" a.CLOCK_NO,a.CLOCK_DESC, b.SUB_CD+'-'+b.SUB_DESC CLOCK_TYPE,a.IS_VALID, ");
            //--DC010卡鐘維護 當卡鐘類別為餐廳(B)時用途區分是取共用代碼檔的MAIN_CD = RESTAURANT_CD
            //--當卡鐘類別為停車場(C)時用途區分是取共用代碼檔的MAIN_CD = PARKING_PLANT_CD
            sb.Append(" case when a.CLOCK_TYPE='B' then c.SUB_CD+'-'+c.SUB_DESC ");
            sb.Append("      when a.CLOCK_TYPE='C' then d.SUB_CD+'-'+d.SUB_DESC ");
            sb.Append(" else '' end CLOCK_USED_CD, ");
            sb.Append(" e.SUB_CD+'-'+e.SUB_DESC PLANT_CD,a.PARK_DEFAULT,a.CLOCK_IP ");
            sb.Append(" from TB_D_M_CLOCK a ");
            sb.Append(" left join TB_9_M_COMM_D b on b.SYS_CD='DC' and b.MAIN_CD='CLOCK_TYPE' and b.IS_VALID='Y' and a.CLOCK_TYPE=b.SUB_CD ");
            sb.Append(" left join TB_9_M_COMM_D c on c.SYS_CD='DE' and c.MAIN_CD='RESTAURANT_CD' and c.IS_VALID='Y' and a.CLOCK_USED_CD=c.SUB_CD ");
            sb.Append(" left join TB_9_M_COMM_D d on d.SYS_CD='DG' and d.MAIN_CD='PARKING_CD' and d.IS_VALID='Y' and a.CLOCK_USED_CD=d.SUB_CD ");
            sb.Append(" left join TB_9_M_COMM_D e on e.SYS_CD='HB' and e.MAIN_CD='PLANT_CD' and e.IS_VALID='Y' and a.PLANT_CD=e.SUB_CD");
            sb.Append(" where 1=1 ");

            if (clock_type != "-1" && clock_type != null)
            {
                sb.Append(" and a.CLOCK_TYPE = @clock_type ");
                ht.Add("@clock_type", clock_type);
            }

            if (plant_cd != "-1" && plant_cd != null)
            {
                sb.Append(" and a.PLANT_CD = @plant_cd ");
                ht.Add("@plant_cd", plant_cd);
            }

            if (clock_no != "")
            {
                sb.Append(" and a.CLOCK_NO LIKE @clock_no ");
                ht.Add("@clock_no", clock_no + "%");
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

    public int getCount(int startRowIndex, int maximumRows, string clock_type, string plant_cd, string clock_no)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select COUNT(*) total_record ");
            sb.Append(" from TB_D_M_CLOCK a ");
            sb.Append(" left join TB_9_M_COMM_D b on b.SYS_CD='DC' and b.MAIN_CD='CLOCK_TYPE' and b.IS_VALID='Y' and a.CLOCK_TYPE=b.SUB_CD ");
            sb.Append(" left join TB_9_M_COMM_D c on c.SYS_CD='DE' and c.MAIN_CD='RESTAURANT_CD' and c.IS_VALID='Y' and a.CLOCK_USED_CD=c.SUB_CD ");
            sb.Append(" left join TB_9_M_COMM_D d on d.SYS_CD='DG' and d.MAIN_CD='PARKING_CD' and d.IS_VALID='Y' and a.CLOCK_USED_CD=d.SUB_CD ");
            sb.Append(" left join TB_9_M_COMM_D e on e.SYS_CD='HB' and e.MAIN_CD='PLANT_CD' and e.IS_VALID='Y' and a.PLANT_CD=e.SUB_CD");
            sb.Append(" where 1=1 ");

            if (clock_type != "-1" && clock_type != null)
            {
                sb.Append(" and a.CLOCK_TYPE = @clock_type ");
                ht.Add("@clock_type", clock_type);
            }

            if (plant_cd != "-1" && plant_cd != null)
            {
                sb.Append(" and a.PLANT_CD = @plant_cd ");
                ht.Add("@plant_cd", plant_cd);
            }

            if (clock_no != "")
            {
                sb.Append(" and a.CLOCK_NO LIKE @clock_no ");
                ht.Add("@clock_no", clock_no + "%");
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

    //刪除 TB_D_M_CLOCK 卡鐘情報檔
    public void deleteCLOCK(string item)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //寫log
            sb.Append(" update TB_D_M_CLOCK set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2DC010' ");
            sb.Append(" where CLOCK_NO = @CLOCK_NO;");
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);

            sb.Append(" delete from TB_D_M_CLOCK");
            sb.Append(" where CLOCK_NO = @CLOCK_NO;");
            ht.Add("@CLOCK_NO", item);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public DataTable getExistData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select CLOCK_NO from TB_D_M_CLOCK");
            sb.Append(" where CLOCK_NO = @CLOCK_NO");
            ht.Add("@CLOCK_NO", CLOCK_NO);

            return dbConn.Query(sb, ht);
        }
        catch (Exception ex)
        {

            throw;
        }
    }

    //新增 TB_D_M_CLOCK 卡鐘情報檔
    public void addCLOCK()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("insert into TB_D_M_CLOCK ( ");
            sb.Append(" CLOCK_NO,CLOCK_TYPE,CLOCK_USED_CD,IS_VALID,CLOCK_IP,PLANT_CD,CLOCK_DESC,PARK_DEFAULT,");
            sb.Append(" CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" values ( ");
            sb.Append(" @CLOCK_NO,@CLOCK_TYPE,@CLOCK_USED_CD,@IS_VALID,@CLOCK_IP,@PLANT_CD,@CLOCK_DESC,@PARK_DEFAULT,");
            sb.Append(" @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");

            ht.Add("@CLOCK_NO", CLOCK_NO);
            ht.Add("@CLOCK_TYPE", CLOCK_TYPE);
            ht.Add("@CLOCK_USED_CD", CLOCK_USED_CD);
            ht.Add("@IS_VALID", IS_VALID);
            ht.Add("@CLOCK_IP", CLOCK_IP);
            ht.Add("@PLANT_CD", PLANT_CD);
            ht.Add("@CLOCK_DESC", CLOCK_DESC);
            ht.Add("@PARK_DEFAULT", PARK_DEFAULT);
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

    //更新 TB_D_M_CLOCK 卡鐘情報檔
    public void updateCLOCK()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_D_M_CLOCK ");
            sb.Append(" set CLOCK_TYPE=@CLOCK_TYPE,CLOCK_USED_CD=@CLOCK_USED_CD,IS_VALID=@IS_VALID,CLOCK_IP=@CLOCK_IP,");
            sb.Append(" PLANT_CD=@PLANT_CD,CLOCK_DESC=@CLOCK_DESC,PARK_DEFAULT=@PARK_DEFAULT,");
            sb.Append(" UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE(),FUNC_ID=@FUNC_ID");
            sb.Append(" where CLOCK_NO=@CLOCK_NO ");

            ht.Add("@CLOCK_NO", CLOCK_NO);
            ht.Add("@CLOCK_TYPE", CLOCK_TYPE);
            ht.Add("@CLOCK_USED_CD", CLOCK_USED_CD);
            ht.Add("@IS_VALID", IS_VALID);
            ht.Add("@CLOCK_IP", CLOCK_IP);
            ht.Add("@PLANT_CD", PLANT_CD);
            ht.Add("@CLOCK_DESC", CLOCK_DESC);
            ht.Add("@PARK_DEFAULT", PARK_DEFAULT);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }


    public DataTable check_CLOCK_IP()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select count(*) resultCount ");
            sb.Append(" from TB_D_M_CLOCK ");
            sb.Append(" where CLOCK_IP=@CLOCK_IP and CLOCK_NO!=@CLOCK_NO");
            ht.Add("@CLOCK_NO", CLOCK_NO);
            ht.Add("@CLOCK_IP", CLOCK_IP);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }

    }



    public DataTable getCLOCK_DESC(string clock_no)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select CLOCK_NO,CLOCK_DESC ");
            sb.Append(" from TB_D_M_CLOCK ");
            sb.Append(" where CLOCK_NO=@CLOCK_NO ");
            ht.Add("@CLOCK_NO", clock_no);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
}