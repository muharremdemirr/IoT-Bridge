namespace RestAPI.Models
{
    public class SensorData
    {
        public int Id { get; set; }
        public string DeviceId { get; set; }
        public string Topic { get; set; }
        public string Payload { get; set; } //
        public DateTime TimeStamp { get; set; }
    }
}
