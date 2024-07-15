﻿
namespace Pinewood.Customers.Models.DTOModels
{
    //A DTO ensures that the entity details (such as database annotations) do not leak into the API responses. 
    public class CustomerDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public int Age { get; set; }
    }
}
