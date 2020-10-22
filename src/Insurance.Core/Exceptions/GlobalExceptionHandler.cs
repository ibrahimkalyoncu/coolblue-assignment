using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;

namespace Insurance.Core.Exceptions
{
    public static class GlobalExceptionHandler
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(builder =>
            {
                builder.Run(async context =>
                {
                    const int defaultStatusCode = (int)HttpStatusCode.InternalServerError;
                    
                    context.Response.StatusCode = defaultStatusCode;
                    context.Response.ContentType = "application/json";
                    
                    var exceptionHandlerFeature = context.Features.Get<IExceptionHandlerFeature>();
                    if(exceptionHandlerFeature != null)
                    {
                        var publicErrorDetails = new ErrorDetails
                        {
                            StatusCode = defaultStatusCode,
                            Message = "Internal Server Error."
                        };
                        
                        var exception = exceptionHandlerFeature.Error;

                        if (exception is KnownException knownException )
                        {
                            if (knownException.StatusCode > 0)
                                context.Response.StatusCode = knownException.StatusCode;

                            publicErrorDetails = new ErrorDetails
                            {
                                Message = knownException.Message,
                                StatusCode = knownException.StatusCode
                            };
                        }
                        
                        await context.Response.WriteAsync(JsonConvert.SerializeObject(publicErrorDetails));
                    }
                });
            });
        }
    }
}