using LineBot_api.Models;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using LineBot_api.Services.Interfaces;
using LineBot_api.Services;
using Serilog;

namespace LineBot_api.Filters
{
    public class ReturnFormatFilter: IAsyncActionFilter
    {
        private readonly IIDCreate _idCreate;

        public ReturnFormatFilter(IIDCreate iDCreate)
        {
            _idCreate = iDCreate;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            context.HttpContext.Response.ContentType = "application/json";

            var resultContext = await next();

            if (resultContext.Result is ObjectResult objectResult)
            {
                MidReturn? tempReturn = objectResult.Value as MidReturn;

                objectResult.Value = new ReturnFormat
                {
                    traceId = _idCreate.traceId,
                    rtnCode = tempReturn?.msg ?? "",
                    msg = ReturnFormatMapSetting.ConvertToMessage(tempReturn?.msg ?? ""),
                    info = tempReturn?.info ?? ""
                };

                var json = JsonConvert.SerializeObject(objectResult.Value);

                Log.Information(json);
            }
        }
    }
}
