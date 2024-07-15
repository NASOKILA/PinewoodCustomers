using AutoMapper;
using Pinewood.Customers.Models.DbModels;
using Pinewood.Customers.Models.DTOModels;

namespace Pinewood.Customers.API.Mappers
{
    public class CustomerMappingProfile : Profile
    {
        public CustomerMappingProfile()
        {
            CreateMap<Customer, CustomerDto>();
            CreateMap<CustomerDto, Customer>();
        }
    }
}
