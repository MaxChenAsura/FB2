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
public class CFB2DG010DAO : BaseDAO
{
    public CFB2DG010DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public string RowNumber { get; set; }
    public string QDATAKEY { get; set; }
    public string CAR_PARK_NO { get; set; }
    public string PLANT_CD { get; set; }
    public string PARKING_NAME { get; set; }
    public string PARKING_TYPE { get; set; }
    public string PARKING_SPOT { get; set; }
    public string PARKING_SPOT1 { get; set; }
    public string USING_PARKING_SPOT { get; set; }
    public string USING_PARKING_SPOT1 { get; set; }
    public string OVERLAP { get; set; }
    public string PARKING_LC_TYPE { get; set; }
    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }
    public string REMAINDER_PARKING_SPOT { get; set; }
    public string NEEDSELECT { get; set; }

    //for查詢欄位
    public string ddl_SYS_ID { get; set; }
    public string SYSCODE { get; set; }


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
    
    public string getTotalShift(string CAR_PARK_NO,string shift)
    {
        try
        {
            string st = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select FN_D_GET_WK_SHIFT_COUNT(@CAR_PARK_NO,@shift) total");
            ht.Add("@CAR_PARK_NO", CAR_PARK_NO);
            ht.Add("@shift", shift);

            DataTable dt = dbConn.Query(sb,ht);
            if (dt.Rows.Count > 0)
	        {
	            st = dt.Rows[0]["total"].ToString();	 
	        }
            return st;
        }
        catch
        {
            throw;
        }
    }

    public string getSetRate()
    {
        try
        {
            string st = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select CODE_VAL1 from TB_9_M_PARAMETER");
            sb.Append(" where SYS_CD = 'DG' and MAIN_CD = 'PARKING_RATE'");
           
            DataTable dt = dbConn.Query(sb);
            if (dt.Rows.Count > 0)
            {
                st = dt.Rows[0]["CODE_VAL1"].ToString();
            }
            return st;
        }
        catch
        {
            throw;
        }
    }

    public DataTable getSYS_ID()
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select SYS_CD , MAIN_CD , SUB_CD ,SUB_DESC from TB_9_M_COMM_D");
            sb.Append(" where SYS_CD='DG' and MAIN_CD='PARKING_PLANT_CD'  ");
            sb.Append(" AND  SUB_CD in (" + SYSCODE + ") ");
            
            return dbConn.Query(sb);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getPARKING_TYPE()
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
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string ddl_PLANT_CD, string txt_CAR_PARK_NO)
    {
        try
        {
            if (sortExpression == "")
            {
                sortExpression = "CAR_TYPE";
            }
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select * From");
            sb.Append(" (select ROW_NUMBER() OVER(ORDER BY CAR_PARK_NO ) As RowNumber");
            sb.Append(" , t.CAR_PARK_NO, t.PARKING_NAME, t.PARKING_TYPE, t.PARKING_SPOT");
            sb.Append(" , (SELECT COUNT(*) FROM TB_D_M_PARKING_EMP_MAIN WHERE CAR_PARK_NO = t.CAR_PARK_NO) AS USING_PARKING_SPOT");
            sb.Append(" , dbo.FN_D_GET_REMAINDER_PARKING(t.CAR_PARK_NO) as REMAINDER_PARKING");
            sb.Append(" , t.OVERLAP, t.CREATED_BY, t.CREATED_DT, t.UPDATED_BY, t.UPDATED_DT, t.FUNC_ID");
            sb.Append(" , d.SUB_CD+'-'+d.SUB_DESC as SUBPARKING_TYPE");
            sb.Append(" , t.PLANT_CD, E.SUB_DESC, t.PLANT_CD + '-' + E.SUB_DESC as PLANT_NAME, t.NEEDSELECT");
            sb.Append(" from TB_D_M_PARKING_MAIN t");
            sb.Append(" LEFT OUTER JOIN TB_9_M_COMM_D d ON d.SUB_CD=t.PARKING_TYPE and d.SYS_CD='DG' and d.MAIN_CD='PARKING_CD'");
            sb.Append(" LEFT OUTER JOIN TB_9_M_COMM_D E ON E.SUB_CD=t.PLANT_CD and E.SYS_CD='DG' and E.MAIN_CD='PARKING_PLANT_CD'");
            sb.Append(" where 1=1 ");
            if (ddl_PLANT_CD != "" && ddl_PLANT_CD != "-1")
            {
                sb.Append(" and PLANT_CD = @PLANT_CD  ");
                ht.Add("@PLANT_CD", ddl_PLANT_CD);
            }
            if (txt_CAR_PARK_NO != "")
            {
                sb.Append(" and CAR_PARK_NO = @CAR_PARK_NO  ");
                ht.Add("@CAR_PARK_NO", txt_CAR_PARK_NO);
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
    public int getCount(int startRowIndex, int maximumRows, string ddl_PLANT_CD, string txt_CAR_PARK_NO)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select COUNT(*) total_record ");
            sb.Append(" from TB_D_M_PARKING_MAIN");
            sb.Append(" where 1=1");
            if (ddl_PLANT_CD != "" && ddl_PLANT_CD != "-1")
            {
                sb.Append(" and PLANT_CD = @PLANT_CD  ");
                ht.Add("@PLANT_CD", ddl_PLANT_CD);
            }
            if (txt_CAR_PARK_NO != "")
            {
                sb.Append(" and CAR_PARK_NO = @CAR_PARK_NO  ");
                ht.Add("@CAR_PARK_NO", txt_CAR_PARK_NO);
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
    public int getUSING_PARKING_SPOT(string CAR_PARK_NO)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select COUNT(*) AS PE_PARKING_SPOT");
            sb.Append(" from TB_D_M_PARKING_EMP_MAIN");
            sb.Append(" where CAR_PARK_NO = @CAR_PARK_NO");
            ht.Add("@CAR_PARK_NO", CAR_PARK_NO);

            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
            {
                t = (int)dt.Rows[0]["PE_PARKING_SPOT"];
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
        sb.Append(" update TB_D_M_PARKING_MAIN set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2DG010' ");
        sb.Append(" where CAR_PARK_NO = @CAR_PARK_NO;");
        ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);

        sb.Append(" Delete from TB_D_M_PARKING_MAIN ");
        sb.Append(" where CAR_PARK_NO = @CAR_PARK_NO;");
        ht.Add("@CAR_PARK_NO", deleteitem);
        dbConn.ExecuteT(sb, ht, true);

        return "0";
    }
    internal DataTable getExistData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select * from TB_D_M_PARKING_MAIN where CAR_PARK_NO = @CAR_PARK_NO");
            ht.Add("@CAR_PARK_NO", CAR_PARK_NO);
           
            return dbConn.Query(sb, ht);
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

    internal void addData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("INSERT INTO TB_D_M_PARKING_MAIN (CAR_PARK_NO,PLANT_CD,PARKING_NAME,PARKING_TYPE,PARKING_SPOT,REMAINDER_PARKING_SPOT,USING_PARKING_SPOT,OVERLAP,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID,NEEDSELECT)");
            sb.Append(" Values (@CAR_PARK_NO,@PLANT_CD,@PARKING_NAME,@PARKING_TYPE,@PARKING_SPOT,@REMAINDER_PARKING_SPOT,@USING_PARKING_SPOT,@OVERLAP,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID,@NEEDSELECT)");
           
            ht.Add("@CAR_PARK_NO", CAR_PARK_NO.ToUpper());
            ht.Add("@PLANT_CD", PLANT_CD);
            ht.Add("@PARKING_NAME", PARKING_NAME);
            ht.Add("@PARKING_TYPE", PARKING_TYPE);
            ht.Add("@PARKING_SPOT", PARKING_SPOT);
            ht.Add("@REMAINDER_PARKING_SPOT", PARKING_SPOT);
            ht.Add("@USING_PARKING_SPOT", USING_PARKING_SPOT);
            ht.Add("@OVERLAP", OVERLAP);
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", FUNC_ID);
            ht.Add("@NEEDSELECT", NEEDSELECT);
            
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
            sb.Append("Update TB_D_M_PARKING_MAIN ");
            sb.Append(" Set PLANT_CD=@PLANT_CD,PARKING_NAME=@PARKING_NAME,PARKING_TYPE=@PARKING_TYPE,PARKING_SPOT=@PARKING_SPOT,OVERLAP=@OVERLAP,NEEDSELECT = @NEEDSELECT");
            sb.Append(" where CAR_PARK_NO = @CAR_PARK_NO");
            ht.Add("@CAR_PARK_NO", CAR_PARK_NO);
            ht.Add("@PLANT_CD", PLANT_CD.Substring(0, 1));
            ht.Add("@PARKING_NAME", PARKING_NAME);
            ht.Add("@PARKING_TYPE", PARKING_TYPE.Substring(0, 1));
            ht.Add("@PARKING_SPOT", PARKING_SPOT);
            ht.Add("@OVERLAP", OVERLAP);
            ht.Add("@NEEDSELECT", NEEDSELECT);
          

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
            sb.Append(" Set REMAINDER_PARKING_SPOT=@REMAINDER_PARKING_SPOT");
            sb.Append(" where CAR_PARK_NO = @CAR_PARK_NO");
            ht.Add("@CAR_PARK_NO", CAR_PARK_NO);
            int RPS = 0;
            int R = Convert.ToInt32(PARKING_SPOT1);
            int U = Convert.ToInt32(USING_PARKING_SPOT1);
            RPS = R - U;

            ht.Add("@REMAINDER_PARKING_SPOT", RPS.ToString());


            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }


    public string Syscode()
    {
        //權限:依登入者的權限設定，顯示符合該登入者的功能鍵																														
        //依照所取得的小分類，來顯示底下下拉選單- 廠別的內容																											
        string syscode = string.Empty;
        string derolecd = string.Empty;
        string dept = string.Empty;
        string departments = string.Empty;
        string SysCode = string.Empty;
        string st = string.Empty;
        ACESLib.ACES aces = new ACESLib.ACES();
        List<string> syscodelist = new List<string>();
        List<string> Codelist = new List<string>();
        string a = aces.GetRoles();
        foreach (string dbRoleCD in aces.GetRoles().Split(','))
        {
            derolecd = dbRoleCD.Trim();
            ACESLib.DEPTBean deptbean = (ACESLib.DEPTBean)aces.GetDEPTAuth(derolecd);
                      //第一個dbRoleCD執行不會exception
            //derolecd = "FB2DBOWNER";
            dept = deptbean.IsDEPT;
            departments = deptbean.Departments;
            SysCode = deptbean.SysCode;

            foreach (string code in SysCode.Split(','))
            {
                if (code.Trim().Equals("PLANT_CD"))
                {
                    string syscodeatt = aces.GetCodeAtt(derolecd.Trim(), code.Trim());

                    foreach (string item in syscodeatt.Split(','))
                    {
                        st = string.Format("'{0}'", item.Trim());
                        if (!Codelist.Contains(st))
                        {
                            Codelist.Add(st);
                        }
                    }

                }
            }

        }

        return string.Join(",", Codelist.ToArray());
    }

}