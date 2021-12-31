using System;
using System.Collections.Generic;
using System.Text;
using BLL.Enums;
namespace BLL.Models
{
    public class Order
    {
        public string id;
        public string creation_date;

        public string client_id { get; set; }
        public string supplier_id { get; set; }
        public bool is_supplier_accepted { get; set; }
        public OrderStatus status { get; set; }
    }
}
