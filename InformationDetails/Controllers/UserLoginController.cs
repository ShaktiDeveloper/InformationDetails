using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using InformationDetails.Models;
using System.Net;
using System.Web.Security;
using System.Net.Mail;
using System.Web.Helpers;

namespace InformationDetails.Controllers
{

    [Custom_Exception_filter]
    public class UserLoginController : Controller
    {
        InformationDetailsEntities informationDetailsEntities = new InformationDetailsEntities();
        List<UserRole> roles;

        UserFile GetUser;
       
        // GET: UserLogin
        public ActionResult Index()
        {
            return View();
        }


        [HttpGet]
        public ActionResult Login()
        {
            return View();
        }


        [Custom_Exception_filter]
        [HttpPost]
        public ActionResult Login(string EmailAddress, string Password)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    //var rows = from myRow in informationDetailsEntities.UserLogins.ToList()
                    //           select myRow;
                    string password = Crypto.Hash(Password);
                    List<UserFile> userfiles = informationDetailsEntities.UserFiles.ToList();
                    List<UserLogin> userLogins = informationDetailsEntities.UserLogins.ToList();
                    Utility.userLogin = userLogins.FindAll(x => x.EmailAddress.Equals(EmailAddress) && x.password.Equals(password)).FirstOrDefault();
                    GetUser = userfiles.FindAll(x => x.Email == EmailAddress).FirstOrDefault();

                    if (Utility.userLogin != null)
                    {
                        Session["Name"] = Utility.userLogin.Fullname;
                        Session["MobileNumber"] = Convert.ToString(GetUser.MobileNumber);
                        Session["Email"] =Convert.ToString(GetUser.Email);
                        Session["Age"] = Convert.ToString(GetUser.Age);
                        Session["Residence"] = Convert.ToString(GetUser.Residence);
                        Session["Address"] = Convert.ToString(GetUser.Address);
                        FormsAuthentication.SetAuthCookie(Utility.userLogin.Fullname, false);
                        //return RedirectToAction("GetDetails");
                        return RedirectToAction("Portfolio", "UserLogin");
                    }
                    ViewBag.Message = "Invalid EmailAddress or Password";

                    //foreach (UserLogin login in userLogins)
                    //{
                    //    if (EmailAddress == login.EmailAddress && Crypto.Hash(Password) == login.password)
                    //    {

                    //    }
                    //}

                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
                return View();
            }
            

        [Authorize]
        [HttpGet]
        public ActionResult LoginDashboard()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult LoginDashboard(UserLogin userLogin)
        {
            return View();
        }

        [Authorize]
        [HttpGet]
        public ActionResult Portfolio(UserLogin userLogin)
        {
            //ViewBag.Data = Utility.userLogin.EmailAddress;
            return View();
        }

        [Authorize]
        [HttpPost]
        public ActionResult Portfolio()
        {
            return View();
        }


        [HttpGet]
        public ActionResult Registration()
        {
            List<UserRole> roles = informationDetailsEntities.UserRoles.ToList();
            //List<SelectListItem> item = new List<SelectListItem>();
            //foreach (var role in UserRole)
            //{
            //    item.Add(new SelectListItem
            //    {
            //        Text = role.RoleName,
            //        Value = role.Id.ToString()
            //    });
            //}
            //ViewBag.Name = item;
            ViewBag.Name=(from c in roles select new { c.Id, c.RoleName }).Distinct();
            // ViewData["Name"] = new SelectList(informationDetailsEntities.UserRoles, "Id", "RoleName");
            // ViewBag.ProcedureID = new SelectList(db.ProcedureCategory, "ID", "Name");
            // userLogin.RoleId = new SelectList(informationDetailsEntities.UserRoles, "Id", "RoleName");
            // ViewBag.Name = new SelectList(informationDetailsEntities.UserRoles.Select(u => u.RoleName).ToList(), "Id", "RoleName");
            //ViewBag.Name = new SelectList(UserRole, "Id", "RoleName");
            return View();
        }
        
        [HttpPost]
        public ActionResult Registration(UserLogin userLogin)
        {
            List<UserRole> roles = informationDetailsEntities.UserRoles.ToList();
            UserRole role = new UserRole();

            role = roles.Where(x => x.Id == userLogin.RoleId).FirstOrDefault();
            ViewBag.Name = (from c in roles select new { c.Id, c.RoleName }).Distinct();
      
            if (ModelState.IsValid)
            {
                UserLogin detali = new UserLogin();

                bool detalinew = informationDetailsEntities.UserLogins.Any(x => x.Fullname == userLogin.Fullname);
                if (detalinew)
                {
                    ViewBag.CustomerName = "<b>CustomerName is allrady in database</b>";
                    //ModelState.AddModelError("CustomerName", "CustomerName is allrady in database");

                }
                else
                {
                    detali.Fullname = userLogin.Fullname;
                    detali.EmailAddress = userLogin.EmailAddress;
                    detali.password = Crypto.Hash(nullcheck(userLogin.password));
                    detali.RoleId = userLogin.RoleId;
                    informationDetailsEntities.UserLogins.Add(detali);
                    informationDetailsEntities.SaveChanges();
                    //return RedirectToAction("Login");
                    return RedirectToAction("Login", "UserLogin");
                }
            }
            return View();
        }

        private string nullcheck(string password)
        {
            if(password !=null)
            {
                return password;
            }
            else
            {
                password = string.Empty;
                
            }
            return password;
        }

        [HttpGet]
        public ActionResult AddRole()
        {

            return View();
        }
      
        [HttpPost]
        public ActionResult AddRole(UserRole user)
        {
            UserRole roles = new UserRole();
            if (ModelState.IsValid)
            {
               bool role = informationDetailsEntities.UserRoles.Any(x => x.RoleName == user.RoleName);
               if (role)
               {
                    ViewBag.Message = "RoleName is allrady in database";
               }
                else
                {
                    roles.RoleName = user.RoleName;
                    informationDetailsEntities.UserRoles.Add(roles);
                    informationDetailsEntities.SaveChanges();
                }
                return RedirectToAction("Registration", "UserLogin");
            }
            return View();
        }

        public ActionResult Logout()
        {
            Session.Abandon();
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            FormsAuthentication.SignOut();
            Session.RemoveAll();
            Session["Name"] = null;
            Session.Clear();
            string[] myCookies = Request.Cookies.AllKeys;
            foreach (string cookie in myCookies)
            {
                Response.Cookies[cookie].Expires = DateTime.Now.AddDays(-1);
            }
            //Session.Remove("Name");
            //Session.Clear();
            //Session.Abandon();
            //Response.Cookies.Clear();
            //FormsAuthentication.SignOut();
            //window.localStorage.clear();
            //sessionStorage.clear();
            // it will clear the session at the end of request
            return RedirectToAction("Login", "UserLogin"); ;

        }

        
        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }
      
        [HttpPost]
        public ActionResult ForgotPassword(string EmailID)
        {

            //Verify Email ID
            //Generate Reset password link 
            //Send Email 
            string message = "";
            bool status = false;

            using (InformationDetailsEntities dc = new InformationDetailsEntities())
            {
                var account = dc.UserLogins.Where(a => a.EmailAddress == EmailID).FirstOrDefault();
                if (account != null)
                {
                    //Send email for reset password
                    string resetCode = Guid.NewGuid().ToString();
                    SendVerificationLinkEmail(account.EmailAddress, resetCode, "ResetPassword");
                    account.ResetPasswordCode = resetCode;
                    //This line I have added here to avoid confirm password not match issue , as we had added a confirm password property 
                    //in our model class in part 1
                    dc.Configuration.ValidateOnSaveEnabled = false;
                    dc.SaveChanges();
                    message = "Reset password link has been sent to your email id.";
                }
                else
                {
                    message = "Account not found";
                }
            }
            ViewBag.Message = message;
            return View();
        }

        
        [NonAction]
        public void SendVerificationLinkEmail(string emailID, string activationCode, string emailFor = "VerifyAccount")
        {
            var verifyUrl = "/UserLogin/" + emailFor + "/" + activationCode;
            var link = Request.Url.AbsoluteUri.Replace(Request.Url.PathAndQuery, verifyUrl);

            var fromEmail = new MailAddress("ShaktiRai914@gmail.com", "ShaktiRai");
            var toEmail = new MailAddress(emailID);
            var fromEmailPassword = "9044068891";

            string subject = "";
            string body = "";
            if (emailFor == "VerifyAccount")
            {
                subject = "Your account is successfully created!";
                body = "<br/><br/>We are excited to tell you that your account is" +
                    " successfully created. Please click on the below link to verify your account" +
                    " <br/><br/><a href='" + link + "'>" + link + "</a> ";
            }
            else if (emailFor == "ResetPassword")
            {
                subject = "Reset Password";
                body = "Hi,<br/>br/>We got request for reset your account password. Please click on the below link to reset your password" +
                    "<br/><br/><a href=" + link + ">Reset Password link</a>";
            }

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromEmail.Address, fromEmailPassword)
            };

            using (var message = new MailMessage(fromEmail, toEmail)
            {
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            })
                smtp.Send(message);
        }

        
        [HttpGet]
        public ActionResult ResetPassword(string id)
        {
            //Verify the reset password link
            //Find account associated with this link
            //redirect to reset password page
            if (string.IsNullOrWhiteSpace(id))
            {
                return HttpNotFound();
            }

            using (InformationDetailsEntities dc = new InformationDetailsEntities())
            {
                var user = dc.UserLogins.Where(a => a.ResetPasswordCode == id).FirstOrDefault();
                if (user != null)
                {
                    ResetPasswordModel model = new ResetPasswordModel();
                    model.ResetCode = id;
                    return View(model);
                }
                else
                {
                    return HttpNotFound();
                }
            }
        }
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ResetPassword(ResetPasswordModel model)
        {
            var message = "";
            //if (ModelState.IsValid)
            //{

            if (model.NewPassword == model.ConfirmPassword)
            {
                using (InformationDetailsEntities dc = new InformationDetailsEntities())
                {
                    var user = dc.UserLogins.Where(a => a.ResetPasswordCode == model.ResetCode).FirstOrDefault();
                    if (user != null)
                    {
                        user.password = Crypto.Hash(model.NewPassword);
                        user.ResetPasswordCode = "";
                        dc.Configuration.ValidateOnSaveEnabled = false;
                        dc.SaveChanges();
                        message = "New password updated successfully";
                    }
                }
            }
            else
            {
                message = "New password and confirm password does not match";
            }
            //}
            //else
            //{
            //    message = "Something invalid";
            //}
            ViewBag.Message = message;
            return View(model);
        }


    }
}