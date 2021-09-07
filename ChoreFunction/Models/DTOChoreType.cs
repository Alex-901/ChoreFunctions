using System;
using System.Collections.Generic;
using System.Text;

namespace ChoreFunction.Models
{
    public class DTOChoreType
    {
        public int ChoreTypeId { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public int? FrequencyDays { get; set; }
    }
}
