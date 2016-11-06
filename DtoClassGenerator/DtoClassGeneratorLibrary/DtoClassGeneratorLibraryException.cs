using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoClassGeneratorLibrary
{

    public class DtoClassGeneratorLibraryException : Exception
    {
        public DtoClassGeneratorLibraryException()
        {
        }

        public DtoClassGeneratorLibraryException(string message)
        : base(message)
        {
        }

        public DtoClassGeneratorLibraryException(string message, Exception inner)
        : base(message, inner)
        {
        }
    }
}
