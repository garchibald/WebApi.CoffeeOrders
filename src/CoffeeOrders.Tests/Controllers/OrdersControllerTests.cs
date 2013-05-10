using System;
using System.Linq.Expressions;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Hosting;
using System.Web.Http.Routing;
using CoffeeOrders.Controllers;
using CoffeeOrders.Models;
using CoffeeOrders.Models.Data;
using CoffeeOrders.Services;
using Moq;
using Should;
using Xunit;

namespace CoffeeOrders.Tests.Controllers
{
    public class OrdersControllerTests
    {
        [Fact]
        public void CanPostNewOrder()
        {
            // Arrange
            new MappingConfig().RegisterMapings();

            var repository = new Mock<IRepository>();
            var price = new Mock<IPriceEngine>();
            var linkManager = new Mock<ICustomerOrderLinkManager>();

            var controller = new OrderController(repository.Object, price.Object, linkManager.Object);
            var orderRequest = new OrderRequest
                                   {
                                       Drink = "latte"
                                   };

            InitController(controller, HttpMethod.Post, "order");

            var created = new Order {Id = 1, Drink = "latte", Cost = 1};

            repository.Setup(r => r.Create(It.IsAny<Order>())).Returns(created);
            price.Setup(p => p.Calculate(It.IsAny<Order>())).Returns(1);
            linkManager.Setup(l => l.LinkToSelf(It.IsAny<CustomerOrder>(), controller))
                       .Returns(new Uri("http://localhost/api/order/1"));

            // Act
            HttpResponseMessage response = controller.Post(orderRequest);

            // Assert
            response.StatusCode.ShouldEqual(HttpStatusCode.Created);

            response.Headers.Location.ShouldEqual(new Uri("http://localhost/api/order/1"));
            var responseContent = response.Content as ObjectContent<CustomerOrder>;

            var responseOrder = responseContent.Value as CustomerOrder;
            responseOrder.Drink.ShouldEqual("latte");
            responseOrder.Cost.ShouldEqual(1);
        }

        [Fact]
        public void CanPostNewOrderWithNotificationUrl()
        {
            // Arrange
            new MappingConfig().RegisterMapings();

            var repository = new Mock<IRepository>();
            var price = new Mock<IPriceEngine>();
            var linkManager = new Mock<ICustomerOrderLinkManager>();

            var controller = new OrderController(repository.Object, price.Object, linkManager.Object);
            var orderRequest = new OrderRequest
                                   {
                                       Drink = "latte",
                                       NotificationUrl = new Uri("http://localhost")
                                   };

            InitController(controller, HttpMethod.Post, "order");

            var created = new Order {Id = 1, Drink = "latte", NotificationUrl = orderRequest.NotificationUrl.ToString()};

            repository.Setup(r => r.Create(It.IsAny<Order>())).Returns(created);
            price.Setup(p => p.Calculate(It.IsAny<Order>())).Returns(1);
            linkManager.Setup(l => l.LinkToSelf(It.IsAny<CustomerOrder>(), controller))
                       .Returns(new Uri("http://localhost/api/order/1"));

            // Act
            HttpResponseMessage response = controller.Post(orderRequest);

            // Assert
            response.StatusCode.ShouldEqual(HttpStatusCode.Created);

            var responseContent = response.Content as ObjectContent<CustomerOrder>;

            var responseOrder = responseContent.Value as CustomerOrder;
            responseOrder.NotificationUrl.ShouldEqual(orderRequest.NotificationUrl);
        }

        [Fact]
        public void CanUpdateExisting()
        {
            // Arrange
            new MappingConfig().RegisterMapings();

            var repository = new Mock<IRepository>();
            var price = new Mock<IPriceEngine>();
            var linkManager = new Mock<ICustomerOrderLinkManager>();

            var controller = new OrderController(repository.Object, price.Object, linkManager.Object);
            var orderRequest = new ChangeOrderRequest
                                   {
                                       Additions = new[] {"shot"}
                                   };

            InitController(controller, HttpMethod.Put, "order");

            var existing = new Order {Id = 1, Drink = "latte", Cost = 1};

            repository.Setup(r => r.Get(It.IsAny<Expression<Func<Order, bool>>>(), null)).Returns(existing);
            price.Setup(p => p.Calculate(It.IsAny<Order>())).Returns(1.5);
            linkManager.Setup(l => l.LinkToSelf(It.IsAny<CustomerOrder>(), controller))
                       .Returns(new Uri("http://localhost/api/order/1"));

            // Act
            HttpResponseMessage response = controller.Put(1, orderRequest);

            // Assert
            response.StatusCode.ShouldEqual(HttpStatusCode.OK);

            response.Headers.Location.ShouldEqual(new Uri("http://localhost/api/order/1"));
            var responseContent = response.Content as ObjectContent<CustomerOrder>;

            var responseOrder = responseContent.Value as CustomerOrder;
            responseOrder.Drink.ShouldEqual("latte");
            responseOrder.Additions.ShouldEqual(new[] {"shot"});
            responseOrder.Cost.ShouldEqual(1.5);
        }

        private void InitController(ApiController controller, HttpMethod method, string url, string id = null)
        {
            var request = new HttpRequestMessage(method, string.Format("http://localhost/api/{0}/{1}", url, id));
            var config = new HttpConfiguration();
            IHttpRoute route = config.Routes.MapHttpRoute("DefaultApi", "api/{controller}/{id}",
                                                          new {id = RouteParameter.Optional});
            var routeData = new HttpRouteData(route, new HttpRouteValueDictionary {{"controller", url}});
            config.Routes.MapHttpRoute("TypeApi", "api/{controller}/{type}/{id}", new {id = RouteParameter.Optional});

            controller.ControllerContext = new HttpControllerContext(config, routeData, request)
                                               {
                                                   ControllerDescriptor = new HttpControllerDescriptor(config, "order",
                                                                                                       typeof (
                                                                                                           OrderController
                                                                                                           ))
                                               };
            controller.Request = request;
            controller.Request.Properties[HttpPropertyKeys.HttpConfigurationKey] = config;
            controller.Request.Properties.Add(HttpPropertyKeys.HttpRouteDataKey, routeData);
        }
    }
}