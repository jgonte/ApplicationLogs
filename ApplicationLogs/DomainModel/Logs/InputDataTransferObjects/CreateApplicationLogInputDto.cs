using DomainFramework.Core;
using System;
using System.Collections.Generic;
using Utilities.Validation;

namespace ApplicationLogs.Logs
{
    public class CreateApplicationLogInputDto : IInputDataTransferObject
    {
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

        public virtual void Validate(ValidationResult result)
        {
            ((int)Type).ValidateRequired(result, nameof(Type));

            UserId.ValidateMaxLength(result, nameof(UserId), 50);

            Source.ValidateMaxLength(result, nameof(Source), 256);

            Message.ValidateRequired(result, nameof(Message));

            Message.ValidateMaxLength(result, nameof(Message), 1024);

            Data.ValidateMaxLength(result, nameof(Data), 1024);

            Url.ValidateMaxLength(result, nameof(Url), 512);

            StackTrace.ValidateMaxLength(result, nameof(StackTrace), 2048);

            HostIpAddress.ValidateMaxLength(result, nameof(HostIpAddress), 25);

            UserIpAddress.ValidateMaxLength(result, nameof(UserIpAddress), 25);

            UserAgent.ValidateMaxLength(result, nameof(UserAgent), 25);
        }

    }
}