using System;
using System.Collections.Generic;

namespace TaskListApp.Models
{
    public partial class Tasks
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public DateTime Due { get; set; }
        public bool Complete { get; set; }
        public string User { get; set; }

        public virtual AspNetUsers UserNavigation { get; set; }
    }
}
