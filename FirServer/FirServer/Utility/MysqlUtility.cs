using System;
using System.Data;
using MySql.Data.MySqlClient;
using FirServer.Defines;

namespace FirServer.Utility
{
    public class MysqlUtility
    {
        private static MySqlConnection connection;
        private static string connString = "server=" + AppConst.MySQL_Host +
                                           ";database=" + AppConst.MySQL_DB + 
                                           ";uid=" + AppConst.MySQL_User + 
                                           ";pwd=" + AppConst.MySQL_Pass + ";SslMode=none;";

        public static void Initialize()
        {
            if (AppConst.MySQLMode)
            {
                connection = new MySqlConnection(connString);
                connection.Open();
            }
        }

        public static int ExecuteSql(string strsql, MySqlParameter[] sqlParams = null)
        {
            if (connection == null)
            {
                return 0;
            }
            using (MySqlCommand cmd = new MySqlCommand(strsql, connection))
            {
                if (sqlParams != null)
                {
                    cmd.Parameters.AddRange(sqlParams);
                }
                try
                {
                    return cmd.ExecuteNonQuery();
                }
                catch (MySqlException e)
                {
                    throw e;
                }
            }
        }

        public static MySqlDataReader ExecuteReader(string strsql, MySqlParameter[] sqlParams = null)
        {
            if (connection == null)
            {
                return null;
            }
            using (MySqlCommand cmd = new MySqlCommand(strsql, connection))
            {
                if (sqlParams != null)
                {
                    cmd.Parameters.AddRange(sqlParams);
                }
                try
                {
                    return cmd.ExecuteReader();
                }
                catch (MySqlException e)
                {
                    throw e;
                }
            }
        }

        public static DataSet ExecuteQuery(string strsql, MySqlParameter[] sqlParams = null)
        {
            if (connection == null)
            {
                return null;
            }
            using (MySqlCommand cmd = new MySqlCommand(strsql, connection))
            {
                if (sqlParams != null)
                {
                    cmd.Parameters.AddRange(sqlParams);
                }
                using (DataSet ds = new DataSet())
                {
                    try
                    {
                        var command = new MySqlDataAdapter(cmd);
                        command.Fill(ds, "ds");
                    }
                    catch (MySqlException ex)
                    {
                        throw new Exception(ex.Message);
                    }
                    return ds;
                }
            }
        }

        public static void Close()
        {
            if (connection != null)
            {
                connection.Close();
            }
        }
    }
}
