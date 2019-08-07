namespace Infrastructure.Common.Web.Metrics
{
    public class MetricsOptions
    {
        public bool Enabled { get; set; }
        public string InfluxUrl { get; set; }
        public string Database { get; set; }
        public int Interval { get; set; }
    }
}