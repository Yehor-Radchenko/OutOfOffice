using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace OutOfOffice.BLL.Exceptions
{
    public class PhotoNotFoundException : Exception
    {
        public PhotoNotFoundException()
        {
        }
        public PhotoNotFoundException(string? message) : base(message)
        {
        }
    }
}
