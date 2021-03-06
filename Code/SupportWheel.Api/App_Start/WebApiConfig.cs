﻿using SupportWheel.Api.Filters;
using SupportWheel.Api.Providers;
using Microsoft.Owin.Security.OAuth;
using Newtonsoft.Json.Serialization;
using System.Linq;
using System.Net.Http.Formatting;
using System.Web.Http;
using System.Web.Http.ExceptionHandling;

namespace SupportWheel.Api
{
    public class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            //Configure WebApi to accept only a Bearer Token
            config.SuppressDefaultHostAuthentication();
            config.Filters.Add(new HostAuthenticationFilter(OAuthDefaults.AuthenticationType));
            config.Filters.Add(new ValidateModelAttribute()); //Validate that models are valid and not null
            config.MessageHandlers.Add(new SignalRHandler()); //Register Webapi Handler for SignalR

            //Global Exception Handler
            config.Services.Replace(typeof(IExceptionHandler), new GeneralExceptionHandler());

            // Web API routes
            config.MapHttpAttributeRoutes();
            
            //Default Route
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            var jsonFormatter = config.Formatters.OfType<JsonMediaTypeFormatter>().First();
            config.Formatters.XmlFormatter.SupportedMediaTypes.Clear(); //Remove o XML, deixando apenas JSON
            jsonFormatter.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}