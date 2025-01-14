using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaskWebApp.Models
{
    public class TaskViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TaskState State { get; set; }
        public Nullable<System.DateTime> LastDate { get; set; }
    }
}