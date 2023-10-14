using Newtonsoft.Json;

namespace ApiInterviewTest.Contracts.Requests
{
    public class UpdatePatientRequest:PatientRequestBase
    {
        [JsonProperty("Id")]
        public int PatientId { get; set; }
    }
}
