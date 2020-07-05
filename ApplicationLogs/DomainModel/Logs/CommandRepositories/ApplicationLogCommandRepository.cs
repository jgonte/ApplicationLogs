using DataAccess;
using DomainFramework.Core;
using DomainFramework.DataAccess;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace ApplicationLogs.Logs
{
    public class ApplicationLogCommandRepository : EntityCommandRepository<ApplicationLog>
    {
        protected override Command CreateInsertCommand(ApplicationLog entity, IAuthenticatedUser user, string selector)
        {
            var command = Query<ApplicationLog>
                .Single()
                .Connection(ApplicationLogsConnectionClass.GetConnectionName())
                .StoredProcedure("[Logs].[pApplicationLog_Insert]")
                .Parameters(
                    p => p.Name("type").Value(entity.Type),
                    p => p.Name("userId").Value(entity.UserId),
                    p => p.Name("source").Value(entity.Source),
                    p => p.Name("message").Value(entity.Message),
                    p => p.Name("data").Value(entity.Data),
                    p => p.Name("url").Value(entity.Url),
                    p => p.Name("stackTrace").Value(entity.StackTrace),
                    p => p.Name("hostIpAddress").Value(entity.HostIpAddress),
                    p => p.Name("userIpAddress").Value(entity.UserIpAddress),
                    p => p.Name("userAgent").Value(entity.UserAgent)
                )
                .Instance(entity)
                .MapProperties(
                    p => p.Name("Id").Index(0),
                    p => p.Name("When").Index(1)
                );

            return command;
        }

        protected override void HandleInsert(Command command)
        {
            ((SingleQuery<ApplicationLog>)command).Execute();
        }

        protected async override Task HandleInsertAsync(Command command)
        {
            await ((SingleQuery<ApplicationLog>)command).ExecuteAsync();
        }

        protected override Command CreateDeleteCommand(ApplicationLog entity, IAuthenticatedUser user, string selector)
        {
            switch (selector)
            {
                case "DeleteOlderLogs": return Command
                    .NonQuery()
                    .Connection(ApplicationLogsConnectionClass.GetConnectionName())
                    .StoredProcedure("[Logs].[pApplicationLog_DeleteOlderLogs]")
                    .Parameters(
                        p => p.Name("when").Value(entity.When)
                    );

                default: return Command
                    .NonQuery()
                    .Connection(ApplicationLogsConnectionClass.GetConnectionName())
                    .StoredProcedure("[Logs].[pApplicationLog_Delete]")
                    .Parameters(
                        p => p.Name("applicationLogId").Value(entity.Id)
                    );
            }
        }

        protected override bool HandleDelete(Command command)
        {
            var result = ((NonQueryCommand)command).Execute();

            return result.AffectedRows > 0;
        }

        protected async override Task<bool> HandleDeleteAsync(Command command)
        {
            var result = await ((NonQueryCommand)command).ExecuteAsync();

            return result.AffectedRows > 0;
        }

    }
}