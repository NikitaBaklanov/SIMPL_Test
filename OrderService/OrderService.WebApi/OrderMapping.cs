using AutoMapper;
using OrderService.DataAccess.Postgres.Models;
using OrderService.WebApi.Models;

namespace OrderService.WebApi.Mappings;

public class OrderProfile : Profile
{
    public OrderProfile()
    {
        //Преобразование клиентских данных о заказе в класс для БД 
        CreateMap<CreateOrderRequest, Order>()
            .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.UtcNow))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => "Pending"));
        //Преобразование данных из БД для клиента
        CreateMap<Order, OrderResponse>();
    }
}