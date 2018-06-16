using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TaxiService.Repository;

namespace TaxiService.ApiHelpers
{
    public class ServiceSecurity
    {
        public static bool Login(string username, string password)
        {
            if (DataRepository._driverRepo.LogIn(username, password))
            {
                return true;
            }
            else if (DataRepository._dispatcherRepo.LogIn(username, password))
            {
                return true;
            }
            else if (DataRepository._customerRepo.LogIn(username, password))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string MakeToken(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }

        //GO TO https://www.c-sharpcorner.com/article/introduction-to-aes-and-des-encryption-algorithms-in-net/
    }
}