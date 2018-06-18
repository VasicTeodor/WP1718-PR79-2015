using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TaxiService.Models;
using TaxiService.Repository;

namespace TaxiService.Controllers
{
    public class CustomerController : ApiController
    {
        [HttpPost]
        [Route("api/Customer/CreateNewDrive")]
        public HttpResponseMessage CreateNewDrive([FromBody]Drive drive)
        {
            drive.DriveId = Guid.NewGuid();
            drive.State = Enums.Status.Created;
            drive.Destination = new Location
            {
                Address = "Unset",
                X = 0,
                Y = 0
            };
            drive.Price = 0;
            DataRepository._driveRepo.AddNewDriveCustomer(drive);
            return Request.CreateResponse(HttpStatusCode.Created, DataRepository._driveRepo.GetAllDrives());
        }
    }
}
