using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Tripbox.Accounts.API.Models.Auth;

namespace Tripbox.Accounts.API.Views.Auth
{
    public class IndexModel : PageModel
    {
        public string siteid { get; set; }
        public string authenticationtype { get; set; }
        public string callbackurl { get; set; }

        public string callbackurapiurl { get; set; }

        public bool _success = false;

        public void OnGet(Account account)
        {
            try
            {
                siteid = account.siteid;
                authenticationtype = account.authenticationtype;
                callbackurl = account.callbackurl;
                callbackurapiurl = account.callbackurapiurl;

                _success = true;
            }
            catch (Exception ex)
            {
                _success = false;
            }
        }

        public void OnPost(Account account)
        {
            try
            {
                siteid = account.siteid;
                authenticationtype = account.authenticationtype;
                callbackurl = account.callbackurl;
                callbackurapiurl = account.callbackurapiurl;

                _success = true;
            }
            catch (Exception ex)
            {
                _success = false;
            }
        }

    }
}
