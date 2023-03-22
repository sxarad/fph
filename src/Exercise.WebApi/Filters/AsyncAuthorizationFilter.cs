using Microsoft.AspNetCore.Mvc.Filters;
using System.Text;

namespace Exercise.WebApi.Filters
{
    public class AsyncAuthorizationFilter : IAsyncAuthorizationFilter
    {
        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            Console.WriteLine("AsyncAuthorizationFilter.OnAuthorizationAsync");

            // Note: Some research to see if posted data can be accessed from filters other than Action filters

            if (context.HttpContext.Request.ContentType != null &&
                context.HttpContext.Request.ContentType.StartsWith("application/json", StringComparison.InvariantCultureIgnoreCase))
            {
                if (!context.HttpContext.Request.Body.CanSeek)
                {
                    context.HttpContext.Request.EnableBuffering();
                }
                context.HttpContext.Request.Body.Position = 0;

                using var reader = new StreamReader(
                        context.HttpContext.Request.Body,
                        encoding: Encoding.UTF8,
                        detectEncodingFromByteOrderMarks: false,
                        leaveOpen: true);

                var body = (await reader.ReadToEndAsync()) ?? "";
                await Console.Out.WriteLineAsync($"Body: {body}");

                context.HttpContext.Request.Body.Position = 0;
            }
        }
    }
}
