using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IpLocator.WebAPI
{
    public class CityController : ApiController
    {
        [HttpGet]
        public string Locations(string city)
        {
            return city + "сосити";
        }
    }
}
