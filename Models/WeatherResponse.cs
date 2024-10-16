namespace Weather.Models
{
    public class WeatherResponse
    {
        public Data Data { get; set; }
    }

    public class Data
    {
        public List<Timeline> Timelines { get; set; }
    }

    public class Timeline
    {
        public List<Interval> Intervals { get; set; }
        public string Timestep { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }

    public class Interval
    {
        public DateTime StartTime { get; set; }
        public Values Values { get; set; }
    }

    public class Values
    {
        public float Temperature { get; set; }
        public int WeatherCode { get; set; } 
    }

}
