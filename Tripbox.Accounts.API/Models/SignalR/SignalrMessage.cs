using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tripbox.Accounts.API.SignalR
{
    /// <summary>
    /// 시그날R 메세지 정보
    /// </summary>
    public class SignalrMessage
    {
        /// <summary>
        /// 태그
        /// </summary>
        public string tag { get; set; }

        /// <summary>
        /// 받는 사람 유저번호 
        /// </summary>
        public string to_user_no { get; set; }


        /// <summary>
        /// 받는 사람 유저번호
        /// </summary>
        public string from_user_no { get; set; }

        
        /// <summary>
        /// 메세지내용
        /// </summary>
        public string message_text { get; set; }

    }

    /// <summary>
    /// </summary>
    public class SignalrMessage2
    {
       
        /// <summary>
        /// 메세지내용
        /// </summary>
        public string message_json { get; set; }

    }
}
