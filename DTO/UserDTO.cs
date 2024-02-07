using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace dotnet_JWT.DTO
{
    public class UserDTO
    {
        public required string Username { get; set; } = string.Empty ; 
        public  required string   Password { get; set; } = string.Empty ; 
    }
}