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
    public class DispatcherController : ApiController
    {
        [HttpPost]
        [Route("api/Dispatcher/AddDriver")]
        public HttpResponseMessage AddDriver([FromBody]Driver driver)
        {
            if (!DataRepository._customerRepo.CheckIfCustomerExists(driver.Username) &&
                !DataRepository._dispatcherRepo.CheckIfDispatcherExists(driver.Username) &&
                !DataRepository._driverRepo.CheckIfDriverExists(driver.Username))
            {
                driver.Id = Guid.NewGuid();
                driver.Role = Enums.Roles.Driver;
                driver.Location = new Location { Address = "garage", X = 0, Y = 0 };
                driver.Car = new Car { CarId = -1, ModelYear = 1995, RegNumber = "NS993TX", Type = Enums.CarTypes.Car };
                DataRepository._driverRepo.NewDriver(driver);
                return Request.CreateResponse(HttpStatusCode.Created, DataRepository._driverRepo.RetriveDriverById(driver.Id));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("api/Dispatcher/CreateDrive")]
        public HttpResponseMessage CreateDrive([FromBody]Drive drive)
        {
            drive.DriveId = Guid.NewGuid();
            drive.Date = DateTime.Now;
            drive.State = Enums.Status.Formated;
            DataRepository._driveRepo.AddNewDriveDispatcher(drive);
            return Request.CreateResponse(HttpStatusCode.Created, DataRepository._driveRepo.GetAllDrives());
        }
    }
}
