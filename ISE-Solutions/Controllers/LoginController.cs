using System.Web.Mvc;
using System.Web.Security;
using DataLayer.Interface;
using ISE_Solutions.Model;
using ISE_Solutions.SessionHelper;
using DataLayer;
namespace ISE_Solutions.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login

        IConfiguration configuration = null;

        public LoginController()
        {
            this.configuration = new ConfigurationDBL();
        }

        public ActionResult Index()
        {
            if (SessionHelper.SessionHandler.UserID != 0)
            {
                return RedirectToAction("Dashboard", "Home");
            }
            else
                return RedirectToAction("Login");
        }
        [HttpGet]
        public ActionResult Login()
        {

        
            return View();
        }

        public ActionResult getLogin(HomeLoginViewModel objLogin)
        {
            HomeController _obj = new HomeController();
            if (ModelState.IsValid)
            {
                bool _result = false;
                _result = _obj.UserAuthentication(objLogin.Username, objLogin.Password); ///objLogin.Password.Encrypt()
                if (_result == true)
                {
                    SessionHandler.UserID = 12344;
                    SessionHandler.UserName = "Admin";
                    SessionHandler.UserRole = "Admin";
                    SessionHandler.RoleID = 1;
                    return RedirectToAction("Dashboard", "Home");
                }
                else
                {
                    ViewData["Message"] = "Invalid username or password";

                }

            }
            else
            {
                ViewData["Message"] = "";
            }


            return View("Login");
        }
        public ActionResult Logout()
        {
            Session.Abandon();
            FormsAuthentication.SignOut();
            return RedirectToAction("Login","Login");
        }
    }
}