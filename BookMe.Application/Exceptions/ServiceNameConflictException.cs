using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookMe.Application.Exceptions
{
    public class ServiceNameConflictException : Exception
    {
        public ServiceNameConflictException(string message) : base(message) { }
    }
}
