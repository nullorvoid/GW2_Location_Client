using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GW2_Location_Client.Models
{
    public class Api_Update
    {
        public string Api_Key { get; set; }
        private Api_Player _Player;
        private Api_Location _Location;

        public Api_Update()
        {
            Player = new Api_Player();
            Location = new Api_Location();
        }

        public Api_Player Player
        {
            get
            {
                return _Player;
            }
            set
            {
                _Player = value;
            }
        }

        public Api_Location Location
        {
            get
            {
                return _Location;
            }
            set
            {
                _Location = value;
            }
        }
    }
}
