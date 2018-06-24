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
            if (DataRepository._driverRepo.LogIn(login.Username, ServiceSecurity.EncryptData(login.Password, "password")))
            {
                Driver driver = DataRepository._driverRepo.RetriveDriverByUserName(login.Username);

                if (!driver.IsBanned)
                {
                    LoginDto logObj = new LoginDto();
                    logObj.User = driver;
                    logObj.AccessToken = ServiceSecurity.MakeToken($"{login.Username}:{login.Password}");

                    List<Drive> allDrives = DataRepository._driveRepo.GetAllDrives().ToList();
                    logObj.User.Drives = allDrives.FindAll(x => (x.DrivedBy != null) && (x.DrivedBy.Id == logObj.User.Id));

                    return Request.CreateResponse(HttpStatusCode.OK, logObj);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
                
            }
            else if(DataRepository._dispatcherRepo.LogIn(login.Username, ServiceSecurity.EncryptData(login.Password,"password")))
            {
                LoginDto logObj = new LoginDto();
                logObj.User = DataRepository._dispatcherRepo.RetriveDispatcherByUserName(login.Username);
                logObj.AccessToken = ServiceSecurity.MakeToken($"{login.Username}:{login.Password}");

                List<Drive> allDrives = DataRepository._driveRepo.GetAllDrives().ToList();
                logObj.User.Drives = allDrives.FindAll(x => (x.ApprovedBy != null) && (x.ApprovedBy.Id == logObj.User.Id));

                return Request.CreateResponse(HttpStatusCode.OK, logObj);
            }
            else if(DataRepository._customerRepo.LogIn(login.Username, ServiceSecurity.EncryptData(login.Password, "password")))
            {
                Customer customer = DataRepository._customerRepo.RetriveCustomerByUserName(login.Username);

                if (!customer.IsBanned)
                {
                    LoginDto logObj = new LoginDto();
                    logObj.User = customer;
                    logObj.User.Drives = (List<Drive>)DataRepository._driveRepo.GetAllDrivesForCustomerId(logObj.User.Id);
                    logObj.AccessToken = ServiceSecurity.MakeToken($"{login.Username}:{login.Password}");
                    return Request.CreateResponse(HttpStatusCode.OK, logObj);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }
    }
}
