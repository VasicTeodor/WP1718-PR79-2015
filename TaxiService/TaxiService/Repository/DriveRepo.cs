using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using TaxiService.Interfaces;
using TaxiService.Models;

namespace TaxiService.Repository
{
    public class DriveRepo : IDriveRepo
    {
        private string fileName = HttpContext.Current.Server.MapPath("~/App_Data/Drives.xml");

        public void AddNewDrive(Drive drive)
        {
            if (!File.Exists(fileName))
            {
                XDocument xmlDocument = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),

                new XElement("Drives",
                new XElement("Drive", new XAttribute("Id", drive.Id),
                new XElement("Id", drive.Id),
                new XElement("CustomerId", drive.CustomerId),
                new XElement("Date", drive.Date),
                new XElement("CarType", drive.CarType),
                new XElement("Price", drive.Price),
                new XElement("State", drive.State),
                    new XElement("Location",
                    new XElement("X", drive.Address.X),
                    new XElement("Y", drive.Address.Y),
                    new XElement("Address", drive.Address.Address)),
                    new XElement("Location",
                    new XElement("X", drive.Destination.X),
                    new XElement("Y", drive.Destination.Y),
                    new XElement("Address", drive.Destination.Address))
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
                    XElement drivers = doc.Element("Drives");
                    drivers.Add(new XElement("Drive", new XAttribute("Id", drive.Id),
                                new XElement("Id", drive.Id),
                                new XElement("CustomerId", drive.CustomerId),
                                new XElement("Date", drive.Date),
                                new XElement("CarType", drive.CarType),
                                new XElement("Price", drive.Price),
                                new XElement("State", drive.State),
                                    new XElement("Location",
                                    new XElement("X", drive.Address.X),
                                    new XElement("Y", drive.Address.Y),
                                    new XElement("Address", drive.Address.Address)),
                                    new XElement("Location",
                                    new XElement("X", drive.Destination.X),
                                    new XElement("Y", drive.Destination.Y),
                                    new XElement("Address", drive.Destination.Address))
                                    ));
                    doc.Save(fileName);
                }
                catch { }
            }
        }

        public void EditDrive(Drive drive)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Drive> GetAllDrives()
        {
            throw new NotImplementedException();
        }

        public Drive RetriveDriveById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Drive RetriveDriveByUser(User user)
        {
            throw new NotImplementedException();
        }
    }
}