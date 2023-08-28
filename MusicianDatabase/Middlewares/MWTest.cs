using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace MusicianDatabase.Middleware
{
    public class MWTest
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<MWTest> _logger;

        public MWTest(RequestDelegate next, ILogger<MWTest> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            // Log information about the incoming request.
            _logger.LogInformation($"Received request: {context.Request.Method} {context.Request.Path} at {DateTime.UtcNow}");

            // Add a custom header to the response.
            context.Response.Headers.Add("X-Custom-Header", "Hello from MWTest!");

            // Call the next middleware in the pipeline.
            await _next(context);
        }
    }
}
