using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PrintShop.API.Filters
{
    public class GlobalExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            if (context.Exception is ArgumentOutOfRangeException or ArgumentException)
            {
                // Превращаем любую ошибку валидации домена в 400 Bad Request
                context.Result = new BadRequestObjectResult(new
                {
                    Error = context.Exception.Message
                });
                context.ExceptionHandled = true;
            }
            else
            {
                // Для остальных ошибок (база упала и т.д.) - 500
                context.Result = new StatusCodeResult(StatusCodes.Status500InternalServerError);
                context.ExceptionHandled = true;
            }
        }
    }
}
