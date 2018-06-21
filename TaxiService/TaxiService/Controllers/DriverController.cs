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
    public class DriverController : ApiController
    {
        [HttpPut]
        [Route("api/Driver/UpdateDriver")]
        [BasicAuthentication]
        public HttpResponseMessage UpdateDriver([FromBody]Driver driver)
        {
            if (DataRepository._customerRepo.CheckIfCustomerExists(driver.Username) ||
                DataRepository._dispatcherRepo.CheckIfDispatcherExists(driver.Username) ||
                DataRepository._driverRepo.CheckIfDriverExists(driver.Username))
            {
                driver.Role = Enums.Roles.Driver;
                driver.Password = ServiceSecurity.EncryptData(driver.Password, "password");
                DataRepository._driverRepo.EditDriverProfile(driver);
                return Request.CreateResponse(HttpStatusCode.OK, DataRepository._driverRepo.RetriveDriverById(driver.Id));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        [Route("api/Driver/GetDriver")]
        [BasicAuthentication]
        public HttpResponseMessage GetDriver([FromUri] string id)
        {
            Guid chosenId = Guid.Parse(id);

            Driver driver = null;
            driver = DataRepository._driverRepo.RetriveDriverById(chosenId);

            if(driver != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, driver);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }

        [HttpGet]
        [Route("api/Driver/GetFreeDrivers")]
        [BasicAuthentication]
        public HttpResponseMessage GetFreeDrivers()
        {
            List<Driver> drivers = DataRepository._driverRepo.RetriveAllDrivers().ToList();
            List<Driver> freeDrivers = new List<Driver>();

            foreach(var d in drivers)
            {
                if(d.Occupied == false)
                {
                    freeDrivers.Add(d);
                }
            }

            return Request.CreateResponse(HttpStatusCode.OK, freeDrivers);
        }

        [HttpPut]
        [Route("api/Driver/AcceptDrive")]
        [BasicAuthentication]
        public HttpResponseMessage AcceptDrive([FromBody]JToken jToken)
        {
            string id = jToken.Value<string>("id");
            Guid driveId = Guid.Parse(id);
            string driverId = jToken.Value<string>("driverId");
            Guid drivenBy = Guid.Parse(driverId);

            Drive update = null;
            Driver driver = null;

            update = DataRepository._driveRepo.RetriveDriveById(driveId);
            driver = DataRepository._driverRepo.RetriveDriverById(drivenBy);

            if (update != null && driver != null)
            {
                update.DrivedBy = driver;
                update.State = Enums.Status.Accepted;
                driver.Occupied = true;
                DataRepository._driverRepo.DriverOccupation(driver);
                DataRepository._driveRepo.DriverEditDrive(update);
                return Request.CreateResponse(HttpStatusCode.OK, update);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }

        [HttpPut]
        [Route("api/Driver/UpdateDrive")]
        [BasicAuthentication]
        public HttpResponseMessage UpdateDrive([FromBody]JToken jToken)
        {
            string id = jToken.Value<string>("driveId");
            Guid driveId = Guid.Parse(id);
            string driverId = jToken.Value<string>("drivedBy");
            Guid drivenBy = Guid.Parse(driverId);

            Enums.Status state = (Enums.Status)Enum.Parse(typeof(Enums.Status), jToken.Value<string>("state"));
            Location location = new Location();
            location.Address = jToken.Value<string>("destination");
            location.X = Double.Parse(jToken.Value<string>("destinationX"));
            location.X = Double.Parse(jToken.Value<string>("destinationY"));
            double price = Double.Parse(jToken.Value<string>("price"));
            Drive update = null;
            Driver driver = null;

            update = DataRepository._driveRepo.RetriveDriveById(driveId);
            driver = DataRepository._driverRepo.RetriveDriverById(drivenBy);

            if (update != null && driver != null)
            {
                update.State = state;
                update.Destination = location;
                update.Price = price;
                driver.Occupied = false;
                DataRepository._driverRepo.DriverOccupation(driver);
                DataRepository._driveRepo.FinishDrive(update);
                return Request.CreateResponse(HttpStatusCode.OK, update);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }

        [HttpPut]
        [Route("api/Driver/FailedDrive")]
        [BasicAuthentication]
        public HttpResponseMessage FailedDrive([FromBody]JToken jToken)
        {
            string id = jToken.Value<string>("driveId");
            Guid driveId = Guid.Parse(id);
            string driverId = jToken.Value<string>("drivedBy");
            Guid drivenBy = Guid.Parse(driverId);

            Enums.Status state = (Enums.Status)Enum.Parse(typeof(Enums.Status), jToken.Value<string>("state"));
            Comment comment = new Comment();
            comment.CommentId = Guid.NewGuid();
            comment.CreatedOn = DateTime.Now;
            comment.Description = jToken.Value<string>("text");
            comment.Grade = Int32.Parse(jToken.Value<string>("grade"));
            Drive update = null;
            Driver driver = null;

            update = DataRepository._driveRepo.RetriveDriveById(driveId);
            driver = DataRepository._driverRepo.RetriveDriverById(drivenBy);

            if (update != null && driver != null)
            {
                comment.CreatedBy = driver;
                comment.CommentedOn = update;
                update.Comments = comment;
                update.State = state;
                driver.Occupied = false;
                DataRepository._driverRepo.DriverOccupation(driver);
                DataRepository._commentRepo.AddNewComment(comment);
                DataRepository._driveRepo.AddComment(update, update.Comments.CommentId);
                DataRepository._driveRepo.UpdateState(update);
                return Request.CreateResponse(HttpStatusCode.OK, update);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }
    }
}
