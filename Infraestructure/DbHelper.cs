using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infraestructure
{
    public class DbHelper
    {
        private static SqlConnection connection;
        //private static DbHelper _instance;
        public DbHelper()
        {
            connection = new SqlConnection(ConfigHelper.GetInstance().GetConnectionStrings("interview-test"));
        }

        //public static DbHelper GetInstance()
        //{
        //    if(_instance is null)
        //        _instance = new DbHelper();

        //    return _instance;
        //}
        
        public DataTable EjecutarQuery(CommandType cmdType, string cmdText, ICollection<SqlParameter> parameters)
        {
            DataTable dt = new DataTable();
            try
            {

                using (connection)
                {
                    connection.Open();

                    SqlCommand mysqlCmd = IniciarMySqlCmd(cmdType, cmdText, parameters);

                    SqlDataReader reader = mysqlCmd.ExecuteReader();

                    dt.Load(reader);
                }
            }
            catch (SqlException sqllEx)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }

            return dt;
        }

        public DataTable EjecutarQuery(CommandType cmdType, string cmdText)
        {
            DataTable dt = new DataTable();
            try
            {

                using (connection)
                {
                    connection.Open();

                    SqlCommand mysqlCmd = IniciarMySqlCmd(cmdType, cmdText);



                    SqlDataReader reader = mysqlCmd.ExecuteReader();


                    dt.Load(reader);
                }
            }
            catch (SqlException sqllEx)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }

            return dt;
        }

        private SqlCommand IniciarMySqlCmd(CommandType cmdType, string cmdText)
        {

            SqlCommand mysqlCmd = connection.CreateCommand();
            mysqlCmd.CommandType = cmdType;
            mysqlCmd.CommandTimeout = 90;
            mysqlCmd.CommandText = cmdText;


            return mysqlCmd;

        }

        private SqlCommand IniciarMySqlCmd(CommandType cmdType, string cmdText, ICollection<SqlParameter> parametros)
        {
            SqlCommand mysqlCmd = connection.CreateCommand();
            mysqlCmd.CommandType = cmdType;

            foreach (SqlParameter param in parametros)
            {
                mysqlCmd.Parameters.Add(param);
            }
            mysqlCmd.CommandText = cmdText;
            return mysqlCmd;

        }

        public int ExecuteNonQuery(CommandType cmdType, string query)
        {
            int insertUpdateResult = -1;

            try
            {

                using (connection)
                {
                    connection.Open();

                    SqlCommand sqlCmd = IniciarMySqlCmd(cmdType, query);

                    sqlCmd.Transaction = connection.BeginTransaction();

                    int result = sqlCmd.ExecuteNonQuery();

                    if (sqlCmd.CommandText.ToLower().Contains("update"))
                    {
                        if (result >= 0)
                        {
                            insertUpdateResult = result;
                            sqlCmd.Transaction.Commit();
                        }
                        else
                        {
                            sqlCmd.Transaction.Rollback();
                        }
                    }
                    else
                    {
                        if (result == -1 || result > 1)
                        {
                            sqlCmd.Transaction.Rollback();
                        }
                        else
                        {
                            
                            insertUpdateResult = result;
                            sqlCmd.Transaction.Commit();
                        }
                    }

                }
            }
            catch (SqlException sqllEx)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }

            return insertUpdateResult;
        }

        public object ExecuteScalar(CommandType cmdType, string query)
        {
            object result = null;
            try
            {

                using (connection)
                {
                    connection.Open();

                    SqlCommand mysqlCmd = IniciarMySqlCmd(cmdType, query);

                    mysqlCmd.Transaction = connection.BeginTransaction();

                    result = mysqlCmd.ExecuteScalar();

                   
                    if (result is null)
                    {
                        mysqlCmd.Transaction.Rollback();
                    }
                    else
                    {
                        //insertUpdateResult = mysqlCmd.LastInsertedId;

                        mysqlCmd.Transaction.Commit();
                    }
                    

                }
            }
            catch (SqlException sqllEx)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }

            return result;
        }
    }
}
