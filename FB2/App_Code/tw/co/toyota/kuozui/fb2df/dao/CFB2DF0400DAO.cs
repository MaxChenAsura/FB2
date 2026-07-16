using FB2.tw.co.toyota.kuozui.dao;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

/// <summary>
/// CFB2DF0400DAO 的摘要描述
/// </summary>
public class CFB2DF0400DAO : BaseDAO
{
    //畫面
    public string MANAGER_YM { get; set; }
  
    //TEMP PARA
    public string EMP_ID { get; set; }
    public string EMP_NAME { get; set; }
    public string EMP_CD { get; set; }
    public string DEPT_NO { get; set; }
    public string CLASS_NAME { get; set; }
    public string POTO { get; set; }
    public string CARD_NO { get; set; }
    public string ROOM_NO { get; set; }
    public string START_DT { get; set; }
    public string END_DT { get; set; }
    public string CAR { get; set; }
    public string MOTOR { get; set; }
    public string CAR_NO { get; set; }
    public string MOTOR_NO { get; set; }
    public string BIRTH_DT { get; set; }
    public string LEAVE_DT { get; set; }

	public CFB2DF0400DAO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    public DataTable checkData()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(@" select count(EMP_ID) count,a.EMP_ID from TB_D_M_ACCOM_MAIN a
                         left join (
                         select x.CARD_TYPE,PERSON_ID,CARD_MID_NO,CARD_NO,START_DT,END_DT from TB_D_M_CARD x 
		                           left join TB_D_M_CARD_TYPE y on x.CARD_TYPE = y.CARD_TYPE where y.CARD_USED_CD = 'A' and @MANAGER_YM >= x.START_DT 
		                           and (@MANAGER_YM <= x.END_DT or isnull(x.END_DT,'')='')
                         ) b on a.EMP_ID = b.PERSON_ID
                         group by b.PERSON_ID,a.EMP_ID ");

            ht.Add("@MANAGER_YM", MANAGER_YM);

            DataTable dt = dbConn.Query(sb, ht);


            return dt;
        }
        catch
        {
            throw;
        }
    }

    public void insertRECORD()
    {        
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("insert into TB_D_S_ACCOM_TEMP_RECORD");
            sb.Append(" (EMP_ID,EMP_NAME,EMP_CD,DEPT_NO,CLASS_NAME,POTO,CARD_NO,ROOM_NO,");
            sb.Append(" START_DT,END_DT,CAR,MOTOR,CAR_NO,MOTOR_NO)");
            sb.Append("Select a.EMP_ID,a.EMP_NAME,b.EMP_CD,a.DEPT_NO ");
            //sb.Append(",b.DEPT_FULL_NAME ");
            sb.Append("    ,case when isnull(b.DEPT_NAME_40,'')='' then b.DEPT_NAME else b.DEPT_NAME_40 end DEPT_NAME_40,");
            //sb.Append(" b.DEPT_FULL_NAME,");
            sb.Append(" (select substring(b.WORK_SHIFT_CD,2,1)),c.CARD_NO,a.ROOM_NO,");
            sb.Append(" a.START_DT,a.END_DT,(select CAR = CASE WHEN a.CAR_NO <> '' then 'Y' ELSE '' END),");
            sb.Append(" (select MOTOR = CASE WHEN a.MOTOR_NO <> '' then 'Y' ELSE '' END),a.CAR_NO,a.MOTOR_NO");
            sb.Append(" From TB_D_M_ACCOM_MAIN a");
            sb.Append(" left join VW_H_EMP_DATA b");
            sb.Append(" on a.EMP_ID = b.EMP_ID");
            sb.Append(" left join (select x.CARD_TYPE,PERSON_ID,CARD_MID_NO,CARD_NO,START_DT,END_DT from TB_D_M_CARD x");
            sb.Append(" left join TB_D_M_CARD_TYPE y on x.CARD_TYPE = y.CARD_TYPE where y.CARD_USED_CD = 'A'");
            sb.Append(" and @MANAGER_YM >= x.START_DT and (@MANAGER_YM <= x.END_DT or isnull(x.END_DT,'')='') ) c");
            sb.Append(" on a.EMP_ID = c.PERSON_ID");
            sb.Append(" where a.START_DT <= @MANAGER_YM and (a.END_DT >= @MANAGER_YM");
            sb.Append(" or isnull(a.END_DT,'')  ='' )");

            ht.Add("@MANAGER_YM", MANAGER_YM);

            dbConn.Execute(sb, ht, true);
            
        }
        catch
        {
            throw;
        }
    }

    public void deleteRECORD()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("delete from TB_D_S_ACCOM_TEMP_RECORD");
           

            dbConn.Execute(sb, ht, true);

        }
        catch
        {
            throw;
        }
    }

    public DataTable selectData()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select a.EMP_ID,a.EMP_NAME,a.EMP_CD,a.DEPT_NO,d.DEPT_FULL_NAME as CLASS_NAME,a.POTO,a.CARD_NO,a.ROOM_NO");
            sb.Append(", CONVERT(VARCHAR(8),isnull(a.START_DT,'99991231'),112) as START_DT_AD ");
            sb.Append(", CONVERT(VARCHAR(8),isnull(a.END_DT,'99991231'),112) as END_DT_AD ");
            sb.Append(", iif(d.LEAVE_DT is null,'', CONVERT(VARCHAR(8),d.LEAVE_DT,112) )as LEAVE_DT_AD ");
            sb.Append(", CONVERT(VARCHAR(8),d.BIRTH_DT,112) as BIRTH_DT_AD ");
            sb.Append(",case when isnull(a.START_DT,'') ='' then '9999-12-31' else  CONVERT(char(10),a.START_DT, 120) end START_DT");
            sb.Append(",case when isnull(a.END_DT,'') ='' then '9999-12-31' else  CONVERT(char(10),a.END_DT, 120) end END_DT ");
            sb.Append(", CONVERT(char(10), d.LEAVE_DT, 111) LEAVE_DT ");
            sb.Append(", a.CAR,a.MOTOR,a.CAR_NO,a.MOTOR_NO ");
            sb.Append(" ,CONVERT(char(10), d.BIRTH_DT, 111) BIRTH_DT,b.ACCOM_BUILD_CD");
            sb.Append(" from TB_D_S_ACCOM_TEMP_RECORD a");
            sb.Append(" left join VW_H_EMP_DATA d on a.EMP_ID = d.EMP_ID");
            sb.Append(" left join TB_D_M_ACCOM_MAIN b on a.EMP_ID = b.EMP_ID");
            

           
            DataTable dt = dbConn.Query(sb, ht);


            return dt;
        }
        catch
        {
            throw;
        }
    }

    public DataTable selectMainData()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select * from TB_D_M_ACCOM_MAIN");
            sb.Append(" where START_DT <= @MANAGER_YM and (END_DT >= @MANAGER_YM");
            sb.Append(" or isnull(END_DT,'')  ='' )");

            ht.Add("@MANAGER_YM", MANAGER_YM);



            DataTable dt = dbConn.Query(sb, ht);


            return dt;
        }
        catch
        {
            throw;
        }
    }

}