using System;
using System.ComponentModel.DataAnnotations;

namespace CoffeeOrders.Models
{
    /// <summary>
    /// An internal representation of the order
    /// </summary>
    public class Order
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// The requested drink type
        /// </summary>
        public string Drink { get; set; }

        /// <summary>
        /// Additional requests for the drink
        /// </summary>
        public string[] Additions { get; set; }

        /// <summary>
        /// The cost of the curent drink and additions
        /// </summary>
        public double Cost { get; set; }

        /// <summary>
        /// The external state of the order from the customer point of view
        /// </summary>
        public string State { get; set; }

        /// <summary>
        /// The internal workflow status
        /// </summary>
        public string Status { get; set; }

        ///<summary>
        ///Notification Url
        ///</summary>
        public string NotificationUrl { get; set; }

        ///<summary>
        ///The optional order reference
        ///</summary>
        public string OrderReference { get; set; }
    }
}