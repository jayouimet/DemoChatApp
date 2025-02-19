using System.Net;
using Newtonsoft.Json;
using ChatApp.Common;
using ChatApp.Common.Errors;
using ChatApp.Common.Errors.Abstracts;
using ChatApp.Common.Responses.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json.Serialization;

namespace ChatApp.Server.ActionFilters;

public class ResultActionFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
    }
    
    public void OnActionExecuted(ActionExecutedContext context)
    {
        if (context.Exception != null)
        {
            HandleException(context, context.Exception);
            return;
        }

        if (context.Result is ObjectResult objectResult)
        {
            ApiSuccessResponse<object?> apiSuccessResponse = new ApiSuccessResponse<object?>()
            {
                Success = true,
                Data = objectResult.Value,
                Code = context.HttpContext.Response.StatusCode,
            };

            context.Result = new JsonResult(apiSuccessResponse, new JsonSerializerSettings()
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            })
            {
                StatusCode = context.HttpContext.Response.StatusCode,
                ContentType = "application/json"
            };
        }
    }

    public void HandleException(ActionExecutedContext context, Exception exception)
    {
        string key = ErrorKeys.UnknownError;
        string message = exception.Message;
        int code = (int)HttpStatusCode.InternalServerError;

        if (exception is UnauthorizedAccessException)
        {
            code = (int)HttpStatusCode.Unauthorized;
            key = ErrorKeys.UnauthorizedError;
        }
        else if (exception is NotImplementedException)
        {
            code = (int)HttpStatusCode.NotImplemented;
            key = ErrorKeys.NotImplementedError;
        }
        else if (exception is BaseHttpError baseHttpError)
        {
            code = baseHttpError.StatusCode;
            key = baseHttpError.Key;
            message = baseHttpError.Message;
        }

        context.Exception = null;
        context.ExceptionHandled = true;

        ApiErrorResponse responseDto = new ApiErrorResponse()
        {
            Success = false, 
            ErrorKey = key, 
            Message = message, 
            Code = code
        };
        
        context.Result = new JsonResult(responseDto, new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        })
        {
            StatusCode = code,
            ContentType = "application/json"
        };
    }
}