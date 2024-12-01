using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gastapp.Models
{
    public class LoginResponse
    {

        public int Code { get; set; }
        public string Message { get; set; } = string.Empty;
        public UserLogin? Data { get; set; }
    }
}
