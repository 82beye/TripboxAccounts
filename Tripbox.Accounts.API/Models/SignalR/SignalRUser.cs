using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Tripbox.Accounts.API.Models.SignalR
{
    /// <summary>
    /// 시그날r 사용자
    /// </summary>
    public class SignalRUser
    {
        /// <summary>
        /// 클라이언트 ID
        /// </summary>
        [Key]
        public string clientId { get; set; }

        /// <summary>
        /// 유저번호
        /// </summary>
        public long userno { get; set; }

        /// <summary>
        /// 경로
        /// </summary>
        public string path { get; set; }

        /// <summary>
        /// 경로
        /// </summary>
        public string serverip { get; set; }
    }
}
