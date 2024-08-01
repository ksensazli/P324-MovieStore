using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MovieStore.Data;
using MovieStore.DTOs;
using MovieStore.Models;

namespace MovieStore.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PurchasesController : ControllerBase
{
    private readonly MovieStoreContext _context;
    private readonly IMapper _mapper;

    public PurchasesController(MovieStoreContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> PurchaseMovie([FromBody] PurchaseDto purchaseDto)
    {
        var customer = await _context.Customers.FindAsync(purchaseDto.Id);
        var movie = await _context.Movies.FindAsync(purchaseDto.MovieId);
        
        if (customer == null || movie == null || !movie.IsActive)
            return BadRequest("Invalid customer or movie");

        var purchase = new Purchase
        {
            CustomerId = purchaseDto.Id,
            MovieId = purchaseDto.MovieId,
            Price = movie.Price,
            PurchaseDate = DateTime.Now
        };

        _context.Purchases.Add(purchase);
        await _context.SaveChangesAsync();

        return Ok(_mapper.Map<PurchaseDto>(purchase));
    }

    [HttpGet("customer/{customerId}")]
    public async Task<IActionResult> GetPurchasesByCustomer(int customerId)
    {
        var purchases = await _context.Purchases
            .Include(p => p.Movie)
            .Where(p => p.CustomerId == customerId)
            .ToListAsync();

        return Ok(_mapper.Map<IEnumerable<PurchaseDto>>(purchases));
    }
}
