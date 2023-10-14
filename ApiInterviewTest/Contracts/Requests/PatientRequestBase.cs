

using Newtonsoft.Json;

namespace ApiInterviewTest.Contracts.Requests
{
    [JsonConverter(typeof(RequestConverter))]
    public abstract class PatientRequestBase
    {
        
        public string Name { get; set; }
        public string LastName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string Sickness { get; set; }
    }
}
