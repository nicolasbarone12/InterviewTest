﻿namespace ApiInterviewTest.Contracts.Responses
{
    public class AuthResponse
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public long expires_in { get; set; }
    }
}