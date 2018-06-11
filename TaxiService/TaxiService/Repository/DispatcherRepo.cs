using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TaxiService.Interfaces;
using TaxiService.Models;

namespace TaxiService.Repository
{
    public class DispatcherRepo : IDispatcherRepo
    {
        public bool CheckIfDispatcherExists(string username)
        {
            throw new NotImplementedException();
        }

        public void EditDispatcherProfile(Dispatcher dispatcher)
        {
            throw new NotImplementedException();
        }

        public bool LogIn(string username, string password)
        {
            throw new NotImplementedException();
        }

        public Dispatcher RetriveDispatcherById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Dispatcher RetriveDispatcherByUserName(string Name)
        {
            throw new NotImplementedException();
        }
    }
}