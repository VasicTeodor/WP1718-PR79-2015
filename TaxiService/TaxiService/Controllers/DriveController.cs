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
    [BasicAuthentication]
    public class DriveController : ApiController
    {
        [HttpGet]
        [Route("api/Drive/GetDrive")]
        public HttpResponseMessage QuitDrive([FromUri]string id)
        {
            Guid driveId = Guid.Parse(id);
            Drive drive = null; 
            drive = DataRepository._driveRepo.RetriveDriveById(driveId);
            if(drive != null)
            {
                return Request.CreateResponse(HttpStatusCode.OK, drive);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.NotFound);
            }
        }

        [HttpGet]
        [Route("api/Drive/GetAllDrives")]
        public HttpResponseMessage GetAllDrives([FromUri]string id, string role)
        {
            Guid userId = Guid.Parse(id);
            Enums.Roles userRole = (Enums.Roles)Enum.Parse(typeof(Enums.Roles), role);

            List<Drive> allDrives = null;
            allDrives = DataRepository._driveRepo.GetAllDrives().ToList();
            if (allDrives != null)
            {
                if(userRole == Enums.Roles.Driver)
                {
                    Driver driver = DataRepository._driverRepo.RetriveDriverById(userId);
                    allDrives.RemoveAll(d => (d.State != Enums.Status.Created) || (d.CarType != driver.Car.Type));
                }

                if(userRole == Enums.Roles.Customer)
                {
                    allDrives.RemoveAll(d => d.OrderedBy.Id != userId);
                }
                return Request.CreateResponse(HttpStatusCode.OK, allDrives);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpGet]
        [Route("api/Drive/GetAllDrivesForId")]
        public HttpResponseMessage GetAllDrivesForId([FromUri]string id, string role)
        {
            Guid userId = Guid.Parse(id);
            Enums.Roles userRole = (Enums.Roles)Enum.Parse(typeof(Enums.Roles), role);

            List<Drive> allDrives = null;
            allDrives = DataRepository._driveRepo.GetAllDrives().ToList();
            if (allDrives != null)
            {
                if (userRole == Enums.Roles.Driver)
                {
                    allDrives.RemoveAll(d => (d.DrivedBy == null) || (d.DrivedBy.Id != userId));
                }

                if (userRole == Enums.Roles.Customer)
                {
                    allDrives.RemoveAll(d => (d.OrderedBy == null) || (d.OrderedBy.Id != userId));
                }

                if(userRole == Enums.Roles.Dispatcher)
                {
                    allDrives.RemoveAll(d => (d.ApprovedBy == null) || (d.ApprovedBy.Id != userId));
                }
                return Request.CreateResponse(HttpStatusCode.OK, allDrives);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        [HttpPost]
        [Route("api/Drive/Filters")]
        public HttpResponseMessage Filters([FromBody]JToken jToken)
        {
            List<Drive> filteredDrives = null;
            List<Drive> allDrives = null;
            allDrives = DataRepository._driveRepo.GetAllDrives().ToList();

            string userId = jToken.Value<string>("userId");
            Guid user = Guid.Parse(userId);

            List<Drive> userDrives = null;
            userDrives = DataRepository._driveRepo.GetAllDrivesForCustomerId(user).ToList();

            Enums.Roles role = (Enums.Roles)Enum.Parse(typeof(Enums.Roles), jToken.Value<string>("userRole"));

            if (allDrives != null && userDrives != null)
            {
                if(role == Enums.Roles.Customer)
                {
                    List<Drive> filtered = new List<Drive>();

                    string sortBy = jToken.Value<string>("sortBy");

                    if (!sortBy.Equals("None"))
                    {
                        if (sortBy.Equals("Date"))
                        {
                            userDrives.Sort((d1, d2) => DateTime.Compare(d1.Date, d2.Date));
                        }else if (sortBy.Equals("Grade"))
                        {
                            userDrives.RemoveAll(x => x.Comments == null);

                            userDrives.Sort((d1, d2) => d2.Comments.Grade.CompareTo(d1.Comments.Grade));
                        }
                    }

                    if (!jToken.Value<string>("filterBy").Equals("State"))
                    {
                        Enums.Status state = (Enums.Status)Enum.Parse(typeof(Enums.Status),jToken.Value<string>("filterBy"));
                        userDrives.RemoveAll(x => x.State != state);
                    }

                    if (!String.IsNullOrEmpty(jToken.Value<string>("fromDate")) || !String.IsNullOrEmpty(jToken.Value<string>("toDate")))
                    {
                        if (!String.IsNullOrEmpty(jToken.Value<string>("fromDate")))
                        {
                            DateTime dateFrom = DateTime.Parse(jToken.Value<string>("fromDate"));

                            if (!String.IsNullOrEmpty(jToken.Value<string>("toDate")))
                            {
                                DateTime dateTo = DateTime.Parse(jToken.Value<string>("toDate"));

                                userDrives.RemoveAll(d => (d.Date.Date < dateFrom.Date) || (d.Date.Date > dateTo.Date));
                            }

                            userDrives.RemoveAll(d => d.Date.Date < dateFrom.Date);
                        }
                        else
                        {
                            DateTime dateTo = DateTime.Parse(jToken.Value<string>("toDate"));

                            userDrives.RemoveAll(d => d.Date.Date > dateTo.Date);
                        }
                    }
                    
                    if (!String.IsNullOrEmpty(jToken.Value<string>("gradeFrom")) || !String.IsNullOrEmpty(jToken.Value<string>("gradeTo")))
                    {

                        if (!String.IsNullOrEmpty(jToken.Value<string>("gradeFrom")) && !jToken.Value<string>("gradeFrom").Equals("Grade"))
                        {
                            userDrives.RemoveAll(x => x.Comments == null);

                            int gradeFrom = Int32.Parse(jToken.Value<string>("gradeFrom"));

                            if (!String.IsNullOrEmpty(jToken.Value<string>("gradeTo")) && !jToken.Value<string>("gradeTo").Equals("Grade"))
                            {
                                int gradeTo = Int32.Parse(jToken.Value<string>("gradeTo"));

                                userDrives.RemoveAll(d => (d.Comments.Grade < gradeFrom) || (d.Comments.Grade > gradeTo));
                            }

                            userDrives.RemoveAll(d => d.Comments.Grade < gradeFrom);

                        }
                        else
                        {
                            if (!String.IsNullOrEmpty(jToken.Value<string>("gradeTo")) && !jToken.Value<string>("gradeTo").Equals("Grade"))
                            {
                                userDrives.RemoveAll(x => x.Comments == null);
                                int gradeTo = Int32.Parse(jToken.Value<string>("gradeTo"));

                                userDrives.RemoveAll(d => d.Comments.Grade > gradeTo);
                            }
                        }
                    }

                    if (!String.IsNullOrEmpty(jToken.Value<string>("priceFrom")) || !String.IsNullOrEmpty(jToken.Value<string>("priceTo")))
                    {
                        if (!String.IsNullOrEmpty(jToken.Value<string>("priceFrom")))
                        {
                            double priceFrom = Double.Parse(jToken.Value<string>("priceFrom"));

                            if (!String.IsNullOrEmpty(jToken.Value<string>("priceTo")))
                            {
                                double priceTo = Double.Parse(jToken.Value<string>("priceTo"));

                                userDrives.RemoveAll(d => (d.Price < priceFrom) || (d.Price > priceTo));
                            }

                            userDrives.RemoveAll(d => d.Price < priceFrom);

                        }
                        else
                        {
                            double priceTo = Double.Parse(jToken.Value<string>("priceTo"));

                            userDrives.RemoveAll(d => d.Price > priceTo);
                        }
                    }
                    
                    filteredDrives = userDrives;
                }

                if(role == Enums.Roles.Dispatcher)
                {
                    if (jToken.Value<string>("searchRole").Equals("Customer"))
                    {
                        allDrives.RemoveAll(d => d.OrderedBy == null);
                        if(!String.IsNullOrEmpty(jToken.Value<string>("filterName")) || !String.IsNullOrEmpty(jToken.Value<string>("filterSurname")))
                        {
                            if (!String.IsNullOrEmpty(jToken.Value<string>("filterName")))
                            {
                                string name = jToken.Value<string>("filterName");

                                if (!String.IsNullOrEmpty(jToken.Value<string>("filterSurname")))
                                {
                                    string surname = jToken.Value<string>("filterSurname");

                                    allDrives.RemoveAll(d => (!d.OrderedBy.Name.Contains(name)) && (!d.OrderedBy.Surname.Contains(surname)));
                                }

                                allDrives.RemoveAll(d => !d.OrderedBy.Name.Contains(name));

                            }
                            else
                            {
                                string surname = jToken.Value<string>("filterSurname");

                                allDrives.RemoveAll(d => !d.OrderedBy.Surname.Contains(surname));
                            }
                        }
                        filteredDrives = allDrives;
                    }
                    else if (jToken.Value<string>("searchRole").Equals("Driver"))
                    {
                        allDrives.RemoveAll(d => d.DrivedBy == null);
                        if (!String.IsNullOrEmpty(jToken.Value<string>("filterName")) || !String.IsNullOrEmpty(jToken.Value<string>("filterSurname")))
                        {
                            if (!String.IsNullOrEmpty(jToken.Value<string>("filterName")))
                            {
                                string name = jToken.Value<string>("filterName");

                                if (!String.IsNullOrEmpty(jToken.Value<string>("filterSurname")))
                                {
                                    string surname = jToken.Value<string>("filterSurname");

                                    allDrives.RemoveAll(d => (!d.DrivedBy.Name.Contains(name)) && (!d.DrivedBy.Surname.Contains(surname)));
                                }

                                allDrives.RemoveAll(d => !d.DrivedBy.Name.Contains(name));

                            }
                            else
                            {
                                string surname = jToken.Value<string>("filterSurname");

                                allDrives.RemoveAll(d => !d.DrivedBy.Surname.Contains(surname));
                            }
                        }
                        filteredDrives = allDrives;
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, filteredDrives);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }
    }
}
