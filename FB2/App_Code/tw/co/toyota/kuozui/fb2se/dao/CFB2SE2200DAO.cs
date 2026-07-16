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
/// CFB2SE220BO 的摘要描述
/// </summary>
public class CFB2SE2200DAO : BaseDAO
{
    public CFB2SE2200DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }
    public string RowNumber { get; set; }
    public string QDATAKEY { get; set; }
    public string EFFECT_YM { get; set; }
    public string EMP_ID { get; set; }
    public string EMP_ID_TA { get; set; }
    public string SALARY_EMAIL { get; set; }
    public string MAIL_DT { get; set; }
    public string TITLE { get; set; }
    public string MAIL_DESC { get; set; }
    public string vSendto { get; set; }
    public string SALARY_ID { get; set; }
    public string SALARY_YM { get; set; }
    public string SALARY_NAME { get; set; }
    public string CHG_AMT_A { get; set; }
    public string CHG_AMT_B { get; set; }
    public string PROCESS_STATUS { get; set; }
    public string START_DT_B { get; set; }
    public string START_DT_A { get; set; }
    public string END_DATE_B { get; set; }
    public string END_DATE_A { get; set; }
    public string CHG_STATUS { get; set; }
    public string APPROVE_DT { get; set; }
    public string APPROVE_BY { get; set; }
    public string REMARK { get; set; }
    public string APP_REMARK { get; set; }
    public string CREATED_BY { get; set; }
    public string CREATED_DT { get; set; }
    public string UPDATED_BY { get; set; }
    public string UPDATED_DT { get; set; }
    public string FUNC_ID { get; set; }

    public string IS_PLUS { get; set; }
    public string IS_TAX { get; set; }

    public DataTable getJPN_CD()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select SYS_CD , MAIN_CD , SUB_CD ,SUB_DESC from TB_9_M_COMM_D");
            sb.Append(" where SYS_CD='HB' and MAIN_CD='JPN_CD' and IS_VALID = 'Y' ");
            return dbConn.Query(sb);

        }
        catch
        {
            throw;
        }
    }
    public DataTable getSEND_DT(string EFFECT_YM)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select CONVERT(varchar,SEND_DT,111) SEND_DT");
            sb.Append(" from TB_S_M_MAIL_BAT_H a");
            sb.Append(" where a.EFFECT_YM=@EFFECT_YM");
            ht.Add("@EFFECT_YM", EFFECT_YM);
            return dbConn.Query(sb,ht);

        }
        catch
        {
            throw;
        }
    }
    public DataTable getTemp1(string txt_EFFECT_YM, string txt_EMP_ID)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select count(1) cnt from TB_S_M_SALARY_ADJ_H a");
            if (txt_EMP_ID != "")
            {
                sb.Append(" inner join TB_S_M_SALARY_ADJ_D b on a.EFFECT_YM = b.EFFECT_YM");
            }

            sb.Append(" where a.EFFECT_YM=@EFFECT_YM ");

            if (txt_EMP_ID != "")
            {
                sb.Append(" and b.EMP_ID= @EMP_ID");
            }
            
            ht.Add("@EFFECT_YM", txt_EFFECT_YM);
            ht.Add("@EMP_ID", txt_EMP_ID);

            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    public DataTable getNot_ADJ(string txt_EFFECT_YM)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select count(1) cnt from TB_S_M_SALARY_ADJ_H a");
            sb.Append(" where isnull(a.MEM_CREATE_BY,'')=''");
            sb.Append(" and a.EFFECT_YM=@EFFECT_YM");
            ht.Add("@EFFECT_YM", txt_EFFECT_YM);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }

    public DataTable getTemp2(string M_EMP_ID)
    {
        try
        { 
            //取寄信人的MAIL
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select a.SALARY_EMAIL from TB_H_M_EMP a");
            sb.Append(" WHERE a.EMP_ID=@EMP_ID");
            ht.Add("@EMP_ID", M_EMP_ID);
            return dbConn.Query(sb, ht);
        }
        catch
        {
            throw;
        }
    }
    public DataTable getTempCHK1(string EFFECT_YM, string EMP_ID)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" select t.EMP_ID,t2.EMP_NAME");
            sb.Append(" from TB_S_M_SALARY_ADJ_D t ");
            sb.Append(" left join TB_H_M_EMP t2 on t.EMP_ID=t2.EMP_ID");
            sb.Append(" where t.EFFECT_YM=@EFFECT_YM  and isnull(t2.SALARY_EMAIL,'')='' and isnull(t2.JPN_CD,'')=''");
            if (EMP_ID != "")
            {
                sb.Append(" and t.EMP_ID=@EMP_ID");
                ht.Add("@EMP_ID", EMP_ID);
            }
            ht.Add("@EFFECT_YM", EFFECT_YM);


            return dbConn.Query(sb, ht);

        }
        catch
        {
            throw;
        }
    }
    public void addData2()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("INSERT INTO TB_S_M_MAIL_BAT_D (SEND_DT,EMP_ID,EMAIL,MAIL_YN,CREATED_BY,CREATED_DT,UPDATED_BY,UPDATED_DT,FUNC_ID,QRY_EMP_ID)");
            sb.Append(" select @SEND_DT,t.EMP_ID,t2.SALARY_EMAIL,'N',@CREATED_BY,GETDATE(),@UPDATED_BY,GETDATE(),'FB2SE220',@EMP_ID from  TB_S_M_SALARY_ADJ_D t ");
            sb.Append(" left join TB_H_M_EMP t2 on t.EMP_ID=t2.EMP_ID ");
            sb.Append(" where t.EFFECT_YM=@EFFECT_YM  and isnull(t2.SALARY_EMAIL,'')<>'' and isnull(t2.JPN_CD,'')='' ");

            if (EMP_ID != "")
            {
                sb.Append(" and t.EMP_ID= @EMP_ID");
                
            }
            ht.Add("@EMP_ID", EMP_ID);
            ht.Add("@SEND_DT", MAIL_DT);
            ht.Add("@CREATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@UPDATED_BY", SessionHandle.Current.emp_id);
            ht.Add("@EFFECT_YM", EFFECT_YM);
            dbConn.ExecuteT(sb, ht, true);
                        
        }
        catch
        {
            throw;
        }
    }
    
        internal DataTable getExistData()
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                Hashtable ht = new Hashtable();
                sb.Append("Select * from TB_S_M_MAIL_BAT_H where SEND_DT = @SEND_DT");
                ht.Add("@SEND_DT", MAIL_DT);

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
                char[] ch1 = new Char[] { '|' };
                string[] split1 = deleteitem.Split(ch1);
                string EMP_ID_TA2 = split1[0].ToString();
                string SALARY_EMAIL2 = split1[1].ToString();
                string MAIL_DT2 = split1[2].ToString();
                sb.Append("Select * from TB_S_M_MAIL_BAT_D where SEND_DT = @SEND_DT and EMP_ID = @EMP_ID");
                ht.Add("@SEND_DT", MAIL_DT2);
                ht.Add("@EMP_ID", EMP_ID_TA2);
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
            sb.Append("INSERT INTO TB_S_M_MAIL_BAT_H (SEND_DT,MAIL_TITLE,MAIL_DESC,EFFECT_YM,QRY_EMP_ID,SENDTO_MAIL,CREATED_BY,CREATED_DT,FUNC_ID)");
            sb.Append(" Values (@SEND_DT,@MAIL_TITLE,@MAIL_DESC,@EFFECT_YM,@QRY_EMP_ID,@SENDTO_MAIL,@CREATED_BY,GETDATE(),@FUNC_ID)");
            ht.Add("@SEND_DT", MAIL_DT);
            ht.Add("@MAIL_TITLE", TITLE);
            ht.Add("@MAIL_DESC", MAIL_DESC);
            ht.Add("@EFFECT_YM", EFFECT_YM);
            ht.Add("@QRY_EMP_ID", EMP_ID);
            ht.Add("@SENDTO_MAIL", vSendto);
            ht.Add("@CREATED_BY", CREATED_BY);
            ht.Add("@FUNC_ID", "FB2SE220");
           
            dbConn.ExecuteT(sb, ht, true);

            
        }
        catch (Exception)
        {
            throw;
        }
    }
  
    public void deleteData()
    {
        StringBuilder sb = new StringBuilder();
        Hashtable ht = new Hashtable();

        sb.Append(" delete from TB_S_M_MAIL_BAT_H where EFFECT_YM=@EFFECT_YM AND SEND_DT =@SEND_DT  ");
        sb.Append(" and QRY_EMP_ID= @EMP_ID");
        
        //if (EMP_ID!="")
        //{ 
        //    sb.Append(" and QRY_EMP_ID= @EMP_ID");
        //    ht.Add("@EMP_ID", EMP_ID);
        //}
        ht.Add("@EMP_ID", EMP_ID);
        ht.Add("@SEND_DT", MAIL_DT);
        ht.Add("@EFFECT_YM", EFFECT_YM);
        dbConn.ExecuteT(sb, ht, true);
        sb.Clear();
        ht.Clear();

        sb.Append("  delete from TB_S_M_MAIL_BAT_D where SEND_DT =@SEND_DT  ");
        sb.Append(" and QRY_EMP_ID = @EMP_ID");
        //if (EMP_ID != "")
        //{
        //    //sb.Append(" and EMP_ID = @EMP_ID");
        //    sb.Append(" and QRY_EMP_ID = @EMP_ID");
        //    ht.Add("@EMP_ID", EMP_ID);
        //}
        ht.Add("@EMP_ID", EMP_ID);
        ht.Add("@SEND_DT",MAIL_DT);
        dbConn.ExecuteT(sb, ht, true);        
    }

}