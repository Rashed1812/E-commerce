using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using ServiceAbstraction;

namespace Presentation.Attributes
{
    internal class CacheAttribute(int DurationInSeconde = 90) : ActionFilterAttribute
    {
        public override async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            string CasheKey = CreateCasheKey(context.HttpContext.Request);

            ICasheService casheService = context.HttpContext.RequestServices.GetRequiredService<ICasheService>();
            var CasheValue = await casheService.GetAsync(CasheKey);

            if (CasheValue is not null) 
            {
                context.Result = new ContentResult()
                {
                    Content = CasheValue,
                    ContentType = "application/json",
                    StatusCode = StatusCodes.Status200OK
                };
                return;
            }
           var ExecuteContext = await next.Invoke();
            if (ExecuteContext.Result is OkObjectResult result)
            {
                await casheService.SetAsync(CasheKey, result.Value, TimeSpan.FromSeconds(DurationInSeconde));
            }
        }

        private string CreateCasheKey(HttpRequest request)
        {
            StringBuilder Key = new StringBuilder();
            Key.Append(request.Path + '?');
            foreach (var Item in request.Query.OrderBy(Q=>Q.Key))
            {
                Key.Append($"{Item.Key}={Item.Value}&");
            }
            return Key.ToString();
        }
    }
}
