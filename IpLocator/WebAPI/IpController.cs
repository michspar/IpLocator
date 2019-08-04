using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace IpLocator.WebAPI
{
    public class IpController : ApiController
    {
        [HttpGet]
        public string Location(string ip)
        {
            return "fuck " + ip;
        }
    }
}
