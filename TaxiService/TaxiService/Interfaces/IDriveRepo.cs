using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiService.Models;

namespace TaxiService.Interfaces
{
    public interface IDriveRepo
    {
        IEnumerable<Drive> GetAllDrives();
        IEnumerable<Drive> GetAllDrivesForCustomerId(Guid id);
        void AddNewDriveCustomer(Drive drive);
        void AddNewDriveDispatcher(Drive drive);
        Drive RetriveDriveById(Guid id);
        void CustomerEditDrive(Drive drive);
        void DispatcherEditDrive(Drive drive);
        void DriverEditDrive(Drive drive);
    }
}
