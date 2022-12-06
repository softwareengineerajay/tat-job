using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NOV.ES.TAT.Common.Exception;

namespace NOV.ES.TAT.Job.API.Filters
{
    public class HttpGlobalExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<HttpGlobalExceptionFilter> logger;
        private readonly IConfiguration configuration;

        public HttpGlobalExceptionFilter(ILogger<HttpGlobalExceptionFilter> logger,
            IConfiguration configuration)
        {
            this.logger = logger;
            this.configuration = configuration;
        }

        public void OnException(ExceptionContext context)
        {
            logger.LogError(new EventId(context.Exception.HResult),
                context.Exception,
                context.Exception.Message);
            var errorModel = ExceptionExtensions.GetCustomErrorModel(context
                 , context.HttpContext.Request.Headers.RequestId
                 , "TraceId-"
                 , Convert.ToInt32(configuration["Lookup_Error_Code"])
                 , configuration["BaseHelpUrl"]);

            context.Result = new ContentResult
            {
                Content = Newtonsoft.Json.JsonConvert.SerializeObject(errorModel),
                ContentType = "text/json",
                StatusCode = errorModel.Code
            };
            context.ExceptionHandled = true;
        }
    }
}
