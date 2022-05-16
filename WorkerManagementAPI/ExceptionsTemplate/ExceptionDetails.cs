using Newtonsoft.Json;

namespace WorkerManagementAPI.ExceptionsTemplate
{
    public class ExceptionDetails
    {
        public string Message { get; set; }
        public int Status { get; set; }
        public DateTime DateTime { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
