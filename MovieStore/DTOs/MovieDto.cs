namespace MovieStore.DTOs;

public class MovieDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public int Year { get; set; }
    public string Genre { get; set; }
    public decimal Price { get; set; }
    public int DirectorId { get; set; }
    public string DirectorName { get; set; }
    public ICollection<ActorDto> Actors { get; set; }
}
