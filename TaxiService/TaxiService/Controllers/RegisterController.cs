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
                if (Validate(customer))
                {
                    customer.Id = Guid.NewGuid();
                    customer.Role = Enums.Roles.Customer;
                    customer.IsBanned = false;
                    LoginDto logObj = new LoginDto();
                    logObj.AccessToken = ServiceSecurity.MakeToken($"{customer.Username}:{customer.Password}");
                    customer.Password = ServiceSecurity.EncryptData(customer.Password, "password");
                    logObj.User = customer;
                    DataRepository._customerRepo.NewCustomer(customer);

                    return Request.CreateResponse(HttpStatusCode.Created, logObj);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);

                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
        }

        [HttpPost]
        [Route("api/Register/CheckUsername")]
        public HttpResponseMessage CheckUsername([FromBody]LoginClass customer)
        {
            if (!string.IsNullOrEmpty(customer.Username))
            {
                if (!DataRepository._customerRepo.CheckIfCustomerExists(customer.Username) &&
                   !DataRepository._dispatcherRepo.CheckIfDispatcherExists(customer.Username) &&
                   !DataRepository._driverRepo.CheckIfDriverExists(customer.Username))
                {
                    return Request.CreateResponse(HttpStatusCode.OK, true);
                }
                else
                {
                    return Request.CreateResponse(HttpStatusCode.NotAcceptable, false);
                }
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotAcceptable, false);
            }
        }

        private bool Validate(Customer customer)
        {
            if(String.IsNullOrEmpty(customer.Name) || customer.Name.Length < 4)
            {
                return false;
            }

            if(String.IsNullOrEmpty(customer.Surname) || customer.Surname.Length < 4)
            {
                return false;
            }

            if(!customer.Email.Contains('@') || !customer.Email.Contains('.'))
            {
                return false;
            }

            if(customer.Phone.ToString().Length < 6)
            {
                return false;
            }

            if(customer.Jmbg.ToString().Length < 13)
            {
                return false;
            }

            if (String.IsNullOrEmpty(customer.Username))
            {
                return false;
            }

            if (String.IsNullOrEmpty(customer.Password))
            {
                return false;
            }

            return true;
        }
    }
}
