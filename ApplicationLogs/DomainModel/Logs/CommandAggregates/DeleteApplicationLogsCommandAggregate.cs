using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApplicationLogs.Logs
{
    public class DeleteApplicationLogsCommandAggregate : CommandAggregate<ApplicationLog>
    {
        public DeleteApplicationLogsCommandAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(ApplicationLogsConnectionClass.GetConnectionName()))
        {
        }

        public DeleteApplicationLogsCommandAggregate(DeleteApplicationLogsInputDto log, EntityDependency[] dependencies = null) : base(new DomainFramework.DataAccess.RepositoryContext(ApplicationLogsConnectionClass.GetConnectionName()))
        {
            Initialize(log, dependencies);
        }

        public override void Initialize(IInputDataTransferObject log, EntityDependency[] dependencies)
        {
            Initialize((DeleteApplicationLogsInputDto)log, dependencies);
        }

        private void Initialize(DeleteApplicationLogsInputDto log, EntityDependency[] dependencies)
        {
            RegisterCommandRepositoryFactory<ApplicationLog>(() => new ApplicationLogCommandRepository());

            RootEntity = new ApplicationLog
            {
                When = log.When
            };

            Enqueue(new DeleteEntityCommandOperation<ApplicationLog>(RootEntity, null, "DeleteOlderLogs"));
        }

    }
}