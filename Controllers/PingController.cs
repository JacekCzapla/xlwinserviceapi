using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace XLWinServiceAPI.Controllers
{
    public class PingController : ApiController
    {
        public IHttpActionResult Get()
        {

            return Ok(new PingResult {
                AppCode = "XLSAPI",
                AppVersion = Program.Version,
                Date = DateTime.Now,
                Message = $"Ping from XL API - {Program.VersionName} - {DateTime.Now}"
            });
            //return Ok("Ping from Messa XL SAPI - " + Program.VersionName);


        }
    }

    public class PingResult
    {
        public string AppCode { get; set; }
        public DateTime Date { get; set; }
        public string Message { get; set; }
        public string AppVersion { get; set; }
    }
}
