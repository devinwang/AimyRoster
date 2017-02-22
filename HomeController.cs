using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AimyRoster.Models;
using AimyRoster.ViewModel;
using System.Data.Entity.Core.Objects;
using System.Data.Entity;

namespace AimyRoster.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult ListSite()
        {
            var db = new SiteEntities();
            var sites = new List<SiteViewModel>();

            sites = (from org in db.Orgs
                     join lookup in db.Lookups
                     on org.TypeId equals lookup.Id
                     where lookup.Description == "Site"
                     select new SiteViewModel
                     {
                         siteName = org.Name
                         //siteId
                     }).ToList();

            return Json(sites, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ListStaff(int siteId)
        {
            var db = new SiteEntities();
            var staffs = new List<StaffViewModel>();

            staffs = (from org in db.Orgs
                      join org_user in db.Org_User
                      on org.Id equals org_user.OrgId
                      join user in db.Users
                      on org_user.UserId equals user.Id
                      join contact in db.Contacts
                      on user.ContactId equals contact.Id
                      join lookup in db.Lookups
                      on user.RoleId equals lookup.Id
                      where org.TypeId == 6 && lookup.Description != "parent" && org.Id == 8
                      select new StaffViewModel
                      {
                          staffName = contact.FirstName + " " + contact.LastName
                      }).ToList();
            return Json(staffs, JsonRequestBehavior.AllowGet);
        }

        //ListSchedule? userId = 4131 & orgId = 8 & dateTime = 08 - 05 - 2015
        public ActionResult ListSchedule(int userId, int orgId, DateTime dateTime)
        {
            var db = new SiteEntities();
            var schedules = new List<ScheduleViewModel>();

            schedules = (
                      from user in db.Users
                      join contact in db.Contacts
                      on user.ContactId equals contact.Id
                      join lookup in db.Lookups
                      on user.RoleId equals lookup.Id
                      join staffroster in db.StaffRosters
                      on user.Id equals staffroster.StaffId
                      join org_user in db.Org_User
                      on user.Id equals org_user.UserId
                      join org in db.Orgs
                      on org_user.OrgId equals org.Id
                      where (user.Id == userId) && (org.Id == orgId) && (DbFunctions.TruncateTime(staffroster.StartDate) == dateTime)
                      select new ScheduleViewModel
                      {
                          startTime = staffroster.StartDate,
                          endTime = staffroster.EndDate
                      }).ToList();
            return Json(schedules, JsonRequestBehavior.AllowGet);
        }

    }
}