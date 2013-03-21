using System;
using System.Web.Mvc;
using System.Web.Security;
using QuickStartProject.Domain.Entities;
using QuickStartProject.Domain.Repository;
using QuickStartProject.Web.UI.Models.Account;
using QuickStartProject.Web.UI.Security;

namespace QuickStartProject.Web.UI.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IRepository<User, Guid> _userRepository;

        public AccountController(IRepository<User, Guid> userRepository)
        {
            _userRepository = userRepository;
        }

        [HttpGet]
        public ActionResult Logon()
        {
            return View("Index", new AccountViewModel());
        }

        [HttpPost]
        public ActionResult Logon(AccountViewModel accountModel)
        {
            if (!ModelState.IsValid)
            {
                ShowError("Please provide your username and password!");
                MergeModelState();
                return RedirectToAction("Logon");
            }
            var userItem = _userRepository.GetOne(u => u.Email == accountModel.Username);
            if (!AuthorizeUser(accountModel, userItem))
            {
                ShowError("Invalid username or password");
                MergeModelState();
                return RedirectToAction("Logon");
            }
            return RedirectToAction("Index", "Home");
        }

        private static bool AuthorizeUser(AccountViewModel accountModel, User user)
        {
            if (user != null && PasswordHash.ValidatePassword(accountModel.Password, user.Password, user.Salt))
            {
                FormsAuthentication.SetAuthCookie(accountModel.Username, accountModel.Remember);
                return true;
            }
            return false;
        }

        [HttpGet]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Register()
        {
            return View("Register", new UserViewModel());
        }

        [HttpPost]
        public ActionResult Register(UserViewModel user)
        {
            if (!ModelState.IsValid)
            {
                ShowError("Please provide all required fields!");
                MergeModelState();
                return RedirectToAction("Register");
            }
            var userItem = _userRepository.GetOne(u => u.Email == user.Email);
            if (userItem != null)
            {
                ShowError("User with the same email already exist!");
                MergeModelState();
                return RedirectToAction("Register");
            }
            _userRepository.Save(ToUser(user));
            FormsAuthentication.SetAuthCookie(user.Email, false);
            return RedirectToAction("Index", "Home");
        }

        private static User ToUser(UserViewModel userViewModel)
        {
            var hashData = PasswordHash.CreateHash(userViewModel.Password);
            return new User
                       {
                           Name = userViewModel.Name,
                           Company = userViewModel.Company,
                           Email = userViewModel.Email,
                           Password = hashData.Hash,
                           Salt = hashData.Salt
                       };
        }
    }
}