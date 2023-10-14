using Services.Models;

namespace ApiInterviewTest.Contracts.Responses
{
    public class PatientResponse:IResponse
    {
        public ICollection<Patient> Patients { get; set; }
    }
}
