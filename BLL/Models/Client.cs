using Newtonsoft.Json;

namespace BLL.Models
{
    public class Client
    {
        public double main_percentage { get; set; }
        public double secondary_percentage { get; set; }
        public bool status { get; set; }
        public bool auto { get; set; }


        [JsonIgnore]
        public double main_percentage_water => main_percentage * 100;
        [JsonIgnore]
        public double secondary_percentage_water => secondary_percentage * 100;
    }
}