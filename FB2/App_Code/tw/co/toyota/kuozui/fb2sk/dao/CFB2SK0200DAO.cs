using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;

/// <summary>
/// CFB2SK0200DAO 的摘要描述
/// </summary>
public class CFB2SK0200DAO : BaseDAO
{
    public string EMP_ID { get; set; }
    public string DEPT_NAME_20 { get; set; }
    public string DEPT_NAME_30 { get; set; }
    public string DEPT_NAME_40 { get; set; }
    public string DEPT_NAME_50 { get; set; }
    public string DEPT_NAME_60 { get; set; }
    public string DEPT_NAME_70 { get; set; }
    public string PLANT_CD { get; set; }
    public string LINE_CD { get; set; }
    public string DEPT_NO { get; set; }
    public string WS_CD { get; set; }
    public string EMP_NAME { get; set; }
    public string JPN_CD { get; set; }
    public string PJOB_CD { get; set; }
    public string PJOB_DESC { get; set; }
    public string JOIN_DT { get; set; }
    public string BIRTH_DT { get; set; }
    public string LICENSE_ID { get; set; }
    public string SCHOOL_NAME { get; set; }
    public string EMP_UPDATETIMED_DT { get; set; }
    public string EMP_CD { get; set; }
    public string EMP_CHG_CD { get; set; }
    public string CONTACT_TEL { get; set; }
    public string REGISTER_ADDR { get; set; }
    public string CONTACT_ADDR { get; set; }
    public string SALARY_ACCOUNT_NO { get; set; }
    public string LEVEL_CD { get; set; }
    public string GRADE_CD { get; set; }
    public string END_DT { get; set; }
    public string GRADE { get; set; }
    public string SHIFT_CD { get; set; }
    public string EMP_STATUS { get; set; }


    public CFB2SK0200DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    //刪除福利會用人事主檔
    public void Delete_TB_S_MUTUAL_EMP()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Delete From TB_S_M_MUTUAL_EMP ");
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }

    //新增福利會用人事主檔
    public void insert_TB_S_MUTUAL_EMP()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("INSERT INTO TB_S_M_MUTUAL_EMP (EMP_ID,DEPT_NAME_20,DEPT_NAME_30,DEPT_NAME_40,DEPT_NAME_50,DEPT_NAME_60,DEPT_NAME_70,");
            sb.Append(" PLANT_CD,LINE_CD,DEPT_NO,WS_CD,EMP_NAME,PJOB_DESC,JOIN_DT,BIRTH_DT,LICENSE_ID,SCHOOL_NAME,EMP_UPDATETIMED_DT,");
            sb.Append(" EMP_CD,EMP_CHG_CD,CONTACT_TEL,REGISTER_ADDR,CONTACT_ADDR,SALARY_ACCOUNT_NO,LEVEL_CD,GRADE_CD,END_DT,GRADE,SHIFT_CD,");
            sb.Append(" CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(@" select 
                         a.EMP_ID
                        ,IIF(a.EMP_STATUS='01',A.DEPT_NAME_20,C.DEPT_NAME_20 ) DEPT_NAME_20
                        ,IIF(a.EMP_STATUS='01',A.DEPT_NAME_30,C.DEPT_NAME_30 ) DEPT_NAME_30
                        ,IIF(a.EMP_STATUS='01',A.DEPT_NAME_40,C.DEPT_NAME_40 ) DEPT_NAME_40
                        ,IIF(a.EMP_STATUS='01',A.DEPT_NAME_50,C.DEPT_NAME_50 ) DEPT_NAME_50
                        ,IIF(a.EMP_STATUS='01',A.DEPT_NAME_60,C.DEPT_NAME_60 ) DEPT_NAME_60
                        ,IIF(a.EMP_STATUS='01',A.DEPT_NAME_70,C.DEPT_NAME_70 ) DEPT_NAME_70 
                        ,a.PLANT_CD,right(a.WORK_SHIFT_CD,1)
                        ,a.DEPT_NO,a.WS_CD,a.EMP_NAME,a.PJOB_DESC,a.JOIN_DT,a.BIRTH_DT,a.LICENSE_ID 
                        ,( select SCHOOL_NAME=case when a.PJOB_CD='PJ50' then  a.SCHOOL_NAME else '' end )  
                        ,(select UPDATED_DT from TB_H_M_EMP WHERE  EMP_ID= a.EMP_ID) 
                        ,(select  EMP_CD =   case when a.EMP_CD='1' and a.PJOB_CD='PJ50' then '2'  	 when a.EMP_CD='1' and a.JPN_CD<>'' then  '4'  	 when a.EMP_CD='1' then  '1'  	 when a.EMP_CD='2' then '3'  end  )  
                        ,(select EMP_CHG_CD =  
                        case when a.EMP_CHG_CD='21' then '3'  
                        when a.EMP_CHG_CD='31' then '2'  
                        when a.EMP_CHG_CD='91' or a.EMP_CHG_CD='92' or a.EMP_CHG_CD='93' then '1'  
                        when a.EMP_CHG_CD='11' or a.EMP_CHG_CD='13' or a.EMP_CHG_CD='14' then ''  
                        when a.EMP_CHG_CD='12' then '5'  end  ) 
                        ,a.CONTACT_TEL
                        ,a.REGISTER_ADDR
                        ,a.CONTACT_ADDR
                        ,a.SALARY_ACCOUNT_NO
                        ,a.LEVEL_CD
                        ,a.GRADE_CD 
                        ,(select  END_DT =   
                        case when a.EMP_STATUS='01' then null  
                        else     ( select UPDATED_DT from TB_H_M_EMP WHERE  EMP_ID= a.EMP_ID )   end  )  
                        ,@GRADE,@SHIFT_CD,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID
                        from VW_H_EMP_DATA a 
                        left join (
	                        select A.* from TB_H_R_EMP_DATA_MONTH A
	                        inner join (
	                        select max(YM) maxYM,EMP_ID from TB_H_R_EMP_DATA_MONTH group by EMP_ID
	                        ) B  on A.EMP_ID = B.EMP_ID and A.YM=B.maxYM
                        ) C on a.EMP_ID=C.EMP_ID
                        where a.EMP_CD in ('1','2')  
                        and a.EMP_STATUS<>'04'  
                        and ( a.LEAVE_DT>='2013/12/01' or isnull(a.LEAVE_DT,'')=''  ) 
                        ");

/*
            sb.Append(@" select EMP_ID,DEPT_NAME_20,DEPT_NAME_30,DEPT_NAME_40,DEPT_NAME_50,DEPT_NAME_60,DEPT_NAME_70 " +
                       " ,PLANT_CD,right(a.WORK_SHIFT_CD,1),DEPT_NO,WS_CD,EMP_NAME,PJOB_DESC,JOIN_DT,BIRTH_DT,LICENSE_ID" +
                       " ,( select SCHOOL_NAME=case when PJOB_CD='PJ50' then  SCHOOL_NAME else '' end ) " +
                       " ,(select UPDATED_DT from TB_H_M_EMP WHERE  EMP_ID= a.EMP_ID) " +
                       ",(select  EMP_CD =  " +
                       " case when EMP_CD='1' and PJOB_CD='PJ50' then '2' " +
                       " 	 when EMP_CD='1' and JPN_CD<>'' then  '4' " +
                       " 	 when EMP_CD='1' then  '1' " +
                       " 	 when EMP_CD='2' then '3' " +
                       " end " +
                       " ) " +
                       " ,(select EMP_CHG_CD = " +
                       " case when EMP_CHG_CD='21' then '3' " +
                       " when EMP_CHG_CD='31' then '2' " +
                       " when EMP_CHG_CD='91' or EMP_CHG_CD='92' or EMP_CHG_CD='93' then '1' " +
                       " when EMP_CHG_CD='11' or EMP_CHG_CD='13' or EMP_CHG_CD='14' then '' " +
                       " when EMP_CHG_CD='12' then '5' " +
                       " end " +
                       " ) " +
                       ",CONTACT_TEL,REGISTER_ADDR,CONTACT_ADDR,SALARY_ACCOUNT_NO,LEVEL_CD,GRADE_CD " +
                       ",(select  END_DT =  " +
                       " case when EMP_STATUS='01' then null " +
                       " else  " +
                       "   ( select UPDATED_DT from TB_H_M_EMP WHERE  EMP_ID= a.EMP_ID )  " +
                       " end " +
                       " ) " +
                       ", @GRADE,@SHIFT_CD,@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID"
                );
            sb.Append(" from VW_H_EMP_DATA a");
            sb.Append(" where EMP_CD in ('1','2') ");
            sb.Append(" and EMP_STATUS<>'04' ");
            sb.Append(" and ( LEAVE_DT>='2013/12/01' or isnull(LEAVE_DT,'')=''  ) ");
            */

            //sb.Append(" and EMP_ID='10105' ");
            
            ht.Add("@GRADE", "");
            ht.Add("@SHIFT_CD", "");
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SK0200");


            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //人事異動檔
    public DataTable getCHANGE_CODE_B07(string emp_id)
    {
        try
        {
            StringBuilder sb2 = new StringBuilder();
            Hashtable ht2 = new Hashtable();
            sb2.Append(" select count(*) resultCount from TB_H_M_EMP_HR_CHANGE_H	");
            sb2.Append(" where EMP_ID=@EMP_ID and HR_CHG_CD='B07' ");
            sb2.Append(" and is_END='N'  							   ");
            ht2.Add("@EMP_ID", emp_id);
            return dbConn.Query(sb2, ht2);

        }
        catch (Exception)
        {

            throw;
        }
    }




    //讀取員工人事資料
    public DataTable VW_H_EMP_DATA()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select a.*,");
            sb.Append(" (select UPDATED_DT  from TB_H_M_EMP where EMP_ID=a.EMP_ID) UPDATED_DT,");
            sb.Append(" (select SHIFT_CD  from TB_D_M_EMP_DAY_DUTY");
            sb.Append(" where EMP_ID=a.EMP_ID");
            sb.Append(" and CALENDAR_DT =");
            sb.Append(" (select max(CALENDAR_DT)");
            sb.Append(" from TB_D_M_EMP_DAY_DUTY");
            sb.Append(" where EMP_ID = a.EMP_ID)) SHIFT_CD");
            sb.Append(" from VW_H_EMP_DATA a");
            sb.Append(" where EMP_CD in ('1','2')");

            return dbConn.Query(sb, ht);

        }
        catch (Exception)
        {

            throw;
        }
    }



    //新增福利會用人事主檔
    public void Add_TB_S_MUTUAL_EMP()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("INSERT INTO TB_S_M_MUTUAL_EMP (EMP_ID,DEPT_NAME_20,DEPT_NAME_30,DEPT_NAME_40,DEPT_NAME_50,DEPT_NAME_60,DEPT_NAME_70,");
            sb.Append(" PLANT_CD,LINE_CD,DEPT_NO,WS_CD,EMP_NAME,PJOB_DESC,JOIN_DT,BIRTH_DT,LICENSE_ID,SCHOOL_NAME,EMP_UPDATETIMED_DT,");
            sb.Append(" EMP_CD,EMP_CHG_CD,CONTACT_TEL,REGISTER_ADDR,CONTACT_ADDR,SALARY_ACCOUNT_NO,LEVEL_CD,GRADE_CD,END_DT,GRADE,SHIFT_CD,");
            sb.Append(" CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID)");
            sb.Append(" Values (@EMP_ID,@DEPT_NAME_20,@DEPT_NAME_30,@DEPT_NAME_40,@DEPT_NAME_50,@DEPT_NAME_60,@DEPT_NAME_70,");
            sb.Append(" @PLANT_CD,@LINE_CD,@DEPT_NO,@WS_CD,@EMP_NAME,@PJOB_DESC,@JOIN_DT,@BIRTH_DT,@LICENSE_ID,@SCHOOL_NAME,@EMP_UPDATETIMED_DT,");
            sb.Append(" @EMP_CD,@EMP_CHG_CD,@CONTACT_TEL,@REGISTER_ADDR,@CONTACT_ADDR,@SALARY_ACCOUNT_NO,@LEVEL_CD,@GRADE_CD,@END_DT,@GRADE,@SHIFT_CD,");
            sb.Append(" @CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),@FUNC_ID)");
            ht.Clear();
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@DEPT_NAME_20", DEPT_NAME_20);
            ht.Add("@DEPT_NAME_30", DEPT_NAME_30);
            ht.Add("@DEPT_NAME_40", DEPT_NAME_40);
            ht.Add("@DEPT_NAME_50", DEPT_NAME_50);
            ht.Add("@DEPT_NAME_60", DEPT_NAME_60);
            ht.Add("@DEPT_NAME_70", DEPT_NAME_70);
            ht.Add("@PLANT_CD", PLANT_CD);
            ht.Add("@LINE_CD", LINE_CD);
            ht.Add("@DEPT_NO", DEPT_NO);
            ht.Add("@WS_CD", WS_CD);
            ht.Add("@EMP_NAME", EMP_NAME);
            ht.Add("@PJOB_DESC", PJOB_DESC);
            if (JOIN_DT == "")
                ht.Add("@JOIN_DT", DBNull.Value);
            else
                ht.Add("@JOIN_DT", Convert.ToDateTime(JOIN_DT).ToString("yyyyMMdd"));
            if (BIRTH_DT == "")
                ht.Add("@BIRTH_DT", DBNull.Value);
            else
                ht.Add("@BIRTH_DT", Convert.ToDateTime(BIRTH_DT).ToString("yyyyMMdd"));
            ht.Add("@LICENSE_ID", LICENSE_ID);
            if (PJOB_DESC.IndexOf("建教生") != -1)
                ht.Add("@SCHOOL_NAME", SCHOOL_NAME);
            else
                ht.Add("@SCHOOL_NAME", "");
            if (EMP_UPDATETIMED_DT == "")
                ht.Add("@EMP_UPDATETIMED_DT", DBNull.Value);
            else
                ht.Add("@EMP_UPDATETIMED_DT", Convert.ToDateTime(EMP_UPDATETIMED_DT).ToString("yyyyMMdd"));

            if (EMP_CD == "1") //一般員工
            {
                if (PJOB_CD == "PJ50")
                    ht.Add("@EMP_CD", "2"); //建教生
                else if (JPN_CD != "")
                    ht.Add("@EMP_CD", "4"); //日本人
                else
                    ht.Add("@EMP_CD", "1");
            }
            if (EMP_CD == "2") //期間員工
            {
                ht.Add("@EMP_CD", "3"); //契約工
            }

            string emp_chg_cd_tmp = "";
            //在職區分為 21(留停)時，在職區分為「3」(留職)
            if (EMP_CHG_CD == "21")
                emp_chg_cd_tmp = "3";
            //在職區分為 31(返校)時，在職區分為「2」(在校)		
            if (EMP_CHG_CD == "31")
                emp_chg_cd_tmp = "2";
            //在職區分為 91(離職)、92(退休)、93(非自願離職)時，在職區分為「1」(離職)
            if (EMP_CHG_CD == "91" || EMP_CHG_CD == "92" || EMP_CHG_CD == "93")
                emp_chg_cd_tmp = "1";
            //在職區分為 11(在職)、13(返廠)、14(應受援)時，在職區分為「空白」(在職)		
            if (EMP_CHG_CD == "11" || EMP_CHG_CD == "13" || EMP_CHG_CD == "14")
                emp_chg_cd_tmp = "";
            //在職區分為 12(外調)時，在職區分為「5」(國外)		
            if (EMP_CHG_CD == "12")
                emp_chg_cd_tmp = "5";
            //且若 人事異動主檔 為B07時且狀態尚未結束，則在職區分為「空白」(在職)																																	
            int t = 0;
            StringBuilder sb2 = new StringBuilder();
            Hashtable ht2 = new Hashtable();
            sb2.Append(" select count(*) resultCount from TB_H_M_EMP_HR_CHANGE_H	");
            sb2.Append(" where EMP_ID=@EMP_ID and HR_CHG_CD='B07' ");
            sb2.Append(" and is_END='N'  							   ");
            ht2.Add("@EMP_ID", EMP_ID);
            DataTable dt = dbConn.Query(sb2, ht2);
            if (dt.Rows.Count > 0)
            {
                t = (int)dt.Rows[0]["resultCount"];
            }
            if (t > 0)
                emp_chg_cd_tmp = "";

            ht.Add("@EMP_CHG_CD", emp_chg_cd_tmp);
            ht.Add("@CONTACT_TEL", CONTACT_TEL);
            ht.Add("@REGISTER_ADDR", REGISTER_ADDR);
            ht.Add("@CONTACT_ADDR", CONTACT_ADDR);
            ht.Add("@SALARY_ACCOUNT_NO", SALARY_ACCOUNT_NO);
            ht.Add("@LEVEL_CD", LEVEL_CD);
            ht.Add("@GRADE_CD", GRADE_CD);
            if ((EMP_STATUS == "99" || EMP_STATUS == "03" || EMP_STATUS == "02") && EMP_UPDATETIMED_DT != "")
            {

                ht.Add("@END_DT", Convert.ToDateTime(EMP_UPDATETIMED_DT).ToString("yyyyMMdd"));
            }
            else
            {
                ht.Add("@END_DT", DBNull.Value);
            }
            ht.Add("@GRADE", "");
            ht.Add("@SHIFT_CD", "");
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@FUNC_ID", "FB2SK0200");
            dbConn.ExecuteT(sb, ht, true);
        }
        catch (Exception)
        {
            throw;
        }
    }
    //讀取福利會用人事主檔
    public DataTable TB_S_MUTUAL_EMP()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select * from TB_S_M_MUTUAL_EMP order by EMP_ID ASC ");
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {
            throw;
        }
    }

   
}