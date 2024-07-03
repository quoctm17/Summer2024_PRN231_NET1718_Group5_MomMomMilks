﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataTransfer
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public byte Status { get; set; }
        public int Point { get; set; }
        public string Role { get; set; }
        public string Token { get; set; }
    }
}
