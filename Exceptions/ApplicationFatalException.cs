using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exceptions
{
    public class ApplicationFatalException : Exception
    {
        public ApplicationFatalException(string message)
            : base(message)
        { }
    }
}
