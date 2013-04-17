using System.Data.Entity;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using CoffeeOrders.Models.Data;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace CoffeeOrders
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AuthConfig.RegisterAuth();

            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //NOTE: Maping
            new MappingConfig().RegisterMapings();

            Database.SetInitializer(new EFContextInitializer());
            var context = new EFContext();
            //NOTE:To delete database Use SQl Enterprise Manager to DROP database before remove from filesystem
            // http://odetocode.com/Blogs/scott/archive/2012/08/14/a-troubleshooting-guide-for-entity-framework-connections-amp-migrations.aspx
            context.Database.Initialize(true);

            //NOTE: Formatting
            // http://odetocode.com/blogs/scott/archive/2013/03/25/asp-net-webapi-tip-3-camelcasing-json.aspx
            var formatters = GlobalConfiguration.Configuration.Formatters;
            var jsonFormatter = formatters.JsonFormatter;
            var settings = jsonFormatter.SerializerSettings;
            settings.Formatting = Formatting.Indented;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();
        }
    }
}