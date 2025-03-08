using Ardalis.Result;
using Kata.ApiModels;
using MediatR;

namespace ChainOfRespKataWeb.UseCases.Heroes;

public record GetHeroCommand(int id) : IRequest<Result<HeroDto>>;
