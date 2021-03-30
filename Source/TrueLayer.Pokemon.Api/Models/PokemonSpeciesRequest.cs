using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrueLayer.Pokemon.Api.Models
{
    public class PokemonSpeciesRequest
    {

        public PokemonSpeciesRequest(string name)
        {
            Name = name;
        }

        public string Name { get; }
    }
}
