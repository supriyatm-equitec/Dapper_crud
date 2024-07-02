using Dapper_mvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Dapper_mvc.Controllers
{
    public class UserController : Controller
    {
        private readonly UserRepository _userRepository = new UserRepository();

        // GET: User
        public ActionResult Index(string searchString)
        {
       
            var users = _userRepository.GetAllUsers();
        
            if (!String.IsNullOrEmpty(searchString))
            {
                searchString = searchString.ToUpper(); // Convert to uppercase for case-insensitive search

                users = users.Where(u =>
                    u.Username.ToUpper().Contains(searchString) ||
                    u.FirstName.ToUpper().Contains(searchString) ||
                    u.LastName.ToUpper().Contains(searchString) ||
                    u.Department.ToUpper().Contains(searchString) ||
                    u.Email.ToUpper().Contains(searchString)
                // Add more fields as needed for filtering
                ).ToList();
            }

            ViewBag.CurrentFilter = searchString;
            return View(users);
        }

        public ActionResult Details(int id)
        {
            var user = _userRepository.GetUserById(id);
            return View(user);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(User user)
        {
            if (ModelState.IsValid)
            {
                _userRepository.CreateUser(user);
                return RedirectToAction("Index");
            }
            return View(user);
        }

        public ActionResult Edit(int id)
        {
            var user = _userRepository.GetUserById(id);
            return View(user);
        }

        [HttpPost]
        public ActionResult Edit(User user)
        {
            if (ModelState.IsValid)
            {
                _userRepository.UpdateUser(user);
                return RedirectToAction("Index");
            }
            return View(user);
        }

        public ActionResult Delete(int id)
        {
            var user = _userRepository.GetUserById(id);
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);
        }

        [HttpPost]
[ValidateAntiForgeryToken]
public ActionResult DeleteConfirmed(int id)
{
    _userRepository.SoftDeleteUser(id);
    return RedirectToAction("Index");
}



        public ActionResult SoftDeleteUser()
        {
            var deletedUsers = _userRepository.GetDeletedUsers(); // Add a method to get deleted users
            return View(deletedUsers);
        }
    }
}