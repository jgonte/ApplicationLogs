using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationLogs.Logs
{
    public class GetApplicationLogByIdQueryAggregate : GetByIdQueryAggregate<ApplicationLog, int?, ApplicationLogOutputDto>
    {
        public GetApplicationLogByIdQueryAggregate() : this(null)
        {
        }

        public GetApplicationLogByIdQueryAggregate(HashSet<(string, IEntity)> processedEntities = null) : base(new DomainFramework.DataAccess.RepositoryContext(ApplicationLogsConnectionClass.GetConnectionName()), processedEntities)
        {
            var context = (DomainFramework.DataAccess.RepositoryContext)RepositoryContext;

            ApplicationLogQueryRepository.Register(context);
        }

        public override void PopulateDto()
        {
            OutputDto.Id = RootEntity.Id.Value;

            OutputDto.Type = RootEntity.Type;

            OutputDto.UserId = RootEntity.UserId;

            OutputDto.Source = RootEntity.Source;

            OutputDto.Message = RootEntity.Message;

            OutputDto.Data = RootEntity.Data;

            OutputDto.Url = RootEntity.Url;

            OutputDto.StackTrace = RootEntity.StackTrace;

            OutputDto.HostIpAddress = RootEntity.HostIpAddress;

            OutputDto.UserIpAddress = RootEntity.UserIpAddress;

            OutputDto.UserAgent = RootEntity.UserAgent;

            OutputDto.When = RootEntity.When;
        }

    }
}