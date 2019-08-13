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
        DbHolder.DbHolder holder = new DbHolder.DbHolder();

        [HttpGet]
        public DbHolder.DbLocation Location(string ip)
        {
            IPAddress inputIp = null;

            if (!IPAddress.TryParse(ip, out inputIp))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            holder.LoadDbFromFile(System.Web.Hosting.HostingEnvironment.MapPath("~/geobase.dat"));

            var ipBytes = BitConverter.ToUInt32(inputIp.GetAddressBytes(), 0);
            var location = holder.Ranges.FirstOrDefault(r => r.ip_from <= ipBytes && r.ip_to >= ipBytes);

            if (location.location_index == 0 && location.ip_from == 0 && location.ip_to == 0)
                throw new HttpResponseException(HttpStatusCode.NoContent);

            return holder.Locations[location.location_index];
        }
    }
}
