using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication4.Util
{
    public class AuthMiddleware
    {
          private readonly RequestDelegate _next;
          public AuthMiddleware(RequestDelegate next)
          {
            _next = next;
          }
          public async Task Invoke(HttpContext httpContext)
          {
            string path = httpContext.Request.Path;
            int? userId = httpContext.Session.GetInt32("usuarioId");
            if ((userId.HasValue && userId != 0) ||
                "/Usuarios/Cadastrar".Equals(path) ||
                "/".Equals(path) ||
                "/Usuarios/Login".Equals(path))
            {
                await _next(httpContext);
            }
            else
            {
                if (path.StartsWith("/api"))
                {
                    httpContext.Response.StatusCode = 403; //Forbidden
                    string resp = Newtonsoft.Json.JsonConvert.SerializeObject(
                        new {
                            status = 1,
                            message = "Necessário reautenticar",
                            url = "/Usuarios/Login"
                        });
                    await httpContext.Response.WriteAsync(resp);
                }
                else
                {
                    httpContext.Response.Redirect("/Usuarios/Login", false);
                }
            }
          }
    }
    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class MyMiddlewareExtensions
    {
        public static IApplicationBuilder UseMyMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthMiddleware>();
        }
    }
}
