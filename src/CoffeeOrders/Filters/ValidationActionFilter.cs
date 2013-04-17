using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace CoffeeOrders.Filters
{
    /// <summary>
    /// Applies validation filter to WebApi controllers
    /// </summary>
    public class ValidationActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            var modelState = actionContext.ModelState;

            //http://www.asp.net/web-api/overview/formats-and-model-binding/model-validation-in-aspnet-web-api
            //Overposting vs Under posting
            if (!modelState.IsValid)
                actionContext.Response = actionContext.Request
                                                      .CreateErrorResponse(HttpStatusCode.BadRequest, modelState);
        }
    }
}