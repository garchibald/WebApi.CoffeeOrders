using System;
using System.Web.Http;
using CoffeeOrders.Controllers;
using CoffeeOrders.Models;

namespace CoffeeOrders.Services
{
    public class CustomerOrderLinkManager : ICustomerOrderLinkManager
    {
        public void AddLinks(CustomerOrder order, ApiController controller)
        {
            if (controller is OrderController)
            {
                AddCustomerLinks(order, controller);
            }

            if (controller is OrdersController)
            {
                AddStaffLinks(order, controller);
            }
        }

        public Uri LinkToSelf(CustomerOrder order, ApiController controller)
        {
   
            // http://odetocode.com/blogs/scott/archive/2013/03/27/webapi-tip-5-generating-links.aspx
            var orderUri = controller.Url.Link("DefaultApi", new { id = order.Id });

            if (orderUri != null)
                return new Uri(orderUri);

            throw new NullReferenceException(string.Format("Unable to generate url for order {0}", order.Id));
        }

        private void AddStaffLinks(CustomerOrder order, ApiController controller)
        {
            var status = string.IsNullOrEmpty(order.Status) ? string.Empty : order.Status;
            switch (status.ToLower())
            {
                case "":
                    AddOrdersActionLink(order, controller, "InProgress", "start");
                    break;
                case "inprogress":
                    AddOrdersActionLink(order, controller, "Complete", "end");
                    break;
                case "complete":
                    AddOrdersActionLink(order, controller, "Paid", "paid");
                    break;
            }
        }

        private void AddCustomerLinks(CustomerOrder order, ApiController controller)
        {
            var state = string.IsNullOrEmpty(order.State) ? string.Empty : order.State;

            switch (state.ToLower())
            {
                case "":
                    // TODO: Add new link to payments controller to make online payment, SignalR callback
                    
                    DeleteOrder(order, controller);
                    break;
                case "ready for pickup":
                    // TODO: Add new link to add related order for upgrades e.g. Cookie
                    break;
            } 
        }

        public Uri GetOrder(CustomerOrder order, ApiController controller)
        {
            // http://odetocode.com/blogs/scott/archive/2013/03/27/webapi-tip-5-generating-links.aspx
            var link = controller.Url.Link("DefaultApi",
                                             new
                                             {
                                                 controller = "order",
                                                 id = order.Id
                                             });

            return link != null ? new Uri(link) : null;
        }

        private void DeleteOrder(CustomerOrder order, ApiController controller)
        {
            // http://odetocode.com/blogs/scott/archive/2013/03/27/webapi-tip-5-generating-links.aspx
            var orders = controller.Url.Link("DefaultApi",
                                             new
                                                 {
                                                     controller = "order",
                                                     id = order.Id
                                                 });

            if (orders != null)
                order.Links.Add(new UrlReference
                                    {
                                        HRef = new Uri(orders),
                                        Rel = "delete",
                                        Method = "DELETE"
                                    });
        }

        private void AddOrdersActionLink(CustomerOrder order, ApiController controller, string action, string rel)
        {
            // http://odetocode.com/blogs/scott/archive/2013/03/27/webapi-tip-5-generating-links.aspx
            var orders = controller.Url.Link("DefaultApi",
                                             new
                                                 {
                                                     controller = "orders",
                                                     id = order.Id
                                                 });

            if (orders != null)
                order.Links.Add(new UrlReference
                                    {
                                        HRef = new Uri(orders + "?action=" + Uri.EscapeDataString(action)),
                                        Rel = rel,
                                        Method = "PUT"
                                    });
        }
    }
}