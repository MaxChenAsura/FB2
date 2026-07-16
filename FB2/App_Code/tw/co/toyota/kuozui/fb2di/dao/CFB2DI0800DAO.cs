using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;

/// <summary>
/// CFBDI0800DAO 的摘要描述
/// </summary>
public class CFB2DI0800DAO : BaseDAO
{
    public CFB2DI0800DAO()
    {
        //
        // TODO: 在這裡新增建構函式邏輯
        //
    }

    //Gridview 查詢資料
    public DataTable getData(int startRowIndex, int maximumRows, string sortExpression, string dept_no, string target_type, string ym)
    {
        try
        {
            //因年月為會計年度, 依傳入的年月來判斷 該區間如2015/03-> 2014-04, 2015-03 2015/04-> 2015-04~2016-03
            //20170206
            // 2016/09 -> 201604 ~ 201703
            // 2017/01 -> 201604 ~ 201703
            // 2017/05 -> 201704 ~ 201803
            //若月份介於1~3 : 開始年 = 傳入年-1,結束年 = 傳入年
            //若月份介於4~12: 開始年 = 傳件年,  結束年 = 傳件年+1
            string[] tmp = ym.Split('/');
            int year_start = 0;
            int year_end = 0;
            if (tmp.Count() > 0)
            {
                int year = Convert.ToInt32(tmp[0]);
                int month = Convert.ToInt32(tmp[1]);

                if (month < 4)
                {
                    year_start = year - 1;
                    year_end = year;
                }
                else
                {
                    year_start = year;
                    year_end = year + 1;
                }
            }
            string startYM = Convert.ToString(year_start) + "-04";
            string endYM = Convert.ToString(year_end) + "-03";


            if (sortExpression.Contains("EMP_ID"))
            {
                sortExpression = sortExpression.Replace("EMP_ID", "A.EMP_ID");
            }
            if (sortExpression.Contains("EMP_NAME"))
            {
                sortExpression = sortExpression.Replace("EMP_NAME", "A.EMP_NAME");
            }
            if (sortExpression.Contains("DEPT_NO"))
            {
                sortExpression = sortExpression.Replace("DEPT_NO", "A.DEPT_NO");
            }
            if (sortExpression.Contains("OVERTIME_CTL_CD"))
            {
                sortExpression = sortExpression.Replace("OVERTIME_CTL_CD", "A.OVERTIME_CTL_CD");
            }
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@"Select * "
                     + " from "
                     + " ("
                     + " select  ROW_NUMBER() OVER( ORDER BY " + sortExpression + " ) As RowNumber,"
                     + " A.DEPT_NO,A.EMP_ID,A.EMP_NAME,A.OVERTIME_CTL_CD,A.OVERTIME_CTL_DESC "
                     //+ " , CAST(ROUND(((convert(decimal(12,2),sum(isnull(C.A_OVERTIME_HOUR_M, 0)) + sum(isnull(F.APPROVE_OVERTIME_HOUR_G, 0)))) / 60), 2)AS DECIMAL(12, 2))  APPROVE_OVERTIME_HOUR "//46H加班累計
                     + " , CAST(ROUND(((convert(decimal,sum(isnull(I.HYPER_HOUR, 0)))) / 60), 2) AS DECIMAL(12, 2)) HYPER_HOUR "//三高累計時數
                     + " , CAST(ROUND(((convert(decimal,sum(isnull(I.NORMAL_HOUR, 0)))) / 60), 2) AS DECIMAL(12, 2)) NORMAL_HOUR "//一般累計時數
                     + " , CAST(ROUND(((convert(decimal,sum(isnull(C.A_OVERTIME_HOUR_M, 0)))) / 60), 2) AS DECIMAL(12, 2)) A_APPROVE_OVERTIME_HOUR "//加班實績(月)-平日,假日
                     + " , CAST(ROUND(((convert(decimal(12,2),sum(isnull(F.B_OVERTIME_HOUR_M, 0)))) / 60), 2)  AS DECIMAL(12, 2))  B_APPROVE_OVERTIME_HOUR "
                     + " , CAST(ROUND(((convert(decimal(12,2),sum(isnull(D.Z0_TOTAL_TIME_M, 0)))) / 60), 2) AS DECIMAL(12, 2))    Z_TOTAL_TIME_APPROVE "//換休實績(月)-平日,假日
                     + " , CAST(ROUND(((convert(decimal(12,2),sum(isnull(G.X0_TOTAL_TIME_M, 0)))) / 60), 2) AS DECIMAL(12, 2))    X_TOTAL_TIME_APPROVE "
                     + " , CAST(ROUND(((convert(decimal(12,2),sum(isnull(C.A_OVERTIME_HOUR_M, 0)) - sum(isnull(D.Z0_TOTAL_TIME_M, 0)))) / 60), 2) AS DECIMAL(12, 2))  AZ_TOTAL_TIME_APPROVE  " //未換休累計-平日,假日
                     + " , CAST(ROUND(((convert(decimal(12,2),sum(isnull(H.X0_IS_APPLY_Y, 0)) - sum(isnull(H.X0_TOTAL_TIME_Y, 0)))) / 60), 2) AS DECIMAL(12, 2))     BX_TOTAL_TIME_APPROVE "
                     + " , CAST(ROUND(((convert(decimal(12,2),sum(isnull(E.X0_IS_APPLY_M, 0)))) / 60), 2) AS DECIMAL(12, 2))		  X0_IS_APPLY_M "   //假日加班申告及未申告(月)
                     + " , CAST(ROUND(((convert(decimal(12,2),sum(isnull(F.B_OVERTIME_HOUR_M, 0)) - sum(isnull(E.X0_IS_APPLY_M, 0)))) / 60), 2) AS DECIMAL(12, 2))    BX_NOT_APPLY_M "
                     + " , case when sum(isnull(C.A_OVERTIME_HOUR_M, 0))=0 then 0"//換休率%，平日,假日,合計
                     + " else CAST(ROUND(((convert(decimal(12,4),sum(isnull(convert(decimal(12,4),D.Z0_TOTAL_TIME_M), 0)) / sum(isnull(convert(decimal(12,4),C.A_OVERTIME_HOUR_M), 0)))) * 100), 2)  AS DECIMAL(12, 2)) end  A_EXCHANG_RATE "
                     + " , case when sum(isnull(F.B_OVERTIME_HOUR_M, 0))=0 then 0 "
                     + " else CAST(ROUND(((convert(decimal(12,4),sum(isnull(convert(decimal(12,4),E.X0_IS_APPLY_M), 0)) / sum(isnull(convert(decimal(12,4),F.B_OVERTIME_HOUR_M), 0)))) * 100), 2)  AS DECIMAL(12, 2)) end  B_EXCHANG_RATE"
                     + " , case when (sum(isnull(C.A_OVERTIME_HOUR_M, 0))+sum(isnull(F.B_OVERTIME_HOUR_M, 0)))=0 then 0 "
                     + " else CAST(ROUND(((convert(decimal(12,4),(sum(isnull(convert(decimal(12,4),D.Z0_TOTAL_TIME_M), 0))+sum(isnull(convert(decimal(12,4),E.X0_IS_APPLY_M), 0))) / (sum(isnull(convert(decimal(12,4),C.A_OVERTIME_HOUR_M), 0))+sum(isnull(convert(decimal(12,4),F.B_OVERTIME_HOUR_M), 0))) )) * 100), 2)   AS DECIMAL(12, 2))end  AB_EXCHANG_RATE "
                     );

            sb.Append(@" from ( ");
            //若是當月用 VW_H_EMP_DATA, 其餘用 TB_H_R_EMP_DATA_MONTH
            string targetTableNAme = " ";
            string extraCondition = " ";
            if (Convert.ToInt32(ym.Replace("/", "")) < Convert.ToInt32(DateTime.Now.ToString("yyyyMM")))
            {
                targetTableNAme = " TB_H_R_EMP_DATA_MONTH A  with (nolock) ";
                extraCondition = " AND A.YM = @YM ";
            }
            else
            {
                targetTableNAme = " VW_H_EMP_DATA A  with (nolock) ";
                extraCondition = " ";
                if (SessionHandle.Current.is_super != "Y")
                {
                    extraCondition = " AND A.EMP_ID IN(	 select EMP_ID from dbo.FN_H_GET_AUTH_EMP(@loginID,@departments)  ) ";
                    ht.Add("@loginID", SessionHandle.Current.emp_id);
                    ht.Add("@departments", SessionHandle.Current.departments);
                }
                
            }
            //個人資料
            sb.Append(@" SELECT  A.EMP_ID, A.EMP_NAME, A.EMP_STATUS, A.PLANT_CD "
                     + " , A.DEPT_NO_20, A.DEPT_NO, A.PJOB_CD, A.WS_CD "
                     + " , A.OVERTIME_CTL_CD, D.SUB_DESC + '/' + D.CODE_VAL1 OVERTIME_CTL_DESC "
                     + " from " + targetTableNAme 
                     + " left join TB_9_M_COMM_D D  with (nolock) on A.OVERTIME_CTL_CD = D.SUB_CD and D.SYS_CD = 'HB' and D.MAIN_CD = 'OVERTIME_CTL_CD'   "
                     + " inner join TB_D_M_OVERTIME_TARGET_EMP E  with (nolock) ON E.TARGET_TYPE=@TARGET_TYPE and E.TARGET_YEAR=@year "
                     + " inner join  ( SELECT DEPT_NO FROM  [dbo].[FN_H_GET_WORK_DEPT](@DEPT_NO) )F ON A.DEPT_NO=F.DEPT_NO  "
                     + " WHERE  (A.EMP_STATUS=('01') or (A.EMP_STATUS=('99') and substring(convert(char(10),DATEADD(DAY,-1,A.LEAVE_DT),120),0,8) =@YM2 ))  "
                     + " AND A.WORK_CD = E.WORK_CD AND A.WS_CD= E.WS_CD AND A.PJOB_CD=E.PJOB_CD "
                     + extraCondition
                     + ") A "
                     );
            //C.平日加班實績(月)
            sb.Append(@" left join (    "
                     + " SELECT EMP_ID,SUM(APPROVE_OVERTIME_HOUR) A_OVERTIME_HOUR_M "
                     + " FROM TB_D_M_OVERTIME_APPLY A  with (nolock) "
                     + " WHERE A.FORM_STATUS  in ('Y','C','X','P') "
                     + " AND A.CHECK_STATUS = 'Y' "
                     + " AND A.DT_TYPE = '1' "
                     + " AND A.APPLY_OVERTIME_DT >= DATEADD(month, DATEDIFF(month, 0, @YMD), 0) "
                     + " AND A.APPLY_OVERTIME_DT < DATEADD(month, DATEDIFF(month, 0, @YMD) + 1, 0) "
                     + "  GROUP BY EMP_ID  "
                     + " ) C on A.EMP_ID = C.EMP_ID "
                     + " "
                );
            //D.平日換休實績(月)
            sb.Append(@" left join ( "
                    + " select EMP_ID,sum(TOTAL_TIME_APPROVE)  Z0_TOTAL_TIME_M "
                    + " from TB_D_M_LEAVE_APPLY_DAY A  with (nolock) "
                    + " where A.FORM_STATUS in ('Y','C','X','P')  "
                    + " and A.CHECK_STATUS ='Y'  "
                    + " and A.SUB_LEAVE_CD in ('Z0') "
                    + " and A.APPLY_LEAVE_SDT >= DATEADD(month, DATEDIFF(month, 0, @YMD), 0) "
                    + " and A.APPLY_LEAVE_SDT < DATEADD(month, DATEDIFF(month, 0, @YMD) + 1, 0) "
                    + " group by EMP_ID "
                    + " ) D on a.EMP_ID = D.EMP_ID "
                    + " "
                );
            //E.假日加班已申告(月)
            sb.Append(@" left join ( "
                    + " select A.EMP_ID, SUM(APPROVE_OVERTIME_HOUR)  X0_IS_APPLY_M "
                    + " from TB_D_M_OVERTIME_APPLY A  with (nolock) "
                    + " left join VW_H_EMP_DATA B  with (nolock) on A.EMP_ID=B.EMP_ID  "
                    + " Where FORM_STATUS in ('Y','C','X','P') and A.CHECK_STATUS = 'Y' "
                    + " and A.DT_TYPE != '1' "
                    + " and IS_APPLY='Y' "
                    + " and A.APPLY_OVERTIME_DT >= DATEADD(month, DATEDIFF(month, 0, @YMD), 0) "
                    + " and A.APPLY_OVERTIME_DT < DATEADD(month, DATEDIFF(month, 0, @YMD) + 1, 0) "
                    + " group by A.EMP_ID "
                    + " ) E on A.EMP_ID = E.EMP_ID "
                    + " "
                );
            //F.假日加班實績
            sb.Append(@" left join ( "

                    + " SELECT BB.EMP_ID, SUM(AA.APPROVE_OVERTIME_HOUR) B_OVERTIME_HOUR_M "
                    +" 	  ,APPROVE_OVERTIME_HOUR_G "
                    +" 	  =  SUM( case when (BB.OVERTIME_CTL_CD='1' or BB.OVERTIME_CTL_CD='4') and AA.APPROVE_OVERTIME_HOUR <=8*60 then 0  	"
                    +" 				   when (BB.OVERTIME_CTL_CD='1' or BB.OVERTIME_CTL_CD='4' )and AA.APPROVE_OVERTIME_HOUR >8*60 then  AA.APPROVE_OVERTIME_HOUR-(8*60)  "
                    +" 				   else APPROVE_OVERTIME_HOUR  end  )  "
                    +" FROM		  "
                    +" ( SELECT A.EMP_ID,A.APPLY_OVERTIME_DT, SUM(APPROVE_OVERTIME_HOUR) APPROVE_OVERTIME_HOUR   "
                    +" 	FROM TB_D_M_OVERTIME_APPLY A   with (nolock) "
                    +" 	left join VW_H_EMP_DATA B  with (nolock) on A.EMP_ID=B.EMP_ID "
                    + " WHERE A.FORM_STATUS  in ('Y','C','X','P')  "
                    +" 	AND A.CHECK_STATUS = 'Y' " 
                    +" 	AND A.DT_TYPE != '1' "  
                    + " and A.APPLY_OVERTIME_DT >= DATEADD(month, DATEDIFF(month, 0, @YMD), 0) "
                    + " and A.APPLY_OVERTIME_DT < DATEADD(month, DATEDIFF(month, 0, @YMD) + 1, 0) "
                    +" 	group by A.EMP_ID, A.APPLY_OVERTIME_DT ) AA	 "
                    +" left join (select * from "
                    + " " + targetTableNAme + "where 1=1 " + extraCondition
                    + " ) BB on AA.EMP_ID=BB.EMP_ID  "

                    +" GROUP BY BB.EMP_ID "

                    + " ) F on A.EMP_ID = F.EMP_ID "
                    + " "
                );
            //G.假日換休實績(月)
            sb.Append(@" left join ( "
                    + " select EMP_ID ,SUM(TOTAL_TIME_APPROVE)  X0_TOTAL_TIME_M "
                    + " from TB_D_M_LEAVE_APPLY_DAY A  with (nolock) "
                    + "  Where A.FORM_STATUS in ('Y','C','X','P') and A.CHECK_STATUS ='Y' "
                    + " and A.SUB_LEAVE_CD in ('X0') "
                    + " and A.APPLY_LEAVE_SDT >= DATEADD(month, DATEDIFF(month, 0, @YMD), 0) "
                    + " and A.APPLY_LEAVE_SDT < DATEADD(month, DATEDIFF(month, 0, @YMD) + 1, 0) "
                    + "  group by EMP_ID "
                    + " ) G on A.EMP_ID = G.EMP_ID "
                    + " "
                );

            //H.假日換休已休(年)  H.假日申告換休可休(年)
            sb.Append(@" left join ( "
                   + " select E.EMP_ID  "
                   + " , isnull(F.X0_TOTAL_TIME_APPROVE,0)   as X0_TOTAL_TIME_Y "
                   + " , isnull(E.APPROVE_OVERTIME_HOUR,0)   as X0_IS_APPLY_Y "
                   + " from  ( select A.EMP_ID, SUM(OVERTIME_PAY_HOUR)  APPROVE_OVERTIME_HOUR "
                   + "         from TB_D_M_OVERTIME_APPLY A with (nolock)  "
                   + "         left join VW_H_EMP_DATA B with (nolock)  on A.EMP_ID=B.EMP_ID "
                   + "         Where A.FORM_STATUS in ('Y','C','X','P') and A.CHECK_STATUS = 'Y' "
                   + "         and A.DT_TYPE != '1' and IS_APPLY='Y' "
                   + "         and A.APPLY_OVERTIME_DT >= DATEADD(month, DATEDIFF(month, 0, @startYMD), 0) "
                   + "         and A.APPLY_OVERTIME_DT < DATEADD(month, DATEDIFF(month, 0, @endYMD) + 1, 0) "
                   + "         group by A.EMP_ID ) E "
                   + " left join (select EMP_ID ,SUM(TOTAL_TIME_APPROVE)  X0_TOTAL_TIME_APPROVE "
                   + "         from TB_D_M_LEAVE_APPLY_DAY A  with (nolock) "
                   + "         Where A.FORM_STATUS in ('Y','C','X','P') and A.CHECK_STATUS ='Y' "
                   + "         and A.SUB_LEAVE_CD in ('X0') "
                   + "         and A.APPLY_LEAVE_SDT >= DATEADD(month, DATEDIFF(month, 0, @startYMD), 0) "
                   + "         and A.APPLY_LEAVE_SDT < DATEADD(month, DATEDIFF(month, 0, @endYMD) + 1, 0) "
                   + "         group by EMP_ID ) F on E.EMP_ID = F.EMP_ID  "
                   + " ) H on A.EMP_ID = H.EMP_ID "
                   + " "
                );

            //I.平日加班實績(月)-不含C條件
            sb.Append(@" left join ( "
                     + " SELECT EMP_ID,SUM(APPROVE_OVERTIME_HOUR) A_OVERTIME_HOUR_M, SUM(HYPER_HOUR) HYPER_HOUR, SUM(NORMAL_HOUR) NORMAL_HOUR "
                     + " FROM TB_D_M_OVERTIME_APPLY A with (nolock) "
                     + " WHERE A.FORM_STATUS  in ('Y','C','X','P') "
                     + " AND A.CHECK_STATUS = 'Y' "
                     + " AND A.APPLY_OVERTIME_DT >= DATEADD(month, DATEDIFF(month, 0, @YMD), 0) "
                     + " AND A.APPLY_OVERTIME_DT < DATEADD(month, DATEDIFF(month, 0, @YMD) + 1, 0) "
                     + " GROUP BY EMP_ID "
                     + " ) I on A.EMP_ID = I.EMP_ID "
                     + " "
                );

            sb.Append(" Where A.EMP_ID is not null ");
            sb.Append("group by A.DEPT_NO,A.EMP_ID,A.EMP_NAME,A.OVERTIME_CTL_CD,A.OVERTIME_CTL_DESC");
            sb.Append(") god_data  where RowNumber between CAST(@startRowIndex+1 as varchar)");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar)");

            ht.Add("@year", tmp[0]);
            ht.Add("@DEPT_NO", dept_no);
            ht.Add("@TARGET_TYPE", target_type);
            ht.Add("@YM", ym.Replace("/", ""));
            ht.Add("@YM2", ym.Replace("/", "-"));
            ht.Add("@YMD", ym.Replace("/", "") + "01");
            ht.Add("@startYM", startYM);
            ht.Add("@endYM", endYM);
            ht.Add("@startYMD", startYM + "-01");
            ht.Add("@endYMD", endYM + "-01");
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
    public int getCount(int startRowIndex, int maximumRows, string dept_no, string target_type, string ym)
    {
        try
        {


            //若年月為會計年度, 依傳入的年月來判斷 該區間如2015/03-> 2014-04, 2015-03 2015/04-> 2015-04~2016-03
            /*
           if (tmp.Count() > 0)
           {
               int year = Convert.ToInt32(tmp[0]);
               int month = Convert.ToInt32(tmp[1]);
               if (month >= 4)
               {
                   startYM = Convert.ToString(year) + "-04";
                   endYM = Convert.ToString(year + 1) + "-03";
               }
           }
           */
            
            //20170206
            // 2016/09 -> 201604 ~ 201703
            // 2017/01 -> 201604 ~ 201703
            // 2017/05 -> 201704 ~ 201803
            //若月份介於1~3 : 開始年 = 傳入年-1,結束年 = 傳入年
            //若月份介於4~12: 開始年 = 傳件年,  結束年 = 傳件年+1
            string[] tmp = ym.Split('/');
            int year_start = 0;
            int year_end = 0;
            if (tmp.Count() > 0)
            {
                int year = Convert.ToInt32(tmp[0]);
                int month = Convert.ToInt32(tmp[1]);
                
                if (month < 4)
                {
                    year_start = year - 1;
                    year_end = year;
                }
                else
                {
                    year_start = year;
                    year_end = year + 1;
                }
            }
            string startYM = Convert.ToString(year_start) + "-04";
            string endYM = Convert.ToString(year_end) + "-03";



            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();


            sb.Append("select   COUNT(*) total_record ");
            sb.Append(@" from (  select A.EMP_ID ");
            sb.Append(@" from (  ");
            //若是當月用 VW_H_EMP_DATA, 其餘用 TB_H_R_EMP_DATA_MONTH
            string targetTableNAme = "";
            string extraCondition = "";
            if (Convert.ToInt32(ym.Replace("/", "")) < Convert.ToInt32(DateTime.Now.ToString("yyyyMM")))
            {
                targetTableNAme = " TB_H_R_EMP_DATA_MONTH A  with (nolock) ";
                extraCondition = " AND A.YM = @YM "; //月檔的年月條件
            }
            else
            {
                targetTableNAme = " VW_H_EMP_DATA A  with (nolock) ";
                extraCondition = "";
            }
            //個人資料
            sb.Append(@" SELECT  A.DEPT_NO,A.EMP_ID,A.OVERTIME_CTL_CD,A.PJOB_CD,A.WS_CD,A.WORK_CD "
                     + " from " + targetTableNAme
                    // + " left join TB_9_M_COMM_D D  with (nolock) on A.OVERTIME_CTL_CD = D.SUB_CD and D.SYS_CD = 'HB' and D.MAIN_CD = 'OVERTIME_CTL_CD'   "
                     + " inner join TB_D_M_OVERTIME_TARGET_EMP E  with (nolock) ON E.TARGET_TYPE=@TARGET_TYPE and E.TARGET_YEAR = @year "
                     + " inner join  ( SELECT DEPT_NO FROM  [dbo].[FN_H_GET_WORK_DEPT](@DEPT_NO) )F ON A.DEPT_NO=F.DEPT_NO  "
                     + " WHERE  (A.EMP_STATUS=('01') or (A.EMP_STATUS=('99') and substring(convert(char(10),DATEADD(DAY,-1,A.LEAVE_DT),120),0,8) =@YM2 ))  "
                     + " AND A.WORK_CD = E.WORK_CD AND A.WS_CD= E.WS_CD AND A.PJOB_CD=E.PJOB_CD "
                     +  extraCondition
                     + ") A "
                     );
           
            sb.Append(" Where A.EMP_ID is not null ");
            sb.Append("group by A.DEPT_NO,A.EMP_ID,A.OVERTIME_CTL_CD");
            sb.Append(") god_data ");


            ht.Add("@year", tmp[0]);
            ht.Add("@DEPT_NO", dept_no);
            ht.Add("@TARGET_TYPE", target_type);
            ht.Add("@YM", ym.Replace("/", ""));
            ht.Add("@YM2", ym.Replace("/", "-"));
            ht.Add("@startYM", startYM);
            ht.Add("@endYM", endYM);

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

    internal List<string> getDEPT_LIST()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("select DEPT_NO from VW_H_DEPT_DATA ");
            DataTable dt = dbConn.Query(sb, ht);
            List<string> Depts = new List<string>();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    Depts.Add(dt.Rows[i]["DEPT_NO"].ToString());
                }

            }
            return Depts;
        }
        catch (Exception)
        {

            throw;
        }
    }


    /*
    internal DataTable getTotalOvertimeData(string dept_no, string target_type, string ym)
    {
        try
        {
            string[] tmp = ym.Split('/');
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" select A.IS_APPLY,A.OVERTIME_DT_TYPE                                                                                             ");
            sb.Append("       ,isnull(sum(A.APPROVE_OVERTIME_HOUR), 0) APPROVE_OVERTIME_HOUR ");
            sb.Append(" from(                                                                                                                            ");
            sb.Append("     SELECT EMP_ID,SUM(APPROVE_OVERTIME_HOUR) APPROVE_OVERTIME_HOUR,OVERTIME_DT_TYPE ,IS_APPLY                                    ");
            sb.Append("     FROM TB_D_M_OVERTIME_APPLY A                                                                                                 ");
            sb.Append("     WHERE substring(convert(char(10),A.APPLY_OVERTIME_DT,120),0,8) =@YM                                                    ");
            sb.Append("       AND A.FORM_STATUS  not in ('N','D')  AND A.CHECK_STATUS ='Y' ");
            sb.Append("     group by EMP_ID,OVERTIME_DT_TYPE,IS_APPLY                                                                                    ");
            sb.Append("     ) A                                                                                                                          ");
            sb.Append(" LEFT join (                                                                                                                      ");
            //本月以前(不包含當月)
            if (Convert.ToInt32(ym.Replace("/", "")) < Convert.ToInt32(DateTime.Now.ToString("yyyyMM")))
            {
                sb.Append("     SELECT  A.EMP_ID                                                                                                             ");
                sb.Append("     FROM TB_H_R_EMP_DATA_MONTH  A                                                                                                ");
                sb.Append("     left join TB_H_M_DEPT_ORG B on A.DEPT_NO = B.DEPT_NO AND B.END_DT >= GETDATE() and B.UP_DEPT_NO = @DEPT_NO                  ");
                sb.Append("     inner join TB_D_M_OVERTIME_TARGET_EMP E ON E.TARGET_TYPE=@TARGET_TYPE and E.TARGET_YEAR=@year                  ");
                sb.Append("     WHERE  (A.EMP_STATUS=('01') or (A.EMP_STATUS=('99') and substring(convert(char(10),A.LEAVE_DT,120),0,8) =@YM2 ))  ");
                sb.Append("     AND (B.UP_DEPT_NO = @DEPT_NO or A.DEPT_NO_20 = @DEPT_NO )                                                                     ");
                sb.Append("     AND A.WORK_CD = E.WORK_CD AND A.WS_CD= E.WS_CD AND A.PJOB_CD=E.PJOB_CD AND A.YM = @YM2               ");
            }
            else
            {
                sb.Append("     SELECT  A.EMP_ID                                                                                                             ");
                sb.Append("     FROM VW_H_EMP_DATA  A                                                                                                ");
                sb.Append("     left join TB_H_M_DEPT_ORG B on A.DEPT_NO = B.DEPT_NO AND B.END_DT >= GETDATE() and B.UP_DEPT_NO = @DEPT_NO                  ");
                sb.Append("     inner join TB_D_M_OVERTIME_TARGET_EMP E ON E.TARGET_TYPE=@TARGET_TYPE and E.TARGET_YEAR=@year                  ");
                sb.Append("     WHERE  (A.EMP_STATUS=('01') or (A.EMP_STATUS=('99') and substring(convert(char(10),A.LEAVE_DT,120),0,8) =@YM2 ))  ");
                sb.Append("     AND (B.UP_DEPT_NO = @DEPT_NO or A.DEPT_NO_20 = @DEPT_NO )                                                                     ");
                sb.Append("     AND A.WORK_CD = E.WORK_CD AND A.WS_CD= E.WS_CD AND A.PJOB_CD=E.PJOB_CD  ");
            }
            sb.Append("            ) C on A.EMP_ID = C.EMP_ID                                                                                            ");
            sb.Append(" Where C.EMP_ID is not null                                                                                                       ");
            sb.Append(" group by A.OVERTIME_DT_TYPE,A.IS_APPLY,C.EMP_ID                                                                                  ");

            ht.Add("@year", tmp[0]);
            ht.Add("@YM2", ym.Replace("/", ""));
            ht.Add("@DEPT_NO", dept_no);
            ht.Add("@TARGET_TYPE", target_type);
            ht.Add("@YM", ym.Replace("/", "-"));

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
    */
    internal DataTable getLeaveData(string dept_no, string target_type, string ym)
    {
        try
        {
            string[] tmp = ym.Split('/');
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" select isnull(sum(A.TOTAL_TIME_APPROVE), 0) TOTAL_TIME_APPROVE                                                   ");
            sb.Append(" from(                                                                                                            ");
            sb.Append("     select EMP_ID,sum(TOTAL_TIME_APPROVE) TOTAL_TIME_APPROVE                                                     ");
            sb.Append("     from TB_D_M_LEAVE_APPLY_DAY A                                                                                ");
            sb.Append("     Where FORM_STATUS NOT IN ('N','D')                                                                           ");
            sb.Append("     and A.SUB_LEAVE_CD in ('Z0') AND A.CHECK_STATUS ='Y'                                                                                 ");
            sb.Append("     and substring(convert(char(10),A.APPLY_LEAVE_SDT,120),0,8) = @YM group by EMP_ID                       ");
            sb.Append(" ) A                                                                                                              ");
            sb.Append(" LEFT join (                                                                                                      ");
            //本月以前(不包含當月)
            if (Convert.ToInt32(ym.Replace("/", "")) < Convert.ToInt32(DateTime.Now.ToString("yyyyMM")))
            {
                sb.Append("     SELECT  A.EMP_ID                                                                                                             ");
                sb.Append("     FROM TB_H_R_EMP_DATA_MONTH  A                                                                                                ");
                sb.Append("     left join TB_H_M_DEPT_ORG B on A.DEPT_NO = B.DEPT_NO AND B.END_DT >= GETDATE() and B.UP_DEPT_NO = @DEPT_NO                  ");
                sb.Append("     inner join TB_D_M_OVERTIME_TARGET_EMP E ON E.TARGET_TYPE=@TARGET_TYPE and E.TARGET_YEAR=@year                  ");
                sb.Append("     WHERE  (A.EMP_STATUS=('01') or (A.EMP_STATUS=('99') and substring(convert(char(10),A.LEAVE_DT,120),0,8) =@YM2 ))");
                sb.Append("     AND (B.UP_DEPT_NO = @DEPT_NO or A.DEPT_NO_20 = @DEPT_NO )                                                                     ");
                sb.Append("     AND A.WORK_CD = E.WORK_CD AND A.WS_CD= E.WS_CD AND A.PJOB_CD=E.PJOB_CD AND A.YM = @YM2                                                                ");
            }
            else
            {
                sb.Append("     SELECT  A.EMP_ID                                                                                                             ");
                sb.Append("     FROM VW_H_EMP_DATA  A                                                                                                ");
                sb.Append("     left join TB_H_M_DEPT_ORG B on A.DEPT_NO = B.DEPT_NO AND B.END_DT >= GETDATE() and B.UP_DEPT_NO = @DEPT_NO                  ");
                sb.Append("     inner join TB_D_M_OVERTIME_TARGET_EMP E ON E.TARGET_TYPE=@TARGET_TYPE and E.TARGET_YEAR=@year                  ");
                sb.Append("     WHERE  (A.EMP_STATUS=('01') or (A.EMP_STATUS=('99') and substring(convert(char(10),A.LEAVE_DT,120),0,8) =@YM2 ))");
                sb.Append("     AND (B.UP_DEPT_NO = @DEPT_NO or A.DEPT_NO_20 = @DEPT_NO )                                                                     ");
                sb.Append("     AND A.WORK_CD = E.WORK_CD AND A.WS_CD= E.WS_CD AND A.PJOB_CD=E.PJOB_CD                                                                 ");
            }
            sb.Append(" ) D on A.EMP_ID = D.EMP_ID                                                                                       ");
            sb.Append(" Where D.EMP_ID is not null                                                                                       ");

            ht.Add("@year", tmp[0]);
            ht.Add("@DEPT_NO", dept_no);
            ht.Add("@TARGET_TYPE", target_type);
            ht.Add("@YM", ym.Replace("/", "-"));
            ht.Add("@YM2", ym.Replace("/", ""));



            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getOvertimeTargetData(string dept_no, string target_type, string ym)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            string[] tmp = ym.Split('/');
            if (tmp.Count() > 0)
            {
                sb.Append(" select ");
                switch (tmp[1])
                {
                    case "01":
                        sb.Append(" TARGET_VALUE_01");
                        break;
                    case "02":
                        sb.Append(" TARGET_VALUE_02");
                        break;
                    case "03":
                        sb.Append(" TARGET_VALUE_03");
                        break;
                    case "04":
                        sb.Append(" TARGET_VALUE_04");
                        break;
                    case "05":
                        sb.Append(" TARGET_VALUE_05");
                        break;
                    case "06":
                        sb.Append(" TARGET_VALUE_06");
                        break;
                    case "07":
                        sb.Append(" TARGET_VALUE_07");
                        break;
                    case "08":
                        sb.Append(" TARGET_VALUE_08");
                        break;
                    case "09":
                        sb.Append(" TARGET_VALUE_09");
                        break;
                    case "10":
                        sb.Append(" TARGET_VALUE_10");
                        break;
                    case "11":
                        sb.Append(" TARGET_VALUE_11");
                        break;
                    case "12":
                        sb.Append(" TARGET_VALUE_12");
                        break;
                    default:
                        break;
                }
                sb.Append(" TARGET_VALUE");
                sb.Append(" from TB_D_M_OVERTIME_TARGET where DEPT_NO like @DEPT_NO and TARGET_TYPE = @TARGET_TYPE and TARGET_YEAR = @TARGET_YEAR");
                ht.Add("@DEPT_NO", dept_no + '%');
                ht.Add("@TARGET_TYPE", target_type);
                ht.Add("@TARGET_YEAR", tmp[0]);
                return dbConn.Query(sb, ht);
            }
            else
            {
                return new DataTable();
            }

        }
        catch (Exception)
        {

            throw;
        }
    }
    /*
    internal DataTable getTotalEmp(string dept_no, string target_type, string ym)
    {
        try
        {
            string[] tmp = ym.Split('/');
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            if (Convert.ToInt32(ym.Replace("/", "")) < Convert.ToInt32(DateTime.Now.ToString("yyyyMM")))
            {
                sb.Append(" select count(A.EMP_ID) TOTAL_EMP_ID                                                                            ");
                sb.Append(" FROM TB_H_R_EMP_DATA_MONTH A                                                                                   ");
                sb.Append(" inner join TB_D_M_OVERTIME_TARGET_EMP E ON E.TARGET_TYPE=@TARGET_TYPE and E.TARGET_YEAR=@year");
                sb.Append(" left join TB_H_M_DEPT_ORG B on A.DEPT_NO = B.DEPT_NO AND B.END_DT >= GETDATE() and B.UP_DEPT_NO = @DEPT_NO ");
                sb.Append(" left join ( SELECT EMP_ID,SUM(APPROVE_OVERTIME_HOUR) APPROVE_OVERTIME_HOUR                                     ");
                sb.Append("               FROM TB_D_M_OVERTIME_APPLY A                                                                     ");
                sb.Append("              WHERE substring(convert(char(10),A.APPLY_OVERTIME_DT,120),0,8) =@YM2                         ");
                sb.Append("                AND  A.FORM_STATUS  not in ('N','D')                                                            ");
                sb.Append("                AND  A.OVERTIME_DT_TYPE = '1'                                                                   ");
                sb.Append("              group by EMP_ID ) D on A.EMP_ID = D.EMP_ID                                                        ");
                sb.Append(" WHERE (A.EMP_STATUS=('01') or (A.EMP_STATUS=('99') and substring(convert(char(10),A.LEAVE_DT,120),0,8) =@YM2 )) ");
                sb.Append("   AND (B.UP_DEPT_NO = @DEPT_NO or A.DEPT_NO_20 = @DEPT_NO )                                                     ");
                sb.Append("   AND A.WORK_CD = E.WORK_CD AND A.WS_CD= E.WS_CD AND A.PJOB_CD=E.PJOB_CD AND A.YM = @YM ");
            }
            else
            {
                sb.Append(" select count(A.EMP_ID) TOTAL_EMP_ID                                                                            ");
                sb.Append(" FROM VW_H_EMP_DATA A                                                                                   ");
                sb.Append(" inner join TB_D_M_OVERTIME_TARGET_EMP E ON E.TARGET_TYPE=@TARGET_TYPE and E.TARGET_YEAR=@year ");
                sb.Append(" left join TB_H_M_DEPT_ORG B on A.DEPT_NO = B.DEPT_NO AND B.END_DT >= GETDATE() and B.UP_DEPT_NO = @DEPT_NO ");
                sb.Append(" left join ( SELECT EMP_ID,SUM(APPROVE_OVERTIME_HOUR) APPROVE_OVERTIME_HOUR                                     ");
                sb.Append("               FROM TB_D_M_OVERTIME_APPLY A                                                                     ");
                sb.Append("              WHERE substring(convert(char(10),A.APPLY_OVERTIME_DT,120),0,8) =@YM2                         ");
                sb.Append("                AND  A.FORM_STATUS  not in ('N','D')                                                            ");
                sb.Append("                AND  A.OVERTIME_DT_TYPE = '1'                                                                   ");
                sb.Append("              group by EMP_ID ) D on A.EMP_ID = D.EMP_ID                                                        ");
                sb.Append(" WHERE (A.EMP_STATUS=('01') or (A.EMP_STATUS=('99') and substring(convert(char(10),A.LEAVE_DT,120),0,8) =@YM2 )) ");
                sb.Append("   AND (B.UP_DEPT_NO = @DEPT_NO or A.DEPT_NO_20 = @DEPT_NO )                                                     ");
                sb.Append("   AND A.WORK_CD = E.WORK_CD AND A.WS_CD= E.WS_CD AND A.PJOB_CD=E.PJOB_CD                                           ");
            }


            ht.Add("@year", tmp[0]);
            ht.Add("@YM", ym.Replace("/", ""));
            ht.Add("@YM2", ym.Replace("/", "-"));
            ht.Add("@TARGET_TYPE", target_type);
            ht.Add("@DEPT_NO", dept_no);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
       */
    internal DataTable getdata(string emp_id, string ym)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            //sb.Append(" select A.EMP_NAME,A.DEPT_NO,A.DEPT_NAME from VW_H_EMP_DATA A");
            //sb.Append("  LEFT JOIN  TB_H_R_EMP_DATA_MONTH  B  ON A.EMP_ID = B.EMP_ID  and  B.YM = @YM and B.EMP_STATUS=('01') and A.EMP_ID = @EMP_ID");

            //ht.Add("@EMP_ID", emp_id);
            //ht.Add("@YM", ym);

            sb.Append("select EMP_ID,EMP_NAME,DEPT_NO,DEPT_NAME ");
            sb.Append(" from VW_H_EMP_DATA ");
            sb.Append(" where EMP_ID=@EMP_ID ");
            ht.Add("@EMP_ID", emp_id);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
 

    //Gridview 查詢資料
    public DataTable getDtlData(int startRowIndex, int maximumRows, string sortExpression, string emp_id, string ym)
    {
        try
        {
            if (sortExpression.Contains("EMP_ID"))
            {
                sortExpression = sortExpression.Replace("EMP_ID", "B.EMP_ID");
            }
            if (sortExpression.Contains("DEPT_NO"))
            {
                sortExpression = sortExpression.Replace("DEPT_NO", "B.DEPT_NO");
            }
            if (sortExpression.Contains("OVERTIME_CD"))
            {
                sortExpression = sortExpression.Replace("OVERTIME_CD", "A.OVERTIME_CD");
            }
            if (sortExpression.Contains("OVERTIME_DT_TYPE"))
            {
                sortExpression = sortExpression.Replace("OVERTIME_DT_TYPE", "A.OVERTIME_DT_TYPE");
            }
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" replace(convert(varchar(10),APPLY_OVERTIME_DT,120),'-','/') APPLY_OVERTIME_DT,");
            sb.Append(" A.OVERTIME_CD,A.OVERTIME_DT_TYPE,OVERTIME_TIME_CD,");
            sb.Append(" CASE WHEN CHECK_STATUS ='Y' THEN CAST(ROUND(((convert(decimal,APPROVE_OVERTIME_HOUR, 0)) / 60), 2)AS DECIMAL(12, 2)) ELSE 0 END APPROVE_OVERTIME_HOUR,");
            sb.Append(" CAST(ROUND(((convert(decimal,BEFORE_HOUR, 0)) / 60), 2) AS DECIMAL(12, 2)) BEFORE_HOUR,");
            sb.Append(" convert(varchar(5),BEFORE_STIME,108) + '~' + convert(varchar(5),BEFORE_ETIME,108) BEFORE_SETIME, ");
            sb.Append(" CAST(ROUND(((convert(decimal,AFTER_HOUR, 0)) / 60), 2) AS DECIMAL(12, 2)) AFTER_HOUR,");
            sb.Append(" convert(varchar(5),AFTER_STIME,108) + '~' + convert(varchar(5),AFTER_ETIME,108) AFTER_SETIME, ");
            sb.Append(" replace(convert(varchar(7),IFLOW_APPROVE_DT,120),'-','/') IFLOW_APPROVE_DT,replace(convert(varchar(10),PAY_DT,120),'-','/') PAY_DT,IFLOW_NO");
            sb.Append(" , D.OVERTIME_CD+'-'+D.OVERTIME_DESC OVERTIME_DESC ");
            sb.Append(" ,E.SUB_CD+'-'+E.SUB_DESC OVERTIME_TIME_DESC ");
            sb.Append(" ,f.SUB_CD+'-'+f.SUB_DESC FORM_STATUS ");
            sb.Append(" , g.SUB_CD+'-'+g.SUB_DESC CHECK_STATUS ");
            sb.Append(" , h.SUB_CD+'-'+h.SUB_DESC SALARY_SETTLE_STATUS ");
            sb.Append(" FROM TB_D_M_OVERTIME_APPLY A");
            //sb.Append(" left join TB_D_M_OVERTIME_TYPE D ON A.OVERTIME_CD = D.OVERTIME_CD AND A.OVERTIME_DT_TYPE = D.OVERTIME_DT_TYPE ");
            sb.Append(" left join TB_D_M_OVERTIME_TYPE D ON A.OVERTIME_CD = D.OVERTIME_CD  ");
            sb.Append(" left join TB_9_M_COMM_D E ON A.OVERTIME_TIME_CD = E.SUB_CD and E.SYS_CD = 'DI' and E.MAIN_CD = 'OVERTIME_TIME_CD'");
            sb.Append(" left join TB_9_M_COMM_D f on f.main_cd = 'FORM_STATUS' and f.sys_cd = 'DH' and f.IS_VALID='Y' and a.FORM_STATUS=f.SUB_CD ");
            sb.Append(" left join TB_9_M_COMM_D g on g.main_cd = 'CHECK_STATUS' and g.sys_cd = 'DI' and g.IS_VALID='Y' and a.CHECK_STATUS=g.SUB_CD ");
            sb.Append(" left join TB_9_M_COMM_D h on h.main_cd = 'SALARY_SETTLE_STATUS' and h.sys_cd = 'DH' and h.IS_VALID='Y' and a.SALARY_SETTLE_STATUS=h.SUB_CD ");
            sb.Append(" WHERE substring(convert(char(10),A.APPLY_OVERTIME_DT,120),0,8) =@YM2 AND A.EMP_ID = @EMP_ID");
            sb.Append(" AND A.FORM_STATUS NOT IN('N','D') ");
            ht.Add("@EMP_ID", emp_id);
            ht.Add("@YM", ym.Replace("/", ""));
            ht.Add("@YM2", ym.Replace("/", "-"));


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



    //Gridview 查詢總筆數
    public int getDtlCount(int startRowIndex, int maximumRows, string emp_id, string ym)
    {
        try
        {

            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select COUNT(*) total_record ");
            sb.Append(" FROM TB_D_M_OVERTIME_APPLY A");
            //sb.Append(" left join TB_D_M_OVERTIME_TYPE D ON A.OVERTIME_CD = D.OVERTIME_CD AND A.OVERTIME_DT_TYPE = D.OVERTIME_DT_TYPE ");
            sb.Append(" left join TB_D_M_OVERTIME_TYPE D ON A.OVERTIME_CD = D.OVERTIME_CD ");
            sb.Append(" left join TB_9_M_COMM_D E ON A.OVERTIME_TIME_CD = E.SUB_CD and E.SYS_CD = 'DI' and E.MAIN_CD = 'OVERTIME_TIME_CD'");
            sb.Append(" WHERE substring(convert(char(10),A.APPLY_OVERTIME_DT,120),0,8) =@YM2 AND A.EMP_ID = @EMP_ID");
            sb.Append(" AND A.FORM_STATUS NOT IN('N','D') ");
            ht.Add("@EMP_ID", emp_id);


            ht.Add("@YM", ym.Replace("/", ""));

            ht.Add("@YM2", ym.Replace("/", "-"));


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


    //Gridview 查詢資料 (刷卡超時)
    public DataTable getAb1Data(int startRowIndex, int maximumRows, string sortExpression, string dept_no, string target_type, string ym)
    {
        try
        {
            if (sortExpression.Contains("EMP_ID"))
            {
                sortExpression = sortExpression.Replace("EMP_ID", "A.EMP_ID");
            }
            if (sortExpression.Contains("DEPT_NO"))
            {
                sortExpression = sortExpression.Replace("DEPT_NO", "B.DEPT_NO");
            }
            if (sortExpression.Contains("SHIFT_CD"))
            {
                sortExpression = sortExpression.Replace("SHIFT_CD", "A.SHIFT_CD");
            }
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" B.DEPT_NO+'-'+B.DEPT_NAME as DEPT_NO,A.EMP_ID,B.EMP_NAME,replace(convert(varchar(10),A.CALENDAR_DT,120),'-','/') CALENDAR_DT,A.SHIFT_CD,D.SHIFT_DESC,");
            sb.Append(" replace(convert(varchar(20),CLOCK_IN_DT,120),'-','/') + ' ~ ' + replace(convert(varchar(20),CLOCK_OUT_DT,120),'-','/') CLOCK_IN_OUT_DT,");
            sb.Append(" CAST(ROUND(((convert(decimal,VIOLATE_BEFORE_HOUR, 0)) / 60), 2) AS DECIMAL(12, 2)) VIOLATE_BEFORE_HOUR,");
            sb.Append(" CAST(ROUND(((convert(decimal,VIOLATE_AFTER_HOUR, 0)) / 60), 2) AS DECIMAL(12, 2)) VIOLATE_AFTER_HOUR");
            sb.Append(" FROM TB_D_M_EMP_DUTY_CHECK_STATUS  A");
            sb.Append(" inner join (SELECT  A.EMP_ID, A.EMP_NAME, A.EMP_STATUS, A.PLANT_CD, A.DEPT_NO_20, A.DEPT_NO, A.PJOB_CD, A.WS_CD	");
            sb.Append("	  , A.OVERTIME_CTL_CD, D.SUB_DESC + '/' + D.CODE_VAL1 OVERTIME_CTL_DESC, C.DEPT_NAME ");
            sb.Append(" FROM VW_H_EMP_DATA  A ");
            sb.Append(" left join TB_H_M_DEPT_ORG B on A.DEPT_NO = B.DEPT_NO AND B.END_DT >= GETDATE() ");
            sb.Append(" left join TB_H_M_DEPT C on A.DEPT_NO = C.DEPT_NO AND C.END_DT >= GETDATE()  ");
            sb.Append(" left join TB_9_M_COMM_D D on A.OVERTIME_CTL_CD = D.SUB_CD and D.SYS_CD = 'HB' and D.MAIN_CD = 'OVERTIME_CTL_CD' ");
            sb.Append(" inner join TB_D_M_OVERTIME_TARGET_EMP E ON E.TARGET_TYPE= @TARGET_TYPE and E.TARGET_YEAR= @YEAR ");
            sb.Append(" WHERE  (A.EMP_STATUS=('01') or (A.EMP_STATUS=('99')and convert(char(7),A.LEAVE_DT,111) = @YM ))  ");
            sb.Append(" AND  A.DEPT_NO in ( select dept_no from [dbo].[FN_H_GET_WORK_DEPT](@DEPT_NO)  ) ");
            sb.Append("		AND A.WORK_CD = E.WORK_CD AND A.WS_CD= E.WS_CD AND A.PJOB_CD=E.PJOB_CD ) B  ");
            sb.Append(" 		ON A.EMP_ID = B.EMP_ID  ");

            sb.Append(" left join VW_D_M_SHIFT_H D on A.SHIFT_CD = D.SHIFT_CD ");
            sb.Append(" WHERE convert(char(7),A.CALENDAR_DT,111) =@YM ");
            sb.Append(" AND  (A.VIOLATE_BEFORE_HOUR+A.VIOLATE_AFTER_HOUR)>0");

            ht.Add("@DEPT_NO", dept_no);
            ht.Add("@YM", ym);
            ht.Add("@YEAR", ym.Substring(0, 4));
            ht.Add("@TARGET_TYPE", target_type);
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
    //Gridview 查詢總筆數
    public int getAb1Count(int startRowIndex, int maximumRows, string dept_no, string target_type, string ym)
    {
        try
        {

            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append(" Select COUNT(*) total_record ");
            sb.Append(" FROM TB_D_M_EMP_DUTY_CHECK_STATUS  A");
            sb.Append(" inner join (SELECT  A.EMP_ID, A.EMP_NAME, A.EMP_STATUS, A.PLANT_CD, A.DEPT_NO_20, A.DEPT_NO, A.PJOB_CD, A.WS_CD	");
            sb.Append("	  , A.OVERTIME_CTL_CD, D.SUB_DESC + '/' + D.CODE_VAL1 OVERTIME_CTL_DESC, C.DEPT_NAME ");
            sb.Append(" FROM VW_H_EMP_DATA  A ");
            sb.Append(" left join TB_H_M_DEPT_ORG B on A.DEPT_NO = B.DEPT_NO AND B.END_DT >= GETDATE()  ");
            sb.Append(" left join TB_H_M_DEPT C on A.DEPT_NO = C.DEPT_NO AND C.END_DT >= GETDATE()  ");
            sb.Append(" left join TB_9_M_COMM_D D on A.OVERTIME_CTL_CD = D.SUB_CD and D.SYS_CD = 'HB' and D.MAIN_CD = 'OVERTIME_CTL_CD' ");
            sb.Append(" inner join TB_D_M_OVERTIME_TARGET_EMP E ON E.TARGET_TYPE= @TARGET_TYPE and E.TARGET_YEAR= @YEAR ");
            sb.Append(" WHERE  (A.EMP_STATUS=('01') or (A.EMP_STATUS=('99')and convert(char(7),A.LEAVE_DT,111) = @YM ))  ");
            sb.Append(" AND  A.DEPT_NO in ( select dept_no from [dbo].[FN_H_GET_WORK_DEPT](@DEPT_NO)  ) ");
            sb.Append("		AND A.WORK_CD = E.WORK_CD AND A.WS_CD= E.WS_CD AND A.PJOB_CD=E.PJOB_CD ) B  ");
            sb.Append(" 		ON A.EMP_ID = B.EMP_ID  ");

            sb.Append(" left join VW_D_M_SHIFT_H D on A.SHIFT_CD = D.SHIFT_CD ");
            sb.Append(" WHERE convert(char(7),A.CALENDAR_DT,111) =@YM ");
            sb.Append(" AND  (A.VIOLATE_BEFORE_HOUR+A.VIOLATE_AFTER_HOUR)>0");

            ht.Add("@DEPT_NO", dept_no);
            ht.Add("@YM", ym);
            ht.Add("@YEAR", ym.Substring(0, 4));
            ht.Add("@TARGET_TYPE", target_type);

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


    //(1).平日加班時數>4  Gridview 查詢資料
    public DataTable getAb2Data1(int startRowIndex, int maximumRows, string sortExpression, string dept_no, string target_type, string ym)
    {
        try
        {
            if (sortExpression.Contains("EMP_ID"))
            {
                sortExpression = sortExpression.Replace("EMP_ID", "A.EMP_ID");
            }
            if (sortExpression.Contains("EMP_NAME"))
            {
                sortExpression = sortExpression.Replace("EMP_NAME", "B.EMP_ID");
            }
            if (sortExpression.Contains("DEPT_NO"))
            {
                sortExpression = sortExpression.Replace("DEPT_NO", "B.DEPT_NO");
            }
            if (sortExpression.Contains("SHIFT_CD"))
            {
                sortExpression = sortExpression.Replace("SHIFT_CD", "A.SHIFT_CD");
            }
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select * From");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" B.DEPT_NO+'-'+B.DEPT_NAME as DEPT_NO,A.EMP_ID,B.EMP_NAME,");
            sb.Append(" replace(convert(varchar(10),A.APPLY_OVERTIME_DT,120),'-','/') APPLY_OVERTIME_DT, ");
            sb.Append(" CAST(ROUND(((convert(decimal,A.APPROVE_OVERTIME_HOUR_LIMIT, 0)) / 60), 2) AS DECIMAL(12, 2)) APPROVE_OVERTIME_HOUR");
            //sb.Append(" FROM TB_D_M_OVERTIME_APPLY A ");
            //若加班日期類型 為2假日時,需-8小時
            sb.Append(" FROM ( ");
            sb.Append(" select  case when OVERTIME_DT_TYPE=2  ");
            sb.Append("   then  IIF(OVERTIME_PAY_HOUR-8*60>0, OVERTIME_PAY_HOUR-8*60,0) ");
            sb.Append("   else  OVERTIME_PAY_HOUR ");
            sb.Append("   end	   as  APPROVE_OVERTIME_HOUR_LIMIT");
            sb.Append("   ,* ");
            sb.Append("   from TB_D_M_OVERTIME_APPLY ");
            sb.Append(" ) A ");
            sb.Append("   inner join (SELECT  A.EMP_ID, A.EMP_NAME, A.EMP_STATUS, A.PLANT_CD, A.DEPT_NO_20, A.DEPT_NO, A.PJOB_CD, A.WS_CD ");
            sb.Append(" 	  , A.OVERTIME_CTL_CD, D.SUB_DESC + '/' + D.CODE_VAL1 OVERTIME_CTL_DESC, C.DEPT_NAME ");
            sb.Append(" FROM VW_H_EMP_DATA  A ");
            sb.Append(" left join TB_H_M_DEPT_ORG B on A.DEPT_NO = B.DEPT_NO AND B.END_DT >= GETDATE() ");
            sb.Append(" left join TB_H_M_DEPT C on A.DEPT_NO = C.DEPT_NO AND C.END_DT >= GETDATE()  ");
            sb.Append(" left join TB_9_M_COMM_D D on A.OVERTIME_CTL_CD = D.SUB_CD and D.SYS_CD = 'HB' and D.MAIN_CD = 'OVERTIME_CTL_CD'  ");
            sb.Append(" inner join TB_D_M_OVERTIME_TARGET_EMP E ON E.TARGET_TYPE= @TARGET_TYPE and E.TARGET_YEAR= @YEAR   ");
            sb.Append(" WHERE  (A.EMP_STATUS=('01') or (A.EMP_STATUS=('99')and convert(char(7),A.LEAVE_DT,111) =@YM ))  ");
            sb.Append(" AND  A.DEPT_NO in ( select dept_no from [dbo].[FN_H_GET_WORK_DEPT](@DEPT_NO)  ) ");
            sb.Append(" AND A.WORK_CD = E.WORK_CD AND A.WS_CD= E.WS_CD AND A.PJOB_CD=E.PJOB_CD ) B ");
            sb.Append(" ON A.EMP_ID = B.EMP_ID ");
            sb.Append(" WHERE convert(char(7),A.APPLY_OVERTIME_DT,111) =@YM  ");
            sb.Append(" AND  A.FORM_STATUS not in ('N','D')  ");
            sb.Append(" AND  A.CHECK_STATUS='Y' ");
            sb.Append(" AND  A.OVERTIME_DT_TYPE = ('1') ");
            sb.Append(" AND  ((convert(decimal,A.OVERTIME_PAY_HOUR, 0)) / 60)>4");

            ht.Add("@DEPT_NO", dept_no);
            ht.Add("@YM", ym);
            ht.Add("@YEAR", ym.Substring(0, 4));
            ht.Add("@TARGET_TYPE", target_type);

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
    //(1).平日加班時數>4  Gridview 查詢總筆數
    public int getAb2Count1(int startRowIndex, int maximumRows, string dept_no, string target_type, string ym)
    {
        try
        {
            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("Select COUNT(*) total_record ");
            sb.Append(" FROM TB_D_M_OVERTIME_APPLY A ");
            sb.Append("   inner join (SELECT  A.EMP_ID, A.EMP_NAME, A.EMP_STATUS, A.PLANT_CD, A.DEPT_NO_20, A.DEPT_NO, A.PJOB_CD, A.WS_CD ");
            sb.Append(" 	  , A.OVERTIME_CTL_CD, D.SUB_DESC + '/' + D.CODE_VAL1 OVERTIME_CTL_DESC, C.DEPT_NAME ");
            sb.Append(" FROM VW_H_EMP_DATA  A ");
            sb.Append(" left join TB_H_M_DEPT_ORG B on A.DEPT_NO = B.DEPT_NO AND B.END_DT >= GETDATE()  ");
            sb.Append(" left join TB_H_M_DEPT C on A.DEPT_NO = C.DEPT_NO AND C.END_DT >= GETDATE()  ");
            sb.Append(" left join TB_9_M_COMM_D D on A.OVERTIME_CTL_CD = D.SUB_CD and D.SYS_CD = 'HB' and D.MAIN_CD = 'OVERTIME_CTL_CD'  ");
            sb.Append(" inner join TB_D_M_OVERTIME_TARGET_EMP E ON E.TARGET_TYPE= @TARGET_TYPE and E.TARGET_YEAR= @YEAR   ");
            sb.Append(" WHERE  (A.EMP_STATUS=('01') or (A.EMP_STATUS=('99')and convert(char(7),A.LEAVE_DT,111) =@YM ))  ");
            sb.Append(" AND  A.DEPT_NO in ( select dept_no from [dbo].[FN_H_GET_WORK_DEPT](@DEPT_NO)  ) ");
            sb.Append(" AND A.WORK_CD = E.WORK_CD AND A.WS_CD= E.WS_CD AND A.PJOB_CD=E.PJOB_CD ) B ");
            sb.Append(" ON A.EMP_ID = B.EMP_ID ");
            sb.Append(" WHERE convert(char(7),A.APPLY_OVERTIME_DT,111) =@YM  ");
            sb.Append(" AND  A.FORM_STATUS not in ('N','D')  ");
            sb.Append(" AND  A.CHECK_STATUS='Y' ");
            sb.Append(" AND  A.OVERTIME_DT_TYPE = ('1') ");
            sb.Append(" AND  ((convert(decimal,A.OVERTIME_PAY_HOUR, 0)) / 60)>4");

            ht.Add("@DEPT_NO", dept_no);
            ht.Add("@YM", ym);
            ht.Add("@YEAR", ym.Substring(0, 4));
            ht.Add("@TARGET_TYPE", target_type);

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

    //(2).假日加班時數>12  查詢資料
    public DataTable getAb2Data2(int startRowIndex, int maximumRows, string sortExpression, string dept_no, string target_type, string ym)
    {
        try
        {

            if (sortExpression.Contains("EMP_ID"))
            {
                sortExpression = sortExpression.Replace("EMP_ID", "A.EMP_ID");
            }
            if (sortExpression.Contains("EMP_NAME"))
            {
                sortExpression = sortExpression.Replace("EMP_NAME", "B.EMP_ID");
            }
            if (sortExpression.Contains("DEPT_NO"))
            {
                sortExpression = sortExpression.Replace("DEPT_NO", "B.DEPT_NO");
            }
            if (sortExpression.Contains("SHIFT_CD"))
            {
                sortExpression = sortExpression.Replace("SHIFT_CD", "A.SHIFT_CD");
            }
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select * From ");
            sb.Append(" (Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" B.DEPT_NO+'-'+B.DEPT_NAME as DEPT_NO,A.EMP_ID,B.EMP_NAME,");
            sb.Append(" replace(convert(varchar(10),A.APPLY_OVERTIME_DT,120),'-','/') APPLY_OVERTIME_DT, ");
            sb.Append(" CAST(ROUND(((convert(decimal,A.OVERTIME_PAY_HOUR, 0)) / 60), 2) AS DECIMAL(12, 2)) APPROVE_OVERTIME_HOUR");
            sb.Append(" FROM TB_D_M_OVERTIME_APPLY A ");
            sb.Append(" inner join (SELECT  A.EMP_ID, A.EMP_NAME, A.EMP_STATUS, A.PLANT_CD, A.DEPT_NO_20, A.DEPT_NO, A.PJOB_CD, A.WS_CD ");
            sb.Append(" 	  , A.OVERTIME_CTL_CD, D.SUB_DESC + '/' + D.CODE_VAL1 OVERTIME_CTL_DESC, C.DEPT_NAME ");
            sb.Append(" FROM VW_H_EMP_DATA  A ");
            sb.Append(" left join TB_H_M_DEPT_ORG B on A.DEPT_NO = B.DEPT_NO AND B.END_DT >= GETDATE() ");
            sb.Append(" left join TB_H_M_DEPT C on A.DEPT_NO = C.DEPT_NO AND C.END_DT >= GETDATE() ");
            sb.Append(" left join TB_9_M_COMM_D D on A.OVERTIME_CTL_CD = D.SUB_CD and D.SYS_CD = 'HB' and D.MAIN_CD = 'OVERTIME_CTL_CD' ");
            sb.Append(" inner join TB_D_M_OVERTIME_TARGET_EMP E ON E.TARGET_TYPE= @TARGET_TYPE and E.TARGET_YEAR=@YEAR ");
            sb.Append(" WHERE  (A.EMP_STATUS=('01') or (A.EMP_STATUS=('99')and convert(char(7),A.LEAVE_DT,111) =@YM )) ");
            sb.Append(" AND  A.DEPT_NO in ( select dept_no from [dbo].[FN_H_GET_WORK_DEPT](@DEPT_NO)  ) ");
            sb.Append("		AND A.WORK_CD = E.WORK_CD AND A.WS_CD= E.WS_CD AND A.PJOB_CD=E.PJOB_CD ) B ");
            sb.Append("		ON A.EMP_ID = B.EMP_ID ");
            sb.Append(" WHERE convert(char(7),A.APPLY_OVERTIME_DT,111) =@YM ");
            sb.Append(" AND A.FORM_STATUS not in ('N','D')  ");
            sb.Append(" AND A.CHECK_STATUS='Y' ");
            sb.Append(" AND A.OVERTIME_DT_TYPE = ('2') ");

            sb.Append(" AND  ((convert(decimal,A.OVERTIME_PAY_HOUR, 0)) / 60)>12");

            ht.Add("@DEPT_NO", dept_no);
            ht.Add("@YM", ym);
            ht.Add("@YEAR", ym.Substring(0, 4));
            ht.Add("@TARGET_TYPE", target_type);


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
    //(2).假日加班時數>12  Gridview 查詢總筆數
    public int getAb2Count2(int startRowIndex, int maximumRows, string dept_no, string target_type, string ym)
    {
        try
        {

            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select COUNT(*) total_record ");
            sb.Append(" FROM TB_D_M_OVERTIME_APPLY A ");
            sb.Append(" inner join (SELECT  A.EMP_ID, A.EMP_NAME, A.EMP_STATUS, A.PLANT_CD, A.DEPT_NO_20, A.DEPT_NO, A.PJOB_CD, A.WS_CD ");
            sb.Append(" 	  , A.OVERTIME_CTL_CD, D.SUB_DESC + '/' + D.CODE_VAL1 OVERTIME_CTL_DESC, C.DEPT_NAME ");
            sb.Append(" FROM VW_H_EMP_DATA  A ");
            sb.Append(" left join TB_H_M_DEPT_ORG B on A.DEPT_NO = B.DEPT_NO AND B.END_DT >= GETDATE()  ");
            sb.Append(" left join TB_H_M_DEPT C on A.DEPT_NO = C.DEPT_NO AND C.END_DT >= GETDATE() ");
            sb.Append(" left join TB_9_M_COMM_D D on A.OVERTIME_CTL_CD = D.SUB_CD and D.SYS_CD = 'HB' and D.MAIN_CD = 'OVERTIME_CTL_CD' ");
            sb.Append(" inner join TB_D_M_OVERTIME_TARGET_EMP E ON E.TARGET_TYPE= @TARGET_TYPE and E.TARGET_YEAR=@YEAR ");
            sb.Append(" WHERE  (A.EMP_STATUS=('01') or (A.EMP_STATUS=('99')and convert(char(7),A.LEAVE_DT,111) =@YM )) ");
            sb.Append(" AND  A.DEPT_NO in ( select dept_no from [dbo].[FN_H_GET_WORK_DEPT](@DEPT_NO)  ) ");
            sb.Append("		AND A.WORK_CD = E.WORK_CD AND A.WS_CD= E.WS_CD AND A.PJOB_CD=E.PJOB_CD ) B ");
            sb.Append("		ON A.EMP_ID = B.EMP_ID ");
            sb.Append(" WHERE convert(char(7),A.APPLY_OVERTIME_DT,111) =@YM ");
            sb.Append(" AND A.FORM_STATUS not in ('N','D')  ");
            sb.Append(" AND A.CHECK_STATUS='Y' ");
            sb.Append(" AND A.OVERTIME_DT_TYPE = ('2') ");
            sb.Append(" AND  ((convert(decimal,A.OVERTIME_PAY_HOUR, 0)) / 60)>12");

            ht.Add("@DEPT_NO", dept_no);
            ht.Add("@YM", ym);
            ht.Add("@YEAR", ym.Substring(0, 4));
            ht.Add("@TARGET_TYPE", target_type);


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


    // (3).超過管制上限時數  Gridview 查詢資料
    public DataTable getAb2Data3(int startRowIndex, int maximumRows, string sortExpression, string dept_no, string target_type, string ym)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select * From ( ");
            sb.Append(" Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber, *  FROM( ");

            sb.Append(" Select  ");

            sb.Append(" B.DEPT_NO+'-'+B.DEPT_NAME as DEPT_NO,A.EMP_ID,B.EMP_NAME,D.CODE_VAL1 OVERTIME_CTL_CD,");
            //sb.Append(" CAST(ROUND(((convert(decimal,sum(isnull(A.APPROVE_OVERTIME_HOUR,0)))) / 60), 2) AS DECIMAL(12, 2)) APPROVE_OVERTIME_HOUR ");
            //假日加班要扣除 8*60
            sb.Append(" CAST(ROUND(((convert(decimal,sum(isnull(A.APPROVE_OVERTIME_HOUR_LIMIT,0)))) / 60), 2) AS DECIMAL(12, 2)) APPROVE_OVERTIME_HOUR ");
            //sb.Append(" FROM TB_D_M_OVERTIME_APPLY A ");
            //若加班日期類型 為2假日時,需-8小時  sb.Append(" FROM TB_D_M_OVERTIME_APPLY A ");
            sb.Append(" FROM ( ");
            sb.Append(" select  NORMAL_HOUR  APPROVE_OVERTIME_HOUR_LIMIT ");
            //sb.Append(" select  case when OVERTIME_DT_TYPE=2  ");
            //sb.Append("   then  IIF(OVERTIME_PAY_HOUR-8*60>0, OVERTIME_PAY_HOUR-8*60,0) ");
            //sb.Append("   else  OVERTIME_PAY_HOUR ");
            //sb.Append("   end	   as  APPROVE_OVERTIME_HOUR_LIMIT");
            sb.Append("   ,* ");
            sb.Append("   from TB_D_M_OVERTIME_APPLY ");
            sb.Append(" ) A ");


            sb.Append(" inner join (SELECT  A.EMP_ID, A.EMP_NAME, A.EMP_STATUS, A.PLANT_CD, A.DEPT_NO_20, A.DEPT_NO, A.PJOB_CD, A.WS_CD ");
            sb.Append("	  , A.OVERTIME_CTL_CD, D.SUB_DESC + '/' + D.CODE_VAL1 OVERTIME_CTL_DESC, C.DEPT_NAME ");
            sb.Append(" FROM VW_H_EMP_DATA  A ");
            sb.Append(" left join TB_H_M_DEPT_ORG B on A.DEPT_NO = B.DEPT_NO AND B.END_DT >= GETDATE()  ");
            sb.Append(" left join TB_H_M_DEPT C on A.DEPT_NO = C.DEPT_NO AND C.END_DT >= GETDATE()  ");
            sb.Append(" left join TB_9_M_COMM_D D on A.OVERTIME_CTL_CD = D.SUB_CD and D.SYS_CD = 'HB' and D.MAIN_CD = 'OVERTIME_CTL_CD' ");
            sb.Append(" inner join TB_D_M_OVERTIME_TARGET_EMP E ON E.TARGET_TYPE=@TARGET_TYPE and E.TARGET_YEAR=@YEAR  ");
            sb.Append(" WHERE  (A.EMP_STATUS=('01') or (A.EMP_STATUS=('99')and convert(char(7),A.LEAVE_DT,111) =@YM ))  ");
            sb.Append(" AND  A.DEPT_NO in ( select dept_no from [dbo].[FN_H_GET_WORK_DEPT](@DEPT_NO)  ) ");
            sb.Append("		AND A.WORK_CD = E.WORK_CD AND A.WS_CD= E.WS_CD AND A.PJOB_CD=E.PJOB_CD ) B  ");
            sb.Append("		ON A.EMP_ID = B.EMP_ID ");
            sb.Append(" left join TB_9_M_COMM_D D on A.OVERTIME_CTL_CD = D.SUB_CD and D.SYS_CD = 'HB' and D.MAIN_CD = 'OVERTIME_CTL_CD' ");
            sb.Append(" WHERE convert(char(7),A.APPLY_OVERTIME_DT,111) =@YM  ");
            sb.Append(" AND A.FORM_STATUS not in ('N','D')  ");
            sb.Append(" AND A.CHECK_STATUS='Y'  ");
            sb.Append(" group by  B.DEPT_NO, B.DEPT_NAME, A.EMP_ID, B.EMP_NAME, D.CODE_VAL1 ");

            sb.Append(" )z where z.APPROVE_OVERTIME_HOUR > OVERTIME_CTL_CD  ");

            sb.Append(" )god_data where RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar)");
            ht.Add("@DEPT_NO", dept_no);
            ht.Add("@YM", ym);
            ht.Add("@YEAR", ym.Substring(0, 4));
            ht.Add("@TARGET_TYPE", target_type);

            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            return dbConn.Query(sb, ht);

            /*
           if (sortExpression.Contains("EMP_ID"))
            {
                sortExpression = sortExpression.Replace("EMP_ID", "A.EMP_ID");
            }
            if (sortExpression.Contains("EMP_NAME"))
            {
                sortExpression = sortExpression.Replace("EMP_NAME", "B.EMP_NAME");
            }
            if (sortExpression.Contains("DEPT_NO"))
            {
                sortExpression = sortExpression.Replace("DEPT_NO", "B.DEPT_NO");
            }
            if (sortExpression.Contains("SHIFT_CD"))
            {
                sortExpression = sortExpression.Replace("SHIFT_CD", "A.SHIFT_CD");
            }
            if (sortExpression.Contains("OVERTIME_CTL_CD"))
            {
                sortExpression = sortExpression.Replace("OVERTIME_CTL_CD", "D.CODE_VAL1");
            }
            sb.Append(" Select * From ( ");
            sb.Append(" Select ROW_NUMBER() OVER(ORDER BY " + sortExpression + " ) As RowNumber,");
            sb.Append(" B.DEPT_NO+'-'+B.DEPT_NAME as DEPT_NO,A.EMP_ID,B.EMP_NAME,D.CODE_VAL1 OVERTIME_CTL_CD,");
            sb.Append(" CAST(ROUND(((convert(decimal,sum(isnull(A.APPROVE_OVERTIME_HOUR,0)))) / 60), 2) AS DECIMAL(12, 2)) APPROVE_OVERTIME_HOUR ");
            sb.Append(" FROM TB_D_M_OVERTIME_APPLY A ");
            sb.Append(" inner join (SELECT  A.EMP_ID, A.EMP_NAME, A.EMP_STATUS, A.PLANT_CD, A.DEPT_NO_20, A.DEPT_NO, A.PJOB_CD, A.WS_CD ");
            sb.Append("	  , A.OVERTIME_CTL_CD, D.SUB_DESC + '/' + D.CODE_VAL1 OVERTIME_CTL_DESC, C.DEPT_NAME ");
            sb.Append(" FROM VW_H_EMP_DATA  A ");
            sb.Append(" left join TB_H_M_DEPT_ORG B on A.DEPT_NO = B.DEPT_NO AND B.END_DT >= GETDATE()  ");
            sb.Append(" left join TB_H_M_DEPT C on A.DEPT_NO = C.DEPT_NO AND C.END_DT >= GETDATE()  ");
            sb.Append(" left join TB_9_M_COMM_D D on A.OVERTIME_CTL_CD = D.SUB_CD and D.SYS_CD = 'HB' and D.MAIN_CD = 'OVERTIME_CTL_CD' ");
            sb.Append(" inner join TB_D_M_OVERTIME_TARGET_EMP E ON E.TARGET_TYPE=@TARGET_TYPE and E.TARGET_YEAR=@YEAR  ");
            sb.Append(" WHERE  (A.EMP_STATUS=('01') or (A.EMP_STATUS=('99')and convert(char(7),A.LEAVE_DT,111) =@YM ))  ");
            sb.Append(" AND  A.DEPT_NO in ( select dept_no from [dbo].[FN_H_GET_WORK_DEPT](@DEPT_NO)  ) ");
            sb.Append("		AND A.WORK_CD = E.WORK_CD AND A.WS_CD= E.WS_CD AND A.PJOB_CD=E.PJOB_CD ) B  ");
            sb.Append("		ON A.EMP_ID = B.EMP_ID ");
            sb.Append(" left join TB_9_M_COMM_D D on A.OVERTIME_CTL_CD = D.SUB_CD and D.SYS_CD = 'HB' and D.MAIN_CD = 'OVERTIME_CTL_CD' ");
            sb.Append(" WHERE convert(char(7),A.APPLY_OVERTIME_DT,111) =@YM  ");
            sb.Append(" AND A.FORM_STATUS not in ('N','D')  ");
            sb.Append(" AND A.CHECK_STATUS='Y'  ");
            sb.Append(" group by  B.DEPT_NO, B.DEPT_NAME, A.EMP_ID, B.EMP_NAME, D.CODE_VAL1 ");
            sb.Append(" )god_data where god_data.APPROVE_OVERTIME_HOUR > OVERTIME_CTL_CD and RowNumber between CAST(@startRowIndex+1 as varchar) ");
            sb.Append(" AND CAST(@startRowIndex+@maximumRows as varchar)");
             
           

            ht.Add("@DEPT_NO", dept_no);
            ht.Add("@YM", ym);
            ht.Add("@YEAR", ym.Substring(0, 4));
            ht.Add("@TARGET_TYPE", target_type);

            ht.Add("@startRowIndex", startRowIndex);
            ht.Add("@maximumRows", maximumRows);
            return dbConn.Query(sb, ht);
             */
        }
        catch
        {
            throw;
        }
    }
    // (3).超過管制上限時數   Gridview 查詢總筆數
    public int getAb2Count3(int startRowIndex, int maximumRows, string dept_no, string target_type, string ym)
    {
        try
        {

            int t = 0;
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();

            sb.Append("Select COUNT(*) total_record ");
            sb.Append(" FROM (select  ");
            sb.Append(" A.EMP_ID,B.EMP_NAME,D.CODE_VAL1 OVERTIME_CTL_CD,");
            sb.Append(" CAST(ROUND(((convert(decimal,sum(isnull(A.OVERTIME_PAY_HOUR,0)))) / 60), 2) AS DECIMAL(12, 2)) APPROVE_OVERTIME_HOUR");
            sb.Append(" FROM ( ");
            sb.Append(" select  NORMAL_HOUR  APPROVE_OVERTIME_HOUR_LIMIT ");
            //sb.Append(" select  case when OVERTIME_DT_TYPE=2  ");
            //sb.Append("   then  IIF(OVERTIME_PAY_HOUR-8*60>0, OVERTIME_PAY_HOUR-8*60,0) ");
            //sb.Append("   else  OVERTIME_PAY_HOUR ");
            //sb.Append("   end	   as  APPROVE_OVERTIME_HOUR_LIMIT");
            sb.Append("   ,* ");
            sb.Append("   from TB_D_M_OVERTIME_APPLY ");
            sb.Append(" ) A ");



            sb.Append(" inner join (SELECT  A.EMP_ID, A.EMP_NAME, A.EMP_STATUS, A.PLANT_CD, A.DEPT_NO_20, A.DEPT_NO, A.PJOB_CD, A.WS_CD ");
            sb.Append("	  , A.OVERTIME_CTL_CD, D.SUB_DESC + '/' + D.CODE_VAL1 OVERTIME_CTL_DESC, C.DEPT_NAME ");
            sb.Append(" FROM VW_H_EMP_DATA  A ");
            sb.Append(" left join TB_H_M_DEPT_ORG B on A.DEPT_NO = B.DEPT_NO AND B.END_DT >= GETDATE()  ");
            sb.Append(" left join TB_H_M_DEPT C on A.DEPT_NO = C.DEPT_NO AND C.END_DT >= GETDATE()  ");
            sb.Append(" left join TB_9_M_COMM_D D on A.OVERTIME_CTL_CD = D.SUB_CD and D.SYS_CD = 'HB' and D.MAIN_CD = 'OVERTIME_CTL_CD' ");
            sb.Append(" inner join TB_D_M_OVERTIME_TARGET_EMP E ON E.TARGET_TYPE=@TARGET_TYPE and E.TARGET_YEAR=@YEAR  ");
            sb.Append(" WHERE  (A.EMP_STATUS=('01') or (A.EMP_STATUS=('99')and convert(char(7),A.LEAVE_DT,111) =@YM ))  ");
            sb.Append(" AND  A.DEPT_NO in ( select dept_no from [dbo].[FN_H_GET_WORK_DEPT](@DEPT_NO)  ) ");
            sb.Append("		AND A.WORK_CD = E.WORK_CD AND A.WS_CD= E.WS_CD AND A.PJOB_CD=E.PJOB_CD ) B  ");
            sb.Append("		ON A.EMP_ID = B.EMP_ID ");
            sb.Append(" left join TB_9_M_COMM_D D on A.OVERTIME_CTL_CD = D.SUB_CD and D.SYS_CD = 'HB' and D.MAIN_CD = 'OVERTIME_CTL_CD' ");
            sb.Append(" WHERE convert(char(7),A.APPLY_OVERTIME_DT,111) =@YM  ");
            sb.Append(" AND A.FORM_STATUS not in ('N','D')  ");
            sb.Append(" AND A.CHECK_STATUS='Y'  ");
            sb.Append(" group by  B.DEPT_NO, B.DEPT_NAME, A.EMP_ID, B.EMP_NAME, D.CODE_VAL1 ");
            sb.Append(" )god_data where god_data.APPROVE_OVERTIME_HOUR > OVERTIME_CTL_CD ");
            ht.Add("@DEPT_NO", dept_no);
            ht.Add("@YM", ym);
            ht.Add("@YEAR", ym.Substring(0, 4));
            ht.Add("@TARGET_TYPE", target_type);

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

    internal void SP_DI_OVERTIME_TOTAL_IFLOW(string dept_no, string work_day_cd, string ym, string target_type)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("SP_DI080_OVERTIME_TOTAL_IFLOW");
            ht.Add("@p_DEPT_NO", dept_no);
            ht.Add("@p_WORK_DAY_CD", work_day_cd);
            ht.Add("@p_YM", ym.Replace("/", ""));
            ht.Add("@p_TARGET_TYPE", target_type);
            ht.Add("@p_UserID", SessionHandle.Current.emp_id);
            ht.Add("@p_FuncID", "DI080");

            dbConn.ExecuteSP(sb, ht, true);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getTOTAL_TIME_OVERTIME_IFLOW(string dept_no, string work_day_cd, string ym, string target_type)
    {
        try
        {

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@"Select  isnull(sum(APPLY_OVERTIME_HOUR), 0) TOTAL_TIME_OVERTIME_IFLOW     
                        from TB_D_M_OVERTIME_FLOW_TMP 
                        where CREATED_BY= @p_UserID and FUNC_ID=@p_FuncID and  WORK_DAY_CD = @p_WORK_DAY_CD
                        ");

            ht.Add("@p_UserID", SessionHandle.Current.emp_id);
            ht.Add("@p_FuncID", "DI080");
            ht.Add("@p_WORK_DAY_CD", work_day_cd);

            /*
            string[] tmp = ym.Split('/');
            //若是當月用 VW_H_EMP_DATA, 其餘用 TB_H_R_EMP_DATA_MONTH
            string targetTableNAme = "";
            string extraCondition = "";
            if (Convert.ToInt32(ym.Replace("/", "")) < Convert.ToInt32(DateTime.Now.ToString("yyyyMM")))
            {
                targetTableNAme = " TB_H_R_EMP_DATA_MONTH A ";
                extraCondition = " AND A.YM = @YM ";
            }
            else
            {
                targetTableNAme = " VW_H_EMP_DATA A ";
                extraCondition = "";
            }

            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(@"Select isnull(sum(A.APPLY_OVERTIME_HOUR), 0) TOTAL_TIME_OVERTIME_IFLOW   "
                    + " from (  "
                    + "  select a.EMP_ID,sum(a.APPLY_OVERTIME_HOUR) APPLY_OVERTIME_HOUR "
                    + "  from [" + utilities.IFLOWName + "].[IFLOW2].[dbo].[VW_D_M_OVERTIME_FLOW] a "
                    + "  LEFT JOIN TB_D_M_OVERTIME_TYPE b ON a.OVERTIME_CD = b.OVERTIME_CD COLLATE Chinese_Taiwan_Stroke_BIN "
                    + "  where b.OVERTIME_DT_TYPE collate Chinese_Taiwan_Stroke_BIN = @OVERTIME_DT_TYPE  "
                    + "  and (substring(convert(char(10),a.APPLY_OVERTIME_DT,120),0,8))= @YM2 "
                    + "  and  A.EMP_ID COLLATE Chinese_Taiwan_Stroke_BIN in  ( "
                    + "     select emp_id from " + targetTableNAme
                    + "     inner join TB_D_M_OVERTIME_TARGET_EMP E ON E.TARGET_TYPE=@TARGET_TYPE and E.TARGET_YEAR=@year "
                    + "     where a.dept_no in ( SELECT DEPT_NO FROM  [dbo].[FN_H_GET_WORK_DEPT](@DEPT_NO)   ) " + extraCondition
                    + "     and (A.EMP_STATUS=('01') or (A.EMP_STATUS=('99') and substring(convert(char(10),A.LEAVE_DT,120),0,8) =@YM2 ))   "
                    + "     GROUP BY a.EMP_ID  "
                    + "     )"
                    + " GROUP BY a.EMP_ID  "
                    + " ) A  "
                    + "  "
                    );

            ht.Add("@year", tmp[0]);
            ht.Add("@YM", ym.Replace("/",  ""));
            ht.Add("@YM2", ym.Replace("/", "-"));
            ht.Add("@TARGET_TYPE", target_type);
            ht.Add("@DEPT_NO", dept_no);
            //1.平日加班  2.假日加班
            ht.Add("@OVERTIME_DT_TYPE", work_day_cd);
            */
            return dbConn.Query(sb, ht);


           
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getDEPT_NAME(string dept_no, string dept_no_list)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select DEPT_NO,DEPT_NAME ");
            sb.Append(" from TB_H_M_DEPT ");
            sb.Append(" where DEPT_NO=@DEPT_NO ");
            if (dept_no_list != "")
            {
                sb.Append(" and DEPT_NO in (@dept_no_list)");
                ht.Add("@dept_no_list", dept_no_list.Split(','));
            }
            ht.Add("@DEPT_NO", dept_no);

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    internal DataTable getOVERTIME_SPECIAL_HOUR()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append("select distinct DEPT_NO ");
            sb.Append(" from TB_D_M_OVERTIME_TARGET ");

            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }
}