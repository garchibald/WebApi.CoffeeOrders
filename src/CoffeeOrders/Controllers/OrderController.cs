using System;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CoffeeOrders.Models;
using AutoMapper;
using CoffeeOrders.Models.Data;
using CoffeeOrders.Services;

namespace CoffeeOrders.Controllers
{
    /// <summary>
    /// Responsible for taking an order
    /// </summary>
    public class OrderController : ApiController
    {
        public IRepository Repository { get; set; }
        public IPriceEngine PriceEngine { get; set; }
        public ICustomerOrderLinkManager LinkManager { get; set; }

        public OrderController(IRepository repository, IPriceEngine priceEngine, ICustomerOrderLinkManager linkManager)
        {
            PriceEngine = priceEngine;
            LinkManager = linkManager;
            Repository = repository;
        }

        /// <summary>
        /// Creates a new order
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public HttpResponseMessage Post(OrderRequest request)
        {
            var order = Mapper.Map<Order>(request);
            order.Cost = PriceEngine.Calculate(order);
            order.State = "Pending";
            
            var created = Repository.Create(order);

            var newOrder = Mapper.Map<CustomerOrder>(created);
            LinkManager.AddLinks(newOrder, this);
            
            var response = Request.CreateResponse(HttpStatusCode.Created, newOrder);
            response.Headers.Location = LinkManager.LinkToSelf(newOrder, this);

            return response;
        }

        /// <summary>
        /// Gets an existing order
        /// </summary>
        /// <param name="id">The order id to get</param>
        /// <returns>The matching customer order</returns>
        public HttpResponseMessage Get(int id)
        {
            var order = Repository.Get<Order>(o => o.Id == id);
            if (order == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            var customerOrder = Mapper.Map<CustomerOrder>(order);
            LinkManager.AddLinks(customerOrder, this);

            var response = Request.CreateResponse(HttpStatusCode.OK, customerOrder);
            response.Headers.Location = LinkManager.LinkToSelf(customerOrder, this);

            return response;
        }

        /// <summary>
        /// Attempts to update an existing order
        /// </summary>
        /// <param name="id">The order to be updated</param>
        /// <param name="change">The change to apply</param>
        /// <returns>The updated order</returns>
        public HttpResponseMessage Put(int id, ChangeOrderRequest change)
        {
            var order = Repository.Get<Order>(o => o.Id == id);
            if (order == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            //TODO handle etag and 409 Conflict

            order.State = "Updated";

            Mapper.Map(change, order);
            order.Cost = PriceEngine.Calculate(order);

            Repository.Update(order);

            var customerOrder = Mapper.Map<CustomerOrder>(order);
            LinkManager.AddLinks(customerOrder,this);

            var response = Request.CreateResponse(HttpStatusCode.OK, customerOrder);

            response.Headers.Location = LinkManager.LinkToSelf(customerOrder, this);

            return response;
        }

        
    }
}
