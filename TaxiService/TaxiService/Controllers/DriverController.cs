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
    }
}
