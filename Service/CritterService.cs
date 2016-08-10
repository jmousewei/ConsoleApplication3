using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Common.Geometry;
using PokemonGo.RocketAPI;
using POGOProtos.Enums;
using Website.Service.Models;

namespace Website.Service
{
    public class CritterService
    {
        private static HashSet<PokemonId> Pests =
            new HashSet<PokemonId>
            {
                PokemonId.Bellsprout,
                PokemonId.Caterpie,
                PokemonId.Clefairy,
                PokemonId.Cubone,
                PokemonId.Diglett,
                PokemonId.Dodrio,
                PokemonId.Doduo,
                PokemonId.Eevee,
                PokemonId.Ekans,
                PokemonId.Fearow,
                PokemonId.Golbat,
                PokemonId.Goldeen,
                PokemonId.Horsea,
                PokemonId.Kakuna,
                PokemonId.Krabby,
                PokemonId.Magikarp,
                PokemonId.NidoranFemale,
                PokemonId.NidoranMale,
                PokemonId.Oddish,
                PokemonId.Paras,
                PokemonId.Pidgeot,
                PokemonId.Pidgeotto,
                PokemonId.Pidgey,
                PokemonId.Pinsir,
                PokemonId.Poliwag,
                PokemonId.Psyduck,
                PokemonId.Raticate,
                PokemonId.Rattata,
                PokemonId.Spearow,
                PokemonId.Staryu,
                PokemonId.Tentacool,
                PokemonId.Weedle,
                PokemonId.Zubat
            };

        public async Task<IEnumerable<Critter>> GetWildPokemons(double lat, double lng)
        {
            //-43.410237, 172.805990
            //-43.610216, 172.418190
            var topRight = S2LatLng.FromDegrees(-43.410237, 172.805990);
            var bottomLeft = S2LatLng.FromDegrees(-43.610216, 172.418190);
            var rect = new S2LatLngRect(bottomLeft, topRight);
            var latLng = S2LatLng.FromDegrees(lat, lng);

            if (!rect.Contains(latLng))
            {
                // Out of range.
                return Enumerable.Empty<Critter>();
            }

            var client = PokemonGoApiHack.GetClient();
            await client.Login.DoLogin();
            await client.Player.UpdatePlayerLocation(lat, lng, 0d);
            var mapObjs = await client.Map.GetMapObjects();
            var approxNow = DateTime.Now;
            var theBeginning = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            //var test = mapObjs.Item1.MapCells.SelectMany(x => x.WildPokemons).FirstOrDefault();
            //if (test != null)
            //{
            //    await Task.Delay(15000);
            //    await client.Player.UpdatePlayerLocation(test.Latitude, test.Longitude, 0d);
            //    var result = await client.Encounter.EncounterPokemon(test.EncounterId, test.SpawnPointId);
            //}

            var wildPokemons =
                mapObjs.Item1.MapCells.SelectMany(x => x.CatchablePokemons)
                    .Select(x =>
                            {
                                var timeTillHidden = x.ExpirationTimestampMs < 0
                                    ? TimeSpan.Zero
                                    : theBeginning.AddMilliseconds(x.ExpirationTimestampMs).ToLocalTime() -
                                      approxNow;

                                return new Critter
                                       {
                                           Name = x.PokemonId.ToString(),
                                           Lat = x.Latitude,
                                           Lng = x.Longitude,
                                           TimeTillHidden = timeTillHidden,
                                           TimeTillHiddenString =
                                               timeTillHidden == TimeSpan.Zero
                                                   ? "unknown"
                                                   : string.Format("{0:mm}:{0:ss}", timeTillHidden),
                                           ServerTime = approxNow.ToUniversalTime()
                                       };
                            })
                    .ToList();

            return wildPokemons;
        }
    }
}
