namespace CoffeeOrders.Models
{
    /// <summary>
    /// A request to change an order from thr customer
    /// </summary>
    /// <remarks>Conflict handling will detect if change can be made</remarks>
    public class ChangeOrderRequest
    {
        public string Drink { get; set; }
        public string[] Additions { get; set; }  
    }
}