using ChatApp.Common.Dtos.Interfaces;
using ChatApp.Common.Responses.Base;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace ChatApp.Server.ActionFilters;

public class ValidateDtoActionFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
        if (context.ActionArguments.Any())
        {
            foreach (var argument in context.ActionArguments)
            {
                if (argument.Value is IBaseDto dto && !dto.ValidateDto())
                {
                    ApiErrorResponse responseDto = new ApiErrorResponse()
                    {
                        Success = false, 
                        ErrorKey = dto.ValidationResultKey, 
                        Message = dto.ValidationResultMessage, 
                        Code = StatusCodes.Status400BadRequest
                    };
                    
                    context.Result = new JsonResult(responseDto, new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    })
                    {
                        StatusCode = StatusCodes.Status400BadRequest,
                        ContentType = "application/json"
                    };

                    return;
                }
            }
        }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        
    }
}