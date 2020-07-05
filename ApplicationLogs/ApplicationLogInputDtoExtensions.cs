using ApplicationLogs.Logs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System;
using System.Linq;

namespace ApplicationLogs
{
    public static class ApplicationLogInputDtoExtensions
    {
        public static void AddHttpContextInformation(this CreateApplicationLogInputDto log, HttpContext httpContext)
        {
            var request = httpContext.Request;

            log.UserAgent = request.Headers["user-agent"].FirstOrDefault();

            var connection = httpContext.Features.Get<IHttpConnectionFeature>();

            log.UserIpAddress = connection?.RemoteIpAddress?.ToString();

            log.HostIpAddress = connection?.LocalIpAddress?.ToString();

            log.Url = new UriBuilder
            {
                Scheme = request.Scheme,
                Host = request.Host.Host,
                Port = request.Host.Port.GetValueOrDefault(80),
                Path = request.Path.ToString(),
                Query = request.QueryString.ToString()
            }
            .ToString();
        }
    }
}
