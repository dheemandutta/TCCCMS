using TCCCMS.Models;
using TCCCMS.Business;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace TCCCMS.Controllers
{
    public class MaintenanceOfShipAndEquipmentController : Controller
    {
        //--------------------Vol5-------------------------

        private string controllerName = "MaintenanceOfShipAndEquipment";
        ManualBL manualBL = new ManualBL();

        // GET: MaintenanceOfShipAndEquipment
        public ActionResult Index()
        {
            Manual file = new Manual();
            string xPath = Server.MapPath("~/xmlMenu/" + "ALLVOLUMES.xml");
            file.ManualBodyHtml = manualBL.GenerateBodyContentHtml(xPath, 5);

            return View(file);
        }
        public ActionResult Pages(string actionName)
        {
            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, actionName);
            TempData[actionName] = file.ManualBodyHtml;
            return View(file);
        }

        public ActionResult PDFViewer(string fileName)
        {
            Manual file = new Manual();
            string filePath = "../ManualsPDF/Volume V/";
            filePath = filePath + fileName + ".pdf";
            file.PdfName = fileName;
            file.PdfPath = filePath;
            return View(file);
        }

        #region All--(45+7+5+2)(59)
        public ActionResult Manual()
        {
            //---------------Vol. V Manual-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "Manual");
            TempData["Manual"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult Boiler()
        {
            //---------------1-Boiler-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "Boiler");
            TempData["Boiler"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult SteeringGear()
        {
            //---------------2-Steering Gear-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "SteeringGear");
            TempData["SteeringGear"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult MainEngines()
        {
            //---------------3-Main Engines-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "MainEngines");
            TempData["MainEngines"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult Generator()
        {
            //---------------4-Generator----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "Generator");
            TempData["Generator"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult STLG()
        {
            //---------------5-Stern Tube Leaking Guideline-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "STLG");
            TempData["STLG"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult FuelOil()
        {
            //---------------6-Fuel Oil-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "FuelOil");
            TempData["FuelOil"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult Lubrication()
        {
            //---------------7-Lubrication-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "Lubrication");
            TempData["Lubrication"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult EEE()
        {
            //---------------8-Electrical and Electronic Equipment-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "EEE");
            TempData["EEE"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult Refrigeration()
        {
            //---------------9-Refrigeration-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "Refrigeration");
            TempData["Refrigeration"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult MOTER()
        {
            //---------------10-Machinery outside the engine room----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "MOTER");
            TempData["MOTER"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult Surveys()
        {
            //---------------11-Surveys-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "Surveys");
            TempData["Surveys"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult UMS()
        {
            //---------------12-Unmanned machinery space-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "UMS");
            TempData["UMS"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult AuxiliaryMachiney()
        {
            //---------------13-Auxiliary Machiney-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "AuxiliaryMachiney");
            TempData["AuxiliaryMachiney"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult LandingAdvice()
        {
            //---------------15-Landing Advice-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "LandingAdvice");
            TempData["LandingAdvice"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult NewBuilding()
        {
            //---------------16 - New Building-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "NewBuilding");
            TempData["NewBuilding"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult FreshWater()
        {
            //---------------17-Fresh Water-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "FreshWater");
            TempData["FreshWater"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult MonitoringAndAlarm()
        {
            //---------------18- Monitoring and Alarm-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "MonitoringAndAlarm");
            TempData["MonitoringAndAlarm"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult ECWT()
        {
            //---------------19-Engine Cooling Water Treatment-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "ECWT");
            TempData["ECWT"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult SPS()
        {
            //---------------20-Standard Paint Scheme-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "SPS");
            TempData["SPS"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult ORB()
        {
            //---------------21-Oil Record Book-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "ORB");
            TempData["ORB"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult VesselImmobilisation()
        {
            //---------------22-Vessel Immobilisation-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "VesselImmobilisation");
            TempData["VesselImmobilisation"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult MaintenancePMS()
        {
            //---------------24-Maintenance PMS (Bassnet)-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "MaintenancePMS");
            TempData["MaintenancePMS"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult SRMRTIR1()
        {
            //---------------25- Annex A -Soft Rope and Mooring Rope Tail Inspection Rev 01-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "SRMRTIR1");
            TempData["SRMRTIR1"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult MooringMaintenanceR1()
        {
            //---------------25-Mooring Maintenance Rev 01-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "MooringMaintenanceR1");
            TempData["MooringMaintenanceR1"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult LiftingAppliances()
        {
            //---------------26-Lifting Appliances-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "LiftingAppliances");
            TempData["LiftingAppliances"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult EUCR()
        {
            //---------------28- Annex I - EU Commision Recommendation-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "EUCR");
            TempData["EUCR"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult EUDFAQ()
        {
            //---------------28- Annex II - EU Directive FAQ-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "EUDFAQ");
            TempData["EUDFAQ"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult MFSRBS()
        {
            //---------------28- Annex III Marine Fuel Sulphur Record Book Sample-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "MFSRBS");
            TempData["MFSRBS"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult CRFS()
        {
            //---------------28- Annex IV CARB Recordkeeping Form Samples-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "CRFS");
            TempData["CRFS"] = file.ManualBodyHtml;
            return View(file);
        }
        //public ActionResult FCOCV6()
        //{
        //    //---------------28- Annex V - FOBAS Change Over Calculator V6-----xls------------

        //    Manual file = new Manual();
        //    file = manualBL.GetManual(controllerName, "FCOCV6");
        //    TempData["FCOCV6"] = file.ManualBodyHtml;
        //    return View(file);
        //}
        public ActionResult BNSECA()
        {
            //---------------28- Annex VI (A) - Baltic and North Sea ECA-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "BNSECA");
            TempData["BNSECA"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult TurkeyECA()
        {
            //---------------28- Annex VI (B) - Turkey ECA-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "TurkeyECA");
            TempData["TurkeyECA"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult NACECA()
        {
            //---------------28- Annex VI (C) - North Amercian and Caribbean ECA-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "NACECA");
            TempData["NACECA"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult HKECA()
        {
            //---------------28- Annex VI (D) - HK ECA-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "HKECA");
            TempData["HKECA"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult ChinaECAR1()
        {
            //---------------28- Annex VI (E) - China ECA Rev 01-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "ChinaECAR1");
            TempData["ChinaECAR1"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult EUSD()
        {
            //---------------28- Annex VI (F) - EU Sulphur Directive-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "EUSD");
            TempData["EUSD"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult GKECAR()
        {
            //---------------28- Annex VI (G) - Guidance for Korea Emission Control Areas Regulation-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "GKECAR");
            TempData["GKECAR"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult SampleOfFONAR()
        {
            //---------------28- Annex VII - Sample of FONAR-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "SampleOfFONAR");
            TempData["SampleOfFONAR"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult LSMGO()
        {
            //---------------28-Low sulphur MGO-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "LSMGO");
            TempData["LSMGO"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult CES()
        {
            //---------------29 - Critical Equipment and Spare-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "CES");
            TempData["CES"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult LCE()
        {
            //---------------29.3 - Annex A - List of Critical Equipment-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "LCE");
            TempData["LCE"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult GTI()
        {
            //---------------30-Guidelines for Tanker Inspection-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "GTI");
            TempData["GTI"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult RecyclingOfVessel()
        {
            //---------------31-Recycling of Vessel-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "RecyclingOfVessel");
            TempData["RecyclingOfVessel"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult SCRS()
        {
            //---------------33-Selective Catalytic Reduction System-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "SCRS");
            TempData["SCRS"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult SPM()
        {
            //---------------34-Shipboard Procurement Management-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "SPM");
            TempData["SPM"] = file.ManualBodyHtml;
            return View(file);
        }

        #region 14-Dry Docking (3+4)(7)
        public ActionResult DryDocking()
        {
            //---------------14 - Dry Docking-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "DryDocking");
            TempData["DryDocking"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult DDPFC()
        {
            //---------------Annex 2 - Dry Docking Process Flow Chart----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "DDPFC");
            TempData["DDPFC"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult DDPBM()
        {
            //---------------Annex 3 - Dry Docking Project Bassnet Module-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "DDPBM");
            TempData["DDPBM"] = file.ManualBodyHtml;
            return View(file);
        }

        #region Annex 1 - Dry dock Spec (4)
        public ActionResult DSTC()
        {
            //---------------Annex 1A - Dry Dock Spec Terms _ Condition-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "DSTC");
            TempData["DSTC"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult GeneralServices()
        {
            //---------------Annex 1B - General Services-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "GeneralServices");
            TempData["GeneralServices"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult HullParts()
        {
            //---------------Annex 1C - Hull Parts-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "HullParts");
            TempData["HullParts"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult HPP()
        {
            //---------------Annex 1D - Hull Preparation _ Paintings-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "HPP");
            TempData["HPP"] = file.ManualBodyHtml;
            return View(file);
        }
        #endregion

        #endregion

        #region 23-Maintenance Procedure (5)
        public ActionResult DRFC()
        {
            //---------------23-Defect Reporting Flow Chart (Annex D)-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "DRFC");
            TempData["DRFC"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult MaintenanceMatrix()
        {
            //---------------23-Maintenance Matrix (Annex C)-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "MaintenanceMatrix");
            TempData["MaintenanceMatrix"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult MaintenanceProcedure()
        {
            //---------------23-Maintenance Procedure-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "MaintenanceProcedure");
            TempData["MaintenanceProcedure"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult SPMSWPFC()
        {
            //---------------23-Shipboard PMS Work process flow chart (Annex A)-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "SPMSWPFC");
            TempData["SPMSWPFC"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult SBPMSWPFC()
        {
            //---------------23-Shore Based PMS Work process flow chart (Annex B)-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "SBPMSWPFC");
            TempData["SBPMSWPFC"] = file.ManualBodyHtml;
            return View(file);
        }

        #endregion

        #region Appendix (2)
        public ActionResult GILOG()
        {
            //---------------General Information on Lub Oil Guideline-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "GILOG");
            TempData["GILOG"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult StandardPS()
        {
            //---------------Standard Paint Scheme-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "StandardPS");
            TempData["StandardPS"] = file.ManualBodyHtml;
            return View(file);
        }

        #endregion

        #endregion
    }
}