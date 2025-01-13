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
        private TaskManagmentDatabaseEntities db = new TaskManagmentDatabaseEntities();

        // GET: Tasks
        public ActionResult Index(int? stateFilter)
        {
            // Get all tasks
            var tasks = db.TasksDatas.AsQueryable();

            // Apply the filter if stateFilter is provided
            if (stateFilter.HasValue)
            {
                tasks = tasks.Where(t => (int)t.State == stateFilter.Value);
            }

            // Pass the current filter to the view via ViewBag
            ViewBag.StateFilter = stateFilter;

            // Populate the dropdown options
            ViewBag.StateList = Enum.GetValues(typeof(TaskState))
                .Cast<TaskState>()
                .Select(e => new SelectListItem
                {
                    Value = ((int)e).ToString(),
                    Text = e.ToString(),
                    Selected = stateFilter.HasValue && ((int)e) == stateFilter.Value
                }).ToList();
            
            return View(tasks.ToList());
        }

        // GET: Tasks/Create
        public ActionResult Create()
        {
            // Pass enum options to the ViewBag for the dropdown
            ViewBag.StateOptions = Enum.GetValues(typeof(TaskState))
                .Cast<TaskState>()
                .Select(e => new SelectListItem
                {
                    Value = ((int)e).ToString(),
                    Text = e.ToString()
                });

            return View();
        }

        // POST: Tasks/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Description,State,LastDate")] TasksData tasksData)
        {
            if (ModelState.IsValid)
            {
                tasksData.Id = Guid.NewGuid();
                db.TasksDatas.Add(tasksData);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            // Re-populate StateOptions in case of validation errors
            ViewBag.StateOptions = Enum.GetValues(typeof(TaskState))
                .Cast<TaskState>()
                .Select(e => new SelectListItem
                {
                    Value = ((int)e).ToString(),
                    Text = e.ToString()
                });

            return View(tasksData);
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

            // Populate the State dropdown
            ViewBag.StateOptions = Enum.GetValues(typeof(TaskState))
                .Cast<TaskState>()
                .Select(e => new SelectListItem
                {
                    Value = ((int)e).ToString(),
                    Text = e.ToString(),
                    Selected = (int)e == (int)tasksData.State // Set the current state as selected
                });

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

            // Re-populate the State dropdown in case of validation errors
            ViewBag.StateOptions = Enum.GetValues(typeof(TaskState))
                .Cast<TaskState>()
                .Select(e => new SelectListItem
                {
                    Value = ((int)e).ToString(),
                    Text = e.ToString(),
                    Selected = (int)e == (int)tasksData.State
                });

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
