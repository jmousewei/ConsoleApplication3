using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Website.Models;

namespace Website.Controllers
{
    [RoutePrefix("api/critters")]
    public class CrittersController : ApiController
    {
        [Route("@{lat:double},{lng:double},{level:int}")]
        [HttpGet]
        public IEnumerable<Critter> Get()
        {
            return Enumerable.Empty<Critter>();
        }
    }
}
