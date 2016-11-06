using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoClassGeneratorLibrary
{
    public class GeneratedClass
    {
        public string Name { get; set; }
        public string Contents { get; set; }
        public GeneratedClass( string className, string classContents)
        {
            Name = className;
            Contents = classContents;
        }
    }
}
