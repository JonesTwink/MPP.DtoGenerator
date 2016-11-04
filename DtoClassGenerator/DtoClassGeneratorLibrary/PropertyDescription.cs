using Newtonsoft.Json;

namespace DtoClassGeneratorLibrary
{
    public class PropertyDescription
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("format")]
        public string Format { get; set; }

        public PropertyDescription(string name, string type, string format)
        {
            Name = name;
            Type = type;
            Format = format;
        }
    }
}
