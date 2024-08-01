using AutoMapper;
using Microsoft.EntityFrameworkCore;
using MovieStore.Data;
using MovieStore.DTOs;
using MovieStore.Models;

namespace MovieStore.Services;

public class CustomerService : ICustomerService
{
    private readonly MovieStoreContext _context;
    private readonly IMapper _mapper;

    public CustomerService(MovieStoreContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<CustomerDto> RegisterCustomerAsync(CustomerDto customerDto)
    {
        var customer = _mapper.Map<Customer>(customerDto);
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync();
        return _mapper.Map<CustomerDto>(customer);
    }

    public async Task<CustomerDto> GetCustomerByIdAsync(int id)
    {
        var customer = await _context.Customers
            .Include(c => c.Purchases)
            .FirstOrDefaultAsync(c => c.Id == id);
        if (customer == null)
            throw new KeyNotFoundException("Customer not found");

        return _mapper.Map<CustomerDto>(customer);
    }

    public async Task<IEnumerable<CustomerDto>> GetAllCustomersAsync()
    {
        var customers = await _context.Customers
            .Include(c => c.Purchases)
            .ToListAsync();
        return _mapper.Map<IEnumerable<CustomerDto>>(customers);
    }

    public async Task DeleteCustomerAsync(int id)
    {
        var customer = await _context.Customers.FirstOrDefaultAsync(c => c.Id == id);
        if (customer == null)
            throw new KeyNotFoundException("Customer not found");

        _context.Customers.Remove(customer);
        await _context.SaveChangesAsync();
    }
}
