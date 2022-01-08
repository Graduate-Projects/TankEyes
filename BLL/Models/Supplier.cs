using Newtonsoft.Json;
using System;

namespace BLL.Models
{

    public class Supplier
    {

        public string id { get; set; }
        public string full_name { get; set; }
        public string phone_number { get; set; }
        public bool available { get; set; }
        public string estimated_price { get; set; }
        public string tank_size { get; set; }
        public string tank_color { get; set; }
        public string tank_plate_number { get; set; }
        public string work_location { get; set; }

        public double longitude { get; set; }
        public double latitude { get; set; }

        [JsonIgnore]
        public string estimated_time { get; set; } = "--:--:--";
    }
}
