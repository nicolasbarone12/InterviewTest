namespace ApiInterviewTest.Contracts.Responses
{
    public class ErrorResponse:IResponse
    {
        public int StatusCode { get; set; }
        public string ErrorMessage { get; set; }
    }
}
