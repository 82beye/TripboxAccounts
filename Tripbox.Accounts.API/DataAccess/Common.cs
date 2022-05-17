using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Tripbox.Accounts.API.DataAccess
{
    class 쿼리파라미터
    {
        public DataTable dt = null;

        public 쿼리파라미터()
        {

            dt = new DataTable();
            dt.Columns.AddRange(new DataColumn[] { new DataColumn("컬럼명"),
                                                   new DataColumn("타입"),
                                                   new DataColumn("값"),
                                                   new DataColumn("리스트값")
            }
            );

            dt.Columns["리스트값"].DataType = Type.GetType("System.Object");
        }

        public void ADD(string 파라미터명, string 값)
        {
            dt.Rows.Add(파라미터명, "", 값, null);
        }

        public void ADD(string 파라미터명, string 타입, int 사이즈, object 값)
        {
            dt.Rows.Add(파라미터명, 타입, null, 값);
            //dt.Rows.Add(파라미터명,  null, 값);
        }

        public DataSet 정보()
        {
            DataSet ds = new DataSet();

            ds.Tables.Add(dt);

            return ds;
        }
    }

    class 버전정보
    {
        private static string vVer = "";

        public static string 버전
        {
            get { return vVer; }
            set { vVer = value; }
        }
    }

    class 쿼리타입
    {
        private static string vBigInt = "SqlDbType.BigInt";
        private static string vBinary = "SqlDbType.Binary";
        private static string vVarBinary = "SqlDbType.VarBinary";
        private static string vBit = "SqlDbType.Bit";
        private static string vChar = "SqlDbType.Char";
        private static string vDate = "SqlDbType.Date";
        private static string vDateTime = "SqlDbType.DateTime";
        private static string vDateTime2 = "SqlDbType.DateTime2";
        private static string vDecimal = "SqlDbType.Decimal";
        private static string vFloat = "SqlDbType.Float";
        private static string vImage = "SqlDbType.Image";
        private static string vInt = "SqlDbType.Int";
        private static string vMoney = "SqlDbType.Money";
        private static string vNChar = "SqlD bType.NChar";
        private static string vNText = "SqlDbType.NText";
        private static string vNVarChar = "SqlDbType.NVarChar";
        private static string vReal = "SqlDbType.Real";
        private static string vVarChar = "SqlDbType.VarChar";
        private static string vTinyInt = "SqlDbType.TinyInt";
        private static string vText = "SqlDbType.Text";
        private static string vStructured = "SqlDbType.Structured";
        private static string vXml = "SqlDbType.Xml";

        public static string Structured
        {
            get { return vStructured; }
        }
        public static string Image
        {
            get { return vImage; }
        }
        public static string Int
        {
            get { return vInt; }
        }
        public static string Money
        {
            get { return vMoney; }
        }
        public static string NChar
        {
            get { return vNChar; }
        }

        public static string NText
        {
            get { return vNText; }
        }
        public static string NVarChar
        {
            get { return vNVarChar; }
        }
        public static string Real
        {
            get { return vReal; }
        }
        public static string VarChar
        {
            get { return vVarChar; }
        }

        public static string DateTime2
        {
            get { return vDateTime2; }
        }

        public static string Decimal
        {
            get { return vDecimal; }
        }

        public static string Float
        {
            get { return vFloat; }
        }

        public static string BigInt
        {
            get { return vBigInt; }
        }

        public static string Binary
        {
            get { return vBinary; }
        }

        public static string VarBinary
        {
            get { return vVarBinary; }
        }

        public static string Bit
        {
            get { return vBit; }
        }

        public static string Char
        {
            get { return vChar; }
        }

        public static string Date
        {
            get { return vDate; }
        }

        public static string DateTime
        {
            get { return vDateTime; }
        }

        public static string TinyInt
        {
            get { return vTinyInt; }
        }

        public static string Text
        {
            get { return vText; }
        }

        public static string Xml
        {
            get { return vXml; }
        }
    }
}
