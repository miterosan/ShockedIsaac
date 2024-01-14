using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShockedIsaac.API
{
    public class Device
    {
        public string name { get; set; }
        public string id { get; set; }
        public string createdOn { get; set; }

        public Shocker[] shockers { get;set; }
    }
}