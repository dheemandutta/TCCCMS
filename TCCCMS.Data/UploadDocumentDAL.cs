using TCCCMS.Models;

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
    public class UploadDocumentDAL
    {
        string connectionString = ConfigurationManager.ConnectionStrings["TCCCMSDBConnectionString"].ConnectionString;

        public List<FormsCategory> GetCategoryList()
        {
            List<FormsCategory> categoryList = new List<FormsCategory>();

            using (SqlConnection con = new SqlConnection(connectionString))
            {
                using (SqlCommand cmd = new SqlCommand("GetCategoryList", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    con.Open();

                    DataSet ds          = new DataSet();
                    SqlDataAdapter da   = new SqlDataAdapter(cmd);
                    da.Fill(ds);

                    foreach (DataRow dr in ds.Tables[0].Rows)
                    {
                        categoryList.Add(new FormsCategory
                        {
                            ID              = Convert.ToInt32(dr["ID"]),
                            CatecoryName    = Convert.ToString(dr["CatecoryName"]),
                            Description     = Convert.ToString(dr["Description"])
                        });
                    }
                    con.Close();
                }
            }
            return categoryList;

        }

        public int SaveUploadedForms(List<Forms> formsList)
        {
            int recorSaved = 0;
            foreach(Forms form in formsList)
            {
                SqlConnection con = new SqlConnection(connectionString);
                con.Open();
                SqlCommand cmd  = new SqlCommand("SaveFormsDetails", con);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Name",        form.FormName);
                cmd.Parameters.AddWithValue("@Path",        form.FilePath);
                cmd.Parameters.AddWithValue("@CategoryId",  form.CategoryId);
                cmd.Parameters.AddWithValue("@Description", form.FormName);
                cmd.Parameters.AddWithValue("@User",        form.CreateedBy);
                int x = cmd.ExecuteNonQuery();
                con.Close();
                recorSaved = recorSaved + x;
            }

            

            return recorSaved;
        }
    }
}
