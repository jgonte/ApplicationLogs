using DomainFramework.Core;
using System;
using System.Collections.Generic;
using Utilities.Validation;

namespace ApplicationLogs.Logs
{
    public class DeleteApplicationLogsInputDto : IInputDataTransferObject
    {
        public DateTime When { get; set; }

        public virtual void Validate(ValidationResult result)
        {
        }

    }
}