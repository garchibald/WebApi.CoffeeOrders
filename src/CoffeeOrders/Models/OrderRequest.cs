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
    }
}