using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_JWT.Models
{
    public class User
    {
        public string Username { get; set; } = string.Empty; 
        public string PasswordHash { get; set; } = string.Empty;

     }
}

