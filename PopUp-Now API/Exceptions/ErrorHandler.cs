using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SendGrid.Helpers.Errors.Model;

namespace PopUp_Now_API.Exceptions
{
    public class ErrorHandler
    {
        private readonly RequestDelegate _requestDelegate;

        public ErrorHandler(RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _requestDelegate(context);
            }
            catch (Exception exception)
            {
                var response = context.Response;
                response.ContentType = "application/json";

                switch (exception)
                {
                    case PopUpNowException:
                        response.StatusCode = (int) HttpStatusCode.BadRequest;
                        break;
                    case NotFoundException:
                        response.StatusCode = (int) HttpStatusCode.NotFound;
                        break;
                    default:
                        response.StatusCode = (int) HttpStatusCode.InternalServerError;
                        break;
                }

                var result = JsonSerializer.Serialize(new {message = exception?.Message});
                await response.WriteAsync(result);
            } 
        }
    }
}
