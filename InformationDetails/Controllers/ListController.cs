using InformationDetails.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace InformationDetails.Controllers
{

    [Custom_Exception_filter]
    public class ListController : Controller
    {
        static public List<UserList> userLists = new List<UserList>();

        List<UserList> Lists;
        // GET: List

        [HttpGet]
        public ActionResult Index()
        {
            if (userLists.Count == 0)
            {
                return View();
            }
            ViewBag.datasource = userLists;
            return View(userLists);
        }


        [HttpPost]
        public ActionResult Index(string searchBy, string Searchtext)
        {
            if (searchBy == "CustomerName")
            {
                Lists = userLists.Where(x => x.CustomerName.StartsWith(Searchtext) || Searchtext == null).ToList();
            }
            else 
            {
                Lists = userLists.Where(x => x.City.StartsWith(Searchtext) || Searchtext == null).ToList();
            }
           
            return View(Lists);
        }

       

     
        [HttpGet]
        public ActionResult Insert()
        {
            return View();
        }
       
        [HttpPost]
        public ActionResult Insert(UserList user)
        {
            if (ModelState.IsValid)
            {
                if (userLists.Exists(x => x.CustomerName == user.CustomerName))
                {
                    ViewBag.Message = "CustomerName is allrady in database";

                }
                else
                {
                    //list1.Add(new mentis { Id = txtid.Text, Name = txtname.Text, City = txtcity.Text, Email_id = txtmail.Text, Mo_number = txtMo_number.Text });
                    userLists.Add(new UserList { Id = user.Id, CustomerName = user.CustomerName, CustomerAddress = user.CustomerAddress, City = user.City, Phone = user.Phone, Email = user.Email });

                }
                //return View();
            }

            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Edit(int? id)
        {

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            UserList UserList = userLists.Find(x => x.Id == id);

            if (UserList == null)
            {
                return HttpNotFound();
            }
            return View(UserList);
        }

        //[HttpPost]
        //public ActionResult Edit([Bind(Include = "Id,CustomerName,Email,City,CustomerAddress,Phone")]UserList userList)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        UserList user = (from c in userLists
        //                         where c.Id == userList.Id
        //                         select c).FirstOrDefault();
        //        user.CustomerName = userList.CustomerName;
        //        user.Email = userList.Email;
        //        user.City = userList.City;
        //        user.CustomerAddress = userList.CustomerAddress;
        //        user.Phone = userList.Phone;
        //    }
        //    return RedirectToAction("Index");
        //}

        [HttpPost]
        public ActionResult Edit(UserList userList)
        {
            if (ModelState.IsValid)                                     //// Update List Data use List Concept.
            {
                UserList List = userLists.FirstOrDefault(x => x.Id == userList.Id);
                List.CustomerName = userList.CustomerName;
                List.CustomerAddress = userList.CustomerAddress;
                List.Email = userList.CustomerAddress;
                List.Phone = userList.Phone;
                List.City = userList.City;
            }

            return RedirectToAction("Index");
        }





        [HttpGet]
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            UserList user = userLists.Find(x => x.Id == id);
            if (user == null)
            {
                return HttpNotFound();
            }

            return View(user);

        }

        [HttpPost]
        public ActionResult Delete(UserList user)
        {                                                                        //////  List data delete use only list concept
            user = userLists.FirstOrDefault(x => x.Id == user.Id);
            userLists.Remove(user);
            return RedirectToAction("Index");
        }

        [AllowAnonymous]
        [HttpPost]
        public JsonResult IsUserExists(string UserName)
        {
            //check if any of the UserName matches the UserName specified in the Parameter using the ANY extension method.  
            return Json(!userLists.Any(x => x.CustomerName == UserName), JsonRequestBehavior.AllowGet);
        }
    }
}