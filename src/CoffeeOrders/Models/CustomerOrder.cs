using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace CoffeeOrders.Models
{
    /// <summary>
    /// Public facing view of order seen by consumers of the service
    /// </summary>
    [DataContract(Namespace = "http://coffee.orders")]
    public class CustomerOrder
    {
        public CustomerOrder()
        {
            Links = new List<UrlReference>();
        }

        [DataMember]
        public int Id { get; set; }

        [DataMember]
        public string Drink { get; set; }

        [DataMember]
        public string[] Additions { get; set; }

        [DataMember]
        public double Cost { get; set; }

        /// <summary>
        /// The external state of the order from the customer point of view
        /// </summary>
        [DataMember]
        public string State { get; set; }

        // <summary>
        /// The internal workflow status
        /// </summary>
        [DataMember]
        public string Status { get; set; }

        [DataMember]
        public IList<UrlReference> Links { get; private set; }

        [DataMember]
        public Uri NotificationUrl { get; set; }

        [DataMember]
        public string OrderReference { get; set; }
    }
}