using log4net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Web;
using FB2.tw.co.toyota.kuozui.comm;
using FB2.tw.co.toyota.kuozui.COMMON;

namespace FB2.tw.co.toyota.kuozui.dao
{
    public class DBConnector
    {
        // SQL Log記錄字串
        private StringBuilder logSql = new StringBuilder();
        // 500亳妙
        private static readonly int dbExecTime = 500;

        //宣告 transaction 模組 物件
        private TranMd tranMd = new TranMd();

        //其它連線字串
        public string OtherCommStr = "";


        /// <summary>
        /// 取得 Transaction 物件模組
        /// </summary>
        private void GetTranMd()
        {
            if (HttpContext.Current.Session[Constant.TRAN_SESSION] != null)
            {
                tranMd = (TranMd)HttpContext.Current.Session[Constant.TRAN_SESSION];
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

        /// <summary>
        /// 查詢SQL 模組
        /// </summary>
        /// <param name="sb">SQL語法</param>
        /// <returns></returns>
        public DataTable Query(StringBuilder sb)
        {
            return Query(sb, null);
        }

        /// <summary>
        /// 查詢SQL 模組
        /// </summary>
        /// <param name="sb">SQL語法</param>
        /// <param name="ht">參數物件</param>
        /// <returns></returns>
        public DataTable Query(StringBuilder sb, Hashtable ht)
        {
            return Query(sb, ht, false);
        }

        /// <summary>
        /// 查詢SQL 模組
        /// </summary>
        /// <param name="sb">SQL語法</param>
        /// <param name="ht">參數物件</param>
        /// <param name="blClear">是否清空傳入參數物件</param>
        /// <returns></returns>
        public DataTable Query(StringBuilder sb, Hashtable ht, Boolean blClear)
        {
            SqlConnection conn = Connection();
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
            logSql.Append(sb);

            try
            {
                conn.Open();
                if (ht != null)
                {
                    foreach (DictionaryEntry objDE in ht)
                    {
                        if (!SQLIN(cmd, objDE, sb))
                        {
                            logSql.Replace(objDE.Key.ToString(), "'" + objDE.Value.ToString() + "'");
                            cmd.Parameters.AddWithValue(objDE.Key.ToString(), objDE.Value);
                        }
                    }
                }

                dt.BeginLoadData();
                TimeSpan dtStart = DateTime.Now.TimeOfDay;
                dt.Load(cmd.ExecuteReader());
                TimeSpan dtEnd = DateTime.Now.TimeOfDay;
                TimeSpan executionTime = dtEnd.Subtract(dtStart);
                //Sql 執行超過500毫秒
                if (executionTime.Milliseconds > dbExecTime)
                {
                    StackTrace ss = new StackTrace(true);
                    MethodBase mb = ss.GetFrame(2).GetMethod();
                    FB2Log.Warn(mb.DeclaringType.Name, logSql.ToString());
                }

                dt.EndLoadData();
                FB2Log.Debug(logSql.ToString());
            }
            catch (Exception ex)
            {
                StackTrace ss = new StackTrace(true);
                MethodBase mb = ss.GetFrame(2).GetMethod();
                FB2Log.Error(mb.DeclaringType.Name, "SQL 錯誤:" + logSql.ToString(), ex);
                throw ;
            }
            finally
            {
                if (blClear) { ht.Clear(); }
                logSql.Clear();
                sb.Clear();
                cmd.Cancel();
                conn.Close();
                conn.Dispose();
            }

            return dt;
        }

        

        /// <summary>
        /// 查詢SQL 模組 transaction
        /// </summary>
        /// <param name="sb">SQL語法</param>
        /// <returns></returns>
        public DataTable QueryT(StringBuilder sb)
        {
            return QueryT(sb, null);
        }

        /// <summary>
        /// 查詢SQL 模組 transaction
        /// </summary>
        /// <param name="sb">SQL語法</param>
        /// <param name="ht">參數物件</param>
        /// <returns></returns>
        public DataTable QueryT(StringBuilder sb, Hashtable ht)
        {
            return QueryT(sb, ht, false);
        }

        /// <summary>
        /// 查詢SQL 模組 transaction
        /// </summary>
        /// <param name="sb">SQL語法</param>
        /// <param name="ht">參數物件</param>
        /// <param name="blClear">是否清空傳入參數物件</param>
        /// <returns></returns>
        public DataTable QueryT(StringBuilder sb, Hashtable ht, Boolean blClear)
        {
            GetTranMd();
            SqlCommand cmd = new SqlCommand(sb.ToString(), tranMd.TranConn, tranMd.Tran);
            DataTable dt = new DataTable();
            logSql.Append(sb);

            try
            {
                if (ht != null)
                {
                    foreach (DictionaryEntry objDE in ht)
                    {
                        if (!SQLIN(cmd, objDE, sb))
                        {
                            logSql.Replace(objDE.Key.ToString(), "'" + objDE.Value.ToString() + "'");
                            cmd.Parameters.AddWithValue(objDE.Key.ToString(), objDE.Value);
                        }
                    }
                }

                dt.BeginLoadData();
                TimeSpan dtStart = DateTime.Now.TimeOfDay;
                dt.Load(cmd.ExecuteReader());
                TimeSpan dtEnd = DateTime.Now.TimeOfDay;
                TimeSpan executionTime = dtEnd.Subtract(dtStart);
                //Sql 執行超過500毫秒
                if (executionTime.Milliseconds > dbExecTime)
                {
                    StackTrace ss = new StackTrace(true);
                    MethodBase mb = ss.GetFrame(2).GetMethod();
                    FB2Log.Warn(mb.DeclaringType.Name, "transaction:" + logSql.ToString());
                }

                dt.EndLoadData();
                FB2Log.Debug("transaction SQL:" + logSql.ToString());
            }
            catch (Exception ex)
            {
                StackTrace ss = new StackTrace(true);
                MethodBase mb = ss.GetFrame(2).GetMethod();
                FB2Log.Error(mb.DeclaringType.Name, "transaction SQL 錯誤:" + logSql.ToString(), ex);
                throw;
            }
            finally
            {
                if (blClear) { ht.Clear(); }
                logSql.Clear();
                sb.Clear();
                cmd.Cancel();
            }

            return dt;
        }

        /// <summary>
        /// 新增/修改/刪除SQL 模組 
        /// </summary>
        /// <param name="sb">SQL語法</param>
        /// <returns></returns>
        public int Execute(StringBuilder sb)
        {
            return Execute(sb, null);
        }

        /// <summary>
        /// 新增/修改/刪除SQL 模組 
        /// </summary>
        /// <param name="sb">SQL語法</param>
        /// <param name="ht">參數物件</param>
        /// <returns></returns>
        public int Execute(StringBuilder sb, Hashtable ht)
        {
            return Execute(sb, ht, false);
        }

        /// <summary>
        /// 新增/修改/刪除SQL 模組 
        /// </summary>
        /// <param name="sb">SQL語法</param>
        /// <param name="ht">參數物件</param>
        /// <param name="blClear">是否清空傳入參數物件</param>
        /// <returns></returns>
        public int Execute(StringBuilder sb, Hashtable ht, Boolean blClear)
        {
            SqlConnection conn = Connection();
            int cnt = 0;
            SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
            logSql.Append(sb);

            try
            {
                conn.Open();
                if (ht != null)
                {
                    foreach (DictionaryEntry objDE in ht)
                    {
                        if (!SQLIN(cmd, objDE, sb))
                        {
                            logSql.Replace(objDE.Key.ToString(), "'" + objDE.Value.ToString() + "'");
                            cmd.Parameters.AddWithValue(objDE.Key.ToString(), objDE.Value);
                        }
                    }
                }

                TimeSpan dtStart = DateTime.Now.TimeOfDay;
                cnt = cmd.ExecuteNonQuery();
                TimeSpan dtEnd = DateTime.Now.TimeOfDay;
                TimeSpan executionTime = dtEnd.Subtract(dtStart);
                //Sql 執行超過500毫秒
                if (executionTime.Milliseconds > dbExecTime)
                {
                    StackTrace ss = new StackTrace(true);
                    MethodBase mb = ss.GetFrame(2).GetMethod();
                    FB2Log.Warn(mb.DeclaringType.Name, logSql.ToString());
                }

                FB2Log.Debug(logSql.ToString());
            }
            catch (Exception ex)
            {
                StackTrace ss = new StackTrace(true);
                MethodBase mb = ss.GetFrame(2).GetMethod();
                FB2Log.Error(mb.DeclaringType.Name, "SQL 錯誤:" + logSql.ToString(), ex);
                throw;
            }
            finally
            {
                if (blClear) { ht.Clear(); }
                logSql.Clear();
                sb.Clear();
                cmd.Cancel();
                conn.Close();
                conn.Dispose();
            }

            return cnt;
        }

        /// <summary>
        /// StoredProcedure SQL 模組 
        /// </summary>
        /// <param name="sb">StoredProcedure</param>
        /// <param name="ht">參數物件</param>
        /// <param name="blClear">是否清空傳入參數物件</param>
        /// <returns></returns>
        public int ExecuteSP(StringBuilder sb, Hashtable ht, Boolean blClear)
        {
            SqlConnection conn = Connection();
            int cnt = 0;
            SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
            cmd.CommandTimeout = 600;
            cmd.CommandType = CommandType.StoredProcedure;
            logSql.Append(sb);

            try
            {
                conn.Open();
                if (ht != null)
                {
                    foreach (DictionaryEntry objDE in ht)
                    {
                        if (!SQLIN(cmd, objDE, sb))
                        {
                            logSql.Replace(objDE.Key.ToString(), "'" + objDE.Value.ToString() + "'");
                            cmd.Parameters.AddWithValue(objDE.Key.ToString(), objDE.Value);
                        }
                    }
                }

                TimeSpan dtStart = DateTime.Now.TimeOfDay;
                cnt = cmd.ExecuteNonQuery();
                TimeSpan dtEnd = DateTime.Now.TimeOfDay;
                TimeSpan executionTime = dtEnd.Subtract(dtStart);
                //Sql 執行超過500毫秒
                if (executionTime.Milliseconds > dbExecTime)
                {
                    StackTrace ss = new StackTrace(true);
                    MethodBase mb = ss.GetFrame(2).GetMethod();
                    FB2Log.Warn(mb.DeclaringType.Name, logSql.ToString());
                }

                FB2Log.Debug(logSql.ToString());
            }
            catch (Exception ex)
            {
                StackTrace ss = new StackTrace(true);
                MethodBase mb = ss.GetFrame(2).GetMethod();
                FB2Log.Error(mb.DeclaringType.Name, "SQL 錯誤:" + logSql.ToString(), ex);
                throw;
            }
            finally
            {
                if (blClear) { ht.Clear(); }
                logSql.Clear();
                sb.Clear();
                cmd.Cancel();
                conn.Close();
                conn.Dispose();
            }

            return cnt;
        }

        /// <summary>
        /// StoredProcedure SQL 模組 
        /// </summary>
        /// <param name="sb">StoredProcedure</param>
        /// <param name="ht">參數物件</param>
        /// <param name="blClear">是否清空傳入參數物件</param>
        /// <returns></returns>
        public SqlParameterCollection ExecuteSP(StringBuilder sb, Hashtable ht, Hashtable htOut, Boolean blClear)
        {
            SqlConnection conn = Connection();
            int cnt = 0;
            SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
            cmd.CommandTimeout = 600;
            cmd.CommandType = CommandType.StoredProcedure;
            logSql.Append(sb);

            try
            {
                conn.Open();
                if (ht != null)
                {
                    foreach (DictionaryEntry objDE in ht)
                    {
                        if (!SQLIN(cmd, objDE, sb))
                        {
                            logSql.Replace(objDE.Key.ToString(), "'" + objDE.Value.ToString() + "'");
                            cmd.Parameters.AddWithValue(objDE.Key.ToString(), objDE.Value);
                        }
                    }
                }

                if (htOut != null)
                {
                    foreach (DictionaryEntry objDE in htOut)
                    {
                        if (!SQLIN(cmd, objDE, sb))
                        {
                            logSql.Replace(objDE.Key.ToString(), "'" + objDE.Value.ToString() + "'");
                            cmd.Parameters.Add(objDE.Key.ToString(), SqlDbType.NVarChar, 500);//.AddWithValue(objDE.Key.ToString(), objDE.Value);
                            cmd.Parameters[objDE.Key.ToString()].Direction = ParameterDirection.Output;

                        }
                    }
                }

                TimeSpan dtStart = DateTime.Now.TimeOfDay;
                cnt = cmd.ExecuteNonQuery();
                TimeSpan dtEnd = DateTime.Now.TimeOfDay;
                TimeSpan executionTime = dtEnd.Subtract(dtStart);
                //Sql 執行超過500毫秒
                if (executionTime.Milliseconds > dbExecTime)
                {
                    StackTrace ss = new StackTrace(true);
                    MethodBase mb = ss.GetFrame(2).GetMethod();
                    FB2Log.Warn(mb.DeclaringType.Name, logSql.ToString());
                }

                FB2Log.Debug(logSql.ToString());
            }
            catch (Exception ex)
            {
                StackTrace ss = new StackTrace(true);
                MethodBase mb = ss.GetFrame(2).GetMethod();
                FB2Log.Error(mb.DeclaringType.Name, "SQL 錯誤:" + logSql.ToString(), ex);
                throw;
            }
            finally
            {
                if (blClear) { ht.Clear(); }
                logSql.Clear();
                sb.Clear();
                cmd.Cancel();
                conn.Close();
                conn.Dispose();
            }

            return cmd.Parameters;
        }

        /// <summary>
        /// StoredProcedure SQL 模組 
        /// </summary>
        /// <param name="sb">StoredProcedure</param>
        /// <param name="ht">參數物件</param>
        /// <param name="blClear">是否清空傳入參數物件</param>
        /// <returns></returns>
        public int ExecuteSPT(StringBuilder sb, Hashtable ht, Boolean blClear)
        {
            SqlConnection conn = Connection();
            GetTranMd();
            int cnt = 0;
            SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
            cmd.CommandTimeout = 600;
            cmd.CommandType = CommandType.StoredProcedure;
            logSql.Append(sb);

            try
            {
                conn.Open();

                if (ht != null)
                {
                    foreach (DictionaryEntry objDE in ht)
                    {
                        if (!SQLIN(cmd, objDE, sb))
                        {
                            logSql.Replace(objDE.Key.ToString(), "'" + objDE.Value.ToString() + "'");
                            cmd.Parameters.AddWithValue(objDE.Key.ToString(), objDE.Value);
                        }
                    }
                }

                TimeSpan dtStart = DateTime.Now.TimeOfDay;
                cnt = cmd.ExecuteNonQuery();
                TimeSpan dtEnd = DateTime.Now.TimeOfDay;
                TimeSpan executionTime = dtEnd.Subtract(dtStart);
                //Sql 執行超過500毫秒
                if (executionTime.Milliseconds > dbExecTime)
                {
                    StackTrace ss = new StackTrace(true);
                    MethodBase mb = ss.GetFrame(2).GetMethod();
                    FB2Log.Warn(mb.DeclaringType.Name, logSql.ToString());
                }

                FB2Log.Debug(logSql.ToString());
            }
            catch (Exception ex)
            {
                StackTrace ss = new StackTrace(true);
                MethodBase mb = ss.GetFrame(2).GetMethod();
                FB2Log.Error(mb.DeclaringType.Name, "SQL 錯誤:" + logSql.ToString(), ex);
                throw;
            }
            finally
            {
                if (blClear) { ht.Clear(); }
                logSql.Clear();
                sb.Clear();
                cmd.Cancel();
                conn.Close();
                conn.Dispose();
            }

            return cnt;
        }
        /// <summary>
        /// 查詢SQL 模組
        /// </summary>
        /// <param name="sb">SQL語法</param>
        /// <param name="ht">參數物件</param>
        /// <param name="blClear">是否清空傳入參數物件</param>
        /// <returns></returns>
        public DataTable QuerySP(StringBuilder sb, Hashtable ht, Boolean blClear)
        {
            SqlConnection conn = Connection();
            DataTable dt = new DataTable();
            SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
            cmd.CommandType = CommandType.StoredProcedure;
            logSql.Append(sb);

            try
            {
                conn.Open();
                if (ht != null)
                {
                    foreach (DictionaryEntry objDE in ht)
                    {
                        if (!SQLIN(cmd, objDE, sb))
                        {
                            logSql.Replace(objDE.Key.ToString(), "'" + objDE.Value.ToString() + "'");
                            cmd.Parameters.AddWithValue(objDE.Key.ToString(), objDE.Value);
                        }
                    }
                }

                dt.BeginLoadData();
                TimeSpan dtStart = DateTime.Now.TimeOfDay;
                dt.Load(cmd.ExecuteReader());
                TimeSpan dtEnd = DateTime.Now.TimeOfDay;
                TimeSpan executionTime = dtEnd.Subtract(dtStart);
                //Sql 執行超過500毫秒
                if (executionTime.Milliseconds > dbExecTime)
                {
                    StackTrace ss = new StackTrace(true);
                    MethodBase mb = ss.GetFrame(2).GetMethod();
                    FB2Log.Warn(mb.DeclaringType.Name, logSql.ToString());
                }

                dt.EndLoadData();
                FB2Log.Debug(logSql.ToString());
            }
            catch (Exception ex)
            {
                StackTrace ss = new StackTrace(true);
                MethodBase mb = ss.GetFrame(2).GetMethod();
                FB2Log.Error(mb.DeclaringType.Name, "SQL 錯誤:" + logSql.ToString(), ex);
                throw;
            }
            finally
            {
                if (blClear) { ht.Clear(); }
                logSql.Clear();
                sb.Clear();
                cmd.Cancel();
                conn.Close();
                conn.Dispose();
            }

            return dt;
        }

        /// <summary>
        /// StoredProcedure SQL 模組 
        /// </summary>
        /// <param name="sb">StoredProcedure</param>
        /// <param name="ht">參數物件</param>
        /// <param name="blClear">是否清空傳入參數物件</param>
        /// <param name="rtnPara">SP中所設定的回傳參數名稱</param>
        /// <returns>@rtn </returns>
        public string getSP_String(StringBuilder sb, Hashtable ht, Boolean blClear, string rtnPara)
        {
            SqlConnection conn = Connection();
            SqlParameter sp = null;
            SqlCommand cmd = new SqlCommand(sb.ToString(), conn);
            cmd.CommandTimeout = 600;
            cmd.CommandType = CommandType.StoredProcedure;
            logSql.Append(sb);

            try
            {
                conn.Open();
                if (ht != null)
                {
                    foreach (DictionaryEntry objDE in ht)
                    {
                        if (!SQLIN(cmd, objDE, sb))
                        {
                            logSql.Replace(objDE.Key.ToString(), "'" + objDE.Value.ToString() + "'");
                            cmd.Parameters.AddWithValue(objDE.Key.ToString(), objDE.Value);
                        }
                    }
                }

                TimeSpan dtStart = DateTime.Now.TimeOfDay;
                sp = cmd.Parameters.Add(rtnPara, SqlDbType.VarChar, 100);
                sp.Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                TimeSpan dtEnd = DateTime.Now.TimeOfDay;
                TimeSpan executionTime = dtEnd.Subtract(dtStart);
                //Sql 執行超過500毫秒
                if (executionTime.Milliseconds > dbExecTime)
                {
                    StackTrace ss = new StackTrace(true);
                    MethodBase mb = ss.GetFrame(2).GetMethod();
                    FB2Log.Warn(mb.DeclaringType.Name, logSql.ToString());
                }

                FB2Log.Debug(logSql.ToString());
            }
            catch (Exception ex)
            {
                StackTrace ss = new StackTrace(true);
                MethodBase mb = ss.GetFrame(2).GetMethod();
                FB2Log.Error(mb.DeclaringType.Name, "SQL 錯誤:" + logSql.ToString(), ex);
                throw;
            }
            finally
            {
                if (blClear) { ht.Clear(); }
                logSql.Clear();
                sb.Clear();
                cmd.Cancel();
                conn.Close();
                conn.Dispose();
            }

            return Convert.ToString(sp.Value);
        }

        /// <summary>
        /// 新增/修改/刪除SQL 模組 transaction
        /// </summary>
        /// <param name="sb">SQL語法</param>
        /// <returns></returns>
        public int ExecuteT(StringBuilder sb)
        {
            return ExecuteT(sb, null);
        }

        /// <summary>
        /// 新增/修改/刪除SQL 模組 transaction
        /// </summary>
        /// <param name="sb">SQL語法</param>
        /// <param name="ht">參數物件</param>
        /// <returns></returns>
        public int ExecuteT(StringBuilder sb, Hashtable ht)
        {
            return ExecuteT(sb, ht, false);
        }

        /// <summary>
        /// 新增/修改/刪除SQL 模組 transaction
        /// </summary>
        /// <param name="sb">SQL語法</param>
        /// <param name="ht">參數物件</param>
        /// <param name="blClear">是否清空傳入參數物件</param>
        /// <returns></returns>
        public int ExecuteT(StringBuilder sb, Hashtable ht, Boolean blClear)
        {
            int cnt = 0;
            GetTranMd();
            SqlCommand cmd = new SqlCommand(sb.ToString(), tranMd.TranConn, tranMd.Tran);
            cmd.CommandTimeout = 600;
            logSql.Append(sb);

            try
            {
                if (ht != null)
                {
                    foreach (DictionaryEntry objDE in ht)
                    {
                        if (!SQLIN(cmd, objDE, sb))
                        {
                            logSql.Replace(objDE.Key.ToString(), "'" + objDE.Value.ToString() + "'");
                            cmd.Parameters.AddWithValue(objDE.Key.ToString(), objDE.Value);
                        }
                    }
                }

                TimeSpan dtStart = DateTime.Now.TimeOfDay;
                cnt = cmd.ExecuteNonQuery();
                TimeSpan dtEnd = DateTime.Now.TimeOfDay;
                TimeSpan executionTime = dtEnd.Subtract(dtStart);
                //Sql 執行超過500毫秒
                if (executionTime.Milliseconds > dbExecTime)
                {
                    StackTrace ss = new StackTrace(true);
                    MethodBase mb = ss.GetFrame(2).GetMethod();
                    FB2Log.Warn(mb.DeclaringType.Name, "transaction:" + logSql.ToString());
                }

                FB2Log.Debug("transaction SQL:" + logSql.ToString());
            }
            catch (Exception ex)
            {
                StackTrace ss = new StackTrace(true);
                MethodBase mb = ss.GetFrame(2).GetMethod();
                FB2Log.Error(mb.DeclaringType.Name, "transaction SQL 錯誤:" + logSql.ToString(), ex);
                throw;
            }
            finally
            {
                if (blClear) { ht.Clear(); }
                logSql.Clear();
                sb.Clear();
                cmd.Cancel();
            }

            return cnt;
        }

        /****************************************************************************************************************************/

        /// <summary>
        /// SQL In 語法陣列資料(String[],int[],lang[])處理
        /// </summary>
        /// <param name="cmd">DB連結物件</param>
        /// <param name="objDE">參數物件</param>
        /// <param name="sb">SQL語法</param>
        /// <returns></returns>
        private Boolean SQLIN(SqlCommand cmd, DictionaryEntry objDE, StringBuilder sb)
        {
            Boolean isIn = true;
            String key = objDE.Key.ToString();
            int i = 1;
            StringBuilder logT = new StringBuilder(), sqlT = new StringBuilder();
            if (objDE.Value == null)
            {
                logSql.Replace(objDE.Key.ToString(), "''");
                cmd.Parameters.AddWithValue(objDE.Key.ToString(), "");
            }
            else if (objDE.Value is String[])
            {
                foreach (String str in (String[])objDE.Value)
                {
                    if (i == 1)
                    {
                        logT.Append("'" + str + "'");
                        sqlT.Append(key + "_CON_" + i.ToString());
                    }
                    else
                    {
                        logT.Append(",'" + str + "'");
                        sqlT.Append("," + key + "_CON_" + i.ToString());
                    }
                    cmd.Parameters.AddWithValue(key + "_CON_" + i.ToString(), str);
                    i++;
                }

                sb.Replace(key, sqlT.ToString());
                logSql.Replace(key, logT.ToString());
                cmd.CommandText = sb.ToString();
            }
            else if (objDE.Value is int[])
            {
                foreach (int ii in (int[])objDE.Value)
                {
                    if (i == 1)
                    {
                        logT.Append("'" + ii.ToString() + "'");
                        sqlT.Append(key + "_CON_" + i.ToString());
                    }
                    else
                    {
                        logT.Append(",'" + ii.ToString() + "'");
                        sqlT.Append("," + key + "_CON_" + i.ToString());
                    }
                    cmd.Parameters.AddWithValue(key + "_CON_" + i.ToString(), ii);
                    i++;
                }

                sb.Replace(key, sqlT.ToString());
                logSql.Replace(key, logT.ToString());
                cmd.CommandText = sb.ToString();
            }
            else if (objDE.Value is long[])
            {
                foreach (long ii in (long[])objDE.Value)
                {
                    if (i == 1)
                    {
                        logT.Append("'" + ii.ToString() + "'");
                        sqlT.Append(key + "_CON_" + i.ToString());
                    }
                    else
                    {
                        logT.Append(",'" + ii.ToString() + "'");
                        sqlT.Append("," + key + "_CON_" + i.ToString());
                    }
                    cmd.Parameters.AddWithValue(key + "_CON_" + i.ToString(), ii);
                    i++;
                }

                sb.Replace(key, sqlT.ToString());
                logSql.Replace(key, logT.ToString());
                cmd.CommandText = sb.ToString();
            }
            else if (objDE.Value is HashSet<string>)
            {
                foreach (string str in (HashSet<string>)objDE.Value)
                {
                    if (i == 1)
                    {
                        logT.Append("'" + str + "'");
                        sqlT.Append(key + "_CON_" + i.ToString());
                    }
                    else
                    {
                        logT.Append(",'" + str + "'");
                        sqlT.Append("," + key + "_CON_" + i.ToString());
                    }
                    cmd.Parameters.AddWithValue(key + "_CON_" + i.ToString(), str);
                    i++;
                }

                sb.Replace(key, sqlT.ToString());
                logSql.Replace(key, logT.ToString());
                cmd.CommandText = sb.ToString();
            }
            else
            {
                isIn = false;
            }

            return isIn;
        }
    }
}