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
public class CFB2DG030DAO : BaseDAO
{
    public CFB2DG030DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public string RowNumber { get; set; }
    public string QDATAKEY { get; set; }

    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }
    //for查詢欄位
    public string PLANT_CD { get; set; }
    public string PARKING_SPOT { get; set; }
    public string PARKING_SPOT1 { get; set; }
    public string USING_PARKING_SPOT1 { get; set; }

    public string ddl_SYS_ID { get; set; }
    public int X { get; set; }
    public string PARKING_PLANT_CD { get; set; }
    public string CAR_NO { get; set; }
    public string CLOCK2 { get; set; }
    public string EMP_NAME { get; set; }
    public string EMP_ID { get; set; }
    public string EMP_CD { get; set; }
    public string DEPT_NO { get; set; }
    public string DEPT_NAME { get; set; }
    public string CAR_BRAND { get; set; }
    public string CAR_TYPE { get; set; }
    public string CAR_PARK_NO { get; set; }
    public string CAR_PARK_NO_N { get; set; }
    public string PARKING_CD { get; set; }
    public string LEVEL_CD { get; set; }
    public string PJOB_CD { get; set; }
    public string PJOB_NAME { get; set; }
    public string WORK_SHIFT { get; set; }
    public string PARKING_TOOL { get; set; }
    public string IFLOW_NO { get; set; }
    public string REMAINDER_PARKING_SPOT { get; set; }    
    
    public DataTable getCB(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select CAR_BRAND,CAR_TYPE from TB_D_M_PARKING_EMP_MAIN");
            sb.Append(" where emp_id = @emp_id  ");

            ht.Add("@emp_id", emp_id);

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getCarParkNo(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select CAR_PARK_NO,PARKING_PLANT_CD from TB_D_M_PARKING_EMP_MAIN");
            sb.Append(" where emp_id = @emp_id  ");

            ht.Add("@emp_id", emp_id);

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getCAR_TYPE()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select SYS_CD , MAIN_CD , SUB_CD ,SUB_DESC from TB_9_M_COMM_D");
            sb.Append(" where SYS_CD='DG' and MAIN_CD='CAR_TYPE'  ");
            if (CAR_TYPE == "2")
            {
                sb.Append(" and  SUB_CD = '4'");
            }
            //else {
            //    sb.Append(" and  SUB_CD != '4'");
            //}
            return dbConn.Query(sb);
        }
        catch
        {
            throw;
        }
    }

    public DataTable getPARKING_PLANT_CD()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select SYS_CD , MAIN_CD , SUB_CD ,SUB_DESC from TB_9_M_COMM_D");
            sb.Append(" where SYS_CD='DG' and MAIN_CD='PARKING_PLANT_CD'  ");
            return dbConn.Query(sb);
        }
        catch
        {
            throw;
        }
    }

    public DataTable getPLANT_CD()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select SYS_CD , MAIN_CD , SUB_CD ,SUB_DESC from TB_9_M_COMM_D");
            sb.Append(" where SYS_CD='HB' and MAIN_CD='PLANT_CD'  ");
            return dbConn.Query(sb);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getPARKING_CD()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select SYS_CD , MAIN_CD , SUB_CD ,SUB_DESC from TB_9_M_COMM_D");
            sb.Append(" where SYS_CD='DG' and MAIN_CD='PARKING_CD'  ");
            return dbConn.Query(sb);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getCAR_PARK_NO()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select CAR_PARK_NO , PARKING_NAME  from TB_D_M_PARKING_MAIN");
            if (PLANT_CD != "-1")
            {
                sb.Append(" where PLANT_CD = @PLANT_CD  ");
                ht.Add("@PLANT_CD", PLANT_CD);
            }

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getREMAINDER_PARKING_SPOT()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select CAR_PARK_NO , (select dbo.FN_D_GET_REMAINDER_PARKING(@CAR_PARK_NO)) REMAINDER_PARKING_SPOT, NEEDSELECT  from TB_D_M_PARKING_MAIN");
            if (CAR_PARK_NO != "-1")
            {
                sb.Append(" where CAR_PARK_NO = @CAR_PARK_NO  ");
                ht.Add("@CAR_PARK_NO", CAR_PARK_NO);
            }

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getCAR_BRAND()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select SYS_CD , MAIN_CD , SUB_CD ,SUB_DESC from TB_9_M_COMM_D");
            sb.Append(" where SYS_CD='DG' and MAIN_CD='CAR_BRAND'  ");
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
    internal DataTable getDefaultData(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" select ROW_NUMBER() OVER(ORDER BY p.EMP_ID ) As RowNumber,");
            sb.Append(" P.*,D.DEPT_NAME,C.SUB_CD+'-'+C.SUB_DESC AS CAR_BRAND2,CC.SUB_CD+'-'+CC.SUB_DESC AS CAR_PARK_NO2,PC.SUB_CD+'-'+PC.SUB_DESC AS PLANT_CD ,PM.CAR_PARK_NO+'-'+PM.PARKING_NAME as CAR_PARK2,CT.SUB_CD+'-'+CT.SUB_DESC as CAR_TYPE2,substring(ve.WORK_SHIFT_CD,2,1) as WORK_SHIFT_CD , ve.WORK_SHIFT_CD as wsc ,ve.LEVEL_CD,ve.PJOB_DESC,ve.PJOB_CD   ");
            sb.Append(" from TB_D_M_PARKING_EMP_MAIN P");
            sb.Append(" left join TB_H_M_DEPT D  on  P.DEPT_NO=D.DEPT_NO");
            sb.Append(" left join TB_9_M_COMM_D C on  P.CAR_BRAND=C.SUB_CD and C.SYS_CD='DG' and C.MAIN_CD='CAR_BRAND'");
            sb.Append(" left join TB_9_M_COMM_D CC on  P.PARKING_TOOL=CC.SUB_CD and CC.SYS_CD='DG' and CC.MAIN_CD='CAR_PARK_NO'");
            sb.Append(" left join TB_9_M_COMM_D PC on  P.PARKING_PLANT_CD=PC.SUB_CD and PC.SYS_CD='DG' and PC.MAIN_CD='PARKING_PLANT_CD'");
            sb.Append(" left join TB_D_M_PARKING_MAIN PM on  P.CAR_PARK_NO=PM.CAR_PARK_NO");
            sb.Append(" left join TB_9_M_COMM_D CT on  P.CAR_TYPE=CT.SUB_CD and CT.SYS_CD='DG' and CT.MAIN_CD='CAR_TYPE'");
            sb.Append(" left join VW_H_EMP_DATA VE on  Ve.EMP_ID=P.EMP_ID");
            sb.Append(" where 1=1");



            sb.Append(" and (p.EMP_ID = @EMP_ID)  ");
            ht.Add("@EMP_ID", emp_id);

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
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string txt_EMP_ID, string txt_EMP_NAME, string txt_DEPT_NO, string ddl_PLANT_CD, string txt_CAR_PARK_NO, string txt_CAR_NO)
    {
        try
        {
            //if (sortExpression.Contains("DEPT_NAME"))
            //    sortExpression = sortExpression.Replace("DEPT_NAME", "D.DEPT_NAME");

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * From");
            if (sortExpression.Contains("DEPT_NAME"))
                sb.Append(" (select ROW_NUMBER() OVER(ORDER BY D." + sortExpression + " ) As RowNumber,e.LEVEL_CD,e.PJOB_CD,e.PJOB_DESC,e.WORK_SHIFT_CD,e.WORK_SHIFT_DESC,");
            else
                sb.Append(" (select ROW_NUMBER() OVER(ORDER BY p." + sortExpression + " ) As RowNumber,e.LEVEL_CD,e.PJOB_CD,e.PJOB_DESC,e.WORK_SHIFT_CD,e.WORK_SHIFT_DESC,");
            sb.Append(" P.*,IIF(isnull(E.DIV_DEPT_FULL_NAME,'')='',E.DEPT_NAME,E.DIV_DEPT_FULL_NAME) DEPT_NAME,C.SUB_CD+'-'+C.SUB_DESC AS CAR_BRAND2,CC.SUB_CD+'-'+CC.SUB_DESC AS CAR_PARK_NO2,PC.SUB_CD+'-'+PC.SUB_DESC AS PLANT_CD ,PM.CAR_PARK_NO+'-'+PM.PARKING_NAME as CAR_PARK2 ");
            sb.Append(" from TB_D_M_PARKING_EMP_MAIN P");
            //sb.Append(" left join VW_H_DEPT_DATA D  on  P.DEPT_NO=D.DEPT_NO");
            sb.Append(" left join TB_9_M_COMM_D C on  P.CAR_BRAND=C.SUB_CD and C.SYS_CD='DG' and C.MAIN_CD='CAR_BRAND'");
            sb.Append(" left join TB_9_M_COMM_D CC on  P.PARKING_TOOL=CC.SUB_CD and CC.SYS_CD='DG' and CC.MAIN_CD='CAR_PARK_NO'");
            sb.Append(" left join TB_9_M_COMM_D PC on  P.PARKING_PLANT_CD=PC.SUB_CD and PC.SYS_CD='DG' and PC.MAIN_CD='PLANT_CD'");
            sb.Append(" left join TB_D_M_PARKING_MAIN PM on  P.CAR_PARK_NO=PM.CAR_PARK_NO");
            sb.Append(" left join VW_H_EMP_DATA e on e.EMP_ID=P.EMP_ID");
            sb.Append(" where 1=1");
            if (txt_EMP_ID != "")
            {
                sb.Append(" and P.EMP_ID = @EMP_ID  ");
                ht.Add("@EMP_ID", txt_EMP_ID);
            }
            if (txt_EMP_NAME != "")
            {
                sb.Append(" and P.EMP_NAME LIKE @EMP_NAME  ");
                ht.Add("@EMP_NAME", string.Format("%{0}%", txt_EMP_NAME));
            }
            if (txt_DEPT_NO != "")
            {
                sb.Append(" and P.DEPT_NO = @DEPT_NO  ");
                ht.Add("@DEPT_NO", txt_DEPT_NO);
            }
            if (ddl_PLANT_CD != "" && ddl_PLANT_CD != "-1")
            {
                sb.Append(" and P.PARKING_PLANT_CD = @PLANT_CD  ");
                ht.Add("@PLANT_CD", ddl_PLANT_CD.Substring(0, 1));
            }
            if (txt_CAR_PARK_NO != "")
            {
                sb.Append(" and PM.CAR_PARK_NO = @CAR_PARK_NO  ");
                ht.Add("@CAR_PARK_NO", txt_CAR_PARK_NO);
            }
            if (txt_CAR_NO != "")
            {
                sb.Append(" and P.CAR_NO = @CAR_NO  ");
                ht.Add("@CAR_NO", txt_CAR_NO);
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
    public int getCount(int startRowIndex, int maximumRows, string txt_EMP_ID, string txt_EMP_NAME, string txt_DEPT_NO, string ddl_PLANT_CD, string txt_CAR_PARK_NO, string txt_CAR_NO)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) total_record ");
            sb.Append(" from TB_D_M_PARKING_EMP_MAIN p");
            //sb.Append(" left join VW_H_DEPT_DATA D  on  P.DEPT_NO=D.DEPT_NO");
            sb.Append(" left join TB_9_M_COMM_D C on  P.CAR_BRAND=C.SUB_CD and C.SYS_CD='DG' and C.MAIN_CD='CAR_BRAND'");
            sb.Append(" left join TB_9_M_COMM_D CC on  P.PARKING_TOOL=CC.SUB_CD and CC.SYS_CD='DG' and CC.MAIN_CD='CAR_PARK_NO'");
            sb.Append(" left join TB_9_M_COMM_D PC on  P.PARKING_PLANT_CD=PC.SUB_CD and PC.SYS_CD='DG' and PC.MAIN_CD='PLANT_CD'");
            sb.Append(" left join TB_D_M_PARKING_MAIN PM on  P.CAR_PARK_NO=PM.CAR_PARK_NO");
            sb.Append(" left join VW_H_EMP_DATA E on e.EMP_ID=P.EMP_ID");
            sb.Append(" where 1=1");
            if (txt_EMP_ID != "")
            {
                sb.Append(" and p.EMP_ID = @EMP_ID  ");
                ht.Add("@EMP_ID", txt_EMP_ID);
            }
            if (txt_EMP_NAME != "")
            {
                sb.Append(" and P.EMP_NAME LIKE @EMP_NAME  ");
                ht.Add("@EMP_NAME", string.Format("%{0}%", txt_EMP_NAME));
            }
            if (txt_DEPT_NO != "")
            {
                sb.Append(" and p.DEPT_NO = @DEPT_NO  ");
                ht.Add("@DEPT_NO", txt_DEPT_NO);
            }
            if (ddl_PLANT_CD != "" && ddl_PLANT_CD != "-1")
            {
                sb.Append(" and p.PARKING_PLANT_CD = @PLANT_CD  ");
                ht.Add("@PLANT_CD", ddl_PLANT_CD.Substring(0, 1));
            }
            if (txt_CAR_PARK_NO != "")
            {
                sb.Append(" and PM.CAR_PARK_NO = @CAR_PARK_NO  ");
                ht.Add("@CAR_PARK_NO", txt_CAR_PARK_NO);
            }
            if (txt_CAR_NO != "")
            {
                sb.Append(" and p.CAR_NO = @CAR_NO  ");
                ht.Add("@CAR_NO", txt_CAR_NO);
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
    public DataTable getData2(int startRowIndex, int maximumRows, string sortExpression, string txt_EMP_ID)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * From  ");
            sb.Append(" (select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" T.EMP_ID, convert(varchar,T.UPDATE_DT,111) UPDATE_DT,T.EMP_NAME,D1.SUB_CD+'-'+D1.SUB_DESC AS PARKING_PLANT_CD ,T.DEPT_NO,IIF(isnull(E.DIV_DEPT_FULL_NAME,'')='',T.DEPT_NAME,E.DIV_DEPT_FULL_NAME) DEPT_NAME,");
            sb.Append(" T.LEVEL_CD,T.PJOB_NAME,D3.SUB_DESC AS PARKING_TOOL,T.CAR_PARK_NO,D2.SUB_DESC AS CAR_BRAND,T.CAR_TYPE+'-'+D4.SUB_DESC CAR_TYPE,T.CAR_NO");
            sb.Append(" from TB_D_M_PARKING_HISTORY T ");
            sb.Append(" left join VW_H_EMP_DATA e on e.EMP_ID=T.EMP_ID");
            sb.Append(" LEFT JOIN TB_9_M_COMM_D D1 ON T.PARKING_PLANT_CD= D1.SUB_CD AND D1.SYS_CD='DG' and D1.MAIN_CD='PARKING_PLANT_CD'");
            sb.Append(" LEFT JOIN TB_9_M_COMM_D D2 ON T.CAR_BRAND= D2.SUB_CD AND D2.SYS_CD='DG' and D2.MAIN_CD='CAR_BRAND' and D2.IS_VALID = 'Y' ");
            sb.Append(" LEFT JOIN TB_9_M_COMM_D D3 ON T.PARKING_TOOL= D3.SUB_CD AND D3.SYS_CD='DG' and D3.MAIN_CD='PARKING_CD' and D3.IS_VALID = 'Y'");
            sb.Append(" LEFT JOIN TB_9_M_COMM_D D4 ON T.CAR_TYPE= D4.SUB_CD AND D4.SYS_CD='DG' and D4.MAIN_CD='CAR_TYPE' and D4.IS_VALID = 'Y'");

            sb.Append(" where 1=1 and T.EMP_ID =@EMP_ID ");
            ht.Add("@EMP_ID", txt_EMP_ID);



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
    public int getCount2(int startRowIndex, int maximumRows, string txt_EMP_ID)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) total_record ");
            sb.Append(" from TB_D_M_PARKING_HISTORY");
            sb.Append(" where 1=1 and EMP_ID =@EMP_ID");
            ht.Add("@EMP_ID", txt_EMP_ID);

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
    public DataTable getaddData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select CLOCK_NO, CLOCK_NO+'-'+CLOCK_DESC as CLOCK_NAME");
            sb.Append(" from TB_D_M_CLOCK C");
            sb.Append(" where 1=1 AND CLOCK_TYPE = 'C' ");
            if (PARKING_CD != "-1")
            {
                sb.Append(" and C.CLOCK_USED_CD=@PLANT_CD  ");
                ht.Add("@PLANT_CD", PARKING_CD);
            }

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getaddData2(string id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select CLOCK_NO,CLOCK_NO+'-'+CLOCK_DESC as CLOCK,C.EMP_ID from TB_D_M_CLOCK T,TB_D_M_PARKING_CLOCK C where T.CLOCK_NO=C.PARKING_VALID_CLOCK and C.EMP_ID=@EMP_ID");
            ht.Add("@EMP_ID", id);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    public DataTable getModeData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * from (");
            sb.Append(" select DISTINCT CLOCK_NO, CLOCK_NO+'-'+CLOCK_DESC as CLOCK,pc.PARKING_VALID_CLOCK");
            sb.Append(" from TB_D_M_CLOCK C");
            sb.Append(" left outer join TB_D_M_PARKING_CLOCK as PC on EMP_ID=@EMP_ID and pc.PARKING_VALID_CLOCK=c.CLOCK_NO");
            ht.Add("@EMP_ID", EMP_ID);
            sb.Append(" where 1=1 and c.CLOCK_TYPE = 'C' ");
            if (PARKING_CD != "-1")
            {
                sb.Append(" and C.CLOCK_USED_CD=@PLANT_CD  ");
                ht.Add("@PLANT_CD", PARKING_CD);
            }
            sb.Append(" )td where isnull(PARKING_VALID_CLOCK,'') = ''");





            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public DataTable checkNEEDSELECT(string id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select a.NEEDSELECT from TB_D_M_PARKING_MAIN a");
            sb.Append(" left join TB_D_M_PARKING_EMP_MAIN b on a.CAR_PARK_NO = b.CAR_PARK_NO");
            sb.Append(" where b.EMP_ID=@EMP_ID");
            ht.Add("@EMP_ID", id);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getModeData2(string id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select CLOCK_NO,CLOCK_NO+'-'+CLOCK_DESC as CLOCK,C.EMP_ID from TB_D_M_CLOCK T,TB_D_M_PARKING_CLOCK C where T.CLOCK_NO=C.PARKING_VALID_CLOCK and C.EMP_ID=@EMP_ID");
            ht.Add("@EMP_ID", id);
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
    public string deleteData_2(string deleteitem)
    {
        //刪除共用代碼主檔
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        char[] ch1 = new Char[] { '|' };
        string[] split1 = deleteitem.Split(ch1);
        string EMP_ID = split1[0].ToString();
        //寫log
        sb.Append(" update TB_D_M_PARKING_EMP_MAIN set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2DG030' ");
        sb.Append(" where EMP_ID = @EMP_ID; ");
        ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);

        sb.Append(" Delete from TB_D_M_PARKING_EMP_MAIN ");
        sb.Append(" where EMP_ID = @EMP_ID;");
        ht.Add("@EMP_ID", EMP_ID);
        dbConn.ExecuteT(sb, ht, true);

        //寫log
        sb.Append(" update TB_D_M_PARKING_CLOCK set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2DG030' ");
        sb.Append(" where EMP_ID = @EMP_ID; ");
        ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);

        sb.Append(" Delete from TB_D_M_PARKING_CLOCK ");
        sb.Append(" where EMP_ID = @EMP_ID;");
        ht.Add("@EMP_ID", EMP_ID);

        dbConn.ExecuteT(sb, ht, true);

        return "0";
    }
    internal DataTable getExistData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select * from TB_D_M_PARKING_EMP_MAIN where EMP_ID = @EMP_ID");
            ht.Add("@EMP_ID", EMP_ID);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
    internal void addData_1_2()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("INSERT INTO TB_D_M_PARKING_EMP_MAIN (EMP_ID,EMP_NAME,PARKING_PLANT_CD,DEPT_NO,CAR_NO,CAR_BRAND,CAR_TYPE,CAR_PARK_NO,PARKING_TOOL,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" Values (@EMP_ID,@EMP_NAME,@PARKING_PLANT_CD,@DEPT_NO,@CAR_NO,@CAR_BRAND,@CAR_TYPE,@CAR_PARK_NO,@PARKING_TOOL,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@EMP_NAME", EMP_NAME);

            ht.Add("@PARKING_PLANT_CD", PARKING_PLANT_CD);
            ht.Add("@DEPT_NO", DEPT_NO);
            ht.Add("@CAR_NO", CAR_NO);
            ht.Add("@CAR_BRAND", CAR_BRAND);
            ht.Add("@CAR_TYPE", CAR_TYPE);

            ht.Add("@CAR_PARK_NO", CAR_PARK_NO);
            ht.Add("@PARKING_TOOL", PARKING_CD);

            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@CREATED_BY", CREATED_BY);
            ht.Add("@FUNC_ID", "FB2DG030");

            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }
    internal void addData_1_1()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Update TB_D_M_PARKING_EMP_MAIN ");
            sb.Append(" Set PARKING_PLANT_CD=@PARKING_PLANT_CD,CAR_NO=@CAR_NO,CAR_BRAND=@CAR_BRAND,CAR_TYPE=@CAR_TYPE,CAR_PARK_NO=@CAR_PARK_NO,PARKING_TOOL=@PARKING_TOOL,UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE() ");
            sb.Append(" where EMP_ID = @EMP_ID");
            ht.Add("@EMP_ID", EMP_ID);


            ht.Add("@PARKING_PLANT_CD", PARKING_PLANT_CD);

            ht.Add("@CAR_NO", CAR_NO);
            ht.Add("@CAR_BRAND", CAR_BRAND);
            ht.Add("@CAR_TYPE", CAR_TYPE);

            ht.Add("@CAR_PARK_NO", CAR_PARK_NO);
            ht.Add("@PARKING_TOOL", PARKING_CD);

            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@CREATED_BY", CREATED_BY);
            ht.Add("@FUNC_ID", "FB2DG030");

            dbConn.ExecuteT(sb, ht, true);

        }
        catch (Exception)
        {
            throw;
        }
    }
    public string addData_1(string deleteitem)
    {

        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();
        char[] ch1 = new Char[] { '|' };
        string[] split1 = deleteitem.Split(ch1);
        string EMP_ID = split1[0].ToString();
        string EMP_NAME = split1[1].ToString();
        string PARKING_PLANT_CD = split1[2].ToString();
        string DEPT_NO = split1[3].ToString();
        string DEPT_NAME = split1[4].ToString();
        string LEVEL_CD = split1[5].ToString();
        string PJOB_CD = split1[6].ToString();
        string PJOB_NAME = split1[7].ToString();
        string WORK_SHIFT = split1[8].ToString();
        string CAR_NO = split1[9].ToString();
        string CAR_BRAND = split1[10].ToString();
        string CAR_TYPE = split1[11].ToString();
        string PARKING_TOOL = split1[12].ToString();
        string CAR_PARK_NO = split1[13].ToString();
        string IFLOW_NO = split1[14].ToString();
        string CREATED_BY = split1[15].ToString();
        string FUNC_ID = "FB2DG030";




        sb.Append("INSERT INTO TB_D_M_PARKING_HISTORY (EMP_ID,UPDATE_DT,EMP_NAME,PARKING_PLANT_CD,DEPT_NO,DEPT_NAME,LEVEL_CD,PJOB_CD,PJOB_NAME,WORK_SHIFT,CAR_NO,CAR_BRAND,CAR_TYPE,PARKING_TOOL,CAR_PARK_NO,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,IFLOW_NO,FUNC_ID)");
        sb.Append(" Values (@EMP_ID,GETDATE(),@EMP_NAME,@PARKING_PLANT_CD,@DEPT_NO,@DEPT_NAME,@LEVEL_CD,@PJOB_CD,@PJOB_NAME,@WORK_SHIFT,@CAR_NO,@CAR_BRAND,@CAR_TYPE,@PARKING_TOOL,@CAR_PARK_NO,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@IFLOW_NO,@FUNC_ID)");
        ht.Add("@EMP_ID", EMP_ID);
        ht.Add("@EMP_NAME", EMP_NAME.Trim());
        ht.Add("@PARKING_PLANT_CD", PARKING_PLANT_CD);
        ht.Add("@DEPT_NO", DEPT_NO);
        ht.Add("@DEPT_NAME", DEPT_NAME);
        ht.Add("@LEVEL_CD", LEVEL_CD);
        //ht.Add("@PJOB_CD", PJOB_CD.Substring(0, 2));
        ht.Add("@PJOB_CD", PJOB_CD);
        ht.Add("@PJOB_NAME", PJOB_NAME.Trim());
        ht.Add("@WORK_SHIFT", WORK_SHIFT.Trim());
        ht.Add("@CAR_NO", CAR_NO.Trim());
        ht.Add("@CAR_BRAND", CAR_BRAND.Trim());
        ht.Add("@CAR_TYPE", CAR_TYPE.Trim());
        ht.Add("@PARKING_TOOL", PARKING_TOOL);
        ht.Add("@CAR_PARK_NO", CAR_PARK_NO);
        ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
        ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
        ht.Add("@IFLOW_NO", IFLOW_NO);
        ht.Add("@FUNC_ID", FUNC_ID);


        dbConn.ExecuteT(sb, ht, true);
        return "0";

    }
    internal DataTable addData_2(string CAR_PARK)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select COUNT(*) total_record ");
            sb.Append(" from TB_D_M_PARKING_EMP_MAIN");
            sb.Append(" where CAR_PARK_NO=@CAR_PARK_NO");

            ht.Add("@CAR_PARK_NO", CAR_PARK);

            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }
    internal string checkParking(string CAR_PARK_NO)
    {
        try
        {
            string NEEDSELECT = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" select NEEDSELECT from TB_D_M_PARKING_MAIN");
            sb.Append(" where CAR_PARK_NO = @CAR_PARK_NO");

            ht.Add("@CAR_PARK_NO", CAR_PARK_NO);
            DataTable dt = new DataTable();

            dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
	        {
                NEEDSELECT = dt.Rows[0]["NEEDSELECT"].ToString();
	        }

            return NEEDSELECT;

        }
        catch (Exception)
        {

            throw;
        }
    }
    internal DataTable addData_2_1()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("INSERT INTO TB_D_M_PARKING_HISTORY (EMP_ID,UPDATE_DT,EMP_NAME,PARKING_PLANT_CD,DEPT_NO,DEPT_NAME,LEVEL_CD,PJOB_CD,PJOB_NAME,WORK_SHIFT,CAR_NO,CAR_BRAND,CAR_TYPE,PARKING_TOOL,CAR_PARK_NO,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,IFLOW_NO,FUNC_ID)");
            sb.Append(" Values (@EMP_ID,GETDATE(),@EMP_NAME,@PARKING_PLANT_CD,@DEPT_NO,@DEPT_NAME,@LEVEL_CD,@PJOB_CD,@PJOB_NAME,@WORK_SHIFT,@CAR_NO,@CAR_BRAND,@CAR_TYPE,@PARKING_TOOL,@CAR_PARK_NO,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@IFLOW_NO,@FUNC_ID)");
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@EMP_NAME", EMP_NAME);
            ht.Add("@PARKING_PLANT_CD", PARKING_PLANT_CD);
            ht.Add("@DEPT_NO", DEPT_NO);
            ht.Add("@DEPT_NAME", DEPT_NAME);
            ht.Add("@LEVEL_CD", LEVEL_CD);
            //ht.Add("@PJOB_CD", PJOB_CD.Substring(0, 2));
            ht.Add("@PJOB_CD", PJOB_CD);
            ht.Add("@PJOB_NAME", PJOB_NAME);
            ht.Add("@WORK_SHIFT", WORK_SHIFT);
            ht.Add("@CAR_NO", CAR_NO);
            ht.Add("@CAR_BRAND", CAR_BRAND);
            ht.Add("@CAR_TYPE", CAR_TYPE);
            ht.Add("@PARKING_TOOL", PARKING_TOOL);
            ht.Add("@CAR_PARK_NO", CAR_PARK_NO);
            ht.Add("@CREATED_BY", CREATED_BY);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@IFLOW_NO", IFLOW_NO);
            ht.Add("@FUNC_ID", FUNC_ID);



            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }
    internal void addData_3()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Update TB_D_M_PARKING_MAIN ");
            sb.Append(" Set USING_PARKING_SPOT=@USING_PARKING_SPOT,REMAINDER_PARKING_SPOT = (select dbo.FN_D_GET_REMAINDER_PARKING(@CAR_PARK_NO) ),UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = 'FB2DG030'");
            sb.Append(" where CAR_PARK_NO = @CAR_PARK_NO");


            ht.Add("@CAR_PARK_NO", CAR_PARK_NO);
            ht.Add("@USING_PARKING_SPOT", CAR_PARK_NO_N);
            //ht.Add("@REMAINDER_PARKING_SPOT", REMAINDER_PARKING_SPOT);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
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
            sb.Append("Update TB_D_M_PARKING_EMP_MAIN ");
            sb.Append(" Set PARKING_PLANT_CD=@PARKING_PLANT_CD,CAR_NO=@CAR_NO,CAR_BRAND=@CAR_BRAND,CAR_TYPE=@CAR_TYPE,CAR_PARK_NO=@CAR_PARK_NO,PARKING_TOOL=@PARKING_TOOL,UPDATED_BY=@UPDATED_BY,UPDATED_DT=GETDATE() ");
            sb.Append(" where EMP_ID = @EMP_ID");
            ht.Add("@EMP_ID", EMP_ID);


            ht.Add("@PARKING_PLANT_CD", PARKING_PLANT_CD);

            ht.Add("@CAR_NO", CAR_NO);
            ht.Add("@CAR_BRAND", CAR_BRAND);
            ht.Add("@CAR_TYPE", CAR_TYPE);

            ht.Add("@CAR_PARK_NO", CAR_PARK_NO);
            ht.Add("@PARKING_TOOL", PARKING_CD);

            ht.Add("@UPDATED_BY", UPDATED_BY);




            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    internal void delCLOCK()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Delete from TB_D_M_PARKING_CLOCK ");
            sb.Append(" where EMP_ID = @EMP_ID;");

            ht.Add("@EMP_ID", EMP_ID);

            dbConn.ExecuteT(sb, ht, true);            
        }
        catch (Exception)
        {
            throw;
        }
    }

    internal void CLOCK()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();


            if (X == 0)
            {
                //寫log
                sb.Append(" update TB_D_M_PARKING_CLOCK set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2DG030' ");
                sb.Append(" where EMP_ID = @EMP_ID; ");
                ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);

                sb.Append(" Delete from TB_D_M_PARKING_CLOCK ");
                sb.Append(" where EMP_ID = @EMP_ID;");

                ht.Add("@EMP_ID", EMP_ID);

                dbConn.ExecuteT(sb, ht, true);

            }



            sb.Append("INSERT INTO TB_D_M_PARKING_CLOCK (EMP_ID,PARKING_VALID_CLOCK,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" Values (@EMP_ID,@PARKING_VALID_CLOCK,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),'FB2DG030')");
            //Int32 C = CLOCK2.IndexOf("-");
            ht.Add("@PARKING_VALID_CLOCK", CLOCK2.Substring(0, 3));
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    internal void re_Cal_REMainder(string car_park_no)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(@"declare @nowUsing varchar(3);
                        select @nowUsing = count(*) from TB_D_M_PARKING_EMP_MAIN
                        where CAR_PARK_NO = @CAR_PARK_NO;

                        Update TB_D_M_PARKING_MAIN
                        Set REMAINDER_PARKING_SPOT=(SELECT dbo.FN_D_GET_REMAINDER_PARKING(@CAR_PARK_NO)),USING_PARKING_SPOT = @nowUsing,
                        UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = 'FB2DG030'
                        where CAR_PARK_NO = @CAR_PARK_NO;  ");
            ht.Add("@CAR_PARK_NO", car_park_no);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    internal void REMAINDER_PARKING_SPOT_2()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Update TB_D_M_PARKING_MAIN ");
            sb.Append(" Set REMAINDER_PARKING_SPOT=@REMAINDER_PARKING_SPOT,UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = 'FB2DG030'");
            sb.Append(" where CAR_PARK_NO = @CAR_PARK_NO");
            ht.Add("@CAR_PARK_NO", CAR_PARK_NO);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);

            string RPS = string.Empty;
            DataTable dt = new DataTable();
            dt = getREMAINDER_PARKING_SPOT_2(CAR_PARK_NO);
            if (dt.Rows.Count > 0)
            {
                RPS = dt.Rows[0]["REMAINDER_PARKING_SPOT"].ToString();
            }

            ht.Add("@REMAINDER_PARKING_SPOT", RPS);


            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    public DataTable calREMAINDER_PARKING_SPOT()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * from TB_D_M_PARKING_MAIN ");

            return dbConn.Query(sb);
        }
        catch
        {
            throw;
        }
    }

    public DataTable getREMAINDER_PARKING_SPOT_1()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * from TB_D_M_PARKING_MAIN ");

            return dbConn.Query(sb);
        }
        catch
        {
            throw;
        }
    }

    public DataTable getREMAINDER_PARKING_SPOT_2(string CAR_PARK_NO)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" SELECT dbo.FN_D_GET_REMAINDER_PARKING(@CAR_PARK_NO) AS REMAINDER_PARKING_SPOT ");
            ht.Add("@CAR_PARK_NO", CAR_PARK_NO);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

}