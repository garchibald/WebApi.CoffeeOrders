using System;
using System.Runtime.Serialization;

namespace CoffeeOrders.Models
{
    [DataContract(Namespace = "http://coffee.orders")]
    public class NotificationResponse
    {
        [DataMember]
        public Uri Changed { get; set; } 
    }
}