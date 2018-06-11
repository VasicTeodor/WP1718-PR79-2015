using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiService.Models;

namespace TaxiService.Interfaces
{
    public interface IDriverRepo
    {
        void NewDriver(Driver driver);
        Driver RetriveDriverById(Guid id);
        Driver RetriveDriverByUserName(string Name);
        bool CheckIfDriverExists(string username);
        bool LogIn(string username, string password);
        IEnumerable<Driver> RetriveAllDrivers();
        void EditDriverProfile(Driver driver);
    }
}
