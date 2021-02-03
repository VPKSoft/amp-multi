#region License
/*
MIT License

Copyright(c) 2021 Petteri Kautonen

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/
#endregion

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
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword, NO IT DOES NOT..
            public void Configuration(IAppBuilder appBuilder)
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
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
