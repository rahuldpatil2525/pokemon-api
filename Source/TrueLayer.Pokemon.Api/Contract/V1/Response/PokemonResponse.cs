using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrueLayer.Pokemon.Api.Contract.V1.Response
{
    public class PokemonResponse
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Habitat { get; set; }

        public bool IsLegendary { get; set; }
    }

    public class PokemonTranslatedResponse
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public string Habitat { get; set; }

        public bool IsLegendary { get; set; }

        public string TranslationProvider { get; set; }
    }
}
