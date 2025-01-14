using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TaskWebApp.Models;

namespace TaskWebApp.Controllers
{
    public class TasksController : Controller
    {
        private TaskContext db = new TaskContext();
        // GET: Tasks
        public ActionResult Index(TaskState? stateFilter)
        {
            var tasks = stateFilter.HasValue 
                ? db.TasksDatas.Where(t => t.State == stateFilter.Value)
                : db.TasksDatas.AsQueryable();

            // Create the ViewModel
            var viewModel = new TaskListViewModel
            {
                Tasks = tasks.ToList(),
                SelectedState = stateFilter,
                StateList = new List<SelectListItem>
                {
                    new SelectListItem
                    {
                        Text = "All States",
                        Value = "", // Empty value corresponds to null
                        Selected = !stateFilter.HasValue
                    }
                }
                .Concat(
                    Enum.GetValues(typeof(TaskState))
                        .Cast<TaskState>()
                        .Select(s => new SelectListItem
                        {
                            Text = s.ToString(),
                            Value = ((int)s).ToString(),
                            Selected = stateFilter.HasValue && s == stateFilter.Value
                        })).ToList()
                };

            return View(viewModel);
        }

        
        // GET: Tasks/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Tasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TaskViewModel model)
        {
            var tasksData = new TasksData
            {
                Id = Guid.NewGuid(),
                Name = model.Name,
                Description = model.Description,
                State = model.State,
                LastDate = model.LastDate

            };
            model.Id = tasksData.Id.ToString();

            db.TasksDatas.Add(tasksData);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        // GET: Tasks/Edit/5
        // GET: Tasks/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TasksData tasksData = db.TasksDatas.Find(id);
            if (tasksData == null)
            {
                return HttpNotFound();
            }

            return View(tasksData);
        }


        // POST: Tasks/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Description,State,LastDate")] TasksData tasksData)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tasksData).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tasksData);
        }


        // GET: Tasks/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TasksData tasksData = db.TasksDatas.Find(id);
            if (tasksData == null)
            {
                return HttpNotFound();
            }
            return View(tasksData);
        }

        // POST: Tasks/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            TasksData tasksData = db.TasksDatas.Find(id);
            db.TasksDatas.Remove(tasksData);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
