using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TaskListApp.Models;

namespace TaskListApp.Controllers
{
    [Authorize]
    public class TaskListController : Controller
    {
        private readonly TaskListDbContext _context;

        public TaskListController(TaskListDbContext context)
        {
            _context = context;
        }
        
        [HttpPost]
        public IActionResult AddTask(Tasks t)
        {
            if (ModelState.IsValid)
            {
                _context.Tasks.Add(t);
                _context.SaveChanges();
                return RedirectToAction("TaskList");
            }
            return View();
        }
        
        [HttpGet]
        public IActionResult AddTask()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            ViewBag.UserId = id;
            return View();
        }

        [HttpGet]
        public IActionResult EditTask(int id)
        {
            Tasks t = _context.Tasks.FirstOrDefault(x => x.Id.Equals(id));
            return View(t);
        }

        [HttpPost]
        public IActionResult EditTask(Tasks t)
        {
            _context.Update(t);
            _context.SaveChanges();
            return RedirectToAction("TaskList");
        }

        public IActionResult DeleteTask(int id)
        {
            Tasks t = _context.Tasks.FirstOrDefault(x => x.Id.Equals(id));
            if (t == null)
            {
                return RedirectToAction("TaskList");
            }
            _context.Tasks.Remove(t);
            _context.SaveChanges();
            return RedirectToAction("TaskList");
        }
        
        [HttpGet]
        public IActionResult TaskList()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<Tasks> tasks = _context.Tasks.Where(x => x.User.Equals(id)).ToList();
            return View(tasks);
        }
        
        [HttpPost]
        public IActionResult TaskList(string search, string complete)
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            List<Tasks> tasks = _context.Tasks.Where(x => true).ToList();
            if (!string.IsNullOrEmpty(search))
            {
                tasks = _context.Tasks.Where(x => x.User.Equals(id) && x.Description.ToLower().Contains(search.ToLower())).ToList();
            }
            if (complete != "donotfilter")
            {
                bool statusSearch = complete.Equals("filtercomplete");
                tasks = tasks.Where(x => x.Complete == statusSearch).ToList();
            }
            return View(tasks);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
        }
    }
}