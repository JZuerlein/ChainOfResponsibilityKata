using Kata.ApiModels;
using Kata.Entities;
using MediatR;
using Ardalis.Result;

namespace ChainOfRespKataWeb.UseCases.Heroes;

public record AddHeroCommand(string Name) : IRequest<Result<HeroDto>>;
