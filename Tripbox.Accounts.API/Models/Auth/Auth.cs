namespace Tripbox.Accounts.API.Models.Auth
{
    public class Auth
    {

            /// <summary>
            /// Provider
            /// </summary>

            public string AuAuthenticationType { get; set; }

            /// <summary>
            /// Provider 식별자
            /// </summary>
            public string NameIdentifier { get; set; }

            /// <summary>
            /// 이름
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// 이메일
            /// </summary>
            public string Email { get; set; }


    }
}
