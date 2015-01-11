using System.Web.Http;
using log4net;

namespace PRTracker.Web
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);

            var log = LogManager.GetLogger("PRTracker");
            log.Info("Application Start");
        }
    }
}
