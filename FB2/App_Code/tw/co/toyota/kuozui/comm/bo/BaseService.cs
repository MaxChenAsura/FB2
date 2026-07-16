using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using FB2.tw.co.toyota.kuozui.comm;
using FB2.tw.co.toyota.kuozui.dao;
using FB2.tw.co.toyota.kuozui.COMMON;

namespace FB2.tw.co.toyota.kuozui.bo
{
    public class BaseService
    {

        //宣告 transaction 模組 物件
        private TranMd tranMd = new TranMd();

        //DAO 物件
        private BaseDAO dao = new BaseDAO();

        //其它連線字串
        public string OtherCommStr = "";

        /// <summary>
        /// 開始Transaction
        /// </summary>
        protected void BeginTransaction()
        {
            if (HttpContext.Current.Session[Constant.TRAN_SESSION] == null)
            {
                tranMd.TranConn = Connection();
                tranMd.TranConn.Open();
                tranMd.Tran = tranMd.TranConn.BeginTransaction();
                HttpContext.Current.Session[Constant.TRAN_SESSION] = tranMd;
            }
        }

        /// <summary>
        /// Transaction Commit
        /// </summary>
        protected void Commit()
        {
            if (HttpContext.Current.Session[Constant.TRAN_SESSION] != null)
            {
                tranMd = (TranMd)HttpContext.Current.Session[Constant.TRAN_SESSION];
                tranMd.Tran.Commit();
                tranMd.Tran.Dispose();
                if (tranMd.TranConn != null)
                {
                    tranMd.TranConn.Close();
                    tranMd.TranConn.Dispose();
                }

                HttpContext.Current.Session[Constant.TRAN_SESSION] = null;
            }
        }

        /// <summary>
        /// Transaction RollBack
        /// </summary>
        protected void RollBack()
        {
            if (HttpContext.Current.Session[Constant.TRAN_SESSION] != null)
            {
                tranMd = (TranMd)HttpContext.Current.Session[Constant.TRAN_SESSION];
                tranMd.Tran.Rollback();
                tranMd.Tran.Dispose();
                if (tranMd.TranConn != null)
                {
                    tranMd.TranConn.Close();
                    tranMd.TranConn.Dispose();
                }

                HttpContext.Current.Session[Constant.TRAN_SESSION] = null;
            }
        }

        /// <summary>
        /// DB Connection
        /// </summary>
        /// <returns></returns>
        private SqlConnection Connection()
        {
            if (String.IsNullOrEmpty(Constant.DB_CONN_STR)) { Constant.SetConnStr(); }
            SqlConnection conn = new SqlConnection();
            conn.ConnectionString = string.IsNullOrEmpty(OtherCommStr) ? Constant.DB_CONN_STR : OtherCommStr;
            return conn;
        }


    }
}