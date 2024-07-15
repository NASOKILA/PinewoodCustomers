using Microsoft.AspNetCore.Mvc;
using Pinewood.Customers.Models.Interfaces;
using Pinewood.Customers.Models.DbModels;
using AutoMapper;
using Pinewood.Customers.Models.DTOModels;

[ApiController]
[Route("api/[controller]")]
public class CustomersController : ControllerBase
{
    private readonly IRepository<Customer> _customerRepository;
    private readonly IMapper _mapper;

    public CustomersController(IRepository<Customer> customerRepository, IMapper mapper)
    {
        _customerRepository = customerRepository;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomerDto>>> GetCustomers()
    {
        var customers = await _customerRepository.GetAllAsync();
        
        var customerDtos = _mapper.Map<IEnumerable<CustomerDto>>(customers);

        return Ok(customerDtos);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<CustomerDto>> GetCustomerById(int id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);

        if (customer == null)
        {
            return NotFound();
        }

        var customerDto = _mapper.Map<CustomerDto>(customer);

        return Ok(customerDto);
    }

    [HttpPost]
    public async Task<ActionResult<CustomerDto>> CreateCustomer(CustomerDto customerDto)
    {
        var customer = _mapper.Map<Customer>(customerDto);

        var createdCustomer = await _customerRepository.AddAsync(customer);

        await _customerRepository.SaveChangesAsync();

        var createdCustomerDto = _mapper.Map<CustomerDto>(createdCustomer);

        return CreatedAtAction(nameof(GetCustomerById), new { id = createdCustomerDto.Id }, createdCustomerDto);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateCustomer(int id, CustomerDto customerDto)
    {
        if (id != customerDto.Id)
        {
            return BadRequest();
        }

        var existingCustomer = await _customerRepository.GetByIdAsync(id);

        if (existingCustomer == null)
        {
            return NotFound();
        }

        _mapper.Map(customerDto, existingCustomer);

        await _customerRepository.UpdateAsync(existingCustomer);

        await _customerRepository.SaveChangesAsync();

        return Ok(existingCustomer);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteCustomer(int id)
    {
        await _customerRepository.DeleteAsync(id);

        await _customerRepository.SaveChangesAsync();

        return NoContent();
    }
}
