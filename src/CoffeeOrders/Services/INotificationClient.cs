using System;

namespace CoffeeOrders.Services
{
    /// <summary>
    /// Defines required functionality for notification client
    /// </summary>
    public interface INotificationClient
    {
        /// <summary>
        /// Notifies the callback url that the resource has changed
        /// </summary>
        /// <param name="callback">The url to send change to</param>
        /// <param name="response">The response</param>
        void NotifyChange<T>(Uri callback, T response);
    }
}