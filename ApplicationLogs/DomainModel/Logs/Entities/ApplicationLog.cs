using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace ApplicationLogs.Logs
{
    public class ApplicationLog : Entity<int?>
    {
        public enum LogTypes
        {
            Information,
            Warning,
            Error,
            Security
        }

        public LogTypes Type { get; set; }

        /// <summary>
        /// The user id logging
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// The system, subsystem, application or library where the message is logged
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// The message being logged, the exception message in case of an exception
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// The parameters or data of interest to what is being logged
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// The url of the request being logged
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// The stack trace of the exception being logged
        /// </summary>
        public string StackTrace { get; set; }

        /// <summary>
        /// The IP address of the server handling the request
        /// </summary>
        public string HostIpAddress { get; set; }

        /// <summary>
        /// The IP address of the request
        /// </summary>
        public string UserIpAddress { get; set; }

        /// <summary>
        /// The IP address of the request
        /// </summary>
        public string UserAgent { get; set; }

        /// <summary>
        /// When it was logged
        /// </summary>
        public DateTime When { get; set; }

    }
}