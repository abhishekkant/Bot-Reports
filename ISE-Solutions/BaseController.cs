using log4net;
using ISE_Solutions.CustomAttribute;

using System;
using System.Web.Mvc;

namespace ISE_Solutions
{
    [CustomAuthorize]
    [CustomException]
    public class BaseController: Controller
    {


    }

    #region EXCEPTION HANDLING
    public class CustomExceptionAttribute : FilterAttribute, IExceptionFilter
    {
        private static readonly ILog log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public void OnException(ExceptionContext filterContext)
        {
            Log(filterContext.Exception);
            filterContext.ExceptionHandled = true;
            filterContext.Controller.ViewBag.OnExceptionError = "Exception filter called";
            filterContext.HttpContext.Response.Write("Exception filter called");
            var controllerName = (string)filterContext.RouteData.Values["controller"];
             var actionName = (string)filterContext.RouteData.Values["action"];
            var model = new HandleErrorInfo(filterContext.Exception, controllerName, actionName);

            filterContext.Result = new ViewResult
            {
                ViewName = "~/Views/Shared/Error.cshtml",
                // MasterName = "~/Views/Shared/_Layout.cshtml",
                // ViewData = new ViewDataDictionary<HandleErrorInfo>(model),
                //TempData = filterContext.Controller.TempData
            };


        }
        private void Log(Exception ex)
        {
            //Log4netConfig.LogingConfiguration();
           // log.Error(Log4netConfig.exceptionMessageFormat(ex));

        }
    }
    #endregion
}