﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using TCCCMS.Models;

namespace TCCCMS.Data
{
    public class ApproverSignDAL
    {
        public int SaveApproverSign(ApproverMaster pOCO)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("SaveApproverSign", con);
            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.AddWithValue("@ApproverUserId", pOCO.ApproverUserId);
            cmd.Parameters.AddWithValue("@SignImagePath", pOCO.SignImagePath.ToString());
            cmd.Parameters.AddWithValue("@Name", pOCO.Name.ToString());
            cmd.Parameters.AddWithValue("@Position", pOCO.Position.ToString());
            //cmd.Parameters.AddWithValue("@CreatedOn", pOCO.CreatedOn1.ToString());
            //cmd.Parameters.AddWithValue("@ModifiedOn", pOCO.ModifiedOn1.ToString());

            if (pOCO.Id > 0)
            {
                cmd.Parameters.AddWithValue("@Id ", pOCO.Id);
            }
            else
            {
                cmd.Parameters.AddWithValue("@Id ", DBNull.Value);
            }

            int recordsAffected = cmd.ExecuteNonQuery();
            con.Close();

            return recordsAffected;
        }

        public List<ApproverMaster> GetAllApproverSignPageWise(int pageIndex, ref int totalCount, int length/*, int VesselID*/)
        {
            List<ApproverMaster> pOList = new List<ApproverMaster>();
            List<ApproverMaster> equipmentsPO = new List<ApproverMaster>();

            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetAllApproverSignPageWise", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@PageIndex", pageIndex);
                    cmd.Parameters.AddWithValue("@PageSize", length);
                    //cmd.Parameters.Add("@RecordCount", SqlDbType.Int, 4);
                    //cmd.Parameters["@RecordCount"].Direction = ParameterDirection.Output;
                    cmd.Parameters.Add("@TotalCount", SqlDbType.Int, 4);
                    cmd.Parameters["@TotalCount"].Direction = ParameterDirection.Output;
                    //cmd.Parameters.AddWithValue("@VesselID", VesselID);
                    con.Open();

                    DataSet ds = new DataSet();
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    //prodPOList = Common.CommonDAL.ConvertDataTable<ProductPOCO>(ds.Tables[0]);

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        pOList.Add(new ApproverMaster
                        {
                            //Id = Convert.ToInt32(dr["Id"]),
                            UserName = Convert.ToString(dr["UserName"]),
                            SignImagePath = Convert.ToString(dr["SignImagePath"]),
                            Name = Convert.ToString(dr["Name"]),
                            Position = Convert.ToString(dr["Position"])
                        });
                    }
                    totalCount = Convert.ToInt32(cmd.Parameters["@TotalCount"].Value);
                    con.Close();
                }
            }
            return pOList;
        }

        public ApproverMaster GetAllApproverSign(int ApproverUserId, string uploadedFormName = null)
        {
            ApproverMaster prodPOList = new ApproverMaster();
            ApproverMaster prodPO = new ApproverMaster();
            DataSet ds = new DataSet();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetAllApproverSign", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@ApproverUserId", ApproverUserId);
                    cmd.Parameters.AddWithValue("@UploadedFormName", uploadedFormName);

                    con.Open();

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(ds);
                    con.Close();
                }
            }
            return ConvertDataTableToApproverSignByIdList(ds);
        }

        private ApproverMaster ConvertDataTableToApproverSignByIdList(DataSet ds)
        {
            ApproverMaster pPOCOPC = new ApproverMaster();
            //check if there is at all any data
            if (ds.Tables.Count > 0)
            {
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    //RankPOCO pPOCOPC = new RankPOCO();

                    //if (item["Id"] != null)
                    //    pPOCOPC.Id = Convert.ToInt32(item["Id"].ToString());

                    if (item["ApproverUserId"] != System.DBNull.Value)
                        pPOCOPC.ApproverUserId = Convert.ToInt32(item["ApproverUserId"].ToString());

                    if (item["UserName"] != System.DBNull.Value)
                        pPOCOPC.Name = item["UserName"].ToString();

                    if (item["SignImagePath"] != System.DBNull.Value)
                        pPOCOPC.SignImagePath = item["SignImagePath"].ToString();

                    if (item["Position"] != System.DBNull.Value)
                        pPOCOPC.Position = item["Position"].ToString();

                    //if (item["CreatedOn"] != System.DBNull.Value)
                    //    pPOCOPC.CreatedOn1 = item["CreatedOn"].ToString();

                    //if (item["ModifiedOn"] != System.DBNull.Value)
                    //    pPOCOPC.ModifiedOn1 = item["ModifiedOn"].ToString();

                    //pcList.Add(pPOCOPC);
                }
                pPOCOPC.ApprovedCount = Convert.ToInt32(ds.Tables[1].Rows[0]["ApprovedCount"].ToString());
                pPOCOPC.ApproversCount = Convert.ToInt32(ds.Tables[2].Rows[0]["ApproversCount"].ToString());
                if(Convert.ToInt32(ds.Tables[3].Rows[0]["IsApprove"].ToString()) >0)
                {
                    pPOCOPC.IsFinalApproved = Convert.ToInt32(ds.Tables[3].Rows[0]["IsApprove"].ToString());
                    pPOCOPC.FinalApprovedOn = Convert.ToDateTime(ds.Tables[3].Rows[0]["ApprovedOn"].ToString());
                }
                
            }
            return pPOCOPC;
        }


        //for ApproverSignUser drp
        public List<ApproverMaster> GetAllUserForDrpApproverSign(/*int VesselID*/)
        {
            SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString);
            con.Open();
            SqlCommand cmd = new SqlCommand("usp_GetAllUserForDrpApproverSign", con);
            cmd.CommandType = CommandType.StoredProcedure;
            //cmd.Parameters.AddWithValue("@VesselID", VesselID);
            DataSet ds = new DataSet();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            da.Fill(ds);
            DataTable myTable = ds.Tables[0];
            List<ApproverMaster> ranksList = myTable.AsEnumerable().Select(m => new ApproverMaster()
            {
                UserId = m.Field<int>("UserId"),
                UserName = m.Field<string>("UserName")

            }).ToList();
            con.Close();
            return ranksList;

        }
    }
}
