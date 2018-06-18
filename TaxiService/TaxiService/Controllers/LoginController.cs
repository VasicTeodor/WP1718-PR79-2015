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
    public class LoginController : ApiController
    {
        [HttpPost]
        [Route("api/Login/SignIn")]
        [AllowAnonymous]
        public HttpResponseMessage SignIn([FromBody]LoginClass login)
        {
            //LoginClass login = new LoginClass
            //{
            //    Username = jToken.Value<string>("username"),
            //    Password = jToken.Value<string>("password")
            //};

            if (DataRepository._driverRepo.LogIn(login.Username, ServiceSecurity.EncryptData(login.Password, "password")))
            {
                LoginDto logObj = new LoginDto();
                logObj.User = DataRepository._driverRepo.RetriveDriverByUserName(login.Username);
                logObj.AccessToken = ServiceSecurity.MakeToken($"{login.Username}:{login.Password}");
                return Request.CreateResponse(HttpStatusCode.OK, logObj);
            }
            else if(DataRepository._dispatcherRepo.LogIn(login.Username, ServiceSecurity.EncryptData(login.Password,"password")))
            {
                LoginDto logObj = new LoginDto();
                logObj.User = DataRepository._dispatcherRepo.RetriveDispatcherByUserName(login.Username);
                logObj.AccessToken = ServiceSecurity.MakeToken($"{login.Username}:{login.Password}");
                return Request.CreateResponse(HttpStatusCode.OK, logObj);
            }
            else if(DataRepository._customerRepo.LogIn(login.Username, ServiceSecurity.EncryptData(login.Password, "password")))
            {
                LoginDto logObj = new LoginDto();
                logObj.User = DataRepository._customerRepo.RetriveCustomerByUserName(login.Username);
                logObj.User.Drives = (List<Drive>)DataRepository._driveRepo.GetAllDrivesForCustomerId(logObj.User.Id);
                logObj.AccessToken = ServiceSecurity.MakeToken($"{login.Username}:{login.Password}");
                return Request.CreateResponse(HttpStatusCode.OK, logObj);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }
    }
}
