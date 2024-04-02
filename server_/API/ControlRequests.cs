using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShockedIsaac.API
{
    public class ControlRequests
    {
        
        public required Shocker[] Shockers { get; set; }
        public required ShockerCommandType Type { get; set; }
        public required int Amount { get; set; }
        public required int Duration { get; set; }
        public required string Name { get; set; }
    }
}