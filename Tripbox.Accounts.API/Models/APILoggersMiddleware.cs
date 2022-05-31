using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Tripbox.Accounts.API.Models;

namespace Tripbox.Accounts.API.Models
{
    public class APILoggersMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<APILoggersMiddleware> _logger;

        public APILoggersMiddleware(RequestDelegate next, ILogger<APILoggersMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            
            var controller = context.Request.RouteValues["controller"];

            if (controller != null)
            {
                CommonAPILoggers _apiLoggers = new CommonAPILoggers();
                
                CommonAPILoggers.RequesetClass _apiRequest = new CommonAPILoggers.RequesetClass();
                CommonAPILoggers.HeadersClass _apiHeaders = new CommonAPILoggers.HeadersClass();
                CommonAPILoggers.ResponseClass _apiResponse = new CommonAPILoggers.ResponseClass();

                string requestTimeStamp = string.Empty;
                string responseTimeStamp = string.Empty;

                _apiRequest.URL = String.Format("{0}://{1}{2}", context.Request.Scheme.ToString(), context.Request.Host.ToString(), context.Request.Path.ToString());
                _apiRequest.Method = context.Request.Method != null ? context.Request.Method.ToString() : "";
                
                if (context.Request.Headers != null)
                {
                    _apiHeaders.Accept = context.Request.Headers != null ? context.Request.Headers["Accept"].ToString() : "";
                    _apiHeaders.Authorization = context.Request.Headers != null ? context.Request.Headers["Authorization"].ToString() : "";
                    _apiHeaders.Accept_Encoding = context.Request.Headers != null ? context.Request.Headers["Accept-Encoding"].ToString() : "";
                    _apiHeaders.Accept_Language = context.Request.Headers != null ? context.Request.Headers["Accept-Language"].ToString() : "";
                    _apiHeaders.Connection = context.Request.Headers != null ? context.Request.Headers["Connection"].ToString() : "";
                    _apiHeaders.Host = context.Request.Headers != null ? context.Request.Headers["Host"].ToString() : "";
                    _apiHeaders.Referer = context.Request.Headers != null ? context.Request.Headers["Referer"].ToString() : "";
                    _apiHeaders.User_Agent = context.Request.Headers != null ? context.Request.Headers["User-Agent"].ToString() : "";
                    _apiHeaders.sec_ch_ua = context.Request.Headers != null ? context.Request.Headers["sec-ch-ua"].ToString() : "";
                    _apiHeaders.X_Version = context.Request.Headers != null ? context.Request.Headers["X-Version"].ToString() : "";
                    _apiHeaders.sec_ch_ua_mobile = context.Request.Headers != null ? context.Request.Headers["sec-ch-ua-mobile"].ToString() : "";
                    _apiHeaders.sec_ch_ua_platform = context.Request.Headers != null ? context.Request.Headers["sec-ch-ua-platform"].ToString() : "";
                    _apiHeaders.Sec_Fetch_Site = context.Request.Headers != null ? context.Request.Headers["Sec-Fetch-Site"].ToString() : "";
                    _apiHeaders.Sec_Fetch_Mode = context.Request.Headers != null ? context.Request.Headers["Sec-Fetch-Mode"].ToString() : "";
                    _apiHeaders.Sec_Fetch_Dest = context.Request.Headers != null ? context.Request.Headers["Sec-Fetch-Dest"].ToString() : "";
                }
                _apiRequest.Headers = _apiHeaders;

                _apiRequest.Querys = context.Request.Query != null ? JsonConvert.SerializeObject(context.Request.Query).ToString() : "";

                //if (_apiRequest.Method.Equals("POST"))
                //{
                //    _apiRequest.Forms = context.Request.Form != null ? JsonConvert.SerializeObject(context.Request.Form.ToDictionary(x => x.Key, x => x.Value.ToString())) : "";
                //}
                
                _apiRequest.RequestDatetime = DateTime.Now;
                requestTimeStamp = Convert.ToString((int)DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);

                _apiLoggers.Requeset = _apiRequest;

                var existingBody = context.Response.Body;

                using (var newBody = new MemoryStream())
                {
                    context.Response.Body = newBody;

                    //await _next(context);

                    var newResponse = await FormatResponse(context.Response);

                    context.Response.Body = new MemoryStream();
                    newBody.Seek(0, SeekOrigin.Begin);
                    context.Response.Body = existingBody;

                    var newContent = new StreamReader(newBody).ReadToEnd();

                    if (newResponse != null)
                    {
                        JObject json = JObject.Parse(newResponse.ToString());

                        _apiResponse.StatusCode = Convert.ToInt32(json["StatusCode"]);
                        _apiResponse.Size = Convert.ToInt32(json["Size"]);
                        _apiResponse.ErrorMessage = json["ErrorMessage"].ToString();
                        //_apiResponse.Content = JsonConvert.SerializeObject(json["Content"].ToString());
                    }
                    else
                    {
                        _apiResponse.StatusCode = 400;
                        _apiResponse.Size = 0;
                        _apiResponse.ErrorMessage = "회신 오류";
                        _apiResponse.Content = null;
                    }

                    _apiResponse.ResponseDatetime = DateTime.Now;
                    responseTimeStamp = Convert.ToString((int)DateTime.Now.Subtract(new DateTime(1970, 1, 1)).TotalSeconds);
                    _apiLoggers.Response = _apiResponse;
                }

                _apiLoggers.ProcessTimestamp = Convert.ToString(Convert.ToDouble(responseTimeStamp) - Convert.ToDouble(requestTimeStamp));

                _logger.LogInformation(JsonConvert.SerializeObject(_apiLoggers).ToString());
            }

            await _next(context);
        }


        private async Task<string> FormatResponse(HttpResponse response)
        {
            //We need to read the response stream from the beginning...and copy it into a string...I'D LIKE TO SEE A BETTER WAY TO DO THIS
            response.Body.Seek(0, SeekOrigin.Begin);

            var content = await new StreamReader(response.Body).ReadToEndAsync();
            var apiResponse = new APIResponse(); // CREATE THIS CLASS

            apiResponse.StatusCode = response.StatusCode;

            if (!IsResponseValid(response))
            {
                apiResponse.ErrorMessage = content;
            }
            else
            {
                apiResponse.Content = content;
            }

            apiResponse.Size = response.ToString().Length;

            var json = JsonConvert.SerializeObject(apiResponse);

            //We need to reset the reader for the response so that the client an read it
            response.Body.Seek(0, SeekOrigin.Begin);
            
            return $"{json}";
        }

        private bool IsResponseValid(HttpResponse response)
        {
            if ((response != null)
                && (response.StatusCode == 200
                || response.StatusCode == 201
                || response.StatusCode == 202))
            {
                return true;
            }
            
            return false;
        }
    }


    public class APIResponse
    {
        public int StatusCode { get; set; }

        public string ErrorMessage { get; set; }

        public string Content { get; set; }

        public int Size { get; set; }
    }
}
