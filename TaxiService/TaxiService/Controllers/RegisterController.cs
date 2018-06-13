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
    public class RegisterController : ApiController
    {
        private DispatcherRepo _dispRepo = new DispatcherRepo();
        private DriverRepo _drivRepo = new DriverRepo();
        private CustomerRepo _custRepo = new CustomerRepo();

        [HttpPost]
        public HttpResponseMessage RegisterAccount([FromBody]Customer customer)
        {
            if(!_custRepo.CheckIfCustomerExists(customer.Username) && 
                !_dispRepo.CheckIfDispatcherExists(customer.Username) &&
                !_drivRepo.CheckIfDriverExists(customer.Username))
            {
                customer.Id = Guid.NewGuid();
                customer.Role = Enums.Roles.Customer;
                _custRepo.NewCustomer(customer);

                return Request.CreateResponse(HttpStatusCode.Created,_custRepo.RetriveCustomerById(customer.Id));
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
    }
}
