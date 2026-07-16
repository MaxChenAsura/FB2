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
/// Dept_Search 的摘要描述
/// </summary>
public class Dept_Search : BaseDAO
{


    public string DEPT_NO { get; set; }
    public string DEPT_NAME { get; set; }
    public string DEPT_LEVEL { get; set; }
    public string UP_DEPT_NO { get; set; }
    public string UP_DEPT_NAME { get; set; }
    public string HEAD_EMP_ID { get; set; }
    public string HEAD_EMP_NAME { get; set; }

    public string DEPT_NO_20 { get; set; }
    public string DEPT_NAME_20 { get; set; }
    public string DEPT_NO_30 { get; set; }
    public string DEPT_NAME_30 { get; set; }
    public string DEPT_NO_40 { get; set; }
    public string DEPT_NAME_40 { get; set; }
    public string DEPT_NO_50 { get; set; }
    public string DEPT_NAME_50 { get; set; }
    public string DEPT_NO_60 { get; set; }
    public string DEPT_NAME_60 { get; set; }
    public string DEPT_NO_70 { get; set; }
    public string DEPT_NAME_70 { get; set; }
    public string DEPT_NAME_DESC { get; set; }
    public string DEPT_FULL_NAME { get; set; }
    public string DIV_DEPT_FULL_NAME { get; set; }

    public string EMP_ID { get; set; }
    public string EMP_NAME { get; set; }
    public bool onlySelf { get; set; }

    public Dept_Search()
    {

    }

    public List<Dept_Search> getTreeViewList()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select DEPT_NO,DEPT_NO + ' ' + DEPT_NAME DEPT_NAME,DEPT_LEVEL,UP_DEPT_NO,UP_DEPT_NAME,HEAD_EMP_ID ,isnull(HEAD_EMP_NAME,'') HEAD_EMP_NAME ");
            //sb.Append(" Select DEPT_NO,DEPT_NAME DEPT_NAME,DEPT_LEVEL,UP_DEPT_NO,UP_DEPT_NAME,HEAD_EMP_ID ,isnull(HEAD_EMP_NAME,'') HEAD_EMP_NAME ");
            sb.Append(" ,DEPT_NO_20,DEPT_NAME_20,DEPT_NO_30,DEPT_NAME_30,DEPT_NO_40,DEPT_NAME_40,DEPT_NO_50,DEPT_NAME_50,DEPT_NO_60,DEPT_NAME_60,DEPT_NO_70,DEPT_NAME_70, DEPT_NAME as DEPT_NAME_DESC  ");
            sb.Append(" ,DEPT_FULL_NAME,DIV_DEPT_FULL_NAME");
            sb.Append(" From VW_H_DEPT_DATA ");
            DataTable tmp = dbConn.Query(sb, ht);
            List<Dept_Search> temp = new List<Dept_Search>();
            if (tmp.Rows.Count > 0)
            {
                temp = tmp.AsEnumerable().Select(x => new Dept_Search {
                    DEPT_NO = x.Field<string>(0).ToString(),
                    DEPT_NAME = x.Field<string>(1).ToString(),
                    DEPT_LEVEL = x.Field<decimal>(2).ToString(),
                    UP_DEPT_NO = x.Field<string>(3).ToString(),
                    UP_DEPT_NAME = x.Field<string>(4).ToString(),
                    HEAD_EMP_ID = x.Field<string>(5).ToString(),
                    HEAD_EMP_NAME = x.Field<string>(6).ToString(),

                    DEPT_NO_20 = x.Field<string>(7).ToString(),
                    DEPT_NAME_20 = x.Field<string>(8).ToString(),
                    DEPT_NO_30 = x.Field<string>(9).ToString(),
                    DEPT_NAME_30 = x.Field<string>(10).ToString(),
                    DEPT_NO_40 = x.Field<string>(11).ToString(),
                    DEPT_NAME_40 = x.Field<string>(12).ToString(),
                    DEPT_NO_50 = x.Field<string>(13).ToString(),
                    DEPT_NAME_50 = x.Field<string>(14).ToString(),
                    DEPT_NO_60 = x.Field<string>(15).ToString(),
                    DEPT_NAME_60 = x.Field<string>(16).ToString(),
                    DEPT_NO_70 = x.Field<string>(17).ToString(),
                    DEPT_NAME_70 = x.Field<string>(18).ToString(),
                    DEPT_NAME_DESC = x.Field<string>(19).ToString(),
                    DEPT_FULL_NAME = x.Field<string>(20).ToString(),
                    DIV_DEPT_FULL_NAME = x.Field<string>(21).ToString(),
                }).ToList();
            }
            
            return temp;

        }
        catch
        {

            throw;
        }
    }

    public DataTable getEmpDate(List<string> depts = null, string super = "N")
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            string tmp = "";
            Hashtable ht = new Hashtable();
            tmp += "Select top 500 ROW_NUMBER() OVER(ORDER BY e.EMP_ID) AS ROWID,e.EMP_ID,e.EMP_NAME,e.PJOB_CD,d.DEPT_NAME,e.DEPT_NO,EMP_CD,";
            tmp += " LEVEL_CD,GRADE_CD,CONVERT(char(10), JOIN_DT, 111) JOIN_DT,CONVERT(char(10), BE_EMP_DT, 111) BE_EMP_DT,WS_CD,EMP_STATUS,EMP_STATUS_DESC,PLANT_NAME,WORK_SHIFT_DESC";
            tmp += " ,d.DEPT_NO_20,d.DEPT_NAME_20,d.DEPT_NO_30,d.DEPT_NAME_30,d.DEPT_NO_40,d.DEPT_NAME_40,d.DEPT_NO_50,d.DEPT_NAME_50,d.DEPT_NO_60,d.DEPT_NAME_60,d.DEPT_NO_70,d.DEPT_NAME_70,d.DEPT_NAME as DEPT_NAME_DESC ";
            tmp += " ,d.DEPT_FULL_NAME,d.DIV_DEPT_FULL_NAME, e.PJOB_DESC";
            tmp += " from VW_H_EMP_DATA e left join VW_H_DEPT_DATA d on e.DEPT_NO = d.DEPT_NO";
            tmp += " where EMP_STATUS = '01'";

            if (DEPT_NO != null && DEPT_NO != "")
            {
                tmp += " and d.DEPT_NO = @dept_no";
                ht.Add("@dept_no", DEPT_NO);
            }
            if (EMP_ID != null && EMP_ID != "" || onlySelf)
            {
                tmp += " and e.EMP_ID like @EMP_ID";
                ht.Add("@EMP_ID", "%" + EMP_ID + "%");
            }
            if (EMP_NAME != null && EMP_NAME != "")
            {
                tmp += " and e.EMP_NAME LIKE @EMP_NAME";
                ht.Add("@EMP_NAME", "%" + EMP_NAME + "%");
            }
            if (depts != null && super == "N")
            {
                tmp += " and (";
                for (int i = 0; i < depts.Count; i++)
                {
                    tmp += " d.DEPT_NO = @dept_no" + i + " or";
                    ht.Add("@dept_no" + i, depts[i]);
                }
                tmp = tmp.Substring(0, tmp.Length - 2);
                tmp += ")";
            }
            sb.Append(tmp);
            return dbConn.Query(sb, ht);
        }
        catch (Exception)
        {

            throw;
        }
    }

    public List<string> getUserDept(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            List<string> rtnList = new List<string>();
            sb.Append("Select TB_H_M_EMP.dept_no,HEAD_EMP_ID,DEPT_LEVEL from TB_H_M_EMP,VW_H_DEPT_DATA where TB_H_M_EMP.DEPT_NO = VW_H_DEPT_DATA.DEPT_NO and emp_id = @emp_id");
            ht.Add("@emp_id", emp_id);
            DataTable tmp = dbConn.Query(sb, ht);

            if (tmp.Rows.Count > 0)
            {
                rtnList.Add(tmp.Rows[0][0].ToString());
                rtnList.Add(tmp.Rows[0][1].ToString());
                rtnList.Add(tmp.Rows[0][2].ToString());
            }

            return rtnList;

        }
        catch (Exception)
        {

            throw;
        }
    }

    public List<string> getHead_Dept(string emp_id)
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select MNG_DEPT_NO from TB_H_R_HEAD_DEPT where emp_id = @emp_id ");
            sb.Append(" union ");
            sb.Append(" select sub_cd  from [dbo].[FN_SPLIT_CHARACTOR](',',@departments) ");
            ht.Add("@emp_id", emp_id);
            ht.Add("@departments", SessionHandle.Current.departments);
            List<string> rtnList = new List<string>();
            DataTable tmp = dbConn.Query(sb, ht);
            if (tmp.Rows.Count > 0)
            {
                for (int i = 0; i < tmp.Rows.Count; i++)
                {
                    rtnList.Add(tmp.Rows[i]["MNG_DEPT_NO"].ToString());
                }
            }
            return rtnList;

        }
        catch (Exception)
        {

            throw;
        }
    }

    public List<string> getHead_Dept()
    {
        try
        {
            StringBuilder sb = new StringBuilder();
            Hashtable ht = new Hashtable();
            sb.Append(" Select MNG_DEPT_NO from TB_H_R_HEAD_DEPT  ");
            List<string> rtnList = new List<string>();
            DataTable tmp = dbConn.Query(sb, ht);
            if (tmp.Rows.Count > 0)
            {
                for (int i = 0; i < tmp.Rows.Count; i++)
                {
                    rtnList.Add(tmp.Rows[i]["MNG_DEPT_NO"].ToString());
                }
            }
            return rtnList;

        }
        catch (Exception)
        {

            throw;
        }
    }


}