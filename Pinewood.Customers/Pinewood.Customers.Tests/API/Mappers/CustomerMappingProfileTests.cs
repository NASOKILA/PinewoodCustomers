using NUnit.Framework;
using AutoMapper;
using Pinewood.Customers.Models.DbModels;
using Pinewood.Customers.Models.DTOModels;
using Pinewood.Customers.API.Mappers;

namespace Pinewood.Customers.Tests.API.Mappers
{
    [TestFixture]
    public class CustomerMappingProfileTests
    {
        private IMapper _mapper;

        [SetUp]
        public void Setup()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new CustomerMappingProfile());
            });
            _mapper = mappingConfig.CreateMapper();
        }

        [Test]
        public void CustomerToCustomerDto_Mapping_IsValid()
        {
            // Arrange
            var customer = new Customer
            {
                Id = 1,
                Name = "Test Customer",
                Email = "test@example.com",
                Age = 30
            };

            // Act
            var customerDto = _mapper.Map<CustomerDto>(customer);

            // Assert
            Assert.IsNotNull(customerDto);
            Assert.AreEqual(customer.Id, customerDto.Id);
            Assert.AreEqual(customer.Name, customerDto.Name);
            Assert.AreEqual(customer.Email, customerDto.Email);
            Assert.AreEqual(customer.Age, customerDto.Age);
        }

        [Test]
        public void CustomerDtoToCustomer_Mapping_IsValid()
        {
            // Arrange
            var customerDto = new CustomerDto
            {
                Id = 1,
                Name = "Test Customer",
                Email = "test@example.com",
                Age = 30
            };

            // Act
            var customer = _mapper.Map<Customer>(customerDto);

            // Assert
            Assert.IsNotNull(customer);
            Assert.AreEqual(customerDto.Id, customer.Id);
            Assert.AreEqual(customerDto.Name, customer.Name);
            Assert.AreEqual(customerDto.Email, customer.Email);
            Assert.AreEqual(customerDto.Age, customer.Age);
        }

        [Test]
        public void CustomerMappingProfile_Configuration_IsValid()
        {
            var config = new MapperConfiguration(cfg => cfg.AddProfile<CustomerMappingProfile>());
            config.AssertConfigurationIsValid();
        }
    }
}
