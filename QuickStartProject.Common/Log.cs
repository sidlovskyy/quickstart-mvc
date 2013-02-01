using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using log4net;
using log4net.Config;

namespace QuickStartProject.Common
{
    public static class Log
    {
        private static readonly Dictionary<Type, ILog> _loggers = new Dictionary<Type, ILog>();
        private static bool _logInitialized;
        private static readonly object _lock = new object();

        public static string SerializeException(Exception exception)
        {
            return SerializeException(exception, string.Empty);
        }

        private static string SerializeException(Exception e, string exceptionMessage)
        {
            if (e == null) return string.Empty;

            exceptionMessage = string.Format(
                CultureInfo.InvariantCulture,
                "{0}{1}{2}\n{3}",
                exceptionMessage,
                string.IsNullOrEmpty(exceptionMessage) ? string.Empty : "\n\n",
                e.Message,
                e.StackTrace);

            if (e.InnerException != null)
            {
                exceptionMessage = SerializeException(e.InnerException, exceptionMessage);
            }

            return exceptionMessage;
        }

        private static ILog GetLogger(Type source)
        {
            lock (_lock)
            {
                if (_loggers.ContainsKey(source))
                {
                    return _loggers[source];
                }

                ILog logger = LogManager.GetLogger(source);
                EnsureInitialized();
                _loggers.Add(source, logger);
                return logger;
            }
        }

        /* Log a message object */

        private static Type GetLogerCaller()
        {
            return new StackFrame(3, true).GetMethod().ReflectedType;
        }

        public static void Debug(string message, params object[] args)
        {
            DebugInternal(string.Format(message, args));
        }

        public static void Debug(string message, Exception ex)
        {
            DebugInternal(message, ex);
        }

        private static void DebugInternal(string message, Exception ex = null)
        {
            Type logType = GetLogerCaller();
            ILog logger = GetLogger(logType);
            if (logger.IsDebugEnabled)
            {
                if (ex == null)
                {
                    logger.Debug(message);
                }
                else
                {
                    logger.Debug(message, ex);
                }
            }
        }

        public static void Info(string message, params object[] args)
        {
            InfoInternal(string.Format(message, args));
        }

        public static void Info(string message, Exception ex)
        {
            InfoInternal(message, ex);
        }

        private static void InfoInternal(object message, Exception ex = null)
        {
            Type logType = GetLogerCaller();
            ILog logger = GetLogger(logType);
            if (logger.IsInfoEnabled)
            {
                if (ex == null)
                {
                    logger.Info(message);
                }
                else
                {
                    logger.Info(message, ex);
                }
            }
        }

        public static void Warn(string message, params object[] args)
        {
            WarnInternal(string.Format(message, args));
        }

        public static void Warn(string message, Exception ex)
        {
            WarnInternal(message, ex);
        }

        private static void WarnInternal(object message, Exception ex = null)
        {
            Type logType = GetLogerCaller();
            ILog logger = GetLogger(logType);
            if (logger.IsWarnEnabled)
            {
                if (ex == null)
                {
                    logger.Warn(message);
                }
                else
                {
                    logger.Warn(message, ex);
                }
            }
        }

        public static void Error(string message, params object[] args)
        {
            ErrorInternal(string.Format(message, args));
        }

        public static void Error(string message, Exception ex)
        {
            ErrorInternal(message, ex);
        }

        private static void ErrorInternal(object message, Exception ex = null)
        {
            Type logType = GetLogerCaller();
            ILog logger = GetLogger(logType);
            if (logger.IsErrorEnabled)
            {
                if (ex == null)
                {
                    logger.Error(message);
                }
                else
                {
                    logger.Error(message, ex);
                }
            }
        }

        public static void Fatal(string message, params object[] args)
        {
            FatalInternal(string.Format(message, args));
        }

        public static void Fatal(string message, Exception ex)
        {
            FatalInternal(message, ex);
        }

        private static void FatalInternal(object message, Exception ex = null)
        {
            Type logType = GetLogerCaller();
            ILog logger = GetLogger(logType);
            if (logger.IsFatalEnabled)
            {
                if (ex == null)
                {
                    logger.Fatal(message);
                }
                else
                {
                    logger.Fatal(message, ex);
                }
            }
        }

        private static void Initialize()
        {
            XmlConfigurator.Configure();
            _logInitialized = true;
        }

        public static void EnsureInitialized()
        {
            if (!_logInitialized)
            {
                Initialize();
            }
        }
    }
}