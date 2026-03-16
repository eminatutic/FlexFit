using System.Collections.Concurrent;
using FlexFit.MongoModels;
using FlexFit.MongoModels.Models;
using FlexFit.MongoModels.Repositories;
using FlexFit.Repositories;
using Microsoft.AspNetCore.Http;

namespace FlexFit.Middleware
{
    public class SmartThrottlingMiddleware
    {
        private readonly RequestDelegate _next;
        private static readonly ConcurrentDictionary<string, List<DateTime>> RequestLog = new();

        // Limit za svaki tip rute
        private readonly Dictionary<string, (int Limit, TimeSpan Period)> _limits = new()
        {
            { "/api/member/check-availability", (30, TimeSpan.FromMinutes(1)) },
            { "/api/member/activate-card", (3, TimeSpan.FromMinutes(1)) },
            { "/api/employee/scan", (60, TimeSpan.FromMinutes(1)) }
        };

        public SmartThrottlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Scoped servise uzimamo ovde po request-u
            var _entryLogRepository = context.RequestServices.GetRequiredService<EntryLogRepository>();
            var _violationRepository = context.RequestServices.GetRequiredService<RateLimitViolationRepository>();

            var ip = context.Connection.RemoteIpAddress?.ToString() ?? "unknown";
            var path = context.Request.Path.ToString().ToLower();

            // Provera da li postoji limit za ovu rutu
            if (_limits.TryGetValue(path, out var limitInfo))
            {
                var now = DateTime.UtcNow;

                var key = $"{ip}:{path}";
                if (!RequestLog.ContainsKey(key))
                    RequestLog[key] = new List<DateTime>();

                // Očisti stare zapise
                RequestLog[key] = RequestLog[key].Where(t => now - t < limitInfo.Period).ToList();

                if (RequestLog[key].Count >= limitInfo.Limit)
                {
                    // Logovanje u MongoDB
                    await _violationRepository.AddAsync(new RateLimitViolation
                    {
                        IpAddress = ip,
                        Route = path,
                        Timestamp = now
                    });

                    context.Response.StatusCode = 429;
                    context.Response.Headers["Retry-After"] = limitInfo.Period.TotalSeconds.ToString();
                    await context.Response.WriteAsync("Too Many Requests. Probajte kasnije.");
                    return;
                }

                RequestLog[key].Add(now);
            }

            // Nastavi dalje u pipeline
            await _next(context);
        }
    }
}