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
            if ((httpContext.Session.GetInt32("usuarioId").HasValue &&
                httpContext.Session.GetInt32("usuarioId") != 0) ||
                "/Usuarios/Cadastrar".Equals(path) ||
                "/".Equals(path) ||
                "/Usuarios/Login".Equals(path))
            {
                await _next(httpContext);
            }
            else
            {
                httpContext.Response.Redirect("/Usuarios/Login", false);
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
