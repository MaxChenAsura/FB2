using FB2.tw.co.toyota.kuozui.dao;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;

/// <summary>
/// CFB2DE0400DAO 的摘要描述
/// </summary>
public class CFB2DE0400DAO : BaseDAO
{
    public string RESTAURANT_CD { get; set; }
    public string DOCUMENT_CD { get; set; }
    public string MANAGER_YM_S { get; set; }
    public string MANAGER_YM_E { get; set; }
    public string MANAGER_YM { get; set; }
    public string BF_AMOUNT { get; set; }
    public string DN_AMOUNT { get; set; }
    public string COMPANY_CD { get; set; }
    public string BR_PEOPLE { get; set; }
    public string PLANT_CD { get; set; }
    public string MANAGER_UNIT { get; set; }
    
	public CFB2DE0400DAO()
	{
		//
		// TODO: 在這裡新增建構函式邏輯
		//
	}

    public DataTable getCommCode(string SYS_CD, string MAIN_CD, string CODE_VAL1, string CODE_VAL2)
    {

        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select sub_cd ,sub_cd+'-'+sub_desc sub_desc From TB_9_M_COMM_D Where main_cd = @MAIN_CD");
            sb.Append(" and sys_cd = @SYS_CD and IS_VALID = 'Y'");
            ht.Add("@MAIN_CD", MAIN_CD);
            ht.Add("@SYS_CD", SYS_CD);
            if (CODE_VAL1 != "")
            {
                sb.Append(" and CODE_VAL1 = @CODE_VAL1");
                ht.Add("@CODE_VAL1", CODE_VAL1);
            }
            if (CODE_VAL2 != "")
            {
                sb.Append(" and CODE_VAL2 = @CODE_VAL2");
                ht.Add("@CODE_VAL2", CODE_VAL2);
            }

            return dbConn.Query(sb, ht);

        }
        catch
        {
            throw;
        }
    }

    public DataTable searchDateResult()
    {

        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select MANAGER_DT,RESTAURANT_CD,MANAGER_UNIT,shiftA,shiftA_error,shiftB,shiftC,edu,nightErr,SUM(BR_PEOPLE)BR_PEOPLE");
            sb.Append(",MONTH_MD_PEOPLE,convert(int,MONTH_MD_AMOUNT) MONTH_MD_AMOUNT,ERROR_MD_PEOPLE,convert(int,ERROR_MD_AMOUNT)ERROR_MD_AMOUNT from(");
            sb.Append(" Select distinct( CONVERT(char(10),a.MANAGER_DT, 120)) MANAGER_DT,a.RESTAURANT_CD+'-'+b.SUB_DESC RESTAURANT_CD,a.MANAGER_UNIT,");
            sb.Append(" (CASE WHEN c.shiftA IS NULL THEN 0 ELSE c.shiftA END) shiftA,(CASE WHEN d.shiftA_error IS NULL THEN 0 ELSE d.shiftA_error END) shiftA_error,");
            sb.Append(" (CASE WHEN e.shiftB IS NULL THEN 0 ELSE e.shiftB END) shiftB,(CASE WHEN f.shiftC IS NULL THEN 0 ELSE f.shiftC END) shiftC,");
            sb.Append(" (CASE WHEN g.edu IS NULL THEN 0 ELSE g.edu END)edu,(CASE WHEN h.nightErr IS NULL THEN 0 ELSE h.nightErr END) nightErr");
            sb.Append(" ,(CASE WHEN i.BR_PEOPLE IS NULL THEN 0 ELSE i.BR_PEOPLE END) BR_PEOPLE");
            sb.Append(" ,(CASE WHEN j.MONTH_MD_PEOPLE IS NULL THEN 0 ELSE j.MONTH_MD_PEOPLE END) MONTH_MD_PEOPLE ");
            sb.Append(" ,(CASE WHEN k.MONTH_MD_AMOUNT IS NULL THEN 0 ELSE k.MONTH_MD_AMOUNT END) MONTH_MD_AMOUNT ");
            sb.Append(" ,(CASE WHEN m.ERROR_MD_PEOPLE IS NULL THEN 0 ELSE m.ERROR_MD_PEOPLE END) ERROR_MD_PEOPLE ");
            sb.Append(" ,(CASE WHEN n.ERROR_MD_AMOUNT IS NULL THEN 0 ELSE n.ERROR_MD_AMOUNT END) ERROR_MD_AMOUNT ");
            sb.Append(" From TB_D_R_RES_ACTURL a ");
            sb.Append(" left join TB_9_M_COMM_D b on a.RESTAURANT_CD = b.SUB_CD");
            sb.Append(" and b.SYS_CD ='DE' and b.MAIN_CD = 'RESTAURANT_CD' and b.IS_VALID = 'Y'");
            sb.Append(" left join (");
            sb.Append(" select COUNT(MEALSHIFT) shiftA,CONVERT(char(10),MANAGER_DT, 120)MANAGER_DT,RESTAURANT_CD,MANAGER_UNIT");
            sb.Append(" from TB_D_R_RES_ACTURL where MEALSHIFT = 'A' and isnull(RESTAURANT_ERROR_CD,'') =''");
            sb.Append(" and left(CONVERT(varchar, MANAGER_DT,120),10)  between @MANAGER_YM_S and @MANAGER_YM_E ");
            if (!RESTAURANT_CD.Equals("-1"))
            {
                sb.Append(" and RESTAURANT_CD = @RESTAURANT_CD");
                ht.Add("@RESTAURANT_CD", RESTAURANT_CD);
            }

            sb.Append(" group by CONVERT(char(10),MANAGER_DT, 120) ,RESTAURANT_CD,MANAGER_UNIT");
            sb.Append(" ) c");
            sb.Append(" on CONVERT(char(10),a.MANAGER_DT, 120) = CONVERT(char(10),c.MANAGER_DT, 120) and a.RESTAURANT_CD = c.RESTAURANT_CD and a.MANAGER_UNIT = c.MANAGER_UNIT ");
            sb.Append(" left join (");
            sb.Append(" select COUNT(MEALSHIFT)shiftA_error,CONVERT(char(10),MANAGER_DT, 120)MANAGER_DT,RESTAURANT_CD,MANAGER_UNIT ");
            sb.Append(" from TB_D_R_RES_ACTURL");
            sb.Append(" where MEALSHIFT = 'A' and isnull(RESTAURANT_ERROR_CD,'') <>''");
            sb.Append(" and left(CONVERT(varchar, MANAGER_DT,120),10)  between @MANAGER_YM_S and @MANAGER_YM_E");
            if (!RESTAURANT_CD.Equals("-1"))
            {
                sb.Append(" and RESTAURANT_CD = @RESTAURANT_CD");
                //ht.Add("@RESTAURANT_CD", RESTAURANT_CD);
            }
            sb.Append(" group by CONVERT(char(10),MANAGER_DT, 120) ,RESTAURANT_CD,MANAGER_UNIT");
            sb.Append(" ) d");
            sb.Append(" on CONVERT(char(10),a.MANAGER_DT, 120) = CONVERT(char(10),d.MANAGER_DT, 120) and a.RESTAURANT_CD = d.RESTAURANT_CD and a.MANAGER_UNIT = d.MANAGER_UNIT ");
            sb.Append(" left join (");
            sb.Append(" select COUNT(MEALSHIFT)shiftB,CONVERT(char(10),MANAGER_DT, 120)MANAGER_DT,RESTAURANT_CD,MANAGER_UNIT");
            sb.Append(" from TB_D_R_RES_ACTURL");
            sb.Append(" where MEALSHIFT = 'B' and isnull(RESTAURANT_ERROR_CD,'') ='' ");
            sb.Append(" and left(CONVERT(varchar, MANAGER_DT,120),10)  between @MANAGER_YM_S and @MANAGER_YM_E");
            if (!RESTAURANT_CD.Equals("-1"))
            {
                sb.Append(" and RESTAURANT_CD = @RESTAURANT_CD");
                //ht.Add("@RESTAURANT_CD", RESTAURANT_CD);
            }
            sb.Append(" group by CONVERT(char(10),MANAGER_DT, 120) ,RESTAURANT_CD,MANAGER_UNIT");
            sb.Append(" ) e");
            sb.Append(" on CONVERT(char(10),a.MANAGER_DT, 120) = CONVERT(char(10),e.MANAGER_DT, 120) and a.RESTAURANT_CD = e.RESTAURANT_CD and a.MANAGER_UNIT = e.MANAGER_UNIT ");
            sb.Append(" left join (");
            sb.Append(" select COUNT(MEALSHIFT) shiftC,CONVERT(char(10),MANAGER_DT, 120)MANAGER_DT,RESTAURANT_CD,MANAGER_UNIT");
            sb.Append(" from TB_D_R_RES_ACTURL");
            sb.Append(" where MEALSHIFT = 'C' and isnull(RESTAURANT_ERROR_CD,'') =''");
            sb.Append(" and left(CONVERT(varchar, MANAGER_DT,120),10)  between @MANAGER_YM_S and @MANAGER_YM_E");
            if (!RESTAURANT_CD.Equals("-1"))
            {
                sb.Append(" and RESTAURANT_CD = @RESTAURANT_CD");
                //ht.Add("@RESTAURANT_CD", RESTAURANT_CD);
            }
            sb.Append(" group by CONVERT(char(10),MANAGER_DT, 120) ,RESTAURANT_CD,MANAGER_UNIT");
            sb.Append(" ) f ");
            sb.Append(" on CONVERT(char(10),a.MANAGER_DT, 120) = CONVERT(char(10),f.MANAGER_DT, 120) and a.RESTAURANT_CD = f.RESTAURANT_CD and a.MANAGER_UNIT = f.MANAGER_UNIT");
            sb.Append(" left join ( ");
            sb.Append(" select COUNT(MEALSHIFT) edu,CONVERT(char(10),MANAGER_DT, 120)MANAGER_DT,RESTAURANT_CD,MANAGER_UNIT ");
            sb.Append(" from TB_D_R_RES_ACTURL ");
            sb.Append(" where RESTAURANT_ERROR_CD ='7' ");
            sb.Append(" and left(CONVERT(varchar, MANAGER_DT,120),10)  between @MANAGER_YM_S and @MANAGER_YM_E");
            if (!RESTAURANT_CD.Equals("-1"))
            {
                sb.Append(" and RESTAURANT_CD = @RESTAURANT_CD");
                //ht.Add("@RESTAURANT_CD", RESTAURANT_CD);
            }
            sb.Append(" group by CONVERT(char(10),MANAGER_DT, 120) ,RESTAURANT_CD,MANAGER_UNIT");
            sb.Append(" ) g ");
            sb.Append(" on CONVERT(char(10),a.MANAGER_DT, 120) = CONVERT(char(10),g.MANAGER_DT, 120) and a.RESTAURANT_CD = g.RESTAURANT_CD and a.MANAGER_UNIT = g.MANAGER_UNIT ");
            sb.Append(" left join ( ");
            sb.Append(" select COUNT(MEALSHIFT)nightErr,CONVERT(char(10),MANAGER_DT, 120)MANAGER_DT,RESTAURANT_CD,MANAGER_UNIT");
            sb.Append(" from TB_D_R_RES_ACTURL");
            sb.Append(" where (MEALSHIFT = 'B' or MEALSHIFT = 'C')");
            sb.Append(" and RESTAURANT_ERROR_CD <> '' and RESTAURANT_ERROR_CD <> '7'");
            sb.Append(" and left(CONVERT(varchar, MANAGER_DT,120),10)  between @MANAGER_YM_S and @MANAGER_YM_E");
            if (!RESTAURANT_CD.Equals("-1"))
            {
                sb.Append(" and RESTAURANT_CD = @RESTAURANT_CD");
                //ht.Add("@RESTAURANT_CD", RESTAURANT_CD);
            }
            sb.Append(" group by CONVERT(char(10),MANAGER_DT, 120) ,RESTAURANT_CD,MANAGER_UNIT");
            sb.Append(" ) h");
            sb.Append(" on CONVERT(char(10),a.MANAGER_DT, 120) = CONVERT(char(10),h.MANAGER_DT, 120) and a.RESTAURANT_CD = h.RESTAURANT_CD and a.MANAGER_UNIT = h.MANAGER_UNIT ");
            sb.Append(" left join TB_D_R_RES_DAY_ATTEND i");
            sb.Append(" on CONVERT(char(10),a.MANAGER_DT, 120) = CONVERT(char(10),i.MANAGER_DT, 120) and a.MANAGER_UNIT = i.MANAGER_UNIT");
            if (!RESTAURANT_CD.Equals("-1"))
            {
                sb.Append(" and i.RESTAURANT_CD = @RESTAURANT_CD");
                //ht.Add("@RESTAURANT_CD", RESTAURANT_CD);
            }
            //begin
            sb.Append(@" left join (  
	                     select COUNT(MEALSHIFT)MONTH_MD_PEOPLE,CONVERT(char(10),MANAGER_DT, 120)MANAGER_DT,RESTAURANT_CD,MANAGER_UNIT 
	                     from TB_D_R_RES_ACTURL where MEALSHIFT = 'D' and isnull(RESTAURANT_ERROR_CD,'') = ''
	                     and left(CONVERT(varchar, MANAGER_DT,120),10)  between @MANAGER_YM_S and @MANAGER_YM_E");
            if (!RESTAURANT_CD.Equals("-1"))
            {
                sb.Append(" and RESTAURANT_CD = @RESTAURANT_CD");
                //ht.Add("@RESTAURANT_CD", RESTAURANT_CD);
            }
            sb.Append(@" group by CONVERT(char(10),MANAGER_DT, 120) ,RESTAURANT_CD,MANAGER_UNIT
	                     ) j on CONVERT(char(10),a.MANAGER_DT, 120) = CONVERT(char(10),j.MANAGER_DT, 120) and a.RESTAURANT_CD = j.RESTAURANT_CD and a.MANAGER_UNIT = j.MANAGER_UNIT 
                    left join (  
	                    select SUM(MEAL_AMOUNT)MONTH_MD_AMOUNT,CONVERT(char(10),MANAGER_DT, 120)MANAGER_DT,RESTAURANT_CD,MANAGER_UNIT 
	                    from TB_D_R_RES_ACTURL where MEALSHIFT = 'D' and isnull(RESTAURANT_ERROR_CD,'') = ''
	                    and left(CONVERT(varchar, MANAGER_DT,120),10)  between @MANAGER_YM_S and @MANAGER_YM_E");
            if (!RESTAURANT_CD.Equals("-1"))
            {
                sb.Append(" and RESTAURANT_CD = @RESTAURANT_CD");
                //ht.Add("@RESTAURANT_CD", RESTAURANT_CD);
            }
            sb.Append(@" group by CONVERT(char(10),MANAGER_DT, 120) ,RESTAURANT_CD,MANAGER_UNIT
	                     ) k on CONVERT(char(10),a.MANAGER_DT, 120) = CONVERT(char(10),k.MANAGER_DT, 120) and a.RESTAURANT_CD = k.RESTAURANT_CD and a.MANAGER_UNIT = k.MANAGER_UNIT
                    left join (  
	                    select COUNT(MEALSHIFT)ERROR_MD_PEOPLE,CONVERT(char(10),MANAGER_DT, 120)MANAGER_DT,RESTAURANT_CD,MANAGER_UNIT 
	                    from TB_D_R_RES_ACTURL where MEALSHIFT = 'D' and isnull(RESTAURANT_ERROR_CD,'') <> ''
	                    and left(CONVERT(varchar, MANAGER_DT,120),10)  between @MANAGER_YM_S and @MANAGER_YM_E");
            if (!RESTAURANT_CD.Equals("-1"))
            {
                sb.Append(" and RESTAURANT_CD = @RESTAURANT_CD");
                //ht.Add("@RESTAURANT_CD", RESTAURANT_CD);
            }
            sb.Append(@" group by CONVERT(char(10),MANAGER_DT, 120) ,RESTAURANT_CD,MANAGER_UNIT
	                     ) m on CONVERT(char(10),a.MANAGER_DT, 120) = CONVERT(char(10),m.MANAGER_DT, 120) and a.RESTAURANT_CD = m.RESTAURANT_CD and a.MANAGER_UNIT = m.MANAGER_UNIT
                    left join (  
	                    select SUM(MEAL_AMOUNT)ERROR_MD_AMOUNT,CONVERT(char(10),MANAGER_DT, 120)MANAGER_DT,RESTAURANT_CD,MANAGER_UNIT 
	                    from TB_D_R_RES_ACTURL where MEALSHIFT = 'D' and isnull(RESTAURANT_ERROR_CD,'') <> ''
	                    and left(CONVERT(varchar, MANAGER_DT,120),10)  between @MANAGER_YM_S and @MANAGER_YM_E");
            if (!RESTAURANT_CD.Equals("-1"))
            {
                sb.Append(" and RESTAURANT_CD = @RESTAURANT_CD");
                //ht.Add("@RESTAURANT_CD", RESTAURANT_CD);
            }
            sb.Append(@" group by CONVERT(char(10),MANAGER_DT, 120) ,RESTAURANT_CD,MANAGER_UNIT
	                    ) n on CONVERT(char(10),a.MANAGER_DT, 120) = CONVERT(char(10),n.MANAGER_DT, 120) and a.RESTAURANT_CD = n.RESTAURANT_CD and a.MANAGER_UNIT = n.MANAGER_UNIT");
               
            //end
            sb.Append(" Where left(CONVERT(varchar, a.MANAGER_DT,120),10)  between @MANAGER_YM_S and @MANAGER_YM_E");
                    
            if (!RESTAURANT_CD.Equals("-1"))
            {
                sb.Append(" and a.RESTAURANT_CD = @RESTAURANT_CD");
                //ht.Add("@RESTAURANT_CD", RESTAURANT_CD);
            }
            //sb.Append(" group by a.MANAGER_DT,a.RESTAURANT_CD+'-'+b.SUB_DESC,MEALSHIFT,a.MANAGER_UNIT,");
            //sb.Append(" c.shiftA,d.shiftA_error,e.shiftB,f.shiftC,g.edu,h.nightErr");
            sb.Append(" )k");

            ht.Add("@MANAGER_YM_S", MANAGER_YM_S);
            ht.Add("@MANAGER_YM_E", MANAGER_YM_E);

            sb.Append(" group by CONVERT(char(10),MANAGER_DT, 120) ,RESTAURANT_CD,MANAGER_UNIT,shiftA,shiftA_error,shiftB,shiftC,edu,nightErr ");
            sb.Append(" ,MONTH_MD_PEOPLE,MONTH_MD_AMOUNT,ERROR_MD_PEOPLE,ERROR_MD_AMOUNT ");
            sb.Append(" order by MANAGER_DT,RESTAURANT_CD,MANAGER_UNIT");

            return dbConn.Query(sb, ht);

        }
        catch
        {
            throw;
        }
    }

    public void getRes_Amount()
    {
        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select BF_AMOUNT,DN_AMOUNT");
            sb.Append(" from TB_D_M_RES_PARA");
            sb.Append(" where COMPANY_CD = @COMPANY_CD");

            ht.Add("@COMPANY_CD", COMPANY_CD);


            DataTable dt = dbConn.Query(sb, ht);

            if (dt.Rows.Count > 0)
            {               
                BF_AMOUNT = dt.Rows[0]["BF_AMOUNT"].ToString();
                DN_AMOUNT = dt.Rows[0]["DN_AMOUNT"].ToString();
            }

        }
        catch
        {
            throw;
        }
    }

    public string getRES_DAY(string md, string mu)
    {
        DBConnector dbConn = new DBConnector();
        string st = "0";
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select BR_PEOPLE from TB_D_R_RES_DAY_ATTEND");
            sb.Append(" where MANAGER_DT = @MANAGER_DT");
            sb.Append(" and MANAGER_UNIT = @MANAGER_UNIT");

            ht.Add("@MANAGER_DT", md);
            ht.Add("@MANAGER_UNIT", mu);

            DataTable dt = dbConn.Query(sb, ht);

            if(dt.Rows.Count > 0){
                st = dt.Rows[0]["BR_PEOPLE"].ToString();
            }
            return st;

        }
        catch
        {
            throw;
        }
    }

    public string getBR_Data(string md, string mu)
    {
        DBConnector dbConn = new DBConnector();
        string st = "0";
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select count(MEALSHIFT) from TB_D_R_RES_ACTURL");
            sb.Append(" where MANAGER_DT = @MANAGER_DT");
            sb.Append(" and MANAGER_UNIT = @MANAGER_UNIT");
            sb.Append(" and MEALSHIFT = @MEALSHIFT");

            ht.Add("@MANAGER_DT", md);
            ht.Add("@MANAGER_UNIT", mu);

            DataTable dt = dbConn.Query(sb, ht);

            if (dt.Rows.Count > 0)
            {
                st = dt.Rows[0]["MEALSHIFT"].ToString();
            }
            return st;

        }
        catch
        {
            throw;
        }
    }

    public DataTable getErr_Data()
    {
        DBConnector dbConn = new DBConnector();        
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select CONVERT(char(10),a.MANAGER_DT, 120) MANAGER_DT,a.EMP_ID,isnull(b.EMP_NAME,'') as EMP_NAME,a.DEPT_NO,a.MEALSHIFT,a.RESTAURANT_ERROR_CD+'-'+c.SUB_DESC RESTAURANT_ERROR_CD,");
            sb.Append(" a.MEAL_TIMES,a.CARD_START,a.CARD_END");
            sb.Append(" from TB_D_R_RES_ACTURL a");
            sb.Append(" left join TB_H_M_EMP b");
            sb.Append(" on a.EMP_ID = b.EMP_ID");
            sb.Append(" left join TB_9_M_COMM_D c");
            sb.Append(" on a.RESTAURANT_ERROR_CD = c.SUB_CD and c.SYS_CD = 'DE' and c.MAIN_CD = 'RESTAURANT_ERROR_CD'");
            sb.Append(" and c.IS_VALID='Y'");
            sb.Append(" where CAST(a.MANAGER_DT as DATE) between @MANAGER_YM_S and @MANAGER_YM_E");           
            sb.Append(" and RESTAURANT_ERROR_CD <> '' and RESTAURANT_ERROR_CD <> '7'");
            

            if (!RESTAURANT_CD.Equals("-1"))
            {
                sb.Append(" and RESTAURANT_CD = @RESTAURANT_CD");
                ht.Add("@RESTAURANT_CD", RESTAURANT_CD);
            }
            

            ht.Add("@MANAGER_YM_S", MANAGER_YM_S);
            ht.Add("@MANAGER_YM_E", MANAGER_YM_E);

            return dbConn.Query(sb, ht);

        }
        catch
        {
            throw;
        }
    }

    public DataTable searchMonthDateResult()
    {

        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select a.MANAGER_YM,RESTAURANT_CD+'-'+b.SUB_DESC RESTAURANT_CD,a.MANAGER_UNIT,SUM(MONTH_BR_PEOPLE)MONTH_BR_PEOPLE,SUM(MONTH_BR_BOND_PEOPLE)MONTH_BR_BOND_PEOPLE,");
            sb.Append("SUM(ERROE_BR_PEOPLE)ERROE_BR_PEOPLE,SUM(MONTH_DN_BOND_PEOPLE)MONTH_DN_BOND_PEOPLE,");
            sb.Append("SUM(OVERTIME_BOND_PEOPLE)OVERTIME_BOND_PEOPLE,SUM(EDU_PEOPLE)EDU_PEOPLE,SUM(ERROE_DN_PEOPLE)ERROE_DN_PEOPLE");
            sb.Append(",isnull(SUM(c.L_AMOUNT),0) L_AMOUNT,isnull(SUM(c.L_PRICE * c.L_AMOUNT),0) L_PRICE,isnull(SUM(c.E1_AMOUNT),0) E1_AMOUNT,isnull(SUM(c.E1_PRICE * c.E1_AMOUNT),0) E1_PRICE,isnull(SUM(c.G_TOTAL_AMOUNT),0) G_TOTAL_AMOUNT");
            sb.Append(",isnull(SUM(c.G_TOTAL_PRICE),0) G_TOTAL_PRICE");
            sb.Append(" ,isnull(SUM(a.MONTH_MD_PEOPLE),0) MONTH_MD_PEOPLE ,isnull(SUM(a.MONTH_MD_AMOUNT),0) MONTH_MD_AMOUNT");
            sb.Append(" ,isnull(SUM(a.ERROR_MD_PEOPLE),0) ERROR_MD_PEOPLE ,isnull(SUM(a.ERROR_MD_AMOUNT),0) ERROR_MD_AMOUNT");
            sb.Append(" From TB_D_R_RES_MONTH_ACTURL a");
            sb.Append(" left join TB_9_M_COMM_D b");
            sb.Append(" on a.RESTAURANT_CD = b.SUB_CD");
            sb.Append(" and b.SYS_CD ='DE' and b.MAIN_CD = 'RESTAURANT_CD' and b.IS_VALID = 'Y'");
            sb.Append(" left join TB_D_R_RES_BOND_DTL c");
            sb.Append(" on a.MANAGER_YM = c.MANAGER_YM and a.MANAGER_UNIT = c.MANAGER_UNIT ");
            sb.Append(" Where a.MANAGER_YM = @MANAGER_YM");

            if (!RESTAURANT_CD.Equals("-1"))
            {
                sb.Append(" and RESTAURANT_CD = @RESTAURANT_CD");
                ht.Add("@RESTAURANT_CD", RESTAURANT_CD);
            }
            sb.Append(" group by a.MANAGER_YM,RESTAURANT_CD+'-'+b.SUB_DESC,a.MANAGER_UNIT");
            sb.Append(" order by a.MANAGER_YM,RESTAURANT_CD,a.MANAGER_UNIT");
            ht.Add("@MANAGER_YM", MANAGER_YM);
           

            return dbConn.Query(sb, ht);

        }
        catch
        {
            throw;
        }
    }

    //與新誠確認邏輯　餐券於中壢時　　寫至中壢二製　　於觀音時寫至事務棟
    public DataTable searchMonthBondDate()
    {

        DBConnector dbConn = new DBConnector();
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select MANAGER_UNIT,PLANT_CD,isnull(SUM(L_AMOUNT),0) L_AMOUNT,isnull(SUM(L_PRICE * L_AMOUNT),0) L_PRICE,isnull(SUM(E1_AMOUNT),0) E1_AMOUNT,");
            sb.Append("isnull(SUM(E1_PRICE * E1_AMOUNT),0) E1_PRICE,isnull(SUM(G_TOTAL_AMOUNT),0) G_TOTAL_AMOUNT,isnull(SUM(G_TOTAL_PRICE),0) G_TOTAL_PRICE");
            sb.Append(" From TB_D_R_RES_BOND_DTL ");
            sb.Append(" Where MANAGER_YM = @MANAGER_YM and PLANT_CD = @PLANT_CD and MANAGER_UNIT = @MANAGER_UNIT");
            sb.Append(" group by MANAGER_YM,MANAGER_UNIT,PLANT_CD ");
            sb.Append(" order by MANAGER_YM,MANAGER_UNIT,PLANT_CD ");

            ht.Add("@MANAGER_YM", MANAGER_YM);
            ht.Add("@PLANT_CD", PLANT_CD);
            ht.Add("@MANAGER_UNIT", MANAGER_UNIT);

            return dbConn.Query(sb, ht);

        }
        catch
        {
            throw;
        }
    }

}