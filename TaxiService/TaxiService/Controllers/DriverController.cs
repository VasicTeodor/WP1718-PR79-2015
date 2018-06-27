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
        public HttpResponseMessage GetFreeDrivers([FromUri]string type)
        {
            List<Driver> drivers = null;
            drivers = DataRepository._driverRepo.RetriveAllDrivers().ToList();
            
            if(drivers != null)
            {
                Enums.CarTypes car = (Enums.CarTypes)Enum.Parse(typeof(Enums.CarTypes), type);

                if(car != Enums.CarTypes.Bez_Naznake)
                {
                    drivers.RemoveAll(d => (d.Occupied == true) || (d.Car.Type != car));
                }
                else
                {
                    drivers.RemoveAll(d => d.Occupied == true);
                }

                return Request.CreateResponse(HttpStatusCode.OK, drivers);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }

        [HttpGet]
        [Route("api/Driver/GetFreeDriversByLen")]
        [BasicAuthentication]
        public HttpResponseMessage GetFreeDriversByLen([FromUri]string type, double x, double y)
        {
            List<Driver> drivers = null;
            drivers = DataRepository._driverRepo.RetriveAllDrivers().ToList();

            if (drivers != null)
            {
                Enums.CarTypes car = (Enums.CarTypes)Enum.Parse(typeof(Enums.CarTypes), type);

                if (car != Enums.CarTypes.Bez_Naznake)
                {
                    drivers.RemoveAll(d => (d.Occupied == true) || (d.Car.Type != car));
                    drivers.Sort(delegate (Driver d1, Driver d2)
                    {
                        if (CalculateLength(x, y, d1.Location.X, d1.Location.Y) < CalculateLength(x, y, d2.Location.X, d2.Location.Y))
                            return -1;
                        else
                            return 1;
                    });

                    if(drivers.Count > 5)
                    {
                        drivers.RemoveRange(5, drivers.Count - 5);
                    }
                }
                else
                {
                    drivers.RemoveAll(d => d.Occupied == true);
                    drivers.Sort(delegate (Driver d1, Driver d2)
                    {
                        if (CalculateLength(x, y, d1.Location.X, d1.Location.Y) < CalculateLength(x, y, d2.Location.X, d2.Location.Y))
                            return -1;
                        else
                            return 1;
                    });

                    if (drivers.Count > 5)
                    {
                        drivers.RemoveRange(5, drivers.Count - 5);
                    }
                }

                return Request.CreateResponse(HttpStatusCode.OK, drivers);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
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

            if (update != null && driver != null && driver.Occupied == false)
            {
                if(update.State == Enums.Status.Created)
                {
                    update.DrivedBy = driver;
                    update.State = Enums.Status.Accepted;
                    driver.Occupied = true;
                    DataRepository._driverRepo.DriverOccupation(driver);
                    DataRepository._driveRepo.DriverEditDrive(update);
                    return Request.CreateResponse(HttpStatusCode.OK, driver);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.Gone);
                }
                
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
            location.X = jToken.Value<double>("destinationX");
            location.Y = jToken.Value<double>("destinationY");
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
                return Request.CreateResponse(HttpStatusCode.OK, driver);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }

        [HttpPut]
        [Route("api/Driver/UpdateDriverLocation")]
        [BasicAuthentication]
        public HttpResponseMessage UpdateDriverLocation([FromBody]JToken jToken)
        {
            string driverId = jToken.Value<string>("drivedBy");
            Guid drivenBy = Guid.Parse(driverId);
            
            Location location = new Location();
            location.Address = jToken.Value<string>("address");
            location.X = jToken.Value<double>("addressX");
            location.Y = jToken.Value<double>("addressY");
            Driver driver = null;
            
            driver = DataRepository._driverRepo.RetriveDriverById(drivenBy);

            if (driver != null)
            {
                driver.Location = location;
                DataRepository._driverRepo.ChangeDriverLocation(driver);
                return Request.CreateResponse(HttpStatusCode.OK, driver);
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
            comment.Grade = jToken.Value<int>("grade");
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
                update.Comments.CommentedOn = null;
                DataRepository._driveRepo.AddComment(update, update.Comments.CommentId);
                DataRepository._driveRepo.UpdateState(update);
                return Request.CreateResponse(HttpStatusCode.OK, update);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }

        [HttpPut]
        [Route("api/Driver/BanDriver")]
        [BasicAuthentication]
        public HttpResponseMessage BanDriver([FromBody]JToken jToken)
        {
            string customerId = jToken.Value<string>("id");
            Guid customer = Guid.Empty;
            customer = Guid.Parse(customerId);

            bool ban = bool.Parse(jToken.Value<string>("ban"));

            if (customer != Guid.Empty)
            {
                DataRepository._driverRepo.BannDriver(customer, ban);
                return Request.CreateResponse(HttpStatusCode.OK, true);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }

        private double CalculateLength(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow((x1 - x2), 2) + Math.Pow((y1 - y2), 2));
        }
    }
}
