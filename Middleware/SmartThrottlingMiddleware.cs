using System.Collections.Concurrent;
namespace FlexFit.Middleware
{
    

    public class SmartThrottlingMiddleware
    {
        private readonly RequestDelegate _next;

        private static ConcurrentDictionary<string, List<DateTime>> requests = new();

        public SmartThrottlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            var ip = context.Connection.RemoteIpAddress.ToString();

            if (!requests.ContainsKey(ip))
                requests[ip] = new List<DateTime>();

            requests[ip].Add(DateTime.UtcNow);

            requests[ip] = requests[ip]
                .Where(x => x > DateTime.UtcNow.AddMinutes(-1))
                .ToList();

            if (requests[ip].Count > 30)
            {
                context.Response.StatusCode = 429;
                await context.Response.WriteAsync("Too many requests");
                return;
            }

            await _next(context);
        }
    }
}
