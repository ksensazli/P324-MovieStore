using FluentValidation;
using MovieStore.DTOs;

namespace MovieStore.Validations;

public class MovieValidator : AbstractValidator<MovieDto>
{
    public MovieValidator()
    {
        RuleFor(m => m.Title).NotEmpty();
        RuleFor(m => m.Year).GreaterThan(1900);
        RuleFor(m => m.Genre).NotEmpty();
        RuleFor(m => m.Price).GreaterThan(0);
        RuleFor(m => m.DirectorId).GreaterThan(0);
    }
}
