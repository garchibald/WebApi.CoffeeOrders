using System;
using System.Net.Http;

namespace CoffeeOrders.Services
{
    internal class AsyncHttpNotificationClient : INotificationClient
    {
        public async void NotifyChange<T>(Uri callback, T response)
        {
            var client = new HttpClient();
            //TODO Support other types
            client.PostAsXmlAsync(callback.ToString(), response);
        }
    }
}