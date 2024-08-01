namespace MovieStore.Models;

public class Customer
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public ICollection<Purchase> Purchases { get; set; }
    public ICollection<string> FavoriteGenres { get; set; }
}
