using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Moq;
using MovieStore.Data;
using MovieStore.DTOs;
using MovieStore.Models;
using MovieStore.Services;
using MovieStore.Tests.Helpers;

namespace MovieStore.Tests;

public class CustomerServiceTests
{
    private readonly CustomerService _customerService;
    private readonly Mock<MovieStoreContext> _contextMock;
    private readonly Mock<IMapper> _mapperMock;

    public CustomerServiceTests()
    {
        _contextMock = new Mock<MovieStoreContext>();
        _mapperMock = new Mock<IMapper>();
        _customerService = new CustomerService(_contextMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task RegisterCustomerAsync_AddsCustomer()
    {
        // Arrange
        var customerDto = new CustomerDto { FirstName = "New Customer" };
        var customer = new Customer { Id = 1, FirstName = "New Customer" };

        _mapperMock.Setup(m => m.Map<Customer>(It.IsAny<CustomerDto>())).Returns(customer);
        _contextMock.Setup(c => c.Customers.Add(customer)).Verifiable();
        _mapperMock.Setup(m => m.Map<CustomerDto>(customer)).Returns(customerDto);

        // Act
        var result = await _customerService.RegisterCustomerAsync(customerDto);

        // Assert
        _contextMock.Verify(c => c.Customers.Add(customer), Times.Once);
        Assert.NotNull(result);
        Assert.Equal(customerDto.FirstName, result.FirstName);
    }

    [Fact]
    public async Task GetAllCustomersAsync_ReturnsCustomers()
    {
        // Arrange
        var customers = new List<Customer>
        {
            new Customer { Id = 1, FirstName = "Customer 1" },
            new Customer { Id = 2, FirstName = "Customer 2" }
        };

        var customersDbSetMock = customers.CreateDbSetMock();
        _contextMock.Setup(c => c.Customers).Returns(customersDbSetMock.Object);

        var customerDtos = new List<CustomerDto>
        {
            new CustomerDto { Id = 1, FirstName = "Customer 1" },
            new CustomerDto { Id = 2, FirstName = "Customer 2" }
        };

        _mapperMock.Setup(m => m.Map<IEnumerable<CustomerDto>>(It.IsAny<IEnumerable<Customer>>()))
            .Returns(customerDtos);

        // Act
        var result = await _customerService.GetAllCustomersAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count());
    }

    [Fact]
    public async Task GetCustomerByIdAsync_ReturnsCustomer()
    {
        // Arrange
        var customer = new Customer { Id = 1, FirstName = "Customer 1" };

        _contextMock.Setup(c => c.Customers.FindAsync(1))
            .ReturnsAsync(customer);

        var customerDto = new CustomerDto { Id = 1, FirstName = "Customer 1" };

        _mapperMock.Setup(m => m.Map<CustomerDto>(It.IsAny<Customer>())).Returns(customerDto);

        // Act
        var result = await _customerService.GetCustomerByIdAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task DeleteCustomerAsync_RemovesCustomer()
    {
        // Arrange
        var customer = new Customer { Id = 1, FirstName = "Customer 1" };

        _contextMock.Setup(c => c.Customers.FirstOrDefaultAsync(c => c.Id == 1, default))
            .ReturnsAsync(customer);

        // Act
        await _customerService.DeleteCustomerAsync(1);

        // Assert
        _contextMock.Verify(c => c.Customers.Remove(customer), Times.Once);
    }
}