using Ardalis.Result;
using ChainOfRespKataWeb.DataAccess;
using Kata.ApiModels;
using MediatR;

namespace ChainOfRespKataWeb.UseCases.Heroes;

public class GetHeroHandler(HeroesDa herosDa, ILogger<GetHeroHandler> logger)
    : IRequestHandler<GetHeroCommand, Result<HeroDto>>
{
  private readonly HeroesDa _heroesDa = herosDa;
  private readonly ILogger<GetHeroHandler> _logger = logger;
  
  public async Task<Result<HeroDto>> Handle(GetHeroCommand request, CancellationToken cancellationToken)
  {
    // I took the code in CodeManager.AddHero, and pasted it in here.
    // I'll implement the chain of responsibility after I've removed the HeroManager class
    
    try
    {
      if (request.id < 0)
      {
        _logger.LogError("Invalid id");
        return Result.Invalid(new ValidationError("Invalid id"));
      }
      var hero = await _heroesDa.GetById(request.id);
      if (hero == null)
      {
        _logger.LogError("Hero not found");
        return Result.NotFound();
      }

      return new HeroDto { Id = hero.Id, Name = hero.Name };
    }
    catch (Exception ex)
    {
      _logger.LogError(ex, "Error getting hero by id");
      throw;
    }
  }
}
