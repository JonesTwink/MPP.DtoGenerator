using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DtoClassGeneratorLibrary
{
    internal class SupportedTypes
    {
        private Dictionary<string, Type> container = new Dictionary<string, Type>();

        public string this[string key]
        {
            get
            {  
                return container[key].ToString();
            }
        }

        public SupportedTypes()
        {
            container.Add("int32",typeof(int));
            container.Add("int64", typeof(long));
            container.Add("string", typeof(string));
            container.Add("float", typeof(float));
            container.Add("double", typeof(double));
            container.Add("byte", typeof(byte));
            container.Add("date", typeof(DateTime));
            container.Add("bool", typeof(bool));
        }
    }
}
