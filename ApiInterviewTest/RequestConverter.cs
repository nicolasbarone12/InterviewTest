using ApiInterviewTest.Contracts.Requests;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ApiInterviewTest
{
    public class RequestConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return typeof(PatientRequestBase).IsAssignableFrom(objectType);
        }

        public override object ReadJson(JsonReader reader,
            Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jo = JObject.Load(reader);

            PatientRequestBase request = null;
            
            if (jo.ContainsKey("Id"))
                request = new UpdatePatientRequest();
            else
                request = new NewPatientRequest();

            serializer.Populate(jo.CreateReader(), request);

            return request;
        }

        public override bool CanWrite
        {
            get { return false; }
        }

        public override void WriteJson(JsonWriter writer,
            object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
