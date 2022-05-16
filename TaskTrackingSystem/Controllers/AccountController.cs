using System.Web.Mvc;
using System.Web.Security;
using TaskTrackingSystem.CustomAuthorizeAttribute;
using TaskTrackingSystem.Shared.Entities;
using TaskTrackingSystem.UI.Web.Models;

namespace TaskTrackingSystem.UI.Web.Controllers
{
    public class AccountController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            ViewBag.IsAdmin = Logic.Logic.Instance.AccountAccess.IsAdmin();
            return View("IndexJNew", AccountModel.GetAll());
        }
        
        [ControllerAccess]
        public ActionResult Create()
        {
            return View("CreateJNew", new AccountModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ControllerAccess]
        public ActionResult Create(AccountModel model)
        {
            if (ModelState.IsValid)
            {
                var id = Logic.Logic.Instance.Account.Create((Account)model);

                return RedirectToAction("Index");
                
            }
            return View("CreateJNew", model);
        }

        [ControllerAccess]
        public ActionResult Update(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                return View("UpdateJNew",(AccountModel)Logic.Logic.Instance.Account.Get(id));
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ControllerAccess]
        public ActionResult Update(AccountModel model)
        {
            if (model.DateBirth.Year == 1)
            {
                model.DateBirth = new System.DateTime(1900, 1, 1);
            }

            ModelState.Clear();
            ValidateModel(model);

            if (ModelState.IsValid)
            {
                var account = (Account)model;
                var response = Logic.Logic.Instance.Account.Update(account);
                return RedirectToAction("Index");
            }
            return View("UpdateJNew", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ControllerAccess]
        public ActionResult Delete(string login)
        {
            if (!string.IsNullOrEmpty(login))
            {
                Logic.Logic.Instance.Account.Delete(login);
            }

            return RedirectToAction("Index");
        }

        [Authorize]
        public ActionResult Details(string id)
        {            
            var model = AccountModel.Get(id);
            if (model is null) 
            {
                //TODO Такого пользователя не найдено
            }
            else
            {
                return View("DetailsJNew", model);
            }
            return null;
        }

        #region Login
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string login, string password, string returnUrl)
        {
            if (AccountModel.LoginSuccess(login, password))
            {
                //Имя получаем из базы ( иначе будет залогинено такое, которое было введено пользователем)
                FormsAuthentication.SetAuthCookie(Logic.Logic.Instance.Account.Get(login).Login, true);

                //TempData["Status"] = "Login success";

                return Redirect(string.IsNullOrEmpty(returnUrl) ? "~" : returnUrl);
            }
            TempData["Status"] = "Пара Логин/Пароль не найдена";
            return View();
        }

        [ChildActionOnly]
        public ActionResult UserInfo()
        {
            return PartialView();
        }

        [HttpGet]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return Redirect("~");
        }

        #endregion
    }
}