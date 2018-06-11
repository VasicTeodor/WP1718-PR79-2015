using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiService.Models;

namespace TaxiService.Interfaces
{
    public interface ICustomerRepo
    {
        void NewCustomer(Customer customer);
        Customer RetriveCustomerById(Guid id);
        Customer RetriveCustomerByUserName(string Name);
        bool CheckIfCustomerExists(string username);
        bool LogIn(string username, string password);
        IEnumerable<Customer> RetriveAllCustomers();
        void EditCustomerProfile(Customer customer);
    }
}
