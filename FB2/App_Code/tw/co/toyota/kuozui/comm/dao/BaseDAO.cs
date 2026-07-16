using log4net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using FB2.tw.co.toyota.kuozui.dao;

namespace FB2.tw.co.toyota.kuozui.dao
{
    public class BaseDAO
    {
        /// <summary>
        /// DB 連接器
        /// </summary>
        protected DBConnector dbConn = new DBConnector();
        public DBConnector GetdbConn { get { return dbConn; } }

    }
}