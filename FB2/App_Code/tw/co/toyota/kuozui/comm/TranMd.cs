using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace FB2.tw.co.toyota.kuozui.comm
{
    public class TranMd
    {
        /// <summary>
        /// transaction DB連線
        /// </summary>
        public SqlConnection TranConn { get; set; }

        /// <summary>
        /// transaction 物件
        /// </summary>
        public SqlTransaction Tran { get; set; }
    }
}