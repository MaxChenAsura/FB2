using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;

/// <summary>
/// Card_Search 的摘要描述
/// </summary>
public class Card_Search : BaseDAO
{
    public string CARD_NO { get; set; }
    public string CARD_NAME { get; set; }
    public string TEMP_CARD_CD { get; set; }

    public Card_Search()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    public System.Data.DataTable getCardData(string sortExpression)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();


            sb.Append("Select CARD_NO,CARD_NAME from TB_D_M_CARD where CARD_TYPE is not null ");

            if (CARD_NO != "")
            {
                sb.Append(" and CARD_NO like @CARD_NO");
                ht.Add("@CARD_NO", CARD_NO + "%");
            }
            if (CARD_NAME != "")
            {
                sb.Append(" and CARD_NAME like @CARD_NAME");
                ht.Add("@CARD_NAME", "%" + CARD_NAME + "%");
            }
            sb.Append(" and END_DT >= GETDATE() or END_DT is null ");
            sb.Append(" order by " + sortExpression);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public System.Data.DataTable getCardData2(string sortExpression)
    {
        try
        {
            if (sortExpression.Contains("CARD_NO"))
                sortExpression = sortExpression.Replace("CARD_NO", "a.CARD_NO");

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select a.CARD_NO,a.CARD_NAME from TB_D_M_CARD a left join TB_D_M_CARD_TYPE b on b.CARD_TYPE=a.CARD_TYPE ");
            sb.Append(" where a.CARD_TYPE is not null ");
            sb.Append(" and a.CARD_NO not in( ");
            sb.Append(" select c.CARD_NO from TB_D_M_TEMP_CARD_RECORD c");
            sb.Append(" where c.BORROW_STATUS != 'Y' )");

            if (CARD_NO != "")
            {
                sb.Append(" and a.CARD_NO like @CARD_NO");
                ht.Add("@CARD_NO", CARD_NO + "%");
            }
            if (CARD_NAME != "")
            {
                sb.Append(" and a.CARD_NAME like @CARD_NAME");
                ht.Add("@CARD_NAME", "%" + CARD_NAME + "%");
            }
            if (TEMP_CARD_CD != "")
            {
                sb.Append(" and a.TEMP_CARD_CD in (@TEMP_CARD_CD)");
                ht.Add("@TEMP_CARD_CD", TEMP_CARD_CD.Split(','));
            }
            sb.Append(" and GETDATE() >= a.START_DT and GETDATE() <= a.END_DT and b.CARD_USED_CD='C' and b.CARD_TYPE='05'  ");

            sb.Append(" order by " + sortExpression);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public System.Data.DataTable getCardData3(string sortExpression)
    {
        try
        {
            if (sortExpression.Contains("CARD_NO"))
                sortExpression = sortExpression.Replace("CARD_NO", "a.CARD_NO");

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select a.CARD_NO,a.CARD_NAME from TB_D_M_CARD a left join TB_D_M_CARD_TYPE b on b.CARD_TYPE=a.CARD_TYPE ");
            sb.Append(" where a.CARD_TYPE is not null ");
            sb.Append(" and a.CARD_NO in( ");
            sb.Append(" select c.CARD_NO from TB_D_M_TEMP_CARD_RECORD c ");
            //sb.Append(" where c.START_DT <= GETDATE() and c.END_DT >= GETDATE() ");
            // BORROW_STATUS	 臨時卡借用狀態 
            // N.未還  Y.已還  L.遺失。遺失時，同步註銷卡片檔的資料。
            sb.Append(" where c.BORROW_STATUS='N' )");

            if (CARD_NO != "")
            {
                sb.Append(" and a.CARD_NO like @CARD_NO");
                ht.Add("@CARD_NO", CARD_NO + "%");
            }
            if (CARD_NAME != "")
            {
                sb.Append(" and a.CARD_NAME like @CARD_NAME");
                ht.Add("@CARD_NAME", "%" + CARD_NAME + "%");
            }
            if (TEMP_CARD_CD != "")
            {
                sb.Append(" and a.TEMP_CARD_CD in (@TEMP_CARD_CD)");
                ht.Add("@TEMP_CARD_CD", TEMP_CARD_CD.Split(','));
            }
            sb.Append(" and GETDATE() >= a.START_DT and GETDATE() <= a.END_DT and b.CARD_USED_CD='C' and b.CARD_TYPE='05'  ");
            sb.Append(" order by " + sortExpression);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public System.Data.DataTable getCardData4(string sortExpression)
    {
        try
        {
            if (sortExpression.Contains("CARD_NO"))
                sortExpression = sortExpression.Replace("CARD_NO", "a.CARD_NO");

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select a.CARD_NO,a.CARD_NAME ");
            sb.Append(" from TB_D_M_CARD a ");  //卡片資料檔
            sb.Append(" left join TB_D_M_CARD_TYPE b on a.CARD_TYPE=b.CARD_TYPE ");  //卡片屬性設定檔
            sb.Append(" where a.TEMP_CARD_CD != '' ");  //臨時卡區分
            sb.Append(" and GETDATE() >= a.START_DT and GETDATE() <= a.END_DT and b.CARD_USED_CD = 'C'  and b.CARD_TYPE='05' ");    //卡片使用對象代碼  A.社內  B.社外  C.共用
            if (CARD_NO != "")
            {
                sb.Append(" and a.CARD_NO like @CARD_NO");
                ht.Add("@CARD_NO", CARD_NO + "%");
            }
            if (CARD_NAME != "")
            {
                sb.Append(" and a.CARD_NAME like @CARD_NAME");
                ht.Add("@CARD_NAME", "%" + CARD_NAME + "%");
            }
            sb.Append(" order by " + sortExpression);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

}