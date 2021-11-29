using TCCCMS.Models;
using TCCCMS.Business;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using TCCCMS.Infrastructure;

namespace TCCCMS.Controllers
{
    [CustomAuthorizationFilter]
    public class UserGroupController : Controller
    {
        // GET: UserGroup
        [CustomAuthorizationFilter]
        public ActionResult Index()
        {
            UserGroupPOCO poco = new UserGroupPOCO();
            poco = GetAllGroupsForDrp();

            GetAllUserForDrp();
            //GetAllGroupsForDrp();
            return View(poco);
        }
        [CustomAuthorizationFilter]
        public JsonResult LoadData(int selectedUserId)
        {
            UserGroupBL bL = new UserGroupBL();
            string commaSeperatedValues = bL.GetAllCommaSeperatedUserGroupByUserId(selectedUserId);
            //var data = string.Empty;
            return Json(commaSeperatedValues, JsonRequestBehavior.AllowGet);
        }


        //for User drp
        public void GetAllUserForDrp()
        {
            UserGroupBL bL = new UserGroupBL();
            List<UserGroupPOCO> pocoList = new List<UserGroupPOCO>();

            pocoList = bL.GetAllUserForDrp(/*int.Parse(Session["VesselID"].ToString())*/);


            List<UserGroupPOCO> itmasterList = new List<UserGroupPOCO>();

            foreach (UserGroupPOCO up in pocoList)
            {
                UserGroupPOCO unt = new UserGroupPOCO();
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


        //for Group drp
        public UserGroupPOCO GetAllGroupsForDrp()
        {
            UserGroupBL bL = new UserGroupBL();
            List<UserGroupPOCO> pocoList = new List<UserGroupPOCO>();

            pocoList = bL.GetAllGroupsForDrp();
            UserGroupPOCO pOCO = new UserGroupPOCO();

            var list = new List<KeyValuePair<string, string>>();

            foreach (UserGroupPOCO up in pocoList)
            {
                UserGroupPOCO unt = new UserGroupPOCO();
                //unt.GroupId = up.GroupId;
                //unt.GroupName = up.GroupName;



                list.Add(new KeyValuePair<string, string>(up.GroupId.ToString(), up.GroupName));

                //itmasterList.Add(unt);
            }

            pOCO.Groups = list;

            //ViewBag.Groups = itmasterList.Select(x =>
            //                                new SelectListItem()
            //                                {
            //                                    Text = x.GroupName,
            //                                    Value = x.GroupId.ToString()
            //                                });

            return pOCO;
        }


        [CustomAuthorizationFilter]
        public JsonResult SaveUserGroupMapping(int userId, string[] selectedGroups)
        {
            var data = string.Empty;
            string selectedGroupIDs = string.Empty;
            if (selectedGroups.Length > 0 && selectedGroups[0] != ",")
                 selectedGroupIDs = String.Join(",", selectedGroups);
            UserGroupBL bL = new UserGroupBL();
           int records= bL.SaveUserGroupMapping(userId, selectedGroupIDs);

            return Json(records, JsonRequestBehavior.AllowGet);
        }




    }
}