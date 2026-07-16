using log4net.Core;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Web;

namespace FB2.tw.co.toyota.kuozui.COMMON
{
    public static class FB2Log
    {
        private readonly static Type ThisDeclaringType = typeof(FB2Log);
        private static ILogger defaultLogger;

        /// <summary>
        /// Request Log記錄
        /// </summary>
        /// <param name="message"></param>
        public static void Info(string message)
        {
            Exception ex = null;
            Info(message, ex);
        }

        /// <summary>
        /// Request Log記錄
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public static void Info(string message, Exception ex)
        {
            SetDeclaringTypeName();
            defaultLogger.Log(typeof(FB2Log), log4net.Core.Level.Info, message, ex);
        }

        /// <summary>
        /// Request Log記錄
        /// </summary>
        /// <param name="methobName"></param>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public static void Info(string methodName, string message)
        {
            defaultLogger = LoggerManager.GetLogger(Assembly.GetCallingAssembly(), methodName);
            defaultLogger.Log(typeof(FB2Log), log4net.Core.Level.Info, message, null);
        }

        /// <summary>
        /// 記錄每筆sql
        /// </summary>
        /// <param name="message"></param>
        public static void Debug(string message)
        {
            Debug(message, null);
        }

        /// <summary>
        /// 記錄每筆sql
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public static void Debug(string message, Exception ex)
        {
            defaultLogger = LoggerManager.GetLogger(Assembly.GetCallingAssembly(), "TraceSQL");
            defaultLogger.Log(typeof(FB2Log), log4net.Core.Level.Debug, message, ex);
        }

        /// <summary>
        /// 執行錯誤
        /// </summary>
        /// <param name="message"></param>
        public static void Error(string methodName, string message)
        {
            Error(methodName, message, null);
        }

        /// <summary>
        /// 執行錯誤
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public static void Error(string methodName, string message, Exception ex)
        {   
            defaultLogger = LoggerManager.GetLogger(Assembly.GetCallingAssembly(), methodName);
            defaultLogger.Log(typeof(FB2Log), log4net.Core.Level.Error, message, ex);
        }

        /// <summary>
        /// Sql 執行超過500毫秒
        /// </summary>
        /// <param name="message"></param>
        public static void Warn(string methodName, string message)
        {
            Warn(methodName, message, null);
        }

        /// <summary>
        /// Sql 執行超過500毫秒
        /// </summary>
        /// <param name="message"></param>
        /// <param name="ex"></param>
        public static void Warn(string methodName, string message, Exception ex)
        {
            defaultLogger = LoggerManager.GetLogger(Assembly.GetCallingAssembly(), methodName);
            defaultLogger.Log(typeof(FB2Log), log4net.Core.Level.Warn, message, ex);
        }


        /// <summary>
        /// 設定Method名稱
        /// </summary>
        private static void SetDeclaringTypeName()
        {
            StackTrace ss = new StackTrace(true);
            MethodBase mb = ss.GetFrame(3).GetMethod();
            defaultLogger = LoggerManager.GetLogger(Assembly.GetCallingAssembly(), mb.DeclaringType.Name);
        }
    }
}