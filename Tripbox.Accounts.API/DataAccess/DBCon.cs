using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Tripbox.Accounts.API.DataAccess
{
    public class DBCon
    {
        private SqlConnection SqlCon = null;

        public DBCon(string _ConnectionString)
        {
            try
            {
                SqlCon = new SqlConnection(_ConnectionString);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void DBCon_Close()
        {
            if (SqlCon.State == ConnectionState.Open)
            {
                SqlCon.Close();
            }

            SqlCon.Dispose();
        }

        public DataSet FillDataSet(string _query, SqlParameter[] _params, CommandType _type)
        {
            DataSet ds = new DataSet();

            try
            {
                SqlDataAdapter dsAdapter = new SqlDataAdapter(_query, SqlCon);
                dsAdapter.SelectCommand.CommandTimeout = 300;
                dsAdapter.SelectCommand.CommandType = _type;

                ds = new DataSet();

                if (_params != null)
                {
                    foreach (SqlParameter param in _params)
                    {
                        dsAdapter.SelectCommand.Parameters.Add(param);
                    }
                }

                if (SqlCon.State == ConnectionState.Closed)
                {

                    SqlCon.Open();
                }
                dsAdapter.Fill(ds);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (SqlCon.State == ConnectionState.Open)
                {
                    SqlCon.Close();
                }

                SqlCon.Dispose();
            }

            return ds;

        }

        public void ExecuteNonQuery(string _query, SqlParameter[] _params, CommandType _type)
        {
            int nRnt = -1;

            try
            {
                SqlCommand sqlcmd = new SqlCommand(_query, SqlCon);
                sqlcmd.CommandTimeout = 300;
                sqlcmd.CommandType = _type;

                if (_params != null)
                {
                    foreach (SqlParameter param in _params)
                    {
                        sqlcmd.Parameters.Add(param);
                    }
                }

                if (SqlCon.State == ConnectionState.Closed)
                {
                    SqlCon.Open();
                }

                nRnt = sqlcmd.ExecuteNonQuery();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (SqlCon.State == ConnectionState.Open)
                {
                    SqlCon.Close();
                }

                SqlCon.Dispose();
            }
        }


        public Hashtable ExecuteNonQuery(string _query, SqlParameter[] _params, CommandType _type, params string[] _outparams)
        {
            int nRnt = -1;

            Hashtable ht = new Hashtable();

            try
            {
                SqlCommand sqlcmd = new SqlCommand(_query, SqlCon);
                sqlcmd.CommandTimeout = 300;
                sqlcmd.CommandType = _type;

                if (_params != null)
                {
                    foreach (SqlParameter param in _params)
                    {
                        sqlcmd.Parameters.Add(param);
                    }
                }

                if (SqlCon.State == ConnectionState.Closed)
                {
                    SqlCon.Open();
                }

                nRnt = sqlcmd.ExecuteNonQuery();

                if (_outparams.Length > 0)
                {
                    for (int lCnt = 0; lCnt < _outparams.Length; lCnt++)
                    {
                        ht.Add(_outparams[lCnt].ToString(), sqlcmd.Parameters[_outparams[lCnt].ToString()].Value.ToString());
                    }
                }

                return ht;

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (SqlCon.State == ConnectionState.Open)
                {
                    SqlCon.Close();
                }

                SqlCon.Dispose();
            }

        }
    }
}
