using System;
using CoffeeOrders.Models;

namespace CoffeeOrders
{
    public class MappingConfig
    {
        public void RegisterMapings()
        {
            AutoMapper.Mapper.CreateMap<OrderRequest, Order>();
            AutoMapper.Mapper.CreateMap<ChangeOrderRequest, Order>()
                      .ForMember(dst => dst.Drink, opt =>
                                                       {
                                                           opt.Condition(req => !string.IsNullOrEmpty(req.Drink));
                                                           opt.MapFrom(req => req.Drink);
                                                       });

            AutoMapper.Mapper.CreateMap<Order, CustomerOrder>()
                .ForMember(dst => dst.NotificationUrl, opt => opt.MapFrom(data => string.IsNullOrEmpty(data.NotificationUrl) ? null : new Uri(data.NotificationUrl)));

            AutoMapper.Mapper.CreateMap<CustomerOrder, Order>()
                .ForMember(dst => dst.NotificationUrl, opt => opt.MapFrom(data => data.NotificationUrl == null ? null : data.ToString()));
        }
    }
}