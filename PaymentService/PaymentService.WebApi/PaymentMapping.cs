using AutoMapper;
using PaymentService.DataAccess.Postgres.Models;
using PaymentService.WebApi.Models;

namespace PaymentService.WebApi.Mappings;

public class PaymentMappingProfile : Profile
{
    public PaymentMappingProfile()
    {
        CreateMap<Payment, PaymentResponse>();
    }
}