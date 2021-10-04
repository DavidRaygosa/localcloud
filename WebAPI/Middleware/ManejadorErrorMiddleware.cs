using System.Net;
using System;
using System.Threading.Tasks;
using Application.ManejadorError;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace WebAPI.Middleware
{
    public class ManejadorErrorMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ManejadorErrorMiddleware> _logger;
        public ManejadorErrorMiddleware(RequestDelegate next, ILogger<ManejadorErrorMiddleware> logger){
            _next = next;
            _logger = logger;
        }
        public async Task Invoke(HttpContext context){
            try{
                await _next(context);
            } catch(Exception ex){
                await ManejadorExcepcionAsincrono(context, ex, _logger);
            } 
        }
        private async Task ManejadorExcepcionAsincrono(HttpContext context, Exception ex, ILogger<ManejadorErrorMiddleware> logger){
            object message = null;
            switch(ex){
                case ManejadorExcepcion me :
                    message = me.Errores;
                    context.Response.StatusCode = (int)me.Codigo;
                    break;
                case Exception e:
                    message = string.IsNullOrWhiteSpace(e.Message) ? "Error" : e.Message;
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }
            context.Response.ContentType = "application/json";
            if(message != null){
                var resultados = JsonConvert.SerializeObject(new {message});
                await context.Response.WriteAsync(resultados);
            }
        }
    }
}