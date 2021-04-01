using TCCCMS.Models;
using TCCCMS.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Text.RegularExpressions;

namespace TCCCMS.Controllers
{
    public class RoleGroupController : Controller
    {
        // GET: RoleGroup
        public ActionResult RoleGroup()
        {
            GetAllRoles();
            GetAllGroupsNotInRoles();
            return View();
        }


        public void GetAllRoles()
        {
            RoleMasterBL bL = new RoleMasterBL();
            List<RoleGroup> pocoList = new List<RoleGroup>();

            pocoList = bL.GetAllRoles(/*int.Parse(Session["VesselID"].ToString())*/);
            List<RoleGroup> itmasterList = new List<RoleGroup>();

            foreach (RoleGroup up in pocoList)
            {
                RoleGroup unt = new RoleGroup();
                unt.RoleId = up.RoleId;
                unt.RoleName = up.RoleName;

                itmasterList.Add(unt);
            }

            ViewBag.Roles = itmasterList.Select(x =>
                                            new SelectListItem()
                                            {
                                                Text = x.RoleName,
                                                Value = x.RoleId.ToString()
                                            });

        }


        public void GetAllGroupsNotInRoles()
        {
            GroupMasterBL bL = new GroupMasterBL();
            List<RoleGroup> pocoList = new List<RoleGroup>();

            pocoList = bL.GetAllGroupsNotInRoles(/*int.Parse(Session["VesselID"].ToString())*/);
            List<RoleGroup> itmasterList = new List<RoleGroup>();

            foreach (RoleGroup up in pocoList)
            {
                RoleGroup unt = new RoleGroup();
                unt.GroupId = up.GroupId;
                unt.GroupName = up.GroupName;

                itmasterList.Add(unt);
            }

            ViewBag.Groups = itmasterList.Select(x =>
                                            new SelectListItem()
                                            {
                                                Text = x.GroupName,
                                                Value = x.GroupId.ToString()
                                            });

        }



        public JsonResult SaveRoleGroup(RoleGroup pOCO)
        {
            RoleMasterBL bL = new RoleMasterBL();
            RoleGroup pC = new RoleGroup();

            pC.RoleId = pOCO.RoleId;
            pC.GroupId = pOCO.GroupId;

            return Json(bL.SaveRoleGroup(pC  /*, int.Parse(Session["VesselID"].ToString())*/  ), JsonRequestBehavior.AllowGet);
        }
    }
}