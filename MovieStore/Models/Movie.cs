namespace MovieStore.Models;

public class Movie
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int Year { get; set; }
    public string Genre { get; set; }
    public decimal Price { get; set; }
    public int DirectorId { get; set; }
    public Director Director { get; set; }
    public ICollection<MovieActor> MovieActors { get; set; }
    public bool IsActive { get; set; } = true;
}