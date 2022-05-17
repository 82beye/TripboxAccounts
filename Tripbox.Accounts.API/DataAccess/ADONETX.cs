using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Tripbox.Accounts.API.DataAccess
{
    public class ADONETX
    {
        private SqlDbType DB타입(string 타입값)
        {
            SqlDbType 타입;

            if (타입값 == "SqlDbType.BigInt")
            {
                타입 = SqlDbType.BigInt;
            }
            else if (타입값 == "SqlDbType.Binary")
            {
                타입 = SqlDbType.Binary;
            }
            else if (타입값 == "SqlDbType.Bit")
            {
                타입 = SqlDbType.Bit;
            }
            else if (타입값 == "SqlDbType.Char")
            {
                타입 = SqlDbType.Char;
            }
            else if (타입값 == "SqlDbType.Date")
            {
                타입 = SqlDbType.Date;
            }
            else if (타입값 == "SqlDbType.DateTime")
            {
                타입 = SqlDbType.DateTime;
            }
            else if (타입값 == "SqlDbType.DateTime2")
            {
                타입 = SqlDbType.DateTime2;
            }
            else if (타입값 == "SqlDbType.Decimal")
            {
                타입 = SqlDbType.Decimal;
            }
            else if (타입값 == "SqlDbType.Float")
            {
                타입 = SqlDbType.Float;
            }
            else if (타입값 == "SqlDbType.Image")
            {
                타입 = SqlDbType.Image;
            }
            else if (타입값 == "SqlDbType.Int")
            {
                타입 = SqlDbType.Int;
            }
            else if (타입값 == "SqlDbType.Money")
            {
                타입 = SqlDbType.Money;
            }
            else if (타입값 == "SqlD bType.NChar")
            {
                타입 = SqlDbType.NChar;
            }
            else if (타입값 == "SqlDbType.NText")
            {
                타입 = SqlDbType.NText;
            }
            else if (타입값 == "SqlDbType.NVarChar")
            {
                타입 = SqlDbType.NVarChar;
            }
            else if (타입값 == "SqlDbType.Real")
            {
                타입 = SqlDbType.Real;
            }
            else if (타입값 == "SqlDbType.VarChar")
            {
                타입 = SqlDbType.VarChar;
            }
            else if (타입값 == "SqlDbType.Text")
            {
                타입 = SqlDbType.Text;
            }
            else if (타입값 == "SqlDbType.Structured")
            {
                타입 = SqlDbType.Structured;
            }
            else
            {
                타입 = SqlDbType.Int;
            }
            return 타입;
        }
        //

        private string DB연결문자(string DB, string 프로그램버전)
        {
            string v연결값 = "";
            string Workstationid = "";

            if (프로그램버전.IndexOf("|") >= 0)
            {
                Workstationid = 프로그램버전.Substring(프로그램버전.IndexOf("|") + 1);
                프로그램버전 = 프로그램버전.Substring(0, 프로그램버전.IndexOf("|"));
            }
            else
            {
                Workstationid = "NoData";
            }

            if (DB == "Tripbox")
            {
                // v연결값 = "Server=tcp:210.116.96.86;Initial Catalog=TripBox;User ID=tripbox;Password=Jjong0105!;";

                //v연결값 = "Server=tcp:tripbox.sldb.iwinv.net;Initial Catalog=tripbox;User ID=tripbox_erp;Password=Tripbox0105!;";
                v연결값 = "Server=tcp:49.247.36.167,1952;Initial Catalog=tripbox;User ID=tripbox_erp;Password=Tripbox0105!;";
                //v연결값 = "Server=tcp:tripbox.sldb.iwinv.net;Initial Catalog=tripboxbak;User ID=tripbox_erp;Password=Jjong0105!;";
            }
            else if (DB == "Tripboxbak")
            {
                // v연결값 = "Server=tcp:210.116.96.86;Initial Catalog=TripBox;User ID=tripbox;Password=Jjong0105!;";

                v연결값 = "Server=tcp:tripbox.sldb.iwinv.net;Initial Catalog=Tripboxbak;User ID=tripbox_erp;Password=Jjong0105!;";
            }
            else
            {
                v연결값 = "";
            }

            return v연결값;
        }

        public DataSet 쿼리FillDataSet(string 프로시저명, DataSet 파라미터, string 프로그램버전)
        {
            int i = 0;
            DBCon dbCon = null;
            DataSet ds = new DataSet();
            SqlParameter[] sqlParam = null;

            try
            {

                dbCon = new DBCon(DB연결문자("Tripbox", 프로그램버전));
                sqlParam = new SqlParameter[파라미터.Tables[0].Rows.Count];

                foreach (DataRow dr in 파라미터.Tables[0].Rows)
                {

                    sqlParam[i] = new SqlParameter();
                    sqlParam[i].ParameterName = dr["컬럼명"].ToString();

                    //sqlParam[i].Value = dr["값"].ToString();
                    if (dr["타입"].ToString() == string.Empty)
                    {
                        if (dr["값"].ToString().ToUpper() == "NULL")
                        {
                            sqlParam[i].Value = DBNull.Value;
                        }
                        else
                        {
                            sqlParam[i].Value = dr["값"].ToString();
                        }
                    }
                    else
                    {
                        sqlParam[i].SqlDbType = DB타입(dr["타입"].ToString());
                        sqlParam[i].Value = (DataTable)dr["리스트값"];
                    }

                    i++;

                }

                ds = dbCon.FillDataSet(프로시저명, sqlParam, CommandType.StoredProcedure);

                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbCon.DBCon_Close();
            }
        }

        public void 쿼리ExecuteNonQuery(bool 트랜잭션, string 프로시저명, DataSet 파라미터, string 프로그램버전)
        {
            int i = 0;
            SqlParameter[] sqlParam = null;

            if (트랜잭션 == false)
            {
                DBCon dbCon = null;

                try
                {

                    dbCon = new DBCon(DB연결문자("Tripbox", 프로그램버전));
                    sqlParam = new SqlParameter[파라미터.Tables[0].Rows.Count];

                    foreach (DataRow dr in 파라미터.Tables[0].Rows)
                    {

                        sqlParam[i] = new SqlParameter();
                        sqlParam[i].ParameterName = dr["컬럼명"].ToString();

                        if (dr["타입"].ToString() == string.Empty)
                        {
                            //sqlParam[i].Value = dr["값"].ToString();
                            if (dr["값"].ToString().ToUpper() == "NULL")
                            {
                                sqlParam[i].Value = DBNull.Value;
                            }
                            else
                            {
                                sqlParam[i].Value = dr["값"].ToString();
                            }
                        }
                        else
                        {
                            sqlParam[i].SqlDbType = DB타입(dr["타입"].ToString());
                            sqlParam[i].Value = (DataTable)dr["리스트값"];
                        }
                        i++;

                    }

                    dbCon.ExecuteNonQuery(프로시저명, sqlParam, CommandType.StoredProcedure);

                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dbCon.DBCon_Close();
                }
            }

        }

        public void 쿼리ExecuteNonQueryOut(string 프로시저명, DataSet 파라미터, string 프로그램버전, ref string Output값)
        {
            int i = 0;
            SqlParameter[] sqlParam = null;
            Hashtable ht = new Hashtable();

            DBCon dbCon = null;

            try
            {

                dbCon = new DBCon(DB연결문자("Tripbox", 프로그램버전));
                sqlParam = new SqlParameter[파라미터.Tables[0].Rows.Count];

                foreach (DataRow dr in 파라미터.Tables[0].Rows)
                {

                    sqlParam[i] = new SqlParameter();
                    sqlParam[i].ParameterName = dr["컬럼명"].ToString();

                    if (dr["타입"].ToString() == string.Empty)
                    {
                        if (dr["값"].ToString().ToUpper() == "NULL")
                        {
                            sqlParam[i].Value = DBNull.Value;
                        }
                        else
                        {
                            sqlParam[i].Value = dr["값"].ToString();
                        }
                    }
                    else
                    {
                        sqlParam[i].SqlDbType = DB타입(dr["타입"].ToString());
                        sqlParam[i].Value = (DataTable)dr["리스트값"];
                    }


                    if (Output값.Equals(dr["컬럼명"].ToString()))
                    {
                        sqlParam[i].SqlDbType = SqlDbType.BigInt;
                        sqlParam[i].Direction = ParameterDirection.InputOutput;
                    }

                    i++;

                }

                ht = dbCon.ExecuteNonQuery(프로시저명, sqlParam, CommandType.StoredProcedure, Output값);

                Output값 = GetHashTableKeyOutPut(ht, Output값).ToString();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbCon.DBCon_Close();
            }
        }


        #region 백업본 연결
        public DataSet 쿼리FillDataSet_bak(string 프로시저명, DataSet 파라미터, string 프로그램버전)
        {
            int i = 0;
            DBCon dbCon = null;
            DataSet ds = new DataSet();
            SqlParameter[] sqlParam = null;

            try
            {

                dbCon = new DBCon(DB연결문자("Tripboxbak", 프로그램버전));
                sqlParam = new SqlParameter[파라미터.Tables[0].Rows.Count];

                foreach (DataRow dr in 파라미터.Tables[0].Rows)
                {

                    sqlParam[i] = new SqlParameter();
                    sqlParam[i].ParameterName = dr["컬럼명"].ToString();

                    //sqlParam[i].Value = dr["값"].ToString();
                    if (dr["타입"].ToString() == string.Empty)
                    {
                        if (dr["값"].ToString().ToUpper() == "NULL")
                        {
                            sqlParam[i].Value = DBNull.Value;
                        }
                        else
                        {
                            sqlParam[i].Value = dr["값"].ToString();
                        }
                    }
                    else
                    {
                        sqlParam[i].SqlDbType = DB타입(dr["타입"].ToString());
                        sqlParam[i].Value = (DataTable)dr["리스트값"];
                    }

                    i++;

                }

                ds = dbCon.FillDataSet(프로시저명, sqlParam, CommandType.StoredProcedure);

                return ds;
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbCon.DBCon_Close();
            }
        }

        public void 쿼리ExecuteNonQuery_bak(bool 트랜잭션, string 프로시저명, DataSet 파라미터, string 프로그램버전)
        {
            int i = 0;
            SqlParameter[] sqlParam = null;

            if (트랜잭션 == false)
            {
                DBCon dbCon = null;

                try
                {

                    dbCon = new DBCon(DB연결문자("Tripboxbak", 프로그램버전));
                    sqlParam = new SqlParameter[파라미터.Tables[0].Rows.Count];

                    foreach (DataRow dr in 파라미터.Tables[0].Rows)
                    {

                        sqlParam[i] = new SqlParameter();
                        sqlParam[i].ParameterName = dr["컬럼명"].ToString();

                        if (dr["타입"].ToString() == string.Empty)
                        {
                            //sqlParam[i].Value = dr["값"].ToString();
                            if (dr["값"].ToString().ToUpper() == "NULL")
                            {
                                sqlParam[i].Value = DBNull.Value;
                            }
                            else
                            {
                                sqlParam[i].Value = dr["값"].ToString();
                            }
                        }
                        else
                        {
                            sqlParam[i].SqlDbType = DB타입(dr["타입"].ToString());
                            sqlParam[i].Value = (DataTable)dr["리스트값"];
                        }
                        i++;

                    }

                    dbCon.ExecuteNonQuery(프로시저명, sqlParam, CommandType.StoredProcedure);

                }
                catch (SqlException ex)
                {
                    throw ex;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    dbCon.DBCon_Close();
                }
            }

        }


        public void 쿼리ExecuteNonQueryOut_bak(string 프로시저명, DataSet 파라미터, string 프로그램버전, ref string Output값)
        {
            int i = 0;
            SqlParameter[] sqlParam = null;
            Hashtable ht = new Hashtable();

            DBCon dbCon = null;

            try
            {

                dbCon = new DBCon(DB연결문자("Tripboxbak", 프로그램버전));
                sqlParam = new SqlParameter[파라미터.Tables[0].Rows.Count];

                foreach (DataRow dr in 파라미터.Tables[0].Rows)
                {

                    sqlParam[i] = new SqlParameter();
                    sqlParam[i].ParameterName = dr["컬럼명"].ToString();

                    if (dr["타입"].ToString() == string.Empty)
                    {
                        if (dr["값"].ToString().ToUpper() == "NULL")
                        {
                            sqlParam[i].Value = DBNull.Value;
                        }
                        else
                        {
                            sqlParam[i].Value = dr["값"].ToString();
                        }
                    }
                    else
                    {
                        sqlParam[i].SqlDbType = DB타입(dr["타입"].ToString());
                        sqlParam[i].Value = (DataTable)dr["리스트값"];
                    }


                    if (Output값.Equals(dr["컬럼명"].ToString()))
                    {
                        sqlParam[i].SqlDbType = SqlDbType.BigInt;
                        sqlParam[i].Direction = ParameterDirection.InputOutput;
                    }

                    i++;

                }

                ht = dbCon.ExecuteNonQuery(프로시저명, sqlParam, CommandType.StoredProcedure, Output값);

                Output값 = GetHashTableKeyOutPut(ht, Output값).ToString();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                dbCon.DBCon_Close();
            }
        }

        #endregion

        public static object GetHashTableKeyOutPut(Hashtable myList, object Keys)
        {
            IDictionaryEnumerator myEnumerator = myList.GetEnumerator();

            while (myEnumerator.MoveNext())
            {
                if (myEnumerator.Key.ToString() == Keys.ToString().Trim())
                {
                    return myEnumerator.Value;
                }
            }
            return "";
        }
    }
}
