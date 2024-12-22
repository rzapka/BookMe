using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMe.Application.Exceptions
{
    public class UserEmailConflictException : Exception
    {
        public UserEmailConflictException(string message) : base(message) { }
    }
}
