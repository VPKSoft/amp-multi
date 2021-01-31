using System;
using System.Linq;
using System.Web.Http;
using Microsoft.Owin.Extensions;
using Microsoft.Owin.Hosting;
using Owin;

namespace amp.Remote.RESTful
{
    /// <summary>
    /// A remote REST API controller for the amp# software.
    /// Implements the <see cref="System.Web.Http.ApiController" />
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class AmpRemoteController
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AmpRemoteController"/> class.
        /// </summary>
        /// <param name="baseUrl">The base URL.</param>
        public static void CreateInstance(string baseUrl)
        {
            InstanceContext?.Dispose();

            InstanceContext = WebApp.Start<Startup>(baseUrl);
        }

        /// <summary>
        /// Gets or sets the instance context of this <see cref="AmpRemoteController"/> class.
        /// </summary>
        /// <value>The instance context of this <see cref="AmpRemoteController"/> class.</value>
        public static IDisposable InstanceContext { get; set; }

        /// <summary>
        /// A startup class for the RESTful API.
        /// Implements the <see cref="System.Web.Http.ApiController" />
        /// </summary>
        /// <seealso cref="System.Web.Http.ApiController" />
        public class Startup: ApiController
        {
            /// <summary>
            /// Configurations the specified application builder.
            /// </summary>
            /// <param name="appBuilder">The application builder.</param>
            public void Configuration(IAppBuilder appBuilder)
            {
                appBuilder.Use((context, next) =>
                {
                    context.Response.Headers.Remove("Server");
                    return next.Invoke();
                });
                appBuilder.UseStageMarker(PipelineStage.PostAcquireState);

                // Configure Web API for self-host. 
                HttpConfiguration config = new HttpConfiguration();
                //// Web API routes
                config.MapHttpAttributeRoutes();
                config.Routes.MapHttpRoute(
                    name: "DefaultApi",
                    routeTemplate: "api/{controller}/{id}",
                    defaults: new { id = RouteParameter.Optional }
                );   

                config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(
                    config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml"));

                appBuilder.UseWebApi(config);
            }
        }
    }
}
