﻿using TCCCMS.Models;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Configuration;

namespace TCCCMS.Data
{
    public class DocumentDAL
    {
        string connectionString = ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString;
        string strXMLApprovers = "";

        public List<FormsCategory> GetCategoryList()
        {
            List<FormsCategory> categoryList = new List<FormsCategory>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetCategoryList", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();

                    DataSet ds = new DataSet();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        categoryList.Add(new FormsCategory
                        {
                            ID = Convert.ToInt32(dr["ID"]),
                            CatecoryName = Convert.ToString(dr["CatecoryName"]),
                            Description = Convert.ToString(dr["Description"])
                        });
                    }
                    con.Close();
                }
            }
            return categoryList;

        }

        public List<Forms> GetFormsListCategoryWise(int categoryId)
        {
            List<Forms> formsList = new List<Forms>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetFormsListCategoryWise", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CategoryId",categoryId);
                    con.Open();

                    DataSet ds = new DataSet();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        formsList.Add(new Forms
                        {
                            ID          = Convert.ToInt32(dr["ID"]),
                            RowNumber   = Convert.ToInt32(dr["RowNumber"]),
                            FormName    = Convert.ToString(dr["FormName"]),
                            Description = Convert.ToString(dr["Description"]),
                            FilePath    = Convert.ToString(dr["Path"])
                        });
                    }
                    con.Close();
                }
            }
            return formsList;

        }

        public int SaveUploadedForms(List<Forms> formsList)
        {
            int recorSaved = 0;
            foreach (Forms form in formsList)
            {
                SqlConnection con = new SqlConnection(connectionString);
                con.Open();
                SqlCommand cmd = new SqlCommand("SaveFormsDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Name", form.FormName);
                cmd.Parameters.AddWithValue("@Path", form.FilePath);
                cmd.Parameters.AddWithValue("@CategoryId", form.CategoryId);
                cmd.Parameters.AddWithValue("@Description", form.FormName);
                cmd.Parameters.AddWithValue("@Version", form.Version);
                cmd.Parameters.AddWithValue("@User", form.CreateedBy);

                cmd.Parameters.AddWithValue("@ModifiedSection", form.ModifiedSection);

                int x = cmd.ExecuteNonQuery();
                con.Close();
                recorSaved = recorSaved + x;
            }



            return recorSaved;
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
            int recorSaved = 0;
            if(GenerateApproverXML(approvers, ref catchMessage))
            {
                try
                {
                    SqlConnection con = new SqlConnection(connectionString);
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SaveFilledUpFormsForApproval", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Name", form.FilledUpFormName);
                    cmd.Parameters.AddWithValue("@Path", form.FilePath);
                    cmd.Parameters.AddWithValue("@ShipId", form.CategoryId);
                    cmd.Parameters.AddWithValue("@xmlApprovers", strXMLApprovers);
                    cmd.Parameters.AddWithValue("@OriginalForm", form.FormName);
                    cmd.Parameters.AddWithValue("@User", form.CreateedBy);
                    int x = cmd.ExecuteNonQuery();
                    con.Close();
                    //recorSaved = recorSaved + x;
                    recorSaved = x;



                    return recorSaved;
                }
                catch(Exception expErr)
                {
                    recorSaved = 0;
                    catchMessage = expErr.Message;
                }
            }

            return recorSaved;
        }

        /// <summary>
        /// New
        /// </summary>
        /// <param name="form"></param>
        /// <param name="approvers"></param>
        /// <param name="catchMessage"></param>
        /// <returns></returns>
        public int SaveFilledUpForm(Forms form, ref string catchMessage)
        {
            int recorSaved = 0;
           
                try
                {
                    SqlConnection con = new SqlConnection(connectionString);
                    con.Open();
                    SqlCommand cmd = new SqlCommand("SaveFilledUpFormsForCompanyApproval", con);
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@Name",            form.FilledUpFormName);
                    cmd.Parameters.AddWithValue("@Path",            form.FilePath);
                    cmd.Parameters.AddWithValue("@ShipId",          form.ShipId);
                    cmd.Parameters.AddWithValue("@OriginalForm",    form.FormName);
                    cmd.Parameters.AddWithValue("@User",            form.CreateedBy);
                    int x = cmd.ExecuteNonQuery();
                    con.Close();
                    //recorSaved = recorSaved + x;
                    recorSaved = x;



                    return recorSaved;
                }
                catch (Exception expErr)
                {
                    recorSaved = 0;
                    catchMessage = expErr.Message;
                }
          

            return recorSaved;
        }


        public int DeleteForm(string formName)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("DeleteForms", con);
            cmd.CommandType = CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@FormName", formName);

            int recordsAffected = cmd.ExecuteNonQuery();
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);

            con.Close();
            return recordsAffected;
        }

        #region DropDown
        
        public List<Forms> GetFormsByCategoryForDropdown(int categoryId)
        {
            List<Forms> formsList = new List<Forms>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetFormsListByCategoryForDopDown", con))
                {
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@CategoryId", categoryId);


                    DataSet ds = new DataSet();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        formsList.Add(new Forms
                        {
                            ID = Convert.ToInt32(dr["ID"]),
                            FormName = Convert.ToString(dr["Name"]),

                        });
                    }
                    con.Close();
                }
            }
            return formsList;

        }
        #endregion

        private bool GenerateApproverXML(List<ApproverMaster> approvers, ref string CatchMessage)
        {
            
            bool bReturn = false;
            StringBuilder sbXML = new StringBuilder();

            try
            {
                if (approvers.Count > 0)
                {
                    int i = 0;
                    sbXML.Append("<approver>");

                    foreach(ApproverMaster approver in approvers)
                    {
                        sbXML.Append("<row>");
                        sbXML.Append("<approverId>" + approver.ApproverId.ToString() + "</approverId>");
                        sbXML.Append("<userId>" + approver.UserId.ToString() + "</userId>");
                        sbXML.Append("<row_id>" + Convert.ToString(i + 1) + "</row_id>");
                        sbXML.Append("</row>");

                        i = i + 1;
                    }


                    //for (int i = 0; i < ArrObj.Length; i = i + 1)
                    //{
                    //    sbXML.Append("<row>");
                    //    sbXML.Append("<device_id>" + ArrObj[i].ID.ToString() + "</device_id>");
                    //    sbXML.Append("<manufacturer><![CDATA[" + ArrObj[i].MANUFACTURER + "]]></manufacturer>");
                    //    sbXML.Append("<modality><![CDATA[" + ArrObj[i].MODALITY + "]]></modality>");
                    //    sbXML.Append("<modality_ae_title><![CDATA[" + ArrObj[i].MODALITY_AE_TITLE + "]]></modality_ae_title>");
                    //    sbXML.Append("<weight_uom><![CDATA[" + ArrObj[i].WEIGHT_UOM + "]]></weight_uom>");
                    //    sbXML.Append("<row_id>" + Convert.ToString(i + 1) + "</row_id>");
                    //    sbXML.Append("</row>");
                    //}

                    sbXML.Append("</approver>");
                    strXMLApprovers = sbXML.ToString();


                }
                bReturn = true;
            }
            catch (Exception expErr)
            {
                bReturn = false;
                CatchMessage = expErr.Message;
            }
            return bReturn;
        }

        public List<Forms> GetFilledupFormList()
        {
            List<Forms> filledupFormList = new List<Forms>();


            return filledupFormList;

        }

        public List<Forms> GetFilledupFormRequiredApprovalList(int approverUserId)
        {
            List<Forms> filledupFormList = new List<Forms>();
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetFilledupFormRequiredApprovalListPageWise", con))
                {   ///----------------
                    con.Open();
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ApproverUserId", approverUserId);
                    DataSet ds = new DataSet();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        Ship ship           = new Ship();
                        ship.ID             = Convert.ToInt32(dr["ShipId"]);
                        ship.ShipName       = Convert.ToString(dr["ShipName"]);

                        UserMasterPOCO user = new UserMasterPOCO();
                        user.UserId         = Convert.ToInt32(dr["UserId"]);
                        user.UserName       = Convert.ToString(dr["UserName"]);

                        filledupFormList.Add(new Forms
                        {
                            ID                  = Convert.ToInt32(dr["ID"]),//--FilledUp Form Id
                            FilledUpFormName    = Convert.ToString(dr["FilledupFormName"]),
                            FilePath            = Convert.ToString(dr["FormsPath"]),

                            ShipId              = Convert.ToInt32(dr["ShipId"]),
                            Ship                = ship,

                            User                = user,
                        }); ;
                    }
                    con.Close();
                }
            }

                    
            return filledupFormList;

        }

        public int ApproveFilledUpForm(int filledUpFormId, int approverUserId)
        {
            int recorSaved = 0;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("ApproveFilledupForm", con))
                {
                    try
                    {

                        con.Open();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@ApproverUserId", approverUserId);
                        cmd.Parameters.AddWithValue("@FilledUpFormId", filledUpFormId);

                        int x = cmd.ExecuteNonQuery();
                        con.Close();
                        recorSaved = x;
                        return recorSaved;
                    }
                    catch (Exception expErr)
                    {
                        recorSaved = 0;
                       // catchMessage = expErr.Message;
                    }
                }
            }

            return recorSaved;
        }
    }
}
