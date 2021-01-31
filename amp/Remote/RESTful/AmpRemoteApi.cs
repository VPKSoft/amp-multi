using System;
using System.Web.Http;

namespace amp.Remote.RESTful
{
    /// <summary>
    /// Initializes the RESTful API controllers.
    /// </summary>
    public static class RestInitializer
    {
        /// <summary>
        /// Initializes the RESTful API with a specified base URL, port and an instance to a <paramref name="remoteProvider"/>.
        /// </summary>
        /// <param name="baseUrl">The base URL.</param>
        /// <param name="port">The port. If set to zero the port is assumed to be in the <paramref name="baseUrl"/></param>
        /// <param name="remoteProvider">The remote data provider.</param>
        public static void InitializeRest(string baseUrl, int port, RemoteProvider remoteProvider)
        {
            baseUrl = port > 0 ? baseUrl.TrimEnd('/') + ":" + port + '/' : baseUrl;

            RemoteProvider = remoteProvider;

            AmpRemoteController.CreateInstance(baseUrl);
        }

        /// <summary>
        /// Gets or sets the remote control provider.
        /// </summary>
        /// <value>The remote control provider.</value>
        public static RemoteProvider RemoteProvider { get; set; }
    }

    /// <summary>
    /// A remote REST API for the amp# software.
    /// </summary>
    [RoutePrefix("api/values")]
    public class AlbumController: ApiController
    {
/*        [Route("test")]
        [HttpGet]
        public string GetCurrentAlbum()
        {
            return RestInitializer.RemoteProvider.CurrentAlbum;
        }*/

//
        public string Get()
        {
            return RestInitializer.RemoteProvider.CurrentAlbum;
        }

        public string Get(int id)
        {
            return Get();
        }

        // POST api/values 
        public void Post([FromBody]string value)
        {
            Console.WriteLine("Post method called with value = " + value);
        }

        // PUT api/values/5 
        public void Put(int id, [FromBody]string value)
        {
            Console.WriteLine("Put method called with value = " + value);
        }

        // DELETE api/values/5 
        public void Delete(int id)
        {
            Console.WriteLine("Delete method called with id = " + id);
        }
    }
}
