using System;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace AS.VW.Scheduler.G2ML
{
    public class DBConnection
    {
        private Boolean connectioned;
        private SqlConnection connection;
        public DBConnection()
        {
        }

        public bool openConnection()
        {
            try
            {
                if (!connectioned)
                {
                    var connectionString = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                    connection = new SqlConnection(connectionString);
                    connection.Open();
                    connectioned = true;
                }

            }
            catch (SqlException)
            {
                connectioned = false;
            }
            return connectioned;
        }

        public DataTable Execute(string sql, params SqlParameter[] spar)
        {
            DataTable dt = new DataTable();
            try
            {
                if (!connectioned)
                {
                    openConnection();
                }
                SqlCommand cmd = new SqlCommand(sql, connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.Parameters.AddRange(spar);
                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adp.Fill(ds);
                dt = ds.Tables[0];
                return dt;
            }
            catch
            {
                return dt;
            }

        }

        public int ExecuteNonQuery(string sql, params SqlParameter[] spar)
        {
            try
            {
                if (!connectioned)
                {
                    openConnection();
                }
                SqlCommand cmd;
                cmd = new SqlCommand(sql, connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Clear();
                cmd.Parameters.AddRange(spar);
                cmd.ExecuteNonQuery();
                return 1;
            }
            catch
            {
                return 0;
            }

        }
    }
}
