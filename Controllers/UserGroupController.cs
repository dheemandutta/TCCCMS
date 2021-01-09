using TCCCMS.Models;
using TCCCMS.Business;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;

namespace TCCCMS.Controllers
{
    public class UserGroupController : Controller
    {
        // GET: UserGroup
        public ActionResult Index()
        {
            UserGroupPOCO poco = new UserGroupPOCO();
            poco = GetAllGroupsForDrp();

            GetAllUserForDrp();
            //GetAllGroupsForDrp();
            return View(poco);
        }

        public JsonResult LoadData()
        {
            int draw, start, length;
            int pageIndex = 0;

            if (null != Request.Form.GetValues("draw"))
            {
                draw = int.Parse(Request.Form.GetValues("draw").FirstOrDefault().ToString());
                start = int.Parse(Request.Form.GetValues("start").FirstOrDefault().ToString());
                length = int.Parse(Request.Form.GetValues("length").FirstOrDefault().ToString());
            }
            else
            {
                draw = 1;
                start = 0;
                length = 500;
            }

            if (start == 0)
            {
                pageIndex = 1;
            }
            else
            {
                pageIndex = (start / length) + 1;
            }

            UserGroupBL bL = new UserGroupBL(); ///////////////////////////////////////////////////////////////////////////
            int totalrecords = 0;

            List<UserGroupPOCO> pocoList = new List<UserGroupPOCO>();
            pocoList = bL.GetAllUserGroupPageWise(pageIndex, ref totalrecords, length/*, int.Parse(Session["VesselID"].ToString())*/);
            List<UserGroupPOCO> pList = new List<UserGroupPOCO>();
            foreach (UserGroupPOCO pC in pocoList)
            {
                UserGroupPOCO pOCO = new UserGroupPOCO();
               // pOCO.UserGroupId = pC.UserGroupId;
                pOCO.UserId = pC.UserId;
                pOCO.UserName = pC.UserName;
                pOCO.SelectedGroups = pC.SelectedGroups;
                //pOCO.CreatedBy = pC.CreatedBy;
                //pOCO.ModifiedBy = pC.ModifiedBy;

                pList.Add(pOCO);
            }

            var data = pList;
            return Json(new { draw = draw, recordsFiltered = totalrecords, recordsTotal = totalrecords, data = data }, JsonRequestBehavior.AllowGet);
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








    }
}