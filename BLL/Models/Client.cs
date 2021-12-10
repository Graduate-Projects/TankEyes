using Newtonsoft.Json;

namespace BLL.Models
{
    public class Client
    {
        public string id { get; set; }
        public string full_name { get; set; }
        public string phone_number { get; set; }
        public string tank_size { get; set; }
        public bool pumb_status { get; set; }
        public bool auto { get; set; }
        public string address { get; set; }
        public double main_percentage { get; set; }
        public double secondary_percentage { get; set; }
        public string eta { get; set; }


        [JsonIgnore]
        public double main_percentage_water => main_percentage * 100;
        [JsonIgnore]
        public double secondary_percentage_water => secondary_percentage * 100;

    }
}