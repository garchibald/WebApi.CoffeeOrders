using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using AutoMapper;
using CoffeeOrders.Filters;
using CoffeeOrders.Models;
using CoffeeOrders.Models.Data;
using CoffeeOrders.Services;

namespace CoffeeOrders.Controllers
{
    /// <summary>
    /// Provides the ability for staff to administer orders
    /// </summary>
    [Authorize]
    [StaffRoleFilter]
    public class OrdersController : ApiController
    {
        public ICustomerOrderLinkManager LinkManager { get; set; }
        private readonly IRepository _repository;

        /// <summary>
        /// Contructs a new instances of a orders controller. 
        /// </summary>
        /// <param name="repository">The repository instance to get and update orders</param>
        /// <param name="linkManager">The link manager to controler workflow</param>
        public OrdersController(IRepository repository, ICustomerOrderLinkManager linkManager)
        {
            LinkManager = linkManager;
            _repository = repository;
        }

        /// <summary>
        /// Get an existing order
        /// </summary>
        /// <remarks>The links will be from a authorised staff member point of view</remarks>
        /// <param name="id">The order to return</param>
        /// <returns></returns>
        public HttpResponseMessage Get(int id)
        {
            var order = _repository.Get<Order>(o => o.Id == id);
            if (order == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            var customerOrder = Mapper.Map<CustomerOrder>(order);
            LinkManager.AddLinks(customerOrder,this);
            
            var response = Request.CreateResponse(HttpStatusCode.OK, customerOrder);
            response.Headers.Location = LinkManager.LinkToSelf(customerOrder, this);

            return response;
        }


        /// <summary>
        /// Search for existing orders that are pending completion
        /// </summary>
        /// <param name="query">The </param>
        /// <returns></returns>
        //[Queryable] - Removed as want to map to external object
        public IQueryable<CustomerOrder> Get(ODataQueryOptions<Order> query)
        {
            query.Validate(new ODataValidationSettings());
            var results = query.ApplyTo(_repository.All<Order>().Where(o => o.State == "Pending" || o.State =="Updated")) as IQueryable<Order>;
            return ConvertToCustomerOrders(results);

            return new List<CustomerOrder>().AsQueryable();
        }

        /// <summary>
        /// Update the internal state of an order
        /// </summary>
        /// <param name="id"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public HttpResponseMessage Put(int id, [FromUri] string action)
        {
       
            HttpResponseMessage response;
            if (ValidateAction(action, out response)) return response;

            var order = _repository.Get<Order>(o => o.Id == id);
            if (order == null)
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }

            //TODO handle etag and 409 Conflict


            order.Status = action;
            UpdateInternalState(action, order);
            _repository.Update(order);

            var customerOrder = Mapper.Map<CustomerOrder>(order);
            LinkManager.AddLinks(customerOrder, this);

            return Request.CreateResponse(HttpStatusCode.OK, customerOrder);
        }

        private void UpdateInternalState(string action, Order order)
        {
            if (action.ToLower() == "completed")
            {
                order.State = "Ready For Pickup";
            }
            if (action.ToLower() == "paid")
            {
                order.State = "Complete";
            }
        }

        private bool ValidateAction(string action, out HttpResponseMessage response)
        {
            if (string.IsNullOrEmpty(action))
            {
                response = Request.CreateResponse(HttpStatusCode.BadRequest);
                return true;
            }
            switch (action.ToLower())
            {
                case "inprogress":
                case "complete":
                case "paid":
                    response = null;
                    break;
                default:
                    {
                        response = Request.CreateResponse(HttpStatusCode.BadRequest, "Invalid action");
                        return true;
                    }
            }
            return false;
        }

        private IQueryable<CustomerOrder> ConvertToCustomerOrders(IEnumerable<Order> results)
        {
            var converted = new List<CustomerOrder>();

            foreach (var result in results)
            {
                var mapped = Mapper.Map<CustomerOrder>(result);
                LinkManager.AddLinks(mapped, this);
                converted.Add(mapped);
            }

            return converted.AsQueryable();
        }
    }
}
