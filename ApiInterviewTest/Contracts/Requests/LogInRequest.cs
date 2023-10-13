﻿

using Newtonsoft.Json;

namespace ApiInterviewTest.Contracts.Requests
{
    public class LogInRequest
    {
        [JsonRequired]        
        public string UserCode { get; set; }
        
        [JsonRequired]
        public string Password { get; set; }
    }
}
