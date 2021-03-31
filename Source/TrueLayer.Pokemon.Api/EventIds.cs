using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TrueLayer.Pokemon.Api
{
    public class EventIds
    {
        public const int GeneralUnhandledException = 100;
        public const int TranslationApiFailedException = 101;
        public const int RetryException = 102;
        public const int PokemonNotFoundException = 103;
    }
}
