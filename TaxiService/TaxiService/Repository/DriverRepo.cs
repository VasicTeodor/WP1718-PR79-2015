﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using TaxiService.Interfaces;
using TaxiService.Models;
using static TaxiService.Models.Enums;

namespace TaxiService.Repository
{
    public class DriverRepo : IDriverRepo
    {
        private string fileName = HttpContext.Current.Server.MapPath("~/App_Data/Drivers.xml");
        //D:\WebProjekat\WP1718-PR79-2015\TaxiService\TaxiService\App_Data/

        public bool CheckIfDriverExists(string username)
        {
            if (File.Exists(fileName))
            {
                XDocument xmlDocument = XDocument.Load(fileName);

                bool retVal = (from driver in xmlDocument.Root.Elements("Driver")
                               where driver.Element("Username").Value.ToString().ToLower().Equals(username.ToLower())
                               select driver).Any();

                return retVal;
            }
            else
            {
                return false;
            }
        }

        public void EditDriverProfile(Driver driver)
        {
            if (File.Exists(fileName))
            {
                XDocument xmlDocument = XDocument.Load(fileName);

                xmlDocument.Element("Drivers")
                                        .Elements("Driver")
                                        .Where(x => x.Attribute("Id").Value == driver.Id.ToString()).FirstOrDefault()
                                        .SetElementValue("Username", driver.Username);
                xmlDocument.Element("Drivers")
                                        .Elements("Driver")
                                        .Where(x => x.Attribute("Id").Value == driver.Id.ToString()).FirstOrDefault()
                                        .SetElementValue("Password", driver.Password);
                xmlDocument.Element("Drivers")
                                        .Elements("Driver")
                                        .Where(x => x.Attribute("Id").Value == driver.Id.ToString()).FirstOrDefault()
                                        .SetElementValue("Name", driver.Name);
                xmlDocument.Element("Drivers")
                                        .Elements("Driver")
                                        .Where(x => x.Attribute("Id").Value == driver.Id.ToString()).FirstOrDefault()
                                        .SetElementValue("Surname", driver.Surname);
                xmlDocument.Element("Drivers")
                                        .Elements("Driver")
                                        .Where(x => x.Attribute("Id").Value == driver.Id.ToString()).FirstOrDefault()
                                        .SetElementValue("Jmbg", driver.Jmbg);
                xmlDocument.Element("Drivers")
                                        .Elements("Driver")
                                        .Where(x => x.Attribute("Id").Value == driver.Id.ToString()).FirstOrDefault()
                                        .SetElementValue("Phone", driver.Phone);
                xmlDocument.Element("Drivers")
                                        .Elements("Driver")
                                        .Where(x => x.Attribute("Id").Value == driver.Id.ToString()).FirstOrDefault()
                                        .SetElementValue("Email", driver.Email);
                xmlDocument.Element("Drivers")
                                        .Elements("Driver")
                                        .Where(x => x.Attribute("Id").Value == driver.Id.ToString()).FirstOrDefault()
                                        .SetElementValue("Gender", driver.Gender);
                xmlDocument.Element("Drivers")
                                       .Elements("Driver")
                                       .Where(x => x.Attribute("Id").Value == driver.Id.ToString()).FirstOrDefault()
                                       .SetElementValue("CarId", driver.Car.CarId);
                xmlDocument.Element("Drivers")
                                       .Elements("Driver")
                                       .Where(x => x.Attribute("Id").Value == driver.Id.ToString()).FirstOrDefault()
                                       .SetElementValue("ModelYear", driver.Car.ModelYear);
                xmlDocument.Element("Drivers")
                                       .Elements("Driver")
                                       .Where(x => x.Attribute("Id").Value == driver.Id.ToString()).FirstOrDefault()
                                       .SetElementValue("RegNumber", driver.Car.RegNumber);
                xmlDocument.Element("Drivers")
                                       .Elements("Driver")
                                       .Where(x => x.Attribute("Id").Value == driver.Id.ToString()).FirstOrDefault()
                                       .SetElementValue("Type", driver.Car.Type);

                xmlDocument.Save(fileName);
            }
        }

        public void ChangeDriverLocation(Driver driver)
        {
            if (File.Exists(fileName))
            {
                XDocument xmlDocument = XDocument.Load(fileName);

                xmlDocument.Element("Drivers")
                                        .Elements("Driver")
                                        .Where(x => x.Attribute("Id").Value == driver.Id.ToString()).FirstOrDefault()
                                        .SetElementValue("X", driver.Location.X);
                xmlDocument.Element("Drivers")
                                        .Elements("Driver")
                                        .Where(x => x.Attribute("Id").Value == driver.Id.ToString()).FirstOrDefault()
                                        .SetElementValue("Y", driver.Location.Y);
                xmlDocument.Element("Drivers")
                                        .Elements("Driver")
                                        .Where(x => x.Attribute("Id").Value == driver.Id.ToString()).FirstOrDefault()
                                        .SetElementValue("Address", driver.Location.Address);


                xmlDocument.Save(fileName);
            }
        }

        public void BannDriver(Guid id, bool ban)
        {
            if (File.Exists(fileName))
            {
                XDocument xmlDocument = XDocument.Load(fileName);

                xmlDocument.Element("Drivers")
                                        .Elements("Driver")
                                        .Where(x => x.Attribute("Id").Value == id.ToString()).FirstOrDefault()
                                        .SetElementValue("IsBanned", ban);

                xmlDocument.Save(fileName);
            }
        }

        public void DriverOccupation(Driver driver)
        {
            if (File.Exists(fileName))
            {
                XDocument xmlDocument = XDocument.Load(fileName);

                xmlDocument.Element("Drivers")
                                        .Elements("Driver")
                                        .Where(x => x.Attribute("Id").Value == driver.Id.ToString()).FirstOrDefault()
                                        .SetElementValue("Occupied", driver.Occupied);

                xmlDocument.Save(fileName);
            }
        }

        public bool LogIn(string username, string password)
        {
            if (File.Exists(fileName))
            {
                XDocument xmlDocument = XDocument.Load(fileName);

                bool retVal = (from driver in xmlDocument.Root.Elements("Driver")
                               where driver.Element("Username").Value.ToString().ToLower().Equals(username.ToLower())
                               where driver.Element("Password").Value.ToString() == password
                               select driver).Any();

                return retVal;
            }
            else
            {
                return false;
            }
        }

        public void NewDriver(Driver driver)
        {
            if (!File.Exists(fileName))
            {
                XDocument xmlDocument = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),

                new XElement("Drivers",
                new XElement("Driver", new XAttribute("Id", driver.Id),
                new XElement("Id", driver.Id),
                new XElement("Username", driver.Username),
                new XElement("Password", driver.Password),
                new XElement("Name", driver.Name),
                new XElement("Surname", driver.Surname),
                new XElement("Jmbg", driver.Jmbg),
                new XElement("Phone", driver.Phone),
                new XElement("Email", driver.Email),
                new XElement("Gender", driver.Gender),
                new XElement("IsBanned", driver.IsBanned),
                new XElement("Role", driver.Role),
                    new XElement("Occupied", driver.Occupied),
                    new XElement("X", driver.Location.X),
                    new XElement("Y", driver.Location.Y),
                    new XElement("Address", driver.Location.Address),
                    new XElement("CarId", driver.Car.CarId),
                    new XElement("ModelYear", driver.Car.ModelYear),
                    new XElement("RegNumber", driver.Car.RegNumber),
                    new XElement("Type", driver.Car.Type)
                )
                ));

                xmlDocument.Save(fileName);
            }
            else
            {
                try
                {
                    FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                    XDocument doc = XDocument.Load(stream);
                    XElement drivers = doc.Element("Drivers");
                    drivers.Add(new XElement("Driver", new XAttribute("Id", driver.Id),
                                  new XElement("Id", driver.Id),
                                  new XElement("Username", driver.Username),
                                  new XElement("Password", driver.Password),
                                  new XElement("Name", driver.Name),
                                  new XElement("Surname", driver.Surname),
                                  new XElement("Jmbg", driver.Jmbg),
                                  new XElement("Phone", driver.Phone),
                                  new XElement("Email", driver.Email),
                                  new XElement("Gender", driver.Gender),
                                  new XElement("IsBanned", driver.IsBanned),
                                  new XElement("Role", driver.Role),
                                  new XElement("Occupied", driver.Occupied),
                                  new XElement("X", driver.Location.X),
                                  new XElement("Y", driver.Location.Y),
                                  new XElement("Address", driver.Location.Address),
                                  new XElement("CarId", driver.Car.CarId),
                                  new XElement("ModelYear", driver.Car.ModelYear),
                                  new XElement("RegNumber", driver.Car.RegNumber),
                                  new XElement("Type", driver.Car.Type)
                                    ));
                    doc.Save(fileName);
                }
                catch { }
            }
        }

        public IEnumerable<Driver> RetriveAllDrivers()
        {
            if (File.Exists(fileName))
            {
                FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                XDocument doc = XDocument.Load(stream);
                IEnumerable<Driver> drivers =
                    doc.Root
                    .Elements("Driver")
                    .Select(driver => new Driver
                    {
                        Id = Guid.Parse(driver.Element("Id").Value),
                        Username = driver.Element("Username").Value,
                        Password = driver.Element("Password").Value,
                        Name = driver.Element("Name").Value,
                        Surname = driver.Element("Surname").Value,
                        Jmbg = driver.Element("Jmbg").Value,
                        Email = driver.Element("Email").Value,
                        Phone = driver.Element("Phone").Value,
                        Gender = (Genders)Enum.Parse(typeof(Genders), driver.Element("Gender").Value),
                        Role = (Roles)Enum.Parse(typeof(Roles), driver.Element("Role").Value),
                        IsBanned = bool.Parse(driver.Element("IsBanned").Value),
                        Occupied = bool.Parse(driver.Element("Occupied").Value),
                        Location = new Location
                        {
                            Address = driver.Element("Address").Value,
                            X = (double)(driver.Element("X")),
                            Y = (double)(driver.Element("Y"))
                        },
                        Car = new Car
                        {
                            CarId = Int32.Parse(driver.Element("CarId").Value),
                            ModelYear = Int32.Parse(driver.Element("ModelYear").Value),
                            RegNumber = driver.Element("RegNumber").Value,
                            Type = (CarTypes)Enum.Parse(typeof(CarTypes), driver.Element("Type").Value)
                        }
                    }).ToList();

                return drivers;
            }
            else
            {
                return null;
            }
        }

        public Driver RetriveDriverById(Guid id)
        {
            if (File.Exists(fileName))
            {
                FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                XDocument doc = XDocument.Load(stream);
                IEnumerable<Driver> drivers =
                    doc.Root
                    .Elements("Driver")
                    .Where(x => x.Attribute("Id").Value == id.ToString())
                    .Select(driverx => new Driver
                    {
                        Id = Guid.Parse(driverx.Element("Id").Value),
                        Username = driverx.Element("Username").Value,
                        Password = driverx.Element("Password").Value,
                        Name = driverx.Element("Name").Value,
                        Surname = driverx.Element("Surname").Value,
                        Jmbg = driverx.Element("Jmbg").Value,
                        Email = driverx.Element("Email").Value,
                        Phone = driverx.Element("Phone").Value,
                        Gender = (Genders)Enum.Parse(typeof(Genders), driverx.Element("Gender").Value),
                        Role = (Roles)Enum.Parse(typeof(Roles), driverx.Element("Role").Value),
                        IsBanned = bool.Parse(driverx.Element("IsBanned").Value),
                        Occupied = bool.Parse(driverx.Element("Occupied").Value),
                        Location = new Location
                        {
                            Address = driverx.Element("Address").Value,
                            X = (double)(driverx.Element("X")),
                            Y = (double)(driverx.Element("Y"))
                        },
                        Car = new Car
                        {
                            CarId = Int32.Parse(driverx.Element("CarId").Value),
                            ModelYear = Int32.Parse(driverx.Element("ModelYear").Value),
                            RegNumber = driverx.Element("RegNumber").Value,
                            Type = (CarTypes)Enum.Parse(typeof(CarTypes), driverx.Element("Type").Value)
                        }
                    }).ToList();

                Driver driver = drivers.First(x => x.Id.Equals(id));

                return driver;
            }
            else
            {
                return null;
            }
        }

        public Driver RetriveDriverByUserName(string Name)
        {
            if (File.Exists(fileName))
            {
                FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                XDocument doc = XDocument.Load(stream);
                IEnumerable<Driver> drivers =
                    doc.Root
                    .Elements("Driver")
                    .Where(x => x.Element("Username").Value.ToLower() == Name.ToString().ToLower())
                    .Select(driverx => new Driver
                    {
                        Id = Guid.Parse(driverx.Element("Id").Value),
                        Username = driverx.Element("Username").Value,
                        Password = driverx.Element("Password").Value,
                        Name = driverx.Element("Name").Value,
                        Surname = driverx.Element("Surname").Value,
                        Jmbg = driverx.Element("Jmbg").Value,
                        Email = driverx.Element("Email").Value,
                        Phone = driverx.Element("Phone").Value,
                        Gender = (Genders)Enum.Parse(typeof(Genders), driverx.Element("Gender").Value),
                        Role = (Roles)Enum.Parse(typeof(Roles), driverx.Element("Role").Value),
                        IsBanned = bool.Parse(driverx.Element("IsBanned").Value),
                        Occupied = bool.Parse(driverx.Element("Occupied").Value),
                        Location = new Location
                        {
                            Address = driverx.Element("Address").Value,
                            X = (double)(driverx.Element("X")),
                            Y = (double)(driverx.Element("Y"))
                        },
                        Car = new Car
                        {
                            CarId = Int32.Parse(driverx.Element("CarId").Value),
                            ModelYear = Int32.Parse(driverx.Element("ModelYear").Value),
                            RegNumber = driverx.Element("RegNumber").Value,
                            Type = (CarTypes)Enum.Parse(typeof(CarTypes), driverx.Element("Type").Value)
                        }
                    }).ToList();

                Driver driver = drivers.First(x => x.Username.ToLower().Equals(Name.ToLower()));

                return driver;
            }
            else
            {
                return null;
            }
        }
    }
}