using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TaxiService.ApiHelpers;
using TaxiService.Models;
using TaxiService.Repository;

namespace TaxiService.Controllers
{
    [BasicAuthentication]
    public class DriveController : ApiController
    {
        [HttpGet]
        [Route("api/Drive/GetDrive")]
        public HttpResponseMessage QuitDrive([FromUri]string id)
        {
            Guid driveId = Guid.Parse(id);
            Drive drive = null; 
            drive = DataRepository._driveRepo.RetriveDriveById(driveId);
            if(drive != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, drive);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }

        [HttpGet]
        [Route("api/Drive/GetAllDrives")]
        public HttpResponseMessage GetAllDrives()
        {
            List<Drive> allDrives = null;
            allDrives = DataRepository._driveRepo.GetAllDrives().ToList();
            if (allDrives != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, allDrives);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
    }
}
