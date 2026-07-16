using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace FB2.tw.co.toyota.kuozui.COMMON
{
    public class Constant
    {
        /// <summary>
        /// Sesion 名稱
        /// </summary>
        public static readonly string ACES_SESSION = "ACES_SESSION";

        /// <summary>
        /// transaction Session 名稱
        /// </summary>
        public static readonly string TRAN_SESSION = "TRAN_SESSION";

        /// <summary>
        /// 頁面資料相關資訊暫存(For 新增/修改/刪除)
        /// PS.使用 Response.Redirect 不使用 Server.Transfer 為避免網頁重新整理時，DB資料重覆問題防止
        /// </summary>
        public static readonly string PAGE_EXECUTE_SESSION = "PAGE_EXECUTE_SESSION";

        /// <summary>
        /// DB Connection 字串
        /// </summary>
        public static string DB_CONN_STR = "";

        /// <summary>
        /// 人事系統 DB Connection 名稱
        /// </summary>
        public static string FB_CONN_NAME = "MSSQLDBConn_FB2";

        /// <summary>
        /// LDAP URL 路徑
        /// </summary>
        public static readonly string LDAP_URL = "LDAP://192.168.16.220:389";

        /// <summary>
        /// 訊息檔
        /// </summary>
        public static Dictionary<string, string> MESSAGE = new Dictionary<string,string>();

        /// <summary>
        /// 是否進行 LDAP 認證註記 (Y:是 N:否)
        /// </summary>
        public static string OFFLINE = "";

        /// <summary>
        /// 最高管理者角色代碼
        /// </summary>
        public static readonly string ADMIN = "ADMIN";

        /// <summary>
        /// 系統擔當角色代碼
        /// </summary>
        public static readonly string SRS = "SRS";

        /// <summary>
        /// 業務擔當角色代碼
        /// </summary>
        public static readonly string BRS = "BRS";

        /// <summary>
        /// 共用/系統代碼檔 - 共用系統代碼
        /// </summary>
        public static readonly string COMMON_SYS = "COM";

        /// <summary>
        /// 共用/系統代碼檔 - 共用系統名稱
        /// </summary>
        public static readonly string COMMON_SYS_NAME = "共用系統";

        /// <summary>
        /// ACE 專案代碼
        /// </summary>
        public static readonly string ACE_CD = "ZZ1";

        /// <summary>
        /// 機能第一層代碼
        /// </summary>
        public static readonly string FUNC_TOP = "TOP";

        /// <summary>
        /// 匯入Excel錯誤清單參數
        /// </summary>
        public static readonly string IMP_EXL = "IMP_EXL";

        /// <summary>
        /// 分隔符號
        /// </summary>
        public static readonly string DELIMITER = ", ";

        /// <summary>
        /// DB Connection 設定
        /// </summary>
        public static void SetConnStr()
        {
            DB_CONN_STR = ConfigurationManager.ConnectionStrings["dbconnection"].ConnectionString;
        }

        /// <summary>
        /// DB 其它 Connection
        /// </summary>
        public static string SetOtherConnStr(string ConnName)
        {
            return ConfigurationManager.ConnectionStrings[ConnName].ConnectionString;
        }
    }
}