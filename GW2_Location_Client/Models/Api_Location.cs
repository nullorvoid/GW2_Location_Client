using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GW2_Location_Client.Models
{
    public class Api_Location
    {
        public int World_id { get; set; }
        public int Map_id { get; set; }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
    }
}
