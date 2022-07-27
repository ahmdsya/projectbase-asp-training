using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace ProjectBase.Utilities
{
    public class SQL
    {
        public static string GetConnectionString(string pServer)
        {
            string connectionString = "";

            if ((string.IsNullOrEmpty(pServer)) || (pServer == "PRD"))
                connectionString = "Server=10.1.1.152\\SQLEXPRESS;Database=TechnicalSupportDashboard;User ID=sa; Password=server!2345;Max Pool Size=200;";
            else if ((pServer == "LOCALDEV"))
                connectionString = "Server=localhost\\SQLEXPRESS;Database=TechnicalSupportDashboard;User ID=sa; Password=123456;Max Pool Size=200;";
            else if ((pServer == "LOCALTEST"))
                connectionString = "Server=localhost\\SQLEXPRESS;Database=TSD;User ID=sa; Password=123456;Max Pool Size=200;";
            else if ((pServer == "QA"))
                connectionString = "Server=10.1.1.152\\SQLEXPRESS;Database=TechnicalSupportDashboardQA;User ID=sa; Password=server!2345;Max Pool Size=200;";

            return connectionString;
        }



        public static string ExecuteQuery(string pConnSetting,
                                          String query,
                                          Dictionary<string, object> parameters,
                                          Boolean useExistingConnection = false,
                                          SqlConnection conn = null,
                                          SqlTransaction trans = null)
        {
            ProjectBaseEntities db = new ProjectBaseEntities();


            string connString = string.IsNullOrEmpty(pConnSetting) ? db.Database.Connection.ConnectionString : GetConnectionString(pConnSetting);
            try
            {
                if (string.IsNullOrEmpty(connString))
                {
                    throw new Exception("Connection is invalid");
                }


                if (!useExistingConnection || conn == null)
                {
                    conn = new SqlConnection(connString);
                    conn.Open();
                }

                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    if (useExistingConnection && trans != null) command.Transaction = trans;

                    foreach (KeyValuePair<string, object> parameter in parameters)
                    {
                        command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                    }
                    command.CommandTimeout = 6000;
                    command.ExecuteNonQuery();
                }

                if (!useExistingConnection && trans != null)
                {
                    trans.Commit();
                }
            }
            catch (Exception exc)
            {
                if (!useExistingConnection && trans != null)
                {
                    trans.Rollback();
                }
                return exc.Message;
            }
            finally
            {
                if (!useExistingConnection)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            return "OK";
        }

        public static string GetDataTable(String pConnSetting,
                                          String query,
                                          Dictionary<string, object> parameters,
                                          out DataTable data, 
                                          Boolean useExistingConnection = false,
                                          SqlConnection conn = null,
                                          SqlTransaction trans = null,
                                          bool increaseTimeOut = false)
        {
            ProjectBaseEntities db = new ProjectBaseEntities();
            data = new DataTable();
            string connString = string.IsNullOrEmpty(pConnSetting) ? db.Database.Connection.ConnectionString : GetConnectionString(pConnSetting);

            try
            {
                if (string.IsNullOrEmpty(connString))
                {
                    throw new Exception("Connection is invalid");
                }

                if (!useExistingConnection || conn == null)
                {
                    conn = new SqlConnection(connString);
                    conn.Open();
                }

                using (SqlCommand command = new SqlCommand(query, conn))
                {
                    if (useExistingConnection && trans != null) command.Transaction = trans;
                    if (increaseTimeOut || true)
                        command.CommandTimeout = 300;
                    foreach (var parameter in parameters)
                    {
                        //command.Parameters.AddWithValue(parameter.Key, parameter.Value);
                        if (parameter.Value.GetType() == typeof(string))
                        {
                            command.Parameters.Add(new SqlParameter(parameter.Key, SqlDbType.VarChar, parameter.Value.ToString().Length, ParameterDirection.Input, false, 0, 0, "", DataRowVersion.Default, parameter.Value));
                        }
                        else
                            command.Parameters.AddWithValue(parameter.Key, parameter.Value);

                    }
                    using (SqlDataAdapter adapter = new SqlDataAdapter(command))
                    {
                        adapter.Fill(data);
                    }
                }
            }
            catch (Exception exc)
            {
                return exc.Message;
            }
            finally
            {
                if (!useExistingConnection)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
            return "OK";
        }
    }
}