using System;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using SendGrid.Helpers.Errors.Model;

namespace PopUp_Now_API.Exceptions
{
    /**
     * Error handler class
     * If an error is being thrown before the response is created, will be handled by this middleware
     */
    public class ErrorHandler
    {
        private readonly RequestDelegate _requestDelegate;

        public ErrorHandler(RequestDelegate requestDelegate)
        {
            _requestDelegate = requestDelegate;
        }

        /**
         * Method called after a HTTP request is sent.
         * Handles exceptions that could be thrown while processing the request
         */
        public async Task Invoke(HttpContext context)
        {
            try
            {
                //try to process the request
                await _requestDelegate(context);
            }
            //if an exception occurs, format a proper response with an HTTP Status Code and a message
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
