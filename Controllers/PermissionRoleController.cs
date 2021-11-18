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
    public class PermissionRoleController : Controller
    {
        // GET: PermissionRole
        [CustomAuthorizationFilter]
        public ActionResult Index()
        {
            PermissionRolePOCO poco = new PermissionRolePOCO();
            poco = GetAllRolesForDrp();

            GetAllPermissionForDrp();
            //GetAllGroupsForDrp();
            return View(poco);
        }

        [CustomAuthorizationFilter]

        public JsonResult LoadData(int selectedPermissionId)
        {
            PermissionRoleBL bL = new PermissionRoleBL();
            string commaSeperatedValues = bL.GetAllCommaSeperatedPermissionRoleByPermissionId(selectedPermissionId);
            //var data = string.Empty;
            return Json(commaSeperatedValues, JsonRequestBehavior.AllowGet);
        }


        //for Permission drp
        public void GetAllPermissionForDrp()
        {
            PermissionRoleBL bL = new PermissionRoleBL();
            List<PermissionRolePOCO> pocoList = new List<PermissionRolePOCO>();

            pocoList = bL.GetAllPermissionForDrp(/*int.Parse(Session["VesselID"].ToString())*/);


            List<PermissionRolePOCO> itmasterList = new List<PermissionRolePOCO>();

            foreach (PermissionRolePOCO up in pocoList)
            {
                PermissionRolePOCO unt = new PermissionRolePOCO();
                unt.PermissionId = up.PermissionId;
                unt.PermissionName = up.PermissionName;

                itmasterList.Add(unt);
            }

            ViewBag.Permissions = itmasterList.Select(x =>
                                            new SelectListItem()
                                            {
                                                Text = x.PermissionName,
                                                Value = x.PermissionId.ToString()
                                            });

        }


        //for Role drp
        public PermissionRolePOCO GetAllRolesForDrp()
        {
            PermissionRoleBL bL = new PermissionRoleBL();
            List<PermissionRolePOCO> pocoList = new List<PermissionRolePOCO>();

            pocoList = bL.GetAllRolesForDrp();
            PermissionRolePOCO pOCO = new PermissionRolePOCO();

            var list = new List<KeyValuePair<string, string>>();

            foreach (PermissionRolePOCO up in pocoList)
            {
                PermissionRolePOCO unt = new PermissionRolePOCO();
                //unt.GroupId = up.GroupId;
                //unt.GroupName = up.GroupName;



                list.Add(new KeyValuePair<string, string>(up.RoleId.ToString(), up.RoleName));

                //itmasterList.Add(unt);
            }

            pOCO.Roles = list;

            //ViewBag.Groups = itmasterList.Select(x =>
            //                                new SelectListItem()
            //                                {
            //                                    Text = x.GroupName,
            //                                    Value = x.GroupId.ToString()
            //                                });

            return pOCO;
        }


        [CustomAuthorizationFilter]
        public JsonResult SavePermissionRole(int permissionId, string[] selectedRoles)
        {
            var data = string.Empty;
            string selectedRoleIDs = string.Empty;
            if (selectedRoles.Length > 0 && selectedRoles[0] != ",")
                selectedRoleIDs = String.Join(",", selectedRoles);
            PermissionRoleBL bL = new PermissionRoleBL();
            int records = bL.SavePermissionRole(permissionId, selectedRoleIDs);

            return Json(records, JsonRequestBehavior.AllowGet);
        }



    }
}