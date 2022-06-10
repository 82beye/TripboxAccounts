using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text.Json;
using Tripbox.Accounts.API.Extensions;
using Tripbox.Accounts.API.Models;
using System.Threading.Tasks;
using Tripbox.Accounts.API.Models.Auth;
using Microsoft.AspNetCore.Http.Extensions;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.WebUtilities;

namespace Tripbox.Accounts.API.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [ApiVersion("1")]
    public class AuthController : Controller
    {
        private readonly ILogger<AuthController> _logger;

        public AuthController(ILogger<AuthController> logger)
        {
            _logger = logger;
        }


        [HttpGet("~/signin")]
        public async Task<IActionResult> SignIn() 
        {
            var schemes = await HttpContext.GetExternalProvidersAsync();
            return View("SignIn", schemes);
        }


        [HttpPost("~/signin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [MapToApiVersion("1")]
        public async Task<IActionResult> SignIn([FromForm] Account account)
        {
            if (string.IsNullOrWhiteSpace(account.authenticationtype))
            {
                return BadRequest();
            }

            if (string.IsNullOrWhiteSpace(account.siteid))
            {
                return BadRequest();
            }

            var schemes = await HttpContext.GetExternalProvidersAsync();

            if (schemes == null)
            {
                return BadRequest();
            }

            string provider = string.Empty;

            if (account.authenticationtype.Equals("304001"))
            {
                provider = "Google";
            }
            else if (account.authenticationtype.Equals("304002"))
            {
                provider = "Naver";
            }
            else if (account.authenticationtype.Equals("304003"))
            {
                provider = "KakaoTalk";
            }

            return Challenge(new AuthenticationProperties { 
                            RedirectUri = string.Format("/callback/{0}", provider.ToLower().ToString()),
                            Items = { 
                                { "siteid", account.siteid },
                                { "authenticationtype", account.authenticationtype },
                                { "callbackurl", account.callbackurl },
                                { "callbackapiurl", account.callbackapiurl }
                            },
                        }, provider);
        }


        [HttpGet("~/signout")]
        [HttpPost("~/signout")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [MapToApiVersion("1")]
        public IActionResult SignOutCurrentUser()
        {
            return SignOut(new AuthenticationProperties { RedirectUri = "/" },
                CookieAuthenticationDefaults.AuthenticationScheme);
        }


        [HttpGet("~/callback/{provider}")]
        [HttpPost("~/callback/{provider}")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [MapToApiVersion("1")]
        public async Task<IActionResult> CallBack()
        {
            var accessToken = await HttpContext.GetTokenAsync("access_token");

            Auth authModel = new Auth();
            Account account = new Account();

            if (User?.Identity?.IsAuthenticated ?? false)
            {
                foreach (var claim in HttpContext.User.Claims)
                {
                    authModel.AuAuthenticationType = !string.IsNullOrEmpty(claim.Subject.AuthenticationType) ? claim.Subject.AuthenticationType : "";
                    authModel.NameIdentifier = claim.Type.IndexOf("/nameidentifier") > -1 ? claim.Value : authModel.NameIdentifier;
                    authModel.Name = claim.Type.IndexOf("/name") > -1 ? claim.Value : authModel.Name;
                    authModel.Email = claim.Type.IndexOf("/email") > -1 ? claim.Value : authModel.Email;
                }
            }
            
            var authResult = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            account.siteid = authResult.Properties.Items["siteid"].ToString();
            account.authenticationtype = authResult.Properties.Items["authenticationtype"].ToString();
            account.callbackurl = authResult.Properties.Items["callbackurl"].ToString();
            account.callbackapiurl = authResult.Properties.Items["callbackapiurl"].ToString();

            if (authModel == null || account == null)
            {
                return Unauthorized();
            }
            else
            {
                var absoluteUri = string.Concat(account.callbackapiurl);

                var json = JsonConvert.SerializeObject(authModel).ToString();
                var values = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(json);

                var json2 = JsonConvert.SerializeObject(account).ToString();
                var values2 = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(json2);

                Dictionary<String, String> allTables = new Dictionary<String, String>();
                allTables = values.Union(values2).ToDictionary(pair => pair.Key, pair => pair.Value);

                var url = QueryHelpers.AddQueryString(absoluteUri, allTables);

                return Redirect(url);
            }
        }

        [HttpPost("~/Account")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [MapToApiVersion("1")]
        public async Task<IActionResult> Account([FromForm] Account account)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            ViewData["Account"] = account;

            if (account.authenticationtype != null)
            {
                return View("Account", account);
            } 
            else
            {
                return BadRequest();
            }
            
        }

    }
}

