using Exercise.Model;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using System.Web.Http;

namespace Exercise.WebApi.Filters
{
    public class AsyncActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            Console.WriteLine("AsyncActionFilter.OnActionExecutionAsync");
            foreach (var aa in context.ActionArguments)
            {
                Console.WriteLine($"{aa.Key} = {aa.Value}");
                if (aa.Value.GetType() == typeof(ModelType1)) 
                {
                    var mt1 = aa.Value as ModelType1;
                    if (mt1 != null 
                        && mt1.DenyUnlessLoggedIn 
                        && !HasValidJwt(context.HttpContext)) 
                    {
                        throw new HttpResponseException(new HttpResponseMessage(HttpStatusCode.Unauthorized));
                    }
                    break;
                }
            }

            await next();
        }

        public bool HasValidJwt(HttpContext httpContext)
        {
            // Note: Should validate JWT here

            var authHeader = httpContext?.Request?.Headers["Authorization"];
            if (authHeader.Value.ToString().StartsWith("Bearer"))
                return true;

            return false;
        }
    }
}
