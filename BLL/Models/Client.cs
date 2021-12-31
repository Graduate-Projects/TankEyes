using Newtonsoft.Json;
using System;

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
        public string home_location { get; set; }
        public double main_percentage { get; set; }
        public double secondary_percentage { get; set; }
        public double eta { get; set; }
        public double longitude { get; set; }
        public double latitude { get; set; }
        public int Notification { get; set; }

        [JsonIgnore]
        public double main_percentage_water => main_percentage * 100;
        [JsonIgnore]
        public double secondary_percentage_water => secondary_percentage * 100;
        [JsonIgnore]
        public TimeSpan estimated_time => TimeSpan.FromMinutes(eta);

    }
}