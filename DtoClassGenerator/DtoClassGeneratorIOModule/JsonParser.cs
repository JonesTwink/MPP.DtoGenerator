using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
namespace DtoClassGeneratorIOModule
{
    internal class JsonParser
    {
        public static ClassDescription[] Parse(string input)
        {
            ClassDescriptionArray parseResult = new ClassDescriptionArray();

            try
            {
                parseResult = JsonConvert.DeserializeObject<ClassDescriptionArray>(input);
            }
            catch
            {
                throw new IOModuleException("An error occured while parsing the input file. Check your json syntax.");
            }

            return parseResult.values;
        }
    }
}
