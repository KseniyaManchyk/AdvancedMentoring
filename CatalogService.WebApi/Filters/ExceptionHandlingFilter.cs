using CatalogService.Domain.ExceptionHandling;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CartingService.WebApi.Filters
{
    public class ExceptionHandlingFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            context.Result = context.Exception is NotFoundException ?
                new NotFoundObjectResult(context.Exception.Message) :
                new BadRequestObjectResult($"Exception has been thrown during request processing. {context.Exception.Message}");
        }
    }
}
