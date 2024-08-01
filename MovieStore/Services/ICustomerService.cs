using MovieStore.DTOs;

namespace MovieStore.Services;

public interface ICustomerService
{
    Task<CustomerDto> RegisterCustomerAsync(CustomerDto customerDto);
    Task<CustomerDto> GetCustomerByIdAsync(int id);
    Task<IEnumerable<CustomerDto>> GetAllCustomersAsync();
    Task DeleteCustomerAsync(int id);
}
