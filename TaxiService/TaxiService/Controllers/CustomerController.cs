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
    public class CustomerController : ApiController
    {
        [HttpPut]
        [Route("api/Customer/UpdateCustomer")]
        [BasicAuthentication]
        public HttpResponseMessage UpdateCustomer([FromBody]Customer customer)
        {
            if (DataRepository._customerRepo.CheckIfCustomerExists(customer.Username) ||
                DataRepository._dispatcherRepo.CheckIfDispatcherExists(customer.Username) ||
                DataRepository._driverRepo.CheckIfDriverExists(customer.Username))
            {
                customer.Password = ServiceSecurity.EncryptData(customer.Password, "password");
                customer.Role = Enums.Roles.Customer;
                DataRepository._customerRepo.EditCustomerProfile(customer);

                return Request.CreateResponse(HttpStatusCode.OK, DataRepository._customerRepo.RetriveCustomerById(customer.Id));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

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
