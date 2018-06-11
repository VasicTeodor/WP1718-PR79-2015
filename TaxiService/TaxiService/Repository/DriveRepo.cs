using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TaxiService.Interfaces;
using TaxiService.Models;

namespace TaxiService.Repository
{
    public class DriveRepo : IDriveRepo
    {
        public void AddNewDrive(Drive drive)
        {
            throw new NotImplementedException();
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