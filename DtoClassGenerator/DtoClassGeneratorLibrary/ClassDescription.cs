using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoClassGeneratorLibrary
{
    public class ClassDescription
    {
        public string ClassName { get; set; }
        public PropertyDescription[] Properties { get; set; }

        public ClassDescription(string className)
        {
            ClassName = className;
        }
    }
}
