using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrueLayer.Pokemon.Api.Contract.PokeApi;

namespace TrueLayer.Pokemon.Api.FunctionalTests.Builders
{
    public class PokeApiResponseBuilder
    {
        private string _name = "mewtwo";
        private string _description = "It was created by scientist after years of horrific gene splicing and DNA engineering experiments.";
        private string _habitat = "rare";
        private bool _isLegendary = false;

        public PokeApiResponseBuilder WithName(string name)
        {
            _name = name;
            return this;
        }

        public PokeApiResponseBuilder WithDescription(string description)
        {
            _description = description;
            return this;
        }

        public PokeApiResponseBuilder WithHabitat(string habitat)
        {
            _habitat = habitat;
            return this;
        }

        public PokeApiResponseBuilder WithIsLegendary(bool isLegendary)
        {
            _isLegendary = isLegendary;
            return this;
        }

        public PokeApiResponse Create()
        {
            return new()
            {
                Name=_name,
                IsLegendary=_isLegendary,
                Habitat = new()
                {
                    Name=_habitat
                },
                FlavourTexts=new List<FlavourText>() 
                {
                   new FlavourText()
                   {
                       FlavourDescriptionText=_description,
                       Language = new()
                       {
                           Name="en"
                       }
                   }
                }
            };
        }
    }
}
