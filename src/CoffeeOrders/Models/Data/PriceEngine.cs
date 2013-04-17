using System;
using System.IO;

namespace CoffeeOrders.Models.Data
{
    public class PriceEngine : IPriceEngine
    {
        public double Calculate(Order order)
        {
            double price = 0;
            if (order == null)
                throw new ArgumentException("Null order", "order");

            if (string.IsNullOrEmpty(order.Drink))
            {
                throw new InvalidDataException("Drink type not specified");
            }

            switch (order.Drink.ToLower())
            {
                case "latte":
                    price = 3;
                    break;
                case "expresso":
                    price = 3.5;
                    break;
                case "long black":
                    price = 4;
                    break;
            }

            if (order.Additions != null)
            {
                foreach (var addition in order.Additions)
                {
                    switch (addition.ToLower())
                    {
                        case "shot":
                            price += 0.5;
                            break;
                    }
                }
            }

            return price;
        }
    }

    public interface IPriceEngine
    {
        double Calculate(Order order);
    }
}