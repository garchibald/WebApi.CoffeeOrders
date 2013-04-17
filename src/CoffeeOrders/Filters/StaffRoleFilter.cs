using System.Net;
using System.Net.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;
using System.Threading;


namespace CoffeeOrders.Filters
{
    /// <summary>
    /// Applies staff role check
    /// </summary>
    public class StaffRoleFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(HttpActionContext actionContext)
        {
            if (!Thread.CurrentPrincipal.IsInRole("Staff"))
                actionContext.Response = actionContext.Request
                                                      .CreateErrorResponse(HttpStatusCode.Forbidden,"");
        }
    }
}