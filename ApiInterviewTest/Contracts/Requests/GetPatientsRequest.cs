namespace ApiInterviewTest.Contracts.Requests
{
    public class GetPatientsRequest
    {
        public string? Name { get; set; }
        public string? LastName { get; set; }
        public DateTime? DateOfBirth { get; set; }

        
        public string? Sickness { get; set; }
    }
}
