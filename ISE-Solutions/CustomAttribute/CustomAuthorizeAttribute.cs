using ISE_Solutions.SessionHelper;
using System.Web.Mvc;
using System.Web.Mvc.Filters;

namespace ISE_Solutions.CustomAttribute
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
       // private static bool authenticationflag = true;
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (SessionHandler.UserID != 0)
            {

            }
            
            else 
            {
                base.OnAuthorization(filterContext); //returns to login url

                if (filterContext.Result == null || filterContext.Result is HttpUnauthorizedResult)
                {
                    filterContext.Result = new RedirectToRouteResult("Default",
                        new System.Web.Routing.RouteValueDictionary{
                        {"controller", "Login"},
                        {"action", "Index"},
                        {"returnUrl", filterContext.HttpContext.Request.RawUrl}
                        });
                }
            }

        }

       
    }
}