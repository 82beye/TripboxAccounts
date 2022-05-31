using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Tripbox.Accounts.API.Models
{
    public class CommonAPILoggers
    {
        public RequesetClass Requeset { get; set; }

        public ResponseClass Response { get; set; }

        public string ProcessTimestamp { get; set; }

        public class RequesetClass
        {
            public string URL { get; set; }

            public string Method { get; set; }

            public HeadersClass Headers { get; set; }

            public string Forms { get; set; }

            public string Querys { get; set; }
            
            public DateTime RequestDatetime { get; set; }
        }

        public class HeadersClass
        {
          
            public string Accept { get; set; }

            public string Authorization { get; set; }

            public string Accept_Encoding { get; set; }

            public string Accept_Language { get; set; }

            public string Connection { get; set; }

            public string Host { get; set; }

            public string Referer { get; set; }

            public string User_Agent { get; set; }

            public string sec_ch_ua { get; set; }

            public string X_Version { get; set; }

            public string sec_ch_ua_mobile { get; set; }

            public string sec_ch_ua_platform { get; set; }

            public string Sec_Fetch_Site { get; set; }

            public string Sec_Fetch_Mode { get; set; }

            public string Sec_Fetch_Dest { get; set; }
        }

        public class ResponseClass
        {
            public int StatusCode { get; set; }

            public string ErrorMessage { get; set; }

            public string Content { get; set; }

            public int Size { get; set; }

            public DateTime ResponseDatetime { get; set; }
        }
    }
}
