using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Common.Geometry;
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

        public async Task<IEnumerable<Critter>> GetWildPokemons(double lat, double lng, int level = 15)
        {
            if (level < 1 || level > 30)
            {
                level = 15;
            }

            var latLng = S2LatLng.FromDegrees(lat, lng);
            var s2CellId = S2CellId.FromLatLng(latLng).ParentForLevel(level);
            var cellCenter = s2CellId.ToLatLng();
            var client = PokemonGoApiHack.GetClient();
            await client.Login.DoLogin();

            await client.Player.UpdatePlayerLocation(cellCenter.LatDegrees, cellCenter.LngDegrees, 0d);
            var mapObjs = await client.Map.GetMapObjects();
            var wildPokemons = mapObjs.Item1.MapCells.SelectMany(x => x.WildPokemons)
                .Where(p => !Pests.Contains(p.PokemonData.PokemonId))
                .Select(
                    p =>
                        new Critter
                        {
                            Name = p.PokemonData.PokemonId.ToString(),
                            Lat = p.Latitude,
                            Lng = p.Longitude,
                            TimeTillHidden = TimeSpan.FromMilliseconds(p.TimeTillHiddenMs)
                        });

            //var test = mapObjs.Item1.MapCells.SelectMany(x => x.WildPokemons).FirstOrDefault();
            //if (test != null)
            //{
            //    await Task.Delay(15000);
            //    await client.Player.UpdatePlayerLocation(test.Latitude, test.Longitude, 0d);
            //    var result = await client.Encounter.EncounterPokemon(test.EncounterId, test.SpawnPointId);
            //}

            return wildPokemons;
        }
    }
}
