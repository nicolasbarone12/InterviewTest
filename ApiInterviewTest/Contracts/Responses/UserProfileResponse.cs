namespace ApiInterviewTest.Contracts.Responses
{
    internal class UserProfileResponse:IResponse
    {
        public string UserName { get; set; }
        public string LastName { get; set; }
        public string UserCode { get; set; }
    }
}
