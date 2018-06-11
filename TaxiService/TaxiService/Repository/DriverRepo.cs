using System;
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
        private string fileName = @"../App_Data/Driver.xml";

        public bool CheckIfDriverExists(string username)
        {
            XDocument xmlDocument = XDocument.Load(fileName);

            bool retVal = (from driver in xmlDocument.Root.Elements("Driver")
                           where driver.Element("Username").Value.ToString().ToLower().Equals(username.ToLower())
                           select driver).Any();

            return retVal;
        }

        public void EditDriverProfile(Driver driver)
        {
            XDocument xmlDocument = XDocument.Load(fileName);

            xmlDocument.Element("Drivers")
                                    .Elements("Driver")
                                    .Where(x => x.Attribute("Id").Value == driver.Id.ToString()).FirstOrDefault()
                                    .SetElementValue("Id", driver.Id);
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
                                    .SetElementValue("Role", driver.Role);

            xmlDocument.Save(fileName);
        }

        public bool LogIn(string username, string password)
        {
            XDocument xmlDocument = XDocument.Load(fileName);

            bool retVal = (from driver in xmlDocument.Root.Elements("Driver")
                           where driver.Element("Username").Value.ToString().ToLower().Equals(username.ToLower())
                           where driver.Element("Password").Value.ToString() == password
                           select driver).Any();

            return retVal;
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
                new XElement("Role", driver.Role))
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
                                  new XElement("Role", driver.Role)));
                    doc.Save(fileName);
                }
                catch { }
            }
        }

        public IEnumerable<Driver> RetriveAllDrivers()
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
                    Role = (Roles)Enum.Parse(typeof(Roles), driver.Element("Email").Value)
                }).ToList();

            return drivers;
        }

        public Driver RetriveDriverById(Guid id)
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
                    Role = (Roles)Enum.Parse(typeof(Roles), driverx.Element("Email").Value)
                }).ToList();

            Driver driver = drivers.First(x => x.Id.Equals(id));

            return driver;
        }

        public Driver RetriveDriverByUserName(string Name)
        {
            FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            XDocument doc = XDocument.Load(stream);
            IEnumerable<Driver> drivers =
                doc.Root
                .Elements("Driver")
                .Where(x => x.Attribute("Username").Value.ToLower() == Name.ToString().ToLower())
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
                    Role = (Roles)Enum.Parse(typeof(Roles), driverx.Element("Email").Value)
                }).ToList();

            Driver driver = drivers.First(x => x.Username.ToLower().Equals(Name.ToLower()));

            return driver;
        }
    }
}