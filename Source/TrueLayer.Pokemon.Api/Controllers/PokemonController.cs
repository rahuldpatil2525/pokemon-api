using Microsoft.AspNetCore.Mvc;
using TrueLayer.Pokemon.Api.Contract.V1.Response;

namespace TrueLayer.Pokemon.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PokemonController : ControllerBase
    {
        [HttpGet]
        [Route("v1/pokemon/{name}")]
        public PokemonResponse Get(string name)
        {
            return new()
            {
                Name = name,
                Description = "It was created by scientist after years of horrific gene splicing and DNA engineering experiments.",
                Habitat = "rare",
                IsLegendary = false
            };
        }

        [HttpGet]
        [Route("v1/pokemon/translated/{name}")]
        public PokemonResponse GetTranslation(string name)
        {
            return new()
            {
                Name = name,
                Description = "It was created by scientist after years of horrific gene splicing and DNA engineering experiments.",
                Habitat = "rare",
                IsLegendary = true
            };
        }
    }
}
