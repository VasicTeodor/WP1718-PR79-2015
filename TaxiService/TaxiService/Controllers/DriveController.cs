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
        //private static List<Drive> sortDrives = new List<Drive>();

        [HttpGet]
        [Route("api/Drive/GetDrive")]
        public HttpResponseMessage QuitDrive([FromUri]string id)
        {
            Guid driveId = Guid.Parse(id);
            Drive drive = null;
            drive = DataRepository._driveRepo.RetriveDriveById(driveId);
            if (drive != null)
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
                if (userRole == Enums.Roles.Driver)
                {
                    Driver driver = DataRepository._driverRepo.RetriveDriverById(userId);
                    allDrives.RemoveAll(d => (d.State != Enums.Status.Created) || (d.CarType != driver.Car.Type && d.CarType != Enums.CarTypes.Bez_Naznake));
                }

                if (userRole == Enums.Roles.Customer)
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

                if (userRole == Enums.Roles.Dispatcher)
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

            Enums.Roles role = (Enums.Roles)Enum.Parse(typeof(Enums.Roles), jToken.Value<string>("userRole"));

            if (allDrives != null)
            {
                List<Drive> resultDrives = new List<Drive>();

                if (role == Enums.Roles.Customer)
                {
                    resultDrives = DataRepository._driveRepo.GetAllDrivesForCustomerId(user).ToList(); ;
                }
                else if (role != Enums.Roles.Customer && jToken.Value<string>("whatDrives").Equals("My"))
                {
                    if (role == Enums.Roles.Dispatcher)
                    {
                        allDrives.RemoveAll(d => (d.ApprovedBy == null) || (d.ApprovedBy.Id != user));
                        resultDrives = allDrives;
                    }
                    else
                    {
                        allDrives.RemoveAll(d => (d.DrivedBy == null) || (d.DrivedBy.Id != user));
                        resultDrives = allDrives;
                    }
                }
                else
                {
                    if (role == Enums.Roles.Dispatcher)
                    {
                        resultDrives = allDrives;
                    }
                    else
                    {
                        allDrives.RemoveAll(d => d.State != Enums.Status.Created);
                        resultDrives = allDrives;
                    }
                }
                List<Drive> filtered = new List<Drive>();

                string sortBy = jToken.Value<string>("sortBy");

                if (!sortBy.Equals("None"))
                {
                    if (sortBy.Equals("Date"))
                    {
                        resultDrives.Sort((d1, d2) => DateTime.Compare(d2.Date, d1.Date));
                    }
                    else if (sortBy.Equals("Grade"))
                    {
                        resultDrives.RemoveAll(x => x.Comments == null);

                        resultDrives.Sort((d1, d2) => d2.Comments.Grade.CompareTo(d1.Comments.Grade));
                    }
                }

                if (!jToken.Value<string>("filterBy").Equals("State"))
                {
                    Enums.Status state = (Enums.Status)Enum.Parse(typeof(Enums.Status), jToken.Value<string>("filterBy"));
                    resultDrives.RemoveAll(x => x.State != state);
                }

                if (!String.IsNullOrEmpty(jToken.Value<string>("fromDate")) || !String.IsNullOrEmpty(jToken.Value<string>("toDate")))
                {
                    if (!String.IsNullOrEmpty(jToken.Value<string>("fromDate")))
                    {
                        DateTime dateFrom = DateTime.Parse(jToken.Value<string>("fromDate"));

                        if (!String.IsNullOrEmpty(jToken.Value<string>("toDate")))
                        {
                            DateTime dateTo = DateTime.Parse(jToken.Value<string>("toDate"));

                            resultDrives.RemoveAll(d => (d.Date.Date < dateFrom.Date) || (d.Date.Date > dateTo.Date));
                        }

                        resultDrives.RemoveAll(d => d.Date.Date < dateFrom.Date);
                    }
                    else
                    {
                        DateTime dateTo = DateTime.Parse(jToken.Value<string>("toDate"));

                        resultDrives.RemoveAll(d => d.Date.Date > dateTo.Date);
                    }
                }

                if (!String.IsNullOrEmpty(jToken.Value<string>("gradeFrom")) || !String.IsNullOrEmpty(jToken.Value<string>("gradeTo")))
                {

                    if (!String.IsNullOrEmpty(jToken.Value<string>("gradeFrom")) && !jToken.Value<string>("gradeFrom").Equals("Grade"))
                    {
                        resultDrives.RemoveAll(x => x.Comments == null);

                        int gradeFrom = Int32.Parse(jToken.Value<string>("gradeFrom"));

                        if (!String.IsNullOrEmpty(jToken.Value<string>("gradeTo")) && !jToken.Value<string>("gradeTo").Equals("Grade"))
                        {
                            int gradeTo = Int32.Parse(jToken.Value<string>("gradeTo"));

                            resultDrives.RemoveAll(d => (d.Comments.Grade < gradeFrom) || (d.Comments.Grade > gradeTo));
                        }

                        resultDrives.RemoveAll(d => d.Comments.Grade < gradeFrom);

                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(jToken.Value<string>("gradeTo")) && !jToken.Value<string>("gradeTo").Equals("Grade"))
                        {
                            resultDrives.RemoveAll(x => x.Comments == null);
                            int gradeTo = Int32.Parse(jToken.Value<string>("gradeTo"));

                            resultDrives.RemoveAll(d => d.Comments.Grade > gradeTo);
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

                            resultDrives.RemoveAll(d => (d.Price < priceFrom) || (d.Price > priceTo));
                        }

                        resultDrives.RemoveAll(d => d.Price < priceFrom);

                    }
                    else
                    {
                        double priceTo = Double.Parse(jToken.Value<string>("priceTo"));

                        resultDrives.RemoveAll(d => d.Price > priceTo);
                    }
                }

                filteredDrives = resultDrives;

                if (role == Enums.Roles.Dispatcher)
                {
                    if (jToken.Value<string>("searchRole").Equals("Customer"))
                    {
                        if (!String.IsNullOrEmpty(jToken.Value<string>("filterName")) || !String.IsNullOrEmpty(jToken.Value<string>("filterSurname")))
                        {
                            resultDrives.RemoveAll(d => d.OrderedBy == null);
                            if (!String.IsNullOrEmpty(jToken.Value<string>("filterName")))
                            {
                                string name = jToken.Value<string>("filterName");

                                if (!String.IsNullOrEmpty(jToken.Value<string>("filterSurname")))
                                {
                                    string surname = jToken.Value<string>("filterSurname");

                                    resultDrives.RemoveAll(d => (!d.OrderedBy.Name.ToLower().Contains(name.ToLower())) || (!d.OrderedBy.Surname.ToLower().Contains(surname.ToLower())));
                                }

                                resultDrives.RemoveAll(d => !d.OrderedBy.Name.ToLower().Contains(name.ToLower()));

                            }
                            else
                            {
                                string surname = jToken.Value<string>("filterSurname");

                                resultDrives.RemoveAll(d => !d.OrderedBy.Surname.ToLower().Contains(surname.ToLower()));
                            }
                        }
                        filteredDrives = resultDrives;
                    }
                    else if (jToken.Value<string>("searchRole").Equals("Driver"))
                    {
                        if (!String.IsNullOrEmpty(jToken.Value<string>("filterName")) || !String.IsNullOrEmpty(jToken.Value<string>("filterSurname")))
                        {
                            resultDrives.RemoveAll(d => d.DrivedBy == null);
                            if (!String.IsNullOrEmpty(jToken.Value<string>("filterName")))
                            {
                                string name = jToken.Value<string>("filterName");

                                if (!String.IsNullOrEmpty(jToken.Value<string>("filterSurname")))
                                {
                                    string surname = jToken.Value<string>("filterSurname");

                                    resultDrives.RemoveAll(d => (!d.DrivedBy.Name.ToLower().Contains(name.ToLower())) || (!d.DrivedBy.Surname.ToLower().Contains(surname.ToLower())));
                                }

                                resultDrives.RemoveAll(d => !d.DrivedBy.Name.ToLower().Contains(name.ToLower()));

                            }
                            else
                            {
                                string surname = jToken.Value<string>("filterSurname");

                                resultDrives.RemoveAll(d => !d.DrivedBy.Surname.ToLower().Contains(surname.ToLower()));
                            }
                        }
                        filteredDrives = resultDrives;
                    }
                }

                if(role == Enums.Roles.Driver)
                {
                    if (jToken.Value<string>("drivesNearMe").Equals("Yes"))
                    {
                        Driver driver = DataRepository._driverRepo.RetriveDriverById(user);
                        resultDrives.RemoveAll(d => (d.DrivedBy == null) || (d.CarType != driver.Car.Type));
                        resultDrives.Sort(delegate (Drive d1, Drive d2)
                        {
                            if (CalculateLength(driver.Location.X, driver.Location.Y, d1.Address.X, d1.Address.Y) < CalculateLength(driver.Location.X, driver.Location.Y, d2.Address.X, d2.Address.Y))
                            {
                                return -1;
                            }
                            else
                            {
                                return 1;
                            }
                        });
                    }
                    filteredDrives = resultDrives;
                }
                return Request.CreateResponse(HttpStatusCode.OK, filteredDrives);
            }
            else
            {
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }

        //private void BubbleSort(List<Drive> data, Driver driver)
        //{
        //    int i, j;
        //    int N = data.Count;

        //    for (j = N - 1; j > 0; j--)
        //    {
        //        for (i = 0; i < j; i++)
        //        {
        //            if (CalculateLength(driver.Location.X,driver.Location.Y,data[i].Address.X,data[i].Address.Y) > CalculateLength(driver.Location.X, driver.Location.Y, data[i+1].Address.X, data[i+1].Address.Y))
        //                Exchange(data, i, i + 1);
        //        }
        //    }
        //}

        //private static void Exchange(List<Drive> drives, int m, int n)
        //{
        //    Drive temporary;

        //    temporary = drives[m];
        //    drives[m] = drives[n];
        //    drives[n] = temporary;
        //}

        private double CalculateLength(double x1, double y1, double x2, double y2)
        {
            return Math.Sqrt(Math.Pow((x1 - x2), 2) + Math.Pow((y1 - y2), 2));
        }
    }
}
