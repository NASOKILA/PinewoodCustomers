using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using Pinewood.Customers.Db;
using Pinewood.Customers.Models.DbModels;

namespace Pinewood.Customers.Tests.Repos
{
    [TestFixture]
    public class CustomerRepositoryTests
    {
        private DbContextOptions<PinewoodCustomersDbContext> _dbContextOptions;
        private PinewoodCustomersDbContext _context;
        private CustomerRepository _repository;

        [SetUp]
        public void Setup()
        {
            _dbContextOptions = new DbContextOptionsBuilder<PinewoodCustomersDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            _context = new PinewoodCustomersDbContext(_dbContextOptions);
            _repository = new CustomerRepository(_context);
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.EnsureDeleted();
            _context.Dispose();
        }

        [Test]
        public async Task GetByIdAsync_ExistingId_ReturnsCustomer()
        {
            // Arrange
            var customer = new Customer { Id = 1, Email =  "testcustomer@abv.bg", Name = "Test Customer", Age = 30 };
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetByIdAsync(1);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("Test Customer", result.Name);
            Assert.AreEqual("testcustomer@abv.bg", result.Email);
            Assert.AreEqual(30, result.Age);
        }

        [Test]
        public async Task GetByIdAsync_NonExistingId_ReturnsNull()
        {
            // Act
            var result = await _repository.GetByIdAsync(999);

            // Assert
            Assert.IsNull(result);
        }

        [Test]
        public async Task GetAllAsync_ReturnsAllCustomers()
        {
            // Arrange
            var customers = new List<Customer>
            {
                new Customer { Id = 1, Name = "Customer 1", Email = "customer1@abv.bg", Age = 55 },
                new Customer { Id = 2, Name = "Customer 2", Email = "customer2@abv.bg", Age = 22 }
            };
            _context.Customers.AddRange(customers);
            await _context.SaveChangesAsync();

            // Act
            var result = await _repository.GetAllAsync();

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.IsTrue(result.Any(c => c.Name == "Customer 1"));
            Assert.IsTrue(result.Any(c => c.Name == "Customer 2"));
        }

        [Test]
        public async Task AddAsync_AddsCustomer()
        {
            // Arrange
            var customer = new Customer { Id = 1, Name = "New Customer", Email = "newcustomer@abv.bg", Age = 12 };

            // Act
            var result = await _repository.AddAsync(customer);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("New Customer", result.Name);

            Assert.AreEqual("newcustomer@abv.bg", result.Email);
            Assert.AreEqual(12, result.Age);
            Assert.AreEqual(1, _context.Customers.Count());
        }

        [Test]
        public async Task UpdateAsync_UpdatesCustomer()
        {
            // Arrange
            var customer = new Customer { Id = 1, Name = "Original Customer", Email = "originalCustomer@abv.bg", Age = 24 };
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            customer.Name = "Updated Customer";
            customer.Email = "updatedCustomer@abv.bg";

            // Act
            var result = await _repository.UpdateAsync(customer);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(1, result.Id);
            Assert.AreEqual("Updated Customer", result.Name);
            Assert.AreEqual("updatedCustomer@abv.bg", result.Email);
            Assert.AreEqual(24, result.Age);

            var updatedCustomer = await _context.Customers.FindAsync(1);
            Assert.AreEqual("Updated Customer", updatedCustomer.Name);
            Assert.AreEqual("updatedCustomer@abv.bg", updatedCustomer.Email);
        }

        [Test]
        public async Task DeleteAsync_ExistingId_DeletesCustomer()
        {
            // Arrange
            var customer = new Customer { Id = 1, Name = "Customer to Delete", Email = "customerToDelete@abv.bg", Age = 35 };
            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            // Act
            await _repository.DeleteAsync(1);

            // Assert
            var deletedCustomer = await _context.Customers.FindAsync(1);
            Assert.IsNull(deletedCustomer);
        }

        [Test]
        public async Task DeleteAsync_NonExistingId_DoesNothing()
        {
            // Act
            await _repository.DeleteAsync(999);

            // Assert
            // No exception means pass
        }
    }
}
