using AutoMapper;
using PaymentService.DataAccess.Postgres.Models;
using PaymentService.WebApi.Models;

namespace PaymentService.WebApi.Mappings;

/// <summary>
/// Профиль AutoMapper для отображения Payment -> PaymentResponse.
/// </summary>
public class PaymentMappingProfile : Profile
{
    public PaymentMappingProfile()
    {
        CreateMap<Payment, PaymentResponse>();
    }
}