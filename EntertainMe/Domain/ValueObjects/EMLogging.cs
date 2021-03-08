using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace EntertainMe.Domain.ValueObjects
{
    public enum LoggingType
    {
        Info,
        Warning,
        Error,
        Fatal
    }

    public class EMLogging
    {
        /// <summary>
        /// Path to store log file in
        /// </summary>
        private string _LogPath { get; set; }
        /// <summary>
        /// Name to use for log file
        /// </summary>
        private string _LogName { get; set; }

        /// <summary>
        /// Should entries in the log file be time stamped
        /// </summary>
        public bool TimeStamp { get; set; } = true;

        /// <summary>
        /// Should the log file name include a date stamp
        /// </summary>
        public bool DateStamp { get; set; } = false;

        /// <summary>
        /// Initialize logging object
        /// </summary>
        public EMLogging()
        { 
        }

        /// <summary>
        /// Initialize logging object
        /// </summary>
        /// <param name="logFilePath">Path log file will be written to</param>
        /// <param name="logFileName">Name to use for log file.  Do not supply an extension, .log is automatially appended</param>
        public EMLogging(string logFilePath, string logFileName)
        {
            SetLoggingInfo(logFilePath, logFileName);
        }

        /// <summary>
        /// Set logging information
        /// </summary>
        /// <param name="logFilePath">Path log file will be written to</param>
        /// <param name="logFileName">Name to use for log file.  Do not supply an extension, .log is automatially appended</param>
        public void SetLoggingInfo(string logFilePath, string logFileName)
        {

            _LogPath = logFilePath + (!logFileName.EndsWith(@"\") ? @"\" : "");
            _LogName = logFileName;
            Directory.CreateDirectory(_LogPath);
        }

        /// <summary>
        /// Write an entry in to the log file
        /// </summary>
        /// <param name="logData">Entry into the log file</param>
        /// <param name="logType">Type of the log entry</param>
        public void Log(string logData, LoggingType logType = LoggingType.Info)
        {
            if (string.IsNullOrEmpty(_LogPath) || string.IsNullOrEmpty(_LogName))
            {
                throw new Exception("Path and/or name for log file not set");
            }

            string logDataPrefix = logType.ToString() + "::";
            
            string timeStamp = DateTime.Now.ToString() + " - ";
            string formattedLogLine = $"{(!TimeStamp ? "" : timeStamp)}{logDataPrefix + logData}";
            List<string> logLine = new List<string> { formattedLogLine };

            string dateStamp = "." + DateTime.Now.ToString("yyyyMMdd");
            string fileName = $"{_LogName}{(!DateStamp ? "" : dateStamp)}.log";

            File.AppendAllLines(_LogPath + fileName, logLine );
        }

    }
}
