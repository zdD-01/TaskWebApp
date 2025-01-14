using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaskWebApp.Models
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity.ModelConfiguration.Conventions;
    using System.Data.Entity;

    public enum TaskState
    {
        Waiting,
        Continues,
        Complete
    }

    public partial class TasksData
    {
        public System.Guid Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public TaskState State { get; set; }
        public Nullable<System.DateTime> LastDate { get; set; }
    }

    public class TaskContext : DbContext
    {
        public TaskContext() : base("name=TaskContext")
        {
        }

        public DbSet<TasksData> TasksDatas { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<TasksData>().ToTable("TasksData");
        }
    }
}