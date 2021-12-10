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
    }
}
