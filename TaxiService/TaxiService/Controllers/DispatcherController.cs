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
    public class DispatcherController : ApiController
    {
        [HttpPost]
        [Route("api/Dispatcher/AddDriver")]
        [BasicAuthentication]
        public HttpResponseMessage AddDriver([FromBody]Driver driver)
        {
            if (!DataRepository._customerRepo.CheckIfCustomerExists(driver.Username) &&
                !DataRepository._dispatcherRepo.CheckIfDispatcherExists(driver.Username) &&
                !DataRepository._driverRepo.CheckIfDriverExists(driver.Username))
            {
                driver.Id = Guid.NewGuid();
                driver.Role = Enums.Roles.Driver;
                driver.Password = ServiceSecurity.EncryptData(driver.Password, "password");
                driver.Ocuppied = false;
                driver.Location = new Location { Address = "garage", X = 0, Y = 0 };
                DataRepository._driverRepo.NewDriver(driver);
                return Request.CreateResponse(HttpStatusCode.Created, DataRepository._driverRepo.RetriveDriverById(driver.Id));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPut]
        [Route("api/Dispatcher/UpdateDispatcher")]
        [BasicAuthentication]
        public HttpResponseMessage UpdateDispatcher([FromBody]Dispatcher dispatcher)
        {
            if (DataRepository._customerRepo.CheckIfCustomerExists(dispatcher.Username) ||
                DataRepository._dispatcherRepo.CheckIfDispatcherExists(dispatcher.Username) ||
                DataRepository._driverRepo.CheckIfDriverExists(dispatcher.Username))
            {
                dispatcher.Role = Enums.Roles.Dispatcher;
                dispatcher.Password = ServiceSecurity.EncryptData(dispatcher.Password, "password");
                DataRepository._dispatcherRepo.EditDispatcherProfile(dispatcher);
                return Request.CreateResponse(HttpStatusCode.OK, DataRepository._dispatcherRepo.RetriveDispatcherById(dispatcher.Id));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        [HttpPost]
        [Route("api/Dispatcher/CreateDrive")]
        [BasicAuthentication]
        public HttpResponseMessage CreateDrive([FromBody]Drive drive)
        {
            drive.DriveId = Guid.NewGuid();
            drive.Date = DateTime.Now;
            drive.State = Enums.Status.Processed;
            DataRepository._driveRepo.AddNewDriveDispatcher(drive);
            return Request.CreateResponse(HttpStatusCode.Created, DataRepository._driveRepo.GetAllDrives());
        }
    }
}
