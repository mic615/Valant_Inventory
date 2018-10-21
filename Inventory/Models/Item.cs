using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inventory.Models
{

    public class Item
    {
        public int Label { get; set; } = 0;
        public string Type { get; set; } = "";
        public bool IsExpired { get; set; } = false;
    }
}