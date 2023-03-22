using Microsoft.AspNetCore.Mvc.Filters;

namespace Exercise.WebApi.Filters
{
    public class AsyncResourceFilter : IAsyncResourceFilter
    {
        public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
        {
            Console.WriteLine("AsyncResourceFilter.OnActionExecutionAsync");

            // Note: Unused

            await next();
        }
    }
}
