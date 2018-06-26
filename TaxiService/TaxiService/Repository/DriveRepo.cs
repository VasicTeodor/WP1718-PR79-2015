using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using TaxiService.Interfaces;
using TaxiService.Models;
using static TaxiService.Models.Enums;

namespace TaxiService.Repository
{
    public class DriveRepo : IDriveRepo
    {
        private string fileName = HttpContext.Current.Server.MapPath("~/App_Data/Drives.xml");

        public void AddComment(Drive drive, Guid CommentId)
        {
            if (File.Exists(fileName))
            {
                XDocument xmlDocument = XDocument.Load(fileName);

                xmlDocument.Element("Drives")
                                        .Elements("Drive")
                                        .Where(x => x.Attribute("Id").Value == drive.DriveId.ToString()).FirstOrDefault()
                                        .SetElementValue("CommentId", CommentId);

                xmlDocument.Save(fileName);
            }
        }

        public void AddNewDriveCustomer(Drive drive)
        {
            if (!File.Exists(fileName))
            {
                XDocument xmlDocument = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),

                new XElement("Drives",
                new XElement("Drive", new XAttribute("Id", drive.DriveId),
                new XElement("Id", drive.DriveId),
                new XElement("CustomerId", drive.OrderedBy.Id),
                new XElement("DispatcherId", "00000000-0000-0000-0000-000000000000"),
                new XElement("DriverId", "00000000-0000-0000-0000-000000000000"),
                new XElement("CommentId", "00000000-0000-0000-0000-000000000000"),
                new XElement("Date", drive.Date),
                new XElement("CarType", drive.CarType),
                new XElement("Price", drive.Price),
                new XElement("State", drive.State),
                new XElement("AddressX", drive.Address.X),
                new XElement("AddressY", drive.Address.Y),
                new XElement("AddressA", drive.Address.Address),
                new XElement("DestinationX", drive.Destination.X),
                new XElement("DestinationY", drive.Destination.Y),
                new XElement("DestinationAddress", drive.Destination.Address)
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
                    drivers.Add(new XElement("Drive", new XAttribute("Id", drive.DriveId),
                                new XElement("Id", drive.DriveId),
                                new XElement("CustomerId", drive.OrderedBy.Id),
                                new XElement("DispatcherId", "00000000-0000-0000-0000-000000000000"),
                                new XElement("DriverId", "00000000-0000-0000-0000-000000000000"),
                                new XElement("CommentId", "00000000-0000-0000-0000-000000000000"),
                                new XElement("Date", drive.Date),
                                new XElement("CarType", drive.CarType),
                                new XElement("Price", drive.Price),
                                new XElement("State", drive.State),
                                new XElement("AddressX", drive.Address.X),
                                new XElement("AddressY", drive.Address.Y),
                                new XElement("AddressA", drive.Address.Address),
                                new XElement("DestinationX", drive.Destination.X),
                                new XElement("DestinationY", drive.Destination.Y),
                                new XElement("DestinationAddress", drive.Destination.Address)
                                    ));
                    doc.Save(fileName);
                }
                catch { }
            }
        }

        public void AddNewDriveDispatcher(Drive drive)
        {
            if (!File.Exists(fileName))
            {
                XDocument xmlDocument = new XDocument(
                new XDeclaration("1.0", "utf-8", "yes"),

                new XElement("Drives",
                new XElement("Drive", new XAttribute("Id", drive.DriveId),
                new XElement("Id", drive.DriveId),
                new XElement("CommentId", "00000000-0000-0000-0000-000000000000"),
                new XElement("CustomerId", "00000000-0000-0000-0000-000000000000"),
                new XElement("DispatcherId", drive.ApprovedBy.Id),
                new XElement("DriverId", drive.DrivedBy.Id),
                new XElement("Date", drive.Date),
                new XElement("CarType", drive.CarType),
                new XElement("Price", drive.Price),
                new XElement("State", drive.State),
                new XElement("AddressX", drive.Address.X),
                new XElement("AddressY", drive.Address.Y),
                new XElement("AddressA", drive.Address.Address),
                new XElement("DestinationX", drive.Destination.X),
                new XElement("DestinationY", drive.Destination.Y),
                new XElement("DestinationAddress", drive.Destination.Address)
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
                    drivers.Add(new XElement("Drive", new XAttribute("Id", drive.DriveId),
                                new XElement("Id", drive.DriveId),
                                new XElement("CommentId", "00000000-0000-0000-0000-000000000000"),
                                new XElement("CustomerId", "00000000-0000-0000-0000-000000000000"),
                                new XElement("DispatcherId", drive.ApprovedBy.Id),
                                new XElement("DriverId", drive.DrivedBy.Id),
                                new XElement("Date", drive.Date),
                                new XElement("CarType", drive.CarType),
                                new XElement("Price", drive.Price),
                                new XElement("State", drive.State),
                                new XElement("AddressX", drive.Address.X),
                                new XElement("AddressY", drive.Address.Y),
                                new XElement("AddressA", drive.Address.Address),
                                new XElement("DestinationX", drive.Destination.X),
                                new XElement("DestinationY", drive.Destination.Y),
                                new XElement("DestinationAddress", drive.Destination.Address)
                                    ));
                    doc.Save(fileName);
                }
                catch { }
            }
        }

        public void CustomerEditDrive(Drive drive)
        {
            if (File.Exists(fileName))
            {
                XDocument xmlDocument = XDocument.Load(fileName);

                xmlDocument.Element("Drives")
                                        .Elements("Drive")
                                        .Where(x => x.Attribute("Id").Value == drive.DriveId.ToString()).FirstOrDefault()
                                        .SetElementValue("AddressX", drive.Address.X);
                xmlDocument.Element("Drives")
                                        .Elements("Drive")
                                        .Where(x => x.Attribute("Id").Value == drive.DriveId.ToString()).FirstOrDefault()
                                        .SetElementValue("AddressY", drive.Address.Y);
                xmlDocument.Element("Drives")
                                        .Elements("Drive")
                                        .Where(x => x.Attribute("Id").Value == drive.DriveId.ToString()).FirstOrDefault()
                                        .SetElementValue("AddressA", drive.Address.Address);
                xmlDocument.Element("Drives")
                                        .Elements("Drive")
                                        .Where(x => x.Attribute("Id").Value == drive.DriveId.ToString()).FirstOrDefault()
                                        .SetElementValue("CarType", drive.CarType);

                xmlDocument.Save(fileName);
            }
        }

        public void DispatcherEditDrive(Drive drive)
        {
            if (File.Exists(fileName))
            {
                XDocument xmlDocument = XDocument.Load(fileName);

                xmlDocument.Element("Drives")
                                        .Elements("Drive")
                                        .Where(x => x.Attribute("Id").Value == drive.DriveId.ToString()).FirstOrDefault()
                                        .SetElementValue("DispatcherId", drive.ApprovedBy.Id);
                xmlDocument.Element("Drives")
                                        .Elements("Drive")
                                        .Where(x => x.Attribute("Id").Value == drive.DriveId.ToString()).FirstOrDefault()
                                        .SetElementValue("DriverId", drive.DrivedBy.Id);
                xmlDocument.Element("Drives")
                                       .Elements("Drive")
                                       .Where(x => x.Attribute("Id").Value == drive.DriveId.ToString()).FirstOrDefault()
                                       .SetElementValue("State", drive.State);

                xmlDocument.Save(fileName);
            }
        }

        public void DriverEditDrive(Drive drive)
        {
            if (File.Exists(fileName))
            {
                XDocument xmlDocument = XDocument.Load(fileName);

                xmlDocument.Element("Drives")
                                        .Elements("Drive")
                                        .Where(x => x.Attribute("Id").Value == drive.DriveId.ToString()).FirstOrDefault()
                                        .SetElementValue("DriverId", drive.DrivedBy.Id);
                xmlDocument.Element("Drives")
                                       .Elements("Drive")
                                       .Where(x => x.Attribute("Id").Value == drive.DriveId.ToString()).FirstOrDefault()
                                       .SetElementValue("State", drive.State);

                xmlDocument.Save(fileName);
            }
        }

        public void UpdateState(Drive drive)
        {
            if (File.Exists(fileName))
            {
                XDocument xmlDocument = XDocument.Load(fileName);

                xmlDocument.Element("Drives")
                                        .Elements("Drive")
                                        .Where(x => x.Attribute("Id").Value == drive.DriveId.ToString()).FirstOrDefault()
                                        .SetElementValue("State", drive.State);

                xmlDocument.Save(fileName);
            }
        }

        public void FinishDrive(Drive drive)
        {
            if (File.Exists(fileName))
            {
                XDocument xmlDocument = XDocument.Load(fileName);

                xmlDocument.Element("Drives")
                                        .Elements("Drive")
                                        .Where(x => x.Attribute("Id").Value == drive.DriveId.ToString()).FirstOrDefault()
                                        .SetElementValue("DestinationX", drive.Destination.X);
                xmlDocument.Element("Drives")
                                        .Elements("Drive")
                                        .Where(x => x.Attribute("Id").Value == drive.DriveId.ToString()).FirstOrDefault()
                                        .SetElementValue("DestinationY", drive.Destination.Y);
                xmlDocument.Element("Drives")
                                        .Elements("Drive")
                                        .Where(x => x.Attribute("Id").Value == drive.DriveId.ToString()).FirstOrDefault()
                                        .SetElementValue("DestinationAddress", drive.Destination.Address);
                xmlDocument.Element("Drives")
                                        .Elements("Drive")
                                        .Where(x => x.Attribute("Id").Value == drive.DriveId.ToString()).FirstOrDefault()
                                        .SetElementValue("Price", drive.Price);
                xmlDocument.Element("Drives")
                                        .Elements("Drive")
                                        .Where(x => x.Attribute("Id").Value == drive.DriveId.ToString()).FirstOrDefault()
                                        .SetElementValue("State", drive.State);

                xmlDocument.Save(fileName);
            }
        }

        public IEnumerable<Drive> GetAllDrives()
        {
            if (File.Exists(fileName))
            {
                List<Drive> fullDrives = new List<Drive>();
                FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                XDocument doc = XDocument.Load(stream);
                IEnumerable<DriveDto> drives =
                    doc.Root
                    .Elements("Drive")
                    .Select(drive => new DriveDto
                    {
                        DriveId = Guid.Parse(drive.Element("Id").Value),
                        CustomerId = Guid.Parse(drive.Element("CustomerId").Value),
                        DispatcherId = Guid.Parse(drive.Element("DispatcherId").Value),
                        DriverId = Guid.Parse(drive.Element("DriverId").Value),
                        CommentId = Guid.Parse(drive.Element("CommentId").Value),
                        Date = DateTime.Parse(drive.Element("Date").Value),
                        CarType = (CarTypes)Enum.Parse(typeof(CarTypes), drive.Element("CarType").Value),
                        Price = Double.Parse(drive.Element("Price").Value, CultureInfo.InvariantCulture),
                        State = (Status)Enum.Parse(typeof(Status), drive.Element("State").Value),
                        Address = new Location
                        {
                            Address = drive.Element("AddressA").Value,
                            X = (double)(drive.Element("AddressX")),
                            Y = (double)(drive.Element("AddressY")),
                        },
                        Destination = new Location
                        {
                            Address = drive.Element("DestinationAddress").Value,
                            X = (double)(drive.Element("DestinationX")),
                            Y = (double)(drive.Element("DestinationY")),
                        }
                    }).ToList();

                foreach (var d in drives)
                {

                    if (d.DispatcherId.Equals(Guid.Empty) && d.DriverId.Equals(Guid.Empty))
                    {
                        if (d.CommentId.Equals(Guid.Empty))
                        {
                            fullDrives.Add(new Drive
                            {
                                Address = d.Address,
                                CarType = d.CarType,
                                OrderedBy = DataRepository._customerRepo.RetriveCustomerById(d.CustomerId),
                                Date = d.Date,
                                Destination = d.Destination,
                                DriveId = d.DriveId,
                                Price = d.Price,
                                State = d.State
                            });
                        }
                        else
                        {
                            fullDrives.Add(new Drive
                            {
                                Address = d.Address,
                                CarType = d.CarType,
                                OrderedBy = DataRepository._customerRepo.RetriveCustomerById(d.CustomerId),
                                Date = d.Date,
                                Destination = d.Destination,
                                DriveId = d.DriveId,
                                Price = d.Price,
                                State = d.State,
                                Comments = DataRepository._commentRepo.GetCommentById(d.CommentId)
                            });
                        }
                    }
                    else if (d.CustomerId.Equals(Guid.Empty))
                    {
                        if (d.CommentId.Equals(Guid.Empty))
                        {
                            fullDrives.Add(new Drive
                            {
                                Address = d.Address,
                                CarType = d.CarType,
                                ApprovedBy = DataRepository._dispatcherRepo.RetriveDispatcherById(d.DispatcherId),
                                Date = d.Date,
                                Destination = d.Destination,
                                DrivedBy = DataRepository._driverRepo.RetriveDriverById(d.DriverId),
                                DriveId = d.DriveId,
                                Price = d.Price,
                                State = d.State
                            });
                        }
                        else
                        {
                            fullDrives.Add(new Drive
                            {
                                Address = d.Address,
                                CarType = d.CarType,
                                ApprovedBy = DataRepository._dispatcherRepo.RetriveDispatcherById(d.DispatcherId),
                                Date = d.Date,
                                Destination = d.Destination,
                                DrivedBy = DataRepository._driverRepo.RetriveDriverById(d.DriverId),
                                DriveId = d.DriveId,
                                Price = d.Price,
                                State = d.State,
                                Comments = DataRepository._commentRepo.GetCommentById(d.CommentId)
                            });
                        }
                    }
                    else if (d.DispatcherId.Equals(Guid.Empty))
                    {
                        if (d.CommentId.Equals(Guid.Empty))
                        {
                            fullDrives.Add(new Drive
                            {
                                Address = d.Address,
                                CarType = d.CarType,
                                OrderedBy = DataRepository._customerRepo.RetriveCustomerById(d.CustomerId),
                                Date = d.Date,
                                Destination = d.Destination,
                                DrivedBy = DataRepository._driverRepo.RetriveDriverById(d.DriverId),
                                DriveId = d.DriveId,
                                Price = d.Price,
                                State = d.State
                            });
                        }
                        else
                        {
                            fullDrives.Add(new Drive
                            {
                                Address = d.Address,
                                CarType = d.CarType,
                                OrderedBy = DataRepository._customerRepo.RetriveCustomerById(d.CustomerId),
                                Date = d.Date,
                                Destination = d.Destination,
                                DrivedBy = DataRepository._driverRepo.RetriveDriverById(d.DriverId),
                                DriveId = d.DriveId,
                                Price = d.Price,
                                State = d.State,
                                Comments = DataRepository._commentRepo.GetCommentById(d.CommentId)
                            });
                        }
                    }
                    else
                    {
                        if (d.CommentId.Equals(Guid.Empty))
                        {
                            fullDrives.Add(new Drive
                            {
                                Address = d.Address,
                                CarType = d.CarType,
                                ApprovedBy = DataRepository._dispatcherRepo.RetriveDispatcherById(d.DispatcherId),
                                Date = d.Date,
                                Destination = d.Destination,
                                DrivedBy = DataRepository._driverRepo.RetriveDriverById(d.DriverId),
                                DriveId = d.DriveId,
                                OrderedBy = DataRepository._customerRepo.RetriveCustomerById(d.CustomerId),
                                Price = d.Price,
                                State = d.State
                            });
                        }
                        else
                        {
                            fullDrives.Add(new Drive
                            {
                                Address = d.Address,
                                CarType = d.CarType,
                                ApprovedBy = DataRepository._dispatcherRepo.RetriveDispatcherById(d.DispatcherId),
                                Date = d.Date,
                                Destination = d.Destination,
                                DrivedBy = DataRepository._driverRepo.RetriveDriverById(d.DriverId),
                                DriveId = d.DriveId,
                                OrderedBy = DataRepository._customerRepo.RetriveCustomerById(d.CustomerId),
                                Price = d.Price,
                                State = d.State,
                                Comments = DataRepository._commentRepo.GetCommentById(d.CommentId)
                            });
                        }
                    }
                }

                return fullDrives;
            }
            else
            {
                return null;
            }
        }

        public IEnumerable<Drive> GetAllDrivesForCustomerId(Guid id)
        {
            if (File.Exists(fileName))
            {
                List<Drive> fullDrives = new List<Drive>();
                FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                XDocument doc = XDocument.Load(stream);
                IEnumerable<DriveDto> drives =
                    doc.Root
                    .Elements("Drive")
                    .Where(x => Guid.Parse(x.Element("CustomerId").Value) == id)
                    .Select(drive => new DriveDto
                    {
                        DriveId = Guid.Parse(drive.Element("Id").Value),
                        CustomerId = Guid.Parse(drive.Element("CustomerId").Value),
                        DispatcherId = Guid.Parse(drive.Element("DispatcherId").Value),
                        DriverId = Guid.Parse(drive.Element("DriverId").Value),
                        CommentId = Guid.Parse(drive.Element("CommentId").Value),
                        Date = DateTime.Parse(drive.Element("Date").Value),
                        CarType = (CarTypes)Enum.Parse(typeof(CarTypes), drive.Element("CarType").Value),
                        Price = Double.Parse(drive.Element("Price").Value, CultureInfo.InvariantCulture),
                        State = (Status)Enum.Parse(typeof(Status), drive.Element("State").Value),
                        Address = new Location
                        {
                            Address = drive.Element("AddressA").Value,
                            X = (double)(drive.Element("AddressX")),
                            Y = (double)(drive.Element("AddressY"))
                        },
                        Destination = new Location
                        {
                            Address = drive.Element("DestinationAddress").Value,
                            X = (double)(drive.Element("DestinationX")),
                            Y = (double)(drive.Element("DestinationY"))
                        }
                    }).ToList();

                foreach (var d in drives)
                {
                    if (d.DispatcherId.Equals(Guid.Empty) && d.DriverId.Equals(Guid.Empty))
                    {
                        if (d.CommentId.Equals(Guid.Empty))
                        {
                            fullDrives.Add(new Drive
                            {
                                Address = d.Address,
                                CarType = d.CarType,
                                OrderedBy = DataRepository._customerRepo.RetriveCustomerById(d.CustomerId),
                                Date = d.Date,
                                Destination = d.Destination,
                                DriveId = d.DriveId,
                                Price = d.Price,
                                State = d.State
                            });
                        }
                        else
                        {
                            fullDrives.Add(new Drive
                            {
                                Address = d.Address,
                                CarType = d.CarType,
                                OrderedBy = DataRepository._customerRepo.RetriveCustomerById(d.CustomerId),
                                Date = d.Date,
                                Destination = d.Destination,
                                DriveId = d.DriveId,
                                Price = d.Price,
                                State = d.State,
                                Comments = DataRepository._commentRepo.GetCommentById(d.CommentId)
                            });
                        }
                    }
                    else if (d.DispatcherId.Equals(Guid.Empty))
                    {
                        if (d.CommentId.Equals(Guid.Empty))
                        {
                            fullDrives.Add(new Drive
                            {
                                Address = d.Address,
                                CarType = d.CarType,
                                OrderedBy = DataRepository._customerRepo.RetriveCustomerById(d.CustomerId),
                                Date = d.Date,
                                Destination = d.Destination,
                                DrivedBy = DataRepository._driverRepo.RetriveDriverById(d.DriverId),
                                DriveId = d.DriveId,
                                Price = d.Price,
                                State = d.State
                            });
                        }
                        else
                        {
                            fullDrives.Add(new Drive
                            {
                                Address = d.Address,
                                CarType = d.CarType,
                                OrderedBy = DataRepository._customerRepo.RetriveCustomerById(d.CustomerId),
                                Date = d.Date,
                                Destination = d.Destination,
                                DrivedBy = DataRepository._driverRepo.RetriveDriverById(d.DriverId),
                                DriveId = d.DriveId,
                                Price = d.Price,
                                State = d.State,
                                Comments = DataRepository._commentRepo.GetCommentById(d.CommentId)
                            });
                        }
                    }
                    else
                    {
                        if (d.CommentId.Equals(Guid.Empty))
                        {
                            fullDrives.Add(new Drive
                            {
                                Address = d.Address,
                                CarType = d.CarType,
                                ApprovedBy = DataRepository._dispatcherRepo.RetriveDispatcherById(d.DispatcherId),
                                Date = d.Date,
                                Destination = d.Destination,
                                DrivedBy = DataRepository._driverRepo.RetriveDriverById(d.DriverId),
                                DriveId = d.DriveId,
                                OrderedBy = DataRepository._customerRepo.RetriveCustomerById(d.CustomerId),
                                Price = d.Price,
                                State = d.State
                            });
                        }
                        else
                        {
                            fullDrives.Add(new Drive
                            {
                                Address = d.Address,
                                CarType = d.CarType,
                                ApprovedBy = DataRepository._dispatcherRepo.RetriveDispatcherById(d.DispatcherId),
                                Date = d.Date,
                                Destination = d.Destination,
                                DrivedBy = DataRepository._driverRepo.RetriveDriverById(d.DriverId),
                                DriveId = d.DriveId,
                                OrderedBy = DataRepository._customerRepo.RetriveCustomerById(d.CustomerId),
                                Price = d.Price,
                                State = d.State,
                                Comments = DataRepository._commentRepo.GetCommentById(d.CommentId)
                            });
                        }
                    }
                }

                return fullDrives;
            }
            else
            {
                return null;
            }
        }

        public Drive RetriveDriveById(Guid id)
        {
            if (File.Exists(fileName))
            {
                List<Drive> fullDrives = new List<Drive>();
                FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
                XDocument doc = XDocument.Load(stream);
                IEnumerable<DriveDto> drives =
                    doc.Root
                    .Elements("Drive")
                    .Select(drive => new DriveDto
                    {
                        DriveId = Guid.Parse(drive.Element("Id").Value),
                        CustomerId = Guid.Parse(drive.Element("CustomerId").Value),
                        DispatcherId = Guid.Parse(drive.Element("DispatcherId").Value),
                        DriverId = Guid.Parse(drive.Element("DriverId").Value),
                        CommentId = Guid.Parse(drive.Element("CommentId").Value),
                        Date = DateTime.Parse(drive.Element("Date").Value),
                        CarType = (CarTypes)Enum.Parse(typeof(CarTypes), drive.Element("CarType").Value),
                        Price = Double.Parse(drive.Element("Price").Value, CultureInfo.InvariantCulture),
                        State = (Status)Enum.Parse(typeof(Status), drive.Element("State").Value),
                        Address = new Location
                        {
                            Address = drive.Element("AddressA").Value,
                            X = (double)(drive.Element("AddressX")),
                            Y = (double)(drive.Element("AddressY"))
                        },
                        Destination = new Location
                        {
                            Address = drive.Element("DestinationAddress").Value,
                            X = (double)(drive.Element("DestinationX")),
                            Y = (double)(drive.Element("DestinationY"))
                        }
                    }).ToList();

                foreach (var d in drives)
                {

                    if (d.DispatcherId.Equals(Guid.Empty) && d.DriverId.Equals(Guid.Empty))
                    {
                        if (d.CommentId.Equals(Guid.Empty))
                        {
                            fullDrives.Add(new Drive
                            {
                                Address = d.Address,
                                CarType = d.CarType,
                                OrderedBy = DataRepository._customerRepo.RetriveCustomerById(d.CustomerId),
                                Date = d.Date,
                                Destination = d.Destination,
                                DriveId = d.DriveId,
                                Price = d.Price,
                                State = d.State
                            });
                        }
                        else
                        {
                            fullDrives.Add(new Drive
                            {
                                Address = d.Address,
                                CarType = d.CarType,
                                OrderedBy = DataRepository._customerRepo.RetriveCustomerById(d.CustomerId),
                                Date = d.Date,
                                Destination = d.Destination,
                                DriveId = d.DriveId,
                                Price = d.Price,
                                State = d.State,
                                Comments = DataRepository._commentRepo.GetCommentById(d.CommentId)
                            });
                        }
                    }
                    else if (d.CustomerId.Equals(Guid.Empty))
                    {
                        if (d.CommentId.Equals(Guid.Empty))
                        {
                            fullDrives.Add(new Drive
                            {
                                Address = d.Address,
                                CarType = d.CarType,
                                ApprovedBy = DataRepository._dispatcherRepo.RetriveDispatcherById(d.DispatcherId),
                                Date = d.Date,
                                Destination = d.Destination,
                                DrivedBy = DataRepository._driverRepo.RetriveDriverById(d.DriverId),
                                DriveId = d.DriveId,
                                Price = d.Price,
                                State = d.State
                            });
                        }
                        else
                        {
                            fullDrives.Add(new Drive
                            {
                                Address = d.Address,
                                CarType = d.CarType,
                                ApprovedBy = DataRepository._dispatcherRepo.RetriveDispatcherById(d.DispatcherId),
                                Date = d.Date,
                                Destination = d.Destination,
                                DrivedBy = DataRepository._driverRepo.RetriveDriverById(d.DriverId),
                                DriveId = d.DriveId,
                                Price = d.Price,
                                State = d.State,
                                Comments = DataRepository._commentRepo.GetCommentById(d.CommentId)
                            });
                        }
                    }
                    else if (d.DispatcherId.Equals(Guid.Empty))
                    {
                        if (d.CommentId.Equals(Guid.Empty))
                        {
                            fullDrives.Add(new Drive
                            {
                                Address = d.Address,
                                CarType = d.CarType,
                                OrderedBy = DataRepository._customerRepo.RetriveCustomerById(d.CustomerId),
                                Date = d.Date,
                                Destination = d.Destination,
                                DrivedBy = DataRepository._driverRepo.RetriveDriverById(d.DriverId),
                                DriveId = d.DriveId,
                                Price = d.Price,
                                State = d.State
                            });
                        }
                        else
                        {
                            fullDrives.Add(new Drive
                            {
                                Address = d.Address,
                                CarType = d.CarType,
                                OrderedBy = DataRepository._customerRepo.RetriveCustomerById(d.CustomerId),
                                Date = d.Date,
                                Destination = d.Destination,
                                DrivedBy = DataRepository._driverRepo.RetriveDriverById(d.DriverId),
                                DriveId = d.DriveId,
                                Price = d.Price,
                                State = d.State,
                                Comments = DataRepository._commentRepo.GetCommentById(d.CommentId)
                            });
                        }
                    }
                    else
                    {
                        if (d.CommentId.Equals(Guid.Empty))
                        {
                            fullDrives.Add(new Drive
                            {
                                Address = d.Address,
                                CarType = d.CarType,
                                ApprovedBy = DataRepository._dispatcherRepo.RetriveDispatcherById(d.DispatcherId),
                                Date = d.Date,
                                Destination = d.Destination,
                                DrivedBy = DataRepository._driverRepo.RetriveDriverById(d.DriverId),
                                DriveId = d.DriveId,
                                OrderedBy = DataRepository._customerRepo.RetriveCustomerById(d.CustomerId),
                                Price = d.Price,
                                State = d.State
                            });
                        }
                        else
                        {
                            fullDrives.Add(new Drive
                            {
                                Address = d.Address,
                                CarType = d.CarType,
                                ApprovedBy = DataRepository._dispatcherRepo.RetriveDispatcherById(d.DispatcherId),
                                Date = d.Date,
                                Destination = d.Destination,
                                DrivedBy = DataRepository._driverRepo.RetriveDriverById(d.DriverId),
                                DriveId = d.DriveId,
                                OrderedBy = DataRepository._customerRepo.RetriveCustomerById(d.CustomerId),
                                Price = d.Price,
                                State = d.State,
                                Comments = DataRepository._commentRepo.GetCommentById(d.CommentId)
                            });
                        }
                    }
                }

                return fullDrives.FirstOrDefault(d => d.DriveId == id);
            }
            else
            {
                return null;
            }
        }
    }
}