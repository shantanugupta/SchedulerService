using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace SchedulerApi.ApiFilters
{
    internal class RequireJsonContentTypeAttribute : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            // Check the Content-Type header of the request
            if (!context.HttpContext.Request.ContentType.Contains("application/json"))
            {
                context.Result = new UnsupportedMediaTypeResult();
                return;
            }

            await next();
        }
    }
}
