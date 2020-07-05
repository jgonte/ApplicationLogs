using DomainFramework.Core;
using System;
using System.Collections.Generic;

namespace ApplicationLogs.Logs
{
    public class ApplicationLogOutputDto : IOutputDataTransferObject
    {
        public int Id { get; set; }

        public ApplicationLog.LogTypes Type { get; set; }

        public string UserId { get; set; }

        public string Source { get; set; }

        public string Message { get; set; }

        public string Data { get; set; }

        public string Url { get; set; }

        public string StackTrace { get; set; }

        public string HostIpAddress { get; set; }

        public string UserIpAddress { get; set; }

        public string UserAgent { get; set; }

        public DateTime When { get; set; }

    }
}