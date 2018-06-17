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
    public class LoginController : ApiController
    {
        [HttpPost]
        [Route("api/Login/SignIn")]
        [AllowAnonymous]
        public HttpResponseMessage SignIn([FromBody]LoginClass login)
        {
            if (DataRepository._driverRepo.LogIn(login.Username, ServiceSecurity.EncryptData(login.Password, "password")))
            {
                Driver driver = DataRepository._driverRepo.RetriveDriverByUserName(login.Username);
                return Request.CreateResponse(HttpStatusCode.OK, driver);
            }
            else if(DataRepository._dispatcherRepo.LogIn(login.Username, ServiceSecurity.EncryptData(login.Password,"password")))
            {
                Dispatcher dispatcher = DataRepository._dispatcherRepo.RetriveDispatcherByUserName(login.Username);
                return Request.CreateResponse(HttpStatusCode.OK, dispatcher);
            }
            else if(DataRepository._customerRepo.LogIn(login.Username, ServiceSecurity.EncryptData(login.Password, "password")))
            {
                Customer customer = DataRepository._customerRepo.RetriveCustomerByUserName(login.Username);
                customer.Drives = (List<Drive>)DataRepository._driveRepo.GetAllDrivesForCustomerId(customer.Id);
                return Request.CreateResponse(HttpStatusCode.OK, customer);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }
    }
}
