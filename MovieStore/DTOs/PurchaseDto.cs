namespace MovieStore.DTOs;

public class PurchaseDto
{
    public int Id { get; set; }
    public int MovieId { get; set; }
    public string MovieTitle { get; set; }
    public decimal Price { get; set; }
    public DateTime PurchaseDate { get; set; }
}