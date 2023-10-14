

using Newtonsoft.Json;

namespace ApiInterviewTest.Contracts.Requests
{
    public class RegisterUserRequest: IUserRequest
    {

        [JsonRequired]        
        public string UserLastName { get; set; }

        [JsonRequired]
        public string UserName { get; set; }

        [JsonRequired]
        public string UserCode { get; set; }

        [JsonRequired]
        public string Password { get; set; }
    }
}
