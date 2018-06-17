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
    public class RegisterController : ApiController
    {
        [HttpPost]
        public HttpResponseMessage RegisterAccount([FromBody]Customer customer)
        {
            if(!DataRepository._customerRepo.CheckIfCustomerExists(customer.Username) && 
                !DataRepository._dispatcherRepo.CheckIfDispatcherExists(customer.Username) &&
                !DataRepository._driverRepo.CheckIfDriverExists(customer.Username))
            {
                customer.Id = Guid.NewGuid();
                customer.Password = ServiceSecurity.EncryptData(customer.Password, "password");
                customer.Role = Enums.Roles.Customer;
                DataRepository._customerRepo.NewCustomer(customer);

                return Request.CreateResponse(HttpStatusCode.Created, DataRepository._customerRepo.RetriveCustomerById(customer.Id));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("api/Register/CheckUsername")]
        public HttpResponseMessage CheckUsername([FromBody]LoginClass customer)
        {
            if (!DataRepository._customerRepo.CheckIfCustomerExists(customer.Username) &&
               !DataRepository._dispatcherRepo.CheckIfDispatcherExists(customer.Username) &&
               !DataRepository._driverRepo.CheckIfDriverExists(customer.Username))
            {
                return Request.CreateResponse(HttpStatusCode.OK);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotAcceptable);
            }
        }
    }
}
