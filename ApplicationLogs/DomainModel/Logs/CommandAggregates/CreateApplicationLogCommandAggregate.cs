using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationLogs.Logs
{
    public class CreateApplicationLogCommandAggregate : CommandAggregate<ApplicationLog>
    {
        public CreateApplicationLogCommandAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(ApplicationLogsConnectionClass.GetConnectionName()))
        {
        }

        public CreateApplicationLogCommandAggregate(CreateApplicationLogInputDto log, EntityDependency[] dependencies = null) : base(new DomainFramework.DataAccess.RepositoryContext(ApplicationLogsConnectionClass.GetConnectionName()))
        {
            Initialize(log, dependencies);
        }

        public override void Initialize(IInputDataTransferObject log, EntityDependency[] dependencies)
        {
            Initialize((CreateApplicationLogInputDto)log, dependencies);
        }

        private void Initialize(CreateApplicationLogInputDto log, EntityDependency[] dependencies)
        {
            RegisterCommandRepositoryFactory<ApplicationLog>(() => new ApplicationLogCommandRepository());

            RootEntity = new ApplicationLog
            {
                Type = log.Type,
                UserId = log.UserId,
                Source = log.Source,
                Message = log.Message,
                Data = log.Data,
                Url = log.Url,
                StackTrace = log.StackTrace,
                HostIpAddress = log.HostIpAddress,
                UserIpAddress = log.UserIpAddress,
                UserAgent = log.UserAgent
            };

            Enqueue(new InsertEntityCommandOperation<ApplicationLog>(RootEntity, dependencies));
        }

    }
}