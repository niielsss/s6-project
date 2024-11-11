using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MWL.ContentService.Messaging
{
    public class BrokerMessage
    {
        public string Action { get; set; }
        public string Data { get; set; }
    }
}
