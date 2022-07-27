using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using ProjectBase.Utilities;
using ProjectBase.ViewModels;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.DirectoryServices.AccountManagement;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace ProjectBase.Controllers
{
    public class AuthController : Controller
    {
        ProjectBaseEntities db = new ProjectBaseEntities();

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model, string returnUrl)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            UserPrincipal adUserPrincipal;

            string adResult = ADSignIn(model.EmployeeNumber, model.Password, out adUserPrincipal);

            if (adResult == "Wrong password!")
            {
                ModelState.AddModelError(Constants._ERROR, adResult);
            }
            else if (adResult == "OK")
            {
                User user = db.Users.Where(r => r.Employee_Number == model.EmployeeNumber).SingleOrDefault();
                if (user != null)
                {
                    user.Last_Access_Date = DateTime.Now;
                    db.Entry(user).State = System.Data.Entity.EntityState.Modified;
                    db.SaveChanges();

                    SignInAsync(user, true);
                    FormsAuthentication.SetAuthCookie(model.EmployeeNumber, model.RememberMe);
                    return RedirectToLocal(returnUrl);
                }
                else
                {
                    ModelState.AddModelError(Constants._ERROR, "[" + model.EmployeeNumber + "] You are not registered to local account yet.<br/>Please contact administrator.");
                }
            }
            else if (adResult == "User not found!")
            {
                //string result = AjaxController.HRSystemSignIn(model.EmployeeNumber, Url.Encode(model.Password));
                //if (result == "OK")
                //{
                //    SignInAsync(model.EmployeeNumber);
                //    FormsAuthentication.SetAuthCookie(model.EmployeeNumber, model.RememberMe);
                //    return RedirectToLocal(returnUrl);
                //}
                //else
                //{
                //    ModelState.AddModelError(Constants._ERROR, result);
                //}

                User user = db.Users.Where(r => r.Employee_Number == model.EmployeeNumber).SingleOrDefault();

                if (user != null)
                {
                    if (user.Password == model.Password)
                    {
                        user.Last_Access_Date = DateTime.Now;
                        db.Entry(user).State = System.Data.Entity.EntityState.Modified;

                        SignInAsync(user, false);
                        FormsAuthentication.SetAuthCookie(model.EmployeeNumber, model.RememberMe);
                        return RedirectToLocal(returnUrl);
                    }
                    else
                    {
                        ModelState.AddModelError(Constants._ERROR, "Wrong password!");
                    }
                }
                else
                {
                    ModelState.AddModelError(Constants._ERROR, "[" + model.EmployeeNumber + "] You are not registered to local account yet.<br/>Please contact administrator.");
                }
            }
            else
            {
                ModelState.AddModelError(Constants._ERROR, adResult);
            }

            return View(model);
        }

        private void SignInAsync(string userid)
        {
            var claims = new List<Claim>();
            User user = db.Users.Where(r => r.Employee_Number == userid).SingleOrDefault();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, userid));
            claims.Add(new Claim(Constants.CLAIM_EMPLOYEE_NUMBER, userid));
            claims.Add(new Claim(Constants.CLAIM_ID, user.Id.ToString()));
            claims.Add(new Claim(Constants.CLAIM_FULLNAME, string.IsNullOrEmpty(user.Full_Name) ? "" : user.Full_Name));
            claims.Add(new Claim(Constants.CLAIM_EMAIL, string.IsNullOrEmpty(user.Email) ? "" : user.Email));
            claims.Add(new Claim(Constants.CLAIM_IS_ADMIN, user.Is_Admin ? "1" : "0"));

            //var userData = AjaxController.GetEmployeeDataFromHRSystemAsDictionary(userid);
            //foreach (string s in Constants.dataKey)
            //{
            //    string val = "";
            //    if (userData.TryGetValue(s, out val))
            //        claims.Add(new Claim(s, val));
            //}         
            var id = new ClaimsIdentity(claims,
                                        DefaultAuthenticationTypes.ApplicationCookie);
            var ctx = Request.GetOwinContext();
            var authenticationManager = ctx.Authentication;
            authenticationManager.SignIn(id);
        }

        private void SignInAsync(User user, bool loginByAD)
        {
            var claims = new List<Claim>();
            claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Employee_Number));
            claims.Add(new Claim(Constants.CLAIM_EMPLOYEE_NUMBER, user.Employee_Number));
            claims.Add(new Claim(Constants.CLAIM_ID, user.Id.ToString()));
            claims.Add(new Claim(Constants.CLAIM_NAME, user.Full_Name.Split(new string[] { " " }, StringSplitOptions.None)[0]));
            claims.Add(new Claim(Constants.CLAIM_FULLNAME, user.Full_Name));
            claims.Add(new Claim(Constants.CLAIM_EMAIL, string.IsNullOrEmpty(user.Email) ? "" : user.Email));
            claims.Add(new Claim(Constants.CLAIM_IS_ADMIN, user.Is_Admin ? "1" : "0"));

            //Generate Menu
            string menuJson = JsonConvert.SerializeObject(UMenu.GetAuthorizedMenu(user.Employee_Number, user.Is_Admin ? true : false));
            claims.Add(new Claim(Constants.CLAIM_MENU, menuJson));

            claims.Add(new Claim(Constants.CLAIM_LOGIN_BY_AD, loginByAD ? "1" : "0"));

            var id = new ClaimsIdentity(claims,
                                        DefaultAuthenticationTypes.ApplicationCookie);
            var ctx = Request.GetOwinContext();
            var authenticationManager = ctx.Authentication;
            authenticationManager.SignIn(id);
        }

        private string ADSignIn(string user, string password, out UserPrincipal userData)
        {
            userData = null;
            try
            {
                using (PrincipalContext ctx = new PrincipalContext(ContextType.Domain, Constants.AD_DOMAIN, Constants.AD_SA_USER, Constants.AD_SA_PASSWORD))
                {
                    userData = UserPrincipal.FindByIdentity(ctx, user);
                    if (userData == null)
                        return "User not found!";
                    if (userData.IsAccountLockedOut())
                        return "Account is locked. Please wait for 15 minutes or contact local IT";
                    if (userData.AccountExpirationDate.HasValue)
                    {

                        if (userData.AccountExpirationDate.Value >= DateTime.Now)
                            return "Password expired. <a href='https://adpass.wilmar.co.id/RDWeb/Pages/en-US/password.aspx' target='_blank' style='text-decoration:underline'>Click here to change password</a>";
                    }
                    if (userData.LastPasswordSet == null)
                    {
                        return "Password must be change at first login. <a href='https://adpass.wilmar.co.id/RDWeb/Pages/en-US/password.aspx' target='_blank' style='text-decoration:underline'>Click here to change password</a>.";
                    }

                    //else if ((DateTime.Now - userData.LastPasswordSet.Value.AddHours(7)).TotalHours < 24)
                    //    return "Password has been changed today at " + userData.LastPasswordSet.Value.AddHours(7).ToString("HH:mm") + ", please try again tomorrow.";
                    if (userData.Enabled.HasValue)
                        if (userData.Enabled.Value == false) return "Account has been terminated. Please contact local IT";
                    if (!ctx.ValidateCredentials(user, password))
                        return "Wrong password. Please check your password and try again. If you enter wrong password 3 times, your account will be locked for 15 minutes.";
                    else
                        return "OK";
                }
            }
            catch (Exception exc)
            {
                return exc.Message;
            }
        }

        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            var ctx = Request.GetOwinContext();
            var authenticationManager = ctx.Authentication;
            authenticationManager.SignOut();
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}