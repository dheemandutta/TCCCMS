﻿using TCCCMS.Models;
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
        public int SaveFilledUpForm(Forms formsList)
        {
            DocumentDAL documentDal = new DocumentDAL();

            return documentDal.SaveFilledUpForm(formsList);

        }

        public int DeleteForm(string formName)
        {
            DocumentDAL documentDal = new DocumentDAL();

            return documentDal.DeleteForm(formName);
        }

        #region DropDown

        public List<Forms> GetFormsByCategoryForDropdown(int categoryId)
        {

            DocumentDAL documentDal = new DocumentDAL();
            return documentDal.GetFormsByCategoryForDropdown(categoryId);
        }

        #endregion
    }
}
