using System;
using System.Data;
using System.Net.Mime;
using System.Threading.Tasks;
using Contacts.API.Models.Result;
using Contacts.BL.DTOs.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Contacts.API.Middlewares
{
    public class ErrorHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ErrorHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, ILogger<ErrorHandlingMiddleware> logger)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, logger);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception ex, ILogger logger)
        {
            logger.LogError(ex, "Global exception handler caught an error");

            var errorCode = ex is DBConcurrencyException || ex is DbUpdateConcurrencyException
                ? ErrorCodeDto.GeneralConcurrencyConflict
                : ErrorCodeDto.GeneralInternalError;

            var errorResult = new ErrorBaseModel<ErrorCodeDto>(errorCode);

            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = MediaTypeNames.Application.Json;

            return context.Response.WriteAsync(JsonConvert.SerializeObject(
                errorResult,
                new Newtonsoft.Json.Converters.StringEnumConverter())
            );
        }
    }

}
