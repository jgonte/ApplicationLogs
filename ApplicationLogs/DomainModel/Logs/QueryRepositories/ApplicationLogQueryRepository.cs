using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationLogs.Logs
{
    public class ApplicationLogQueryRepository : EntityQueryRepository<ApplicationLog, int?>
    {
        public override (int, IEnumerable<ApplicationLog>) Get(CollectionQueryParameters queryParameters)
        {
            var result = Query<ApplicationLog>
                .Collection()
                .Connection(ApplicationLogsConnectionClass.GetConnectionName())
                .StoredProcedure("[Logs].[pApplicationLog_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Size(20).Output())
                .Execute();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public async override Task<(int, IEnumerable<ApplicationLog>)> GetAsync(CollectionQueryParameters queryParameters)
        {
            var result = await Query<ApplicationLog>
                .Collection()
                .Connection(ApplicationLogsConnectionClass.GetConnectionName())
                .StoredProcedure("[Logs].[pApplicationLog_Get]")
                .QueryParameters(queryParameters)
                .Parameters(p => p.Name("count").Size(20).Output())
                .ExecuteAsync();

            var count = (string)result.GetParameter("count").Value;

            return (int.Parse(count), result.Data);
        }

        public override ApplicationLog GetById(int? applicationLogId)
        {
            var result = Query<ApplicationLog>
                .Single()
                .Connection(ApplicationLogsConnectionClass.GetConnectionName())
                .StoredProcedure("[Logs].[pApplicationLog_GetById]")
                .Parameters(
                    p => p.Name("applicationLogId").Value(applicationLogId.Value)
                )
                .Execute();

            return result.Data;
        }

        public async override Task<ApplicationLog> GetByIdAsync(int? applicationLogId)
        {
            var result = await Query<ApplicationLog>
                .Single()
                .Connection(ApplicationLogsConnectionClass.GetConnectionName())
                .StoredProcedure("[Logs].[pApplicationLog_GetById]")
                .Parameters(
                    p => p.Name("applicationLogId").Value(applicationLogId.Value)
                )
                .ExecuteAsync();

            return result.Data;
        }

        public static void Register(DomainFramework.DataAccess.RepositoryContext context)
        {
            context.RegisterQueryRepository<ApplicationLog>(new ApplicationLogQueryRepository());
        }

    }
}