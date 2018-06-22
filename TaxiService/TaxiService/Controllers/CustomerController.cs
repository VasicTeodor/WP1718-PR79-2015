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
        [BasicAuthentication]
        public HttpResponseMessage CreateNewDrive([FromBody]Drive drive)
        {
            drive.DriveId = Guid.NewGuid();
            drive.State = Enums.Status.Created;
            drive.Date = DateTime.Now;
            drive.Destination = new Location
            {
                Address = "Unset",
                X = 0,
                Y = 0
            };
            drive.Price = 0;
            DataRepository._driveRepo.AddNewDriveCustomer(drive);
            return Request.CreateResponse(HttpStatusCode.Created, DataRepository._driveRepo.RetriveDriveById(drive.DriveId));
        }

        [HttpPut]
        [Route("api/Customer/QuitDrive")]
        [BasicAuthentication]
        public HttpResponseMessage QuitDrive([FromBody]JToken jToken)
        {
            string id = jToken.Value<string>("quitId");
            Guid driveId = Guid.Parse(id);
            Drive quit = null;
            quit = DataRepository._driveRepo.RetriveDriveById(driveId);

            if (quit != null)
            {
                quit.State = Enums.Status.Canceled;

                DataRepository._driveRepo.UpdateState(quit);

                return Request.CreateResponse(HttpStatusCode.OK, quit);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }

        [HttpPut]
        [Route("api/Customer/UpdateDrive")]
        [BasicAuthentication]
        public HttpResponseMessage UpdateDrive([FromBody]Drive drive)
        {
            Drive old = null; 
            old = DataRepository._driveRepo.RetriveDriveById(drive.DriveId);

            if (old != null)
            {
                old.Address = drive.Address;
                old.CarType = drive.CarType;
                DataRepository._driveRepo.CustomerEditDrive(old);
                return Request.CreateResponse(HttpStatusCode.OK, old);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }

        [HttpPut]
        [Route("api/Customer/AddComment")]
        [BasicAuthentication]
        public HttpResponseMessage AddComment([FromBody]JToken jToken)
        {
            string id = jToken.Value<string>("driveId");
            Guid driveId = Guid.Parse(id);
            string customerId = jToken.Value<string>("orderedBy");
            Guid orderedBy = Guid.Parse(customerId);
            
            Comment comment = new Comment();
            comment.CommentId = Guid.NewGuid();
            comment.CreatedOn = DateTime.Now;
            comment.Description = jToken.Value<string>("text");
            comment.Grade = Int32.Parse(jToken.Value<string>("grade"));
            Drive update = null;
            Customer customer = null;

            update = DataRepository._driveRepo.RetriveDriveById(driveId);
            customer = DataRepository._customerRepo.RetriveCustomerById(orderedBy);

            if (update != null && customer != null)
            {
                comment.CreatedBy = customer;
                comment.CommentedOn = update;
                update.Comments = comment;
                DataRepository._commentRepo.AddNewComment(comment);
                DataRepository._driveRepo.AddComment(update, update.Comments.CommentId);
                return Request.CreateResponse(HttpStatusCode.OK, update);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }
    }
}
