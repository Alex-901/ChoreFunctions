using System;
using System.Collections.Generic;
using System.Text;

namespace ChoreFunction.Models
{
    public class DTOChores
    {
        public int ChoreId { get; set; }
        public int? ChoreTypeId { get; set; }
        public DTOChoreType ChoreType { get; set; }
        public DateTime? CompletionDate { get; set; }
        public string CompletionUser { get; set; }
        public DateTime? DueDate { get; set; }
        public string LastCompletedBy { get; set; }
        public DateTime LastCompleted { get; set; }
    }
}
