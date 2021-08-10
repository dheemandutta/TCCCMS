using TCCCMS.Models;
using TCCCMS.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCCCMS.Business
{
    public class DocumentBL
    {
        public List<FormsCategory> GetCategoryList()
        {
            DocumentDAL uploadDal = new DocumentDAL();
            List<FormsCategory> categoryList = new List<FormsCategory>();
            categoryList = uploadDal.GetCategoryList();
            return categoryList;
        }
        public List<Forms> GetFormsListCategoryWise(int categoryId)
        {
            DocumentDAL uploadDal = new DocumentDAL();
            List<Forms> categoryList = new List<Forms>();
            categoryList = uploadDal.GetFormsListCategoryWise(categoryId);
            return categoryList;
        }
        public int SaveUploadedForms(List<Forms> formsList)
        {
            DocumentDAL documentDal = new DocumentDAL();

            return documentDal.SaveUploadedForms(formsList);

        }
        /// <summary>
        /// Old
        /// </summary>
        /// <param name="form"></param>
        /// <param name="approvers"></param>
        /// <param name="catchMessage"></param>
        /// <returns></returns>
        public int SaveFilledUpForm(Forms form, List<ApproverMaster> approvers, ref string catchMessage)
        {
            DocumentDAL documentDal = new DocumentDAL();

            return documentDal.SaveFilledUpForm(form, approvers,ref catchMessage);

        }
        /// <summary>
        /// new 
        /// </summary>
        /// <param name="form"></param>
        /// <param name="catchMessage"></param>
        /// <returns></returns>
        public int SaveFilledUpForm(Forms form,  ref string catchMessage)
        {
            DocumentDAL documentDal = new DocumentDAL();

            return documentDal. SaveFilledUpForm(form, ref catchMessage);

        }

        public int DeleteForm(string formName)
        {
            DocumentDAL documentDal = new DocumentDAL();

            return documentDal.DeleteForm(formName);
        }

        public List<Forms> GetFilledupFormRequiredApprovalList(int approverUserId)
        {
            DocumentDAL documentDal = new DocumentDAL();
            return documentDal.GetFilledupFormRequiredApprovalList(approverUserId);
        }

        public int ApproveFilledUpForm(int filledUpFormId,int approverUserId, string uploadedFormName)
        {
            DocumentDAL documentDal = new DocumentDAL();



            return documentDal.ApproveFilledUpForm(filledUpFormId, approverUserId, uploadedFormName);
        }

        public int ReviewedFilledUpForm(int approverUserId, string uploadedFormName)
        {
            DocumentDAL documentDal = new DocumentDAL();



            return documentDal.ReviewedFilledUpForm(approverUserId, uploadedFormName);
        }

        public ApprovedFilledupFormAndApproverViewModel GetApprovedFilledUpForms(int userId, int currentPage = 1)
        {
            DocumentDAL documentDal = new DocumentDAL();
            return documentDal.GetApprovedFilledUpForms(userId, currentPage);
        }

        public List<Forms> GetFillupFormsListForNotification(int aApproverUserId)
        {
            DocumentDAL documentDal = new DocumentDAL();
            return documentDal.GetFillupFormsListForNotification(aApproverUserId);
        }


        #region DropDown

        public List<Forms> GetFormsByCategoryForDropdown(int categoryId)
        {

            DocumentDAL documentDal = new DocumentDAL();
            return documentDal.GetFormsByCategoryForDropdown(categoryId);
        }

        #endregion



        public int SaveFilledUpFormsForCompanyApproval(Forms form, ref string catchMessage)
        {
            DocumentDAL documentDal = new DocumentDAL();

            return documentDal.SaveFilledUpFormsForCompanyApproval(form, ref catchMessage);

        }

    }
}
