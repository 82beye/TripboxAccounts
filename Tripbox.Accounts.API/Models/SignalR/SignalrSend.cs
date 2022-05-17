using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Tripbox.Accounts.API.Models.SignalR
{
    /// <summary>
    /// 시그날r 메세지 전송 ...
    /// </summary>
    public class SignalrSend
    {
        [DefaultValue("")]
        public string 메세지번호 { get; set; }

        [DefaultValue("")]
        public string 그룹구분 { get; set; }

        [DefaultValue("")]
        public string 참조구분 { get; set; }

        [DefaultValue("")]
        public string 참조번호 { get; set; }

        [DefaultValue("")]
        public string 발송자구분 { get; set; }

        [DefaultValue("")]
        public string 발송자번호 { get; set; }

        [DefaultValue("")]
        public string 발송자주소 { get; set; }

        [DefaultValue("")]
        public string 내용참조번호 { get; set; }

        [DefaultValue("")]
        public string 제목 { get; set; }

        [DefaultValue("")]
        public string 내용 { get; set; }

        [DefaultValue("")]
        public string 메세지발송번호 { get; set; }

        [DefaultValue("")]
        public string 전송수단코드 { get; set; }

        [DefaultValue("")]
        public string 발송일시 { get; set; }

        [DefaultValue("")]
        public string 수신자구분 { get; set; }

        [DefaultValue("")]
        public string 수신자번호 { get; set; }

        [DefaultValue("")]
        public string 수신자주소 { get; set; }

        [DefaultValue("")]
        public string 수신일시 { get; set; }
        
        public bool 알림여부 { get; set; }

        [DefaultValue("")]
        public string 발송일시2 { get; set; }
    }
}
