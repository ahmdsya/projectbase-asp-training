using ProjectBase.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ProjectBase
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        private string currentMenu = "", currentAction = "", menuCategory = "";

        public CustomAuthorizeAttribute(string menuCode = "", string action = "", string menuCategory = "")
        {
            currentMenu = menuCode;
            currentAction = action;
            this.menuCategory = menuCategory;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            bool valid = true;

            if (BaseProgram.UserAuthenticated())
            {
                string controllerName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName;
                string actionName = filterContext.ActionDescriptor.ActionName;
                var requests = filterContext.ActionDescriptor.GetParameters()
                            .Select(r => new
                            {
                                Key = r.ParameterName,
                                Value = filterContext.HttpContext.Request[r.ParameterName]
                            }).ToDictionary(r => r.Key);
                string mode = "";
                string username = System.Web.HttpContext.Current.User.Identity.GetUserDataByKey(Constants.CLAIM_EMPLOYEE_NUMBER);

                if (string.IsNullOrEmpty(username))
                {
                    filterContext.Result = new RedirectToRouteResult(new
                        RouteValueDictionary(new { controller = "Auth", action = "Login" }));
                    return;
                }
                else
                {
                    if (System.Web.HttpContext.Current.User.Identity.GetUserDataByKey(Constants.CLAIM_IS_ADMIN) == "1")
                        valid = true;
                    else
                    {
                        if (requests.Count() > 0 && requests.ContainsKey("mode"))
                        {
                            mode = requests["mode"].Value;
                        }

                        valid = CheckAuthorization(username, actionName, controllerName, mode);
                    }
                }

                if (!valid)
                {
                    filterContext.Result = new RedirectToRouteResult(new
                        RouteValueDictionary(new { controller = "Home", action = "AccessDenied" }));
                }

            }
            else
            {
                filterContext.Result = new RedirectToRouteResult(new
                    RouteValueDictionary(new { controller = "Auth", action = "Login" }));
            }
        }

        public static bool CheckAuthorization(string username = "", string actionName = "", string controllerName = "", string mode = "")
        {
            Group_Menu groupMenu = null;
            if (actionName == "Editor") actionName = "Index";

            bool hasAuth = UUtils.UserHasAuthorization(username, actionName, controllerName, out groupMenu);

            if (hasAuth)
            {
                if (string.IsNullOrEmpty(mode))
                {
                    return true;
                }
                else
                {
                    if (mode == Constants.FORM_MODE_CREATE) return groupMenu.Create;
                    else if (mode == Constants.FORM_MODE_EDIT) return groupMenu.Edit;
                    else if (mode == Constants.FORM_MODE_DELETE) return groupMenu.Delete;
                    else if (mode == Constants.FORM_MODE_VIEW) return groupMenu.View;
                    else if (mode == Constants.FORM_MODE_COPY) return groupMenu.Create;
                }
            }

            return false;
        }
    }


}