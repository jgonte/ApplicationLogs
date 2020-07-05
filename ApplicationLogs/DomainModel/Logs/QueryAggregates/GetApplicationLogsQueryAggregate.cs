using DomainFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationLogs.Logs
{
    public class GetApplicationLogsQueryAggregate : QueryAggregateCollection<ApplicationLog, ApplicationLogOutputDto, GetApplicationLogByIdQueryAggregate>
    {
        public GetApplicationLogsQueryAggregate() : base(new DomainFramework.DataAccess.RepositoryContext(ApplicationLogsConnectionClass.GetConnectionName()))
        {
            var context = (DomainFramework.DataAccess.RepositoryContext)RepositoryContext;

            ApplicationLogQueryRepository.Register(context);
        }

        public (int, IEnumerable<ApplicationLogOutputDto>) Get(CollectionQueryParameters queryParameters)
        {
            var repository = (ApplicationLogQueryRepository)RepositoryContext.GetQueryRepository(typeof(ApplicationLog));

            var (count, entities) = repository.Get(queryParameters);

            var data = new Tuple<int, IEnumerable<IEntity>>(count, entities);

            return Get(data);
        }

        public async Task<(int, IEnumerable<ApplicationLogOutputDto>)> GetAsync(CollectionQueryParameters queryParameters)
        {
            var repository = (ApplicationLogQueryRepository)RepositoryContext.GetQueryRepository(typeof(ApplicationLog));

            var (count, entities) = await repository.GetAsync(queryParameters);

            var data = new Tuple<int, IEnumerable<IEntity>>(count, entities);

            return await GetAsync(data);
        }

    }
}