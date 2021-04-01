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

        public ActionResult GroupUser()
        {
            GetAllGroupMaster();
            GetAllUser();
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




        public void GetAllGroupMaster()
        {
            GroupMasterBL bL = new GroupMasterBL();
            List<GroupUser> pocoList = new List<GroupUser>();

            pocoList = bL.GetAllGroupMaster(/*int.Parse(Session["VesselID"].ToString())*/);
            List<GroupUser> itmasterList = new List<GroupUser>();

            foreach (GroupUser up in pocoList)
            {
                GroupUser unt = new GroupUser();
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


        public JsonResult GetRoleByGroupId(int GroupId)
        {
            RoleMasterBL bL = new RoleMasterBL();
            RoleMasterPOCO pOCOList = new RoleMasterPOCO();

            pOCOList = bL.GetRoleByGroupId(GroupId);

            RoleMasterPOCO dept = new RoleMasterPOCO();

            dept.RoleId = pOCOList.RoleId;
            dept.RoleName = pOCOList.RoleName;

            var data = dept;

            return Json(data, JsonRequestBehavior.AllowGet);
        }



        public void GetAllUser()
        {
            UserMasterBL bL = new UserMasterBL();
            List<GroupUser> pocoList = new List<GroupUser>();

            pocoList = bL.GetAllUser(/*int.Parse(Session["VesselID"].ToString())*/);
            List<GroupUser> itmasterList = new List<GroupUser>();

            foreach (GroupUser up in pocoList)
            {
                GroupUser unt = new GroupUser();
                unt.UserId = up.UserId;
                unt.UserName = up.UserName;

                itmasterList.Add(unt);
            }

            ViewBag.Users = itmasterList.Select(x =>
                                            new SelectListItem()
                                            {
                                                Text = x.UserName,
                                                Value = x.UserId.ToString()
                                            });

        }


        public JsonResult SaveUserGroupMapping(int userId, string userGroupMapping)
        {
            UserGroupBL bL = new UserGroupBL();         
            return Json(bL.SaveUserGroupMapping(userId, userGroupMapping), JsonRequestBehavior.AllowGet);
        }




    }
}