using CaptchaMvc.HtmlHelpers;
using InformationDetails.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace InformationDetails.Controllers
{

    [Custom_Exception_filter]
    public class ApplyForJobController : Controller
    {
        InformationDetailsEntities informationDetailsEntities = new InformationDetailsEntities();
        // GET: ApplyForJob
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult ApplyJob()
        {
            return View();
        }


        [HttpPost]
        public ActionResult ApplyJob(HttpPostedFileBase file, UserFile applyjob)
        {
            UserFile files = new UserFile();
            try
            {
                //if (this.IsCaptchaValid("Captcha  valid"))
                //{

                if (ModelState.IsValid)
                {
                    if (file == null)
                    {
                        ModelState.AddModelError("File", "Please Upload Your file");
                    }
                    else if (file.ContentLength > 0)
                    {
                        int MaxContentLength = 1024 * 1024 * 3; //3 MB
                        string[] AllowedFileExtensions = new string[] { ".jpg", ".gif", ".png", ".pdf", ".docx", ".txt" };

                        if (!AllowedFileExtensions.Contains(file.FileName.Substring(file.FileName.LastIndexOf('.'))))
                        {
                            ModelState.AddModelError("File", "Please file of type: " + string.Join(", ", AllowedFileExtensions));
                        }

                        else if (file.ContentLength > MaxContentLength)
                        {
                            ModelState.AddModelError("File", "Your file is too large, maximum allowed size is: " + MaxContentLength + " MB");
                        }
                        else
                        {
                            //TO:DO
                            var fileName = Path.GetFileName(file.FileName);
                            System.Guid newid = System.Guid.NewGuid();
                            var path = Path.Combine(Server.MapPath("~/UserFile"), fileName + newid);
                            file.SaveAs(path);

                            files.FullName = applyjob.FullName;
                            files.Email = applyjob.Email;
                            files.MobileNumber = applyjob.MobileNumber;
                            // files.TotalProject = applyjob.TotalProject;
                            files.Message = applyjob.Message;
                            files.FilePath = path;
                            files.Age = applyjob.Age;
                            files.Residence = applyjob.Residence;
                            files.Address = applyjob.Address;
                            informationDetailsEntities.UserFiles.Add(files);
                            informationDetailsEntities.SaveChanges();
                            ModelState.Clear();
                            ViewBag.Message = "File uploaded successfully";
                        }
                    }
                }
                return RedirectToAction("ExperienceAndEducation");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }





        [HttpGet]
        public ActionResult ExperienceAndEducation()
        {
            return View();
        }


        [HttpPost]
        public ActionResult ExperienceAndEducation(ExperienceAndEducation experienceAndEducation)
        {
            if (ModelState.IsValid)
            {
                ExperienceAndEducation experienceAnd = new ExperienceAndEducation();


                experienceAnd.Education_1st = experienceAndEducation.Education_1st;
                experienceAnd.Education_2st = experienceAndEducation.Education_2st;
                experienceAnd.Education_3st = experienceAndEducation.Education_3st;
                experienceAnd.Technology_1st = experienceAndEducation.Technology_1st;
                experienceAnd.Technology_2st = experienceAndEducation.Technology_2st;
                experienceAnd.Technology_3st = experienceAndEducation.Technology_3st;
                informationDetailsEntities.ExperienceAndEducations.Add(experienceAnd);
                informationDetailsEntities.SaveChanges();
                return RedirectToAction("Login", "UserLogin");

            }
            return View();
        }





        public JsonResult IsUserEmailAvailable(UserFile userFile)
        {
            List<UserLogin> emailckeck = informationDetailsEntities.UserLogins.ToList();

            return Json(emailckeck.Exists(u => u.EmailAddress.ToUpper() == userFile.Email.ToUpper()), JsonRequestBehavior.AllowGet);
        }


        //[HttpGet]
        //public ActionResult DownloadResume(int id)
        //{
        //    return View();
        //}

        public ActionResult DownloadResume()
        {
            string Currentuseemail = Utility.userLogin.EmailAddress;
            List<UserFile> users = informationDetailsEntities.UserFiles.ToList();
            //var filesCol = obj.GetFiles();
            string CurrentFileName = (from fls in users
                                      where fls.Email == Currentuseemail
                                      select fls.FilePath).FirstOrDefault();
            if (CurrentFileName != null)
            {
                string contentType = string.Empty;

                if (CurrentFileName.Contains(".pdf"))
                {
                    contentType = "application/pdf";
                }

                else if (CurrentFileName.Contains(".docx"))
                {
                    contentType = "application/docx";
                }
                else if (CurrentFileName.Contains(".jpg"))
                {
                    contentType = "application/jpg";
                }
                else if (CurrentFileName.Contains(".txt"))
                {
                    contentType = "application/txt";
                }
                else if (CurrentFileName.Contains(".xlsx"))
                {
                    contentType = "application/xlsx";
                }
                else
                {
                    ViewBag.Msg = "This User Resume Not Found";
                    return View("~\\Views\\UserLogin\\Portfolio.cshtml");

                }
                return File(CurrentFileName, contentType, CurrentFileName);
            }

            ViewBag.Msg = "This User Resume Not Found";
            return View("~\\Views\\UserLogin\\Portfolio.cshtml");
        }
    }

}