namespace MovieStore.Models;

public class Purchase
{
    public int Id { get; set; }
    public int CustomerId { get; set; }
    public Customer Customer { get; set; }
    public int MovieId { get; set; }
    public Movie Movie { get; set; }
    public decimal Price { get; set; }
    public DateTime PurchaseDate { get; set; }
}
