using TCCCMS.Models;
using TCCCMS.Business;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TCCCMS.Controllers
{
    public class FormsController : Controller
    {
        // GET: Forms
        public ActionResult ListOfAdministrationMmanagementForms()
        {
            return View();
        }

        public ActionResult ListOfMaintenanceFroms()
        {
            return View();
        }

        public ActionResult ListOfMmanagementOfShipPersonalForms()
        {
            return View();
        }

        public ActionResult NavigationMmanagementForms()
        {
            return View();
        }
        public ActionResult SafetyMmanagementForms()
        {
            return View();
        }


        public JsonResult LoadData(int CategoryId)
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
                length = 50000;
            }

            if (start == 0)
            {
                pageIndex = 1;
            }
            else
            {
                pageIndex = (start / length) + 1;
            }

            DownloadableFromsBL bL = new DownloadableFromsBL(); ///////////////////////////////////////////////////////////////////////////
            int totalrecords = 0;

            List<DownloadableFroms> pocoList = new List<DownloadableFroms>();
            pocoList = bL.GetDownloadableFromsPageWise(pageIndex, ref totalrecords, length, CategoryId);
            List<DownloadableFroms> pList = new List<DownloadableFroms>();
            foreach (DownloadableFroms pC in pocoList)
            {
                DownloadableFroms pOCO = new DownloadableFroms();
                pOCO.ID         = pC.ID;
                pOCO.FormName   = pC.FormName;
                pOCO.Path       = pC.Path;
                pOCO.Version    = pC.Version;
                pOCO.IsUpload   = pC.IsUpload;

                pList.Add(pOCO);
            }

            var data = pList;
            return Json(new { draw = draw, recordsFiltered = totalrecords, recordsTotal = totalrecords, data = data }, JsonRequestBehavior.AllowGet);
        }


        public ActionResult FormsApprovalList()
        {
            List<Forms> formList = new List<Forms>();
            DocumentBL documentBL = new DocumentBL();
            formList = documentBL.GetFilledupFormRequiredApprovalList(Convert.ToInt32(Session["UserId"].ToString()));
            return View(formList);
        }

        public JsonResult ApproveFilledUpForm(Forms form)
        {
            DocumentBL documentBl = new DocumentBL();
            int filledUpFormId = form.ID;
           int x = documentBl.ApproveFilledUpForm(filledUpFormId, Convert.ToInt32(Session["UserId"].ToString()));
            return Json("", JsonRequestBehavior.AllowGet);
        }

        public ActionResult FillupFormList()
        {
            ApprovedFilledupFormAndApproverViewModel affaVM = new ApprovedFilledupFormAndApproverViewModel();
            DocumentBL documentBL = new DocumentBL();
            affaVM = documentBL.GetApprovedFilledUpForms(Convert.ToInt32(Session["UserId"].ToString()));
            return View(affaVM);
        }

        public JsonResult SendMailForapproval(string approvalId,string approverUserId)
        {

            return Json("", JsonRequestBehavior.AllowGet);
        }


    }
}