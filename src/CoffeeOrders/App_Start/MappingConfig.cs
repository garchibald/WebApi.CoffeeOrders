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

            AutoMapper.Mapper.CreateMap<Order, CustomerOrder>();
        }
    }
}