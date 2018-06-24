using Newtonsoft.Json.Linq;
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
                driver.Occupied = false;
                driver.IsBanned = false;
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
            drive.State = Enums.Status.Formated;
            drive.Destination = new Location
            {
                Address = "Unset",
                X = 0,
                Y = 0
            };
            drive.DrivedBy.Occupied = true;
            DataRepository._driverRepo.DriverOccupation(drive.DrivedBy);
            DataRepository._driveRepo.AddNewDriveDispatcher(drive);
            return Request.CreateResponse(HttpStatusCode.Created, DataRepository._driveRepo.RetriveDriveById(drive.DriveId));
        }

        [HttpPut]
        [Route("api/Dispatcher/UpdateDrive")]
        [BasicAuthentication]
        public HttpResponseMessage UpdateDrive([FromBody]JToken jToken)
        {
            string id = jToken.Value<string>("driveId");
            Guid driveId = Guid.Parse(id);
            string driverId = jToken.Value<string>("drivedBy");
            Guid drivenBy = Guid.Parse(driverId);
            string approvedBy = jToken.Value<string>("approvedBy");
            Guid dispatcherId = Guid.Parse(approvedBy);
            Drive update = null;
            Driver driver = null;
            Dispatcher dispatcher = null;
            dispatcher = DataRepository._dispatcherRepo.RetriveDispatcherById(dispatcherId);
            update = DataRepository._driveRepo.RetriveDriveById(driveId);
            driver = DataRepository._driverRepo.RetriveDriverById(drivenBy);

            if (update != null && driver != null && dispatcher != null)
            {
                update.DrivedBy = driver;
                update.ApprovedBy = dispatcher;
                update.State = Enums.Status.Formated;
                driver.Occupied = true;
                DataRepository._driverRepo.DriverOccupation(driver);
                DataRepository._driveRepo.DispatcherEditDrive(update);
                return Request.CreateResponse(HttpStatusCode.OK, update);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }

        [HttpGet]
        [Route("api/Dispatcher/GetAllUsers")]
        [BasicAuthentication]
        public HttpResponseMessage GetAllUsers()
        {
            List<User> allUsers = null;
            allUsers = DataRepository._customerRepo.RetriveAllCustomers().ToList<User>();
            allUsers.AddRange(DataRepository._driverRepo.RetriveAllDrivers().ToList<User>());

            if(allUsers != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, allUsers);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }
    }
}
