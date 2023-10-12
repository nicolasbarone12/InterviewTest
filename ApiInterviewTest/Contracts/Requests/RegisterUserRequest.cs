namespace ApiInterviewTest.Contracts.Requests
{
    public class RegisterUserRequest
    {
        public string UserLastName { get; set; }
        public string UserName { get; set; }

        public string UserCode { get; set; }
        public string Password { get; set; }
    }
}
