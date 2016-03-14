using System;
using System.IO;
using System.Web.Hosting;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;
using Nancy;

namespace Proto.Service
{
    public class Bootstrapper : DefaultNancyBootstrapper
    {
        protected override void ApplicationStartup(TinyIoCContainer container, IPipelines pipelines)
        {
            pipelines.OnError += (errorContext, ex) => HandleError(container, ex);
        }

        private static Response HandleError(TinyIoCContainer container, Exception exception)
        {
            var errorMessage = string.Format("Internal error: {0}", exception.Message);
            var responseContent = string.Concat(errorMessage, Environment.NewLine, exception.StackTrace);

            Response response = responseContent;
            response.StatusCode = HttpStatusCode.InternalServerError;
            return response;
        }

    }
}