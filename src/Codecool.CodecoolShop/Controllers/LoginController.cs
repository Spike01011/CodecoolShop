using Codecool.CodecoolShop.Daos;
using Codecool.CodecoolShop.Daos.Implementations;
using Codecool.CodecoolShop.Models;
using Codecool.CodecoolShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace Codecool.CodecoolShop.Controllers
{
    public class LoginController : Controller
    {
        private IUserDao userDao;
        public LoginController()
        {
            SqlManager sqlManager = SqlManager.GetInstance();
            if (!sqlManager.testConnection())
            {
                userDao = new UserDaoMemory();
            }
            else
            {
                userDao = new UserDaoDB();
            }
        }
        [HttpGet]
        public IActionResult Login(bool wrongInput=false)
        {
            ViewBag.Username = MyGlobals.Username;
            ViewBag.wrongInput = wrongInput;
            return View(new LoginData());
        }

        [HttpPost]
        public IActionResult LoginValidation(LoginData model)
        {
            ViewBag.Username = MyGlobals.Username;
            if (ModelState.IsValid)
            {
                var expected = userDao.GetBy(model.UserName.ToLower());
                if (expected != null)
                {
                    if (model.Password == expected.Password)
                    {
                        MyGlobals.Id = expected.Id;
                        MyGlobals.Username = expected.UserName;
                        return RedirectToAction("Index", "Product");
                    }

                    return RedirectToAction("Login", "Login", new { wrongInput = true });
                }

                return RedirectToAction("Login", "Login", new { wrongInput = true });
            }

            return RedirectToAction("Login", "Login", new{ wrongInput = true});
        }

        [HttpGet]
        public IActionResult Signup(bool taken=false)
        {
            ViewBag.Username = MyGlobals.Username;
            ViewBag.taken = taken;
            return View(new LoginData());
        }

        [HttpPost]
        public IActionResult SignupValidation(LoginData model)
        {
            ViewBag.Username = MyGlobals.Username;
            if (ModelState.IsValid)
            {
                var usernames = userDao.GetAllNames();
                if (!usernames.Contains(model.UserName.ToLower()))
                {
                    var user = new User(model.UserName.ToLower(), model.Password);
                    userDao.Add(user);
                    MyGlobals.Username = user.UserName.ToLower();
                    MyGlobals.Id = user.Id;
                    return RedirectToAction("Index", "Product");

                }

                return RedirectToAction("Signup", "Login", new { taken = true });
            }

            return RedirectToAction("Signup", "Login");

        }

        public IActionResult LogOut()
        {
            MyGlobals.Username = null;
            MyGlobals.Id = null;
            return RedirectToAction("Index", "Product");
        }
    }
}
