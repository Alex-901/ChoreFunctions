using System;
using System.Collections.Generic;
using System.Text;

namespace ChoreFunction.Models
{
    public class ConfigurationItems
    {
        public string APIBaseAddress { get; set; }
        public string TwilioUser { get; set; }
        public string TwilioPassword { get; set; }
        public string SendGridkey { get; set; }
    }
}
