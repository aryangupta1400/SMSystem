using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SMSystem.Models
{
    public class CaptchaResponse
    {
        [JsonProperty("success")]
        public string Success { get; set; }



        [JsonProperty("error-codes")]
        public List<string> ErrorCodes { get; set; }
    }
}