using System;

namespace CoffeeOrders.Models
{
    /// <summary>
    /// A reference to another hypermedia resource
    /// </summary>
    public class UrlReference
    {
        /// <summary>
        /// The resource locator for the related action
        /// </summary>
        /// <remarks>This link may change over time by the rel will stay the same</remarks>
        public Uri HRef { get; set; }

        /// <summary>
        /// The resource locator for tye of action
        /// </summary>
        /// <remarks>This should stay the same so that consumers can find expected actions</remarks>
        public string Rel { get; set; }

        /// <summary>
        /// The HTTP method that the user should use
        /// </summary>
        public string Method { get; set; } 
    }
}