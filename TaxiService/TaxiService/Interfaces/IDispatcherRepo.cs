using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiService.Models;

namespace TaxiService.Interfaces
{
    public interface IDispatcherRepo
    {
        Dispatcher RetriveDispatcherById(Guid id);
        Dispatcher RetriveDispatcherByUserName(string Name);
        bool CheckIfDispatcherExists(string username);
        void EditDispatcherProfile(Dispatcher dispatcher);
        bool LogIn(string username, string password);
    }
}
