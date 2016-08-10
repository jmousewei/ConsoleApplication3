using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Website.Service;
using Website.Service.Models;

namespace Website.Controllers
{
    [RoutePrefix("api/critters")]
    public class CrittersController : ApiController
    {
        [Route("@{lat:double},{lng:double}")]
        [HttpGet]
        public async Task<IEnumerable<Critter>> Get(double lat, double lng)
        {
            var service = new CritterService();
            return await service.GetWildPokemons(lat, lng);
        }
    }
}
