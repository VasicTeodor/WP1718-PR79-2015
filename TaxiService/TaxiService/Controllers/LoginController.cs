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
    public class LoginController : ApiController
    {
        private DispatcherRepo _dispRepo = new DispatcherRepo();
        private DriverRepo _drivRepo = new DriverRepo();
        private CustomerRepo _custRepo = new CustomerRepo();

        [HttpPost]
        //[Route("api/Login/SignIn")]
        public HttpResponseMessage SignIn([FromBody]LoginClass login)
        {
            if (_drivRepo.LogIn(login.Username, login.Password))
            {
                Driver driver = _drivRepo.RetriveDriverByUserName(login.Username);
                return Request.CreateResponse(HttpStatusCode.OK, driver);
            }
            else if(_dispRepo.LogIn(login.Username, login.Password))
            {
                Dispatcher dispatcher = _dispRepo.RetriveDispatcherByUserName(login.Username);
                return Request.CreateResponse(HttpStatusCode.OK, dispatcher);
            }
            else if(_custRepo.LogIn(login.Username, login.Password))
            {
                Customer customer = _custRepo.RetriveCustomerByUserName(login.Username);
                return Request.CreateResponse(HttpStatusCode.OK, customer);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }
    }
}
