﻿using Logger.Appenders;
using Logger.Enums;
using System.Text;

namespace Logger.Loggers
{
    public class Loggger : ILogger
    {
        private readonly IAppender[] appenders;
        public Loggger(params IAppender[] appenders)
        {
            this.appenders = appenders;
        }
        public void Critical(string date, string message)
        {
            AppendToAppenders(date, ReportLevel.CRITICAL, message);
        }

        public void Error(string date, string message)
        {
            AppendToAppenders(date, ReportLevel.ERROR, message);
        }

        public void Fatal(string date, string message)
        {
            AppendToAppenders(date, ReportLevel.FATAL, message);
        }

        public void Info(string date, string message)
        {
            AppendToAppenders(date, ReportLevel.INFO, message);
        }

        public void Warning(string date, string message)
        {
            AppendToAppenders(date, ReportLevel.WARNING, message);
        }
        public override string ToString()
        {
            var sb = new StringBuilder();

            foreach (var appender in appenders)
            {
                sb.AppendLine(appender.ToString());
            }
            return sb.ToString().TrimEnd();
        }
        private void AppendToAppenders(string date, ReportLevel reportLevel, string message)
        {
            foreach (var appender in appenders)
            {
                if (reportLevel >= appender.ReportLevel)
                {
                    appender.MessagesCount++;
                    appender.Append(date, reportLevel, message);
                }
            }
        }
    }
}
