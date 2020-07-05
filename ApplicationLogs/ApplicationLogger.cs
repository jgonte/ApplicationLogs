using ApplicationLogs.Logs;
using DomainFramework.Core;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationLogs
{
    public static class ApplicationLogger
    {
        public static async Task<int?> Log(CreateApplicationLogInputDto log)
        {
            var aggregate = new CreateApplicationLogCommandAggregate(log);

            await aggregate.SaveAsync();

            return aggregate.RootEntity.Id;
        }

        public static async Task<int?> Log(
            ApplicationLog.LogTypes type,
            string message,
            string source = null,
            string data = null,
            string stackTrace = null,
            string userId = null,
            HttpContext httpContext = null)
        {
            var log = new CreateApplicationLogInputDto
            {
                Type = type,
                Message = message,
                Source = source,
                Data = data,
                StackTrace = stackTrace,
                UserId = userId
            };

            if (httpContext != null)
            {
                log.AddHttpContextInformation(httpContext);
            }

            return await Log(log);
        }

        public static async Task<int?> LogSecurity(
            string message,
            string source = null,
            string data = null,
            string userId = null,
            HttpContext httpContext = null)
        {
            return await Log(
                type: ApplicationLog.LogTypes.Security,
                message: message,
                source: source,
                data: data,
                userId: userId,
                httpContext: httpContext
            );
        }

        public static async Task<int?> LogInformation(
            string message,
            string source = null,
            string data = null,
            string userId = null,
            HttpContext httpContext = null)
        {
            return await Log(
                type: ApplicationLog.LogTypes.Information,
                message: message,
                source: source,
                data: data,
                userId: userId,
                httpContext: httpContext
            );
        }

        public static async Task<int?> LogWarning(
            string message,
            string source = null,
            string data = null,
            string userId = null,
            HttpContext httpContext = null)
        {
            return await Log(
                type: ApplicationLog.LogTypes.Warning,
                message: message,
                source: source,
                data: data,
                userId: userId,
                httpContext: httpContext
            );
        }

        public static async Task<int?> LogException(Exception exception,
            string source = null,
            string data = null,
            string userId = null,
            HttpContext httpContext = null)
        {
            return await Log(
                type: ApplicationLog.LogTypes.Security,
                message: exception.Message,
                stackTrace: exception.StackTrace,
                source: source,
                data: data,
                userId: userId,
                httpContext: httpContext
            );
        }

        public static async Task DeleteLogs(DateTime dateTime)
        {
            var aggregate = new DeleteApplicationLogsCommandAggregate(new DeleteApplicationLogsInputDto
            {
                When = dateTime
            });

            await aggregate.SaveAsync();
        }

        public static async Task<(int, IEnumerable<ApplicationLogOutputDto>)> Get(CollectionQueryParameters queryParameters)
        {
            var aggregateCollection = new GetApplicationLogsQueryAggregate();

            return await aggregateCollection.GetAsync(queryParameters);
        }
    }
}
