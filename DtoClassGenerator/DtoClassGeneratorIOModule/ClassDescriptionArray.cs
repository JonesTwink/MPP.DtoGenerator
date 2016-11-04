using Newtonsoft.Json;
using DtoClassGeneratorLibrary;

namespace DtoClassGeneratorIOModule
{
    public class ClassDescriptionArray
    {
        [JsonProperty("ClassDescriptions")]
        public ClassDescription[] values { get; set; }
    }

}
