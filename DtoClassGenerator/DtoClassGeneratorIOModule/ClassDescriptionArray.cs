using Newtonsoft.Json;

namespace DtoClassGeneratorIOModule
{
    public class ClassDescriptionArray
    {
        [JsonProperty("ClassDescriptions")]
        public ClassDescription[] values { get; set; }
    }

}
