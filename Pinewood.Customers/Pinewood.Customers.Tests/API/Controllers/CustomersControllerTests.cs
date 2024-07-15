using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Pinewood.Customers.Models.Interfaces;
using Pinewood.Customers.Models.DbModels;
using Pinewood.Customers.Models.DTOModels;
using AutoMapper;

namespace Pinewood.Customers.Tests
{
    [TestFixture]
    public class CustomersControllerTests
    {
        private CustomersController _controller;
        private Mock<IRepository<Customer>> _mockCustomerRepository;
        private Mock<IMapper> _mockMapper;

        [SetUp]
        public void Setup()
        {
            _mockCustomerRepository = new Mock<IRepository<Customer>>();
            _mockMapper = new Mock<IMapper>();
            _controller = new CustomersController(_mockCustomerRepository.Object, _mockMapper.Object);
        }

        [Test]
        public async Task GetCustomers_ReturnsOkWithCustomerDtos()
        {
            // Arrange
            var customers = new List<Customer>
            {
                new Customer { Id = 1, Name = "Customer 1" },
                new Customer { Id = 2, Name = "Customer 2" }
            };
            _mockCustomerRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(customers);

            var customerDtos = customers.Select(c => new CustomerDto { Id = c.Id, Name = c.Name });
            _mockMapper.Setup(m => m.Map<IEnumerable<CustomerDto>>(customers)).Returns(customerDtos);

            // Act
            var result = await _controller.GetCustomers();

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var returnedDtos = okResult.Value as IEnumerable<CustomerDto>;
            Assert.IsNotNull(returnedDtos);
            Assert.AreEqual(customers.Count, returnedDtos.Count());
        }

        [Test]
        public async Task GetCustomerById_ExistingId_ReturnsOkWithCustomerDto()
        {
            // Arrange
            var customerId = 1;
            var customer = new Customer { Id = customerId, Name = "Customer 1" };
            _mockCustomerRepository.Setup(repo => repo.GetByIdAsync(customerId)).ReturnsAsync(customer);

            var customerDto = new CustomerDto { Id = customer.Id, Name = customer.Name };
            _mockMapper.Setup(m => m.Map<CustomerDto>(customer)).Returns(customerDto);

            // Act
            var result = await _controller.GetCustomerById(customerId);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result.Result);
            var okResult = result.Result as OkObjectResult;
            Assert.IsNotNull(okResult);

            var returnedDto = okResult.Value as CustomerDto;
            Assert.IsNotNull(returnedDto);
            Assert.AreEqual(customer.Id, returnedDto.Id);
        }

        [Test]
        public async Task GetCustomerById_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var nonExistingId = 999;
            _mockCustomerRepository.Setup(repo => repo.GetByIdAsync(nonExistingId)).ReturnsAsync((Customer)null);

            // Act
            var result = await _controller.GetCustomerById(nonExistingId);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result.Result);
        }

        [Test]
        public async Task CreateCustomer_ValidCustomerDto_ReturnsCreatedAtAction()
        {
            // Arrange
            var customerDto = new CustomerDto { Name = "New Customer" };
            var customer = new Customer { Id = 1, Name = customerDto.Name };
            _mockMapper.Setup(m => m.Map<Customer>(customerDto)).Returns(customer);

            _mockCustomerRepository.Setup(repo => repo.AddAsync(customer)).ReturnsAsync(customer);
            _mockCustomerRepository.Setup(repo => repo.SaveChangesAsync()).Returns(Task.CompletedTask);

            var createdCustomerDto = new CustomerDto { Id = customer.Id, Name = customer.Name };
            _mockMapper.Setup(m => m.Map<CustomerDto>(customer)).Returns(createdCustomerDto);

            // Act
            var result = await _controller.CreateCustomer(customerDto);

            // Assert
            Assert.IsInstanceOf<CreatedAtActionResult>(result.Result);
            var createdAtActionResult = result.Result as CreatedAtActionResult;
            Assert.IsNotNull(createdAtActionResult);
            Assert.AreEqual(nameof(CustomersController.GetCustomerById), createdAtActionResult.ActionName);
            Assert.AreEqual(customer.Id, createdAtActionResult.RouteValues["id"]);
            Assert.AreEqual(createdCustomerDto, createdAtActionResult.Value);
        }

        [Test]
        public async Task UpdateCustomer_ExistingIdAndValidCustomerDto_ReturnsOk()
        {
            // Arrange
            var customerId = 1;
            var customerDto = new CustomerDto { Id = customerId, Name = "Updated Customer" };
            var existingCustomer = new Customer { Id = customerId, Name = "Original Customer" };

            _mockCustomerRepository.Setup(repo => repo.GetByIdAsync(customerId)).ReturnsAsync(existingCustomer);

            _mockMapper.Setup(m => m.Map(customerDto, existingCustomer)).Returns(existingCustomer);

            _mockCustomerRepository.Setup(repo => repo.UpdateAsync(existingCustomer)).ReturnsAsync(existingCustomer);
            _mockCustomerRepository.Setup(repo => repo.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateCustomer(customerId, customerDto);

            // Assert
            Assert.IsInstanceOf<OkObjectResult>(result);
        }

        [Test]
        public async Task UpdateCustomer_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var nonExistingId = 999;
            var customerDto = new CustomerDto { Id = nonExistingId, Name = "Updated Customer" };

            _mockCustomerRepository.Setup(repo => repo.GetByIdAsync(nonExistingId)).ReturnsAsync((Customer)null);

            // Act
            var result = await _controller.UpdateCustomer(nonExistingId, customerDto);

            // Assert
            Assert.IsInstanceOf<NotFoundResult>(result);
        }

        [Test]
        public async Task DeleteCustomer_ExistingId_ReturnsNoContent()
        {
            // Arrange
            var customerId = 1;

            _mockCustomerRepository.Setup(repo => repo.DeleteAsync(customerId)).Returns(Task.CompletedTask);
            _mockCustomerRepository.Setup(repo => repo.SaveChangesAsync()).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.DeleteCustomer(customerId);

            // Assert
            Assert.IsInstanceOf<NoContentResult>(result);
        }
    }
}
