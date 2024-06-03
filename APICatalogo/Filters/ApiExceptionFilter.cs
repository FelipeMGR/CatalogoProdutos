using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace APICatalogo.Filters
{
    public class ApiExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<ApiExceptionFilter> _logger;
        public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger)
        {
            _logger = logger;
        }
        public void OnException(ExceptionContext context)
        {
            _logger.LogError("Ocorreu um erro na solicitação. Status Code: 500");

            context.Result = new ObjectResult("Ocorreu um erro ao tratar sua solicitação. Status Code: 500")
            {
                StatusCode = StatusCodes.Status500InternalServerError,
            };
        }
    }
}
