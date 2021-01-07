using TCCCMS.Models;
using TCCCMS.Data;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TCCCMS.Business
{
    public class UploadDocumentBL
    {
        public List<FormsCategory> GetCategoryList()
        {
            UploadDocumentDAL uploadDal = new UploadDocumentDAL();
            List<FormsCategory> categoryList = new List<FormsCategory>();
            categoryList = uploadDal.GetCategoryList();
            return categoryList;
        }
        public int SaveUploadedForms(List<Forms> formsList)
        {
            UploadDocumentDAL uploadDal = new UploadDocumentDAL();

            return uploadDal.SaveUploadedForms(formsList);

        }
    }
}
