using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Exercise.Model;
using System.Web.Http;

namespace Exercise.Unit.Tests
{
    public class AsyncActionFilterTests
    {
        [Fact]
        public async Task Should_Throw_HttpResponseException()
        {
            var actionFilter = new Exercise.WebApi.Filters.AsyncActionFilter();

            var httpContext = new DefaultHttpContext();
            var actionContext = new ActionContext(httpContext,
                new RouteData(),
                new ActionDescriptor(),
                new ModelStateDictionary());
            var dict = new Dictionary<string, object>() { { "ModelType1", new ModelType1() { DenyUnlessLoggedIn = true } } };
            var actionExecutingContext = new ActionExecutingContext(actionContext,
                new List<IFilterMetadata>(),
                dict,
                controller: null);
            Task<ActionExecutedContext> mockDelegate()
            {
                var a = new ActionExecutedContext(actionContext, new List<IFilterMetadata>(), null);
                a.Result = new ContentResult() { StatusCode = (int)System.Net.HttpStatusCode.OK };
                return Task.FromResult(a);
            }

            await Assert.ThrowsAsync<HttpResponseException>(async () =>
            {
                await actionFilter.OnActionExecutionAsync(actionExecutingContext, mockDelegate);
            });

        }
        [Fact]
        public async Task Should_Not_Throw_HttpResponseException()
        {
            var actionFilter = new Exercise.WebApi.Filters.AsyncActionFilter();

            var httpContext = new DefaultHttpContext();
            var actionContext = new ActionContext(httpContext,
                new RouteData(),
                new ActionDescriptor(),
                new ModelStateDictionary());
            var dict = new Dictionary<string, object>() { { "ModelType1", new ModelType1() { DenyUnlessLoggedIn = false } } };
            var actionExecutingContext = new ActionExecutingContext(actionContext,
                new List<IFilterMetadata>(),
                dict,
                controller: null);
            Task<ActionExecutedContext> mockDelegate()
            {
                var a = new ActionExecutedContext(actionContext, new List<IFilterMetadata>(), null);
                a.Result = new ContentResult() { StatusCode = (int)System.Net.HttpStatusCode.OK };
                return Task.FromResult(a);
            }

            await actionFilter.OnActionExecutionAsync(actionExecutingContext, mockDelegate);
        }
    }
}