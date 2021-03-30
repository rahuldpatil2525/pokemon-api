using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TrueLayer.Pokemon.Api.Contract.V1.Response;
using TrueLayer.Pokemon.Api.Models;
using TrueLayer.Pokemon.Api.Services;

namespace TrueLayer.Pokemon.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonSpeciesService _pokemonSpeciesService;

        public PokemonController(IPokemonSpeciesService pokemonSpeciesService)
        {
            _pokemonSpeciesService = pokemonSpeciesService;
        }

        [HttpGet]
        [Route("v1/pokemon/{name}")]
        public async Task<ActionResult> Get(string name, CancellationToken ct = default)
        {
            var request = new PokemonSpeciesRequest(name);

            var result = await _pokemonSpeciesService.GetPokemonSpeciesAsync(request, ct);

            return Ok(new PokemonResponse()
            {
                Name = result.Name,
                Description = result.Description,
                Habitat = result.Habitat,
                IsLegendary = result.IsLegendary
            });
        }

        [HttpGet]
        [Route("v1/pokemon/translated/{name}")]
        public async Task<ActionResult> GetTranslation(string name, CancellationToken ct = default)
        {
            var request = new PokemonSpeciesRequest(name);

            var result = await _pokemonSpeciesService.GetTranslatedPokemonSpeciesAsync(request, ct);

            return Ok(new PokemonTranslatedResponse()
            {
                Name = result.Name,
                Description = result.Description,
                Habitat = result.Habitat,
                IsLegendary = result.IsLegendary,
                TranslationProvider=result.TranslationProvider
            });
        }
    }
}
