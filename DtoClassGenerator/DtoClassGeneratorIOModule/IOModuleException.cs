using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoClassGeneratorIOModule
{
    class IOModuleException : Exception
    {
        public IOModuleException()
        {
        }

        public IOModuleException(string message)
        : base(message)
        {
        }

        public IOModuleException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
