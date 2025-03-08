using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using ChainOfRespKataWeb.UseCases.Heroes;
using Kata.ApiModels;
using ChainOfRespKataWeb.DataAccess;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ChainOfRespKataWeb.Controllers;

[Route("[controller]")]
public class HeroesController(ILogger<HeroesController> logger,
    IMediator mediator) : ControllerBase
{
  private readonly ILogger<HeroesController> _logger = logger;
  private readonly IMediator _mediator = mediator;

  [HttpGet("{id}")]
  public async Task<ActionResult<HeroDto>> GetById(int id)
  {
    if (id < 0)
    {
      _logger.LogError("Invalid id");
      return BadRequest();
    }

    // First Attempt
    // I've added MediatR, commands and handlers.
    // But mapping the Ardalis.Result to an ActionResult looks ugly.
    // var response = await _mediator.Send(new GetHeroCommand(id));
    //
    // if (response.IsSuccess)
    //   return Ok(response.Value);
    // else if (response.IsNotFound())
    //   return NotFound();
    // else
    //   return Problem();
    
    // Second Attempt
    // I added Ardalis.Result.AspNetCore so it could do the mapping for me.
    // It works, but I'm going to tighten it up.
    // var response = await _mediator.Send(new GetHeroCommand(id));
    // return response.ToActionResult(this);
    
    // Third Attempt
    // :^)
    return this.ToActionResult(await _mediator.Send(new GetHeroCommand(id)));
  }

  [HttpPost()]
  public async Task<ActionResult<HeroDto>> Add([FromBody] HeroDto hero)
  {
    if (hero.Id != 0)
    {
      _logger.LogError("New Hero cannot have Id");
      return BadRequest();
    }

    if (String.IsNullOrWhiteSpace(hero.Name))
    {
      _logger.LogError("New Hero must have a name");
      return BadRequest();
    }

    try
    {
      return this.ToActionResult(await _mediator.Send(new AddHeroCommand(hero.Name)));
    }
    catch (DuplicateKeyException)
    {
      return BadRequest("Name must be unique");
    }
  }
}
