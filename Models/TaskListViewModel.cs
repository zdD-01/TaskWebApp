using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TaskWebApp.Models
{
    public class TaskListViewModel
    {
        public IEnumerable<TasksData> Tasks { get; set; }
        public List<SelectListItem> StateList { get; set; }
        public TaskState? SelectedState { get; set; }
    }
}