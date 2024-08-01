namespace MovieStore.DTOs;

public class CustomerDto
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public ICollection<PurchaseDto> Purchases { get; set; }
    public ICollection<string> FavoriteGenres { get; set; }
}