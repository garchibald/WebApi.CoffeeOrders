using System.Reflection;
using System.Web.Http;
using System.Web.Http.OData.Builder;
using Autofac;
using Autofac.Integration.WebApi;
using CoffeeOrders.Filters;
using CoffeeOrders.Models;
using CoffeeOrders.Models.Data;
using CoffeeOrders.Services;
using Microsoft.Data.Edm;

namespace CoffeeOrders
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            RegisterODataRoutes(config);
            RegisterRoutes(config);

            RegisterDependancyInjection();

            // Apply filter to validate model for all WebApi controllers
            config.Filters.Add(new ValidationActionFilter());

            // Uncomment the following line of code to enable query support for actions with an IQueryable or IQueryable<T> return type.
            // To avoid processing unexpected or malicious queries, use the validation settings on QueryableAttribute to validate incoming queries.
            // For more information, visit http://go.microsoft.com/fwlink/?LinkId=279712.
            config.EnableQuerySupport();

            // To disable tracing in your application, please comment out or remove the following line of code
            // For more information, refer to: http://www.asp.net/web-api
            config.EnableSystemDiagnosticsTracing();
        }

        /// <summary>
        /// Register dependancy injection using Autofac
        /// </summary>
        private static void RegisterDependancyInjection()
        {
            // Create the container builder.
            var builder = new ContainerBuilder();

            // Register the Web API controllers.
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            // Register other dependencies.
            builder.Register(c => new EFRepository()).As<IRepository>().InstancePerApiRequest();
            builder.Register(c => new PriceEngine()).As<IPriceEngine>().InstancePerApiRequest();
            builder.Register(c => new CustomerOrderLinkManager()).As<ICustomerOrderLinkManager>().InstancePerApiRequest();
            builder.Register(c => new AsyncHttpNotificationClient()).As<INotificationClient>().InstancePerApiRequest();

            // Build the container.
            var container = builder.Build();

            // Create the depenedency resolver.
            var resolver = new AutofacWebApiDependencyResolver(container);

            // Configure Web API with the dependency resolver.
            GlobalConfiguration.Configuration.DependencyResolver = resolver;

            
        }

        public static void RegisterRoutes(HttpConfiguration config)
        {
      

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new {id = RouteParameter.Optional}
                );

            
        }

        private static void RegisterODataRoutes(HttpConfiguration config)
        {
            //Components for implementing OData services
            //   Ex Model builders, formatters (Atom/JSON/XML), path and query parsers, LINQ expression generator, etc
            //Support common patterns using an open protocol
            //   Ex. query, paging, relationships, metadata
            //Integrates with OData client ecosystem
            //   Ex. Add Service Reference, Excel, datajs
            //Built on ODataLib
            //   Same underpinnings as WCF Data Services

            var modelBuilder = new ODataConventionModelBuilder();
            modelBuilder.EntitySet<CustomerOrder>("Orders");
            modelBuilder.AddEntity(typeof(Order));
            IEdmModel model = modelBuilder.GetEdmModel();
            

            config.Routes.MapODataRoute("odata", "odata", model);
        }
    }
}
