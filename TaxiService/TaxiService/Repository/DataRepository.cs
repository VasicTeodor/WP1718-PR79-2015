using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiService.Models;

namespace TaxiService.Repository
{
    class DataRepository
    {
        public static DataIO repo = new DataIO();
        public static List<User> users = new List<User>();
        public static List<Driver> freeDrivers = new List<Driver>();
        public static List<Driver> ocupiedDrivers = new List<Driver>();
        public static List<Drive> drives = new List<Drive>();
    }
}
