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
        void AddNewDrive(Drive drive);
        Drive RetriveDriveById(Guid id);
        Drive RetriveDriveByUser(User user);
        void EditDrive(Drive drive);
    }
}
