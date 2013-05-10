using System;
using System.ComponentModel.DataAnnotations;

namespace CoffeeOrders.Models
{
    /// <summary>
    /// A request from the customer for a new order
    /// </summary>
    /// <remarks>Limited number of fields to request on</remarks>
    public class OrderRequest
    {
        [Required]
        public string Drink { get; set; }

        /// <summary>
        /// Any additions to the drink
        /// </summary>
        public string[] Additions { get; set; }

        /// <summary>
        /// Optional reference to assign to the order
        /// </summary>
        public string OrderReference { get; set; }

        /// <summary>
        /// A location that the caller wants callback to when status of order changed
        /// </summary>
        /// <remarks>Optional</remarks>
        public Uri NotificationUrl { get; set; }
    }
}