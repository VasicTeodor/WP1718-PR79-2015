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
        private DispatcherRepo _dispRepo = new DispatcherRepo();
        private DriverRepo _drivRepo = new DriverRepo();
        private CustomerRepo _custRepo = new CustomerRepo();
        private DriveRepo _driveRepo = new DriveRepo();

        [HttpPost]
        [Route("api/Dispatcher/AddDriver")]
        public HttpResponseMessage AddDriver([FromBody]Driver driver)
        {
            if (!_custRepo.CheckIfCustomerExists(driver.Username) &&
                !_dispRepo.CheckIfDispatcherExists(driver.Username) &&
                !_drivRepo.CheckIfDriverExists(driver.Username))
            {
                driver.Id = Guid.NewGuid();
                driver.Role = Enums.Roles.Driver;
                driver.Location = new Location { Address = "see", X = 0, Y = 0 };
                driver.Car = new Car { Id = -1, ModelYear = 1995, RegNumber = "NS993TX", Type = Enums.CarTypes.Car };
                _drivRepo.NewDriver(driver);
                return Request.CreateResponse(HttpStatusCode.Created, _drivRepo.RetriveDriverById(driver.Id));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        public HttpResponseMessage CreateDrive([FromBody]Drive drive)
        {
            _driveRepo.AddNewDrive(drive);
            return Request.CreateResponse(HttpStatusCode.Created, _driveRepo.GetAllDrives());
        }
    }
}
