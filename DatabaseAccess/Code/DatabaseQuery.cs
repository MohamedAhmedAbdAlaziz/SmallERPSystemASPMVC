using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DatabaseAccess.Code
{
 public  class DatabaseQuery
    {
        public static SqlConnection conn;

        public static SqlConnection ConnOpen()
        {
            if (conn == null)
            {
                var contring = "data source=.;initial catalog=CloudErpV1;integrated security=True;";
                //System.Configuration.ConfigurationManager.
                //    ConnectionStrings["CloudErpV1Entities"].ConnectionString;
                  conn = new SqlConnection(contring);
            //    conn = new SqlConnection(@"Data Source=dfdfbfdbfb");

            }
            if(conn.State != System.Data.ConnectionState.Open)
            {
                conn.Open();
            }
            return conn;
        }
        public static bool Insert(string query)
        {
            try
            {
                int noofrows = 0;
                SqlCommand cmb = new SqlCommand(query, ConnOpen());
                noofrows = cmb.ExecuteNonQuery();
                if(noofrows > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }

            catch { return false; }


        }
         public static bool Update(string query)
        {
            try
            {
                int noofrows = 0;
                SqlCommand cmb = new SqlCommand(query, ConnOpen());
                noofrows = cmb.ExecuteNonQuery();
                if(noofrows > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }

            catch { return false; }


        }
    
  public static bool Delete(string query)
        {
            try
            {
                int noofrows = 0;
                SqlCommand cmb = new SqlCommand(query, ConnOpen());
                noofrows = cmb.ExecuteNonQuery();
                if(noofrows > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }

            catch { return false; }


        }
            
  public static DataTable Retrive(string query)
        {
            try
            {
                DataTable dt = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(query, ConnOpen());
                da.Fill(dt);
                return dt;

            }

            catch { return null; }


        }

        public static bool ExpiryProduct()
        {
            using(SqlCommand cmd = new SqlCommand("GetExpriyProductList", ConnOpen()))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                using(SqlDataReader dr = cmd.ExecuteReader())
                {
                    if (dr.Read())
                    {
                        return true;
                    }
                    return false;
                }
            }
        }


    

    }
}
