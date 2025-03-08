using Kata.ApiModels;
using Kata.Entities;
using MediatR;
using Ardalis.Result;
using ChainOfRespKataWeb.DataAccess;

namespace ChainOfRespKataWeb.UseCases.Heroes;

public class AddHeroHandler(HeroesDa heroesDa, ILogger<AddHeroHandler> logger)
    : IRequestHandler<AddHeroCommand, Result<HeroDto>>
{
  private readonly HeroesDa _heroesDa = heroesDa;
  private readonly ILogger<AddHeroHandler> _logger = logger;

  public async Task<Result<HeroDto>> Handle(AddHeroCommand request, CancellationToken cancellationToken)
  {
    // I took the code in CodeManager.AddHero, and pasted it in here.
    // I'll implement the chain of responsibility after I've removed the HeroManager class
    
    try
    {
      if (string.IsNullOrWhiteSpace(request.Name))
      {
        _logger.LogError("New Hero must have a name");
        return Result.Invalid(new ValidationError("New Hero must have a name"));
      }
      var hero = new Hero { Name = request.Name };
      hero = await _heroesDa.Add(hero);
      
      if (hero == null)
      {
        _logger.LogError("Hero FAILED TO ADD");
        return Result.Error("Hero failed to add");
      }
      
      return new HeroDto { Id = hero.Id, Name = hero.Name };
    }
    catch (DuplicateKeyException ex)
    {
      _logger.LogWarning(ex, "Duplicate name provided.");
      throw;
    }
  }
}
