using TCCCMS.Models;
using TCCCMS.Business;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using TCCCMS.Infrastructure;

namespace TCCCMS.Controllers
{
    [CustomAuthorizationFilter]
    public class ShipController : Controller
    {
        // GET: Ship
        [CustomAuthorizationFilter]
        public ActionResult Index()
        {
            ShipBL shipBl = new ShipBL();
            List<VesselType> vesselTypeList = new List<VesselType>();
            vesselTypeList = shipBl.GetVesselTypeListForDopDown();
            ViewBag.VesselType = vesselTypeList.Select(vt => 
                                                            new SelectListItem() 
                                                            { 
                                                                Text = vt.Description,
                                                                Value = vt .ID.ToString()
                                                            }).ToList();
            return View();
        }

        [CustomAuthorizationFilter]
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
                length = 10;
            }

            if (start == 0)
            {
                pageIndex = 1;
            }
            else
            {
                pageIndex = (start / length) + 1;
            }

            ShipBL shipBL = new ShipBL();
            int totalrecords = 0;

            List<Ship> shipList = new List<Ship>();
            shipList = shipBL.GetAllShipsPageWise(pageIndex, ref totalrecords, length);
            //List<Ship> shipList = new List<Ship>();
            //foreach (Ship shipPC in shippocoList)
            //{
            //    Ship ship = new Ship();
            //    ship.ID = shipPC.ID;
            //    ship.ShipName = shipPC.ShipName;
            //    ship.FlagOfShip = shipPC.FlagOfShip;
            //    ship.IMONumber = shipPC.IMONumber;
            //    shipList.Add(ship);
            //}

            var data = shipList;

            return Json(new { draw = draw, recordsFiltered = totalrecords, recordsTotal = totalrecords, data = data }, JsonRequestBehavior.AllowGet);
        }

        [CustomAuthorizationFilter]
        public JsonResult SaveShip(Ship ship)
        {
            ShipBL shipBL = new ShipBL();
            //Ship shipPC = new Ship();
            //shipPC.ID = ship.ID;

            //shipPC.ShipName = ship.ShipName;
            //shipPC.FlagOfShip = ship.FlagOfShip;
            //shipPC.IMONumber = ship.IMONumber;

            //shipPC.ID = ship.ID;
            //shipPC.VesselTypeId = ship.VesselTypeId;
            //shipPC.VesselSubTypeId = ship.VesselSubTypeId;
            //shipPC.VesselSubSubTypeId = ship.VesselSubSubTypeId;

            //shipPC.ShipEmail1 = ship.ShipEmail1;
            //shipPC.ShipEmail2 = ship.ShipEmail2;
            //shipPC.Voices1 = ship.Voices1;
            //shipPC.Voices2 = ship.Voices2;
            //shipPC.Fax1 = ship.Fax1;
            //shipPC.Fax2 = ship.Fax2;
            //shipPC.VOIP1 = ship.VOIP1;
            //shipPC.VOIP2 = ship.VOIP2;
            //shipPC.Mobile1 = ship.Mobile1;
            //shipPC.Mobile2 = ship.Mobile2;

            int rowaffected = shipBL.SaveShipDetails(ship);

            //if (rowaffected > 0)
            //{
            //    return Json(new { result = "Redirect", url = Url.Action("Index", "Ship") });
            //}
            //else
            //{
            //    return Json(new { result = "Error" }, JsonRequestBehavior.AllowGet);
            //}

            return Json(rowaffected, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        [CustomAuthorizationFilter]
        public JsonResult GetShipById(string id)
        {
            ShipBL shipBL = new ShipBL();
            Ship ship = new Ship();

            ship = shipBL.GetShipDetailsById(Convert.ToInt32(id));


            return Json(ship, JsonRequestBehavior.AllowGet);
        }

        #region DropDown
        [CustomAuthorizationFilter]
        public JsonResult GetVesselSubTypeByVesselTypeForDopDown(string vesselTypeID)
        {
            int vesselTypeId = Convert.ToInt32(vesselTypeID);
            ShipBL shipBL = new ShipBL();
            List<VesselSubType> vesselSubTypeList = new List<VesselSubType>();

            vesselSubTypeList = shipBL.GetVesselSubTypeListByTypeForDopDown(vesselTypeId);


            var data = vesselSubTypeList;

            return Json(data, JsonRequestBehavior.AllowGet);
        }


        //for GetVesselSubSubTypeByVesselSubTypeIDForDrp drp
        [CustomAuthorizationFilter]
        public JsonResult GetVesselSubSubTypeByVesselSubTypeForDropdown(string vesselSubTypeID)
        {
            int vesselSubTypeId = Convert.ToInt32(vesselSubTypeID);
            ShipBL shipBL = new ShipBL();
            List<VesselSubSubType> vesselSubSubTypeList = new List<VesselSubSubType>();

            vesselSubSubTypeList = shipBL.GetVesselSubSubTypeListBySubTypeForDopDown(vesselSubTypeId);


            var data = vesselSubSubTypeList;

            return Json(data, JsonRequestBehavior.AllowGet);
        }


        #endregion

    }
}