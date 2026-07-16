using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;

/// <summary>
/// CFB2DC0400DAO 的摘要描述
/// </summary>
public class CFB2DC0400DAO : BaseDAO
{
    public CFB2DC0400DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
  
    public string CARD_TYPE { get; set; }
    public string CARD_MID_NO { get; set; }
    public string CARD_SEQ { get; set; }
    public string CARD_NAME { get; set; }
    public string NOTES { get; set; }
    public string START_DT { get; set; }
    public string END_DT { get; set; }
    public string TEMP_CARD_CD { get; set; }
    public string CARD_HANDLE { get; set; }
    public string CARD_USED_CD { get; set; }
    public string PLANT_CD { get; set; }
    public string CREATED_BY { get; set; }
    public string UPDATED_BY { get; set; }
    public string FUNC_ID { get; set; }

    internal System.Data.DataTable getCARD_TYPE()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select CARD_TYPE,CARD_TYPE + '-' + CARD_TYPE_DESC CARD_TYPE_DESC from TB_D_M_CARD_TYPE order by CARD_TYPE");

            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }

    //Gridview 查詢資料
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string card_type, string card_no,
                            bool card_status1, bool card_status2, string card_handle, string change_dt, string ddl_change_dt, string plant_cd,string card_name)
    {
        try
        {

            //if (sortExpression.Contains("CARD_TYPE"))
            //{
               // sortExpression = sortExpression.Replace("CARD_TYPE", "a.CARD_TYPE");
            //}
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,H.*");
            sb.Append(" from ( select a.CARD_TYPE,a.CARD_TYPE + '-' + b.CARD_TYPE_DESC CARD_TYPE_DESC,");
            sb.Append(" b.CARD_USED_CD,b.CARD_USED_CD + '-' + c.SUB_DESC CARD_USED_DESC, ");
            sb.Append(" PERSON_ID,CARD_MID_NO,CARD_SEQ,NOTES,");
            sb.Append(" case CARD_USED_CD when 'A' then (select top 1 CARD_NAME from TB_D_M_CARD where PERSON_ID =a.PERSON_ID and CARD_TYPE in ('00','10') order by START_DT desc )");
            sb.Append(" when 'B' then (select VENDOR_MEMBER_NAME from TB_D_M_VENDOR_D where TB_D_M_VENDOR_D.VENDOR_MEMBER_NO = a.PERSON_ID) ");
            sb.Append(" else CARD_NAME END CARD_NAME ");
            sb.Append(" , isnull(g.DEPT_NO +'-'+ g.DEPT_NAME,'') as DEPT_NO ");
            sb.Append(" ,REPLACE(CONVERT(char(10), a.START_DT, 120),'-','/') START_DT,REPLACE(CONVERT(char(10), a.END_DT, 120),'-','/') END_DT,");
            sb.Append(" a.TEMP_CARD_CD,a.TEMP_CARD_CD + '-' + f.SUB_DESC TEMP_CARD_DESC,");
            sb.Append(" a.CARD_HANDLE ,a.CARD_HANDLE + '-' + e.SUB_DESC CARD_HANDLE_DESC");
            sb.Append(" ,g.EMP_CD ,g.EMP_CD + '-' + h.SUB_DESC EMP_CD_DESC");
            sb.Append(" ,g.LEVEL_CD+''+g.GRADE_CD LEVEL_CD_DESC ");
            sb.Append(" ,g.PJOB_CD+' '+g.PJOB_DESC	PJOB_DESC ");
            sb.Append(" from TB_D_M_CARD a inner join TB_D_M_CARD_TYPE b on a.CARD_TYPE = b.CARD_TYPE ");
            sb.Append(" inner join TB_9_M_COMM_D c on b.CARD_USED_CD = c.SUB_CD and c.SYS_CD = 'DC' and c.MAIN_CD = 'CARD_USED_CD'");
            sb.Append(" left join TB_9_M_COMM_D e on a.CARD_HANDLE = e.SUB_CD and e.SYS_CD = 'DC' and e.MAIN_CD = 'CARD_HANDLE'");
            sb.Append(" left join TB_9_M_COMM_D f on a.TEMP_CARD_CD = f.SUB_CD and f.SYS_CD = 'DC' and f.MAIN_CD = 'TEMP_CARD_CD'");
            sb.Append(" left join VW_H_EMP_DATA g on a.PERSON_ID = g.EMP_ID and a.CARD_TYPE in ('00','10') ");
            sb.Append(" left join TB_9_M_COMM_D h on g.EMP_CD = h.SUB_CD and h.SYS_CD = 'HB' and h.MAIN_CD = 'EMP_CD'");


            sb.Append(" where a.CARD_TYPE is not null ");

            if (card_type != "-1")
            {
                sb.Append(" and a.CARD_TYPE = @CARD_TYPE");
                ht.Add("@CARD_TYPE", card_type);
            }
            if (card_no != "")
            {
                sb.Append(" AND CARD_MID_NO + convert(varchar(1),CARD_SEQ) like @CARD_NO");
                ht.Add("@CARD_NO", card_no + "%");
            }
            if (card_name != "")
            {
                sb.Append(" AND CARD_NAME  like @CARD_NAME");
                ht.Add("@CARD_NAME", card_name + "%");
            }

            if (card_status1 && card_status2)
            {
                sb.Append(" and (a.END_DT < convert(char(10),GETDATE(),120) or a.END_DT >= convert(char(10),GETDATE(),120) or a.END_DT is null) ");

            }
            else if (card_status1 && !card_status2)
            {
                sb.Append(" and a.END_DT < convert(char(10),GETDATE(),120) ");
            }
            else if (!card_status1 && card_status2)
            {
                sb.Append(" and (a.END_DT >= convert(char(10),GETDATE(),120) or a.END_DT is null) ");
            }

            if (card_handle != "-1")
            {
                sb.Append(" and a.CARD_HANDLE = @CARD_HANDLE");
                ht.Add("@CARD_HANDLE", card_handle);
            }
            if (ddl_change_dt == "1")  //報到日
            {
                if (change_dt != "")
                {
                    sb.Append(" and exists (select EMP_ID from VW_H_EMP_DATA where REPLACE(convert(char(10),JOIN_DT,120),'-','/') = @BE_EMP_DT and a.PERSON_ID = VW_H_EMP_DATA.EMP_ID)");
                    ht.Add("@BE_EMP_DT", change_dt);
                }
            }
            if (ddl_change_dt == "2") //任現資格日
            {
                if (change_dt != "")
                {
                    sb.Append(" and exists (select EMP_ID from VW_H_EMP_DATA where REPLACE(convert(char(10),RECENT_LEVEL_DT,120),'-','/') = @RECENT_LEVEL_DT and a.PERSON_ID = VW_H_EMP_DATA.EMP_ID)");
                    ht.Add("@RECENT_LEVEL_DT", change_dt);
                }
            }
            if (ddl_change_dt == "3") //任現職務日
            {
                if (change_dt != "")
                {
                    sb.Append(" and exists (select EMP_ID from VW_H_EMP_DATA where REPLACE(convert(char(10),RECENT_PJOB_DT,120),'-','/') = @RECENT_PJOB_DT and a.PERSON_ID = VW_H_EMP_DATA.EMP_ID)");
                    ht.Add("@RECENT_PJOB_DT", change_dt);
                }
            }
            if (ddl_change_dt == "4") //任現部級部門日
            {
                if (change_dt != "")
                {
                    sb.Append(" and exists (select EMP_ID from VW_H_EMP_DATA where REPLACE(convert(char(10),RECENT_DEPT_DT,120),'-','/') = @RECENT_DEPT_DT and a.PERSON_ID = VW_H_EMP_DATA.EMP_ID)");
                    ht.Add("@RECENT_DEPT_DT", change_dt);
                }
            }

            if (ddl_change_dt == "-1")  //就用異動日
            {
                if (change_dt != "")
                {
                    sb.Append(" and CONVERT(VARCHAR(10),a.START_DT,111) =@UPDATED_DT ");
                    ht.Add("@UPDATED_DT", change_dt);
                }
            }
            if (plant_cd != "-1")
            {
                sb.Append(" and a.PLANT_CD = @PLANT_CD");
                ht.Add("@PLANT_CD", plant_cd);
            }
            sb.Append(" ) H )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
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
    //Gridview 查詢總筆數
    public int getCount(int startRowIndex, int maximumRows, string card_type, string card_no,
                            bool card_status1, bool card_status2, string card_handle, string change_dt, string ddl_change_dt, string plant_cd
                           , string card_name)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select COUNT(*) total_record ");
            sb.Append(" from TB_D_M_CARD a inner join TB_D_M_CARD_TYPE b on a.CARD_TYPE = b.CARD_TYPE ");
            sb.Append(" inner join TB_9_M_COMM_D c on b.CARD_USED_CD = c.SUB_CD and c.SYS_CD = 'DC' and c.MAIN_CD = 'CARD_USED_CD'");
            sb.Append(" left join TB_9_M_COMM_D e on a.CARD_HANDLE = e.SUB_CD and e.SYS_CD = 'DC' and e.MAIN_CD = 'CARD_HANDLE'");
            sb.Append(" left join TB_9_M_COMM_D f on a.TEMP_CARD_CD = f.SUB_CD and f.SYS_CD = 'DC' and f.MAIN_CD = 'TEMP_CARD_CD'");
            sb.Append(" where a.CARD_TYPE is not null ");
            if (card_type != "-1")
            {
                sb.Append(" and a.CARD_TYPE = @CARD_TYPE");
                ht.Add("@CARD_TYPE", card_type);
            }
            if (card_no != "")
            {
                sb.Append(" AND CARD_MID_NO + convert(varchar(1),CARD_SEQ) like @CARD_NO");
                ht.Add("@CARD_NO", card_no + "%");
            }
            if (card_name != "")
            {
                sb.Append(" AND CARD_NAME  like @CARD_NAME");
                ht.Add("@CARD_NAME", card_name + "%");
            }
            if (card_status1 && card_status2)
            {
                sb.Append(" and (a.END_DT < convert(char(10),GETDATE(),120) or a.END_DT >= convert(char(10),GETDATE(),120) or a.END_DT is null) ");

            }
            else if (card_status1 && !card_status2)
            {
                sb.Append(" and a.END_DT < convert(char(10),GETDATE(),120) ");
            }
            else if (!card_status1 && card_status2)
            {
                sb.Append(" and (a.END_DT >= convert(char(10),GETDATE(),120) or a.END_DT is null) ");
            }
            if (card_handle != "-1")
            {
                sb.Append(" and a.CARD_HANDLE = @CARD_HANDLE");
                ht.Add("@CARD_HANDLE", card_handle);
            }
            if (ddl_change_dt == "1")  //報到日
            {
                if (change_dt != "")
                {
                    sb.Append(" and exists (select EMP_ID from VW_H_EMP_DATA where REPLACE(convert(char(10),JOIN_DT,120),'-','/') = @BE_EMP_DT and a.PERSON_ID = VW_H_EMP_DATA.EMP_ID)");
                    ht.Add("@BE_EMP_DT", change_dt);
                }
            }
            if (ddl_change_dt == "2") //任現資格日
            {
                if (change_dt != "")
                {
                    sb.Append(" and exists (select EMP_ID from VW_H_EMP_DATA where REPLACE(convert(char(10),RECENT_LEVEL_DT,120),'-','/') = @RECENT_LEVEL_DT and a.PERSON_ID = VW_H_EMP_DATA.EMP_ID)");
                    ht.Add("@RECENT_LEVEL_DT", change_dt);
                }
            }
            if (ddl_change_dt == "3") //任現職務日
            {
                if (change_dt != "")
                {
                    sb.Append(" and exists (select EMP_ID from VW_H_EMP_DATA where REPLACE(convert(char(10),RECENT_PJOB_DT,120),'-','/') = @RECENT_PJOB_DT and a.PERSON_ID = VW_H_EMP_DATA.EMP_ID)");
                    ht.Add("@RECENT_PJOB_DT", change_dt);
                }
            }
            if (ddl_change_dt == "4")  //任現部級部門日
            {
                if (change_dt != "")
                {
                    sb.Append(" and exists (select EMP_ID from VW_H_EMP_DATA where REPLACE(convert(char(10),RECENT_DEPT_DT,120),'-','/') = @RECENT_DEPT_DT and a.PERSON_ID = VW_H_EMP_DATA.EMP_ID)");
                    ht.Add("@RECENT_DEPT_DT", change_dt);
                }
            }
            if (ddl_change_dt == "-1")  //就用異動日
            {
                if (change_dt != "")
                {
                    sb.Append(" and CONVERT(VARCHAR(10),a.START_DT,111) =@UPDATED_DT ");
                    ht.Add("@UPDATED_DT", change_dt);
                }
            }
            if (plant_cd != "-1")
            {
                sb.Append(" and a.PLANT_CD = @PLANT_CD");
                ht.Add("@PLANT_CD", plant_cd);
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

    internal DataTable getCARD_USED_CD(string card_type, string CARD_MID_NO)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select a.CARD_USED_CD,a.CARD_USED_CD + '-' + b.SUB_DESC CARD_USED_DESC,");
            sb.Append(" (select MAX(CARD_SEQ) + 1 from TB_D_M_CARD where TB_D_M_CARD.CARD_TYPE = a.CARD_TYPE and CARD_MID_NO = @CARD_MID_NO) CARD_SEQ");
            sb.Append(" from TB_D_M_CARD_TYPE a, TB_9_M_COMM_D b where a.CARD_USED_CD = b.SUB_CD and b.SYS_CD = 'DC' and b.MAIN_CD = 'CARD_USED_CD'");
            sb.Append(" and a.CARD_TYPE = @CARD_TYPE");
            ht.Add("@CARD_TYPE", card_type);
            ht.Add("@CARD_MID_NO", CARD_MID_NO);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getVW_H_EMP_DATA(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select EMP_NAME NAME,DEPT_NO+'-'+DEPT_NAME as DEPT_NO_DESC");
            sb.Append(" ,g.EMP_CD ,g.EMP_CD + '-' + h.SUB_DESC EMP_CD_DESC ");
            sb.Append(" ,g.LEVEL_CD+''+g.GRADE_CD LEVEL_CD_DESC ");
            sb.Append(" ,g.PJOB_CD+' '+g.PJOB_DESC	PJOB_DESC ");
            sb.Append(" from VW_H_EMP_DATA g ");
            sb.Append(" left join TB_9_M_COMM_D h on g.EMP_CD = h.SUB_CD and h.SYS_CD = 'HB' and h.MAIN_CD = 'EMP_CD'");
            sb.Append(" where EMP_ID = @EMP_ID");
            ht.Add("@EMP_ID", emp_id);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            
            throw;
        }
    }

    internal DataTable getTB_D_M_VENDOR_D(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select VENDOR_MEMBER_NAME NAME,'' as EMP_CD_DESC from TB_D_M_VENDOR_D where TB_D_M_VENDOR_D.VENDOR_MEMBER_NO = @EMP_ID");
            ht.Add("@EMP_ID", emp_id);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }



    internal DataTable getExistEMP()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select EMP_ID from VW_H_EMP_DATA where EMP_ID = @EMP_ID");
            ht.Add("@EMP_ID", PERSON_ID);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getExistV_EMP()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select VENDOR_MEMBER_NO NAME from TB_D_M_VENDOR_D where TB_D_M_VENDOR_D.VENDOR_MEMBER_NO = @EMP_ID");
            ht.Add("@EMP_ID", PERSON_ID);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getCardData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select count(*) CARD_COUNT from TB_D_M_CARD where PERSON_ID = @PERSON_ID and CARD_TYPE = @CARD_TYPE");
            ht.Add("@PERSON_ID", PERSON_ID);
            ht.Add("@CARD_TYPE", CARD_TYPE);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getCardSeqData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select isnull(max(CARD_SEQ),-1) CARD_SEQ from TB_D_M_CARD where CARD_MID_NO = @CARD_MID_NO and CARD_TYPE = @CARD_TYPE");
            ht.Add("@CARD_MID_NO", CARD_MID_NO);
            ht.Add("@CARD_TYPE", CARD_TYPE);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
    internal string getLoginPlantCD(string emp_id)
    {
        try
        {
            string st = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select PLANT_CD from TB_H_M_EMP where EMP_ID = @EMP_ID");
            ht.Add("@EMP_ID", emp_id);
            DataTable dt = dbConn.Query(sb, ht);
            if (dt.Rows.Count > 0)
	        {
		        st = dt.Rows[0]["PLANT_CD"].ToString();
	        }
            
            return st;
        }
        catch (Exception)
        {

            throw;
        }
    }
    internal void updateCardData(string CARD_SEQ)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_D_M_CARD set END_DT = @END_DT, ");
            sb.Append(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID");
            sb.Append(" from TB_D_M_CARD where CARD_TYPE = @CARD_TYPE and CARD_MID_NO = @CARD_MID_NO and CARD_SEQ = @CARD_SEQ");

            ht.Add("@END_DT", DateTime.Parse(START_DT).AddDays(-1));
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            ht.Add("@CARD_TYPE", CARD_TYPE);
            ht.Add("@CARD_MID_NO", CARD_MID_NO);
            ht.Add("@CARD_SEQ", CARD_SEQ);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void updateCardData_EMP()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" update TB_D_M_CARD set END_DT = @END_DT, ");
            sb.Append(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID");
            sb.Append(" from TB_D_M_CARD ");
            sb.Append(@" where PERSON_ID = @CARD_MID_NO
					     and CARD_TYPE in ('10','00')
					     and @START_DT between START_DT and END_DT");
            ht.Add("@START_DT", START_DT );
            ht.Add("@END_DT", DateTime.Parse(START_DT).AddDays(-1));
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            ht.Add("@CARD_MID_NO", CARD_MID_NO);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void deleteCardUPD_CTL()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //寫log
            sb.Append(" update TB_D_M_CARD_UPD_CTL set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2DC040' ");
            sb.Append(" where CARD_NO = @CARD_NO; ");
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);

            sb.Append("delete from TB_D_M_CARD_UPD_CTL Where CARD_NO = @CARD_NO; ");
            ht.Add("@CARD_NO", CARD_TYPE + CARD_MID_NO + CARD_SEQ);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void addCardUPD_CTL(string CARD_CHANGE_CD = "D")
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("insert into TB_D_M_CARD_UPD_CTL (CARD_NO,CARD_CHANGE_CD,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID) ");
            sb.Append(" values (@CARD_NO,@CARD_CHANGE_CD,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");
            ht.Add("@CARD_NO", CARD_TYPE + CARD_MID_NO + CARD_SEQ);
            ht.Add("@CARD_CHANGE_CD", CARD_CHANGE_CD);
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

    internal void addNewCard(int card_seq = 0)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("insert into TB_D_M_CARD (CARD_TYPE,CARD_MID_NO,CARD_SEQ,PERSON_ID,CARD_NAME,START_DT,END_DT,");
            sb.Append(" CARD_HANDLE,TEMP_CARD_CD,CARD_NO,NOTES,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID,PLANT_CD) ");
            sb.Append(" values (@CARD_TYPE,@CARD_MID_NO,@CARD_SEQ,@PERSON_ID,@CARD_NAME,@START_DT,@END_DT,");
            sb.Append(" '1',@TEMP_CARD_CD,@CARD_NO,@NOTES,");
            sb.Append(" @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID,@PLANT_CD)");
            ht.Add("@CARD_TYPE", CARD_TYPE);
            ht.Add("@CARD_MID_NO", CARD_MID_NO);
            if (card_seq != 0)
                ht.Add("@CARD_SEQ", card_seq);
            else
                ht.Add("@CARD_SEQ", CARD_SEQ);
            ht.Add("@PERSON_ID", PERSON_ID);
            ht.Add("@CARD_NAME", CARD_NAME);
            ht.Add("@START_DT", START_DT);
            if (END_DT == "")
                ht.Add("@END_DT", "9999/12/31");
            else
                ht.Add("@END_DT", END_DT);
            if (card_seq != 0)
                ht.Add("@CARD_NO", CARD_TYPE + CARD_MID_NO + card_seq);
            else
                ht.Add("@CARD_NO", CARD_TYPE + CARD_MID_NO + CARD_SEQ);
            ht.Add("@NOTES", NOTES);
            ht.Add("@TEMP_CARD_CD", TEMP_CARD_CD == "-1" ? "" : TEMP_CARD_CD);
            ht.Add("@CREATED_BY", CREATED_BY);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            ht.Add("@PLANT_CD", PLANT_CD);

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
            sb.Append("update TB_D_M_CARD set CARD_NAME = @CARD_NAME,NOTES = @NOTES,START_DT = @START_DT,END_DT = @END_DT,TEMP_CARD_CD = @TEMP_CARD_CD, ");
            sb.Append(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID");
            sb.Append(" from TB_D_M_CARD where CARD_TYPE = @CARD_TYPE and CARD_MID_NO = @CARD_MID_NO and CARD_SEQ = @CARD_SEQ");
            ht.Add("@CARD_NAME", CARD_NAME);
            ht.Add("@NOTES", NOTES);
            ht.Add("@START_DT", START_DT);
            if (END_DT == "")
                ht.Add("@END_DT", "9999/12/31");
            else
                ht.Add("@END_DT", END_DT);
            ht.Add("@TEMP_CARD_CD", TEMP_CARD_CD == "-1" ? "" : TEMP_CARD_CD);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);

            ht.Add("@CARD_TYPE", CARD_TYPE);
            ht.Add("@CARD_MID_NO", CARD_MID_NO);
            ht.Add("@CARD_SEQ", CARD_SEQ);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void deleteCard(string CARD_TYPE, string CARD_MID_NO, string CARD_SEQ)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //寫log
            sb.Append(" update TB_D_M_CARD set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2DC040' ");
            sb.Append(" where CARD_TYPE = @CARD_TYPE and CARD_MID_NO = @CARD_MID_NO and CARD_SEQ = @CARD_SEQ; ");
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);

            sb.Append("delete from TB_D_M_CARD Where CARD_TYPE = @CARD_TYPE and CARD_MID_NO = @CARD_MID_NO and CARD_SEQ = @CARD_SEQ;");
            ht.Add("@CARD_TYPE", CARD_TYPE);
            ht.Add("@CARD_MID_NO", CARD_MID_NO);
            ht.Add("@CARD_SEQ", CARD_SEQ);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void deleteCardUPD_CTL(string CARD_TYPE, string CARD_MID_NO, string CARD_SEQ)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //寫log
            sb.Append(" update TB_D_M_CARD_UPD_CTL set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2DC040' ");
            sb.Append(" where CARD_NO = @CARD_NO; ");
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);

            sb.Append("delete from TB_D_M_CARD_UPD_CTL where CARD_NO = @CARD_NO; ");
            ht.Add("@CARD_NO", CARD_TYPE + CARD_MID_NO + CARD_SEQ);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void insertCardUPD_CTL(string CARD_TYPE, string CARD_MID_NO, string CARD_SEQ)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("insert into TB_D_M_CARD_UPD_CTL (CARD_NO,CARD_CHANGE_CD,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID) ");
            sb.Append(" values (@CARD_NO,'D',@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");
            ht.Add("@CARD_NO", CARD_TYPE + CARD_MID_NO + CARD_SEQ);
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

    internal void update_CardHandle(string PLANT_CD, string CARD_TYPE, string CARD_MID_NO, string CARD_SEQ, string card_handle)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("update TB_D_M_CARD set CARD_HANDLE = @CARD_HANDLE, ");
            sb.Append(" UPDATED_BY = @UPDATED_BY,UPDATED_DT = GETDATE(),FUNC_ID = @FUNC_ID,PLANT_CD = @PLANT_CD ");
            sb.Append(" from TB_D_M_CARD where CARD_TYPE = @CARD_TYPE and CARD_MID_NO = @CARD_MID_NO and CARD_SEQ = @CARD_SEQ");
            ht.Add("@CARD_HANDLE", card_handle);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            ht.Add("@PLANT_CD", PLANT_CD);

            ht.Add("@CARD_TYPE", CARD_TYPE);
            ht.Add("@CARD_MID_NO", CARD_MID_NO);
            ht.Add("@CARD_SEQ", CARD_SEQ);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getCARD_UPD_CTL()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select CARD_NO,CARD_CHANGE_CD,CLOCK_TYPE_A,CLOCK_TYPE_B from TB_D_M_CARD_UPD_CTL a,TB_D_M_CARD_TYPE b where substring(a.CARD_NO,1,2) = b.CARD_TYPE");
            ht.Add("@PERSON_ID", PERSON_ID);
            ht.Add("@CARD_TYPE", CARD_TYPE);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getCLOCK()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select CLOCK_NO from TB_D_M_CLOCK where CLOCK_TYPE in ('A','B') ");

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void add_CARD_UPD_NOW(string CARD_NO, string CARD_READER_NO, string CARD_CHANGE_CD)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("insert into TB_D_M_CARD_UPD_NOW (CARD_NO,CARD_READER_NO,CARD_CHANGE_CD,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append("values (@CARD_NO,@CARD_READER_NO,@CARD_CHANGE_CD,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");
            ht.Add("@CARD_NO", CARD_NO);
            ht.Add("@CARD_READER_NO", CARD_READER_NO);
            ht.Add("@CARD_CHANGE_CD", CARD_CHANGE_CD);
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

    internal void deleteCARD_UPD_CTL()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            //寫log
            sb.Append(" update TB_D_M_CARD_UPD_CTL set UPDATED_DT =getdate(),UPDATED_BY = @CURRENT_EMP,FUNC_ID = 'FB2DC040'; ");
            ht.Add("@CURRENT_EMP", SessionHandle.Current.emp_id);

            sb.Append("delete from TB_D_M_CARD_UPD_CTL;");

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }
    


    internal DataTable getExportToMake()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" Select  a.CARD_TYPE ,a.CARD_MID_NO ,a.CARD_SEQ ,a.CARD_NO ,a.PERSON_ID ,b.CARD_USED_CD ,NOTES 
                        ,e.SUB_DESC CARD_HANDLE_DESC 
                        ,case b.CARD_USED_CD when 'A' then c.EMP_NAME when 'B' then isnull(v.VENDOR_MEMBER_NAME,'') else isnull(b.CARD_TYPE_DESC,'') END CARD_NAME   
                        ,case b.CARD_USED_CD when 'A' then c.DEPT_NO when 'B' then isnull(v.VENDOR_NO,'') else '' END DEPT_NO   
                        ,case b.CARD_USED_CD when 'A' then IIF(c.DEPT_NAME_20='',DEPT_NAME_30,DEPT_NAME_20)    when 'B' then isnull(v.VENDOR_MEMBER_NAME,'') else '' END DEPT_NAME   
                        ,isnull(a.CARD_HANDLE,'') CARD_HANDLE 
                        ,isnull(a.CARD_HANDLE,'') + '-' + isnull(e.SUB_DESC,'') CARD_HANDLE_DESC 
                        ,isnull(c.PJOB_CD,'') PJOB_CD
                        ,isnull(c.PJOB_DESC,'')  PJOB_DESC
                        ,isnull(c.LEVEL_CD,'') 	LEVEL_CD
                        ,isnull(c.WS_CD,'')  WS_CD
                        ,isnull(c.EMP_CD,'') EMP_CD
                        ,isnull(d2.SUB_DESC,'') as TEMP_CARD_DESC 
                        ,isnull(h.ORDER_SEQ,0) ORDER_SEQ 
                        ,isnull(a.CARD_NAME,'') CARD_NAME_C    
                        ");
            sb.Append(" from TB_D_M_CARD a ");
            sb.Append(" left join TB_D_M_CARD_TYPE b on a.CARD_TYPE = b.CARD_TYPE");
            sb.Append(" left join VW_H_EMP_DATA c on a.PERSON_ID = c.EMP_ID");
            sb.Append(" left join TB_9_M_COMM_D e on a.CARD_HANDLE = e.SUB_CD and e.SYS_CD = 'DC' and e.MAIN_CD = 'CARD_HANDLE'");
            sb.Append(" left join TB_9_M_COMM_D d2 on a.TEMP_CARD_CD = d2.SUB_CD and d2.SYS_CD = 'DC' and d2.MAIN_CD = 'TEMP_CARD_CD'");
            sb.Append(" left join TB_D_M_VENDOR_D v on v.VENDOR_MEMBER_NO = a.PERSON_ID");
            sb.Append(" left join TB_H_M_LEVEL h on c.LEVEL_CD = h.LEVEL_CD and getdate() >= h.START_DT and getdate()<= h.END_DT");
            sb.Append(" where isNull(a.CARD_HANDLE,'') != '' ");
            sb.Append(" and a.PLANT_CD = @PLANT_CD ");

            ht.Add("@PLANT_CD", PLANT_CD);

            //sb.Append(" Select ");
            //sb.Append(" a.CARD_TYPE,a.CARD_MID_NO,a.CARD_SEQ,a.CARD_NO,a.PERSON_ID,CARD_USED_CD,");
            //sb.Append(" case CARD_USED_CD when 'A' then (select EMP_NAME from VW_H_EMP_DATA where VW_H_EMP_DATA.EMP_ID = a.PERSON_ID)");
            //sb.Append(" when 'B' then (select VENDOR_MEMBER_NAME from TB_D_M_VENDOR_D where TB_D_M_VENDOR_D.VENDOR_MEMBER_NO = a.PERSON_ID) ");
            //sb.Append(" else CARD_NAME END CARD_NAME,");
            //sb.Append(" case CARD_USED_CD when 'A' then (select DEPT_NO from VW_H_EMP_DATA where VW_H_EMP_DATA.EMP_ID = a.PERSON_ID)");
            //sb.Append(" when 'B' then (select VENDOR_NO from TB_D_M_VENDOR_D where TB_D_M_VENDOR_D.VENDOR_MEMBER_NO = a.PERSON_ID) ");
            //sb.Append(" else '' END DEPT_NO,");
            //sb.Append(" NOTES, e.SUB_DESC CARD_HANDLE_DESC,");
            //sb.Append(" case CARD_USED_CD when 'A' then (select DEPT_NAME from VW_H_EMP_DATA where VW_H_EMP_DATA.EMP_ID = a.PERSON_ID)");
            //sb.Append(" when 'B' then (select VENDOR_NAME from TB_D_M_VENDOR_D,TB_D_M_VENDOR_H where TB_D_M_VENDOR_D.VENDOR_MEMBER_NO = a.PERSON_ID and TB_D_M_VENDOR_D.VENDOR_NO = TB_D_M_VENDOR_H.VENDOR_NO) ");
            //sb.Append(" else '' END DEPT_NAME,");
            //sb.Append(" a.CARD_HANDLE ,a.CARD_HANDLE + '-' + e.SUB_DESC CARD_HANDLE_DESC,");
            //sb.Append(" case CARD_USED_CD when 'A' then (select PJOB_CD from VW_H_EMP_DATA where VW_H_EMP_DATA.EMP_ID = a.PERSON_ID)");
            //sb.Append(" else '' END PJOB_CD,");
            //sb.Append(" case CARD_USED_CD when 'A' then (select PJOB_DESC from VW_H_EMP_DATA where VW_H_EMP_DATA.EMP_ID = a.PERSON_ID)");
            //sb.Append(" else '' END PJOB_DESC,");
            //sb.Append(" case CARD_USED_CD when 'A' then (select LEVEL_CD from VW_H_EMP_DATA where VW_H_EMP_DATA.EMP_ID = a.PERSON_ID)");
            //sb.Append(" else '' END LEVEL_CD,");
            ////sb.Append(" case CARD_USED_CD when 'A' then (select distinct convert(varchar(3),ORDER_SEQ) from VW_H_EMP_DATA,TB_H_M_LEVEL where VW_H_EMP_DATA.LEVEL_CD = TB_H_M_LEVEL.LEVEL_CD and VW_H_EMP_DATA.EMP_ID = a.PERSON_ID)");
            ////sb.Append(" else '' END ORDER_SEQ,");
            //sb.Append(" case CARD_USED_CD when 'A' then (select top 1 convert(varchar(3),ORDER_SEQ) from VW_H_EMP_DATA,TB_H_M_LEVEL ");
            //sb.Append(" where VW_H_EMP_DATA.LEVEL_CD = TB_H_M_LEVEL.LEVEL_CD and VW_H_EMP_DATA.EMP_ID = TB_H_M_LEVEL.EMP_ID order by START_DT desc) ");
            //sb.Append(" else '' END ORDER_SEQ,  ");
            //sb.Append(" case CARD_USED_CD when 'A' then (select WS_CD from VW_H_EMP_DATA where VW_H_EMP_DATA.EMP_ID = a.PERSON_ID)");
            //sb.Append(" else '' END WS_CD,");
            //sb.Append(" (select EMP_CD from VW_H_EMP_DATA where VW_H_EMP_DATA.EMP_ID = a.PERSON_ID)EMP_CD");
            //sb.Append(" from TB_D_M_CARD a inner join TB_D_M_CARD_TYPE b on a.CARD_TYPE = b.CARD_TYPE");
            //sb.Append(" left join TB_9_M_COMM_D e on a.CARD_HANDLE = e.SUB_CD and e.SYS_CD = 'DC' and e.MAIN_CD = 'CARD_HANDLE'");
            //sb.Append(" where isNull(a.CARD_HANDLE,'') != '' ");

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void addCARD_HANDLE(string CARD_NO, string PERSON_ID, string CARD_NAME, string CARD_HANDLE)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("insert into TB_D_M_CARD_HANDLE (CARD_NO,HANDLE_DT,PERSON_ID,CARD_NAME,CARD_HANDLE,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append("values (@CARD_NO,GETDATE(),@PERSON_ID,@CARD_NAME,@CARD_HANDLE,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");
            ht.Add("@CARD_NO", CARD_NO);
            ht.Add("@PERSON_ID", PERSON_ID);
            ht.Add("@CARD_NAME", CARD_NAME);
            ht.Add("@CARD_HANDLE", CARD_HANDLE);
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
    public string getVendorName(string card_min_no)
    {
        try
        {
            string result = "";
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@" select H.VENDOR_NAME from TB_D_M_VENDOR_H H 
                         left join TB_D_M_VENDOR_D D on H.VENDOR_NO=D.VENDOR_NO
                         where D.VENDOR_MEMBER_NO=@VENDOR_MEMBER_NO
                        ");
            ht.Add("@VENDOR_MEMBER_NO", card_min_no);
            DataTable dt = dbConn.Query(sb, ht);

            if (dt.Rows.Count > 0)
            {
                result = (string)dt.Rows[0]["VENDOR_NAME"];
            }


            return result;
        }
        catch (Exception)
        {

            throw;
        }
    }
    internal DataTable getDupData()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select CARD_TYPE from TB_D_M_CARD where CARD_TYPE = @CARD_TYPE and CARD_MID_NO = @CARD_MID_NO and CARD_SEQ = @CARD_SEQ");
            ht.Add("@CARD_TYPE", CARD_TYPE);
            ht.Add("@CARD_MID_NO", CARD_MID_NO);
            ht.Add("@CARD_SEQ", CARD_SEQ);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
    public string PERSON_ID { get; set; }



    //參數檔 
    internal DataTable getTB_9_M_PARAMETER()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select (select order_seq   from VW_TB_H_M_LEVEL t where LEVEL_CD= ");
            sb.Append(" (select CODE_VAL1 from TB_9_M_PARAMETER a where SYS_CD = 'DC' and MAIN_CD = 'CARD_LEVEL_CD')) as CARD_LEVEL_CD, "); //變數.資格代號序號
            sb.Append(" ISNULL((select CODE_VAL1 from TB_9_M_PARAMETER where SYS_CD = 'DC' and MAIN_CD = 'CARD_TEMP_LABEL'),'')CARD_TEMP_LABEL ");//臨時卡顯示名稱
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void addCARD_DATA(string CARD_NO, string PERSON_ID, string CARD_NAME, string DEPT_NO,
        string CARD_HANDLE_DESC, string DEPT_NAME, string PJOB_DESC, string LEVEL_CD, string WS_CD,string pPLANT_CD)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("insert into B4C.dbo.CARD_DATA (CARD_NO,PERSON_ID,CARD_NAME,DEPT_NO,DEPT_NAME,PJOB_DESC,LEVEL_CD,NOTES,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID,PLANT_CD)");
            sb.Append("values (@CARD_NO,@PERSON_ID,@CARD_NAME,@DEPT_NO,@DEPT_NAME,@PJOB_DESC,@LEVEL_CD,@NOTES,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID,@PLANT_CD)");
            ht.Add("@CARD_NO", CARD_NO);
            ht.Add("@PERSON_ID", PERSON_ID);
            ht.Add("@CARD_NAME", CARD_NAME);
            ht.Add("@DEPT_NO", DEPT_NO);
            ht.Add("@DEPT_NAME", DEPT_NAME);
            ht.Add("@PJOB_DESC", PJOB_DESC);
            ht.Add("@LEVEL_CD", LEVEL_CD);
            ht.Add("@NOTES", CARD_HANDLE);
            ht.Add("@CREATED_BY", CREATED_BY);
            ht.Add("@UPDATED_BY", UPDATED_BY);
            ht.Add("@FUNC_ID", FUNC_ID);
            ht.Add("@PLANT_CD", pPLANT_CD);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public void addCARD_B4C(string tableName, string card_no, string card_mid_no, string col1, string col2, string col3, string col4)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("insert into [B4C].[dbo].[" + tableName + "] (CARD_NO,CARD_MID_NO,COL1,COL2,COL3,COL4)");
            sb.Append("values (@CARD_NO,@CARD_MID_NO,@COL1,@COL2,@COL3,@COL4)");
            ht.Add("@CARD_NO", card_no);
            ht.Add("@CARD_MID_NO", card_mid_no);
            ht.Add("@COL1", col1);
            ht.Add("@COL2", col2);
            ht.Add("@COL3", col3);
            ht.Add("@COL4", col4);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }
    //internal void addCARD_DATA(string label1, string label2, string label3, string label4, string card_no, string card_mid_no)
    //{
    //    try
    //    {
    //        StringBuilder sb = new StringBuilder();
    //        Hashtable ht = new Hashtable();
    //        sb.Append("insert into TB_D_M_CARD_PRINT (CARD_MID_NO,LABEL1,LABEL2,LABEL3,LABEL4,CARD_NO,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
    //        sb.Append("values (@CARD_MID_NO,@LABEL1,@LABEL2,@LABEL3,@LABEL4,@CARD_NO,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");
    //        ht.Add("@CARD_MID_NO", card_mid_no);
    //        ht.Add("@LABEL1", label1);
    //        ht.Add("@LABEL2", label2);
    //        ht.Add("@LABEL3", label3);
    //        ht.Add("@LABEL4", label4);
    //        ht.Add("@CARD_NO", card_no);
    //        ht.Add("@CREATED_BY", CREATED_BY);
    //        ht.Add("@UPDATED_BY", UPDATED_BY);
    //        ht.Add("@FUNC_ID", FUNC_ID);
    //        dbConn.ExecuteT(sb, ht, true);
    //    }
    //    catch (Exception)
    //    {

    //        throw;
    //    }
    //}

    internal void deleteCARD_DATA()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" delete from [B4C].[dbo].[CARD_DATA] ");
            sb.Append(" where PLANT_CD = @PLANT_CD ");
            ht.Add("@PLANT_CD", PLANT_CD);

            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal void deleteCARD_HANDLE()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            if (PLANT_CD.Equals("1"))
            {
                sb.Append("delete from [B4C].[dbo].[CARD_PRINT_MGR1]");
                sb.Append("delete from [B4C].[dbo].[CARD_PRINT_NOR1]");
                sb.Append("delete from [B4C].[dbo].[CARD_PRINT_VEN1]");
                sb.Append("delete from [B4C].[dbo].[CARD_PRINT_SPE1]");
                sb.Append("delete from [B4C].[dbo].[CARD_PRINT_COMM1]");
            }
            if (PLANT_CD.Equals("2"))
            {
                sb.Append("delete from [B4C].[dbo].[CARD_PRINT_MGR2]");
                sb.Append("delete from [B4C].[dbo].[CARD_PRINT_NOR2]");
                sb.Append("delete from [B4C].[dbo].[CARD_PRINT_VEN2]");
                sb.Append("delete from [B4C].[dbo].[CARD_PRINT_SPE2]");
                sb.Append("delete from [B4C].[dbo].[CARD_PRINT_COMM2]");
            }
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }


    //維護卡片資料檔(重新製卡及重新卡)
    public void SP_D_UPD_CARD_DATA_RE()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_D_UPD_CARD_DATA");

            //卡片處理 1:製新卡
            ht.Add("@pHandleCd", "I2");
            ht.Add("@pEmpId", CARD_MID_NO);

            if (CARD_TYPE == "00" || CARD_TYPE == "10")
            {
                ht.Add("@pCardUsedCd", "A");
            }
            else {
                ht.Add("@pCardUsedCd", CARD_TYPE);
            }

            ht.Add("@pStartDt", DateTime.Now.ToString("yyyy/MM/dd"));
            ht.Add("@pEndDt", "9999/12/31");
            ht.Add("@pUserID", SessionHandle.Current.emp_id);
            ht.Add("@pFuncID", "FB2DC040");
            dbConn.ExecuteSP(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }



}