using System;
using System.Web.Http;
using CoffeeOrders.Models;

namespace CoffeeOrders.Services
{
    public interface ICustomerOrderLinkManager
    {
        void AddLinks(CustomerOrder order, ApiController controller);
        Uri LinkToSelf(CustomerOrder order, ApiController controller);
    }
}