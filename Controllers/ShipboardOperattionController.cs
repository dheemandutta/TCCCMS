using TCCCMS.Models;
using TCCCMS.Business;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace TCCCMS.Controllers
{
    public class ShipboardOperattionController : Controller
    {
        //--------------------Vol4-------------------------

        private string controllerName = "ShipboardOperattion";
        ManualBL manualBL = new ManualBL();
        // GET: ShipboardOperattion
        public ActionResult Index()
        {
            return View();
        }
        [HttpGet]

        #region All (1+36+47+31+10+11+7+13)(156)
        public ActionResult Manual()
        {
            //---------------Vol. IV Manual-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "Manual");
            TempData["Manual"] = file.ManualBodyHtml;
            return View(file);
        }

        #region Appendix---(4+32)(36)


        public ActionResult SFCE()
        {
            //---------------Standard Filing for Chief Engineer-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "SFCE");
            TempData["SFCE"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult SFCO()
        {
            //---------------Standard Filing for Chief Officer Rev 01-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "SFCO");
            TempData["SFCO"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult SFM()
        {
            //---------------Standard Filing for Master-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "SFM");
            TempData["SFM"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult SFSO()
        {
            //---------------Standard Filing for Second Officer-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "SFSO");
            TempData["SFSO"] = file.ManualBodyHtml;
            return View(file);
        }

        #region JHA Template (2+17+13)(32)
        public ActionResult A1LSRATD()
        {
            //---------------Appendix 1 - List of Sample Risk Assessment Template - Deck-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "A1LSRATD");
            TempData["A1LSRATD"] = file.ManualBodyHtml;
            return View(file);
        }

        public ActionResult A1LSRATE()
        {
            //---------------Appendix 1 - List of Sample Risk Assessment Template - Engine-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "A1LSRATE");
            TempData["A1LSRATE"] = file.ManualBodyHtml;
            return View(file);
        }

        #region Deck (17)
        ////=========Deck==========================
        public ActionResult STSOperation()
        {
            //---------------D1- STS Operation-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "STSOperation");
            TempData["STSOperation"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult SBMOperation()
        {
            //---------------D2 - SBM Operation-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "SBMOperation");
            TempData["SBMOperation"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult RURWAP()
        {
            //---------------D3 - Rigging Unrigging of Razor Wire for anti-piracy-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "RURWAP");
            TempData["RURWAP"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult MRWMSCI()
        {
            //---------------D4 - Mast Riser Wire Mesh Strainer cleaning _ inspection-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "MRWMSCI");
            TempData["MRWMSCI"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult PTUSC()
        {
            //---------------D5 - Personnel Transfer Using Ships Crane-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "PTUSC");
            TempData["PTUSC"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult RSIC()
        {
            //---------------D6 - Radar Scanner inspection and cleaning-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "RSIC");
            TempData["RSIC"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult IGSI()
        {
            //---------------D7 - IG Scrubber inspection-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "IGSI");
            TempData["IGSI"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult COPSSI()
        {
            //---------------D8 - COP Strainer, Seperator inspection-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "COPSSI");
            TempData["COPSSI"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult COTWW()
        {
            //---------------D9 - Cargo Oil Tank water washing-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "COTWW");
            TempData["COTWW"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult TMLD()
        {
            //---------------D10- Three monthly lifeboat drill-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "TMLD");
            TempData["TMLD"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult HHH2SC()
        {
            //---------------D11- Handling of High H2S Cargo-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "HHH2SC");
            TempData["HHH2SC"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult MooringOperation()
        {
            //---------------D12 - Mooring Operation-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "MooringOperation");
            TempData["MooringOperation"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult TugOperation()
        {
            //---------------D13 - Tug Operation-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "TugOperation");
            TempData["TugOperation"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult AnchoringOperation()
        {
            //---------------D14 - Anchoring Operation-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "AnchoringOperation");
            TempData["AnchoringOperation"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult EBTADSR()
        {
            //---------------D15 - Entry BT after de-ballasting for sediment removal (Hyundai Hi-Ballast)-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "EBTADSR");
            TempData["EBTADSR"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult EntryBTADSR()
        {
            //---------------D16 - Entry BT after de-ballasting for sediment removal (NEI)-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "EntryBTADSR");
            TempData["EntryBTADSR"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult SOICOB()
        {
            //---------------D17 - Simultaneous Operations including Cargo Operations and bunkering-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "SOICOB");
            TempData["SOICOB"] = file.ManualBodyHtml;
            return View(file);
        }

        ////===END======Deck===========17===============


        #endregion

        #region Engine (13)
        ////=========Engine==========================
        public ActionResult AEM()
        {
            //---------------E1- AE Maintenance-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "AEM");
            TempData["AEM"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult COFFHFO2LSMGO()
        {
            //---------------E2- Change Over of Fuel from HFO to LSMGO-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "COFFHFO2LSMGO");
            TempData["COFFHFO2LSMGO"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult ICACG()
        {
            //---------------E3- Inspection or Cleaning of AC Generator-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "ICACG");
            TempData["ICACG"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult MBSAB()
        {
            //---------------E4- Maintenance of Burner _ Swirler of Aux Boiler-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "MBSAB");
            TempData["MBSAB"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult MEEAP()
        {
            //---------------E5- Maintenance of Elect. Equipment at Aloft Position-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "MEEAP");
            TempData["MEEAP"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult MOWS()
        {
            //---------------E6- Maintenance of OWS-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "MOWS");
            TempData["MOWS"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult MESSCI()
        {
            //---------------E7- ME Scavenge space Cleaning and Inspection-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "MESSCI");
            TempData["MESSCI"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult OEM()
        {
            //---------------E8- Overhauling of Electric Motor-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "OEM");
            TempData["OEM"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult RPMS()
        {
            //---------------E9- Renewal of Pump Mechanical Seal-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "RPMS");
            TempData["RPMS"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult RMEFVHPP()
        {
            //---------------E10- Replacement of ME Fuel Valve _ HP Pipes-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "RMEFVHPP");
            TempData["RMEFVHPP"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult SCSC()
        {
            //---------------E11- Sea Chest Strainer Cleaning-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "SCSC");
            TempData["SCSC"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult UREABO()
        {
            //---------------E12 -UREA Bunkering Operation-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "UREABO");
            TempData["UREABO"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult BWTSPO()
        {
            //---------------E13 - BWTS Plant Operation-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "BWTSPO");
            TempData["BWTSPO"] = file.ManualBodyHtml;
            return View(file);
        }


        ////===END======Engine=========11=================
        #endregion



        ////---End---JHA Template------30--

        #endregion

        ////----End-----Appendix-----34-----
        #endregion

        #region  Sec 1 Health Safety and Environmental (39 + 8)(47)

        public ActionResult LHM()
        {
            //---------------Ch. 31 Annex 1 - List of Hazardous Materials-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "LHM");
            TempData["LHM"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult FMD()
        {
            //---------------Ch. 31 Annex 2 - Form of Material Declaration-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "FMD");
            TempData["FMD"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult FSDC()
        {
            //---------------Ch. 31 Annex 3 - Form of Suppliers Declaration of Conformity-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "FSDC");
            TempData["FSDC"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult GSS()
        {
            //---------------Chapter 1 General Shipboard Safety-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "GSS");
            TempData["GSS"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult FSTS()
        {
            //---------------Chapter 2 Annex 1 - First Schedule of Toxic Substances-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "FSTS");
            TempData["FSTS"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult HealthAndHygiene()
        {
            //---------------Chapter 2 Health and Hygiene-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "HealthAndHygiene");
            TempData["HealthAndHygiene"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult Fatigue()
        {
            //---------------Chapter 3 Fatigue-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "Fatigue");
            TempData["Fatigue"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult ECR1()
        {
            //---------------Chapter 4 Environmental Control Rev 10 - Annex 1-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "ECR1");
            TempData["ECR1"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult ECR2()
        {
            //---------------Chapter 4 Environmental Control Rev 10 - Annex 2-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "ECR2");
            TempData["ECR2"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult EnvironmentalControl()
        {
            //---------------Chapter 4 Environmental Control----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "EnvironmentalControl");
            TempData["EnvironmentalControl"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult CTRR()
        {
            //---------------Chapter 6 Casualty Treatment  Resuscitation and Rescue-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "CTRR");
            TempData["CTRR"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult CWP()
        {
            //---------------Chapter 7 Cold Weather Precaution-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "CWP");
            TempData["CWP"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult TWM()
        {
            //---------------Chapter 8 Tools and Workshop Machinery----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "TWM");
            TempData["TWM"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult EEE()
        {
            //---------------Chapter 9 Electric and Electrical Equipment-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "EEE");
            TempData["EEE"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult SGP()
        {
            //---------------Chapter 10 Safety in the Galley and Pantry-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "SGP");
            TempData["SGP"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult ERESO()
        {
            //---------------Chapter 12 Engine Room - Emergency _ Safe Operation-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "ERESO");
            TempData["ERESO"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult PSP()
        {
            //---------------Chapter 13 Painting - Safety Precaution -----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "PSP");
            TempData["PSP"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult ShipyardSafety()
        {
            //---------------Chapter 14 Shipyard safety-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "ShipyardSafety");
            TempData["ShipyardSafety"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult LOTO()
        {
            //---------------Chapter 15 Lock Out and Tag Out (LOTO) Rev 01-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "LOTO");
            TempData["LOTO"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult LSE()
        {
            //---------------Chapter 16 Life Saving Equipment-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "LSE");
            TempData["LSE"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult HullMarking()
        {
            //---------------Chapter 17 Hull Marking-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "HullMarking");
            TempData["HullMarking"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult Fishing()
        {
            //---------------Chapter 18 Fishing-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "Fishing");
            TempData["Fishing"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult PCE1()
        {
            //---------------Chapter 19 Protective Clothing and Equipment - Annex No.1-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "PCE1");
            TempData["PCE1"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult PCE()
        {
            //---------------Chapter 19 Protective Clothing and Equipment-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "PCE");
            TempData["PCE"] = file.ManualBodyHtml;
            return View(file);
        }
        //public ActionResult PTA()
        //{
        //    //---------------Chapter 20 Annex 1-IMO 1045(27)- Pilot Transfer Arrangement----pdf-------------

        //    Manual file = new Manual();
        //    file = manualBL.GetManual(controllerName, "PTA");
        //    TempData["PTA"] = file.ManualBodyHtml;
        //    return View(file);
        //}
        public ActionResult MeansAccess()
        {
            //---------------Chapter 20 Means of Access-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "MeansAccess");
            TempData["MeansAccess"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult PRE()
        {
            //---------------Chapter 21 Pump room entry-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "PRE");
            TempData["PRE"] = file.ManualBodyHtml;
            return View(file);
        }
        //public ActionResult MSDS()
        //{
        //    //---------------Chapter 22 MSDS - Annex IMO Res 286(86)------pdf-----------

        //    Manual file = new Manual();
        //    file = manualBL.GetManual(controllerName, "MSDS");
        //    TempData["MSDS"] = file.ManualBodyHtml;
        //    return View(file);
        //}
        public ActionResult MSDS()
        {
            //---------------Chapter 22 MSDS-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "MSDS");
            TempData["MSDS"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult CII()
        {
            //---------------Chapter 23 Control and Isolation of Infectious-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "CII");
            TempData["CII"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult SWO()
        {
            //---------------Chapter 24 Stop Work Order-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "SWO");
            TempData["SWO"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult TDCP()
        {
            //---------------Chapter 25 Temporary Deviation from Company Procedure-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "TDCP");
            TempData["TDCP"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult HandlingBenzene()
        {
            //---------------Chapter 26 Handling of Benzene-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "HandlingBenzene");
            TempData["HandlingBenzene"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult HCCM()
        {
            //---------------Chapter 27 Handling cargo containing Mercury-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "HCCM");
            TempData["HCCM"] = file.ManualBodyHtml;
            return View(file);
        }
        //public ActionResult HMCOG()
        //{
        //    //---------------Chapter 27 HIGH MERCURY CARGOES OCIMF guidelines------pdf-----------

        //    Manual file = new Manual();
        //    file = manualBL.GetManual(controllerName, "HMCOG");
        //    TempData["HMCOG"] = file.ManualBodyHtml;
        //    return View(file);
        //}
        public ActionResult PTSLG()
        {
            //---------------Chapter 28 Personnel Transfer by Ship_s Lifting Gear----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "PTSLG");
            TempData["PTSLG"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult AWSP()
        {
            //---------------Chapter 29 Arc Welding - Safety Precaution-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "AWSP");
            TempData["AWSP"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult HHS()
        {
            //---------------Chapter 30 Handling of Hydrogen Sulphide-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "HHS");
            TempData["HHS"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult IHM()
        {
            //---------------Chapter 31 Inventory of Hazardous Materials-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "IHM");
            TempData["IHM"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult HCCMMercaptan()
        {
            //---------------Chapter 32 Handling cargo containing Mercaptan-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "HCCMMercaptan");
            TempData["HCCMMercaptan"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult SHUC()
        {
            //---------------Chapter 33 Safe handling and Use of Chemicals-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "SHUC");
            TempData["SHUC"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult S1Contents()
        {
            //---------------Contents Rev 01-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "S1Contents");
            TempData["S1Contents"] = file.ManualBodyHtml;
            return View(file);
        }

        #region  Chapter 5 Work Permit System (8)

        public ActionResult WPS()
        {
            //---------------Chapter 5.1 Work Permit System-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "WPS");
            TempData["WPS"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult HWPP()
        {
            //---------------Chapter 5.2 Hot Work permit procedure-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "HWPP");
            TempData["HWPP"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult CWPP()
        {
            //---------------Chapter 5.3 Cold Work Permit Procedure-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "CWPP");
            TempData["CWPP"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult ESEWPP()
        {
            //---------------Chapter 5.4 Enclosed space entry work permit procedure-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "ESEWPP");
            TempData["ESEWPP"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult WAOWPP()
        {
            //---------------Chapter 5.5 Working Aloft Overside Work Permit Procedure-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "WAOWPP");
            TempData["WAOWPP"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult HVWPP()
        {
            //---------------Chapter 5.6 High Voltage Work Permit Procedure-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "HVWPP");
            TempData["HVWPP"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult UWPP()
        {
            //---------------Chapter 5.7 Underwater work permit procedure-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "UWPP");
            TempData["UWPP"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult WDDHWPP()
        {
            //---------------Chapter 5.8 Working on Deck during Heavy Weather Permit Procedure-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "WDDHWPP");
            TempData["WDDHWPP"] = file.ManualBodyHtml;
            return View(file);
        }

        #endregion

        #endregion


        #region  Sec 2 Safety of the Vessel (11 +20)(31)
        public ActionResult ERP()
        {
            //---------------Chapter 2 Engine Room Procedures-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "ERP");
            TempData["ERP"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult SSOP()
        {
            //---------------Chapter 3 Special Shipboard Operation Procedures -----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "SSOP");
            TempData["SSOP"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult HelicopterOperations()
        {
            //---------------Chapter 4 Helicopter Operations----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "HelicopterOperations");
            TempData["HelicopterOperations"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult FPFF()
        {
            //---------------Chapter 5 Fire Precautions _ Fire Fighting-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "FPFF");
            TempData["FPFF"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult MooringTowing()
        {
            //---------------Chapter 6 Mooring and Towing-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "MooringTowing");
            TempData["MooringTowing"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult LaunchingLifeboat()
        {
            //---------------Chapter 7 Launching of lifeboat-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "LaunchingLifeboat");
            TempData["LaunchingLifeboat"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult SFD()
        {
            //---------------Chapter 8 Stability and Flooding Dangers-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "SFD");
            TempData["SFD"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult PAP()
        {
            //---------------Chapter 9 Preparation for arrival in port-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "PAP");
            TempData["PAP"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult PreparingSea()
        {
            //---------------Chapter 10 Preparing for Sea-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "PreparingSea");
            TempData["PreparingSea"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult ManoeuvringData()
        {
            //---------------Chapter 11 Manoeuvring Data-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "ManoeuvringData");
            TempData["ManoeuvringData"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult S2Contents()
        {
            //---------------Sec 2 Contents-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "S2Contents");
            TempData["S2Contents"] = file.ManualBodyHtml;
            return View(file);
        }
        ////---View Added

        #region Chapter 1 Navigational and Bridge Procedures (20)
        public ActionResult NBPC()
        {
            //---------------Chapter 1 Navigational and Bridge Procedures Contents-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "NBPC");
            TempData["NBPC"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult Responsibility()
        {
            //---------------Chapter 1.1 Responsibility-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "Responsibility");
            TempData["Responsibility"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult GuidelineMasterA1()
        {
            //---------------Chapter 1.2 - Guideline to Master - Annex 1-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "GuidelineMasterA1");
            TempData["GuidelineMasterA1"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult GuidelineMaster()
        {
            //---------------Chapter 1.2 - Guideline to Master-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "GuidelineMaster");
            TempData["GuidelineMaster"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult NavigationProcedure()
        {
            //---------------Chapter 1.3 - Navigation Procedure-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "NavigationProcedure");
            TempData["NavigationProcedure"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult WatchKeeping()
        {
            //---------------Chapter 1.4 - Watch Keeping-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "WatchKeeping");
            TempData["WatchKeeping"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult ChartPublication()
        {
            //---------------Chapter 1.5 - Chart _ Publication-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "ChartPublication");
            TempData["ChartPublication"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult BRD()
        {
            //---------------Chapter 1.6 - Bridge Records and Display-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "BRD");
            TempData["BRD"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult BNE()
        {
            //---------------Chapter 1.7 - Bridge Navigation Equipment-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "BNE");
            TempData["BNE"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult ECDISR1()
        {
            //---------------Chapter 1.7.30 - ECDIS Rev 01-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "ECDISR1");
            TempData["Manual"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult ECDISA1()
        {
            //---------------Chapter 1.7.30.23 - ECDIS Annex 1-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "ECDISA1");
            TempData["ECDISA1"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult ECDISA2()
        {
            //---------------Chapter 1.7.30.23 - ECDIS Annex 2-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "ECDISA2");
            TempData["ECDISA2"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult CoastalON()
        {
            //---------------Chapter 1.8 - Coastal and Ocean Navigation-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "CoastalON");
            TempData["CoastalON"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult PilotageR1()
        {
            //---------------Chapter 1.9 - Pilotage Rev 01-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "ManPilotageR1ual");
            TempData["PilotageR1"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult Anchoring()
        {
            //---------------Chapter 1.10 - Anchoring-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "Anchoring");
            TempData["Anchoring"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult Weather()
        {
            //---------------Chapter 1.11 - Weather-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "Weather");
            TempData["Weather"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult Contingency()
        {
            //---------------Chapter 1.12 - Contingency----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "Contingency");
            TempData["Contingency"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult DistressSea()
        {
            //---------------Chapter 1.13 - Distress at sea-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "DistressSea");
            TempData["DistressSea"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult IceNavigation()
        {
            //---------------Chapter 1.14 Ice Navigation-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "IceNavigation");
            TempData["IceNavigation"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult NavigationAudit()
        {
            //---------------Chapter 1.15 - Navigation Audit-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "NavigationAudit");
            TempData["NavigationAudit"] = file.ManualBodyHtml;
            return View(file);
        }

        #endregion

        #endregion
        ////---View Added
        #region Sec 3 Port Cargo Operations (10)

        public ActionResult WKP()
        {
            //---------------Chapter 1 Watch Keeping in Port-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "WKP");
            TempData["WKP"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult SafetyPort()
        {
            //---------------Chapter 2 Safety in Port-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "SafetyPort");
            TempData["SafetyPort"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult CargoCare()
        {
            //---------------Chapter 3 Cargo Care-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "CargoCare");
            TempData["CargoCare"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult Bunker()
        {
            //---------------Chapter 4 Bunker-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "Bunker");
            TempData["Bunker"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult CPHS()
        {
            //---------------Chapter 5 Cargo planning Handling  Stowage-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "CPHS");
            TempData["CPHS"] = file.ManualBodyHtml;
            return View(file);
        }

        public ActionResult CargoLoading()
        {
            //---------------Chapter 6 Cargo Loading-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "CargoLoading");
            TempData["CargoLoading"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult CargoDischarging()
        {
            //---------------Chapter 7 Cargo Discharging-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "CargoDischarging");
            TempData["CargoDischarging"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult BallastOperations()
        {
            //---------------Chapter 8 Ballast Operations-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "BallastOperations");
            TempData["BallastOperations"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult TankCleaning()
        {
            //---------------Chapter 9 Tank Cleaning-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "TankCleaning");
            TempData["TankCleaning"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult S3Contents()
        {
            //---------------Sec 3 Contents-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "S3Contents");
            TempData["S3Contents"] = file.ManualBodyHtml;
            return View(file);
        }

        #endregion

        #region Sec 4 Safety Inspection and Audits (11)
        public ActionResult LSAI()
        {
            //---------------Chapter 1 Life Saving Appliances Inspection-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "LSAI");
            TempData["LSAI"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult FFAI()
        {
            //---------------Chapter 2 Fire Fighting Appliances Inspection-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "FFAI");
            TempData["FFAI"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult ISMIA()
        {
            //---------------Chapter 3 ISM Internal Audit-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "ISMIA");
            TempData["ISMIA"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult SafetyMeeting()
        {
            //---------------Chapter 4 Safety Meeting-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "SafetyMeeting");
            TempData["SafetyMeeting"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult SIF()
        {
            //---------------Chapter 5 Safety Induction and Familiarisation-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "SIF");
            TempData["SIF"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult SITOA()
        {
            //---------------Chapter 6 Ship Inspection  Tanker Opers Audit-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "SITOA");
            TempData["SITOA"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult PSCIA1()
        {
            //---------------Chapter 7 Port State Control Inspection - Annex A-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "PSCIA1");
            TempData["PSCIA1"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult PSCI()
        {
            //---------------Chapter 7 Port State Control Inspection-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "PSCI");
            TempData["PSCI"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult OMV()
        {
            //---------------Chapter 8 Oil Major Vetting-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "OMV");
            TempData["OMV"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult ImprovementProcess()
        {
            //---------------Chapter 9 Improvement Process-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "ImprovementProcess");
            TempData["ImprovementProcess"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult S4Content()
        {
            //---------------Sec 4 Content-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "S4Content");
            TempData["S4Content"] = file.ManualBodyHtml;
            return View(file);
        }

        #endregion

        #region  Sec 5 Reporting Accidents _ NC (10 - 3)(7)
        public ActionResult TINC()
        {
            //---------------Chapter 1 Types of Incidents _ NC-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "TINC");
            TempData["TINC"] = file.ManualBodyHtml;
            return View(file);
        }
        //public ActionResult IncidentMatrix()
        //{
        //    //---------------Chapter 1-Annex 1-Incident Matrix------xls-----------

        //    Manual file = new Manual();
        //    file = manualBL.GetManual(controllerName, "IncidentMatrix");
        //    TempData["IncidentMatrix"] = file.ManualBodyHtml;
        //    return View(file);
        //}
        //public ActionResult SRS()
        //{
        //    //---------------Chapter 2 Annex-SAFIR Report Sample (Incident)-------pdf----------

        //    Manual file = new Manual();
        //    file = manualBL.GetManual(controllerName, "SRS");
        //    TempData["SRS"] = file.ManualBodyHtml;
        //    return View(file);
        //}
        //public ActionResult SRS2()
        //{
        //    //---------------Chapter 2 Annex-SAFIR Report Sample (Near Miss)--------pdf---------

        //    Manual file = new Manual();
        //    file = manualBL.GetManual(controllerName, "SRS2");
        //    TempData["SRS2"] = file.ManualBodyHtml;
        //    return View(file);
        //}
        public ActionResult RUAC()
        {
            //---------------Chapter 2 Reporting of Unsafe Acts, Conditions-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "RUAC");
            TempData["RUAC"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult IncidentInvestigationA1()
        {
            //---------------Chapter 3 Incident Investigation - Annex 1-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "IncidentInvestigationA1");
            TempData["IncidentInvestigationA1"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult IncidentInvestigation()
        {
            //---------------Chapter 3 Incident Investigation-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "IncidentInvestigation");
            TempData["IncidentInvestigation"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult IncidentManagement()
        {
            //---------------Chapter 4 Incident Management-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "IncidentManagement");
            TempData["IncidentManagement"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult JHRA()
        {
            //---------------Chapter 5 Job Hazard _ Risk Analyses----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "JHRA");
            TempData["JHRA"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult S5Contents()
        {
            //---------------Sec 5 Contents-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "S5Contents");
            TempData["S5Contents"] = file.ManualBodyHtml;
            return View(file);
        }


        #endregion
        ////---View Added
        #region Sec 6 Admintration Management (13)
        public ActionResult DDC()
        {
            //---------------Chapter 1 Document and Data Control-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "DDC");
            TempData["DDC"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult SSF()
        {
            //---------------Chapter 2 Shipboard Standard Filing-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "SSF");
            TempData["SSF"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult ManagementChange()
        {
            //---------------Chapter 3 Management of Change-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "ManagementChange");
            TempData["ManagementChange"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult MPC()
        {
            //---------------Chapter 4 Managing procedure changes----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "MPC");
            TempData["MPC"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult MSMSR()
        {
            //---------------Chapter 5 Master_s SMS Review-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "MSMSR");
            TempData["MSMSR"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult ManagementReview()
        {
            //---------------Chapter 6 Management Review-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "ManagementReview");
            TempData["ManagementReview"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult ShipComputer()
        {
            //---------------Chapter 7 Ship Computer-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "ShipComputer");
            TempData["ShipComputer"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult FleetCommunications()
        {
            //---------------Chapter 8 Fleet Communications-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "FleetCommunications");
            TempData["FleetCommunications"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult RKEDLR1()
        {
            //---------------Chapter 9 Record Keeping - Entry of Data in Logbook Rev 01-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "RKEDLR1");
            TempData["RKEDLR1"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult ListOfPublications()
        {
            //---------------Chapter 10 List of Publications-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "ListOfPublications");
            TempData["ListOfPublications"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult ListOfPosters()
        {
            //---------------Chapter 11 List of Posters-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "ListOfPosters");
            TempData["ListOfPosters"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult DelegationOfDuty()
        {
            //---------------Chapter 12 Delegation of Duty----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "DelegationOfDuty");
            TempData["DelegationOfDuty"] = file.ManualBodyHtml;
            return View(file);
        }
        public ActionResult S6Contents()
        {
            //---------------Sec 6 Contents-----------------

            Manual file = new Manual();
            file = manualBL.GetManual(controllerName, "S6Contents");
            TempData["S6Contents"] = file.ManualBodyHtml;
            return View(file);
        }

        #endregion
        #endregion

    }
}